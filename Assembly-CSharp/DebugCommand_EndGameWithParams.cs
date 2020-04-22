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
		string[] array = new string[3]
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
			if (array2[0].IsNullOrEmpty() || array2[0].EqualsIgnoreCase("help"))
			{
				goto IL_0081;
			}
			while (true)
			{
				switch (7)
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
			if (Array.IndexOf(array, array2[0].ToLower()) == -1)
			{
				goto IL_0081;
			}
			PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
			Team team = localPlayerData.LookupDetails().m_team;
			GameResult debugResult = GameResultExtensions.Parse(array2[0], team);
			int num;
			if (array2.Length > 1)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
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
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				num2 = int.Parse(array2[2]);
			}
			else
			{
				num2 = 0;
			}
			int ggBoostUsedCount = num2;
			bool ggBoostUsedToSelf = array2.Length > 3 && bool.Parse(array2[3]);
			int num3;
			if (array2.Length > 4)
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
				num3 = (bool.Parse(array2[4]) ? 1 : 0);
			}
			else
			{
				num3 = 0;
			}
			bool playWithFriendsBonus = (byte)num3 != 0;
			int num4;
			if (array2.Length > 5)
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
				num4 = (bool.Parse(array2[5]) ? 1 : 0);
			}
			else
			{
				num4 = 1;
			}
			bool playedLastTurn = (byte)num4 != 0;
			ClientGameManager.Get().RequestToUseGGPack();
			localPlayerData.CallCmdDebugEndGame(debugResult, matchSeconds, ggBoostUsedCount, ggBoostUsedToSelf, playWithFriendsBonus, playedLastTurn);
			goto end_IL_0025;
			IL_0081:
			TextConsole.Get().Write(text);
			return false;
			end_IL_0025:;
		}
		catch (Exception)
		{
			TextConsole.Get().Write(text);
			return false;
		}
		return true;
	}
}
