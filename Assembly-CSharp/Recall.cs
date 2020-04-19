using System;
using UnityEngine;

public class Recall : Ability
{
	[Tooltip("Cooldown of the ability will be set to this after successfully teleporting (ignored if negative).")]
	public int m_cooldownOnSuccess = -1;

	private void Start()
	{
		this.m_abilityName = "Recall";
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		bool flag = GameplayData.Get().m_recallAllowed;
		if (flag && GameplayData.Get().m_recallOnlyWhenOutOfCombat)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Recall.CustomCanCastValidation(ActorData)).MethodHandle;
			}
			flag = caster.OutOfCombat;
		}
		return flag;
	}
}
