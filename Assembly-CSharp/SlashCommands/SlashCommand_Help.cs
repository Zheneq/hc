public class SlashCommand_Help : SlashCommand
{
    public SlashCommand_Help()
        : base("/help", SlashCommandType.Everywhere)
    {
    }

    public override void OnSlashCommand(string arguments)
    {
        foreach (SlashCommand slashCommand in SlashCommands.Get().m_slashCommands)
        {
            if (slashCommand.PublicFacing) // TODO CLIENT UI || clientGameManager.HasDeveloperAccess() ?
            {
                TextConsole.Get().Write(
                    new TextConsole.Message
                    {
                        Text = slashCommand.Description,
                        MessageType = ConsoleMessageType.SystemMessage
                    });
            }
        }
    }
}