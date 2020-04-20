using System;
using UnityEngine;

[Serializable]
public class TargetSelectModBase
{
	[Header("-- Target Data override. Only applicable if target select component overrides base ability's Target Data")]
	public bool m_overrideTargetDataOnTargetSelect;

	public TargetData[] m_targetDataOverride;

	[Header("-- Targeting - Team Filters, LOS. (Overrides fields in ConeTargetingInfo and LaserTargetingInfo)")]
	public AbilityModPropertyBool m_includeEnemiesMod;

	public AbilityModPropertyBool m_includeAlliesMod;

	public AbilityModPropertyBool m_includeCasterMod;

	public AbilityModPropertyBool m_ignoreLosMod;

	public string GetInEditorDesc(GenericAbility_TargetSelectBase targetSelectBase, string header)
	{
		string text = string.Empty;
		if (this.m_overrideTargetDataOnTargetSelect)
		{
			if (this.m_targetDataOverride != null)
			{
				text = text + InEditorDescHelper.ColoredString("* Using TargetData Override *", "cyan", false) + "\n";
				foreach (TargetData targetData in this.m_targetDataOverride)
				{
					text = text + "    [Paradigm] " + ((targetData.m_targetingParadigm <= (Ability.TargetingParadigm)0) ? "INVALID" : targetData.m_targetingParadigm.ToString());
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						", [Range (without range mods)] ",
						targetData.m_minRange,
						" to ",
						targetData.m_range
					});
					text = text + ", [Require Los] = " + targetData.m_checkLineOfSight.ToString() + "\n";
				}
			}
		}
		text += AbilityModHelper.GetModPropertyDesc(this.m_includeEnemiesMod, "[IncludeEnemies]", targetSelectBase.m_includeEnemies, false);
		text += AbilityModHelper.GetModPropertyDesc(this.m_includeAlliesMod, "[IncludeAllies]", targetSelectBase.m_includeAllies, false);
		text += AbilityModHelper.GetModPropertyDesc(this.m_includeCasterMod, "[IncludeCaster]", targetSelectBase.m_includeCaster, false);
		text += AbilityModHelper.GetModPropertyDesc(this.m_ignoreLosMod, "[IgnoreLos]", targetSelectBase.m_ignoreLos, false);
		text += this.GetModSpecificInEditorDesc(targetSelectBase, header);
		if (text.Length > 0)
		{
			text = header + "\n" + text + "\n";
		}
		return text;
	}

	public virtual string GetModSpecificInEditorDesc(GenericAbility_TargetSelectBase targetSelectBase, string header)
	{
		return string.Empty;
	}
}
