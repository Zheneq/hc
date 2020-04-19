using System;

public static class GameTypeExtensions
{
	public static bool IsHumanVsHumanGame(this GameType gameType)
	{
		if (gameType != GameType.PvP)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameType.IsHumanVsHumanGame()).MethodHandle;
			}
			if (gameType != GameType.Ranked)
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
				return gameType == GameType.NewPlayerPvP;
			}
		}
		return true;
	}

	public static bool IsQueueable(this GameType gameType)
	{
		if (gameType != GameType.Coop)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameType.IsQueueable()).MethodHandle;
			}
			if (gameType != GameType.PvP)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (gameType != GameType.Ranked)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameType.IsAutoLaunchable()).MethodHandle;
			}
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameType.TracksElo()).MethodHandle;
			}
			if (gameType != GameType.Ranked)
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
				return gameType == GameType.NewPlayerPvP;
			}
		}
		return true;
	}

	public static bool AllowsLockedCharacters(this GameType gameType)
	{
		if (gameType != GameType.Practice)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameType.AllowsLockedCharacters()).MethodHandle;
			}
			if (gameType != GameType.Tutorial)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				return gameType == GameType.NewPlayerSolo;
			}
		}
		return true;
	}

	public static bool AllowsReconnect(this GameType gameType)
	{
		if (gameType != GameType.Coop)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameType.AllowsReconnect()).MethodHandle;
			}
			if (gameType != GameType.PvP)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (gameType != GameType.Ranked)
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
					if (gameType != GameType.NewPlayerPvP)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
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
