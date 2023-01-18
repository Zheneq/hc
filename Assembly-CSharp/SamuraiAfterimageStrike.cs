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
		m_syncComp = ActorData.GetComponent<Samurai_SyncComponent>();
		m_parentAbility = GetAbilityOfType<SamuraiSwordDash>();
	}

	public int GetDamageAmount()
	{
		return m_parentAbility != null
			? m_parentAbility.GetKnockbackDamage()
			: 0;
	}

	public int GetLessDamagePerTarget()
	{
		return m_parentAbility != null
			? m_parentAbility.GetKnockbackLessDamagePerTarget()
			: 0;
	}

	public float GetKnockbackDist()
	{
		return m_parentAbility != null
			? m_parentAbility.GetKnockbackDist()
			: 0f;
	}

	public KnockbackType GetKnockbackType()
	{
		return m_parentAbility != null
			? m_parentAbility.GetKnockbackType()
			: KnockbackType.AwayFromSource;
	}

	public float GetExtraDamageFromDamageTakenMult()
	{
		return m_parentAbility != null
			? m_parentAbility.GetKnockbackExtraDamageFromDamageTakenMult()
			: 0f;
	}
}
