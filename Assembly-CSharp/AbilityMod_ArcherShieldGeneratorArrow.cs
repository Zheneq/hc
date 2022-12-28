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
		if (archerShieldGeneratorArrow != null)
		{
			AddToken(tokens, m_lessAbsorbPerTurnMod, "LessAbsorbPerTurn", string.Empty, archerShieldGeneratorArrow.m_lessAbsorbPerTurn);
			AddToken_EffectMod(tokens, m_directHitEnemyEffectMod, "DirectHitEnemyEffect", archerShieldGeneratorArrow.m_directHitEnemyEffect);
			AddToken_EffectMod(tokens, m_directHitAllyEffectMod, "DirectHitAllyEffect", archerShieldGeneratorArrow.m_directHitAllyEffect);
			AddToken_EffectMod(tokens, m_extraAllyHitEffectMod, "ExtraAllyHitEffect", archerShieldGeneratorArrow.m_extraAllyHitEffect);
			AddToken(tokens, m_cooldownReductionOnDash, "CooldownReductionOnDashAbility", string.Empty, 0);
			AddToken(tokens, m_extraAbsorbPerEnemyHit, "ExtraAbsorbPerEnemyHit", string.Empty, 0);
			AddToken(tokens, m_extraAbsorbIfEnemyHit, "ExtraAbsorbIfAnyEnemyHit", string.Empty, 0);
			AddToken(tokens, m_extraAbsorbIfOnlyOneAllyHit, "ExtraAbsorbIfOnlyOneAllyHit", string.Empty, 0);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ArcherShieldGeneratorArrow archerShieldGeneratorArrow = GetTargetAbilityOnAbilityData(abilityData) as ArcherShieldGeneratorArrow;
		bool isValid = archerShieldGeneratorArrow != null;
		string desc = string.Empty;
		desc += PropDesc(m_penetrateLoSMod, "[PenetrateLoS]", isValid, isValid && archerShieldGeneratorArrow.m_penetrateLoS);
		desc += PropDesc(m_affectsEnemiesMod, "[AffectsEnemies]", isValid, isValid && archerShieldGeneratorArrow.m_affectsEnemies);
		desc += PropDesc(m_affectsAlliesMod, "[AffectsAllies]", isValid, isValid && archerShieldGeneratorArrow.m_affectsAllies);
		desc += PropDesc(m_affectsCasterMod, "[AffectsCaster]", isValid, isValid && archerShieldGeneratorArrow.m_affectsCaster);
		desc += PropDesc(m_lessAbsorbPerTurnMod, "[LessAbsorbPerTurn]", isValid, isValid ? archerShieldGeneratorArrow.m_lessAbsorbPerTurn : 0);
		desc += PropDesc(m_directHitEnemyEffectMod, "[DirectHitEnemyEffect]", isValid, isValid ? archerShieldGeneratorArrow.m_directHitEnemyEffect : null);
		desc += PropDesc(m_directHitAllyEffectMod, "[DirectHitAllyEffect]", isValid, isValid ? archerShieldGeneratorArrow.m_directHitAllyEffect : null);
		desc += PropDesc(m_extraAllyHitEffectMod, "[ExtraAllyHitEffect]", isValid, isValid ? archerShieldGeneratorArrow.m_extraAllyHitEffect : null);
		desc += PropDesc(m_cooldownReductionOnDash, "[CooldownReductionOnDashAbility]", isValid);
		desc += PropDesc(m_extraAbsorbPerEnemyHit, "[ExtraAbsorbPerEnemyHit]", isValid);
		desc += PropDesc(m_extraAbsorbIfEnemyHit, "[ExtraAbsorbIfAnyEnemyHit]", isValid);
		return desc + PropDesc(m_extraAbsorbIfOnlyOneAllyHit, "[ExtraAbsorbIfOnlyOneAllyHit]", isValid);
	}
}
