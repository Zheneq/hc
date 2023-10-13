using System;
using System.Collections.Generic;

[Serializable]
public class TargetSelectMod_LayerCones : TargetSelectModBase
{
	[Separator("Targeting Properties", true)]
	public AbilityModPropertyFloat m_coneWidthAngleMod;
	[Separator("Cone Radius Overrides", true)]
	public bool m_useConeRadiusOverrides;
	public List<float> m_coneRadiusOverrides = new List<float>();

	public override string GetModSpecificInEditorDesc(GenericAbility_TargetSelectBase targetSelectBase, string header)
	{
		string text = string.Empty;
		TargetSelect_LayerCones targetSelect_LayerCones = targetSelectBase as TargetSelect_LayerCones;
		if (targetSelect_LayerCones != null)
		{
			text += AbilityModHelper.GetModPropertyDesc(m_coneWidthAngleMod, "[ConeWidthAngle]", true, targetSelect_LayerCones.m_coneWidthAngle);
			if (m_useConeRadiusOverrides)
			{
				text += "-- Using Cone Radius Overrides --\n";
				if (m_coneRadiusOverrides != null)
				{
					foreach (float num in m_coneRadiusOverrides)
					{
						text += "\t" + num + "\n";
					}
				}
			}
		}
		return text;
	}
}
