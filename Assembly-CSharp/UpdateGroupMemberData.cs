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
			if (this.VisualData != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UpdateGroupMemberData.get_VisualInfo()).MethodHandle;
				}
				result = this.VisualData.Value.VisualInfo;
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
			if (this.VisualData != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UpdateGroupMemberData.get_ForegroundBannerID()).MethodHandle;
				}
				result = this.VisualData.Value.ForegroundBannerID;
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
			if (this.VisualData != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UpdateGroupMemberData.get_BackgroundBannerID()).MethodHandle;
				}
				result = this.VisualData.Value.BackgroundBannerID;
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
			if (this.VisualData != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UpdateGroupMemberData.get_TitleID()).MethodHandle;
				}
				result = this.VisualData.Value.TitleID;
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
			if (this.VisualData != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UpdateGroupMemberData.get_RibbonID()).MethodHandle;
				}
				result = this.VisualData.Value.RibbonID;
			}
			else
			{
				result = 0;
			}
			return result;
		}
	}
}
