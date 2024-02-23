using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_ScampAoeTether : AbilityMod
{
	[Separator("Targeting")]
	public AbilityModPropertyFloat m_aoeRadiusMod;
	public AbilityModPropertyBool m_ignoreLosMod;
	[Header("-- if > 0, will use this as distance to check whether tether should break")]
	public AbilityModPropertyFloat m_tetherBreakDistanceOverrideMod;
	[Separator("Whether to pull towards caster if target is out of range. If true, no longer do movement hits")]
	public AbilityModPropertyBool m_pullToCasterInKnockbackMod;
	public AbilityModPropertyFloat m_maxKnockbackDistMod;
	[Separator("Disable in Shield Down mode?")]
	public AbilityModPropertyBool m_disableIfShieldDownMod;
	[Separator("On Tether Apply")]
	public AbilityModPropertyEffectInfo m_tetherApplyEnemyEffectMod;
	[Separator("On Tether Break")]
	public AbilityModPropertyInt m_tetherBreakDamageMod;
	public AbilityModPropertyEffectInfo m_tetherBreakEnemyEffecfMod;
	[Separator("Cdr if not triggered")]
	public AbilityModPropertyInt m_cdrIfNoTetherTriggerMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ScampAoeTether);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ScampAoeTether scampAoeTether = targetAbility as ScampAoeTether;
		if (scampAoeTether != null)
		{
			AddToken(tokens, m_aoeRadiusMod, "AoeRadius", string.Empty, scampAoeTether.m_aoeRadius);
			AddToken(tokens, m_tetherBreakDistanceOverrideMod, "TetherBreakDistanceOverride", string.Empty, scampAoeTether.m_tetherBreakDistanceOverride);
			AddToken(tokens, m_maxKnockbackDistMod, "MaxKnockbackDist", string.Empty, scampAoeTether.m_maxKnockbackDist);
			AddToken_EffectMod(tokens, m_tetherApplyEnemyEffectMod, "TetherApplyEnemyEffect", scampAoeTether.m_tetherApplyEnemyEffect);
			AddToken(tokens, m_tetherBreakDamageMod, "TetherBreakDamage", string.Empty, scampAoeTether.m_tetherBreakDamage);
			AddToken_EffectMod(tokens, m_tetherBreakEnemyEffecfMod, "TetherBreakEnemyEffecf", scampAoeTether.m_tetherBreakEnemyEffecf);
			AddToken(tokens, m_cdrIfNoTetherTriggerMod, "CdrIfNoTetherTrigger", string.Empty, scampAoeTether.m_cdrIfNoTetherTrigger);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScampAoeTether scampAoeTether = GetTargetAbilityOnAbilityData(abilityData) as ScampAoeTether;
		bool isValid = scampAoeTether != null;
		string desc = string.Empty;
		desc += PropDesc(m_aoeRadiusMod, "[AoeRadius]", isValid, isValid ? scampAoeTether.m_aoeRadius : 0f);
		desc += PropDesc(m_ignoreLosMod, "[IgnoreLos]", isValid, isValid && scampAoeTether.m_ignoreLos);
		desc += PropDesc(m_tetherBreakDistanceOverrideMod, "[TetherBreakDistanceOverride]", isValid, isValid ? scampAoeTether.m_tetherBreakDistanceOverride : 0f);
		desc += PropDesc(m_pullToCasterInKnockbackMod, "[PullToCasterInKnockback]", isValid, isValid && scampAoeTether.m_pullToCasterInKnockback);
		desc += PropDesc(m_maxKnockbackDistMod, "[MaxKnockbackDist]", isValid, isValid ? scampAoeTether.m_maxKnockbackDist : 0f);
		desc += PropDesc(m_disableIfShieldDownMod, "[DisableIfShieldDown]", isValid, isValid && scampAoeTether.m_disableIfShieldDown);
		desc += PropDesc(m_tetherApplyEnemyEffectMod, "[TetherApplyEnemyEffect]", isValid, isValid ? scampAoeTether.m_tetherApplyEnemyEffect : null);
		desc += PropDesc(m_tetherBreakDamageMod, "[TetherBreakDamage]", isValid, isValid ? scampAoeTether.m_tetherBreakDamage : 0);
		desc += PropDesc(m_tetherBreakEnemyEffecfMod, "[TetherBreakEnemyEffecf]", isValid, isValid ? scampAoeTether.m_tetherBreakEnemyEffecf : null);
		return new StringBuilder().Append(desc).Append(PropDesc(m_cdrIfNoTetherTriggerMod, "[CdrIfNoTetherTrigger]", isValid, isValid ? scampAoeTether.m_cdrIfNoTetherTrigger : 0)).ToString();
	}
}
