using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Options_UI : UIScene, IGameEventListener
{
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

	private Dictionary<GameObject, Options_UI.ResolutionSetting> m_resolutionButtonData = new Dictionary<GameObject, Options_UI.ResolutionSetting>();

	private List<GameObject> m_languageButtons = new List<GameObject>();

	public TextMeshProUGUI m_optionsLabelText;

	public string ActiveStateName
	{
		get
		{
			return this.m_activeState.ToString();
		}
	}

	public static Options_UI Get()
	{
		return Options_UI.s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.Options;
	}

	public override void Awake()
	{
		Options_UI.s_instance = this;
		this.m_scrollRect = base.GetComponentInChildren<ScrollRect>();
		UIManager.SetGameObjectActive(this.m_container, false, null);
		Options_UI.s_hwnd = (IntPtr)0;
		this.m_pauseUpdate = true;
		UIManager.SetGameObjectActive(this.m_resolutionDropdown, false, null);
		UIManager.SetGameObjectActive(this.m_windowModeDropdown, false, null);
		UIManager.SetGameObjectActive(this.m_gameResolutionDropdown, false, null);
		UIManager.SetGameObjectActive(this.m_gameWindowModeDropdown, false, null);
		UIManager.SetGameObjectActive(this.m_regionDropdown, false, null);
		UIManager.SetGameObjectActive(this.m_languageDropdown, false, null);
		UIManager.SetGameObjectActive(this.m_restartWarning, false, null);
		if (this.m_ResolutionScrollView != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.Awake()).MethodHandle;
			}
			if (this.m_resolutionBackgroundHitBox != null)
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
				_MouseEventPasser mouseEventPasser = this.m_resolutionBackgroundHitBox.gameObject.AddComponent<_MouseEventPasser>();
				mouseEventPasser.AddNewHandler(this.m_ResolutionScrollView);
			}
		}
		if (this.m_gameResolutionScrollView != null)
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
			if (this.m_gameResolutionBackgroundHitBox != null)
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
				_MouseEventPasser mouseEventPasser2 = this.m_gameResolutionBackgroundHitBox.gameObject.AddComponent<_MouseEventPasser>();
				mouseEventPasser2.AddNewHandler(this.m_gameResolutionScrollView);
			}
		}
		if (this.m_LanguageScrollView != null)
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
			if (this.m_languageBackgroundHitBox != null)
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
				_MouseEventPasser mouseEventPasser3 = this.m_languageBackgroundHitBox.gameObject.AddComponent<_MouseEventPasser>();
				mouseEventPasser3.AddNewHandler(this.m_LanguageScrollView);
			}
		}
		this.m_graphicsLowButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_graphicsMediumButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_graphicsHighButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_windowModeButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_windowModeWindowedButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_windowModeFullscreenButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_resolutionButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_gameWindowModeButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_gameWindowModeInheritButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_gameWindowModeWindowedButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_gameWindowModeFullscreenButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_gameResolutionButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_regionButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_regionNorthAmericaButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_regionEuropeButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_languageButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_lockCursorButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_unlockCursorButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_smartLockCursorButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_enableChatterButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_disableChatterButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_rightClickTargetingConfirm.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_rightClickTargetingCancel.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_showGlobalChatButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_hideGlobalChatButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_showAllChatButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_hideAllChatButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_enableProfanityFilterButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_disableProfanityFilterButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_enableAutoJoinDiscordButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_disableAutoJoinDiscordButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_groupGameModeVoiceChatButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_teamGameModeVoiceChatButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_hideTutorialVideosButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_showTutorialVideosButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_allowCancelActionWhileConfirmedButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_disallowCancelActionWhileConfirmedButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.AppStateChanged);
		base.Awake();
	}

	private void Start()
	{
		if (this.m_secretButton != null)
		{
			UIEventTriggerUtils.AddListener(this.m_secretButton.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnSecretButton));
		}
		if (this.m_secretButtonText != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.Start()).MethodHandle;
			}
			UIEventTriggerUtils.AddListener(this.m_secretButtonText.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnSecretButton));
		}
		if (this.m_okButton != null)
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
			this.m_okButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnOkButton);
		}
		if (this.m_applyButton != null)
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
			this.m_applyButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnApplyButton);
		}
		if (this.m_revertDefaultsButton != null)
		{
			this.m_revertDefaultsButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnRevertDefaultsButton);
		}
		if (this.m_graphicsLowButton != null)
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
			this.m_graphicsLowButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnGraphicsQualityLow);
			this.m_graphicsLowButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_graphicsMediumButton != null)
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
			this.m_graphicsMediumButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnGraphicsQualityMedium);
			this.m_graphicsMediumButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_graphicsHighButton != null)
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
			this.m_graphicsHighButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnGraphicsQualityHigh);
			this.m_graphicsHighButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_windowModeButton != null)
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
			this.m_windowModeButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnWindowMode);
			this.m_windowModeButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
			UIEventTriggerUtils.AddListener(this.m_windowModeButton.spriteController.gameObject, EventTriggerType.Deselect, new UIEventTriggerUtils.EventDelegate(this.OnResolutionDeselect));
		}
		if (this.m_gameWindowModeButton != null)
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
			this.m_gameWindowModeButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnGameWindowMode);
			this.m_gameWindowModeButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
			UIEventTriggerUtils.AddListener(this.m_gameWindowModeButton.spriteController.gameObject, EventTriggerType.Deselect, new UIEventTriggerUtils.EventDelegate(this.OnGameResolutionDeselect));
		}
		if (this.m_closeButton != null)
		{
			this.m_closeButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnCancelButton);
		}
		if (this.m_windowModeWindowedButton != null)
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
			this.m_windowModeWindowedButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnWindowModeWindowed);
			this.m_windowModeWindowedButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_windowModeFullscreenButton != null)
		{
			this.m_windowModeFullscreenButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnWindowModeFullscreen);
			this.m_windowModeFullscreenButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_gameWindowModeInheritButton != null)
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
			this.m_gameWindowModeInheritButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnGameWindowModeInherit);
			this.m_gameWindowModeInheritButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_gameWindowModeWindowedButton != null)
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
			this.m_gameWindowModeWindowedButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnGameWindowModeWindowed);
			this.m_gameWindowModeWindowedButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_gameWindowModeFullscreenButton != null)
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
			this.m_gameWindowModeFullscreenButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnGameWindowModeFullscreen);
			this.m_gameWindowModeFullscreenButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_resolutionButton != null)
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
			this.m_resolutionButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnResolution);
			this.m_resolutionButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
			UIEventTriggerUtils.AddListener(this.m_resolutionButton.spriteController.gameObject, EventTriggerType.Deselect, new UIEventTriggerUtils.EventDelegate(this.OnResolutionDeselect));
		}
		if (this.m_gameResolutionButton != null)
		{
			this.m_gameResolutionButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnGameResolution);
			this.m_gameResolutionButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
			UIEventTriggerUtils.AddListener(this.m_gameResolutionButton.spriteController.gameObject, EventTriggerType.Deselect, new UIEventTriggerUtils.EventDelegate(this.OnGameResolutionDeselect));
		}
		if (this.m_regionButton != null)
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
			this.m_regionButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnRegion);
			this.m_regionButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
			UIEventTriggerUtils.AddListener(this.m_regionButton.spriteController.gameObject, EventTriggerType.Deselect, new UIEventTriggerUtils.EventDelegate(this.OnRegionDeselect));
		}
		if (this.m_languageButton != null)
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
			this.m_languageButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnLanguage);
			this.m_languageButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
			UIEventTriggerUtils.AddListener(this.m_languageButton.spriteController.gameObject, EventTriggerType.Deselect, new UIEventTriggerUtils.EventDelegate(this.OnLanguageDeselect));
		}
		if (this.m_regionNorthAmericaButton != null)
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
			this.m_regionNorthAmericaButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnRegionNorthAmerica);
		}
		if (this.m_regionEuropeButton != null)
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
			this.m_regionEuropeButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnRegionEurope);
		}
		if (this.m_masterVolumeSlider != null)
		{
			this.m_masterVolumeSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnMasterVolumeSliderChange));
		}
		if (this.m_musicVolumeSlider != null)
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
			this.m_musicVolumeSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnMusicVolumeSliderChange));
		}
		if (this.m_lockCursorButton != null)
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
			this.m_lockCursorButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnLockCursor);
			this.m_lockCursorButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_unlockCursorButton != null)
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
			this.m_unlockCursorButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnUnlockCursor);
			this.m_unlockCursorButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_smartLockCursorButton != null)
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
			this.m_smartLockCursorButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnSmartLockCursor);
			this.m_smartLockCursorButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_enableChatterButton != null)
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
			this.m_enableChatterButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnEnableChatter);
			this.m_enableChatterButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_disableChatterButton != null)
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
			this.m_disableChatterButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnDisableChatter);
			this.m_disableChatterButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_rightClickTargetingConfirm != null)
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
			this.m_rightClickTargetingConfirm.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnRightClickConfirm);
			this.m_rightClickTargetingConfirm.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_rightClickTargetingCancel != null)
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
			this.m_rightClickTargetingCancel.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnRightClickCancel);
			this.m_rightClickTargetingCancel.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_shiftClickForWaypoints != null)
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
			this.m_shiftClickForWaypoints.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnShiftClickWaypointsConfirm);
			this.m_shiftClickForWaypoints.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_shiftClickForNewPath != null)
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
			this.m_shiftClickForNewPath.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnShiftClickWaypointsCancel);
			this.m_shiftClickForNewPath.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_showGlobalChatButton != null)
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
			this.m_showGlobalChatButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnShowGlobalChat);
			this.m_showGlobalChatButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_hideGlobalChatButton != null)
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
			this.m_hideGlobalChatButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnHideGlobalChat);
			this.m_hideGlobalChatButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_showAllChatButton != null)
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
			this.m_showAllChatButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnShowAllChat);
			this.m_showAllChatButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_hideAllChatButton != null)
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
			this.m_hideAllChatButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnHideAllChat);
			this.m_hideAllChatButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_enableProfanityFilterButton != null)
		{
			this.m_enableProfanityFilterButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnEnableProfanityFilter);
			this.m_enableProfanityFilterButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_disableProfanityFilterButton != null)
		{
			this.m_disableProfanityFilterButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnDisableProfanityFilter);
			this.m_disableProfanityFilterButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_enableAutoJoinDiscordButton != null)
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
			this.m_enableAutoJoinDiscordButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnEnableAutoJoinDiscord);
			this.m_enableAutoJoinDiscordButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_disableAutoJoinDiscordButton != null)
		{
			this.m_disableAutoJoinDiscordButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnDisableAutoJoinDiscord);
			this.m_disableAutoJoinDiscordButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_groupGameModeVoiceChatButton != null)
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
			this.m_groupGameModeVoiceChatButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnGroupGameModeVoiceChat);
			this.m_groupGameModeVoiceChatButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_teamGameModeVoiceChatButton != null)
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
			this.m_teamGameModeVoiceChatButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnTeamGameModeVoiceChat);
			this.m_teamGameModeVoiceChatButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_hideTutorialVideosButton != null)
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
			this.m_hideTutorialVideosButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnHideTutorialVideos);
			this.m_hideTutorialVideosButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_showTutorialVideosButton != null)
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
			this.m_showTutorialVideosButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnShowTutorialVideos);
			this.m_showTutorialVideosButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_allowCancelActionWhileConfirmedButton != null)
		{
			this.m_allowCancelActionWhileConfirmedButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnAllowCancelActionWhileConfirmed);
			this.m_allowCancelActionWhileConfirmedButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		if (this.m_disallowCancelActionWhileConfirmedButton != null)
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
			this.m_disallowCancelActionWhileConfirmedButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnDisallowCancelActionWhileConfirmed);
			this.m_disallowCancelActionWhileConfirmedButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		this.m_activeState = new SettingsState();
		this.m_pendingState = new SettingsState();
		this.TrySetupHwnd();
		if (!PlayerPrefs.HasKey("OptionsInitialized"))
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
			PlayerPrefs.SetInt("OptionsInitialized", 1);
			this.m_activeState.InitToDefaults();
			this.m_activeState.ApplyPendingValues(null);
			this.m_activeState.ApplyToPlayerPrefs();
		}
		else
		{
			this.m_activeState.InitFromPlayerPrefs();
			this.m_activeState.ApplyPendingValues(null);
		}
		this.m_pendingState = (SettingsState)this.m_activeState.Clone();
		this.SendNotifyOptions(false);
	}

	private void OnDestroy()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.AppStateChanged);
		Options_UI.s_instance = null;
	}

	private void OnScroll(BaseEventData data)
	{
		this.m_scrollRect.OnScroll((PointerEventData)data);
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.AppStateChanged)
		{
			this.m_activeState.UpdateGameResolution();
		}
	}

	public void TrySetupHwnd()
	{
		IntPtr value = WinUtils.User32.GetActiveWindow();
		if (value == (IntPtr)0)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.TrySetupHwnd()).MethodHandle;
			}
			if (this.m_firstSetupTry)
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
				this.m_firstSetupTry = false;
				List<IntPtr> list = (List<IntPtr>)Options_UI.FindWindowsWithExactText("Atlas Reactor");
				if (list.Count<IntPtr>() == 1)
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
					value = list[0];
				}
			}
		}
		if (!(value != (IntPtr)0))
		{
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
		Options_UI.s_hwnd = value;
		Options_UI.Get().m_pauseUpdate = false;
		WinUtils.User32.RECT rect;
		if (!WinUtils.User32.GetWindowRect(Options_UI.s_hwnd, out rect))
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
			return;
		}
		WinUtils.User32.RECT rect2;
		if (!WinUtils.User32.GetClientRect(Options_UI.s_hwnd, out rect2))
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
			return;
		}
		int num = rect.Right - rect.Left - (rect2.Right - rect2.Left);
		int num2 = rect.Bottom - rect.Top - (rect2.Bottom - rect2.Top);
		this.m_borderWidth = num / 2;
		this.m_captionHeight = num2 - this.m_borderWidth * 2;
	}

	[DllImport("user32.dll", CharSet = CharSet.Unicode)]
	private static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

	[DllImport("user32.dll", CharSet = CharSet.Unicode)]
	private static extern int GetWindowTextLength(IntPtr hWnd);

	[DllImport("user32.dll")]
	private static extern bool EnumWindows(Options_UI.EnumWindowsProc enumProc, IntPtr lParam);

	public static string GetWindowText(IntPtr hWnd)
	{
		int windowTextLength = Options_UI.GetWindowTextLength(hWnd);
		if (windowTextLength++ > 0)
		{
			StringBuilder stringBuilder = new StringBuilder(windowTextLength);
			Options_UI.GetWindowText(hWnd, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString();
		}
		return string.Empty;
	}

	public static IEnumerable<IntPtr> FindWindowsWithExactText(string titleText)
	{
		List<IntPtr> windows = new List<IntPtr>();
		Options_UI.EnumWindows(delegate(IntPtr wnd, IntPtr param)
		{
			if (Options_UI.GetWindowText(wnd) == titleText)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.<FindWindowsWithExactText>c__AnonStorey0.<>m__0(IntPtr, IntPtr)).MethodHandle;
				}
				windows.Add(wnd);
			}
			return true;
		}, IntPtr.Zero);
		return windows;
	}

	private void Update()
	{
		if (this.m_pauseUpdate)
		{
			return;
		}
		if (!Application.isEditor)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.Update()).MethodHandle;
			}
			this.m_activeState.UpdateResolutionFromScreen();
			if (this.m_activeState.lockCursorMode != SettingsState.LockCursorMode.On)
			{
				if (this.m_activeState.lockCursorMode != SettingsState.LockCursorMode.Smart)
				{
					goto IL_84;
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!(GameFlowData.Get() != null))
				{
					goto IL_84;
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (Cursor.lockState != CursorLockMode.Confined)
			{
				Cursor.lockState = CursorLockMode.Confined;
			}
			IL_84:
			if (this.m_activeState.lockCursorMode != SettingsState.LockCursorMode.Off)
			{
				if (this.m_activeState.lockCursorMode != SettingsState.LockCursorMode.Smart)
				{
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
				if (!(GameFlowData.Get() == null))
				{
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
			if (Cursor.lockState != CursorLockMode.None)
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
				Cursor.lockState = CursorLockMode.None;
			}
		}
	}

	private void OnSecretButton(BaseEventData data)
	{
		this.m_secretButtonClicked = !this.m_secretButtonClicked;
	}

	private void OnOkButton(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
		this.ApplyCurrentSettings();
		this.HideOptions();
	}

	private void OnApplyButton(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
		this.ApplyCurrentSettings();
	}

	private void OnRevertDefaultsButton(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
		if (this.m_pendingState.overrideGlyphLanguage)
		{
			UIManager.SetGameObjectActive(this.m_restartWarning, true, null);
		}
		this.m_pendingState.InitToDefaults();
		this.m_pendingState.ApplyToOptionsUI();
		this.ApplyCurrentSettings();
	}

	private void OnCancelButton(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.Back);
		this.HideOptions();
		this.m_activeState.RevertVolume();
	}

	public void ToggleOptions()
	{
		if (!this.m_container.gameObject.activeSelf)
		{
			this.ShowOptions();
		}
		else
		{
			this.HideOptions();
		}
	}

	public void ShowOptions()
	{
		if (Screen.fullScreen)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.ShowOptions()).MethodHandle;
			}
			this.m_activeState.windowMode = SettingsState.WindowMode.Fullscreen;
		}
		else if (this.m_activeState.windowMode == SettingsState.WindowMode.Fullscreen)
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
			this.m_activeState.windowMode = SettingsState.WindowMode.Windowed;
		}
		if (this.m_activeState.windowMode == SettingsState.WindowMode.Fullscreen)
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
			this.m_activeState.resolutionWidth = Screen.currentResolution.width;
			this.m_activeState.resolutionHeight = Screen.currentResolution.height;
		}
		else
		{
			this.m_activeState.resolutionWidth = Screen.width;
			this.m_activeState.resolutionHeight = Screen.height;
		}
		this.m_optionsLabelText.text = StringUtil.TR("OptionLabelsDiscord", "Options");
		if (DiscordClientInterface.IsEnabled)
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
			if (DiscordClientInterface.IsSdkEnabled || DiscordClientInterface.IsInstalled)
			{
				this.SetDisabled(this.m_enableAutoJoinDiscordButton, false);
				this.SetDisabled(this.m_disableAutoJoinDiscordButton, false);
				this.SetDisabled(this.m_groupGameModeVoiceChatButton, false);
				this.SetDisabled(this.m_teamGameModeVoiceChatButton, false);
				goto IL_169;
			}
		}
		this.SetDisabled(this.m_enableAutoJoinDiscordButton, true);
		this.SetDisabled(this.m_disableAutoJoinDiscordButton, true);
		this.SetDisabled(this.m_groupGameModeVoiceChatButton, true);
		this.SetDisabled(this.m_teamGameModeVoiceChatButton, true);
		IL_169:
		this.m_pendingState = (SettingsState)this.m_activeState.Clone();
		this.m_pendingState.ApplyToOptionsUI();
		UIManager.SetGameObjectActive(this.m_container, true, null);
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
		if (this.m_container.gameObject.activeSelf)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.HideOptions()).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_container, false, null);
			UIManager.SetGameObjectActive(this.m_restartWarning, false, null);
			this.SendNotifyOptions(true);
		}
	}

	private void SendNotifyOptions(bool userDialog)
	{
		if (this.m_activeState != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.SendNotifyOptions(bool)).MethodHandle;
			}
			if (ClientGameManager.Get() != null)
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
				OptionsNotification optionsNotification = new OptionsNotification();
				optionsNotification.UserDialog = userDialog;
				optionsNotification.DeviceIdentifier = SystemInfo.deviceUniqueIdentifier;
				optionsNotification.GraphicsQuality = (byte)this.m_activeState.graphicsQuality;
				optionsNotification.WindowMode = (byte)this.m_activeState.windowMode;
				optionsNotification.ResolutionWidth = (short)this.m_activeState.resolutionWidth;
				optionsNotification.ResolutionHeight = (short)this.m_activeState.resolutionHeight;
				optionsNotification.GameWindowMode = (byte)this.m_activeState.gameWindowMode;
				optionsNotification.GameResolutionWidth = (short)this.m_activeState.gameResolutionWidth;
				optionsNotification.GameResolutionHeight = (short)this.m_activeState.gameResolutionHeight;
				optionsNotification.LockWindowSize = this.m_activeState.lockWindowSize;
				optionsNotification.MasterVolume = (byte)this.m_activeState.masterVolume;
				optionsNotification.MusicVolume = (byte)this.m_activeState.musicVolume;
				optionsNotification.AmbianceVolume = (byte)this.m_activeState.ambianceVolume;
				optionsNotification.LockCursorMode = (byte)this.m_activeState.lockCursorMode;
				optionsNotification.EnableChatter = this.m_activeState.enableChatter;
				optionsNotification.RightClickingConfirmsAbilityTargets = this.m_activeState.rightClickingConfirmsAbilityTargets;
				optionsNotification.ShiftClickForMovementWaypoints = this.m_activeState.shiftClickForMovementWaypoints;
				optionsNotification.ShowGlobalChat = this.m_activeState.showGlobalChat;
				optionsNotification.ShowAllChat = this.m_activeState.showAllChat;
				optionsNotification.EnableProfanityFilter = this.m_activeState.enableProfanityFilter;
				optionsNotification.VoiceMute = this.m_activeState.voiceMute;
				optionsNotification.VoiceVolume = this.m_activeState.voiceVolume;
				optionsNotification.MicVolume = this.m_activeState.micVolume;
				optionsNotification.VoicePushToTalk = this.m_activeState.voicePushToTalk;
				optionsNotification.AutoJoinDiscord = this.m_activeState.autoJoinDiscord;
				optionsNotification.GameModeVoiceChat = (byte)this.m_activeState.gameModeVoiceChat;
				optionsNotification.HideTutorialVideos = this.m_activeState.hideTutorialVideos;
				optionsNotification.AllowCancelActionWhileConfirmed = this.m_activeState.allowCancelActionWhileConfirmed;
				optionsNotification.Region = this.m_activeState.region;
				optionsNotification.OverrideGlyphLanguageCode = this.m_activeState.overrideGlyphLanguageCode;
				ClientGameManager.Get().NotifyOptions(optionsNotification);
			}
		}
	}

	public bool IsVisible()
	{
		return this.m_container.gameObject.activeSelf;
	}

	internal void ApplyCurrentSettings()
	{
		this.m_activeState.ApplyPendingValues(this.m_pendingState);
		this.m_activeState.ApplyToPlayerPrefs();
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
		if (!flag)
		{
			string text = Convert.ToString(num);
			if (text == value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.OnMasterVolumeChange(string)).MethodHandle;
				}
				if (num >= 0)
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
					if (num <= 0x64)
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
						this.m_pendingState.masterVolume = num;
						this.m_masterVolumeLabel.text = text;
						if (this.m_masterVolumeSlider.value != (float)num / 100f && !this.m_dontUpdateSliders)
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
							this.m_masterVolumeSlider.value = (float)num / 100f;
						}
						AudioMixer audioMixer = AudioManager.mixSnapshotManager.snapshot_game.audioMixer;
						float value2 = this.m_pendingState.ConvertPercentToDb(this.m_pendingState.masterVolume);
						audioMixer.SetFloat("VolMaster", value2);
						return;
					}
				}
			}
		}
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		if (!Application.isEditor)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.OnApplicationFocus(bool)).MethodHandle;
			}
			if (AudioManager.mixSnapshotManager != null)
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
				if (AudioManager.mixSnapshotManager.snapshot_game != null)
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
					if (AudioManager.mixSnapshotManager.snapshot_game.audioMixer != null)
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
						AudioMixer audioMixer = AudioManager.mixSnapshotManager.snapshot_game.audioMixer;
						float value = this.m_activeState.ConvertPercentToDb(this.m_activeState.musicVolume);
						int percent = 0x64 - this.m_activeState.musicVolume;
						float value2 = this.m_activeState.ConvertPercentToDb(percent);
						if (focusStatus)
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
							audioMixer.SetFloat("VolMusic", value);
							audioMixer.SetFloat("VolUIAmbiance", value2);
							audioMixer.SetFloat("VolGameAmbiance", 0f);
						}
						else
						{
							audioMixer.SetFloat("VolMusic", -80f);
							audioMixer.SetFloat("VolUIAmbiance", -80f);
							audioMixer.SetFloat("VolGameAmbiance", -80f);
						}
					}
				}
			}
		}
	}

	public void OnMasterVolumeSliderChange(float value)
	{
		int num = Mathf.RoundToInt(value * 100f);
		this.m_dontUpdateSliders = true;
		this.OnMasterVolumeChange(num.ToString());
		this.m_dontUpdateSliders = false;
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
		if (!flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.OnMusicVolumeChange(string)).MethodHandle;
			}
			string text = Convert.ToString(num);
			if (text == value)
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
				if (num >= 0)
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
					if (num <= 0x64)
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
						this.m_pendingState.musicVolume = num;
						this.m_musicVolumeLabel.text = text;
						if (this.m_musicVolumeSlider.value != (float)num / 100f)
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
							if (!this.m_dontUpdateSliders)
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
								this.m_musicVolumeSlider.value = (float)num / 100f;
							}
						}
						AudioMixer audioMixer = AudioManager.mixSnapshotManager.snapshot_game.audioMixer;
						float value2 = this.m_pendingState.ConvertPercentToDb(this.m_pendingState.musicVolume);
						audioMixer.SetFloat("VolMusic", value2);
						int percent = 0x64 - this.m_pendingState.musicVolume;
						float value3 = this.m_pendingState.ConvertPercentToDb(percent);
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
		this.m_dontUpdateSliders = true;
		this.OnMusicVolumeChange(num.ToString());
		this.m_dontUpdateSliders = false;
	}

	public void OnGraphicsQualityLow(BaseEventData data)
	{
		this.m_pendingState.graphicsQuality = GraphicsQuality.Low;
		this.m_graphicsLowButton.SetSelected(true, false, string.Empty, string.Empty);
		this.m_graphicsMediumButton.SetSelected(false, false, string.Empty, string.Empty);
		this.m_graphicsHighButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnGraphicsQualityMedium(BaseEventData data)
	{
		this.m_pendingState.graphicsQuality = GraphicsQuality.Medium;
		this.m_graphicsLowButton.SetSelected(false, false, string.Empty, string.Empty);
		this.m_graphicsMediumButton.SetSelected(true, false, string.Empty, string.Empty);
		this.m_graphicsHighButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnGraphicsQualityHigh(BaseEventData data)
	{
		this.m_pendingState.graphicsQuality = GraphicsQuality.High;
		this.m_graphicsLowButton.SetSelected(false, false, string.Empty, string.Empty);
		this.m_graphicsMediumButton.SetSelected(false, false, string.Empty, string.Empty);
		this.m_graphicsHighButton.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnWindowMode(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_windowModeDropdown, !this.m_windowModeDropdown.activeSelf, null);
	}

	public void OnWindowModeDeselect(BaseEventData data)
	{
		PointerEventData pointerEventData = (PointerEventData)data;
		if (!RectTransformUtility.RectangleContainsScreenPoint(this.m_windowModeDropdown.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera) && !RectTransformUtility.RectangleContainsScreenPoint(this.m_windowModeButton.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.OnWindowModeDeselect(BaseEventData)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_windowModeDropdown, false, null);
		}
	}

	public void SetResolutionText(string text)
	{
		for (int i = 0; i < this.m_resolutionText.Length; i++)
		{
			this.m_resolutionText[i].text = text;
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.SetResolutionText(string)).MethodHandle;
		}
	}

	public void SetWindowModeText(string text)
	{
		for (int i = 0; i < this.m_windowModeText.Length; i++)
		{
			this.m_windowModeText[i].text = text;
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.SetWindowModeText(string)).MethodHandle;
		}
	}

	public void OnWindowModeWindowed(BaseEventData data)
	{
		this.m_pendingState.windowMode = SettingsState.WindowMode.Windowed;
		this.SetWindowModeText(StringUtil.TR("Windowed", "Options"));
		UIManager.SetGameObjectActive(this.m_windowModeDropdown, false, null);
		bool flag = false;
		foreach (Resolution resolution in Screen.resolutions)
		{
			if (resolution.width == this.m_pendingState.resolutionWidth)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.OnWindowModeWindowed(BaseEventData)).MethodHandle;
				}
				if (resolution.height == this.m_pendingState.resolutionHeight)
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
					flag = true;
				}
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		if (flag)
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
			Options_UI.Get().SetResolutionText(this.m_pendingState.resolutionWidth.ToString() + " x " + this.m_pendingState.resolutionHeight.ToString());
		}
		else
		{
			Options_UI.Get().SetResolutionText(StringUtil.TR("Custom", "Options"));
		}
	}

	public void OnWindowModeFullscreen(BaseEventData data)
	{
		this.m_pendingState.windowMode = SettingsState.WindowMode.Fullscreen;
		this.SetWindowModeText(StringUtil.TR("Fullscreen", "Options"));
		UIManager.SetGameObjectActive(this.m_windowModeDropdown, false, null);
		this.m_pendingState.resolutionWidth = Screen.currentResolution.width;
		this.m_pendingState.resolutionHeight = Screen.currentResolution.height;
		this.SetResolutionText(this.m_pendingState.resolutionWidth.ToString() + " x " + this.m_pendingState.resolutionHeight.ToString());
	}

	public void OnResolution(BaseEventData data)
	{
		if (!this.m_resolutionDropdown.activeSelf)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.OnResolution(BaseEventData)).MethodHandle;
			}
			this.PopulateResolutionDropdown();
		}
		UIManager.SetGameObjectActive(this.m_resolutionDropdown, !this.m_resolutionDropdown.activeSelf, null);
	}

	public void OnResolutionDeselect(BaseEventData data)
	{
		PointerEventData pointerEventData = (PointerEventData)data;
		if (!RectTransformUtility.RectangleContainsScreenPoint(this.m_ResolutionScrollView.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.OnResolutionDeselect(BaseEventData)).MethodHandle;
			}
			if (!RectTransformUtility.RectangleContainsScreenPoint(this.m_resolutionButton.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
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
				if (!RectTransformUtility.RectangleContainsScreenPoint(this.m_ResolutionScrollView.verticalScrollbar.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
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
					UIManager.SetGameObjectActive(this.m_resolutionDropdown, false, null);
				}
			}
		}
	}

	public void OnGameWindowMode(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_gameWindowModeDropdown, !this.m_gameWindowModeDropdown.activeSelf, null);
	}

	public void OnGameWindowModeDeselect(BaseEventData data)
	{
		PointerEventData pointerEventData = (PointerEventData)data;
		if (!RectTransformUtility.RectangleContainsScreenPoint(this.m_gameWindowModeDropdown.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.OnGameWindowModeDeselect(BaseEventData)).MethodHandle;
			}
			if (!RectTransformUtility.RectangleContainsScreenPoint(this.m_gameWindowModeButton.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
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
				UIManager.SetGameObjectActive(this.m_gameWindowModeDropdown, false, null);
			}
		}
	}

	public void SetGameResolutionText(string text)
	{
		for (int i = 0; i < this.m_gameResolutionText.Length; i++)
		{
			this.m_gameResolutionText[i].text = text;
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.SetGameResolutionText(string)).MethodHandle;
		}
	}

	public void SetGameWindowModeText(string text)
	{
		for (int i = 0; i < this.m_gameWindowModeText.Length; i++)
		{
			this.m_gameWindowModeText[i].text = text;
		}
	}

	public void OnGameWindowModeInherit(BaseEventData data)
	{
		this.m_pendingState.gameWindowMode = SettingsState.WindowMode.Inherit;
		this.SetGameWindowModeText(StringUtil.TR("Same as Menu", "Options"));
		UIManager.SetGameObjectActive(this.m_gameWindowModeDropdown, false, null);
		this.m_pendingState.gameResolutionWidth = -1;
		this.m_pendingState.gameResolutionHeight = -1;
		Options_UI.Get().SetGameResolutionText(StringUtil.TR("Same as Menu", "Options"));
	}

	public void OnGameWindowModeWindowed(BaseEventData data)
	{
		this.m_pendingState.gameWindowMode = SettingsState.WindowMode.Windowed;
		this.SetGameWindowModeText(StringUtil.TR("Windowed", "Options"));
		UIManager.SetGameObjectActive(this.m_gameWindowModeDropdown, false, null);
		bool flag = false;
		foreach (Resolution resolution in Screen.resolutions)
		{
			if (resolution.width == this.m_pendingState.gameResolutionWidth)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.OnGameWindowModeWindowed(BaseEventData)).MethodHandle;
				}
				if (resolution.height == this.m_pendingState.gameResolutionHeight)
				{
					flag = true;
				}
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		if (flag)
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
			Options_UI.Get().SetGameResolutionText(this.m_pendingState.gameResolutionWidth.ToString() + " x " + this.m_pendingState.gameResolutionHeight.ToString());
		}
		else
		{
			Options_UI.Get().SetGameResolutionText(StringUtil.TR("Custom", "Options"));
		}
	}

	public void OnGameWindowModeFullscreen(BaseEventData data)
	{
		this.m_pendingState.gameWindowMode = SettingsState.WindowMode.Fullscreen;
		this.SetGameWindowModeText(StringUtil.TR("Fullscreen", "Options"));
		UIManager.SetGameObjectActive(this.m_gameWindowModeDropdown, false, null);
		this.m_pendingState.gameResolutionWidth = Screen.currentResolution.width;
		this.m_pendingState.gameResolutionHeight = Screen.currentResolution.height;
		this.SetGameResolutionText(this.m_pendingState.gameResolutionWidth.ToString() + " x " + this.m_pendingState.gameResolutionHeight.ToString());
	}

	public void OnGameResolution(BaseEventData data)
	{
		if (!this.m_gameResolutionDropdown.activeSelf)
		{
			this.PopulateGameResolutionDropdown();
		}
		UIManager.SetGameObjectActive(this.m_gameResolutionDropdown, !this.m_gameResolutionDropdown.activeSelf, null);
	}

	public void OnGameResolutionDeselect(BaseEventData data)
	{
		PointerEventData pointerEventData = (PointerEventData)data;
		if (!RectTransformUtility.RectangleContainsScreenPoint(this.m_gameResolutionScrollView.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera) && !RectTransformUtility.RectangleContainsScreenPoint(this.m_gameResolutionButton.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera) && !RectTransformUtility.RectangleContainsScreenPoint(this.m_gameResolutionScrollView.verticalScrollbar.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.OnGameResolutionDeselect(BaseEventData)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_gameResolutionDropdown, false, null);
		}
	}

	public void OnRegion(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_regionDropdown, !this.m_regionDropdown.activeSelf, null);
	}

	public void OnRegionDeselect(BaseEventData data)
	{
		PointerEventData pointerEventData = (PointerEventData)data;
		if (!RectTransformUtility.RectangleContainsScreenPoint(this.m_regionDropdown.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.OnRegionDeselect(BaseEventData)).MethodHandle;
			}
			if (!RectTransformUtility.RectangleContainsScreenPoint(this.m_regionButton.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
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
				UIManager.SetGameObjectActive(this.m_regionDropdown, false, null);
			}
		}
	}

	public void SetRegionText(string text)
	{
		for (int i = 0; i < this.m_regionText.Length; i++)
		{
			this.m_regionText[i].text = text;
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.SetRegionText(string)).MethodHandle;
		}
	}

	public void OnLanguage(BaseEventData data)
	{
		if (!this.m_languageDropdown.activeSelf)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.OnLanguage(BaseEventData)).MethodHandle;
			}
			this.PopulateLanguageDropdown();
		}
		UIManager.SetGameObjectActive(this.m_languageDropdown, !this.m_languageDropdown.activeSelf, null);
	}

	public void OnLanguageDeselect(BaseEventData data)
	{
		PointerEventData pointerEventData = (PointerEventData)data;
		if (!RectTransformUtility.RectangleContainsScreenPoint(this.m_LanguageScrollView.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera) && !RectTransformUtility.RectangleContainsScreenPoint(this.m_languageButton.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera) && !RectTransformUtility.RectangleContainsScreenPoint(this.m_LanguageScrollView.verticalScrollbar.transform as RectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.OnLanguageDeselect(BaseEventData)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_languageDropdown, false, null);
		}
	}

	public void SetLanguageText(string text)
	{
		for (int i = 0; i < this.m_languageText.Length; i++)
		{
			this.m_languageText[i].text = text;
		}
	}

	public void OnRegionNorthAmerica(BaseEventData data)
	{
		this.m_pendingState.region = Region.US;
		this.SetRegionText(StringUtil.TR("NorthAmerica", "Options"));
		UIManager.SetGameObjectActive(this.m_regionDropdown, false, null);
	}

	public void OnRegionEurope(BaseEventData data)
	{
		this.m_pendingState.region = Region.EU;
		this.SetRegionText(StringUtil.TR("Europe", "Options"));
		UIManager.SetGameObjectActive(this.m_regionDropdown, false, null);
	}

	public void OnLockCursor(BaseEventData data)
	{
		this.m_pendingState.lockCursorMode = SettingsState.LockCursorMode.On;
		this.m_lockCursorButton.SetSelected(true, false, string.Empty, string.Empty);
		this.m_unlockCursorButton.SetSelected(false, false, string.Empty, string.Empty);
		this.m_smartLockCursorButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnUnlockCursor(BaseEventData data)
	{
		this.m_pendingState.lockCursorMode = SettingsState.LockCursorMode.Off;
		this.m_lockCursorButton.SetSelected(false, false, string.Empty, string.Empty);
		this.m_unlockCursorButton.SetSelected(true, false, string.Empty, string.Empty);
		this.m_smartLockCursorButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnSmartLockCursor(BaseEventData data)
	{
		this.m_pendingState.lockCursorMode = SettingsState.LockCursorMode.Smart;
		this.m_lockCursorButton.SetSelected(false, false, string.Empty, string.Empty);
		this.m_unlockCursorButton.SetSelected(false, false, string.Empty, string.Empty);
		this.m_smartLockCursorButton.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnEnableChatter(BaseEventData data)
	{
		this.m_pendingState.enableChatter = true;
		this.m_enableChatterButton.SetSelected(true, false, string.Empty, string.Empty);
		this.m_disableChatterButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnShowGlobalChat(BaseEventData data)
	{
		this.m_pendingState.showGlobalChat = true;
		this.m_showGlobalChatButton.SetSelected(true, false, string.Empty, string.Empty);
		this.m_hideGlobalChatButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnHideGlobalChat(BaseEventData data)
	{
		this.m_pendingState.showGlobalChat = false;
		this.m_showGlobalChatButton.SetSelected(false, false, string.Empty, string.Empty);
		this.m_hideGlobalChatButton.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnShowAllChat(BaseEventData data)
	{
		this.m_pendingState.showAllChat = true;
		this.m_showAllChatButton.SetSelected(true, false, string.Empty, string.Empty);
		this.m_hideAllChatButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnHideAllChat(BaseEventData data)
	{
		this.m_pendingState.showAllChat = false;
		this.m_showAllChatButton.SetSelected(false, false, string.Empty, string.Empty);
		this.m_hideAllChatButton.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnEnableProfanityFilter(BaseEventData data)
	{
		this.m_pendingState.enableProfanityFilter = true;
		this.m_enableProfanityFilterButton.SetSelected(true, false, string.Empty, string.Empty);
		this.m_disableProfanityFilterButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnDisableProfanityFilter(BaseEventData data)
	{
		this.m_pendingState.enableProfanityFilter = false;
		this.m_enableProfanityFilterButton.SetSelected(false, false, string.Empty, string.Empty);
		this.m_disableProfanityFilterButton.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnEnableAutoJoinDiscord(BaseEventData data)
	{
		this.m_pendingState.autoJoinDiscord = true;
		this.m_enableAutoJoinDiscordButton.SetSelected(true, false, string.Empty, string.Empty);
		this.m_disableAutoJoinDiscordButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnDisableAutoJoinDiscord(BaseEventData data)
	{
		this.m_pendingState.autoJoinDiscord = false;
		this.m_enableAutoJoinDiscordButton.SetSelected(false, false, string.Empty, string.Empty);
		this.m_disableAutoJoinDiscordButton.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnGroupGameModeVoiceChat(BaseEventData data)
	{
		this.m_pendingState.gameModeVoiceChat = SettingsState.VoiceChatMode.Group;
		this.m_groupGameModeVoiceChatButton.SetSelected(true, false, string.Empty, string.Empty);
		this.m_teamGameModeVoiceChatButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnTeamGameModeVoiceChat(BaseEventData data)
	{
		this.m_pendingState.gameModeVoiceChat = SettingsState.VoiceChatMode.Team;
		this.m_groupGameModeVoiceChatButton.SetSelected(false, false, string.Empty, string.Empty);
		this.m_teamGameModeVoiceChatButton.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnDisableChatter(BaseEventData data)
	{
		this.m_pendingState.enableChatter = false;
		this.m_enableChatterButton.SetSelected(false, false, string.Empty, string.Empty);
		this.m_disableChatterButton.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnRightClickConfirm(BaseEventData data)
	{
		this.m_pendingState.rightClickingConfirmsAbilityTargets = true;
		this.m_rightClickTargetingConfirm.SetSelected(true, false, string.Empty, string.Empty);
		this.m_rightClickTargetingCancel.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnRightClickCancel(BaseEventData data)
	{
		this.m_pendingState.rightClickingConfirmsAbilityTargets = false;
		this.m_rightClickTargetingConfirm.SetSelected(false, false, string.Empty, string.Empty);
		this.m_rightClickTargetingCancel.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnShiftClickWaypointsConfirm(BaseEventData data)
	{
		this.m_pendingState.shiftClickForMovementWaypoints = true;
		this.m_shiftClickForWaypoints.SetSelected(true, false, string.Empty, string.Empty);
		this.m_shiftClickForNewPath.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnShiftClickWaypointsCancel(BaseEventData data)
	{
		this.m_pendingState.shiftClickForMovementWaypoints = false;
		this.m_shiftClickForWaypoints.SetSelected(false, false, string.Empty, string.Empty);
		this.m_shiftClickForNewPath.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnHideTutorialVideos(BaseEventData data)
	{
		this.m_pendingState.hideTutorialVideos = true;
		this.m_hideTutorialVideosButton.SetSelected(true, false, string.Empty, string.Empty);
		this.m_showTutorialVideosButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnShowTutorialVideos(BaseEventData data)
	{
		this.m_pendingState.hideTutorialVideos = false;
		this.m_hideTutorialVideosButton.SetSelected(false, false, string.Empty, string.Empty);
		this.m_showTutorialVideosButton.SetSelected(true, false, string.Empty, string.Empty);
	}

	public void OnAllowCancelActionWhileConfirmed(BaseEventData data)
	{
		this.m_pendingState.allowCancelActionWhileConfirmed = true;
		this.m_allowCancelActionWhileConfirmedButton.SetSelected(true, false, string.Empty, string.Empty);
		this.m_disallowCancelActionWhileConfirmedButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void OnDisallowCancelActionWhileConfirmed(BaseEventData data)
	{
		this.m_pendingState.allowCancelActionWhileConfirmed = false;
		this.m_allowCancelActionWhileConfirmedButton.SetSelected(false, false, string.Empty, string.Empty);
		this.m_disallowCancelActionWhileConfirmedButton.SetSelected(true, false, string.Empty, string.Empty);
	}

	private float ResolutionRound(float f, float digits)
	{
		float num = Mathf.Pow(10f, digits);
		return Mathf.Round(f * num) / num;
	}

	private void PopulateResolutionDropdown()
	{
		this.PopulateGeneralResolutionDropdown(this.m_ResolutionScrollView, this.m_resolutionItemContainer, this.m_pendingState.windowMode, new _ButtonSwapSprite.ButtonClickCallback(this.OnSpecificResolution), this.m_activeState.resolutionWidth, this.m_activeState.resolutionHeight);
	}

	private void PopulateGameResolutionDropdown()
	{
		this.PopulateGeneralResolutionDropdown(this.m_gameResolutionScrollView, this.m_gameResolutionItemContainer, this.m_pendingState.gameWindowMode, new _ButtonSwapSprite.ButtonClickCallback(this.OnSpecificGameResolution), this.m_activeState.resolutionWidth, this.m_activeState.resolutionHeight);
	}

	private void PopulateGeneralResolutionDropdown(ScrollRect scrollView, GridLayoutGroup itemContainer, SettingsState.WindowMode pendingWindowMode, _ButtonSwapSprite.ButtonClickCallback changeCallback, int currentWidth, int currentHeight)
	{
		foreach (GameObject obj in this.m_resolutionButtons)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.m_resolutionButtons.Clear();
		int num = 0;
		int num2 = 1;
		IEnumerable<Resolution> resolutions = Screen.resolutions;
		if (Options_UI.<>f__am$cache0 == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.PopulateGeneralResolutionDropdown(ScrollRect, GridLayoutGroup, SettingsState.WindowMode, _ButtonSwapSprite.ButtonClickCallback, int, int)).MethodHandle;
			}
			Options_UI.<>f__am$cache0 = ((Resolution r) => r.width);
		}
		IOrderedEnumerable<Resolution> source = resolutions.OrderByDescending(Options_UI.<>f__am$cache0);
		if (Options_UI.<>f__am$cache1 == null)
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
			Options_UI.<>f__am$cache1 = ((Resolution r) => r.height);
		}
		Resolution[] array = source.ThenByDescending(Options_UI.<>f__am$cache1).ToArray<Resolution>();
		bool flag = false;
		foreach (Resolution resolution in array)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Options_UI.Get().m_resolutionButtonPrefab);
			gameObject.transform.SetParent(itemContainer.transform, false);
			num2++;
			RectTransform rectTransform = gameObject.transform as RectTransform;
			Vector2 anchoredPosition = rectTransform.anchoredPosition;
			anchoredPosition.y = (float)(-0x14 + -0x1E * num);
			rectTransform.anchoredPosition = anchoredPosition;
			TextMeshProUGUI[] componentsInChildren = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].text = resolution.width.ToString() + " x " + resolution.height.ToString();
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
			this.m_resolutionButtons.Add(gameObject);
			UIManager.SetGameObjectActive(gameObject, true, null);
			this.RegisterResolutionSetting(gameObject, new Options_UI.ResolutionSetting
			{
				resolution = resolution,
				custom = false
			});
			_SelectableBtn component = gameObject.GetComponent<_SelectableBtn>();
			component.spriteController.callback = changeCallback;
			component.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
			if (scrollView != null)
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
				_MouseEventPasser mouseEventPasser = component.spriteController.gameObject.AddComponent<_MouseEventPasser>();
				mouseEventPasser.AddNewHandler(scrollView);
			}
			if (currentWidth == resolution.width && currentHeight == resolution.height)
			{
				flag = true;
			}
			num++;
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
		if (pendingWindowMode == SettingsState.WindowMode.Windowed && !flag)
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
			if (currentWidth > 0)
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
				if (currentHeight > 0)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(Options_UI.Get().m_resolutionButtonPrefab);
					gameObject2.transform.SetParent(itemContainer.transform, false);
					num2++;
					RectTransform rectTransform2 = gameObject2.transform as RectTransform;
					Vector2 anchoredPosition2 = rectTransform2.anchoredPosition;
					anchoredPosition2.y = (float)(-0x14 + -0x1E * num);
					rectTransform2.anchoredPosition = anchoredPosition2;
					TextMeshProUGUI[] componentsInChildren2 = gameObject2.GetComponentsInChildren<TextMeshProUGUI>();
					for (int k = 0; k < componentsInChildren2.Length; k++)
					{
						componentsInChildren2[k].text = StringUtil.TR("Custom", "Options");
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_resolutionButtons.Add(gameObject2);
					UIManager.SetGameObjectActive(gameObject2, true, null);
					Options_UI.ResolutionSetting setting = default(Options_UI.ResolutionSetting);
					setting.resolution.width = this.m_activeState.resolutionWidth;
					setting.resolution.height = this.m_activeState.resolutionHeight;
					setting.custom = true;
					this.RegisterResolutionSetting(gameObject2, setting);
					_SelectableBtn component2 = gameObject2.GetComponent<_SelectableBtn>();
					component2.spriteController.callback = changeCallback;
					component2.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
					if (scrollView != null)
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
						_MouseEventPasser mouseEventPasser2 = component2.spriteController.gameObject.AddComponent<_MouseEventPasser>();
						mouseEventPasser2.AddNewHandler(scrollView);
					}
					num++;
				}
			}
		}
		RectTransform rectTransform3 = itemContainer.transform as RectTransform;
		rectTransform3.sizeDelta = new Vector2(rectTransform3.sizeDelta.x, itemContainer.cellSize.y * (float)num2);
	}

	private void PopulateLanguageDropdown()
	{
		using (List<GameObject>.Enumerator enumerator = this.m_languageButtons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject obj = enumerator.Current;
				UnityEngine.Object.Destroy(obj);
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.PopulateLanguageDropdown()).MethodHandle;
			}
		}
		this.m_languageButtons.Clear();
		for (int i = 0; i < 0xB; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Options_UI.Get().m_resolutionButtonPrefab);
			gameObject.transform.SetParent(this.m_languageItemContainer.transform, false);
			RectTransform rectTransform = gameObject.transform as RectTransform;
			Vector2 anchoredPosition = rectTransform.anchoredPosition;
			anchoredPosition.y = (float)(-0x14 + -0x1E * i);
			rectTransform.anchoredPosition = anchoredPosition;
			foreach (TextMeshProUGUI tmp_Text in gameObject.GetComponentsInChildren<TextMeshProUGUI>())
			{
				LanguageOptions languageOptions = (LanguageOptions)i;
				tmp_Text.text = StringUtil.TR(languageOptions.ToString(), "LanguageSelection");
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_languageButtons.Add(gameObject);
			UIManager.SetGameObjectActive(gameObject, true, null);
			_SelectableBtn component = gameObject.GetComponent<_SelectableBtn>();
			component.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnSpecificLanguage);
			component.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
			if (this.m_LanguageScrollView != null)
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
				_MouseEventPasser mouseEventPasser = component.spriteController.gameObject.AddComponent<_MouseEventPasser>();
				mouseEventPasser.AddNewHandler(this.m_LanguageScrollView);
			}
		}
		RectTransform rectTransform2 = this.m_languageItemContainer.transform as RectTransform;
		rectTransform2.sizeDelta = new Vector2(rectTransform2.sizeDelta.x, this.m_languageItemContainer.cellSize.y * 11f);
	}

	private void RegisterResolutionSetting(GameObject obj, Options_UI.ResolutionSetting setting)
	{
		this.m_resolutionButtonData[obj.GetComponent<_SelectableBtn>().spriteController.gameObject] = setting;
	}

	public void OnSpecificGameResolution(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_gameResolutionDropdown, false, null);
		if (!this.m_resolutionButtonData.ContainsKey(data.selectedObject))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.OnSpecificGameResolution(BaseEventData)).MethodHandle;
			}
			return;
		}
		Options_UI.ResolutionSetting resolutionSetting = this.m_resolutionButtonData[data.selectedObject];
		this.m_pendingState.gameResolutionWidth = resolutionSetting.resolution.width;
		this.m_pendingState.gameResolutionHeight = resolutionSetting.resolution.height;
		if (resolutionSetting.custom)
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
			this.SetGameResolutionText(StringUtil.TR("Custom", "Options"));
		}
		else
		{
			this.SetGameResolutionText(resolutionSetting.resolution.width.ToString() + " x " + resolutionSetting.resolution.height.ToString());
		}
	}

	public void OnSpecificResolution(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_resolutionDropdown, false, null);
		if (!this.m_resolutionButtonData.ContainsKey(data.selectedObject))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.OnSpecificResolution(BaseEventData)).MethodHandle;
			}
			return;
		}
		Options_UI.ResolutionSetting resolutionSetting = this.m_resolutionButtonData[data.selectedObject];
		this.m_pendingState.resolutionWidth = resolutionSetting.resolution.width;
		this.m_pendingState.resolutionHeight = resolutionSetting.resolution.height;
		if (resolutionSetting.custom)
		{
			this.SetResolutionText(StringUtil.TR("Custom", "Options"));
		}
		else
		{
			this.SetResolutionText(resolutionSetting.resolution.width.ToString() + " x " + resolutionSetting.resolution.height.ToString());
		}
	}

	public void OnSpecificLanguage(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_languageDropdown, false, null);
		int num = 0;
		using (List<GameObject>.Enumerator enumerator = this.m_languageButtons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject gameObject = enumerator.Current;
				if (gameObject.GetComponent<_SelectableBtn>().spriteController.gameObject == data.selectedObject)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.OnSpecificLanguage(BaseEventData)).MethodHandle;
					}
					LanguageOptions languageOptions = (LanguageOptions)num;
					if (languageOptions == LanguageOptions.GlyphSettings)
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
						this.m_pendingState.overrideGlyphLanguage = false;
						this.m_pendingState.overrideGlyphLanguageCode = string.Empty;
					}
					else
					{
						this.m_pendingState.overrideGlyphLanguage = true;
						this.m_pendingState.overrideGlyphLanguageCode = languageOptions.ToString();
					}
					LanguageOptions languageOptions2 = (LanguageOptions)num;
					this.SetLanguageText(StringUtil.TR(languageOptions2.ToString(), "LanguageSelection"));
				}
				num++;
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
		UIManager.SetGameObjectActive(this.m_restartWarning, true, null);
	}

	public GraphicsQuality GetCurrentGraphicsQuality()
	{
		return this.m_activeState.graphicsQuality;
	}

	public void SetPendingGraphicsQuality(GraphicsQuality graphicsQuality)
	{
		this.m_pendingState.graphicsQuality = graphicsQuality;
	}

	public bool GetGraphicsQualityEverSetManually()
	{
		return PlayerPrefs.GetInt("OptionsGraphicsQualityEverSetManually") != 0;
	}

	public bool GetChatterEnabled()
	{
		return this.m_activeState.enableChatter;
	}

	public bool GetRightClickingConfirmsAbilityTargets()
	{
		return this.m_activeState.rightClickingConfirmsAbilityTargets;
	}

	public bool GetShiftClickForMovementWaypoints()
	{
		return this.m_activeState.shiftClickForMovementWaypoints;
	}

	public bool GetShowGlobalChat()
	{
		return this.m_activeState.showGlobalChat;
	}

	public void SetShowGlobalChat(bool show)
	{
		this.m_activeState.showGlobalChat = show;
		this.m_showGlobalChatButton.SetSelected(show, false, string.Empty, string.Empty);
		this.m_hideGlobalChatButton.SetSelected(!show, false, string.Empty, string.Empty);
	}

	public bool GetShowAllChat()
	{
		return this.m_activeState.showAllChat;
	}

	public void SetShowAllChat(bool show)
	{
		this.m_activeState.showAllChat = show;
		this.m_showAllChatButton.SetSelected(show, false, string.Empty, string.Empty);
		this.m_hideAllChatButton.SetSelected(!show, false, string.Empty, string.Empty);
	}

	public bool GetEnableProfanityFilter()
	{
		return this.m_activeState.enableProfanityFilter;
	}

	public void SetEnableProfanityFilter(bool show)
	{
		this.m_activeState.enableProfanityFilter = show;
		this.m_enableProfanityFilterButton.SetSelected(show, false, string.Empty, string.Empty);
		this.m_disableProfanityFilterButton.SetSelected(!show, false, string.Empty, string.Empty);
	}

	public bool GetEnableAutoJoinDiscord()
	{
		return this.m_activeState.autoJoinDiscord;
	}

	public void SetEnableAutoJoinDiscord(bool show)
	{
		this.m_activeState.autoJoinDiscord = show;
		this.m_enableAutoJoinDiscordButton.SetSelected(show, false, string.Empty, string.Empty);
		this.m_disableAutoJoinDiscordButton.SetSelected(!show, false, string.Empty, string.Empty);
	}

	public bool GetVoicePushToTalk()
	{
		return this.m_activeState.voicePushToTalk;
	}

	public void SetVoicePushToTalk(bool pushToTalk)
	{
		this.m_activeState.voicePushToTalk = pushToTalk;
	}

	public bool GetVoiceMute()
	{
		return this.m_activeState.voiceMute;
	}

	public void SetVoiceMute(bool mute)
	{
		this.m_activeState.voiceMute = mute;
	}

	public float GetVoiceVolume()
	{
		return this.m_activeState.voiceVolume;
	}

	public void SetVoiceVolume(float volume)
	{
		this.m_activeState.voiceVolume = volume;
	}

	public float GetMicVolume()
	{
		return this.m_activeState.micVolume;
	}

	public void SetMicVolume(float volume)
	{
		this.m_activeState.micVolume = volume;
	}

	public static Region GetRegion()
	{
		if (Options_UI.s_instance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.GetRegion()).MethodHandle;
			}
			return Options_UI.s_instance.m_activeState.region;
		}
		return (Region)PlayerPrefs.GetInt("Region", (int)Options_UI.GetDefaultRegion());
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.GetDefaultRegion()).MethodHandle;
			}
			if (totalHours < num2)
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
				return Region.EU;
			}
		}
		return Region.US;
	}

	public SettingsState.VoiceChatMode GetGameModeVoiceChat()
	{
		return this.m_activeState.gameModeVoiceChat;
	}

	public void SetGameModeVoiceChat(SettingsState.VoiceChatMode setting)
	{
		this.m_activeState.gameModeVoiceChat = setting;
		if (setting != SettingsState.VoiceChatMode.None)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Options_UI.SetGameModeVoiceChat(SettingsState.VoiceChatMode)).MethodHandle;
			}
			if (setting != SettingsState.VoiceChatMode.Group)
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
				if (setting != SettingsState.VoiceChatMode.Team)
				{
					return;
				}
				this.m_groupGameModeVoiceChatButton.SetSelected(false, false, string.Empty, string.Empty);
				this.m_teamGameModeVoiceChatButton.SetSelected(true, false, string.Empty, string.Empty);
				return;
			}
		}
		this.m_groupGameModeVoiceChatButton.SetSelected(true, false, string.Empty, string.Empty);
		this.m_teamGameModeVoiceChatButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	public bool GetShowTutorialVideos()
	{
		return !this.m_activeState.hideTutorialVideos;
	}

	public bool ShouldCancelActionWhileConfirmed()
	{
		return this.m_activeState.allowCancelActionWhileConfirmed;
	}

	private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

	private struct ResolutionSetting
	{
		public Resolution resolution;

		public bool custom;
	}
}
