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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.GetReferenceModel(ActorData, Sequence.ReferenceModelType)).MethodHandle;
			}
			if (referenceActorData != null)
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
				result = referenceActorData.gameObject;
			}
		}
		else if (referenceModelType == Sequence.ReferenceModelType.TempSatellite)
		{
			result = SequenceManager.Get().FindTempSatellite(this.Source);
		}
		else if (referenceModelType == Sequence.ReferenceModelType.PersistentSatellite1)
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
			if (referenceActorData != null)
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
				SatelliteController component = referenceActorData.GetComponent<SatelliteController>();
				if (component != null)
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
					if (component.GetSatellite(0) != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.get_Ready()).MethodHandle;
				}
				if (base.enabled)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.MarkSequenceArrayForRemoval(Sequence[])).MethodHandle;
			}
			for (int i = 0; i < sequencesArray.Length; i++)
			{
				if (sequencesArray[i] != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.RequestsHitAnimation(ActorData)).MethodHandle;
			}
			if (target != null)
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
				if (this.Caster != null)
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
					if (!this.m_canTriggerHitReactOnAllyHit)
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
						result = ((this.Caster.\u0012() == target.\u000E()) ? 1 : 0);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.OnDoClientEnable()).MethodHandle;
			}
			this.FinishSetup();
			this.DoneInitialization();
			this.ProcessSequenceVisibility();
			if (SequenceManager.SequenceDebugTraceOn)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.OnDestroy()).MethodHandle;
			}
			using (List<GameObject>.Enumerator enumerator = this.m_parentedFXs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject obj = enumerator.Current;
					UnityEngine.Object.Destroy(obj);
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
		}
		if (SequenceManager.Get() != null)
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
			SequenceManager.Get().OnDestroySequence(this);
		}
	}

	private void DoneInitialization()
	{
		if (!this.InitializedEver)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.DoneInitialization()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.BaseInitialize_Client(BoardSquare, Vector3, Quaternion, ActorData[], ActorData, int, GameObject, short, SequenceSource, Sequence.IExtraSequenceParams[])).MethodHandle;
			}
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.OnActorAdded(ActorData)).MethodHandle;
			}
			if (base.enabled)
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
				if (!this.MarkedForRemoval)
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
					if (this.m_caster == null)
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
						if (this.m_casterId != ActorData.s_invalidActorIndex)
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
							this.m_caster = GameFlowData.Get().FindActorByActorIndex(this.m_casterId);
							if (this.m_caster != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.IsCasterOrTargetsVisible()).MethodHandle;
			}
			result = true;
		}
		else if (this.Targets != null)
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
			foreach (ActorData actor in this.Targets)
			{
				if (this.IsActorConsideredVisible(actor))
				{
					return true;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.IsSequencePosVisible()).MethodHandle;
			}
			if (this.GetSequencePos().magnitude > 0f)
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
				if (Board.\u000E().m_showLOS)
				{
					if (!clientFog.IsVisible(Board.\u000E().\u000E(this.GetSequencePos())))
					{
						goto IL_81;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.IsTargetPosVisible()).MethodHandle;
			}
			if (clientFog.IsVisible(Board.\u000E().\u000E(this.TargetPos)))
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.IsCasterOrTargetPosVisible()).MethodHandle;
			}
			if (this.IsActorConsideredVisible(this.Caster))
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
				result = true;
			}
			else if (clientFog.IsVisible(Board.\u000E().\u000E(this.TargetPos)))
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
			else if (clientFog.IsVisible(Board.\u000E().\u000E(this.TargetPos)))
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.IsCasterOrTargetOrTargetPosVisible()).MethodHandle;
				}
				result = true;
			}
			else if (this.Targets != null)
			{
				foreach (ActorData actor in this.Targets)
				{
					if (this.IsActorConsideredVisible(actor))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.SetSequenceVisibility(bool)).MethodHandle;
			}
			if (this.m_sequenceHideType == Sequence.SequenceHideType.MoveOffCamera_KeepEnabled)
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
				Transform transform = this.m_fxParent.transform;
				Vector3 localPosition;
				if (visible)
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
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_sequenceHideType == Sequence.SequenceHideType.MoveOffCamera_KeepEnabled)
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
						Transform transform2 = gameObject.transform;
						Vector3 localPosition2;
						if (visible)
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
							for (;;)
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
								for (;;)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
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
			for (;;)
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

	protected bool LastDesiredVisible()
	{
		return this.m_lastSetVisibleValue;
	}

	protected bool IsActorConsideredVisible(ActorData actor)
	{
		int result;
		if (actor != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.IsActorConsideredVisible(ActorData)).MethodHandle;
			}
			if (actor.\u0018())
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
				if (!(actor.\u000E() == null))
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
					result = (actor.\u000E().IsVisibleToClient() ? 1 : 0);
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.ProcessSequenceVisibility()).MethodHandle;
			}
			if (!this.MarkedForRemoval)
			{
				if (this.m_phaseVisibilityType != Sequence.PhaseBasedVisibilityType.Any)
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
					if (GameFlowData.Get() != null)
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
						GameState gameState = GameFlowData.Get().gameState;
						if (this.m_phaseVisibilityType == Sequence.PhaseBasedVisibilityType.InDecisionOnly)
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
							if (gameState != GameState.BothTeams_Decision)
							{
								goto IL_A0;
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
						}
						if (this.m_phaseVisibilityType != Sequence.PhaseBasedVisibilityType.InResolutionOnly)
						{
							goto IL_A8;
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
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
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
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_keepVFXInCinematicCamForCaster)
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
						if (this.Caster != null)
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
							ActorData cinematicTargetActor = CameraManager.Get().GetCinematicTargetActor();
							int cinematicActionAnimIndex = CameraManager.Get().GetCinematicActionAnimIndex();
							bool flag3;
							if (this.m_keepCasterVFXForAnimIndex > 0)
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
								for (;;)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								flag5 = (this.AgeInTurns == 0);
							}
							else
							{
								flag5 = true;
							}
							bool flag6 = flag5;
							if (cinematicTargetActor.ActorIndex == this.Caster.ActorIndex && flag4)
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
								if (flag6)
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
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					this.SetSequenceVisibility(false);
				}
				else
				{
					if (activeOwnedActorData == null)
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
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
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
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						bool flag7;
						if (this.Targets != null)
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
							if (this.Targets.Length > 0)
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
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						this.SetSequenceVisibility(this.IsTargetPosVisible());
					}
					else if (this.m_visibilityType == Sequence.VisibilityType.AlwaysOnlyIfCaster)
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
						this.SetSequenceVisibility(this.Caster == activeOwnedActorData);
					}
					else if (this.m_visibilityType == Sequence.VisibilityType.AlwaysOnlyIfTarget)
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
						this.SetSequenceVisibility(this.Target == activeOwnedActorData);
					}
					else if (this.m_visibilityType == Sequence.VisibilityType.AlwaysIfCastersTeam)
					{
						if (this.Caster != null && activeOwnedActorData != null)
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
							this.SetSequenceVisibility(this.Caster.\u000E() == activeOwnedActorData.\u000E());
						}
						else
						{
							this.SetSequenceVisibility(true);
						}
					}
					else if (this.m_visibilityType == Sequence.VisibilityType.SequencePosition)
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
						this.SetSequenceVisibility(this.IsSequencePosVisible());
					}
					else if (this.m_visibilityType == Sequence.VisibilityType.Always)
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
						this.SetSequenceVisibility(true);
					}
					else if (this.m_visibilityType == Sequence.VisibilityType.CastersTeamOrSequencePosition)
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
						if (this.Caster != null)
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
							if (activeOwnedActorData != null)
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
								this.SetSequenceVisibility(this.Caster.\u000E() == activeOwnedActorData.\u000E() || this.IsSequencePosVisible());
								goto IL_4C3;
							}
						}
						this.SetSequenceVisibility(this.IsSequencePosVisible());
						IL_4C3:;
					}
					else if (this.m_visibilityType == Sequence.VisibilityType.TargetPosAndCaster)
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
						bool flag8;
						if (this.IsTargetPosVisible())
						{
							if (!(this.Caster == null))
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
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							if (this.m_visibilityType != Sequence.VisibilityType.CasterIfNotEvading)
							{
								return;
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
						bool sequenceVisibility3 = false;
						ActorData actorData2;
						if (this.m_visibilityType == Sequence.VisibilityType.TargetIfNotEvading)
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
							ActorData actorData;
							if (this.Targets != null)
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
								if (this.Targets.Length > 0)
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
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (this.IsActorConsideredVisible(actorData2))
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
								if (actorData2.\u000E() != null)
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
									sequenceVisibility3 = !actorData2.\u000E().InChargeState();
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.CallHitSequenceOnTargets(Vector3, float, List<ActorData>, bool)).MethodHandle;
			}
			for (int i = 0; i < this.Targets.Length; i++)
			{
				if (this.Targets[i] != null)
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
					float magnitude = (this.Targets[i].transform.position - impactPos).magnitude;
					if (magnitude > num)
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
						num = magnitude;
					}
				}
			}
		}
		float num2;
		if (num < Board.\u000E().squareSize / 2f)
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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			for (int j = 0; j < this.Targets.Length; j++)
			{
				if (this.Targets[j] != null)
				{
					if (actorsToIgnore != null)
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
						if (actorsToIgnore.Contains(this.Targets[j]))
						{
							goto IL_130;
						}
					}
					this.Source.OnSequenceHit(this, this.Targets[j], impulseInfo, ActorModelData.RagdollActivation.HealthBased, tryHitReactIfAlreadyHit);
				}
				IL_130:;
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
		this.Source.OnSequenceHit(this, this.TargetPos, impulseInfo);
		List<Team> list = new List<Team>();
		list.Add(this.Caster.\u0012());
		list.Add(this.Caster.\u000E());
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(impactPos, num * 3f, false, this.Caster, list, null, false, default(Vector3));
		for (int k = 0; k < actorsInRadius.Count; k++)
		{
			if (actorsInRadius[k] != null)
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
				if (actorsInRadius[k].\u000E() != null)
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
					Vector3 direction = actorsInRadius[k].transform.position - impactPos;
					direction.y = 0f;
					direction.Normalize();
					actorsInRadius[k].\u000E().ImpartWindImpulse(direction);
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.GetTargetHitPosition(ActorData, JointPopupProperty)).MethodHandle;
			return result;
		}
		return result;
	}

	protected Vector3 GetTargetHitPosition(int targetIndex, JointPopupProperty fxJoint)
	{
		bool flag = false;
		Vector3 result = Vector3.zero;
		if (this.Targets != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.GetTargetHitPosition(int, JointPopupProperty)).MethodHandle;
			}
			if (this.Targets.Length > targetIndex && this.Targets[targetIndex] != null)
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
				if (this.Targets[targetIndex].gameObject != null)
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
					fxJoint.Initialize(this.Targets[targetIndex].gameObject);
					if (fxJoint.m_jointObject != null)
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
						result = fxJoint.m_jointObject.transform.position;
						flag = true;
					}
				}
			}
		}
		if (!flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.GetTargetPosition(int, bool)).MethodHandle;
			}
			if (this.Targets != null)
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
				if (this.Targets.Length > targetIndex && this.Targets[targetIndex] != null)
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
					if (this.Targets[targetIndex].gameObject != null)
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
						GameObject gameObject = this.Targets[targetIndex].gameObject.FindInChildren(Sequence.s_defaultHitAttachJoint, 0);
						if (gameObject != null)
						{
							return gameObject.transform.position + Vector3.up;
						}
						gameObject = this.Targets[targetIndex].gameObject.FindInChildren(Sequence.s_defaultFallbackHitAttachJoint, 0);
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
							return gameObject.transform.position + Vector3.up;
						}
						return this.Targets[targetIndex].gameObject.transform.position;
					}
				}
			}
		}
		if (this.TargetSquare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.IsHitFXVisible(ActorData)).MethodHandle;
			}
			if (hitVisibilityType != Sequence.HitVisibilityType.Target)
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
				result = true;
			}
			else
			{
				bool flag;
				if (hitTarget)
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
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (activeOwnedActorData != null)
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
					if (hitTarget.\u000E() == this.Caster.\u000E())
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
						if (activeOwnedActorData.\u000E() != hitTarget.\u000E())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.IsHitFXVisibleWrtTeamFilter(ActorData, Sequence.HitVFXSpawnTeam)).MethodHandle;
			}
			if (hitTarget != null)
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
				if (teamFilter != Sequence.HitVFXSpawnTeam.AllTargets)
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
					bool flag2 = this.Caster.\u000E() == hitTarget.\u000E();
					bool flag3;
					if (teamFilter != Sequence.HitVFXSpawnTeam.AllTargets)
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
						if (teamFilter == Sequence.HitVFXSpawnTeam.AllExcludeCaster)
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
							if (this.Caster != hitTarget)
							{
								goto IL_D5;
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
						if (teamFilter == Sequence.HitVFXSpawnTeam.AllyAndCaster)
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
							if (flag2)
							{
								goto IL_D5;
							}
						}
						if (teamFilter == Sequence.HitVFXSpawnTeam.EnemyOnly)
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
		return actor != null && actor.\u0012();
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.InstantiateFX(GameObject, Vector3, Quaternion, bool, bool)).MethodHandle;
			}
			this.InitializeFXStorage();
		}
		if (tryApplyCameraOffset)
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
			if (prefab != null)
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
				if (prefab.GetComponent<OffsetVFXTowardsCamera>())
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
					position = OffsetVFXTowardsCamera.ProcessOffset(position);
				}
			}
		}
		GameObject gameObject;
		if (prefab != null)
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
			gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, position, rotation);
		}
		else
		{
			gameObject = new GameObject("FallbackForNullFx");
			if (Application.isEditor)
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
				if (logErrorOnNullPrefab)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.ReplaceVFXPrefabs(GameObject)).MethodHandle;
			}
			if (this.Caster != null)
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
				gameEventManager.FireEvent(GameEventManager.EventType.ReplaceVFXPrefab, new GameEventManager.ReplaceVFXPrefab
				{
					characterResourceLink = this.Caster.\u000E(),
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.SetAttribute(GameObject, string, int)).MethodHandle;
			}
			PKFxFX[] componentsInChildren = fx.GetComponentsInChildren<PKFxFX>(true);
			foreach (PKFxFX pkfxFX in componentsInChildren)
			{
				if (pkfxFX != null)
				{
					PKFxManager.AttributeDesc desc = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Int, name);
					if (pkfxFX.AttributeExists(desc))
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
						pkfxFX.SetAttribute(new PKFxManager.Attribute(desc)
						{
							m_Value0 = (float)value
						});
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

	internal static void SetAttribute(GameObject fx, string name, float value)
	{
		if (fx != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.SetAttribute(GameObject, string, float)).MethodHandle;
			}
			PKFxFX[] componentsInChildren = fx.GetComponentsInChildren<PKFxFX>(true);
			foreach (PKFxFX pkfxFX in componentsInChildren)
			{
				if (pkfxFX != null)
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
					PKFxManager.AttributeDesc desc = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Float, name);
					if (pkfxFX.AttributeExists(desc))
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
						pkfxFX.SetAttribute(new PKFxManager.Attribute(desc)
						{
							m_Value0 = value
						});
					}
				}
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
	}

	internal static void SetAttribute(GameObject fx, string name, Vector3 value)
	{
		if (fx != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.SetAttribute(GameObject, string, Vector3)).MethodHandle;
			}
			PKFxFX[] componentsInChildren = fx.GetComponentsInChildren<PKFxFX>(true);
			foreach (PKFxFX pkfxFX in componentsInChildren)
			{
				if (pkfxFX != null)
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
					PKFxManager.AttributeDesc desc = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Float3, name);
					if (pkfxFX.AttributeExists(desc))
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
						pkfxFX.SetAttribute(new PKFxManager.Attribute(desc)
						{
							m_Value0 = value.x,
							m_Value1 = value.y,
							m_Value2 = value.z
						});
					}
				}
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
	}

	internal bool AreFXFinished(GameObject fx)
	{
		if (!this.Source.RemoveAtEndOfTurn)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.AreFXFinished(GameObject)).MethodHandle;
			}
			return false;
		}
		bool result = false;
		if (fx != null)
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
			if (!fx.activeSelf)
			{
				result = true;
			}
			else
			{
				if (fx.GetComponent<ParticleSystem>() != null)
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
					if (!fx.GetComponent<ParticleSystem>().IsAlive())
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
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							return false;
						}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.GetFXDuration(GameObject)).MethodHandle;
			}
			ParticleSystem component = fxPrefab.GetComponent<ParticleSystem>();
			if (component != null)
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
				result = component.main.duration;
			}
			else
			{
				PKFxManager.AttributeDesc attributeDesc = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Float, "Duration");
				attributeDesc.DefaultValue0 = 1f;
				PKFxFX[] componentsInChildren = fxPrefab.GetComponentsInChildren<PKFxFX>(true);
				if (componentsInChildren.Length > 0)
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
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						if (componentsInChildren[i] != null && componentsInChildren[i].AttributeExists(attributeDesc))
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
							result = Mathf.Max(new float[]
							{
								componentsInChildren[i].GetAttributeFromDesc(attributeDesc).ValueFloat
							});
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
		return result;
	}

	public static GameObject SpawnAndAttachFx(Sequence sequence, GameObject fxPrefab, ActorData targetActor, JointPopupProperty fxJoint, bool attachToJoint, bool aimAtCaster, bool reverseDir)
	{
		GameObject gameObject = null;
		if (targetActor != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.SpawnAndAttachFx(Sequence, GameObject, ActorData, JointPopupProperty, bool, bool, bool)).MethodHandle;
			}
			if (!fxJoint.IsInitialized())
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
				fxJoint.Initialize(targetActor.gameObject);
			}
			if (fxPrefab != null)
			{
				if (fxJoint.m_jointObject != null)
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
					if (fxJoint.m_jointObject.transform.localScale != Vector3.zero)
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
						if (attachToJoint)
						{
							gameObject = sequence.InstantiateFX(fxPrefab);
							sequence.AttachToBone(gameObject, fxJoint.m_jointObject);
							gameObject.transform.localPosition = Vector3.zero;
							gameObject.transform.localRotation = Quaternion.identity;
							Quaternion rotation = default(Quaternion);
							if (aimAtCaster)
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
								Vector3 position = sequence.Caster.transform.position;
								Vector3 vector = position - fxJoint.m_jointObject.transform.position;
								vector.y = 0f;
								vector.Normalize();
								if (reverseDir)
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
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					Vector3 position3 = sequence.Caster.transform.position;
					Vector3 vector2 = position3 - position2;
					vector2.y = 0f;
					vector2.Normalize();
					if (reverseDir)
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.AttachToBone(GameObject, GameObject)).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.CreateImpulseInfoWithActorForward(ActorData)).MethodHandle;
			}
			return new ActorModelData.ImpulseInfo(actor.transform.position, Vector3.up + actor.transform.forward);
		}
		return null;
	}

	internal static ActorModelData.ImpulseInfo CreateImpulseInfoBetweenActors(ActorData fromActor, ActorData targetActor)
	{
		if (fromActor != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.CreateImpulseInfoBetweenActors(ActorData, ActorData)).MethodHandle;
			}
			if (targetActor != null)
			{
				if (fromActor == targetActor)
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.ToString()).MethodHandle;
			}
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.GetTargetsString()).MethodHandle;
			}
			if (this.Targets.Length > 0)
			{
				for (int i = 0; i < this.Targets.Length; i++)
				{
					ActorData actorData = this.Targets[i];
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
						if (text.Length > 0)
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
							text += " | ";
						}
						text += actorData.ActorIndex.ToString();
					}
				}
				for (;;)
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

	public void OverridePhaseTimingParams(Sequence.PhaseTimingParameters timingParams, Sequence.IExtraSequenceParams iParams)
	{
		if (iParams != null && iParams is Sequence.PhaseTimingExtraParams)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.OverridePhaseTimingParams(Sequence.PhaseTimingParameters, Sequence.IExtraSequenceParams)).MethodHandle;
			}
			Sequence.PhaseTimingExtraParams phaseTimingExtraParams = iParams as Sequence.PhaseTimingExtraParams;
			if (phaseTimingExtraParams != null)
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
				if (timingParams != null && timingParams.m_acceptOverrideFromParams)
				{
					if ((int)phaseTimingExtraParams.m_turnDelayStartOverride >= 0)
					{
						timingParams.m_turnDelayStart = (int)phaseTimingExtraParams.m_turnDelayStartOverride;
					}
					if ((int)phaseTimingExtraParams.m_turnDelayEndOverride >= 0)
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
						timingParams.m_turnDelayEnd = (int)phaseTimingExtraParams.m_turnDelayEndOverride;
					}
					if ((int)phaseTimingExtraParams.m_abilityPhaseStartOverride >= 0)
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
						timingParams.m_usePhaseStartTiming = true;
						timingParams.m_abilityPhaseStart = (AbilityPriority)phaseTimingExtraParams.m_abilityPhaseStartOverride;
					}
					if ((int)phaseTimingExtraParams.m_abilityPhaseEndOverride >= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.GetInEditorDescription()).MethodHandle;
			}
			text = "<empty>";
		}
		string str = "Setup Note: " + text + "\n----------\n";
		if (this.m_targetHitAnimation)
		{
			str += "<[x] Target Hit Animation> Can trigger hit react anim\n\n";
			if (this.m_canTriggerHitReactOnAllyHit)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.GetVisibilityDescription()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.GetVisibilityTypeDescription(Sequence.VisibilityType, bool*, bool*)).MethodHandle;
			}
			result = "always visible";
		}
		else if (visType == Sequence.VisibilityType.Caster)
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
			result = "visible if " + Sequence.c_casterToken + " is visible";
		}
		else if (visType == Sequence.VisibilityType.CasterOrTarget)
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			result = "visible if " + Sequence.c_targetPosToken + " square is visible";
			usesTargetPos = true;
		}
		else if (visType == Sequence.VisibilityType.AlwaysOnlyIfCaster)
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.PhaseTimingParameters.OnTurnStart(int)).MethodHandle;
				}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.PhaseTimingParameters.OnAbilityPhaseStart(AbilityPriority)).MethodHandle;
				}
				if (abilityPhase == this.m_abilityPhaseStart)
				{
					this.m_started = true;
				}
			}
			if (this.m_turnDelayEnd <= 0)
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
				if (abilityPhase == this.m_abilityPhaseEnd)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.PhaseTimingParameters.ShouldSequenceBeActive()).MethodHandle;
				}
			}
			bool result;
			if (this.m_finished)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.PhaseTimingParameters.ShouldSpawnSequence(AbilityPriority)).MethodHandle;
				}
				if (this.m_usePhaseStartTiming)
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
			return result;
		}

		internal bool ShouldStopSequence(AbilityPriority abilityPhase)
		{
			bool result = false;
			if (this.m_turnDelayEnd == 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.PhaseTimingParameters.ShouldStopSequence(AbilityPriority)).MethodHandle;
				}
				if (abilityPhase == this.m_abilityPhaseEnd)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.FxAttributeParam.GetAttributeName()).MethodHandle;
				}
				return "scaleControl";
			}
			if (this.m_paramNameCode == Sequence.FxAttributeParam.ParamNameCode.LengthInSquares)
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
				return "lengthInSquares";
			}
			if (this.m_paramNameCode == Sequence.FxAttributeParam.ParamNameCode.WidthInSquares)
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
				return "widthInSquares";
			}
			if (this.m_paramNameCode == Sequence.FxAttributeParam.ParamNameCode.AbilityAreaLength)
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
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(Sequence.GenericActorListParam.XSP_SerializeToStream(IBitStream)).MethodHandle;
				}
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
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					b3 = (sbyte)actorData.ActorIndex;
				}
				else
				{
					b3 = (sbyte)ActorData.s_invalidActorIndex;
				}
				sbyte b4 = b3;
				stream.Serialize(ref b4);
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
