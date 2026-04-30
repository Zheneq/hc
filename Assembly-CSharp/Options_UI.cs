using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using I2.Loc;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Options_UI : UIScene, IGameEventListener
{
    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    private struct ResolutionSetting
    {
        public Resolution resolution;
        public bool custom;
    }

    public RectTransform m_container;
    public Button m_secretButton;
    public TextMeshProUGUI m_secretButtonText;
    public _SelectableBtn m_okButton;
    public _SelectableBtn m_applyButton;
    public _SelectableBtn m_revertDefaultsButton;
    public _SelectableBtn m_closeButton;

    public _SelectableBtn m_graphicsLowButton;
    public _SelectableBtn m_graphicsMediumButton;
    public _SelectableBtn m_graphicsHighButton;

    public TextMeshProUGUI[] m_windowModeText;
    public _SelectableBtn m_windowModeButton;
    public GameObject m_windowModeDropdown;
    public _SelectableBtn m_windowModeWindowedButton;
    public _SelectableBtn m_windowModeFullscreenButton;

    public TextMeshProUGUI[] m_resolutionText;
    public _SelectableBtn m_resolutionButton;
    public GameObject m_resolutionDropdown;
    public GridLayoutGroup m_resolutionItemContainer;
    public ScrollRect m_ResolutionScrollView;
    public Image m_resolutionBackgroundHitBox;

    public TextMeshProUGUI[] m_gameWindowModeText;
    public _SelectableBtn m_gameWindowModeButton;
    public GameObject m_gameWindowModeDropdown;
    public _SelectableBtn m_gameWindowModeInheritButton;
    public _SelectableBtn m_gameWindowModeWindowedButton;
    public _SelectableBtn m_gameWindowModeFullscreenButton;

    public TextMeshProUGUI[] m_gameResolutionText;
    public _SelectableBtn m_gameResolutionButton;
    public GameObject m_gameResolutionDropdown;
    public GridLayoutGroup m_gameResolutionItemContainer;
    public ScrollRect m_gameResolutionScrollView;
    public Image m_gameResolutionBackgroundHitBox;

    public TextMeshProUGUI[] m_regionText;
    public _SelectableBtn m_regionButton;
    public GameObject m_regionDropdown;
    public _SelectableBtn m_regionNorthAmericaButton;
    public _SelectableBtn m_regionEuropeButton;

    public TextMeshProUGUI[] m_languageText;
    public _SelectableBtn m_languageButton;
    public GameObject m_languageDropdown;
    public GridLayoutGroup m_languageItemContainer;
    public ScrollRect m_LanguageScrollView;
    public Image m_languageBackgroundHitBox;

    public GameObject m_restartWarning;

    public Slider m_masterVolumeSlider;
    public TextMeshProUGUI m_masterVolumeLabel;
    public Slider m_musicVolumeSlider;
    public TextMeshProUGUI m_musicVolumeLabel;

    public _SelectableBtn m_lockCursorButton;
    public _SelectableBtn m_unlockCursorButton;
    public _SelectableBtn m_smartLockCursorButton;

    public _SelectableBtn m_enableChatterButton;
    public _SelectableBtn m_disableChatterButton;

    public _SelectableBtn m_rightClickTargetingConfirm;
    public _SelectableBtn m_rightClickTargetingCancel;

    public _SelectableBtn m_shiftClickForWaypoints;
    public _SelectableBtn m_shiftClickForNewPath;

    public _SelectableBtn m_showGlobalChatButton;
    public _SelectableBtn m_hideGlobalChatButton;

    public _SelectableBtn m_showAllChatButton;
    public _SelectableBtn m_hideAllChatButton;

    public _SelectableBtn m_enableProfanityFilterButton;
    public _SelectableBtn m_disableProfanityFilterButton;

    public _SelectableBtn m_enableAutoJoinDiscordButton;
    public _SelectableBtn m_disableAutoJoinDiscordButton;

    public _SelectableBtn m_groupGameModeVoiceChatButton;
    public _SelectableBtn m_teamGameModeVoiceChatButton;

    public _SelectableBtn m_hideTutorialVideosButton;
    public _SelectableBtn m_showTutorialVideosButton;

    public _SelectableBtn m_allowCancelActionWhileConfirmedButton;
    public _SelectableBtn m_disallowCancelActionWhileConfirmedButton;

    [HideInInspector]
    private SettingsState m_activeState;
    [HideInInspector]
    private SettingsState m_pendingState;
    [HideInInspector]
    private bool m_dontUpdateSliders;
    [HideInInspector]
    public bool m_pauseUpdate = true;
    [HideInInspector]
    public int m_borderWidth;
    [HideInInspector]
    public int m_captionHeight;
    [HideInInspector]
    public bool m_forceStyleUpdate;
    [HideInInspector]
    public bool m_secretButtonClicked;

    private ScrollRect m_scrollRect;

    private static Options_UI s_instance;
    public static IntPtr s_hwnd;

    private bool m_firstSetupTry = true;
    public GameObject m_resolutionButtonPrefab;

    private List<GameObject> m_resolutionButtons = new List<GameObject>();
    private Dictionary<GameObject, ResolutionSetting> m_resolutionButtonData =
        new Dictionary<GameObject, ResolutionSetting>();
    private List<GameObject> m_languageButtons = new List<GameObject>();
    public TextMeshProUGUI m_optionsLabelText;

    public string ActiveStateName => m_activeState.ToString();

    public static Options_UI Get()
    {
        return s_instance;
    }

    public override SceneType GetSceneType()
    {
        return SceneType.Options;
    }

    public override void Awake()
    {
        s_instance = this;
        m_scrollRect = GetComponentInChildren<ScrollRect>();
        UIManager.SetGameObjectActive(m_container, false);
        s_hwnd = (IntPtr)0;
        m_pauseUpdate = true;
        UIManager.SetGameObjectActive(m_resolutionDropdown, false);
        UIManager.SetGameObjectActive(m_windowModeDropdown, false);
        UIManager.SetGameObjectActive(m_gameResolutionDropdown, false);
        UIManager.SetGameObjectActive(m_gameWindowModeDropdown, false);
        UIManager.SetGameObjectActive(m_regionDropdown, false);
        UIManager.SetGameObjectActive(m_languageDropdown, false);
        UIManager.SetGameObjectActive(m_restartWarning, false);
        if (m_ResolutionScrollView != null && m_resolutionBackgroundHitBox != null)
        {
            m_resolutionBackgroundHitBox.gameObject
                .AddComponent<_MouseEventPasser>()
                .AddNewHandler(m_ResolutionScrollView);
        }

        if (m_gameResolutionScrollView != null && m_gameResolutionBackgroundHitBox != null)
        {
            m_gameResolutionBackgroundHitBox.gameObject
                .AddComponent<_MouseEventPasser>()
                .AddNewHandler(m_gameResolutionScrollView);
        }

        if (m_LanguageScrollView != null && m_languageBackgroundHitBox != null)
        {
            m_languageBackgroundHitBox.gameObject
                .AddComponent<_MouseEventPasser>()
                .AddNewHandler(m_LanguageScrollView);
        }

        m_graphicsLowButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_graphicsMediumButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_graphicsHighButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_windowModeButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_windowModeWindowedButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_windowModeFullscreenButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_resolutionButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_gameWindowModeButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_gameWindowModeInheritButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_gameWindowModeWindowedButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_gameWindowModeFullscreenButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_gameResolutionButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_regionButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_regionNorthAmericaButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_regionEuropeButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_languageButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_lockCursorButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_unlockCursorButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_smartLockCursorButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_enableChatterButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_disableChatterButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_rightClickTargetingConfirm.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_rightClickTargetingCancel.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_showGlobalChatButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_hideGlobalChatButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_showAllChatButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_hideAllChatButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_enableProfanityFilterButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_disableProfanityFilterButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_enableAutoJoinDiscordButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_disableAutoJoinDiscordButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_groupGameModeVoiceChatButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_teamGameModeVoiceChatButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_hideTutorialVideosButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_showTutorialVideosButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_allowCancelActionWhileConfirmedButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        m_disallowCancelActionWhileConfirmedButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
        GameEventManager.Get().AddListener(this, GameEventManager.EventType.AppStateChanged);
        base.Awake();
    }

    private void Start()
    {
        if (m_secretButton != null)
        {
            UIEventTriggerUtils.AddListener(m_secretButton.gameObject, EventTriggerType.PointerClick, OnSecretButton);
        }

        if (m_secretButtonText != null)
        {
            UIEventTriggerUtils.AddListener(
                m_secretButtonText.gameObject,
                EventTriggerType.PointerClick,
                OnSecretButton);
        }

        if (m_okButton != null)
        {
            m_okButton.spriteController.callback = OnOkButton;
        }

        if (m_applyButton != null)
        {
            m_applyButton.spriteController.callback = OnApplyButton;
        }

        if (m_revertDefaultsButton != null)
        {
            m_revertDefaultsButton.spriteController.callback = OnRevertDefaultsButton;
        }

        if (m_graphicsLowButton != null)
        {
            m_graphicsLowButton.spriteController.callback = OnGraphicsQualityLow;
            m_graphicsLowButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_graphicsMediumButton != null)
        {
            m_graphicsMediumButton.spriteController.callback = OnGraphicsQualityMedium;
            m_graphicsMediumButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_graphicsHighButton != null)
        {
            m_graphicsHighButton.spriteController.callback = OnGraphicsQualityHigh;
            m_graphicsHighButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_windowModeButton != null)
        {
            m_windowModeButton.spriteController.callback = OnWindowMode;
            m_windowModeButton.spriteController.RegisterScrollListener(OnScroll);
            UIEventTriggerUtils.AddListener(
                m_windowModeButton.spriteController.gameObject,
                EventTriggerType.Deselect,
                OnResolutionDeselect);
        }

        if (m_gameWindowModeButton != null)
        {
            m_gameWindowModeButton.spriteController.callback = OnGameWindowMode;
            m_gameWindowModeButton.spriteController.RegisterScrollListener(OnScroll);
            UIEventTriggerUtils.AddListener(
                m_gameWindowModeButton.spriteController.gameObject,
                EventTriggerType.Deselect,
                OnGameResolutionDeselect);
        }

        if (m_closeButton != null)
        {
            m_closeButton.spriteController.callback = OnCancelButton;
        }

        if (m_windowModeWindowedButton != null)
        {
            m_windowModeWindowedButton.spriteController.callback = OnWindowModeWindowed;
            m_windowModeWindowedButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_windowModeFullscreenButton != null)
        {
            m_windowModeFullscreenButton.spriteController.callback = OnWindowModeFullscreen;
            m_windowModeFullscreenButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_gameWindowModeInheritButton != null)
        {
            m_gameWindowModeInheritButton.spriteController.callback = OnGameWindowModeInherit;
            m_gameWindowModeInheritButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_gameWindowModeWindowedButton != null)
        {
            m_gameWindowModeWindowedButton.spriteController.callback = OnGameWindowModeWindowed;
            m_gameWindowModeWindowedButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_gameWindowModeFullscreenButton != null)
        {
            m_gameWindowModeFullscreenButton.spriteController.callback = OnGameWindowModeFullscreen;
            m_gameWindowModeFullscreenButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_resolutionButton != null)
        {
            m_resolutionButton.spriteController.callback = OnResolution;
            m_resolutionButton.spriteController.RegisterScrollListener(OnScroll);
            UIEventTriggerUtils.AddListener(
                m_resolutionButton.spriteController.gameObject,
                EventTriggerType.Deselect,
                OnResolutionDeselect);
        }

        if (m_gameResolutionButton != null)
        {
            m_gameResolutionButton.spriteController.callback = OnGameResolution;
            m_gameResolutionButton.spriteController.RegisterScrollListener(OnScroll);
            UIEventTriggerUtils.AddListener(
                m_gameResolutionButton.spriteController.gameObject,
                EventTriggerType.Deselect,
                OnGameResolutionDeselect);
        }

        if (m_regionButton != null)
        {
            m_regionButton.spriteController.callback = OnRegion;
            m_regionButton.spriteController.RegisterScrollListener(OnScroll);
            UIEventTriggerUtils.AddListener(
                m_regionButton.spriteController.gameObject,
                EventTriggerType.Deselect,
                OnRegionDeselect);
        }

        if (m_languageButton != null)
        {
            m_languageButton.spriteController.callback = OnLanguage;
            m_languageButton.spriteController.RegisterScrollListener(OnScroll);
            UIEventTriggerUtils.AddListener(
                m_languageButton.spriteController.gameObject,
                EventTriggerType.Deselect,
                OnLanguageDeselect);
        }

        if (m_regionNorthAmericaButton != null)
        {
            m_regionNorthAmericaButton.spriteController.callback = OnRegionNorthAmerica;
        }

        if (m_regionEuropeButton != null)
        {
            m_regionEuropeButton.spriteController.callback = OnRegionEurope;
        }

        if (m_masterVolumeSlider != null)
        {
            m_masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeSliderChange);
        }

        if (m_musicVolumeSlider != null)
        {
            m_musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderChange);
        }

        if (m_lockCursorButton != null)
        {
            m_lockCursorButton.spriteController.callback = OnLockCursor;
            m_lockCursorButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_unlockCursorButton != null)
        {
            m_unlockCursorButton.spriteController.callback = OnUnlockCursor;
            m_unlockCursorButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_smartLockCursorButton != null)
        {
            m_smartLockCursorButton.spriteController.callback = OnSmartLockCursor;
            m_smartLockCursorButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_enableChatterButton != null)
        {
            m_enableChatterButton.spriteController.callback = OnEnableChatter;
            m_enableChatterButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_disableChatterButton != null)
        {
            m_disableChatterButton.spriteController.callback = OnDisableChatter;
            m_disableChatterButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_rightClickTargetingConfirm != null)
        {
            m_rightClickTargetingConfirm.spriteController.callback = OnRightClickConfirm;
            m_rightClickTargetingConfirm.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_rightClickTargetingCancel != null)
        {
            m_rightClickTargetingCancel.spriteController.callback = OnRightClickCancel;
            m_rightClickTargetingCancel.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_shiftClickForWaypoints != null)
        {
            m_shiftClickForWaypoints.spriteController.callback = OnShiftClickWaypointsConfirm;
            m_shiftClickForWaypoints.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_shiftClickForNewPath != null)
        {
            m_shiftClickForNewPath.spriteController.callback = OnShiftClickWaypointsCancel;
            m_shiftClickForNewPath.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_showGlobalChatButton != null)
        {
            m_showGlobalChatButton.spriteController.callback = OnShowGlobalChat;
            m_showGlobalChatButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_hideGlobalChatButton != null)
        {
            m_hideGlobalChatButton.spriteController.callback = OnHideGlobalChat;
            m_hideGlobalChatButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_showAllChatButton != null)
        {
            m_showAllChatButton.spriteController.callback = OnShowAllChat;
            m_showAllChatButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_hideAllChatButton != null)
        {
            m_hideAllChatButton.spriteController.callback = OnHideAllChat;
            m_hideAllChatButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_enableProfanityFilterButton != null)
        {
            m_enableProfanityFilterButton.spriteController.callback = OnEnableProfanityFilter;
            m_enableProfanityFilterButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_disableProfanityFilterButton != null)
        {
            m_disableProfanityFilterButton.spriteController.callback = OnDisableProfanityFilter;
            m_disableProfanityFilterButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_enableAutoJoinDiscordButton != null)
        {
            m_enableAutoJoinDiscordButton.spriteController.callback = OnEnableAutoJoinDiscord;
            m_enableAutoJoinDiscordButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_disableAutoJoinDiscordButton != null)
        {
            m_disableAutoJoinDiscordButton.spriteController.callback = OnDisableAutoJoinDiscord;
            m_disableAutoJoinDiscordButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_groupGameModeVoiceChatButton != null)
        {
            m_groupGameModeVoiceChatButton.spriteController.callback = OnGroupGameModeVoiceChat;
            m_groupGameModeVoiceChatButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_teamGameModeVoiceChatButton != null)
        {
            m_teamGameModeVoiceChatButton.spriteController.callback = OnTeamGameModeVoiceChat;
            m_teamGameModeVoiceChatButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_hideTutorialVideosButton != null)
        {
            m_hideTutorialVideosButton.spriteController.callback = OnHideTutorialVideos;
            m_hideTutorialVideosButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_showTutorialVideosButton != null)
        {
            m_showTutorialVideosButton.spriteController.callback = OnShowTutorialVideos;
            m_showTutorialVideosButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_allowCancelActionWhileConfirmedButton != null)
        {
            m_allowCancelActionWhileConfirmedButton.spriteController.callback = OnAllowCancelActionWhileConfirmed;
            m_allowCancelActionWhileConfirmedButton.spriteController.RegisterScrollListener(OnScroll);
        }

        if (m_disallowCancelActionWhileConfirmedButton != null)
        {
            m_disallowCancelActionWhileConfirmedButton.spriteController.callback = OnDisallowCancelActionWhileConfirmed;
            m_disallowCancelActionWhileConfirmedButton.spriteController.RegisterScrollListener(OnScroll);
        }
        
