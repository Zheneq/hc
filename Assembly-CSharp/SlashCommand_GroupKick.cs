using System;
using LobbyGameClientMessages;

public class SlashCommand_GroupKick : SlashCommand
{
	public SlashCommand_GroupKick() : base("/kick", SlashCommandType.Everywhere)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (arguments.IsNullOrEmpty() || ClientGameManager.Get() == null)
		{
			TextConsole.Get().Write(new TextConsole.Message
			{
				Text = StringUtil.TR("KickNameError", "SlashCommand"),
				MessageType = ConsoleMessageType.SystemMessage
			}, null);
			return;
		}
		ClientGameManager.Get().KickFromGroup(arguments, delegate(GroupKickResponse r)
		{
			if (!r.Success)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SlashCommand_GroupKick.<OnSlashCommand>m__0(GroupKickResponse)).MethodHandle;
				}
				if (r.LocalizedFailure != null)
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
		});
	}
}
