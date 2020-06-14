using System;
using System.Collections.Generic;

[Serializable]
public class StandardEffectInfo
{
	public bool m_applyEffect;

	public StandardActorEffectData m_effectData;

	public virtual void ReportAbilityTooltipNumbers(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject subject)
	{
		if (!m_applyEffect)
		{
			return;
		}
		m_effectData.ReportAbilityTooltipNumbers(ref numbers, subject);
	}

	public StandardEffectInfo GetShallowCopy()
	{
		StandardEffectInfo standardEffectInfo = new StandardEffectInfo();
		standardEffectInfo.m_applyEffect = m_applyEffect;
		standardEffectInfo.m_effectData = m_effectData.GetShallowCopy();
		return standardEffectInfo;
	}
}
