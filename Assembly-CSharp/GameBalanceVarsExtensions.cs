using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameBalanceVarsExtensions
{
	private const string kFallbackSprite = "QuestRewards/general";

	private const string kTauntIconPath = "QuestRewards/taunt";

	private const string kVfxIconPath = "QuestRewards/vfxicon";

	private const string kModIconPath = "QuestRewards/modicon";

	public static bool IsUnlockConditionMet(GameBalanceVars.UnlockCondition unlockCondition, GameBalanceVars.UnlockConditionValue unlockConditionValue)
	{
		switch (unlockCondition.ConditionType)
		{
		case GameBalanceVars.UnlockData.UnlockType.CharacterLevel:
			if (unlockCondition.typeSpecificData == unlockConditionValue.typeSpecificData && unlockCondition.typeSpecificData2 != 0)
			{
				if (unlockCondition.typeSpecificData2 <= unlockConditionValue.typeSpecificData2)
				{
					return true;
				}
			}
			break;
		case GameBalanceVars.UnlockData.UnlockType.PlayerLevel:
			if (unlockCondition.typeSpecificData != 0 && unlockCondition.typeSpecificData <= unlockConditionValue.typeSpecificData)
			{
				return true;
			}
			break;
		case GameBalanceVars.UnlockData.UnlockType.Purchase:
			if (unlockCondition.typeSpecificData <= 0)
			{
				if (unlockCondition.typeSpecificData2 <= 0)
				{
					if (unlockCondition.typeSpecificData3 <= 0)
					{
						break;
					}
				}
			}
			if (unlockCondition.typeSpecificData <= unlockConditionValue.typeSpecificData)
			{
				if (unlockCondition.typeSpecificData2 <= unlockConditionValue.typeSpecificData2)
				{
					if (unlockCondition.typeSpecificData3 <= unlockConditionValue.typeSpecificData3)
					{
						return true;
					}
				}
			}
			break;
		case GameBalanceVars.UnlockData.UnlockType.ELO:
			if (unlockCondition.typeSpecificData != 0 && unlockCondition.typeSpecificData <= unlockConditionValue.typeSpecificData)
			{
				return true;
			}
			break;
		case GameBalanceVars.UnlockData.UnlockType.HasDateTimePassed:
		{
			DateTime t = new DateTime(unlockCondition.typeSpecificDate[0], unlockCondition.typeSpecificDate[1], unlockCondition.typeSpecificDate[2], unlockCondition.typeSpecificDate[3], unlockCondition.typeSpecificDate[4], unlockCondition.typeSpecificDate[5]);
			DateTime t2 = default(DateTime);
			t2 = ClientGameManager.Get().PacificNow();
			if (t2 > t)
			{
				return true;
			}
			break;
		}
		case GameBalanceVars.UnlockData.UnlockType.FactionTierReached:
			if (unlockCondition.typeSpecificData != 0)
			{
				if (unlockCondition.typeSpecificData == unlockConditionValue.typeSpecificData)
				{
					if (unlockCondition.typeSpecificData2 == unlockConditionValue.typeSpecificData2)
					{
						if (unlockCondition.typeSpecificData3 != 0 && unlockCondition.typeSpecificData3 <= unlockConditionValue.typeSpecificData3)
						{
							return true;
						}
					}
				}
			}
			break;
		case GameBalanceVars.UnlockData.UnlockType.TitleLevelReached:
		{
			int typeSpecificData = unlockCondition.typeSpecificData;
			int typeSpecificData2 = unlockCondition.typeSpecificData2;
			if (typeSpecificData > 0)
			{
				if (typeSpecificData == unlockConditionValue.typeSpecificData)
				{
					if (typeSpecificData2 <= unlockConditionValue.typeSpecificData2)
					{
						return true;
					}
				}
			}
			break;
		}
		case GameBalanceVars.UnlockData.UnlockType.CurrentSeason:
		{
			int typeSpecificData3 = unlockCondition.typeSpecificData;
			if (typeSpecificData3 == unlockConditionValue.typeSpecificData)
			{
				return true;
			}
			break;
		}
		}
		return false;
	}

	public static bool IsUnlockConditionMet(this GameBalanceVars.UnlockData unlockData, GameBalanceVars.UnlockData.UnlockType unlockConditionType, int typeSpecificData, int typeSpecificData2 = 0, int typeSpecificData3 = 0, string typeSpecificString = "")
	{
		if (!unlockData.UnlockConditions.IsNullOrEmpty<GameBalanceVars.UnlockCondition>() && unlockData.UnlockConditions.Length <= 1)
		{
			if (unlockData.UnlockConditions[0].ConditionType == unlockConditionType)
			{
				GameBalanceVars.UnlockCondition unlockCondition = unlockData.UnlockConditions[0];
				return GameBalanceVarsExtensions.IsUnlockConditionMet(unlockCondition, new GameBalanceVars.UnlockConditionValue
				{
					typeSpecificData = typeSpecificData,
					typeSpecificData2 = typeSpecificData2,
					typeSpecificData3 = typeSpecificData3,
					typeSpecificString = typeSpecificString
				});
			}
		}
		return false;
	}

	public static bool ArePurchaseableConditionsMet(this GameBalanceVars.UnlockData unlockData, List<GameBalanceVars.UnlockConditionValue> unlockConditionValues, List<GameBalanceVars.UnlockData.UnlockType> ignoreUnlockTypes)
	{
		if (unlockData.PurchaseableConditions.IsNullOrEmpty<GameBalanceVars.UnlockCondition>())
		{
			return false;
		}
		if (!unlockConditionValues.IsNullOrEmpty<GameBalanceVars.UnlockConditionValue>())
		{
			if (unlockData.PurchaseableConditions.Length != unlockConditionValues.Count)
			{
			}
			else
			{
				List<bool> list = new List<bool>(unlockData.PurchaseableConditions.Length);
				for (int i = 0; i < unlockData.PurchaseableConditions.Length; i++)
				{
					list.Add(false);
				}
				int j = 0;
				while (j < unlockData.PurchaseableConditions.Length)
				{
					if (ignoreUnlockTypes.IsNullOrEmpty<GameBalanceVars.UnlockData.UnlockType>())
					{
						goto IL_B5;
					}
					if (!ignoreUnlockTypes.Contains(unlockData.PurchaseableConditions[j].ConditionType))
					{
						goto IL_B5;
					}
					list[j] = true;
					IL_D2:
					j++;
					continue;
					IL_B5:
					list[j] = GameBalanceVarsExtensions.IsUnlockConditionMet(unlockData.PurchaseableConditions[j], unlockConditionValues[j]);
					goto IL_D2;
				}
				if (unlockData.PurchaseableLogicStatement.IsNullOrEmpty())
				{
					bool result = true;
					using (List<bool>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (!enumerator.Current)
							{
								result = false;
							}
						}
					}
					return result;
				}
				LogicOpClass logicOpClass = LogicStatement.EvaluateLogicStatement(unlockData.PurchaseableLogicStatement);
				return logicOpClass.GetValue(list);
			}
		}
		return false;
	}

	public static bool MeetsPurchaseabilityConditions(GameBalanceVars.PlayerUnlockable unlockable)
	{
		List<GameBalanceVars.UnlockConditionValue> list = new List<GameBalanceVars.UnlockConditionValue>();
		if (unlockable != null)
		{
			if (unlockable.m_unlockData == null)
			{
			}
			else
			{
				if (unlockable.m_unlockData.PurchaseableConditions.IsNullOrEmpty<GameBalanceVars.UnlockCondition>())
				{
					return true;
				}
				for (int i = 0; i < unlockable.m_unlockData.PurchaseableConditions.Length; i++)
				{
					list.Add(new GameBalanceVars.UnlockConditionValue
					{
						ConditionType = unlockable.m_unlockData.PurchaseableConditions[i].ConditionType
					});
				}
				for (int j = 0; j < unlockable.m_unlockData.PurchaseableConditions.Length; j++)
				{
					GameBalanceVars.UnlockCondition unlockCondition = unlockable.m_unlockData.PurchaseableConditions[j];
					GameBalanceVars.UnlockConditionValue unlockConditionValue = list[j];
					switch (unlockCondition.ConditionType)
					{
					case GameBalanceVars.UnlockData.UnlockType.CharacterLevel:
					{
						PersistedCharacterData persistedCharacterData = ClientGameManager.Get().GetAllPlayerCharacterData().TryGetValue((CharacterType)unlockCondition.typeSpecificData);
						if (persistedCharacterData != null)
						{
							unlockConditionValue.typeSpecificData = unlockCondition.typeSpecificData;
							unlockConditionValue.typeSpecificData2 = persistedCharacterData.ExperienceComponent.Level;
						}
						break;
					}
					case GameBalanceVars.UnlockData.UnlockType.PlayerLevel:
						unlockConditionValue.typeSpecificData = ClientGameManager.Get().GetPlayerAccountData().ExperienceComponent.Level;
						break;
					case GameBalanceVars.UnlockData.UnlockType.ELO:
						unlockConditionValue.typeSpecificData = 0;
						break;
					case GameBalanceVars.UnlockData.UnlockType.Quest:
						unlockConditionValue.typeSpecificData = unlockCondition.typeSpecificData;
						QuestItem.GetQuestProgress(unlockCondition.typeSpecificData, out unlockConditionValue.typeSpecificData2, out unlockConditionValue.typeSpecificData3);
						break;
					case GameBalanceVars.UnlockData.UnlockType.FactionTierReached:
					{
						ClientGameManager clientGameManager = ClientGameManager.Get();
						FactionWideData factionWideData = FactionWideData.Get();
						unlockConditionValue.typeSpecificData = unlockCondition.typeSpecificData;
						unlockConditionValue.typeSpecificData2 = unlockCondition.typeSpecificData2;
						long factionScore;
						if (clientGameManager.ActiveFactionCompetition == unlockCondition.typeSpecificData && clientGameManager.FactionScores.TryGetValue(unlockCondition.typeSpecificData2, out factionScore))
						{
							unlockConditionValue.typeSpecificData3 = factionWideData.GetCompetitionFactionTierReached(unlockCondition.typeSpecificData, unlockCondition.typeSpecificData2, factionScore);
						}
						break;
					}
					case GameBalanceVars.UnlockData.UnlockType.TitleLevelReached:
					{
						PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
						int typeSpecificData = unlockCondition.typeSpecificData;
						int currentTitleLevel = playerAccountData.AccountComponent.GetCurrentTitleLevel(typeSpecificData);
						unlockConditionValue.typeSpecificData = typeSpecificData;
						unlockConditionValue.typeSpecificData2 = currentTitleLevel;
						break;
					}
					case GameBalanceVars.UnlockData.UnlockType.CurrentSeason:
						unlockConditionValue.typeSpecificData = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
						break;
					}
				}
				List<GameBalanceVars.UnlockData.UnlockType> ignoreUnlockTypes = new List<GameBalanceVars.UnlockData.UnlockType>();
				return unlockable.m_unlockData.ArePurchaseableConditionsMet(list, ignoreUnlockTypes);
			}
		}
		return false;
	}

	public static bool MeetsVisibilityConditions(GameBalanceVars.PlayerUnlockable unlockable)
	{
		List<GameBalanceVars.UnlockConditionValue> list = new List<GameBalanceVars.UnlockConditionValue>();
		if (unlockable != null)
		{
			if (unlockable.m_unlockData == null)
			{
			}
			else
			{
				if (unlockable.m_unlockData.VisibilityConditions.IsNullOrEmpty<GameBalanceVars.UnlockCondition>())
				{
					return true;
				}
				for (int i = 0; i < unlockable.m_unlockData.VisibilityConditions.Length; i++)
				{
					list.Add(new GameBalanceVars.UnlockConditionValue
					{
						ConditionType = unlockable.m_unlockData.VisibilityConditions[i].ConditionType
					});
				}
				for (int j = 0; j < unlockable.m_unlockData.VisibilityConditions.Length; j++)
				{
					GameBalanceVars.UnlockCondition unlockCondition = unlockable.m_unlockData.VisibilityConditions[j];
					GameBalanceVars.UnlockConditionValue unlockConditionValue = list[j];
					switch (unlockCondition.ConditionType)
					{
					case GameBalanceVars.UnlockData.UnlockType.CharacterLevel:
					{
						PersistedCharacterData persistedCharacterData = ClientGameManager.Get().GetAllPlayerCharacterData().TryGetValue((CharacterType)unlockCondition.typeSpecificData);
						if (persistedCharacterData != null)
						{
							unlockConditionValue.typeSpecificData = unlockCondition.typeSpecificData;
							unlockConditionValue.typeSpecificData2 = persistedCharacterData.ExperienceComponent.Level;
						}
						break;
					}
					case GameBalanceVars.UnlockData.UnlockType.PlayerLevel:
						unlockConditionValue.typeSpecificData = ClientGameManager.Get().GetPlayerAccountData().ExperienceComponent.Level;
						break;
					case GameBalanceVars.UnlockData.UnlockType.ELO:
						unlockConditionValue.typeSpecificData = 0;
						break;
					case GameBalanceVars.UnlockData.UnlockType.Quest:
						unlockConditionValue.typeSpecificData = unlockCondition.typeSpecificData;
						QuestItem.GetQuestProgress(unlockCondition.typeSpecificData, out unlockConditionValue.typeSpecificData2, out unlockConditionValue.typeSpecificData3);
						break;
					case GameBalanceVars.UnlockData.UnlockType.FactionTierReached:
					{
						ClientGameManager clientGameManager = ClientGameManager.Get();
						FactionWideData factionWideData = FactionWideData.Get();
						unlockConditionValue.typeSpecificData = unlockCondition.typeSpecificData;
						unlockConditionValue.typeSpecificData2 = unlockCondition.typeSpecificData2;
						long factionScore;
						if (clientGameManager.ActiveFactionCompetition == unlockCondition.typeSpecificData && clientGameManager.FactionScores.TryGetValue(unlockCondition.typeSpecificData2, out factionScore))
						{
							unlockConditionValue.typeSpecificData3 = factionWideData.GetCompetitionFactionTierReached(unlockCondition.typeSpecificData, unlockCondition.typeSpecificData2, factionScore);
						}
						break;
					}
					case GameBalanceVars.UnlockData.UnlockType.TitleLevelReached:
					{
						PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
						int typeSpecificData = unlockCondition.typeSpecificData;
						int currentTitleLevel = playerAccountData.AccountComponent.GetCurrentTitleLevel(typeSpecificData);
						unlockConditionValue.typeSpecificData = typeSpecificData;
						unlockConditionValue.typeSpecificData2 = currentTitleLevel;
						break;
					}
					case GameBalanceVars.UnlockData.UnlockType.CurrentSeason:
						unlockConditionValue.typeSpecificData = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
						break;
					}
				}
				List<GameBalanceVars.UnlockData.UnlockType> ignoreUnlockTypes = new List<GameBalanceVars.UnlockData.UnlockType>();
				return unlockable.m_unlockData.AreVisibilityConditionsMet(list, ignoreUnlockTypes);
			}
		}
		return false;
	}

	public static bool AreVisibilityConditionsMet(this GameBalanceVars.UnlockData unlockData, List<GameBalanceVars.UnlockConditionValue> unlockConditionValues, List<GameBalanceVars.UnlockData.UnlockType> ignoreUnlockTypes)
	{
		if (unlockData.VisibilityConditions.IsNullOrEmpty<GameBalanceVars.UnlockCondition>())
		{
			return false;
		}
		if (unlockConditionValues.IsNullOrEmpty<GameBalanceVars.UnlockConditionValue>() || unlockData.VisibilityConditions.Length != unlockConditionValues.Count)
		{
			return false;
		}
		List<bool> list = new List<bool>(unlockData.VisibilityConditions.Length);
		for (int i = 0; i < unlockData.VisibilityConditions.Length; i++)
		{
			list.Add(false);
		}
		for (int j = 0; j < unlockData.VisibilityConditions.Length; j++)
		{
			if (!ignoreUnlockTypes.IsNullOrEmpty<GameBalanceVars.UnlockData.UnlockType>() && ignoreUnlockTypes.Contains(unlockData.VisibilityConditions[j].ConditionType))
			{
				list[j] = true;
			}
			else
			{
				list[j] = GameBalanceVarsExtensions.IsUnlockConditionMet(unlockData.VisibilityConditions[j], unlockConditionValues[j]);
			}
		}
		if (unlockData.VisibilityLogicStatement.IsNullOrEmpty())
		{
			bool result = true;
			using (List<bool>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current)
					{
						result = false;
					}
				}
			}
			return result;
		}
		LogicOpClass logicOpClass = LogicStatement.EvaluateLogicStatement(unlockData.VisibilityLogicStatement);
		return logicOpClass.GetValue(list);
	}

	public static bool AreUnlockConditionsMet(this GameBalanceVars.UnlockData unlockData, List<GameBalanceVars.UnlockConditionValue> unlockConditionValues, List<GameBalanceVars.UnlockData.UnlockType> ignoreUnlockTypes)
	{
		if (unlockData.UnlockConditions.IsNullOrEmpty<GameBalanceVars.UnlockCondition>())
		{
			return false;
		}
		if (!unlockConditionValues.IsNullOrEmpty<GameBalanceVars.UnlockConditionValue>())
		{
			if (unlockData.UnlockConditions.Length != unlockConditionValues.Count)
			{
			}
			else
			{
				List<bool> list = new List<bool>(unlockData.UnlockConditions.Length);
				for (int i = 0; i < unlockData.UnlockConditions.Length; i++)
				{
					list.Add(false);
				}
				for (int j = 0; j < unlockData.UnlockConditions.Length; j++)
				{
					if (!ignoreUnlockTypes.IsNullOrEmpty<GameBalanceVars.UnlockData.UnlockType>() && ignoreUnlockTypes.Contains(unlockData.UnlockConditions[j].ConditionType))
					{
						list[j] = true;
					}
					else
					{
						list[j] = GameBalanceVarsExtensions.IsUnlockConditionMet(unlockData.UnlockConditions[j], unlockConditionValues[j]);
					}
				}
				if (unlockData.LogicStatement.IsNullOrEmpty())
				{
					bool result = true;
					using (List<bool>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (!enumerator.Current)
							{
								result = false;
							}
						}
					}
					return result;
				}
				LogicOpClass logicOpClass = LogicStatement.EvaluateLogicStatement(unlockData.LogicStatement);
				return logicOpClass.GetValue(list);
			}
		}
		return false;
	}

	public static bool AreUnlockConditionsMet(this GameBalanceVars.UnlockData unlockData, List<GameBalanceVars.UnlockConditionValue> unlockConditionValues, bool ignorePurchaseCondition)
	{
		if (unlockData.UnlockConditions.IsNullOrEmpty<GameBalanceVars.UnlockCondition>())
		{
			return false;
		}
		List<GameBalanceVars.UnlockData.UnlockType> list = new List<GameBalanceVars.UnlockData.UnlockType>();
		if (ignorePurchaseCondition)
		{
			list.Add(GameBalanceVars.UnlockData.UnlockType.Purchase);
		}
		return unlockData.AreUnlockConditionsMet(unlockConditionValues, list);
	}

	public static CharacterType GetUnlockCharacterType(this GameBalanceVars.PlayerUnlockable playerUnlockable)
	{
		GameBalanceVars.UnlockData unlockData = playerUnlockable.m_unlockData;
		if (unlockData != null)
		{
			if (!unlockData.UnlockConditions.IsNullOrEmpty<GameBalanceVars.UnlockCondition>())
			{
				foreach (GameBalanceVars.UnlockCondition unlockCondition in unlockData.UnlockConditions)
				{
					if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.CharacterLevel)
					{
						return (CharacterType)unlockCondition.typeSpecificData;
					}
				}
				return CharacterType.None;
			}
		}
		return CharacterType.None;
	}

	public static int GetUnlockPlayerLevel(this GameBalanceVars.PlayerUnlockable playerUnlockable)
	{
		GameBalanceVars.UnlockData unlockData = playerUnlockable.m_unlockData;
		if (unlockData != null)
		{
			if (!unlockData.UnlockConditions.IsNullOrEmpty<GameBalanceVars.UnlockCondition>())
			{
				foreach (GameBalanceVars.UnlockCondition unlockCondition in unlockData.UnlockConditions)
				{
					if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.PlayerLevel)
					{
						return unlockCondition.typeSpecificData;
					}
				}
				return 0;
			}
		}
		return 0;
	}

	public static int GetUnlockCharacterLevel(this GameBalanceVars.PlayerUnlockable playerUnlockable, CharacterType characterType, bool checkForPurchaseableAfterLevel = false)
	{
		GameBalanceVars.UnlockData unlockData = playerUnlockable.m_unlockData;
		if (unlockData != null)
		{
			if (unlockData.UnlockConditions.IsNullOrEmpty<GameBalanceVars.UnlockCondition>())
			{
			}
			else
			{
				if (unlockData.UnlockConditions.Length != 1)
				{
					return 0;
				}
				foreach (GameBalanceVars.UnlockCondition unlockCondition in unlockData.UnlockConditions)
				{
					if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.CharacterLevel)
					{
						if (unlockCondition.typeSpecificData == (int)characterType)
						{
							return unlockCondition.typeSpecificData2;
						}
					}
				}
				return 0;
			}
		}
		return 0;
	}

	public static int GetUnlockISOPrice(this GameBalanceVars.PlayerUnlockable playerUnlockable)
	{
		GameBalanceVars.UnlockData unlockData = playerUnlockable.m_unlockData;
		if (unlockData != null)
		{
			if (!unlockData.UnlockConditions.IsNullOrEmpty<GameBalanceVars.UnlockCondition>())
			{
				foreach (GameBalanceVars.UnlockCondition unlockCondition in unlockData.UnlockConditions)
				{
					if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.Purchase)
					{
						return unlockCondition.typeSpecificData2;
					}
				}
				return 0;
			}
		}
		return 0;
	}

	public static int GetUnlockRankedCurrencyPrice(this GameBalanceVars.PlayerUnlockable playerUnlockable)
	{
		GameBalanceVars.UnlockData unlockData = playerUnlockable.m_unlockData;
		if (unlockData == null || unlockData.UnlockConditions.IsNullOrEmpty<GameBalanceVars.UnlockCondition>())
		{
			return 0;
		}
		foreach (GameBalanceVars.UnlockCondition unlockCondition in unlockData.UnlockConditions)
		{
			if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.Purchase)
			{
				return unlockCondition.typeSpecificData;
			}
		}
		return 0;
	}

	public static int GetUnlockFreelancerCurrencyPrice(this GameBalanceVars.PlayerUnlockable playerUnlockable)
	{
		GameBalanceVars.UnlockData unlockData = playerUnlockable.m_unlockData;
		if (unlockData != null)
		{
			if (!unlockData.UnlockConditions.IsNullOrEmpty<GameBalanceVars.UnlockCondition>())
			{
				foreach (GameBalanceVars.UnlockCondition unlockCondition in unlockData.UnlockConditions)
				{
					if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.Purchase)
					{
						return unlockCondition.typeSpecificData3;
					}
				}
				return 0;
			}
		}
		return 0;
	}

	public static float GetRealCurrencyPrice(this GameBalanceVars.PlayerUnlockable playerUnlockable)
	{
		if (playerUnlockable.Prices != null)
		{
			return playerUnlockable.Prices.GetPrice(HydrogenConfig.Get().Ticket.AccountCurrency);
		}
		return 0f;
	}

	public static bool CanStillPurchaseIfOwned(this GameBalanceVars.PlayerUnlockable unlockable)
	{
		if (unlockable is GameBalanceVars.PlayerTitle)
		{
			GameBalanceVars.PlayerTitle title = unlockable as GameBalanceVars.PlayerTitle;
			return !ClientGameManager.Get().IsTitleAtMaxLevel(title);
		}
		return unlockable is GameBalanceVars.StoreItemForPurchase;
	}

	public static bool IsOwned(this GameBalanceVars.PlayerUnlockable unlockable)
	{
		if (unlockable is GameBalanceVars.PlayerTitle)
		{
			return ClientGameManager.Get().IsTitleUnlocked(unlockable as GameBalanceVars.PlayerTitle);
		}
		if (unlockable is GameBalanceVars.PlayerBanner)
		{
			GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(unlockable.ID);
			List<GameBalanceVars.UnlockConditionValue> list;
			return ClientGameManager.Get().IsBannerUnlocked(banner, out list);
		}
		if (unlockable is GameBalanceVars.SkinUnlockData)
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData((CharacterType)unlockable.Index1);
			return playerCharacterData != null && playerCharacterData.CharacterComponent.GetSkin(unlockable.ID).Unlocked;
		}
		if (unlockable is GameBalanceVars.PatternUnlockData)
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData((CharacterType)unlockable.Index1);
			if (playerCharacterData == null)
			{
				return false;
			}
			return playerCharacterData.CharacterComponent.GetSkin(unlockable.Index2).GetPattern(unlockable.ID).Unlocked;
		}
		else if (unlockable is GameBalanceVars.ColorUnlockData)
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData((CharacterType)unlockable.Index1);
			if (playerCharacterData == null)
			{
				return false;
			}
			return playerCharacterData.CharacterComponent.GetSkin(unlockable.Index2).GetPattern(unlockable.Index3).GetColor(unlockable.ID).Unlocked;
		}
		else
		{
			if (unlockable is GameBalanceVars.TauntUnlockData)
			{
				PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData((CharacterType)unlockable.Index1);
				return playerCharacterData.CharacterComponent.GetTaunt(unlockable.ID).Unlocked;
			}
			if (unlockable is GameBalanceVars.AbilityModUnlockData)
			{
				bool result = false;
				PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData((CharacterType)unlockable.Index1);
				if (playerCharacterData != null)
				{
					if (!playerCharacterData.CharacterComponent.IsModUnlocked(unlockable.Index2, unlockable.ID))
					{
						if (!GameManager.Get().GameplayOverrides.EnableAllMods)
						{
							return false;
						}
					}
					result = true;
				}
				return result;
			}
			if (unlockable is GameBalanceVars.ChatEmoticon)
			{
				return ClientGameManager.Get().IsEmojiUnlocked(unlockable as GameBalanceVars.ChatEmoticon);
			}
			if (unlockable is GameBalanceVars.OverconUnlockData)
			{
				return ClientGameManager.Get().IsOverconUnlocked(unlockable.ID);
			}
			if (unlockable is GameBalanceVars.StoreItemForPurchase)
			{
				return false;
			}
			if (unlockable is GameBalanceVars.AbilityVfxUnlockData)
			{
				PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData((CharacterType)unlockable.Index1);
				if (playerCharacterData == null)
				{
					return false;
				}
				return playerCharacterData.CharacterComponent.IsAbilityVfxSwapUnlocked(unlockable.Index2, unlockable.ID);
			}
			else
			{
				if (unlockable is GameBalanceVars.PlayerRibbon)
				{
					List<GameBalanceVars.UnlockConditionValue> list2;
					return ClientGameManager.Get().IsRibbonUnlocked(unlockable as GameBalanceVars.PlayerRibbon, out list2);
				}
				if (unlockable is GameBalanceVars.LoadingScreenBackground)
				{
					return ClientGameManager.Get().IsLoadingScreenBackgroundUnlocked(unlockable.ID);
				}
				throw new Exception("Not implemented");
			}
		}
	}

	public static string GetSpritePath(this GameBalanceVars.PlayerUnlockable unlockable)
	{
		if (unlockable == null)
		{
			return string.Empty;
		}
		if (unlockable is GameBalanceVars.PlayerBanner)
		{
			GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(unlockable.ID);
			if (banner != null)
			{
				if (!banner.m_iconResourceString.IsNullOrEmpty())
				{
					return banner.m_iconResourceString;
				}
			}
			return "Banners/Background/02_blue";
		}
		if (unlockable is GameBalanceVars.ChatEmoticon)
		{
			return (unlockable as GameBalanceVars.ChatEmoticon).IconPath;
		}
		if (unlockable is GameBalanceVars.OverconUnlockData)
		{
			if (UIOverconData.Get() == null)
			{
				throw new Exception("UIOverconData doesn't exist");
			}
			UIOverconData.NameToOverconEntry nameToOverconEntry = null;
			int i = 0;
			while (i < UIOverconData.Get().m_nameToOverconEntry.Count)
			{
				if (UIOverconData.Get().m_nameToOverconEntry[i].m_overconId == unlockable.ID)
				{
					nameToOverconEntry = UIOverconData.Get().m_nameToOverconEntry[i];
					IL_10F:
					if (nameToOverconEntry == null)
					{
						throw new Exception("Overcon doesn't exist for " + unlockable.ID);
					}
					return nameToOverconEntry.m_iconSpritePath;
				}
				else
				{
					i++;
				}
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				goto IL_10F;
			}
		}
		else if (unlockable is GameBalanceVars.StoreItemForPurchase)
		{
			InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate((unlockable as GameBalanceVars.StoreItemForPurchase).m_itemTemplateId);
			if (itemTemplate == null)
			{
				throw new Exception("inventoryItem " + (unlockable as GameBalanceVars.StoreItemForPurchase).m_itemTemplateId + " doesn't exist");
			}
			return itemTemplate.IconPath;
		}
		else
		{
			if (unlockable is GameBalanceVars.AbilityModUnlockData)
			{
				return "QuestRewards/modicon";
			}
			if (unlockable is GameBalanceVars.SkinUnlockData)
			{
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)unlockable.Index1);
				if (characterResourceLink == null)
				{
					throw new Exception("Character " + (CharacterType)unlockable.Index1 + " doesn't exist");
				}
				if (characterResourceLink.m_skins.Count <= unlockable.ID)
				{
					throw new Exception(string.Concat(new object[]
					{
						"Skin index ",
						unlockable.ID,
						" for ",
						(CharacterType)unlockable.Index1,
						" missing"
					}));
				}
				return characterResourceLink.m_skins[unlockable.ID].m_skinSelectionIconPath;
			}
			else if (unlockable is GameBalanceVars.ColorUnlockData)
			{
				CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink((CharacterType)unlockable.Index1);
				if (characterResourceLink2 == null)
				{
					throw new Exception("Character " + (CharacterType)unlockable.Index1 + " doesn't exist");
				}
				if (characterResourceLink2.m_skins.Count <= unlockable.Index2)
				{
					throw new Exception(string.Concat(new object[]
					{
						"Skin index ",
						unlockable.Index2,
						" for ",
						(CharacterType)unlockable.Index1,
						" missing"
					}));
				}
				CharacterSkin characterSkin = characterResourceLink2.m_skins[unlockable.Index2];
				if (characterSkin.m_patterns.Count <= unlockable.Index3)
				{
					throw new Exception(string.Concat(new object[]
					{
						"Pattern index of ",
						unlockable.Index3,
						" of skin index ",
						unlockable.Index2,
						" for ",
						(CharacterType)unlockable.Index1,
						" missing"
					}));
				}
				CharacterPattern characterPattern = characterSkin.m_patterns[unlockable.Index3];
				if (characterPattern.m_colors.Count <= unlockable.ID)
				{
					throw new Exception(string.Concat(new object[]
					{
						"Color index ",
						unlockable.ID,
						" of Pattern index of ",
						unlockable.Index3,
						" of skin index ",
						unlockable.Index2,
						" for ",
						(CharacterType)unlockable.Index1,
						" missing"
					}));
				}
				return characterPattern.m_colors[unlockable.ID].m_iconResourceString;
			}
			else
			{
				if (unlockable is GameBalanceVars.TauntUnlockData)
				{
					return "QuestRewards/taunt";
				}
				if (unlockable is GameBalanceVars.PlayerTitle)
				{
					return "QuestRewards/general";
				}
				if (unlockable is GameBalanceVars.AbilityVfxUnlockData)
				{
					return "QuestRewards/vfxicon";
				}
				if (unlockable is GameBalanceVars.PlayerRibbon)
				{
					GameBalanceVars.PlayerRibbon playerRibbon = unlockable as GameBalanceVars.PlayerRibbon;
					return playerRibbon.m_resourceIconString;
				}
				if (unlockable is GameBalanceVars.LoadingScreenBackground)
				{
					GameBalanceVars.LoadingScreenBackground loadingScreenBackground = unlockable as GameBalanceVars.LoadingScreenBackground;
					return loadingScreenBackground.m_iconPath;
				}
				throw new Exception("Sprite Not Implemented for " + unlockable.GetType());
			}
		}
	}

	public static Sprite GetItemFg(this GameBalanceVars.PlayerUnlockable unlockable)
	{
		if (unlockable == null)
		{
			return null;
		}
		AbilityData component;
		AbilityData.ActionType actionType;
		if (unlockable is GameBalanceVars.TauntUnlockData)
		{
			CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)unlockable.Index1);
			component = characterResourceLink.ActorDataPrefab.GetComponent<AbilityData>();
			actionType = characterResourceLink.m_taunts[unlockable.ID].m_actionForTaunt;
		}
		else
		{
			if (!(unlockable is GameBalanceVars.AbilityVfxUnlockData))
			{
				if (!(unlockable is GameBalanceVars.AbilityModUnlockData))
				{
					return null;
				}
			}
			CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)unlockable.Index1);
			component = characterResourceLink.ActorDataPrefab.GetComponent<AbilityData>();
			actionType = (AbilityData.ActionType)unlockable.Index2;
		}
		if (actionType == AbilityData.ActionType.ABILITY_0)
		{
			return component.m_sprite0;
		}
		if (actionType == AbilityData.ActionType.ABILITY_1)
		{
			return component.m_sprite1;
		}
		if (actionType == AbilityData.ActionType.ABILITY_2)
		{
			return component.m_sprite2;
		}
		if (actionType == AbilityData.ActionType.ABILITY_3)
		{
			return component.m_sprite3;
		}
		if (actionType == AbilityData.ActionType.ABILITY_4)
		{
			return component.m_sprite4;
		}
		if (actionType == AbilityData.ActionType.ABILITY_5)
		{
			return component.m_sprite5;
		}
		if (actionType == AbilityData.ActionType.ABILITY_6)
		{
			return component.m_sprite6;
		}
		return null;
	}

	public static GameBalanceVars.AbilityModUnlockData GetAbilityModUnlockData(this AbilityMod mod, CharacterType type, int abilityId)
	{
		return new GameBalanceVars.AbilityModUnlockData
		{
			Index1 = (int)type,
			Index2 = abilityId,
			ID = mod.m_abilityScopeId,
			m_unlockData = new GameBalanceVars.UnlockData()
		};
	}
}
