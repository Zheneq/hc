using System;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAttachedVFXSequence : Sequence
{
	[Separator("Main FX Prefab", true)]
	public GameObject m_fxPrefab;

	[JointPopup("FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_fxJoint;

	public Sequence.ReferenceModelType m_jointReferenceType;

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

	public SimpleAttachedVFXSequence.AudioEventType m_hitAudioEventType;

	[Header("-- Alternative Impact Audio Events, handled per ability, unused otherwise")]
	[AudioEvent(false)]
	public string[] m_alternativeImpactAudioEvents;

	[Separator("Team restrictions for Hit FX on Targets", true)]
	public Sequence.HitVFXSpawnTeam m_hitVfxSpawnTeamMode;

	public List<SimpleAttachedVFXSequence.HitVFXStatusFilters> m_hitVfxStatusRequirements = new List<SimpleAttachedVFXSequence.HitVFXStatusFilters>();

	[Separator("Phase-Based Timing", true)]
	public Sequence.PhaseTimingParameters m_phaseTimingParameters;

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

	private List<SimpleAttachedVFXSequence.DelayedImpact> m_delayedImpacts = new List<SimpleAttachedVFXSequence.DelayedImpact>();

	private int m_alternativeAudioIndex = -1;

	private bool Finished()
	{
		bool result = false;
		if (!(this.GetFxPrefab() == null))
		{
			if (!base.AreFXFinished(this.m_fx))
			{
				return result;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXSequence.Finished()).MethodHandle;
			}
		}
		if (this.m_hitFxPrefab == null)
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
			if (this.m_playHitReactsWithoutFx && this.m_playedHitReact)
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
				result = true;
			}
		}
		else if (this.m_hitFx != null)
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
			result = true;
			using (List<GameObject>.Enumerator enumerator = this.m_hitFx.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject = enumerator.Current;
					if (gameObject != null)
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
						if (gameObject.activeSelf)
						{
							result = false;
							goto IL_E7;
						}
					}
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
		}
		IL_E7:
		if (this.m_delayedImpacts.Count > 0)
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
			result = false;
		}
		return result;
	}

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			SimpleAttachedVFXSequence.MultiEventExtraParams multiEventExtraParams = extraSequenceParams as SimpleAttachedVFXSequence.MultiEventExtraParams;
			if (multiEventExtraParams != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
				}
				this.m_eventNumberToKeyOffOf = multiEventExtraParams.eventNumberToKeyOffOf;
			}
			SimpleAttachedVFXSequence.ImpactDelayParams impactDelayParams = extraSequenceParams as SimpleAttachedVFXSequence.ImpactDelayParams;
			if (impactDelayParams != null)
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
				if (impactDelayParams.impactDelayTime > 0f)
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
					this.m_hitImpactDelayTime = impactDelayParams.impactDelayTime;
				}
				if ((int)impactDelayParams.alternativeImpactAudioIndex >= 0)
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
					this.m_alternativeAudioIndex = (int)impactDelayParams.alternativeImpactAudioIndex;
				}
			}
			if (extraSequenceParams is Sequence.FxAttributeParam)
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
				Sequence.FxAttributeParam fxAttributeParam = extraSequenceParams as Sequence.FxAttributeParam;
				if (fxAttributeParam != null && fxAttributeParam.m_paramNameCode != Sequence.FxAttributeParam.ParamNameCode.None)
				{
					string attributeName = fxAttributeParam.GetAttributeName();
					float paramValue = fxAttributeParam.m_paramValue;
					if (fxAttributeParam.m_paramTarget == Sequence.FxAttributeParam.ParamTarget.MainVfx)
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
						if (!this.m_fxAttributes.ContainsKey(attributeName))
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
							this.m_fxAttributes.Add(attributeName, paramValue);
						}
					}
				}
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
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
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXSequence.OnAbilityPhaseStart(AbilityPriority)).MethodHandle;
			}
			if (!this.m_spawnAttempted)
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
				if (this.m_phaseTimingParameters.ShouldSpawnSequence(abilityPhase))
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
					if (this.m_phaseTimingParameters.ShouldSequenceBeActive())
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
						this.SpawnFX(null);
					}
				}
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
				this.StopFX();
			}
		}
	}

	internal override Vector3 GetSequencePos()
	{
		if (this.m_fx != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXSequence.GetSequencePos()).MethodHandle;
			}
			return this.m_fx.transform.position;
		}
		return Vector3.zero;
	}

	public override void FinishSetup()
	{
		if (this.m_startEvent == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXSequence.FinishSetup()).MethodHandle;
			}
			this.SpawnFX(null);
		}
	}

	private bool IsHitFXVisibleForActor(ActorData hitTarget)
	{
		bool flag = base.IsHitFXVisibleWrtTeamFilter(hitTarget, this.m_hitVfxSpawnTeamMode);
		if (flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXSequence.IsHitFXVisibleForActor(ActorData)).MethodHandle;
			}
			if (this.m_hitVfxStatusRequirements != null)
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
				if (this.m_hitVfxStatusRequirements.Count > 0)
				{
					int num = 0;
					while (num < this.m_hitVfxStatusRequirements.Count && flag)
					{
						SimpleAttachedVFXSequence.HitVFXStatusFilters hitVFXStatusFilters = this.m_hitVfxStatusRequirements[num];
						if (hitVFXStatusFilters.m_status != StatusType.INVALID)
						{
							bool flag2 = hitTarget.GetActorStatus().HasStatus(hitVFXStatusFilters.m_status, true);
							if (hitVFXStatusFilters.m_condition == SimpleAttachedVFXSequence.HitVFXStatusFilters.FilterCond.HasStatus)
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
								if (!flag2)
								{
									goto IL_B7;
								}
							}
							if (hitVFXStatusFilters.m_condition != SimpleAttachedVFXSequence.HitVFXStatusFilters.FilterCond.DoesntHaveStatus)
							{
								goto IL_B9;
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
							if (!flag2)
							{
								goto IL_B9;
							}
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							IL_B7:
							flag = false;
						}
						IL_B9:
						num++;
					}
				}
			}
		}
		return flag;
	}

	protected virtual void SetFxRotation()
	{
		if (this.m_fx != null && this.m_useRootOrientation && base.Caster != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXSequence.SetFxRotation()).MethodHandle;
			}
			this.m_fx.transform.rotation = base.Caster.transform.rotation;
		}
	}

	protected virtual GameObject GetFxPrefab()
	{
		return this.m_fxPrefab;
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
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXSequence.OnUpdate()).MethodHandle;
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
				if (this.m_fxAttachToJoint)
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
					if (this.m_jointReferenceType == Sequence.ReferenceModelType.Actor)
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
						if (base.ShouldHideForActorIfAttached(base.Caster))
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
							goto IL_83;
						}
					}
				}
			}
			base.ProcessSequenceVisibility();
			IL_83:
			if (this.m_mainFxFoFSelector != null && base.Caster != null)
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
				this.m_mainFxFoFSelector.Setup(base.Caster.GetTeam());
			}
			this.SetFxRotation();
			if (this.m_hitSpawnTime > 0f && GameTime.time > this.m_hitSpawnTime)
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
				if (this.m_hitImpactDelayTime > 0f)
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
					this.m_delayedImpacts.Add(new SimpleAttachedVFXSequence.DelayedImpact(GameTime.time + this.m_hitImpactDelayTime, true));
				}
				else
				{
					this.SpawnHitFX(true);
				}
				this.m_hitSpawnTime = -1f;
			}
			for (int i = this.m_delayedImpacts.Count - 1; i >= 0; i--)
			{
				SimpleAttachedVFXSequence.DelayedImpact delayedImpact = this.m_delayedImpacts[i];
				if (GameTime.time >= delayedImpact.m_timeToSpawnImpact)
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
					this.SpawnHitFX(delayedImpact.m_lastHit);
					this.m_delayedImpacts.RemoveAt(i);
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_hitFx != null)
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
				if (this.m_hitFx.Count > 0)
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
						if (this.m_hitFxAttachToJoint)
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
							for (int j = 0; j < this.m_hitFx.Count; j++)
							{
								GameObject gameObject = this.m_hitFx[j];
								if (gameObject != null)
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
									if (this.m_hitAlignedWithCaster)
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
										Vector3 vector = gameObject.transform.position - base.Caster.GetTravelBoardSquareWorldPosition();
										vector.y = 0f;
										if (vector.magnitude > 1E-05f)
										{
											vector.Normalize();
											if (this.m_hitFxReverseAlignDir)
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
												vector *= -1f;
											}
											Quaternion rotation = Quaternion.LookRotation(vector);
											gameObject.transform.rotation = rotation;
										}
									}
									if (j < this.m_hitFxAttachedActors.Count)
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
										ActorData actorData = this.m_hitFxAttachedActors[j];
										if (actorData != null)
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
											bool desiredActive = base.IsActorConsideredVisible(actorData);
											gameObject.SetActiveIfNeeded(desiredActive);
										}
									}
								}
							}
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
						}
					}
				}
			}
			if (this.Finished())
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
				if (base.Source != null)
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
					if (!base.Source.RemoveAtEndOfTurn)
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
						base.MarkForRemoval();
					}
				}
			}
		}
	}

	protected void StopFX()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXSequence.StopFX()).MethodHandle;
			}
			this.m_fx.SetActive(false);
		}
	}

	private void SpawnHitFX(bool lastHit)
	{
		this.m_playedHitReact = true;
		if (this.m_hitFx == null)
		{
			this.m_hitFx = new List<GameObject>();
		}
		if (base.Targets != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXSequence.SpawnHitFX(bool)).MethodHandle;
			}
			if (base.Targets.Length > 0)
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
				CameraManager.Get().PlayCameraShake(this.m_hitCameraShakeType);
			}
			for (int i = 0; i < base.Targets.Length; i++)
			{
				ActorData actorData;
				if (i < base.Targets.Length)
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
					actorData = base.Targets[i];
				}
				else
				{
					actorData = null;
				}
				ActorData actorData2 = actorData;
				Vector3 targetHitPosition = base.GetTargetHitPosition(i, this.m_hitFxJoint);
				Vector3 vector = base.Caster.transform.position;
				if ((vector - base.Targets[i].transform.position).magnitude < 0.1f)
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
					vector -= base.Caster.transform.forward * 0.5f;
				}
				Vector3 vector2 = targetHitPosition - vector;
				vector2.y = 0f;
				vector2.Normalize();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, vector2);
				Quaternion quaternion;
				if (this.m_hitAlignedWithCaster)
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
					Vector3 forward;
					if (this.m_hitFxReverseAlignDir)
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
						forward = -1f * vector2;
					}
					else
					{
						forward = vector2;
					}
					quaternion = Quaternion.LookRotation(forward);
				}
				else
				{
					quaternion = Quaternion.identity;
				}
				Quaternion rotation = quaternion;
				bool flag = this.IsHitFXVisibleForActor(base.Targets[i]);
				if (this.m_hitFxPrefab)
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
					if (flag)
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
						GameObject gameObject = base.InstantiateFX(this.m_hitFxPrefab, targetHitPosition, rotation, true, true);
						if (!this.m_hitFxAttachToJoint)
						{
							goto IL_25E;
						}
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!(actorData2 != null))
						{
							goto IL_25E;
						}
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_hitFxJoint.Initialize(actorData2.gameObject);
						gameObject.transform.parent = this.m_hitFxJoint.m_jointObject.transform;
						gameObject.transform.localPosition = Vector3.zero;
						gameObject.transform.localRotation = Quaternion.identity;
						IL_274:
						FriendlyEnemyVFXSelector component = gameObject.GetComponent<FriendlyEnemyVFXSelector>();
						if (component != null)
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
							component.Setup(base.Caster.GetTeam());
						}
						this.m_hitFx.Add(gameObject);
						this.m_hitFxAttachedActors.Add(base.Targets[i]);
						goto IL_2CB;
						IL_25E:
						gameObject.transform.parent = base.transform;
						goto IL_274;
					}
				}
				IL_2CB:
				if (flag)
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
					string text = this.m_hitAudioEvent;
					if (this.m_alternativeAudioIndex >= 0)
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
						if (this.m_alternativeAudioIndex < this.m_alternativeImpactAudioEvents.Length)
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
							text = this.m_alternativeImpactAudioEvents[this.m_alternativeAudioIndex];
						}
					}
					if (this.m_hitAudioEventType == SimpleAttachedVFXSequence.AudioEventType.Pickup && !AudioManager.s_pickupAudio)
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
					}
					else if (!string.IsNullOrEmpty(text))
					{
						AudioManager.PostEvent(text, base.Targets[i].gameObject);
					}
				}
				if (base.Targets[i] != null)
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
					if (!lastHit)
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
						base.Source.OnSequenceHit(this, base.Targets[i], impulseInfo, ActorModelData.RagdollActivation.None, true);
					}
					else
					{
						base.Source.OnSequenceHit(this, base.Targets[i], impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
					}
				}
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
		base.Source.OnSequenceHit(this, base.TargetPos, null);
	}

	protected void SpawnFX(GameObject overrideFxPrefab = null)
	{
		this.m_spawnAttempted = true;
		if (!this.m_fxJoint.IsInitialized())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXSequence.SpawnFX(GameObject)).MethodHandle;
			}
			GameObject referenceModel = base.GetReferenceModel(base.Caster, this.m_jointReferenceType);
			if (referenceModel != null)
			{
				this.m_fxJoint.Initialize(referenceModel);
			}
		}
		GameObject gameObject = overrideFxPrefab;
		if (gameObject == null)
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
			gameObject = this.GetFxPrefab();
		}
		if (gameObject != null)
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
			Vector3 b = Vector3.zero;
			if (this.m_fxJoint.m_jointObject != null && this.m_fxJoint.m_jointObject.transform.localScale != Vector3.zero)
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
				if (this.m_fxAttachToJoint)
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
					this.m_fx = base.InstantiateFX(gameObject);
					base.AttachToBone(this.m_fx, this.m_fxJoint.m_jointObject);
					this.m_fx.transform.localPosition = Vector3.zero;
					this.m_fx.transform.localRotation = Quaternion.identity;
					b = this.m_fxJoint.m_jointObject.transform.position;
					goto IL_253;
				}
			}
			Vector3 vector = default(Vector3);
			if (this.m_fxJoint.m_jointObject != null)
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
				vector = this.m_fxJoint.m_jointObject.transform.position;
			}
			else
			{
				vector = base.transform.position;
			}
			Quaternion rotation = default(Quaternion);
			if (this.m_aimAtTarget)
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
				Vector3 targetPosition = base.GetTargetPosition(0, false);
				Vector3 lookRotation = targetPosition - vector;
				lookRotation.y = 0f;
				lookRotation.Normalize();
				rotation.SetLookRotation(lookRotation);
			}
			else if (this.m_fxJoint.m_jointObject)
			{
				rotation = this.m_fxJoint.m_jointObject.transform.rotation;
			}
			else
			{
				rotation = base.transform.rotation;
			}
			this.m_fx = base.InstantiateFX(gameObject, vector, rotation, true, true);
			b = vector;
			IL_253:
			Sequence.SetAttribute(this.m_fx, "abilityAreaLength", (base.TargetPos - b).magnitude);
			if (this.m_fx != null)
			{
				this.m_mainFxFoFSelector = this.m_fx.GetComponent<FriendlyEnemyVFXSelector>();
				if (this.m_mainFxFoFSelector != null)
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
					if (base.Caster != null)
					{
						this.m_mainFxFoFSelector.Setup(base.Caster.GetTeam());
					}
				}
			}
		}
		if (this.m_hitEvent == null)
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
			if (this.m_playHitReactsWithoutFx)
			{
				if (this.m_hitDelay > 0f)
				{
					this.m_hitSpawnTime = GameTime.time + this.m_hitDelay;
				}
				else
				{
					this.SpawnHitFX(true);
				}
			}
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
			if (this.m_fxAttributes != null)
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
				using (Dictionary<string, float>.Enumerator enumerator = this.m_fxAttributes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, float> keyValuePair = enumerator.Current;
						Sequence.SetAttribute(this.m_fx, keyValuePair.Key, keyValuePair.Value);
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
		}
		if (!string.IsNullOrEmpty(this.m_audioEvent))
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
			AudioManager.PostEvent(this.m_audioEvent, base.Caster.gameObject);
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
		{
			if (this.m_eventNumberToKeyOffOf >= 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
				}
				if (this.m_numStartEventsReceived != this.m_eventNumberToKeyOffOf)
				{
					this.m_numStartEventsReceived++;
					goto IL_7F;
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
			}
			if (this.m_eventNumberToKeyOffOf >= 0)
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
				this.m_numStartEventsReceived++;
			}
			this.SpawnFX(null);
			IL_7F:;
		}
		else if (this.m_stopEvent == parameter)
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
			if (this.m_spawnAttempted)
			{
				this.StopFX();
			}
		}
		if (this.m_hitEvent == parameter)
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
			if (this.m_hitImpactDelayTime > 0f)
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
				this.m_delayedImpacts.Add(new SimpleAttachedVFXSequence.DelayedImpact(GameTime.time + this.m_hitImpactDelayTime, this.m_lastHitEvent == null));
			}
			else
			{
				this.SpawnHitFX(this.m_lastHitEvent == null);
			}
		}
		else if (this.m_lastHitEvent == parameter)
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
			if (this.m_hitImpactDelayTime > 0f)
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
				this.m_delayedImpacts.Add(new SimpleAttachedVFXSequence.DelayedImpact(GameTime.time + this.m_hitImpactDelayTime, true));
			}
			else
			{
				this.SpawnHitFX(true);
			}
		}
	}

	private void OnDisable()
	{
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXSequence.OnDisable()).MethodHandle;
			}
			this.m_mainFxFoFSelector = null;
			UnityEngine.Object.Destroy(this.m_fx.gameObject);
			this.m_fx = null;
		}
		if (this.m_hitFx != null)
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
			using (List<GameObject>.Enumerator enumerator = this.m_hitFx.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject = enumerator.Current;
					UnityEngine.Object.Destroy(gameObject.gameObject);
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			this.m_hitFx = null;
		}
	}

	public override string GetSequenceSpecificDescription()
	{
		string text = string.Empty;
		if (this.m_fxPrefab == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleAttachedVFXSequence.GetSequenceSpecificDescription()).MethodHandle;
			}
			text += "<color=yellow>WARNING: </color>No VFX Prefab for <FX Prefab>\n\n";
		}
		if (this.m_fxJoint != null && string.IsNullOrEmpty(this.m_fxJoint.m_joint))
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
			text += "<color=yellow>WARNING: </color>VFX joint is empty, may not spawn at expected location.\n\n";
		}
		if (this.m_jointReferenceType != Sequence.ReferenceModelType.Actor)
		{
			text = text + "<Joint Reference Type> is <color=cyan>" + this.m_jointReferenceType.ToString() + "</color>\n\n";
		}
		if (this.m_fxJoint != null)
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
				text = text + "<color=cyan>VFX is attaching to joint (" + this.m_fxJoint.m_joint + ")</color>\n";
				if (this.m_aimAtTarget)
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
					text += "<[x] Aim At Target> ignored, attaching to joint\n";
				}
				text += "\n";
			}
			else
			{
				text = text + "VFX spawning at joint (<color=cyan>" + this.m_fxJoint.m_joint + "</color>), not set to attach.\n\n";
			}
		}
		if (this.m_useRootOrientation)
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
			text += "<[x] Use Root Orientaion> rotation is set to Caster's orientation per update\n\n";
		}
		if (!(this.m_hitEvent != null))
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
			if (!this.m_playHitReactsWithoutFx)
			{
				text += "Ignoring Gameplay Hits\n";
				text += "(If need Gameplay Hits, check <[x] Play Hit React Without Fx> or add <Hit Event>)\n\n";
				goto IL_19A;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		text += "<color=cyan>Can do Gameplay Hits</color>\n";
		IL_19A:
		if (this.m_lastHitEvent != null)
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
			text += "Has <Last Hit Event>, will not trigger ragdoll until that event is fired\n\n";
		}
		if (this.m_hitEvent != null)
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
			if (this.m_hitDelay > 0f)
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
				return text + "<color=yellow>WARNING: </color>Has <Hit Event>, <Hit Delay> will be ignored\n\n";
			}
		}
		if (this.m_hitEvent == null)
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
			if (this.m_hitDelay > 0f)
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
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"Using <Hit Delay> for timing, Gameplay Hit and <Hit FX Prefab> will spawn ",
					this.m_hitDelay,
					" second(s) after VFX spawn\n\n"
				});
			}
		}
		return text;
	}

	public class MultiEventExtraParams : Sequence.IExtraSequenceParams
	{
		public int eventNumberToKeyOffOf;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			short num = (short)this.eventNumberToKeyOffOf;
			stream.Serialize(ref num);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			short num = 0;
			stream.Serialize(ref num);
			this.eventNumberToKeyOffOf = (int)num;
		}
	}

	public class ImpactDelayParams : Sequence.IExtraSequenceParams
	{
		public float impactDelayTime = -1f;

		public sbyte alternativeImpactAudioIndex = -1;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref this.impactDelayTime);
			stream.Serialize(ref this.alternativeImpactAudioIndex);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref this.impactDelayTime);
			stream.Serialize(ref this.alternativeImpactAudioIndex);
		}
	}

	public class DelayedImpact
	{
		public float m_timeToSpawnImpact;

		public bool m_lastHit;

		public DelayedImpact(float timeToSpawn, bool lastHit)
		{
			this.m_timeToSpawnImpact = timeToSpawn;
			this.m_lastHit = lastHit;
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
		public SimpleAttachedVFXSequence.HitVFXStatusFilters.FilterCond m_condition;

		public StatusType m_status = StatusType.INVALID;

		public enum FilterCond
		{
			HasStatus,
			DoesntHaveStatus
		}
	}
}
