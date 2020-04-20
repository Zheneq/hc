﻿using System;
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
			text += AbilityModHelper.GetModPropertyDesc(this.m_coneWidthAngleMod, "[ConeWidthAngle]", true, targetSelect_LayerCones.m_coneWidthAngle);
			if (this.m_useConeRadiusOverrides)
			{
				text += "-- Using Cone Radius Overrides --\n";
				if (this.m_coneRadiusOverrides != null)
				{
					using (List<float>.Enumerator enumerator = this.m_coneRadiusOverrides.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							float num = enumerator.Current;
							float num2 = num;
							string text2 = text;
							text = string.Concat(new object[]
							{
								text2,
								"\t",
								num2,
								"\n"
							});
						}
					}
				}
			}
		}
		return text;
	}
}
