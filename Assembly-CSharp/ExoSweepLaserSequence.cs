using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ExoSweepLaserSequence : Sequence
{
	public class ExtraParams : BlasterStretchConeSequence.ExtraParams
	{
		public float rotationDuration;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			base.XSP_SerializeToStream(stream);
			stream.Serialize(ref rotationDuration);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			base.XSP_DeserializeFromStream(stream);
			stream.Serialize(ref rotationDuration);
		}
	}

	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;
	[JointPopup("FX attach joint on the caster")]
	public JointPopupProperty m_fxCasterJoint;
	[Header("-- Impact Fx --")]
	public GameObject m_hitFxPrefab;
	[Header("-- Start and Stop Anim Events --")]
	[AnimEventPicker]
	public Object m_startEvent;
	[AnimEventPicker]
	public Object m_stopEvent;
	[AnimEventPicker]
	public Object m_hitFailsafeEvent;
	[AnimEventPicker]
	public Object m_rotationStartEvent;
	[AnimEventPicker]
	public Object m_removePreviousLaserEvent;
	[Header("-- Audio Events --")]
	[AudioEvent(false)]
	public string m_audioEvent;
	[AudioEvent(false)]
	public string m_impactAudioEvent;
	[Space(10f)]
	public float m_maxProjectileDistInWorld = 8f;
	public PhaseTimingParameters m_phaseTimingParameters;

	private GameObject m_fx;
	private List<GameObject> m_hitFxInstances = new List<GameObject>();
	private List<ActorData> m_actorsToHit = new List<ActorData>();
	private Vector3 m_lastLaserDir;
	private float m_actorRotationDuration = 1f;
	private bool m_actorRotationStarted;
	private static readonly int animTimeToRotationGoal = Animator.StringToHash("TimeToRotationGoal");

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			OverridePhaseTimingParams(m_phaseTimingParameters, extraSequenceParams);
			if (extraSequenceParams is ExtraParams extraParams2)
			{
				if (extraParams2.lengthInSquares > 0f)
				{
					m_maxProjectileDistInWorld = extraParams2.lengthInSquares * Board.Get().squareSize;
				}
				if (extraParams2.rotationDuration > 0f)
				{
					m_actorRotationDuration = extraParams2.rotationDuration;
				}
			}
		}
		if (Targets != null)
		{
			foreach (ActorData target in Targets)
			{
				m_actorsToHit.Add(target);
			}
		}
	}

	public override void FinishSetup()
	{
		if (m_startEvent == null && m_phaseTimingParameters.ShouldSequenceBeActive())
		{
			SpawnFX();
		}
	}

	internal override void OnTurnStart(int currentTurn)
	{
		m_phaseTimingParameters.OnTurnStart(currentTurn);
	}

	internal override void OnAbilityPhaseStart(AbilityPriority abilityPhase)
	{
		m_phaseTimingParameters.OnAbilityPhaseStart(abilityPhase);
		if (m_startEvent == null && m_phaseTimingParameters.ShouldSpawnSequence(abilityPhase))
		{
			SpawnFX();
		}
		if (m_phaseTimingParameters.ShouldStopSequence(abilityPhase)
		    && m_fx != null
		    && m_fx != null)
		{
			m_fx.SetActive(false);
		}
	}

	private void Update()
	{
		if (!m_initialized || !m_phaseTimingParameters.ShouldSequenceBeActive())
		{
			return;
		}
		if (m_fx != null && Caster != null)
		{
			FriendlyEnemyVFXSelector component = m_fx.GetComponent<FriendlyEnemyVFXSelector>();
			if (m_fx != null && component != null)
			{
				component.Setup(Caster.GetTeam());
			}
		}
		ProcessSequenceVisibility();
		if (m_fxCasterJoint.m_jointObject != null)
		{
			Vector3 position = m_fxCasterJoint.m_jointObject.transform.position;
			SetAttribute(m_fx, "startPoint", position);
			Vector3 forward = m_fxCasterJoint.m_jointObject.transform.forward;
			forward.y = 0f;
			forward.Normalize();
			float projectileDistance = GetProjectileDistance(position, forward, m_maxProjectileDistInWorld);
			SetAttribute(m_fx, "endPoint", position + projectileDistance * forward);
			if (m_actorsToHit.Count > 0)
			{
				Vector3 forward2 = m_fxCasterJoint.m_jointObject.transform.forward;
				float num = Vector3.Angle(m_lastLaserDir, forward2);
				float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(m_lastLaserDir + forward2);
				float coneLengthRadiusInSquares = m_maxProjectileDistInWorld / Board.Get().squareSize + 2f;
				for (int i = m_actorsToHit.Count - 1; i >= 0; i--)
				{
					BoardSquare currentBoardSquare = m_actorsToHit[i].GetCurrentBoardSquare();
					bool isSquareInConeByActorRadius = AreaEffectUtils.IsSquareInConeByActorRadius(
						currentBoardSquare,
						Caster.GetLoSCheckPos(),
						coneCenterAngleDegrees,
						num + 1f,
						coneLengthRadiusInSquares,
						0f,
						true,
						Caster);
					if (currentBoardSquare != null && isSquareInConeByActorRadius)
					{
						DoActorHit(m_actorsToHit[i]);
						m_actorsToHit.RemoveAt(i);
					}
				}
			}

			m_lastLaserDir = m_fxCasterJoint.m_jointObject.transform.forward;
			if (m_actorRotationStarted)
			{
				Caster.GetModelAnimator().SetFloat(animTimeToRotationGoal, Caster.GetTurnToPositionTimeRemaining());
			}
		}
	}

	private float GetProjectileDistance(Vector3 start, Vector3 forward, float maxDist)
	{
		Vector3 vector = start;
		vector.y = Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(vector, forward, maxDist, false, Caster);
		return (vector - laserEndPoint).magnitude;
	}

	private void SpawnFX()
	{
		if (m_fx != null)
		{
			return;
		}
		if (!m_fxCasterJoint.IsInitialized())
		{
			GameObject referenceModel = GetReferenceModel(Caster, ReferenceModelType.Actor);
			if (referenceModel != null)
			{
				m_fxCasterJoint.Initialize(referenceModel);
			}
		}
		if (m_fxPrefab != null)
		{
			if (m_fxCasterJoint != null)
			{
				if (m_fxCasterJoint.m_jointObject != null)
				{
					Vector3 position = m_fxCasterJoint.m_jointObject.transform.position;
					m_fx = InstantiateFX(m_fxPrefab, position, default(Quaternion));
					m_lastLaserDir = m_fxCasterJoint.m_jointObject.transform.forward;
				}
				else
				{
					Log.Error(new StringBuilder().Append(gameObject.name).Append(" - m_fxCasterJoint.m_jointObject is NULL! Caster: {0} Target: {1}").ToString(), Caster.DisplayName, Target.DisplayName);
				}
			}
			else
			{
				Log.Error(new StringBuilder().Append(gameObject.name).Append(" - m_fxCasterJoint is NULL! Caster: {0} Target: {1}").ToString(), Caster.DisplayName, Target.DisplayName);
			}
		}
		if (!string.IsNullOrEmpty(m_audioEvent))
		{
			AudioManager.PostEvent(m_audioEvent, Caster.gameObject);
		}
	}

	private void StopFX()
	{
		if (m_fx != null)
		{
			m_fx.SetActive(false);
		}
		if (m_hitFxInstances != null)
		{
			foreach (GameObject fx in m_hitFxInstances)
			{
				if (fx != null)
				{
					fx.SetActive(false);
				}
			}
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (!m_phaseTimingParameters.ShouldSequenceBeActive())
		{
			return;
		}
		if (m_startEvent == parameter)
		{
			SpawnFX();
		}
		if (m_stopEvent == parameter)
		{
			StopFX();
		}
		if (m_rotationStartEvent == parameter && !m_actorRotationStarted)
		{
			Caster.TurnToPosition(TargetPos, m_actorRotationDuration);
			m_actorRotationStarted = true;
		}
		if (m_removePreviousLaserEvent == parameter)
		{
			Source.OnSequenceHit(this, Caster, null);
		}
		if (m_hitFailsafeEvent == parameter)
		{
			foreach (ActorData actor in m_actorsToHit)
			{
				DoActorHit(actor);
			}
			Source.OnSequenceHit(this, TargetPos);
		}
	}

	private void DoActorHit(ActorData actor)
	{
		Vector3 position = actor.transform.position;
		Vector3 vector = position - Caster.transform.position;
		vector.y = 0f;
		vector.Normalize();
		ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, vector);
		Source.OnSequenceHit(this, actor, impulseInfo);
		if (m_hitFxPrefab != null)
		{
			Quaternion rotation = Quaternion.LookRotation(vector);
			m_hitFxInstances.Add(InstantiateFX(m_hitFxPrefab, position, rotation));
		}
		if (!string.IsNullOrEmpty(m_impactAudioEvent))
		{
			AudioManager.PostEvent(m_impactAudioEvent, actor.gameObject);
		}
	}

	private void OnDisable()
	{
		if (m_fx != null)
		{
			Destroy(m_fx.gameObject);
			m_fx = null;
		}
		if (m_hitFxInstances != null)
		{
			for (int i = m_hitFxInstances.Count - 1; i >= 0; i--)
			{
				GameObject fx = m_hitFxInstances[i];
				if (fx != null)
				{
					Destroy(fx);
				}
			}
			m_hitFxInstances.Clear();
		}
	}
}
