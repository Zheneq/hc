using System;
using UnityEngine;

[Serializable]
public class TargetSelectMod_ChargeSingleStep : TargetSelectModBase
{
	[Header("Targeting Properties")]
	public AbilityModPropertyShape m_destShapeMod;

	public override string GetModSpecificInEditorDesc(GenericAbility_TargetSelectBase targetSelectBase, string header)
	{
		string text = string.Empty;
		TargetSelect_ChargeSingleStep targetSelect_ChargeSingleStep = targetSelectBase as TargetSelect_ChargeSingleStep;
		if (targetSelect_ChargeSingleStep != null)
		{
			text += AbilityModHelper.GetModPropertyDesc(this.m_destShapeMod, "[DestShape]", true, targetSelect_ChargeSingleStep.m_destShape);
		}
		return text;
	}
}
