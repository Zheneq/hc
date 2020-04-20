using System;
using System.Collections.Generic;
using UnityEngine;

public class BlasterStretchConeSequence : Sequence
{
	[Separator("Muzzle flash / cone-covering FX prefab, scales by angle and length.", true)]
	public GameObject m_blastFxPrefab;

	[Separator("Projectile FX prefab; many get spawned.", true)]
	public GameObject m_projectileFxPrefab;

	[JointPopup("FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_fxJoint;

	[Separator("Impact FX", true)]
	public GameObject m_impactFxPrefab;

	public Sequence.HitVFXSpawnTeam m_hitVfxSpawnTeamMode = Sequence.HitVFXSpawnTeam.AllExcludeCaster;

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

	public BlasterStretchConeSequence.ProjectileDistanceMode m_projectileDistanceMode = BlasterStretchConeSequence.ProjectileDistanceMode.RandomChoice_MaxVsRandDist;

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

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			BlasterStretchConeSequence.ExtraParams extraParams2 = extraSequenceParams as BlasterStretchConeSequence.ExtraParams;
			if (extraParams2 != null)
			{
				this.m_maxDistInWorld = extraParams2.lengthInSquares * Board.Get().squareSize;
				this.m_angleRangeDegrees = extraParams2.angleInDegrees;
				this.m_aimForwardDir = VectorUtils.AngleDegreesToVector(extraParams2.forwardAngle);
				this.m_useStartPosOverride = extraParams2.useStartPosOverride;
				this.m_startPosOverride = extraParams2.startPosOverride;
				this.m_didSetValuesFromExtraParams = true;
			}
		}
	}

	public override void FinishSetup()
	{
		if (this.m_staggeredRateOfFire <= 0f)
		{
			this.m_staggeredRateOfFire = 0.5f;
		}
		this.m_projectileFxInstances = new List<GameObject>();
		this.m_impactFxInstances = new List<GameObject>();
		this.m_projectileActorImpacts = new Dictionary<ActorData, float>();
		if (this.m_startEvent == null)
		{
			this.SpawnFX();
		}
		if (this.m_hitReactEvent == null)
		{
			if (this.m_lastHitReactEvent == null)
			{
				this.SpawnImpactFX(true, null);
			}
		}
	}

	private float GetProjectileDistanceWithActorCollisions(Vector3 start, Vector3 forward, float maxDist, int projectileNum)
	{
		Vector3 vector = start;
		vector.y = (float)Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
		Vector3 startPos = vector;
		float laserRangeInSquares = maxDist / Board.Get().squareSize;
		float laserWidthInSquares = 0.1f;
		ActorData caster = base.Caster;
		List<Team> opposingTeams = base.Caster.GetOpposingTeams();
		bool penetrateLos = false;
		int maxTargets;
		if (this.m_projectilesStopOnEnemy)
		{
			maxTargets = 1;
		}
		else
		{
			maxTargets = 4;
		}
		Vector3 b;
		AreaEffectUtils.GetActorsInLaser(startPos, forward, laserRangeInSquares, laserWidthInSquares, caster, opposingTeams, penetrateLos, maxTargets, false, true, out b, null, null, false, true);
		return (vector - b).magnitude;
	}

	private float GetProjectileDistance(Vector3 start, Vector3 forward, float maxDist, int projectileNum)
	{
		Vector3 vector = start;
		vector.y = (float)Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(vector, forward, maxDist, false, base.Caster, null, true);
		float magnitude = (vector - laserEndPoint).magnitude;
		bool flag;
		if (this.m_projectileDistanceMode == BlasterStretchConeSequence.ProjectileDistanceMode.AlwaysMaxDistance)
		{
			flag = false;
		}
		else if (this.m_projectileDistanceMode == BlasterStretchConeSequence.ProjectileDistanceMode.AlwaysRandDistance)
		{
			flag = true;
		}
		else if (this.m_projectileDistanceMode == BlasterStretchConeSequence.ProjectileDistanceMode.HalfMaxDist_HalfRandDist)
		{
			flag = (projectileNum % 2 == 1);
		}
		else if (this.m_projectileDistanceMode == BlasterStretchConeSequence.ProjectileDistanceMode.RandomChoice_MaxVsRandDist)
		{
			flag = (UnityEngine.Random.value >= 0.5f);
		}
		else
		{
			flag = false;
		}
		float result;
		if (flag)
		{
			float a = UnityEngine.Random.Range(0f, maxDist);
			result = Mathf.Min(a, magnitude);
		}
		else
		{
			result = magnitude;
		}
		return result;
	}

