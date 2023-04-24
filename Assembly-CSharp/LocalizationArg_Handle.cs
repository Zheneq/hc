using System;

[Serializable]
public class LocalizationArg_Handle : LocalizationArg
{
	public string m_handle;

	public static LocalizationArg_Handle Create(string handle)
	{
		return new LocalizationArg_Handle
		{
			m_handle = handle
		};
	}

	public override string TR()
	{
		return m_handle;
	}
}
