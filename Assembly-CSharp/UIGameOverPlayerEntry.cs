using LobbyGameClientMessages;
using System.Collections.Generic;
using System.Linq;
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

	private static string[] s_groupColors = new string[7]
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

	public ActorData EntryActorData => m_actor;

	private void ShowAbilityTooltip(Ability ability, AbilityMod abilityMod, UIAbilityTooltip tooltip)
	{
		tooltip.Setup(ability, abilityMod);
	}

	private bool CanPlayerReport()
	{
		int result;
		if (m_actor != null)
		{
			if (m_actor.GetOriginalAccountId() != -1)
			{
				if (m_actor.GetAccountId() != ClientGameManager.Get().GetPlayerAccountData().AccountId)
				{
					if (!m_actor.IsHumanControlled())
					{
						if (!m_actor.GetPlayerDetails().m_botsMasqueradeAsHumans)
						{
							result = (m_actor.GetPlayerDetails().ReplacedWithBots ? 1 : 0);
							goto IL_00b3;
						}
					}
					result = 1;
					goto IL_00b3;
				}
			}
		}
		result = 0;
		goto IL_00b3;
		IL_00b3:
		return (byte)result != 0;
	}

	private bool ClickedContribution(UITooltipBase tooltip)
	{
		if (CanPlayerReport())
		{
			while (true)
			{
				(tooltip as GameOverBannerMenu).Setup(m_actor);
				return true;
			}
		}
		if (m_statline != null)
		{
			if (m_statline.AccountID != ClientGameManager.Get().GetPlayerAccountData().AccountId)
			{
				if (m_statline.AccountID != 0)
				{
					if (!m_statline.IsHumanControlled)
					{
						if (!m_statline.HumanReplacedByBot)
						{
							goto IL_00b3;
						}
					}
					(tooltip as GameOverBannerMenu).Setup(m_statline);
					return true;
				}
			}
		}
		goto IL_00b3;
		IL_00b3:
		return false;
	}

	private bool SetupContibutionTooltip(UITooltipBase tooltip)
	{
		UIContributionTooltip uIContributionTooltip = tooltip as UIContributionTooltip;
		if (m_actor != null)
		{
			if (uIContributionTooltip != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						ActorBehavior actorBehavior = m_actor.GetActorBehavior();
						uIContributionTooltip.Setup(StringUtil.TR("Contribution", "GameOver"), actorBehavior.GetContributionBreakdownForUI());
						return true;
					}
					}
				}
			}
		}
		return false;
	}

	public void SetStatPage(UIGameStatsWindow.StatsPage page)
	{
		if (page == UIGameStatsWindow.StatsPage.Mods)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (m_statsPageOneContainer != null)
					{
						UIManager.SetGameObjectActive(m_statsPageOneContainer, false);
					}
					if (m_statsPageTwoContainer != null)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								UIManager.SetGameObjectActive(m_statsPageTwoContainer, true);
								return;
							}
						}
					}
					return;
				}
			}
		}
		if (page != 0)
		{
			return;
		}
		while (true)
		{
			if (m_statsPageOneContainer != null)
			{
				UIManager.SetGameObjectActive(m_statsPageOneContainer, true);
			}
			if (m_statsPageTwoContainer != null)
			{
				UIManager.SetGameObjectActive(m_statsPageTwoContainer, false);
			}
			return;
		}
	}

	public void Setup(MatchResultsStatline statline)
	{
		ActorData actorData = (ActorData)statline.Actor;
		m_statline = statline;
		m_actor = actorData;
		bool secretButtonClicked = Options_UI.Get().m_secretButtonClicked;
		UIManager.SetGameObjectActive(m_DebugStats, secretButtonClicked);
		if (m_playerLabel != null)
		{
			m_playerLabel.text = statline.DisplayName;
		}
		if (m_freelancerLabel != null)
		{
			m_freelancerLabel.text = statline.Character.GetDisplayName();
		}
		Image ggPackImage;
		int doActive;
		if (m_ggPackImage != null)
		{
			ggPackImage = m_ggPackImage;
			if (actorData != null)
			{
				if (actorData.PlayerData != null)
				{
					doActive = ((HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.NumberGGPacksUsed(actorData.PlayerData.PlayerHandle) > 0) ? 1 : 0);
					goto IL_0118;
				}
			}
			doActive = 0;
			goto IL_0118;
		}
		goto IL_011e;
		IL_0916:
		AbilityData abilityData = (!(actorData != null)) ? null : actorData.GetAbilityData();
		AbilityData abilityData2;
		List<Ability> list;
		for (int i = 0; i < statline.AbilityEntries.Length; i++)
		{
			if (i >= m_abilityBtns.Length)
			{
				continue;
			}
			if (!(abilityData2 != null))
			{
				continue;
			}
			if (i >= list.Count)
			{
				continue;
			}
			MatchResultsStatline.AbilityEntry abilityEntry = statline.AbilityEntries[i];
			AbilityData.AbilityEntry abilityEntry2 = (!(abilityData != null)) ? null : abilityData.abilityEntries[i];
			Ability ability;
			if (abilityEntry2 != null)
			{
				ability = abilityEntry2.ability;
			}
			else
			{
				ability = list[i];
			}
			Ability theAbility = ability;
			if (!(theAbility != null))
			{
				continue;
			}
			AbilityMod abilityMod = AbilityModHelper.GetModForAbility(theAbility, abilityEntry.AbilityModId);
			m_abilityReferences.Add(theAbility);
			m_abilityBtns[i].spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Ability, delegate(UITooltipBase tooltip)
			{
				ShowAbilityTooltip(theAbility, abilityMod, (UIAbilityTooltip)tooltip);
				return true;
			});
			m_modIcons[i].material = null;
			m_abilityIcons[i].sprite = theAbility.sprite;
			UIManager.SetGameObjectActive(m_freeActionLabel[i], theAbility.IsFreeAction());
			if (abilityMod != null)
			{
				m_abilityModReferences.Add(abilityMod);
				m_modIcons[i].sprite = abilityMod.m_iconSprite;
				m_modIcons[i].color = Color.white;
			}
			else
			{
				m_abilityModReferences.Add(null);
				m_modIcons[i].color = Color.clear;
			}
			int num;
			string text;
			if (abilityEntry2 != null && actorData != null)
			{
				num = actorData.TechPoints + actorData.ReservedTechPoints;
				bool flag = abilityData.HasQueuedAction((AbilityData.ActionType)i);
				int num2;
				if (theAbility.GetModdedCost() > num)
				{
					num2 = (flag ? 1 : 0);
				}
				else
				{
					num2 = 1;
				}
				bool flag2 = (byte)num2 != 0;
				text = string.Empty;
				if (theAbility.GetModdedMaxStocks() <= 0)
				{
					if (abilityEntry2.GetCooldownRemaining() == 0)
					{
						if (flag2)
						{
							goto IL_0d1f;
						}
					}
					if (abilityEntry2.GetCooldownRemaining() > 0)
					{
						int num3;
						if (num >= actorData.GetMaxTechPoints())
						{
							num3 = (AbilityUtils.AbilityHasTag(theAbility, AbilityTags.IgnoreCooldownIfFullEnergy) ? 1 : 0);
						}
						else
						{
							num3 = 0;
						}
						if (num3 == 0)
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
						int num4 = moddedMaxStocks - consumedStocksCount;
						if (!component.IsAbilityAllowedByUnlockTurns(actionTypeOfAbility))
						{
							text = component.GetTurnsTillUnlock(actionTypeOfAbility).ToString();
						}
						else if (stockRefreshCountdown > 0)
						{
							if (num4 != 0)
							{
								if (!theAbility.RefillAllStockOnRefresh())
								{
									goto IL_0d1f;
								}
							}
							text = stockRefreshCountdown.ToString();
						}
					}
				}
				goto IL_0d1f;
			}
			UIManager.SetGameObjectActive(m_disableContainer[i], false);
			m_cooldownLabel[i].text = string.Empty;
			goto IL_0e1a;
			IL_0d1f:
			if (theAbility.GetModdedCost() > 0)
			{
				bool flag3 = theAbility.GetModdedCost() <= num;
				UIManager.SetGameObjectActive(m_disableContainer[i], !flag3);
				m_cooldownLabel[i].text = string.Empty;
			}
			else
			{
				if (m_cooldownLabel[i].text != text)
				{
					m_cooldownLabel[i].text = text;
				}
				if (m_cooldownLabel[i].text.IsNullOrEmpty())
				{
					UIManager.SetGameObjectActive(m_disableContainer[i], false);
				}
				else
				{
					UIManager.SetGameObjectActive(m_disableContainer[i], true);
				}
			}
			goto IL_0e1a;
			IL_0e1a:
			Color color;
			if (theAbility.GetPhaseString() == StringUtil.TR("Blast", "Global"))
			{
				color = m_blastColor;
			}
			else if (theAbility.GetPhaseString() == StringUtil.TR("Dash", "Global"))
			{
				color = m_dashColor;
			}
			else if (theAbility.GetPhaseString() == StringUtil.TR("Prep", "Global"))
			{
				color = m_prepColor;
			}
			else
			{
				color = m_blastColor;
			}
			m_phaseColor[i].color = new Color(color.r, color.g, color.b, m_bgAlpha);
		}
		while (true)
		{
			m_ContributionHitBoxTooltip.spriteController.GetComponent<UITooltipClickObject>().Setup(TooltipType.PlayerBannerMenu, ClickedContribution);
			m_ContributionHitBoxTooltip.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Contribution, SetupContibutionTooltip);
			if (m_KillsLabel != null)
			{
				m_KillsLabel.text = statline.TotalPlayerKills.ToString();
			}
			if (m_DeathsLabel != null)
			{
				m_DeathsLabel.text = statline.TotalDeaths.ToString();
			}
			if (m_DamageDealt != null)
			{
				m_DamageDealt.text = statline.TotalPlayerDamage.ToString();
			}
			if (m_RoleNumber != null)
			{
				m_RoleNumber.text = statline.TotalPlayerAssists.ToString();
			}
			if (m_HealingNumber != null)
			{
				m_HealingNumber.text = (statline.TotalPlayerHealingFromAbility + statline.TotalPlayerAbsorb).ToString();
			}
			if (m_DamageReceivedNumber != null)
			{
				m_DamageReceivedNumber.text = statline.TotalPlayerDamageReceived.ToString();
			}
			if (m_KDANumber != null)
			{
				m_KDANumber.text = $"{statline.TotalPlayerAssists}:{statline.TotalDeaths}:{statline.TotalPlayerKills}";
			}
			if (m_TimePerTurn != null)
			{
				if (statline.TotalPlayerTurns == 0)
				{
					m_TimePerTurn.text = "-";
				}
				else
				{
					m_TimePerTurn.text = $"{statline.TotalPlayerLockInTime / (float)statline.TotalPlayerTurns:F2}";
				}
			}
			if (secretButtonClicked)
			{
				if (m_DebugTotalContribution != null)
				{
					m_DebugTotalContribution.text = statline.TotalPlayerContribution.ToString();
				}
			}
			m_ISO_toShow = 0;
			m_ISO_showing = 0f;
			ISO_increase_rate = 1f;
			if (m_catalystPips != null)
			{
				UIManager.SetGameObjectActive(m_catalystPips.m_PrepPhaseOn, statline.CatalystHasPrepPhase);
				UIManager.SetGameObjectActive(m_catalystPips.m_DashPhaseOn, statline.CatalystHasDashPhase);
				UIManager.SetGameObjectActive(m_catalystPips.m_BlastPhaseOn, statline.CatalystHasBlastPhase);
				UIUtils.SetAsLastSiblingIfNeeded(m_catalystPips.transform);
			}
			return;
		}
		IL_0118:
		UIManager.SetGameObjectActive(ggPackImage, (byte)doActive != 0);
		goto IL_011e;
		IL_011e:
		abilityData2 = null;
		list = null;
		if (statline.Character != CharacterType.PunchingDummy)
		{
			if (statline.Character != 0)
			{
				GameWideData gameWideData = GameWideData.Get();
				if ((bool)gameWideData)
				{
					CharacterResourceLink characterResourceLink = gameWideData.GetCharacterResourceLink(statline.Character);
					m_characterImage.sprite = characterResourceLink.GetCharacterSelectIcon();
					abilityData2 = characterResourceLink.ActorDataPrefab.GetComponent<AbilityData>();
					list = abilityData2.GetAbilitiesAsList();
					abilityData2.InitAbilitySprites();
				}
			}
		}
		if (m_abilityReferences == null)
		{
			m_abilityReferences = new List<Ability>();
		}
		if (m_abilityModReferences == null)
		{
			m_abilityModReferences = new List<AbilityMod>();
		}
		m_abilityReferences.Clear();
		m_abilityModReferences.Clear();
		LobbyTeamInfo teamInfo = GameManager.Get().TeamInfo;
		UIManager.SetGameObjectActive(m_botBanner, false);
		UIManager.SetGameObjectActive(m_playerBanner, true);
		Team team = Team.Spectator;
		object obj;
		if (GameFlowData.Get() != null)
		{
			obj = GameFlowData.Get().activeOwnedActorData;
		}
		else
		{
			obj = null;
		}
		ActorData actorData2 = (ActorData)obj;
		if (actorData2 != null)
		{
			team = actorData2.GetTeam();
		}
		UIManager.SetGameObjectActive(m_reportPlayerBtn, CanPlayerReport());
		m_reportPlayerBtn.spriteController.callback = ReportPlayerBtnClicked;
		m_reportPlayerBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, ReportBtnTooltipSetup);
		bool doActive2 = true;
		bool flag4;
		if (statline.IsHumanControlled)
		{
			flag4 = true;
		}
		else if (statline.IsBotMasqueradingAsHuman)
		{
			flag4 = true;
		}
		else if (statline.HumanReplacedByBot)
		{
			flag4 = true;
		}
		else
		{
			flag4 = false;
		}
		if (flag4)
		{
			UIManager.SetGameObjectActive(m_botOverlay, false);
		}
		else
		{
			UIManager.SetGameObjectActive(m_DebugBotNameText, secretButtonClicked);
			UIManager.SetGameObjectActive(m_DebugBotDifficultyText, secretButtonClicked);
			m_DebugBotNameText.text = statline.DisplayName;
			if (!(m_actor == null))
			{
				if (team != m_actor.GetTeam())
				{
					if (!(SinglePlayerCoordinator.Get() != null))
					{
						UIManager.SetGameObjectActive(m_botOverlay, false);
						goto IL_03d3;
					}
				}
			}
			UIManager.SetGameObjectActive(m_botOverlay, true);
			doActive2 = false;
		}
		goto IL_03d3;
		IL_03d3:
		if (statline.IsPerspective)
		{
			UIManager.SetGameObjectActive(m_selfImage, true);
			UIManager.SetGameObjectActive(m_enemyImage, false);
			UIManager.SetGameObjectActive(m_allyImage, false);
		}
		else
		{
			bool flag5 = false;
			if (actorData2 != null)
			{
				flag5 = (actorData == actorData2);
			}
			UIManager.SetGameObjectActive(m_selfImage, flag5);
			Image enemyImage = m_enemyImage;
			int doActive3;
			if (!statline.IsAlly)
			{
				doActive3 = ((!flag5) ? 1 : 0);
			}
			else
			{
				doActive3 = 0;
			}
			UIManager.SetGameObjectActive(enemyImage, (byte)doActive3 != 0);
			UIManager.SetGameObjectActive(m_allyImage, statline.IsAlly && !flag5);
		}
		if (team == Team.Spectator)
		{
			bool flag6 = m_actor == null || m_actor.GetTeam() == Team.TeamA;
			UIManager.SetGameObjectActive(m_teamAImage, flag6);
			UIManager.SetGameObjectActive(m_DebugTeamAImage, flag6);
			UIManager.SetGameObjectActive(m_teamBImage, !flag6);
			UIManager.SetGameObjectActive(m_DebugTeamBImage, !flag6);
		}
		else
		{
			UIManager.SetGameObjectActive(m_teamAImage, statline.IsAlly);
			UIManager.SetGameObjectActive(m_DebugTeamAImage, statline.IsAlly);
			UIManager.SetGameObjectActive(m_teamBImage, !statline.IsAlly);
			UIManager.SetGameObjectActive(m_DebugTeamBImage, !statline.IsAlly);
		}
		if (m_playertitle != null)
		{
#if !VANILLA && !SERVER
            // Custom titles
            m_playertitle.text = GameBalanceVars.Get().GetTitle(statline.TitleID, statline.DisplayName, string.Empty, statline.TitleLevel);
#else
			m_playertitle.text = GameBalanceVars.Get().GetTitle(statline.TitleID, string.Empty, statline.TitleLevel);
#endif
		}
		GameBalanceVars.PlayerBanner banner = GameWideData.Get().m_gameBalanceVars.GetBanner(statline.BannerID);
		if (banner != null)
		{
			m_bannerImage.sprite = (Sprite)Resources.Load(banner.m_resourceString, typeof(Sprite));
		}
		GameBalanceVars.PlayerBanner banner2 = GameWideData.Get().m_gameBalanceVars.GetBanner(statline.EmblemID);
		if (banner2 != null)
		{
			UIManager.SetGameObjectActive(m_emblemImage, true);
			m_emblemImage.sprite = (Sprite)Resources.Load(banner2.m_resourceString, typeof(Sprite));
		}
		UIManager.SetGameObjectActive(m_emblemImage, doActive2);
		GameBalanceVars.PlayerRibbon ribbon = GameWideData.Get().m_gameBalanceVars.GetRibbon(statline.RibbonID);
		if (ribbon != null)
		{
			m_ribbonImage.sprite = Resources.Load<Sprite>(ribbon.m_resourceString);
			UIManager.SetGameObjectActive(m_ribbonImage, ribbon != null);
		}
		else
		{
			UIManager.SetGameObjectActive(m_ribbonImage, false);
		}
		if (m_playerLevel != null)
		{
			UIManager.SetGameObjectActive(m_playerLevel, false);
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
							m_DebugBotDifficultyText.text = $"AI Level: {(int)(lobbyPlayerInfo.Difficulty + 1)}";
						}
					}
					Dictionary<int, ForbiddenDevKnowledge> forbiddenDevKnowledge = GameManager.Get().ForbiddenDevKnowledge;
					if (!forbiddenDevKnowledge.IsNullOrEmpty())
					{
						if (forbiddenDevKnowledge.TryGetValue(lobbyPlayerInfo.PlayerId, out ForbiddenDevKnowledge value))
						{
							if (m_DebugAccountELO != null)
							{
								string arg = (value.UsedMatchmakingElo == value.AccMatchmakingElo) ? "green" : "orange";
								m_DebugAccountELO.text = $"<color={arg}>{value.UsedMatchmakingElo:F0}</color>";
							}
							if (m_DebugCharacterELO != null)
							{
								int value2 = 0;
								if (!s_groupToColorMap.TryGetValue(value.GroupIdAtStartOfMatch, out value2))
								{
									value2 = s_nextColorId;
									s_nextColorId++;
									s_groupToColorMap.Add(value.GroupIdAtStartOfMatch, value2);
								}
								m_DebugCharacterELO.text = $"<color={s_groupColors[value2]}>{value.CharMatchmakingElo:F0}</color>";
							}
							goto IL_0916;
						}
					}
					if (m_DebugAccountELO != null)
					{
						m_DebugAccountELO.text = "Dev Only";
					}
					if (m_DebugCharacterELO != null)
					{
						m_DebugCharacterELO.text = "Dev Only";
					}
				}
			}
		}
		goto IL_0916;
	}

	private bool ReportBtnTooltipSetup(UITooltipBase tooltip)
	{
		UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
		uITitledTooltip.Setup(StringUtil.TR("ReportPlayer", "Global"), string.Format(StringUtil.TR("ReportPlayerName", "Global"), m_actor.DisplayName), string.Empty);
		return true;
	}

	public void ReportPlayerBtnClicked(BaseEventData data)
	{
		PlayerDetails playerDetails = GameFlow.Get().playerDetails[m_actor.PlayerData.GetPlayer()];
		UILandingPageFullScreenMenus.Get().SetReportContainerVisible(true, playerDetails.m_handle, playerDetails.m_accountId, playerDetails.m_botsMasqueradeAsHumans);
	}

	private void Update()
	{
		if (m_ISO_showing < (float)m_ISO_toShow)
		{
			m_ISO_showing += Time.deltaTime * ISO_increase_rate;
		}
		if (m_ISO_showing > (float)m_ISO_toShow)
		{
			m_ISO_showing = m_ISO_toShow;
		}
	}

	public static void PreSetupInitialization()
	{
		s_groupToColorMap = new Dictionary<long, int>();
		s_groupToColorMap.Add(0L, 0);
		s_nextColorId = 1;
	}
}
