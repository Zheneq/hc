using System;

[Serializable]
public class LocalizationArg_Handle : LocalizationArg
{
	public string m_handle;

	public static LocalizationArg_Handle Create(string handle)
	{
		LocalizationArg_Handle localizationArg_Handle = new LocalizationArg_Handle();
		localizationArg_Handle.m_handle = handle;
		return localizationArg_Handle;
	}

	public override string TR()
	{
		return m_handle;
	}
}
