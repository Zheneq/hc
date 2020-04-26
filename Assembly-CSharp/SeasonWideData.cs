using System;
using System.Collections.Generic;
using UnityEngine;

public class SeasonWideData : MonoBehaviour
{
	private static SeasonWideData s_instance;

	[Header("Seasons")]
	public List<SeasonTemplate> m_seasons;

	[Header("Alerts")]
	public List<AlertMission> m_alerts;

	[Header("Season Quest Pools")]
	public List<QuestPool> m_seasonQuestPools;

	public static SeasonWideData Get()
	{
		return s_instance;
	}

	public int GetSeasonExperience(int seasonNumber, int seasonLevel)
	{
		using (List<SeasonTemplate>.Enumerator enumerator = m_seasons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SeasonTemplate current = enumerator.Current;
				if (current.Index == seasonNumber)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return current.GetSeasonExperience(seasonLevel);
						}
					}
				}
			}
		}
		return 1;
	}

	public SeasonTemplate GetSeasonTemplate(int seasonNumber)
	{
		using (List<SeasonTemplate>.Enumerator enumerator = m_seasons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SeasonTemplate current = enumerator.Current;
				if (current.Index == seasonNumber)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return current;
						}
					}
				}
			}
		}
		return null;
	}

	public int GetPlayerFacingSeasonNumber(int seasonIndex)
	{
		SeasonTemplate seasonTemplate = GetSeasonTemplate(seasonIndex);
		if (seasonTemplate != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return seasonTemplate.GetPlayerFacingSeasonNumber();
				}
			}
		}
		return seasonIndex;
	}

	public List<SeasonTemplate.SeasonEndRewards> GetAvailableEndRewards(SeasonTemplate season)
	{
		List<SeasonTemplate.SeasonEndRewards> list = new List<SeasonTemplate.SeasonEndRewards>();
		list.Add(season.EndRewards);
		foreach (SeasonTemplate.ConditionalSeasonEndRewards conditionalEndReward in season.ConditionalEndRewards)
		{
			if (QuestWideData.AreConditionsMet(conditionalEndReward.Prerequisites.Conditions, conditionalEndReward.Prerequisites.LogicStatement))
			{
				list.Add(conditionalEndReward);
			}
		}
		return list;
	}

	public static bool IsDateInTimedSeason(SeasonTemplate season, DateTime dateTime)
	{
		if (GetTimeBounds(season, out DateTime startTime, out DateTime endTime))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					int result;
					if (startTime < dateTime)
					{
						result = ((dateTime <= endTime) ? 1 : 0);
					}
					else
					{
						result = 0;
					}
					return (byte)result != 0;
				}
				}
			}
		}
		return false;
	}

	public static bool GetTimeBounds(SeasonTemplate season, out DateTime startTime, out DateTime endTime)
	{
		startTime = DateTime.MinValue;
		endTime = DateTime.MaxValue;
		QuestPrerequisites prerequisites = season.Prerequisites;
		if (prerequisites.Conditions.Count >= 1)
		{
			if (prerequisites.Conditions.Count <= 2)
			{
				if (!prerequisites.LogicStatement.IsNullOrEmpty())
				{
					LogicOpClass logicOpClass = LogicStatement.EvaluateLogicStatement(prerequisites.LogicStatement);
					if (logicOpClass is NegateLogicOpClass)
					{
						NegateLogicOpClass negateLogicOpClass = logicOpClass as NegateLogicOpClass;
						if (negateLogicOpClass.m_target is ConstantLogicOpClass)
						{
							ConstantLogicOpClass constantLogicOpClass = negateLogicOpClass.m_target as ConstantLogicOpClass;
							if (constantLogicOpClass.myIndex == 0)
							{
								QuestCondition questCondition = prerequisites.Conditions[0];
								endTime = new DateTime(questCondition.typeSpecificDate[0], questCondition.typeSpecificDate[1], questCondition.typeSpecificDate[2], questCondition.typeSpecificDate[3], questCondition.typeSpecificDate[4], questCondition.typeSpecificDate[5]);
								return true;
							}
						}
					}
					else if (logicOpClass is AndLogicOpClass)
					{
						AndLogicOpClass andLogicOpClass = logicOpClass as AndLogicOpClass;
						LogicOpClass[] array = new LogicOpClass[2]
						{
							andLogicOpClass.m_left,
							andLogicOpClass.m_right
						};
						bool flag = false;
						bool flag2 = false;
						int index = 0;
						int index2 = 0;
						bool[] array2 = new bool[array.Length];
						for (int i = 0; i < array.Length; i++)
						{
							ConstantLogicOpClass constantLogicOpClass2 = null;
							if (array[i] is ConstantLogicOpClass)
							{
								flag = true;
								constantLogicOpClass2 = (array[i] as ConstantLogicOpClass);
								index = constantLogicOpClass2.myIndex;
							}
							else if (array[i] is NegateLogicOpClass)
							{
								NegateLogicOpClass negateLogicOpClass2 = array[i] as NegateLogicOpClass;
								if (negateLogicOpClass2.m_target is ConstantLogicOpClass)
								{
									flag2 = true;
									constantLogicOpClass2 = (negateLogicOpClass2.m_target as ConstantLogicOpClass);
									index2 = constantLogicOpClass2.myIndex;
								}
							}
							if (constantLogicOpClass2 == null)
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
							array2[i] = true;
						}
						if (flag)
						{
							if (flag2)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										break;
									default:
									{
										for (int j = 0; j < array2.Length; j++)
										{
											if (!array2[j])
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
										}
										while (true)
										{
											switch (6)
											{
											case 0:
												break;
											default:
											{
												QuestCondition questCondition2 = prerequisites.Conditions[index];
												startTime = new DateTime(questCondition2.typeSpecificDate[0], questCondition2.typeSpecificDate[1], questCondition2.typeSpecificDate[2], questCondition2.typeSpecificDate[3], questCondition2.typeSpecificDate[4], questCondition2.typeSpecificDate[5]);
												questCondition2 = prerequisites.Conditions[index2];
												endTime = new DateTime(questCondition2.typeSpecificDate[0], questCondition2.typeSpecificDate[1], questCondition2.typeSpecificDate[2], questCondition2.typeSpecificDate[3], questCondition2.typeSpecificDate[4], questCondition2.typeSpecificDate[5]);
												return true;
											}
											}
										}
									}
									}
								}
							}
						}
					}
					return false;
				}
			}
		}
		return false;
	}

	private void Awake()
	{
		s_instance = this;
		if (m_seasons.Count == 0)
		{
			throw new Exception("SeasonWideData failed to load");
		}
	}

	private void OnDestroy()
	{
		s_instance = null;
	}
}
