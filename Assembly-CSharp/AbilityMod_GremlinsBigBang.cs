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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_GremlinsBigBang.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_knockbackDistanceMod, "KnockbackDist", string.Empty, gremlinsBigBang.m_knockbackDistance, true, false, false);
			AbilityMod.AddToken(tokens, this.m_knockbackDistanceIfSingleTarget, "KnockbackDist_SingleTarget", string.Empty, gremlinsBigBang.m_knockbackDistance, true, false, false);
			AbilityMod.AddToken(tokens, this.m_extraDamagePerTarget, "Damage_ExtraPerTarget", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_damageIfSingleTarget, "Damage_SingleTarget", string.Empty, gremlinsBigBang.m_bombDamageAmount, true, false);
			GremlinsLandMineInfoComponent component = gremlinsBigBang.GetComponent<GremlinsLandMineInfoComponent>();
			if (component != null)
			{
				AbilityMod.AddToken(tokens, this.m_mineDamageMod, "Damage", string.Empty, component.m_damageAmount, true, false);
				AbilityMod.AddToken(tokens, this.m_mineDurationMod, "MineDuration", string.Empty, component.m_mineDuration, true, false);
				AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnEnemyOverride, "EffectOnEnemyHit", component.m_enemyHitEffect, true);
				AbilityMod.AddToken(tokens, this.m_energyOnMineExplosionMod, "EnergyOnExplosion", string.Empty, component.m_energyGainOnExplosion, true, false);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		GremlinsBigBang gremlinsBigBang = base.GetTargetAbilityOnAbilityData(abilityData) as GremlinsBigBang;
		GremlinsLandMineInfoComponent gremlinsLandMineInfoComponent;
		if (gremlinsBigBang != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_GremlinsBigBang.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			gremlinsLandMineInfoComponent = gremlinsBigBang.GetComponent<GremlinsLandMineInfoComponent>();
		}
		else
		{
			gremlinsLandMineInfoComponent = null;
		}
		GremlinsLandMineInfoComponent gremlinsLandMineInfoComponent2 = gremlinsLandMineInfoComponent;
		bool flag = gremlinsLandMineInfoComponent2 != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyShape bombShape = this.m_bombShape;
		string prefix = "[Bomb Shape]";
		bool showBaseVal = flag;
		AbilityAreaShape baseVal;
		if (flag)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal = gremlinsBigBang.m_bombShape;
		}
		else
		{
			baseVal = AbilityAreaShape.SingleSquare;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(bombShape, prefix, showBaseVal, baseVal);
		text += AbilityModHelper.GetModPropertyDesc(this.m_knockbackShape, "[Knockback Shape]", flag, (!flag) ? AbilityAreaShape.SingleSquare : gremlinsBigBang.m_knockbackShape);
		text += base.PropDesc(this.m_knockbackDistanceMod, "[KnockbackDistance]", flag, (!flag) ? 0f : gremlinsBigBang.m_knockbackDistance);
		text += AbilityModHelper.GetModPropertyDesc(this.m_extraDamagePerTarget, "[Extra Damage Per Target]", flag, 0);
		string str2 = text;
		AbilityModPropertyInt damageIfSingleTarget = this.m_damageIfSingleTarget;
		string prefix2 = "[Damage If Single Target]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = gremlinsBigBang.m_bombDamageAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(damageIfSingleTarget, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat knockbackDistanceIfSingleTarget = this.m_knockbackDistanceIfSingleTarget;
		string prefix3 = "[Knockback Distance If Single Target]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal3 = gremlinsBigBang.m_knockbackDistance;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(knockbackDistanceIfSingleTarget, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt mineDamageMod = this.m_mineDamageMod;
		string prefix4 = "[Mine Damage]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal4 = gremlinsLandMineInfoComponent2.m_damageAmount;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(mineDamageMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt mineDurationMod = this.m_mineDurationMod;
		string prefix5 = "[Mine Duration]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal5 = gremlinsLandMineInfoComponent2.m_mineDuration;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + AbilityModHelper.GetModPropertyDesc(mineDurationMod, prefix5, showBaseVal5, baseVal5);
		text += AbilityModHelper.GetModPropertyDesc(this.m_effectOnEnemyOverride, "{ Effect on Enemy Hit Override }", flag, (!flag) ? null : gremlinsLandMineInfoComponent2.m_enemyHitEffect);
		string str6 = text;
		AbilityModPropertyInt energyOnMineExplosionMod = this.m_energyOnMineExplosionMod;
		string prefix6 = "[Energy Gain on Mine Explosion (on splort and mines left behind from primary/ult)]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal6 = gremlinsLandMineInfoComponent2.m_energyGainOnExplosion;
		}
		else
		{
			baseVal6 = 0;
		}
		return str6 + AbilityModHelper.GetModPropertyDesc(energyOnMineExplosionMod, prefix6, showBaseVal6, baseVal6);
	}
}
