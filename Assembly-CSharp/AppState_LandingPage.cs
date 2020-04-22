using LobbyGameClientMessages;
using System;

public class AppState_LandingPage : AppState
{
	private static AppState_LandingPage s_instance;

	private UIDialogBox m_messageBox;

	private string m_lastLobbyErrorMessage;

	private bool m_receivedLobbyinfo;

	private bool m_returningFromGroupCharacterSelect;

	private bool m_goToCharacterSelect;

	public bool ReceivedLobbyStatusInfo => m_receivedLobbyinfo;

	public static AppState_LandingPage Get()
	{
		return s_instance;
	}

	public void Enter(bool returningFromGroupCharacterSelect)
	{
		m_returningFromGroupCharacterSelect = returningFromGroupCharacterSelect;
		base.Enter();
	}

	public void Enter(string lastLobbyErrorMessage, bool goToCharacterSelect = false)
	{
		m_lastLobbyErrorMessage = lastLobbyErrorMessage;
		m_goToCharacterSelect = goToCharacterSelect;
		base.Enter();
	}

	public static void Create()
	{
		AppState.Create<AppState_LandingPage>();
	}

	private void Awake()
	{
		s_instance = this;
	}

	protected override void OnEnter()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		UIFrontEnd.Get().m_landingPageScreen.m_inCustomGame = false;
		AudioManager.GetMixerSnapshotManager().SetMix_Menu();
		UIFrontEnd.Get().SetVisible(true);
		UIFrontEnd.Get().EnableFrontendEnvironment(true);
		UIFrontEnd.Get().ShowScreen(FrontEndScreenState.LandingPage);
		UIFrontEnd.Get().m_landingPageScreen.ShowMOTD();
		UIFrontEnd.Get().m_landingPageScreen.SetServerIsLocked(clientGameManager.IsServerLocked);
		UIFrontEnd.Get().m_frontEndNavPanel.SetShopVisible(GameManager.Get().GameplayOverrides.EnableShop);
		UIFrontEnd.Get().m_frontEndNavPanel.CheckSeasonsVisibility();
		UIManager.SetGameObjectActive(UISystemMenuPanel.Get(), true);
		clientGameManager.OnConnectedToLobbyServer += HandleConnectedToLobbyServer;
		clientGameManager.OnDisconnectedFromLobbyServer += HandleDisconnectedFromLobbyServer;
		clientGameManager.OnLobbyServerReadyNotification += HandleLobbyServerReadyNotification;
		clientGameManager.OnLobbyStatusNotification += HandleStatusNotification;
		clientGameManager.OnAccountDataUpdated += HandleAccountDataUpdated;
		clientGameManager.OnChatNotification += HandleChatNotification;
		GameManager.Get().OnGameAssembling += HandleGameAssembling;
		GameManager.Get().OnGameSelecting += HandleGameSelecting;
		GameManager.Get().OnGameLaunched += HandleGameLaunched;
		ConnectToLobbyServer();
		if (m_lastLobbyErrorMessage != null && m_messageBox == null)
		{
			UINewUserFlowManager.HideDisplay();
			string lastLobbyErrorMessage = m_lastLobbyErrorMessage;
			m_lastLobbyErrorMessage = null;
			m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(string.Empty, lastLobbyErrorMessage, StringUtil.TR("Ok", "Global"), delegate
			{
				m_messageBox = null;
			});
		}
		if (clientGameManager != null)
		{
			if (clientGameManager.IsConnectedToLobbyServer)
			{
				if (UILandingPageScreen.Get() == null)
				{
					clientGameManager.SendCheckAccountStatusRequest(HandleCheckAccountStatusResponse);
					clientGameManager.SendCheckRAFStatusRequest(false);
				}
			}
		}
		if (UILoadingScreenPanel.Get() != null)
		{
			UILoadingScreenPanel.Get().SetVisible(false);
		}
		if (clientGameManager.IsPlayerAccountDataAvailable())
		{
			HandleAccountDataUpdated(clientGameManager.GetPlayerAccountData());
		}
		UINewUserFlowManager.HighlightQueued();
		UINewUserFlowManager.OnNavBarDisplayed();
		if (HighlightUtils.Get() != null)
		{
			HighlightUtils.Get().HideCursorHighlights();
		}
		CheckForPreviousGame();
		if (clientGameManager.IsServerLocked)
		{
			m_goToCharacterSelect = false;
		}
		if (m_goToCharacterSelect)
		{
			goto IL_031f;
		}
		if (!m_returningFromGroupCharacterSelect)
		{
			if (clientGameManager != null && clientGameManager.GroupInfo != null)
			{
				if (clientGameManager.GroupInfo.InAGroup)
				{
					goto IL_031f;
				}
			}
		}
		if (UIRankedModeSelectScreen.Get() != null)
		{
			UIRankedModeSelectScreen.Get().SetVisible(false);
		}
		goto IL_042d;
		IL_031f:
		m_goToCharacterSelect = false;
		UIFrontendLoadingScreen.Get().SetVisible(false);
		AppState_GroupCharacterSelect.Get().Enter();
		UIFrontEnd.Get().m_frontEndNavPanel.SetNavButtonSelected(UIFrontEnd.Get().m_frontEndNavPanel.m_PlayBtn);
		if (UIRankedModeSelectScreen.Get() != null)
		{
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().GroupInfo != null)
				{
					if (ClientGameManager.Get().GroupInfo.SelectedQueueType == GameType.Ranked)
					{
						UIFrontEnd.Get().ShowScreen(FrontEndScreenState.RankedModeSelect);
						UIRankedModeSelectScreen.Get().SetVisible(true);
						goto IL_042d;
					}
				}
			}
			UIFrontEnd.Get().ShowScreen(FrontEndScreenState.GroupCharacterSelect);
			UIRankedModeSelectScreen.Get().SetVisible(false);
		}
		goto IL_042d;
		IL_042d:
		m_returningFromGroupCharacterSelect = false;
	}

	protected override void OnLeave()
	{
		if (m_messageBox != null)
		{
			m_messageBox.Close();
			m_messageBox = null;
		}
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnConnectedToLobbyServer -= HandleConnectedToLobbyServer;
		clientGameManager.OnDisconnectedFromLobbyServer -= HandleDisconnectedFromLobbyServer;
		clientGameManager.OnLobbyServerReadyNotification -= HandleLobbyServerReadyNotification;
		clientGameManager.OnLobbyStatusNotification -= HandleStatusNotification;
		clientGameManager.OnAccountDataUpdated -= HandleAccountDataUpdated;
		clientGameManager.OnChatNotification -= HandleChatNotification;
		GameManager.Get().OnGameAssembling -= HandleGameAssembling;
		GameManager.Get().OnGameSelecting -= HandleGameSelecting;
		GameManager.Get().OnGameLaunched -= HandleGameLaunched;
	}

	private void HandleGameLaunched(GameType gameType)
	{
		UIFrontEnd.Get().NotifyGameLaunched();
		AppState_GameLoading.Get().Enter(gameType);
	}

	public void ConnectToLobbyServer()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (!clientGameManager.IsConnectedToLobbyServer)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if ((bool)m_messageBox)
					{
						m_messageBox.Close();
						m_messageBox = null;
					}
					if (!m_lastLobbyErrorMessage.IsNullOrEmpty())
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
							{
								string lastLobbyErrorMessage = m_lastLobbyErrorMessage;
								m_lastLobbyErrorMessage = null;
								UINewUserFlowManager.HideDisplay();
								if (clientGameManager.AllowRelogin)
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											break;
										default:
										{
											string empty = string.Empty;
											string description = string.Format(StringUtil.TR("PressOkToReconnect", "Global"), lastLobbyErrorMessage);
											string leftButtonLabel = StringUtil.TR("Ok", "Global");
											string rightButtonLabel = StringUtil.TR("Cancel", "Global");
											UIDialogBox.DialogButtonCallback leftButtonCallback = delegate
											{
												ConnectToLobbyServer();
											};
											if (_003C_003Ef__am_0024cache0 == null)
											{
												_003C_003Ef__am_0024cache0 = delegate
												{
													AppState_Shutdown.Get().Enter();
												};
											}
											m_messageBox = UIDialogPopupManager.OpenTwoButtonDialog(empty, description, leftButtonLabel, rightButtonLabel, leftButtonCallback, _003C_003Ef__am_0024cache0);
											return;
										}
										}
									}
								}
								m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(string.Empty, string.Format(StringUtil.TR("PressOkToExit", "Global"), lastLobbyErrorMessage), StringUtil.TR("Ok", "Global"), delegate
								{
									AppState_Shutdown.Get().Enter();
								});
								return;
							}
							}
						}
					}
					try
					{
						m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(string.Empty, StringUtil.TR("ConnectingToLobbyServer", "Global"), StringUtil.TR("Cancel", "Global"), delegate
						{
							AppState_Shutdown.Get().Enter();
						});
						clientGameManager.ConnectToLobbyServer();
					}
					catch (Exception ex)
					{
						if (m_messageBox != null)
						{
							m_messageBox.Close();
							m_messageBox = null;
						}
						UINewUserFlowManager.HideDisplay();
						string empty2 = string.Empty;
						string description2 = string.Format(StringUtil.TR("FailedToConnectToLobbyServer", "Global"), ex.Message);
						string buttonLabelText = StringUtil.TR("Ok", "Global");
						if (_003C_003Ef__am_0024cache3 == null)
						{
							_003C_003Ef__am_0024cache3 = delegate
							{
								AppState_Shutdown.Get().Enter();
							};
						}
						m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(empty2, description2, buttonLabelText, _003C_003Ef__am_0024cache3);
					}
					return;
				}
			}
		}
		UIFrontEnd.Get().m_landingPageScreen.ShowMOTD();
		UIFrontEnd.Get().m_landingPageScreen.SetServerIsLocked(clientGameManager.IsServerLocked);
	}

	private void Update()
	{
	}

	public void HandleConnectedToLobbyServer(RegisterGameClientResponse response)
	{
		if ((bool)m_messageBox)
		{
			m_messageBox.Close();
			m_messageBox = null;
		}
		if (response.Success)
		{
			return;
		}
		while (true)
		{
			if (response.LocalizedFailure != null)
			{
				response.ErrorMessage = response.LocalizedFailure.ToString();
			}
			if (response.ErrorMessage.IsNullOrEmpty())
			{
				response.ErrorMessage = StringUtil.TR("UnknownError", "Global");
			}
			UINewUserFlowManager.HideDisplay();
			string text;
			if (response.ErrorMessage == "INVALID_PROTOCOL_VERSION")
			{
				text = StringUtil.TR("NotRecentVersionOfTheGame", "Frontend");
			}
			else if (response.ErrorMessage == "INVALID_IP_ADDRESS")
			{
				text = StringUtil.TR("IPAddressChanged", "Frontend");
			}
			else if (response.ErrorMessage == "ACCOUNT_BANNED")
			{
				text = StringUtil.TR("AccountBanned", "Frontend");
			}
			else
			{
				text = string.Format(StringUtil.TR("FailedToConnectToLobbyServer", "Global"), response.ErrorMessage);
			}
			string empty = string.Empty;
			string description = text;
			string buttonLabelText = StringUtil.TR("Ok", "Global");
			if (_003C_003Ef__am_0024cache4 == null)
			{
				_003C_003Ef__am_0024cache4 = delegate
				{
					AppState_Shutdown.Get().Enter();
				};
			}
			m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(empty, description, buttonLabelText, _003C_003Ef__am_0024cache4);
			return;
		}
	}

	public void HandleLobbyServerReadyNotification(LobbyServerReadyNotification notification)
	{
		CheckForPreviousGame();
	}

	public void HandleAccountDataUpdated(PersistedAccountData accountData)
	{
		UIFrontEnd.Get().m_landingPageScreen.UpdateMatchData();
	}

	public void HandleStatusNotification(LobbyStatusNotification notification)
	{
		m_receivedLobbyinfo = true;
		UIFrontEnd.Get().m_landingPageScreen.ShowMOTD();
		UIFrontEnd.Get().m_landingPageScreen.SetServerIsLocked(ClientGameManager.Get().IsServerLocked);
		UIFrontEnd.Get().m_frontEndNavPanel.SetShopVisible(GameManager.Get().GameplayOverrides.EnableShop);
		UIFrontEnd.Get().m_frontEndNavPanel.CheckSeasonsVisibility();
	}

	private void CheckForPreviousGame()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager != null)
		{
			if (clientGameManager.IsRegistered)
			{
				if (clientGameManager.IsReady)
				{
					Log.Info("Checking for previous game");
					clientGameManager.RequestPreviousGameInfo(delegate(PreviousGameInfoResponse response)
					{
						if (AppState.GetCurrent() != this)
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
						if (response.PreviousGameInfo != null)
						{
							if (!response.PreviousGameInfo.IsQueuedGame)
							{
								if (!response.PreviousGameInfo.IsCustomGame)
								{
									return;
								}
							}
							if (response.PreviousGameInfo.GameConfig.TotalHumanPlayers < 2)
							{
								while (true)
								{
									switch (2)
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
					});
					return;
				}
			}
		}
		Log.Info("Not checking for previous game-- {0}/{1}", clientGameManager.IsRegistered, clientGameManager.IsReady);
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
		if (!(ClientCrashReportDetector.Get().m_crashDialog != null))
		{
			return;
		}
		while (true)
		{
			ClientCrashReportDetector.Get().m_crashDialog.gameObject.transform.SetAsLastSibling();
			return;
		}
	}

	private void HandleChatNotification(ChatNotification notification)
	{
		if (notification.ConsoleMessageType != ConsoleMessageType.Error)
		{
			return;
		}
		while (true)
		{
			if (notification.LocalizedText != null && notification.LocalizedText.ToString() == StringUtil.TR("RejoinGameNoLongerAvailable", "Global"))
			{
				while (true)
				{
					AppState_FrontendLoadingScreen.Get().Enter(null);
					return;
				}
			}
			return;
		}
	}

	public void OnQuickPlayClicked()
	{
		if (UIMatchStartPanel.Get().IsVisible())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					AppState_CharacterSelect.Get().Enter();
					return;
				}
			}
		}
		AppState_GroupCharacterSelect.Get().Enter();
	}

	public void OnTutorial1Clicked()
	{
		AppState_FrontendLoadingScreen.Get().Enter(null, AppState_FrontendLoadingScreen.NextState.GoToTutorial);
	}

	private void HandleGameSelecting()
	{
		AppState_CharacterSelect.Get().Enter();
	}

	private void HandleGameAssembling()
	{
		int num;
		if (!AppState.IsInGame())
		{
			if (GameManager.Get().GameInfo != null && GameManager.Get().GameInfo.IsCustomGame)
			{
				num = ((GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped) ? 1 : 0);
				goto IL_0064;
			}
		}
		num = 0;
		goto IL_0064;
		IL_0064:
		if (num == 0)
		{
			return;
		}
		while (true)
		{
			AppState_CharacterSelect.Get().Enter();
			return;
		}
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
	{
		Get().Enter(lastLobbyErrorMessage);
	}

	private void HandleShowDailyQuests(QuestOfferNotification quests)
	{
		if (QuestOfferPanel.Get() != null)
		{
			QuestOfferPanel.Get().ShowDailyQuests(quests);
		}
	}

	public void HandleCheckAccountStatusResponse(CheckAccountStatusResponse response)
	{
		if (!response.Success)
		{
			return;
		}
		while (true)
		{
			if (!(QuestOfferPanel.Get() != null))
			{
				return;
			}
			while (true)
			{
				FactionCompetition factionCompetition = FactionWideData.Get().GetFactionCompetition(FactionWideData.Get().GetCurrentFactionCompetition());
				AccountComponent.UIStateIdentifier uIStateIdentifier = AccountComponent.UIStateIdentifier.NONE;
				if (factionCompetition != null)
				{
					uIStateIdentifier = factionCompetition.UIToDisplayOnLogin;
				}
				if (uIStateIdentifier != AccountComponent.UIStateIdentifier.NONE)
				{
					bool flag = true;
					if (ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uIStateIdentifier) == 0)
					{
						flag = false;
					}
					if (flag)
					{
						return;
					}
					while (true)
					{
						if (uIStateIdentifier == AccountComponent.UIStateIdentifier.HasSeenFactionWarSeasonTwoChapterTwo)
						{
							UIFactionsIntroduction.Get().SetupIntro(response.QuestOffers);
							ClientGameManager.Get().RequestUpdateUIState(uIStateIdentifier, 1, null);
						}
						else
						{
							Log.Warning("Did not handle to display ui state {0} on log in", uIStateIdentifier);
						}
						return;
					}
				}
				if (!response.QuestOffers.OfferDailyQuest)
				{
					return;
				}
				while (true)
				{
					if (!(UIFactionsIntroduction.Get() == null))
					{
						if (UIFactionsIntroduction.Get().IsActive())
						{
							return;
						}
					}
					if (!response.QuestOffers.DailyQuestIds.IsNullOrEmpty())
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								QuestOfferPanel.Get().ShowDailyQuests(response.QuestOffers);
								return;
							}
						}
					}
					Log.Error("CheckForDailyQuestsResponse offered daily quests with no ID's");
					return;
				}
			}
		}
	}
}
