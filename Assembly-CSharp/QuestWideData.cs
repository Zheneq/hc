using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestWideData : MonoBehaviour
{
	private static QuestWideData s_instance;

	[Header("Quest Global Values")]
	public int m_generalSlotCount;

	public int m_questResetHour;

	public DayOfWeek m_questResetDayOfWeek;

	public int m_questBonusPerRejection = 10;

	public int m_questMaxRejectPercentage = 50;

	public int m_notIncludedWeightIncrease;

	public int m_rejectedWeightDecrease;

	public int m_completedWeightDecrease;

	public int m_abandonedWeightDecrease;

	[Header("Quests")]
	public List<QuestTemplate> m_quests;

	[Header("Style Groups")]
	public List<QuestStyleGroup> m_styleGroups;

	[Header("Daily Quest Pools")]
	public List<QuestPool> m_dailyQuestPools;

	private static Dictionary<int, bool> m_dailyQuestsCache = new Dictionary<int, bool>();

	public static QuestWideData Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		if (m_quests.Count == 0)
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
					throw new Exception("QuestWideData failed to load");
				}
			}
		}
		m_dailyQuestsCache = new Dictionary<int, bool>();
		using (List<QuestPool>.Enumerator enumerator = m_dailyQuestPools.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				QuestPool current = enumerator.Current;
				if (current.Valid)
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
					using (List<QuestPool.Quest>.Enumerator enumerator2 = current.Quests.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							QuestPool.Quest current2 = enumerator2.Current;
							if (!m_dailyQuestsCache.ContainsKey(current2.QuestId))
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
								m_dailyQuestsCache.Add(current2.QuestId, true);
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
				}
			}
			while (true)
			{
				switch (1)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public QuestTemplate GetQuestTemplate(int templateId)
	{
		if (templateId > 0 && m_quests.Count >= templateId)
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
					return m_quests[templateId - 1];
				}
			}
		}
		return null;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public bool IsDailyQuest(int questId)
	{
		int result;
		if (m_dailyQuestsCache.TryGetValue(questId, out bool value))
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
			result = (value ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public bool CheckAllIndices()
	{
		bool result = false;
		if (Application.isEditor)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					int num = 1;
					foreach (QuestTemplate quest in m_quests)
					{
						if (quest.Index >= num)
						{
							num = quest.Index + 1;
						}
					}
					using (List<QuestTemplate>.Enumerator enumerator2 = m_quests.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							QuestTemplate current2 = enumerator2.Current;
							bool flag = false;
							if (current2.Index == 0)
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
								flag = true;
							}
							else
							{
								int num2 = 0;
								foreach (QuestTemplate quest2 in m_quests)
								{
									if (quest2.Index == current2.Index)
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
										num2++;
									}
								}
								if (num2 > 1)
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
									flag = true;
								}
							}
							if (flag)
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
								current2.Index = num;
								num++;
								result = true;
							}
						}
						while (true)
						{
							switch (3)
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
		return result;
	}

	public static bool IsCharacterQuest(List<QuestObjective> Objectives, CharacterResourceLink charLink)
	{
		using (List<QuestObjective>.Enumerator enumerator = Objectives.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				QuestObjective current = enumerator.Current;
				using (List<QuestTrigger>.Enumerator enumerator2 = current.Triggers.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						QuestTrigger current2 = enumerator2.Current;
						using (List<QuestCondition>.Enumerator enumerator3 = current2.Conditions.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								QuestCondition current3 = enumerator3.Current;
								if (current3.ConditionType == QuestConditionType.UsingCharacter)
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
									if (current3.typeSpecificData == (int)charLink.m_characterType)
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
								}
								if (current3.ConditionType == QuestConditionType.HasCharacterLevel)
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
									if (current3.typeSpecificData == (int)charLink.m_characterType)
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
								}
								if (current3.ConditionType == QuestConditionType.UsingCharacterRole && current3.typeSpecificData == (int)charLink.m_characterRole)
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
								if (current3.ConditionType == QuestConditionType.UsingCharacterFaction)
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
									FactionCompetition factionCompetition = FactionWideData.Get().GetFactionCompetition(current3.typeSpecificData);
									FactionGroup factionGroup = FactionWideData.Get().GetFactionGroup(factionCompetition.Factions[current3.typeSpecificData2].FactionGroupIDToUse);
									if (factionGroup.Characters.Exists((CharacterType x) => x == charLink.m_characterType))
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

	public static int GetEndLevel(QuestPrerequisites prereqs, int seasonIndex)
	{
		Queue<LogicOpClass> queue = new Queue<LogicOpClass>();
		string text = prereqs.LogicStatement;
		if (text.IsNullOrEmpty())
		{
			int num = 65;
			for (int i = 0; i < prereqs.Conditions.Count; i++)
			{
				text = ((!text.IsNullOrEmpty()) ? (text + " & " + Convert.ToChar(num)) : Convert.ToChar(num).ToString());
				num++;
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
		queue.Enqueue(LogicStatement.EvaluateLogicStatement(text));
		int result = 0;
		while (queue.Count > 0)
		{
			LogicOpClass logicOpClass = queue.Dequeue();
			if (logicOpClass is ConstantLogicOpClass)
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
				continue;
			}
			if (logicOpClass is AndLogicOpClass)
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
				AndLogicOpClass andLogicOpClass = (AndLogicOpClass)logicOpClass;
				queue.Enqueue(andLogicOpClass.m_left);
				queue.Enqueue(andLogicOpClass.m_right);
				continue;
			}
			if (logicOpClass is OrLogicOpClass)
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
				OrLogicOpClass orLogicOpClass = (OrLogicOpClass)logicOpClass;
				queue.Enqueue(orLogicOpClass.m_left);
				queue.Enqueue(orLogicOpClass.m_right);
				continue;
			}
			if (!(logicOpClass is NegateLogicOpClass))
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
				continue;
			}
			NegateLogicOpClass negateLogicOpClass = (NegateLogicOpClass)logicOpClass;
			if (negateLogicOpClass.m_target is ConstantLogicOpClass)
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
				int myIndex = ((ConstantLogicOpClass)negateLogicOpClass.m_target).myIndex;
				if (prereqs.Conditions[myIndex].ConditionType != QuestConditionType.HasSeasonLevel)
				{
					continue;
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
				if (prereqs.Conditions[myIndex].typeSpecificData == seasonIndex)
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
					result = prereqs.Conditions[myIndex].typeSpecificData2;
				}
			}
			else if (negateLogicOpClass.m_target is AndLogicOpClass)
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
				AndLogicOpClass andLogicOpClass2 = (AndLogicOpClass)negateLogicOpClass.m_target;
				negateLogicOpClass = new NegateLogicOpClass();
				negateLogicOpClass.m_target = andLogicOpClass2.m_left;
				queue.Enqueue(negateLogicOpClass);
				negateLogicOpClass = new NegateLogicOpClass();
				negateLogicOpClass.m_target = andLogicOpClass2.m_right;
				queue.Enqueue(negateLogicOpClass);
			}
			else if (negateLogicOpClass.m_target is OrLogicOpClass)
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
				OrLogicOpClass orLogicOpClass2 = (OrLogicOpClass)negateLogicOpClass.m_target;
				negateLogicOpClass = new NegateLogicOpClass();
				negateLogicOpClass.m_target = orLogicOpClass2.m_left;
				queue.Enqueue(negateLogicOpClass);
				negateLogicOpClass = new NegateLogicOpClass();
				negateLogicOpClass.m_target = orLogicOpClass2.m_right;
				queue.Enqueue(negateLogicOpClass);
			}
			else if (negateLogicOpClass.m_target is NegateLogicOpClass)
			{
				negateLogicOpClass = (NegateLogicOpClass)negateLogicOpClass.m_target;
				queue.Enqueue(negateLogicOpClass.m_target);
			}
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			return result;
		}
	}

	public static bool AreConditionsMet(List<QuestCondition> conditions, string logicStatement, bool tryUseCharDataOnInitialLoad = false)
	{
		List<bool> list = new List<bool>(conditions.Count);
		for (int i = 0; i < conditions.Count; i++)
		{
			QuestCondition questCondition = conditions[i];
			if (questCondition.ConditionType == QuestConditionType.HasDateTimePassed)
			{
				DateTime t = new DateTime(questCondition.typeSpecificDate[0], questCondition.typeSpecificDate[1], questCondition.typeSpecificDate[2], questCondition.typeSpecificDate[3], questCondition.typeSpecificDate[4], questCondition.typeSpecificDate[5]);
				if (ClientGameManager.Get().PacificNow() > t)
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
					list.Add(true);
				}
				else
				{
					list.Add(false);
				}
			}
			else if (questCondition.ConditionType == QuestConditionType.HasCompletedQuest)
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
				list.Add(ClientGameManager.Get().GetPlayerAccountData().QuestComponent.GetCompletedCount(questCondition.typeSpecificData) > 0);
			}
			else if (questCondition.ConditionType == QuestConditionType.HasPurchasedGame)
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
				list.Add(ClientGameManager.Get().HasPurchasedGame);
			}
			else if (questCondition.ConditionType == QuestConditionType.HasUnlockedCharacter)
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
				list.Add(ClientGameManager.Get().GetPlayerCharacterData((CharacterType)questCondition.typeSpecificData).CharacterComponent.Unlocked);
			}
			else if (questCondition.ConditionType == QuestConditionType.HasUnlockedStyle)
			{
				bool item = false;
				CharacterType typeSpecificData = (CharacterType)questCondition.typeSpecificData;
				PersistedCharacterData persistedCharacterData;
				if (tryUseCharDataOnInitialLoad)
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
					persistedCharacterData = ClientGameManager.Get().GetCharacterDataOnInitialLoad(typeSpecificData);
				}
				else
				{
					persistedCharacterData = ClientGameManager.Get().GetPlayerCharacterData(typeSpecificData);
				}
				PersistedCharacterData persistedCharacterData2 = persistedCharacterData;
				if (persistedCharacterData2 != null)
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
					item = persistedCharacterData2.CharacterComponent.IsStyleUnlocked(questCondition.typeSpecificData2, questCondition.typeSpecificData3, questCondition.typeSpecificData4);
				}
				list.Add(item);
			}
			else if (questCondition.ConditionType == QuestConditionType.HasUnlockedTitle)
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
				list.Add(ClientGameManager.Get().GetPlayerAccountData().AccountComponent.IsTitleUnlocked(questCondition.typeSpecificData));
			}
			else if (questCondition.ConditionType == QuestConditionType.HasUnlockedChatEmoji)
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
				list.Add(ClientGameManager.Get().GetPlayerAccountData().AccountComponent.IsChatEmojiUnlocked(questCondition.typeSpecificData));
			}
			else if (questCondition.ConditionType == QuestConditionType.HasUnlockedOvercon)
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
				list.Add(ClientGameManager.Get().GetPlayerAccountData().AccountComponent.IsOverconUnlocked(questCondition.typeSpecificData));
			}
			else if (questCondition.ConditionType == QuestConditionType.HasUnlockedTaunt)
			{
				bool item2 = false;
				CharacterType typeSpecificData2 = (CharacterType)questCondition.typeSpecificData;
				PersistedCharacterData persistedCharacterData3;
				if (tryUseCharDataOnInitialLoad)
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
					persistedCharacterData3 = ClientGameManager.Get().GetCharacterDataOnInitialLoad(typeSpecificData2);
				}
				else
				{
					persistedCharacterData3 = ClientGameManager.Get().GetPlayerCharacterData(typeSpecificData2);
				}
				PersistedCharacterData persistedCharacterData4 = persistedCharacterData3;
				if (persistedCharacterData4 != null)
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
					if (questCondition.typeSpecificData2 < persistedCharacterData4.CharacterComponent.Taunts.Count)
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
						item2 = persistedCharacterData4.CharacterComponent.GetTaunt(questCondition.typeSpecificData2).Unlocked;
					}
				}
				list.Add(item2);
			}
			else
			{
				if (questCondition.ConditionType != QuestConditionType.HasSeasonAccess)
				{
					list.Add(false);
					throw new Exception("Unimplemented quest condition: " + questCondition.ConditionType);
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
				list.Add(ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason == questCondition.typeSpecificData);
			}
		}
		if (logicStatement == string.Empty)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					bool result = true;
					{
						foreach (bool item3 in list)
						{
							if (!item3)
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
								result = false;
							}
						}
						return result;
					}
				}
				}
			}
		}
		return LogicStatement.EvaluateLogicStatement(logicStatement).GetValue(list);
	}
}
