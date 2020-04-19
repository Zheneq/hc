using System;
using System.Collections.Generic;

public class SorceressAbsorbShield : Ability
{
	public int m_duration = 3;

	public int m_absorbAmount = 0x32;

	public AbilityAreaShape m_shape;

	private void Start()
	{
		base.Targeter = new AbilityUtil_Targeter_Shape(this, this.m_shape, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, true, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Primary, this.m_absorbAmount)
		};
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return base.CanTargetActorInDecision(caster, currentBestActorTarget, false, true, true, Ability.ValidateCheckPath.Ignore, true, true, false);
	}
}
