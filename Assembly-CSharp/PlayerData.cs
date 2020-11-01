using CameraManagerInternal;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerData : NetworkBehaviour
{
	[HideInInspector]
	public static int s_invalidPlayerIndex = -1;
	internal string m_playerHandle = "";
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

	private static int kCmdCmdTheatricsManagerUpdatePhaseEnded = -438226347;
	private static int kCmdCmdTutorialQueueEmpty = -1356677979;
	private static int kCmdCmdDebugEndGame = -1295019579;
	private static int kCmdCmdSetPausedForDebugging = -1571708758;

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
			if (m_playerIndex == s_invalidPlayerIndex
				&& value != s_invalidPlayerIndex
				&& GameFlowData.Get() != null
				&& MyNetworkManager.Get() != null)
			{
				m_playerIndex = value;
			}
		}
	}

	static PlayerData()
	{
		RegisterCommandDelegate(typeof(PlayerData), kCmdCmdTheatricsManagerUpdatePhaseEnded, InvokeCmdCmdTheatricsManagerUpdatePhaseEnded);
		RegisterCommandDelegate(typeof(PlayerData), kCmdCmdTutorialQueueEmpty, InvokeCmdCmdTutorialQueueEmpty);
		RegisterCommandDelegate(typeof(PlayerData), kCmdCmdDebugEndGame, InvokeCmdCmdDebugEndGame);
		RegisterCommandDelegate(typeof(PlayerData), kCmdCmdSetPausedForDebugging, InvokeCmdCmdSetPausedForDebugging);
		NetworkCRC.RegisterBehaviour("PlayerData", 0);
	}

	internal FogOfWar GetFogOfWar()
	{
		if (m_fogOfWar == null)
		{
			m_fogOfWar = GetComponent<FogOfWar>();
			if (m_fogOfWar == null)
			{
				m_fogOfWar = gameObject.AddComponent<FogOfWar>();
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
		string playerHandle = GameFlow.Get() != null
			? GameFlow.Get().GetPlayerHandleFromConnectionId(m_player.m_connectionId)
			: "";
		if (!string.IsNullOrEmpty(playerHandle))
		{
			m_playerHandle = playerHandle;
		}
	}

	public Player GetPlayer()
	{
		return m_player;
	}

	public PlayerDetails LookupDetails()
	{
		GameFlow.Get().playerDetails.TryGetValue(m_player, out PlayerDetails details);
		return details;
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
		if (GameFlowData.Get() != null)
		{
			GameFlowData.Get().RemoveExistingPlayer(base.gameObject);
		}
	}

	public void Update()
	{
		if (m_reconnecting)
		{
			RestoreClientLastKnownStateOnReconnect();
			m_reconnecting = false;
		}
		if (m_isLocal
			&& ActorData == null
			&& InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.CycleTeam))
		{
			CycleSpectatingTeam();
		}
	}

	public override void OnStartClient()
	{
		base.OnStartClient();
		if (NetworkClient.active
			&& !NetworkServer.active
			&& GameFlow.Get().playerDetails.TryGetValue(m_player, out PlayerDetails details)
			&& details.IsLocal())
		{
			ClientGameManager.Get().Client.Send(
				(int)MyMsgType.ClientRequestTimeUpdate,
				new GameManager.PlayerObjectStartedOnClientNotification
				{
					AccountId = GameManager.Get().PlayerInfo.AccountId,
					PlayerId = GameManager.Get().PlayerInfo.PlayerId
				});
			m_isLocal = true;
			m_reconnecting = true;
		}
	}

	public override void OnStartLocalPlayer()
	{
		if (NetworkServer.active)
		{
			GetComponent<NetworkIdentity>().RebuildObservers(true);
		}
		Log.Info("ActorData.OnStartClient: local player");
		m_isLocal = true;
		ClientGameManager.Get().PlayerObjectStartedOnClient = true;
		Log.Info("HEALTHBARCHECK: IS STARTED: " + ClientGameManager.Get().DesignSceneStarted);
		if (ClientGameManager.Get().DesignSceneStarted)
		{
			UIScreenManager.Get().TryLoadAndSetupInGameUI();
		}
		ClientGameManager.Get().CheckAndSendClientPreparedForGameStartNotification();
		string playerHandle = GameFlow.Get() != null
			? GameFlow.Get().GetPlayerHandleFromConnectionId(m_player.m_connectionId)
			: "";
		if (!string.IsNullOrEmpty(playerHandle))
		{
			m_playerHandle = playerHandle;
		}
		SetupCamera();
		SetupHUD();
	}

	private void SetupHUD()
	{
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_abilityBar.Setup(ActorData);
			HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.ProcessTeams();
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.MarkFramesForForceUpdate();
			if (HUD_UI.Get().m_mainScreenPanel.m_spectatorHUD != null)
			{
				UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_spectatorHUD, ActorData == null);
			}
		}
	}

	private void SetupCamera(bool reconnecting = false)
	{
		if (ActorData == null && SpawnPointManager.Get() != null)
		{
			List<BoardSquare> squaresInRegionA = SpawnPointManager.Get().m_initialSpawnPointsTeamA.GetSquaresInRegion();
			List<BoardSquare> squaresInRegionB = SpawnPointManager.Get().m_initialSpawnPointsTeamB.GetSquaresInRegion();
			if (squaresInRegionA.Count > 0 && squaresInRegionB.Count > 0)
			{
				Vector3 positionA = squaresInRegionA[0].transform.position;
				Vector3 positionB = squaresInRegionB[0].transform.position;
				Vector3 lhs = positionA - positionB;
				lhs.y = 0f;
				if (Mathf.Abs(lhs.x) > Mathf.Abs(lhs.z))
				{
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
					if (CameraControls.Get() != null)
					{
						CameraControls.Get().m_desiredRotationEulerAngles.y = y;
					}
				}
			}
		}
		if (reconnecting)
		{
			IsometricCamera isometricCamera = CameraManager.Get()?.GetIsometricCamera();
			if (isometricCamera != null)
			{
				isometricCamera.OnReconnect();
			}
		}
	}

	private void UpdateHUD()
	{
		if (m_fogOfWar != null)
		{
			m_fogOfWar.MarkForRecalculateVisibility();
		}
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.MarkFramesForForceUpdate();
			if (HUD_UI.Get().m_mainScreenPanel.m_spectatorHUD != null)
			{
				HUD_UI.Get().m_mainScreenPanel.m_spectatorHUD.SetTeamViewing(m_spectatingTeam);
			}
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
		LogJson();
	}

	private void LogJson()
	{
		Log.Info($"[JSON] {{\"playerData\":{{" +
			$"\"playerHandle\": \"{m_playerHandle}\"," +
			$"\"playerIndex\": {m_playerIndex}," +
			$"\"valid\": {DefaultJsonSerializer.Serialize(m_player.m_valid)}," +
			$"\"id\": {m_player.m_id}," +
			$"\"connectionId\": {m_player.m_connectionId}" +
			$"}}}}");
	}

	private void RestoreClientLastKnownStateOnReconnect()
	{
		Log.Info($"restoring reconnected client's last known state {GameFlowData.Get().gameState}");
		switch (GameFlowData.Get().gameState)
		{
			case GameState.EndingGame:
				AppState_GameTeardown.Get().Enter();
				break;
			case GameState.StartingGame:
			case GameState.SpawningPlayers:
			case GameState.Deployment:
				break;
			default:
				UIScreenManager.Get().ClearAllPanels();
				UIScreenManager.Get().TryLoadAndSetupInGameUI();
				UIManager.SetGameObjectActive(HUD_UI.Get().m_textConsole, true);
				AppState_InGameDecision.Get().Enter();
				ActorData component = GetComponent<ActorData>();
				if (component != null)
				{
					component.GetAbilityData().SpawnAndSetupCardsOnReconnect();
					component.SetupAbilityModOnReconnect();
					component.SetupForRespawnOnReconnect();
				}
				if (TeamSensitiveDataMatchmaker.Get() != null)
				{
					TeamSensitiveDataMatchmaker.Get().SetTeamSensitiveDataForUnhandledActors();
				}
				SetupCamera(true);
				SetupHUD();
				break;
		}
	}

	public Team GetTeamViewing()
	{
		if (GameFlowData.Get().LocalPlayerData == this
			&& GameFlowData.Get().activeOwnedActorData != null)
		{
			return GameFlowData.Get().activeOwnedActorData.GetTeam();
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
		return teamViewing == targetTeam || teamViewing == Team.Invalid;
	}

	public void CycleSpectatingTeam()
	{
		if (m_spectatingTeam != Team.TeamA && m_spectatingTeam != Team.TeamB)
		{
			if (m_prevSpectatingTeam == Team.TeamA)
			{
				m_spectatingTeam = Team.TeamB;
			}
			else
			{
				m_spectatingTeam = Team.TeamA;
			}
		}
		else
		{
			m_prevSpectatingTeam = m_spectatingTeam;
			m_spectatingTeam = Team.Invalid;
		}
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
		Log.Info($"[JSON] {{\"CmdTheatricsManagerUpdatePhaseEnded\":{{" +
			$"\"phaseCompleted\":{DefaultJsonSerializer.Serialize(phaseCompleted)}," +
			$"\"phaseSeconds\":{DefaultJsonSerializer.Serialize(phaseSeconds)}," +
			$"\"phaseDeltaSeconds\":{DefaultJsonSerializer.Serialize(phaseDeltaSeconds)}}}}}");
		TheatricsManager theatricsManager = TheatricsManager.Get();
		if (theatricsManager != null)
		{
			theatricsManager.OnUpdatePhaseEnded(m_player.m_accountId, phaseCompleted, phaseSeconds, phaseDeltaSeconds);
		}
	}

	[Command]
	internal void CmdTutorialQueueEmpty()
	{
		Log.Info($"[JSON] {{\"CmdTutorialQueueEmpty\":{{}}}}");
		SinglePlayerManager singlePlayerManager = SinglePlayerManager.Get();
		if (singlePlayerManager != null)
		{
			singlePlayerManager.OnTutorialQueueEmpty();
		}
	}

	[Command]
	internal void CmdDebugEndGame(GameResult debugResult, int matchSeconds, int ggBoostUsedCount, bool ggBoostUsedToSelf, bool playWithFriendsBonus, bool playedLastTurn)
	{
		Log.Info($"[JSON] {{\"CmdDebugEndGame\":{{" +
			$"\"debugResult\":{DefaultJsonSerializer.Serialize(debugResult)}," +
			$"\"matchSeconds\":{DefaultJsonSerializer.Serialize(matchSeconds)}," +
			$"\"ggBoostUsedCount\":{DefaultJsonSerializer.Serialize(ggBoostUsedCount)}," +
			$"\"ggBoostUsedToSelf\":{DefaultJsonSerializer.Serialize(ggBoostUsedToSelf)}," +
			$"\"playWithFriendsBonus\":{DefaultJsonSerializer.Serialize(playWithFriendsBonus)}," +
			$"\"playedLastTurn\":{DefaultJsonSerializer.Serialize(playedLastTurn)}}}}}");
		if (!HydrogenConfig.Get().AllowDebugCommands)
		{
			return;
		}
		if (ObjectivePoints.Get() != null)
		{
			ObjectivePoints.Get().DebugEndGame(this, debugResult, matchSeconds, ggBoostUsedCount, ggBoostUsedToSelf, playWithFriendsBonus, playedLastTurn);
		}
		else
		{
			GameFlowData.Get().gameState = GameState.EndingGame;
		}
	}

	[Command]
	internal void CmdSetPausedForDebugging(bool pause)
	{
		Log.Info($"[JSON] {{\"CmdSetPausedForDebugging\":{{\"pause\":{DefaultJsonSerializer.Serialize(pause)}}}}}");
		if (!HydrogenConfig.Get().AllowDebugCommands)
		{
			return;
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
			Debug.LogError("Command CmdTheatricsManagerUpdatePhaseEnded called on client.");
			return;
		}
		((PlayerData)obj).CmdTheatricsManagerUpdatePhaseEnded((int)reader.ReadPackedUInt32(), reader.ReadSingle(), reader.ReadSingle());
	}

	protected static void InvokeCmdCmdTutorialQueueEmpty(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdTutorialQueueEmpty called on client.");
			return;
		}
		((PlayerData)obj).CmdTutorialQueueEmpty();
	}

	protected static void InvokeCmdCmdDebugEndGame(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDebugEndGame called on client.");
			return;
		}
		((PlayerData)obj).CmdDebugEndGame((GameResult)reader.ReadInt32(), (int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32(), reader.ReadBoolean(), reader.ReadBoolean(), reader.ReadBoolean());
	}

	protected static void InvokeCmdCmdSetPausedForDebugging(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetPausedForDebugging called on client.");
			return;
		}
		((PlayerData)obj).CmdSetPausedForDebugging(reader.ReadBoolean());
	}

	public void CallCmdTheatricsManagerUpdatePhaseEnded(int phaseCompleted, float phaseSeconds, float phaseDeltaSeconds)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdTheatricsManagerUpdatePhaseEnded called on server.");
			return;
		}
		if (base.isServer)
		{
			CmdTheatricsManagerUpdatePhaseEnded(phaseCompleted, phaseSeconds, phaseDeltaSeconds);
		}
		else
		{
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
			CmdTutorialQueueEmpty();
		}
		else
		{
			NetworkWriter networkWriter = new NetworkWriter();
			networkWriter.Write((short)0);
			networkWriter.Write((short)5);
			networkWriter.WritePackedUInt32((uint)kCmdCmdTutorialQueueEmpty);
			networkWriter.Write(GetComponent<NetworkIdentity>().netId);
			SendCommandInternal(networkWriter, 0, "CmdTutorialQueueEmpty");
		}
	}

	public void CallCmdDebugEndGame(GameResult debugResult, int matchSeconds, int ggBoostUsedCount, bool ggBoostUsedToSelf, bool playWithFriendsBonus, bool playedLastTurn)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdDebugEndGame called on server.");
			return;
		}
		if (base.isServer)
		{
			CmdDebugEndGame(debugResult, matchSeconds, ggBoostUsedCount, ggBoostUsedToSelf, playWithFriendsBonus, playedLastTurn);
		}
		else
		{
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
	}

	public void CallCmdSetPausedForDebugging(bool pause)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdSetPausedForDebugging called on server.");
			return;
		}
		if (base.isServer)
		{
			CmdSetPausedForDebugging(pause);
		}
		else
		{
			NetworkWriter networkWriter = new NetworkWriter();
			networkWriter.Write((short)0);
			networkWriter.Write((short)5);
			networkWriter.WritePackedUInt32((uint)kCmdCmdSetPausedForDebugging);
			networkWriter.Write(GetComponent<NetworkIdentity>().netId);
			networkWriter.Write(pause);
			SendCommandInternal(networkWriter, 0, "CmdSetPausedForDebugging");
		}
	}
}
