using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ValkyrieDashAoE : AbilityMod
{
	[Header("-- Shield effect")]
	public AbilityModPropertyEffectInfo m_shieldEffectInfoMod;

	public AbilityModPropertyInt m_techPointGainPerCoveredHitMod;

	public AbilityModPropertyInt m_techPointGainPerTooCloseForCoverHitMod;

	[Header("-- Targeting")]
	public AbilityModPropertyShape m_aoeShapeMod;

	public AbilityModPropertyBool m_aoePenetratesLoSMod;

	[Separator("Aim Shield and Cone", true)]
	public AbilityModPropertyFloat m_coneWidthAngleMod;

	public AbilityModPropertyFloat m_coneRadiusMod;

	public AbilityModPropertyInt m_coverDurationMod;

	[Header("-- Cover Ignore Min Dist?")]
	public AbilityModPropertyBool m_coverIgnoreMinDistMod;

	[Header("-- Whether to put guard ability on cooldown")]
	public AbilityModPropertyBool m_triggerCooldownOnGuardAbiityMod;

	[Separator("Enemy hits", true)]
	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyEffectInfo m_enemyDebuffMod;

	[Separator("Ally & self hits", true)]
	public AbilityModPropertyInt m_absorbMod;

	public AbilityModPropertyEffectInfo m_allyBuffMod;

	public AbilityModPropertyEffectInfo m_selfBuffMod;

	[Header("-- Cooldown reductions")]
	public AbilityModPropertyInt m_cooldownReductionIfDamagedThisTurnMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ValkyrieDashAoE);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ValkyrieDashAoE valkyrieDashAoE = targetAbility as ValkyrieDashAoE;
		if (!(valkyrieDashAoE != null))
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
			AbilityMod.AddToken_EffectMod(tokens, m_shieldEffectInfoMod, "ShieldEffectInfo", valkyrieDashAoE.m_shieldEffectInfo);
			AbilityMod.AddToken(tokens, m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, valkyrieDashAoE.m_coneWidthAngle);
			AbilityMod.AddToken(tokens, m_coneRadiusMod, "ConeRadius", string.Empty, valkyrieDashAoE.m_coneRadius);
			AbilityMod.AddToken(tokens, m_damageMod, "Damage", string.Empty, valkyrieDashAoE.m_damage);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyDebuffMod, "EnemyDebuff", valkyrieDashAoE.m_enemyDebuff);
			AbilityMod.AddToken(tokens, m_absorbMod, "Absorb", string.Empty, valkyrieDashAoE.m_absorb);
			AbilityMod.AddToken_EffectMod(tokens, m_allyBuffMod, "AllyBuff", valkyrieDashAoE.m_allyBuff);
			AbilityMod.AddToken_EffectMod(tokens, m_selfBuffMod, "SelfBuff", valkyrieDashAoE.m_selfBuff);
			AbilityMod.AddToken(tokens, m_techPointGainPerCoveredHitMod, "TechPointGainPerCoveredHit", string.Empty, valkyrieDashAoE.m_techPointGainPerCoveredHit);
			AbilityMod.AddToken(tokens, m_techPointGainPerTooCloseForCoverHitMod, "TechPointGainPerTooCloseForCoverHit", string.Empty, valkyrieDashAoE.m_techPointGainPerTooCloseForCoverHit);
			AbilityMod.AddToken(tokens, m_cooldownReductionIfDamagedThisTurnMod, "CooldownReductionIfDamagedThisTurn", string.Empty, valkyrieDashAoE.m_cooldownReductionIfDamagedThisTurn.cooldownAddAmount);
			AbilityMod.AddToken_IntDiff(tokens, "CoverDuration_Final", string.Empty, m_coverDurationMod.GetModifiedValue(valkyrieDashAoE.m_coverDuration) - 1, false, 0);
			AbilityMod.AddToken(tokens, m_coverDurationMod, "CoverDuration_Alt", string.Empty, valkyrieDashAoE.m_coverDuration);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ValkyrieDashAoE valkyrieDashAoE = GetTargetAbilityOnAbilityData(abilityData) as ValkyrieDashAoE;
		bool flag = valkyrieDashAoE != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyEffectInfo shieldEffectInfoMod = m_shieldEffectInfoMod;
		object baseVal;
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
			baseVal = valkyrieDashAoE.m_shieldEffectInfo;
		}
		else
		{
			baseVal = null;
		}
		empty = str + PropDesc(shieldEffectInfoMod, "[ShieldEffectInfo]", flag, (StandardEffectInfo)baseVal);
		string str2 = empty;
		AbilityModPropertyShape aoeShapeMod = m_aoeShapeMod;
		int baseVal2;
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
			baseVal2 = (int)valkyrieDashAoE.m_aoeShape;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(aoeShapeMod, "[AoeShape]", flag, (AbilityAreaShape)baseVal2);
		empty += PropDesc(m_aoePenetratesLoSMod, "[AoePenetratesLoS]", flag, flag && valkyrieDashAoE.m_aoePenetratesLoS);
		string str3 = empty;
		AbilityModPropertyFloat coneWidthAngleMod = m_coneWidthAngleMod;
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
			baseVal3 = valkyrieDashAoE.m_coneWidthAngle;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(coneWidthAngleMod, "[ConeWidthAngle]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyFloat coneRadiusMod = m_coneRadiusMod;
		float baseVal4;
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
			baseVal4 = valkyrieDashAoE.m_coneRadius;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + PropDesc(coneRadiusMod, "[ConeRadius]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyBool triggerCooldownOnGuardAbiityMod = m_triggerCooldownOnGuardAbiityMod;
		int baseVal5;
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
			baseVal5 = (valkyrieDashAoE.m_triggerCooldownOnGuardAbiity ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(triggerCooldownOnGuardAbiityMod, "[TriggerCooldownOnGuardAbiity]", flag, (byte)baseVal5 != 0);
		string str6 = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal6;
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
			baseVal6 = valkyrieDashAoE.m_damage;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(damageMod, "[Damage]", flag, baseVal6);
		empty += PropDesc(m_enemyDebuffMod, "[EnemyDebuff]", flag, (!flag) ? null : valkyrieDashAoE.m_enemyDebuff);
		empty += PropDesc(m_absorbMod, "[Absorb]", flag, flag ? valkyrieDashAoE.m_absorb : 0);
		string str7 = empty;
		AbilityModPropertyEffectInfo allyBuffMod = m_allyBuffMod;
		object baseVal7;
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
			baseVal7 = valkyrieDashAoE.m_allyBuff;
		}
		else
		{
			baseVal7 = null;
		}
		empty = str7 + PropDesc(allyBuffMod, "[AllyBuff]", flag, (StandardEffectInfo)baseVal7);
		empty += PropDesc(m_selfBuffMod, "[SelfBuff]", flag, (!flag) ? null : valkyrieDashAoE.m_selfBuff);
		string str8 = empty;
		AbilityModPropertyInt techPointGainPerCoveredHitMod = m_techPointGainPerCoveredHitMod;
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
			baseVal8 = valkyrieDashAoE.m_techPointGainPerCoveredHit;
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(techPointGainPerCoveredHitMod, "[TechPointGainPerCoveredHit]", flag, baseVal8);
		empty += PropDesc(m_techPointGainPerTooCloseForCoverHitMod, "[TechPointGainPerTooCloseForCoverHit]", flag, flag ? valkyrieDashAoE.m_techPointGainPerTooCloseForCoverHit : 0);
		empty += PropDesc(m_cooldownReductionIfDamagedThisTurnMod, "[CooldownReductionIfDamagedThisTurn]", flag, flag ? valkyrieDashAoE.m_cooldownReductionIfDamagedThisTurn.cooldownAddAmount : 0);
		string str9 = empty;
		AbilityModPropertyInt coverDurationMod = m_coverDurationMod;
		int baseVal9;
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
			baseVal9 = valkyrieDashAoE.m_coverDuration;
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(coverDurationMod, "[CoverDuration]", flag, baseVal9);
		string str10 = empty;
		AbilityModPropertyBool coverIgnoreMinDistMod = m_coverIgnoreMinDistMod;
		int baseVal10;
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
			baseVal10 = (valkyrieDashAoE.m_coverIgnoreMinDist ? 1 : 0);
		}
		else
		{
			baseVal10 = 0;
		}
		return str10 + PropDesc(coverIgnoreMinDistMod, "[CoverIgnoreMinDist]", flag, (byte)baseVal10 != 0);
	}
}
