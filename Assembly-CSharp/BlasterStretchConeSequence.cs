using System;
using System.Collections.Generic;
using UnityEngine;

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

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref angleInDegrees);
			stream.Serialize(ref lengthInSquares);
			stream.Serialize(ref forwardAngle);
			stream.Serialize(ref useStartPosOverride);
			if (!useStartPosOverride)
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
				stream.Serialize(ref startPosOverride);
				return;
			}
		}
	}

	[Separator("Muzzle flash / cone-covering FX prefab, scales by angle and length.", true)]
	public GameObject m_blastFxPrefab;

	[Separator("Projectile FX prefab; many get spawned.", true)]
	public GameObject m_projectileFxPrefab;

	[JointPopup("FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_fxJoint;

	[Separator("Impact FX", true)]
	public GameObject m_impactFxPrefab;

	public HitVFXSpawnTeam m_hitVfxSpawnTeamMode = HitVFXSpawnTeam.AllExcludeCaster;

	[JointPopup("hit FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_impactFxJoint;

	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[AnimEventPicker]
	[Separator("Anim Events -- ( main cone FX )", "orange")]
	public UnityEngine.Object m_startEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_stopEvent;

	[Tooltip("Aim the Fx Prefab in the target dir. If unchecked, inherits the attach joint transformation.")]
	public bool m_setBlastFxRotationToGamplayAim = true;

	[Tooltip("Sets the rotation of the impact FX to the direction of caster-to-target.")]
	public bool m_orientImpactToDirOfCasterToTarget = true;

	[Separator("Projectile Properties", true)]
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
	public UnityEngine.Object m_hitReactEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_lastHitReactEvent;

	[Separator("Default Distance and Angle (ability may override these)", true)]
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

	private const string c_angleControl = "angleControl";

	private const string c_lengthControl = "lengthControl";

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			ExtraParams extraParams2 = extraSequenceParams as ExtraParams;
			if (extraParams2 != null)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_maxDistInWorld = extraParams2.lengthInSquares * Board.Get().squareSize;
				m_angleRangeDegrees = extraParams2.angleInDegrees;
				m_aimForwardDir = VectorUtils.AngleDegreesToVector(extraParams2.forwardAngle);
				m_useStartPosOverride = extraParams2.useStartPosOverride;
				m_startPosOverride = extraParams2.startPosOverride;
				m_didSetValuesFromExtraParams = true;
			}
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public override void FinishSetup()
	{
		if (m_staggeredRateOfFire <= 0f)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_staggeredRateOfFire = 0.5f;
		}
		m_projectileFxInstances = new List<GameObject>();
		m_impactFxInstances = new List<GameObject>();
		m_projectileActorImpacts = new Dictionary<ActorData, float>();
		if (m_startEvent == null)
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
			SpawnFX();
		}
		if (!(m_hitReactEvent == null))
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
			if (m_lastHitReactEvent == null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					SpawnImpactFX(true, null);
					return;
				}
			}
			return;
		}
	}

	private float GetProjectileDistanceWithActorCollisions(Vector3 start, Vector3 forward, float maxDist, int projectileNum)
	{
		Vector3 vector = start;
		vector.y = (float)Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
		Vector3 startPos = vector;
		float laserRangeInSquares = maxDist / Board.Get().squareSize;
		ActorData caster = base.Caster;
		List<Team> opposingTeams = base.Caster.GetOpposingTeams();
		int maxTargets;
		if (m_projectilesStopOnEnemy)
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
			maxTargets = 1;
		}
		else
		{
			maxTargets = 4;
		}
		AreaEffectUtils.GetActorsInLaser(startPos, forward, laserRangeInSquares, 0.1f, caster, opposingTeams, false, maxTargets, false, true, out Vector3 laserEndPos, null);
		return (vector - laserEndPos).magnitude;
	}

	private float GetProjectileDistance(Vector3 start, Vector3 forward, float maxDist, int projectileNum)
	{
		Vector3 vector = start;
		vector.y = (float)Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(vector, forward, maxDist, false, base.Caster);
		float magnitude = (vector - laserEndPoint).magnitude;
		bool flag;
		if (m_projectileDistanceMode == ProjectileDistanceMode.AlwaysMaxDistance)
		{
			flag = false;
		}
		else if (m_projectileDistanceMode == ProjectileDistanceMode.AlwaysRandDistance)
		{
			flag = true;
		}
		else if (m_projectileDistanceMode == ProjectileDistanceMode.HalfMaxDist_HalfRandDist)
		{
			flag = (projectileNum % 2 == 1);
		}
		else if (m_projectileDistanceMode == ProjectileDistanceMode.RandomChoice_MaxVsRandDist)
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
			flag = (UnityEngine.Random.value >= 0.5f);
		}
		else
		{
			flag = false;
		}
		if (flag)
		{
			float a = UnityEngine.Random.Range(0f, maxDist);
			return Mathf.Min(a, magnitude);
		}
		return magnitude;
	}

	private Vector3 GetConeStartPos()
	{
		if (m_useStartPosOverride)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_startPosOverride;
				}
			}
		}
		if (m_fxJoint.IsInitialized())
		{
			return m_fxJoint.m_jointObject.transform.position;
		}
		return base.Caster.GetTravelBoardSquareWorldPosition();
	}

	private void SpawnFX()
	{
		if (!m_fxJoint.IsInitialized())
		{
			m_fxJoint.Initialize(base.Caster.gameObject);
		}
		Vector3 vector;
		if (m_setBlastFxRotationToGamplayAim)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_didSetValuesFromExtraParams)
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
				vector = m_aimForwardDir;
				goto IL_0085;
			}
		}
		vector = m_fxJoint.m_jointObject.transform.forward;
		vector.y = 0f;
		goto IL_0085;
		IL_0085:
		float value = m_angleRangeDegrees / 2f * ((float)Math.PI / 180f);
		if (m_blastFxPrefab != null)
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
			m_blastFxInstance = InstantiateFX(m_blastFxPrefab, GetConeStartPos(), Quaternion.LookRotation(vector));
			FriendlyEnemyVFXSelector component = m_blastFxInstance.GetComponent<FriendlyEnemyVFXSelector>();
			if (component != null)
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
				component.Setup(base.Caster.GetTeam());
				component.SetAttribute("angleControl", value);
				component.SetAttribute("lengthControl", m_maxDistInWorld / Board.Get().squareSize);
			}
			else
			{
				Sequence.SetAttribute(m_blastFxInstance, "angleControl", value);
				Sequence.SetAttribute(m_blastFxInstance, "lengthControl", m_maxDistInWorld / Board.Get().squareSize);
			}
		}
		if (m_staggerProjectiles)
		{
			m_projectileAngleOrder.Clear();
			float num = -0.5f * m_angleRangeDegrees;
			float num2;
			if (m_numProjectilesToSpawn < 2)
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
				num2 = 0f;
			}
			else
			{
				num2 = m_angleRangeDegrees / (float)(m_numProjectilesToSpawn - 1);
			}
			float num3 = num2;
			for (int i = 0; i < m_numProjectilesToSpawn; i++)
			{
				m_projectileAngleOrder.Add(num + num3 * (float)i);
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
			m_projectileAngleOrder.Shuffle(new System.Random());
			m_forwardDirForProjectileSpawns = vector;
			CreateProjectile(vector, 0, false);
			m_timeForNextStaggeredProjectile = GameTime.time + 1f / m_staggeredRateOfFire;
		}
		else
		{
			for (int j = 0; j < m_numProjectilesToSpawn; j++)
			{
				CreateProjectile(vector, j, true);
			}
		}
		if (m_projectilesCauseHitReacts)
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
			foreach (ActorData actorData in targets)
			{
				float num4 = (actorData.GetTravelBoardSquareWorldPositionForLos() - base.Caster.GetTravelBoardSquareWorldPositionForLos()).magnitude;
				if (GameWideData.Get().UseActorRadiusForCone())
				{
					num4 += GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize;
				}
				float num5 = GameTime.time + num4 / m_projectileSpeed;
				if (m_projectileActorImpacts.ContainsKey(actorData))
				{
					if (!(num5 < m_projectileActorImpacts[actorData]))
					{
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
				}
				m_projectileActorImpacts[actorData] = num5;
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
		if (string.IsNullOrEmpty(m_audioEvent))
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
			AudioManager.PostEvent(m_audioEvent, base.Caster.gameObject);
			return;
		}
	}

	private void CreateProjectile(Vector3 forward, int projectileIndex, bool randomAngle)
	{
		Quaternion rotation;
		if (randomAngle)
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
			rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0f - m_angleRangeDegrees, m_angleRangeDegrees), Vector3.up);
		}
		else
		{
			rotation = Quaternion.AngleAxis(m_projectileAngleOrder[projectileIndex], Vector3.up);
		}
		Vector3 vector = rotation * forward;
		if (!(m_projectileFxPrefab != null))
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
			Vector3 coneStartPos = GetConeStartPos();
			GameObject gameObject = InstantiateFX(m_projectileFxPrefab, coneStartPos, Quaternion.LookRotation(vector));
			float value;
			if (m_projectilesCauseHitReacts)
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
				Vector3 lhs = m_fxJoint.m_jointObject.transform.position - base.Caster.GetTravelBoardSquareWorldPositionForLos();
				lhs = Vector3.Dot(lhs, vector) * vector.normalized;
				value = GetProjectileDistanceWithActorCollisions(coneStartPos, vector, m_maxDistInWorld - lhs.magnitude, projectileIndex);
			}
			else
			{
				value = GetProjectileDistance(base.Caster.transform.position, vector, m_maxDistInWorld, projectileIndex);
			}
			Sequence.SetAttribute(gameObject, "projectileDistance", value);
			m_projectileFxInstances.Add(gameObject);
			return;
		}
	}

	private void SpawnImpactFX(bool lastHit, ActorData specificTarget)
	{
		if (base.Targets != null)
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
			for (int i = 0; i < base.Targets.Length; i++)
			{
				if (!(specificTarget == null))
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
					if (!(base.Targets[i] == specificTarget))
					{
						continue;
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
				Vector3 targetHitPosition = GetTargetHitPosition(i, m_impactFxJoint);
				Vector3 vector = targetHitPosition - base.Caster.transform.position;
				vector.y = 0f;
				vector.Normalize();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, vector);
				if (m_impactFxPrefab != null)
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
					if (IsHitFXVisibleWrtTeamFilter(base.Targets[i], m_hitVfxSpawnTeamMode))
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
						Quaternion rotation;
						if (m_setBlastFxRotationToGamplayAim)
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
							rotation = Quaternion.LookRotation(vector);
						}
						else
						{
							rotation = Quaternion.identity;
						}
						GameObject gameObject = InstantiateFX(m_impactFxPrefab, targetHitPosition, rotation);
						FriendlyEnemyVFXSelector component = gameObject.GetComponent<FriendlyEnemyVFXSelector>();
						if (component != null)
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
							component.Setup(base.Caster.GetTeam());
						}
						m_impactFxInstances.Add(gameObject);
					}
				}
				if (!string.IsNullOrEmpty(m_impactAudioEvent))
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
					AudioManager.PostEvent(m_impactAudioEvent, base.Targets[i].gameObject);
				}
				if (!(base.Targets[i] != null))
				{
					continue;
				}
				SequenceSource source = base.Source;
				ActorData target = base.Targets[i];
				int ragdollActivation;
				if (lastHit)
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
					ragdollActivation = 1;
				}
				else
				{
					ragdollActivation = 0;
				}
				source.OnSequenceHit(this, target, impulseInfo, (ActorModelData.RagdollActivation)ragdollActivation);
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
		base.Source.OnSequenceHit(this, base.TargetPos);
	}

	private void StopFX()
	{
		if (m_blastFxInstance != null)
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
			m_blastFxInstance.SetActive(false);
		}
		if (m_projectileFxInstances == null)
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
			for (int i = 0; i < m_projectileFxInstances.Count; i++)
			{
				if (m_projectileFxInstances[i] != null)
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
					m_projectileFxInstances[i].SetActive(false);
				}
			}
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
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
			if (m_projectileFxInstances.Count > 0)
			{
				using (Dictionary<ActorData, float>.Enumerator enumerator = m_projectileActorImpacts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<ActorData, float> current = enumerator.Current;
						if (current.Value >= GameTime.time)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									SpawnImpactFX(false, current.Key);
									m_projectileActorImpacts.Remove(current.Key);
									return;
								}
							}
						}
					}
					while (true)
					{
						switch (6)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
			}
			return;
		}
	}

	private bool ImpactsFinished()
	{
		bool result = true;
		using (List<GameObject>.Enumerator enumerator = m_impactFxInstances.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject current = enumerator.Current;
				if (current.activeSelf)
				{
					return false;
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (true)
					{
						return result;
					}
					/*OpCode not supported: LdMemberToken*/;
					return result;
				}
			}
		}
	}

	private void Update()
	{
		if (!m_initialized)
		{
			return;
		}
		if (m_staggerProjectiles)
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
			if (m_timeForNextStaggeredProjectile > 0f)
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
				if (m_projectileFxInstances.Count < m_numProjectilesToSpawn)
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
					if (GameTime.time >= m_timeForNextStaggeredProjectile)
					{
						CreateProjectile(m_forwardDirForProjectileSpawns, m_projectileFxInstances.Count, false);
						m_timeForNextStaggeredProjectile = GameTime.time + 1f / m_staggeredRateOfFire;
					}
				}
			}
		}
		CheckProjectileImpacts();
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (parameter == m_startEvent)
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
			SpawnFX();
		}
		if (parameter == m_hitReactEvent)
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
			SpawnImpactFX(m_lastHitReactEvent == null, null);
		}
		else if (parameter == m_lastHitReactEvent)
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
			SpawnImpactFX(true, null);
		}
		if (!(parameter == m_stopEvent))
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
			StopFX();
			return;
		}
	}

	private void OnDisable()
	{
		if (m_blastFxInstance != null)
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
			UnityEngine.Object.Destroy(m_blastFxInstance.gameObject);
			m_blastFxInstance = null;
		}
		if (m_projectileFxInstances != null)
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
			using (List<GameObject>.Enumerator enumerator = m_projectileFxInstances.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject current = enumerator.Current;
					UnityEngine.Object.Destroy(current.gameObject);
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			m_projectileFxInstances = null;
		}
		if (m_impactFxInstances != null)
		{
			using (List<GameObject>.Enumerator enumerator2 = m_impactFxInstances.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					GameObject current2 = enumerator2.Current;
					UnityEngine.Object.Destroy(current2.gameObject);
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
			m_impactFxInstances = null;
		}
		m_initialized = false;
	}
}
