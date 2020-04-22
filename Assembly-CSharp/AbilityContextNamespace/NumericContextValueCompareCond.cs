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
			if (m_contextKey == 0)
			{
				m_contextKey = ContextVars.GetHash(m_contextName);
			}
			return m_contextKey;
		}

		public NumericContextValueCompareCond Clone()
		{
			return MemberwiseClone() as NumericContextValueCompareCond;
		}
	}
}
