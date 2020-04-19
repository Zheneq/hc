using System;
using System.Collections.Generic;
using UnityEngine;

namespace AbilityContextNamespace
{
	public class TargetFilterHelper
	{
		public static bool \u001D(TargetFilterConditions \u001D, ActorData \u000E, ActorData \u0012, ActorHitContext \u0015, ContextVars \u0016)
		{
			return \u000E != null && \u0012 != null && TargetFilterHelper.\u001D(\u001D.m_teamFilter, \u000E, \u0012) && TargetFilterHelper.\u001D(\u001D.m_numCompareConditions, \u0015, \u0016);
		}

		public static bool \u001D(TeamFilter \u001D, ActorData \u000E, ActorData \u0012)
		{
			bool result = false;
			if (\u000E != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TargetFilterHelper.\u001D(TeamFilter, ActorData, ActorData)).MethodHandle;
				}
				if (\u0012 != null)
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
					bool flag = \u000E.\u000E() == \u0012.\u000E();
					bool flag2 = \u000E == \u0012;
					bool flag3;
					if (\u001D != TeamFilter.\u001D)
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
						if (\u001D == TeamFilter.\u000E)
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
							if (!flag)
							{
								goto IL_B3;
							}
						}
						if (\u001D == TeamFilter.\u0012)
						{
							if (flag)
							{
								goto IL_B3;
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
						if (\u001D == TeamFilter.\u0015)
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
							if (flag)
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
								if (!flag2)
								{
									goto IL_B3;
								}
								for (;;)
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
						flag3 = (\u001D == TeamFilter.\u0016 && flag2);
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

		public static bool \u001D(List<NumericContextValueCompareCond> \u001D, ActorHitContext \u000E, ContextVars \u0012)
		{
			bool flag = true;
			if (\u001D != null)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TargetFilterHelper.\u001D(List<NumericContextValueCompareCond>, ActorHitContext, ContextVars)).MethodHandle;
				}
				int i = 0;
				while (i < \u001D.Count)
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
					if (flag)
					{
						NumericContextValueCompareCond numericContextValueCompareCond = \u001D[i];
						if (numericContextValueCompareCond.m_compareOp != ContextCompareOp.\u001D)
						{
							if (string.IsNullOrEmpty(numericContextValueCompareCond.m_contextName))
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
							}
							else
							{
								int u001D = numericContextValueCompareCond.\u001D();
								bool flag2 = false;
								float num = 0f;
								ContextVars contextVars = \u000E.\u0015;
								if (numericContextValueCompareCond.m_nonActorSpecificContext)
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
									contextVars = \u0012;
								}
								if (contextVars.\u0015(u001D, ContextValueType.\u001D))
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
									num = (float)contextVars.\u0015(u001D);
									flag2 = true;
								}
								else if (contextVars.\u0015(u001D, ContextValueType.\u000E))
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
									num = contextVars.\u0015(u001D);
									flag2 = true;
								}
								float testValue = numericContextValueCompareCond.m_testValue;
								ContextCompareOp compareOp = numericContextValueCompareCond.m_compareOp;
								if (flag2)
								{
									if (compareOp != ContextCompareOp.\u000E)
									{
										goto IL_104;
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
										for (;;)
										{
											switch (6)
											{
											case 0:
												continue;
											}
											break;
										}
										flag = false;
										goto IL_1A3;
									}
									goto IL_1A3;
									IL_104:
									if (compareOp == ContextCompareOp.\u0012)
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
										if (Mathf.RoundToInt(testValue) == Mathf.RoundToInt(num))
										{
											goto IL_190;
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
									if (compareOp == ContextCompareOp.\u0018)
									{
										if (num > testValue)
										{
											goto IL_190;
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
									if (compareOp == ContextCompareOp.\u0013)
									{
										if (num >= testValue)
										{
											goto IL_190;
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
									if (compareOp == ContextCompareOp.\u0016)
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
										if (num < testValue)
										{
											goto IL_190;
										}
									}
									if (compareOp == ContextCompareOp.\u0015)
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
									for (;;)
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
