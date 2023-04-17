using System.Collections.Generic;

public class ThiefStealthGenerator : Ability
{
	public GroundEffectField m_stealthGeneratorInfo;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Stealth Generator";
		}
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			m_stealthGeneratorInfo.shape,
			m_stealthGeneratorInfo.penetrateLos,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			m_stealthGeneratorInfo.IncludeEnemies(),
			m_stealthGeneratorInfo.IncludeAllies(),
			m_stealthGeneratorInfo.canIncludeCaster
				? AbilityUtil_Targeter.AffectsActor.Possible
				: AbilityUtil_Targeter.AffectsActor.Never);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_stealthGeneratorInfo.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally);
		return numbers;
	}
}
