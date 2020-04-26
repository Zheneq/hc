using UnityEngine;

public class SniperOverwatchSatelliteSequence : TempSatelliteSequence
{
	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public Object m_startEvent;

	[JointPopup("FX attach joint (or start position for spawn).")]
	public JointPopupProperty m_fxJoint;

	public bool m_parentToSequence;

	public override void FinishSetup()
	{
		if (!(m_startEvent == null))
		{
			return;
		}
		while (true)
		{
			TriggerSpawn();
			return;
		}
	}

	private void TriggerSpawn()
	{
		if (!m_fxJoint.IsInitialized())
		{
			m_fxJoint.Initialize(base.Caster.gameObject);
		}
		m_tempSatelliteInstance = InstantiateFX(m_tempSatellitePrefab, m_fxJoint.m_jointObject.transform.position, m_fxJoint.m_jointObject.transform.rotation);
		if (!m_parentToSequence)
		{
			m_tempSatelliteInstance.transform.parent = null;
		}
		m_tempSatelliteInstance.GetComponent<TempSatellite>().Setup(this);
		m_tempSatelliteInstance.GetComponent<SniperOverwatchSatellite>().TriggerSpawn();
	}

	private void Update()
	{
		if (!m_initialized)
		{
			return;
		}
		while (true)
		{
			ProcessSequenceVisibility();
			return;
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (m_startEvent == parameter)
		{
			TriggerSpawn();
		}
	}

	private void OnDisable()
	{
		if (!(m_tempSatelliteInstance != null))
		{
			return;
		}
		while (true)
		{
			m_tempSatelliteInstance.GetComponent<SniperOverwatchSatellite>().TriggerDespawn();
			return;
		}
	}
}
