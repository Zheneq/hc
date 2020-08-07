namespace AbilityContextNamespace
{
	public class ContextNameKeyPair
	{
		private string name;

		private int key;

		public ContextNameKeyPair(string name)
		{
			this.name = name;
			key = ContextVars.GetKey(name);
		}

		public int GetKey()
		{
			return key;
		}

		public string GetName()
		{
			return name;
		}
	}
}
