using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SenseiBideAttachedSequence : Sequence
{
	[Header("-- Whether switch between different levels after spawn --", order = 1)]
	[Separator("For Main Fx", order = 0)]
	public bool m_switchBetweenLevelsAfterSpawn = true;
	[Header("-- Vfx Prefabs, will use first as base, and rest used for subsequent levels --")]
	public List<GameObject> m_fxPrefabs;
	[Header("    For alternative version of base, if not advanced to higher levels will switch to this one")]
	public GameObject m_fxPrefabAfterFirstTurn;
	[JointPopup("Main FX attach joint")]
	public JointPopupProperty m_fxJoint;
	[Tooltip("Check if Fx Prefab should stay attached to the joint. If unchecked, the Fx Prefab will start with the joint position and rotation.")]
	public bool m_fxAttachToJoint;
	[AnimEventPicker]
	[Header("-- Anim Events --")]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public Object m_startEvent;
	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before stopping the sequence. Search project for EventObjects.")]
	public Object m_stopEvent;
	[Header("-- Spawn Delay (ignored if there is Start Event) --")]
	public float m_startDelayTime;

	private float m_timeToSpawnVfx = -1f;

	public bool m_useRootOrientation;
	[AudioEvent(false)]
	public string m_audioEvent;
	[Separator("For Impact Fx")]
	public GameObject m_hitFxPrefab;
	[JointPopup("hit FX attach joint")]
	public JointPopupProperty m_hitFxJoint;
	public bool m_hitAlignedWithCaster;
	[AnimEventPicker]
	public Object m_hitEvent;
	public float m_hitDelay;
	[Header("-- Team restrictions for Hit VFX on Targets --")]
	public HitVFXSpawnTeam m_hitVfxSpawnTeamMode;
	[AudioEvent(false)]
	public string m_hitAudioEvent;
	public PhaseTimingParameters m_phaseTimingParameters;

	private List<GameObject> m_fxInstances;
	private GameObject m_alternateBaseFxInstance;
	private List<GameObject> m_hitFxInstances;
	private float m_hitSpawnTime = -1f;
	private bool m_attemptedToSpawnHitFx;
	private Sensei_SyncComponent m_syncComp;
	private int m_lastActiveIndex = -1;

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		ActorData actorData = null;
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			OverridePhaseTimingParams(m_phaseTimingParameters, extraSequenceParams);
			ActorIndexExtraParam actorIndexExtraParam = extraSequenceParams as ActorIndexExtraParam;
			if (actorIndexExtraParam != null && GameFlowData.Get() != null)
			{
				int actorIndex = actorIndexExtraParam.m_actorIndex;
				actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
			}
		}
		if (actorData != null)
		{
			m_syncComp = actorData.GetComponent<Sensei_SyncComponent>();
		}
		else if (Application.isEditor)
		{
			Debug.LogError("Did not find Sensei for sensei ult sequence");
		}
	}

	public int GetCurrnetFxIndex()
	{
		if (m_syncComp == null || m_fxPrefabs.Count <= 1)
		{
			return 0;
		}
		int last = m_fxPrefabs.Count - 1;
		float syncBideExtraDamagePct = m_syncComp.m_syncBideExtraDamagePct;
		if (syncBideExtraDamagePct >= 0.99f)
		{
			return last;
		}
		float step = 1f / last;
		return Mathf.FloorToInt(syncBideExtraDamagePct / step);
	}

	public override void FinishSetup()
	{
		if (m_startEvent != null || !m_phaseTimingParameters.ShouldSequenceBeActive())
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

	private bool IsHitFXVisibleForActor(ActorData hitTarget)
	{
		return IsHitFXVisibleWrtTeamFilter(hitTarget, m_hitVfxSpawnTeamMode);
	}

	internal override void OnTurnStart(int currentTurn)
	{
		m_phaseTimingParameters.OnTurnStart(currentTurn);
	}

	internal override void OnAbilityPhaseStart(AbilityPriority abilityPhase)
	{
		m_phaseTimingParameters.OnAbilityPhaseStart(abilityPhase);
		if (m_startEvent == null
		    && m_phaseTimingParameters.ShouldSpawnSequence(abilityPhase)
		    && m_phaseTimingParameters.ShouldSequenceBeActive())
		{
			SpawnFX();
		}
		if (m_phaseTimingParameters.ShouldStopSequence(abilityPhase)
		    && m_fxInstances != null)
		{
			StopFX();
		}
	}

	private void Update()
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
		if (m_hitSpawnTime > 0f && GameTime.time > m_hitSpawnTime)
		{
			SpawnHitFX();
			m_hitSpawnTime = -1f;
		}
		int curFxIndex = GetCurrnetFxIndex();
		if (m_switchBetweenLevelsAfterSpawn
		    && m_fxInstances != null
		    && m_fxInstances.Count > 1)
		{
			int count = m_fxInstances.Count;
			curFxIndex = Mathf.Min(curFxIndex, count - 1);
			if (curFxIndex != m_lastActiveIndex)
			{
				if (m_lastActiveIndex >= 0
				    && m_lastActiveIndex < count
				    && m_fxInstances[m_lastActiveIndex] != null)
				{
					m_fxInstances[m_lastActiveIndex].SetActive(false);
				}
				if ((curFxIndex > 0 || m_alternateBaseFxInstance == null || AgeInTurns <= 0)
				    && m_fxInstances[curFxIndex] != null)
				{
					m_fxInstances[curFxIndex].SetActive(true);
				}
				Debug.LogWarning(new StringBuilder().Append("Setting index from ").Append(m_lastActiveIndex).Append(" to ").Append(curFxIndex).ToString());
				m_lastActiveIndex = curFxIndex;
			}
		}
		if (curFxIndex == 0 && AgeInTurns > 0 && m_alternateBaseFxInstance != null)
		{
			if (!m_alternateBaseFxInstance.activeSelf)
			{
				m_alternateBaseFxInstance.SetActive(true);
			}
			if (m_fxInstances != null && m_fxInstances.Count > 0 && m_fxInstances[0].activeSelf)
			{
				m_fxInstances[0].SetActive(false);
			}
		}
		if (m_fxInstances != null
		    && m_fxAttachToJoint
		    && m_fxJoint.IsInitialized()
		    && Caster != null
		    && ShouldHideForActorIfAttached(Caster))
		{
			SetSequenceVisibility(false);
		}
		else
		{
			ProcessSequenceVisibility();
		}
		if (m_fxInstances != null)
		{
			foreach (GameObject fx in m_fxInstances)
			{
				if (fx == null)
				{
					continue;
				}
				if (fx.GetComponent<FriendlyEnemyVFXSelector>() != null && Caster != null)
				{
					fx.GetComponent<FriendlyEnemyVFXSelector>().Setup(Caster.GetTeam());
				}
				if (m_useRootOrientation && Caster != null)
				{
					fx.transform.rotation = Caster.transform.rotation;
				}
			}
		}
	}

	private void StopFX()
	{
		if (m_fxInstances == null)
		{
			return;
		}
		foreach (GameObject fx in m_fxInstances)
		{
			if (fx != null)
			{
				fx.SetActive(false);
			}
		}
	}

	private void SpawnFX()
	{
		if (Caster != null)
		{
			if (!m_fxJoint.IsInitialized())
			{
				m_fxJoint.Initialize(Caster.gameObject);
			}
			if (m_fxPrefabs != null)
			{
				m_fxInstances = new List<GameObject>();
				List<GameObject> fxPrefabs = m_fxPrefabs;
				int currnetFxIndex = GetCurrnetFxIndex();
				currnetFxIndex = Mathf.Clamp(currnetFxIndex, 0, m_fxPrefabs.Count - 1);
				if (!m_switchBetweenLevelsAfterSpawn && m_fxPrefabs.Count > 1)
				{
					fxPrefabs = new List<GameObject> { m_fxPrefabs[currnetFxIndex] };
				}
				for (int i = 0; i < fxPrefabs.Count; i++)
				{
					GameObject fx = InstantiateAttachedFx(fxPrefabs[i]);
					if (m_switchBetweenLevelsAfterSpawn)
					{
						fx.SetActive(i == currnetFxIndex);
						if (i == currnetFxIndex)
						{
							m_lastActiveIndex = i;
						}
					}
					m_fxInstances.Add(fx);
				}
				if (m_fxPrefabAfterFirstTurn != null)
				{
					m_alternateBaseFxInstance = InstantiateAttachedFx(m_fxPrefabAfterFirstTurn);
					m_alternateBaseFxInstance.SetActive(false);
				}
			}
			if (!string.IsNullOrEmpty(m_audioEvent))
			{
				AudioManager.PostEvent(m_audioEvent, Caster.gameObject);
			}
		}

		if (m_hitSpawnTime < 0f && !m_attemptedToSpawnHitFx)
		{
			if (m_hitEvent == null && m_hitDelay <= 0f)
			{
				m_hitSpawnTime = GameTime.time;
			}
			else
			{
				m_hitSpawnTime = GameTime.time + m_hitDelay;
			}
		}
	}

	private GameObject InstantiateAttachedFx(GameObject fxPrefab)
	{
		GameObject fx;
		if (m_fxJoint.m_jointObject != null && m_fxJoint.m_jointObject.transform.localScale != Vector3.zero && m_fxAttachToJoint)
		{
			fx = InstantiateFX(fxPrefab);
			AttachToBone(fx, m_fxJoint.m_jointObject);
			fx.transform.localPosition = Vector3.zero;
			fx.transform.localRotation = Quaternion.identity;
		}
		else
		{
			Vector3 position = m_fxJoint.m_jointObject.transform.position;
			Quaternion quaternion = m_fxJoint.m_jointObject.transform.rotation;
			fx = InstantiateFX(fxPrefab, position, quaternion);
			SetAttribute(fx, "abilityAreaLength", (TargetPos - position).magnitude);
		}
		SetAttribute(fx, "targetDiameter", Caster.GetActorModelData().GetModelSize());
		return fx;
	}

	private void SpawnHitFX()
	{
		if (!m_attemptedToSpawnHitFx)
		{
			if (m_hitFxInstances == null)
			{
				m_hitFxInstances = new List<GameObject>();
			}
			if (Targets != null)
			{
				for (int i = 0; i < Targets.Length; i++)
				{
					Vector3 targetHitPosition = GetTargetHitPosition(i, m_hitFxJoint);
					Vector3 position = Caster.transform.position;
					if ((position - Targets[i].transform.position).magnitude < 0.1f)
					{
						position -= Caster.transform.forward * 0.5f;
					}
					Vector3 vector = targetHitPosition - position;
					vector.y = 0f;
					vector.Normalize();
					ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, vector);
					Quaternion rotation = m_hitAlignedWithCaster ? Quaternion.LookRotation(vector) : Quaternion.identity;
					bool isVisible = IsHitFXVisibleForActor(Targets[i]);
					if (m_hitFxPrefab != null && isVisible)
					{
						m_hitFxInstances.Add(InstantiateFX(m_hitFxPrefab, targetHitPosition, rotation));
					}
					if (isVisible)
					{
						if (!string.IsNullOrEmpty(m_hitAudioEvent))
						{
							AudioManager.PostEvent(m_hitAudioEvent, Targets[i].gameObject);
						}
					}
					if (Targets[i] != null)
					{
						Source.OnSequenceHit(this, Targets[i], impulseInfo);
					}
				}
			}
			Source.OnSequenceHit(this, TargetPos);
		}
		m_attemptedToSpawnHitFx = true;
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
		if (m_hitEvent == parameter)
		{
			SpawnHitFX();
		}
	}

	private void OnDisable()
	{
		if (m_fxInstances != null)
		{
			foreach (GameObject fx in m_fxInstances)
			{
				if (fx != null)
				{
					Destroy(fx.gameObject);
				}
			}
			m_fxInstances.Clear();
		}
		if (m_alternateBaseFxInstance != null)
		{
			Destroy(m_alternateBaseFxInstance);
			m_alternateBaseFxInstance = null;
		}
		if (m_hitFxInstances != null)
		{
			foreach (GameObject fx in m_hitFxInstances)
			{
				if (fx != null)
				{
					Destroy(fx.gameObject);
				}
			}
			m_hitFxInstances.Clear();
		}
	}
}
