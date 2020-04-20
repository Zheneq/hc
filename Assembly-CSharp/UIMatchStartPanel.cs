﻿using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMatchStartPanel : UIScene
{
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

	private UIMatchStartPanel.MatchStartCountdown m_currentDisplay;

	private GameStatus m_lastGameStatus;

	private bool m_canDisplayMatchFound = true;

	public static UIMatchStartPanel Get()
	{
		return UIMatchStartPanel.s_instance;
	}

	public bool IsVisible()
	{
		return this.m_isVisible;
	}

	public bool IsDuplicateFreelancerResolving()
	{
		bool result;
		if (this.m_duplicateFreelancerResolving)
		{
			if (this.m_currentDisplay != UIMatchStartPanel.MatchStartCountdown.ChooseNewFreelancer)
			{
				result = (this.m_currentDisplay == UIMatchStartPanel.MatchStartCountdown.ResolvingDuplicateFreelancer);
			}
			else
			{
				result = true;
			}
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.MatchStart;
	}

	public override void Awake()
	{
		UIMatchStartPanel.s_instance = this;
		UIManager.SetGameObjectActive(this.m_MatchFoundContainer, false, null);
		base.Awake();
	}

	public void SetVisible(bool visible, UIMatchStartPanel.MatchStartCountdown containerDisplayType)
	{
		if (visible)
		{
			if (HitchDetector.Get() != null)
			{
				HitchDetector.Get().RecordFrameTimeForHitch("Setting Match Start Panel Visible: " + visible);
			}
			Component matchFoundContainer = this.m_MatchFoundContainer;
			bool doActive;
			if (containerDisplayType != UIMatchStartPanel.MatchStartCountdown.MatchFound)
			{
				doActive = (containerDisplayType == UIMatchStartPanel.MatchStartCountdown.LoadingMatch);
			}
			else
			{
				doActive = true;
			}
			UIManager.SetGameObjectActive(matchFoundContainer, doActive, null);
			if (this.m_canDisplayMatchFound)
			{
				if (!this.m_matchFoundAnimator.gameObject.activeSelf)
				{
					this.m_canDisplayMatchFound = false;
					Component matchFoundAnimator = this.m_matchFoundAnimator;
					bool doActive2;
					if (containerDisplayType != UIMatchStartPanel.MatchStartCountdown.MatchFound)
					{
						if (containerDisplayType != UIMatchStartPanel.MatchStartCountdown.ResolvingDuplicateFreelancer)
						{
							doActive2 = (containerDisplayType == UIMatchStartPanel.MatchStartCountdown.ChooseNewFreelancer);
							goto IL_C1;
						}
					}
					doActive2 = true;
					IL_C1:
					UIManager.SetGameObjectActive(matchFoundAnimator, doActive2, null);
				}
			}
			UIManager.SetGameObjectActive(this.m_chooseNewFreelancerContainer, containerDisplayType == UIMatchStartPanel.MatchStartCountdown.ChooseNewFreelancer, null);
			UIManager.SetGameObjectActive(this.m_resolvingFreelancerContainer, containerDisplayType == UIMatchStartPanel.MatchStartCountdown.ResolvingDuplicateFreelancer, null);
			if (containerDisplayType == UIMatchStartPanel.MatchStartCountdown.ChooseNewFreelancer)
			{
				UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_miscCharSelectButtons.gameObject, false, null);
			}
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_MatchFoundContainer, false, null);
			UIManager.SetGameObjectActive(this.m_chooseNewFreelancerContainer, false, null);
			UIManager.SetGameObjectActive(this.m_resolvingFreelancerContainer, false, null);
			UIManager.SetGameObjectActive(this.m_matchFoundAnimator, false, null);
			this.m_canDisplayMatchFound = true;
		}
		if (this.m_isVisible != visible && !this.m_isVisible)
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
		this.m_duplicateFreelancerResolving = isResolving;
	}

	public void SetSelfRingReady()
	{
		UICharacterSelectWorldObjects uicharacterSelectWorldObjects = UICharacterSelectWorldObjects.Get();
		if (uicharacterSelectWorldObjects != null && ClientGameManager.Get().GroupInfo != null)
		{
			if (!ClientGameManager.Get().GroupInfo.InAGroup)
			{
				if (!uicharacterSelectWorldObjects.m_ringAnimations[0].m_readyAnimation.gameObject.activeSelf)
				{
					uicharacterSelectWorldObjects.m_ringAnimations[0].PlayAnimation("ReadyIn");
				}
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
					if (gameManager.GameInfo.GameStatus == GameStatus.LoadoutSelecting)
					{
						result = true;
					}
					else
					{
						if (gameManager.GameInfo.GameStatus == GameStatus.FreelancerSelecting)
						{
							if (!(UIMatchStartPanel.Get() == null))
							{
								if (!(UIMatchStartPanel.Get() != null))
								{
									goto IL_168;
								}
								if (!UIMatchStartPanel.Get().m_duplicateFreelancerResolving)
								{
									goto IL_168;
								}
							}
							if (gameManager.GameInfo.GameConfig.GameType != GameType.Custom && gameManager.GameInfo.GameConfig.GameType != GameType.Practice)
							{
								if (gameManager.GameInfo.GameConfig.GameType != GameType.Solo)
								{
									if (!gameManager.GameInfo.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
									{
										return true;
									}
								}
							}
						}
						IL_168:
						if (gameManager.GameInfo.GameStatus >= GameStatus.Launching)
						{
							result = true;
						}
					}
				}
			}
		}
		return result;
	}

	public void Update()
	{
		if (UIRankedModeDraftScreen.Get() != null && UIRankedModeDraftScreen.Get().IsVisible)
		{
			return;
		}
		bool flag = false;
		UIMatchStartPanel.MatchStartCountdown matchStartCountdown = UIMatchStartPanel.MatchStartCountdown.None;
		if (!(AppState.GetCurrent() == AppState_CharacterSelect.Get()))
		{
			if (!(AppState.GetCurrent() == AppState_GroupCharacterSelect.Get()))
			{
				goto IL_2F1;
			}
		}
		GameManager gameManager = GameManager.Get();
		if (gameManager != null)
		{
			if (gameManager.GameInfo != null)
			{
				if (gameManager.GameInfo.GameConfig != null && gameManager.GameInfo.GameStatus != GameStatus.Stopped)
				{
					MapData mapData = GameWideData.Get().GetMapData(gameManager.GameInfo.GameConfig.Map);
					this.m_introMapText.text = GameWideData.Get().GetMapDisplayName(gameManager.GameInfo.GameConfig.Map);
					Sprite sprite;
					if (mapData != null)
					{
						sprite = (Resources.Load(mapData.ResourceImageSpriteLocation, typeof(Sprite)) as Sprite);
					}
					else
					{
						sprite = (Resources.Load("Stages/information_stage_image", typeof(Sprite)) as Sprite);
					}
					this.m_introMapImage.sprite = sprite;
					if (gameManager.GameInfo.GameConfig.InstanceSubType.LocalizedName != null)
					{
						this.m_introGameTypeText.text = string.Format(StringUtil.TR("SubtypeFound", "Global"), StringUtil.TR(gameManager.GameInfo.GameConfig.InstanceSubType.LocalizedName));
					}
					else
					{
						this.m_introGameTypeText.text = string.Empty;
					}
					if (gameManager.GameInfo.GameStatus == GameStatus.LoadoutSelecting)
					{
						flag = true;
						matchStartCountdown = UIMatchStartPanel.MatchStartCountdown.MatchFound;
						this.SetSelfRingReady();
					}
					else
					{
						if (gameManager.GameInfo.GameStatus == GameStatus.FreelancerSelecting)
						{
							if (this.m_duplicateFreelancerResolving && gameManager.GameInfo.GameConfig.GameType != GameType.Custom && gameManager.GameInfo.GameConfig.GameType != GameType.Practice)
							{
								if (gameManager.GameInfo.GameConfig.GameType != GameType.Solo)
								{
									if (!gameManager.GameInfo.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
									{
										flag = true;
										if (UICharacterSelectScreenController.Get().RepickingCharacter())
										{
											matchStartCountdown = UIMatchStartPanel.MatchStartCountdown.ChooseNewFreelancer;
										}
										else
										{
											matchStartCountdown = UIMatchStartPanel.MatchStartCountdown.ResolvingDuplicateFreelancer;
											this.SetSelfRingReady();
										}
										goto IL_2F1;
									}
								}
							}
						}
						if (gameManager.GameInfo.GameStatus >= GameStatus.Launching)
						{
							flag = true;
							matchStartCountdown = UIMatchStartPanel.MatchStartCountdown.LoadingMatch;
							this.SetSelfRingReady();
						}
					}
				}
			}
		}
		IL_2F1:
		this.m_isVisible = flag;
		this.m_currentDisplay = matchStartCountdown;
		if (this.m_matchFoundAnimator.gameObject.activeInHierarchy)
		{
			if (UIAnimationEventManager.IsAnimationDone(this.m_matchFoundAnimator, "MatchFoundIntro", 0))
			{
				UIManager.SetGameObjectActive(this.m_matchFoundAnimator, false, null);
			}
		}
		if (flag)
		{
			this.SetVisible(flag, matchStartCountdown);
			if (matchStartCountdown == UIMatchStartPanel.MatchStartCountdown.MatchFound)
			{
				LobbyGameInfo gameInfo = GameManager.Get().GameInfo;
				float num = Time.realtimeSinceStartup - this.m_loadoutSelectStartTime;
				float num2 = Mathf.Max(0f, (float)gameInfo.LoadoutSelectTimeout.TotalSeconds - num);
				this.m_matchFoundText.text = StringUtil.TR("SelectModsAndCatalysts", "Global");
				this.m_countdownTimerText.text = string.Format("{0}", (int)num2 + 1);
				AnnouncerSounds.GetAnnouncerSounds().PlayCountdownAnnouncementIfAppropriate(this.m_previousTimeRemaining, num2);
				if (Mathf.Floor(this.m_previousTimeRemaining) != Mathf.Floor(num2))
				{
					UIManager.SetGameObjectActive(this.m_countdownNumberController, true, null);
					this.m_countdownNumberController.Play("matchStartCountdownDefaultIN", 0, 0f);
				}
				bool? characterSelectButtonsVisible = UICharacterScreen.GetCurrentSpecificState().CharacterSelectButtonsVisible;
				if (characterSelectButtonsVisible.Value)
				{
					UICharacterSelectScreenController.Get().SetCharacterSelectVisible(false);
					UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_changeFreelancerBtn, false, null);
				}
				this.m_previousTimeRemaining = num2;
				this.UpdateCharacterList();
			}
			else if (matchStartCountdown == UIMatchStartPanel.MatchStartCountdown.ChooseNewFreelancer || matchStartCountdown == UIMatchStartPanel.MatchStartCountdown.ResolvingDuplicateFreelancer)
			{
				float num3 = Time.realtimeSinceStartup - this.m_selectStartTime;
				float num4 = Mathf.Max(0f, (float)GameManager.Get().GameInfo.SelectTimeout.TotalSeconds - num3);
				this.m_chooseNewFreelancerTimerText.text = string.Format("{0}", (int)num4 + 1);
				this.m_resolvingDuplicateFreelancerTimerText.text = string.Format("{0}", (int)num4 + 1);
			}
			else if (matchStartCountdown == UIMatchStartPanel.MatchStartCountdown.LoadingMatch)
			{
				UICharacterSelectScreenController.Get().NotifyGameIsLoading();
				this.m_matchFoundText.text = "\n" + StringUtil.TR("LoadingMatch", "Global");
				this.m_countdownTimerText.text = string.Empty;
				UIManager.SetGameObjectActive(this.m_countdownNumberController, false, null);
				this.UpdateCharacterList();
			}
			if (UIMainMenu.Get().IsOpen())
			{
				UIMainMenu.Get().SetMenuVisible(false, false);
			}
			UIStorePanel.Get().ClosePurchaseDialog();
			if (FrontEndNavPanel.Get() != null)
			{
				FrontEndNavPanel.Get().PlayBtnClicked(null);
				UIManager.SetGameObjectActive(FrontEndNavPanel.Get(), false, null);
			}
		}
		else
		{
			this.SetVisible(false, UIMatchStartPanel.MatchStartCountdown.None);
			if (FrontEndNavPanel.Get() != null)
			{
				UIManager.SetGameObjectActive(FrontEndNavPanel.Get(), true, null);
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
					this.m_mapImage.sprite = (Resources.Load(mapData.ResourceImageSpriteLocation, typeof(Sprite)) as Sprite);
				}
				else
				{
					this.m_mapImage.sprite = (Resources.Load("Stages/information_stage_image", typeof(Sprite)) as Sprite);
				}
				this.m_mapName.text = GameWideData.Get().GetMapDisplayName(gameInfo.GameConfig.Map);
				int i = 0;
				if (playerInfo.TeamId == Team.TeamA)
				{
					IEnumerator<LobbyPlayerInfo> enumerator = teamInfo.TeamBPlayerInfo.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
							if (i < this.m_enemyCharacterImages.Length)
							{
								UIManager.SetGameObjectActive(this.m_enemyCharacterContainers[i], true, null);
								CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(lobbyPlayerInfo.CharacterType);
								this.m_enemyCharacterImages[i].sprite = characterResourceLink.GetCharacterSelectIcon();
								i++;
							}
						}
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				else
				{
					foreach (LobbyPlayerInfo lobbyPlayerInfo2 in teamInfo.TeamAPlayerInfo)
					{
						if (i < this.m_enemyCharacterImages.Length)
						{
							UIManager.SetGameObjectActive(this.m_enemyCharacterContainers[i], true, null);
							CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink(lobbyPlayerInfo2.CharacterType);
							this.m_enemyCharacterImages[i].sprite = characterResourceLink2.GetCharacterSelectIcon();
							i++;
						}
					}
				}
				while (i < this.m_enemyCharacterContainers.Length)
				{
					UIManager.SetGameObjectActive(this.m_enemyCharacterContainers[i], false, null);
					i++;
				}
				return;
			}
		}
		for (int j = 0; j < this.m_enemyCharacterImages.Length; j++)
		{
			UIManager.SetGameObjectActive(this.m_enemyCharacterContainers[j], false, null);
		}
	}

	public void HandleGameStatusChanged(GameInfoNotification notification)
	{
		if (notification.GameInfo.GameStatus == GameStatus.LoadoutSelecting)
		{
			if (this.m_lastGameStatus != notification.GameInfo.GameStatus)
			{
				this.m_loadoutSelectStartTime = Time.realtimeSinceStartup;
			}
		}
		else if (notification.GameInfo.GameStatus == GameStatus.FreelancerSelecting)
		{
			if (this.m_lastGameStatus != notification.GameInfo.GameStatus)
			{
				this.m_selectStartTime = Time.realtimeSinceStartup;
			}
			if (notification.GameInfo.GameConfig.GameType == GameType.Ranked && AppState_RankModeDraft.Get() != AppState.GetCurrent())
			{
				AppState_RankModeDraft.Get().Enter();
			}
		}
		this.m_lastGameStatus = notification.GameInfo.GameStatus;
	}

	public enum MatchStartCountdown
	{
		None,
		MatchFound,
		ChooseNewFreelancer,
		ResolvingDuplicateFreelancer,
		LoadingMatch,
		MatchFoundCountdown
	}
}
