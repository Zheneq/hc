using System;

public class SlashCommand_EnableProfanityFilter : SlashCommand
{
	public SlashCommand_EnableProfanityFilter() : base("/profanityfilter", SlashCommandType.InFrontEnd)
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
					Options_UI.Get().SetEnableProfanityFilter(true);
					message.Text = StringUtil.TR("ProfanityFilterEnabled", "SlashCommand");
					TextConsole.Get().Write(message, null);
				}
				else if (arguments.EqualsIgnoreCase(StringUtil.TR("off", "SlashCommand")))
				{
					Options_UI.Get().SetEnableProfanityFilter(false);
					message.Text = StringUtil.TR("ProfanityFilterDisabled", "SlashCommand");
					TextConsole.Get().Write(message, null);
				}
				return;
			}
		}
	}
}
