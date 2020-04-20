using System;

public class SlashCommand_Version : SlashCommand
{
	public SlashCommand_Version() : base("/version", SlashCommandType.Everywhere)
	{
		base.PublicFacing = true;
	}

	public override void OnSlashCommand(string arguments)
	{
		string text = string.Format("Current Version: {0}", BuildVersion.FullVersionString);
		TextConsole.Get().Write(text, ConsoleMessageType.SystemMessage);
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
