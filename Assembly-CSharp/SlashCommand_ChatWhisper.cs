using System;

public class SlashCommand_ChatWhisper : SlashCommand
{
	public SlashCommand_ChatWhisper() : base("/whisper", SlashCommandType.Everywhere)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (!arguments.IsNullOrEmpty())
		{
			if (ClientGameManager.Get() == null)
			{
			}
			else
			{
				string[] array = arguments.Split((string[])null, 2, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length < 2)
				{
					return;
				}
				string recipientHandle = array[0];
				string text = array[1];
				ClientGameManager.Get().SendChatNotification(recipientHandle, ConsoleMessageType.WhisperChat, text);
				return;
			}
		}
	}
}
