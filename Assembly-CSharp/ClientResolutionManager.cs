using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientResolutionManager : MonoBehaviour
{
	public const float c_individualHitFailsafeTime = 7f;

	private static ClientResolutionManager s_instance;

	private int m_currentTurnIndex;

	private AbilityPriority m_currentAbilityPhase;

	private int m_numResolutionActionsThisPhase;

	private List<ClientResolutionAction> m_resolutionActions;

	private List<ClientResolutionAction> m_movementActions;

	private List<ClientCastAction> m_castActions;

	private List<ClientResolutionActionMessageData> m_receivedMessages;

	private ClientResolutionManager.ClientResolutionManagerState m_state = ClientResolutionManager.ClientResolutionManagerState.Idle;

	private List<ActorData> m_actorsToKillOnLastHitExecution;

	private float m_timeOfLastEvent;

	private bool m_waitingForAllMessages;

	private ClientResolutionManager.MessageHandlersState m_currentMessageHandlersState;

	private ClientResolutionManager.MessageHandlersState m_expectedMessageHandlersState;

	private List<ActorData> m_tempCombatPhaseDataReceivedActors = new List<ActorData>();

	public static ClientResolutionManager Get()
	{
		return ClientResolutionManager.s_instance;
	}

	private void Awake()
	{
		ClientResolutionManager.s_instance = this;
		this.m_currentMessageHandlersState = ClientResolutionManager.MessageHandlersState.NotYetRegistered;
		this.m_expectedMessageHandlersState = ClientResolutionManager.MessageHandlersState.NotYetRegistered;
		this.RegisterHandler();
		this.m_expectedMessageHandlersState = ClientResolutionManager.MessageHandlersState.Registered;
		this.m_resolutionActions = new List<ClientResolutionAction>();
		this.m_movementActions = new List<ClientResolutionAction>();
		this.m_castActions = new List<ClientCastAction>();
		this.m_receivedMessages = new List<ClientResolutionActionMessageData>();
		this.m_actorsToKillOnLastHitExecution = new List<ActorData>();
	}

	private void OnDestroy()
	{
		this.UnregisterHandlers();
		this.m_expectedMessageHandlersState = ClientResolutionManager.MessageHandlersState.Unregistered;
		ClientResolutionManager.s_instance = null;
	}

	private void VerifyMessageHandlerState()
	{
		if (this.m_currentMessageHandlersState != this.m_expectedMessageHandlersState)
		{
			if (this.m_expectedMessageHandlersState == ClientResolutionManager.MessageHandlersState.Registered)
			{
				if (this.m_currentMessageHandlersState == ClientResolutionManager.MessageHandlersState.NotYetRegistered)
				{
					Debug.LogError("ClientResolutionManager believes message handlers should be registered, but they aren't yet, as of Update() being called.  Registering them...");
					this.RegisterHandler();
				}
				else if (this.m_currentMessageHandlersState == ClientResolutionManager.MessageHandlersState.Unregistered)
				{
					Debug.LogError("ClientResolutionManager believes message handlers should be registered, but they were already unregistered.");
					this.m_expectedMessageHandlersState = ClientResolutionManager.MessageHandlersState.Unregistered;
				}
			}
			else if (this.m_expectedMessageHandlersState == ClientResolutionManager.MessageHandlersState.Unregistered)
			{
				if (this.m_currentMessageHandlersState == ClientResolutionManager.MessageHandlersState.Registered)
				{
					Debug.LogError("ClientResolutionManager believes message handlers should be unregistered, but they are still registered, as of Update() being called.  Unregistering them...");
					this.UnregisterHandlers();
				}
				else if (this.m_currentMessageHandlersState == ClientResolutionManager.MessageHandlersState.NotYetRegistered)
				{
					Debug.LogError("ClientResolutionManager believes message handlers should be unregistered, but they never registered in the first place.");
					this.m_currentMessageHandlersState = ClientResolutionManager.MessageHandlersState.Unregistered;
				}
			}
		}
	}

	private void RegisterHandler()
	{
		if (this.m_currentMessageHandlersState == ClientResolutionManager.MessageHandlersState.NotYetRegistered)
		{
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().Client != null)
				{
					ClientGameManager.Get().Client.RegisterHandler(0x3A, new NetworkMessageDelegate(this.MsgStartResolutionPhase));
					ClientGameManager.Get().Client.RegisterHandler(0x40, new NetworkMessageDelegate(this.MsgSingleResolutionAction));
					ClientGameManager.Get().Client.RegisterHandler(0x3F, new NetworkMessageDelegate(this.MsgRunResolutionActionsOutsideResolve));
					ClientGameManager.Get().Client.RegisterHandler(0x42, new NetworkMessageDelegate(this.MsgFailsafeHurryResolutionPhase));
					this.m_currentMessageHandlersState = ClientResolutionManager.MessageHandlersState.Registered;
				}
			}
		}
	}

	public void UnregisterHandlers()
	{
		if (this.m_currentMessageHandlersState == ClientResolutionManager.MessageHandlersState.Registered)
		{
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().Client != null)
				{
					ClientGameManager.Get().Client.UnregisterHandler(0x3A);
					ClientGameManager.Get().Client.UnregisterHandler(0x40);
					ClientGameManager.Get().Client.UnregisterHandler(0x3F);
					ClientGameManager.Get().Client.UnregisterHandler(0x42);
					this.m_currentMessageHandlersState = ClientResolutionManager.MessageHandlersState.Unregistered;
				}
			}
		}
	}

	public void OnTurnStart()
	{
		if (this.m_state == ClientResolutionManager.ClientResolutionManagerState.Idle)
		{
			this.m_currentAbilityPhase = AbilityPriority.INVALID;
		}
	}

	public void OnAbilityPhaseStart(AbilityPriority phase)
	{
		this.m_waitingForAllMessages = false;
		if (phase != this.m_currentAbilityPhase)
		{
			if (this.m_state != ClientResolutionManager.ClientResolutionManagerState.Resolving)
			{
				if (this.m_state != ClientResolutionManager.ClientResolutionManagerState.WaitingForActionMsgs)
				{
					this.m_waitingForAllMessages = true;
				}
			}
		}
	}

	internal void MsgStartResolutionPhase(NetworkMessage netMsg)
	{
		NetworkReader reader = netMsg.reader;
		this.m_currentTurnIndex = reader.ReadInt32();
		sbyte b = reader.ReadSByte();
		this.m_currentAbilityPhase = (AbilityPriority)b;
		sbyte b2 = reader.ReadSByte();
		this.m_numResolutionActionsThisPhase = (int)b2;
		this.m_castActions = new List<ClientCastAction>();
		this.m_timeOfLastEvent = GameTime.time;
		if (this.m_state != ClientResolutionManager.ClientResolutionManagerState.Idle)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Received StartResolutionPhase message for turn ",
				this.m_currentTurnIndex,
				", phase ",
				this.m_currentAbilityPhase.ToString(),
				", but ClientResolutionManager's state is ",
				this.m_state.ToString(),
				"!"
			}));
		}
		this.m_waitingForAllMessages = false;
		this.m_state = ClientResolutionManager.ClientResolutionManagerState.WaitingForActionMsgs;
		this.ProcessReceivedMessages();
	}

	public unsafe List<ClientCastAction> DeSerializeCastActionsFromServer(ref NetworkReader reader)
	{
		sbyte b = reader.ReadSByte();
		List<ClientCastAction> list = new List<ClientCastAction>((int)b);
		for (int i = 0; i < (int)b; i++)
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
		sbyte b = reader.ReadSByte();
		IBitStream bitStream = new NetworkReaderAdapter(reader);
		ClientResolutionAction action = ClientResolutionAction.ClientResolutionAction_DeSerializeFromStream(ref bitStream);
		ClientResolutionActionMessageData item = new ClientResolutionActionMessageData(action, (int)turnIndex, (int)b);
		this.m_receivedMessages.Add(item);
		this.UpdateLastEventTime();
		this.ProcessReceivedMessages();
	}

	private void ProcessReceivedMessages()
	{
		if (this.m_state != ClientResolutionManager.ClientResolutionManagerState.WaitingForActionMsgs)
		{
			return;
		}
		List<ClientResolutionActionMessageData> list = new List<ClientResolutionActionMessageData>();
		using (List<ClientResolutionActionMessageData>.Enumerator enumerator = this.m_receivedMessages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ClientResolutionActionMessageData clientResolutionActionMessageData = enumerator.Current;
				if (clientResolutionActionMessageData.m_phase == this.m_currentAbilityPhase && clientResolutionActionMessageData.m_turnIndex == this.m_currentTurnIndex)
				{
					list.Add(clientResolutionActionMessageData);
				}
			}
		}
		if (list.Count >= this.m_numResolutionActionsThisPhase)
		{
			if (list.Count > this.m_numResolutionActionsThisPhase)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Somehow got more matching ClientResolutionActionMessageData messages (",
					list.Count,
					") than expected (",
					this.m_numResolutionActionsThisPhase,
					") for this turn (",
					this.m_currentTurnIndex.ToString(),
					") / phase (",
					this.m_currentAbilityPhase.ToString(),
					")."
				}));
			}
			this.m_resolutionActions.Clear();
			this.m_movementActions.Clear();
			using (List<ClientResolutionActionMessageData>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ClientResolutionActionMessageData clientResolutionActionMessageData2 = enumerator2.Current;
					ClientResolutionAction action = clientResolutionActionMessageData2.m_action;
					this.m_resolutionActions.Add(action);
					if (action.ReactsToMovement())
					{
						this.m_movementActions.Add(action);
					}
					this.m_receivedMessages.Remove(clientResolutionActionMessageData2);
				}
			}
			this.m_movementActions.Sort();
			if (this.m_receivedMessages.Count > 0)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Received last resolution action for turn ",
					this.m_currentTurnIndex,
					" / phase ",
					this.m_currentAbilityPhase.ToString(),
					", but there are still ",
					this.m_receivedMessages.Count,
					" received message(s) left over!  Clearing..."
				}));
				this.m_receivedMessages.Clear();
			}
			this.OnReceivedLastResolutionAction();
		}
	}

	private void OnReceivedLastResolutionAction()
	{
		this.m_timeOfLastEvent = GameTime.time;
		if (TheatricsManager.Get() != null && TheatricsManager.Get().GetPhaseToUpdate() == AbilityPriority.Combat_Damage)
		{
			this.OnCombatPhasePlayDataReceived();
		}
		if (this.m_currentAbilityPhase == AbilityPriority.Combat_Knockback)
		{
			ClientKnockbackManager.Get().InitKnockbacksFromActions(this.m_resolutionActions);
		}
		if (!ClientAbilityResults.LogMissingSequences)
		{
			if (!ClientAbilityResults.symbol_000E)
			{
				goto IL_DC;
			}
		}
		Log.Warning(string.Concat(new string[]
		{
			ClientAbilityResults.s_clientResolutionNetMsgHeader,
			"<color=white>OnReceivedLastResolutionAction</color> received for phase ",
			this.m_currentAbilityPhase.ToString(),
			".  ",
			this.GetActionsDoneExecutingDebugStr()
		}), new object[0]);
		IL_DC:
		this.m_waitingForAllMessages = false;
		this.m_state = ClientResolutionManager.ClientResolutionManagerState.Resolving;
		if (ClientGameManager.Get())
		{
			if (ClientGameManager.Get().IsFastForward)
			{
				this.ExecuteAllUnexecutedActions(false);
				this.SendResolutionPhaseCompleted(this.m_currentAbilityPhase, true, false);
				return;
			}
		}
		for (int i = 0; i < this.m_resolutionActions.Count; i++)
		{
			ClientResolutionAction clientResolutionAction = this.m_resolutionActions[i];
			if (!clientResolutionAction.ReactsToMovement())
			{
				clientResolutionAction.RunStartSequences();
			}
		}
		if (this.m_currentAbilityPhase == AbilityPriority.INVALID)
		{
			GameEventManager.NormalMovementStartAgs normalMovementStartAgs = new GameEventManager.NormalMovementStartAgs();
			normalMovementStartAgs.m_actorsBeingHitMidMovement = this.GetActorsWithMovementHits();
			GameEventManager.Get().FireEvent(GameEventManager.EventType.NormalMovementStart, normalMovementStartAgs);
		}
	}

	internal void OnCombatPhasePlayDataReceived()
	{
		this.m_tempCombatPhaseDataReceivedActors.Clear();
		for (int i = 0; i < this.m_resolutionActions.Count; i++)
		{
			ClientResolutionAction clientResolutionAction = this.m_resolutionActions[i];
			ActorData caster = clientResolutionAction.GetCaster();
			if (caster != null && caster.GetAbilityData() != null)
			{
				if (!this.m_tempCombatPhaseDataReceivedActors.Contains(caster))
				{
					caster.GetAbilityData().OnClientCombatPhasePlayDataReceived(this.m_resolutionActions);
					this.m_tempCombatPhaseDataReceivedActors.Add(caster);
				}
			}
		}
		this.m_tempCombatPhaseDataReceivedActors.Clear();
	}

	internal void MsgRunResolutionActionsOutsideResolve(NetworkMessage netMsg)
	{
		NetworkReader reader = netMsg.reader;
		sbyte b = reader.ReadSByte();
		IBitStream bitStream = new NetworkReaderAdapter(reader);
		List<ClientResolutionAction> list = new List<ClientResolutionAction>((int)b);
		for (int i = 0; i < (int)b; i++)
		{
			ClientResolutionAction item = ClientResolutionAction.ClientResolutionAction_DeSerializeFromStream(ref bitStream);
			list.Add(item);
		}
		using (List<ClientResolutionAction>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ClientResolutionAction clientResolutionAction = enumerator.Current;
				clientResolutionAction.Run_OutsideResolution();
			}
		}
	}

	internal void MsgFailsafeHurryResolutionPhase(NetworkMessage netMsg)
	{
		NetworkReader reader = netMsg.reader;
		int num = reader.ReadInt32();
		sbyte b = reader.ReadSByte();
		AbilityPriority abilityPriority = (AbilityPriority)b;
		sbyte b2 = reader.ReadSByte();
		List<ActorData> list = new List<ActorData>();
		string text = "Actors not done resolving:";
		if (GameFlowData.Get() == null)
		{
			Log.Warning("Server sent 'hurry' failsafe to clients for turn = {0}, phase = {1}, and we're on turn = {2}, phase = {3}. But GameFlowData is null. Doing nothing... ({4})", new object[]
			{
				num,
				abilityPriority,
				this.m_currentTurnIndex,
				this.m_currentAbilityPhase,
				netMsg.conn
			});
			return;
		}
		for (int i = 0; i < (int)b2; i++)
		{
			int num2 = reader.ReadInt32();
			if (num2 != ActorData.s_invalidActorIndex)
			{
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(num2);
				if (actorData != null)
				{
					list.Add(actorData);
					text = text + "\n\t" + actorData.GetDebugName();
				}
			}
		}
		bool flag = false;
		for (int j = 0; j < list.Count; j++)
		{
			if (GameFlowData.Get().IsActorDataOwned(list[j]))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			if (num == this.m_currentTurnIndex)
			{
				if (abilityPriority == this.m_currentAbilityPhase)
				{
					if (this.m_state == ClientResolutionManager.ClientResolutionManagerState.Idle)
					{
						Debug.Log(string.Concat(new object[]
						{
							"Server sent 'hurry' failsafe to clients for turn = ",
							num,
							", phase = ",
							abilityPriority.ToString(),
							", and we ARE included in the list of actors-still-resolving; but our state is Idle, so it doesn't apply to us.\n",
							text,
							"\n(This client = ",
							GameFlowData.Get().GetActiveOwnedActorDataDebugNameString(),
							".)\nIgnoring failsafe."
						}));
					}
					else if (this.m_state == ClientResolutionManager.ClientResolutionManagerState.WaitingForActionMsgs)
					{
						Debug.LogWarning(string.Concat(new object[]
						{
							"Server sent 'hurry' failsafe to clients for turn = ",
							num,
							", phase = ",
							abilityPriority.ToString(),
							", and we ARE included in the list of actors-still-resolving; but our state is still in WaitingForActionMsgs, so that's very unexpected.\n",
							text,
							"\n(This client = ",
							GameFlowData.Get().GetActiveOwnedActorDataDebugNameString(),
							".)\nIgnoring failsafe... I guess..."
						}));
					}
					else if (this.m_state == ClientResolutionManager.ClientResolutionManagerState.Resolving)
					{
						Debug.LogWarning(string.Concat(new object[]
						{
							"Server sent 'hurry' failsafe to clients for turn = ",
							num,
							", phase = ",
							abilityPriority.ToString(),
							", and we ARE included in the list of actors-still-resolving; the failsafe applies to us.\n",
							text,
							"\n(This client = ",
							GameFlowData.Get().GetActiveOwnedActorDataDebugNameString(),
							".)\nExecuting failsafe..."
						}));
						this.ExecuteFailsafe();
					}
					goto IL_480;
				}
			}
			if (this.m_currentTurnIndex <= num)
			{
				if (this.m_currentTurnIndex == num)
				{
					if (this.m_currentAbilityPhase > abilityPriority)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							goto IL_351;
						}
					}
				}
				Debug.LogWarning(string.Concat(new object[]
				{
					"Server sent 'hurry' failsafe to clients for turn = ",
					num,
					", phase = ",
					abilityPriority.ToString(),
					", but we're on turn = ",
					this.m_currentTurnIndex,
					", phase = ",
					this.m_currentAbilityPhase.ToString(),
					".\n\n(This client = ",
					GameFlowData.Get().GetActiveOwnedActorDataDebugNameString(),
					".)\nThat's... in the future.  Ignoring failsafe..."
				}));
				goto IL_480;
			}
			IL_351:
			Debug.LogWarning(string.Concat(new object[]
			{
				"Server sent 'hurry' failsafe to clients for turn = ",
				num,
				", phase = ",
				abilityPriority.ToString(),
				", but we're on turn = ",
				this.m_currentTurnIndex,
				", phase = ",
				this.m_currentAbilityPhase.ToString(),
				".\n\n(This client = ",
				GameFlowData.Get().GetActiveOwnedActorDataDebugNameString(),
				".)\nThat's in the past, unexpectedly.  Doing nothing..."
			}));
			IL_480:;
		}
		else
		{
			Debug.LogWarning(string.Concat(new object[]
			{
				"Server sent 'hurry' failsafe to clients for turn = ",
				num,
				", phase = ",
				abilityPriority.ToString(),
				", but we're not one of the actors-still-resolving.",
				text,
				"\n(This client = ",
				GameFlowData.Get().GetActiveOwnedActorDataDebugNameString(),
				".)\nIgnoring failsafe."
			}));
		}
	}

	internal bool HitsDoneExecuting()
	{
		bool result = true;
		using (List<ClientResolutionAction>.Enumerator enumerator = this.m_resolutionActions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ClientResolutionAction clientResolutionAction = enumerator.Current;
				if (!clientResolutionAction.CompletedAction())
				{
					return false;
				}
			}
		}
		return result;
	}

	internal bool HitsDoneExecuting(SequenceSource sequenceSource)
	{
		bool result = true;
		for (int i = 0; i < this.m_resolutionActions.Count; i++)
		{
			ClientResolutionAction clientResolutionAction = this.m_resolutionActions[i];
			if (clientResolutionAction.ContainsSequenceSource(sequenceSource))
			{
				result = clientResolutionAction.CompletedAction();
				return result;
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			return result;
		}
	}

	internal bool HasUnexecutedHitsOnActor(ActorData targetActor, int sequenceSourceToIgnore = -1)
	{
		bool flag = false;
		int i = 0;
		while (i < this.m_resolutionActions.Count)
		{
			if (!flag)
			{
				if (sequenceSourceToIgnore < 0)
				{
					goto IL_35;
				}
				if (!this.m_resolutionActions[i].ContainsSequenceSourceID((uint)sequenceSourceToIgnore))
				{
					goto IL_35;
				}
				IL_4C:
				i++;
				continue;
				IL_35:
				flag = this.m_resolutionActions[i].HasUnexecutedHitOnActor(targetActor);
				goto IL_4C;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				return flag;
			}
		}
		return flag;
	}

	internal void ExecuteUnexecutedActions(SequenceSource sequenceSource, string extraInfo)
	{
		for (int i = 0; i < this.m_resolutionActions.Count; i++)
		{
			ClientResolutionAction clientResolutionAction = this.m_resolutionActions[i];
			if (clientResolutionAction.ContainsSequenceSource(sequenceSource))
			{
				if (!clientResolutionAction.CompletedAction())
				{
					string message = string.Concat(new string[]
					{
						"Executing Unexecuted Action: ",
						clientResolutionAction.GetDebugDescription(),
						clientResolutionAction.GetUnexecutedHitsDebugStr(true),
						SequenceManager.Get().GetSequenceHitsSeenDebugString(sequenceSource, true),
						extraInfo
					});
					Log.Error(message, new object[0]);
					clientResolutionAction.ExecuteUnexecutedClientHitsInAction();
				}
			}
		}
	}

	internal void ExecuteAllUnexecutedActions(bool showAsError = true)
	{
		for (int i = 0; i < this.m_resolutionActions.Count; i++)
		{
			ClientResolutionAction clientResolutionAction = this.m_resolutionActions[i];
			if (!clientResolutionAction.CompletedAction())
			{
				if (showAsError)
				{
					Log.Error("Executing Unexecuted Action: " + clientResolutionAction.GetDebugDescription() + clientResolutionAction.GetUnexecutedHitsDebugStr(false), new object[0]);
				}
				clientResolutionAction.ExecuteUnexecutedClientHitsInAction();
			}
		}
	}

	internal string GetActionsDoneExecutingDebugStr()
	{
		string text = string.Empty;
		int num = 0;
		using (List<ClientResolutionAction>.Enumerator enumerator = this.m_resolutionActions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ClientResolutionAction clientResolutionAction = enumerator.Current;
				if (!clientResolutionAction.CompletedAction())
				{
					num++;
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						"\n\t",
						num,
						". ",
						clientResolutionAction.GetDebugDescription(),
						clientResolutionAction.GetUnexecutedHitsDebugStr(false)
					});
				}
			}
		}
		return "Action not done: " + num + text;
	}

	private void Update()
	{
		this.VerifyMessageHandlerState();
		if (this.m_state != ClientResolutionManager.ClientResolutionManagerState.Resolving)
		{
			if (this.m_state != ClientResolutionManager.ClientResolutionManagerState.WaitingForActionMsgs)
			{
				return;
			}
		}
		bool flag = this.HitsDoneExecuting();
		bool flag2;
		if (GameTime.time - this.m_timeOfLastEvent > 15f)
		{
			if (!(GameFlowData.Get() == null))
			{
				flag2 = !GameFlowData.Get().IsResolutionPaused();
			}
			else
			{
				flag2 = true;
			}
		}
		else
		{
			flag2 = false;
		}
		bool flag3 = flag2;
		if (flag)
		{
			if (this.m_state == ClientResolutionManager.ClientResolutionManagerState.Resolving)
			{
				goto IL_9F;
			}
		}
		if (!flag3)
		{
			return;
		}
		IL_9F:
		if (flag3)
		{
			this.ExecuteFailsafe();
		}
		else
		{
			this.SendResolutionPhaseCompleted(this.m_currentAbilityPhase, false, false);
		}
	}

	private void ExecuteFailsafe()
	{
		bool flag = true;
		string text = "ClientResolutionManager sending phase completed message due to failsafe.  State = " + this.m_state.ToString() + ".\n";
		if (this.m_currentAbilityPhase == AbilityPriority.INVALID)
		{
			text += "Phase = Normal Movement\n";
			flag = false;
		}
		else
		{
			text = text + "Phase = " + this.m_currentAbilityPhase.ToString() + "\n";
		}
		text += this.GetActionsDoneExecutingDebugStr();
		if (flag)
		{
			text = text + "\n" + TheatricsManager.Get().GetTheatricsStateString();
		}
		Debug.LogError(text);
		this.ExecuteAllUnexecutedActions(true);
		this.SendResolutionPhaseCompleted(this.m_currentAbilityPhase, true, false);
	}

	public void OnActorMoveStart_ClientResolutionManager(ActorData mover, BoardSquarePathInfo curPath)
	{
		if (curPath != null)
		{
			if (curPath.m_moverDiesHere)
			{
				this.OnActorWillDie(mover);
			}
			using (List<ClientResolutionAction>.Enumerator enumerator = this.m_movementActions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ClientResolutionAction clientResolutionAction = enumerator.Current;
					clientResolutionAction.OnActorMoved_ClientResolutionAction(mover, curPath);
				}
			}
		}
	}

	public void OnActorMoved_ClientResolutionManager(ActorData mover, BoardSquarePathInfo curPath)
	{
		if (curPath.m_moverDiesHere)
		{
			this.OnActorWillDie(mover);
		}
		foreach (ClientResolutionAction clientResolutionAction in this.m_movementActions)
		{
			clientResolutionAction.OnActorMoved_ClientResolutionAction(mover, curPath);
		}
	}

	internal void SendResolutionPhaseCompleted(AbilityPriority abilityPhase, bool asFailsafe, bool asResend)
	{
		if (ClientAbilityResults.LogMissingSequences)
		{
			Log.Warning(string.Concat(new object[]
			{
				ClientAbilityResults.s_clientResolutionNetMsgHeader,
				"<color=white>ClientResolutionPhaseCompleted</color> message sent for phase ",
				abilityPhase.ToString(),
				" (failsafe = ",
				asFailsafe,
				")."
			}), new object[0]);
		}
		using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().m_ownedActorDatas.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				NetworkWriter networkWriter = new NetworkWriter();
				networkWriter.StartMessage(0x3B);
				networkWriter.Write((sbyte)abilityPhase);
				networkWriter.Write(actorData.ActorIndex);
				networkWriter.Write(asFailsafe);
				networkWriter.Write(asResend);
				networkWriter.FinishMessage();
				ClientGameManager.Get().Client.SendWriter(networkWriter, 0);
			}
		}
		this.m_waitingForAllMessages = false;
		this.m_state = ClientResolutionManager.ClientResolutionManagerState.Idle;
	}

	internal void SendActorReadyToResolveKnockback(ActorData knockbackedTarget, ActorData sendingPlayer)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.StartMessage(0x3C);
		networkWriter.Write(knockbackedTarget.ActorIndex);
		networkWriter.Write(sendingPlayer.ActorIndex);
		networkWriter.FinishMessage();
		if (ClientAbilityResults.LogMissingSequences)
		{
			Log.Warning(string.Concat(new string[]
			{
				ClientAbilityResults.s_clientResolutionNetMsgHeader,
				"Sending <color=white>ResolveKnockbackForActor</color>, Caster: ",
				sendingPlayer.GetDebugName(),
				", KnockedBackActor: ",
				knockbackedTarget.GetDebugName()
			}), new object[0]);
		}
		ClientGameManager.Get().Client.SendWriter(networkWriter, 0);
	}

	public void UpdateLastEventTime()
	{
		this.m_timeOfLastEvent = GameTime.time;
	}

	public string GetCurrentStateName()
	{
		return this.m_state.ToString();
	}

	public bool IsInResolutionState()
	{
		return this.m_state == ClientResolutionManager.ClientResolutionManagerState.Resolving;
	}

	public bool IsWaitingForActionMessages(AbilityPriority phase)
	{
		bool result;
		if (this.m_state != ClientResolutionManager.ClientResolutionManagerState.WaitingForActionMsgs)
		{
			if (this.m_waitingForAllMessages)
			{
				result = (this.m_state != ClientResolutionManager.ClientResolutionManagerState.Resolving);
			}
			else
			{
				result = false;
			}
		}
		else
		{
			result = true;
		}
		return result;
	}

	public void OnAbilityCast(ActorData casterActor, Ability ability)
	{
	}

	public void OnActorWillDie(ActorData actor)
	{
		this.m_actorsToKillOnLastHitExecution.Add(actor);
	}

	public void OnHitExecutedOnActor(ActorData targetActor, ActorData caster, bool hasDamage, bool hasHealing, bool canBeReactedTo)
	{
		if (canBeReactedTo)
		{
			for (int i = 0; i < this.m_resolutionActions.Count; i++)
			{
				ClientResolutionAction clientResolutionAction = this.m_resolutionActions[i];
				clientResolutionAction.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
			}
		}
		if (this.m_actorsToKillOnLastHitExecution.Contains(targetActor))
		{
			if (!this.HasUnexecutedHitsOnActor(targetActor, -1))
			{
				this.m_actorsToKillOnLastHitExecution.Remove(targetActor);
				Vector3 position = targetActor.transform.position;
				Vector3 currentMovementDir = targetActor.GetActorMovement().GetCurrentMovementDir();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, currentMovementDir);
				targetActor.DoVisualDeath(impulseInfo);
				targetActor.GetActorMovement().OnMidMovementDeath();
			}
		}
	}

	public List<ActorData> GetActorsWithMovementHits()
	{
		List<ActorData> list = new List<ActorData>();
		for (int i = 0; i < this.m_movementActions.Count; i++)
		{
			ClientResolutionAction clientResolutionAction = this.m_movementActions[i];
			ActorData triggeringMovementActor = clientResolutionAction.GetTriggeringMovementActor();
			if (triggeringMovementActor != null)
			{
				if (!list.Contains(triggeringMovementActor))
				{
					list.Add(triggeringMovementActor);
				}
			}
		}
		return list;
	}

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
}
