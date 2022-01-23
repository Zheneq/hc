using System.Collections.Generic;
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
			ExtraParams extraParams2 = extraSequenceParams as ExtraParams;
			if (extraParams2 == null)
			{
				continue;
			}
			if (extraParams2.lengthInSquares > 0f)
			{
				m_maxProjectileDistInWorld = extraParams2.lengthInSquares * Board.Get().squareSize;
			}
			if (extraParams2.rotationDuration > 0f)
			{
				m_actorRotationDuration = extraParams2.rotationDuration;
			}
		}
		while (true)
		{
			if (base.Targets == null)
			{
				return;
			}
			while (true)
			{
				for (int j = 0; j < base.Targets.Length; j++)
				{
					m_actorsToHit.Add(base.Targets[j]);
				}
				while (true)
				{
					switch (4)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}

	public override void FinishSetup()
	{
		if (!(m_startEvent == null) || !m_phaseTimingParameters.ShouldSequenceBeActive())
		{
			return;
		}
		while (true)
		{
			SpawnFX();
			return;
		}
	}

	internal override void OnTurnStart(int currentTurn)
	{
		m_phaseTimingParameters.OnTurnStart(currentTurn);
	}

	internal override void OnAbilityPhaseStart(AbilityPriority abilityPhase)
	{
		m_phaseTimingParameters.OnAbilityPhaseStart(abilityPhase);
		if (m_startEvent == null)
		{
			if (m_phaseTimingParameters.ShouldSpawnSequence(abilityPhase))
			{
				SpawnFX();
			}
		}
		if (!m_phaseTimingParameters.ShouldStopSequence(abilityPhase))
		{
			return;
		}
		while (true)
		{
			if (!(m_fx != null))
			{
				return;
			}
			while (true)
			{
				if ((bool)m_fx)
				{
					while (true)
					{
						m_fx.SetActive(false);
						return;
					}
				}
				return;
			}
		}
	}

	private void Update()
	{
		if (!m_initialized)
		{
			return;
		}
		while (true)
		{
			if (!m_phaseTimingParameters.ShouldSequenceBeActive())
			{
				return;
			}
			while (true)
			{
				if (m_fx != null && base.Caster != null)
				{
					FriendlyEnemyVFXSelector component = m_fx.GetComponent<FriendlyEnemyVFXSelector>();
					if (m_fx != null && component != null)
					{
						component.Setup(base.Caster.GetTeam());
					}
				}
				ProcessSequenceVisibility();
				if (!(m_fxCasterJoint.m_jointObject != null))
				{
					return;
				}
				while (true)
				{
					Vector3 position = m_fxCasterJoint.m_jointObject.transform.position;
					Sequence.SetAttribute(m_fx, "startPoint", position);
					Vector3 forward = m_fxCasterJoint.m_jointObject.transform.forward;
					forward.y = 0f;
					forward.Normalize();
					float projectileDistance = GetProjectileDistance(position, forward, m_maxProjectileDistInWorld);
					Sequence.SetAttribute(m_fx, "endPoint", position + projectileDistance * forward);
					if (m_actorsToHit.Count > 0)
					{
						Vector3 forward2 = m_fxCasterJoint.m_jointObject.transform.forward;
						float num = Vector3.Angle(m_lastLaserDir, forward2);
						Vector3 vec = m_lastLaserDir + forward2;
						float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(vec);
						float coneLengthRadiusInSquares = m_maxProjectileDistInWorld / Board.Get().squareSize + 2f;
						for (int num2 = m_actorsToHit.Count - 1; num2 >= 0; num2--)
						{
							BoardSquare currentBoardSquare = m_actorsToHit[num2].GetCurrentBoardSquare();
							if (currentBoardSquare != null && AreaEffectUtils.IsSquareInConeByActorRadius(currentBoardSquare, base.Caster.GetLoSCheckPos(), coneCenterAngleDegrees, num + 1f, coneLengthRadiusInSquares, 0f, true, base.Caster))
							{
								DoActorHit(m_actorsToHit[num2]);
								m_actorsToHit.RemoveAt(num2);
							}
						}
					}
					m_lastLaserDir = m_fxCasterJoint.m_jointObject.transform.forward;
					if (m_actorRotationStarted)
					{
						while (true)
						{
							Animator modelAnimator = base.Caster.GetModelAnimator();
							modelAnimator.SetFloat(animTimeToRotationGoal, base.Caster.GetTurnToPositionTimeRemaining());
							return;
						}
					}
					return;
				}
			}
		}
	}

	private float GetProjectileDistance(Vector3 start, Vector3 forward, float maxDist)
	{
		Vector3 vector = start;
		vector.y = (float)Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(vector, forward, maxDist, false, base.Caster);
		return (vector - laserEndPoint).magnitude;
	}

	private void SpawnFX()
	{
		if (m_fx != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (!m_fxCasterJoint.IsInitialized())
		{
			GameObject referenceModel = GetReferenceModel(base.Caster, ReferenceModelType.Actor);
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
					Log.Error(base.gameObject.name + " - m_fxCasterJoint.m_jointObject is NULL! Caster: {0} Target: {1}", base.Caster.DisplayName, base.Target.DisplayName);
				}
			}
			else
			{
				Log.Error(base.gameObject.name + " - m_fxCasterJoint is NULL! Caster: {0} Target: {1}", base.Caster.DisplayName, base.Target.DisplayName);
			}
		}
		if (string.IsNullOrEmpty(m_audioEvent))
		{
			return;
		}
		while (true)
		{
			AudioManager.PostEvent(m_audioEvent, base.Caster.gameObject);
			return;
		}
	}

	private void StopFX()
	{
		if (m_fx != null)
		{
			m_fx.SetActive(false);
		}
		if (m_hitFxInstances == null)
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < m_hitFxInstances.Count; i++)
			{
				if (m_hitFxInstances[i] != null)
				{
					m_hitFxInstances[i].SetActive(false);
				}
			}
			while (true)
			{
				switch (1)
				{
				default:
					return;
				case 0:
					break;
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
		while (true)
		{
			if (m_startEvent == parameter)
			{
				SpawnFX();
			}
			if (m_stopEvent == parameter)
			{
				StopFX();
			}
			if (m_rotationStartEvent == parameter)
			{
				if (!m_actorRotationStarted)
				{
					base.Caster.TurnToPosition(base.TargetPos, m_actorRotationDuration);
					m_actorRotationStarted = true;
				}
			}
			if (m_removePreviousLaserEvent == parameter)
			{
				base.Source.OnSequenceHit(this, base.Caster, null);
			}
			if (!(m_hitFailsafeEvent == parameter))
			{
				return;
			}
			while (true)
			{
				for (int i = 0; i < m_actorsToHit.Count; i++)
				{
					DoActorHit(m_actorsToHit[i]);
				}
				base.Source.OnSequenceHit(this, base.TargetPos);
				return;
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
		base.Source.OnSequenceHit(this, actor, impulseInfo);
		if ((bool)m_hitFxPrefab)
		{
			Quaternion rotation = Quaternion.LookRotation(vector);
			m_hitFxInstances.Add(InstantiateFX(m_hitFxPrefab, position, rotation));
		}
		if (string.IsNullOrEmpty(m_impactAudioEvent))
		{
			return;
		}
		while (true)
		{
			AudioManager.PostEvent(m_impactAudioEvent, actor.gameObject);
			return;
		}
	}

	private void OnDisable()
	{
		if (m_fx != null)
		{
			Object.Destroy(m_fx.gameObject);
			m_fx = null;
		}
		if (m_hitFxInstances == null)
		{
			return;
		}
		for (int num = m_hitFxInstances.Count - 1; num >= 0; num--)
		{
			GameObject gameObject = m_hitFxInstances[num];
			if (gameObject != null)
			{
				Object.Destroy(gameObject);
				gameObject = null;
			}
		}
		while (true)
		{
			m_hitFxInstances.Clear();
			return;
		}
	}
}
