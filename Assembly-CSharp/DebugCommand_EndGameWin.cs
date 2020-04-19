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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(DebugCommand_EndGameWin.OnSlashCommand(string)).MethodHandle;
			}
			this.OnIncreaseClick();
			return true;
		}
		return false;
	}

	public override bool CheckGameControllerTrigger()
	{
		if (Application.platform == RuntimePlatform.XboxOne)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(DebugCommand_EndGameWin.CheckGameControllerTrigger()).MethodHandle;
			}
			bool result;
			if (ControlpadGameplay.Get().GetButtonDown(ControlpadInputValue.Button_leftShoulder))
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
