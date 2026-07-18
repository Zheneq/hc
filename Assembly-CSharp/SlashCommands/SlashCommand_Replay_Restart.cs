public class SlashCommand_Replay_Restart : SlashCommand
{
    public SlashCommand_Replay_Restart()
        : base("/replay_restart", SlashCommandType.InGame)
    {
        PublicFacing = false;
    }

    public override void OnSlashCommand(string arguments)
    {
        ReplayPlayManager replayPlayManager = ReplayPlayManager.Get();
        if (replayPlayManager == null || !replayPlayManager.IsPlayback())
        {
            TextConsole.Get().Write("Not currently playing a replay.");
            return;
        }

        replayPlayManager.Seek(
            new ReplayTimestamp
            {
                turn = 1,
                phase = AbilityPriority.INVALID
            });
        TextConsole.Get().Write("Restarted replay.");
    }
}