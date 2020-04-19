using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ClericMeleeKnockback : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyBool m_penetrateLineOfSightMod;

	public AbilityModPropertyFloat m_minSeparationBetweenAoeAndCasterMod;

	public AbilityModPropertyFloat m_maxSeparationBetweenAoeAndCasterMod;

	public AbilityModPropertyFloat m_aoeRadiusMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	[Header("-- On Hit Damage/Effect")]
	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyFloat m_knockbackDistanceMod;

	public AbilityModPropertyKnockbackType m_knockbackTypeMod;

	public AbilityModPropertyEffectInfo m_targetHitEffectMod;

	public AbilityModPropertyEffectInfo m_singleTargetHitEffectMod;

	public AbilityModPropertyInt m_extraTechPointsPerHitWithAreaBuff;

	[Separator("Connecting Laser between caster and aoe center", true)]
	public AbilityModPropertyFloat m_connectLaserWidthMod;

	public AbilityModPropertyInt m_connectLaserDamageMod;

	public AbilityModPropertyEffectInfo m_connectLaserEnemyHitEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ClericMeleeKnockback);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ClericMeleeKnockback clericMeleeKnockback = targetAbility as ClericMeleeKnockback;
		if (clericMeleeKnockback != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ClericMeleeKnockback.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_minSeparationBetweenAoeAndCasterMod, "MinSeparationBetweenAoeAndCaster", string.Empty, clericMeleeKnockback.m_minSeparationBetweenAoeAndCaster, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxSeparationBetweenAoeAndCasterMod, "MaxSeparationBetweenAoeAndCaster", string.Empty, clericMeleeKnockback.m_maxSeparationBetweenAoeAndCaster, true, false, false);
			AbilityMod.AddToken(tokens, this.m_aoeRadiusMod, "AoeRadius", string.Empty, clericMeleeKnockback.m_aoeRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTargetsMod, "MaxTargets", string.Empty, clericMeleeKnockback.m_maxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountMod, "DamageAmount", string.Empty, clericMeleeKnockback.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_knockbackDistanceMod, "KnockbackDistance", string.Empty, clericMeleeKnockback.m_knockbackDistance, true, false, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_targetHitEffectMod, "TargetHitEffect", clericMeleeKnockback.m_targetHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_singleTargetHitEffectMod, "SingleTargetHitEffect", clericMeleeKnockback.m_targetHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_extraTechPointsPerHitWithAreaBuff, "ExtraEnergyPerHitWithAreaBuff", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_connectLaserWidthMod, "ConnectLaserWidth", string.Empty, clericMeleeKnockback.m_connectLaserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_connectLaserDamageMod, "ConnectLaserDamage", string.Empty, clericMeleeKnockback.m_connectLaserDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_connectLaserEnemyHitEffectMod, "ConnectLaserEnemyHitEffect", clericMeleeKnockback.m_connectLaserEnemyHitEffect, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClericMeleeKnockback clericMeleeKnockback = base.GetTargetAbilityOnAbilityData(abilityData) as ClericMeleeKnockback;
		bool flag = clericMeleeKnockback != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyBool penetrateLineOfSightMod = this.m_penetrateLineOfSightMod;
		string prefix = "[PenetrateLineOfSight]";
		bool showBaseVal = flag;
		bool baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ClericMeleeKnockback.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = clericMeleeKnockback.m_penetrateLineOfSight;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(penetrateLineOfSightMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat minSeparationBetweenAoeAndCasterMod = this.m_minSeparationBetweenAoeAndCasterMod;
		string prefix2 = "[MinSeparationBetweenAoeAndCaster]";
		bool showBaseVal2 = flag;
		float baseVal2;
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
			baseVal2 = clericMeleeKnockback.m_minSeparationBetweenAoeAndCaster;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(minSeparationBetweenAoeAndCasterMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_maxSeparationBetweenAoeAndCasterMod, "[MaxSeparationBetweenAoeAndCaster]", flag, (!flag) ? 0f : clericMeleeKnockback.m_maxSeparationBetweenAoeAndCaster);
		string str3 = text;
		AbilityModPropertyFloat aoeRadiusMod = this.m_aoeRadiusMod;
		string prefix3 = "[AoeRadius]";
		bool showBaseVal3 = flag;
		float baseVal3;
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
			baseVal3 = clericMeleeKnockback.m_aoeRadius;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(aoeRadiusMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt maxTargetsMod = this.m_maxTargetsMod;
		string prefix4 = "[MaxTargets]";
		bool showBaseVal4 = flag;
		int baseVal4;
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
			baseVal4 = clericMeleeKnockback.m_maxTargets;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(maxTargetsMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt damageAmountMod = this.m_damageAmountMod;
		string prefix5 = "[DamageAmount]";
		bool showBaseVal5 = flag;
		int baseVal5;
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
			baseVal5 = clericMeleeKnockback.m_damageAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(damageAmountMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyFloat knockbackDistanceMod = this.m_knockbackDistanceMod;
		string prefix6 = "[KnockbackDistance]";
		bool showBaseVal6 = flag;
		float baseVal6;
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
			baseVal6 = clericMeleeKnockback.m_knockbackDistance;
		}
		else
		{
			baseVal6 = 0f;
		}
		text = str6 + base.PropDesc(knockbackDistanceMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyKnockbackType knockbackTypeMod = this.m_knockbackTypeMod;
		string prefix7 = "[KnockbackType]";
		bool showBaseVal7 = flag;
		KnockbackType baseVal7;
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
			baseVal7 = clericMeleeKnockback.m_knockbackType;
		}
		else
		{
			baseVal7 = KnockbackType.AwayFromSource;
		}
		text = str7 + base.PropDesc(knockbackTypeMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyEffectInfo targetHitEffectMod = this.m_targetHitEffectMod;
		string prefix8 = "[TargetHitEffect]";
		bool showBaseVal8 = flag;
		StandardEffectInfo baseVal8;
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
			baseVal8 = clericMeleeKnockback.m_targetHitEffect;
		}
		else
		{
			baseVal8 = null;
		}
		text = str8 + base.PropDesc(targetHitEffectMod, prefix8, showBaseVal8, baseVal8);
		text += base.PropDesc(this.m_extraTechPointsPerHitWithAreaBuff, "[ExtraEnergyPerHitWithAreaBuff]", flag, 0);
		string str9 = text;
		AbilityModPropertyFloat connectLaserWidthMod = this.m_connectLaserWidthMod;
		string prefix9 = "[ConnectLaserWidth]";
		bool showBaseVal9 = flag;
		float baseVal9;
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
			baseVal9 = clericMeleeKnockback.m_connectLaserWidth;
		}
		else
		{
			baseVal9 = 0f;
		}
		text = str9 + base.PropDesc(connectLaserWidthMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyInt connectLaserDamageMod = this.m_connectLaserDamageMod;
		string prefix10 = "[ConnectLaserDamage]";
		bool showBaseVal10 = flag;
		int baseVal10;
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
			baseVal10 = clericMeleeKnockback.m_connectLaserDamage;
		}
		else
		{
			baseVal10 = 0;
		}
		text = str10 + base.PropDesc(connectLaserDamageMod, prefix10, showBaseVal10, baseVal10);
		string str11 = text;
		AbilityModPropertyEffectInfo connectLaserEnemyHitEffectMod = this.m_connectLaserEnemyHitEffectMod;
		string prefix11 = "[ConnectLaserEnemyHitEffect]";
		bool showBaseVal11 = flag;
		StandardEffectInfo baseVal11;
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
			baseVal11 = clericMeleeKnockback.m_connectLaserEnemyHitEffect;
		}
		else
		{
			baseVal11 = null;
		}
		return str11 + base.PropDesc(connectLaserEnemyHitEffectMod, prefix11, showBaseVal11, baseVal11);
	}
}
