using System;

public class AbilityUtil_Targeter_ClericAreaBuff : AbilityUtil_Targeter_Shape
{
	private Cleric_SyncComponent m_syncComp;

	public AbilityUtil_Targeter_ClericAreaBuff(Ability ability, AbilityAreaShape shape, bool penetrateLoS, AbilityUtil_Targeter_Shape.DamageOriginType damageOriginType = AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, bool affectsEnemies = true, bool affectsAllies = false, AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor affectsBestTarget = AbilityUtil_Targeter.AffectsActor.Possible) : base(ability, shape, penetrateLoS, damageOriginType, affectsEnemies, affectsAllies, affectsCaster, affectsBestTarget)
	{
		this.m_syncComp = ability.GetComponent<Cleric_SyncComponent>();
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (this.m_syncComp != null && this.m_syncComp.m_turnsAreaBuffActive == 0)
		{
			base.UpdateTargeting(currentTarget, targetingActor);
		}
	}
}
