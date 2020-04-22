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
		if (!(claymoreCharge != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_widthMod, "ChargeLineWidth", "charge line width", claymoreCharge.m_width);
			AbilityMod.AddToken(tokens, m_maxRangeMod, "ChargeLineRange", "max charge range", claymoreCharge.m_maxRange);
			AbilityMod.AddToken(tokens, m_directHitDamageMod, "Damage_DirectHit", "direct hit damage from charge", claymoreCharge.m_directHitDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_directEnemyHitEffectMod, "Effect_DirectHit", claymoreCharge.m_directEnemyHitEffect);
			AbilityMod.AddToken(tokens, m_aoeDamageMod, "Damage_AoeHit", "aoe hit damage", claymoreCharge.m_aoeDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_aoeEnemyHitEffectMod, "Effect_AoeHit", claymoreCharge.m_aoeEnemyHitEffect);
			AbilityMod.AddToken(tokens, m_extraDirectHitDamagePerSquareMod, "ExtraDirectHitDamagePerSquare", string.Empty, claymoreCharge.m_extraDirectHitDamagePerSquare);
			AbilityMod.AddToken(tokens, m_healOnSelfPerTargetHitMod, "HealOnSelfPerTargetHit", string.Empty, claymoreCharge.m_healOnSelfPerTargetHit);
			AbilityMod.AddToken(tokens, m_cooldownOnHitMod, "CooldownOnHit", "set cooldown to this when hit enemy", claymoreCharge.m_cooldownOnHit);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClaymoreCharge claymoreCharge = GetTargetAbilityOnAbilityData(abilityData) as ClaymoreCharge;
		bool flag = claymoreCharge != null;
		string empty = string.Empty;
		empty += PropDesc(m_aoeShapeMod, "[Charge Hit Shape]", flag, flag ? claymoreCharge.m_aoeShape : AbilityAreaShape.SingleSquare);
		string str = empty;
		AbilityModPropertyFloat widthMod = m_widthMod;
		float baseVal;
		if (flag)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = claymoreCharge.m_width;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(widthMod, "[Charge Line Width]", flag, baseVal);
		empty += PropDesc(m_maxRangeMod, "[Charge Line Range]", flag, (!flag) ? 0f : claymoreCharge.m_maxRange);
		empty += PropDesc(m_directHitIgnoreCoverMod, "[DirectHitIgnoreCover]", flag, flag && claymoreCharge.m_directHitIgnoreCover);
		string str2 = empty;
		AbilityModPropertyInt directHitDamageMod = m_directHitDamageMod;
		int baseVal2;
		if (flag)
		{
			while (true)
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
		empty = str2 + PropDesc(directHitDamageMod, "[Direct Hit Damage]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyEffectInfo directEnemyHitEffectMod = m_directEnemyHitEffectMod;
		object baseVal3;
		if (flag)
		{
			while (true)
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
		empty = str3 + PropDesc(directEnemyHitEffectMod, "[Direct Hit Effect]", flag, (StandardEffectInfo)baseVal3);
		string str4 = empty;
		AbilityModPropertyInt aoeDamageMod = m_aoeDamageMod;
		int baseVal4;
		if (flag)
		{
			while (true)
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
		empty = str4 + PropDesc(aoeDamageMod, "[AOE Hit Damage]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyEffectInfo aoeEnemyHitEffectMod = m_aoeEnemyHitEffectMod;
		object baseVal5;
		if (flag)
		{
			while (true)
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
		empty = str5 + PropDesc(aoeEnemyHitEffectMod, "[AOE Hit Effect]", flag, (StandardEffectInfo)baseVal5);
		string str6 = empty;
		AbilityModPropertyInt extraDirectHitDamagePerSquareMod = m_extraDirectHitDamagePerSquareMod;
		int baseVal6;
		if (flag)
		{
			while (true)
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
		empty = str6 + PropDesc(extraDirectHitDamagePerSquareMod, "[ExtraDirectHitDamagePerSquare]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyInt healOnSelfPerTargetHitMod = m_healOnSelfPerTargetHitMod;
		int baseVal7;
		if (flag)
		{
			while (true)
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
		empty = str7 + PropDesc(healOnSelfPerTargetHitMod, "[HealOnSelfPerTargetHit]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyInt cooldownOnHitMod = m_cooldownOnHitMod;
		int baseVal8;
		if (flag)
		{
			while (true)
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
		empty = str8 + PropDesc(cooldownOnHitMod, "[Cooldown Override (on charge ability) on Hit]", flag, baseVal8);
		string str9 = empty;
		AbilityModPropertyBool chaseHitActorMod = m_chaseHitActorMod;
		int baseVal9;
		if (flag)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal9 = (claymoreCharge.m_chaseHitActor ? 1 : 0);
		}
		else
		{
			baseVal9 = 0;
		}
		return str9 + PropDesc(chaseHitActorMod, "[Chase Hit Target?]", flag, (byte)baseVal9 != 0);
	}
}
