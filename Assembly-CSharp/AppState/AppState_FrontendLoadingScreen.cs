using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using LobbyGameClientMessages;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppState_FrontendLoadingScreen : AppState
{
	private static AppState_FrontendLoadingScreen s_instance;

	private string m_lastLobbyErrorMessage;

	private bool m_isLoadingStarted;

	private bool m_isLoadingComplete;

	private bool m_isFirstTime;

	private bool m_isUILoaded;

	private bool m_hasGoneToLandingPage;

	private bool m_hasEnteredMatch;

	private AppState_FrontendLoadingScreen.NextState m_nextState;

	private AppState_FrontendLoadingScreen.State m_state;

	public static AppState_FrontendLoadingScreen Get()
	{
		return AppState_FrontendLoadingScreen.s_instance;
	}

	public void Enter(string lastLobbyErrorMessage, AppState_FrontendLoadingScreen.NextState nextState = AppState_FrontendLoadingScreen.NextState.GoToLandingPage)
	{
		this.m_lastLobbyErrorMessage = lastLobbyErrorMessage;
		this.m_nextState = nextState;
		base.Enter();
	}

	public static void Create()
	{
		AppState.Create<AppState_FrontendLoadingScreen>();
	}

	private void Awake()
	{
		AppState_FrontendLoadingScreen.s_instance = this;
		this.m_isLoadingStarted = false;
		this.m_isLoadingComplete = false;
		this.m_isFirstTime = true;
		this.m_isUILoaded = false;
		this.m_hasGoneToLandingPage = false;
		this.m_nextState = AppState_FrontendLoadingScreen.NextState.GoToLandingPage;
	}

	protected override void OnEnter()
	{
		this.m_state = AppState_FrontendLoadingScreen.State.Loading;
		this.m_isLoadingStarted = false;
		this.m_isLoadingComplete = false;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnConnectedToLobbyServer += this.HandleConnectedToLobbyServer;
		clientGameManager.OnDisconnectedFromLobbyServer += this.HandleDisconnectedFromLobbyServer;
		clientGameManager.OnLobbyServerReadyNotification += this.HandleLobbyServerReadyNotification;
		clientGameManager.OnLobbyStatusNotification += this.HandleStatusNotification;
		clientGameManager.OnAccountDataUpdated += this.HandleAccountDataUpdated;
		AudioManager.GetMixerSnapshotManager().SetMix_LoadingScreen();
		AudioManager.PostEvent("sw_music_selection", AudioManager.EventAction.SetSwitch, "menu", null);
		AudioManager.PostEvent("sw_ambiance_selection", AudioManager.EventAction.SetSwitch, "menu", null);
		UIFrontendLoadingScreen.Get().SetVisible(true);
		UIFrontendLoadingScreen.Get().StartDisplayLoading(null);
		if (UILoadingScreenPanel.Get() != null)
		{
			UILoadingScreenPanel.Get().SetVisible(false);
		}
		if (!this.m_isFirstTime)
		{
			SceneManager.LoadScene("Disconnected");
		}
		this.ConnectToLobbyServer();
		this.m_isFirstTime = false;
	}

	protected override void OnLeave()
	{
		AudioManager.StandardizeAudioLinkages();
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnConnectedToLobbyServer -= this.HandleConnectedToLobbyServer;
		clientGameManager.OnDisconnectedFromLobbyServer -= this.HandleDisconnectedFromLobbyServer;
		clientGameManager.OnLobbyServerReadyNotification -= this.HandleLobbyServerReadyNotification;
		clientGameManager.OnLobbyStatusNotification -= this.HandleStatusNotification;
		clientGameManager.OnAccountDataUpdated -= this.HandleAccountDataUpdated;
	}

	private void Update()
	{
		this.CheckEndWaitingForKey();
		this.CheckEndFadingOut();
	}

	private void StartLoading()
	{
		this.m_isLoadingStarted = true;
		if (UIFrontendLoadingScreen.Get() != null)
		{
			UIFrontendLoadingScreen.Get().StartDisplayLoading(null);
		}
		if (this.m_nextState != AppState_FrontendLoadingScreen.NextState.GoToCharacterSelect)
		{
			if (this.m_nextState == AppState_FrontendLoadingScreen.NextState.GoToLandingPage)
			{
			}
			else
			{
				if (!this.m_isUILoaded)
				{
					base.StartCoroutine(this.WaitForLoadUIBackground());
					return;
				}
				this.m_isLoadingComplete = true;
				this.ChooseNextAction();
				return;
			}
		}
		Log.Info(Log.Category.Loading, "PKFxManager.DeepReset going to frontend", new object[0]);
		PKFxManager.DeepReset();
		if (!this.m_isUILoaded)
		{
			base.StartCoroutine(this.LoadUIAndFrontEndBackground());
		}
		else
		{
			base.StartCoroutine(this.LoadFrontEndBackground());
		}
	}

	private void ChooseNextAction()
	{
		if (ClientGameManager.Get().IsServerLocked)
		{
			if (!this.m_hasGoneToLandingPage)
			{
				this.StartServerLocked();
				return;
			}
		}
		if (ClientGameManager.Get().IsServerQueued)
		{
			this.StartServerQueued();
		}
		else if (ClientGameManager.Get().IsReady)
		{
			if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
			{
				if (this.ShouldAutomaticallyEnterTutorial())
				{
					this.m_nextState = AppState_FrontendLoadingScreen.NextState.GoToTutorial;
				}
				if (this.ShouldAutomaticallyEnterGame())
				{
					this.m_nextState = AppState_FrontendLoadingScreen.NextState.GoToGame;
				}
				if (!this.m_isLoadingStarted)
				{
					if (this.m_nextState != AppState_FrontendLoadingScreen.NextState.GoToGame)
					{
						if (this.m_nextState != AppState_FrontendLoadingScreen.NextState.GoToTutorial)
						{
							goto IL_106;
						}
					}
					UIManager.Get().SetGameState(UIManager.ClientState.InGame);
					IL_106:
					this.StartLoading();
				}
				else if (this.m_isLoadingComplete)
				{
					if (this.m_nextState == AppState_FrontendLoadingScreen.NextState.GoToTutorial)
					{
						if (!this.m_hasGoneToLandingPage)
						{
							UINewUserFlowManager.MarkShowPlayHighlight(true);
							ClientGameManager.Get().GroupInfo.SelectedQueueType = GameType.Coop;
							this.StartWaitingForKey();
							return;
						}
					}
					this.StartFadingOut();
				}
			}
		}
	}

	private void StartWaitingForKey()
	{
		if (this.m_state != AppState_FrontendLoadingScreen.State.WaitingForKey)
		{
			this.m_state = AppState_FrontendLoadingScreen.State.WaitingForKey;
			UIFrontendLoadingScreen.Get().StartDisplayPressKey();
		}
	}

	private void StartServerLocked()
	{
		if (this.m_state != AppState_FrontendLoadingScreen.State.ServerLocked)
		{
			this.m_state = AppState_FrontendLoadingScreen.State.ServerLocked;
			UIFrontendLoadingScreen.Get().StartDisplayServerLocked();
			UIFrontendLoadingScreen.Get().SetServerLockButtonVisible(true);
		}
		UIFrontendLoadingScreen.Get().UpdateServerLockLabels(ClientGameManager.Get().ServerMessageOverrides);
	}

	private void StartServerQueued()
	{
		if (this.m_state != AppState_FrontendLoadingScreen.State.ServerQueued)
		{
			this.m_state = AppState_FrontendLoadingScreen.State.ServerQueued;
		}
		ConnectionQueueInfo connectionQueueInfo = ClientGameManager.Get().ConnectionQueueInfo;
		string arg;
		if (connectionQueueInfo.QueueEstimatedSeconds != 0)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)connectionQueueInfo.QueueEstimatedSeconds);
			if (timeSpan.Hours >= 1)
			{
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
		this.m_state = AppState_FrontendLoadingScreen.State.FadingOut;
		UIFrontendLoadingScreen.Get().StartDisplayFadeOut();
	}

	private void StartError(string mainErrorText, string subErrorText = null)
	{
		this.m_state = AppState_FrontendLoadingScreen.State.Error;
		UIFrontendLoadingScreen.Get().StartDisplayError(mainErrorText, subErrorText);
	}

	private void CheckEndWaitingForKey()
	{
		if (this.m_state == AppState_FrontendLoadingScreen.State.WaitingForKey)
		{
			if (UIFrontendLoadingScreen.Get().gameObject.activeSelf)
			{
				if (UIFrontendLoadingScreen.Get().DisplayState == UIFrontendLoadingScreen.DisplayStates.PressKey)
				{
					if (Input.anyKeyDown)
					{
						this.StartFadingOut();
					}
				}
			}
		}
	}

	private void CheckEndFadingOut()
	{
		if (this.m_state == AppState_FrontendLoadingScreen.State.FadingOut)
		{
			HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
			if (UIFrontendLoadingScreen.Get().IsReadyToReveal())
			{
				TutorialVersion tutorialVersion = TutorialVersion.None;
				if (this.m_nextState == AppState_FrontendLoadingScreen.NextState.GoToLandingPage)
				{
					AppState_LandingPage.Get().Enter(this.m_lastLobbyErrorMessage, false);
					this.m_hasGoneToLandingPage = true;
					this.m_lastLobbyErrorMessage = null;
				}
				else if (this.m_nextState == AppState_FrontendLoadingScreen.NextState.GoToCharacterSelect)
				{
					AppState_LandingPage.Get().Enter(this.m_lastLobbyErrorMessage, true);
					this.m_hasGoneToLandingPage = true;
					this.m_lastLobbyErrorMessage = null;
				}
				else if (this.m_nextState == AppState_FrontendLoadingScreen.NextState.GoToTutorial && !ClientGameManager.Get().IsConnectedToGameServer && !ClientGameManager.Get().IsRegisteredToGameServer)
				{
					this.m_hasEnteredMatch = true;
					UIFrontendLoadingScreen.Get().StartDisplayLoading(null);
					UIManager.Get().SetGameState(UIManager.ClientState.InGame);
					AppState_GameTypeSelect.Get().Enter(GameType.Tutorial, null);
					tutorialVersion = TutorialVersion.CargoShip_Tutorial1;
				}
				else if (this.m_nextState == AppState_FrontendLoadingScreen.NextState.GoToGame)
				{
					if (!ClientGameManager.Get().IsConnectedToGameServer)
					{
						if (!ClientGameManager.Get().IsRegisteredToGameServer)
						{
							this.m_hasEnteredMatch = true;
							UIFrontendLoadingScreen.Get().StartDisplayLoading(null);
							UIManager.Get().SetGameState(UIManager.ClientState.InGame);
							if (hydrogenConfig.AutoLaunchGameType == GameType.Custom)
							{
								if (hydrogenConfig.AutoLaunchCustomGameConfig.GameConfig != null)
								{
									if (hydrogenConfig.AutoLaunchCustomGameConfig.TeamInfo != null)
									{
										AppState_GameTypeSelect.Get().Enter(hydrogenConfig.AutoLaunchCustomGameConfig);
										goto IL_1E4;
									}
								}
							}
							AppState_GameTypeSelect.Get().Enter(hydrogenConfig.AutoLaunchGameType, null);
						}
					}
				}
				IL_1E4:
				if (ClientGameManager.Get().GetPlayerAccountData().ExperienceComponent.TutorialProgress < tutorialVersion)
				{
					ClientGameManager.Get().GetPlayerAccountData().ExperienceComponent.TutorialProgress = tutorialVersion;
				}
			}
		}
	}

	private IEnumerator WaitForLoadUIBackground()
	{
		this.m_state = AppState_FrontendLoadingScreen.State.Loading;
		this.m_isLoadingComplete = false;
		Application.backgroundLoadingPriority = ThreadPriority.Low;
		while (!UIManager.Get().DoneInitialLoading)
		{
			yield return 1;
		}
		if (ClientQualityComponentEnabler.OptimizeForMemory())
		{
			Resources.UnloadUnusedAssets();
		}
		this.m_isLoadingComplete = true;
		this.m_isUILoaded = true;
		this.m_state = AppState_FrontendLoadingScreen.State.Loaded;
		this.ChooseNextAction();
		yield break;
	}

	private IEnumerator LoadUIAndFrontEndBackground()
	{
		UIManager.Get().SetGameState(UIManager.ClientState.InFrontEnd);
		this.m_state = AppState_FrontendLoadingScreen.State.Loading;
		this.m_isLoadingComplete = false;
		Application.backgroundLoadingPriority = ThreadPriority.Low;
		while (!UIManager.Get().DoneInitialLoading)
		{
			yield return 1;
		}
		for (;;)
		{
			if (!(UIFrontEnd.Get() == null))
			{
				if (!(UIStorePanel.Get() == null))
				{
					break;
				}
			}
			yield return null;
		}
		AppState_GroupCharacterSelect.ShowScreen();
		yield return 0;
		this.m_isLoadingComplete = true;
		this.m_isUILoaded = true;
		this.m_state = AppState_FrontendLoadingScreen.State.Loaded;
		this.ChooseNextAction();
		yield break;
	}

	private IEnumerator LoadFrontEndBackground()
	{
		this.m_state = AppState_FrontendLoadingScreen.State.Loading;
		this.m_isLoadingComplete = false;
		yield return null;
		ClientGameManager.Get().UnloadAssets();
		Application.backgroundLoadingPriority = ThreadPriority.Low;
		if (ClientQualityComponentEnabler.OptimizeForMemory())
		{
			IntPtr contiguousBlock = IntPtr.Zero;
			try
			{
				contiguousBlock = Marshal.AllocHGlobal(0x4000000);
			}
			catch
			{
				Marshal.FreeHGlobal(contiguousBlock);
				contiguousBlock = IntPtr.Zero;
			}
			if (contiguousBlock == IntPtr.Zero)
			{
				Log.Error("Memory is fragmented, or too low, restarting application.", new object[0]);
				string path = Application.dataPath;
				if (Application.platform == RuntimePlatform.OSXPlayer)
				{
					path += "/../../AtlasReactor";
				}
				else if (Application.platform == RuntimePlatform.WindowsPlayer)
				{
					path += "/../AtlasReactor";
				}
				Process.Start(path, string.Join(" ", Environment.GetCommandLineArgs()));
				yield return new WaitForSeconds(32f);
				Application.Quit();
				yield return new WaitForSeconds(999f);
			}
			else
			{
				Marshal.FreeHGlobal(contiguousBlock);
			}
		}
		Log.Info("Load InFrontEnd UI state", new object[0]);
		UIManager.Get().SetGameState(UIManager.ClientState.InFrontEnd);
		while (!UIManager.Get().DoneInitialLoading)
		{
			yield return 1;
		}
		for (;;)
		{
			if (!(UIFrontEnd.Get() == null))
			{
				if (!(UIStorePanel.Get() == null))
				{
					break;
				}
			}
			yield return null;
		}
		AppState_GroupCharacterSelect.ShowScreen();
		yield return 0;
		this.m_isLoadingComplete = true;
		this.m_isUILoaded = true;
		this.m_state = AppState_FrontendLoadingScreen.State.Loaded;
		this.ChooseNextAction();
		yield break;
	}

	public void ConnectToLobbyServer()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (!clientGameManager.IsConnectedToLobbyServer)
		{
			try
			{
				clientGameManager.ConnectToLobbyServer();
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
				this.StartError(StringUtil.TR("FailedToConnectToLobbyServer", "Frontend"), ex.Message);
			}
		}
		else
		{
			this.ChooseNextAction();
		}
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
				response.ErrorMessage = StringUtil.TR("UnknownError", "Global");
			}
			string subErrorText;
			if (response.ErrorMessage == "INVALID_PROTOCOL_VERSION")
			{
				subErrorText = StringUtil.TR("NotRecentVersionOfTheGame", "Frontend");
			}
			else if (response.ErrorMessage == "INVALID_IP_ADDRESS")
			{
				subErrorText = StringUtil.TR("IPAddressChanged", "Frontend");
			}
			else if (response.ErrorMessage == "ACCOUNT_BANNED")
			{
				subErrorText = StringUtil.TR("AccountBanned", "Frontend");
			}
			else
			{
				subErrorText = string.Format(StringUtil.TR("FailedToConnectToLobbyServerError", "Frontend"), response.ErrorMessage);
			}
			this.StartError(StringUtil.TR("FailedToConnectToLobbyServer", "Frontend"), subErrorText);
			return;
		}
		this.ChooseNextAction();
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
	{
		if (this.m_state != AppState_FrontendLoadingScreen.State.Error)
		{
			this.StartError(lastLobbyErrorMessage, null);
		}
	}

	public void HandleLobbyServerReadyNotification(LobbyServerReadyNotification notification)
	{
		if (!notification.Success)
		{
			if (notification.ErrorMessage.IsNullOrEmpty())
			{
				notification.ErrorMessage = StringUtil.TR("UnknownError", "Global");
			}
			this.StartError(StringUtil.TR("FailedToConnectToLobbyServer", "Frontend"), notification.ErrorMessage);
			return;
		}
		this.ChooseNextAction();
	}

	public void HandleAccountDataUpdated(PersistedAccountData accountData)
	{
		this.ChooseNextAction();
	}

	public void HandleStatusNotification(LobbyStatusNotification notification)
	{
		this.ChooseNextAction();
	}

	public bool ShouldAutomaticallyEnterTutorial()
	{
		if (HydrogenConfig.Get().AutoLaunchTutorial)
		{
			if (this.m_nextState == AppState_FrontendLoadingScreen.NextState.GoToLandingPage)
			{
				if (!this.m_hasEnteredMatch)
				{
					if (ClientGameManager.Get().IsConnectedToLobbyServer)
					{
						if (ClientGameManager.Get().IsReady && !ClientGameManager.Get().IsServerLocked)
						{
							if (!ClientGameManager.Get().IsServerQueued)
							{
								if (ClientGameManager.Get().IsPlayerAccountDataAvailable() && ClientGameManager.Get().GetPlayerAccountData().ExperienceComponent != null)
								{
									if (ClientGameManager.Get().GetPlayerAccountData().ExperienceComponent.TutorialProgress < TutorialVersion.CargoShip_Tutorial1)
									{
										if (ClientGameManager.Get().GetPlayerAccountData().ExperienceComponent.Matches < 3)
										{
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
		return false;
	}

	public bool ShouldAutomaticallyEnterGame()
	{
		return false;
	}

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
}
