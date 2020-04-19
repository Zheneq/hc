using System;
using UnityEngine;

public class GenericTempSatelliteSequence : TempSatelliteSequence
{
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[AnimEventPicker]
	public UnityEngine.Object m_startEvent;

	[JointPopup("FX attach joint (or start position for spawn).")]
	public JointPopupProperty m_fxJoint;

	[Header("-- Whether to use Caster rotation or rotation passed to sequence")]
	public bool m_useCasterRotation = true;

	public override void FinishSetup()
	{
		if (this.m_startEvent == null)
		{
			this.TriggerSpawn();
		}
	}

	private void TriggerSpawn()
	{
		if (!this.m_fxJoint.IsInitialized())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericTempSatelliteSequence.TriggerSpawn()).MethodHandle;
			}
			this.m_fxJoint.Initialize(base.Caster.gameObject);
		}
		if (this.m_tempSatellitePrefab != null)
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
			Quaternion rotation = (!this.m_useCasterRotation) ? base.TargetRotation : this.m_fxJoint.m_jointObject.transform.rotation;
			this.m_tempSatelliteInstance = base.InstantiateFX(this.m_tempSatellitePrefab, this.m_fxJoint.m_jointObject.transform.position, rotation, true, true);
			TempSatellite component = this.m_tempSatelliteInstance.GetComponent<TempSatellite>();
			if (component != null)
			{
				component.Setup(this);
				component.TriggerSpawn();
			}
		}
	}

	private void Update()
	{
		if (this.m_initialized)
		{
			base.ProcessSequenceVisibility();
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericTempSatelliteSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			this.TriggerSpawn();
		}
	}

	private void OnDisable()
	{
		if (this.m_tempSatelliteInstance != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericTempSatelliteSequence.OnDisable()).MethodHandle;
			}
			TempSatellite component = this.m_tempSatelliteInstance.GetComponent<TempSatellite>();
			if (component != null)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				component.TriggerDespawn();
			}
		}
	}
}
