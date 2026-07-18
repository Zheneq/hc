using System;

[Serializable]
public class PersistedAccountDataSnapshot : ICloneable
{
	public long AccountId
	{
		get;
		set;
	}

	public long SnapshotId
	{
		get;
		set;
	}

	public DateTime SnapshotDate
	{
		get;
		set;
	}

	public string SnapshottingUserName
	{
		get;
		set;
	}

	public PersistedAccountDataSnapshotReason SnapshotReason
	{
		get;
		set;
	}

	public string SnapshotNote
	{
		get;
		set;
	}

	public PersistedAccountData AccountData
	{
		get;
		set;
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
