using System;

public class SlashCommand_UserReport : SlashCommand
{
	public SlashCommand_UserReport() : base("/report", SlashCommandType.Everywhere)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (!arguments.IsNullOrEmpty())
		{
			if (!(ClientGameManager.Get() == null))
			{
				UILandingPageFullScreenMenus.Get().SetReportContainerVisible(true, arguments, 0L, false);
				return;
			}
		}
		TextConsole.Get().Write(new TextConsole.Message
		{
			Text = StringUtil.TR("ReportNameError", "SlashCommand"),
			MessageType = ConsoleMessageType.SystemMessage
		}, null);
	}
}
