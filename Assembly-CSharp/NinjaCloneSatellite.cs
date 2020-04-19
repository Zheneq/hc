using System;
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
		this.m_modelAnimator.SetFloat("RandomValue", UnityEngine.Random.value);
	}

	private void IncrementRandomParameter()
	{
		float value = (float)this.m_lastAttackIndex / 3f;
		this.m_modelAnimator.SetFloat("RandomValue", value);
		this.m_lastAttackIndex++;
		this.m_lastAttackIndex %= 3;
	}

	public void TriggerRandomAttack(GameObject attackTarget)
	{
		this.SetRandomParameter();
		this.m_modelAnimator.SetTrigger("StartAttack");
		this.m_attackTarget = attackTarget;
		this.m_numAttacksLeft = 0;
		this.m_attackDelay = 0f;
		this.m_lastAttackTime = Time.time;
	}

	public void TriggerDeathMarkAttack()
	{
		this.m_modelAnimator.SetTrigger("StartDeathMark");
	}

	public void TriggerMultiAttack(GameObject attackTarget, int numAttacks, float attackDelay)
	{
		this.IncrementRandomParameter();
		this.m_modelAnimator.SetTrigger("StartAttack");
		this.m_attackTarget = attackTarget;
		this.m_numAttacksLeft = numAttacks - 1;
		this.m_attackDelay = attackDelay;
		this.m_lastAttackTime = Time.time;
	}

	public override void TriggerDespawn()
	{
		this.m_modelAnimator.SetTrigger("Despawn");
	}

	public bool IsDespawning()
	{
		return this.m_modelAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Despawn");
	}

	private void Update()
	{
		AnimatorStateInfo currentAnimatorStateInfo = this.m_modelAnimator.GetCurrentAnimatorStateInfo(0);
		if (currentAnimatorStateInfo.IsTag("Despawn"))
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaCloneSatellite.Update()).MethodHandle;
			}
			if (currentAnimatorStateInfo.normalizedTime >= 1f)
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
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
		}
		if (currentAnimatorStateInfo.IsTag("Attack"))
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
			if (this.m_attackTarget != null)
			{
				base.transform.rotation = Quaternion.LookRotation((this.m_attackTarget.transform.position - base.transform.position).normalized);
			}
			if (this.m_numAttacksLeft > 0 && Time.time > this.m_lastAttackTime + this.m_attackDelay)
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
				this.IncrementRandomParameter();
				this.m_modelAnimator.SetTrigger("StartAttack");
				this.m_lastAttackTime = Time.time;
				this.m_numAttacksLeft--;
			}
			else if (this.m_numAttacksLeft == 0)
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
				if (currentAnimatorStateInfo.normalizedTime >= 1f)
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
					this.TriggerDespawn();
				}
			}
		}
	}
}
