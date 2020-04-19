using System;

public static class PlayerGameOptionFlagExtensions
{
	public static bool HasGameOption(this PlayerGameOptionFlag thisFlags, PlayerGameOptionFlag gameOptionFlag)
	{
		return (thisFlags & gameOptionFlag) == gameOptionFlag;
	}

	public static PlayerGameOptionFlag WithGameOption(this PlayerGameOptionFlag thisFlags, PlayerGameOptionFlag flag)
	{
		return thisFlags | flag;
	}

	public static PlayerGameOptionFlag WithoutGameOption(this PlayerGameOptionFlag thisFlags, PlayerGameOptionFlag flag)
	{
		return thisFlags & ~flag;
	}
}
