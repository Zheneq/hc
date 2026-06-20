using System;

[Serializable]
public class TargetSelectMod_Cone : TargetSelectModBase
{
	[Separator("Input Params", true)]
	public AbilityModPropertyConeInfo m_coneInfoMod;

	public override string GetModSpecificInEditorDesc(GenericAbility_TargetSelectBase targetSelectBase, string header)
	{
		string text = string.Empty;
		TargetSelect_Cone targetSelect_Cone = targetSelectBase as TargetSelect_Cone;
		if (targetSelect_Cone != null)
		{
			text += AbilityModHelper.GetModPropertyDesc(m_coneInfoMod, "[ConeInfo]", true, targetSelect_Cone.m_coneInfo);
		}
		return text;
	}
}
