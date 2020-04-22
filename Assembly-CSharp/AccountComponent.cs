using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class AccountComponent : ICloneable
{
	public enum UIStateIdentifier
	{
		HasResetMods = 0,
		HasSeenTrustWarEndPopup = 1,
		HasSeenSeasonTwoChapterTwo = 2,
		HasSeenFactionWarSeasonTwoChapterTwo = 3,
		TutorialOverview = 4,
		TutorialPhases = 5,
		TutorialMovement = 6,
		TutorialCatalysts = 7,
		TutorialCooldowns = 8,
		TutorialPowerups = 9,
		TutorialCover = 10,
		TutorialFoW = 11,
		TutorialRespawn = 12,
		HasSeenSeasonTwoChapterThree = 13,
		HasSeenSeasonTwoChapterFour = 14,
		HasSeenSeasonTwoChapterFive = 0xF,
		CashShopFeaturedItemsVersionViewed = 0x10,
		NumLootMatrixesOpened = 17,
		HasViewedFluxHighlight = 18,
		HasViewedGGHighlight = 19,
		NumDailiesChosen = 20,
		HasSeenSeasonFourChapterOne = 21,
		HasViewedFreelancerTokenHighlight = 22,
		NONE = 10000
	}

	public Dictionary<int, int> TitleLevels;

	public Dictionary<string, int> AppliedEntitlements;

	public const int MinNumGlobalCharacterLoadouts = 2;

	public LeakyBucket RecentSoloGamesPlayed;

	public string RAFReferralCode;

	public CharacterType LastCharacter
	{
		get;
		set;
	}

	public List<CharacterType> LastRemoteCharacters
	{
		get;
		set;
	}

	public List<int> UnlockedTitleIDs
	{
		get;
		set;
	}

	public List<int> UnlockedEmojiIDs
	{
		get;
		set;
	}

	public List<int> UnlockedOverconIDs
	{
		get;
		set;
	}

	public List<int> UnlockedBannerIDs
	{
		get;
		set;
	}

	public List<int> UnlockedRibbonIDs
	{
		get;
		set;
	}

	public int SelectedTitleID
	{
		get;
		set;
	}

	public int SelectedForegroundBannerID
	{
		get;
		set;
	}

	public int SelectedBackgroundBannerID
	{
		get;
		set;
	}

	public int SelectedRibbonID
	{
		get;
		set;
	}

	public Dictionary<int, bool> UnlockedLoadingScreenBackgroundIdsToActivatedState
	{
		get;
		set;
	}

	public List<PendingPurchaseDetails> PendingPurchases
	{
		get;
		set;
	}

	public int NumGlobalCharacterLoadouts
	{
		get;
		set;
	}

	public Dictionary<UIStateIdentifier, int> UIStates
	{
		get;
		set;
	}

	public int RankedSortKarma
	{
		get;
		set;
	}

	public Dictionary<int, Dictionary<string, int>> HighestRankedTierReached
	{
		get;
		set;
	}

	public Dictionary<int, PlayerFactionCompetitionData> FactionCompetitionData
	{
		get;
		set;
	}

	public bool DisplayDevTag
	{
		get;
		set;
	}

	public bool DailyQuestsAvailable
	{
		get;
		set;
	}

	public CharacterType[] FreeRotationCharacters
	{
		get;
		set;
	}

	public DateTime FreeRotationNextUpdateDate
	{
		get;
		set;
	}

	public Dictionary<int, KeyCodeData> KeyCodeMapping
	{
		get;
		set;
	}

	public int PushToTalkKeyType
	{
		get;
		set;
	}

	public int PushToTalkKeyCode
	{
		get;
		set;
	}

	public string PushToTalkKeyName
	{
		get;
		set;
	}

	public int FreelancerExpBonusGames
	{
		get;
		set;
	}

	public DateTime FreelancerExpBonusTime
	{
		get;
		set;
	}

	public AccountComponent()
	{
		LastCharacter = CharacterType.Scoundrel;
		LastRemoteCharacters = new List<CharacterType>();
		UnlockedTitleIDs = new List<int>();
		TitleLevels = new Dictionary<int, int>();
		UnlockedBannerIDs = new List<int>();
		UnlockedRibbonIDs = new List<int>();
		UnlockedEmojiIDs = new List<int>();
		UnlockedOverconIDs = new List<int>();
		UnlockedLoadingScreenBackgroundIdsToActivatedState = new Dictionary<int, bool>();
		NumGlobalCharacterLoadouts = 2;
		SelectedTitleID = -1;
		SelectedForegroundBannerID = -1;
		SelectedBackgroundBannerID = -1;
		SelectedRibbonID = -1;
		AppliedEntitlements = new Dictionary<string, int>();
		PendingPurchases = new List<PendingPurchaseDetails>();
		KeyCodeMapping = new Dictionary<int, KeyCodeData>();
		UIStates = new Dictionary<UIStateIdentifier, int>();
		RecentSoloGamesPlayed = new LeakyBucket();
		RankedSortKarma = 0;
		HighestRankedTierReached = new Dictionary<int, Dictionary<string, int>>();
		FactionCompetitionData = new Dictionary<int, PlayerFactionCompetitionData>();
		PushToTalkKeyType = 0;
		PushToTalkKeyCode = 0;
		PushToTalkKeyName = null;
	}

	public static bool IsUIStateTutorialVideo(UIStateIdentifier state)
	{
		if (state != UIStateIdentifier.TutorialCatalysts && state != UIStateIdentifier.TutorialCooldowns)
		{
			if (state != UIStateIdentifier.TutorialCover)
			{
				if (state != UIStateIdentifier.TutorialFoW && state != UIStateIdentifier.TutorialMovement)
				{
					if (state != UIStateIdentifier.TutorialOverview && state != UIStateIdentifier.TutorialPhases)
					{
						if (state != UIStateIdentifier.TutorialPowerups)
						{
							if (state != UIStateIdentifier.TutorialRespawn)
							{
								return false;
							}
						}
					}
				}
			}
		}
		return true;
	}

	private void ValidateExclusivityPool(GameBalanceVars.UnlockExlusivePool pool, List<int> unlockedIDs)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < unlockedIDs.Count; i++)
		{
			if (pool.PoolOfBannerIDs.Contains(unlockedIDs[i]))
			{
				list.Add(unlockedIDs[i]);
			}
		}
		while (true)
		{
			if (list.Count <= pool.TotalBannersAbleToBeUnlockedAtOnce)
			{
				return;
			}
			while (true)
			{
				for (int j = 0; j < list.Count - pool.TotalBannersAbleToBeUnlockedAtOnce; j++)
				{
					Log.Info("Unlock Exclusivity Exceeded. Removing ID {0} from {1}", list[j], pool.PoolType);
					unlockedIDs.Remove(list[j]);
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
		}
	}

	public void ValidateSelectedBanner()
	{
		if (UnlockedBannerIDs.Contains(SelectedBackgroundBannerID))
		{
			return;
		}
		SelectedBackgroundBannerID = -1;
		for (int i = 0; i < UnlockedBannerIDs.Count; i++)
		{
			int num = UnlockedBannerIDs[i];
			if (LobbyGameplayData.Get().GameBalanceVars.GetBanner(num).m_type != GameBalanceVars.PlayerBanner.BannerType.Background)
			{
				continue;
			}
			while (true)
			{
				SelectedBackgroundBannerID = num;
				return;
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

	public void ValidateSelectedEmblem()
	{
		if (UnlockedBannerIDs.Contains(SelectedForegroundBannerID))
		{
			return;
		}
		SelectedForegroundBannerID = -1;
		for (int i = 0; i < UnlockedBannerIDs.Count; i++)
		{
			int num = UnlockedBannerIDs[i];
			if (LobbyGameplayData.Get().GameBalanceVars.GetBanner(num).m_type != 0)
			{
				continue;
			}
			while (true)
			{
				SelectedForegroundBannerID = num;
				return;
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

	public void ValidateSelectedTitle()
	{
		if (UnlockedTitleIDs.Contains(SelectedTitleID))
		{
			return;
		}
		while (true)
		{
			if (UnlockedTitleIDs.Count > 0)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						SelectedTitleID = UnlockedTitleIDs[0];
						return;
					}
				}
			}
			SelectedTitleID = -1;
			return;
		}
	}

	public void ValidateExclusivityPools()
	{
		if (LobbyGameplayData.Get() == null || LobbyGameplayData.Get().GameBalanceVars.ExclusivityPools == null)
		{
			return;
		}
		GameBalanceVars.UnlockExlusivePool[] exclusivityPools = LobbyGameplayData.Get().GameBalanceVars.ExclusivityPools;
		foreach (GameBalanceVars.UnlockExlusivePool unlockExlusivePool in exclusivityPools)
		{
			if (unlockExlusivePool.PoolType == GameBalanceVars.UnlockExlusivePool.ExclusivePoolType.Banner)
			{
				ValidateExclusivityPool(unlockExlusivePool, UnlockedBannerIDs);
				ValidateSelectedBanner();
				ValidateSelectedBanner();
			}
			else if (unlockExlusivePool.PoolType == GameBalanceVars.UnlockExlusivePool.ExclusivePoolType.Emoticon)
			{
				ValidateExclusivityPool(unlockExlusivePool, UnlockedEmojiIDs);
			}
			else if (unlockExlusivePool.PoolType == GameBalanceVars.UnlockExlusivePool.ExclusivePoolType.Overcon)
			{
				ValidateExclusivityPool(unlockExlusivePool, UnlockedOverconIDs);
			}
			else if (unlockExlusivePool.PoolType == GameBalanceVars.UnlockExlusivePool.ExclusivePoolType.Ribbon)
			{
				ValidateExclusivityPool(unlockExlusivePool, UnlockedRibbonIDs);
				if (!UnlockedRibbonIDs.Contains(SelectedRibbonID))
				{
					SelectedRibbonID = -1;
				}
			}
			else
			{
				if (unlockExlusivePool.PoolType != GameBalanceVars.UnlockExlusivePool.ExclusivePoolType.Title)
				{
					continue;
				}
				ValidateExclusivityPool(unlockExlusivePool, UnlockedTitleIDs);
				if (!UnlockedTitleIDs.Contains(SelectedTitleID))
				{
					SelectedTitleID = -1;
				}
			}
		}
	}

	private void ClearAllItemsInExclusivityPool(GameBalanceVars.UnlockExlusivePool pool, List<int> unlockedIDs)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < unlockedIDs.Count; i++)
		{
			if (pool.PoolOfBannerIDs.Contains(unlockedIDs[i]))
			{
				list.Add(unlockedIDs[i]);
			}
		}
		while (true)
		{
			for (int j = 0; j < list.Count; j++)
			{
				Log.Info("Clearing Exclusivity Poos. Removing ID {0} from {1}", list[j], pool.PoolType);
				unlockedIDs.Remove(list[j]);
			}
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void ClearAllItemsInExclusivityPool(string clearPoolName)
	{
		if (LobbyGameplayData.Get() == null)
		{
			return;
		}
		while (true)
		{
			if (LobbyGameplayData.Get().GameBalanceVars.ExclusivityPools == null)
			{
				return;
			}
			while (true)
			{
				GameBalanceVars.UnlockExlusivePool[] exclusivityPools = LobbyGameplayData.Get().GameBalanceVars.ExclusivityPools;
				foreach (GameBalanceVars.UnlockExlusivePool unlockExlusivePool in exclusivityPools)
				{
					if (!(unlockExlusivePool.PoolName == clearPoolName))
					{
						continue;
					}
					if (unlockExlusivePool.PoolType == GameBalanceVars.UnlockExlusivePool.ExclusivePoolType.Banner)
					{
						ClearAllItemsInExclusivityPool(unlockExlusivePool, UnlockedBannerIDs);
						ValidateSelectedBanner();
						ValidateSelectedBanner();
						continue;
					}
					if (unlockExlusivePool.PoolType == GameBalanceVars.UnlockExlusivePool.ExclusivePoolType.Emoticon)
					{
						ClearAllItemsInExclusivityPool(unlockExlusivePool, UnlockedEmojiIDs);
						continue;
					}
					if (unlockExlusivePool.PoolType == GameBalanceVars.UnlockExlusivePool.ExclusivePoolType.Overcon)
					{
						ClearAllItemsInExclusivityPool(unlockExlusivePool, UnlockedOverconIDs);
						continue;
					}
					if (unlockExlusivePool.PoolType == GameBalanceVars.UnlockExlusivePool.ExclusivePoolType.Ribbon)
					{
						ClearAllItemsInExclusivityPool(unlockExlusivePool, UnlockedRibbonIDs);
						if (!UnlockedRibbonIDs.Contains(SelectedRibbonID))
						{
							SelectedRibbonID = -1;
						}
						continue;
					}
					if (unlockExlusivePool.PoolType != GameBalanceVars.UnlockExlusivePool.ExclusivePoolType.Title)
					{
						continue;
					}
					ClearAllItemsInExclusivityPool(unlockExlusivePool, UnlockedTitleIDs);
					if (!UnlockedTitleIDs.Contains(SelectedTitleID))
					{
						SelectedTitleID = -1;
					}
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
		}
	}

	public PlayerFactionCompetitionData GetPlayerCompetitionData(int competitionID)
	{
		if (FactionCompetitionData.ContainsKey(competitionID))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return FactionCompetitionData[competitionID];
				}
			}
		}
		PlayerFactionCompetitionData playerFactionCompetitionData = new PlayerFactionCompetitionData();
		playerFactionCompetitionData.CompetitionID = competitionID;
		playerFactionCompetitionData.Factions = new Dictionary<int, FactionPlayerData>();
		FactionCompetitionData[competitionID] = playerFactionCompetitionData;
		return playerFactionCompetitionData;
	}

	public FactionPlayerData GetPlayerCompetitionFactionData(int competitionID, int factionID)
	{
		PlayerFactionCompetitionData playerCompetitionData = GetPlayerCompetitionData(competitionID);
		if (playerCompetitionData.Factions.ContainsKey(factionID))
		{
			return playerCompetitionData.Factions[factionID];
		}
		FactionPlayerData factionPlayerData = new FactionPlayerData();
		factionPlayerData.FactionID = factionID;
		factionPlayerData.TotalXP = 0;
		playerCompetitionData.Factions[factionID] = factionPlayerData;
		return factionPlayerData;
	}

	public int GetAppliedEntitlementCount(string entitlement)
	{
		if (AppliedEntitlements.ContainsKey(entitlement))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return AppliedEntitlements[entitlement];
				}
			}
		}
		return 0;
	}

	public int GetHighestRankedTierReached(int seasonIndex, string eloKey)
	{
		if (!HighestRankedTierReached.ContainsKey(seasonIndex) || !HighestRankedTierReached[seasonIndex].ContainsKey(eloKey))
		{
			return -1;
		}
		return HighestRankedTierReached[seasonIndex][eloKey];
	}

	public void SetHighestRankedTierReached(int seasonIndex, string eloKey, int newTier)
	{
		if (!HighestRankedTierReached.ContainsKey(seasonIndex))
		{
			HighestRankedTierReached[seasonIndex] = new Dictionary<string, int>();
		}
		HighestRankedTierReached[seasonIndex][eloKey] = newTier;
	}

	public void IncrementAppliedEntitlementCount(string entitlement, int quantity = 1)
	{
		AppliedEntitlements[entitlement] = GetAppliedEntitlementCount(entitlement) + quantity;
	}

	public bool IsTitleUnlocked(int titleId)
	{
		return UnlockedTitleIDs.Contains(titleId);
	}

	public bool IsTitleUnlocked(GameBalanceVars.PlayerTitle title)
	{
		return UnlockedTitleIDs.Contains(title.ID);
	}

	public int GetCurrentTitleLevel(int titleId)
	{
		int result = 1;
		if (TitleLevels != null)
		{
			if (TitleLevels.ContainsKey(titleId))
			{
				result = TitleLevels[titleId];
			}
		}
		return result;
	}

	public bool IsChatEmojiUnlocked(int emojiID)
	{
		return UnlockedEmojiIDs.Contains(emojiID);
	}

	public bool IsChatEmojiUnlocked(GameBalanceVars.ChatEmoticon emoji)
	{
		return UnlockedEmojiIDs.Contains(emoji.ID);
	}

	public bool IsOverconUnlocked(int overconID)
	{
		return UnlockedOverconIDs.Contains(overconID);
	}

	public bool IsBannerUnlocked(int bannerId)
	{
		return UnlockedBannerIDs.Contains(bannerId);
	}

	public bool IsBannerUnlocked(GameBalanceVars.PlayerBanner banner)
	{
		return UnlockedBannerIDs.Contains(banner.ID);
	}

	public bool IsRibbonUnlocked(int ribbonId)
	{
		return UnlockedRibbonIDs.Contains(ribbonId);
	}

	public bool IsRibbonUnlocked(GameBalanceVars.PlayerRibbon ribbon)
	{
		return UnlockedRibbonIDs.Contains(ribbon.ID);
	}

	public bool IsLoadingScreenBackgroundUnlocked(int loadingScreenBackgroundId)
	{
		return UnlockedLoadingScreenBackgroundIdsToActivatedState.ContainsKey(loadingScreenBackgroundId);
	}

	public bool IsLoadingScreenBackgroundActive(int loadingScreenBackgroundId)
	{
		UnlockedLoadingScreenBackgroundIdsToActivatedState.TryGetValue(loadingScreenBackgroundId, out bool value);
		return value;
	}

	public bool ToggleLoadingScreenBackgroundActive(int loadingScreenBackgroundId, bool newState)
	{
		if (!IsLoadingScreenBackgroundUnlocked(loadingScreenBackgroundId))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		UnlockedLoadingScreenBackgroundIdsToActivatedState[loadingScreenBackgroundId] = newState;
		return true;
	}

	public bool UnlockLoadingScreenBackground(int loadingScreenBackgroundId)
	{
		if (IsLoadingScreenBackgroundUnlocked(loadingScreenBackgroundId))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		UnlockedLoadingScreenBackgroundIdsToActivatedState[loadingScreenBackgroundId] = true;
		return true;
	}

	public bool IsCharacterInFreeRotation(CharacterType characterType)
	{
		int result;
		if (FreeRotationCharacters != null)
		{
			result = (FreeRotationCharacters.Contains(characterType) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public int GetUIState(UIStateIdentifier uiState)
	{
		if (uiState == UIStateIdentifier.NONE)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					throw new Exception("UI State not specified!");
				}
			}
		}
		if (UIStates.ContainsKey(uiState))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return UIStates[uiState];
				}
			}
		}
		return 0;
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
