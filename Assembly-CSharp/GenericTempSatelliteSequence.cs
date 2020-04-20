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
			this.m_fxJoint.Initialize(base.Caster.gameObject);
		}
		if (this.m_tempSatellitePrefab != null)
		{
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
			this.TriggerSpawn();
		}
	}

	private void OnDisable()
	{
		if (this.m_tempSatelliteInstance != null)
		{
			TempSatellite component = this.m_tempSatelliteInstance.GetComponent<TempSatellite>();
			if (component != null)
			{
				component.TriggerDespawn();
			}
		}
	}
}
