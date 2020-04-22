using System;

[Serializable]
public class AlertMission : AlertMissionBase
{
	public QuestPrerequisites Prerequisites;

	public bool Enabled;

	public AlertMission Clone()
	{
		return (AlertMission)MemberwiseClone();
	}
}
