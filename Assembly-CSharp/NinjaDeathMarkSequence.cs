using UnityEngine;

public class NinjaDeathMarkSequence : TempSatelliteSequence
{
	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public Object m_startEvent;

	private bool m_spawnedTempSatellite;

	private bool m_setFinishTrigger;

	public override void FinishSetup()
	{
		if (!(m_startEvent == null))
		{
			return;
		}
		while (true)
		{
			SpawnTempSatellite();
			return;
		}
	}

	private void SpawnTempSatellite()
	{
		m_spawnedTempSatellite = true;
		m_tempSatelliteInstance = InstantiateFX(m_tempSatellitePrefab, base.TargetPos, Quaternion.identity);
		m_tempSatelliteInstance.GetComponent<NinjaCloneSatellite>().Setup(this);
		m_tempSatelliteInstance.GetComponent<NinjaCloneSatellite>().TriggerDeathMarkAttack();
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (!(m_startEvent == parameter))
		{
			return;
		}
		while (true)
		{
			SpawnTempSatellite();
			return;
		}
	}

	private void Update()
	{
		if (!m_spawnedTempSatellite)
		{
			return;
		}
		while (true)
		{
			if (m_setFinishTrigger)
			{
				return;
			}
			while (true)
			{
				if (!(m_tempSatelliteInstance == null))
				{
					if (!m_tempSatelliteInstance.GetComponent<NinjaCloneSatellite>().IsDespawning())
					{
						return;
					}
				}
				m_setFinishTrigger = true;
				base.Caster.GetActorModelData().GetModelAnimator().SetTrigger("FinishAttack");
				return;
			}
		}
	}

	private void OnDisable()
	{
		m_initialized = false;
	}
}
