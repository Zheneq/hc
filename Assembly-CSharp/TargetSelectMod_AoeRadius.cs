using System;
using UnityEngine;

[Serializable]
public class TargetSelectMod_AoeRadius : TargetSelectModBase
{
	[Header("Targeting Properties")]
	public AbilityModPropertyFloat m_radiusMod;

	[Space(10f)]
	public AbilityModPropertyBool m_useSquareCenterPosMod;

	public override string GetModSpecificInEditorDesc(GenericAbility_TargetSelectBase targetSelectBase, string header)
	{
		string text = string.Empty;
		TargetSelect_AoeRadius targetSelect_AoeRadius = targetSelectBase as TargetSelect_AoeRadius;
		if (targetSelect_AoeRadius != null)
		{
			text += AbilityModHelper.GetModPropertyDesc(this.m_radiusMod, "[Radius]", true, targetSelect_AoeRadius.m_radius);
			text += AbilityModHelper.GetModPropertyDesc(this.m_useSquareCenterPosMod, "[UseSquareCenterPos]", true, targetSelect_AoeRadius.m_useSquareCenterPos);
		}
		return text;
	}
}
