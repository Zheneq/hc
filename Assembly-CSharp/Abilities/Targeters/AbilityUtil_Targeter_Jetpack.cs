public class AbilityUtil_Targeter_Jetpack : AbilityUtil_Targeter_Shape
{
	public AbilityUtil_Targeter_Jetpack(Ability ability, AbilityAreaShape landingShape, bool penetrateLoS)
		: base(ability, landingShape, penetrateLoS)
	{
		m_showArcToShape = false;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
	}
}
