using System;
using System.Collections.Generic;

public class AbilityMod_IceborgDamageArea : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod")]
	public TargetSelectMod_Shape m_targetSelectMod;
	[Separator("Targeting, Max Ranges")]
	public AbilityModPropertyFloat m_initialCastMaxRangeMod;
	public AbilityModPropertyFloat m_moveAreaCastMaxRangeMod;
	public AbilityModPropertyBool m_targetingAreaCheckLosMod;
	[Separator("Whether to add damage field")]
	public AbilityModPropertyBool m_addGroundFieldMod;
	public AbilityModPropertyBool m_stopMoversWithSlowStatusMod;
	public AbilityModPropertyBool m_stopMoverIfHitPreviousTurnMod;
	public AbilityModPropertyGroundEffectField m_groundFieldDataMod;
	[Separator("Extra Damage on Initial Cast")]
	public AbilityModPropertyInt m_extraDamageOnInitialCastMod;
	[Separator("Damage change on ground field per turn")]
	public AbilityModPropertyInt m_groundFieldDamageChangePerTurnMod;
	[Separator("Min Damage")]
	public AbilityModPropertyInt m_minDamageMod;
	[Separator("Shielding per enemy hit on cast")]
	public AbilityModPropertyInt m_shieldPerEnemyHitMod;
	public AbilityModPropertyInt m_shieldDurationMod;
	[Separator("Effect to apply if target has been hit by this ability on previous turn")]
	public AbilityModPropertyEffectInfo m_effectOnEnemyIfHitPreviousTurnMod;
	[Separator("Apply Nova effect?")]
	public AbilityModPropertyBool m_applyDelayedAoeEffectMod;
	public AbilityModPropertyBool m_applyNovaCoreIfHitPreviousTurnMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(IceborgDamageArea);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		IceborgDamageArea iceborgDamageArea = targetAbility as IceborgDamageArea;
		if (iceborgDamageArea != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AddToken(tokens, m_initialCastMaxRangeMod, "InitialCastMaxRange", string.Empty, iceborgDamageArea.m_initialCastMaxRange);
			AddToken(tokens, m_moveAreaCastMaxRangeMod, "MoveAreaCastMaxRange", string.Empty, iceborgDamageArea.m_moveAreaCastMaxRange);
			AddToken_GroundFieldMod(tokens, m_groundFieldDataMod, "GroundFieldData", iceborgDamageArea.m_groundFieldData);
			AddToken(tokens, m_extraDamageOnInitialCastMod, "ExtraDamageOnInitialCast", string.Empty, iceborgDamageArea.m_extraDamageOnInitialCast);
			AddToken(tokens, m_groundFieldDamageChangePerTurnMod, "GroundFieldDamageChangePerTurn", string.Empty, iceborgDamageArea.m_groundFieldDamageChangePerTurn);
			AddToken(tokens, m_minDamageMod, "MinDamage", string.Empty, iceborgDamageArea.m_minDamage);
			AddToken(tokens, m_shieldPerEnemyHitMod, "ShieldPerEnemyHit", string.Empty, iceborgDamageArea.m_shieldPerEnemyHit);
			AddToken(tokens, m_shieldDurationMod, "ShieldDuration", string.Empty, iceborgDamageArea.m_shieldDuration);
			AddToken_EffectMod(tokens, m_effectOnEnemyIfHitPreviousTurnMod, "EffectOnEnemyIfHitPreviousTurn", iceborgDamageArea.m_effectOnEnemyIfHitPreviousTurn);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		IceborgDamageArea iceborgDamageArea = GetTargetAbilityOnAbilityData(abilityData) as IceborgDamageArea;
		bool isValid = iceborgDamageArea != null;
		string desc = base.ModSpecificAutogenDesc(abilityData);
		if (isValid)
		{
			desc += GetTargetSelectModDesc(m_targetSelectMod, iceborgDamageArea.m_targetSelectComp, "-- Target Select --");
			desc += PropDesc(m_initialCastMaxRangeMod, "[InitialCastMaxRange]", isValid, isValid ? iceborgDamageArea.m_initialCastMaxRange : 0f);
			desc += PropDesc(m_moveAreaCastMaxRangeMod, "[MoveAreaCastMaxRange]", isValid, isValid ? iceborgDamageArea.m_moveAreaCastMaxRange : 0f);
			desc += PropDesc(m_targetingAreaCheckLosMod, "[TargetingAreaCheckLos]", isValid, isValid && iceborgDamageArea.m_targetingAreaCheckLos);
			desc += PropDesc(m_addGroundFieldMod, "[AddGroundField]", isValid, isValid && iceborgDamageArea.m_addGroundField);
			desc += PropDesc(m_stopMoversWithSlowStatusMod, "[StopMoversWithSlowStatus]", isValid, isValid && iceborgDamageArea.m_stopMoversWithSlowStatus);
			desc += PropDesc(m_stopMoverIfHitPreviousTurnMod, "[StopMoverIfHitPreviousTurn]", isValid, isValid && iceborgDamageArea.m_stopMoverIfHitPreviousTurn);
			desc += PropDescGroundFieldMod(m_groundFieldDataMod, "{ GroundFieldData }", iceborgDamageArea.m_groundFieldData);
			desc += PropDesc(m_extraDamageOnInitialCastMod, "[ExtraDamageOnInitialCast]", isValid, isValid ? iceborgDamageArea.m_extraDamageOnInitialCast : 0);
			desc += PropDesc(m_groundFieldDamageChangePerTurnMod, "[GroundFieldDamageChangePerTurn]", isValid, isValid ? iceborgDamageArea.m_groundFieldDamageChangePerTurn : 0);
			desc += PropDesc(m_minDamageMod, "[MinDamage]", isValid, isValid ? iceborgDamageArea.m_minDamage : 0);
			desc += PropDesc(m_shieldPerEnemyHitMod, "[ShieldPerEnemyHit]", isValid, isValid ? iceborgDamageArea.m_shieldPerEnemyHit : 0);
			desc += PropDesc(m_shieldDurationMod, "[ShieldDuration]", isValid, isValid ? iceborgDamageArea.m_shieldDuration : 0);
			desc += PropDesc(m_effectOnEnemyIfHitPreviousTurnMod, "[EffectOnEnemyIfHitPreviousTurn]", isValid, isValid ? iceborgDamageArea.m_effectOnEnemyIfHitPreviousTurn : null);
			desc += PropDesc(m_applyDelayedAoeEffectMod, "[ApplyDelayedAoeEffect]", isValid, isValid && iceborgDamageArea.m_applyDelayedAoeEffect);
			desc += PropDesc(m_applyNovaCoreIfHitPreviousTurnMod, "[ApplyNovaCoreIfHitPreviousTurn]", isValid, isValid && iceborgDamageArea.m_applyNovaCoreIfHitPreviousTurn);
		}
		return desc;
	}
}
