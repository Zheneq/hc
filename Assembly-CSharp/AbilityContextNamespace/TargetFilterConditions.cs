using System;
using System.Collections.Generic;

namespace AbilityContextNamespace
{
	[Serializable]
	public class TargetFilterConditions
	{
		public TeamFilter m_teamFilter;

		public List<NumericContextValueCompareCond> m_numCompareConditions = new List<NumericContextValueCompareCond>();

		public TargetFilterConditions _001D()
		{
			TargetFilterConditions targetFilterConditions = MemberwiseClone() as TargetFilterConditions;
			targetFilterConditions.m_numCompareConditions = new List<NumericContextValueCompareCond>();
			for (int i = 0; i < m_numCompareConditions.Count; i++)
			{
				NumericContextValueCompareCond item = m_numCompareConditions[i].Clone();
				targetFilterConditions.m_numCompareConditions.Add(item);
			}
			while (true)
			{
				return targetFilterConditions;
			}
		}

		public void _001D(List<TooltipTokenEntry> _001D, string _000E)
		{
		}

		public string _001D(string _001D)
		{
			string text = _001D + "Team = " + m_teamFilter.ToString() + "\n";
			if (m_numCompareConditions != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						{
							foreach (NumericContextValueCompareCond numCompareCondition in m_numCompareConditions)
							{
								if (numCompareCondition.m_compareOp != 0)
								{
									string text2 = text;
									text = string.Concat(text2, _001D, InEditorDescHelper.ContextVarName(numCompareCondition.m_contextName, !numCompareCondition.m_nonActorSpecificContext), " is ", numCompareCondition.m_compareOp, " ", InEditorDescHelper.ColoredString(numCompareCondition.m_testValue), "\n");
								}
							}
							return text;
						}
					}
				}
			}
			return text;
		}
	}
}
