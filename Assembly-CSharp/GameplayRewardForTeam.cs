using System;

[Serializable]
public class GameplayRewardForTeam
{
	public string m_name = "Gameplay Reward";

	public int m_objectivePointAdjust;

	public StandardEffectInfo m_effectOnMembers;

	public int m_creditsToMembers;

	public int m_healingToMembers;

	public int m_techPointsToMembers;

	public AbilityStatMod[] m_permanentStatMods;

	public StatusType[] m_permanentStatusChanges;

	public void ClientApplyRewardTo(Team team)
	{
		if (this.m_objectivePointAdjust != 0)
		{
			ObjectivePoints objectivePoints = ObjectivePoints.Get();
			if (objectivePoints != null)
			{
				objectivePoints.AdjustUnresolvedPoints(this.m_objectivePointAdjust, team);
			}
		}
	}
}
