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
		Type = other.Type;
		DurationHours = other.DurationHours;
		QuestId = other.QuestId;
		BonusType = other.BonusType;
		BonusMultiplier = other.BonusMultiplier;
	}
}
