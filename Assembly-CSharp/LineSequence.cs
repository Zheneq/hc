using UnityEngine;

public class LineSequence : Sequence
{
	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	[JointPopup("FX attach joint on the caster")]
	public JointPopupProperty m_fxCasterJoint;

	public ReferenceModelType m_fxCasterJointReferenceType;

	[Header("-- Whether to use target position of sequence as start position instead of joint on caster")]
	public bool m_useTargetPosAsStartPosition;

	public float m_fixedTargetPosYOffset;

	public bool m_fixedTargetPosUseGroundHeight;

	[JointPopup("FX attach joint on the target")]
	public JointPopupProperty m_fxTargetJoint;

	public ReferenceModelType m_fxTargetJointReferenceType;

	[Header("-- Whether to keep line visible if target ragdolls")]
	public bool m_canBeVisibleIfTargetRagdolled;

	public bool m_useTargetDeathPosIfRagdolled;

	public float m_targetDeathPosYOffset;

	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[AnimEventPicker]
	public Object m_startEvent;

	public float m_duration = -1f;

	private float m_despawnTime;

	[AudioEvent(false)]
	public string m_audioEvent;

	protected GameObject m_fx;

	public PhaseTimingParameters m_phaseTimingParameters;

	protected Vector3 m_fixedStartPos;

	protected const string c_startPointAttr = "startPoint";

