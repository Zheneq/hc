using System;

namespace AbilityContextNamespace
{
	[Serializable]
	public class HasContextCond
	{
		public string m_contextName;

		public ContextValueType m_valueType;

		private int m_contextKey;

		public int \u001D()
		{
			if (this.m_contextKey == 0)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(HasContextCond.\u001D()).MethodHandle;
				}
				this.m_contextKey = ContextVars.GetHash(this.m_contextName);
			}
			return this.m_contextKey;
		}
	}
}
