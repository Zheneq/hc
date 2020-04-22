using UnityEngine;

public class Recall : Ability
{
	[Tooltip("Cooldown of the ability will be set to this after successfully teleporting (ignored if negative).")]
	public int m_cooldownOnSuccess = -1;

	private void Start()
	{
		m_abilityName = "Recall";
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		bool flag = GameplayData.Get().m_recallAllowed;
		if (flag && GameplayData.Get().m_recallOnlyWhenOutOfCombat)
		{
			while (true)
			{
				switch (2)
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
			flag = caster.OutOfCombat;
		}
		return flag;
	}
}
