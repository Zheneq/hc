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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverPlayerEntry.CanPlayerReport()).MethodHandle;
			}
			if (this.m_actor.GetAccountIdWithSomeConditionB_zq() != -1L)
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
				if (this.m_actor.GetAccountIdWithSomeConditionA_zq() != ClientGameManager.Get().GetPlayerAccountData().AccountId)
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
					if (!this.m_actor.GetIsHumanControlled())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverPlayerEntry.ClickedContribution(UITooltipBase)).MethodHandle;
			}
			(tooltip as GameOverBannerMenu).Setup(this.m_actor);
			return true;
		}
		if (this.m_statline != null)
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
			if (this.m_statline.AccountID != ClientGameManager.Get().GetPlayerAccountData().AccountId)
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
				if (this.m_statline.AccountID != 0L)
				{
					if (!this.m_statline.IsHumanControlled)
					{
						if (!this.m_statline.HumanReplacedByBot)
						{
							return false;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverPlayerEntry.SetupContibutionTooltip(UITooltipBase)).MethodHandle;
			}
			if (uicontributionTooltip != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverPlayerEntry.SetStatPage(UIGameStatsWindow.StatsPage)).MethodHandle;
			}
			if (this.m_statsPageOneContainer != null)
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
				UIManager.SetGameObjectActive(this.m_statsPageOneContainer, false, null);
			}
			if (this.m_statsPageTwoContainer != null)
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
				UIManager.SetGameObjectActive(this.m_statsPageTwoContainer, true, null);
			}
		}
		else if (page == UIGameStatsWindow.StatsPage.Numbers)
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
			if (this.m_statsPageOneContainer != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverPlayerEntry.Setup(MatchResultsStatline)).MethodHandle;
				}
				if (actorData.PlayerData != null)
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (statline.Character != CharacterType.None)
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
				GameWideData gameWideData = GameWideData.Get();
				if (gameWideData)
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_abilityReferences = new List<Ability>();
		}
		if (this.m_abilityModReferences == null)
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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			flag = true;
		}
		else if (statline.IsBotMasqueradingAsHuman)
		{
			flag = true;
		}
		else if (statline.HumanReplacedByBot)
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
		else
		{
			flag = false;
		}
		if (flag)
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
			UIManager.SetGameObjectActive(this.m_botOverlay, false, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_DebugBotNameText, secretButtonClicked, null);
			UIManager.SetGameObjectActive(this.m_DebugBotDifficultyText, secretButtonClicked, null);
			this.m_DebugBotNameText.text = statline.DisplayName;
			if (!(this.m_actor == null))
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
				if (team != this.m_actor.GetTeam())
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
					if (!(SinglePlayerCoordinator.Get() != null))
					{
						UIManager.SetGameObjectActive(this.m_botOverlay, false, null);
						goto IL_3D3;
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
				}
			}
			UIManager.SetGameObjectActive(this.m_botOverlay, true, null);
			doActive2 = false;
		}
		IL_3D3:
		if (statline.IsPerspective)
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
			UIManager.SetGameObjectActive(this.m_selfImage, true, null);
			UIManager.SetGameObjectActive(this.m_enemyImage, false, null);
			UIManager.SetGameObjectActive(this.m_allyImage, false, null);
		}
		else
		{
			bool flag2 = false;
			if (actorData3 != null)
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
				flag2 = (actorData == actorData3);
			}
			UIManager.SetGameObjectActive(this.m_selfImage, flag2, null);
			Component enemyImage = this.m_enemyImage;
			bool doActive3;
			if (!statline.IsAlly)
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_bannerImage.sprite = (Sprite)Resources.Load(banner.m_resourceString, typeof(Sprite));
		}
		GameBalanceVars.PlayerBanner banner2 = GameWideData.Get().m_gameBalanceVars.GetBanner(statline.EmblemID);
		if (banner2 != null)
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
			UIManager.SetGameObjectActive(this.m_emblemImage, true, null);
			this.m_emblemImage.sprite = (Sprite)Resources.Load(banner2.m_resourceString, typeof(Sprite));
		}
		UIManager.SetGameObjectActive(this.m_emblemImage, doActive2, null);
		GameBalanceVars.PlayerRibbon ribbon = GameWideData.Get().m_gameBalanceVars.GetRibbon(statline.RibbonID);
		if (ribbon != null)
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
			this.m_ribbonImage.sprite = Resources.Load<Sprite>(ribbon.m_resourceString);
			UIManager.SetGameObjectActive(this.m_ribbonImage, ribbon != null, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_ribbonImage, false, null);
		}
		if (this.m_playerLevel != null)
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
			UIManager.SetGameObjectActive(this.m_playerLevel, false, null);
		}
		if (teamInfo != null)
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
			if (teamInfo.TeamPlayerInfo != null)
			{
				LobbyPlayerInfo lobbyPlayerInfo = teamInfo.TeamPlayerInfo.FirstOrDefault((LobbyPlayerInfo element) => element.PlayerId == statline.PlayerId);
				if (lobbyPlayerInfo != null)
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
					if (!statline.IsHumanControlled)
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
						if (secretButtonClicked)
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
							this.m_DebugBotDifficultyText.text = string.Format("AI Level: {0}", (int)(lobbyPlayerInfo.Difficulty + 1));
						}
					}
					Dictionary<int, ForbiddenDevKnowledge> forbiddenDevKnowledge = GameManager.Get().ForbiddenDevKnowledge;
					if (!forbiddenDevKnowledge.IsNullOrEmpty<KeyValuePair<int, ForbiddenDevKnowledge>>())
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
						ForbiddenDevKnowledge forbiddenDevKnowledge2;
						if (forbiddenDevKnowledge.TryGetValue(lobbyPlayerInfo.PlayerId, out forbiddenDevKnowledge2))
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
							if (this.m_DebugAccountELO != null)
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
								string arg = (forbiddenDevKnowledge2.UsedMatchmakingElo == forbiddenDevKnowledge2.AccMatchmakingElo) ? "green" : "orange";
								this.m_DebugAccountELO.text = string.Format("<color={0}>{1:F0}</color>", arg, forbiddenDevKnowledge2.UsedMatchmakingElo);
							}
							if (this.m_DebugCharacterELO != null)
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
								int num = 0;
								if (!UIGameOverPlayerEntry.s_groupToColorMap.TryGetValue(forbiddenDevKnowledge2.GroupIdAtStartOfMatch, out num))
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
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_DebugAccountELO.text = "Dev Only";
					}
					if (this.m_DebugCharacterELO != null)
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (abilityData != null)
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
					if (i < list.Count)
					{
						UIGameOverPlayerEntry.<Setup>c__AnonStorey1 <Setup>c__AnonStorey2 = new UIGameOverPlayerEntry.<Setup>c__AnonStorey1();
						MatchResultsStatline.AbilityEntry abilityEntry = statline.AbilityEntries[i];
						AbilityData.AbilityEntry abilityEntry2 = (!(abilityData2 != null)) ? null : abilityData2.abilityEntries[i];
						UIGameOverPlayerEntry.<Setup>c__AnonStorey1 <Setup>c__AnonStorey3 = <Setup>c__AnonStorey2;
						Ability theAbility;
						if (abilityEntry2 != null)
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
							theAbility = abilityEntry2.ability;
						}
						else
						{
							theAbility = list[i];
						}
						<Setup>c__AnonStorey3.theAbility = theAbility;
						if (<Setup>c__AnonStorey2.theAbility != null)
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
							AbilityMod abilityMod = AbilityModHelper.GetModForAbility(<Setup>c__AnonStorey2.theAbility, abilityEntry.AbilityModId);
							this.m_abilityReferences.Add(<Setup>c__AnonStorey2.theAbility);
							this.m_abilityBtns[i].spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Ability, delegate(UITooltipBase tooltip)
							{
								this.ShowAbilityTooltip(<Setup>c__AnonStorey2.theAbility, abilityMod, (UIAbilityTooltip)tooltip);
								return true;
							}, null);
							this.m_modIcons[i].material = null;
							this.m_abilityIcons[i].sprite = <Setup>c__AnonStorey2.theAbility.sprite;
							UIManager.SetGameObjectActive(this.m_freeActionLabel[i], <Setup>c__AnonStorey2.theAbility.IsFreeAction(), null);
							if (abilityMod != null)
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
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								int num2 = actorData.TechPoints + actorData.ReservedTechPoints;
								bool flag4 = abilityData2.HasQueuedAction((AbilityData.ActionType)i);
								bool flag5;
								if (<Setup>c__AnonStorey2.theAbility.GetModdedCost() > num2)
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
									flag5 = flag4;
								}
								else
								{
									flag5 = true;
								}
								bool flag6 = flag5;
								string text = string.Empty;
								if (<Setup>c__AnonStorey2.theAbility.GetModdedMaxStocks() <= 0)
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
									if (abilityEntry2.GetCooldownRemaining() != 0)
									{
										goto IL_BC6;
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
											for (;;)
											{
												switch (4)
												{
												case 0:
													continue;
												}
												break;
											}
											flag7 = AbilityUtils.AbilityHasTag(<Setup>c__AnonStorey2.theAbility, AbilityTags.IgnoreCooldownIfFullEnergy);
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
										for (;;)
										{
											switch (2)
											{
											case 0:
												continue;
											}
											break;
										}
										text = "~";
									}
								}
								else
								{
									int moddedMaxStocks = <Setup>c__AnonStorey2.theAbility.GetModdedMaxStocks();
									AbilityData component = actorData.GetComponent<AbilityData>();
									AbilityData.ActionType actionTypeOfAbility = component.GetActionTypeOfAbility(<Setup>c__AnonStorey2.theAbility);
									if (actionTypeOfAbility != AbilityData.ActionType.INVALID_ACTION)
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
										int consumedStocksCount = component.GetConsumedStocksCount(actionTypeOfAbility);
										int stockRefreshCountdown = component.GetStockRefreshCountdown(actionTypeOfAbility);
										int num3 = moddedMaxStocks - consumedStocksCount;
										if (!component.IsAbilityAllowedByUnlockTurns(actionTypeOfAbility))
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
											text = component.GetTurnsTillUnlock(actionTypeOfAbility).ToString();
										}
										else if (stockRefreshCountdown > 0)
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
											if (num3 != 0)
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
												if (!<Setup>c__AnonStorey2.theAbility.RefillAllStockOnRefresh())
												{
													goto IL_D1F;
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
											}
											text = stockRefreshCountdown.ToString();
										}
									}
								}
								IL_D1F:
								if (<Setup>c__AnonStorey2.theAbility.GetModdedCost() > 0)
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
									bool flag8 = <Setup>c__AnonStorey2.theAbility.GetModdedCost() <= num2;
									UIManager.SetGameObjectActive(this.m_disableContainer[i], !flag8, null);
									this.m_cooldownLabel[i].text = string.Empty;
								}
								else
								{
									if (this.m_cooldownLabel[i].text != text)
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
										this.m_cooldownLabel[i].text = text;
									}
									if (this.m_cooldownLabel[i].text.IsNullOrEmpty())
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
							if (<Setup>c__AnonStorey2.theAbility.GetPhaseString() == StringUtil.TR("Blast", "Global"))
							{
								color = this.m_blastColor;
							}
							else if (<Setup>c__AnonStorey2.theAbility.GetPhaseString() == StringUtil.TR("Dash", "Global"))
							{
								color = this.m_dashColor;
							}
							else if (<Setup>c__AnonStorey2.theAbility.GetPhaseString() == StringUtil.TR("Prep", "Global"))
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
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		this.m_ContributionHitBoxTooltip.spriteController.GetComponent<UITooltipClickObject>().Setup(TooltipType.PlayerBannerMenu, new TooltipPopulateCall(this.ClickedContribution), null);
		this.m_ContributionHitBoxTooltip.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Contribution, new TooltipPopulateCall(this.SetupContibutionTooltip), null);
		if (this.m_KillsLabel != null)
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
			this.m_KillsLabel.text = statline.TotalPlayerKills.ToString();
		}
		if (this.m_DeathsLabel != null)
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
			this.m_DeathsLabel.text = statline.TotalDeaths.ToString();
		}
		if (this.m_DamageDealt != null)
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
			this.m_DamageDealt.text = statline.TotalPlayerDamage.ToString();
		}
		if (this.m_RoleNumber != null)
		{
			this.m_RoleNumber.text = statline.TotalPlayerAssists.ToString();
		}
		if (this.m_HealingNumber != null)
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
			this.m_HealingNumber.text = (statline.TotalPlayerHealingFromAbility + statline.TotalPlayerAbsorb).ToString();
		}
		if (this.m_DamageReceivedNumber != null)
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
			this.m_DamageReceivedNumber.text = statline.TotalPlayerDamageReceived.ToString();
		}
		if (this.m_KDANumber != null)
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
			this.m_KDANumber.text = string.Format("{0}:{1}:{2}", statline.TotalPlayerAssists, statline.TotalDeaths, statline.TotalPlayerKills);
		}
		if (this.m_TimePerTurn != null)
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
			if (statline.TotalPlayerTurns == 0)
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
				this.m_TimePerTurn.text = "-";
			}
			else
			{
				this.m_TimePerTurn.text = string.Format("{0:F2}", statline.TotalPlayerLockInTime / (float)statline.TotalPlayerTurns);
			}
		}
		if (secretButtonClicked)
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
			if (this.m_DebugTotalContribution != null)
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