#if EVOS        
        // custom
        AddCustomButtons();
#endif

        m_activeState = new SettingsState();
        m_pendingState = new SettingsState();
        TrySetupHwnd();
        if (!PlayerPrefs.HasKey("OptionsInitialized"))
        {
            PlayerPrefs.SetInt("OptionsInitialized", 1);
            m_activeState.InitToDefaults();
            m_activeState.ApplyPendingValues(null);
            m_activeState.ApplyToPlayerPrefs();
        }
        else
        {
            m_activeState.InitFromPlayerPrefs();
            m_activeState.ApplyPendingValues(null);
        }

        m_pendingState = (SettingsState)m_activeState.Clone();
        SendNotifyOptions(false);
    }

#if EVOS
    // custom options
    private Transform m_toggleButtonSource;
    private RectTransform m_content;
    private static readonly string[] c_rowNames = {
        "graphicsQualityGridLayout",
        "menuModeResolutionContainer",
        "gameModeResolutionContainer",
        "regionContainer",
        "languageContainer",
        "masterVolumeContainer",
        "musicVolumeContainer",
        "lockCursorGridLayout",
        "freelancerChatterGridLayout",
        "rightClickGridLayout",
        "shiftClickWaypointGridLayout",
        "cancelWhileConfirmed",
        "showGlobalChat",
        "showAllChat",
        "enableProfanityFilter",
        "tutorialVideos"
    };
    private static readonly string[] c_dropDownNames = {
        "windowModeList",
        "resolutionList",
        "gameWindowModeList",
        "gameResolutionList",
        "regionList",
        "languageList"
    };
    private List<RectTransform> m_rows;
    private const int c_rowHeight = 52;
    private const int c_vertStartOffset = -24;
    
    // custom
    private void AddCustomButtons()
    {
        m_content = m_container
            .Find("Options")
            ?.Find("Scroll View")
            ?.Find("Content") as RectTransform;
        Transform rows = m_content
            ?.Find("optionsGridLayout");

        if (rows == null)
        {
            Log.Error("Failed to hack into options menu");
            return;
        }
        
        int ui = LayerMask.NameToLayer("UI");
        
        m_rows = new List<RectTransform>(c_rowNames.Length);
        foreach (string rowName in c_rowNames)
        {
            RectTransform row = rows.Find(rowName) as RectTransform;
            if (row == null)
            {
                Log.Error($"Failed to hack into options menu: row {rowName} not found");
                return;
            }

            GameObject rowWrapper = new GameObject(rowName + "Row")
            {
                layer = ui,
                transform =
                {
                    localScale = Vector3.one
                }
            };
            
            GridLayoutGroup rowLayout = rowWrapper.AddComponent<GridLayoutGroup>();
            rowLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            rowLayout.constraintCount = 2;
            rowLayout.startAxis = GridLayoutGroup.Axis.Vertical;
            rowLayout.cellSize = new Vector2(440, c_rowHeight);
            rowLayout.childAlignment = TextAnchor.MiddleLeft;
            
            row.transform.SetParent(rowLayout.transform);
            row.transform.localScale = Vector3.one;
            
            rowWrapper.transform.SetParent(rows.transform);
            
            m_rows.Add(rowLayout.transform as RectTransform);
        }

        // add a label to each control
        List<string> rowNames = StringUtil.TR("OptionLabelsDiscord", "Options").Split('\n').ToList();
        for (int i = 0; i < m_rows.Count; i++)
        {
            var row = m_rows[i];
            GameObject labelWrapperObject = new GameObject("labelWrapper", typeof(RectTransform))
            {
                layer = ui,
                transform =
                {
                    localPosition = new Vector3(0, 0, 100),
                    localScale = Vector3.one,
                }
            };
            RectTransform labelWrapper = labelWrapperObject.GetComponent<RectTransform>();
            labelWrapper.SetParent(row);
            labelWrapper.SetAsFirstSibling();
            var label = Instantiate(m_optionsLabelText, labelWrapper);
            label.transform.localScale = Vector3.one;
            var rectTransform = label.transform as RectTransform;
            if (rectTransform)
            {
                rectTransform.anchoredPosition = new Vector2(50, -496);
            }
            label.gameObject.name = "label";
            label.text = rowNames[i];
            
        }
        
        // remove the old label
        m_optionsLabelText.gameObject.SetActive(false);
        m_optionsLabelText.gameObject.transform.SetParent(null);

        // create custom controls
        m_toggleButtonSource = m_rows[c_rowNames.Length - 1];
        foreach (EvosOptions.Option option in EvosOptions.Get().m_options)
        {
            AddCustomToggle(
                option.termTitle,
                option.gameObjectName,
                out var btnEnable,
                out var btnDisable,
                delegate
                {
                    option.stateSetter(m_pendingState, true);
                    option.UpdateButtons(true);
                },
                delegate
                {
                    option.stateSetter(m_pendingState, false);
                    option.UpdateButtons(false);
                },
                option.position,
                option.termEnable,
                option.termDisable);
            option.AssignButtons(btnEnable, btnDisable);
        }

        // dropdowns should render on the top
        foreach (string dropDownName in c_dropDownNames)
        {
            RectTransform dropDown = rows.Find(dropDownName) as RectTransform;
            if (dropDown != null)
            {
                dropDown.SetAsLastSibling();
            }
        }
        
        // arrange rows
        for (int i = 0; i < m_rows.Count; i++)
        {
            RectTransform elem = m_rows[i];
            elem.anchorMin = new Vector2(0, 1);
            elem.anchorMax = new Vector2(0, 1);
            elem.localScale = Vector3.one;
            Vector2 pos = elem.anchoredPosition;
            pos.y = c_vertStartOffset - c_rowHeight * i;
            elem.anchoredPosition = pos;
        }
    }

    private void AddCustomToggle(
        string termLabel,
        string key,
        out _SelectableBtn btnEnable,
        out _SelectableBtn btnDisable,
        _ButtonSwapSprite.ButtonClickCallback onEnable,
        _ButtonSwapSprite.ButtonClickCallback onDisable,
        int position = -1,
        string termEnable = "On@Global",
        string termDisable = "Off@Global")
    {
        if (m_toggleButtonSource == null || m_content == null)
        {
            btnEnable = null;
            btnDisable = null;
            return;
        }
        
        GameObject myCustomButton = Instantiate(m_toggleButtonSource.gameObject, m_toggleButtonSource.parent);
        myCustomButton.gameObject.name = key;

        RectTransform btnTransform = myCustomButton.transform as RectTransform;
        if (btnTransform != null)
        {
            if (position == -1)
            {
                position = m_rows.Count;
            }
            
            m_rows.Insert(position, btnTransform);
            for (int i = position; i < m_rows.Count; i++)
            {
                RectTransform elem = m_rows[i];
                Vector2 pos = elem.anchoredPosition;
                pos.y = c_vertStartOffset - c_rowHeight * i;
                elem.anchoredPosition = pos;
            }
        }

        btnEnable = myCustomButton.FindInChildren("enableBtn")?.GetComponent<_SelectableBtn>();
        btnDisable = myCustomButton.FindInChildren("disableBtn")?.GetComponent<_SelectableBtn>();
        var label = myCustomButton.FindInChildren("label")?.GetComponent<TextMeshProUGUI>();
        
        if (btnEnable != null && btnDisable != null)
        {
            btnEnable.spriteController.callback = onEnable;
            btnEnable.spriteController.RegisterScrollListener(OnScroll);
            foreach (Localize loc in btnEnable.gameObject.GetComponentsInChildren<Localize>())
            {
                loc.Term = termEnable;
                loc.SecondaryTerm = termEnable;
            }
            
            btnDisable.spriteController.callback = onDisable;
            btnDisable.spriteController.RegisterScrollListener(OnScroll);
            foreach (Localize loc in btnDisable.gameObject.GetComponentsInChildren<Localize>())
            {
                loc.Term = termDisable;
                loc.SecondaryTerm = termDisable;
            }

            if (label)
            {
                label.text = StringUtil.TR(termLabel);
            }
        }
        
        m_content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, c_rowHeight * m_rows.Count);
    }

    public bool GetOption(EvosOptions.StateGetter getter)
    {
        return getter(m_activeState);
    }
