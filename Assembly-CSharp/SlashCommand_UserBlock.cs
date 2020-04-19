using System;
using LobbyGameClientMessages;

public class SlashCommand_UserBlock : SlashCommand
{
	public SlashCommand_UserBlock() : base("/block", SlashCommandType.Everywhere)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (!arguments.IsNullOrEmpty())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SlashCommand_UserBlock.OnSlashCommand(string)).MethodHandle;
			}
			if (!(ClientGameManager.Get() == null))
			{
				ClientGameManager.Get().UpdateFriend(arguments, 0L, FriendOperation.Block, string.Empty, delegate(FriendUpdateResponse r)
				{
					string text = string.Empty;
					if (r.Success)
					{
						text = string.Format(StringUtil.TR("SuccessfullyBlocked", "SlashCommand"), arguments);
					}
					else
					{
						if (r.LocalizedFailure != null)
						{
							r.ErrorMessage = r.LocalizedFailure.ToString();
						}
						else if (r.ErrorMessage.IsNullOrEmpty())
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
								RuntimeMethodHandle runtimeMethodHandle2 = methodof(SlashCommand_UserBlock.<OnSlashCommand>c__AnonStorey0.<>m__0(FriendUpdateResponse)).MethodHandle;
							}
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
			for (;;)
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
		}, null);
	}
}
