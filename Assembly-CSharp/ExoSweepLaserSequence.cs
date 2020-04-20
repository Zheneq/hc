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
				if (extraParams2.lengthInSquares > 0f)
				{
					this.m_maxProjectileDistInWorld = extraParams2.lengthInSquares * Board.Get().squareSize;
				}
				if (extraParams2.rotationDuration > 0f)
				{
					this.m_actorRotationDuration = extraParams2.rotationDuration;
				}
			}
		}
		if (base.Targets != null)
		{
			for (int j = 0; j < base.Targets.Length; j++)
			{
				this.m_actorsToHit.Add(base.Targets[j]);
			}
		}
	}

	public override void FinishSetup()
	{
		if (this.m_startEvent == null && this.m_phaseTimingParameters.ShouldSequenceBeActive())
		{
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
			if (this.m_phaseTimingParameters.ShouldSpawnSequence(abilityPhase))
			{
				this.SpawnFX();
			}
		}
		if (this.m_phaseTimingParameters.ShouldStopSequence(abilityPhase))
		{
			if (this.m_fx != null)
			{
				if (this.m_fx)
				{
					this.m_fx.SetActive(false);
				}
			}
		}
	}

	private void Update()
	{
		if (this.m_initialized)
		{
			if (this.m_phaseTimingParameters.ShouldSequenceBeActive())
			{
				if (this.m_fx != null && base.Caster != null)
				{
					FriendlyEnemyVFXSelector component = this.m_fx.GetComponent<FriendlyEnemyVFXSelector>();
					if (this.m_fx != null && component != null)
					{
						component.Setup(base.Caster.GetTeam());
					}
				}
				base.ProcessSequenceVisibility();
				if (this.m_fxCasterJoint.m_jointObject != null)
				{
					Vector3 position = this.m_fxCasterJoint.m_jointObject.transform.position;
					Sequence.SetAttribute(this.m_fx, "startPoint", position);
					Vector3 forward = this.m_fxCasterJoint.m_jointObject.transform.forward;
					forward.y = 0f;
					forward.Normalize();
					float projectileDistance = this.GetProjectileDistance(position, forward, this.m_maxProjectileDistInWorld);
					Sequence.SetAttribute(this.m_fx, "endPoint", position + projectileDistance * forward);
					if (this.m_actorsToHit.Count > 0)
					{
						Vector3 forward2 = this.m_fxCasterJoint.m_jointObject.transform.forward;
						float num = Vector3.Angle(this.m_lastLaserDir, forward2);
						Vector3 vec = this.m_lastLaserDir + forward2;
						float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(vec);
						float coneLengthRadiusInSquares = this.m_maxProjectileDistInWorld / Board.Get().squareSize + 2f;
						for (int i = this.m_actorsToHit.Count - 1; i >= 0; i--)
						{
							BoardSquare currentBoardSquare = this.m_actorsToHit[i].GetCurrentBoardSquare();
							if (currentBoardSquare != null)
							{
								bool flag = AreaEffectUtils.IsSquareInConeByActorRadius(currentBoardSquare, base.Caster.GetTravelBoardSquareWorldPositionForLos(), coneCenterAngleDegrees, num + 1f, coneLengthRadiusInSquares, 0f, true, base.Caster, false, default(Vector3));
								if (flag)
								{
									this.DoActorHit(this.m_actorsToHit[i]);
									this.m_actorsToHit.RemoveAt(i);
								}
							}
						}
					}
					this.m_lastLaserDir = this.m_fxCasterJoint.m_jointObject.transform.forward;
					if (this.m_actorRotationStarted)
					{
						Animator modelAnimator = base.Caster.GetModelAnimator();
						modelAnimator.SetFloat(ExoSweepLaserSequence.animTimeToRotationGoal, base.Caster.GetRotationTimeRemaining());
					}
				}
			}
		}
	}

	private float GetProjectileDistance(Vector3 start, Vector3 forward, float maxDist)
	{
		Vector3 vector = start;
		vector.y = (float)Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(vector, forward, maxDist, false, base.Caster, null, true);
		return (vector - laserEndPoint).magnitude;
	}

	private void SpawnFX()
	{
		if (this.m_fx != null)
		{
			return;
		}
		if (!this.m_fxCasterJoint.IsInitialized())
		{
			GameObject referenceModel = base.GetReferenceModel(base.Caster, Sequence.ReferenceModelType.Actor);
			if (referenceModel != null)
			{
				this.m_fxCasterJoint.Initialize(referenceModel);
			}
		}
		if (this.m_fxPrefab != null)
		{
			if (this.m_fxCasterJoint != null)
			{
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
			AudioManager.PostEvent(this.m_audioEvent, base.Caster.gameObject);
		}
	}

	private void StopFX()
	{
		if (this.m_fx != null)
		{
			this.m_fx.SetActive(false);
		}
		if (this.m_hitFxInstances != null)
		{
			for (int i = 0; i < this.m_hitFxInstances.Count; i++)
			{
				if (this.m_hitFxInstances[i] != null)
				{
					this.m_hitFxInstances[i].SetActive(false);
				}
			}
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_phaseTimingParameters.ShouldSequenceBeActive())
		{
			if (this.m_startEvent == parameter)
			{
				this.SpawnFX();
			}
			if (this.m_stopEvent == parameter)
			{
				this.StopFX();
			}
			if (this.m_rotationStartEvent == parameter)
			{
				if (!this.m_actorRotationStarted)
				{
					base.Caster.TurnToPosition(base.TargetPos, this.m_actorRotationDuration);
					this.m_actorRotationStarted = true;
				}
			}
			if (this.m_removePreviousLaserEvent == parameter)
			{
				base.Source.OnSequenceHit(this, base.Caster, null, ActorModelData.RagdollActivation.HealthBased, true);
			}
			if (this.m_hitFailsafeEvent == parameter)
			{
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
			AudioManager.PostEvent(this.m_impactAudioEvent, actor.gameObject);
		}
	}

	private void OnDisable()
	{
		if (this.m_fx != null)
		{
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
					UnityEngine.Object.Destroy(gameObject);
				}
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
