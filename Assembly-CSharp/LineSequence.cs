using System;
using UnityEngine;

public class LineSequence : Sequence
{
	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	[JointPopup("FX attach joint on the caster")]
	public JointPopupProperty m_fxCasterJoint;

	public Sequence.ReferenceModelType m_fxCasterJointReferenceType;

	[Header("-- Whether to use target position of sequence as start position instead of joint on caster")]
	public bool m_useTargetPosAsStartPosition;

	public float m_fixedTargetPosYOffset;

	public bool m_fixedTargetPosUseGroundHeight;

	[JointPopup("FX attach joint on the target")]
	public JointPopupProperty m_fxTargetJoint;

	public Sequence.ReferenceModelType m_fxTargetJointReferenceType;

	[Header("-- Whether to keep line visible if target ragdolls")]
	public bool m_canBeVisibleIfTargetRagdolled;

	public bool m_useTargetDeathPosIfRagdolled;

	public float m_targetDeathPosYOffset;

	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[AnimEventPicker]
	public UnityEngine.Object m_startEvent;

	public float m_duration = -1f;

	private float m_despawnTime;

	[AudioEvent(false)]
	public string m_audioEvent;

	protected GameObject m_fx;

	public Sequence.PhaseTimingParameters m_phaseTimingParameters;

	protected Vector3 m_fixedStartPos;

	protected const string c_startPointAttr = "startPoint";

