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
		int result;
		if (value >= GameStatus.Launching)
		{
			result = ((value <= GameStatus.Stopped) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}
}
