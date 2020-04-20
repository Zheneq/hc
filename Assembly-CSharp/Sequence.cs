using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sequence : MonoBehaviour
{
	public Sequence.SequenceNotes m_setupNotes;

	[Separator("For Hit React Animation", true)]
	public bool m_targetHitAnimation = true;

	public bool m_canTriggerHitReactOnAllyHit;

	public string m_customHitReactTriggerName = string.Empty;

	[Separator("Visibility (please don't forget me T_T ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~)", true)]
	[Tooltip("What visibility rules to use for this sequence")]
	public Sequence.VisibilityType m_visibilityType;

	[Tooltip("What visibility rules to use for the hitFX")]
	public Sequence.HitVisibilityType m_hitVisibilityType;

	public Sequence.PhaseBasedVisibilityType m_phaseVisibilityType;

	[Separator("How to Hide Vfx Object", true)]
	public Sequence.SequenceHideType m_sequenceHideType;

	[Space(5f)]
	public bool m_turnOffVFXDuringCinematicCam = true;

	[Separator("For keeping VFX in Caster Taunts", true)]
	public bool m_keepVFXInCinematicCamForCaster;

	public int m_keepCasterVFXForAnimIndex = -1;

	public bool m_keepCasterVFXForTurnOfSpawnOnly = true;

	[Header("-- For Debugging Only --")]
	public bool m_debugFlag;

	private ActorData m_caster;

	internal int m_casterId = ActorData.s_invalidActorIndex;

	private BoardSquare m_targetBoardSquare;

	public static string s_defaultHitAttachJoint = "upperRoot_JNT";

	public static string s_defaultFallbackHitAttachJoint = "root_JNT";

	private bool m_waitForClientEnable = true;

	private GameObject m_fxParent;

	private List<GameObject> m_parentedFXs;

	private bool m_lastSetVisibleValue = true;

	protected bool m_forceAlwaysVisible;

	protected float m_startTime;

	protected bool m_initialized;

	private static Sequence.IExtraSequenceParams[] s_emptyParams = new Sequence.IExtraSequenceParams[0];

	private bool m_debugHasReceivedAnimEventBeforeReady;

	public const string c_editorDescWarning = "<color=yellow>WARNING: </color>";

	public const string c_canDoGameplayHit = "<color=cyan>Can do Gameplay Hits</color>\n";

	public const string c_ignoreGameplayHit = "Ignoring Gameplay Hits\n";

	private static string c_casterToken = "<color=white>[Caster]</color>";

	private static string c_targetActorToken = "<color=white>[TargetActor]</color>";

	private static string c_targetPosToken = "<color=lightblue>[TargetPos]</color>";

	private static string c_seqPosToken = "<color=lightblue>[SeqPos]</color>";

	private static string c_clientActorToken = "<color=white>[ClientActor]</color>";

	public static string s_seqPosNote = "note: <color=lightblue>[SeqPos]</color> is usually only relevant for projectiles which is projectile's current position\n";

	public static string s_targetPosNote = "note: <color=lightblue>[TargetPos]</color> is passed in from ability, usually Caster or Target's position for attached vfx, or end position for projectiles\n";

	public GameObject GetReferenceModel(ActorData referenceActorData, Sequence.ReferenceModelType referenceModelType)
	{
		GameObject result = null;
		if (referenceModelType == Sequence.ReferenceModelType.Actor)
		{
			if (referenceActorData != null)
			{
				result = referenceActorData.gameObject;
			}
		}
		else if (referenceModelType == Sequence.ReferenceModelType.TempSatellite)
		{
			result = SequenceManager.Get().FindTempSatellite(this.Source);
		}
		else if (referenceModelType == Sequence.ReferenceModelType.PersistentSatellite1)
		{
			if (referenceActorData != null)
			{
				SatelliteController component = referenceActorData.GetComponent<SatelliteController>();
				if (component != null)
				{
					if (component.GetSatellite(0) != null)
					{
						result = component.GetSatellite(0).gameObject;
					}
				}
			}
		}
		return result;
	}

	public short PrefabLookupId { get; private set; }

	public void InitPrefabLookupId(short lookupId)
	{
		this.PrefabLookupId = lookupId;
	}

	internal ActorData[] Targets { get; set; }

	internal ActorData Target
	{
		get
		{
			if (this.Targets == null || this.Targets.Length == 0)
			{
				return null;
			}
			return this.Targets[0];
		}
	}

	internal ActorData Caster
	{
		get
		{
			return this.m_caster;
		}
		set
		{
			this.m_caster = value;
			if (value != null)
			{
				this.m_casterId = value.ActorIndex;
			}
		}
	}

	internal BoardSquare TargetSquare
	{
		get
		{
			return this.m_targetBoardSquare;
		}
		set
		{
			this.m_targetBoardSquare = value;
		}
	}

	internal Vector3 TargetPos { get; set; }

	internal Quaternion TargetRotation { get; set; }

	internal int AgeInTurns { get; set; }

	internal bool Ready
	{
		get
		{
			if (this.m_initialized)
			{
				if (base.enabled)
				{
					return !this.MarkedForRemoval;
				}
			}
			return false;
		}
	}

	internal bool InitializedEver { get; private set; }

	internal bool MarkedForRemoval { get; private set; }

	internal void MarkForRemoval()
	{
		this.MarkedForRemoval = true;
		base.enabled = false;
	}

	internal static void MarkSequenceArrayForRemoval(Sequence[] sequencesArray)
	{
		if (sequencesArray != null)
		{
			for (int i = 0; i < sequencesArray.Length; i++)
			{
				if (sequencesArray[i] != null)
				{
					sequencesArray[i].MarkForRemoval();
				}
			}
		}
	}

	internal bool RequestsHitAnimation(ActorData target)
	{
		int result;
		if (this.m_targetHitAnimation)
		{
			if (target != null)
			{
				if (this.Caster != null)
				{
					if (!this.m_canTriggerHitReactOnAllyHit)
					{
						result = ((this.Caster.GetOpposingTeam() == target.GetTeam()) ? 1 : 0);
					}
					else
					{
						result = 1;
					}
					return result != 0;
				}
			}
		}
		result = 0;
		return result != 0;
	}

	internal bool RemoveAtTurnEnd { get; set; }

	internal int Id { get; set; }

	internal SequenceSource Source { get; private set; }

	protected virtual void Awake()
	{
	}

	protected virtual void Start()
	{
		this.m_startTime = GameTime.time;
	}

	internal void OnDoClientEnable()
	{
		if (this.m_waitForClientEnable)
		{
			this.FinishSetup();
			this.DoneInitialization();
			this.ProcessSequenceVisibility();
			if (SequenceManager.SequenceDebugTraceOn)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					"<color=yellow>Client Enable: </color><<color=lightblue>",
					base.gameObject.name,
					" | ",
					base.GetType(),
					"</color>> @time= ",
					GameTime.time
				}));
			}
			base.enabled = true;
		}
	}

	protected virtual void OnDestroy()
	{
		if (this.m_parentedFXs != null)
		{
			using (List<GameObject>.Enumerator enumerator = this.m_parentedFXs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject obj = enumerator.Current;
					UnityEngine.Object.Destroy(obj);
				}
			}
		}
		if (SequenceManager.Get() != null)
		{
			SequenceManager.Get().OnDestroySequence(this);
		}
	}

	private void DoneInitialization()
	{
		if (!this.InitializedEver)
		{
			this.m_initialized = true;
			this.InitializedEver = true;
		}
	}

	internal virtual void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
	}

	public virtual void FinishSetup()
	{
	}

	internal void BaseInitialize_Client(BoardSquare targetSquare, Vector3 targetPos, Quaternion targetRotation, ActorData[] targets, ActorData caster, int id, GameObject prefab, short baseSequenceLookupId, SequenceSource source, Sequence.IExtraSequenceParams[] extraParams)
	{
		this.RemoveAtTurnEnd = source.RemoveAtEndOfTurn;
		this.TargetSquare = targetSquare;
		this.TargetPos = targetPos;
		this.TargetRotation = targetRotation;
		this.Targets = targets;
		this.Caster = caster;
		this.Id = id;
		this.Source = source;
		this.InitPrefabLookupId(baseSequenceLookupId);
		this.m_startTime = GameTime.time;
		Sequence.IExtraSequenceParams[] extraParams2;
		if (extraParams != null)
		{
			extraParams2 = extraParams;
		}
		else
		{
			extraParams2 = Sequence.s_emptyParams;
		}
		this.Initialize(extraParams2);
		this.m_waitForClientEnable = this.Source.WaitForClientEnable;
		base.enabled = !this.m_waitForClientEnable;
		if (!this.m_waitForClientEnable)
		{
			this.FinishSetup();
			this.DoneInitialization();
		}
	}

	protected virtual void OnStopVfxOnClient()
	{
	}

	private void OnActorAdded(ActorData actor)
	{
		if (!this.InitializedEver)
		{
			if (base.enabled)
			{
				if (!this.MarkedForRemoval)
				{
					if (this.m_caster == null)
					{
						if (this.m_casterId != ActorData.s_invalidActorIndex)
						{
							this.m_caster = GameFlowData.Get().FindActorByActorIndex(this.m_casterId);
							if (this.m_caster != null)
							{
								this.FinishSetup();
								this.DoneInitialization();
								GameFlowData.s_onAddActor -= this.OnActorAdded;
							}
						}
					}
				}
			}
		}
	}

	internal bool IsCasterOrTargetsVisible()
	{
		bool result = false;
		if (this.IsActorConsideredVisible(this.Caster))
		{
			result = true;
		}
		else if (this.Targets != null)
		{
			foreach (ActorData actor in this.Targets)
			{
				if (this.IsActorConsideredVisible(actor))
				{
					return true;
				}
			}
		}
		return result;
	}

	internal virtual Vector3 GetSequencePos()
	{
		return this.TargetPos;
	}

	internal bool IsSequencePosVisible()
	{
		bool result = false;
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (clientFog != null)
		{
			if (this.GetSequencePos().magnitude > 0f)
			{
				if (Board.Get().m_showLOS)
				{
					if (!clientFog.IsVisible(Board.Get().GetBoardSquare(this.GetSequencePos())))
					{
						goto IL_81;
					}
				}
				result = true;
			}
			IL_81:;
		}
		else
		{
			result = true;
		}
		return result;
	}

	internal bool IsTargetPosVisible()
	{
		bool result = false;
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (clientFog != null)
		{
			if (clientFog.IsVisible(Board.Get().GetBoardSquare(this.TargetPos)))
			{
				result = true;
			}
		}
		else
		{
			result = true;
		}
		return result;
	}

	internal bool IsCasterOrTargetPosVisible()
	{
		bool result = false;
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (clientFog != null)
		{
			if (this.IsActorConsideredVisible(this.Caster))
			{
				result = true;
			}
			else if (clientFog.IsVisible(Board.Get().GetBoardSquare(this.TargetPos)))
			{
				result = true;
			}
		}
		else
		{
			result = true;
		}
		return result;
	}

	internal bool IsCasterOrTargetOrTargetPosVisible()
	{
		bool result = false;
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (clientFog != null)
		{
			if (this.IsActorConsideredVisible(this.Caster))
			{
				result = true;
			}
			else if (clientFog.IsVisible(Board.Get().GetBoardSquare(this.TargetPos)))
			{
				result = true;
			}
			else if (this.Targets != null)
			{
				foreach (ActorData actor in this.Targets)
				{
					if (this.IsActorConsideredVisible(actor))
					{
						result = true;
						break;
					}
				}
			}
		}
		else
		{
			result = true;
		}
		return result;
	}

	protected void SetSequenceVisibility(bool visible)
	{
		this.m_lastSetVisibleValue = visible;
		if (this.m_fxParent != null && this.m_parentedFXs != null)
		{
			if (this.m_sequenceHideType == Sequence.SequenceHideType.MoveOffCamera_KeepEnabled)
			{
				Transform transform = this.m_fxParent.transform;
				Vector3 localPosition;
				if (visible)
				{
					localPosition = Vector3.zero;
				}
				else
				{
					localPosition = -10000f * Vector3.one;
				}
				transform.localPosition = localPosition;
			}
			else
			{
				this.m_fxParent.SetActive(visible);
			}
			for (int i = 0; i < this.m_parentedFXs.Count; i++)
			{
				GameObject gameObject = this.m_parentedFXs[i];
				if (gameObject)
				{
					if (this.m_sequenceHideType == Sequence.SequenceHideType.MoveOffCamera_KeepEnabled)
					{
						Transform transform2 = gameObject.transform;
						Vector3 localPosition2;
						if (visible)
						{
							localPosition2 = Vector3.zero;
						}
						else
						{
							localPosition2 = -10000f * Vector3.one;
						}
						transform2.localPosition = localPosition2;
					}
					else
					{
						if (this.m_sequenceHideType == Sequence.SequenceHideType.KillThenDisable)
						{
							if (!visible)
							{
								PKFxFX[] componentsInChildren = gameObject.GetComponentsInChildren<PKFxFX>(true);
								foreach (PKFxFX pkfxFX in componentsInChildren)
								{
									pkfxFX.KillEffect();
								}
							}
						}
						gameObject.SetActive(visible);
					}
				}
			}
		}
	}

	protected bool LastDesiredVisible()
	{
		return this.m_lastSetVisibleValue;
	}

	protected bool IsActorConsideredVisible(ActorData actor)
	{
		int result;
		if (actor != null)
		{
			if (actor.IsVisibleToClient())
			{
				if (!(actor.GetActorModelData() == null))
				{
					result = (actor.GetActorModelData().IsVisibleToClient() ? 1 : 0);
				}
				else
				{
					result = 1;
				}
				return result != 0;
			}
		}
		result = 0;
		return result != 0;
	}

	protected void ProcessSequenceVisibility()
	{
		if (this.m_initialized)
		{
			if (!this.MarkedForRemoval)
			{
				if (this.m_phaseVisibilityType != Sequence.PhaseBasedVisibilityType.Any)
				{
					if (GameFlowData.Get() != null)
					{
						GameState gameState = GameFlowData.Get().gameState;
						if (this.m_phaseVisibilityType == Sequence.PhaseBasedVisibilityType.InDecisionOnly)
						{
							if (gameState != GameState.BothTeams_Decision)
							{
								goto IL_A0;
							}
						}
						if (this.m_phaseVisibilityType != Sequence.PhaseBasedVisibilityType.InResolutionOnly)
						{
							goto IL_A8;
						}
						if (gameState == GameState.BothTeams_Resolve)
						{
							goto IL_A8;
						}
						IL_A0:
						this.SetSequenceVisibility(false);
						return;
					}
				}
				IL_A8:
				bool flag;
				if (this.m_turnOffVFXDuringCinematicCam)
				{
					if (CameraManager.Get())
					{
						flag = CameraManager.Get().InCinematic();
						goto IL_DB;
					}
				}
				flag = false;
				IL_DB:
				bool flag2 = flag;
				if (flag2)
				{
					if (this.m_keepVFXInCinematicCamForCaster)
					{
						if (this.Caster != null)
						{
							ActorData cinematicTargetActor = CameraManager.Get().GetCinematicTargetActor();
							int cinematicActionAnimIndex = CameraManager.Get().GetCinematicActionAnimIndex();
							bool flag3;
							if (this.m_keepCasterVFXForAnimIndex > 0)
							{
								flag3 = (this.m_keepCasterVFXForAnimIndex == cinematicActionAnimIndex);
							}
							else
							{
								flag3 = true;
							}
							bool flag4 = flag3;
							bool flag5;
							if (this.m_keepCasterVFXForTurnOfSpawnOnly)
							{
								flag5 = (this.AgeInTurns == 0);
							}
							else
							{
								flag5 = true;
							}
							bool flag6 = flag5;
							if (cinematicTargetActor.ActorIndex == this.Caster.ActorIndex && flag4)
							{
								if (flag6)
								{
									flag2 = false;
								}
							}
						}
					}
				}
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
				if (flag2)
				{
					this.SetSequenceVisibility(false);
				}
				else
				{
					if (activeOwnedActorData == null)
					{
						if (localPlayerData == null)
						{
							this.SetSequenceVisibility(true);
							return;
						}
					}
					if (this.m_forceAlwaysVisible)
					{
						this.SetSequenceVisibility(true);
					}
					else if (this.m_visibilityType == Sequence.VisibilityType.Caster)
					{
						if (this.Caster != null)
						{
							this.SetSequenceVisibility(this.IsActorConsideredVisible(this.Caster));
						}
						else
						{
							this.SetSequenceVisibility(true);
						}
					}
					else if (this.m_visibilityType == Sequence.VisibilityType.CasterOrTarget)
					{
						this.SetSequenceVisibility(this.IsCasterOrTargetsVisible());
					}
					else if (this.m_visibilityType == Sequence.VisibilityType.CasterOrTargetPos)
					{
						this.SetSequenceVisibility(this.IsCasterOrTargetPosVisible());
					}
					else if (this.m_visibilityType == Sequence.VisibilityType.CasterOrTargetOrTargetPos)
					{
						this.SetSequenceVisibility(this.IsCasterOrTargetOrTargetPosVisible());
					}
					else if (this.m_visibilityType == Sequence.VisibilityType.Target)
					{
						bool flag7;
						if (this.Targets != null)
						{
							if (this.Targets.Length > 0)
							{
								flag7 = this.IsActorConsideredVisible(this.Targets[0]);
								goto IL_30D;
							}
						}
						flag7 = false;
						IL_30D:
						bool sequenceVisibility = flag7;
						this.SetSequenceVisibility(sequenceVisibility);
					}
					else if (this.m_visibilityType == Sequence.VisibilityType.TargetPos)
					{
						this.SetSequenceVisibility(this.IsTargetPosVisible());
					}
					else if (this.m_visibilityType == Sequence.VisibilityType.AlwaysOnlyIfCaster)
					{
						this.SetSequenceVisibility(this.Caster == activeOwnedActorData);
					}
					else if (this.m_visibilityType == Sequence.VisibilityType.AlwaysOnlyIfTarget)
					{
						this.SetSequenceVisibility(this.Target == activeOwnedActorData);
					}
					else if (this.m_visibilityType == Sequence.VisibilityType.AlwaysIfCastersTeam)
					{
						if (this.Caster != null && activeOwnedActorData != null)
						{
							this.SetSequenceVisibility(this.Caster.GetTeam() == activeOwnedActorData.GetTeam());
						}
						else
						{
							this.SetSequenceVisibility(true);
						}
					}
					else if (this.m_visibilityType == Sequence.VisibilityType.SequencePosition)
					{
						this.SetSequenceVisibility(this.IsSequencePosVisible());
					}
					else if (this.m_visibilityType == Sequence.VisibilityType.Always)
					{
						this.SetSequenceVisibility(true);
					}
					else if (this.m_visibilityType == Sequence.VisibilityType.CastersTeamOrSequencePosition)
					{
						if (this.Caster != null)
						{
							if (activeOwnedActorData != null)
							{
								this.SetSequenceVisibility(this.Caster.GetTeam() == activeOwnedActorData.GetTeam() || this.IsSequencePosVisible());
								goto IL_4C3;
							}
						}
						this.SetSequenceVisibility(this.IsSequencePosVisible());
						IL_4C3:;
					}
					else if (this.m_visibilityType == Sequence.VisibilityType.TargetPosAndCaster)
					{
						bool flag8;
						if (this.IsTargetPosVisible())
						{
							if (!(this.Caster == null))
							{
								flag8 = this.IsActorConsideredVisible(this.Caster);
							}
							else
							{
								flag8 = true;
							}
						}
						else
						{
							flag8 = false;
						}
						bool sequenceVisibility2 = flag8;
						this.SetSequenceVisibility(sequenceVisibility2);
					}
					else
					{
						if (this.m_visibilityType != Sequence.VisibilityType.TargetIfNotEvading)
						{
							if (this.m_visibilityType != Sequence.VisibilityType.CasterIfNotEvading)
							{
								return;
							}
						}
						bool sequenceVisibility3 = false;
						ActorData actorData2;
						if (this.m_visibilityType == Sequence.VisibilityType.TargetIfNotEvading)
						{
							ActorData actorData;
							if (this.Targets != null)
							{
								if (this.Targets.Length > 0)
								{
									actorData = this.Targets[0];
									goto IL_59E;
								}
							}
							actorData = null;
							IL_59E:
							actorData2 = actorData;
						}
						else
						{
							actorData2 = this.Caster;
						}
						if (actorData2 != null)
						{
							if (this.IsActorConsideredVisible(actorData2))
							{
								if (actorData2.GetActorMovement() != null)
								{
									sequenceVisibility3 = !actorData2.GetActorMovement().InChargeState();
								}
								else
								{
									sequenceVisibility3 = true;
								}
							}
						}
						this.SetSequenceVisibility(sequenceVisibility3);
					}
				}
				return;
			}
		}
		this.SetSequenceVisibility(false);
	}

	public bool HasReceivedAnimEventBeforeReady
	{
		get
		{
			return this.m_debugHasReceivedAnimEventBeforeReady;
		}
		set
		{
			this.m_debugHasReceivedAnimEventBeforeReady = value;
		}
	}

	internal virtual void OnTurnStart(int currentTurn)
	{
	}

	internal virtual void OnAbilityPhaseStart(AbilityPriority abilityPhase)
	{
	}

	protected virtual void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
	}

	internal virtual void SetTimerController(int value)
	{
	}

	internal void AnimationEvent(UnityEngine.Object eventObject, GameObject sourceObject)
	{
		if (this.Ready)
		{
			this.OnAnimationEvent(eventObject, sourceObject);
		}
	}

	protected void CallHitSequenceOnTargets(Vector3 impactPos, float defaultImpulseRadius = 1f, List<ActorData> actorsToIgnore = null, bool tryHitReactIfAlreadyHit = true)
	{
		float num = 0f;
		if (this.Targets != null)
		{
			for (int i = 0; i < this.Targets.Length; i++)
			{
				if (this.Targets[i] != null)
				{
					float magnitude = (this.Targets[i].transform.position - impactPos).magnitude;
					if (magnitude > num)
					{
						num = magnitude;
					}
				}
			}
		}
		float num2;
		if (num < Board.Get().squareSize / 2f)
		{
			num2 = defaultImpulseRadius;
		}
		else
		{
			num2 = num;
		}
		num = num2;
		ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(num, impactPos);
		if (this.Targets != null)
		{
			for (int j = 0; j < this.Targets.Length; j++)
			{
				if (this.Targets[j] != null)
				{
					if (actorsToIgnore != null)
					{
						if (actorsToIgnore.Contains(this.Targets[j]))
						{
							goto IL_130;
						}
					}
					this.Source.OnSequenceHit(this, this.Targets[j], impulseInfo, ActorModelData.RagdollActivation.HealthBased, tryHitReactIfAlreadyHit);
				}
				IL_130:;
			}
		}
		this.Source.OnSequenceHit(this, this.TargetPos, impulseInfo);
		List<Team> list = new List<Team>();
		list.Add(this.Caster.GetOpposingTeam());
		list.Add(this.Caster.GetTeam());
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(impactPos, num * 3f, false, this.Caster, list, null, false, default(Vector3));
		for (int k = 0; k < actorsInRadius.Count; k++)
		{
			if (actorsInRadius[k] != null)
			{
				if (actorsInRadius[k].GetActorModelData() != null)
				{
					Vector3 direction = actorsInRadius[k].transform.position - impactPos;
					direction.y = 0f;
					direction.Normalize();
					actorsInRadius[k].GetActorModelData().ImpartWindImpulse(direction);
				}
			}
		}
	}

	protected Vector3 GetTargetHitPosition(ActorData actorData, JointPopupProperty fxJoint)
	{
		Vector3 result = Vector3.zero;
		for (int i = 0; i < this.Targets.Length; i++)
		{
			if (this.Targets[i] == actorData)
			{
				result = this.GetTargetHitPosition(i, fxJoint);
				return result;
			}
		}
		return result;
	}

	protected Vector3 GetTargetHitPosition(int targetIndex, JointPopupProperty fxJoint)
	{
		bool flag = false;
		Vector3 result = Vector3.zero;
		if (this.Targets != null)
		{
			if (this.Targets.Length > targetIndex && this.Targets[targetIndex] != null)
			{
				if (this.Targets[targetIndex].gameObject != null)
				{
					fxJoint.Initialize(this.Targets[targetIndex].gameObject);
					if (fxJoint.m_jointObject != null)
					{
						result = fxJoint.m_jointObject.transform.position;
						flag = true;
					}
				}
			}
		}
		if (!flag)
		{
			result = this.GetTargetHitPosition(targetIndex);
		}
		return result;
	}

	protected Vector3 GetTargetHitPosition(int targetIndex)
	{
		return this.GetTargetPosition(targetIndex, true);
	}

	protected Vector3 GetTargetPosition(int targetIndex, bool secondaryActorHits = false)
	{
		if (secondaryActorHits)
		{
			if (this.Targets != null)
			{
				if (this.Targets.Length > targetIndex && this.Targets[targetIndex] != null)
				{
					if (this.Targets[targetIndex].gameObject != null)
					{
						GameObject gameObject = this.Targets[targetIndex].gameObject.FindInChildren(Sequence.s_defaultHitAttachJoint, 0);
						if (gameObject != null)
						{
							return gameObject.transform.position + Vector3.up;
						}
						gameObject = this.Targets[targetIndex].gameObject.FindInChildren(Sequence.s_defaultFallbackHitAttachJoint, 0);
						if (gameObject != null)
						{
							return gameObject.transform.position + Vector3.up;
						}
						return this.Targets[targetIndex].gameObject.transform.position;
					}
				}
			}
		}
		if (this.TargetSquare != null)
		{
			Vector3 a = this.TargetSquare.ToVector3();
			return a + Vector3.up;
		}
		return this.TargetPos;
	}

	protected bool IsHitFXVisible(ActorData hitTarget)
	{
		Sequence.HitVisibilityType hitVisibilityType = this.m_hitVisibilityType;
		bool result;
		if (hitVisibilityType != Sequence.HitVisibilityType.Always)
		{
			if (hitVisibilityType != Sequence.HitVisibilityType.Target)
			{
				result = true;
			}
			else
			{
				bool flag;
				if (hitTarget)
				{
					flag = this.IsActorConsideredVisible(hitTarget);
				}
				else
				{
					flag = false;
				}
				result = flag;
			}
		}
		else
		{
			if (hitTarget != null && this.Caster != null)
			{
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (activeOwnedActorData != null)
				{
					if (hitTarget.GetTeam() == this.Caster.GetTeam())
					{
						if (activeOwnedActorData.GetTeam() != hitTarget.GetTeam())
						{
							return this.IsActorConsideredVisible(hitTarget);
						}
					}
				}
				return true;
			}
			result = true;
		}
		return result;
	}

	public bool IsHitFXVisibleWrtTeamFilter(ActorData hitTarget, Sequence.HitVFXSpawnTeam teamFilter)
	{
		bool flag = this.IsHitFXVisible(hitTarget);
		if (flag && this.Caster != null)
		{
			if (hitTarget != null)
			{
				if (teamFilter != Sequence.HitVFXSpawnTeam.AllTargets)
				{
					bool flag2 = this.Caster.GetTeam() == hitTarget.GetTeam();
					bool flag3;
					if (teamFilter != Sequence.HitVFXSpawnTeam.AllTargets)
					{
						if (teamFilter == Sequence.HitVFXSpawnTeam.AllExcludeCaster)
						{
							if (this.Caster != hitTarget)
							{
								goto IL_D5;
							}
						}
						if (teamFilter == Sequence.HitVFXSpawnTeam.AllyAndCaster)
						{
							if (flag2)
							{
								goto IL_D5;
							}
						}
						if (teamFilter == Sequence.HitVFXSpawnTeam.EnemyOnly)
						{
							flag3 = !flag2;
						}
						else
						{
							flag3 = false;
						}
						goto IL_D6;
					}
					IL_D5:
					flag3 = true;
					IL_D6:
					flag = flag3;
				}
			}
		}
		return flag;
	}

	public bool ShouldHideForActorIfAttached(ActorData actor)
	{
		return actor != null && actor.IsModelAnimatorDisabled();
	}

	protected void InitializeFXStorage()
	{
		if (this.m_fxParent == null)
		{
			this.m_fxParent = new GameObject("fxParent_" + base.GetType().ToString());
			this.m_fxParent.transform.parent = base.transform;
		}
		if (this.m_parentedFXs == null)
		{
			this.m_parentedFXs = new List<GameObject>();
		}
	}

	protected GameObject GetFxParentObject()
	{
		return this.m_fxParent;
	}

	internal GameObject InstantiateFX(GameObject prefab)
	{
		return this.InstantiateFX(prefab, Vector3.zero, Quaternion.identity, false, true);
	}

	internal GameObject InstantiateFX(GameObject prefab, Vector3 position, Quaternion rotation, bool tryApplyCameraOffset = true, bool logErrorOnNullPrefab = true)
	{
		if (this.m_fxParent == null)
		{
			this.InitializeFXStorage();
		}
		if (tryApplyCameraOffset)
		{
			if (prefab != null)
			{
				if (prefab.GetComponent<OffsetVFXTowardsCamera>())
				{
					position = OffsetVFXTowardsCamera.ProcessOffset(position);
				}
			}
		}
		GameObject gameObject;
		if (prefab != null)
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, position, rotation);
		}
		else
		{
			gameObject = new GameObject("FallbackForNullFx");
			if (Application.isEditor)
			{
				if (logErrorOnNullPrefab)
				{
					Debug.LogError(base.gameObject.name + " Trying to instantiate null FX prefab");
				}
			}
		}
		this.ReplaceVFXPrefabs(gameObject);
		gameObject.transform.parent = this.m_fxParent.transform;
		gameObject.gameObject.SetLayerRecursively(LayerMask.NameToLayer("DynamicLit"));
		return gameObject;
	}

	private void ReplaceVFXPrefabs(GameObject vfxInstanceRoot)
	{
		GameEventManager gameEventManager = GameEventManager.Get();
		if (gameEventManager != null)
		{
			if (this.Caster != null)
			{
				gameEventManager.FireEvent(GameEventManager.EventType.ReplaceVFXPrefab, new GameEventManager.ReplaceVFXPrefab
				{
					characterResourceLink = this.Caster.GetCharacterResourceLink(),
					characterVisualInfo = this.Caster.m_visualInfo,
					characterAbilityVfxSwapInfo = this.Caster.m_abilityVfxSwapInfo,
					vfxRoot = vfxInstanceRoot.transform
				});
			}
		}
	}

	internal static void SetAttribute(GameObject fx, string name, int value)
	{
		if (fx != null)
		{
			PKFxFX[] componentsInChildren = fx.GetComponentsInChildren<PKFxFX>(true);
			foreach (PKFxFX pkfxFX in componentsInChildren)
			{
				if (pkfxFX != null)
				{
					PKFxManager.AttributeDesc desc = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Int, name);
					if (pkfxFX.AttributeExists(desc))
					{
						pkfxFX.SetAttribute(new PKFxManager.Attribute(desc)
						{
							m_Value0 = (float)value
						});
					}
				}
			}
		}
	}

	internal static void SetAttribute(GameObject fx, string name, float value)
	{
		if (fx != null)
		{
			PKFxFX[] componentsInChildren = fx.GetComponentsInChildren<PKFxFX>(true);
			foreach (PKFxFX pkfxFX in componentsInChildren)
			{
				if (pkfxFX != null)
				{
					PKFxManager.AttributeDesc desc = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Float, name);
					if (pkfxFX.AttributeExists(desc))
					{
						pkfxFX.SetAttribute(new PKFxManager.Attribute(desc)
						{
							m_Value0 = value
						});
					}
				}
			}
		}
	}

	internal static void SetAttribute(GameObject fx, string name, Vector3 value)
	{
		if (fx != null)
		{
			PKFxFX[] componentsInChildren = fx.GetComponentsInChildren<PKFxFX>(true);
			foreach (PKFxFX pkfxFX in componentsInChildren)
			{
				if (pkfxFX != null)
				{
					PKFxManager.AttributeDesc desc = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Float3, name);
					if (pkfxFX.AttributeExists(desc))
					{
						pkfxFX.SetAttribute(new PKFxManager.Attribute(desc)
						{
							m_Value0 = value.x,
							m_Value1 = value.y,
							m_Value2 = value.z
						});
					}
				}
			}
		}
	}

	internal bool AreFXFinished(GameObject fx)
	{
		if (!this.Source.RemoveAtEndOfTurn)
		{
			return false;
		}
		bool result = false;
		if (fx != null)
		{
			if (!fx.activeSelf)
			{
				result = true;
			}
			else
			{
				if (fx.GetComponent<ParticleSystem>() != null)
				{
					if (!fx.GetComponent<ParticleSystem>().IsAlive())
					{
						return true;
					}
				}
				PKFxFX[] componentsInChildren = fx.GetComponentsInChildren<PKFxFX>(true);
				if (componentsInChildren.Length > 0)
				{
					result = true;
					foreach (PKFxFX pkfxFX in componentsInChildren)
					{
						if (pkfxFX.Alive())
						{
							return false;
						}
					}
				}
			}
		}
		return result;
	}

	internal static float GetFXDuration(GameObject fxPrefab)
	{
		float result = 1f;
		if (fxPrefab != null)
		{
			ParticleSystem component = fxPrefab.GetComponent<ParticleSystem>();
			if (component != null)
			{
				result = component.main.duration;
			}
			else
			{
				PKFxManager.AttributeDesc attributeDesc = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Float, "Duration");
				attributeDesc.DefaultValue0 = 1f;
				PKFxFX[] componentsInChildren = fxPrefab.GetComponentsInChildren<PKFxFX>(true);
				if (componentsInChildren.Length > 0)
				{
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						if (componentsInChildren[i] != null && componentsInChildren[i].AttributeExists(attributeDesc))
						{
							result = Mathf.Max(new float[]
							{
								componentsInChildren[i].GetAttributeFromDesc(attributeDesc).ValueFloat
							});
						}
					}
				}
			}
		}
		return result;
	}

	public static GameObject SpawnAndAttachFx(Sequence sequence, GameObject fxPrefab, ActorData targetActor, JointPopupProperty fxJoint, bool attachToJoint, bool aimAtCaster, bool reverseDir)
	{
		GameObject gameObject = null;
		if (targetActor != null)
		{
			if (!fxJoint.IsInitialized())
			{
				fxJoint.Initialize(targetActor.gameObject);
			}
			if (fxPrefab != null)
			{
				if (fxJoint.m_jointObject != null)
				{
					if (fxJoint.m_jointObject.transform.localScale != Vector3.zero)
					{
						if (attachToJoint)
						{
							gameObject = sequence.InstantiateFX(fxPrefab);
							sequence.AttachToBone(gameObject, fxJoint.m_jointObject);
							gameObject.transform.localPosition = Vector3.zero;
							gameObject.transform.localRotation = Quaternion.identity;
							Quaternion rotation = default(Quaternion);
							if (aimAtCaster)
							{
								Vector3 position = sequence.Caster.transform.position;
								Vector3 vector = position - fxJoint.m_jointObject.transform.position;
								vector.y = 0f;
								vector.Normalize();
								if (reverseDir)
								{
									vector *= -1f;
								}
								rotation.SetLookRotation(vector);
								gameObject.transform.rotation = rotation;
							}
							return gameObject;
						}
					}
				}
				Vector3 position2 = fxJoint.m_jointObject.transform.position;
				Quaternion rotation2 = default(Quaternion);
				if (aimAtCaster)
				{
					Vector3 position3 = sequence.Caster.transform.position;
					Vector3 vector2 = position3 - position2;
					vector2.y = 0f;
					vector2.Normalize();
					if (reverseDir)
					{
						vector2 *= -1f;
					}
					rotation2.SetLookRotation(vector2);
				}
				else
				{
					rotation2 = fxJoint.m_jointObject.transform.rotation;
				}
				gameObject = sequence.InstantiateFX(fxPrefab, position2, rotation2, true, true);
				Sequence.SetAttribute(gameObject, "abilityAreaLength", (sequence.TargetPos - position2).magnitude);
			}
		}
		return gameObject;
	}

	internal void AttachToBone(GameObject fx, GameObject parent)
	{
		if (this.m_parentedFXs == null)
		{
			this.InitializeFXStorage();
		}
		GameObject gameObject = new GameObject();
		gameObject.transform.parent = parent.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localRotation = Quaternion.identity;
		fx.transform.parent = gameObject.transform;
		this.m_parentedFXs.Add(gameObject);
	}

	internal static ActorModelData.ImpulseInfo CreateImpulseInfoWithObjectPose(GameObject obj)
	{
		if (obj != null)
		{
			return new ActorModelData.ImpulseInfo(obj.transform.position, obj.transform.forward);
		}
		return null;
	}

	internal static ActorModelData.ImpulseInfo CreateImpulseInfoWithActorForward(ActorData actor)
	{
		if (actor != null)
		{
			return new ActorModelData.ImpulseInfo(actor.transform.position, Vector3.up + actor.transform.forward);
		}
		return null;
	}

	internal static ActorModelData.ImpulseInfo CreateImpulseInfoBetweenActors(ActorData fromActor, ActorData targetActor)
	{
		if (fromActor != null)
		{
			if (targetActor != null)
			{
				if (fromActor == targetActor)
				{
					return Sequence.CreateImpulseInfoWithActorForward(fromActor);
				}
				return new ActorModelData.ImpulseInfo(targetActor.transform.position, targetActor.transform.position - fromActor.transform.position);
			}
		}
		return null;
	}

	public override string ToString()
	{
		string format = "[Sequence: {0}, Object: {1}, id: {2}, initialized: {3}, enabled: {4}, MarkedForRemoval: {5}, Caster: {6}]";
		object[] array = new object[7];
		array[0] = base.GetType().ToString();
		array[1] = base.gameObject.name;
		array[2] = this.Id;
		array[3] = this.m_initialized;
		array[4] = base.enabled;
		array[5] = this.MarkedForRemoval;
		int num = 6;
		object obj;
		if (this.Caster == null)
		{
			obj = "NULL";
		}
		else
		{
			obj = this.Caster.ToString();
		}
		array[num] = obj;
		return string.Format(format, array);
	}

	public string GetTargetsString()
	{
		string text = string.Empty;
		if (this.Targets != null)
		{
			if (this.Targets.Length > 0)
			{
				for (int i = 0; i < this.Targets.Length; i++)
				{
					ActorData actorData = this.Targets[i];
					if (actorData != null)
					{
						if (text.Length > 0)
						{
							text += " | ";
						}
						text += actorData.ActorIndex.ToString();
					}
				}
			}
		}
		return text;
	}

	public void OverridePhaseTimingParams(Sequence.PhaseTimingParameters timingParams, Sequence.IExtraSequenceParams iParams)
	{
		if (iParams != null && iParams is Sequence.PhaseTimingExtraParams)
		{
			Sequence.PhaseTimingExtraParams phaseTimingExtraParams = iParams as Sequence.PhaseTimingExtraParams;
			if (phaseTimingExtraParams != null)
			{
				if (timingParams != null && timingParams.m_acceptOverrideFromParams)
				{
					if ((int)phaseTimingExtraParams.m_turnDelayStartOverride >= 0)
					{
						timingParams.m_turnDelayStart = (int)phaseTimingExtraParams.m_turnDelayStartOverride;
					}
					if ((int)phaseTimingExtraParams.m_turnDelayEndOverride >= 0)
					{
						timingParams.m_turnDelayEnd = (int)phaseTimingExtraParams.m_turnDelayEndOverride;
					}
					if ((int)phaseTimingExtraParams.m_abilityPhaseStartOverride >= 0)
					{
						timingParams.m_usePhaseStartTiming = true;
						timingParams.m_abilityPhaseStart = (AbilityPriority)phaseTimingExtraParams.m_abilityPhaseStartOverride;
					}
					if ((int)phaseTimingExtraParams.m_abilityPhaseEndOverride >= 0)
					{
						timingParams.m_usePhaseEndTiming = true;
						timingParams.m_abilityPhaseEnd = (AbilityPriority)phaseTimingExtraParams.m_abilityPhaseEndOverride;
					}
				}
			}
		}
	}

	public string GetInEditorDescription()
	{
		string text = this.m_setupNotes.m_notes;
		if (string.IsNullOrEmpty(text))
		{
			text = "<empty>";
		}
		string str = "Setup Note: " + text + "\n----------\n";
		if (this.m_targetHitAnimation)
		{
			str += "<[x] Target Hit Animation> Can trigger hit react anim\n\n";
			if (this.m_canTriggerHitReactOnAllyHit)
			{
				str += "Can trigger hit react on ally hit\n\n";
			}
		}
		str += this.GetVisibilityDescription();
		return str + "\n<color=white>--Sequence Specific--</color>\n" + this.GetSequenceSpecificDescription();
	}

	public virtual string GetVisibilityDescription()
	{
		string text = string.Empty;
		if (this.m_visibilityType == Sequence.VisibilityType.Always)
		{
			text += "<color=yellow>WARNING: </color>VisibilityType is set to be always visible. Ignore if that is intended.\n";
		}
		return text;
	}

	public virtual string GetSequenceSpecificDescription()
	{
		return "NO SEQUENCE SPECIFIC DESCRIPTION IMPLEMENTED YET T_T";
	}

	public unsafe static string GetVisibilityTypeDescription(Sequence.VisibilityType visType, out bool usesTargetPos, out bool usesSeqPos)
	{
		string result = "UNKNOWN";
		usesTargetPos = false;
		usesSeqPos = false;
		if (visType == Sequence.VisibilityType.Always)
		{
			result = "always visible";
		}
		else if (visType == Sequence.VisibilityType.Caster)
		{
			result = "visible if " + Sequence.c_casterToken + " is visible";
		}
		else if (visType == Sequence.VisibilityType.CasterOrTarget)
		{
			result = string.Concat(new string[]
			{
				"visible if either ",
				Sequence.c_casterToken,
				" or any ",
				Sequence.c_targetActorToken,
				" is visible"
			});
		}
		else if (visType == Sequence.VisibilityType.CasterOrTargetPos)
		{
			result = string.Concat(new string[]
			{
				"visible if either ",
				Sequence.c_casterToken,
				" visible or ",
				Sequence.c_targetPosToken,
				" square visible"
			});
			usesTargetPos = true;
		}
		else if (visType == Sequence.VisibilityType.CasterOrTargetOrTargetPos)
		{
			result = string.Concat(new string[]
			{
				"visible if either ",
				Sequence.c_casterToken,
				" or any ",
				Sequence.c_targetActorToken,
				" visible, or ",
				Sequence.c_seqPosToken,
				" square is visible"
			});
			usesTargetPos = true;
		}
		else if (visType == Sequence.VisibilityType.Target)
		{
			result = "visible if any " + Sequence.c_targetActorToken + " is visible";
		}
		else if (visType == Sequence.VisibilityType.TargetPos)
		{
			result = "visible if " + Sequence.c_targetPosToken + " square is visible";
			usesTargetPos = true;
		}
		else if (visType == Sequence.VisibilityType.AlwaysOnlyIfCaster)
		{
			result = string.Concat(new string[]
			{
				"visible only if ",
				Sequence.c_casterToken,
				" is current ",
				Sequence.c_clientActorToken,
				" that player controls"
			});
		}
		else if (visType == Sequence.VisibilityType.AlwaysOnlyIfTarget)
		{
			result = string.Concat(new string[]
			{
				"visible only if first ",
				Sequence.c_targetActorToken,
				" is current ",
				Sequence.c_clientActorToken,
				" that player controls"
			});
		}
		else if (visType == Sequence.VisibilityType.AlwaysIfCastersTeam)
		{
			result = string.Concat(new string[]
			{
				"visible if ",
				Sequence.c_casterToken,
				" is on same team as current ",
				Sequence.c_clientActorToken,
				" that player controls"
			});
		}
		else if (visType == Sequence.VisibilityType.SequencePosition)
		{
			result = "visible if " + Sequence.c_seqPosToken + " square is visible\n(ex. for most projectiles)";
			usesSeqPos = true;
		}
		else if (visType == Sequence.VisibilityType.CastersTeamOrSequencePosition)
		{
			result = string.Concat(new string[]
			{
				"visible if ",
				Sequence.c_casterToken,
				" is on same team as current ",
				Sequence.c_clientActorToken,
				" that player controls, OR ",
				Sequence.c_seqPosToken,
				" square is visible\n(ex. for projectile that should always be visible for allies but only visible if projectile position is visible for enemies)"
			});
			usesSeqPos = true;
		}
		else if (visType == Sequence.VisibilityType.TargetPosAndCaster)
		{
			result = string.Concat(new string[]
			{
				"visible if ",
				Sequence.c_casterToken,
				" is visible AND ",
				Sequence.c_targetPosToken,
				" square is visible\n(ex. for Flash catalyst while stealthed)"
			});
			usesTargetPos = true;
		}
		return result;
	}

	[Serializable]
	public class PhaseTimingParameters
	{
		public bool m_usePhaseStartTiming;

		public int m_turnDelayStart;

		public AbilityPriority m_abilityPhaseStart;

		public bool m_usePhaseEndTiming;

		public int m_turnDelayEnd;

		public AbilityPriority m_abilityPhaseEnd;

		[Header("-- Whether this sequence component accept ability params to override phase timing params --")]
		public bool m_acceptOverrideFromParams;

		internal int m_startTurn;

		internal bool m_started;

		internal bool m_finished;

		internal void OnTurnStart(int currentTurn)
		{
			this.m_turnDelayStart--;
			this.m_turnDelayEnd--;
			if (this.m_usePhaseEndTiming && !this.m_finished && this.m_turnDelayEnd < 0)
			{
				if (this.m_abilityPhaseEnd != AbilityPriority.INVALID)
				{
					this.m_finished = true;
				}
			}
		}

		internal void OnAbilityPhaseStart(AbilityPriority abilityPhase)
		{
			if (this.m_turnDelayStart <= 0)
			{
				if (abilityPhase == this.m_abilityPhaseStart)
				{
					this.m_started = true;
				}
			}
			if (this.m_turnDelayEnd <= 0)
			{
				if (abilityPhase == this.m_abilityPhaseEnd)
				{
					this.m_finished = true;
				}
			}
		}

		internal bool ShouldSequenceBeActive()
		{
			if (!this.m_started)
			{
				if (this.m_usePhaseStartTiming)
				{
					return false;
				}
			}
			bool result;
			if (this.m_finished)
			{
				result = !this.m_usePhaseEndTiming;
			}
			else
			{
				result = true;
			}
			return result;
		}

		internal bool ShouldSpawnSequence(AbilityPriority abilityPhase)
		{
			bool result = false;
			if (this.m_turnDelayStart == 0 && abilityPhase == this.m_abilityPhaseStart)
			{
				if (this.m_usePhaseStartTiming)
				{
					result = true;
				}
			}
			return result;
		}

		internal bool ShouldStopSequence(AbilityPriority abilityPhase)
		{
			bool result = false;
			if (this.m_turnDelayEnd == 0)
			{
				if (abilityPhase == this.m_abilityPhaseEnd)
				{
					if (this.m_usePhaseEndTiming)
					{
						result = true;
					}
				}
			}
			return result;
		}
	}

	[Serializable]
	public class SequenceNotes
	{
		[TextArea(1, 5)]
		public string m_notes;
	}

	public enum ReferenceModelType
	{
		Actor,
		TempSatellite,
		PersistentSatellite1
	}

	public enum VisibilityType
	{
		Always,
		Caster,
		CasterOrTarget,
		CasterOrTargetPos,
		CasterOrTargetOrTargetPos,
		Target,
		TargetPos,
		AlwaysOnlyIfCaster,
		AlwaysOnlyIfTarget,
		AlwaysIfCastersTeam,
		SequencePosition,
		CastersTeamOrSequencePosition,
		TargetPosAndCaster,
		CasterIfNotEvading,
		TargetIfNotEvading
	}

	public enum HitVisibilityType
	{
		Always,
		Target
	}

	public enum SequenceHideType
	{
		Default_DisableObject,
		MoveOffCamera_KeepEnabled,
		KillThenDisable
	}

	public enum PhaseBasedVisibilityType
	{
		Any,
		InDecisionOnly,
		InResolutionOnly
	}

	public enum HitVFXSpawnTeam
	{
		AllTargets,
		AllyAndCaster,
		EnemyOnly,
		AllExcludeCaster
	}

	public abstract class IExtraSequenceParams
	{
		public abstract void XSP_SerializeToStream(IBitStream stream);

		public abstract void XSP_DeserializeFromStream(IBitStream stream);

		public Sequence.IExtraSequenceParams[] ToArray()
		{
			return new Sequence.IExtraSequenceParams[]
			{
				this
			};
		}
	}

	public class PhaseTimingExtraParams : Sequence.IExtraSequenceParams
	{
		public sbyte m_turnDelayStartOverride = -1;

		public sbyte m_turnDelayEndOverride = -1;

		public sbyte m_abilityPhaseStartOverride = -1;

		public sbyte m_abilityPhaseEndOverride = -1;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref this.m_turnDelayStartOverride);
			stream.Serialize(ref this.m_turnDelayEndOverride);
			stream.Serialize(ref this.m_abilityPhaseStartOverride);
			stream.Serialize(ref this.m_abilityPhaseEndOverride);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref this.m_turnDelayStartOverride);
			stream.Serialize(ref this.m_turnDelayEndOverride);
			stream.Serialize(ref this.m_abilityPhaseStartOverride);
			stream.Serialize(ref this.m_abilityPhaseEndOverride);
		}
	}

	public class FxAttributeParam : Sequence.IExtraSequenceParams
	{
		public Sequence.FxAttributeParam.ParamNameCode m_paramNameCode;

		public Sequence.FxAttributeParam.ParamTarget m_paramTarget;

		public float m_paramValue;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			sbyte b = (sbyte)this.m_paramNameCode;
			sbyte b2 = (sbyte)this.m_paramTarget;
			stream.Serialize(ref b);
			stream.Serialize(ref b2);
			stream.Serialize(ref this.m_paramValue);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			sbyte b = 0;
			sbyte b2 = 0;
			stream.Serialize(ref b);
			stream.Serialize(ref b2);
			stream.Serialize(ref this.m_paramValue);
			this.m_paramNameCode = (Sequence.FxAttributeParam.ParamNameCode)b;
			this.m_paramTarget = (Sequence.FxAttributeParam.ParamTarget)b2;
		}

		public string GetAttributeName()
		{
			if (this.m_paramNameCode == Sequence.FxAttributeParam.ParamNameCode.ScaleControl)
			{
				return "scaleControl";
			}
			if (this.m_paramNameCode == Sequence.FxAttributeParam.ParamNameCode.LengthInSquares)
			{
				return "lengthInSquares";
			}
			if (this.m_paramNameCode == Sequence.FxAttributeParam.ParamNameCode.WidthInSquares)
			{
				return "widthInSquares";
			}
			if (this.m_paramNameCode == Sequence.FxAttributeParam.ParamNameCode.AbilityAreaLength)
			{
				return "abilityAreaLength";
			}
			return string.Empty;
		}

		public void SetValues(Sequence.FxAttributeParam.ParamTarget paramTarget, Sequence.FxAttributeParam.ParamNameCode nameCode, float value)
		{
			this.m_paramTarget = paramTarget;
			this.m_paramNameCode = nameCode;
			this.m_paramValue = value;
		}

		public enum ParamNameCode
		{
			None,
			ScaleControl,
			LengthInSquares,
			WidthInSquares,
			AbilityAreaLength
		}

		public enum ParamTarget
		{
			None,
			MainVfx,
			ImpactVfx
		}
	}

	public class ActorIndexExtraParam : Sequence.IExtraSequenceParams
	{
		public short m_actorIndex = -1;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref this.m_actorIndex);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref this.m_actorIndex);
		}
	}

	public class GenericIntParam : Sequence.IExtraSequenceParams
	{
		public Sequence.GenericIntParam.FieldIdentifier m_fieldIdentifier;

		public short m_value;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			sbyte b = (sbyte)this.m_fieldIdentifier;
			stream.Serialize(ref b);
			stream.Serialize(ref this.m_value);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			sbyte b = 0;
			stream.Serialize(ref b);
			stream.Serialize(ref this.m_value);
			this.m_fieldIdentifier = (Sequence.GenericIntParam.FieldIdentifier)b;
		}

		public enum FieldIdentifier
		{
			None,
			Index
		}
	}

	public class GenericActorListParam : Sequence.IExtraSequenceParams
	{
		public List<ActorData> m_actors;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			List<ActorData> actors = this.m_actors;
			sbyte b;
			if (actors != null)
			{
				b = (sbyte)actors.Count;
			}
			else
			{
				b = 0;
			}
			sbyte b2 = b;
			stream.Serialize(ref b2);
			for (int i = 0; i < (int)b2; i++)
			{
				ActorData actorData = actors[i];
				sbyte b3;
				if (actorData != null)
				{
					b3 = (sbyte)actorData.ActorIndex;
				}
				else
				{
					b3 = (sbyte)ActorData.s_invalidActorIndex;
				}
				sbyte b4 = b3;
				stream.Serialize(ref b4);
			}
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			sbyte b = 0;
			stream.Serialize(ref b);
			this.m_actors = new List<ActorData>((int)b);
			for (int i = 0; i < (int)b; i++)
			{
				sbyte b2 = (sbyte)ActorData.s_invalidActorIndex;
				stream.Serialize(ref b2);
				ActorData item = GameFlowData.Get().FindActorByActorIndex((int)b2);
				this.m_actors.Add(item);
			}
		}
	}
}
