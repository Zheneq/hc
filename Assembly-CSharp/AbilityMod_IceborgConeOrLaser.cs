using System;
using System.Collections.Generic;

public class AbilityMod_IceborgConeOrLaser : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod")]
	public TargetSelectMod_ConeOrLaser m_targetSelectMod;
	[Separator("Shielding per enemy hit on cast")]
	public AbilityModPropertyInt m_shieldPerEnemyHitMod;
	public AbilityModPropertyInt m_shieldDurationMod;
	[Separator("Apply Nova effect?")]
	public AbilityModPropertyBool m_applyDelayedAoeEffectMod;
	public AbilityModPropertyBool m_skipDelayedAoeEffectIfHasExistingMod;
	[Separator("Cdr Per Hit Enemy with Nova Core")]
	public AbilityModPropertyInt m_cdrPerEnemyWithNovaCoreMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(IceborgConeOrLaser);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		IceborgConeOrLaser iceborgConeOrLaser = targetAbility as IceborgConeOrLaser;
		if (iceborgConeOrLaser != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AddToken(tokens, m_shieldPerEnemyHitMod, "ShieldPerEnemyHit", string.Empty, iceborgConeOrLaser.m_shieldPerEnemyHit);
			AddToken(tokens, m_shieldDurationMod, "ShieldDuration", string.Empty, iceborgConeOrLaser.m_shieldDuration);
			AddToken(tokens, m_cdrPerEnemyWithNovaCoreMod, "CdrPerEnemyWithNovaCore", string.Empty, iceborgConeOrLaser.m_cdrPerEnemyWithNovaCore);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		IceborgConeOrLaser iceborgConeOrLaser = GetTargetAbilityOnAbilityData(abilityData) as IceborgConeOrLaser;
		bool isValid = iceborgConeOrLaser != null;
		string desc = base.ModSpecificAutogenDesc(abilityData);
		if (isValid)
		{
			desc += GetTargetSelectModDesc(m_targetSelectMod, iceborgConeOrLaser.m_targetSelectComp, "-- Target Select --");
			desc += PropDesc(m_shieldPerEnemyHitMod, "[ShieldPerEnemyHit]", isValid, isValid ? iceborgConeOrLaser.m_shieldPerEnemyHit : 0);
			desc += PropDesc(m_shieldDurationMod, "[ShieldDuration]", isValid, isValid ? iceborgConeOrLaser.m_shieldDuration : 0);
			desc += PropDesc(m_applyDelayedAoeEffectMod, "[ApplyDelayedAoeEffect]", isValid, isValid && iceborgConeOrLaser.m_applyDelayedAoeEffect);
			desc += PropDesc(m_skipDelayedAoeEffectIfHasExistingMod, "[SkipDelayedAoeEffectIfHasExisting]", isValid, isValid && iceborgConeOrLaser.m_skipDelayedAoeEffectIfHasExisting);
			desc += PropDesc(m_cdrPerEnemyWithNovaCoreMod, "[CdrPerEnemyWithNovaCore]", isValid, isValid ? iceborgConeOrLaser.m_cdrPerEnemyWithNovaCore : 0);
		}
		return desc;
	}
}
