using LobbyGameClientMessages;

public class SlashCommand_GroupLeave : SlashCommand
{
    public SlashCommand_GroupLeave()
        : base("/leave", SlashCommandType.Everywhere)
    {
    }

    public override void OnSlashCommand(string arguments)
    {
        ClientGameManager.Get().LeaveGroup(
            delegate(GroupLeaveResponse r)
            {
                if (!r.Success)
                {
                    string errorMsg = r.LocalizedFailure != null
                        ? r.LocalizedFailure.ToString()
                        : r.ErrorMessage != null
                            ? $"{r.ErrorMessage}#needsLocalization"
                            : StringUtil.TR("UnknownError", "Global");
                    TextConsole.Get().Write(
                        new TextConsole.Message
                        {
                            Text = string.Format(StringUtil.TR("FailedMessage", "Global"), errorMsg),
                            MessageType = ConsoleMessageType.SystemMessage
                        });
                    return;
                }

                ClientGameManager clientGameManager = ClientGameManager.Get();
                if (clientGameManager != null && clientGameManager.GroupInfo != null)
                {
                    ClientGameManager.Get().GroupInfo.InAGroup = false;
                    ClientGameManager.Get().GroupInfo.IsLeader = false;
                    ClientGameManager.Get().GroupInfo.Members.Clear();
                }

                if (UICharacterSelectScreen.Get() != null)
                {
                    UICharacterSelectScreenController.Get().NotifyGroupUpdate();
                }
            });
    }
}