	protected const string c_endPointAttr = "endPoint";

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		foreach (IExtraSequenceParams iParams in extraParams)
		{
			OverridePhaseTimingParams(m_phaseTimingParameters, iParams);
		}
		while (true)
		{
			m_fixedStartPos = base.TargetPos;
			if (m_fixedTargetPosUseGroundHeight)
			{
				if (Board.Get() != null)
				{
					m_fixedStartPos.y = Board.Get().BaselineHeight;
				}
			}
			m_fixedStartPos.y += m_fixedTargetPosYOffset;
			return;
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
						m_despawnTime = GameTime.time;
						return;
					}
				}
				return;
			}
		}
	}

	private void Update()
	{
		OnUpdate();
	}

	protected virtual void OnUpdate()
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
				if (m_fx != null)
				{
					if (base.Caster != null)
					{
						if (m_fx != null && m_fx.GetComponent<FriendlyEnemyVFXSelector>() != null)
						{
							m_fx.GetComponent<FriendlyEnemyVFXSelector>().Setup(base.Caster.GetTeam());
						}
					}
				}
				bool flag = m_fx != null;
				if (m_fx != null)
				{
					bool flag2 = ShouldHideForCaster();
					bool flag3 = ShouldHideForTarget();
					int num;
					if (!flag2)
					{
						num = ((!flag3) ? 1 : 0);
					}
					else
					{
						num = 0;
					}
					flag = ((byte)num != 0);
				}
				if (!flag)
				{
					SetSequenceVisibility(false);
				}
				else
				{
					ProcessSequenceVisibility();
				}
				if (m_fx != null)
				{
					if (!m_useTargetPosAsStartPosition)
					{
						if (!(m_fxCasterJoint.m_jointObject != null))
						{
							goto IL_0176;
						}
					}
					Vector3 lineStartPos = GetLineStartPos();
					Sequence.SetAttribute(m_fx, "startPoint", lineStartPos);
					goto IL_0176;
				}
				goto IL_01af;
				IL_01af:
				if (!(m_despawnTime < GameTime.time))
				{
					return;
				}
				while (true)
				{
					if (m_despawnTime > 0f)
					{
						while (true)
						{
							Object.Destroy(m_fx);
							m_fx = null;
							return;
						}
					}
					return;
				}
				IL_0176:
				if (m_fxTargetJoint.m_jointObject != null)
				{
					Vector3 lineEndPos = GetLineEndPos();
					Sequence.SetAttribute(m_fx, "endPoint", lineEndPos);
				}
				goto IL_01af;
			}
		}
	}

	protected virtual Vector3 GetLineStartPos()
	{
		if (m_useTargetPosAsStartPosition)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_fixedStartPos;
				}
			}
		}
		if (m_fxCasterJoint.m_jointObject != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_fxCasterJoint.m_jointObject.transform.position;
				}
			}
		}
		return Vector3.zero;
	}

	protected virtual Vector3 GetLineEndPos()
	{
		if (m_fxTargetJoint.m_jointObject != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					Vector3 result = m_fxTargetJoint.m_jointObject.transform.position;
					bool flag = false;
					flag = base.Target.IsModelAnimatorDisabled();
					if (m_useTargetDeathPosIfRagdolled)
					{
						if (flag)
						{
							result = base.Target.LastDeathPosition;
							result.y += m_targetDeathPosYOffset;
						}
					}
					return result;
				}
				}
			}
		}
		return Vector3.zero;
	}

	protected virtual bool ShouldHideForCaster()
	{
		int result;
		if (!m_useTargetPosAsStartPosition && m_fxCasterJointReferenceType == ReferenceModelType.Actor)
		{
			result = (ShouldHideForActorIfAttached(base.Caster) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	protected virtual bool ShouldHideForTarget()
	{
		return !m_canBeVisibleIfTargetRagdolled && m_fxTargetJointReferenceType == ReferenceModelType.Actor && ShouldHideForActorIfAttached(base.Target);
	}

	protected virtual void SpawnFX()
	{
		if (!m_fxCasterJoint.IsInitialized())
		{
			GameObject referenceModel = GetReferenceModel(base.Caster, m_fxCasterJointReferenceType);
			if (referenceModel != null)
			{
				m_fxCasterJoint.Initialize(referenceModel);
			}
		}
		if (!m_fxTargetJoint.IsInitialized())
		{
			GameObject referenceModel2 = GetReferenceModel(base.Target, m_fxTargetJointReferenceType);
			if (referenceModel2 != null)
			{
				m_fxTargetJoint.Initialize(referenceModel2);
			}
		}
		if (m_fxPrefab != null)
		{
			if (m_useTargetPosAsStartPosition)
			{
				m_fx = InstantiateFX(m_fxPrefab, m_fixedStartPos, default(Quaternion));
			}
			else if (m_fxCasterJoint != null)
			{
				if (m_fxCasterJoint.m_jointObject != null)
				{
					Vector3 position = m_fxCasterJoint.m_jointObject.transform.position;
					m_fx = InstantiateFX(m_fxPrefab, position, default(Quaternion));
				}
				else
				{
					Log.Error("LineSequence::SpawnFx() - m_fxCasterJoint.m_jointObject is NULL! Caster: {0} Target: {1}", base.Caster.DisplayName, base.Target.DisplayName);
				}
			}
			else
			{
				Log.Error("LineSequence::SpawnFx() - m_fxCasterJoint is NULL! Caster: {0} Target: {1}", base.Caster.DisplayName, base.Target.DisplayName);
			}
		}
		for (int i = 0; i < base.Targets.Length; i++)
		{
			if (base.Targets[i] != null)
			{
				Vector3 targetHitPosition = GetTargetHitPosition(i);
				Vector3 hitDirection = targetHitPosition - base.Caster.transform.position;
				hitDirection.y = 0f;
				hitDirection.Normalize();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, hitDirection);
				base.Source.OnSequenceHit(this, base.Targets[i], impulseInfo);
			}
		}
		while (true)
		{
			if (!string.IsNullOrEmpty(m_audioEvent))
			{
				AudioManager.PostEvent(m_audioEvent, base.Caster.gameObject);
			}
			if (m_duration > 0f)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						m_despawnTime = GameTime.time + m_duration;
						return;
					}
				}
			}
			m_despawnTime = -1f;
			return;
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
			return;
		}
	}

	private void OnDisable()
	{
		DestroyFx();
	}

	protected virtual void DestroyFx()
	{
		if (!(m_fx != null))
		{
			return;
		}
		while (true)
		{
			Object.Destroy(m_fx.gameObject);
			m_fx = null;
			return;
		}
	}

	public void ForceHideLine()
	{
		DestroyFx();
	}
}
