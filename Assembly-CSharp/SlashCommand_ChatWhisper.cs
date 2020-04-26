using System;

public class SlashCommand_ChatWhisper : SlashCommand
{
	public SlashCommand_ChatWhisper()
		: base("/whisper", SlashCommandType.Everywhere)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (arguments.IsNullOrEmpty())
		{
			return;
		}
		while (true)
		{
			if (ClientGameManager.Get() == null)
			{
				while (true)
				{
					switch (6)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			string[] array = arguments.Split((string[])null, 2, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length >= 2)
			{
				string recipientHandle = array[0];
				string text = array[1];
				ClientGameManager.Get().SendChatNotification(recipientHandle, ConsoleMessageType.WhisperChat, text);
			}
			return;
		}
	}
}
