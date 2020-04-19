using System;

public class SlashCommand_Replay_FastForward : SlashCommand
{
	public SlashCommand_Replay_FastForward() : base("/replay_ff", SlashCommandType.InGame)
	{
		base.PublicFacing = false;
	}

	public override void OnSlashCommand(string arguments)
	{
		string[] array = arguments.Split(null);
		string text = "/replay_ff command requires following format\n\t/replay_ff [turnId]";
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SlashCommand_Replay_FastForward.OnSlashCommand(string)).MethodHandle;
			}
			if (!array[0].EqualsIgnoreCase("help"))
			{
				ReplayPlayManager replayPlayManager = ReplayPlayManager.Get();
				if (replayPlayManager != null)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (replayPlayManager.IsPlayback())
					{
						for (;;)
						{
							switch (2)
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
						TextConsole.Get().Write(string.Format("Fastforwarded to turn {0}.", array[0]), ConsoleMessageType.SystemMessage);
						return;
					}
				}
				TextConsole.Get().Write("Not currently playing a replay.", ConsoleMessageType.SystemMessage);
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
