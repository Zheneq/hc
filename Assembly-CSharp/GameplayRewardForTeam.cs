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
		if (m_objectivePointAdjust == 0)
		{
			return;
		}
		while (true)
		{
			ObjectivePoints objectivePoints = ObjectivePoints.Get();
			if (objectivePoints != null)
			{
				while (true)
				{
					objectivePoints.AdjustUnresolvedPoints(m_objectivePointAdjust, team);
					return;
				}
			}
			return;
		}
	}
}
