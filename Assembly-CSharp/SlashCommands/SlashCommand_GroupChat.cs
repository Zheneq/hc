using LobbyGameClientMessages;

public class SlashCommand_GroupChat : SlashCommand
{
    public SlashCommand_GroupChat()
        : base("/group", SlashCommandType.Everywhere)
    {
    }

    public override void OnSlashCommand(string arguments)
    {
        if (arguments.IsNullOrEmpty() || ClientGameManager.Get() == null)
        {
            TextConsole.Get().Write(
                new TextConsole.Message
                {
                    Text = "Error: name who you wish to invite", // TODO CLIENT UI wrong (and not localized) error message
                    MessageType = ConsoleMessageType.SystemMessage
                });
        }
        else
        {
            ClientGameManager clientGameManager = ClientGameManager.Get();

            clientGameManager.ChatToGroup(
                arguments,
                delegate(GroupChatResponse r)
                {
                    if (r.Success)
                    {
                        return;
                    }

                    if (r.LocalizedFailure != null)
                    {
                        r.ErrorMessage = r.LocalizedFailure.ToString();
                    }

                    string text = string.Format(
                        StringUtil.TR("FailedMessage", "Global"),
                        r.ErrorMessage.IsNullOrEmpty()
                            ? StringUtil.TR("UnknownError", "Global")
                            : r.ErrorMessage);
                    TextConsole.Get().Write(
                        new TextConsole.Message
                        {
                            Text = text,
                            MessageType = ConsoleMessageType.SystemMessage
                        });
                });
        }
    }
}