using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientMovementManager : MonoBehaviour, IGameEventListener
{
	private static ClientMovementManager s_instance;

	private List<ActorMovementEntry> m_movingActors;

	private List<ActorData> m_movedButUnhandledActors;

	private bool m_amMovingActors;

	private ServerMovementManager.MovementType m_currentMovementType;

	private ClientMovementManager.MessageHandlersState m_currentMessageHandlersState;

	public static ClientMovementManager Get()
	{
		return ClientMovementManager.s_instance;
	}

	private void Awake()
	{
		ClientMovementManager.s_instance = this;
		this.m_currentMessageHandlersState = ClientMovementManager.MessageHandlersState.NotYetRegistered;
		this.RegisterHandler();
		this.ResetData();
	}

	private void ResetData()
	{
		this.m_movingActors = new List<ActorMovementEntry>();
		this.m_movedButUnhandledActors = new List<ActorData>();
		this.m_amMovingActors = false;
		this.m_currentMovementType = ServerMovementManager.MovementType.None;
	}

	private void OnDestroy()
	{
		this.UnregisterHandlers();
		ClientMovementManager.s_instance = null;
	}

	private void RegisterHandler()
	{
		if (this.m_currentMessageHandlersState == ClientMovementManager.MessageHandlersState.NotYetRegistered)
		{
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().Client != null)
				{
					ClientGameManager.Get().Client.RegisterHandler(0x45, new NetworkMessageDelegate(this.MsgServerMovementStarting));
					ClientGameManager.Get().Client.RegisterHandler(0x47, new NetworkMessageDelegate(this.MsgFailsafeHurryMovementPhase));
					this.m_currentMessageHandlersState = ClientMovementManager.MessageHandlersState.Registered;
				}
			}
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.ReplayRestart);
		}
	}

	public void UnregisterHandlers()
	{
		if (this.m_currentMessageHandlersState == ClientMovementManager.MessageHandlersState.Registered)
		{
			if (ClientGameManager.Get() != null && ClientGameManager.Get().Client != null)
			{
				ClientGameManager.Get().Client.UnregisterHandler(0x45);
				ClientGameManager.Get().Client.UnregisterHandler(0x47);
				this.m_currentMessageHandlersState = ClientMovementManager.MessageHandlersState.Unregistered;
			}
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ReplayRestart);
		}
	}

	private void Update()
	{
		if (this.m_amMovingActors && this.AreActorsDoneMoving())
		{
			using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().m_ownedActorDatas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData player = enumerator.Current;
					this.SendMovementCompleted(player, false);
				}
			}
			this.m_movingActors.Clear();
			this.m_currentMovementType = ServerMovementManager.MovementType.None;
			this.m_amMovingActors = false;
		}
	}

	internal void SendMovementCompleted(ActorData player, bool asFailsafe)
	{
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.StartMessage(0x46);
		networkWriter.Write(player.ActorIndex);
		networkWriter.Write(asFailsafe);
		networkWriter.FinishMessage();
		ClientGameManager.Get().Client.SendWriter(networkWriter, 0);
	}

	internal void MsgServerMovementStarting(NetworkMessage netMsg)
	{
		if (ClientGameManager.Get())
		{
			if (ClientGameManager.Get().IsFastForward)
			{
				this.m_movedButUnhandledActors.Clear();
			}
		}
		NetworkReader reader = netMsg.reader;
		sbyte b = reader.ReadSByte();
		ServerMovementManager.MovementType currentMovementType = (ServerMovementManager.MovementType)b;
		sbyte b2 = reader.ReadSByte();
		int num = (int)b2;
		if (this.m_movingActors.Count > 0)
		{
			string text = string.Empty;
			for (int i = 0; i < this.m_movingActors.Count; i++)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"\n\t",
					i,
					". ",
					this.m_movingActors[i].ToString()
				});
			}
			Debug.LogError(string.Concat(new object[]
			{
				"Client received ServerMovementStarting message for movement type ",
				currentMovementType.ToString(),
				", but ClientMovementManager still has ",
				this.m_movingActors.Count,
				" moving actors:",
				text
			}));
		}
		if (this.m_currentMovementType != ServerMovementManager.MovementType.None)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Client received ServerMovementStarting message for movement type ",
				currentMovementType.ToString(),
				", but ClientMovementManager is still in MovementType ",
				this.m_currentMovementType.ToString(),
				"."
			}));
		}
		this.m_movingActors.Clear();
		for (int j = 0; j < num; j++)
		{
			int actorIndex = reader.ReadInt32();
			byte bitField = reader.ReadByte();
			ActorData actor = GameFlowData.Get().FindActorByActorIndex(actorIndex);
			bool doomed;
			ServerClientUtils.GetBoolsFromBitfield(bitField, out doomed);
			ActorMovementEntry item = new ActorMovementEntry(actor, doomed);
			this.m_movingActors.Add(item);
		}
		this.m_currentMovementType = currentMovementType;
		this.m_amMovingActors = true;
		this.ExamineActorMovement();
		if (ClientGameManager.Get())
		{
			if (ClientGameManager.Get().IsFastForward)
			{
				this.ExecuteMovementFailsafe();
				this.m_currentMovementType = ServerMovementManager.MovementType.None;
				return;
			}
		}
	}

	private bool AreActorsDoneMoving()
	{
		bool result = true;
		for (int i = 0; i < this.m_movingActors.Count; i++)
		{
			ActorMovementEntry actorMovementEntry = this.m_movingActors[i];
			if (actorMovementEntry == null)
			{
				Debug.LogError("For some reason, there's a null entry in m_movingActors.");
			}
			else if (actorMovementEntry.m_progressState == ActorMovementEntry.MovementProgressState.FinishedMovement)
			{
			}
			else
			{
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
						Debug.LogError("One of the actors claimed to be moving is dead (" + actor.GetDebugName() + "), but the server doesn't consider him doomed.");
					}
					flag = true;
				}
				else if (actor.IsModelAnimatorDisabled())
				{
					if (!actorMovementEntry.m_doomed)
					{
						if (this.m_currentMovementType != ServerMovementManager.MovementType.NormalMovement_Chase)
						{
							if (this.m_currentMovementType != ServerMovementManager.MovementType.NormalMovement_NonChase)
							{
								goto IL_127;
							}
						}
						Debug.LogError("One of the actors claimed to be moving is ragdolled (" + actor.GetDebugName() + "), but the server doesn't consider him doomed.");
					}
					IL_127:
					flag = true;
				}
				else if (actorMovementEntry.m_progressState == ActorMovementEntry.MovementProgressState.NotStartedMovement)
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
							goto IL_1A5;
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
				IL_1A5:
				if (!flag)
				{
					result = false;
				}
			}
		}
		return result;
	}

	public static bool DoMovementTypesMatch(ActorData.MovementType actorMovementType, ServerMovementManager.MovementType managerMovementType)
	{
		bool result;
		if (actorMovementType == ActorData.MovementType.None)
		{
			result = false;
		}
		else if (managerMovementType == ServerMovementManager.MovementType.None)
		{
			result = false;
		}
		else
		{
			if (actorMovementType != ActorData.MovementType.Charge)
			{
				if (actorMovementType != ActorData.MovementType.Flight)
				{
					if (actorMovementType != ActorData.MovementType.Teleport)
					{
						if (actorMovementType == ActorData.MovementType.WaypointFlight)
						{
						}
						else
						{
							if (actorMovementType == ActorData.MovementType.Knockback)
							{
								return managerMovementType == ServerMovementManager.MovementType.Knockback;
							}
							if (actorMovementType == ActorData.MovementType.Normal)
							{
								bool result2;
								if (managerMovementType != ServerMovementManager.MovementType.NormalMovement_Chase)
								{
									result2 = (managerMovementType == ServerMovementManager.MovementType.NormalMovement_NonChase);
								}
								else
								{
									result2 = true;
								}
								return result2;
							}
							return false;
						}
					}
				}
			}
			result = (managerMovementType == ServerMovementManager.MovementType.Evade);
		}
		return result;
	}

	public void OnActorMoveStart_ClientMovementManager(ActorData mover, BoardSquare dest, ActorData.MovementType actorMovementType, BoardSquarePathInfo path)
	{
		if (actorMovementType != ActorData.MovementType.Normal)
		{
			if (actorMovementType != ActorData.MovementType.Knockback)
			{
				return;
			}
		}
		bool flag = false;
		for (int i = 0; i < this.m_movingActors.Count; i++)
		{
			ActorMovementEntry actorMovementEntry = this.m_movingActors[i];
			if (actorMovementEntry.m_actor == mover)
			{
				flag = true;
				if (ClientMovementManager.DoMovementTypesMatch(actorMovementType, this.m_currentMovementType))
				{
					if (actorMovementType == ActorData.MovementType.Teleport)
					{
						actorMovementEntry.m_progressState = ActorMovementEntry.MovementProgressState.FinishedMovement;
					}
					else
					{
						actorMovementEntry.m_progressState = ActorMovementEntry.MovementProgressState.CurrentlyMoving;
					}
				}
				else
				{
					if (GameFlowData.Get().LocalPlayerData.IsViewingTeam(mover.GetTeam()))
					{
						string text;
						if (dest == null)
						{
							text = "(null)";
						}
						else
						{
							text = string.Concat(new object[]
							{
								"(",
								dest.x,
								", ",
								dest.y,
								")"
							});
						}
						string text2 = text;
						Debug.LogError(string.Concat(new string[]
						{
							"Friendly-to-client actor ",
							mover.GetDebugName(),
							" started moving to boardsquare ",
							text2,
							" with MovementType ",
							actorMovementType.ToString(),
							", and ClientMovementManager has him moving, but manager's current movement type is ",
							this.m_currentMovementType.ToString(),
							"  Declaring it done..."
						}));
					}
					actorMovementEntry.m_progressState = ActorMovementEntry.MovementProgressState.FinishedMovement;
				}
				IL_1AD:
				if (!flag)
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
					this.m_movedButUnhandledActors.Add(mover);
				}
				return;
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			goto IL_1AD;
		}
	}

	public void OnActorMoveStart_Disappeared(ActorData mover, ActorData.MovementType actorMovementType)
	{
		if (actorMovementType != ActorData.MovementType.Normal && actorMovementType != ActorData.MovementType.Knockback)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < this.m_movingActors.Count; i++)
		{
			ActorMovementEntry actorMovementEntry = this.m_movingActors[i];
			if (actorMovementEntry.m_actor == mover)
			{
				flag = true;
				actorMovementEntry.m_progressState = ActorMovementEntry.MovementProgressState.FinishedMovement;
				IL_71:
				if (!flag)
				{
					this.m_movedButUnhandledActors.Add(mover);
				}
				return;
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			goto IL_71;
		}
	}

	private void ExamineActorMovement()
	{
		List<ActorData> list = new List<ActorData>();
		for (int i = 0; i < this.m_movedButUnhandledActors.Count; i++)
		{
			ActorData actorData = this.m_movedButUnhandledActors[i];
			ActorMovementEntry movementEntryForActor = this.GetMovementEntryForActor(actorData);
			if (movementEntryForActor != null)
			{
				movementEntryForActor.m_progressState = ActorMovementEntry.MovementProgressState.CurrentlyMoving;
				list.Add(actorData);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			this.m_movedButUnhandledActors.Remove(list[j]);
		}
		if (this.m_movedButUnhandledActors.Count > 0)
		{
			string str = "There were " + this.m_movedButUnhandledActors.Count.ToString() + " actor(s) with unhandled movement even after examining new movement.  Unhandled actors:";
			for (int k = 0; k < this.m_movedButUnhandledActors.Count; k++)
			{
				str = str + "\n\t" + this.m_movedButUnhandledActors[k].GetDebugName();
			}
			Debug.LogError(str + "\nClearing them...");
			this.m_movedButUnhandledActors.Clear();
		}
	}

	private bool IsActorKnownMover(ActorData actor)
	{
		return this.GetMovementEntryForActor(actor) != null;
	}

	private ActorMovementEntry GetMovementEntryForActor(ActorData actor)
	{
		for (int i = 0; i < this.m_movingActors.Count; i++)
		{
			ActorMovementEntry actorMovementEntry = this.m_movingActors[i];
			if (actorMovementEntry.m_actor == actor)
			{
				return actorMovementEntry;
			}
		}
		return null;
	}

	internal void MsgFailsafeHurryMovementPhase(NetworkMessage netMsg)
	{
		NetworkReader reader = netMsg.reader;
		int num = reader.ReadInt32();
		sbyte b = reader.ReadSByte();
		List<ActorData> list = new List<ActorData>();
		string text = "Actors not done resolving (turn " + num + ")";
		for (int i = 0; i < (int)b; i++)
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
				IL_F3:
				if (flag)
				{
					if (this.AreActorsDoneMoving())
					{
						Debug.Log(string.Concat(new string[]
						{
							"ServerMovementManager sent 'hurry' failsafe to clients, and we ARE included in the list of actors-still-resolving; but our state is Idle, so it doesn't apply to us.\n",
							text,
							"\n(This client = ",
							GameFlowData.Get().GetActiveOwnedActorDataDebugNameString(),
							".)\nIgnoring failsafe."
						}));
					}
					else if (list.Count == 0)
					{
						Debug.LogWarning(string.Concat(new string[]
						{
							"ServerMovementManager sent 'hurry' failsafe to clients, and we ARE included in the list of actors-still-resolving; but the client doesn't know of any actors still resolving, so that's unexpected.\n",
							text,
							"\n(This client = ",
							GameFlowData.Get().GetActiveOwnedActorDataDebugNameString(),
							".)\nIgnoring failsafe... I guess..."
						}));
					}
					else
					{
						Debug.LogWarning(string.Concat(new string[]
						{
							"ServerMovementManager sent 'hurry' failsafe to clients, and we ARE included in the list of actors-still-resolving; the failsafe applies to us.\n",
							text,
							"\n(This client = ",
							GameFlowData.Get().GetActiveOwnedActorDataDebugNameString(),
							".)\nExecuting failsafe..."
						}));
						this.ExecuteMovementFailsafe();
					}
				}
				return;
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			goto IL_F3;
		}
	}

	private void ExecuteMovementFailsafe()
	{
		using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().m_ownedActorDatas.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData player = enumerator.Current;
				this.SendMovementCompleted(player, true);
			}
		}
		this.m_movingActors.Clear();
		this.m_amMovingActors = false;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.ReplayRestart)
		{
			this.ResetData();
		}
	}

	private enum MessageHandlersState
	{
		NotYetRegistered,
		Registered,
		Unregistered
	}
}
