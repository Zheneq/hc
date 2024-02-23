using System;
using System.Text;

[Serializable]
public class FriendInfo
{
	public long FriendAccountId;

	public string FriendHandle;

	public FriendStatus FriendStatus;

	public bool IsOnline;

	public string StatusString;

	public string FriendNote;

	public int BannerID;

	public int EmblemID;

	public int TitleID;

	public int TitleLevel;

	public int RibbonID;

	public FriendInfo()
	{
		BannerID = -1;
		EmblemID = -1;
		TitleID = -1;
		TitleLevel = -1;
		RibbonID = -1;
	}

	public bool IsJoinable(LobbyGameplayOverrides GameplayOverrides)
	{
		int result;
		if (StatusString == "In Game")
		{
			result = (GameplayOverrides.AllowSpectatorsOutsideCustom ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public override string ToString()
	{
		return new StringBuilder().Append(FriendHandle).Append(" (").Append(FriendAccountId).Append(") ").Append(FriendStatus.ToString()).ToString();
	}
}
