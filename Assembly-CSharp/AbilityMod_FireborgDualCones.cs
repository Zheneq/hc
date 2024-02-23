using System;
using System.Collections.Generic;
using System.Text;
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
			text += GetTargetSelectModDesc(m_targetSelectMod, fireborgDualCones.m_targetSelectComp, "-- Target Select --");
			text += PropDesc(m_extraDamageIfOverlapMod, "[ExtraDamageIfOverlap]", flag, flag ? fireborgDualCones.m_extraDamageIfOverlap : 0);
			string str = text;
			AbilityModPropertyInt extraDamageNonOverlapMod = m_extraDamageNonOverlapMod;
			int baseVal;
			if (flag)
			{
				baseVal = fireborgDualCones.m_extraDamageNonOverlap;
			}
			else
			{
				baseVal = 0;
			}

			text = new StringBuilder().Append(str).Append(PropDesc(extraDamageNonOverlapMod, "[ExtraDamageNonOverlap]", flag, baseVal)).ToString();
			text += PropDesc(m_igniteTargetIfOverlapHitMod, "[IgniteTargetIfOverlapHit]", flag, flag && fireborgDualCones.m_igniteTargetIfOverlapHit);
			string str2 = text;
			AbilityModPropertyBool igniteTargetIfSuperheatedMod = m_igniteTargetIfSuperheatedMod;
			int baseVal2;
			if (flag)
			{
				baseVal2 = (fireborgDualCones.m_igniteTargetIfSuperheated ? 1 : 0);
			}
			else
			{
				baseVal2 = 0;
			}

			text = new StringBuilder().Append(str2).Append(PropDesc(igniteTargetIfSuperheatedMod, "[IgniteTargetIfSuperheated]", flag, (byte)baseVal2 != 0)).ToString();
			string str3 = text;
			AbilityModPropertyBool groundFireOnAllIfNormalMod = m_groundFireOnAllIfNormalMod;
			int baseVal3;
			if (flag)
			{
				baseVal3 = (fireborgDualCones.m_groundFireOnAllIfNormal ? 1 : 0);
			}
			else
			{
				baseVal3 = 0;
			}

			text = new StringBuilder().Append(str3).Append(PropDesc(groundFireOnAllIfNormalMod, "[GroundFireOnAllIfNormal]", flag, (byte)baseVal3 != 0)).ToString();
			string str4 = text;
			AbilityModPropertyBool groundFireOnOverlapIfNormalMod = m_groundFireOnOverlapIfNormalMod;
			int baseVal4;
			if (flag)
			{
				baseVal4 = (fireborgDualCones.m_groundFireOnOverlapIfNormal ? 1 : 0);
			}
			else
			{
				baseVal4 = 0;
			}

			text = new StringBuilder().Append(str4).Append(PropDesc(groundFireOnOverlapIfNormalMod, "[GroundFireOnOverlapIfNormal]", flag, (byte)baseVal4 != 0)).ToString();
			string str5 = text;
			AbilityModPropertyBool groundFireOnAllIfSuperheatedMod = m_groundFireOnAllIfSuperheatedMod;
			int baseVal5;
			if (flag)
			{
				baseVal5 = (fireborgDualCones.m_groundFireOnAllIfSuperheated ? 1 : 0);
			}
			else
			{
				baseVal5 = 0;
			}

			text = new StringBuilder().Append(str5).Append(PropDesc(groundFireOnAllIfSuperheatedMod, "[GroundFireOnAllIfSuperheated]", flag, (byte)baseVal5 != 0)).ToString();
			string str6 = text;
			AbilityModPropertyBool groundFireOnOverlapIfSuperheatedMod = m_groundFireOnOverlapIfSuperheatedMod;
			int baseVal6;
			if (flag)
			{
				baseVal6 = (fireborgDualCones.m_groundFireOnOverlapIfSuperheated ? 1 : 0);
			}
			else
			{
				baseVal6 = 0;
			}

			text = new StringBuilder().Append(str6).Append(PropDesc(groundFireOnOverlapIfSuperheatedMod, "[GroundFireOnOverlapIfSuperheated]", flag, (byte)baseVal6 != 0)).ToString();
		}
		return text;
	}
}
