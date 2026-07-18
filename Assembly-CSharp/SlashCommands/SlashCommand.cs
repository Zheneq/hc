using I2.Loc;
using System.Collections.Generic;

public abstract class SlashCommand
{
    private string internalCommand;
    public List<string> Aliases;

    public string Command { get; private set; }
    public SlashCommandType Type { get; private set; }
    public string Description { get; private set; }
    public bool PublicFacing { get; set; }
    public bool AvailableInFrontEnd => (Type & SlashCommandType.InFrontEnd) != 0;
    public bool AvailableInGame => (Type & SlashCommandType.InGame) != 0;

    public SlashCommand(string command, SlashCommandType type)
    {
        internalCommand = Command = command;
        Type = type;
        PublicFacing = true;
        Aliases = new List<string>();
    }

    public abstract void OnSlashCommand(string arguments);

    public bool IsSlashCommand(string command)
    {
        if (command.EqualsIgnoreCase(Command) || command.EqualsIgnoreCase(internalCommand))
        {
            return true;
        }

        if (Aliases.IsNullOrEmpty())
        {
            return false;
        }

        foreach (string alias in Aliases)
        {
            if (command.EqualsIgnoreCase(alias))
            {
                return true;
            }
        }

        return false;
    }

    public void Localize()
    {
        if (!PublicFacing && (ClientGameManager.Get() == null || !ClientGameManager.Get().HasDeveloperAccess()))
        {
            Command = internalCommand;
            Description = string.Empty;
            Aliases.Clear();
            return;
        }

        Command = LocalizeSlashCommand(internalCommand);
        Description = ScriptLocalization.Get(ScriptLocalization.GetSlashCommandDescKey(internalCommand));
        Aliases.Clear();

        for (int aliasId = 1; aliasId < 10; aliasId++)
        {
            if (!LocalizationManager.TryGetTermTranslation(
                    ScriptLocalization.GetSlashCommandAliasKey(internalCommand, aliasId),
                    out string Translation))
            {
                break;
            }

            Aliases.Add(Translation);
        }
    }

    private string LocalizeSlashCommand(string command)
    {
        return LocalizationManager.TryGetTermTranslation(
            ScriptLocalization.GetSlashCommandKey(command),
            out string Translation)
            ? Translation
            : command;
    }
}