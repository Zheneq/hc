using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class LobbyGameplayOverrides
{
    public Dictionary<CharacterType, CharacterConfig> CharacterConfigOverrides =
        new Dictionary<CharacterType, CharacterConfig>(default(CharacterTypeComparer));
    public Dictionary<CharacterType, CharacterConfig> CharacterConfigs =
        new Dictionary<CharacterType, CharacterConfig>(default(CharacterTypeComparer));
    public Dictionary<CardType, CardConfigOverride>
        CardConfigOverrides = new Dictionary<CardType, CardConfigOverride>();
    public Dictionary<CharacterType, CharacterAbilityConfigOverride> CharacterAbilityConfigOverrides =
        new Dictionary<CharacterType, CharacterAbilityConfigOverride>();
    public Dictionary<CharacterType, CharacterSkinConfigOverride> CharacterSkinConfigOverrides =
        new Dictionary<CharacterType, CharacterSkinConfigOverride>();
    public Dictionary<int, QuestConfigOverride> QuestConfigOverrides = new Dictionary<int, QuestConfigOverride>();
    public Dictionary<int, FactionCompetitionConfigOverride> FactionCompetitionConfigOverrides =
        new Dictionary<int, FactionCompetitionConfigOverride>();
    public List<string> DisabledMaps = new List<string>();
    public List<GameType> DisabledGameTypes = new List<GameType>();
    public bool EnableMods = true;
    public bool EnableCards = true;
    public bool EnableTaunts = true;
    public bool EnableQuests = true;
    public bool EnableHiddenCharacters;
    public bool RankedUpdatesEnabled = true;
    public bool EnableAllMods;
    public bool EnableAllAbilityVfxSwaps;
    public bool EnableShop = true;
    public bool EnableSeasons = true;
    public bool EnableDiscord;
    public bool EnableDiscordSdk;
    public bool EnableFacebook;
    public bool EnableConversations = true;
    public bool EnableEventBonus = true;
    public bool EnableClientPerformanceCollecting;
    public bool EnableSteamAchievements;
    public bool AllowSpectators = true;
    public bool AllowSpectatorsOutsideCustom;
    public bool AllowReconnectingToGameInstantly;
    public int SpectatorOutsideCustomTurnDelay = 2;
    public bool SoloGameNoAutoLockinOnTimeout = true;
    public bool UseSpectatorReplays;
    public bool DisableControlPadInput;
    public bool UseFakeGameServersForLoadTests;
    public bool UseFakeClientConnectionsForLoadTests = true;
    public int LoadTestClients = 10;
    public int LoadTestMaxClientsPerInstance = 10;
    public double LoadTestLoginRate = 1.0;
    public double LoadTestLobbyDelay;
    public double LoadTestReadyDelay;
    public int LoadTestSoloGamePercentage;
    public int LoadTestGroupingPercentage;
    public string LoadTestFakeEntitlements = "GAME_ACCESS";
    public DateTime PlayerPenaltyAmnesty = DateTime.MinValue;
    public int EventFreePlayerXPBonusPercent;
    public int EventPaidPlayerXPBonusPercent;
    public int EventISOBonusPercent;
    public int EventGGBoostBonusPercent;
    public int EventTrustInfluenceBonusPercent;
    public int EventFreelancerCurrencyPerMatchBonusPercent;
    public DateTime EventBonusStartDate = DateTime.MinValue;
    public DateTime EventBonusEndDate = DateTime.MaxValue;
    public string RequiredEventBonusEntitlement;
    public CharacterType ForcedFreeRotationCharacterForGroupA;
    public CharacterType ForcedFreeRotationCharacterForGroupB;
    public TimeSpan ClientPerformanceCollectingFrequency = TimeSpan.FromMinutes(5.0);
    public TimeSpan RankedLeaderboardExpirationTime = TimeSpan.MaxValue;

    public LobbyGameplayOverrides Clone()
    {
        return (LobbyGameplayOverrides)MemberwiseClone();
    }

    public void SetBaseCharacterConfigs(GameWideData gameWideData)
    {
        if (Application.isEditor)
        {
            return;
        }

        CharacterResourceLink[] characterResourceLinks = gameWideData.m_characterResourceLinks;
        foreach (CharacterResourceLink characterResourceLink in characterResourceLinks)
        {
            CharacterConfig characterConfig = GetCharacterConfig(characterResourceLink.m_characterType);
            if (characterConfig != null)
            {
                characterResourceLink.m_allowForBots = characterConfig.AllowForBots;
                characterResourceLink.m_allowForPlayers = characterConfig.AllowForPlayers;
                characterResourceLink.m_isHidden = characterConfig.IsHidden;
            }
        }
    }

    public void SetFactionConfigs(FactionWideData factionWideData)
    {
        if (Application.isEditor)
        {
            return;
        }

        foreach (FactionCompetitionConfigOverride configOverride in FactionCompetitionConfigOverrides.Values)
        {
            foreach (FactionTierConfigOverride factionTierConfig in configOverride.FactionTierConfigs)
            {
                factionWideData.SetCompetitionFactionTierInfo(
                    factionTierConfig.CompetitionId,
                    factionTierConfig.FactionId,
                    factionTierConfig.TierId,
                    factionTierConfig.ContributionToComplete);
            }
        }
    }

    public void SetBaseCharacterConfigs(LobbyGameplayData gameplayData)
    {
        CharacterConfigs.Clear();
        foreach (LobbyCharacterGameplayData characterData in gameplayData.CharacterData.Values)
        {
            CharacterConfigs.Add(characterData.CharacterType, characterData.CharacterConfig);
        }
    }

    public void ClearBaseCharacterConfigs()
    {
        CharacterConfigs.Clear();
    }

    public IEnumerable<CharacterType> GetCharacterTypes()
    {
        return CharacterConfigs.Keys;
    }

    public CharacterConfig GetCharacterConfig(CharacterType characterType)
    {
        CharacterConfigOverrides.TryGetValue(characterType, out CharacterConfig value);
        if (value == null)
        {
            CharacterConfigs.TryGetValue(characterType, out value);
        }

        return value;
    }

    public void SetCharacterConfigOverride(CharacterConfig characterConfigOverride)
    {
        CharacterConfigOverrides[characterConfigOverride.CharacterType] = characterConfigOverride;
    }

    public bool IsCharacterAllowedForPlayers(CharacterType characterType)
    {
        CharacterConfig characterConfig = GetCharacterConfig(characterType);
        return characterType.IsValidForHumanGameplay()
               && characterConfig != null
               && characterConfig.AllowForPlayers
               && (EnableHiddenCharacters || !characterConfig.IsHidden);
    }

    public bool IsCharacterAllowedForGameType(
        CharacterType characterType,
        GameType gameType,
        GameSubType gameSubType,
        IFreelancerSetQueryInterface qi)
    {
        CharacterConfig characterConfig = GetCharacterConfig(characterType);
        if (characterConfig == null)
        {
            return false;
        }

        if (!characterConfig.GameTypesProhibitedFrom.IsNullOrEmpty()
            && characterConfig.GameTypesProhibitedFrom.Contains(gameType))
        {
            return false;
        }

        return gameSubType == null || gameSubType.IsCharacterAllowed(characterType, qi);
    }

    public bool IsCharacterAllowedForBots(CharacterType characterType)
    {
        CharacterConfig characterConfig = GetCharacterConfig(characterType);
        return characterConfig != null
               && characterConfig.AllowForBots
               && (EnableHiddenCharacters || !characterConfig.IsHidden);
    }

    public bool IsValidForHumanPreGameSelection(CharacterType characterType)
    {
        CharacterConfig characterConfig = GetCharacterConfig(characterType);
        return characterType.IsValidForHumanPreGameSelection()
               && characterConfig != null
               && characterConfig.AllowForPlayers
               && (EnableHiddenCharacters || !characterConfig.IsHidden);
    }

    public bool IsCharacterVisible(CharacterType characterType)
    {
        CharacterConfig characterConfig = GetCharacterConfig(characterType);
        return characterConfig != null
               && (EnableHiddenCharacters || !characterConfig.IsHidden);
    }

    public void SetCardConfigOverride(CardConfigOverride cardConfigOverride)
    {
        CardConfigOverrides[cardConfigOverride.CardType] = cardConfigOverride;
    }

    public CardConfigOverride GetCardConfig(CardType cardType)
    {
        CardConfigOverrides.TryGetValue(cardType, out CardConfigOverride value);
        return value;
    }

    public bool IsCardAllowed(CardType cardType)
    {
        if (!EnableCards)
        {
            return false;
        }

        CardConfigOverride cardConfig = GetCardConfig(cardType);
        if (cardConfig == null)
        {
            return true;
        }

        return cardConfig.Allowed;
    }

    public void SetCharacterAbilityConfigOverride(CharacterAbilityConfigOverride characterAbilityConfigOverride)
    {
        CharacterAbilityConfigOverrides[characterAbilityConfigOverride.CharacterType] = characterAbilityConfigOverride;
    }

    public CharacterAbilityConfigOverride GetCharacterAbilityConfigOverride(CharacterType characterType)
    {
        CharacterAbilityConfigOverrides.TryGetValue(characterType, out CharacterAbilityConfigOverride value);
        return value;
    }

    public bool AreTauntsEnabled()
    {
        return EnableTaunts;
    }

    public bool IsTauntAllowed(CharacterType characterType, int abilityIndex, int tauntId)
    {
        if (abilityIndex < 0)
        {
            return false;
        }

        if (!AreTauntsEnabled())
        {
            return false;
        }

        CharacterAbilityConfigOverride characterAbilityConfigOverride =
            GetCharacterAbilityConfigOverride(characterType);
        if (characterAbilityConfigOverride == null)
        {
            return true;
        }

        AbilityConfigOverride abilityConfig = characterAbilityConfigOverride.GetAbilityConfig(abilityIndex);
        if (abilityConfig == null)
        {
            return true;
        }

        return abilityConfig.GetAbilityTauntConfig(tauntId)?.Allowed ?? true;
    }

    public bool IsAbilityModAllowed(CharacterType characterType, int abilityIndex, int modIndex)
    {
        if (!EnableMods)
        {
            return false;
        }

        CharacterAbilityConfigOverride characterAbilityConfigOverride =
            GetCharacterAbilityConfigOverride(characterType);
        if (characterAbilityConfigOverride == null)
        {
            return true;
        }

        AbilityConfigOverride abilityConfig = characterAbilityConfigOverride.GetAbilityConfig(abilityIndex);
        if (abilityConfig == null)
        {
            return true;
        }

        AbilityModConfigOverride abilityModConfig = abilityConfig.GetAbilityModConfig(modIndex);
        if (abilityModConfig == null)
        {
            return true;
        }

        return abilityModConfig.Allowed;
    }

    public void SetCharacterSkinConfigOverride(CharacterSkinConfigOverride characterSkinConfigOverride)
    {
        CharacterSkinConfigOverrides[characterSkinConfigOverride.CharacterType] = characterSkinConfigOverride;
    }

    public CharacterSkinConfigOverride GetCharacterSkinConfigOverride(CharacterType characterType)
    {
        CharacterSkinConfigOverrides.TryGetValue(characterType, out CharacterSkinConfigOverride value);
        return value;
    }

    public SkinConfigOverride GetSkinConfigOverride(
        CharacterType characterType,
        int skinIndex,
        int patternIndex,
        int colorIndex)
    {
        CharacterSkinConfigOverride characterSkinConfigOverride = GetCharacterSkinConfigOverride(characterType);
        if (characterSkinConfigOverride == null)
        {
            return null;
        }

        return characterSkinConfigOverride.SkinConfigs.FirstOrDefault(c =>
            c.SkinIndex == skinIndex && c.PatternIndex == patternIndex && c.ColorIndex == colorIndex);
    }

    public bool IsColorAllowed(CharacterType characterType, int skinIndex, int patternIndex, int colorIndex)
    {
        if (skinIndex == 0 && patternIndex == 0 && colorIndex == 0)
        {
            return true;
        }

        CharacterSkinConfigOverride characterSkinConfigOverride = GetCharacterSkinConfigOverride(characterType);
        if (characterSkinConfigOverride == null)
        {
            return true;
        }

        return characterSkinConfigOverride.SkinConfigs.FirstOrDefault(c =>
            c.SkinIndex == skinIndex && c.PatternIndex == patternIndex && c.ColorIndex == colorIndex)?.Allowed ?? true;
    }

    public QuestConfigOverride GetQuestConfig(int questId)
    {
        QuestConfigOverrides.TryGetValue(questId, out QuestConfigOverride value);
        return value;
    }

    public bool IsQuestEnabled(int questId)
    {
        if (!EnableQuests)
        {
            return false;
        }

        QuestConfigOverride questConfig = GetQuestConfig(questId);
        if (questConfig == null)
        {
            return true;
        }

        return questConfig.Enabled;
    }

    public bool ShouldAbandonQuest(int questId)
    {
        if (!EnableQuests)
        {
            return false;
        }

        QuestConfigOverride questConfig = GetQuestConfig(questId);
        if (questConfig == null)
        {
            return false;
        }

        return questConfig.ShouldAbandon;
    }

    public List<string> GetDifferences(LobbyGameplayOverrides other)
    {
        List<string> list = new List<string>();
        try
        {
            if (other.EnableCards != EnableCards)
            {
                list.Add($"Cards have been {(other.EnableCards ? "enabled" : "disabled")}");
            }

            if (other.EnableMods != EnableMods)
            {
                list.Add($"Mods have been {(other.EnableMods ? "enabled" : "disabled")}");
            }

            if (other.EnableTaunts != EnableTaunts)
            {
                list.Add($"Taunts have been {(other.EnableTaunts ? "enabled" : "disabled")}");
            }

            if (other.EnableAllMods != EnableAllMods)
            {
                list.Add($"All mods mode has been {(other.EnableAllMods ? "enabled" : "disabled")}");
            }

            if (other.EnableShop != EnableShop)
            {
                list.Add($"Shop has been {(other.EnableShop ? "enabled" : "disabled")}");
            }

            if (other.EnableSeasons != EnableSeasons)
            {
                list.Add($"Seasons has been {(other.EnableSeasons ? "enabled" : "disabled")}");
            }

            if (other.EnableQuests != EnableQuests)
            {
                list.Add($"Quests have been {(other.EnableQuests ? "enabled" : "disabled")}");
            }

            if (other.EventFreePlayerXPBonusPercent != EventFreePlayerXPBonusPercent)
            {
                list.Add($"XP Bonus (Free Player) is now {other.EventFreePlayerXPBonusPercent} %");
            }

            if (other.EventPaidPlayerXPBonusPercent != EventPaidPlayerXPBonusPercent)
            {
                list.Add($"XP Bonus (Paid Player) is now {other.EventPaidPlayerXPBonusPercent} %");
            }

            if (other.EventISOBonusPercent != EventISOBonusPercent)
            {
                list.Add($"ISO Bonus is now {other.EventISOBonusPercent} %");
            }

            if (other.EventGGBoostBonusPercent != EventGGBoostBonusPercent)
            {
                list.Add($"GG Boost Bonus is now {other.EventGGBoostBonusPercent} %");
            }

            if (other.EventTrustInfluenceBonusPercent != EventTrustInfluenceBonusPercent)
            {
                list.Add($"Trust Influence Bonus is now {other.EventTrustInfluenceBonusPercent} %");
            }

            if (other.EventFreelancerCurrencyPerMatchBonusPercent != EventFreelancerCurrencyPerMatchBonusPercent)
            {
                list.Add($"Freelancer Currency Bonus is now {other.EventFreelancerCurrencyPerMatchBonusPercent} %");
            }

            foreach (GameType item in other.DisabledGameTypes.Union(DisabledGameTypes).Distinct())
            {
                bool flag = !DisabledGameTypes.IsNullOrEmpty() && DisabledGameTypes.Contains(item);
                bool flag2 = !other.DisabledGameTypes.IsNullOrEmpty() && other.DisabledGameTypes.Contains(item);
                if (flag != flag2)
                {
                    list.Add($"{(flag2 ? "Adding admin lock on" : "Removing admin lock from")} game type {item}");
                }
            }

            foreach (string current2 in other.DisabledMaps.Union(DisabledMaps).Distinct())
            {
                bool flag3 = !DisabledMaps.IsNullOrEmpty() && DisabledMaps.Contains(current2);
                bool flag4 = !other.DisabledMaps.IsNullOrEmpty() && other.DisabledMaps.Contains(current2);
                if (flag3 != flag4)
                {
                    list.Add($"{(flag4 ? "Adding admin lock on" : "Removing admin lock from")} map {current2}");
                }
            }

            foreach (CharacterConfig current3 in other.CharacterConfigOverrides.Values)
            {
                if (current3.CharacterRole == CharacterRole.None && current3.CharacterType.IsValidForHumanGameplay())
                {
                    Log.Error(
                        "Why is the console marking {0} as being role NONE?! That's going to mess up Free Rotation!",
                        current3.CharacterType.GetDisplayName());
                }

                CharacterConfig characterConfig = CharacterConfigOverrides.TryGetValue(current3.CharacterType);
                if (characterConfig != null)
                {
                    if (characterConfig.AllowForBots != current3.AllowForBots
                        || characterConfig.AllowForPlayers != current3.AllowForPlayers)
                    {
                        list.Add($"Overriding character {current3.CharacterType} to be {(current3.AllowForPlayers ? "allowed" : "not allowed")} for players and {(current3.AllowForBots ? "allowed" : "not allowed")} for bots");
                    }
                }
                else
                {
                    list.Add($"Adding overriding character {current3.CharacterType} to be {(current3.AllowForPlayers ? "allowed" : "not allowed")} for players and {(current3.AllowForBots ? "allowed" : "not allowed")} for bots");
                }

                foreach (GameType gameType in Enum.GetValues(typeof(GameType)))
                {
                    bool flag5 = characterConfig != null
                                 && !characterConfig.GameTypesProhibitedFrom.IsNullOrEmpty()
                                 && characterConfig.GameTypesProhibitedFrom.Contains(gameType);
                    bool flag6 = !current3.GameTypesProhibitedFrom.IsNullOrEmpty()
                                 && current3.GameTypesProhibitedFrom.Contains(gameType);
                    if (flag5 != flag6)
                    {
                        list.Add($"Overriding character {current3.CharacterType} to be {(flag6 ? "not allowed" : "allowed")} in {gameType} matches");
                    }
                }
            }

            foreach (CharacterConfig current4 in CharacterConfigOverrides.Values)
            {
                if (!other.CharacterConfigOverrides.ContainsKey(current4.CharacterType))
                {
                    list.Add($"Removing overrides for character {current4.CharacterType}");
                }
            }

            foreach (CardConfigOverride current5 in other.CardConfigOverrides.Values)
            {
                CardConfigOverride cardConfigOverride = CardConfigOverrides.TryGetValue(current5.CardType);
                if (cardConfigOverride == null || cardConfigOverride.Allowed != current5.Allowed)
                {
                    list.Add($"Overriding catalyst {current5.CardType} to be {(current5.Allowed ? "allowed" : "not allowed")}");
                }
            }

            foreach (CardConfigOverride current6 in CardConfigOverrides.Values)
            {
                if (!other.CardConfigOverrides.ContainsKey(current6.CardType))
                {
                    list.Add($"Removing overrides for catalyst {current6.CardType}");
                }
            }

            if (LobbyGameplayData.Get() != null)
            {
                foreach (CharacterAbilityConfigOverride current7 in other.CharacterAbilityConfigOverrides.Values)
                {
                    if (current7 == null)
                    {
                        continue;
                    }

                    LobbyCharacterGameplayData characterData =
                        LobbyGameplayData.Get().GetCharacterData(current7.CharacterType);
                    CharacterAbilityConfigOverride characterAbilityConfigOverride =
                        CharacterAbilityConfigOverrides.TryGetValue(current7.CharacterType);
                    for (int i = 0; i < current7.AbilityConfigs.Length; i++)
                    {
                        AbilityConfigOverride abilityConfigOverride = current7.AbilityConfigs[i];
                        if (abilityConfigOverride == null)
                        {
                            continue;
                        }

                        AbilityConfigOverride abilityConfigOverride2 =
                            characterAbilityConfigOverride?.AbilityConfigs?[i];
                        LobbyAbilityGameplayData abilityData = characterData.GetAbilityData(i);
                        if (abilityConfigOverride.AbilityModConfigs != null)
                        {
                            foreach (AbilityModConfigOverride current8 in
                                     abilityConfigOverride.AbilityModConfigs.Values)
                            {
                                AbilityModConfigOverride abilityModConfigOverride = abilityConfigOverride2
                                        ?.AbilityModConfigs
                                        ?.TryGetValue(current8.AbilityModIndex);
                                LobbyAbilityModGameplayData abilityModData =
                                    abilityData.GetAbilityModData(current8.AbilityModIndex);
                                if (abilityModConfigOverride == null
                                    || abilityModConfigOverride.Allowed != current8.Allowed)
                                {
                                    list.Add($"Overriding {current7.CharacterType} ability '{abilityData.Name}' mod '{abilityModData.Name}' to be {(current8.Allowed ? "allowed" : "not allowed")}");
                                }
                            }
                        }

                        if (abilityConfigOverride.AbilityTauntConfigs != null)
                        {
                            foreach (AbilityTauntConfigOverride current9 in abilityConfigOverride.AbilityTauntConfigs.Values)
                            {
                                AbilityTauntConfigOverride abilityTauntConfigOverride = abilityConfigOverride2
                                    ?.AbilityTauntConfigs
                                    ?.TryGetValue(current9.AbilityTauntIndex);
                                LobbyAbilityTauntData abilityTauntData =
                                    abilityData.GetAbilityTauntData(current9.AbilityTauntID);
                                if (abilityTauntConfigOverride == null
                                    || abilityTauntConfigOverride.Allowed != current9.Allowed)
                                {
                                    list.Add($"Overriding {current7.CharacterType} ability '{abilityData.Name}' taunt '{abilityTauntData.Name}' to be {(current9.Allowed ? "allowed" : "not allowed")}");
                                }
                            }
                        }
                    }
                }

                foreach (CharacterAbilityConfigOverride current10 in CharacterAbilityConfigOverrides.Values)
                {
                    CharacterAbilityConfigOverride characterAbilityConfigOverride2 =
                        other.CharacterAbilityConfigOverrides.TryGetValue(current10.CharacterType);
                    if (characterAbilityConfigOverride2 == null)
                    {
                        list.Add($"Removing all mod and taunt overrides for {current10.CharacterType}");
                    }
                    else
                    {
                        LobbyCharacterGameplayData characterData2 = LobbyGameplayData.Get()
                            .GetCharacterData(characterAbilityConfigOverride2.CharacterType);
                        for (int j = 0; j < current10.AbilityConfigs.Length; j++)
                        {
                            AbilityConfigOverride abilityConfigOverride3 = current10.AbilityConfigs[j];
                            if (abilityConfigOverride3 == null)
                            {
                                continue;
                            }

                            AbilityConfigOverride abilityConfigOverride4 =
                                characterAbilityConfigOverride2.AbilityConfigs[j];
                            LobbyAbilityGameplayData abilityData2 = characterData2.GetAbilityData(j);
                            if (abilityConfigOverride3.AbilityModConfigs != null)
                            {
                                foreach (AbilityModConfigOverride current11 in abilityConfigOverride3.AbilityModConfigs.Values)
                                {
                                    if (abilityConfigOverride4.AbilityModConfigs.TryGetValue(current11.AbilityModIndex) == null)
                                    {
                                        LobbyAbilityModGameplayData abilityModData2 = abilityData2.GetAbilityModData(current11.AbilityModIndex);
                                        list.Add($"Removing override for {characterAbilityConfigOverride2.CharacterType} ability '{abilityData2.Name}' mod '{abilityModData2.Name}'");
                                    }
                                }
                            }

                            if (abilityConfigOverride3.AbilityTauntConfigs != null)
                            {
                                foreach (AbilityTauntConfigOverride current12 in abilityConfigOverride3
                                             .AbilityTauntConfigs.Values)
                                {
                                    if (abilityConfigOverride4.AbilityTauntConfigs.TryGetValue(current12.AbilityTauntIndex) == null)
                                    {
                                        LobbyAbilityTauntData abilityTauntData2 = abilityData2.GetAbilityTauntData(current12.AbilityTauntID);
                                        list.Add($"Removing override for {characterAbilityConfigOverride2.CharacterType} ability '{abilityData2.Name}' taunt '{abilityTauntData2.Name}'");
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (FactionCompetitionConfigOverride current13 in other.FactionCompetitionConfigOverrides.Values)
                {
                    foreach (FactionTierConfigOverride otherFactionTierConfigOverride in current13.FactionTierConfigs)
                    {
                        FactionTierConfigOverride factionTierConfigOverride = FactionCompetitionConfigOverrides
                            .TryGetValue(current13.Index)
                            ?.FactionTierConfigs
                            .Find(o =>
                                o.CompetitionId == otherFactionTierConfigOverride.CompetitionId
                                && o.FactionId == otherFactionTierConfigOverride.FactionId
                                && o.TierId == otherFactionTierConfigOverride.TierId);
                        if (factionTierConfigOverride == null || otherFactionTierConfigOverride.ContributionToComplete != factionTierConfigOverride.ContributionToComplete)
                        {
                            list.Add($"Overriding faction(competitionId={otherFactionTierConfigOverride.CompetitionId} factionId={otherFactionTierConfigOverride.FactionId} tierId={otherFactionTierConfigOverride.TierId}) ContributionToComplete to be {otherFactionTierConfigOverride.ContributionToComplete}");
                        }
                    }
                }
            }

            return list;
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
            list.Add($"LobbyGameplayOverrides.GetDifferences failed: {ex.Message}");
            return list;
        }
    }
}