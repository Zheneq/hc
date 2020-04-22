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
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_contextKey = ContextVars.GetHash(m_contextName);
			}
			return m_contextKey;
		}
	}
}
