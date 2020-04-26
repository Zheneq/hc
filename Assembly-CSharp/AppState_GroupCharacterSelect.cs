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
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		if (ClientGameManager.Get() != null && ClientGameManager.Get().GroupInfo != null)
		{
			for (int i = 0; i < ClientGameManager.Get().GroupInfo.Members.Count; i++)
			{
				if (ClientGameManager.Get().GroupInfo.Members[i].AccountID != ClientGameManager.Get().GetPlayerAccountData().AccountId)
				{
					continue;
				}
				if (!ClientGameManager.Get().GroupInfo.Members[i].IsReady)
				{
					if (ClientGameManager.Get().PlayerInfo == null)
					{
						break;
					}
					if (!ClientGameManager.Get().PlayerInfo.IsSpectator)
					{
						break;
					}
				}
				return true;
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
		if (localizationArg_TimeSpan == null)
		{
			return;
		}
		while (true)
		{
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("RecentlyLeftGame", "Global"), LocalizationPayload.Create("RecentlyLeftGameDesc", "Global", localizationArg_GameType, localizationArg_TimeSpan).ToString(), StringUtil.TR("Ok", "Global"));
			return;
		}
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
		int result;
		if (!m_inQueue)
		{
			if (!m_wasRequeued)
			{
				result = (m_pendingJoinQueueRequest ? 1 : 0);
				goto IL_0036;
			}
		}
		result = 1;
		goto IL_0036;
		IL_0036:
		return (byte)result != 0;
	}

	public bool IsReady()
	{
		int result;
		if (!IsSelfReady())
		{
			if (m_inQueue)
			{
				result = ((!m_pendingLeaveQueueRequest) ? 1 : 0);
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

	public void CreateGame(GameType gameType, string mapName = null)
	{
		LobbyGameConfig gameConfig = new LobbyGameConfig();
		gameConfig.GameType = gameType;
		gameConfig.Map = mapName;
		BotDifficulty selectedBotSkillTeamA = BotDifficulty.Easy;
		BotDifficulty selectedBotSkillTeamB = BotDifficulty.Easy;
		if (gameType != GameType.Solo)
		{
			if (gameType != GameType.Coop)
			{
				goto IL_006b;
			}
		}
		selectedBotSkillTeamA = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().AllyBotDifficultyToDisplay;
		selectedBotSkillTeamB = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().EnemyBotDifficultyToDisplay;
		goto IL_006b;
		IL_006b:
		ClientGameManager.Get().CreateGame(gameConfig, ReadyState.Ready, selectedBotSkillTeamA, selectedBotSkillTeamB, delegate(CreateGameResponse response)
		{
			if (!response.Success)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						string description;
						if (response.LocalizedFailure == null)
						{
							description = (response.ErrorMessage.IsNullOrEmpty() ? StringUtil.TR("UnknownErrorTryAgain", "Frontend") : $"{response.ErrorMessage}#NeedsLocalization");
						}
						else
						{
							description = response.LocalizedFailure.ToString();
						}
						UIDialogPopupManager.OpenOneButtonDialog(string.Empty, description, StringUtil.TR("Ok", "Global"));
						if (gameConfig.GameType != GameType.Practice)
						{
							if (gameConfig.GameType != GameType.Solo)
							{
								if (gameConfig.GameType != GameType.Coop)
								{
									return;
								}
								if (!gameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
								{
									return;
								}
							}
						}
						m_readyForSoloGame = false;
						return;
					}
					}
				}
			}
		});
	}

	public void NotifyDroppedGroup()
	{
		if (!(UICharacterSelectScreenController.Get() != null))
		{
			return;
		}
		while (true)
		{
			UICharacterSelectScreenController.Get().NotifyDroppedGroup();
			return;
		}
	}

	public void NotifyJoinedNewGroup()
	{
		if (!m_inQueue)
		{
			return;
		}
		while (true)
		{
			NotifyQueueDrop();
			return;
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
		if (!(uICharacterSelectWorldObjects != null))
		{
			return;
		}
		while (true)
		{
			if (!ClientGameManager.Get().GroupInfo.InAGroup)
			{
				uICharacterSelectWorldObjects.m_ringAnimations[0].PlayAnimation("ReadyOut");
			}
			return;
		}
	}

	public void HandlePlayerInfoUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (!response.Success)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					UIDialogPopupManager.OpenOneButtonDialog(string.Empty, response.ErrorMessage, StringUtil.TR("Ok", "Global"));
					return;
				}
			}
		}
		if (response.PlayerInfo == null || response.PlayerInfo.PlayerId != GameManager.Get().PlayerInfo.PlayerId)
		{
			return;
		}
		while (true)
		{
			UICharacterSelectScreenController.Get().UpdateCharacters(response.PlayerInfo, null, GameManager.Get().GameplayOverrides);
			return;
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
		UIDialogPopupManager.OpenOneButtonDialog(string.Empty, StringUtil.TR("FailedStartGameServer", "Frontend"), StringUtil.TR("Ok", "Global"));
	}

	public void UpdateReadyState(bool ready)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		LobbyPlayerGroupInfo groupInfo = clientGameManager.GroupInfo;
		if (groupInfo == null)
		{
			return;
		}
		for (int i = 0; i < groupInfo.Members.Count; i++)
		{
			if (groupInfo.Members[i].AccountID == ClientGameManager.Get().GetPlayerAccountData().AccountId)
			{
				groupInfo.Members[i].IsReady = ready;
				break;
			}
		}
		BotDifficulty? allyDifficulty = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().AllyBotDifficultyToDisplay;
		BotDifficulty? enemyDifficulty = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().EnemyBotDifficultyToDisplay;
		if (ready)
		{
			if (enemyDifficulty.HasValue)
			{
				PlayerPrefs.SetInt("CoopDifficulty", (int)(1 + enemyDifficulty.Value));
			}
		}
		int num;
		if (ready)
		{
			num = 3;
		}
		else
		{
			num = 1;
		}
		ReadyState readyState = (ReadyState)num;
		if (groupInfo.SelectedQueueType != GameType.Practice)
		{
			if (groupInfo.SelectedQueueType != GameType.Solo)
			{
				if (groupInfo.InAGroup)
				{
					if (!groupInfo.IsLeader)
					{
						goto IL_013b;
					}
				}
				if (groupInfo.SelectedQueueType.IsQueueable())
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							if (ready)
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										break;
									default:
										if (!m_inQueue)
										{
											while (true)
											{
												switch (7)
												{
												case 0:
													break;
												default:
													if (groupInfo.IsLeader)
													{
														while (true)
														{
															switch (2)
															{
															case 0:
																break;
															default:
																clientGameManager.UpdateReadyState(readyState, allyDifficulty, enemyDifficulty, HandlePlayerGroupInfoUpdateResponse);
																return;
															}
														}
													}
													JoinQueue(ClientGameManager.Get().GroupInfo.SelectedQueueType);
													return;
												}
											}
										}
										return;
									}
								}
							}
							if (m_inQueue)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										break;
									default:
										Log.Info("Sending Leave Queue Request because setting myself not ready");
										LeaveQueue();
										return;
									}
								}
							}
							clientGameManager.UpdateReadyState(readyState, allyDifficulty, enemyDifficulty, HandlePlayerGroupInfoUpdateResponse);
							return;
						}
					}
				}
				Log.Error("Don't know what to do with a ready change when you're the leader of a group for a {0} game", groupInfo.SelectedQueueType);
				return;
			}
		}
		goto IL_013b;
		IL_013b:
		if (!ready)
		{
			if (groupInfo.InAGroup)
			{
				m_pendingLeaveQueueRequest = true;
			}
		}
		if (groupInfo.SelectedQueueType != GameType.Practice)
		{
			if (groupInfo.SelectedQueueType != GameType.Solo)
			{
				goto IL_0195;
			}
		}
		m_readyForSoloGame = true;
		UICharacterScreen.Get().DoRefreshFunctions(128);
		goto IL_0195;
		IL_0195:
		clientGameManager.UpdateReadyState(readyState, allyDifficulty, enemyDifficulty, HandlePlayerGroupInfoUpdateResponse);
	}

	public void HandlePlayerGroupInfoUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (!response.Success)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					Log.Info("HandlePlayerGroupInfoUpdateResponse failed response");
					if (response.LocalizedFailure != null)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								UIDialogPopupManager.OpenOneButtonDialog(string.Empty, response.LocalizedFailure.ToString(), StringUtil.TR("Ok", "Global"));
								return;
							}
						}
					}
					if (!response.ErrorMessage.IsNullOrEmpty())
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								UIDialogPopupManager.OpenOneButtonDialog(string.Empty, $"{response.ErrorMessage}#NeedsLocalization", StringUtil.TR("Ok", "Global"));
								return;
							}
						}
					}
					UIDialogPopupManager.OpenOneButtonDialog(string.Empty, StringUtil.TR("UnknownErrorTryAgain", "Frontend"), StringUtil.TR("Ok", "Global"));
					return;
				}
			}
		}
		if (response.OriginalPlayerInfoUpdate != null)
		{
			if (!response.OriginalPlayerInfoUpdate.ContextualReadyState.HasValue)
			{
				return;
			}
			while (true)
			{
				ContextualReadyState value = response.OriginalPlayerInfoUpdate.ContextualReadyState.Value;
				if (value.ReadyState != ReadyState.Ready)
				{
					return;
				}
				while (true)
				{
					if (ClientGameManager.Get().GroupInfo.SelectedQueueType != GameType.Practice)
					{
						if (ClientGameManager.Get().GroupInfo.SelectedQueueType != GameType.Solo)
						{
							return;
						}
					}
					CreateGame(ClientGameManager.Get().GroupInfo.SelectedQueueType);
					return;
				}
			}
		}
		Log.Error("HandlePlayerGroupInfoUpdateResponse :: OriginalPlayerInfoUpdate is null for player {0}!! This should never happen!", ClientGameManager.Get().Handle);
	}

	private void LeaveQueue()
	{
		if (m_pendingLeaveQueueRequest)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_pendingLeaveQueueRequest = true;
		ClientGameManager.Get().LobbyInterface.LeaveQueue(delegate(LeaveMatchmakingQueueResponse response)
		{
			m_pendingLeaveQueueRequest = false;
			if (response.Success)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						m_inQueue = false;
						if (IsReady())
						{
							UICharacterSelectScreenController.Get().SetReady(false);
						}
						return;
					}
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
		if (selectedQueueType.IsQueueable())
		{
			if (!groupInfo.InAGroup && m_inQueue)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						reason = StringUtil.TR("AlreadyQueued", "Frontend");
						return false;
					}
				}
			}
		}
		reason = string.Empty;
		return true;
	}

	public void JoinQueue(GameType gameType)
	{
		if (m_inQueue)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("Error", "Global"), StringUtil.TR("CantJoinAlreadyQueued", "Frontend"), StringUtil.TR("Ok", "Global"));
					return;
				}
			}
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
				string description;
				if (response.LocalizedFailure != null)
				{
					description = response.LocalizedFailure.ToString();
				}
				else if (response.ErrorMessage.IsNullOrEmpty())
				{
					description = StringUtil.TR("UnknownErrorTryAgain", "Global");
				}
				else
				{
					description = $"{response.ErrorMessage}#NeedsLocalization";
				}
				UIDialogPopupManager.OpenOneButtonDialog(string.Empty, description, StringUtil.TR("Ok", "Global"));
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
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("Requeued", "Global"), StringUtil.TR("SomeoneDroppedReaddedQueue", "Global"), StringUtil.TR("Ok", "Global"));
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
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (ClientGameManager.Get().GroupInfo == null)
			{
				return;
			}
			while (true)
			{
				if (ClientGameManager.Get().GroupInfo.SelectedQueueType == GameType.Ranked)
				{
					while (true)
					{
						UICharacterSelectScreenController.Get().SetupForRanked(true);
						return;
					}
				}
				return;
			}
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
		int num;
		if (!AppState.IsInGame() && GameManager.Get().GameInfo != null)
		{
			if (GameManager.Get().GameInfo.IsCustomGame)
			{
				num = ((GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped) ? 1 : 0);
				goto IL_0068;
			}
		}
		num = 0;
		goto IL_0068;
		IL_0068:
		bool flag = (byte)num != 0;
		if (GameManager.Get().GameInfo.AcceptTimeout != TimeSpan.Zero)
		{
		}
		if (!flag)
		{
			return;
		}
		while (true)
		{
			AppState_CharacterSelect.Get().Enter();
			return;
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
		if (notification.GameInfo.GameConfig.GameType != GameType.Coop)
		{
			return;
		}
		while (true)
		{
			UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
			{
				SelectedEnemyBotDifficulty = (int)notification.GameInfo.SelectedBotSkillTeamB
			});
			return;
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
		if (!(clientGameManager != null))
		{
			return;
		}
		while (true)
		{
			if (!clientGameManager.IsRegistered)
			{
				return;
			}
			while (true)
			{
				if (clientGameManager.IsReady)
				{
					while (true)
					{
						clientGameManager.RequestPreviousGameInfo(delegate(PreviousGameInfoResponse response)
						{
							if (AppState.GetCurrent() != this)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										break;
									default:
										return;
									}
								}
							}
							if (response.PreviousGameInfo != null)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										break;
									default:
										if (response.PreviousGameInfo.IsQueuedGame || response.PreviousGameInfo.IsCustomGame)
										{
											if (response.PreviousGameInfo.GameConfig.TotalHumanPlayers < 2)
											{
												while (true)
												{
													switch (7)
													{
													default:
														return;
													case 0:
														break;
													}
												}
											}
											PromptToRejoinGame(response.PreviousGameInfo);
										}
										return;
									}
								}
							}
						});
						return;
					}
				}
				return;
			}
		}
	}

	private void PromptToRejoinGame(LobbyGameInfo previousGameInfo)
	{
		UINewUserFlowManager.HideDisplay();
		m_messageBox = UIDialogPopupManager.OpenTwoButtonDialog(StringUtil.TR("Reconnect", "Global"), string.Format(StringUtil.TR("ReconnectUnderDevelopment", "Global"), previousGameInfo.GameConfig.GameType.GetDisplayName()), StringUtil.TR("Reconnect", "Global"), StringUtil.TR("Cancel", "Global"), delegate
		{
			Log.Info("Attempting to reconnect!");
			ClientGameManager.Get().RejoinGame(true);
			m_messageBox = null;
		}, delegate
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
