using System;
using System.Collections.Generic;

public class AbilityMod_DinoLayerCones : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod")]
	public TargetSelectMod_LayerCones m_targetSelectMod;
	[Separator("Power Level")]
	public AbilityModPropertyInt m_powerLevelAdjustIfNoInnerHitsMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(DinoLayerCones);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		DinoLayerCones dinoLayerCones = targetAbility as DinoLayerCones;
		if (dinoLayerCones != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AddToken(tokens, m_powerLevelAdjustIfNoInnerHitsMod, "PowerLevelAdjustIfNoInnerHits", string.Empty, dinoLayerCones.m_powerLevelAdjustIfNoInnerHits);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		DinoLayerCones dinoLayerCones = GetTargetAbilityOnAbilityData(abilityData) as DinoLayerCones;
		bool isValid = dinoLayerCones != null;
		string desc = base.ModSpecificAutogenDesc(abilityData);
		if (dinoLayerCones != null)
		{
			desc += GetTargetSelectModDesc(m_targetSelectMod, dinoLayerCones.m_targetSelectComp);
			desc += PropDesc(m_powerLevelAdjustIfNoInnerHitsMod, "[PowerLevelAdjustIfNoInnerHits]", isValid, isValid ? dinoLayerCones.m_powerLevelAdjustIfNoInnerHits : 0);
		}
		return desc;
	}
}
