using System;
using LobbyGameClientMessages;

public class SlashCommand_GroupChat : SlashCommand
{
	public SlashCommand_GroupChat() : base("/group", SlashCommandType.Everywhere)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SlashCommand_GroupChat.OnSlashCommand(string)).MethodHandle;
			}
			if (!(ClientGameManager.Get() == null))
			{
				ClientGameManager clientGameManager = ClientGameManager.Get();
				if (SlashCommand_GroupChat.<>f__am$cache0 == null)
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
					SlashCommand_GroupChat.<>f__am$cache0 = delegate(GroupChatResponse r)
					{
						if (!r.Success)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!true)
							{
								RuntimeMethodHandle runtimeMethodHandle2 = methodof(SlashCommand_GroupChat.<OnSlashCommand>m__0(GroupChatResponse)).MethodHandle;
							}
							if (r.LocalizedFailure != null)
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
								r.ErrorMessage = r.LocalizedFailure.ToString();
							}
							string format = StringUtil.TR("FailedMessage", "Global");
							object arg;
							if (r.ErrorMessage.IsNullOrEmpty())
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
								arg = StringUtil.TR("UnknownError", "Global");
							}
							else
							{
								arg = r.ErrorMessage;
							}
							string text = string.Format(format, arg);
							TextConsole.Get().Write(new TextConsole.Message
							{
								Text = text,
								MessageType = ConsoleMessageType.SystemMessage
							}, null);
						}
					};
				}
				clientGameManager.ChatToGroup(arguments, SlashCommand_GroupChat.<>f__am$cache0);
				return;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		TextConsole.Get().Write(new TextConsole.Message
		{
			Text = "Error: name who you wish to invite",
			MessageType = ConsoleMessageType.SystemMessage
		}, null);
	}
}
