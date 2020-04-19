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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SlashCommand_SpectateGame.OnSlashCommand(string)).MethodHandle;
			}
			TextConsole.Get().Write(StringUtil.TR("FriendGameSpectatingNotAvailable", "Frontend"), ConsoleMessageType.SystemMessage);
			return;
		}
		if (SlashCommand_SpectateGame.<>f__am$cache0 == null)
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
			SlashCommand_SpectateGame.<>f__am$cache0 = delegate(GameSpectatorResponse response)
			{
				TextConsole.Message message = default(TextConsole.Message);
				message.MessageType = ConsoleMessageType.SystemMessage;
				if (!response.Success)
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle2 = methodof(SlashCommand_SpectateGame.<OnSlashCommand>m__0(GameSpectatorResponse)).MethodHandle;
					}
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
		Action<GameSpectatorResponse> onResponseCallback = SlashCommand_SpectateGame.<>f__am$cache0;
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
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					ClientGameManager.Get().SpectateGame(keyValuePair.Value.FriendHandle, onResponseCallback);
					goto IL_11C;
				}
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
		IL_11C:
		TextConsole.Get().Write(StringUtil.TR("NoFriendsIngame", "Frontend"), ConsoleMessageType.SystemMessage);
	}
}
