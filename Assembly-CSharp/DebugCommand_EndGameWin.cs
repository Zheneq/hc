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
		localPlayerData.CallCmdDebugEndGame((team != 0) ? GameResult.TeamBWon : GameResult.TeamAWon, 0, 0, false, false, true);
	}

	public override string GetSlashCommand()
	{
		return "/endgame";
	}

	public override bool OnSlashCommand(string arguments)
	{
		if (arguments.EqualsIgnoreCase("win"))
		{
			while (true)
			{
				switch (4)
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

	public override bool CheckGameControllerTrigger()
	{
		if (Application.platform == RuntimePlatform.XboxOne)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					int result;
					if (ControlpadGameplay.Get().GetButtonDown(ControlpadInputValue.Button_leftShoulder))
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
						result = (ControlpadGameplay.Get().GetButtonDown(ControlpadInputValue.Button_rightShoulder) ? 1 : 0);
					}
					else
					{
						result = 0;
					}
					return (byte)result != 0;
				}
				}
			}
		}
		return false;
	}
}
