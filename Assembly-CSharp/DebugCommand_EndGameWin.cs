using System;
using UnityEngine;

public class DebugCommand_EndGameWin : DebugCommand
{
	public override string GetDebugItemName()
	{
		return "End Game (Win)";
	}

	public override string GetPath()
	{
		return "End Game";
	}

	public override void OnIncreaseClick()
	{
		PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
		Team team = localPlayerData.LookupDetails().m_team;
		localPlayerData.CallCmdDebugEndGame((team != Team.TeamA) ? GameResult.TeamBWon : GameResult.TeamAWon, 0, 0, false, false, true);
	}

	public override string GetSlashCommand()
	{
		return "/endgame";
	}

	public override bool OnSlashCommand(string arguments)
	{
		if (arguments.EqualsIgnoreCase("win"))
		{
			this.OnIncreaseClick();
			return true;
		}
		return false;
	}

	public override bool CheckGameControllerTrigger()
	{
		if (Application.platform == RuntimePlatform.XboxOne)
		{
			bool result;
			if (ControlpadGameplay.Get().GetButtonDown(ControlpadInputValue.Button_leftShoulder))
			{
				result = ControlpadGameplay.Get().GetButtonDown(ControlpadInputValue.Button_rightShoulder);
			}
			else
			{
				result = false;
			}
			return result;
		}
		return false;
	}
}
