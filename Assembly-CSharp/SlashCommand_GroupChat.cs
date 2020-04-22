using LobbyGameClientMessages;

public class SlashCommand_GroupChat : SlashCommand
{
	public SlashCommand_GroupChat()
		: base("/group", SlashCommandType.Everywhere)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (!arguments.IsNullOrEmpty())
		{
			if (!(ClientGameManager.Get() == null))
			{
				ClientGameManager clientGameManager = ClientGameManager.Get();
				if (_003C_003Ef__am_0024cache0 == null)
				{
					_003C_003Ef__am_0024cache0 = delegate(GroupChatResponse r)
					{
						if (!r.Success)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									break;
								default:
								{
									if (r.LocalizedFailure != null)
									{
										r.ErrorMessage = r.LocalizedFailure.ToString();
									}
									string format = StringUtil.TR("FailedMessage", "Global");
									string arg;
									if (r.ErrorMessage.IsNullOrEmpty())
									{
										arg = StringUtil.TR("UnknownError", "Global");
									}
									else
									{
										arg = r.ErrorMessage;
									}
									string text = string.Format(format, arg);
									TextConsole.Get().Write(new TextConsole.Message
									{
										Text = text,
										MessageType = ConsoleMessageType.SystemMessage
									});
									return;
								}
								}
							}
						}
					};
				}
				clientGameManager.ChatToGroup(arguments, _003C_003Ef__am_0024cache0);
				return;
			}
		}
		TextConsole.Get().Write(new TextConsole.Message
		{
			Text = "Error: name who you wish to invite",
			MessageType = ConsoleMessageType.SystemMessage
		});
	}
}
