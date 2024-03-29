﻿// ROGUES
// SERVER
//using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CameraManagerInternal;
//using Mirror;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerData : NetworkBehaviour
{
	[HideInInspector]
	public static int s_invalidPlayerIndex = -1;
	//[SyncVar]
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
	//[SyncVar]
	private int m_playerIndex = s_invalidPlayerIndex;

	// removed in rogues
	private static int kCmdCmdTheatricsManagerUpdatePhaseEnded = -438226347;
	// removed in rogues
	private static int kCmdCmdTutorialQueueEmpty = -1356677979;
	// removed in rogues
	private static int kCmdCmdDebugEndGame = -1295019579;
	// removed in rogues
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
				// reactor
				m_playerIndex = value;
				// rogues
				//Networkm_playerIndex = value;
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
		// rogues
		//NetworkBehaviour.RegisterCommandDelegate(typeof(PlayerData), "CmdTheatricsManagerUpdatePhaseEnded", new NetworkBehaviour.CmdDelegate(InvokeCmdCmdTheatricsManagerUpdatePhaseEnded));
		//NetworkBehaviour.RegisterCommandDelegate(typeof(PlayerData), "CmdTutorialQueueEmpty", new NetworkBehaviour.CmdDelegate(InvokeCmdCmdTutorialQueueEmpty));
		//NetworkBehaviour.RegisterCommandDelegate(typeof(PlayerData), "CmdDebugEndGame", new NetworkBehaviour.CmdDelegate(InvokeCmdCmdDebugEndGame));
		//NetworkBehaviour.RegisterCommandDelegate(typeof(PlayerData), "CmdSetPausedForDebugging", new NetworkBehaviour.CmdDelegate(InvokeCmdCmdSetPausedForDebugging));
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
			// reactor
			m_playerHandle = playerHandle;
			// rogues
			//Networkm_playerHandle = playerHandle;
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

	// removed in rogues
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

		// removed in rogues
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
		Log.Info($"ActorData.OnStartClient: local player netId:{GetComponent<NetworkIdentity>().netId} obj:{gameObject.name}"); // log message expanded in rogues
		m_isLocal = true;
		ClientGameManager.Get().PlayerObjectStartedOnClient = true;
		Log.Info("HEALTHBARCHECK: IS STARTED: " + ClientGameManager.Get().DesignSceneStarted); // removed in rogues
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
			// reactor
			m_playerHandle = playerHandle;
			// rogues
			//Networkm_playerHandle = playerHandle;
		}
		// reactor
		SetupCamera();
		// rogues
		//SetupCamera(true);
		SetupHUD();
	}

	private void SetupHUD()
	{
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_abilityBar.Setup(ActorData);
			HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.ProcessTeams(); // removed in rogues
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.MarkFramesForForceUpdate();
			if (HUD_UI.Get().m_mainScreenPanel.m_spectatorHUD != null)
			{
				// reactor
				UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_spectatorHUD, ActorData == null);
				// rogues
				//HUD_UI.Get().m_mainScreenPanel.m_spectatorHUD.gameObject.SetActive(ActorData == null);
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
			// removed in rogues
			if (HUD_UI.Get().m_mainScreenPanel.m_spectatorHUD != null)
			{
				HUD_UI.Get().m_mainScreenPanel.m_spectatorHUD.SetTeamViewing(m_spectatingTeam);
			}
		}
	}

	// reactor
	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		NetworkWriterAdapter networkWriterAdapter = new NetworkWriterAdapter(writer);
		networkWriterAdapter.Serialize(ref m_playerHandle);
		networkWriterAdapter.Serialize(ref m_playerIndex);
		m_player.OnSerializeHelper(networkWriterAdapter);
		return true;
	}

	// reactor
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		NetworkReaderAdapter networkReaderAdapter = new NetworkReaderAdapter(reader);
		networkReaderAdapter.Serialize(ref m_playerHandle);
		networkReaderAdapter.Serialize(ref m_playerIndex);
		m_player.OnSerializeHelper(networkReaderAdapter);
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
				if (HUD_UI.Get().m_textConsole != null) // check added in rogues
				{
					// reactor
					UIManager.SetGameObjectActive(HUD_UI.Get().m_textConsole, true);
					// rogues
					//HUD_UI.Get().m_textConsole.gameObject.SetActive(true);
				}
				AppState_InGameDecision.Get().Enter();
				ActorData component = GetComponent<ActorData>();
				if (component != null)
				{
					component.GetAbilityData().SpawnAndSetupCardsOnReconnect();
					component.SetupAbilityModOnReconnect();
					component.SetupForRespawnOnReconnect();
					// rogues
					//component.SetupAbilityGearOnReconnect();
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

	// added in rogues
#if SERVER
	public bool IsReconnecting()
	{
		if (!m_player.m_valid)
		{
			return false;
		}
		long accountId = GameFlow.Get().playerDetails[m_player].m_accountId;
		return ServerGameManager.Get().IsAccountReconnecting(accountId);
	}
#endif

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
		TheatricsManager theatricsManager = TheatricsManager.Get();
		if (theatricsManager != null)
		{
			theatricsManager.OnUpdatePhaseEnded(m_player.m_accountId, phaseCompleted, phaseSeconds, phaseDeltaSeconds);
		}
	}

	[Command]
	internal void CmdTutorialQueueEmpty()
	{
		SinglePlayerManager singlePlayerManager = SinglePlayerManager.Get();
		if (singlePlayerManager != null)
		{
			singlePlayerManager.OnTutorialQueueEmpty();
		}
	}

	[Command]
	internal void CmdDebugEndGame(GameResult debugResult, int matchSeconds, int ggBoostUsedCount, bool ggBoostUsedToSelf, bool playWithFriendsBonus, bool playedLastTurn)
	{
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

	// reactor
	private void UNetVersion()
	{
	}
	// rogues
	//private void MirrorProcessed()
	//{
	//}

	// added in rogues
	//public string Networkm_playerHandle
	//{
	//	get
	//	{
	//		return m_playerHandle;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<string>(value, ref m_playerHandle, 1U);
	//	}
	//}

	// added in rogues
	//public int Networkm_playerIndex
	//{
	//	get
	//	{
	//		return m_playerIndex;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<int>(value, ref m_playerIndex, 2U);
	//	}
	//}

	protected static void InvokeCmdCmdTheatricsManagerUpdatePhaseEnded(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdTheatricsManagerUpdatePhaseEnded called on client.");
			return;
		}
		// reactor
		((PlayerData)obj).CmdTheatricsManagerUpdatePhaseEnded((int)reader.ReadPackedUInt32(), reader.ReadSingle(), reader.ReadSingle());
		// rogues
		//((PlayerData)obj).CmdTheatricsManagerUpdatePhaseEnded(reader.ReadPackedInt32(), reader.ReadSingle(), reader.ReadSingle());
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
		// reactor
		((PlayerData)obj).CmdDebugEndGame((GameResult)reader.ReadInt32(), (int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32(), reader.ReadBoolean(), reader.ReadBoolean(), reader.ReadBoolean());
		// rogues
		//((PlayerData)obj).CmdDebugEndGame((GameResult)reader.ReadPackedInt32(), reader.ReadPackedInt32(), reader.ReadPackedInt32(), reader.ReadBoolean(), reader.ReadBoolean(), reader.ReadBoolean());
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
		if (!NetworkClient.active)  // removed in rogues
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
			// reactor
			NetworkWriter networkWriter = new NetworkWriter();
			networkWriter.Write((short)0);
			networkWriter.Write((short)5);
			networkWriter.WritePackedUInt32((uint)kCmdCmdTheatricsManagerUpdatePhaseEnded);
			networkWriter.Write(GetComponent<NetworkIdentity>().netId);
			networkWriter.WritePackedUInt32((uint)phaseCompleted);
			networkWriter.Write(phaseSeconds);
			networkWriter.Write(phaseDeltaSeconds);
			SendCommandInternal(networkWriter, 0, "CmdTheatricsManagerUpdatePhaseEnded");
			// rogues
			//NetworkWriter networkWriter = new NetworkWriter();
			//networkWriter.WritePackedInt32(phaseCompleted);
			//networkWriter.Write(phaseSeconds);
			//networkWriter.Write(phaseDeltaSeconds);
			//base.SendCommandInternal(typeof(PlayerData), "CmdTheatricsManagerUpdatePhaseEnded", networkWriter, 0);
		}
	}

	public void CallCmdTutorialQueueEmpty()
	{
		if (!NetworkClient.active)  // removed in rogues
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
			// reactor
			NetworkWriter networkWriter = new NetworkWriter();
			networkWriter.Write((short)0);
			networkWriter.Write((short)5);
			networkWriter.WritePackedUInt32((uint)kCmdCmdTutorialQueueEmpty);
			networkWriter.Write(GetComponent<NetworkIdentity>().netId);
			SendCommandInternal(networkWriter, 0, "CmdTutorialQueueEmpty");
			// rogues
			//NetworkWriter networkWriter = new NetworkWriter();
			//base.SendCommandInternal(typeof(PlayerData), "CmdTutorialQueueEmpty", networkWriter, 0);
		}
	}

	public void CallCmdDebugEndGame(GameResult debugResult, int matchSeconds, int ggBoostUsedCount, bool ggBoostUsedToSelf, bool playWithFriendsBonus, bool playedLastTurn)
	{
		if (!NetworkClient.active)  // removed in rogues
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
			// reactor
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
			// rogues
			//NetworkWriter networkWriter = new NetworkWriter();
			//networkWriter.WritePackedInt32((int)debugResult);
			//networkWriter.WritePackedInt32(matchSeconds);
			//networkWriter.WritePackedInt32(ggBoostUsedCount);
			//networkWriter.Write(ggBoostUsedToSelf);
			//networkWriter.Write(playWithFriendsBonus);
			//networkWriter.Write(playedLastTurn);
			//base.SendCommandInternal(typeof(PlayerData), "CmdDebugEndGame", networkWriter, 0);
		}
	}

	public void CallCmdSetPausedForDebugging(bool pause)
	{
		if (!NetworkClient.active)  // removed in rogues
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
			// reactor
			NetworkWriter networkWriter = new NetworkWriter();
			networkWriter.Write((short)0);
			networkWriter.Write((short)5);
			networkWriter.WritePackedUInt32((uint)kCmdCmdSetPausedForDebugging);
			networkWriter.Write(GetComponent<NetworkIdentity>().netId);
			networkWriter.Write(pause);
			SendCommandInternal(networkWriter, 0, "CmdSetPausedForDebugging");
			// rogues
			//NetworkWriter networkWriter = new NetworkWriter();
			//networkWriter.Write(pause);
			//base.SendCommandInternal(typeof(PlayerData), "CmdSetPausedForDebugging", networkWriter, 0);
		}
	}
}
