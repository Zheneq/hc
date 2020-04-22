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
		targetSelect.SetTargetSelectMod(m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		DinoLayerCones dinoLayerCones = targetAbility as DinoLayerCones;
		if (!(dinoLayerCones != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, m_powerLevelAdjustIfNoInnerHitsMod, "PowerLevelAdjustIfNoInnerHits", string.Empty, dinoLayerCones.m_powerLevelAdjustIfNoInnerHits);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		DinoLayerCones dinoLayerCones = GetTargetAbilityOnAbilityData(abilityData) as DinoLayerCones;
		bool flag = dinoLayerCones != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (dinoLayerCones != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			text += GetTargetSelectModDesc(m_targetSelectMod, dinoLayerCones.m_targetSelectComp);
			text += PropDesc(m_powerLevelAdjustIfNoInnerHitsMod, "[PowerLevelAdjustIfNoInnerHits]", flag, flag ? dinoLayerCones.m_powerLevelAdjustIfNoInnerHits : 0);
		}
		return text;
	}
}
