using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SplineProjectileSequence : Sequence
{
	[Tooltip("Main FX prefab.")]
	[Separator("Projectile FX", true)]
	public GameObject m_fxPrefab;

	[Tooltip("FX at point(s) of impact")]
	[Separator("Impact FX (at end of projectile)", true)]
	public GameObject m_fxImpactPrefab;

	[Tooltip("Use different FX/sound at impact if no Target")]
	public bool m_useDifferentImpactIfNoTarget;

	public GameObject m_fxNoTargetImpactPrefab;

	[Tooltip("If spawning impact vfx, whether it's always visible")]
	public bool m_impactAlwaysVisibleIfSpawned;

	public bool m_spawnImpactAtFXDespawn;

	public bool m_skipImpactFxIfNoTargetActor;

	[Separator("Target Hit FX (as hits happen on targets)", true)]
	[Tooltip("Fx at targets hit")]
	public GameObject m_targetHitFxPrefab;

	public Sequence.HitVFXSpawnTeam m_hitFxTeamFilter = Sequence.HitVFXSpawnTeam.AllExcludeCaster;

	[JointPopup("Start position for projectile", order = 1)]
	[Space(10f, order = 0)]
	[Separator("Joints", true, order = 2)]
	public JointPopupProperty m_fxJoint;

	public Sequence.ReferenceModelType m_jointReferenceType;

	[JointPopup("FX attach joint for Target Hit Fx")]
	[Header("-- joint for hit location --")]
	public JointPopupProperty m_hitPosJoint;

	public bool m_targetHitFxAttachToJoint;

	[Separator("Anim Event: to spawn projectile", "orange", order = 2)]
	[AnimEventPicker(order = 3)]
	[Space(10f, order = 0)]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.", order = 1)]
	public UnityEngine.Object m_startEvent;

	[AudioEvent(false, order = 1)]
	[Separator("Audio Events", "orange", order = 2)]
	[Space(10f, order = 0)]
	public string m_audioEvent;

	[Header("    (( Audio Event on END of projectile ))")]
	[AudioEvent(false)]
	public string m_impactAudioEvent;

	[AudioEvent(false)]
	[Header("    (( only applicable if using different impact vfx if no target ))")]
	public string m_impactAudioEventIfNoTarget;

	[Header("    (( Audio Events on hitting target (useful for piercing projectiles) ))")]
	[AudioEvent(false)]
	public string m_targetHitAudioEvent;

	[Header("    (( Audio Events on hitting [ally] target (useful for piercing projectiles) ))")]
	[AudioEvent(false)]
	public string m_allyTargetHitAudioEvent;

	[Separator("Delay Timing", true)]
	public float m_hitReactDelay;

	public float m_startDelay;

	[Separator("Speed / Rotation", true)]
	public float m_projectileSpeed;

	public float m_projectileAcceleration;

	public float m_splineFractionUntilImpact = 1f;

	public bool m_alignToTravelDir = true;

	public bool m_keepAlignmentOnHorizontalPlane;

	public float m_projectileTravelHitWidth = -1f;

	private bool m_startEventHappened;

	private float m_impactDuration;

	private int m_eventNumberToKeyOffOf;

	private int m_numStartEventsReceived;

	protected bool m_doHitsAsProjectileTravels = true;

	private float m_hitReactTime = -1f;

	private bool m_hitReactPlayed;

	protected GameObject m_fx;

	private GameObject m_fxImpact;

	private List<GameObject> m_targetHitFx = new List<GameObject>();

	protected CRSpline m_spline;

	private float m_curSplineSpeed;

	private float m_splineSpeed;

	private float m_splineAcceleration;

	protected float m_splineTraveled;

	private float m_impactDurationLeft;

	private int m_curIndex;

	private int m_maxIndex;

	private bool m_skipImpactFx;

	private bool m_ignoreStartEvent;

	protected bool m_useOverrideStartPos;

	protected Vector3 m_overrideStartPos = Vector3.zero;

	protected bool m_markForRemovalAfterImpact = true;

	protected List<ActorData> m_actorsAlreadyHit = new List<ActorData>();

	protected Dictionary<string, float> m_projectileFxAttributes;

	protected Dictionary<string, float> m_impactFxAttributes;

	protected Vector3 m_fxSpawnPos;

	protected float m_totalTravelDist2D = 10f;

	internal abstract Vector3[] GetSplinePath(int curIndex, int maxIndex);

	protected int GetCurrentSplineSegment()
	{
		return this.m_spline.Section(this.m_splineTraveled);
	}

	public override void FinishSetup()
	{
		this.m_impactDuration = Sequence.GetFXDuration(this.GetImpactFxPrefab());
		if (this.m_startEvent == null || this.m_ignoreStartEvent)
		{
			this.ScheduleFX();
		}
	}

	protected virtual GameObject GetProjectileFxPrefab()
	{
		return this.m_fxPrefab;
	}

	protected virtual GameObject GetImpactFxPrefab()
	{
		if (this.m_useDifferentImpactIfNoTarget)
		{
			if (base.Targets != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SplineProjectileSequence.GetImpactFxPrefab()).MethodHandle;
				}
				if (base.Targets.Length != 0)
				{
					goto IL_42;
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			return this.m_fxNoTargetImpactPrefab;
		}
		IL_42:
		return this.m_fxImpactPrefab;
	}

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		if (base.Source.RemoveAtEndOfTurn)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SplineProjectileSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
			}
			if (GameWideData.Get() != null)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (GameWideData.Get().ShouldMakeCasterVisibleOnCast())
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_forceAlwaysVisible = true;
				}
			}
		}
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			if (extraSequenceParams is SplineProjectileSequence.DelayedProjectileExtraParams)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				SplineProjectileSequence.DelayedProjectileExtraParams delayedProjectileExtraParams = extraSequenceParams as SplineProjectileSequence.DelayedProjectileExtraParams;
				if (delayedProjectileExtraParams != null)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_curIndex = delayedProjectileExtraParams.curIndex;
					this.m_maxIndex = delayedProjectileExtraParams.maxIndex;
					if (delayedProjectileExtraParams.startDelay >= 0f)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_startDelay = delayedProjectileExtraParams.startDelay;
					}
					this.m_skipImpactFx = delayedProjectileExtraParams.skipImpactFx;
					this.m_useOverrideStartPos = delayedProjectileExtraParams.useOverrideStartPos;
					this.m_overrideStartPos = delayedProjectileExtraParams.overrideStartPos;
				}
			}
			else if (extraSequenceParams is SplineProjectileSequence.MultiEventExtraParams)
			{
				SplineProjectileSequence.MultiEventExtraParams multiEventExtraParams = extraSequenceParams as SplineProjectileSequence.MultiEventExtraParams;
				if (multiEventExtraParams != null)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_eventNumberToKeyOffOf = multiEventExtraParams.eventNumberToKeyOffOf;
				}
			}
			else if (extraSequenceParams is SplineProjectileSequence.ProjectilePropertyParams)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				SplineProjectileSequence.ProjectilePropertyParams projectilePropertyParams = extraSequenceParams as SplineProjectileSequence.ProjectilePropertyParams;
				if (projectilePropertyParams != null)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_projectileTravelHitWidth = projectilePropertyParams.projectileWidthInWorld;
				}
			}
			else if (extraSequenceParams is Sequence.FxAttributeParam)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				Sequence.FxAttributeParam fxAttributeParam = extraSequenceParams as Sequence.FxAttributeParam;
				if (fxAttributeParam != null && fxAttributeParam.m_paramNameCode != Sequence.FxAttributeParam.ParamNameCode.None)
				{
					string attributeName = fxAttributeParam.GetAttributeName();
					float paramValue = fxAttributeParam.m_paramValue;
					if (fxAttributeParam.m_paramTarget == Sequence.FxAttributeParam.ParamTarget.MainVfx)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.m_projectileFxAttributes == null)
						{
							this.m_projectileFxAttributes = new Dictionary<string, float>();
						}
						if (!this.m_projectileFxAttributes.ContainsKey(attributeName))
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							this.m_projectileFxAttributes.Add(attributeName, paramValue);
						}
					}
					else if (fxAttributeParam.m_paramTarget == Sequence.FxAttributeParam.ParamTarget.ImpactVfx)
					{
						if (this.m_impactFxAttributes == null)
						{
							this.m_impactFxAttributes = new Dictionary<string, float>();
						}
						if (!this.m_impactFxAttributes.ContainsKey(attributeName))
						{
							this.m_impactFxAttributes.Add(attributeName, paramValue);
						}
					}
				}
			}
			else if (extraSequenceParams is SimpleVFXAtTargetPosSequence.IgnoreStartEventExtraParam)
			{
				SimpleVFXAtTargetPosSequence.IgnoreStartEventExtraParam ignoreStartEventExtraParam = extraSequenceParams as SimpleVFXAtTargetPosSequence.IgnoreStartEventExtraParam;
				this.m_ignoreStartEvent = ignoreStartEventExtraParam.ignoreStartEvent;
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void OnDisable()
	{
		this.OnSequenceDisable();
	}

	protected virtual void OnSequenceDisable()
	{
		if (this.m_fx != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SplineProjectileSequence.OnSequenceDisable()).MethodHandle;
			}
			UnityEngine.Object.Destroy(this.m_fx);
			this.m_fx = null;
		}
		if (this.m_fxImpact != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			UnityEngine.Object.Destroy(this.m_fxImpact);
			this.m_fxImpact = null;
		}
		if (this.m_targetHitFx != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_targetHitFx.Count > 0)
			{
				for (int i = 0; i < this.m_targetHitFx.Count; i++)
				{
					UnityEngine.Object.Destroy(this.m_targetHitFx[i]);
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_targetHitFx.Clear();
			}
		}
		this.m_initialized = false;
	}

	protected virtual void SpawnImpactFX(Vector3 impactPos, Quaternion impactRot)
	{
		if (!this.m_skipImpactFx)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SplineProjectileSequence.SpawnImpactFX(Vector3, Quaternion)).MethodHandle;
			}
			if (this.m_skipImpactFxIfNoTargetActor)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (base.Targets != null)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (base.Targets.Length <= 0)
					{
						goto IL_305;
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			GameObject impactFxPrefab = this.GetImpactFxPrefab();
			if (impactFxPrefab)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_impactAlwaysVisibleIfSpawned)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_fxImpact = UnityEngine.Object.Instantiate<GameObject>(impactFxPrefab, impactPos, impactRot);
					this.m_fxImpact.transform.parent = base.gameObject.transform;
				}
				else
				{
					bool flag = false;
					if (base.Targets != null && base.Targets.Length > 0)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (base.Caster != null)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							for (int i = 0; i < base.Targets.Length; i++)
							{
								if (base.Caster.\u000E() != base.Targets[i].\u000E())
								{
									for (;;)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
									flag = true;
									goto IL_149;
								}
							}
							for (;;)
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
					IL_149:
					if (!flag)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!base.LastDesiredVisible())
						{
							goto IL_19D;
						}
					}
					this.m_fxImpact = base.InstantiateFX(impactFxPrefab, impactPos, impactRot, true, true);
					if (flag)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_fxImpact.transform.parent = base.gameObject.transform;
					}
				}
				IL_19D:
				this.m_impactDurationLeft = this.m_impactDuration;
				if (this.m_fxImpact != null)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_impactFxAttributes != null)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						foreach (KeyValuePair<string, float> keyValuePair in this.m_impactFxAttributes)
						{
							Sequence.SetAttribute(this.m_fxImpact, keyValuePair.Key, keyValuePair.Value);
						}
					}
				}
				if (this.m_fxImpact != null)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_fxImpact.GetComponent<FriendlyEnemyVFXSelector>() != null)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_fxImpact.GetComponent<FriendlyEnemyVFXSelector>().Setup(this.GetFoFObservingTeam());
					}
				}
			}
			if (this.m_useDifferentImpactIfNoTarget)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (base.Targets != null)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (base.Targets.Length != 0)
					{
						goto IL_2DF;
					}
				}
				if (!string.IsNullOrEmpty(this.m_impactAudioEventIfNoTarget))
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					AudioManager.PostEvent(this.m_impactAudioEventIfNoTarget, this.m_fx.gameObject);
				}
				goto IL_305;
			}
			IL_2DF:
			if (!string.IsNullOrEmpty(this.m_impactAudioEvent))
			{
				AudioManager.PostEvent(this.m_impactAudioEvent, this.m_fx.gameObject);
			}
		}
		IL_305:
		this.m_hitReactTime = GameTime.time + this.m_hitReactDelay;
	}

	protected virtual Team GetFoFObservingTeam()
	{
		return base.Caster.\u000E();
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SplineProjectileSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			if (this.m_eventNumberToKeyOffOf == this.m_numStartEventsReceived)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				this.ScheduleFX();
			}
			else
			{
				this.m_numStartEventsReceived++;
			}
		}
	}

	private void ScheduleFX()
	{
		this.m_startEventHappened = true;
	}

	protected virtual void SpawnFX()
	{
		Vector3[] splinePath = this.GetSplinePath(this.m_curIndex, this.m_maxIndex);
		this.m_spline = new CRSpline(splinePath);
		Vector3 a = this.m_spline.Interp(0.05f);
		Vector3 lookRotation = a - splinePath[1];
		if (lookRotation.magnitude > 1E-05f)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SplineProjectileSequence.SpawnFX()).MethodHandle;
			}
			lookRotation.Normalize();
		}
		else
		{
			lookRotation = Vector3.forward;
		}
		Quaternion rotation = default(Quaternion);
		if (this.m_alignToTravelDir)
		{
			if (this.m_keepAlignmentOnHorizontalPlane)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				lookRotation.y = 0f;
			}
			if (lookRotation.sqrMagnitude > 0f)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				rotation.SetLookRotation(lookRotation);
			}
		}
		if (this.m_projectileSpeed <= 0f)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_projectileSpeed = 10f;
		}
		float num = (splinePath[1] - splinePath[2]).magnitude + (splinePath[2] - splinePath[3]).magnitude;
		float num2 = num / this.m_projectileSpeed;
		if (num2 == 0f)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_splineSpeed = this.m_projectileSpeed;
		}
		else
		{
			this.m_splineSpeed = 1f / num2;
		}
		this.m_splineAcceleration = this.m_projectileAcceleration * this.m_splineSpeed / this.m_projectileSpeed;
		if (this.m_projectileAcceleration == 0f)
		{
			this.m_curSplineSpeed = this.m_splineSpeed;
		}
		else
		{
			this.m_curSplineSpeed = 0f;
		}
		this.m_fxSpawnPos = splinePath[1];
		this.m_totalTravelDist2D = VectorUtils.HorizontalPlaneDistInWorld(this.m_fxSpawnPos, splinePath[3]);
		this.m_fx = base.InstantiateFX(this.GetProjectileFxPrefab(), this.m_fxSpawnPos, rotation, true, false);
		if (this.m_fx != null && this.m_projectileFxAttributes != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			using (Dictionary<string, float>.Enumerator enumerator = this.m_projectileFxAttributes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, float> keyValuePair = enumerator.Current;
					Sequence.SetAttribute(this.m_fx, keyValuePair.Key, keyValuePair.Value);
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		if (!string.IsNullOrEmpty(this.m_audioEvent))
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			AudioManager.PostEvent(this.m_audioEvent, base.Caster.gameObject);
		}
	}

	protected virtual void SpawnTargetHitFx(ActorData target)
	{
		if (target != null && this.m_fx != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SplineProjectileSequence.SpawnTargetHitFx(ActorData)).MethodHandle;
			}
			if (base.IsHitFXVisibleWrtTeamFilter(target, this.m_hitFxTeamFilter))
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_targetHitFxPrefab != null)
				{
					GameObject gameObject = this.m_hitPosJoint.FindJointObject(target.gameObject);
					Vector3 vector;
					if (gameObject != null)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						vector = gameObject.transform.position;
					}
					else
					{
						vector = target.\u0016();
					}
					Vector3 position = vector;
					GameObject gameObject2 = base.InstantiateFX(this.m_targetHitFxPrefab, position, this.m_fx.transform.rotation, true, true);
					if (gameObject2 != null)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.m_targetHitFxAttachToJoint)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							base.AttachToBone(gameObject2, gameObject);
							gameObject2.transform.localPosition = Vector3.zero;
							gameObject2.transform.localScale = Vector3.one;
							gameObject2.transform.localRotation = Quaternion.identity;
						}
						else
						{
							gameObject2.transform.parent = base.transform;
						}
						this.m_targetHitFx.Add(gameObject2);
					}
				}
				if (target.\u000E() == base.Caster.\u000E())
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!string.IsNullOrEmpty(this.m_allyTargetHitAudioEvent))
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						AudioManager.PostEvent(this.m_allyTargetHitAudioEvent, target.gameObject);
						return;
					}
				}
				if (!string.IsNullOrEmpty(this.m_targetHitAudioEvent))
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					AudioManager.PostEvent(this.m_targetHitAudioEvent, target.gameObject);
				}
			}
		}
	}

	private void Update()
	{
		this.OnUpdate();
	}

	protected virtual void OnUpdate()
	{
		base.ProcessSequenceVisibility();
		if (this.m_initialized && this.m_startEventHappened)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SplineProjectileSequence.OnUpdate()).MethodHandle;
			}
			if (this.m_fx == null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_startDelay -= GameTime.deltaTime;
				if (this.m_startDelay <= 0f)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					GameObject referenceModel = base.GetReferenceModel(base.Caster, this.m_jointReferenceType);
					if (referenceModel != null)
					{
						this.m_fxJoint.Initialize(referenceModel);
					}
					this.SpawnFX();
				}
			}
			else
			{
				if (this.m_fx.activeSelf)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_fx != null)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.m_fx.GetComponent<FriendlyEnemyVFXSelector>() != null)
						{
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							this.m_fx.GetComponent<FriendlyEnemyVFXSelector>().Setup(this.GetFoFObservingTeam());
						}
					}
					this.m_curSplineSpeed += this.m_splineAcceleration;
					this.m_curSplineSpeed = Mathf.Min(this.m_splineSpeed, this.m_curSplineSpeed);
					this.m_splineTraveled += this.m_curSplineSpeed * GameTime.deltaTime;
					if (this.m_splineTraveled < this.m_splineFractionUntilImpact)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						Vector3 vector = this.m_spline.Interp(this.m_splineTraveled);
						if (this.m_alignToTravelDir)
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							if (GameTime.scale != 0f)
							{
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								Quaternion rotation = default(Quaternion);
								Vector3 vector2 = vector - this.m_fx.transform.position;
								if (this.m_keepAlignmentOnHorizontalPlane)
								{
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									vector2.y = 0f;
								}
								if (vector2.sqrMagnitude > 0f)
								{
									rotation.SetLookRotation(vector2.normalized);
								}
								this.m_fx.transform.rotation = rotation;
							}
						}
						if (this.m_doHitsAsProjectileTravels && base.Targets != null)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							Vector3 vector3 = vector - this.m_fx.transform.position;
							vector3.Normalize();
							float actorTargetingRadius = AreaEffectUtils.GetActorTargetingRadius();
							foreach (ActorData actorData in base.Targets)
							{
								if (actorData != null)
								{
									for (;;)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									if (!this.m_actorsAlreadyHit.Contains(actorData))
									{
										Vector3 rhs = actorData.transform.position - this.m_fx.transform.position;
										if (Vector3.Dot(vector3, rhs) < 0f)
										{
											for (;;)
											{
												switch (6)
												{
												case 0:
													continue;
												}
												break;
											}
											if (this.m_projectileTravelHitWidth > 0f)
											{
												for (;;)
												{
													switch (6)
													{
													case 0:
														continue;
													}
													break;
												}
												if (AreaEffectUtils.PointToLineDistance2D(actorData.transform.position, vector - vector3, vector + vector3) > this.m_projectileTravelHitWidth + actorTargetingRadius)
												{
													goto IL_3A7;
												}
											}
											Vector3 position = this.m_fx.transform.position;
											ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, vector3);
											base.Source.OnSequenceHit(this, actorData, impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
											this.m_actorsAlreadyHit.Add(actorData);
											this.SpawnTargetHitFx(actorData);
										}
									}
								}
								IL_3A7:;
							}
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						this.m_fx.transform.position = vector;
					}
					else
					{
						if (this.m_splineFractionUntilImpact > 0f)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							Vector3 position2 = this.m_spline.Interp(this.m_splineFractionUntilImpact);
							this.m_fx.transform.position = position2;
						}
						if (this.m_spawnImpactAtFXDespawn)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							this.SpawnImpactFX(this.m_fx.transform.position, this.m_fx.transform.rotation);
						}
						else
						{
							this.SpawnImpactFX(base.TargetPos, Quaternion.identity);
						}
						this.m_fx.SetActive(false);
						if (this.GetImpactFxPrefab() == null)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (this.m_markForRemovalAfterImpact)
							{
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								if (this.m_hitReactPlayed)
								{
									for (;;)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									base.MarkForRemoval();
								}
							}
						}
					}
				}
				if (GameTime.time > this.m_hitReactTime)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_hitReactTime > 0f && !this.m_hitReactPlayed)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_hitReactPlayed = true;
						Vector3 forward = this.m_fx.transform.forward;
						foreach (ActorData actorData2 in base.Targets)
						{
							if (actorData2 != null && !this.m_actorsAlreadyHit.Contains(actorData2))
							{
								Vector3 position3 = this.m_fx.transform.position;
								ActorModelData.ImpulseInfo impulseInfo2 = new ActorModelData.ImpulseInfo(position3, forward);
								base.Source.OnSequenceHit(this, actorData2, impulseInfo2, ActorModelData.RagdollActivation.HealthBased, true);
								this.m_actorsAlreadyHit.Add(actorData2);
								this.SpawnTargetHitFx(actorData2);
							}
						}
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						base.CallHitSequenceOnTargets(base.TargetPos, 1f, this.m_actorsAlreadyHit, !this.m_doHitsAsProjectileTravels);
					}
				}
				if (this.m_fxImpact != null)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_fxImpact.activeSelf)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.m_impactDurationLeft > 0f)
						{
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							this.m_impactDurationLeft -= GameTime.deltaTime;
						}
						else if (this.m_markForRemovalAfterImpact)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							if (this.m_hitReactPlayed)
							{
								for (;;)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								base.MarkForRemoval();
							}
						}
					}
				}
			}
		}
	}

	public class DelayedProjectileExtraParams : Sequence.IExtraSequenceParams
	{
		public float startDelay = -1f;

		public int curIndex;

		public int maxIndex;

		public bool skipImpactFx;

		public bool useOverrideStartPos;

		public Vector3 overrideStartPos = Vector3.zero;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			sbyte b = (sbyte)this.curIndex;
			sbyte b2 = (sbyte)this.maxIndex;
			stream.Serialize(ref this.startDelay);
			stream.Serialize(ref b);
			stream.Serialize(ref b2);
			stream.Serialize(ref this.skipImpactFx);
			stream.Serialize(ref this.useOverrideStartPos);
			if (this.useOverrideStartPos)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SplineProjectileSequence.DelayedProjectileExtraParams.XSP_SerializeToStream(IBitStream)).MethodHandle;
				}
				stream.Serialize(ref this.overrideStartPos);
			}
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			sbyte b = 0;
			sbyte b2 = 0;
			stream.Serialize(ref this.startDelay);
			stream.Serialize(ref b);
			stream.Serialize(ref b2);
			stream.Serialize(ref this.skipImpactFx);
			stream.Serialize(ref this.useOverrideStartPos);
			if (this.useOverrideStartPos)
			{
				stream.Serialize(ref this.overrideStartPos);
			}
			this.curIndex = (int)b;
			this.maxIndex = (int)b2;
		}
	}

	public class MultiEventExtraParams : Sequence.IExtraSequenceParams
	{
		public int eventNumberToKeyOffOf;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			short num = (short)this.eventNumberToKeyOffOf;
			stream.Serialize(ref num);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			short num = 0;
			stream.Serialize(ref num);
			this.eventNumberToKeyOffOf = (int)num;
		}
	}

	public class ProjectilePropertyParams : Sequence.IExtraSequenceParams
	{
		public float projectileWidthInWorld = -1f;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref this.projectileWidthInWorld);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref this.projectileWidthInWorld);
		}
	}
}
