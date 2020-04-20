using System;
using UnityEngine;

public class PointToActorLineSequence : Sequence
{
	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	[JointPopup("FX attach joint on the caster")]
	public JointPopupProperty m_fxJoint;

	public Sequence.ReferenceModelType m_fxJointReferenceType;

	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[AnimEventPicker]
	public UnityEngine.Object m_startEvent;

	[AudioEvent(false)]
	public string m_audioEvent;

	private GameObject m_fx;

	public override void FinishSetup()
	{
		if (this.m_startEvent == null)
		{
			this.SpawnFX();
		}
	}

	private void Update()
	{
		if (this.m_initialized)
		{
			Sequence.SetAttribute(this.m_fx, "startPoint", base.TargetPos);
			if (this.m_fxJoint.m_jointObject != null)
			{
				Sequence.SetAttribute(this.m_fx, "endPoint", this.m_fxJoint.m_jointObject.transform.position);
			}
		}
	}

	private void SpawnFX()
	{
		if (!this.m_fxJoint.IsInitialized())
		{
			GameObject referenceModel = base.GetReferenceModel(base.Target, this.m_fxJointReferenceType);
			if (referenceModel != null)
			{
				this.m_fxJoint.Initialize(referenceModel);
			}
		}
		if (this.m_fxPrefab != null)
		{
			Vector3 targetPos = base.TargetPos;
			Quaternion rotation = default(Quaternion);
			this.m_fx = base.InstantiateFX(this.m_fxPrefab, targetPos, rotation, true, true);
		}
		for (int i = 0; i < base.Targets.Length; i++)
		{
			if (base.Targets[i] != null)
			{
				Vector3 targetHitPosition = base.GetTargetHitPosition(i);
				Vector3 hitDirection = targetHitPosition - base.Caster.transform.position;
				hitDirection.y = 0f;
				hitDirection.Normalize();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, hitDirection);
				base.Source.OnSequenceHit(this, base.Targets[i], impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
			}
		}
		if (!string.IsNullOrEmpty(this.m_audioEvent))
		{
			AudioManager.PostEvent(this.m_audioEvent, base.Caster.gameObject);
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
		{
			this.SpawnFX();
		}
	}

	private void OnDisable()
	{
		if (this.m_fx != null)
		{
			UnityEngine.Object.Destroy(this.m_fx.gameObject);
			this.m_fx = null;
		}
	}
}
