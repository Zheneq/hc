using System;
using UnityEngine;

public class SniperOverwatchSatelliteSequence : TempSatelliteSequence
{
	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public UnityEngine.Object m_startEvent;

	[JointPopup("FX attach joint (or start position for spawn).")]
	public JointPopupProperty m_fxJoint;

	public bool m_parentToSequence;

	public override void FinishSetup()
	{
		if (this.m_startEvent == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SniperOverwatchSatelliteSequence.FinishSetup()).MethodHandle;
			}
			this.TriggerSpawn();
		}
	}

	private void TriggerSpawn()
	{
		if (!this.m_fxJoint.IsInitialized())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SniperOverwatchSatelliteSequence.TriggerSpawn()).MethodHandle;
			}
			this.m_fxJoint.Initialize(base.Caster.gameObject);
		}
		this.m_tempSatelliteInstance = base.InstantiateFX(this.m_tempSatellitePrefab, this.m_fxJoint.m_jointObject.transform.position, this.m_fxJoint.m_jointObject.transform.rotation, true, true);
		if (!this.m_parentToSequence)
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
			this.m_tempSatelliteInstance.transform.parent = null;
		}
		this.m_tempSatelliteInstance.GetComponent<TempSatellite>().Setup(this);
		this.m_tempSatelliteInstance.GetComponent<SniperOverwatchSatellite>().TriggerSpawn();
	}

	private void Update()
	{
		if (this.m_initialized)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SniperOverwatchSatelliteSequence.Update()).MethodHandle;
			}
			base.ProcessSequenceVisibility();
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
		{
			this.TriggerSpawn();
		}
	}

	private void OnDisable()
	{
		if (this.m_tempSatelliteInstance != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SniperOverwatchSatelliteSequence.OnDisable()).MethodHandle;
			}
			this.m_tempSatelliteInstance.GetComponent<SniperOverwatchSatellite>().TriggerDespawn();
		}
	}
}
