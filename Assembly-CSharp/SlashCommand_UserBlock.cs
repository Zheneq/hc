using LobbyGameClientMessages;

public class SlashCommand_UserBlock : SlashCommand
{
	public SlashCommand_UserBlock()
		: base("/block", SlashCommandType.Everywhere)
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
				ClientGameManager.Get().UpdateFriend(arguments, 0L, FriendOperation.Block, string.Empty, delegate(FriendUpdateResponse r)
				{
					string empty = string.Empty;
					if (r.Success)
					{
						empty = string.Format(StringUtil.TR("SuccessfullyBlocked", "SlashCommand"), arguments);
					}
					else
					{
						if (r.LocalizedFailure != null)
						{
							r.ErrorMessage = r.LocalizedFailure.ToString();
						}
						else if (r.ErrorMessage.IsNullOrEmpty())
						{
							while (true)
							{
								switch (1)
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
							r.ErrorMessage = StringUtil.TR("UnknownError", "Global");
						}
						empty = string.Format(StringUtil.TR("FailedMessage", "Global"), r.ErrorMessage);
					}
					TextConsole.Get().Write(new TextConsole.Message
					{
						Text = empty,
						MessageType = ConsoleMessageType.SystemMessage
					});
				});
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		TextConsole.Get().Write(new TextConsole.Message
		{
			Text = StringUtil.TR("BlockNameError", "SlashCommand"),
			MessageType = ConsoleMessageType.SystemMessage
		});
	}
}
