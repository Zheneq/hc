using UnityEngine;

public class GenericTempSatelliteSequence : TempSatelliteSequence
{
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[AnimEventPicker]
	public Object m_startEvent;

	[JointPopup("FX attach joint (or start position for spawn).")]
	public JointPopupProperty m_fxJoint;

	[Header("-- Whether to use Caster rotation or rotation passed to sequence")]
	public bool m_useCasterRotation = true;

	public override void FinishSetup()
	{
		if (m_startEvent == null)
		{
			TriggerSpawn();
		}
	}

	private void TriggerSpawn()
	{
		if (!m_fxJoint.IsInitialized())
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
			m_fxJoint.Initialize(base.Caster.gameObject);
		}
		if (!(m_tempSatellitePrefab != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			Quaternion rotation = (!m_useCasterRotation) ? base.TargetRotation : m_fxJoint.m_jointObject.transform.rotation;
			m_tempSatelliteInstance = InstantiateFX(m_tempSatellitePrefab, m_fxJoint.m_jointObject.transform.position, rotation);
			TempSatellite component = m_tempSatelliteInstance.GetComponent<TempSatellite>();
			if (component != null)
			{
				component.Setup(this);
				component.TriggerSpawn();
			}
			return;
		}
	}

	private void Update()
	{
		if (m_initialized)
		{
			ProcessSequenceVisibility();
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (!(m_startEvent == parameter))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			TriggerSpawn();
			return;
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
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			TempSatellite component = m_tempSatelliteInstance.GetComponent<TempSatellite>();
			if (component != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					component.TriggerDespawn();
					return;
				}
			}
			return;
		}
	}
}
