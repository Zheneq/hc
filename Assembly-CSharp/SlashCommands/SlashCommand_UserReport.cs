public class SlashCommand_UserReport : SlashCommand
{
    public SlashCommand_UserReport()
        : base("/report", SlashCommandType.Everywhere)
    {
    }

    public override void OnSlashCommand(string arguments)
    {
        if (!arguments.IsNullOrEmpty() && ClientGameManager.Get() != null)
        {
            UILandingPageFullScreenMenus.Get().SetReportContainerVisible(true, arguments);
            return;
        }

        TextConsole.Get().Write(
            new TextConsole.Message
            {
                Text = StringUtil.TR("ReportNameError", "SlashCommand"),
                MessageType = ConsoleMessageType.SystemMessage
            });
    }
}