using System;
using System.Collections.Generic;
using System.Linq;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGameOverPlayerEntry : MonoBehaviour
{
	public TextMeshProUGUI m_playerLabel;

	public TextMeshProUGUI m_playerLevel;

	public TextMeshProUGUI m_playertitle;

	public TextMeshProUGUI m_KillsLabel;

	public TextMeshProUGUI m_DeathsLabel;

	public TextMeshProUGUI m_DamageDealt;

	public TextMeshProUGUI m_RoleNumber;

	public TextMeshProUGUI m_freelancerLabel;

	public TextMeshProUGUI m_KDANumber;

	public TextMeshProUGUI m_HealingNumber;

	public TextMeshProUGUI m_DamageReceivedNumber;

	public TextMeshProUGUI m_TimePerTurn;

	public GameObject m_DebugStats;

	public TextMeshProUGUI m_DebugTotalContribution;

	public TextMeshProUGUI m_DebugAccountELO;

	public TextMeshProUGUI m_DebugCharacterELO;

	public Image m_DebugTeamAImage;

	public Image m_DebugTeamBImage;

	public TextMeshProUGUI m_DebugBotNameText;

	public TextMeshProUGUI m_DebugBotDifficultyText;

	public RectTransform m_botBanner;

	public RectTransform m_playerBanner;

	public Image m_botOverlay;

	public Image m_characterImage;

	public Image m_ggPackImage;

	public Image m_bannerImage;

	public Image m_emblemImage;

	public Image m_ribbonImage;

	public Image m_teamAImage;

	public Image m_teamBImage;

	public Image m_selfImage;

	public Image m_allyImage;

	public Image m_enemyImage;

	public _SelectableBtn m_ContributionHitBoxTooltip;

	public _SelectableBtn m_reportPlayerBtn;

	public RectTransform m_statsPageOneContainer;

	public RectTransform m_statsPageTwoContainer;

	public _SelectableBtn[] m_abilityBtns;

	public RectTransform[] m_freeActionLabel;

	public TextMeshProUGUI[] m_cooldownLabel;

	public RectTransform[] m_disableContainer;

	public Image[] m_phaseColor;

	public Image[] m_abilityIcons;

	public Image[] m_modIcons;

	public UITargetingAbilityCatalystPipContainer m_catalystPips;

	public Color m_blastColor;

	public Color m_dashColor;

	public Color m_prepColor;

	public float m_bgAlpha;

	private MatchResultsStatline m_statline;

	private ActorData m_actor;

	private List<Ability> m_abilityReferences;

	private List<AbilityMod> m_abilityModReferences;

	private int m_ISO_toShow;

	private float m_ISO_showing;

	private float ISO_increase_rate;

	private static string[] s_groupColors = new string[]
	{
		"#A0A0A0",
		"#0000C0",
		"#A000A0",
		"#00A0A0",
		"#C00000",
		"#00C000",
		"#A0A000"
	};

	private static int s_nextColorId = 0;

	private static Dictionary<long, int> s_groupToColorMap = new Dictionary<long, int>();

	public ActorData EntryActorData
	{
		get
		{
			return this.m_actor;
		}
	}

	private void ShowAbilityTooltip(Ability ability, AbilityMod abilityMod, UIAbilityTooltip tooltip)
	{
		tooltip.Setup(ability, abilityMod);
	}

	private bool CanPlayerReport()
	{
		int result;
		if (this.m_actor != null)
		{
			if (this.m_actor.GetAccountIdWithSomeConditionB_zq() != -1L)
			{
				if (this.m_actor.GetAccountIdWithSomeConditionA_zq() != ClientGameManager.Get().GetPlayerAccountData().AccountId)
				{
					if (!this.m_actor.GetIsHumanControlled())
					{
						if (!this.m_actor.GetPlayerDetails().m_botsMasqueradeAsHumans)
						{
							result = (this.m_actor.GetPlayerDetails().ReplacedWithBots ? 1 : 0);
							goto IL_B0;
						}
					}
					result = 1;
					IL_B0:
					return result != 0;
				}
			}
		}
		result = 0;
		return result != 0;
	}

	private bool ClickedContribution(UITooltipBase tooltip)
	{
		if (this.CanPlayerReport())
		{
			(tooltip as GameOverBannerMenu).Setup(this.m_actor);
			return true;
		}
		if (this.m_statline != null)
		{
			if (this.m_statline.AccountID != ClientGameManager.Get().GetPlayerAccountData().AccountId)
			{
				if (this.m_statline.AccountID != 0L)
				{
					if (!this.m_statline.IsHumanControlled)
					{
						if (!this.m_statline.HumanReplacedByBot)
						{
							return false;
						}
					}
					(tooltip as GameOverBannerMenu).Setup(this.m_statline);
					return true;
				}
			}
		}
		return false;
	}

	private bool SetupContibutionTooltip(UITooltipBase tooltip)
	{
		UIContributionTooltip uicontributionTooltip = tooltip as UIContributionTooltip;
		if (this.m_actor != null)
		{
			if (uicontributionTooltip != null)
			{
				ActorBehavior actorBehavior = this.m_actor.GetActorBehavior();
				uicontributionTooltip.Setup(StringUtil.TR("Contribution", "GameOver"), actorBehavior.GetContributionBreakdownForUI());
				return true;
			}
		}
		return false;
	}

	public void SetStatPage(UIGameStatsWindow.StatsPage page)
	{
		if (page == UIGameStatsWindow.StatsPage.Mods)
		{
			if (this.m_statsPageOneContainer != null)
			{
				UIManager.SetGameObjectActive(this.m_statsPageOneContainer, false, null);
			}
			if (this.m_statsPageTwoContainer != null)
			{
				UIManager.SetGameObjectActive(this.m_statsPageTwoContainer, true, null);
			}
		}
		else if (page == UIGameStatsWindow.StatsPage.Numbers)
		{
			if (this.m_statsPageOneContainer != null)
			{
				UIManager.SetGameObjectActive(this.m_statsPageOneContainer, true, null);
			}
			if (this.m_statsPageTwoContainer != null)
			{
				UIManager.SetGameObjectActive(this.m_statsPageTwoContainer, false, null);
			}
		}
	}

	public void Setup(MatchResultsStatline statline)
	{
		ActorData actorData = (ActorData)statline.Actor;
		this.m_statline = statline;
		this.m_actor = actorData;
		bool secretButtonClicked = Options_UI.Get().m_secretButtonClicked;
		UIManager.SetGameObjectActive(this.m_DebugStats, secretButtonClicked, null);
		if (this.m_playerLabel != null)
		{
			this.m_playerLabel.text = statline.DisplayName;
		}
		if (this.m_freelancerLabel != null)
		{
			this.m_freelancerLabel.text = statline.Character.GetDisplayName();
		}
		if (this.m_ggPackImage != null)
		{
			Component ggPackImage = this.m_ggPackImage;
			bool doActive;
			if (actorData != null)
			{
				if (actorData.PlayerData != null)
				{
					doActive = (HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.NumberGGPacksUsed(actorData.PlayerData.PlayerHandle) > 0);
					goto IL_118;
				}
			}
			doActive = false;
			IL_118:
			UIManager.SetGameObjectActive(ggPackImage, doActive, null);
		}
		AbilityData abilityData = null;
		List<Ability> list = null;
		if (statline.Character != CharacterType.PunchingDummy)
		{
			if (statline.Character != CharacterType.None)
			{
				GameWideData gameWideData = GameWideData.Get();
				if (gameWideData)
				{
					CharacterResourceLink characterResourceLink = gameWideData.GetCharacterResourceLink(statline.Character);
					this.m_characterImage.sprite = characterResourceLink.GetCharacterSelectIcon();
					abilityData = characterResourceLink.ActorDataPrefab.GetComponent<AbilityData>();
					list = abilityData.GetAbilitiesAsList();
					abilityData.InitAbilitySprites();
				}
			}
		}
		if (this.m_abilityReferences == null)
		{
			this.m_abilityReferences = new List<Ability>();
		}
		if (this.m_abilityModReferences == null)
		{
			this.m_abilityModReferences = new List<AbilityMod>();
		}
		this.m_abilityReferences.Clear();
		this.m_abilityModReferences.Clear();
		LobbyTeamInfo teamInfo = GameManager.Get().TeamInfo;
		UIManager.SetGameObjectActive(this.m_botBanner, false, null);
		UIManager.SetGameObjectActive(this.m_playerBanner, true, null);
		Team team = Team.Spectator;
		ActorData actorData2;
		if (GameFlowData.Get() != null)
		{
			actorData2 = GameFlowData.Get().activeOwnedActorData;
		}
		else
		{
			actorData2 = null;
		}
		ActorData actorData3 = actorData2;
		if (actorData3 != null)
		{
			team = actorData3.GetTeam();
		}
		UIManager.SetGameObjectActive(this.m_reportPlayerBtn, this.CanPlayerReport(), null);
		this.m_reportPlayerBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ReportPlayerBtnClicked);
		this.m_reportPlayerBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.ReportBtnTooltipSetup), null);
		bool doActive2 = true;
		bool flag;
		if (statline.IsHumanControlled)
		{
			flag = true;
		}
		else if (statline.IsBotMasqueradingAsHuman)
		{
			flag = true;
		}
		else if (statline.HumanReplacedByBot)
		{
			flag = true;
		}
		else
		{
			flag = false;
		}
		if (flag)
		{
			UIManager.SetGameObjectActive(this.m_botOverlay, false, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_DebugBotNameText, secretButtonClicked, null);
			UIManager.SetGameObjectActive(this.m_DebugBotDifficultyText, secretButtonClicked, null);
			this.m_DebugBotNameText.text = statline.DisplayName;
			if (!(this.m_actor == null))
			{
				if (team != this.m_actor.GetTeam())
				{
					if (!(SinglePlayerCoordinator.Get() != null))
					{
						UIManager.SetGameObjectActive(this.m_botOverlay, false, null);
						goto IL_3D3;
					}
				}
			}
			UIManager.SetGameObjectActive(this.m_botOverlay, true, null);
			doActive2 = false;
		}
		IL_3D3:
		if (statline.IsPerspective)
		{
			UIManager.SetGameObjectActive(this.m_selfImage, true, null);
			UIManager.SetGameObjectActive(this.m_enemyImage, false, null);
			UIManager.SetGameObjectActive(this.m_allyImage, false, null);
		}
		else
		{
			bool flag2 = false;
			if (actorData3 != null)
			{
				flag2 = (actorData == actorData3);
			}
			UIManager.SetGameObjectActive(this.m_selfImage, flag2, null);
			Component enemyImage = this.m_enemyImage;
			bool doActive3;
			if (!statline.IsAlly)
			{
				doActive3 = !flag2;
			}
			else
			{
				doActive3 = false;
			}
			UIManager.SetGameObjectActive(enemyImage, doActive3, null);
			UIManager.SetGameObjectActive(this.m_allyImage, statline.IsAlly && !flag2, null);
		}
		if (team == Team.Spectator)
		{
			bool flag3 = this.m_actor == null || this.m_actor.GetTeam() == Team.TeamA;
			UIManager.SetGameObjectActive(this.m_teamAImage, flag3, null);
			UIManager.SetGameObjectActive(this.m_DebugTeamAImage, flag3, null);
			UIManager.SetGameObjectActive(this.m_teamBImage, !flag3, null);
			UIManager.SetGameObjectActive(this.m_DebugTeamBImage, !flag3, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_teamAImage, statline.IsAlly, null);
			UIManager.SetGameObjectActive(this.m_DebugTeamAImage, statline.IsAlly, null);
			UIManager.SetGameObjectActive(this.m_teamBImage, !statline.IsAlly, null);
			UIManager.SetGameObjectActive(this.m_DebugTeamBImage, !statline.IsAlly, null);
		}
		if (this.m_playertitle != null)
		{
			this.m_playertitle.text = GameBalanceVars.Get().GetTitle(statline.TitleID, string.Empty, statline.TitleLevel);
		}
		GameBalanceVars.PlayerBanner banner = GameWideData.Get().m_gameBalanceVars.GetBanner(statline.BannerID);
		if (banner != null)
		{
			this.m_bannerImage.sprite = (Sprite)Resources.Load(banner.m_resourceString, typeof(Sprite));
		}
		GameBalanceVars.PlayerBanner banner2 = GameWideData.Get().m_gameBalanceVars.GetBanner(statline.EmblemID);
		if (banner2 != null)
		{
			UIManager.SetGameObjectActive(this.m_emblemImage, true, null);
			this.m_emblemImage.sprite = (Sprite)Resources.Load(banner2.m_resourceString, typeof(Sprite));
		}
		UIManager.SetGameObjectActive(this.m_emblemImage, doActive2, null);
		GameBalanceVars.PlayerRibbon ribbon = GameWideData.Get().m_gameBalanceVars.GetRibbon(statline.RibbonID);
		if (ribbon != null)
		{
			this.m_ribbonImage.sprite = Resources.Load<Sprite>(ribbon.m_resourceString);
			UIManager.SetGameObjectActive(this.m_ribbonImage, ribbon != null, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_ribbonImage, false, null);
		}
		if (this.m_playerLevel != null)
		{
			UIManager.SetGameObjectActive(this.m_playerLevel, false, null);
		}
		if (teamInfo != null)
		{
			if (teamInfo.TeamPlayerInfo != null)
			{
				LobbyPlayerInfo lobbyPlayerInfo = teamInfo.TeamPlayerInfo.FirstOrDefault((LobbyPlayerInfo element) => element.PlayerId == statline.PlayerId);
				if (lobbyPlayerInfo != null)
				{
					if (!statline.IsHumanControlled)
					{
						if (secretButtonClicked)
						{
							this.m_DebugBotDifficultyText.text = string.Format("AI Level: {0}", (int)(lobbyPlayerInfo.Difficulty + 1));
						}
					}
					Dictionary<int, ForbiddenDevKnowledge> forbiddenDevKnowledge = GameManager.Get().ForbiddenDevKnowledge;
					if (!forbiddenDevKnowledge.IsNullOrEmpty<KeyValuePair<int, ForbiddenDevKnowledge>>())
					{
						ForbiddenDevKnowledge forbiddenDevKnowledge2;
						if (forbiddenDevKnowledge.TryGetValue(lobbyPlayerInfo.PlayerId, out forbiddenDevKnowledge2))
						{
							if (this.m_DebugAccountELO != null)
							{
								string arg = (forbiddenDevKnowledge2.UsedMatchmakingElo == forbiddenDevKnowledge2.AccMatchmakingElo) ? "green" : "orange";
								this.m_DebugAccountELO.text = string.Format("<color={0}>{1:F0}</color>", arg, forbiddenDevKnowledge2.UsedMatchmakingElo);
							}
							if (this.m_DebugCharacterELO != null)
							{
								int num = 0;
								if (!UIGameOverPlayerEntry.s_groupToColorMap.TryGetValue(forbiddenDevKnowledge2.GroupIdAtStartOfMatch, out num))
								{
									num = UIGameOverPlayerEntry.s_nextColorId;
									UIGameOverPlayerEntry.s_nextColorId++;
									UIGameOverPlayerEntry.s_groupToColorMap.Add(forbiddenDevKnowledge2.GroupIdAtStartOfMatch, num);
								}
								this.m_DebugCharacterELO.text = string.Format("<color={0}>{1:F0}</color>", UIGameOverPlayerEntry.s_groupColors[num], forbiddenDevKnowledge2.CharMatchmakingElo);
							}
							goto IL_916;
						}
					}
					if (this.m_DebugAccountELO != null)
					{
						this.m_DebugAccountELO.text = "Dev Only";
					}
					if (this.m_DebugCharacterELO != null)
					{
						this.m_DebugCharacterELO.text = "Dev Only";
					}
				}
			}
		}
		IL_916:
		AbilityData abilityData2 = (!(actorData != null)) ? null : actorData.GetAbilityData();
		for (int i = 0; i < statline.AbilityEntries.Length; i++)
		{
			if (i < this.m_abilityBtns.Length)
			{
				if (abilityData != null)
				{
					if (i < list.Count)
					{
						MatchResultsStatline.AbilityEntry abilityEntry = statline.AbilityEntries[i];
						AbilityData.AbilityEntry abilityEntry2 = (!(abilityData2 != null)) ? null : abilityData2.abilityEntries[i];
						Ability theAbility;
						if (abilityEntry2 != null)
						{
							theAbility = abilityEntry2.ability;
						}
						else
						{
							theAbility = list[i];
						}
						if (theAbility != null)
						{
							AbilityMod abilityMod = AbilityModHelper.GetModForAbility(theAbility, abilityEntry.AbilityModId);
							this.m_abilityReferences.Add(theAbility);
							this.m_abilityBtns[i].spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Ability, delegate(UITooltipBase tooltip)
							{
								this.ShowAbilityTooltip(theAbility, abilityMod, (UIAbilityTooltip)tooltip);
								return true;
							}, null);
							this.m_modIcons[i].material = null;
							this.m_abilityIcons[i].sprite = theAbility.sprite;
							UIManager.SetGameObjectActive(this.m_freeActionLabel[i], theAbility.IsFreeAction(), null);
							if (abilityMod != null)
							{
								this.m_abilityModReferences.Add(abilityMod);
								this.m_modIcons[i].sprite = abilityMod.m_iconSprite;
								this.m_modIcons[i].color = Color.white;
							}
							else
							{
								this.m_abilityModReferences.Add(null);
								this.m_modIcons[i].color = Color.clear;
							}
							if (abilityEntry2 != null && actorData != null)
							{
								int num2 = actorData.TechPoints + actorData.ReservedTechPoints;
								bool flag4 = abilityData2.HasQueuedAction((AbilityData.ActionType)i);
								bool flag5;
								if (theAbility.GetModdedCost() > num2)
								{
									flag5 = flag4;
								}
								else
								{
									flag5 = true;
								}
								bool flag6 = flag5;
								string text = string.Empty;
								if (theAbility.GetModdedMaxStocks() <= 0)
								{
									if (abilityEntry2.GetCooldownRemaining() != 0)
									{
										goto IL_BC6;
									}
									if (!flag6)
									{
										goto IL_BC6;
									}
									goto IL_D1F;
									IL_BC6:
									if (abilityEntry2.GetCooldownRemaining() > 0)
									{
										bool flag7;
										if (num2 >= actorData.GetActualMaxTechPoints())
										{
											flag7 = AbilityUtils.AbilityHasTag(theAbility, AbilityTags.IgnoreCooldownIfFullEnergy);
										}
										else
										{
											flag7 = false;
										}
										if (!flag7)
										{
											text = abilityEntry2.GetCooldownRemaining().ToString();
										}
									}
									else if (abilityEntry2.GetCooldownRemaining() == -1)
									{
										text = "~";
									}
								}
								else
								{
									int moddedMaxStocks = theAbility.GetModdedMaxStocks();
									AbilityData component = actorData.GetComponent<AbilityData>();
									AbilityData.ActionType actionTypeOfAbility = component.GetActionTypeOfAbility(theAbility);
									if (actionTypeOfAbility != AbilityData.ActionType.INVALID_ACTION)
									{
										int consumedStocksCount = component.GetConsumedStocksCount(actionTypeOfAbility);
										int stockRefreshCountdown = component.GetStockRefreshCountdown(actionTypeOfAbility);
										int num3 = moddedMaxStocks - consumedStocksCount;
										if (!component.IsAbilityAllowedByUnlockTurns(actionTypeOfAbility))
										{
											text = component.GetTurnsTillUnlock(actionTypeOfAbility).ToString();
										}
										else if (stockRefreshCountdown > 0)
										{
											if (num3 != 0)
											{
												if (!theAbility.RefillAllStockOnRefresh())
												{
													goto IL_D1F;
												}
											}
											text = stockRefreshCountdown.ToString();
										}
									}
								}
								IL_D1F:
								if (theAbility.GetModdedCost() > 0)
								{
									bool flag8 = theAbility.GetModdedCost() <= num2;
									UIManager.SetGameObjectActive(this.m_disableContainer[i], !flag8, null);
									this.m_cooldownLabel[i].text = string.Empty;
								}
								else
								{
									if (this.m_cooldownLabel[i].text != text)
									{
										this.m_cooldownLabel[i].text = text;
									}
									if (this.m_cooldownLabel[i].text.IsNullOrEmpty())
									{
										UIManager.SetGameObjectActive(this.m_disableContainer[i], false, null);
									}
									else
									{
										UIManager.SetGameObjectActive(this.m_disableContainer[i], true, null);
									}
								}
							}
							else
							{
								UIManager.SetGameObjectActive(this.m_disableContainer[i], false, null);
								this.m_cooldownLabel[i].text = string.Empty;
							}
							Color color;
							if (theAbility.GetPhaseString() == StringUtil.TR("Blast", "Global"))
							{
								color = this.m_blastColor;
							}
							else if (theAbility.GetPhaseString() == StringUtil.TR("Dash", "Global"))
							{
								color = this.m_dashColor;
							}
							else if (theAbility.GetPhaseString() == StringUtil.TR("Prep", "Global"))
							{
								color = this.m_prepColor;
							}
							else
							{
								color = this.m_blastColor;
							}
							this.m_phaseColor[i].color = new Color(color.r, color.g, color.b, this.m_bgAlpha);
						}
					}
				}
			}
		}
		this.m_ContributionHitBoxTooltip.spriteController.GetComponent<UITooltipClickObject>().Setup(TooltipType.PlayerBannerMenu, new TooltipPopulateCall(this.ClickedContribution), null);
		this.m_ContributionHitBoxTooltip.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Contribution, new TooltipPopulateCall(this.SetupContibutionTooltip), null);
		if (this.m_KillsLabel != null)
		{
			this.m_KillsLabel.text = statline.TotalPlayerKills.ToString();
		}
		if (this.m_DeathsLabel != null)
		{
			this.m_DeathsLabel.text = statline.TotalDeaths.ToString();
		}
		if (this.m_DamageDealt != null)
		{
			this.m_DamageDealt.text = statline.TotalPlayerDamage.ToString();
		}
		if (this.m_RoleNumber != null)
		{
			this.m_RoleNumber.text = statline.TotalPlayerAssists.ToString();
		}
		if (this.m_HealingNumber != null)
		{
			this.m_HealingNumber.text = (statline.TotalPlayerHealingFromAbility + statline.TotalPlayerAbsorb).ToString();
		}
		if (this.m_DamageReceivedNumber != null)
		{
			this.m_DamageReceivedNumber.text = statline.TotalPlayerDamageReceived.ToString();
		}
		if (this.m_KDANumber != null)
		{
			this.m_KDANumber.text = string.Format("{0}:{1}:{2}", statline.TotalPlayerAssists, statline.TotalDeaths, statline.TotalPlayerKills);
		}
		if (this.m_TimePerTurn != null)
		{
			if (statline.TotalPlayerTurns == 0)
			{
				this.m_TimePerTurn.text = "-";
			}
			else
			{
				this.m_TimePerTurn.text = string.Format("{0:F2}", statline.TotalPlayerLockInTime / (float)statline.TotalPlayerTurns);
			}
		}
		if (secretButtonClicked)
		{
			if (this.m_DebugTotalContribution != null)
			{
				this.m_DebugTotalContribution.text = statline.TotalPlayerContribution.ToString();
			}
		}
		this.m_ISO_toShow = 0;
		this.m_ISO_showing = 0f;
		this.ISO_increase_rate = 1f;
		if (this.m_catalystPips != null)
		{
			UIManager.SetGameObjectActive(this.m_catalystPips.m_PrepPhaseOn, statline.CatalystHasPrepPhase, null);
			UIManager.SetGameObjectActive(this.m_catalystPips.m_DashPhaseOn, statline.CatalystHasDashPhase, null);
			UIManager.SetGameObjectActive(this.m_catalystPips.m_BlastPhaseOn, statline.CatalystHasBlastPhase, null);
			UIUtils.SetAsLastSiblingIfNeeded(this.m_catalystPips.transform);
		}
	}

	private bool ReportBtnTooltipSetup(UITooltipBase tooltip)
	{
		UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
		uititledTooltip.Setup(StringUtil.TR("ReportPlayer", "Global"), string.Format(StringUtil.TR("ReportPlayerName", "Global"), this.m_actor.DisplayName), string.Empty);
		return true;
	}

	public void ReportPlayerBtnClicked(BaseEventData data)
	{
		PlayerDetails playerDetails = GameFlow.Get().playerDetails[this.m_actor.PlayerData.GetPlayer()];
		UILandingPageFullScreenMenus.Get().SetReportContainerVisible(true, playerDetails.m_handle, playerDetails.m_accountId, playerDetails.m_botsMasqueradeAsHumans);
	}

	private void Update()
	{
		if (this.m_ISO_showing < (float)this.m_ISO_toShow)
		{
			this.m_ISO_showing += Time.deltaTime * this.ISO_increase_rate;
		}
		if (this.m_ISO_showing > (float)this.m_ISO_toShow)
		{
			this.m_ISO_showing = (float)this.m_ISO_toShow;
		}
	}

	public static void PreSetupInitialization()
	{
		UIGameOverPlayerEntry.s_groupToColorMap = new Dictionary<long, int>();
		UIGameOverPlayerEntry.s_groupToColorMap.Add(0L, 0);
		UIGameOverPlayerEntry.s_nextColorId = 1;
	}
}
