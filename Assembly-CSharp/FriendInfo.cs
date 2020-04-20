using System;

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
		this.BannerID = -1;
		this.EmblemID = -1;
		this.TitleID = -1;
		this.TitleLevel = -1;
		this.RibbonID = -1;
	}

	public bool IsJoinable(LobbyGameplayOverrides GameplayOverrides)
	{
		bool result;
		if (this.StatusString == "In Game")
		{
			result = GameplayOverrides.AllowSpectatorsOutsideCustom;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override string ToString()
	{
		return string.Format("{0} ({1}) {2}", this.FriendHandle, this.FriendAccountId, this.FriendStatus.ToString());
	}
}
