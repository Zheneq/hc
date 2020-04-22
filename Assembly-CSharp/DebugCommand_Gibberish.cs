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
			while (true)
			{
				switch (2)
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
			LocalizationManager.GibberishMode = !LocalizationManager.GibberishMode;
			if (LocalizationManager.GibberishMode)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				TextConsole.Get().Write("Gibberish mode on");
			}
			else
			{
				TextConsole.Get().Write("Gibberish mode off");
			}
		}
		return true;
	}
}
