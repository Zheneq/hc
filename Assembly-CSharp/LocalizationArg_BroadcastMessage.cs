using I2.Loc;
using System;
using System.Collections.Generic;

[Serializable]
public class LocalizationArg_BroadcastMessage : LocalizationArg
{
	public Dictionary<string, string> m_packedMessages;

	private TimeSpan m_timeInAdvance;

	public static LocalizationArg_BroadcastMessage Create(Dictionary<string, string> messages)
	{
		LocalizationArg_BroadcastMessage localizationArg_BroadcastMessage = new LocalizationArg_BroadcastMessage();
		localizationArg_BroadcastMessage.m_packedMessages = messages;
		return localizationArg_BroadcastMessage;
	}

	public static LocalizationArg_BroadcastMessage Create(Dictionary<string, string> messages, TimeSpan timeValue)
	{
		LocalizationArg_BroadcastMessage localizationArg_BroadcastMessage = new LocalizationArg_BroadcastMessage();
		localizationArg_BroadcastMessage.m_packedMessages = messages;
		localizationArg_BroadcastMessage.m_timeInAdvance = timeValue;
		return localizationArg_BroadcastMessage;
	}

	public override string TR()
	{
		string currentLanguageCode = LocalizationManager.CurrentLanguageCode;
		if (m_packedMessages.ContainsKey(currentLanguageCode))
		{
			if (!m_packedMessages[currentLanguageCode].IsNullOrEmpty())
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return m_packedMessages[currentLanguageCode].SafeReplace("${TIME}", StringUtil.GetTimeDifferenceText(m_timeInAdvance, true));
					}
				}
			}
		}
		if (m_packedMessages.ContainsKey("en"))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_packedMessages["en"].SafeReplace("${TIME}", StringUtil.GetTimeDifferenceText(m_timeInAdvance, true));
				}
			}
		}
		return string.Empty;
	}
}
