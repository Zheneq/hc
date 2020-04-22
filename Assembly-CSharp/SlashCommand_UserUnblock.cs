using LobbyGameClientMessages;

public class SlashCommand_UserUnblock : SlashCommand
{
	public SlashCommand_UserUnblock()
		: base("/unblock", SlashCommandType.Everywhere)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (!arguments.IsNullOrEmpty())
		{
			while (true)
			{
				switch (7)
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
				ClientGameManager.Get().UpdateFriend(arguments, 0L, FriendOperation.Unblock, string.Empty, delegate(FriendUpdateResponse r)
				{
					string empty = string.Empty;
					if (r.Success)
					{
						while (true)
						{
							switch (4)
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
						empty = string.Format(StringUtil.TR("SuccessfullyUnblocked", "SlashCommand"), arguments);
					}
					else
					{
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
						else if (r.ErrorMessage.IsNullOrEmpty())
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
			Text = StringUtil.TR("UnblockNameError", "SlashCommand"),
			MessageType = ConsoleMessageType.SystemMessage
		});
	}
}
