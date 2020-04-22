using LobbyGameClientMessages;
using System;
using System.Collections.Generic;

public class SlashCommand_InviteToGame : SlashCommand
{
	public SlashCommand_InviteToGame()
		: base("/invitetogame", SlashCommandType.InFrontEnd)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (_003C_003Ef__am_0024cache0 == null)
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
			_003C_003Ef__am_0024cache0 = delegate(GameInvitationResponse response)
			{
				TextConsole.Message message = default(TextConsole.Message);
				message.MessageType = ConsoleMessageType.SystemMessage;
				if (response.Success)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					message.Text = string.Format(StringUtil.TR("InviteSentTo", "SlashCommand"), response.InviteeHandle);
				}
				else if (response.LocalizedFailure != null)
				{
					message.Text = response.LocalizedFailure.ToString();
				}
				else if (!response.ErrorMessage.IsNullOrEmpty())
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
					message.Text = $"Failed: {response.ErrorMessage}#NeedsLocalization";
				}
				else
				{
					message.Text = StringUtil.TR("UnknownErrorTryAgain", "Frontend");
				}
				TextConsole.Get().Write(message);
			};
		}
		Action<GameInvitationResponse> onResponseCallback = _003C_003Ef__am_0024cache0;
		if (!arguments.IsNullOrEmpty())
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
			if (!(ClientGameManager.Get() == null))
			{
				ClientGameManager.Get().InvitePlayerToGame(arguments, onResponseCallback);
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		FriendList friendList = ClientGameManager.Get().FriendList;
		foreach (KeyValuePair<long, FriendInfo> friend in friendList.Friends)
		{
			if (friend.Value.IsOnline)
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
				ClientGameManager.Get().InvitePlayerToGame(friend.Value.FriendHandle, onResponseCallback);
			}
		}
	}
}