	private Vector3 GetConeStartPos()
	{
		if (this.m_useStartPosOverride)
		{
			return this.m_startPosOverride;
		}
		if (this.m_fxJoint.IsInitialized())
		{
			return this.m_fxJoint.m_jointObject.transform.position;
		}
		return base.Caster.GetTravelBoardSquareWorldPosition();
	}

	private void SpawnFX()
	{
		if (!this.m_fxJoint.IsInitialized())
		{
			this.m_fxJoint.Initialize(base.Caster.gameObject);
		}
		Vector3 vector;
		if (this.m_setBlastFxRotationToGamplayAim)
		{
			if (this.m_didSetValuesFromExtraParams)
			{
				vector = this.m_aimForwardDir;
				goto IL_85;
			}
		}
		vector = this.m_fxJoint.m_jointObject.transform.forward;
		vector.y = 0f;
		IL_85:
		float value = this.m_angleRangeDegrees / 2f * 0.0174532924f;
		if (this.m_blastFxPrefab != null)
		{
			this.m_blastFxInstance = base.InstantiateFX(this.m_blastFxPrefab, this.GetConeStartPos(), Quaternion.LookRotation(vector), true, true);
			FriendlyEnemyVFXSelector component = this.m_blastFxInstance.GetComponent<FriendlyEnemyVFXSelector>();
			if (component != null)
			{
				component.Setup(base.Caster.GetTeam());
				component.SetAttribute("angleControl", value);
				component.SetAttribute("lengthControl", this.m_maxDistInWorld / Board.Get().squareSize);
			}
			else
			{
				Sequence.SetAttribute(this.m_blastFxInstance, "angleControl", value);
				Sequence.SetAttribute(this.m_blastFxInstance, "lengthControl", this.m_maxDistInWorld / Board.Get().squareSize);
			}
		}
		if (this.m_staggerProjectiles)
		{
			this.m_projectileAngleOrder.Clear();
			float num = -0.5f * this.m_angleRangeDegrees;
			float num2;
			if (this.m_numProjectilesToSpawn < 2)
			{
				num2 = 0f;
			}
			else
			{
				num2 = this.m_angleRangeDegrees / (float)(this.m_numProjectilesToSpawn - 1);
			}
			float num3 = num2;
			for (int i = 0; i < this.m_numProjectilesToSpawn; i++)
			{
				this.m_projectileAngleOrder.Add(num + num3 * (float)i);
			}
			this.m_projectileAngleOrder.Shuffle(new System.Random());
			this.m_forwardDirForProjectileSpawns = vector;
			this.CreateProjectile(vector, 0, false);
			this.m_timeForNextStaggeredProjectile = GameTime.time + 1f / this.m_staggeredRateOfFire;
		}
		else
		{
			for (int j = 0; j < this.m_numProjectilesToSpawn; j++)
			{
				this.CreateProjectile(vector, j, true);
			}
		}
		if (this.m_projectilesCauseHitReacts)
		{
			ActorData[] targets = base.Targets;
			int k = 0;
			while (k < targets.Length)
			{
				ActorData actorData = targets[k];
				float num4 = (actorData.GetTravelBoardSquareWorldPositionForLos() - base.Caster.GetTravelBoardSquareWorldPositionForLos()).magnitude;
				if (GameWideData.Get().UseActorRadiusForCone())
				{
					num4 += GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize;
				}
				float num5 = GameTime.time + num4 / this.m_projectileSpeed;
				if (!this.m_projectileActorImpacts.ContainsKey(actorData))
				{
					goto IL_316;
				}
				if (num5 < this.m_projectileActorImpacts[actorData])
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						goto IL_316;
					}
				}
				IL_325:
				k++;
				continue;
				IL_316:
				this.m_projectileActorImpacts[actorData] = num5;
				goto IL_325;
			}
		}
		if (!string.IsNullOrEmpty(this.m_audioEvent))
		{
			AudioManager.PostEvent(this.m_audioEvent, base.Caster.gameObject);
		}
	}

	private void CreateProjectile(Vector3 forward, int projectileIndex, bool randomAngle)
	{
		Quaternion rotation;
		if (randomAngle)
		{
			rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(-this.m_angleRangeDegrees, this.m_angleRangeDegrees), Vector3.up);
		}
		else
		{
			rotation = Quaternion.AngleAxis(this.m_projectileAngleOrder[projectileIndex], Vector3.up);
		}
		Vector3 vector = rotation * forward;
		if (this.m_projectileFxPrefab != null)
		{
			Vector3 coneStartPos = this.GetConeStartPos();
			GameObject gameObject = base.InstantiateFX(this.m_projectileFxPrefab, coneStartPos, Quaternion.LookRotation(vector), true, true);
			float value;
			if (this.m_projectilesCauseHitReacts)
			{
				Vector3 lhs = this.m_fxJoint.m_jointObject.transform.position - base.Caster.GetTravelBoardSquareWorldPositionForLos();
				lhs = Vector3.Dot(lhs, vector) * vector.normalized;
				value = this.GetProjectileDistanceWithActorCollisions(coneStartPos, vector, this.m_maxDistInWorld - lhs.magnitude, projectileIndex);
			}
			else
			{
				value = this.GetProjectileDistance(base.Caster.transform.position, vector, this.m_maxDistInWorld, projectileIndex);
			}
			Sequence.SetAttribute(gameObject, "projectileDistance", value);
			this.m_projectileFxInstances.Add(gameObject);
		}
	}

	private void SpawnImpactFX(bool lastHit, ActorData specificTarget)
	{
		if (base.Targets != null)
		{
			int i = 0;
			while (i < base.Targets.Length)
			{
				if (specificTarget == null)
				{
					goto IL_5B;
				}
				if (base.Targets[i] == specificTarget)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						goto IL_5B;
					}
				}
				IL_1D0:
				i++;
				continue;
				IL_5B:
				Vector3 targetHitPosition = base.GetTargetHitPosition(i, this.m_impactFxJoint);
				Vector3 vector = targetHitPosition - base.Caster.transform.position;
				vector.y = 0f;
				vector.Normalize();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, vector);
				if (this.m_impactFxPrefab != null)
				{
					if (base.IsHitFXVisibleWrtTeamFilter(base.Targets[i], this.m_hitVfxSpawnTeamMode))
					{
						Quaternion rotation;
						if (this.m_setBlastFxRotationToGamplayAim)
						{
							rotation = Quaternion.LookRotation(vector);
						}
						else
						{
							rotation = Quaternion.identity;
						}
						GameObject gameObject = base.InstantiateFX(this.m_impactFxPrefab, targetHitPosition, rotation, true, true);
						FriendlyEnemyVFXSelector component = gameObject.GetComponent<FriendlyEnemyVFXSelector>();
						if (component != null)
						{
							component.Setup(base.Caster.GetTeam());
						}
						this.m_impactFxInstances.Add(gameObject);
					}
				}
				if (!string.IsNullOrEmpty(this.m_impactAudioEvent))
				{
					AudioManager.PostEvent(this.m_impactAudioEvent, base.Targets[i].gameObject);
				}
				if (base.Targets[i] != null)
				{
					SequenceSource source = base.Source;
					ActorData target = base.Targets[i];
					ActorModelData.ImpulseInfo impulseInfo2 = impulseInfo;
					ActorModelData.RagdollActivation ragdollActivation;
					if (lastHit)
					{
						ragdollActivation = ActorModelData.RagdollActivation.HealthBased;
					}
					else
					{
						ragdollActivation = ActorModelData.RagdollActivation.None;
					}
					source.OnSequenceHit(this, target, impulseInfo2, ragdollActivation, true);
					goto IL_1D0;
				}
				goto IL_1D0;
			}
		}
		base.Source.OnSequenceHit(this, base.TargetPos, null);
	}

	private void StopFX()
	{
		if (this.m_blastFxInstance != null)
		{
			this.m_blastFxInstance.SetActive(false);
		}
		if (this.m_projectileFxInstances != null)
		{
			for (int i = 0; i < this.m_projectileFxInstances.Count; i++)
			{
				if (this.m_projectileFxInstances[i] != null)
				{
					this.m_projectileFxInstances[i].SetActive(false);
				}
			}
		}
	}

	private void CheckProjectileImpacts()
	{
		if (this.m_projectilesCauseHitReacts)
		{
			if (this.m_projectileFxInstances.Count > 0)
			{
				using (Dictionary<ActorData, float>.Enumerator enumerator = this.m_projectileActorImpacts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<ActorData, float> keyValuePair = enumerator.Current;
						if (keyValuePair.Value >= GameTime.time)
						{
							this.SpawnImpactFX(false, keyValuePair.Key);
							this.m_projectileActorImpacts.Remove(keyValuePair.Key);
							return;
						}
					}
				}
			}
		}
	}

	private bool ImpactsFinished()
	{
		bool result = true;
		using (List<GameObject>.Enumerator enumerator = this.m_impactFxInstances.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject gameObject = enumerator.Current;
				if (gameObject.activeSelf)
				{
					return false;
				}
			}
		}
		return result;
	}

	private void Update()
	{
		if (this.m_initialized)
		{
			if (this.m_staggerProjectiles)
			{
				if (this.m_timeForNextStaggeredProjectile > 0f)
				{
					if (this.m_projectileFxInstances.Count < this.m_numProjectilesToSpawn)
					{
						if (GameTime.time >= this.m_timeForNextStaggeredProjectile)
						{
							this.CreateProjectile(this.m_forwardDirForProjectileSpawns, this.m_projectileFxInstances.Count, false);
							this.m_timeForNextStaggeredProjectile = GameTime.time + 1f / this.m_staggeredRateOfFire;
						}
					}
				}
			}
			this.CheckProjectileImpacts();
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (parameter == this.m_startEvent)
		{
			this.SpawnFX();
		}
		if (parameter == this.m_hitReactEvent)
		{
			this.SpawnImpactFX(this.m_lastHitReactEvent == null, null);
		}
		else if (parameter == this.m_lastHitReactEvent)
		{
			this.SpawnImpactFX(true, null);
		}
		if (parameter == this.m_stopEvent)
		{
			this.StopFX();
		}
	}

	private void OnDisable()
	{
		if (this.m_blastFxInstance != null)
		{
			UnityEngine.Object.Destroy(this.m_blastFxInstance.gameObject);
			this.m_blastFxInstance = null;
		}
		if (this.m_projectileFxInstances != null)
		{
			using (List<GameObject>.Enumerator enumerator = this.m_projectileFxInstances.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject = enumerator.Current;
					UnityEngine.Object.Destroy(gameObject.gameObject);
				}
			}
			this.m_projectileFxInstances = null;
		}
		if (this.m_impactFxInstances != null)
		{
			using (List<GameObject>.Enumerator enumerator2 = this.m_impactFxInstances.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					GameObject gameObject2 = enumerator2.Current;
					UnityEngine.Object.Destroy(gameObject2.gameObject);
				}
			}
			this.m_impactFxInstances = null;
		}
		this.m_initialized = false;
	}

	public enum ProjectileDistanceMode
	{
		AlwaysMaxDistance,
		AlwaysRandDistance,
		HalfMaxDist_HalfRandDist,
		RandomChoice_MaxVsRandDist
	}

	public class ExtraParams : Sequence.IExtraSequenceParams
	{
		public float angleInDegrees;

		public float lengthInSquares;

		public float forwardAngle;

		public bool useStartPosOverride;

		public Vector3 startPosOverride = Vector3.zero;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref this.angleInDegrees);
			stream.Serialize(ref this.lengthInSquares);
			stream.Serialize(ref this.forwardAngle);
			stream.Serialize(ref this.useStartPosOverride);
			if (this.useStartPosOverride)
			{
				stream.Serialize(ref this.startPosOverride);
			}
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref this.angleInDegrees);
			stream.Serialize(ref this.lengthInSquares);
			stream.Serialize(ref this.forwardAngle);
			stream.Serialize(ref this.useStartPosOverride);
			if (this.useStartPosOverride)
			{
				stream.Serialize(ref this.startPosOverride);
			}
		}
	}
}
