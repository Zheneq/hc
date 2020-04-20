using System;
using System.Collections.Generic;

[Serializable]
public class StandardGroundEffectInfo
{
	public bool m_applyGroundEffect;

	public GroundEffectField m_groundEffectData;

	public unsafe virtual void ReportAbilityTooltipNumbers(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject enemySubject, AbilityTooltipSubject allySubject)
	{
		if (this.m_applyGroundEffect)
		{
			this.m_groundEffectData.ReportAbilityTooltipNumbers(ref numbers, enemySubject, allySubject);
		}
	}
}
