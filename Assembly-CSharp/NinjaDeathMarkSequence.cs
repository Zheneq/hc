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
		if (m_startEvent == null)
		{
			SpawnTempSatellite();
		}
	}

	private void SpawnTempSatellite()
	{
		m_spawnedTempSatellite = true;
		m_tempSatelliteInstance = InstantiateFX(m_tempSatellitePrefab, TargetPos, Quaternion.identity);
		m_tempSatelliteInstance.GetComponent<NinjaCloneSatellite>().Setup(this);
		m_tempSatelliteInstance.GetComponent<NinjaCloneSatellite>().TriggerDeathMarkAttack();
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (m_startEvent == parameter)
		{
			SpawnTempSatellite();
		}
	}

	private void Update()
	{
		if (m_spawnedTempSatellite
		    && !m_setFinishTrigger
		    && (m_tempSatelliteInstance == null || m_tempSatelliteInstance.GetComponent<NinjaCloneSatellite>().IsDespawning()))
		{
			m_setFinishTrigger = true;
			Caster.GetActorModelData().GetModelAnimator().SetTrigger("FinishAttack");
		}
	}

	private void OnDisable()
	{
		m_initialized = false;
	}
}
