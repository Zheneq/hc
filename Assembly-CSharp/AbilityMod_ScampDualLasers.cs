using System;
using System.Collections.Generic;
using System.Text;
using AbilityContextNamespace;

public class AbilityMod_ScampDualLasers : AbilityMod
{
	[Separator("On Hit Data - In Suit", "yellow")]
	public OnHitDataMod m_defaultOnHitDataMod;
	[Separator("On Hit Data - No Suit", "yellow")]
	public OnHitDataMod m_shieldDownOnHitDataMod;
	[Separator("Target Select Mod - In Suit")]
	public TargetSelectMod_DualMeetingLasers m_defaultTargetSelectMod;
	[Separator("Target Select Mod - No Suit")]
	public TargetSelectMod_DualMeetingLasers m_shieldDownTargetSelectMod;
	[Separator("Extra Damage and Aoe Radius for turn after losing suit")]
	public AbilityModPropertyInt m_extraDamageTurnAfterLosingSuitMod;
	public AbilityModPropertyFloat m_extraAoeRadiusTurnAfterLosingSuitMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ScampDualLasers);
	}

	public override OnHitAuthoredData GenModImpl_GetModdedOnHitData(OnHitAuthoredData onHitDataFromBase)
	{
		return m_defaultOnHitDataMod.GetModdedOnHitData(onHitDataFromBase);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ScampDualLasers scampDualLasers = targetAbility as ScampDualLasers;
		if (scampDualLasers != null)
		{
			m_defaultOnHitDataMod?.AddTooltipTokens(tokens, scampDualLasers.m_onHitData);
			m_shieldDownOnHitDataMod?.AddTooltipTokens(tokens, scampDualLasers.m_shieldDownOnHitData);
			AddToken(tokens, m_extraDamageTurnAfterLosingSuitMod, "ExtraDamageTurnAfterLosingSuit", string.Empty, scampDualLasers.m_extraDamageTurnAfterLosingSuit);
			AddToken(tokens, m_extraAoeRadiusTurnAfterLosingSuitMod, "ExtraAoeRadiusTurnAfterLosingSuit", string.Empty, scampDualLasers.m_extraAoeRadiusTurnAfterLosingSuit);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScampDualLasers scampDualLasers = GetTargetAbilityOnAbilityData(abilityData) as ScampDualLasers;
		bool isValid = scampDualLasers != null;
		string desc = string.Empty;
		if (scampDualLasers != null && m_defaultOnHitDataMod != null)
		{
			desc += m_defaultOnHitDataMod.GetInEditorDesc("-- Default On Hit Data Mod --", scampDualLasers != null ? scampDualLasers.m_onHitData : null);
			desc += m_shieldDownOnHitDataMod.GetInEditorDesc("-- Shield Down On Hit Data Mod --", scampDualLasers != null ? scampDualLasers.m_shieldDownOnHitData : null);
			desc += m_defaultTargetSelectMod.GetInEditorDesc(scampDualLasers.m_targetSelectComp, "-- In Default Target Select --");
			desc += m_shieldDownTargetSelectMod.GetInEditorDesc(scampDualLasers.m_shieldDownTargetSelect, "-- In Shield Down Target Select --");
		}

		desc += PropDesc(m_extraDamageTurnAfterLosingSuitMod, "[ExtraDamageTurnAfterLosingSuit]", isValid, isValid ? scampDualLasers.m_extraDamageTurnAfterLosingSuit : 0);
		return new StringBuilder().Append(desc).Append(PropDesc(m_extraAoeRadiusTurnAfterLosingSuitMod, "[ExtraAoeRadiusTurnAfterLosingSuit]", isValid, isValid ? scampDualLasers.m_extraAoeRadiusTurnAfterLosingSuit : 0f)).ToString();
	}
}
