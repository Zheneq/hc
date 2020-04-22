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

	public FrontEndNavPanel m_frontEndNavPanel => FrontEndNavPanel.Get();

	public UIPlayerNavPanel m_playerPanel => UIPlayerNavPanel.Get();

	public UILandingPageScreen m_landingPageScreen => UILandingPageScreen.Get();

	public UIJoinGameScreen m_joinGameScreen => UIJoinGameScreen.Get();

	public UICreateGameScreen m_createGameScreen => UICreateGameScreen.Get();

	public UITextConsole m_frontEndChatConsole => UIChatBox.GetChatBox(UIManager.ClientState.InFrontEnd);

	public static UIFrontEnd Get()
	{
		return s_instance;
	}

	public static void PlaySound(FrontEndButtonSounds sound)
	{
		switch (sound)
		{
		case FrontEndButtonSounds.RankFreelancerSelectClick:
			break;
		case FrontEndButtonSounds.SeasonTransitionSeasonPoints:
		case FrontEndButtonSounds.SeasonTransitionReactorPoints:
			break;
		case FrontEndButtonSounds.Back:
			AudioManager.PostEvent("ui/frontend/v1/btn/back");
			break;
		case FrontEndButtonSounds.Cancel:
			AudioManager.PostEvent("ui/frontend/v1/btn/cancel");
			break;
		case FrontEndButtonSounds.Generic:
			AudioManager.PostEvent("ui/frontend/v1/btn/generic");
			break;
		case FrontEndButtonSounds.GenericSmall:
			AudioManager.PostEvent("ui/frontend/v1/btn/generic_small");
			break;
		case FrontEndButtonSounds.GameModeSelect:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01");
			AudioManager.PostEvent("ui/frontend/v1/btn/gamemode_select");
			break;
		case FrontEndButtonSounds.MenuOpen:
			AudioManager.PostEvent("ui/frontend/v1/btn/menu/open");
			break;
		case FrontEndButtonSounds.MenuChoice:
			AudioManager.PostEvent("ui/frontend/v1/btn/menu/choice");
			break;
		case FrontEndButtonSounds.SubMenuOpen:
			AudioManager.PostEvent("ui/frontend/v1/btn/menu/submenu/open");
			break;
		case FrontEndButtonSounds.SelectChoice:
			AudioManager.PostEvent("ui/frontend/v1/notify/teammember_select");
			break;
		case FrontEndButtonSounds.SelectColorChoice:
			AudioManager.PostEvent("ui/frontend/v1/notify/teammember_select");
			break;
		case FrontEndButtonSounds.TeamMemberSelect:
			AudioManager.PostEvent("ui/frontend/v1/notify/teammember_select");
			break;
		case FrontEndButtonSounds.TeamMemberCancel:
			AudioManager.PostEvent("ui/frontend/v1/notify/teammember_cancel");
			break;
		case FrontEndButtonSounds.StartGameReady:
			AudioManager.PostEvent("ui/frontend/btn_ready_start_game");
			break;
		case FrontEndButtonSounds.PlayCategorySelect:
			AudioManager.PostEvent("ui/frontend/v2/menu/tab/selection");
			break;
		case FrontEndButtonSounds.OptionsChoice:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01");
			break;
		case FrontEndButtonSounds.TopMenuSelect:
			AudioManager.PostEvent("ui/frontend/v1/btn/menu/choice");
			break;
		case FrontEndButtonSounds.OptionsOK:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/button");
			break;
		case FrontEndButtonSounds.OptionsCancel:
			AudioManager.PostEvent("ui/frontend/v1/btn/options/cancel");
			break;
		case FrontEndButtonSounds.CharacterSelectCharacter:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/character");
			break;
		case FrontEndButtonSounds.Close:
			AudioManager.PostEvent("ui/frontend/v1/btn/close");
			break;
		case FrontEndButtonSounds.CharacterSelectOpen:
			AudioManager.PostEvent("ui/frontend/v1/btn/menu/submenu/open");
			AudioManager.PostEvent("ui/frontend/v1/wnd/charselect/open");
			break;
		case FrontEndButtonSounds.CharacterSelectClose:
			AudioManager.PostEvent("ui/frontend/v1/wnd/charselect/close");
			break;
		case FrontEndButtonSounds.CharacterSelectOptions:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/options");
			break;
		case FrontEndButtonSounds.CharacterSelectOptionsChoice:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/options/choice");
			break;
		case FrontEndButtonSounds.CharacterSelectSkinChoice:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add");
			break;
		case FrontEndButtonSounds.CharacterSelectModUnlocked:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/unlock");
			break;
		case FrontEndButtonSounds.CharacterSelectModAdd:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add");
			break;
		case FrontEndButtonSounds.CharacterSelectModClear:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/clear");
			break;
		case FrontEndButtonSounds.CharacterSelectNotifyCharLoaded:
			AudioManager.PostEvent("ui/frontend/v1/notify/charselect/loaded");
			break;
		case FrontEndButtonSounds.StorePurchased:
			AudioManager.PostEvent("ui/frontend/v1/btn/menu/choice");
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add");
			break;
		case FrontEndButtonSounds.StoreCurrencySelect:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add");
			break;
		case FrontEndButtonSounds.StoreGGPackSelect:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add");
			break;
		case FrontEndButtonSounds.TutorialPhaseIconAppear:
			AudioManager.PostEvent("ui/frontend/v2/menu/tutorial/phase/icon/appear");
			break;
		case FrontEndButtonSounds.TutorialPhaseIconImpact:
			AudioManager.PostEvent("ui/frontend/v2/menu/tutorial/phase/icon/impact");
			break;
		case FrontEndButtonSounds.TutorialPhaseIconHighlight:
			AudioManager.PostEvent("ui/frontend/v2/menu/tutorial/phase/icon/highlight");
			break;
		case FrontEndButtonSounds.DialogBoxOpen:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/window/appear");
			break;
		case FrontEndButtonSounds.DialogBoxButton:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/button");
			break;
		case FrontEndButtonSounds.GGButtonInGameUsed:
			AudioManager.PostEvent("ui/ingame/ggboost_button");
			break;
		case FrontEndButtonSounds.GGPackUsedNotification:
			AudioManager.PostEvent("ui/ingame/ggboost_button");
			break;
		case FrontEndButtonSounds.GGButtonEndGameUsed:
			AudioManager.PostEvent("ui/endgame/ggboost_button");
			break;
		case FrontEndButtonSounds.NotifyWarning:
			AudioManager.PostEvent("ui/frontend/v2/notify/warning");
			break;
		case FrontEndButtonSounds.MainMenuOpen:
			AudioManager.PostEvent("ui/frontend/v2/menu/mainmenu_open");
			break;
		case FrontEndButtonSounds.MainMenuClose:
			AudioManager.PostEvent("ui/frontend/v2/menu/mainmenu_close");
			break;
		case FrontEndButtonSounds.NotifyMatchFound:
			AudioManager.PostEvent("ui/frontend/v1/notify/match_found");
			break;
		case FrontEndButtonSounds.WhisperMessage:
			AudioManager.PostEvent("ui/frontend/v1/chat/whisper_notify");
			break;
		case FrontEndButtonSounds.LockboxAppear:
			AudioManager.PostEvent("ui/lockbox/appear");
			break;
		case FrontEndButtonSounds.LockboxHit:
			AudioManager.PostEvent("ui/lockbox/hit");
			break;
		case FrontEndButtonSounds.LockboxUnlock:
			AudioManager.PostEvent("ui/lockbox/unlock");
			break;
		case FrontEndButtonSounds.InventoryCraftClick:
			AudioManager.PostEvent("ui_btn_menu_click");
			break;
		case FrontEndButtonSounds.InventorySalvage:
			AudioManager.PostEvent("ui/lockbox/salvage");
			break;
		case FrontEndButtonSounds.LockboxSelect:
			AudioManager.PostEvent("ui/frontend/v1/btn/menu/choice");
			break;
		case FrontEndButtonSounds.InventoryFilterSelect:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01");
			break;
		case FrontEndButtonSounds.CraftButtonClick:
			AudioManager.PostEvent("ui_btn_menu_click");
			break;
		case FrontEndButtonSounds.SeasonChallengeButtonClick:
			AudioManager.PostEvent("ui_btn_menu_click");
			break;
		case FrontEndButtonSounds.LockboxUnlockUncommon:
			AudioManager.PostEvent("ui/lockbox/unlock_uncommon");
			break;
		case FrontEndButtonSounds.LockboxUnlockRare:
			AudioManager.PostEvent("ui/lockbox/unlock_rare");
			break;
		case FrontEndButtonSounds.LockboxUnlockEpic:
			AudioManager.PostEvent("ui/lockbox/unlock_epic");
			break;
		case FrontEndButtonSounds.LockboxUnlockLegendary:
			AudioManager.PostEvent("ui/lockbox/unlock_legendary");
			break;
		case FrontEndButtonSounds.LockboxOkCloseButton:
			AudioManager.PostEvent("ui/frontend/v1/btn/options/ok");
			break;
		case FrontEndButtonSounds.LockboxCancelButton:
			AudioManager.PostEvent("ui/frontend/v1/btn/options/cancel");
			break;
		case FrontEndButtonSounds.GeneralGetMoreCredits:
			AudioManager.PostEvent("ui_btn_menu_click");
			break;
		case FrontEndButtonSounds.GeneralExternalWebsite:
			AudioManager.PostEvent("ui_btn_menu_click");
			break;
		case FrontEndButtonSounds.InventoryTab:
			AudioManager.PostEvent("ui/frontend/v2/menu/tab/selection");
			break;
		case FrontEndButtonSounds.InventoryItemSelect:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add");
			break;
		case FrontEndButtonSounds.InventorySchematicListSelect:
			AudioManager.PostEvent("ui/frontend/v1/btn/menu/choice");
			break;
		case FrontEndButtonSounds.LockboxOpenClick:
			AudioManager.PostEvent("ui_btn_menu_click");
			AudioManager.PostEvent("ui/lockbox/hit");
			break;
		case FrontEndButtonSounds.InventoryCollectAllClick:
			AudioManager.PostEvent("ui_btn_menu_click");
			break;
		case FrontEndButtonSounds.InventorySalvageAllClick:
			AudioManager.PostEvent("ui_btn_menu_click");
			break;
		case FrontEndButtonSounds.InventoryCollect:
			AudioManager.PostEvent("ui/lockbox/collect");
			break;
		case FrontEndButtonSounds.SeasonsChapterTab:
			AudioManager.PostEvent("ui/frontend/v2/menu/tab/selection");
			break;
		case FrontEndButtonSounds.SeasonsBuyMoreLevels:
			AudioManager.PostEvent("ui_btn_menu_click");
			break;
		case FrontEndButtonSounds.SeasonsBuyLevelsSelect:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add");
			break;
		case FrontEndButtonSounds.SeasonsChallengeClickExpand:
			AudioManager.PostEvent("ui/frontend/v1/btn/menu/choice");
			AudioManager.PostEvent("ui/seasons/challenge_button_click_expand");
			break;
		case FrontEndButtonSounds.SeasonsChallengeTrashcanClick:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01");
			break;
		case FrontEndButtonSounds.SeasonsChallengeTrashcanYes:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01");
			AudioManager.PostEvent("ui_btn_menu_click");
			break;
		case FrontEndButtonSounds.SeasonsChallengeTrashcanNo:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01");
			AudioManager.PostEvent("ui/frontend/v1/btn/options/cancel");
			break;
		case FrontEndButtonSounds.ItemCrafting:
			AudioManager.PostEvent("ui/lockbox/craft");
			break;
		case FrontEndButtonSounds.InGameTauntClick:
			AudioManager.PostEvent("ui/ingame/v1/taunt_click");
			break;
		case FrontEndButtonSounds.InGameTauntSelect:
			AudioManager.PostEvent("ui/ingame/v1/taunt_select");
			break;
		case FrontEndButtonSounds.DailyQuestChoice:
			AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add");
			break;
		case FrontEndButtonSounds.DeploymentBegin:
			AudioManager.PostEvent("ui/ingame/notify/match_start");
			break;
		case FrontEndButtonSounds.MaxEnergyReached:
			AudioManager.PostEvent("ui/ingame/v1/energy_max");
			break;
		case FrontEndButtonSounds.FirstTenGamesPregressComplete:
			AudioManager.PostEvent("ui/endgame/firsttengames_progress_complete");
			break;
		case FrontEndButtonSounds.FirstTenGamesProgressIncrement:
			AudioManager.PostEvent("ui/endgame/firsttengames_progress_increment");
			break;
		case FrontEndButtonSounds.HudLockIn:
			AudioManager.PostEvent("ui/ingame/v1/hud/lockin");
			break;
		case FrontEndButtonSounds.RankModeTimerTick:
			AudioManager.PostEvent("ui/frontend/ranked/timer_tick");
			break;
		case FrontEndButtonSounds.RankModeBanPlayer:
			AudioManager.PostEvent("ui/frontend/ranked/ban_player");
			break;
		case FrontEndButtonSounds.RankModePickPlayer:
			AudioManager.PostEvent("ui/frontend/ranked/pick_player");
			break;
		case FrontEndButtonSounds.RankDropdownClick:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01");
			break;
		case FrontEndButtonSounds.RankDropdownSelect:
			AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01");
			break;
		case FrontEndButtonSounds.RankTabClick:
			AudioManager.PostEvent("ui/frontend/v2/menu/tab/selection");
			break;
		case FrontEndButtonSounds.RankQueueButtonClick:
			AudioManager.PostEvent("ui/frontend/btn_ready_start_game");
			break;
		case FrontEndButtonSounds.RankFreelancerClick:
			AudioManager.PostEvent("ui/frontend/v1/notify/teammember_select");
			break;
		case FrontEndButtonSounds.RankFreelancerSettingTab:
			AudioManager.PostEvent("ui/frontend/v2/menu/tab/selection");
			break;
		case FrontEndButtonSounds.RankFreelancerLockin:
			AudioManager.PostEvent("ui/frontend/ranked/lockin");
			break;
		case FrontEndButtonSounds.RankFreelancerSwapClick:
			AudioManager.PostEvent("ui/frontend/v1/notify/teammember_select");
			break;
		case FrontEndButtonSounds.OverconUsed:
			AudioManager.PostEvent("ui/ingame/v1/overcon_generic");
			break;
		case FrontEndButtonSounds.PurchaseComplete:
			AudioManager.PostEvent("ui/frontend/store/purchasecomplete");
			break;
		case FrontEndButtonSounds.ContractsTab:
			AudioManager.PostEvent("ui/frontend/v2/menu/tab/selection");
			break;
		case FrontEndButtonSounds.SeasonTransitionIntro:
			AudioManager.PostEvent("ui/endgame/unlock");
			break;
		case FrontEndButtonSounds.SeasonTransitionRewardDisplay:
			AudioManager.PostEvent("ui/seasons/endseason_SeasonTransitionRewardDisplay");
			break;
		case FrontEndButtonSounds.SeasonTransitionScoreCircle1:
			AudioManager.PostEvent("ui/seasons/endseason_scorecircle_01");
			break;
		case FrontEndButtonSounds.SeasonTransitionScoreCircle2:
			AudioManager.PostEvent("ui/seasons/endseason_scorecircle_02");
			break;
		case FrontEndButtonSounds.EndGameBadgeBasic:
			AudioManager.PostEvent("ui/endgame/badge/basic");
			break;
		case FrontEndButtonSounds.EndGameBadgeAchievement:
			AudioManager.PostEvent("ui/endgame/badge/achievement");
			break;
		}
	}

	public static void PlayLoopingSound(FrontEndButtonSounds sound)
	{
		if (sound != FrontEndButtonSounds.SeasonTransitionSeasonPoints)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (sound != FrontEndButtonSounds.SeasonTransitionReactorPoints)
					{
						while (true)
						{
							switch (4)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
					AudioManager.PostEvent("ui/endgame/points/counter_ggboost_loop", AudioManager.EventAction.PlaySound);
					return;
				}
			}
		}
		AudioManager.PostEvent("ui/endgame/points/counter_normal_loop", AudioManager.EventAction.PlaySound);
	}

	public static void StopLoopingSound(FrontEndButtonSounds sound)
	{
		if (sound != FrontEndButtonSounds.SeasonTransitionSeasonPoints)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (sound == FrontEndButtonSounds.SeasonTransitionReactorPoints)
					{
						AudioManager.PostEvent("ui/endgame/points/counter_ggboost_loop", AudioManager.EventAction.StopSound);
					}
					return;
				}
			}
		}
		AudioManager.PostEvent("ui/endgame/points/counter_normal_loop", AudioManager.EventAction.StopSound);
	}

	public static bool IsMapTypeName(string name)
	{
		int result;
		switch (name)
		{
		default:
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
			result = ((name == "Testing") ? 1 : 0);
			break;
		case "Practice":
		case "Tutorial":
		case "Deathmatch":
			result = 1;
			break;
		}
		return (byte)result != 0;
	}

	public static string GetSceneDescription(string sceneName)
	{
		if (sceneName.IsNullOrEmpty())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return null;
				}
			}
		}
		string[] array = sceneName.Split('_');
		if (array.Length >= 2 && IsMapTypeName(array[1]))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (array.Length == 2)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return $"{array[0]} ({array[1]})";
							}
						}
					}
					return string.Format("{0} {2} ({1})", array[0], array[1], array[2]);
				}
			}
		}
		return sceneName;
	}

	private void Awake()
	{
		s_instance = this;
		if (base.gameObject.transform.parent == null)
		{
			Object.DontDestroyOnLoad(base.gameObject);
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
			Log.Info(Log.Category.Loading, "PKFxManager.DeepReset leaving frontend");
			PKFxManager.DeepReset();
		}
		UIManager uIManager = UIManager.Get();
		int gameState;
		if (enableEnvironment)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			gameState = 0;
		}
		else
		{
			gameState = 1;
		}
		uIManager.SetGameState((UIManager.ClientState)gameState);
	}

	public bool IsProgressScreenOpen()
	{
		int result;
		if (UIPlayerProgressPanel.Get() != null && (bool)UIPlayerProgressPanel.Get().gameObject)
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
			result = (UIPlayerProgressPanel.Get().gameObject.activeInHierarchy ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public bool IsStoreOpen()
	{
		int result;
		if (!UIStoreViewHeroPage.Get().IsVisible())
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = (UIStorePanel.Get().IsVisible() ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public void TogglePlayerProgressScreenVisibility(bool needToUpdatePlayerProgress = true)
	{
		if (UIMatchStartPanel.Get().IsVisible())
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
					return;
				}
			}
		}
		bool flag = false;
		flag = UIPlayerProgressPanel.Get().IsVisible();
		UIPlayerProgressPanel.Get().SetVisible(!flag, needToUpdatePlayerProgress);
		if (!flag)
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
			UIPlayerProgressPanel.Get().LogPlayerChanges();
			return;
		}
	}

	public void ToggleStoreVisibility()
	{
		UIStorePanel.Get().ToggleStore();
		UIPlayerProgressPanel.Get().SetVisible(false);
	}

	public void TogglePlayerFriendListVisibility()
	{
		FriendListPanel.Get().SetVisible(!FriendListPanel.Get().IsVisible());
	}

	public void ShowScreen(FrontEndScreenState newScreen, bool refreshOnly = false)
	{
		m_currentScreen = newScreen;
		if (UICharacterSelectScreenController.Get() != null)
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
			UICharacterSelectScreenController.Get().SetupReadyButton();
		}
		bool visible = false;
		bool visible2 = false;
		bool visible3 = false;
		bool visible4 = false;
		bool visible5 = false;
		bool doActive = false;
		bool flag = false;
		int num;
		switch (newScreen)
		{
		case FrontEndScreenState.LandingPage:
			visible3 = true;
			doActive = true;
			visible2 = true;
			flag = true;
			Get().m_frontEndNavPanel.SetPlayMenuCatgeoryVisible(false);
			break;
		case FrontEndScreenState.CharacterSelect:
		case FrontEndScreenState.GroupCharacterSelect:
			if (UILootMatrixScreen.Get() != null && !UILootMatrixScreen.Get().IsVisible && UIStorePanel.Get() != null && !UIStorePanel.Get().IsVisible())
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
				visible = true;
				goto IL_0173;
			}
			if (!AppState.IsInGame())
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
				if (GameManager.Get().GameInfo != null)
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
					if (GameManager.Get().GameInfo.IsCustomGame)
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
						num = ((GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped) ? 1 : 0);
						goto IL_0154;
					}
				}
			}
			num = 0;
			goto IL_0154;
		case FrontEndScreenState.RankedModeSelect:
			visible2 = true;
			doActive = true;
			flag = true;
			break;
		case FrontEndScreenState.GameTypeSelect:
			doActive = true;
			visible2 = true;
			break;
		case FrontEndScreenState.JoinGame:
			visible4 = true;
			m_joinGameScreen.Setup();
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
		case FrontEndScreenState.CreateGame:
			{
				visible5 = true;
				doActive = true;
				visible2 = true;
				break;
			}
			IL_0154:
			if (num != 0)
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
				visible = true;
				UILootMatrixScreen.Get().SetVisible(false);
			}
			goto IL_0173;
			IL_0173:
			visible2 = true;
			doActive = true;
			flag = true;
			break;
		}
		if (ClientGameManager.Get().IsServerLocked)
		{
			visible2 = false;
		}
		if (m_playerPanel != null)
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
			m_playerPanel.SetVisible(visible2, refreshOnly);
		}
		if (m_joinGameScreen != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			m_joinGameScreen.SetVisible(visible4);
		}
		if (m_createGameScreen != null)
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
			m_createGameScreen.SetVisible(visible5);
		}
		if (!refreshOnly)
		{
			if (UIPlayerProgressPanel.Get() != null)
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
				UIPlayerProgressPanel.Get().SetVisible(false);
			}
			if (UICharacterSelectWorldObjects.Get() != null)
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
				UICharacterSelectWorldObjects.Get().SetVisible(visible);
			}
			if (m_landingPageScreen != null)
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
				m_landingPageScreen.SetVisible(visible3);
			}
			if (UICharacterSelectScreen.Get() != null)
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
				UICharacterSelectScreen.Get().SetVisible(false);
			}
			if (m_frontEndChatConsole != null)
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
				UIManager.SetGameObjectActive(m_frontEndChatConsole, doActive);
			}
		}
		if (UICharacterSelectScreenController.Get() != null)
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
			UICharacterSelectScreenController.Get().SetVisible(visible, refreshOnly);
		}
		if (!flag || !(m_frontEndChatConsole != null))
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
			m_frontEndChatConsole.ChangeChatRoom();
			return;
		}
	}

	public bool IsDraggingModel()
	{
		int result;
		if (!m_isDragging)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = (m_justStoppedDragging ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	private void Start()
	{
		UIScreenManager.Get().ClearAllPanels();
		m_currentCameraPosition = FrontEndScene.LobbyScreen;
		SetVisible(false);
	}

	private void OnDestroy()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager != null)
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
			clientGameManager.OnLobbyStatusNotification -= HandleLobbyStatusNotification;
		}
		if (!(s_instance == this))
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
			Log.Info(string.Concat(GetType(), " OnDestroy, clearing singleton reference"));
			s_instance = null;
			return;
		}
	}

	public void HandleLobbyStatusNotification(LobbyStatusNotification notification)
	{
		if (!(UIPlayerProgressPanel.Get() == null))
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
			if (UIPlayerProgressPanel.Get().IsVisible())
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
				break;
			}
		}
		ShowScreen(m_currentScreen, true);
	}

	public void ConfirmExit(UIDialogBox boxReference)
	{
		AppState_Shutdown.Get().Enter();
	}

	public void OnExitGameClick(BaseEventData data)
	{
		PlaySound(FrontEndButtonSounds.MenuChoice);
		UIDialogPopupManager.OpenTwoButtonDialog(StringUtil.TR("ExitGameTitle", "Global"), StringUtil.TR("ExitGamePrompt", "Global"), StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), ConfirmExit);
	}

	public void OnCreditsClick(BaseEventData data)
	{
		PlaySound(FrontEndButtonSounds.MenuChoice);
		if (UICreditsScreen.Get() != null)
		{
			UICreditsScreen.Get().SetVisible(true);
		}
		else
		{
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("CreditsTitle", "Global"), StringUtil.TR("CreditsBody", "Global"), StringUtil.TR("Close", "Global"), null, 20);
		}
	}

	public void ConfirmBack(UIDialogBox boxReference)
	{
		if (AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
		{
			AppState_LandingPage.Get().Enter(true);
			PlaySound(FrontEndButtonSounds.Back);
		}
		else
		{
			AppState_GameTeardown.Get().Enter();
		}
	}

	public void OnBackClick(BaseEventData data)
	{
		PlaySound(FrontEndButtonSounds.MenuChoice);
		if (AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					AppState_LandingPage.Get().Enter(true);
					return;
				}
			}
		}
		UIDialogPopupManager.OpenTwoButtonDialog(StringUtil.TR("LeavingGame", "Global"), StringUtil.TR("QuitGamePrompt", "Global"), StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), ConfirmBack);
	}

	public void Disable()
	{
		EnableFrontendEnvironment(false);
		if (!ClientQualityComponentEnabler.OptimizeForMemory())
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
			Resources.UnloadUnusedAssets();
			return;
		}
	}

	public void SetVisible(bool visible)
	{
		m_isVisible = visible;
		if (!(UI_Persistent.Get() != null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UI_Persistent.Get().NotifyFrontEndVisible(visible);
			return;
		}
	}

	public void ResetCharacterRotation()
	{
		m_currentRotationOffset = Vector3.zero;
	}

	public Vector3 GetRotationOffset()
	{
		return m_currentRotationOffset;
	}

	public void Update()
	{
		if (!m_attachedHandler)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (ClientGameManager.Get() != null)
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
				ClientGameManager.Get().OnLobbyStatusNotification += HandleLobbyStatusNotification;
				m_attachedHandler = true;
			}
		}
		if (s_firstLogInQuestCount != -1)
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
			if (QuestOfferPanel.Get() != null && !QuestOfferPanel.Get().IsActive() && QuestListPanel.Get() != null)
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
				if (Get().m_frontEndNavPanel != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					s_firstLogInQuestCount = -1;
					Get().m_frontEndNavPanel.NotificationBtnClicked(null);
				}
			}
		}
		if (!m_isVisible || UIManager.Get().GetEnvirontmentCamera() == null)
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
			if (DebugParameters.Get() != null)
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
				if (DebugParameters.Get().GetParameterAsBool("DebugCamera"))
				{
					while (true)
					{
						switch (2)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
			}
			float num = (Time.time - m_charCamStartTime) * m_cameraRotationSpeed;
			float t = 0f;
			if (m_rotationStartLength != 0f)
			{
				t = num / m_rotationStartLength;
			}
			if (m_lookAtOffset != m_cameraPositions[(int)m_currentCameraPosition].rotation)
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
				m_lookAtOffset = Vector3.Lerp(m_lookAtOffset, m_cameraPositions[(int)m_currentCameraPosition].rotation, t);
			}
			m_justStoppedDragging = false;
			if ((bool)UIManager.Get().GetEnvirontmentCamera())
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
				if (UICharacterSelectWorldObjects.Get() != null && UICharacterSelectWorldObjects.Get().m_ringAnimations[0] != null)
				{
					UIActorModelData componentInChildren = UICharacterSelectWorldObjects.Get().m_ringAnimations[0].GetComponentInChildren<UIActorModelData>();
					if (Input.GetMouseButtonDown(0))
					{
						while (true)
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
							m_isStartDrag = true;
							m_startDragPosition = Input.mousePosition;
							m_startRotation = m_currentRotationOffset;
						}
					}
				}
				if (!UIUtils.InputFieldHasFocus())
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
					if (AccountPreferences.DoesApplicationHaveFocus())
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
						if (Input.GetKey(KeyCode.RightArrow))
						{
							m_currentRotationOffset -= new Vector3(0f, 5f, 0f);
						}
						if (Input.GetKey(KeyCode.LeftArrow))
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
							m_currentRotationOffset += new Vector3(0f, 5f, 0f);
						}
					}
				}
			}
			if (UICharacterStoreAndProgressWorldObjects.Get() != null && UICharacterStoreAndProgressWorldObjects.Get().IsVisible())
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
				if (UICharacterStoreAndProgressWorldObjects.Get().m_ringAnimations[0] != null)
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
					UIActorModelData componentInChildren2 = UICharacterStoreAndProgressWorldObjects.Get().m_ringAnimations[0].GetComponentInChildren<UIActorModelData>();
					if (Input.GetMouseButtonDown(0))
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
						if (componentInChildren2 != null)
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
							if (componentInChildren2.MousedOver(UIManager.Get().GetEnvirontmentCamera()))
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
								m_isStartDrag = true;
								m_startDragPosition = Input.mousePosition;
								m_startRotation = m_currentRotationOffset;
							}
						}
					}
				}
			}
			if (m_isStartDrag)
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
				if ((Input.mousePosition - m_startDragPosition).magnitude > 10f)
				{
					m_startDragPosition = Input.mousePosition;
					m_isStartDrag = false;
					m_isDragging = true;
				}
			}
			if (m_isDragging)
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
				m_lastMouseLocation = Input.mousePosition;
				Vector3 vector = m_startDragPosition - m_lastMouseLocation;
				m_currentRotationOffset = m_startRotation + new Vector3(0f, vector.x / (float)Screen.width * 360f, 0f);
			}
			if (Input.GetMouseButtonUp(0))
			{
				if (m_isDragging)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					m_justStoppedDragging = true;
				}
				m_isDragging = false;
				m_isStartDrag = false;
			}
			if (!(GameManager.Get() != null))
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
				if (GameManager.Get().GameplayOverrides.DisableControlPadInput)
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
					if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.RightStickX) != 0f)
					{
						m_currentRotationOffset += new Vector3(0f, ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.RightStickX) * 10f, 0f);
					}
					if (!(UICharacterSelectScreenController.Get() != null))
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
						if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.DpadX) > 0f)
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
							CharacterType characterType = UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay;
							bool flag = false;
							while (!flag)
							{
								characterType++;
								if (characterType >= CharacterType.Last)
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
									characterType = CharacterType.BattleMonk;
								}
								flag = GameManager.Get().IsCharacterAllowedForPlayers(characterType);
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
							UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
							{
								ClientRequestToServerSelectCharacter = characterType
							});
						}
						if (!(ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.DpadX) < 0f))
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
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
								{
									ClientRequestToServerSelectCharacter = characterType2
								});
								return;
							}
						}
					}
				}
			}
		}
	}

	public bool CanMenuEscape()
	{
		if (!IsProgressScreenOpen())
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
			if (UIStorePanel.Get() != null)
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
				if (!UIStorePanel.Get().CanOpenMenu())
				{
					goto IL_0044;
				}
			}
			if (AppState_RankModeDraft.Get() == AppState.GetCurrent())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
			bool flag = UIDialogPopupManager.Get() != null && UIDialogPopupManager.Get().IsDialogBoxOpen();
			int result;
			if (!IsChatWindowFocused())
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
				result = ((!flag) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}
		goto IL_0044;
		IL_0044:
		return false;
	}

	public bool IsChatWindowFocused()
	{
		int result;
		if (m_frontEndChatConsole != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_frontEndChatConsole.IsVisible())
			{
				result = ((EventSystem.current.currentSelectedGameObject == m_frontEndChatConsole.m_textInput.gameObject || m_frontEndChatConsole.InputJustcleared()) ? 1 : 0);
				goto IL_0074;
			}
		}
		result = 0;
		goto IL_0074;
		IL_0074:
		return (byte)result != 0;
	}

	public void NotifyGameLaunched()
	{
	}

	public static UICharacterWorldObjects GetVisibleCharacters()
	{
		if (UICharacterStoreAndProgressWorldObjects.Get() != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (UICharacterStoreAndProgressWorldObjects.Get().IsVisible())
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return UICharacterStoreAndProgressWorldObjects.Get();
					}
				}
			}
		}
		if (UICharacterSelectWorldObjects.Get() != null)
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
			if (UICharacterSelectWorldObjects.Get().IsVisible())
			{
				return UICharacterSelectWorldObjects.Get();
			}
		}
		return null;
	}
}
