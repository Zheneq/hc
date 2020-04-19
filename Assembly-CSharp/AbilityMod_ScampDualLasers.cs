using System;
using System.Collections.Generic;
using AbilityContextNamespace;

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
		return this.m_defaultOnHitDataMod.\u001D(onHitDataFromBase);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ScampDualLasers scampDualLasers = targetAbility as ScampDualLasers;
		if (scampDualLasers != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ScampDualLasers.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			if (this.m_defaultOnHitDataMod != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_defaultOnHitDataMod.\u001D(tokens, scampDualLasers.m_onHitData);
			}
			if (this.m_shieldDownOnHitDataMod != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_shieldDownOnHitDataMod.\u001D(tokens, scampDualLasers.m_shieldDownOnHitData);
			}
			AbilityMod.AddToken(tokens, this.m_extraDamageTurnAfterLosingSuitMod, "ExtraDamageTurnAfterLosingSuit", string.Empty, scampDualLasers.m_extraDamageTurnAfterLosingSuit, true, false);
			AbilityMod.AddToken(tokens, this.m_extraAoeRadiusTurnAfterLosingSuitMod, "ExtraAoeRadiusTurnAfterLosingSuit", string.Empty, scampDualLasers.m_extraAoeRadiusTurnAfterLosingSuit, true, false, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScampDualLasers scampDualLasers = base.GetTargetAbilityOnAbilityData(abilityData) as ScampDualLasers;
		bool flag = scampDualLasers != null;
		string text = string.Empty;
		if (scampDualLasers != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ScampDualLasers.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			if (this.m_defaultOnHitDataMod != null)
			{
				string str = text;
				OnHitDataMod defaultOnHitDataMod = this.m_defaultOnHitDataMod;
				string u001D = "-- Default On Hit Data Mod --";
				OnHitAuthoredData u000E;
				if (scampDualLasers != null)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					u000E = scampDualLasers.m_onHitData;
				}
				else
				{
					u000E = null;
				}
				text = str + defaultOnHitDataMod.\u001D(u001D, u000E);
				string str2 = text;
				OnHitDataMod shieldDownOnHitDataMod = this.m_shieldDownOnHitDataMod;
				string u001D2 = "-- Shield Down On Hit Data Mod --";
				OnHitAuthoredData u000E2;
				if (scampDualLasers != null)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					u000E2 = scampDualLasers.m_shieldDownOnHitData;
				}
				else
				{
					u000E2 = null;
				}
				text = str2 + shieldDownOnHitDataMod.\u001D(u001D2, u000E2);
				text += this.m_defaultTargetSelectMod.GetInEditorDesc(scampDualLasers.m_targetSelectComp, "-- In Default Target Select --");
				text += this.m_shieldDownTargetSelectMod.GetInEditorDesc(scampDualLasers.m_shieldDownTargetSelect, "-- In Shield Down Target Select --");
			}
		}
		string str3 = text;
		AbilityModPropertyInt extraDamageTurnAfterLosingSuitMod = this.m_extraDamageTurnAfterLosingSuitMod;
		string prefix = "[ExtraDamageTurnAfterLosingSuit]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal = scampDualLasers.m_extraDamageTurnAfterLosingSuit;
		}
		else
		{
			baseVal = 0;
		}
		text = str3 + base.PropDesc(extraDamageTurnAfterLosingSuitMod, prefix, showBaseVal, baseVal);
		string str4 = text;
		AbilityModPropertyFloat extraAoeRadiusTurnAfterLosingSuitMod = this.m_extraAoeRadiusTurnAfterLosingSuitMod;
		string prefix2 = "[ExtraAoeRadiusTurnAfterLosingSuit]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = scampDualLasers.m_extraAoeRadiusTurnAfterLosingSuit;
		}
		else
		{
			baseVal2 = 0f;
		}
		return str4 + base.PropDesc(extraAoeRadiusTurnAfterLosingSuitMod, prefix2, showBaseVal2, baseVal2);
	}
}
