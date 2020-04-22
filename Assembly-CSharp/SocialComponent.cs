using System;
using System.Collections.Generic;

[Serializable]
public class SocialComponent : ICloneable
{
	[Serializable]
	public class FriendData
	{
		public int LastSeenTitleID;

		public int LastSeenTitleLevel;

		public int LastSeenBackbroundID;

		public int LastSeenForegroundID;

		public int LastSeenRibbonID;

		public string LastSeenNote;

		public FriendData()
		{
			LastSeenTitleID = -1;
			LastSeenTitleLevel = -1;
			LastSeenForegroundID = -1;
			LastSeenBackbroundID = -1;
			LastSeenRibbonID = -1;
			LastSeenNote = string.Empty;
		}
	}

	public Dictionary<long, FriendData> FriendInfo;

	public DateTime TimeOfDecay;

	public int ReportedPermanentPoints
	{
		get;
		set;
	}

	public int ReportedTemporaryPoints
	{
		get;
		set;
	}

	public Dictionary<int, int> GrantedRAFRewards
	{
		get;
		set;
	}

	public SocialComponent()
	{
		FriendInfo = new Dictionary<long, FriendData>();
		ReportedPermanentPoints = 0;
		ReportedTemporaryPoints = 0;
		TimeOfDecay = DateTime.MinValue;
		GrantedRAFRewards = new Dictionary<int, int>();
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public FriendData GetOrCreateFriendInfo(long friendAccountId)
	{
		if (!FriendInfo.TryGetValue(friendAccountId, out FriendData value))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			value = new FriendData();
			FriendInfo[friendAccountId] = value;
		}
		return value;
	}

	public void UpdateLastSeenVisuals(long friendAccountId, int titleId, int titleLevel, int bgId, int fgId, int ribbonId, string note)
	{
		FriendData orCreateFriendInfo = GetOrCreateFriendInfo(friendAccountId);
		orCreateFriendInfo.LastSeenTitleID = titleId;
		orCreateFriendInfo.LastSeenTitleLevel = titleLevel;
		orCreateFriendInfo.LastSeenForegroundID = fgId;
		orCreateFriendInfo.LastSeenBackbroundID = bgId;
		orCreateFriendInfo.LastSeenRibbonID = ribbonId;
		orCreateFriendInfo.LastSeenNote = note;
	}

	public void UpdateNote(long friendAccountId, string note)
	{
		FriendData orCreateFriendInfo = GetOrCreateFriendInfo(friendAccountId);
		orCreateFriendInfo.LastSeenNote = note.Substring(0, Math.Min(note.Length, 50));
	}

	public void AddReportAgainst()
	{
		ReportedPermanentPoints++;
		ReportedTemporaryPoints++;
	}

	public void CalculatePointDecay(float ReportDecayValue)
	{
		if (ReportedTemporaryPoints <= 0)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(TimeOfDecay != DateTime.MinValue))
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (!(DateTime.UtcNow > TimeOfDecay))
				{
					return;
				}
				int num = (int)((DateTime.UtcNow - TimeOfDecay).TotalHours * (double)ReportDecayValue);
				if (num > ReportedTemporaryPoints)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							ReportedTemporaryPoints = 0;
							return;
						}
					}
				}
				ReportedTemporaryPoints -= num;
				return;
			}
		}
	}

	public void CalculateNewTimeOfDecay(int muteDuration)
	{
		DateTime dateTime;
		if (muteDuration > 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			dateTime = DateTime.UtcNow + TimeSpan.FromSeconds(muteDuration) + TimeSpan.FromHours(12.0);
		}
		else
		{
			dateTime = DateTime.UtcNow + TimeSpan.FromHours(1.0);
		}
		if (!(dateTime > TimeOfDecay))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			TimeOfDecay = dateTime;
			return;
		}
	}
}
