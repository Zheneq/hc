using LobbyGameClientMessages;
using System;
using System.Collections.Generic;

public class SlashCommand_SpectateGame : SlashCommand
{
	public SlashCommand_SpectateGame()
		: base("/spectategame", SlashCommandType.InFrontEnd)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (!GameManager.Get().GameplayOverrides.AllowSpectatorsOutsideCustom)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					TextConsole.Get().Write(StringUtil.TR("FriendGameSpectatingNotAvailable", "Frontend"));
					return;
				}
			}
		}
		if (_003C_003Ef__am_0024cache0 == null)
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
			_003C_003Ef__am_0024cache0 = delegate(GameSpectatorResponse response)
			{
				TextConsole.Message message = default(TextConsole.Message);
				message.MessageType = ConsoleMessageType.SystemMessage;
				if (!response.Success)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							if (response.LocalizedFailure != null)
							{
								message.Text = response.LocalizedFailure.ToString();
							}
							else if (!response.ErrorMessage.IsNullOrEmpty())
							{
								message.Text = $"Failed: {response.ErrorMessage}#NeedsLocalization";
							}
							else
							{
								message.Text = StringUtil.TR("UnknownErrorTryAgain", "Frontend");
							}
							TextConsole.Get().Write(message);
							return;
						}
					}
				}
			};
		}
		Action<GameSpectatorResponse> onResponseCallback = _003C_003Ef__am_0024cache0;
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
				ClientGameManager.Get().SpectateGame(arguments, onResponseCallback);
				return;
			}
		}
		FriendList friendList = ClientGameManager.Get().FriendList;
		using (Dictionary<long, FriendInfo>.Enumerator enumerator = friendList.Friends.GetEnumerator())
		{
			while (true)
			{
				if (!enumerator.MoveNext())
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
					break;
				}
				KeyValuePair<long, FriendInfo> current = enumerator.Current;
				if (current.Value.IsJoinable(GameManager.Get().GameplayOverrides))
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							ClientGameManager.Get().SpectateGame(current.Value.FriendHandle, onResponseCallback);
							goto end_IL_00aa;
						}
					}
				}
			}
			end_IL_00aa:;
		}
		TextConsole.Get().Write(StringUtil.TR("NoFriendsIngame", "Frontend"));
	}
}
