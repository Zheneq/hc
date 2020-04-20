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

	public int LoadTestClients = 0xA;

	public int LoadTestMaxClientsPerInstance = 0xA;

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
		return (LobbyGameplayOverrides)base.MemberwiseClone();
	}

	public void SetBaseCharacterConfigs(GameWideData gameWideData)
	{
		if (!Application.isEditor)
		{
			foreach (CharacterResourceLink characterResourceLink in gameWideData.m_characterResourceLinks)
			{
				CharacterConfig characterConfig = this.GetCharacterConfig(characterResourceLink.m_characterType);
				if (characterConfig != null)
				{
					characterResourceLink.m_allowForBots = characterConfig.AllowForBots;
					characterResourceLink.m_allowForPlayers = characterConfig.AllowForPlayers;
					characterResourceLink.m_isHidden = characterConfig.IsHidden;
				}
			}
		}
	}

	public void SetFactionConfigs(FactionWideData factionWideData)
	{
		if (!Application.isEditor)
		{
			using (Dictionary<int, FactionCompetitionConfigOverride>.ValueCollection.Enumerator enumerator = this.FactionCompetitionConfigOverrides.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FactionCompetitionConfigOverride factionCompetitionConfigOverride = enumerator.Current;
					foreach (FactionTierConfigOverride factionTierConfigOverride in factionCompetitionConfigOverride.FactionTierConfigs)
					{
						factionWideData.SetCompetitionFactionTierInfo(factionTierConfigOverride.CompetitionId, factionTierConfigOverride.FactionId, factionTierConfigOverride.TierId, factionTierConfigOverride.ContributionToComplete);
					}
				}
			}
		}
	}

	public void SetBaseCharacterConfigs(LobbyGameplayData gameplayData)
	{
		this.CharacterConfigs.Clear();
		using (Dictionary<CharacterType, LobbyCharacterGameplayData>.ValueCollection.Enumerator enumerator = gameplayData.CharacterData.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				LobbyCharacterGameplayData lobbyCharacterGameplayData = enumerator.Current;
				this.CharacterConfigs.Add(lobbyCharacterGameplayData.CharacterType, lobbyCharacterGameplayData.CharacterConfig);
			}
		}
	}

	public void ClearBaseCharacterConfigs()
	{
		this.CharacterConfigs.Clear();
	}

	public IEnumerable<CharacterType> GetCharacterTypes()
	{
		return this.CharacterConfigs.Keys;
	}

	public CharacterConfig GetCharacterConfig(CharacterType characterType)
	{
		CharacterConfig characterConfig = null;
		this.CharacterConfigOverrides.TryGetValue(characterType, out characterConfig);
		if (characterConfig == null)
		{
			this.CharacterConfigs.TryGetValue(characterType, out characterConfig);
		}
		return characterConfig;
	}

	public void SetCharacterConfigOverride(CharacterConfig characterConfigOverride)
	{
		if (this.CharacterConfigOverrides.ContainsKey(characterConfigOverride.CharacterType))
		{
			this.CharacterConfigOverrides[characterConfigOverride.CharacterType] = characterConfigOverride;
		}
		else
		{
			this.CharacterConfigOverrides.Add(characterConfigOverride.CharacterType, characterConfigOverride);
		}
	}

	public bool IsCharacterAllowedForPlayers(CharacterType characterType)
	{
		CharacterConfig characterConfig = this.GetCharacterConfig(characterType);
		if (characterType.IsValidForHumanGameplay() && characterConfig != null)
		{
			if (characterConfig.AllowForPlayers)
			{
				return this.EnableHiddenCharacters || !characterConfig.IsHidden;
			}
		}
		return false;
	}

	public bool IsCharacterAllowedForGameType(CharacterType characterType, GameType gameType, GameSubType gameSubType, IFreelancerSetQueryInterface qi)
	{
		CharacterConfig characterConfig = this.GetCharacterConfig(characterType);
		if (characterConfig != null)
		{
			if (!characterConfig.GameTypesProhibitedFrom.IsNullOrEmpty<GameType>())
			{
				if (characterConfig.GameTypesProhibitedFrom.Contains(gameType))
				{
					return false;
				}
			}
			if (gameSubType != null)
			{
				if (!gameSubType.IsCharacterAllowed(characterType, qi))
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	public bool IsCharacterAllowedForBots(CharacterType characterType)
	{
		CharacterConfig characterConfig = this.GetCharacterConfig(characterType);
		if (characterConfig != null)
		{
			if (characterConfig.AllowForBots)
			{
				return this.EnableHiddenCharacters || !characterConfig.IsHidden;
			}
		}
		return false;
	}

	public bool IsValidForHumanPreGameSelection(CharacterType characterType)
	{
		CharacterConfig characterConfig = this.GetCharacterConfig(characterType);
		if (characterType.IsValidForHumanPreGameSelection())
		{
			if (characterConfig != null && characterConfig.AllowForPlayers)
			{
				return this.EnableHiddenCharacters || !characterConfig.IsHidden;
			}
		}
		return false;
	}

	public bool IsCharacterVisible(CharacterType characterType)
	{
		CharacterConfig characterConfig = this.GetCharacterConfig(characterType);
		bool result;
		if (characterConfig != null)
		{
			result = (this.EnableHiddenCharacters || !characterConfig.IsHidden);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public void SetCardConfigOverride(CardConfigOverride cardConfigOverride)
	{
		if (this.CardConfigOverrides.ContainsKey(cardConfigOverride.CardType))
		{
			this.CardConfigOverrides[cardConfigOverride.CardType] = cardConfigOverride;
		}
		else
		{
			this.CardConfigOverrides.Add(cardConfigOverride.CardType, cardConfigOverride);
		}
	}

	public CardConfigOverride GetCardConfig(CardType cardType)
	{
		CardConfigOverride result = null;
		this.CardConfigOverrides.TryGetValue(cardType, out result);
		return result;
	}

	public bool IsCardAllowed(CardType cardType)
	{
		if (!this.EnableCards)
		{
			return false;
		}
		CardConfigOverride cardConfig = this.GetCardConfig(cardType);
		if (cardConfig == null)
		{
			return true;
		}
		return cardConfig.Allowed;
	}

	public void SetCharacterAbilityConfigOverride(CharacterAbilityConfigOverride characterAbilityConfigOverride)
	{
		if (this.CharacterAbilityConfigOverrides.ContainsKey(characterAbilityConfigOverride.CharacterType))
		{
			this.CharacterAbilityConfigOverrides[characterAbilityConfigOverride.CharacterType] = characterAbilityConfigOverride;
		}
		else
		{
			this.CharacterAbilityConfigOverrides.Add(characterAbilityConfigOverride.CharacterType, characterAbilityConfigOverride);
		}
	}

	public CharacterAbilityConfigOverride GetCharacterAbilityConfigOverride(CharacterType characterType)
	{
		CharacterAbilityConfigOverride result = null;
		this.CharacterAbilityConfigOverrides.TryGetValue(characterType, out result);
		return result;
	}

	public bool AreTauntsEnabled()
	{
		return this.EnableTaunts;
	}

	public bool IsTauntAllowed(CharacterType characterType, int abilityIndex, int tauntId)
	{
		if (abilityIndex < 0)
		{
			return false;
		}
		if (!this.AreTauntsEnabled())
		{
			return false;
		}
		CharacterAbilityConfigOverride characterAbilityConfigOverride = this.GetCharacterAbilityConfigOverride(characterType);
		if (characterAbilityConfigOverride == null)
		{
			return true;
		}
		AbilityConfigOverride abilityConfig = characterAbilityConfigOverride.GetAbilityConfig(abilityIndex);
		if (abilityConfig == null)
		{
			return true;
		}
		AbilityTauntConfigOverride abilityTauntConfig = abilityConfig.GetAbilityTauntConfig(tauntId);
		return abilityTauntConfig == null || abilityTauntConfig.Allowed;
	}

	public bool IsAbilityModAllowed(CharacterType characterType, int abilityIndex, int modIndex)
	{
		if (!this.EnableMods)
		{
			return false;
		}
		CharacterAbilityConfigOverride characterAbilityConfigOverride = this.GetCharacterAbilityConfigOverride(characterType);
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
		this.CharacterSkinConfigOverrides[characterSkinConfigOverride.CharacterType] = characterSkinConfigOverride;
	}

	public CharacterSkinConfigOverride GetCharacterSkinConfigOverride(CharacterType characterType)
	{
		CharacterSkinConfigOverride result = null;
		this.CharacterSkinConfigOverrides.TryGetValue(characterType, out result);
		return result;
	}

	public SkinConfigOverride GetSkinConfigOverride(CharacterType characterType, int skinIndex, int patternIndex, int colorIndex)
	{
		CharacterSkinConfigOverride characterSkinConfigOverride = this.GetCharacterSkinConfigOverride(characterType);
		if (characterSkinConfigOverride == null)
		{
			return null;
		}
		return characterSkinConfigOverride.SkinConfigs.FirstOrDefault(delegate(SkinConfigOverride c)
		{
			if (c.SkinIndex == skinIndex)
			{
				if (c.PatternIndex == patternIndex)
				{
					return c.ColorIndex == colorIndex;
				}
			}
			return false;
		});
	}

	public bool IsColorAllowed(CharacterType characterType, int skinIndex, int patternIndex, int colorIndex)
	{
		if (skinIndex == 0)
		{
			if (patternIndex == 0 && colorIndex == 0)
			{
				return true;
			}
		}
		CharacterSkinConfigOverride characterSkinConfigOverride = this.GetCharacterSkinConfigOverride(characterType);
		if (characterSkinConfigOverride == null)
		{
			return true;
		}
		SkinConfigOverride skinConfigOverride = characterSkinConfigOverride.SkinConfigs.FirstOrDefault(delegate(SkinConfigOverride c)
		{
			if (c.SkinIndex == skinIndex)
			{
				if (c.PatternIndex == patternIndex)
				{
					return c.ColorIndex == colorIndex;
				}
			}
			return false;
		});
		return skinConfigOverride == null || skinConfigOverride.Allowed;
	}

	public QuestConfigOverride GetQuestConfig(int questId)
	{
		QuestConfigOverride result = null;
		this.QuestConfigOverrides.TryGetValue(questId, out result);
		return result;
	}

	public bool IsQuestEnabled(int questId)
	{
		if (!this.EnableQuests)
		{
			return false;
		}
		QuestConfigOverride questConfig = this.GetQuestConfig(questId);
		if (questConfig == null)
		{
			return true;
		}
		return questConfig.Enabled;
	}

	public bool ShouldAbandonQuest(int questId)
	{
		if (!this.EnableQuests)
		{
			return false;
		}
		QuestConfigOverride questConfig = this.GetQuestConfig(questId);
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
			if (other.EnableCards != this.EnableCards)
			{
				List<string> list2 = list;
				string format = "Cards have been {0}";
				object arg;
				if (other.EnableCards)
				{
					arg = "enabled";
				}
				else
				{
					arg = "disabled";
				}
				list2.Add(string.Format(format, arg));
			}
			if (other.EnableMods != this.EnableMods)
			{
				List<string> list3 = list;
				string format2 = "Mods have been {0}";
				object arg2;
				if (other.EnableMods)
				{
					arg2 = "enabled";
				}
				else
				{
					arg2 = "disabled";
				}
				list3.Add(string.Format(format2, arg2));
			}
			if (other.EnableTaunts != this.EnableTaunts)
			{
				List<string> list4 = list;
				string format3 = "Taunts have been {0}";
				object arg3;
				if (other.EnableTaunts)
				{
					arg3 = "enabled";
				}
				else
				{
					arg3 = "disabled";
				}
				list4.Add(string.Format(format3, arg3));
			}
			if (other.EnableAllMods != this.EnableAllMods)
			{
				List<string> list5 = list;
				string format4 = "All mods mode has been {0}";
				object arg4;
				if (other.EnableAllMods)
				{
					arg4 = "enabled";
				}
				else
				{
					arg4 = "disabled";
				}
				list5.Add(string.Format(format4, arg4));
			}
			if (other.EnableShop != this.EnableShop)
			{
				List<string> list6 = list;
				string format5 = "Shop has been {0}";
				object arg5;
				if (other.EnableShop)
				{
					arg5 = "enabled";
				}
				else
				{
					arg5 = "disabled";
				}
				list6.Add(string.Format(format5, arg5));
			}
			if (other.EnableSeasons != this.EnableSeasons)
			{
				List<string> list7 = list;
				string format6 = "Seasons has been {0}";
				object arg6;
				if (other.EnableSeasons)
				{
					arg6 = "enabled";
				}
				else
				{
					arg6 = "disabled";
				}
				list7.Add(string.Format(format6, arg6));
			}
			if (other.EnableQuests != this.EnableQuests)
			{
				list.Add(string.Format("Quests have been {0}", (!other.EnableQuests) ? "disabled" : "enabled"));
			}
			if (other.EventFreePlayerXPBonusPercent != this.EventFreePlayerXPBonusPercent)
			{
				list.Add(string.Format("XP Bonus (Free Player) is now {0} %", other.EventFreePlayerXPBonusPercent));
			}
			if (other.EventPaidPlayerXPBonusPercent != this.EventPaidPlayerXPBonusPercent)
			{
				list.Add(string.Format("XP Bonus (Paid Player) is now {0} %", other.EventPaidPlayerXPBonusPercent));
			}
			if (other.EventISOBonusPercent != this.EventISOBonusPercent)
			{
				list.Add(string.Format("ISO Bonus is now {0} %", other.EventISOBonusPercent));
			}
			if (other.EventGGBoostBonusPercent != this.EventGGBoostBonusPercent)
			{
				list.Add(string.Format("GG Boost Bonus is now {0} %", other.EventGGBoostBonusPercent));
			}
			if (other.EventTrustInfluenceBonusPercent != this.EventTrustInfluenceBonusPercent)
			{
				list.Add(string.Format("Trust Influence Bonus is now {0} %", other.EventTrustInfluenceBonusPercent));
			}
			if (other.EventFreelancerCurrencyPerMatchBonusPercent != this.EventFreelancerCurrencyPerMatchBonusPercent)
			{
				list.Add(string.Format("Freelancer Currency Bonus is now {0} %", other.EventFreelancerCurrencyPerMatchBonusPercent));
			}
			foreach (GameType gameType in other.DisabledGameTypes.Union(this.DisabledGameTypes).Distinct<GameType>())
			{
				bool flag;
				if (!this.DisabledGameTypes.IsNullOrEmpty<GameType>())
				{
					flag = this.DisabledGameTypes.Contains(gameType);
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				bool flag3;
				if (!other.DisabledGameTypes.IsNullOrEmpty<GameType>())
				{
					flag3 = other.DisabledGameTypes.Contains(gameType);
				}
				else
				{
					flag3 = false;
				}
				bool flag4 = flag3;
				if (flag2 != flag4)
				{
					list.Add(string.Format("{0} game type {1}", (!flag4) ? "Removing admin lock from" : "Adding admin lock on", gameType));
				}
			}
			IEnumerator<string> enumerator2 = other.DisabledMaps.Union(this.DisabledMaps).Distinct<string>().GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					string text = enumerator2.Current;
					bool flag5 = !this.DisabledMaps.IsNullOrEmpty<string>() && this.DisabledMaps.Contains(text);
					bool flag6;
					if (!other.DisabledMaps.IsNullOrEmpty<string>())
					{
						flag6 = other.DisabledMaps.Contains(text);
					}
					else
					{
						flag6 = false;
					}
					bool flag7 = flag6;
					if (flag5 != flag7)
					{
						List<string> list8 = list;
						string format7 = "{0} map {1}";
						object arg7;
						if (flag7)
						{
							arg7 = "Adding admin lock on";
						}
						else
						{
							arg7 = "Removing admin lock from";
						}
						list8.Add(string.Format(format7, arg7, text));
					}
				}
			}
			finally
			{
				if (enumerator2 != null)
				{
					enumerator2.Dispose();
				}
			}
			using (Dictionary<CharacterType, CharacterConfig>.ValueCollection.Enumerator enumerator3 = other.CharacterConfigOverrides.Values.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					CharacterConfig characterConfig = enumerator3.Current;
					if (characterConfig.CharacterRole == CharacterRole.None && characterConfig.CharacterType.IsValidForHumanGameplay())
					{
						Log.Error("Why is the console marking {0} as being role NONE?! That's going to mess up Free Rotation!", new object[]
						{
							characterConfig.CharacterType.GetDisplayName()
						});
					}
					CharacterConfig characterConfig2 = this.CharacterConfigOverrides.TryGetValue(characterConfig.CharacterType);
					if (characterConfig2 != null)
					{
						if (characterConfig2.AllowForBots != characterConfig.AllowForBots)
						{
							goto IL_59E;
						}
						if (characterConfig2.AllowForPlayers != characterConfig.AllowForPlayers)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								goto IL_59E;
							}
						}
						goto IL_65C;
						IL_59E:
						List<string> list9 = list;
						string format8 = "Overriding character {0} to be {1} for players and {2} for bots";
						object arg8 = characterConfig.CharacterType.ToString();
						object arg9 = (!characterConfig.AllowForPlayers) ? "not allowed" : "allowed";
						object arg10;
						if (characterConfig.AllowForBots)
						{
							arg10 = "allowed";
						}
						else
						{
							arg10 = "not allowed";
						}
						list9.Add(string.Format(format8, arg8, arg9, arg10));
					}
					else
					{
						List<string> list10 = list;
						string format9 = "Adding overriding character {0} to be {1} for players and {2} for bots";
						object arg11 = characterConfig.CharacterType.ToString();
						object arg12;
						if (characterConfig.AllowForPlayers)
						{
							arg12 = "allowed";
						}
						else
						{
							arg12 = "not allowed";
						}
						object arg13;
						if (characterConfig.AllowForBots)
						{
							arg13 = "allowed";
						}
						else
						{
							arg13 = "not allowed";
						}
						list10.Add(string.Format(format9, arg11, arg12, arg13));
					}
					IL_65C:
					IEnumerator enumerator4 = Enum.GetValues(typeof(GameType)).GetEnumerator();
					try
					{
						while (enumerator4.MoveNext())
						{
							object obj = enumerator4.Current;
							GameType gameType2 = (GameType)obj;
							bool flag8;
							if (characterConfig2 != null && !characterConfig2.GameTypesProhibitedFrom.IsNullOrEmpty<GameType>())
							{
								flag8 = characterConfig2.GameTypesProhibitedFrom.Contains(gameType2);
							}
							else
							{
								flag8 = false;
							}
							bool flag9 = flag8;
							bool flag10 = !characterConfig.GameTypesProhibitedFrom.IsNullOrEmpty<GameType>() && characterConfig.GameTypesProhibitedFrom.Contains(gameType2);
							if (flag9 != flag10)
							{
								list.Add(string.Format("Overriding character {0} to be {1} in {2} matches", characterConfig.CharacterType, (!flag10) ? "allowed" : "not allowed", gameType2));
							}
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator4 as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
			}
			using (Dictionary<CharacterType, CharacterConfig>.ValueCollection.Enumerator enumerator5 = this.CharacterConfigOverrides.Values.GetEnumerator())
			{
				while (enumerator5.MoveNext())
				{
					CharacterConfig characterConfig3 = enumerator5.Current;
					if (!other.CharacterConfigOverrides.ContainsKey(characterConfig3.CharacterType))
					{
						list.Add(string.Format("Removing overrides for character {0}", characterConfig3.CharacterType.ToString()));
					}
				}
			}
			using (Dictionary<CardType, CardConfigOverride>.ValueCollection.Enumerator enumerator6 = other.CardConfigOverrides.Values.GetEnumerator())
			{
				while (enumerator6.MoveNext())
				{
					CardConfigOverride cardConfigOverride = enumerator6.Current;
					CardConfigOverride cardConfigOverride2 = this.CardConfigOverrides.TryGetValue(cardConfigOverride.CardType);
					if (cardConfigOverride2 != null)
					{
						if (cardConfigOverride2.Allowed == cardConfigOverride.Allowed)
						{
							continue;
						}
					}
					List<string> list11 = list;
					string format10 = "Overriding catalyst {0} to be {1}";
					object arg14 = cardConfigOverride.CardType.ToString();
					object arg15;
					if (cardConfigOverride.Allowed)
					{
						arg15 = "allowed";
					}
					else
					{
						arg15 = "not allowed";
					}
					list11.Add(string.Format(format10, arg14, arg15));
				}
			}
			using (Dictionary<CardType, CardConfigOverride>.ValueCollection.Enumerator enumerator7 = this.CardConfigOverrides.Values.GetEnumerator())
			{
				while (enumerator7.MoveNext())
				{
					CardConfigOverride cardConfigOverride3 = enumerator7.Current;
					if (!other.CardConfigOverrides.ContainsKey(cardConfigOverride3.CardType))
					{
						list.Add(string.Format("Removing overrides for catalyst {0}", cardConfigOverride3.CardType.ToString()));
					}
				}
			}
			if (LobbyGameplayData.Get() != null)
			{
				using (Dictionary<CharacterType, CharacterAbilityConfigOverride>.ValueCollection.Enumerator enumerator8 = other.CharacterAbilityConfigOverrides.Values.GetEnumerator())
				{
					while (enumerator8.MoveNext())
					{
						CharacterAbilityConfigOverride characterAbilityConfigOverride = enumerator8.Current;
						if (characterAbilityConfigOverride == null)
						{
						}
						else
						{
							LobbyCharacterGameplayData characterData = LobbyGameplayData.Get().GetCharacterData(characterAbilityConfigOverride.CharacterType);
							CharacterAbilityConfigOverride characterAbilityConfigOverride2 = this.CharacterAbilityConfigOverrides.TryGetValue(characterAbilityConfigOverride.CharacterType);
							for (int i = 0; i < characterAbilityConfigOverride.AbilityConfigs.Length; i++)
							{
								AbilityConfigOverride abilityConfigOverride = characterAbilityConfigOverride.AbilityConfigs[i];
								if (abilityConfigOverride == null)
								{
								}
								else
								{
									AbilityConfigOverride abilityConfigOverride2 = null;
									if (characterAbilityConfigOverride2 != null && characterAbilityConfigOverride2.AbilityConfigs != null)
									{
										abilityConfigOverride2 = characterAbilityConfigOverride2.AbilityConfigs[i];
									}
									LobbyAbilityGameplayData abilityData = characterData.GetAbilityData(i);
									if (abilityConfigOverride.AbilityModConfigs != null)
									{
										using (Dictionary<int, AbilityModConfigOverride>.ValueCollection.Enumerator enumerator9 = abilityConfigOverride.AbilityModConfigs.Values.GetEnumerator())
										{
											while (enumerator9.MoveNext())
											{
												AbilityModConfigOverride abilityModConfigOverride = enumerator9.Current;
												AbilityModConfigOverride abilityModConfigOverride2 = null;
												if (abilityConfigOverride2 != null)
												{
													if (abilityConfigOverride2.AbilityModConfigs != null)
													{
														abilityModConfigOverride2 = abilityConfigOverride2.AbilityModConfigs.TryGetValue(abilityModConfigOverride.AbilityModIndex);
													}
												}
												LobbyAbilityModGameplayData abilityModData = abilityData.GetAbilityModData(abilityModConfigOverride.AbilityModIndex);
												if (abilityModConfigOverride2 != null)
												{
													if (abilityModConfigOverride2.Allowed == abilityModConfigOverride.Allowed)
													{
														continue;
													}
												}
												List<string> list12 = list;
												string format11 = "Overriding {0} ability '{1}' mod '{2}' to be {3}";
												object[] array = new object[4];
												array[0] = characterAbilityConfigOverride.CharacterType.ToString();
												array[1] = abilityData.Name;
												array[2] = abilityModData.Name;
												int num = 3;
												object obj2;
												if (abilityModConfigOverride.Allowed)
												{
													obj2 = "allowed";
												}
												else
												{
													obj2 = "not allowed";
												}
												array[num] = obj2;
												list12.Add(string.Format(format11, array));
											}
										}
									}
									if (abilityConfigOverride.AbilityTauntConfigs != null)
									{
										using (Dictionary<int, AbilityTauntConfigOverride>.ValueCollection.Enumerator enumerator10 = abilityConfigOverride.AbilityTauntConfigs.Values.GetEnumerator())
										{
											while (enumerator10.MoveNext())
											{
												AbilityTauntConfigOverride abilityTauntConfigOverride = enumerator10.Current;
												AbilityTauntConfigOverride abilityTauntConfigOverride2 = null;
												if (abilityConfigOverride2 != null && abilityConfigOverride2.AbilityTauntConfigs != null)
												{
													abilityTauntConfigOverride2 = abilityConfigOverride2.AbilityTauntConfigs.TryGetValue(abilityTauntConfigOverride.AbilityTauntIndex);
												}
												LobbyAbilityTauntData abilityTauntData = abilityData.GetAbilityTauntData(abilityTauntConfigOverride.AbilityTauntID);
												if (abilityTauntConfigOverride2 != null)
												{
													if (abilityTauntConfigOverride2.Allowed == abilityTauntConfigOverride.Allowed)
													{
														continue;
													}
												}
												List<string> list13 = list;
												string format12 = "Overriding {0} ability '{1}' taunt '{2}' to be {3}";
												object[] array2 = new object[4];
												array2[0] = characterAbilityConfigOverride.CharacterType.ToString();
												array2[1] = abilityData.Name;
												array2[2] = abilityTauntData.Name;
												int num2 = 3;
												object obj3;
												if (abilityTauntConfigOverride.Allowed)
												{
													obj3 = "allowed";
												}
												else
												{
													obj3 = "not allowed";
												}
												array2[num2] = obj3;
												list13.Add(string.Format(format12, array2));
											}
										}
									}
								}
							}
						}
					}
				}
				using (Dictionary<CharacterType, CharacterAbilityConfigOverride>.ValueCollection.Enumerator enumerator11 = this.CharacterAbilityConfigOverrides.Values.GetEnumerator())
				{
					while (enumerator11.MoveNext())
					{
						CharacterAbilityConfigOverride characterAbilityConfigOverride3 = enumerator11.Current;
						CharacterAbilityConfigOverride characterAbilityConfigOverride4 = other.CharacterAbilityConfigOverrides.TryGetValue(characterAbilityConfigOverride3.CharacterType);
						if (characterAbilityConfigOverride4 == null)
						{
							list.Add(string.Format("Removing all mod and taunt overrides for {0}", characterAbilityConfigOverride3.CharacterType));
						}
						else
						{
							LobbyCharacterGameplayData characterData2 = LobbyGameplayData.Get().GetCharacterData(characterAbilityConfigOverride4.CharacterType);
							for (int j = 0; j < characterAbilityConfigOverride3.AbilityConfigs.Length; j++)
							{
								AbilityConfigOverride abilityConfigOverride3 = characterAbilityConfigOverride3.AbilityConfigs[j];
								if (abilityConfigOverride3 == null)
								{
								}
								else
								{
									AbilityConfigOverride abilityConfigOverride4 = characterAbilityConfigOverride4.AbilityConfigs[j];
									LobbyAbilityGameplayData abilityData2 = characterData2.GetAbilityData(j);
									if (abilityConfigOverride3.AbilityModConfigs != null)
									{
										using (Dictionary<int, AbilityModConfigOverride>.ValueCollection.Enumerator enumerator12 = abilityConfigOverride3.AbilityModConfigs.Values.GetEnumerator())
										{
											while (enumerator12.MoveNext())
											{
												AbilityModConfigOverride abilityModConfigOverride3 = enumerator12.Current;
												if (abilityConfigOverride4.AbilityModConfigs.TryGetValue(abilityModConfigOverride3.AbilityModIndex) == null)
												{
													LobbyAbilityModGameplayData abilityModData2 = abilityData2.GetAbilityModData(abilityModConfigOverride3.AbilityModIndex);
													list.Add(string.Format("Removing override for {0} ability '{1}' mod '{2}'", characterAbilityConfigOverride4.CharacterType.ToString(), abilityData2.Name, abilityModData2.Name));
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
												AbilityTauntConfigOverride abilityTauntConfigOverride3 = enumerator13.Current;
												if (abilityConfigOverride4.AbilityTauntConfigs.TryGetValue(abilityTauntConfigOverride3.AbilityTauntIndex) == null)
												{
													LobbyAbilityTauntData abilityTauntData2 = abilityData2.GetAbilityTauntData(abilityTauntConfigOverride3.AbilityTauntID);
													list.Add(string.Format("Removing override for {0} ability '{1}' taunt '{2}'", characterAbilityConfigOverride4.CharacterType.ToString(), abilityData2.Name, abilityTauntData2.Name));
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
						FactionCompetitionConfigOverride factionCompetitionConfigOverride = enumerator14.Current;
						using (List<FactionTierConfigOverride>.Enumerator enumerator15 = factionCompetitionConfigOverride.FactionTierConfigs.GetEnumerator())
						{
							while (enumerator15.MoveNext())
							{
								FactionTierConfigOverride otherFactionTierConfigOverride = enumerator15.Current;
								FactionCompetitionConfigOverride factionCompetitionConfigOverride2 = this.FactionCompetitionConfigOverrides.TryGetValue(factionCompetitionConfigOverride.Index);
								FactionTierConfigOverride factionTierConfigOverride = (factionCompetitionConfigOverride2 == null) ? null : factionCompetitionConfigOverride2.FactionTierConfigs.Find(delegate(FactionTierConfigOverride o)
								{
									if (o.CompetitionId == otherFactionTierConfigOverride.CompetitionId)
									{
										if (o.FactionId == otherFactionTierConfigOverride.FactionId)
										{
											return o.TierId == otherFactionTierConfigOverride.TierId;
										}
									}
									return false;
								});
								if (factionTierConfigOverride != null)
								{
									if (otherFactionTierConfigOverride.ContributionToComplete == factionTierConfigOverride.ContributionToComplete)
									{
										continue;
									}
								}
								list.Add(string.Format("Overriding faction(competitionId={0} factionId={1} tierId={2}) ContributionToComplete to be {3}", new object[]
								{
									otherFactionTierConfigOverride.CompetitionId,
									otherFactionTierConfigOverride.FactionId,
									otherFactionTierConfigOverride.TierId,
									otherFactionTierConfigOverride.ContributionToComplete
								}));
							}
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Exception(ex);
			list.Add(string.Format("LobbyGameplayOverrides.GetDifferences failed: {0}", ex.Message));
		}
		return list;
	}
}
