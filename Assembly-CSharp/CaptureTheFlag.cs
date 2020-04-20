using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class CaptureTheFlag : NetworkBehaviour, IGameEventListener
{
	public static byte s_nextFlagGuid;

	private static CaptureTheFlag s_instance;

	[Header("Locations")]
	public BoardRegion m_flagSpawnsNeutral;

	public BoardRegion m_flagSpawnsTeamA;

	public BoardRegion m_flagSpawnsTeamB;

	public BoardRegion m_flagTurninTeamA;

	public BoardRegion m_flagTurninTeamB;

	public BoardRegion m_flagTurninNeutral;

	public List<BoardRegion> m_potentialFlagTurnins;

	public bool m_potentialTurninsAreTeamSpecific = true;

	public float m_potentialTurninRegion_minDistFromFlag = -1f;

	public float m_potentialTurninRegion_maxDistFromFlag = -1f;

	public float m_potentialTurninRegion_desiredDistFromFlag = -1f;

	public float m_potentialTurninRegion_minDistFromTurnin = -1f;

	public float m_potentialTurninRegion_maxDistFromTurnin = -1f;

	[Header("Visuals")]
	public bool m_autoGenerateTurninVisuals = true;

	public bool m_autoGenerateSpawnLocVisuals;

	public float m_boundaryOscillationSpeed = 3.14159f;

	public float m_boundaryOscillationHeight = 0.05f;

	public Color m_primaryColor_friendly = Color.blue;

	public Color m_primaryColor_hostile = Color.red;

	public Color m_primaryColor_neutral = Color.gray;

	public Color m_secondaryColor_locked = Color.yellow;

	public Color m_neutralFlagColor_idle = Color.gray;

	public Color m_neutralFlagColor_held = Color.white;

	public Color m_friendlyFlagColor_idle = Color.blue;

	public Color m_friendlyFlagColor_held = Color.blue;

	public Color m_hostileFlagColor_idle = Color.red;

	public Color m_hostileFlagColor_held = Color.red;

	public Color m_textColor_positive = Color.blue;

	public Color m_textColor_negative = Color.red;

	public Color m_textColor_neutral = Color.white;

	public float m_timeTillCameraFocusesOntoExtractionPoint = 0.5f;

	public float m_timeTillCameraFocusesOntoExtraction = 0.1f;

	[Header("Capturing Logic")]
	public GameplayRewardForTeam m_rewardToCapturingTeam;

	public GameplayRewardForTeam m_rewardToOtherTeam;

	public int m_disableTurninTeamAUntilTheirScore = -1;

	public int m_disableTurninTeamBUntilTheirScore = -1;

	public int m_disableTurninNeutralUntilAnyScore = -1;

	public int m_numTurnsToLockTurninOnEnable;

	public CaptureTheFlag.TurninRegionState m_turninRegionInitialState;

	public List<CaptureTheFlag.TurninType> m_turnInRequirements;

	[Header("Spawning Logic")]
	public CaptureTheFlag.FlagSpawnData m_neutralFlagSpawningLogic;

	public CaptureTheFlag.FlagSpawnData m_teamAFlagSpawningLogic;

	public CaptureTheFlag.FlagSpawnData m_teamBFlagSpawningLogic;

	public GameObject m_flagPrefab;

	public StandardEffectInfo m_flagHolderEffect;

	public bool m_flagRevealsHolder = true;

	public StandardEffectInfo m_onDroppedFlagEffect;

	public StandardEffectInfo m_onTurnedInFlagEffect;

	public StandardEffectInfo m_onReturnedFlagEffect;

	[Header("Special movement rules")]
	public bool m_evasionDropsFlags = true;

	public bool m_beingKnockedBackDropsFlags;

	public bool m_evadersCanPickUpFlags;

	public bool m_knockbackedMoversCanPickUpFlags;

	public bool m_disableAllyReturningOwnFlags;

	public bool m_allowFlagJuggling;

	public int m_damageInOneTurnToDropFlag_gross = -1;

	public int m_damageSincePickedUpToDropFlag_gross = -1;

	public int m_damageThesholdIncreaseOnDrop;

	[Header("Sequences")]
	public GameObject m_flagPickedUpSequence;

	public GameObject m_flagDroppedSequence;

	public GameObject m_flagTurnedInSequence;

	public GameObject m_flagReturnedToSpawnSequence;

	public GameObject m_flagBeingHeldSequence;

	public GameObject m_friendlyTurninRegionActivatedSequence;

	public GameObject m_enemyTurninRegionActivatedSequence;

	public GameObject m_neutralTurninRegionActivatedSequence;

	public Sprite m_flagIcon;

	public Sprite m_turnInRegionIcon;

	[Header("Objective Points")]
	public CaptureTheFlag.FlagHolderObjectivePointData m_objectivePointsData_flagHoldersTeam;

	public CaptureTheFlag.FlagHolderObjectivePointData m_objectivePointsData_otherTeam;

	[Header("Strings")]
	public string m_alliedExtractionPointNowActive;

	public string m_alliedExtractionPointNowInactive;

	public string m_alliedExtractionPointNowUnlocking;

	public string m_enemyExtractionPointNowActive;

	public string m_enemyExtractionPointNowInactive;

	public string m_enemyExtractionPointNowUnlocking;

	public string m_neutralExtractionPointNowActive;

	public string m_neutralExtractionPointNowInactive;

	public string m_neutralExtractionPointNowUnlocking;

	private List<CTF_Flag> m_flags;

	private int m_teamACaptures;

	private int m_teamBCaptures;

	private GameObject m_autoBoundary_spawn_teamA;

	private GameObject m_autoBoundary_spawn_teamB;

	private GameObject m_autoBoundary_spawn_neutral;

	private GameObject m_autoBoundary_turnin_teamA;

	private GameObject m_autoBoundary_turnin_teamB;

	private GameObject m_autoBoundary_turnin_neutral;

	private float m_autoBoundaryHeight;

	private float m_timeToFocusCameraOnTurninTeamA = -1f;

	private float m_timeToFocusCameraOnTurninTeamB = -1f;

	private float m_timeToFocusCameraOnTurninNeutral = -1f;

	private float m_timeToFocusCameraOnExtraction = -1f;

	private BoardSquare m_lastExtractionSquare;

	[SyncVar(hook = "HookSetTurninRegionState_TeamA")]
	private int m_turninRegionState_TeamA;

	[SyncVar(hook = "HookSetTurninRegionState_TeamB")]
	private int m_turninRegionState_TeamB;

	[SyncVar(hook = "HookSetTurninRegionState_Neutral")]
	private int m_turninRegionState_Neutral;

	[SyncVar(hook = "HookSetTurninRegionIndex_TeamA")]
	private int m_turninRegionIndex_TeamA = -1;

	[SyncVar(hook = "HookSetTurninRegionIndex_TeamB")]
	private int m_turninRegionIndex_TeamB = -1;

	[SyncVar(hook = "HookSetTurninRegionIndex_Neutral")]
	private int m_turninRegionIndex_Neutral = -1;

	[SyncVar(hook = "HookSetNumFlagDrops")]
	private int m_numFlagDrops;

	private int m_clientUnresolvedNumFlagDrops;

	[SyncVar]
	private uint m_sequenceSourceId;

	private SequenceSource _sequenceSource;

	private float m_lastFlagCarrierDamageCur;

	private float m_lastFlagCarrierDamageMax = 1f;

	private void Awake()
	{
		if (CaptureTheFlag.s_instance == null)
		{
			CaptureTheFlag.s_instance = this;
		}
		else
		{
			Log.Error("Multiple CaptureTheFlag components in this scene, remove extraneous ones.", new object[0]);
		}
		if (NetworkServer.active)
		{
			SequenceSource sequenceSource = new SequenceSource(null, null, false, null, null);
			this.Networkm_sequenceSourceId = sequenceSource.RootID;
		}
	}

	private void OnDestroy()
	{
		if (this.m_autoBoundary_spawn_neutral != null)
		{
			HighlightUtils.DestroyBoundaryHighlightObject(this.m_autoBoundary_spawn_neutral);
			this.m_autoBoundary_spawn_neutral = null;
		}
		if (this.m_autoBoundary_spawn_teamA != null)
		{
			HighlightUtils.DestroyBoundaryHighlightObject(this.m_autoBoundary_spawn_teamA);
			this.m_autoBoundary_spawn_teamA = null;
		}
		if (this.m_autoBoundary_spawn_teamB != null)
		{
			HighlightUtils.DestroyBoundaryHighlightObject(this.m_autoBoundary_spawn_teamB);
			this.m_autoBoundary_spawn_teamB = null;
		}
		if (this.m_autoBoundary_turnin_neutral != null)
		{
			HighlightUtils.DestroyBoundaryHighlightObject(this.m_autoBoundary_turnin_neutral);
			this.m_autoBoundary_turnin_neutral = null;
		}
		if (this.m_autoBoundary_turnin_teamA != null)
		{
			HighlightUtils.DestroyBoundaryHighlightObject(this.m_autoBoundary_turnin_teamA);
			this.m_autoBoundary_turnin_teamA = null;
		}
		if (this.m_autoBoundary_turnin_teamB != null)
		{
			HighlightUtils.DestroyBoundaryHighlightObject(this.m_autoBoundary_turnin_teamB);
			this.m_autoBoundary_turnin_teamB = null;
		}
		CaptureTheFlag.s_instance = null;
	}

	public static CaptureTheFlag Get()
	{
		return CaptureTheFlag.s_instance;
	}

	internal SequenceSource SequenceSource
	{
		get
		{
			if (this._sequenceSource == null)
			{
				this._sequenceSource = new SequenceSource(null, null, this.m_sequenceSourceId, false);
			}
			return this._sequenceSource;
		}
	}

	private CaptureTheFlag.TurninRegionState TurninRegionState_TeamA
	{
		get
		{
			return (CaptureTheFlag.TurninRegionState)this.m_turninRegionState_TeamA;
		}
		set
		{
			if (this.m_turninRegionState_TeamA != (int)value)
			{
				this.Networkm_turninRegionState_TeamA = (int)value;
			}
		}
	}

	private CaptureTheFlag.TurninRegionState TurninRegionState_TeamB
	{
		get
		{
			return (CaptureTheFlag.TurninRegionState)this.m_turninRegionState_TeamB;
		}
		set
		{
			if (this.m_turninRegionState_TeamB != (int)value)
			{
				this.Networkm_turninRegionState_TeamB = (int)value;
			}
		}
	}

	private CaptureTheFlag.TurninRegionState TurninRegionState_Neutral
	{
		get
		{
			return (CaptureTheFlag.TurninRegionState)this.m_turninRegionState_Neutral;
		}
		set
		{
			if (this.m_turninRegionState_Neutral != (int)value)
			{
				this.Networkm_turninRegionState_Neutral = (int)value;
			}
		}
	}

	private BoardRegion FlagTurninRegion_TeamA
	{
		get
		{
			if (this.TurninRegionState_TeamA == CaptureTheFlag.TurninRegionState.Active)
			{
				if (this.m_potentialFlagTurnins != null)
				{
					if (this.m_potentialFlagTurnins.Count != 0)
					{
						if (this.m_potentialTurninsAreTeamSpecific)
						{
							if (this.m_turninRegionIndex_TeamA == -1)
							{
								return null;
							}
							return this.m_potentialFlagTurnins[this.m_turninRegionIndex_TeamA];
						}
					}
				}
				return this.m_flagTurninTeamA;
			}
			return null;
		}
	}

	private BoardRegion FlagTurninRegion_TeamB
	{
		get
		{
			if (this.TurninRegionState_TeamB == CaptureTheFlag.TurninRegionState.Active)
			{
				if (this.m_potentialFlagTurnins != null)
				{
					if (this.m_potentialFlagTurnins.Count != 0)
					{
						if (!this.m_potentialTurninsAreTeamSpecific)
						{
						}
						else
						{
							if (this.m_turninRegionIndex_TeamB == -1)
							{
								return null;
							}
							return this.m_potentialFlagTurnins[this.m_turninRegionIndex_TeamB];
						}
					}
				}
				return this.m_flagTurninTeamB;
			}
			return null;
		}
	}

	private BoardRegion FlagTurninRegion_Neutral
	{
		get
		{
			if (this.TurninRegionState_Neutral == CaptureTheFlag.TurninRegionState.Active)
			{
				if (this.m_potentialFlagTurnins != null)
				{
					if (this.m_potentialFlagTurnins.Count != 0)
					{
						if (!this.m_potentialTurninsAreTeamSpecific)
						{
							if (this.m_turninRegionIndex_Neutral == -1)
							{
								return null;
							}
							return this.m_potentialFlagTurnins[this.m_turninRegionIndex_Neutral];
						}
					}
				}
				return this.m_flagTurninNeutral;
			}
			return null;
		}
	}

	public void OnNewFlagStarted(CTF_Flag flag)
	{
		if (!this.m_flags.Contains(flag))
		{
			this.m_flags.Add(flag);
		}
	}

	public void OnFlagDestroyed(CTF_Flag flag)
	{
		if (this.m_flags.Contains(flag))
		{
			this.m_flags.Remove(flag);
		}
	}

	private CTF_Flag GetFlagByGuid(byte flagGuid)
	{
		for (int i = 0; i < this.m_flags.Count; i++)
		{
			if (this.m_flags[i] != null)
			{
				if (this.m_flags[i].m_flagGuid == flagGuid)
				{
					return this.m_flags[i];
				}
			}
		}
		return null;
	}

	private List<CTF_Flag> GetFlagsHeldByActor_Server(ActorData actor)
	{
		List<CTF_Flag> list = null;
		for (int i = 0; i < this.m_flags.Count; i++)
		{
			if (this.m_flags[i] != null)
			{
				if (this.m_flags[i].ServerHolderActor == actor)
				{
					if (list == null)
					{
						list = new List<CTF_Flag>();
					}
					list.Add(this.m_flags[i]);
				}
			}
		}
		return list;
	}

	private List<CTF_Flag> GetFlagsHeldByActor_Client(ActorData actor)
	{
		List<CTF_Flag> list = null;
		if (actor != null)
		{
			for (int i = 0; i < this.m_flags.Count; i++)
			{
				if (this.m_flags[i] != null)
				{
					if (this.m_flags[i].ClientHolderActor == actor)
					{
						if (list == null)
						{
							list = new List<CTF_Flag>();
						}
						list.Add(this.m_flags[i]);
					}
				}
			}
		}
		return list;
	}

	public static CTF_Flag GetMainFlag()
	{
		if (CaptureTheFlag.s_instance == null)
		{
			return null;
		}
		if (CaptureTheFlag.s_instance.m_flags != null)
		{
			if (CaptureTheFlag.s_instance.m_flags.Count > 0)
			{
				return CaptureTheFlag.s_instance.m_flags[0];
			}
		}
		return null;
	}

	public static BoardSquare GetMainFlagIdleSquare_Server()
	{
		CTF_Flag mainFlag = CaptureTheFlag.GetMainFlag();
		if (mainFlag != null)
		{
			return mainFlag.ServerIdleSquare;
		}
		return null;
	}

	public static BoardSquare GetMainFlagIdleSquare_Client()
	{
		CTF_Flag mainFlag = CaptureTheFlag.GetMainFlag();
		if (mainFlag != null)
		{
			return mainFlag.ClientIdleSquare;
		}
		return null;
	}

	public static ActorData GetMainFlagCarrier_Server()
	{
		CTF_Flag mainFlag = CaptureTheFlag.GetMainFlag();
		if (mainFlag != null)
		{
			return mainFlag.ServerHolderActor;
		}
		return null;
	}

	public static ActorData GetMainFlagCarrier_Client()
	{
		CTF_Flag mainFlag = CaptureTheFlag.GetMainFlag();
		if (mainFlag != null)
		{
			return mainFlag.ClientHolderActor;
		}
		return null;
	}

	public static List<ActorData> GetActorsRevealedByFlags_Client()
	{
		List<ActorData> list = new List<ActorData>();
		if (NetworkClient.active)
		{
			if (CaptureTheFlag.Get() != null)
			{
				if (CaptureTheFlag.Get().m_flagRevealsHolder)
				{
					List<CTF_Flag> flags = CaptureTheFlag.Get().m_flags;
					for (int i = 0; i < flags.Count; i++)
					{
						CTF_Flag ctf_Flag = flags[i];
						if (ctf_Flag != null && ctf_Flag.ClientHolderActor != null)
						{
							if (!list.Contains(ctf_Flag.ClientHolderActor))
							{
								list.Add(ctf_Flag.ClientHolderActor);
							}
						}
					}
				}
			}
		}
		return list;
	}

	public static bool IsActorRevealedByFlag_Client(ActorData actor)
	{
		List<ActorData> actorsRevealedByFlags_Client = CaptureTheFlag.GetActorsRevealedByFlags_Client();
		if (actorsRevealedByFlags_Client.Contains(actor))
		{
			return true;
		}
		return false;
	}

	public static List<ActorData> GetActorsRevealedByFlags_Server()
	{
		List<ActorData> list = new List<ActorData>();
		if (NetworkServer.active && CaptureTheFlag.Get() != null)
		{
			if (CaptureTheFlag.Get().m_flagRevealsHolder)
			{
				List<CTF_Flag> flags = CaptureTheFlag.Get().m_flags;
				for (int i = 0; i < flags.Count; i++)
				{
					CTF_Flag ctf_Flag = flags[i];
					if (ctf_Flag != null)
					{
						if (ctf_Flag.GatheredHolderActor != null)
						{
							if (!list.Contains(ctf_Flag.GatheredHolderActor))
							{
								list.Add(ctf_Flag.GatheredHolderActor);
							}
						}
					}
				}
			}
		}
		return list;
	}

	public static bool IsActorRevealedByFlag_Server(ActorData actor)
	{
		List<ActorData> actorsRevealedByFlags_Server = CaptureTheFlag.GetActorsRevealedByFlags_Server();
		return actorsRevealedByFlags_Server.Contains(actor);
	}

	public static BoardRegion GetExtractionRegion()
	{
		if (CaptureTheFlag.s_instance != null)
		{
			return CaptureTheFlag.s_instance.FlagTurninRegion_Neutral;
		}
		return null;
	}

	public static BoardRegion GetExtractionRegionOfTeam(Team team)
	{
		if (!(CaptureTheFlag.s_instance != null))
		{
			return null;
		}
		if (team == Team.TeamA)
		{
			return CaptureTheFlag.s_instance.FlagTurninRegion_TeamA;
		}
		if (team == Team.TeamB)
		{
			return CaptureTheFlag.s_instance.FlagTurninRegion_TeamB;
		}
		return CaptureTheFlag.s_instance.FlagTurninRegion_Neutral;
	}

	public static CaptureTheFlag.TurninRegionState GetExtractionRegionState()
	{
		if (CaptureTheFlag.s_instance != null)
		{
			return CaptureTheFlag.s_instance.TurninRegionState_Neutral;
		}
		return CaptureTheFlag.TurninRegionState.Disabled;
	}

	public static CaptureTheFlag.TurninRegionState GetExtractionRegionStateOfTeam(Team team)
	{
		if (!(CaptureTheFlag.s_instance != null))
		{
			return CaptureTheFlag.TurninRegionState.Disabled;
		}
		if (team == Team.TeamA)
		{
			return CaptureTheFlag.s_instance.TurninRegionState_TeamA;
		}
		if (team == Team.TeamB)
		{
			return CaptureTheFlag.s_instance.TurninRegionState_TeamB;
		}
		return CaptureTheFlag.s_instance.TurninRegionState_Neutral;
	}

	private void Start()
	{
		this.m_flagSpawnsNeutral.Initialize();
		this.m_flagSpawnsTeamA.Initialize();
		this.m_flagSpawnsTeamB.Initialize();
		this.m_flagTurninTeamA.Initialize();
		this.m_flagTurninTeamB.Initialize();
		this.m_flagTurninNeutral.Initialize();
		for (int i = 0; i < this.m_potentialFlagTurnins.Count; i++)
		{
			this.m_potentialFlagTurnins[i].Initialize();
		}
		this.TurninRegionState_TeamA = this.m_turninRegionInitialState;
		this.TurninRegionState_TeamB = this.m_turninRegionInitialState;
		this.TurninRegionState_Neutral = this.m_turninRegionInitialState;
		if (NetworkClient.active)
		{
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.ActorDamaged_Client);
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.ActorHealed_Client);
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.ActorGainedAbsorb_Client);
			this.GenerateBoundaryVisuals();
		}
		this.m_flags = new List<CTF_Flag>();
	}

	public void Client_OnActorDeath(ActorData actor)
	{
		bool flag = false;
		List<ActorData> contributorsToKillOnClient = GameFlowData.Get().GetContributorsToKillOnClient(actor, true);
		List<ActorData> contributorsToKillOnClient2 = GameFlowData.Get().GetContributorsToKillOnClient(actor, false);
		List<ActorData> list = new List<ActorData>();
		List<ActorData> list2 = new List<ActorData>();
		using (List<CTF_Flag>.Enumerator enumerator = this.m_flags.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CTF_Flag ctf_Flag = enumerator.Current;
				if (ctf_Flag.ClientHolderActor != null)
				{
					if (ctf_Flag.ClientHolderActor == actor)
					{
						flag = true;
						ctf_Flag.OnDropped_Client(actor.GetMostResetDeathSquare(), -1);
					}
					if (contributorsToKillOnClient.Contains(ctf_Flag.ClientHolderActor))
					{
						list.Add(ctf_Flag.ClientHolderActor);
					}
					if (contributorsToKillOnClient2.Contains(ctf_Flag.ClientHolderActor))
					{
						list2.Add(ctf_Flag.ClientHolderActor);
					}
				}
			}
		}
		if (flag)
		{
			if (ObjectivePoints.Get() != null)
			{
				ObjectivePoints.Get().AdjustUnresolvedPoints(this.m_objectivePointsData_flagHoldersTeam.m_pointsPerDeathOfFlagHolder, actor.GetTeam());
				ObjectivePoints.Get().AdjustUnresolvedPoints(this.m_objectivePointsData_otherTeam.m_pointsPerDeathOfFlagHolder, actor.GetOpposingTeam());
			}
		}
		foreach (ActorData actorData in list)
		{
			if (ObjectivePoints.Get() != null)
			{
				ObjectivePoints.Get().AdjustUnresolvedPoints(this.m_objectivePointsData_flagHoldersTeam.m_pointsPerDeathblowByFlagHolder, actorData.GetTeam());
				ObjectivePoints.Get().AdjustUnresolvedPoints(this.m_objectivePointsData_otherTeam.m_pointsPerDeathblowByFlagHolder, actorData.GetOpposingTeam());
			}
		}
		using (List<ActorData>.Enumerator enumerator3 = list2.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				ActorData actorData2 = enumerator3.Current;
				if (ObjectivePoints.Get() != null)
				{
					ObjectivePoints.Get().AdjustUnresolvedPoints(this.m_objectivePointsData_flagHoldersTeam.m_pointsPerTakedownByFlagHolder, actorData2.GetTeam());
					ObjectivePoints.Get().AdjustUnresolvedPoints(this.m_objectivePointsData_otherTeam.m_pointsPerTakedownByFlagHolder, actorData2.GetOpposingTeam());
				}
			}
		}
	}

	public void ExecuteClientGameModeEvent(ClientGameModeEvent gameModeEvent)
	{
		if (gameModeEvent == null)
		{
			return;
		}
		GameModeEventType eventType = gameModeEvent.m_eventType;
		byte objectGuid = gameModeEvent.m_objectGuid;
		CTF_Flag flagByGuid = this.GetFlagByGuid(objectGuid);
		int eventGuid = gameModeEvent.m_eventGuid;
		if (flagByGuid == null)
		{
			return;
		}
		if (eventType == GameModeEventType.Ctf_FlagPickedUp)
		{
			ActorData primaryActor = gameModeEvent.m_primaryActor;
			flagByGuid.OnPickedUp_Client(primaryActor, eventGuid);
		}
		else if (eventType == GameModeEventType.Ctf_FlagDropped)
		{
			BoardSquare square = gameModeEvent.m_square;
			flagByGuid.OnDropped_Client(square, eventGuid);
			this.m_clientUnresolvedNumFlagDrops++;
		}
		else if (eventType == GameModeEventType.Ctf_FlagTurnedIn)
		{
			ActorData primaryActor2 = gameModeEvent.m_primaryActor;
			BoardSquare square2 = gameModeEvent.m_square;
			this.TurnInFlag_Client(flagByGuid, primaryActor2, square2, eventGuid);
		}
		else
		{
			if (eventType != GameModeEventType.Ctf_FlagSentToSpawn)
			{
				Debug.LogError("CaptureTheFlag trying to handle non-CtF event type " + eventType.ToString() + ".");
				return;
			}
			ActorData primaryActor3 = gameModeEvent.m_primaryActor;
			flagByGuid.OnReturned_Client(primaryActor3);
		}
	}

	private void TurnInFlag_Client(CTF_Flag flag, ActorData capturingActor, BoardSquare captureSquare, int eventGuid)
	{
		Team team = capturingActor.GetTeam();
		if (team == Team.TeamA)
		{
			this.m_rewardToCapturingTeam.ClientApplyRewardTo(Team.TeamA);
			this.m_rewardToOtherTeam.ClientApplyRewardTo(Team.TeamB);
		}
		else if (team == Team.TeamB)
		{
			this.m_rewardToCapturingTeam.ClientApplyRewardTo(Team.TeamB);
			this.m_rewardToOtherTeam.ClientApplyRewardTo(Team.TeamA);
		}
		this.m_timeToFocusCameraOnExtraction = Time.time + this.m_timeTillCameraFocusesOntoExtraction;
		this.m_lastExtractionSquare = captureSquare;
		flag.OnTurnedIn_Client(capturingActor, eventGuid);
	}

	public void Client_OnFlagHolderChanged(ActorData oldHolder, ActorData newHolder, bool beingTurnedIn, bool alreadyTurnedIn)
	{
		if (oldHolder == newHolder)
		{
			return;
		}
		if (alreadyTurnedIn)
		{
			return;
		}
		UI_CTF_BriefcasePanel.Get().UpdateFlagHolder(oldHolder, newHolder);
	}

	public void Client_OnTurninStateChanged(Team turninRegionTeam, CaptureTheFlag.TurninRegionState prevState, CaptureTheFlag.TurninRegionState newState)
	{
		if (newState != prevState)
		{
			if (InterfaceManager.Get() != null)
			{
				if (GameFlowData.Get() != null)
				{
					if (GameFlowData.Get().LocalPlayerData)
					{
						Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
						if (teamViewing != Team.TeamA)
						{
							if (teamViewing != Team.TeamB)
							{
								goto IL_DE;
							}
						}
						if (turninRegionTeam != Team.TeamA)
						{
							if (turninRegionTeam != Team.TeamB)
							{
								goto IL_DE;
							}
						}
						CaptureTheFlag.RelationshipToClient relationship;
						Color color;
						if (teamViewing == turninRegionTeam)
						{
							relationship = CaptureTheFlag.RelationshipToClient.Friendly;
							color = this.m_textColor_positive;
						}
						else
						{
							relationship = CaptureTheFlag.RelationshipToClient.Hostile;
							color = this.m_textColor_negative;
						}
						goto IL_E7;
						IL_DE:
						relationship = CaptureTheFlag.RelationshipToClient.Neutral;
						color = this.m_textColor_neutral;
						IL_E7:
						string turninStateChangedString = this.GetTurninStateChangedString(relationship, newState);
						string alertText = StringUtil.TR(turninStateChangedString);
						InterfaceManager.Get().DisplayAlert(alertText, color, 5f, true, 1);
					}
				}
			}
		}
	}

	private string GetTurninStateChangedString(CaptureTheFlag.RelationshipToClient relationship, CaptureTheFlag.TurninRegionState newState)
	{
		if (relationship == CaptureTheFlag.RelationshipToClient.Friendly)
		{
			if (newState == CaptureTheFlag.TurninRegionState.Active)
			{
				return this.m_alliedExtractionPointNowActive;
			}
			if (newState == CaptureTheFlag.TurninRegionState.Disabled)
			{
				return this.m_alliedExtractionPointNowInactive;
			}
			if (newState == CaptureTheFlag.TurninRegionState.Locked)
			{
				return this.m_alliedExtractionPointNowUnlocking;
			}
		}
		else if (relationship == CaptureTheFlag.RelationshipToClient.Hostile)
		{
			if (newState == CaptureTheFlag.TurninRegionState.Active)
			{
				return this.m_enemyExtractionPointNowActive;
			}
			if (newState == CaptureTheFlag.TurninRegionState.Disabled)
			{
				return this.m_enemyExtractionPointNowInactive;
			}
			if (newState == CaptureTheFlag.TurninRegionState.Locked)
			{
				return this.m_enemyExtractionPointNowUnlocking;
			}
		}
		else if (relationship == CaptureTheFlag.RelationshipToClient.Neutral)
		{
			if (newState == CaptureTheFlag.TurninRegionState.Active)
			{
				return this.m_neutralExtractionPointNowActive;
			}
			if (newState == CaptureTheFlag.TurninRegionState.Disabled)
			{
				return this.m_neutralExtractionPointNowInactive;
			}
			if (newState == CaptureTheFlag.TurninRegionState.Locked)
			{
				return this.m_neutralExtractionPointNowUnlocking;
			}
		}
		Debug.LogWarning(string.Format("CaptureTheFlag trying to find string for turnin point changed, but failed.  Relationship = {0}, new state = {1}.  Returning empty string...", relationship.ToString(), newState.ToString()));
		return string.Empty;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (args == null)
		{
			return;
		}
		if (NetworkClient.active)
		{
			if (eventType != GameEventManager.EventType.ActorDamaged_Client)
			{
				if (eventType != GameEventManager.EventType.ActorHealed_Client)
				{
					if (eventType != GameEventManager.EventType.ActorGainedAbsorb_Client)
					{
						return;
					}
				}
			}
			this.OnActorHealthChanged(args, true);
		}
	}

	private void OnActorHealthChanged(GameEventManager.GameEventArgs args, bool clientMode)
	{
		if (this.m_evasionDropsFlags)
		{
			if (ServerClientUtils.GetCurrentAbilityPhase() == AbilityPriority.Evasion)
			{
				return;
			}
		}
		GameEventManager.ActorHitHealthChangeArgs actorHitHealthChangeArgs = args as GameEventManager.ActorHitHealthChangeArgs;
		bool fromCharacterSpecificAbility = actorHitHealthChangeArgs.m_fromCharacterSpecificAbility;
		List<CTF_Flag> flagsHeldByActor_Client = this.GetFlagsHeldByActor_Client(actorHitHealthChangeArgs.m_caster);
		if (flagsHeldByActor_Client != null && flagsHeldByActor_Client.Count > 0)
		{
			if (actorHitHealthChangeArgs.m_caster != null)
			{
				Team team = actorHitHealthChangeArgs.m_caster.GetTeam();
				Team opposingTeam = actorHitHealthChangeArgs.m_caster.GetOpposingTeam();
				float pointsPerHealthChange = this.GetPointsPerHealthChange(this.m_objectivePointsData_flagHoldersTeam, actorHitHealthChangeArgs.m_type, true, fromCharacterSpecificAbility);
				float pointsPerHealthChange2 = this.GetPointsPerHealthChange(this.m_objectivePointsData_otherTeam, actorHitHealthChangeArgs.m_type, true, fromCharacterSpecificAbility);
				int points = Mathf.RoundToInt(pointsPerHealthChange * (float)actorHitHealthChangeArgs.m_amount);
				int points2 = Mathf.RoundToInt(pointsPerHealthChange2 * (float)actorHitHealthChangeArgs.m_amount);
				this.AdjustObjectivePoints(points, team, clientMode);
				this.AdjustObjectivePoints(points2, opposingTeam, clientMode);
			}
		}
		List<CTF_Flag> flagsHeldByActor_Client2 = this.GetFlagsHeldByActor_Client(actorHitHealthChangeArgs.m_target);
		if (flagsHeldByActor_Client2 != null)
		{
			if (flagsHeldByActor_Client2.Count > 0)
			{
				Team team2 = actorHitHealthChangeArgs.m_target.GetTeam();
				Team opposingTeam2 = actorHitHealthChangeArgs.m_target.GetOpposingTeam();
				float pointsPerHealthChange3 = this.GetPointsPerHealthChange(this.m_objectivePointsData_flagHoldersTeam, actorHitHealthChangeArgs.m_type, false, fromCharacterSpecificAbility);
				float pointsPerHealthChange4 = this.GetPointsPerHealthChange(this.m_objectivePointsData_otherTeam, actorHitHealthChangeArgs.m_type, false, fromCharacterSpecificAbility);
				int points3 = Mathf.RoundToInt(pointsPerHealthChange3 * (float)actorHitHealthChangeArgs.m_amount);
				int points4 = Mathf.RoundToInt(pointsPerHealthChange4 * (float)actorHitHealthChangeArgs.m_amount);
				this.AdjustObjectivePoints(points3, team2, clientMode);
				this.AdjustObjectivePoints(points4, opposingTeam2, clientMode);
			}
		}
	}

	private float GetPointsPerHealthChange(CaptureTheFlag.FlagHolderObjectivePointData data, GameEventManager.ActorHitHealthChangeArgs.ChangeType healthChangeType, bool outgoing, bool fromCharacterSpecificAbility)
	{
		if (!fromCharacterSpecificAbility)
		{
			if (!data.m_includeContributionFromNonCharacterAbilities)
			{
				return 0f;
			}
		}
		if (healthChangeType == GameEventManager.ActorHitHealthChangeArgs.ChangeType.Damage)
		{
			if (outgoing)
			{
				return data.m_pointsPerDamageDealtByFlagHolder;
			}
			return data.m_pointsPerDamageTakenByFlagHolder;
		}
		else if (healthChangeType == GameEventManager.ActorHitHealthChangeArgs.ChangeType.Healing)
		{
			if (outgoing)
			{
				return data.m_pointsPerHealingDealtByFlagHolder;
			}
			return data.m_pointsPerHealingTakenByFlagHolder;
		}
		else
		{
			if (healthChangeType != GameEventManager.ActorHitHealthChangeArgs.ChangeType.Absorb)
			{
				return 0f;
			}
			if (outgoing)
			{
				return data.m_pointsPerAbsorbDealtByFlagHolder;
			}
			return data.m_pointsPerAbsorbTakenByFlagHolder;
		}
	}

	private void AdjustObjectivePoints(int points, Team team, bool clientMode)
	{
		if (points != 0)
		{
			if (clientMode)
			{
				ObjectivePoints.Get().AdjustUnresolvedPoints(points, team);
			}
			else
			{
				ObjectivePoints.Get().AdjustPoints(points, team);
			}
		}
	}

	public static bool AreCtfVictoryConditionsMetForTeam(CaptureTheFlag.CTF_VictoryCondition[] conditions, Team checkTeam)
	{
		if (CaptureTheFlag.Get() == null)
		{
			return true;
		}
		if (conditions != null)
		{
			if (conditions.Length != 0)
			{
				if (checkTeam != Team.TeamA && checkTeam != Team.TeamB)
				{
					return true;
				}
				bool flag = false;
				bool flag2 = false;
				bool flag3;
				bool flag4;
				if (checkTeam == Team.TeamA)
				{
					flag3 = (CaptureTheFlag.Get().m_teamACaptures > 0);
					flag4 = (CaptureTheFlag.Get().m_teamBCaptures > 0);
				}
				else
				{
					flag3 = (CaptureTheFlag.Get().m_teamBCaptures > 0);
					flag4 = (CaptureTheFlag.Get().m_teamACaptures > 0);
				}
				foreach (CTF_Flag ctf_Flag in CaptureTheFlag.Get().m_flags)
				{
					if (ctf_Flag.ServerHolderActor != null)
					{
						if (ctf_Flag.ServerHolderActor.GetTeam() == checkTeam)
						{
							flag = true;
						}
						else
						{
							flag2 = true;
						}
					}
				}
				bool result = true;
				foreach (CaptureTheFlag.CTF_VictoryCondition ctf_VictoryCondition in conditions)
				{
					if (ctf_VictoryCondition == CaptureTheFlag.CTF_VictoryCondition.TeamMustBeHoldingFlag)
					{
						if (!flag)
						{
							result = false;
							break;
						}
					}
					else if (ctf_VictoryCondition == CaptureTheFlag.CTF_VictoryCondition.TeamMustNotBeHoldingFlag)
					{
						if (flag)
						{
							result = false;
							break;
						}
					}
					else if (ctf_VictoryCondition == CaptureTheFlag.CTF_VictoryCondition.OtherTeamMustBeHoldingFlag)
					{
						if (!flag2)
						{
							result = false;
							break;
						}
					}
					else if (ctf_VictoryCondition == CaptureTheFlag.CTF_VictoryCondition.OtherTeamMustNotBeHoldingFlag)
					{
						if (flag2)
						{
							result = false;
							break;
						}
					}
					else if (ctf_VictoryCondition == CaptureTheFlag.CTF_VictoryCondition.TeamMustHaveCapturedFlag)
					{
						if (!flag3)
						{
							result = false;
							break;
						}
					}
					else if (ctf_VictoryCondition == CaptureTheFlag.CTF_VictoryCondition.TeamMustNotHaveCapturedFlag)
					{
						if (flag3)
						{
							result = false;
							break;
						}
					}
					else if (ctf_VictoryCondition == CaptureTheFlag.CTF_VictoryCondition.OtherTeamMustHaveCapturedFlag)
					{
						if (!flag4)
						{
							result = false;
							break;
						}
					}
					else if (ctf_VictoryCondition == CaptureTheFlag.CTF_VictoryCondition.OtherTeamMustNotHaveCapturedFlag)
					{
						if (flag4)
						{
							result = false;
							break;
						}
					}
				}
				return result;
			}
		}
		return true;
	}

	protected void HookSetTurninRegionState_TeamA(int turninRegionState_TeamA)
	{
		CaptureTheFlag.TurninRegionState turninRegionState_TeamA2 = (CaptureTheFlag.TurninRegionState)this.m_turninRegionState_TeamA;
		this.Networkm_turninRegionState_TeamA = turninRegionState_TeamA;
		if (this.FlagTurninRegion_TeamA != null)
		{
			if (this.FlagTurninRegion_TeamA.HasNonZeroArea())
			{
				this.Client_OnTurninStateChanged(Team.TeamA, turninRegionState_TeamA2, this.TurninRegionState_TeamA);
			}
		}
		this.OnTurninChanged_TeamA();
	}

	protected void HookSetTurninRegionState_TeamB(int turninRegionState_TeamB)
	{
		CaptureTheFlag.TurninRegionState turninRegionState_TeamB2 = (CaptureTheFlag.TurninRegionState)this.m_turninRegionState_TeamB;
		this.Networkm_turninRegionState_TeamB = turninRegionState_TeamB;
		if (this.FlagTurninRegion_TeamB != null)
		{
			if (this.FlagTurninRegion_TeamB.HasNonZeroArea())
			{
				this.Client_OnTurninStateChanged(Team.TeamB, turninRegionState_TeamB2, this.TurninRegionState_TeamB);
			}
		}
		this.OnTurninChanged_TeamB();
	}

	protected void HookSetTurninRegionState_Neutral(int turninRegionState_Neutral)
	{
		CaptureTheFlag.TurninRegionState turninRegionState_Neutral2 = (CaptureTheFlag.TurninRegionState)this.m_turninRegionState_Neutral;
		this.Networkm_turninRegionState_Neutral = turninRegionState_Neutral;
		if (this.FlagTurninRegion_Neutral != null)
		{
			if (this.FlagTurninRegion_Neutral.HasNonZeroArea())
			{
				this.Client_OnTurninStateChanged(Team.Invalid, turninRegionState_Neutral2, this.TurninRegionState_Neutral);
			}
		}
		this.OnTurninChanged_Neutral();
	}

	protected void HookSetTurninRegionIndex_TeamA(int turninRegionIndex_TeamA)
	{
		int turninRegionIndex_TeamA2 = this.m_turninRegionIndex_TeamA;
		this.Networkm_turninRegionIndex_TeamA = turninRegionIndex_TeamA;
		if (turninRegionIndex_TeamA2 == -1 && this.FlagTurninRegion_TeamA != null)
		{
			this.GenerateFlagTurninVisuals();
			if (this.TurninRegionState_TeamA != CaptureTheFlag.TurninRegionState.Disabled)
			{
				this.Client_OnTurninStateChanged(Team.TeamA, CaptureTheFlag.TurninRegionState.Disabled, this.TurninRegionState_TeamA);
			}
		}
		this.OnTurninChanged_TeamA();
	}

	protected void HookSetTurninRegionIndex_TeamB(int turninRegionIndex_TeamB)
	{
		int turninRegionIndex_TeamB2 = this.m_turninRegionIndex_TeamB;
		this.Networkm_turninRegionIndex_TeamB = turninRegionIndex_TeamB;
		if (turninRegionIndex_TeamB2 == -1)
		{
			if (this.FlagTurninRegion_TeamB != null)
			{
				this.GenerateFlagTurninVisuals();
				if (this.TurninRegionState_TeamB != CaptureTheFlag.TurninRegionState.Disabled)
				{
					this.Client_OnTurninStateChanged(Team.TeamB, CaptureTheFlag.TurninRegionState.Disabled, this.TurninRegionState_TeamB);
				}
			}
		}
		this.OnTurninChanged_TeamB();
	}

	protected void HookSetTurninRegionIndex_Neutral(int turninRegionIndex_Neutral)
	{
		int turninRegionIndex_Neutral2 = this.m_turninRegionIndex_Neutral;
		this.Networkm_turninRegionIndex_Neutral = turninRegionIndex_Neutral;
		if (turninRegionIndex_Neutral2 == -1)
		{
			if (this.FlagTurninRegion_Neutral != null)
			{
				this.GenerateFlagTurninVisuals();
				if (this.TurninRegionState_Neutral != CaptureTheFlag.TurninRegionState.Disabled)
				{
					this.Client_OnTurninStateChanged(Team.Invalid, CaptureTheFlag.TurninRegionState.Disabled, this.TurninRegionState_Neutral);
				}
			}
		}
		this.OnTurninChanged_Neutral();
	}

	protected void HookSetNumFlagDrops(int numFlagDrops)
	{
		this.Networkm_numFlagDrops = numFlagDrops;
		this.m_clientUnresolvedNumFlagDrops = 0;
	}

	private void OnTurninChanged_TeamA()
	{
		bool flag = this.TurninRegionState_TeamA == CaptureTheFlag.TurninRegionState.Active;
		bool flag2 = this.m_turninRegionIndex_TeamA >= 0;
		bool flag3 = this.m_timeTillCameraFocusesOntoExtractionPoint >= 0f;
		bool flag4 = this.m_timeToFocusCameraOnTurninTeamA >= 0f;
		if (flag)
		{
			if (flag2)
			{
				if (flag3)
				{
					if (!flag4)
					{
						this.m_timeToFocusCameraOnTurninTeamA = Time.time + this.m_timeTillCameraFocusesOntoExtractionPoint;
					}
				}
			}
		}
		if (this.FlagTurninRegion_TeamA != null)
		{
			if (HUD_UI.Get() != null)
			{
				bool flag5 = true;
				if (GameFlowData.Get().activeOwnedActorData != null)
				{
					flag5 = (GameFlowData.Get().activeOwnedActorData.GetTeam() == Team.TeamA);
				}
				if (flag)
				{
					if (flag2)
					{
						UIOffscreenIndicatorPanel offscreenIndicatorPanel = HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel;
						BoardRegion flagTurninRegion_TeamA = this.FlagTurninRegion_TeamA;
						Team teamRegion;
						if (flag5)
						{
							teamRegion = Team.TeamA;
						}
						else
						{
							teamRegion = Team.TeamB;
						}
						offscreenIndicatorPanel.AddCtfFlagTurnInRegion(flagTurninRegion_TeamA, teamRegion);
						return;
					}
				}
				HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.RemoveCtfFlagTurnInRegion(this.FlagTurninRegion_TeamA);
			}
		}
	}

	private void OnTurninChanged_TeamB()
	{
		bool flag = this.TurninRegionState_TeamB == CaptureTheFlag.TurninRegionState.Active;
		bool flag2 = this.m_turninRegionIndex_TeamB >= 0;
		bool flag3 = this.m_timeTillCameraFocusesOntoExtractionPoint >= 0f;
		bool flag4 = this.m_timeToFocusCameraOnTurninTeamB >= 0f;
		if (flag)
		{
			if (flag2 && flag3)
			{
				if (!flag4)
				{
					this.m_timeToFocusCameraOnTurninTeamB = Time.time + this.m_timeTillCameraFocusesOntoExtractionPoint;
				}
			}
		}
		if (this.FlagTurninRegion_TeamB != null)
		{
			if (HUD_UI.Get() != null)
			{
				bool flag5 = false;
				if (GameFlowData.Get().activeOwnedActorData != null)
				{
					flag5 = (GameFlowData.Get().activeOwnedActorData.GetTeam() == Team.TeamB);
				}
				if (flag)
				{
					if (flag2)
					{
						HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddCtfFlagTurnInRegion(this.FlagTurninRegion_TeamB, (!flag5) ? Team.TeamB : Team.TeamA);
						return;
					}
				}
				HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.RemoveCtfFlagTurnInRegion(this.FlagTurninRegion_TeamB);
			}
		}
	}

	private void OnTurninChanged_Neutral()
	{
		bool flag = this.TurninRegionState_Neutral == CaptureTheFlag.TurninRegionState.Active;
		bool flag2 = this.m_turninRegionIndex_Neutral >= 0;
		bool flag3 = this.m_timeTillCameraFocusesOntoExtractionPoint >= 0f;
		bool flag4 = this.m_timeToFocusCameraOnTurninNeutral >= 0f;
		if (flag)
		{
			if (flag2 && flag3)
			{
				if (!flag4)
				{
					this.m_timeToFocusCameraOnTurninNeutral = Time.time + this.m_timeTillCameraFocusesOntoExtractionPoint;
				}
			}
		}
		if (this.FlagTurninRegion_Neutral != null)
		{
			if (HUD_UI.Get() != null)
			{
				if (flag)
				{
					if (flag2)
					{
						HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddCtfFlagTurnInRegion(this.FlagTurninRegion_Neutral, Team.Invalid);
						return;
					}
				}
				HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.RemoveCtfFlagTurnInRegion(this.FlagTurninRegion_Neutral);
			}
		}
	}

	private GameObject InstantiateBoundaryObject(BoardRegion region, string boundaryName)
	{
		if (region != null)
		{
			if (region.GetSquaresInRegion().Count > 0)
			{
				GameObject gameObject = HighlightUtils.Get().CreateBoundaryHighlight(region.GetSquaresInRegion(), Color.yellow, false);
				gameObject.name = base.name + " " + boundaryName;
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				this.m_autoBoundaryHeight = gameObject.transform.position.y;
				return gameObject;
			}
		}
		return null;
	}

	private void GenerateBoundaryVisuals()
	{
		if (this.m_autoGenerateSpawnLocVisuals)
		{
			this.GenerateFlagSpawnBoundaryVisuals();
		}
		if (this.m_autoGenerateTurninVisuals)
		{
			this.GenerateFlagTurninVisuals();
		}
	}

	private void GenerateFlagSpawnBoundaryVisuals()
	{
		this.m_autoBoundary_spawn_neutral = this.InstantiateBoundaryObject(this.m_flagSpawnsNeutral, "Neutral Flag-Spawn Auto-Boundary");
		this.m_autoBoundary_spawn_teamA = this.InstantiateBoundaryObject(this.m_flagSpawnsTeamA, "TeamA Flag-Spawn Auto-Boundary");
		this.m_autoBoundary_spawn_teamB = this.InstantiateBoundaryObject(this.m_flagSpawnsTeamB, "TeamB Flag-Spawn Auto-Boundary");
	}

	private void GenerateFlagTurninVisuals()
	{
		if (this.m_autoBoundary_turnin_neutral == null)
		{
			if (this.FlagTurninRegion_Neutral != null)
			{
				this.m_autoBoundary_turnin_neutral = this.InstantiateBoundaryObject(this.FlagTurninRegion_Neutral, "Neutral Turnin Auto-Boundary");
			}
		}
		if (this.m_autoBoundary_turnin_teamA == null)
		{
			if (this.FlagTurninRegion_TeamA != null)
			{
				this.m_autoBoundary_turnin_teamA = this.InstantiateBoundaryObject(this.FlagTurninRegion_TeamA, "TeamA Turnin Auto-Boundary");
			}
		}
		if (this.m_autoBoundary_turnin_teamB == null)
		{
			if (this.FlagTurninRegion_TeamB != null)
			{
				this.m_autoBoundary_turnin_teamB = this.InstantiateBoundaryObject(this.FlagTurninRegion_TeamB, "TeamB Turnin Auto-Boundary");
			}
		}
	}

	private void SetBoundaryColor(GameObject autoBoundary, Color mainColor, Color secondaryColor, float oscillationLevel)
	{
		if (autoBoundary != null)
		{
			float num = 1f - oscillationLevel * oscillationLevel;
			float num2 = oscillationLevel * oscillationLevel;
			Color color = new Color(mainColor.r * num + secondaryColor.r * num2, mainColor.g * num + secondaryColor.g * num2, mainColor.b * num + secondaryColor.b * num2, mainColor.a * num + secondaryColor.a * num2);
			this.SetBoundaryColor(autoBoundary, color);
		}
	}

	private void SetBoundaryColor(GameObject autoBoundary, Color color)
	{
		if (autoBoundary != null)
		{
			autoBoundary.GetComponent<Renderer>().material.SetColor("_TintColor", color);
		}
	}

	private Color DetermineSecondaryColor(bool condition1, Color color1, bool condition2, Color color2, Color fallbackColor)
	{
		if (condition1)
		{
			return color1;
		}
		if (condition2)
		{
			return color2;
		}
		return fallbackColor;
	}

	private void AdjustPositionOfObjToOscillation(GameObject obj, float oscillationLevel)
	{
		if (obj != null)
		{
			float num = oscillationLevel * this.m_boundaryOscillationHeight;
			obj.transform.position = new Vector3(obj.transform.position.x, this.m_autoBoundaryHeight + num, obj.transform.position.z);
		}
	}

	private void Update()
	{
		float oscillationLevel = (1f - Mathf.Cos(Time.time * this.m_boundaryOscillationSpeed)) / 2f;
		Team team;
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().LocalPlayerData != null)
			{
				team = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
				goto IL_80;
			}
		}
		team = Team.Invalid;
		IL_80:
		Color color;
		Color color2;
		if (team == Team.TeamA)
		{
			color = this.m_primaryColor_friendly;
			color2 = this.m_primaryColor_hostile;
		}
		else if (team == Team.TeamB)
		{
			color = this.m_primaryColor_hostile;
			color2 = this.m_primaryColor_friendly;
		}
		else
		{
			color = this.m_primaryColor_neutral;
			color2 = this.m_primaryColor_neutral;
		}
		this.AdjustPositionOfObjToOscillation(this.m_autoBoundary_spawn_neutral, oscillationLevel);
		this.SetBoundaryColor(this.m_autoBoundary_spawn_neutral, this.m_primaryColor_neutral);
		this.AdjustPositionOfObjToOscillation(this.m_autoBoundary_spawn_teamA, oscillationLevel);
		this.SetBoundaryColor(this.m_autoBoundary_spawn_teamA, color);
		this.AdjustPositionOfObjToOscillation(this.m_autoBoundary_spawn_teamB, oscillationLevel);
		this.SetBoundaryColor(this.m_autoBoundary_spawn_teamB, color2);
		if (this.TurninRegionState_Neutral != CaptureTheFlag.TurninRegionState.Disabled)
		{
			this.AdjustPositionOfObjToOscillation(this.m_autoBoundary_turnin_neutral, oscillationLevel);
			bool flag = this.TurninRegionState_Neutral == CaptureTheFlag.TurninRegionState.Locked;
			Color secondaryColor = (!flag) ? this.m_primaryColor_neutral : this.m_secondaryColor_locked;
			this.SetBoundaryColor(this.m_autoBoundary_turnin_neutral, this.m_primaryColor_neutral, secondaryColor, oscillationLevel);
		}
		else
		{
			this.AdjustPositionOfObjToOscillation(this.m_autoBoundary_turnin_neutral, 0f);
			Color color3 = new Color(this.m_primaryColor_neutral.r * 0.5f, this.m_primaryColor_neutral.g * 0.5f, this.m_primaryColor_neutral.b * 0.5f, this.m_primaryColor_neutral.a * 0.5f);
			this.SetBoundaryColor(this.m_autoBoundary_turnin_neutral, color3);
		}
		if (this.TurninRegionState_TeamA != CaptureTheFlag.TurninRegionState.Disabled)
		{
			this.AdjustPositionOfObjToOscillation(this.m_autoBoundary_turnin_teamA, oscillationLevel);
			bool flag2 = this.TurninRegionState_TeamA == CaptureTheFlag.TurninRegionState.Locked;
			Color color4;
			if (flag2)
			{
				color4 = this.m_secondaryColor_locked;
			}
			else
			{
				color4 = color;
			}
			Color secondaryColor2 = color4;
			this.SetBoundaryColor(this.m_autoBoundary_turnin_teamA, color, secondaryColor2, oscillationLevel);
		}
		else
		{
			this.AdjustPositionOfObjToOscillation(this.m_autoBoundary_turnin_teamA, 0f);
			Color color5 = new Color(color.r * 0.5f, color.g * 0.5f, color.b * 0.5f, color.a * 0.5f);
			this.SetBoundaryColor(this.m_autoBoundary_turnin_teamA, color5);
		}
		if (this.TurninRegionState_TeamB != CaptureTheFlag.TurninRegionState.Disabled)
		{
			this.AdjustPositionOfObjToOscillation(this.m_autoBoundary_turnin_teamB, oscillationLevel);
			bool flag3 = this.TurninRegionState_TeamB == CaptureTheFlag.TurninRegionState.Locked;
			Color secondaryColor3 = (!flag3) ? color2 : this.m_secondaryColor_locked;
			this.SetBoundaryColor(this.m_autoBoundary_turnin_teamB, color2, secondaryColor3, oscillationLevel);
		}
		else
		{
			this.AdjustPositionOfObjToOscillation(this.m_autoBoundary_turnin_teamB, 0f);
			Color color6 = new Color(color2.r * 0.5f, color2.g * 0.5f, color2.b * 0.5f, color2.a * 0.5f);
			this.SetBoundaryColor(this.m_autoBoundary_turnin_teamB, color6);
		}
		float num;
		float num2;
		CaptureTheFlag.GetFlagCarrierDamageTillDropProgressForUI(out num, out num2);
		UI_CTF_BriefcasePanel ui_CTF_BriefcasePanel = UI_CTF_BriefcasePanel.Get();
		if (ui_CTF_BriefcasePanel != null)
		{
			if (!ui_CTF_BriefcasePanel.m_initialized)
			{
				ui_CTF_BriefcasePanel.Setup(this);
			}
			if (num == this.m_lastFlagCarrierDamageCur)
			{
				if (num2 == this.m_lastFlagCarrierDamageMax)
				{
					goto IL_3AF;
				}
			}
			if (ui_CTF_BriefcasePanel.UpdateDamageForFlagHolder(num, num2))
			{
				this.m_lastFlagCarrierDamageCur = num;
				this.m_lastFlagCarrierDamageMax = num2;
			}
		}
		IL_3AF:
		if (this.m_timeToFocusCameraOnTurninTeamA > 0f)
		{
			if (this.m_timeToFocusCameraOnTurninTeamA <= Time.time)
			{
				if (CameraManager.Get() != null)
				{
					if (this.FlagTurninRegion_TeamA != null)
					{
						if (this.FlagTurninRegion_TeamA.GetCenterSquare() != null)
						{
							CameraManager.Get().SetTargetObject(this.FlagTurninRegion_TeamA.GetCenterSquare().gameObject, CameraManager.CameraTargetReason.CtfTurninRegionSpawned);
						}
					}
				}
				this.m_timeToFocusCameraOnTurninTeamA = -1f;
				this.CreateTurninRegionActivatedSequence(Team.TeamA, this.FlagTurninRegion_TeamA.GetCenter());
			}
		}
		if (this.m_timeToFocusCameraOnTurninTeamB > 0f)
		{
			if (this.m_timeToFocusCameraOnTurninTeamB <= Time.time)
			{
				if (CameraManager.Get() != null)
				{
					if (this.FlagTurninRegion_TeamB != null)
					{
						if (this.FlagTurninRegion_TeamB.GetCenterSquare() != null)
						{
							CameraManager.Get().SetTargetObject(this.FlagTurninRegion_TeamB.GetCenterSquare().gameObject, CameraManager.CameraTargetReason.CtfTurninRegionSpawned);
						}
					}
				}
				this.m_timeToFocusCameraOnTurninTeamB = -1f;
				this.CreateTurninRegionActivatedSequence(Team.TeamB, this.FlagTurninRegion_TeamB.GetCenter());
			}
		}
		if (this.m_timeToFocusCameraOnTurninNeutral > 0f)
		{
			if (this.m_timeToFocusCameraOnTurninNeutral <= Time.time)
			{
				if (CameraManager.Get() != null)
				{
					if (this.FlagTurninRegion_Neutral != null && this.FlagTurninRegion_Neutral.GetCenterSquare() != null)
					{
						CameraManager.Get().SetTargetObject(this.FlagTurninRegion_Neutral.GetCenterSquare().gameObject, CameraManager.CameraTargetReason.CtfTurninRegionSpawned);
					}
				}
				this.m_timeToFocusCameraOnTurninNeutral = -1f;
				this.CreateTurninRegionActivatedSequence(Team.Invalid, this.FlagTurninRegion_Neutral.GetCenter());
			}
		}
		if (this.m_timeToFocusCameraOnExtraction > 0f)
		{
			if (this.m_timeToFocusCameraOnExtraction <= Time.time)
			{
				if (CameraManager.Get() != null && this.m_lastExtractionSquare != null)
				{
					CameraManager.Get().SetTargetObject(this.m_lastExtractionSquare.gameObject, CameraManager.CameraTargetReason.CtfFlagTurnedIn);
				}
				this.m_timeToFocusCameraOnExtraction = -1f;
				this.m_timeToFocusCameraOnTurninTeamA = -1f;
				this.m_timeToFocusCameraOnTurninTeamB = -1f;
				this.m_timeToFocusCameraOnTurninNeutral = -1f;
				this.m_lastExtractionSquare = null;
			}
		}
	}

	public void CreateTurninRegionActivatedSequence(Team teamOfTurninRegionActivating, Vector3 centerPos)
	{
		Team team;
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().LocalPlayerData != null)
			{
				team = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
				goto IL_50;
			}
		}
		team = Team.Invalid;
		IL_50:
		GameObject gameObject;
		if (teamOfTurninRegionActivating != Team.TeamA)
		{
			if (teamOfTurninRegionActivating != Team.TeamB)
			{
				gameObject = this.m_neutralTurninRegionActivatedSequence;
				goto IL_A3;
			}
		}
		if (team != teamOfTurninRegionActivating)
		{
			if (teamOfTurninRegionActivating != Team.TeamA || team == Team.TeamB)
			{
				gameObject = this.m_enemyTurninRegionActivatedSequence;
				goto IL_9A;
			}
		}
		gameObject = this.m_friendlyTurninRegionActivatedSequence;
		IL_9A:
		IL_A3:
		if (gameObject != null)
		{
			SequenceManager.Get().CreateClientSequences(gameObject, centerPos, new ActorData[0], null, this.SequenceSource, null);
		}
	}

	public unsafe static float GetFlagCarrierDamageTillDropProgressForUI(out float cur, out float max)
	{
		CTF_Flag mainFlag = CaptureTheFlag.GetMainFlag();
		cur = -1f;
		max = -1f;
		int num = (!(CaptureTheFlag.s_instance != null)) ? 0 : (CaptureTheFlag.s_instance.m_numFlagDrops + CaptureTheFlag.s_instance.m_clientUnresolvedNumFlagDrops);
		if (CaptureTheFlag.s_instance == null)
		{
			max = 1f;
			cur = 1f;
		}
		else if (CaptureTheFlag.s_instance.m_damageInOneTurnToDropFlag_gross > 0)
		{
			max = (float)(CaptureTheFlag.s_instance.m_damageInOneTurnToDropFlag_gross + CaptureTheFlag.s_instance.m_damageThesholdIncreaseOnDrop * num);
			if (mainFlag != null)
			{
				cur = (float)(mainFlag.DamageOnHolderSinceTurnStart_Gross + mainFlag.ClientUnresolvedDamageOnHolder);
			}
			else
			{
				cur = max;
			}
		}
		else if (CaptureTheFlag.s_instance.m_damageSincePickedUpToDropFlag_gross > 0)
		{
			max = (float)(CaptureTheFlag.s_instance.m_damageSincePickedUpToDropFlag_gross + CaptureTheFlag.s_instance.m_damageThesholdIncreaseOnDrop * num);
			if (mainFlag != null)
			{
				cur = (float)(mainFlag.DamageOnHolderSincePickedUp_Gross + mainFlag.ClientUnresolvedDamageOnHolder);
			}
			else
			{
				cur = max;
			}
		}
		else
		{
			cur = -1f;
			max = -1f;
		}
		return Mathf.Clamp(cur / max, 0f, 1f);
	}

	public static void OnActorDamaged_Client(ActorData actor, int damage)
	{
		CTF_Flag mainFlag = CaptureTheFlag.GetMainFlag();
		ActorData mainFlagCarrier_Client = CaptureTheFlag.GetMainFlagCarrier_Client();
		if (CaptureTheFlag.s_instance != null)
		{
			if (mainFlag != null && mainFlagCarrier_Client != null)
			{
				if (actor != null)
				{
					if (mainFlagCarrier_Client == actor)
					{
						mainFlag.ClientUnresolvedDamageOnHolder += damage;
					}
				}
			}
		}
	}

	private void UNetVersion()
	{
	}

	public int Networkm_turninRegionState_TeamA
	{
		get
		{
			return this.m_turninRegionState_TeamA;
		}
		[param: In]
		set
		{
			uint dirtyBit = 1U;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					this.HookSetTurninRegionState_TeamA(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<int>(value, ref this.m_turninRegionState_TeamA, dirtyBit);
		}
	}

	public int Networkm_turninRegionState_TeamB
	{
		get
		{
			return this.m_turninRegionState_TeamB;
		}
		[param: In]
		set
		{
			uint dirtyBit = 2U;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					this.HookSetTurninRegionState_TeamB(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<int>(value, ref this.m_turninRegionState_TeamB, dirtyBit);
		}
	}

	public int Networkm_turninRegionState_Neutral
	{
		get
		{
			return this.m_turninRegionState_Neutral;
		}
		[param: In]
		set
		{
			uint dirtyBit = 4U;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					this.HookSetTurninRegionState_Neutral(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<int>(value, ref this.m_turninRegionState_Neutral, dirtyBit);
		}
	}

	public int Networkm_turninRegionIndex_TeamA
	{
		get
		{
			return this.m_turninRegionIndex_TeamA;
		}
		[param: In]
		set
		{
			uint dirtyBit = 8U;
			if (NetworkServer.localClientActive && !base.syncVarHookGuard)
			{
				base.syncVarHookGuard = true;
				this.HookSetTurninRegionIndex_TeamA(value);
				base.syncVarHookGuard = false;
			}
			base.SetSyncVar<int>(value, ref this.m_turninRegionIndex_TeamA, dirtyBit);
		}
	}

	public int Networkm_turninRegionIndex_TeamB
	{
		get
		{
			return this.m_turninRegionIndex_TeamB;
		}
		[param: In]
		set
		{
			uint dirtyBit = 0x10U;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					this.HookSetTurninRegionIndex_TeamB(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<int>(value, ref this.m_turninRegionIndex_TeamB, dirtyBit);
		}
	}

	public int Networkm_turninRegionIndex_Neutral
	{
		get
		{
			return this.m_turninRegionIndex_Neutral;
		}
		[param: In]
		set
		{
			uint dirtyBit = 0x20U;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					this.HookSetTurninRegionIndex_Neutral(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<int>(value, ref this.m_turninRegionIndex_Neutral, dirtyBit);
		}
	}

	public int Networkm_numFlagDrops
	{
		get
		{
			return this.m_numFlagDrops;
		}
		[param: In]
		set
		{
			uint dirtyBit = 0x40U;
			if (NetworkServer.localClientActive && !base.syncVarHookGuard)
			{
				base.syncVarHookGuard = true;
				this.HookSetNumFlagDrops(value);
				base.syncVarHookGuard = false;
			}
			base.SetSyncVar<int>(value, ref this.m_numFlagDrops, dirtyBit);
		}
	}

	public uint Networkm_sequenceSourceId
	{
		get
		{
			return this.m_sequenceSourceId;
		}
		[param: In]
		set
		{
			base.SetSyncVar<uint>(value, ref this.m_sequenceSourceId, 0x80U);
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)this.m_turninRegionState_TeamA);
			writer.WritePackedUInt32((uint)this.m_turninRegionState_TeamB);
			writer.WritePackedUInt32((uint)this.m_turninRegionState_Neutral);
			writer.WritePackedUInt32((uint)this.m_turninRegionIndex_TeamA);
			writer.WritePackedUInt32((uint)this.m_turninRegionIndex_TeamB);
			writer.WritePackedUInt32((uint)this.m_turninRegionIndex_Neutral);
			writer.WritePackedUInt32((uint)this.m_numFlagDrops);
			writer.WritePackedUInt32(this.m_sequenceSourceId);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_turninRegionState_TeamA);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_turninRegionState_TeamB);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_turninRegionState_Neutral);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_turninRegionIndex_TeamA);
		}
		if ((base.syncVarDirtyBits & 0x10U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_turninRegionIndex_TeamB);
		}
		if ((base.syncVarDirtyBits & 0x20U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_turninRegionIndex_Neutral);
		}
		if ((base.syncVarDirtyBits & 0x40U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_numFlagDrops);
		}
		if ((base.syncVarDirtyBits & 0x80U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32(this.m_sequenceSourceId);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			this.m_turninRegionState_TeamA = (int)reader.ReadPackedUInt32();
			this.m_turninRegionState_TeamB = (int)reader.ReadPackedUInt32();
			this.m_turninRegionState_Neutral = (int)reader.ReadPackedUInt32();
			this.m_turninRegionIndex_TeamA = (int)reader.ReadPackedUInt32();
			this.m_turninRegionIndex_TeamB = (int)reader.ReadPackedUInt32();
			this.m_turninRegionIndex_Neutral = (int)reader.ReadPackedUInt32();
			this.m_numFlagDrops = (int)reader.ReadPackedUInt32();
			this.m_sequenceSourceId = reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			this.HookSetTurninRegionState_TeamA((int)reader.ReadPackedUInt32());
		}
		if ((num & 2) != 0)
		{
			this.HookSetTurninRegionState_TeamB((int)reader.ReadPackedUInt32());
		}
		if ((num & 4) != 0)
		{
			this.HookSetTurninRegionState_Neutral((int)reader.ReadPackedUInt32());
		}
		if ((num & 8) != 0)
		{
			this.HookSetTurninRegionIndex_TeamA((int)reader.ReadPackedUInt32());
		}
		if ((num & 0x10) != 0)
		{
			this.HookSetTurninRegionIndex_TeamB((int)reader.ReadPackedUInt32());
		}
		if ((num & 0x20) != 0)
		{
			this.HookSetTurninRegionIndex_Neutral((int)reader.ReadPackedUInt32());
		}
		if ((num & 0x40) != 0)
		{
			this.HookSetNumFlagDrops((int)reader.ReadPackedUInt32());
		}
		if ((num & 0x80) != 0)
		{
			this.m_sequenceSourceId = reader.ReadPackedUInt32();
		}
	}

	public enum CTF_VictoryCondition
	{
		TeamMustBeHoldingFlag,
		TeamMustNotBeHoldingFlag,
		OtherTeamMustBeHoldingFlag,
		OtherTeamMustNotBeHoldingFlag,
		TeamMustHaveCapturedFlag,
		TeamMustNotHaveCapturedFlag,
		OtherTeamMustHaveCapturedFlag,
		OtherTeamMustNotHaveCapturedFlag
	}

	public enum TurninRegionState
	{
		Active,
		Locked,
		Disabled
	}

	public enum TurninType
	{
		FlagHolderMovingIntoCaptureRegion,
		FlagHolderEndingTurnInCaptureRegion,
		FlagHolderSpendingWholeTurnInCaptureRegion,
		CaptureRegionActivatingUnderFlagHolder
	}

	public enum RelationshipToClient
	{
		Neutral,
		Friendly,
		Hostile
	}

	[Serializable]
	public class FlagSpawnData
	{
		public int m_maxActiveSimultaneously;

		public int m_totalMaxSpawns = -1;

		public int m_minTurnsTillFirstSpawn;

		public int m_minTurnsAfterCaptureTillRespawn;

		public int NumFlagsSpawned { get; set; }

		public int LastCaptureTurn { get; set; }
	}

	[Serializable]
	public class FlagHolderObjectivePointData
	{
		public float m_pointsPerDamageDealtByFlagHolder;

		public float m_pointsPerDamageTakenByFlagHolder;

		public float m_pointsPerHealingDealtByFlagHolder;

		public float m_pointsPerHealingTakenByFlagHolder;

		public float m_pointsPerAbsorbDealtByFlagHolder;

		public float m_pointsPerAbsorbTakenByFlagHolder;

		public bool m_includeContributionFromNonCharacterAbilities;

		public int m_pointsPerDeathblowByFlagHolder;

		public int m_pointsPerTakedownByFlagHolder;

		public int m_pointsPerDeathOfFlagHolder;

		public int m_pointsPerTurn;
	}
}
