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
			if (!(ClientGameManager.Get() == null))
			{
				ClientGameManager clientGameManager = ClientGameManager.Get();
				
				clientGameManager.ChatToGroup(arguments, delegate(GroupChatResponse r)
					{
						if (!r.Success)
						{
							if (r.LocalizedFailure != null)
							{
								r.ErrorMessage = r.LocalizedFailure.ToString();
							}
							string format = StringUtil.TR("FailedMessage", "Global");
							object arg;
							if (r.ErrorMessage.IsNullOrEmpty())
							{
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
				return;
			}
		}
		TextConsole.Get().Write(new TextConsole.Message
		{
			Text = "Error: name who you wish to invite",
			MessageType = ConsoleMessageType.SystemMessage
		}, null);
	}
}
