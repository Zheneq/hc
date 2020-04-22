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
		CopyBaseVariables(alert);
	}

	public ActiveAlertMission Clone()
	{
		return (ActiveAlertMission)MemberwiseClone();
	}
}
