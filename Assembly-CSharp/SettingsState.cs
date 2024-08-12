using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsState : ICloneable
{
    private const GraphicsQuality GRAPHICS_QUALITY_DEFAULT = GraphicsQuality.Medium;
    private const int OPTIONS_VERSION = 14;

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
    
#if EVOS
    public bool allowResettingWaypoints;
#endif

    public object Clone()
    {
        return MemberwiseClone();
    }

    public void InitToDefaults()
    {
        graphicsQuality = GRAPHICS_QUALITY_DEFAULT;
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

#if EVOS
        EvosOptions.Get().InitDefaults(this);
#endif
    }

    private void VersionPreferences(int version)
    {
        if (version >= OPTIONS_VERSION)
        {
            return;
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
            graphicsQuality = GRAPHICS_QUALITY_DEFAULT;
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

        Log.Info(Log.Category.UI, $"Versioned Options from {version} to {OPTIONS_VERSION}");
        PlayerPrefs.SetInt("OptionsVersion", OPTIONS_VERSION);
        ApplyToPlayerPrefs();
    }

    public void InitFromPlayerPrefs()
    {
        InitToDefaults();
        switch (PlayerPrefs.GetInt("OptionsGraphicsQuality", (int)graphicsQuality))
        {
            case (int)GraphicsQuality.Low:
                graphicsQuality = GraphicsQuality.Low;
                break;
            case (int)GraphicsQuality.Medium:
                graphicsQuality = GraphicsQuality.Medium;
                break;
            case (int)GraphicsQuality.High:
                graphicsQuality = GraphicsQuality.High;
                break;
            case -10:
                graphicsQuality = GraphicsQuality.VeryLow;
                break;
            default:
                graphicsQuality = GraphicsQuality.High;
                break;
        }

        switch (PlayerPrefs.GetInt("OptionsWindowMode", (int)windowMode))
        {
            case (int)WindowMode.Windowed:
                windowMode = WindowMode.Windowed;
                break;
            case (int)WindowMode.Fullscreen:
                windowMode = WindowMode.Fullscreen;
                break;
            case (int)WindowMode.WindowedFullscreen:
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
            case (int)WindowMode.Windowed:
                gameWindowMode = WindowMode.Windowed;
                break;
            case (int)WindowMode.Fullscreen:
                gameWindowMode = WindowMode.Fullscreen;
                break;
            case (int)WindowMode.WindowedFullscreen:
                gameWindowMode = WindowMode.Fullscreen;
                break;
            case (int)WindowMode.Inherit:
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
            case (int)LockCursorMode.Off:
                lockCursorMode = LockCursorMode.Off;
                break;
            case (int)LockCursorMode.On:
                lockCursorMode = LockCursorMode.On;
                break;
            default:
                lockCursorMode = LockCursorMode.Smart;
                break;
        }

        enableChatter = PlayerPrefs.GetInt("OptionsEnableChatter", 1) != 0;
        rightClickingConfirmsAbilityTargets = PlayerPrefs.GetInt("OptionsRightClickingConfirmsAbilityTargets", 1) != 0;
        shiftClickForMovementWaypoints = PlayerPrefs.GetInt("OptionsShiftClickForMovementWaypoints", 1) != 0;
        showGlobalChat = PlayerPrefs.GetInt("OptionsShowGlobalChat", 1) != 0;
        showAllChat = PlayerPrefs.GetInt("OptionsShowAllChat", 1) != 0;
        enableProfanityFilter = PlayerPrefs.GetInt("OptionsEnableProfanityFilter", 1) != 0;
        autoJoinDiscord = PlayerPrefs.GetInt("AutoJoinDiscord", 0) != 0;
        voicePushToTalk = PlayerPrefs.GetInt("VoicePushToTalk", 0) != 0;
        voiceMute = PlayerPrefs.GetInt("VoiceMute", 0) != 0;
        voiceVolume = PlayerPrefs.GetFloat("VoiceVolume", 100f);
        micVolume = PlayerPrefs.GetFloat("MicVolume", 100f);
        region = (Region)PlayerPrefs.GetInt("Region", (int)Options_UI.GetDefaultRegion());
        switch (PlayerPrefs.GetInt("OptionsGameModeVoiceChat", (int)gameModeVoiceChat))
        {
            case (int)VoiceChatMode.None:
                gameModeVoiceChat = VoiceChatMode.None;
                break;
            case (int)VoiceChatMode.Group:
                gameModeVoiceChat = VoiceChatMode.Group;
                break;
            default:
                gameModeVoiceChat = VoiceChatMode.Team;
                break;
        }

        hideTutorialVideos = PlayerPrefs.GetInt("HideTutorialVideos", 0) != 0;
        allowCancelActionWhileConfirmed = PlayerPrefs.GetInt("AllowCancelActionWhileConfirmed", 0) != 0;
        overrideGlyphLanguage = PlayerPrefs.GetInt("OptionsOverrideGlyphLanguage", 0) != 0;
        overrideGlyphLanguageCode = PlayerPrefs.GetString("OverrideGlyphLanguageCode", string.Empty);
        VersionPreferences(PlayerPrefs.GetInt("OptionsVersion", 0));

#if EVOS
        EvosOptions.Get().LoadFromPrefs(this);
#endif
    }

    public void RevertVolume()
    {
        AudioMixer audioMixer = AudioManager.mixSnapshotManager.snapshot_game.audioMixer;
        audioMixer.SetFloat("VolMaster", ConvertPercentToDb(masterVolume));
        audioMixer.SetFloat("VolMusic", ConvertPercentToDb(musicVolume));
        audioMixer.SetFloat("VolUIAmbiance", ConvertPercentToDb(100 - musicVolume));
    }

    public void ApplyPendingValues(SettingsState newState)
    {
        Options_UI.Get().StartCoroutine(ApplyPendingValuesInternal(newState));
    }

    public IEnumerator ReapplyPendingValues()
    {
        yield return Options_UI.Get().StartCoroutine(ApplyPendingValuesInternal(null));
    }

    public void UpdateGameResolution()
    {
        bool fullscreen = AppState.IsInGame() && gameWindowMode != WindowMode.Inherit
            ? gameWindowMode != WindowMode.Windowed
            : windowMode != WindowMode.Windowed;
        int width = AppState.IsInGame() && gameResolutionWidth != -1
            ? gameResolutionWidth
            : resolutionWidth;
        int height = AppState.IsInGame() && gameResolutionHeight != -1
            ? gameResolutionHeight
            : resolutionHeight;

        if (Application.isEditor)
        {
            return;
        }

        if (!m_isResolutionInitialized)
        {
            m_isResolutionInitialized = true;
            if (!UIFrontendLoadingScreen.Get().IsSameAsInitialResolution())
            {
                UpdateResolutionFromScreen();
                return;
            }
        }

        if (fullscreen != Screen.fullScreen
            || width != Screen.width
            || height != Screen.height)
        {
            Screen.SetResolution(width, height, fullscreen);
        }
    }

    public void UpdateResolutionFromScreen()
    {
        WindowMode actualWindowMode = Screen.fullScreen
            ? WindowMode.Fullscreen
            : WindowMode.Windowed;
        if (AppState.IsInGame() && gameWindowMode != WindowMode.Inherit)
        {
            if (Screen.width != gameResolutionWidth
                || Screen.height != gameResolutionHeight
                || actualWindowMode != gameWindowMode)
            {
                gameResolutionWidth = Screen.width;
                gameResolutionHeight = Screen.height;
                gameWindowMode = windowMode;
                ApplyToPlayerPrefs();
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            if (Screen.width != resolutionWidth
                || Screen.height != resolutionHeight
                || actualWindowMode != windowMode)
            {
                resolutionWidth = Screen.width;
                resolutionHeight = Screen.height;
                windowMode = actualWindowMode;
                ApplyToPlayerPrefs();
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    private IEnumerator ApplyPendingValuesInternal(SettingsState newState)
    {
        Options_UI.Get().m_pauseUpdate = true;
        AudioMixer mixer = AudioManager.mixSnapshotManager != null
            ? AudioManager.mixSnapshotManager.snapshot_game.audioMixer
            : null;

        if (mixer != null)
        {
            if (newState == null || masterVolume != newState.masterVolume)
            {
                if (newState != null)
                {
                    masterVolume = newState.masterVolume;
                }

                mixer.SetFloat("VolMaster", ConvertPercentToDb(masterVolume));
            }

            if (newState == null || musicVolume != newState.musicVolume)
            {
                if (newState != null)
                {
                    musicVolume = newState.musicVolume;
                }

                mixer.SetFloat("VolMusic", ConvertPercentToDb(musicVolume));
                mixer.SetFloat("VolUIAmbiance", ConvertPercentToDb(100 - musicVolume));
                AudioManager.EnableMusicAtStartup();
            }

            AudioManager.EnableAmbianceAtStartup();
        }

        while (Options_UI.s_hwnd == (IntPtr)0)
        {
            yield return null;
            Options_UI.Get().TrySetupHwnd();
        }

        yield return null;

        if (newState == null || graphicsQuality != newState.graphicsQuality)
        {
            if (newState != null)
            {
                graphicsQuality = newState.graphicsQuality;
            }

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
                case GraphicsQuality.VeryLow:
                    qualityName = "Fastest";
                    break;
                default:
                    qualityName = "Fantastic";
                    break;
            }

            for (int i = 0; i < QualitySettings.names.Length; i++)
            {
                if (QualitySettings.names[i] == qualityName)
                {
                    QualitySettings.SetQualityLevel(i, true);
                    break;
                }
            }

            GameEventManager.Get().FireEvent(GameEventManager.EventType.GraphicsQualityChanged, null);
            yield return null;
        }

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

        if (newState != null)
        {
            lockCursorMode = newState.lockCursorMode;
        }

        Cursor.lockState = lockCursorMode != LockCursorMode.On
                           && (lockCursorMode != LockCursorMode.Smart || GameFlowData.Get() == null)
            ? CursorLockMode.None
            : CursorLockMode.Confined;

        if (newState == null || enableChatter != newState.enableChatter)
        {
            yield return null;
            if (newState != null)
            {
                enableChatter = newState.enableChatter;
            }
        }

        if (newState == null || rightClickingConfirmsAbilityTargets != newState.rightClickingConfirmsAbilityTargets)
        {
            yield return null;
            if (newState != null)
            {
                rightClickingConfirmsAbilityTargets = newState.rightClickingConfirmsAbilityTargets;
            }
        }

        if (newState == null || shiftClickForMovementWaypoints != newState.shiftClickForMovementWaypoints)
        {
            yield return null;
            if (newState != null)
            {
                shiftClickForMovementWaypoints = newState.shiftClickForMovementWaypoints;
            }
        }
        
        if (newState == null || showGlobalChat != newState.showGlobalChat)
        {
            yield return null;
            if (newState != null)
            {
                showGlobalChat = newState.showGlobalChat;
            }
        }

        if (newState == null || showAllChat != newState.showAllChat)
        {
            yield return null;
            if (newState != null)
            {
                showAllChat = newState.showAllChat;
            }
        }

        if (newState == null || enableProfanityFilter != newState.enableProfanityFilter)
        {
            yield return null;
            if (newState != null)
            {
                enableProfanityFilter = newState.enableProfanityFilter;
            }
        }

        if (newState == null || autoJoinDiscord != newState.autoJoinDiscord)
        {
            yield return null;
            if (newState != null)
            {
                autoJoinDiscord = newState.autoJoinDiscord;
            }
        }

        if (newState == null || voicePushToTalk != newState.voicePushToTalk)
        {
            yield return null;
            if (newState != null)
            {
                voicePushToTalk = newState.voicePushToTalk;
            }
        }

        if (newState == null || voiceMute != newState.voiceMute)
        {
            yield return null;
            if (newState != null)
            {
                voiceMute = newState.voiceMute;
            }
        }

        if (newState == null || voiceVolume != newState.voiceVolume)
        {
            yield return null;
            if (newState != null)
            {
                voiceVolume = newState.voiceVolume;
            }
        }

        if (newState == null || micVolume != newState.micVolume)
        {
            yield return null;
            if (newState != null)
            {
                micVolume = newState.micVolume;
            }
        }

        if (newState == null || region != newState.region)
        {
            yield return null;
            if (newState != null)
            {
                region = newState.region;
                ClientGameManager.Get().SendSetRegionRequest(region);
            }
        }

        if (newState == null || gameModeVoiceChat != newState.gameModeVoiceChat)
        {
            yield return null;
            if (newState != null)
            {
                gameModeVoiceChat = newState.gameModeVoiceChat;
            }
        }

        if (newState == null || hideTutorialVideos != newState.hideTutorialVideos)
        {
            yield return null;
            if (newState != null)
            {
                hideTutorialVideos = newState.hideTutorialVideos;
            }
        }

        if (newState == null || allowCancelActionWhileConfirmed != newState.allowCancelActionWhileConfirmed)
        {
            yield return null;
            if (newState != null)
            {
                allowCancelActionWhileConfirmed = newState.allowCancelActionWhileConfirmed;
            }
        }

        if (newState == null || overrideGlyphLanguage != newState.overrideGlyphLanguage)
        {
            yield return null;
            if (newState != null)
            {
                overrideGlyphLanguage = newState.overrideGlyphLanguage;
            }
        }

        if (newState == null || overrideGlyphLanguageCode != newState.overrideGlyphLanguageCode)
        {
            yield return null;
            if (newState != null)
            {
                overrideGlyphLanguageCode = newState.overrideGlyphLanguageCode;
            }
        }

#if EVOS
        foreach (EvosOptions.Option option in EvosOptions.Get().m_options)
        {
            if (newState == null || option.stateGetter(this) != option.stateGetter(newState))
            {
                yield return null;
                if (newState != null)
                {
                    option.stateSetter(this, option.stateGetter(newState));
                }
            }
        }
#endif

        Options_UI.Get().m_pauseUpdate = false;
        if (newState != null)
        {
            ApplyToPlayerPrefs();
        }
    }

    public void ApplyToOptionsUI()
    {
        Options_UI.Get().UpdateGraphicsQualityButtons(graphicsQuality);

        UpdateModeResolution(
            windowMode,
            resolutionWidth,
            resolutionHeight,
            Options_UI.Get().SetWindowModeText,
            Options_UI.Get().SetResolutionText);

        UpdateModeResolution(
            gameWindowMode,
            gameResolutionWidth,
            gameResolutionHeight,
            Options_UI.Get().SetGameWindowModeText,
            Options_UI.Get().SetGameResolutionText);

        Options_UI.Get().SetRegionText(region);
        
        Options_UI.Get().m_masterVolumeLabel.text = Convert.ToString(masterVolume);
        Options_UI.Get().m_masterVolumeSlider.value = masterVolume / 100f;
        Options_UI.Get().m_musicVolumeLabel.text = Convert.ToString(musicVolume);
        Options_UI.Get().m_musicVolumeSlider.value = musicVolume / 100f;
        
        Options_UI.Get().UpdateLockCursorButtons(lockCursorMode);
        Options_UI.Get().UpdateChatterButtons(enableChatter);
        Options_UI.Get().UpdateRightClickButtons(rightClickingConfirmsAbilityTargets);
        Options_UI.Get().UpdateShiftClickWaypointsButtons(shiftClickForMovementWaypoints);
        Options_UI.Get().UpdateShowGlobalChatButtons(showGlobalChat);
        Options_UI.Get().UpdateShowAllChatButtons(showAllChat);
        Options_UI.Get().UpdateProfanityFilterButtons(enableProfanityFilter);

        if (DiscordClientInterface.IsEnabled
            && (DiscordClientInterface.IsSdkEnabled || DiscordClientInterface.IsInstalled))
        {
            Options_UI.Get().UpdateAutoJoinDiscordButtons(autoJoinDiscord);
            Options_UI.Get().UpdateVoiceChatModeButtons(gameModeVoiceChat != VoiceChatMode.Team);
        }
        else
        {
            Options_UI.Get().m_enableAutoJoinDiscordButton.SetSelected(false);
            Options_UI.Get().m_disableAutoJoinDiscordButton.SetSelected(false);
            Options_UI.Get().m_groupGameModeVoiceChatButton.SetSelected(false);
            Options_UI.Get().m_teamGameModeVoiceChatButton.SetSelected(false);
        }

        Options_UI.Get().UpdateTutorialVideosButtons(hideTutorialVideos);
        Options_UI.Get().UpdateAllowCancelActionWhileConfirmedButtons(allowCancelActionWhileConfirmed);
        Options_UI.Get().SetLanguageText(
            overrideGlyphLanguage
                ? StringUtil.TR(overrideGlyphLanguageCode, "LanguageSelection")
                : StringUtil.TR(LanguageOptions.GlyphSettings.ToString(), "LanguageSelection"));

#if EVOS
        EvosOptions.Get().UpdateButtons(this);
#endif
    }

    private static void UpdateModeResolution(
        WindowMode thisWindowMode,
        int thisResolutionWidth,
        int thisResolutionHeight,
        Action<string> setModeText,
        Action<string> setResolutionText)
    {
        switch (thisWindowMode)
        {
            case WindowMode.Windowed:
            {
                setModeText(StringUtil.TR("Windowed", "Options"));

                bool isStandard = false;
                foreach (Resolution resolution in Screen.resolutions)
                {
                    if (resolution.width == thisResolutionWidth && resolution.height == thisResolutionHeight)
                    {
                        isStandard = true;
                    }
                }

                if (isStandard)
                {
                    setResolutionText(thisResolutionWidth + " x " + thisResolutionHeight);
                }
                else
                {
                    setResolutionText(StringUtil.TR("Custom", "Options"));
                }

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
        if (graphicsQuality != GRAPHICS_QUALITY_DEFAULT)
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
        PlayerPrefs.SetInt("OptionsEnableChatter", enableChatter ? 1 : 0);
        PlayerPrefs.SetInt("OptionsRightClickingConfirmsAbilityTargets", rightClickingConfirmsAbilityTargets ? 1 : 0);
        PlayerPrefs.SetInt("OptionsShiftClickForMovementWaypoints", shiftClickForMovementWaypoints ? 1 : 0);
        PlayerPrefs.SetInt("OptionsShowGlobalChat", showGlobalChat ? 1 : 0);
        PlayerPrefs.SetInt("OptionsShowAllChat", showAllChat ? 1 : 0);
        PlayerPrefs.SetInt("OptionsEnableProfanityFilter", enableProfanityFilter ? 1 : 0);
        PlayerPrefs.SetInt("AutoJoinDiscord", autoJoinDiscord ? 1 : 0);
        PlayerPrefs.SetInt("VoicePushToTalk", voicePushToTalk ? 1 : 0);
        PlayerPrefs.SetInt("VoiceMute", voiceMute ? 1 : 0);
        PlayerPrefs.SetFloat("VoiceVolume", voiceVolume);
        PlayerPrefs.SetFloat("MicVolume", micVolume);
        PlayerPrefs.SetInt("Region", (int)region);
        PlayerPrefs.SetInt("OptionsGameModeVoiceChat", (int)gameModeVoiceChat);
        PlayerPrefs.SetInt("HideTutorialVideos", hideTutorialVideos ? 1 : 0);
        PlayerPrefs.SetInt("AllowCancelActionWhileConfirmed", allowCancelActionWhileConfirmed ? 1 : 0);
        PlayerPrefs.SetInt("OptionsOverrideGlyphLanguage", !overrideGlyphLanguage ? 0 : 1);
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

#if EVOS
        EvosOptions.Get().SaveToPrefs(this);
#endif

        PlayerPrefs.Save();
    }

    public float ConvertPercentToDb(int percent)
    {
        if (percent == 0)
        {
            return -80f;
        }

        float value = 20f * Mathf.Log(percent * 0.01f) / Mathf.Log(10f);
        return Mathf.Clamp(value, -80f, 0f);
    }

    public int ConvertDbToPercent(float db)
    {
        if (db <= -80f)
        {
            return 0;
        }

        int value = Convert.ToInt32(100f * Mathf.Pow(10f, db / 20f));
        return Mathf.Clamp(value, 0, 100);
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