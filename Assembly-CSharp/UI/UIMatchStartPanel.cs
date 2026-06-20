using LobbyGameClientMessages;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMatchStartPanel : UIScene
{
	public enum MatchStartCountdown
	{
		None,
		MatchFound,
		ChooseNewFreelancer,
		ResolvingDuplicateFreelancer,
		LoadingMatch,
		MatchFoundCountdown
	}

	public static UIMatchStartPanel s_instance;

	public Image m_mapImage;
	public TextMeshProUGUI m_mapName;
	public Image[] m_enemyCharacterImages;
	public RectTransform[] m_enemyCharacterContainers;
	public RectTransform m_MatchFoundContainer;
	public TextMeshProUGUI m_matchFoundText;
	public TextMeshProUGUI m_countdownTimerText;
	public RectTransform m_chooseNewFreelancerContainer;
	public RectTransform m_resolvingFreelancerContainer;
	public TextMeshProUGUI m_chooseNewFreelancerTimerText;
	public TextMeshProUGUI m_resolvingDuplicateFreelancerTimerText;
	public Animator m_countdownNumberController;
	public Animator m_matchStartPanelAnimator;
	public Animator m_matchFoundAnimator;
	public TextMeshProUGUI m_introGameTypeText;
	public Image m_introMapImage;
	public TextMeshProUGUI m_introMapText;

	private float m_loadoutSelectStartTime;
	private float m_selectStartTime;
	private float m_previousTimeRemaining;
	private bool m_duplicateFreelancerResolving;
	private bool m_isVisible;
	private MatchStartCountdown m_currentDisplay;
	private GameStatus m_lastGameStatus;
	private bool m_canDisplayMatchFound = true;

	public static UIMatchStartPanel Get()
	{
		return s_instance;
	}

	public bool IsVisible()
	{
		return m_isVisible;
	}

	public bool IsDuplicateFreelancerResolving()
	{
		return m_duplicateFreelancerResolving
		       && (m_currentDisplay == MatchStartCountdown.ChooseNewFreelancer
		           || m_currentDisplay == MatchStartCountdown.ResolvingDuplicateFreelancer);
	}

	public override SceneType GetSceneType()
	{
		return SceneType.MatchStart;
	}

	public override void Awake()
	{
		s_instance = this;
		UIManager.SetGameObjectActive(m_MatchFoundContainer, false);
		base.Awake();
	}

	public void SetVisible(bool visible, MatchStartCountdown containerDisplayType)
	{
		if (visible)
		{
			if (HitchDetector.Get() != null)
			{
				HitchDetector.Get().RecordFrameTimeForHitch("Setting Match Start Panel Visible: " + visible);
			}

			UIManager.SetGameObjectActive(m_MatchFoundContainer, 
				containerDisplayType == MatchStartCountdown.MatchFound
				|| containerDisplayType == MatchStartCountdown.LoadingMatch);
			if (m_canDisplayMatchFound && !m_matchFoundAnimator.gameObject.activeSelf)
			{
				m_canDisplayMatchFound = false;
				UIManager.SetGameObjectActive(m_matchFoundAnimator, 
					containerDisplayType == MatchStartCountdown.MatchFound
					|| containerDisplayType == MatchStartCountdown.ResolvingDuplicateFreelancer
					|| containerDisplayType == MatchStartCountdown.ChooseNewFreelancer);
			}
			UIManager.SetGameObjectActive(m_chooseNewFreelancerContainer, 
				containerDisplayType == MatchStartCountdown.ChooseNewFreelancer);
			UIManager.SetGameObjectActive(m_resolvingFreelancerContainer, 
				containerDisplayType == MatchStartCountdown.ResolvingDuplicateFreelancer);
			if (containerDisplayType == MatchStartCountdown.ChooseNewFreelancer)
			{
				UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_miscCharSelectButtons.gameObject, false);
			}
		}
		else
		{
			UIManager.SetGameObjectActive(m_MatchFoundContainer, false);
			UIManager.SetGameObjectActive(m_chooseNewFreelancerContainer, false);
			UIManager.SetGameObjectActive(m_resolvingFreelancerContainer, false);
			UIManager.SetGameObjectActive(m_matchFoundAnimator, false);
			m_canDisplayMatchFound = true;
		}

		if (m_isVisible != visible && !m_isVisible)
		{
			UICharacterSelectScreenController.Get().NotifyGroupUpdate();
			if (UIFrontEnd.Get().m_frontEndNavPanel != null)
			{
				UIFrontEnd.Get().m_frontEndNavPanel.ToggleUiForGameStarting(true);
			}
		}
	}

	public void NotifyDuplicateFreelancer(bool isResolving)
	{
		m_duplicateFreelancerResolving = isResolving;
	}

	public void SetSelfRingReady()
	{
		UICharacterSelectWorldObjects uICharacterSelectWorldObjects = UICharacterSelectWorldObjects.Get();
		if (uICharacterSelectWorldObjects == null
		    || ClientGameManager.Get().GroupInfo == null
		    || ClientGameManager.Get().GroupInfo.InAGroup)
		{
			return;
		}
		if (!uICharacterSelectWorldObjects.m_ringAnimations[0].m_readyAnimation.gameObject.activeSelf)
		{
			uICharacterSelectWorldObjects.m_ringAnimations[0].PlayAnimation("ReadyIn");
		}
	}

	public static bool IsMatchCountdownStarting()
	{
		GameManager gameManager = GameManager.Get();
		return gameManager != null
		       && gameManager.GameInfo != null
		       && gameManager.GameInfo.GameConfig != null
		       && gameManager.GameInfo.GameStatus != GameStatus.Stopped
		       && (gameManager.GameInfo.GameStatus == GameStatus.LoadoutSelecting
		           || gameManager.GameInfo.GameStatus == GameStatus.FreelancerSelecting 
					&& (Get() == null || (Get() != null && Get().m_duplicateFreelancerResolving))
					&& gameManager.GameInfo.GameConfig.GameType != GameType.Custom
					&& gameManager.GameInfo.GameConfig.GameType != GameType.Practice
					&& gameManager.GameInfo.GameConfig.GameType != GameType.Solo
					&& !gameManager.GameInfo.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial)
		           || gameManager.GameInfo.GameStatus >= GameStatus.Launching);
	}

	public void Update()
	{
		if (UIRankedModeDraftScreen.Get() != null && UIRankedModeDraftScreen.Get().IsVisible)
		{
			return;
		}
		bool isVisible = false;
		MatchStartCountdown matchStartCountdown = MatchStartCountdown.None;
		if (AppState.GetCurrent() == AppState_CharacterSelect.Get()
		    || AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
		{
			GameManager gameManager = GameManager.Get();
			if (gameManager != null
			    && gameManager.GameInfo != null
			    && gameManager.GameInfo.GameConfig != null
			    && gameManager.GameInfo.GameStatus != GameStatus.Stopped)
			{
				MapData mapData = GameWideData.Get().GetMapData(gameManager.GameInfo.GameConfig.Map);
				m_introMapText.text = GameWideData.Get().GetMapDisplayName(gameManager.GameInfo.GameConfig.Map);

				m_introMapImage.sprite = mapData != null
					? Resources.Load(mapData.ResourceImageSpriteLocation, typeof(Sprite)) as Sprite
					: Resources.Load("Stages/information_stage_image", typeof(Sprite)) as Sprite;
				m_introGameTypeText.text = gameManager.GameInfo.GameConfig.InstanceSubType.LocalizedName != null
					? string.Format(
						StringUtil.TR("SubtypeFound", "Global"),
						StringUtil.TR(gameManager.GameInfo.GameConfig.InstanceSubType.LocalizedName))
					: string.Empty;

				if (gameManager.GameInfo.GameStatus == GameStatus.LoadoutSelecting)
				{
					isVisible = true;
					matchStartCountdown = MatchStartCountdown.MatchFound;
					SetSelfRingReady();
				}
				else if (gameManager.GameInfo.GameStatus == GameStatus.FreelancerSelecting
				         && m_duplicateFreelancerResolving
				         && gameManager.GameInfo.GameConfig.GameType != GameType.Custom
				         && gameManager.GameInfo.GameConfig.GameType != GameType.Practice
				         && gameManager.GameInfo.GameConfig.GameType != GameType.Solo
				         && !gameManager.GameInfo.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
				{
					isVisible = true;
					if (UICharacterSelectScreenController.Get().RepickingCharacter())
					{
						matchStartCountdown = MatchStartCountdown.ChooseNewFreelancer;
					}
					else
					{
						matchStartCountdown = MatchStartCountdown.ResolvingDuplicateFreelancer;
						SetSelfRingReady();
					}
				}
				else if (gameManager.GameInfo.GameStatus >= GameStatus.Launching)
				{
					isVisible = true;
					matchStartCountdown = MatchStartCountdown.LoadingMatch;
					SetSelfRingReady();
				}
			}
		}
		m_isVisible = isVisible;
		m_currentDisplay = matchStartCountdown;
		if (m_matchFoundAnimator.gameObject.activeInHierarchy
		    && UIAnimationEventManager.IsAnimationDone(m_matchFoundAnimator, "MatchFoundIntro", 0))
		{
			UIManager.SetGameObjectActive(m_matchFoundAnimator, false);
		}
		if (isVisible)
		{
			SetVisible(isVisible, matchStartCountdown);
			if (matchStartCountdown == MatchStartCountdown.MatchFound)
			{
				LobbyGameInfo gameInfo = GameManager.Get().GameInfo;
				float timeSinceLoadoutSelectStart = Time.realtimeSinceStartup - m_loadoutSelectStartTime;
				float loadoutSelectTimeRemaining = Mathf.Max(0f, (float)gameInfo.LoadoutSelectTimeout.TotalSeconds - timeSinceLoadoutSelectStart);
				m_matchFoundText.text = StringUtil.TR("SelectModsAndCatalysts", "Global");
				m_countdownTimerText.text = $"{(int)loadoutSelectTimeRemaining + 1}";
				AnnouncerSounds.GetAnnouncerSounds().PlayCountdownAnnouncementIfAppropriate(m_previousTimeRemaining, loadoutSelectTimeRemaining);
				if (Mathf.Floor(m_previousTimeRemaining) != Mathf.Floor(loadoutSelectTimeRemaining))
				{
					UIManager.SetGameObjectActive(m_countdownNumberController, true);
					m_countdownNumberController.Play("matchStartCountdownDefaultIN", 0, 0f);
				}
				if (UICharacterScreen.GetCurrentSpecificState().CharacterSelectButtonsVisible.Value)
				{
					UICharacterSelectScreenController.Get().SetCharacterSelectVisible(false);
					UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_changeFreelancerBtn, false);
				}
				m_previousTimeRemaining = loadoutSelectTimeRemaining;
				UpdateCharacterList();
			}
			else if (matchStartCountdown == MatchStartCountdown.ChooseNewFreelancer
			         || matchStartCountdown == MatchStartCountdown.ResolvingDuplicateFreelancer)
			{
				float timeSinceFreelancerSelectStart = Time.realtimeSinceStartup - m_selectStartTime;
				float freelancerSelectTimeRemaining = Mathf.Max(0f, (float)GameManager.Get().GameInfo.SelectTimeout.TotalSeconds - timeSinceFreelancerSelectStart);
				m_chooseNewFreelancerTimerText.text = $"{(int)freelancerSelectTimeRemaining + 1}";
				m_resolvingDuplicateFreelancerTimerText.text = $"{(int)freelancerSelectTimeRemaining + 1}";
			}
			else if (matchStartCountdown == MatchStartCountdown.LoadingMatch)
			{
				UICharacterSelectScreenController.Get().NotifyGameIsLoading();
				m_matchFoundText.text = "\n" + StringUtil.TR("LoadingMatch", "Global");
				m_countdownTimerText.text = string.Empty;
				UIManager.SetGameObjectActive(m_countdownNumberController, false);
				UpdateCharacterList();
			}
			if (UIMainMenu.Get().IsOpen())
			{
				UIMainMenu.Get().SetMenuVisible(false);
			}
			UIStorePanel.Get().ClosePurchaseDialog();
			if (FrontEndNavPanel.Get() != null)
			{
				FrontEndNavPanel.Get().PlayBtnClicked(null);
				UIManager.SetGameObjectActive(FrontEndNavPanel.Get(), false);
			}
		}
		else
		{
			SetVisible(false, MatchStartCountdown.None);
			if (FrontEndNavPanel.Get() != null)
			{
				UIManager.SetGameObjectActive(FrontEndNavPanel.Get(), true);
			}
		}
	}

	public void UpdateCharacterList()
	{
		GameManager gameManager = GameManager.Get();
		LobbyGameInfo gameInfo = gameManager.GameInfo;
		LobbyPlayerInfo playerInfo = gameManager.PlayerInfo;
		LobbyTeamInfo teamInfo = gameManager.TeamInfo;
		
		if (gameInfo == null || playerInfo == null || teamInfo == null)
		{
			for (int j = 0; j < m_enemyCharacterImages.Length; j++)
			{
				UIManager.SetGameObjectActive(m_enemyCharacterContainers[j], false);
			}

			return;
		}
		
		MapData mapData = GameWideData.Get().GetMapData(gameInfo.GameConfig.Map);
		m_mapImage.sprite = mapData != null
			? Resources.Load(mapData.ResourceImageSpriteLocation, typeof(Sprite)) as Sprite
			: Resources.Load("Stages/information_stage_image", typeof(Sprite)) as Sprite;
		m_mapName.text = GameWideData.Get().GetMapDisplayName(gameInfo.GameConfig.Map);
		IEnumerable<LobbyPlayerInfo> enemyTeamInfo = playerInfo.TeamId == Team.TeamA
			? teamInfo.TeamBPlayerInfo
			: teamInfo.TeamAPlayerInfo;
		int i = 0;
		foreach (LobbyPlayerInfo item in enemyTeamInfo)
		{
			if (i < m_enemyCharacterImages.Length)
			{
				UIManager.SetGameObjectActive(m_enemyCharacterContainers[i], true);
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(item.CharacterType);
				m_enemyCharacterImages[i].sprite = characterResourceLink.GetCharacterSelectIcon();
				i++;
			}
		}

		for (; i < m_enemyCharacterContainers.Length; i++)
		{
			UIManager.SetGameObjectActive(m_enemyCharacterContainers[i], false);
		}
	}

	public void HandleGameStatusChanged(GameInfoNotification notification)
	{
		if (notification.GameInfo.GameStatus == GameStatus.LoadoutSelecting)
		{
			if (m_lastGameStatus != notification.GameInfo.GameStatus)
			{
				m_loadoutSelectStartTime = Time.realtimeSinceStartup;
			}
		}
		else if (notification.GameInfo.GameStatus == GameStatus.FreelancerSelecting)
		{
			if (m_lastGameStatus != notification.GameInfo.GameStatus)
			{
				m_selectStartTime = Time.realtimeSinceStartup;
			}
			if (notification.GameInfo.GameConfig.GameType == GameType.Ranked && AppState_RankModeDraft.Get() != AppState.GetCurrent())
			{
				AppState_RankModeDraft.Get().Enter();
			}
		}
		m_lastGameStatus = notification.GameInfo.GameStatus;
	}
}
