using System;

[Serializable]
public class LocalizationArg_LocalizationPayload : LocalizationArg
{
	public LocalizationPayload m_payload;

	public static LocalizationArg_LocalizationPayload Create(LocalizationPayload payload)
	{
		return new LocalizationArg_LocalizationPayload
		{
			m_payload = payload
		};
	}

	public override string TR()
	{
		return m_payload.ToString();
	}
}
