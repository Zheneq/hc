using System;
using System.Collections.Generic;

public class AbilityMod_DinoLayerCones : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod", true)]
	public TargetSelectMod_LayerCones m_targetSelectMod;

	[Separator("Power Level", true)]
	public AbilityModPropertyInt m_powerLevelAdjustIfNoInnerHitsMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(DinoLayerCones);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(this.m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		DinoLayerCones dinoLayerCones = targetAbility as DinoLayerCones;
		if (dinoLayerCones != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, this.m_powerLevelAdjustIfNoInnerHitsMod, "PowerLevelAdjustIfNoInnerHits", string.Empty, dinoLayerCones.m_powerLevelAdjustIfNoInnerHits, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		DinoLayerCones dinoLayerCones = base.GetTargetAbilityOnAbilityData(abilityData) as DinoLayerCones;
		bool flag = dinoLayerCones != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (dinoLayerCones != null)
		{
			text += base.GetTargetSelectModDesc(this.m_targetSelectMod, dinoLayerCones.m_targetSelectComp, "-- Target Select Mod --");
			text += base.PropDesc(this.m_powerLevelAdjustIfNoInnerHitsMod, "[PowerLevelAdjustIfNoInnerHits]", flag, (!flag) ? 0 : dinoLayerCones.m_powerLevelAdjustIfNoInnerHits);
		}
		return text;
	}
}
