using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientResolutionManager : MonoBehaviour
{
	private enum ClientResolutionManagerState
	{
		WaitingForActionMsgs,
		Resolving,
		Idle
	}

	private enum MessageHandlersState
	{
		NotYetRegistered,
		Registered,
		Unregistered
	}

	public const float c_individualHitFailsafeTime = 7f;

	private static ClientResolutionManager s_instance;

	private int m_currentTurnIndex;
	private AbilityPriority m_currentAbilityPhase;
	private int m_numResolutionActionsThisPhase;
	private List<ClientResolutionAction> m_resolutionActions;
	private List<ClientResolutionAction> m_movementActions;
	private List<ClientCastAction> m_castActions;
	private List<ClientResolutionActionMessageData> m_receivedMessages;
	private ClientResolutionManagerState m_state = ClientResolutionManagerState.Idle;
	private List<ActorData> m_actorsToKillOnLastHitExecution;
	private float m_timeOfLastEvent;
	private bool m_waitingForAllMessages;
	private MessageHandlersState m_currentMessageHandlersState;
	private MessageHandlersState m_expectedMessageHandlersState;
	private List<ActorData> m_tempCombatPhaseDataReceivedActors = new List<ActorData>();

	public static ClientResolutionManager Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		m_currentMessageHandlersState = MessageHandlersState.NotYetRegistered;
		m_expectedMessageHandlersState = MessageHandlersState.NotYetRegistered;
		RegisterHandler();
		m_expectedMessageHandlersState = MessageHandlersState.Registered;
		m_resolutionActions = new List<ClientResolutionAction>();
		m_movementActions = new List<ClientResolutionAction>();
		m_castActions = new List<ClientCastAction>();
		m_receivedMessages = new List<ClientResolutionActionMessageData>();
		m_actorsToKillOnLastHitExecution = new List<ActorData>();
	}

	private void OnDestroy()
	{
		UnregisterHandlers();
		m_expectedMessageHandlersState = MessageHandlersState.Unregistered;
		s_instance = null;
	}

	private void VerifyMessageHandlerState()
	{
		if (m_currentMessageHandlersState == m_expectedMessageHandlersState)
		{
			return;
		}
		if (m_expectedMessageHandlersState == MessageHandlersState.Registered)
		{
			if (m_currentMessageHandlersState == MessageHandlersState.NotYetRegistered)
			{
				Debug.LogError("ClientResolutionManager believes message handlers should be registered, but they aren't yet, as of Update() being called.  Registering them...");
				RegisterHandler();
			}
			else if (m_currentMessageHandlersState == MessageHandlersState.Unregistered)
			{
				Debug.LogError("ClientResolutionManager believes message handlers should be registered, but they were already unregistered.");
				m_expectedMessageHandlersState = MessageHandlersState.Unregistered;
			}
		}
		else if (m_expectedMessageHandlersState == MessageHandlersState.Unregistered)
		{
			if (m_currentMessageHandlersState == MessageHandlersState.Registered)
			{
				Debug.LogError("ClientResolutionManager believes message handlers should be unregistered, but they are still registered, as of Update() being called.  Unregistering them...");
				UnregisterHandlers();
			}
			else if (m_currentMessageHandlersState == MessageHandlersState.NotYetRegistered)
			{
				Debug.LogError("ClientResolutionManager believes message handlers should be unregistered, but they never registered in the first place.");
				m_currentMessageHandlersState = MessageHandlersState.Unregistered;
			}
		}
	}

	private void RegisterHandler()
	{
		if (m_currentMessageHandlersState == MessageHandlersState.NotYetRegistered
			&& ClientGameManager.Get() != null
			&& ClientGameManager.Get().Client != null)
		{
			ClientGameManager.Get().Client.RegisterHandler((int)MyMsgType.StartResolutionPhase, MsgStartResolutionPhase);
			ClientGameManager.Get().Client.RegisterHandler((int)MyMsgType.SingleResolutionAction, MsgSingleResolutionAction);
			ClientGameManager.Get().Client.RegisterHandler((int)MyMsgType.RunResolutionActionsOutsideResolve, MsgRunResolutionActionsOutsideResolve);
			ClientGameManager.Get().Client.RegisterHandler((int)MyMsgType.Failsafe_HurryResolutionPhase, MsgFailsafeHurryResolutionPhase);
			m_currentMessageHandlersState = MessageHandlersState.Registered;
		}
	}

	public void UnregisterHandlers()
	{
		if (m_currentMessageHandlersState == MessageHandlersState.Registered
			&& ClientGameManager.Get() != null
			&& ClientGameManager.Get().Client != null)
		{
			ClientGameManager.Get().Client.UnregisterHandler((int)MyMsgType.StartResolutionPhase);
			ClientGameManager.Get().Client.UnregisterHandler((int)MyMsgType.SingleResolutionAction);
			ClientGameManager.Get().Client.UnregisterHandler((int)MyMsgType.RunResolutionActionsOutsideResolve);
			ClientGameManager.Get().Client.UnregisterHandler((int)MyMsgType.Failsafe_HurryResolutionPhase);
			m_currentMessageHandlersState = MessageHandlersState.Unregistered;
		}
	}

	public void OnTurnStart()
	{
		if (m_state == ClientResolutionManagerState.Idle)
		{
			m_currentAbilityPhase = AbilityPriority.INVALID;
		}
	}

	public void OnAbilityPhaseStart(AbilityPriority phase)
	{
		m_waitingForAllMessages = false;
		if (phase != m_currentAbilityPhase
			&& m_state != ClientResolutionManagerState.Resolving
			&& m_state != ClientResolutionManagerState.WaitingForActionMsgs)
		{
			m_waitingForAllMessages = true;
		}
	}

	internal void MsgStartResolutionPhase(NetworkMessage netMsg)
	{
		NetworkReader reader = netMsg.reader;
		m_currentTurnIndex = reader.ReadInt32();
		m_currentAbilityPhase = (AbilityPriority)reader.ReadSByte();
		m_numResolutionActionsThisPhase = reader.ReadSByte();
		m_castActions = new List<ClientCastAction>();
		m_timeOfLastEvent = GameTime.time;
		if (m_state != ClientResolutionManagerState.Idle)
		{
			Debug.LogError($"Received StartResolutionPhase message for turn {m_currentTurnIndex}, " +
				$"phase {m_currentAbilityPhase}, but ClientResolutionManager's state is {m_state}!");
		}
		m_waitingForAllMessages = false;
		m_state = ClientResolutionManagerState.WaitingForActionMsgs;
		ProcessReceivedMessages();
	}

	public List<ClientCastAction> DeSerializeCastActionsFromServer(ref NetworkReader reader)
	{
		sbyte num = reader.ReadSByte();
		List<ClientCastAction> list = new List<ClientCastAction>(num);
		for (int i = 0; i < num; i++)
		{
			ClientCastAction item = ClientCastAction.ClientCastAction_DeSerializeFromReader(ref reader);
			list.Add(item);
		}
		return list;
	}

	private void MsgSingleResolutionAction(NetworkMessage netMsg)
	{
		NetworkReader reader = netMsg.reader;
		uint turnIndex = reader.ReadPackedUInt32();
		sbyte phaseIndex = reader.ReadSByte();
		Log.Info($"MsgSingleResolutionAction: turn {turnIndex}, phase {(AbilityPriority)phaseIndex}");
		IBitStream stream = new NetworkReaderAdapter(reader);
		ClientResolutionAction action = ClientResolutionAction.ClientResolutionAction_DeSerializeFromStream(ref stream);
		ClientResolutionActionMessageData item = new ClientResolutionActionMessageData(action, (int)turnIndex, phaseIndex);
		m_receivedMessages.Add(item);
		UpdateLastEventTime();
		ProcessReceivedMessages();

		Log.Info($"[JSON] {{\"msgSingleResolutionAction\":{{\"turn\":{turnIndex},\"phase\":{DefaultJsonSerializer.Serialize((AbilityPriority)b)},\"action\":{action.json()}}}}}");
	}

	private void ProcessReceivedMessages()
	{
		if (m_state != ClientResolutionManagerState.WaitingForActionMsgs)
		{
			return;
		}
		List<ClientResolutionActionMessageData> messages = new List<ClientResolutionActionMessageData>();
		foreach (ClientResolutionActionMessageData msg in m_receivedMessages)
		{
			if (msg.m_phase == m_currentAbilityPhase && msg.m_turnIndex == m_currentTurnIndex)
			{
				messages.Add(msg);
			}
		}
		if (messages.Count < m_numResolutionActionsThisPhase)
		{
			return;
		}
		if (messages.Count > m_numResolutionActionsThisPhase)
		{
			Debug.LogError($"Somehow got more matching ClientResolutionActionMessageData messages ({messages.Count}) than expected ({m_numResolutionActionsThisPhase}) " +
				$"for this turn ({m_currentTurnIndex}) / phase ({m_currentAbilityPhase}).");
		}
		m_resolutionActions.Clear();
		m_movementActions.Clear();
		foreach (ClientResolutionActionMessageData msg in messages)
		{
			m_resolutionActions.Add(msg.m_action);
			if (msg.m_action.ReactsToMovement())
			{
				m_movementActions.Add(msg.m_action);
			}
			m_receivedMessages.Remove(msg);
		}
		m_movementActions.Sort();
		if (m_receivedMessages.Count > 0)
		{
			Debug.LogError($"Received last resolution action for turn {m_currentTurnIndex} / phase {m_currentAbilityPhase}, " +
				$"but there are still {m_receivedMessages.Count} received message(s) left over!  Clearing...");
			m_receivedMessages.Clear();
		}
		OnReceivedLastResolutionAction();
	}

	private void OnReceivedLastResolutionAction()
	{
		m_timeOfLastEvent = GameTime.time;
		if (TheatricsManager.Get() != null
			&& TheatricsManager.Get().GetPhaseToUpdate() == AbilityPriority.Combat_Damage)
		{
			OnCombatPhasePlayDataReceived();
		}
		if (m_currentAbilityPhase == AbilityPriority.Combat_Knockback)
		{
			ClientKnockbackManager.Get().InitKnockbacksFromActions(m_resolutionActions);
		}
		if (ClientAbilityResults.DebugTraceOn || ClientAbilityResults.DebugSerializeSizeOn)
		{
			Log.Warning(ClientAbilityResults.s_clientResolutionNetMsgHeader +
				$"<color=white>OnReceivedLastResolutionAction</color> received for phase {m_currentAbilityPhase}.  {GetActionsDoneExecutingDebugStr()}");
		}
		m_waitingForAllMessages = false;
		m_state = ClientResolutionManagerState.Resolving;
		if (ClientGameManager.Get() != null && ClientGameManager.Get().IsFastForward)
		{
			ExecuteAllUnexecutedActions(false);
			SendResolutionPhaseCompleted(m_currentAbilityPhase, true, false);
		}
		else
		{
			foreach (ClientResolutionAction action in m_resolutionActions)
			{
				if (!action.ReactsToMovement())
				{
					action.RunStartSequences();
				}
			}
			if (m_currentAbilityPhase == AbilityPriority.INVALID)
			{
				GameEventManager.NormalMovementStartAgs normalMovementStartAgs = new GameEventManager.NormalMovementStartAgs();
				normalMovementStartAgs.m_actorsBeingHitMidMovement = GetActorsWithMovementHits();
				GameEventManager.Get().FireEvent(GameEventManager.EventType.NormalMovementStart, normalMovementStartAgs);
			}
		}
	}

	internal void OnCombatPhasePlayDataReceived()
	{
		m_tempCombatPhaseDataReceivedActors.Clear();
		foreach (ClientResolutionAction action in m_resolutionActions)
		{
			ActorData caster = action.GetCaster();
			if (caster != null
				&& caster.GetAbilityData() != null
				&& !m_tempCombatPhaseDataReceivedActors.Contains(caster))
			{
				caster.GetAbilityData().OnClientCombatPhasePlayDataReceived(m_resolutionActions);
				m_tempCombatPhaseDataReceivedActors.Add(caster);
			}
		}
		m_tempCombatPhaseDataReceivedActors.Clear();
	}

	internal void MsgRunResolutionActionsOutsideResolve(NetworkMessage netMsg)
	{
		NetworkReader reader = netMsg.reader;
		sbyte num = reader.ReadSByte();
		IBitStream stream = new NetworkReaderAdapter(reader);
		List<ClientResolutionAction> list = new List<ClientResolutionAction>(num);
		for (int i = 0; i < num; i++)
		{
			ClientResolutionAction item = ClientResolutionAction.ClientResolutionAction_DeSerializeFromStream(ref stream);
			list.Add(item);
			jsonLog.Add(item.json());
		}
		Log.Info($"[JSON] {{\"msgRunResolutionActionsOutsideResolve\":{{\"numActions\":{b},\"actions\":[{System.String.Join(",", jsonLog.ToArray())}]}}}}");
		foreach (ClientResolutionAction current in list)
		{
			current.Run_OutsideResolution();
		}
	}

	internal void MsgFailsafeHurryResolutionPhase(NetworkMessage netMsg)
	{
		NetworkReader reader = netMsg.reader;
		int turnIndex = reader.ReadInt32();
		sbyte abilityPhase = reader.ReadSByte();
		AbilityPriority abilityPriority = (AbilityPriority)abilityPhase;
		sbyte num = reader.ReadSByte();
		List<ActorData> actors = new List<ActorData>();
		string text = "Actors not done resolving:";
		if (GameFlowData.Get() == null)
		{
			Log.Warning("Server sent 'hurry' failsafe to clients for turn = {0}, phase = {1}, and we're on turn = {2}, phase = {3}. But GameFlowData is null. Doing nothing... ({4})", turnIndex, abilityPriority, m_currentTurnIndex, m_currentAbilityPhase, netMsg.conn);
			return;
		}
		for (int i = 0; i < num; i++)
		{
			int actorIndex = reader.ReadInt32();
			if (actorIndex != ActorData.s_invalidActorIndex)
			{
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
				if (actorData != null)
				{
					actors.Add(actorData);
					text = text + "\n\t" + actorData.DebugNameString();
				}
			}
		}
		bool isOwnedActorData = false;
		foreach (ActorData actor in actors)
		{
			if (GameFlowData.Get().IsActorDataOwned(actor))
			{
				isOwnedActorData = true;
				break;
			}
		}
		if (!isOwnedActorData)
		{
			Debug.LogWarning("Server sent 'hurry' failsafe to clients for turn = " + turnIndex + ", phase = " + abilityPriority.ToString() + ", but we're not one of the actors-still-resolving." + text + "\n(This client = " + GameFlowData.Get().GetActiveOwnedActorDataDebugNameString() + ".)\nIgnoring failsafe.");
		}
		else if (turnIndex == m_currentTurnIndex && abilityPriority == m_currentAbilityPhase)
		{
			if (m_state == ClientResolutionManagerState.Idle)
			{
				Debug.Log("Server sent 'hurry' failsafe to clients for turn = " + turnIndex + ", phase = " + abilityPriority.ToString() + ", and we ARE included in the list of actors-still-resolving; but our state is Idle, so it doesn't apply to us.\n" + text + "\n(This client = " + GameFlowData.Get().GetActiveOwnedActorDataDebugNameString() + ".)\nIgnoring failsafe.");
			}
			else if (m_state == ClientResolutionManagerState.WaitingForActionMsgs)
			{
				Debug.LogWarning("Server sent 'hurry' failsafe to clients for turn = " + turnIndex + ", phase = " + abilityPriority.ToString() + ", and we ARE included in the list of actors-still-resolving; but our state is still in WaitingForActionMsgs, so that's very unexpected.\n" + text + "\n(This client = " + GameFlowData.Get().GetActiveOwnedActorDataDebugNameString() + ".)\nIgnoring failsafe... I guess...");
			}
			else if (m_state == ClientResolutionManagerState.Resolving)
			{
				Debug.LogWarning("Server sent 'hurry' failsafe to clients for turn = " + turnIndex + ", phase = " + abilityPriority.ToString() + ", and we ARE included in the list of actors-still-resolving; the failsafe applies to us.\n" + text + "\n(This client = " + GameFlowData.Get().GetActiveOwnedActorDataDebugNameString() + ".)\nExecuting failsafe...");
				ExecuteFailsafe();
			}
		}
		else if (m_currentTurnIndex <= turnIndex
			&& (m_currentTurnIndex != turnIndex || m_currentAbilityPhase <= abilityPriority))
		{
			Debug.LogWarning("Server sent 'hurry' failsafe to clients for turn = " + turnIndex + ", phase = " + abilityPriority.ToString() + ", but we're on turn = " + m_currentTurnIndex + ", phase = " + m_currentAbilityPhase.ToString() + ".\n\n(This client = " + GameFlowData.Get().GetActiveOwnedActorDataDebugNameString() + ".)\nThat's... in the future.  Ignoring failsafe...");
		}
		else
		{
			Debug.LogWarning("Server sent 'hurry' failsafe to clients for turn = " + turnIndex + ", phase = " + abilityPriority.ToString() + ", but we're on turn = " + m_currentTurnIndex + ", phase = " + m_currentAbilityPhase.ToString() + ".\n\n(This client = " + GameFlowData.Get().GetActiveOwnedActorDataDebugNameString() + ".)\nThat's in the past, unexpectedly.  Doing nothing...");
		}
	}

	internal bool HitsDoneExecuting()
	{
		foreach (ClientResolutionAction action in m_resolutionActions)
		{
			if (!action.CompletedAction())
			{
				return false;
			}
		}
		return true;
	}

	internal bool HitsDoneExecuting(SequenceSource sequenceSource)
	{
		foreach (ClientResolutionAction action in m_resolutionActions)
		{
			if (action.ContainsSequenceSource(sequenceSource))
			{
				return action.CompletedAction();
			}
		}
		return true;
	}

	internal bool HasUnexecutedHitsOnActor(ActorData targetActor, int sequenceSourceToIgnore = -1)
	{
		bool result = false;
		foreach (ClientResolutionAction action in m_resolutionActions)
		{
			if (result)
			{
				break;
			}
			if (sequenceSourceToIgnore < 0
				|| !action.ContainsSequenceSourceID((uint)sequenceSourceToIgnore))
			{
				result = action.HasUnexecutedHitOnActor(targetActor);
			}
		}
		return result;
	}

	internal void ExecuteUnexecutedActions(SequenceSource sequenceSource, string extraInfo)
	{
		foreach (ClientResolutionAction action in m_resolutionActions)
		{
			if (action.ContainsSequenceSource(sequenceSource) && !action.CompletedAction())
			{
				string message = "Executing Unexecuted Action: " + action.GetDebugDescription() + action.GetUnexecutedHitsDebugStr(true)
					+ SequenceManager.Get().GetSequenceHitsSeenDebugString(sequenceSource) + extraInfo;
				Log.Error(message);
				action.ExecuteUnexecutedClientHitsInAction();
			}
		}
	}

	internal void ExecuteAllUnexecutedActions(bool showAsError = true)
	{
		foreach (ClientResolutionAction action in m_resolutionActions)
		{
			if (!action.CompletedAction())
			{
				if (showAsError)
				{
					Log.Error("Executing Unexecuted Action: " + action.GetDebugDescription() + action.GetUnexecutedHitsDebugStr());
				}
				action.ExecuteUnexecutedClientHitsInAction();
			}
		}
	}

	internal string GetActionsDoneExecutingDebugStr()
	{
		string text = "";
		int num = 0;
		foreach (ClientResolutionAction current in m_resolutionActions)
		{
			if (!current.CompletedAction())
			{
				num++;
				text += "\n\t" + num + ". " + current.GetDebugDescription() + current.GetUnexecutedHitsDebugStr();
			}
		}
		return "Action not done: " + num + text;
	}

	private void Update()
	{
		VerifyMessageHandlerState();
		if (m_state == ClientResolutionManagerState.Resolving
			|| m_state == ClientResolutionManagerState.WaitingForActionMsgs)
		{
			bool isTimeForFailsafe = GameTime.time - m_timeOfLastEvent > 15f
				&& (GameFlowData.Get() == null || !GameFlowData.Get().IsResolutionPaused());
			if (HitsDoneExecuting() && m_state == ClientResolutionManagerState.Resolving
				|| isTimeForFailsafe)
			{
				if (isTimeForFailsafe)
				{
					ExecuteFailsafe();
				}
				else
				{
					SendResolutionPhaseCompleted(m_currentAbilityPhase, false, false);
				}
			}
		}
	}

	private void ExecuteFailsafe()
	{
		bool flag = true;
		string str = $"ClientResolutionManager sending phase completed message due to failsafe.  State = {m_state}.\n";
		if (m_currentAbilityPhase == AbilityPriority.INVALID)
		{
			str += "Phase = Normal Movement\n";
			flag = false;
		}
		else
		{
			str += "Phase = " + m_currentAbilityPhase.ToString() + "\n";
		}
		str += GetActionsDoneExecutingDebugStr();
		if (flag)
		{
			str = str + "\n" + TheatricsManager.Get().GetTheatricsStateString();
		}
		Debug.LogError(str);
		ExecuteAllUnexecutedActions();
		SendResolutionPhaseCompleted(m_currentAbilityPhase, true, false);
	}

	public void OnActorMoveStart_ClientResolutionManager(ActorData mover, BoardSquarePathInfo curPath)
	{
		if (curPath != null)
		{
			if (curPath.m_moverDiesHere)
			{
				OnActorWillDie(mover);
			}
			foreach (ClientResolutionAction action in m_movementActions)
			{
				action.OnActorMoved_ClientResolutionAction(mover, curPath);
			}
		}
	}

	public void OnActorMoved_ClientResolutionManager(ActorData mover, BoardSquarePathInfo curPath)
	{
		if (curPath.m_moverDiesHere)
		{
			OnActorWillDie(mover);
		}
		foreach (ClientResolutionAction movementAction in m_movementActions)
		{
			movementAction.OnActorMoved_ClientResolutionAction(mover, curPath);
		}
	}

	internal void SendResolutionPhaseCompleted(AbilityPriority abilityPhase, bool asFailsafe, bool asResend)
	{
		if (ClientAbilityResults.DebugTraceOn)
		{
			Log.Warning(ClientAbilityResults.s_clientResolutionNetMsgHeader + "<color=white>ClientResolutionPhaseCompleted</color> message sent for phase " + abilityPhase.ToString() + " (failsafe = " + asFailsafe + ").");
		}
		foreach (ActorData current in GameFlowData.Get().m_ownedActorDatas)
		{
			NetworkWriter networkWriter = new NetworkWriter();
			networkWriter.StartMessage((int)MyMsgType.ClientResolutionPhaseCompleted);
			networkWriter.Write((sbyte)abilityPhase);
			networkWriter.Write(current.ActorIndex);
			networkWriter.Write(asFailsafe);
			networkWriter.Write(asResend);
			networkWriter.FinishMessage();
			ClientGameManager.Get().Client.SendWriter(networkWriter, 0);
		}
		m_waitingForAllMessages = false;
		m_state = ClientResolutionManagerState.Idle;
	}

	internal void SendActorReadyToResolveKnockback(ActorData knockbackedTarget, ActorData sendingPlayer)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.StartMessage((int)MyMsgType.ResolveKnockbacksForActor);
		networkWriter.Write(knockbackedTarget.ActorIndex);
		networkWriter.Write(sendingPlayer.ActorIndex);
		networkWriter.FinishMessage();
		if (ClientAbilityResults.DebugTraceOn)
		{
			Log.Warning(ClientAbilityResults.s_clientResolutionNetMsgHeader + "Sending <color=white>ResolveKnockbackForActor</color>, Caster: " + sendingPlayer.DebugNameString() + ", KnockedBackActor: " + knockbackedTarget.DebugNameString());
		}
		ClientGameManager.Get().Client.SendWriter(networkWriter, 0);
	}

	public void UpdateLastEventTime()
	{
		m_timeOfLastEvent = GameTime.time;
	}

	public string GetCurrentStateName()
	{
		return m_state.ToString();
	}

	public bool IsInResolutionState()
	{
		return m_state == ClientResolutionManagerState.Resolving;
	}

	public bool IsWaitingForActionMessages(AbilityPriority phase)
	{
		return m_state == ClientResolutionManagerState.WaitingForActionMsgs
			|| m_waitingForAllMessages && m_state != ClientResolutionManagerState.Resolving;
	}

	public void OnAbilityCast(ActorData casterActor, Ability ability)
	{
	}

	public void OnActorWillDie(ActorData actor)
	{
		m_actorsToKillOnLastHitExecution.Add(actor);
	}

	public void OnHitExecutedOnActor(ActorData targetActor, ActorData caster, bool hasDamage, bool hasHealing, bool canBeReactedTo)
	{
		if (canBeReactedTo)
		{
			foreach (ClientResolutionAction action in m_resolutionActions)
			{
				action.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
			}
		}
		if (m_actorsToKillOnLastHitExecution.Contains(targetActor) && !HasUnexecutedHitsOnActor(targetActor))
		{
			m_actorsToKillOnLastHitExecution.Remove(targetActor);
			Vector3 position = targetActor.transform.position;
			Vector3 currentMovementDir = targetActor.GetActorMovement().GetCurrentMovementDir();
			ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, currentMovementDir);
			targetActor.DoVisualDeath(impulseInfo);
			targetActor.GetActorMovement().OnMidMovementDeath();
		}
	}

	public List<ActorData> GetActorsWithMovementHits()
	{
		List<ActorData> list = new List<ActorData>();
		foreach (ClientResolutionAction action in m_movementActions)
		{
			ActorData triggeringMovementActor = action.GetTriggeringMovementActor();
			if (triggeringMovementActor != null && !list.Contains(triggeringMovementActor))
			{
				list.Add(triggeringMovementActor);
			}
		}
		return list;
	}
}
