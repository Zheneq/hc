public class SlashCommand_EnableProfanityFilter : SlashCommand
{
	public SlashCommand_EnableProfanityFilter()
		: base("/profanityfilter", SlashCommandType.InFrontEnd)
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
			if (ClientGameManager.Get() == null)
			{
				while (true)
				{
					switch (2)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			TextConsole.Message message = default(TextConsole.Message);
			message.MessageType = ConsoleMessageType.SystemMessage;
			if (arguments.EqualsIgnoreCase(StringUtil.TR("on", "SlashCommand")))
			{
				Options_UI.Get().SetEnableProfanityFilter(true);
				message.Text = StringUtil.TR("ProfanityFilterEnabled", "SlashCommand");
				TextConsole.Get().Write(message);
				return;
			}
			if (!arguments.EqualsIgnoreCase(StringUtil.TR("off", "SlashCommand")))
			{
				return;
			}
			while (true)
			{
				Options_UI.Get().SetEnableProfanityFilter(false);
				message.Text = StringUtil.TR("ProfanityFilterDisabled", "SlashCommand");
				TextConsole.Get().Write(message);
				return;
			}
		}
	}
}
