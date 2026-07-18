using System;

[Serializable]
public class PersistedCharacterMatchData : ICloneable
{
	public SchemaVersion<MatchSchemaChange> SchemaVersion
	{
		get;
		set;
	}

	public DateTime CreateDate
	{
		get;
		set;
	}

	public DateTime UpdateDate
	{
		get;
		set;
	}

	public string GameServerProcessCode
	{
		get;
		set;
	}

	public MatchComponent MatchComponent
	{
		get;
		set;
	}

	public MatchDetailsComponent MatchDetailsComponent
	{
		get;
		set;
	}

	public MatchFreelancerStats MatchFreelancerStats
	{
		get;
		set;
	}

	public PersistedCharacterMatchData()
	{
		SchemaVersion = new SchemaVersion<MatchSchemaChange>();
		MatchComponent = new MatchComponent();
		MatchDetailsComponent = new MatchDetailsComponent();
		MatchFreelancerStats = null;
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
