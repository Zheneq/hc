using System;
using System.Collections.Generic;
using System.Text;

public class AbilityMod_IceborgSelfShield : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod")]
	public TargetSelectMod_Shape m_targetSelectMod;
	[Separator("Health to be considered low health if below")]
	public AbilityModPropertyInt m_lowHealthThreshMod;
	public bool m_lowHealthUseStatusOverride;
	public List<StatusType> m_lowHealthStatusWhenRequested;
	[Separator("Shield if all shield depleted on first turn")]
	public AbilityModPropertyInt m_shieldOnNextTurnIfDepletedMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(IceborgSelfShield);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		IceborgSelfShield iceborgSelfShield = targetAbility as IceborgSelfShield;
		if (iceborgSelfShield != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AddToken(tokens, m_lowHealthThreshMod, "LowHealthThresh", string.Empty, iceborgSelfShield.m_lowHealthThresh);
			AddToken(tokens, m_shieldOnNextTurnIfDepletedMod, "ShieldOnNextTurnIfDepleted", string.Empty, iceborgSelfShield.m_shieldOnNextTurnIfDepleted);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		IceborgSelfShield iceborgSelfShield = GetTargetAbilityOnAbilityData(abilityData) as IceborgSelfShield;
		bool isValid = iceborgSelfShield != null;
		string desc = base.ModSpecificAutogenDesc(abilityData);
		if (isValid)
		{
			desc += GetTargetSelectModDesc(m_targetSelectMod, iceborgSelfShield.m_targetSelectComp, "-- Target Select --");
			desc += PropDesc(m_lowHealthThreshMod, "[LowHealthThresh]", isValid, isValid ? iceborgSelfShield.m_lowHealthThresh : 0);
			if (m_lowHealthUseStatusOverride && m_lowHealthStatusWhenRequested != null)
			{
				desc += "Status to apply when ability is requested, if low health:\n";
				foreach (StatusType statusType in m_lowHealthStatusWhenRequested)
				{
					desc += new StringBuilder().Append("\t").Append(statusType).Append("\n").ToString();
				}
			}
			desc += PropDesc(m_shieldOnNextTurnIfDepletedMod, "[ShieldOnNextTurnIfDepleted]", isValid, isValid ? iceborgSelfShield.m_shieldOnNextTurnIfDepleted : 0);
		}
		return desc;
	}
}
