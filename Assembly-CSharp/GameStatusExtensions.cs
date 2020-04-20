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
			result = (value <= GameStatus.Stopped);
		}
		else
		{
			result = false;
		}
		return result;
	}
}
