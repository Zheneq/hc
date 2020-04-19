using System;

public static class GameStatusExtensions
{
	public static bool IsActiveStatus(this GameStatus value)
	{
		return value >= GameStatus.Assembling && value <= GameStatus.Started;
	}

	public static bool IsPreLaunchStatus(this GameStatus value)
	{
		return value >= GameStatus.Assembling && value < GameStatus.Launching;
	}

	public static bool IsPostLaunchStatus(this GameStatus value)
	{
		bool result;
		if (value >= GameStatus.Launching)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameStatus.IsPostLaunchStatus()).MethodHandle;
			}
			result = (value <= GameStatus.Stopped);
		}
		else
		{
			result = false;
		}
		return result;
	}
}
