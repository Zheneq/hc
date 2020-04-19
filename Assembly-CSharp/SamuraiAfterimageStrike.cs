using System;
using UnityEngine;

public class SamuraiAfterimageStrike : Ability
{
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	[Header("-- for removing afterimage --")]
	public GameObject m_selfHitSequencePrefab;

	public ActorModelData.ActionAnimationType m_afterImageAnim = ActorModelData.ActionAnimationType.Ability1;

	private SamuraiSwordDash m_parentAbility;

	private Samurai_SyncComponent m_syncComp;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "AfterimageStrike (intended as a chain ability)";
		}
		this.m_syncComp = base.ActorData.GetComponent<Samurai_SyncComponent>();
		this.m_parentAbility = base.GetAbilityOfType<SamuraiSwordDash>();
	}

	public int GetDamageAmount()
	{
		int result;
		if (this.m_parentAbility)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiAfterimageStrike.GetDamageAmount()).MethodHandle;
			}
			result = this.m_parentAbility.GetKnockbackDamage();
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetLessDamagePerTarget()
	{
		int result;
		if (this.m_parentAbility)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiAfterimageStrike.GetLessDamagePerTarget()).MethodHandle;
			}
			result = this.m_parentAbility.GetKnockbackLessDamagePerTarget();
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public float GetKnockbackDist()
	{
		return (!this.m_parentAbility) ? 0f : this.m_parentAbility.GetKnockbackDist();
	}

	public KnockbackType GetKnockbackType()
	{
		KnockbackType result;
		if (this.m_parentAbility)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiAfterimageStrike.GetKnockbackType()).MethodHandle;
			}
			result = this.m_parentAbility.GetKnockbackType();
		}
		else
		{
			result = KnockbackType.AwayFromSource;
		}
		return result;
	}

	public float GetExtraDamageFromDamageTakenMult()
	{
		float result;
		if (this.m_parentAbility)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiAfterimageStrike.GetExtraDamageFromDamageTakenMult()).MethodHandle;
			}
			result = this.m_parentAbility.GetKnockbackExtraDamageFromDamageTakenMult();
		}
		else
		{
			result = 0f;
		}
		return result;
	}
}
