using UnityEngine;

public class TripleTwistProjectileSequence : Sequence
{
	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	[Tooltip("FX at point(s) of impact")]
	public GameObject m_fxImpactPrefab;

	[JointPopup("FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_fxJoint;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public Object m_startEvent;

	[Tooltip("Starting projectile speed.")]
	public float m_projectileSpeed;

	private float m_impactDuration;

	private float m_impactDurationLeft;

	public int m_numProjectiles = 3;

	public float m_projectileOffset = 0.5f;

	private float m_projectileTravelTime;

	private GameObject[] m_projectileFXs;

	private GameObject m_fxImpact;

	private Vector3 m_startPos;

	private Vector3 m_projectileDir;

	[AudioEvent(false)]
	public string m_audioEvent;

	[AudioEvent(false)]
	public string m_impactAudioEvent;

	public float m_rotationSpeed = 180f;

	public override void FinishSetup()
	{
		if (m_startEvent == null)
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
			SpawnFX();
		}
		m_impactDuration = Sequence.GetFXDuration(m_fxImpactPrefab);
	}

	private void SpawnFX()
	{
		m_fxJoint.Initialize(base.Caster.gameObject);
		m_startPos = m_fxJoint.m_jointObject.transform.position;
		m_projectileDir = (base.TargetPos - m_startPos).normalized;
		Quaternion rotation = Quaternion.LookRotation(m_projectileDir);
		m_projectileFXs = new GameObject[m_numProjectiles];
		for (int i = 0; i < m_numProjectiles; i++)
		{
			float angle = (float)i * 360f / (float)m_numProjectiles;
			Quaternion rotation2 = Quaternion.AngleAxis(angle, m_projectileDir);
			Vector3 b = (rotation2 * Vector3.up).normalized * m_projectileOffset;
			m_projectileFXs[i] = InstantiateFX(m_fxPrefab, m_startPos + b, rotation);
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!string.IsNullOrEmpty(m_audioEvent))
			{
				AudioManager.PostEvent(m_audioEvent, base.Caster.gameObject);
			}
			return;
		}
	}

	private void SpawnImpactFX()
	{
		if ((bool)m_fxImpactPrefab)
		{
			while (true)
			{
				switch (7)
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
			m_fxImpact = InstantiateFX(m_fxImpactPrefab, base.TargetPos, Quaternion.identity);
			m_impactDurationLeft = m_impactDuration;
		}
		if (!string.IsNullOrEmpty(m_impactAudioEvent))
		{
			AudioManager.PostEvent(m_impactAudioEvent, m_fxImpact.gameObject);
		}
		CallHitSequenceOnTargets(base.TargetPos);
	}

	private void DeactivateProjectiles()
	{
		if (m_projectileFXs == null)
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
			for (int i = 0; i < m_projectileFXs.Length; i++)
			{
				m_projectileFXs[i].SetActive(false);
			}
			while (true)
			{
				switch (1)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private bool ProjectilesActive()
	{
		bool result = false;
		if (m_projectileFXs != null)
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
			int num = 0;
			while (true)
			{
				if (num < m_projectileFXs.Length)
				{
					if (m_projectileFXs[num].activeSelf)
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
						result = true;
						break;
					}
					num++;
					continue;
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
				break;
			}
		}
		return result;
	}

	private void Update()
	{
		if (!m_initialized || m_projectileFXs == null)
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
			if (ProjectilesActive())
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
				m_projectileTravelTime += GameTime.deltaTime;
				Vector3 a = m_startPos + m_projectileDir * m_projectileTravelTime * m_projectileSpeed;
				if (!(Vector3.Dot(a - m_startPos, a - base.TargetPos) > 0f))
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					for (int i = 0; i < m_numProjectiles; i++)
					{
						float angle = (float)i * 360f / (float)m_numProjectiles + m_projectileTravelTime * m_rotationSpeed;
						Quaternion rotation = Quaternion.AngleAxis(angle, m_projectileDir);
						Vector3 b = (rotation * Vector3.up).normalized * m_projectileOffset;
						m_projectileFXs[i].transform.position = a + b;
					}
				}
				else
				{
					DeactivateProjectiles();
					if (m_fxImpactPrefab != null)
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
						SpawnImpactFX();
					}
					else
					{
						MarkForRemoval();
					}
				}
			}
			if (!(m_fxImpact != null))
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
				if (!m_fxImpact.activeSelf)
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
					if (m_impactDurationLeft > 0f)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								m_impactDurationLeft -= GameTime.deltaTime;
								return;
							}
						}
					}
					MarkForRemoval();
					return;
				}
			}
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (!(parameter == m_startEvent))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			SpawnFX();
			return;
		}
	}

	private void OnDisable()
	{
		if (m_projectileFXs != null)
		{
			while (true)
			{
				switch (7)
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
			for (int i = 0; i < m_projectileFXs.Length; i++)
			{
				if (m_projectileFXs[i] != null)
				{
					Object.Destroy(m_projectileFXs[i].gameObject);
				}
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			m_projectileFXs = null;
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
}
