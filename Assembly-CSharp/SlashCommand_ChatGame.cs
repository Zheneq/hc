using System;

public class SlashCommand_ChatGame : SlashCommand
{
	public SlashCommand_ChatGame() : base("/game", SlashCommandType.Everywhere)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (!arguments.IsNullOrEmpty())
		{
			if (!(ClientGameManager.Get() == null))
			{
				ClientGameManager.Get().SendChatNotification(null, ConsoleMessageType.GameChat, arguments);
				return;
			}
		}
	}
}
