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
		int result;
		if (m_duplicateFreelancerResolving)
		{
			if (m_currentDisplay != MatchStartCountdown.ChooseNewFreelancer)
			{
				result = ((m_currentDisplay == MatchStartCountdown.ResolvingDuplicateFreelancer) ? 1 : 0);
			}
			else
			{
				result = 1;
			}
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
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
		Animator matchFoundAnimator;
		int doActive2;
		if (visible)
		{
			if (HitchDetector.Get() != null)
			{
				HitchDetector.Get().RecordFrameTimeForHitch("Setting Match Start Panel Visible: " + visible);
			}
			RectTransform matchFoundContainer = m_MatchFoundContainer;
			int doActive;
			if (containerDisplayType != MatchStartCountdown.MatchFound)
			{
				doActive = ((containerDisplayType == MatchStartCountdown.LoadingMatch) ? 1 : 0);
			}
			else
			{
				doActive = 1;
			}
			UIManager.SetGameObjectActive(matchFoundContainer, (byte)doActive != 0);
			if (m_canDisplayMatchFound)
			{
				if (!m_matchFoundAnimator.gameObject.activeSelf)
				{
					m_canDisplayMatchFound = false;
					matchFoundAnimator = m_matchFoundAnimator;
					if (containerDisplayType != MatchStartCountdown.MatchFound)
					{
						if (containerDisplayType != MatchStartCountdown.ResolvingDuplicateFreelancer)
						{
							doActive2 = ((containerDisplayType == MatchStartCountdown.ChooseNewFreelancer) ? 1 : 0);
							goto IL_00c1;
						}
					}
					doActive2 = 1;
					goto IL_00c1;
				}
			}
			goto IL_00c7;
		}
		UIManager.SetGameObjectActive(m_MatchFoundContainer, false);
		UIManager.SetGameObjectActive(m_chooseNewFreelancerContainer, false);
		UIManager.SetGameObjectActive(m_resolvingFreelancerContainer, false);
		UIManager.SetGameObjectActive(m_matchFoundAnimator, false);
		m_canDisplayMatchFound = true;
		goto IL_014a;
		IL_00c7:
		UIManager.SetGameObjectActive(m_chooseNewFreelancerContainer, containerDisplayType == MatchStartCountdown.ChooseNewFreelancer);
		UIManager.SetGameObjectActive(m_resolvingFreelancerContainer, containerDisplayType == MatchStartCountdown.ResolvingDuplicateFreelancer);
		if (containerDisplayType == MatchStartCountdown.ChooseNewFreelancer)
		{
			UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_miscCharSelectButtons.gameObject, false);
		}
		goto IL_014a;
		IL_014a:
		if (m_isVisible == visible || m_isVisible)
		{
			return;
		}
		while (true)
		{
			UICharacterSelectScreenController.Get().NotifyGroupUpdate();
			if (UIFrontEnd.Get().m_frontEndNavPanel != null)
			{
				while (true)
				{
					UIFrontEnd.Get().m_frontEndNavPanel.ToggleUiForGameStarting(true);
					return;
				}
			}
			return;
		}
		IL_00c1:
		UIManager.SetGameObjectActive(matchFoundAnimator, (byte)doActive2 != 0);
		goto IL_00c7;
	}

	public void NotifyDuplicateFreelancer(bool isResolving)
	{
		m_duplicateFreelancerResolving = isResolving;
	}

	public void SetSelfRingReady()
	{
		UICharacterSelectWorldObjects uICharacterSelectWorldObjects = UICharacterSelectWorldObjects.Get();
		if (!(uICharacterSelectWorldObjects != null) || ClientGameManager.Get().GroupInfo == null)
		{
			return;
		}
		while (true)
		{
			if (ClientGameManager.Get().GroupInfo.InAGroup)
			{
				return;
			}
			while (true)
			{
				if (!uICharacterSelectWorldObjects.m_ringAnimations[0].m_readyAnimation.gameObject.activeSelf)
				{
					while (true)
					{
						uICharacterSelectWorldObjects.m_ringAnimations[0].PlayAnimation("ReadyIn");
						return;
					}
				}
				return;
			}
		}
	}

	public static bool IsMatchCountdownStarting()
	{
		bool result = false;
		GameManager gameManager = GameManager.Get();
		if (gameManager != null)
		{
			if (gameManager.GameInfo != null)
			{
				if (gameManager.GameInfo.GameConfig != null && gameManager.GameInfo.GameStatus != GameStatus.Stopped)
				{
					if (gameManager.GameInfo.GameStatus != GameStatus.LoadoutSelecting)
					{
						if (gameManager.GameInfo.GameStatus == GameStatus.FreelancerSelecting)
						{
							if (Get() == null)
							{
								goto IL_00ee;
							}
							if (Get() != null)
							{
								if (Get().m_duplicateFreelancerResolving)
								{
									goto IL_00ee;
								}
							}
						}
						goto IL_0168;
					}
					result = true;
				}
			}
		}
		goto IL_0184;
		IL_0168:
		if (gameManager.GameInfo.GameStatus >= GameStatus.Launching)
		{
			result = true;
		}
		goto IL_0184;
		IL_00ee:
		if (gameManager.GameInfo.GameConfig.GameType != 0 && gameManager.GameInfo.GameConfig.GameType != GameType.Practice)
		{
			if (gameManager.GameInfo.GameConfig.GameType != GameType.Solo)
			{
				if (!gameManager.GameInfo.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
				{
					result = true;
					goto IL_0184;
				}
			}
		}
		goto IL_0168;
		IL_0184:
		return result;
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
				float num = Time.realtimeSinceStartup - m_loadoutSelectStartTime;
				float num2 = Mathf.Max(0f, (float)gameInfo.LoadoutSelectTimeout.TotalSeconds - num);
				m_matchFoundText.text = StringUtil.TR("SelectModsAndCatalysts", "Global");
				m_countdownTimerText.text = $"{(int)num2 + 1}";
				AnnouncerSounds.GetAnnouncerSounds().PlayCountdownAnnouncementIfAppropriate(m_previousTimeRemaining, num2);
				if (Mathf.Floor(m_previousTimeRemaining) != Mathf.Floor(num2))
				{
					UIManager.SetGameObjectActive(m_countdownNumberController, true);
					m_countdownNumberController.Play("matchStartCountdownDefaultIN", 0, 0f);
				}
				if (UICharacterScreen.GetCurrentSpecificState().CharacterSelectButtonsVisible.Value)
				{
					UICharacterSelectScreenController.Get().SetCharacterSelectVisible(false);
					UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_changeFreelancerBtn, false);
				}
				m_previousTimeRemaining = num2;
				UpdateCharacterList();
			}
			else if (matchStartCountdown == MatchStartCountdown.ChooseNewFreelancer || matchStartCountdown == MatchStartCountdown.ResolvingDuplicateFreelancer)
			{
				float num3 = Time.realtimeSinceStartup - m_selectStartTime;
				float num4 = Mathf.Max(0f, (float)GameManager.Get().GameInfo.SelectTimeout.TotalSeconds - num3);
				m_chooseNewFreelancerTimerText.text = $"{(int)num4 + 1}";
				m_resolvingDuplicateFreelancerTimerText.text = $"{(int)num4 + 1}";
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
		if (gameInfo != null && playerInfo != null)
		{
			if (teamInfo != null)
			{
				MapData mapData = GameWideData.Get().GetMapData(gameInfo.GameConfig.Map);
				if (mapData != null)
				{
					m_mapImage.sprite = (Resources.Load(mapData.ResourceImageSpriteLocation, typeof(Sprite)) as Sprite);
				}
				else
				{
					m_mapImage.sprite = (Resources.Load("Stages/information_stage_image", typeof(Sprite)) as Sprite);
				}
				m_mapName.text = GameWideData.Get().GetMapDisplayName(gameInfo.GameConfig.Map);
				int i = 0;
				if (playerInfo.TeamId == Team.TeamA)
				{
					IEnumerator<LobbyPlayerInfo> enumerator = teamInfo.TeamBPlayerInfo.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							LobbyPlayerInfo current = enumerator.Current;
							if (i < m_enemyCharacterImages.Length)
							{
								UIManager.SetGameObjectActive(m_enemyCharacterContainers[i], true);
								CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(current.CharacterType);
								m_enemyCharacterImages[i].sprite = characterResourceLink.GetCharacterSelectIcon();
								i++;
							}
						}
					}
					finally
					{
						if (enumerator != null)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									break;
								default:
									enumerator.Dispose();
									goto end_IL_01a7;
								}
							}
						}
						end_IL_01a7:;
					}
				}
				else
				{
					foreach (LobbyPlayerInfo item in teamInfo.TeamAPlayerInfo)
					{
						if (i < m_enemyCharacterImages.Length)
						{
							UIManager.SetGameObjectActive(m_enemyCharacterContainers[i], true);
							CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink(item.CharacterType);
							m_enemyCharacterImages[i].sprite = characterResourceLink2.GetCharacterSelectIcon();
							i++;
						}
					}
				}
				for (; i < m_enemyCharacterContainers.Length; i++)
				{
					UIManager.SetGameObjectActive(m_enemyCharacterContainers[i], false);
				}
				return;
			}
		}
		for (int j = 0; j < m_enemyCharacterImages.Length; j++)
		{
			UIManager.SetGameObjectActive(m_enemyCharacterContainers[j], false);
		}
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
