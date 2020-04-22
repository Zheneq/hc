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
		if (!(gremlinsBigBang != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_knockbackDistanceMod, "KnockbackDist", string.Empty, gremlinsBigBang.m_knockbackDistance);
			AbilityMod.AddToken(tokens, m_knockbackDistanceIfSingleTarget, "KnockbackDist_SingleTarget", string.Empty, gremlinsBigBang.m_knockbackDistance);
			AbilityMod.AddToken(tokens, m_extraDamagePerTarget, "Damage_ExtraPerTarget", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_damageIfSingleTarget, "Damage_SingleTarget", string.Empty, gremlinsBigBang.m_bombDamageAmount);
			GremlinsLandMineInfoComponent component = gremlinsBigBang.GetComponent<GremlinsLandMineInfoComponent>();
			if (component != null)
			{
				AbilityMod.AddToken(tokens, m_mineDamageMod, "Damage", string.Empty, component.m_damageAmount);
				AbilityMod.AddToken(tokens, m_mineDurationMod, "MineDuration", string.Empty, component.m_mineDuration);
				AbilityMod.AddToken_EffectMod(tokens, m_effectOnEnemyOverride, "EffectOnEnemyHit", component.m_enemyHitEffect);
				AbilityMod.AddToken(tokens, m_energyOnMineExplosionMod, "EnergyOnExplosion", string.Empty, component.m_energyGainOnExplosion);
			}
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		GremlinsBigBang gremlinsBigBang = GetTargetAbilityOnAbilityData(abilityData) as GremlinsBigBang;
		object obj;
		if (gremlinsBigBang != null)
		{
			obj = gremlinsBigBang.GetComponent<GremlinsLandMineInfoComponent>();
		}
		else
		{
			obj = null;
		}
		GremlinsLandMineInfoComponent gremlinsLandMineInfoComponent = (GremlinsLandMineInfoComponent)obj;
		bool flag = gremlinsLandMineInfoComponent != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyShape bombShape = m_bombShape;
		int baseVal;
		if (flag)
		{
			baseVal = (int)gremlinsBigBang.m_bombShape;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(bombShape, "[Bomb Shape]", flag, (AbilityAreaShape)baseVal);
		empty += AbilityModHelper.GetModPropertyDesc(m_knockbackShape, "[Knockback Shape]", flag, flag ? gremlinsBigBang.m_knockbackShape : AbilityAreaShape.SingleSquare);
		empty += PropDesc(m_knockbackDistanceMod, "[KnockbackDistance]", flag, (!flag) ? 0f : gremlinsBigBang.m_knockbackDistance);
		empty += AbilityModHelper.GetModPropertyDesc(m_extraDamagePerTarget, "[Extra Damage Per Target]", flag);
		string str2 = empty;
		AbilityModPropertyInt damageIfSingleTarget = m_damageIfSingleTarget;
		int baseVal2;
		if (flag)
		{
			baseVal2 = gremlinsBigBang.m_bombDamageAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(damageIfSingleTarget, "[Damage If Single Target]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat knockbackDistanceIfSingleTarget = m_knockbackDistanceIfSingleTarget;
		float baseVal3;
		if (flag)
		{
			baseVal3 = gremlinsBigBang.m_knockbackDistance;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(knockbackDistanceIfSingleTarget, "[Knockback Distance If Single Target]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyInt mineDamageMod = m_mineDamageMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = gremlinsLandMineInfoComponent.m_damageAmount;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(mineDamageMod, "[Mine Damage]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyInt mineDurationMod = m_mineDurationMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = gremlinsLandMineInfoComponent.m_mineDuration;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + AbilityModHelper.GetModPropertyDesc(mineDurationMod, "[Mine Duration]", flag, baseVal5);
		empty += AbilityModHelper.GetModPropertyDesc(m_effectOnEnemyOverride, "{ Effect on Enemy Hit Override }", flag, (!flag) ? null : gremlinsLandMineInfoComponent.m_enemyHitEffect);
		string str6 = empty;
		AbilityModPropertyInt energyOnMineExplosionMod = m_energyOnMineExplosionMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = gremlinsLandMineInfoComponent.m_energyGainOnExplosion;
		}
		else
		{
			baseVal6 = 0;
		}
		return str6 + AbilityModHelper.GetModPropertyDesc(energyOnMineExplosionMod, "[Energy Gain on Mine Explosion (on splort and mines left behind from primary/ult)]", flag, baseVal6);
	}
}