	protected const string c_endPointAttr = "endPoint";

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		foreach (Sequence.IExtraSequenceParams iParams in extraParams)
		{
			base.OverridePhaseTimingParams(this.m_phaseTimingParameters, iParams);
		}
		this.m_fixedStartPos = base.TargetPos;
		if (this.m_fixedTargetPosUseGroundHeight)
		{
			if (Board.Get() != null)
			{
				this.m_fixedStartPos.y = (float)Board.Get().BaselineHeight;
			}
		}
		this.m_fixedStartPos.y = this.m_fixedStartPos.y + this.m_fixedTargetPosYOffset;
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
					this.m_despawnTime = GameTime.time;
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
		if (this.m_initialized)
		{
			if (this.m_phaseTimingParameters.ShouldSequenceBeActive())
			{
				if (this.m_fx != null)
				{
					if (base.Caster != null)
					{
						if (this.m_fx != null && this.m_fx.GetComponent<FriendlyEnemyVFXSelector>() != null)
						{
							this.m_fx.GetComponent<FriendlyEnemyVFXSelector>().Setup(base.Caster.GetTeam());
						}
					}
				}
				bool flag = this.m_fx != null;
				if (this.m_fx != null)
				{
					bool flag2 = this.ShouldHideForCaster();
					bool flag3 = this.ShouldHideForTarget();
					bool flag4;
					if (!flag2)
					{
						flag4 = !flag3;
					}
					else
					{
						flag4 = false;
					}
					flag = flag4;
				}
				if (!flag)
				{
					base.SetSequenceVisibility(false);
				}
				else
				{
					base.ProcessSequenceVisibility();
				}
				if (this.m_fx != null)
				{
					if (!this.m_useTargetPosAsStartPosition)
					{
						if (!(this.m_fxCasterJoint.m_jointObject != null))
						{
							goto IL_176;
						}
					}
					Vector3 lineStartPos = this.GetLineStartPos();
					Sequence.SetAttribute(this.m_fx, "startPoint", lineStartPos);
					IL_176:
					if (this.m_fxTargetJoint.m_jointObject != null)
					{
						Vector3 lineEndPos = this.GetLineEndPos();
						Sequence.SetAttribute(this.m_fx, "endPoint", lineEndPos);
					}
				}
				if (this.m_despawnTime < GameTime.time)
				{
					if (this.m_despawnTime > 0f)
					{
						UnityEngine.Object.Destroy(this.m_fx);
						this.m_fx = null;
					}
				}
			}
		}
	}

	protected virtual Vector3 GetLineStartPos()
	{
		if (this.m_useTargetPosAsStartPosition)
		{
			return this.m_fixedStartPos;
		}
		if (this.m_fxCasterJoint.m_jointObject != null)
		{
			return this.m_fxCasterJoint.m_jointObject.transform.position;
		}
		return Vector3.zero;
	}

	protected virtual Vector3 GetLineEndPos()
	{
		if (this.m_fxTargetJoint.m_jointObject != null)
		{
			Vector3 result = this.m_fxTargetJoint.m_jointObject.transform.position;
			bool flag = base.Target.IsModelAnimatorDisabled();
			if (this.m_useTargetDeathPosIfRagdolled)
			{
				if (flag)
				{
					result = base.Target.LastDeathPosition;
					result.y += this.m_targetDeathPosYOffset;
				}
			}
			return result;
		}
		return Vector3.zero;
	}

	protected virtual bool ShouldHideForCaster()
	{
		bool result;
		if (!this.m_useTargetPosAsStartPosition && this.m_fxCasterJointReferenceType == Sequence.ReferenceModelType.Actor)
		{
			result = base.ShouldHideForActorIfAttached(base.Caster);
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected virtual bool ShouldHideForTarget()
	{
		return !this.m_canBeVisibleIfTargetRagdolled && this.m_fxTargetJointReferenceType == Sequence.ReferenceModelType.Actor && base.ShouldHideForActorIfAttached(base.Target);
	}

	protected virtual void SpawnFX()
	{
		if (!this.m_fxCasterJoint.IsInitialized())
		{
			GameObject referenceModel = base.GetReferenceModel(base.Caster, this.m_fxCasterJointReferenceType);
			if (referenceModel != null)
			{
				this.m_fxCasterJoint.Initialize(referenceModel);
			}
		}
		if (!this.m_fxTargetJoint.IsInitialized())
		{
			GameObject referenceModel2 = base.GetReferenceModel(base.Target, this.m_fxTargetJointReferenceType);
			if (referenceModel2 != null)
			{
				this.m_fxTargetJoint.Initialize(referenceModel2);
			}
		}
		if (this.m_fxPrefab != null)
		{
			if (this.m_useTargetPosAsStartPosition)
			{
				Quaternion rotation = default(Quaternion);
				this.m_fx = base.InstantiateFX(this.m_fxPrefab, this.m_fixedStartPos, rotation, true, true);
			}
			else if (this.m_fxCasterJoint != null)
			{
				if (this.m_fxCasterJoint.m_jointObject != null)
				{
					Vector3 position = this.m_fxCasterJoint.m_jointObject.transform.position;
					Quaternion rotation2 = default(Quaternion);
					this.m_fx = base.InstantiateFX(this.m_fxPrefab, position, rotation2, true, true);
				}
				else
				{
					Log.Error("LineSequence::SpawnFx() - m_fxCasterJoint.m_jointObject is NULL! Caster: {0} Target: {1}", new object[]
					{
						base.Caster.DisplayName,
						base.Target.DisplayName
					});
				}
			}
			else
			{
				Log.Error("LineSequence::SpawnFx() - m_fxCasterJoint is NULL! Caster: {0} Target: {1}", new object[]
				{
					base.Caster.DisplayName,
					base.Target.DisplayName
				});
			}
		}
		for (int i = 0; i < base.Targets.Length; i++)
		{
			if (base.Targets[i] != null)
			{
				Vector3 targetHitPosition = base.GetTargetHitPosition(i);
				Vector3 hitDirection = targetHitPosition - base.Caster.transform.position;
				hitDirection.y = 0f;
				hitDirection.Normalize();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, hitDirection);
				base.Source.OnSequenceHit(this, base.Targets[i], impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
			}
		}
		if (!string.IsNullOrEmpty(this.m_audioEvent))
		{
			AudioManager.PostEvent(this.m_audioEvent, base.Caster.gameObject);
		}
		if (this.m_duration > 0f)
		{
			this.m_despawnTime = GameTime.time + this.m_duration;
		}
		else
		{
			this.m_despawnTime = -1f;
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
		}
	}

	private void OnDisable()
	{
		this.DestroyFx();
	}

	protected virtual void DestroyFx()
	{
		if (this.m_fx != null)
		{
			UnityEngine.Object.Destroy(this.m_fx.gameObject);
			this.m_fx = null;
		}
	}

	public void ForceHideLine()
	{
		this.DestroyFx();
	}
}
