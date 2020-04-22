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
		float value = (float)m_lastAttackIndex / 3f;
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
		if (currentAnimatorStateInfo.IsTag("Despawn"))
		{
			while (true)
			{
				switch (1)
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
			if (currentAnimatorStateInfo.normalizedTime >= 1f)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						Object.Destroy(base.gameObject);
						return;
					}
				}
			}
		}
		if (!currentAnimatorStateInfo.IsTag("Attack"))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (m_attackTarget != null)
			{
				base.transform.rotation = Quaternion.LookRotation((m_attackTarget.transform.position - base.transform.position).normalized);
			}
			if (m_numAttacksLeft > 0 && Time.time > m_lastAttackTime + m_attackDelay)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						IncrementRandomParameter();
						m_modelAnimator.SetTrigger("StartAttack");
						m_lastAttackTime = Time.time;
						m_numAttacksLeft--;
						return;
					}
				}
			}
			if (m_numAttacksLeft != 0)
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
				if (currentAnimatorStateInfo.normalizedTime >= 1f)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						TriggerDespawn();
						return;
					}
				}
				return;
			}
		}
	}
}
