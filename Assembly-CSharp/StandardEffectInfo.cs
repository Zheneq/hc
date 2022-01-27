using System;
using System.Collections.Generic;

[Serializable]
public class StandardEffectInfo
{
	public bool m_applyEffect;
	public StandardActorEffectData m_effectData;

	public virtual void ReportAbilityTooltipNumbers(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject subject)
	{
		if (m_applyEffect)
		{
			m_effectData.ReportAbilityTooltipNumbers(ref numbers, subject);
		}
	}

	public StandardEffectInfo GetShallowCopy()
	{
		return new StandardEffectInfo
		{
			m_applyEffect = m_applyEffect,
			m_effectData = m_effectData.GetShallowCopy()
		};
	}
}
