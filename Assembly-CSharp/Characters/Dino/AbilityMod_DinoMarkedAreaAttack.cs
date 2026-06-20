using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_DinoMarkedAreaAttack : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod")]
	public TargetSelectMod_AoeRadius m_targetSelectMod;
	[Separator("For Delayed Hit")]
	public AbilityModPropertyInt m_delayTurnsMod;
	public AbilityModPropertyShape m_shapeMod;
	public AbilityModPropertyBool m_delayedHitIgnoreLosMod;
	[Space(10f)]
	public AbilityModPropertyInt m_extraDamageForSingleMarkMod;
	public AbilityModPropertyInt m_energyToAllyOnDamageHitMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(DinoMarkedAreaAttack);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		DinoMarkedAreaAttack dinoMarkedAreaAttack = targetAbility as DinoMarkedAreaAttack;
		if (dinoMarkedAreaAttack != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AddToken(tokens, m_delayTurnsMod, "DelayTurns", string.Empty, dinoMarkedAreaAttack.m_delayTurns);
			AddToken(tokens, m_extraDamageForSingleMarkMod, "ExtraDamageForSingleMark", string.Empty, dinoMarkedAreaAttack.m_extraDamageForSingleMark);
			AddToken(tokens, m_energyToAllyOnDamageHitMod, "EnergyToAllyOnDamageHit", string.Empty, dinoMarkedAreaAttack.m_energyToAllyOnDamageHit);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		DinoMarkedAreaAttack dinoMarkedAreaAttack = GetTargetAbilityOnAbilityData(abilityData) as DinoMarkedAreaAttack;
		bool isValid = dinoMarkedAreaAttack != null;
		string desc = base.ModSpecificAutogenDesc(abilityData);
		if (dinoMarkedAreaAttack != null)
		{
			desc += GetTargetSelectModDesc(m_targetSelectMod, dinoMarkedAreaAttack.m_targetSelectComp);
		}
		desc += PropDesc(m_delayTurnsMod, "[DelayTurns]", isValid, isValid ? dinoMarkedAreaAttack.m_delayTurns : 0);
		desc += PropDesc(m_shapeMod, "[Shape]", isValid, isValid ? dinoMarkedAreaAttack.m_shape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_delayedHitIgnoreLosMod, "[DelayedHitIgnoreLos]", isValid, isValid && dinoMarkedAreaAttack.m_delayedHitIgnoreLos);
		desc += PropDesc(m_extraDamageForSingleMarkMod, "[ExtraDamageForSingleMark]", isValid, isValid ? dinoMarkedAreaAttack.m_extraDamageForSingleMark : 0);
		return desc + PropDesc(m_energyToAllyOnDamageHitMod, "[EnergyToAllyOnDamageHit]", isValid, isValid ? dinoMarkedAreaAttack.m_energyToAllyOnDamageHit : 0);
	}
}
