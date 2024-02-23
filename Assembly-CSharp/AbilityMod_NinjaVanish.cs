using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_NinjaVanish : AbilityMod
{
	[Separator("Evade/Move settings")]
	public AbilityModPropertyBool m_canQueueMoveAfterEvadeMod;
	[Header("-- Whether to skip dash (if want to be a combat ability, etc) --")]
	public AbilityModPropertyBool m_skipEvadeMod;
	[Separator("Self Hit - Effects / Heal on Turn Start")]
	public AbilityModPropertyEffectInfo m_effectOnSelfMod;
	public AbilityModPropertyEffectInfo m_selfEffectOnNextTurnMod;
	[Header("-- Heal on Self on next turn start if inside field --")]
	public AbilityModPropertyInt m_selfHealOnTurnStartIfInFieldMod;
	[Separator("Initial Cast Hit On Enemy")]
	public AbilityModPropertyEffectInfo m_effectOnEnemyMod;
	[Separator("Duration for barrier and ground effect")]
	public AbilityModPropertyInt m_smokeFieldDurationMod;
	[Separator("Vision Blocking Barrier")]
	public AbilityModPropertyFloat m_barrierWidthMod;
	public AbilityModPropertyBarrierDataV2 m_visionBlockBarrierDataMod;
	[Separator("Ground Effect")]
	public AbilityModPropertyGroundEffectField m_groundEffectDataMod;
	[Separator("Cooldown Reduction if only ability used in turn")]
	public AbilityModPropertyInt m_cdrIfOnlyAbilityUsedMod;
	public AbilityModPropertyBool m_cdrConsiderCatalystMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NinjaVanish);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NinjaVanish ninjaVanish = targetAbility as NinjaVanish;
		if (ninjaVanish != null)
		{
			AddToken_EffectMod(tokens, m_effectOnSelfMod, "EffectOnSelf", ninjaVanish.m_effectOnSelf);
			AddToken_EffectMod(tokens, m_selfEffectOnNextTurnMod, "SelfEffectOnNextTurn", ninjaVanish.m_selfEffectOnNextTurn);
			AddToken(tokens, m_selfHealOnTurnStartIfInFieldMod, "SelfHealOnTurnStartIfInField", string.Empty, ninjaVanish.m_selfHealOnTurnStartIfInField);
			AddToken_EffectMod(tokens, m_effectOnEnemyMod, "EffectOnEnemy", ninjaVanish.m_effectOnEnemy);
			AddToken(tokens, m_smokeFieldDurationMod, "SmokeFieldDuration", string.Empty, ninjaVanish.m_smokeFieldDuration);
			AddToken(tokens, m_barrierWidthMod, "BarrierWidth", string.Empty, ninjaVanish.m_barrierWidth);
			AddToken_BarrierMod(tokens, m_visionBlockBarrierDataMod, "VisionBlockBarrierData", ninjaVanish.m_visionBlockBarrierData);
			AddToken_GroundFieldMod(tokens, m_groundEffectDataMod, "GroundEffectData", ninjaVanish.m_groundEffectData);
			AddToken(tokens, m_cdrIfOnlyAbilityUsedMod, "CdrIfOnlyAbilityUsed", string.Empty, ninjaVanish.m_cdrIfOnlyAbilityUsed);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NinjaVanish ninjaVanish = GetTargetAbilityOnAbilityData(abilityData) as NinjaVanish;
		bool isValid = ninjaVanish != null;
		string desc = string.Empty;
		desc += PropDesc(m_canQueueMoveAfterEvadeMod, "[CanQueueMoveAfterEvade]", isValid, isValid && ninjaVanish.m_canQueueMoveAfterEvade);
		desc += PropDesc(m_skipEvadeMod, "[SkipEvade]", isValid, isValid && ninjaVanish.m_skipEvade);
		desc += PropDesc(m_effectOnSelfMod, "[EffectOnSelf]", isValid, isValid ? ninjaVanish.m_effectOnSelf : null);
		desc += PropDesc(m_selfEffectOnNextTurnMod, "[SelfEffectOnNextTurn]", isValid, isValid ? ninjaVanish.m_selfEffectOnNextTurn : null);
		desc += PropDesc(m_selfHealOnTurnStartIfInFieldMod, "[SelfHealOnTurnStartIfInField]", isValid, isValid ? ninjaVanish.m_selfHealOnTurnStartIfInField : 0);
		desc += PropDesc(m_effectOnEnemyMod, "[EffectOnEnemy]", isValid, isValid ? ninjaVanish.m_effectOnEnemy : null);
		desc += PropDesc(m_smokeFieldDurationMod, "[SmokeFieldDuration]", isValid, isValid ? ninjaVanish.m_smokeFieldDuration : 0);
		desc += PropDesc(m_barrierWidthMod, "[BarrierWidth]", isValid, isValid ? ninjaVanish.m_barrierWidth : 0f);
		desc += PropDescBarrierMod(m_visionBlockBarrierDataMod, "{ VisionBlockBarrierData }", ninjaVanish.m_visionBlockBarrierData);
		desc += PropDescGroundFieldMod(m_groundEffectDataMod, "{ GroundEffectData }", ninjaVanish.m_groundEffectData);
		desc += PropDesc(m_cdrIfOnlyAbilityUsedMod, "[CdrIfOnlyAbilityUsed]", isValid, isValid ? ninjaVanish.m_cdrIfOnlyAbilityUsed : 0);
		return new StringBuilder().Append(desc).Append(PropDesc(m_cdrConsiderCatalystMod, "[CdrConsiderCatalyst]", isValid, isValid && ninjaVanish.m_cdrConsiderCatalyst)).ToString();
	}
}
