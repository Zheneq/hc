using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsState : ICloneable
{
	public enum WindowMode
	{
		Windowed,
		Fullscreen,
		WindowedFullscreen,
		Inherit
	}

	public enum LockCursorMode
	{
		Off,
		On,
		Smart
	}

	public enum VoiceChatMode
	{
		None,
		Group,
		Team
	}

	private const GraphicsQuality GRAPHICS_QUALITY_DEFAULT = GraphicsQuality.Medium;

	public GraphicsQuality graphicsQuality;

	public WindowMode windowMode;

	public int resolutionWidth;

	public int resolutionHeight;

	public WindowMode gameWindowMode;

	public int gameResolutionWidth;

	public int gameResolutionHeight;

	public bool lockWindowSize;

	public int masterVolume;

	public int musicVolume;

	public int ambianceVolume;

	public LockCursorMode lockCursorMode;

	public bool enableChatter;

	public bool rightClickingConfirmsAbilityTargets;

	public bool shiftClickForMovementWaypoints;

	public bool showGlobalChat;

	public bool showAllChat;

	public bool enableProfanityFilter;

	public bool autoJoinDiscord;

	public bool voicePushToTalk;

	public bool voiceMute;

	public float voiceVolume;

	public float micVolume;

	public VoiceChatMode gameModeVoiceChat;

	public bool hideTutorialVideos;

	public Region region;

	public bool allowCancelActionWhileConfirmed;

	public bool overrideGlyphLanguage;

	public string overrideGlyphLanguageCode;

	private bool m_isResolutionInitialized;

	private const int OPTIONS_VERSION = 14;

	public object Clone()
	{
		return MemberwiseClone();
	}

	public void InitToDefaults()
	{
		graphicsQuality = GraphicsQuality.Medium;
		windowMode = WindowMode.Fullscreen;
		gameWindowMode = WindowMode.Inherit;
		resolutionWidth = Screen.currentResolution.width;
		resolutionHeight = Screen.currentResolution.height;
		gameResolutionWidth = -1;
		gameResolutionHeight = -1;
		if (Application.isEditor)
		{
			resolutionWidth = 1600;
			resolutionHeight = 900;
		}
		lockWindowSize = false;
		masterVolume = 100;
		musicVolume = 100;
		ambianceVolume = 100;
		lockCursorMode = LockCursorMode.Smart;
		enableChatter = true;
		rightClickingConfirmsAbilityTargets = false;
		shiftClickForMovementWaypoints = true;
		showGlobalChat = true;
		showAllChat = false;
		enableProfanityFilter = true;
		autoJoinDiscord = false;
		voicePushToTalk = false;
		voiceMute = false;
		voiceVolume = 100f;
		micVolume = 100f;
		region = Options_UI.GetDefaultRegion();
		gameModeVoiceChat = VoiceChatMode.Group;
		hideTutorialVideos = false;
		allowCancelActionWhileConfirmed = true;
		overrideGlyphLanguage = false;
		overrideGlyphLanguageCode = string.Empty;
	}

	private void VersionPreferences(int version)
	{
		if (version >= 14)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (version < 1)
		{
			musicVolume = 100;
			ambianceVolume = 100;
			windowMode = WindowMode.Fullscreen;
		}
		if (version < 2)
		{
		}
		if (version < 3)
		{
			windowMode = WindowMode.WindowedFullscreen;
			resolutionWidth = Screen.currentResolution.width;
			resolutionHeight = Screen.currentResolution.height;
			graphicsQuality = GraphicsQuality.Medium;
		}
		if (version < 4)
		{
			lockCursorMode = LockCursorMode.Smart;
		}
		if (version < 5 && windowMode == WindowMode.WindowedFullscreen)
		{
			windowMode = WindowMode.Fullscreen;
			resolutionWidth = Screen.currentResolution.width;
			resolutionHeight = Screen.currentResolution.height;
		}
		if (version < 6)
		{
			showAllChat = false;
		}
		if (version < 7)
		{
			shiftClickForMovementWaypoints = false;
		}
		if (version < 8)
		{
			autoJoinDiscord = false;
		}
		if (version < 9)
		{
			gameModeVoiceChat = VoiceChatMode.Group;
		}
		if (version < 10)
		{
			hideTutorialVideos = false;
		}
		if (version < 11)
		{
			region = Options_UI.GetDefaultRegion();
		}
		if (version < 12)
		{
			allowCancelActionWhileConfirmed = false;
		}
		if (version < 13)
		{
			overrideGlyphLanguage = false;
			overrideGlyphLanguageCode = string.Empty;
		}
		if (version < 14)
		{
			voicePushToTalk = false;
			voiceMute = false;
			voiceVolume = 100f;
			micVolume = 100f;
		}
		Log.Info(Log.Category.UI, $"Versioned Options from {version} to {14}");
		PlayerPrefs.SetInt("OptionsVersion", 14);
		ApplyToPlayerPrefs();
	}

	public void InitFromPlayerPrefs()
	{
		InitToDefaults();
		switch (PlayerPrefs.GetInt("OptionsGraphicsQuality", (int)graphicsQuality))
		{
		default:
			graphicsQuality = GraphicsQuality.High;
			break;
		case -10:
			graphicsQuality = GraphicsQuality.VeryLow;
			break;
		case 0:
			graphicsQuality = GraphicsQuality.Low;
			break;
		case 1:
			graphicsQuality = GraphicsQuality.Medium;
			break;
		case 2:
			graphicsQuality = GraphicsQuality.High;
			break;
		}
		switch (PlayerPrefs.GetInt("OptionsWindowMode", (int)windowMode))
		{
		case 0:
			windowMode = WindowMode.Windowed;
			break;
		case 1:
			windowMode = WindowMode.Fullscreen;
			break;
		case 2:
			windowMode = WindowMode.Fullscreen;
			break;
		default:
			windowMode = WindowMode.Windowed;
			break;
		}
		resolutionWidth = PlayerPrefs.GetInt("OptionsResolutionWidth", resolutionWidth);
		resolutionHeight = PlayerPrefs.GetInt("OptionsResolutionHeight", resolutionHeight);
		switch (PlayerPrefs.GetInt("OptionsGameWindowMode", (int)gameWindowMode))
		{
		case 0:
			gameWindowMode = WindowMode.Windowed;
			break;
		case 1:
			gameWindowMode = WindowMode.Fullscreen;
			break;
		case 2:
			gameWindowMode = WindowMode.Fullscreen;
			break;
		case 3:
			gameWindowMode = WindowMode.Inherit;
			break;
		default:
			gameWindowMode = WindowMode.Windowed;
			break;
		}
		gameResolutionWidth = PlayerPrefs.GetInt("OptionsGameResolutionWidth", gameResolutionWidth);
		gameResolutionHeight = PlayerPrefs.GetInt("OptionsGameResolutionHeight", gameResolutionHeight);
		if (gameWindowMode == WindowMode.Inherit)
		{
			gameResolutionWidth = -1;
			gameResolutionHeight = -1;
		}
		lockWindowSize = false;
		masterVolume = PlayerPrefs.GetInt("OptionsMasterVolume", masterVolume);
		masterVolume = Mathf.Clamp(masterVolume, 0, 100);
		musicVolume = PlayerPrefs.GetInt("OptionsMusicVolume", musicVolume);
		musicVolume = Mathf.Clamp(musicVolume, 0, 100);
		switch (PlayerPrefs.GetInt("OptionsLockCursor", (int)lockCursorMode))
		{
		case 0:
			lockCursorMode = LockCursorMode.Off;
			break;
		case 1:
			lockCursorMode = LockCursorMode.On;
			break;
		default:
			lockCursorMode = LockCursorMode.Smart;
			break;
		}
		int num;
		if (PlayerPrefs.GetInt("OptionsEnableChatter", 1) == 0)
		{
			num = 0;
		}
		else
		{
			num = 1;
		}
		enableChatter = ((byte)num != 0);
		int num2;
		if (PlayerPrefs.GetInt("OptionsRightClickingConfirmsAbilityTargets", 1) == 0)
		{
			num2 = 0;
		}
		else
		{
			num2 = 1;
		}
		rightClickingConfirmsAbilityTargets = ((byte)num2 != 0);
		shiftClickForMovementWaypoints = ((PlayerPrefs.GetInt("OptionsShiftClickForMovementWaypoints", 1) != 0) ? true : false);
		int num3;
		if (PlayerPrefs.GetInt("OptionsShowGlobalChat", 1) == 0)
		{
			num3 = 0;
		}
		else
		{
			num3 = 1;
		}
		showGlobalChat = ((byte)num3 != 0);
		int num4;
		if (PlayerPrefs.GetInt("OptionsShowAllChat", 1) == 0)
		{
			num4 = 0;
		}
		else
		{
			num4 = 1;
		}
		showAllChat = ((byte)num4 != 0);
		int num5;
		if (PlayerPrefs.GetInt("OptionsEnableProfanityFilter", 1) == 0)
		{
			num5 = 0;
		}
		else
		{
			num5 = 1;
		}
		enableProfanityFilter = ((byte)num5 != 0);
		int num6;
		if (PlayerPrefs.GetInt("AutoJoinDiscord", 0) == 0)
		{
			num6 = 0;
		}
		else
		{
			num6 = 1;
		}
		autoJoinDiscord = ((byte)num6 != 0);
		int num7;
		if (PlayerPrefs.GetInt("VoicePushToTalk", 0) == 0)
		{
			num7 = 0;
		}
		else
		{
			num7 = 1;
		}
		voicePushToTalk = ((byte)num7 != 0);
		int num8;
		if (PlayerPrefs.GetInt("VoiceMute", 0) == 0)
		{
			num8 = 0;
		}
		else
		{
			num8 = 1;
		}
		voiceMute = ((byte)num8 != 0);
		voiceVolume = PlayerPrefs.GetFloat("VoiceVolume", 100f);
		micVolume = PlayerPrefs.GetFloat("MicVolume", 100f);
		region = (Region)PlayerPrefs.GetInt("Region", (int)Options_UI.GetDefaultRegion());
		switch (PlayerPrefs.GetInt("OptionsGameModeVoiceChat", (int)gameModeVoiceChat))
		{
		case 0:
			gameModeVoiceChat = VoiceChatMode.None;
			break;
		case 1:
			gameModeVoiceChat = VoiceChatMode.Group;
			break;
		default:
			gameModeVoiceChat = VoiceChatMode.Team;
			break;
		}
		int num9;
		if (PlayerPrefs.GetInt("HideTutorialVideos", 0) == 0)
		{
			num9 = 0;
		}
		else
		{
			num9 = 1;
		}
		hideTutorialVideos = ((byte)num9 != 0);
		int num10;
		if (PlayerPrefs.GetInt("AllowCancelActionWhileConfirmed", 0) == 0)
		{
			num10 = 0;
		}
		else
		{
			num10 = 1;
		}
		allowCancelActionWhileConfirmed = ((byte)num10 != 0);
		int num11;
		if (PlayerPrefs.GetInt("OptionsOverrideGlyphLanguage", 0) == 0)
		{
			num11 = 0;
		}
		else
		{
			num11 = 1;
		}
		overrideGlyphLanguage = ((byte)num11 != 0);
		overrideGlyphLanguageCode = PlayerPrefs.GetString("OverrideGlyphLanguageCode", string.Empty);
		int @int = PlayerPrefs.GetInt("OptionsVersion", 0);
		VersionPreferences(@int);
	}

	public void RevertVolume()
	{
		AudioMixer audioMixer = AudioManager.mixSnapshotManager.snapshot_game.audioMixer;
		float value = ConvertPercentToDb(masterVolume);
		audioMixer.SetFloat("VolMaster", value);
		float value2 = ConvertPercentToDb(musicVolume);
		audioMixer.SetFloat("VolMusic", value2);
		int percent = 100 - musicVolume;
		float value3 = ConvertPercentToDb(percent);
		audioMixer.SetFloat("VolUIAmbiance", value3);
	}

	public void ApplyPendingValues(SettingsState newState)
	{
		Options_UI.Get().StartCoroutine(ApplyPendingValuesInternal(newState));
	}

	public IEnumerator ReapplyPendingValues()
	{
		yield return Options_UI.Get().StartCoroutine(ApplyPendingValuesInternal(null));
		/*Error: Unable to find new state assignment for yield return*/;
	}

	public void UpdateGameResolution()
	{
		WindowMode num;
		if (AppState.IsInGame() && gameWindowMode != WindowMode.Inherit)
		{
			num = gameWindowMode;
		}
		else
		{
			num = windowMode;
		}
		int num2;
		if (num == WindowMode.Windowed)
		{
			num2 = 0;
		}
		else
		{
			num2 = 1;
		}
		bool flag = (byte)num2 != 0;
		int num3;
		if (AppState.IsInGame())
		{
			if (gameResolutionWidth != -1)
			{
				num3 = gameResolutionWidth;
				goto IL_006e;
			}
		}
		num3 = resolutionWidth;
		goto IL_006e;
		IL_00a1:
		int num4;
		int num5 = num4;
		if (Application.isEditor)
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
		if (!m_isResolutionInitialized)
		{
			m_isResolutionInitialized = true;
			if (!UIFrontendLoadingScreen.Get().IsSameAsInitialResolution())
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						UpdateResolutionFromScreen();
						return;
					}
				}
			}
		}
		int num6;
		if (flag == Screen.fullScreen)
		{
			if (num6 == Screen.width)
			{
				if (num5 == Screen.height)
				{
					return;
				}
			}
		}
		Screen.SetResolution(num6, num5, flag);
		return;
		IL_006e:
		num6 = num3;
		if (AppState.IsInGame())
		{
			if (gameResolutionHeight != -1)
			{
				num4 = gameResolutionHeight;
				goto IL_00a1;
			}
		}
		num4 = resolutionHeight;
		goto IL_00a1;
	}

	public void UpdateResolutionFromScreen()
	{
		bool flag = AppState.IsInGame() && gameWindowMode != WindowMode.Inherit;
		WindowMode windowMode = Screen.fullScreen ? WindowMode.Fullscreen : WindowMode.Windowed;
		if (!flag)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (Screen.width == resolutionWidth)
					{
						if (Screen.height == resolutionHeight)
						{
							if (windowMode == this.windowMode)
							{
								return;
							}
						}
					}
					resolutionWidth = Screen.width;
					resolutionHeight = Screen.height;
					this.windowMode = windowMode;
					ApplyToPlayerPrefs();
					Cursor.lockState = CursorLockMode.None;
					return;
				}
			}
		}
		if (Screen.width == gameResolutionWidth)
		{
			if (Screen.height == gameResolutionHeight)
			{
				if (windowMode == gameWindowMode)
				{
					return;
				}
			}
		}
		gameResolutionWidth = Screen.width;
		gameResolutionHeight = Screen.height;
		gameWindowMode = this.windowMode;
		ApplyToPlayerPrefs();
		Cursor.lockState = CursorLockMode.None;
	}

	private IEnumerator ApplyPendingValuesInternal(SettingsState newState)
	{
		Options_UI.Get().m_pauseUpdate = true;
		object obj;
		if (AudioManager.mixSnapshotManager != null)
		{
			obj = AudioManager.mixSnapshotManager.snapshot_game.audioMixer;
		}
		else
		{
			obj = null;
		}
		AudioMixer mixer = (AudioMixer)obj;
		if (mixer != null)
		{
			if (newState != null)
			{
				if (masterVolume == newState.masterVolume)
				{
					goto IL_0156;
				}
			}
			if (newState != null)
			{
				masterVolume = newState.masterVolume;
			}
			float value = ConvertPercentToDb(masterVolume);
			mixer.SetFloat("VolMaster", value);
			goto IL_0156;
		}
		goto IL_0247;
		IL_0156:
		if (newState != null)
		{
			if (musicVolume == newState.musicVolume)
			{
				goto IL_0219;
			}
		}
		if (newState != null)
		{
			musicVolume = newState.musicVolume;
		}
		float value2 = ConvertPercentToDb(musicVolume);
		mixer.SetFloat("VolMusic", value2);
		int percent = 100 - musicVolume;
		float value3 = ConvertPercentToDb(percent);
		mixer.SetFloat("VolUIAmbiance", value3);
		AudioManager.EnableMusicAtStartup();
		goto IL_0219;
		IL_0247:
		while (Options_UI.s_hwnd == (IntPtr)0)
		{
			yield return null;
			Options_UI.Get().TrySetupHwnd();
		}
		while (true)
		{
			yield return null;
			if (newState != null)
			{
				if (graphicsQuality == newState.graphicsQuality)
				{
					if (newState != null)
					{
						resolutionWidth = newState.resolutionWidth;
						resolutionHeight = newState.resolutionHeight;
						windowMode = newState.windowMode;
						gameResolutionWidth = newState.gameResolutionWidth;
						gameResolutionHeight = newState.gameResolutionHeight;
						gameWindowMode = newState.gameWindowMode;
					}
					UpdateGameResolution();
					yield return null;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (newState != null)
			{
				graphicsQuality = newState.graphicsQuality;
			}
			string qualityName;
			switch (graphicsQuality)
			{
			default:
				qualityName = "Fantastic";
				break;
			case GraphicsQuality.VeryLow:
				qualityName = "Fastest";
				break;
			case GraphicsQuality.Low:
				qualityName = "Fast";
				break;
			case GraphicsQuality.Medium:
				qualityName = "Simple";
				break;
			case GraphicsQuality.High:
				qualityName = "Fantastic";
				break;
			}
			string[] names = QualitySettings.names;
			int num = 0;
			while (true)
			{
				if (num < names.Length)
				{
					if (names[num] == qualityName)
					{
						QualitySettings.SetQualityLevel(num, true);
						break;
					}
					num++;
					continue;
				}
				break;
			}
			GameEventManager.Get().FireEvent(GameEventManager.EventType.GraphicsQualityChanged, null);
			yield return null;
			/*Error: Unable to find new state assignment for yield return*/;
		}
		IL_0219:
		AudioManager.EnableAmbianceAtStartup();
		goto IL_0247;
	}

	public void ApplyToOptionsUI()
	{
		switch (graphicsQuality)
		{
		default:
			break;
		case GraphicsQuality.VeryLow:
		case GraphicsQuality.Low:
			Options_UI.Get().m_graphicsLowButton.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_graphicsMediumButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_graphicsHighButton.SetSelected(false, false, string.Empty, string.Empty);
			break;
		case GraphicsQuality.Medium:
			Options_UI.Get().m_graphicsLowButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_graphicsMediumButton.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_graphicsHighButton.SetSelected(false, false, string.Empty, string.Empty);
			break;
		case GraphicsQuality.High:
			Options_UI.Get().m_graphicsLowButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_graphicsMediumButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_graphicsHighButton.SetSelected(true, false, string.Empty, string.Empty);
			break;
		}
		WindowMode thisWindowMode = windowMode;
		int thisResolutionWidth = resolutionWidth;
		int thisResolutionHeight = resolutionHeight;
		Action<string> setModeText = delegate(string modeText)
		{
			Options_UI.Get().SetWindowModeText(modeText);
		};
		if (_003C_003Ef__am_0024cache1 == null)
		{
			_003C_003Ef__am_0024cache1 = delegate(string resolutionText)
			{
				Options_UI.Get().SetResolutionText(resolutionText);
			};
		}
		UpdateModeResolution(thisWindowMode, thisResolutionWidth, thisResolutionHeight, setModeText, _003C_003Ef__am_0024cache1);
		WindowMode thisWindowMode2 = gameWindowMode;
		int thisResolutionWidth2 = gameResolutionWidth;
		int thisResolutionHeight2 = gameResolutionHeight;
		Action<string> setModeText2 = delegate(string modeText)
		{
			Options_UI.Get().SetGameWindowModeText(modeText);
		};
		if (_003C_003Ef__am_0024cache3 == null)
		{
			_003C_003Ef__am_0024cache3 = delegate(string resolutionText)
			{
				Options_UI.Get().SetGameResolutionText(resolutionText);
			};
		}
		UpdateModeResolution(thisWindowMode2, thisResolutionWidth2, thisResolutionHeight2, setModeText2, _003C_003Ef__am_0024cache3);
		Region region = this.region;
		if (region != 0)
		{
			if (region != Region.EU)
			{
			}
			else
			{
				Options_UI.Get().SetRegionText(StringUtil.TR("Europe", "Options"));
			}
		}
		else
		{
			Options_UI.Get().SetRegionText(StringUtil.TR("NorthAmerica", "Options"));
		}
		Options_UI.Get().m_masterVolumeLabel.text = Convert.ToString(masterVolume);
		Options_UI.Get().m_masterVolumeSlider.value = (float)masterVolume / 100f;
		Options_UI.Get().m_musicVolumeLabel.text = Convert.ToString(musicVolume);
		Options_UI.Get().m_musicVolumeSlider.value = (float)musicVolume / 100f;
		if (lockCursorMode == LockCursorMode.On)
		{
			Options_UI.Get().m_lockCursorButton.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_unlockCursorButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_smartLockCursorButton.SetSelected(false, false, string.Empty, string.Empty);
		}
		else if (lockCursorMode == LockCursorMode.Off)
		{
			Options_UI.Get().m_lockCursorButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_unlockCursorButton.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_smartLockCursorButton.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			Options_UI.Get().m_lockCursorButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_unlockCursorButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_smartLockCursorButton.SetSelected(true, false, string.Empty, string.Empty);
		}
		if (enableChatter)
		{
			Options_UI.Get().m_enableChatterButton.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_disableChatterButton.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			Options_UI.Get().m_enableChatterButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_disableChatterButton.SetSelected(true, false, string.Empty, string.Empty);
		}
		if (rightClickingConfirmsAbilityTargets)
		{
			Options_UI.Get().m_rightClickTargetingConfirm.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_rightClickTargetingCancel.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			Options_UI.Get().m_rightClickTargetingConfirm.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_rightClickTargetingCancel.SetSelected(true, false, string.Empty, string.Empty);
		}
		if (shiftClickForMovementWaypoints)
		{
			Options_UI.Get().m_shiftClickForWaypoints.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_shiftClickForNewPath.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			Options_UI.Get().m_shiftClickForWaypoints.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_shiftClickForNewPath.SetSelected(true, false, string.Empty, string.Empty);
		}
		if (showGlobalChat)
		{
			Options_UI.Get().m_showGlobalChatButton.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_hideGlobalChatButton.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			Options_UI.Get().m_showGlobalChatButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_hideGlobalChatButton.SetSelected(true, false, string.Empty, string.Empty);
		}
		if (showAllChat)
		{
			Options_UI.Get().m_showAllChatButton.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_hideAllChatButton.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			Options_UI.Get().m_showAllChatButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_hideAllChatButton.SetSelected(true, false, string.Empty, string.Empty);
		}
		if (enableProfanityFilter)
		{
			Options_UI.Get().m_enableProfanityFilterButton.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_disableProfanityFilterButton.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			Options_UI.Get().m_enableProfanityFilterButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_disableProfanityFilterButton.SetSelected(true, false, string.Empty, string.Empty);
		}
		if (!DiscordClientInterface.IsEnabled)
		{
			goto IL_0855;
		}
		if (!DiscordClientInterface.IsSdkEnabled)
		{
			if (!DiscordClientInterface.IsInstalled)
			{
				goto IL_0855;
			}
		}
		if (autoJoinDiscord)
		{
			Options_UI.Get().m_enableAutoJoinDiscordButton.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_disableAutoJoinDiscordButton.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			Options_UI.Get().m_enableAutoJoinDiscordButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_disableAutoJoinDiscordButton.SetSelected(true, false, string.Empty, string.Empty);
		}
		if (gameModeVoiceChat == VoiceChatMode.Team)
		{
			Options_UI.Get().m_groupGameModeVoiceChatButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_teamGameModeVoiceChatButton.SetSelected(true, false, string.Empty, string.Empty);
		}
		else
		{
			Options_UI.Get().m_groupGameModeVoiceChatButton.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_teamGameModeVoiceChatButton.SetSelected(false, false, string.Empty, string.Empty);
		}
		goto IL_08c3;
		IL_0855:
		Options_UI.Get().m_enableAutoJoinDiscordButton.SetSelected(false, false, string.Empty, string.Empty);
		Options_UI.Get().m_disableAutoJoinDiscordButton.SetSelected(false, false, string.Empty, string.Empty);
		Options_UI.Get().m_groupGameModeVoiceChatButton.SetSelected(false, false, string.Empty, string.Empty);
		Options_UI.Get().m_teamGameModeVoiceChatButton.SetSelected(false, false, string.Empty, string.Empty);
		goto IL_08c3;
		IL_08c3:
		if (hideTutorialVideos)
		{
			Options_UI.Get().m_hideTutorialVideosButton.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_showTutorialVideosButton.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			Options_UI.Get().m_hideTutorialVideosButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_showTutorialVideosButton.SetSelected(true, false, string.Empty, string.Empty);
		}
		if (allowCancelActionWhileConfirmed)
		{
			Options_UI.Get().m_allowCancelActionWhileConfirmedButton.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_disallowCancelActionWhileConfirmedButton.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			Options_UI.Get().m_allowCancelActionWhileConfirmedButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_disallowCancelActionWhileConfirmedButton.SetSelected(true, false, string.Empty, string.Empty);
		}
		if (overrideGlyphLanguage)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Options_UI.Get().SetLanguageText(StringUtil.TR(overrideGlyphLanguageCode, "LanguageSelection"));
					return;
				}
			}
		}
		Options_UI.Get().SetLanguageText(StringUtil.TR(LanguageOptions.GlyphSettings.ToString(), "LanguageSelection"));
	}

	private static void UpdateModeResolution(WindowMode thisWindowMode, int thisResolutionWidth, int thisResolutionHeight, Action<string> setModeText, Action<string> setResolutionText)
	{
		switch (thisWindowMode)
		{
		case WindowMode.WindowedFullscreen:
			break;
		case WindowMode.Windowed:
		{
			setModeText(StringUtil.TR("Windowed", "Options"));
			bool flag = false;
			Resolution[] resolutions = Screen.resolutions;
			for (int i = 0; i < resolutions.Length; i++)
			{
				Resolution resolution = resolutions[i];
				if (resolution.width == thisResolutionWidth)
				{
					if (resolution.height == thisResolutionHeight)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						setResolutionText(thisResolutionWidth + " x " + thisResolutionHeight);
						return;
					}
				}
			}
			setResolutionText(StringUtil.TR("Custom", "Options"));
			break;
		}
		case WindowMode.Fullscreen:
			setModeText(StringUtil.TR("Fullscreen", "Options"));
			setResolutionText(Screen.currentResolution.width + " x " + Screen.currentResolution.height);
			break;
		case WindowMode.Inherit:
			setModeText(StringUtil.TR("Same as Menu", "Options"));
			setResolutionText(StringUtil.TR("Same as Menu", "Options"));
			break;
		}
	}

	public void ApplyToPlayerPrefs()
	{
		PlayerPrefs.SetInt("OptionsGraphicsQuality", (int)graphicsQuality);
		if (graphicsQuality != GraphicsQuality.Medium)
		{
			PlayerPrefs.SetInt("OptionsGraphicsQualityEverSetManually", 1);
		}
		PlayerPrefs.SetInt("OptionsWindowMode", (int)windowMode);
		PlayerPrefs.SetInt("OptionsResolutionWidth", resolutionWidth);
		PlayerPrefs.SetInt("OptionsResolutionHeight", resolutionHeight);
		PlayerPrefs.SetInt("OptionsGameWindowMode", (int)gameWindowMode);
		PlayerPrefs.SetInt("OptionsGameResolutionWidth", gameResolutionWidth);
		PlayerPrefs.SetInt("OptionsGameResolutionHeight", gameResolutionHeight);
		PlayerPrefs.SetInt("OptionsMasterVolume", masterVolume);
		PlayerPrefs.SetInt("OptionsMusicVolume", musicVolume);
		PlayerPrefs.SetInt("OptionsLockCursor", (int)lockCursorMode);
		int value;
		if (enableChatter)
		{
			value = 1;
		}
		else
		{
			value = 0;
		}
		PlayerPrefs.SetInt("OptionsEnableChatter", value);
		int value2;
		if (rightClickingConfirmsAbilityTargets)
		{
			value2 = 1;
		}
		else
		{
			value2 = 0;
		}
		PlayerPrefs.SetInt("OptionsRightClickingConfirmsAbilityTargets", value2);
		int value3;
		if (shiftClickForMovementWaypoints)
		{
			value3 = 1;
		}
		else
		{
			value3 = 0;
		}
		PlayerPrefs.SetInt("OptionsShiftClickForMovementWaypoints", value3);
		int value4;
		if (showGlobalChat)
		{
			value4 = 1;
		}
		else
		{
			value4 = 0;
		}
		PlayerPrefs.SetInt("OptionsShowGlobalChat", value4);
		PlayerPrefs.SetInt("OptionsShowAllChat", showAllChat ? 1 : 0);
		int value5;
		if (enableProfanityFilter)
		{
			value5 = 1;
		}
		else
		{
			value5 = 0;
		}
		PlayerPrefs.SetInt("OptionsEnableProfanityFilter", value5);
		int value6;
		if (autoJoinDiscord)
		{
			value6 = 1;
		}
		else
		{
			value6 = 0;
		}
		PlayerPrefs.SetInt("AutoJoinDiscord", value6);
		int value7;
		if (voicePushToTalk)
		{
			value7 = 1;
		}
		else
		{
			value7 = 0;
		}
		PlayerPrefs.SetInt("VoicePushToTalk", value7);
		int value8;
		if (voiceMute)
		{
			value8 = 1;
		}
		else
		{
			value8 = 0;
		}
		PlayerPrefs.SetInt("VoiceMute", value8);
		PlayerPrefs.SetFloat("VoiceVolume", voiceVolume);
		PlayerPrefs.SetFloat("MicVolume", micVolume);
		PlayerPrefs.SetInt("Region", (int)region);
		PlayerPrefs.SetInt("OptionsGameModeVoiceChat", (int)gameModeVoiceChat);
		int value9;
		if (hideTutorialVideos)
		{
			value9 = 1;
		}
		else
		{
			value9 = 0;
		}
		PlayerPrefs.SetInt("HideTutorialVideos", value9);
		int value10;
		if (allowCancelActionWhileConfirmed)
		{
			value10 = 1;
		}
		else
		{
			value10 = 0;
		}
		PlayerPrefs.SetInt("AllowCancelActionWhileConfirmed", value10);
		PlayerPrefs.SetInt("OptionsOverrideGlyphLanguage", overrideGlyphLanguage ? 1 : 0);
		PlayerPrefs.SetString("OverrideGlyphLanguageCode", overrideGlyphLanguageCode);
		if (windowMode == WindowMode.Windowed)
		{
			PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 0);
			PlayerPrefs.SetInt("Screenmanager Resolution Width", resolutionWidth);
			PlayerPrefs.SetInt("Screenmanager Resolution Height", resolutionHeight);
		}
		else
		{
			PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 1);
			PlayerPrefs.SetInt("Screenmanager Resolution Width", Screen.currentResolution.width);
			PlayerPrefs.SetInt("Screenmanager Resolution Height", Screen.currentResolution.height);
		}
		PlayerPrefs.Save();
	}

	public float ConvertPercentToDb(int percent)
	{
		if (percent == 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return -80f;
				}
			}
		}
		float value = 20f * Mathf.Log((float)percent * 0.01f) / Mathf.Log(10f);
		return Mathf.Clamp(value, -80f, 0f);
	}

	public int ConvertDbToPercent(float db)
	{
		if (db <= -80f)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return 0;
				}
			}
		}
		int value = Convert.ToInt32(100f * Mathf.Pow(10f, db / 20f));
		return Mathf.Clamp(value, 0, 100);
	}
}
