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
		while (true)
		{
			if (m_expectedMessageHandlersState == MessageHandlersState.Registered)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						if (m_currentMessageHandlersState == MessageHandlersState.NotYetRegistered)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									Debug.LogError("ClientResolutionManager believes message handlers should be registered, but they aren't yet, as of Update() being called.  Registering them...");
									RegisterHandler();
									return;
								}
							}
						}
						if (m_currentMessageHandlersState == MessageHandlersState.Unregistered)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									Debug.LogError("ClientResolutionManager believes message handlers should be registered, but they were already unregistered.");
									m_expectedMessageHandlersState = MessageHandlersState.Unregistered;
									return;
								}
							}
						}
						return;
					}
				}
			}
			if (m_expectedMessageHandlersState == MessageHandlersState.Unregistered)
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
			return;
		}
	}

	private void RegisterHandler()
	{
		if (m_currentMessageHandlersState != 0)
		{
			return;
		}
		while (true)
		{
			if (!(ClientGameManager.Get() != null))
			{
				return;
			}
			while (true)
			{
				if (ClientGameManager.Get().Client != null)
				{
					while (true)
					{
						ClientGameManager.Get().Client.RegisterHandler(58, MsgStartResolutionPhase);
						ClientGameManager.Get().Client.RegisterHandler(64, MsgSingleResolutionAction);
						ClientGameManager.Get().Client.RegisterHandler(63, MsgRunResolutionActionsOutsideResolve);
						ClientGameManager.Get().Client.RegisterHandler(66, MsgFailsafeHurryResolutionPhase);
						m_currentMessageHandlersState = MessageHandlersState.Registered;
						return;
					}
				}
				return;
			}
		}
	}

	public void UnregisterHandlers()
	{
		if (m_currentMessageHandlersState != MessageHandlersState.Registered)
		{
			return;
		}
		while (true)
		{
			if (!(ClientGameManager.Get() != null))
			{
				return;
			}
			while (true)
			{
				if (ClientGameManager.Get().Client != null)
				{
					while (true)
					{
						ClientGameManager.Get().Client.UnregisterHandler(58);
						ClientGameManager.Get().Client.UnregisterHandler(64);
						ClientGameManager.Get().Client.UnregisterHandler(63);
						ClientGameManager.Get().Client.UnregisterHandler(66);
						m_currentMessageHandlersState = MessageHandlersState.Unregistered;
						return;
					}
				}
				return;
			}
		}
	}

	public void OnTurnStart()
	{
		if (m_state != ClientResolutionManagerState.Idle)
		{
			return;
		}
		while (true)
		{
			m_currentAbilityPhase = AbilityPriority.INVALID;
			return;
		}
	}

	public void OnAbilityPhaseStart(AbilityPriority phase)
	{
		m_waitingForAllMessages = false;
		if (phase == m_currentAbilityPhase)
		{
			return;
		}
		while (true)
		{
			if (m_state == ClientResolutionManagerState.Resolving)
			{
				return;
			}
			while (true)
			{
				if (m_state != 0)
				{
					while (true)
					{
						m_waitingForAllMessages = true;
						return;
					}
				}
				return;
			}
		}
	}

	internal void MsgStartResolutionPhase(NetworkMessage netMsg)
	{
		NetworkReader reader = netMsg.reader;
		m_currentTurnIndex = reader.ReadInt32();
		sbyte b = reader.ReadSByte();
		m_currentAbilityPhase = (AbilityPriority)b;
		sbyte b2 = reader.ReadSByte();
		m_numResolutionActionsThisPhase = b2;
		m_castActions = new List<ClientCastAction>();
		m_timeOfLastEvent = GameTime.time;
		if (m_state != ClientResolutionManagerState.Idle)
		{
			Debug.LogError("Received StartResolutionPhase message for turn " + m_currentTurnIndex + ", phase " + m_currentAbilityPhase.ToString() + ", but ClientResolutionManager's state is " + m_state.ToString() + "!");
		}
		m_waitingForAllMessages = false;
		m_state = ClientResolutionManagerState.WaitingForActionMsgs;
		ProcessReceivedMessages();
	}

	public List<ClientCastAction> DeSerializeCastActionsFromServer(ref NetworkReader reader)
	{
		sbyte b = reader.ReadSByte();
		List<ClientCastAction> list = new List<ClientCastAction>(b);
		for (int i = 0; i < b; i++)
		{
			ClientCastAction item = ClientCastAction.ClientCastAction_DeSerializeFromReader(ref reader);
			list.Add(item);
		}
		while (true)
		{
			return list;
		}
	}

	private void MsgSingleResolutionAction(NetworkMessage netMsg)
	{
		NetworkReader reader = netMsg.reader;
		uint turnIndex = reader.ReadPackedUInt32();
		sbyte b = reader.ReadSByte();
		IBitStream stream = new NetworkReaderAdapter(reader);
		ClientResolutionAction action = ClientResolutionAction.ClientResolutionAction_DeSerializeFromStream(ref stream);
		ClientResolutionActionMessageData item = new ClientResolutionActionMessageData(action, (int)turnIndex, b);
		m_receivedMessages.Add(item);
		UpdateLastEventTime();
		ProcessReceivedMessages();
	}

	private void ProcessReceivedMessages()
	{
		if (m_state != 0)
		{
			return;
		}
		List<ClientResolutionActionMessageData> list = new List<ClientResolutionActionMessageData>();
		using (List<ClientResolutionActionMessageData>.Enumerator enumerator = m_receivedMessages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ClientResolutionActionMessageData current = enumerator.Current;
				if (current.m_phase == m_currentAbilityPhase && current.m_turnIndex == m_currentTurnIndex)
				{
					list.Add(current);
				}
			}
		}
		if (list.Count < m_numResolutionActionsThisPhase)
		{
			return;
		}
		while (true)
		{
			if (list.Count > m_numResolutionActionsThisPhase)
			{
				Debug.LogError("Somehow got more matching ClientResolutionActionMessageData messages (" + list.Count + ") than expected (" + m_numResolutionActionsThisPhase + ") for this turn (" + m_currentTurnIndex.ToString() + ") / phase (" + m_currentAbilityPhase.ToString() + ").");
			}
			m_resolutionActions.Clear();
			m_movementActions.Clear();
			using (List<ClientResolutionActionMessageData>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ClientResolutionActionMessageData current2 = enumerator2.Current;
					ClientResolutionAction action = current2.m_action;
					m_resolutionActions.Add(action);
					if (action.ReactsToMovement())
					{
						m_movementActions.Add(action);
					}
					m_receivedMessages.Remove(current2);
				}
			}
			m_movementActions.Sort();
			if (m_receivedMessages.Count > 0)
			{
				Debug.LogError("Received last resolution action for turn " + m_currentTurnIndex + " / phase " + m_currentAbilityPhase.ToString() + ", but there are still " + m_receivedMessages.Count + " received message(s) left over!  Clearing...");
				m_receivedMessages.Clear();
			}
			OnReceivedLastResolutionAction();
			return;
		}
	}

	private void OnReceivedLastResolutionAction()
	{
		m_timeOfLastEvent = GameTime.time;
		if (TheatricsManager.Get() != null && TheatricsManager.Get().GetPhaseToUpdate() == AbilityPriority.Combat_Damage)
		{
			OnCombatPhasePlayDataReceived();
		}
		if (m_currentAbilityPhase == AbilityPriority.Combat_Knockback)
		{
			ClientKnockbackManager.Get().InitKnockbacksFromActions(m_resolutionActions);
		}
		if (!ClientAbilityResults.DebugTraceOn)
		{
			if (!ClientAbilityResults.DebugSerializeSizeOn)
			{
				goto IL_00dc;
			}
		}
		Log.Warning(ClientAbilityResults.s_clientResolutionNetMsgHeader + "<color=white>OnReceivedLastResolutionAction</color> received for phase " + m_currentAbilityPhase.ToString() + ".  " + GetActionsDoneExecutingDebugStr());
		goto IL_00dc;
		IL_00dc:
		m_waitingForAllMessages = false;
		m_state = ClientResolutionManagerState.Resolving;
		if ((bool)ClientGameManager.Get())
		{
			if (ClientGameManager.Get().IsFastForward)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						ExecuteAllUnexecutedActions(false);
						SendResolutionPhaseCompleted(m_currentAbilityPhase, true, false);
						return;
					}
				}
			}
		}
		for (int i = 0; i < m_resolutionActions.Count; i++)
		{
			ClientResolutionAction clientResolutionAction = m_resolutionActions[i];
			if (!clientResolutionAction.ReactsToMovement())
			{
				clientResolutionAction.RunStartSequences();
			}
		}
		if (m_currentAbilityPhase != AbilityPriority.INVALID)
		{
			return;
		}
		while (true)
		{
			GameEventManager.NormalMovementStartAgs normalMovementStartAgs = new GameEventManager.NormalMovementStartAgs();
			normalMovementStartAgs.m_actorsBeingHitMidMovement = GetActorsWithMovementHits();
			GameEventManager.Get().FireEvent(GameEventManager.EventType.NormalMovementStart, normalMovementStartAgs);
			return;
		}
	}

	internal void OnCombatPhasePlayDataReceived()
	{
		m_tempCombatPhaseDataReceivedActors.Clear();
		for (int i = 0; i < m_resolutionActions.Count; i++)
		{
			ClientResolutionAction clientResolutionAction = m_resolutionActions[i];
			ActorData caster = clientResolutionAction.GetCaster();
			if (!(caster != null) || !(caster.GetAbilityData() != null))
			{
				continue;
			}
			if (!m_tempCombatPhaseDataReceivedActors.Contains(caster))
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
		sbyte b = reader.ReadSByte();
		IBitStream stream = new NetworkReaderAdapter(reader);
		List<ClientResolutionAction> list = new List<ClientResolutionAction>(b);
		for (int i = 0; i < b; i++)
		{
			ClientResolutionAction item = ClientResolutionAction.ClientResolutionAction_DeSerializeFromStream(ref stream);
			list.Add(item);
		}
		while (true)
		{
			using (List<ClientResolutionAction>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ClientResolutionAction current = enumerator.Current;
					current.Run_OutsideResolution();
				}
				while (true)
				{
					switch (1)
					{
					default:
						return;
					case 0:
						break;
					}
				}
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
			Log.Warning("Server sent 'hurry' failsafe to clients for turn = {0}, phase = {1}, and we're on turn = {2}, phase = {3}. But GameFlowData is null. Doing nothing... ({4})", num, abilityPriority, m_currentTurnIndex, m_currentAbilityPhase, netMsg.conn);
			return;
		}
		for (int i = 0; i < b2; i++)
		{
			int num2 = reader.ReadInt32();
			if (num2 != ActorData.s_invalidActorIndex)
			{
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(num2);
				if (actorData != null)
				{
					list.Add(actorData);
					text = text + "\n\t" + actorData.DebugNameString();
				}
			}
		}
		while (true)
		{
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
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						{
							if (num == m_currentTurnIndex)
							{
								if (abilityPriority == m_currentAbilityPhase)
								{
									while (true)
									{
										switch (7)
										{
										case 0:
											break;
										default:
											if (m_state == ClientResolutionManagerState.Idle)
											{
												while (true)
												{
													switch (6)
													{
													case 0:
														break;
													default:
														Debug.Log("Server sent 'hurry' failsafe to clients for turn = " + num + ", phase = " + abilityPriority.ToString() + ", and we ARE included in the list of actors-still-resolving; but our state is Idle, so it doesn't apply to us.\n" + text + "\n(This client = " + GameFlowData.Get().GetActiveOwnedActorDataDebugNameString() + ".)\nIgnoring failsafe.");
														return;
													}
												}
											}
											if (m_state == ClientResolutionManagerState.WaitingForActionMsgs)
											{
												while (true)
												{
													switch (4)
													{
													case 0:
														break;
													default:
														Debug.LogWarning("Server sent 'hurry' failsafe to clients for turn = " + num + ", phase = " + abilityPriority.ToString() + ", and we ARE included in the list of actors-still-resolving; but our state is still in WaitingForActionMsgs, so that's very unexpected.\n" + text + "\n(This client = " + GameFlowData.Get().GetActiveOwnedActorDataDebugNameString() + ".)\nIgnoring failsafe... I guess...");
														return;
													}
												}
											}
											if (m_state == ClientResolutionManagerState.Resolving)
											{
												while (true)
												{
													switch (4)
													{
													case 0:
														break;
													default:
														Debug.LogWarning("Server sent 'hurry' failsafe to clients for turn = " + num + ", phase = " + abilityPriority.ToString() + ", and we ARE included in the list of actors-still-resolving; the failsafe applies to us.\n" + text + "\n(This client = " + GameFlowData.Get().GetActiveOwnedActorDataDebugNameString() + ".)\nExecuting failsafe...");
														ExecuteFailsafe();
														return;
													}
												}
											}
											return;
										}
									}
								}
							}
							if (m_currentTurnIndex <= num)
							{
								if (m_currentTurnIndex == num)
								{
									if (m_currentAbilityPhase > abilityPriority)
									{
										goto IL_0351;
									}
								}
								Debug.LogWarning("Server sent 'hurry' failsafe to clients for turn = " + num + ", phase = " + abilityPriority.ToString() + ", but we're on turn = " + m_currentTurnIndex + ", phase = " + m_currentAbilityPhase.ToString() + ".\n\n(This client = " + GameFlowData.Get().GetActiveOwnedActorDataDebugNameString() + ".)\nThat's... in the future.  Ignoring failsafe...");
								return;
							}
							goto IL_0351;
						}
						IL_0351:
						Debug.LogWarning("Server sent 'hurry' failsafe to clients for turn = " + num + ", phase = " + abilityPriority.ToString() + ", but we're on turn = " + m_currentTurnIndex + ", phase = " + m_currentAbilityPhase.ToString() + ".\n\n(This client = " + GameFlowData.Get().GetActiveOwnedActorDataDebugNameString() + ".)\nThat's in the past, unexpectedly.  Doing nothing...");
						return;
					}
				}
			}
			Debug.LogWarning("Server sent 'hurry' failsafe to clients for turn = " + num + ", phase = " + abilityPriority.ToString() + ", but we're not one of the actors-still-resolving." + text + "\n(This client = " + GameFlowData.Get().GetActiveOwnedActorDataDebugNameString() + ".)\nIgnoring failsafe.");
			return;
		}
	}

	internal bool HitsDoneExecuting()
	{
		bool result = true;
		using (List<ClientResolutionAction>.Enumerator enumerator = m_resolutionActions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ClientResolutionAction current = enumerator.Current;
				if (!current.CompletedAction())
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return result;
				}
			}
		}
	}

	internal bool HitsDoneExecuting(SequenceSource sequenceSource)
	{
		bool result = true;
		int num = 0;
		while (true)
		{
			if (num < m_resolutionActions.Count)
			{
				ClientResolutionAction clientResolutionAction = m_resolutionActions[num];
				if (clientResolutionAction.ContainsSequenceSource(sequenceSource))
				{
					result = clientResolutionAction.CompletedAction();
					break;
				}
				num++;
				continue;
			}
			break;
		}
		return result;
	}

	internal bool HasUnexecutedHitsOnActor(ActorData targetActor, int sequenceSourceToIgnore = -1)
	{
		bool flag = false;
		for (int i = 0; i < m_resolutionActions.Count; i++)
		{
			if (!flag)
			{
				if (sequenceSourceToIgnore >= 0)
				{
					if (m_resolutionActions[i].ContainsSequenceSourceID((uint)sequenceSourceToIgnore))
					{
						continue;
					}
				}
				flag = m_resolutionActions[i].HasUnexecutedHitOnActor(targetActor);
				continue;
			}
			break;
		}
		return flag;
	}

	internal void ExecuteUnexecutedActions(SequenceSource sequenceSource, string extraInfo)
	{
		for (int i = 0; i < m_resolutionActions.Count; i++)
		{
			ClientResolutionAction clientResolutionAction = m_resolutionActions[i];
			if (!clientResolutionAction.ContainsSequenceSource(sequenceSource))
			{
				continue;
			}
			if (!clientResolutionAction.CompletedAction())
			{
				string message = "Executing Unexecuted Action: " + clientResolutionAction.GetDebugDescription() + clientResolutionAction.GetUnexecutedHitsDebugStr(true) + SequenceManager.Get().GetSequenceHitsSeenDebugString(sequenceSource) + extraInfo;
				Log.Error(message);
				clientResolutionAction.ExecuteUnexecutedClientHitsInAction();
			}
		}
	}

	internal void ExecuteAllUnexecutedActions(bool showAsError = true)
	{
		for (int i = 0; i < m_resolutionActions.Count; i++)
		{
			ClientResolutionAction clientResolutionAction = m_resolutionActions[i];
			if (clientResolutionAction.CompletedAction())
			{
				continue;
			}
			if (showAsError)
			{
				Log.Error("Executing Unexecuted Action: " + clientResolutionAction.GetDebugDescription() + clientResolutionAction.GetUnexecutedHitsDebugStr());
			}
			clientResolutionAction.ExecuteUnexecutedClientHitsInAction();
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	internal string GetActionsDoneExecutingDebugStr()
	{
		string text = string.Empty;
		int num = 0;
		using (List<ClientResolutionAction>.Enumerator enumerator = m_resolutionActions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ClientResolutionAction current = enumerator.Current;
				if (!current.CompletedAction())
				{
					num++;
					string text2 = text;
					text = text2 + "\n\t" + num + ". " + current.GetDebugDescription() + current.GetUnexecutedHitsDebugStr();
				}
			}
		}
		return "Action not done: " + num + text;
	}

	private void Update()
	{
		VerifyMessageHandlerState();
		if (m_state != ClientResolutionManagerState.Resolving)
		{
			if (m_state != 0)
			{
				return;
			}
		}
		bool flag = HitsDoneExecuting();
		int num;
		if (GameTime.time - m_timeOfLastEvent > 15f)
		{
			if (!(GameFlowData.Get() == null))
			{
				num = ((!GameFlowData.Get().IsResolutionPaused()) ? 1 : 0);
			}
			else
			{
				num = 1;
			}
		}
		else
		{
			num = 0;
		}
		bool flag2 = (byte)num != 0;
		if (flag)
		{
			if (m_state == ClientResolutionManagerState.Resolving)
			{
				goto IL_009f;
			}
		}
		if (!flag2)
		{
			return;
		}
		goto IL_009f;
		IL_009f:
		if (flag2)
		{
			ExecuteFailsafe();
		}
		else
		{
			SendResolutionPhaseCompleted(m_currentAbilityPhase, false, false);
		}
	}

	private void ExecuteFailsafe()
	{
		bool flag = true;
		string str = "ClientResolutionManager sending phase completed message due to failsafe.  State = " + m_state.ToString() + ".\n";
		if (m_currentAbilityPhase == AbilityPriority.INVALID)
		{
			str += "Phase = Normal Movement\n";
			flag = false;
		}
		else
		{
			str = str + "Phase = " + m_currentAbilityPhase.ToString() + "\n";
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
			using (List<ClientResolutionAction>.Enumerator enumerator = m_movementActions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ClientResolutionAction current = enumerator.Current;
					current.OnActorMoved_ClientResolutionAction(mover, curPath);
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return;
					}
				}
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
		using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().m_ownedActorDatas.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				NetworkWriter networkWriter = new NetworkWriter();
				networkWriter.StartMessage(59);
				networkWriter.Write((sbyte)abilityPhase);
				networkWriter.Write(current.ActorIndex);
				networkWriter.Write(asFailsafe);
				networkWriter.Write(asResend);
				networkWriter.FinishMessage();
				ClientGameManager.Get().Client.SendWriter(networkWriter, 0);
			}
		}
		m_waitingForAllMessages = false;
		m_state = ClientResolutionManagerState.Idle;
	}

	internal void SendActorReadyToResolveKnockback(ActorData knockbackedTarget, ActorData sendingPlayer)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.StartMessage(60);
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
		int result;
		if (m_state != 0)
		{
			if (m_waitingForAllMessages)
			{
				result = ((m_state != ClientResolutionManagerState.Resolving) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
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
			for (int i = 0; i < m_resolutionActions.Count; i++)
			{
				ClientResolutionAction clientResolutionAction = m_resolutionActions[i];
				clientResolutionAction.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
			}
		}
		if (!m_actorsToKillOnLastHitExecution.Contains(targetActor))
		{
			return;
		}
		while (true)
		{
			if (!HasUnexecutedHitsOnActor(targetActor))
			{
				while (true)
				{
					m_actorsToKillOnLastHitExecution.Remove(targetActor);
					Vector3 position = targetActor.transform.position;
					Vector3 currentMovementDir = targetActor.GetActorMovement().GetCurrentMovementDir();
					ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, currentMovementDir);
					targetActor.DoVisualDeath(impulseInfo);
					targetActor.GetActorMovement().OnMidMovementDeath();
					return;
				}
			}
			return;
		}
	}

	public List<ActorData> GetActorsWithMovementHits()
	{
		List<ActorData> list = new List<ActorData>();
		for (int i = 0; i < m_movementActions.Count; i++)
		{
			ClientResolutionAction clientResolutionAction = m_movementActions[i];
			ActorData triggeringMovementActor = clientResolutionAction.GetTriggeringMovementActor();
			if (triggeringMovementActor == null)
			{
				continue;
			}
			if (!list.Contains(triggeringMovementActor))
			{
				list.Add(triggeringMovementActor);
			}
		}
		return list;
	}
}
