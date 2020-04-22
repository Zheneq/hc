public class DebugCommand_EndGameNoResult : DebugCommand
{
	public override string GetDebugItemName()
	{
		return "End Game (No Result)";
	}

	public override string GetPath()
	{
		return "End Game";
	}

	public override void OnIncreaseClick()
	{
		PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
		localPlayerData.CallCmdDebugEndGame(GameResult.NoResult, 0, 0, false, false, true);
	}

	public override string GetSlashCommand()
	{
		return "/endgame";
	}

	public override bool OnSlashCommand(string arguments)
	{
		if (arguments.EqualsIgnoreCase("noresult"))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					OnIncreaseClick();
					return true;
				}
			}
		}
		return false;
	}
}
