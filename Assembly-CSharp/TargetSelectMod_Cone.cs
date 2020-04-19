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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelectMod_Cone.GetModSpecificInEditorDesc(GenericAbility_TargetSelectBase, string)).MethodHandle;
			}
			text += AbilityModHelper.GetModPropertyDesc(this.m_coneInfoMod, "[ConeInfo]", true, targetSelect_Cone.m_coneInfo);
		}
		return text;
	}
}
