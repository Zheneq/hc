public class SlashCommand_ShowGeneralChat : SlashCommand
{
    public SlashCommand_ShowGeneralChat()
        : base("/showgeneralchat", SlashCommandType.InFrontEnd)
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
            Options_UI.Get().SetShowGlobalChat(true);
            message.Text = StringUtil.TR("GlobalChatDisplayed", "SlashCommand");
            TextConsole.Get().Write(message);
        }
        else if (arguments.EqualsIgnoreCase(StringUtil.TR("off", "SlashCommand")))
        {
            Options_UI.Get().SetShowGlobalChat(false);
            message.Text = StringUtil.TR("GlobalChatHidden", "SlashCommand");
            TextConsole.Get().Write(message);
        }
    }
}