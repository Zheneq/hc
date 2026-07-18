using LobbyGameClientMessages;

public class SlashCommand_GroupKick : SlashCommand
{
    public SlashCommand_GroupKick()
        : base("/kick", SlashCommandType.Everywhere)
    {
    }

    public override void OnSlashCommand(string arguments)
    {
        if (arguments.IsNullOrEmpty() || ClientGameManager.Get() == null)
        {
            TextConsole.Get().Write(
                new TextConsole.Message
                {
                    Text = StringUtil.TR("KickNameError", "SlashCommand"),
                    MessageType = ConsoleMessageType.SystemMessage
                });
            return;
        }

        ClientGameManager.Get().KickFromGroup(
            arguments,
            delegate(GroupKickResponse r)
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