using System;

namespace AbilityContextNamespace
{
	[Serializable]
	public class HasContextCond
	{
		public string m_contextName;

		public ContextValueType m_valueType;

		private int m_contextKey;

		public int _001D()
		{
			if (m_contextKey == 0)
			{
				m_contextKey = ContextVars.ToContextKey(m_contextName);
			}
			return m_contextKey;
		}
	}
}
