using System;

[Serializable]
public class LocalizationArg_LocalizationPayload : LocalizationArg
{
	public LocalizationPayload m_payload;

	public static LocalizationArg_LocalizationPayload Create(LocalizationPayload payload)
	{
		LocalizationArg_LocalizationPayload localizationArg_LocalizationPayload = new LocalizationArg_LocalizationPayload();
		localizationArg_LocalizationPayload.m_payload = payload;
		return localizationArg_LocalizationPayload;
	}

	public override string TR()
	{
		return m_payload.ToString();
	}
}
