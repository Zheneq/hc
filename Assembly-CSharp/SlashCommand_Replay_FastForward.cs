using System;
using System.Text;

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
			if (!array[0].EqualsIgnoreCase("help"))
			{
				ReplayPlayManager replayPlayManager = ReplayPlayManager.Get();
				if (replayPlayManager != null)
				{
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
								TextConsole.Get().Write(new StringBuilder().Append("Fastforwarded to turn ").Append(array[0]).Append(".").ToString());
								return;
							}
						}
					}
				}
				TextConsole.Get().Write("Not currently playing a replay.");
				return;
			}
		}
		TextConsole.Get().Write(text);
	}
}
