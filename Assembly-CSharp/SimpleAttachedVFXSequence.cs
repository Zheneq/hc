using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SimpleAttachedVFXSequence : Sequence
{
	public class MultiEventExtraParams : IExtraSequenceParams
	{
		public int eventNumberToKeyOffOf;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			short value = (short)eventNumberToKeyOffOf;
			stream.Serialize(ref value);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			short value = 0;
			stream.Serialize(ref value);
			eventNumberToKeyOffOf = value;
		}
	}

	public class ImpactDelayParams : IExtraSequenceParams
	{
		public float impactDelayTime = -1f;

		public sbyte alternativeImpactAudioIndex = -1;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref impactDelayTime);
			stream.Serialize(ref alternativeImpactAudioIndex);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref impactDelayTime);
			stream.Serialize(ref alternativeImpactAudioIndex);
		}
	}

	public class DelayedImpact
	{
		public float m_timeToSpawnImpact;

		public bool m_lastHit;

		public DelayedImpact(float timeToSpawn, bool lastHit)
		{
			m_timeToSpawnImpact = timeToSpawn;
			m_lastHit = lastHit;
		}
	}

	public enum AudioEventType
	{
		General,
		Pickup
	}

	[Serializable]
	public class HitVFXStatusFilters
	{
		public enum FilterCond
		{
			HasStatus,
			DoesntHaveStatus
		}

		public FilterCond m_condition;

		public StatusType m_status = StatusType.INVALID;
	}

	[Separator("Main FX Prefab", true)]
	public GameObject m_fxPrefab;

	[JointPopup("FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_fxJoint;

	public ReferenceModelType m_jointReferenceType;

	[Tooltip("Check if Fx Prefab should stay attached to the joint. If unchecked, the Fx Prefab will start with the joint position and rotation.")]
	public bool m_fxAttachToJoint;

	[Separator("Anim Event -- ( main FX start / stop )", "orange")]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[AnimEventPicker]
	public UnityEngine.Object m_startEvent;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before stopping the sequence. Search project for EventObjects.")]
	public UnityEngine.Object m_stopEvent;

	[Separator("Rotation/Alignment", true)]
	[Tooltip("Aim the Fx Prefab at the target (character or mouse click). If unchecked, inherits the attach joint transformation.")]
	public bool m_aimAtTarget = true;

	public bool m_useRootOrientation;

	[Separator("Audio Event -- ( on main FX spawn )", "orange")]
	[AudioEvent(false)]
	public string m_audioEvent;

	[Separator("Hit FX (on Targets)", true)]
	public bool m_playHitReactsWithoutFx;

	[Tooltip("FX at point(s) of impact")]
	public GameObject m_hitFxPrefab;

	[JointPopup("hit FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_hitFxJoint;

	public bool m_hitFxAttachToJoint;

	[Space(10f)]
	public CameraManager.CameraShakeIntensity m_hitCameraShakeType = CameraManager.CameraShakeIntensity.None;

	[Tooltip("Delay after Start Event before creating Hit Fx Prefab")]
	public float m_hitDelay;

	[Header("-- Orient hit fx to point from caster to target?")]
	public bool m_hitAlignedWithCaster;

	[Header("-- If orienting hit fx, whether to reverse direction (target to caster)")]
	public bool m_hitFxReverseAlignDir;

	[AnimEventPicker]
	[Separator("Anim Events -- ( hit timing )", "orange")]
	public UnityEngine.Object m_hitEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_lastHitEvent;

	[Tooltip("Amount of time to trigger actual impact after hit react event has been received")]
	public float m_hitImpactDelayTime = -1f;

	[Separator("Audio Event -- ( played per target hit by default )", "orange")]
	[AudioEvent(false)]
	public string m_hitAudioEvent;

	public AudioEventType m_hitAudioEventType;

	[Header("-- Alternative Impact Audio Events, handled per ability, unused otherwise")]
	[AudioEvent(false)]
	public string[] m_alternativeImpactAudioEvents;

	[Separator("Team restrictions for Hit FX on Targets", true)]
	public HitVFXSpawnTeam m_hitVfxSpawnTeamMode;

	public List<HitVFXStatusFilters> m_hitVfxStatusRequirements = new List<HitVFXStatusFilters>();

	[Separator("Phase-Based Timing", true)]
	public PhaseTimingParameters m_phaseTimingParameters;

	protected GameObject m_fx;

	protected FriendlyEnemyVFXSelector m_mainFxFoFSelector;

	private List<GameObject> m_hitFx;

	private List<ActorData> m_hitFxAttachedActors = new List<ActorData>();

	private Dictionary<string, float> m_fxAttributes = new Dictionary<string, float>();

	private float m_hitSpawnTime = -1f;

	private bool m_playedHitReact;

	private bool m_spawnAttempted;

	private int m_eventNumberToKeyOffOf = -1;

	private int m_numStartEventsReceived;

	private List<DelayedImpact> m_delayedImpacts = new List<DelayedImpact>();

	private int m_alternativeAudioIndex = -1;

	private bool Finished()
	{
		bool result = false;
		if (!(GetFxPrefab() == null))
		{
			if (!AreFXFinished(m_fx))
			{
				goto IL_0103;
			}
		}
		if (m_hitFxPrefab == null)
		{
			if (m_playHitReactsWithoutFx && m_playedHitReact)
			{
				result = true;
			}
		}
		else if (m_hitFx != null)
		{
			result = true;
			using (List<GameObject>.Enumerator enumerator = m_hitFx.GetEnumerator())
			{
				while (true)
				{
					if (!enumerator.MoveNext())
					{
						break;
					}
					GameObject current = enumerator.Current;
					if (current != null)
					{
						if (current.activeSelf)
						{
							result = false;
							break;
						}
					}
				}
			}
		}
		if (m_delayedImpacts.Count > 0)
		{
			result = false;
		}
		goto IL_0103;
		IL_0103:
		return result;
	}

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			MultiEventExtraParams multiEventExtraParams = extraSequenceParams as MultiEventExtraParams;
			if (multiEventExtraParams != null)
			{
				m_eventNumberToKeyOffOf = multiEventExtraParams.eventNumberToKeyOffOf;
			}
			ImpactDelayParams impactDelayParams = extraSequenceParams as ImpactDelayParams;
			if (impactDelayParams != null)
			{
				if (impactDelayParams.impactDelayTime > 0f)
				{
					m_hitImpactDelayTime = impactDelayParams.impactDelayTime;
				}
				if (impactDelayParams.alternativeImpactAudioIndex >= 0)
				{
					m_alternativeAudioIndex = impactDelayParams.alternativeImpactAudioIndex;
				}
			}
			if (!(extraSequenceParams is FxAttributeParam))
			{
				continue;
			}
			FxAttributeParam fxAttributeParam = extraSequenceParams as FxAttributeParam;
			if (fxAttributeParam == null || fxAttributeParam.m_paramNameCode == FxAttributeParam.ParamNameCode.None)
			{
				continue;
			}
			string attributeName = fxAttributeParam.GetAttributeName();
			float paramValue = fxAttributeParam.m_paramValue;
			if (fxAttributeParam.m_paramTarget != FxAttributeParam.ParamTarget.MainVfx)
			{
				continue;
			}
			if (!m_fxAttributes.ContainsKey(attributeName))
			{
				m_fxAttributes.Add(attributeName, paramValue);
			}
		}
		while (true)
		{
			switch (6)
			{
			default:
				return;
			case 0:
				break;
			}
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
			if (!m_spawnAttempted)
			{
				if (m_phaseTimingParameters.ShouldSpawnSequence(abilityPhase))
				{
					if (m_phaseTimingParameters.ShouldSequenceBeActive())
					{
						SpawnFX();
					}
				}
			}
		}
		if (!m_phaseTimingParameters.ShouldStopSequence(abilityPhase))
		{
			return;
		}
		while (true)
		{
			if (m_fx != null)
			{
				StopFX();
			}
			return;
		}
	}

	internal override Vector3 GetSequencePos()
	{
		if (m_fx != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_fx.transform.position;
				}
			}
		}
		return Vector3.zero;
	}

	public override void FinishSetup()
	{
		if (!(m_startEvent == null))
		{
			return;
		}
		while (true)
		{
			SpawnFX();
			return;
		}
	}

	private bool IsHitFXVisibleForActor(ActorData hitTarget)
	{
		bool flag = IsHitFXVisibleWrtTeamFilter(hitTarget, m_hitVfxSpawnTeamMode);
		if (flag)
		{
			if (m_hitVfxStatusRequirements != null)
			{
				if (m_hitVfxStatusRequirements.Count > 0)
				{
					for (int i = 0; i < m_hitVfxStatusRequirements.Count && flag; i++)
					{
						HitVFXStatusFilters hitVFXStatusFilters = m_hitVfxStatusRequirements[i];
						if (hitVFXStatusFilters.m_status == StatusType.INVALID)
						{
							continue;
						}
						bool flag2 = hitTarget.GetActorStatus().HasStatus(hitVFXStatusFilters.m_status);
						if (hitVFXStatusFilters.m_condition == HitVFXStatusFilters.FilterCond.HasStatus)
						{
							if (!flag2)
							{
								goto IL_00b7;
							}
						}
						if (hitVFXStatusFilters.m_condition != HitVFXStatusFilters.FilterCond.DoesntHaveStatus)
						{
							continue;
						}
						if (!flag2)
						{
							continue;
						}
						goto IL_00b7;
						IL_00b7:
						flag = false;
					}
				}
			}
		}
		return flag;
	}

	protected virtual void SetFxRotation()
	{
		if (!(m_fx != null) || !m_useRootOrientation || !(base.Caster != null))
		{
			return;
		}
		while (true)
		{
			m_fx.transform.rotation = base.Caster.transform.rotation;
			return;
		}
	}

	protected virtual GameObject GetFxPrefab()
	{
		return m_fxPrefab;
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
			if (m_fx != null)
			{
				if (m_fxAttachToJoint)
				{
					if (m_jointReferenceType == ReferenceModelType.Actor)
					{
						if (ShouldHideForActorIfAttached(base.Caster))
						{
							SetSequenceVisibility(false);
							goto IL_0083;
						}
					}
				}
			}
			ProcessSequenceVisibility();
			goto IL_0083;
			IL_0083:
			if (m_mainFxFoFSelector != null && base.Caster != null)
			{
				m_mainFxFoFSelector.Setup(base.Caster.GetTeam());
			}
			SetFxRotation();
			if (m_hitSpawnTime > 0f && GameTime.time > m_hitSpawnTime)
			{
				if (m_hitImpactDelayTime > 0f)
				{
					m_delayedImpacts.Add(new DelayedImpact(GameTime.time + m_hitImpactDelayTime, true));
				}
				else
				{
					SpawnHitFX(true);
				}
				m_hitSpawnTime = -1f;
			}
			for (int num = m_delayedImpacts.Count - 1; num >= 0; num--)
			{
				DelayedImpact delayedImpact = m_delayedImpacts[num];
				if (GameTime.time >= delayedImpact.m_timeToSpawnImpact)
				{
					SpawnHitFX(delayedImpact.m_lastHit);
					m_delayedImpacts.RemoveAt(num);
				}
			}
			while (true)
			{
				if (m_hitFx != null)
				{
					if (m_hitFx.Count > 0)
					{
						if (base.Caster != null)
						{
							if (m_hitFxAttachToJoint)
							{
								for (int i = 0; i < m_hitFx.Count; i++)
								{
									GameObject gameObject = m_hitFx[i];
									if (!(gameObject != null))
									{
										continue;
									}
									if (m_hitAlignedWithCaster)
									{
										Vector3 forward = gameObject.transform.position - base.Caster.GetFreePos();
										forward.y = 0f;
										if (forward.magnitude > 1E-05f)
										{
											forward.Normalize();
											if (m_hitFxReverseAlignDir)
											{
												forward *= -1f;
											}
											Quaternion rotation = Quaternion.LookRotation(forward);
											gameObject.transform.rotation = rotation;
										}
									}
									if (i >= m_hitFxAttachedActors.Count)
									{
										continue;
									}
									ActorData actorData = m_hitFxAttachedActors[i];
									if (actorData != null)
									{
										bool desiredActive = IsActorConsideredVisible(actorData);
										gameObject.SetActiveIfNeeded(desiredActive);
									}
								}
							}
						}
					}
				}
				if (!Finished())
				{
					return;
				}
				while (true)
				{
					if (!(base.Source != null))
					{
						return;
					}
					while (true)
					{
						if (!base.Source.RemoveAtEndOfTurn)
						{
							while (true)
							{
								MarkForRemoval();
								return;
							}
						}
						return;
					}
				}
			}
		}
	}

	protected void StopFX()
	{
		if (!(m_fx != null))
		{
			return;
		}
		while (true)
		{
			m_fx.SetActive(false);
			return;
		}
	}

	private void SpawnHitFX(bool lastHit)
	{
		m_playedHitReact = true;
		if (m_hitFx == null)
		{
			m_hitFx = new List<GameObject>();
		}
		if (base.Targets != null)
		{
			if (base.Targets.Length > 0)
			{
				CameraManager.Get().PlayCameraShake(m_hitCameraShakeType);
			}
			int num = 0;
			while (num < base.Targets.Length)
			{
				object obj;
				if (num < base.Targets.Length)
				{
					obj = base.Targets[num];
				}
				else
				{
					obj = null;
				}
				ActorData actorData = (ActorData)obj;
				Vector3 targetHitPosition = GetTargetHitPosition(num, m_hitFxJoint);
				Vector3 position = base.Caster.transform.position;
				if ((position - base.Targets[num].transform.position).magnitude < 0.1f)
				{
					position -= base.Caster.transform.forward * 0.5f;
				}
				Vector3 vector = targetHitPosition - position;
				vector.y = 0f;
				vector.Normalize();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, vector);
				Quaternion quaternion;
				if (m_hitAlignedWithCaster)
				{
					Vector3 forward;
					if (m_hitFxReverseAlignDir)
					{
						forward = -1f * vector;
					}
					else
					{
						forward = vector;
					}
					quaternion = Quaternion.LookRotation(forward);
				}
				else
				{
					quaternion = Quaternion.identity;
				}
				Quaternion rotation = quaternion;
				bool flag = IsHitFXVisibleForActor(base.Targets[num]);
				GameObject gameObject;
				if ((bool)m_hitFxPrefab)
				{
					if (flag)
					{
						gameObject = InstantiateFX(m_hitFxPrefab, targetHitPosition, rotation);
						if (m_hitFxAttachToJoint)
						{
							if (actorData != null)
							{
								m_hitFxJoint.Initialize(actorData.gameObject);
								gameObject.transform.parent = m_hitFxJoint.m_jointObject.transform;
								gameObject.transform.localPosition = Vector3.zero;
								gameObject.transform.localRotation = Quaternion.identity;
								goto IL_0274;
							}
						}
						gameObject.transform.parent = base.transform;
						goto IL_0274;
					}
				}
				goto IL_02cb;
				IL_02cb:
				if (flag)
				{
					string text = m_hitAudioEvent;
					if (m_alternativeAudioIndex >= 0)
					{
						if (m_alternativeAudioIndex < m_alternativeImpactAudioEvents.Length)
						{
							text = m_alternativeImpactAudioEvents[m_alternativeAudioIndex];
						}
					}
					if (m_hitAudioEventType == AudioEventType.Pickup && !AudioManager.s_pickupAudio)
					{
					}
					else if (!string.IsNullOrEmpty(text))
					{
						AudioManager.PostEvent(text, base.Targets[num].gameObject);
					}
				}
				if (base.Targets[num] != null)
				{
					if (!lastHit)
					{
						base.Source.OnSequenceHit(this, base.Targets[num], impulseInfo, ActorModelData.RagdollActivation.None);
					}
					else
					{
						base.Source.OnSequenceHit(this, base.Targets[num], impulseInfo);
					}
				}
				num++;
				continue;
				IL_0274:
				FriendlyEnemyVFXSelector component = gameObject.GetComponent<FriendlyEnemyVFXSelector>();
				if (component != null)
				{
					component.Setup(base.Caster.GetTeam());
				}
				m_hitFx.Add(gameObject);
				m_hitFxAttachedActors.Add(base.Targets[num]);
				goto IL_02cb;
			}
		}
		base.Source.OnSequenceHit(this, base.TargetPos);
	}

	protected void SpawnFX(GameObject overrideFxPrefab = null)
	{
		m_spawnAttempted = true;
		if (!m_fxJoint.IsInitialized())
		{
			GameObject referenceModel = GetReferenceModel(base.Caster, m_jointReferenceType);
			if (referenceModel != null)
			{
				m_fxJoint.Initialize(referenceModel);
			}
		}
		GameObject gameObject = overrideFxPrefab;
		if (gameObject == null)
		{
			gameObject = GetFxPrefab();
		}
		Vector3 zero;
		if (gameObject != null)
		{
			zero = Vector3.zero;
			if (m_fxJoint.m_jointObject != null && m_fxJoint.m_jointObject.transform.localScale != Vector3.zero)
			{
				if (m_fxAttachToJoint)
				{
					m_fx = InstantiateFX(gameObject);
					AttachToBone(m_fx, m_fxJoint.m_jointObject);
					m_fx.transform.localPosition = Vector3.zero;
					m_fx.transform.localRotation = Quaternion.identity;
					zero = m_fxJoint.m_jointObject.transform.position;
					goto IL_0253;
				}
			}
			Vector3 vector = default(Vector3);
			if (m_fxJoint.m_jointObject != null)
			{
				vector = m_fxJoint.m_jointObject.transform.position;
			}
			else
			{
				vector = base.transform.position;
			}
			Quaternion rotation = default(Quaternion);
			if (!m_aimAtTarget)
			{
				rotation = ((!m_fxJoint.m_jointObject) ? base.transform.rotation : m_fxJoint.m_jointObject.transform.rotation);
			}
			else
			{
				Vector3 targetPosition = GetTargetPosition(0);
				Vector3 lookRotation = targetPosition - vector;
				lookRotation.y = 0f;
				lookRotation.Normalize();
				rotation.SetLookRotation(lookRotation);
			}
			m_fx = InstantiateFX(gameObject, vector, rotation);
			zero = vector;
			goto IL_0253;
		}
		goto IL_02df;
		IL_0253:
		Sequence.SetAttribute(m_fx, "abilityAreaLength", (base.TargetPos - zero).magnitude);
		if (m_fx != null)
		{
			m_mainFxFoFSelector = m_fx.GetComponent<FriendlyEnemyVFXSelector>();
			if (m_mainFxFoFSelector != null)
			{
				if (base.Caster != null)
				{
					m_mainFxFoFSelector.Setup(base.Caster.GetTeam());
				}
			}
		}
		goto IL_02df;
		IL_02df:
		if (m_hitEvent == null)
		{
			if (m_playHitReactsWithoutFx)
			{
				if (m_hitDelay > 0f)
				{
					m_hitSpawnTime = GameTime.time + m_hitDelay;
				}
				else
				{
					SpawnHitFX(true);
				}
			}
		}
		if (m_fx != null)
		{
			if (m_fxAttributes != null)
			{
				using (Dictionary<string, float>.Enumerator enumerator = m_fxAttributes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, float> current = enumerator.Current;
						Sequence.SetAttribute(m_fx, current.Key, current.Value);
					}
				}
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

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (m_startEvent == parameter)
		{
			if (m_eventNumberToKeyOffOf >= 0)
			{
				if (m_numStartEventsReceived != m_eventNumberToKeyOffOf)
				{
					m_numStartEventsReceived++;
					goto IL_00a9;
				}
			}
			if (m_eventNumberToKeyOffOf >= 0)
			{
				m_numStartEventsReceived++;
			}
			SpawnFX();
		}
		else if (m_stopEvent == parameter)
		{
			if (m_spawnAttempted)
			{
				StopFX();
			}
		}
		goto IL_00a9;
		IL_00a9:
		if (m_hitEvent == parameter)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (m_hitImpactDelayTime > 0f)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								m_delayedImpacts.Add(new DelayedImpact(GameTime.time + m_hitImpactDelayTime, m_lastHitEvent == null));
								return;
							}
						}
					}
					SpawnHitFX(m_lastHitEvent == null);
					return;
				}
			}
		}
		if (!(m_lastHitEvent == parameter))
		{
			return;
		}
		while (true)
		{
			if (m_hitImpactDelayTime > 0f)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						m_delayedImpacts.Add(new DelayedImpact(GameTime.time + m_hitImpactDelayTime, true));
						return;
					}
				}
			}
			SpawnHitFX(true);
			return;
		}
	}

	private void OnDisable()
	{
		if (m_fx != null)
		{
			m_mainFxFoFSelector = null;
			UnityEngine.Object.Destroy(m_fx.gameObject);
			m_fx = null;
		}
		if (m_hitFx == null)
		{
			return;
		}
		while (true)
		{
			using (List<GameObject>.Enumerator enumerator = m_hitFx.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject current = enumerator.Current;
					UnityEngine.Object.Destroy(current.gameObject);
				}
			}
			m_hitFx = null;
			return;
		}
	}

	public override string GetSequenceSpecificDescription()
	{
		string str = string.Empty;
		if (m_fxPrefab == null)
		{
			str += "<color=yellow>WARNING: </color>No VFX Prefab for <FX Prefab>\n\n";
		}
		if (m_fxJoint != null && string.IsNullOrEmpty(m_fxJoint.m_joint))
		{
			str += "<color=yellow>WARNING: </color>VFX joint is empty, may not spawn at expected location.\n\n";
		}
		if (m_jointReferenceType != 0)
		{
			str = new StringBuilder().Append(str).Append("<Joint Reference Type> is <color=cyan>").Append(m_jointReferenceType.ToString()).Append("</color>\n\n").ToString();
		}
		if (m_fxJoint != null)
		{
			if (m_fxAttachToJoint)
			{
				str = new StringBuilder().Append(str).Append("<color=cyan>VFX is attaching to joint (").Append(m_fxJoint.m_joint).Append(")</color>\n").ToString();
				if (m_aimAtTarget)
				{
					str += "<[x] Aim At Target> ignored, attaching to joint\n";
				}
				str += "\n";
			}
			else
			{
				str = new StringBuilder().Append(str).Append("VFX spawning at joint (<color=cyan>").Append(m_fxJoint.m_joint).Append("</color>), not set to attach.\n\n").ToString();
			}
		}
		if (m_useRootOrientation)
		{
			str += "<[x] Use Root Orientaion> rotation is set to Caster's orientation per update\n\n";
		}
		if (!(m_hitEvent != null))
		{
			if (!m_playHitReactsWithoutFx)
			{
				str += "Ignoring Gameplay Hits\n";
				str += "(If need Gameplay Hits, check <[x] Play Hit React Without Fx> or add <Hit Event>)\n\n";
				goto IL_019a;
			}
		}
		str += "<color=cyan>Can do Gameplay Hits</color>\n";
		goto IL_019a;
		IL_019a:
		if (m_lastHitEvent != null)
		{
			str += "Has <Last Hit Event>, will not trigger ragdoll until that event is fired\n\n";
		}
		if (m_hitEvent != null)
		{
			if (m_hitDelay > 0f)
			{
				str += "<color=yellow>WARNING: </color>Has <Hit Event>, <Hit Delay> will be ignored\n\n";
				goto IL_0266;
			}
		}
		if (m_hitEvent == null)
		{
			if (m_hitDelay > 0f)
			{
				string text = str;
				str = new StringBuilder().Append(text).Append("Using <Hit Delay> for timing, Gameplay Hit and <Hit FX Prefab> will spawn ").Append(m_hitDelay).Append(" second(s) after VFX spawn\n\n").ToString();
			}
		}
		goto IL_0266;
		IL_0266:
		return str;
	}
}
