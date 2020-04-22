using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SamuraiSwordDash : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_damageRadiusMod;

	public AbilityModPropertyFloat m_damageRadiusAtStartMod;

	public AbilityModPropertyFloat m_damageRadiusAtEndMod;

	public AbilityModPropertyBool m_penetrateLineOfSightMod;

	public AbilityModPropertyBool m_canMoveAfterEvadeMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	[Header("-- How many targets can be damaged")]
	public AbilityModPropertyInt m_maxDamageTargetsMod;

	[Header("-- Enemy Hits, Dash Phase")]
	public AbilityModPropertyInt m_dashDamageMod;

	public AbilityModPropertyInt m_dashLessDamagePerTargetMod;

	public AbilityModPropertyEffectInfo m_dashEnemyHitEffectMod;

	[Header("-- Effect on Self")]
	public AbilityModPropertyEffectInfo m_dashSelfHitEffectMod;

	[Header("-- Mark data")]
	public AbilityModPropertyEffectInfo m_markEffectInfoMod;

	[Header("-- Energy Refund if target dashed away")]
	public AbilityModPropertyInt m_energyRefundIfTargetDashedAwayMod;

	[Separator("For Chain Ability (Knockback phase)", true)]
	public AbilityModPropertyInt m_knockbackDamageMod;

	public AbilityModPropertyInt m_knockbackLessDamagePerTargetMod;

	public AbilityModPropertyFloat m_knockbackExtraDamageFromDamageTakenMultMod;

	[Space(10f)]
	public AbilityModPropertyInt m_knockbackExtraDamageByDistMod;

	public AbilityModPropertyInt m_knockbackExtraDamageChangePerDistMod;

	[Header("-- Knockback")]
	public AbilityModPropertyFloat m_knockbackDistMod;

	public AbilityModPropertyKnockbackType m_knockbackTypeMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SamuraiSwordDash);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SamuraiSwordDash samuraiSwordDash = targetAbility as SamuraiSwordDash;
		if (!(samuraiSwordDash != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_damageRadiusMod, "DamageRadius", string.Empty, samuraiSwordDash.m_damageRadius);
			AbilityMod.AddToken(tokens, m_damageRadiusAtStartMod, "DamageRadiusAtStart", string.Empty, samuraiSwordDash.m_damageRadiusAtStart);
			AbilityMod.AddToken(tokens, m_damageRadiusAtEndMod, "DamageRadiusAtEnd", string.Empty, samuraiSwordDash.m_damageRadiusAtEnd);
			AbilityMod.AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, samuraiSwordDash.m_maxTargets);
			AbilityMod.AddToken(tokens, m_maxDamageTargetsMod, "MaxDamageTargets", string.Empty, samuraiSwordDash.m_maxDamageTargets);
			AbilityMod.AddToken(tokens, m_dashDamageMod, "DamageAmount", string.Empty, samuraiSwordDash.m_dashDamage);
			AbilityMod.AddToken(tokens, m_dashLessDamagePerTargetMod, "LessDamagePerTarget", string.Empty, samuraiSwordDash.m_dashLessDamagePerTarget);
			AbilityMod.AddToken_EffectMod(tokens, m_dashEnemyHitEffectMod, "DashEnemyHitEffect", samuraiSwordDash.m_dashEnemyHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_dashSelfHitEffectMod, "DashSelfHitEffect", samuraiSwordDash.m_dashSelfHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_markEffectInfoMod, "MarkEffectInfo", samuraiSwordDash.m_markEffectInfo);
			AbilityMod.AddToken(tokens, m_energyRefundIfTargetDashedAwayMod, "EnergyRefundIfTargetDashedAway", string.Empty, samuraiSwordDash.m_energyRefundIfTargetDashedAway);
			AbilityMod.AddToken(tokens, m_knockbackDamageMod, "KnockbackDamage", string.Empty, samuraiSwordDash.m_knockbackDamage);
			AbilityMod.AddToken(tokens, m_knockbackLessDamagePerTargetMod, "KnockbackLessDamagePerTarget", string.Empty, samuraiSwordDash.m_knockbackLessDamagePerTarget);
			AbilityMod.AddToken(tokens, m_knockbackExtraDamageFromDamageTakenMultMod, "KnockbackExtraDamageFromDamageTakenMult", string.Empty, samuraiSwordDash.m_knockbackExtraDamageFromDamageTakenMult, true, false, true);
			AbilityMod.AddToken(tokens, m_knockbackExtraDamageByDistMod, "KnockbackExtraDamageByDist", string.Empty, samuraiSwordDash.m_knockbackExtraDamageByDist);
			AbilityMod.AddToken(tokens, m_knockbackExtraDamageChangePerDistMod, "KnockbackExtraDamageChangePerDist", string.Empty, samuraiSwordDash.m_knockbackExtraDamageChangePerDist);
			AbilityMod.AddToken(tokens, m_knockbackDistMod, "KnockbackDist", string.Empty, samuraiSwordDash.m_knockbackDist);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SamuraiSwordDash samuraiSwordDash = GetTargetAbilityOnAbilityData(abilityData) as SamuraiSwordDash;
		bool flag = samuraiSwordDash != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat damageRadiusMod = m_damageRadiusMod;
		float baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = samuraiSwordDash.m_damageRadius;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(damageRadiusMod, "[DamageRadius]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat damageRadiusAtStartMod = m_damageRadiusAtStartMod;
		float baseVal2;
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
			baseVal2 = samuraiSwordDash.m_damageRadiusAtStart;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(damageRadiusAtStartMod, "[DamageRadiusAtStart]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat damageRadiusAtEndMod = m_damageRadiusAtEndMod;
		float baseVal3;
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
			baseVal3 = samuraiSwordDash.m_damageRadiusAtEnd;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(damageRadiusAtEndMod, "[DamageRadiusAtEnd]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyBool penetrateLineOfSightMod = m_penetrateLineOfSightMod;
		int baseVal4;
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
			baseVal4 = (samuraiSwordDash.m_penetrateLineOfSight ? 1 : 0);
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(penetrateLineOfSightMod, "[PenetrateLineOfSight]", flag, (byte)baseVal4 != 0);
		string str5 = empty;
		AbilityModPropertyBool canMoveAfterEvadeMod = m_canMoveAfterEvadeMod;
		int baseVal5;
		if (flag)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal5 = (samuraiSwordDash.m_canMoveAfterEvade ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(canMoveAfterEvadeMod, "[CanMoveAfterEvade]", flag, (byte)baseVal5 != 0);
		string str6 = empty;
		AbilityModPropertyInt maxTargetsMod = m_maxTargetsMod;
		int baseVal6;
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
			baseVal6 = samuraiSwordDash.m_maxTargets;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(maxTargetsMod, "[MaxTargets]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyInt maxDamageTargetsMod = m_maxDamageTargetsMod;
		int baseVal7;
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
			baseVal7 = samuraiSwordDash.m_maxDamageTargets;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(maxDamageTargetsMod, "[MaxDamageTargets]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyInt dashDamageMod = m_dashDamageMod;
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
			baseVal8 = samuraiSwordDash.m_dashDamage;
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(dashDamageMod, "[DamageAmount]", flag, baseVal8);
		string str9 = empty;
		AbilityModPropertyInt dashLessDamagePerTargetMod = m_dashLessDamagePerTargetMod;
		int baseVal9;
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
			baseVal9 = samuraiSwordDash.m_dashLessDamagePerTarget;
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(dashLessDamagePerTargetMod, "[LessDamagePerTarget]", flag, baseVal9);
		string str10 = empty;
		AbilityModPropertyEffectInfo dashEnemyHitEffectMod = m_dashEnemyHitEffectMod;
		object baseVal10;
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
			baseVal10 = samuraiSwordDash.m_dashEnemyHitEffect;
		}
		else
		{
			baseVal10 = null;
		}
		empty = str10 + PropDesc(dashEnemyHitEffectMod, "[DashEnemyHitEffect]", flag, (StandardEffectInfo)baseVal10);
		string str11 = empty;
		AbilityModPropertyEffectInfo dashSelfHitEffectMod = m_dashSelfHitEffectMod;
		object baseVal11;
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
			baseVal11 = samuraiSwordDash.m_dashSelfHitEffect;
		}
		else
		{
			baseVal11 = null;
		}
		empty = str11 + PropDesc(dashSelfHitEffectMod, "[DashSelfHitEffect]", flag, (StandardEffectInfo)baseVal11);
		string str12 = empty;
		AbilityModPropertyEffectInfo markEffectInfoMod = m_markEffectInfoMod;
		object baseVal12;
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
			baseVal12 = samuraiSwordDash.m_markEffectInfo;
		}
		else
		{
			baseVal12 = null;
		}
		empty = str12 + PropDesc(markEffectInfoMod, "[MarkEffectInfo]", flag, (StandardEffectInfo)baseVal12);
		empty += PropDesc(m_energyRefundIfTargetDashedAwayMod, "[EnergyRefundIfTargetDashedAway]", flag, flag ? samuraiSwordDash.m_energyRefundIfTargetDashedAway : 0);
		empty += PropDesc(m_knockbackDamageMod, "[KnockbackDamage]", flag, flag ? samuraiSwordDash.m_knockbackDamage : 0);
		string str13 = empty;
		AbilityModPropertyInt knockbackLessDamagePerTargetMod = m_knockbackLessDamagePerTargetMod;
		int baseVal13;
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
			baseVal13 = samuraiSwordDash.m_knockbackLessDamagePerTarget;
		}
		else
		{
			baseVal13 = 0;
		}
		empty = str13 + PropDesc(knockbackLessDamagePerTargetMod, "[KnockbackLessDamagePerTarget]", flag, baseVal13);
		string str14 = empty;
		AbilityModPropertyFloat knockbackExtraDamageFromDamageTakenMultMod = m_knockbackExtraDamageFromDamageTakenMultMod;
		float baseVal14;
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
			baseVal14 = samuraiSwordDash.m_knockbackExtraDamageFromDamageTakenMult;
		}
		else
		{
			baseVal14 = 0f;
		}
		empty = str14 + PropDesc(knockbackExtraDamageFromDamageTakenMultMod, "[KnockbackExtraDamageFromDamageTakenMult]", flag, baseVal14);
		string str15 = empty;
		AbilityModPropertyInt knockbackExtraDamageByDistMod = m_knockbackExtraDamageByDistMod;
		int baseVal15;
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
			baseVal15 = samuraiSwordDash.m_knockbackExtraDamageByDist;
		}
		else
		{
			baseVal15 = 0;
		}
		empty = str15 + PropDesc(knockbackExtraDamageByDistMod, "[KnockbackExtraDamageByDist]", flag, baseVal15);
		empty += PropDesc(m_knockbackExtraDamageChangePerDistMod, "[KnockbackExtraDamageChangePerDist]", flag, flag ? samuraiSwordDash.m_knockbackExtraDamageChangePerDist : 0);
		string str16 = empty;
		AbilityModPropertyFloat knockbackDistMod = m_knockbackDistMod;
		float baseVal16;
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
			baseVal16 = samuraiSwordDash.m_knockbackDist;
		}
		else
		{
			baseVal16 = 0f;
		}
		empty = str16 + PropDesc(knockbackDistMod, "[KnockbackDist]", flag, baseVal16);
		return empty + PropDesc(m_knockbackTypeMod, "[KnockbackType]", flag, (!flag) ? KnockbackType.AwayFromSource : samuraiSwordDash.m_knockbackType);
	}
}
