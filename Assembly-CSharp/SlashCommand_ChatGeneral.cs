using System;

public class SlashCommand_ChatGeneral : SlashCommand
{
	public SlashCommand_ChatGeneral() : base("/general", SlashCommandType.InFrontEnd)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (!arguments.IsNullOrEmpty())
		{
			if (!(ClientGameManager.Get() == null))
			{
				ClientGameManager.Get().SendChatNotification(null, ConsoleMessageType.GlobalChat, arguments);
				return;
			}
		}
	}
}
