using System;
using System.Collections.Generic;

namespace AbilityContextNamespace
{
	[Serializable]
	public class TargetFilterConditions
	{
		public TeamFilter m_teamFilter;

		public List<NumericContextValueCompareCond> m_numCompareConditions = new List<NumericContextValueCompareCond>();

		public TargetFilterConditions \u001D()
		{
			TargetFilterConditions targetFilterConditions = base.MemberwiseClone() as TargetFilterConditions;
			targetFilterConditions.m_numCompareConditions = new List<NumericContextValueCompareCond>();
			for (int i = 0; i < this.m_numCompareConditions.Count; i++)
			{
				NumericContextValueCompareCond item = this.m_numCompareConditions[i].\u001D();
				targetFilterConditions.m_numCompareConditions.Add(item);
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetFilterConditions.\u001D()).MethodHandle;
			}
			return targetFilterConditions;
		}

		public void \u001D(List<TooltipTokenEntry> \u001D, string \u000E)
		{
		}

		public string \u001D(string \u001D)
		{
			string text = \u001D + "Team = " + this.m_teamFilter.ToString() + "\n";
			if (this.m_numCompareConditions != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TargetFilterConditions.\u001D(string)).MethodHandle;
				}
				foreach (NumericContextValueCompareCond numericContextValueCompareCond in this.m_numCompareConditions)
				{
					if (numericContextValueCompareCond.m_compareOp != ContextCompareOp.\u001D)
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
						string text2 = text;
						text = string.Concat(new object[]
						{
							text2,
							\u001D,
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
