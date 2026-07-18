public class DebugCommand_EndGameLoss : DebugCommand
{
	public override string GetDebugItemName()
	{
		return "End Game (Loss)";
	}

	public override string GetPath()
	{
		return "End Game";
	}

	public override void OnIncreaseClick()
	{
		PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
		Team team = localPlayerData.LookupDetails().m_team;
		localPlayerData.CallCmdDebugEndGame((team != 0) ? GameResult.TeamAWon : GameResult.TeamBWon, 0, 0, false, false, true);
	}

	public override string GetSlashCommand()
	{
		return "/endgame";
	}

	public override bool OnSlashCommand(string arguments)
	{
		if (arguments.EqualsIgnoreCase("loss"))
		{
			OnIncreaseClick();
			return true;
		}
		return false;
	}
}
