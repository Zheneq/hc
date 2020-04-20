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
		if (sparkDash != null)
		{
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnEnemyMod, "ChargeEnemyEffect", sparkDash.m_effectOnTargetEnemy, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnAllyMod, "ChargeAllyEffect", sparkDash.m_effectOnTargetAlly, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnEnemyInBetweenMod, "EffectOnEnemyInBetween", sparkDash.m_effectOnEnemyInBetween, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnAllyInBetweenMod, "EffectOnAllyInBetween", sparkDash.m_effectOnAllyInBetween, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SparkDash sparkDash = base.GetTargetAbilityOnAbilityData(abilityData) as SparkDash;
		bool flag = sparkDash != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_chaseTargetActorMod, "[Chase Target Actor?]", flag, flag && sparkDash.m_chaseTargetActor);
		string str = text;
		AbilityModPropertyEffectInfo effectOnEnemyMod = this.m_effectOnEnemyMod;
		string prefix = "[Effect on Target Enemy]";
		bool showBaseVal = flag;
		StandardEffectInfo baseVal;
		if (flag)
		{
			baseVal = sparkDash.m_effectOnTargetEnemy;
		}
		else
		{
			baseVal = null;
		}
		text = str + base.PropDesc(effectOnEnemyMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyEffectInfo effectOnAllyMod = this.m_effectOnAllyMod;
		string prefix2 = "[Effect on Target Ally]";
		bool showBaseVal2 = flag;
		StandardEffectInfo baseVal2;
		if (flag)
		{
			baseVal2 = sparkDash.m_effectOnTargetAlly;
		}
		else
		{
			baseVal2 = null;
		}
		text = str2 + base.PropDesc(effectOnAllyMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_chooseDestinationMod, "[Choose Destination?]", flag, flag && sparkDash.m_chooseDestination);
		string str3 = text;
		AbilityModPropertyShape chooseDestShapeMod = this.m_chooseDestShapeMod;
		string prefix3 = "[Destination Shape]";
		bool showBaseVal3 = flag;
		AbilityAreaShape baseVal3;
		if (flag)
		{
			baseVal3 = sparkDash.m_chooseDestinationShape;
		}
		else
		{
			baseVal3 = AbilityAreaShape.SingleSquare;
		}
		text = str3 + base.PropDesc(chooseDestShapeMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyBool hitActorsInBetweenMod = this.m_hitActorsInBetweenMod;
		string prefix4 = "[Hit Actors In Between?]";
		bool showBaseVal4 = flag;
		bool baseVal4;
		if (flag)
		{
			baseVal4 = sparkDash.m_hitActorsInBetween;
		}
		else
		{
			baseVal4 = false;
		}
		text = str4 + base.PropDesc(hitActorsInBetweenMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyFloat chargeHitWidthMod = this.m_chargeHitWidthMod;
		string prefix5 = "[Charge Width]";
		bool showBaseVal5 = flag;
		float baseVal5;
		if (flag)
		{
			baseVal5 = sparkDash.m_chargeHitWidth;
		}
		else
		{
			baseVal5 = 0f;
		}
		text = str5 + base.PropDesc(chargeHitWidthMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_effectOnEnemyInBetweenMod, "[Effect on Enemy In Between]", flag, (!flag) ? null : sparkDash.m_effectOnEnemyInBetween);
		return text + base.PropDesc(this.m_effectOnAllyInBetweenMod, "[Effect on Ally In Between]", flag, (!flag) ? null : sparkDash.m_effectOnAllyInBetween);
	}
}
