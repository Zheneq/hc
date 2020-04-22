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
		targetSelect.SetTargetSelectMod(m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		IceborgSelfShield iceborgSelfShield = targetAbility as IceborgSelfShield;
		if (!(iceborgSelfShield != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, m_lowHealthThreshMod, "LowHealthThresh", string.Empty, iceborgSelfShield.m_lowHealthThresh);
			AbilityMod.AddToken(tokens, m_shieldOnNextTurnIfDepletedMod, "ShieldOnNextTurnIfDepleted", string.Empty, iceborgSelfShield.m_shieldOnNextTurnIfDepleted);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		IceborgSelfShield iceborgSelfShield = GetTargetAbilityOnAbilityData(abilityData) as IceborgSelfShield;
		bool flag = iceborgSelfShield != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (iceborgSelfShield != null)
		{
			while (true)
			{
				switch (3)
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
			text += GetTargetSelectModDesc(m_targetSelectMod, iceborgSelfShield.m_targetSelectComp, "-- Target Select --");
			string str = text;
			AbilityModPropertyInt lowHealthThreshMod = m_lowHealthThreshMod;
			int baseVal;
			if (flag)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				baseVal = iceborgSelfShield.m_lowHealthThresh;
			}
			else
			{
				baseVal = 0;
			}
			text = str + PropDesc(lowHealthThreshMod, "[LowHealthThresh]", flag, baseVal);
			if (m_lowHealthUseStatusOverride)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_lowHealthStatusWhenRequested != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					text += "Status to apply when ability is requested, if low health:\n";
					using (List<StatusType>.Enumerator enumerator = m_lowHealthStatusWhenRequested.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							text = text + "\t" + enumerator.Current.ToString() + "\n";
						}
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
			}
			string str2 = text;
			AbilityModPropertyInt shieldOnNextTurnIfDepletedMod = m_shieldOnNextTurnIfDepletedMod;
			int baseVal2;
			if (flag)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				baseVal2 = iceborgSelfShield.m_shieldOnNextTurnIfDepleted;
			}
			else
			{
				baseVal2 = 0;
			}
			text = str2 + PropDesc(shieldOnNextTurnIfDepletedMod, "[ShieldOnNextTurnIfDepleted]", flag, baseVal2);
		}
		return text;
	}
}
