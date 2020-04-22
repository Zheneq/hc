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
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(ClientGameManager.Get() == null))
			{
				ClientGameManager clientGameManager = ClientGameManager.Get();
				if (_003C_003Ef__am_0024cache0 == null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
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
									if (1 == 0)
									{
										/*OpCode not supported: LdMemberToken*/;
									}
									if (r.LocalizedFailure != null)
									{
										while (true)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
										r.ErrorMessage = r.LocalizedFailure.ToString();
									}
									string format = StringUtil.TR("FailedMessage", "Global");
									string arg;
									if (r.ErrorMessage.IsNullOrEmpty())
									{
										while (true)
										{
											switch (6)
											{
											case 0:
												continue;
											}
											break;
										}
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
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		TextConsole.Get().Write(new TextConsole.Message
		{
			Text = "Error: name who you wish to invite",
			MessageType = ConsoleMessageType.SystemMessage
		});
	}
}
