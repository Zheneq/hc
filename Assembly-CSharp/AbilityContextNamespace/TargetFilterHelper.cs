using System.Collections.Generic;
using UnityEngine;

namespace AbilityContextNamespace
{
	public class TargetFilterHelper
	{
		public static bool _001D(TargetFilterConditions _001D, ActorData _000E, ActorData _0012, ActorHitContext _0015, ContextVars _0016)
		{
			bool flag = true;
			if (_000E != null && _0012 != null)
			{
				return TargetFilterHelper._001D(_001D.m_teamFilter, _000E, _0012) && TargetFilterHelper._001D(_001D.m_numCompareConditions, _0015, _0016);
			}
			return false;
		}

		public static bool _001D(TeamFilter _001D, ActorData _000E, ActorData _0012)
		{
			bool result = false;
			int num;
			if (_000E != null)
			{
				if (_0012 != null)
				{
					bool flag = _000E.GetTeam() == _0012.GetTeam();
					bool flag2 = _000E == _0012;
					if (_001D == TeamFilter._001D)
					{
						goto IL_00b3;
					}
					if (_001D == TeamFilter._000E)
					{
						if (!flag)
						{
							goto IL_00b3;
						}
					}
					if (_001D == TeamFilter._0012)
					{
						if (flag)
						{
							goto IL_00b3;
						}
					}
					if (_001D == TeamFilter._0015)
					{
						if (flag)
						{
							if (!flag2)
							{
								goto IL_00b3;
							}
						}
					}
					num = ((_001D == TeamFilter._0016 && flag2) ? 1 : 0);
					goto IL_00b4;
				}
			}
			goto IL_00b5;
			IL_00b4:
			result = ((byte)num != 0);
			goto IL_00b5;
			IL_00b5:
			return result;
			IL_00b3:
			num = 1;
			goto IL_00b4;
		}

		public static bool _001D(List<NumericContextValueCompareCond> _001D, ActorHitContext _000E, ContextVars _0012)
		{
			bool flag = true;
			if (_001D != null)
			{
				for (int i = 0; i < _001D.Count; i++)
				{
					NumericContextValueCompareCond numericContextValueCompareCond;
					bool flag2;
					int num2;
					if (flag)
					{
						numericContextValueCompareCond = _001D[i];
						if (numericContextValueCompareCond.m_compareOp == ContextCompareOp.Ignore)
						{
							continue;
						}
						if (string.IsNullOrEmpty(numericContextValueCompareCond.m_contextName))
						{
							continue;
						}
						int contextKey = numericContextValueCompareCond.GetContextKey();
						flag2 = false;
						float num = 0f;
						ContextVars contextVars = _000E.m_contextVars;
						if (numericContextValueCompareCond.m_nonActorSpecificContext)
						{
							contextVars = _0012;
						}
						if (contextVars.HasVar(contextKey, ContextValueType.Int))
						{
							num = contextVars.GetValueInt(contextKey);
							flag2 = true;
						}
						else if (contextVars.HasVar(contextKey, ContextValueType.Float))
						{
							num = contextVars.GetValueFloat(contextKey);
							flag2 = true;
						}
						float testValue = numericContextValueCompareCond.m_testValue;
						ContextCompareOp compareOp = numericContextValueCompareCond.m_compareOp;
						if (flag2)
						{
							if (compareOp == ContextCompareOp.Equals)
							{
								if (testValue == num)
								{
									goto IL_0190;
								}
							}
							if (compareOp == ContextCompareOp.EqualsRoundToInt)
							{
								if (Mathf.RoundToInt(testValue) == Mathf.RoundToInt(num))
								{
									goto IL_0190;
								}
							}
							if (compareOp == ContextCompareOp.GreaterThan)
							{
								if (num > testValue)
								{
									goto IL_0190;
								}
							}
							if (compareOp == ContextCompareOp.GreaterThanOrEqual)
							{
								if (num >= testValue)
								{
									goto IL_0190;
								}
							}
							if (compareOp == ContextCompareOp.LessThan)
							{
								if (num < testValue)
								{
									goto IL_0190;
								}
							}
							if (compareOp == ContextCompareOp.LessThanOrEqual)
							{
								num2 = ((num <= testValue) ? 1 : 0);
							}
							else
							{
								num2 = 0;
							}
							goto IL_0191;
						}
						goto IL_01a3;
					}
					break;
					IL_0190:
					num2 = 1;
					goto IL_0191;
					IL_0191:
					if (num2 == 0)
					{
						flag = false;
					}
					goto IL_01a3;
					IL_01a3:
					if (!flag2)
					{
						if (!numericContextValueCompareCond.m_ignoreIfNoContext)
						{
							flag = false;
						}
					}
				}
			}
			return flag;
		}
	}
}
