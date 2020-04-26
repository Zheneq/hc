using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ArcherShieldGeneratorArrow : AbilityMod
{
	[Header("-- Ground effect")]
	public AbilityModPropertyBool m_penetrateLoSMod;

	public AbilityModPropertyBool m_affectsEnemiesMod;

	public AbilityModPropertyBool m_affectsAlliesMod;

	public AbilityModPropertyBool m_affectsCasterMod;

	public AbilityModPropertyInt m_lessAbsorbPerTurnMod;

	public AbilityModPropertyEffectInfo m_directHitEnemyEffectMod;

	public AbilityModPropertyEffectInfo m_directHitAllyEffectMod;

	[Header("-- Extra effect for shielding that last different number of turns from main effect, etc")]
	public AbilityModPropertyEffectInfo m_extraAllyHitEffectMod;

	[Header("-- Num hits mods")]
	public AbilityModPropertyInt m_extraAbsorbPerEnemyHit;

	public AbilityModPropertyInt m_extraAbsorbIfEnemyHit;

	public AbilityModPropertyInt m_extraAbsorbIfOnlyOneAllyHit;

	[Header("-- Misc ability interactions")]
	public AbilityModPropertyInt m_cooldownReductionOnDash;

	public override Type GetTargetAbilityType()
	{
		return typeof(ArcherShieldGeneratorArrow);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ArcherShieldGeneratorArrow archerShieldGeneratorArrow = targetAbility as ArcherShieldGeneratorArrow;
		if (!(archerShieldGeneratorArrow != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_lessAbsorbPerTurnMod, "LessAbsorbPerTurn", string.Empty, archerShieldGeneratorArrow.m_lessAbsorbPerTurn);
			AbilityMod.AddToken_EffectMod(tokens, m_directHitEnemyEffectMod, "DirectHitEnemyEffect", archerShieldGeneratorArrow.m_directHitEnemyEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_directHitAllyEffectMod, "DirectHitAllyEffect", archerShieldGeneratorArrow.m_directHitAllyEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_extraAllyHitEffectMod, "ExtraAllyHitEffect", archerShieldGeneratorArrow.m_extraAllyHitEffect);
			AbilityMod.AddToken(tokens, m_cooldownReductionOnDash, "CooldownReductionOnDashAbility", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_extraAbsorbPerEnemyHit, "ExtraAbsorbPerEnemyHit", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_extraAbsorbIfEnemyHit, "ExtraAbsorbIfAnyEnemyHit", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_extraAbsorbIfOnlyOneAllyHit, "ExtraAbsorbIfOnlyOneAllyHit", string.Empty, 0);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ArcherShieldGeneratorArrow archerShieldGeneratorArrow = GetTargetAbilityOnAbilityData(abilityData) as ArcherShieldGeneratorArrow;
		bool flag = archerShieldGeneratorArrow != null;
		string empty = string.Empty;
		empty += PropDesc(m_penetrateLoSMod, "[PenetrateLoS]", flag, flag && archerShieldGeneratorArrow.m_penetrateLoS);
		string str = empty;
		AbilityModPropertyBool affectsEnemiesMod = m_affectsEnemiesMod;
		int baseVal;
		if (flag)
		{
			baseVal = (archerShieldGeneratorArrow.m_affectsEnemies ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(affectsEnemiesMod, "[AffectsEnemies]", flag, (byte)baseVal != 0);
		string str2 = empty;
		AbilityModPropertyBool affectsAlliesMod = m_affectsAlliesMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = (archerShieldGeneratorArrow.m_affectsAllies ? 1 : 0);
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(affectsAlliesMod, "[AffectsAllies]", flag, (byte)baseVal2 != 0);
		string str3 = empty;
		AbilityModPropertyBool affectsCasterMod = m_affectsCasterMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = (archerShieldGeneratorArrow.m_affectsCaster ? 1 : 0);
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(affectsCasterMod, "[AffectsCaster]", flag, (byte)baseVal3 != 0);
		string str4 = empty;
		AbilityModPropertyInt lessAbsorbPerTurnMod = m_lessAbsorbPerTurnMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = archerShieldGeneratorArrow.m_lessAbsorbPerTurn;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(lessAbsorbPerTurnMod, "[LessAbsorbPerTurn]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyEffectInfo directHitEnemyEffectMod = m_directHitEnemyEffectMod;
		object baseVal5;
		if (flag)
		{
			baseVal5 = archerShieldGeneratorArrow.m_directHitEnemyEffect;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str5 + PropDesc(directHitEnemyEffectMod, "[DirectHitEnemyEffect]", flag, (StandardEffectInfo)baseVal5);
		string str6 = empty;
		AbilityModPropertyEffectInfo directHitAllyEffectMod = m_directHitAllyEffectMod;
		object baseVal6;
		if (flag)
		{
			baseVal6 = archerShieldGeneratorArrow.m_directHitAllyEffect;
		}
		else
		{
			baseVal6 = null;
		}
		empty = str6 + PropDesc(directHitAllyEffectMod, "[DirectHitAllyEffect]", flag, (StandardEffectInfo)baseVal6);
		empty += PropDesc(m_extraAllyHitEffectMod, "[ExtraAllyHitEffect]", flag, (!flag) ? null : archerShieldGeneratorArrow.m_extraAllyHitEffect);
		empty += PropDesc(m_cooldownReductionOnDash, "[CooldownReductionOnDashAbility]", flag);
		empty += PropDesc(m_extraAbsorbPerEnemyHit, "[ExtraAbsorbPerEnemyHit]", flag);
		empty += PropDesc(m_extraAbsorbIfEnemyHit, "[ExtraAbsorbIfAnyEnemyHit]", flag);
		return empty + PropDesc(m_extraAbsorbIfOnlyOneAllyHit, "[ExtraAbsorbIfOnlyOneAllyHit]", flag);
	}
}
