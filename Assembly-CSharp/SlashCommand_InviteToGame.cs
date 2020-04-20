using System;
using System.Collections.Generic;
using LobbyGameClientMessages;

public class SlashCommand_InviteToGame : SlashCommand
{
	public SlashCommand_InviteToGame() : base("/invitetogame", SlashCommandType.InFrontEnd)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (SlashCommand_InviteToGame.f__am_cache0 == null)
		{
			SlashCommand_InviteToGame.f__am_cache0 = delegate(GameInvitationResponse response)
			{
				TextConsole.Message message = default(TextConsole.Message);
				message.MessageType = ConsoleMessageType.SystemMessage;
				if (response.Success)
				{
					message.Text = string.Format(StringUtil.TR("InviteSentTo", "SlashCommand"), response.InviteeHandle);
				}
				else if (response.LocalizedFailure != null)
				{
					message.Text = response.LocalizedFailure.ToString();
				}
				else if (!response.ErrorMessage.IsNullOrEmpty())
				{
					message.Text = string.Format("Failed: {0}#NeedsLocalization", response.ErrorMessage);
				}
				else
				{
					message.Text = StringUtil.TR("UnknownErrorTryAgain", "Frontend");
				}
				TextConsole.Get().Write(message, null);
			};
		}
		Action<GameInvitationResponse> onResponseCallback = SlashCommand_InviteToGame.f__am_cache0;
		if (!arguments.IsNullOrEmpty())
		{
			if (!(ClientGameManager.Get() == null))
			{
				ClientGameManager.Get().InvitePlayerToGame(arguments, onResponseCallback);
				return;
			}
		}
		FriendList friendList = ClientGameManager.Get().FriendList;
		foreach (KeyValuePair<long, FriendInfo> keyValuePair in friendList.Friends)
		{
			if (keyValuePair.Value.IsOnline)
			{
				ClientGameManager.Get().InvitePlayerToGame(keyValuePair.Value.FriendHandle, onResponseCallback);
			}
		}
	}
}
