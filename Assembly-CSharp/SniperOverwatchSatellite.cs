using UnityEngine;

public class SniperOverwatchSatellite : TempSatellite
{
	private GameObject m_attackTarget;

	private float m_timeDespawnTriggered = -1f;

	public override void TriggerAttack(GameObject attackTarget)
	{
		m_modelAnimator.SetTrigger("StartAttack");
		m_attackTarget = attackTarget;
	}

	public override void TriggerSpawn()
	{
		m_modelAnimator.SetTrigger("Spawn");
	}

	public override void TriggerDespawn()
	{
		m_modelAnimator.SetTrigger("Despawn");
		m_timeDespawnTriggered = Time.time;
	}

	private void Update()
	{
		AnimatorStateInfo currentAnimatorStateInfo = m_modelAnimator.GetCurrentAnimatorStateInfo(0);
		if (currentAnimatorStateInfo.IsTag("Despawn"))
		{
			if (currentAnimatorStateInfo.normalizedTime >= 1f)
			{
				goto IL_006c;
			}
		}
		if (m_timeDespawnTriggered > 0f && Time.time - m_timeDespawnTriggered >= 10f)
		{
			goto IL_006c;
		}
		if (!currentAnimatorStateInfo.IsTag("Attack"))
		{
			return;
		}
		while (true)
		{
			if (m_attackTarget != null)
			{
				while (true)
				{
					base.transform.rotation = Quaternion.LookRotation((m_attackTarget.transform.position - base.transform.position).normalized);
					return;
				}
			}
			return;
		}
		IL_006c:
		Object.Destroy(base.gameObject);
	}
}
