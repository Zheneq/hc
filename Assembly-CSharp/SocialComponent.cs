using System;
using System.Collections.Generic;

[Serializable]
public class SocialComponent : ICloneable
{
	public Dictionary<long, SocialComponent.FriendData> FriendInfo;

	public DateTime TimeOfDecay;

	public SocialComponent()
	{
		this.FriendInfo = new Dictionary<long, SocialComponent.FriendData>();
		this.ReportedPermanentPoints = 0;
		this.ReportedTemporaryPoints = 0;
		this.TimeOfDecay = DateTime.MinValue;
		this.GrantedRAFRewards = new Dictionary<int, int>();
	}

	public int ReportedPermanentPoints { get; set; }

	public int ReportedTemporaryPoints { get; set; }

	public Dictionary<int, int> GrantedRAFRewards { get; set; }

	public object Clone()
	{
		return base.MemberwiseClone();
	}

	public SocialComponent.FriendData GetOrCreateFriendInfo(long friendAccountId)
	{
		SocialComponent.FriendData friendData;
		if (!this.FriendInfo.TryGetValue(friendAccountId, out friendData))
		{
			friendData = new SocialComponent.FriendData();
			this.FriendInfo[friendAccountId] = friendData;
		}
		return friendData;
	}

	public void UpdateLastSeenVisuals(long friendAccountId, int titleId, int titleLevel, int bgId, int fgId, int ribbonId, string note)
	{
		SocialComponent.FriendData orCreateFriendInfo = this.GetOrCreateFriendInfo(friendAccountId);
		orCreateFriendInfo.LastSeenTitleID = titleId;
		orCreateFriendInfo.LastSeenTitleLevel = titleLevel;
		orCreateFriendInfo.LastSeenForegroundID = fgId;
		orCreateFriendInfo.LastSeenBackbroundID = bgId;
		orCreateFriendInfo.LastSeenRibbonID = ribbonId;
		orCreateFriendInfo.LastSeenNote = note;
	}

	public void UpdateNote(long friendAccountId, string note)
	{
		SocialComponent.FriendData orCreateFriendInfo = this.GetOrCreateFriendInfo(friendAccountId);
		orCreateFriendInfo.LastSeenNote = note.Substring(0, Math.Min(note.Length, 0x32));
	}

	public void AddReportAgainst()
	{
		this.ReportedPermanentPoints++;
		this.ReportedTemporaryPoints++;
	}

	public void CalculatePointDecay(float ReportDecayValue)
	{
		if (this.ReportedTemporaryPoints > 0)
		{
			if (this.TimeOfDecay != DateTime.MinValue)
			{
				if (DateTime.UtcNow > this.TimeOfDecay)
				{
					int num = (int)((DateTime.UtcNow - this.TimeOfDecay).TotalHours * (double)ReportDecayValue);
					if (num > this.ReportedTemporaryPoints)
					{
						this.ReportedTemporaryPoints = 0;
					}
					else
					{
						this.ReportedTemporaryPoints -= num;
					}
				}
			}
		}
	}

	public void CalculateNewTimeOfDecay(int muteDuration)
	{
		DateTime dateTime;
		if (muteDuration > 0)
		{
			dateTime = DateTime.UtcNow + TimeSpan.FromSeconds((double)muteDuration) + TimeSpan.FromHours(12.0);
		}
		else
		{
			dateTime = DateTime.UtcNow + TimeSpan.FromHours(1.0);
		}
		if (dateTime > this.TimeOfDecay)
		{
			this.TimeOfDecay = dateTime;
		}
	}

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
			this.LastSeenTitleID = -1;
			this.LastSeenTitleLevel = -1;
			this.LastSeenForegroundID = -1;
			this.LastSeenBackbroundID = -1;
			this.LastSeenRibbonID = -1;
			this.LastSeenNote = string.Empty;
		}
	}
}
