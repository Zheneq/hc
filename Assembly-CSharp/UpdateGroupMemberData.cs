using System;

[Serializable]
public class UpdateGroupMemberData
{
	public string MemberDisplayName;

	public string MemberHandle;

	public bool HasFullAccess;

	public bool IsLeader;

	public bool IsReady;

	public bool IsInGame;

	public long CreateGameTimestamp;

	public long AccountID;

	public CharacterType MemberDisplayCharacter;

	public GroupMemberVisualData? VisualData;

	public DateTime? PenaltyTimeout;

	public float GameLeavingPoints;

	public CharacterVisualInfo VisualInfo
	{
		get
		{
			CharacterVisualInfo result;
			if (VisualData.HasValue)
			{
				GroupMemberVisualData value = VisualData.Value;
				result = value.VisualInfo;
			}
			else
			{
				result = new CharacterVisualInfo(0, 0, 0);
			}
			return result;
		}
	}

	public int ForegroundBannerID
	{
		get
		{
			int result;
			if (VisualData.HasValue)
			{
				GroupMemberVisualData value = VisualData.Value;
				result = value.ForegroundBannerID;
			}
			else
			{
				result = 0;
			}
			return result;
		}
	}

	public int BackgroundBannerID
	{
		get
		{
			int result;
			if (VisualData.HasValue)
			{
				GroupMemberVisualData value = VisualData.Value;
				result = value.BackgroundBannerID;
			}
			else
			{
				result = 0;
			}
			return result;
		}
	}

	public int TitleID
	{
		get
		{
			int result;
			if (VisualData.HasValue)
			{
				GroupMemberVisualData value = VisualData.Value;
				result = value.TitleID;
			}
			else
			{
				result = 0;
			}
			return result;
		}
	}

	public int RibbonID
	{
		get
		{
			int result;
			if (VisualData.HasValue)
			{
				GroupMemberVisualData value = VisualData.Value;
				result = value.RibbonID;
			}
			else
			{
				result = 0;
			}
			return result;
		}
	}
}
