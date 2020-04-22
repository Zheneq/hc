using UnityEngine;

public class HomingProjectileSequence : Sequence
{
	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	public GameObject m_hitFxPrefab;

	[JointPopup("FX attach joint on the caster")]
	public JointPopupProperty m_fxCasterJoint;

	public ReferenceModelType m_fxCasterJointReferenceType;

	[JointPopup("FX attach joint on the target")]
	public JointPopupProperty m_fxTargetJoint;

	[AudioEvent(false)]
	[Tooltip("Audio event to play when the projectile initially fires")]
	public string m_audioEvent;

	[AudioEvent(false)]
	[Tooltip("Audio event to play when the projectile hits a target")]
	public string m_impactAudioEvent;

	public float m_shotRange = 4f;

	public float m_projectileSpeed = 10f;

	public float m_initialProjectileTurnSpeed = 180f;

	public float m_projectileTurnAcceleration = 600f;

	private GameObject m_fx;

	private GameObject m_hitFx;

	private JointPopupProperty m_joint;

	private float m_curTurnSpeed;

	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[AnimEventPicker]
	public Object m_startEvent;

	private void OnDisable()
	{
		Object.Destroy(m_fx);
		Object.Destroy(m_hitFx);
	}

	public override void FinishSetup()
	{
		GameObject referenceModel = GetReferenceModel(base.Caster, m_fxCasterJointReferenceType);
		if (referenceModel != null)
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
			m_fxCasterJoint.Initialize(referenceModel);
		}
		if (!(m_startEvent == null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			FireAtTarget();
			return;
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (m_startEvent == parameter)
		{
			FireAtTarget();
		}
	}

	private void FireAtTarget()
	{
		m_fx = InstantiateFX(m_fxPrefab, m_fxCasterJoint.m_jointObject.transform.position, Quaternion.LookRotation(base.Caster.transform.forward));
		m_curTurnSpeed = m_initialProjectileTurnSpeed;
		if (!string.IsNullOrEmpty(m_audioEvent))
		{
			AudioManager.PostEvent(m_audioEvent, base.Caster.gameObject);
		}
		JointPopupProperty jointPopupProperty = new JointPopupProperty();
		jointPopupProperty.m_joint = m_fxTargetJoint.m_joint;
		jointPopupProperty.m_jointCharacter = m_fxTargetJoint.m_jointCharacter;
		jointPopupProperty.Initialize(base.Target.gameObject);
		m_joint = jointPopupProperty;
	}

	private void SpawnHitFx()
	{
		if (m_hitFxPrefab != null)
		{
			while (true)
			{
				switch (2)
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
			GameObject gameObject = m_hitFx = InstantiateFX(m_hitFxPrefab, m_joint.m_jointObject.transform.position, Quaternion.identity);
			if (!string.IsNullOrEmpty(m_impactAudioEvent))
			{
				AudioManager.PostEvent(m_impactAudioEvent, m_fx.gameObject);
			}
		}
		Vector3 position = m_joint.m_jointObject.transform.position;
		Vector3 forward = m_fx.transform.forward;
		forward.Normalize();
		ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, forward);
		base.Source.OnSequenceHit(this, base.Target, impulseInfo);
	}

	private void UpdateShot()
	{
		if (!(m_fx != null) || !(m_fx.transform != null))
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
			if (m_joint == null)
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (!(m_joint.m_jointObject != null))
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
					if (!(m_joint.m_jointObject.transform != null))
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
						Vector3 position = m_fx.transform.position;
						Vector3 forward = m_fx.transform.forward;
						m_curTurnSpeed += m_projectileTurnAcceleration * GameTime.deltaTime;
						Vector3 vector = m_joint.m_jointObject.transform.position - m_fx.transform.position;
						float magnitude = vector.magnitude;
						Quaternion rotation = Quaternion.RotateTowards(Quaternion.LookRotation(forward), Quaternion.LookRotation(vector), m_curTurnSpeed * GameTime.deltaTime);
						Vector3 vector2 = rotation * Vector3.forward;
						Vector3 vector3 = position + vector2 * m_projectileSpeed * GameTime.deltaTime;
						Vector3 lhs = m_joint.m_jointObject.transform.position - vector3;
						int num;
						if (!(magnitude <= 0.5f))
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
							if (Vector3.Dot(lhs, vector) <= 0f)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								num = ((magnitude < 2f) ? 1 : 0);
							}
							else
							{
								num = 0;
							}
						}
						else
						{
							num = 1;
						}
						if (num != 0)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									SpawnHitFx();
									Object.Destroy(m_fx);
									return;
								}
							}
						}
						m_fx.transform.position = vector3;
						m_fx.transform.forward = vector2;
						return;
					}
				}
			}
		}
	}

	private void Update()
	{
		UpdateShot();
	}
}
