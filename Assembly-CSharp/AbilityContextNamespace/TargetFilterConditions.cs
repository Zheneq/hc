using System;
using System.Collections.Generic;

namespace AbilityContextNamespace
{
	[Serializable]
	public class TargetFilterConditions
	{
		public TeamFilter m_teamFilter;

		public List<NumericContextValueCompareCond> m_numCompareConditions = new List<NumericContextValueCompareCond>();

		public TargetFilterConditions symbol_001D()
		{
			TargetFilterConditions targetFilterConditions = base.MemberwiseClone() as TargetFilterConditions;
			targetFilterConditions.m_numCompareConditions = new List<NumericContextValueCompareCond>();
			for (int i = 0; i < this.m_numCompareConditions.Count; i++)
			{
				NumericContextValueCompareCond item = this.m_numCompareConditions[i].Clone();
				targetFilterConditions.m_numCompareConditions.Add(item);
			}
			return targetFilterConditions;
		}

		public void symbol_001D(List<TooltipTokenEntry> symbol_001D, string symbol_000E)
		{
		}

		public string symbol_001D(string symbol_001D)
		{
			string text = symbol_001D + "Team = " + this.m_teamFilter.ToString() + "\n";
			if (this.m_numCompareConditions != null)
			{
				foreach (NumericContextValueCompareCond numericContextValueCompareCond in this.m_numCompareConditions)
				{
					if (numericContextValueCompareCond.m_compareOp != ContextCompareOp.symbol_001D)
					{
						string text2 = text;
						text = string.Concat(new object[]
						{
							text2,
							symbol_001D,
							InEditorDescHelper.ContextVarName(numericContextValueCompareCond.m_contextName, !numericContextValueCompareCond.m_nonActorSpecificContext),
							" is ",
							numericContextValueCompareCond.m_compareOp,
							" ",
							InEditorDescHelper.ColoredString(numericContextValueCompareCond.m_testValue, "cyan", false),
							"\n"
						});
					}
				}
			}
			return text;
		}
	}
}
