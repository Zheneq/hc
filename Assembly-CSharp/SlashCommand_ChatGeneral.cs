public class SlashCommand_ChatGeneral : SlashCommand
{
	public SlashCommand_ChatGeneral()
		: base("/general", SlashCommandType.InFrontEnd)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (arguments.IsNullOrEmpty())
		{
			return;
		}
		if (ClientGameManager.Get() == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		ClientGameManager.Get().SendChatNotification(null, ConsoleMessageType.GlobalChat, arguments);
	}
}
