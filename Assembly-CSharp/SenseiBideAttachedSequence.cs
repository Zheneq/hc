using System;
using System.Collections.Generic;
using UnityEngine;

public class SenseiBideAttachedSequence : Sequence
{
	[Header("-- Whether switch between different levels after spawn --", order = 1)]
	[Separator("For Main Fx", true, order = 0)]
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
	public UnityEngine.Object m_startEvent;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before stopping the sequence. Search project for EventObjects.")]
	public UnityEngine.Object m_stopEvent;

	[Header("-- Spawn Delay (ignored if there is Start Event) --")]
	public float m_startDelayTime;

	private float m_timeToSpawnVfx = -1f;

	public bool m_useRootOrientation;

	[AudioEvent(false)]
	public string m_audioEvent;

	[Separator("For Impact Fx", true)]
	public GameObject m_hitFxPrefab;

	[JointPopup("hit FX attach joint")]
	public JointPopupProperty m_hitFxJoint;

	public bool m_hitAlignedWithCaster;

	[AnimEventPicker]
	public UnityEngine.Object m_hitEvent;

	public float m_hitDelay;

	[Header("-- Team restrictions for Hit VFX on Targets --")]
	public Sequence.HitVFXSpawnTeam m_hitVfxSpawnTeamMode;

	[AudioEvent(false)]
	public string m_hitAudioEvent;

	public Sequence.PhaseTimingParameters m_phaseTimingParameters;

	private List<GameObject> m_fxInstances;

	private GameObject m_alternateBaseFxInstance;

	private List<GameObject> m_hitFxInstances;

	private float m_hitSpawnTime = -1f;

	private bool m_attemptedToSpawnHitFx;

	private Sensei_SyncComponent m_syncComp;

