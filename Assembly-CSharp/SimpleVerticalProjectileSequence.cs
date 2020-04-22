using UnityEngine;

public class SimpleVerticalProjectileSequence : Sequence
{
	[Separator("Fx Prefabs", true)]
	public GameObject m_fxPrefab;

	[Tooltip("FX at point of impact")]
	public GameObject m_fxImpactPrefab;

	[JointPopup("Start position for projectiles that are traveling up")]
	public JointPopupProperty m_fxJoint;

	[Separator("Travel Direction. If [Traveling Up] is set, Height Offset is how high projectile up go upward", true)]
	public bool m_travelingUp;

	[Header("-- Whether to align projectile to joint's direction on moment of spawn")]
	public bool m_useJointDirectionForProjectile;

	public float m_heightOffset;

	public float m_projectileSpeed;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public Object m_startEvent;

	[Tooltip("Spawn FX if Caster is Dead on Start")]
	public bool m_spawnFxOnStartIfCasterIsDead;

	[Separator("Audio", true)]
	[AudioEvent(false)]
	public string m_audioEvent;

	[AudioEvent(false)]
	public string m_impactAudioEvent;

	private GameObject m_fx;

	private GameObject m_fxImpact;

	private Vector3 m_startPos;

	private float m_impactDurationLeft;

	private float m_impactDuration;

	private Vector3 m_endPos = Vector3.zero;

	private Vector3 m_travelDir = Vector3.up;

	public override void FinishSetup()
	{
		m_impactDurationLeft = 0f;
		m_fxJoint.Initialize(base.Caster.gameObject);
		m_impactDuration = Sequence.GetFXDuration(m_fxImpactPrefab);
		if (!(m_startEvent == null))
		{
			while (true)
			{
				switch (6)
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
			if (!m_spawnFxOnStartIfCasterIsDead)
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!(base.Caster != null))
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!base.Caster.IsDead())
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		SpawnFX();
	}

	private void Update()
	{
		ProcessSequenceVisibility();
		if (!m_initialized)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(m_fx != null))
			{
				return;
			}
			if (m_fx.activeSelf)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 vector = m_fx.transform.position + m_travelDir * m_projectileSpeed * GameTime.deltaTime;
				Vector3 lhs = m_endPos - vector;
				Vector3 rhs = m_startPos - vector;
				if (Vector3.Dot(lhs, rhs) > 0f)
				{
					m_fx.SetActive(false);
					if (m_fxImpactPrefab != null)
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
						SpawnImpactFX(m_endPos);
					}
					else
					{
						base.Source.OnSequenceHit(this, base.TargetPos);
						if (base.Targets != null)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							ActorData[] targets = base.Targets;
							foreach (ActorData target in targets)
							{
								base.Source.OnSequenceHit(this, target, Sequence.CreateImpulseInfoWithObjectPose(m_fx));
							}
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						MarkForRemoval();
					}
				}
				else
				{
					m_fx.transform.position = vector;
				}
			}
			if (!(m_fxImpact != null) || !m_fxImpact.activeSelf)
			{
				return;
			}
			if (m_impactDurationLeft > 0f)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				m_impactDurationLeft -= GameTime.deltaTime;
			}
			if (m_impactDurationLeft <= 0f)
			{
				MarkForRemoval();
			}
			return;
		}
	}

	private void OnDisable()
	{
		if (m_fx != null)
		{
			while (true)
			{
				switch (6)
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
			Object.Destroy(m_fx);
			m_fx = null;
		}
		if (m_fxImpact != null)
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
			Object.Destroy(m_fxImpact);
			m_fxImpact = null;
		}
		m_initialized = false;
	}

	private void SpawnImpactFX(Vector3 impactPos)
	{
		if ((bool)m_fxImpactPrefab)
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
			m_fxImpact = InstantiateFX(m_fxImpactPrefab, impactPos, Quaternion.identity);
			m_impactDurationLeft = m_impactDuration;
		}
		if (!string.IsNullOrEmpty(m_impactAudioEvent))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			AudioManager.PostEvent(m_impactAudioEvent, m_fx.gameObject);
		}
		CallHitSequenceOnTargets(impactPos);
		CameraManager.Get().PlayCameraShake(CameraManager.CameraShakeIntensity.Large);
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (m_startEvent == parameter)
		{
			SpawnFX();
		}
	}

	private void SpawnFX()
	{
		if (!(m_fx == null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(m_fxPrefab != null))
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				Quaternion rotation = default(Quaternion);
				if (m_travelingUp)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					m_startPos = m_fxJoint.m_jointObject.transform.position;
					m_travelDir = Vector3.up;
					if (m_useJointDirectionForProjectile)
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
						if (!base.Caster.IsDead())
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							m_travelDir = m_fxJoint.m_jointObject.transform.forward;
						}
					}
					rotation.SetLookRotation(m_travelDir);
					m_endPos = m_travelDir * m_heightOffset;
					Debug.DrawRay(m_startPos, 10f * m_travelDir, Color.green, 10f);
				}
				else
				{
					m_startPos = base.TargetPos;
					m_startPos.y += m_heightOffset;
					m_endPos = base.TargetPos;
					m_travelDir = -1f * Vector3.up;
					rotation.SetLookRotation(m_travelDir);
				}
				m_fx = InstantiateFX(m_fxPrefab, m_startPos, rotation);
				if (m_fx != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					FriendlyEnemyVFXSelector component = m_fx.GetComponent<FriendlyEnemyVFXSelector>();
					if (component != null)
					{
						component.Setup(base.Caster.GetTeam());
					}
				}
				if (!string.IsNullOrEmpty(m_audioEvent))
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						AudioManager.PostEvent(m_audioEvent, base.Caster.gameObject);
						return;
					}
				}
				return;
			}
		}
	}
}
