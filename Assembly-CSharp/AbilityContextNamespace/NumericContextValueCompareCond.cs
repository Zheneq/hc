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

		public int GetContextKey()
		{
			if (this.m_contextKey == 0)
			{
				this.m_contextKey = ContextVars.GetHash(this.m_contextName);
			}
			return this.m_contextKey;
		}

		public NumericContextValueCompareCond Clone()
		{
			return base.MemberwiseClone() as NumericContextValueCompareCond;
		}
	}
}
