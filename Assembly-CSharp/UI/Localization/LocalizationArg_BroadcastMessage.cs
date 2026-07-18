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
		return new LocalizationArg_BroadcastMessage
		{
			m_packedMessages = messages
		};
	}

	public static LocalizationArg_BroadcastMessage Create(Dictionary<string, string> messages, TimeSpan timeValue)
	{
		return new LocalizationArg_BroadcastMessage
		{
			m_packedMessages = messages,
			m_timeInAdvance = timeValue
		};
	}

	public override string TR()
	{
		string currentLanguageCode = LocalizationManager.CurrentLanguageCode;
		if (m_packedMessages.TryGetValue(currentLanguageCode, out string localizedMessage) && !localizedMessage.IsNullOrEmpty())
		{
			return m_packedMessages[currentLanguageCode].SafeReplace("${TIME}", StringUtil.GetTimeDifferenceText(m_timeInAdvance, true));

		}
		if (m_packedMessages.TryGetValue("en", out string message))
		{
			return message.SafeReplace("${TIME}", StringUtil.GetTimeDifferenceText(m_timeInAdvance, true));

		}
		return string.Empty;
	}
}
