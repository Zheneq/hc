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
			if (!clientGameManager.HasDeveloperAccess())
			{
				return;
			}
		}
		WinUtils.SetClipboardText(BuildVersion.FullVersionString);
	}
}
