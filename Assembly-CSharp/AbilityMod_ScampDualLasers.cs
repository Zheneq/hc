using AbilityContextNamespace;
using System;
using System.Collections.Generic;

public class AbilityMod_ScampDualLasers : AbilityMod
{
	[Separator("On Hit Data - In Suit", "yellow")]
	public OnHitDataMod m_defaultOnHitDataMod;

	[Separator("On Hit Data - No Suit", "yellow")]
	public OnHitDataMod m_shieldDownOnHitDataMod;

	[Separator("Target Select Mod - In Suit", true)]
	public TargetSelectMod_DualMeetingLasers m_defaultTargetSelectMod;

	[Separator("Target Select Mod - No Suit", true)]
	public TargetSelectMod_DualMeetingLasers m_shieldDownTargetSelectMod;

	[Separator("Extra Damage and Aoe Radius for turn after losing suit", true)]
	public AbilityModPropertyInt m_extraDamageTurnAfterLosingSuitMod;

	public AbilityModPropertyFloat m_extraAoeRadiusTurnAfterLosingSuitMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ScampDualLasers);
	}

	public override OnHitAuthoredData GenModImpl_GetModdedOnHitData(OnHitAuthoredData onHitDataFromBase)
	{
		return m_defaultOnHitDataMod._001D(onHitDataFromBase);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ScampDualLasers scampDualLasers = targetAbility as ScampDualLasers;
		if (!(scampDualLasers != null))
		{
			return;
		}
		while (true)
		{
			if (m_defaultOnHitDataMod != null)
			{
				m_defaultOnHitDataMod._001D(tokens, scampDualLasers.m_onHitData);
			}
			if (m_shieldDownOnHitDataMod != null)
			{
				m_shieldDownOnHitDataMod._001D(tokens, scampDualLasers.m_shieldDownOnHitData);
			}
			AbilityMod.AddToken(tokens, m_extraDamageTurnAfterLosingSuitMod, "ExtraDamageTurnAfterLosingSuit", string.Empty, scampDualLasers.m_extraDamageTurnAfterLosingSuit);
			AbilityMod.AddToken(tokens, m_extraAoeRadiusTurnAfterLosingSuitMod, "ExtraAoeRadiusTurnAfterLosingSuit", string.Empty, scampDualLasers.m_extraAoeRadiusTurnAfterLosingSuit);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScampDualLasers scampDualLasers = GetTargetAbilityOnAbilityData(abilityData) as ScampDualLasers;
		bool flag = scampDualLasers != null;
		string text = string.Empty;
		if (scampDualLasers != null)
		{
			if (m_defaultOnHitDataMod != null)
			{
				string str = text;
				OnHitDataMod defaultOnHitDataMod = m_defaultOnHitDataMod;
				object obj;
				if (scampDualLasers != null)
				{
					obj = scampDualLasers.m_onHitData;
				}
				else
				{
					obj = null;
				}
				text = str + defaultOnHitDataMod._001D("-- Default On Hit Data Mod --", (OnHitAuthoredData)obj);
				string str2 = text;
				OnHitDataMod shieldDownOnHitDataMod = m_shieldDownOnHitDataMod;
				object obj2;
				if (scampDualLasers != null)
				{
					obj2 = scampDualLasers.m_shieldDownOnHitData;
				}
				else
				{
					obj2 = null;
				}
				text = str2 + shieldDownOnHitDataMod._001D("-- Shield Down On Hit Data Mod --", (OnHitAuthoredData)obj2);
				text += m_defaultTargetSelectMod.GetInEditorDesc(scampDualLasers.m_targetSelectComp, "-- In Default Target Select --");
				text += m_shieldDownTargetSelectMod.GetInEditorDesc(scampDualLasers.m_shieldDownTargetSelect, "-- In Shield Down Target Select --");
			}
		}
		string str3 = text;
		AbilityModPropertyInt extraDamageTurnAfterLosingSuitMod = m_extraDamageTurnAfterLosingSuitMod;
		int baseVal;
		if (flag)
		{
			baseVal = scampDualLasers.m_extraDamageTurnAfterLosingSuit;
		}
		else
		{
			baseVal = 0;
		}
		text = str3 + PropDesc(extraDamageTurnAfterLosingSuitMod, "[ExtraDamageTurnAfterLosingSuit]", flag, baseVal);
		string str4 = text;
		AbilityModPropertyFloat extraAoeRadiusTurnAfterLosingSuitMod = m_extraAoeRadiusTurnAfterLosingSuitMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = scampDualLasers.m_extraAoeRadiusTurnAfterLosingSuit;
		}
		else
		{
			baseVal2 = 0f;
		}
		return str4 + PropDesc(extraAoeRadiusTurnAfterLosingSuitMod, "[ExtraAoeRadiusTurnAfterLosingSuit]", flag, baseVal2);
	}
}
