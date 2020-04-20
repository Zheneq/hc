using System;

public class DebugCommand_EndGameTie : DebugCommand
{
	public override string GetDebugItemName()
	{
		return "End Game (Tie)";
	}

	public override string GetPath()
	{
		return "End Game";
	}

	public override void OnIncreaseClick()
	{
		PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
		localPlayerData.CallCmdDebugEndGame(GameResult.TieGame, 0, 0, false, false, true);
	}

	public override string GetSlashCommand()
	{
		return "/endgame";
	}

	public override bool OnSlashCommand(string arguments)
	{
		if (arguments.EqualsIgnoreCase("tie"))
		{
			this.OnIncreaseClick();
			return true;
		}
		return false;
	}
}
