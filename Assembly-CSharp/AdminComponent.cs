using System;
using System.Collections.Generic;

[Serializable]
public class AdminComponent
{
	public AdminComponent()
	{
		this.AdminActions = new List<AdminComponent.AdminActionRecord>();
		this.ActiveQueuePenalties = new Dictionary<GameType, QueuePenalties>();
		this.Region = Region.US;
	}

	public object Clone()
	{
		return base.MemberwiseClone();
	}

	public AdminComponent CloneForClient()
	{
		return new AdminComponent
		{
			GameLeavingPoints = this.GameLeavingPoints
		};
	}

	public bool Locked { get; set; }

	public DateTime LockedUntil { get; set; }

	public bool Muted { get; set; }

	public DateTime MutedUntil { get; set; }

	public bool Online { get; set; }

	public DateTime LastLogin { get; set; }

	public DateTime LastLogout { get; set; }

	public string LastLogoutSessionToken { get; set; }

	public Region Region { get; set; }

	public string LanguageCode { get; set; }

	public List<AdminComponent.AdminActionRecord> AdminActions { get; set; }

	public Dictionary<GameType, QueuePenalties> ActiveQueuePenalties { get; set; }

	public float GameLeavingPoints { get; set; }

	public DateTime GameLeavingLastForgivenessCheckpoint { get; set; }

	public enum AdminActionType
	{
		Lock,
		Unlock,
		Mute,
		Unmute,
		Kick,
		Alter
	}

	[Serializable]
	public class AdminActionRecord
	{
		public string AdminUsername { get; set; }

		public AdminComponent.AdminActionType ActionType { get; set; }

		public TimeSpan Duration { get; set; }

		public string Description { get; set; }

		public DateTime Time { get; set; }
	}
}
