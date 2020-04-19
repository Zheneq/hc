using System;

public class SlashCommand_ShowAllChat : SlashCommand
{
	public SlashCommand_ShowAllChat() : base("/showallchat", SlashCommandType.Everywhere)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (!arguments.IsNullOrEmpty())
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SlashCommand_ShowAllChat.OnSlashCommand(string)).MethodHandle;
			}
			if (!(ClientGameManager.Get() == null))
			{
				TextConsole.Message message = default(TextConsole.Message);
				message.MessageType = ConsoleMessageType.SystemMessage;
				if (arguments.EqualsIgnoreCase(StringUtil.TR("on", "SlashCommand")))
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					Options_UI.Get().SetShowAllChat(true);
					message.Text = StringUtil.TR("AllChatEnabled", "SlashCommand");
					TextConsole.Get().Write(message, null);
				}
				else if (arguments.EqualsIgnoreCase(StringUtil.TR("off", "SlashCommand")))
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					Options_UI.Get().SetShowAllChat(false);
					message.Text = StringUtil.TR("AllChatDisabled", "SlashCommand");
					TextConsole.Get().Write(message, null);
				}
				return;
			}
		}
	}
}
