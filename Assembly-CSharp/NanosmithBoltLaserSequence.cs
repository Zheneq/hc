using System.Collections.Generic;
using UnityEngine;

public class NanosmithBoltLaserSequence : Sequence
{
	public class ExtraParams : IExtraSequenceParams
	{
		public float startDelay;

		public int curIndex;

		public int maxIndex;

		public bool skipImpactFx;

		public bool useOverrideStartPos;

		public Vector3 overrideStartPos = Vector3.zero;

		public Vector3[] boltEndPositions;

		public List<ActorData[]> boltHitActors;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref startDelay);
			stream.Serialize(ref curIndex);
			stream.Serialize(ref maxIndex);
			stream.Serialize(ref skipImpactFx);
			stream.Serialize(ref useOverrideStartPos);
			stream.Serialize(ref overrideStartPos);
			int num;
			if (boltEndPositions != null)
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
				num = boltEndPositions.Length;
			}
			else
			{
				num = 0;
			}
			int value = num;
			stream.Serialize(ref value);
			for (int i = 0; i < value; i++)
			{
				Vector3 value2 = boltEndPositions[i];
				stream.Serialize(ref value2);
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				int num2;
				if (boltHitActors != null)
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
					num2 = boltHitActors.Count;
				}
				else
				{
					num2 = 0;
				}
				int value3 = num2;
				stream.Serialize(ref value3);
				for (int j = 0; j < value3; j++)
				{
					ActorData[] array = boltHitActors[j];
					int value4 = (array != null) ? array.Length : 0;
					stream.Serialize(ref value4);
					for (int k = 0; k < value4; k++)
					{
						ActorData actorData = array[k];
						int num3;
						if (actorData != null)
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
							num3 = actorData.ActorIndex;
						}
						else
						{
							num3 = ActorData.s_invalidActorIndex;
						}
						int value5 = num3;
						stream.Serialize(ref value5);
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							goto end_IL_0146;
						}
						continue;
						end_IL_0146:
						break;
					}
				}
				return;
			}
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref startDelay);
			stream.Serialize(ref curIndex);
			stream.Serialize(ref maxIndex);
			stream.Serialize(ref skipImpactFx);
			stream.Serialize(ref useOverrideStartPos);
			stream.Serialize(ref overrideStartPos);
			int value = 0;
			stream.Serialize(ref value);
			boltEndPositions = new Vector3[value];
			for (int i = 0; i < value; i++)
			{
				Vector3 value2 = Vector3.zero;
				stream.Serialize(ref value2);
				boltEndPositions[i] = value2;
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
				int value3 = 0;
				stream.Serialize(ref value3);
				boltHitActors = new List<ActorData[]>(value3);
				for (int j = 0; j < value3; j++)
				{
					int value4 = 0;
					stream.Serialize(ref value4);
					ActorData[] array = new ActorData[value4];
					for (int k = 0; k < value4; k++)
					{
						int value5 = ActorData.s_invalidActorIndex;
						stream.Serialize(ref value5);
						ActorData actorData = array[k] = GameFlowData.Get().FindActorByActorIndex(value5);
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							goto end_IL_0107;
						}
						continue;
						end_IL_0107:
						break;
					}
					boltHitActors.Add(array);
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
	}

	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	[Tooltip("FX at point(s) of impact")]
	public GameObject m_fxImpactPrefab;

	[JointPopup("Start position for projectile")]
	public JointPopupProperty m_fxJoint;

	public ReferenceModelType m_jointReferenceType;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public Object m_startEvent;

	[JointPopup("FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_hitPosJoint;

	public float m_projectileSpeed;

	[AudioEvent(false)]
	public string m_audioEvent;

	[AudioEvent(false)]
	public string m_impactAudioEvent;

	public float m_startDelay;

	public float m_projectileAcceleration;

	public float m_splineFractionUntilImpact = 1f;

	public bool m_spawnImpactAtFXDespawn;

	[Header("-- For Arcing Projectile Sequence")]
	public float m_maxHeight;

	public bool m_useTargetHitPos;

	public float m_yOffset;

	public bool m_reverseDirection;

	[Header("-- Bolt Projectile Info")]
	public GenericSequenceProjectileAuthoredInfo m_boltProjectileInfo;

	private bool m_startEventHappened;

	private float m_impactDuration;

	protected GameObject m_fx;

	private GameObject m_fxImpact;

	private CRSpline m_spline;

	private float m_curSplineSpeed;

	private float m_splineSpeed;

	private float m_splineAcceleration;

	private float m_splineTraveled;

	private float m_impactDurationLeft;

	private int m_curIndex;

	private int m_maxIndex;

	private bool m_skipImpactFx;

	protected bool m_useOverrideStartPos;

	protected Vector3 m_overrideStartPos = Vector3.zero;

	private bool m_boltsSpawned;

	private Vector3[] m_boltEndPositions;

	private List<ActorData[]> m_boltHitActors;

	private List<GenericSequenceProjectileInfo> m_boltProjectiles = new List<GenericSequenceProjectileInfo>();

	protected bool m_markForRemovalAfterImpact = true;

	public Vector3[] GetSplinePath(int curIndex, int maxIndex)
	{
		Vector3 vector = m_fxJoint.m_jointObject.transform.position;
		if (m_useOverrideStartPos)
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
			vector = m_overrideStartPos;
		}
		Vector3[] array = new Vector3[5];
		if (m_maxHeight == 0f)
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
			Vector3 vector2;
			if (m_useTargetHitPos)
			{
				vector2 = GetTargetHitPosition(0, m_hitPosJoint);
			}
			else
			{
				vector2 = base.TargetPos;
				vector2.y += m_yOffset;
			}
			Vector3 b = vector2 - vector;
			array[0] = vector - b;
			array[1] = vector;
			array[2] = (vector + vector2) * 0.5f;
			array[3] = vector2;
			array[4] = vector2 + b;
		}
		else
		{
			Vector3 vector3;
			if (m_useTargetHitPos)
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
				vector3 = GetTargetHitPosition(0, m_hitPosJoint);
			}
			else
			{
				vector3 = base.TargetPos;
			}
			array[0] = vector + Vector3.down * m_maxHeight;
			array[1] = vector;
			array[2] = (vector + vector3) * 0.5f + Vector3.up * m_maxHeight;
			array[3] = vector3;
			array[4] = vector3 + Vector3.down * m_maxHeight;
		}
		if (m_reverseDirection)
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
			Vector3 vector4 = array[0];
			array[0] = array[4];
			array[4] = vector4;
			vector4 = array[1];
			array[1] = array[3];
			array[3] = vector4;
		}
		return array;
	}

	protected int GetCurrentSplineSegment()
	{
		return m_spline.Section(m_splineTraveled);
	}

	public override void FinishSetup()
	{
		m_impactDuration = Sequence.GetFXDuration(m_fxImpactPrefab);
		if (m_startEvent == null)
		{
			ScheduleFX();
		}
	}

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			ExtraParams extraParams2 = extraSequenceParams as ExtraParams;
			if (extraParams2 != null)
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
				m_curIndex = extraParams2.curIndex;
				m_maxIndex = extraParams2.maxIndex;
				m_startDelay = extraParams2.startDelay;
				m_skipImpactFx = extraParams2.skipImpactFx;
				m_useOverrideStartPos = extraParams2.useOverrideStartPos;
				m_overrideStartPos = extraParams2.overrideStartPos;
				m_boltEndPositions = extraParams2.boltEndPositions;
				m_boltHitActors = extraParams2.boltHitActors;
			}
		}
		while (true)
		{
			switch (7)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void OnDisable()
	{
		using (List<GenericSequenceProjectileInfo>.Enumerator enumerator = m_boltProjectiles.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GenericSequenceProjectileInfo current = enumerator.Current;
				if (current != null)
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
					current.OnSequenceDisable();
				}
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
		OnSequenceDisable();
	}

	protected virtual void OnSequenceDisable()
	{
		if (m_fx != null)
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
			Object.Destroy(m_fx);
			m_fx = null;
		}
		if (m_fxImpact != null)
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
			Object.Destroy(m_fxImpact);
			m_fxImpact = null;
		}
		m_initialized = false;
	}

	protected virtual void SpawnImpactFX(Vector3 impactPos, Quaternion impactRot)
	{
		if (!m_skipImpactFx)
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
				m_fxImpact = InstantiateFX(m_fxImpactPrefab, impactPos, impactRot);
				m_impactDurationLeft = m_impactDuration;
			}
			if (!string.IsNullOrEmpty(m_impactAudioEvent))
			{
				AudioManager.PostEvent(m_impactAudioEvent, m_fx.gameObject);
			}
		}
		CallHitSequenceOnTargets(impactPos);
		SpawnBolts();
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (!(m_startEvent == parameter))
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
			ScheduleFX();
			return;
		}
	}

	private void ScheduleFX()
	{
		m_startEventHappened = true;
	}

	private void SpawnFX()
	{
		Vector3[] splinePath = GetSplinePath(m_curIndex, m_maxIndex);
		m_spline = new CRSpline(splinePath);
		Vector3 a = m_spline.Interp(0.05f);
		(a - splinePath[1]).Normalize();
		Quaternion rotation = default(Quaternion);
		float num = (splinePath[1] - splinePath[2]).magnitude + (splinePath[2] - splinePath[3]).magnitude;
		float num2 = num / m_projectileSpeed;
		m_splineSpeed = 1f / num2;
		m_splineAcceleration = m_projectileAcceleration * m_splineSpeed / m_projectileSpeed;
		if (m_projectileAcceleration == 0f)
		{
			m_curSplineSpeed = m_splineSpeed;
		}
		else
		{
			m_curSplineSpeed = 0f;
		}
		m_fx = InstantiateFX(m_fxPrefab, splinePath[1], rotation);
		if (string.IsNullOrEmpty(m_audioEvent))
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
			AudioManager.PostEvent(m_audioEvent, base.Caster.gameObject);
			return;
		}
	}

	private void Update()
	{
		OnUpdate();
	}

	protected virtual void OnUpdate()
	{
		ProcessSequenceVisibility();
		if (m_initialized && m_startEventHappened)
		{
			if (m_fx == null)
			{
				m_startDelay -= GameTime.deltaTime;
				if (m_startDelay <= 0f)
				{
					GameObject referenceModel = GetReferenceModel(base.Caster, m_jointReferenceType);
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
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						m_fxJoint.Initialize(referenceModel);
					}
					SpawnFX();
				}
			}
			else
			{
				if (m_fx.activeSelf)
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
					m_curSplineSpeed += m_splineAcceleration;
					m_curSplineSpeed = Mathf.Min(m_splineSpeed, m_curSplineSpeed);
					m_splineTraveled += m_curSplineSpeed * GameTime.deltaTime;
					if (m_splineTraveled < m_splineFractionUntilImpact)
					{
						Vector3 vector = m_spline.Interp(m_splineTraveled);
						Quaternion rotation = default(Quaternion);
						rotation.SetLookRotation((vector - m_fx.transform.position).normalized);
						m_fx.transform.position = vector;
						m_fx.transform.rotation = rotation;
					}
					else
					{
						if (m_spawnImpactAtFXDespawn)
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
							SpawnImpactFX(m_fx.transform.position, m_fx.transform.rotation);
						}
						else
						{
							SpawnImpactFX(base.TargetPos, Quaternion.identity);
						}
						m_fx.SetActive(false);
						if (m_fxImpactPrefab == null && m_markForRemovalAfterImpact)
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
						}
					}
				}
				if (m_fxImpact != null)
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
					if (m_fxImpact.activeSelf)
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
						if (m_impactDurationLeft > 0f)
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
							m_impactDurationLeft -= GameTime.deltaTime;
						}
						else if (m_markForRemovalAfterImpact)
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
						}
					}
				}
			}
		}
		using (List<GenericSequenceProjectileInfo>.Enumerator enumerator = m_boltProjectiles.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GenericSequenceProjectileInfo current = enumerator.Current;
				current.OnUpdate();
			}
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private void SpawnBolts()
	{
		if (m_boltsSpawned)
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
			if (m_boltEndPositions == null)
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
				for (int i = 0; i < m_boltEndPositions.Length; i++)
				{
					ActorData[] targetActors = null;
					if (m_boltHitActors.Count > i)
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
						targetActors = m_boltHitActors[i];
					}
					GenericSequenceProjectileInfo item = new GenericSequenceProjectileInfo(this, m_boltProjectileInfo, base.TargetPos, m_boltEndPositions[i], targetActors);
					m_boltProjectiles.Add(item);
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					m_boltsSpawned = true;
					return;
				}
			}
		}
	}
}
