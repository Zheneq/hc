using System;

namespace AbilityContextNamespace
{
	public class ContextNameKeyPair
	{
		private string \u001D;

		private int \u000E;

		public ContextNameKeyPair(string \u001D)
		{
			this.\u001D = \u001D;
			this.\u000E = ContextVars.\u0015(\u001D);
		}

		public int \u0012()
		{
			return this.\u000E;
		}

		public string \u0012()
		{
			return this.\u001D;
		}
	}
}
