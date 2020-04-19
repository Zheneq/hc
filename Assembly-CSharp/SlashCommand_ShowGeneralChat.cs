using System;

public class SlashCommand_ShowGeneralChat : SlashCommand
{
	public SlashCommand_ShowGeneralChat() : base("/showgeneralchat", SlashCommandType.InFrontEnd)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (arguments.IsNullOrEmpty() || ClientGameManager.Get() == null)
		{
			return;
		}
		TextConsole.Message message = default(TextConsole.Message);
		message.MessageType = ConsoleMessageType.SystemMessage;
		if (arguments.EqualsIgnoreCase(StringUtil.TR("on", "SlashCommand")))
		{
			Options_UI.Get().SetShowGlobalChat(true);
			message.Text = StringUtil.TR("GlobalChatDisplayed", "SlashCommand");
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SlashCommand_ShowGeneralChat.OnSlashCommand(string)).MethodHandle;
			}
			Options_UI.Get().SetShowGlobalChat(false);
			message.Text = StringUtil.TR("GlobalChatHidden", "SlashCommand");
			TextConsole.Get().Write(message, null);
		}
	}
}
