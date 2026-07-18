using System;
using System.Globalization;

[Serializable]
public class LocalizationArg_Int32 : LocalizationArg
{
	public int m_value;

	public static LocalizationArg_Int32 Create(int value)
	{
		return new LocalizationArg_Int32
		{
			m_value = value
		};
	}

	public override string TR()
	{
		return m_value.ToString("g", CultureInfo.CurrentCulture);
	}
}
