using LobbyGameClientMessages;

public class SlashCommand_GroupKick : SlashCommand
{
	public SlashCommand_GroupKick()
		: base("/kick", SlashCommandType.Everywhere)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (arguments.IsNullOrEmpty() || ClientGameManager.Get() == null)
		{
			TextConsole.Get().Write(new TextConsole.Message
			{
				Text = StringUtil.TR("KickNameError", "SlashCommand"),
				MessageType = ConsoleMessageType.SystemMessage
			});
		}
		else
		{
			ClientGameManager.Get().KickFromGroup(arguments, delegate(GroupKickResponse r)
			{
				if (!r.Success)
				{
					while (true)
					{
						switch (3)
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
									switch (6)
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
			});
		}
	}
}
