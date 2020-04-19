using System;
using System.Collections.Generic;
using UnityEngine;

public class ExoSweepLaserSequence : Sequence
{
	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	[JointPopup("FX attach joint on the caster")]
	public JointPopupProperty m_fxCasterJoint;

	[Header("-- Impact Fx --")]
	public GameObject m_hitFxPrefab;

	[Header("-- Start and Stop Anim Events --")]
	[AnimEventPicker]
	public UnityEngine.Object m_startEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_stopEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_hitFailsafeEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_rotationStartEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_removePreviousLaserEvent;

	[Header("-- Audio Events --")]
	[AudioEvent(false)]
	public string m_audioEvent;

	[AudioEvent(false)]
	public string m_impactAudioEvent;

	[Space(10f)]
	public float m_maxProjectileDistInWorld = 8f;

	public Sequence.PhaseTimingParameters m_phaseTimingParameters;

	private GameObject m_fx;

	private List<GameObject> m_hitFxInstances = new List<GameObject>();

	private List<ActorData> m_actorsToHit = new List<ActorData>();

	private Vector3 m_lastLaserDir;

	private float m_actorRotationDuration = 1f;

	private bool m_actorRotationStarted;

	private static readonly int animTimeToRotationGoal = Animator.StringToHash("TimeToRotationGoal");

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			base.OverridePhaseTimingParams(this.m_phaseTimingParameters, extraSequenceParams);
			ExoSweepLaserSequence.ExtraParams extraParams2 = extraSequenceParams as ExoSweepLaserSequence.ExtraParams;
			if (extraParams2 != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ExoSweepLaserSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
				}
				if (extraParams2.lengthInSquares > 0f)
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
					this.m_maxProjectileDistInWorld = extraParams2.lengthInSquares * Board.\u000E().squareSize;
				}
				if (extraParams2.rotationDuration > 0f)
				{
					this.m_actorRotationDuration = extraParams2.rotationDuration;
				}
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
		if (base.Targets != null)
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
			for (int j = 0; j < base.Targets.Length; j++)
			{
				this.m_actorsToHit.Add(base.Targets[j]);
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
	}

	public override void FinishSetup()
	{
		if (this.m_startEvent == null && this.m_phaseTimingParameters.ShouldSequenceBeActive())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoSweepLaserSequence.FinishSetup()).MethodHandle;
			}
			this.SpawnFX();
		}
	}

	internal override void OnTurnStart(int currentTurn)
	{
		this.m_phaseTimingParameters.OnTurnStart(currentTurn);
	}

	internal override void OnAbilityPhaseStart(AbilityPriority abilityPhase)
	{
		this.m_phaseTimingParameters.OnAbilityPhaseStart(abilityPhase);
		if (this.m_startEvent == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoSweepLaserSequence.OnAbilityPhaseStart(AbilityPriority)).MethodHandle;
			}
			if (this.m_phaseTimingParameters.ShouldSpawnSequence(abilityPhase))
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
				this.SpawnFX();
			}
		}
		if (this.m_phaseTimingParameters.ShouldStopSequence(abilityPhase))
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
			if (this.m_fx != null)
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
				if (this.m_fx)
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
					this.m_fx.SetActive(false);
				}
			}
		}
	}

	private void Update()
	{
		if (this.m_initialized)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoSweepLaserSequence.Update()).MethodHandle;
			}
			if (this.m_phaseTimingParameters.ShouldSequenceBeActive())
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
				if (this.m_fx != null && base.Caster != null)
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
					FriendlyEnemyVFXSelector component = this.m_fx.GetComponent<FriendlyEnemyVFXSelector>();
					if (this.m_fx != null && component != null)
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
						component.Setup(base.Caster.\u000E());
					}
				}
				base.ProcessSequenceVisibility();
				if (this.m_fxCasterJoint.m_jointObject != null)
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
					Vector3 position = this.m_fxCasterJoint.m_jointObject.transform.position;
					Sequence.SetAttribute(this.m_fx, "startPoint", position);
					Vector3 forward = this.m_fxCasterJoint.m_jointObject.transform.forward;
					forward.y = 0f;
					forward.Normalize();
					float projectileDistance = this.GetProjectileDistance(position, forward, this.m_maxProjectileDistInWorld);
					Sequence.SetAttribute(this.m_fx, "endPoint", position + projectileDistance * forward);
					if (this.m_actorsToHit.Count > 0)
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
						Vector3 forward2 = this.m_fxCasterJoint.m_jointObject.transform.forward;
						float num = Vector3.Angle(this.m_lastLaserDir, forward2);
						Vector3 vec = this.m_lastLaserDir + forward2;
						float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(vec);
						float coneLengthRadiusInSquares = this.m_maxProjectileDistInWorld / Board.\u000E().squareSize + 2f;
						for (int i = this.m_actorsToHit.Count - 1; i >= 0; i--)
						{
							BoardSquare boardSquare = this.m_actorsToHit[i].\u0012();
							if (boardSquare != null)
							{
								bool flag = AreaEffectUtils.IsSquareInConeByActorRadius(boardSquare, base.Caster.\u0015(), coneCenterAngleDegrees, num + 1f, coneLengthRadiusInSquares, 0f, true, base.Caster, false, default(Vector3));
								if (flag)
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
									this.DoActorHit(this.m_actorsToHit[i]);
									this.m_actorsToHit.RemoveAt(i);
								}
							}
						}
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					this.m_lastLaserDir = this.m_fxCasterJoint.m_jointObject.transform.forward;
					if (this.m_actorRotationStarted)
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
						Animator animator = base.Caster.\u000E();
						animator.SetFloat(ExoSweepLaserSequence.animTimeToRotationGoal, base.Caster.\u0016());
					}
				}
			}
		}
	}

	private float GetProjectileDistance(Vector3 start, Vector3 forward, float maxDist)
	{
		Vector3 vector = start;
		vector.y = (float)Board.\u000E().BaselineHeight + BoardSquare.s_LoSHeightOffset;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(vector, forward, maxDist, false, base.Caster, null, true);
		return (vector - laserEndPoint).magnitude;
	}

	private void SpawnFX()
	{
		if (this.m_fx != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoSweepLaserSequence.SpawnFX()).MethodHandle;
			}
			return;
		}
		if (!this.m_fxCasterJoint.IsInitialized())
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
			GameObject referenceModel = base.GetReferenceModel(base.Caster, Sequence.ReferenceModelType.Actor);
			if (referenceModel != null)
			{
				this.m_fxCasterJoint.Initialize(referenceModel);
			}
		}
		if (this.m_fxPrefab != null)
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
			if (this.m_fxCasterJoint != null)
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
				if (this.m_fxCasterJoint.m_jointObject != null)
				{
					Vector3 position = this.m_fxCasterJoint.m_jointObject.transform.position;
					Quaternion rotation = default(Quaternion);
					this.m_fx = base.InstantiateFX(this.m_fxPrefab, position, rotation, true, true);
					this.m_lastLaserDir = this.m_fxCasterJoint.m_jointObject.transform.forward;
				}
				else
				{
					Log.Error(base.gameObject.name + " - m_fxCasterJoint.m_jointObject is NULL! Caster: {0} Target: {1}", new object[]
					{
						base.Caster.DisplayName,
						base.Target.DisplayName
					});
				}
			}
			else
			{
				Log.Error(base.gameObject.name + " - m_fxCasterJoint is NULL! Caster: {0} Target: {1}", new object[]
				{
					base.Caster.DisplayName,
					base.Target.DisplayName
				});
			}
		}
		if (!string.IsNullOrEmpty(this.m_audioEvent))
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
			AudioManager.PostEvent(this.m_audioEvent, base.Caster.gameObject);
		}
	}

	private void StopFX()
	{
		if (this.m_fx != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoSweepLaserSequence.StopFX()).MethodHandle;
			}
			this.m_fx.SetActive(false);
		}
		if (this.m_hitFxInstances != null)
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
			for (int i = 0; i < this.m_hitFxInstances.Count; i++)
			{
				if (this.m_hitFxInstances[i] != null)
				{
					this.m_hitFxInstances[i].SetActive(false);
				}
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

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_phaseTimingParameters.ShouldSequenceBeActive())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoSweepLaserSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			if (this.m_startEvent == parameter)
			{
				this.SpawnFX();
			}
			if (this.m_stopEvent == parameter)
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
				this.StopFX();
			}
			if (this.m_rotationStartEvent == parameter)
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
				if (!this.m_actorRotationStarted)
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
					base.Caster.TurnToPosition(base.TargetPos, this.m_actorRotationDuration);
					this.m_actorRotationStarted = true;
				}
			}
			if (this.m_removePreviousLaserEvent == parameter)
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
				base.Source.OnSequenceHit(this, base.Caster, null, ActorModelData.RagdollActivation.HealthBased, true);
			}
			if (this.m_hitFailsafeEvent == parameter)
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
				for (int i = 0; i < this.m_actorsToHit.Count; i++)
				{
					this.DoActorHit(this.m_actorsToHit[i]);
				}
				base.Source.OnSequenceHit(this, base.TargetPos, null);
			}
		}
	}

	private void DoActorHit(ActorData actor)
	{
		Vector3 position = actor.transform.position;
		Vector3 vector = position - base.Caster.transform.position;
		vector.y = 0f;
		vector.Normalize();
		ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, vector);
		base.Source.OnSequenceHit(this, actor, impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
		if (this.m_hitFxPrefab)
		{
			Quaternion rotation = Quaternion.LookRotation(vector);
			this.m_hitFxInstances.Add(base.InstantiateFX(this.m_hitFxPrefab, position, rotation, true, true));
		}
		if (!string.IsNullOrEmpty(this.m_impactAudioEvent))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoSweepLaserSequence.DoActorHit(ActorData)).MethodHandle;
			}
			AudioManager.PostEvent(this.m_impactAudioEvent, actor.gameObject);
		}
	}

	private void OnDisable()
	{
		if (this.m_fx != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoSweepLaserSequence.OnDisable()).MethodHandle;
			}
			UnityEngine.Object.Destroy(this.m_fx.gameObject);
			this.m_fx = null;
		}
		if (this.m_hitFxInstances != null)
		{
			for (int i = this.m_hitFxInstances.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = this.m_hitFxInstances[i];
				if (gameObject != null)
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
					UnityEngine.Object.Destroy(gameObject);
				}
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_hitFxInstances.Clear();
		}
	}

	public class ExtraParams : BlasterStretchConeSequence.ExtraParams
	{
		public float rotationDuration;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			base.XSP_SerializeToStream(stream);
			stream.Serialize(ref this.rotationDuration);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			base.XSP_DeserializeFromStream(stream);
			stream.Serialize(ref this.rotationDuration);
		}
	}
}
