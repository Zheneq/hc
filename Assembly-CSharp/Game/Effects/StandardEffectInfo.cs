// ROGUES
// SERVER
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

#if SERVER
	public StandardActorEffect CreateEffect(EffectSource parent, ActorData target, ActorData caster)
	{
		StandardActorEffect result = null;
		if (m_applyEffect)
		{
			result = new StandardActorEffect(parent, target.GetCurrentBoardSquare(), target, caster, m_effectData);
		}
		return result;
	}
#endif
}
