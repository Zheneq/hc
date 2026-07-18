using LobbyGameClientMessages;

public class SlashCommand_GroupInvite : SlashCommand
{
    public SlashCommand_GroupInvite()
        : base("/invite", SlashCommandType.Everywhere)
    {
    }

    public override void OnSlashCommand(string arguments)
    {
        if (arguments.IsNullOrEmpty() || ClientGameManager.Get() == null)
        {
            TextConsole.Get().Write(
                new TextConsole.Message
                {
                    Text = StringUtil.TR("InviteNameError", "SlashCommand"),
                    MessageType = ConsoleMessageType.SystemMessage
                });
            return;
        }

        TextConsole.Get().Write(
            new TextConsole.Message
            {
                Text = string.Format(StringUtil.TR("InvitedFriendToGroup", "Global"), arguments),
                MessageType = ConsoleMessageType.SystemMessage
            });

        ClientGameManager.Get().InviteToGroup(
            arguments,
            delegate(GroupInviteResponse r)
            {
                if (r.Success)
                {
                    return;
                }

                string text;
                if (r.LocalizedFailure != null)
                {
                    if ((ClientGameManager.Get().GroupInfo == null || !ClientGameManager.Get().GroupInfo.InAGroup)
                        && r.LocalizedFailure.Context == "Invite"
                        && r.LocalizedFailure.Term == "OtherPlayerInOtherGroup")
                    {
                        UIDialogPopupManager.OpenTwoButtonDialog(
                            string.Empty,
                            string.Format(
                                StringUtil.TR("OtherPlayerInOtherGroupSendJoinRequest", "Invite"),
                                arguments),
                            StringUtil.TR("Yes", "Global"),
                            StringUtil.TR("No", "Global"),
                            delegate
                            {
                                ClientGameManager.Get().RequestToJoinGroup(
                                    arguments,
                                    delegate(GroupJoinResponse response)
                                    {
                                        if (response.Success)
                                        {
                                            return;
                                        }

                                        TextConsole.Get().Write(
                                            new TextConsole.Message
                                            {
                                                Text = response.LocalizedFailure != null
                                                    ? response.LocalizedFailure.ToString()
                                                    : !response.ErrorMessage.IsNullOrEmpty()
                                                        ? $"Failed: {response.ErrorMessage}#NeedsLocalization"
                                                        : StringUtil.TR("UnknownErrorTryAgain", "Frontend"),
                                                MessageType = ConsoleMessageType.SystemMessage
                                            });
                                    });
                            },
                            delegate { });
                        return;
                    }

                    text = r.LocalizedFailure.ToString();
                }
                else
                {
                    text = r.ErrorMessage.IsNullOrEmpty()
                        ? StringUtil.TR("UnknownErrorTryAgain", "Frontend")
                        : $"Failed: {r.ErrorMessage}#NeedsLocalization";
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