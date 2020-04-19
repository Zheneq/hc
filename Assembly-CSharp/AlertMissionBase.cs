using System;

[Serializable]
public abstract class AlertMissionBase
{
	public AlertMissionType Type;

	public float DurationHours;

	public int QuestId;

	public CurrencyType BonusType;

	public int BonusMultiplier;

	protected void CopyBaseVariables(AlertMissionBase other)
	{
		this.Type = other.Type;
		this.DurationHours = other.DurationHours;
		this.QuestId = other.QuestId;
		this.BonusType = other.BonusType;
		this.BonusMultiplier = other.BonusMultiplier;
	}
}
