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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "AfterimageStrike (intended as a chain ability)";
		}
		m_syncComp = base.ActorData.GetComponent<Samurai_SyncComponent>();
		m_parentAbility = GetAbilityOfType<SamuraiSwordDash>();
	}

	public int GetDamageAmount()
	{
		int result;
		if ((bool)m_parentAbility)
		{
			result = m_parentAbility.GetKnockbackDamage();
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
		if ((bool)m_parentAbility)
		{
			result = m_parentAbility.GetKnockbackLessDamagePerTarget();
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public float GetKnockbackDist()
	{
		return (!m_parentAbility) ? 0f : m_parentAbility.GetKnockbackDist();
	}

	public KnockbackType GetKnockbackType()
	{
		int result;
		if ((bool)m_parentAbility)
		{
			result = (int)m_parentAbility.GetKnockbackType();
		}
		else
		{
			result = 4;
		}
		return (KnockbackType)result;
	}

	public float GetExtraDamageFromDamageTakenMult()
	{
		float result;
		if ((bool)m_parentAbility)
		{
			result = m_parentAbility.GetKnockbackExtraDamageFromDamageTakenMult();
		}
		else
		{
			result = 0f;
		}
		return result;
	}
}
