using System;
using UnityEngine;

public class BazookaGirlDelayedMissileEffectSequence : Sequence
{
	public GameObject m_fxPrefab;

	private const float FLOOR_OFFSET = 0.12f;

	private GameObject m_fx;

	[AudioEvent(false)]
	public string m_audioEventApply;

	[AudioEvent(false)]
	public string m_audioEventFire = "ablty/bazookagirl/above_fire";

	protected override void OnStopVfxOnClient()
	{
		if (this.m_fx != null)
		{
			this.m_fx.SetActive(false);
		}
	}

	private void Update()
	{
		if (this.m_fxPrefab)
		{
			if (this.m_initialized && this.m_fx == null)
			{
				if (base.Caster != null)
				{
					this.m_fx = base.InstantiateFX(this.m_fxPrefab);
					if (this.m_fx.GetComponent<FriendlyEnemyVFXSelector>() != null)
					{
						this.m_fx.GetComponent<FriendlyEnemyVFXSelector>().Setup(base.Caster.GetTeam());
					}
					this.m_fx.transform.position = base.TargetPos + Vector3.up * 0.12f;
					this.m_fx.transform.localRotation = Quaternion.identity;
				}
			}
		}
	}

	private void OnDisable()
	{
		if (this.m_fx)
		{
			UnityEngine.Object.Destroy(this.m_fx);
		}
	}
}
