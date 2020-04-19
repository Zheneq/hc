using System;

[Serializable]
public class PersistedCharacterMatchData : ICloneable
{
	public PersistedCharacterMatchData()
	{
		this.SchemaVersion = new SchemaVersion<MatchSchemaChange>();
		this.MatchComponent = new MatchComponent();
		this.MatchDetailsComponent = new MatchDetailsComponent();
		this.MatchFreelancerStats = null;
	}

	public SchemaVersion<MatchSchemaChange> SchemaVersion { get; set; }

	public DateTime CreateDate { get; set; }

	public DateTime UpdateDate { get; set; }

	public string GameServerProcessCode { get; set; }

	public MatchComponent MatchComponent { get; set; }

	public MatchDetailsComponent MatchDetailsComponent { get; set; }

	public MatchFreelancerStats MatchFreelancerStats { get; set; }

	public object Clone()
	{
		return base.MemberwiseClone();
	}
}
