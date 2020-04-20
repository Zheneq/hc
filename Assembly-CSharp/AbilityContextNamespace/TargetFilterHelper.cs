using System;
using System.Collections.Generic;
using UnityEngine;

namespace AbilityContextNamespace
{
	public class TargetFilterHelper
	{
		public static bool symbol_001D(TargetFilterConditions symbol_001D, ActorData symbol_000E, ActorData symbol_0012, ActorHitContext symbol_0015, ContextVars symbol_0016)
		{
			return symbol_000E != null && symbol_0012 != null && TargetFilterHelper.symbol_001D(symbol_001D.m_teamFilter, symbol_000E, symbol_0012) && TargetFilterHelper.symbol_001D(symbol_001D.m_numCompareConditions, symbol_0015, symbol_0016);
		}

		public static bool symbol_001D(TeamFilter symbol_001D, ActorData symbol_000E, ActorData symbol_0012)
		{
			bool result = false;
			if (symbol_000E != null)
			{
				if (symbol_0012 != null)
				{
					bool flag = symbol_000E.GetTeam() == symbol_0012.GetTeam();
					bool flag2 = symbol_000E == symbol_0012;
					bool flag3;
					if (symbol_001D != TeamFilter.symbol_001D)
					{
						if (symbol_001D == TeamFilter.symbol_000E)
						{
							if (!flag)
							{
								goto IL_B3;
							}
						}
						if (symbol_001D == TeamFilter.symbol_0012)
						{
							if (flag)
							{
								goto IL_B3;
							}
						}
						if (symbol_001D == TeamFilter.symbol_0015)
						{
							if (flag)
							{
								if (!flag2)
								{
									goto IL_B3;
								}
							}
						}
						flag3 = (symbol_001D == TeamFilter.symbol_0016 && flag2);
						goto IL_B4;
					}
					IL_B3:
					flag3 = true;
					IL_B4:
					result = flag3;
				}
			}
			return result;
		}

		public static bool symbol_001D(List<NumericContextValueCompareCond> symbol_001D, ActorHitContext symbol_000E, ContextVars symbol_0012)
		{
			bool flag = true;
			if (symbol_001D != null)
			{
				int i = 0;
				while (i < symbol_001D.Count)
				{
					if (flag)
					{
						NumericContextValueCompareCond numericContextValueCompareCond = symbol_001D[i];
						if (numericContextValueCompareCond.m_compareOp != ContextCompareOp.symbol_001D)
						{
							if (string.IsNullOrEmpty(numericContextValueCompareCond.m_contextName))
							{
							}
							else
							{
								int contextKey = numericContextValueCompareCond.GetContextKey();
								bool flag2 = false;
								float num = 0f;
								ContextVars contextVars = symbol_000E.symbol_0015;
								if (numericContextValueCompareCond.m_nonActorSpecificContext)
								{
									contextVars = symbol_0012;
								}
								if (contextVars.Contains(contextKey, ContextValueType.INT))
								{
									num = (float)contextVars.GetInt(contextKey);
									flag2 = true;
								}
								else if (contextVars.Contains(contextKey, ContextValueType.FLOAT))
								{
									num = contextVars.GetFloat(contextKey);
									flag2 = true;
								}
								float testValue = numericContextValueCompareCond.m_testValue;
								ContextCompareOp compareOp = numericContextValueCompareCond.m_compareOp;
								if (flag2)
								{
									if (compareOp != ContextCompareOp.symbol_000E)
									{
										goto IL_104;
									}
									if (testValue == num)
									{
										goto IL_190;
									}
									for (;;)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										goto IL_104;
									}
									IL_191:
									bool flag3;
									if (!flag3)
									{
										flag = false;
										goto IL_1A3;
									}
									goto IL_1A3;
									IL_104:
									if (compareOp == ContextCompareOp.symbol_0012)
									{
										if (Mathf.RoundToInt(testValue) == Mathf.RoundToInt(num))
										{
											goto IL_190;
										}
									}
									if (compareOp == ContextCompareOp.symbol_0018)
									{
										if (num > testValue)
										{
											goto IL_190;
										}
									}
									if (compareOp == ContextCompareOp.symbol_0013)
									{
										if (num >= testValue)
										{
											goto IL_190;
										}
									}
									if (compareOp == ContextCompareOp.symbol_0016)
									{
										if (num < testValue)
										{
											goto IL_190;
										}
									}
									if (compareOp == ContextCompareOp.symbol_0015)
									{
										flag3 = (num <= testValue);
									}
									else
									{
										flag3 = false;
									}
									goto IL_191;
									IL_190:
									flag3 = true;
									goto IL_191;
								}
								IL_1A3:
								if (!flag2)
								{
									if (!numericContextValueCompareCond.m_ignoreIfNoContext)
									{
										flag = false;
									}
								}
							}
						}
						IL_1BB:
						i++;
						continue;
						goto IL_1BB;
					}
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						return flag;
					}
				}
			}
			return flag;
		}
	}
}
