using System;

public class SlashCommand_Replay_FastForward : SlashCommand
{
    public SlashCommand_Replay_FastForward()
        : base("/replay_ff", SlashCommandType.InGame)
    {
        PublicFacing = false;
    }

    public override void OnSlashCommand(string arguments)
    {
        string[] array = arguments.Split(null);
        if (array[0].IsNullOrEmpty() || array[0].EqualsIgnoreCase("help"))
        {
            TextConsole.Get().Write("/replay_ff command requires following format\n\t/replay_ff [turnId]");
            return;
        }

        ReplayPlayManager replayPlayManager = ReplayPlayManager.Get();
        if (replayPlayManager == null || !replayPlayManager.IsPlayback())
        {
            TextConsole.Get().Write("Not currently playing a replay.");
            return;
        }

        replayPlayManager.Seek(
            new ReplayTimestamp
            {
                turn = Convert.ToInt32(array[0]),
                phase = AbilityPriority.INVALID
            });
        TextConsole.Get().Write($"Fastforwarded to turn {array[0]}.");
    }
}