using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsState : ICloneable
{
	private const GraphicsQuality GRAPHICS_QUALITY_DEFAULT = GraphicsQuality.Medium;

	public GraphicsQuality graphicsQuality;

	public SettingsState.WindowMode windowMode;

	public int resolutionWidth;

	public int resolutionHeight;

	public SettingsState.WindowMode gameWindowMode;

	public int gameResolutionWidth;

	public int gameResolutionHeight;

	public bool lockWindowSize;

	public int masterVolume;

	public int musicVolume;

	public int ambianceVolume;

	public SettingsState.LockCursorMode lockCursorMode;

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

	public SettingsState.VoiceChatMode gameModeVoiceChat;

	public bool hideTutorialVideos;

	public Region region;

	public bool allowCancelActionWhileConfirmed;

	public bool overrideGlyphLanguage;

	public string overrideGlyphLanguageCode;

	private bool m_isResolutionInitialized;

	private const int OPTIONS_VERSION = 0xE;

	public object Clone()
	{
		return base.MemberwiseClone();
	}

	public void InitToDefaults()
	{
		this.graphicsQuality = GraphicsQuality.Medium;
		this.windowMode = SettingsState.WindowMode.Fullscreen;
		this.gameWindowMode = SettingsState.WindowMode.Inherit;
		this.resolutionWidth = Screen.currentResolution.width;
		this.resolutionHeight = Screen.currentResolution.height;
		this.gameResolutionWidth = -1;
		this.gameResolutionHeight = -1;
		if (Application.isEditor)
		{
			this.resolutionWidth = 0x640;
			this.resolutionHeight = 0x384;
		}
		this.lockWindowSize = false;
		this.masterVolume = 0x64;
		this.musicVolume = 0x64;
		this.ambianceVolume = 0x64;
		this.lockCursorMode = SettingsState.LockCursorMode.Smart;
		this.enableChatter = true;
		this.rightClickingConfirmsAbilityTargets = false;
		this.shiftClickForMovementWaypoints = true;
		this.showGlobalChat = true;
		this.showAllChat = false;
		this.enableProfanityFilter = true;
		this.autoJoinDiscord = false;
		this.voicePushToTalk = false;
		this.voiceMute = false;
		this.voiceVolume = 100f;
		this.micVolume = 100f;
		this.region = Options_UI.GetDefaultRegion();
		this.gameModeVoiceChat = SettingsState.VoiceChatMode.Group;
		this.hideTutorialVideos = false;
		this.allowCancelActionWhileConfirmed = true;
		this.overrideGlyphLanguage = false;
		this.overrideGlyphLanguageCode = string.Empty;
	}

	private void VersionPreferences(int version)
	{
		if (version >= 0xE)
		{
			return;
		}
		if (version < 1)
		{
			this.musicVolume = 0x64;
			this.ambianceVolume = 0x64;
			this.windowMode = SettingsState.WindowMode.Fullscreen;
		}
		if (version < 2)
		{
		}
		if (version < 3)
		{
			this.windowMode = SettingsState.WindowMode.WindowedFullscreen;
			this.resolutionWidth = Screen.currentResolution.width;
			this.resolutionHeight = Screen.currentResolution.height;
			this.graphicsQuality = GraphicsQuality.Medium;
		}
		if (version < 4)
		{
			this.lockCursorMode = SettingsState.LockCursorMode.Smart;
		}
		if (version < 5 && this.windowMode == SettingsState.WindowMode.WindowedFullscreen)
		{
			this.windowMode = SettingsState.WindowMode.Fullscreen;
			this.resolutionWidth = Screen.currentResolution.width;
			this.resolutionHeight = Screen.currentResolution.height;
		}
		if (version < 6)
		{
			this.showAllChat = false;
		}
		if (version < 7)
		{
			this.shiftClickForMovementWaypoints = false;
		}
		if (version < 8)
		{
			this.autoJoinDiscord = false;
		}
		if (version < 9)
		{
			this.gameModeVoiceChat = SettingsState.VoiceChatMode.Group;
		}
		if (version < 0xA)
		{
			this.hideTutorialVideos = false;
		}
		if (version < 0xB)
		{
			this.region = Options_UI.GetDefaultRegion();
		}
		if (version < 0xC)
		{
			this.allowCancelActionWhileConfirmed = false;
		}
		if (version < 0xD)
		{
			this.overrideGlyphLanguage = false;
			this.overrideGlyphLanguageCode = string.Empty;
		}
		if (version < 0xE)
		{
			this.voicePushToTalk = false;
			this.voiceMute = false;
			this.voiceVolume = 100f;
			this.micVolume = 100f;
		}
		Log.Info(Log.Category.UI, string.Format("Versioned Options from {0} to {1}", version, 0xE), new object[0]);
		PlayerPrefs.SetInt("OptionsVersion", 0xE);
		this.ApplyToPlayerPrefs();
	}

	public void InitFromPlayerPrefs()
	{
		this.InitToDefaults();
		int @int = PlayerPrefs.GetInt("OptionsGraphicsQuality", (int)this.graphicsQuality);
		switch (@int)
		{
		case 0:
			this.graphicsQuality = GraphicsQuality.Low;
			break;
		case 1:
			this.graphicsQuality = GraphicsQuality.Medium;
			break;
		case 2:
			this.graphicsQuality = GraphicsQuality.High;
			break;
		default:
			if (@int != -0xA)
			{
				this.graphicsQuality = GraphicsQuality.High;
			}
			else
			{
				this.graphicsQuality = GraphicsQuality.VeryLow;
			}
			break;
		}
		switch (PlayerPrefs.GetInt("OptionsWindowMode", (int)this.windowMode))
		{
		case 0:
			this.windowMode = SettingsState.WindowMode.Windowed;
			break;
		case 1:
			this.windowMode = SettingsState.WindowMode.Fullscreen;
			break;
		case 2:
			this.windowMode = SettingsState.WindowMode.Fullscreen;
			break;
		default:
			this.windowMode = SettingsState.WindowMode.Windowed;
			break;
		}
		this.resolutionWidth = PlayerPrefs.GetInt("OptionsResolutionWidth", this.resolutionWidth);
		this.resolutionHeight = PlayerPrefs.GetInt("OptionsResolutionHeight", this.resolutionHeight);
		switch (PlayerPrefs.GetInt("OptionsGameWindowMode", (int)this.gameWindowMode))
		{
		case 0:
			this.gameWindowMode = SettingsState.WindowMode.Windowed;
			break;
		case 1:
			this.gameWindowMode = SettingsState.WindowMode.Fullscreen;
			break;
		case 2:
			this.gameWindowMode = SettingsState.WindowMode.Fullscreen;
			break;
		case 3:
			this.gameWindowMode = SettingsState.WindowMode.Inherit;
			break;
		default:
			this.gameWindowMode = SettingsState.WindowMode.Windowed;
			break;
		}
		this.gameResolutionWidth = PlayerPrefs.GetInt("OptionsGameResolutionWidth", this.gameResolutionWidth);
		this.gameResolutionHeight = PlayerPrefs.GetInt("OptionsGameResolutionHeight", this.gameResolutionHeight);
		if (this.gameWindowMode == SettingsState.WindowMode.Inherit)
		{
			this.gameResolutionWidth = -1;
			this.gameResolutionHeight = -1;
		}
		this.lockWindowSize = false;
		this.masterVolume = PlayerPrefs.GetInt("OptionsMasterVolume", this.masterVolume);
		this.masterVolume = Mathf.Clamp(this.masterVolume, 0, 0x64);
		this.musicVolume = PlayerPrefs.GetInt("OptionsMusicVolume", this.musicVolume);
		this.musicVolume = Mathf.Clamp(this.musicVolume, 0, 0x64);
		switch (PlayerPrefs.GetInt("OptionsLockCursor", (int)this.lockCursorMode))
		{
		case 0:
			this.lockCursorMode = SettingsState.LockCursorMode.Off;
			goto IL_230;
		case 1:
			this.lockCursorMode = SettingsState.LockCursorMode.On;
			goto IL_230;
		}
		this.lockCursorMode = SettingsState.LockCursorMode.Smart;
		IL_230:
		bool flag;
		if (PlayerPrefs.GetInt("OptionsEnableChatter", 1) == 0)
		{
			flag = false;
		}
		else
		{
			flag = true;
		}
		this.enableChatter = flag;
		bool flag2;
		if (PlayerPrefs.GetInt("OptionsRightClickingConfirmsAbilityTargets", 1) == 0)
		{
			flag2 = false;
		}
		else
		{
			flag2 = true;
		}
		this.rightClickingConfirmsAbilityTargets = flag2;
		this.shiftClickForMovementWaypoints = (PlayerPrefs.GetInt("OptionsShiftClickForMovementWaypoints", 1) != 0);
		bool flag3;
		if (PlayerPrefs.GetInt("OptionsShowGlobalChat", 1) == 0)
		{
			flag3 = false;
		}
		else
		{
			flag3 = true;
		}
		this.showGlobalChat = flag3;
		bool flag4;
		if (PlayerPrefs.GetInt("OptionsShowAllChat", 1) == 0)
		{
			flag4 = false;
		}
		else
		{
			flag4 = true;
		}
		this.showAllChat = flag4;
		bool flag5;
		if (PlayerPrefs.GetInt("OptionsEnableProfanityFilter", 1) == 0)
		{
			flag5 = false;
		}
		else
		{
			flag5 = true;
		}
		this.enableProfanityFilter = flag5;
		bool flag6;
		if (PlayerPrefs.GetInt("AutoJoinDiscord", 0) == 0)
		{
			flag6 = false;
		}
		else
		{
			flag6 = true;
		}
		this.autoJoinDiscord = flag6;
		bool flag7;
		if (PlayerPrefs.GetInt("VoicePushToTalk", 0) == 0)
		{
			flag7 = false;
		}
		else
		{
			flag7 = true;
		}
		this.voicePushToTalk = flag7;
		bool flag8;
		if (PlayerPrefs.GetInt("VoiceMute", 0) == 0)
		{
			flag8 = false;
		}
		else
		{
			flag8 = true;
		}
		this.voiceMute = flag8;
		this.voiceVolume = PlayerPrefs.GetFloat("VoiceVolume", 100f);
		this.micVolume = PlayerPrefs.GetFloat("MicVolume", 100f);
		this.region = (Region)PlayerPrefs.GetInt("Region", (int)Options_UI.GetDefaultRegion());
		switch (PlayerPrefs.GetInt("OptionsGameModeVoiceChat", (int)this.gameModeVoiceChat))
		{
		case 0:
			this.gameModeVoiceChat = SettingsState.VoiceChatMode.None;
			goto IL_3E4;
		case 1:
			this.gameModeVoiceChat = SettingsState.VoiceChatMode.Group;
			goto IL_3E4;
		}
		this.gameModeVoiceChat = SettingsState.VoiceChatMode.Team;
		IL_3E4:
		bool flag9;
		if (PlayerPrefs.GetInt("HideTutorialVideos", 0) == 0)
		{
			flag9 = false;
		}
		else
		{
			flag9 = true;
		}
		this.hideTutorialVideos = flag9;
		bool flag10;
		if (PlayerPrefs.GetInt("AllowCancelActionWhileConfirmed", 0) == 0)
		{
			flag10 = false;
		}
		else
		{
			flag10 = true;
		}
		this.allowCancelActionWhileConfirmed = flag10;
		bool flag11;
		if (PlayerPrefs.GetInt("OptionsOverrideGlyphLanguage", 0) == 0)
		{
			flag11 = false;
		}
		else
		{
			flag11 = true;
		}
		this.overrideGlyphLanguage = flag11;
		this.overrideGlyphLanguageCode = PlayerPrefs.GetString("OverrideGlyphLanguageCode", string.Empty);
		int int2 = PlayerPrefs.GetInt("OptionsVersion", 0);
		this.VersionPreferences(int2);
	}

	public void RevertVolume()
	{
		AudioMixer audioMixer = AudioManager.mixSnapshotManager.snapshot_game.audioMixer;
		float value = this.ConvertPercentToDb(this.masterVolume);
		audioMixer.SetFloat("VolMaster", value);
		float value2 = this.ConvertPercentToDb(this.musicVolume);
		audioMixer.SetFloat("VolMusic", value2);
		int percent = 0x64 - this.musicVolume;
		float value3 = this.ConvertPercentToDb(percent);
		audioMixer.SetFloat("VolUIAmbiance", value3);
	}

	public void ApplyPendingValues(SettingsState newState)
	{
		Options_UI.Get().StartCoroutine(this.ApplyPendingValuesInternal(newState));
	}

	public IEnumerator ReapplyPendingValues()
	{
		yield return Options_UI.Get().StartCoroutine(this.ApplyPendingValuesInternal(null));
		yield break;
	}

	public void UpdateGameResolution()
	{
		bool flag;
		if (AppState.IsInGame() && this.gameWindowMode != SettingsState.WindowMode.Inherit)
		{
			flag = (this.gameWindowMode != SettingsState.WindowMode.Windowed);
		}
		else
		{
			flag = (this.windowMode != SettingsState.WindowMode.Windowed);
		}
		bool flag2;
		if (!flag)
		{
			flag2 = false;
		}
		else
		{
			flag2 = true;
		}
		bool flag3 = flag2;
		int num;
		if (AppState.IsInGame())
		{
			if (this.gameResolutionWidth != -1)
			{
				num = this.gameResolutionWidth;
				goto IL_6E;
			}
		}
		num = this.resolutionWidth;
		IL_6E:
		int num2 = num;
		int num3;
		if (AppState.IsInGame())
		{
			if (this.gameResolutionHeight != -1)
			{
				num3 = this.gameResolutionHeight;
				goto IL_A1;
			}
		}
		num3 = this.resolutionHeight;
		IL_A1:
		int num4 = num3;
		if (Application.isEditor)
		{
			return;
		}
		if (!this.m_isResolutionInitialized)
		{
			this.m_isResolutionInitialized = true;
			if (!UIFrontendLoadingScreen.Get().IsSameAsInitialResolution())
			{
				this.UpdateResolutionFromScreen();
				return;
			}
		}
		if (flag3 == Screen.fullScreen)
		{
			if (num2 == Screen.width)
			{
				if (num4 == Screen.height)
				{
					return;
				}
			}
		}
		Screen.SetResolution(num2, num4, flag3);
	}

	public void UpdateResolutionFromScreen()
	{
		bool flag = AppState.IsInGame() && this.gameWindowMode != SettingsState.WindowMode.Inherit;
		SettingsState.WindowMode windowMode = (!Screen.fullScreen) ? SettingsState.WindowMode.Windowed : SettingsState.WindowMode.Fullscreen;
		if (!flag)
		{
			if (Screen.width == this.resolutionWidth)
			{
				if (Screen.height == this.resolutionHeight)
				{
					if (windowMode == this.windowMode)
					{
						goto IL_A4;
					}
				}
			}
			this.resolutionWidth = Screen.width;
			this.resolutionHeight = Screen.height;
			this.windowMode = windowMode;
			this.ApplyToPlayerPrefs();
			Cursor.lockState = CursorLockMode.None;
			IL_A4:;
		}
		else
		{
			if (Screen.width == this.gameResolutionWidth)
			{
				if (Screen.height == this.gameResolutionHeight)
				{
					if (windowMode == this.gameWindowMode)
					{
						return;
					}
				}
			}
			this.gameResolutionWidth = Screen.width;
			this.gameResolutionHeight = Screen.height;
			this.gameWindowMode = this.windowMode;
			this.ApplyToPlayerPrefs();
			Cursor.lockState = CursorLockMode.None;
		}
	}

	private IEnumerator ApplyPendingValuesInternal(SettingsState newState)
	{
		Options_UI.Get().m_pauseUpdate = true;
		AudioMixer audioMixer;
		if (AudioManager.mixSnapshotManager != null)
		{
			audioMixer = AudioManager.mixSnapshotManager.snapshot_game.audioMixer;
		}
		else
		{
			audioMixer = null;
		}
		AudioMixer mixer = audioMixer;
		if (mixer != null)
		{
			if (newState != null)
			{
				if (this.masterVolume == newState.masterVolume)
				{
					goto IL_156;
				}
			}
			if (newState != null)
			{
				this.masterVolume = newState.masterVolume;
			}
			float value = this.ConvertPercentToDb(this.masterVolume);
			mixer.SetFloat("VolMaster", value);
			IL_156:
			if (newState != null)
			{
				if (this.musicVolume == newState.musicVolume)
				{
					goto IL_219;
				}
			}
			if (newState != null)
			{
				this.musicVolume = newState.musicVolume;
			}
			float value2 = this.ConvertPercentToDb(this.musicVolume);
			mixer.SetFloat("VolMusic", value2);
			int percent = 0x64 - this.musicVolume;
			float value3 = this.ConvertPercentToDb(percent);
			mixer.SetFloat("VolUIAmbiance", value3);
			AudioManager.EnableMusicAtStartup();
			IL_219:
			AudioManager.EnableAmbianceAtStartup();
		}
		while (Options_UI.s_hwnd == (IntPtr)0)
		{
			yield return null;
			Options_UI.Get().TrySetupHwnd();
		}
		yield return null;
		if (newState != null)
		{
			if (this.graphicsQuality == newState.graphicsQuality)
			{
				goto IL_3E3;
			}
		}
		if (newState != null)
		{
			this.graphicsQuality = newState.graphicsQuality;
		}
		GraphicsQuality graphicsQuality = this.graphicsQuality;
		string qualityName;
		switch (graphicsQuality)
		{
		case GraphicsQuality.Low:
			qualityName = "Fast";
			break;
		case GraphicsQuality.Medium:
			qualityName = "Simple";
			break;
		case GraphicsQuality.High:
			qualityName = "Fantastic";
			break;
		default:
			if (graphicsQuality != GraphicsQuality.VeryLow)
			{
				qualityName = "Fantastic";
			}
			else
			{
				qualityName = "Fastest";
			}
			break;
		}
		string[] names = QualitySettings.names;
		int i = 0;
		while (i < names.Length)
		{
			if (names[i] == qualityName)
			{
				QualitySettings.SetQualityLevel(i, true);
				break;
				
			}
			else
			{
				i++;
			}
		}
		GameEventManager.Get().FireEvent(GameEventManager.EventType.GraphicsQualityChanged, null);
		yield return null;

	IL_3E3:
		if (newState != null)
		{
			this.resolutionWidth = newState.resolutionWidth;
			this.resolutionHeight = newState.resolutionHeight;
			this.windowMode = newState.windowMode;
			this.gameResolutionWidth = newState.gameResolutionWidth;
			this.gameResolutionHeight = newState.gameResolutionHeight;
			this.gameWindowMode = newState.gameWindowMode;
		}
		this.UpdateGameResolution();
		yield return null;
		if (newState != null)
		{
			this.lockCursorMode = newState.lockCursorMode;
		}
		if (this.lockCursorMode != SettingsState.LockCursorMode.On)
		{
			if (this.lockCursorMode == SettingsState.LockCursorMode.Smart)
			{
				if (GameFlowData.Get() != null)
				{
					goto IL_507;
				}
			}
			Cursor.lockState = CursorLockMode.None;
			goto IL_515;
		}
		IL_507:
		Cursor.lockState = CursorLockMode.Confined;
		IL_515:
		if (newState != null)
		{
			if (this.enableChatter == newState.enableChatter)
			{
				goto IL_596;
			}
		}
		yield return null;
		if (newState != null)
		{
			this.enableChatter = newState.enableChatter;
		}
		IL_596:
		if (newState != null)
		{
			if (this.rightClickingConfirmsAbilityTargets == newState.rightClickingConfirmsAbilityTargets)
			{
				goto IL_617;
			}
		}
		yield return null;
		if (newState != null)
		{
			this.rightClickingConfirmsAbilityTargets = newState.rightClickingConfirmsAbilityTargets;
		}
		IL_617:
		if (newState != null)
		{
			if (this.shiftClickForMovementWaypoints == newState.shiftClickForMovementWaypoints)
			{
				goto IL_698;
			}
		}
		yield return null;
		if (newState != null)
		{
			this.shiftClickForMovementWaypoints = newState.shiftClickForMovementWaypoints;
		}
		IL_698:
		if (newState != null)
		{
			if (this.showGlobalChat == newState.showGlobalChat)
			{
				goto IL_6FB;
			}
		}
		yield return null;
		if (newState != null)
		{
			this.showGlobalChat = newState.showGlobalChat;
		}
		IL_6FB:
		if (newState != null)
		{
			if (this.showAllChat == newState.showAllChat)
			{
				goto IL_77D;
			}
		}
		yield return null;
		if (newState != null)
		{
			this.showAllChat = newState.showAllChat;
		}
		IL_77D:
		if (newState != null)
		{
			if (this.enableProfanityFilter == newState.enableProfanityFilter)
			{
				goto IL_7F5;
			}
		}
		yield return null;
		if (newState != null)
		{
			this.enableProfanityFilter = newState.enableProfanityFilter;
		}
		IL_7F5:
		if (newState != null)
		{
			if (this.autoJoinDiscord == newState.autoJoinDiscord)
			{
				goto IL_86D;
			}
		}
		yield return null;
		if (newState != null)
		{
			this.autoJoinDiscord = newState.autoJoinDiscord;
		}
		IL_86D:
		if (newState != null)
		{
			if (this.voicePushToTalk == newState.voicePushToTalk)
			{
				goto IL_8E5;
			}
		}
		yield return null;
		if (newState != null)
		{
			this.voicePushToTalk = newState.voicePushToTalk;
		}
		IL_8E5:
		if (newState != null)
		{
			if (this.voiceMute == newState.voiceMute)
			{
				goto IL_953;
			}
		}
		yield return null;
		if (newState != null)
		{
			this.voiceMute = newState.voiceMute;
		}
		IL_953:
		if (newState != null)
		{
			if (this.voiceVolume == newState.voiceVolume)
			{
				goto IL_9D5;
			}
		}
		yield return null;
		if (newState != null)
		{
			this.voiceVolume = newState.voiceVolume;
		}
		IL_9D5:
		if (newState != null)
		{
			if (this.micVolume == newState.micVolume)
			{
				goto IL_A4D;
			}
		}
		yield return null;
		if (newState != null)
		{
			this.micVolume = newState.micVolume;
		}
		IL_A4D:
		if (newState != null)
		{
			if (this.region == newState.region)
			{
				goto IL_ADC;
			}
		}
		yield return null;
		if (newState != null)
		{
			this.region = newState.region;
			ClientGameManager.Get().SendSetRegionRequest(this.region);
		}
		IL_ADC:
		if (newState != null)
		{
			if (this.gameModeVoiceChat == newState.gameModeVoiceChat)
			{
				goto IL_B5E;
			}
		}
		yield return null;
		if (newState != null)
		{
			this.gameModeVoiceChat = newState.gameModeVoiceChat;
		}
		IL_B5E:
		if (newState != null)
		{
			if (this.hideTutorialVideos == newState.hideTutorialVideos)
			{
				goto IL_BD6;
			}
		}
		yield return null;
		if (newState != null)
		{
			this.hideTutorialVideos = newState.hideTutorialVideos;
		}
		IL_BD6:
		if (newState != null)
		{
			if (this.allowCancelActionWhileConfirmed == newState.allowCancelActionWhileConfirmed)
			{
				goto IL_C44;
			}
		}
		yield return null;
		if (newState != null)
		{
			this.allowCancelActionWhileConfirmed = newState.allowCancelActionWhileConfirmed;
		}
		IL_C44:
		if (newState != null)
		{
			if (this.overrideGlyphLanguage == newState.overrideGlyphLanguage)
			{
				goto IL_CA8;
			}
		}
		yield return null;
		if (newState != null)
		{
			this.overrideGlyphLanguage = newState.overrideGlyphLanguage;
		}
		IL_CA8:
		if (newState != null)
		{
			if (!(this.overrideGlyphLanguageCode != newState.overrideGlyphLanguageCode))
			{
				goto IL_D1A;
			}
		}
		yield return null;
		if (newState != null)
		{
			this.overrideGlyphLanguageCode = newState.overrideGlyphLanguageCode;
		}
		IL_D1A:
		Options_UI.Get().m_pauseUpdate = false;
		if (newState != null)
		{
			this.ApplyToPlayerPrefs();
		}
		yield break;
	}

	public void ApplyToOptionsUI()
	{
		GraphicsQuality graphicsQuality = this.graphicsQuality;
		switch (graphicsQuality)
		{
		case GraphicsQuality.Low:
			break;
		case GraphicsQuality.Medium:
			Options_UI.Get().m_graphicsLowButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_graphicsMediumButton.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_graphicsHighButton.SetSelected(false, false, string.Empty, string.Empty);
			goto IL_13E;
		case GraphicsQuality.High:
			Options_UI.Get().m_graphicsLowButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_graphicsMediumButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_graphicsHighButton.SetSelected(true, false, string.Empty, string.Empty);
			goto IL_13E;
		default:
			if (graphicsQuality != GraphicsQuality.VeryLow)
			{
				goto IL_13E;
			}
			break;
		}
		Options_UI.Get().m_graphicsLowButton.SetSelected(true, false, string.Empty, string.Empty);
		Options_UI.Get().m_graphicsMediumButton.SetSelected(false, false, string.Empty, string.Empty);
		Options_UI.Get().m_graphicsHighButton.SetSelected(false, false, string.Empty, string.Empty);
		IL_13E:
		SettingsState.WindowMode thisWindowMode = this.windowMode;
		int thisResolutionWidth = this.resolutionWidth;
		int thisResolutionHeight = this.resolutionHeight;
		Action<string> setModeText = delegate(string modeText)
		{
			Options_UI.Get().SetWindowModeText(modeText);
		};
		
		SettingsState.UpdateModeResolution(thisWindowMode, thisResolutionWidth, thisResolutionHeight, setModeText, delegate(string resolutionText)
			{
				Options_UI.Get().SetResolutionText(resolutionText);
			});
		SettingsState.WindowMode thisWindowMode2 = this.gameWindowMode;
		int thisResolutionWidth2 = this.gameResolutionWidth;
		int thisResolutionHeight2 = this.gameResolutionHeight;
		Action<string> setModeText2 = delegate(string modeText)
		{
			Options_UI.Get().SetGameWindowModeText(modeText);
		};
		
		SettingsState.UpdateModeResolution(thisWindowMode2, thisResolutionWidth2, thisResolutionHeight2, setModeText2, delegate(string resolutionText)
			{
				Options_UI.Get().SetGameResolutionText(resolutionText);
			});
		Region region = this.region;
		if (region != Region.US)
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
		Options_UI.Get().m_masterVolumeLabel.text = Convert.ToString(this.masterVolume);
		Options_UI.Get().m_masterVolumeSlider.value = (float)this.masterVolume / 100f;
		Options_UI.Get().m_musicVolumeLabel.text = Convert.ToString(this.musicVolume);
		Options_UI.Get().m_musicVolumeSlider.value = (float)this.musicVolume / 100f;
		if (this.lockCursorMode == SettingsState.LockCursorMode.On)
		{
			Options_UI.Get().m_lockCursorButton.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_unlockCursorButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_smartLockCursorButton.SetSelected(false, false, string.Empty, string.Empty);
		}
		else if (this.lockCursorMode == SettingsState.LockCursorMode.Off)
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
		if (this.enableChatter)
		{
			Options_UI.Get().m_enableChatterButton.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_disableChatterButton.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			Options_UI.Get().m_enableChatterButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_disableChatterButton.SetSelected(true, false, string.Empty, string.Empty);
		}
		if (this.rightClickingConfirmsAbilityTargets)
		{
			Options_UI.Get().m_rightClickTargetingConfirm.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_rightClickTargetingCancel.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			Options_UI.Get().m_rightClickTargetingConfirm.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_rightClickTargetingCancel.SetSelected(true, false, string.Empty, string.Empty);
		}
		if (this.shiftClickForMovementWaypoints)
		{
			Options_UI.Get().m_shiftClickForWaypoints.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_shiftClickForNewPath.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			Options_UI.Get().m_shiftClickForWaypoints.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_shiftClickForNewPath.SetSelected(true, false, string.Empty, string.Empty);
		}
		if (this.showGlobalChat)
		{
			Options_UI.Get().m_showGlobalChatButton.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_hideGlobalChatButton.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			Options_UI.Get().m_showGlobalChatButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_hideGlobalChatButton.SetSelected(true, false, string.Empty, string.Empty);
		}
		if (this.showAllChat)
		{
			Options_UI.Get().m_showAllChatButton.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_hideAllChatButton.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			Options_UI.Get().m_showAllChatButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_hideAllChatButton.SetSelected(true, false, string.Empty, string.Empty);
		}
		if (this.enableProfanityFilter)
		{
			Options_UI.Get().m_enableProfanityFilterButton.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_disableProfanityFilterButton.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			Options_UI.Get().m_enableProfanityFilterButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_disableProfanityFilterButton.SetSelected(true, false, string.Empty, string.Empty);
		}
		if (DiscordClientInterface.IsEnabled)
		{
			if (!DiscordClientInterface.IsSdkEnabled)
			{
				if (!DiscordClientInterface.IsInstalled)
				{
					goto IL_855;
				}
			}
			if (this.autoJoinDiscord)
			{
				Options_UI.Get().m_enableAutoJoinDiscordButton.SetSelected(true, false, string.Empty, string.Empty);
				Options_UI.Get().m_disableAutoJoinDiscordButton.SetSelected(false, false, string.Empty, string.Empty);
			}
			else
			{
				Options_UI.Get().m_enableAutoJoinDiscordButton.SetSelected(false, false, string.Empty, string.Empty);
				Options_UI.Get().m_disableAutoJoinDiscordButton.SetSelected(true, false, string.Empty, string.Empty);
			}
			if (this.gameModeVoiceChat == SettingsState.VoiceChatMode.Team)
			{
				Options_UI.Get().m_groupGameModeVoiceChatButton.SetSelected(false, false, string.Empty, string.Empty);
				Options_UI.Get().m_teamGameModeVoiceChatButton.SetSelected(true, false, string.Empty, string.Empty);
			}
			else
			{
				Options_UI.Get().m_groupGameModeVoiceChatButton.SetSelected(true, false, string.Empty, string.Empty);
				Options_UI.Get().m_teamGameModeVoiceChatButton.SetSelected(false, false, string.Empty, string.Empty);
			}
			goto IL_8C3;
		}
		IL_855:
		Options_UI.Get().m_enableAutoJoinDiscordButton.SetSelected(false, false, string.Empty, string.Empty);
		Options_UI.Get().m_disableAutoJoinDiscordButton.SetSelected(false, false, string.Empty, string.Empty);
		Options_UI.Get().m_groupGameModeVoiceChatButton.SetSelected(false, false, string.Empty, string.Empty);
		Options_UI.Get().m_teamGameModeVoiceChatButton.SetSelected(false, false, string.Empty, string.Empty);
		IL_8C3:
		if (this.hideTutorialVideos)
		{
			Options_UI.Get().m_hideTutorialVideosButton.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_showTutorialVideosButton.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			Options_UI.Get().m_hideTutorialVideosButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_showTutorialVideosButton.SetSelected(true, false, string.Empty, string.Empty);
		}
		if (this.allowCancelActionWhileConfirmed)
		{
			Options_UI.Get().m_allowCancelActionWhileConfirmedButton.SetSelected(true, false, string.Empty, string.Empty);
			Options_UI.Get().m_disallowCancelActionWhileConfirmedButton.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			Options_UI.Get().m_allowCancelActionWhileConfirmedButton.SetSelected(false, false, string.Empty, string.Empty);
			Options_UI.Get().m_disallowCancelActionWhileConfirmedButton.SetSelected(true, false, string.Empty, string.Empty);
		}
		if (this.overrideGlyphLanguage)
		{
			Options_UI.Get().SetLanguageText(StringUtil.TR(this.overrideGlyphLanguageCode, "LanguageSelection"));
		}
		else
		{
			Options_UI.Get().SetLanguageText(StringUtil.TR(LanguageOptions.GlyphSettings.ToString(), "LanguageSelection"));
		}
	}

	private static void UpdateModeResolution(SettingsState.WindowMode thisWindowMode, int thisResolutionWidth, int thisResolutionHeight, Action<string> setModeText, Action<string> setResolutionText)
	{
		switch (thisWindowMode)
		{
		case SettingsState.WindowMode.Windowed:
		{
			setModeText(StringUtil.TR("Windowed", "Options"));
			bool flag = false;
			foreach (Resolution resolution in Screen.resolutions)
			{
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
				setResolutionText(thisResolutionWidth.ToString() + " x " + thisResolutionHeight.ToString());
			}
			else
			{
				setResolutionText(StringUtil.TR("Custom", "Options"));
			}
			break;
		}
		case SettingsState.WindowMode.Fullscreen:
			setModeText(StringUtil.TR("Fullscreen", "Options"));
			setResolutionText(Screen.currentResolution.width.ToString() + " x " + Screen.currentResolution.height.ToString());
			break;
		case SettingsState.WindowMode.Inherit:
			setModeText(StringUtil.TR("Same as Menu", "Options"));
			setResolutionText(StringUtil.TR("Same as Menu", "Options"));
			break;
		}
	}

	public void ApplyToPlayerPrefs()
	{
		PlayerPrefs.SetInt("OptionsGraphicsQuality", (int)this.graphicsQuality);
		if (this.graphicsQuality != GraphicsQuality.Medium)
		{
			PlayerPrefs.SetInt("OptionsGraphicsQualityEverSetManually", 1);
		}
		PlayerPrefs.SetInt("OptionsWindowMode", (int)this.windowMode);
		PlayerPrefs.SetInt("OptionsResolutionWidth", this.resolutionWidth);
		PlayerPrefs.SetInt("OptionsResolutionHeight", this.resolutionHeight);
		PlayerPrefs.SetInt("OptionsGameWindowMode", (int)this.gameWindowMode);
		PlayerPrefs.SetInt("OptionsGameResolutionWidth", this.gameResolutionWidth);
		PlayerPrefs.SetInt("OptionsGameResolutionHeight", this.gameResolutionHeight);
		PlayerPrefs.SetInt("OptionsMasterVolume", this.masterVolume);
		PlayerPrefs.SetInt("OptionsMusicVolume", this.musicVolume);
		PlayerPrefs.SetInt("OptionsLockCursor", (int)this.lockCursorMode);
		string key = "OptionsEnableChatter";
		int value;
		if (this.enableChatter)
		{
			value = 1;
		}
		else
		{
			value = 0;
		}
		PlayerPrefs.SetInt(key, value);
		string key2 = "OptionsRightClickingConfirmsAbilityTargets";
		int value2;
		if (this.rightClickingConfirmsAbilityTargets)
		{
			value2 = 1;
		}
		else
		{
			value2 = 0;
		}
		PlayerPrefs.SetInt(key2, value2);
		string key3 = "OptionsShiftClickForMovementWaypoints";
		int value3;
		if (this.shiftClickForMovementWaypoints)
		{
			value3 = 1;
		}
		else
		{
			value3 = 0;
		}
		PlayerPrefs.SetInt(key3, value3);
		string key4 = "OptionsShowGlobalChat";
		int value4;
		if (this.showGlobalChat)
		{
			value4 = 1;
		}
		else
		{
			value4 = 0;
		}
		PlayerPrefs.SetInt(key4, value4);
		PlayerPrefs.SetInt("OptionsShowAllChat", (!this.showAllChat) ? 0 : 1);
		string key5 = "OptionsEnableProfanityFilter";
		int value5;
		if (this.enableProfanityFilter)
		{
			value5 = 1;
		}
		else
		{
			value5 = 0;
		}
		PlayerPrefs.SetInt(key5, value5);
		string key6 = "AutoJoinDiscord";
		int value6;
		if (this.autoJoinDiscord)
		{
			value6 = 1;
		}
		else
		{
			value6 = 0;
		}
		PlayerPrefs.SetInt(key6, value6);
		string key7 = "VoicePushToTalk";
		int value7;
		if (this.voicePushToTalk)
		{
			value7 = 1;
		}
		else
		{
			value7 = 0;
		}
		PlayerPrefs.SetInt(key7, value7);
		string key8 = "VoiceMute";
		int value8;
		if (this.voiceMute)
		{
			value8 = 1;
		}
		else
		{
			value8 = 0;
		}
		PlayerPrefs.SetInt(key8, value8);
		PlayerPrefs.SetFloat("VoiceVolume", this.voiceVolume);
		PlayerPrefs.SetFloat("MicVolume", this.micVolume);
		PlayerPrefs.SetInt("Region", (int)this.region);
		PlayerPrefs.SetInt("OptionsGameModeVoiceChat", (int)this.gameModeVoiceChat);
		string key9 = "HideTutorialVideos";
		int value9;
		if (this.hideTutorialVideos)
		{
			value9 = 1;
		}
		else
		{
			value9 = 0;
		}
		PlayerPrefs.SetInt(key9, value9);
		string key10 = "AllowCancelActionWhileConfirmed";
		int value10;
		if (this.allowCancelActionWhileConfirmed)
		{
			value10 = 1;
		}
		else
		{
			value10 = 0;
		}
		PlayerPrefs.SetInt(key10, value10);
		PlayerPrefs.SetInt("OptionsOverrideGlyphLanguage", (!this.overrideGlyphLanguage) ? 0 : 1);
		PlayerPrefs.SetString("OverrideGlyphLanguageCode", this.overrideGlyphLanguageCode);
		if (this.windowMode == SettingsState.WindowMode.Windowed)
		{
			PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 0);
			PlayerPrefs.SetInt("Screenmanager Resolution Width", this.resolutionWidth);
			PlayerPrefs.SetInt("Screenmanager Resolution Height", this.resolutionHeight);
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
			return -80f;
		}
		float value = 20f * Mathf.Log((float)percent * 0.01f) / Mathf.Log(10f);
		return Mathf.Clamp(value, -80f, 0f);
	}

	public int ConvertDbToPercent(float db)
	{
		if (db <= -80f)
		{
			return 0;
		}
		int value = Convert.ToInt32(100f * Mathf.Pow(10f, db / 20f));
		return Mathf.Clamp(value, 0, 0x64);
	}

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
}
