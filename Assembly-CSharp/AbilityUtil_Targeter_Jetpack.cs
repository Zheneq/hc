using System;

public class AbilityUtil_Targeter_Jetpack : AbilityUtil_Targeter_Shape
{
	public AbilityUtil_Targeter_Jetpack(Ability ability, AbilityAreaShape landingShape, bool penetrateLoS) : base(ability, landingShape, penetrateLoS, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible)
	{
		this.m_showArcToShape = false;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
	}
}
