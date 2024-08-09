public class SlashCommand_Version : SlashCommand
{
	public SlashCommand_Version()
		: base("/version", SlashCommandType.Everywhere)
	{
		PublicFacing = true;
	}

	public override void OnSlashCommand(string arguments)
	{
#if EVOS
        TextConsole.Get().Write($"Current Version: {BuildVersion.s_version}");
#else
        TextConsole.Get().Write($"Current Version: {BuildVersion.FullVersionString}");
#endif
        ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager.EnvironmentType != EnvironmentType.External || clientGameManager.HasDeveloperAccess())
		{
			WinUtils.SetClipboardText(BuildVersion.FullVersionString);
		}
	}
}
