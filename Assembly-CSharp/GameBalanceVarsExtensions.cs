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
		case GameBalanceVars.UnlockData.UnlockType.HasDateTimePassed:
		{
			DateTime t = new DateTime(unlockCondition.typeSpecificDate[0], unlockCondition.typeSpecificDate[1], unlockCondition.typeSpecificDate[2], unlockCondition.typeSpecificDate[3], unlockCondition.typeSpecificDate[4], unlockCondition.typeSpecificDate[5]);
			DateTime dateTime = default(DateTime);
			dateTime = ClientGameManager.Get().PacificNow();
			if (!(dateTime > t))
			{
				break;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return true;
			}
		}
		case GameBalanceVars.UnlockData.UnlockType.CharacterLevel:
			if (unlockCondition.typeSpecificData == unlockConditionValue.typeSpecificData && unlockCondition.typeSpecificData2 != 0)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
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
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (unlockCondition.typeSpecificData2 <= 0)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (unlockCondition.typeSpecificData3 <= 0)
					{
						break;
					}
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			if (unlockCondition.typeSpecificData > unlockConditionValue.typeSpecificData)
			{
				break;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (unlockCondition.typeSpecificData2 > unlockConditionValue.typeSpecificData2)
			{
				break;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (unlockCondition.typeSpecificData3 > unlockConditionValue.typeSpecificData3)
			{
				break;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				return true;
			}
		case GameBalanceVars.UnlockData.UnlockType.ELO:
			if (unlockCondition.typeSpecificData != 0 && unlockCondition.typeSpecificData <= unlockConditionValue.typeSpecificData)
			{
				return true;
			}
			break;
		case GameBalanceVars.UnlockData.UnlockType.FactionTierReached:
			if (unlockCondition.typeSpecificData == 0)
			{
				break;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (unlockCondition.typeSpecificData != unlockConditionValue.typeSpecificData)
			{
				break;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (unlockCondition.typeSpecificData2 == unlockConditionValue.typeSpecificData2)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (unlockCondition.typeSpecificData3 != 0 && unlockCondition.typeSpecificData3 <= unlockConditionValue.typeSpecificData3)
				{
					return true;
				}
			}
			break;
		case GameBalanceVars.UnlockData.UnlockType.TitleLevelReached:
		{
			int typeSpecificData2 = unlockCondition.typeSpecificData;
			int typeSpecificData3 = unlockCondition.typeSpecificData2;
			if (typeSpecificData2 <= 0)
			{
				break;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (typeSpecificData2 != unlockConditionValue.typeSpecificData)
			{
				break;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (typeSpecificData3 > unlockConditionValue.typeSpecificData2)
			{
				break;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				return true;
			}
		}
		case GameBalanceVars.UnlockData.UnlockType.CurrentSeason:
		{
			int typeSpecificData = unlockCondition.typeSpecificData;
			if (typeSpecificData == unlockConditionValue.typeSpecificData)
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
		if (!unlockData.UnlockConditions.IsNullOrEmpty() && unlockData.UnlockConditions.Length <= 1)
		{
			if (unlockData.UnlockConditions[0].ConditionType == unlockConditionType)
			{
				GameBalanceVars.UnlockCondition unlockCondition = unlockData.UnlockConditions[0];
				GameBalanceVars.UnlockConditionValue unlockConditionValue = new GameBalanceVars.UnlockConditionValue();
				unlockConditionValue.typeSpecificData = typeSpecificData;
				unlockConditionValue.typeSpecificData2 = typeSpecificData2;
				unlockConditionValue.typeSpecificData3 = typeSpecificData3;
				unlockConditionValue.typeSpecificString = typeSpecificString;
				return IsUnlockConditionMet(unlockCondition, unlockConditionValue);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
		}
		return false;
	}

	public static bool ArePurchaseableConditionsMet(this GameBalanceVars.UnlockData unlockData, List<GameBalanceVars.UnlockConditionValue> unlockConditionValues, List<GameBalanceVars.UnlockData.UnlockType> ignoreUnlockTypes)
	{
		if (unlockData.PurchaseableConditions.IsNullOrEmpty())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return false;
				}
			}
		}
		if (!unlockConditionValues.IsNullOrEmpty())
		{
			if (unlockData.PurchaseableConditions.Length == unlockConditionValues.Count)
			{
				List<bool> list = new List<bool>(unlockData.PurchaseableConditions.Length);
				for (int i = 0; i < unlockData.PurchaseableConditions.Length; i++)
				{
					list.Add(false);
				}
				for (int j = 0; j < unlockData.PurchaseableConditions.Length; j++)
				{
					if (!ignoreUnlockTypes.IsNullOrEmpty())
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (ignoreUnlockTypes.Contains(unlockData.PurchaseableConditions[j].ConditionType))
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							list[j] = true;
							continue;
						}
					}
					list[j] = IsUnlockConditionMet(unlockData.PurchaseableConditions[j], unlockConditionValues[j]);
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
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								return result;
							}
						}
					}
				}
				LogicOpClass logicOpClass = LogicStatement.EvaluateLogicStatement(unlockData.PurchaseableLogicStatement);
				return logicOpClass.GetValue(list);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}

	public static bool MeetsPurchaseabilityConditions(GameBalanceVars.PlayerUnlockable unlockable)
	{
		List<GameBalanceVars.UnlockConditionValue> list = new List<GameBalanceVars.UnlockConditionValue>();
		if (unlockable != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (unlockable.m_unlockData != null)
			{
				if (unlockable.m_unlockData.PurchaseableConditions.IsNullOrEmpty())
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
				for (int i = 0; i < unlockable.m_unlockData.PurchaseableConditions.Length; i++)
				{
					list.Add(new GameBalanceVars.UnlockConditionValue
					{
						ConditionType = unlockable.m_unlockData.PurchaseableConditions[i].ConditionType
					});
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
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
							if (clientGameManager.ActiveFactionCompetition == unlockCondition.typeSpecificData && clientGameManager.FactionScores.TryGetValue(unlockCondition.typeSpecificData2, out long value))
							{
								unlockConditionValue.typeSpecificData3 = factionWideData.GetCompetitionFactionTierReached(unlockCondition.typeSpecificData, unlockCondition.typeSpecificData2, value);
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
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						List<GameBalanceVars.UnlockData.UnlockType> ignoreUnlockTypes = new List<GameBalanceVars.UnlockData.UnlockType>();
						return unlockable.m_unlockData.ArePurchaseableConditionsMet(list, ignoreUnlockTypes);
					}
				}
			}
			while (true)
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
	}

	public static bool MeetsVisibilityConditions(GameBalanceVars.PlayerUnlockable unlockable)
	{
		List<GameBalanceVars.UnlockConditionValue> list = new List<GameBalanceVars.UnlockConditionValue>();
		if (unlockable != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (unlockable.m_unlockData != null)
			{
				if (unlockable.m_unlockData.VisibilityConditions.IsNullOrEmpty())
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
				for (int i = 0; i < unlockable.m_unlockData.VisibilityConditions.Length; i++)
				{
					list.Add(new GameBalanceVars.UnlockConditionValue
					{
						ConditionType = unlockable.m_unlockData.VisibilityConditions[i].ConditionType
					});
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
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
								while (true)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
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
							if (clientGameManager.ActiveFactionCompetition == unlockCondition.typeSpecificData && clientGameManager.FactionScores.TryGetValue(unlockCondition.typeSpecificData2, out long value))
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								unlockConditionValue.typeSpecificData3 = factionWideData.GetCompetitionFactionTierReached(unlockCondition.typeSpecificData, unlockCondition.typeSpecificData2, value);
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
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						List<GameBalanceVars.UnlockData.UnlockType> ignoreUnlockTypes = new List<GameBalanceVars.UnlockData.UnlockType>();
						return unlockable.m_unlockData.AreVisibilityConditionsMet(list, ignoreUnlockTypes);
					}
				}
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}

	public static bool AreVisibilityConditionsMet(this GameBalanceVars.UnlockData unlockData, List<GameBalanceVars.UnlockConditionValue> unlockConditionValues, List<GameBalanceVars.UnlockData.UnlockType> ignoreUnlockTypes)
	{
		if (unlockData.VisibilityConditions.IsNullOrEmpty())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return false;
				}
			}
		}
		if (unlockConditionValues.IsNullOrEmpty() || unlockData.VisibilityConditions.Length != unlockConditionValues.Count)
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
			if (!ignoreUnlockTypes.IsNullOrEmpty() && ignoreUnlockTypes.Contains(unlockData.VisibilityConditions[j].ConditionType))
			{
				list[j] = true;
			}
			else
			{
				list[j] = IsUnlockConditionMet(unlockData.VisibilityConditions[j], unlockConditionValues[j]);
			}
		}
		if (unlockData.VisibilityLogicStatement.IsNullOrEmpty())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					bool result = true;
					using (List<bool>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (!enumerator.Current)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								result = false;
							}
						}
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								return result;
							}
						}
					}
				}
				}
			}
		}
		LogicOpClass logicOpClass = LogicStatement.EvaluateLogicStatement(unlockData.VisibilityLogicStatement);
		return logicOpClass.GetValue(list);
	}

	public static bool AreUnlockConditionsMet(this GameBalanceVars.UnlockData unlockData, List<GameBalanceVars.UnlockConditionValue> unlockConditionValues, List<GameBalanceVars.UnlockData.UnlockType> ignoreUnlockTypes)
	{
		if (unlockData.UnlockConditions.IsNullOrEmpty())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return false;
				}
			}
		}
		if (!unlockConditionValues.IsNullOrEmpty())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (unlockData.UnlockConditions.Length == unlockConditionValues.Count)
			{
				List<bool> list = new List<bool>(unlockData.UnlockConditions.Length);
				for (int i = 0; i < unlockData.UnlockConditions.Length; i++)
				{
					list.Add(false);
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					for (int j = 0; j < unlockData.UnlockConditions.Length; j++)
					{
						if (!ignoreUnlockTypes.IsNullOrEmpty() && ignoreUnlockTypes.Contains(unlockData.UnlockConditions[j].ConditionType))
						{
							list[j] = true;
						}
						else
						{
							list[j] = IsUnlockConditionMet(unlockData.UnlockConditions[j], unlockConditionValues[j]);
						}
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						if (unlockData.LogicStatement.IsNullOrEmpty())
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
								{
									bool result = true;
									using (List<bool>.Enumerator enumerator = list.GetEnumerator())
									{
										while (enumerator.MoveNext())
										{
											if (!enumerator.Current)
											{
												while (true)
												{
													switch (2)
													{
													case 0:
														continue;
													}
													break;
												}
												result = false;
											}
										}
										while (true)
										{
											switch (4)
											{
											case 0:
												break;
											default:
												return result;
											}
										}
									}
								}
								}
							}
						}
						LogicOpClass logicOpClass = LogicStatement.EvaluateLogicStatement(unlockData.LogicStatement);
						return logicOpClass.GetValue(list);
					}
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}

	public static bool AreUnlockConditionsMet(this GameBalanceVars.UnlockData unlockData, List<GameBalanceVars.UnlockConditionValue> unlockConditionValues, bool ignorePurchaseCondition)
	{
		if (unlockData.UnlockConditions.IsNullOrEmpty())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return false;
				}
			}
		}
		List<GameBalanceVars.UnlockData.UnlockType> list = new List<GameBalanceVars.UnlockData.UnlockType>();
		if (ignorePurchaseCondition)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			list.Add(GameBalanceVars.UnlockData.UnlockType.Purchase);
		}
		return unlockData.AreUnlockConditionsMet(unlockConditionValues, list);
	}

	public static CharacterType GetUnlockCharacterType(this GameBalanceVars.PlayerUnlockable playerUnlockable)
	{
		GameBalanceVars.UnlockData unlockData = playerUnlockable.m_unlockData;
		if (unlockData != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!unlockData.UnlockConditions.IsNullOrEmpty())
			{
				GameBalanceVars.UnlockCondition[] unlockConditions = unlockData.UnlockConditions;
				foreach (GameBalanceVars.UnlockCondition unlockCondition in unlockConditions)
				{
					if (unlockCondition.ConditionType != 0)
					{
						continue;
					}
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						return (CharacterType)unlockCondition.typeSpecificData;
					}
				}
				return CharacterType.None;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return CharacterType.None;
	}

	public static int GetUnlockPlayerLevel(this GameBalanceVars.PlayerUnlockable playerUnlockable)
	{
		GameBalanceVars.UnlockData unlockData = playerUnlockable.m_unlockData;
		if (unlockData != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!unlockData.UnlockConditions.IsNullOrEmpty())
			{
				GameBalanceVars.UnlockCondition[] unlockConditions = unlockData.UnlockConditions;
				foreach (GameBalanceVars.UnlockCondition unlockCondition in unlockConditions)
				{
					if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.PlayerLevel)
					{
						return unlockCondition.typeSpecificData;
					}
				}
				return 0;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return 0;
	}

	public static int GetUnlockCharacterLevel(this GameBalanceVars.PlayerUnlockable playerUnlockable, CharacterType characterType, bool checkForPurchaseableAfterLevel = false)
	{
		GameBalanceVars.UnlockData unlockData = playerUnlockable.m_unlockData;
		if (unlockData != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!unlockData.UnlockConditions.IsNullOrEmpty())
			{
				if (unlockData.UnlockConditions.Length != 1)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return 0;
						}
					}
				}
				GameBalanceVars.UnlockCondition[] unlockConditions = unlockData.UnlockConditions;
				foreach (GameBalanceVars.UnlockCondition unlockCondition in unlockConditions)
				{
					if (unlockCondition.ConditionType != 0)
					{
						continue;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (unlockCondition.typeSpecificData != (int)characterType)
					{
						continue;
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						return unlockCondition.typeSpecificData2;
					}
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					return 0;
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return 0;
	}

	public static int GetUnlockISOPrice(this GameBalanceVars.PlayerUnlockable playerUnlockable)
	{
		GameBalanceVars.UnlockData unlockData = playerUnlockable.m_unlockData;
		if (unlockData != null)
		{
			if (!unlockData.UnlockConditions.IsNullOrEmpty())
			{
				GameBalanceVars.UnlockCondition[] unlockConditions = unlockData.UnlockConditions;
				foreach (GameBalanceVars.UnlockCondition unlockCondition in unlockConditions)
				{
					if (unlockCondition.ConditionType != GameBalanceVars.UnlockData.UnlockType.Purchase)
					{
						continue;
					}
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						return unlockCondition.typeSpecificData2;
					}
				}
				return 0;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
		}
		return 0;
	}

	public static int GetUnlockRankedCurrencyPrice(this GameBalanceVars.PlayerUnlockable playerUnlockable)
	{
		GameBalanceVars.UnlockData unlockData = playerUnlockable.m_unlockData;
		if (unlockData == null || unlockData.UnlockConditions.IsNullOrEmpty())
		{
			return 0;
		}
		GameBalanceVars.UnlockCondition[] unlockConditions = unlockData.UnlockConditions;
		foreach (GameBalanceVars.UnlockCondition unlockCondition in unlockConditions)
		{
			if (unlockCondition.ConditionType != GameBalanceVars.UnlockData.UnlockType.Purchase)
			{
				continue;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return unlockCondition.typeSpecificData;
			}
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			return 0;
		}
	}

	public static int GetUnlockFreelancerCurrencyPrice(this GameBalanceVars.PlayerUnlockable playerUnlockable)
	{
		GameBalanceVars.UnlockData unlockData = playerUnlockable.m_unlockData;
		if (unlockData != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!unlockData.UnlockConditions.IsNullOrEmpty())
			{
				GameBalanceVars.UnlockCondition[] unlockConditions = unlockData.UnlockConditions;
				foreach (GameBalanceVars.UnlockCondition unlockCondition in unlockConditions)
				{
					if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.Purchase)
					{
						return unlockCondition.typeSpecificData3;
					}
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					return 0;
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return 0;
	}

	public static float GetRealCurrencyPrice(this GameBalanceVars.PlayerUnlockable playerUnlockable)
	{
		if (playerUnlockable.Prices != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return playerUnlockable.Prices.GetPrice(HydrogenConfig.Get().Ticket.AccountCurrency);
				}
			}
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
		if (unlockable is GameBalanceVars.StoreItemForPurchase)
		{
			return true;
		}
		return false;
	}

	public static bool IsOwned(this GameBalanceVars.PlayerUnlockable unlockable)
	{
		if (unlockable is GameBalanceVars.PlayerTitle)
		{
			return ClientGameManager.Get().IsTitleUnlocked(unlockable as GameBalanceVars.PlayerTitle);
		}
		if (unlockable is GameBalanceVars.PlayerBanner)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(unlockable.ID);
					List<GameBalanceVars.UnlockConditionValue> unlockConditionValues;
					return ClientGameManager.Get().IsBannerUnlocked(banner, out unlockConditionValues);
				}
				}
			}
		}
		if (unlockable is GameBalanceVars.SkinUnlockData)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return ClientGameManager.Get().GetPlayerCharacterData((CharacterType)unlockable.Index1)?.CharacterComponent.GetSkin(unlockable.ID).Unlocked ?? false;
				}
			}
		}
		if (unlockable is GameBalanceVars.PatternUnlockData)
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData((CharacterType)unlockable.Index1);
			if (playerCharacterData == null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
			return playerCharacterData.CharacterComponent.GetSkin(unlockable.Index2).GetPattern(unlockable.ID).Unlocked;
		}
		if (unlockable is GameBalanceVars.ColorUnlockData)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData((CharacterType)unlockable.Index1);
					if (playerCharacterData == null)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return false;
							}
						}
					}
					return playerCharacterData.CharacterComponent.GetSkin(unlockable.Index2).GetPattern(unlockable.Index3).GetColor(unlockable.ID)
						.Unlocked;
				}
				}
			}
		}
		if (unlockable is GameBalanceVars.TauntUnlockData)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData((CharacterType)unlockable.Index1);
					return playerCharacterData.CharacterComponent.GetTaunt(unlockable.ID).Unlocked;
				}
				}
			}
		}
		if (unlockable is GameBalanceVars.AbilityModUnlockData)
		{
			while (true)
			{
				bool result;
				switch (7)
				{
				case 0:
					break;
				default:
					{
						result = false;
						PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData((CharacterType)unlockable.Index1);
						if (playerCharacterData != null)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!playerCharacterData.CharacterComponent.IsModUnlocked(unlockable.Index2, unlockable.ID))
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!GameManager.Get().GameplayOverrides.EnableAllMods)
								{
									result = false;
									goto IL_0217;
								}
							}
							result = true;
						}
						goto IL_0217;
					}
					IL_0217:
					return result;
				}
			}
		}
		if (unlockable is GameBalanceVars.ChatEmoticon)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return ClientGameManager.Get().IsEmojiUnlocked(unlockable as GameBalanceVars.ChatEmoticon);
				}
			}
		}
		if (unlockable is GameBalanceVars.OverconUnlockData)
		{
			return ClientGameManager.Get().IsOverconUnlocked(unlockable.ID);
		}
		if (unlockable is GameBalanceVars.StoreItemForPurchase)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (unlockable is GameBalanceVars.AbilityVfxUnlockData)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData((CharacterType)unlockable.Index1);
					if (playerCharacterData == null)
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
					return playerCharacterData.CharacterComponent.IsAbilityVfxSwapUnlocked(unlockable.Index2, unlockable.ID);
				}
				}
			}
		}
		if (unlockable is GameBalanceVars.PlayerRibbon)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					List<GameBalanceVars.UnlockConditionValue> unlockConditionValues2;
					return ClientGameManager.Get().IsRibbonUnlocked(unlockable as GameBalanceVars.PlayerRibbon, out unlockConditionValues2);
				}
				}
			}
		}
		if (unlockable is GameBalanceVars.LoadingScreenBackground)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return ClientGameManager.Get().IsLoadingScreenBackgroundUnlocked(unlockable.ID);
				}
			}
		}
		throw new Exception("Not implemented");
	}

	public static string GetSpritePath(this GameBalanceVars.PlayerUnlockable unlockable)
	{
		if (unlockable == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return string.Empty;
				}
			}
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
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
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
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						throw new Exception("UIOverconData doesn't exist");
					}
				}
			}
			UIOverconData.NameToOverconEntry nameToOverconEntry = null;
			int num = 0;
			while (true)
			{
				if (num < UIOverconData.Get().m_nameToOverconEntry.Count)
				{
					if (UIOverconData.Get().m_nameToOverconEntry[num].m_overconId == unlockable.ID)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						nameToOverconEntry = UIOverconData.Get().m_nameToOverconEntry[num];
						break;
					}
					num++;
					continue;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				break;
			}
			if (nameToOverconEntry == null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						throw new Exception("Overcon doesn't exist for " + unlockable.ID);
					}
				}
			}
			return nameToOverconEntry.m_iconSpritePath;
		}
		if (unlockable is GameBalanceVars.StoreItemForPurchase)
		{
			InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate((unlockable as GameBalanceVars.StoreItemForPurchase).m_itemTemplateId);
			if (itemTemplate == null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						throw new Exception("inventoryItem " + (unlockable as GameBalanceVars.StoreItemForPurchase).m_itemTemplateId + " doesn't exist");
					}
				}
			}
			return itemTemplate.IconPath;
		}
		if (unlockable is GameBalanceVars.AbilityModUnlockData)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return "QuestRewards/modicon";
				}
			}
		}
		if (unlockable is GameBalanceVars.SkinUnlockData)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)unlockable.Index1);
					if (characterResourceLink == null)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								throw new Exception(string.Concat("Character ", (CharacterType)unlockable.Index1, " doesn't exist"));
							}
						}
					}
					if (characterResourceLink.m_skins.Count <= unlockable.ID)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								throw new Exception(string.Concat("Skin index ", unlockable.ID, " for ", (CharacterType)unlockable.Index1, " missing"));
							}
						}
					}
					return characterResourceLink.m_skins[unlockable.ID].m_skinSelectionIconPath;
				}
				}
			}
		}
		if (unlockable is GameBalanceVars.ColorUnlockData)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink((CharacterType)unlockable.Index1);
					if (characterResourceLink2 == null)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								throw new Exception(string.Concat("Character ", (CharacterType)unlockable.Index1, " doesn't exist"));
							}
						}
					}
					if (characterResourceLink2.m_skins.Count <= unlockable.Index2)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								throw new Exception(string.Concat("Skin index ", unlockable.Index2, " for ", (CharacterType)unlockable.Index1, " missing"));
							}
						}
					}
					CharacterSkin characterSkin = characterResourceLink2.m_skins[unlockable.Index2];
					if (characterSkin.m_patterns.Count <= unlockable.Index3)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								throw new Exception(string.Concat("Pattern index of ", unlockable.Index3, " of skin index ", unlockable.Index2, " for ", (CharacterType)unlockable.Index1, " missing"));
							}
						}
					}
					CharacterPattern characterPattern = characterSkin.m_patterns[unlockable.Index3];
					if (characterPattern.m_colors.Count <= unlockable.ID)
					{
						throw new Exception(string.Concat("Color index ", unlockable.ID, " of Pattern index of ", unlockable.Index3, " of skin index ", unlockable.Index2, " for ", (CharacterType)unlockable.Index1, " missing"));
					}
					return characterPattern.m_colors[unlockable.ID].m_iconResourceString;
				}
				}
			}
		}
		if (unlockable is GameBalanceVars.TauntUnlockData)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return "QuestRewards/taunt";
				}
			}
		}
		if (unlockable is GameBalanceVars.PlayerTitle)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return "QuestRewards/general";
				}
			}
		}
		if (unlockable is GameBalanceVars.AbilityVfxUnlockData)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return "QuestRewards/vfxicon";
				}
			}
		}
		if (unlockable is GameBalanceVars.PlayerRibbon)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					GameBalanceVars.PlayerRibbon playerRibbon = unlockable as GameBalanceVars.PlayerRibbon;
					return playerRibbon.m_resourceIconString;
				}
				}
			}
		}
		if (unlockable is GameBalanceVars.LoadingScreenBackground)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					GameBalanceVars.LoadingScreenBackground loadingScreenBackground = unlockable as GameBalanceVars.LoadingScreenBackground;
					return loadingScreenBackground.m_iconPath;
				}
				}
			}
		}
		throw new Exception("Sprite Not Implemented for " + unlockable.GetType());
	}

	public static Sprite GetItemFg(this GameBalanceVars.PlayerUnlockable unlockable)
	{
		if (unlockable == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return null;
				}
			}
		}
		AbilityData component;
		AbilityData.ActionType actionType;
		if (unlockable is GameBalanceVars.TauntUnlockData)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)unlockable.Index1);
			component = characterResourceLink.ActorDataPrefab.GetComponent<AbilityData>();
			actionType = characterResourceLink.m_taunts[unlockable.ID].m_actionForTaunt;
		}
		else
		{
			if (!(unlockable is GameBalanceVars.AbilityVfxUnlockData))
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!(unlockable is GameBalanceVars.AbilityModUnlockData))
				{
					return null;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)unlockable.Index1);
			component = characterResourceLink.ActorDataPrefab.GetComponent<AbilityData>();
			actionType = (AbilityData.ActionType)unlockable.Index2;
		}
		if (actionType == AbilityData.ActionType.ABILITY_0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return component.m_sprite0;
				}
			}
		}
		if (actionType == AbilityData.ActionType.ABILITY_1)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return component.m_sprite1;
				}
			}
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
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return component.m_sprite4;
				}
			}
		}
		if (actionType == AbilityData.ActionType.ABILITY_5)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return component.m_sprite5;
				}
			}
		}
		if (actionType == AbilityData.ActionType.ABILITY_6)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return component.m_sprite6;
				}
			}
		}
		return null;
	}

	public static GameBalanceVars.AbilityModUnlockData GetAbilityModUnlockData(this AbilityMod mod, CharacterType type, int abilityId)
	{
		GameBalanceVars.AbilityModUnlockData abilityModUnlockData = new GameBalanceVars.AbilityModUnlockData();
		abilityModUnlockData.Index1 = (int)type;
		abilityModUnlockData.Index2 = abilityId;
		abilityModUnlockData.ID = mod.m_abilityScopeId;
		abilityModUnlockData.m_unlockData = new GameBalanceVars.UnlockData();
		return abilityModUnlockData;
	}
}
