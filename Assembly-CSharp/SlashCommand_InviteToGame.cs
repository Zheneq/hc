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
		if (SlashCommand_InviteToGame.<>f__am$cache0 == null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SlashCommand_InviteToGame.OnSlashCommand(string)).MethodHandle;
			}
			SlashCommand_InviteToGame.<>f__am$cache0 = delegate(GameInvitationResponse response)
			{
				TextConsole.Message message = default(TextConsole.Message);
				message.MessageType = ConsoleMessageType.SystemMessage;
				if (response.Success)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle2 = methodof(SlashCommand_InviteToGame.<OnSlashCommand>m__0(GameInvitationResponse)).MethodHandle;
					}
					message.Text = string.Format(StringUtil.TR("InviteSentTo", "SlashCommand"), response.InviteeHandle);
				}
				else if (response.LocalizedFailure != null)
				{
					message.Text = response.LocalizedFailure.ToString();
				}
				else if (!response.ErrorMessage.IsNullOrEmpty())
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					message.Text = string.Format("Failed: {0}#NeedsLocalization", response.ErrorMessage);
				}
				else
				{
					message.Text = StringUtil.TR("UnknownErrorTryAgain", "Frontend");
				}
				TextConsole.Get().Write(message, null);
			};
		}
		Action<GameInvitationResponse> onResponseCallback = SlashCommand_InviteToGame.<>f__am$cache0;
		if (!arguments.IsNullOrEmpty())
		{
			for (;;)
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
			for (;;)
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
		foreach (KeyValuePair<long, FriendInfo> keyValuePair in friendList.Friends)
		{
			if (keyValuePair.Value.IsOnline)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				ClientGameManager.Get().InvitePlayerToGame(keyValuePair.Value.FriendHandle, onResponseCallback);
			}
		}
	}
}
