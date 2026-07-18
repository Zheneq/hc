public class SlashCommand_EnableProfanityFilter : SlashCommand
{
    public SlashCommand_EnableProfanityFilter()
        : base("/profanityfilter", SlashCommandType.InFrontEnd)
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
            Options_UI.Get().SetEnableProfanityFilter(true);
            message.Text = StringUtil.TR("ProfanityFilterEnabled", "SlashCommand");
            TextConsole.Get().Write(message);
        }
        else if (arguments.EqualsIgnoreCase(StringUtil.TR("off", "SlashCommand")))
        {
            Options_UI.Get().SetEnableProfanityFilter(false);
            message.Text = StringUtil.TR("ProfanityFilterDisabled", "SlashCommand");
            TextConsole.Get().Write(message);
        }
    }
}