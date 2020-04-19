using System;
using System.Collections.Generic;

public static class LogicStatement
{
	private static readonly Dictionary<string, LogicOpClass> s_logicStatementCache = new Dictionary<string, LogicOpClass>();

	public static LogicOpClass EvaluateLogicStatement(string logicStatement)
	{
		if (LogicStatement.s_logicStatementCache.ContainsKey(logicStatement))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LogicStatement.EvaluateLogicStatement(string)).MethodHandle;
			}
			return LogicStatement.s_logicStatementCache[logicStatement];
		}
		int i = 0;
		string text = logicStatement.Trim();
		char[] array = text.ToCharArray();
		LogicOpClass logicOpClass = null;
		bool flag = false;
		while (i < array.Length)
		{
			char c = array[i];
			if (c == '!')
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
				flag = true;
			}
			else if (c == '(')
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
				int num = -1;
				int num2 = i + 1;
				int num3 = 0;
				while (num == -1)
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
					if (num2 >= text.Length)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							goto IL_E4;
						}
					}
					else
					{
						if (array[num2] == ')')
						{
							if (num3 == 0)
							{
								num = num2;
							}
							else
							{
								num3--;
							}
						}
						else if (array[num2] == '(')
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
							num3++;
						}
						num2++;
					}
				}
				IL_E4:
				if (num == -1)
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
					Log.Warning("Error, part of the logic statement (invalid parens) passed to quest evaluation was bad.  Returning false!", new object[0]);
					return new ConstantLogicOpClass();
				}
				string logicStatement2 = text.Substring(i + 1, num - (i + 1));
				LogicOpClass logicOpClass2 = LogicStatement.EvaluateLogicStatement(logicStatement2);
				if (flag)
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
					logicOpClass = new NegateLogicOpClass
					{
						m_target = logicOpClass2
					};
				}
				else
				{
					logicOpClass = logicOpClass2;
				}
				i = num + 1;
			}
			else if (c == '|')
			{
				OrLogicOpClass orLogicOpClass = new OrLogicOpClass();
				orLogicOpClass.m_left = logicOpClass;
				string logicStatement3 = text.Substring(i + 1);
				orLogicOpClass.m_right = LogicStatement.EvaluateLogicStatement(logicStatement3);
				i = text.Length;
				logicOpClass = orLogicOpClass;
			}
			else if (c == '&')
			{
				AndLogicOpClass andLogicOpClass = new AndLogicOpClass();
				andLogicOpClass.m_left = logicOpClass;
				string logicStatement4 = text.Substring(i + 1);
				andLogicOpClass.m_right = LogicStatement.EvaluateLogicStatement(logicStatement4);
				i = text.Length;
				logicOpClass = andLogicOpClass;
			}
			else if (c >= 'A')
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
				if (c <= 'Z')
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
					ConstantLogicOpClass constantLogicOpClass = new ConstantLogicOpClass();
					int myIndex = (int)(c - 'A');
					constantLogicOpClass.myIndex = myIndex;
					if (flag)
					{
						logicOpClass = new NegateLogicOpClass
						{
							m_target = constantLogicOpClass
						};
					}
					else
					{
						logicOpClass = constantLogicOpClass;
					}
				}
			}
			i++;
		}
		if (logicOpClass == null)
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
			Log.Warning("Error, part of the logic statement {0} passed to quest evaluation was bad.  Returning false!", new object[]
			{
				logicStatement
			});
			logicOpClass = new ConstantLogicOpClass();
		}
		LogicStatement.s_logicStatementCache[logicStatement] = logicOpClass;
		return logicOpClass;
	}

	public static DateTime ToDateTime(this QuestCondition dateTimeCondition)
	{
		List<int> typeSpecificDate = dateTimeCondition.typeSpecificDate;
		if (!typeSpecificDate.IsNullOrEmpty<int>())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestCondition.ToDateTime()).MethodHandle;
			}
			if (typeSpecificDate.Count >= 6)
			{
				return new DateTime(typeSpecificDate[0], typeSpecificDate[1], typeSpecificDate[2], typeSpecificDate[3], typeSpecificDate[4], typeSpecificDate[5]);
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
		return DateTime.MinValue;
	}

	public static TimeRange GetTimeRange(this QuestPrerequisites prereqs)
	{
		Queue<LogicOpClass> queue = new Queue<LogicOpClass>();
		string text = prereqs.LogicStatement;
		if (text.IsNullOrEmpty())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestPrerequisites.GetTimeRange()).MethodHandle;
			}
			int num = 0x41;
			for (int i = 0; i < prereqs.Conditions.Count; i++)
			{
				if (text.IsNullOrEmpty())
				{
					text = Convert.ToChar(num).ToString();
				}
				else
				{
					text = text + " & " + Convert.ToChar(num);
				}
				num++;
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
		queue.Enqueue(LogicStatement.EvaluateLogicStatement(text));
		TimeRange timeRange = new TimeRange();
		while (queue.Count > 0)
		{
			LogicOpClass logicOpClass = queue.Dequeue();
			if (logicOpClass is ConstantLogicOpClass)
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
				int myIndex = ((ConstantLogicOpClass)logicOpClass).myIndex;
				if (myIndex < prereqs.Conditions.Count)
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
					if (prereqs.Conditions[myIndex].ConditionType == QuestConditionType.HasDateTimePassed)
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
						timeRange.StartTime = new DateTime?(prereqs.Conditions[myIndex].ToDateTime());
					}
				}
			}
			else if (logicOpClass is AndLogicOpClass)
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
				AndLogicOpClass andLogicOpClass = (AndLogicOpClass)logicOpClass;
				queue.Enqueue(andLogicOpClass.m_left);
				queue.Enqueue(andLogicOpClass.m_right);
			}
			else if (logicOpClass is OrLogicOpClass)
			{
				OrLogicOpClass orLogicOpClass = (OrLogicOpClass)logicOpClass;
				queue.Enqueue(orLogicOpClass.m_left);
				queue.Enqueue(orLogicOpClass.m_right);
			}
			else if (!(logicOpClass is NegateLogicOpClass))
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
			}
			else
			{
				NegateLogicOpClass negateLogicOpClass = (NegateLogicOpClass)logicOpClass;
				if (negateLogicOpClass.m_target is ConstantLogicOpClass)
				{
					int myIndex2 = ((ConstantLogicOpClass)negateLogicOpClass.m_target).myIndex;
					if (myIndex2 < prereqs.Conditions.Count)
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
						if (prereqs.Conditions[myIndex2].ConditionType == QuestConditionType.HasDateTimePassed)
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
							timeRange.EndTime = new DateTime?(prereqs.Conditions[myIndex2].ToDateTime());
						}
					}
				}
				else if (negateLogicOpClass.m_target is AndLogicOpClass)
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
					AndLogicOpClass andLogicOpClass2 = (AndLogicOpClass)negateLogicOpClass.m_target;
					queue.Enqueue(new NegateLogicOpClass
					{
						m_target = andLogicOpClass2.m_left
					});
					queue.Enqueue(new NegateLogicOpClass
					{
						m_target = andLogicOpClass2.m_right
					});
				}
				else if (negateLogicOpClass.m_target is OrLogicOpClass)
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
					OrLogicOpClass orLogicOpClass2 = (OrLogicOpClass)negateLogicOpClass.m_target;
					queue.Enqueue(new NegateLogicOpClass
					{
						m_target = orLogicOpClass2.m_left
					});
					queue.Enqueue(new NegateLogicOpClass
					{
						m_target = orLogicOpClass2.m_right
					});
				}
				else if (negateLogicOpClass.m_target is NegateLogicOpClass)
				{
					negateLogicOpClass = (NegateLogicOpClass)negateLogicOpClass.m_target;
					queue.Enqueue(negateLogicOpClass.m_target);
				}
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
		return timeRange;
	}
}
