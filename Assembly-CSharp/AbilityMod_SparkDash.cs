using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SparkDash : AbilityMod
{
	[Header("-- On Charge Target Hit")]
	public AbilityModPropertyBool m_chaseTargetActorMod;

	public AbilityModPropertyEffectInfo m_effectOnEnemyMod;

	public AbilityModPropertyEffectInfo m_effectOnAllyMod;

	[Header("-- Targeting: choosing destination")]
	public AbilityModPropertyBool m_chooseDestinationMod;

	public AbilityModPropertyShape m_chooseDestShapeMod;

	[Header("-- Targeting: hit actors in between")]
	public AbilityModPropertyBool m_hitActorsInBetweenMod;

	public AbilityModPropertyFloat m_chargeHitWidthMod;

	public AbilityModPropertyEffectInfo m_effectOnEnemyInBetweenMod;

	public AbilityModPropertyEffectInfo m_effectOnAllyInBetweenMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SparkDash);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SparkDash sparkDash = targetAbility as SparkDash;
		if (!(sparkDash != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnEnemyMod, "ChargeEnemyEffect", sparkDash.m_effectOnTargetEnemy);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnAllyMod, "ChargeAllyEffect", sparkDash.m_effectOnTargetAlly);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnEnemyInBetweenMod, "EffectOnEnemyInBetween", sparkDash.m_effectOnEnemyInBetween);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnAllyInBetweenMod, "EffectOnAllyInBetween", sparkDash.m_effectOnAllyInBetween);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SparkDash sparkDash = GetTargetAbilityOnAbilityData(abilityData) as SparkDash;
		bool flag = sparkDash != null;
		string empty = string.Empty;
		empty += PropDesc(m_chaseTargetActorMod, "[Chase Target Actor?]", flag, flag && sparkDash.m_chaseTargetActor);
		string str = empty;
		AbilityModPropertyEffectInfo effectOnEnemyMod = m_effectOnEnemyMod;
		object baseVal;
		if (flag)
		{
			baseVal = sparkDash.m_effectOnTargetEnemy;
		}
		else
		{
			baseVal = null;
		}
		empty = str + PropDesc(effectOnEnemyMod, "[Effect on Target Enemy]", flag, (StandardEffectInfo)baseVal);
		string str2 = empty;
		AbilityModPropertyEffectInfo effectOnAllyMod = m_effectOnAllyMod;
		object baseVal2;
		if (flag)
		{
			baseVal2 = sparkDash.m_effectOnTargetAlly;
		}
		else
		{
			baseVal2 = null;
		}
		empty = str2 + PropDesc(effectOnAllyMod, "[Effect on Target Ally]", flag, (StandardEffectInfo)baseVal2);
		empty += PropDesc(m_chooseDestinationMod, "[Choose Destination?]", flag, flag && sparkDash.m_chooseDestination);
		string str3 = empty;
		AbilityModPropertyShape chooseDestShapeMod = m_chooseDestShapeMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = (int)sparkDash.m_chooseDestinationShape;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(chooseDestShapeMod, "[Destination Shape]", flag, (AbilityAreaShape)baseVal3);
		string str4 = empty;
		AbilityModPropertyBool hitActorsInBetweenMod = m_hitActorsInBetweenMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = (sparkDash.m_hitActorsInBetween ? 1 : 0);
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(hitActorsInBetweenMod, "[Hit Actors In Between?]", flag, (byte)baseVal4 != 0);
		string str5 = empty;
		AbilityModPropertyFloat chargeHitWidthMod = m_chargeHitWidthMod;
		float baseVal5;
		if (flag)
		{
			baseVal5 = sparkDash.m_chargeHitWidth;
		}
		else
		{
			baseVal5 = 0f;
		}
		empty = str5 + PropDesc(chargeHitWidthMod, "[Charge Width]", flag, baseVal5);
		empty += PropDesc(m_effectOnEnemyInBetweenMod, "[Effect on Enemy In Between]", flag, (!flag) ? null : sparkDash.m_effectOnEnemyInBetween);
		return empty + PropDesc(m_effectOnAllyInBetweenMod, "[Effect on Ally In Between]", flag, (!flag) ? null : sparkDash.m_effectOnAllyInBetween);
	}
}
