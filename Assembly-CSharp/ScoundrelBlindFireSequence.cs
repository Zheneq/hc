using System.Collections.Generic;
using UnityEngine;

public class ScoundrelBlindFireSequence : Sequence
{
	public class ConeExtraParams : IExtraSequenceParams
	{
		public float halfAngleDegrees = -1f;
		public float maxDistInSquares = -1f;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref halfAngleDegrees);
			stream.Serialize(ref maxDistInSquares);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref halfAngleDegrees);
			stream.Serialize(ref maxDistInSquares);
		}
	}

	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;
	[Tooltip("FX at point(s) of impact")]
	public GameObject m_fxImpactPrefab;
	[JointPopup("hit FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_hitFxJoint;
	[JointPopup("FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_fxJointLeft;
	[JointPopup("FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_fxJointRight;
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[AnimEventPicker]
	public Object m_startEventLeft;
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[AnimEventPicker]
	public Object m_startEventRight;
	[AnimEventPicker]
	public Object m_hitReactEvent;
	[AnimEventPicker]
	public Object m_lastHitReactEvent;
	[Tooltip("Amount of time to trigger actual impact after hit react event has been received")]
	public float m_hitImpactDelayTime = -1f;
	public float m_maxDistInSquares = 7f;
	[Header("    (this is half angle, to left and right of center direction)")]
	public float m_angleRange = 30f;
	[Tooltip("will assume sequence will shoot at least 1 at the moment")]
	public int m_projectilesPerAnimEvent = 1;
	public bool m_raycastDistanceFromLosHeight = true;
	[Header("    ( if 1 projectile at a time, estimated number of projectiles total )")]
	public int m_expectedNumProjectilesForEdgeShots = -1;
	[Header("    ( chance to add extra shots at edges beyond guaranteed ones )")]
	public float m_extraEdgeShotChance;

	private List<GameObject> m_FXs;
	private List<GameObject> m_fxImpacts;

	[AudioEvent(false)]
	public string m_audioEvent;
	[AudioEvent(false)]
	public string m_impactAudioEvent;
	[Header("-- Alternative IMpact Audio Events, handled per ability, unused otherwise")]
	[AudioEvent(false)]
	public string[] m_alternativeImpactAudioEvents;

	private int m_alternativeAudioIndex = -1;
	private int m_numSpawnAttempts;
	private int m_spawnIndexEdgeMin = -1;
	private int m_spawnIndexEdgeMax = -1;
	private List<SimpleAttachedVFXSequence.DelayedImpact> m_delayedImpacts = new List<SimpleAttachedVFXSequence.DelayedImpact>();

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			ConeExtraParams coneExtraParams = extraSequenceParams as ConeExtraParams;
			if (coneExtraParams != null)
			{
				if (coneExtraParams.maxDistInSquares > 0f)
				{
					m_maxDistInSquares = coneExtraParams.maxDistInSquares;
				}
				if (coneExtraParams.halfAngleDegrees > 0f)
				{
					m_angleRange = coneExtraParams.halfAngleDegrees;
				}
			}

			SimpleAttachedVFXSequence.ImpactDelayParams impactDelayParams = extraSequenceParams as SimpleAttachedVFXSequence.ImpactDelayParams;
			if (impactDelayParams != null)
			{
				if (impactDelayParams.impactDelayTime > 0f)
				{
					m_hitImpactDelayTime = impactDelayParams.impactDelayTime;
				}
				if (impactDelayParams.alternativeImpactAudioIndex >= 0)
				{
					m_alternativeAudioIndex = impactDelayParams.alternativeImpactAudioIndex;
				}
			}
		}
	}

	public override void FinishSetup()
	{
		m_FXs = new List<GameObject>();
		m_fxImpacts = new List<GameObject>();
		if (m_projectilesPerAnimEvent > 20 || m_projectilesPerAnimEvent < 1)
		{
			m_projectilesPerAnimEvent = 1;
		}
		if (m_projectilesPerAnimEvent <= 1)
		{
			int num = m_expectedNumProjectilesForEdgeShots;
			if (num <= 0)
			{
				num = 4;
			}
			m_spawnIndexEdgeMin = Random.Range(0, num);
			m_spawnIndexEdgeMax = Random.Range(0, num);
			if (m_spawnIndexEdgeMax == m_spawnIndexEdgeMin)
			{
				m_spawnIndexEdgeMax = (m_spawnIndexEdgeMax + 1) % num;
			}
		}
	}

	private float GetProjectileDistance(Vector3 start, Vector3 forward, float maxDist)
	{
		Vector3 vector = start;
		if (m_raycastDistanceFromLosHeight)
		{
			vector.y = Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
		}
		else
		{
			vector.y += 0.5f;
		}
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(vector, forward, maxDist, false, Caster);
		return (vector - laserEndPoint).magnitude;
	}

	private void SpawnFX(bool left)
	{
		JointPopupProperty jointPopupProperty = left ? m_fxJointLeft : m_fxJointRight;
		if (!jointPopupProperty.IsInitialized())
		{
			jointPopupProperty.Initialize(Caster.gameObject);
		}
		Vector3 forward = jointPopupProperty.m_jointObject.transform.forward;
		forward.y = 0f;
		forward.Normalize();
		int projectilesPerAnimEvent = m_projectilesPerAnimEvent;
		int num = -1;
		int num2 = -1;
		if (m_angleRange < 180f)
		{
			if (projectilesPerAnimEvent > 1)
			{
				num = Random.Range(0, projectilesPerAnimEvent);
				num2 = Random.Range(0, projectilesPerAnimEvent);
				if (num2 == num)
				{
					num2 = (num2 + 1) % projectilesPerAnimEvent;
				}
			}
			else if (projectilesPerAnimEvent == 1)
			{
				if (m_numSpawnAttempts == m_spawnIndexEdgeMin)
				{
					num = m_spawnIndexEdgeMin;
				}
				else if (m_numSpawnAttempts == m_spawnIndexEdgeMax)
				{
					num2 = m_spawnIndexEdgeMax;
				}
			}
		}
		for (int i = 0; i < projectilesPerAnimEvent; i++)
		{
			float angle = Random.Range(-m_angleRange, m_angleRange);
			if (i == num)
			{
				angle = -m_angleRange;
			}
			else if (i == num2)
			{
				angle = m_angleRange;
			}
			GameObject item = CreateProjectileFx(forward, angle, jointPopupProperty);
			m_FXs.Add(item);
			if (Random.Range(0f, 1f) < m_extraEdgeShotChance)
			{
				float angle2 = -m_angleRange;
				if (Random.Range(0f, 1f) < 0.5f)
				{
					angle2 = m_angleRange;
				}
				GameObject item2 = CreateProjectileFx(forward, angle2, jointPopupProperty);
				m_FXs.Add(item2);
			}
		}
		if (!string.IsNullOrEmpty(m_audioEvent))
		{
			AudioManager.PostEvent(m_audioEvent, Caster.gameObject);
		}
		if (m_hitReactEvent == null && m_lastHitReactEvent == null)
		{
			if (m_hitImpactDelayTime > 0f)
			{
				m_delayedImpacts.Add(new SimpleAttachedVFXSequence.DelayedImpact(GameTime.time + m_hitImpactDelayTime, true));
			}
			else
			{
				SpawnImpactFX(true);
			}
		}
		m_numSpawnAttempts++;
	}

	private GameObject CreateProjectileFx(Vector3 forward, float angle, JointPopupProperty fxJoint)
	{
		Vector3 dir = Quaternion.AngleAxis(angle, Vector3.up) * forward;
		GameObject gameObject = InstantiateFX(m_fxPrefab, fxJoint.m_jointObject.transform.position, Quaternion.LookRotation(dir));
		float projectileDistance = GetProjectileDistance(Caster.transform.position, dir, m_maxDistInSquares * 1.5f);
		SetAttribute(gameObject, "projectileDistance", projectileDistance);
		Debug.DrawRay(fxJoint.m_jointObject.transform.position, projectileDistance * dir, Color.red, 5f);
		return gameObject;
	}

	private void SpawnImpactFX(bool lastHit)
	{
		if (Targets != null)
		{
			for (int i = 0; i < Targets.Length; i++)
			{
				Vector3 targetHitPosition = GetTargetHitPosition(i, m_hitFxJoint);
				Vector3 hitDirection = targetHitPosition - Caster.transform.position;
				hitDirection.y = 0f;
				hitDirection.Normalize();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, hitDirection);
				if (m_fxImpactPrefab != null)
				{
					m_fxImpacts.Add(InstantiateFX(m_fxImpactPrefab, targetHitPosition, Quaternion.identity));
				}
				string text = m_impactAudioEvent;
				if (m_alternativeAudioIndex >= 0 && m_alternativeAudioIndex < m_alternativeImpactAudioEvents.Length)
				{
					text = m_alternativeImpactAudioEvents[m_alternativeAudioIndex];
				}
				if (!string.IsNullOrEmpty(text))
				{
					AudioManager.PostEvent(text, Targets[i].gameObject);
				}
				if (Targets[i] != null)
				{
					Source.OnSequenceHit(this, Targets[i], impulseInfo, lastHit ? ActorModelData.RagdollActivation.HealthBased : ActorModelData.RagdollActivation.None);
				}
			}
		}
		Source.OnSequenceHit(this, TargetPos);
	}

	private bool ImpactsFinished()
	{
		bool result = true;
		if (m_FXs.Count == 0)
		{
			result = false;
		}
		foreach (GameObject fx in m_FXs)
		{
			if (fx.activeSelf)
			{
				result = false;
				break;
			}
		}
		foreach (GameObject fxImpact in m_fxImpacts)
		{
			if (fxImpact.activeSelf)
			{
				result = false;
				break;
			}
		}
		if (m_delayedImpacts.Count > 0)
		{
			result = false;
		}
		return result;
	}

	private void Update()
	{
		if (m_initialized)
		{
			for (int i = m_delayedImpacts.Count - 1; i >= 0; i--)
			{
				SimpleAttachedVFXSequence.DelayedImpact delayedImpact = m_delayedImpacts[i];
				if (GameTime.time >= delayedImpact.m_timeToSpawnImpact)
				{
					SpawnImpactFX(delayedImpact.m_lastHit);
					m_delayedImpacts.RemoveAt(i);
				}
			}
			if (ImpactsFinished())
			{
				MarkForRemoval();
			}
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (parameter == m_startEventLeft)
		{
			SpawnFX(true);
		}
		else if (parameter == m_startEventRight)
		{
			SpawnFX(false);
		}
		else if (parameter == m_hitReactEvent)
		{
			if (m_hitImpactDelayTime > 0f)
			{
				m_delayedImpacts.Add(new SimpleAttachedVFXSequence.DelayedImpact(GameTime.time + m_hitImpactDelayTime, m_lastHitReactEvent == null));
			}
			else
			{
				SpawnImpactFX(m_lastHitReactEvent == null);
			}
		}
		else if (parameter == m_lastHitReactEvent)
		{
			if (m_hitImpactDelayTime > 0f)
			{
				m_delayedImpacts.Add(new SimpleAttachedVFXSequence.DelayedImpact(GameTime.time + m_hitImpactDelayTime, true));
			}
			else
			{
				SpawnImpactFX(true);
			}
		}
	}

	private void OnDisable()
	{
		if (m_FXs != null)
		{
			foreach (GameObject fx in m_FXs)
			{
				Destroy(fx.gameObject);
			}
			m_FXs = null;
		}
		if (m_fxImpacts != null)
		{
			foreach (GameObject fxImpact in m_fxImpacts)
			{
				Destroy(fxImpact.gameObject);
			}
			m_fxImpacts = null;
		}
		m_initialized = false;
	}
}
