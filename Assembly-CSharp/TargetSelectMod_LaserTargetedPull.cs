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
			while (true)
			{
				switch (6)
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
			text += AbilityModHelper.GetModPropertyDesc(m_laserRangeMod, "[LaserRange]", true, targetSelect_LaserTargetedPull.m_laserRange);
			text += AbilityModHelper.GetModPropertyDesc(m_laserWidthMod, "[LaserWidth]", true, targetSelect_LaserTargetedPull.m_laserWidth);
			text += AbilityModHelper.GetModPropertyDesc(m_maxTargetsMod, "[MaxTargets]", true, targetSelect_LaserTargetedPull.m_maxTargets);
			text += AbilityModHelper.GetModPropertyDesc(m_maxKnockbackDistMod, "[MaxKnockbackDist]", true, targetSelect_LaserTargetedPull.m_maxKnockbackDist);
			text += AbilityModHelper.GetModPropertyDesc(m_squareRangeFromCasterMod, "[SquareRangeFromCaster]", true, targetSelect_LaserTargetedPull.m_squareRangeFromCaster);
			text += AbilityModHelper.GetModPropertyDesc(m_destinationAngleDegWithBackMod, "[DestinationAngleDegWithBack]", true, targetSelect_LaserTargetedPull.m_destinationAngleDegWithBack);
		}
		return text;
	}
}
