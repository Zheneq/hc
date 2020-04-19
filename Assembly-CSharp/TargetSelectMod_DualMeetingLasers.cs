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
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat laserWidthMod = this.m_laserWidthMod;
		string prefix = "[LaserWidth]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelectMod_DualMeetingLasers.GetModSpecificInEditorDesc(GenericAbility_TargetSelectBase, string)).MethodHandle;
			}
			baseVal = targetSelect_DualMeetingLasers.m_laserWidth;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(laserWidthMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat minMeetingDistFromCasterMod = this.m_minMeetingDistFromCasterMod;
		string prefix2 = "[MinMeetingDistFromCaster]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
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
			baseVal2 = targetSelect_DualMeetingLasers.m_minMeetingDistFromCaster;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(minMeetingDistFromCasterMod, prefix2, showBaseVal2, baseVal2);
		text += AbilityModHelper.GetModPropertyDesc(this.m_maxMeetingDistFromCasterMod, "[MaxMeetingDistFromCaster]", flag, (!flag) ? 0f : targetSelect_DualMeetingLasers.m_maxMeetingDistFromCaster);
		string str3 = text;
		AbilityModPropertyFloat laserStartForwardOffsetMod = this.m_laserStartForwardOffsetMod;
		string prefix3 = "[LaserStartForwardOffset]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
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
			baseVal3 = targetSelect_DualMeetingLasers.m_laserStartForwardOffset;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(laserStartForwardOffsetMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyFloat laserStartSideOffsetMod = this.m_laserStartSideOffsetMod;
		string prefix4 = "[LaserStartSideOffset]";
		bool showBaseVal4 = flag;
		float baseVal4;
		if (flag)
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
			baseVal4 = targetSelect_DualMeetingLasers.m_laserStartSideOffset;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(laserStartSideOffsetMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyFloat aoeBaseRadiusMod = this.m_aoeBaseRadiusMod;
		string prefix5 = "[AoeBaseRadius]";
		bool showBaseVal5 = flag;
		float baseVal5;
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
			baseVal5 = targetSelect_DualMeetingLasers.m_aoeBaseRadius;
		}
		else
		{
			baseVal5 = 0f;
		}
		text = str5 + AbilityModHelper.GetModPropertyDesc(aoeBaseRadiusMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyFloat aoeMinRadiusMod = this.m_aoeMinRadiusMod;
		string prefix6 = "[AoeMinRadius]";
		bool showBaseVal6 = flag;
		float baseVal6;
		if (flag)
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
			baseVal6 = targetSelect_DualMeetingLasers.m_aoeMinRadius;
		}
		else
		{
			baseVal6 = 0f;
		}
		text = str6 + AbilityModHelper.GetModPropertyDesc(aoeMinRadiusMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyFloat aoeMaxRadiusMod = this.m_aoeMaxRadiusMod;
		string prefix7 = "[AoeMaxRadius]";
		bool showBaseVal7 = flag;
		float baseVal7;
		if (flag)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal7 = targetSelect_DualMeetingLasers.m_aoeMaxRadius;
		}
		else
		{
			baseVal7 = 0f;
		}
		text = str7 + AbilityModHelper.GetModPropertyDesc(aoeMaxRadiusMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyFloat aoeRadiusChangePerUnitFromMinMod = this.m_aoeRadiusChangePerUnitFromMinMod;
		string prefix8 = "[AoeRadiusChangePerUnitFromMin]";
		bool showBaseVal8 = flag;
		float baseVal8;
		if (flag)
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
			baseVal8 = targetSelect_DualMeetingLasers.m_aoeRadiusChangePerUnitFromMin;
		}
		else
		{
			baseVal8 = 0f;
		}
		text = str8 + AbilityModHelper.GetModPropertyDesc(aoeRadiusChangePerUnitFromMinMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyFloat radiusMultIfPartialBlockMod = this.m_radiusMultIfPartialBlockMod;
		string prefix9 = "[RadiusMultIfPartialBlock]";
		bool showBaseVal9 = flag;
		float baseVal9;
		if (flag)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal9 = targetSelect_DualMeetingLasers.m_radiusMultIfPartialBlock;
		}
		else
		{
			baseVal9 = 0f;
		}
		text = str9 + AbilityModHelper.GetModPropertyDesc(radiusMultIfPartialBlockMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyBool aoeIgnoreMinCoverDistMod = this.m_aoeIgnoreMinCoverDistMod;
		string prefix10 = "[AoeIgnoreMinCoverDist]";
		bool showBaseVal10 = flag;
		bool baseVal10;
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
			baseVal10 = targetSelect_DualMeetingLasers.m_aoeIgnoreMinCoverDist;
		}
		else
		{
			baseVal10 = false;
		}
		return str10 + AbilityModHelper.GetModPropertyDesc(aoeIgnoreMinCoverDistMod, prefix10, showBaseVal10, baseVal10);
	}
}
