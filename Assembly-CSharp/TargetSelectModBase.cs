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
		string str = string.Empty;
		if (m_overrideTargetDataOnTargetSelect)
		{
			while (true)
			{
				switch (5)
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
			if (m_targetDataOverride != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				str = str + InEditorDescHelper.ColoredString("* Using TargetData Override *") + "\n";
				TargetData[] targetDataOverride = m_targetDataOverride;
				foreach (TargetData targetData in targetDataOverride)
				{
					str = str + "    [Paradigm] " + ((targetData.m_targetingParadigm <= (Ability.TargetingParadigm)0) ? "INVALID" : targetData.m_targetingParadigm.ToString());
					string text = str;
					str = text + ", [Range (without range mods)] " + targetData.m_minRange + " to " + targetData.m_range;
					str = str + ", [Require Los] = " + targetData.m_checkLineOfSight + "\n";
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
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			str = header + "\n" + str + "\n";
		}
		return str;
	}

	public virtual string GetModSpecificInEditorDesc(GenericAbility_TargetSelectBase targetSelectBase, string header)
	{
		return string.Empty;
	}
}
