using System;
using System.Collections.Generic;
using UnityEngine;

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
			if (!m_usePhaseEndTiming || m_finished || m_turnDelayEnd >= 0)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (m_abilityPhaseEnd != AbilityPriority.INVALID)
				{
					m_finished = true;
				}
				return;
			}
		}

		internal void OnAbilityPhaseStart(AbilityPriority abilityPhase)
		{
			if (m_turnDelayStart <= 0)
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
				if (abilityPhase == m_abilityPhaseStart)
				{
					m_started = true;
				}
			}
			if (m_turnDelayEnd > 0)
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (abilityPhase == m_abilityPhaseEnd)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						m_finished = true;
						return;
					}
				}
				return;
			}
		}

		internal bool ShouldSequenceBeActive()
		{
			int result;
			if (!m_started)
			{
				if (m_usePhaseStartTiming)
				{
					result = 0;
					goto IL_0044;
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
			}
			if (m_finished)
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
				result = ((!m_usePhaseEndTiming) ? 1 : 0);
			}
			else
			{
				result = 1;
			}
			goto IL_0044;
			IL_0044:
			return (byte)result != 0;
		}

		internal bool ShouldSpawnSequence(AbilityPriority abilityPhase)
		{
			bool result = false;
			if (m_turnDelayStart == 0 && abilityPhase == m_abilityPhaseStart)
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
				if (m_usePhaseStartTiming)
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
					result = true;
				}
			}
			return result;
		}

		internal bool ShouldStopSequence(AbilityPriority abilityPhase)
		{
			bool result = false;
			if (m_turnDelayEnd == 0)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (abilityPhase == m_abilityPhaseEnd)
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
					if (m_usePhaseEndTiming)
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

		public IExtraSequenceParams[] ToArray()
		{
			return new IExtraSequenceParams[1]
			{
				this
			};
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
			sbyte value = (sbyte)m_paramNameCode;
			sbyte value2 = (sbyte)m_paramTarget;
			stream.Serialize(ref value);
			stream.Serialize(ref value2);
			stream.Serialize(ref m_paramValue);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			sbyte value = 0;
			sbyte value2 = 0;
			stream.Serialize(ref value);
			stream.Serialize(ref value2);
			stream.Serialize(ref m_paramValue);
			m_paramNameCode = (ParamNameCode)value;
			m_paramTarget = (ParamTarget)value2;
		}

		public string GetAttributeName()
		{
			if (m_paramNameCode == ParamNameCode.ScaleControl)
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
						return "scaleControl";
					}
				}
			}
			if (m_paramNameCode == ParamNameCode.LengthInSquares)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return "lengthInSquares";
					}
				}
			}
			if (m_paramNameCode == ParamNameCode.WidthInSquares)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return "widthInSquares";
					}
				}
			}
			if (m_paramNameCode == ParamNameCode.AbilityAreaLength)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return "abilityAreaLength";
					}
				}
			}
			return string.Empty;
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
			sbyte value = (sbyte)m_fieldIdentifier;
			stream.Serialize(ref value);
			stream.Serialize(ref m_value);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			sbyte value = 0;
			stream.Serialize(ref value);
			stream.Serialize(ref m_value);
			m_fieldIdentifier = (FieldIdentifier)value;
		}
	}

	public class GenericActorListParam : IExtraSequenceParams
	{
		public List<ActorData> m_actors;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			List<ActorData> actors = m_actors;
			int num;
			if (actors != null)
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
				num = (sbyte)actors.Count;
			}
			else
			{
				num = 0;
			}
			sbyte value = (sbyte)num;
			stream.Serialize(ref value);
			for (int i = 0; i < value; i++)
			{
				ActorData actorData = actors[i];
				int num2;
				if (actorData != null)
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
					num2 = (sbyte)actorData.ActorIndex;
				}
				else
				{
					num2 = (sbyte)ActorData.s_invalidActorIndex;
				}
				sbyte value2 = (sbyte)num2;
				stream.Serialize(ref value2);
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

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			sbyte value = 0;
			stream.Serialize(ref value);
			m_actors = new List<ActorData>(value);
			for (int i = 0; i < value; i++)
			{
				sbyte value2 = (sbyte)ActorData.s_invalidActorIndex;
				stream.Serialize(ref value2);
				ActorData item = GameFlowData.Get().FindActorByActorIndex(value2);
				m_actors.Add(item);
			}
		}
	}

	public SequenceNotes m_setupNotes;

	[Separator("For Hit React Animation", true)]
	public bool m_targetHitAnimation = true;

	public bool m_canTriggerHitReactOnAllyHit;

	public string m_customHitReactTriggerName = string.Empty;

	[Separator("Visibility (please don't forget me T_T ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~)", true)]
	[Tooltip("What visibility rules to use for this sequence")]
	public VisibilityType m_visibilityType;

	[Tooltip("What visibility rules to use for the hitFX")]
	public HitVisibilityType m_hitVisibilityType;

	public PhaseBasedVisibilityType m_phaseVisibilityType;

	[Separator("How to Hide Vfx Object", true)]
	public SequenceHideType m_sequenceHideType;

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

	public short PrefabLookupId
	{
		get;
		private set;
	}

	internal ActorData[] Targets
	{
		get;
		set;
	}

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
		get
		{
			return m_caster;
		}
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
		get
		{
			return m_targetBoardSquare;
		}
		set
		{
			m_targetBoardSquare = value;
		}
	}

	internal Vector3 TargetPos
	{
		get;
		set;
	}

	internal Quaternion TargetRotation
	{
		get;
		set;
	}

	internal int AgeInTurns
	{
		get;
		set;
	}

	internal bool Ready
	{
		get
		{
			int result;
			if (m_initialized)
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
				if (base.enabled)
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
					result = ((!MarkedForRemoval) ? 1 : 0);
					goto IL_003d;
				}
			}
			result = 0;
			goto IL_003d;
			IL_003d:
			return (byte)result != 0;
		}
	}

	internal bool InitializedEver
	{
		get;
		private set;
	}

	internal bool MarkedForRemoval
	{
		get;
		private set;
	}

	internal bool RemoveAtTurnEnd
	{
		get;
		set;
	}

	internal int Id
	{
		get;
		set;
	}

	internal SequenceSource Source
	{
		get;
		private set;
	}

	public bool HasReceivedAnimEventBeforeReady
	{
		get
		{
			return m_debugHasReceivedAnimEventBeforeReady;
		}
		set
		{
			m_debugHasReceivedAnimEventBeforeReady = value;
		}
	}

	public GameObject GetReferenceModel(ActorData referenceActorData, ReferenceModelType referenceModelType)
	{
		GameObject result = null;
		if (referenceModelType == ReferenceModelType.Actor)
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
			if (referenceActorData != null)
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
				result = referenceActorData.gameObject;
			}
		}
		else if (referenceModelType == ReferenceModelType.TempSatellite)
		{
			result = SequenceManager.Get().FindTempSatellite(Source);
		}
		else if (referenceModelType == ReferenceModelType.PersistentSatellite1)
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
			if (referenceActorData != null)
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
				SatelliteController component = referenceActorData.GetComponent<SatelliteController>();
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
					if (component.GetSatellite(0) != null)
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
						result = component.GetSatellite(0).gameObject;
					}
				}
			}
		}
		return result;
	}

	public void InitPrefabLookupId(short lookupId)
	{
		PrefabLookupId = lookupId;
	}

	internal void MarkForRemoval()
	{
		MarkedForRemoval = true;
		base.enabled = false;
	}

	internal static void MarkSequenceArrayForRemoval(Sequence[] sequencesArray)
	{
		if (sequencesArray == null)
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
			for (int i = 0; i < sequencesArray.Length; i++)
			{
				if (sequencesArray[i] != null)
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
					sequencesArray[i].MarkForRemoval();
				}
			}
			return;
		}
	}

	internal bool RequestsHitAnimation(ActorData target)
	{
		int result;
		if (m_targetHitAnimation)
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
			if (target != null)
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
				if (Caster != null)
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
					if (!m_canTriggerHitReactOnAllyHit)
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
						result = ((Caster.GetOpposingTeam() == target.GetTeam()) ? 1 : 0);
					}
					else
					{
						result = 1;
					}
					goto IL_007c;
				}
			}
		}
		result = 0;
		goto IL_007c;
		IL_007c:
		return (byte)result != 0;
	}

	protected virtual void Awake()
	{
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
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			FinishSetup();
			DoneInitialization();
			ProcessSequenceVisibility();
			if (SequenceManager.SequenceDebugTraceOn)
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
				Debug.LogWarning(string.Concat("<color=yellow>Client Enable: </color><<color=lightblue>", base.gameObject.name, " | ", GetType(), "</color>> @time= ", GameTime.time));
			}
			base.enabled = true;
			return;
		}
	}

	protected virtual void OnDestroy()
	{
		if (m_parentedFXs != null)
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
			using (List<GameObject>.Enumerator enumerator = m_parentedFXs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject current = enumerator.Current;
					UnityEngine.Object.Destroy(current);
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		if (!(SequenceManager.Get() != null))
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
			SequenceManager.Get().OnDestroySequence(this);
			return;
		}
	}

	private void DoneInitialization()
	{
		if (InitializedEver)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_initialized = true;
			InitializedEver = true;
			return;
		}
	}

	internal virtual void Initialize(IExtraSequenceParams[] extraParams)
	{
	}

	public virtual void FinishSetup()
	{
	}

	internal void BaseInitialize_Client(BoardSquare targetSquare, Vector3 targetPos, Quaternion targetRotation, ActorData[] targets, ActorData caster, int id, GameObject prefab, short baseSequenceLookupId, SequenceSource source, IExtraSequenceParams[] extraParams)
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
		IExtraSequenceParams[] extraParams2;
		if (extraParams != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			extraParams2 = extraParams;
		}
		else
		{
			extraParams2 = s_emptyParams;
		}
		Initialize(extraParams2);
		m_waitForClientEnable = Source.WaitForClientEnable;
		base.enabled = !m_waitForClientEnable;
		if (m_waitForClientEnable)
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
			FinishSetup();
			DoneInitialization();
			return;
		}
	}

	protected virtual void OnStopVfxOnClient()
	{
	}

	private void OnActorAdded(ActorData actor)
	{
		if (InitializedEver)
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
			if (!base.enabled)
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
				if (MarkedForRemoval)
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
					if (!(m_caster == null))
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
						if (m_casterId == ActorData.s_invalidActorIndex)
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
							m_caster = GameFlowData.Get().FindActorByActorIndex(m_casterId);
							if (m_caster != null)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									FinishSetup();
									DoneInitialization();
									GameFlowData.s_onAddActor -= OnActorAdded;
									return;
								}
							}
							return;
						}
					}
				}
			}
		}
	}

	internal bool IsCasterOrTargetsVisible()
	{
		bool result = false;
		if (IsActorConsideredVisible(Caster))
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
			result = true;
		}
		else if (Targets != null)
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
			ActorData[] targets = Targets;
			int num = 0;
			while (true)
			{
				if (num < targets.Length)
				{
					ActorData actor = targets[num];
					if (IsActorConsideredVisible(actor))
					{
						result = true;
						break;
					}
					num++;
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
				break;
			}
		}
		return result;
	}

	internal virtual Vector3 GetSequencePos()
	{
		return TargetPos;
	}

	internal bool IsSequencePosVisible()
	{
		bool result = false;
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (clientFog != null)
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
			if (GetSequencePos().magnitude > 0f)
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
				if (Board.Get().m_showLOS)
				{
					if (!clientFog.IsVisible(Board.Get().GetBoardSquare(GetSequencePos())))
					{
						goto IL_0085;
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
				result = true;
			}
		}
		else
		{
			result = true;
		}
		goto IL_0085;
		IL_0085:
		return result;
	}

	internal bool IsTargetPosVisible()
	{
		bool result = false;
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (clientFog != null)
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
			if (clientFog.IsVisible(Board.Get().GetBoardSquare(TargetPos)))
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
			if (IsActorConsideredVisible(Caster))
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
				result = true;
			}
			else if (clientFog.IsVisible(Board.Get().GetBoardSquare(TargetPos)))
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
			if (IsActorConsideredVisible(Caster))
			{
				result = true;
			}
			else if (clientFog.IsVisible(Board.Get().GetBoardSquare(TargetPos)))
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
				result = true;
			}
			else if (Targets != null)
			{
				ActorData[] targets = Targets;
				foreach (ActorData actor in targets)
				{
					if (IsActorConsideredVisible(actor))
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
		m_lastSetVisibleValue = visible;
		if (!(m_fxParent != null) || m_parentedFXs == null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_sequenceHideType == SequenceHideType.MoveOffCamera_KeepEnabled)
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
				Transform transform = m_fxParent.transform;
				Vector3 localPosition;
				if (visible)
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
				m_fxParent.SetActive(visible);
			}
			for (int i = 0; i < m_parentedFXs.Count; i++)
			{
				GameObject gameObject = m_parentedFXs[i];
				if (!gameObject)
				{
					continue;
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
				if (m_sequenceHideType == SequenceHideType.MoveOffCamera_KeepEnabled)
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
					Transform transform2 = gameObject.transform;
					Vector3 localPosition2;
					if (visible)
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
						localPosition2 = Vector3.zero;
					}
					else
					{
						localPosition2 = -10000f * Vector3.one;
					}
					transform2.localPosition = localPosition2;
					continue;
				}
				if (m_sequenceHideType == SequenceHideType.KillThenDisable)
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
					if (!visible)
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
						PKFxFX[] componentsInChildren = gameObject.GetComponentsInChildren<PKFxFX>(true);
						PKFxFX[] array = componentsInChildren;
						foreach (PKFxFX pKFxFX in array)
						{
							pKFxFX.KillEffect();
						}
					}
				}
				gameObject.SetActive(visible);
			}
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	protected bool LastDesiredVisible()
	{
		return m_lastSetVisibleValue;
	}

	protected bool IsActorConsideredVisible(ActorData actor)
	{
		int result;
		if (actor != null)
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
			if (actor.IsVisibleToClient())
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
				if (!(actor.GetActorModelData() == null))
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
					result = (actor.GetActorModelData().IsVisibleToClient() ? 1 : 0);
				}
				else
				{
					result = 1;
				}
				goto IL_005f;
			}
		}
		result = 0;
		goto IL_005f;
		IL_005f:
		return (byte)result != 0;
	}

	protected void ProcessSequenceVisibility()
	{
		int num;
		if (m_initialized)
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
			if (!MarkedForRemoval)
			{
				if (m_phaseVisibilityType != 0)
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
					if (GameFlowData.Get() != null)
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
						GameState gameState = GameFlowData.Get().gameState;
						if (m_phaseVisibilityType == PhaseBasedVisibilityType.InDecisionOnly)
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
							if (gameState != GameState.BothTeams_Decision)
							{
								goto IL_00a0;
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
						if (m_phaseVisibilityType == PhaseBasedVisibilityType.InResolutionOnly)
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
							if (gameState != GameState.BothTeams_Resolve)
							{
								goto IL_00a0;
							}
						}
					}
				}
				if (m_turnOffVFXDuringCinematicCam)
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
					if ((bool)CameraManager.Get())
					{
						num = (CameraManager.Get().InCinematic() ? 1 : 0);
						goto IL_00db;
					}
				}
				num = 0;
				goto IL_00db;
			}
		}
		SetSequenceVisibility(false);
		return;
		IL_059e:
		object obj;
		ActorData actorData = (ActorData)obj;
		goto IL_05aa;
		IL_00a0:
		SetSequenceVisibility(false);
		return;
		IL_00db:
		bool flag = (byte)num != 0;
		if (flag)
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
			if (m_keepVFXInCinematicCamForCaster)
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
				if (Caster != null)
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
					ActorData cinematicTargetActor = CameraManager.Get().GetCinematicTargetActor();
					int cinematicActionAnimIndex = CameraManager.Get().GetCinematicActionAnimIndex();
					int num2;
					if (m_keepCasterVFXForAnimIndex > 0)
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
						num2 = ((m_keepCasterVFXForAnimIndex == cinematicActionAnimIndex) ? 1 : 0);
					}
					else
					{
						num2 = 1;
					}
					bool flag2 = (byte)num2 != 0;
					int num3;
					if (m_keepCasterVFXForTurnOfSpawnOnly)
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
						num3 = ((AgeInTurns == 0) ? 1 : 0);
					}
					else
					{
						num3 = 1;
					}
					bool flag3 = (byte)num3 != 0;
					if (cinematicTargetActor.ActorIndex == Caster.ActorIndex && flag2)
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
						if (flag3)
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
							flag = false;
						}
					}
				}
			}
		}
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
		if (flag)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				SetSequenceVisibility(false);
				return;
			}
		}
		if (activeOwnedActorData == null)
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
			if (localPlayerData == null)
			{
				SetSequenceVisibility(true);
				return;
			}
		}
		if (m_forceAlwaysVisible)
		{
			SetSequenceVisibility(true);
			return;
		}
		if (m_visibilityType == VisibilityType.Caster)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (Caster != null)
				{
					SetSequenceVisibility(IsActorConsideredVisible(Caster));
				}
				else
				{
					SetSequenceVisibility(true);
				}
				return;
			}
		}
		if (m_visibilityType == VisibilityType.CasterOrTarget)
		{
			SetSequenceVisibility(IsCasterOrTargetsVisible());
			return;
		}
		if (m_visibilityType == VisibilityType.CasterOrTargetPos)
		{
			SetSequenceVisibility(IsCasterOrTargetPosVisible());
			return;
		}
		if (m_visibilityType == VisibilityType.CasterOrTargetOrTargetPos)
		{
			SetSequenceVisibility(IsCasterOrTargetOrTargetPosVisible());
			return;
		}
		if (m_visibilityType == VisibilityType.Target)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				int num4;
				if (Targets != null)
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
					if (Targets.Length > 0)
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
						num4 = (IsActorConsideredVisible(Targets[0]) ? 1 : 0);
						goto IL_030d;
					}
				}
				num4 = 0;
				goto IL_030d;
				IL_030d:
				bool sequenceVisibility = (byte)num4 != 0;
				SetSequenceVisibility(sequenceVisibility);
				return;
			}
		}
		if (m_visibilityType == VisibilityType.TargetPos)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				SetSequenceVisibility(IsTargetPosVisible());
				return;
			}
		}
		if (m_visibilityType == VisibilityType.AlwaysOnlyIfCaster)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				SetSequenceVisibility(Caster == activeOwnedActorData);
				return;
			}
		}
		if (m_visibilityType == VisibilityType.AlwaysOnlyIfTarget)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				SetSequenceVisibility(Target == activeOwnedActorData);
				return;
			}
		}
		if (m_visibilityType == VisibilityType.AlwaysIfCastersTeam)
		{
			if (Caster != null && activeOwnedActorData != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						SetSequenceVisibility(Caster.GetTeam() == activeOwnedActorData.GetTeam());
						return;
					}
				}
			}
			SetSequenceVisibility(true);
			return;
		}
		if (m_visibilityType == VisibilityType.SequencePosition)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				SetSequenceVisibility(IsSequencePosVisible());
				return;
			}
		}
		if (m_visibilityType == VisibilityType.Always)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				SetSequenceVisibility(true);
				return;
			}
		}
		if (m_visibilityType == VisibilityType.CastersTeamOrSequencePosition)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (Caster != null)
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
					if (activeOwnedActorData != null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								SetSequenceVisibility(Caster.GetTeam() == activeOwnedActorData.GetTeam() || IsSequencePosVisible());
								return;
							}
						}
					}
				}
				SetSequenceVisibility(IsSequencePosVisible());
				return;
			}
		}
		if (m_visibilityType == VisibilityType.TargetPosAndCaster)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				int num5;
				if (IsTargetPosVisible())
				{
					if (!(Caster == null))
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
						num5 = (IsActorConsideredVisible(Caster) ? 1 : 0);
					}
					else
					{
						num5 = 1;
					}
				}
				else
				{
					num5 = 0;
				}
				bool sequenceVisibility2 = (byte)num5 != 0;
				SetSequenceVisibility(sequenceVisibility2);
				return;
			}
		}
		if (m_visibilityType != VisibilityType.TargetIfNotEvading)
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
			if (m_visibilityType != VisibilityType.CasterIfNotEvading)
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
				break;
			}
		}
		bool sequenceVisibility3 = false;
		actorData = null;
		if (m_visibilityType == VisibilityType.TargetIfNotEvading)
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
			if (Targets != null)
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
				if (Targets.Length > 0)
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
					obj = Targets[0];
					goto IL_059e;
				}
			}
			obj = null;
			goto IL_059e;
		}
		actorData = Caster;
		goto IL_05aa;
		IL_05aa:
		if (actorData != null)
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
			if (IsActorConsideredVisible(actorData))
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
				if (actorData.GetActorMovement() != null)
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
					sequenceVisibility3 = !actorData.GetActorMovement().InChargeState();
				}
				else
				{
					sequenceVisibility3 = true;
				}
			}
		}
		SetSequenceVisibility(sequenceVisibility3);
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
		if (Ready)
		{
			OnAnimationEvent(eventObject, sourceObject);
		}
	}

	protected void CallHitSequenceOnTargets(Vector3 impactPos, float defaultImpulseRadius = 1f, List<ActorData> actorsToIgnore = null, bool tryHitReactIfAlreadyHit = true)
	{
		float num = 0f;
		if (Targets != null)
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
			for (int i = 0; i < Targets.Length; i++)
			{
				if (!(Targets[i] != null))
				{
					continue;
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
				float magnitude = (Targets[i].transform.position - impactPos).magnitude;
				if (magnitude > num)
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
					num = magnitude;
				}
			}
		}
		float num2;
		if (num < Board.Get().squareSize / 2f)
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
			num2 = defaultImpulseRadius;
		}
		else
		{
			num2 = num;
		}
		num = num2;
		ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(num, impactPos);
		if (Targets != null)
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
			for (int j = 0; j < Targets.Length; j++)
			{
				if (!(Targets[j] != null))
				{
					continue;
				}
				if (actorsToIgnore != null)
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
					if (actorsToIgnore.Contains(Targets[j]))
					{
						continue;
					}
				}
				Source.OnSequenceHit(this, Targets[j], impulseInfo, ActorModelData.RagdollActivation.HealthBased, tryHitReactIfAlreadyHit);
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
		}
		Source.OnSequenceHit(this, TargetPos, impulseInfo);
		List<Team> list = new List<Team>();
		list.Add(Caster.GetOpposingTeam());
		list.Add(Caster.GetTeam());
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(impactPos, num * 3f, false, Caster, list, null);
		for (int k = 0; k < actorsInRadius.Count; k++)
		{
			if (!(actorsInRadius[k] != null))
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
			if (actorsInRadius[k].GetActorModelData() != null)
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
				Vector3 direction = actorsInRadius[k].transform.position - impactPos;
				direction.y = 0f;
				direction.Normalize();
				actorsInRadius[k].GetActorModelData().ImpartWindImpulse(direction);
			}
		}
	}

	protected Vector3 GetTargetHitPosition(ActorData actorData, JointPopupProperty fxJoint)
	{
		Vector3 result = Vector3.zero;
		int num = 0;
		while (true)
		{
			if (num < Targets.Length)
			{
				if (Targets[num] == actorData)
				{
					result = GetTargetHitPosition(num, fxJoint);
					break;
				}
				num++;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			break;
		}
		return result;
	}

	protected Vector3 GetTargetHitPosition(int targetIndex, JointPopupProperty fxJoint)
	{
		bool flag = false;
		Vector3 result = Vector3.zero;
		if (Targets != null)
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
			if (Targets.Length > targetIndex && Targets[targetIndex] != null)
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
				if (Targets[targetIndex].gameObject != null)
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
					fxJoint.Initialize(Targets[targetIndex].gameObject);
					if (fxJoint.m_jointObject != null)
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
						result = fxJoint.m_jointObject.transform.position;
						flag = true;
					}
				}
			}
		}
		if (!flag)
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
		if (secondaryActorHits)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (Targets != null)
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
				if (Targets.Length > targetIndex && Targets[targetIndex] != null)
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
					if (Targets[targetIndex].gameObject != null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
							{
								GameObject gameObject = Targets[targetIndex].gameObject.FindInChildren(s_defaultHitAttachJoint);
								if (gameObject != null)
								{
									return gameObject.transform.position + Vector3.up;
								}
								gameObject = Targets[targetIndex].gameObject.FindInChildren(s_defaultFallbackHitAttachJoint);
								if (gameObject != null)
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											break;
										default:
											return gameObject.transform.position + Vector3.up;
										}
									}
								}
								return Targets[targetIndex].gameObject.transform.position;
							}
							}
						}
					}
				}
			}
		}
		if (TargetSquare != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					Vector3 a = TargetSquare.ToVector3();
					return a + Vector3.up;
				}
				}
			}
		}
		return TargetPos;
	}

	protected bool IsHitFXVisible(ActorData hitTarget)
	{
		bool flag = false;
		HitVisibilityType hitVisibilityType = m_hitVisibilityType;
		if (hitVisibilityType != 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (hitVisibilityType != HitVisibilityType.Target)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
					int result;
					if ((bool)hitTarget)
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
						result = (IsActorConsideredVisible(hitTarget) ? 1 : 0);
					}
					else
					{
						result = 0;
					}
					return (byte)result != 0;
				}
				}
			}
		}
		if (hitTarget != null && Caster != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
					if (activeOwnedActorData != null)
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
						if (hitTarget.GetTeam() == Caster.GetTeam())
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
							if (activeOwnedActorData.GetTeam() != hitTarget.GetTeam())
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										break;
									default:
										return IsActorConsideredVisible(hitTarget);
									}
								}
							}
						}
					}
					return true;
				}
				}
			}
		}
		return true;
	}

	public bool IsHitFXVisibleWrtTeamFilter(ActorData hitTarget, HitVFXSpawnTeam teamFilter)
	{
		bool flag = IsHitFXVisible(hitTarget);
		int num;
		if (flag && Caster != null)
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
			if (hitTarget != null)
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
				if (teamFilter != 0)
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
					bool flag2 = Caster.GetTeam() == hitTarget.GetTeam();
					if (teamFilter == HitVFXSpawnTeam.AllTargets)
					{
						goto IL_00d5;
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
					if (teamFilter == HitVFXSpawnTeam.AllExcludeCaster)
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
						if (Caster != hitTarget)
						{
							goto IL_00d5;
						}
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					if (teamFilter == HitVFXSpawnTeam.AllyAndCaster)
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
						if (flag2)
						{
							goto IL_00d5;
						}
					}
					if (teamFilter == HitVFXSpawnTeam.EnemyOnly)
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
						num = ((!flag2) ? 1 : 0);
					}
					else
					{
						num = 0;
					}
					goto IL_00d6;
				}
			}
		}
		goto IL_00d7;
		IL_00d5:
		num = 1;
		goto IL_00d6;
		IL_00d6:
		flag = ((byte)num != 0);
		goto IL_00d7;
		IL_00d7:
		return flag;
	}

	public bool ShouldHideForActorIfAttached(ActorData actor)
	{
		return actor != null && actor.IsModelAnimatorDisabled();
	}

	protected void InitializeFXStorage()
	{
		if (m_fxParent == null)
		{
			m_fxParent = new GameObject("fxParent_" + GetType().ToString());
			m_fxParent.transform.parent = base.transform;
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

	internal GameObject InstantiateFX(GameObject prefab, Vector3 position, Quaternion rotation, bool tryApplyCameraOffset = true, bool logErrorOnNullPrefab = true)
	{
		if (m_fxParent == null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			InitializeFXStorage();
		}
		if (tryApplyCameraOffset)
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
			if (prefab != null)
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
				if ((bool)prefab.GetComponent<OffsetVFXTowardsCamera>())
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
					position = OffsetVFXTowardsCamera.ProcessOffset(position);
				}
			}
		}
		GameObject gameObject = null;
		if (prefab != null)
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
			gameObject = UnityEngine.Object.Instantiate(prefab, position, rotation);
		}
		else
		{
			gameObject = new GameObject("FallbackForNullFx");
			if (Application.isEditor)
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
				if (logErrorOnNullPrefab)
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
					Debug.LogError(base.gameObject.name + " Trying to instantiate null FX prefab");
				}
			}
		}
		ReplaceVFXPrefabs(gameObject);
		gameObject.transform.parent = m_fxParent.transform;
		gameObject.gameObject.SetLayerRecursively(LayerMask.NameToLayer("DynamicLit"));
		return gameObject;
	}

	private void ReplaceVFXPrefabs(GameObject vfxInstanceRoot)
	{
		GameEventManager gameEventManager = GameEventManager.Get();
		if (gameEventManager == null)
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
			if (Caster != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					GameEventManager.ReplaceVFXPrefab replaceVFXPrefab = new GameEventManager.ReplaceVFXPrefab();
					replaceVFXPrefab.characterResourceLink = Caster.GetCharacterResourceLink();
					replaceVFXPrefab.characterVisualInfo = Caster.m_visualInfo;
					replaceVFXPrefab.characterAbilityVfxSwapInfo = Caster.m_abilityVfxSwapInfo;
					replaceVFXPrefab.vfxRoot = vfxInstanceRoot.transform;
					gameEventManager.FireEvent(GameEventManager.EventType.ReplaceVFXPrefab, replaceVFXPrefab);
					return;
				}
			}
			return;
		}
	}

	internal static void SetAttribute(GameObject fx, string name, int value)
	{
		if (!(fx != null))
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
			PKFxFX[] componentsInChildren = fx.GetComponentsInChildren<PKFxFX>(true);
			PKFxFX[] array = componentsInChildren;
			foreach (PKFxFX pKFxFX in array)
			{
				if (!(pKFxFX != null))
				{
					continue;
				}
				PKFxManager.AttributeDesc desc = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Int, name);
				if (pKFxFX.AttributeExists(desc))
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
					PKFxManager.Attribute attribute = new PKFxManager.Attribute(desc);
					attribute.m_Value0 = value;
					pKFxFX.SetAttribute(attribute);
				}
			}
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	internal static void SetAttribute(GameObject fx, string name, float value)
	{
		if (!(fx != null))
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
			PKFxFX[] componentsInChildren = fx.GetComponentsInChildren<PKFxFX>(true);
			PKFxFX[] array = componentsInChildren;
			foreach (PKFxFX pKFxFX in array)
			{
				if (!(pKFxFX != null))
				{
					continue;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				PKFxManager.AttributeDesc desc = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Float, name);
				if (pKFxFX.AttributeExists(desc))
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
					PKFxManager.Attribute attribute = new PKFxManager.Attribute(desc);
					attribute.m_Value0 = value;
					pKFxFX.SetAttribute(attribute);
				}
			}
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	internal static void SetAttribute(GameObject fx, string name, Vector3 value)
	{
		if (!(fx != null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			PKFxFX[] componentsInChildren = fx.GetComponentsInChildren<PKFxFX>(true);
			PKFxFX[] array = componentsInChildren;
			foreach (PKFxFX pKFxFX in array)
			{
				if (!(pKFxFX != null))
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
				PKFxManager.AttributeDesc desc = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Float3, name);
				if (pKFxFX.AttributeExists(desc))
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
					PKFxManager.Attribute attribute = new PKFxManager.Attribute(desc);
					attribute.m_Value0 = value.x;
					attribute.m_Value1 = value.y;
					attribute.m_Value2 = value.z;
					pKFxFX.SetAttribute(attribute);
				}
			}
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	internal bool AreFXFinished(GameObject fx)
	{
		if (!Source.RemoveAtEndOfTurn)
		{
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
				return false;
			}
		}
		bool result = false;
		if (fx != null)
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
			if (!fx.activeSelf)
			{
				result = true;
			}
			else
			{
				if (fx.GetComponent<ParticleSystem>() != null)
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
					if (!fx.GetComponent<ParticleSystem>().IsAlive())
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
						result = true;
						goto IL_00d9;
					}
				}
				PKFxFX[] componentsInChildren = fx.GetComponentsInChildren<PKFxFX>(true);
				if (componentsInChildren.Length > 0)
				{
					result = true;
					PKFxFX[] array = componentsInChildren;
					int num = 0;
					while (true)
					{
						if (num < array.Length)
						{
							PKFxFX pKFxFX = array[num];
							if (pKFxFX.Alive())
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
								result = false;
								break;
							}
							num++;
							continue;
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
						break;
					}
				}
			}
		}
		goto IL_00d9;
		IL_00d9:
		return result;
	}

	internal static float GetFXDuration(GameObject fxPrefab)
	{
		float result = 1f;
		if (fxPrefab != null)
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
			ParticleSystem component = fxPrefab.GetComponent<ParticleSystem>();
			if (component != null)
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
				result = component.main.duration;
			}
			else
			{
				PKFxManager.AttributeDesc attributeDesc = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Float, "Duration");
				attributeDesc.DefaultValue0 = 1f;
				PKFxFX[] componentsInChildren = fxPrefab.GetComponentsInChildren<PKFxFX>(true);
				if (componentsInChildren.Length > 0)
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
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						if (componentsInChildren[i] != null && componentsInChildren[i].AttributeExists(attributeDesc))
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
							result = Mathf.Max(componentsInChildren[i].GetAttributeFromDesc(attributeDesc).ValueFloat);
						}
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
			if (!fxJoint.IsInitialized())
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
				fxJoint.Initialize(targetActor.gameObject);
			}
			if (fxPrefab != null)
			{
				if (fxJoint.m_jointObject != null)
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
					if (fxJoint.m_jointObject.transform.localScale != Vector3.zero)
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
						if (attachToJoint)
						{
							gameObject = sequence.InstantiateFX(fxPrefab);
							sequence.AttachToBone(gameObject, fxJoint.m_jointObject);
							gameObject.transform.localPosition = Vector3.zero;
							gameObject.transform.localRotation = Quaternion.identity;
							Quaternion rotation = default(Quaternion);
							if (aimAtCaster)
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
								Vector3 position = sequence.Caster.transform.position;
								Vector3 lookRotation = position - fxJoint.m_jointObject.transform.position;
								lookRotation.y = 0f;
								lookRotation.Normalize();
								if (reverseDir)
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
									lookRotation *= -1f;
								}
								rotation.SetLookRotation(lookRotation);
								gameObject.transform.rotation = rotation;
							}
							goto IL_0243;
						}
					}
				}
				Vector3 position2 = fxJoint.m_jointObject.transform.position;
				Quaternion rotation2 = default(Quaternion);
				if (aimAtCaster)
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
					Vector3 position3 = sequence.Caster.transform.position;
					Vector3 lookRotation2 = position3 - position2;
					lookRotation2.y = 0f;
					lookRotation2.Normalize();
					if (reverseDir)
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
						lookRotation2 *= -1f;
					}
					rotation2.SetLookRotation(lookRotation2);
				}
				else
				{
					rotation2 = fxJoint.m_jointObject.transform.rotation;
				}
				gameObject = sequence.InstantiateFX(fxPrefab, position2, rotation2);
				SetAttribute(gameObject, "abilityAreaLength", (sequence.TargetPos - position2).magnitude);
			}
		}
		goto IL_0243;
		IL_0243:
		return gameObject;
	}

	internal void AttachToBone(GameObject fx, GameObject parent)
	{
		if (m_parentedFXs == null)
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
			InitializeFXStorage();
		}
		GameObject gameObject = new GameObject();
		gameObject.transform.parent = parent.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localRotation = Quaternion.identity;
		fx.transform.parent = gameObject.transform;
		m_parentedFXs.Add(gameObject);
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
					return new ActorModelData.ImpulseInfo(actor.transform.position, Vector3.up + actor.transform.forward);
				}
			}
		}
		return null;
	}

	internal static ActorModelData.ImpulseInfo CreateImpulseInfoBetweenActors(ActorData fromActor, ActorData targetActor)
	{
		if (fromActor != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (targetActor != null)
			{
				if (fromActor == targetActor)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return CreateImpulseInfoWithActorForward(fromActor);
						}
					}
				}
				return new ActorModelData.ImpulseInfo(targetActor.transform.position, targetActor.transform.position - fromActor.transform.position);
			}
		}
		return null;
	}

	public override string ToString()
	{
		object[] obj = new object[7]
		{
			GetType().ToString(),
			base.gameObject.name,
			Id,
			m_initialized,
			base.enabled,
			MarkedForRemoval,
			null
		};
		object obj2;
		if (Caster == null)
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
			obj2 = "NULL";
		}
		else
		{
			obj2 = Caster.ToString();
		}
		obj[6] = obj2;
		return string.Format("[Sequence: {0}, Object: {1}, id: {2}, initialized: {3}, enabled: {4}, MarkedForRemoval: {5}, Caster: {6}]", obj);
	}

	public string GetTargetsString()
	{
		string text = string.Empty;
		if (Targets != null)
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
			if (Targets.Length > 0)
			{
				for (int i = 0; i < Targets.Length; i++)
				{
					ActorData actorData = Targets[i];
					if (!(actorData != null))
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
					if (text.Length > 0)
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
						text += " | ";
					}
					text += actorData.ActorIndex;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		return text;
	}

	public void OverridePhaseTimingParams(PhaseTimingParameters timingParams, IExtraSequenceParams iParams)
	{
		if (iParams == null || !(iParams is PhaseTimingExtraParams))
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
			PhaseTimingExtraParams phaseTimingExtraParams = iParams as PhaseTimingExtraParams;
			if (phaseTimingExtraParams == null)
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
				if (timingParams == null || !timingParams.m_acceptOverrideFromParams)
				{
					return;
				}
				if (phaseTimingExtraParams.m_turnDelayStartOverride >= 0)
				{
					timingParams.m_turnDelayStart = phaseTimingExtraParams.m_turnDelayStartOverride;
				}
				if (phaseTimingExtraParams.m_turnDelayEndOverride >= 0)
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
					timingParams.m_turnDelayEnd = phaseTimingExtraParams.m_turnDelayEndOverride;
				}
				if (phaseTimingExtraParams.m_abilityPhaseStartOverride >= 0)
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
					timingParams.m_usePhaseStartTiming = true;
					timingParams.m_abilityPhaseStart = (AbilityPriority)phaseTimingExtraParams.m_abilityPhaseStartOverride;
				}
				if (phaseTimingExtraParams.m_abilityPhaseEndOverride >= 0)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						timingParams.m_usePhaseEndTiming = true;
						timingParams.m_abilityPhaseEnd = (AbilityPriority)phaseTimingExtraParams.m_abilityPhaseEndOverride;
						return;
					}
				}
				return;
			}
		}
	}

	public string GetInEditorDescription()
	{
		string text = m_setupNotes.m_notes;
		if (string.IsNullOrEmpty(text))
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
			text = "<empty>";
		}
		string str = "Setup Note: " + text + "\n----------\n";
		if (m_targetHitAnimation)
		{
			str += "<[x] Target Hit Animation> Can trigger hit react anim\n\n";
			if (m_canTriggerHitReactOnAllyHit)
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
		if (visType == VisibilityType.Always)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = "always visible";
		}
		else if (visType == VisibilityType.Caster)
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
			result = "visible if " + c_casterToken + " is visible";
		}
		else if (visType == VisibilityType.CasterOrTarget)
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
			result = "visible if either " + c_casterToken + " or any " + c_targetActorToken + " is visible";
		}
		else if (visType == VisibilityType.CasterOrTargetPos)
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
			result = "visible if either " + c_casterToken + " visible or " + c_targetPosToken + " square visible";
			usesTargetPos = true;
		}
		else if (visType == VisibilityType.CasterOrTargetOrTargetPos)
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
			result = "visible if either " + c_casterToken + " or any " + c_targetActorToken + " visible, or " + c_seqPosToken + " square is visible";
			usesTargetPos = true;
		}
		else if (visType == VisibilityType.Target)
		{
			result = "visible if any " + c_targetActorToken + " is visible";
		}
		else if (visType == VisibilityType.TargetPos)
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
			result = "visible if " + c_targetPosToken + " square is visible";
			usesTargetPos = true;
		}
		else if (visType == VisibilityType.AlwaysOnlyIfCaster)
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
			result = "visible only if " + c_casterToken + " is current " + c_clientActorToken + " that player controls";
		}
		else if (visType == VisibilityType.AlwaysOnlyIfTarget)
		{
			result = "visible only if first " + c_targetActorToken + " is current " + c_clientActorToken + " that player controls";
		}
		else if (visType == VisibilityType.AlwaysIfCastersTeam)
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
			result = "visible if " + c_casterToken + " is on same team as current " + c_clientActorToken + " that player controls";
		}
		else if (visType == VisibilityType.SequencePosition)
		{
			result = "visible if " + c_seqPosToken + " square is visible\n(ex. for most projectiles)";
			usesSeqPos = true;
		}
		else if (visType == VisibilityType.CastersTeamOrSequencePosition)
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
			result = "visible if " + c_casterToken + " is on same team as current " + c_clientActorToken + " that player controls, OR " + c_seqPosToken + " square is visible\n(ex. for projectile that should always be visible for allies but only visible if projectile position is visible for enemies)";
			usesSeqPos = true;
		}
		else if (visType == VisibilityType.TargetPosAndCaster)
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
			result = "visible if " + c_casterToken + " is visible AND " + c_targetPosToken + " square is visible\n(ex. for Flash catalyst while stealthed)";
			usesTargetPos = true;
		}
		return result;
	}
}
