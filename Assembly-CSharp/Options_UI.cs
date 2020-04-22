using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

	private Dictionary<GameObject, ResolutionSetting> m_resolutionButtonData = new Dictionary<GameObject, ResolutionSetting>();

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
		if (m_ResolutionScrollView != null)
		{
			if (m_resolutionBackgroundHitBox != null)
			{
				_MouseEventPasser mouseEventPasser = m_resolutionBackgroundHitBox.gameObject.AddComponent<_MouseEventPasser>();
				mouseEventPasser.AddNewHandler(m_ResolutionScrollView);
			}
		}
		if (m_gameResolutionScrollView != null)
		{
			if (m_gameResolutionBackgroundHitBox != null)
			{
				_MouseEventPasser mouseEventPasser2 = m_gameResolutionBackgroundHitBox.gameObject.AddComponent<_MouseEventPasser>();
				mouseEventPasser2.AddNewHandler(m_gameResolutionScrollView);
			}
		}
		if (m_LanguageScrollView != null)
		{
			if (m_languageBackgroundHitBox != null)
			{
				_MouseEventPasser mouseEventPasser3 = m_languageBackgroundHitBox.gameObject.AddComponent<_MouseEventPasser>();
				mouseEventPasser3.AddNewHandler(m_LanguageScrollView);
			}
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
			UIEventTriggerUtils.AddListener(m_secretButtonText.gameObject, EventTriggerType.PointerClick, OnSecretButton);
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
			UIEventTriggerUtils.AddListener(m_windowModeButton.spriteController.gameObject, EventTriggerType.Deselect, OnResolutionDeselect);
		}
		if (m_gameWindowModeButton != null)
		{
			m_gameWindowModeButton.spriteController.callback = OnGameWindowMode;
			m_gameWindowModeButton.spriteController.RegisterScrollListener(OnScroll);
			UIEventTriggerUtils.AddListener(m_gameWindowModeButton.spriteController.gameObject, EventTriggerType.Deselect, OnGameResolutionDeselect);
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
			UIEventTriggerUtils.AddListener(m_resolutionButton.spriteController.gameObject, EventTriggerType.Deselect, OnResolutionDeselect);
		}
		if (m_gameResolutionButton != null)
		{
			m_gameResolutionButton.spriteController.callback = OnGameResolution;
			m_gameResolutionButton.spriteController.RegisterScrollListener(OnScroll);
			UIEventTriggerUtils.AddListener(m_gameResolutionButton.spriteController.gameObject, EventTriggerType.Deselect, OnGameResolutionDeselect);
		}
		if (m_regionButton != null)
		{
			m_regionButton.spriteController.callback = OnRegion;
			m_regionButton.spriteController.RegisterScrollListener(OnScroll);
			UIEventTriggerUtils.AddListener(m_regionButton.spriteController.gameObject, EventTriggerType.Deselect, OnRegionDeselect);
		}
		if (m_languageButton != null)
		{
			m_languageButton.spriteController.callback = OnLanguage;
			m_languageButton.spriteController.RegisterScrollListener(OnScroll);
			UIEventTriggerUtils.AddListener(m_languageButton.spriteController.gameObject, EventTriggerType.Deselect, OnLanguageDeselect);
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
		IntPtr value = WinUtils.User32.GetActiveWindow();
		if (value == (IntPtr)0)
		{
			if (m_firstSetupTry)
			{
				m_firstSetupTry = false;
				List<IntPtr> list = (List<IntPtr>)FindWindowsWithExactText("Atlas Reactor");
				if (list.Count() == 1)
				{
					value = list[0];
				}
			}
		}
		if (!(value != (IntPtr)0))
		{
			return;
		}
		while (true)
		{
			s_hwnd = value;
			Get().m_pauseUpdate = false;
			if (!WinUtils.User32.GetWindowRect(s_hwnd, out WinUtils.User32.RECT rect))
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
			if (!WinUtils.User32.GetClientRect(s_hwnd, out WinUtils.User32.RECT rect2))
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
			int num = rect.Right - rect.Left - (rect2.Right - rect2.Left);
			int num2 = rect.Bottom - rect.Top - (rect2.Bottom - rect2.Top);
			m_borderWidth = num / 2;
			m_captionHeight = num2 - m_borderWidth * 2;
			return;
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
		EnumWindows(delegate(IntPtr wnd, IntPtr param)
		{
			if (GetWindowText(wnd) == titleText)
			{
				windows.Add(wnd);
			}
			return true;
		}, IntPtr.Zero);
		return windows;
	}

	private void Update()
	{
		if (m_pauseUpdate || Application.isEditor)
		{
			return;
		}
		while (true)
		{
			m_activeState.UpdateResolutionFromScreen();
			if (m_activeState.lockCursorMode == SettingsState.LockCursorMode.On)
			{
				goto IL_0074;
			}
			if (m_activeState.lockCursorMode == SettingsState.LockCursorMode.Smart)
			{
				if (GameFlowData.Get() != null)
				{
					goto IL_0074;
				}
			}
			goto IL_0084;
			IL_0084:
			if (m_activeState.lockCursorMode != 0)
			{
				if (m_activeState.lockCursorMode != SettingsState.LockCursorMode.Smart)
				{
					return;
				}
				if (!(GameFlowData.Get() == null))
				{
					return;
				}
			}
			if (Cursor.lockState != 0)
			{
				while (true)
				{
					Cursor.lockState = CursorLockMode.None;
					return;
				}
			}
			return;
			IL_0074:
			if (Cursor.lockState != CursorLockMode.Confined)
			{
				Cursor.lockState = CursorLockMode.Confined;
			}
			goto IL_0084;
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
		if (DiscordClientInterface.IsEnabled)
		{
			if (DiscordClientInterface.IsSdkEnabled || DiscordClientInterface.IsInstalled)
			{
				SetDisabled(m_enableAutoJoinDiscordButton, false);
				SetDisabled(m_disableAutoJoinDiscordButton, false);
				SetDisabled(m_groupGameModeVoiceChatButton, false);
				SetDisabled(m_teamGameModeVoiceChatButton, false);
				goto IL_0169;
			}
		}
		SetDisabled(m_enableAutoJoinDiscordButton, true);
		SetDisabled(m_disableAutoJoinDiscordButton, true);
		SetDisabled(m_groupGameModeVoiceChatButton, true);
		SetDisabled(m_teamGameModeVoiceChatButton, true);
		goto IL_0169;
		IL_0169:
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
		while (true)
		{
			UIManager.SetGameObjectActive(m_container, false);
			UIManager.SetGameObjectActive(m_restartWarning, false);
			SendNotifyOptions(true);
			return;
		}
	}

	private void SendNotifyOptions(bool userDialog)
	{
		if (m_activeState == null)
		{
			return;
		}
		while (true)
		{
			if (ClientGameManager.Get() != null)
			{
				while (true)
				{
					OptionsNotification optionsNotification = new OptionsNotification();
					optionsNotification.UserDialog = userDialog;
					optionsNotification.DeviceIdentifier = SystemInfo.deviceUniqueIdentifier;
					optionsNotification.GraphicsQuality = (byte)m_activeState.graphicsQuality;
					optionsNotification.WindowMode = (byte)m_activeState.windowMode;
					optionsNotification.ResolutionWidth = (short)m_activeState.resolutionWidth;
					optionsNotification.ResolutionHeight = (short)m_activeState.resolutionHeight;
					optionsNotification.GameWindowMode = (byte)m_activeState.gameWindowMode;
					optionsNotification.GameResolutionWidth = (short)m_activeState.gameResolutionWidth;
					optionsNotification.GameResolutionHeight = (short)m_activeState.gameResolutionHeight;
					optionsNotification.LockWindowSize = m_activeState.lockWindowSize;
					optionsNotification.MasterVolume = (byte)m_activeState.masterVolume;
					optionsNotification.MusicVolume = (byte)m_activeState.musicVolume;
					optionsNotification.AmbianceVolume = (byte)m_activeState.ambianceVolume;
					optionsNotification.LockCursorMode = (byte)m_activeState.lockCursorMode;
					optionsNotification.EnableChatter = m_activeState.enableChatter;
					optionsNotification.RightClickingConfirmsAbilityTargets = m_activeState.rightClickingConfirmsAbilityTargets;
					optionsNotification.ShiftClickForMovementWaypoints = m_activeState.shiftClickForMovementWaypoints;
					optionsNotification.ShowGlobalChat = m_activeState.showGlobalChat;
					optionsNotification.ShowAllChat = m_activeState.showAllChat;
					optionsNotification.EnableProfanityFilter = m_activeState.enableProfanityFilter;
					optionsNotification.VoiceMute = m_activeState.voiceMute;
					optionsNotification.VoiceVolume = m_activeState.voiceVolume;
					optionsNotification.MicVolume = m_activeState.micVolume;
					optionsNotification.VoicePushToTalk = m_activeState.voicePushToTalk;
					optionsNotification.AutoJoinDiscord = m_activeState.autoJoinDiscord;
					optionsNotification.GameModeVoiceChat = (byte)m_activeState.gameModeVoiceChat;
					optionsNotification.HideTutorialVideos = m_activeState.hideTutorialVideos;
					optionsNotification.AllowCancelActionWhileConfirmed = m_activeState.allowCancelActionWhileConfirmed;
					optionsNotification.Region = m_activeState.region;
					optionsNotification.OverrideGlyphLanguageCode = m_activeState.overrideGlyphLanguageCode;
					ClientGameManager.Get().NotifyOptions(optionsNotification);
					return;
				}
			}
			return;
		}
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
		bool flag = false;
		int num = 0;
		try
		{
			num = Convert.ToInt32(value);
		}
		catch
		{
			flag = true;
		}
		if (flag)
		{
			return;
		}
		string text = Convert.ToString(num);
		if (!(text == value))
		{
			return;
		}
		while (true)
		{
			if (num < 0)
			{
				return;
			}
			while (true)
			{
				if (num > 100)
				{
					return;
				}
				while (true)
				{
					m_pendingState.masterVolume = num;
					m_masterVolumeLabel.text = text;
					if (m_masterVolumeSlider.value != (float)num / 100f && !m_dontUpdateSliders)
					{
						m_masterVolumeSlider.value = (float)num / 100f;
					}
					AudioMixer audioMixer = AudioManager.mixSnapshotManager.snapshot_game.audioMixer;
					float value2 = m_pendingState.ConvertPercentToDb(m_pendingState.masterVolume);
					audioMixer.SetFloat("VolMaster", value2);
					return;
				}
			}
		}
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		if (Application.isEditor)
		{
			return;
		}
		while (true)
		{
			if (!(AudioManager.mixSnapshotManager != null))
			{
				return;
			}
			while (true)
			{
				if (!(AudioManager.mixSnapshotManager.snapshot_game != null))
				{
					return;
				}
				while (true)
				{
					if (!(AudioManager.mixSnapshotManager.snapshot_game.audioMixer != null))
					{
						return;
					}
					while (true)
					{
						AudioMixer audioMixer = AudioManager.mixSnapshotManager.snapshot_game.audioMixer;
						float value = m_activeState.ConvertPercentToDb(m_activeState.musicVolume);
						int percent = 100 - m_activeState.musicVolume;
						float value2 = m_activeState.ConvertPercentToDb(percent);
						if (focusStatus)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									audioMixer.SetFloat("VolMusic", value);
									audioMixer.SetFloat("VolUIAmbiance", value2);
									audioMixer.SetFloat("VolGameAmbiance", 0f);
									return;
								}
							}
						}
						audioMixer.SetFloat("VolMusic", -80f);
						audioMixer.SetFloat("VolUIAmbiance", -80f);
						audioMixer.SetFloat("VolGameAmbiance", -80f);
						return;
					}
				}
			}
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
		bool flag = false;
		int num = 0;
		try
		{
			num = Convert.ToInt32(value);
		}
		catch
		{
			flag = true;
		}
		if (flag)
		{
			return;
		}
		while (true)
		{
			string text = Convert.ToString(num);
			if (!(text == value))
			{
				return;
			}
			while (true)
			{
				if (num < 0)
				{
					return;
				}
				while (true)
				{
					if (num > 100)
					{
						return;
					}
					while (true)
					{
						m_pendingState.musicVolume = num;
						m_musicVolumeLabel.text = text;
						if (m_musicVolumeSlider.value != (float)num / 100f)
						{
							if (!m_dontUpdateSliders)
							{
								m_musicVolumeSlider.value = (float)num / 100f;
							}
						}
						AudioMixer audioMixer = AudioManager.mixSnapshotManager.snapshot_game.audioMixer;
						float value2 = m_pendingState.ConvertPercentToDb(m_pendingState.musicVolume);
						audioMixer.SetFloat("VolMusic", value2);
						int percent = 100 - m_pendingState.musicVolume;
						float value3 = m_pendingState.ConvertPercentToDb(percent);
						audioMixer.SetFloat("VolUIAmbiance", value3);
						return;
					}
				}
			}
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
		m_graphicsLowButton.SetSelected(true, false, string.Empty, string.Empty);
		m_graphicsMediumButton.SetSelected(false, false, string.Empty, string.Empty);
		m_graphicsHighButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnGraphicsQualityMedium(BaseEventData data)
	{
		m_pendingState.graphicsQuality = GraphicsQuality.Medium;
		m_graphicsLowButton.SetSelected(false, false, string.Empty, string.Empty);
		m_graphicsMediumButton.SetSelected(true, false, string.Empty, string.Empty);
		m_graphicsHighButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnGraphicsQualityHigh(BaseEventData data)
	{
		m_pendingState.graphicsQuality = GraphicsQuality.High;
		m_graphicsLowButton.SetSelected(false, false, string.Empty, string.Empty);
		m_graphicsMediumButton.SetSelected(false, false, string.Empty, string.Empty);
		m_graphicsHighButton.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnWindowMode(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_windowModeDropdown, !m_windowModeDropdown.activeSelf);
	}

	public void OnWindowModeDeselect(BaseEventData data)
	{
		PointerEventData pointerEventData = (PointerEventData)data;
		if (RectTransformUtility.RectangleContainsScreenPoint(m_windowModeDropdown.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera) || RectTransformUtility.RectangleContainsScreenPoint(m_windowModeButton.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_windowModeDropdown, false);
			return;
		}
	}

	public void SetResolutionText(string text)
	{
		for (int i = 0; i < m_resolutionText.Length; i++)
		{
			m_resolutionText[i].text = text;
		}
		while (true)
		{
			return;
		}
	}

	public void SetWindowModeText(string text)
	{
		for (int i = 0; i < m_windowModeText.Length; i++)
		{
			m_windowModeText[i].text = text;
		}
		while (true)
		{
			return;
		}
	}

	public void OnWindowModeWindowed(BaseEventData data)
	{
		m_pendingState.windowMode = SettingsState.WindowMode.Windowed;
		SetWindowModeText(StringUtil.TR("Windowed", "Options"));
		UIManager.SetGameObjectActive(m_windowModeDropdown, false);
		bool flag = false;
		Resolution[] resolutions = Screen.resolutions;
		for (int i = 0; i < resolutions.Length; i++)
		{
			Resolution resolution = resolutions[i];
			if (resolution.width != m_pendingState.resolutionWidth)
			{
				continue;
			}
			if (resolution.height == m_pendingState.resolutionHeight)
			{
				flag = true;
			}
		}
		while (true)
		{
			if (flag)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						Get().SetResolutionText(m_pendingState.resolutionWidth + " x " + m_pendingState.resolutionHeight);
						return;
					}
				}
			}
			Get().SetResolutionText(StringUtil.TR("Custom", "Options"));
			return;
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
		if (RectTransformUtility.RectangleContainsScreenPoint(m_ResolutionScrollView.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
		{
			return;
		}
		while (true)
		{
			if (RectTransformUtility.RectangleContainsScreenPoint(m_resolutionButton.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
			{
				return;
			}
			while (true)
			{
				if (!RectTransformUtility.RectangleContainsScreenPoint(m_ResolutionScrollView.verticalScrollbar.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
				{
					while (true)
					{
						UIManager.SetGameObjectActive(m_resolutionDropdown, false);
						return;
					}
				}
				return;
			}
		}
	}

	public void OnGameWindowMode(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_gameWindowModeDropdown, !m_gameWindowModeDropdown.activeSelf);
	}

	public void OnGameWindowModeDeselect(BaseEventData data)
	{
		PointerEventData pointerEventData = (PointerEventData)data;
		if (RectTransformUtility.RectangleContainsScreenPoint(m_gameWindowModeDropdown.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
		{
			return;
		}
		while (true)
		{
			if (!RectTransformUtility.RectangleContainsScreenPoint(m_gameWindowModeButton.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
			{
				while (true)
				{
					UIManager.SetGameObjectActive(m_gameWindowModeDropdown, false);
					return;
				}
			}
			return;
		}
	}

	public void SetGameResolutionText(string text)
	{
		for (int i = 0; i < m_gameResolutionText.Length; i++)
		{
			m_gameResolutionText[i].text = text;
		}
		while (true)
		{
			return;
		}
	}

	public void SetGameWindowModeText(string text)
	{
		for (int i = 0; i < m_gameWindowModeText.Length; i++)
		{
			m_gameWindowModeText[i].text = text;
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
		bool flag = false;
		Resolution[] resolutions = Screen.resolutions;
		for (int i = 0; i < resolutions.Length; i++)
		{
			Resolution resolution = resolutions[i];
			if (resolution.width == m_pendingState.gameResolutionWidth)
			{
				if (resolution.height == m_pendingState.gameResolutionHeight)
				{
					flag = true;
				}
			}
		}
		while (true)
		{
			if (flag)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						Get().SetGameResolutionText(m_pendingState.gameResolutionWidth + " x " + m_pendingState.gameResolutionHeight);
						return;
					}
				}
			}
			Get().SetGameResolutionText(StringUtil.TR("Custom", "Options"));
			return;
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
		if (RectTransformUtility.RectangleContainsScreenPoint(m_gameResolutionScrollView.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera) || RectTransformUtility.RectangleContainsScreenPoint(m_gameResolutionButton.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera) || RectTransformUtility.RectangleContainsScreenPoint(m_gameResolutionScrollView.verticalScrollbar.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_gameResolutionDropdown, false);
			return;
		}
	}

	public void OnRegion(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_regionDropdown, !m_regionDropdown.activeSelf);
	}

	public void OnRegionDeselect(BaseEventData data)
	{
		PointerEventData pointerEventData = (PointerEventData)data;
		if (RectTransformUtility.RectangleContainsScreenPoint(m_regionDropdown.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
		{
			return;
		}
		while (true)
		{
			if (!RectTransformUtility.RectangleContainsScreenPoint(m_regionButton.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
			{
				while (true)
				{
					UIManager.SetGameObjectActive(m_regionDropdown, false);
					return;
				}
			}
			return;
		}
	}

	public void SetRegionText(string text)
	{
		for (int i = 0; i < m_regionText.Length; i++)
		{
			m_regionText[i].text = text;
		}
		while (true)
		{
			return;
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
		if (RectTransformUtility.RectangleContainsScreenPoint(m_LanguageScrollView.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera) || RectTransformUtility.RectangleContainsScreenPoint(m_languageButton.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera) || RectTransformUtility.RectangleContainsScreenPoint(m_LanguageScrollView.verticalScrollbar.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_languageDropdown, false);
			return;
		}
	}

	public void SetLanguageText(string text)
	{
		for (int i = 0; i < m_languageText.Length; i++)
		{
			m_languageText[i].text = text;
		}
	}

	public void OnRegionNorthAmerica(BaseEventData data)
	{
		m_pendingState.region = Region.US;
		SetRegionText(StringUtil.TR("NorthAmerica", "Options"));
		UIManager.SetGameObjectActive(m_regionDropdown, false);
	}

	public void OnRegionEurope(BaseEventData data)
	{
		m_pendingState.region = Region.EU;
		SetRegionText(StringUtil.TR("Europe", "Options"));
		UIManager.SetGameObjectActive(m_regionDropdown, false);
	}

	public void OnLockCursor(BaseEventData data)
	{
		m_pendingState.lockCursorMode = SettingsState.LockCursorMode.On;
		m_lockCursorButton.SetSelected(true, false, string.Empty, string.Empty);
		m_unlockCursorButton.SetSelected(false, false, string.Empty, string.Empty);
		m_smartLockCursorButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnUnlockCursor(BaseEventData data)
	{
		m_pendingState.lockCursorMode = SettingsState.LockCursorMode.Off;
		m_lockCursorButton.SetSelected(false, false, string.Empty, string.Empty);
		m_unlockCursorButton.SetSelected(true, false, string.Empty, string.Empty);
		m_smartLockCursorButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnSmartLockCursor(BaseEventData data)
	{
		m_pendingState.lockCursorMode = SettingsState.LockCursorMode.Smart;
		m_lockCursorButton.SetSelected(false, false, string.Empty, string.Empty);
		m_unlockCursorButton.SetSelected(false, false, string.Empty, string.Empty);
		m_smartLockCursorButton.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnEnableChatter(BaseEventData data)
	{
		m_pendingState.enableChatter = true;
		m_enableChatterButton.SetSelected(true, false, string.Empty, string.Empty);
		m_disableChatterButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnShowGlobalChat(BaseEventData data)
	{
		m_pendingState.showGlobalChat = true;
		m_showGlobalChatButton.SetSelected(true, false, string.Empty, string.Empty);
		m_hideGlobalChatButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnHideGlobalChat(BaseEventData data)
	{
		m_pendingState.showGlobalChat = false;
		m_showGlobalChatButton.SetSelected(false, false, string.Empty, string.Empty);
		m_hideGlobalChatButton.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnShowAllChat(BaseEventData data)
	{
		m_pendingState.showAllChat = true;
		m_showAllChatButton.SetSelected(true, false, string.Empty, string.Empty);
		m_hideAllChatButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnHideAllChat(BaseEventData data)
	{
		m_pendingState.showAllChat = false;
		m_showAllChatButton.SetSelected(false, false, string.Empty, string.Empty);
		m_hideAllChatButton.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnEnableProfanityFilter(BaseEventData data)
	{
		m_pendingState.enableProfanityFilter = true;
		m_enableProfanityFilterButton.SetSelected(true, false, string.Empty, string.Empty);
		m_disableProfanityFilterButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnDisableProfanityFilter(BaseEventData data)
	{
		m_pendingState.enableProfanityFilter = false;
		m_enableProfanityFilterButton.SetSelected(false, false, string.Empty, string.Empty);
		m_disableProfanityFilterButton.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnEnableAutoJoinDiscord(BaseEventData data)
	{
		m_pendingState.autoJoinDiscord = true;
		m_enableAutoJoinDiscordButton.SetSelected(true, false, string.Empty, string.Empty);
		m_disableAutoJoinDiscordButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnDisableAutoJoinDiscord(BaseEventData data)
	{
		m_pendingState.autoJoinDiscord = false;
		m_enableAutoJoinDiscordButton.SetSelected(false, false, string.Empty, string.Empty);
		m_disableAutoJoinDiscordButton.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnGroupGameModeVoiceChat(BaseEventData data)
	{
		m_pendingState.gameModeVoiceChat = SettingsState.VoiceChatMode.Group;
		m_groupGameModeVoiceChatButton.SetSelected(true, false, string.Empty, string.Empty);
		m_teamGameModeVoiceChatButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnTeamGameModeVoiceChat(BaseEventData data)
	{
		m_pendingState.gameModeVoiceChat = SettingsState.VoiceChatMode.Team;
		m_groupGameModeVoiceChatButton.SetSelected(false, false, string.Empty, string.Empty);
		m_teamGameModeVoiceChatButton.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnDisableChatter(BaseEventData data)
	{
		m_pendingState.enableChatter = false;
		m_enableChatterButton.SetSelected(false, false, string.Empty, string.Empty);
		m_disableChatterButton.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnRightClickConfirm(BaseEventData data)
	{
		m_pendingState.rightClickingConfirmsAbilityTargets = true;
		m_rightClickTargetingConfirm.SetSelected(true, false, string.Empty, string.Empty);
		m_rightClickTargetingCancel.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnRightClickCancel(BaseEventData data)
	{
		m_pendingState.rightClickingConfirmsAbilityTargets = false;
		m_rightClickTargetingConfirm.SetSelected(false, false, string.Empty, string.Empty);
		m_rightClickTargetingCancel.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnShiftClickWaypointsConfirm(BaseEventData data)
	{
		m_pendingState.shiftClickForMovementWaypoints = true;
		m_shiftClickForWaypoints.SetSelected(true, false, string.Empty, string.Empty);
		m_shiftClickForNewPath.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnShiftClickWaypointsCancel(BaseEventData data)
	{
		m_pendingState.shiftClickForMovementWaypoints = false;
		m_shiftClickForWaypoints.SetSelected(false, false, string.Empty, string.Empty);
		m_shiftClickForNewPath.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnHideTutorialVideos(BaseEventData data)
	{
		m_pendingState.hideTutorialVideos = true;
		m_hideTutorialVideosButton.SetSelected(true, false, string.Empty, string.Empty);
		m_showTutorialVideosButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnShowTutorialVideos(BaseEventData data)
	{
		m_pendingState.hideTutorialVideos = false;
		m_hideTutorialVideosButton.SetSelected(false, false, string.Empty, string.Empty);
		m_showTutorialVideosButton.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnAllowCancelActionWhileConfirmed(BaseEventData data)
	{
		m_pendingState.allowCancelActionWhileConfirmed = true;
		m_allowCancelActionWhileConfirmedButton.SetSelected(true, false, string.Empty, string.Empty);
		m_disallowCancelActionWhileConfirmedButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnDisallowCancelActionWhileConfirmed(BaseEventData data)
	{
		m_pendingState.allowCancelActionWhileConfirmed = false;
		m_allowCancelActionWhileConfirmedButton.SetSelected(false, false, string.Empty, string.Empty);
		m_disallowCancelActionWhileConfirmedButton.SetSelected(true, false, string.Empty, string.Empty);
	}

	private float ResolutionRound(float f, float digits)
	{
		float num = Mathf.Pow(10f, digits);
		return Mathf.Round(f * num) / num;
	}

	private void PopulateResolutionDropdown()
	{
		PopulateGeneralResolutionDropdown(m_ResolutionScrollView, m_resolutionItemContainer, m_pendingState.windowMode, OnSpecificResolution, m_activeState.resolutionWidth, m_activeState.resolutionHeight);
	}

	private void PopulateGameResolutionDropdown()
	{
		PopulateGeneralResolutionDropdown(m_gameResolutionScrollView, m_gameResolutionItemContainer, m_pendingState.gameWindowMode, OnSpecificGameResolution, m_activeState.resolutionWidth, m_activeState.resolutionHeight);
	}

	private void PopulateGeneralResolutionDropdown(ScrollRect scrollView, GridLayoutGroup itemContainer, SettingsState.WindowMode pendingWindowMode, _ButtonSwapSprite.ButtonClickCallback changeCallback, int currentWidth, int currentHeight)
	{
		foreach (GameObject resolutionButton in m_resolutionButtons)
		{
			UnityEngine.Object.Destroy(resolutionButton);
		}
		m_resolutionButtons.Clear();
		int num = 0;
		int num2 = 1;
		Resolution[] resolutions = Screen.resolutions;
		if (_003C_003Ef__am_0024cache0 == null)
		{
			_003C_003Ef__am_0024cache0 = ((Resolution r) => r.width);
		}
		IOrderedEnumerable<Resolution> source = resolutions.OrderByDescending(_003C_003Ef__am_0024cache0);
		if (_003C_003Ef__am_0024cache1 == null)
		{
			_003C_003Ef__am_0024cache1 = ((Resolution r) => r.height);
		}
		Resolution[] array = source.ThenByDescending(_003C_003Ef__am_0024cache1).ToArray();
		bool flag = false;
		Resolution[] array2 = array;
		int num3 = 0;
		while (num3 < array2.Length)
		{
			Resolution resolution = array2[num3];
			GameObject gameObject = UnityEngine.Object.Instantiate(Get().m_resolutionButtonPrefab);
			gameObject.transform.SetParent(itemContainer.transform, false);
			num2++;
			RectTransform rectTransform = gameObject.transform as RectTransform;
			Vector2 anchoredPosition = rectTransform.anchoredPosition;
			anchoredPosition.y = -20 + -30 * num;
			rectTransform.anchoredPosition = anchoredPosition;
			TextMeshProUGUI[] componentsInChildren = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].text = resolution.width + " x " + resolution.height;
			}
			while (true)
			{
				m_resolutionButtons.Add(gameObject);
				UIManager.SetGameObjectActive(gameObject, true);
				ResolutionSetting setting = default(ResolutionSetting);
				setting.resolution = resolution;
				setting.custom = false;
				RegisterResolutionSetting(gameObject, setting);
				_SelectableBtn component = gameObject.GetComponent<_SelectableBtn>();
				component.spriteController.callback = changeCallback;
				component.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
				if (scrollView != null)
				{
					_MouseEventPasser mouseEventPasser = component.spriteController.gameObject.AddComponent<_MouseEventPasser>();
					mouseEventPasser.AddNewHandler(scrollView);
				}
				if (currentWidth == resolution.width && currentHeight == resolution.height)
				{
					flag = true;
				}
				num++;
				num3++;
				goto IL_0260;
			}
			IL_0260:;
		}
		while (true)
		{
			if (pendingWindowMode == SettingsState.WindowMode.Windowed && !flag)
			{
				if (currentWidth > 0)
				{
					if (currentHeight > 0)
					{
						GameObject gameObject2 = UnityEngine.Object.Instantiate(Get().m_resolutionButtonPrefab);
						gameObject2.transform.SetParent(itemContainer.transform, false);
						num2++;
						RectTransform rectTransform2 = gameObject2.transform as RectTransform;
						Vector2 anchoredPosition2 = rectTransform2.anchoredPosition;
						anchoredPosition2.y = -20 + -30 * num;
						rectTransform2.anchoredPosition = anchoredPosition2;
						TextMeshProUGUI[] componentsInChildren2 = gameObject2.GetComponentsInChildren<TextMeshProUGUI>();
						for (int j = 0; j < componentsInChildren2.Length; j++)
						{
							componentsInChildren2[j].text = StringUtil.TR("Custom", "Options");
						}
						m_resolutionButtons.Add(gameObject2);
						UIManager.SetGameObjectActive(gameObject2, true);
						ResolutionSetting setting2 = default(ResolutionSetting);
						setting2.resolution.width = m_activeState.resolutionWidth;
						setting2.resolution.height = m_activeState.resolutionHeight;
						setting2.custom = true;
						RegisterResolutionSetting(gameObject2, setting2);
						_SelectableBtn component2 = gameObject2.GetComponent<_SelectableBtn>();
						component2.spriteController.callback = changeCallback;
						component2.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
						if (scrollView != null)
						{
							_MouseEventPasser mouseEventPasser2 = component2.spriteController.gameObject.AddComponent<_MouseEventPasser>();
							mouseEventPasser2.AddNewHandler(scrollView);
						}
						num++;
					}
				}
			}
			RectTransform rectTransform3 = itemContainer.transform as RectTransform;
			Vector2 sizeDelta = rectTransform3.sizeDelta;
			float x = sizeDelta.x;
			Vector2 cellSize = itemContainer.cellSize;
			rectTransform3.sizeDelta = new Vector2(x, cellSize.y * (float)num2);
			return;
		}
	}

	private void PopulateLanguageDropdown()
	{
		using (List<GameObject>.Enumerator enumerator = m_languageButtons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject current = enumerator.Current;
				UnityEngine.Object.Destroy(current);
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					goto end_IL_000e;
				}
			}
			end_IL_000e:;
		}
		m_languageButtons.Clear();
		int num = 0;
		while (num < 11)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Get().m_resolutionButtonPrefab);
			gameObject.transform.SetParent(m_languageItemContainer.transform, false);
			RectTransform rectTransform = gameObject.transform as RectTransform;
			Vector2 anchoredPosition = rectTransform.anchoredPosition;
			anchoredPosition.y = -20 + -30 * num;
			rectTransform.anchoredPosition = anchoredPosition;
			TextMeshProUGUI[] componentsInChildren = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
			foreach (TextMeshProUGUI obj in componentsInChildren)
			{
				LanguageOptions languageOptions = (LanguageOptions)num;
				obj.text = StringUtil.TR(languageOptions.ToString(), "LanguageSelection");
			}
			while (true)
			{
				m_languageButtons.Add(gameObject);
				UIManager.SetGameObjectActive(gameObject, true);
				_SelectableBtn component = gameObject.GetComponent<_SelectableBtn>();
				component.spriteController.callback = OnSpecificLanguage;
				component.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
				if (m_LanguageScrollView != null)
				{
					_MouseEventPasser mouseEventPasser = component.spriteController.gameObject.AddComponent<_MouseEventPasser>();
					mouseEventPasser.AddNewHandler(m_LanguageScrollView);
				}
				num++;
				goto IL_0192;
			}
			IL_0192:;
		}
		RectTransform rectTransform2 = m_languageItemContainer.transform as RectTransform;
		Vector2 sizeDelta = rectTransform2.sizeDelta;
		float x = sizeDelta.x;
		Vector2 cellSize = m_languageItemContainer.cellSize;
		rectTransform2.sizeDelta = new Vector2(x, cellSize.y * 11f);
	}

	private void RegisterResolutionSetting(GameObject obj, ResolutionSetting setting)
	{
		m_resolutionButtonData[obj.GetComponent<_SelectableBtn>().spriteController.gameObject] = setting;
	}

	public void OnSpecificGameResolution(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_gameResolutionDropdown, false);
		if (!m_resolutionButtonData.ContainsKey(data.selectedObject))
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
		ResolutionSetting resolutionSetting = m_resolutionButtonData[data.selectedObject];
		m_pendingState.gameResolutionWidth = resolutionSetting.resolution.width;
		m_pendingState.gameResolutionHeight = resolutionSetting.resolution.height;
		if (resolutionSetting.custom)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					SetGameResolutionText(StringUtil.TR("Custom", "Options"));
					return;
				}
			}
		}
		SetGameResolutionText(resolutionSetting.resolution.width + " x " + resolutionSetting.resolution.height);
	}

	public void OnSpecificResolution(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_resolutionDropdown, false);
		if (!m_resolutionButtonData.ContainsKey(data.selectedObject))
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
		ResolutionSetting resolutionSetting = m_resolutionButtonData[data.selectedObject];
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
		int num = 0;
		using (List<GameObject>.Enumerator enumerator = m_languageButtons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject current = enumerator.Current;
				if (current.GetComponent<_SelectableBtn>().spriteController.gameObject == data.selectedObject)
				{
					LanguageOptions languageOptions = (LanguageOptions)num;
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
					LanguageOptions languageOptions2 = (LanguageOptions)num;
					SetLanguageText(StringUtil.TR(languageOptions2.ToString(), "LanguageSelection"));
				}
				num++;
			}
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
		m_showGlobalChatButton.SetSelected(show, false, string.Empty, string.Empty);
		m_hideGlobalChatButton.SetSelected(!show, false, string.Empty, string.Empty);
	}

	public bool GetShowAllChat()
	{
		return m_activeState.showAllChat;
	}

	public void SetShowAllChat(bool show)
	{
		m_activeState.showAllChat = show;
		m_showAllChatButton.SetSelected(show, false, string.Empty, string.Empty);
		m_hideAllChatButton.SetSelected(!show, false, string.Empty, string.Empty);
	}

	public bool GetEnableProfanityFilter()
	{
		return m_activeState.enableProfanityFilter;
	}

	public void SetEnableProfanityFilter(bool show)
	{
		m_activeState.enableProfanityFilter = show;
		m_enableProfanityFilterButton.SetSelected(show, false, string.Empty, string.Empty);
		m_disableProfanityFilterButton.SetSelected(!show, false, string.Empty, string.Empty);
	}

	public bool GetEnableAutoJoinDiscord()
	{
		return m_activeState.autoJoinDiscord;
	}

	public void SetEnableAutoJoinDiscord(bool show)
	{
		m_activeState.autoJoinDiscord = show;
		m_enableAutoJoinDiscordButton.SetSelected(show, false, string.Empty, string.Empty);
		m_disableAutoJoinDiscordButton.SetSelected(!show, false, string.Empty, string.Empty);
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
		if (s_instance != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return s_instance.m_activeState.region;
				}
			}
		}
		return (Region)PlayerPrefs.GetInt("Region", (int)GetDefaultRegion());
	}

	public static Region GetDefaultRegion()
	{
		DateTime utcNow = DateTime.UtcNow;
		DateTime d = utcNow.ToLocalTime();
		double totalHours = (d - utcNow).TotalHours;
		double num = -2.5;
		double num2 = 7.5;
		if (totalHours > num)
		{
			if (totalHours < num2)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return Region.EU;
					}
				}
			}
		}
		return Region.US;
	}

	public SettingsState.VoiceChatMode GetGameModeVoiceChat()
	{
		return m_activeState.gameModeVoiceChat;
	}

	public void SetGameModeVoiceChat(SettingsState.VoiceChatMode setting)
	{
		m_activeState.gameModeVoiceChat = setting;
		if (setting != 0)
		{
			if (setting != SettingsState.VoiceChatMode.Group)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						if (setting == SettingsState.VoiceChatMode.Team)
						{
							m_groupGameModeVoiceChatButton.SetSelected(false, false, string.Empty, string.Empty);
							m_teamGameModeVoiceChatButton.SetSelected(true, false, string.Empty, string.Empty);
						}
						return;
					}
				}
			}
		}
		m_groupGameModeVoiceChatButton.SetSelected(true, false, string.Empty, string.Empty);
		m_teamGameModeVoiceChatButton.SetSelected(false, false, string.Empty, string.Empty);
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
