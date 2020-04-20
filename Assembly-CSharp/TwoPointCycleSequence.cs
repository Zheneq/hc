using System;
using UnityEngine;

public class TwoPointCycleSequence : Sequence
{
	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	[JointPopup("FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_fxJoint;

	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[AnimEventPicker]
	public UnityEngine.Object m_startEvent;

	public TwoPointCycleSequence.CycleType m_cycleType = TwoPointCycleSequence.CycleType.TargetToCaster;

	[AudioEvent(false)]
	public string m_audioEvent;

	public float m_projectileSpeed = 10f;

	private float m_parameterizedPosition;

	private GameObject m_fx;

	public override void FinishSetup()
	{
		if (this.m_startEvent == null)
		{
			this.SpawnFX();
		}
	}

	private float GetCyclePosition()
	{
		float num = 0f;
		if (this.m_cycleType == TwoPointCycleSequence.CycleType.PingPong)
		{
			this.m_parameterizedPosition += this.m_projectileSpeed * GameTime.deltaTime;
			if (this.m_parameterizedPosition > 1f)
			{
				this.m_parameterizedPosition = 0f;
			}
			num = this.m_parameterizedPosition * 2f;
			if (num > 1f)
			{
				num = 2f - num;
			}
		}
		else if (this.m_cycleType == TwoPointCycleSequence.CycleType.CasterToTarget)
		{
			this.m_parameterizedPosition += this.m_projectileSpeed * GameTime.deltaTime;
			if (this.m_parameterizedPosition > 1f)
			{
				this.m_parameterizedPosition = 0f;
			}
			num = this.m_parameterizedPosition;
		}
		else if (this.m_cycleType == TwoPointCycleSequence.CycleType.TargetToCaster)
		{
			this.m_parameterizedPosition -= this.m_projectileSpeed * GameTime.deltaTime;
			if (this.m_parameterizedPosition < 0f)
			{
				this.m_parameterizedPosition = 1f;
			}
			num = this.m_parameterizedPosition;
		}
		return num;
	}

	private void UpdatePosition()
	{
		if (this.m_fx != null)
		{
			float cyclePosition = this.GetCyclePosition();
			Vector3 position = this.m_fxJoint.m_jointObject.transform.position;
			Vector3 targetHitPosition = base.GetTargetHitPosition(0);
			Vector3 a = targetHitPosition - position;
			Vector3 vector = position + a * cyclePosition;
			Vector3 vector2 = vector - this.m_fx.transform.position;
			this.m_fx.transform.position = vector;
			this.m_fx.transform.rotation = Quaternion.LookRotation(vector2.normalized);
		}
	}

	private void Update()
	{
		if (this.m_initialized)
		{
			this.UpdatePosition();
			base.ProcessSequenceVisibility();
		}
	}

	private void SpawnFX()
	{
		if (!this.m_fxJoint.IsInitialized())
		{
			this.m_fxJoint.Initialize(base.Caster.gameObject);
		}
		if (this.m_fxPrefab != null)
		{
			Vector3 position = this.m_fxJoint.m_jointObject.transform.position;
			Quaternion rotation = default(Quaternion);
			Vector3 targetPosition = base.GetTargetPosition(0, false);
			Vector3 lookRotation = targetPosition - position;
			lookRotation.y = 0f;
			lookRotation.Normalize();
			rotation.SetLookRotation(lookRotation);
			this.m_fx = base.InstantiateFX(this.m_fxPrefab, position, rotation, true, true);
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

	public enum CycleType
	{
		CasterToTarget,
		TargetToCaster,
		PingPong
	}
}
