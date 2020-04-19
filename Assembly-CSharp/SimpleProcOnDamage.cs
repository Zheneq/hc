using System;

public class SimpleProcOnDamage : Ability
{
	public StandardActorEffectData m_effectToProc;

	public int m_duration = 1;

	private void Start()
	{
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Always, AbilityUtil_Targeter.AffectsActor.Possible);
	}
}
