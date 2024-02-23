using System.Collections.Generic;
using System.Text;
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

	[Separator("FX To Spawn")]
	public GameObject m_fxPrefab;
	[Separator("Height")]
	public float m_yOffset;
	public bool m_useGroundHeight;
	private GameObject m_fx;
	private FriendlyEnemyVFXSelector m_fxFoFSelectComp;
	[Separator("Start delay time (ignored if has Start Event)")]
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
	[Separator("Gameplay Hits")]
	public bool m_callOnHitForGameplay;
	public float m_hitDelayTime;
	private float m_timeToHit = -1f;
	private bool m_sequenceHitCalled;
	[Separator("Audio Event -- ( on FX spawn )", "orange")]
	[AudioEvent(false)]
	public string m_audioEvent;
	[Separator("Phase-Based Timing")]
	public PhaseTimingParameters m_phaseTimingParameters;
	[Separator("Special case handler for additional VFX at target position")]
	public AdditionalVfxContainerBase m_additionalFxAtTargetPos;

	private int m_initialTimerControllerValue;
	private int m_timeControllerValueNow = -100;
	private Dictionary<string, float> m_fxAttributes = new Dictionary<string, float>();
	private Vector3 m_fxSpawnPosition;

	internal override Vector3 GetSequencePos()
	{
		return m_fx != null
			? m_fx.transform.position
			: Vector3.zero;
	}

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		m_fxSpawnPosition = TargetPos;
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			OverridePhaseTimingParams(m_phaseTimingParameters, extraSequenceParams);
			if (extraSequenceParams is IgnoreStartEventExtraParam ignoreStartEventExtraParam)
			{
				m_ignoreStartEvent = ignoreStartEventExtraParam.ignoreStartEvent;
			}
			if (extraSequenceParams is PositionOverrideParam positionOverrideParam)
			{
				m_fxSpawnPosition = positionOverrideParam.m_positionOverride;
			}
			if (extraSequenceParams is FxAttributeParam fxAttributeParam
			    && fxAttributeParam.m_paramNameCode != FxAttributeParam.ParamNameCode.None)
			{
				string attributeName = fxAttributeParam.GetAttributeName();
				float paramValue = fxAttributeParam.m_paramValue;
				if (fxAttributeParam.m_paramTarget == FxAttributeParam.ParamTarget.MainVfx
				    && !m_fxAttributes.ContainsKey(attributeName))
				{
					m_fxAttributes.Add(attributeName, paramValue);
				}
			}
		}
		if (m_additionalFxAtTargetPos != null)
		{
			m_additionalFxAtTargetPos.Initialize(this);
		}
	}

	public override void FinishSetup()
	{
		if (m_startEvent != null
		    && !m_ignoreStartEvent
		    && !ClientGameManager.Get().IsFastForward)
		{
			return;
		}
		if (!m_phaseTimingParameters.ShouldSequenceBeActive())
		{
			return;
		}
		if (m_startDelayTime <= 0f)
		{
			SpawnFX();
		}
		else
		{
			m_timeToSpawnVfx = GameTime.time + m_startDelayTime;
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
		if (m_fx == null)
		{
			m_initialTimerControllerValue = value;
			return;
		}
		if (value == m_timeControllerValueNow)
		{
			return;
		}
		m_timeControllerValueNow = value;
		if (value >= 4)
		{
			SetAttribute(m_fx, "timerControl01", 0);
			SetAttribute(m_fx, "timerControl02", 0);
			SetAttribute(m_fx, "timerControl03", 0);
			SetAttribute(m_fx, "timerControl04", 0);
		}
		else if (value == 3)
		{
			SetAttribute(m_fx, "timerControl01", 0);
			SetAttribute(m_fx, "timerControl02", 0);
			SetAttribute(m_fx, "timerControl03", 0);
			SetAttribute(m_fx, "timerControl04", 1);
		}
		else if (value == 2)
		{
			SetAttribute(m_fx, "timerControl01", 0);
			SetAttribute(m_fx, "timerControl02", 0);
			SetAttribute(m_fx, "timerControl03", 1);
			SetAttribute(m_fx, "timerControl04", 1);
		}
		else if (value == 1)
		{
			SetAttribute(m_fx, "timerControl01", 0);
			SetAttribute(m_fx, "timerControl02", 1);
			SetAttribute(m_fx, "timerControl03", 1);
			SetAttribute(m_fx, "timerControl04", 1);
		}
		else if (value <= 0)
		{
			SetAttribute(m_fx, "timerControl01", 1);
			SetAttribute(m_fx, "timerControl02", 1);
			SetAttribute(m_fx, "timerControl03", 1);
			SetAttribute(m_fx, "timerControl04", 1);
		}
	}

	private void SpawnFX()
	{
		if (m_fxPrefab != null)
		{
			Vector3 fxSpawnPosition = m_fxSpawnPosition;
			if (m_useGroundHeight)
			{
				fxSpawnPosition.y = Board.Get().BaselineHeight;
			}
			fxSpawnPosition.y += m_yOffset;
			Quaternion targetRotation = TargetRotation;
			m_fx = InstantiateFX(m_fxPrefab, fxSpawnPosition, targetRotation);
			SetTimerController(m_initialTimerControllerValue);
			m_fxFoFSelectComp = m_fx.GetComponent<FriendlyEnemyVFXSelector>();
			if (m_fxFoFSelectComp != null && Caster != null)
			{
				m_fxFoFSelectComp.Setup(Caster.GetTeam());
			}
			if (!m_sequenceHitCalled && m_callOnHitForGameplay)
			{
				if (m_hitDelayTime > 0f && m_timeToHit < 0f)
				{
					m_timeToHit = GameTime.time + m_hitDelayTime;
				}
				else if (m_hitDelayTime <= 0f)
				{
					CallHitSequenceOnTargets(TargetPos);
					m_sequenceHitCalled = true;
				}
			}
			if (m_fx != null && m_fxAttributes != null)
			{
				foreach (KeyValuePair<string, float> fxAttribute in m_fxAttributes)
				{
					SetAttribute(m_fx, fxAttribute.Key, fxAttribute.Value);
				}
			}
			if (m_fx != null && m_additionalFxAtTargetPos != null)
			{
				m_additionalFxAtTargetPos.SpawnFX(m_fx.transform.position, m_fx.transform.rotation, this);
			}
		}
		if (!string.IsNullOrEmpty(m_audioEvent))
		{
			GameObject eventParentGameObject = null;
			if (m_fx != null)
			{
				eventParentGameObject = m_fx;
			}
			else if (Caster != null)
			{
				eventParentGameObject = Caster.gameObject;
			}

			if (eventParentGameObject != null)
			{
				AudioManager.PostEvent(m_audioEvent, eventParentGameObject);
			}
		}
	}

	private void StopFX()
	{
		if (m_fx != null)
		{
			m_fx.SetActive(false);
		}
		if (m_additionalFxAtTargetPos != null)
		{
			m_additionalFxAtTargetPos.SetAsInactive();
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

		if (!m_phaseTimingParameters.ShouldSequenceBeActive())
		{
			SetSequenceVisibility(false);
			return;
		}
		
		if (m_timeToSpawnVfx > 0f && GameTime.time >= m_timeToSpawnVfx)
		{
			m_timeToSpawnVfx = -1f;
			SpawnFX();
		}
		
		if (m_callOnHitForGameplay
		    && !m_sequenceHitCalled
		    && m_initialized
		    && (m_fxPrefab == null || (m_timeToHit > 0f && GameTime.time >= m_timeToHit)))
		{
			CallHitSequenceOnTargets(TargetPos);
			m_sequenceHitCalled = true;
		}

		if (m_fx != null
		    && m_fxFoFSelectComp != null
		    && Caster != null)
		{
			m_fxFoFSelectComp.Setup(Caster.GetTeam());
		}

		ProcessSequenceVisibility();
		if (m_additionalFxAtTargetPos != null)
		{
			m_additionalFxAtTargetPos.OnUpdate(LastDesiredVisible(), Caster);
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
		else if (m_stopEvent == parameter)
		{
			StopFX();
		}
	}

	private void OnDisable()
	{
		if (m_fx != null)
		{
			Destroy(m_fx.gameObject);
			m_fx = null;
		}
		if (m_additionalFxAtTargetPos != null)
		{
			m_additionalFxAtTargetPos.DestroyFX();
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
				str += new StringBuilder().Append("Gameplay Hit happens ").Append(m_hitDelayTime).Append(" second(s) after VFX start.\n\n").ToString();
			}
		}
		else
		{
			str += "Ignoring Gameplay Hits\n";
		}
		if (m_startEvent != null)
		{
			if (m_startDelayTime > 0f)
			{
				str += "<color=yellow>WARNING: </color>Start Delay Time is ignored, will use StartEvent.\n\n";
			}
		}
		else if (m_startDelayTime > 0f)
		{
			str += new StringBuilder().Append("Starts ").Append(m_startDelayTime).Append(" second(s) after sequence spawn.").ToString();
		}
		return str;
	}
}
