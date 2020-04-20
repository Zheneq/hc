using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class AccountComponent : ICloneable
{
	public Dictionary<int, int> TitleLevels;

	public Dictionary<string, int> AppliedEntitlements;

	public const int MinNumGlobalCharacterLoadouts = 2;

	public LeakyBucket RecentSoloGamesPlayed;

	public string RAFReferralCode;

	public AccountComponent()
	{
		this.LastCharacter = CharacterType.Scoundrel;
		this.LastRemoteCharacters = new List<CharacterType>();
		this.UnlockedTitleIDs = new List<int>();
		this.TitleLevels = new Dictionary<int, int>();
		this.UnlockedBannerIDs = new List<int>();
		this.UnlockedRibbonIDs = new List<int>();
		this.UnlockedEmojiIDs = new List<int>();
		this.UnlockedOverconIDs = new List<int>();
		this.UnlockedLoadingScreenBackgroundIdsToActivatedState = new Dictionary<int, bool>();
		this.NumGlobalCharacterLoadouts = 2;
		this.SelectedTitleID = -1;
		this.SelectedForegroundBannerID = -1;
		this.SelectedBackgroundBannerID = -1;
		this.SelectedRibbonID = -1;
		this.AppliedEntitlements = new Dictionary<string, int>();
		this.PendingPurchases = new List<PendingPurchaseDetails>();
		this.KeyCodeMapping = new Dictionary<int, KeyCodeData>();
		this.UIStates = new Dictionary<AccountComponent.UIStateIdentifier, int>();
		this.RecentSoloGamesPlayed = new LeakyBucket();
		this.RankedSortKarma = 0;
		this.HighestRankedTierReached = new Dictionary<int, Dictionary<string, int>>();
		this.FactionCompetitionData = new Dictionary<int, PlayerFactionCompetitionData>();
		this.PushToTalkKeyType = 0;
		this.PushToTalkKeyCode = 0;
		this.PushToTalkKeyName = null;
	}

	public CharacterType LastCharacter { get; set; }

	public List<CharacterType> LastRemoteCharacters { get; set; }

	public List<int> UnlockedTitleIDs { get; set; }

	public List<int> UnlockedEmojiIDs { get; set; }

	public List<int> UnlockedOverconIDs { get; set; }

	public List<int> UnlockedBannerIDs { get; set; }

	public List<int> UnlockedRibbonIDs { get; set; }

	public int SelectedTitleID { get; set; }

	public int SelectedForegroundBannerID { get; set; }

	public int SelectedBackgroundBannerID { get; set; }

	public int SelectedRibbonID { get; set; }

	public Dictionary<int, bool> UnlockedLoadingScreenBackgroundIdsToActivatedState { get; set; }

	public List<PendingPurchaseDetails> PendingPurchases { get; set; }

	public int NumGlobalCharacterLoadouts { get; set; }

	public Dictionary<AccountComponent.UIStateIdentifier, int> UIStates { get; set; }

	public int RankedSortKarma { get; set; }

	public Dictionary<int, Dictionary<string, int>> HighestRankedTierReached { get; set; }

	public Dictionary<int, PlayerFactionCompetitionData> FactionCompetitionData { get; set; }

	public bool DisplayDevTag { get; set; }

	public bool DailyQuestsAvailable { get; set; }

	public CharacterType[] FreeRotationCharacters { get; set; }

	public DateTime FreeRotationNextUpdateDate { get; set; }

	public Dictionary<int, KeyCodeData> KeyCodeMapping { get; set; }

	public int PushToTalkKeyType { get; set; }

	public int PushToTalkKeyCode { get; set; }

	public string PushToTalkKeyName { get; set; }

	public int FreelancerExpBonusGames { get; set; }

	public DateTime FreelancerExpBonusTime { get; set; }

	public static bool IsUIStateTutorialVideo(AccountComponent.UIStateIdentifier state)
	{
		if (state != AccountComponent.UIStateIdentifier.TutorialCatalysts && state != AccountComponent.UIStateIdentifier.TutorialCooldowns)
		{
			if (state != AccountComponent.UIStateIdentifier.TutorialCover)
			{
				if (state != AccountComponent.UIStateIdentifier.TutorialFoW && state != AccountComponent.UIStateIdentifier.TutorialMovement)
				{
					if (state != AccountComponent.UIStateIdentifier.TutorialOverview && state != AccountComponent.UIStateIdentifier.TutorialPhases)
					{
						if (state != AccountComponent.UIStateIdentifier.TutorialPowerups)
						{
							if (state != AccountComponent.UIStateIdentifier.TutorialRespawn)
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
		if (list.Count > pool.TotalBannersAbleToBeUnlockedAtOnce)
		{
			for (int j = 0; j < list.Count - pool.TotalBannersAbleToBeUnlockedAtOnce; j++)
			{
				Log.Info("Unlock Exclusivity Exceeded. Removing ID {0} from {1}", new object[]
				{
					list[j],
					pool.PoolType
				});
				unlockedIDs.Remove(list[j]);
			}
		}
	}

	public void ValidateSelectedBanner()
	{
		if (!this.UnlockedBannerIDs.Contains(this.SelectedBackgroundBannerID))
		{
			this.SelectedBackgroundBannerID = -1;
			for (int i = 0; i < this.UnlockedBannerIDs.Count; i++)
			{
				int num = this.UnlockedBannerIDs[i];
				if (LobbyGameplayData.Get().GameBalanceVars.GetBanner(num).m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
				{
					this.SelectedBackgroundBannerID = num;
					return;
				}
			}
		}
	}

	public void ValidateSelectedEmblem()
	{
		if (!this.UnlockedBannerIDs.Contains(this.SelectedForegroundBannerID))
		{
			this.SelectedForegroundBannerID = -1;
			for (int i = 0; i < this.UnlockedBannerIDs.Count; i++)
			{
				int num = this.UnlockedBannerIDs[i];
				if (LobbyGameplayData.Get().GameBalanceVars.GetBanner(num).m_type == GameBalanceVars.PlayerBanner.BannerType.Foreground)
				{
					this.SelectedForegroundBannerID = num;
					return;
				}
			}
		}
	}

	public void ValidateSelectedTitle()
	{
		if (!this.UnlockedTitleIDs.Contains(this.SelectedTitleID))
		{
			if (this.UnlockedTitleIDs.Count > 0)
			{
				this.SelectedTitleID = this.UnlockedTitleIDs[0];
			}
			else
			{
				this.SelectedTitleID = -1;
			}
		}
	}

	public void ValidateExclusivityPools()
	{
		if (LobbyGameplayData.Get() != null && LobbyGameplayData.Get().GameBalanceVars.ExclusivityPools != null)
		{
			foreach (GameBalanceVars.UnlockExlusivePool unlockExlusivePool in LobbyGameplayData.Get().GameBalanceVars.ExclusivityPools)
			{
				if (unlockExlusivePool.PoolType == GameBalanceVars.UnlockExlusivePool.ExclusivePoolType.Banner)
				{
					this.ValidateExclusivityPool(unlockExlusivePool, this.UnlockedBannerIDs);
					this.ValidateSelectedBanner();
					this.ValidateSelectedBanner();
				}
				else if (unlockExlusivePool.PoolType == GameBalanceVars.UnlockExlusivePool.ExclusivePoolType.Emoticon)
				{
					this.ValidateExclusivityPool(unlockExlusivePool, this.UnlockedEmojiIDs);
				}
				else if (unlockExlusivePool.PoolType == GameBalanceVars.UnlockExlusivePool.ExclusivePoolType.Overcon)
				{
					this.ValidateExclusivityPool(unlockExlusivePool, this.UnlockedOverconIDs);
				}
				else if (unlockExlusivePool.PoolType == GameBalanceVars.UnlockExlusivePool.ExclusivePoolType.Ribbon)
				{
					this.ValidateExclusivityPool(unlockExlusivePool, this.UnlockedRibbonIDs);
					if (!this.UnlockedRibbonIDs.Contains(this.SelectedRibbonID))
					{
						this.SelectedRibbonID = -1;
					}
				}
				else if (unlockExlusivePool.PoolType == GameBalanceVars.UnlockExlusivePool.ExclusivePoolType.Title)
				{
					this.ValidateExclusivityPool(unlockExlusivePool, this.UnlockedTitleIDs);
					if (!this.UnlockedTitleIDs.Contains(this.SelectedTitleID))
					{
						this.SelectedTitleID = -1;
					}
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
		for (int j = 0; j < list.Count; j++)
		{
			Log.Info("Clearing Exclusivity Poos. Removing ID {0} from {1}", new object[]
			{
				list[j],
				pool.PoolType
			});
			unlockedIDs.Remove(list[j]);
		}
	}

	public void ClearAllItemsInExclusivityPool(string clearPoolName)
	{
		if (LobbyGameplayData.Get() != null)
		{
			if (LobbyGameplayData.Get().GameBalanceVars.ExclusivityPools != null)
			{
				foreach (GameBalanceVars.UnlockExlusivePool unlockExlusivePool in LobbyGameplayData.Get().GameBalanceVars.ExclusivityPools)
				{
					if (unlockExlusivePool.PoolName == clearPoolName)
					{
						if (unlockExlusivePool.PoolType == GameBalanceVars.UnlockExlusivePool.ExclusivePoolType.Banner)
						{
							this.ClearAllItemsInExclusivityPool(unlockExlusivePool, this.UnlockedBannerIDs);
							this.ValidateSelectedBanner();
							this.ValidateSelectedBanner();
						}
						else if (unlockExlusivePool.PoolType == GameBalanceVars.UnlockExlusivePool.ExclusivePoolType.Emoticon)
						{
							this.ClearAllItemsInExclusivityPool(unlockExlusivePool, this.UnlockedEmojiIDs);
						}
						else if (unlockExlusivePool.PoolType == GameBalanceVars.UnlockExlusivePool.ExclusivePoolType.Overcon)
						{
							this.ClearAllItemsInExclusivityPool(unlockExlusivePool, this.UnlockedOverconIDs);
						}
						else if (unlockExlusivePool.PoolType == GameBalanceVars.UnlockExlusivePool.ExclusivePoolType.Ribbon)
						{
							this.ClearAllItemsInExclusivityPool(unlockExlusivePool, this.UnlockedRibbonIDs);
							if (!this.UnlockedRibbonIDs.Contains(this.SelectedRibbonID))
							{
								this.SelectedRibbonID = -1;
							}
						}
						else if (unlockExlusivePool.PoolType == GameBalanceVars.UnlockExlusivePool.ExclusivePoolType.Title)
						{
							this.ClearAllItemsInExclusivityPool(unlockExlusivePool, this.UnlockedTitleIDs);
							if (!this.UnlockedTitleIDs.Contains(this.SelectedTitleID))
							{
								this.SelectedTitleID = -1;
							}
						}
					}
				}
			}
		}
	}

	public PlayerFactionCompetitionData GetPlayerCompetitionData(int competitionID)
	{
		if (this.FactionCompetitionData.ContainsKey(competitionID))
		{
			return this.FactionCompetitionData[competitionID];
		}
		PlayerFactionCompetitionData playerFactionCompetitionData = new PlayerFactionCompetitionData();
		playerFactionCompetitionData.CompetitionID = competitionID;
		playerFactionCompetitionData.Factions = new Dictionary<int, FactionPlayerData>();
		this.FactionCompetitionData[competitionID] = playerFactionCompetitionData;
		return playerFactionCompetitionData;
	}

	public FactionPlayerData GetPlayerCompetitionFactionData(int competitionID, int factionID)
	{
		PlayerFactionCompetitionData playerCompetitionData = this.GetPlayerCompetitionData(competitionID);
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
		if (this.AppliedEntitlements.ContainsKey(entitlement))
		{
			return this.AppliedEntitlements[entitlement];
		}
		return 0;
	}

	public int GetHighestRankedTierReached(int seasonIndex, string eloKey)
	{
		if (!this.HighestRankedTierReached.ContainsKey(seasonIndex) || !this.HighestRankedTierReached[seasonIndex].ContainsKey(eloKey))
		{
			return -1;
		}
		return this.HighestRankedTierReached[seasonIndex][eloKey];
	}

	public void SetHighestRankedTierReached(int seasonIndex, string eloKey, int newTier)
	{
		if (!this.HighestRankedTierReached.ContainsKey(seasonIndex))
		{
			this.HighestRankedTierReached[seasonIndex] = new Dictionary<string, int>();
		}
		this.HighestRankedTierReached[seasonIndex][eloKey] = newTier;
	}

	public void IncrementAppliedEntitlementCount(string entitlement, int quantity = 1)
	{
		this.AppliedEntitlements[entitlement] = this.GetAppliedEntitlementCount(entitlement) + quantity;
	}

	public bool IsTitleUnlocked(int titleId)
	{
		return this.UnlockedTitleIDs.Contains(titleId);
	}

	public bool IsTitleUnlocked(GameBalanceVars.PlayerTitle title)
	{
		return this.UnlockedTitleIDs.Contains(title.ID);
	}

	public int GetCurrentTitleLevel(int titleId)
	{
		int result = 1;
		if (this.TitleLevels != null)
		{
			if (this.TitleLevels.ContainsKey(titleId))
			{
				result = this.TitleLevels[titleId];
			}
		}
		return result;
	}

	public bool IsChatEmojiUnlocked(int emojiID)
	{
		return this.UnlockedEmojiIDs.Contains(emojiID);
	}

	public bool IsChatEmojiUnlocked(GameBalanceVars.ChatEmoticon emoji)
	{
		return this.UnlockedEmojiIDs.Contains(emoji.ID);
	}

	public bool IsOverconUnlocked(int overconID)
	{
		return this.UnlockedOverconIDs.Contains(overconID);
	}

	public bool IsBannerUnlocked(int bannerId)
	{
		return this.UnlockedBannerIDs.Contains(bannerId);
	}

	public bool IsBannerUnlocked(GameBalanceVars.PlayerBanner banner)
	{
		return this.UnlockedBannerIDs.Contains(banner.ID);
	}

	public bool IsRibbonUnlocked(int ribbonId)
	{
		return this.UnlockedRibbonIDs.Contains(ribbonId);
	}

	public bool IsRibbonUnlocked(GameBalanceVars.PlayerRibbon ribbon)
	{
		return this.UnlockedRibbonIDs.Contains(ribbon.ID);
	}

	public bool IsLoadingScreenBackgroundUnlocked(int loadingScreenBackgroundId)
	{
		return this.UnlockedLoadingScreenBackgroundIdsToActivatedState.ContainsKey(loadingScreenBackgroundId);
	}

	public bool IsLoadingScreenBackgroundActive(int loadingScreenBackgroundId)
	{
		bool result;
		this.UnlockedLoadingScreenBackgroundIdsToActivatedState.TryGetValue(loadingScreenBackgroundId, out result);
		return result;
	}

	public bool ToggleLoadingScreenBackgroundActive(int loadingScreenBackgroundId, bool newState)
	{
		if (!this.IsLoadingScreenBackgroundUnlocked(loadingScreenBackgroundId))
		{
			return false;
		}
		this.UnlockedLoadingScreenBackgroundIdsToActivatedState[loadingScreenBackgroundId] = newState;
		return true;
	}

	public bool UnlockLoadingScreenBackground(int loadingScreenBackgroundId)
	{
		if (this.IsLoadingScreenBackgroundUnlocked(loadingScreenBackgroundId))
		{
			return false;
		}
		this.UnlockedLoadingScreenBackgroundIdsToActivatedState[loadingScreenBackgroundId] = true;
		return true;
	}

	public bool IsCharacterInFreeRotation(CharacterType characterType)
	{
		bool result;
		if (this.FreeRotationCharacters != null)
		{
			result = this.FreeRotationCharacters.Contains(characterType);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public int GetUIState(AccountComponent.UIStateIdentifier uiState)
	{
		if (uiState == AccountComponent.UIStateIdentifier.NONE)
		{
			throw new Exception("UI State not specified!");
		}
		if (this.UIStates.ContainsKey(uiState))
		{
			return this.UIStates[uiState];
		}
		return 0;
	}

	public object Clone()
	{
		return base.MemberwiseClone();
	}

	public enum UIStateIdentifier
	{
		HasResetMods,
		HasSeenTrustWarEndPopup,
		HasSeenSeasonTwoChapterTwo,
		HasSeenFactionWarSeasonTwoChapterTwo,
		TutorialOverview,
		TutorialPhases,
		TutorialMovement,
		TutorialCatalysts,
		TutorialCooldowns,
		TutorialPowerups,
		TutorialCover,
		TutorialFoW,
		TutorialRespawn,
		HasSeenSeasonTwoChapterThree,
		HasSeenSeasonTwoChapterFour,
		HasSeenSeasonTwoChapterFive,
		CashShopFeaturedItemsVersionViewed,
		NumLootMatrixesOpened,
		HasViewedFluxHighlight,
		HasViewedGGHighlight,
		NumDailiesChosen,
		HasSeenSeasonFourChapterOne,
		HasViewedFreelancerTokenHighlight,
		NONE = 0x2710
	}
}
