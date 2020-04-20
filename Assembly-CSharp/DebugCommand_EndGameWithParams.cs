using System;

public class DebugCommand_EndGameWithParams : DebugCommand
{
	public override string GetDebugItemName()
	{
		return "End Game (With Parameters)";
	}

	public override string GetPath()
	{
		return "End Game";
	}

	public override string GetSlashCommand()
	{
		return "/endgame";
	}

	public override bool OnSlashCommand(string arguments)
	{
		string[] array = new string[]
		{
			"win",
			"loss",
			"tie"
		};
		string text = "\r\n/endgame [win/loss/tie] [matchSeconds] [ggPacksUsedCount] [ggPacksUsedToSelf=true/false] [playWithFriendsBonus=true/false] [playedLastTurn=true/false]\r\n/endgame help";
		try
		{
			if (!HydrogenConfig.Get().AllowDebugCommands)
			{
				return false;
			}
			string[] array2 = arguments.Split(null);
			if (!array2[0].IsNullOrEmpty() && !array2[0].EqualsIgnoreCase("help"))
			{
				if (Array.IndexOf<string>(array, array2[0].ToLower()) != -1)
				{
					PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
					Team team = localPlayerData.LookupDetails().m_team;
					GameResult debugResult = GameResultExtensions.Parse(array2[0], team);
					int num;
					if (array2.Length > 1)
					{
						num = int.Parse(array2[1]);
					}
					else
					{
						num = 0;
					}
					int matchSeconds = num;
					int num2;
					if (array2.Length > 2)
					{
						num2 = int.Parse(array2[2]);
					}
					else
					{
						num2 = 0;
					}
					int ggBoostUsedCount = num2;
					bool ggBoostUsedToSelf = array2.Length > 3 && bool.Parse(array2[3]);
					bool flag;
					if (array2.Length > 4)
					{
						flag = bool.Parse(array2[4]);
					}
					else
					{
						flag = false;
					}
					bool playWithFriendsBonus = flag;
					bool flag2;
					if (array2.Length > 5)
					{
						flag2 = bool.Parse(array2[5]);
					}
					else
					{
						flag2 = true;
					}
					bool playedLastTurn = flag2;
					ClientGameManager.Get().RequestToUseGGPack();
					localPlayerData.CallCmdDebugEndGame(debugResult, matchSeconds, ggBoostUsedCount, ggBoostUsedToSelf, playWithFriendsBonus, playedLastTurn);
					return true;
				}
			}
			TextConsole.Get().Write(text, ConsoleMessageType.SystemMessage);
			return false;
		}
		catch (Exception)
		{
			TextConsole.Get().Write(text, ConsoleMessageType.SystemMessage);
			return false;
		}
		return true;
	}
}
