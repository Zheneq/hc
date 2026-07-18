using I2.Loc;

public class SlashCommand_Language : SlashCommand
{
    public SlashCommand_Language()
        : base("/language", SlashCommandType.Everywhere)
    {
        PublicFacing = false;
    }

    public override void OnSlashCommand(string arguments)
    {
        if (arguments.IsNullOrEmpty())
        {
            TextConsole.Get().Write($"Current Language: {LocalizationManager.CurrentLanguage}");
            return;
        }

        if (!LocalizationManager.HasLanguage(arguments))
        {
            TextConsole.Get().Write($"Unrecognized Language: {arguments}");
            return;
        }

        LocalizationManager.CurrentLanguage = arguments;
        TextConsole.Get().Write($"Language changed to: {LocalizationManager.CurrentLanguage}");
        ClientGameManager.Get().SetNewSessionLanguage(LocalizationManager.CurrentLanguageCode);
    }
}