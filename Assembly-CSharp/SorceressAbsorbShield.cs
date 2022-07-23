using System.Collections.Generic;

public class SorceressAbsorbShield : Ability
{
	public int m_duration = 3;
	public int m_absorbAmount = 50;
	public AbilityAreaShape m_shape;

	private void Start()
	{
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			m_shape,
			true,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			false, 
			true);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Primary, m_absorbAmount)
		};
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return CanTargetActorInDecision(
			caster,
			currentBestActorTarget,
			false,
			true, 
			true,
			ValidateCheckPath.Ignore,
			true,
			true);
	}
}
