using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class LobbyGameplayOverrides
{
	public Dictionary<CharacterType, CharacterConfig> CharacterConfigOverrides = new Dictionary<CharacterType, CharacterConfig>(default(CharacterTypeComparer));

	public Dictionary<CharacterType, CharacterConfig> CharacterConfigs = new Dictionary<CharacterType, CharacterConfig>(default(CharacterTypeComparer));

	public Dictionary<CardType, CardConfigOverride> CardConfigOverrides = new Dictionary<CardType, CardConfigOverride>();

	public Dictionary<CharacterType, CharacterAbilityConfigOverride> CharacterAbilityConfigOverrides = new Dictionary<CharacterType, CharacterAbilityConfigOverride>();

	public Dictionary<CharacterType, CharacterSkinConfigOverride> CharacterSkinConfigOverrides = new Dictionary<CharacterType, CharacterSkinConfigOverride>();

	public Dictionary<int, QuestConfigOverride> QuestConfigOverrides = new Dictionary<int, QuestConfigOverride>();

	public Dictionary<int, FactionCompetitionConfigOverride> FactionCompetitionConfigOverrides = new Dictionary<int, FactionCompetitionConfigOverride>();

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
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void SetFactionConfigs(FactionWideData factionWideData)
	{
		if (!Application.isEditor)
		{
			using (Dictionary<int, FactionCompetitionConfigOverride>.ValueCollection.Enumerator enumerator = FactionCompetitionConfigOverrides.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FactionCompetitionConfigOverride current = enumerator.Current;
					foreach (FactionTierConfigOverride factionTierConfig in current.FactionTierConfigs)
					{
						factionWideData.SetCompetitionFactionTierInfo(factionTierConfig.CompetitionId, factionTierConfig.FactionId, factionTierConfig.TierId, factionTierConfig.ContributionToComplete);
					}
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return;
					}
				}
			}
		}
	}

	public void SetBaseCharacterConfigs(LobbyGameplayData gameplayData)
	{
		CharacterConfigs.Clear();
		using (Dictionary<CharacterType, LobbyCharacterGameplayData>.ValueCollection.Enumerator enumerator = gameplayData.CharacterData.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				LobbyCharacterGameplayData current = enumerator.Current;
				CharacterConfigs.Add(current.CharacterType, current.CharacterConfig);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return;
				}
			}
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
		CharacterConfig value = null;
		CharacterConfigOverrides.TryGetValue(characterType, out value);
		if (value == null)
		{
			CharacterConfigs.TryGetValue(characterType, out value);
		}
		return value;
	}

	public void SetCharacterConfigOverride(CharacterConfig characterConfigOverride)
	{
		if (CharacterConfigOverrides.ContainsKey(characterConfigOverride.CharacterType))
		{
			CharacterConfigOverrides[characterConfigOverride.CharacterType] = characterConfigOverride;
		}
		else
		{
			CharacterConfigOverrides.Add(characterConfigOverride.CharacterType, characterConfigOverride);
		}
	}

	public bool IsCharacterAllowedForPlayers(CharacterType characterType)
	{
		CharacterConfig characterConfig = GetCharacterConfig(characterType);
		int result;
		if (characterType.IsValidForHumanGameplay() && characterConfig != null)
		{
			if (characterConfig.AllowForPlayers)
			{
				result = ((EnableHiddenCharacters || !characterConfig.IsHidden) ? 1 : 0);
				goto IL_004f;
			}
		}
		result = 0;
		goto IL_004f;
		IL_004f:
		return (byte)result != 0;
	}

	public bool IsCharacterAllowedForGameType(CharacterType characterType, GameType gameType, GameSubType gameSubType, IFreelancerSetQueryInterface qi)
	{
		CharacterConfig characterConfig = GetCharacterConfig(characterType);
		if (characterConfig != null)
		{
			if (!characterConfig.GameTypesProhibitedFrom.IsNullOrEmpty())
			{
				if (characterConfig.GameTypesProhibitedFrom.Contains(gameType))
				{
					goto IL_006d;
				}
			}
			if (gameSubType != null)
			{
				if (!gameSubType.IsCharacterAllowed(characterType, qi))
				{
					goto IL_006d;
				}
			}
			return true;
		}
		goto IL_006d;
		IL_006d:
		return false;
	}

	public bool IsCharacterAllowedForBots(CharacterType characterType)
	{
		CharacterConfig characterConfig = GetCharacterConfig(characterType);
		int result;
		if (characterConfig != null)
		{
			if (characterConfig.AllowForBots)
			{
				result = ((EnableHiddenCharacters || !characterConfig.IsHidden) ? 1 : 0);
				goto IL_0049;
			}
		}
		result = 0;
		goto IL_0049;
		IL_0049:
		return (byte)result != 0;
	}

	public bool IsValidForHumanPreGameSelection(CharacterType characterType)
	{
		CharacterConfig characterConfig = GetCharacterConfig(characterType);
		int result;
		if (characterType.IsValidForHumanPreGameSelection())
		{
			if (characterConfig != null && characterConfig.AllowForPlayers)
			{
				result = ((EnableHiddenCharacters || !characterConfig.IsHidden) ? 1 : 0);
				goto IL_0053;
			}
		}
		result = 0;
		goto IL_0053;
		IL_0053:
		return (byte)result != 0;
	}

	public bool IsCharacterVisible(CharacterType characterType)
	{
		CharacterConfig characterConfig = GetCharacterConfig(characterType);
		int result;
		if (characterConfig != null)
		{
			result = ((EnableHiddenCharacters || !characterConfig.IsHidden) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public void SetCardConfigOverride(CardConfigOverride cardConfigOverride)
	{
		if (CardConfigOverrides.ContainsKey(cardConfigOverride.CardType))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					CardConfigOverrides[cardConfigOverride.CardType] = cardConfigOverride;
					return;
				}
			}
		}
		CardConfigOverrides.Add(cardConfigOverride.CardType, cardConfigOverride);
	}

	public CardConfigOverride GetCardConfig(CardType cardType)
	{
		CardConfigOverride value = null;
		CardConfigOverrides.TryGetValue(cardType, out value);
		return value;
	}

	public bool IsCardAllowed(CardType cardType)
	{
		if (!EnableCards)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		CardConfigOverride cardConfig = GetCardConfig(cardType);
		if (cardConfig == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		return cardConfig.Allowed;
	}

	public void SetCharacterAbilityConfigOverride(CharacterAbilityConfigOverride characterAbilityConfigOverride)
	{
		if (CharacterAbilityConfigOverrides.ContainsKey(characterAbilityConfigOverride.CharacterType))
		{
			CharacterAbilityConfigOverrides[characterAbilityConfigOverride.CharacterType] = characterAbilityConfigOverride;
		}
		else
		{
			CharacterAbilityConfigOverrides.Add(characterAbilityConfigOverride.CharacterType, characterAbilityConfigOverride);
		}
	}

	public CharacterAbilityConfigOverride GetCharacterAbilityConfigOverride(CharacterType characterType)
	{
		CharacterAbilityConfigOverride value = null;
		CharacterAbilityConfigOverrides.TryGetValue(characterType, out value);
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
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (!AreTauntsEnabled())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		CharacterAbilityConfigOverride characterAbilityConfigOverride = GetCharacterAbilityConfigOverride(characterType);
		if (characterAbilityConfigOverride == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		AbilityConfigOverride abilityConfig = characterAbilityConfigOverride.GetAbilityConfig(abilityIndex);
		if (abilityConfig == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		return abilityConfig.GetAbilityTauntConfig(tauntId)?.Allowed ?? true;
	}

	public bool IsAbilityModAllowed(CharacterType characterType, int abilityIndex, int modIndex)
	{
		if (!EnableMods)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		CharacterAbilityConfigOverride characterAbilityConfigOverride = GetCharacterAbilityConfigOverride(characterType);
		if (characterAbilityConfigOverride == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		AbilityConfigOverride abilityConfig = characterAbilityConfigOverride.GetAbilityConfig(abilityIndex);
		if (abilityConfig == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		AbilityModConfigOverride abilityModConfig = abilityConfig.GetAbilityModConfig(modIndex);
		if (abilityModConfig == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		return abilityModConfig.Allowed;
	}

	public void SetCharacterSkinConfigOverride(CharacterSkinConfigOverride characterSkinConfigOverride)
	{
		CharacterSkinConfigOverrides[characterSkinConfigOverride.CharacterType] = characterSkinConfigOverride;
	}

	public CharacterSkinConfigOverride GetCharacterSkinConfigOverride(CharacterType characterType)
	{
		CharacterSkinConfigOverride value = null;
		CharacterSkinConfigOverrides.TryGetValue(characterType, out value);
		return value;
	}

	public SkinConfigOverride GetSkinConfigOverride(CharacterType characterType, int skinIndex, int patternIndex, int colorIndex)
	{
		CharacterSkinConfigOverride characterSkinConfigOverride = GetCharacterSkinConfigOverride(characterType);
		if (characterSkinConfigOverride == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		return characterSkinConfigOverride.SkinConfigs.FirstOrDefault(delegate(SkinConfigOverride c)
		{
			int result;
			if (c.SkinIndex == skinIndex)
			{
				if (c.PatternIndex == patternIndex)
				{
					result = ((c.ColorIndex == colorIndex) ? 1 : 0);
					goto IL_004a;
				}
			}
			result = 0;
			goto IL_004a;
			IL_004a:
			return (byte)result != 0;
		});
	}

	public bool IsColorAllowed(CharacterType characterType, int skinIndex, int patternIndex, int colorIndex)
	{
		if (skinIndex == 0)
		{
			if (patternIndex == 0 && colorIndex == 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
		}
		CharacterSkinConfigOverride characterSkinConfigOverride = GetCharacterSkinConfigOverride(characterType);
		if (characterSkinConfigOverride == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		return characterSkinConfigOverride.SkinConfigs.FirstOrDefault(delegate(SkinConfigOverride c)
		{
			int result;
			if (c.SkinIndex == skinIndex)
			{
				if (c.PatternIndex == patternIndex)
				{
					result = ((c.ColorIndex == colorIndex) ? 1 : 0);
					goto IL_004a;
				}
			}
			result = 0;
			goto IL_004a;
			IL_004a:
			return (byte)result != 0;
		})?.Allowed ?? true;
	}

	public QuestConfigOverride GetQuestConfig(int questId)
	{
		QuestConfigOverride value = null;
		QuestConfigOverrides.TryGetValue(questId, out value);
		return value;
	}

	public bool IsQuestEnabled(int questId)
	{
		if (!EnableQuests)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		QuestConfigOverride questConfig = GetQuestConfig(questId);
		if (questConfig == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		return questConfig.Enabled;
	}

	public bool ShouldAbandonQuest(int questId)
	{
		if (!EnableQuests)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		QuestConfigOverride questConfig = GetQuestConfig(questId);
		if (questConfig == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
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
				object arg;
				if (other.EnableCards)
				{
					arg = "enabled";
				}
				else
				{
					arg = "disabled";
				}
				list.Add($"Cards have been {arg}");
			}
			if (other.EnableMods != EnableMods)
			{
				object arg2;
				if (other.EnableMods)
				{
					arg2 = "enabled";
				}
				else
				{
					arg2 = "disabled";
				}
				list.Add($"Mods have been {arg2}");
			}
			if (other.EnableTaunts != EnableTaunts)
			{
				object arg3;
				if (other.EnableTaunts)
				{
					arg3 = "enabled";
				}
				else
				{
					arg3 = "disabled";
				}
				list.Add($"Taunts have been {arg3}");
			}
			if (other.EnableAllMods != EnableAllMods)
			{
				object arg4;
				if (other.EnableAllMods)
				{
					arg4 = "enabled";
				}
				else
				{
					arg4 = "disabled";
				}
				list.Add($"All mods mode has been {arg4}");
			}
			if (other.EnableShop != EnableShop)
			{
				object arg5;
				if (other.EnableShop)
				{
					arg5 = "enabled";
				}
				else
				{
					arg5 = "disabled";
				}
				list.Add($"Shop has been {arg5}");
			}
			if (other.EnableSeasons != EnableSeasons)
			{
				object arg6;
				if (other.EnableSeasons)
				{
					arg6 = "enabled";
				}
				else
				{
					arg6 = "disabled";
				}
				list.Add($"Seasons has been {arg6}");
			}
			if (other.EnableQuests != EnableQuests)
			{
				list.Add(string.Format("Quests have been {0}", (!other.EnableQuests) ? "disabled" : "enabled"));
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
				int num;
				if (!DisabledGameTypes.IsNullOrEmpty())
				{
					num = (DisabledGameTypes.Contains(item) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				bool flag = (byte)num != 0;
				int num2;
				if (!other.DisabledGameTypes.IsNullOrEmpty())
				{
					num2 = (other.DisabledGameTypes.Contains(item) ? 1 : 0);
				}
				else
				{
					num2 = 0;
				}
				bool flag2 = (byte)num2 != 0;
				if (flag != flag2)
				{
					list.Add(string.Format("{0} game type {1}", (!flag2) ? "Removing admin lock from" : "Adding admin lock on", item));
				}
			}
			IEnumerator<string> enumerator2 = other.DisabledMaps.Union(DisabledMaps).Distinct().GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					string current2 = enumerator2.Current;
					bool flag3 = !DisabledMaps.IsNullOrEmpty() && DisabledMaps.Contains(current2);
					int num3;
					if (!other.DisabledMaps.IsNullOrEmpty())
					{
						num3 = (other.DisabledMaps.Contains(current2) ? 1 : 0);
					}
					else
					{
						num3 = 0;
					}
					bool flag4 = (byte)num3 != 0;
					if (flag3 != flag4)
					{
						object arg7;
						if (flag4)
						{
							arg7 = "Adding admin lock on";
						}
						else
						{
							arg7 = "Removing admin lock from";
						}
						list.Add($"{arg7} map {current2}");
					}
				}
			}
			finally
			{
				if (enumerator2 != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							enumerator2.Dispose();
							goto end_IL_04c5;
						}
					}
				}
				end_IL_04c5:;
			}
			using (Dictionary<CharacterType, CharacterConfig>.ValueCollection.Enumerator enumerator3 = other.CharacterConfigOverrides.Values.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					CharacterConfig current3 = enumerator3.Current;
					if (current3.CharacterRole == CharacterRole.None && current3.CharacterType.IsValidForHumanGameplay())
					{
						Log.Error("Why is the console marking {0} as being role NONE?! That's going to mess up Free Rotation!", current3.CharacterType.GetDisplayName());
					}
					CharacterConfig characterConfig = CharacterConfigOverrides.TryGetValue(current3.CharacterType);
					if (characterConfig != null)
					{
						if (characterConfig.AllowForBots == current3.AllowForBots)
						{
							if (characterConfig.AllowForPlayers == current3.AllowForPlayers)
							{
								goto IL_065c;
							}
						}
						string arg8 = current3.CharacterType.ToString();
						string arg9 = (!current3.AllowForPlayers) ? "not allowed" : "allowed";
						object arg10;
						if (current3.AllowForBots)
						{
							arg10 = "allowed";
						}
						else
						{
							arg10 = "not allowed";
						}
						list.Add($"Overriding character {arg8} to be {arg9} for players and {arg10} for bots");
					}
					else
					{
						string arg11 = current3.CharacterType.ToString();
						object arg12;
						if (current3.AllowForPlayers)
						{
							arg12 = "allowed";
						}
						else
						{
							arg12 = "not allowed";
						}
						object arg13;
						if (current3.AllowForBots)
						{
							arg13 = "allowed";
						}
						else
						{
							arg13 = "not allowed";
						}
						list.Add($"Adding overriding character {arg11} to be {arg12} for players and {arg13} for bots");
					}
					goto IL_065c;
					IL_065c:
					IEnumerator enumerator4 = Enum.GetValues(typeof(GameType)).GetEnumerator();
					try
					{
						while (enumerator4.MoveNext())
						{
							GameType gameType = (GameType)enumerator4.Current;
							int num4;
							if (characterConfig != null && !characterConfig.GameTypesProhibitedFrom.IsNullOrEmpty())
							{
								num4 = (characterConfig.GameTypesProhibitedFrom.Contains(gameType) ? 1 : 0);
							}
							else
							{
								num4 = 0;
							}
							bool flag5 = (byte)num4 != 0;
							bool flag6 = !current3.GameTypesProhibitedFrom.IsNullOrEmpty() && current3.GameTypesProhibitedFrom.Contains(gameType);
							if (flag5 != flag6)
							{
								list.Add(string.Format("Overriding character {0} to be {1} in {2} matches", current3.CharacterType, (!flag6) ? "allowed" : "not allowed", gameType));
							}
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator4 as IDisposable)) != null)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									disposable.Dispose();
									goto end_IL_073a;
								}
							}
						}
						end_IL_073a:;
					}
				}
			}
			using (Dictionary<CharacterType, CharacterConfig>.ValueCollection.Enumerator enumerator5 = CharacterConfigOverrides.Values.GetEnumerator())
			{
				while (enumerator5.MoveNext())
				{
					CharacterConfig current4 = enumerator5.Current;
					if (!other.CharacterConfigOverrides.ContainsKey(current4.CharacterType))
					{
						list.Add($"Removing overrides for character {current4.CharacterType.ToString()}");
					}
				}
			}
			using (Dictionary<CardType, CardConfigOverride>.ValueCollection.Enumerator enumerator6 = other.CardConfigOverrides.Values.GetEnumerator())
			{
				while (enumerator6.MoveNext())
				{
					CardConfigOverride current5 = enumerator6.Current;
					CardConfigOverride cardConfigOverride = CardConfigOverrides.TryGetValue(current5.CardType);
					if (cardConfigOverride != null)
					{
						if (cardConfigOverride.Allowed == current5.Allowed)
						{
							continue;
						}
					}
					string arg14 = current5.CardType.ToString();
					object arg15;
					if (current5.Allowed)
					{
						arg15 = "allowed";
					}
					else
					{
						arg15 = "not allowed";
					}
					list.Add($"Overriding catalyst {arg14} to be {arg15}");
				}
			}
			using (Dictionary<CardType, CardConfigOverride>.ValueCollection.Enumerator enumerator7 = CardConfigOverrides.Values.GetEnumerator())
			{
				while (enumerator7.MoveNext())
				{
					CardConfigOverride current6 = enumerator7.Current;
					if (!other.CardConfigOverrides.ContainsKey(current6.CardType))
					{
						list.Add($"Removing overrides for catalyst {current6.CardType.ToString()}");
					}
				}
			}
			if (LobbyGameplayData.Get() != null)
			{
				using (Dictionary<CharacterType, CharacterAbilityConfigOverride>.ValueCollection.Enumerator enumerator8 = other.CharacterAbilityConfigOverrides.Values.GetEnumerator())
				{
					while (enumerator8.MoveNext())
					{
						CharacterAbilityConfigOverride current7 = enumerator8.Current;
						if (current7 == null)
						{
						}
						else
						{
							LobbyCharacterGameplayData characterData = LobbyGameplayData.Get().GetCharacterData(current7.CharacterType);
							CharacterAbilityConfigOverride characterAbilityConfigOverride = CharacterAbilityConfigOverrides.TryGetValue(current7.CharacterType);
							for (int i = 0; i < current7.AbilityConfigs.Length; i++)
							{
								AbilityConfigOverride abilityConfigOverride = current7.AbilityConfigs[i];
								if (abilityConfigOverride == null)
								{
								}
								else
								{
									AbilityConfigOverride abilityConfigOverride2 = null;
									if (characterAbilityConfigOverride != null && characterAbilityConfigOverride.AbilityConfigs != null)
									{
										abilityConfigOverride2 = characterAbilityConfigOverride.AbilityConfigs[i];
									}
									LobbyAbilityGameplayData abilityData = characterData.GetAbilityData(i);
									if (abilityConfigOverride.AbilityModConfigs != null)
									{
										using (Dictionary<int, AbilityModConfigOverride>.ValueCollection.Enumerator enumerator9 = abilityConfigOverride.AbilityModConfigs.Values.GetEnumerator())
										{
											while (enumerator9.MoveNext())
											{
												AbilityModConfigOverride current8 = enumerator9.Current;
												AbilityModConfigOverride abilityModConfigOverride = null;
												if (abilityConfigOverride2 != null)
												{
													if (abilityConfigOverride2.AbilityModConfigs != null)
													{
														abilityModConfigOverride = abilityConfigOverride2.AbilityModConfigs.TryGetValue(current8.AbilityModIndex);
													}
												}
												LobbyAbilityModGameplayData abilityModData = abilityData.GetAbilityModData(current8.AbilityModIndex);
												if (abilityModConfigOverride != null)
												{
													if (abilityModConfigOverride.Allowed == current8.Allowed)
													{
														continue;
													}
												}
												object[] obj = new object[4]
												{
													current7.CharacterType.ToString(),
													abilityData.Name,
													abilityModData.Name,
													null
												};
												object obj2;
												if (current8.Allowed)
												{
													obj2 = "allowed";
												}
												else
												{
													obj2 = "not allowed";
												}
												obj[3] = obj2;
												list.Add(string.Format("Overriding {0} ability '{1}' mod '{2}' to be {3}", obj));
											}
										}
									}
									if (abilityConfigOverride.AbilityTauntConfigs != null)
									{
										using (Dictionary<int, AbilityTauntConfigOverride>.ValueCollection.Enumerator enumerator10 = abilityConfigOverride.AbilityTauntConfigs.Values.GetEnumerator())
										{
											while (enumerator10.MoveNext())
											{
												AbilityTauntConfigOverride current9 = enumerator10.Current;
												AbilityTauntConfigOverride abilityTauntConfigOverride = null;
												if (abilityConfigOverride2 != null && abilityConfigOverride2.AbilityTauntConfigs != null)
												{
													abilityTauntConfigOverride = abilityConfigOverride2.AbilityTauntConfigs.TryGetValue(current9.AbilityTauntIndex);
												}
												LobbyAbilityTauntData abilityTauntData = abilityData.GetAbilityTauntData(current9.AbilityTauntID);
												if (abilityTauntConfigOverride != null)
												{
													if (abilityTauntConfigOverride.Allowed == current9.Allowed)
													{
														continue;
													}
												}
												object[] obj3 = new object[4]
												{
													current7.CharacterType.ToString(),
													abilityData.Name,
													abilityTauntData.Name,
													null
												};
												object obj4;
												if (current9.Allowed)
												{
													obj4 = "allowed";
												}
												else
												{
													obj4 = "not allowed";
												}
												obj3[3] = obj4;
												list.Add(string.Format("Overriding {0} ability '{1}' taunt '{2}' to be {3}", obj3));
											}
										}
									}
								}
							}
						}
					}
				}
				using (Dictionary<CharacterType, CharacterAbilityConfigOverride>.ValueCollection.Enumerator enumerator11 = CharacterAbilityConfigOverrides.Values.GetEnumerator())
				{
					while (enumerator11.MoveNext())
					{
						CharacterAbilityConfigOverride current10 = enumerator11.Current;
						CharacterAbilityConfigOverride characterAbilityConfigOverride2 = other.CharacterAbilityConfigOverrides.TryGetValue(current10.CharacterType);
						if (characterAbilityConfigOverride2 == null)
						{
							list.Add($"Removing all mod and taunt overrides for {current10.CharacterType}");
						}
						else
						{
							LobbyCharacterGameplayData characterData2 = LobbyGameplayData.Get().GetCharacterData(characterAbilityConfigOverride2.CharacterType);
							for (int j = 0; j < current10.AbilityConfigs.Length; j++)
							{
								AbilityConfigOverride abilityConfigOverride3 = current10.AbilityConfigs[j];
								if (abilityConfigOverride3 == null)
								{
								}
								else
								{
									AbilityConfigOverride abilityConfigOverride4 = characterAbilityConfigOverride2.AbilityConfigs[j];
									LobbyAbilityGameplayData abilityData2 = characterData2.GetAbilityData(j);
									if (abilityConfigOverride3.AbilityModConfigs != null)
									{
										using (Dictionary<int, AbilityModConfigOverride>.ValueCollection.Enumerator enumerator12 = abilityConfigOverride3.AbilityModConfigs.Values.GetEnumerator())
										{
											while (enumerator12.MoveNext())
											{
												AbilityModConfigOverride current11 = enumerator12.Current;
												if (abilityConfigOverride4.AbilityModConfigs.TryGetValue(current11.AbilityModIndex) == null)
												{
													LobbyAbilityModGameplayData abilityModData2 = abilityData2.GetAbilityModData(current11.AbilityModIndex);
													list.Add($"Removing override for {characterAbilityConfigOverride2.CharacterType.ToString()} ability '{abilityData2.Name}' mod '{abilityModData2.Name}'");
												}
											}
										}
									}
									if (abilityConfigOverride3.AbilityTauntConfigs != null)
									{
										using (Dictionary<int, AbilityTauntConfigOverride>.ValueCollection.Enumerator enumerator13 = abilityConfigOverride3.AbilityTauntConfigs.Values.GetEnumerator())
										{
											while (enumerator13.MoveNext())
											{
												AbilityTauntConfigOverride current12 = enumerator13.Current;
												if (abilityConfigOverride4.AbilityTauntConfigs.TryGetValue(current12.AbilityTauntIndex) == null)
												{
													LobbyAbilityTauntData abilityTauntData2 = abilityData2.GetAbilityTauntData(current12.AbilityTauntID);
													list.Add($"Removing override for {characterAbilityConfigOverride2.CharacterType.ToString()} ability '{abilityData2.Name}' taunt '{abilityTauntData2.Name}'");
												}
											}
										}
									}
								}
							}
						}
					}
				}
				using (Dictionary<int, FactionCompetitionConfigOverride>.ValueCollection.Enumerator enumerator14 = other.FactionCompetitionConfigOverrides.Values.GetEnumerator())
				{
					while (enumerator14.MoveNext())
					{
						FactionCompetitionConfigOverride current13 = enumerator14.Current;
						using (List<FactionTierConfigOverride>.Enumerator enumerator15 = current13.FactionTierConfigs.GetEnumerator())
						{
							while (enumerator15.MoveNext())
							{
								FactionTierConfigOverride otherFactionTierConfigOverride = enumerator15.Current;
								FactionTierConfigOverride factionTierConfigOverride = FactionCompetitionConfigOverrides.TryGetValue(current13.Index)?.FactionTierConfigs.Find(delegate(FactionTierConfigOverride o)
								{
									int result;
									if (o.CompetitionId == otherFactionTierConfigOverride.CompetitionId)
									{
										if (o.FactionId == otherFactionTierConfigOverride.FactionId)
										{
											result = ((o.TierId == otherFactionTierConfigOverride.TierId) ? 1 : 0);
											goto IL_004f;
										}
									}
									result = 0;
									goto IL_004f;
									IL_004f:
									return (byte)result != 0;
								});
								if (factionTierConfigOverride != null)
								{
									if (otherFactionTierConfigOverride.ContributionToComplete == factionTierConfigOverride.ContributionToComplete)
									{
										continue;
									}
								}
								list.Add($"Overriding faction(competitionId={otherFactionTierConfigOverride.CompetitionId} factionId={otherFactionTierConfigOverride.FactionId} tierId={otherFactionTierConfigOverride.TierId}) ContributionToComplete to be {otherFactionTierConfigOverride.ContributionToComplete}");
							}
						}
					}
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							return list;
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
