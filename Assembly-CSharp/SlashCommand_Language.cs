using System;
using I2.Loc;

public class SlashCommand_Language : SlashCommand
{
	public SlashCommand_Language() : base("/language", SlashCommandType.Everywhere)
	{
		base.PublicFacing = false;
	}

	public override void OnSlashCommand(string arguments)
	{
		if (!arguments.IsNullOrEmpty())
		{
			if (LocalizationManager.HasLanguage(arguments, true, true))
			{
				LocalizationManager.CurrentLanguage = arguments;
				string text = string.Format("Language changed to: {0}", LocalizationManager.CurrentLanguage);
				TextConsole.Get().Write(text, ConsoleMessageType.SystemMessage);
				ClientGameManager.Get().SetNewSessionLanguage(LocalizationManager.CurrentLanguageCode);
			}
			else
			{
				TextConsole.Get().Write(string.Format("Unrecognized Language: {0}", arguments), ConsoleMessageType.SystemMessage);
			}
		}
		else
		{
			string text2 = string.Format("Current Language: {0}", LocalizationManager.CurrentLanguage);
			TextConsole.Get().Write(text2, ConsoleMessageType.SystemMessage);
		}
	}
}
