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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		ClientGameManager.Get().SendChatNotification(null, ConsoleMessageType.GlobalChat, arguments);
	}
}
