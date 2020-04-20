using System;
using System.Collections.Generic;
using CameraManagerInternal;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerData : NetworkBehaviour
{
	[HideInInspector]
	public static int s_invalidPlayerIndex = -1;

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

	private int m_playerIndex = PlayerData.s_invalidPlayerIndex;

	private static int kCmdCmdTheatricsManagerUpdatePhaseEnded = -0x1A1ECDAB;

	private static int kCmdCmdTutorialQueueEmpty;

	private static int kCmdCmdDebugEndGame;

	private static int kCmdCmdSetPausedForDebugging;

	static PlayerData()
	{
		NetworkBehaviour.RegisterCommandDelegate(typeof(PlayerData), PlayerData.kCmdCmdTheatricsManagerUpdatePhaseEnded, new NetworkBehaviour.CmdDelegate(PlayerData.InvokeCmdCmdTheatricsManagerUpdatePhaseEnded));
		PlayerData.kCmdCmdTutorialQueueEmpty = -0x50DD435B;
		NetworkBehaviour.RegisterCommandDelegate(typeof(PlayerData), PlayerData.kCmdCmdTutorialQueueEmpty, new NetworkBehaviour.CmdDelegate(PlayerData.InvokeCmdCmdTutorialQueueEmpty));
		PlayerData.kCmdCmdDebugEndGame = -0x4D306E3B;
		NetworkBehaviour.RegisterCommandDelegate(typeof(PlayerData), PlayerData.kCmdCmdDebugEndGame, new NetworkBehaviour.CmdDelegate(PlayerData.InvokeCmdCmdDebugEndGame));
		PlayerData.kCmdCmdSetPausedForDebugging = -0x5DAE5F56;
		NetworkBehaviour.RegisterCommandDelegate(typeof(PlayerData), PlayerData.kCmdCmdSetPausedForDebugging, new NetworkBehaviour.CmdDelegate(PlayerData.InvokeCmdCmdSetPausedForDebugging));
		NetworkCRC.RegisterBehaviour("PlayerData", 0);
	}

	internal string PlayerHandle
	{
		get
		{
			return this.m_playerHandle;
		}
	}

	public bool IsLocal
	{
		get
		{
			return this.m_isLocal;
		}
	}

	internal FogOfWar GetFogOfWar()
	{
		if (this.m_fogOfWar == null)
		{
			this.m_fogOfWar = base.GetComponent<FogOfWar>();
			if (this.m_fogOfWar == null)
			{
				this.m_fogOfWar = base.gameObject.AddComponent<FogOfWar>();
			}
		}
		return this.m_fogOfWar;
	}

	public void SetPlayerOnReconnect(Player agent)
	{
		this.m_player = agent;
		NetworkIdentity component = base.GetComponent<NetworkIdentity>();
		if (component != null)
		{
			component.RebuildObservers(true);
		}
		string text;
		if (GameFlow.Get() != null)
		{
			text = GameFlow.Get().GetPlayerHandleFromConnectionId(this.m_player.m_connectionId);
		}
		else
		{
			text = string.Empty;
		}
		string text2 = text;
		if (!string.IsNullOrEmpty(text2))
		{
			this.m_playerHandle = text2;
		}
	}

	public Player GetPlayer()
	{
		return this.m_player;
	}

	public int PlayerIndex
	{
		get
		{
			return this.m_playerIndex;
		}
		set
		{
			if (this.m_playerIndex == PlayerData.s_invalidPlayerIndex)
			{
				if (value != PlayerData.s_invalidPlayerIndex)
				{
					if (GameFlowData.Get() != null && MyNetworkManager.Get() != null)
					{
						this.m_playerIndex = value;
					}
				}
			}
		}
	}

	public PlayerDetails LookupDetails()
	{
		PlayerDetails result = null;
		GameFlow.Get().playerDetails.TryGetValue(this.m_player, out result);
		return result;
	}

	public void Awake()
	{
		this.ActorData = base.GetComponent<ActorData>();
	}

	public void Start()
	{
		this.m_spectatingTeam = Team.Invalid;
		this.m_prevSpectatingTeam = Team.Invalid;
		GameFlowData.Get().AddPlayer(base.gameObject);
		this.GetFogOfWar().MarkForRecalculateVisibility();
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
		if (this.m_reconnecting)
		{
			this.RestoreClientLastKnownStateOnReconnect();
			this.m_reconnecting = false;
		}
		if (this.m_isLocal)
		{
			if (!this.ActorData)
			{
				if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.CycleTeam))
				{
					this.CycleSpectatingTeam();
				}
			}
		}
	}

	public override void OnStartClient()
	{
		base.OnStartClient();
		if (NetworkClient.active)
		{
			if (!NetworkServer.active)
			{
				PlayerDetails playerDetails = null;
				if (GameFlow.Get().playerDetails.TryGetValue(this.m_player, out playerDetails) && playerDetails.IsLocal())
				{
					GameManager.PlayerObjectStartedOnClientNotification playerObjectStartedOnClientNotification = new GameManager.PlayerObjectStartedOnClientNotification();
					playerObjectStartedOnClientNotification.AccountId = GameManager.Get().PlayerInfo.AccountId;
					playerObjectStartedOnClientNotification.PlayerId = GameManager.Get().PlayerInfo.PlayerId;
					ClientGameManager.Get().Client.Send(0x41, playerObjectStartedOnClientNotification);
					this.m_isLocal = true;
					this.m_reconnecting = true;
				}
			}
		}
	}

	public override void OnStartLocalPlayer()
	{
		if (NetworkServer.active)
		{
			NetworkIdentity component = base.GetComponent<NetworkIdentity>();
			component.RebuildObservers(true);
		}
		Log.Info("ActorData.OnStartClient: local player", new object[0]);
		this.m_isLocal = true;
		ClientGameManager.Get().PlayerObjectStartedOnClient = true;
		Log.Info("HEALTHBARCHECK: IS STARTED: " + ClientGameManager.Get().DesignSceneStarted, new object[0]);
		if (ClientGameManager.Get().DesignSceneStarted)
		{
			UIScreenManager.Get().TryLoadAndSetupInGameUI();
		}
		ClientGameManager.Get().CheckAndSendClientPreparedForGameStartNotification();
		string text;
		if (GameFlow.Get() != null)
		{
			text = GameFlow.Get().GetPlayerHandleFromConnectionId(this.m_player.m_connectionId);
		}
		else
		{
			text = string.Empty;
		}
		string text2 = text;
		if (!string.IsNullOrEmpty(text2))
		{
			this.m_playerHandle = text2;
		}
		this.SetupCamera(false);
		this.SetupHUD();
	}

	private void SetupHUD()
	{
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_abilityBar.Setup(this.ActorData);
			HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.ProcessTeams();
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.MarkFramesForForceUpdate();
			if (HUD_UI.Get().m_mainScreenPanel.m_spectatorHUD != null)
			{
				UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_spectatorHUD, this.ActorData == null, null);
			}
		}
	}

	private void SetupCamera(bool reconnecting = false)
	{
		if (this.ActorData == null)
		{
			if (SpawnPointManager.Get() != null)
			{
				List<BoardSquare> squaresInRegion = SpawnPointManager.Get().m_initialSpawnPointsTeamA.GetSquaresInRegion();
				List<BoardSquare> squaresInRegion2 = SpawnPointManager.Get().m_initialSpawnPointsTeamB.GetSquaresInRegion();
				if (squaresInRegion.Count > 0 && squaresInRegion2.Count > 0)
				{
					Vector3 position = squaresInRegion[0].transform.position;
					Vector3 position2 = squaresInRegion2[0].transform.position;
					Vector3 lhs = position - position2;
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
						if (CameraControls.Get())
						{
							CameraControls.Get().m_desiredRotationEulerAngles.y = y;
						}
					}
				}
			}
		}
		if (reconnecting)
		{
			IsometricCamera isometricCamera;
			if (CameraManager.Get() != null)
			{
				isometricCamera = CameraManager.Get().GetIsometricCamera();
			}
			else
			{
				isometricCamera = null;
			}
			IsometricCamera isometricCamera2 = isometricCamera;
			if (isometricCamera2 != null)
			{
				isometricCamera2.OnReconnect();
			}
		}
	}

	private void UpdateHUD()
	{
		if (this.m_fogOfWar != null)
		{
			this.m_fogOfWar.MarkForRecalculateVisibility();
		}
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.MarkFramesForForceUpdate();
			if (HUD_UI.Get().m_mainScreenPanel.m_spectatorHUD != null)
			{
				HUD_UI.Get().m_mainScreenPanel.m_spectatorHUD.SetTeamViewing(this.m_spectatingTeam);
			}
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		NetworkWriterAdapter networkWriterAdapter = new NetworkWriterAdapter(writer);
		networkWriterAdapter.Serialize(ref this.m_playerHandle);
		networkWriterAdapter.Serialize(ref this.m_playerIndex);
		this.m_player.OnSerializeHelper(networkWriterAdapter);
		return true;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		NetworkReaderAdapter networkReaderAdapter = new NetworkReaderAdapter(reader);
		networkReaderAdapter.Serialize(ref this.m_playerHandle);
		networkReaderAdapter.Serialize(ref this.m_playerIndex);
		this.m_player.OnSerializeHelper(networkReaderAdapter);
	}

	private void RestoreClientLastKnownStateOnReconnect()
	{
		Log.Info("restoring reconnected client's last known state {0}", new object[]
		{
			GameFlowData.Get().gameState
		});
		if (GameFlowData.Get().gameState == GameState.EndingGame)
		{
			AppState_GameTeardown.Get().Enter();
		}
		else if (GameFlowData.Get().gameState != GameState.StartingGame)
		{
			if (GameFlowData.Get().gameState != GameState.SpawningPlayers)
			{
				if (GameFlowData.Get().gameState != GameState.Deployment)
				{
					UIScreenManager.Get().ClearAllPanels();
					UIScreenManager.Get().TryLoadAndSetupInGameUI();
					UIManager.SetGameObjectActive(HUD_UI.Get().m_textConsole, true, null);
					AppState_InGameDecision.Get().Enter();
					ActorData component = base.GetComponent<ActorData>();
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
					this.SetupCamera(true);
					this.SetupHUD();
				}
			}
		}
	}

	public Team GetTeamViewing()
	{
		if (GameFlowData.Get().LocalPlayerData == this)
		{
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				return GameFlowData.Get().activeOwnedActorData.GetTeam();
			}
		}
		if (this.ActorData != null)
		{
			return this.ActorData.GetTeam();
		}
		return this.m_spectatingTeam;
	}

	public bool IsViewingTeam(Team targetTeam)
	{
		Team teamViewing = this.GetTeamViewing();
		bool result;
		if (teamViewing != targetTeam)
		{
			result = (teamViewing == Team.Invalid);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public void CycleSpectatingTeam()
	{
		if (this.m_spectatingTeam != Team.TeamA)
		{
			if (this.m_spectatingTeam == Team.TeamB)
			{
			}
			else
			{
				if (this.m_prevSpectatingTeam == Team.TeamA)
				{
					this.m_spectatingTeam = Team.TeamB;
					goto IL_5B;
				}
				this.m_spectatingTeam = Team.TeamA;
				goto IL_5B;
			}
		}
		this.m_prevSpectatingTeam = this.m_spectatingTeam;
		this.m_spectatingTeam = Team.Invalid;
		IL_5B:
		this.UpdateHUD();
	}

	public void SetSpectatingTeam(Team team)
	{
		this.m_spectatingTeam = team;
		this.UpdateHUD();
	}

	[Command]
	internal void CmdTheatricsManagerUpdatePhaseEnded(int phaseCompleted, float phaseSeconds, float phaseDeltaSeconds)
	{
		TheatricsManager theatricsManager = TheatricsManager.Get();
		if (theatricsManager != null)
		{
			theatricsManager.OnUpdatePhaseEnded(this.m_player.m_accountId, phaseCompleted, phaseSeconds, phaseDeltaSeconds);
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
			ObjectivePoints.Get().symbol_001D(this, debugResult, matchSeconds, ggBoostUsedCount, ggBoostUsedToSelf, playWithFriendsBonus, playedLastTurn);
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
		return string.Format("[PlayerData: ({0}) {1}] {2}", this.m_playerIndex, this.m_playerHandle, this.m_player);
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
			this.CmdTheatricsManagerUpdatePhaseEnded(phaseCompleted, phaseSeconds, phaseDeltaSeconds);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)PlayerData.kCmdCmdTheatricsManagerUpdatePhaseEnded);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)phaseCompleted);
		networkWriter.Write(phaseSeconds);
		networkWriter.Write(phaseDeltaSeconds);
		base.SendCommandInternal(networkWriter, 0, "CmdTheatricsManagerUpdatePhaseEnded");
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
			this.CmdTutorialQueueEmpty();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)PlayerData.kCmdCmdTutorialQueueEmpty);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		base.SendCommandInternal(networkWriter, 0, "CmdTutorialQueueEmpty");
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
			this.CmdDebugEndGame(debugResult, matchSeconds, ggBoostUsedCount, ggBoostUsedToSelf, playWithFriendsBonus, playedLastTurn);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)PlayerData.kCmdCmdDebugEndGame);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.Write((int)debugResult);
		networkWriter.WritePackedUInt32((uint)matchSeconds);
		networkWriter.WritePackedUInt32((uint)ggBoostUsedCount);
		networkWriter.Write(ggBoostUsedToSelf);
		networkWriter.Write(playWithFriendsBonus);
		networkWriter.Write(playedLastTurn);
		base.SendCommandInternal(networkWriter, 0, "CmdDebugEndGame");
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
			this.CmdSetPausedForDebugging(pause);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)PlayerData.kCmdCmdSetPausedForDebugging);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(pause);
		base.SendCommandInternal(networkWriter, 0, "CmdSetPausedForDebugging");
	}
}
