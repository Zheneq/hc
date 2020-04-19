using System;

public class NPCSuicideForPointsAbility : Ability
{
	public int m_thisTeamObjectivePointChange = 1;

	public override bool IsDamageUnpreventable()
	{
		return true;
	}
}
