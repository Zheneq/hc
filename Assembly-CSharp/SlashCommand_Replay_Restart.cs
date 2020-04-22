public class SlashCommand_Replay_Restart : SlashCommand
{
	public SlashCommand_Replay_Restart()
		: base("/replay_restart", SlashCommandType.InGame)
	{
		base.PublicFacing = false;
	}

	public override void OnSlashCommand(string arguments)
	{
		ReplayPlayManager replayPlayManager = ReplayPlayManager.Get();
		if (replayPlayManager != null)
		{
			while (true)
			{
				switch (5)
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
			if (replayPlayManager.IsPlayback())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						replayPlayManager.Seek(new ReplayTimestamp
						{
							turn = 1,
							phase = AbilityPriority.INVALID
						});
						TextConsole.Get().Write("Restarted replay.");
						return;
					}
				}
			}
		}
		TextConsole.Get().Write("Not currently playing a replay.");
	}
}
