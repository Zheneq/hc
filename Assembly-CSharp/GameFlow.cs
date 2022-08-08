// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Theatrics;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Class <c>GameFlow</c> governs the overall flow of the game.
/// <para>
/// It creates certain subsystems (<see cref="ControlPointManager" />, <see cref="BarrierManager" />, <see cref="BotManager" />),
/// spawns players at the beginning of a game, initiates game loop and controls turn resoltion.
/// Also, it holds <see cref="PlayerDetails" />, receives Cast Ability requests (<see cref="SendCastAbility(ActorData, AbilityData.ActionType, List{AbilityTarget})">SendCastAbility</see>),
/// and syncronizes match time (<see cref="RpcSetMatchTime(float)">RpcSetMatchTime</see>).
/// </para>
/// <para>
/// Game start is triggered by <see cref="ServerGameManager.StartGame"/>.
/// </para>
/// </summary>
public class GameFlow : NetworkBehaviour
{
	public class SetHumanInfoMessage : MessageBase
	{
		public string m_userName;
		public string m_buildVersion;
		public string m_accountIdString;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(m_userName);
			writer.Write(m_buildVersion);
			writer.Write(m_accountIdString);
		}

		public override void Deserialize(NetworkReader reader)
		{
			m_userName = reader.ReadString();
			m_buildVersion = reader.ReadString();
			m_accountIdString = reader.ReadString();
		}
	}

	public class SelectCharacterMessage : MessageBase
	{
		public string m_characterName;
		public int m_skinIndex;
		public int m_patternIndex;
		public int m_colorIndex;

		public override void Serialize(NetworkWriter writer)
		{
			// reactor
			writer.Write(m_characterName);
			writer.WritePackedUInt32((uint)m_skinIndex);
			writer.WritePackedUInt32((uint)m_patternIndex);
			writer.WritePackedUInt32((uint)m_colorIndex);
		}

		public override void Deserialize(NetworkReader reader)
		{
			// reactor
			m_characterName = reader.ReadString();
			m_skinIndex = (int)reader.ReadPackedUInt32();
			m_patternIndex = (int)reader.ReadPackedUInt32();
			m_colorIndex = (int)reader.ReadPackedUInt32();
		}
	}

	public class SetTeamFinalizedMessage : MessageBase
	{
		public int m_team;

		public override void Serialize(NetworkWriter writer)
		{
			// reactor
			writer.WritePackedUInt32((uint)m_team);
		}

		public override void Deserialize(NetworkReader reader)
		{
			// reactor
			m_team = (int)reader.ReadPackedUInt32();
		}
	}

	// added in rogues
#if SERVER
	private class SpawnInProgressData
	{
		public ServerPlayerInfo playerInfo;
		public GameObject character;
	}
#endif

	// removed in rogues
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct PlayerComparer : IEqualityComparer<Player>
	{
		public bool Equals(Player x, Player y)
		{
			return x == y;
		}

		public int GetHashCode(Player obj)
		{
			return obj.GetHashCode();
		}
	}

	private Dictionary<Player, PlayerDetails> m_playerDetails = new Dictionary<Player, PlayerDetails>(default(PlayerComparer));
	private static GameFlow s_instance;

	// removed in rogues
	private static int kRpcRpcDisplayConsoleText = -789469928;
	// removed in rogues
	private static int kRpcRpcSetMatchTime = -559523706;

	internal Dictionary<Player, PlayerDetails> playerDetails => m_playerDetails;

#if SERVER
	// added in rogues
	private List<ActorData> m_spawningActors = new List<ActorData>(8);
	// added in rogues
	private int m_nextPlayerIndexToSpawn;
	// added in rogues
	private const float c_startWaitTimeoutTime = 120f;
