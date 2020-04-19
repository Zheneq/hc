using System;

[Serializable]
public class LocalizationArg_ChatChannel : LocalizationArg
{
	public ConsoleMessageType m_value;

	public static LocalizationArg_ChatChannel Create(ConsoleMessageType cmt)
	{
		return new LocalizationArg_ChatChannel
		{
			m_value = cmt
		};
	}

	public override string TR()
	{
		return StringUtil.TR(string.Format("ChannelName_{0}", this.m_value.ToString()), "Chat");
	}
}
