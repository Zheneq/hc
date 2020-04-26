namespace AbilityContextNamespace
{
	public class ContextNameKeyPair
	{
		private string name;

		private int hash;

		public ContextNameKeyPair(string name)
		{
			this.name = name;
			hash = ContextVars.GetHash(name);
		}

		public int GetHash()
		{
			return hash;
		}

		public string GetName()
		{
			return name;
		}
	}
}
