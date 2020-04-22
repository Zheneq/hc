using System;

namespace I2.Loc
{
	[Serializable]
	public class LanguageData
	{
		public string Name;

		public string Code;

		[NonSerialized]
		public bool Compressed;
	}
}
