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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(StandardGroundEffectInfo.ReportAbilityTooltipNumbers(List<AbilityTooltipNumber>*, AbilityTooltipSubject, AbilityTooltipSubject)).MethodHandle;
			}
			this.m_groundEffectData.ReportAbilityTooltipNumbers(ref numbers, enemySubject, allySubject);
		}
	}
}
