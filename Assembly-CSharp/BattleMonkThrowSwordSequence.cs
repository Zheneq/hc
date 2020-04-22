using UnityEngine;

public class BattleMonkThrowSwordSequence : SplineProjectileSequence
{
	[Header("-- Prefabs")]
	public GameObject m_swordTempSatellitePrefab;

	public GameObject m_linePrefab;

	[Header("-- Sword / Satellite Info")]
	[AnimEventPicker]
	public Object m_swordRemoveEvent;

	public float m_splineMaxHeight;

	public float m_satelliteHeightOffset;

	[Header("-- Line Info")]
	public bool m_lineSpawnOnProjectileImpact;

	[AnimEventPicker]
	public Object m_lineStartEvent;

	[AnimEventPicker]
	public Object m_lineRemoveEvent;

	public float m_lineDuration = -1f;

	[AudioEvent(false)]
	public string m_lineAudioEvent;

	[JointPopup("Line Start Joint Caster")]
	public JointPopupProperty m_lineFxCasterStartJoint;

	[JointPopup("Line End Joint Sword Satellite")]
	public JointPopupProperty m_lineFxSatelliteJoint;

	private GameObject m_swordTempSatelliteInstance;

	private GameObject m_lineFx;

	private float m_lineDespawnTime;

	private bool m_lineSpawned;

	public override void FinishSetup()
	{
		base.FinishSetup();
		if (m_lineStartEvent == null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!m_lineSpawnOnProjectileImpact)
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
				SpawnLineFX();
			}
		}
		m_markForRemovalAfterImpact = false;
		m_doHitsAsProjectileTravels = false;
	}

	protected override void SpawnImpactFX(Vector3 impactPos, Quaternion impactRot)
	{
		base.SpawnImpactFX(impactPos, impactRot);
		m_swordTempSatelliteInstance = InstantiateFX(m_swordTempSatellitePrefab, base.TargetPos + new Vector3(0f, m_satelliteHeightOffset, 0f), Quaternion.identity);
		if (m_swordTempSatelliteInstance != null)
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
			TempSatellite component = m_swordTempSatelliteInstance.GetComponent<TempSatellite>();
			component.Setup(this);
			component.SetNotifyOwnerOnAnimEvent(false);
			if (m_lineSpawned)
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
				if (!m_lineFxSatelliteJoint.IsInitialized())
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
					GameObject swordTempSatelliteInstance = m_swordTempSatelliteInstance;
					if (swordTempSatelliteInstance != null)
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
						m_lineFxSatelliteJoint.Initialize(swordTempSatelliteInstance);
					}
				}
			}
		}
		if (!m_lineSpawnOnProjectileImpact)
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
			SpawnLineFX();
			return;
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		base.OnAnimationEvent(parameter, sourceObject);
		if (m_lineStartEvent == parameter)
		{
			SpawnLineFX();
		}
		if (m_swordRemoveEvent == parameter)
		{
			DestroySatellite();
		}
		if (m_lineRemoveEvent == parameter)
		{
			DestroyLine();
		}
	}

	private void SpawnLineFX()
	{
		if (m_lineSpawned)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		if (!m_lineFxCasterStartJoint.IsInitialized())
		{
			GameObject referenceModel = GetReferenceModel(base.Caster, ReferenceModelType.Actor);
			if (referenceModel != null)
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
				m_lineFxCasterStartJoint.Initialize(referenceModel);
			}
		}
		if (!m_lineFxSatelliteJoint.IsInitialized())
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
			GameObject swordTempSatelliteInstance = m_swordTempSatelliteInstance;
			if (swordTempSatelliteInstance != null)
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
				m_lineFxSatelliteJoint.Initialize(swordTempSatelliteInstance);
			}
		}
		if (m_linePrefab != null)
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
			Vector3 position = m_lineFxCasterStartJoint.m_jointObject.transform.position;
			m_lineFx = InstantiateFX(m_linePrefab, position, default(Quaternion));
		}
		if (!string.IsNullOrEmpty(m_lineAudioEvent))
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
			AudioManager.PostEvent(m_lineAudioEvent, base.Caster.gameObject);
		}
		if (m_lineDuration > 0f)
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
			m_lineDespawnTime = GameTime.time + m_lineDuration;
		}
		else
		{
			m_lineDespawnTime = -1f;
		}
		m_lineSpawned = true;
	}

	private void Update()
	{
		OnUpdate();
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (!m_initialized)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_lineFxCasterStartJoint.m_jointObject != null)
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
				Sequence.SetAttribute(m_lineFx, "startPoint", m_lineFxCasterStartJoint.m_jointObject.transform.position);
			}
			if (m_lineFxSatelliteJoint.m_jointObject != null)
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
				Sequence.SetAttribute(m_lineFx, "endPoint", m_lineFxSatelliteJoint.m_jointObject.transform.position);
			}
			else if (m_lineFxCasterStartJoint.m_jointObject != null)
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
				if (m_fx != null)
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
					Sequence.SetAttribute(m_lineFx, "endPoint", m_fx.transform.position);
				}
			}
			if (!(m_lineDespawnTime < GameTime.time))
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
				if (m_lineDespawnTime > 0f)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						DestroyLine();
						return;
					}
				}
				return;
			}
		}
	}

	protected override void OnSequenceDisable()
	{
		base.OnSequenceDisable();
		DestroyLine();
		DestroySatellite();
	}

	private void DestroyLine()
	{
		if (!(m_lineFx != null))
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
			Object.Destroy(m_lineFx);
			m_lineFx = null;
			return;
		}
	}

	private void DestroySatellite()
	{
		if (m_swordTempSatelliteInstance != null)
		{
			Object.Destroy(m_swordTempSatelliteInstance);
			m_swordTempSatelliteInstance = null;
		}
	}

	internal override Vector3[] GetSplinePath(int curIndex, int maxIndex)
	{
		Vector3 vector = m_fxJoint.m_jointObject.transform.position;
		if (m_useOverrideStartPos)
		{
			vector = m_overrideStartPos;
		}
		Vector3[] array = new Vector3[5];
		Vector3 targetPos = base.TargetPos;
		if (m_splineMaxHeight == 0f)
		{
			Vector3 b = targetPos - vector;
			array[0] = vector - b;
			array[1] = vector;
			array[2] = (vector + targetPos) * 0.5f;
			array[3] = targetPos;
			array[4] = targetPos + b;
		}
		else
		{
			array[0] = vector + Vector3.down * m_splineMaxHeight;
			array[1] = vector;
			array[2] = (vector + targetPos) * 0.5f + Vector3.up * m_splineMaxHeight;
			array[3] = targetPos;
			array[4] = targetPos + Vector3.down * m_splineMaxHeight;
		}
		return array;
	}
}
