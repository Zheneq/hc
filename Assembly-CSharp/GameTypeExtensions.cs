using System;

public static class GameTypeExtensions
{
	public static bool IsHumanVsHumanGame(this GameType gameType)
	{
		if (gameType != GameType.PvP)
		{
			if (gameType != GameType.Ranked)
			{
				return gameType == GameType.NewPlayerPvP;
			}
		}
		return true;
	}

	public static bool IsQueueable(this GameType gameType)
	{
		if (gameType != GameType.Coop)
		{
			if (gameType != GameType.PvP)
			{
				if (gameType != GameType.Ranked)
				{
					return gameType == GameType.NewPlayerPvP;
				}
			}
		}
		return true;
	}

	public static bool IsAutoLaunchable(this GameType gameType)
	{
		bool result;
		if (gameType != GameType.Custom && gameType != GameType.Practice)
		{
			result = (gameType == GameType.Tutorial);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public static bool TracksElo(this GameType gameType)
	{
		if (gameType != GameType.PvP)
		{
			if (gameType != GameType.Ranked)
			{
				return gameType == GameType.NewPlayerPvP;
			}
		}
		return true;
	}

	public static bool AllowsLockedCharacters(this GameType gameType)
	{
		if (gameType != GameType.Practice)
		{
			if (gameType != GameType.Tutorial)
			{
				return gameType == GameType.NewPlayerSolo;
			}
		}
		return true;
	}

	public static bool AllowsReconnect(this GameType gameType)
	{
		if (gameType != GameType.Coop)
		{
			if (gameType != GameType.PvP)
			{
				if (gameType != GameType.Ranked)
				{
					if (gameType != GameType.NewPlayerPvP)
					{
						return gameType == GameType.Custom;
					}
				}
			}
		}
		return true;
	}

	public static string GetDisplayName(this GameType gameType)
	{
		switch (gameType)
		{
		case GameType.Custom:
			return StringUtil.TR("Custom", "Global");
		case GameType.Practice:
			return StringUtil.TR("Practice", "Global");
		case GameType.Tutorial:
			return StringUtil.TR("Tutorial", "Global");
		case GameType.Coop:
			return StringUtil.TR("VersusBots", "Global");
		case GameType.PvP:
		case GameType.NewPlayerPvP:
			return StringUtil.TR("PVP", "Global");
		case GameType.Solo:
		case GameType.PvE:
		case GameType.NewPlayerSolo:
			return StringUtil.TR("PvE", "Global");
		case GameType.Duel:
			return StringUtil.TR("Duel", "Global");
		case GameType.QuickPlay:
			return StringUtil.TR("QuickPlay", "Global");
		case GameType.Ranked:
			return StringUtil.TR("Ranked", "Global");
		}
		return gameType.ToString() + "#NotLocalized";
	}
}
