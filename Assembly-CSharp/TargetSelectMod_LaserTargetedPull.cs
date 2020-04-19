using System;

[Serializable]
public class TargetSelectMod_LaserTargetedPull : TargetSelectModBase
{
	[Separator("Targeting Properties", true)]
	public AbilityModPropertyFloat m_laserRangeMod;

	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	public AbilityModPropertyFloat m_maxKnockbackDistMod;

	[Separator("For Pull Destination", true)]
	public AbilityModPropertyFloat m_squareRangeFromCasterMod;

	public AbilityModPropertyFloat m_destinationAngleDegWithBackMod;

	public override string GetModSpecificInEditorDesc(GenericAbility_TargetSelectBase targetSelectBase, string header)
	{
		string text = string.Empty;
		TargetSelect_LaserTargetedPull targetSelect_LaserTargetedPull = targetSelectBase as TargetSelect_LaserTargetedPull;
		if (targetSelect_LaserTargetedPull != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelectMod_LaserTargetedPull.GetModSpecificInEditorDesc(GenericAbility_TargetSelectBase, string)).MethodHandle;
			}
			text += AbilityModHelper.GetModPropertyDesc(this.m_laserRangeMod, "[LaserRange]", true, targetSelect_LaserTargetedPull.m_laserRange);
			text += AbilityModHelper.GetModPropertyDesc(this.m_laserWidthMod, "[LaserWidth]", true, targetSelect_LaserTargetedPull.m_laserWidth);
			text += AbilityModHelper.GetModPropertyDesc(this.m_maxTargetsMod, "[MaxTargets]", true, targetSelect_LaserTargetedPull.m_maxTargets);
			text += AbilityModHelper.GetModPropertyDesc(this.m_maxKnockbackDistMod, "[MaxKnockbackDist]", true, targetSelect_LaserTargetedPull.m_maxKnockbackDist);
			text += AbilityModHelper.GetModPropertyDesc(this.m_squareRangeFromCasterMod, "[SquareRangeFromCaster]", true, targetSelect_LaserTargetedPull.m_squareRangeFromCaster);
			text += AbilityModHelper.GetModPropertyDesc(this.m_destinationAngleDegWithBackMod, "[DestinationAngleDegWithBack]", true, targetSelect_LaserTargetedPull.m_destinationAngleDegWithBack);
		}
		return text;
	}
}
