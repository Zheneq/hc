using System;

namespace AbilityContextNamespace
{
	public class ContextNameKeyPair
	{
		private string name;

		private int hash;

		public ContextNameKeyPair(string name)
		{
			this.name = name;
			this.hash = ContextVars.GetHash(name);
		}

		public int GetHash()
		{
			return this.hash;
		}

		public string GetName()
		{
			return this.name;
		}
	}
}
