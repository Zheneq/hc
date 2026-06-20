using UnityEngine;

public class TwoPointCycleSequence : Sequence
{
	public enum CycleType
	{
		CasterToTarget,
		TargetToCaster,
		PingPong
	}

	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	[JointPopup("FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_fxJoint;

	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[AnimEventPicker]
	public Object m_startEvent;

	public CycleType m_cycleType = CycleType.TargetToCaster;

	[AudioEvent(false)]
	public string m_audioEvent;

	public float m_projectileSpeed = 10f;

	private float m_parameterizedPosition;

	private GameObject m_fx;

	public override void FinishSetup()
	{
		if (!(m_startEvent == null))
		{
			return;
		}
		while (true)
		{
			SpawnFX();
			return;
		}
	}

	private float GetCyclePosition()
	{
		float num = 0f;
		if (m_cycleType == CycleType.PingPong)
		{
			m_parameterizedPosition += m_projectileSpeed * GameTime.deltaTime;
			if (m_parameterizedPosition > 1f)
			{
				m_parameterizedPosition = 0f;
			}
			num = m_parameterizedPosition * 2f;
			if (num > 1f)
			{
				num = 2f - num;
			}
		}
		else if (m_cycleType == CycleType.CasterToTarget)
		{
			m_parameterizedPosition += m_projectileSpeed * GameTime.deltaTime;
			if (m_parameterizedPosition > 1f)
			{
				m_parameterizedPosition = 0f;
			}
			num = m_parameterizedPosition;
		}
		else if (m_cycleType == CycleType.TargetToCaster)
		{
			m_parameterizedPosition -= m_projectileSpeed * GameTime.deltaTime;
			if (m_parameterizedPosition < 0f)
			{
				m_parameterizedPosition = 1f;
			}
			num = m_parameterizedPosition;
		}
		return num;
	}

	private void UpdatePosition()
	{
		if (!(m_fx != null))
		{
			return;
		}
		while (true)
		{
			float cyclePosition = GetCyclePosition();
			Vector3 position = m_fxJoint.m_jointObject.transform.position;
			Vector3 targetHitPosition = GetTargetHitPosition(0);
			Vector3 a = targetHitPosition - position;
			Vector3 vector = position + a * cyclePosition;
			Vector3 vector2 = vector - m_fx.transform.position;
			m_fx.transform.position = vector;
			m_fx.transform.rotation = Quaternion.LookRotation(vector2.normalized);
			return;
		}
	}

	private void Update()
	{
		if (!m_initialized)
		{
			return;
		}
		while (true)
		{
			UpdatePosition();
			ProcessSequenceVisibility();
			return;
		}
	}

	private void SpawnFX()
	{
		if (!m_fxJoint.IsInitialized())
		{
			m_fxJoint.Initialize(base.Caster.gameObject);
		}
		if (m_fxPrefab != null)
		{
			Vector3 position = m_fxJoint.m_jointObject.transform.position;
			Quaternion rotation = default(Quaternion);
			Vector3 targetPosition = GetTargetPosition(0);
			Vector3 lookRotation = targetPosition - position;
			lookRotation.y = 0f;
			lookRotation.Normalize();
			rotation.SetLookRotation(lookRotation);
			m_fx = InstantiateFX(m_fxPrefab, position, rotation);
		}
		for (int i = 0; i < base.Targets.Length; i++)
		{
			if (base.Targets[i] != null)
			{
				Vector3 targetHitPosition = GetTargetHitPosition(i);
				Vector3 hitDirection = targetHitPosition - base.Caster.transform.position;
				hitDirection.y = 0f;
				hitDirection.Normalize();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, hitDirection);
				base.Source.OnSequenceHit(this, base.Targets[i], impulseInfo);
			}
		}
		while (true)
		{
			if (!string.IsNullOrEmpty(m_audioEvent))
			{
				while (true)
				{
					AudioManager.PostEvent(m_audioEvent, base.Caster.gameObject);
					return;
				}
			}
			return;
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
			SpawnFX();
			return;
		}
	}

	private void OnDisable()
	{
		if (!(m_fx != null))
		{
			return;
		}
		while (true)
		{
			Object.Destroy(m_fx.gameObject);
			m_fx = null;
			return;
		}
	}
}
