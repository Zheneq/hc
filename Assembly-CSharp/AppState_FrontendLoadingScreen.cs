using LobbyGameClientMessages;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppState_FrontendLoadingScreen : AppState
{
	public enum NextState
	{
		GoToLandingPage,
		GoToCharacterSelect,
		GoToTutorial,
		GoToGame
	}

	private enum State
	{
		Loading,
		Loaded,
		ServerLocked,
		ServerQueued,
		WaitingForKey,
		Error,
		FadingOut
	}

	private static AppState_FrontendLoadingScreen s_instance;

	private string m_lastLobbyErrorMessage;

	private bool m_isLoadingStarted;

	private bool m_isLoadingComplete;

	private bool m_isFirstTime;

	private bool m_isUILoaded;

	private bool m_hasGoneToLandingPage;

	private bool m_hasEnteredMatch;

	private NextState m_nextState;

	private State m_state;

	public static AppState_FrontendLoadingScreen Get()
	{
		return s_instance;
	}

	public void Enter(string lastLobbyErrorMessage, NextState nextState = NextState.GoToLandingPage)
	{
		m_lastLobbyErrorMessage = lastLobbyErrorMessage;
		m_nextState = nextState;
		base.Enter();
	}

	public static void Create()
	{
		AppState.Create<AppState_FrontendLoadingScreen>();
	}

	private void Awake()
	{
		s_instance = this;
		m_isLoadingStarted = false;
		m_isLoadingComplete = false;
		m_isFirstTime = true;
		m_isUILoaded = false;
		m_hasGoneToLandingPage = false;
		m_nextState = NextState.GoToLandingPage;
	}

	protected override void OnEnter()
	{
		m_state = State.Loading;
		m_isLoadingStarted = false;
		m_isLoadingComplete = false;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnConnectedToLobbyServer += HandleConnectedToLobbyServer;
		clientGameManager.OnDisconnectedFromLobbyServer += HandleDisconnectedFromLobbyServer;
		clientGameManager.OnLobbyServerReadyNotification += HandleLobbyServerReadyNotification;
		clientGameManager.OnLobbyStatusNotification += HandleStatusNotification;
		clientGameManager.OnAccountDataUpdated += HandleAccountDataUpdated;
		AudioManager.GetMixerSnapshotManager().SetMix_LoadingScreen();
		AudioManager.PostEvent("sw_music_selection", AudioManager.EventAction.SetSwitch, "menu");
		AudioManager.PostEvent("sw_ambiance_selection", AudioManager.EventAction.SetSwitch, "menu");
		UIFrontendLoadingScreen.Get().SetVisible(true);
		UIFrontendLoadingScreen.Get().StartDisplayLoading();
		if (UILoadingScreenPanel.Get() != null)
		{
			UILoadingScreenPanel.Get().SetVisible(false);
		}
		if (!m_isFirstTime)
		{
			SceneManager.LoadScene("Disconnected");
		}
		ConnectToLobbyServer();
		m_isFirstTime = false;
	}

	protected override void OnLeave()
	{
		AudioManager.StandardizeAudioLinkages();
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnConnectedToLobbyServer -= HandleConnectedToLobbyServer;
		clientGameManager.OnDisconnectedFromLobbyServer -= HandleDisconnectedFromLobbyServer;
		clientGameManager.OnLobbyServerReadyNotification -= HandleLobbyServerReadyNotification;
		clientGameManager.OnLobbyStatusNotification -= HandleStatusNotification;
		clientGameManager.OnAccountDataUpdated -= HandleAccountDataUpdated;
	}

	private void Update()
	{
		CheckEndWaitingForKey();
		CheckEndFadingOut();
	}

	private void StartLoading()
	{
		m_isLoadingStarted = true;
		if (UIFrontendLoadingScreen.Get() != null)
		{
			UIFrontendLoadingScreen.Get().StartDisplayLoading();
		}
		if (m_nextState != NextState.GoToCharacterSelect)
		{
			if (m_nextState != 0)
			{
				if (!m_isUILoaded)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							StartCoroutine(WaitForLoadUIBackground());
							return;
						}
					}
				}
				m_isLoadingComplete = true;
				ChooseNextAction();
				return;
			}
			while (true)
			{
				switch (4)
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
		}
		Log.Info(Log.Category.Loading, "PKFxManager.DeepReset going to frontend");
		PKFxManager.DeepReset();
		if (!m_isUILoaded)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					StartCoroutine(LoadUIAndFrontEndBackground());
					return;
				}
			}
		}
		StartCoroutine(LoadFrontEndBackground());
	}

	private void ChooseNextAction()
	{
		if (ClientGameManager.Get().IsServerLocked)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!m_hasGoneToLandingPage)
			{
				StartServerLocked();
				return;
			}
		}
		if (ClientGameManager.Get().IsServerQueued)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					StartServerQueued();
					return;
				}
			}
		}
		if (!ClientGameManager.Get().IsReady)
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
			if (!ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
				if (ShouldAutomaticallyEnterTutorial())
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
					m_nextState = NextState.GoToTutorial;
				}
				if (ShouldAutomaticallyEnterGame())
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
					m_nextState = NextState.GoToGame;
				}
				if (!m_isLoadingStarted)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							{
								if (m_nextState != NextState.GoToGame)
								{
									if (m_nextState != NextState.GoToTutorial)
									{
										goto IL_0106;
									}
									while (true)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
								}
								UIManager.Get().SetGameState(UIManager.ClientState.InGame);
								goto IL_0106;
							}
							IL_0106:
							StartLoading();
							return;
						}
					}
				}
				if (!m_isLoadingComplete)
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
					if (m_nextState == NextState.GoToTutorial)
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
						if (!m_hasGoneToLandingPage)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									UINewUserFlowManager.MarkShowPlayHighlight(true);
									ClientGameManager.Get().GroupInfo.SelectedQueueType = GameType.Coop;
									StartWaitingForKey();
									return;
								}
							}
						}
					}
					StartFadingOut();
					return;
				}
			}
		}
	}

	private void StartWaitingForKey()
	{
		if (m_state == State.WaitingForKey)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_state = State.WaitingForKey;
			UIFrontendLoadingScreen.Get().StartDisplayPressKey();
			return;
		}
	}

	private void StartServerLocked()
	{
		if (m_state != State.ServerLocked)
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
			m_state = State.ServerLocked;
			UIFrontendLoadingScreen.Get().StartDisplayServerLocked();
			UIFrontendLoadingScreen.Get().SetServerLockButtonVisible(true);
		}
		UIFrontendLoadingScreen.Get().UpdateServerLockLabels(ClientGameManager.Get().ServerMessageOverrides);
	}

	private void StartServerQueued()
	{
		if (m_state != State.ServerQueued)
		{
			m_state = State.ServerQueued;
		}
		ConnectionQueueInfo connectionQueueInfo = ClientGameManager.Get().ConnectionQueueInfo;
		string arg;
		if (connectionQueueInfo.QueueEstimatedSeconds != 0)
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
			TimeSpan timeSpan = TimeSpan.FromSeconds(connectionQueueInfo.QueueEstimatedSeconds);
			if (timeSpan.Hours >= 1)
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
				arg = string.Format(StringUtil.TR("ETAhoursminutes", "Frontend"), timeSpan.Hours, timeSpan.Minutes);
			}
			else
			{
				arg = string.Format(StringUtil.TR("ETAminutes", "Frontend"), timeSpan.Minutes);
			}
		}
		else
		{
			arg = StringUtil.TR("ESTIMATINGTIME", "Frontend");
		}
		string queueStatusString = string.Format(StringUtil.TR("QUEUEPOSITION", "Frontend"), connectionQueueInfo.QueuePosition * connectionQueueInfo.QueueMultiplier, connectionQueueInfo.QueueSize * connectionQueueInfo.QueueMultiplier, arg);
		UIFrontendLoadingScreen.Get().StartDisplayServerQueued(queueStatusString);
	}

	private void StartFadingOut()
	{
		m_state = State.FadingOut;
		UIFrontendLoadingScreen.Get().StartDisplayFadeOut();
	}

	private void StartError(string mainErrorText, string subErrorText = null)
	{
		m_state = State.Error;
		UIFrontendLoadingScreen.Get().StartDisplayError(mainErrorText, subErrorText);
	}

	private void CheckEndWaitingForKey()
	{
		if (m_state != State.WaitingForKey)
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
			if (!UIFrontendLoadingScreen.Get().gameObject.activeSelf)
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
				if (UIFrontendLoadingScreen.Get().DisplayState != UIFrontendLoadingScreen.DisplayStates.PressKey)
				{
					return;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					if (Input.anyKeyDown)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							StartFadingOut();
							return;
						}
					}
					return;
				}
			}
		}
	}

	private void CheckEndFadingOut()
	{
		if (m_state != State.FadingOut)
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
			HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
			if (!UIFrontendLoadingScreen.Get().IsReadyToReveal())
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				TutorialVersion tutorialVersion = TutorialVersion.None;
				if (m_nextState == NextState.GoToLandingPage)
				{
					AppState_LandingPage.Get().Enter(m_lastLobbyErrorMessage);
					m_hasGoneToLandingPage = true;
					m_lastLobbyErrorMessage = null;
				}
				else if (m_nextState == NextState.GoToCharacterSelect)
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
					AppState_LandingPage.Get().Enter(m_lastLobbyErrorMessage, true);
					m_hasGoneToLandingPage = true;
					m_lastLobbyErrorMessage = null;
				}
				else if (m_nextState == NextState.GoToTutorial && !ClientGameManager.Get().IsConnectedToGameServer && !ClientGameManager.Get().IsRegisteredToGameServer)
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
					m_hasEnteredMatch = true;
					UIFrontendLoadingScreen.Get().StartDisplayLoading();
					UIManager.Get().SetGameState(UIManager.ClientState.InGame);
					AppState_GameTypeSelect.Get().Enter(GameType.Tutorial);
					tutorialVersion = TutorialVersion.CargoShip_Tutorial1;
				}
				else if (m_nextState == NextState.GoToGame)
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
					if (!ClientGameManager.Get().IsConnectedToGameServer)
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
						if (!ClientGameManager.Get().IsRegisteredToGameServer)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							m_hasEnteredMatch = true;
							UIFrontendLoadingScreen.Get().StartDisplayLoading();
							UIManager.Get().SetGameState(UIManager.ClientState.InGame);
							if (hydrogenConfig.AutoLaunchGameType == GameType.Custom)
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
								if (hydrogenConfig.AutoLaunchCustomGameConfig.GameConfig != null)
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
									if (hydrogenConfig.AutoLaunchCustomGameConfig.TeamInfo != null)
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
										AppState_GameTypeSelect.Get().Enter(hydrogenConfig.AutoLaunchCustomGameConfig);
										goto IL_01e4;
									}
								}
							}
							AppState_GameTypeSelect.Get().Enter(hydrogenConfig.AutoLaunchGameType);
						}
					}
				}
				goto IL_01e4;
				IL_01e4:
				if (ClientGameManager.Get().GetPlayerAccountData().ExperienceComponent.TutorialProgress < tutorialVersion)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						ClientGameManager.Get().GetPlayerAccountData().ExperienceComponent.TutorialProgress = tutorialVersion;
						return;
					}
				}
				return;
			}
		}
	}

	private IEnumerator WaitForLoadUIBackground()
	{
		m_state = State.Loading;
		m_isLoadingComplete = false;
		Application.backgroundLoadingPriority = ThreadPriority.Low;
		if (!UIManager.Get().DoneInitialLoading)
		{
			yield return 1;
			/*Error: Unable to find new state assignment for yield return*/;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (ClientQualityComponentEnabler.OptimizeForMemory())
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
				Resources.UnloadUnusedAssets();
			}
			m_isLoadingComplete = true;
			m_isUILoaded = true;
			m_state = State.Loaded;
			ChooseNextAction();
			yield break;
		}
	}

	private IEnumerator LoadUIAndFrontEndBackground()
	{
		UIManager.Get().SetGameState(UIManager.ClientState.InFrontEnd);
		m_state = State.Loading;
		m_isLoadingComplete = false;
		Application.backgroundLoadingPriority = ThreadPriority.Low;
		while (!UIManager.Get().DoneInitialLoading)
		{
			yield return 1;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(UIFrontEnd.Get() == null))
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
				if (!(UIStorePanel.Get() == null))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							AppState_GroupCharacterSelect.ShowScreen();
							yield return 0;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
			yield return null;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}

	private IEnumerator LoadFrontEndBackground()
	{
		m_state = State.Loading;
		m_isLoadingComplete = false;
		yield return null;
		/*Error: Unable to find new state assignment for yield return*/;
	}

	public void ConnectToLobbyServer()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (!clientGameManager.IsConnectedToLobbyServer)
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
					try
					{
						clientGameManager.ConnectToLobbyServer();
					}
					catch (Exception ex)
					{
						Log.Exception(ex);
						StartError(StringUtil.TR("FailedToConnectToLobbyServer", "Frontend"), ex.Message);
					}
					return;
				}
			}
		}
		ChooseNextAction();
	}

	public void HandleConnectedToLobbyServer(RegisterGameClientResponse response)
	{
		if (!response.Success)
		{
			if (response.LocalizedFailure != null)
			{
				response.ErrorMessage = response.LocalizedFailure.ToString();
			}
			if (response.ErrorMessage.IsNullOrEmpty())
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
				response.ErrorMessage = StringUtil.TR("UnknownError", "Global");
			}
			StartError(subErrorText: (response.ErrorMessage == "INVALID_PROTOCOL_VERSION") ? StringUtil.TR("NotRecentVersionOfTheGame", "Frontend") : ((response.ErrorMessage == "INVALID_IP_ADDRESS") ? StringUtil.TR("IPAddressChanged", "Frontend") : ((!(response.ErrorMessage == "ACCOUNT_BANNED")) ? string.Format(StringUtil.TR("FailedToConnectToLobbyServerError", "Frontend"), response.ErrorMessage) : StringUtil.TR("AccountBanned", "Frontend"))), mainErrorText: StringUtil.TR("FailedToConnectToLobbyServer", "Frontend"));
		}
		else
		{
			ChooseNextAction();
		}
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
	{
		if (m_state == State.Error)
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
			StartError(lastLobbyErrorMessage);
			return;
		}
	}

	public void HandleLobbyServerReadyNotification(LobbyServerReadyNotification notification)
	{
		if (!notification.Success)
		{
			if (notification.ErrorMessage.IsNullOrEmpty())
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				notification.ErrorMessage = StringUtil.TR("UnknownError", "Global");
			}
			StartError(StringUtil.TR("FailedToConnectToLobbyServer", "Frontend"), notification.ErrorMessage);
		}
		else
		{
			ChooseNextAction();
		}
	}

	public void HandleAccountDataUpdated(PersistedAccountData accountData)
	{
		ChooseNextAction();
	}

	public void HandleStatusNotification(LobbyStatusNotification notification)
	{
		ChooseNextAction();
	}

	public bool ShouldAutomaticallyEnterTutorial()
	{
		if (HydrogenConfig.Get().AutoLaunchTutorial)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_nextState == NextState.GoToLandingPage)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!m_hasEnteredMatch)
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
					if (ClientGameManager.Get().IsConnectedToLobbyServer)
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
						if (ClientGameManager.Get().IsReady && !ClientGameManager.Get().IsServerLocked)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!ClientGameManager.Get().IsServerQueued)
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
								if (ClientGameManager.Get().IsPlayerAccountDataAvailable() && ClientGameManager.Get().GetPlayerAccountData().ExperienceComponent != null)
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
									if (ClientGameManager.Get().GetPlayerAccountData().ExperienceComponent.TutorialProgress < TutorialVersion.CargoShip_Tutorial1)
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
										if (ClientGameManager.Get().GetPlayerAccountData().ExperienceComponent.Matches < 3)
										{
											while (true)
											{
												switch (2)
												{
												case 0:
													break;
												default:
													return true;
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return false;
	}

	public bool ShouldAutomaticallyEnterGame()
	{
		return false;
	}
}
