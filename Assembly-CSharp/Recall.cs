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
			flag = caster.OutOfCombat;
		}
		return flag;
	}
}
