using System;
using UnityEngine;

[Serializable]
public class TargetSelectMod_FanCones : TargetSelectModBase
{
	[Separator("Targeting Properties", true)]
	public AbilityModPropertyConeInfo m_coneInfoMod;

	[Space(10f)]
	public AbilityModPropertyInt m_coneCountMod;

	[Header("Starting offset, move towards forward/aim direction")]
	public AbilityModPropertyFloat m_coneStartOffsetInAimDirMod;

	[Header("Starting offset, move towards left/right")]
	public AbilityModPropertyFloat m_coneStartOffsetToSidesMod;

	[Header("Starting offset, move towards each cone's direction")]
	public AbilityModPropertyFloat m_coneStartOffsetInConeDirMod;

	[Header("-- If Fixed Angle")]
	public AbilityModPropertyFloat m_angleInBetweenMod;

	[Header("-- If Interpolating Angle")]
	public AbilityModPropertyBool m_changeAngleByCursorDistanceMod;

	public AbilityModPropertyFloat m_targeterMinAngleMod;

	public AbilityModPropertyFloat m_targeterMaxAngleMod;

	public AbilityModPropertyFloat m_startAngleOffsetMod;

	public override string GetModSpecificInEditorDesc(GenericAbility_TargetSelectBase targetSelectBase, string header)
	{
		string text = string.Empty;
		TargetSelect_FanCones targetSelect_FanCones = targetSelectBase as TargetSelect_FanCones;
		if (targetSelect_FanCones != null)
		{
			text += AbilityModHelper.GetModPropertyDesc(m_coneInfoMod, "[ConeInfo]", true, targetSelect_FanCones.m_coneInfo);
			text += AbilityModHelper.GetModPropertyDesc(m_coneCountMod, "[ConeCount]", true, targetSelect_FanCones.m_coneCount);
			text += AbilityModHelper.GetModPropertyDesc(m_coneStartOffsetInAimDirMod, "[ConeStartOffsetInAimDir]", true, targetSelect_FanCones.m_coneStartOffsetInAimDir);
			text += AbilityModHelper.GetModPropertyDesc(m_coneStartOffsetToSidesMod, "[ConeStartOffsetToSides]", true, targetSelect_FanCones.m_coneStartOffsetToSides);
			text += AbilityModHelper.GetModPropertyDesc(m_coneStartOffsetInConeDirMod, "[ConeStartOffsetInConeDir]", true, targetSelect_FanCones.m_coneStartOffsetInConeDir);
			text += AbilityModHelper.GetModPropertyDesc(m_angleInBetweenMod, "[AngleInBetween]", true, targetSelect_FanCones.m_angleInBetween);
			text += AbilityModHelper.GetModPropertyDesc(m_changeAngleByCursorDistanceMod, "[ChangeAngleByCursorDistance]", true, targetSelect_FanCones.m_changeAngleByCursorDistance);
			text += AbilityModHelper.GetModPropertyDesc(m_targeterMinAngleMod, "[TargeterMinAngle]", true, targetSelect_FanCones.m_targeterMinAngle);
			text += AbilityModHelper.GetModPropertyDesc(m_targeterMaxAngleMod, "[TargeterMaxAngle]", true, targetSelect_FanCones.m_targeterMaxAngle);
			text += AbilityModHelper.GetModPropertyDesc(m_startAngleOffsetMod, "[StartAngleOffset]", true, targetSelect_FanCones.m_startAngleOffset);
		}
		return text;
	}
}
