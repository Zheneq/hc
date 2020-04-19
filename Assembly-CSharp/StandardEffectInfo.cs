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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(StandardEffectInfo.ReportAbilityTooltipNumbers(List<AbilityTooltipNumber>*, AbilityTooltipSubject)).MethodHandle;
			}
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
