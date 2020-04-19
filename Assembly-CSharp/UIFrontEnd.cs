using System;
using LobbyGameClientMessages;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIFrontEnd : MonoBehaviour
{
	public bool m_enableBuyButtons;

	public float m_baseRingMovingSpeed = 0.75f;

	public float m_cameraMovementSpeed = 1f;

	public float m_cameraRotationSpeed = 5f;

	public float m_panelMovementSpeed = 1f;

	public float m_whisperSoundThreshold = 5f;

	public Camera m_HUDPanelCamera;

	public CameraPositions[] m_cameraPositions;

	public FrontEndScreenState m_currentScreen;

	public CanvasGroup[] m_frontendCanvasContainers;

	public static int s_firstLogInQuestCount = -1;

	private static UIFrontEnd s_instance;

	private Vector3 m_lookAtOffset;

	private float m_rotationStartLength;

	private float m_charCamStartTime;

	private FrontEndScene m_currentCameraPosition;

	private bool m_isStartDrag;

	private bool m_isDragging;

	private bool m_justStoppedDragging;

	private Vector3 m_currentRotationOffset;

	private Vector3 m_startDragPosition;

	private Vector3 m_startRotation;

	private Vector3 m_lastMouseLocation;

	private bool m_isVisible;

	private bool m_attachedHandler;

	public FrontEndNavPanel m_frontEndNavPanel
	{
		get
		{
			return FrontEndNavPanel.Get();
		}
	}

	public UIPlayerNavPanel m_playerPanel
	{
		get
		{
			return UIPlayerNavPanel.Get();
		}
	}

	public UILandingPageScreen m_landingPageScreen
	{
		get
		{
			return UILandingPageScreen.Get();
		}
	}

	public UIJoinGameScreen m_joinGameScreen
	{
		get
		{
			return UIJoinGameScreen.Get();
		}
	}

	public UICreateGameScreen m_createGameScreen
	{
		get
		{
			return UICreateGameScreen.Get();
		}
	}

	public UITextConsole m_frontEndChatConsole
	{
		get
		{
			return UIChatBox.GetChatBox(UIManager.ClientState.InFrontEnd);
		}
	}

	public static UIFrontEnd Get()
	{
		return UIFrontEnd.s_instance;
	}

	public static void PlaySound(FrontEndButtonSounds sound)
	{
		switch (sound)
		{
		case FrontEndButtonSounds.Back:
			AudioManager.PostEvent("ui/frontend/v1/btn/back", null);
			break;
		case FrontEndButtonSounds.Cancel:
			AudioManager.PostEvent("ui/frontend/v1/btn/cancel", null);
			break;
		case FrontEndButtonSounds.Generic:
			AudioManager.PostEvent("ui/frontend/v1/btn/generic", null);
			break;
		case FrontEndButtonSounds.GenericSmall:
			AudioManager.PostEvent("ui/frontend/v1/btn/generic_small", null);
			break;
		case FrontEndButtonSounds.StartGameReady:
			AudioManager.PostEvent("ui/frontend/btn_ready_start_game", null);
			break;
		case FrontEndButtonSounds.SelectChoice:
			AudioManager.PostEvent("ui/frontend/v1/notify/teammember_select", null);
			break;
		case FrontEndButtonSounds.SelectColorChoice:
			AudioManager.PostEvent("ui/frontend/v1/notify/teammember_select", null);
			break;
		case FrontEndButtonSounds.CharacterSelectCharacter:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/character", null);
			break;
		case FrontEndButtonSounds.CharacterSelectClose:
			AudioManager.PostEvent("ui/frontend/v1/wnd/charselect/close", null);
			break;
		case FrontEndButtonSounds.CharacterSelectOpen:
			AudioManager.PostEvent("ui/frontend/v1/btn/menu/submenu/open", null);
			AudioManager.PostEvent("ui/frontend/v1/wnd/charselect/open", null);
			break;
		case FrontEndButtonSounds.CharacterSelectOptions:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/options", null);
			break;
		case FrontEndButtonSounds.CharacterSelectOptionsChoice:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/options/choice", null);
			break;
		case FrontEndButtonSounds.CharacterSelectModAdd:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add", null);
			break;
		case FrontEndButtonSounds.CharacterSelectModUnlocked:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/unlock", null);
			break;
		case FrontEndButtonSounds.CharacterSelectModClear:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/clear", null);
			break;
		case FrontEndButtonSounds.CharacterSelectNotifyCharLoaded:
			AudioManager.PostEvent("ui/frontend/v1/notify/charselect/loaded", null);
			break;
		case FrontEndButtonSounds.CharacterSelectSkinChoice:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add", null);
			break;
		case FrontEndButtonSounds.StoreCurrencySelect:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add", null);
			break;
		case FrontEndButtonSounds.StoreGGPackSelect:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add", null);
			break;
		case FrontEndButtonSounds.StorePurchased:
			AudioManager.PostEvent("ui/frontend/v1/btn/menu/choice", null);
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add", null);
			break;
		case FrontEndButtonSounds.GameModeSelect:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01", null);
			AudioManager.PostEvent("ui/frontend/v1/btn/gamemode_select", null);
			break;
		case FrontEndButtonSounds.OptionsOK:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/button", null);
			break;
		case FrontEndButtonSounds.OptionsCancel:
			AudioManager.PostEvent("ui/frontend/v1/btn/options/cancel", null);
			break;
		case FrontEndButtonSounds.OptionsChoice:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01", null);
			break;
		case FrontEndButtonSounds.MenuOpen:
			AudioManager.PostEvent("ui/frontend/v1/btn/menu/open", null);
			break;
		case FrontEndButtonSounds.MenuChoice:
			AudioManager.PostEvent("ui/frontend/v1/btn/menu/choice", null);
			break;
		case FrontEndButtonSounds.SubMenuOpen:
			AudioManager.PostEvent("ui/frontend/v1/btn/menu/submenu/open", null);
			break;
		case FrontEndButtonSounds.Close:
			AudioManager.PostEvent("ui/frontend/v1/btn/close", null);
			break;
		case FrontEndButtonSounds.TeamMemberSelect:
			AudioManager.PostEvent("ui/frontend/v1/notify/teammember_select", null);
			break;
		case FrontEndButtonSounds.TeamMemberCancel:
			AudioManager.PostEvent("ui/frontend/v1/notify/teammember_cancel", null);
			break;
		case FrontEndButtonSounds.MainMenuOpen:
			AudioManager.PostEvent("ui/frontend/v2/menu/mainmenu_open", null);
			break;
		case FrontEndButtonSounds.MainMenuClose:
			AudioManager.PostEvent("ui/frontend/v2/menu/mainmenu_close", null);
			break;
		case FrontEndButtonSounds.PlayCategorySelect:
			AudioManager.PostEvent("ui/frontend/v2/menu/tab/selection", null);
			break;
		case FrontEndButtonSounds.TutorialPhaseIconAppear:
			AudioManager.PostEvent("ui/frontend/v2/menu/tutorial/phase/icon/appear", null);
			break;
		case FrontEndButtonSounds.TutorialPhaseIconImpact:
			AudioManager.PostEvent("ui/frontend/v2/menu/tutorial/phase/icon/impact", null);
			break;
		case FrontEndButtonSounds.TutorialPhaseIconHighlight:
			AudioManager.PostEvent("ui/frontend/v2/menu/tutorial/phase/icon/highlight", null);
			break;
		case FrontEndButtonSounds.DialogBoxOpen:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/window/appear", null);
			break;
		case FrontEndButtonSounds.DialogBoxButton:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/button", null);
			break;
		case FrontEndButtonSounds.GGButtonInGameUsed:
			AudioManager.PostEvent("ui/ingame/ggboost_button", null);
			break;
		case FrontEndButtonSounds.GGButtonEndGameUsed:
			AudioManager.PostEvent("ui/endgame/ggboost_button", null);
			break;
		case FrontEndButtonSounds.NotifyWarning:
			AudioManager.PostEvent("ui/frontend/v2/notify/warning", null);
			break;
		case FrontEndButtonSounds.TopMenuSelect:
			AudioManager.PostEvent("ui/frontend/v1/btn/menu/choice", null);
			break;
		case FrontEndButtonSounds.NotifyMatchFound:
			AudioManager.PostEvent("ui/frontend/v1/notify/match_found", null);
			break;
		case FrontEndButtonSounds.GGPackUsedNotification:
			AudioManager.PostEvent("ui/ingame/ggboost_button", null);
			break;
		case FrontEndButtonSounds.WhisperMessage:
			AudioManager.PostEvent("ui/frontend/v1/chat/whisper_notify", null);
			break;
		case FrontEndButtonSounds.LockboxAppear:
			AudioManager.PostEvent("ui/lockbox/appear", null);
			break;
		case FrontEndButtonSounds.LockboxHit:
			AudioManager.PostEvent("ui/lockbox/hit", null);
			break;
		case FrontEndButtonSounds.LockboxUnlock:
			AudioManager.PostEvent("ui/lockbox/unlock", null);
			break;
		case FrontEndButtonSounds.InventoryCraftClick:
			AudioManager.PostEvent("ui_btn_menu_click", null);
			break;
		case FrontEndButtonSounds.InventorySalvage:
			AudioManager.PostEvent("ui/lockbox/salvage", null);
			break;
		case FrontEndButtonSounds.LockboxSelect:
			AudioManager.PostEvent("ui/frontend/v1/btn/menu/choice", null);
			break;
		case FrontEndButtonSounds.InventoryFilterSelect:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01", null);
			break;
		case FrontEndButtonSounds.CraftButtonClick:
			AudioManager.PostEvent("ui_btn_menu_click", null);
			break;
		case FrontEndButtonSounds.SeasonChallengeButtonClick:
			AudioManager.PostEvent("ui_btn_menu_click", null);
			break;
		case FrontEndButtonSounds.LockboxUnlockUncommon:
			AudioManager.PostEvent("ui/lockbox/unlock_uncommon", null);
			break;
		case FrontEndButtonSounds.LockboxUnlockRare:
			AudioManager.PostEvent("ui/lockbox/unlock_rare", null);
			break;
		case FrontEndButtonSounds.LockboxUnlockEpic:
			AudioManager.PostEvent("ui/lockbox/unlock_epic", null);
			break;
		case FrontEndButtonSounds.LockboxUnlockLegendary:
			AudioManager.PostEvent("ui/lockbox/unlock_legendary", null);
			break;
		case FrontEndButtonSounds.LockboxOkCloseButton:
			AudioManager.PostEvent("ui/frontend/v1/btn/options/ok", null);
			break;
		case FrontEndButtonSounds.LockboxCancelButton:
			AudioManager.PostEvent("ui/frontend/v1/btn/options/cancel", null);
			break;
		case FrontEndButtonSounds.GeneralGetMoreCredits:
			AudioManager.PostEvent("ui_btn_menu_click", null);
			break;
		case FrontEndButtonSounds.GeneralExternalWebsite:
			AudioManager.PostEvent("ui_btn_menu_click", null);
			break;
		case FrontEndButtonSounds.InventoryTab:
			AudioManager.PostEvent("ui/frontend/v2/menu/tab/selection", null);
			break;
		case FrontEndButtonSounds.InventoryItemSelect:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add", null);
			break;
		case FrontEndButtonSounds.InventorySchematicListSelect:
			AudioManager.PostEvent("ui/frontend/v1/btn/menu/choice", null);
			break;
		case FrontEndButtonSounds.LockboxOpenClick:
			AudioManager.PostEvent("ui_btn_menu_click", null);
			AudioManager.PostEvent("ui/lockbox/hit", null);
			break;
		case FrontEndButtonSounds.InventoryCollectAllClick:
			AudioManager.PostEvent("ui_btn_menu_click", null);
			break;
		case FrontEndButtonSounds.InventorySalvageAllClick:
			AudioManager.PostEvent("ui_btn_menu_click", null);
			break;
		case FrontEndButtonSounds.InventoryCollect:
			AudioManager.PostEvent("ui/lockbox/collect", null);
			break;
		case FrontEndButtonSounds.SeasonsChapterTab:
			AudioManager.PostEvent("ui/frontend/v2/menu/tab/selection", null);
			break;
		case FrontEndButtonSounds.SeasonsBuyMoreLevels:
			AudioManager.PostEvent("ui_btn_menu_click", null);
			break;
		case FrontEndButtonSounds.SeasonsBuyLevelsSelect:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add", null);
			break;
		case FrontEndButtonSounds.SeasonsChallengeClickExpand:
			AudioManager.PostEvent("ui/frontend/v1/btn/menu/choice", null);
			AudioManager.PostEvent("ui/seasons/challenge_button_click_expand", null);
			break;
		case FrontEndButtonSounds.SeasonsChallengeTrashcanClick:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01", null);
			break;
		case FrontEndButtonSounds.SeasonsChallengeTrashcanYes:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01", null);
			AudioManager.PostEvent("ui_btn_menu_click", null);
			break;
		case FrontEndButtonSounds.SeasonsChallengeTrashcanNo:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01", null);
			AudioManager.PostEvent("ui/frontend/v1/btn/options/cancel", null);
			break;
		case FrontEndButtonSounds.ItemCrafting:
			AudioManager.PostEvent("ui/lockbox/craft", null);
			break;
		case FrontEndButtonSounds.InGameTauntClick:
			AudioManager.PostEvent("ui/ingame/v1/taunt_click", null);
			break;
		case FrontEndButtonSounds.InGameTauntSelect:
			AudioManager.PostEvent("ui/ingame/v1/taunt_select", null);
			break;
		case FrontEndButtonSounds.DailyQuestChoice:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add", null);
			break;
		case FrontEndButtonSounds.DeploymentBegin:
			AudioManager.PostEvent("ui/ingame/notify/match_start", null);
			break;
		case FrontEndButtonSounds.MaxEnergyReached:
			AudioManager.PostEvent("ui/ingame/v1/energy_max", null);
			break;
		case FrontEndButtonSounds.RankModeTimerTick:
			AudioManager.PostEvent("ui/frontend/ranked/timer_tick", null);
			break;
		case FrontEndButtonSounds.RankModeBanPlayer:
			AudioManager.PostEvent("ui/frontend/ranked/ban_player", null);
			break;
		case FrontEndButtonSounds.RankModePickPlayer:
			AudioManager.PostEvent("ui/frontend/ranked/pick_player", null);
			break;
		case FrontEndButtonSounds.RankTabClick:
			AudioManager.PostEvent("ui/frontend/v2/menu/tab/selection", null);
			break;
		case FrontEndButtonSounds.RankDropdownClick:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01", null);
			break;
		case FrontEndButtonSounds.RankDropdownSelect:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01", null);
			break;
		case FrontEndButtonSounds.RankQueueButtonClick:
			AudioManager.PostEvent("ui/frontend/btn_ready_start_game", null);
			break;
		case FrontEndButtonSounds.RankFreelancerClick:
			AudioManager.PostEvent("ui/frontend/v1/notify/teammember_select", null);
			break;
		case FrontEndButtonSounds.RankFreelancerSettingTab:
			AudioManager.PostEvent("ui/frontend/v2/menu/tab/selection", null);
			break;
		case FrontEndButtonSounds.RankFreelancerLockin:
			AudioManager.PostEvent("ui/frontend/ranked/lockin", null);
			break;
		case FrontEndButtonSounds.RankFreelancerSwapClick:
			AudioManager.PostEvent("ui/frontend/v1/notify/teammember_select", null);
			break;
		case FrontEndButtonSounds.OverconUsed:
			AudioManager.PostEvent("ui/ingame/v1/overcon_generic", null);
			break;
		case FrontEndButtonSounds.PurchaseComplete:
			AudioManager.PostEvent("ui/frontend/store/purchasecomplete", null);
			break;
		case FrontEndButtonSounds.ContractsTab:
			AudioManager.PostEvent("ui/frontend/v2/menu/tab/selection", null);
			break;
		case FrontEndButtonSounds.SeasonTransitionIntro:
			AudioManager.PostEvent("ui/endgame/unlock", null);
			break;
		case FrontEndButtonSounds.SeasonTransitionRewardDisplay:
			AudioManager.PostEvent("ui/seasons/endseason_SeasonTransitionRewardDisplay", null);
			break;
		case FrontEndButtonSounds.SeasonTransitionScoreCircle1:
			AudioManager.PostEvent("ui/seasons/endseason_scorecircle_01", null);
			break;
		case FrontEndButtonSounds.SeasonTransitionScoreCircle2:
			AudioManager.PostEvent("ui/seasons/endseason_scorecircle_02", null);
			break;
		case FrontEndButtonSounds.FirstTenGamesPregressComplete:
			AudioManager.PostEvent("ui/endgame/firsttengames_progress_complete", null);
			break;
		case FrontEndButtonSounds.FirstTenGamesProgressIncrement:
			AudioManager.PostEvent("ui/endgame/firsttengames_progress_increment", null);
			break;
		case FrontEndButtonSounds.EndGameBadgeBasic:
			AudioManager.PostEvent("ui/endgame/badge/basic", null);
			break;
		case FrontEndButtonSounds.EndGameBadgeAchievement:
			AudioManager.PostEvent("ui/endgame/badge/achievement", null);
			break;
		case FrontEndButtonSounds.HudLockIn:
			AudioManager.PostEvent("ui/ingame/v1/hud/lockin", null);
			break;
		}
	}

	public static void PlayLoopingSound(FrontEndButtonSounds sound)
	{
		if (sound != FrontEndButtonSounds.SeasonTransitionSeasonPoints)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEnd.PlayLoopingSound(FrontEndButtonSounds)).MethodHandle;
			}
			if (sound != FrontEndButtonSounds.SeasonTransitionReactorPoints)
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
			}
			else
			{
				AudioManager.PostEvent("ui/endgame/points/counter_ggboost_loop", AudioManager.EventAction.PlaySound, null, null);
			}
		}
		else
		{
			AudioManager.PostEvent("ui/endgame/points/counter_normal_loop", AudioManager.EventAction.PlaySound, null, null);
		}
	}

	public static void StopLoopingSound(FrontEndButtonSounds sound)
	{
		if (sound != FrontEndButtonSounds.SeasonTransitionSeasonPoints)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEnd.StopLoopingSound(FrontEndButtonSounds)).MethodHandle;
			}
			if (sound == FrontEndButtonSounds.SeasonTransitionReactorPoints)
			{
				AudioManager.PostEvent("ui/endgame/points/counter_ggboost_loop", AudioManager.EventAction.StopSound, null, null);
			}
		}
		else
		{
			AudioManager.PostEvent("ui/endgame/points/counter_normal_loop", AudioManager.EventAction.StopSound, null, null);
		}
	}

	public static bool IsMapTypeName(string name)
	{
		bool result;
		if (!(name == "Practice") && !(name == "Tutorial") && !(name == "Deathmatch"))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEnd.IsMapTypeName(string)).MethodHandle;
			}
			result = (name == "Testing");
		}
		else
		{
			result = true;
		}
		return result;
	}

	public static string GetSceneDescription(string sceneName)
	{
		if (sceneName.IsNullOrEmpty())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEnd.GetSceneDescription(string)).MethodHandle;
			}
			return null;
		}
		string[] array = sceneName.Split(new char[]
		{
			'_'
		});
		if (array.Length < 2 || !UIFrontEnd.IsMapTypeName(array[1]))
		{
			return sceneName;
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
		if (array.Length == 2)
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
			return string.Format("{0} ({1})", array[0], array[1]);
		}
		return string.Format("{0} {2} ({1})", array[0], array[1], array[2]);
	}

	private void Awake()
	{
		UIFrontEnd.s_instance = this;
		if (base.gameObject.transform.parent == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	public bool IsLobbyOwner()
	{
		return false;
	}

	public void EnableFrontendEnvironment(bool enableEnvironment)
	{
		if (!enableEnvironment)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEnd.EnableFrontendEnvironment(bool)).MethodHandle;
			}
			Log.Info(Log.Category.Loading, "PKFxManager.DeepReset leaving frontend", new object[0]);
			PKFxManager.DeepReset();
		}
		UIManager uimanager = UIManager.Get();
		UIManager.ClientState gameState;
		if (enableEnvironment)
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
			gameState = UIManager.ClientState.InFrontEnd;
		}
		else
		{
			gameState = UIManager.ClientState.InGame;
		}
		uimanager.SetGameState(gameState);
	}

	public bool IsProgressScreenOpen()
	{
		bool result;
		if (UIPlayerProgressPanel.Get() != null && UIPlayerProgressPanel.Get().gameObject)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEnd.IsProgressScreenOpen()).MethodHandle;
			}
			result = UIPlayerProgressPanel.Get().gameObject.activeInHierarchy;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool IsStoreOpen()
	{
		bool result;
		if (!UIStoreViewHeroPage.Get().IsVisible())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEnd.IsStoreOpen()).MethodHandle;
			}
			result = UIStorePanel.Get().IsVisible();
		}
		else
		{
			result = true;
		}
		return result;
	}

	public void TogglePlayerProgressScreenVisibility(bool needToUpdatePlayerProgress = true)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEnd.TogglePlayerProgressScreenVisibility(bool)).MethodHandle;
			}
			return;
		}
		bool flag = UIPlayerProgressPanel.Get().IsVisible();
		UIPlayerProgressPanel.Get().SetVisible(!flag, needToUpdatePlayerProgress);
		if (flag)
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
			UIPlayerProgressPanel.Get().LogPlayerChanges();
		}
	}

	public void ToggleStoreVisibility()
	{
		UIStorePanel.Get().ToggleStore();
		UIPlayerProgressPanel.Get().SetVisible(false, true);
	}

	public void TogglePlayerFriendListVisibility()
	{
		FriendListPanel.Get().SetVisible(!FriendListPanel.Get().IsVisible(), false, false);
	}

	public void ShowScreen(FrontEndScreenState newScreen, bool refreshOnly = false)
	{
		this.m_currentScreen = newScreen;
		if (UICharacterSelectScreenController.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEnd.ShowScreen(FrontEndScreenState, bool)).MethodHandle;
			}
			UICharacterSelectScreenController.Get().SetupReadyButton();
		}
		bool visible = false;
		bool visible2 = false;
		bool visible3 = false;
		bool visible4 = false;
		bool visible5 = false;
		bool doActive = false;
		bool flag = false;
		switch (newScreen)
		{
		case FrontEndScreenState.LandingPage:
			visible3 = true;
			doActive = true;
			visible2 = true;
			flag = true;
			UIFrontEnd.Get().m_frontEndNavPanel.SetPlayMenuCatgeoryVisible(false);
			break;
		case FrontEndScreenState.GameTypeSelect:
			doActive = true;
			visible2 = true;
			break;
		case FrontEndScreenState.CharacterSelect:
		case FrontEndScreenState.GroupCharacterSelect:
			if (UILootMatrixScreen.Get() != null && !UILootMatrixScreen.Get().IsVisible && UIStorePanel.Get() != null && !UIStorePanel.Get().IsVisible())
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
				visible = true;
			}
			else
			{
				bool flag2;
				if (!AppState.IsInGame())
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
					if (GameManager.Get().GameInfo != null)
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
						if (GameManager.Get().GameInfo.IsCustomGame)
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
							flag2 = (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped);
							goto IL_154;
						}
					}
				}
				flag2 = false;
				IL_154:
				bool flag3 = flag2;
				if (flag3)
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
					visible = true;
					UILootMatrixScreen.Get().SetVisible(false);
				}
			}
			visible2 = true;
			doActive = true;
			flag = true;
			break;
		case FrontEndScreenState.JoinGame:
			visible4 = true;
			this.m_joinGameScreen.Setup();
			doActive = true;
			visible2 = true;
			break;
		case FrontEndScreenState.CreateGame:
			visible5 = true;
			doActive = true;
			visible2 = true;
			break;
		case FrontEndScreenState.WaitingForGame:
			doActive = true;
			visible2 = true;
			break;
		case FrontEndScreenState.FoundGame:
			doActive = true;
			break;
		case FrontEndScreenState.RankedModeSelect:
			visible2 = true;
			doActive = true;
			flag = true;
			break;
		}
		if (ClientGameManager.Get().IsServerLocked)
		{
			visible2 = false;
		}
		if (this.m_playerPanel != null)
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
			this.m_playerPanel.SetVisible(visible2, refreshOnly);
		}
		if (this.m_joinGameScreen != null)
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
			this.m_joinGameScreen.SetVisible(visible4);
		}
		if (this.m_createGameScreen != null)
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
			this.m_createGameScreen.SetVisible(visible5);
		}
		if (!refreshOnly)
		{
			if (UIPlayerProgressPanel.Get() != null)
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
				UIPlayerProgressPanel.Get().SetVisible(false, true);
			}
			if (UICharacterSelectWorldObjects.Get() != null)
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
				UICharacterSelectWorldObjects.Get().SetVisible(visible);
			}
			if (this.m_landingPageScreen != null)
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
				this.m_landingPageScreen.SetVisible(visible3);
			}
			if (UICharacterSelectScreen.Get() != null)
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
				UICharacterSelectScreen.Get().SetVisible(false);
			}
			if (this.m_frontEndChatConsole != null)
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
				UIManager.SetGameObjectActive(this.m_frontEndChatConsole, doActive, null);
			}
		}
		if (UICharacterSelectScreenController.Get() != null)
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
			UICharacterSelectScreenController.Get().SetVisible(visible, refreshOnly);
		}
		if (flag && this.m_frontEndChatConsole != null)
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
			this.m_frontEndChatConsole.ChangeChatRoom();
		}
	}

	public bool IsDraggingModel()
	{
		bool result;
		if (!this.m_isDragging)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEnd.IsDraggingModel()).MethodHandle;
			}
			result = this.m_justStoppedDragging;
		}
		else
		{
			result = true;
		}
		return result;
	}

	private void Start()
	{
		UIScreenManager.Get().ClearAllPanels();
		this.m_currentCameraPosition = FrontEndScene.LobbyScreen;
		this.SetVisible(false);
	}

	private void OnDestroy()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEnd.OnDestroy()).MethodHandle;
			}
			clientGameManager.OnLobbyStatusNotification -= this.HandleLobbyStatusNotification;
		}
		if (UIFrontEnd.s_instance == this)
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
			Log.Info(base.GetType() + " OnDestroy, clearing singleton reference", new object[0]);
			UIFrontEnd.s_instance = null;
		}
	}

	public void HandleLobbyStatusNotification(LobbyStatusNotification notification)
	{
		if (!(UIPlayerProgressPanel.Get() == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEnd.HandleLobbyStatusNotification(LobbyStatusNotification)).MethodHandle;
			}
			if (UIPlayerProgressPanel.Get().IsVisible())
			{
				return;
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
		}
		this.ShowScreen(this.m_currentScreen, true);
	}

	public void ConfirmExit(UIDialogBox boxReference)
	{
		AppState_Shutdown.Get().Enter();
	}

	public void OnExitGameClick(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
		UIDialogPopupManager.OpenTwoButtonDialog(StringUtil.TR("ExitGameTitle", "Global"), StringUtil.TR("ExitGamePrompt", "Global"), StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), new UIDialogBox.DialogButtonCallback(this.ConfirmExit), null, false, false);
	}

	public void OnCreditsClick(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
		if (UICreditsScreen.Get() != null)
		{
			UICreditsScreen.Get().SetVisible(true);
		}
		else
		{
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("CreditsTitle", "Global"), StringUtil.TR("CreditsBody", "Global"), StringUtil.TR("Close", "Global"), null, 0x14, false);
		}
	}

	public void ConfirmBack(UIDialogBox boxReference)
	{
		if (AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
		{
			AppState_LandingPage.Get().Enter(true);
			UIFrontEnd.PlaySound(FrontEndButtonSounds.Back);
		}
		else
		{
			AppState_GameTeardown.Get().Enter();
		}
	}

	public void OnBackClick(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
		if (AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEnd.OnBackClick(BaseEventData)).MethodHandle;
			}
			AppState_LandingPage.Get().Enter(true);
		}
		else
		{
			UIDialogPopupManager.OpenTwoButtonDialog(StringUtil.TR("LeavingGame", "Global"), StringUtil.TR("QuitGamePrompt", "Global"), StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), new UIDialogBox.DialogButtonCallback(this.ConfirmBack), null, false, false);
		}
	}

	public void Disable()
	{
		this.EnableFrontendEnvironment(false);
		if (ClientQualityComponentEnabler.OptimizeForMemory())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEnd.Disable()).MethodHandle;
			}
			Resources.UnloadUnusedAssets();
		}
	}

	public void SetVisible(bool visible)
	{
		this.m_isVisible = visible;
		if (UI_Persistent.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEnd.SetVisible(bool)).MethodHandle;
			}
			UI_Persistent.Get().NotifyFrontEndVisible(visible);
		}
	}

	public void ResetCharacterRotation()
	{
		this.m_currentRotationOffset = Vector3.zero;
	}

	public Vector3 GetRotationOffset()
	{
		return this.m_currentRotationOffset;
	}

	public void Update()
	{
		if (!this.m_attachedHandler)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEnd.Update()).MethodHandle;
			}
			if (ClientGameManager.Get() != null)
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
				ClientGameManager.Get().OnLobbyStatusNotification += this.HandleLobbyStatusNotification;
				this.m_attachedHandler = true;
			}
		}
		if (UIFrontEnd.s_firstLogInQuestCount != -1)
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
			if (QuestOfferPanel.Get() != null && !QuestOfferPanel.Get().IsActive() && QuestListPanel.Get() != null)
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
				if (UIFrontEnd.Get().m_frontEndNavPanel != null)
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
					UIFrontEnd.s_firstLogInQuestCount = -1;
					UIFrontEnd.Get().m_frontEndNavPanel.NotificationBtnClicked(null);
				}
			}
		}
		if (this.m_isVisible && !(UIManager.Get().GetEnvirontmentCamera() == null))
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
			if (DebugParameters.Get() != null)
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
				if (DebugParameters.Get().GetParameterAsBool("DebugCamera"))
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						return;
					}
				}
			}
			float num = (Time.time - this.m_charCamStartTime) * this.m_cameraRotationSpeed;
			float t = 0f;
			if (this.m_rotationStartLength != 0f)
			{
				t = num / this.m_rotationStartLength;
			}
			if (this.m_lookAtOffset != this.m_cameraPositions[(int)this.m_currentCameraPosition].rotation)
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
				this.m_lookAtOffset = Vector3.Lerp(this.m_lookAtOffset, this.m_cameraPositions[(int)this.m_currentCameraPosition].rotation, t);
			}
			this.m_justStoppedDragging = false;
			if (UIManager.Get().GetEnvirontmentCamera())
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
				if (UICharacterSelectWorldObjects.Get() != null && UICharacterSelectWorldObjects.Get().m_ringAnimations[0] != null)
				{
					UIActorModelData componentInChildren = UICharacterSelectWorldObjects.Get().m_ringAnimations[0].GetComponentInChildren<UIActorModelData>();
					if (Input.GetMouseButtonDown(0))
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
						if (componentInChildren != null && componentInChildren.MousedOver(UIManager.Get().GetEnvirontmentCamera()))
						{
							this.m_isStartDrag = true;
							this.m_startDragPosition = Input.mousePosition;
							this.m_startRotation = this.m_currentRotationOffset;
						}
					}
				}
				if (!UIUtils.InputFieldHasFocus())
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
					if (AccountPreferences.DoesApplicationHaveFocus())
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
						if (Input.GetKey(KeyCode.RightArrow))
						{
							this.m_currentRotationOffset -= new Vector3(0f, 5f, 0f);
						}
						if (Input.GetKey(KeyCode.LeftArrow))
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
							this.m_currentRotationOffset += new Vector3(0f, 5f, 0f);
						}
					}
				}
			}
			if (UICharacterStoreAndProgressWorldObjects.Get() != null && UICharacterStoreAndProgressWorldObjects.Get().IsVisible())
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
				if (UICharacterStoreAndProgressWorldObjects.Get().m_ringAnimations[0] != null)
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
					UIActorModelData componentInChildren2 = UICharacterStoreAndProgressWorldObjects.Get().m_ringAnimations[0].GetComponentInChildren<UIActorModelData>();
					if (Input.GetMouseButtonDown(0))
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
						if (componentInChildren2 != null)
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
							if (componentInChildren2.MousedOver(UIManager.Get().GetEnvirontmentCamera()))
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
								this.m_isStartDrag = true;
								this.m_startDragPosition = Input.mousePosition;
								this.m_startRotation = this.m_currentRotationOffset;
							}
						}
					}
				}
			}
			if (this.m_isStartDrag)
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
				if ((Input.mousePosition - this.m_startDragPosition).magnitude > 10f)
				{
					this.m_startDragPosition = Input.mousePosition;
					this.m_isStartDrag = false;
					this.m_isDragging = true;
				}
			}
			if (this.m_isDragging)
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
				this.m_lastMouseLocation = Input.mousePosition;
				Vector3 vector = this.m_startDragPosition - this.m_lastMouseLocation;
				this.m_currentRotationOffset = this.m_startRotation + new Vector3(0f, vector.x / (float)Screen.width * 360f, 0f);
			}
			if (Input.GetMouseButtonUp(0))
			{
				if (this.m_isDragging)
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
					this.m_justStoppedDragging = true;
				}
				this.m_isDragging = false;
				this.m_isStartDrag = false;
			}
			if (GameManager.Get() != null)
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
				if (!GameManager.Get().GameplayOverrides.DisableControlPadInput)
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
					if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.RightStickX) != 0f)
					{
						this.m_currentRotationOffset += new Vector3(0f, ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.RightStickX) * 10f, 0f);
					}
					if (UICharacterSelectScreenController.Get() != null)
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
						if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.DpadX) > 0f)
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
							CharacterType characterType = UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay;
							bool flag = false;
							while (!flag)
							{
								characterType++;
								if (characterType >= CharacterType.Last)
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
									characterType = CharacterType.BattleMonk;
								}
								flag = GameManager.Get().IsCharacterAllowedForPlayers(characterType);
							}
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
							{
								ClientRequestToServerSelectCharacter = new CharacterType?(characterType)
							});
						}
						if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.DpadX) < 0f)
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
							CharacterType characterType2 = UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay;
							bool flag2 = false;
							while (!flag2)
							{
								characterType2--;
								if (characterType2 == CharacterType.None)
								{
									characterType2 = CharacterType.Fireborg;
								}
								flag2 = GameManager.Get().IsCharacterAllowedForPlayers(characterType2);
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
							UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
							{
								ClientRequestToServerSelectCharacter = new CharacterType?(characterType2)
							});
						}
					}
				}
			}
			return;
		}
	}

	public bool CanMenuEscape()
	{
		if (!this.IsProgressScreenOpen())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEnd.CanMenuEscape()).MethodHandle;
			}
			if (UIStorePanel.Get() != null)
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
				if (!UIStorePanel.Get().CanOpenMenu())
				{
					return false;
				}
			}
			if (AppState_RankModeDraft.Get() == AppState.GetCurrent())
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
				return false;
			}
			bool flag = UIDialogPopupManager.Get() != null && UIDialogPopupManager.Get().IsDialogBoxOpen();
			bool result;
			if (!this.IsChatWindowFocused())
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
				result = !flag;
			}
			else
			{
				result = false;
			}
			return result;
		}
		return false;
	}

	public bool IsChatWindowFocused()
	{
		if (this.m_frontEndChatConsole != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEnd.IsChatWindowFocused()).MethodHandle;
			}
			if (this.m_frontEndChatConsole.IsVisible())
			{
				return EventSystem.current.currentSelectedGameObject == this.m_frontEndChatConsole.m_textInput.gameObject || this.m_frontEndChatConsole.InputJustcleared();
			}
		}
		return false;
	}

	public void NotifyGameLaunched()
	{
	}

	public static UICharacterWorldObjects GetVisibleCharacters()
	{
		if (UICharacterStoreAndProgressWorldObjects.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontEnd.GetVisibleCharacters()).MethodHandle;
			}
			if (UICharacterStoreAndProgressWorldObjects.Get().IsVisible())
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
				return UICharacterStoreAndProgressWorldObjects.Get();
			}
		}
		if (UICharacterSelectWorldObjects.Get() != null)
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
			if (UICharacterSelectWorldObjects.Get().IsVisible())
			{
				return UICharacterSelectWorldObjects.Get();
			}
		}
		return null;
	}
}
