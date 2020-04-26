using System;
using System.Collections.Generic;

[Serializable]
public class StandardGroundEffectInfo
{
	public bool m_applyGroundEffect;

	public GroundEffectField m_groundEffectData;

	public virtual void ReportAbilityTooltipNumbers(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject enemySubject, AbilityTooltipSubject allySubject)
	{
		if (!m_applyGroundEffect)
		{
			return;
		}
		while (true)
		{
			m_groundEffectData.ReportAbilityTooltipNumbers(ref numbers, enemySubject, allySubject);
			return;
		}
	}
}
