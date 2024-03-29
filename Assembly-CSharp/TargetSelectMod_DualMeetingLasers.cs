using System;
using UnityEngine;

[Serializable]
public class TargetSelectMod_DualMeetingLasers : TargetSelectModBase
{
	[Header("-- Targeting - Laser")]
	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyFloat m_minMeetingDistFromCasterMod;

	public AbilityModPropertyFloat m_maxMeetingDistFromCasterMod;

	public AbilityModPropertyFloat m_laserStartForwardOffsetMod;

	public AbilityModPropertyFloat m_laserStartSideOffsetMod;

	[Header("-- Targeting - AoE")]
	public AbilityModPropertyFloat m_aoeBaseRadiusMod;

	public AbilityModPropertyFloat m_aoeMinRadiusMod;

	public AbilityModPropertyFloat m_aoeMaxRadiusMod;

	public AbilityModPropertyFloat m_aoeRadiusChangePerUnitFromMinMod;

	[Header("-- Multiplier on radius if not all lasers meet")]
	public AbilityModPropertyFloat m_radiusMultIfPartialBlockMod;

	[Space(10f)]
	public AbilityModPropertyBool m_aoeIgnoreMinCoverDistMod;

	public override string GetModSpecificInEditorDesc(GenericAbility_TargetSelectBase targetSelectBase, string header)
	{
		TargetSelect_DualMeetingLasers targetSelect_DualMeetingLasers = targetSelectBase as TargetSelect_DualMeetingLasers;
		bool flag = targetSelect_DualMeetingLasers != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat laserWidthMod = m_laserWidthMod;
		float baseVal;
		if (flag)
		{
			baseVal = targetSelect_DualMeetingLasers.m_laserWidth;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(laserWidthMod, "[LaserWidth]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat minMeetingDistFromCasterMod = m_minMeetingDistFromCasterMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = targetSelect_DualMeetingLasers.m_minMeetingDistFromCaster;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(minMeetingDistFromCasterMod, "[MinMeetingDistFromCaster]", flag, baseVal2);
		empty += AbilityModHelper.GetModPropertyDesc(m_maxMeetingDistFromCasterMod, "[MaxMeetingDistFromCaster]", flag, (!flag) ? 0f : targetSelect_DualMeetingLasers.m_maxMeetingDistFromCaster);
		string str3 = empty;
		AbilityModPropertyFloat laserStartForwardOffsetMod = m_laserStartForwardOffsetMod;
		float baseVal3;
		if (flag)
		{
			baseVal3 = targetSelect_DualMeetingLasers.m_laserStartForwardOffset;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(laserStartForwardOffsetMod, "[LaserStartForwardOffset]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyFloat laserStartSideOffsetMod = m_laserStartSideOffsetMod;
		float baseVal4;
		if (flag)
		{
			baseVal4 = targetSelect_DualMeetingLasers.m_laserStartSideOffset;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(laserStartSideOffsetMod, "[LaserStartSideOffset]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyFloat aoeBaseRadiusMod = m_aoeBaseRadiusMod;
		float baseVal5;
		if (flag)
		{
			baseVal5 = targetSelect_DualMeetingLasers.m_aoeBaseRadius;
		}
		else
		{
			baseVal5 = 0f;
		}
		empty = str5 + AbilityModHelper.GetModPropertyDesc(aoeBaseRadiusMod, "[AoeBaseRadius]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyFloat aoeMinRadiusMod = m_aoeMinRadiusMod;
		float baseVal6;
		if (flag)
		{
			baseVal6 = targetSelect_DualMeetingLasers.m_aoeMinRadius;
		}
		else
		{
			baseVal6 = 0f;
		}
		empty = str6 + AbilityModHelper.GetModPropertyDesc(aoeMinRadiusMod, "[AoeMinRadius]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyFloat aoeMaxRadiusMod = m_aoeMaxRadiusMod;
		float baseVal7;
		if (flag)
		{
			baseVal7 = targetSelect_DualMeetingLasers.m_aoeMaxRadius;
		}
		else
		{
			baseVal7 = 0f;
		}
		empty = str7 + AbilityModHelper.GetModPropertyDesc(aoeMaxRadiusMod, "[AoeMaxRadius]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyFloat aoeRadiusChangePerUnitFromMinMod = m_aoeRadiusChangePerUnitFromMinMod;
		float baseVal8;
		if (flag)
		{
			baseVal8 = targetSelect_DualMeetingLasers.m_aoeRadiusChangePerUnitFromMin;
		}
		else
		{
			baseVal8 = 0f;
		}
		empty = str8 + AbilityModHelper.GetModPropertyDesc(aoeRadiusChangePerUnitFromMinMod, "[AoeRadiusChangePerUnitFromMin]", flag, baseVal8);
		string str9 = empty;
		AbilityModPropertyFloat radiusMultIfPartialBlockMod = m_radiusMultIfPartialBlockMod;
		float baseVal9;
		if (flag)
		{
			baseVal9 = targetSelect_DualMeetingLasers.m_radiusMultIfPartialBlock;
		}
		else
		{
			baseVal9 = 0f;
		}
		empty = str9 + AbilityModHelper.GetModPropertyDesc(radiusMultIfPartialBlockMod, "[RadiusMultIfPartialBlock]", flag, baseVal9);
		string str10 = empty;
		AbilityModPropertyBool aoeIgnoreMinCoverDistMod = m_aoeIgnoreMinCoverDistMod;
		int baseVal10;
		if (flag)
		{
			baseVal10 = (targetSelect_DualMeetingLasers.m_aoeIgnoreMinCoverDist ? 1 : 0);
		}
		else
		{
			baseVal10 = 0;
		}
		return str10 + AbilityModHelper.GetModPropertyDesc(aoeIgnoreMinCoverDistMod, "[AoeIgnoreMinCoverDist]", flag, (byte)baseVal10 != 0);
	}
}
