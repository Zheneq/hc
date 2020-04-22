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
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_groundEffectData.ReportAbilityTooltipNumbers(ref numbers, enemySubject, allySubject);
			return;
		}
	}
}
