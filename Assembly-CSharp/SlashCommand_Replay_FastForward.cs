using System;

public class SlashCommand_Replay_FastForward : SlashCommand
{
	public SlashCommand_Replay_FastForward()
		: base("/replay_ff", SlashCommandType.InGame)
	{
		base.PublicFacing = false;
	}

	public override void OnSlashCommand(string arguments)
	{
		string[] array = arguments.Split(null);
		string text = "/replay_ff command requires following format\n\t/replay_ff [turnId]";
		if (!array[0].IsNullOrEmpty())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!array[0].EqualsIgnoreCase("help"))
			{
				ReplayPlayManager replayPlayManager = ReplayPlayManager.Get();
				if (replayPlayManager != null)
				{
					while (true)
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
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								replayPlayManager.Seek(new ReplayTimestamp
								{
									turn = Convert.ToInt32(array[0]),
									phase = AbilityPriority.INVALID
								});
								TextConsole.Get().Write($"Fastforwarded to turn {array[0]}.");
								return;
							}
						}
					}
				}
				TextConsole.Get().Write("Not currently playing a replay.");
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		TextConsole.Get().Write(text);
	}
}
