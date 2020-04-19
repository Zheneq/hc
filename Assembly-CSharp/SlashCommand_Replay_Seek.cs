using System;

public class SlashCommand_Replay_Seek : SlashCommand
{
	public SlashCommand_Replay_Seek() : base("/replay_seek", SlashCommandType.InGame)
	{
		base.PublicFacing = false;
	}

	public override void OnSlashCommand(string arguments)
	{
		string[] array = arguments.Split(null);
		string text = "/replay_seek command requires following format\n\t/replay_seek [turnId]";
		if (!array[0].IsNullOrEmpty())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SlashCommand_Replay_Seek.OnSlashCommand(string)).MethodHandle;
			}
			if (!array[0].EqualsIgnoreCase("help"))
			{
				ReplayPlayManager replayPlayManager = ReplayPlayManager.Get();
				if (replayPlayManager != null && replayPlayManager.IsPlayback())
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
						turn = Convert.ToInt32(array[0]),
						phase = AbilityPriority.INVALID
					});
					TextConsole.Get().Write(string.Format("Seeking to turn {0}.", array[0]), ConsoleMessageType.SystemMessage);
				}
				else
				{
					TextConsole.Get().Write("Not currently playing a replay.", ConsoleMessageType.SystemMessage);
				}
				return;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		TextConsole.Get().Write(text, ConsoleMessageType.SystemMessage);
	}
}
