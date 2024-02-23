using System;
using System.Collections.Generic;
using System.Text;

namespace AbilityContextNamespace
{
	[Serializable]
	public class TargetFilterConditions
	{
		public TeamFilter m_teamFilter;
		public List<NumericContextValueCompareCond> m_numCompareConditions = new List<NumericContextValueCompareCond>();

		public TargetFilterConditions GetCopy()
		{
			TargetFilterConditions targetFilterConditions = MemberwiseClone() as TargetFilterConditions;
			targetFilterConditions.m_numCompareConditions = new List<NumericContextValueCompareCond>();
			for (int i = 0; i < m_numCompareConditions.Count; i++)
			{
				NumericContextValueCompareCond item = m_numCompareConditions[i].GetCopy();
				targetFilterConditions.m_numCompareConditions.Add(item);
			}
			return targetFilterConditions;
		}

		public void AddTooltipTokens(List<TooltipTokenEntry> tokens, string prefix)
		{
		}

		public string GetInEditorDesc(string indent)
		{
			string text = new StringBuilder().Append(indent).Append("Team = ").Append(m_teamFilter.ToString()).Append("\n").ToString();
			if (m_numCompareConditions != null)
			{
				foreach (NumericContextValueCompareCond numCompareCondition in m_numCompareConditions)
				{
					if (numCompareCondition.m_compareOp != ContextCompareOp.Ignore)
					{
						text = string.Concat(
							text,
							indent,
							InEditorDescHelper.ContextVarName(numCompareCondition.m_contextName, !numCompareCondition.m_nonActorSpecificContext),
							" is ",
							numCompareCondition.m_compareOp,
							" ",
							InEditorDescHelper.ColoredString(numCompareCondition.m_testValue),
							"\n");
					}
				}
			}
			return text;
		}
	}
}
