using System.Collections.Generic;
using UnityEngine;

public class SimpleVFXAtTargetPosSequence : Sequence
{
	public class IgnoreStartEventExtraParam : IExtraSequenceParams
	{
		public bool ignoreStartEvent;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref ignoreStartEvent);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref ignoreStartEvent);
		}
	}

	public class PositionOverrideParam : IExtraSequenceParams
	{
		public Vector3 m_positionOverride;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref m_positionOverride);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref m_positionOverride);
		}
	}

	[Separator("FX To Spawn", true)]
	public GameObject m_fxPrefab;

	[Separator("Height", true)]
	public float m_yOffset;

	public bool m_useGroundHeight;

	private GameObject m_fx;

	private FriendlyEnemyVFXSelector m_fxFoFSelectComp;

	[Separator("Start delay time (ignored if has Start Event)", true)]
	public float m_startDelayTime;

	private float m_timeToSpawnVfx = -1f;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[Separator("Anim Events -- ( start / stop )", "orange")]
	public Object m_startEvent;

	private bool m_ignoreStartEvent;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before stopping the sequence. Search project for EventObjects.")]
	public Object m_stopEvent;

	[Separator("Gameplay Hits", true)]
	public bool m_callOnHitForGameplay;

	public float m_hitDelayTime;

	private float m_timeToHit = -1f;

	private bool m_sequenceHitCalled;

	[Separator("Audio Event -- ( on FX spawn )", "orange")]
	[AudioEvent(false)]
	public string m_audioEvent;

	[Separator("Phase-Based Timing", true)]
	public PhaseTimingParameters m_phaseTimingParameters;

	[Separator("Special case handler for additional VFX at target position", true)]
	public AdditionalVfxContainerBase m_additionalFxAtTargetPos;

	private int m_initialTimerControllerValue;

	private int m_timeControllerValueNow = -100;

	private Dictionary<string, float> m_fxAttributes = new Dictionary<string, float>();

	private Vector3 m_fxSpawnPosition;

	internal override Vector3 GetSequencePos()
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_fx.transform.position;
				}
			}
		}
		return Vector3.zero;
	}

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		m_fxSpawnPosition = base.TargetPos;
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			OverridePhaseTimingParams(m_phaseTimingParameters, extraSequenceParams);
			IgnoreStartEventExtraParam ignoreStartEventExtraParam = extraSequenceParams as IgnoreStartEventExtraParam;
			if (ignoreStartEventExtraParam != null)
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
				m_ignoreStartEvent = ignoreStartEventExtraParam.ignoreStartEvent;
			}
			if (extraSequenceParams is PositionOverrideParam)
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
				PositionOverrideParam positionOverrideParam = extraSequenceParams as PositionOverrideParam;
				m_fxSpawnPosition = positionOverrideParam.m_positionOverride;
			}
			if (!(extraSequenceParams is FxAttributeParam))
			{
				continue;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			FxAttributeParam fxAttributeParam = extraSequenceParams as FxAttributeParam;
			if (fxAttributeParam == null)
			{
				continue;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (fxAttributeParam.m_paramNameCode == FxAttributeParam.ParamNameCode.None)
			{
				continue;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			string attributeName = fxAttributeParam.GetAttributeName();
			float paramValue = fxAttributeParam.m_paramValue;
			if (fxAttributeParam.m_paramTarget != FxAttributeParam.ParamTarget.MainVfx)
			{
				continue;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!m_fxAttributes.ContainsKey(attributeName))
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
				m_fxAttributes.Add(attributeName, paramValue);
			}
		}
		if (!(m_additionalFxAtTargetPos != null))
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
			m_additionalFxAtTargetPos.Initialize(this);
			return;
		}
	}

	public override void FinishSetup()
	{
		if (!(m_startEvent == null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!m_ignoreStartEvent)
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
				if (!ClientGameManager.Get().IsFastForward)
				{
					return;
				}
			}
		}
		if (!m_phaseTimingParameters.ShouldSequenceBeActive())
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
			if (m_startDelayTime <= 0f)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						SpawnFX();
						return;
					}
				}
			}
			m_timeToSpawnVfx = GameTime.time + m_startDelayTime;
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
	}

	internal override void SetTimerController(int value)
	{
		if (m_fx != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (value != m_timeControllerValueNow)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								m_timeControllerValueNow = value;
								if (value >= 4)
								{
									Sequence.SetAttribute(m_fx, "timerControl01", 0);
									Sequence.SetAttribute(m_fx, "timerControl02", 0);
									Sequence.SetAttribute(m_fx, "timerControl03", 0);
									Sequence.SetAttribute(m_fx, "timerControl04", 0);
								}
								else
								{
									if (value == 3)
									{
										while (true)
										{
											switch (4)
											{
											case 0:
												break;
											default:
												Sequence.SetAttribute(m_fx, "timerControl01", 0);
												Sequence.SetAttribute(m_fx, "timerControl02", 0);
												Sequence.SetAttribute(m_fx, "timerControl03", 0);
												Sequence.SetAttribute(m_fx, "timerControl04", 1);
												return;
											}
										}
									}
									if (value == 2)
									{
										Sequence.SetAttribute(m_fx, "timerControl01", 0);
										Sequence.SetAttribute(m_fx, "timerControl02", 0);
										Sequence.SetAttribute(m_fx, "timerControl03", 1);
										Sequence.SetAttribute(m_fx, "timerControl04", 1);
									}
									else if (value == 1)
									{
										Sequence.SetAttribute(m_fx, "timerControl01", 0);
										Sequence.SetAttribute(m_fx, "timerControl02", 1);
										Sequence.SetAttribute(m_fx, "timerControl03", 1);
										Sequence.SetAttribute(m_fx, "timerControl04", 1);
									}
									else if (value <= 0)
									{
										while (true)
										{
											switch (1)
											{
											case 0:
												break;
											default:
												Sequence.SetAttribute(m_fx, "timerControl01", 1);
												Sequence.SetAttribute(m_fx, "timerControl02", 1);
												Sequence.SetAttribute(m_fx, "timerControl03", 1);
												Sequence.SetAttribute(m_fx, "timerControl04", 1);
												return;
											}
										}
									}
								}
								return;
							}
						}
					}
					return;
				}
			}
		}
		m_initialTimerControllerValue = value;
	}

	private void SpawnFX()
	{
		if ((bool)m_fxPrefab)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Vector3 fxSpawnPosition = m_fxSpawnPosition;
			if (m_useGroundHeight)
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
				fxSpawnPosition.y = Board.Get().BaselineHeight;
			}
			fxSpawnPosition.y += m_yOffset;
			Quaternion targetRotation = base.TargetRotation;
			m_fx = InstantiateFX(m_fxPrefab, fxSpawnPosition, targetRotation);
			SetTimerController(m_initialTimerControllerValue);
			m_fxFoFSelectComp = m_fx.GetComponent<FriendlyEnemyVFXSelector>();
			if (m_fxFoFSelectComp != null)
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
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					m_fxFoFSelectComp.Setup(base.Caster.GetTeam());
				}
			}
			if (!m_sequenceHitCalled && m_callOnHitForGameplay)
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
				if (m_hitDelayTime > 0f && m_timeToHit < 0f)
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
					m_timeToHit = GameTime.time + m_hitDelayTime;
				}
				else if (m_hitDelayTime <= 0f)
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
					CallHitSequenceOnTargets(base.TargetPos);
					m_sequenceHitCalled = true;
				}
			}
			if (m_fx != null && m_fxAttributes != null)
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
				using (Dictionary<string, float>.Enumerator enumerator = m_fxAttributes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, float> current = enumerator.Current;
						Sequence.SetAttribute(m_fx, current.Key, current.Value);
					}
					while (true)
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
			if (m_fx != null && m_additionalFxAtTargetPos != null)
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
				m_additionalFxAtTargetPos.SpawnFX(m_fx.transform.position, m_fx.transform.rotation, this);
			}
		}
		if (string.IsNullOrEmpty(m_audioEvent))
		{
			return;
		}
		GameObject gameObject = null;
		if (m_fx != null)
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
			gameObject = m_fx;
		}
		else if (base.Caster != null)
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
			gameObject = base.Caster.gameObject;
		}
		if (gameObject != null)
		{
			AudioManager.PostEvent(m_audioEvent, gameObject);
		}
	}

	private void StopFX()
	{
		if (m_fx != null)
		{
			m_fx.SetActive(false);
		}
		if (!m_additionalFxAtTargetPos)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_additionalFxAtTargetPos.SetAsInactive();
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
			if (m_phaseTimingParameters.ShouldSequenceBeActive())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						{
							if (m_timeToSpawnVfx > 0f)
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
								if (GameTime.time >= m_timeToSpawnVfx)
								{
									m_timeToSpawnVfx = -1f;
									SpawnFX();
								}
							}
							if (m_callOnHitForGameplay)
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
								if (!m_sequenceHitCalled)
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
									if (m_initialized)
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
										if (m_fxPrefab == null)
										{
											goto IL_00f7;
										}
										while (true)
										{
											switch (4)
											{
											case 0:
												continue;
											}
											break;
										}
										if (m_timeToHit > 0f)
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
											if (GameTime.time >= m_timeToHit)
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
												goto IL_00f7;
											}
										}
									}
								}
							}
							goto IL_0113;
						}
						IL_00f7:
						CallHitSequenceOnTargets(base.TargetPos);
						m_sequenceHitCalled = true;
						goto IL_0113;
						IL_0113:
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
							if (m_fxFoFSelectComp != null)
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
								if (base.Caster != null)
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
									m_fxFoFSelectComp.Setup(base.Caster.GetTeam());
								}
							}
						}
						ProcessSequenceVisibility();
						if (m_additionalFxAtTargetPos != null)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									m_additionalFxAtTargetPos.OnUpdate(LastDesiredVisible(), base.Caster);
									return;
								}
							}
						}
						return;
					}
				}
			}
			SetSequenceVisibility(false);
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
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_startEvent == parameter)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						SpawnFX();
						return;
					}
				}
			}
			if (m_stopEvent == parameter)
			{
				StopFX();
			}
			return;
		}
	}

	private void OnDisable()
	{
		if ((bool)m_fx)
		{
			Object.Destroy(m_fx.gameObject);
			m_fx = null;
		}
		if (!(m_additionalFxAtTargetPos != null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_additionalFxAtTargetPos.DestroyFX();
			return;
		}
	}

	public override string GetVisibilityDescription()
	{
		return string.Empty;
	}

	public override string GetSequenceSpecificDescription()
	{
		string str = string.Empty;
		if (m_fxPrefab == null)
		{
			str += "<color=yellow>WARNING: </color>No VFX Prefab for field [Fx Prefab]\n\n";
		}
		if (m_callOnHitForGameplay)
		{
			str += "<color=cyan>Can do Gameplay Hits</color>\n";
			if (m_hitDelayTime > 0f)
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
				string text = str;
				str = text + "Gameplay Hit happens " + m_hitDelayTime + " second(s) after VFX start.\n\n";
			}
		}
		else
		{
			str += "Ignoring Gameplay Hits\n";
		}
		if (m_startEvent != null)
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
			if (m_startDelayTime > 0f)
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
				str += "<color=yellow>WARNING: </color>Start Delay Time is ignored, will use StartEvent.\n\n";
			}
		}
		else if (m_startDelayTime > 0f)
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
			string text = str;
			str = text + "Starts " + m_startDelayTime + " second(s) after sequence spawn.";
		}
		return str;
	}
}
