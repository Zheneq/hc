using System;

[Serializable]
public class LocalizationArg_ChatChannel : LocalizationArg
{
	public ConsoleMessageType m_value;

	public static LocalizationArg_ChatChannel Create(ConsoleMessageType cmt)
	{
		LocalizationArg_ChatChannel localizationArg_ChatChannel = new LocalizationArg_ChatChannel();
		localizationArg_ChatChannel.m_value = cmt;
		return localizationArg_ChatChannel;
	}

	public override string TR()
	{
		return StringUtil.TR($"ChannelName_{m_value.ToString()}", "Chat");
	}
}
