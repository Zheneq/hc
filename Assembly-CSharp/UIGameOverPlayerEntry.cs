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

    private static int s_nextColorId;

    private static Dictionary<long, int> s_groupToColorMap = new Dictionary<long, int>();

    public ActorData EntryActorData => m_actor;

    private void ShowAbilityTooltip(Ability ability, AbilityMod abilityMod, UIAbilityTooltip tooltip)
    {
        tooltip.Setup(ability, abilityMod);
    }

    private bool CanPlayerReport()
    {
        return m_actor != null
               && m_actor.GetOriginalAccountId() != -1
               && m_actor.GetAccountId() != ClientGameManager.Get().GetPlayerAccountData().AccountId
               && (m_actor.IsHumanControlled()
                   || m_actor.GetPlayerDetails().m_botsMasqueradeAsHumans
                   || m_actor.GetPlayerDetails().ReplacedWithBots);
    }

    private bool ClickedContribution(UITooltipBase tooltip)
    {
        if (CanPlayerReport())
        {
            (tooltip as GameOverBannerMenu).Setup(m_actor);
            return true;
        }

        if (m_statline != null
            && m_statline.AccountID != ClientGameManager.Get().GetPlayerAccountData().AccountId
            && m_statline.AccountID != 0
            && (m_statline.IsHumanControlled || m_statline.HumanReplacedByBot))
        {
            (tooltip as GameOverBannerMenu).Setup(m_statline);
            return true;
        }

        return false;
    }

    private bool SetupContibutionTooltip(UITooltipBase tooltip)
    {
        UIContributionTooltip uIContributionTooltip = tooltip as UIContributionTooltip;
        if (m_actor != null && uIContributionTooltip != null)
        {
            uIContributionTooltip.Setup(
                StringUtil.TR("Contribution", "GameOver"),
                m_actor.GetActorBehavior().GetContributionBreakdownForUI());
            return true;
        }

        return false;
    }

    public void SetStatPage(UIGameStatsWindow.StatsPage page)
    {
        switch (page)
        {
            case UIGameStatsWindow.StatsPage.Mods:
            {
                if (m_statsPageOneContainer != null)
                {
                    UIManager.SetGameObjectActive(m_statsPageOneContainer, false);
                }

                if (m_statsPageTwoContainer != null)
                {
                    UIManager.SetGameObjectActive(m_statsPageTwoContainer, true);
                }

                break;
            }
            case UIGameStatsWindow.StatsPage.Numbers:
            {
                if (m_statsPageOneContainer != null)
                {
                    UIManager.SetGameObjectActive(m_statsPageOneContainer, true);
                }

                if (m_statsPageTwoContainer != null)
                {
                    UIManager.SetGameObjectActive(m_statsPageTwoContainer, false);
                }

                break;
            }
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

        if (m_ggPackImage != null)
        {
            UIManager.SetGameObjectActive(
                m_ggPackImage,
                actorData != null
                && actorData.PlayerData != null
                && HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel
                    .NumberGGPacksUsed(actorData.PlayerData.PlayerHandle) > 0);
        }

        AbilityData actorAbilityData = null;
        List<Ability> actorAbilities = null;
        if (statline.Character != CharacterType.PunchingDummy && statline.Character != CharacterType.None)
        {
            GameWideData gameWideData = GameWideData.Get();
            if (gameWideData != null)
            {
                CharacterResourceLink characterResourceLink = gameWideData.GetCharacterResourceLink(statline.Character);
                m_characterImage.sprite = characterResourceLink.GetCharacterSelectIcon();
                actorAbilityData = characterResourceLink.ActorDataPrefab.GetComponent<AbilityData>();
                actorAbilities = actorAbilityData.GetAbilitiesAsList();
                actorAbilityData.InitAbilitySprites();
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
        ActorData activeOwnedActorData = GameFlowData.Get() != null
            ? GameFlowData.Get().activeOwnedActorData
            : null;
        Team team = activeOwnedActorData != null
            ? activeOwnedActorData.GetTeam()
            : Team.Spectator;
        UIManager.SetGameObjectActive(m_reportPlayerBtn, CanPlayerReport());
        m_reportPlayerBtn.spriteController.callback = ReportPlayerBtnClicked;
        m_reportPlayerBtn.spriteController.GetComponent<UITooltipHoverObject>()
            .Setup(TooltipType.Titled, ReportBtnTooltipSetup);
        bool showEmblem = true;
        bool showAsHuman = statline.IsHumanControlled || statline.IsBotMasqueradingAsHuman
                                                      || statline.HumanReplacedByBot;

        if (showAsHuman)
        {
            UIManager.SetGameObjectActive(m_botOverlay, false);
        }
        else
        {
            UIManager.SetGameObjectActive(m_DebugBotNameText, secretButtonClicked);
            UIManager.SetGameObjectActive(m_DebugBotDifficultyText, secretButtonClicked);
            m_DebugBotNameText.text = statline.DisplayName;
            if (m_actor != null
                && team != m_actor.GetTeam()
                && SinglePlayerCoordinator.Get() == null)
            {
                UIManager.SetGameObjectActive(m_botOverlay, false);
            }
            else
            {
                UIManager.SetGameObjectActive(m_botOverlay, true);
                showEmblem = false;
            }
        }

        if (statline.IsPerspective)
        {
            UIManager.SetGameObjectActive(m_selfImage, true);
            UIManager.SetGameObjectActive(m_enemyImage, false);
            UIManager.SetGameObjectActive(m_allyImage, false);
        }
        else
        {
            UIManager.SetGameObjectActive(
                m_selfImage,
                activeOwnedActorData != null && actorData == activeOwnedActorData);
            UIManager.SetGameObjectActive(
                m_enemyImage,
                !statline.IsAlly && (activeOwnedActorData == null || actorData != activeOwnedActorData));
            UIManager.SetGameObjectActive(
                m_allyImage,
                statline.IsAlly && (activeOwnedActorData == null || actorData != activeOwnedActorData));
        }

        if (team == Team.Spectator)
        {
            bool isTeamA = m_actor == null || m_actor.GetTeam() == Team.TeamA;
            UIManager.SetGameObjectActive(m_teamAImage, isTeamA);
            UIManager.SetGameObjectActive(m_DebugTeamAImage, isTeamA);
            UIManager.SetGameObjectActive(m_teamBImage, !isTeamA);
            UIManager.SetGameObjectActive(m_DebugTeamBImage, !isTeamA);
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
#if EVOS
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

        GameBalanceVars.PlayerBanner emblem = GameWideData.Get().m_gameBalanceVars.GetBanner(statline.EmblemID);
        if (emblem != null)
        {
            UIManager.SetGameObjectActive(m_emblemImage, true);
            m_emblemImage.sprite = (Sprite)Resources.Load(emblem.m_resourceString, typeof(Sprite));
        }

        UIManager.SetGameObjectActive(m_emblemImage, showEmblem);
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

        if (teamInfo != null && teamInfo.TeamPlayerInfo != null)
        {
            LobbyPlayerInfo lobbyPlayerInfo = teamInfo
                .TeamPlayerInfo
                .FirstOrDefault(element => element.PlayerId == statline.PlayerId);
            if (lobbyPlayerInfo != null)
            {
                if (!statline.IsHumanControlled && secretButtonClicked)
                {
                    m_DebugBotDifficultyText.text = $"AI Level: {(int)(lobbyPlayerInfo.Difficulty + 1)}";
                }

                Dictionary<int, ForbiddenDevKnowledge> forbiddenDevKnowledge = GameManager.Get().ForbiddenDevKnowledge;
                if (!forbiddenDevKnowledge.IsNullOrEmpty()
                    && forbiddenDevKnowledge.TryGetValue(lobbyPlayerInfo.PlayerId, out ForbiddenDevKnowledge value))
                {
                    if (m_DebugAccountELO != null)
                    {
                        string arg = value.UsedMatchmakingElo == value.AccMatchmakingElo ? "green" : "orange";
                        m_DebugAccountELO.text = $"<color={arg}>{value.UsedMatchmakingElo:F0}</color>";
                    }

                    if (m_DebugCharacterELO != null)
                    {
                        if (!s_groupToColorMap.TryGetValue(value.GroupIdAtStartOfMatch, out int groupColor))
                        {
                            groupColor = s_nextColorId;
                            s_nextColorId++;
                            s_groupToColorMap.Add(value.GroupIdAtStartOfMatch, groupColor);
                        }

                        m_DebugCharacterELO.text =
                            $"<color={s_groupColors[groupColor]}>{value.CharMatchmakingElo:F0}</color>";
                    }
                }
                else
                {
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

        AbilityData abilityData = actorData != null ? actorData.GetAbilityData() : null;
        for (int i = 0; i < statline.AbilityEntries.Length; i++)
        {
            if (i >= m_abilityBtns.Length
                || actorAbilityData == null
                || i >= actorAbilities.Count)
            {
                continue;
            }

            MatchResultsStatline.AbilityEntry statlineAbilityEntry = statline.AbilityEntries[i];
            AbilityData.AbilityEntry abilityEntry = abilityData != null ? abilityData.abilityEntries[i] : null;
            Ability theAbility = abilityEntry != null ? abilityEntry.ability : actorAbilities[i];
            if (theAbility == null)
            {
                continue;
            }

            AbilityMod abilityMod = AbilityModHelper.GetModForAbility(theAbility, statlineAbilityEntry.AbilityModId);
            m_abilityReferences.Add(theAbility);
            m_abilityBtns[i].spriteController.GetComponent<UITooltipHoverObject>().Setup(
                TooltipType.Ability,
                tooltip =>
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

            if (abilityEntry != null && actorData != null)
            {
                int currentTechPoints = actorData.TechPoints + actorData.ReservedTechPoints;
                bool isAbilityQueued = abilityData.HasQueuedAction((AbilityData.ActionType)i);
                bool hasTechPoints = theAbility.GetModdedCost() <= currentTechPoints || isAbilityQueued;
                string cooldown = string.Empty;
                if (theAbility.GetModdedMaxStocks() <= 0)
                {
                    if (abilityEntry.GetCooldownRemaining() != 0 || !hasTechPoints)
                    {
                        if (abilityEntry.GetCooldownRemaining() > 0)
                        {
                            if (currentTechPoints < actorData.GetMaxTechPoints()
                                || !AbilityUtils.AbilityHasTag(theAbility, AbilityTags.IgnoreCooldownIfFullEnergy))
                            {
                                cooldown = abilityEntry.GetCooldownRemaining().ToString();
                            }
                        }
                        else if (abilityEntry.GetCooldownRemaining() == -1)
                        {
                            cooldown = "~";
                        }
                    }
                }
                else
                {
                    int moddedMaxStocks = theAbility.GetModdedMaxStocks();
                    AbilityData ad = actorData.GetComponent<AbilityData>();
                    AbilityData.ActionType actionTypeOfAbility =
                        ad.GetActionTypeOfAbility(theAbility);
                    if (actionTypeOfAbility != AbilityData.ActionType.INVALID_ACTION)
                    {
                        int consumedStocksCount = ad.GetConsumedStocksCount(actionTypeOfAbility);
                        int stockRefreshCountdown = ad.GetStockRefreshCountdown(actionTypeOfAbility);
                        int stocksRemaining = moddedMaxStocks - consumedStocksCount;
                        if (!ad.IsAbilityAllowedByUnlockTurns(actionTypeOfAbility))
                        {
                            cooldown = ad.GetTurnsTillUnlock(actionTypeOfAbility).ToString();
                        }
                        else if (stockRefreshCountdown > 0)
                        {
                            if (stocksRemaining == 0 || theAbility.RefillAllStockOnRefresh())
                            {
                                cooldown = stockRefreshCountdown.ToString();
                            }
                        }
                    }
                }

                if (theAbility.GetModdedCost() > 0)
                {
                    UIManager.SetGameObjectActive(
                        m_disableContainer[i],
                        theAbility.GetModdedCost() > currentTechPoints);
                    m_cooldownLabel[i].text = string.Empty;
                }
                else
                {
                    if (m_cooldownLabel[i].text != cooldown)
                    {
                        m_cooldownLabel[i].text = cooldown;
                    }

                    UIManager.SetGameObjectActive(m_disableContainer[i], !m_cooldownLabel[i].text.IsNullOrEmpty());
                }
            }
            else
            {
                UIManager.SetGameObjectActive(m_disableContainer[i], false);
                m_cooldownLabel[i].text = string.Empty;
            }

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

        m_ContributionHitBoxTooltip.spriteController
            .GetComponent<UITooltipClickObject>()
            .Setup(TooltipType.PlayerBannerMenu, ClickedContribution);
        m_ContributionHitBoxTooltip.spriteController
            .GetComponent<UITooltipHoverObject>()
            .Setup(TooltipType.Contribution, SetupContibutionTooltip);
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
            m_TimePerTurn.text = statline.TotalPlayerTurns == 0
                ? "-"
                : $"{statline.TotalPlayerLockInTime / statline.TotalPlayerTurns:F2}";
        }

        if (secretButtonClicked && m_DebugTotalContribution != null)
        {
            m_DebugTotalContribution.text = statline.TotalPlayerContribution.ToString();
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
    }

    private bool ReportBtnTooltipSetup(UITooltipBase tooltip)
    {
        (tooltip as UITitledTooltip).Setup(
            StringUtil.TR("ReportPlayer", "Global"),
            string.Format(StringUtil.TR("ReportPlayerName", "Global"), m_actor.DisplayName));
        return true;
    }

    public void ReportPlayerBtnClicked(BaseEventData data)
    {
        PlayerDetails playerDetails = GameFlow.Get().playerDetails[m_actor.PlayerData.GetPlayer()];
        UILandingPageFullScreenMenus.Get().SetReportContainerVisible(
            true,
            playerDetails.m_handle,
            playerDetails.m_accountId,
            playerDetails.m_botsMasqueradeAsHumans);
    }

    private void Update()
    {
        if (m_ISO_showing < m_ISO_toShow)
        {
            m_ISO_showing += Time.deltaTime * ISO_increase_rate;
        }

        if (m_ISO_showing > m_ISO_toShow)
        {
            m_ISO_showing = m_ISO_toShow;
        }
    }

    public static void PreSetupInitialization()
    {
        s_groupToColorMap = new Dictionary<long, int> { { 0L, 0 } };
        s_nextColorId = 1;
    }
}