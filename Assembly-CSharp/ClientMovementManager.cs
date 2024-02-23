using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ClientMovementManager : MonoBehaviour, IGameEventListener
{
	private enum MessageHandlersState
	{
		NotYetRegistered,
		Registered,
		Unregistered
	}

	private static ClientMovementManager s_instance;

	private List<ActorMovementEntry> m_movingActors;

	private List<ActorData> m_movedButUnhandledActors;

	private bool m_amMovingActors;

	private ServerMovementManager.MovementType m_currentMovementType;

	private MessageHandlersState m_currentMessageHandlersState;

	public static ClientMovementManager Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		m_currentMessageHandlersState = MessageHandlersState.NotYetRegistered;
		RegisterHandler();
		ResetData();
	}

	private void ResetData()
	{
		m_movingActors = new List<ActorMovementEntry>();
		m_movedButUnhandledActors = new List<ActorData>();
		m_amMovingActors = false;
		m_currentMovementType = ServerMovementManager.MovementType.None;
	}

	private void OnDestroy()
	{
		UnregisterHandlers();
		s_instance = null;
	}

	private void RegisterHandler()
	{
		if (m_currentMessageHandlersState != 0)
		{
			return;
		}
		if (ClientGameManager.Get() != null)
		{
			if (ClientGameManager.Get().Client != null)
			{
				ClientGameManager.Get().Client.RegisterHandler(69, MsgServerMovementStarting);
				ClientGameManager.Get().Client.RegisterHandler(71, MsgFailsafeHurryMovementPhase);
				m_currentMessageHandlersState = MessageHandlersState.Registered;
			}
		}
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.ReplayRestart);
	}

	public void UnregisterHandlers()
	{
		if (m_currentMessageHandlersState != MessageHandlersState.Registered)
		{
			return;
		}
		while (true)
		{
			if (ClientGameManager.Get() != null && ClientGameManager.Get().Client != null)
			{
				ClientGameManager.Get().Client.UnregisterHandler(69);
				ClientGameManager.Get().Client.UnregisterHandler(71);
				m_currentMessageHandlersState = MessageHandlersState.Unregistered;
			}
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ReplayRestart);
			return;
		}
	}

	private void Update()
	{
		if (!m_amMovingActors || !AreActorsDoneMoving())
		{
			return;
		}
		while (true)
		{
			using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().m_ownedActorDatas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					SendMovementCompleted(current, false);
				}
			}
			m_movingActors.Clear();
			m_currentMovementType = ServerMovementManager.MovementType.None;
			m_amMovingActors = false;
			return;
		}
	}

	internal void SendMovementCompleted(ActorData player, bool asFailsafe)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.StartMessage(70);
		networkWriter.Write(player.ActorIndex);
		networkWriter.Write(asFailsafe);
		networkWriter.FinishMessage();
		ClientGameManager.Get().Client.SendWriter(networkWriter, 0);
	}

	internal void MsgServerMovementStarting(NetworkMessage netMsg)
	{
		if ((bool)ClientGameManager.Get())
		{
			if (ClientGameManager.Get().IsFastForward)
			{
				m_movedButUnhandledActors.Clear();
			}
		}
		NetworkReader reader = netMsg.reader;
		sbyte b = reader.ReadSByte();
		ServerMovementManager.MovementType currentMovementType = (ServerMovementManager.MovementType)b;
		sbyte b2 = reader.ReadSByte();
		int num = b2;
		if (m_movingActors.Count > 0)
		{
			string text = string.Empty;
			for (int i = 0; i < m_movingActors.Count; i++)
			{
				string text2 = text;
				text = new StringBuilder().Append(text2).Append("\n\t").Append(i).Append(". ").Append(m_movingActors[i].ToString()).ToString();
			}
			Debug.LogError(new StringBuilder().Append("Client received ServerMovementStarting message for movement type ").Append(currentMovementType.ToString()).Append(", but ClientMovementManager still has ").Append(m_movingActors.Count).Append(" moving actors:").Append(text).ToString());
		}
		if (m_currentMovementType != 0)
		{
			Debug.LogError(new StringBuilder().Append("Client received ServerMovementStarting message for movement type ").Append(currentMovementType.ToString()).Append(", but ClientMovementManager is still in MovementType ").Append(m_currentMovementType.ToString()).Append(".").ToString());
		}
		m_movingActors.Clear();
		for (int j = 0; j < num; j++)
		{
			int actorIndex = reader.ReadInt32();
			byte bitField = reader.ReadByte();
			ActorData actor = GameFlowData.Get().FindActorByActorIndex(actorIndex);
			ServerClientUtils.GetBoolsFromBitfield(bitField, out bool @out);
			ActorMovementEntry item = new ActorMovementEntry(actor, @out);
			m_movingActors.Add(item);
		}
		while (true)
		{
			m_currentMovementType = currentMovementType;
			m_amMovingActors = true;
			ExamineActorMovement();
			if (!ClientGameManager.Get())
			{
				return;
			}
			while (true)
			{
				if (ClientGameManager.Get().IsFastForward)
				{
					while (true)
					{
						ExecuteMovementFailsafe();
						m_currentMovementType = ServerMovementManager.MovementType.None;
						return;
					}
				}
				return;
			}
		}
	}

	private bool AreActorsDoneMoving()
	{
		bool result = true;
		for (int i = 0; i < m_movingActors.Count; i++)
		{
			ActorMovementEntry actorMovementEntry = m_movingActors[i];
			if (actorMovementEntry == null)
			{
				Debug.LogError("For some reason, there's a null entry in m_movingActors.");
				continue;
			}
			if (actorMovementEntry.m_progressState == ActorMovementEntry.MovementProgressState.FinishedMovement)
			{
				continue;
			}
			ActorData actor = actorMovementEntry.m_actor;
			bool flag;
			if (actor == null)
			{
				Debug.LogError("For some reason, there's an entry with a null actor in m_movingActors.");
				flag = true;
			}
			else if (actor.IsDead())
			{
				if (!actorMovementEntry.m_doomed)
				{
					Debug.LogError(new StringBuilder().Append("One of the actors claimed to be moving is dead (").Append(actor.DebugNameString()).Append("), but the server doesn't consider him doomed.").ToString());
				}
				flag = true;
			}
			else
			{
				if (actor.IsInRagdoll())
				{
					if (!actorMovementEntry.m_doomed)
					{
						if (m_currentMovementType != ServerMovementManager.MovementType.NormalMovement_Chase)
						{
							if (m_currentMovementType != ServerMovementManager.MovementType.NormalMovement_NonChase)
							{
								goto IL_0127;
							}
						}
						Debug.LogError(new StringBuilder().Append("One of the actors claimed to be moving is ragdolled (").Append(actor.DebugNameString()).Append("), but the server doesn't consider him doomed.").ToString());
					}
					goto IL_0127;
				}
				if (actorMovementEntry.m_progressState == ActorMovementEntry.MovementProgressState.NotStartedMovement)
				{
					flag = false;
				}
				else
				{
					if (actor.GetCurrentBoardSquare() != null)
					{
						if (actor.GetCurrentBoardSquare() != actor.GetTravelBoardSquare())
						{
							flag = false;
							goto IL_01a5;
						}
					}
					if (actor.GetActorMovement().AmMoving())
					{
						flag = false;
					}
					else
					{
						flag = true;
						actorMovementEntry.m_progressState = ActorMovementEntry.MovementProgressState.FinishedMovement;
					}
				}
			}
			goto IL_01a5;
			IL_0127:
			flag = true;
			goto IL_01a5;
			IL_01a5:
			if (!flag)
			{
				result = false;
			}
		}
		while (true)
		{
			return result;
		}
	}

	public static bool DoMovementTypesMatch(ActorData.MovementType actorMovementType, ServerMovementManager.MovementType managerMovementType)
	{
		if (actorMovementType == ActorData.MovementType.None)
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
		if (managerMovementType == ServerMovementManager.MovementType.None)
		{
			return false;
		}
		if (actorMovementType != ActorData.MovementType.Charge)
		{
			if (actorMovementType != ActorData.MovementType.Flight)
			{
				if (actorMovementType != ActorData.MovementType.Teleport)
				{
					if (actorMovementType != ActorData.MovementType.WaypointFlight)
					{
						if (actorMovementType == ActorData.MovementType.Knockback)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									return managerMovementType == ServerMovementManager.MovementType.Knockback;
								}
							}
						}
						if (actorMovementType == ActorData.MovementType.Normal)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
								{
									int result;
									if (managerMovementType != ServerMovementManager.MovementType.NormalMovement_Chase)
									{
										result = ((managerMovementType == ServerMovementManager.MovementType.NormalMovement_NonChase) ? 1 : 0);
									}
									else
									{
										result = 1;
									}
									return (byte)result != 0;
								}
								}
							}
						}
						return false;
					}
				}
			}
		}
		return managerMovementType == ServerMovementManager.MovementType.Evade;
	}

	public void OnActorMoveStart_ClientMovementManager(ActorData mover, BoardSquare dest, ActorData.MovementType actorMovementType, BoardSquarePathInfo path)
	{
		if (actorMovementType != 0)
		{
			if (actorMovementType != ActorData.MovementType.Knockback)
			{
				return;
			}
		}
		bool flag = false;
		int num = 0;
		while (true)
		{
			if (num < m_movingActors.Count)
			{
				ActorMovementEntry actorMovementEntry = m_movingActors[num];
				if (actorMovementEntry.m_actor == mover)
				{
					flag = true;
					if (DoMovementTypesMatch(actorMovementType, m_currentMovementType))
					{
						if (actorMovementType == ActorData.MovementType.Teleport)
						{
							actorMovementEntry.m_progressState = ActorMovementEntry.MovementProgressState.FinishedMovement;
						}
						else
						{
							actorMovementEntry.m_progressState = ActorMovementEntry.MovementProgressState.CurrentlyMoving;
						}
						break;
					}
					if (GameFlowData.Get().LocalPlayerData.IsViewingTeam(mover.GetTeam()))
					{
						object obj;
						if (dest == null)
						{
							obj = "(null)";
						}
						else
						{
							obj = new StringBuilder().Append("(").Append(dest.x).Append(", ").Append(dest.y).Append(")").ToString();
						}
						string text = (string)obj;
						Debug.LogError(new StringBuilder().Append("Friendly-to-client actor ").Append(mover.DebugNameString()).Append(" started moving to boardsquare ").Append(text).Append(" with MovementType ").Append(actorMovementType.ToString()).Append(", and ClientMovementManager has him moving, but manager's current movement type is ").Append(m_currentMovementType.ToString()).Append("  Declaring it done...").ToString());
					}
					actorMovementEntry.m_progressState = ActorMovementEntry.MovementProgressState.FinishedMovement;
					break;
				}
				num++;
				continue;
			}
			break;
		}
		if (flag)
		{
			return;
		}
		while (true)
		{
			if (!(dest != mover.GetCurrentBoardSquare()))
			{
				if (path == null)
				{
					return;
				}
				if (path.next == null)
				{
					return;
				}
			}
			m_movedButUnhandledActors.Add(mover);
			return;
		}
	}

	public void OnActorMoveStart_Disappeared(ActorData mover, ActorData.MovementType actorMovementType)
	{
		if (actorMovementType != 0 && actorMovementType != ActorData.MovementType.Knockback)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		bool flag = false;
		int num = 0;
		while (true)
		{
			if (num < m_movingActors.Count)
			{
				ActorMovementEntry actorMovementEntry = m_movingActors[num];
				if (actorMovementEntry.m_actor == mover)
				{
					flag = true;
					actorMovementEntry.m_progressState = ActorMovementEntry.MovementProgressState.FinishedMovement;
					break;
				}
				num++;
				continue;
			}
			break;
		}
		if (flag)
		{
			return;
		}
		while (true)
		{
			m_movedButUnhandledActors.Add(mover);
			return;
		}
	}

	private void ExamineActorMovement()
	{
		List<ActorData> list = new List<ActorData>();
		for (int i = 0; i < m_movedButUnhandledActors.Count; i++)
		{
			ActorData actorData = m_movedButUnhandledActors[i];
			ActorMovementEntry movementEntryForActor = GetMovementEntryForActor(actorData);
			if (movementEntryForActor != null)
			{
				movementEntryForActor.m_progressState = ActorMovementEntry.MovementProgressState.CurrentlyMoving;
				list.Add(actorData);
			}
		}
		while (true)
		{
			for (int j = 0; j < list.Count; j++)
			{
				m_movedButUnhandledActors.Remove(list[j]);
			}
			while (true)
			{
				if (m_movedButUnhandledActors.Count > 0)
				{
					string str = new StringBuilder().Append("There were ").Append(m_movedButUnhandledActors.Count).Append(" actor(s) with unhandled movement even after examining new movement.  Unhandled actors:").ToString();
					for (int k = 0; k < m_movedButUnhandledActors.Count; k++)
					{
						str = new StringBuilder().Append(str).Append("\n\t").Append(m_movedButUnhandledActors[k].DebugNameString()).ToString();
					}
					while (true)
					{
						Debug.LogError(new StringBuilder().Append(str).Append("\nClearing them...").ToString());
						m_movedButUnhandledActors.Clear();
						return;
					}
				}
				return;
			}
		}
	}

	private bool IsActorKnownMover(ActorData actor)
	{
		return GetMovementEntryForActor(actor) != null;
	}

	private ActorMovementEntry GetMovementEntryForActor(ActorData actor)
	{
		for (int i = 0; i < m_movingActors.Count; i++)
		{
			ActorMovementEntry actorMovementEntry = m_movingActors[i];
			if (actorMovementEntry.m_actor == actor)
			{
				return actorMovementEntry;
			}
		}
		while (true)
		{
			return null;
		}
	}

	internal void MsgFailsafeHurryMovementPhase(NetworkMessage netMsg)
	{
		NetworkReader reader = netMsg.reader;
		int num = reader.ReadInt32();
		sbyte b = reader.ReadSByte();
		List<ActorData> list = new List<ActorData>();
		string text = new StringBuilder().Append("Actors not done resolving (turn ").Append(num).Append(")").ToString();
		for (int i = 0; i < b; i++)
		{
			int num2 = reader.ReadInt32();
			if (num2 == ActorData.s_invalidActorIndex)
			{
				continue;
			}
			ActorData actorData = GameFlowData.Get().FindActorByActorIndex(num2);
			if (actorData != null)
			{
				list.Add(actorData);
				text = new StringBuilder().Append(text).Append("\n\t").Append(actorData.DebugNameString()).ToString();
			}
		}
		while (true)
		{
			bool flag = false;
			int num3 = 0;
			while (true)
			{
				if (num3 < list.Count)
				{
					if (GameFlowData.Get().IsActorDataOwned(list[num3]))
					{
						flag = true;
						break;
					}
					num3++;
					continue;
				}
				break;
			}
			if (!flag)
			{
				return;
			}
			while (true)
			{
				if (AreActorsDoneMoving())
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							Debug.Log(new StringBuilder().Append("ServerMovementManager sent 'hurry' failsafe to clients, and we ARE included in the list of actors-still-resolving; but our state is Idle, so it doesn't apply to us.\n").Append(text).Append("\n(This client = ").Append(GameFlowData.Get().GetActiveOwnedActorDataDebugNameString()).Append(".)\nIgnoring failsafe.").ToString());
							return;
						}
					}
				}
				if (list.Count == 0)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							Debug.LogWarning(new StringBuilder().Append("ServerMovementManager sent 'hurry' failsafe to clients, and we ARE included in the list of actors-still-resolving; but the client doesn't know of any actors still resolving, so that's unexpected.\n").Append(text).Append("\n(This client = ").Append(GameFlowData.Get().GetActiveOwnedActorDataDebugNameString()).Append(".)\nIgnoring failsafe... I guess...").ToString());
							return;
						}
					}
				}
				Debug.LogWarning(new StringBuilder().Append("ServerMovementManager sent 'hurry' failsafe to clients, and we ARE included in the list of actors-still-resolving; the failsafe applies to us.\n").Append(text).Append("\n(This client = ").Append(GameFlowData.Get().GetActiveOwnedActorDataDebugNameString()).Append(".)\nExecuting failsafe...").ToString());
				ExecuteMovementFailsafe();
				return;
			}
		}
	}

	private void ExecuteMovementFailsafe()
	{
		using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().m_ownedActorDatas.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				SendMovementCompleted(current, true);
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					goto end_IL_0014;
				}
			}
			end_IL_0014:;
		}
		m_movingActors.Clear();
		m_amMovingActors = false;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.ReplayRestart)
		{
			ResetData();
		}
	}
}
