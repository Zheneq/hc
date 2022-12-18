using UnityEngine;

public class NinjaCloneSatellite : TempSatellite
{
	private GameObject m_attackTarget;
	private int m_numAttacksLeft;
	private float m_attackDelay;
	private float m_lastAttackTime = -1f;
	private int m_lastAttackIndex;

	private void SetRandomParameter()
	{
		m_modelAnimator.SetFloat("RandomValue", Random.value);
	}

	private void IncrementRandomParameter()
	{
		float value = m_lastAttackIndex / 3f;
		m_modelAnimator.SetFloat("RandomValue", value);
		m_lastAttackIndex++;
		m_lastAttackIndex %= 3;
	}

	public void TriggerRandomAttack(GameObject attackTarget)
	{
		SetRandomParameter();
		m_modelAnimator.SetTrigger("StartAttack");
		m_attackTarget = attackTarget;
		m_numAttacksLeft = 0;
		m_attackDelay = 0f;
		m_lastAttackTime = Time.time;
	}

	public void TriggerDeathMarkAttack()
	{
		m_modelAnimator.SetTrigger("StartDeathMark");
	}

	public void TriggerMultiAttack(GameObject attackTarget, int numAttacks, float attackDelay)
	{
		IncrementRandomParameter();
		m_modelAnimator.SetTrigger("StartAttack");
		m_attackTarget = attackTarget;
		m_numAttacksLeft = numAttacks - 1;
		m_attackDelay = attackDelay;
		m_lastAttackTime = Time.time;
	}

	public override void TriggerDespawn()
	{
		m_modelAnimator.SetTrigger("Despawn");
	}

	public bool IsDespawning()
	{
		return m_modelAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Despawn");
	}

	private void Update()
	{
		AnimatorStateInfo currentAnimatorStateInfo = m_modelAnimator.GetCurrentAnimatorStateInfo(0);
		if (currentAnimatorStateInfo.IsTag("Despawn") && currentAnimatorStateInfo.normalizedTime >= 1f)
		{
			Destroy(gameObject);
			return;
		}
		if (!currentAnimatorStateInfo.IsTag("Attack"))
		{
			return;
		}
		if (m_attackTarget != null)
		{
			transform.rotation = Quaternion.LookRotation((m_attackTarget.transform.position - transform.position).normalized);
		}
		if (m_numAttacksLeft > 0 && Time.time > m_lastAttackTime + m_attackDelay)
		{
			IncrementRandomParameter();
			m_modelAnimator.SetTrigger("StartAttack");
			m_lastAttackTime = Time.time;
			m_numAttacksLeft--;
		}
		else if (m_numAttacksLeft == 0 && currentAnimatorStateInfo.normalizedTime >= 1f)
		{
			TriggerDespawn();
		}
	}
}
