using System;
using UnityEngine;

[Serializable]
public class TargetSelectMod_LaserChargeWithReverseCones : TargetSelectModBase
{
	[Separator("Targeting Properties", true)]
	public AbilityModPropertyFloat m_laserRangeMod;

	public AbilityModPropertyFloat m_laserWidthMod;

	[Header("Cone Properties")]
	public AbilityModPropertyConeInfo m_coneInfoMod;

	[Space(10f)]
	public AbilityModPropertyInt m_coneCountMod;

	public AbilityModPropertyFloat m_coneStartOffsetMod;

	public AbilityModPropertyFloat m_perConeHorizontalOffsetMod;

	public AbilityModPropertyFloat m_angleInBetweenMod;

	public override string GetModSpecificInEditorDesc(GenericAbility_TargetSelectBase targetSelectBase, string header)
	{
		string text = string.Empty;
		TargetSelect_LaserChargeWithReverseCones targetSelect_LaserChargeWithReverseCones = targetSelectBase as TargetSelect_LaserChargeWithReverseCones;
		if (targetSelect_LaserChargeWithReverseCones != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelectMod_LaserChargeWithReverseCones.GetModSpecificInEditorDesc(GenericAbility_TargetSelectBase, string)).MethodHandle;
			}
			text += AbilityModHelper.GetModPropertyDesc(this.m_laserRangeMod, "[LaserRange]", true, targetSelect_LaserChargeWithReverseCones.m_laserRange);
			text += AbilityModHelper.GetModPropertyDesc(this.m_laserWidthMod, "[LaserWidth]", true, targetSelect_LaserChargeWithReverseCones.m_laserWidth);
			text += AbilityModHelper.GetModPropertyDesc(this.m_coneInfoMod, "[ConeInfo]", true, targetSelect_LaserChargeWithReverseCones.m_coneInfo);
			text += AbilityModHelper.GetModPropertyDesc(this.m_coneCountMod, "[ConeCount]", true, targetSelect_LaserChargeWithReverseCones.m_coneCount);
			text += AbilityModHelper.GetModPropertyDesc(this.m_coneStartOffsetMod, "[ConeStartOffset]", true, targetSelect_LaserChargeWithReverseCones.m_coneStartOffset);
			text += AbilityModHelper.GetModPropertyDesc(this.m_perConeHorizontalOffsetMod, "[PerConeHorizontalOffset]", true, targetSelect_LaserChargeWithReverseCones.m_perConeHorizontalOffset);
			text += AbilityModHelper.GetModPropertyDesc(this.m_angleInBetweenMod, "[AngleInBetween]", true, targetSelect_LaserChargeWithReverseCones.m_angleInBetween);
		}
		return text;
	}
}
