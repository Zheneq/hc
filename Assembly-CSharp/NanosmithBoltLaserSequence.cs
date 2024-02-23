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
			int boltEndPositionNum = boltEndPositions != null ? boltEndPositions.Length : 0;
			stream.Serialize(ref boltEndPositionNum);
			for (int i = 0; i < boltEndPositionNum; i++)
			{
				Vector3 boltEndPosition = boltEndPositions[i];
				stream.Serialize(ref boltEndPosition);
			}
			int boltHitActorNum = boltHitActors != null ? boltHitActors.Count : 0;
			stream.Serialize(ref boltHitActorNum);
			for (int i = 0; i < boltHitActorNum; i++)
			{
				ActorData[] actors = boltHitActors[i];
				int actorNum = actors != null ? actors.Length : 0;
				stream.Serialize(ref actorNum);
				for (int j = 0; j < actorNum; j++)
				{
					ActorData actorData = actors[j];
					int actorIndex = actorData != null ? actorData.ActorIndex : ActorData.s_invalidActorIndex;
					stream.Serialize(ref actorIndex);
				}
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
			int boltEndPositionNum = 0;
			stream.Serialize(ref boltEndPositionNum);
			boltEndPositions = new Vector3[boltEndPositionNum];
			for (int i = 0; i < boltEndPositionNum; i++)
			{
				Vector3 boltEndPosition = Vector3.zero;
				stream.Serialize(ref boltEndPosition);
				boltEndPositions[i] = boltEndPosition;
			}
			int boltHitActorNum = 0;
			stream.Serialize(ref boltHitActorNum);
			boltHitActors = new List<ActorData[]>(boltHitActorNum);
			for (int i = 0; i < boltHitActorNum; i++)
			{
				int actorNum = 0;
				stream.Serialize(ref actorNum);
				ActorData[] actors = new ActorData[actorNum];
				for (int j = 0; j < actorNum; j++)
				{
					int actorIndex = ActorData.s_invalidActorIndex;
					stream.Serialize(ref actorIndex);
					actors[j] = GameFlowData.Get().FindActorByActorIndex(actorIndex);
				}
				boltHitActors.Add(actors);
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
			vector = m_overrideStartPos;
		}
		Vector3[] array = new Vector3[5];
		if (m_maxHeight == 0f)
		{
			Vector3 vector2;
			if (m_useTargetHitPos)
			{
				vector2 = GetTargetHitPosition(0, m_hitPosJoint);
			}
			else
			{
				vector2 = TargetPos;
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
				vector3 = GetTargetHitPosition(0, m_hitPosJoint);
			}
			else
			{
				vector3 = TargetPos;
			}
			array[0] = vector + Vector3.down * m_maxHeight;
			array[1] = vector;
			array[2] = (vector + vector3) * 0.5f + Vector3.up * m_maxHeight;
			array[3] = vector3;
			array[4] = vector3 + Vector3.down * m_maxHeight;
		}
		if (m_reverseDirection)
		{
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
		m_impactDuration = GetFXDuration(m_fxImpactPrefab);
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
	}

	private void OnDisable()
	{
		foreach (GenericSequenceProjectileInfo projectile in m_boltProjectiles)
		{
			if (projectile != null)
			{
				projectile.OnSequenceDisable();
			}
		}
		OnSequenceDisable();
	}

	protected virtual void OnSequenceDisable()
	{
		if (m_fx != null)
		{
			Destroy(m_fx);
			m_fx = null;
		}
		if (m_fxImpact != null)
		{
			Destroy(m_fxImpact);
			m_fxImpact = null;
		}
		m_initialized = false;
	}

	protected virtual void SpawnImpactFX(Vector3 impactPos, Quaternion impactRot)
	{
		if (!m_skipImpactFx)
		{
			if (m_fxImpactPrefab)
			{
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
		if (m_startEvent == parameter)
		{
			ScheduleFX();
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
		m_curSplineSpeed = m_projectileAcceleration == 0f ? m_splineSpeed : 0f;
		m_fx = InstantiateFX(m_fxPrefab, splinePath[1], rotation);
		if (!string.IsNullOrEmpty(m_audioEvent))
		{
			AudioManager.PostEvent(m_audioEvent, Caster.gameObject);
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
					GameObject referenceModel = GetReferenceModel(Caster, m_jointReferenceType);
					if (referenceModel != null)
					{
						m_fxJoint.Initialize(referenceModel);
					}
					SpawnFX();
				}
			}
			else
			{
				if (m_fx.activeSelf)
				{
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
							SpawnImpactFX(m_fx.transform.position, m_fx.transform.rotation);
						}
						else
						{
							SpawnImpactFX(TargetPos, Quaternion.identity);
						}
						m_fx.SetActive(false);
						if (m_fxImpactPrefab == null && m_markForRemovalAfterImpact)
						{
						}
					}
				}
				if (m_fxImpact != null && m_fxImpact.activeSelf)
				{
					if (m_impactDurationLeft > 0f)
					{
						m_impactDurationLeft -= GameTime.deltaTime;
					}
					else if (m_markForRemovalAfterImpact)
					{
					}
				}
			}
		}
		foreach (GenericSequenceProjectileInfo projectile in m_boltProjectiles)
		{
			projectile.OnUpdate();
		}
	}

	private void SpawnBolts()
	{
		if (m_boltsSpawned || m_boltEndPositions == null)
		{
			return;
		}
		for (int i = 0; i < m_boltEndPositions.Length; i++)
		{
			ActorData[] targetActors = null;
			if (m_boltHitActors.Count > i)
			{
				targetActors = m_boltHitActors[i];
			}
			GenericSequenceProjectileInfo item = new GenericSequenceProjectileInfo(this, m_boltProjectileInfo, TargetPos, m_boltEndPositions[i], targetActors);
			m_boltProjectiles.Add(item);
		}
		m_boltsSpawned = true;
	}
}
