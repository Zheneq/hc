public class SlashCommand_ChatGame : SlashCommand
{
	public SlashCommand_ChatGame()
		: base("/game", SlashCommandType.Everywhere)
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
					switch (3)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			ClientGameManager.Get().SendChatNotification(null, ConsoleMessageType.GameChat, arguments);
			return;
		}
	}
}
