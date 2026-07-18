using LobbyGameClientMessages;

public class SlashCommand_SetDevChatTag : SlashCommand
{
    public SlashCommand_SetDevChatTag()
        : base("/devtag", SlashCommandType.Everywhere)
    {
        PublicFacing = false;
    }

    public override void OnSlashCommand(string arguments)
    {
        if (arguments.IsNullOrEmpty() || ClientGameManager.Get() == null)
        {
            return;
        }

        if (arguments.EqualsIgnoreCase(StringUtil.TR("on", "SlashCommand")))
        {
            ClientGameManager.Get().SendSetDevTagRequest(
                true,
                delegate(SetDevTagResponse response) { TextConsole.Get().HandleSetDevTagResponse(response); });
        }

        else if (arguments.EqualsIgnoreCase(StringUtil.TR("off", "SlashCommand")))
        {
            ClientGameManager.Get().SendSetDevTagRequest(
                false,
                delegate(SetDevTagResponse response) { TextConsole.Get().HandleSetDevTagResponse(response); });
        }
    }
}