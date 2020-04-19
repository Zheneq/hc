using System;

namespace I2.Loc
{
	public enum TranslationFlag : byte
	{
		AutoTranslated_Normal = 1,
		AutoTranslated_Touch,
		AutoTranslated_All = 0xFF
	}
}
