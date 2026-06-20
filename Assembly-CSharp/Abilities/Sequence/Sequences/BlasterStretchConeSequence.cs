// ROGUES
// SERVER

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class BlasterStretchConeSequence : Sequence
{
	public enum ProjectileDistanceMode
	{
		AlwaysMaxDistance,
		AlwaysRandDistance,
		HalfMaxDist_HalfRandDist,
		RandomChoice_MaxVsRandDist
	}

	public class ExtraParams : IExtraSequenceParams
	{
		public float angleInDegrees;
		public float lengthInSquares;
		public float forwardAngle;
		public bool useStartPosOverride;
		public Vector3 startPosOverride = Vector3.zero;

		// reactor
		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref angleInDegrees);
			stream.Serialize(ref lengthInSquares);
			stream.Serialize(ref forwardAngle);
			stream.Serialize(ref useStartPosOverride);
			if (useStartPosOverride)
			{
				stream.Serialize(ref startPosOverride);
			}
		}
		// rogues
		// public override void XSP_SerializeToStream(NetworkWriter writer)
		// {
		// 	writer.Write(angleInDegrees);
		// 	writer.Write(lengthInSquares);
		// 	writer.Write(forwardAngle);
		// 	writer.Write(useStartPosOverride);
		// 	if (useStartPosOverride)
		// 	{
		// 		writer.Write(startPosOverride);
		// 	}
		// }

		// reactor
		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref angleInDegrees);
			stream.Serialize(ref lengthInSquares);
			stream.Serialize(ref forwardAngle);
			stream.Serialize(ref useStartPosOverride);
			if (useStartPosOverride)
			{
				stream.Serialize(ref startPosOverride);
			}
		}
		// rogues
		// public override void XSP_DeserializeFromStream(NetworkReader reader)
		// {
		// 	angleInDegrees = reader.ReadSingle();
		// 	lengthInSquares = reader.ReadSingle();
		// 	forwardAngle = reader.ReadSingle();
		// 	useStartPosOverride = reader.ReadBoolean();
		// 	if (useStartPosOverride)
		// 	{
		// 		startPosOverride = reader.ReadVector3();
		// 	}
		// }
	}

	[Separator("Muzzle flash / cone-covering FX prefab, scales by angle and length.")]
	public GameObject m_blastFxPrefab;
	[Separator("Projectile FX prefab; many get spawned.")]
	public GameObject m_projectileFxPrefab;
	[JointPopup("FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_fxJoint;
	[Separator("Impact FX")]
	public GameObject m_impactFxPrefab;
	public HitVFXSpawnTeam m_hitVfxSpawnTeamMode = HitVFXSpawnTeam.AllExcludeCaster;
	[JointPopup("hit FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_impactFxJoint;
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[AnimEventPicker]
	[Separator("Anim Events -- ( main cone FX )", "orange")]
	public Object m_startEvent;
	[AnimEventPicker]
	public Object m_stopEvent;
	[Tooltip("Aim the Fx Prefab in the target dir. If unchecked, inherits the attach joint transformation.")]
	public bool m_setBlastFxRotationToGamplayAim = true;
	[Tooltip("Sets the rotation of the impact FX to the direction of caster-to-target.")]
	public bool m_orientImpactToDirOfCasterToTarget = true;
	[Separator("Projectile Properties")]
	public int m_numProjectilesToSpawn;
	public ProjectileDistanceMode m_projectileDistanceMode = ProjectileDistanceMode.RandomChoice_MaxVsRandDist;
	public bool m_projectilesCauseHitReacts;
	public bool m_projectilesStopOnEnemy;
	[Tooltip("This needs to match the particle velocity in the popcorn fx.")]
	public float m_projectileSpeed = 20f;
	public bool m_staggerProjectiles;
	[Tooltip("Projectiles per second")]
	public float m_staggeredRateOfFire = 6f;
	[Separator("Anim Event -- ( hit timing )", "orange")]
	[AnimEventPicker]
	public Object m_hitReactEvent;
	[AnimEventPicker]
	public Object m_lastHitReactEvent;
	[Separator("Default Distance and Angle (ability may override these)")]
	public float m_maxDistInWorld = 8f;
	public float m_angleRangeDegrees = 30f;
	[AudioEvent(false)]
	[Separator("Audio Events", "orange")]
	public string m_audioEvent;
	[AudioEvent(false)]
	public string m_impactAudioEvent;
	
	private Vector3 m_aimForwardDir = Vector3.zero;
	private GameObject m_blastFxInstance;
	private List<GameObject> m_projectileFxInstances;
	private List<GameObject> m_impactFxInstances;
	private bool m_didSetValuesFromExtraParams;
	private Vector3 m_forwardDirForProjectileSpawns = Vector3.zero;
	private bool m_useStartPosOverride;
	private Vector3 m_startPosOverride = Vector3.zero;
	private float m_timeForNextStaggeredProjectile = -1f;
	private List<float> m_projectileAngleOrder = new List<float>();
	private Dictionary<ActorData, float> m_projectileActorImpacts = new Dictionary<ActorData, float>();
	// removed in rogues
	private const string c_angleControl = "angleControl";
	// removed in rogues
	private const string c_lengthControl = "lengthControl";

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			if (extraSequenceParams is ExtraParams extraParam)
			{
				m_maxDistInWorld = extraParam.lengthInSquares * Board.Get().squareSize;
				m_angleRangeDegrees = extraParam.angleInDegrees;
				m_aimForwardDir = VectorUtils.AngleDegreesToVector(extraParam.forwardAngle);
				m_useStartPosOverride = extraParam.useStartPosOverride;
				m_startPosOverride = extraParam.startPosOverride;
				m_didSetValuesFromExtraParams = true;
			}
		}
	}

	public override void FinishSetup()
	{
		// removed in rogues
		if (m_staggeredRateOfFire <= 0f)
		{
			m_staggeredRateOfFire = 0.5f;
		}
		// end removed in rogues
		
		m_projectileFxInstances = new List<GameObject>();
		m_impactFxInstances = new List<GameObject>();
		m_projectileActorImpacts = new Dictionary<ActorData, float>();
		if (m_startEvent == null)
		{
			SpawnFX();
		}
		if (m_hitReactEvent == null && m_lastHitReactEvent == null)
		{
			SpawnImpactFX(true, null);
		}
	}

	private float GetProjectileDistanceWithActorCollisions(Vector3 start, Vector3 forward, float maxDist, int projectileNum)
	{
		Vector3 startPos = start;
		startPos.y = Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
		float laserRangeInSquares = maxDist / Board.Get().squareSize;
		int maxTargets = m_projectilesStopOnEnemy ? 1 : 4;
		AreaEffectUtils.GetActorsInLaser(
			startPos,
			forward,
			laserRangeInSquares,
			0.1f,
			Caster,
#if VANILLA
			Caster.GetEnemyTeamAsList(), // reactor
#else
			Caster.GetOtherTeams(), // rogues
#endif
			false,
			maxTargets,
			false,
			true,
			out Vector3 laserEndPos,
			null);
		return (startPos - laserEndPos).magnitude;
	}

	private float GetProjectileDistance(Vector3 start, Vector3 forward, float maxDist, int projectileNum)
	{
		Vector3 vector = start;
		vector.y = Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(vector, forward, maxDist, false, Caster);
		float magnitude = (vector - laserEndPoint).magnitude;
		bool random;
		switch (m_projectileDistanceMode)
		{
			case ProjectileDistanceMode.AlwaysMaxDistance:
				random = false;
				break;
			case ProjectileDistanceMode.AlwaysRandDistance:
				random = true;
				break;
			case ProjectileDistanceMode.HalfMaxDist_HalfRandDist:
				random = projectileNum % 2 == 1;
				break;
			case ProjectileDistanceMode.RandomChoice_MaxVsRandDist:
				random = Random.value >= 0.5f;
				break;
			default:
				random = false;
				break;
		}
		return random
			? Mathf.Min(Random.Range(0f, maxDist), magnitude)
			: magnitude;
	}

	private Vector3 GetConeStartPos()
	{
		if (m_useStartPosOverride)
		{
			return m_startPosOverride;
		}
		if (m_fxJoint.IsInitialized())
		{
			return m_fxJoint.m_jointObject.transform.position;
		}
		return Caster.GetFreePos();
	}

	private void SpawnFX()
	{
		if (!m_fxJoint.IsInitialized())
		{
			m_fxJoint.Initialize(Caster.gameObject);
		}
		Vector3 vector;
		if (m_setBlastFxRotationToGamplayAim && m_didSetValuesFromExtraParams)
		{
			vector = m_aimForwardDir;
		}
		else
		{
			vector = m_fxJoint.m_jointObject.transform.forward;
			vector.y = 0f;
		}
		float value = m_angleRangeDegrees / 2f * ((float)Math.PI / 180f);
		if (m_blastFxPrefab != null)
		{
			m_blastFxInstance = InstantiateFX(m_blastFxPrefab, GetConeStartPos(), Quaternion.LookRotation(vector));
			// reactor
			FriendlyEnemyVFXSelector component = m_blastFxInstance.GetComponent<FriendlyEnemyVFXSelector>();
			if (component != null)
			{
				component.Setup(Caster.GetTeam());
				component.SetAttribute("angleControl", value);
				component.SetAttribute("lengthControl", m_maxDistInWorld / Board.Get().squareSize);
			}
			else
			{
				SetAttribute(m_blastFxInstance, "angleControl", value);
				SetAttribute(m_blastFxInstance, "lengthControl", m_maxDistInWorld / Board.Get().squareSize);
			}
			// rogues
			// SetAttribute(m_blastFxInstance, "angleControl", value);
			// SetAttribute(m_blastFxInstance, "lengthControl", m_maxDistInWorld / Board.Get().squareSize);
		}
		if (m_staggerProjectiles)
		{
			m_projectileAngleOrder.Clear();
			float angleStart = -0.5f * m_angleRangeDegrees;
			// reactor
			float angleStep = m_numProjectilesToSpawn > 1
				? m_angleRangeDegrees / (m_numProjectilesToSpawn - 1)
				: 0f;
			// rogues
			// float angleStep = m_angleRangeDegrees / (m_numProjectilesToSpawn - 1);
			for (int i = 0; i < m_numProjectilesToSpawn; i++)
			{
				m_projectileAngleOrder.Add(angleStart + angleStep * i);
			}
			m_projectileAngleOrder.Shuffle(new System.Random());
			m_forwardDirForProjectileSpawns = vector;
			CreateProjectile(vector, 0, false);
			m_timeForNextStaggeredProjectile = GameTime.time + 1f / m_staggeredRateOfFire;
		}
		else
		{
			for (int i = 0; i < m_numProjectilesToSpawn; i++)
			{
				CreateProjectile(vector, i, true);
			}
		}
		if (m_projectilesCauseHitReacts)
		{
			foreach (ActorData actorData in Targets)
			{
				float distance = (actorData.GetLoSCheckPos() - Caster.GetLoSCheckPos()).magnitude;
				if (GameWideData.Get().UseActorRadiusForCone())
				{
					distance += GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize;
				}
				float endTime = GameTime.time + distance / m_projectileSpeed;
				if (!m_projectileActorImpacts.ContainsKey(actorData) || endTime < m_projectileActorImpacts[actorData])
				{
					m_projectileActorImpacts[actorData] = endTime;
				}
			}
		}
		if (!string.IsNullOrEmpty(m_audioEvent))
		{
			AudioManager.PostEvent(m_audioEvent, Caster.gameObject);
		}
	}

	private void CreateProjectile(Vector3 forward, int projectileIndex, bool randomAngle)
	{
		Quaternion rotation = Quaternion.AngleAxis(
			randomAngle
				? Random.Range(0f - m_angleRangeDegrees, m_angleRangeDegrees)
				: m_projectileAngleOrder[projectileIndex],
			Vector3.up);
		Vector3 vector = rotation * forward;
		if (m_projectileFxPrefab == null)
		{
			return;
		}
		Vector3 coneStartPos = GetConeStartPos();
		GameObject fxObject = InstantiateFX(m_projectileFxPrefab, coneStartPos, Quaternion.LookRotation(vector));
		float distance;
		if (m_projectilesCauseHitReacts)
		{
			Vector3 lhs = m_fxJoint.m_jointObject.transform.position - Caster.GetLoSCheckPos();
			lhs = Vector3.Dot(lhs, vector) * vector.normalized;
			distance = GetProjectileDistanceWithActorCollisions(coneStartPos, vector, m_maxDistInWorld - lhs.magnitude, projectileIndex);
		}
		else
		{
			distance = GetProjectileDistance(Caster.transform.position, vector, m_maxDistInWorld, projectileIndex);
		}
		SetAttribute(fxObject, "projectileDistance", distance);
		m_projectileFxInstances.Add(fxObject);
	}

	private void SpawnImpactFX(bool lastHit, ActorData specificTarget)
	{
		if (Targets != null)
		{
			for (int i = 0; i < Targets.Length; i++)
			{
				if (specificTarget == null || Targets[i] == specificTarget)
				{
					Vector3 targetHitPosition = GetTargetHitPosition(i, m_impactFxJoint);
					Vector3 vector = targetHitPosition - Caster.transform.position;
					vector.y = 0f;
					vector.Normalize();
					ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, vector);
					if (m_impactFxPrefab != null && IsHitFXVisibleWrtTeamFilter(Targets[i], m_hitVfxSpawnTeamMode))
					{
						Quaternion rotation = m_setBlastFxRotationToGamplayAim
							? Quaternion.LookRotation(vector)
							: Quaternion.identity;
						GameObject fxObject = InstantiateFX(m_impactFxPrefab, targetHitPosition, rotation);
						
						// removed in rogues
						FriendlyEnemyVFXSelector component = fxObject.GetComponent<FriendlyEnemyVFXSelector>();
						if (component != null)
						{
							component.Setup(Caster.GetTeam());
						}
						// end removed in rogues

						m_impactFxInstances.Add(fxObject);
					}

					if (!string.IsNullOrEmpty(m_impactAudioEvent))
					{
						AudioManager.PostEvent(m_impactAudioEvent, Targets[i].gameObject);
					}
					if (Targets[i] != null)
					{
						SequenceSource source = Source;
						ActorData target = Targets[i];
						ActorModelData.RagdollActivation ragdollActivation = lastHit
							? ActorModelData.RagdollActivation.HealthBased
							: ActorModelData.RagdollActivation.None;
						source.OnSequenceHit(this, target, impulseInfo, ragdollActivation);
					}
				}
			}
		}
		Source.OnSequenceHit(this, TargetPos);
	}

	private void StopFX()
	{
		if (m_blastFxInstance != null)
		{
			m_blastFxInstance.SetActive(false);
		}
		if (m_projectileFxInstances != null)
		{
			foreach (GameObject fx in m_projectileFxInstances)
			{
				if (fx != null)
				{
					fx.SetActive(false);
				}
			}
		}
	}

	private void CheckProjectileImpacts()
	{
		if (!m_projectilesCauseHitReacts)
		{
			return;
		}
		if (m_projectileFxInstances.Count > 0)
		{
			foreach (KeyValuePair<ActorData, float> current in m_projectileActorImpacts)
			{
				if (current.Value >= GameTime.time)
				{
					SpawnImpactFX(false, current.Key);
					m_projectileActorImpacts.Remove(current.Key);
					return;
				}
			}
		}
	}

	private bool ImpactsFinished()
	{
		foreach (GameObject current in m_impactFxInstances)
		{
			if (current.activeSelf)
			{
				return false;
			}
		}
		return true;
	}

	private void Update()
	{
		if (!m_initialized)
		{
			return;
		}
		if (m_staggerProjectiles
		    && m_timeForNextStaggeredProjectile > 0f
		    && m_projectileFxInstances.Count < m_numProjectilesToSpawn
		    && GameTime.time >= m_timeForNextStaggeredProjectile)
		{
			CreateProjectile(m_forwardDirForProjectileSpawns, m_projectileFxInstances.Count, false);
			m_timeForNextStaggeredProjectile = GameTime.time + 1f / m_staggeredRateOfFire;
		}
		CheckProjectileImpacts();
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (parameter == m_startEvent)
		{
			SpawnFX();
		}
		if (parameter == m_hitReactEvent)
		{
			SpawnImpactFX(m_lastHitReactEvent == null, null);
		}
		else if (parameter == m_lastHitReactEvent)
		{
			SpawnImpactFX(true, null);
		}
		if (parameter == m_stopEvent)
		{
			StopFX();
		}
	}

	private void OnDisable()
	{
		if (m_blastFxInstance != null)
		{
			Destroy(m_blastFxInstance.gameObject);
			m_blastFxInstance = null;
		}
		if (m_projectileFxInstances != null)
		{
			foreach (GameObject fx in m_projectileFxInstances)
			{
				Destroy(fx.gameObject);
			}
			m_projectileFxInstances = null;
		}
		if (m_impactFxInstances != null)
		{
			foreach (GameObject fx in m_impactFxInstances)
			{
				Destroy(fx.gameObject);
			}
			m_impactFxInstances = null;
		}
		m_initialized = false;
	}
}
