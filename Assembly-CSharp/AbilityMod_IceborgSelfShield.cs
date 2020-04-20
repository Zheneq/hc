using System;
using System.Collections.Generic;

public class AbilityMod_IceborgSelfShield : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod", true)]
	public TargetSelectMod_Shape m_targetSelectMod;

	[Separator("Health to be considered low health if below", true)]
	public AbilityModPropertyInt m_lowHealthThreshMod;

	public bool m_lowHealthUseStatusOverride;

	public List<StatusType> m_lowHealthStatusWhenRequested;

	[Separator("Shield if all shield depleted on first turn", true)]
	public AbilityModPropertyInt m_shieldOnNextTurnIfDepletedMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(IceborgSelfShield);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(this.m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		IceborgSelfShield iceborgSelfShield = targetAbility as IceborgSelfShield;
		if (iceborgSelfShield != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, this.m_lowHealthThreshMod, "LowHealthThresh", string.Empty, iceborgSelfShield.m_lowHealthThresh, true, false);
			AbilityMod.AddToken(tokens, this.m_shieldOnNextTurnIfDepletedMod, "ShieldOnNextTurnIfDepleted", string.Empty, iceborgSelfShield.m_shieldOnNextTurnIfDepleted, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		IceborgSelfShield iceborgSelfShield = base.GetTargetAbilityOnAbilityData(abilityData) as IceborgSelfShield;
		bool flag = iceborgSelfShield != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (iceborgSelfShield != null)
		{
			text += base.GetTargetSelectModDesc(this.m_targetSelectMod, iceborgSelfShield.m_targetSelectComp, "-- Target Select --");
			string str = text;
			AbilityModPropertyInt lowHealthThreshMod = this.m_lowHealthThreshMod;
			string prefix = "[LowHealthThresh]";
			bool showBaseVal = flag;
			int baseVal;
			if (flag)
			{
				baseVal = iceborgSelfShield.m_lowHealthThresh;
			}
			else
			{
				baseVal = 0;
			}
			text = str + base.PropDesc(lowHealthThreshMod, prefix, showBaseVal, baseVal);
			if (this.m_lowHealthUseStatusOverride)
			{
				if (this.m_lowHealthStatusWhenRequested != null)
				{
					text += "Status to apply when ability is requested, if low health:\n";
					using (List<StatusType>.Enumerator enumerator = this.m_lowHealthStatusWhenRequested.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							StatusType statusType = enumerator.Current;
							text = text + "\t" + statusType.ToString() + "\n";
						}
					}
				}
			}
			string str2 = text;
			AbilityModPropertyInt shieldOnNextTurnIfDepletedMod = this.m_shieldOnNextTurnIfDepletedMod;
			string prefix2 = "[ShieldOnNextTurnIfDepleted]";
			bool showBaseVal2 = flag;
			int baseVal2;
			if (flag)
			{
				baseVal2 = iceborgSelfShield.m_shieldOnNextTurnIfDepleted;
			}
			else
			{
				baseVal2 = 0;
			}
			text = str2 + base.PropDesc(shieldOnNextTurnIfDepletedMod, prefix2, showBaseVal2, baseVal2);
		}
		return text;
	}
}
