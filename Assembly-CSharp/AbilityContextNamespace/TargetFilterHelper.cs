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
				if (_0012 != null)
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
					bool flag = _000E.GetTeam() == _0012.GetTeam();
					bool flag2 = _000E == _0012;
					if (_001D == TeamFilter._001D)
					{
						goto IL_00b3;
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
					if (_001D == TeamFilter._000E)
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
					if (_001D == TeamFilter._0015)
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
						if (flag)
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
							if (!flag2)
							{
								goto IL_00b3;
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
				while (true)
				{
					switch (7)
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
				for (int i = 0; i < _001D.Count; i++)
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
					NumericContextValueCompareCond numericContextValueCompareCond;
					bool flag2;
					int num2;
					if (flag)
					{
						numericContextValueCompareCond = _001D[i];
						if (numericContextValueCompareCond.m_compareOp == ContextCompareOp._001D)
						{
							continue;
						}
						if (string.IsNullOrEmpty(numericContextValueCompareCond.m_contextName))
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
							continue;
						}
						int contextKey = numericContextValueCompareCond.GetContextKey();
						flag2 = false;
						float num = 0f;
						ContextVars contextVars = _000E._0015;
						if (numericContextValueCompareCond.m_nonActorSpecificContext)
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
							contextVars = _0012;
						}
						if (contextVars.Contains(contextKey, ContextValueType.INT))
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
							num = contextVars.GetInt(contextKey);
							flag2 = true;
						}
						else if (contextVars.Contains(contextKey, ContextValueType.FLOAT))
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
							num = contextVars.GetFloat(contextKey);
							flag2 = true;
						}
						float testValue = numericContextValueCompareCond.m_testValue;
						ContextCompareOp compareOp = numericContextValueCompareCond.m_compareOp;
						if (flag2)
						{
							if (compareOp == ContextCompareOp._000E)
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
								if (testValue == num)
								{
									goto IL_0190;
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
							if (compareOp == ContextCompareOp._0012)
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
								if (Mathf.RoundToInt(testValue) == Mathf.RoundToInt(num))
								{
									goto IL_0190;
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
							if (compareOp == ContextCompareOp._0018)
							{
								if (num > testValue)
								{
									goto IL_0190;
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
							if (compareOp == ContextCompareOp._0013)
							{
								if (num >= testValue)
								{
									goto IL_0190;
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
							if (compareOp == ContextCompareOp._0016)
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
								if (num < testValue)
								{
									goto IL_0190;
								}
							}
							if (compareOp == ContextCompareOp._0015)
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
					IL_0190:
					num2 = 1;
					goto IL_0191;
					IL_0191:
					if (num2 == 0)
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
						flag = false;
					}
					goto IL_01a3;
					IL_01a3:
					if (!flag2)
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
