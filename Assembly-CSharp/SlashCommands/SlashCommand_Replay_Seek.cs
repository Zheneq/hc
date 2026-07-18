using System;

public class SlashCommand_Replay_Seek : SlashCommand
{
    public SlashCommand_Replay_Seek()
        : base("/replay_seek", SlashCommandType.InGame)
    {
        PublicFacing = false;
    }

    public override void OnSlashCommand(string arguments)
    {
        string[] array = arguments.Split(null);
        if (array[0].IsNullOrEmpty() || array[0].EqualsIgnoreCase("help"))
        {
            TextConsole.Get().Write("/replay_seek command requires following format\n\t/replay_seek [turnId]");
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
        TextConsole.Get().Write($"Seeking to turn {array[0]}.");
    }
}