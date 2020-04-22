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
					m_maxDistInSquares = coneExtraParams.maxDistInSquares;
				}
				if (coneExtraParams.halfAngleDegrees > 0f)
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
					m_angleRange = coneExtraParams.halfAngleDegrees;
				}
			}
			SimpleAttachedVFXSequence.ImpactDelayParams impactDelayParams = extraSequenceParams as SimpleAttachedVFXSequence.ImpactDelayParams;
			if (impactDelayParams == null)
			{
				continue;
			}
			if (impactDelayParams.impactDelayTime > 0f)
			{
				m_hitImpactDelayTime = impactDelayParams.impactDelayTime;
			}
			if (impactDelayParams.alternativeImpactAudioIndex >= 0)
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
				m_alternativeAudioIndex = impactDelayParams.alternativeImpactAudioIndex;
			}
		}
	}

	public override void FinishSetup()
	{
		m_FXs = new List<GameObject>();
		m_fxImpacts = new List<GameObject>();
		if (m_projectilesPerAnimEvent <= 20)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_projectilesPerAnimEvent >= 1)
			{
				goto IL_004d;
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
		}
		m_projectilesPerAnimEvent = 1;
		goto IL_004d;
		IL_004d:
		if (m_projectilesPerAnimEvent > 1)
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
			int num = m_expectedNumProjectilesForEdgeShots;
			if (num <= 0)
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
				num = 4;
			}
			m_spawnIndexEdgeMin = Random.Range(0, num);
			m_spawnIndexEdgeMax = Random.Range(0, num);
			if (m_spawnIndexEdgeMax == m_spawnIndexEdgeMin)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					m_spawnIndexEdgeMax = (m_spawnIndexEdgeMax + 1) % num;
					return;
				}
			}
			return;
		}
	}

	private float GetProjectileDistance(Vector3 start, Vector3 forward, float maxDist)
	{
		Vector3 vector = start;
		if (m_raycastDistanceFromLosHeight)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			vector.y = (float)Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
		}
		else
		{
			vector.y += 0.5f;
		}
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(vector, forward, maxDist, false, base.Caster);
		return (vector - laserEndPoint).magnitude;
	}

	private void SpawnFX(bool left)
	{
		JointPopupProperty jointPopupProperty = (!left) ? m_fxJointRight : m_fxJointLeft;
		if (!jointPopupProperty.IsInitialized())
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
			jointPopupProperty.Initialize(base.Caster.gameObject);
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
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				num = Random.Range(0, projectilesPerAnimEvent);
				num2 = Random.Range(0, projectilesPerAnimEvent);
				if (num2 == num)
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
					num2 = (num2 + 1) % projectilesPerAnimEvent;
				}
			}
			else if (projectilesPerAnimEvent == 1)
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
				if (m_numSpawnAttempts == m_spawnIndexEdgeMin)
				{
					num = m_spawnIndexEdgeMin;
				}
				else if (m_numSpawnAttempts == m_spawnIndexEdgeMax)
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
					num2 = m_spawnIndexEdgeMax;
				}
			}
		}
		for (int i = 0; i < projectilesPerAnimEvent; i++)
		{
			float angle = Random.Range(0f - m_angleRange, m_angleRange);
			if (i == num)
			{
				angle = 0f - m_angleRange;
			}
			else if (i == num2)
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
				angle = m_angleRange;
			}
			GameObject item = CreateProjectileFx(forward, angle, jointPopupProperty);
			m_FXs.Add(item);
			if (!(Random.Range(0f, 1f) < m_extraEdgeShotChance))
			{
				continue;
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
			float angle2 = 0f - m_angleRange;
			if (Random.Range(0f, 1f) < 0.5f)
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
				angle2 = m_angleRange;
			}
			GameObject item2 = CreateProjectileFx(forward, angle2, jointPopupProperty);
			m_FXs.Add(item2);
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (!string.IsNullOrEmpty(m_audioEvent))
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
				AudioManager.PostEvent(m_audioEvent, base.Caster.gameObject);
			}
			if (m_hitReactEvent == null)
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
				if (m_lastHitReactEvent == null)
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
			m_numSpawnAttempts++;
			return;
		}
	}

	private GameObject CreateProjectileFx(Vector3 forward, float angle, JointPopupProperty fxJoint)
	{
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
		Vector3 vector = rotation * forward;
		GameObject gameObject = InstantiateFX(m_fxPrefab, fxJoint.m_jointObject.transform.position, Quaternion.LookRotation(vector));
		float projectileDistance = GetProjectileDistance(base.Caster.transform.position, vector, m_maxDistInSquares * 1.5f);
		Sequence.SetAttribute(gameObject, "projectileDistance", projectileDistance);
		Debug.DrawRay(fxJoint.m_jointObject.transform.position, projectileDistance * vector, Color.red, 5f);
		return gameObject;
	}

	private void SpawnImpactFX(bool lastHit)
	{
		if (base.Targets != null)
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
			for (int i = 0; i < base.Targets.Length; i++)
			{
				Vector3 targetHitPosition = GetTargetHitPosition(i, m_hitFxJoint);
				Vector3 hitDirection = targetHitPosition - base.Caster.transform.position;
				hitDirection.y = 0f;
				hitDirection.Normalize();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, hitDirection);
				if ((bool)m_fxImpactPrefab)
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
					m_fxImpacts.Add(InstantiateFX(m_fxImpactPrefab, targetHitPosition, Quaternion.identity));
				}
				string text = m_impactAudioEvent;
				if (m_alternativeAudioIndex >= 0)
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
					if (m_alternativeAudioIndex < m_alternativeImpactAudioEvents.Length)
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
						text = m_alternativeImpactAudioEvents[m_alternativeAudioIndex];
					}
				}
				if (!string.IsNullOrEmpty(text))
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
					AudioManager.PostEvent(text, base.Targets[i].gameObject);
				}
				if (base.Targets[i] != null)
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
					base.Source.OnSequenceHit(this, base.Targets[i], impulseInfo, lastHit ? ActorModelData.RagdollActivation.HealthBased : ActorModelData.RagdollActivation.None);
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		base.Source.OnSequenceHit(this, base.TargetPos);
	}

	private bool ImpactsFinished()
	{
		bool result = true;
		if (m_FXs.Count == 0)
		{
			result = false;
		}
		using (List<GameObject>.Enumerator enumerator = m_FXs.GetEnumerator())
		{
			while (true)
			{
				if (!enumerator.MoveNext())
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							goto end_IL_0021;
						}
					}
				}
				GameObject current = enumerator.Current;
				if (current.activeSelf)
				{
					result = false;
					break;
				}
			}
			end_IL_0021:;
		}
		using (List<GameObject>.Enumerator enumerator2 = m_fxImpacts.GetEnumerator())
		{
			while (true)
			{
				if (!enumerator2.MoveNext())
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
					break;
				}
				GameObject current2 = enumerator2.Current;
				if (current2.activeSelf)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							result = false;
							goto end_IL_0076;
						}
					}
				}
			}
			end_IL_0076:;
		}
		if (m_delayedImpacts.Count > 0)
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
			result = false;
		}
		return result;
	}

	private void Update()
	{
		if (!m_initialized)
		{
			return;
		}
		for (int num = m_delayedImpacts.Count - 1; num >= 0; num--)
		{
			SimpleAttachedVFXSequence.DelayedImpact delayedImpact = m_delayedImpacts[num];
			if (GameTime.time >= delayedImpact.m_timeToSpawnImpact)
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
				SpawnImpactFX(delayedImpact.m_lastHit);
				m_delayedImpacts.RemoveAt(num);
			}
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (ImpactsFinished())
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					MarkForRemoval();
					return;
				}
			}
			return;
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (parameter == m_startEventLeft)
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
					SpawnFX(true);
					return;
				}
			}
		}
		if (parameter == m_startEventRight)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					SpawnFX(false);
					return;
				}
			}
		}
		if (parameter == m_hitReactEvent)
		{
			if (m_hitImpactDelayTime > 0f)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						m_delayedImpacts.Add(new SimpleAttachedVFXSequence.DelayedImpact(GameTime.time + m_hitImpactDelayTime, m_lastHitReactEvent == null));
						return;
					}
				}
			}
			SpawnImpactFX(m_lastHitReactEvent == null);
		}
		else
		{
			if (!(parameter == m_lastHitReactEvent))
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
				if (m_hitImpactDelayTime > 0f)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							m_delayedImpacts.Add(new SimpleAttachedVFXSequence.DelayedImpact(GameTime.time + m_hitImpactDelayTime, true));
							return;
						}
					}
				}
				SpawnImpactFX(true);
				return;
			}
		}
	}

	private void OnDisable()
	{
		if (m_FXs != null)
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
			using (List<GameObject>.Enumerator enumerator = m_FXs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject current = enumerator.Current;
					Object.Destroy(current.gameObject);
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
			}
			m_FXs = null;
		}
		if (m_fxImpacts != null)
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
			using (List<GameObject>.Enumerator enumerator2 = m_fxImpacts.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					GameObject current2 = enumerator2.Current;
					Object.Destroy(current2.gameObject);
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
			m_fxImpacts = null;
		}
		m_initialized = false;
	}
}
