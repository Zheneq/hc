using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_RageBeastCharge : AbilityMod
{
	[Header("-- Damage Mod")]
	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyInt m_damageNearChargeEndMod;

	[Header("-- On Hit Effect")]
	public StandardEffectInfo m_effectOnTargetsHit;

	public AbilityModPropertyEffectInfo m_enemyHitEffectNearChargeEndMod;

	[Header("-- Targeting Mod")]
	public AbilityModPropertyFloat m_chargeLineRadiusMod;

	public AbilityModPropertyFloat m_chargeEndRadius;

	public override Type GetTargetAbilityType()
	{
		return typeof(RageBeastCharge);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		RageBeastCharge rageBeastCharge = targetAbility as RageBeastCharge;
		if (rageBeastCharge != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_RageBeastCharge.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_damageMod, "DamageAmount", string.Empty, rageBeastCharge.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_damageNearChargeEndMod, "DamageNearChargeEnd", string.Empty, rageBeastCharge.m_damageNearChargeEnd, true, false);
			AbilityMod.AddToken(tokens, this.m_chargeLineRadiusMod, "DamageRadius", string.Empty, rageBeastCharge.m_damageRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_chargeEndRadius, "RadiusBeyondEnd", string.Empty, rageBeastCharge.m_radiusBeyondEnd, true, false, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectNearChargeEndMod, "EnemyHitEffectNearChargeEnd", rageBeastCharge.m_enemyHitEffectNearChargeEnd, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RageBeastCharge rageBeastCharge = base.GetTargetAbilityOnAbilityData(abilityData) as RageBeastCharge;
		bool flag = rageBeastCharge != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix = "[Damage Mod]";
		bool showBaseVal = flag;
		int baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_RageBeastCharge.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = rageBeastCharge.m_damageAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(damageMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt damageNearChargeEndMod = this.m_damageNearChargeEndMod;
		string prefix2 = "[Damage Near Charge End Mod]";
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
			baseVal2 = rageBeastCharge.m_damageNearChargeEnd;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(damageNearChargeEndMod, prefix2, showBaseVal2, baseVal2);
		text += AbilityModHelper.GetModPropertyDesc(this.m_chargeLineRadiusMod, "[Charge Line Radius/Half-Width Mod]", flag, (!flag) ? 0f : rageBeastCharge.m_damageRadius);
		text += AbilityModHelper.GetModPropertyDesc(this.m_chargeEndRadius, "[Charge End Radius Mod]", flag, (!flag) ? 0f : rageBeastCharge.m_radiusBeyondEnd);
		text += AbilityModHelper.GetModEffectInfoDesc(this.m_effectOnTargetsHit, "{ Effect on Target Hit }", string.Empty, flag, null);
		string str3 = text;
		AbilityModPropertyEffectInfo enemyHitEffectNearChargeEndMod = this.m_enemyHitEffectNearChargeEndMod;
		string prefix3 = "[EnemyHitEffectNearChargeEnd]";
		bool showBaseVal3 = flag;
		StandardEffectInfo baseVal3;
		if (flag)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal3 = rageBeastCharge.m_enemyHitEffectNearChargeEnd;
		}
		else
		{
			baseVal3 = null;
		}
		return str3 + base.PropDesc(enemyHitEffectNearChargeEndMod, prefix3, showBaseVal3, baseVal3);
	}
}
