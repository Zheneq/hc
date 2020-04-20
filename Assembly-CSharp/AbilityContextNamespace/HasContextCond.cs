using System;

namespace AbilityContextNamespace
{
	[Serializable]
	public class HasContextCond
	{
		public string m_contextName;

		public ContextValueType m_valueType;

		private int m_contextKey;

		public int symbol_001D()
		{
			if (this.m_contextKey == 0)
			{
				this.m_contextKey = ContextVars.GetHash(this.m_contextName);
			}
			return this.m_contextKey;
		}
	}
}
