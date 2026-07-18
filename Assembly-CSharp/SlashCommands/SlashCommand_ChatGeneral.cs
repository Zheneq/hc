public class SlashCommand_ChatGeneral : SlashCommand
{
    public SlashCommand_ChatGeneral()
        : base("/general", SlashCommandType.InFrontEnd)
    {
    }

    public override void OnSlashCommand(string arguments)
    {
        if (arguments.IsNullOrEmpty() || ClientGameManager.Get() == null)
        {
            return;
        }

        ClientGameManager.Get().SendChatNotification(null, ConsoleMessageType.GlobalChat, arguments);
    }
}