using System.Collections.Generic;

public class SlashCommand_Apropos : SlashCommand
{
    public SlashCommand_Apropos() : base("/apropos", SlashCommandType.Everywhere)
    {
    }

    private void DumpCommand(
        string arguments,
        string command,
        List<string> aliases,
        bool bAvailableBecauseWereInFrontEnd,
        bool bAvailableBecauseWereInGame)
    {
        bool isMatch = arguments.IsNullOrEmpty() || command.Contains(arguments);
        if (!isMatch && !aliases.IsNullOrEmpty())
        {
            foreach (string alias in aliases)
            {
                if (alias.Contains(arguments))
                {
                    isMatch = true;
                    break;
                }
            }
        }

        if (!isMatch)
        {
            return;
        }

        if (!bAvailableBecauseWereInFrontEnd && !bAvailableBecauseWereInGame)
        {
            return;
        }

        TextConsole.Message message = new TextConsole.Message
        {
            MessageType = ConsoleMessageType.SystemMessage,
            Text = command
        };
        if (!aliases.IsNullOrEmpty())
        {
            foreach (string alias in aliases)
            {
                message.Text = message.Text + ", " + alias;
            }
        }

        TextConsole.Get().Write(message);
    }

    public override void OnSlashCommand(string arguments)
    {
        ClientGameManager clientGameManager = ClientGameManager.Get();
        if (clientGameManager == null)
        {
            return;
        }

        bool isNotInGame = GameFlowData.Get() == null;
        foreach (SlashCommand slashCommand in SlashCommands.Get().m_slashCommands)
        {
            if (!slashCommand.PublicFacing && !clientGameManager.HasDeveloperAccess())
            {
                continue;
            }

            bool bAvailableBecauseWereInFrontEnd = isNotInGame && slashCommand.AvailableInFrontEnd;
            bool bAvailableBecauseWereInGame = !isNotInGame && slashCommand.AvailableInGame;
            DumpCommand(
                arguments,
                slashCommand.Command,
                slashCommand.Aliases,
                bAvailableBecauseWereInFrontEnd,
                bAvailableBecauseWereInGame);
        }
    }
}