using System;
using System.Text;
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
		string str = string.Empty;
		if (m_overrideTargetDataOnTargetSelect)
		{
			if (m_targetDataOverride != null)
			{
				str = new StringBuilder().Append(str).Append(InEditorDescHelper.ColoredString("* Using TargetData Override *")).Append("\n").ToString();
				TargetData[] targetDataOverride = m_targetDataOverride;
				foreach (TargetData targetData in targetDataOverride)
				{
					str = new StringBuilder().Append(str).Append("    [Paradigm] ").Append((targetData.m_targetingParadigm <= (Ability.TargetingParadigm)0) ? "INVALID" : targetData.m_targetingParadigm.ToString()).ToString();
					string text = str;
					str = new StringBuilder().Append(text).Append(", [Range (without range mods)] ").Append(targetData.m_minRange).Append(" to ").Append(targetData.m_range).ToString();
					str = new StringBuilder().Append(str).Append(", [Require Los] = ").Append(targetData.m_checkLineOfSight).Append("\n").ToString();
				}
			}
		}
		str += AbilityModHelper.GetModPropertyDesc(m_includeEnemiesMod, "[IncludeEnemies]", targetSelectBase.m_includeEnemies);
		str += AbilityModHelper.GetModPropertyDesc(m_includeAlliesMod, "[IncludeAllies]", targetSelectBase.m_includeAllies);
		str += AbilityModHelper.GetModPropertyDesc(m_includeCasterMod, "[IncludeCaster]", targetSelectBase.m_includeCaster);
		str += AbilityModHelper.GetModPropertyDesc(m_ignoreLosMod, "[IgnoreLos]", targetSelectBase.m_ignoreLos);
		str += GetModSpecificInEditorDesc(targetSelectBase, header);
		if (str.Length > 0)
		{
			str = new StringBuilder().Append(header).Append("\n").Append(str).Append("\n").ToString();
		}
		return str;
	}

	public virtual string GetModSpecificInEditorDesc(GenericAbility_TargetSelectBase targetSelectBase, string header)
	{
		return string.Empty;
	}
}
