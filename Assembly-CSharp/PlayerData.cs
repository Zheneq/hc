using CameraManagerInternal;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerData : NetworkBehaviour
{
	[HideInInspector]
	public static int s_invalidPlayerIndex;

	internal string m_playerHandle = string.Empty;

	internal bool m_reconnecting;

	[HideInInspector]
	internal Team m_spectatingTeam;

	private Team m_prevSpectatingTeam;

	[HideInInspector]
	internal bool m_isLocal;

	[HideInInspector]
	public ActorData ActorData;

	[HideInInspector]
	private FogOfWar m_fogOfWar;

	internal Player m_player;

	private int m_playerIndex = s_invalidPlayerIndex;

	private static int kCmdCmdTheatricsManagerUpdatePhaseEnded;

	private static int kCmdCmdTutorialQueueEmpty;

	private static int kCmdCmdDebugEndGame;

	private static int kCmdCmdSetPausedForDebugging;

	internal string PlayerHandle => m_playerHandle;

	public bool IsLocal => m_isLocal;

	public int PlayerIndex
	{
		get
		{
			return m_playerIndex;
		}
		set
		{
			if (m_playerIndex != s_invalidPlayerIndex)
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (value == s_invalidPlayerIndex)
				{
					return;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					if (GameFlowData.Get() != null && MyNetworkManager.Get() != null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							m_playerIndex = value;
							return;
						}
					}
					return;
				}
			}
		}
	}

	static PlayerData()
	{
		s_invalidPlayerIndex = -1;
		kCmdCmdTheatricsManagerUpdatePhaseEnded = -438226347;
		NetworkBehaviour.RegisterCommandDelegate(typeof(PlayerData), kCmdCmdTheatricsManagerUpdatePhaseEnded, InvokeCmdCmdTheatricsManagerUpdatePhaseEnded);
		kCmdCmdTutorialQueueEmpty = -1356677979;
		NetworkBehaviour.RegisterCommandDelegate(typeof(PlayerData), kCmdCmdTutorialQueueEmpty, InvokeCmdCmdTutorialQueueEmpty);
		kCmdCmdDebugEndGame = -1295019579;
		NetworkBehaviour.RegisterCommandDelegate(typeof(PlayerData), kCmdCmdDebugEndGame, InvokeCmdCmdDebugEndGame);
		kCmdCmdSetPausedForDebugging = -1571708758;
		NetworkBehaviour.RegisterCommandDelegate(typeof(PlayerData), kCmdCmdSetPausedForDebugging, InvokeCmdCmdSetPausedForDebugging);
		NetworkCRC.RegisterBehaviour("PlayerData", 0);
	}

	internal FogOfWar GetFogOfWar()
	{
		if (m_fogOfWar == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_fogOfWar = GetComponent<FogOfWar>();
			if (m_fogOfWar == null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				m_fogOfWar = base.gameObject.AddComponent<FogOfWar>();
			}
		}
		return m_fogOfWar;
	}

	public void SetPlayerOnReconnect(Player agent)
	{
		m_player = agent;
		NetworkIdentity component = GetComponent<NetworkIdentity>();
		if (component != null)
		{
			component.RebuildObservers(true);
		}
		string text;
		if (GameFlow.Get() != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			text = GameFlow.Get().GetPlayerHandleFromConnectionId(m_player.m_connectionId);
		}
		else
		{
			text = string.Empty;
		}
		string text2 = text;
		if (string.IsNullOrEmpty(text2))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			m_playerHandle = text2;
			return;
		}
	}

	public Player GetPlayer()
	{
		return m_player;
	}

	public PlayerDetails LookupDetails()
	{
		PlayerDetails value = null;
		GameFlow.Get().playerDetails.TryGetValue(m_player, out value);
		return value;
	}

	public void Awake()
	{
		ActorData = GetComponent<ActorData>();
	}

	public void Start()
	{
		m_spectatingTeam = Team.Invalid;
		m_prevSpectatingTeam = Team.Invalid;
		GameFlowData.Get().AddPlayer(base.gameObject);
		GetFogOfWar().MarkForRecalculateVisibility();
	}

	private void OnDestroy()
	{
		if (!(GameFlowData.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			GameFlowData.Get().RemoveExistingPlayer(base.gameObject);
			return;
		}
	}

	public void Update()
	{
		if (m_reconnecting)
		{
			RestoreClientLastKnownStateOnReconnect();
			m_reconnecting = false;
		}
		if (!m_isLocal)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if ((bool)ActorData)
			{
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.CycleTeam))
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						CycleSpectatingTeam();
						return;
					}
				}
				return;
			}
		}
	}

	public override void OnStartClient()
	{
		base.OnStartClient();
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (NetworkServer.active)
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				PlayerDetails value = null;
				if (GameFlow.Get().playerDetails.TryGetValue(m_player, out value) && value.IsLocal())
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						GameManager.PlayerObjectStartedOnClientNotification playerObjectStartedOnClientNotification = new GameManager.PlayerObjectStartedOnClientNotification();
						playerObjectStartedOnClientNotification.AccountId = GameManager.Get().PlayerInfo.AccountId;
						playerObjectStartedOnClientNotification.PlayerId = GameManager.Get().PlayerInfo.PlayerId;
						ClientGameManager.Get().Client.Send(65, playerObjectStartedOnClientNotification);
						m_isLocal = true;
						m_reconnecting = true;
						return;
					}
				}
				return;
			}
		}
	}

	public override void OnStartLocalPlayer()
	{
		if (NetworkServer.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			NetworkIdentity component = GetComponent<NetworkIdentity>();
			component.RebuildObservers(true);
		}
		Log.Info("ActorData.OnStartClient: local player");
		m_isLocal = true;
		ClientGameManager.Get().PlayerObjectStartedOnClient = true;
		Log.Info("HEALTHBARCHECK: IS STARTED: " + ClientGameManager.Get().DesignSceneStarted);
		if (ClientGameManager.Get().DesignSceneStarted)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			UIScreenManager.Get().TryLoadAndSetupInGameUI();
		}
		ClientGameManager.Get().CheckAndSendClientPreparedForGameStartNotification();
		string text;
		if (GameFlow.Get() != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			text = GameFlow.Get().GetPlayerHandleFromConnectionId(m_player.m_connectionId);
		}
		else
		{
			text = string.Empty;
		}
		string text2 = text;
		if (!string.IsNullOrEmpty(text2))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			m_playerHandle = text2;
		}
		SetupCamera();
		SetupHUD();
	}

	private void SetupHUD()
	{
		if (!(HUD_UI.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			HUD_UI.Get().m_mainScreenPanel.m_abilityBar.Setup(ActorData);
			HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.ProcessTeams();
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.MarkFramesForForceUpdate();
			if (HUD_UI.Get().m_mainScreenPanel.m_spectatorHUD != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_spectatorHUD, ActorData == null);
					return;
				}
			}
			return;
		}
	}

	private void SetupCamera(bool reconnecting = false)
	{
		if (ActorData == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (SpawnPointManager.Get() != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				List<BoardSquare> squaresInRegion = SpawnPointManager.Get().m_initialSpawnPointsTeamA.GetSquaresInRegion();
				List<BoardSquare> squaresInRegion2 = SpawnPointManager.Get().m_initialSpawnPointsTeamB.GetSquaresInRegion();
				if (squaresInRegion.Count > 0 && squaresInRegion2.Count > 0)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					Vector3 position = squaresInRegion[0].transform.position;
					Vector3 position2 = squaresInRegion2[0].transform.position;
					Vector3 lhs = position - position2;
					lhs.y = 0f;
					if (Mathf.Abs(lhs.x) > Mathf.Abs(lhs.z))
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						lhs.z = 0f;
					}
					else
					{
						lhs.x = 0f;
					}
					float magnitude = lhs.magnitude;
					if (magnitude > 0f)
					{
						lhs.Normalize();
						float f = Vector3.Dot(lhs, Vector3.left);
						float y = Mathf.Acos(f) * 57.29578f;
						if ((bool)CameraControls.Get())
						{
							CameraControls.Get().m_desiredRotationEulerAngles.y = y;
						}
					}
				}
			}
		}
		if (!reconnecting)
		{
			return;
		}
		object obj;
		if (CameraManager.Get() != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			obj = CameraManager.Get().GetIsometricCamera();
		}
		else
		{
			obj = null;
		}
		IsometricCamera isometricCamera = (IsometricCamera)obj;
		if (!(isometricCamera != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			isometricCamera.OnReconnect();
			return;
		}
	}

	private void UpdateHUD()
	{
		if (m_fogOfWar != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_fogOfWar.MarkForRecalculateVisibility();
		}
		if (!(HUD_UI.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.MarkFramesForForceUpdate();
			if (HUD_UI.Get().m_mainScreenPanel.m_spectatorHUD != null)
			{
				HUD_UI.Get().m_mainScreenPanel.m_spectatorHUD.SetTeamViewing(m_spectatingTeam);
			}
			return;
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		NetworkWriterAdapter networkWriterAdapter = new NetworkWriterAdapter(writer);
		networkWriterAdapter.Serialize(ref m_playerHandle);
		networkWriterAdapter.Serialize(ref m_playerIndex);
		m_player.OnSerializeHelper(networkWriterAdapter);
		return true;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		NetworkReaderAdapter networkReaderAdapter = new NetworkReaderAdapter(reader);
		networkReaderAdapter.Serialize(ref m_playerHandle);
		networkReaderAdapter.Serialize(ref m_playerIndex);
		m_player.OnSerializeHelper(networkReaderAdapter);
	}

	private void RestoreClientLastKnownStateOnReconnect()
	{
		Log.Info("restoring reconnected client's last known state {0}", GameFlowData.Get().gameState);
		if (GameFlowData.Get().gameState == GameState.EndingGame)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					AppState_GameTeardown.Get().Enter();
					return;
				}
			}
		}
		if (GameFlowData.Get().gameState == GameState.StartingGame)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (GameFlowData.Get().gameState == GameState.SpawningPlayers)
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (GameFlowData.Get().gameState == GameState.Deployment)
				{
					return;
				}
				UIScreenManager.Get().ClearAllPanels();
				UIScreenManager.Get().TryLoadAndSetupInGameUI();
				UIManager.SetGameObjectActive(HUD_UI.Get().m_textConsole, true);
				AppState_InGameDecision.Get().Enter();
				ActorData component = GetComponent<ActorData>();
				if (component != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					component.GetAbilityData().SpawnAndSetupCardsOnReconnect();
					component.SetupAbilityModOnReconnect();
					component.SetupForRespawnOnReconnect();
				}
				if (TeamSensitiveDataMatchmaker.Get() != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					TeamSensitiveDataMatchmaker.Get().SetTeamSensitiveDataForUnhandledActors();
				}
				SetupCamera(true);
				SetupHUD();
				return;
			}
		}
	}

	public Team GetTeamViewing()
	{
		if (GameFlowData.Get().LocalPlayerData == this)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return GameFlowData.Get().activeOwnedActorData.GetTeam();
					}
				}
			}
		}
		if (ActorData != null)
		{
			return ActorData.GetTeam();
		}
		return m_spectatingTeam;
	}

	public bool IsViewingTeam(Team targetTeam)
	{
		Team teamViewing = GetTeamViewing();
		int result;
		if (teamViewing != targetTeam)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = ((teamViewing == Team.Invalid) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public void CycleSpectatingTeam()
	{
		if (m_spectatingTeam != 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_spectatingTeam != Team.TeamB)
			{
				if (m_prevSpectatingTeam == Team.TeamA)
				{
					m_spectatingTeam = Team.TeamB;
				}
				else
				{
					m_spectatingTeam = Team.TeamA;
				}
				goto IL_005b;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_prevSpectatingTeam = m_spectatingTeam;
		m_spectatingTeam = Team.Invalid;
		goto IL_005b;
		IL_005b:
		UpdateHUD();
	}

	public void SetSpectatingTeam(Team team)
	{
		m_spectatingTeam = team;
		UpdateHUD();
	}

	[Command]
	internal void CmdTheatricsManagerUpdatePhaseEnded(int phaseCompleted, float phaseSeconds, float phaseDeltaSeconds)
	{
		TheatricsManager theatricsManager = TheatricsManager.Get();
		if (!(theatricsManager != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			theatricsManager.OnUpdatePhaseEnded(m_player.m_accountId, phaseCompleted, phaseSeconds, phaseDeltaSeconds);
			return;
		}
	}

	[Command]
	internal void CmdTutorialQueueEmpty()
	{
		SinglePlayerManager singlePlayerManager = SinglePlayerManager.Get();
		if (!(singlePlayerManager != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			singlePlayerManager.OnTutorialQueueEmpty();
			return;
		}
	}

	[Command]
	internal void CmdDebugEndGame(GameResult debugResult, int matchSeconds, int ggBoostUsedCount, bool ggBoostUsedToSelf, bool playWithFriendsBonus, bool playedLastTurn)
	{
		if (!HydrogenConfig.Get().AllowDebugCommands)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		if (ObjectivePoints.Get() != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					ObjectivePoints.Get()._001D(this, debugResult, matchSeconds, ggBoostUsedCount, ggBoostUsedToSelf, playWithFriendsBonus, playedLastTurn);
					return;
				}
			}
		}
		GameFlowData.Get().gameState = GameState.EndingGame;
	}

	[Command]
	internal void CmdSetPausedForDebugging(bool pause)
	{
		if (!HydrogenConfig.Get().AllowDebugCommands)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		GameFlowData.Get().SetPausedForDebugging(pause);
	}

	public override string ToString()
	{
		return $"[PlayerData: ({m_playerIndex}) {m_playerHandle}] {m_player}";
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeCmdCmdTheatricsManagerUpdatePhaseEnded(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("Command CmdTheatricsManagerUpdatePhaseEnded called on client.");
					return;
				}
			}
		}
		((PlayerData)obj).CmdTheatricsManagerUpdatePhaseEnded((int)reader.ReadPackedUInt32(), reader.ReadSingle(), reader.ReadSingle());
	}

	protected static void InvokeCmdCmdTutorialQueueEmpty(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdTutorialQueueEmpty called on client.");
		}
		else
		{
			((PlayerData)obj).CmdTutorialQueueEmpty();
		}
	}

	protected static void InvokeCmdCmdDebugEndGame(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDebugEndGame called on client.");
		}
		else
		{
			((PlayerData)obj).CmdDebugEndGame((GameResult)reader.ReadInt32(), (int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32(), reader.ReadBoolean(), reader.ReadBoolean(), reader.ReadBoolean());
		}
	}

	protected static void InvokeCmdCmdSetPausedForDebugging(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("Command CmdSetPausedForDebugging called on client.");
					return;
				}
			}
		}
		((PlayerData)obj).CmdSetPausedForDebugging(reader.ReadBoolean());
	}

	public void CallCmdTheatricsManagerUpdatePhaseEnded(int phaseCompleted, float phaseSeconds, float phaseDeltaSeconds)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("Command function CmdTheatricsManagerUpdatePhaseEnded called on server.");
					return;
				}
			}
		}
		if (base.isServer)
		{
			CmdTheatricsManagerUpdatePhaseEnded(phaseCompleted, phaseSeconds, phaseDeltaSeconds);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdTheatricsManagerUpdatePhaseEnded);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)phaseCompleted);
		networkWriter.Write(phaseSeconds);
		networkWriter.Write(phaseDeltaSeconds);
		SendCommandInternal(networkWriter, 0, "CmdTheatricsManagerUpdatePhaseEnded");
	}

	public void CallCmdTutorialQueueEmpty()
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdTutorialQueueEmpty called on server.");
			return;
		}
		if (base.isServer)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					CmdTutorialQueueEmpty();
					return;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdTutorialQueueEmpty);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, "CmdTutorialQueueEmpty");
	}

	public void CallCmdDebugEndGame(GameResult debugResult, int matchSeconds, int ggBoostUsedCount, bool ggBoostUsedToSelf, bool playWithFriendsBonus, bool playedLastTurn)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("Command function CmdDebugEndGame called on server.");
					return;
				}
			}
		}
		if (base.isServer)
		{
			CmdDebugEndGame(debugResult, matchSeconds, ggBoostUsedCount, ggBoostUsedToSelf, playWithFriendsBonus, playedLastTurn);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdDebugEndGame);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write((int)debugResult);
		networkWriter.WritePackedUInt32((uint)matchSeconds);
		networkWriter.WritePackedUInt32((uint)ggBoostUsedCount);
		networkWriter.Write(ggBoostUsedToSelf);
		networkWriter.Write(playWithFriendsBonus);
		networkWriter.Write(playedLastTurn);
		SendCommandInternal(networkWriter, 0, "CmdDebugEndGame");
	}

	public void CallCmdSetPausedForDebugging(bool pause)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("Command function CmdSetPausedForDebugging called on server.");
					return;
				}
			}
		}
		if (base.isServer)
		{
			CmdSetPausedForDebugging(pause);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdSetPausedForDebugging);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(pause);
		SendCommandInternal(networkWriter, 0, "CmdSetPausedForDebugging");
	}
}
