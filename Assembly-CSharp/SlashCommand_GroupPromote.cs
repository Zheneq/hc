using LobbyGameClientMessages;

public class SlashCommand_GroupPromote : SlashCommand
{
	public SlashCommand_GroupPromote()
		: base("/promote", SlashCommandType.Everywhere)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (!arguments.IsNullOrEmpty())
		{
			if (!(ClientGameManager.Get() == null))
			{
				ClientGameManager.Get().PromoteWithinGroup(arguments, delegate(GroupPromoteResponse r)
				{
					if (!r.Success)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
							{
								if (r.LocalizedFailure != null)
								{
									r.ErrorMessage = r.LocalizedFailure.ToString();
								}
								string text = string.Format(StringUtil.TR("FailedMessage", "Global"), (!r.ErrorMessage.IsNullOrEmpty()) ? r.ErrorMessage : StringUtil.TR("UnknownError", "Global"));
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
				return;
			}
		}
		TextConsole.Get().Write(new TextConsole.Message
		{
			Text = StringUtil.TR("PromoteNameError", "SlashCommand"),
			MessageType = ConsoleMessageType.SystemMessage
		});
	}
}