#endif

	static GameFlow()
	{
		// reactor
		NetworkBehaviour.RegisterRpcDelegate(typeof(GameFlow), kRpcRpcDisplayConsoleText, InvokeRpcRpcDisplayConsoleText);
		NetworkBehaviour.RegisterRpcDelegate(typeof(GameFlow), kRpcRpcSetMatchTime, InvokeRpcRpcSetMatchTime);
		NetworkCRC.RegisterBehaviour("GameFlow", 0);
		// rogues
		//NetworkBehaviour.RegisterRpcDelegate(typeof(GameFlow), "RpcDisplayConsoleText", new NetworkBehaviour.CmdDelegate(GameFlow.InvokeRpcRpcDisplayConsoleText));
		//NetworkBehaviour.RegisterRpcDelegate(typeof(GameFlow), "RpcSetMatchTime", new NetworkBehaviour.CmdDelegate(GameFlow.InvokeRpcRpcSetMatchTime));
	}

	public override void OnStartClient()
	{
	}

	private void Client_OnDestroy()
	{
	}

	[Client]
	internal void SendCastAbility(ActorData caster, AbilityData.ActionType actionType, List<AbilityTarget> targets)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void GameFlow::SendCastAbility(ActorData,AbilityData/ActionType,System.Collections.Generic.List`1<AbilityTarget>)' called on server");
			return;
		}

		// reactor
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.StartMessage((int)MyMsgType.CastAbility);
		networkWriter.Write(caster.ActorIndex);
		networkWriter.Write((int)actionType);
		AbilityTarget.SerializeAbilityTargetList(targets, networkWriter);
		networkWriter.FinishMessage();
		ClientGameManager.Get().Client.SendWriter(networkWriter, 0);
		// rogues
		//NetworkClient.Send<CastAbility>(new CastAbility
		//{
		//	m_actorIndex = caster.ActorIndex,
		//	m_actionType = actionType,
		//	m_targets = targets
		//});
	}

	internal static GameFlow Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void Start()
	{
		// added in rogues
#if SERVER
		Server_Start();
#endif

		OnLoadedLevel();
		GameFlowData.s_onGameStateChanged += OnGameStateChanged;
		if (!NetworkServer.active && GameFlowData.Get() != null)
		{
			OnGameStateChanged(GameFlowData.Get().gameState);
		}
	}

	private void OnLoadedLevel()
	{
		// TODO HACK custom check
		if (HighlightUtils.Get())
		{
			HighlightUtils.Get().HideCursor = false;
		}
	}

	private void OnDestroy()
	{
		Client_OnDestroy();
		// some broken code
		//if (EventManager.Instance != null)
		//{
		//}
		// added in rogues
#if SERVER
		Server_OnDestroy();
#endif

		GameFlowData.s_onGameStateChanged -= OnGameStateChanged;
		s_instance = null;
	}

	private void OnGameStateChanged(GameState newState)
	{
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (clientFog != null)
		{
			clientFog.MarkForRecalculateVisibility();
		}
		switch (newState)
		{
			case GameState.BothTeams_Decision:
				AudioManager.PostEvent("sw_game_state", AudioManager.EventAction.SetSwitch, "game_state_decision");
				if (AudioManager.GetMixerSnapshotManager())  // check added in rogues
				{
					AudioManager.GetMixerSnapshotManager().SetMix_DecisionCam();
				}

#if SERVER
				HandleUpdateTeamTurnStart_FCFS();
				SetupTeamsForDecision();
#endif

				break;
			case GameState.BothTeams_Resolve:
				AudioManager.PostEvent("sw_game_state", AudioManager.EventAction.SetSwitch, "game_state_resolve");
				AudioManager.PostEvent("ui_resolution_cam_start");
				if (AudioManager.GetMixerSnapshotManager())  // check added in rogues
				{
					AudioManager.GetMixerSnapshotManager().SetMix_ResolveCam();
				}
				if (GameEventManager.Get() != null)
				{
					GameEventManager.Get().FireEvent(GameEventManager.EventType.ClientResolutionStarted, null);
				}

#if SERVER
				TheatricsManager.Get().InitTurn();
				Log.Info($"Initializing turn in theatrics");
#endif

				break;
				// rogues
				//case GameState.PVE_TeamTurnStart:
				//	break;
				//case GameState.PVE_TeamActions:
				//	AudioManager.PostEvent("sw_game_state", AudioManager.EventAction.SetSwitch, "game_state_resolve");
				//	AudioManager.PostEvent("ui_resolution_cam_start", null);
				//	if (AudioManager.GetMixerSnapshotManager())
				//	{
				//		AudioManager.GetMixerSnapshotManager().SetMix_ResolveCam();
				//	}
				//	if (HUD_UI.Get() != null)
				//	{
				//		HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_theTimer.m_abilityUsedTracker.ClearAllAbilties(UIQueueListPanel.UIPhase.None);
				//		HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.ClearUsedActionsDisplay();
				//	}
				//	break;
		}
	}

	internal void CheckTutorialAutoselectCharacter()
	{
		// some broken code
		if (m_playerDetails.Count == 1 && GameFlowData.Get().GetNumAvailableCharacterResourceLinks() != 1)
		{
		}
	}

	public Player GetPlayerFromConnectionId(int connectionId)
	{
		foreach (Player key in m_playerDetails.Keys)
		{
			if (key.m_connectionId == connectionId)
			{
				return key;
			}
		}
		return default(Player);
	}

	public string GetPlayerHandleFromConnectionId(int connectionId)
	{
		foreach (KeyValuePair<Player, PlayerDetails> keyValuePair in m_playerDetails)
		{
			if (keyValuePair.Key.m_connectionId == connectionId)
			{
				return keyValuePair.Value.m_handle;
			}
		}
		return "";
	}

	public string GetPlayerHandleFromAccountId(long accountId)
	{
		foreach (KeyValuePair<Player, PlayerDetails> keyValuePair in m_playerDetails)
		{
			if (keyValuePair.Key.m_accountId == accountId)
			{
				return keyValuePair.Value.m_handle;
			}
		}
		return "";
	}

	[ClientRpc]
	private void RpcDisplayConsoleText(DisplayConsoleTextMessage message)
	{
		if (message.RestrictVisibiltyToTeam == Team.Invalid
			|| (GameFlowData.Get().activeOwnedActorData != null
				&& GameFlowData.Get().activeOwnedActorData.GetTeam() == message.RestrictVisibiltyToTeam))
		{
			string text;
			if (!message.Unlocalized.IsNullOrEmpty())
			{
				text = message.Unlocalized;
			}
			else if (message.Token.IsNullOrEmpty())
			{
				text = StringUtil.TR(message.Term, message.Context);
			}
			else
			{
				text = string.Format(StringUtil.TR(message.Term, message.Context), message.Token);
			}
			TextConsole.Get().Write(new TextConsole.Message
			{
				Text = text,
				MessageType = message.MessageType,
				RestrictVisibiltyToTeam = message.RestrictVisibiltyToTeam,
				SenderHandle = message.SenderHandle,
				CharacterType = message.CharacterType
			});
		}
	}

	[ClientRpc]
	private void RpcSetMatchTime(float timeSinceMatchStart)
	{
		UITimerPanel.Get().SetMatchTime(timeSinceMatchStart);
	}

	// reactor
	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		if (!initialState)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		if (!initialState && syncVarDirtyBits == 0)
		{
			return false;
		}
		NetworkWriterAdapter networkWriterAdapter = new NetworkWriterAdapter(writer);
		int value = m_playerDetails.Count;
		networkWriterAdapter.Serialize(ref value);
		if (value < 0 || value > 20)
		{
			Log.Error("Invalid number of players: " + value);
			value = Mathf.Clamp(value, 0, 20);
		}
		foreach (var current in m_playerDetails)
		{
			Player player = current.Key;
			PlayerDetails details = current.Value;
			if (details != null)
			{
				player.OnSerializeHelper(networkWriterAdapter);
				details.OnSerializeHelper(networkWriterAdapter);
			}
		}

		return true;
	}

	// reactor
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint num = uint.MaxValue;
		if (!initialState)
		{
			num = reader.ReadPackedUInt32();
		}
		if (num != 0)
		{
			NetworkReaderAdapter networkReaderAdapter = new NetworkReaderAdapter(reader);
			int value = m_playerDetails.Count;
			networkReaderAdapter.Serialize(ref value);
			if (value < 0 || value > 20)
			{
				Log.Error("Invalid number of players: " + value);
				value = Mathf.Clamp(value, 0, 20);
			}
			m_playerDetails.Clear();
			for (int i = 0; i < value; i++)
			{
				Player key = default(Player);
				PlayerDetails playerDetails = new PlayerDetails(PlayerGameAccountType.None);
				key.OnSerializeHelper(networkReaderAdapter);
				playerDetails.OnSerializeHelper(networkReaderAdapter);
				key.m_accountId = playerDetails.m_accountId;
				m_playerDetails[key] = playerDetails;
				if ((bool)GameFlowData.Get() && GameFlowData.Get().LocalPlayerData == null)
				{
					GameFlowData.Get().SetLocalPlayerData();
				}
			}
		}
	}

	// added in rogues
#if SERVER
	private void Server_Start()
	{
		GameFlowData.s_onAddActor += this.OnAddActor;
		this.m_nextPlayerIndexToSpawn = 0;
	}
#endif

	// added in rogues
#if SERVER
	[Server]
	public override void OnStartServer()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameFlow::OnStartServer()' called on client");
			return;
		}
		// custom
		NetworkServer.RegisterHandler((short)MyMsgType.CastAbility, MsgCastAbility);
		// rogues
		//NetworkServer.RegisterHandler<CastAbility>(new Action<NetworkConnection, CastAbility>(GameFlow.MsgCastAbility));
		if (base.GetComponent<BotManager>() == null)
		{
			base.gameObject.AddComponent<BotManager>();
		}
	}
#endif

	// added in rogues
#if SERVER
	private void Server_OnDestroy()
	{
		// custom
		NetworkServer.UnregisterHandler((short)MyMsgType.CastAbility);
		// rogues
		//NetworkServer.UnregisterHandler<CastAbility>();
		GameFlowData.s_onAddActor -= this.OnAddActor;
	}
#endif

	// added in rogues
#if SERVER
	[ServerCallback]
	private void Update()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.Update_FCFS();
	}
#endif

	// added in rogues
#if SERVER
	private void HandleUpdateSpawningPlayers()
	{
		this.SpawnNextPlayer(out bool flag);
		if (!flag && this.m_spawningActors.Count == 0)
		{
			GameFlowData.Get().gameState = GameState.StartingGame;
		}
	}
#endif

	// added in rogues
#if SERVER
	private void HandleUpdateStartingGame()
	{
		GameFlowData gameFlowData = GameFlowData.Get();
		if (gameFlowData.GetTimeInState() > gameFlowData.m_startTime)
		{
			bool flag = gameFlowData.GetTimeInState() > c_startWaitTimeoutTime;
			if (ServerGameManager.Get().ClientsPreparedForGameStart() || flag)
			{
				if (flag)
				{
					Debug.LogWarning("Going into decision phase but did not receive all client actordata ready notifications");
					ServerGameManager.Get().LogAllClientStates();
				}
				GameplayMetricHelper.InitializePlayerGameSummary(this.m_playerDetails.Values.ToList<PlayerDetails>());
				MatchLogger.Get().NewMatch();
				if (GameManager.Get().GameStatus != GameStatus.Stopped)
				{
					this.SetupCardCooldownsOnGameStart();
				}
				gameFlowData.gameState = GameState.Deployment;
				gameFlowData.Networkm_deploymentTime = 1f;
				if (NPCCoordinator.Get() != null)
				{
					NPCCoordinator.Get().OnTurnStart();
				}
			}
		}
	}
#endif

	// custom
	private void HandleUpdateResolve()
	{
		ServerActionBuffer actionBuffer = ServerActionBuffer.Get();
		TheatricsManager theatrics = TheatricsManager.Get();
		if (actionBuffer.ActionPhase == ActionBufferPhase.Abilities)
		{
			ServerResolutionManager manager = ServerResolutionManager.Get();

			if (manager.ActionsDoneResolving() && !actionBuffer.IsWaitingForPlayPhaseEnded())
			{
				while (true)
				{
					ServerEffectManager.Get().OnAbilityPhaseEnd(actionBuffer.AbilityPhase);
					if (actionBuffer.AbilityPhase == AbilityUtils.GetLowestAbilityPriority())
					{
						actionBuffer.AbilityPhase = AbilityPriority.INVALID;
						actionBuffer.ActionPhase = ActionBufferPhase.AbilitiesWait;
						Log.Info($"Going to next action phase {actionBuffer.ActionPhase}");
						return;
					}

					actionBuffer.AbilityPhase = actionBuffer.AbilityPhase == AbilityPriority.INVALID
						? AbilityUtils.GetHighestAbilityPriority()
						: AbilityUtils.GetNextAbilityPriority(actionBuffer.AbilityPhase);
					ServerEffectManager.Get().OnAbilityPhaseStart(actionBuffer.AbilityPhase);
					Log.Info($"Going to next turn ability phase {actionBuffer.AbilityPhase}");

					// from QueuedPlayerActionsContainer::InitEffectsForExecution
					List<Effect> executingEffects = new List<Effect>();
					foreach (List<Effect> effectsOnActor in ServerEffectManager.Get().GetAllActorEffects().Values)
					{
						foreach (Effect effect in effectsOnActor)
						{
							if (effect.HitPhase == actionBuffer.AbilityPhase)
							{
								EffectResults resultsForPhase = effect.GetResultsForPhase(actionBuffer.AbilityPhase, true);
								if (effect.HitPhase == actionBuffer.AbilityPhase && (resultsForPhase == null || !resultsForPhase.GatheredResults))
								{
									effect.Resolve();
									executingEffects.Add(effect);
								}
							}
						}
					}
					foreach (Effect effect in ServerEffectManager.Get().GetWorldEffects())
					{
						if (effect.HitPhase == actionBuffer.AbilityPhase)
						{
							EffectResults resultsForPhase = effect.GetResultsForPhase(actionBuffer.AbilityPhase, true);
							if (effect.HitPhase == actionBuffer.AbilityPhase && (resultsForPhase == null || !resultsForPhase.GatheredResults))
							{
								effect.Resolve();
								executingEffects.Add(effect);
							}
						}
					}
					bool hasActionsThisPhase = false;
					List<ActorAnimation> anims = new List<ActorAnimation>();
					if (executingEffects.Count > 0)
					{
						Log.Info($"Have {executingEffects.Count} effects in this phase, playing them...");
						PlayerAction_Effect action = new PlayerAction_Effect(executingEffects, actionBuffer.AbilityPhase);
						anims.AddRange(action.PrepareResults());
						hasActionsThisPhase = true;
					}

					List<AbilityRequest> requestsThisPhase = actionBuffer.GetAllStoredAbilityRequests().FindAll(r => r?.m_ability?.RunPriority == actionBuffer.AbilityPhase);
					if (requestsThisPhase.Count > 0)
					{
						Log.Info($"Have {requestsThisPhase.Count} requests in this phase, playing them...");
						anims.AddRange(new PlayerAction_Ability(requestsThisPhase, actionBuffer.AbilityPhase).PrepareResults());
						hasActionsThisPhase = true;
					}

					theatrics.SetupTurnAbilityPhase(
						actionBuffer.AbilityPhase,
						actionBuffer.GetAllStoredAbilityRequests(),
						new HashSet<int>() { },  // TODO LOW (hacked inside)
						false);

					if (hasActionsThisPhase)
					{
						//foreach (ActorAnimation actorAnimation in anims)
						//{
						//	actorAnimation.SetTurn_FCFS(turn);
						//}
						//theatrics.m_turn.m_abilityPhases[(int)currentPhase].m_actorAnimations = new List<ActorAnimation>(animEntries);
						//TheatricsManager.Get().SetTurn_FCFS(turn);
						//TheatricsManager.Get().InitPhaseClient_FCFS(currentPhase);

						ServerResolutionManager.Get().OnAbilityPhaseStart(actionBuffer.AbilityPhase);
						ServerActionBuffer.Get().SynchronizePositionsOfActorsParticipatingInPhase(actionBuffer.AbilityPhase); /// check? see PlayerAction_*.ExecuteAction for more resolution stuff gathered from all over ARe
						break;
					}
					else
					{
						Log.Info($"No requests in this phase, going to the next one");
					}
				}

				theatrics.SetDirtyBit(uint.MaxValue);
				theatrics.PlayPhase(actionBuffer.AbilityPhase);
			}
		}
		else if (actionBuffer.ActionPhase == ActionBufferPhase.AbilitiesWait)
		{
			foreach (ActorData actor in GameFlowData.Get().GetActors())
			{
				var turnSm = actor.gameObject.GetComponent<ActorTurnSM>();
				turnSm.OnMessage(TurnMessage.CLIENTS_RESOLVED_ABILITIES);
			}
			new PlayerAction_Movement(ServerActionBuffer.Get().GetAllStoredMovementRequests().FindAll(req => !req.IsChasing())).ExecuteAction();
			actionBuffer.ActionPhase = ActionBufferPhase.Movement;
		}
		else if (actionBuffer.ActionPhase == ActionBufferPhase.Movement)
		{
			ServerMovementManager manager = ServerMovementManager.Get();
			if (!manager.WaitingOnClients)
			{
				new PlayerAction_Movement(ServerActionBuffer.Get().GetAllStoredMovementRequests().FindAll(req => req.IsChasing())).ExecuteAction();
				actionBuffer.ActionPhase = ActionBufferPhase.MovementChase;
			}
		}
		else if (actionBuffer.ActionPhase == ActionBufferPhase.MovementChase)
		{
			//ServerEvadeManager manager = ServerEvadeManager.Get();
			ServerMovementManager manager = ServerMovementManager.Get();
			if (!manager.WaitingOnClients)
			{
				foreach (ActorData actor in GameFlowData.Get().GetActors())
				{
					var turnSm = actor.gameObject.GetComponent<ActorTurnSM>();
					turnSm.OnMessage(TurnMessage.MOVEMENT_RESOLVED);
				}
				actionBuffer.ActionPhase = ActionBufferPhase.MovementWait;
			}
		}
		else
		{
			theatrics.MarkPhasesOnActionsDone();
			actionBuffer.ActionPhase = ActionBufferPhase.Done;
			ServerCombatManager.Get().ResolveHitPoints();
			ServerCombatManager.Get().ResolveTechPoints();

			// TODO wait a couple seconds here? (if we wait in ending turn, it can cause ui artifacts)
			GameFlowData.Get().gameState = GameState.EndingTurn;
			
			foreach (ActorData actorData in GameFlowData.Get().GetActors())
			{
				FreelancerStats freelancerStats = actorData.GetFreelancerStats();
				if (freelancerStats != null)
				{
					for (int i = 0; i < 4; i++)
					{
						Log.Info($"Freelancer stats {actorData.m_displayName}:" +
						         $" {freelancerStats.GetLocalizedDescriptionOfStat(i)} = {freelancerStats.GetValueOfStat(i)}"); 
					}
				}
			}
		}
	}

#if SERVER
	// added in rogues
	internal void FlagPlayerAsDisconnected(NetworkConnection conn)
	{
		foreach (Player player in this.m_playerDetails.Keys)
		{
			if (player.m_connectionId == conn.connectionId)
			{
				this.m_playerDetails[player].m_disconnected = true;
			}
		}
	}

	// added in rogues
	internal void ReplaceWithBots(NetworkConnection conn)
	{
		foreach (Player player in this.m_playerDetails.Keys)
		{
			if (player.m_connectionId == conn.connectionId)
			{
				PlayerDetails playerDetails = this.m_playerDetails[player];
				playerDetails.m_disconnected = true;
				this.ReplaceWithBots(player, playerDetails, true);
			}
		}
	}

	// added in rogues
	internal void ReplaceWithBots(Player player, PlayerDetails playerDetails, bool clearHumanFlag)
	{
		if (clearHumanFlag)
		{
			playerDetails.ReplaceWithBots();
		}
		base.SetDirtyBit(1U);
		for (int i = 0; i < playerDetails.m_gameObjects.Count; i++)
		{
			GameObject gameObject = playerDetails.m_gameObjects[i];
			ActorData component = gameObject.GetComponent<ActorData>();
			gameObject.AddComponent<BotController>();
			component.HasBotController = true;
			NPCBrain_Adaptive component2 = gameObject.GetComponent<NPCBrain_Adaptive>();
			if (component2 != null)
			{
				component2.isReplacingHuman = true;
			}
		}
		for (int j = 0; j < NetworkServer.connections.Count; j++)
		{
			NetworkConnection networkConnection = NetworkServer.connections[j];
			if (networkConnection != null && networkConnection.connectionId == player.m_connectionId)
			{
				ServerGameManager.Get().OnClientReplacedWithBots(networkConnection);
			}
		}
		TheatricsManager.Get().OnReplacedWithBots(player);
	}

	// added in rogues
	public void ReplaceWithHumans(NetworkConnection conn, long accountId)
	{
		Player player = new Player(conn, accountId);
		List<Player> playersToReplace = new List<Player>();
		List<PlayerDetails> playerDetailsToReplace = new List<PlayerDetails>();
		foreach (KeyValuePair<Player, PlayerDetails> keyValuePair in playerDetails)
		{
			Player key = keyValuePair.Key;
			PlayerDetails value = keyValuePair.Value;
			if (value.m_accountId == accountId)
			{
				playersToReplace.Add(key);
				playerDetailsToReplace.Add(value);
				value.ReplaceWithHumans();
				SetDirtyBit(1U);
				for (int i = 0; i < value.m_gameObjects.Count; i++)
				{
					GameObject gameObject = value.m_gameObjects[i];
					gameObject.GetComponent<PlayerData>().m_player = player;
					ActorData actorData = gameObject.GetComponent<ActorData>();
					NetworkServer.ReplacePlayerForConnection(conn, actorData.gameObject, 0); // TODO check. There was no playerControllerId in rogues. 0 is supposed to be actual player.
					if (actorData.HasBotController)
					{
						UnityEngine.Object.Destroy(gameObject.GetComponent<BotController>());
						actorData.HasBotController = false;
						int length = value.m_handle.LastIndexOf('#');
						actorData.m_displayName = value.m_handle.Substring(0, length);
						// rogues
						//component.SetupAbilityGear();
					}
				}
				break;
			}
		}
		ServerGameManager.Get().OnClientReplacedWithHumans(conn);
		foreach (Player key2 in playersToReplace)
		{
			this.playerDetails.Remove(key2);
		}
		foreach (PlayerDetails value2 in playerDetailsToReplace)
		{
			this.playerDetails[player] = value2;
		}
	}

	// added in rogues
	[Server]
	public int GetConnectedPlayerCount()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Int32 GameFlow::GetConnectedPlayerCount()' called on client");
			return 0;
		}
		int num = 0;
		foreach (Player key in this.m_playerDetails.Keys)
		{
			if (this.m_playerDetails[key].IsHumanControlled && !this.m_playerDetails[key].m_disconnected)
			{
				num++;
			}
		}
		return num;
	}

	// added in rogues
	[Server]
	private void OnAddActor(ActorData actor)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameFlow::OnAddActor(ActorData)' called on client");
			return;
		}
		this.m_spawningActors.Remove(actor);
		if (CardManager.Get() != null)
		{
			CardManager.Get().SetDeckAndGiveCards(actor, actor.m_selectedCards);
		}
	}

	// added in rogues
	[Server]
	public void AddPlayer(ServerPlayerState serverPlayerState)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameFlow::AddPlayer(ServerPlayerState)' called on client");
			return;
		}
		PlayerDetails playerDetails = new PlayerDetails(serverPlayerState.PlayerInfo.LobbyPlayerInfo.GameAccountType);
		playerDetails.m_handle = serverPlayerState.PlayerInfo.LobbyPlayerInfo.Handle;
		playerDetails.m_team = serverPlayerState.PlayerInfo.TeamId;
		playerDetails.m_lobbyPlayerInfoId = serverPlayerState.PlayerInfo.PlayerId;
		playerDetails.m_botsMasqueradeAsHumans = GameManager.Get().IsBotMasqueradingAsHuman(serverPlayerState.PlayerInfo.PlayerId);
		if (serverPlayerState.SessionInfo != null)
		{
			playerDetails.m_accountId = serverPlayerState.SessionInfo.AccountId;
			playerDetails.m_buildVersion = serverPlayerState.SessionInfo.BuildVersion;
		}
		if (serverPlayerState.PlayerInfo.ReplacedWithBots)
		{
			playerDetails.ReplaceWithBots();
		}
		Player player = new Player(serverPlayerState.ConnectionPersistent, playerDetails.m_accountId);
		if (serverPlayerState.PlayerInfo.IsAIControlled && BotManager.Get())
		{
			BotManager.Get().AddExistingBot(player);
		}
		this.m_playerDetails[player] = playerDetails;
		if (serverPlayerState.PlayerInfo.CharacterType != CharacterType.None && GameWideData.Get().GetCharacterResourceLink(serverPlayerState.PlayerInfo.CharacterType) == null)
		{
			throw new Exception(string.Format("Could not load character {0}", serverPlayerState.PlayerInfo.CharacterType.ToString()));
		}
		playerDetails.m_serverPlayerInfo = serverPlayerState.PlayerInfo;
		base.SetDirtyBit(1U);
	}

	// added in rogues
	[Server]
	private void SpawnNextPlayer(out bool moreToSpawn)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameFlow::SpawnNextPlayer(System.Boolean&)' called on client");
			moreToSpawn = false;
			return;
		}
		int num = 0;
		foreach (KeyValuePair<Player, PlayerDetails> keyValuePair in this.m_playerDetails)
		{
			if (num == this.m_nextPlayerIndexToSpawn)
			{
				this.SpawnAgent(keyValuePair.Key, keyValuePair.Value, num);
				this.m_nextPlayerIndexToSpawn++;
				moreToSpawn = true;
				return;
			}
			num++;
		}
		moreToSpawn = false;
	}

	// added in rogues
	[Server]
	private void SpawnCharacters(Player playerID, PlayerDetails playerDetails, int playerIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameFlow::SpawnCharacters(Player,PlayerDetails,System.Int32)' called on client");
			return;
		}
		List<GameFlow.SpawnInProgressData> list = new List<GameFlow.SpawnInProgressData>();
		int num = 0;
		foreach (ServerPlayerInfo serverPlayerInfo in playerDetails.AllServerPlayerInfos)
		{
			GameObject character;
			if (!playerDetails.IsSpectator)
			{
				if (serverPlayerInfo.SelectedCharacter.ActorDataPrefab == null)
				{
					Log.Error("Tried to spawn a character with no ActorDataPrefab: [{0}-{1}]", new object[]
					{
						serverPlayerInfo.SelectedCharacter.resourceLink.m_displayName,
						serverPlayerInfo.SelectedCharacter.selectedSkin.ToString()
					});
					continue;
				}
				Log.Info($"GameFlow::SpawnCharacters {serverPlayerInfo.CharacterType}");
				character = UnityEngine.Object.Instantiate<GameObject>(serverPlayerInfo.SelectedCharacter.ActorDataPrefab, Vector3.zero, Quaternion.identity);
			}
			else
			{
				Log.Info($"GameFlow::SpawnCharacters SPECTATOR");
				character = UnityEngine.Object.Instantiate<GameObject>(GameWideData.Get().SpectatorPrefab, Vector3.zero, Quaternion.identity);
			}
			if (character)
			{
				PlayerData playerData = character.GetComponent<PlayerData>();
				if (playerData == null)
				{
					throw new Exception(string.Format("Character {0} needs a PlayerData component", serverPlayerInfo.SelectedCharacter.resourceLink.m_displayName));
				}
				playerData.m_player = playerID;
				playerData.PlayerIndex = playerIndex;
				playerDetails.m_gameObjects.Add(character);
			}
			list.Add(new GameFlow.SpawnInProgressData
			{
				playerInfo = serverPlayerInfo,
				character = character
			});
		}
		bool flag = false;
		foreach (GameFlow.SpawnInProgressData spawnInProgressData in list)
		{
			ServerPlayerInfo playerInfo = spawnInProgressData.playerInfo;
			GameObject character = spawnInProgressData.character;
			if (character)
			{
				PlayerData playerData = character.GetComponent<PlayerData>();
				ActorData actorData = character.GetComponent<ActorData>();
				CharacterVisualInfo characterVisualInfo = new CharacterVisualInfo();
				if (actorData != null)
				{
					actorData.Initialize(playerInfo.SelectedCharacter.heroPrefabLink, false); // TODO LOW no addMasterSkinVfx in rogues
					characterVisualInfo = playerInfo.SelectedCharacter.selectedSkin;
					CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(playerInfo.CharacterType);
					if (characterResourceLink != null)
					{
						int skinIndex = characterVisualInfo.skinIndex;
						int patternIndex = characterVisualInfo.patternIndex;
						int colorIndex = characterVisualInfo.colorIndex;
						if (skinIndex >= characterResourceLink.m_skins.Count || patternIndex >= characterResourceLink.m_skins[skinIndex].m_patterns.Count || colorIndex >= characterResourceLink.m_skins[skinIndex].m_patterns[patternIndex].m_colors.Count || characterResourceLink.m_skins[skinIndex].m_patterns[patternIndex].m_colors[colorIndex].m_heroPrefab == null || characterResourceLink.m_skins[skinIndex].m_patterns[patternIndex].m_colors[colorIndex].m_heroPrefab.IsEmpty)
						{
							Log.Warning(string.Format("Player {2} with character {0} using non-existing visual info, setting to default. Desired selection = {1}", playerInfo.SelectedCharacter.resourceLink.m_displayName, characterVisualInfo.ToString(), string.IsNullOrEmpty(playerInfo.Handle) ? "UNKNOWN" : playerInfo.Handle));
							characterVisualInfo.ResetToDefault();
						}
					}
					actorData.m_visualInfo = characterVisualInfo;
					actorData.m_abilityVfxSwapInfo = playerInfo.CharacterAbilityVfxSwaps;
					SatelliteController component4 = character.GetComponent<SatelliteController>();
					if (component4 != null)
					{
						component4.OverridePrefabs(playerInfo.SelectedCharacter.resourceLink, characterVisualInfo);
					}
					if (!playerDetails.IsHumanControlled || playerDetails.GameOptionFlags.HasGameOption(PlayerGameOptionFlag.ReplaceHumanWithBot))
					{
						character.AddComponent<BotController>();
						actorData.HasBotController = true;
						if (playerDetails.IsHumanControlled)
						{
							foreach (NetworkConnection networkConnection in NetworkServer.connections)
							{
								if (networkConnection != null && networkConnection.connectionId == playerID.m_connectionId)
								{
									ServerGameManager.Get().OnClientReplacedWithBots(networkConnection);
								}
							}
							TheatricsManager.Get().OnReplacedWithBots(playerID);
						}
					}
					// custom
					actorData.PlayerIndex = playerIndex;
					// rogues
					//actorData.NetworkPlayerIndex = playerIndex;
					if (playerDetails.IsHumanControlled || playerInfo.LobbyPlayerInfo.ReplacedWithBots || playerInfo.IsLoadTestBot || playerDetails.m_botsMasqueradeAsHumans)
					{
						int num2 = playerDetails.m_handle.LastIndexOf('#');
						actorData.m_displayName = ((num2 != -1) ? playerDetails.m_handle.Substring(0, num2) : playerDetails.m_handle);
						// custom
						playerData.m_playerHandle = playerDetails.m_handle;
						// rogues
						//playerData.Networkm_playerHandle = playerDetails.m_handle;
					}
					else
					{
						actorData.m_displayName = playerDetails.m_handle;
					}
					actorData.SetTeam(playerInfo.TeamId);
					//actorData.InitEquipmentStats(); // rogues

					// custom
					// TODO ARTEMIS actors seem to be not network spawned yet at this point (OnStartServer not called)
					//actorData.GetAbilityData().SpawnAndSetupCards(new CharacterCardInfo  // TODO get cards from lobby
					//{
					//	PrepCard = CardType.NoOverride,
					//	DashCard = CardType.NoOverride,
					//	CombatCard = CardType.NoOverride,
					//});
					//actorData.SetupAbilityMods(new CharacterModInfo  // TODO get mods from lobby
					//{
					//	ModForAbility0 = 0,
					//	ModForAbility1 = 0,
					//	ModForAbility2 = 0,
					//	ModForAbility3 = 0,
					//	ModForAbility4 = 0,
					//});

					// TODO probably will break fourlancer
					actorData.PlayerIndex = playerInfo.PlayerId;
					actorData.ActorIndex = playerInfo.PlayerId;

					actorData.UpdateDisplayName(playerInfo.Handle);
					//end custom

					actorData.InitActorNetworkVisibilityObjects();
					BoardSquare initialSpawnSquare = SpawnPointManager.Get().GetInitialSpawnSquare(actorData, this.m_spawningActors);
					actorData.AssignToInitialBoardSquare(initialSpawnSquare);
					this.m_spawningActors.Add(actorData);
					if (playerDetails.IsHumanControlled)
					{
						actorData.m_availableTauntIDs.Clear();
						CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink(playerInfo.CharacterType);
						int num3 = 0;
						while (num3 < characterResourceLink2.m_taunts.Count && num3 < playerInfo.CharacterTaunts.Count)
						{
							if (playerInfo.CharacterTaunts[num3].Unlocked)
							{
								actorData.m_availableTauntIDs.Add(characterResourceLink2.m_taunts[num3].m_uniqueID);
							}
							num3++;
						}
					}
					//actorData.Networkm_selectedGear = playerInfo.CharacterGear;  // rogues
					actorData.m_selectedCards = playerInfo.CharacterCards;
					if (CardManagerData.Get() != null)
					{
						if (!CardManagerData.Get().IsCardTypePossibleInGame(actorData.m_selectedCards.PrepCard, AbilityRunPhase.Prep))
						{
							actorData.m_selectedCards.PrepCard = CardManagerData.Get().GetDefaultPrepCardType();
						}
						if (!CardManagerData.Get().IsCardTypePossibleInGame(actorData.m_selectedCards.DashCard, AbilityRunPhase.Dash))
						{
							actorData.m_selectedCards.DashCard = CardManagerData.Get().GetDefaultDashCardType();
						}
						if (!CardManagerData.Get().IsCardTypePossibleInGame(actorData.m_selectedCards.CombatCard, AbilityRunPhase.Combat))
						{
							actorData.m_selectedCards.CombatCard = CardManagerData.Get().GetDefaultCombatCardType();
						}
					}
				}
				if (playerDetails.IsHumanControlled)
				{
					int playerConnectionId = ServerGameManager.Get().GetPlayerConnectionId(playerDetails.m_accountId);
					NetworkConnection networkConnection2 = null;

					// rogues
					//if (NetworkServer.connections.TryGetValue(playerConnectionId, out networkConnection2))
					// custom
					foreach (NetworkConnection nc in NetworkServer.connections)
					{
						if (nc != null && nc.connectionId == playerConnectionId)
						{
							networkConnection2 = nc;
							break;
						}
					}

					if (networkConnection2 != null)
					// end custom
					{
						if (playerDetails.m_serverPlayerInfo != null && !playerDetails.m_serverPlayerInfo.ProxyPlayerInfos.Contains(playerInfo) && !flag)
						{
							// TODO don't know what to do about player controller
							//Component playerController = networkConnection2.playerController;
							//NetworkServer.ReplacePlayerForConnection(networkConnection2, character, 0); // TODO check. There was no playerControllerId in rogues. 0 is supposed to be actual player.
							NetworkServer.AddPlayerForConnection(networkConnection2, character, 0); // custom
																									//NetworkServer.UnSpawn(playerController.gameObject);
							flag = true;
						}
						else
						{
							NetworkServer.SpawnWithClientAuthority(character, networkConnection2);
						}
					}
					else
					{
						Log.Error("Player {0} failed to spawn, connectionId {1} was not found on NetworkServer", new object[]
						{
							playerInfo.Handle,
							playerConnectionId
						});
					}
				}
				else
				{
					NetworkServer.Spawn(character);
				}
				if (playerInfo.SelectedCharacter != null)
				{
					Log.Info(string.Format("Spawned character for {2}: [{0}-{1}]", playerInfo.SelectedCharacter.resourceLink.m_displayName, characterVisualInfo.ToString(), (playerDetails == null || playerDetails.m_handle == null) ? "NULL" : playerDetails.m_handle));
				}
				num++;
			}
			else
			{
				Log.Error("Failed to spawn a character: [{0}-{1}]", new object[]
				{
					playerInfo.SelectedCharacter.resourceLink.m_displayName,
					playerInfo.SelectedCharacter.selectedSkin.ToString()
				});
			}
		}
		if (!flag)
		{
			Log.Error("No primary Player for connection playerId: " + playerID.ToString());
		}
	}

	// added in rogues
	[Server]
	private void SpawnAgent(Player networkAgent, PlayerDetails playerDetails, int playerIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameFlow::SpawnAgent(Player,PlayerDetails,System.Int32)' called on client");
			return;
		}
		this.SpawnCharacters(networkAgent, playerDetails, playerIndex);
	}

	// added in rogues, but not used in rogues
	[Server]
	// TODO call when going into desicion
	private void SetupTeamsForDecision()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameFlow::SetupTeamsForDecision()' called on client");
			return;
		}
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (actorData != null)
			{
				actorData.GetAbilityData().QueueAutomaticAbilities();
			}
		}
	}

	// added in rogues
	[Server]
	private void NotifyOnTurnStart()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameFlow::NotifyOnTurnStart()' called on client");
			return;
		}
		ServerEffectManager.Get().OnTurnStart();
		ServerActionBuffer.Get().OnTurnStart();
		BarrierManager.Get().OnTurnStart();
		ServerEffectManager.GetSharedEffectBarrierManager().OnTurnStart();
		// TODO CTF CTC
		//if (CaptureTheFlag.Get() != null)
		//{
		//	CaptureTheFlag.Get().OnTurnStart();
		//}
		//if (CollectTheCoins.Get() != null)
		//{
		//	CollectTheCoins.Get().OnTurnStart();
		//}
		if (CoinCarnageManager.Get() != null)
		{
			CoinCarnageManager.Get().OnTurnStart();
		}
		if (NPCCoordinator.Get() != null)
		{
			NPCCoordinator.Get().OnTurnStart();
		}
		if (QueryAreaManager.Get() != null)
		{
			QueryAreaManager.Get().OnTurnStart();
		}
		// rogues
		//if (PveDialogueManager.Get() != null)
		//{
		//	PveDialogueManager.Get().OnTurnStart();
		//}
	}

	// added in rogues
	private bool PlayersDoneResolving(bool log = false)
	{
		bool result = true;
		foreach (GameObject gameObject in GameFlowData.Get().GetPlayers())
		{
			ActorTurnSM component = gameObject.GetComponent<ActorTurnSM>();
			if (component && component.CurrentState != TurnStateEnum.WAITING)
			{
				result = false;
				if (log)
				{
					Log.Error("{0} ActorTurnSM state = {1}", new object[]
					{
						gameObject.name,
						component.CurrentState.ToString()
					});
					break;
				}
				break;
			}
		}
		return result;
	}

	// added in rogues
	internal PlayerDetails FindHumanPlayerInfo(int serverToClientConnectionId, out Player outPlayer)
	{
		PlayerDetails result = null;
		outPlayer = default(Player);
		for (int i = 0; i < this.m_playerDetails.Count; i++)
		{
			Player player = this.m_playerDetails.Keys.ElementAt(i);
			if (this.m_playerDetails[player].IsHumanControlled && player.m_connectionId == serverToClientConnectionId)
			{
				outPlayer = player;
				result = this.m_playerDetails[player];
				break;
			}
		}
		return result;
	}

	// added in rogues
	internal PlayerDetails FindHumanPlayerInfoByAccount(long accountId, out Player outPlayer)
	{
		PlayerDetails result = null;
		outPlayer = default(Player);
		for (int i = 0; i < this.m_playerDetails.Count; i++)
		{
			Player player = this.m_playerDetails.Keys.ElementAt(i);
			if (this.m_playerDetails[player].IsHumanControlled && player.m_accountId == accountId)
			{
				outPlayer = player;
				result = this.m_playerDetails[player];
				break;
			}
		}
		return result;
	}

	// added in rogues
	internal uint FindNumHumanPlayers()
	{
		uint num = 0U;
		foreach (PlayerDetails playerDetails in this.m_playerDetails.Values)
		{
			if (playerDetails.IsHumanControlled && !playerDetails.IsSpectator)
			{
				num += 1U;
			}
		}
		return num;
	}

	// added in rogues
	private List<PlayerDetails> FindAllAccountPlayerInfo()
	{
		List<PlayerDetails> list = new List<PlayerDetails>();
		foreach (Player key in this.m_playerDetails.Keys)
		{
			if (this.m_playerDetails[key].m_gameAccountType != PlayerGameAccountType.None)
			{
				list.Add(this.m_playerDetails[key]);
			}
		}
		return list;
	}

	// added in rogues
	[Server]
	//private static void MsgCastAbility(NetworkConnection conn, CastAbility msg)
	private static void MsgCastAbility(NetworkMessage message)
	{
		CastAbility msg = message.ReadMessage<CastAbility>();
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameFlow::MsgCastAbility(Mirror.NetworkConnection,CastAbility)' called on client");
			return;
		}
		ActorData actorData = GameFlowData.Get().FindActorByActorIndex(msg.m_actorIndex);
		if (actorData != null)
		{
			Log.Info($"MsgCastAbility {actorData.DisplayName} - {msg.m_actionType} {msg.m_targets.Select(e => e.GetDebugString()).ToList().ToJson()}");
			actorData.GetComponent<ServerActorController>().ProcessCastAbilityRequest(msg.m_targets, msg.m_actionType, false);
		}
		else
		{
			Log.Error($"MsgCastAbility NULL actor {msg.m_actorIndex} - {msg.m_actionType} {msg.m_targets.Select(e => e.GetDebugString()).ToList().ToJson()}");
		}
	}

	// added in rogues
	[Server]
	internal void DisplayConsoleText(DisplayConsoleTextMessage message)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameFlow::DisplayConsoleText(DisplayConsoleTextMessage)' called on client");
			return;
		}
		this.CallRpcDisplayConsoleText(message);
		if (!message.Unlocalized.IsNullOrEmpty())
		{
			MatchLogger.Get().Log(message.Unlocalized);
			return;
		}
		MatchLogger.Get().Log(message.Term + " : " + message.Token);
	}

	// added in rogues
	[Server]
	public void SendMatchTime(float timeSinceMatchStart)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameFlow::SendMatchTime(System.Single)' called on client");
			return;
		}
		this.CallRpcSetMatchTime(timeSinceMatchStart);
	}

	// added in rogues
	internal void DisplayConsoleText(string term, string context, string token, ConsoleMessageType messageType)
	{
		this.DisplayConsoleText(new DisplayConsoleTextMessage
		{
			Term = term,
			Context = context,
			Token = token,
			Unlocalized = "",
			MessageType = messageType,
			RestrictVisibiltyToTeam = Team.Invalid
		});
	}

	// added in rogues
	internal void DisplayConsoleText(string term, string context, string token, string unlocalized, ConsoleMessageType messageType, Team restrictVisibilityToTeam, string senderHandle = null)
	{
		this.DisplayConsoleText(new DisplayConsoleTextMessage
		{
			Term = term,
			Context = context,
			Token = token,
			Unlocalized = unlocalized,
			MessageType = messageType,
			RestrictVisibiltyToTeam = restrictVisibilityToTeam,
			SenderHandle = senderHandle
		});
	}

	// added in rogues
	[Server]
	private void SetupCardCooldownsOnGameStart()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameFlow::SetupCardCooldownsOnGameStart()' called on client");
			return;
		}
		if (CardManagerData.Get() != null && CardManagerData.Get().m_cooldownOnAddToHand > 0)
		{
			int cooldownOnAddToHand = CardManagerData.Get().m_cooldownOnAddToHand;
			foreach (ActorData actorData in GameFlowData.Get().GetActors())
			{
				AbilityData abilityData = actorData.GetAbilityData();
				if (abilityData != null)
				{
					for (int i = 7; i <= 9; i++)
					{
						AbilityData.ActionType action = (AbilityData.ActionType)i;
						if (!abilityData.IsActionInCooldown(action))
						{
							actorData.GetAbilityData().PlaceInCooldownTillTurn(action, GameFlowData.Get().CurrentTurn + cooldownOnAddToHand);
						}
					}
				}
			}
		}
	}

	// added in rogues
	private void Update_FCFS()
	{
		if (NetworkServer.active)
		{
			if (base.GetComponent<ControlPointManager>() == null)
			{
				base.gameObject.AddComponent<ControlPointManager>();
			}
			if (base.GetComponent<BarrierManager>() == null)
			{
				base.gameObject.AddComponent<BarrierManager>();
			}
			if (base.GetComponent<BotManager>() == null)
			{
				base.gameObject.AddComponent<BotManager>();
			}
			GameFlowData gameFlowData = GameFlowData.Get();
			switch (gameFlowData.gameState)
			{
				case GameState.SpawningPlayers:
					this.HandleUpdateSpawningPlayers();
					return;
				case GameState.StartingGame:
					this.HandleUpdateStartingGame();
					return;
				case GameState.Deployment:
					if (gameFlowData.GetTimeSinceDeployment() > gameFlowData.m_deploymentTime / Time.timeScale)
					{
						MatchLogger.Get().ResetMatchStartTime();
						GameFlow.Get().SendMatchTime(0f);
						gameFlowData.gameState = GameState.BothTeams_Decision;  // TODO LOW check. GameState.PVE_TeamTurnStart in rogues

						return;
					}
					break;
				// TODO some code might be missing in EndingGame, BothTeams_Decision, BothTeams_Resolve
				case GameState.EndingGame:
				case GameState.BothTeams_Decision:
					break;
				case GameState.BothTeams_Resolve:
					// custom
					HandleUpdateResolve();
					break;
				// custom
				case GameState.EndingTurn:
					if (gameFlowData.GetTimeInState() > 2.0f)
					{
						HandleUpdateTurnEnd();
					}
					break;
				//	rogues
				//case GameState.PVE_TeamTurnStart:
				//	this.HandleUpdateTeamTurnStart_FCFS();
				//	return;
				//case GameState.PVE_TeamActions:
				//	this.HandleUpdateTeamActions_FCFS();
				//	return;
				//case GameState.PVE_TeamTurnEnd:
				//	this.HandleUpdateTeamTurnEnd_FCFS();
				//	break;
				//case GameState.PVE_TurnStartEffectActions:
				//	this.HandleUpdateTurnStartEffectActions_FCFS();
				//	return;
				default:
					return;
			}
		}
	}

	// added in rogues
	private void HandleUpdateTeamTurnStart_FCFS()
	{
		GameFlowData gameFlowData = GameFlowData.Get();
		//gameFlowData.ActingTeam = this.GetNextActingTeam_FCFS();
		//if (gameFlowData.ActingTeam == Team.TeamA)
		//{
		//GameFlowData.Get().ServerIncrementTurn();
		//}
		if (NetworkServer.active && ObjectivePoints.Get() != null)
		{
			ObjectivePoints.Get().ProcessRespawns();
		}
		//GameFlowData.Get().UpdateIsInCombatFlag();
		this.SetupCurrentTeamTurn_FCFS();
		this.NotifyOnTurnStart();
		//ServerActionBuffer.Get().GetPlayerActionFSM().RunEffectActions();
		//gameFlowData.gameState = GameState.PVE_TurnStartEffectActions;
	}

	// added in rogues
	//private Team GetNextActingTeam_FCFS()
	//{
	//	Team actingTeam = GameFlowData.Get().ActingTeam;
	//	if (actingTeam == Team.Invalid)
	//	{
	//		return Team.TeamA;
	//	}
	//	if (actingTeam == Team.TeamA)
	//	{
	//		return Team.TeamB;
	//	}
	//	return Team.TeamA;
	//}

	// added in rogues
	private void SetupCurrentTeamTurn_FCFS()
	{
		//Team actingTeam = GameFlowData.Get().ActingTeam;
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (NetworkServer.active && actorData != null)
			{
				//if (actorData.GetTeam() == actingTeam)
				//{
				//actorData.GetActorTurnSM().ResetUsedAbilityAndMoveData();
				actorData.OnTurnStart();
				actorData.GetActorTurnSM().OnMessage(TurnMessage.TURN_START, true);
				actorData.UpdateServerLastVisibleTurn();
				//}
				//else
				//{
				//    actorData.GetActorTurnSM().OnMessage(TurnMessage.ENEMY_TURN_START, true);
				//}
			}
		}
		if (CardManager.Get() != null)
		{
			CardManager.Get().RemoveUsedCards();
		}
	}

	// added in rogues
	//private void HandleUpdateTeamActions_FCFS()
	//{
	//	GameFlowData gameFlowData = GameFlowData.Get();
	//	bool flag = false;
	//	bool flag2 = false;
	//	this.TeamDecisionPhaseFinished_FCFS(gameFlowData.ActingTeam, out flag, out flag2);
	//	if (flag && ServerActionBuffer.Get().GetPlayerActionFSM().GetCurrentState() == PlayerActionStateMachine.StateFlag.WaitingForInput)
	//	{
	//		gameFlowData.gameState = GameState.PVE_TeamTurnEnd;
	//	}
	//}

	// added in rogues
	//private void TeamDecisionPhaseFinished_FCFS(Team team, out bool finished, out bool finishedHumans)
	//{
	//	finished = true;
	//	finishedHumans = true;
	//	foreach (ActorData actorData in GameFlowData.Get().GetActors())
	//	{
	//		if (!finished && !finishedHumans)
	//		{
	//			break;
	//		}
	//		if (actorData.GetTeam() == team)
	//		{
	//			ActorTurnSM actorTurnSM = actorData.GetActorTurnSM();
	//			bool flag = actorTurnSM.CurrentState == TurnStateEnum.CONFIRMED || actorTurnSM.CurrentState == TurnStateEnum.WAITING;
	//			if (!flag && actorData.IsDead())
	//			{
	//				flag = (actorData.LastDeathTurn == GameFlowData.Get().CurrentTurn || actorTurnSM.CurrentState != TurnStateEnum.PICKING_RESPAWN);
	//			}
	//			if (!flag)
	//			{
	//				finished = false;
	//				if (actorData.IsHumanControlled())
	//				{
	//					finishedHumans = false;
	//				}
	//			}
	//		}
	//	}
	//}

	// added in rogues
	//private void HandleUpdateTurnStartEffectActions_FCFS()
	//{
	//	if (!ServerActionBuffer.Get().GetPlayerActionFSM().IsExecutingEffects() && ServerActionBuffer.Get().GetPlayerActionFSM().IsAcceptingInput())
	//	{
	//		GameFlowData.Get().gameState = GameState.PVE_TeamActions;
	//		BotManager.Get().SetupBotsForDecision_FCFS();
	//	}
	//}

	// added in rogues
	private void HandleUpdateTurnEnd()  // HandleUpdateTeamTurnEnd_FCFS() in rogues
	{
		if (ServerCombatManager.Get().HasUnresolvedHealthEntries())
		{
			ServerCombatManager.Get().ResolveHitPoints();
		}
		if (ServerCombatManager.Get().HasUnresolvedTechPointsEntries())
		{
			ServerCombatManager.Get().ResolveTechPoints();
		}
		//Team nextActingTeam_FCFS = this.GetNextActingTeam_FCFS();

		// custom
		OnTurnEnd();
		GameFlowData.Get().gameState = GameState.BothTeams_Decision;
		// rogues
		//OnTeamTurnEnd(nextActingTeam_FCFS == Team.TeamA);
		//GameFlowData.Get().gameState = GameState.PVE_TeamTurnStart;
	}

	// added in rogues
	private void OnTurnEnd()  //  OnTeamTurnEnd(bool goingIntoNewTurn) in rogues
	{
		List<ActorData> actors = GameFlowData.Get().GetActors();
		foreach (ActorData actorData in actors)
		{
			if (actorData != null)  // && actorData.GetTeam() == GameFlowData.Get().ActingTeam in rogues
			{
				if (actorData.GetActorMovement() != null)
				{
					actorData.GetActorMovement().ClearPath();
				}
				if (actorData.GetActorBehavior() != null)
				{
					actorData.GetActorBehavior().ProcessMovementDeniedStat();
					actorData.GetActorBehavior().ProcessEnemySightedStat();
				}
			}
		}
		if (BrushCoordinator.Get() != null)  // goingIntoNewTurn && in rogues
		{
			BrushCoordinator.Get().OnTurnEnd();
		}
		//Team nextActingTeam_FCFS = this.GetNextActingTeam_FCFS();  // rogues
		ServerEffectManager.Get().OnTurnEnd();  // (nextActingTeam_FCFS) in rogues
		BarrierManager.Get().OnTurnEnd();  // (nextActingTeam_FCFS) in rogues

		foreach (ActorData actorData in actors)
		{
			if (actorData != null && actorData.GetPassiveData() != null)  //  && actorData.GetTeam() == GameFlowData.Get().ActingTeam in rogues
			{
				actorData.GetPassiveData().OnTurnEnd();
			}
			// rogues
			//if (actorData != null)
			//{
			//	actorData.GetActorTurnSM().ResetUsedAbilityAndMoveData();
			//}
		}

		//if (goingIntoNewTurn) // rogues
		//{
		// TODO CTF CTC
		//if (CaptureTheFlag.Get() != null)
		//{
		//	CaptureTheFlag.Get().OnTurnEnd();
		//}
		//if (CollectTheCoins.Get() != null)
		//{
		//	CollectTheCoins.Get().OnTurnEnd();
		//}
		if (ObjectivePoints.Get() != null)
		{
			ObjectivePoints.Get().OnTurnEnd();
		}
		// TODO BOTS
		//if (BotManager.Get() != null)
		//{
		//	BotManager.Get().OnTurnEnd();
		//}
		//}
		if (SinglePlayerManager.Get())
		{
			SinglePlayerManager.Get().OnResolutionEnd();
		}
		// TODO BOTS
		//if (NPCCoordinator.Get() != null)
		//{
		//	NPCCoordinator.Get().OnTurnEnd();  // goingIntoNewTurn && in rogues
		//}
	}
#endif

	// rogues
	//private void MirrorProcessed()
	//{
	//}

	protected static void InvokeRpcRpcDisplayConsoleText(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcDisplayConsoleText called on server.");
			return;
		}
		((GameFlow)obj).RpcDisplayConsoleText(GeneratedNetworkCode._ReadDisplayConsoleTextMessage_None(reader));
	}

	protected static void InvokeRpcRpcSetMatchTime(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSetMatchTime called on server.");
			return;
		}
		((GameFlow)obj).RpcSetMatchTime(reader.ReadSingle());
	}

	public void CallRpcDisplayConsoleText(DisplayConsoleTextMessage message)
	{
		// reactor
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcDisplayConsoleText called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcDisplayConsoleText);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		GeneratedNetworkCode._WriteDisplayConsoleTextMessage_None(networkWriter, message);
		SendRPCInternal(networkWriter, 0, "RpcDisplayConsoleText");
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//GeneratedNetworkCode._WriteDisplayConsoleTextMessage_None(networkWriter, message);
		//this.SendRPCInternal(typeof(GameFlow), "RpcDisplayConsoleText", networkWriter, 0);
	}

	public void CallRpcSetMatchTime(float timeSinceMatchStart)
	{
		// reactor
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcSetMatchTime called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcSetMatchTime);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(timeSinceMatchStart);
		SendRPCInternal(networkWriter, 0, "RpcSetMatchTime");
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//networkWriter.Write(timeSinceMatchStart);
		//this.SendRPCInternal(typeof(GameFlow), "RpcSetMatchTime", networkWriter, 0);
	}
}
