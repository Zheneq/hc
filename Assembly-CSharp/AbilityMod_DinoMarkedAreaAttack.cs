using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_DinoMarkedAreaAttack : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod", true)]
	public TargetSelectMod_AoeRadius m_targetSelectMod;

	[Separator("For Delayed Hit", true)]
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
			AbilityMod.AddToken(tokens, m_delayTurnsMod, "DelayTurns", string.Empty, dinoMarkedAreaAttack.m_delayTurns);
			AbilityMod.AddToken(tokens, m_extraDamageForSingleMarkMod, "ExtraDamageForSingleMark", string.Empty, dinoMarkedAreaAttack.m_extraDamageForSingleMark);
			AbilityMod.AddToken(tokens, m_energyToAllyOnDamageHitMod, "EnergyToAllyOnDamageHit", string.Empty, dinoMarkedAreaAttack.m_energyToAllyOnDamageHit);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		DinoMarkedAreaAttack dinoMarkedAreaAttack = GetTargetAbilityOnAbilityData(abilityData) as DinoMarkedAreaAttack;
		bool flag = dinoMarkedAreaAttack != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (dinoMarkedAreaAttack != null)
		{
			text += GetTargetSelectModDesc(m_targetSelectMod, dinoMarkedAreaAttack.m_targetSelectComp);
		}
		string str = text;
		AbilityModPropertyInt delayTurnsMod = m_delayTurnsMod;
		int baseVal;
		if (flag)
		{
			baseVal = dinoMarkedAreaAttack.m_delayTurns;
		}
		else
		{
			baseVal = 0;
		}
		text = str + PropDesc(delayTurnsMod, "[DelayTurns]", flag, baseVal);
		string str2 = text;
		AbilityModPropertyShape shapeMod = m_shapeMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = (int)dinoMarkedAreaAttack.m_shape;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + PropDesc(shapeMod, "[Shape]", flag, (AbilityAreaShape)baseVal2);
		string str3 = text;
		AbilityModPropertyBool delayedHitIgnoreLosMod = m_delayedHitIgnoreLosMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = (dinoMarkedAreaAttack.m_delayedHitIgnoreLos ? 1 : 0);
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + PropDesc(delayedHitIgnoreLosMod, "[DelayedHitIgnoreLos]", flag, (byte)baseVal3 != 0);
		string str4 = text;
		AbilityModPropertyInt extraDamageForSingleMarkMod = m_extraDamageForSingleMarkMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = dinoMarkedAreaAttack.m_extraDamageForSingleMark;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + PropDesc(extraDamageForSingleMarkMod, "[ExtraDamageForSingleMark]", flag, baseVal4);
		string str5 = text;
		AbilityModPropertyInt energyToAllyOnDamageHitMod = m_energyToAllyOnDamageHitMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = dinoMarkedAreaAttack.m_energyToAllyOnDamageHit;
		}
		else
		{
			baseVal5 = 0;
		}
		return str5 + PropDesc(energyToAllyOnDamageHitMod, "[EnergyToAllyOnDamageHit]", flag, baseVal5);
	}
}
