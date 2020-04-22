using System;

[Serializable]
public class TargetSelectMod_ChargeAoE : TargetSelectModBase
{
	[Separator("Targeting Properties", true)]
	public AbilityModPropertyFloat m_radiusAroundStartMod;

	public AbilityModPropertyFloat m_radiusAroundEndMod;

	public AbilityModPropertyFloat m_rangeFromLineMod;

	public AbilityModPropertyBool m_trimPathOnTargetHitMod;

	public override string GetModSpecificInEditorDesc(GenericAbility_TargetSelectBase targetSelectBase, string header)
	{
		string text = string.Empty;
		TargetSelect_ChargeAoE targetSelect_ChargeAoE = targetSelectBase as TargetSelect_ChargeAoE;
		if (targetSelect_ChargeAoE != null)
		{
			while (true)
			{
				switch (6)
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
			text += AbilityModHelper.GetModPropertyDesc(m_radiusAroundStartMod, "[RadiusAroundStart]", true, targetSelect_ChargeAoE.m_radiusAroundStart);
			text += AbilityModHelper.GetModPropertyDesc(m_radiusAroundEndMod, "[RadiusAroundEnd]", true, targetSelect_ChargeAoE.m_radiusAroundEnd);
			text += AbilityModHelper.GetModPropertyDesc(m_rangeFromLineMod, "[RangeFromLine]", true, targetSelect_ChargeAoE.m_rangeFromLine);
			text += AbilityModHelper.GetModPropertyDesc(m_trimPathOnTargetHitMod, "[TrimPathOnTargetHit]", true, targetSelect_ChargeAoE.m_trimPathOnTargetHit);
		}
		return text;
	}
}
