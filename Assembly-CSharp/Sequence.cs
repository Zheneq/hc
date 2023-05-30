using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class Sequence : MonoBehaviour
{
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
			m_turnDelayStart--;
			m_turnDelayEnd--;
			if (m_usePhaseEndTiming
			    && !m_finished
			    && m_turnDelayEnd < 0
			    && m_abilityPhaseEnd != AbilityPriority.INVALID)
			{
				m_finished = true;
			}
		}

		internal void OnAbilityPhaseStart(AbilityPriority abilityPhase)
		{
			if (m_turnDelayStart <= 0 && abilityPhase == m_abilityPhaseStart)
			{
				m_started = true;
			}
			if (m_turnDelayEnd <= 0 && abilityPhase == m_abilityPhaseEnd)
			{
				m_finished = true;
			}
		}

		internal bool ShouldSequenceBeActive()
		{
			return (m_started || !m_usePhaseStartTiming)
			       && (!m_finished || !m_usePhaseEndTiming);
		}

		internal bool ShouldSpawnSequence(AbilityPriority abilityPhase)
		{
			return m_turnDelayStart == 0
			       && abilityPhase == m_abilityPhaseStart
			       && m_usePhaseStartTiming;
		}

		internal bool ShouldStopSequence(AbilityPriority abilityPhase)
		{
			return m_turnDelayEnd == 0
			       && abilityPhase == m_abilityPhaseEnd
			       && m_usePhaseEndTiming;
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

		public IExtraSequenceParams[] ToArray()
		{
			return new[] { this };
		}
	}

	public class PhaseTimingExtraParams : IExtraSequenceParams
	{
		public sbyte m_turnDelayStartOverride = -1;
		public sbyte m_turnDelayEndOverride = -1;
		public sbyte m_abilityPhaseStartOverride = -1;
		public sbyte m_abilityPhaseEndOverride = -1;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref m_turnDelayStartOverride);
			stream.Serialize(ref m_turnDelayEndOverride);
			stream.Serialize(ref m_abilityPhaseStartOverride);
			stream.Serialize(ref m_abilityPhaseEndOverride);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref m_turnDelayStartOverride);
			stream.Serialize(ref m_turnDelayEndOverride);
			stream.Serialize(ref m_abilityPhaseStartOverride);
			stream.Serialize(ref m_abilityPhaseEndOverride);
		}
	}

	public class FxAttributeParam : IExtraSequenceParams
	{
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

		public ParamNameCode m_paramNameCode;
		public ParamTarget m_paramTarget;
		public float m_paramValue;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			sbyte paramNameCode = (sbyte)m_paramNameCode;
			sbyte paramTarget = (sbyte)m_paramTarget;
			stream.Serialize(ref paramNameCode);
			stream.Serialize(ref paramTarget);
			stream.Serialize(ref m_paramValue);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			sbyte paramNameCode = 0;
			sbyte paramTarget = 0;
			stream.Serialize(ref paramNameCode);
			stream.Serialize(ref paramTarget);
			stream.Serialize(ref m_paramValue);
			m_paramNameCode = (ParamNameCode)paramNameCode;
			m_paramTarget = (ParamTarget)paramTarget;
		}

		public string GetAttributeName()
		{
			switch (m_paramNameCode)
			{
				case ParamNameCode.ScaleControl:
					return "scaleControl";
				case ParamNameCode.LengthInSquares:
					return "lengthInSquares";
				case ParamNameCode.WidthInSquares:
					return "widthInSquares";
				case ParamNameCode.AbilityAreaLength:
					return "abilityAreaLength";
				default:
					return string.Empty;
			}
		}

		public void SetValues(ParamTarget paramTarget, ParamNameCode nameCode, float value)
		{
			m_paramTarget = paramTarget;
			m_paramNameCode = nameCode;
			m_paramValue = value;
		}
	}

	public class ActorIndexExtraParam : IExtraSequenceParams
	{
		public short m_actorIndex = -1;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref m_actorIndex);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref m_actorIndex);
		}
	}

	public class GenericIntParam : IExtraSequenceParams
	{
		public enum FieldIdentifier
		{
			None,
			Index
		}

		public FieldIdentifier m_fieldIdentifier;
		public short m_value;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			sbyte fieldIdentifier = (sbyte)m_fieldIdentifier;
			stream.Serialize(ref fieldIdentifier);
			stream.Serialize(ref m_value);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			sbyte fieldIdentifier = 0;
			stream.Serialize(ref fieldIdentifier);
			stream.Serialize(ref m_value);
			m_fieldIdentifier = (FieldIdentifier)fieldIdentifier;
		}
	}

	public class GenericActorListParam : IExtraSequenceParams
	{
		public List<ActorData> m_actors;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			sbyte numActors = m_actors != null
				? (sbyte)m_actors.Count
				: (sbyte)0;
			stream.Serialize(ref numActors);
			for (int i = 0; i < numActors; i++)
			{
				ActorData actorData = m_actors[i];
				sbyte actorIndex = actorData != null
					? (sbyte)actorData.ActorIndex
					: (sbyte)ActorData.s_invalidActorIndex;
				stream.Serialize(ref actorIndex);
			}
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			sbyte numActors = 0;
			stream.Serialize(ref numActors);
			m_actors = new List<ActorData>(numActors);
			for (int i = 0; i < numActors; i++)
			{
				sbyte actorIndex = (sbyte)ActorData.s_invalidActorIndex;
				stream.Serialize(ref actorIndex);
				ActorData actor = GameFlowData.Get().FindActorByActorIndex(actorIndex);
				m_actors.Add(actor);
			}
		}
	}

	public SequenceNotes m_setupNotes;
	[Separator("For Hit React Animation")]
	public bool m_targetHitAnimation = true;
	public bool m_canTriggerHitReactOnAllyHit;
	public string m_customHitReactTriggerName = string.Empty;
	[Separator("Visibility (please don't forget me T_T ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~)")]
	[Tooltip("What visibility rules to use for this sequence")]
	public VisibilityType m_visibilityType;
	[Tooltip("What visibility rules to use for the hitFX")]
	public HitVisibilityType m_hitVisibilityType;
	public PhaseBasedVisibilityType m_phaseVisibilityType;
	[Separator("How to Hide Vfx Object")]
	public SequenceHideType m_sequenceHideType;
	[Space(5f)]
	public bool m_turnOffVFXDuringCinematicCam = true;
	[Separator("For keeping VFX in Caster Taunts")]
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
	
	private static IExtraSequenceParams[] s_emptyParams = new IExtraSequenceParams[0];

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

	public short PrefabLookupId { get; private set; }

	internal ActorData[] Targets { get; set; }

	internal ActorData Target
	{
		get
		{
			if (Targets == null || Targets.Length == 0)
			{
				return null;
			}
			return Targets[0];
		}
	}

	internal ActorData Caster
	{
		get => m_caster;
		set
		{
			m_caster = value;
			if (value != null)
			{
				m_casterId = value.ActorIndex;
			}
		}
	}

	internal BoardSquare TargetSquare
	{
		get => m_targetBoardSquare;
		set => m_targetBoardSquare = value;
	}

	internal Vector3 TargetPos { get; set; }
	internal Quaternion TargetRotation { get; set; }
	internal int AgeInTurns { get; set; }
	internal bool Ready => m_initialized && enabled && !MarkedForRemoval;
	internal bool InitializedEver { get; private set; }
	internal bool MarkedForRemoval { get; private set; }
	internal bool RemoveAtTurnEnd { get; set; }
	internal int Id { get; set; }
	internal SequenceSource Source { get; private set; }

	public bool HasReceivedAnimEventBeforeReady
	{
		get => m_debugHasReceivedAnimEventBeforeReady;
		set => m_debugHasReceivedAnimEventBeforeReady = value;
	}

	public GameObject GetReferenceModel(ActorData referenceActorData, ReferenceModelType referenceModelType)
	{
		switch (referenceModelType)
		{
			case ReferenceModelType.Actor:
			{
				if (referenceActorData != null)
				{
					return referenceActorData.gameObject;
				}
				break;
			}
			case ReferenceModelType.TempSatellite:
				return SequenceManager.Get().FindTempSatellite(Source);
			case ReferenceModelType.PersistentSatellite1:
			{
				if (referenceActorData != null)
				{
					SatelliteController component = referenceActorData.GetComponent<SatelliteController>();
					if (component != null && component.GetSatellite(0) != null)
					{
						return component.GetSatellite(0).gameObject;
					}
				}

				break;
			}
		}
		return null;
	}

	public void InitPrefabLookupId(short lookupId)
	{
		PrefabLookupId = lookupId;
	}

	internal void MarkForRemoval()
	{
		MarkedForRemoval = true;
		enabled = false;
	}

	internal static void MarkSequenceArrayForRemoval(Sequence[] sequencesArray)
	{
		if (sequencesArray != null)
		{
			foreach (Sequence seq in sequencesArray)
			{
				if (seq != null)
				{
					seq.MarkForRemoval();
				}
			}
		}
	}

	internal bool RequestsHitAnimation(ActorData target)
	{
		return m_targetHitAnimation
		       && target != null
		       && Caster != null
		       && (m_canTriggerHitReactOnAllyHit || Caster.GetEnemyTeam() == target.GetTeam());
	}

	protected virtual void Start()
	{
		m_startTime = GameTime.time;
	}

	internal void OnDoClientEnable()
	{
		if (!m_waitForClientEnable)
		{
			return;
		}
		FinishSetup();
		DoneInitialization();
		ProcessSequenceVisibility();
		if (SequenceManager.SequenceDebugTraceOn)
		{
			Debug.LogWarning($"<color=yellow>Client Enable: </color><<color=lightblue>{gameObject.name} | {GetType()}</color>> @time= {GameTime.time}");
		}
		enabled = true;
	}

	protected virtual void OnDestroy()
	{
		if (m_parentedFXs != null)
		{
			foreach (GameObject fx in m_parentedFXs)
			{
				Destroy(fx);
			}
		}
		if (SequenceManager.Get() != null)
		{
			SequenceManager.Get().OnDestroySequence(this);
		}
	}

	private void DoneInitialization()
	{
		if (InitializedEver)
		{
			return;
		}
		m_initialized = true;
		InitializedEver = true;
	}

	internal virtual void Initialize(IExtraSequenceParams[] extraParams)
	{
	}

	public virtual void FinishSetup()
	{
	}

	internal void BaseInitialize_Client(
		BoardSquare targetSquare,
		Vector3 targetPos,
		Quaternion targetRotation,
		ActorData[] targets,
		ActorData caster,
		int id,
		GameObject prefab,
		short baseSequenceLookupId,
		SequenceSource source,
		IExtraSequenceParams[] extraParams)
	{
		RemoveAtTurnEnd = source.RemoveAtEndOfTurn;
		TargetSquare = targetSquare;
		TargetPos = targetPos;
		TargetRotation = targetRotation;
		Targets = targets;
		Caster = caster;
		Id = id;
		Source = source;
		InitPrefabLookupId(baseSequenceLookupId);
		m_startTime = GameTime.time;
		Initialize(extraParams ?? s_emptyParams);
		m_waitForClientEnable = Source.WaitForClientEnable;
		enabled = !m_waitForClientEnable;
		if (!m_waitForClientEnable)
		{
			FinishSetup();
			DoneInitialization();
		}
	}

	protected virtual void OnStopVfxOnClient()
	{
	}

	private void OnActorAdded(ActorData actor)
	{
		if (InitializedEver
		    || !enabled
		    || MarkedForRemoval
		    || m_caster != null
		    || m_casterId == ActorData.s_invalidActorIndex)
		{
			return;
		}
		
		m_caster = GameFlowData.Get().FindActorByActorIndex(m_casterId);
		if (m_caster != null)
		{
			FinishSetup();
			DoneInitialization();
			GameFlowData.s_onAddActor -= OnActorAdded;
		}
	}

	internal bool IsCasterOrTargetsVisible()
	{
		if (IsActorConsideredVisible(Caster))
		{
			return true;
		}
		if (Targets == null)
		{
			return false;
		}
		foreach (ActorData target in Targets)
		{
			if (IsActorConsideredVisible(target))
			{
				return true;
			}
		}
		return false;
	}

	internal virtual Vector3 GetSequencePos()
	{
		return TargetPos;
	}

	internal bool IsSequencePosVisible()
	{
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (clientFog == null)
		{
			return true;
		}
		if (GetSequencePos().magnitude <= 0f)
		{
			return false;
		}
		return !Board.Get().m_showLOS 
		       || clientFog.IsVisible(Board.Get().GetSquareFromVec3(GetSequencePos()));
	}

	internal bool IsTargetPosVisible()
	{
		FogOfWar clientFog = FogOfWar.GetClientFog();
		return clientFog == null || clientFog.IsVisible(Board.Get().GetSquareFromVec3(TargetPos));
	}

	internal bool IsCasterOrTargetPosVisible()
	{
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (clientFog == null)
		{
			return true;
		}
		return IsActorConsideredVisible(Caster)
		       || clientFog.IsVisible(Board.Get().GetSquareFromVec3(TargetPos));
	}

	internal bool IsCasterOrTargetOrTargetPosVisible()
	{
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (clientFog == null
		    || IsActorConsideredVisible(Caster)
		    || clientFog.IsVisible(Board.Get().GetSquareFromVec3(TargetPos)))
		{
			return true;
		}
		if (Targets != null)
		{
			foreach (ActorData actor in Targets)
			{
				if (IsActorConsideredVisible(actor))
				{
					return true;
				}
			}
		}
		return false;
	}

	protected void SetSequenceVisibility(bool visible)
	{
		m_lastSetVisibleValue = visible;
		if (m_fxParent == null || m_parentedFXs == null)
		{
			return;
		}
		if (m_sequenceHideType == SequenceHideType.MoveOffCamera_KeepEnabled)
		{
			m_fxParent.transform.localPosition = visible
				? Vector3.zero
				: -10000f * Vector3.one;
		}
		else
		{
			m_fxParent.SetActive(visible);
		}
		foreach (GameObject fx in m_parentedFXs)
		{
			if (!fx)
			{
				continue;
			}
			if (m_sequenceHideType == SequenceHideType.MoveOffCamera_KeepEnabled)
			{
				fx.transform.localPosition = visible ? Vector3.zero : -10000f * Vector3.one;
				continue;
			}
			if (m_sequenceHideType == SequenceHideType.KillThenDisable && !visible)
			{
				foreach (PKFxFX pkfx in fx.GetComponentsInChildren<PKFxFX>(true))
				{
					pkfx.KillEffect();
				}
			}
			fx.SetActive(visible);
		}
	}

	protected bool LastDesiredVisible()
	{
		return m_lastSetVisibleValue;
	}

	protected bool IsActorConsideredVisible(ActorData actor)
	{
		return actor != null
		       && actor.IsActorVisibleToClient()
		       && (actor.GetActorModelData() == null || actor.GetActorModelData().IsVisibleToClient());
	}

	protected void ProcessSequenceVisibility()
	{
		if (!m_initialized || MarkedForRemoval)
		{
			SetSequenceVisibility(false);
			return;
		}
		if (m_phaseVisibilityType != 0 && GameFlowData.Get() != null)
		{
			GameState gameState = GameFlowData.Get().gameState;
			if (m_phaseVisibilityType == PhaseBasedVisibilityType.InDecisionOnly &&
			    gameState != GameState.BothTeams_Decision
			    || m_phaseVisibilityType == PhaseBasedVisibilityType.InResolutionOnly &&
			    gameState != GameState.BothTeams_Resolve)
			{
				SetSequenceVisibility(false);
				return;
			}
		}

		bool flag = m_turnOffVFXDuringCinematicCam && CameraManager.Get() != null && CameraManager.Get().InCinematic();
		if (flag && m_keepVFXInCinematicCamForCaster && Caster != null)
		{
			ActorData cinematicTargetActor = CameraManager.Get().GetCinematicTargetActor();
			int cinematicActionAnimIndex = CameraManager.Get().GetCinematicActionAnimIndex();
			bool flag2 = m_keepCasterVFXForAnimIndex <= 0 || m_keepCasterVFXForAnimIndex == cinematicActionAnimIndex;
			bool flag3 = !m_keepCasterVFXForTurnOfSpawnOnly || AgeInTurns == 0;
			if (cinematicTargetActor.ActorIndex == Caster.ActorIndex && flag2 && flag3)
			{
				flag = false;
			}
		}
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
		if (flag)
		{
			SetSequenceVisibility(false);
			return;
		}
		if (activeOwnedActorData == null && localPlayerData == null)
		{
			SetSequenceVisibility(true);
			return;
		}
		if (m_forceAlwaysVisible)
		{
			SetSequenceVisibility(true);
			return;
		}
		switch (m_visibilityType)
		{
			case VisibilityType.Caster:
				SetSequenceVisibility(Caster == null || IsActorConsideredVisible(Caster));
				return;
			case VisibilityType.CasterOrTarget:
				SetSequenceVisibility(IsCasterOrTargetsVisible());
				return;
			case VisibilityType.CasterOrTargetPos:
				SetSequenceVisibility(IsCasterOrTargetPosVisible());
				return;
			case VisibilityType.CasterOrTargetOrTargetPos:
				SetSequenceVisibility(IsCasterOrTargetOrTargetPosVisible());
				return;
			case VisibilityType.Target:
				SetSequenceVisibility(Targets != null && Targets.Length > 0 && IsActorConsideredVisible(Targets[0]));
				return;
			case VisibilityType.TargetPos:
				SetSequenceVisibility(IsTargetPosVisible());
				return;
			case VisibilityType.AlwaysOnlyIfCaster:
				SetSequenceVisibility(Caster == activeOwnedActorData);
				return;
			case VisibilityType.AlwaysOnlyIfTarget:
				SetSequenceVisibility(Target == activeOwnedActorData);
				return;
			case VisibilityType.AlwaysIfCastersTeam:
				SetSequenceVisibility(Caster == null || activeOwnedActorData == null || Caster.GetTeam() == activeOwnedActorData.GetTeam());
				return;
			case VisibilityType.SequencePosition:
				SetSequenceVisibility(IsSequencePosVisible());
				return;
			case VisibilityType.Always:
				SetSequenceVisibility(true);
				return;
			case VisibilityType.CastersTeamOrSequencePosition:
				if (Caster != null && activeOwnedActorData != null)
				{
					SetSequenceVisibility(Caster.GetTeam() == activeOwnedActorData.GetTeam() || IsSequencePosVisible());
				}
				else
				{
					SetSequenceVisibility(IsSequencePosVisible());
				}
				return;
			case VisibilityType.TargetPosAndCaster:
				SetSequenceVisibility(IsTargetPosVisible() && (Caster == null || IsActorConsideredVisible(Caster)));
				return;
			case VisibilityType.TargetIfNotEvading:
			case VisibilityType.CasterIfNotEvading:
			{
				ActorData actorData = m_visibilityType == VisibilityType.TargetIfNotEvading
					? Targets != null && Targets.Length > 0
						? Targets[0]
						: null
					: Caster;
				SetSequenceVisibility(actorData != null
				                      && IsActorConsideredVisible(actorData)
				                      && (actorData.GetActorMovement() == null || !actorData.GetActorMovement().InChargeState()));
				break;
			}
		}
	}

	internal virtual void OnTurnStart(int currentTurn)
	{
	}

	internal virtual void OnAbilityPhaseStart(AbilityPriority abilityPhase)
	{
	}

	protected virtual void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
	}

	internal virtual void SetTimerController(int value)
	{
	}

	internal void AnimationEvent(Object eventObject, GameObject sourceObject)
	{
		if (Ready)
		{
			OnAnimationEvent(eventObject, sourceObject);
		}
	}

	protected void CallHitSequenceOnTargets(
		Vector3 impactPos,
		float defaultImpulseRadius = 1f,
		List<ActorData> actorsToIgnore = null,
		bool tryHitReactIfAlreadyHit = true)
	{
		float maxDist = 0f;
		if (Targets != null)
		{
			foreach (var target in Targets)
			{
				if (target != null)
				{
					float dist = (target.transform.position - impactPos).magnitude;
					if (dist > maxDist)
					{
						maxDist = dist;
					}
				}
			}
		}

		maxDist = maxDist < Board.Get().squareSize / 2f ? defaultImpulseRadius : maxDist;
		ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(maxDist, impactPos);
		if (Targets != null)
		{
			foreach (var target in Targets)
			{
				if (target != null && (actorsToIgnore == null || !actorsToIgnore.Contains(target)))
				{
					Source.OnSequenceHit(this, target, impulseInfo, ActorModelData.RagdollActivation.HealthBased, tryHitReactIfAlreadyHit);
				}
			}
		}
		Source.OnSequenceHit(this, TargetPos, impulseInfo);
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(
			impactPos,
			maxDist * 3f,
			false,
			Caster,
			new List<Team>
			{
				Caster.GetEnemyTeam(),
				Caster.GetTeam()
			},
			null);
		foreach (var hitActor in actorsInRadius)
		{
			if (hitActor != null && hitActor.GetActorModelData() != null)
			{
				Vector3 direction = hitActor.transform.position - impactPos;
				direction.y = 0f;
				direction.Normalize();
				hitActor.GetActorModelData().ImpartWindImpulse(direction);
			}
		}
	}

	protected Vector3 GetTargetHitPosition(ActorData actorData, JointPopupProperty fxJoint)
	{
		for (int i = 0; i < Targets.Length; i++)
		{
			if (Targets[i] == actorData)
			{
				return GetTargetHitPosition(i, fxJoint);
			}
		}
		return Vector3.zero;
	}

	protected Vector3 GetTargetHitPosition(int targetIndex, JointPopupProperty fxJoint)
	{
		bool flag = false;
		Vector3 result = Vector3.zero;
		if (Targets != null
		    && Targets.Length > targetIndex
		    && Targets[targetIndex] != null
		    && Targets[targetIndex].gameObject != null)
		{
			fxJoint.Initialize(Targets[targetIndex].gameObject);
			if (fxJoint.m_jointObject != null)
			{
				result = fxJoint.m_jointObject.transform.position;
				flag = true;
			}
		}
		if (!flag)
		{
			result = GetTargetHitPosition(targetIndex);
		}
		return result;
	}

	protected Vector3 GetTargetHitPosition(int targetIndex)
	{
		return GetTargetPosition(targetIndex, true);
	}

	protected Vector3 GetTargetPosition(int targetIndex, bool secondaryActorHits = false)
	{
		if (secondaryActorHits
		    && Targets != null
		    && Targets.Length > targetIndex
		    && Targets[targetIndex] != null
		    && Targets[targetIndex].gameObject != null)
		{
			GameObject joint = Targets[targetIndex].gameObject.FindInChildren(s_defaultHitAttachJoint);
			if (joint != null)
			{
				return joint.transform.position + Vector3.up;
			}
			joint = Targets[targetIndex].gameObject.FindInChildren(s_defaultFallbackHitAttachJoint);
			if (joint != null)
			{
				return joint.transform.position + Vector3.up;
			}
			return Targets[targetIndex].gameObject.transform.position;
		}
		if (TargetSquare != null)
		{
			return TargetSquare.ToVector3() + Vector3.up;
		}
		return TargetPos;
	}

	protected bool IsHitFXVisible(ActorData hitTarget)
	{
		if (m_hitVisibilityType == HitVisibilityType.Always)
		{
			if (hitTarget == null || Caster == null)
			{
				return true;
			}
			
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			return activeOwnedActorData == null
			       || hitTarget.GetTeam() != Caster.GetTeam()
			       || activeOwnedActorData.GetTeam() == hitTarget.GetTeam()
			       || IsActorConsideredVisible(hitTarget);
		}

		if (m_hitVisibilityType != HitVisibilityType.Target)
		{
			return true;
		}

		return hitTarget != null && IsActorConsideredVisible(hitTarget);
	}

	public bool IsHitFXVisibleWrtTeamFilter(ActorData hitTarget, HitVFXSpawnTeam teamFilter)
	{
		bool isVisible = IsHitFXVisible(hitTarget);
		if (!isVisible
		    || Caster == null
		    || hitTarget == null
		    || teamFilter == HitVFXSpawnTeam.AllTargets)
		{
			return isVisible;
		}
		bool isAlly = Caster.GetTeam() == hitTarget.GetTeam();
		return teamFilter == HitVFXSpawnTeam.AllTargets
		       || teamFilter == HitVFXSpawnTeam.AllExcludeCaster && Caster != hitTarget
		       || teamFilter == HitVFXSpawnTeam.AllyAndCaster && isAlly
		       || teamFilter == HitVFXSpawnTeam.EnemyOnly && !isAlly;
	}

	public bool ShouldHideForActorIfAttached(ActorData actor)
	{
		return actor != null && actor.IsInRagdoll();
	}

	protected void InitializeFXStorage()
	{
		if (m_fxParent == null)
		{
			m_fxParent = new GameObject("fxParent_" + GetType());
			m_fxParent.transform.parent = transform;
		}
		if (m_parentedFXs == null)
		{
			m_parentedFXs = new List<GameObject>();
		}
	}

	protected GameObject GetFxParentObject()
	{
		return m_fxParent;
	}

	internal GameObject InstantiateFX(GameObject prefab)
	{
		return InstantiateFX(prefab, Vector3.zero, Quaternion.identity, false);
	}

	internal GameObject InstantiateFX(
		GameObject prefab,
		Vector3 position,
		Quaternion rotation,
		bool tryApplyCameraOffset = true,
		bool logErrorOnNullPrefab = true)
	{
		if (m_fxParent == null)
		{
			InitializeFXStorage();
		}
		if (tryApplyCameraOffset
		    && prefab != null
		    && prefab.GetComponent<OffsetVFXTowardsCamera>() != null)
		{
			position = OffsetVFXTowardsCamera.ProcessOffset(position);
		}
		GameObject fx;
		if (prefab != null)
		{
			fx = Instantiate(prefab, position, rotation);
		}
		else
		{
			fx = new GameObject("FallbackForNullFx");
			if (Application.isEditor && logErrorOnNullPrefab)
			{
				Debug.LogError(this.gameObject.name + " Trying to instantiate null FX prefab");
			}
		}
		ReplaceVFXPrefabs(fx);
		fx.transform.parent = m_fxParent.transform;
		fx.gameObject.SetLayerRecursively(LayerMask.NameToLayer("DynamicLit"));
		return fx;
	}

	private void ReplaceVFXPrefabs(GameObject vfxInstanceRoot)
	{
		GameEventManager gameEventManager = GameEventManager.Get();
		if (gameEventManager != null && Caster != null)
		{
			GameEventManager.ReplaceVFXPrefab replaceVFXPrefab = new GameEventManager.ReplaceVFXPrefab
			{
				characterResourceLink = Caster.GetCharacterResourceLink(),
				characterVisualInfo = Caster.m_visualInfo,
				characterAbilityVfxSwapInfo = Caster.m_abilityVfxSwapInfo,
				vfxRoot = vfxInstanceRoot.transform
			};
			gameEventManager.FireEvent(GameEventManager.EventType.ReplaceVFXPrefab, replaceVFXPrefab);
		}
	}

	internal static void SetAttribute(GameObject fx, string name, int value)
	{
		if (fx == null)
		{
			return;
		}

		foreach (PKFxFX pkfx in fx.GetComponentsInChildren<PKFxFX>(true))
		{
			if (pkfx != null)
			{
				PKFxManager.AttributeDesc desc = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Int, name);
				if (pkfx.AttributeExists(desc))
				{
					pkfx.SetAttribute(new PKFxManager.Attribute(desc)
					{
						m_Value0 = value
					});
				}
			}
		}
	}

	internal static void SetAttribute(GameObject fx, string name, float value)
	{
		if (fx == null)
		{
			return;
		}

		foreach (PKFxFX pkfx in fx.GetComponentsInChildren<PKFxFX>(true))
		{
			if (pkfx != null)
			{
				PKFxManager.AttributeDesc desc = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Float, name);
				if (pkfx.AttributeExists(desc))
				{
					pkfx.SetAttribute(new PKFxManager.Attribute(desc)
					{
						m_Value0 = value
					});
				}
			}
		}
	}

	internal static void SetAttribute(GameObject fx, string name, Vector3 value)
	{
		if (fx == null)
		{
			return;
		}

		foreach (PKFxFX pkfx in fx.GetComponentsInChildren<PKFxFX>(true))
		{
			if (pkfx != null)
			{
				PKFxManager.AttributeDesc desc = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Float3, name);
				if (pkfx.AttributeExists(desc))
				{
					pkfx.SetAttribute(new PKFxManager.Attribute(desc)
					{
						m_Value0 = value.x,
						m_Value1 = value.y,
						m_Value2 = value.z
					});
				}
			}
		}
	}

	internal bool AreFXFinished(GameObject fx)
	{
		if (!Source.RemoveAtEndOfTurn || fx == null)
		{
			return false;
		}
		if (!fx.activeSelf
		    || fx.GetComponent<ParticleSystem>() != null && !fx.GetComponent<ParticleSystem>().IsAlive())
		{
			return true;
		}
		PKFxFX[] componentsInChildren = fx.GetComponentsInChildren<PKFxFX>(true);
		if (componentsInChildren.Length <= 0)
		{
			return false;
		}
		foreach (PKFxFX pkfx in componentsInChildren)
		{
			if (pkfx.Alive())
			{
				return false;
			}
		}
		return true;
	}

	internal static float GetFXDuration(GameObject fxPrefab)
	{
		float result = 1f;
		if (fxPrefab == null)
		{
			return result;
		}
		ParticleSystem component = fxPrefab.GetComponent<ParticleSystem>();
		if (component != null)
		{
			return component.main.duration;
		}
		PKFxManager.AttributeDesc attributeDesc = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Float, "Duration")
		{
			DefaultValue0 = 1f
		};
		PKFxFX[] componentsInChildren = fxPrefab.GetComponentsInChildren<PKFxFX>(true);
		if (componentsInChildren.Length > 0)
		{
			foreach (PKFxFX pkfx in componentsInChildren)
			{
				if (pkfx != null && pkfx.AttributeExists(attributeDesc))
				{
					result = Mathf.Max(pkfx.GetAttributeFromDesc(attributeDesc).ValueFloat);
				}
			}
		}
		return result;
	}

	public static GameObject SpawnAndAttachFx(
		Sequence sequence,
		GameObject fxPrefab,
		ActorData targetActor,
		JointPopupProperty fxJoint,
		bool attachToJoint,
		bool aimAtCaster,
		bool reverseDir)
	{
		if (targetActor == null)
		{
			return null;
		}
		if (!fxJoint.IsInitialized())
		{
			fxJoint.Initialize(targetActor.gameObject);
		}
		if (fxPrefab == null)
		{
			return null;
		}
		GameObject fx;
		if (fxJoint.m_jointObject != null
		    && fxJoint.m_jointObject.transform.localScale != Vector3.zero
		    && attachToJoint)
		{
			fx = sequence.InstantiateFX(fxPrefab);
			sequence.AttachToBone(fx, fxJoint.m_jointObject);
			fx.transform.localPosition = Vector3.zero;
			fx.transform.localRotation = Quaternion.identity;
			Quaternion rotation = default(Quaternion);
			if (aimAtCaster)
			{
				Vector3 position = sequence.Caster.transform.position;
				Vector3 lookRotation = position - fxJoint.m_jointObject.transform.position;
				lookRotation.y = 0f;
				lookRotation.Normalize();
				if (reverseDir)
				{
					lookRotation *= -1f;
				}
				rotation.SetLookRotation(lookRotation);
				fx.transform.rotation = rotation;
			}
		}
		else
		{
			Vector3 position = fxJoint.m_jointObject.transform.position;
			Quaternion rotation = default(Quaternion);
			if (aimAtCaster)
			{
				Vector3 casterPos = sequence.Caster.transform.position;
				Vector3 lookRotation = casterPos - position;
				lookRotation.y = 0f;
				lookRotation.Normalize();
				if (reverseDir)
				{
					lookRotation *= -1f;
				}
				rotation.SetLookRotation(lookRotation);
			}
			else
			{
				rotation = fxJoint.m_jointObject.transform.rotation;
			}
			fx = sequence.InstantiateFX(fxPrefab, position, rotation);
			SetAttribute(fx, "abilityAreaLength", (sequence.TargetPos - position).magnitude);
		}
		return fx;
	}

	internal void AttachToBone(GameObject fx, GameObject parent)
	{
		if (m_parentedFXs == null)
		{
			InitializeFXStorage();
		}
		GameObject host = new GameObject
		{
			transform =
			{
				parent = parent.transform,
				localPosition = Vector3.zero,
				localScale = Vector3.one,
				localRotation = Quaternion.identity
			}
		};
		fx.transform.parent = host.transform;
		m_parentedFXs.Add(host);
	}

	internal static ActorModelData.ImpulseInfo CreateImpulseInfoWithObjectPose(GameObject obj)
	{
		return obj != null
			? new ActorModelData.ImpulseInfo(obj.transform.position, obj.transform.forward)
			: null;
	}

	internal static ActorModelData.ImpulseInfo CreateImpulseInfoWithActorForward(ActorData actor)
	{
		return actor != null
			? new ActorModelData.ImpulseInfo(actor.transform.position, Vector3.up + actor.transform.forward)
			: null;
	}

	internal static ActorModelData.ImpulseInfo CreateImpulseInfoBetweenActors(ActorData fromActor, ActorData targetActor)
	{
		return fromActor != null && targetActor != null
			? fromActor == targetActor
				? CreateImpulseInfoWithActorForward(fromActor)
				: new ActorModelData.ImpulseInfo(targetActor.transform.position, targetActor.transform.position - fromActor.transform.position)
			: null;
	}

	public override string ToString()
	{
		return $"[Sequence: {GetType()}, " +
		       $"Object: {gameObject.name}, " +
		       $"id: {Id}, " +
		       $"initialized: {m_initialized}, " +
		       $"enabled: {enabled}, " +
		       $"MarkedForRemoval: {MarkedForRemoval}, " +
		       $"Caster: {(Caster == null ? "NULL" : Caster.ToString())}]";
	}

	public string GetTargetsString()
	{
		string text = string.Empty;
		if (Targets == null || Targets.Length <= 0)
		{
			return text;
		}
		foreach (ActorData target in Targets)
		{
			if (target == null)
			{
				continue;
			}
			if (text.Length > 0)
			{
				text += " | ";
			}
			text += target.ActorIndex;
		}
		return text;
	}

	public void OverridePhaseTimingParams(PhaseTimingParameters timingParams, IExtraSequenceParams iParams)
	{
		if (iParams == null
		    || !(iParams is PhaseTimingExtraParams phaseTimingExtraParams)
		    || timingParams == null
		    || !timingParams.m_acceptOverrideFromParams)
		{
			return;
		}
		if (phaseTimingExtraParams.m_turnDelayStartOverride >= 0)
		{
			timingParams.m_turnDelayStart = phaseTimingExtraParams.m_turnDelayStartOverride;
		}
		if (phaseTimingExtraParams.m_turnDelayEndOverride >= 0)
		{
			timingParams.m_turnDelayEnd = phaseTimingExtraParams.m_turnDelayEndOverride;
		}
		if (phaseTimingExtraParams.m_abilityPhaseStartOverride >= 0)
		{
			timingParams.m_usePhaseStartTiming = true;
			timingParams.m_abilityPhaseStart = (AbilityPriority)phaseTimingExtraParams.m_abilityPhaseStartOverride;
		}
		if (phaseTimingExtraParams.m_abilityPhaseEndOverride >= 0)
		{
			timingParams.m_usePhaseEndTiming = true;
			timingParams.m_abilityPhaseEnd = (AbilityPriority)phaseTimingExtraParams.m_abilityPhaseEndOverride;
		}
	}

	public string GetInEditorDescription()
	{
		string text = m_setupNotes.m_notes;
		if (string.IsNullOrEmpty(text))
		{
			text = "<empty>";
		}
		string str = "Setup Note: " + text + "\n----------\n";
		if (m_targetHitAnimation)
		{
			str += "<[x] Target Hit Animation> Can trigger hit react anim\n\n";
			if (m_canTriggerHitReactOnAllyHit)
			{
				str += "Can trigger hit react on ally hit\n\n";
			}
		}
		str += GetVisibilityDescription();
		return str + "\n<color=white>--Sequence Specific--</color>\n" + GetSequenceSpecificDescription();
	}

	public virtual string GetVisibilityDescription()
	{
		string text = string.Empty;
		if (m_visibilityType == VisibilityType.Always)
		{
			text += "<color=yellow>WARNING: </color>VisibilityType is set to be always visible. Ignore if that is intended.\n";
		}
		return text;
	}

	public virtual string GetSequenceSpecificDescription()
	{
		return "NO SEQUENCE SPECIFIC DESCRIPTION IMPLEMENTED YET T_T";
	}

	public static string GetVisibilityTypeDescription(VisibilityType visType, out bool usesTargetPos, out bool usesSeqPos)
	{
		string result = "UNKNOWN";
		usesTargetPos = false;
		usesSeqPos = false;
		switch (visType)
		{
			case VisibilityType.Always:
				result = "always visible";
				break;
			case VisibilityType.Caster:
				result = $"visible if {c_casterToken} is visible";
				break;
			case VisibilityType.CasterOrTarget:
				result = $"visible if either {c_casterToken} or any {c_targetActorToken} is visible";
				break;
			case VisibilityType.CasterOrTargetPos:
				result = $"visible if either {c_casterToken} visible or {c_targetPosToken} square visible";
				usesTargetPos = true;
				break;
			case VisibilityType.CasterOrTargetOrTargetPos:
				result =
					$"visible if either {c_casterToken} or any {c_targetActorToken} visible, or {c_seqPosToken} square is visible";
				usesTargetPos = true;
				break;
			case VisibilityType.Target:
				result = $"visible if any {c_targetActorToken} is visible";
				break;
			case VisibilityType.TargetPos:
				result = $"visible if {c_targetPosToken} square is visible";
				usesTargetPos = true;
				break;
			case VisibilityType.AlwaysOnlyIfCaster:
				result = $"visible only if {c_casterToken} is current {c_clientActorToken} that player controls";
				break;
			case VisibilityType.AlwaysOnlyIfTarget:
				result =
					$"visible only if first {c_targetActorToken} is current {c_clientActorToken} that player controls";
				break;
			case VisibilityType.AlwaysIfCastersTeam:
				result =
					$"visible if {c_casterToken} is on same team as current {c_clientActorToken} that player controls";
				break;
			case VisibilityType.SequencePosition:
				result = $"visible if {c_seqPosToken} square is visible\n(ex. for most projectiles)";
				usesSeqPos = true;
				break;
			case VisibilityType.CastersTeamOrSequencePosition:
				result =
					$"visible if {c_casterToken} is on same team as current {c_clientActorToken} that player controls, " +
					$"OR {c_seqPosToken} square is visible\n(ex. for projectile that should always be visible " +
					$"for allies but only visible if projectile position is visible for enemies)";
				usesSeqPos = true;
				break;
			case VisibilityType.TargetPosAndCaster:
				result =
					$"visible if {c_casterToken} is visible AND {c_targetPosToken} square is visible\n" +
					$"(ex. for Flash catalyst while stealthed)";
				usesTargetPos = true;
				break;
		}
		return result;
	}
}
