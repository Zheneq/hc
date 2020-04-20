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
		targetSelect.SetTargetSelectMod(this.m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FireborgDualCones fireborgDualCones = targetAbility as FireborgDualCones;
		if (fireborgDualCones != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, this.m_extraDamageIfOverlapMod, "ExtraDamageIfOverlap", string.Empty, fireborgDualCones.m_extraDamageIfOverlap, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageNonOverlapMod, "ExtraDamageNonOverlap", string.Empty, fireborgDualCones.m_extraDamageNonOverlap, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FireborgDualCones fireborgDualCones = base.GetTargetAbilityOnAbilityData(abilityData) as FireborgDualCones;
		bool flag = fireborgDualCones != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (fireborgDualCones != null)
		{
			text += base.GetTargetSelectModDesc(this.m_targetSelectMod, fireborgDualCones.m_targetSelectComp, "-- Target Select --");
			text += base.PropDesc(this.m_extraDamageIfOverlapMod, "[ExtraDamageIfOverlap]", flag, (!flag) ? 0 : fireborgDualCones.m_extraDamageIfOverlap);
			string str = text;
			AbilityModPropertyInt extraDamageNonOverlapMod = this.m_extraDamageNonOverlapMod;
			string prefix = "[ExtraDamageNonOverlap]";
			bool showBaseVal = flag;
			int baseVal;
			if (flag)
			{
				baseVal = fireborgDualCones.m_extraDamageNonOverlap;
			}
			else
			{
				baseVal = 0;
			}
			text = str + base.PropDesc(extraDamageNonOverlapMod, prefix, showBaseVal, baseVal);
			text += base.PropDesc(this.m_igniteTargetIfOverlapHitMod, "[IgniteTargetIfOverlapHit]", flag, flag && fireborgDualCones.m_igniteTargetIfOverlapHit);
			string str2 = text;
			AbilityModPropertyBool igniteTargetIfSuperheatedMod = this.m_igniteTargetIfSuperheatedMod;
			string prefix2 = "[IgniteTargetIfSuperheated]";
			bool showBaseVal2 = flag;
			bool baseVal2;
			if (flag)
			{
				baseVal2 = fireborgDualCones.m_igniteTargetIfSuperheated;
			}
			else
			{
				baseVal2 = false;
			}
			text = str2 + base.PropDesc(igniteTargetIfSuperheatedMod, prefix2, showBaseVal2, baseVal2);
			string str3 = text;
			AbilityModPropertyBool groundFireOnAllIfNormalMod = this.m_groundFireOnAllIfNormalMod;
			string prefix3 = "[GroundFireOnAllIfNormal]";
			bool showBaseVal3 = flag;
			bool baseVal3;
			if (flag)
			{
				baseVal3 = fireborgDualCones.m_groundFireOnAllIfNormal;
			}
			else
			{
				baseVal3 = false;
			}
			text = str3 + base.PropDesc(groundFireOnAllIfNormalMod, prefix3, showBaseVal3, baseVal3);
			string str4 = text;
			AbilityModPropertyBool groundFireOnOverlapIfNormalMod = this.m_groundFireOnOverlapIfNormalMod;
			string prefix4 = "[GroundFireOnOverlapIfNormal]";
			bool showBaseVal4 = flag;
			bool baseVal4;
			if (flag)
			{
				baseVal4 = fireborgDualCones.m_groundFireOnOverlapIfNormal;
			}
			else
			{
				baseVal4 = false;
			}
			text = str4 + base.PropDesc(groundFireOnOverlapIfNormalMod, prefix4, showBaseVal4, baseVal4);
			string str5 = text;
			AbilityModPropertyBool groundFireOnAllIfSuperheatedMod = this.m_groundFireOnAllIfSuperheatedMod;
			string prefix5 = "[GroundFireOnAllIfSuperheated]";
			bool showBaseVal5 = flag;
			bool baseVal5;
			if (flag)
			{
				baseVal5 = fireborgDualCones.m_groundFireOnAllIfSuperheated;
			}
			else
			{
				baseVal5 = false;
			}
			text = str5 + base.PropDesc(groundFireOnAllIfSuperheatedMod, prefix5, showBaseVal5, baseVal5);
			string str6 = text;
			AbilityModPropertyBool groundFireOnOverlapIfSuperheatedMod = this.m_groundFireOnOverlapIfSuperheatedMod;
			string prefix6 = "[GroundFireOnOverlapIfSuperheated]";
			bool showBaseVal6 = flag;
			bool baseVal6;
			if (flag)
			{
				baseVal6 = fireborgDualCones.m_groundFireOnOverlapIfSuperheated;
			}
			else
			{
				baseVal6 = false;
			}
			text = str6 + base.PropDesc(groundFireOnOverlapIfSuperheatedMod, prefix6, showBaseVal6, baseVal6);
		}
		return text;
	}
}