#endif
    
    private void OnDestroy()
    {
        GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.AppStateChanged);
        s_instance = null;
    }

    private void OnScroll(BaseEventData data)
    {
        m_scrollRect.OnScroll((PointerEventData)data);
    }

    public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
    {
        if (eventType == GameEventManager.EventType.AppStateChanged)
        {
            m_activeState.UpdateGameResolution();
        }
    }

    public void TrySetupHwnd()
    {
        IntPtr windowPtr = WinUtils.User32.GetActiveWindow();
        if (windowPtr == (IntPtr)0 && m_firstSetupTry)
        {
            m_firstSetupTry = false;
            List<IntPtr> windows = (List<IntPtr>)FindWindowsWithExactText("Atlas Reactor");
            if (windows.Count == 1)
            {
                windowPtr = windows[0];
            }
        }

        if (windowPtr != (IntPtr)0)
        {
            s_hwnd = windowPtr;
            Get().m_pauseUpdate = false;
            if (WinUtils.User32.GetWindowRect(s_hwnd, out WinUtils.User32.RECT windowRect)
                && WinUtils.User32.GetClientRect(s_hwnd, out WinUtils.User32.RECT clientRect))
            {
                int extraWidth = windowRect.Right - windowRect.Left - (clientRect.Right - clientRect.Left);
                int extraHeight = windowRect.Bottom - windowRect.Top - (clientRect.Bottom - clientRect.Top);
                m_borderWidth = extraWidth / 2;
                m_captionHeight = extraHeight - m_borderWidth * 2;
            }
        }
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

    public static string GetWindowText(IntPtr hWnd)
    {
        int windowTextLength = GetWindowTextLength(hWnd);
        if (windowTextLength++ > 0)
        {
            StringBuilder stringBuilder = new StringBuilder(windowTextLength);
            GetWindowText(hWnd, stringBuilder, stringBuilder.Capacity);
            return stringBuilder.ToString();
        }

        return string.Empty;
    }

    public static IEnumerable<IntPtr> FindWindowsWithExactText(string titleText)
    {
        List<IntPtr> windows = new List<IntPtr>();
        EnumWindows(
            delegate(IntPtr wnd, IntPtr param)
            {
                if (GetWindowText(wnd) == titleText)
                {
                    windows.Add(wnd);
                }

                return true;
            },
            IntPtr.Zero);
        return windows;
    }

    private void Update()
    {
        if (m_pauseUpdate || Application.isEditor)
        {
            return;
        }

        m_activeState.UpdateResolutionFromScreen();
        if ((m_activeState.lockCursorMode == SettingsState.LockCursorMode.On
             || (m_activeState.lockCursorMode == SettingsState.LockCursorMode.Smart && GameFlowData.Get() != null))
            && Cursor.lockState != CursorLockMode.Confined)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        if ((m_activeState.lockCursorMode == SettingsState.LockCursorMode.Off
             || (m_activeState.lockCursorMode == SettingsState.LockCursorMode.Smart
                 && GameFlowData.Get() == null)) && Cursor.lockState != CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnSecretButton(BaseEventData data)
    {
        m_secretButtonClicked = !m_secretButtonClicked;
    }

    private void OnOkButton(BaseEventData data)
    {
        UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
        ApplyCurrentSettings();
        HideOptions();
    }

    private void OnApplyButton(BaseEventData data)
    {
        UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
        ApplyCurrentSettings();
    }

    private void OnRevertDefaultsButton(BaseEventData data)
    {
        UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
        if (m_pendingState.overrideGlyphLanguage)
        {
            UIManager.SetGameObjectActive(m_restartWarning, true);
        }

        m_pendingState.InitToDefaults();
        m_pendingState.ApplyToOptionsUI();
        ApplyCurrentSettings();
    }

    private void OnCancelButton(BaseEventData data)
    {
        UIFrontEnd.PlaySound(FrontEndButtonSounds.Back);
        HideOptions();
        m_activeState.RevertVolume();
    }

    public void ToggleOptions()
    {
        if (!m_container.gameObject.activeSelf)
        {
            ShowOptions();
        }
        else
        {
            HideOptions();
        }
    }

    public void ShowOptions()
    {
        if (Screen.fullScreen)
        {
            m_activeState.windowMode = SettingsState.WindowMode.Fullscreen;
        }
        else if (m_activeState.windowMode == SettingsState.WindowMode.Fullscreen)
        {
            m_activeState.windowMode = SettingsState.WindowMode.Windowed;
        }

        if (m_activeState.windowMode == SettingsState.WindowMode.Fullscreen)
        {
            m_activeState.resolutionWidth = Screen.currentResolution.width;
            m_activeState.resolutionHeight = Screen.currentResolution.height;
        }
        else
        {
            m_activeState.resolutionWidth = Screen.width;
            m_activeState.resolutionHeight = Screen.height;
        }

        m_optionsLabelText.text = StringUtil.TR("OptionLabelsDiscord", "Options");
        
        if (DiscordClientInterface.IsEnabled
            && (DiscordClientInterface.IsSdkEnabled || DiscordClientInterface.IsInstalled))
        {
            SetDisabled(m_enableAutoJoinDiscordButton, false);
            SetDisabled(m_disableAutoJoinDiscordButton, false);
            SetDisabled(m_groupGameModeVoiceChatButton, false);
            SetDisabled(m_teamGameModeVoiceChatButton, false);
        }
        else
        {
            SetDisabled(m_enableAutoJoinDiscordButton, true);
            SetDisabled(m_disableAutoJoinDiscordButton, true);
            SetDisabled(m_groupGameModeVoiceChatButton, true);
            SetDisabled(m_teamGameModeVoiceChatButton, true);
        }

        m_pendingState = (SettingsState)m_activeState.Clone();
        m_pendingState.ApplyToOptionsUI();
        UIManager.SetGameObjectActive(m_container, true);
    }

    private void SetDisabled(_SelectableBtn button, bool isDisabled)
    {
        button.m_ignoreDefaultAnimationCall = isDisabled;
        button.m_ignoreHoverAnimationCall = isDisabled;
        button.m_ignorePressAnimationCall = isDisabled;
        button.spriteController.SetClickable(!isDisabled);
    }

    public void HideOptions()
    {
        if (!m_container.gameObject.activeSelf)
        {
            return;
        }

        UIManager.SetGameObjectActive(m_container, false);
        UIManager.SetGameObjectActive(m_restartWarning, false);
        SendNotifyOptions(true);
    }

    private void SendNotifyOptions(bool userDialog)
    {
        if (m_activeState == null || ClientGameManager.Get() == null)
        {
            return;
        }

        ClientGameManager.Get().NotifyOptions(
#if EVOS
            new EvosOptionsNotification
#else
            new OptionsNotification
#endif
            {
                UserDialog = userDialog,
                DeviceIdentifier = SystemInfo.deviceUniqueIdentifier,
                GraphicsQuality = (byte)m_activeState.graphicsQuality,
                WindowMode = (byte)m_activeState.windowMode,
                ResolutionWidth = (short)m_activeState.resolutionWidth,
                ResolutionHeight = (short)m_activeState.resolutionHeight,
                GameWindowMode = (byte)m_activeState.gameWindowMode,
                GameResolutionWidth = (short)m_activeState.gameResolutionWidth,
                GameResolutionHeight = (short)m_activeState.gameResolutionHeight,
                LockWindowSize = m_activeState.lockWindowSize,
                MasterVolume = (byte)m_activeState.masterVolume,
                MusicVolume = (byte)m_activeState.musicVolume,
                AmbianceVolume = (byte)m_activeState.ambianceVolume,
                LockCursorMode = (byte)m_activeState.lockCursorMode,
                EnableChatter = m_activeState.enableChatter,
                RightClickingConfirmsAbilityTargets = m_activeState.rightClickingConfirmsAbilityTargets,
                ShiftClickForMovementWaypoints = m_activeState.shiftClickForMovementWaypoints,
                ShowGlobalChat = m_activeState.showGlobalChat,
                ShowAllChat = m_activeState.showAllChat,
                EnableProfanityFilter = m_activeState.enableProfanityFilter,
                VoiceMute = m_activeState.voiceMute,
                VoiceVolume = m_activeState.voiceVolume,
                MicVolume = m_activeState.micVolume,
                VoicePushToTalk = m_activeState.voicePushToTalk,
                AutoJoinDiscord = m_activeState.autoJoinDiscord,
                GameModeVoiceChat = (byte)m_activeState.gameModeVoiceChat,
                HideTutorialVideos = m_activeState.hideTutorialVideos,
                AllowCancelActionWhileConfirmed = m_activeState.allowCancelActionWhileConfirmed,
                Region = m_activeState.region,
                OverrideGlyphLanguageCode = m_activeState.overrideGlyphLanguageCode,
#if EVOS
                AllowResettingWaypoints = m_activeState.allowResettingWaypoints,
                ExtendedCooldownView = m_activeState.extendedCooldownView,
                EnableGamepadControls = m_activeState.enableGamepadControls,
                EnableUniqueStatusEffectIcons = m_activeState.enableUniqueStatusEffectIcons,
#endif
            });
    }

    public bool IsVisible()
    {
        return m_container.gameObject.activeSelf;
    }

    internal void ApplyCurrentSettings()
    {
        m_activeState.ApplyPendingValues(m_pendingState);
        m_activeState.ApplyToPlayerPrefs();
    }

    public void OnMasterVolumeChange(string value)
    {
        bool isInvalid = false;
        int num = 0;
        try
        {
            num = Convert.ToInt32(value);
        }
        catch
        {
            isInvalid = true;
        }

        if (isInvalid)
        {
            return;
        }

        string text = Convert.ToString(num);
        if (text == value
            && num >= 0
            && num <= 100)
        {
            m_pendingState.masterVolume = num;
            m_masterVolumeLabel.text = text;
            if (m_masterVolumeSlider.value != num / 100f && !m_dontUpdateSliders)
            {
                m_masterVolumeSlider.value = num / 100f;
            }

            AudioMixer audioMixer = AudioManager.mixSnapshotManager.snapshot_game.audioMixer;
            audioMixer.SetFloat("VolMaster", m_pendingState.ConvertPercentToDb(m_pendingState.masterVolume));
        }
    }

    private void OnApplicationFocus(bool focusStatus)
    {
        if (Application.isEditor
            || AudioManager.mixSnapshotManager == null
            || AudioManager.mixSnapshotManager.snapshot_game == null
            || AudioManager.mixSnapshotManager.snapshot_game.audioMixer == null)
        {
            return;
        }

        AudioMixer audioMixer = AudioManager.mixSnapshotManager.snapshot_game.audioMixer;
        if (focusStatus)
        {
            audioMixer.SetFloat("VolMusic", m_activeState.ConvertPercentToDb(m_activeState.musicVolume));
            audioMixer.SetFloat("VolUIAmbiance", m_activeState.ConvertPercentToDb(100 - m_activeState.musicVolume));
            audioMixer.SetFloat("VolGameAmbiance", 0f);
        }
        else
        {
            audioMixer.SetFloat("VolMusic", -80f);
            audioMixer.SetFloat("VolUIAmbiance", -80f);
            audioMixer.SetFloat("VolGameAmbiance", -80f);
        }
    }

    public void OnMasterVolumeSliderChange(float value)
    {
        int num = Mathf.RoundToInt(value * 100f);
        m_dontUpdateSliders = true;
        OnMasterVolumeChange(num.ToString());
        m_dontUpdateSliders = false;
    }

    public void OnMusicVolumeChange(string value)
    {
        bool isInvalid = false;
        int num = 0;
        try
        {
            num = Convert.ToInt32(value);
        }
        catch
        {
            isInvalid = true;
        }

        if (isInvalid)
        {
            return;
        }

        string text = Convert.ToString(num);
        if (text == value
            && num >= 0
            && num <= 100)
        {
            m_pendingState.musicVolume = num;
            m_musicVolumeLabel.text = text;
            if (m_musicVolumeSlider.value != num / 100f && !m_dontUpdateSliders)
            {
                m_musicVolumeSlider.value = num / 100f;
            }

            AudioMixer audioMixer = AudioManager.mixSnapshotManager.snapshot_game.audioMixer;
            audioMixer.SetFloat("VolMusic", m_pendingState.ConvertPercentToDb(m_pendingState.musicVolume));
            audioMixer.SetFloat("VolUIAmbiance", m_pendingState.ConvertPercentToDb(100 - m_pendingState.musicVolume));
        }
    }

    public void OnMusicVolumeSliderChange(float value)
    {
        int num = Mathf.RoundToInt(value * 100f);
        m_dontUpdateSliders = true;
        OnMusicVolumeChange(num.ToString());
        m_dontUpdateSliders = false;
    }

    public void OnGraphicsQualityLow(BaseEventData data)
    {
        m_pendingState.graphicsQuality = GraphicsQuality.Low;
        UpdateGraphicsQualityButtons(GraphicsQuality.Low);
    }

    public void OnGraphicsQualityMedium(BaseEventData data)
    {
        m_pendingState.graphicsQuality = GraphicsQuality.Medium;
        UpdateGraphicsQualityButtons(GraphicsQuality.Medium);
    }

    public void OnGraphicsQualityHigh(BaseEventData data)
    {
        m_pendingState.graphicsQuality = GraphicsQuality.High;
        UpdateGraphicsQualityButtons(GraphicsQuality.High);
    }
    
    // custom, inlined in vanilla
    public void UpdateGraphicsQualityButtons(GraphicsQuality quality)
    {
        bool isLow = quality == GraphicsQuality.Low || quality == GraphicsQuality.VeryLow;
        bool isMedium = quality == GraphicsQuality.Medium;
        bool isHigh = quality == GraphicsQuality.High;
        
        m_graphicsLowButton.SetSelected(isLow);
        m_graphicsMediumButton.SetSelected(isMedium);
        m_graphicsHighButton.SetSelected(isHigh);
    }

    public void OnWindowMode(BaseEventData data)
    {
        UIManager.SetGameObjectActive(m_windowModeDropdown, !m_windowModeDropdown.activeSelf);
    }

    public void OnWindowModeDeselect(BaseEventData data)
    {
        PointerEventData pointerEventData = (PointerEventData)data;
        if (!RectTransformUtility.RectangleContainsScreenPoint(
                m_windowModeDropdown.transform as RectTransform,
                pointerEventData.position,
                pointerEventData.pressEventCamera)
            && !RectTransformUtility.RectangleContainsScreenPoint(
                m_windowModeButton.transform as RectTransform,
                pointerEventData.position,
                pointerEventData.pressEventCamera))
        {
            UIManager.SetGameObjectActive(m_windowModeDropdown, false);
        }
    }

    public void SetResolutionText(string text)
    {
        foreach (TextMeshProUGUI txt in m_resolutionText)
        {
            txt.text = text;
        }
    }

    public void SetWindowModeText(string text)
    {
        foreach (TextMeshProUGUI txt in m_windowModeText)
        {
            txt.text = text;
        }
    }

    public void OnWindowModeWindowed(BaseEventData data)
    {
        m_pendingState.windowMode = SettingsState.WindowMode.Windowed;
        SetWindowModeText(StringUtil.TR("Windowed", "Options"));
        UIManager.SetGameObjectActive(m_windowModeDropdown, false);
        bool isStandard = false;
        foreach (Resolution resolution in Screen.resolutions)
        {
            if (resolution.width == m_pendingState.resolutionWidth
                && resolution.height == m_pendingState.resolutionHeight)
            {
                isStandard = true;
            }
        }

        if (isStandard)
        {
            Get().SetResolutionText(m_pendingState.resolutionWidth + " x " + m_pendingState.resolutionHeight);
        }
        else
        {
            Get().SetResolutionText(StringUtil.TR("Custom", "Options"));
        }
    }

    public void OnWindowModeFullscreen(BaseEventData data)
    {
        m_pendingState.windowMode = SettingsState.WindowMode.Fullscreen;
        SetWindowModeText(StringUtil.TR("Fullscreen", "Options"));
        UIManager.SetGameObjectActive(m_windowModeDropdown, false);
        m_pendingState.resolutionWidth = Screen.currentResolution.width;
        m_pendingState.resolutionHeight = Screen.currentResolution.height;
        SetResolutionText(m_pendingState.resolutionWidth + " x " + m_pendingState.resolutionHeight);
    }

    public void OnResolution(BaseEventData data)
    {
        if (!m_resolutionDropdown.activeSelf)
        {
            PopulateResolutionDropdown();
        }

        UIManager.SetGameObjectActive(m_resolutionDropdown, !m_resolutionDropdown.activeSelf);
    }

    public void OnResolutionDeselect(BaseEventData data)
    {
        PointerEventData pointerEventData = (PointerEventData)data;
        if (!RectTransformUtility.RectangleContainsScreenPoint(
                m_ResolutionScrollView.transform as RectTransform,
                pointerEventData.position,
                pointerEventData.pressEventCamera)
            && !RectTransformUtility.RectangleContainsScreenPoint(
                m_resolutionButton.transform as RectTransform,
                pointerEventData.position,
                pointerEventData.pressEventCamera)
            && !RectTransformUtility.RectangleContainsScreenPoint(
                m_ResolutionScrollView.verticalScrollbar.transform as RectTransform,
                pointerEventData.position,
                pointerEventData.pressEventCamera))
        {
            UIManager.SetGameObjectActive(m_resolutionDropdown, false);
        }
    }

    public void OnGameWindowMode(BaseEventData data)
    {
        UIManager.SetGameObjectActive(m_gameWindowModeDropdown, !m_gameWindowModeDropdown.activeSelf);
    }

    public void OnGameWindowModeDeselect(BaseEventData data)
    {
        PointerEventData pointerEventData = (PointerEventData)data;
        if (!RectTransformUtility.RectangleContainsScreenPoint(
                m_gameWindowModeDropdown.transform as RectTransform,
                pointerEventData.position,
                pointerEventData.pressEventCamera)
            && !RectTransformUtility.RectangleContainsScreenPoint(
                m_gameWindowModeButton.transform as RectTransform,
                pointerEventData.position,
                pointerEventData.pressEventCamera))
        {
            UIManager.SetGameObjectActive(m_gameWindowModeDropdown, false);
        }
    }

    public void SetGameResolutionText(string text)
    {
        foreach (TextMeshProUGUI txt in m_gameResolutionText)
        {
            txt.text = text;
        }
    }

    public void SetGameWindowModeText(string text)
    {
        foreach (TextMeshProUGUI txt in m_gameWindowModeText)
        {
            txt.text = text;
        }
    }

    public void OnGameWindowModeInherit(BaseEventData data)
    {
        m_pendingState.gameWindowMode = SettingsState.WindowMode.Inherit;
        SetGameWindowModeText(StringUtil.TR("Same as Menu", "Options"));
        UIManager.SetGameObjectActive(m_gameWindowModeDropdown, false);
        m_pendingState.gameResolutionWidth = -1;
        m_pendingState.gameResolutionHeight = -1;
        Get().SetGameResolutionText(StringUtil.TR("Same as Menu", "Options"));
    }

    public void OnGameWindowModeWindowed(BaseEventData data)
    {
        m_pendingState.gameWindowMode = SettingsState.WindowMode.Windowed;
        SetGameWindowModeText(StringUtil.TR("Windowed", "Options"));
        UIManager.SetGameObjectActive(m_gameWindowModeDropdown, false);
        bool isStandard = false;
        foreach (Resolution resolution in Screen.resolutions)
        {
            if (resolution.width == m_pendingState.gameResolutionWidth
                && resolution.height == m_pendingState.gameResolutionHeight)
            {
                isStandard = true;
            }
        }

        if (isStandard)
        {
            Get().SetGameResolutionText(
                m_pendingState.gameResolutionWidth + " x " + m_pendingState.gameResolutionHeight);
        }
        else
        {
            Get().SetGameResolutionText(StringUtil.TR("Custom", "Options"));
        }
    }

    public void OnGameWindowModeFullscreen(BaseEventData data)
    {
        m_pendingState.gameWindowMode = SettingsState.WindowMode.Fullscreen;
        SetGameWindowModeText(StringUtil.TR("Fullscreen", "Options"));
        UIManager.SetGameObjectActive(m_gameWindowModeDropdown, false);
        m_pendingState.gameResolutionWidth = Screen.currentResolution.width;
        m_pendingState.gameResolutionHeight = Screen.currentResolution.height;
        SetGameResolutionText(m_pendingState.gameResolutionWidth + " x " + m_pendingState.gameResolutionHeight);
    }

    public void OnGameResolution(BaseEventData data)
    {
        if (!m_gameResolutionDropdown.activeSelf)
        {
            PopulateGameResolutionDropdown();
        }

        UIManager.SetGameObjectActive(m_gameResolutionDropdown, !m_gameResolutionDropdown.activeSelf);
    }

    public void OnGameResolutionDeselect(BaseEventData data)
    {
        PointerEventData pointerEventData = (PointerEventData)data;
        if (!RectTransformUtility.RectangleContainsScreenPoint(
                m_gameResolutionScrollView.transform as RectTransform,
                pointerEventData.position,
                pointerEventData.pressEventCamera)
            && !RectTransformUtility.RectangleContainsScreenPoint(
                m_gameResolutionButton.transform as RectTransform,
                pointerEventData.position,
                pointerEventData.pressEventCamera)
            && !RectTransformUtility.RectangleContainsScreenPoint(
                m_gameResolutionScrollView.verticalScrollbar.transform as RectTransform,
                pointerEventData.position,
                pointerEventData.pressEventCamera))
        {
            UIManager.SetGameObjectActive(m_gameResolutionDropdown, false);
        }
    }

    public void OnRegion(BaseEventData data)
    {
        UIManager.SetGameObjectActive(m_regionDropdown, !m_regionDropdown.activeSelf);
    }

    public void OnRegionDeselect(BaseEventData data)
    {
        PointerEventData pointerEventData = (PointerEventData)data;
        if (!RectTransformUtility.RectangleContainsScreenPoint(
                m_regionDropdown.transform as RectTransform,
                pointerEventData.position,
                pointerEventData.pressEventCamera)
            && !RectTransformUtility.RectangleContainsScreenPoint(
                m_regionButton.transform as RectTransform,
                pointerEventData.position,
                pointerEventData.pressEventCamera))
        {
            UIManager.SetGameObjectActive(m_regionDropdown, false);
        }
    }

    public void SetRegionText(string text)
    {
        foreach (TextMeshProUGUI txt in m_regionText)
        {
            txt.text = text;
        }
    }
    
    // custom, inlined in vanilla
    public void SetRegionText(Region region)
    {
        switch (region)
        {
            case Region.US:
                SetRegionText(StringUtil.TR("NorthAmerica", "Options"));
                break;
            case Region.EU:
                SetRegionText(StringUtil.TR("Europe", "Options"));
                break;
        }
    }

    public void OnLanguage(BaseEventData data)
    {
        if (!m_languageDropdown.activeSelf)
        {
            PopulateLanguageDropdown();
        }

        UIManager.SetGameObjectActive(m_languageDropdown, !m_languageDropdown.activeSelf);
    }

    public void OnLanguageDeselect(BaseEventData data)
    {
        PointerEventData pointerEventData = (PointerEventData)data;
        if (!RectTransformUtility.RectangleContainsScreenPoint(
                m_LanguageScrollView.transform as RectTransform,
                pointerEventData.position,
                pointerEventData.pressEventCamera)
            && !RectTransformUtility.RectangleContainsScreenPoint(
                m_languageButton.transform as RectTransform,
                pointerEventData.position,
                pointerEventData.pressEventCamera)
            && !RectTransformUtility.RectangleContainsScreenPoint(
                m_LanguageScrollView.verticalScrollbar.transform as RectTransform,
                pointerEventData.position,
                pointerEventData.pressEventCamera))
        {
            UIManager.SetGameObjectActive(m_languageDropdown, false);
        }
    }

    public void SetLanguageText(string text)
    {
        foreach (TextMeshProUGUI txt in m_languageText)
        {
            txt.text = text;
        }
    }

    public void OnRegionNorthAmerica(BaseEventData data)
    {
        m_pendingState.region = Region.US;
        SetRegionText(Region.US);
        UIManager.SetGameObjectActive(m_regionDropdown, false);
    }

    public void OnRegionEurope(BaseEventData data)
    {
        m_pendingState.region = Region.EU;
        SetRegionText(Region.EU);
        UIManager.SetGameObjectActive(m_regionDropdown, false);
    }

    public void OnLockCursor(BaseEventData data)
    {
        m_pendingState.lockCursorMode = SettingsState.LockCursorMode.On;
        UpdateLockCursorButtons(SettingsState.LockCursorMode.On);
    }

    public void OnUnlockCursor(BaseEventData data)
    {
        m_pendingState.lockCursorMode = SettingsState.LockCursorMode.Off;
        UpdateLockCursorButtons(SettingsState.LockCursorMode.Off);
    }

    public void OnSmartLockCursor(BaseEventData data)
    {
        m_pendingState.lockCursorMode = SettingsState.LockCursorMode.Smart;
        UpdateLockCursorButtons(SettingsState.LockCursorMode.Smart);
    }
    
    // custom, inlined in vanilla
    public void UpdateLockCursorButtons(SettingsState.LockCursorMode lockCursorMode)
    {
        bool isLock = lockCursorMode == SettingsState.LockCursorMode.On;
        bool isUnlock = lockCursorMode == SettingsState.LockCursorMode.Off;
        
        m_lockCursorButton.SetSelected(isLock);
        m_unlockCursorButton.SetSelected(isUnlock);
        m_smartLockCursorButton.SetSelected(!isLock && !isUnlock);
    }

    public void OnEnableChatter(BaseEventData data)
    {
        m_pendingState.enableChatter = true;
        UpdateChatterButtons(true);
    }

    public void OnDisableChatter(BaseEventData data)
    {
        m_pendingState.enableChatter = false;
        UpdateChatterButtons(false);
    }
    
    // custom, inlined in vanilla
    public void UpdateChatterButtons(bool isEnabled)
    {
        m_enableChatterButton.SetSelected(isEnabled);
        m_disableChatterButton.SetSelected(!isEnabled);
    }

    public void OnShowGlobalChat(BaseEventData data)
    {
        m_pendingState.showGlobalChat = true;
        UpdateShowGlobalChatButtons(true);
    }

    public void OnHideGlobalChat(BaseEventData data)
    {
        m_pendingState.showGlobalChat = false;
        UpdateShowGlobalChatButtons(false);
    }
    
    // custom, inlined in vanilla
    public void UpdateShowGlobalChatButtons(bool isEnabled)
    {
        m_showGlobalChatButton.SetSelected(isEnabled);
        m_hideGlobalChatButton.SetSelected(!isEnabled);
    }

    public void OnShowAllChat(BaseEventData data)
    {
        m_pendingState.showAllChat = true;
        UpdateShowAllChatButtons(true);
    }

    public void OnHideAllChat(BaseEventData data)
    {
        m_pendingState.showAllChat = false;
        UpdateShowAllChatButtons(false);
    }
    
    // custom, inlined in vanilla
    public void UpdateShowAllChatButtons(bool isEnabled)
    {
        m_showAllChatButton.SetSelected(isEnabled);
        m_hideAllChatButton.SetSelected(!isEnabled);
    }

    public void OnEnableProfanityFilter(BaseEventData data)
    {
        m_pendingState.enableProfanityFilter = true;
        UpdateProfanityFilterButtons(true);
    }

    public void OnDisableProfanityFilter(BaseEventData data)
    {
        m_pendingState.enableProfanityFilter = false;
        UpdateProfanityFilterButtons(false);
    }
    
    // custom, inlined in vanilla
    public void UpdateProfanityFilterButtons(bool isEnabled)
    {
        m_enableProfanityFilterButton.SetSelected(isEnabled);
        m_disableProfanityFilterButton.SetSelected(!isEnabled);
    }

    public void OnEnableAutoJoinDiscord(BaseEventData data)
    {
        m_pendingState.autoJoinDiscord = true;
        UpdateAutoJoinDiscordButtons(true);
    }

    public void OnDisableAutoJoinDiscord(BaseEventData data)
    {
        m_pendingState.autoJoinDiscord = false;
        UpdateAutoJoinDiscordButtons(false);
    }
    
    // custom, inlined in vanilla
    public void UpdateAutoJoinDiscordButtons(bool isEnabled)
    {
        m_enableAutoJoinDiscordButton.SetSelected(isEnabled);
        m_disableAutoJoinDiscordButton.SetSelected(!isEnabled);
    }

    public void OnGroupGameModeVoiceChat(BaseEventData data)
    {
        m_pendingState.gameModeVoiceChat = SettingsState.VoiceChatMode.Group;
        UpdateVoiceChatModeButtons(true);
    }

    public void OnTeamGameModeVoiceChat(BaseEventData data)
    {
        m_pendingState.gameModeVoiceChat = SettingsState.VoiceChatMode.Team;
        UpdateVoiceChatModeButtons(false);
    }
    
    // custom, inlined in vanilla
    public void UpdateVoiceChatModeButtons(bool isGame)
    {
        m_groupGameModeVoiceChatButton.SetSelected(isGame);
        m_teamGameModeVoiceChatButton.SetSelected(!isGame);
    }

    public void OnRightClickConfirm(BaseEventData data)
    {
        m_pendingState.rightClickingConfirmsAbilityTargets = true;
        UpdateRightClickButtons(true);
    }

    public void OnRightClickCancel(BaseEventData data)
    {
        m_pendingState.rightClickingConfirmsAbilityTargets = false;
        UpdateRightClickButtons(false);
    }
    
    // custom, inlined in vanilla
    public void UpdateRightClickButtons(bool isConfirm)
    {
        m_rightClickTargetingConfirm.SetSelected(isConfirm);
        m_rightClickTargetingCancel.SetSelected(!isConfirm);
    }

    public void OnShiftClickWaypointsConfirm(BaseEventData data)
    {
        m_pendingState.shiftClickForMovementWaypoints = true;
        UpdateShiftClickWaypointsButtons(true);
    }

    public void OnShiftClickWaypointsCancel(BaseEventData data)
    {
        m_pendingState.shiftClickForMovementWaypoints = false;
        UpdateShiftClickWaypointsButtons(false);
    }
    
    // custom, inlined in vanilla
    public void UpdateShiftClickWaypointsButtons(bool isWaypoints)
    {
        m_shiftClickForWaypoints.SetSelected(isWaypoints);
        m_shiftClickForNewPath.SetSelected(!isWaypoints);
    }

    public void OnHideTutorialVideos(BaseEventData data)
    {
        m_pendingState.hideTutorialVideos = true;
        UpdateTutorialVideosButtons(true);
    }

    public void OnShowTutorialVideos(BaseEventData data)
    {
        m_pendingState.hideTutorialVideos = false;
        UpdateTutorialVideosButtons(false);
    }
    
    // custom, inlined in vanilla
    public void UpdateTutorialVideosButtons(bool isHide)
    {
        m_hideTutorialVideosButton.SetSelected(isHide);
        m_showTutorialVideosButton.SetSelected(!isHide);
    }

    public void OnAllowCancelActionWhileConfirmed(BaseEventData data)
    {
        m_pendingState.allowCancelActionWhileConfirmed = true;
        UpdateAllowCancelActionWhileConfirmedButtons(true);
    }

    public void OnDisallowCancelActionWhileConfirmed(BaseEventData data)
    {
        m_pendingState.allowCancelActionWhileConfirmed = false;
        UpdateAllowCancelActionWhileConfirmedButtons(false);
    }
    
    // custom, inlined in vanilla
    public void UpdateAllowCancelActionWhileConfirmedButtons(bool isAllowed)
    {
        m_allowCancelActionWhileConfirmedButton.SetSelected(isAllowed);
        m_disallowCancelActionWhileConfirmedButton.SetSelected(!isAllowed);
    }

    private float ResolutionRound(float f, float digits)
    {
        float num = Mathf.Pow(10f, digits);
        return Mathf.Round(f * num) / num;
    }

    private void PopulateResolutionDropdown()
    {
        PopulateGeneralResolutionDropdown(
            m_ResolutionScrollView,
            m_resolutionItemContainer,
            m_pendingState.windowMode,
            OnSpecificResolution,
            m_activeState.resolutionWidth,
            m_activeState.resolutionHeight);
    }

    private void PopulateGameResolutionDropdown()
    {
        PopulateGeneralResolutionDropdown(
            m_gameResolutionScrollView,
            m_gameResolutionItemContainer,
            m_pendingState.gameWindowMode,
            OnSpecificGameResolution,
            m_activeState.resolutionWidth,
            m_activeState.resolutionHeight);
    }

    private void PopulateGeneralResolutionDropdown(
        ScrollRect scrollView,
        GridLayoutGroup itemContainer,
        SettingsState.WindowMode pendingWindowMode,
        _ButtonSwapSprite.ButtonClickCallback changeCallback,
        int currentWidth,
        int currentHeight)
    {
        foreach (GameObject resolutionButton in m_resolutionButtons)
        {
            Destroy(resolutionButton);
        }

        m_resolutionButtons.Clear();
        int i = 0;
        int y = 1;
        Resolution[] resolutions = Screen.resolutions;

        IOrderedEnumerable<Resolution> source = resolutions.OrderByDescending((r => r.width));

        bool isActiveResolutionPresent = false;

        foreach (Resolution resolution in source.ThenByDescending(r => r.height).ToArray())
        {
            GameObject resolutionButton = Instantiate(Get().m_resolutionButtonPrefab);
            resolutionButton.transform.SetParent(itemContainer.transform, false);
            y++;
            RectTransform rectTransform = resolutionButton.transform as RectTransform;
            Vector2 anchoredPosition = rectTransform.anchoredPosition;
            anchoredPosition.y = -20 + -30 * i;
            rectTransform.anchoredPosition = anchoredPosition;

            foreach (TextMeshProUGUI txt in resolutionButton.GetComponentsInChildren<TextMeshProUGUI>())
            {
                txt.text = resolution.width + " x " + resolution.height;
            }

            m_resolutionButtons.Add(resolutionButton);

            UIManager.SetGameObjectActive(resolutionButton, true);
            RegisterResolutionSetting(
                resolutionButton,
                new ResolutionSetting
                {
                    resolution = resolution,
                    custom = false
                });

            _SelectableBtn btn = resolutionButton.GetComponent<_SelectableBtn>();
            btn.spriteController.callback = changeCallback;
            btn.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
            if (scrollView != null)
            {
                _MouseEventPasser mouseEventPasser = btn.spriteController.gameObject.AddComponent<_MouseEventPasser>();
                mouseEventPasser.AddNewHandler(scrollView);
            }

            if (currentWidth == resolution.width
                && currentHeight == resolution.height)
            {
                isActiveResolutionPresent = true;
            }

            i++;
        }

        if (pendingWindowMode == SettingsState.WindowMode.Windowed
            && !isActiveResolutionPresent
            && currentWidth > 0
            && currentHeight > 0)
        {
            GameObject resolutionButton = Instantiate(Get().m_resolutionButtonPrefab);
            resolutionButton.transform.SetParent(itemContainer.transform, false);
            y++;
            RectTransform rectTransform = resolutionButton.transform as RectTransform;
            Vector2 anchoredPosition = rectTransform.anchoredPosition;
            anchoredPosition.y = -20 + -30 * i;
            rectTransform.anchoredPosition = anchoredPosition;

            foreach (TextMeshProUGUI txt in resolutionButton.GetComponentsInChildren<TextMeshProUGUI>())
            {
                txt.text = StringUtil.TR("Custom", "Options");
            }

            m_resolutionButtons.Add(resolutionButton);
            UIManager.SetGameObjectActive(resolutionButton, true);

            RegisterResolutionSetting(
                resolutionButton,
                new ResolutionSetting
                {
                    resolution = new Resolution
                    {
                        width = m_activeState.resolutionWidth,
                        height = m_activeState.resolutionHeight
                    },
                    custom = true
                });

            _SelectableBtn btn = resolutionButton.GetComponent<_SelectableBtn>();
            btn.spriteController.callback = changeCallback;
            btn.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;

            if (scrollView != null)
            {
                btn.spriteController.gameObject
                    .AddComponent<_MouseEventPasser>()
                    .AddNewHandler(scrollView);
            }

            i++;
        }

        RectTransform containerRect = itemContainer.transform as RectTransform;
        containerRect.sizeDelta = new Vector2(
            containerRect.sizeDelta.x,
            itemContainer.cellSize.y * y);
    }

    private void PopulateLanguageDropdown()
    {
        foreach (GameObject current in m_languageButtons)
        {
            Destroy(current);
        }

        m_languageButtons.Clear();

        for (int i = 0; i < (int)LanguageOptions.NumOptionLanguages; i++)
        {
            GameObject button = Instantiate(Get().m_resolutionButtonPrefab);
            button.transform.SetParent(m_languageItemContainer.transform, false);
            RectTransform rectTransform = button.transform as RectTransform;
            Vector2 anchoredPosition = rectTransform.anchoredPosition;
            anchoredPosition.y = -20 + -30 * i;
            rectTransform.anchoredPosition = anchoredPosition;

            foreach (TextMeshProUGUI txt in button.GetComponentsInChildren<TextMeshProUGUI>())
            {
                txt.text = StringUtil.TR(((LanguageOptions)i).ToString(), "LanguageSelection");
            }

            m_languageButtons.Add(button);
            UIManager.SetGameObjectActive(button, true);

            _SelectableBtn btn = button.GetComponent<_SelectableBtn>();
            btn.spriteController.callback = OnSpecificLanguage;
            btn.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
            if (m_LanguageScrollView != null)
            {
                btn.spriteController.gameObject
                    .AddComponent<_MouseEventPasser>()
                    .AddNewHandler(m_LanguageScrollView);
            }
        }

        RectTransform containerRect = m_languageItemContainer.transform as RectTransform;
        containerRect.sizeDelta = new Vector2(
            containerRect.sizeDelta.x,
            m_languageItemContainer.cellSize.y * (int)LanguageOptions.NumOptionLanguages);
    }

    private void RegisterResolutionSetting(GameObject obj, ResolutionSetting setting)
    {
        m_resolutionButtonData[obj.GetComponent<_SelectableBtn>().spriteController.gameObject] = setting;
    }

    public void OnSpecificGameResolution(BaseEventData data)
    {
        UIManager.SetGameObjectActive(m_gameResolutionDropdown, false);
        if (!m_resolutionButtonData.TryGetValue(data.selectedObject, out var resolutionSetting))
        {
            return;
        }

        m_pendingState.gameResolutionWidth = resolutionSetting.resolution.width;
        m_pendingState.gameResolutionHeight = resolutionSetting.resolution.height;
        if (resolutionSetting.custom)
        {
            SetGameResolutionText(StringUtil.TR("Custom", "Options"));
        }
        else
        {
            SetGameResolutionText(resolutionSetting.resolution.width + " x " + resolutionSetting.resolution.height);
        }
    }

    public void OnSpecificResolution(BaseEventData data)
    {
        UIManager.SetGameObjectActive(m_resolutionDropdown, false);
        if (!m_resolutionButtonData.TryGetValue(data.selectedObject, out var resolutionSetting))
        {
            return;
        }

        m_pendingState.resolutionWidth = resolutionSetting.resolution.width;
        m_pendingState.resolutionHeight = resolutionSetting.resolution.height;
        if (resolutionSetting.custom)
        {
            SetResolutionText(StringUtil.TR("Custom", "Options"));
        }
        else
        {
            SetResolutionText(resolutionSetting.resolution.width + " x " + resolutionSetting.resolution.height);
        }
    }

    public void OnSpecificLanguage(BaseEventData data)
    {
        UIManager.SetGameObjectActive(m_languageDropdown, false);
        int i = 0;
        foreach (GameObject button in m_languageButtons)
        {
            if (button.GetComponent<_SelectableBtn>().spriteController.gameObject == data.selectedObject)
            {
                LanguageOptions languageOptions = (LanguageOptions)i;
                if (languageOptions == LanguageOptions.GlyphSettings)
                {
                    m_pendingState.overrideGlyphLanguage = false;
                    m_pendingState.overrideGlyphLanguageCode = string.Empty;
                }
                else
                {
                    m_pendingState.overrideGlyphLanguage = true;
                    m_pendingState.overrideGlyphLanguageCode = languageOptions.ToString();
                }

                SetLanguageText(StringUtil.TR(languageOptions.ToString(), "LanguageSelection"));
            }

            i++;
        }

        UIManager.SetGameObjectActive(m_restartWarning, true);
    }

    public GraphicsQuality GetCurrentGraphicsQuality()
    {
        return m_activeState.graphicsQuality;
    }

    public void SetPendingGraphicsQuality(GraphicsQuality graphicsQuality)
    {
        m_pendingState.graphicsQuality = graphicsQuality;
    }

    public bool GetGraphicsQualityEverSetManually()
    {
        return PlayerPrefs.GetInt("OptionsGraphicsQualityEverSetManually") != 0;
    }

    public bool GetChatterEnabled()
    {
        return m_activeState.enableChatter;
    }

    public bool GetRightClickingConfirmsAbilityTargets()
    {
        return m_activeState.rightClickingConfirmsAbilityTargets;
    }

    public bool GetShiftClickForMovementWaypoints()
    {
        return m_activeState.shiftClickForMovementWaypoints;
    }

    public bool GetShowGlobalChat()
    {
        return m_activeState.showGlobalChat;
    }

    public void SetShowGlobalChat(bool show)
    {
        m_activeState.showGlobalChat = show;
        UpdateShowGlobalChatButtons(show);
    }

    public bool GetShowAllChat()
    {
        return m_activeState.showAllChat;
    }

    public void SetShowAllChat(bool show)
    {
        m_activeState.showAllChat = show;
        UpdateShowAllChatButtons(show);
    }

    public bool GetEnableProfanityFilter()
    {
        return m_activeState.enableProfanityFilter;
    }

    public void SetEnableProfanityFilter(bool show)
    {
        m_activeState.enableProfanityFilter = show;
        UpdateProfanityFilterButtons(show);
    }

    public bool GetEnableAutoJoinDiscord()
    {
        return m_activeState.autoJoinDiscord;
    }

    public void SetEnableAutoJoinDiscord(bool show)
    {
        m_activeState.autoJoinDiscord = show;
        UpdateAutoJoinDiscordButtons(show);
    }

    public bool GetVoicePushToTalk()
    {
        return m_activeState.voicePushToTalk;
    }

    public void SetVoicePushToTalk(bool pushToTalk)
    {
        m_activeState.voicePushToTalk = pushToTalk;
    }

    public bool GetVoiceMute()
    {
        return m_activeState.voiceMute;
    }

    public void SetVoiceMute(bool mute)
    {
        m_activeState.voiceMute = mute;
    }

    public float GetVoiceVolume()
    {
        return m_activeState.voiceVolume;
    }

    public void SetVoiceVolume(float volume)
    {
        m_activeState.voiceVolume = volume;
    }

    public float GetMicVolume()
    {
        return m_activeState.micVolume;
    }

    public void SetMicVolume(float volume)
    {
        m_activeState.micVolume = volume;
    }

    public static Region GetRegion()
    {
        return s_instance != null
            ? s_instance.m_activeState.region
            : (Region)PlayerPrefs.GetInt("Region", (int)GetDefaultRegion());
    }

    public static Region GetDefaultRegion()
    {
        DateTime utcNow = DateTime.UtcNow;
        DateTime localNow = utcNow.ToLocalTime();
        double timeZone = (localNow - utcNow).TotalHours;
        return timeZone > -2.5 && timeZone < 7.5
            ? Region.EU
            : Region.US;
    }

    public SettingsState.VoiceChatMode GetGameModeVoiceChat()
    {
        return m_activeState.gameModeVoiceChat;
    }

    public void SetGameModeVoiceChat(SettingsState.VoiceChatMode setting)
    {
        m_activeState.gameModeVoiceChat = setting;
        switch (setting)
        {
            case SettingsState.VoiceChatMode.None:
            case SettingsState.VoiceChatMode.Group:
                UpdateVoiceChatModeButtons(true);
                break;
            case SettingsState.VoiceChatMode.Team:
                UpdateVoiceChatModeButtons(false);
                break;
        }
    }

    public bool GetShowTutorialVideos()
    {
        return !m_activeState.hideTutorialVideos;
    }

    public bool ShouldCancelActionWhileConfirmed()
    {
        return m_activeState.allowCancelActionWhileConfirmed;
    }
}