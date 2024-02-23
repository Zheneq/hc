using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_SenseiYingYangDash : AbilityMod
{
	[Separator("Targeting Info", "cyan")]
	public AbilityModPropertyShape m_chooseDestShapeMod;
	[Separator("For Second Dash", "cyan")]
	public AbilityModPropertyInt m_secondCastTurnsMod;
	public AbilityModPropertyBool m_secondDashAllowBothTeamsMod;
	[Separator("On Enemy Hit")]
	public AbilityModPropertyInt m_damageMod;
	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;
	public AbilityModPropertyInt m_extraDamageForDiffTeamSecondDashMod;
	public AbilityModPropertyInt m_extraDamageForLowHealthMod;
	public AbilityModPropertyFloat m_enemyLowHealthThreshMod;
	public AbilityModPropertyBool m_reverseHealthThreshForEnemyMod;
	[Separator("On Ally Hit")]
	public AbilityModPropertyInt m_healOnAllyMod;
	public AbilityModPropertyEffectInfo m_allyHitEffectMod;
	public AbilityModPropertyInt m_extraHealOnAllyForDiffTeamSecondDashMod;
	public AbilityModPropertyInt m_extraHealOnAllyForLowHealthMod;
	public AbilityModPropertyFloat m_allyLowHealthThreshMod;
	public AbilityModPropertyBool m_reverseHealthThreshForAllyMod;

	[Header("-- Cooldown reduction")]
	public AbilityModPropertyInt m_cdrIfNoSecondDashMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SenseiYingYangDash);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SenseiYingYangDash senseiYingYangDash = targetAbility as SenseiYingYangDash;
		if (senseiYingYangDash != null)
		{
			AddToken(tokens, m_secondCastTurnsMod, "SecondCastTurns", string.Empty, senseiYingYangDash.m_secondCastTurns);
			AddToken(tokens, m_damageMod, "Damage", string.Empty, senseiYingYangDash.m_damage);
			AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", senseiYingYangDash.m_enemyHitEffect);
			AddToken(tokens, m_extraDamageForDiffTeamSecondDashMod, "ExtraDamageForDiffTeamSecondDash", string.Empty, senseiYingYangDash.m_extraDamageForDiffTeamSecondDash);
			AddToken(tokens, m_extraDamageForLowHealthMod, "ExtraDamageForLowHealth", string.Empty, senseiYingYangDash.m_extraDamageForLowHealth);
			AddToken(tokens, m_enemyLowHealthThreshMod, "EnemyLowHealthThresh", string.Empty, senseiYingYangDash.m_enemyLowHealthThresh, true, false, true);
			AddToken(tokens, m_healOnAllyMod, "HealOnAlly", string.Empty, senseiYingYangDash.m_healOnAlly);
			AddToken_EffectMod(tokens, m_allyHitEffectMod, "AllyHitEffect", senseiYingYangDash.m_allyHitEffect);
			AddToken(tokens, m_extraHealOnAllyForDiffTeamSecondDashMod, "ExtraHealOnAllyForDiffTeamSecondDash", string.Empty, senseiYingYangDash.m_extraHealOnAllyForDiffTeamSecondDash);
			AddToken(tokens, m_extraHealOnAllyForLowHealthMod, "ExtraHealOnAllyForLowHealth", string.Empty, senseiYingYangDash.m_extraHealOnAllyForLowHealth);
			AddToken(tokens, m_allyLowHealthThreshMod, "AllyLowHealthThresh", string.Empty, senseiYingYangDash.m_allyLowHealthThresh, true, false, true);
			AddToken(tokens, m_cdrIfNoSecondDashMod, "CdrIfNoSecondDash", string.Empty, senseiYingYangDash.m_cdrIfNoSecondDash);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SenseiYingYangDash senseiYingYangDash = GetTargetAbilityOnAbilityData(abilityData) as SenseiYingYangDash;
		bool isValid = senseiYingYangDash != null;
		string desc = string.Empty;
		desc += PropDesc(m_chooseDestShapeMod, "[ChooseDestShape]", isValid, isValid ? senseiYingYangDash.m_chooseDestShape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_secondCastTurnsMod, "[SecondCastTurns]", isValid, isValid ? senseiYingYangDash.m_secondCastTurns : 0);
		desc += PropDesc(m_secondDashAllowBothTeamsMod, "[SecondDashAllowBothTeams]", isValid, isValid && senseiYingYangDash.m_secondDashAllowBothTeams);
		desc += PropDesc(m_damageMod, "[Damage]", isValid, isValid ? senseiYingYangDash.m_damage : 0);
		desc += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", isValid, isValid ? senseiYingYangDash.m_enemyHitEffect : null);
		desc += PropDesc(m_extraDamageForDiffTeamSecondDashMod, "[ExtraDamageForDiffTeamSecondDash]", isValid, isValid ? senseiYingYangDash.m_extraDamageForDiffTeamSecondDash : 0);
		desc += PropDesc(m_extraDamageForLowHealthMod, "[ExtraDamageForLowHealth]", isValid, isValid ? senseiYingYangDash.m_extraDamageForLowHealth : 0);
		desc += PropDesc(m_enemyLowHealthThreshMod, "[EnemyLowHealthThresh]", isValid, isValid ? senseiYingYangDash.m_enemyLowHealthThresh : 0f);
		desc += PropDesc(m_reverseHealthThreshForEnemyMod, "[ReverseHealthThreshForEnemy]", isValid, isValid && senseiYingYangDash.m_reverseHealthThreshForEnemy);
		desc += PropDesc(m_healOnAllyMod, "[HealOnAlly]", isValid, isValid ? senseiYingYangDash.m_healOnAlly : 0);
		desc += PropDesc(m_allyHitEffectMod, "[AllyHitEffect]", isValid, isValid ? senseiYingYangDash.m_allyHitEffect : null);
		desc += PropDesc(m_extraHealOnAllyForDiffTeamSecondDashMod, "[ExtraHealOnAllyForDiffTeamSecondDash]", isValid, isValid ? senseiYingYangDash.m_extraHealOnAllyForDiffTeamSecondDash : 0);
		desc += PropDesc(m_extraHealOnAllyForLowHealthMod, "[ExtraHealOnAllyForLowHealth]", isValid, isValid ? senseiYingYangDash.m_extraHealOnAllyForLowHealth : 0);
		desc += PropDesc(m_allyLowHealthThreshMod, "[AllyLowHealthThresh]", isValid, isValid ? senseiYingYangDash.m_allyLowHealthThresh : 0f);
		desc += PropDesc(m_reverseHealthThreshForAllyMod, "[ReverseHealthThreshForAlly]", isValid, isValid && senseiYingYangDash.m_reverseHealthThreshForAlly);
		return new StringBuilder().Append(desc).Append(PropDesc(m_cdrIfNoSecondDashMod, "[CdrIfNoSecondDash]", isValid, isValid ? senseiYingYangDash.m_cdrIfNoSecondDash : 0)).ToString();
	}
}
