using System;

namespace AbilityContextNamespace
{
	[Serializable]
	public class NumericContextValueCompareCond
	{
		public string m_contextName;

		public bool m_nonActorSpecificContext;

		public ContextCompareOp m_compareOp;

		public float m_testValue;

		public bool m_ignoreIfNoContext;

		private int m_contextKey;

		public int \u001D()
		{
			if (this.m_contextKey == 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(NumericContextValueCompareCond.\u001D()).MethodHandle;
				}
				this.m_contextKey = ContextVars.\u0015(this.m_contextName);
			}
			return this.m_contextKey;
		}

		public NumericContextValueCompareCond \u001D()
		{
			return base.MemberwiseClone() as NumericContextValueCompareCond;
		}
	}
}
