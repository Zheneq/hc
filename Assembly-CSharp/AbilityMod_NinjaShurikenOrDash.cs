// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_NinjaShurikenOrDash : AbilityMod
{
	[Separator("Dash - Type, Targeting Info")]
	public AbilityModPropertyBool m_isTeleportMod;
	public AbilityModPropertyFloat m_dashRangeDefaultMod;
	public AbilityModPropertyFloat m_dashRangeMarkedMod;
	[Header("-- Who can be dash targets --")]
	public AbilityModPropertyBool m_dashRequireDeathmarkMod;
	public AbilityModPropertyFloat m_dashToUnmarkedRangeMod;
	[Space(5f)]
	public AbilityModPropertyBool m_canDashToAllyMod;
	public AbilityModPropertyBool m_canDashToEnemyMod;
	public AbilityModPropertyBool m_dashIgnoreLosMod;
	public AbilityModPropertyShape m_dashDestShapeMod;
	[Separator("Dash - On Hit Stuff")]
	public AbilityModPropertyInt m_dashDamageMod;
	public AbilityModPropertyInt m_extraDamageOnMarkedMod;
	public AbilityModPropertyInt m_extraDamageIfNotMarkedMod;
	public AbilityModPropertyEffectInfo m_dashEnemyHitEffectMod;
	public AbilityModPropertyEffectInfo m_extraEnemyEffectOnMarkedMod;
	[Header("-- For All Hit --")]
	public AbilityModPropertyInt m_dashHealingMod;
	public AbilityModPropertyEffectInfo m_dashAllyHitEffectMod;
	[Separator("Dash - [Deathmark]", "magenta")]
	public AbilityModPropertyBool m_dashApplyDeathmarkMod;
	public AbilityModPropertyBool m_canTriggerDeathmarkMod;
	[Separator("Dash - Allow move after evade?")]
	public AbilityModPropertyBool m_canQueueMoveAfterEvadeMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NinjaShurikenOrDash);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NinjaShurikenOrDash ninjaShurikenOrDash = targetAbility as NinjaShurikenOrDash;
		if (ninjaShurikenOrDash != null)
		{
			AddToken(tokens, m_dashRangeDefaultMod, "DashRangeDefault", string.Empty, ninjaShurikenOrDash.m_dashRangeDefault);
			AddToken(tokens, m_dashRangeMarkedMod, "DashRangeMarked", string.Empty, ninjaShurikenOrDash.m_dashRangeMarked);
			AddToken(tokens, m_dashToUnmarkedRangeMod, "DashToUnmarkedRange", string.Empty, ninjaShurikenOrDash.m_dashToUnmarkedRange);
			AddToken(tokens, m_dashDamageMod, "DashDamage", string.Empty, ninjaShurikenOrDash.m_dashDamage);
			AddToken(tokens, m_extraDamageOnMarkedMod, "ExtraDamageOnMarked", string.Empty, ninjaShurikenOrDash.m_extraDamageOnMarked);
			AddToken(tokens, m_extraDamageIfNotMarkedMod, "ExtraDamageIfNotMarked", string.Empty, ninjaShurikenOrDash.m_extraDamageIfNotMarked);
			AddToken_EffectMod(tokens, m_dashEnemyHitEffectMod, "DashEnemyHitEffect", ninjaShurikenOrDash.m_dashEnemyHitEffect);
			AddToken_EffectMod(tokens, m_extraEnemyEffectOnMarkedMod, "ExtraEnemyEffectOnMarked", ninjaShurikenOrDash.m_extraEnemyEffectOnMarked);
			AddToken(tokens, m_dashHealingMod, "DashHealing", string.Empty, ninjaShurikenOrDash.m_dashHealing);
			AddToken_EffectMod(tokens, m_dashAllyHitEffectMod, "DashAllyHitEffect", ninjaShurikenOrDash.m_dashAllyHitEffect);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)  // , Ability targetAbility in rogues
	{
		// reactor
		NinjaShurikenOrDash ninjaShurikenOrDash = GetTargetAbilityOnAbilityData(abilityData) as NinjaShurikenOrDash;
		// rogues
		// NinjaShurikenOrDash ninjaShurikenOrDash = targetAbility as NinjaShurikenOrDash;
		bool isValid = ninjaShurikenOrDash != null;
		string desc = string.Empty;
		desc += PropDesc(m_isTeleportMod, "[IsTeleport]", isValid, isValid && ninjaShurikenOrDash.m_isTeleport);
		desc += PropDesc(m_dashRangeDefaultMod, "[DashRangeDefault]", isValid, isValid ? ninjaShurikenOrDash.m_dashRangeDefault : 0f);
		desc += PropDesc(m_dashRangeMarkedMod, "[DashRangeMarked]", isValid, isValid ? ninjaShurikenOrDash.m_dashRangeMarked : 0f);
		desc += PropDesc(m_dashRequireDeathmarkMod, "[DashRequireDeathmark]", isValid, isValid && ninjaShurikenOrDash.m_dashRequireDeathmark);
		desc += PropDesc(m_dashToUnmarkedRangeMod, "[DashToUnmarkedRange]", isValid, isValid ? ninjaShurikenOrDash.m_dashToUnmarkedRange : 0f);
		desc += PropDesc(m_canDashToAllyMod, "[CanDashToAlly]", isValid, isValid && ninjaShurikenOrDash.m_canDashToAlly);
		desc += PropDesc(m_canDashToEnemyMod, "[CanDashToEnemy]", isValid, isValid && ninjaShurikenOrDash.m_canDashToEnemy);
		desc += PropDesc(m_dashIgnoreLosMod, "[DashIgnoreLos]", isValid, isValid && ninjaShurikenOrDash.m_dashIgnoreLos);
		desc += PropDesc(m_dashDestShapeMod, "[DashDestShape]", isValid, isValid ? ninjaShurikenOrDash.m_dashDestShape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_dashDamageMod, "[DashDamage]", isValid, isValid ? ninjaShurikenOrDash.m_dashDamage : 0);
		desc += PropDesc(m_extraDamageOnMarkedMod, "[ExtraDamageOnMarked]", isValid, isValid ? ninjaShurikenOrDash.m_extraDamageOnMarked : 0);
		desc += PropDesc(m_extraDamageIfNotMarkedMod, "[ExtraDamageIfNotMarked]", isValid, isValid ? ninjaShurikenOrDash.m_extraDamageIfNotMarked : 0);
		desc += PropDesc(m_dashEnemyHitEffectMod, "[DashEnemyHitEffect]", isValid, isValid ? ninjaShurikenOrDash.m_dashEnemyHitEffect : null);
		desc += PropDesc(m_extraEnemyEffectOnMarkedMod, "[ExtraEnemyEffectOnMarked]", isValid, isValid ? ninjaShurikenOrDash.m_extraEnemyEffectOnMarked : null);
		desc += PropDesc(m_dashHealingMod, "[DashHealing]", isValid, isValid ? ninjaShurikenOrDash.m_dashHealing : 0);
		desc += PropDesc(m_dashAllyHitEffectMod, "[DashAllyHitEffect]", isValid, isValid ? ninjaShurikenOrDash.m_dashAllyHitEffect : null);
		desc += PropDesc(m_dashApplyDeathmarkMod, "[DashApplyDeathmark]", isValid, isValid && ninjaShurikenOrDash.m_dashApplyDeathmark);
		desc += PropDesc(m_canTriggerDeathmarkMod, "[CanTriggerDeathmark]", isValid, isValid && ninjaShurikenOrDash.m_canTriggerDeathmark);
		return desc + PropDesc(m_canQueueMoveAfterEvadeMod, "[CanQueueMoveAfterEvade]", isValid, isValid && ninjaShurikenOrDash.m_canQueueMoveAfterEvade);
	}
}
