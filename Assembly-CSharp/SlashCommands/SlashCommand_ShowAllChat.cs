public class SlashCommand_ShowAllChat : SlashCommand
{
    public SlashCommand_ShowAllChat()
        : base("/showallchat", SlashCommandType.Everywhere)
    {
    }

    public override void OnSlashCommand(string arguments)
    {
        if (arguments.IsNullOrEmpty() || ClientGameManager.Get() == null)
        {
            return;
        }

        TextConsole.Message message = new TextConsole.Message
        {
            MessageType = ConsoleMessageType.SystemMessage
        };
        if (arguments.EqualsIgnoreCase(StringUtil.TR("on", "SlashCommand")))
        {
            Options_UI.Get().SetShowAllChat(true);
            message.Text = StringUtil.TR("AllChatEnabled", "SlashCommand");
            TextConsole.Get().Write(message);
        }
        else if (arguments.EqualsIgnoreCase(StringUtil.TR("off", "SlashCommand")))
        {
            Options_UI.Get().SetShowAllChat(false);
            message.Text = StringUtil.TR("AllChatDisabled", "SlashCommand");
            TextConsole.Get().Write(message);
        }
    }
}