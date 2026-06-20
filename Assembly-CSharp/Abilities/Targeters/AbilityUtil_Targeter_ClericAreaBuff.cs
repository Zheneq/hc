public class AbilityUtil_Targeter_ClericAreaBuff : AbilityUtil_Targeter_Shape
{
	private Cleric_SyncComponent m_syncComp;

	public AbilityUtil_Targeter_ClericAreaBuff(
		Ability ability,
		AbilityAreaShape shape,
		bool penetrateLoS,
		DamageOriginType damageOriginType = DamageOriginType.CenterOfShape,
		bool affectsEnemies = true,
		bool affectsAllies = false,
		AffectsActor affectsCaster = AffectsActor.Possible,
		AffectsActor affectsBestTarget = AffectsActor.Possible)
		: base(ability, shape, penetrateLoS, damageOriginType, affectsEnemies, affectsAllies, affectsCaster, affectsBestTarget)
	{
		m_syncComp = ability.GetComponent<Cleric_SyncComponent>();
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (m_syncComp != null && m_syncComp.m_turnsAreaBuffActive == 0)
		{
			base.UpdateTargeting(currentTarget, targetingActor);
		}
	}
}
