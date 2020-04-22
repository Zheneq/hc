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
	public Object m_startEvent;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before stopping the sequence. Search project for EventObjects.")]
	public Object m_stopEvent;

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
	public PhaseTimingParameters m_phaseTimingParameters;

	protected GameObject m_fx;

	private float m_despawnCheckStartTime;

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		foreach (IExtraSequenceParams iParams in extraParams)
		{
			OverridePhaseTimingParams(m_phaseTimingParameters, iParams);
		}
	}

	private bool Finished()
	{
		if (m_fxAttachToJoint)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return false;
				}
			}
		}
		return AreFXFinished(m_fx);
	}

	public override void FinishSetup()
	{
		if (!(m_startEvent == null) || !m_phaseTimingParameters.ShouldSequenceBeActive())
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_startDelayTime <= 0f)
			{
				SpawnFX();
			}
			else
			{
				m_timeToSpawnVfx = GameTime.time + m_startDelayTime;
			}
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
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_phaseTimingParameters.ShouldSpawnSequence(abilityPhase) && m_phaseTimingParameters.ShouldSequenceBeActive())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				SpawnFX();
			}
		}
		if (!m_phaseTimingParameters.ShouldStopSequence(abilityPhase))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (m_fx != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					StopFX();
					return;
				}
			}
			return;
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
		if (m_timeToSpawnVfx > 0f && GameTime.time >= m_timeToSpawnVfx)
		{
			m_timeToSpawnVfx = -1f;
			SpawnFX();
		}
		if (m_fx != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_fxAttachToJoint)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_fxJoint.IsInitialized())
				{
					while (true)
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
						while (true)
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
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							if (ShouldHideForActorIfAttached(base.Targets[0]))
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								SetSequenceVisibility(false);
								goto IL_00e0;
							}
						}
					}
				}
			}
		}
		ProcessSequenceVisibility();
		goto IL_00e0;
		IL_0285:
		if (!Finished() || !(GameTime.time > m_despawnCheckStartTime))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			MarkForRemoval();
			return;
		}
		IL_00e0:
		if (!(m_fx != null))
		{
			return;
		}
		if (m_fx.GetComponent<FriendlyEnemyVFXSelector>() != null)
		{
			while (true)
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
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				m_fx.GetComponent<FriendlyEnemyVFXSelector>().Setup(base.Caster.GetTeam());
			}
		}
		if (m_useRootOrientation)
		{
			while (true)
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
				while (true)
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
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					m_fx.transform.rotation = base.Targets[0].transform.rotation;
					goto IL_0285;
				}
			}
		}
		if (m_aimAtCaster)
		{
			while (true)
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
				while (true)
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
					while (true)
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
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						forward.Normalize();
						m_fx.transform.rotation = Quaternion.LookRotation(forward);
					}
				}
			}
		}
		goto IL_0285;
	}

	protected void SwitchFxTo(GameObject fxPrefab)
	{
		int num;
		if (m_fx != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = (m_fx.activeInHierarchy ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool sequenceVisibility = (byte)num != 0;
		Vector3 position = m_fx.transform.position;
		Quaternion rotation = m_fx.transform.rotation;
		Object.Destroy(m_fx);
		m_fx = null;
		if (!(fxPrefab != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			m_fx = InstantiateFX(fxPrefab, position, rotation);
			SetSequenceVisibility(sequenceVisibility);
			FriendlyEnemyVFXSelector component = m_fx.GetComponent<FriendlyEnemyVFXSelector>();
			if (component != null)
			{
				while (true)
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
			if (!(m_fxJoint.m_jointObject != null) || !(m_fxJoint.m_jointObject.transform.localScale != Vector3.zero))
			{
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (m_fxAttachToJoint)
				{
					AttachToBone(m_fx, m_fxJoint.m_jointObject);
					m_fx.transform.localPosition = Vector3.zero;
					m_fx.transform.localRotation = Quaternion.identity;
				}
				return;
			}
		}
	}

	private void StopFX()
	{
		if (!(m_fx != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_fx.SetActive(false);
			return;
		}
	}

	private void SpawnFX()
	{
		if (base.Targets.Length > 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (base.Targets[0] != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!m_fxJoint.IsInitialized())
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					m_fxJoint.Initialize(base.Targets[0].gameObject);
				}
				if (m_fxPrefab != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_fxJoint.m_jointObject != null && m_fxJoint.m_jointObject.transform.localScale != Vector3.zero && m_fxAttachToJoint)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						m_fx = InstantiateFX(m_fxPrefab);
						AttachToBone(m_fx, m_fxJoint.m_jointObject);
						m_fx.transform.localPosition = Vector3.zero;
						m_fx.transform.localRotation = Quaternion.identity;
					}
					else
					{
						Vector3 position = m_fxJoint.m_jointObject.transform.position;
						Quaternion rotation = default(Quaternion);
						if (m_aimAtCaster)
						{
							Vector3 position2 = base.Caster.transform.position;
							Vector3 lookRotation = position2 - position;
							lookRotation.y = 0f;
							lookRotation.Normalize();
							rotation.SetLookRotation(lookRotation);
						}
						else
						{
							rotation = m_fxJoint.m_jointObject.transform.rotation;
						}
						m_fx = InstantiateFX(m_fxPrefab, position, rotation);
						Sequence.SetAttribute(m_fx, "abilityAreaLength", (base.TargetPos - position).magnitude);
					}
					Sequence.SetAttribute(m_fx, "targetDiameter", base.Targets[0].GetActorModelData().GetModelSize());
					m_despawnCheckStartTime = GameTime.time + 2f;
				}
				if (!string.IsNullOrEmpty(m_audioEvent))
				{
					string audioEvent = m_audioEvent;
					GameObject gameObject;
					if (m_playAudioEventOnCaster)
					{
						while (true)
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
		if (!m_callOnHitForGameplay)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (m_sequenceHitCalled)
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				for (int i = 0; i < base.Targets.Length; i++)
				{
					if (base.Targets[i] != null)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						base.Source.OnSequenceHit(this, base.Targets[i], Sequence.CreateImpulseInfoBetweenActors(base.Targets[0], base.Targets[i]));
					}
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					base.Source.OnSequenceHit(this, base.TargetPos);
					m_sequenceHitCalled = true;
					return;
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
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					SpawnFX();
					return;
				}
			}
		}
		if (m_stopEvent == parameter)
		{
			StopFX();
		}
	}

	private void OnDisable()
	{
		if (!(m_fx != null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Object.Destroy(m_fx.gameObject);
			m_fx = null;
			return;
		}
	}
}
