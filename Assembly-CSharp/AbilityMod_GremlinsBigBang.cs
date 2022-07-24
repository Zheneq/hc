using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_GremlinsBigBang : AbilityMod
{
	[Header("-- GremlinsBigBang Properties -----------------------------------")]
	public AbilityModPropertyShape m_bombShape;
	public AbilityModPropertyShape m_knockbackShape;
	public AbilityModPropertyFloat m_knockbackDistanceMod;
	public AbilityModPropertyInt m_extraDamagePerTarget;
	public AbilityModPropertyInt m_damageIfSingleTarget;
	public AbilityModPropertyFloat m_knockbackDistanceIfSingleTarget;
	[Space(10f)]
	[Header("-- Global Mine Data Mods")]
	public AbilityModPropertyInt m_mineDamageMod;
	public AbilityModPropertyInt m_mineDurationMod;
	public AbilityModPropertyEffectInfo m_effectOnEnemyOverride;
	public AbilityModPropertyInt m_energyOnMineExplosionMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(GremlinsBigBang);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		GremlinsBigBang gremlinsBigBang = targetAbility as GremlinsBigBang;
		if (gremlinsBigBang != null)
		{
			AddToken(tokens, m_knockbackDistanceMod, "KnockbackDist", string.Empty, gremlinsBigBang.m_knockbackDistance);
			AddToken(tokens, m_knockbackDistanceIfSingleTarget, "KnockbackDist_SingleTarget", string.Empty, gremlinsBigBang.m_knockbackDistance);
			AddToken(tokens, m_extraDamagePerTarget, "Damage_ExtraPerTarget", string.Empty, 0);
			AddToken(tokens, m_damageIfSingleTarget, "Damage_SingleTarget", string.Empty, gremlinsBigBang.m_bombDamageAmount);
			GremlinsLandMineInfoComponent component = gremlinsBigBang.GetComponent<GremlinsLandMineInfoComponent>();
			if (component != null)
			{
				AddToken(tokens, m_mineDamageMod, "Damage", string.Empty, component.m_damageAmount);
				AddToken(tokens, m_mineDurationMod, "MineDuration", string.Empty, component.m_mineDuration);
				AddToken_EffectMod(tokens, m_effectOnEnemyOverride, "EffectOnEnemyHit", component.m_enemyHitEffect);
				AddToken(tokens, m_energyOnMineExplosionMod, "EnergyOnExplosion", string.Empty, component.m_energyGainOnExplosion);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		GremlinsBigBang gremlinsBigBang = GetTargetAbilityOnAbilityData(abilityData) as GremlinsBigBang;
		GremlinsLandMineInfoComponent gremlinsLandMineInfoComponent = gremlinsBigBang != null
			? gremlinsBigBang.GetComponent<GremlinsLandMineInfoComponent>()
			: null;
		bool isAbilityPresent = gremlinsLandMineInfoComponent != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_bombShape, "[Bomb Shape]", isAbilityPresent, isAbilityPresent ? gremlinsBigBang.m_bombShape : AbilityAreaShape.SingleSquare);
		desc += AbilityModHelper.GetModPropertyDesc(m_knockbackShape, "[Knockback Shape]", isAbilityPresent, isAbilityPresent ? gremlinsBigBang.m_knockbackShape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_knockbackDistanceMod, "[KnockbackDistance]", isAbilityPresent, isAbilityPresent ? gremlinsBigBang.m_knockbackDistance : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_extraDamagePerTarget, "[Extra Damage Per Target]", isAbilityPresent);
		desc += AbilityModHelper.GetModPropertyDesc(m_damageIfSingleTarget, "[Damage If Single Target]", isAbilityPresent, isAbilityPresent ? gremlinsBigBang.m_bombDamageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_knockbackDistanceIfSingleTarget, "[Knockback Distance If Single Target]", isAbilityPresent, isAbilityPresent ? gremlinsBigBang.m_knockbackDistance : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_mineDamageMod, "[Mine Damage]", isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_damageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_mineDurationMod, "[Mine Duration]", isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_mineDuration : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_effectOnEnemyOverride, "{ Effect on Enemy Hit Override }", isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_enemyHitEffect : null);
		return desc + AbilityModHelper.GetModPropertyDesc(m_energyOnMineExplosionMod, "[Energy Gain on Mine Explosion (on splort and mines left behind from primary/ult)]", isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_energyGainOnExplosion : 0);
	}
}
