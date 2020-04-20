using System;
using UnityEngine;

public class NinjaDeathMarkSequence : TempSatelliteSequence
{
	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public UnityEngine.Object m_startEvent;

	private bool m_spawnedTempSatellite;

	private bool m_setFinishTrigger;

	public override void FinishSetup()
	{
		if (this.m_startEvent == null)
		{
			this.SpawnTempSatellite();
		}
	}

	private void SpawnTempSatellite()
	{
		this.m_spawnedTempSatellite = true;
		this.m_tempSatelliteInstance = base.InstantiateFX(this.m_tempSatellitePrefab, base.TargetPos, Quaternion.identity, true, true);
		this.m_tempSatelliteInstance.GetComponent<NinjaCloneSatellite>().Setup(this);
		this.m_tempSatelliteInstance.GetComponent<NinjaCloneSatellite>().TriggerDeathMarkAttack();
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
		{
			this.SpawnTempSatellite();
		}
	}

	private void Update()
	{
		if (this.m_spawnedTempSatellite)
		{
			if (!this.m_setFinishTrigger)
			{
				if (!(this.m_tempSatelliteInstance == null))
				{
					if (!this.m_tempSatelliteInstance.GetComponent<NinjaCloneSatellite>().IsDespawning())
					{
						return;
					}
				}
				this.m_setFinishTrigger = true;
				base.Caster.GetActorModelData().GetModelAnimator().SetTrigger("FinishAttack");
			}
		}
	}

	private void OnDisable()
	{
		this.m_initialized = false;
	}
}
