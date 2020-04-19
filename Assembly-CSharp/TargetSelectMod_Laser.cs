using System;

[Serializable]
public class TargetSelectMod_Laser : TargetSelectModBase
{
	[Separator("Targeting Properties", true)]
	public AbilityModPropertyFloat m_laserRangeMod;

	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	[Separator("AoE around start", true)]
	public AbilityModPropertyFloat m_aoeRadiusAroundStartMod;

	public override string GetModSpecificInEditorDesc(GenericAbility_TargetSelectBase targetSelectBase, string header)
	{
		string text = string.Empty;
		TargetSelect_Laser targetSelect_Laser = targetSelectBase as TargetSelect_Laser;
		if (targetSelect_Laser != null)
		{
			text += AbilityModHelper.GetModPropertyDesc(this.m_laserRangeMod, "[LaserRange]", true, targetSelect_Laser.m_laserRange);
			text += AbilityModHelper.GetModPropertyDesc(this.m_laserWidthMod, "[LaserWidth]", true, targetSelect_Laser.m_laserWidth);
			text += AbilityModHelper.GetModPropertyDesc(this.m_maxTargetsMod, "[MaxTargets]", true, targetSelect_Laser.m_maxTargets);
			text += AbilityModHelper.GetModPropertyDesc(this.m_aoeRadiusAroundStartMod, "[AoeRadiusAroundStart]", true, targetSelect_Laser.m_aoeRadiusAroundStart);
		}
		return text;
	}
}
