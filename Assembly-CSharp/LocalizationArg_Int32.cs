using System;
using System.Globalization;

[Serializable]
public class LocalizationArg_Int32 : LocalizationArg
{
	public int m_value;

	public static LocalizationArg_Int32 Create(int value)
	{
		LocalizationArg_Int32 localizationArg_Int = new LocalizationArg_Int32();
		localizationArg_Int.m_value = value;
		return localizationArg_Int;
	}

	public override string TR()
	{
		return m_value.ToString("g", CultureInfo.CurrentCulture);
	}
}
