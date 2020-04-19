using System;

public static class GameOptionFlagExtensions
{
	public static bool HasGameOption(this GameOptionFlag thisFlags, GameOptionFlag gameOptionFlag)
	{
		return (thisFlags & gameOptionFlag) == gameOptionFlag;
	}

	public static GameOptionFlag WithGameOption(this GameOptionFlag thisFlags, GameOptionFlag flag)
	{
		return thisFlags | flag;
	}

	public static GameOptionFlag WithoutGameOption(this GameOptionFlag thisFlags, GameOptionFlag flag)
	{
		return thisFlags & ~flag;
	}
}
