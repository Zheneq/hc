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
			while (true)
			{
				switch (1)
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
			text += AbilityModHelper.GetModPropertyDesc(m_coneWidthAngleMod, "[ConeWidthAngle]", true, targetSelect_LayerCones.m_coneWidthAngle);
			if (m_useConeRadiusOverrides)
			{
				text += "-- Using Cone Radius Overrides --\n";
				if (m_coneRadiusOverrides != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
						{
							using (List<float>.Enumerator enumerator = m_coneRadiusOverrides.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									float num = enumerator.Current;
									string text2 = text;
									text = text2 + "\t" + num + "\n";
								}
								while (true)
								{
									switch (7)
									{
									case 0:
										break;
									default:
										return text;
									}
								}
							}
						}
						}
					}
				}
			}
		}
		return text;
	}
}
