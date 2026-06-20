// ROGUES
// SERVER
public class AbilityUtil_Targeter_MartyrSmoothAoE : AbilityUtil_Targeter_AoE_Smooth
{
	public AbilityUtil_Targeter_MartyrSmoothAoE(Ability ability, float radius, bool penetrateLoS, bool affectsEnemies = true, bool affectsAllies = false, int maxTargets = -1)
		: base(ability, radius, penetrateLoS, affectsEnemies, affectsAllies, maxTargets)
	{
	}

	protected override float GetRadius(AbilityTarget currentTarget, ActorData targetingActor)
	{
		return (m_ability as MartyrSlowBeam).GetCurrentTargetingRadius();
	}

	protected override bool GetPenetrateLoS()
	{
		return (m_ability as MartyrSlowBeam).GetPenetrateLoS();
	}
}
