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
		bool affectsAllies = m_stealthGeneratorInfo.IncludeAllies();
		bool affectsEnemies = m_stealthGeneratorInfo.IncludeEnemies();
		AbilityAreaShape shape = m_stealthGeneratorInfo.shape;
		bool penetrateLos = m_stealthGeneratorInfo.penetrateLos;
		int affectsCaster;
		if (m_stealthGeneratorInfo.canIncludeCaster)
		{
			affectsCaster = 1;
		}
		else
		{
			affectsCaster = 0;
		}
		base.Targeter = new AbilityUtil_Targeter_Shape(this, shape, penetrateLos, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, affectsEnemies, affectsAllies, (AbilityUtil_Targeter.AffectsActor)affectsCaster);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_stealthGeneratorInfo.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally);
		return numbers;
	}
}
