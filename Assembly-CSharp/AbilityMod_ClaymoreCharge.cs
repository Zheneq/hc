using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ClaymoreCharge : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyShape m_aoeShapeMod;

	public AbilityModPropertyFloat m_widthMod;

	public AbilityModPropertyFloat m_maxRangeMod;

	public AbilityModPropertyBool m_directHitIgnoreCoverMod;

	[Header("-- Hit Damage and Effects")]
	public AbilityModPropertyInt m_directHitDamageMod;

	public AbilityModPropertyEffectInfo m_directEnemyHitEffectMod;

	public AbilityModPropertyInt m_aoeDamageMod;

	public AbilityModPropertyEffectInfo m_aoeEnemyHitEffectMod;

	[Header("-- Extra Damage from Charge Path Length")]
	public AbilityModPropertyInt m_extraDirectHitDamagePerSquareMod;

	[Header("-- Heal on Self")]
	public AbilityModPropertyInt m_healOnSelfPerTargetHitMod;

	[Header("-- Cooldown and Chase on Hit")]
	public AbilityModPropertyInt m_cooldownOnHitMod;

	public AbilityModPropertyBool m_chaseHitActorMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ClaymoreCharge);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ClaymoreCharge claymoreCharge = targetAbility as ClaymoreCharge;
		if (claymoreCharge != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ClaymoreCharge.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_widthMod, "ChargeLineWidth", "charge line width", claymoreCharge.m_width, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxRangeMod, "ChargeLineRange", "max charge range", claymoreCharge.m_maxRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_directHitDamageMod, "Damage_DirectHit", "direct hit damage from charge", claymoreCharge.m_directHitDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_directEnemyHitEffectMod, "Effect_DirectHit", claymoreCharge.m_directEnemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_aoeDamageMod, "Damage_AoeHit", "aoe hit damage", claymoreCharge.m_aoeDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_aoeEnemyHitEffectMod, "Effect_AoeHit", claymoreCharge.m_aoeEnemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_extraDirectHitDamagePerSquareMod, "ExtraDirectHitDamagePerSquare", string.Empty, claymoreCharge.m_extraDirectHitDamagePerSquare, true, false);
			AbilityMod.AddToken(tokens, this.m_healOnSelfPerTargetHitMod, "HealOnSelfPerTargetHit", string.Empty, claymoreCharge.m_healOnSelfPerTargetHit, true, false);
			AbilityMod.AddToken(tokens, this.m_cooldownOnHitMod, "CooldownOnHit", "set cooldown to this when hit enemy", claymoreCharge.m_cooldownOnHit, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClaymoreCharge claymoreCharge = base.GetTargetAbilityOnAbilityData(abilityData) as ClaymoreCharge;
		bool flag = claymoreCharge != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_aoeShapeMod, "[Charge Hit Shape]", flag, (!flag) ? AbilityAreaShape.SingleSquare : claymoreCharge.m_aoeShape);
		string str = text;
		AbilityModPropertyFloat widthMod = this.m_widthMod;
		string prefix = "[Charge Line Width]";
		bool showBaseVal = flag;
		float baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ClaymoreCharge.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = claymoreCharge.m_width;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(widthMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_maxRangeMod, "[Charge Line Range]", flag, (!flag) ? 0f : claymoreCharge.m_maxRange);
		text += base.PropDesc(this.m_directHitIgnoreCoverMod, "[DirectHitIgnoreCover]", flag, flag && claymoreCharge.m_directHitIgnoreCover);
		string str2 = text;
		AbilityModPropertyInt directHitDamageMod = this.m_directHitDamageMod;
		string prefix2 = "[Direct Hit Damage]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
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
			baseVal2 = claymoreCharge.m_directHitDamage;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(directHitDamageMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyEffectInfo directEnemyHitEffectMod = this.m_directEnemyHitEffectMod;
		string prefix3 = "[Direct Hit Effect]";
		bool showBaseVal3 = flag;
		StandardEffectInfo baseVal3;
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
			baseVal3 = claymoreCharge.m_directEnemyHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		text = str3 + base.PropDesc(directEnemyHitEffectMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt aoeDamageMod = this.m_aoeDamageMod;
		string prefix4 = "[AOE Hit Damage]";
		bool showBaseVal4 = flag;
		int baseVal4;
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
			baseVal4 = claymoreCharge.m_aoeDamage;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(aoeDamageMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyEffectInfo aoeEnemyHitEffectMod = this.m_aoeEnemyHitEffectMod;
		string prefix5 = "[AOE Hit Effect]";
		bool showBaseVal5 = flag;
		StandardEffectInfo baseVal5;
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
			baseVal5 = claymoreCharge.m_aoeEnemyHitEffect;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + base.PropDesc(aoeEnemyHitEffectMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt extraDirectHitDamagePerSquareMod = this.m_extraDirectHitDamagePerSquareMod;
		string prefix6 = "[ExtraDirectHitDamagePerSquare]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
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
			baseVal6 = claymoreCharge.m_extraDirectHitDamagePerSquare;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(extraDirectHitDamagePerSquareMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyInt healOnSelfPerTargetHitMod = this.m_healOnSelfPerTargetHitMod;
		string prefix7 = "[HealOnSelfPerTargetHit]";
		bool showBaseVal7 = flag;
		int baseVal7;
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
			baseVal7 = claymoreCharge.m_healOnSelfPerTargetHit;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(healOnSelfPerTargetHitMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyInt cooldownOnHitMod = this.m_cooldownOnHitMod;
		string prefix8 = "[Cooldown Override (on charge ability) on Hit]";
		bool showBaseVal8 = flag;
		int baseVal8;
		if (flag)
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
			baseVal8 = claymoreCharge.m_cooldownOnHit;
		}
		else
		{
			baseVal8 = 0;
		}
		text = str8 + base.PropDesc(cooldownOnHitMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyBool chaseHitActorMod = this.m_chaseHitActorMod;
		string prefix9 = "[Chase Hit Target?]";
		bool showBaseVal9 = flag;
		bool baseVal9;
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
			baseVal9 = claymoreCharge.m_chaseHitActor;
		}
		else
		{
			baseVal9 = false;
		}
		return str9 + base.PropDesc(chaseHitActorMod, prefix9, showBaseVal9, baseVal9);
	}
}
