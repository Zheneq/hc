public class SlashCommand_Version : SlashCommand
{
	public SlashCommand_Version()
		: base("/version", SlashCommandType.Everywhere)
	{
		base.PublicFacing = true;
	}

	public override void OnSlashCommand(string arguments)
	{
		string text = $"Current Version: {BuildVersion.FullVersionString}";
		TextConsole.Get().Write(text);
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager.EnvironmentType == EnvironmentType.External)
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
			if (!clientGameManager.HasDeveloperAccess())
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		WinUtils.SetClipboardText(BuildVersion.FullVersionString);
	}
}
