using System;
using LobbyGameClientMessages;

public class AppState_LandingPage : AppState
{
	private static AppState_LandingPage s_instance;

	private UIDialogBox m_messageBox;

	private string m_lastLobbyErrorMessage;

	private bool m_receivedLobbyinfo;

	private bool m_returningFromGroupCharacterSelect;

	private bool m_goToCharacterSelect;

	public static AppState_LandingPage Get()
	{
		return AppState_LandingPage.s_instance;
	}

	public bool ReceivedLobbyStatusInfo
	{
		get
		{
			return this.m_receivedLobbyinfo;
		}
	}

	public void Enter(bool returningFromGroupCharacterSelect)
	{
		this.m_returningFromGroupCharacterSelect = returningFromGroupCharacterSelect;
		base.Enter();
	}

	public void Enter(string lastLobbyErrorMessage, bool goToCharacterSelect = false)
	{
		this.m_lastLobbyErrorMessage = lastLobbyErrorMessage;
		this.m_goToCharacterSelect = goToCharacterSelect;
		base.Enter();
	}

	public static void Create()
	{
		AppState.Create<AppState_LandingPage>();
	}

	private void Awake()
	{
		AppState_LandingPage.s_instance = this;
	}

	protected override void OnEnter()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		UIFrontEnd.Get().m_landingPageScreen.m_inCustomGame = false;
		AudioManager.GetMixerSnapshotManager().SetMix_Menu();
		UIFrontEnd.Get().SetVisible(true);
		UIFrontEnd.Get().EnableFrontendEnvironment(true);
		UIFrontEnd.Get().ShowScreen(FrontEndScreenState.LandingPage, false);
		UIFrontEnd.Get().m_landingPageScreen.ShowMOTD();
		UIFrontEnd.Get().m_landingPageScreen.SetServerIsLocked(clientGameManager.IsServerLocked);
		UIFrontEnd.Get().m_frontEndNavPanel.SetShopVisible(GameManager.Get().GameplayOverrides.EnableShop);
		UIFrontEnd.Get().m_frontEndNavPanel.CheckSeasonsVisibility();
		UIManager.SetGameObjectActive(UISystemMenuPanel.Get(), true, null);
		clientGameManager.OnConnectedToLobbyServer += this.HandleConnectedToLobbyServer;
		clientGameManager.OnDisconnectedFromLobbyServer += this.HandleDisconnectedFromLobbyServer;
		clientGameManager.OnLobbyServerReadyNotification += this.HandleLobbyServerReadyNotification;
		clientGameManager.OnLobbyStatusNotification += this.HandleStatusNotification;
		clientGameManager.OnAccountDataUpdated += this.HandleAccountDataUpdated;
		clientGameManager.OnChatNotification += this.HandleChatNotification;
		GameManager.Get().OnGameAssembling += this.HandleGameAssembling;
		GameManager.Get().OnGameSelecting += this.HandleGameSelecting;
		GameManager.Get().OnGameLaunched += this.HandleGameLaunched;
		this.ConnectToLobbyServer();
		if (this.m_lastLobbyErrorMessage != null && this.m_messageBox == null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_LandingPage.OnEnter()).MethodHandle;
			}
			UINewUserFlowManager.HideDisplay();
			string lastLobbyErrorMessage = this.m_lastLobbyErrorMessage;
			this.m_lastLobbyErrorMessage = null;
			this.m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(string.Empty, lastLobbyErrorMessage, StringUtil.TR("Ok", "Global"), delegate(UIDialogBox UIDialogBox)
			{
				this.m_messageBox = null;
			}, -1, false);
		}
		if (clientGameManager != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (clientGameManager.IsConnectedToLobbyServer)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (UILandingPageScreen.Get() == null)
				{
					clientGameManager.SendCheckAccountStatusRequest(new Action<CheckAccountStatusResponse>(this.HandleCheckAccountStatusResponse));
					clientGameManager.SendCheckRAFStatusRequest(false, null);
				}
			}
		}
		if (UILoadingScreenPanel.Get() != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			UILoadingScreenPanel.Get().SetVisible(false);
		}
		if (clientGameManager.IsPlayerAccountDataAvailable())
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			this.HandleAccountDataUpdated(clientGameManager.GetPlayerAccountData());
		}
		UINewUserFlowManager.HighlightQueued();
		UINewUserFlowManager.OnNavBarDisplayed();
		if (HighlightUtils.Get() != null)
		{
			HighlightUtils.Get().HideCursorHighlights();
		}
		this.CheckForPreviousGame();
		if (clientGameManager.IsServerLocked)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_goToCharacterSelect = false;
		}
		if (!this.m_goToCharacterSelect)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!this.m_returningFromGroupCharacterSelect)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (clientGameManager != null && clientGameManager.GroupInfo != null)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (clientGameManager.GroupInfo.InAGroup)
					{
						goto IL_31F;
					}
				}
			}
			if (UIRankedModeSelectScreen.Get() != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				UIRankedModeSelectScreen.Get().SetVisible(false);
				goto IL_42D;
			}
			goto IL_42D;
		}
		IL_31F:
		this.m_goToCharacterSelect = false;
		UIFrontendLoadingScreen.Get().SetVisible(false);
		AppState_GroupCharacterSelect.Get().Enter();
		UIFrontEnd.Get().m_frontEndNavPanel.SetNavButtonSelected(UIFrontEnd.Get().m_frontEndNavPanel.m_PlayBtn);
		if (UIRankedModeSelectScreen.Get() != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (ClientGameManager.Get() != null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (ClientGameManager.Get().GroupInfo != null)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (ClientGameManager.Get().GroupInfo.SelectedQueueType == GameType.Ranked)
					{
						UIFrontEnd.Get().ShowScreen(FrontEndScreenState.RankedModeSelect, false);
						UIRankedModeSelectScreen.Get().SetVisible(true);
						goto IL_405;
					}
				}
			}
			UIFrontEnd.Get().ShowScreen(FrontEndScreenState.GroupCharacterSelect, false);
			UIRankedModeSelectScreen.Get().SetVisible(false);
		}
		IL_405:
		IL_42D:
		this.m_returningFromGroupCharacterSelect = false;
	}

	protected override void OnLeave()
	{
		if (this.m_messageBox != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_LandingPage.OnLeave()).MethodHandle;
			}
			this.m_messageBox.Close();
			this.m_messageBox = null;
		}
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnConnectedToLobbyServer -= this.HandleConnectedToLobbyServer;
		clientGameManager.OnDisconnectedFromLobbyServer -= this.HandleDisconnectedFromLobbyServer;
		clientGameManager.OnLobbyServerReadyNotification -= this.HandleLobbyServerReadyNotification;
		clientGameManager.OnLobbyStatusNotification -= this.HandleStatusNotification;
		clientGameManager.OnAccountDataUpdated -= this.HandleAccountDataUpdated;
		clientGameManager.OnChatNotification -= this.HandleChatNotification;
		GameManager.Get().OnGameAssembling -= this.HandleGameAssembling;
		GameManager.Get().OnGameSelecting -= this.HandleGameSelecting;
		GameManager.Get().OnGameLaunched -= this.HandleGameLaunched;
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_LandingPage.ConnectToLobbyServer()).MethodHandle;
			}
			if (this.m_messageBox)
			{
				this.m_messageBox.Close();
				this.m_messageBox = null;
			}
			if (!this.m_lastLobbyErrorMessage.IsNullOrEmpty())
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				string lastLobbyErrorMessage = this.m_lastLobbyErrorMessage;
				this.m_lastLobbyErrorMessage = null;
				UINewUserFlowManager.HideDisplay();
				if (clientGameManager.AllowRelogin)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					string empty = string.Empty;
					string description = string.Format(StringUtil.TR("PressOkToReconnect", "Global"), lastLobbyErrorMessage);
					string leftButtonLabel = StringUtil.TR("Ok", "Global");
					string rightButtonLabel = StringUtil.TR("Cancel", "Global");
					UIDialogBox.DialogButtonCallback leftButtonCallback = delegate(UIDialogBox UIDialogBox)
					{
						this.ConnectToLobbyServer();
					};
					if (AppState_LandingPage.<>f__am$cache0 == null)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						AppState_LandingPage.<>f__am$cache0 = delegate(UIDialogBox UIDialogBox)
						{
							AppState_Shutdown.Get().Enter();
						};
					}
					this.m_messageBox = UIDialogPopupManager.OpenTwoButtonDialog(empty, description, leftButtonLabel, rightButtonLabel, leftButtonCallback, AppState_LandingPage.<>f__am$cache0, false, false);
				}
				else
				{
					this.m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(string.Empty, string.Format(StringUtil.TR("PressOkToExit", "Global"), lastLobbyErrorMessage), StringUtil.TR("Ok", "Global"), delegate(UIDialogBox UIDialogBox)
					{
						AppState_Shutdown.Get().Enter();
					}, -1, false);
				}
			}
			else
			{
				try
				{
					this.m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(string.Empty, StringUtil.TR("ConnectingToLobbyServer", "Global"), StringUtil.TR("Cancel", "Global"), delegate(UIDialogBox UIDialogBox)
					{
						AppState_Shutdown.Get().Enter();
					}, -1, false);
					clientGameManager.ConnectToLobbyServer();
				}
				catch (Exception ex)
				{
					if (this.m_messageBox != null)
					{
						this.m_messageBox.Close();
						this.m_messageBox = null;
					}
					UINewUserFlowManager.HideDisplay();
					string empty2 = string.Empty;
					string description2 = string.Format(StringUtil.TR("FailedToConnectToLobbyServer", "Global"), ex.Message);
					string buttonLabelText = StringUtil.TR("Ok", "Global");
					if (AppState_LandingPage.<>f__am$cache3 == null)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						AppState_LandingPage.<>f__am$cache3 = delegate(UIDialogBox UIDialogBox)
						{
							AppState_Shutdown.Get().Enter();
						};
					}
					this.m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(empty2, description2, buttonLabelText, AppState_LandingPage.<>f__am$cache3, -1, false);
				}
			}
		}
		else
		{
			UIFrontEnd.Get().m_landingPageScreen.ShowMOTD();
			UIFrontEnd.Get().m_landingPageScreen.SetServerIsLocked(clientGameManager.IsServerLocked);
		}
	}

	private void Update()
	{
	}

	public void HandleConnectedToLobbyServer(RegisterGameClientResponse response)
	{
		if (this.m_messageBox)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_LandingPage.HandleConnectedToLobbyServer(RegisterGameClientResponse)).MethodHandle;
			}
			this.m_messageBox.Close();
			this.m_messageBox = null;
		}
		if (!response.Success)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (response.LocalizedFailure != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				response.ErrorMessage = response.LocalizedFailure.ToString();
			}
			if (response.ErrorMessage.IsNullOrEmpty())
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				response.ErrorMessage = StringUtil.TR("UnknownError", "Global");
			}
			UINewUserFlowManager.HideDisplay();
			string text;
			if (response.ErrorMessage == "INVALID_PROTOCOL_VERSION")
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				text = StringUtil.TR("NotRecentVersionOfTheGame", "Frontend");
			}
			else if (response.ErrorMessage == "INVALID_IP_ADDRESS")
			{
				text = StringUtil.TR("IPAddressChanged", "Frontend");
			}
			else if (response.ErrorMessage == "ACCOUNT_BANNED")
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				text = StringUtil.TR("AccountBanned", "Frontend");
			}
			else
			{
				text = string.Format(StringUtil.TR("FailedToConnectToLobbyServer", "Global"), response.ErrorMessage);
			}
			string empty = string.Empty;
			string description = text;
			string buttonLabelText = StringUtil.TR("Ok", "Global");
			if (AppState_LandingPage.<>f__am$cache4 == null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				AppState_LandingPage.<>f__am$cache4 = delegate(UIDialogBox UIDialogBox)
				{
					AppState_Shutdown.Get().Enter();
				};
			}
			this.m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(empty, description, buttonLabelText, AppState_LandingPage.<>f__am$cache4, -1, false);
		}
	}

	public void HandleLobbyServerReadyNotification(LobbyServerReadyNotification notification)
	{
		this.CheckForPreviousGame();
	}

	public void HandleAccountDataUpdated(PersistedAccountData accountData)
	{
		UIFrontEnd.Get().m_landingPageScreen.UpdateMatchData();
	}

	public void HandleStatusNotification(LobbyStatusNotification notification)
	{
		this.m_receivedLobbyinfo = true;
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_LandingPage.CheckForPreviousGame()).MethodHandle;
			}
			if (clientGameManager.IsRegistered)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (clientGameManager.IsReady)
				{
					Log.Info("Checking for previous game", new object[0]);
					clientGameManager.RequestPreviousGameInfo(delegate(PreviousGameInfoResponse response)
					{
						if (AppState.GetCurrent() != this)
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!true)
							{
								RuntimeMethodHandle runtimeMethodHandle2 = methodof(AppState_LandingPage.<CheckForPreviousGame>m__7(PreviousGameInfoResponse)).MethodHandle;
							}
							return;
						}
						if (response.PreviousGameInfo != null)
						{
							if (!response.PreviousGameInfo.IsQueuedGame)
							{
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!response.PreviousGameInfo.IsCustomGame)
								{
									return;
								}
							}
							if (response.PreviousGameInfo.GameConfig.TotalHumanPlayers >= 2)
							{
								this.PromptToRejoinGame(response.PreviousGameInfo);
								return;
							}
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
						}
					});
					return;
				}
			}
		}
		Log.Info("Not checking for previous game-- {0}/{1}", new object[]
		{
			clientGameManager.IsRegistered,
			clientGameManager.IsReady
		});
	}

	private void PromptToRejoinGame(LobbyGameInfo previousGameInfo)
	{
		UINewUserFlowManager.HideDisplay();
		this.m_messageBox = UIDialogPopupManager.OpenTwoButtonDialog(StringUtil.TR("Reconnect", "Global"), string.Format(StringUtil.TR("ReconnectUnderDevelopment", "Global"), previousGameInfo.GameConfig.GameType.GetDisplayName()), StringUtil.TR("Reconnect", "Global"), StringUtil.TR("Cancel", "Global"), delegate(UIDialogBox UIDialogBox)
		{
			Log.Info("Attempting to reconnect!", new object[0]);
			ClientGameManager.Get().RejoinGame(true, null);
			this.m_messageBox = null;
		}, delegate(UIDialogBox UIDialogBox)
		{
			Log.Info("Decided not to reconnect!", new object[0]);
			ClientGameManager.Get().RejoinGame(false, null);
			this.m_messageBox = null;
		}, false, false);
		if (ClientCrashReportDetector.Get().m_crashDialog != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_LandingPage.PromptToRejoinGame(LobbyGameInfo)).MethodHandle;
			}
			ClientCrashReportDetector.Get().m_crashDialog.gameObject.transform.SetAsLastSibling();
		}
	}

	private void HandleChatNotification(ChatNotification notification)
	{
		if (notification.ConsoleMessageType == ConsoleMessageType.Error)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_LandingPage.HandleChatNotification(ChatNotification)).MethodHandle;
			}
			if (notification.LocalizedText != null && notification.LocalizedText.ToString() == StringUtil.TR("RejoinGameNoLongerAvailable", "Global"))
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				AppState_FrontendLoadingScreen.Get().Enter(null, AppState_FrontendLoadingScreen.NextState.GoToLandingPage);
			}
		}
	}

	public void OnQuickPlayClicked()
	{
		if (UIMatchStartPanel.Get().IsVisible())
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_LandingPage.OnQuickPlayClicked()).MethodHandle;
			}
			AppState_CharacterSelect.Get().Enter();
		}
		else
		{
			AppState_GroupCharacterSelect.Get().Enter();
		}
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
		bool flag;
		if (!AppState.IsInGame())
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_LandingPage.HandleGameAssembling()).MethodHandle;
			}
			if (GameManager.Get().GameInfo != null && GameManager.Get().GameInfo.IsCustomGame)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				flag = (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped);
				goto IL_64;
			}
		}
		flag = false;
		IL_64:
		bool flag2 = flag;
		if (flag2)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			AppState_CharacterSelect.Get().Enter();
		}
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
	{
		AppState_LandingPage.Get().Enter(lastLobbyErrorMessage, false);
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
		if (response.Success)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_LandingPage.HandleCheckAccountStatusResponse(CheckAccountStatusResponse)).MethodHandle;
			}
			if (QuestOfferPanel.Get() != null)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				FactionCompetition factionCompetition = FactionWideData.Get().GetFactionCompetition(FactionWideData.Get().GetCurrentFactionCompetition());
				AccountComponent.UIStateIdentifier uistateIdentifier = AccountComponent.UIStateIdentifier.NONE;
				if (factionCompetition != null)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					uistateIdentifier = factionCompetition.UIToDisplayOnLogin;
				}
				if (uistateIdentifier != AccountComponent.UIStateIdentifier.NONE)
				{
					bool flag = true;
					if (ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uistateIdentifier) == 0)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						flag = false;
					}
					if (!flag)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (uistateIdentifier == AccountComponent.UIStateIdentifier.HasSeenFactionWarSeasonTwoChapterTwo)
						{
							UIFactionsIntroduction.Get().SetupIntro(response.QuestOffers);
							ClientGameManager.Get().RequestUpdateUIState(uistateIdentifier, 1, null);
						}
						else
						{
							Log.Warning("Did not handle to display ui state {0} on log in", new object[]
							{
								uistateIdentifier
							});
						}
					}
				}
				else if (response.QuestOffers.OfferDailyQuest)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!(UIFactionsIntroduction.Get() == null))
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (UIFactionsIntroduction.Get().IsActive())
						{
							return;
						}
					}
					if (!response.QuestOffers.DailyQuestIds.IsNullOrEmpty<int>())
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						QuestOfferPanel.Get().ShowDailyQuests(response.QuestOffers);
					}
					else
					{
						Log.Error("CheckForDailyQuestsResponse offered daily quests with no ID's", new object[0]);
					}
				}
			}
		}
	}
}
