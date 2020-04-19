using System;

public class SlashCommand_Replay_Restart : SlashCommand
{
	public SlashCommand_Replay_Restart() : base("/replay_restart", SlashCommandType.InGame)
	{
		base.PublicFacing = false;
	}

	public override void OnSlashCommand(string arguments)
	{
		ReplayPlayManager replayPlayManager = ReplayPlayManager.Get();
		if (replayPlayManager != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SlashCommand_Replay_Restart.OnSlashCommand(string)).MethodHandle;
			}
			if (replayPlayManager.IsPlayback())
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				replayPlayManager.Seek(new ReplayTimestamp
				{
					turn = 1,
					phase = AbilityPriority.INVALID
				});
				TextConsole.Get().Write("Restarted replay.", ConsoleMessageType.SystemMessage);
				return;
			}
		}
		TextConsole.Get().Write("Not currently playing a replay.", ConsoleMessageType.SystemMessage);
	}
}
