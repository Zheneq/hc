public class SlashCommand_ShowAllChat : SlashCommand
{
	public SlashCommand_ShowAllChat()
		: base("/showallchat", SlashCommandType.Everywhere)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (arguments.IsNullOrEmpty())
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (ClientGameManager.Get() == null)
			{
				return;
			}
			TextConsole.Message message = default(TextConsole.Message);
			message.MessageType = ConsoleMessageType.SystemMessage;
			if (arguments.EqualsIgnoreCase(StringUtil.TR("on", "SlashCommand")))
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						Options_UI.Get().SetShowAllChat(true);
						message.Text = StringUtil.TR("AllChatEnabled", "SlashCommand");
						TextConsole.Get().Write(message);
						return;
					}
				}
			}
			if (!arguments.EqualsIgnoreCase(StringUtil.TR("off", "SlashCommand")))
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				Options_UI.Get().SetShowAllChat(false);
				message.Text = StringUtil.TR("AllChatDisabled", "SlashCommand");
				TextConsole.Get().Write(message);
				return;
			}
		}
	}
}
