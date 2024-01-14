using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AppState_GroupCharacterSelect : AppState
{
	private static AppState_GroupCharacterSelect s_instance;
	private static Dictionary<GameType, DateTime> m_paroleTimesWarned = new Dictionary<GameType, DateTime>();

	private bool m_pendingJoinQueueRequest;
	private bool m_pendingLeaveQueueRequest;
	private bool m_inQueue;
	private bool m_readyForSoloGame;
	private bool m_receivedGameInfoNotification;
	private bool m_gameSelecting;
	private bool m_wasRequeued;

	private UIDialogBox m_messageBox;

	public static AppState_GroupCharacterSelect Get()
	{
		return s_instance;
	}

	private bool IsSelfReady()
	{
		if (m_readyForSoloGame)
		{
			return true;
		}

		if (ClientGameManager.Get() == null || ClientGameManager.Get().GroupInfo == null)
		{
			return false;
		}
		
		foreach (UpdateGroupMemberData memberData in ClientGameManager.Get().GroupInfo.Members)
		{
			if (memberData.AccountID == ClientGameManager.Get().GetPlayerAccountData().AccountId)
			{
				if (memberData.IsReady)
				{
					return true;
				}
				if (ClientGameManager.Get().PlayerInfo == null
				    || !ClientGameManager.Get().PlayerInfo.IsSpectator)
				{
					return false;
				}
			}
		}
		return false;
	}

	public static void Create()
	{
		AppState.Create<AppState_GroupCharacterSelect>();
	}

	public void SelectedGameMode(GameType gameType)
	{
		UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
		{
			ClientRequestedGameType = gameType
		});
		ClientGameManager.Get().RequestLoadouts(AbilityMod.GetRequiredModStrictnessForGameSubType() == ModStrictness.Ranked);
	}

	private void CheckForQueueParole()
	{
		LocalizationArg_TimeSpan localizationArg_TimeSpan = null;
		LocalizationArg_GameType localizationArg_GameType = null;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager != null && clientGameManager.GameTypeAvailabilies != null)
		{
			foreach (KeyValuePair<GameType, GameTypeAvailability> gameTypeAvailabily in clientGameManager.GameTypeAvailabilies)
			{
				GameType key = gameTypeAvailabily.Key;
				DateTime? paroleTimeout = gameTypeAvailabily.Value.ParoleTimeout;
				if (paroleTimeout.HasValue)
				{
					if (!m_paroleTimesWarned.ContainsKey(key))
					{
						m_paroleTimesWarned[key] = paroleTimeout.Value;
						localizationArg_TimeSpan = LocalizationArg_TimeSpan.Create(paroleTimeout.Value - DateTime.UtcNow);
						localizationArg_GameType = LocalizationArg_GameType.Create(key);
						break;
					}
					if (m_paroleTimesWarned[key] != paroleTimeout.Value)
					{
						m_paroleTimesWarned[key] = paroleTimeout.Value;
						localizationArg_TimeSpan = LocalizationArg_TimeSpan.Create(paroleTimeout.Value - DateTime.UtcNow);
						localizationArg_GameType = LocalizationArg_GameType.Create(key);
						break;
					}
				}
				else if (m_paroleTimesWarned.ContainsKey(gameTypeAvailabily.Key))
				{
					m_paroleTimesWarned.Remove(gameTypeAvailabily.Key);
				}
			}
		}

		if (localizationArg_TimeSpan != null)
			UIDialogPopupManager.OpenOneButtonDialog(
				StringUtil.TR("RecentlyLeftGame", "Global"),
				LocalizationPayload.Create("RecentlyLeftGameDesc", "Global", localizationArg_GameType, localizationArg_TimeSpan).ToString(),
				StringUtil.TR("Ok", "Global"));
	}

	public override void Enter()
	{
		base.Enter();
		UICharacterScreen.Get().DoRefreshFunctions(ushort.MaxValue);
	}

	public void ReEnter(bool wasRequeued)
	{
		m_wasRequeued = wasRequeued;
		Enter();
	}

	public bool InQueue()
	{
		return m_inQueue || m_wasRequeued || m_pendingJoinQueueRequest;
	}

	public bool IsReady()
	{
		return IsSelfReady() || m_inQueue && !m_pendingLeaveQueueRequest;
	}

	public void CreateGame(GameType gameType, string mapName = null)
	{
		LobbyGameConfig gameConfig = new LobbyGameConfig
		{
			GameType = gameType,
			Map = mapName
		};
		BotDifficulty selectedBotSkillTeamA = BotDifficulty.Easy;
		BotDifficulty selectedBotSkillTeamB = BotDifficulty.Easy;
		if (gameType == GameType.Solo || gameType == GameType.Coop)
		{
			selectedBotSkillTeamA = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().AllyBotDifficultyToDisplay;
			selectedBotSkillTeamB = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().EnemyBotDifficultyToDisplay;
		}

		ClientGameManager.Get().CreateGame(gameConfig,
			ReadyState.Ready,
			selectedBotSkillTeamA,
			selectedBotSkillTeamB,
			response =>
			{
				if (response.Success)
				{
					return;
				}

				UIDialogPopupManager.OpenOneButtonDialog(
					string.Empty, 
					response.LocalizedFailure != null
						? response.LocalizedFailure.ToString()
						: response.ErrorMessage.IsNullOrEmpty()
							? StringUtil.TR("UnknownErrorTryAgain", "Frontend")
							: $"{response.ErrorMessage}#NeedsLocalization",
					StringUtil.TR("Ok", "Global"));
				if (gameConfig.GameType == GameType.Practice
				    || gameConfig.GameType == GameType.Solo
				    || (gameConfig.GameType == GameType.Coop
				        && gameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial)))
				{
					m_readyForSoloGame = false;
				}
			});
	}

	public void NotifyDroppedGroup()
	{
		if (UICharacterSelectScreenController.Get() != null)
		{
			UICharacterSelectScreenController.Get().NotifyDroppedGroup();
		}
	}

	public void NotifyJoinedNewGroup()
	{
		if (m_inQueue)
		{
			NotifyQueueDrop();
		}
	}

	public void NotifyQueueDrop()
	{
		m_pendingLeaveQueueRequest = false;
		m_inQueue = false;
		UICharacterSelectScreenController uICharacterSelectScreenController = UICharacterSelectScreenController.Get();
		if (uICharacterSelectScreenController != null)
		{
			UIManager.SetGameObjectActive(uICharacterSelectScreenController.m_readyBtn, true);
			UICharacterSelectWorldObjects.Get().SetCharacterReady(0, false);
			uICharacterSelectScreenController.m_readyBtn.spriteController.SetClickable(true);
			UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, false);
			NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(false);
			NavigationBar.Get().m_searchQueueText.text = string.Empty;
			UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
			{
				ClientRequestToServerSelectCharacter = UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay
			});
		}
		UICharacterSelectWorldObjects uICharacterSelectWorldObjects = UICharacterSelectWorldObjects.Get();
		if (uICharacterSelectWorldObjects != null && !ClientGameManager.Get().GroupInfo.InAGroup)
		{
			uICharacterSelectWorldObjects.m_ringAnimations[0].PlayAnimation("ReadyOut");
		}
	}

	public void HandlePlayerInfoUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (!response.Success)
		{
			UIDialogPopupManager.OpenOneButtonDialog(string.Empty, response.ErrorMessage, StringUtil.TR("Ok", "Global"));
		}
		else if (response.PlayerInfo != null && response.PlayerInfo.PlayerId == GameManager.Get().PlayerInfo.PlayerId)
		{
			UICharacterSelectScreenController.Get().UpdateCharacters(response.PlayerInfo, null, GameManager.Get().GameplayOverrides);
		}
	}

	private void HandleGameLaunched(GameType gameType)
	{
		m_pendingLeaveQueueRequest = false;
		m_inQueue = false;
		AppState_GameLoading.Get().Enter(gameType);
	}

	private void HandleGameStopped(GameResult gameResult)
	{
		UIDialogPopupManager.OpenOneButtonDialog(
			string.Empty,
			StringUtil.TR("FailedStartGameServer", "Frontend"),
			StringUtil.TR("Ok", "Global"));
	}

	public void UpdateReadyState(bool ready)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		LobbyPlayerGroupInfo groupInfo = clientGameManager.GroupInfo;
		if (groupInfo == null)
		{
			return;
		}
		foreach (UpdateGroupMemberData memberData in groupInfo.Members)
		{
			if (memberData.AccountID == ClientGameManager.Get().GetPlayerAccountData().AccountId)
			{
				memberData.IsReady = ready;
				break;
			}
		}
		BotDifficulty? allyDifficulty = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().AllyBotDifficultyToDisplay;
		BotDifficulty? enemyDifficulty = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().EnemyBotDifficultyToDisplay;
		if (ready && enemyDifficulty.HasValue)
		{
			PlayerPrefs.SetInt("CoopDifficulty", (int)(1 + enemyDifficulty.Value));
		}

		ReadyState readyState = ready ? ReadyState.Ready : ReadyState.Accepted;
		if (groupInfo.SelectedQueueType != GameType.Practice
		    && groupInfo.SelectedQueueType != GameType.Solo
		    && (!groupInfo.InAGroup || groupInfo.IsLeader))
		{
			if (groupInfo.SelectedQueueType.IsQueueable())
			{
				if (ready)
				{
					if (!m_inQueue)
					{
						if (groupInfo.IsLeader)
						{
							clientGameManager.UpdateReadyState(readyState, allyDifficulty, enemyDifficulty,
								HandlePlayerGroupInfoUpdateResponse);
						}
						else
						{
							JoinQueue(ClientGameManager.Get().GroupInfo.SelectedQueueType);
						}
					}
				}
				else if (m_inQueue)
				{
					Log.Info("Sending Leave Queue Request because setting myself not ready");
					LeaveQueue();
				}
				else
				{
					clientGameManager.UpdateReadyState(readyState, allyDifficulty, enemyDifficulty,
						HandlePlayerGroupInfoUpdateResponse);
				}
			}
			else
			{
				Log.Error(
					"Don't know what to do with a ready change when you're the leader of a group for a {0} game",
					groupInfo.SelectedQueueType);
			}
		}
		else
		{
			if (!ready && groupInfo.InAGroup)
			{
				m_pendingLeaveQueueRequest = true;
			}

			if (groupInfo.SelectedQueueType == GameType.Practice || groupInfo.SelectedQueueType == GameType.Solo)
			{
				m_readyForSoloGame = true;
				UICharacterScreen.Get().DoRefreshFunctions(128);
			}
			clientGameManager.UpdateReadyState(readyState, allyDifficulty, enemyDifficulty, HandlePlayerGroupInfoUpdateResponse);
		}
	}

	public void HandlePlayerGroupInfoUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (!response.Success)
		{
			Log.Info("HandlePlayerGroupInfoUpdateResponse failed response");
			UIDialogPopupManager.OpenOneButtonDialog(
				string.Empty,
				response.LocalizedFailure != null
					? response.LocalizedFailure.ToString()
					: response.ErrorMessage.IsNullOrEmpty() 
						? StringUtil.TR("UnknownErrorTryAgain", "Frontend") 
						: $"{response.ErrorMessage}#NeedsLocalization",
				StringUtil.TR("Ok", "Global"));
		}
		else if (response.OriginalPlayerInfoUpdate != null)
		{
			if (response.OriginalPlayerInfoUpdate.ContextualReadyState.HasValue)
			{
				ContextualReadyState value = response.OriginalPlayerInfoUpdate.ContextualReadyState.Value;
				if (value.ReadyState == ReadyState.Ready
				    && (ClientGameManager.Get().GroupInfo.SelectedQueueType == GameType.Practice
				        || ClientGameManager.Get().GroupInfo.SelectedQueueType == GameType.Solo))
				{
					CreateGame(ClientGameManager.Get().GroupInfo.SelectedQueueType);
				}
			}
		}
		else
		{
			Log.Error("HandlePlayerGroupInfoUpdateResponse :: OriginalPlayerInfoUpdate is null for player {0}!! " +
			          "This should never happen!",
				ClientGameManager.Get().Handle);
		}
	}

	private void LeaveQueue()
	{
		if (m_pendingLeaveQueueRequest)
		{
			return;
		}
		m_pendingLeaveQueueRequest = true;
		ClientGameManager.Get().LobbyInterface.LeaveQueue(delegate(LeaveMatchmakingQueueResponse response)
		{
			m_pendingLeaveQueueRequest = false;
			if (response.Success)
			{
				m_inQueue = false;
				if (IsReady())
				{
					UICharacterSelectScreenController.Get().SetReady(false);
				}
			}
		});
	}

	public void ForceJoinQueue()
	{
		m_pendingLeaveQueueRequest = false;
		m_inQueue = true;
		NavigationBar.Get().UpdateStatusMessage();
	}

	public bool CanJoinQueue(out string reason)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		LobbyPlayerGroupInfo groupInfo = clientGameManager.GroupInfo;
		GameType selectedQueueType = groupInfo.SelectedQueueType;
		if (selectedQueueType.IsQueueable() && !groupInfo.InAGroup && m_inQueue)
		{
			reason = StringUtil.TR("AlreadyQueued", "Frontend");
			return false;
		}
		else
		{
			reason = string.Empty;
			return true;
		}
	}

	public void JoinQueue(GameType gameType)
	{
		if (m_inQueue)
		{
			UIDialogPopupManager.OpenOneButtonDialog(
				StringUtil.TR("Error", "Global"),
				StringUtil.TR("CantJoinAlreadyQueued", "Frontend"),
				StringUtil.TR("Ok", "Global"));
			return;
		}
		if (m_pendingJoinQueueRequest)
		{
			return;
		}
		ClientGameManager clientGameManager = ClientGameManager.Get();
		BotDifficulty? allyDifficulty = null;
		BotDifficulty? enemyDifficulty = null;
		if (!gameType.IsHumanVsHumanGame())
		{
			allyDifficulty = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().AllyBotDifficultyToDisplay;
			enemyDifficulty = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().EnemyBotDifficultyToDisplay;
		}
		m_pendingJoinQueueRequest = true;
		clientGameManager.JoinQueue(gameType, allyDifficulty, enemyDifficulty, delegate(JoinMatchmakingQueueResponse response)
		{
			m_pendingJoinQueueRequest = false;
			if (response.Success)
			{
				ForceJoinQueue();
			}
			else
			{
				UIDialogPopupManager.OpenOneButtonDialog(
					string.Empty, 
					response.LocalizedFailure != null
					? response.LocalizedFailure.ToString()
					: response.ErrorMessage.IsNullOrEmpty()
						? StringUtil.TR("UnknownErrorTryAgain", "Global")
						: $"{response.ErrorMessage}#NeedsLocalization",
					StringUtil.TR("Ok", "Global"));
			}
		});
	}

	public void NotifyEnteredCharacterSelect()
	{
		m_pendingLeaveQueueRequest = false;
		m_inQueue = false;
		m_readyForSoloGame = false;
	}

	public void NotifyEnteredRankModeDraft()
	{
		m_inQueue = false;
	}

	protected override void OnEnter()
	{
		GameManager gameManager = GameManager.Get();
		m_receivedGameInfoNotification = false;
		m_gameSelecting = false;
		m_readyForSoloGame = false;
		AudioManager.GetMixerSnapshotManager().SetMix_Menu();
		ShowScreen();
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnQueueStatusNotification += HandleQueueStatusNotification;
		clientGameManager.OnDisconnectedFromLobbyServer += HandleDisconnectedFromLobbyServer;
		clientGameManager.OnGameInfoNotification += HandleGameInfoNotification;
		clientGameManager.OnLobbyStatusNotification += HandleStatusNotification;
		clientGameManager.OnServerQueueConfigurationUpdateNotification += HandleGameTypesAvailability;
		gameManager.OnGameAssembling += HandleGameAssembling;
		gameManager.OnGameSelecting += HandleGameSelecting;
		gameManager.OnGameLaunched += HandleGameLaunched;
		gameManager.OnGameStopped += HandleGameStopped;
		if (m_wasRequeued)
		{
			m_inQueue = true;
			m_wasRequeued = false;
			UIDialogPopupManager.OpenOneButtonDialog(
				StringUtil.TR("Requeued", "Global"),
				StringUtil.TR("SomeoneDroppedReaddedQueue", "Global"),
				StringUtil.TR("Ok", "Global"));
			UICharacterSelectScreenController.Get().SetReady(true);
		}
		else
		{
			CheckForPreviousGame();
		}
		CheckForQueueParole();
		GameEventManager.Get().FireEvent(GameEventManager.EventType.FrontEndSelectionChatterCue, null);
	}

	internal static void ShowScreen()
	{
		GameManager gameManager = GameManager.Get();
		UIFrontEnd.Get().m_frontEndNavPanel.SetPlayMenuCatgeoryVisible(true);
		UICharacterSelectScreenController.Get().QuickPlaySetup(gameManager.GameInfo);
		UICharacterSelectScreenController.Get().QuickPlayUpdateCharacters(gameManager.GameplayOverrides);
		UICharacterSelectScreen.Get().SetGameSettingsButtonVisibility(false);
		UICharacterSelectScreenController.Get().NotifyGroupUpdate();
		UIFrontEnd.Get().ShowScreen(FrontEndScreenState.GroupCharacterSelect);
		if (ClientGameManager.Get() != null
		    && ClientGameManager.Get().GroupInfo != null
		    && ClientGameManager.Get().GroupInfo.SelectedQueueType == GameType.Ranked)
		{
			UICharacterSelectScreenController.Get().SetupForRanked(true);
		}
	}

	public void HandleGameTypesAvailability(ServerQueueConfigurationUpdateNotification notification)
	{
		UIPlayCategoryMenu.Get().UpdateGameTypeAvailability(notification.GameTypeAvailabilies);
	}

	public void HandleStatusNotification(LobbyStatusNotification notification)
	{
		UIFrontEnd.Get().m_frontEndNavPanel.SetShopVisible(GameManager.Get().GameplayOverrides.EnableShop);
		UIFrontEnd.Get().m_frontEndNavPanel.CheckSeasonsVisibility();
	}

	public void HandleQueueStatusNotification(MatchmakingQueueStatusNotification notification)
	{
		NavigationBar.Get().UpdateStatusMessage();
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
	{
		m_inQueue = false;
		AppState_LandingPage.Get().Enter(lastLobbyErrorMessage);
	}

	private void HandleGameAssembling()
	{
		bool flag = !IsInGame()
		            && GameManager.Get().GameInfo != null
		            && GameManager.Get().GameInfo.IsCustomGame
		            && GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped;
		if (GameManager.Get().GameInfo.AcceptTimeout != TimeSpan.Zero)
		{
		}
		if (flag)
		{
			AppState_CharacterSelect.Get().Enter();
		}
	}

	private void HandleGameInfoNotification(GameInfoNotification notification)
	{
		m_receivedGameInfoNotification = true;
		if (m_receivedGameInfoNotification && m_gameSelecting)
		{
			if (UIMatchStartPanel.Get() != null)
			{
				UIMatchStartPanel.Get().HandleGameStatusChanged(notification);
			}
			if (ClientGameManager.Get().GroupInfo.SelectedQueueType != GameType.Ranked)
			{
				AppState_CharacterSelect.Get().Enter();
			}
		}

		if (notification.GameInfo.GameConfig.GameType == GameType.Coop)
		{
			UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
			{
				SelectedEnemyBotDifficulty = (int)notification.GameInfo.SelectedBotSkillTeamB
			});
			
		}
	}

	private void HandleGameSelecting()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager.GroupInfo.SelectedQueueType.IsQueueable() && !GameManager.Get().GameInfo.IsCustomGame)
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.NotifyMatchFound);
			WinUtils.FlashWindow();
			WinUtils.BringApplicationToFront();
			QuestListPanel.Get().SetVisible(false);
			m_gameSelecting = true;
		}
		if (m_receivedGameInfoNotification && m_gameSelecting)
		{
			AppState_CharacterSelect.Get().Enter();
		}
		if (GameManager.Get().GameInfo.IsCustomGame)
		{
			AppState_CharacterSelect.Get().Enter();
			UIRankedModeSelectScreen.Get().SetVisible(false);
		}
	}

	protected override void OnLeave()
	{
		GameManager gameManager = GameManager.Get();
		gameManager.OnGameLaunched -= HandleGameLaunched;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnQueueStatusNotification -= HandleQueueStatusNotification;
		clientGameManager.OnDisconnectedFromLobbyServer -= HandleDisconnectedFromLobbyServer;
		clientGameManager.OnGameInfoNotification -= HandleGameInfoNotification;
		clientGameManager.OnLobbyStatusNotification -= HandleStatusNotification;
		clientGameManager.OnServerQueueConfigurationUpdateNotification -= HandleGameTypesAvailability;
		gameManager.OnGameAssembling -= HandleGameAssembling;
		gameManager.OnGameSelecting -= HandleGameSelecting;
		gameManager.OnGameLaunched -= HandleGameLaunched;
		gameManager.OnGameStopped -= HandleGameStopped;
		m_readyForSoloGame = false;
		if (m_messageBox != null)
		{
			m_messageBox.Close();
			m_messageBox = null;
		}
		UICharacterScreen.Get().DoRefreshFunctions(128);
	}

	private void CheckForPreviousGame()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager == null
		    || !clientGameManager.IsRegistered
		    || !clientGameManager.IsReady)
		{
			return;
		}
		clientGameManager.RequestPreviousGameInfo(response =>
		{
			if (GetCurrent() == this
			    && response.PreviousGameInfo != null
			    && (response.PreviousGameInfo.IsQueuedGame || response.PreviousGameInfo.IsCustomGame)
			    && response.PreviousGameInfo.GameConfig.TotalHumanPlayers >= 2)
			{
				PromptToRejoinGame(response.PreviousGameInfo);
			}
		});
	}

	private void PromptToRejoinGame(LobbyGameInfo previousGameInfo)
	{
		UINewUserFlowManager.HideDisplay();
		m_messageBox = UIDialogPopupManager.OpenTwoButtonDialog(
			StringUtil.TR("Reconnect", "Global"),
			string.Format(StringUtil.TR("ReconnectUnderDevelopment", "Global"),
				previousGameInfo.GameConfig.GameType.GetDisplayName()),
			StringUtil.TR("Reconnect", "Global"), 
			StringUtil.TR("Cancel", "Global"),
			reference =>
			{
				Log.Info("Attempting to reconnect!");
				ClientGameManager.Get().RejoinGame(true);
				m_messageBox = null;
			}, reference =>
			{
				Log.Info("Decided not to reconnect!");
				ClientGameManager.Get().RejoinGame(false);
				m_messageBox = null;
			});
	}

	private void Awake()
	{
		s_instance = this;
	}
}
