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
			while (true)
			{
				switch (2)
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
			text += AbilityModHelper.GetModPropertyDesc(m_coneDistThresholdMod, "[ConeDistThreshold]", true, targetSelect_ConeOrLaser.m_coneDistThreshold);
			text += AbilityModHelper.GetModPropertyDesc(m_coneInfoMod, "[ConeInfo]", true, targetSelect_ConeOrLaser.m_coneInfo);
			text += AbilityModHelper.GetModPropertyDesc(m_laserInfoMod, "[LaserInfo]", true, targetSelect_ConeOrLaser.m_laserInfo);
		}
		return text;
	}
}
