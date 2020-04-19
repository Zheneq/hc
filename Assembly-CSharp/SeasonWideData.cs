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
		return SeasonWideData.s_instance;
	}

	public int GetSeasonExperience(int seasonNumber, int seasonLevel)
	{
		using (List<SeasonTemplate>.Enumerator enumerator = this.m_seasons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SeasonTemplate seasonTemplate = enumerator.Current;
				if (seasonTemplate.Index == seasonNumber)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(SeasonWideData.GetSeasonExperience(int, int)).MethodHandle;
					}
					return seasonTemplate.GetSeasonExperience(seasonLevel);
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
		return 1;
	}

	public SeasonTemplate GetSeasonTemplate(int seasonNumber)
	{
		using (List<SeasonTemplate>.Enumerator enumerator = this.m_seasons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SeasonTemplate seasonTemplate = enumerator.Current;
				if (seasonTemplate.Index == seasonNumber)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(SeasonWideData.GetSeasonTemplate(int)).MethodHandle;
					}
					return seasonTemplate;
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
		}
		return null;
	}

	public int GetPlayerFacingSeasonNumber(int seasonIndex)
	{
		SeasonTemplate seasonTemplate = this.GetSeasonTemplate(seasonIndex);
		if (seasonTemplate != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SeasonWideData.GetPlayerFacingSeasonNumber(int)).MethodHandle;
			}
			return seasonTemplate.GetPlayerFacingSeasonNumber();
		}
		return seasonIndex;
	}

	public List<SeasonTemplate.SeasonEndRewards> GetAvailableEndRewards(SeasonTemplate season)
	{
		List<SeasonTemplate.SeasonEndRewards> list = new List<SeasonTemplate.SeasonEndRewards>();
		list.Add(season.EndRewards);
		foreach (SeasonTemplate.ConditionalSeasonEndRewards conditionalSeasonEndRewards in season.ConditionalEndRewards)
		{
			if (QuestWideData.AreConditionsMet(conditionalSeasonEndRewards.Prerequisites.Conditions, conditionalSeasonEndRewards.Prerequisites.LogicStatement, false))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SeasonWideData.GetAvailableEndRewards(SeasonTemplate)).MethodHandle;
				}
				list.Add(conditionalSeasonEndRewards);
			}
		}
		return list;
	}

	public static bool IsDateInTimedSeason(SeasonTemplate season, DateTime dateTime)
	{
		DateTime t;
		DateTime t2;
		if (SeasonWideData.GetTimeBounds(season, out t, out t2))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SeasonWideData.IsDateInTimedSeason(SeasonTemplate, DateTime)).MethodHandle;
			}
			bool result;
			if (t < dateTime)
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
				result = (dateTime <= t2);
			}
			else
			{
				result = false;
			}
			return result;
		}
		return false;
	}

	public unsafe static bool GetTimeBounds(SeasonTemplate season, out DateTime startTime, out DateTime endTime)
	{
		startTime = DateTime.MinValue;
		endTime = DateTime.MaxValue;
		QuestPrerequisites prerequisites = season.Prerequisites;
		if (prerequisites.Conditions.Count >= 1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SeasonWideData.GetTimeBounds(SeasonTemplate, DateTime*, DateTime*)).MethodHandle;
			}
			if (prerequisites.Conditions.Count <= 2)
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
				if (!prerequisites.LogicStatement.IsNullOrEmpty())
				{
					LogicOpClass logicOpClass = LogicStatement.EvaluateLogicStatement(prerequisites.LogicStatement);
					if (logicOpClass is NegateLogicOpClass)
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
						LogicOpClass[] array = new LogicOpClass[]
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
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								flag = true;
								constantLogicOpClass2 = (array[i] as ConstantLogicOpClass);
								index = constantLogicOpClass2.myIndex;
							}
							else if (array[i] is NegateLogicOpClass)
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
								NegateLogicOpClass negateLogicOpClass2 = array[i] as NegateLogicOpClass;
								if (negateLogicOpClass2.m_target is ConstantLogicOpClass)
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
									flag2 = true;
									constantLogicOpClass2 = (negateLogicOpClass2.m_target as ConstantLogicOpClass);
									index2 = constantLogicOpClass2.myIndex;
								}
							}
							if (constantLogicOpClass2 == null)
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
								return false;
							}
							array2[i] = true;
						}
						if (flag)
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
							if (flag2)
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
								for (int j = 0; j < array2.Length; j++)
								{
									if (!array2[j])
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
										return false;
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
								QuestCondition questCondition2 = prerequisites.Conditions[index];
								startTime = new DateTime(questCondition2.typeSpecificDate[0], questCondition2.typeSpecificDate[1], questCondition2.typeSpecificDate[2], questCondition2.typeSpecificDate[3], questCondition2.typeSpecificDate[4], questCondition2.typeSpecificDate[5]);
								questCondition2 = prerequisites.Conditions[index2];
								endTime = new DateTime(questCondition2.typeSpecificDate[0], questCondition2.typeSpecificDate[1], questCondition2.typeSpecificDate[2], questCondition2.typeSpecificDate[3], questCondition2.typeSpecificDate[4], questCondition2.typeSpecificDate[5]);
								return true;
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
		SeasonWideData.s_instance = this;
		if (this.m_seasons.Count == 0)
		{
			throw new Exception("SeasonWideData failed to load");
		}
	}

	private void OnDestroy()
	{
		SeasonWideData.s_instance = null;
	}
}
