using System;
using System.Collections.Generic;
using System.Text;
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
			AddToken_EffectMod(tokens, m_effectOnEnemyMod, "ChargeEnemyEffect", sparkDash.m_effectOnTargetEnemy);
			AddToken_EffectMod(tokens, m_effectOnAllyMod, "ChargeAllyEffect", sparkDash.m_effectOnTargetAlly);
			AddToken_EffectMod(tokens, m_effectOnEnemyInBetweenMod, "EffectOnEnemyInBetween", sparkDash.m_effectOnEnemyInBetween);
			AddToken_EffectMod(tokens, m_effectOnAllyInBetweenMod, "EffectOnAllyInBetween", sparkDash.m_effectOnAllyInBetween);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SparkDash sparkDash = GetTargetAbilityOnAbilityData(abilityData) as SparkDash;
		bool isAbilityPresent = sparkDash != null;
		string desc = "";
		desc += PropDesc(m_chaseTargetActorMod, "[Chase Target Actor?]", isAbilityPresent, isAbilityPresent && sparkDash.m_chaseTargetActor);
		desc += PropDesc(m_effectOnEnemyMod, "[Effect on Target Enemy]", isAbilityPresent, isAbilityPresent ? sparkDash.m_effectOnTargetEnemy : null);
		desc += PropDesc(m_effectOnAllyMod, "[Effect on Target Ally]", isAbilityPresent, isAbilityPresent ? sparkDash.m_effectOnTargetAlly : null);
		desc += PropDesc(m_chooseDestinationMod, "[Choose Destination?]", isAbilityPresent, isAbilityPresent && sparkDash.m_chooseDestination);
		desc += PropDesc(m_chooseDestShapeMod, "[Destination Shape]", isAbilityPresent, isAbilityPresent ? sparkDash.m_chooseDestinationShape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_hitActorsInBetweenMod, "[Hit Actors In Between?]", isAbilityPresent, isAbilityPresent && sparkDash.m_hitActorsInBetween);
		desc += PropDesc(m_chargeHitWidthMod, "[Charge Width]", isAbilityPresent, isAbilityPresent ? sparkDash.m_chargeHitWidth : 0f);
		desc += PropDesc(m_effectOnEnemyInBetweenMod, "[Effect on Enemy In Between]", isAbilityPresent, isAbilityPresent ? sparkDash.m_effectOnEnemyInBetween : null);
		return new StringBuilder().Append(desc).Append(PropDesc(m_effectOnAllyInBetweenMod, "[Effect on Ally In Between]", isAbilityPresent, isAbilityPresent ? sparkDash.m_effectOnAllyInBetween : null)).ToString();
	}
}
