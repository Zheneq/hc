using System;
using System.Collections.Generic;

public class ThiefStealthGenerator : Ability
{
	public GroundEffectField m_stealthGeneratorInfo;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Stealth Generator";
		}
		bool flag = this.m_stealthGeneratorInfo.IncludeAllies();
		bool flag2 = this.m_stealthGeneratorInfo.IncludeEnemies();
		AbilityAreaShape shape = this.m_stealthGeneratorInfo.shape;
		bool penetrateLos = this.m_stealthGeneratorInfo.penetrateLos;
		AbilityUtil_Targeter_Shape.DamageOriginType damageOriginType = AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape;
		bool affectsEnemies = flag2;
		bool affectsAllies = flag;
		AbilityUtil_Targeter.AffectsActor affectsCaster;
		if (this.m_stealthGeneratorInfo.canIncludeCaster)
		{
			affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible;
		}
		else
		{
			affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
		}
		base.Targeter = new AbilityUtil_Targeter_Shape(this, shape, penetrateLos, damageOriginType, affectsEnemies, affectsAllies, affectsCaster, AbilityUtil_Targeter.AffectsActor.Possible);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.m_stealthGeneratorInfo.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally);
		return result;
	}
}
