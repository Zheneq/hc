﻿using System;
using LobbyGameClientMessages;

public class SlashCommand_UserUnblock : SlashCommand
{
	public SlashCommand_UserUnblock() : base("/unblock", SlashCommandType.Everywhere)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (!arguments.IsNullOrEmpty())
		{
			if (!(ClientGameManager.Get() == null))
			{
				ClientGameManager.Get().UpdateFriend(arguments, 0L, FriendOperation.Unblock, string.Empty, delegate(FriendUpdateResponse r)
				{
					string text = string.Empty;
					if (r.Success)
					{
						text = string.Format(StringUtil.TR("SuccessfullyUnblocked", "SlashCommand"), arguments);
					}
					else
					{
						if (r.LocalizedFailure != null)
						{
							r.ErrorMessage = r.LocalizedFailure.ToString();
						}
						else if (r.ErrorMessage.IsNullOrEmpty())
						{
							r.ErrorMessage = StringUtil.TR("UnknownError", "Global");
						}
						text = string.Format(StringUtil.TR("FailedMessage", "Global"), r.ErrorMessage);
					}
					TextConsole.Get().Write(new TextConsole.Message
					{
						Text = text,
						MessageType = ConsoleMessageType.SystemMessage
					}, null);
				});
				return;
			}
		}
		TextConsole.Get().Write(new TextConsole.Message
		{
			Text = StringUtil.TR("UnblockNameError", "SlashCommand"),
			MessageType = ConsoleMessageType.SystemMessage
		}, null);
	}
}
