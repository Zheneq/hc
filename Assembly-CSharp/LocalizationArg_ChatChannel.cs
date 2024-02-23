using System;
using System.Text;

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
		return StringUtil.TR(new StringBuilder().Append("ChannelName_").Append(m_value.ToString()).ToString(), "Chat");
	}
}
