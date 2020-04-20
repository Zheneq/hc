using System;
using UnityEngine;

[Serializable]
public class TargetSelectMod_ConeOrLaser : TargetSelectModBase
{
	[Separator("Targeting Properties", true)]
	public AbilityModPropertyFloat m_coneDistThresholdMod;

	[Header("  Targeting: For Cone")]
	public AbilityModPropertyConeInfo m_coneInfoMod;

	[Header("  Targeting: For Laser")]
	public AbilityModPropertyLaserInfo m_laserInfoMod;

	public override string GetModSpecificInEditorDesc(GenericAbility_TargetSelectBase targetSelectBase, string header)
	{
		string text = string.Empty;
		TargetSelect_ConeOrLaser targetSelect_ConeOrLaser = targetSelectBase as TargetSelect_ConeOrLaser;
		if (targetSelect_ConeOrLaser != null)
		{
			text += AbilityModHelper.GetModPropertyDesc(this.m_coneDistThresholdMod, "[ConeDistThreshold]", true, targetSelect_ConeOrLaser.m_coneDistThreshold);
			text += AbilityModHelper.GetModPropertyDesc(this.m_coneInfoMod, "[ConeInfo]", true, targetSelect_ConeOrLaser.m_coneInfo);
			text += AbilityModHelper.GetModPropertyDesc(this.m_laserInfoMod, "[LaserInfo]", true, targetSelect_ConeOrLaser.m_laserInfo);
		}
		return text;
	}
}
