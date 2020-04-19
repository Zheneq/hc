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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SlashCommand_ChatWhisper.OnSlashCommand(string)).MethodHandle;
			}
			if (ClientGameManager.Get() == null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			else
			{
				string[] array = arguments.Split(null, 2, StringSplitOptions.RemoveEmptyEntries);
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
