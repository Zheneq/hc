using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class CaptureTheFlag : NetworkBehaviour, IGameEventListener
{
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

		public int NumFlagsSpawned
		{
			get;
			set;
		}

		public int LastCaptureTurn
		{
			get;
			set;
		}
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

	public TurninRegionState m_turninRegionInitialState;

	public List<TurninType> m_turnInRequirements;

	[Header("Spawning Logic")]
	public FlagSpawnData m_neutralFlagSpawningLogic;

	public FlagSpawnData m_teamAFlagSpawningLogic;

	public FlagSpawnData m_teamBFlagSpawningLogic;

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
	public FlagHolderObjectivePointData m_objectivePointsData_flagHoldersTeam;

	public FlagHolderObjectivePointData m_objectivePointsData_otherTeam;

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

	internal SequenceSource SequenceSource
	{
		get
		{
			if (_sequenceSource == null)
			{
				_sequenceSource = new SequenceSource(null, null, m_sequenceSourceId, false);
			}
			return _sequenceSource;
		}
	}

	private TurninRegionState TurninRegionState_TeamA
	{
		get
		{
			return (TurninRegionState)m_turninRegionState_TeamA;
		}
		set
		{
			if (m_turninRegionState_TeamA != (int)value)
			{
				Networkm_turninRegionState_TeamA = (int)value;
			}
		}
	}

	private TurninRegionState TurninRegionState_TeamB
	{
		get
		{
			return (TurninRegionState)m_turninRegionState_TeamB;
		}
		set
		{
			if (m_turninRegionState_TeamB != (int)value)
			{
				Networkm_turninRegionState_TeamB = (int)value;
			}
		}
	}

	private TurninRegionState TurninRegionState_Neutral
	{
		get
		{
			return (TurninRegionState)m_turninRegionState_Neutral;
		}
		set
		{
			if (m_turninRegionState_Neutral == (int)value)
			{
				return;
			}
			while (true)
			{
				Networkm_turninRegionState_Neutral = (int)value;
				return;
			}
		}
	}

	private BoardRegion FlagTurninRegion_TeamA
	{
		get
		{
			if (TurninRegionState_TeamA == TurninRegionState.Active)
			{
				if (m_potentialFlagTurnins != null)
				{
					if (m_potentialFlagTurnins.Count != 0)
					{
						if (m_potentialTurninsAreTeamSpecific)
						{
							if (m_turninRegionIndex_TeamA == -1)
							{
								return null;
							}
							return m_potentialFlagTurnins[m_turninRegionIndex_TeamA];
						}
					}
				}
				return m_flagTurninTeamA;
			}
			return null;
		}
	}

	private BoardRegion FlagTurninRegion_TeamB
	{
		get
		{
			if (TurninRegionState_TeamB == TurninRegionState.Active)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						if (m_potentialFlagTurnins != null)
						{
							if (m_potentialFlagTurnins.Count != 0)
							{
								if (m_potentialTurninsAreTeamSpecific)
								{
									if (m_turninRegionIndex_TeamB == -1)
									{
										while (true)
										{
											switch (1)
											{
											case 0:
												break;
											default:
												return null;
											}
										}
									}
									return m_potentialFlagTurnins[m_turninRegionIndex_TeamB];
								}
							}
						}
						return m_flagTurninTeamB;
					}
				}
			}
			return null;
		}
	}

	private BoardRegion FlagTurninRegion_Neutral
	{
		get
		{
			if (TurninRegionState_Neutral == TurninRegionState.Active)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						if (m_potentialFlagTurnins != null)
						{
							if (m_potentialFlagTurnins.Count != 0)
							{
								if (!m_potentialTurninsAreTeamSpecific)
								{
									if (m_turninRegionIndex_Neutral == -1)
									{
										while (true)
										{
											switch (3)
											{
											case 0:
												break;
											default:
												return null;
											}
										}
									}
									return m_potentialFlagTurnins[m_turninRegionIndex_Neutral];
								}
							}
						}
						return m_flagTurninNeutral;
					}
				}
			}
			return null;
		}
	}

	public int Networkm_turninRegionState_TeamA
	{
		get
		{
			return m_turninRegionState_TeamA;
		}
		[param: In]
		set
		{
			ref int turninRegionState_TeamA = ref m_turninRegionState_TeamA;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					HookSetTurninRegionState_TeamA(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref turninRegionState_TeamA, 1u);
		}
	}

	public int Networkm_turninRegionState_TeamB
	{
		get
		{
			return m_turninRegionState_TeamB;
		}
		[param: In]
		set
		{
			ref int turninRegionState_TeamB = ref m_turninRegionState_TeamB;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					HookSetTurninRegionState_TeamB(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref turninRegionState_TeamB, 2u);
		}
	}

	public int Networkm_turninRegionState_Neutral
	{
		get
		{
			return m_turninRegionState_Neutral;
		}
		[param: In]
		set
		{
			ref int turninRegionState_Neutral = ref m_turninRegionState_Neutral;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					HookSetTurninRegionState_Neutral(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref turninRegionState_Neutral, 4u);
		}
	}

	public int Networkm_turninRegionIndex_TeamA
	{
		get
		{
			return m_turninRegionIndex_TeamA;
		}
		[param: In]
		set
		{
			ref int turninRegionIndex_TeamA = ref m_turninRegionIndex_TeamA;
			if (NetworkServer.localClientActive && !base.syncVarHookGuard)
			{
				base.syncVarHookGuard = true;
				HookSetTurninRegionIndex_TeamA(value);
				base.syncVarHookGuard = false;
			}
			SetSyncVar(value, ref turninRegionIndex_TeamA, 8u);
		}
	}

	public int Networkm_turninRegionIndex_TeamB
	{
		get
		{
			return m_turninRegionIndex_TeamB;
		}
		[param: In]
		set
		{
			ref int turninRegionIndex_TeamB = ref m_turninRegionIndex_TeamB;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					HookSetTurninRegionIndex_TeamB(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref turninRegionIndex_TeamB, 16u);
		}
	}

	public int Networkm_turninRegionIndex_Neutral
	{
		get
		{
			return m_turninRegionIndex_Neutral;
		}
		[param: In]
		set
		{
			ref int turninRegionIndex_Neutral = ref m_turninRegionIndex_Neutral;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					HookSetTurninRegionIndex_Neutral(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref turninRegionIndex_Neutral, 32u);
		}
	}

	public int Networkm_numFlagDrops
	{
		get
		{
			return m_numFlagDrops;
		}
		[param: In]
		set
		{
			ref int numFlagDrops = ref m_numFlagDrops;
			if (NetworkServer.localClientActive && !base.syncVarHookGuard)
			{
				base.syncVarHookGuard = true;
				HookSetNumFlagDrops(value);
				base.syncVarHookGuard = false;
			}
			SetSyncVar(value, ref numFlagDrops, 64u);
		}
	}

	public uint Networkm_sequenceSourceId
	{
		get
		{
			return m_sequenceSourceId;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_sequenceSourceId, 128u);
		}
	}

	private void Awake()
	{
		if (s_instance == null)
		{
			s_instance = this;
		}
		else
		{
			Log.Error("Multiple CaptureTheFlag components in this scene, remove extraneous ones.");
		}
		if (!NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			SequenceSource sequenceSource = new SequenceSource(null, null, false);
			Networkm_sequenceSourceId = sequenceSource.RootID;
			return;
		}
	}

	private void OnDestroy()
	{
		if (m_autoBoundary_spawn_neutral != null)
		{
			HighlightUtils.DestroyBoundaryHighlightObject(m_autoBoundary_spawn_neutral);
			m_autoBoundary_spawn_neutral = null;
		}
		if (m_autoBoundary_spawn_teamA != null)
		{
			HighlightUtils.DestroyBoundaryHighlightObject(m_autoBoundary_spawn_teamA);
			m_autoBoundary_spawn_teamA = null;
		}
		if (m_autoBoundary_spawn_teamB != null)
		{
			HighlightUtils.DestroyBoundaryHighlightObject(m_autoBoundary_spawn_teamB);
			m_autoBoundary_spawn_teamB = null;
		}
		if (m_autoBoundary_turnin_neutral != null)
		{
			HighlightUtils.DestroyBoundaryHighlightObject(m_autoBoundary_turnin_neutral);
			m_autoBoundary_turnin_neutral = null;
		}
		if (m_autoBoundary_turnin_teamA != null)
		{
			HighlightUtils.DestroyBoundaryHighlightObject(m_autoBoundary_turnin_teamA);
			m_autoBoundary_turnin_teamA = null;
		}
		if (m_autoBoundary_turnin_teamB != null)
		{
			HighlightUtils.DestroyBoundaryHighlightObject(m_autoBoundary_turnin_teamB);
			m_autoBoundary_turnin_teamB = null;
		}
		s_instance = null;
	}

	public static CaptureTheFlag Get()
	{
		return s_instance;
	}

	public void OnNewFlagStarted(CTF_Flag flag)
	{
		if (m_flags.Contains(flag))
		{
			return;
		}
		while (true)
		{
			m_flags.Add(flag);
			return;
		}
	}

	public void OnFlagDestroyed(CTF_Flag flag)
	{
		if (!m_flags.Contains(flag))
		{
			return;
		}
		while (true)
		{
			m_flags.Remove(flag);
			return;
		}
	}

	private CTF_Flag GetFlagByGuid(byte flagGuid)
	{
		for (int i = 0; i < m_flags.Count; i++)
		{
			if (!(m_flags[i] != null))
			{
				continue;
			}
			if (m_flags[i].m_flagGuid != flagGuid)
			{
				continue;
			}
			while (true)
			{
				return m_flags[i];
			}
		}
		return null;
	}

	private List<CTF_Flag> GetFlagsHeldByActor_Server(ActorData actor)
	{
		List<CTF_Flag> list = null;
		for (int i = 0; i < m_flags.Count; i++)
		{
			if (!(m_flags[i] != null))
			{
				continue;
			}
			if (m_flags[i].ServerHolderActor == actor)
			{
				if (list == null)
				{
					list = new List<CTF_Flag>();
				}
				list.Add(m_flags[i]);
			}
		}
		while (true)
		{
			return list;
		}
	}

	private List<CTF_Flag> GetFlagsHeldByActor_Client(ActorData actor)
	{
		List<CTF_Flag> list = null;
		if (actor != null)
		{
			for (int i = 0; i < m_flags.Count; i++)
			{
				if (!(m_flags[i] != null))
				{
					continue;
				}
				if (!(m_flags[i].ClientHolderActor == actor))
				{
					continue;
				}
				if (list == null)
				{
					list = new List<CTF_Flag>();
				}
				list.Add(m_flags[i]);
			}
		}
		return list;
	}

	public static CTF_Flag GetMainFlag()
	{
		if (s_instance == null)
		{
			return null;
		}
		if (s_instance.m_flags != null)
		{
			if (s_instance.m_flags.Count > 0)
			{
				return s_instance.m_flags[0];
			}
		}
		return null;
	}

	public static BoardSquare GetMainFlagIdleSquare_Server()
	{
		CTF_Flag mainFlag = GetMainFlag();
		if (mainFlag != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return mainFlag.ServerIdleSquare;
				}
			}
		}
		return null;
	}

	public static BoardSquare GetMainFlagIdleSquare_Client()
	{
		CTF_Flag mainFlag = GetMainFlag();
		if (mainFlag != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return mainFlag.ClientIdleSquare;
				}
			}
		}
		return null;
	}

	public static ActorData GetMainFlagCarrier_Server()
	{
		CTF_Flag mainFlag = GetMainFlag();
		if (mainFlag != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return mainFlag.ServerHolderActor;
				}
			}
		}
		return null;
	}

	public static ActorData GetMainFlagCarrier_Client()
	{
		CTF_Flag mainFlag = GetMainFlag();
		if (mainFlag != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return mainFlag.ClientHolderActor;
				}
			}
		}
		return null;
	}

	public static List<ActorData> GetActorsRevealedByFlags_Client()
	{
		List<ActorData> list = new List<ActorData>();
		if (NetworkClient.active)
		{
			if (Get() != null)
			{
				if (Get().m_flagRevealsHolder)
				{
					List<CTF_Flag> flags = Get().m_flags;
					for (int i = 0; i < flags.Count; i++)
					{
						CTF_Flag cTF_Flag = flags[i];
						if (!(cTF_Flag != null) || !(cTF_Flag.ClientHolderActor != null))
						{
							continue;
						}
						if (!list.Contains(cTF_Flag.ClientHolderActor))
						{
							list.Add(cTF_Flag.ClientHolderActor);
						}
					}
				}
			}
		}
		return list;
	}

	public static bool IsActorRevealedByFlag_Client(ActorData actor)
	{
		List<ActorData> actorsRevealedByFlags_Client = GetActorsRevealedByFlags_Client();
		if (actorsRevealedByFlags_Client.Contains(actor))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		return false;
	}

	public static List<ActorData> GetActorsRevealedByFlags_Server()
	{
		List<ActorData> list = new List<ActorData>();
		if (NetworkServer.active && Get() != null)
		{
			if (Get().m_flagRevealsHolder)
			{
				List<CTF_Flag> flags = Get().m_flags;
				for (int i = 0; i < flags.Count; i++)
				{
					CTF_Flag cTF_Flag = flags[i];
					if (!(cTF_Flag != null))
					{
						continue;
					}
					if (cTF_Flag.GatheredHolderActor != null)
					{
						if (!list.Contains(cTF_Flag.GatheredHolderActor))
						{
							list.Add(cTF_Flag.GatheredHolderActor);
						}
					}
				}
			}
		}
		return list;
	}

	public static bool IsActorRevealedByFlag_Server(ActorData actor)
	{
		List<ActorData> actorsRevealedByFlags_Server = GetActorsRevealedByFlags_Server();
		if (actorsRevealedByFlags_Server.Contains(actor))
		{
			return true;
		}
		return false;
	}

	public static BoardRegion GetExtractionRegion()
	{
		if (s_instance != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return s_instance.FlagTurninRegion_Neutral;
				}
			}
		}
		return null;
	}

	public static BoardRegion GetExtractionRegionOfTeam(Team team)
	{
		if (s_instance != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					switch (team)
					{
					case Team.TeamA:
						return s_instance.FlagTurninRegion_TeamA;
					case Team.TeamB:
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								return s_instance.FlagTurninRegion_TeamB;
							}
						}
					default:
						return s_instance.FlagTurninRegion_Neutral;
					}
				}
			}
		}
		return null;
	}

	public static TurninRegionState GetExtractionRegionState()
	{
		if (s_instance != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return s_instance.TurninRegionState_Neutral;
				}
			}
		}
		return TurninRegionState.Disabled;
	}

	public static TurninRegionState GetExtractionRegionStateOfTeam(Team team)
	{
		if (s_instance != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (team == Team.TeamA)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								return s_instance.TurninRegionState_TeamA;
							}
						}
					}
					if (team == Team.TeamB)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								return s_instance.TurninRegionState_TeamB;
							}
						}
					}
					return s_instance.TurninRegionState_Neutral;
				}
			}
		}
		return TurninRegionState.Disabled;
	}

	private void Start()
	{
		m_flagSpawnsNeutral.Initialize();
		m_flagSpawnsTeamA.Initialize();
		m_flagSpawnsTeamB.Initialize();
		m_flagTurninTeamA.Initialize();
		m_flagTurninTeamB.Initialize();
		m_flagTurninNeutral.Initialize();
		for (int i = 0; i < m_potentialFlagTurnins.Count; i++)
		{
			m_potentialFlagTurnins[i].Initialize();
		}
		while (true)
		{
			TurninRegionState_TeamA = m_turninRegionInitialState;
			TurninRegionState_TeamB = m_turninRegionInitialState;
			TurninRegionState_Neutral = m_turninRegionInitialState;
			if (NetworkClient.active)
			{
				GameEventManager.Get().AddListener(this, GameEventManager.EventType.ActorDamaged_Client);
				GameEventManager.Get().AddListener(this, GameEventManager.EventType.ActorHealed_Client);
				GameEventManager.Get().AddListener(this, GameEventManager.EventType.ActorGainedAbsorb_Client);
				GenerateBoundaryVisuals();
			}
			m_flags = new List<CTF_Flag>();
			return;
		}
	}

	public void Client_OnActorDeath(ActorData actor)
	{
		bool flag = false;
		List<ActorData> contributorsToKillOnClient = GameFlowData.Get().GetContributorsToKillOnClient(actor, true);
		List<ActorData> contributorsToKillOnClient2 = GameFlowData.Get().GetContributorsToKillOnClient(actor);
		List<ActorData> list = new List<ActorData>();
		List<ActorData> list2 = new List<ActorData>();
		using (List<CTF_Flag>.Enumerator enumerator = m_flags.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CTF_Flag current = enumerator.Current;
				if (current.ClientHolderActor != null)
				{
					if (current.ClientHolderActor == actor)
					{
						flag = true;
						current.OnDropped_Client(actor.GetMostResetDeathSquare(), -1);
					}
					if (contributorsToKillOnClient.Contains(current.ClientHolderActor))
					{
						list.Add(current.ClientHolderActor);
					}
					if (contributorsToKillOnClient2.Contains(current.ClientHolderActor))
					{
						list2.Add(current.ClientHolderActor);
					}
				}
			}
		}
		if (flag)
		{
			if (ObjectivePoints.Get() != null)
			{
				ObjectivePoints.Get().AdjustUnresolvedPoints(m_objectivePointsData_flagHoldersTeam.m_pointsPerDeathOfFlagHolder, actor.GetTeam());
				ObjectivePoints.Get().AdjustUnresolvedPoints(m_objectivePointsData_otherTeam.m_pointsPerDeathOfFlagHolder, actor.GetOpposingTeam());
			}
		}
		foreach (ActorData item in list)
		{
			if (ObjectivePoints.Get() != null)
			{
				ObjectivePoints.Get().AdjustUnresolvedPoints(m_objectivePointsData_flagHoldersTeam.m_pointsPerDeathblowByFlagHolder, item.GetTeam());
				ObjectivePoints.Get().AdjustUnresolvedPoints(m_objectivePointsData_otherTeam.m_pointsPerDeathblowByFlagHolder, item.GetOpposingTeam());
			}
		}
		using (List<ActorData>.Enumerator enumerator3 = list2.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				ActorData current3 = enumerator3.Current;
				if (ObjectivePoints.Get() != null)
				{
					ObjectivePoints.Get().AdjustUnresolvedPoints(m_objectivePointsData_flagHoldersTeam.m_pointsPerTakedownByFlagHolder, current3.GetTeam());
					ObjectivePoints.Get().AdjustUnresolvedPoints(m_objectivePointsData_otherTeam.m_pointsPerTakedownByFlagHolder, current3.GetOpposingTeam());
				}
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

	public void ExecuteClientGameModeEvent(ClientGameModeEvent gameModeEvent)
	{
		if (gameModeEvent == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		GameModeEventType eventType = gameModeEvent.m_eventType;
		byte objectGuid = gameModeEvent.m_objectGuid;
		CTF_Flag flagByGuid = GetFlagByGuid(objectGuid);
		int eventGuid = gameModeEvent.m_eventGuid;
		if (flagByGuid == null)
		{
			return;
		}
		if (eventType == GameModeEventType.Ctf_FlagPickedUp)
		{
			ActorData primaryActor = gameModeEvent.m_primaryActor;
			flagByGuid.OnPickedUp_Client(primaryActor, eventGuid);
			return;
		}
		if (eventType == GameModeEventType.Ctf_FlagDropped)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					BoardSquare square = gameModeEvent.m_square;
					flagByGuid.OnDropped_Client(square, eventGuid);
					m_clientUnresolvedNumFlagDrops++;
					return;
				}
				}
			}
		}
		if (eventType == GameModeEventType.Ctf_FlagTurnedIn)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					ActorData primaryActor2 = gameModeEvent.m_primaryActor;
					BoardSquare square2 = gameModeEvent.m_square;
					TurnInFlag_Client(flagByGuid, primaryActor2, square2, eventGuid);
					return;
				}
				}
			}
		}
		if (eventType == GameModeEventType.Ctf_FlagSentToSpawn)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					ActorData primaryActor3 = gameModeEvent.m_primaryActor;
					flagByGuid.OnReturned_Client(primaryActor3);
					return;
				}
				}
			}
		}
		Debug.LogError("CaptureTheFlag trying to handle non-CtF event type " + eventType.ToString() + ".");
	}

	private void TurnInFlag_Client(CTF_Flag flag, ActorData capturingActor, BoardSquare captureSquare, int eventGuid)
	{
		Team team = capturingActor.GetTeam();
		if (team == Team.TeamA)
		{
			m_rewardToCapturingTeam.ClientApplyRewardTo(Team.TeamA);
			m_rewardToOtherTeam.ClientApplyRewardTo(Team.TeamB);
		}
		else if (team == Team.TeamB)
		{
			m_rewardToCapturingTeam.ClientApplyRewardTo(Team.TeamB);
			m_rewardToOtherTeam.ClientApplyRewardTo(Team.TeamA);
		}
		m_timeToFocusCameraOnExtraction = Time.time + m_timeTillCameraFocusesOntoExtraction;
		m_lastExtractionSquare = captureSquare;
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
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		UI_CTF_BriefcasePanel.Get().UpdateFlagHolder(oldHolder, newHolder);
	}

	public void Client_OnTurninStateChanged(Team turninRegionTeam, TurninRegionState prevState, TurninRegionState newState)
	{
		if (newState == prevState)
		{
			return;
		}
		while (true)
		{
			if (!(InterfaceManager.Get() != null))
			{
				return;
			}
			while (true)
			{
				if (!(GameFlowData.Get() != null))
				{
					return;
				}
				while (true)
				{
					if (!GameFlowData.Get().LocalPlayerData)
					{
						return;
					}
					Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
					if (teamViewing != 0)
					{
						if (teamViewing != Team.TeamB)
						{
							goto IL_00de;
						}
					}
					if (turninRegionTeam != 0)
					{
						if (turninRegionTeam != Team.TeamB)
						{
							goto IL_00de;
						}
					}
					RelationshipToClient relationship;
					Color color;
					if (teamViewing == turninRegionTeam)
					{
						relationship = RelationshipToClient.Friendly;
						color = m_textColor_positive;
					}
					else
					{
						relationship = RelationshipToClient.Hostile;
						color = m_textColor_negative;
					}
					goto IL_00e7;
					IL_00e7:
					string turninStateChangedString = GetTurninStateChangedString(relationship, newState);
					string alertText = StringUtil.TR(turninStateChangedString);
					InterfaceManager.Get().DisplayAlert(alertText, color, 5f, true, 1);
					return;
					IL_00de:
					relationship = RelationshipToClient.Neutral;
					color = m_textColor_neutral;
					goto IL_00e7;
				}
			}
		}
	}

	private string GetTurninStateChangedString(RelationshipToClient relationship, TurninRegionState newState)
	{
		if (relationship == RelationshipToClient.Friendly)
		{
			if (newState == TurninRegionState.Active)
			{
				return m_alliedExtractionPointNowActive;
			}
			if (newState == TurninRegionState.Disabled)
			{
				while (true)
				{
					return m_alliedExtractionPointNowInactive;
				}
			}
			if (newState == TurninRegionState.Locked)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return m_alliedExtractionPointNowUnlocking;
					}
				}
			}
		}
		else if (relationship == RelationshipToClient.Hostile)
		{
			if (newState == TurninRegionState.Active)
			{
				while (true)
				{
					return m_enemyExtractionPointNowActive;
				}
			}
			switch (newState)
			{
			case TurninRegionState.Disabled:
				return m_enemyExtractionPointNowInactive;
			case TurninRegionState.Locked:
				return m_enemyExtractionPointNowUnlocking;
			}
		}
		else if (relationship == RelationshipToClient.Neutral)
		{
			if (newState == TurninRegionState.Active)
			{
				while (true)
				{
					return m_neutralExtractionPointNowActive;
				}
			}
			if (newState == TurninRegionState.Disabled)
			{
				while (true)
				{
					return m_neutralExtractionPointNowInactive;
				}
			}
			if (newState == TurninRegionState.Locked)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return m_neutralExtractionPointNowUnlocking;
					}
				}
			}
		}
		Debug.LogWarning($"CaptureTheFlag trying to find string for turnin point changed, but failed.  Relationship = {relationship.ToString()}, new state = {newState.ToString()}.  Returning empty string...");
		return string.Empty;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (args == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
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
			OnActorHealthChanged(args, true);
			return;
		}
	}

	private void OnActorHealthChanged(GameEventManager.GameEventArgs args, bool clientMode)
	{
		if (m_evasionDropsFlags)
		{
			if (ServerClientUtils.GetCurrentAbilityPhase() == AbilityPriority.Evasion)
			{
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
		GameEventManager.ActorHitHealthChangeArgs actorHitHealthChangeArgs = args as GameEventManager.ActorHitHealthChangeArgs;
		bool fromCharacterSpecificAbility = actorHitHealthChangeArgs.m_fromCharacterSpecificAbility;
		List<CTF_Flag> flagsHeldByActor_Client = GetFlagsHeldByActor_Client(actorHitHealthChangeArgs.m_caster);
		if (flagsHeldByActor_Client != null && flagsHeldByActor_Client.Count > 0)
		{
			if (actorHitHealthChangeArgs.m_caster != null)
			{
				Team team = actorHitHealthChangeArgs.m_caster.GetTeam();
				Team opposingTeam = actorHitHealthChangeArgs.m_caster.GetOpposingTeam();
				float pointsPerHealthChange = GetPointsPerHealthChange(m_objectivePointsData_flagHoldersTeam, actorHitHealthChangeArgs.m_type, true, fromCharacterSpecificAbility);
				float pointsPerHealthChange2 = GetPointsPerHealthChange(m_objectivePointsData_otherTeam, actorHitHealthChangeArgs.m_type, true, fromCharacterSpecificAbility);
				int points = Mathf.RoundToInt(pointsPerHealthChange * (float)actorHitHealthChangeArgs.m_amount);
				int points2 = Mathf.RoundToInt(pointsPerHealthChange2 * (float)actorHitHealthChangeArgs.m_amount);
				AdjustObjectivePoints(points, team, clientMode);
				AdjustObjectivePoints(points2, opposingTeam, clientMode);
			}
		}
		List<CTF_Flag> flagsHeldByActor_Client2 = GetFlagsHeldByActor_Client(actorHitHealthChangeArgs.m_target);
		if (flagsHeldByActor_Client2 == null)
		{
			return;
		}
		while (true)
		{
			if (flagsHeldByActor_Client2.Count > 0)
			{
				Team team2 = actorHitHealthChangeArgs.m_target.GetTeam();
				Team opposingTeam2 = actorHitHealthChangeArgs.m_target.GetOpposingTeam();
				float pointsPerHealthChange3 = GetPointsPerHealthChange(m_objectivePointsData_flagHoldersTeam, actorHitHealthChangeArgs.m_type, false, fromCharacterSpecificAbility);
				float pointsPerHealthChange4 = GetPointsPerHealthChange(m_objectivePointsData_otherTeam, actorHitHealthChangeArgs.m_type, false, fromCharacterSpecificAbility);
				int points3 = Mathf.RoundToInt(pointsPerHealthChange3 * (float)actorHitHealthChangeArgs.m_amount);
				int points4 = Mathf.RoundToInt(pointsPerHealthChange4 * (float)actorHitHealthChangeArgs.m_amount);
				AdjustObjectivePoints(points3, team2, clientMode);
				AdjustObjectivePoints(points4, opposingTeam2, clientMode);
			}
			return;
		}
	}

	private float GetPointsPerHealthChange(FlagHolderObjectivePointData data, GameEventManager.ActorHitHealthChangeArgs.ChangeType healthChangeType, bool outgoing, bool fromCharacterSpecificAbility)
	{
		if (!fromCharacterSpecificAbility)
		{
			if (!data.m_includeContributionFromNonCharacterAbilities)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return 0f;
					}
				}
			}
		}
		if (healthChangeType == GameEventManager.ActorHitHealthChangeArgs.ChangeType.Damage)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (outgoing)
					{
						return data.m_pointsPerDamageDealtByFlagHolder;
					}
					return data.m_pointsPerDamageTakenByFlagHolder;
				}
			}
		}
		switch (healthChangeType)
		{
		case GameEventManager.ActorHitHealthChangeArgs.ChangeType.Healing:
			if (outgoing)
			{
				return data.m_pointsPerHealingDealtByFlagHolder;
			}
			return data.m_pointsPerHealingTakenByFlagHolder;
		case GameEventManager.ActorHitHealthChangeArgs.ChangeType.Absorb:
			while (true)
			{
				if (outgoing)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							return data.m_pointsPerAbsorbDealtByFlagHolder;
						}
					}
				}
				return data.m_pointsPerAbsorbTakenByFlagHolder;
			}
		default:
			return 0f;
		}
	}

	private void AdjustObjectivePoints(int points, Team team, bool clientMode)
	{
		if (points == 0)
		{
			return;
		}
		while (true)
		{
			if (clientMode)
			{
				ObjectivePoints.Get().AdjustUnresolvedPoints(points, team);
			}
			else
			{
				ObjectivePoints.Get().AdjustPoints(points, team);
			}
			return;
		}
	}

	public static bool AreCtfVictoryConditionsMetForTeam(CTF_VictoryCondition[] conditions, Team checkTeam)
	{
		if (Get() == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		if (conditions != null)
		{
			if (conditions.Length != 0)
			{
				if (checkTeam != 0 && checkTeam != Team.TeamB)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
				bool flag = false;
				bool flag2 = false;
				bool flag3;
				bool flag4;
				if (checkTeam == Team.TeamA)
				{
					flag3 = (Get().m_teamACaptures > 0);
					flag4 = (Get().m_teamBCaptures > 0);
				}
				else
				{
					flag3 = (Get().m_teamBCaptures > 0);
					flag4 = (Get().m_teamACaptures > 0);
				}
				foreach (CTF_Flag flag5 in Get().m_flags)
				{
					if (flag5.ServerHolderActor != null)
					{
						if (flag5.ServerHolderActor.GetTeam() == checkTeam)
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
				foreach (CTF_VictoryCondition cTF_VictoryCondition in conditions)
				{
					if (cTF_VictoryCondition == CTF_VictoryCondition.TeamMustBeHoldingFlag)
					{
						if (!flag)
						{
							result = false;
							break;
						}
					}
					else if (cTF_VictoryCondition == CTF_VictoryCondition.TeamMustNotBeHoldingFlag)
					{
						if (flag)
						{
							result = false;
							break;
						}
					}
					else if (cTF_VictoryCondition == CTF_VictoryCondition.OtherTeamMustBeHoldingFlag)
					{
						if (!flag2)
						{
							result = false;
							break;
						}
					}
					else if (cTF_VictoryCondition == CTF_VictoryCondition.OtherTeamMustNotBeHoldingFlag)
					{
						if (flag2)
						{
							result = false;
							break;
						}
					}
					else if (cTF_VictoryCondition == CTF_VictoryCondition.TeamMustHaveCapturedFlag)
					{
						if (!flag3)
						{
							result = false;
							break;
						}
					}
					else if (cTF_VictoryCondition == CTF_VictoryCondition.TeamMustNotHaveCapturedFlag)
					{
						if (flag3)
						{
							result = false;
							break;
						}
					}
					else if (cTF_VictoryCondition == CTF_VictoryCondition.OtherTeamMustHaveCapturedFlag)
					{
						if (!flag4)
						{
							result = false;
							break;
						}
					}
					else
					{
						if (cTF_VictoryCondition != CTF_VictoryCondition.OtherTeamMustNotHaveCapturedFlag)
						{
							continue;
						}
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
		TurninRegionState turninRegionState_TeamA2 = (TurninRegionState)m_turninRegionState_TeamA;
		Networkm_turninRegionState_TeamA = turninRegionState_TeamA;
		if (FlagTurninRegion_TeamA != null)
		{
			if (FlagTurninRegion_TeamA.HasNonZeroArea())
			{
				Client_OnTurninStateChanged(Team.TeamA, turninRegionState_TeamA2, TurninRegionState_TeamA);
			}
		}
		OnTurninChanged_TeamA();
	}

	protected void HookSetTurninRegionState_TeamB(int turninRegionState_TeamB)
	{
		TurninRegionState turninRegionState_TeamB2 = (TurninRegionState)m_turninRegionState_TeamB;
		Networkm_turninRegionState_TeamB = turninRegionState_TeamB;
		if (FlagTurninRegion_TeamB != null)
		{
			if (FlagTurninRegion_TeamB.HasNonZeroArea())
			{
				Client_OnTurninStateChanged(Team.TeamB, turninRegionState_TeamB2, TurninRegionState_TeamB);
			}
		}
		OnTurninChanged_TeamB();
	}

	protected void HookSetTurninRegionState_Neutral(int turninRegionState_Neutral)
	{
		TurninRegionState turninRegionState_Neutral2 = (TurninRegionState)m_turninRegionState_Neutral;
		Networkm_turninRegionState_Neutral = turninRegionState_Neutral;
		if (FlagTurninRegion_Neutral != null)
		{
			if (FlagTurninRegion_Neutral.HasNonZeroArea())
			{
				Client_OnTurninStateChanged(Team.Invalid, turninRegionState_Neutral2, TurninRegionState_Neutral);
			}
		}
		OnTurninChanged_Neutral();
	}

	protected void HookSetTurninRegionIndex_TeamA(int turninRegionIndex_TeamA)
	{
		int turninRegionIndex_TeamA2 = m_turninRegionIndex_TeamA;
		Networkm_turninRegionIndex_TeamA = turninRegionIndex_TeamA;
		if (turninRegionIndex_TeamA2 == -1 && FlagTurninRegion_TeamA != null)
		{
			GenerateFlagTurninVisuals();
			if (TurninRegionState_TeamA != TurninRegionState.Disabled)
			{
				Client_OnTurninStateChanged(Team.TeamA, TurninRegionState.Disabled, TurninRegionState_TeamA);
			}
		}
		OnTurninChanged_TeamA();
	}

	protected void HookSetTurninRegionIndex_TeamB(int turninRegionIndex_TeamB)
	{
		int turninRegionIndex_TeamB2 = m_turninRegionIndex_TeamB;
		Networkm_turninRegionIndex_TeamB = turninRegionIndex_TeamB;
		if (turninRegionIndex_TeamB2 == -1)
		{
			if (FlagTurninRegion_TeamB != null)
			{
				GenerateFlagTurninVisuals();
				if (TurninRegionState_TeamB != TurninRegionState.Disabled)
				{
					Client_OnTurninStateChanged(Team.TeamB, TurninRegionState.Disabled, TurninRegionState_TeamB);
				}
			}
		}
		OnTurninChanged_TeamB();
	}

	protected void HookSetTurninRegionIndex_Neutral(int turninRegionIndex_Neutral)
	{
		int turninRegionIndex_Neutral2 = m_turninRegionIndex_Neutral;
		Networkm_turninRegionIndex_Neutral = turninRegionIndex_Neutral;
		if (turninRegionIndex_Neutral2 == -1)
		{
			if (FlagTurninRegion_Neutral != null)
			{
				GenerateFlagTurninVisuals();
				if (TurninRegionState_Neutral != TurninRegionState.Disabled)
				{
					Client_OnTurninStateChanged(Team.Invalid, TurninRegionState.Disabled, TurninRegionState_Neutral);
				}
			}
		}
		OnTurninChanged_Neutral();
	}

	protected void HookSetNumFlagDrops(int numFlagDrops)
	{
		Networkm_numFlagDrops = numFlagDrops;
		m_clientUnresolvedNumFlagDrops = 0;
	}

	private void OnTurninChanged_TeamA()
	{
		bool flag = TurninRegionState_TeamA == TurninRegionState.Active;
		bool flag2 = m_turninRegionIndex_TeamA >= 0;
		bool flag3 = m_timeTillCameraFocusesOntoExtractionPoint >= 0f;
		bool flag4 = m_timeToFocusCameraOnTurninTeamA >= 0f;
		if (flag)
		{
			if (flag2)
			{
				if (flag3)
				{
					if (!flag4)
					{
						m_timeToFocusCameraOnTurninTeamA = Time.time + m_timeTillCameraFocusesOntoExtractionPoint;
					}
				}
			}
		}
		if (FlagTurninRegion_TeamA == null)
		{
			return;
		}
		while (true)
		{
			if (!(HUD_UI.Get() != null))
			{
				return;
			}
			while (true)
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
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
							{
								UIOffscreenIndicatorPanel offscreenIndicatorPanel = HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel;
								BoardRegion flagTurninRegion_TeamA = FlagTurninRegion_TeamA;
								int teamRegion;
								if (flag5)
								{
									teamRegion = 0;
								}
								else
								{
									teamRegion = 1;
								}
								offscreenIndicatorPanel.AddCtfFlagTurnInRegion(flagTurninRegion_TeamA, (Team)teamRegion);
								return;
							}
							}
						}
					}
				}
				HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.RemoveCtfFlagTurnInRegion(FlagTurninRegion_TeamA);
				return;
			}
		}
	}

	private void OnTurninChanged_TeamB()
	{
		bool flag = TurninRegionState_TeamB == TurninRegionState.Active;
		bool flag2 = m_turninRegionIndex_TeamB >= 0;
		bool flag3 = m_timeTillCameraFocusesOntoExtractionPoint >= 0f;
		bool flag4 = m_timeToFocusCameraOnTurninTeamB >= 0f;
		if (flag)
		{
			if (flag2 && flag3)
			{
				if (!flag4)
				{
					m_timeToFocusCameraOnTurninTeamB = Time.time + m_timeTillCameraFocusesOntoExtractionPoint;
				}
			}
		}
		if (FlagTurninRegion_TeamB == null)
		{
			return;
		}
		while (true)
		{
			if (!(HUD_UI.Get() != null))
			{
				return;
			}
			while (true)
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
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddCtfFlagTurnInRegion(FlagTurninRegion_TeamB, (!flag5) ? Team.TeamB : Team.TeamA);
								return;
							}
						}
					}
				}
				HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.RemoveCtfFlagTurnInRegion(FlagTurninRegion_TeamB);
				return;
			}
		}
	}

	private void OnTurninChanged_Neutral()
	{
		bool flag = TurninRegionState_Neutral == TurninRegionState.Active;
		bool flag2 = m_turninRegionIndex_Neutral >= 0;
		bool flag3 = m_timeTillCameraFocusesOntoExtractionPoint >= 0f;
		bool flag4 = m_timeToFocusCameraOnTurninNeutral >= 0f;
		if (flag)
		{
			if (flag2 && flag3)
			{
				if (!flag4)
				{
					m_timeToFocusCameraOnTurninNeutral = Time.time + m_timeTillCameraFocusesOntoExtractionPoint;
				}
			}
		}
		if (FlagTurninRegion_Neutral == null)
		{
			return;
		}
		while (true)
		{
			if (!(HUD_UI.Get() != null))
			{
				return;
			}
			if (flag)
			{
				if (flag2)
				{
					HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddCtfFlagTurnInRegion(FlagTurninRegion_Neutral);
					return;
				}
			}
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.RemoveCtfFlagTurnInRegion(FlagTurninRegion_Neutral);
			return;
		}
	}

	private GameObject InstantiateBoundaryObject(BoardRegion region, string boundaryName)
	{
		if (region != null)
		{
			if (region.GetSquaresInRegion().Count > 0)
			{
				GameObject gameObject = HighlightUtils.Get().CreateBoundaryHighlight(region.GetSquaresInRegion(), Color.yellow);
				gameObject.name = base.name + " " + boundaryName;
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				Vector3 position = gameObject.transform.position;
				m_autoBoundaryHeight = position.y;
				return gameObject;
			}
		}
		return null;
	}

	private void GenerateBoundaryVisuals()
	{
		if (m_autoGenerateSpawnLocVisuals)
		{
			GenerateFlagSpawnBoundaryVisuals();
		}
		if (!m_autoGenerateTurninVisuals)
		{
			return;
		}
		while (true)
		{
			GenerateFlagTurninVisuals();
			return;
		}
	}

	private void GenerateFlagSpawnBoundaryVisuals()
	{
		m_autoBoundary_spawn_neutral = InstantiateBoundaryObject(m_flagSpawnsNeutral, "Neutral Flag-Spawn Auto-Boundary");
		m_autoBoundary_spawn_teamA = InstantiateBoundaryObject(m_flagSpawnsTeamA, "TeamA Flag-Spawn Auto-Boundary");
		m_autoBoundary_spawn_teamB = InstantiateBoundaryObject(m_flagSpawnsTeamB, "TeamB Flag-Spawn Auto-Boundary");
	}

	private void GenerateFlagTurninVisuals()
	{
		if (m_autoBoundary_turnin_neutral == null)
		{
			if (FlagTurninRegion_Neutral != null)
			{
				m_autoBoundary_turnin_neutral = InstantiateBoundaryObject(FlagTurninRegion_Neutral, "Neutral Turnin Auto-Boundary");
			}
		}
		if (m_autoBoundary_turnin_teamA == null)
		{
			if (FlagTurninRegion_TeamA != null)
			{
				m_autoBoundary_turnin_teamA = InstantiateBoundaryObject(FlagTurninRegion_TeamA, "TeamA Turnin Auto-Boundary");
			}
		}
		if (!(m_autoBoundary_turnin_teamB == null))
		{
			return;
		}
		while (true)
		{
			if (FlagTurninRegion_TeamB != null)
			{
				while (true)
				{
					m_autoBoundary_turnin_teamB = InstantiateBoundaryObject(FlagTurninRegion_TeamB, "TeamB Turnin Auto-Boundary");
					return;
				}
			}
			return;
		}
	}

	private void SetBoundaryColor(GameObject autoBoundary, Color mainColor, Color secondaryColor, float oscillationLevel)
	{
		if (!(autoBoundary != null))
		{
			return;
		}
		while (true)
		{
			float num = 1f - oscillationLevel * oscillationLevel;
			float num2 = oscillationLevel * oscillationLevel;
			Color color = new Color(mainColor.r * num + secondaryColor.r * num2, mainColor.g * num + secondaryColor.g * num2, mainColor.b * num + secondaryColor.b * num2, mainColor.a * num + secondaryColor.a * num2);
			SetBoundaryColor(autoBoundary, color);
			return;
		}
	}

	private void SetBoundaryColor(GameObject autoBoundary, Color color)
	{
		if (!(autoBoundary != null))
		{
			return;
		}
		while (true)
		{
			autoBoundary.GetComponent<Renderer>().material.SetColor("_TintColor", color);
			return;
		}
	}

	private Color DetermineSecondaryColor(bool condition1, Color color1, bool condition2, Color color2, Color fallbackColor)
	{
		if (condition1)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return color1;
				}
			}
		}
		if (condition2)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return color2;
				}
			}
		}
		return fallbackColor;
	}

	private void AdjustPositionOfObjToOscillation(GameObject obj, float oscillationLevel)
	{
		if (!(obj != null))
		{
			return;
		}
		while (true)
		{
			float num = oscillationLevel * m_boundaryOscillationHeight;
			Transform transform = obj.transform;
			Vector3 position = obj.transform.position;
			float x = position.x;
			float y = m_autoBoundaryHeight + num;
			Vector3 position2 = obj.transform.position;
			transform.position = new Vector3(x, y, position2.z);
			return;
		}
	}

	private void Update()
	{
		float oscillationLevel = (1f - Mathf.Cos(Time.time * m_boundaryOscillationSpeed)) / 2f;
		Team team;
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().LocalPlayerData != null)
			{
				team = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
				goto IL_0080;
			}
		}
		team = Team.Invalid;
		goto IL_0080;
		IL_03af:
		if (m_timeToFocusCameraOnTurninTeamA > 0f)
		{
			if (m_timeToFocusCameraOnTurninTeamA <= Time.time)
			{
				if (CameraManager.Get() != null)
				{
					if (FlagTurninRegion_TeamA != null)
					{
						if (FlagTurninRegion_TeamA.GetCenterSquare() != null)
						{
							CameraManager.Get().SetTargetObject(FlagTurninRegion_TeamA.GetCenterSquare().gameObject, CameraManager.CameraTargetReason.CtfTurninRegionSpawned);
						}
					}
				}
				m_timeToFocusCameraOnTurninTeamA = -1f;
				CreateTurninRegionActivatedSequence(Team.TeamA, FlagTurninRegion_TeamA.GetCenter());
			}
		}
		if (m_timeToFocusCameraOnTurninTeamB > 0f)
		{
			if (m_timeToFocusCameraOnTurninTeamB <= Time.time)
			{
				if (CameraManager.Get() != null)
				{
					if (FlagTurninRegion_TeamB != null)
					{
						if (FlagTurninRegion_TeamB.GetCenterSquare() != null)
						{
							CameraManager.Get().SetTargetObject(FlagTurninRegion_TeamB.GetCenterSquare().gameObject, CameraManager.CameraTargetReason.CtfTurninRegionSpawned);
						}
					}
				}
				m_timeToFocusCameraOnTurninTeamB = -1f;
				CreateTurninRegionActivatedSequence(Team.TeamB, FlagTurninRegion_TeamB.GetCenter());
			}
		}
		if (m_timeToFocusCameraOnTurninNeutral > 0f)
		{
			if (m_timeToFocusCameraOnTurninNeutral <= Time.time)
			{
				if (CameraManager.Get() != null)
				{
					if (FlagTurninRegion_Neutral != null && FlagTurninRegion_Neutral.GetCenterSquare() != null)
					{
						CameraManager.Get().SetTargetObject(FlagTurninRegion_Neutral.GetCenterSquare().gameObject, CameraManager.CameraTargetReason.CtfTurninRegionSpawned);
					}
				}
				m_timeToFocusCameraOnTurninNeutral = -1f;
				CreateTurninRegionActivatedSequence(Team.Invalid, FlagTurninRegion_Neutral.GetCenter());
			}
		}
		if (!(m_timeToFocusCameraOnExtraction > 0f))
		{
			return;
		}
		while (true)
		{
			if (!(m_timeToFocusCameraOnExtraction <= Time.time))
			{
				return;
			}
			while (true)
			{
				if (CameraManager.Get() != null && m_lastExtractionSquare != null)
				{
					CameraManager.Get().SetTargetObject(m_lastExtractionSquare.gameObject, CameraManager.CameraTargetReason.CtfFlagTurnedIn);
				}
				m_timeToFocusCameraOnExtraction = -1f;
				m_timeToFocusCameraOnTurninTeamA = -1f;
				m_timeToFocusCameraOnTurninTeamB = -1f;
				m_timeToFocusCameraOnTurninNeutral = -1f;
				m_lastExtractionSquare = null;
				return;
			}
		}
		IL_0080:
		Color color;
		Color color2;
		if (team == Team.TeamA)
		{
			color = m_primaryColor_friendly;
			color2 = m_primaryColor_hostile;
		}
		else if (team == Team.TeamB)
		{
			color = m_primaryColor_hostile;
			color2 = m_primaryColor_friendly;
		}
		else
		{
			color = m_primaryColor_neutral;
			color2 = m_primaryColor_neutral;
		}
		AdjustPositionOfObjToOscillation(m_autoBoundary_spawn_neutral, oscillationLevel);
		SetBoundaryColor(m_autoBoundary_spawn_neutral, m_primaryColor_neutral);
		AdjustPositionOfObjToOscillation(m_autoBoundary_spawn_teamA, oscillationLevel);
		SetBoundaryColor(m_autoBoundary_spawn_teamA, color);
		AdjustPositionOfObjToOscillation(m_autoBoundary_spawn_teamB, oscillationLevel);
		SetBoundaryColor(m_autoBoundary_spawn_teamB, color2);
		if (TurninRegionState_Neutral != TurninRegionState.Disabled)
		{
			AdjustPositionOfObjToOscillation(m_autoBoundary_turnin_neutral, oscillationLevel);
			Color secondaryColor = (TurninRegionState_Neutral != TurninRegionState.Locked) ? m_primaryColor_neutral : m_secondaryColor_locked;
			SetBoundaryColor(m_autoBoundary_turnin_neutral, m_primaryColor_neutral, secondaryColor, oscillationLevel);
		}
		else
		{
			AdjustPositionOfObjToOscillation(m_autoBoundary_turnin_neutral, 0f);
			SetBoundaryColor(color: new Color(m_primaryColor_neutral.r * 0.5f, m_primaryColor_neutral.g * 0.5f, m_primaryColor_neutral.b * 0.5f, m_primaryColor_neutral.a * 0.5f), autoBoundary: m_autoBoundary_turnin_neutral);
		}
		if (TurninRegionState_TeamA != TurninRegionState.Disabled)
		{
			AdjustPositionOfObjToOscillation(m_autoBoundary_turnin_teamA, oscillationLevel);
			Color color4;
			if (TurninRegionState_TeamA == TurninRegionState.Locked)
			{
				color4 = m_secondaryColor_locked;
			}
			else
			{
				color4 = color;
			}
			Color secondaryColor2 = color4;
			SetBoundaryColor(m_autoBoundary_turnin_teamA, color, secondaryColor2, oscillationLevel);
		}
		else
		{
			AdjustPositionOfObjToOscillation(m_autoBoundary_turnin_teamA, 0f);
			SetBoundaryColor(color: new Color(color.r * 0.5f, color.g * 0.5f, color.b * 0.5f, color.a * 0.5f), autoBoundary: m_autoBoundary_turnin_teamA);
		}
		if (TurninRegionState_TeamB != TurninRegionState.Disabled)
		{
			AdjustPositionOfObjToOscillation(m_autoBoundary_turnin_teamB, oscillationLevel);
			Color secondaryColor3 = (TurninRegionState_TeamB != TurninRegionState.Locked) ? color2 : m_secondaryColor_locked;
			SetBoundaryColor(m_autoBoundary_turnin_teamB, color2, secondaryColor3, oscillationLevel);
		}
		else
		{
			AdjustPositionOfObjToOscillation(m_autoBoundary_turnin_teamB, 0f);
			SetBoundaryColor(color: new Color(color2.r * 0.5f, color2.g * 0.5f, color2.b * 0.5f, color2.a * 0.5f), autoBoundary: m_autoBoundary_turnin_teamB);
		}
		GetFlagCarrierDamageTillDropProgressForUI(out float cur, out float max);
		UI_CTF_BriefcasePanel uI_CTF_BriefcasePanel = UI_CTF_BriefcasePanel.Get();
		if (uI_CTF_BriefcasePanel != null)
		{
			if (!uI_CTF_BriefcasePanel.m_initialized)
			{
				uI_CTF_BriefcasePanel.Setup(this);
			}
			if (cur == m_lastFlagCarrierDamageCur)
			{
				if (max == m_lastFlagCarrierDamageMax)
				{
					goto IL_03af;
				}
			}
			if (uI_CTF_BriefcasePanel.UpdateDamageForFlagHolder(cur, max))
			{
				m_lastFlagCarrierDamageCur = cur;
				m_lastFlagCarrierDamageMax = max;
			}
		}
		goto IL_03af;
	}

	public void CreateTurninRegionActivatedSequence(Team teamOfTurninRegionActivating, Vector3 centerPos)
	{
		Team team;
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().LocalPlayerData != null)
			{
				team = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
				goto IL_0050;
			}
		}
		team = Team.Invalid;
		goto IL_0050;
		IL_0050:
		GameObject gameObject;
		if (teamOfTurninRegionActivating != 0)
		{
			if (teamOfTurninRegionActivating != Team.TeamB)
			{
				gameObject = m_neutralTurninRegionActivatedSequence;
				goto IL_00a3;
			}
		}
		if (team != teamOfTurninRegionActivating)
		{
			if (teamOfTurninRegionActivating != 0 || team == Team.TeamB)
			{
				gameObject = m_enemyTurninRegionActivatedSequence;
				goto IL_00a3;
			}
		}
		gameObject = m_friendlyTurninRegionActivatedSequence;
		goto IL_00a3;
		IL_00a3:
		if (!(gameObject != null))
		{
			return;
		}
		while (true)
		{
			SequenceManager.Get().CreateClientSequences(gameObject, centerPos, new ActorData[0], null, SequenceSource, null);
			return;
		}
	}

	public static float GetFlagCarrierDamageTillDropProgressForUI(out float cur, out float max)
	{
		CTF_Flag mainFlag = GetMainFlag();
		cur = -1f;
		max = -1f;
		int num = (s_instance != null) ? (s_instance.m_numFlagDrops + s_instance.m_clientUnresolvedNumFlagDrops) : 0;
		if (s_instance == null)
		{
			max = 1f;
			cur = 1f;
		}
		else if (s_instance.m_damageInOneTurnToDropFlag_gross > 0)
		{
			max = s_instance.m_damageInOneTurnToDropFlag_gross + s_instance.m_damageThesholdIncreaseOnDrop * num;
			if (mainFlag != null)
			{
				cur = mainFlag.DamageOnHolderSinceTurnStart_Gross + mainFlag.ClientUnresolvedDamageOnHolder;
			}
			else
			{
				cur = max;
			}
		}
		else if (s_instance.m_damageSincePickedUpToDropFlag_gross > 0)
		{
			max = s_instance.m_damageSincePickedUpToDropFlag_gross + s_instance.m_damageThesholdIncreaseOnDrop * num;
			if (mainFlag != null)
			{
				cur = mainFlag.DamageOnHolderSincePickedUp_Gross + mainFlag.ClientUnresolvedDamageOnHolder;
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
		CTF_Flag mainFlag = GetMainFlag();
		ActorData mainFlagCarrier_Client = GetMainFlagCarrier_Client();
		if (!(s_instance != null))
		{
			return;
		}
		while (true)
		{
			if (!(mainFlag != null) || !(mainFlagCarrier_Client != null))
			{
				return;
			}
			while (true)
			{
				if (!(actor != null))
				{
					return;
				}
				while (true)
				{
					if (mainFlagCarrier_Client == actor)
					{
						while (true)
						{
							mainFlag.ClientUnresolvedDamageOnHolder += damage;
							return;
						}
					}
					return;
				}
			}
		}
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					writer.WritePackedUInt32((uint)m_turninRegionState_TeamA);
					writer.WritePackedUInt32((uint)m_turninRegionState_TeamB);
					writer.WritePackedUInt32((uint)m_turninRegionState_Neutral);
					writer.WritePackedUInt32((uint)m_turninRegionIndex_TeamA);
					writer.WritePackedUInt32((uint)m_turninRegionIndex_TeamB);
					writer.WritePackedUInt32((uint)m_turninRegionIndex_Neutral);
					writer.WritePackedUInt32((uint)m_numFlagDrops);
					writer.WritePackedUInt32(m_sequenceSourceId);
					return true;
				}
			}
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_turninRegionState_TeamA);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_turninRegionState_TeamB);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_turninRegionState_Neutral);
		}
		if ((base.syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_turninRegionIndex_TeamA);
		}
		if ((base.syncVarDirtyBits & 0x10) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_turninRegionIndex_TeamB);
		}
		if ((base.syncVarDirtyBits & 0x20) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_turninRegionIndex_Neutral);
		}
		if ((base.syncVarDirtyBits & 0x40) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_numFlagDrops);
		}
		if ((base.syncVarDirtyBits & 0x80) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32(m_sequenceSourceId);
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
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					m_turninRegionState_TeamA = (int)reader.ReadPackedUInt32();
					m_turninRegionState_TeamB = (int)reader.ReadPackedUInt32();
					m_turninRegionState_Neutral = (int)reader.ReadPackedUInt32();
					m_turninRegionIndex_TeamA = (int)reader.ReadPackedUInt32();
					m_turninRegionIndex_TeamB = (int)reader.ReadPackedUInt32();
					m_turninRegionIndex_Neutral = (int)reader.ReadPackedUInt32();
					m_numFlagDrops = (int)reader.ReadPackedUInt32();
					m_sequenceSourceId = reader.ReadPackedUInt32();
					return;
				}
			}
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			HookSetTurninRegionState_TeamA((int)reader.ReadPackedUInt32());
		}
		if ((num & 2) != 0)
		{
			HookSetTurninRegionState_TeamB((int)reader.ReadPackedUInt32());
		}
		if ((num & 4) != 0)
		{
			HookSetTurninRegionState_Neutral((int)reader.ReadPackedUInt32());
		}
		if ((num & 8) != 0)
		{
			HookSetTurninRegionIndex_TeamA((int)reader.ReadPackedUInt32());
		}
		if ((num & 0x10) != 0)
		{
			HookSetTurninRegionIndex_TeamB((int)reader.ReadPackedUInt32());
		}
		if ((num & 0x20) != 0)
		{
			HookSetTurninRegionIndex_Neutral((int)reader.ReadPackedUInt32());
		}
		if ((num & 0x40) != 0)
		{
			HookSetNumFlagDrops((int)reader.ReadPackedUInt32());
		}
		if ((num & 0x80) == 0)
		{
			return;
		}
		while (true)
		{
			m_sequenceSourceId = reader.ReadPackedUInt32();
			return;
		}
	}
}
