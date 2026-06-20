using System;
using System.Collections.Generic;
using AbilityContextNamespace;

public class AbilityMod_DinoTargetedKnockback : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod")]
	public TargetSelectMod_LaserTargetedPull m_targetSelMod;
	[Separator("Extra Damage, Shielding")]
	public AbilityModPropertyInt m_extraDamageIfFullPowerLayerConeMod;
	public AbilityModPropertyInt m_shieldPerEnemyHitMod;
	public AbilityModPropertyInt m_shieldDurationMod;
	[Separator("For hits around knockback destinations")]
	public AbilityModPropertyBool m_doHitsAroundKnockbackDestMod;
	public AbilityModPropertyShape m_hitsAroundKnockbackDestShapeMod;
	[Separator("On Hit Data Mod for knockback destination hit", "yellow")]
	public OnHitDataMod m_knockbackDestOnHitDataMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(DinoTargetedKnockback);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(m_targetSelMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		DinoTargetedKnockback dinoTargetedKnockback = targetAbility as DinoTargetedKnockback;
		if (dinoTargetedKnockback != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AddToken(tokens, m_extraDamageIfFullPowerLayerConeMod, "ExtraDamageIfFullPowerLayerCone", string.Empty, dinoTargetedKnockback.m_extraDamageIfFullPowerLayerCone);
			AddToken(tokens, m_shieldPerEnemyHitMod, "ShieldPerEnemyHit", string.Empty, dinoTargetedKnockback.m_shieldPerEnemyHit);
			AddToken(tokens, m_shieldDurationMod, "ShieldDuration", string.Empty, dinoTargetedKnockback.m_shieldDuration);
			AddOnHitDataTokens(tokens, m_knockbackDestOnHitDataMod, dinoTargetedKnockback.m_knockbackDestOnHitData);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		DinoTargetedKnockback dinoTargetedKnockback = GetTargetAbilityOnAbilityData(abilityData) as DinoTargetedKnockback;
		bool isValid = dinoTargetedKnockback != null;
		string desc = base.ModSpecificAutogenDesc(abilityData);
		if (dinoTargetedKnockback != null)
		{
			desc += GetTargetSelectModDesc(m_targetSelMod, dinoTargetedKnockback.m_targetSelectComp);
			desc += PropDesc(m_extraDamageIfFullPowerLayerConeMod, "[ExtraDamageIfFullPowerLayerCone]", isValid, isValid ? dinoTargetedKnockback.m_extraDamageIfFullPowerLayerCone : 0);
			desc += PropDesc(m_shieldPerEnemyHitMod, "[ShieldPerEnemyHit]", isValid, isValid ? dinoTargetedKnockback.m_shieldPerEnemyHit : 0);
			desc += PropDesc(m_shieldDurationMod, "[ShieldDuration]", isValid, isValid ? dinoTargetedKnockback.m_shieldDuration : 0);
			desc += PropDesc(m_doHitsAroundKnockbackDestMod, "[DoHitsAroundKnockbackDest]", isValid, isValid && dinoTargetedKnockback.m_doHitsAroundKnockbackDest);
			desc += PropDesc(m_hitsAroundKnockbackDestShapeMod, "[HitsAroundKnockbackDestShape]", isValid, isValid ? dinoTargetedKnockback.m_hitsAroundKnockbackDestShape : AbilityAreaShape.SingleSquare);
			desc += GetOnHitDataDesc(m_knockbackDestOnHitDataMod, dinoTargetedKnockback.m_knockbackDestOnHitData);
		}
		return desc;
	}
}
