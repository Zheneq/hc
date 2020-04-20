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
			if (!(ClientGameManager.Get() == null))
			{
				TextConsole.Message message = default(TextConsole.Message);
				message.MessageType = ConsoleMessageType.SystemMessage;
				if (arguments.EqualsIgnoreCase(StringUtil.TR("on", "SlashCommand")))
				{
					Options_UI.Get().SetShowAllChat(true);
					message.Text = StringUtil.TR("AllChatEnabled", "SlashCommand");
					TextConsole.Get().Write(message, null);
				}
				else if (arguments.EqualsIgnoreCase(StringUtil.TR("off", "SlashCommand")))
				{
					Options_UI.Get().SetShowAllChat(false);
					message.Text = StringUtil.TR("AllChatDisabled", "SlashCommand");
					TextConsole.Get().Write(message, null);
				}
				return;
			}
		}
	}
}
