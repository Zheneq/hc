public class SlashCommand_UserReport : SlashCommand
{
	public SlashCommand_UserReport()
		: base("/report", SlashCommandType.Everywhere)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (!arguments.IsNullOrEmpty())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(ClientGameManager.Get() == null))
			{
				UILandingPageFullScreenMenus.Get().SetReportContainerVisible(true, arguments, 0L);
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		TextConsole.Get().Write(new TextConsole.Message
		{
			Text = StringUtil.TR("ReportNameError", "SlashCommand"),
			MessageType = ConsoleMessageType.SystemMessage
		});
	}
}
