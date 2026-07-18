using System.IO;

public class SlashCommand_PlayReplay : SlashCommand
{
    public SlashCommand_PlayReplay()
        : base("/playreplay", SlashCommandType.InFrontEnd)
    {
    }

    public override void OnSlashCommand(string arguments)
    {
        ReplayPlayManager.Get().StartReplay(Path.Combine(HydrogenConfig.Get().ReplaysPath, arguments));
    }
}