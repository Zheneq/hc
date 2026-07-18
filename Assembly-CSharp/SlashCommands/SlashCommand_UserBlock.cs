using LobbyGameClientMessages;

public class SlashCommand_UserBlock : SlashCommand
{
    public SlashCommand_UserBlock()
        : base("/block", SlashCommandType.Everywhere)
    {
    }

    public override void OnSlashCommand(string arguments)
    {
        if (arguments.IsNullOrEmpty() || ClientGameManager.Get() == null)
        {
            TextConsole.Get().Write(
                new TextConsole.Message
                {
                    Text = StringUtil.TR("BlockNameError", "SlashCommand"),
                    MessageType = ConsoleMessageType.SystemMessage
                });
            return;
        }

        ClientGameManager.Get().UpdateFriend(
            arguments,
            0L,
            FriendOperation.Block,
            string.Empty,
            delegate(FriendUpdateResponse r)
            {
                string text;
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
                        r.ErrorMessage = StringUtil.TR("UnknownError", "Global");
                    }

                    text = string.Format(StringUtil.TR("FailedMessage", "Global"), r.ErrorMessage);
                }

                TextConsole.Get().Write(
                    new TextConsole.Message
                    {
                        Text = text,
                        MessageType = ConsoleMessageType.SystemMessage
                    });
            });
    }
}