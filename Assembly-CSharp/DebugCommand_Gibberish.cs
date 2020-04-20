using System;
using I2.Loc;

public class DebugCommand_Gibberish : DebugCommand
{
	public override string GetSlashCommand()
	{
		return "/gibberish";
	}

	public override bool AvailableInFrontEnd()
	{
		return true;
	}

	public override bool OnSlashCommand(string arguments)
	{
		if (base.CheatEnabled)
		{
			LocalizationManager.GibberishMode = !LocalizationManager.GibberishMode;
			if (LocalizationManager.GibberishMode)
			{
				TextConsole.Get().Write("Gibberish mode on", ConsoleMessageType.SystemMessage);
			}
			else
			{
				TextConsole.Get().Write("Gibberish mode off", ConsoleMessageType.SystemMessage);
			}
		}
		return true;
	}
}
