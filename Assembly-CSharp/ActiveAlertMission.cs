using System;

[Serializable]
public class ActiveAlertMission : AlertMissionBase
{
	public DateTime StartTimePST;

	public ActiveAlertMission()
	{
	}

	public ActiveAlertMission(AlertMission alert)
	{
		base.CopyBaseVariables(alert);
	}

	public ActiveAlertMission Clone()
	{
		return (ActiveAlertMission)base.MemberwiseClone();
	}
}
