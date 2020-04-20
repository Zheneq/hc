using System;
using System.Collections.Generic;
using LobbyGameClientMessages;

public class SlashCommand_SpectateGame : SlashCommand
{
	public SlashCommand_SpectateGame() : base("/spectategame", SlashCommandType.InFrontEnd)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (!GameManager.Get().GameplayOverrides.AllowSpectatorsOutsideCustom)
		{
			TextConsole.Get().Write(StringUtil.TR("FriendGameSpectatingNotAvailable", "Frontend"), ConsoleMessageType.SystemMessage);
			return;
		}
		if (SlashCommand_SpectateGame.f__am_cache0 == null)
		{
			SlashCommand_SpectateGame.f__am_cache0 = delegate(GameSpectatorResponse response)
			{
				TextConsole.Message message = default(TextConsole.Message);
				message.MessageType = ConsoleMessageType.SystemMessage;
				if (!response.Success)
				{
					if (response.LocalizedFailure != null)
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
				}
			};
		}
		Action<GameSpectatorResponse> onResponseCallback = SlashCommand_SpectateGame.f__am_cache0;
		if (!arguments.IsNullOrEmpty())
		{
			if (!(ClientGameManager.Get() == null))
			{
				ClientGameManager.Get().SpectateGame(arguments, onResponseCallback);
				return;
			}
		}
		FriendList friendList = ClientGameManager.Get().FriendList;
		using (Dictionary<long, FriendInfo>.Enumerator enumerator = friendList.Friends.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<long, FriendInfo> keyValuePair = enumerator.Current;
				if (keyValuePair.Value.IsJoinable(GameManager.Get().GameplayOverrides))
				{
					ClientGameManager.Get().SpectateGame(keyValuePair.Value.FriendHandle, onResponseCallback);
					goto IL_11C;
				}
			}
		}
		IL_11C:
		TextConsole.Get().Write(StringUtil.TR("NoFriendsIngame", "Frontend"), ConsoleMessageType.SystemMessage);
	}
}
