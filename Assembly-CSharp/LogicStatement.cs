using System;
using System.Collections.Generic;

public static class LogicStatement
{
	private static readonly Dictionary<string, LogicOpClass> s_logicStatementCache = new Dictionary<string, LogicOpClass>();

	public static LogicOpClass EvaluateLogicStatement(string logicStatement)
	{
		if (s_logicStatementCache.ContainsKey(logicStatement))
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
					return s_logicStatementCache[logicStatement];
				}
			}
		}
		int i = 0;
		string text = logicStatement.Trim();
		char[] array = text.ToCharArray();
		LogicOpClass logicOpClass = null;
		bool flag = false;
		for (; i < array.Length; i++)
		{
			char c = array[i];
			if (c == '!')
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
				flag = true;
			}
			else if (c == '(')
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
				int num = -1;
				int num2 = i + 1;
				int num3 = 0;
				while (num == -1)
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
					if (num2 < text.Length)
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
							while (true)
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
					break;
				}
				if (num == -1)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							Log.Warning("Error, part of the logic statement (invalid parens) passed to quest evaluation was bad.  Returning false!");
							return new ConstantLogicOpClass();
						}
					}
				}
				string logicStatement2 = text.Substring(i + 1, num - (i + 1));
				LogicOpClass logicOpClass2 = EvaluateLogicStatement(logicStatement2);
				if (flag)
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
					NegateLogicOpClass negateLogicOpClass = new NegateLogicOpClass();
					negateLogicOpClass.m_target = logicOpClass2;
					logicOpClass = negateLogicOpClass;
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
				orLogicOpClass.m_right = EvaluateLogicStatement(logicStatement3);
				i = text.Length;
				logicOpClass = orLogicOpClass;
			}
			else if (c == '&')
			{
				AndLogicOpClass andLogicOpClass = new AndLogicOpClass();
				andLogicOpClass.m_left = logicOpClass;
				string logicStatement4 = text.Substring(i + 1);
				andLogicOpClass.m_right = EvaluateLogicStatement(logicStatement4);
				i = text.Length;
				logicOpClass = andLogicOpClass;
			}
			else
			{
				if (c < 'A')
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
					break;
				}
				if (c <= 'Z')
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
					ConstantLogicOpClass constantLogicOpClass = new ConstantLogicOpClass();
					int num4 = constantLogicOpClass.myIndex = c - 65;
					if (flag)
					{
						NegateLogicOpClass negateLogicOpClass2 = new NegateLogicOpClass();
						negateLogicOpClass2.m_target = constantLogicOpClass;
						logicOpClass = negateLogicOpClass2;
					}
					else
					{
						logicOpClass = constantLogicOpClass;
					}
				}
			}
		}
		if (logicOpClass == null)
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
			Log.Warning("Error, part of the logic statement {0} passed to quest evaluation was bad.  Returning false!", logicStatement);
			logicOpClass = new ConstantLogicOpClass();
		}
		s_logicStatementCache[logicStatement] = logicOpClass;
		return logicOpClass;
	}

	public static DateTime ToDateTime(this QuestCondition dateTimeCondition)
	{
		List<int> typeSpecificDate = dateTimeCondition.typeSpecificDate;
		if (!typeSpecificDate.IsNullOrEmpty())
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
			if (typeSpecificDate.Count >= 6)
			{
				return new DateTime(typeSpecificDate[0], typeSpecificDate[1], typeSpecificDate[2], typeSpecificDate[3], typeSpecificDate[4], typeSpecificDate[5]);
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
		}
		return DateTime.MinValue;
	}

	public static TimeRange GetTimeRange(this QuestPrerequisites prereqs)
	{
		Queue<LogicOpClass> queue = new Queue<LogicOpClass>();
		string text = prereqs.LogicStatement;
		if (text.IsNullOrEmpty())
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
			int num = 65;
			for (int i = 0; i < prereqs.Conditions.Count; i++)
			{
				text = ((!text.IsNullOrEmpty()) ? (text + " & " + Convert.ToChar(num)) : Convert.ToChar(num).ToString());
				num++;
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
		queue.Enqueue(EvaluateLogicStatement(text));
		TimeRange timeRange = new TimeRange();
		while (queue.Count > 0)
		{
			LogicOpClass logicOpClass = queue.Dequeue();
			if (logicOpClass is ConstantLogicOpClass)
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
				int myIndex = ((ConstantLogicOpClass)logicOpClass).myIndex;
				if (myIndex >= prereqs.Conditions.Count)
				{
					continue;
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
				if (prereqs.Conditions[myIndex].ConditionType == QuestConditionType.HasDateTimePassed)
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
					timeRange.StartTime = prereqs.Conditions[myIndex].ToDateTime();
				}
				continue;
			}
			if (logicOpClass is AndLogicOpClass)
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
				AndLogicOpClass andLogicOpClass = (AndLogicOpClass)logicOpClass;
				queue.Enqueue(andLogicOpClass.m_left);
				queue.Enqueue(andLogicOpClass.m_right);
				continue;
			}
			if (logicOpClass is OrLogicOpClass)
			{
				OrLogicOpClass orLogicOpClass = (OrLogicOpClass)logicOpClass;
				queue.Enqueue(orLogicOpClass.m_left);
				queue.Enqueue(orLogicOpClass.m_right);
				continue;
			}
			if (!(logicOpClass is NegateLogicOpClass))
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
				continue;
			}
			NegateLogicOpClass negateLogicOpClass = (NegateLogicOpClass)logicOpClass;
			if (negateLogicOpClass.m_target is ConstantLogicOpClass)
			{
				int myIndex2 = ((ConstantLogicOpClass)negateLogicOpClass.m_target).myIndex;
				if (myIndex2 >= prereqs.Conditions.Count)
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
				if (prereqs.Conditions[myIndex2].ConditionType == QuestConditionType.HasDateTimePassed)
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
					timeRange.EndTime = prereqs.Conditions[myIndex2].ToDateTime();
				}
			}
			else if (negateLogicOpClass.m_target is AndLogicOpClass)
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
					switch (7)
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
			switch (7)
			{
			case 0:
				continue;
			}
			return timeRange;
		}
	}
}
