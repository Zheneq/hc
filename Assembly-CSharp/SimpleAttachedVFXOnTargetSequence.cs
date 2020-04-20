using System;
using UnityEngine;

public class SimpleAttachedVFXOnTargetSequence : Sequence
{
	[Separator("FX To Spawn", true)]
	public GameObject m_fxPrefab;

	[JointPopup("FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_fxJoint;

	[Tooltip("Check if Fx Prefab should stay attached to the joint. If unchecked, the Fx Prefab will start with the joint position and rotation.")]
	public bool m_fxAttachToJoint;

	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[Separator("Anim Events -- ( main FX start / stop )", "orange")]
	[AnimEventPicker]
	public UnityEngine.Object m_startEvent;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before stopping the sequence. Search project for EventObjects.")]
	public UnityEngine.Object m_stopEvent;

	[Separator("Start Delay (ignored if there is Start Event)", true)]
	public float m_startDelayTime;

	private float m_timeToSpawnVfx = -1f;

	[Tooltip("Aim the Fx Prefab at the caster. If unchecked, inherits the attach joint transformation.")]
	[Separator("Orientation / Alignment", true)]
	public bool m_aimAtCaster = true;

	public bool m_useRootOrientation;

	[Separator("Audio Event -- ( on FX spawn )", "orange")]
	[AudioEvent(false)]
	public string m_audioEvent;

	[Tooltip("Whether to play audio event on Caster rather than Target")]
	public bool m_playAudioEventOnCaster;

	[Separator("Gameplay Hit", true)]
	public bool m_callOnHitForGameplay;

	private bool m_sequenceHitCalled;

	[Separator("Phase-Based Timing", true)]
	public Sequence.PhaseTimingParameters m_phaseTimingParameters;

	protected GameObject m_fx;

	private float m_despawnCheckStartTime;

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		foreach (Sequence.IExtraSequenceParams iParams in extraParams)
		{
			base.OverridePhaseTimingParams(this.m_phaseTimingParameters, iParams);
		}
	}

	private bool Finished()
	{
		if (this.m_fxAttachToJoint)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXOnTargetSequence.Finished()).MethodHandle;
			}
			return false;
		}
		return base.AreFXFinished(this.m_fx);
	}

	public override void FinishSetup()
	{
		if (this.m_startEvent == null && this.m_phaseTimingParameters.ShouldSequenceBeActive())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXOnTargetSequence.FinishSetup()).MethodHandle;
			}
			if (this.m_startDelayTime <= 0f)
			{
				this.SpawnFX();
			}
			else
			{
				this.m_timeToSpawnVfx = GameTime.time + this.m_startDelayTime;
			}
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
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXOnTargetSequence.OnAbilityPhaseStart(AbilityPriority)).MethodHandle;
			}
			if (this.m_phaseTimingParameters.ShouldSpawnSequence(abilityPhase) && this.m_phaseTimingParameters.ShouldSequenceBeActive())
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
				this.SpawnFX();
			}
		}
		if (this.m_phaseTimingParameters.ShouldStopSequence(abilityPhase))
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
				this.StopFX();
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
			if (this.m_timeToSpawnVfx > 0f && GameTime.time >= this.m_timeToSpawnVfx)
			{
				this.m_timeToSpawnVfx = -1f;
				this.SpawnFX();
			}
			if (this.m_fx != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXOnTargetSequence.OnUpdate()).MethodHandle;
				}
				if (this.m_fxAttachToJoint)
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
					if (this.m_fxJoint.IsInitialized())
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
							if (base.Targets.Length > 0)
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
								if (base.ShouldHideForActorIfAttached(base.Targets[0]))
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
									base.SetSequenceVisibility(false);
									goto IL_E0;
								}
							}
						}
					}
				}
			}
			base.ProcessSequenceVisibility();
			IL_E0:
			if (this.m_fx != null)
			{
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
					if (base.Caster != null)
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
						this.m_fx.GetComponent<FriendlyEnemyVFXSelector>().Setup(base.Caster.GetTeam());
					}
				}
				if (this.m_useRootOrientation)
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
						if (base.Targets.Length == 1)
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
							this.m_fx.transform.rotation = base.Targets[0].transform.rotation;
							goto IL_285;
						}
					}
				}
				if (this.m_aimAtCaster)
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
					if (base.Caster != null)
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
						if (base.Targets != null && base.Targets.Length == 1)
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
							Vector3 forward = base.Targets[0].transform.position - base.Caster.transform.position;
							forward.y = 0f;
							if (forward.sqrMagnitude != 0f)
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
								forward.Normalize();
								this.m_fx.transform.rotation = Quaternion.LookRotation(forward);
							}
						}
					}
				}
				IL_285:
				if (this.Finished() && GameTime.time > this.m_despawnCheckStartTime)
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
					base.MarkForRemoval();
				}
			}
		}
	}

	protected void SwitchFxTo(GameObject fxPrefab)
	{
		bool flag;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXOnTargetSequence.SwitchFxTo(GameObject)).MethodHandle;
			}
			flag = this.m_fx.activeInHierarchy;
		}
		else
		{
			flag = false;
		}
		bool sequenceVisibility = flag;
		Vector3 position = this.m_fx.transform.position;
		Quaternion rotation = this.m_fx.transform.rotation;
		UnityEngine.Object.Destroy(this.m_fx);
		this.m_fx = null;
		if (fxPrefab != null)
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
			this.m_fx = base.InstantiateFX(fxPrefab, position, rotation, true, true);
			base.SetSequenceVisibility(sequenceVisibility);
			FriendlyEnemyVFXSelector component = this.m_fx.GetComponent<FriendlyEnemyVFXSelector>();
			if (component != null)
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
				component.Setup(base.Caster.GetTeam());
			}
			if (this.m_fxJoint.m_jointObject != null && this.m_fxJoint.m_jointObject.transform.localScale != Vector3.zero)
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
				if (this.m_fxAttachToJoint)
				{
					base.AttachToBone(this.m_fx, this.m_fxJoint.m_jointObject);
					this.m_fx.transform.localPosition = Vector3.zero;
					this.m_fx.transform.localRotation = Quaternion.identity;
				}
			}
		}
	}

	private void StopFX()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXOnTargetSequence.StopFX()).MethodHandle;
			}
			this.m_fx.SetActive(false);
		}
	}

	private void SpawnFX()
	{
		if (base.Targets.Length > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXOnTargetSequence.SpawnFX()).MethodHandle;
			}
			if (base.Targets[0] != null)
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
				if (!this.m_fxJoint.IsInitialized())
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
					this.m_fxJoint.Initialize(base.Targets[0].gameObject);
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
					if (this.m_fxJoint.m_jointObject != null && this.m_fxJoint.m_jointObject.transform.localScale != Vector3.zero && this.m_fxAttachToJoint)
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
						this.m_fx = base.InstantiateFX(this.m_fxPrefab);
						base.AttachToBone(this.m_fx, this.m_fxJoint.m_jointObject);
						this.m_fx.transform.localPosition = Vector3.zero;
						this.m_fx.transform.localRotation = Quaternion.identity;
					}
					else
					{
						Vector3 position = this.m_fxJoint.m_jointObject.transform.position;
						Quaternion rotation = default(Quaternion);
						if (this.m_aimAtCaster)
						{
							Vector3 position2 = base.Caster.transform.position;
							Vector3 lookRotation = position2 - position;
							lookRotation.y = 0f;
							lookRotation.Normalize();
							rotation.SetLookRotation(lookRotation);
						}
						else
						{
							rotation = this.m_fxJoint.m_jointObject.transform.rotation;
						}
						this.m_fx = base.InstantiateFX(this.m_fxPrefab, position, rotation, true, true);
						Sequence.SetAttribute(this.m_fx, "abilityAreaLength", (base.TargetPos - position).magnitude);
					}
					Sequence.SetAttribute(this.m_fx, "targetDiameter", base.Targets[0].GetActorModelData().GetModelSize());
					this.m_despawnCheckStartTime = GameTime.time + 2f;
				}
				if (!string.IsNullOrEmpty(this.m_audioEvent))
				{
					string audioEvent = this.m_audioEvent;
					GameObject gameObject;
					if (this.m_playAudioEventOnCaster)
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
						gameObject = base.Caster.gameObject;
					}
					else
					{
						gameObject = base.Targets[0].gameObject;
					}
					AudioManager.PostEvent(audioEvent, gameObject);
				}
			}
		}
		if (this.m_callOnHitForGameplay)
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
			if (!this.m_sequenceHitCalled)
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
				for (int i = 0; i < base.Targets.Length; i++)
				{
					if (base.Targets[i] != null)
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
						base.Source.OnSequenceHit(this, base.Targets[i], Sequence.CreateImpulseInfoBetweenActors(base.Targets[0], base.Targets[i]), ActorModelData.RagdollActivation.HealthBased, true);
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
				base.Source.OnSequenceHit(this, base.TargetPos, null);
				this.m_sequenceHitCalled = true;
			}
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_phaseTimingParameters.ShouldSequenceBeActive())
		{
			if (this.m_startEvent == parameter)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXOnTargetSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
				}
				this.SpawnFX();
			}
			else if (this.m_stopEvent == parameter)
			{
				this.StopFX();
			}
		}
	}

	private void OnDisable()
	{
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXOnTargetSequence.OnDisable()).MethodHandle;
			}
			UnityEngine.Object.Destroy(this.m_fx.gameObject);
			this.m_fx = null;
		}
	}
}
