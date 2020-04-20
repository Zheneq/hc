using System;
using System.Collections.Generic;

[Serializable]
public class StandardEffectInfo
{
	public bool m_applyEffect;

	public StandardActorEffectData m_effectData;

	public unsafe virtual void ReportAbilityTooltipNumbers(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject subject)
	{
		if (this.m_applyEffect)
		{
			this.m_effectData.ReportAbilityTooltipNumbers(ref numbers, subject);
		}
	}

	public StandardEffectInfo GetShallowCopy()
	{
		return new StandardEffectInfo
		{
			m_applyEffect = this.m_applyEffect,
			m_effectData = this.m_effectData.GetShallowCopy()
		};
	}
}
