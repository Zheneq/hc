using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_FireborgDualCones : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod", true)]
	public TargetSelectMod_FanCones m_targetSelectMod;

	[Separator("Extra Damage for overlap state", true)]
	public AbilityModPropertyInt m_extraDamageIfOverlapMod;

	public AbilityModPropertyInt m_extraDamageNonOverlapMod;

	[Separator("Add Ignited Effect If Overlap Hit", true)]
	public AbilityModPropertyBool m_igniteTargetIfOverlapHitMod;

	public AbilityModPropertyBool m_igniteTargetIfSuperheatedMod;

	[Separator("Ground Fire", true)]
	public AbilityModPropertyBool m_groundFireOnAllIfNormalMod;

	public AbilityModPropertyBool m_groundFireOnOverlapIfNormalMod;

	[Space(10f)]
	public AbilityModPropertyBool m_groundFireOnAllIfSuperheatedMod;

	public AbilityModPropertyBool m_groundFireOnOverlapIfSuperheatedMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(FireborgDualCones);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FireborgDualCones fireborgDualCones = targetAbility as FireborgDualCones;
		if (fireborgDualCones != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, m_extraDamageIfOverlapMod, "ExtraDamageIfOverlap", string.Empty, fireborgDualCones.m_extraDamageIfOverlap);
			AbilityMod.AddToken(tokens, m_extraDamageNonOverlapMod, "ExtraDamageNonOverlap", string.Empty, fireborgDualCones.m_extraDamageNonOverlap);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FireborgDualCones fireborgDualCones = GetTargetAbilityOnAbilityData(abilityData) as FireborgDualCones;
		bool flag = fireborgDualCones != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (fireborgDualCones != null)
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
			text += GetTargetSelectModDesc(m_targetSelectMod, fireborgDualCones.m_targetSelectComp, "-- Target Select --");
			text += PropDesc(m_extraDamageIfOverlapMod, "[ExtraDamageIfOverlap]", flag, flag ? fireborgDualCones.m_extraDamageIfOverlap : 0);
			string str = text;
			AbilityModPropertyInt extraDamageNonOverlapMod = m_extraDamageNonOverlapMod;
			int baseVal;
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
				baseVal = fireborgDualCones.m_extraDamageNonOverlap;
			}
			else
			{
				baseVal = 0;
			}
			text = str + PropDesc(extraDamageNonOverlapMod, "[ExtraDamageNonOverlap]", flag, baseVal);
			text += PropDesc(m_igniteTargetIfOverlapHitMod, "[IgniteTargetIfOverlapHit]", flag, flag && fireborgDualCones.m_igniteTargetIfOverlapHit);
			string str2 = text;
			AbilityModPropertyBool igniteTargetIfSuperheatedMod = m_igniteTargetIfSuperheatedMod;
			int baseVal2;
			if (flag)
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
				baseVal2 = (fireborgDualCones.m_igniteTargetIfSuperheated ? 1 : 0);
			}
			else
			{
				baseVal2 = 0;
			}
			text = str2 + PropDesc(igniteTargetIfSuperheatedMod, "[IgniteTargetIfSuperheated]", flag, (byte)baseVal2 != 0);
			string str3 = text;
			AbilityModPropertyBool groundFireOnAllIfNormalMod = m_groundFireOnAllIfNormalMod;
			int baseVal3;
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
				baseVal3 = (fireborgDualCones.m_groundFireOnAllIfNormal ? 1 : 0);
			}
			else
			{
				baseVal3 = 0;
			}
			text = str3 + PropDesc(groundFireOnAllIfNormalMod, "[GroundFireOnAllIfNormal]", flag, (byte)baseVal3 != 0);
			string str4 = text;
			AbilityModPropertyBool groundFireOnOverlapIfNormalMod = m_groundFireOnOverlapIfNormalMod;
			int baseVal4;
			if (flag)
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
				baseVal4 = (fireborgDualCones.m_groundFireOnOverlapIfNormal ? 1 : 0);
			}
			else
			{
				baseVal4 = 0;
			}
			text = str4 + PropDesc(groundFireOnOverlapIfNormalMod, "[GroundFireOnOverlapIfNormal]", flag, (byte)baseVal4 != 0);
			string str5 = text;
			AbilityModPropertyBool groundFireOnAllIfSuperheatedMod = m_groundFireOnAllIfSuperheatedMod;
			int baseVal5;
			if (flag)
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
				baseVal5 = (fireborgDualCones.m_groundFireOnAllIfSuperheated ? 1 : 0);
			}
			else
			{
				baseVal5 = 0;
			}
			text = str5 + PropDesc(groundFireOnAllIfSuperheatedMod, "[GroundFireOnAllIfSuperheated]", flag, (byte)baseVal5 != 0);
			string str6 = text;
			AbilityModPropertyBool groundFireOnOverlapIfSuperheatedMod = m_groundFireOnOverlapIfSuperheatedMod;
			int baseVal6;
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
				baseVal6 = (fireborgDualCones.m_groundFireOnOverlapIfSuperheated ? 1 : 0);
			}
			else
			{
				baseVal6 = 0;
			}
			text = str6 + PropDesc(groundFireOnOverlapIfSuperheatedMod, "[GroundFireOnOverlapIfSuperheated]", flag, (byte)baseVal6 != 0);
		}
		return text;
	}
}