	private int m_lastActiveIndex = -1;

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		ActorData actorData = null;
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			base.OverridePhaseTimingParams(this.m_phaseTimingParameters, extraSequenceParams);
			if (extraSequenceParams is Sequence.ActorIndexExtraParam)
			{
				if (GameFlowData.Get() != null)
				{
					Sequence.ActorIndexExtraParam actorIndexExtraParam = extraSequenceParams as Sequence.ActorIndexExtraParam;
					int actorIndex = (int)actorIndexExtraParam.m_actorIndex;
					actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
				}
			}
		}
		if (actorData != null)
		{
			this.m_syncComp = actorData.GetComponent<Sensei_SyncComponent>();
		}
		else if (Application.isEditor)
		{
			Debug.LogError("Did not find Sensei for sensei ult sequence");
		}
	}

	public int GetCurrnetFxIndex()
	{
		if (!(this.m_syncComp != null) || this.m_fxPrefabs.Count <= 1)
		{
			return 0;
		}
		int num = this.m_fxPrefabs.Count - 1;
		float syncBideExtraDamagePct = this.m_syncComp.m_syncBideExtraDamagePct;
		if (syncBideExtraDamagePct >= 0.99f)
		{
			return num;
		}
		float num2 = 1f / (float)num;
		return Mathf.FloorToInt(syncBideExtraDamagePct / num2);
	}

	public override void FinishSetup()
	{
		if (this.m_startEvent == null)
		{
			if (this.m_phaseTimingParameters.ShouldSequenceBeActive())
			{
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
	}

	private bool IsHitFXVisibleForActor(ActorData hitTarget)
	{
		return base.IsHitFXVisibleWrtTeamFilter(hitTarget, this.m_hitVfxSpawnTeamMode);
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
				if (this.m_phaseTimingParameters.ShouldSequenceBeActive())
				{
					this.SpawnFX();
				}
			}
		}
		if (this.m_phaseTimingParameters.ShouldStopSequence(abilityPhase))
		{
			if (this.m_fxInstances != null)
			{
				this.StopFX();
			}
		}
	}

	private void Update()
	{
		if (this.m_initialized)
		{
			if (this.m_timeToSpawnVfx > 0f && GameTime.time >= this.m_timeToSpawnVfx)
			{
				this.m_timeToSpawnVfx = -1f;
				this.SpawnFX();
			}
			if (this.m_hitSpawnTime > 0f)
			{
				if (GameTime.time > this.m_hitSpawnTime)
				{
					this.SpawnHitFX();
					this.m_hitSpawnTime = -1f;
				}
			}
			int num = this.GetCurrnetFxIndex();
			if (this.m_switchBetweenLevelsAfterSpawn)
			{
				if (this.m_fxInstances != null)
				{
					if (this.m_fxInstances.Count > 1)
					{
						int count = this.m_fxInstances.Count;
						num = Mathf.Min(num, count - 1);
						if (num != this.m_lastActiveIndex)
						{
							if (this.m_lastActiveIndex >= 0)
							{
								if (this.m_lastActiveIndex < count)
								{
									if (this.m_fxInstances[this.m_lastActiveIndex] != null)
									{
										this.m_fxInstances[this.m_lastActiveIndex].SetActive(false);
									}
								}
							}
							if (num <= 0)
							{
								if (!(this.m_alternateBaseFxInstance == null))
								{
									if (base.AgeInTurns > 0)
									{
										goto IL_1C5;
									}
								}
							}
							if (this.m_fxInstances[num] != null)
							{
								this.m_fxInstances[num].SetActive(true);
							}
							IL_1C5:
							Debug.LogWarning(string.Concat(new object[]
							{
								"Setting index from ",
								this.m_lastActiveIndex,
								" to ",
								num
							}));
							this.m_lastActiveIndex = num;
						}
					}
				}
			}
			if (num == 0)
			{
				if (base.AgeInTurns > 0 && this.m_alternateBaseFxInstance != null)
				{
					if (!this.m_alternateBaseFxInstance.activeSelf)
					{
						this.m_alternateBaseFxInstance.SetActive(true);
					}
					if (this.m_fxInstances != null && this.m_fxInstances.Count > 0)
					{
						if (this.m_fxInstances[0].activeSelf)
						{
							this.m_fxInstances[0].SetActive(false);
						}
					}
				}
			}
			if (this.m_fxInstances != null && this.m_fxAttachToJoint)
			{
				if (this.m_fxJoint.IsInitialized())
				{
					if (base.Caster != null)
					{
						if (base.ShouldHideForActorIfAttached(base.Caster))
						{
							base.SetSequenceVisibility(false);
							goto IL_32A;
						}
					}
				}
			}
			base.ProcessSequenceVisibility();
			IL_32A:
			if (this.m_fxInstances != null)
			{
				for (int i = 0; i < this.m_fxInstances.Count; i++)
				{
					GameObject gameObject = this.m_fxInstances[i];
					if (gameObject != null)
					{
						if (gameObject.GetComponent<FriendlyEnemyVFXSelector>() != null && base.Caster != null)
						{
							gameObject.GetComponent<FriendlyEnemyVFXSelector>().Setup(base.Caster.GetTeam());
						}
						if (this.m_useRootOrientation)
						{
							if (base.Caster != null)
							{
								gameObject.transform.rotation = base.Caster.transform.rotation;
							}
						}
					}
				}
			}
		}
	}

	private void StopFX()
	{
		if (this.m_fxInstances != null)
		{
			for (int i = 0; i < this.m_fxInstances.Count; i++)
			{
				GameObject gameObject = this.m_fxInstances[i];
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
			}
		}
	}

	private void SpawnFX()
	{
		if (base.Caster != null)
		{
			if (!this.m_fxJoint.IsInitialized())
			{
				this.m_fxJoint.Initialize(base.Caster.gameObject);
			}
			if (this.m_fxPrefabs != null)
			{
				this.m_fxInstances = new List<GameObject>();
				List<GameObject> list = this.m_fxPrefabs;
				int num = this.GetCurrnetFxIndex();
				num = Mathf.Clamp(num, 0, this.m_fxPrefabs.Count - 1);
				if (!this.m_switchBetweenLevelsAfterSpawn)
				{
					if (this.m_fxPrefabs.Count > 1)
					{
						GameObject item = this.m_fxPrefabs[num];
						list = new List<GameObject>
						{
							item
						};
					}
				}
				for (int i = 0; i < list.Count; i++)
				{
					GameObject fxPrefab = list[i];
					GameObject gameObject = this.InstantiateAttachedFx(fxPrefab);
					if (this.m_switchBetweenLevelsAfterSpawn)
					{
						gameObject.SetActive(i == num);
						if (i == num)
						{
							this.m_lastActiveIndex = i;
						}
					}
					this.m_fxInstances.Add(gameObject);
				}
				if (this.m_fxPrefabAfterFirstTurn != null)
				{
					this.m_alternateBaseFxInstance = this.InstantiateAttachedFx(this.m_fxPrefabAfterFirstTurn);
					this.m_alternateBaseFxInstance.SetActive(false);
				}
			}
			if (!string.IsNullOrEmpty(this.m_audioEvent))
			{
				AudioManager.PostEvent(this.m_audioEvent, base.Caster.gameObject);
			}
		}
		if (this.m_hitSpawnTime < 0f)
		{
			if (!this.m_attemptedToSpawnHitFx)
			{
				if (this.m_hitEvent == null)
				{
					if (this.m_hitDelay <= 0f)
					{
						this.m_hitSpawnTime = GameTime.time;
						return;
					}
				}
				this.m_hitSpawnTime = GameTime.time + this.m_hitDelay;
			}
		}
	}

	private GameObject InstantiateAttachedFx(GameObject fxPrefab)
	{
		GameObject gameObject;
		if (this.m_fxJoint.m_jointObject != null)
		{
			if (this.m_fxJoint.m_jointObject.transform.localScale != Vector3.zero && this.m_fxAttachToJoint)
			{
				gameObject = base.InstantiateFX(fxPrefab);
				base.AttachToBone(gameObject, this.m_fxJoint.m_jointObject);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				goto IL_111;
			}
		}
		Vector3 position = this.m_fxJoint.m_jointObject.transform.position;
		Quaternion rotation = default(Quaternion);
		rotation = this.m_fxJoint.m_jointObject.transform.rotation;
		gameObject = base.InstantiateFX(fxPrefab, position, rotation, true, true);
		Sequence.SetAttribute(gameObject, "abilityAreaLength", (base.TargetPos - position).magnitude);
		IL_111:
		Sequence.SetAttribute(gameObject, "targetDiameter", base.Caster.GetActorModelData().GetModelSize());
		return gameObject;
	}

	private void SpawnHitFX()
	{
		if (!this.m_attemptedToSpawnHitFx)
		{
			if (this.m_hitFxInstances == null)
			{
				this.m_hitFxInstances = new List<GameObject>();
			}
			if (base.Targets != null)
			{
				for (int i = 0; i < base.Targets.Length; i++)
				{
					Vector3 targetHitPosition = base.GetTargetHitPosition(i, this.m_hitFxJoint);
					Vector3 vector = base.Caster.transform.position;
					if ((vector - base.Targets[i].transform.position).magnitude < 0.1f)
					{
						vector -= base.Caster.transform.forward * 0.5f;
					}
					Vector3 vector2 = targetHitPosition - vector;
					vector2.y = 0f;
					vector2.Normalize();
					ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, vector2);
					Quaternion quaternion;
					if (this.m_hitAlignedWithCaster)
					{
						quaternion = Quaternion.LookRotation(vector2);
					}
					else
					{
						quaternion = Quaternion.identity;
					}
					Quaternion rotation = quaternion;
					bool flag = this.IsHitFXVisibleForActor(base.Targets[i]);
					if (this.m_hitFxPrefab)
					{
						if (flag)
						{
							this.m_hitFxInstances.Add(base.InstantiateFX(this.m_hitFxPrefab, targetHitPosition, rotation, true, true));
						}
					}
					if (flag)
					{
						string hitAudioEvent = this.m_hitAudioEvent;
						if (!string.IsNullOrEmpty(hitAudioEvent))
						{
							AudioManager.PostEvent(hitAudioEvent, base.Targets[i].gameObject);
						}
					}
					if (base.Targets[i] != null)
					{
						base.Source.OnSequenceHit(this, base.Targets[i], impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
					}
				}
			}
			base.Source.OnSequenceHit(this, base.TargetPos, null);
		}
		this.m_attemptedToSpawnHitFx = true;
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_phaseTimingParameters.ShouldSequenceBeActive())
		{
			if (this.m_startEvent == parameter)
			{
				this.SpawnFX();
			}
			else if (this.m_stopEvent == parameter)
			{
				this.StopFX();
			}
			if (this.m_hitEvent == parameter)
			{
				this.SpawnHitFX();
			}
		}
	}

	private void OnDisable()
	{
		if (this.m_fxInstances != null)
		{
			for (int i = 0; i < this.m_fxInstances.Count; i++)
			{
				GameObject gameObject = this.m_fxInstances[i];
				if (gameObject != null)
				{
					UnityEngine.Object.Destroy(gameObject.gameObject);
				}
			}
			this.m_fxInstances.Clear();
		}
		if (this.m_alternateBaseFxInstance != null)
		{
			UnityEngine.Object.Destroy(this.m_alternateBaseFxInstance);
			this.m_alternateBaseFxInstance = null;
		}
		if (this.m_hitFxInstances != null)
		{
			for (int j = 0; j < this.m_hitFxInstances.Count; j++)
			{
				GameObject gameObject2 = this.m_hitFxInstances[j];
				if (gameObject2 != null)
				{
					UnityEngine.Object.Destroy(gameObject2.gameObject);
				}
			}
			this.m_hitFxInstances.Clear();
		}
	}
}
