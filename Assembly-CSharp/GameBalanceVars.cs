using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameBalanceVars
{
	public const int NUM_GG_PACKS_ALLOWED_PER_GAME_PER_USER = 3;

	public int ResetSelectionVersion;

	public float GGPackEndGameUsageTimer = 15f;

	public float GGPackInGameCooldownTimer = 2f;

	public float GGPackXPBonusPerPack;

	public float GGPackFreelancerCurrencyMultPerPack;

	public float GGPackSelfXPBonus;

	public float GGPackSelfFreelancerCurrencyMult;

	public int[] GGPackISOAdditionalBonus;

	public int[] GGPackFreelancerCurrencyAdditionalBonus;

	public float[] GGPackXPAdditionalBonus;

	public float[] GGPackFreelancerCurrencyAdditionalMult;

	public int GGPackISOBonusPerPack;

	public int GGPackFreelancerCurrencyBonusPerPack;

	public int GGPackSelfISOBonus;

	public int GGPackSelfFreelancerCurrencyBonus;

	public float PlayWithFriendXPBonusMult;

	public float XPMultForOwnedFreelancers;

	public int m_firstLevelXpAwarded;

	public int m_secondLevelXpAwarded;

	public int FirstWinOfDayQuestId;

	public int IsoRewardedPerFreelancerOnGamePurchase = 0x3E8;

	public int MaxNumberOfGlobalLoadoutSlots = 0xA;

	public int MaxNumberOfFreelancerLoadoutSlots = 0xA;

	public int GlobalLoadoutSlotFluxCost = 0x2710;

	public int FreelancerLoadoutSlotFluxCost = 0x3E8;

	public GameBalanceVars.LevelProgressInfo[] PlayerProgressInfo;

	public GameBalanceVars.LevelProgressInfo[] CharacterProgressInfo;

	public GameBalanceVars.LevelProgressInfo RepeatingCharacterProgressInfo;

	public RewardUtils.RewardType[] RewardDisplayPriorityOrder;

	public GameBalanceVars.GameRewardBucket[] GameRewardBuckets;

	private GameBalanceVars.LevelProgressInfo[] m_CharProgressInfoWithRepeating;

	public GameBalanceVars.CharacterLevelReward[] RepeatingCharacterLevelRewards;

	public GameBalanceVars.TitleLevelDefinition[] TitleLevelDefinitions;

	public GameBalanceVars.PlayerTitle[] PlayerTitles;

	public GameBalanceVars.PlayerBanner[] PlayerBanners;

	public GameBalanceVars.PlayerRibbon[] PlayerRibbons;

	public GameBalanceVars.ChatEmoticon[] ChatEmojis;

	public GameBalanceVars.OverconUnlockData[] Overcons;

	public GameBalanceVars.StoreItemForPurchase[] StoreItemsForPurchase;

	public GameBalanceVars.UnlockExlusivePool[] ExclusivityPools;

	public GameBalanceVars.GameResultBadge[] GameResultBadges;

	public GameBalanceVars.LobbyStatSettings[] StatSettings;

	public GameBalanceVars.LoadingScreenBackground[] LoadingScreenBackgrounds;

	public int NumSecsToOpenLootMatrix;

	public GameBalanceVars.CharacterUnlockData[] characterUnlockData;

	public GameBalanceVars.RAFReward[] RAFRewards;

	public int FreelancerCurrencyPerModToken;

	public bool UseModEquipCostAsModUnlockCost;

	public int FreelancerCurrencyToUnlockMod;

	public GameBalanceVars.RankedTierLockedItems[] RankedTierLockedItemsList;

	public static GameBalanceVars Get()
	{
		return GameWideData.Get().m_gameBalanceVars;
	}

	public GameBalanceVars.LevelProgressInfo[] CharProgressInfoWithRepeating
	{
		get
		{
			if (this.m_CharProgressInfoWithRepeating != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.get_CharProgressInfoWithRepeating()).MethodHandle;
				}
				if (this.m_CharProgressInfoWithRepeating.Length == this.CharacterProgressInfo.Length + 1)
				{
					goto IL_90;
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			this.m_CharProgressInfoWithRepeating = new GameBalanceVars.LevelProgressInfo[this.CharacterProgressInfo.Length + 1];
			for (int i = 0; i < this.CharacterProgressInfo.Length; i++)
			{
				this.m_CharProgressInfoWithRepeating[i] = this.CharacterProgressInfo[i];
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_CharProgressInfoWithRepeating[this.CharacterProgressInfo.Length] = this.RepeatingCharacterProgressInfo;
			IL_90:
			return this.m_CharProgressInfoWithRepeating;
		}
	}

	public bool IsStatLowerBetter(StatDisplaySettings.StatType StatType)
	{
		for (int i = 0; i < this.StatSettings.Length; i++)
		{
			if (this.StatSettings[i].m_StatType == StatType)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.IsStatLowerBetter(StatDisplaySettings.StatType)).MethodHandle;
				}
				return this.StatSettings[i].LowerIsBetter;
			}
		}
		return false;
	}

	public int MaxPlayerLevel
	{
		get
		{
			return 0;
		}
	}

	public int CharacterSilverLevel
	{
		get
		{
			return 0xA;
		}
	}

	public int CharacterMasteryLevel
	{
		get
		{
			return 0x14;
		}
	}

	public int CharacterPurpleLevel
	{
		get
		{
			return 0x28;
		}
	}

	public int CharacterRedLevel
	{
		get
		{
			return 0x3C;
		}
	}

	public int CharacterDiamondLevel
	{
		get
		{
			return 0x50;
		}
	}

	public int CharacterRainbowLevel
	{
		get
		{
			return 0x64;
		}
	}

	public int MaxCharacterLevelForRewards
	{
		get
		{
			if (this.CharacterProgressInfo == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.get_MaxCharacterLevelForRewards()).MethodHandle;
				}
				return 0;
			}
			return this.CharacterProgressInfo.Length + 1;
		}
	}

	public int MaxCharacterLevel
	{
		get
		{
			return 0x7FFF;
		}
	}

	public int GetGGPackBonusISO(int numPacksUsed, int selfUsedPack)
	{
		int num = 0;
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		num += gameBalanceVars.GGPackISOBonusPerPack * numPacksUsed;
		num += gameBalanceVars.GGPackSelfISOBonus * selfUsedPack;
		if (0 < numPacksUsed)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetGGPackBonusISO(int, int)).MethodHandle;
			}
			if (numPacksUsed - 1 < gameBalanceVars.GGPackISOAdditionalBonus.Length)
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
				num += gameBalanceVars.GGPackISOAdditionalBonus[numPacksUsed - 1];
			}
		}
		return num;
	}

	public float GetGGPackXPMultiplier(int numPacksUsed, int selfUsedPack)
	{
		float num = 1f;
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		num += gameBalanceVars.GGPackXPBonusPerPack * (float)numPacksUsed;
		num += gameBalanceVars.GGPackSelfXPBonus * (float)selfUsedPack;
		for (int i = 0; i < numPacksUsed; i++)
		{
			if (numPacksUsed - 1 < gameBalanceVars.GGPackXPAdditionalBonus.Length)
			{
				num += gameBalanceVars.GGPackXPAdditionalBonus[i];
			}
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetGGPackXPMultiplier(int, int)).MethodHandle;
		}
		return (float)Math.Round((double)num, 1);
	}

	public GameBalanceVars.CharacterUnlockData GetCharacterUnlockData(CharacterType character)
	{
		if (this.characterUnlockData != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetCharacterUnlockData(CharacterType)).MethodHandle;
			}
			foreach (GameBalanceVars.CharacterUnlockData characterUnlockData in this.characterUnlockData)
			{
				if (characterUnlockData.character == character)
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
					return characterUnlockData;
				}
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return null;
	}

	public int PlayerExperienceToLevel(int currentLevel)
	{
		if (currentLevel >= 1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.PlayerExperienceToLevel(int)).MethodHandle;
			}
			if (currentLevel <= this.PlayerProgressInfo.Length)
			{
				return this.PlayerProgressInfo[currentLevel - 1].ExperienceToNextLevel;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		throw new ArgumentException(string.Format("Current level {0} is outside the player level range {1}-{2}", currentLevel, 1, this.PlayerProgressInfo.Length));
	}

	public int CharacterExperienceToLevel(int currentLevel)
	{
		if (currentLevel < 1)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.CharacterExperienceToLevel(int)).MethodHandle;
			}
			throw new ArgumentException(string.Format("Attempting to access character experience less than 1", new object[0]));
		}
		if (currentLevel - 1 < this.CharacterProgressInfo.Length)
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
			return this.CharacterProgressInfo[currentLevel - 1].ExperienceToNextLevel;
		}
		return this.RepeatingCharacterProgressInfo.ExperienceToNextLevel;
	}

	public string GetTitle(int titleID, string returnOnEmptyOverride = "", int titleLevel = -1)
	{
		for (int i = 0; i < this.PlayerTitles.Length; i++)
		{
			if (this.PlayerTitles[i].ID == titleID)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetTitle(int, string, int)).MethodHandle;
				}
				return this.PlayerTitles[i].GetTitleText(titleLevel);
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		return returnOnEmptyOverride;
	}

	public int GetMaxTitleLevel(int titleID)
	{
		for (int i = 0; i < this.TitleLevelDefinitions.Length; i++)
		{
			if (this.TitleLevelDefinitions[i].m_titleId == titleID)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetMaxTitleLevel(int)).MethodHandle;
				}
				return this.TitleLevelDefinitions[i].m_maxLevel;
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
		return 0;
	}

	public int GetChatEmojiIDByName(string emojiName)
	{
		for (int i = 0; i < this.ChatEmojis.Length; i++)
		{
			if (this.ChatEmojis[i].Name == emojiName)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetChatEmojiIDByName(string)).MethodHandle;
				}
				return this.ChatEmojis[i].ID;
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		return -1;
	}

	public int GetChatEmojiIndexByName(string emojiName)
	{
		for (int i = 0; i < this.ChatEmojis.Length; i++)
		{
			if (this.ChatEmojis[i].Name == emojiName)
			{
				return i;
			}
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetChatEmojiIndexByName(string)).MethodHandle;
		}
		return -1;
	}

	public GameBalanceVars.GameResultBadge GetGameBadge(int badgeID)
	{
		if (GameResultBadgeData.Get() != null)
		{
			for (int i = 0; i < GameResultBadgeData.Get().GameResultBadges.Length; i++)
			{
				if (GameResultBadgeData.Get().GameResultBadges[i].UniqueBadgeID == badgeID)
				{
					return GameResultBadgeData.Get().GameResultBadges[i];
				}
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetGameBadge(int)).MethodHandle;
			}
		}
		for (int j = 0; j < this.GameResultBadges.Length; j++)
		{
			if (this.GameResultBadges[j].UniqueBadgeID == badgeID)
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
				return this.GameResultBadges[j];
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
		return null;
	}

	public string GetUnlocalizedChatEmojiName(int emojiID, string returnOnEmptyOverride = "")
	{
		for (int i = 0; i < this.ChatEmojis.Length; i++)
		{
			if (this.ChatEmojis[i].ID == emojiID)
			{
				return this.ChatEmojis[i].Name;
			}
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetUnlocalizedChatEmojiName(int, string)).MethodHandle;
		}
		return returnOnEmptyOverride;
	}

	public GameBalanceVars.ChatEmoticon GetChatEmoji(int emojiID)
	{
		for (int i = 0; i < this.ChatEmojis.Length; i++)
		{
			if (this.ChatEmojis[i].ID == emojiID)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetChatEmoji(int)).MethodHandle;
				}
				return this.ChatEmojis[i];
			}
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
		return null;
	}

	public string GetBannerName(int bannerID, string returnOnEmptyOverride = "")
	{
		GameBalanceVars.PlayerBanner banner = this.GetBanner(bannerID);
		string result;
		if (banner != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetBannerName(int, string)).MethodHandle;
			}
			result = banner.GetBannerName();
		}
		else
		{
			result = returnOnEmptyOverride;
		}
		return result;
	}

	public GameBalanceVars.PlayerBanner GetBanner(int bannerID)
	{
		for (int i = 0; i < this.PlayerBanners.Length; i++)
		{
			if (this.PlayerBanners[i].ID == bannerID)
			{
				return this.PlayerBanners[i];
			}
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetBanner(int)).MethodHandle;
		}
		return null;
	}

	public GameBalanceVars.PlayerRibbon GetRibbon(int ribbonID)
	{
		for (int i = 0; i < this.PlayerRibbons.Length; i++)
		{
			if (this.PlayerRibbons[i].ID == ribbonID)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetRibbon(int)).MethodHandle;
				}
				return this.PlayerRibbons[i];
			}
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		return null;
	}

	public GameBalanceVars.LoadingScreenBackground GetLoadingScreenBackground(int loadingScreenID)
	{
		for (int i = 0; i < this.LoadingScreenBackgrounds.Length; i++)
		{
			if (this.LoadingScreenBackgrounds[i].ID == loadingScreenID)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetLoadingScreenBackground(int)).MethodHandle;
				}
				return this.LoadingScreenBackgrounds[i];
			}
		}
		return null;
	}

	public GameBalanceVars.SkinUnlockData GetSkinUnlockData(CharacterType characterType, int skinIndex)
	{
		GameBalanceVars.CharacterUnlockData characterUnlockData = this.GetCharacterUnlockData(characterType);
		return characterUnlockData.skinUnlockData[skinIndex];
	}

	public GameBalanceVars.PatternUnlockData GetPatternUnlockData(CharacterType characterType, int skinIndex, int patternIndex)
	{
		GameBalanceVars.CharacterUnlockData characterUnlockData = this.GetCharacterUnlockData(characterType);
		return characterUnlockData.skinUnlockData[skinIndex].patternUnlockData[patternIndex];
	}

	public GameBalanceVars.ColorUnlockData GetColorUnlockData(CharacterType characterType, int skinIndex, int patternIndex, int colorIndex)
	{
		GameBalanceVars.CharacterUnlockData characterUnlockData = this.GetCharacterUnlockData(characterType);
		return characterUnlockData.skinUnlockData[skinIndex].patternUnlockData[patternIndex].colorUnlockData[colorIndex];
	}

	public string GetSkinName(CharacterType characterType, int skinIndex)
	{
		GameBalanceVars.CharacterUnlockData characterUnlockData = this.GetCharacterUnlockData(characterType);
		return characterUnlockData.skinUnlockData[skinIndex].Name;
	}

	public string GetPatternName(CharacterType characterType, int skinIndex, int patternIndex)
	{
		GameBalanceVars.CharacterUnlockData characterUnlockData = this.GetCharacterUnlockData(characterType);
		return characterUnlockData.skinUnlockData[skinIndex].patternUnlockData[patternIndex].Name;
	}

	public string GetColorName(CharacterType characterType, int skinIndex, int patternIndex, int colorIndex)
	{
		GameBalanceVars.CharacterUnlockData characterUnlockData = this.GetCharacterUnlockData(characterType);
		return characterUnlockData.skinUnlockData[skinIndex].patternUnlockData[patternIndex].colorUnlockData[colorIndex].Name;
	}

	public int GetXPBonusForWin(GameBalanceVars.GameRewardBucketType bucketType)
	{
		if (bucketType >= GameBalanceVars.GameRewardBucketType.FullVsHumanRewards)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetXPBonusForWin(GameBalanceVars.GameRewardBucketType)).MethodHandle;
			}
			if (bucketType < (GameBalanceVars.GameRewardBucketType)this.GameRewardBuckets.Length)
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
				return this.GameRewardBuckets[(int)bucketType].XPBonusForWin;
			}
		}
		return 0;
	}

	public float GetXPBonusPerMinute(GameBalanceVars.GameRewardBucketType bucketType)
	{
		if (bucketType >= GameBalanceVars.GameRewardBucketType.FullVsHumanRewards)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetXPBonusPerMinute(GameBalanceVars.GameRewardBucketType)).MethodHandle;
			}
			if (bucketType < (GameBalanceVars.GameRewardBucketType)this.GameRewardBuckets.Length)
			{
				return this.GameRewardBuckets[(int)bucketType].XPBonusPerMinute;
			}
		}
		return 0f;
	}

	public float GetXPBonusPerMinuteCap(GameBalanceVars.GameRewardBucketType bucketType)
	{
		if (bucketType >= GameBalanceVars.GameRewardBucketType.FullVsHumanRewards)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetXPBonusPerMinuteCap(GameBalanceVars.GameRewardBucketType)).MethodHandle;
			}
			if (bucketType < (GameBalanceVars.GameRewardBucketType)this.GameRewardBuckets.Length)
			{
				return this.GameRewardBuckets[(int)bucketType].XPBonusPerMinuteCap;
			}
		}
		return 0f;
	}

	public float GetXPBonusPerQueueTimeMinute(GameBalanceVars.GameRewardBucketType bucketType)
	{
		if (bucketType >= GameBalanceVars.GameRewardBucketType.FullVsHumanRewards)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetXPBonusPerQueueTimeMinute(GameBalanceVars.GameRewardBucketType)).MethodHandle;
			}
			if (bucketType < (GameBalanceVars.GameRewardBucketType)this.GameRewardBuckets.Length)
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
				return this.GameRewardBuckets[(int)bucketType].XPBonusPerQueueTimeMinute;
			}
		}
		return 0f;
	}

	public float GetXPBonusPerQueueTimeMinuteCap(GameBalanceVars.GameRewardBucketType bucketType)
	{
		if (bucketType >= GameBalanceVars.GameRewardBucketType.FullVsHumanRewards)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetXPBonusPerQueueTimeMinuteCap(GameBalanceVars.GameRewardBucketType)).MethodHandle;
			}
			if (bucketType < (GameBalanceVars.GameRewardBucketType)this.GameRewardBuckets.Length)
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
				return this.GameRewardBuckets[(int)bucketType].XPBonusPerQueueTimeMinuteCap;
			}
		}
		return 0f;
	}

	public int GetXPBonusForMatchTime(GameBalanceVars.GameRewardBucketType bucketType, TimeSpan matchDuration)
	{
		return this.GetXPBonusForMatchTime(bucketType, (float)matchDuration.TotalMinutes);
	}

	public int GetXPBonusForMatchTime(GameBalanceVars.GameRewardBucketType bucketType, float totalMinutes)
	{
		if (totalMinutes < 0f)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetXPBonusForMatchTime(GameBalanceVars.GameRewardBucketType, float)).MethodHandle;
			}
			totalMinutes = 0f;
		}
		float num = this.GetXPBonusPerMinute(bucketType) * totalMinutes;
		num = Math.Min(num, this.GetXPBonusPerMinuteCap(bucketType));
		return (int)num;
	}

	public int GetXPBonusForQueueTime(GameBalanceVars.GameRewardBucketType bucketType, TimeSpan matchDuration)
	{
		return this.GetXPBonusForQueueTime(bucketType, (float)matchDuration.TotalMinutes);
	}

	public int GetXPBonusForQueueTime(GameBalanceVars.GameRewardBucketType bucketType, float totalMinutes)
	{
		if (totalMinutes < 0f)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GetXPBonusForQueueTime(GameBalanceVars.GameRewardBucketType, float)).MethodHandle;
			}
			totalMinutes = 0f;
		}
		float num = this.GetXPBonusPerQueueTimeMinute(bucketType) * totalMinutes;
		num = Math.Min(num, this.GetXPBonusPerQueueTimeMinuteCap(bucketType));
		return (int)num;
	}

	public void OnValidate()
	{
		GameBalanceVars.EnsureUniqueIDs<GameBalanceVars.PlayerTitle>(this.PlayerTitles);
		GameBalanceVars.EnsureUniqueIDs<GameBalanceVars.PlayerBanner>(this.PlayerBanners);
		GameBalanceVars.EnsureUniqueIDs<GameBalanceVars.PlayerRibbon>(this.PlayerRibbons);
		GameBalanceVars.EnsureUniqueIDs<GameBalanceVars.ChatEmoticon>(this.ChatEmojis);
		GameBalanceVars.EnsureUniqueIDs<GameBalanceVars.OverconUnlockData>(this.Overcons);
		GameBalanceVars.EnsureUniqueIDs<GameBalanceVars.StoreItemForPurchase>(this.StoreItemsForPurchase);
	}

	public static void EnsureUniqueIDs<T>(T[] arrayOfIDs) where T : GameBalanceVars.IUniqueID
	{
		if (arrayOfIDs != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.EnsureUniqueIDs(T[])).MethodHandle;
			}
			if (arrayOfIDs.Length > 0)
			{
				int num = 0;
				for (int i = 0; i < arrayOfIDs.Length; i++)
				{
					if (arrayOfIDs[i].GetID() > num)
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
						num = arrayOfIDs[i].GetID();
					}
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
				for (int j = 0; j < arrayOfIDs.Length; j++)
				{
					if (arrayOfIDs[j].GetID() == 0)
					{
						arrayOfIDs[j].SetID(++num);
					}
					for (int k = j + 1; k < arrayOfIDs.Length; k++)
					{
						if (arrayOfIDs[k].GetID() != arrayOfIDs[j].GetID())
						{
							j = k - 1;
							break;
						}
						arrayOfIDs[k].SetID(++num);
					}
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
	}

	[Serializable]
	public class LobbyStatSettings
	{
		public StatDisplaySettings.StatType m_StatType;

		public float LowWatermark = float.MinValue;

		public bool LowerIsBetter;
	}

	[Serializable]
	public class GameResultBadge : GameBalanceVars.IUniqueID
	{
		public string DisplayName;

		public string BadgeDescription;

		public string BadgeGroupRequirementDescription;

		[AssetFileSelector("", "", "")]
		public string BadgeIconString;

		public bool DisplayEvenIfConsolidated;

		public int UniqueBadgeID;

		public GameBalanceVars.GameResultBadge.BadgeCondition[] MinimumConditions;

		public GameBalanceVars.GameResultBadge.BadgeQuality Quality;

		public GameBalanceVars.GameResultBadge.BadgeRole Role;

		public StatDisplaySettings.StatType BadgePointCalcType;

		public List<StatDisplaySettings.StatType> StatsToHighlight = new List<StatDisplaySettings.StatType>();

		public GameBalanceVars.GameResultBadge.ComparisonType ComparisonGroup;

		public int GlobalPercentileToObtain;

		public bool UsesFreelancerStats;

		public float GetQualityWorth()
		{
			switch (this.Quality)
			{
			case GameBalanceVars.GameResultBadge.BadgeQuality.Bronze:
				return 5f;
			case GameBalanceVars.GameResultBadge.BadgeQuality.Silver:
				return 8f;
			case GameBalanceVars.GameResultBadge.BadgeQuality.Gold:
				return 11f;
			default:
				throw new Exception("bad quality");
			}
		}

		public int GetID()
		{
			return this.UniqueBadgeID;
		}

		public void SetID(int newID)
		{
			this.UniqueBadgeID = newID;
		}

		public bool CouldRecieveBothBadgesInOneGame(GameBalanceVars.GameResultBadge other)
		{
			if (this.ComparisonGroup != other.ComparisonGroup)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GameResultBadge.CouldRecieveBothBadgesInOneGame(GameBalanceVars.GameResultBadge)).MethodHandle;
				}
				return true;
			}
			if (this.UsesFreelancerStats != other.UsesFreelancerStats)
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
				return true;
			}
			switch (this.ComparisonGroup)
			{
			case GameBalanceVars.GameResultBadge.ComparisonType.None:
				if (this.MinimumConditions.Length != other.MinimumConditions.Length)
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
					return true;
				}
				for (int i = 0; i < this.MinimumConditions.Length; i++)
				{
					if (this.MinimumConditions[i].StatsToSum.IsNullOrEmpty<StatDisplaySettings.StatType>() != other.MinimumConditions[i].StatsToSum.IsNullOrEmpty<StatDisplaySettings.StatType>())
					{
						return true;
					}
					if (this.MinimumConditions[i].StatsToSum.Length != other.MinimumConditions[i].StatsToSum.Length)
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
						return true;
					}
					for (int j = 0; j < this.MinimumConditions[i].StatsToSum.Length; j++)
					{
						if (this.MinimumConditions[i].StatsToSum[j] != other.MinimumConditions[i].StatsToSum[j])
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
							return true;
						}
					}
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				return false;
			case GameBalanceVars.GameResultBadge.ComparisonType.Global:
			case GameBalanceVars.GameResultBadge.ComparisonType.Game:
				return this.BadgePointCalcType != other.BadgePointCalcType;
			case GameBalanceVars.GameResultBadge.ComparisonType.Freelancer:
				return other.ComparisonGroup != GameBalanceVars.GameResultBadge.ComparisonType.Freelancer;
			default:
				throw new Exception("Strange comparison group");
			}
		}

		public bool DoStatsMeetMinimumRequirementsForBadge(StatDisplaySettings.IPersistatedStatValueSupplier StatSupplier)
		{
			for (int i = 0; i < this.MinimumConditions.Length; i++)
			{
				if (!this.MinimumConditions[i].StatsToSum.IsNullOrEmpty<StatDisplaySettings.StatType>())
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GameResultBadge.DoStatsMeetMinimumRequirementsForBadge(StatDisplaySettings.IPersistatedStatValueSupplier)).MethodHandle;
					}
					float num = 0f;
					for (int j = 0; j < this.MinimumConditions[i].StatsToSum.Length; j++)
					{
						float? stat = StatSupplier.GetStat(this.MinimumConditions[i].StatsToSum[j]);
						if (stat != null)
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
							num += stat.Value;
						}
					}
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!this.MinimumConditions[i].DoesValueMeetConditions(num))
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
						return false;
					}
				}
			}
			return true;
		}

		public enum BadgeQuality
		{
			Bronze,
			Silver,
			Gold
		}

		public enum BadgeRole
		{
			General,
			Frontliner,
			Firepower,
			Support
		}

		public enum ComparisonType
		{
			None,
			Global,
			Game,
			Freelancer
		}

		[Serializable]
		public class BadgeCondition
		{
			public StatDisplaySettings.StatType[] StatsToSum;

			public GameBalanceVars.GameResultBadge.BadgeCondition.BadgeFunctionType FunctionType;

			public float ValueToCompare;

			public bool DoesValueMeetConditions(float value)
			{
				if (this.StatsToSum.Length == 0)
				{
					return true;
				}
				if (this.FunctionType == GameBalanceVars.GameResultBadge.BadgeCondition.BadgeFunctionType.GreaterThan)
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.GameResultBadge.BadgeCondition.DoesValueMeetConditions(float)).MethodHandle;
					}
					return value > this.ValueToCompare;
				}
				if (this.FunctionType == GameBalanceVars.GameResultBadge.BadgeCondition.BadgeFunctionType.GreaterThanOrEqual)
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
					return value >= this.ValueToCompare;
				}
				if (this.FunctionType == GameBalanceVars.GameResultBadge.BadgeCondition.BadgeFunctionType.LessThan)
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
					return value < this.ValueToCompare;
				}
				if (this.FunctionType == GameBalanceVars.GameResultBadge.BadgeCondition.BadgeFunctionType.LessThanOrEqual)
				{
					return value <= this.ValueToCompare;
				}
				Log.Error("Attempting to sum stats but did not specify compare function", new object[0]);
				return false;
			}

			public enum BadgeFunctionType
			{
				None,
				LessThan,
				GreaterThan,
				LessThanOrEqual,
				GreaterThanOrEqual
			}
		}
	}

	[Serializable]
	public class CharacterLevelReward
	{
		public int charType;

		public int startLevel;

		public int repeatingLevel;

		public QuestItemReward reward;
	}

	[Serializable]
	public class RAFReward
	{
		public int pointsRequired;

		public int questId;

		public bool isEnabled = true;

		public bool isRepeating;
	}

	[Serializable]
	public class RankedTierLockedItems
	{
		public int MaxTier;

		public int MinTier;

		public GameBalanceVars.PlayerUnlockableReference[] Items;
	}

	[Serializable]
	public class CharacterUnlockData : GameBalanceVars.PlayerUnlockable
	{
		public CharacterType character;

		public GameBalanceVars.SkinUnlockData[] skinUnlockData;

		public GameBalanceVars.TauntUnlockData[] tauntUnlockData;

		public GameBalanceVars.AbilityVfxUnlockData[] abilityVfxUnlockData;

		public void CopyValuesToCharLinkUnlockData(GameBalanceVars.CharResourceLinkCharUnlockData other)
		{
			base.CopyValuesToBase(other);
		}
	}

	[Serializable]
	public class TauntUnlockData : GameBalanceVars.PlayerUnlockable
	{
		public void SetCharacterTypeInt(int charTypeInt)
		{
			this.Index1 = charTypeInt;
		}

		public int GetCharTypeInt()
		{
			return this.Index1;
		}

		public void CopyValuesTo(GameBalanceVars.TauntUnlockData other)
		{
			base.CopyValuesToBase(other);
		}

		public GameBalanceVars.TauntUnlockData Clone()
		{
			GameBalanceVars.TauntUnlockData tauntUnlockData = new GameBalanceVars.TauntUnlockData();
			base.CopyValuesToBase(tauntUnlockData);
			return tauntUnlockData;
		}
	}

	[Serializable]
	public class SkinUnlockData : GameBalanceVars.PlayerUnlockable
	{
		public GameBalanceVars.PatternUnlockData[] patternUnlockData;

		public void SetCharacterTypeInt(int charTypeInt)
		{
			this.Index1 = charTypeInt;
		}

		public int GetCharTypeInt()
		{
			return this.Index1;
		}

		public void CopyValuesToCharLinkUnlockData(GameBalanceVars.CharResourceLinkSkinUnlockData other)
		{
			base.CopyValuesToBase(other);
		}
	}

	[Serializable]
	public class PatternUnlockData : GameBalanceVars.PlayerUnlockable
	{
		public GameBalanceVars.ColorUnlockData[] colorUnlockData;

		public void SetCharacterTypeInt(int charTypeInt)
		{
			this.Index1 = charTypeInt;
		}

		public int GetCharTypeInt()
		{
			return this.Index1;
		}

		public void SetSkinIndex(int skinIndex)
		{
			this.Index2 = skinIndex;
		}

		public int GetSkinIndex()
		{
			return this.Index2;
		}

		public void CopyValuesToCharLinkUnlockData(GameBalanceVars.CharResourceLinkPatternUnlockData other)
		{
			base.CopyValuesToBase(other);
		}
	}

	[Serializable]
	public class ColorUnlockData : GameBalanceVars.PlayerUnlockable
	{
		public bool UnlockAutomaticallyWithParentSkin;

		public bool UsableByBots;

		public float UsableByBotsWeight;

		public bool IsGoldenAge;

		public void SetCharacterTypeInt(int charTypeInt)
		{
			this.Index1 = charTypeInt;
		}

		public int GetCharTypeInt()
		{
			return this.Index1;
		}

		public void SetSkinIndex(int skinIndex)
		{
			this.Index2 = skinIndex;
		}

		public int GetSkinIndex()
		{
			return this.Index2;
		}

		public void SetPatternIndex(int patternIndex)
		{
			this.Index3 = patternIndex;
		}

		public int GetPatternIndex()
		{
			return this.Index3;
		}

		public void CopyValuesTo(GameBalanceVars.ColorUnlockData other)
		{
			base.CopyValuesToBase(other);
			other.UnlockAutomaticallyWithParentSkin = this.UnlockAutomaticallyWithParentSkin;
			other.UsableByBots = this.UsableByBots;
			other.UsableByBotsWeight = this.UsableByBotsWeight;
			other.IsGoldenAge = this.IsGoldenAge;
		}
	}

	[Serializable]
	public class AbilityVfxUnlockData : GameBalanceVars.PlayerUnlockable
	{
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Ability[",
				this.Index2,
				"]->VfxSwap[",
				this.ID,
				"]"
			});
		}

		public void SetCharacterTypeInt(int charTypeInt)
		{
			this.Index1 = charTypeInt;
		}

		public int GetCharacterTypeInt()
		{
			return this.Index1;
		}

		public void SetSwapAbilityId(int swapAbilityId)
		{
			this.Index2 = swapAbilityId;
		}

		public int GetSwapAbilityId()
		{
			return this.Index2;
		}

		public void CopyValuesTo(GameBalanceVars.AbilityVfxUnlockData other)
		{
			base.CopyValuesToBase(other);
		}

		public GameBalanceVars.AbilityVfxUnlockData Clone()
		{
			GameBalanceVars.AbilityVfxUnlockData abilityVfxUnlockData = new GameBalanceVars.AbilityVfxUnlockData();
			base.CopyValuesToBase(abilityVfxUnlockData);
			return abilityVfxUnlockData;
		}
	}

	[Serializable]
	public class CharResourceLinkCharUnlockData : GameBalanceVars.PlayerUnlockable
	{
		public void CopyValuesTo(GameBalanceVars.CharacterUnlockData other)
		{
			base.CopyValuesToBase(other);
		}
	}

	[Serializable]
	public class CharResourceLinkSkinUnlockData : GameBalanceVars.PlayerUnlockable
	{
		public void SetCharacterTypeInt(int charTypeInt)
		{
			this.Index1 = charTypeInt;
		}

		public int GetCharTypeInt()
		{
			return this.Index1;
		}

		public void CopyValuesTo(GameBalanceVars.SkinUnlockData other)
		{
			base.CopyValuesToBase(other);
		}
	}

	[Serializable]
	public class CharResourceLinkPatternUnlockData : GameBalanceVars.PlayerUnlockable
	{
		public void SetCharacterTypeInt(int charTypeInt)
		{
			this.Index1 = charTypeInt;
		}

		public int GetCharTypeInt()
		{
			return this.Index1;
		}

		public void SetSkinIndex(int skinIndex)
		{
			this.Index2 = skinIndex;
		}

		public int GetSkinIndex()
		{
			return this.Index2;
		}

		public void CopyValuesTo(GameBalanceVars.PatternUnlockData other)
		{
			base.CopyValuesToBase(other);
		}
	}

	[Serializable]
	public class AbilityModUnlockData : GameBalanceVars.PlayerUnlockable
	{
	}

	[Serializable]
	public class LevelProgressInfo
	{
		public int ExperienceToNextLevel;

		public CurrencyData[] CurrencyRewards;
	}

	public enum UnlockableType
	{
		None,
		Banner,
		Title,
		Character,
		Taunt,
		Skin,
		Pattern,
		Color,
		AbilityVfx,
		AbilityMod,
		Emoji,
		Overcon,
		LoadingScreenBackground
	}

	[Serializable]
	public class UnlockData
	{
		public string LogicStatement;

		public GameBalanceVars.UnlockCondition[] UnlockConditions;

		public string VisibilityLogicStatement;

		public GameBalanceVars.UnlockCondition[] VisibilityConditions;

		public string PurchaseableLogicStatement;

		public GameBalanceVars.UnlockCondition[] PurchaseableConditions;

		public void InitValues()
		{
			this.LogicStatement = string.Empty;
			this.UnlockConditions = new GameBalanceVars.UnlockCondition[0];
			this.VisibilityLogicStatement = string.Empty;
			this.VisibilityConditions = new GameBalanceVars.UnlockCondition[0];
			this.PurchaseableLogicStatement = string.Empty;
			this.PurchaseableConditions = new GameBalanceVars.UnlockCondition[0];
		}

		public GameBalanceVars.UnlockData Clone()
		{
			GameBalanceVars.UnlockData unlockData = new GameBalanceVars.UnlockData();
			unlockData.LogicStatement = this.LogicStatement;
			unlockData.VisibilityLogicStatement = this.VisibilityLogicStatement;
			unlockData.PurchaseableLogicStatement = this.PurchaseableLogicStatement;
			unlockData.UnlockConditions = null;
			if (this.UnlockConditions != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.UnlockData.Clone()).MethodHandle;
				}
				unlockData.UnlockConditions = new GameBalanceVars.UnlockCondition[this.UnlockConditions.Length];
				for (int i = 0; i < this.UnlockConditions.Length; i++)
				{
					GameBalanceVars.UnlockCondition[] unlockConditions = unlockData.UnlockConditions;
					int num = i;
					GameBalanceVars.UnlockCondition unlockCondition;
					if (this.UnlockConditions[i] != null)
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
						unlockCondition = this.UnlockConditions[i].Clone();
					}
					else
					{
						unlockCondition = null;
					}
					unlockConditions[num] = unlockCondition;
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
			unlockData.VisibilityConditions = null;
			if (this.VisibilityConditions != null)
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
				unlockData.VisibilityConditions = new GameBalanceVars.UnlockCondition[this.VisibilityConditions.Length];
				for (int j = 0; j < this.VisibilityConditions.Length; j++)
				{
					GameBalanceVars.UnlockCondition[] visibilityConditions = unlockData.VisibilityConditions;
					int num2 = j;
					GameBalanceVars.UnlockCondition unlockCondition2;
					if (this.VisibilityConditions[j] != null)
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
						unlockCondition2 = this.VisibilityConditions[j].Clone();
					}
					else
					{
						unlockCondition2 = null;
					}
					visibilityConditions[num2] = unlockCondition2;
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
			}
			unlockData.PurchaseableConditions = null;
			if (this.PurchaseableConditions != null)
			{
				unlockData.PurchaseableConditions = new GameBalanceVars.UnlockCondition[this.PurchaseableConditions.Length];
				for (int k = 0; k < this.PurchaseableConditions.Length; k++)
				{
					GameBalanceVars.UnlockCondition[] purchaseableConditions = unlockData.PurchaseableConditions;
					int num3 = k;
					GameBalanceVars.UnlockCondition unlockCondition3;
					if (this.PurchaseableConditions[k] != null)
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
						unlockCondition3 = this.PurchaseableConditions[k].Clone();
					}
					else
					{
						unlockCondition3 = null;
					}
					purchaseableConditions[num3] = unlockCondition3;
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
			return unlockData;
		}

		public static GameBalanceVars.UnlockCondition[] CloneUnlockConditionArray(GameBalanceVars.UnlockCondition[] input)
		{
			GameBalanceVars.UnlockCondition[] array = null;
			if (input != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.UnlockData.CloneUnlockConditionArray(GameBalanceVars.UnlockCondition[])).MethodHandle;
				}
				array = new GameBalanceVars.UnlockCondition[input.Length];
				for (int i = 0; i < input.Length; i++)
				{
					array[i] = ((input[i] == null) ? null : input[i].Clone());
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
			return array;
		}

		public enum UnlockType
		{
			CharacterLevel,
			PlayerLevel,
			Purchase,
			ELO,
			Custom,
			Quest,
			HasDateTimePassed,
			FactionTierReached,
			TitleLevelReached,
			CurrentSeason
		}
	}

	[Serializable]
	public class UnlockCondition
	{
		public GameBalanceVars.UnlockData.UnlockType ConditionType;

		public int typeSpecificData;

		public int typeSpecificData2;

		public int typeSpecificData3;

		public List<int> typeSpecificDate;

		public string typeSpecificString;

		public GameBalanceVars.UnlockCondition Clone()
		{
			GameBalanceVars.UnlockCondition unlockCondition = (GameBalanceVars.UnlockCondition)base.MemberwiseClone();
			if (this.typeSpecificDate != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.UnlockCondition.Clone()).MethodHandle;
				}
				unlockCondition.typeSpecificDate = new List<int>(this.typeSpecificDate);
			}
			return unlockCondition;
		}
	}

	[Serializable]
	public class UnlockConditionValue
	{
		public GameBalanceVars.UnlockData.UnlockType ConditionType;

		public int typeSpecificData;

		public int typeSpecificData2;

		public int typeSpecificData3;

		public List<int> typeSpecificDate;

		public string typeSpecificString;
	}

	public interface IUniqueID
	{
		int GetID();

		void SetID(int newID);
	}

	[Serializable]
	public class PlayerUnlockable : GameBalanceVars.IUniqueID
	{
		[HideInInspector]
		public int ID;

		public string Name;

		public int m_sortOrder;

		public string ObtainedDescription;

		public string PurchaseDescription;

		public InventoryItemRarity Rarity;

		public CountryPrices Prices;

		public int Index1;

		public int Index2;

		public int Index3;

		public GameBalanceVars.UnlockData m_unlockData;

		public bool m_isHidden;

		public int GetID()
		{
			return this.ID;
		}

		public void SetID(int newID)
		{
			this.ID = newID;
		}

		public string GetObtainedDescription()
		{
			if (this is GameBalanceVars.ChatEmoticon)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.PlayerUnlockable.GetObtainedDescription()).MethodHandle;
				}
				return StringUtil.TR_EmojiObtainedDescription(this.ID);
			}
			if (this is GameBalanceVars.ColorUnlockData)
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
				CharacterType index = (CharacterType)this.Index1;
				return StringUtil.TR_CharacterPatternColorObtainedDescription(index.ToString(), this.Index2 + 1, this.Index3 + 1, this.ID + 1);
			}
			if (this is GameBalanceVars.TauntUnlockData)
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
				CharacterType index2 = (CharacterType)this.Index1;
				return StringUtil.TR_CharacterTauntObtainedDescription(index2.ToString(), this.ID + 1);
			}
			if (this is GameBalanceVars.AbilityVfxUnlockData)
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
				CharacterType index3 = (CharacterType)this.Index1;
				return StringUtil.TR_GetCharacterVFXSwapObtainedDescription(index3.ToString(), this.Index2 + 1, this.ID);
			}
			if (this is GameBalanceVars.PlayerRibbon)
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
				return StringUtil.TR_RibbonObtainedDescription(this.ID);
			}
			if (this is GameBalanceVars.PlayerBanner)
			{
				return StringUtil.TR_BannerObtainedDescription(this.ID);
			}
			if (this is GameBalanceVars.PlayerTitle)
			{
				return StringUtil.TR_TitleObtainedDescription(this.ID);
			}
			return this.ObtainedDescription;
		}

		public string GetPurchaseDescription()
		{
			if (this is GameBalanceVars.ChatEmoticon)
			{
				return StringUtil.TR_EmojiPurchaseDescription(this.ID);
			}
			if (this is GameBalanceVars.ColorUnlockData)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.PlayerUnlockable.GetPurchaseDescription()).MethodHandle;
				}
				CharacterType index = (CharacterType)this.Index1;
				return StringUtil.TR_CharacterPatternColorPurchaseDescription(index.ToString(), this.Index2 + 1, this.Index3 + 1, this.ID + 1);
			}
			if (this is GameBalanceVars.TauntUnlockData)
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
				CharacterType index2 = (CharacterType)this.Index1;
				return StringUtil.TR_CharacterTauntPurchaseDescription(index2.ToString(), this.ID + 1);
			}
			if (this is GameBalanceVars.AbilityVfxUnlockData)
			{
				CharacterType index3 = (CharacterType)this.Index1;
				return StringUtil.TR_GetCharacterVFXSwapPurchaseDescription(index3.ToString(), this.Index2 + 1, this.ID);
			}
			return this.PurchaseDescription;
		}

		public string GetName()
		{
			return this.Name;
		}

		protected void CopyValuesToBase(GameBalanceVars.PlayerUnlockable other)
		{
			other.ID = this.ID;
			other.Name = this.Name;
			other.m_sortOrder = this.m_sortOrder;
			other.ObtainedDescription = this.ObtainedDescription;
			other.PurchaseDescription = this.PurchaseDescription;
			other.Rarity = this.Rarity;
			other.Prices = this.Prices;
			other.Index1 = this.Index1;
			other.Index2 = this.Index2;
			other.Index3 = this.Index3;
			if (this.m_unlockData != null)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.PlayerUnlockable.CopyValuesToBase(GameBalanceVars.PlayerUnlockable)).MethodHandle;
				}
				other.m_unlockData = this.m_unlockData.Clone();
			}
			else
			{
				other.m_unlockData = new GameBalanceVars.UnlockData();
				other.m_unlockData.InitValues();
			}
			other.m_isHidden = this.m_isHidden;
		}
	}

	[Serializable]
	public class PlayerUnlockableReference
	{
		public GameBalanceVars.UnlockableType Type;

		public int ID;

		public int Index1;

		public int Index2;

		public int Index3;
	}

	[Serializable]
	public class UnlockExlusivePool
	{
		public string PoolName;

		public GameBalanceVars.UnlockExlusivePool.ExclusivePoolType PoolType;

		public int[] PoolOfBannerIDs;

		public int TotalBannersAbleToBeUnlockedAtOnce;

		public enum ExclusivePoolType
		{
			None,
			Banner,
			Ribbon,
			Emoticon,
			Overcon,
			Title
		}
	}

	[Serializable]
	public class ChatEmoticon : GameBalanceVars.PlayerUnlockable
	{
		public string IconPath;

		public string GetEmojiName()
		{
			return StringUtil.TR_EmojiName(this.ID);
		}
	}

	[Serializable]
	public class OverconUnlockData : GameBalanceVars.PlayerUnlockable
	{
		public string m_commandName;

		public string GetOverconName()
		{
			return StringUtil.TR_GetOverconDisplayName(this.ID);
		}
	}

	[Serializable]
	public class LoadingScreenBackground : GameBalanceVars.PlayerUnlockable
	{
		public string m_resourceString;

		public string m_iconPath;

		public string GetLoadingScreenBackgroundName()
		{
			return StringUtil.TR_GetLoadingScreenBackgroundName(this.ID);
		}
	}

	[Serializable]
	public class TitleLevelDefinition
	{
		public string m_name;

		public int m_titleId;

		public int m_maxLevel;
	}

	[Serializable]
	public class PlayerTitle : GameBalanceVars.PlayerUnlockable
	{
		public CharacterType m_relatedCharacter;

		public const string c_levelToken = "[^level^]";

		public string GetTitleText(int titleLevel = -1)
		{
			string text = StringUtil.TR_PlayerTitle(this.ID);
			if (ClientGameManager.Get() != null)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.PlayerTitle.GetTitleText(int)).MethodHandle;
				}
				if (!text.IsNullOrEmpty())
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
					if (text.IndexOf("[^level^]", StringComparison.OrdinalIgnoreCase) >= 0)
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
						if (titleLevel < 0)
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
							titleLevel = ClientGameManager.Get().GetCurrentTitleLevel(this.ID);
						}
						text = GameBalanceVars.PlayerTitle.ReplaceLevelIntoTitle(text, titleLevel);
					}
				}
			}
			return text;
		}

		public static string ReplaceLevelIntoTitle(string title, int level)
		{
			int length = "[^level^]".Length;
			string str = level.ToString();
			int num = 0;
			int num2;
			while ((num2 = title.IndexOf("[^level^]", StringComparison.OrdinalIgnoreCase)) >= 0)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameBalanceVars.PlayerTitle.ReplaceLevelIntoTitle(string, int)).MethodHandle;
				}
				if (num >= 0x64)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						return title;
					}
				}
				else
				{
					title = title.Substring(0, num2) + str + title.Substring(num2 + length);
					num++;
				}
			}
			return title;
		}
	}

	[Serializable]
	public class PlayerBanner : GameBalanceVars.PlayerUnlockable
	{
		public string m_resourceString;

		public string m_iconResourceString;

		public GameBalanceVars.PlayerBanner.BannerType m_type;

		public CharacterType m_relatedCharacter;

		public bool m_isDefault;

		public string GetBannerName()
		{
			return StringUtil.TR_BannerName(this.ID);
		}

		public new string GetObtainedDescription()
		{
			return StringUtil.TR_BannerObtainedDescription(this.ID);
		}

		public enum BannerType
		{
			Foreground,
			Background
		}
	}

	[Serializable]
	public class PlayerRibbon : GameBalanceVars.PlayerUnlockable
	{
		public string m_resourceString;

		public string m_resourceIconString;

		public string GetRibbonName()
		{
			return StringUtil.TR_RibbonName(this.ID);
		}
	}

	[Serializable]
	public class StoreItemForPurchase : GameBalanceVars.PlayerUnlockable
	{
		public int m_itemTemplateId;

		public string m_productCode;

		public string m_overlayText;

		public string GetStoreItemName()
		{
			return StringUtil.TR_InventoryItemName(this.m_itemTemplateId);
		}
	}

	public enum GameRewardBucketType
	{
		FullVsHumanRewards,
		HumanVsBotsRewards,
		CasualGameRewards,
		NewPlayerRewards,
		PlacementMatchRewards,
		NoRewards
	}

	[Serializable]
	public class GameRewardBucket
	{
		public int XPBonusForWin;

		public float XPBonusPerMinute;

		public float XPBonusPerMinuteCap;

		public float XPBonusPerQueueTimeMinute;

		public float XPBonusPerQueueTimeMinuteCap;

		public GameBalanceVars.GameCurrencyReward[] CurrencyRewards;
	}

	[Serializable]
	public class GameCurrencyReward
	{
		public CurrencyType Type;

		public int AmountOnWin;

		public int AmountOnLoss;
	}
}
