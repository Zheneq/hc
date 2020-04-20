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
			RuntimeMethodHandle runtimeMethodHandle = methodof(LineSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
		}
		this.m_fixedStartPos = base.TargetPos;
		if (this.m_fixedTargetPosUseGroundHeight)
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
			if (Board.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LineSequence.OnAbilityPhaseStart(AbilityPriority)).MethodHandle;
			}
			if (this.m_phaseTimingParameters.ShouldSpawnSequence(abilityPhase))
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
				this.SpawnFX();
			}
		}
		if (this.m_phaseTimingParameters.ShouldStopSequence(abilityPhase))
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
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LineSequence.OnUpdate()).MethodHandle;
			}
			if (this.m_phaseTimingParameters.ShouldSequenceBeActive())
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
					if (base.Caster != null)
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
						if (this.m_fx != null && this.m_fx.GetComponent<FriendlyEnemyVFXSelector>() != null)
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
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
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
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!this.m_useTargetPosAsStartPosition)
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
						if (!(this.m_fxCasterJoint.m_jointObject != null))
						{
							goto IL_176;
						}
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					Vector3 lineStartPos = this.GetLineStartPos();
					Sequence.SetAttribute(this.m_fx, "startPoint", lineStartPos);
					IL_176:
					if (this.m_fxTargetJoint.m_jointObject != null)
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
						Vector3 lineEndPos = this.GetLineEndPos();
						Sequence.SetAttribute(this.m_fx, "endPoint", lineEndPos);
					}
				}
				if (this.m_despawnTime < GameTime.time)
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
					if (this.m_despawnTime > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LineSequence.GetLineStartPos()).MethodHandle;
			}
			return this.m_fixedStartPos;
		}
		if (this.m_fxCasterJoint.m_jointObject != null)
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
			return this.m_fxCasterJoint.m_jointObject.transform.position;
		}
		return Vector3.zero;
	}

	protected virtual Vector3 GetLineEndPos()
	{
		if (this.m_fxTargetJoint.m_jointObject != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LineSequence.GetLineEndPos()).MethodHandle;
			}
			Vector3 result = this.m_fxTargetJoint.m_jointObject.transform.position;
			bool flag = base.Target.IsModelAnimatorDisabled();
			if (this.m_useTargetDeathPosIfRagdolled)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LineSequence.ShouldHideForCaster()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LineSequence.SpawnFX()).MethodHandle;
			}
			GameObject referenceModel = base.GetReferenceModel(base.Caster, this.m_fxCasterJointReferenceType);
			if (referenceModel != null)
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
				this.m_fxCasterJoint.Initialize(referenceModel);
			}
		}
		if (!this.m_fxTargetJoint.IsInitialized())
		{
			GameObject referenceModel2 = base.GetReferenceModel(base.Target, this.m_fxTargetJointReferenceType);
			if (referenceModel2 != null)
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
				this.m_fxTargetJoint.Initialize(referenceModel2);
			}
		}
		if (this.m_fxPrefab != null)
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
			if (this.m_useTargetPosAsStartPosition)
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
				Quaternion rotation = default(Quaternion);
				this.m_fx = base.InstantiateFX(this.m_fxPrefab, this.m_fixedStartPos, rotation, true, true);
			}
			else if (this.m_fxCasterJoint != null)
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
				if (this.m_fxCasterJoint.m_jointObject != null)
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
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 targetHitPosition = base.GetTargetHitPosition(i);
				Vector3 hitDirection = targetHitPosition - base.Caster.transform.position;
				hitDirection.y = 0f;
				hitDirection.Normalize();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, hitDirection);
				base.Source.OnSequenceHit(this, base.Targets[i], impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
			}
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
		if (!string.IsNullOrEmpty(this.m_audioEvent))
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
			AudioManager.PostEvent(this.m_audioEvent, base.Caster.gameObject);
		}
		if (this.m_duration > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LineSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LineSequence.DestroyFx()).MethodHandle;
			}
			UnityEngine.Object.Destroy(this.m_fx.gameObject);
			this.m_fx = null;
		}
	}

	public void ForceHideLine()
	{
		this.DestroyFx();
	}
}
