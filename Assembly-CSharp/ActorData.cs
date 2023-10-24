// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
//using System.Linq;
//using System.Runtime.InteropServices;
//using EffectSystem;
using Fabric;
//using Mirror;
//using MoonSharp.Interpreter;
//using Run;
//using Talents;
using UnityEngine;
using UnityEngine.Networking;

public class ActorData : NetworkBehaviour, IGameEventListener
{
	internal enum IntType
	{
		HP,
		TP,
		ABSORB,
		MAX_HP,
		MAX_TP,
		HP_UNRESOLVED_DAMAGE,
		HP_UNRESOLVED_HEALING,
		NUM_TYPES
	}

	public delegate void ActorDataDelegate();

	internal enum NetChannelIdOffset
	{
		Default,
		ActorData,
		Count
	}

	public enum MovementType
	{
		None = -1,
		Normal,
		Teleport,
		Knockback,
		Charge,
		Flight,
		WaypointFlight
	}

	public enum TeleportType
	{
		NotATeleport,
		InitialSpawn,
		Respawn,
		Evasion_DontAdjustToVision,
		Evasion_AdjustToVision,
		Reappear,
		Failsafe,
		Debug,
		TricksterAfterImage

		//_001D = Debug // TODO INLINE
	}

	public enum MovementChangeType
	{
		MoreMovement,
		LessMovement
	}

	//[SyncVar(hook = "OnPlayerIndexUpdated")]
	[HideInInspector]
	public int PlayerIndex = -1;
	[HideInInspector]
	public PlayerData PlayerData;

	//public List<EffectTemplate> m_onInitEffectTemplates = new List<EffectTemplate>();  // rogues

	[Separator("Character Type", true)]
	public CharacterType m_characterType;

	// added in rogues
	//internal List<string> m_characterTypeDefaultTags;  // public in rogues

	[Separator("Taunt Camera Data Reference", true)]
	public TauntCameraSet m_tauntCamSetData;

	public static int s_invalidActorIndex = -1;
	public static short s_nextActorIndex = 0;

	public static Color s_friendlyPlayerColor = new Color(0f, 1f, 0f, 1f);
	public static Color s_friendlyDamagedHealthBarColor = new Color(0.5f, 0.9f, 0.5f, 1f);
	public static Color s_hostilePlayerColor = new Color(1f, 0f, 0f, 1f);
	public static Color s_hostileDamagedHealthBarColor = new Color(1f, 0.6f, 0.6f, 1f);
	public static Color s_neutralPlayerColor = new Color(0f, 1f, 1f, 1f);
	public static Color s_teamAColor = new Color(0.2f, 0.2f, 1f, 1f);
	public static Color s_teamBColor = new Color(1f, 0.5f, 0f, 1f);

	internal static float s_visibleTimeAfterHit = 1f;
	internal static float s_visibleTimeAfterMovementHit = 0.3f;

	private JointPopupProperty m_nameplateJoint;

	internal const float MAX_HEIGHT = 2.5f;

	//private const string c_hudIconPathRoot = "Assets/UI/Textures/AllStars/Resources/CharacterIcons/NEW/";  // rogues?
	//private const string c_hudIconPathPrefix = "CharacterIcons/NEW/";  // rogues?
	//private const string c_hudIconPathExtension = ".png";  // rogues?
	//private const string c_screenIndicatorPathRoot = "Assets/UI/Textures/Resources/CharacterIcons/";  // rogues?
	//private const string c_screenIndicatorPathPrefix = "CharacterIcons/";  // rogues?
	//private const string c_screenIndicatorPathExtension = ".png";  // rogues?

	[AssetFileSelector("Assets/UI/Textures/Resources/CharacterIcons/", "CharacterIcons/", ".png")]
	[Header("-- Icon: Portrait, OffscreenIndicator, Team Panel --")]
	public string m_aliveHUDIconResourceString;

	//[AssetFileSelector("Assets/UI/Textures/AllStars/Resources/CharacterIcons/NEW/", "CharacterIcons/NEW/", ".png")]
	//public string m_selectedHUDIconResourceString;  // rogues?

	[AssetFileSelector("Assets/UI/Textures/Resources/CharacterIcons/", "CharacterIcons/", ".png")]
	public string m_deadHUDIconResourceString;
	[Header("-- Icon: Last Known Position Indicator --")]
	[AssetFileSelector("Assets/UI/Textures/Resources/CharacterIcons/", "CharacterIcons/", ".png")]
	public string m_screenIndicatorIconResourceString;
	[AssetFileSelector("Assets/UI/Textures/Resources/CharacterIcons/", "CharacterIcons/", ".png")]
	public string m_screenIndicatorBWIconResourceString;

	public ActorDataDelegate m_onResolvedHitPoints;

	//public List<GearStatBlock> m_initialGearStatData = new List<GearStatBlock>();  // rogues

	internal bool m_callHandleOnSelectInUpdate;
	internal bool m_hideNameplate;
	internal float m_endVisibilityForHitTime = -10000f;
	internal bool m_needAddToTeam;

	//[SyncVar]
	private bool m_alwaysHideNameplate;

	private ActorBehavior m_actorBehavior;
	private ActorModelData m_actorModelData;
	private ActorModelData m_faceActorModelData;
	private ActorMovement m_actorMovement;

	//private ActorController m_actorInputController;  // rogues?

	private ActorStats m_actorStats;
	private ActorStatus m_actorStatus;

	//private ActorEffectStatus m_actorEffectStatus;  // rogues?

	private ActorTargeting m_actorTargeting;
	private ActorTurnSM m_actorTurnSM;
	private ActorCover m_actorCover;
	private ActorVFX m_actorVFX;
	private TimeBank m_timeBank;
	private ActorAdditionalVisionProviders m_additionalVisionProvider;
	private AbilityData m_abilityData;
	private ItemData m_itemData;
	private PassiveData m_passiveData;
	private CombatText m_combatText;
	private ActorTag m_actorTags;
	private FreelancerStats m_freelancerStats;

	//private SkinnedMeshRenderer[] m_characterMesh;  // rogues?
	//private Script m_script;  // rogues?
	//public List<ActorData> currentTargets = new List<ActorData>();  // rogues?
	//private EquipmentStats m_equipmentStats;  // rogues?

	[HideInInspector]
	public PrefabResourceLink m_actorSkinPrefabLink;
	//[SyncVar]
	[HideInInspector]
	public CharacterVisualInfo m_visualInfo;
	//[SyncVar]
	[HideInInspector]
	public CharacterAbilityVfxSwapInfo m_abilityVfxSwapInfo;

	private bool m_setTeam;
	//[SyncVar]
	private Team m_team;
	private EasedQuaternionNoAccel m_targetRotation = new EasedQuaternionNoAccel(Quaternion.identity);
	private bool m_shouldUpdateLastVisibleToClientThisFrame = true;

	private float m_lastIsVisibleToClientTime;
	private bool m_isVisibleToClientCache;
	//[SyncVar]
	private int m_lastVisibleTurnToClient;
	private BoardSquare m_clientLastKnownPosSquare;
	private BoardSquare m_serverLastKnownPosSquare;

	//[SyncVar]
	//private int m_serverLastKnownPosX;  // rogues? since they are SyncVars and not present in serialize, yes
	//[SyncVar]
	//private int m_serverLastKnownPosY;  // rogues?
	//[SyncVar(hook = "OnCurrentMovementFlagsUpdated")]
	//private byte m_currentMovementFlags;  // rogues?
	//[SyncVar]
	//private sbyte m_queuedChaseTargetActorIndex;  // rogues? reactor seems to use m_queuedChaseTarget

	private bool m_addedToUI;

	[HideInInspector]
	public CharacterModInfo m_selectedMods;

	//[SyncVar]
	//[HideInInspector]
	//public float m_alertDist = 2f;  // rogues
	//private bool m_alerted;  // rogues
	//[SyncVar]
	//private bool m_suspend;  // rogues
	//[SyncVar]
	//[HideInInspector]
	//public CharacterGearInfo m_selectedGear;  // rogues

	[HideInInspector]
	public CharacterAbilityVfxSwapInfo m_selectedAbilityVfxSwaps;
	//[SyncVar]
	[HideInInspector]
	public CharacterCardInfo m_selectedCards;
	[HideInInspector]
	public List<int> m_availableTauntIDs;

	//[SyncVar]
	internal string m_displayName = "Connecting Player";
	//[SyncVar(hook = "OnActorIndexUpdated")]
	private int m_actorIndex = s_invalidActorIndex;
	//[SyncVar]
	private bool m_showInGameHud = true;

	[Header("-- Stats --")]
	public int m_maxHitPoints = 100;
	public int m_hitPointRegen;
	[Space(5f)]
	public int m_maxTechPoints = 100;
	public int m_techPointRegen = 10;
	public int m_techPointsOnSpawn = 100;
	public int m_techPointsOnRespawn = 100;

	//[Space(5f)]  // rogues
	//public int m_outOfCombatShieldMax;
	//[Separator("Pve Stats", true)]
	//public int m_basePower = 100;
	//public int m_baseDefense;
	//public int m_baseAccuracy;
	//public int m_baseDamageMultiplier = 1;
	//public int m_baseIncomingHealingAdjustment = 1;
	//public int m_baseGlanceOffense = 5;
	//public int m_baseGlanceDefense;
	//public int m_baseCritOffense = 5;
	//public int m_baseCritDefense;
	//public int m_baseDodgeOffense = 5;
	//public int m_baseDodgeDefense;
	//public int m_baseBlockOffense = 5;
	//public int m_baseBlockDefense;
	//public int m_baseStrength;
	//public int m_baseExpertise;
	//[Header("-- Accuracy Adjust --")]
	//public ProximityAccuracyAdjustData m_proximityAccuracyAdjust;

	[Space(5f)]
	public float m_maxHorizontalMovement = 8f;
	public float m_postAbilityHorizontalMovement = 5f;
	public int m_maxVerticalUpwardMovement = 1;
	public int m_maxVerticalDownwardMovement = 2;
	public float m_sightRange = 10f;
	public float m_runSpeed = 8f;
	public float m_vaultSpeed = 4f;

	//public float m_walkSpeed = 2.5f;  // rogues?

	[Tooltip("The speed the actor travels when being knocked back by any ability.")]
	public float m_knockbackSpeed = 8f;

	//public bool m_grantCover;  // rogues?

	[Header("-- Audio Events --")]
	[AudioEvent(false)]
	public string m_onDeathAudioEvent = "";

	[Header("-- Additional Network Objects to call Register Prefab on")]
	public List<GameObject> m_additionalNetworkObjectsToRegister;

	// added in rogues
	//private int m_cachedMaxHitPoints;  // rogues?

	//[SyncVar]
	private int m_hitPoints = 1;
	//[SyncVar]
	private int _unresolvedDamage;
	//[SyncVar]
	private int _unresolvedHealing;
	//[SyncVar]
	private int _unresolvedTechPointGain;
	//[SyncVar]
	private int _unresolvedTechPointLoss;
	//[SyncVar]
	private int m_serverExpectedHoTTotal;
	//[SyncVar]
	private int m_serverExpectedHoTThisTurn;
	//[SyncVar]
	private int m_techPoints;
	//[SyncVar]
	private int m_reservedTechPoints;
	private bool m_ignoreForEnergyForHit;
	//[SyncVar]
	private bool m_ignoreFromAbilityHits;
	//[SyncVar]
	private int m_absorbPoints;
	//[SyncVar]
	private int m_mechanicPoints;
	//[SyncVar(hook = "OnSpawnerIdUpdated")]
	private int m_spawnerId;
	private Vector3 m_facingDirAfterMovement;
	private GameEventManager.EventType m_serverMovementWaitForEvent;
	private BoardSquare m_serverMovementDestination;
	private BoardSquarePathInfo m_serverMovementPath;
	private bool m_disappearingAfterCurrentMovement;

	// added in rogues
#if SERVER
	private BoardSquare m_serverTrueBoardSquare;  // server?
#endif

	private BoardSquare m_clientCurrentBoardSquare;
	private BoardSquare m_mostRecentDeathSquare;
	private ActorTeamSensitiveData m_teamSensitiveData_friendly;
	private ActorTeamSensitiveData m_teamSensitiveData_hostile;
	private BoardSquare m_trueMoveFromBoardSquare;
	private BoardSquare m_serverInitialMoveStartSquare;

	// added in rogues
	//[SyncVar]
	//[HideInInspector]
	//private float m_remainingHorizontalMovement;  // server
	// added in rogues
	//[SyncVar]
	//[HideInInspector]
	//private float m_remainingMovementWithQueuedAbility;  // server

	private bool m_internalQueuedMovementAllowsAbility;
	private bool m_queuedMovementRequest;
	private bool m_queuedChaseRequest;
	private ActorData m_queuedChaseTarget;
	private bool m_knockbackMoveStarted;

	// added in rogues
	//[SyncVar]
	//private int m_lastDeathTurn;  // server

	//[SyncVar]
	private int m_lastSpawnTurn = -1;
	//[SyncVar]
	private int m_nextRespawnTurn = -1;
	private List<BoardSquare> m_trueRespawnSquares = new List<BoardSquare>();
	private BoardSquare m_trueRespawnPositionSquare;
	private GameObject m_respawnPositionFlare;
	private BoardSquare m_respawnFlareVfxSquare;
	private bool m_respawnFlareForSameTeam;

	//[SyncVar]
	[HideInInspector]
	private bool m_hasBotController;

	//[SyncVar]
	//private int m_turnPriority;  // rogues?

	private bool m_currentlyVisibleForAbilityCast;
	private bool m_movedForEvade;
	private bool m_serverSuppressInvisibility;

	//[SyncVar]
	//private bool m_visibleTillEndOfPhase;  // rogues?

	private List<ActorData> m_lineOfSightVisibleExceptions = new List<ActorData>();

	//private SyncListInt m_lineOfSightVisibleExceptionActorIndexes = new SyncListInt();  // rogues?

	// added in rogues
	private BoardSquare m_squareAtPhaseStart;  // rogues?
	// added in rogues
	private BoardSquare m_squareAtResolveStart;  // rogues?
	// added in rogues
	private BoardSquare m_squareRequestedForMovementMetrics;  // rogues?

	private SerializeHelper m_serializeHelper;
	private uint m_debugSerializeSizeBeforeVisualInfo;
	private uint m_debugSerializeSizeBeforeSpawnSquares;
	private Rigidbody m_cachedHipJoint;
	private bool m_outOfCombat;
	private List<IForceActorOutlineChecker> m_forceShowOutlineCheckers;
	private bool m_wasUpdatingForConfirmedTargeting;
	private bool m_showingTargetingNumAtFullAlpha;

	private const int c_debugCmdHealthFlag = 0;
	private const int c_debugCmdEnergyFlag = 1;

	private BoardSquare m_initialSpawnSquare;
	private CharacterResourceLink m_characterResourceLink;


	private static int kCmdCmdSetPausedForDebugging = 1605310550;
	private static int kCmdCmdSetResolutionSingleStepping = -1306898411;
	private static int kCmdCmdSetResolutionSingleSteppingAdvance = -1612013907;
	private static int kCmdCmdSetDebugToggleParam = -77600279;
	private static int kCmdCmdDebugReslotCards = -967932322;
	private static int kCmdCmdDebugSetAbilityMod = 1885146022;
	private static int kCmdCmdDebugReplaceWithBot = -1932690655;
	private static int kCmdCmdDebugSetHealthOrEnergy = 946344949;
	private static int kRpcRpcOnHitPointsResolved = 189834458;
	private static int kRpcRpcCombatText = -1860175530;
	private static int kRpcRpcApplyAbilityModById = -1097840381;
	private static int kRpcRpcMarkForRecalculateClientVisibility = -701731415;
	private static int kRpcRpcForceLeaveGame = -1193160397;

	// rogues?
	//private void OnPlayerIndexUpdated(int actorIndex)
	//{
	//	if (actorIndex != s_invalidActorIndex && PlayerIndex != -1)
	//	{
	//		InitModel();
	//	}
	//}

	internal static int Layer { get; private set; }

	internal static int Layer_Mask { get; private set; }

	//public Script script
	//{
	//	get
	//	{
	//		return m_script;
	//	}
	//}

	public bool ForceDisplayTargetHighlight { get; set; }


	internal Vector3 PreviousBoardSquarePosition { get; private set; }

	//private void OnCurrentMovementFlagsUpdated(byte currentMovementFlags)  // rogues?
	//{
	//	bool flag;
	//	bool flag2;
	//	ServerClientUtils.GetBoolsFromBitfield(currentMovementFlags, out m_internalQueuedMovementAllowsAbility, out flag, out flag2, out m_alerted);
	//	if (m_queuedMovementRequest != flag)
	//	{
	//		m_queuedMovementRequest = flag;
	//		m_actorMovement.UpdateSquaresCanMoveTo();
	//	}
	//	if (m_queuedChaseRequest != flag2)
	//	{
	//		m_queuedChaseRequest = flag2;
	//		m_actorMovement.UpdateSquaresCanMoveTo();
	//	}
	//}

	//public bool Alerted  // rogues
	//{
	//	get
	//	{
	//		return m_alerted;
	//	}
	//	set
	//	{
	//		if (m_alerted != value)
	//		{
	//			m_alerted = value;
	//			if (NetworkServer.active && m_alerted && SpawnerId >= 0 && NPCCoordinator.Get() != null)
	//			{
	//				NPCCoordinator.Get().OnActorAlerted(this);
	//			}
	//		}
	//	}
	//}

	public BoardSquare ClientLastKnownPosSquare
	{
		get
		{
			return m_clientLastKnownPosSquare;
		}
		private set
		{
			if (m_clientLastKnownPosSquare != value)
			{
				if (ActorDebugUtils.Get() != null
					&& ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.LastKnownPosition, false))
				{
					Debug.LogWarning(string.Concat(new string[]
					{
						DebugNameString(),
						"----Setting ClientLastKnownPosSquare from ",
						m_clientLastKnownPosSquare ? m_clientLastKnownPosSquare.ToString() : "null",
						" to ",
						value ? value.ToString() : "null"
					}));
				}
				m_clientLastKnownPosSquare = value;
			}
			m_shouldUpdateLastVisibleToClientThisFrame = false;
		}
	}

	// overhauled in rogues -- m_serverLastKnownPosSquare split into m_serverLastKnownPosX & Y
	public BoardSquare ServerLastKnownPosSquare
	{
		get
		{
			return m_serverLastKnownPosSquare;
		}
		set
		{
			if (m_serverLastKnownPosSquare == value)
			{
				return;
			}
			if (ActorDebugUtils.Get() != null && ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.LastKnownPosition, false))
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					DebugNameString(),
					"=====ServerLastKnownPosSquare from ",
					m_serverLastKnownPosSquare ? m_serverLastKnownPosSquare.ToString() : "null",
					" to ",
					value ? value.ToString() : "null"
				}));
			}
			
			m_serverLastKnownPosSquare = value;
		}
	}

	internal string DisplayName => m_displayName;

	public int ActorIndex
	{
		get
		{
			return m_actorIndex;
		}
		set
		{
			if (m_actorIndex != value)
			{
				// reactor
				m_actorIndex = value;
				// rogues
				//Networkm_actorIndex = value; 
			}
		}
	}

	// rogues or server???
	//private void OnActorIndexUpdated(int actorIndex) 
	//{
	//	if (actorIndex != s_invalidActorIndex && PlayerIndex != -1)
	//	{
	//		InitModel();
	//	}
	//}

	public bool ShowInGameGUI
	{
		get
		{
			return m_showInGameHud;
		}
		set
		{
			// reactor
			m_showInGameHud = value;
			// rogues
			//Networkm_showInGameHud = value;  
		}
	}

	internal int HitPoints
	{
		get
		{
			return m_hitPoints;
		}
		private set
		{
			if (MatchLogger.Get() != null)
			{
				MatchLogger.Get().Log(this + " HitPoints.set " + value + ", old: " + HitPoints);
			}
			bool wasAlive = m_hitPoints > 0;
			//m_cachedMaxHitPoints = GetMaxHitPoints();  // rogues
			if (NetworkServer.active)
			{
				// reactor
				m_hitPoints = Mathf.Clamp(value, 0, GetMaxHitPoints());
				// rogues
				//Networkm_hitPoints = Mathf.Clamp(value, 0, m_cachedMaxHitPoints);
			}
			else
			{
				// reactor
				m_hitPoints = value;
				/// rogues
				//Networkm_hitPoints = value;
			}
			int currentTurn = 0;
			if (GameFlowData.Get() != null)
			{
				currentTurn = GameFlowData.Get().CurrentTurn;
			}
			if (wasAlive && m_hitPoints == 0)
			{
				if (GameFlowData.Get() != null)
				{
					LastDeathTurn = GameFlowData.Get().CurrentTurn;
				}
				LastDeathPosition = gameObject.transform.position;
				NextRespawnTurn = -1;
				FogOfWar.CalculateFogOfWarForTeam(GetTeam());
				if (GetCurrentBoardSquare() != null)
				{
					SetMostRecentDeathSquare(GetCurrentBoardSquare());
				}
#if SERVER
				if (GetActorBehavior() != null) // added in rogues
				{
					GetActorBehavior().TrackMovementLostForDeadActor();
				}
#endif
				gameObject.SendMessage("OnDeath");
				if (GameFlowData.Get() != null)
				{
					GameFlowData.Get().NotifyOnActorDeath(this);
				}
				UnoccupyCurrentBoardSquare();
				SetCurrentBoardSquare(null);
				ClientLastKnownPosSquare = null;
#if SERVER
				if (NetworkServer.active)  // was ELSE unconditionally in reactor
				{
					SetServerLastKnownPosSquare(null, "Actor HitPoints Death");
				}
				else
				{
					ServerLastKnownPosSquare = null;
				}
#else
				ServerLastKnownPosSquare = null;
#endif
			}
			else if (!wasAlive && m_hitPoints > 0 && LastDeathTurn > 0)
			{
				gameObject.SendMessage("OnRespawn");
				// reactor
				m_lastVisibleTurnToClient = 0;
				// rogues
				//Networkm_lastVisibleTurnToClient = 0;
				if (NetworkServer.active)
				{
					if (m_teamSensitiveData_friendly != null)
					{
						m_teamSensitiveData_friendly.MarkAsRespawning();
					}
					if (m_teamSensitiveData_hostile != null)
					{
						m_teamSensitiveData_hostile.MarkAsRespawning();
					}
					// TODO LOW missing code in reactor? this statement is removed in rogues
					if (currentTurn > 0)
					{
						return;
					}
				}
			}
		}
	}

	internal int UnresolvedDamage
	{
		get
		{
			return _unresolvedDamage;
		}
		set
		{
			if (MatchLogger.Get() != null)
			{
				MatchLogger.Get().Log(string.Concat(this, " UnresolvedDamage.set ", value, ", old: ", UnresolvedDamage));
			}
			if (_unresolvedDamage != value)
			{
				_unresolvedDamage = value;
				//Network_unresolvedDamage = value;  // rogues
				ClientUnresolvedDamage = 0;
			}
		}
	}

	internal int UnresolvedHealing
	{
		get
		{
			return _unresolvedHealing;
		}
		set
		{
			if (MatchLogger.Get() != null)
			{
				MatchLogger.Get().Log(string.Concat(this, " UnresolvedHealing.set ", value, ", old: ", UnresolvedHealing));
			}
			if (_unresolvedHealing != value)
			{
				_unresolvedHealing = value;
				//Network_unresolvedHealing = value;  // rogues
				ClientUnresolvedHealing = 0;
			}
		}
	}

	internal int UnresolvedTechPointGain
	{
		get
		{
			return _unresolvedTechPointGain;
		}
		set
		{
			if (MatchLogger.Get() != null)
			{
				MatchLogger.Get().Log(string.Concat(this, " UnresolvedTechPointGain.set ", value, ", old: ", UnresolvedTechPointGain));
			}
			if (_unresolvedTechPointGain != value)
			{
				_unresolvedTechPointGain = value;
				//Network_unresolvedTechPointGain = value;  // rogues
				ClientUnresolvedTechPointGain = 0;
			}
		}
	}

	internal int UnresolvedTechPointLoss
	{
		get
		{
			return _unresolvedTechPointLoss;
		}
		set
		{
			if (MatchLogger.Get() != null)
			{
				MatchLogger.Get().Log(string.Concat(this, " UnresolvedTechPointLoss.set ", value, ", old: ", UnresolvedTechPointLoss));
			}
			if (_unresolvedTechPointLoss != value)
			{
				_unresolvedTechPointLoss = value;
				//Network_unresolvedTechPointLoss = value;  // rogues
				ClientUnresolvedTechPointLoss = 0;
			}
		}
	}

	internal int ExpectedHoTTotal
	{
		get
		{
			return m_serverExpectedHoTTotal;
		}
		set
		{
			if (m_serverExpectedHoTTotal != value)
			{
				// reactor
				m_serverExpectedHoTTotal = value;
				// rogues
				//Networkm_serverExpectedHoTTotal = value;
				ClientExpectedHoTTotalAdjust = 0;
			}
		}
	}

	internal int ExpectedHoTThisTurn
	{
		get
		{
			return m_serverExpectedHoTThisTurn;
		}
		set
		{
			if (m_serverExpectedHoTThisTurn != value)
			{
				// reactor
				m_serverExpectedHoTThisTurn = value;
				// rogues
				//Networkm_serverExpectedHoTThisTurn = value;
			}
		}
	}

	internal int ClientUnresolvedDamage { get; set; }
	internal int ClientUnresolvedHealing { get; set; }
	internal int ClientUnresolvedTechPointGain { get; set; }
	internal int ClientUnresolvedTechPointLoss { get; set; }
	internal int ClientUnresolvedAbsorb { get; set; }
	internal int ClientReservedTechPoints { get; set; }
	internal int ClientExpectedHoTTotalAdjust { get; set; }
	internal int ClientAppliedHoTThisTurn { get; set; }

	internal int TechPoints  // public in rogues
	{
		get
		{
			if (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("InfiniteTP"))
			{
				return GetMaxTechPoints();
			}
			return m_techPoints;
		}
		private set
		{
			if (NetworkServer.active)
			{
				// reactor
				m_techPoints = Mathf.Clamp(value, 0, GetMaxTechPoints());
				// rogues
				//Networkm_techPoints = Mathf.Clamp(value, 0, GetMaxTechPoints());  // server
			}
			else
			{
				// reactor
				m_techPoints = value;
				// rogues
				//Networkm_techPoints = value;  // server
			}
		}
	}

	public int ReservedTechPoints
	{
		get
		{
			return m_reservedTechPoints;
		}
		set
		{
			if (m_reservedTechPoints != value)
			{
				// reactor
				m_reservedTechPoints = value;
				// rogues
				//Networkm_reservedTechPoints = value;
				ClientReservedTechPoints = 0;
			}
		}
	}

	public bool IgnoreForEnergyOnHit
	{
		get
		{
			return m_ignoreForEnergyForHit;
		}
		set
		{
			if (NetworkServer.active)
			{
				m_ignoreForEnergyForHit = value;
			}
		}
	}

	public bool IgnoreForAbilityHits
	{
		get
		{
			return m_ignoreFromAbilityHits;
		}
		set
		{
			if (NetworkServer.active)
			{
				// reactor
				m_ignoreFromAbilityHits = value;
				// rogues
				//Networkm_ignoreFromAbilityHits = value;
			}
		}
	}

	internal int AbsorbPoints  // public in rogues
	{
		get
		{
			return m_absorbPoints;
		}
		private set
		{
			if (m_absorbPoints != value)
			{
				// server
				if (NetworkServer.active)
				{
					// reactor
					m_absorbPoints = Mathf.Max(value, 0);
					// rogues
					//Networkm_absorbPoints = Mathf.Max(value, 0);
				}
				else
				{
					// reactor
					m_absorbPoints = Mathf.Max(value, 0);
					// rogues
					//Networkm_absorbPoints = Mathf.Max(value, 0);
				}
				ClientUnresolvedAbsorb = 0;
			}
			// NOTE ROGUES unconditionally reset client unresolved absord
			//ClientUnresolvedAbsorb = 0;
		}
	}

	internal int MechanicPoints
	{
		get
		{
			return m_mechanicPoints;
		}
		set
		{
			if (m_mechanicPoints != value)
			{
				// reactor
				m_mechanicPoints = Mathf.Max(value, 0);
				// rogues
				//Networkm_mechanicPoints = Mathf.Max(value, 0);
			}
		}
	}

	internal int SpawnerId
	{
		get
		{
			return m_spawnerId;
		}
		set
		{
			if (m_spawnerId != value)
			{
				// reactor
				m_spawnerId = value;
				// rogues
				//Networkm_spawnerId = value;
			}
		}
	}

	public bool DisappearingAfterCurrentMovement => m_disappearingAfterCurrentMovement;

	// server?
	public BoardSquare CurrentBoardSquare
	{
		get
		{
#if SERVER
			if (NetworkServer.active)
			{
				return m_serverTrueBoardSquare;
			}
#endif
			return m_clientCurrentBoardSquare;
		}
	}
	//public BoardSquare CurrentBoardSquare => m_clientCurrentBoardSquare;  // reactor

	// just m_teamSensitiveData_friendly in rogues
	public ActorTeamSensitiveData TeamSensitiveData_authority
	{
		get
		{
			if (m_teamSensitiveData_friendly != null)
			{
				return m_teamSensitiveData_friendly;
			}
			return m_teamSensitiveData_hostile;
		}
	}
	
	// custom
	public ActorTeamSensitiveData TeamSensitiveData_hostile => m_teamSensitiveData_hostile;

	public BoardSquare MoveFromBoardSquare
	{
		get
		{
			if (NetworkServer.active)
			{
				return m_trueMoveFromBoardSquare;
			}
			if (m_teamSensitiveData_friendly != null)
			{
				return m_teamSensitiveData_friendly.MoveFromBoardSquare;
			}
			return CurrentBoardSquare;
		}
		set
		{
			if (NetworkServer.active && m_trueMoveFromBoardSquare != value)
			{
				m_trueMoveFromBoardSquare = value;
				if (m_teamSensitiveData_friendly != null)
				{
					m_teamSensitiveData_friendly.MoveFromBoardSquare = value;
				}
			}
		}
	}

	public BoardSquare InitialMoveStartSquare
	{
		get
		{
			if (NetworkServer.active)
			{
				return m_serverInitialMoveStartSquare;
			}
			if (m_teamSensitiveData_friendly != null)
			{
				return m_teamSensitiveData_friendly.InitialMoveStartSquare;
			}
			return CurrentBoardSquare;
		}
		set
		{
			if (NetworkServer.active && m_serverInitialMoveStartSquare != value)
			{
				m_serverInitialMoveStartSquare = value;
				if (GetActorMovement() != null)
				{
					GetActorMovement().UpdateSquaresCanMoveTo();
				}
				if (m_teamSensitiveData_friendly != null)
				{
					m_teamSensitiveData_friendly.InitialMoveStartSquare = value;
				}
			}
		}
	}

	// reactor
	public float RemainingHorizontalMovement { get; set; }
	// rogues
	//public float RemainingHorizontalMovement
	//{
	//	get
	//	{
	//		return m_remainingHorizontalMovement;
	//	}
	//	set
	//	{
	//		Networkm_remainingHorizontalMovement = value;
	//	}
	//}

	// reactor
	public float RemainingMovementWithQueuedAbility { get; set; }
	// rogues

	//public float RemainingMovementWithQueuedAbility
	//{
	//	get
	//	{
	//		return m_remainingMovementWithQueuedAbility;
	//	}
	//	set
	//	{
	//		Networkm_remainingMovementWithQueuedAbility = value;
	//	}
	//}

	public bool QueuedMovementAllowsAbility
	{
		get
		{
			return m_internalQueuedMovementAllowsAbility;
		}
		set
		{
			if (value != m_internalQueuedMovementAllowsAbility)
			{
				m_internalQueuedMovementAllowsAbility = value;
			}
		}
	}

	public bool KnockbackMoveStarted
	{
		get
		{
			return m_knockbackMoveStarted;
		}
		set
		{
			m_knockbackMoveStarted = value;
		}
	}

	internal Vector3 LastDeathPosition { get; private set; }

	// reactor
	internal int LastDeathTurn { get; private set; }
	// rogues
	//internal int LastDeathTurn
	//{
	//	get
	//	{
	//		return m_lastDeathTurn;
	//	}
	//	private set
	//	{
	//		Networkm_lastDeathTurn = value;
	//	}
	//}

	// TODO rogues?
#if SERVER
	public int LastSpawnTurn
	{
		get
		{
			return m_lastSpawnTurn;
		}
	}
#endif

	public int NextRespawnTurn
	{
		get
		{
			return m_nextRespawnTurn;
		}
		set
		{
			// reactor
			m_nextRespawnTurn = Mathf.Max(value, LastDeathTurn + 1);
			// rogues
			//Networkm_nextRespawnTurn = value;
		}
	}

	public List<BoardSquare> respawnSquares
	{
		get
		{
			if (NetworkServer.active)
			{
				return m_trueRespawnSquares;
			}
			if (m_teamSensitiveData_friendly != null)
			{
				return m_teamSensitiveData_friendly.RespawnAvailableSquares;
			}
			return new List<BoardSquare>();
		}
		set
		{
			if (NetworkServer.active)
			{
				m_trueRespawnSquares = value;
				if (m_teamSensitiveData_friendly != null)
				{
					m_teamSensitiveData_friendly.RespawnAvailableSquares = value;
				}
			}
		}
	}

	public BoardSquare RespawnPickedPositionSquare
	{
		get
		{
			if (NetworkServer.active)
			{
				return m_trueRespawnPositionSquare;
			}
			if (m_teamSensitiveData_friendly != null)
			{
				return m_teamSensitiveData_friendly.RespawnPickedSquare;
			}

			// this IF is removed in rogues
			if (m_teamSensitiveData_hostile != null)
			{
				return m_teamSensitiveData_hostile.RespawnPickedSquare;
			}
			return null;
		}
		//set  // rogues
		//{
		//	if (NetworkServer.active)
		//	{
		//		m_trueRespawnPositionSquare = value;
		//		if (m_teamSensitiveData_friendly != null)
		//		{
		//			if (GameFlowData.Get().IsInDecisionState() || GameFlowData.Get().CurrentTurn == NextRespawnTurn)
		//			{
		//				m_teamSensitiveData_friendly.RespawnPickedSquare = value;
		//			}
		//			else
		//			{
		//				m_teamSensitiveData_friendly.RespawnPickedSquare = null;
		//			}
		//		}
		//		ShowRespawnFlare(m_trueRespawnPositionSquare, IsActorInvisibleForRespawn());
		//	}
		//}
		set
		{
			if (!NetworkServer.active)
			{
				return;
			}
			m_trueRespawnPositionSquare = value;
			if (m_teamSensitiveData_friendly != null)
			{
				if (!GameFlowData.Get().IsInDecisionState() && GameFlowData.Get().CurrentTurn != NextRespawnTurn)
				{
					m_teamSensitiveData_friendly.RespawnPickedSquare = null;
				}
				else
				{
					m_teamSensitiveData_friendly.RespawnPickedSquare = value;
				}
			}
			if (m_teamSensitiveData_hostile != null)
			{
				if (GameFlowData.Get().CurrentTurn == NextRespawnTurn)
				{
					if (IsRespawnLocationVisibleToEnemy(value))
					{
						m_teamSensitiveData_hostile.RespawnPickedSquare = value;
					}
					else
					{
						m_teamSensitiveData_hostile.RespawnPickedSquare = null;
					}
				}
				else
				{
					m_teamSensitiveData_hostile.RespawnPickedSquare = null;
				}
			}
		}
	}

	public bool HasBotController
	{
		get
		{
			return m_hasBotController;
		}
		set
		{
			// reactor
			m_hasBotController = value;
			// rogues
			//Networkm_hasBotController = value;
		}
	}

	//public int TurnPriority  // rogues
	//{
	//	get
	//	{
	//		return m_turnPriority;
	//	}
	//	set
	//	{
	//		Networkm_turnPriority = value;
	//	}
	//}

	public bool VisibleTillEndOfPhase { get; set; }

	//public bool VisibleTillEndOfPhase  // rogues
	//{
	//	get
	//	{
	//		return m_visibleTillEndOfPhase;
	//	}
	//	set
	//	{
	//		Networkm_visibleTillEndOfPhase = value;
	//	}
	//}

	// removed in rogues
	public bool CurrentlyVisibleForAbilityCast
	{
		get
		{
			return m_currentlyVisibleForAbilityCast;
		}
		set
		{
			if (m_currentlyVisibleForAbilityCast != value)
			{
				if (ActorDebugUtils.Get() != null && ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.LastKnownPosition, false))
				{
					Debug.LogWarning(DebugNameString() + "Setting visible for ability cast to " + value);
				}
				m_currentlyVisibleForAbilityCast = value;
			}
		}
	}

	// removed in rogues
	public bool MovedForEvade
	{
		get
		{
			return m_movedForEvade;
		}
		set
		{
			if (m_movedForEvade != value)
			{
				m_movedForEvade = value;
			}
		}
	}

	// removed in rogues
	public bool ServerSuppressInvisibility
	{
		get
		{
			return m_serverSuppressInvisibility;
		}
		set
		{
			if (m_serverSuppressInvisibility != value)
			{
				m_serverSuppressInvisibility = value;
			}
		}
	}

	internal ReadOnlyCollection<ActorData> LineOfSightVisibleExceptions => m_lineOfSightVisibleExceptions.AsReadOnly();

	internal ReadOnlyCollection<BoardSquare> LineOfSightVisibleExceptionSquares
	{
		get
		{
			List<BoardSquare> list = new List<BoardSquare>(m_lineOfSightVisibleExceptions.Count);
			foreach (ActorData lineOfSightVisibleException in m_lineOfSightVisibleExceptions)
			{
				list.Add(lineOfSightVisibleException.GetCurrentBoardSquare());
			}
			return list.AsReadOnly();
		}
	}

	public static bool no_op_return_false_unused => false;

	// removed in rogues
	public bool OutOfCombat
	{
		get
		{
			return m_outOfCombat;
		}
		private set
		{
			m_outOfCombat = value;
		}
	}

	public BoardSquare InitialSpawnSquare => m_initialSpawnSquare;

#if SERVER
	// added in rogues
	public event Action<ActorData, ActorHitResults> OnKnockbackHitExecutedDelegate;
#endif

	public event Action OnTurnStartDelegates;
	public event Action<UnityEngine.Object, GameObject> OnAnimationEventDelegates;
	public event Action<Ability> OnSelectedAbilityChangedDelegates;
	public event Action OnClientQueuedActionChangedDelegates;

	public ActorData()
	{
		//InitSyncObject(m_lineOfSightVisibleExceptionActorIndexes);  // rogues
		m_serializeHelper = new SerializeHelper();
		m_forceShowOutlineCheckers = new List<IForceActorOutlineChecker>();
	}

	static ActorData()
	{
		RegisterCommandDelegate(typeof(ActorData), kCmdCmdSetPausedForDebugging, InvokeCmdCmdSetPausedForDebugging);
		RegisterCommandDelegate(typeof(ActorData), kCmdCmdSetResolutionSingleStepping, InvokeCmdCmdSetResolutionSingleStepping);
		RegisterCommandDelegate(typeof(ActorData), kCmdCmdSetResolutionSingleSteppingAdvance, InvokeCmdCmdSetResolutionSingleSteppingAdvance);
		RegisterCommandDelegate(typeof(ActorData), kCmdCmdSetDebugToggleParam, InvokeCmdCmdSetDebugToggleParam);
		RegisterCommandDelegate(typeof(ActorData), kCmdCmdDebugReslotCards, InvokeCmdCmdDebugReslotCards);
		RegisterCommandDelegate(typeof(ActorData), kCmdCmdDebugSetAbilityMod, InvokeCmdCmdDebugSetAbilityMod);
		RegisterCommandDelegate(typeof(ActorData), kCmdCmdDebugReplaceWithBot, InvokeCmdCmdDebugReplaceWithBot);
		RegisterCommandDelegate(typeof(ActorData), kCmdCmdDebugSetHealthOrEnergy, InvokeCmdCmdDebugSetHealthOrEnergy);
		RegisterRpcDelegate(typeof(ActorData), kRpcRpcOnHitPointsResolved, InvokeRpcRpcOnHitPointsResolved);
		RegisterRpcDelegate(typeof(ActorData), kRpcRpcCombatText, InvokeRpcRpcCombatText);
		RegisterRpcDelegate(typeof(ActorData), kRpcRpcApplyAbilityModById, InvokeRpcRpcApplyAbilityModById);
		RegisterRpcDelegate(typeof(ActorData), kRpcRpcMarkForRecalculateClientVisibility, InvokeRpcRpcMarkForRecalculateClientVisibility);
		RegisterRpcDelegate(typeof(ActorData), kRpcRpcForceLeaveGame, InvokeRpcRpcForceLeaveGame);
		NetworkCRC.RegisterBehaviour("ActorData", 0);

		// rogues -- added RpcApplyGearById
		//NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), "CmdSetPausedForDebugging", new NetworkBehaviour.CmdDelegate(ActorData.InvokeCmdCmdSetPausedForDebugging));
		//NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), "CmdSetResolutionSingleStepping", new NetworkBehaviour.CmdDelegate(ActorData.InvokeCmdCmdSetResolutionSingleStepping));
		//NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), "CmdSetResolutionSingleSteppingAdvance", new NetworkBehaviour.CmdDelegate(ActorData.InvokeCmdCmdSetResolutionSingleSteppingAdvance));
		//NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), "CmdSetDebugToggleParam", new NetworkBehaviour.CmdDelegate(ActorData.InvokeCmdCmdSetDebugToggleParam));
		//NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), "CmdDebugReslotCards", new NetworkBehaviour.CmdDelegate(ActorData.InvokeCmdCmdDebugReslotCards));
		//NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), "CmdDebugReplaceWithBot", new NetworkBehaviour.CmdDelegate(ActorData.InvokeCmdCmdDebugReplaceWithBot));
		//NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), "CmdDebugSetHealthOrEnergy", new NetworkBehaviour.CmdDelegate(ActorData.InvokeCmdCmdDebugSetHealthOrEnergy));
		//NetworkBehaviour.RegisterRpcDelegate(typeof(ActorData), "RpcOnHitPointsResolved", new NetworkBehaviour.CmdDelegate(ActorData.InvokeRpcRpcOnHitPointsResolved));
		//NetworkBehaviour.RegisterRpcDelegate(typeof(ActorData), "RpcOnTechPointsResolved", new NetworkBehaviour.CmdDelegate(ActorData.InvokeRpcRpcOnTechPointsResolved));
		//NetworkBehaviour.RegisterRpcDelegate(typeof(ActorData), "RpcCombatText", new NetworkBehaviour.CmdDelegate(ActorData.InvokeRpcRpcCombatText));
		//NetworkBehaviour.RegisterRpcDelegate(typeof(ActorData), "RpcApplyAbilityModById", new NetworkBehaviour.CmdDelegate(ActorData.InvokeRpcRpcApplyAbilityModById));
		//NetworkBehaviour.RegisterRpcDelegate(typeof(ActorData), "RpcApplyGearById", new NetworkBehaviour.CmdDelegate(ActorData.InvokeRpcRpcApplyGearById));
		//NetworkBehaviour.RegisterRpcDelegate(typeof(ActorData), "RpcMarkForRecalculateClientVisibility", new NetworkBehaviour.CmdDelegate(ActorData.InvokeRpcRpcMarkForRecalculateClientVisibility));
		//NetworkBehaviour.RegisterRpcDelegate(typeof(ActorData), "RpcForceLeaveGame", new NetworkBehaviour.CmdDelegate(ActorData.InvokeRpcRpcForceLeaveGame));
	}

	public int GetLastVisibleTurnToClient()
	{
		return m_lastVisibleTurnToClient;
	}

	public Vector3 GetClientLastKnownPosVec()
	{
		if (ClientLastKnownPosSquare)
		{
			return ClientLastKnownPosSquare.transform.position;
		}
		return Vector3.zero;
	}

	public Vector3 GetServerLastKnownPosVec()
	{
		if (ServerLastKnownPosSquare)
		{
			return ServerLastKnownPosSquare.transform.position;
		}
		return Vector3.zero;
	}

	public void ActorData_OnActorMoved(BoardSquare movementSquare, bool visibleToEnemies, bool updateLastKnownPos)
	{
		if (!NetworkClient.active)
		{
			return;
		}
		if (updateLastKnownPos)
		{
			ClientLastKnownPosSquare = movementSquare;
			// reactor
			m_lastVisibleTurnToClient = GameFlowData.Get().CurrentTurn;
			// rogues
			//Networkm_lastVisibleTurnToClient = GameFlowData.Get().CurrentTurn;
		}
		m_shouldUpdateLastVisibleToClientThisFrame = false;
	}

	public ActorBehavior GetActorBehavior()
	{
		return m_actorBehavior;
	}

	public ActorModelData GetActorModelData()
	{
		return m_actorModelData;
	}

	// removed in rogues
	internal ActorModelData GetFaceActorModelData()
	{
		return m_faceActorModelData;
	}

	public Renderer GetModelRenderer()
	{
		return m_actorModelData.GetModelRenderer();
	}

	public void EnableRendererAndUpdateVisibility()
	{
		if (m_actorModelData != null)
		{
			m_actorModelData.EnableRendererAndUpdateVisibility();
		}
	}

	internal ItemData GetItemData()
	{
		return m_itemData;
	}

	internal AbilityData GetAbilityData()
	{
		return m_abilityData;
	}

	internal ActorMovement GetActorMovement()
	{
		return m_actorMovement;
	}

	internal ActorStats GetActorStats()
	{
		return m_actorStats;
	}

	internal ActorStatus GetActorStatus()
	{
		return m_actorStatus;
	}

	//internal ActorEffectStatus GetActorEffectStatus()  // rogues
	//{
	//	return m_actorEffectStatus;
	//}

	internal ActorController GetActorController()
	{
		return GetComponent<ActorController>();
		//return m_actorInputController;  // rogues?
	}

	internal ActorTargeting GetActorTargeting()
	{
		return m_actorTargeting;
	}

	internal FreelancerStats GetFreelancerStats()
	{
		return m_freelancerStats;
	}

	internal NPCBrain GetNPCBrain()
	{
		if (GetActorController() != null)
		{
			foreach (NPCBrain npcbrain in GetComponents<NPCBrain>())
			{
				if (npcbrain.enabled)
				{
					return npcbrain;
				}
			}
		}
		return null;
	}

	internal ActorTurnSM GetActorTurnSM()
	{
		return m_actorTurnSM;
	}

	internal ActorCover GetActorCover()
	{
		return m_actorCover;
	}

	internal ActorVFX GetActorVFX()
	{
		return m_actorVFX;
	}

	internal TimeBank GetTimeBank()
	{
		return m_timeBank;
	}

	internal FogOfWar GetFogOfWar()
	{
		return PlayerData.GetFogOfWar();
	}

	internal ActorAdditionalVisionProviders GetAdditionalActorVisionProviders()
	{
		return m_additionalVisionProvider;
	}

	internal PassiveData GetPassiveData()
	{
		return m_passiveData;
	}

	//internal EquipmentStats GetEquipmentStats()  // rogues
	//{
	//	if (m_equipmentStats == null)
	//	{
	//		m_equipmentStats = base.GetComponent<EquipmentStats>();
	//	}
	//	return m_equipmentStats;
	//}

	//internal void InitEquipmentStats()  // rogues
	//{
	//}

	public string GetDisplayName()
	{
		if (HasBotController
			&& GetOriginalAccountId() == 0L
			&& m_characterType != CharacterType.None
			&& !GetPlayerDetails().m_botsMasqueradeAsHumans)
		{
			return StringUtil.TR_CharacterName(m_characterType.ToString());
		}
		if (m_displayName == "FT")
		{
			TricksterAfterImageNetworkBehaviour[] componentsInChildren = GameFlowData.Get().GetActorRoot().GetComponentsInChildren<TricksterAfterImageNetworkBehaviour>();
			if (componentsInChildren != null)
			{
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					for (int j = 0; j < componentsInChildren[i].m_afterImages.Count; j++)
					{
						int actorIndex = componentsInChildren[i].m_afterImages[j];
						if (GameFlowData.Get().FindActorByActorIndex(actorIndex) == this)
						{
							ActorData component = componentsInChildren[i].GetComponent<ActorData>();
							if (component.m_displayName != "FT")
							{
								return component.GetDisplayName();
							}
						}
					}
				}
			}
		}
		if (CollectTheCoins.Get() != null)
		{
			return $"{m_displayName} ({CollectTheCoins.Get().GetCoinsForActor_Client(this)}c)";
		}
		return m_displayName;
	}

	public void UpdateDisplayName(string newDisplayName)
	{
		// reactor
		m_displayName = newDisplayName;
		// rogues
		//Networkm_displayName = newDisplayName;
	}

	public Sprite GetAliveHUDIcon()
	{
		return Resources.Load<Sprite>(m_aliveHUDIconResourceString);
	}

	//public Sprite GetSelectedHUDIcon()  // rogues
	//{
	//	return Resources.Load<Sprite>(m_selectedHUDIconResourceString);
	//}

	public Sprite GetDeadHUDIcon()
	{
		return Resources.Load<Sprite>(m_deadHUDIconResourceString);
	}

	public Sprite GetScreenIndicatorIcon()
	{
		return Resources.Load<Sprite>(m_screenIndicatorIconResourceString);
	}

	public Sprite GetScreenIndicatorBWIcon()
	{
		return Resources.Load<Sprite>(m_screenIndicatorBWIconResourceString);
	}

	public string GetClassName()
	{
		return name.Replace("(Clone)", "");
	}

	// rogues
	//public float BasePowerDifficultyAdjustement { get; set; } = 1f;
	//public float BaseDefenseDifficultyAdjustement { get; set; } = 1f;
	//public float BaseAccuracyDifficultyAdjustement { get; set; } = 1f;
	//public float BaseDamageMultiplierDifficultyAdjustment { get; set; } = 1f;
	//public float BaseIncomingHealingAdjustmentDifficultyAdjustment { get; set; } = 1f;
	//public float BaseGlanceOffenseAdjustment { get; set; } = 1f;
	//public float BaseGlanceDefenseAdjustment { get; set; } = 1f;
	//public float BaseCritOffenseAdjustment { get; set; } = 1f;
	//public float BaseCritDefenseAdjustment { get; set; } = 1f;
	//public float BaseDodgeOffenseAdjustment { get; set; } = 1f;
	//public float BaseDodgeDefenseAdjustment { get; set; } = 1f;
	//public float BaseBlockOffenseAdjustment { get; set; } = 1f;
	//public float BaseBlockDefenseAdjustment { get; set; } = 1f;
	//public float BaseStrengthAdjustment { get; set; } = 1f;
	//public float BaseExpertiseAdjustment { get; set; } = 1f;

	public float GetAbilityMovementCost()
	{
		return m_maxHorizontalMovement - m_postAbilityHorizontalMovement;
	}

	public int GetMaxHitPoints()
	{
		int result = 1;
		if (m_actorStats != null)
		{
			result = m_actorStats.GetModifiedStatInt(StatType.MaxHitPoints);
		}
		// rogues
		//if (m_equipmentStats != null && m_itemData != null)
		//{
		//	num = Mathf.RoundToInt(m_equipmentStats.GetTotalStatValueForSlot(GearStatType.HealthAdjustment, (float)(num + GetBaseStatValue(GearStatType.HealthAdjustment)), -1, this));
		//}
		//if (num < 1)
		//{
		//	num = 1;
		//}
		return result;
	}

	public void OnMaxHitPointsChanged(int previousMax)
	{
		if (IsDead())
		{
			return;
		}
		float num = HitPoints / (float)previousMax;
		int maxHitPoints = GetMaxHitPoints();
		HitPoints = Mathf.RoundToInt(maxHitPoints * num);
	}

	//public void OnHealthAdjustmentChanged()  // rogues?
	//{
	//	if (m_cachedMaxHitPoints < 1)
	//	{
	//		m_cachedMaxHitPoints = HitPoints;
	//	}
	//	float num = (float)HitPoints / (float)m_cachedMaxHitPoints;
	//	int maxHitPoints = GetMaxHitPoints();
	//	HitPoints = Mathf.RoundToInt((float)maxHitPoints * num);
	//}

	public float GetHitPointPercent()
	{
		int maxHitPoints = GetMaxHitPoints();
		return HitPoints / (float)maxHitPoints;
	}

	public int GetHitPointRegen()
	{
		int num = 0;
		if (m_actorStats != null)
		{
			num = m_actorStats.GetModifiedStatInt(StatType.HitPointRegen);
			float modifiedStatFloat = m_actorStats.GetModifiedStatFloat(StatType.HitPointRegenPercentOfMax);
			float modifiedStatFloat2 = m_actorStats.GetModifiedStatFloat(StatType.MaxHitPoints);
			int num2 = Mathf.RoundToInt(modifiedStatFloat * modifiedStatFloat2);
			num += num2;
		}
		if (GameplayMutators.Get() != null)
		{
			num = Mathf.RoundToInt(num * GameplayMutators.GetPassiveHpRegenMultiplier());
		}
		return num;
	}

	public int GetMaxTechPoints()
	{
		int result = 1;
		if (m_actorStats != null)
		{
			result = m_actorStats.GetModifiedStatInt(StatType.MaxTechPoints);
		}
		return result;
	}

	//public int GetOutOfCombatShieldMax()  // rogues
	//{
	//	return m_outOfCombatShieldMax;
	//}

	//public int GetBaseStatValue(GearStatType statType)  // rogues
	//{
	//	switch (statType)
	//	{
	//	case GearStatType.PowerAdjustment:
	//		return (int)((float)m_basePower * BasePowerDifficultyAdjustement);
	//	case GearStatType.AccuracyAdjustment:
	//		return (int)((float)m_baseAccuracy * BaseAccuracyDifficultyAdjustement);
	//	case GearStatType.DefenseAdjustment:
	//		return (int)((float)m_baseDefense * BaseDefenseDifficultyAdjustement);
	//	case GearStatType.GlanceOffense:
	//		return (int)((float)m_baseGlanceOffense * BaseGlanceOffenseAdjustment);
	//	case GearStatType.GlanceDefense:
	//		return (int)((float)m_baseGlanceDefense * BaseGlanceDefenseAdjustment);
	//	case GearStatType.CritOffense:
	//		return (int)((float)m_baseCritOffense * BaseCritOffenseAdjustment);
	//	case GearStatType.CritDefense:
	//		return (int)((float)m_baseCritDefense * BaseCritDefenseAdjustment);
	//	case GearStatType.IncomingDamageMultiplierAdjustment:
	//		return (int)((float)m_baseDamageMultiplier * BaseDamageMultiplierDifficultyAdjustment);
	//	case GearStatType.DodgeDefense:
	//		return (int)((float)m_baseDodgeDefense * BaseDodgeDefenseAdjustment);
	//	case GearStatType.DodgeOffense:
	//		return (int)((float)m_baseDodgeOffense * BaseDodgeOffenseAdjustment);
	//	case GearStatType.IncomingHealingAdjustment:
	//		return (int)((float)m_baseIncomingHealingAdjustment * BaseIncomingHealingAdjustmentDifficultyAdjustment);
	//	case GearStatType.BlockDefense:
	//		return (int)((float)m_baseBlockDefense * BaseBlockDefenseAdjustment);
	//	case GearStatType.BlockOffense:
	//		return (int)((float)m_baseBlockOffense * BaseBlockOffenseAdjustment);
	//	case GearStatType.StrengthAdjustment:
	//		return (int)((float)m_baseStrength * BaseStrengthAdjustment);
	//	case GearStatType.ExpertiseAdjustment:
	//		return (int)((float)m_baseExpertise * BaseExpertiseAdjustment);
	//	}
	//	return 0;
	//}

	//public int GetAdjustedPowerLevel(int abilityIndex = -1, ActorData targetActor = null)  // rogues
	//{
	//	int num = GetBaseStatValue(GearStatType.PowerAdjustment);
	//	if (GetEquipmentStats() != null)
	//	{
	//		num = Mathf.RoundToInt(GetEquipmentStats().GetTotalStatValueForSlot(GearStatType.PowerAdjustment, (float)num, abilityIndex, targetActor));
	//		if (targetActor != null && targetActor.GetTeam() == GetTeam())
	//		{
	//			num += Mathf.RoundToInt(GetEquipmentStats().GetTotalStatValueForSlot(GearStatType.ExpertiseAdjustment, (float)GetBaseStatValue(GearStatType.ExpertiseAdjustment), abilityIndex, targetActor));
	//		}
	//		else if (targetActor != null && targetActor.GetTeam() != GetTeam())
	//		{
	//			num += Mathf.RoundToInt(GetEquipmentStats().GetTotalStatValueForSlot(GearStatType.StrengthAdjustment, (float)GetBaseStatValue(GearStatType.StrengthAdjustment), abilityIndex, targetActor));
	//		}
	//	}
	//	return num;
	//}

	//public int GetAdjustedStrength(int abilityIndex = -1, ActorData targetActor = null)  // rogues
	//{
	//	int result = GetBaseStatValue(GearStatType.StrengthAdjustment);
	//	if (GetEquipmentStats() != null)
	//	{
	//		result = Mathf.RoundToInt(GetEquipmentStats().GetTotalStatValueForSlot(GearStatType.StrengthAdjustment, (float)GetBaseStatValue(GearStatType.StrengthAdjustment), abilityIndex, targetActor));
	//	}
	//	return result;
	//}

	//public int GetAdjustedExpertise(int abilityIndex = -1, ActorData targetActor = null)  // rogues
	//{
	//	int result = GetBaseStatValue(GearStatType.ExpertiseAdjustment);
	//	if (GetEquipmentStats() != null)
	//	{
	//		result = Mathf.RoundToInt(GetEquipmentStats().GetTotalStatValueForSlot(GearStatType.ExpertiseAdjustment, (float)GetBaseStatValue(GearStatType.ExpertiseAdjustment), abilityIndex, targetActor));
	//	}
	//	return result;
	//}

	//public ProximityAccuracyAdjustData GetProximityAccuAdjust()  // rogues?
	//{
	//	return m_proximityAccuracyAdjust;
	//}

	public void OnMaxTechPointsChanged(int previousMax)
	{
		if (IsDead())
		{
			return;
		}
		int actualMaxTechPoints = GetMaxTechPoints();
		if (actualMaxTechPoints > previousMax)
		{
			int num = actualMaxTechPoints - previousMax;
			TechPoints += num;
#if SERVER
			GameplayMetricHelper.CollectTechPointsRecieved(this, num);  // server only
#endif
		}
		else if (previousMax > actualMaxTechPoints)
		{
			int techPoints = TechPoints;
			TechPoints = Mathf.Min(TechPoints, actualMaxTechPoints);
			if (techPoints - TechPoints != 0)
			{
#if SERVER
				GameplayMetricHelper.CollectTechPointsRecieved(this, techPoints - TechPoints);  // server only -- empty if in reactor
#endif
			}
		}
	}

	public void TriggerVisibilityForHit(bool movementHit, bool updateClientLastKnownPos = true)
	{
		m_endVisibilityForHitTime = Time.time + (movementHit ? s_visibleTimeAfterMovementHit : s_visibleTimeAfterHit);
		ForceUpdateIsVisibleToClientCache();
		if (updateClientLastKnownPos && GetCurrentBoardSquare() != null)
		{
			ClientLastKnownPosSquare = GetTravelBoardSquare();
			// reactor
			m_lastVisibleTurnToClient = GameFlowData.Get().CurrentTurn;
			// rogues
			//Networkm_lastVisibleTurnToClient = GameFlowData.Get().CurrentTurn;
		}
	}

	private void UpdateClientLastKnownPosSquare()
	{
		if (m_shouldUpdateLastVisibleToClientThisFrame && ClientLastKnownPosSquare != GetCurrentBoardSquare())
		{
			Team team = GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData != null
				? GameFlowData.Get().activeOwnedActorData.GetTeam()
				: Team.Invalid;
			bool isInResolveState = GameFlowData.Get() != null && GameFlowData.Get().IsInResolveState();
			bool isNotMoving = GetTravelBoardSquare() == GetCurrentBoardSquare() && !GetActorMovement().AmMoving() && !GetActorMovement().IsYetToCompleteGameplayPath();
			bool isFromAnotherTeam = GetTeam() != team;
			if (isInResolveState && isNotMoving && isFromAnotherTeam && IsActorVisibleToClient())
			{
				ForceUpdateIsVisibleToClientCache();
				if (IsActorVisibleToClient())
				{
					ClientLastKnownPosSquare = GetCurrentBoardSquare();
					// reactor
					m_lastVisibleTurnToClient = GameFlowData.Get().CurrentTurn;
					// rogues
					//Networkm_lastVisibleTurnToClient = GameFlowData.Get().CurrentTurn;
				}
			}

		}
		m_shouldUpdateLastVisibleToClientThisFrame = true;
	}

	public Player GetPlayer()
	{
		return PlayerData.GetPlayer();
	}

	public PlayerDetails GetPlayerDetails()
	{
		if (PlayerData != null
			&& GameFlow.Get() != null
			&& GameFlow.Get().playerDetails != null
			&& GameFlow.Get().playerDetails.ContainsKey(PlayerData.GetPlayer()))
		{
			return GameFlow.Get().playerDetails[PlayerData.GetPlayer()];
		}
		return null;
	}

	public void SetupAbilityModOnReconnect()
	{
		List<ActorData> actors = GameFlowData.Get().GetActors();
		foreach (ActorData item in actors)
		{
			if (item != null && item.GetAbilityData() != null)
			{
				for (int i = 0; i <= 4; i++)
				{
					item.ApplyAbilityModById(i, item.m_selectedMods.GetModForAbility(i));
					//actorData.ApplyAbilityModById(i, actorData.m_selectedGear.GetGearIDForAbility(i));  // rogues
				}
				ActorTargeting component = item.GetComponent<ActorTargeting>();
				if (component != null)
				{
					component.MarkForForceRedraw();
				}
			}
		}
	}

	public void SetupForRespawnOnReconnect()
	{
		if (IsActorInvisibleForRespawn()
			&& GameFlowData.Get() != null
			&& (ServerClientUtils.GetCurrentActionPhase() < ActionBufferPhase.Movement
			|| ServerClientUtils.GetCurrentActionPhase() > ActionBufferPhase.MovementWait))
		{
			ActorModelData actorModelData = GetActorModelData();
			if (actorModelData != null)
			{
				actorModelData.DisableAndHideRenderers();
			}
			if (HighlightUtils.Get().m_recentlySpawnedShader != null)
			{
				TricksterAfterImageNetworkBehaviour.InitializeAfterImageMaterial(actorModelData, GameFlowData.Get().LocalPlayerData.GetTeamViewing() == GetTeam(), 0.5f, HighlightUtils.Get().m_recentlySpawnedShader, false);
			}
			if (RespawnPickedPositionSquare != null)
			{
				ShowRespawnFlare(RespawnPickedPositionSquare, true);
			}
		}
	}

	// reactor
	public void SetupAbilityMods(CharacterModInfo characterMods)
	{
		m_selectedMods = characterMods;
		AbilityData abilityData = GetAbilityData();
		int abilityId = 0;
		foreach (Ability ability in abilityData.GetAbilitiesAsList())
		{
			int modId = -1;
			if (GameManager.Get().GameConfig.GameType == GameType.Tutorial)
			{
				AbilityMod defaultModForAbility = AbilityModManager.Get().GetDefaultModForAbility(ability);
				modId = defaultModForAbility != null ? defaultModForAbility.m_abilityScopeId : -1;
			}
			else
			{
				modId = m_selectedMods.GetModForAbility(abilityId);
			}
			AbilityData.ActionType actionTypeOfAbility = abilityData.GetActionTypeOfAbility(ability);
			if (actionTypeOfAbility != AbilityData.ActionType.INVALID_ACTION && modId > 0)
			{
				ApplyAbilityModById((int)actionTypeOfAbility, modId);
			}
			abilityId++;
		}
	}
	//public void SetupAbilityGear()  // rogues
	//{
	//	if (NetworkServer.active)
	//	{
	//		AbilityData abilityData = GetAbilityData();
	//		IEnumerable<Gear> enumerable = from g in RunManager.Get().Inventory.Gears.Values
	//		where g.EquippedTo == m_characterType
	//		select g;
	//		AbilityData.ActionType actionType = AbilityData.ActionType.ABILITY_0;
	//		foreach (Gear gear in enumerable)
	//		{
	//			Ability abilityOfActionType = abilityData.GetAbilityOfActionType(actionType++);
	//			if (abilityOfActionType != null && gear != null)
	//			{
	//				ApplyGearToAbility(abilityOfActionType, gear, false);
	//			}
	//		}
	//	}
	//}

#if SERVER
	public void UpdateServerLastKnownPosForHit()  // rogues?
	{
		SetServerLastKnownPosSquare(CurrentBoardSquare, "UpdateServerLastKnownPosForHit");
	}
#endif

	public void UpdateServerLastVisibleTurn()
	{
#if SERVER
		if (IsActorVisibleToAnyEnemy() && GameFlowData.Get() != null)
		{
			// custom
			m_lastVisibleTurnToClient = GameFlowData.Get().CurrentTurn;
			// rogues
			//Networkm_lastVisibleTurnToClient = GameFlowData.Get().CurrentTurn;
		}
#endif
	}

	public void SynchClientLastKnownPosToServerLastKnownPos()
	{
		if (NetworkClient.active && ClientLastKnownPosSquare != ServerLastKnownPosSquare)
		{
			ClientLastKnownPosSquare = ServerLastKnownPosSquare;
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				if (GetTeam() == GameFlowData.Get().activeOwnedActorData.GetTeam())
				{
					// TODO missing code?
				}
				// makes even less sense in rogues
				//GetTeam();
				//GameFlowData.Get().activeOwnedActorData.GetTeam();
			}
		}
	}

	public int GetTechPointRegen()
	{
		int num = 0;
		if (m_actorStats != null)
		{
			num = m_actorStats.GetModifiedStatInt(StatType.TechPointRegen);
		}
		if (GameplayMutators.Get() != null)
		{
			num = Mathf.RoundToInt(num * GameplayMutators.GetPassiveEnergyRegenMultiplier());
		}
		return num;
	}

	public float GetSightRange()
	{
		float result = 1f;
		if (m_actorStats != null)
		{
			result = m_actorStats.GetModifiedStatFloat(StatType.SightRange);
		}
		if (m_actorStatus != null && m_actorStatus.HasStatus(StatusType.Blind))
		{
			result = 0.1f;
		}
		return result;
	}

	internal void SetTechPoints(int value, bool combatText = false, ActorData caster = null, string sourceName = null)
	{
		//int maxTechPoints = GetMaxTechPoints(); // rogues
		value = Mathf.Clamp(value, 0, GetMaxTechPoints());
		int diff = value - TechPoints;
		TechPoints = value;

#if SERVER
		GameplayMetricHelper.CollectTechPointsRecieved(this, diff);
		if (diff > 0 && GetActorBehavior() != null)
		{
			GetActorBehavior().OnEnergyGained(diff);
		}
#endif

		if (combatText && sourceName != null && diff != 0)
		{
			DoTechPointsLogAndCombatText(caster, this, sourceName, diff);
		}
	}

	private void DoTechPointsLogAndCombatText(ActorData caster, ActorData target, string sourceName, int healAmount)
	{
		bool isHeal = healAmount >= 0;
		string combatText = $"{healAmount}";
		string casterName = caster == null ? "" : $"{caster.DisplayName}'s ";
		string logText = isHeal
			? string.Format("{0}{1} adds {3} Energy to {2}", casterName, sourceName, target.DisplayName, healAmount)
			: string.Format("{0}{1} removes {3} Energy from {2}", casterName, sourceName, target.DisplayName, healAmount);
		CombatTextCategory category = isHeal ? CombatTextCategory.TP_Recovery : CombatTextCategory.TP_Damage;
		target.CallRpcCombatText(combatText, logText, category, BuffIconToDisplay.None);
	}

	internal void DoCheatLogAndCombatText(string cheatName)
	{
		string combatText = $"{cheatName}";
		string logText = $"{DisplayName} used cheat: {cheatName}";
		CallRpcCombatText(combatText, logText, CombatTextCategory.Other, BuffIconToDisplay.None);
	}

	internal void SetHitPoints(int value)
	{
		HitPoints = value;
	}

	internal void SetAbsorbPoints(int value)
	{
		AbsorbPoints = value;
	}

	internal bool IsDead()
	{
		return HitPoints == 0;
	}

	public ActorTeamSensitiveData GetTeamSensitiveDataForClient()
	{
		if (m_teamSensitiveData_friendly != null)
		{
			return m_teamSensitiveData_friendly;
		}
		if (m_teamSensitiveData_hostile != null)  // this IF is missing in rogues
		{
			return m_teamSensitiveData_hostile;
		}
		return null;
	}

	public void ClearRespawnSquares()
	{
		m_trueRespawnSquares.Clear();
		if (m_teamSensitiveData_friendly != null)
		{
			m_teamSensitiveData_friendly.RespawnAvailableSquares = new List<BoardSquare>();
		}
	}


	// server-only -- returns false in reactor
	public bool IsRespawnLocationVisibleToEnemy(BoardSquare respawnLocation)
	{
		bool result = false;
#if SERVER
		if (SpawnPointManager.Get() != null && respawnLocation != null)
		{
			foreach (ActorData actorData in GameFlowData.Get().GetAllTeamMembers(GetEnemyTeam()))
			{
				if (!actorData.IsDead() || actorData.NextRespawnTurn == GameFlowData.Get().CurrentTurn)
				{
					BoardSquare boardSquare = actorData.CurrentBoardSquare;
					if (boardSquare == null)
					{
						boardSquare = actorData.RespawnPickedPositionSquare;
					}
					if (boardSquare != null)
					{
						float sightRange = actorData.GetSightRange();
						if (respawnLocation.HorizontalDistanceOnBoardTo(boardSquare) <= sightRange)
						{
							result = true;
							if (SpawnPointManager.Get().m_brushHidesRespawnFlares && respawnLocation.BrushRegion >= 0)
							{
								result = !BrushCoordinator.Get().IsRegionFunctioning(respawnLocation.BrushRegion);
								break;
							}
							break;
						}
					}
				}
			}
		}
#endif
		return result;
	}

	internal void AddLineOfSightVisibleException(ActorData visibleActor)
	{
		m_lineOfSightVisibleExceptions.Add(visibleActor);
		//m_lineOfSightVisibleExceptionActorIndexes.Add(visibleActor.ActorIndex);  // rogues?
		GetFogOfWar().MarkForRecalculateVisibility();
	}

	internal void RemoveLineOfSightVisibleException(ActorData visibleActor)
	{
		m_lineOfSightVisibleExceptions.Remove(visibleActor);
		//m_lineOfSightVisibleExceptionActorIndexes.Remove(visibleActor.ActorIndex);  // rogues?
		GetFogOfWar().MarkForRecalculateVisibility();
	}

	internal bool IsLineOfSightVisibleException(ActorData visibleActor)
	{
		return m_lineOfSightVisibleExceptions.Contains(visibleActor);
	}

	// removed in rogues
	public void OnClientQueuedActionChanged()
	{
		OnClientQueuedActionChangedDelegates?.Invoke();
	}

	// removed in rogues
	public void OnSelectedAbilityChanged(Ability ability)
	{
		OnSelectedAbilityChangedDelegates?.Invoke(ability);
	}

	private void Awake()
	{
		GameFlowData.s_onGameStateChanged -= OnGameStateChanged;
		GameFlowData.s_onGameStateChanged += OnGameStateChanged;
		PlayerData = GetComponent<PlayerData>();
		if (PlayerData == null)
		{
			throw new Exception($"Character {gameObject.name} needs a PlayerData component");
		}
		m_actorMovement = gameObject.GetComponent<ActorMovement>();
		if (m_actorMovement == null)
		{
			m_actorMovement = gameObject.AddComponent<ActorMovement>();
		}

		// rogues?
		//m_actorInputController = gameObject.GetComponent<ActorController>();
		//if (m_actorInputController == null)
		//{
		//	m_actorInputController = gameObject.AddComponent<ActorController>();
		//}

		m_actorTurnSM = gameObject.GetComponent<ActorTurnSM>();
		if (m_actorTurnSM == null)
		{
			m_actorTurnSM = gameObject.AddComponent<ActorTurnSM>();
		}
		m_actorCover = gameObject.GetComponent<ActorCover>();
		if (m_actorCover == null)
		{
			m_actorCover = gameObject.AddComponent<ActorCover>();
		}
		m_actorVFX = gameObject.GetComponent<ActorVFX>();
		if (m_actorVFX == null)
		{
			m_actorVFX = gameObject.AddComponent<ActorVFX>();
		}
		m_timeBank = gameObject.GetComponent<TimeBank>();
		if (m_timeBank == null)
		{
			m_timeBank = gameObject.AddComponent<TimeBank>();
		}
		m_additionalVisionProvider = gameObject.GetComponent<ActorAdditionalVisionProviders>();
		if (m_additionalVisionProvider == null)
		{
			m_additionalVisionProvider = gameObject.AddComponent<ActorAdditionalVisionProviders>();
		}
		m_actorBehavior = GetComponent<ActorBehavior>();
		m_abilityData = GetComponent<AbilityData>();
		m_itemData = GetComponent<ItemData>();
		m_actorStats = GetComponent<ActorStats>();
		m_actorStatus = GetComponent<ActorStatus>();
		//m_actorEffectStatus = GetComponent<ActorEffectStatus>();  // rogues?
		m_actorTargeting = GetComponent<ActorTargeting>();
		m_passiveData = GetComponent<PassiveData>();
		m_combatText = GetComponent<CombatText>();
		m_actorTags = GetComponent<ActorTag>();
		m_freelancerStats = GetComponent<FreelancerStats>();
		//m_equipmentStats = GetComponent<EquipmentStats>();  // rogues
		//m_characterMesh = GetComponentsInChildren<SkinnedMeshRenderer>();  // rogues?

#if SERVER
		if (GetComponent<ServerActorController>() == null)
		{
			gameObject.AddComponent<ServerActorController>();
		}
#endif

		if (NetworkServer.active)
		{
			ActorIndex = checked(++s_nextActorIndex);
		}
		Layer = LayerMask.NameToLayer("Actor");
		Layer_Mask = 1 << Layer;
		if (GameFlowData.Get())
		{
			// reactor
			m_lastSpawnTurn = Mathf.Max(1, GameFlowData.Get().CurrentTurn);
			// rogues
			//Networkm_lastSpawnTurn = Mathf.Max(1, GameFlowData.Get().CurrentTurn);
		}
		else
		{
			// reactor
			m_lastSpawnTurn = 1;
			// rogues
			//Networkm_lastSpawnTurn = 1;
		}
		LastDeathTurn = -2;
		NextRespawnTurn = -1;
		HasBotController = false;
		SpawnerId = -1;
		// rogues?
		//Networkm_serverLastKnownPosX = -1;
		//Networkm_serverLastKnownPosY = -1;
		//RegisterScriptData();
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GametimeScaleChange);
	}

	public void Initialize(PrefabResourceLink heroPrefabLink, bool addMasterSkinVfx)
	{
		if (m_actorSkinPrefabLink.GUID == heroPrefabLink.GUID)
		{
			return;
		}
		if (m_actorSkinPrefabLink != null && !m_actorSkinPrefabLink.IsEmpty)
		{
			Log.Warning(Log.Category.ActorData, string.Format("ActorData already initialized to a different prefab.  Currently [{0}], setting to [{1}]", m_actorSkinPrefabLink.ToString(), heroPrefabLink.ToString()));  // log message fixed in rogues
		}
		m_actorSkinPrefabLink = heroPrefabLink;
		if (heroPrefabLink == null)
		{
			throw new ApplicationException("Actor skin not set on awake. [" + base.gameObject.name + "]");
		}
		if (NetworkClient.active || !HydrogenConfig.Get().SkipCharacterModelSpawnOnServer)
		{
			GameObject gameObject = heroPrefabLink.InstantiatePrefab();
			if (gameObject)
			{
				m_actorModelData = gameObject.GetComponent<ActorModelData>();
				if (m_actorModelData)
				{
					int layer = LayerMask.NameToLayer("Actor");
					Transform[] componentsInChildren = m_actorModelData.gameObject.GetComponentsInChildren<Transform>(true);
					foreach (Transform transform in componentsInChildren)
					{
						transform.gameObject.layer = layer;
					}
				}
			}
		}
		if (m_actorModelData != null)
		{
			m_actorModelData.Setup(this);

			// removed in rogues
			if (addMasterSkinVfx && NetworkClient.active && MasterSkinVfxData.Get() != null)
			{
				GameObject masterSkinVfxInst = MasterSkinVfxData.Get().AddMasterSkinVfxOnCharacterObject(m_actorModelData.gameObject, m_characterType, 1f);
				m_actorModelData.SetMasterSkinVfxInst(masterSkinVfxInst);
			}
		}

		// returns here in rogues

		if (m_faceActorModelData != null)
		{
			m_faceActorModelData.Setup(this);
		}
		if (NPCCoordinator.IsSpawnedNPC(this))
		{
			if (!m_addedToUI && HUD_UI.Get() != null && HUD_UI.Get().m_mainScreenPanel != null)
			{
				m_addedToUI = true;
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.AddActor(this);
				HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddActor(this);
			}
			NPCCoordinator.Get().OnActorSpawn(this);
			if (GetActorModelData() != null)
			{
				GetActorModelData().ForceUpdateVisibility();
			}
		}
	}

	// rogues
	//public void Suspend(bool suspend)
	//{
	//	Networkm_suspend = suspend;
	//	base.enabled = !m_suspend;
	//	GetActorController().enabled = base.enabled;
	//	GetAbilityData().enabled = base.enabled;
	//	GetActorStats().enabled = base.enabled;
	//	GetActorBehavior().enabled = base.enabled;
	//	GetActorTargeting().enabled = base.enabled;
	//	GetActorTurnSM().enabled = base.enabled;
	//	GetActorVFX().enabled = base.enabled;
	//	GetActorMovement().enabled = base.enabled;
	//	PlayerData.enabled = base.enabled;
	//	PlayerData.GetFogOfWar().enabled = base.enabled;
	//}

	// missing in reactor
#if SERVER
	public void OnEnable()
	{
		if (GetActorModelData())
		{
			GetActorModelData().gameObject.SetActive(true);
		}
	}
#endif

	private void Start()
	{
		if (NetworkClient.active)
		{
			m_nameplateJoint = new JointPopupProperty();
			m_nameplateJoint.m_joint = "VFX_name"; // missing in rogues
			m_nameplateJoint.Initialize(gameObject);
		}
		if (NetworkServer.active)
		{
			HitPoints = GetMaxHitPoints();
			UnresolvedDamage = 0;
			UnresolvedHealing = 0;
			TechPoints = m_techPointsOnSpawn;
			ReservedTechPoints = 0;
		}
		ClientUnresolvedDamage = 0;
		ClientUnresolvedHealing = 0;
		ClientUnresolvedTechPointGain = 0;
		ClientUnresolvedTechPointLoss = 0;
		ClientUnresolvedAbsorb = 0;
		ClientReservedTechPoints = 0;
		ClientAppliedHoTThisTurn = 0;
		transform.parent = GameFlowData.Get().GetActorRoot().transform;
		// NOTE CHANGE rogues
#if PURE_REACTOR
		GameFlowData.Get().AddActor(this);  // added here in reactor, not a few lines later
#endif
		EnableRagdoll(false);
		if (!m_addedToUI && HUD_UI.Get() != null && HUD_UI.Get().m_mainScreenPanel != null)
		{
			m_addedToUI = true;
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.AddActor(this);
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddActor(this);
		}
#if !PURE_REACTOR
		GameFlowData.Get().AddActor(this);
#endif
		if (GameFlow.Get().playerDetails.TryGetValue(PlayerData.GetPlayer(), out var playerDetails) && playerDetails.IsLocal())
		{
			// logged in rogues even if !playerDetails.IsLocal()
			Log.Info("ActorData.Start {0} {1}", this, playerDetails);
			GameFlowData.Get().AddOwnedActorData(this);

			// rogues
			//if (NetworkClient.active && !NetworkServer.active)
			//{
			//	InitEquipmentStats();
			//}
		}
	}

	public override void OnStartLocalPlayer()
	{
		Log.Info("ActorData.OnStartLocalPlayer {0}", this);
		GameFlowData.Get().AddOwnedActorData(this);
		if (ClientBootstrap.LoadTest)
		{
			CallCmdDebugReplaceWithBot();
		}
	}

	private static void OnGameStateChanged(GameState newState)
	{
		switch (newState)
		{
			case GameState.BothTeams_Decision:
				{
					HandleRagdollOnDecisionStart();
					int currentTurn = GameFlowData.Get().CurrentTurn;
					bool flag = false;
					foreach (ActorData actor in GameFlowData.Get().GetActors())
					{
						if (actor != null
							&& !actor.IsDead()
							&& actor.GetActorModelData() != null
							&& actor.IsInRagdoll())
						{
							Debug.LogError("Unragdolling undead actor on Turn Tick (" + currentTurn + "): " + actor.DebugNameString());
							actor.EnableRagdoll(false);
							flag = true;
						}
						if (actor != null
							&& !actor.IsDead()
							&& actor.GetCurrentBoardSquare() == null
							&& actor.PlayerIndex != PlayerData.s_invalidPlayerIndex)
						{
							if (NetworkClient.active
								&& !NetworkServer.active
								&& GameFlowData.Get().LocalPlayerData.IsViewingTeam(actor.GetTeam()))
							{
								Debug.LogError("On client, living friendly-to-client actor " + actor.DebugNameString() + " has null square on Turn Tick");
								flag = true;
							}

#if SERVER
							// missing in reactor -- server-only
							if (NetworkServer.active)
							{
								Debug.LogError("On server, living actor " + actor.DebugNameString() + " has null square on Turn Tick");
								flag = true;
								BoardSquare squareFromVec = Board.Get().GetSquareFromVec3(actor.LastDeathPosition);
								BoardSquare squareFromVec2 = Board.Get().GetSquareFromVec3(actor.transform.position);
								BoardSquare boardSquare;
								if (actor.LastDeathTurn + 1 == GameFlowData.Get().CurrentTurn && squareFromVec != null && squareFromVec.IsValidForGameplay())
								{
									boardSquare = squareFromVec;
									Debug.LogError("Setting current board square to failsafe square (last death square) " + boardSquare.ToString());
								}
								else if (squareFromVec2 != null && squareFromVec2.IsValidForGameplay())
								{
									boardSquare = squareFromVec2;
									Debug.LogError("Setting current board square to failsafe square (at current position) " + boardSquare.ToString());
								}
								else if (actor.m_serverMovementDestination != null)
								{
									boardSquare = actor.m_serverMovementDestination;
									Debug.LogError("Setting current board square to failsafe square (on server movement square) " + boardSquare.ToString());
								}
								else
								{
									boardSquare = SpawnPointManager.Get().GetSpawnSquare(actor, true, null, null);
									Debug.LogError("Setting current board square to failsafe square (at a respawn square) " + boardSquare.ToString());
								}
								actor.TeleportToBoardSquare(boardSquare, Vector3.zero, TeleportType.Failsafe, null, 20f, MovementType.Teleport, GameEventManager.EventType.Invalid, null);
							}
#endif
						}
					}
#if SERVER
					// missing in reactor -- server-only
					if (NetworkServer.active && flag)
					{
						GameFlowData.Get().LogTurnBehaviorsFromTurnsAgo(1);
						return;
					}
#endif
					return;
				}
			case GameState.BothTeams_Resolve:
				{
					foreach (ActorData actor in GameFlowData.Get().GetActors())
					{
						if (actor != null)
						{
							if (actor.GetActorModelData() != null)
							{
								Animator modelAnimator = actor.GetModelAnimator();
								if (modelAnimator != null && actor.GetActorModelData().HasTurnStartParameter())
								{
									modelAnimator.SetBool("TurnStart", false);
								}
							}
							if (actor.GetComponent<LineData>() != null)
							{
								actor.GetComponent<LineData>().OnResolveStart();
							}
							if (HUD_UI.Get() != null)
							{
								HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateNameplateUntargeted(actor);
							}
#if SERVER
							// custom
							ActorBehavior actorBehavior = actor.GetComponent<ActorBehavior>();
							ActorTurnSM actorTurnSm = actor.GetActorTurnSM();
							if (actorBehavior != null && actorTurnSm != null)
							{
								actorBehavior.OnResolutionStart(actorTurnSm.TimeToLockIn.Seconds);
							}

							PassiveData passiveData = actor.GetPassiveData();
							if (passiveData != null)
							{
								passiveData.OnResolveStart();
							}
#endif
						}
					}
					return;
				}
			case GameState.EndingGame:
				{
					s_nextActorIndex = 0;
					return;
				}
		}
	}

	private static void HandleRagdollOnDecisionStart()
	{
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (actorData != null
				&& actorData.IsDead()
				&& actorData.LastDeathTurn != GameFlowData.Get().CurrentTurn
				&& !actorData.IsInRagdoll()
				&& actorData.NextRespawnTurn != GameFlowData.Get().CurrentTurn)
			{
				// reactor
				actorData.DoVisualDeath(Sequence.CreateImpulseInfoWithActorForward(actorData));
				// rogues
				//actorData.DoVisualDeath(null);
			}
		}
	}

	public Animator GetModelAnimator()
	{
		if (m_actorModelData != null)
		{
			return m_actorModelData.GetModelAnimator();
		}
		return null;
	}

	// added in rogues?
#if SERVER
	public void PlaySpawnAnim()
	{
		Animator modelAnimator = GetModelAnimator();
		if (modelAnimator != null && m_actorModelData.HasAnimatorControllerParamater("StartSpawn"))
		{
			modelAnimator.SetTrigger("StartSpawn");
		}
	}
#endif

	public void PlayDamageReactionAnim(string customDamageReactTriggerName)
	{
		Animator modelAnimator = GetModelAnimator();
		if (modelAnimator != null
			&& m_actorMovement.GetAestheticPath() == null
			&& !m_actorMovement.AmMoving()
			// following condition removed in rogues
			&& (ServerClientUtils.GetCurrentAbilityPhase() != AbilityPriority.Combat_Knockback
				|| ClientKnockbackManager.Get() == null
				|| !ClientKnockbackManager.Get().ActorHasIncomingKnockback(this)))
		{
			string trigger = string.IsNullOrEmpty(customDamageReactTriggerName)
				? "StartDamageReaction"
				: customDamageReactTriggerName;
			modelAnimator.SetTrigger(trigger);
		}
	}

	internal bool IsInRagdoll()
	{
		// TODO HACK ActorData.m_actorModelData can happen to be null, and it affects gameplay for some reason
		// (e.g. AdvanceGameplayPath is not called after a dash)
#if SERVER
		return false;
#else
		Animator modelAnimator = GetModelAnimator();
		return modelAnimator == null || !modelAnimator.enabled;
#endif
	}

	internal void DoVisualDeath(ActorModelData.ImpulseInfo impulseInfo)
	{
		if (IsInRagdoll())
		{
			Debug.LogWarning("Already in ragdoll");
		}
		if (m_actorVFX != null)
		{
			m_actorVFX.ShowOnDeathVfx(GetActorMovement().transform.position);
		}
		EnableRagdoll(true, impulseInfo);
		GameEventManager.Get().FireEvent(GameEventManager.EventType.CharacterVisualDeath, new GameEventManager.CharacterDeathEventArgs
		{
			deadCharacter = this
		});
		if (AudioManager.s_deathAudio)
		{
			AudioManager.PostEvent("ui/ingame/death", gameObject);
			if (!string.IsNullOrEmpty(m_onDeathAudioEvent))
			{
				AudioManager.PostEvent(m_onDeathAudioEvent, gameObject);
			}
		}
		Team team = Team.Invalid;
		if (GameFlowData.Get() != null && GameFlowData.Get().LocalPlayerData != null)
		{
			team = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
		}
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (clientFog != null && GetTeam() == team)
		{
			clientFog.MarkForRecalculateVisibility();
		}
		if (NetworkClient.active)
		{
			if (ObjectivePoints.Get() != null)
			{
				ObjectivePoints.Get().Client_OnActorDeath(this);
				if (GameplayUtils.IsPlayerControlled(this)
					&& GameFlowData.Get().LocalPlayerData != null
					&& ObjectivePoints.Get().Client_GetNumDeathOnTeamForCurrentTurn(GetTeam()) > 0
					&& UIDeathNotifications.Get() != null)
				{
					UIDeathNotifications.Get().NotifyDeathOccurred(this, GetTeam() == team);
				}
			}
			if (CaptureTheFlag.Get() != null)
			{
				CaptureTheFlag.Get().Client_OnActorDeath(this);
			}
			if (CollectTheCoins.Get() != null)
			{
				CollectTheCoins.Get().Client_OnActorDeath(this);
			}
			if (GameWideData.Get().FreeAutomaticOverconOnDeath_OverconID != -1 && UIOverconData.Get() != null)
			{
				UIOverconData.Get().UseOvercon(GameWideData.Get().FreeAutomaticOverconOnDeath_OverconID, ActorIndex, true);
			}
		}
	}

	private void EnableRagdoll(bool ragDollOn, ActorModelData.ImpulseInfo impulseInfo = null, bool isDebugRagdoll = false)
	{
		if (ragDollOn && GetHitPointsToDisplay() > 0 && !isDebugRagdoll)   // !isDebugRagdoll condition removed in rogues
		{
			Log.Error("early_ragdoll: enabling ragdoll on " + DebugNameString() + " with " + HitPoints + " HP,  (HP for display " + GetHitPointsToDisplay() + ")\n" + Environment.StackTrace);
		}
		if (m_actorModelData != null)
		{
			m_actorModelData.EnableRagdoll(ragDollOn, impulseInfo);
		}
	}

	public void OnReplayRestart()
	{
		EnableRendererAndUpdateVisibility();
		EnableRagdoll(false);
		ShowRespawnFlare(null, false);
	}

	public void OnRespawnTeleport()
	{
		EnableRagdoll(false);
		if (this == GameFlowData.Get().activeOwnedActorData
			&& SpawnPointManager.Get() != null
			&& SpawnPointManager.Get().m_spawnInDuringMovement)
		//SpawnPointManager.Get().SpawnInDuringMovement())  // rogues
		{
			InterfaceManager.Get().DisplayAlert(StringUtil.TR("PostRespawnMovement", "Global"), BoardSquare.s_respawnOptionHighlightColor, 60f, true);
		}
	}

	private void OnRespawn()
	{
		EnableRagdoll(false);
		ActorModelData actorModelData = GetActorModelData();
		if (actorModelData != null)
		{
			actorModelData.ForceUpdateVisibility();
		}

		// missing in reactor
#if SERVER
		if (NetworkServer.active && GameplayUtils.IsPlayerControlled(this))
		{
			int num = GameplayData.Get().m_recentlyRespawnedDuration + 1;
			for (int i = 0; i < num; i++)
			{
				m_actorStatus.AddStatus(StatusType.RecentlyRespawned, 1);
			}
		}
#endif

		// missing in rogues
		if (!NetworkServer.active && NPCCoordinator.IsSpawnedNPC(this))
		{
			NPCCoordinator.Get().OnActorSpawn(this);
		}
		// end missing

		GameEventManager.Get().FireEvent(GameEventManager.EventType.CharacterRespawn, new GameEventManager.CharacterRespawnEventArgs
		{
			respawningCharacter = this
		});
		if (GameFlowData.Get().activeOwnedActorData == this)
		{
			CameraManager.Get().SetTargetObject(gameObject, CameraManager.CameraTargetReason.ClientActorRespawned);
		}
		// reactor
		m_lastSpawnTurn = GameFlowData.Get().CurrentTurn;
		// rogues
		//Networkm_lastSpawnTurn = GameFlowData.Get().CurrentTurn;
	}

	public bool IsActorInvisibleForRespawn()
	{
		return !IsDead() &&
			NextRespawnTurn > 0 &&
			NextRespawnTurn == GameFlowData.Get().CurrentTurn &&
			SpawnPointManager.Get() != null &&
			SpawnPointManager.Get().m_spawnInDuringMovement;
		//SpawnPointManager.Get().SpawnInDuringMovement();  // rogues
	}

	// server-only -- missing in reactor
	//#if SERVER
	//	private void OnDeath()
	//	{
	//		if (NetworkServer.active)
	//		{
	//			BoardSquare mostRecentDeathSquare = GetMostRecentDeathSquare();
	//			int num = (mostRecentDeathSquare != null) ? mostRecentDeathSquare.GetGridPos().x : -1;
	//			int num2 = (mostRecentDeathSquare != null) ? mostRecentDeathSquare.GetGridPos().y : -1;
	//			EventLogMessage eventLogMessage = new EventLogMessage("match", "ActorDeath");
	//			eventLogMessage.AddData("GridPosX", num);
	//			eventLogMessage.AddData("GridPosY", num2);
	//			eventLogMessage.AddData("CharacterType", m_characterType);
	//			eventLogMessage.AddData("IsBotControlled", HasBotController);
	//			PlayerDetails playerDetails;
	//			if (GameFlow.Get().playerDetails.TryGetValue(PlayerData.GetPlayer(), out playerDetails))
	//			{
	//				eventLogMessage.AddData("AccountId", playerDetails.m_accountId);
	//			}
	//			eventLogMessage.AddData("Team", (int)m_team);
	//			eventLogMessage.AddData("Turn", GameFlowData.Get().CurrentTurn);

	//			//reactor
	//			eventLogMessage.AddData("Map", GameManager.Get().GameConfig.Map);
	//			// rogues
	//			//eventLogMessage.AddData("Map", GameManager.Get().GameMission.Map);
	//			//eventLogMessage.AddData("Encounter", GameManager.Get().GameMission.Encounter);

	//			eventLogMessage.AddData("ProcessCode", HydrogenConfig.Get().ProcessCode);
	//			eventLogMessage.Write();
	//			ServerActionBuffer.Get().CancelMovementRequests(this, false);
	//			MatchLogger.Get().Log(string.Format("{0} Died", DisplayName));
	//		}
	//	}
	//#endif

	private void OnDestroy()
	{
		if (GameFlowData.Get() != null)
		{
			GameFlowData.Get().RemoveReferencesToDestroyedActor(this);
		}
		GameFlowData.s_onGameStateChanged -= OnGameStateChanged;
		m_actorModelData = null;
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GametimeScaleChange);
	}

	private void Update()
	{
		if (m_needAddToTeam && GameFlowData.Get() != null)
		{
			m_needAddToTeam = false;
			GameFlowData.Get().AddToTeam(this);
			TeamStatusDisplay.GetTeamStatusDisplay().RebuildTeamDisplay();
		}
		if (NetworkClient.active)
		{
			UpdateClientLastKnownPosSquare();
		}
		if (Quaternion.Angle(transform.localRotation, m_targetRotation) > 0.01f)
		{
			transform.localRotation = m_targetRotation;
		}
		if (m_callHandleOnSelectInUpdate)
		{
			HandleOnSelectedAsActiveActor();
			m_callHandleOnSelectInUpdate = false;
		}
		if (NetworkServer.active)
		{
			// TODO check this
			// reactor
			SetDirtyBit(1u);
			// rogues (seems to be rogues-specific code)
			//Networkm_currentMovementFlags = ServerClientUtils.CreateBitfieldFromBools(QueuedMovementAllowsAbility, HasQueuedMovement(), HasQueuedChase(), Alerted, false, false, false, false); // rogues
			//Networkm_queuedChaseTargetActorIndex = (sbyte)GameplayUtils.GetActorIndexOfActor(GetQueuedChaseTarget()); // rogues
		}
		if (!m_addedToUI && HUD_UI.Get() != null)
		{
			m_addedToUI = true;
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.AddActor(this);
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddActor(this);
		}
	}

	public bool IsInBrush()
	{
		int brushRegion = GetBrushRegion();
		return brushRegion != -1 && BrushCoordinator.Get().IsRegionFunctioning(brushRegion);
	}

	public int GetBrushRegion()
	{
		int result = -1;
		BoardSquare travelBoardSquare = GetTravelBoardSquare();
		if (travelBoardSquare != null && travelBoardSquare.IsInBrush())
		{
			result = travelBoardSquare.BrushRegion;
		}
		return result;
	}

	public bool IsNameplateVisible()
	{
		return !m_hideNameplate
			&& !m_alwaysHideNameplate
			&& ShowInGameGUI
			&& IsActorVisibleToClient()
			&& !IsInRagdoll()
			&& (GetActorModelData() == null || GetActorModelData().IsVisibleToClient());
	}

	public void SetNameplateAlwaysInvisible(bool alwaysHide)
	{
		// reactor
		m_alwaysHideNameplate = alwaysHide;
		// rogues
		//Networkm_alwaysHideNameplate = alwaysHide;
	}

	private bool CalculateIsActorVisibleToClient()
	{
		if (GameFlowData.Get() == null || GameFlowData.Get().LocalPlayerData == null)
		{
			return false;
		}
		PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("AllCharactersVisible"))
		{
			return true;
		}
		if (GameFlowData.Get().gameState == GameState.Deployment)
		{
			return true;
		}
		if (activeOwnedActorData != null && activeOwnedActorData.GetTeam() == GetTeam())
		{
			return true;
		}
		if (activeOwnedActorData == null && localPlayerData.IsViewingTeam(GetTeam()))
		{
			return true;
		}
		if (m_endVisibilityForHitTime > Time.time)
		{
			return true;
		}
		if (m_actorModelData != null && m_actorModelData.IsInCinematicCam())
		{
			return true;
		}
		if (CurrentlyVisibleForAbilityCast)  // missing in rogues
		{
			return true;
		}
		if (m_disappearingAfterCurrentMovement && CurrentBoardSquare == null && !GetActorMovement().AmMoving())
		{
			return false;
		}
		if (IsAlwaysVisibleTo(localPlayerData, false))
		{
			return true;
		}
		if (IsNeverVisibleTo(localPlayerData, false))
		{
			return false;
		}
		if (FogOfWar.GetClientFog() == null)
		{
			return false;
		}
		if (FogOfWar.GetClientFog().IsVisible(GetTravelBoardSquare()))
		{
			return true;
		}
		return false;
	}

	// removed in rogues
	public void ForceUpdateActorModelVisibility()
	{
		if (NetworkClient.active && m_actorModelData != null)
		{
			m_actorModelData.ForceUpdateVisibility();
		}
	}

	public void ForceUpdateIsVisibleToClientCache()
	{
		m_lastIsVisibleToClientTime = 0f;
		UpdateIsVisibleToClientCache();
	}

	private void UpdateIsVisibleToClientCache()
	{
		if (m_lastIsVisibleToClientTime < Time.time)
		{
			m_lastIsVisibleToClientTime = Time.time;
			m_isVisibleToClientCache = CalculateIsActorVisibleToClient();
		}
	}

	public bool IsActorVisibleToClient()
	{
		UpdateIsVisibleToClientCache();
		return m_isVisibleToClientCache;
	}

	public bool IsActorVisibleToSpecificClient(ActorData observer)
	{
		if (GameFlowData.Get().IsActorDataOwned(this) && GetTeam() == observer.GetTeam())
		{
			return true;
		}
		if (m_endVisibilityForHitTime > Time.time)
		{
			return true;
		}
		if (IsAlwaysVisibleTo(observer.PlayerData))
		{
			return true;
		}
		if (IsNeverVisibleTo(observer.PlayerData))
		{
			return false;
		}
		if (observer.GetFogOfWar() != null)
		{
			return observer.GetFogOfWar().IsVisible(GetTravelBoardSquare());
		}
		return false;
	}

	public bool IsAlwaysVisibleTo(PlayerData observer, bool includePendingStatus = true)
	{
		if (observer == null)
		{
			return false;
		}
		if (GetActorStatus().HasStatus(StatusType.Revealed, includePendingStatus) && GetTeam() != observer.GetTeamViewing())
		{
			return true;
		}
		if (!NetworkServer.active && CaptureTheFlag.IsActorRevealedByFlag_Client(this))
		{
			return true;
		}
#if SERVER
		if (NetworkServer.active && CaptureTheFlag.IsActorRevealedByFlag_Server(this))  // missing in reactor
		{
			return true;
		}
#endif
		if (VisibleTillEndOfPhase && !MovedForEvade)  // !MovedForEvade part is missing in rogues
		{
			return true;
		}
		if (CurrentlyVisibleForAbilityCast)  // removed in rogues
		{
			return true;
		}
		return IsDead() && IsInRagdoll();

	}


	public bool IsNeverVisibleTo(PlayerData observer, bool includePendingStatus = true, bool forceViewingTeam = false)
	{
		if (observer == null)
		{
			return false;
		}


		// forceViewingTeam parameter is removed in rogues
		Team team;
		if (forceViewingTeam && observer.ActorData != null)
		{
			team = observer.ActorData.GetTeam();
		}
		else
		{
			team = observer.GetTeamViewing();
		}

		if (GetActorStatus().IsInvisibleToEnemies(includePendingStatus) && GetTeam() != team)
		{
			if (observer.ActorData && observer.ActorData.GetActorStatus().HasStatus(StatusType.SeeInvisible, includePendingStatus))
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public bool IsActorVisibleToActor(ActorData observer, bool forceViewingTeam = false, bool debugLog = false) // custom debug log
	{
		if (this == observer)
		{
			if (debugLog) Log.Info($"{DisplayName} is visible to self");
			return true;
		}
		if (!NetworkServer.active && observer == GameFlowData.Get().activeOwnedActorData)
		{
			if (debugLog) Log.Info($"{DisplayName} is {(IsActorVisibleToClient() ? "" : "not ")} visible to client");
			return IsActorVisibleToClient();
		}
		if (!NetworkServer.active)
		{
			Log.Warning("Calling the server-only method ActorData::IsActorVisibleToActor on a client. Clients can only depend on ActorData::IsActorVisibleToClient.");
		}
		if (IsAlwaysVisibleTo(observer.PlayerData))
		{
			if (debugLog) Log.Info($"{DisplayName} is always visible to {observer.DisplayName}");
			return true;
		}
		if (IsNeverVisibleTo(observer.PlayerData, true, forceViewingTeam))
		{
			if (debugLog) Log.Info($"{DisplayName} is never visible to {observer.DisplayName}");
			return false;
		}

		bool isActorVisibleToActor = observer.GetFogOfWar().IsVisible(GetTravelBoardSquare());
		if (debugLog) Log.Info($"{DisplayName} ({GetTravelBoardSquare()?.GetGridPos()}) is " +
		                       $"{(isActorVisibleToActor ? "" : "not ")}" +
		                       $"visible to {observer.DisplayName} ({observer.GetTravelBoardSquare()?.GetGridPos()}) " +
		                       $"in fog of war");
		return isActorVisibleToActor;
	}

	public bool IsActorVisibleToAnyEnemy(bool debugLog = false) // custom debug log
	{
		foreach (ActorData enemy in GameFlowData.Get().GetAllTeamMembers(GetEnemyTeam()))
		{
			if (!enemy.IsDead() && IsActorVisibleToActor(enemy, true, debugLog))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsActorVisibleIgnoringFogOfWar(ActorData observer)
	{
		if (IsAlwaysVisibleTo(observer.PlayerData))
		{
			return true;
		}
		if (IsNeverVisibleTo(observer.PlayerData, true, true))
		{
			return false;
		}
		bool IsBrushRevealed = GetBrushRegion() < 0 || BrushRegion.HasTeamMemberInRegion(GetEnemyTeam(), GetBrushRegion());
		return !IsInBrush() || IsBrushRevealed;
	}

	public void ApplyForceFromPoint(Vector3 pos, float amount, Vector3 overrideDir)
	{
		Vector3 vector = GetBonePosition("hip_JNT") - pos;
		if (vector.magnitude < 1.5f)
		{
			if (overrideDir != Vector3.zero)
			{
				ApplyForce(overrideDir.normalized, amount);
			}
			else
			{
				ApplyForce(vector.normalized, amount);
			}
		}
	}

	public void ApplyForce(Vector3 dir, float amount)  // private in rogues
	{
		Rigidbody hipJointRigidBody = GetHipJoint();
		//Rigidbody hipJointRigidBody = GetRigidBody("hip_JNT");  // rogues
		if (hipJointRigidBody)
		{
			hipJointRigidBody.AddForce(dir * amount, ForceMode.Impulse);
		}
	}

	public Vector3 GetOverheadPosition(float offsetInPixels)
	{
		if (Camera.main == null || m_nameplateJoint == null || m_nameplateJoint.m_jointObject == null)  // last condition missing in reactor
		{
			return default(Vector3);
		}
		Vector3 position = m_nameplateJoint.m_jointObject.transform.position;
		Vector3 b = Camera.main.WorldToScreenPoint(position);
		Vector3 a = Camera.main.WorldToScreenPoint(position + Camera.main.transform.up);
		Vector3 vector = a - b;
		vector.z = 0f;
		float d = offsetInPixels / vector.magnitude;
		Vector3 b2 = Camera.main.transform.up * d;
		return position + b2;
	}

	//public Bounds GetCombinedBounds()  // rogues?
	//{
	//	if (m_characterMesh.Length == 0)
	//	{
	//		m_characterMesh = base.GetComponentsInChildren<SkinnedMeshRenderer>();
	//		if (m_characterMesh.Length == 0)
	//		{
	//			Debug.LogWarning("Could not find skinned mesh renderer for this actor data. Can't generate combined bounds.");
	//			return new Bounds(base.transform.position, Vector3.one);
	//		}
	//	}
	//	Bounds bounds = m_characterMesh[0].bounds;
	//	for (int i = 1; i < m_characterMesh.Length; i++)
	//	{
	//		bounds.Encapsulate(m_characterMesh[i].bounds);
	//	}
	//	return bounds;
	//}

	public Vector3 GetLoSCheckPos()
	{
		return GetLoSCheckPos(GetTravelBoardSquare());
	}

	public Vector3 GetFreePos()
	{
		return GetFreePos(GetTravelBoardSquare());
	}

	public Vector3 GetLoSCheckPos(BoardSquare asIfFromSquare)
	{
		if (asIfFromSquare == null)
		{
			Log.Warning("Trying to get LoS check pos wrt a null square (IsDead: " + IsDead() + ")  for " + DisplayName + " Code issue-- actor probably instantiated but not on Board yet.");  // removed in rogues
			return m_actorMovement.transform.position;
		}
		Vector3 freePos = GetFreePos(asIfFromSquare);
		freePos.y += BoardSquare.s_LoSHeightOffset;
		return freePos;
	}

	public Vector3 GetFreePos(BoardSquare asIfFromSquare)
	{
		if (asIfFromSquare == null)
		{
			Log.Warning("Trying to get free pos wrt a null square (IsDead: " + IsDead() + ").  for " + DisplayName + " Code issue-- actor probably instantiated but not on Board yet.");  // removed in rogues
			return m_actorMovement.transform.position;
		}
		return asIfFromSquare.GetOccupantRefPos();
	}

	public int GetHitPointsToDisplay()
	{
		int num = UnresolvedDamage + ClientUnresolvedDamage;
		int num2 = UnresolvedHealing + ClientUnresolvedHealing;
		int num3 = AbsorbPoints + ClientUnresolvedAbsorb;
		int num4 = Mathf.Max(0, num - num3);
		return Mathf.Clamp(HitPoints - num4 + num2, 0, GetMaxHitPoints());
	}

	public int GetExpectedClientHpForDisplay(int deltaHp)
	{
		int num = UnresolvedDamage + ClientUnresolvedDamage;
		int num2 = UnresolvedHealing + ClientUnresolvedHealing;
		int num3 = AbsorbPoints + ClientUnresolvedAbsorb;
		if (deltaHp > 0)
		{
			num2 += deltaHp;
		}
		else
		{
			num -= deltaHp;
		}
		int num4 = Mathf.Max(0, num - num3);
		return Mathf.Clamp(HitPoints - num4 + num2, 0, GetMaxHitPoints());
	}

	// server-only
#if SERVER
	public int GetHitPointInServerResolution()
	{
		int num = Mathf.Max(0, UnresolvedDamage - AbsorbPoints);
		return Mathf.Clamp(HitPoints - num + UnresolvedHealing, 0, GetMaxHitPoints());
	}
#endif

	// server-only
#if SERVER
	public float GetHpPortionInServerResolution()
	{
		return GetHitPointInServerResolution() / (float)GetMaxHitPoints();
	}
#endif

	public int GetTechPointsToDisplay()
	{
		int num = UnresolvedTechPointGain + ClientUnresolvedTechPointGain;
		int num2 = UnresolvedTechPointLoss + ClientUnresolvedTechPointLoss;
		return Mathf.Clamp(TechPoints + ReservedTechPoints + ClientReservedTechPoints + num - num2, 0, GetMaxTechPoints());
	}

	public int GetHoTTotalToDisplay()
	{
		return Mathf.Max(0, ExpectedHoTTotal + ClientExpectedHoTTotalAdjust - ClientAppliedHoTThisTurn);
	}

	public int GetPendingHoTToDisplay()
	{
		return Mathf.Max(0, ExpectedHoTThisTurn - ClientAppliedHoTThisTurn);
	}

	public string GetHealthString()
	{
		string text = $"{GetHitPointsToDisplay()}";
		if (AbsorbPoints > 0)
		{
			int num = UnresolvedDamage + ClientUnresolvedDamage;
			int num2 = AbsorbPoints + ClientUnresolvedAbsorb;
			int num3 = Mathf.Max(0, num2 - num);
			text += $" +{num3} shield";
		}
		return text;
	}

	public int GetShieldPoints()
	{
		int num = UnresolvedDamage + ClientUnresolvedDamage;
		int num2 = AbsorbPoints + ClientUnresolvedAbsorb;
		return Mathf.Max(0, num2 - num);
	}

	public bool IsHumanControlled()
	{
		if (GameFlow.Get() == null || GameFlow.Get().playerDetails == null)
		{
			Log.Error("Method called too early, results may be incorrect");
			return false;
		}
		if (GameFlow.Get().playerDetails.TryGetValue(PlayerData.GetPlayer(), out PlayerDetails value))
		{
			// NOTE CHANGE rogues: did not check IsLoadTestBot in reactor
			return value.IsHumanControlled && !value.IsLoadTestBot;
		}
		return false;
	}

	public bool IsBotMasqueradingAsHuman()
	{
		if (!GameplayUtils.IsPlayerControlled(this) || !GameplayUtils.IsBot(this))
		{
			return false;
		}
		if (!GameFlow.Get().playerDetails.TryGetValue(PlayerData.GetPlayer(), out PlayerDetails playerDetails))
		{
			return false;
		}
		if (playerDetails == null || !playerDetails.m_botsMasqueradeAsHumans)
		{
			return false;
		}
		return true;
	}

	public long GetAccountId()
	{
		if (GameFlow.Get().playerDetails.TryGetValue(PlayerData.GetPlayer(), out PlayerDetails playerDetails))
		{
			if (IsBotMasqueradingAsHuman() || playerDetails.IsLoadTestBot || IsHumanControlled())
			{
				return playerDetails.m_accountId;
			}
		}
		return -1L;
	}

	public long GetOriginalAccountId()
	{
		if (PlayerData != null)
		{
			if (GameFlow.Get().playerDetails.TryGetValue(PlayerData.GetPlayer(), out PlayerDetails playerDetails))
			{
				return playerDetails.m_accountId;
			}
		}
		return -1L;
	}

	private void OnDisable()
	{
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.RemoveActor(this);
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.RemoveActor(this);
		}
		if (GameFlowData.Get())
		{
			GameFlowData.Get().RemoveFromTeam(this);
		}

		// server-only or rogues?
#if SERVER
		if (GetActorModelData())
		{
			GetActorModelData().gameObject.SetActive(false);
		}
#endif
	}

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		return OnSerializeHelper(new NetworkWriterAdapter(writer), initialState);
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint num = uint.MaxValue;
		if (!initialState)
		{
			num = reader.ReadPackedUInt32();
		}
		if (num != 0)
		{
			OnSerializeHelper(new NetworkReaderAdapter(reader), initialState);
		}
		if (!initialState)
		{
			return;
		}
		if (AsyncPump.Current != null)
		{
			Log.Info("Waiting for objects to be created . . .");
			AsyncPump.Current.Break();
		}
		else
		{
			Log.Info("ActorData initialState without an async pump; something may be broken!");
		}
	}

	private bool OnSerializeHelper(IBitStream stream, bool initialState)
	{
		if (!initialState && m_serializeHelper.ShouldReturnImmediately(ref stream))
		{
			return false;
		}
		uint position = stream.Position;
		sbyte _playerIndex = 0;
		sbyte _actorIndex = 0;
		string _displayName = m_displayName;
		float _RemainingHorizontalMovement = 0f;
		float _RemainingMovementWithQueuedAbility = 0f;
		sbyte _queuedChaseTargetActorIndex = (sbyte)s_invalidActorIndex;
		bool _QueuedMovementAllowsAbility = true;
		bool _HasQueuedMovement = false;
		bool _HasQueuedChase = false;
		byte _queuedMovementBitfield = 0;
		sbyte _team = 0;
		short _HitPoints = 0;
		short _UnresolvedDamage = 0;
		short _UnresolvedHealing = 0;
		short _TechPoints = 0;
		short _ReservedTechPoints = 0;
		short _UnresolvedTechPointGain = 0;
		short _UnresolvedTechPointLoss = 0;
		short _AbsorbPoints = 0;
		short _MechanicPoints = 0;
		short _ExpectedHoTTotal = 0;
		short _ExpectedHoTThisTurn = 0;
		byte _pointsBitfield = 0;
		int _LastDeathTurn = -1;
		int _lastSpawnTurn = -1;
		int _NextRespawnTurn = -1;
		sbyte _SpawnerId = -1;
		sbyte _lineOfSightVisibleExceptionsCount = 0;
		int _lastVisibleTurnToClient = 0;
		short _serverLastKnownPosSquare_x = 0;
		short _serverLastKnownPosSquare_y = 0;
		bool _HasBotController = false;
		bool _showInGameHud = true;
		bool _VisibleTillEndOfPhase = false;
		bool _ignoreFromAbilityHits = false;
		bool _alwaysHideNameplate = false;
		byte _UiGameplayBitfield = 0;
		if (stream.isWriting)
		{
			_playerIndex = (sbyte)PlayerIndex;
			_actorIndex = (sbyte)ActorIndex;
			_team = (sbyte)m_team;
			_lastVisibleTurnToClient = m_lastVisibleTurnToClient;
			if (ServerLastKnownPosSquare != null)
			{
				_serverLastKnownPosSquare_x = (short)ServerLastKnownPosSquare.x;
				_serverLastKnownPosSquare_y = (short)ServerLastKnownPosSquare.y;
			}
			else
			{
				_serverLastKnownPosSquare_x = -1;
				_serverLastKnownPosSquare_y = -1;
			}
			_RemainingHorizontalMovement = RemainingHorizontalMovement;
			_RemainingMovementWithQueuedAbility = RemainingMovementWithQueuedAbility;
			_QueuedMovementAllowsAbility = QueuedMovementAllowsAbility;
			_HasQueuedMovement = HasQueuedMovement();
			_HasQueuedChase = HasQueuedChase();
			_queuedMovementBitfield = ServerClientUtils.CreateBitfieldFromBools(_QueuedMovementAllowsAbility, _HasQueuedMovement, _HasQueuedChase, false, false, false, false, false);
			_queuedChaseTargetActorIndex = (sbyte)GameplayUtils.GetActorIndexOfActor(GetQueuedChaseTarget());
			_HitPoints = (short)HitPoints;
			_UnresolvedDamage = (short)UnresolvedDamage;
			_UnresolvedHealing = (short)UnresolvedHealing;
			_TechPoints = (short)TechPoints;
			_ReservedTechPoints = (short)ReservedTechPoints;
			_UnresolvedTechPointGain = (short)UnresolvedTechPointGain;
			_UnresolvedTechPointLoss = (short)UnresolvedTechPointLoss;
			_AbsorbPoints = (short)AbsorbPoints;
			_MechanicPoints = (short)MechanicPoints;
			_ExpectedHoTTotal = (short)ExpectedHoTTotal;
			_ExpectedHoTThisTurn = (short)ExpectedHoTThisTurn;
			bool _hasHoT = _ExpectedHoTTotal > 0 || _ExpectedHoTThisTurn > 0;
			_pointsBitfield = ServerClientUtils.CreateBitfieldFromBools(_UnresolvedDamage > 0, _UnresolvedHealing > 0, _UnresolvedTechPointGain > 0, _UnresolvedTechPointLoss > 0, _ReservedTechPoints != 0, _AbsorbPoints > 0, _MechanicPoints > 0, _hasHoT);
			_LastDeathTurn = LastDeathTurn;
			_lastSpawnTurn = m_lastSpawnTurn;
			_NextRespawnTurn = NextRespawnTurn;
			_SpawnerId = (sbyte)SpawnerId;
			_lineOfSightVisibleExceptionsCount = (sbyte)m_lineOfSightVisibleExceptions.Count;
			_HasBotController = HasBotController;
			_showInGameHud = m_showInGameHud;
			_VisibleTillEndOfPhase = VisibleTillEndOfPhase;
			_ignoreFromAbilityHits = m_ignoreFromAbilityHits;
			_alwaysHideNameplate = m_alwaysHideNameplate;
			_UiGameplayBitfield = ServerClientUtils.CreateBitfieldFromBools(_HasBotController, _showInGameHud, _VisibleTillEndOfPhase, _ignoreFromAbilityHits, _alwaysHideNameplate, false, false, false);
			stream.Serialize(ref _playerIndex);
			stream.Serialize(ref _actorIndex);
			stream.Serialize(ref _displayName);
			stream.Serialize(ref _team);
			stream.Serialize(ref _queuedMovementBitfield);
			stream.Serialize(ref _RemainingHorizontalMovement);
			stream.Serialize(ref _RemainingMovementWithQueuedAbility);
			stream.Serialize(ref _queuedChaseTargetActorIndex);
			stream.Serialize(ref _HitPoints);
			stream.Serialize(ref _TechPoints);
			stream.Serialize(ref _pointsBitfield);
			if (_UnresolvedDamage > 0)
			{
				stream.Serialize(ref _UnresolvedDamage);
			}
			if (_UnresolvedHealing > 0)
			{
				stream.Serialize(ref _UnresolvedHealing);
			}
			if (_UnresolvedTechPointGain > 0)
			{
				stream.Serialize(ref _UnresolvedTechPointGain);
			}
			if (_UnresolvedTechPointLoss > 0)
			{
				stream.Serialize(ref _UnresolvedTechPointLoss);
			}
			if (_ReservedTechPoints != 0)
			{
				stream.Serialize(ref _ReservedTechPoints);
			}
			if (_AbsorbPoints > 0)
			{
				stream.Serialize(ref _AbsorbPoints);
			}
			if (_MechanicPoints > 0)
			{
				stream.Serialize(ref _MechanicPoints);
			}
			if (_hasHoT)
			{
				stream.Serialize(ref _ExpectedHoTTotal);
				stream.Serialize(ref _ExpectedHoTThisTurn);
			}
			stream.Serialize(ref _LastDeathTurn);
			stream.Serialize(ref _lastSpawnTurn);
			stream.Serialize(ref _NextRespawnTurn);
			stream.Serialize(ref _SpawnerId);
			stream.Serialize(ref _UiGameplayBitfield);
			m_debugSerializeSizeBeforeVisualInfo = stream.Position - position;
			SerializeCharacterVisualInfo(stream, ref m_visualInfo);
			SerializeCharacterCardInfo(stream, ref m_selectedCards);
			SerializeCharacterModInfo(stream, ref m_selectedMods);
			SerializeCharacterAbilityVfxSwapInfo(stream, ref m_abilityVfxSwapInfo);
			m_debugSerializeSizeBeforeSpawnSquares = stream.Position - position;
			stream.Serialize(ref _lineOfSightVisibleExceptionsCount);
			using (List<ActorData>.Enumerator enumerator = m_lineOfSightVisibleExceptions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					short value30 = (short)current.ActorIndex;
					stream.Serialize(ref value30);
				}
			}
			stream.Serialize(ref _lastVisibleTurnToClient);
			stream.Serialize(ref _serverLastKnownPosSquare_x);
			stream.Serialize(ref _serverLastKnownPosSquare_y);
		}
		Team team;
		if (stream.isReading)
		{
			stream.Serialize(ref _playerIndex);
			stream.Serialize(ref _actorIndex);
			stream.Serialize(ref _displayName);
			stream.Serialize(ref _team);
			stream.Serialize(ref _queuedMovementBitfield);
			ServerClientUtils.GetBoolsFromBitfield(_queuedMovementBitfield, out _QueuedMovementAllowsAbility, out _HasQueuedMovement, out _HasQueuedChase);
			stream.Serialize(ref _RemainingHorizontalMovement);
			stream.Serialize(ref _RemainingMovementWithQueuedAbility);
			stream.Serialize(ref _queuedChaseTargetActorIndex);
			stream.Serialize(ref _HitPoints);
			stream.Serialize(ref _TechPoints);
			stream.Serialize(ref _pointsBitfield);
			bool _hasUnresolvedDamage = false;
			bool _hasUnresolvedHealing = false;
			bool _hasUnresolvedTechPointGain = false;
			bool _hasUnresolvedTechPointLoss = false;
			bool _hasReservedTechPoints = false;
			bool _hasAbsorbPoints = false;
			bool _hasMechanicPoints = false;
			bool out16 = false;
			ServerClientUtils.GetBoolsFromBitfield(_pointsBitfield, out _hasUnresolvedDamage, out _hasUnresolvedHealing, out _hasUnresolvedTechPointGain, out _hasUnresolvedTechPointLoss, out _hasReservedTechPoints, out _hasAbsorbPoints, out _hasMechanicPoints, out out16);
			if (_hasUnresolvedDamage)
			{
				stream.Serialize(ref _UnresolvedDamage);
			}
			if (_hasUnresolvedHealing)
			{
				stream.Serialize(ref _UnresolvedHealing);
			}
			if (_hasUnresolvedTechPointGain)
			{
				stream.Serialize(ref _UnresolvedTechPointGain);
			}
			if (_hasUnresolvedTechPointLoss)
			{
				stream.Serialize(ref _UnresolvedTechPointLoss);
			}
			if (_hasReservedTechPoints)
			{
				stream.Serialize(ref _ReservedTechPoints);
			}
			if (_hasAbsorbPoints)
			{
				stream.Serialize(ref _AbsorbPoints);
			}
			if (_hasMechanicPoints)
			{
				stream.Serialize(ref _MechanicPoints);
			}
			if (out16)
			{
				stream.Serialize(ref _ExpectedHoTTotal);
				stream.Serialize(ref _ExpectedHoTThisTurn);
			}
			stream.Serialize(ref _LastDeathTurn);
			stream.Serialize(ref _lastSpawnTurn);
			stream.Serialize(ref _NextRespawnTurn);
			stream.Serialize(ref _SpawnerId);
			stream.Serialize(ref _UiGameplayBitfield);
			ServerClientUtils.GetBoolsFromBitfield(_UiGameplayBitfield, out _HasBotController, out _showInGameHud, out _VisibleTillEndOfPhase, out _ignoreFromAbilityHits, out _alwaysHideNameplate);
			SerializeCharacterVisualInfo(stream, ref m_visualInfo);
			SerializeCharacterCardInfo(stream, ref m_selectedCards);
			SerializeCharacterModInfo(stream, ref m_selectedMods);
			SerializeCharacterAbilityVfxSwapInfo(stream, ref m_abilityVfxSwapInfo);
			PlayerIndex = _playerIndex;
			ActorIndex = _actorIndex;
			team = m_team;
			m_team = (Team)_team;
			SpawnerId = _SpawnerId;
			if (m_actorSkinPrefabLink == null || m_actorModelData == null)
			{
				PrefabResourceLink prefabResourceLink = null;
				CharacterResourceLink characterResourceLink = null;
				if (m_characterType > CharacterType.None)
				{
					characterResourceLink = GameWideData.Get().GetCharacterResourceLink(m_characterType);
				}
				if (characterResourceLink == null && NPCCoordinator.Get() != null)
				{
					characterResourceLink = NPCCoordinator.Get().GetNpcCharacterResourceLinkBySpawnerId(_SpawnerId);
				}
				if (characterResourceLink != null)
				{
					prefabResourceLink = characterResourceLink.GetHeroPrefabLinkFromSelection(m_visualInfo, out CharacterSkin _);
				}
				if (prefabResourceLink != null && !prefabResourceLink.IsEmpty)
				{
					GameObject prefab = prefabResourceLink.GetPrefab(true);
					if (prefab == null && !m_visualInfo.IsDefaultSelection())
					{
						CharacterVisualInfo visualInfo = m_visualInfo;
						visualInfo.ResetToDefault();
						prefabResourceLink = characterResourceLink.GetHeroPrefabLinkFromSelection(visualInfo, out CharacterSkin _);
					}
					bool addMasterSkinVfx = false;
					if (MasterSkinVfxData.Get() != null && MasterSkinVfxData.Get().m_addMasterSkinVfx && characterResourceLink.IsVisualInfoSelectionValid(m_visualInfo))
					{
						CharacterColor characterColor = characterResourceLink.GetCharacterColor(m_visualInfo);
						addMasterSkinVfx = characterColor.m_styleLevel == StyleLevelType.Mastery;
					}
					Initialize(prefabResourceLink, addMasterSkinVfx);
				}
				else
				{
					Log.Error(string.Concat("Failed to find character resource link for ", m_characterType, " with visual info ", m_visualInfo.ToString()));
					GameObject gameObject = GameFlowData.Get().m_availableCharacterResourceLinkPrefabs[0];
					CharacterResourceLink component = gameObject.GetComponent<CharacterResourceLink>();
					Initialize(component.m_skins[0].m_patterns[0].m_colors[0].m_heroPrefab, false);
				}
			}
			m_displayName = _displayName;
			if (initialState)
			{
				TeamSensitiveDataMatchmaker.Get().SetTeamSensitiveDataForActor(this);
			}
			stream.Serialize(ref _lineOfSightVisibleExceptionsCount);
			m_lineOfSightVisibleExceptions.Clear();
			for (int i = 0; i < _lineOfSightVisibleExceptionsCount; i++)
			{
				sbyte value31 = 0;
				stream.Serialize(ref value31);
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(value31);
				if (actorData != null)
				{
					m_lineOfSightVisibleExceptions.Add(actorData);
				}
			}
			stream.Serialize(ref _lastVisibleTurnToClient);
			stream.Serialize(ref _serverLastKnownPosSquare_x);
			stream.Serialize(ref _serverLastKnownPosSquare_y);
			if (_lastVisibleTurnToClient > m_lastVisibleTurnToClient)
			{
				m_lastVisibleTurnToClient = _lastVisibleTurnToClient;
			}
			if (_serverLastKnownPosSquare_x == -1 && _serverLastKnownPosSquare_y == -1)
			{
				ServerLastKnownPosSquare = null;
			}
			else
			{
				ServerLastKnownPosSquare = Board.Get().GetSquareFromIndex(_serverLastKnownPosSquare_x, _serverLastKnownPosSquare_y);
			}
			m_ignoreFromAbilityHits = _ignoreFromAbilityHits;
			m_alwaysHideNameplate = _alwaysHideNameplate;
			GetFogOfWar().MarkForRecalculateVisibility();
			m_showInGameHud = _showInGameHud;
			VisibleTillEndOfPhase = _VisibleTillEndOfPhase;
			if (!m_setTeam || team != m_team)
			{
				if (GameFlowData.Get() != null)
				{
					GameFlowData.Get().AddToTeam(this);
				}
				else
				{
					m_needAddToTeam = true;
				}
				if (TeamStatusDisplay.GetTeamStatusDisplay() != null)
				{
					TeamStatusDisplay.GetTeamStatusDisplay().RebuildTeamDisplay();
				}
				if (GameplayUtils.IsMinion(gameObject) && MinionManager.Get() != null)
				{
					if (m_setTeam)
					{
						MinionManager.Get().RemoveMinion(this);
						MinionManager.Get().AddMinion(this);
					}
					else
					{
						MinionManager.Get().AddMinion(this);
					}
				}
				m_setTeam = true;
			}
			UnresolvedDamage = _UnresolvedDamage;
			UnresolvedHealing = _UnresolvedHealing;
			ReservedTechPoints = _ReservedTechPoints;
			UnresolvedTechPointGain = _UnresolvedTechPointGain;
			UnresolvedTechPointLoss = _UnresolvedTechPointLoss;
			LastDeathTurn = _LastDeathTurn;
			m_lastSpawnTurn = _lastSpawnTurn;
			NextRespawnTurn = _NextRespawnTurn;
			HasBotController = _HasBotController;
			AbsorbPoints = _AbsorbPoints;
			TechPoints = _TechPoints;
			HitPoints = _HitPoints;
			MechanicPoints = _MechanicPoints;
			ExpectedHoTTotal = _ExpectedHoTTotal;
			ExpectedHoTThisTurn = _ExpectedHoTThisTurn;
			bool needToUpdateSquarsCanMoveTo = false;
			if (_RemainingHorizontalMovement != RemainingHorizontalMovement)
			{
				RemainingHorizontalMovement = _RemainingHorizontalMovement;
				needToUpdateSquarsCanMoveTo = true;
			}
			if (_RemainingMovementWithQueuedAbility != RemainingMovementWithQueuedAbility)
			{
				RemainingMovementWithQueuedAbility = _RemainingMovementWithQueuedAbility;
				needToUpdateSquarsCanMoveTo = true;
			}
			QueuedMovementAllowsAbility = _QueuedMovementAllowsAbility;
			if (m_queuedMovementRequest != _HasQueuedMovement)
			{
				m_queuedMovementRequest = _HasQueuedMovement;
				needToUpdateSquarsCanMoveTo = true;
			}
			if (m_queuedChaseRequest != _HasQueuedChase)
			{
				m_queuedChaseRequest = _HasQueuedChase;
				needToUpdateSquarsCanMoveTo = true;
			}
			ActorData actorOfActorIndex = GameplayUtils.GetActorOfActorIndex(_queuedChaseTargetActorIndex);
			if (m_queuedChaseTarget != actorOfActorIndex)
			{
				m_queuedChaseTarget = actorOfActorIndex;
			}
			if (needToUpdateSquarsCanMoveTo)
			{
				m_actorMovement.UpdateSquaresCanMoveTo();
			}
		}
		return m_serializeHelper.End(initialState, syncVarDirtyBits);
	}

	public static void SerializeCharacterVisualInfo(IBitStream stream, ref CharacterVisualInfo visualInfo)
	{
		sbyte value = (sbyte)visualInfo.skinIndex;
		sbyte value2 = (sbyte)visualInfo.patternIndex;
		short value3 = (short)visualInfo.colorIndex;
		stream.Serialize(ref value);
		stream.Serialize(ref value2);
		stream.Serialize(ref value3);
		if (stream.isReading)
		{
			visualInfo.skinIndex = value;
			visualInfo.patternIndex = value2;
			visualInfo.colorIndex = value3;
		}
	}

	public static void SerializeCharacterAbilityVfxSwapInfo(IBitStream stream, ref CharacterAbilityVfxSwapInfo abilityVfxSwapInfo)
	{
		short value = (short)abilityVfxSwapInfo.VfxSwapForAbility0;
		short value2 = (short)abilityVfxSwapInfo.VfxSwapForAbility1;
		short value3 = (short)abilityVfxSwapInfo.VfxSwapForAbility2;
		short value4 = (short)abilityVfxSwapInfo.VfxSwapForAbility3;
		short value5 = (short)abilityVfxSwapInfo.VfxSwapForAbility4;
		stream.Serialize(ref value);
		stream.Serialize(ref value2);
		stream.Serialize(ref value3);
		stream.Serialize(ref value4);
		stream.Serialize(ref value5);
		if (stream.isReading)
		{
			abilityVfxSwapInfo.VfxSwapForAbility0 = value;
			abilityVfxSwapInfo.VfxSwapForAbility1 = value2;
			abilityVfxSwapInfo.VfxSwapForAbility2 = value3;
			abilityVfxSwapInfo.VfxSwapForAbility3 = value4;
			abilityVfxSwapInfo.VfxSwapForAbility4 = value5;
		}
	}

	public static void SerializeCharacterCardInfo(IBitStream stream, ref CharacterCardInfo cardInfo)
	{
		sbyte value = 0;
		sbyte value2 = 0;
		sbyte value3 = 0;
		if (stream.isWriting)
		{
			value = (sbyte)cardInfo.PrepCard;
			value2 = (sbyte)cardInfo.DashCard;
			value3 = (sbyte)cardInfo.CombatCard;
			stream.Serialize(ref value);
			stream.Serialize(ref value2);
			stream.Serialize(ref value3);
		}
		else
		{
			stream.Serialize(ref value);
			stream.Serialize(ref value2);
			stream.Serialize(ref value3);
			cardInfo.PrepCard = (CardType)value;
			cardInfo.DashCard = (CardType)value2;
			cardInfo.CombatCard = (CardType)value3;
		}
	}

	public static void SerializeCharacterModInfo(IBitStream stream, ref CharacterModInfo modInfo)
	{
		sbyte value = -1;
		sbyte value2 = -1;
		sbyte value3 = -1;
		sbyte value4 = -1;
		sbyte value5 = -1;
		if (stream.isWriting)
		{
			value = (sbyte)modInfo.ModForAbility0;
			value2 = (sbyte)modInfo.ModForAbility1;
			value3 = (sbyte)modInfo.ModForAbility2;
			value4 = (sbyte)modInfo.ModForAbility3;
			value5 = (sbyte)modInfo.ModForAbility4;
			stream.Serialize(ref value);
			stream.Serialize(ref value2);
			stream.Serialize(ref value3);
			stream.Serialize(ref value4);
			stream.Serialize(ref value5);
		}
		else
		{
			stream.Serialize(ref value);
			stream.Serialize(ref value2);
			stream.Serialize(ref value3);
			stream.Serialize(ref value4);
			stream.Serialize(ref value5);
			modInfo.ModForAbility0 = value;
			modInfo.ModForAbility1 = value2;
			modInfo.ModForAbility2 = value3;
			modInfo.ModForAbility3 = value4;
			modInfo.ModForAbility4 = value5;
		}
	}

	// TODO maybe check if there is anything interesting in rouges serialization?
	//public override bool OnSerialize(NetworkWriter writer, bool forceAll)  // rogues
	//{
	//	bool result = base.OnSerialize(writer, forceAll);
	//	if (forceAll)
	//	{
	//		writer.WritePackedInt32(PlayerIndex);
	//		writer.Write(m_alwaysHideNameplate);
	//		GeneratedNetworkCode._WriteCharacterVisualInfo_None(writer, m_visualInfo);
	//		GeneratedNetworkCode._WriteCharacterAbilityVfxSwapInfo_None(writer, m_abilityVfxSwapInfo);
	//		writer.WritePackedInt32((int)m_team);
	//		writer.WritePackedInt32(m_lastVisibleTurnToClient);
	//		writer.WritePackedInt32(m_serverLastKnownPosX);
	//		writer.WritePackedInt32(m_serverLastKnownPosY);
	//		writer.Write(m_currentMovementFlags);
	//		writer.Write(m_queuedChaseTargetActorIndex);
	//		writer.Write(m_alertDist);
	//		writer.Write(m_suspend);
	//		GeneratedNetworkCode._WriteCharacterGearInfo_None(writer, m_selectedGear);
	//		GeneratedNetworkCode._WriteCharacterCardInfo_None(writer, m_selectedCards);
	//		writer.Write(m_displayName);
	//		writer.WritePackedInt32(m_actorIndex);
	//		writer.Write(m_showInGameHud);
	//		writer.WritePackedInt32(m_hitPoints);
	//		writer.WritePackedInt32(_unresolvedDamage);
	//		writer.WritePackedInt32(_unresolvedHealing);
	//		writer.WritePackedInt32(_unresolvedTechPointGain);
	//		writer.WritePackedInt32(_unresolvedTechPointLoss);
	//		writer.WritePackedInt32(m_serverExpectedHoTTotal);
	//		writer.WritePackedInt32(m_serverExpectedHoTThisTurn);
	//		writer.WritePackedInt32(m_techPoints);
	//		writer.WritePackedInt32(m_reservedTechPoints);
	//		writer.Write(m_ignoreFromAbilityHits);
	//		writer.WritePackedInt32(m_absorbPoints);
	//		writer.WritePackedInt32(m_mechanicPoints);
	//		writer.WritePackedInt32(m_spawnerId);
	//		writer.Write(m_remainingHorizontalMovement);
	//		writer.Write(m_remainingMovementWithQueuedAbility);
	//		writer.WritePackedInt32(m_lastDeathTurn);
	//		writer.WritePackedInt32(m_lastSpawnTurn);
	//		writer.WritePackedInt32(m_nextRespawnTurn);
	//		writer.Write(m_hasBotController);
	//		writer.WritePackedInt32(m_turnPriority);
	//		writer.Write(m_visibleTillEndOfPhase);
	//		return true;
	//	}
	//	writer.WritePackedUInt64(base.syncVarDirtyBits);
	//	if ((base.syncVarDirtyBits & 1UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(PlayerIndex);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 2UL) != 0UL)
	//	{
	//		writer.Write(m_alwaysHideNameplate);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 4UL) != 0UL)
	//	{
	//		GeneratedNetworkCode._WriteCharacterVisualInfo_None(writer, m_visualInfo);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 8UL) != 0UL)
	//	{
	//		GeneratedNetworkCode._WriteCharacterAbilityVfxSwapInfo_None(writer, m_abilityVfxSwapInfo);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 16UL) != 0UL)
	//	{
	//		writer.WritePackedInt32((int)m_team);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 32UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_lastVisibleTurnToClient);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 64UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_serverLastKnownPosX);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 128UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_serverLastKnownPosY);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 256UL) != 0UL)
	//	{
	//		writer.Write(m_currentMovementFlags);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 512UL) != 0UL)
	//	{
	//		writer.Write(m_queuedChaseTargetActorIndex);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 1024UL) != 0UL)
	//	{
	//		writer.Write(m_alertDist);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 2048UL) != 0UL)
	//	{
	//		writer.Write(m_suspend);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 4096UL) != 0UL)
	//	{
	//		GeneratedNetworkCode._WriteCharacterGearInfo_None(writer, m_selectedGear);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 8192UL) != 0UL)
	//	{
	//		GeneratedNetworkCode._WriteCharacterCardInfo_None(writer, m_selectedCards);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 16384UL) != 0UL)
	//	{
	//		writer.Write(m_displayName);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 32768UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_actorIndex);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 65536UL) != 0UL)
	//	{
	//		writer.Write(m_showInGameHud);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 131072UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_hitPoints);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 262144UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(_unresolvedDamage);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 524288UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(_unresolvedHealing);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 1048576UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(_unresolvedTechPointGain);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 2097152UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(_unresolvedTechPointLoss);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 4194304UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_serverExpectedHoTTotal);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 8388608UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_serverExpectedHoTThisTurn);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 16777216UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_techPoints);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 33554432UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_reservedTechPoints);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 67108864UL) != 0UL)
	//	{
	//		writer.Write(m_ignoreFromAbilityHits);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 134217728UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_absorbPoints);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 268435456UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_mechanicPoints);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 536870912UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_spawnerId);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 1073741824UL) != 0UL)
	//	{
	//		writer.Write(m_remainingHorizontalMovement);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 2147483648UL) != 0UL)
	//	{
	//		writer.Write(m_remainingMovementWithQueuedAbility);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 4294967296UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_lastDeathTurn);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 8589934592UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_lastSpawnTurn);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 17179869184UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_nextRespawnTurn);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 34359738368UL) != 0UL)
	//	{
	//		writer.Write(m_hasBotController);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 68719476736UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_turnPriority);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 137438953472UL) != 0UL)
	//	{
	//		writer.Write(m_visibleTillEndOfPhase);
	//		result = true;
	//	}
	//	return result;
	//}

	//public override void OnDeserialize(NetworkReader reader, bool initialState)  // rogues
	//{
	//	base.OnDeserialize(reader, initialState);
	//	if (initialState)
	//	{
	//		int num = reader.ReadPackedInt32();
	//		OnPlayerIndexUpdated(num);
	//		NetworkPlayerIndex = num;
	//		bool networkm_alwaysHideNameplate = reader.ReadBoolean();
	//		Networkm_alwaysHideNameplate = networkm_alwaysHideNameplate;
	//		CharacterVisualInfo networkm_visualInfo = GeneratedNetworkCode._ReadCharacterVisualInfo_None(reader);
	//		Networkm_visualInfo = networkm_visualInfo;
	//		CharacterAbilityVfxSwapInfo networkm_abilityVfxSwapInfo = GeneratedNetworkCode._ReadCharacterAbilityVfxSwapInfo_None(reader);
	//		Networkm_abilityVfxSwapInfo = networkm_abilityVfxSwapInfo;
	//		Team networkm_team = (Team)reader.ReadPackedInt32();
	//		Networkm_team = networkm_team;
	//		int networkm_lastVisibleTurnToClient = reader.ReadPackedInt32();
	//		Networkm_lastVisibleTurnToClient = networkm_lastVisibleTurnToClient;
	//		int networkm_serverLastKnownPosX = reader.ReadPackedInt32();
	//		Networkm_serverLastKnownPosX = networkm_serverLastKnownPosX;
	//		int networkm_serverLastKnownPosY = reader.ReadPackedInt32();
	//		Networkm_serverLastKnownPosY = networkm_serverLastKnownPosY;
	//		byte b = reader.ReadByte();
	//		OnCurrentMovementFlagsUpdated(b);
	//		Networkm_currentMovementFlags = b;
	//		sbyte networkm_queuedChaseTargetActorIndex = reader.ReadSByte();
	//		Networkm_queuedChaseTargetActorIndex = networkm_queuedChaseTargetActorIndex;
	//		float networkm_alertDist = reader.ReadSingle();
	//		Networkm_alertDist = networkm_alertDist;
	//		bool networkm_suspend = reader.ReadBoolean();
	//		Networkm_suspend = networkm_suspend;
	//		CharacterGearInfo networkm_selectedGear = GeneratedNetworkCode._ReadCharacterGearInfo_None(reader);
	//		Networkm_selectedGear = networkm_selectedGear;
	//		CharacterCardInfo networkm_selectedCards = GeneratedNetworkCode._ReadCharacterCardInfo_None(reader);
	//		Networkm_selectedCards = networkm_selectedCards;
	//		string networkm_displayName = reader.ReadString();
	//		Networkm_displayName = networkm_displayName;
	//		int num2 = reader.ReadPackedInt32();
	//		OnActorIndexUpdated(num2);
	//		Networkm_actorIndex = num2;
	//		bool networkm_showInGameHud = reader.ReadBoolean();
	//		Networkm_showInGameHud = networkm_showInGameHud;
	//		int networkm_hitPoints = reader.ReadPackedInt32();
	//		Networkm_hitPoints = networkm_hitPoints;
	//		int network_unresolvedDamage = reader.ReadPackedInt32();
	//		Network_unresolvedDamage = network_unresolvedDamage;
	//		int network_unresolvedHealing = reader.ReadPackedInt32();
	//		Network_unresolvedHealing = network_unresolvedHealing;
	//		int network_unresolvedTechPointGain = reader.ReadPackedInt32();
	//		Network_unresolvedTechPointGain = network_unresolvedTechPointGain;
	//		int network_unresolvedTechPointLoss = reader.ReadPackedInt32();
	//		Network_unresolvedTechPointLoss = network_unresolvedTechPointLoss;
	//		int networkm_serverExpectedHoTTotal = reader.ReadPackedInt32();
	//		Networkm_serverExpectedHoTTotal = networkm_serverExpectedHoTTotal;
	//		int networkm_serverExpectedHoTThisTurn = reader.ReadPackedInt32();
	//		Networkm_serverExpectedHoTThisTurn = networkm_serverExpectedHoTThisTurn;
	//		int networkm_techPoints = reader.ReadPackedInt32();
	//		Networkm_techPoints = networkm_techPoints;
	//		int networkm_reservedTechPoints = reader.ReadPackedInt32();
	//		Networkm_reservedTechPoints = networkm_reservedTechPoints;
	//		bool networkm_ignoreFromAbilityHits = reader.ReadBoolean();
	//		Networkm_ignoreFromAbilityHits = networkm_ignoreFromAbilityHits;
	//		int networkm_absorbPoints = reader.ReadPackedInt32();
	//		Networkm_absorbPoints = networkm_absorbPoints;
	//		int networkm_mechanicPoints = reader.ReadPackedInt32();
	//		Networkm_mechanicPoints = networkm_mechanicPoints;
	//		int num3 = reader.ReadPackedInt32();
	//		OnSpawnerIdUpdated(num3);
	//		Networkm_spawnerId = num3;
	//		float networkm_remainingHorizontalMovement = reader.ReadSingle();
	//		Networkm_remainingHorizontalMovement = networkm_remainingHorizontalMovement;
	//		float networkm_remainingMovementWithQueuedAbility = reader.ReadSingle();
	//		Networkm_remainingMovementWithQueuedAbility = networkm_remainingMovementWithQueuedAbility;
	//		int networkm_lastDeathTurn = reader.ReadPackedInt32();
	//		Networkm_lastDeathTurn = networkm_lastDeathTurn;
	//		int networkm_lastSpawnTurn = reader.ReadPackedInt32();
	//		Networkm_lastSpawnTurn = networkm_lastSpawnTurn;
	//		int networkm_nextRespawnTurn = reader.ReadPackedInt32();
	//		Networkm_nextRespawnTurn = networkm_nextRespawnTurn;
	//		bool networkm_hasBotController = reader.ReadBoolean();
	//		Networkm_hasBotController = networkm_hasBotController;
	//		int networkm_turnPriority = reader.ReadPackedInt32();
	//		Networkm_turnPriority = networkm_turnPriority;
	//		bool networkm_visibleTillEndOfPhase = reader.ReadBoolean();
	//		Networkm_visibleTillEndOfPhase = networkm_visibleTillEndOfPhase;
	//		return;
	//	}
	//	long num4 = (long)reader.ReadPackedUInt64();
	//	if ((num4 & 1L) != 0L)
	//	{
	//		int num5 = reader.ReadPackedInt32();
	//		OnPlayerIndexUpdated(num5);
	//		NetworkPlayerIndex = num5;
	//	}
	//	if ((num4 & 2L) != 0L)
	//	{
	//		bool networkm_alwaysHideNameplate2 = reader.ReadBoolean();
	//		Networkm_alwaysHideNameplate = networkm_alwaysHideNameplate2;
	//	}
	//	if ((num4 & 4L) != 0L)
	//	{
	//		CharacterVisualInfo networkm_visualInfo2 = GeneratedNetworkCode._ReadCharacterVisualInfo_None(reader);
	//		Networkm_visualInfo = networkm_visualInfo2;
	//	}
	//	if ((num4 & 8L) != 0L)
	//	{
	//		CharacterAbilityVfxSwapInfo networkm_abilityVfxSwapInfo2 = GeneratedNetworkCode._ReadCharacterAbilityVfxSwapInfo_None(reader);
	//		Networkm_abilityVfxSwapInfo = networkm_abilityVfxSwapInfo2;
	//	}
	//	if ((num4 & 16L) != 0L)
	//	{
	//		Team networkm_team2 = (Team)reader.ReadPackedInt32();
	//		Networkm_team = networkm_team2;
	//	}
	//	if ((num4 & 32L) != 0L)
	//	{
	//		int networkm_lastVisibleTurnToClient2 = reader.ReadPackedInt32();
	//		Networkm_lastVisibleTurnToClient = networkm_lastVisibleTurnToClient2;
	//	}
	//	if ((num4 & 64L) != 0L)
	//	{
	//		int networkm_serverLastKnownPosX2 = reader.ReadPackedInt32();
	//		Networkm_serverLastKnownPosX = networkm_serverLastKnownPosX2;
	//	}
	//	if ((num4 & 128L) != 0L)
	//	{
	//		int networkm_serverLastKnownPosY2 = reader.ReadPackedInt32();
	//		Networkm_serverLastKnownPosY = networkm_serverLastKnownPosY2;
	//	}
	//	if ((num4 & 256L) != 0L)
	//	{
	//		byte b2 = reader.ReadByte();
	//		OnCurrentMovementFlagsUpdated(b2);
	//		Networkm_currentMovementFlags = b2;
	//	}
	//	if ((num4 & 512L) != 0L)
	//	{
	//		sbyte networkm_queuedChaseTargetActorIndex2 = reader.ReadSByte();
	//		Networkm_queuedChaseTargetActorIndex = networkm_queuedChaseTargetActorIndex2;
	//	}
	//	if ((num4 & 1024L) != 0L)
	//	{
	//		float networkm_alertDist2 = reader.ReadSingle();
	//		Networkm_alertDist = networkm_alertDist2;
	//	}
	//	if ((num4 & 2048L) != 0L)
	//	{
	//		bool networkm_suspend2 = reader.ReadBoolean();
	//		Networkm_suspend = networkm_suspend2;
	//	}
	//	if ((num4 & 4096L) != 0L)
	//	{
	//		CharacterGearInfo networkm_selectedGear2 = GeneratedNetworkCode._ReadCharacterGearInfo_None(reader);
	//		Networkm_selectedGear = networkm_selectedGear2;
	//	}
	//	if ((num4 & 8192L) != 0L)
	//	{
	//		CharacterCardInfo networkm_selectedCards2 = GeneratedNetworkCode._ReadCharacterCardInfo_None(reader);
	//		Networkm_selectedCards = networkm_selectedCards2;
	//	}
	//	if ((num4 & 16384L) != 0L)
	//	{
	//		string networkm_displayName2 = reader.ReadString();
	//		Networkm_displayName = networkm_displayName2;
	//	}
	//	if ((num4 & 32768L) != 0L)
	//	{
	//		int num6 = reader.ReadPackedInt32();
	//		OnActorIndexUpdated(num6);
	//		Networkm_actorIndex = num6;
	//	}
	//	if ((num4 & 65536L) != 0L)
	//	{
	//		bool networkm_showInGameHud2 = reader.ReadBoolean();
	//		Networkm_showInGameHud = networkm_showInGameHud2;
	//	}
	//	if ((num4 & 131072L) != 0L)
	//	{
	//		int networkm_hitPoints2 = reader.ReadPackedInt32();
	//		Networkm_hitPoints = networkm_hitPoints2;
	//	}
	//	if ((num4 & 262144L) != 0L)
	//	{
	//		int network_unresolvedDamage2 = reader.ReadPackedInt32();
	//		Network_unresolvedDamage = network_unresolvedDamage2;
	//	}
	//	if ((num4 & 524288L) != 0L)
	//	{
	//		int network_unresolvedHealing2 = reader.ReadPackedInt32();
	//		Network_unresolvedHealing = network_unresolvedHealing2;
	//	}
	//	if ((num4 & 1048576L) != 0L)
	//	{
	//		int network_unresolvedTechPointGain2 = reader.ReadPackedInt32();
	//		Network_unresolvedTechPointGain = network_unresolvedTechPointGain2;
	//	}
	//	if ((num4 & 2097152L) != 0L)
	//	{
	//		int network_unresolvedTechPointLoss2 = reader.ReadPackedInt32();
	//		Network_unresolvedTechPointLoss = network_unresolvedTechPointLoss2;
	//	}
	//	if ((num4 & 4194304L) != 0L)
	//	{
	//		int networkm_serverExpectedHoTTotal2 = reader.ReadPackedInt32();
	//		Networkm_serverExpectedHoTTotal = networkm_serverExpectedHoTTotal2;
	//	}
	//	if ((num4 & 8388608L) != 0L)
	//	{
	//		int networkm_serverExpectedHoTThisTurn2 = reader.ReadPackedInt32();
	//		Networkm_serverExpectedHoTThisTurn = networkm_serverExpectedHoTThisTurn2;
	//	}
	//	if ((num4 & 16777216L) != 0L)
	//	{
	//		int networkm_techPoints2 = reader.ReadPackedInt32();
	//		Networkm_techPoints = networkm_techPoints2;
	//	}
	//	if ((num4 & 33554432L) != 0L)
	//	{
	//		int networkm_reservedTechPoints2 = reader.ReadPackedInt32();
	//		Networkm_reservedTechPoints = networkm_reservedTechPoints2;
	//	}
	//	if ((num4 & 67108864L) != 0L)
	//	{
	//		bool networkm_ignoreFromAbilityHits2 = reader.ReadBoolean();
	//		Networkm_ignoreFromAbilityHits = networkm_ignoreFromAbilityHits2;
	//	}
	//	if ((num4 & 134217728L) != 0L)
	//	{
	//		int networkm_absorbPoints2 = reader.ReadPackedInt32();
	//		Networkm_absorbPoints = networkm_absorbPoints2;
	//	}
	//	if ((num4 & 268435456L) != 0L)
	//	{
	//		int networkm_mechanicPoints2 = reader.ReadPackedInt32();
	//		Networkm_mechanicPoints = networkm_mechanicPoints2;
	//	}
	//	if ((num4 & 536870912L) != 0L)
	//	{
	//		int num7 = reader.ReadPackedInt32();
	//		OnSpawnerIdUpdated(num7);
	//		Networkm_spawnerId = num7;
	//	}
	//	if ((num4 & 1073741824L) != 0L)
	//	{
	//		float networkm_remainingHorizontalMovement2 = reader.ReadSingle();
	//		Networkm_remainingHorizontalMovement = networkm_remainingHorizontalMovement2;
	//	}
	//	if ((num4 & 2147483648L) != 0L)
	//	{
	//		float networkm_remainingMovementWithQueuedAbility2 = reader.ReadSingle();
	//		Networkm_remainingMovementWithQueuedAbility = networkm_remainingMovementWithQueuedAbility2;
	//	}
	//	if ((num4 & 4294967296L) != 0L)
	//	{
	//		int networkm_lastDeathTurn2 = reader.ReadPackedInt32();
	//		Networkm_lastDeathTurn = networkm_lastDeathTurn2;
	//	}
	//	if ((num4 & 8589934592L) != 0L)
	//	{
	//		int networkm_lastSpawnTurn2 = reader.ReadPackedInt32();
	//		Networkm_lastSpawnTurn = networkm_lastSpawnTurn2;
	//	}
	//	if ((num4 & 17179869184L) != 0L)
	//	{
	//		int networkm_nextRespawnTurn2 = reader.ReadPackedInt32();
	//		Networkm_nextRespawnTurn = networkm_nextRespawnTurn2;
	//	}
	//	if ((num4 & 34359738368L) != 0L)
	//	{
	//		bool networkm_hasBotController2 = reader.ReadBoolean();
	//		Networkm_hasBotController = networkm_hasBotController2;
	//	}
	//	if ((num4 & 68719476736L) != 0L)
	//	{
	//		int networkm_turnPriority2 = reader.ReadPackedInt32();
	//		Networkm_turnPriority = networkm_turnPriority2;
	//	}
	//	if ((num4 & 137438953472L) != 0L)
	//	{
	//		bool networkm_visibleTillEndOfPhase2 = reader.ReadBoolean();
	//		Networkm_visibleTillEndOfPhase = networkm_visibleTillEndOfPhase2;
	//	}
	//}

	// added in rogues
	//public static bool DebugTraceSerializeSize
	//{
	//	get
	//	{
	//		return false;
	//	}
	//}

	//added in rogues
#if SERVER
	private void InitModel()
	{
		if (m_actorSkinPrefabLink == null || m_actorModelData == null)
		{
			PrefabResourceLink prefabResourceLink = null;
			CharacterResourceLink characterResourceLink = null;
			if (NPCCoordinator.Get() != null && m_spawnerId >= 0)
			{
				characterResourceLink = NPCCoordinator.Get().GetNpcCharacterResourceLinkBySpawnerId(m_spawnerId);
			}
			if (characterResourceLink == null && m_characterType > CharacterType.None)
			{
				characterResourceLink = GameWideData.Get().GetCharacterResourceLink(m_characterType);
			}
			if (characterResourceLink != null)
			{
				CharacterSkin characterSkin;
				prefabResourceLink = characterResourceLink.GetHeroPrefabLinkFromSelection(m_visualInfo, out characterSkin);
			}
			if (prefabResourceLink == null || prefabResourceLink.IsEmpty)
			{
				Log.Error($"Failed to find character resource link for {m_characterType} with visual info {m_visualInfo}");
				CharacterResourceLink component = GameFlowData.Get().m_availableCharacterResourceLinkPrefabs[0].GetComponent<CharacterResourceLink>();
				Initialize(component.m_skins[0].m_patterns[0].m_colors[0].m_heroPrefab, true); // TODO LOW no addMasterSkinVfx in rogues -- so true is a gamble
			}
			else
			{
				if (prefabResourceLink.GetPrefab(true) == null && !m_visualInfo.IsDefaultSelection())
				{
					CharacterVisualInfo visualInfo = m_visualInfo;
					visualInfo.ResetToDefault();
					CharacterSkin characterSkin2;
					prefabResourceLink = characterResourceLink.GetHeroPrefabLinkFromSelection(visualInfo, out characterSkin2);
				}
				Initialize(prefabResourceLink, true); // TODO LOW no addMasterSkinVfx in rogues -- so true is a gamble
			}
			TeamSensitiveDataMatchmaker.Get().SetTeamSensitiveDataForActor(this);
		}
	}
#endif

	public GridPos GetGridPos()
	{
		GridPos result = default(GridPos);
		if (GetCurrentBoardSquare())
		{
			result = GetCurrentBoardSquare().GetGridPos();
			result.height++;
		}
		return result;
	}

	public bool CanMoveToBoardSquare(int x, int y)
	{
		return m_actorMovement.CanMoveToBoardSquare(x, y);
	}

	public bool CanMoveToBoardSquare(BoardSquare dest)
	{
		return m_actorMovement.CanMoveToBoardSquare(dest);
	}

	// added in rogues
#if SERVER
	public void AssignToInitialBoardSquare(BoardSquare initialSquare)
	{
		TeleportToBoardSquare(initialSquare, Vector3.zero, TeleportType.InitialSpawn, null, 20f, MovementType.Teleport, GameEventManager.EventType.Invalid, null);
		gameObject.SendMessage("OnAssignedToInitialBoardSquare", 1);
		m_initialSpawnSquare = initialSquare;

		// custom
		ServerLastKnownPosSquare = initialSquare;
		// rogues
		//Networkm_serverLastKnownPosX = initialSquare.x;
		//      Networkm_serverLastKnownPosY = initialSquare.y;
	}
#endif

	public void ClearFacingDirectionAfterMovement()
	{
		SetFacingDirectionAfterMovement(Vector3.zero);
	}

	public void SetFacingDirectionAfterMovement(Vector3 facingDirAfterMovement)
	{
		if (m_facingDirAfterMovement != facingDirAfterMovement)
		{
			m_facingDirAfterMovement = facingDirAfterMovement;
			if (NetworkServer.active)
			{
				if (m_teamSensitiveData_friendly != null)
				{
					m_teamSensitiveData_friendly.FacingDirAfterMovement = m_facingDirAfterMovement;
				}
				if (m_teamSensitiveData_hostile != null)  // removed in rogues
				{
					m_teamSensitiveData_hostile.FacingDirAfterMovement = m_facingDirAfterMovement;
				}
			}
		}
	}

	public Vector3 GetFacingDirectionAfterMovement()
	{
		return m_facingDirAfterMovement;
	}

	// added in rogues
#if SERVER
	public void TeleportToBoardSquare(BoardSquare dest, Vector3 facingDirAfterMovement, ActorData.TeleportType teleportType, BoardSquarePathInfo teleportPath, float speed = 20f, ActorData.MovementType movementType = MovementType.Teleport, GameEventManager.EventType waitForEvent = GameEventManager.EventType.Invalid, BoardSquare startSquare = null)
	{
		if (teleportPath == null)
		{
			if (startSquare == null && GetCurrentBoardSquare() != null)
			{
				teleportPath = MovementUtils.Build2PointTeleportPath(this, dest);
			}
			else if (startSquare != null)
			{
				teleportPath = MovementUtils.Build2PointTeleportPath(startSquare, dest);
			}
		}
		for (BoardSquarePathInfo boardSquarePathInfo = teleportPath; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			boardSquarePathInfo.segmentMovementSpeed = speed;
		}
		if (waitForEvent != GameEventManager.EventType.Invalid)
		{
			QueueMoveToBoardSquareOnEvent(dest, movementType, teleportType, teleportPath, facingDirAfterMovement, waitForEvent);
			return;
		}
		SetFacingDirectionAfterMovement(facingDirAfterMovement);
		BroadcastMoveToBoardSquare(dest, movementType, teleportPath, teleportType, GameEventManager.EventType.Invalid, false);
	}
#endif

	// added in rogues
#if SERVER
	internal void QueueMoveToBoardSquareOnEvent(BoardSquare dest, ActorData.MovementType movementType, ActorData.TeleportType teleportType, BoardSquarePathInfo path, Vector3 facingDirAfterMovement, GameEventManager.EventType eventType = GameEventManager.EventType.TheatricsEvasionMoveStart)
	{
		SetFacingDirectionAfterMovement(facingDirAfterMovement);
		m_serverMovementWaitForEvent = eventType;
		m_serverMovementDestination = dest;
		m_serverMovementPath = path;
		if (NetworkServer.active)
		{
			if (m_teamSensitiveData_friendly != null)
			{
				m_teamSensitiveData_friendly.BroadcastMovement(eventType, GetGridPos(), dest, movementType, teleportType, path);
			}
			// custom
			if (m_teamSensitiveData_hostile != null)
			{
				m_teamSensitiveData_hostile.BroadcastMovement(eventType, GetGridPos(), dest, movementType, teleportType, path);
			}
			// end custom
		}
	}
#endif

	public void OnMovementWhileDisappeared(MovementType movementType)
	{
		Debug.Log(DebugNameString() + ": calling OnMovementWhileDisappeared.");
		if (ClientMovementManager.Get() != null)
		{
			ClientMovementManager.Get().OnActorMoveStart_Disappeared(this, movementType);
		}
		if (GetCurrentBoardSquare() != null && GetCurrentBoardSquare().occupant == gameObject)
		{
			UnoccupyCurrentBoardSquare();
		}
		m_actorMovement.ClearPath();
		SetCurrentBoardSquare(null);
		GetFogOfWar().MarkForRecalculateVisibility();
	}

	// server-only
#if SERVER
	public void BroadcastMoveToBoardSquare(BoardSquare dest, MovementType movementType, BoardSquarePathInfo path, TeleportType teleportType, GameEventManager.EventType fromEvent = GameEventManager.EventType.Invalid, bool moverWillDisappear = false)
	{
		m_serverMovementPath = path;
		m_serverMovementDestination = dest;
		m_disappearingAfterCurrentMovement = moverWillDisappear;
		if (NetworkServer.active)
		{
			m_serverMovementWaitForEvent = fromEvent;
			if (m_teamSensitiveData_friendly != null)
			{
				m_teamSensitiveData_friendly.BroadcastMovement(fromEvent, GetGridPos(), dest, movementType, teleportType, path);
			}
			// custom
			if (m_teamSensitiveData_hostile != null)
			{
				m_teamSensitiveData_hostile.BroadcastMovement(fromEvent, GetGridPos(), dest, movementType, teleportType, path);
			}
			// end custom
		}
	}
#endif

	public void MoveToBoardSquareLocal(BoardSquare dest, MovementType movementType, BoardSquarePathInfo path, bool moverWillDisappear)
	{
		Log.Info($"MoveToBoardSquareLocal {DisplayName} {CurrentBoardSquare?.GetGridPos()} -> {dest?.GetGridPos()} {movementType} moverWillDisappear={moverWillDisappear}\n{path?.GetDebugPathStringToEnd("")}");  // custom debug
		m_disappearingAfterCurrentMovement = moverWillDisappear;
		if (dest == null)
		{
			if (moverWillDisappear && path == null)
			{
				UnoccupyCurrentBoardSquare();
				SetCurrentBoardSquare(null);
				ForceUpdateIsVisibleToClientCache();  // removed in rogues
				ForceUpdateActorModelVisibility();  // removed in rogues
				return;
			}
			Log.Error($"Actor {DisplayName} in MoveToBoardSquare has null destination (movementType = {movementType})");
			return;
		}
		if (path == null && movementType != MovementType.Teleport)
		{
			Log.Error($"Actor {DisplayName} in MoveToBoardSquare has null path (movementType = {movementType})");
			return;
		}
		if (ServerClientUtils.GetCurrentAbilityPhase() == AbilityPriority.Evasion)  // removed in rogues
		{
			MovedForEvade = true;
		}
		bool willDieAtEnd = path != null && path.WillDieAtEnd();
		BoardSquare endpoint = path != null && path.GetPathEndpoint() != null && path.GetPathEndpoint().square != null
			? path.GetPathEndpoint().square
			: dest;
		if (ClientMovementManager.Get() != null)
		{
			ClientMovementManager.Get().OnActorMoveStart_ClientMovementManager(this, endpoint, movementType, path);
			ClientResolutionManager.Get().OnActorMoveStart_ClientResolutionManager(this, path);
			// TODO added in rogues -- check this
			//if (movementType == ActorData.MovementType.Normal)
			//{
			//	if (HUD_UI.Get() != null)
			//	{
			//		HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.AddUsedActionToNotification(null, this);
			//	}
			//	ActorData clientActor = GameFlowData.ClientActor;
			//	if (!IsActorVisibleToClient() && clientActor != null && !clientActor.IsDead())
			//	{
			//		InterfaceManager.Get().DisplayAlert(StringUtil.TR("HiddenAction", "Global"), Color.red, 2f, false, 0);
			//	}
			//}
		}

		// TODO added in rogues -- check this
		//if (this == GameFlowData.ClientActor && CameraManager.Get() != null)
		//{
		//	CameraManager.Get().SetTargetObject(base.gameObject, CameraManager.CameraTargetReason.MoveStart);
		//}

		if (GetCurrentBoardSquare() != null && GetCurrentBoardSquare().occupant == gameObject)
		{
			UnoccupyCurrentBoardSquare();
		}
		BoardSquare currentBoardSquare = CurrentBoardSquare;
		if (movementType == MovementType.Teleport)
		{
			m_actorMovement.ClearPath();
		}
		if (!willDieAtEnd && !moverWillDisappear)
		{
			SetCurrentBoardSquare(dest);
			if (GetCurrentBoardSquare() != null)
			{
				OccupyCurrentBoardSquare();
			}
		}
		else
		{
			SetCurrentBoardSquare(null);
			SetMostRecentDeathSquare(dest);
		}
		BoardSquarePathInfo boardSquarePathInfo = null;
		if (movementType == MovementType.Teleport)
		{
			ForceUpdateIsVisibleToClientCache();
			ForceUpdateActorModelVisibility();  // removed in rogues
			SetTransformPositionToSquare(dest);
			m_actorMovement.ClearPath();
			if (m_actorCover)
			{
				m_actorCover.RecalculateCover();
			}
			UpdateFacingAfterMovement();
			if (currentBoardSquare != null)
			{
				boardSquarePathInfo = MovementUtils.Build2PointTeleportPath(currentBoardSquare, dest);
				boardSquarePathInfo = boardSquarePathInfo.next;
				if (ClientClashManager.Get() != null)
				{
					ClientClashManager.Get().OnActorMoved_ClientClashManager(this, boardSquarePathInfo);
				}
				if (ClientResolutionManager.Get() != null)
				{
					ClientResolutionManager.Get().OnActorMoved_ClientResolutionManager(this, boardSquarePathInfo);
				}
			}
		}
		else if (movementType == MovementType.Normal
			|| movementType == MovementType.Flight
			|| movementType == MovementType.WaypointFlight)
		{
			if (m_actorCover)
			{
				m_actorCover.DisableCover();
			}
			if (path == null)
			{
				path = new BoardSquarePathInfo
				{
					square = currentBoardSquare,
					prev = null
				};
				path.next = new BoardSquarePathInfo
				{
					square = dest,
					prev = path,
					next = null
				};
			}
			m_actorMovement.BeginTravellingAlongPath(path, movementType);
			m_actorMovement.UpdatePosition();
		}
		else if (movementType == MovementType.Knockback
			|| movementType == MovementType.Charge)
		{
#if SERVER
			if (NetworkServer.active)  // server-only -- added in rogues
			{
				if (movementType == MovementType.Knockback)
				{
					ServerActionBuffer.Get().RemoveMovementRequestsDueToKnockback(this);
				}
				ActorBehavior actorBehavior = GetActorBehavior();
				if (actorBehavior != null)
				{
					if (movementType == MovementType.Charge)
					{
						actorBehavior.CurrentTurn.Charged = true;
					}
					else if (movementType == MovementType.Knockback)
					{
						actorBehavior.CurrentTurn.KnockedBack = true;
					}
					actorBehavior.CurrentTurn.Path = path;
				}
				else
				{
					Log.Error($"Actor {DisplayName} in MoveToBoardSquare has no behavior component");
				}
			}
#endif
			if (m_actorCover)
			{
				m_actorCover.DisableCover();
			}
			m_actorMovement.BeginChargeOrKnockback(currentBoardSquare, dest, path, movementType);
			m_actorMovement.UpdatePosition();
			if (!willDieAtEnd && !moverWillDisappear && path.square == dest && path.next == null)
			{
				UpdateFacingAfterMovement();
			}
			if (movementType == MovementType.Knockback)
			{
				KnockbackMoveStarted = true;
			}
		}
		// server-only? -- added in rogues
#if SERVER
		if (GetPassiveData() != null)
		{
			if (path != null)
			{
				GetPassiveData().OnMovementStart(currentBoardSquare, path, movementType);
			}
			else if (boardSquarePathInfo != null)
			{
				GetPassiveData().OnMovementStart(currentBoardSquare, boardSquarePathInfo, movementType);
			}
		}
#endif
		m_actorMovement.UpdateSquaresCanMoveTo();
		GetFogOfWar().MarkForRecalculateVisibility();
	}

	// server-only? -- added in rogues
#if SERVER
	public void OnServerLastKnownPosUpdateBegin()
	{
		PassiveData passiveData = GetPassiveData();
		if (passiveData != null)
		{
			passiveData.OnServerLastKnownPosUpdateBegin();
		}
	}
#endif

	// server-only? -- added in rogues
#if SERVER
	public void OnServerLastKnownPosUpdateEnd()
	{
		PassiveData passiveData = GetPassiveData();
		if (passiveData != null)
		{
			passiveData.OnServerLastKnownPosUpdateEnd();
		}
	}
#endif

	public void AppearAtBoardSquare(BoardSquare dest)
	{
		if (dest == null)
		{
			Log.Error($"Actor {DisplayName} in AppearAtBoardSquare has null destination)");
			return;
		}
		if (GetCurrentBoardSquare() != null && GetCurrentBoardSquare().occupant == gameObject)
		{
			UnoccupyCurrentBoardSquare();
		}
		SetCurrentBoardSquare(dest);
		SetTransformPositionToSquare(dest);
		if (GetCurrentBoardSquare() != null)
		{
			OccupyCurrentBoardSquare();
		}
	}

	public void SetTransformPositionToSquare(BoardSquare refSquare)
	{
		if (refSquare != null)
		{
			SetTransformPositionToVector(refSquare.GetOccupantRefPos());
		}
	}

	public void SetTransformPositionToVector(Vector3 newPos)
	{
		if (transform.position != newPos)
		{
			BoardSquare boardSquare = Board.Get().GetSquareFromVec3(transform.position);
			BoardSquare boardSquare2 = Board.Get().GetSquareFromVec3(newPos);
			if (boardSquare != boardSquare2 && boardSquare != null)
			{
				PreviousBoardSquarePosition = boardSquare.ToVector3();
			}
			transform.position = newPos;
		}
	}

	public void UnoccupyCurrentBoardSquare()
	{
		if (GetCurrentBoardSquare() != null && GetCurrentBoardSquare().occupant == gameObject)
		{
			GetCurrentBoardSquare().occupant = null;
		}
	}

	public BoardSquare GetTravelBoardSquare()
	{
		return m_actorMovement.GetTravelBoardSquare();
	}

	public BoardSquare GetCurrentBoardSquare()
	{
		return CurrentBoardSquare;
	}

	public BoardSquare GetMostRecentDeathSquare()
	{
		return m_mostRecentDeathSquare;
	}

	public void SetMostRecentDeathSquare(BoardSquare square)
	{
		m_mostRecentDeathSquare = square;
	}

	// added in rogues
#if SERVER
	public void SetSquareAtPhaseStart(BoardSquare square)
	{
		m_squareAtPhaseStart = square;
	}
#endif

	// added in rogues
#if SERVER
	public BoardSquare GetSquareAtPhaseStart()
	{
		if (m_squareAtPhaseStart == null && !IsDead())
		{
			Debug.LogError("Trying to get square at phase start when it's not valid");
		}
		return m_squareAtPhaseStart;
	}
#endif

	// added in rogues
	// TODO check if it is set properly
#if SERVER
	public BoardSquare SquareAtResolveStart
	{
		get
		{
			return m_squareAtResolveStart;
		}
		set
		{
			if (m_squareAtResolveStart != value)
			{
				m_squareAtResolveStart = value;
			}
		}
	}
#endif


	// added in rogues
#if SERVER
	public void SetSquareRequestedForMovementMetrics(BoardSquare square)
	{
		m_squareRequestedForMovementMetrics = square;
	}
#endif

	// added in rogues
#if SERVER
	public BoardSquare GetSquareRequestedForMovementMetrics()
	{
		return m_squareRequestedForMovementMetrics;
	}
#endif

	// server-only -- added in rogues
#if SERVER
	public BoardSquare GetServerMoveRequestStartSquare()
	{
		BoardSquare boardSquare = null;
		if (ServerActionBuffer.Get() != null)
		{
			boardSquare = ServerActionBuffer.Get().GetModifiedMoveStartSquareFromAbilities(this);
		}
		if (boardSquare == null)
		{
			boardSquare = GetCurrentBoardSquare();
		}
		return boardSquare;
	}
#endif

	public void OccupyCurrentBoardSquare()
	{
		if (GetCurrentBoardSquare() != null)
		{
			GetCurrentBoardSquare().occupant = gameObject;
		}
	}

	private void SetCurrentBoardSquare(BoardSquare square)
	{
		if (square != CurrentBoardSquare)
		{
#if SERVER
			if (NetworkServer.active) // server-only
			{
				// TODO missing code (just a sync check?)
				if (m_serverTrueBoardSquare != null)
				{
					// square == null;  // it's not an assignment
				}
				Log.Info($"CurrentBoardSquare {DisplayName} {square?.GetGridPos()}"); // custom debug
				m_serverTrueBoardSquare = square;
			}
#endif
			m_clientCurrentBoardSquare = square;

			// removed in rogues
			Animator modelAnimator = GetModelAnimator();
			if (modelAnimator != null)
			{
				modelAnimator.SetBool("Cover", ActorCover.CalcCoverLevelGeoOnly(out bool[] _, CurrentBoardSquare));
			}
			// end removed

			if (MoveFromBoardSquare == null)
			{
				MoveFromBoardSquare = square;
			}
			InitialMoveStartSquare = square;
		}
	}

	public void ClearCurrentBoardSquare()
	{
		if (CurrentBoardSquare != null)
		{
			UnoccupyCurrentBoardSquare();
		}
#if SERVER
		if (NetworkServer.active)  // server-only
		{
			Log.Info($"CurrentBoardSquare (clear) {DisplayName} null"); // custom debug
			m_serverTrueBoardSquare = null;
		}
#endif
		m_clientCurrentBoardSquare = null;
		MoveFromBoardSquare = null;
	}

	public void ClearPreviousMovementInfo()
	{
		if (NetworkServer.active)
		{
			if (m_teamSensitiveData_friendly != null)
			{
				m_teamSensitiveData_friendly.ClearPreviousMovementInfo();
			}
			if (m_teamSensitiveData_hostile != null)  // removed in rogues
			{
				m_teamSensitiveData_hostile.ClearPreviousMovementInfo();
			}
		}
	}

	// server-only -- added in rogues
#if SERVER
	public void SwapBoardSquare(BoardSquare newSquare)
	{
		if (newSquare == null && PlayerIndex >= 0)
		{
			Log.Error($"Actor SwapBoardSquare() trying to assign null as new square for {DebugNameString()}\n{Environment.StackTrace}");
		}
		if (m_serverTrueBoardSquare != newSquare)
		{
			Log.Info($"CurrentBoardSquare (swap) {DisplayName} {newSquare?.GetGridPos()}"); // custom debug
			m_serverTrueBoardSquare = newSquare;
		}
	}
#endif

	// server-only -- added in rogues
#if SERVER
	public void InitActorNetworkVisibilityObjects()
	{
		ActorTeamSensitiveData friendly = Instantiate(NetworkedSharedGameplayPrefabs.GetActorTeamSensitiveData_FriendlyPrefab()).GetComponent<ActorTeamSensitiveData>();
		m_teamSensitiveData_friendly = friendly;
		friendly.InitToActor(this);
		NetworkServer.Spawn(m_teamSensitiveData_friendly.gameObject);
		
		// custom
		ActorTeamSensitiveData hostile = Instantiate(NetworkedSharedGameplayPrefabs.GetActorTeamSensitiveData_HostilePrefab()).GetComponent<ActorTeamSensitiveData>();
		m_teamSensitiveData_hostile = hostile;
		hostile.InitToActor(this);
		NetworkServer.Spawn(m_teamSensitiveData_hostile.gameObject);
		// end custom
	}
#endif
	
	// server-only -- added in rogues
#if SERVER
	public void DespawnTeamSensitiveDataNetObjects()
	{
		if (m_teamSensitiveData_friendly != null)
		{
			NetworkServer.Destroy(m_teamSensitiveData_friendly.gameObject);
			m_teamSensitiveData_friendly = null;
		}
		
		// custom
		if (m_teamSensitiveData_hostile != null)
		{
			NetworkServer.Destroy(m_teamSensitiveData_hostile.gameObject);
			m_teamSensitiveData_hostile = null;
		}
		// end custom
	}
#endif
	
	// server-only -- added in rogues, empty in rogues
#if SERVER
	public void SynchronizeTeamSensitiveData()
	{
		// custom
		// Log.Info($"SynchronizeTeamSensitiveData {GetPlayerDetails()?.m_handle} {TeamSensitiveData_hostile.MoveFromBoardSquare?.GetGridPos()} -> {TeamSensitiveData_authority.MoveFromBoardSquare?.GetGridPos()}");
		// TeamSensitiveData_hostile.BroadcastMovement(
		// 	GameEventManager.EventType.Invalid,
		// 	GetGridPos(),
		// 	GetCurrentBoardSquare(),
		// 	MovementType.Teleport,
		// 	TeleportType.Reappear,
		// 	new BoardSquarePathInfo
		// 	{
		// 		square = GetCurrentBoardSquare(),
		// 		m_visibleToEnemies = true,
		// 		m_updateLastKnownPos = true,
		// 	});
		
		Log.Info($"SynchronizeTeamSensitiveData {GetPlayerDetails()?.m_handle}");
		BoardSquare square = ServerActionBuffer.Get().AbilityPhase == AbilityPriority.Evasion
			? GetSquareAtPhaseStart()
			: GetCurrentBoardSquare();
		SetServerLastKnownPosSquare(square, "SynchronizeTeamSensitiveData");
	}
#endif


	public void SetClientFriendlyTeamSensitiveData(ActorTeamSensitiveData friendlyTSD)
	{
		if (m_teamSensitiveData_friendly != friendlyTSD)
		{
			Log.Info("Setting Friendly TeamSensitiveData for " + DebugNameString());
			m_teamSensitiveData_friendly = friendlyTSD;
			m_teamSensitiveData_friendly.OnClientAssociatedWithActor(this);
		}
	}

	// removed in rogues
	public void SetClientHostileTeamSensitiveData(ActorTeamSensitiveData hostileTSD)
	{
		if (m_teamSensitiveData_hostile != hostileTSD)
		{
			Log.Info("Setting Hostile TeamSensitiveData for " + DebugNameString());
			m_teamSensitiveData_hostile = hostileTSD;
			m_teamSensitiveData_hostile.OnClientAssociatedWithActor(this);
		}
	}

	public void UpdateFacingAfterMovement()
	{
		if (m_facingDirAfterMovement != Vector3.zero)
		{
			TurnToDirection(m_facingDirAfterMovement);
		}
	}

	//public void UpdateAllyReviveButtons()  // rogues
	//{
	//	if (NetworkClient.active && this == GameFlowData.Get().activeOwnedActorData)
	//	{
	//		UIMainScreenPanel.Get().m_abilityBar.EvaluateAllyReviveButtonVisibility();
	//	}
	//}

	public void SetTeam(Team team)
	{
		// reactor
		m_team = team;
		// rogues
		//Networkm_team = team;
		GameFlowData.Get().AddToTeam(this);
		TeamStatusDisplay.GetTeamStatusDisplay().RebuildTeamDisplay();

		// TODO missing code
		if (!NetworkServer.active) // was in reactor
		{
			return;
		}
		//bool active = NetworkServer.active; // added in rogues
	}

	public Team GetTeam()
	{
		return m_team;
	}

	public Team GetEnemyTeam()
	{
		if (m_team == Team.TeamA)
		{
			return Team.TeamB;
		}
		if (m_team == Team.TeamB)
		{
			return Team.TeamA;
		}
		return Team.Objects;
	}

	public List<Team> GetOtherTeams()
	{
		return GameplayUtils.GetOtherTeamsThan(m_team);
	}

	public List<Team> GetTeamAsList()
	{
		return new List<Team>
		{
			GetTeam()
		};
	}

	public List<Team> GetEnemyTeamAsList()
	{
		return new List<Team>
		{
			GetEnemyTeam()
		};
	}

	public string GetAllyTeamName()
	{
		return m_team == Team.TeamA ? "Blue" : "Orange";
	}

	public string GetEnemyTeamName()
	{
		return m_team == Team.TeamA ? "Orange" : "Blue";
	}

	public Color GetAllyTeamColor()
	{
		return m_team == Team.TeamA ? s_teamAColor : s_teamBColor;
	}

	public Color GetEnemyTeamColor()
	{
		return m_team == Team.TeamA ? s_teamBColor : s_teamAColor;
	}

	public Color GetRelativeColor(Team observingTeam)
	{
		return observingTeam == GetTeam() ? s_friendlyPlayerColor : s_hostilePlayerColor;
	}

	internal void OnTurnTick()
	{
		CurrentlyVisibleForAbilityCast = false;  // removed in rogues
		MovedForEvade = false;  // removed in rogues
		m_actorMovement.ClearPath();
		GetFogOfWar().MarkForRecalculateVisibility();
		if (!NetworkServer.active
			&& m_serverMovementWaitForEvent != GameEventManager.EventType.Invalid
			&& m_serverMovementDestination != GetCurrentBoardSquare()
			&& !IsDead())
		{
			MoveToBoardSquareLocal(m_serverMovementDestination, MovementType.Teleport, m_serverMovementPath, false);
		}
		if (GetActorModelData() != null)
		{
			Animator modelAnimator = GetModelAnimator();
			if (modelAnimator != null)
			{
				if (GetActorModelData().HasTurnStartParameter())
				{
					modelAnimator.SetBool("TurnStart", true);
				}
				modelAnimator.SetInteger("Attack", 0);
				modelAnimator.SetBool("CinematicCam", false);
			}
		}
		if (NetworkClient.active)
		{
			if (ClientUnresolvedDamage != 0)
			{
				Log.Error("ClientUnresolvedDamage not cleared on TurnTick for " + DebugNameString());
				ClientUnresolvedDamage = 0;
			}
			if (ClientUnresolvedHealing != 0)
			{
				Log.Error("ClientUnresolvedHealing not cleared on TurnTick for " + DebugNameString());
				ClientUnresolvedHealing = 0;
			}
			if (ClientUnresolvedTechPointGain != 0)
			{
				ClientUnresolvedTechPointGain = 0;
			}
			if (ClientUnresolvedTechPointLoss != 0)
			{
				ClientUnresolvedTechPointLoss = 0;
			}
			if (ClientReservedTechPoints != 0)
			{
				ClientReservedTechPoints = 0;
			}
			if (ClientUnresolvedAbsorb != 0)
			{
				Log.Error("ClientUnresolvedAbsorb not cleared on TurnTick for " + DebugNameString());
				ClientUnresolvedAbsorb = 0;
			}
			ClientExpectedHoTTotalAdjust = 0;
			ClientAppliedHoTThisTurn = 0;
			SynchClientLastKnownPosToServerLastKnownPos();
			if (GetTeamSensitiveDataForClient() != null)
			{
				GetTeamSensitiveDataForClient().OnTurnTick();
			}
			if (GameFlowData.Get().LocalPlayerData != null && HighlightUtils.Get().m_recentlySpawnedShader != null)
			{
				int currentTurn = GameFlowData.Get().CurrentTurn;
				if (currentTurn == 1)
				{
					TricksterAfterImageNetworkBehaviour.InitializeAfterImageMaterial(GetActorModelData(), GameFlowData.Get().LocalPlayerData.GetTeamViewing() == GetTeam(), 0.5f, HighlightUtils.Get().m_recentlySpawnedShader, false);
				}
				else if (currentTurn == 2 || (currentTurn > 2 && currentTurn == NextRespawnTurn + 1))
				{
					TricksterAfterImageNetworkBehaviour.DisableAfterImageMaterial(GetActorModelData());
				}
			}
		}
		m_actorVFX.OnTurnTick();
		m_wasUpdatingForConfirmedTargeting = false;
		KnockbackMoveStarted = false;
		if (GetActorBehavior() != null)
		{
			GetActorBehavior().Client_ResetKillAssistContribution();
		}
		if (GetActorCover() != null)
		{
			GetActorCover().RecalculateCover();
			if (!IsDead()
				&& GameFlowData.Get() != null
				&& GameFlowData.Get().CurrentTurn > 1
				&& HighlightUtils.Get() != null
				&& HighlightUtils.Get().m_coverDirIndicatorTiming == HighlightUtils.MoveIntoCoverIndicatorTiming.ShowOnTurnStart
				&& HighlightUtils.Get().m_showMoveIntoCoverIndicators)
			{
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (activeOwnedActorData != null && activeOwnedActorData == this && IsActorVisibleToClient())
				{
					GetActorCover().StartShowMoveIntoCoverIndicator();
				}
			}
		}
		OnTurnStartDelegates?.Invoke();
	}

	// server-only -- added in rogues
#if SERVER
	[Server]
	public void OnTurnStart()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorData::OnTurnStart()' called on client");
			return;
		}
		GetComponent<ServerActorController>().CancelActionRequestsForTurnStart();
		//SetSquareAtPhaseStart(GetCurrentBoardSquare());
		m_actorMovement.ClearPath();
		if (m_actorModelData && !IsDead())
		{
			m_actorModelData.EnableRagdoll(false, null);
		}
		if (SpawnPointManager.Get() != null && GameFlowData.Get().CurrentTurn > NextRespawnTurn && NextRespawnTurn > 0)
		{
			IgnoreForAbilityHits = false;
		}
		if (m_abilityData)
		{
			m_abilityData.ProgressCooldowns();
			m_abilityData.ProgressStockRefreshTimes();
			m_abilityData.SoftTargetedActor = null;
			m_abilityData.UnsuppressInvisibility();
			m_abilityData.ClearSelectedAbility();
			m_abilityData.ClearLastSelectedAbility();
			m_abilityData.ClearOnRequestStatusForAllAbilities();
		}
		if (m_actorTurnSM)
		{
			m_actorTurnSM.ResetTurnStartNow();
		}
		ReservedTechPoints = 0;
		if (!IsDead() && m_lastSpawnTurn <= GameFlowData.Get().CurrentTurn && GameFlowData.Get().CurrentTurn > 1)
		{
			SetHitPoints(HitPoints + GetHitPointRegen());
		}
		if (GameFlowData.Get().CurrentTurn > 1)
		{
			int num = GetTechPointRegen();
			if (m_abilityData)
			{
				num += m_abilityData.GetTechPointRegenFromAbilities();
			}
			if (m_actorStatus && GameWideData.Get().m_useEnergyStatusForPassiveRegen)
			{
				ServerGameplayUtils.EnergyStatAdjustments energyStatAdjustments = new ServerGameplayUtils.EnergyStatAdjustments(this, this);
				num = ServerCombatManager.Get().CalcTechPointGain(this, num, AbilityData.ActionType.INVALID_ACTION, energyStatAdjustments);
				energyStatAdjustments.CalculateAdjustments();
				energyStatAdjustments.ApplyStatAdjustments();
			}
			SetTechPoints(TechPoints + num, false, null, null);
		}

		// rogues
		//RefillOutOfCombatShield();

		QueuedMovementAllowsAbility = true;
		InitialMoveStartSquare = GetCurrentBoardSquare();
		LineData component = GetComponent<LineData>();
		if (component)
		{
			component.OnTurnStart();
		}
		PassiveData passiveData = m_passiveData;
		if (passiveData)
		{
			passiveData.OnTurnStart();
		}
		ActorStatus actorStatus = m_actorStatus;
		if (actorStatus)
		{
			if (actorStatus.HasStatus(StatusType.RecentlySpawned, true))
			{
				actorStatus.RemoveStatus(StatusType.RecentlySpawned);
			}
			if (actorStatus.HasStatus(StatusType.RecentlyRespawned, true))
			{
				actorStatus.RemoveStatus(StatusType.RecentlyRespawned);
			}
			if (actorStatus.HasStatus(StatusType.KnockedBack, true))
			{
				actorStatus.RemoveStatus(StatusType.KnockedBack);
			}
			actorStatus.OnTurnStart();
		}
		ActorBehavior actorBehavior = GetActorBehavior();
		if (actorBehavior)
		{
			actorBehavior.OnTurnStart();
			actorBehavior.CurrentTurn.Square = GetCurrentBoardSquare();
			actorBehavior.CurrentTurn.StartingHitPoints = HitPoints;
			actorBehavior.CurrentTurn.StartingTechPoints = TechPoints;
			m_serverMovementPath = null;
		}
		m_serverMovementWaitForEvent = GameEventManager.EventType.Invalid;
		ActorCinematicRequests component2 = GetComponent<ActorCinematicRequests>();
		if (component2)
		{
			component2.OnTurnStart();
		}
		GetActorMovement().UpdateSquaresCanMoveTo();
		GameplayMetricHelper.CollectTurnStart(this);
	}
#endif

	// server-only? or rogues-only? -- added in rogues
	//public void RefillOutOfCombatShield()
	//{
	//	if (NetworkServer.active && !IsDead() && GetTeam() == Team.TeamA && !GameFlowData.Get().IsInCombat)
	//	{
	//		int outOfCombatShieldMax = GetOutOfCombatShieldMax();
	//		if (outOfCombatShieldMax > 0)
	//		{
	//			global::Effect effect = ServerEffectManager.Get().GetEffect(this, typeof(ShieldOutOfCombatEffect));
	//			if (effect == null)
	//			{
	//				ShieldOutOfCombatEffect effect2 = new ShieldOutOfCombatEffect(new EffectSource(DisplayName, null, null), this, outOfCombatShieldMax);
	//				ServerEffectManager.Get().ApplyEffect(effect2, 1);
	//				return;
	//			}
	//			int num = outOfCombatShieldMax - effect.Absorbtion.m_absorbRemaining;
	//			if (num > 0)
	//			{
	//				effect.RefillAbsorb(num);
	//			}
	//		}
	//	}
	//}

	// added in rogues
#if SERVER
		public void OnKnockbackHitExecutedOnTarget(ActorData target, ActorHitResults hitRes)
		{
			OnKnockbackHitExecutedDelegate?.Invoke(target, hitRes);
		}
#endif

	public bool HasQueuedMovement()
	{
#if SERVER
		if (NetworkServer.active)  // server-only
		{
			ServerActionBuffer.Get().PendingMovementRequestInfo(this, out m_queuedMovementRequest, out m_queuedChaseRequest, out m_queuedChaseTarget);
		}
#endif
		return m_queuedMovementRequest || m_queuedChaseRequest;
	}

	public bool HasQueuedChase()
	{
#if SERVER
		if (NetworkServer.active)  // server-only
		{
			ServerActionBuffer.Get().PendingMovementRequestInfo(this, out m_queuedMovementRequest, out m_queuedChaseRequest, out m_queuedChaseTarget);
		}
#endif
		return m_queuedChaseRequest;
	}

	public ActorData GetQueuedChaseTarget()
	{
#if SERVER
		if (NetworkServer.active)  // server-only
		{
			ServerActionBuffer.Get().PendingMovementRequestInfo(this, out m_queuedMovementRequest, out m_queuedChaseRequest, out m_queuedChaseTarget);
		}
#endif
		return m_queuedChaseTarget;
	}

	public void TurnToDirection(Vector3 dir)
	{
		Quaternion quaternion = Quaternion.LookRotation(dir);
		if (Quaternion.Angle(quaternion, m_targetRotation.GetEndValue()) > 0.01f)
		{
			transform.localRotation = m_targetRotation.GetEndValue();
			m_targetRotation.EaseTo(quaternion, 0.1f);
		}
	}

	public void TurnToPosition(Vector3 position, float turnDuration = 0.2f)
	{
		Vector3 vector = position - transform.position;
		vector.y = 0f;
		if (vector != Vector3.zero)
		{
			transform.localRotation = m_targetRotation.GetEndValue();
			Quaternion quaternion = default(Quaternion);
			quaternion.SetLookRotation(vector);
			if (Quaternion.Angle(quaternion, m_targetRotation.GetEndValue()) > 0.01f)
			{
				m_targetRotation.EaseTo(quaternion, turnDuration);
			}
		}
	}

	public float GetTurnToPositionTimeRemaining()
	{
		return m_targetRotation.CalcTimeRemaining();
	}

	public void TurnToPositionInstant(Vector3 position)
	{
		Vector3 vector = position - transform.position;
		vector.y = 0f;
		if (vector != Vector3.zero)
		{
			Quaternion quaternion = default(Quaternion);
			quaternion.SetLookRotation(vector);
			transform.localRotation = quaternion;
			m_targetRotation.SnapTo(quaternion);
		}
	}

	public Rigidbody GetRigidBody(string boneName)
	{
		Rigidbody result = null;
		GameObject gameObject = base.gameObject.FindInChildren(boneName);
		if (gameObject)
		{
			result = gameObject.GetComponentInChildren<Rigidbody>();
		}
		else
		{
			Log.Warning($"GetRigidBody trying to find body of bone {boneName} on actor '{DisplayName}' (obj name '{base.gameObject.name}'), but the bone cannot be found.");
		}
		return result;
	}

	public Rigidbody GetHipJoint()
	{
		if (m_cachedHipJoint == null)
		{
			m_cachedHipJoint = GetRigidBody("hip_JNT");
		}
		return m_cachedHipJoint;
	}

	public Vector3 GetHipJointPos()
	{
		Rigidbody hipJoint = GetHipJoint();
		if (hipJoint != null)
		{
			return hipJoint.transform.position;
		}
		return gameObject.transform.position;
	}

	public Vector3 GetBonePosition(string boneName)
	{
		GameObject boneObject = gameObject.FindInChildren(boneName);
		if (boneObject)
		{
			return boneObject.transform.position;
		}
		return gameObject.transform.position;
	}

	public Quaternion GetBoneRotation(string boneName)
	{
		GameObject boneObject = gameObject.FindInChildren(boneName);
		if (boneObject)
		{
			return boneObject.transform.rotation;
		}
		Log.Warning($"GetBoneRotation trying to find rotation of bone {boneName} on actor '{DisplayName}' (obj name '{gameObject.name}'), but the bone cannot be found.");
		return gameObject.transform.rotation;
	}

	public void OnDeselectedAsActiveActor()
	{
		//if (GetActorTurnSM() != null)  // rogues?
		//{
		//	GetActorTurnSM().BackToDecidingState();
		//}
		if (GetActorController() != null)
		{
			GetActorController().ClearHighlights();
		}
		GetActorCover().UpdateCoverHighlights(null);
		RespawnPickedPositionSquare = RespawnPickedPositionSquare;
	}

	public void OnSelectedAsActiveActor()
	{
		m_callHandleOnSelectInUpdate = true;
		//GetActorController().OnSelectedAsActiveActor();  // rogues?
	}

	private void HandleOnSelectedAsActiveActor()
	{
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.ProcessTeams();  // removed in rogues
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.MarkFramesForForceUpdate();
			HUD_UI.Get().m_mainScreenPanel.m_characterProfile.UpdateStatusDisplay(true);
		}
		m_actorTurnSM.OnSelect();  // removed in rogues
		if (GetFogOfWar() != null)  // added checks in rogues
		{
			GetFogOfWar().MarkForRecalculateVisibility();
		}
		if (CameraManager.Get() != null)  // added checks in rogues
		{
			CameraManager.Get().OnActiveOwnedActorChange(this);
		}
		if (GetActorMovement() != null)
		{
			GetActorMovement().UpdateSquaresCanMoveTo();
		}
	}

	// server-only -- was empty in reactor
	public void OnMovementChanged(MovementChangeType changeType, bool forceChased = false)
	{
#if SERVER
		if (NetworkServer.active)
		{
			BoardSquare boardSquare;
			float num;
			bool flag;
			ServerActionBuffer.Get().GatherMovementInfo(this, out boardSquare, out num, out flag);
			if (flag)
			{
				QueuedMovementAllowsAbility = true;
			}
			else if (QueuedMovementAllowsAbility && changeType == MovementChangeType.MoreMovement && !GetActorMovement().SquaresCanMoveToWithQueuedAbility.Contains(boardSquare))
			{
				QueuedMovementAllowsAbility = false;
			}
			GetComponent<ActorMovement>().UpdateSquaresCanMoveTo();
			if (!QueuedMovementAllowsAbility && changeType == MovementChangeType.LessMovement)
			{
				if (num == 0f)
				{
					QueuedMovementAllowsAbility = true;
				}
				else if (GetActorMovement().SquaresCanMoveToWithQueuedAbility.Contains(boardSquare))
				{
					QueuedMovementAllowsAbility = true;
				}
				else if (RemainingHorizontalMovement > 0f && boardSquare == InitialMoveStartSquare)
				{
					QueuedMovementAllowsAbility = true;
				}
			}
			ServerActionBuffer.Get().UpdateActorLineDataForMovementStatus(this, changeType > MovementChangeType.MoreMovement);
			if (SinglePlayerManager.Get() != null)
			{
				SinglePlayerManager.Get().OnActorMovementChanged(this);
			}

			// rogues
			//GetActorTurnSM().UpdateHasStoredMoveRequestFlag();
		}
#endif
	}

	// reactor
	public bool BeingTargetedByClientAbility(out bool inCover, out bool updatingInConfirm)
	// rogues
	//public bool BeingTargetedByClientAbility(out HitChanceBracketType inCover, out bool updatingInConfirm)
	{
		bool isInRange = false;
		// reactor
		inCover = false;
		// rogues
		//inCover = HitChanceBracketType.Default;
		updatingInConfirm = false;
		GameFlowData gameFlowData = GameFlowData.Get();
		if (gameFlowData != null
			&& gameFlowData.gameState == GameState.BothTeams_Decision
			&& gameFlowData.activeOwnedActorData != null)
		{
			ActorData activeOwnedActorData = gameFlowData.activeOwnedActorData;
			AbilityData abilityData = activeOwnedActorData.GetAbilityData();
			ActorTurnSM actorTurnSM = activeOwnedActorData.GetActorTurnSM();
			if (actorTurnSM.CurrentState != TurnStateEnum.TARGETING_ACTION)
			{
				if (actorTurnSM.CurrentState == TurnStateEnum.DECIDING && abilityData != null)
				{
					Ability lastSelectedAbility = abilityData.GetLastSelectedAbility();
					if (ShouldUpdateForConfirmedTargeting(lastSelectedAbility, actorTurnSM.GetAbilityTargets().Count))
					{
						isInRange = lastSelectedAbility.IsActorInTargetRange(this, out inCover);
						int targetersLeft = lastSelectedAbility.IsSimpleAction()
							? 0
							: actorTurnSM.GetAbilityTargets().Count - 1;
						if (targetersLeft >= 0 && lastSelectedAbility.Targeters != null)
						{
							targetersLeft = Mathf.Clamp(targetersLeft, 0, lastSelectedAbility.Targeters.Count - 1);
							UpdateNameplateForTargetingAbility(activeOwnedActorData, lastSelectedAbility, isInRange, inCover, targetersLeft, true);
							updatingInConfirm = true;
							// reactor
							if (HUD_UI.Get() != null)
							{
								if (activeOwnedActorData.ForceDisplayTargetHighlight)
								{
									HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.ShowTargetingNumberForConfirmedTargeting(this);
									m_showingTargetingNumAtFullAlpha = true;
								}
								else if (!m_wasUpdatingForConfirmedTargeting || m_showingTargetingNumAtFullAlpha)
								{
									HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.StartTargetingNumberFadeout(this);
									m_showingTargetingNumAtFullAlpha = false;
								}
							}
							// rogues
							//if (!m_wasUpdatingForConfirmedTargeting && HUD_UI.Get() != null)
							//{
							//	HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.StartTargetingNumberFadeout(this);
							//}
						}
					}
					else if (m_wasUpdatingForConfirmedTargeting)  // removed in rogues
					{
						HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.StartTargetingNumberFadeout(this);
						m_showingTargetingNumAtFullAlpha = false;
					}
				}
				if (actorTurnSM.CurrentState == TurnStateEnum.DECIDING
					&& !activeOwnedActorData.ForceDisplayTargetHighlight  // && !ForceDisplayTargetHighlight in rogues
					&& !isInRange
					&& HUD_UI.Get() != null)
				{
					HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateNameplateUntargeted(this, !updatingInConfirm);
				}
			}
			else
			{
				Ability selectedAbility = abilityData.GetSelectedAbility();
				if (selectedAbility != null && selectedAbility.Targeters != null)
				{
					isInRange = selectedAbility.IsActorInTargetRange(this, out inCover);
					int count = actorTurnSM.GetAbilityTargets().Count;
					count = Mathf.Clamp(count, 0, selectedAbility.Targeters.Count - 1);
					UpdateNameplateForTargetingAbility(activeOwnedActorData, selectedAbility, isInRange, inCover, count, false);
				}
			}
		}
		m_wasUpdatingForConfirmedTargeting = updatingInConfirm;
		return isInRange;
	}

	// TODO check if there is anything useful
	//public bool BeingTargetedByClientAbility(out HitChanceBracketType inCover, out bool updatingInConfirm)  // rogues
	//{
	//	bool flag = false;
	//	inCover = HitChanceBracketType.Default;
	//	updatingInConfirm = false;
	//	GameFlowData gameFlowData = GameFlowData.Get();
	//	if (gameFlowData != null && gameFlowData.IsInDecisionState() && gameFlowData.activeOwnedActorData != null)
	//	{
	//		ActorData activeOwnedActorData = gameFlowData.activeOwnedActorData;
	//		AbilityData abilityData = activeOwnedActorData.GetAbilityData();
	//		ActorTurnSM actorTurnSM = activeOwnedActorData.GetActorTurnSM();
	//		if (actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION)
	//		{
	//			Ability selectedAbility = abilityData.GetSelectedAbility();
	//			if (selectedAbility != null && selectedAbility.Targeters != null)
	//			{
	//				flag = selectedAbility.IsActorInTargetRange(this, out inCover);
	//				int num = actorTurnSM.GetAbilityTargets().Count;
	//				num = Mathf.Clamp(num, 0, selectedAbility.Targeters.Count - 1);
	//				UpdateNameplateForTargetingAbility(activeOwnedActorData, selectedAbility, flag, inCover, num, false);
	//			}
	//		}
	//		else
	//		{
	//			if (actorTurnSM.CurrentState == TurnStateEnum.DECIDING && abilityData != null)
	//			{
	//				Ability lastSelectedAbility = abilityData.GetLastSelectedAbility();
	//				if (ShouldUpdateForConfirmedTargeting(lastSelectedAbility, actorTurnSM.GetAbilityTargets().Count))
	//				{
	//					flag = lastSelectedAbility.IsActorInTargetRange(this, out inCover);
	//					int num2 = lastSelectedAbility.IsSimpleAction() ? 0 : (actorTurnSM.GetAbilityTargets().Count - 1);
	//					if (num2 >= 0 && lastSelectedAbility.Targeters != null)
	//					{
	//						num2 = Mathf.Clamp(num2, 0, lastSelectedAbility.Targeters.Count - 1);
	//						UpdateNameplateForTargetingAbility(activeOwnedActorData, lastSelectedAbility, flag, inCover, num2, true);
	//						updatingInConfirm = true;
	//						if (!m_wasUpdatingForConfirmedTargeting && HUD_UI.Get() != null)
	//						{
	//							HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.StartTargetingNumberFadeout(this);
	//						}
	//					}
	//				}
	//			}
	//			if (actorTurnSM.CurrentState == TurnStateEnum.DECIDING && !ForceDisplayTargetHighlight && !flag && HUD_UI.Get() != null)
	//			{
	//				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateNameplateUntargeted(this, !updatingInConfirm);
	//			}
	//		}
	//	}
	//	m_wasUpdatingForConfirmedTargeting = updatingInConfirm;
	//	return flag;
	//}

	public void AddForceShowOutlineChecker(IForceActorOutlineChecker checker)  // removed in rogues
	{
		if (checker != null && !m_forceShowOutlineCheckers.Contains(checker))
		{
			m_forceShowOutlineCheckers.Add(checker);
		}
	}

	public void RemoveForceShowOutlineChecker(IForceActorOutlineChecker checker)  // removed in rogues
	{
		if (m_forceShowOutlineCheckers != null)
		{
			m_forceShowOutlineCheckers.Remove(checker);
		}
	}

	public bool ShouldForceTargetOutlineForActor(ActorData actor)  // removed in rogues
	{
		if (GameFlowData.Get().gameState == GameState.BothTeams_Decision && GameFlowData.Get().activeOwnedActorData != null)
		{
			bool flag = false;
			for (int i = 0; i < m_forceShowOutlineCheckers.Count; i++)
			{
				if (flag)
				{
					break;
				}
				IForceActorOutlineChecker forceActorOutlineChecker = m_forceShowOutlineCheckers[i];
				if (forceActorOutlineChecker != null)
				{
					flag = forceActorOutlineChecker.ShouldForceShowOutline(actor);
				}
			}
			return flag;
		}
		return false;
	}

	[Client]
	private void UpdateNameplateForTargetingAbility(ActorData targetingActor, Ability selectedAbility, bool targeted, bool inCover, int currentTargeterIndex, bool inConfirm)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void ActorData::UpdateNameplateForTargetingAbility(ActorData,Ability,System.Boolean,System.Boolean,System.Int32,System.Boolean)' called on server");
			return;
		}

		if (HUD_UI.Get() != null)
		{
			if (this == targetingActor)
			{
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateSelfNameplate(this, selectedAbility, inCover, currentTargeterIndex, inConfirm);
			}
			else if (targeted)
			{
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateNameplateTargeted(targetingActor, this, selectedAbility, inCover, currentTargeterIndex, inConfirm);
			}
			else
			{
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateNameplateUntargeted(this, true);
			}
		}
	}

	// TODO check if there is anything useful
	//[Client]
	//private void UpdateNameplateForTargetingAbility(ActorData targetingActor, Ability selectedAbility, bool targeted, HitChanceBracketType inCover, int currentTargeterIndex, bool inConfirm)  // rogues
	//{
	//	if (!NetworkClient.active)
	//	{
	//		Debug.LogWarning("[Client] function 'System.Void ActorData::UpdateNameplateForTargetingAbility(ActorData,Ability,System.Boolean,HitChanceBracketType,System.Int32,System.Boolean)' called on server");
	//		return;
	//	}
	//	if (HUD_UI.Get() != null)
	//	{
	//		if (this == targetingActor)
	//		{
	//			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateSelfNameplate(this, selectedAbility, inCover, currentTargeterIndex, inConfirm);
	//			return;
	//		}
	//		if (targeted)
	//		{
	//			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateNameplateTargeted(targetingActor, this, selectedAbility, inCover, currentTargeterIndex, inConfirm);
	//			return;
	//		}
	//		HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateNameplateUntargeted(this, true);
	//	}
	//}

	[Client]
	private bool ShouldUpdateForConfirmedTargeting(Ability lastSelectedAbility, int numAbilityTargets)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Boolean ActorData::ShouldUpdateForConfirmedTargeting(Ability,System.Int32)' called on server");
			return false;
		}
		if (lastSelectedAbility == null)
		{
			return false;
		}
		if (ForceDisplayTargetHighlight)
		{
			return true;
		}
		return lastSelectedAbility.Targeter != null
			&& lastSelectedAbility.Targeter.GetConfirmedTargetingRemainingTime() > 0f
			&& (lastSelectedAbility.IsSimpleAction() || numAbilityTargets > 0);
	}

	public static bool WouldSquareBeChasedByClient(BoardSquare square, bool IgnoreChosenChaseTarget = false)
	{
		//return false;  // rogues
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (!activeOwnedActorData.IsSquareChaseableByClient(square))
		{
			return false;
		}
		if (!activeOwnedActorData.HasQueuedMovement() && !activeOwnedActorData.HasQueuedChase())
		{
			return true;
		}
		if (activeOwnedActorData.HasQueuedChase())
		{
			return IgnoreChosenChaseTarget || square != activeOwnedActorData.GetQueuedChaseTarget().GetCurrentBoardSquare();
		}
		if (square == activeOwnedActorData.MoveFromBoardSquare || !activeOwnedActorData.CanMoveToBoardSquare(square))
		{
			return true;
		}
		return false;
	}

	public bool IsSquareChaseableByClient(BoardSquare square)
	{
		if (square == null
			|| square.occupant == null
			|| square.occupant.GetComponent<ActorData>() == null
			|| GameFlowData.Get() == null)
		{
			return false;
		}

		if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
		{
			ActorData occupant = square.occupant.GetComponent<ActorData>();
			AbilityData actor = GetComponent<AbilityData>();
			return !occupant.IsDead()
				&& occupant != this
				&& occupant.IsActorVisibleToSpecificClient(this)
				&& actor.GetQueuedAbilitiesAllowMovement()
				&& !occupant.IgnoreForAbilityHits;
		}

		return false;
	}

	public void OnHitWhileInCover(Vector3 hitOrigin, ActorData caster)
	{
		if (!IsDead() && m_actorVFX != null)
		{
			m_actorVFX.ShowHitWhileInCoverVfx(GetFreePos(), hitOrigin, caster);
			AudioManager.PostEvent("ablty/generic/feedback/behind_cover_hit", gameObject);
		}
	}

	public void OnKnockbackWhileUnstoppable(Vector3 hitOrigin, ActorData caster)
	{
		if (!IsDead() && m_actorVFX != null)
		{
			m_actorVFX.ShowKnockbackWhileUnstoppableVfx(GetFreePos(), hitOrigin, caster);
			AudioManager.PostEvent("ablty/generic/feedback/unstoppable", gameObject);
		}
	}

	public void PostAnimationAudioEvent(string eventAndTag)
	{
		int num = eventAndTag.IndexOf(':');
		string audioTag;
		string eventName;
		if (num == -1)
		{
			audioTag = "default";
			eventName = eventAndTag;
		}
		else
		{
			audioTag = eventAndTag.Substring(0, num);
			eventName = eventAndTag.Substring(num + 1);
		}
		CharacterResourceLink characterResourceLink = GetCharacterResourceLink();
		if (characterResourceLink == null || characterResourceLink.AllowAudioTag(audioTag, m_visualInfo))
		{
			PostAudioEvent(eventName);
		}
	}

	public void PostAudioEvent(string eventName, OnEventNotify notifyCallback = null, AudioManager.EventAction action = AudioManager.EventAction.PlaySound)
	{
		CharacterResourceLink characterResourceLink = GetCharacterResourceLink();
		string text;
		if (characterResourceLink != null)
		{
			text = characterResourceLink.ReplaceAudioEvent(eventName, m_visualInfo);
		}
		else
		{
			text = eventName;
		}

		// TODO missing code?
		//text != eventName;  // rogues
		if (text != eventName)  // reactor
		{
		}
		// end missing code


		if (notifyCallback != null)
		{
			AudioManager.PostEventNotify(text, action, notifyCallback, null, gameObject);
		}
		else
		{
			AudioManager.PostEvent(text, action, null, gameObject);
		}
	}

	[Command]
	internal void CmdSetPausedForDebugging(bool pause)
	{
		if (!HydrogenConfig.Get().AllowDebugCommands)
		{
			return;
		}
		GameFlowData.Get().SetPausedForDebugging(pause);
	}

	[Command]
	internal void CmdSetResolutionSingleStepping(bool singleStepping)
	{
		if (GameFlowData.Get() != null)
		{
			GameFlowData.Get().SetResolutionSingleStepping(singleStepping);
		}
	}

	[Command]
	internal void CmdSetResolutionSingleSteppingAdvance()
	{
		if (GameFlowData.Get() != null)
		{
			GameFlowData.Get().SetResolutionSingleSteppingAdvance();
		}
	}

	[Command]
	public void CmdSetDebugToggleParam(string name, bool value)
	{
	}

	[Command]
	internal void CmdDebugReslotCards(bool reslotAll, int cardTypeInt)
	{
	}

	[Command]
	internal void CmdDebugSetAbilityMod(int abilityIndex, int modId)  // removed in rogues
	{
	}

	[Command]
	private void CmdDebugReplaceWithBot() // was empty in reactor
	{
#if SERVER
		if (!HydrogenConfig.Get().AllowDebugCommands)
		{
			return;
		}
		if (PlayerData)
		{
			GameFlow.Get().ReplaceWithBots(PlayerData.GetPlayer(), GameFlow.Get().playerDetails[PlayerData.GetPlayer()], false);
		}
#endif
	}

	[Command]
	internal void CmdDebugSetHealthOrEnergy(int actorIndex, int valueToSet, int flag)
	{
	}

	public void HandleDebugSetHealth(int actorIndex, int valueToSet)
	{
		CallCmdDebugSetHealthOrEnergy(actorIndex, valueToSet, 0);
	}

	public void HandleDebugSetEnergy(int actorIndex, int valueToSet)
	{
		CallCmdDebugSetHealthOrEnergy(actorIndex, valueToSet, 1);
	}

	[ClientRpc]
	internal void RpcOnHitPointsResolved(int resolvedHitPoints)
	{
		if (NetworkServer.active)
		{
			return;
		}
		bool wasDead = IsDead();
		UnresolvedDamage = 0;
		UnresolvedHealing = 0;
		ClientUnresolvedDamage = 0;
		ClientUnresolvedHealing = 0;
		ClientUnresolvedAbsorb = 0;
		SetHitPoints(resolvedHitPoints);
		if (!wasDead && IsDead() && !IsInRagdoll())
		{
			Debug.LogError("Actor " + DebugNameString() + " died on HP resolved; he should have already been ragdolled, but wasn't.");
			DoVisualDeath(new ActorModelData.ImpulseInfo(GetLoSCheckPos(), Vector3.up));
		}
	}

	// added in rogues?
	//[ClientRpc]
	//internal void RpcOnTechPointsResolved(int resolvedTechPoints)
	//{
	//	if (!NetworkServer.active)
	//	{
	//		UnresolvedTechPointGain = 0;
	//		UnresolvedTechPointLoss = 0;
	//		ClientUnresolvedTechPointGain = 0;
	//		ClientUnresolvedTechPointLoss = 0;
	//		TechPoints = resolvedTechPoints;
	//	}
	//}

	[ClientRpc]
	internal void RpcCombatText(string combatText, string logText, CombatTextCategory category, BuffIconToDisplay icon)
	{
		AddCombatText(combatText, logText, category, icon);
	}

	internal void AddCombatText(string combatText, string logText, CombatTextCategory category, BuffIconToDisplay icon)  // , HitChanceBracket.HitType hitType = HitChanceBracket.HitType.Normal in rogues
	{
		if (m_combatText == null)
		{
			Log.Error(gameObject.name + " does not have a combat text component.");
			return;
		}
		m_combatText.Add(combatText, logText, category, icon); // , hitType in rogues
	}

	[Client]
	internal void ShowDamage(string combatText)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void ActorData::ShowDamage(System.String)' called on server");
			return;
		}
		// TODO missing code?
	}

	[ClientRpc]
	internal void RpcApplyAbilityModById(int actionTypeInt, int abilityScopeId)
	{
		if (!NetworkServer.active && NetworkClient.active && abilityScopeId >= 0)  // no check  && abilityScopeId >= 0 in rogues
		{
			ApplyAbilityModById(actionTypeInt, abilityScopeId);
		}
	}

	internal void ApplyAbilityModById(int actionTypeInt, int abilityScopeId)
	{
		// reactor
		bool isTutorial = GameManager.Get().GameConfig.GameType == GameType.Tutorial;
		// rogues
		//bool isTutorial = GameManager.Get().GameMission.IsMissionTagActive(MissionData.s_missionTagTutorial);  // rogues
		if (!isTutorial && !AbilityModHelper.IsModAllowed(m_characterType, actionTypeInt, abilityScopeId))
		{
			Debug.LogWarning("Mod with ID " + abilityScopeId + " is not allowed on ability at index " + actionTypeInt + " for character " + m_characterType.ToString());
			return;
		}

		AbilityData component = GetComponent<AbilityData>();
		if (component != null)
		{
			Ability abilityOfActionType = component.GetAbilityOfActionType((AbilityData.ActionType)actionTypeInt);
			// reactor
			AbilityMod abilityModForAbilityById = AbilityModManager.Get().GetAbilityModForAbilityById(abilityOfActionType, abilityScopeId);
			// rogues
			//AbilityMod abilityModForAbilityById = TalentManager.Get().GetAbilityMod(m_characterType, (AbilityData.ActionType)actionTypeInt);  // rogues
			if (abilityModForAbilityById != null)
			{
				GameType gameType = GameManager.Get().GameConfig.GameType;
				GameSubType instanceSubType = GameManager.Get().GameConfig.InstanceSubType;
				if (abilityModForAbilityById.EquippableForGameType())  // not checked in rogues
				{
					ApplyAbilityModToAbility(abilityOfActionType, abilityModForAbilityById);
					if (NetworkServer.active)  // removed in rogues
					{
						CallRpcApplyAbilityModById(actionTypeInt, abilityScopeId);
					}
				}
				else
				{
					Log.Warning("Mod with ID " + abilityModForAbilityById.m_abilityScopeId + " is not allowed in game type: " + gameType.ToString() + ", subType: " + instanceSubType.LocalizedName);
				}
			}
		}
	}

	//[ClientRpc]
	//internal void RpcApplyGearById(int actionTypeInt, int gearID)  // rogues
	//{
	//	if (!NetworkServer.active && NetworkClient.active)
	//	{
	//		ApplyGearById(actionTypeInt, gearID);
	//	}
	//}

	//internal void ApplyAbilityGearByID(int actionTypeInt, int gearID)  // rogues
	//{
	//	Ability abilityOfActionType = base.GetComponent<AbilityData>().GetAbilityOfActionType((AbilityData.ActionType)actionTypeInt);
	//	if (abilityOfActionType != null)
	//	{
	//		Gear gearForID = GearHelper.GetGearForID(gearID);
	//		if (gearForID != null)
	//		{
	//			ApplyGearToAbility(abilityOfActionType, gearForID, false);
	//			if (NetworkServer.active)
	//			{
	//				CallRpcApplyGearById(actionTypeInt, gearID);
	//			}
	//		}
	//	}
	//}

	//internal void ApplyGearById(int actionTypeInt, int gearID)  // rogues
	//{
	//	Gear gearForID = GearHelper.GetGearForID(gearID);
	//	if (!GameManager.Get().GameMission.IsMissionTagActive(MissionData.s_missionTagTutorial) && !GearHelper.IsGearAllowed(m_characterType, actionTypeInt, gearForID))
	//	{
	//		Debug.LogWarning(string.Concat(new object[]
	//		{
	//			"Gear with ID ",
	//			gearID,
	//			" is not allowed on ability at index ",
	//			actionTypeInt,
	//			" for character ",
	//			m_characterType.ToString()
	//		}));
	//		return;
	//	}
	//	AbilityData component = base.GetComponent<AbilityData>();
	//	if (component != null)
	//	{
	//		Ability abilityOfActionType = component.GetAbilityOfActionType((AbilityData.ActionType)actionTypeInt);
	//		if (gearForID != null)
	//		{
	//			ApplyGearToAbility(abilityOfActionType, gearForID, false);
	//			if (NetworkServer.active)
	//			{
	//				CallRpcApplyGearById(actionTypeInt, gearID);
	//			}
	//		}
	//	}
	//}

	internal void DebugApplyModToAbility(int actionTypeInt, int abilityScopeId)
	{
	}

	//private void ApplyGearToAbility(Ability ability, Gear gear, bool log = false)  // rogues
	//{
	//	ability.ApplyGear(gear, this);
	//	if (log)
	//	{
	//		Debug.LogWarning("Applied " + gear.GetDebugIdentifier("white") + " to ability " + ability.GetDebugIdentifier("orange"));
	//	}
	//}

	private void ApplyAbilityModToAbility(Ability ability, AbilityMod abilityMod, bool log = false)
	{
		if (ability.GetType() != abilityMod.GetTargetAbilityType()) // && !(abilityMod is GenericAbility_AbilityMod) in rogues
		{
			return;
		}
		ability.ApplyAbilityMod(abilityMod, this);
		if (abilityMod.m_useChainAbilityOverrides)
		{
			ability.SanitizeChainAbilities();
			GetAbilityData().ReInitializeChainAbilityList();
			Ability[] chainAbilities = ability.GetChainAbilities();
			if (chainAbilities != null)
			{
				foreach (Ability ability2 in chainAbilities)
				{
					if (ability2 != null)
					{
						ability2.sprite = ability.sprite;
					}
				}
			}
		}
		if (log)
		{
			Debug.LogWarning("Applied " + abilityMod.GetDebugIdentifier("white") + " to ability " + ability.GetDebugIdentifier("orange"));
		}
	}

	[ClientRpc]
	public void RpcMarkForRecalculateClientVisibility()
	{
		if (GetFogOfWar() != null)
		{
			GetFogOfWar().MarkForRecalculateVisibility();
		}
	}

	public void ShowRespawnFlare(BoardSquare flareSquare, bool respawningThisTurn)
	{
		bool flag = GameFlowData.Get().LocalPlayerData != null && GameFlowData.Get().LocalPlayerData.GetTeamViewing() == GetTeam();
		bool flag2 = false;
		if (m_respawnPositionFlare != null)
		{
			flag2 = m_respawnFlareVfxSquare == flareSquare && m_respawnFlareForSameTeam == flag;
			Destroy(m_respawnPositionFlare);
			m_respawnPositionFlare = null;
			UICharacterMovementPanel.Get().RemoveRespawnIndicator(this);
			m_respawnFlareVfxSquare = null;
			m_respawnFlareForSameTeam = false;
		}
		// m_spawnInDuringMovement -> m_playersSelectRespawn in rogues
		if ((SpawnPointManager.Get() == null || SpawnPointManager.Get().m_spawnInDuringMovement)
			&& flareSquare != null)
		{
			GameObject original;
			if (!flag)
			{
				original = respawningThisTurn
					? HighlightUtils.Get().m_respawnPositionFinalEnemyVFXPrefab
					: HighlightUtils.Get().m_respawnPositionFlareEnemyVFXPrefab;
			}
			else if (respawningThisTurn)
			{
				original = HighlightUtils.Get().m_respawnPositionFinalFriendlyVFXPrefab;
			}
			else
			{
				original = HighlightUtils.Get().m_respawnPositionFlareFriendlyVFXPrefab;
			}
			m_respawnPositionFlare = Instantiate(original);
			m_respawnFlareVfxSquare = flareSquare;
			m_respawnFlareForSameTeam = flag;
			if (!flag2 && this == GameFlowData.Get().activeOwnedActorData)
			{
				UISounds.GetUISounds().Play("ui/ingame/v1/respawn_locator");
			}
			m_respawnPositionFlare.transform.position = flareSquare.ToVector3();
			UICharacterMovementPanel.Get().AddRespawnIndicator(flareSquare, this);
		}
	}

	[ClientRpc]
	public void RpcForceLeaveGame(GameResult gameResult)
	{
		if (GameFlowData.Get().activeOwnedActorData == this && !ClientGameManager.Get().IsFastForward)  // IsFastForward not checked in rogues
		{
			ClientGameManager.Get().LeaveGame(false, gameResult);
		}
	}

#if SERVER
	public void PingOnClient(int teamIndex, Vector3 worldPosition, ActorController.PingType pingType, bool spam)
	{
		// custom log
		Log.Info($"Ping {m_displayName} {pingType} {Board.Get()?.GetSquareFromVec3(worldPosition)?.GetGridPos().ToString() ?? worldPosition.ToString()} "
		         + $"{m_team} ({teamIndex}){(spam ? " SPAM" : "")} to "
		         + string.Join(", ", GameFlowData.Get().GetActors()
			         .Where(a => a.GetTeam() == m_team)
			         .Select(a => $"{a.m_displayName} {a.GetTravelBoardSquare()?.GetGridPos()}")
			         .ToArray()));
		
		if (m_teamSensitiveData_friendly != null)
		{
			m_teamSensitiveData_friendly.CallRpcReceivedPingInfo(teamIndex, worldPosition, pingType, spam);
		}
	}
#endif

#if SERVER
	// custom
	public void PingOnClient(int teamIndex, LocalizationArg_AbilityPing localizedPing, bool spam)
	{
		if (m_teamSensitiveData_friendly != null)
		{
			m_teamSensitiveData_friendly.CallRpcReceivedAbilityPingInfo(teamIndex, localizedPing, spam);
		}
	}
#endif

	public void SendPingRequestToServer(int teamIndex, Vector3 worldPosition, ActorController.PingType pingType)
	{
		if (GetActorController() != null)
		{
			GetActorController().CallCmdSendMinimapPing(teamIndex, worldPosition, pingType);
		}
	}

	// removed in rogues
	public void SendAbilityPingRequestToServer(int teamIndex, LocalizationArg_AbilityPing localizedPing)
	{
		if (GetActorController() != null)
		{
			GetActorController().CallCmdSendAbilityPing(teamIndex, localizedPing);
		}
	}

	public override string ToString()
	{
		return $"[ActorData: {m_displayName}, {GetClassName()}, ActorIndex: {m_actorIndex}, {m_team}] {PlayerData}";
	}

	public string DebugNameString()
	{
		return "[" + GetClassName() + " (" + DisplayName + "), " + ActorIndex + "]";
	}

	public string DebugNameString(string color)
	{
		return "<color=" + color + ">" + DebugNameString() + "</color>";
	}

	public string DebugHpEnergyString()
	{
		int num = ExpectedHoTTotal + ClientExpectedHoTTotalAdjust;
		int expectedHoTThisTurn = ExpectedHoTThisTurn;
		int clientAppliedHoTThisTurn = ClientAppliedHoTThisTurn;
		return "Max HP: " + GetMaxHitPoints()
			+ "\nHP to Display: " + GetHitPointsToDisplay()
			+ "\n HP: " + HitPoints
			+ "\n Damage: " + UnresolvedDamage
			+ "\n Healing: " + UnresolvedHealing
			+ "\n Absorb: " + AbsorbPoints
			+ "\n CL Damage: " + ClientUnresolvedDamage
			+ "\n CL Healing: " + ClientUnresolvedHealing
			+ "\n CL Absorb: " + ClientUnresolvedAbsorb
			+ "\n\n Energy to Display: " + GetTechPointsToDisplay()
			+ "\n  Energy: " + TechPoints
			+ "\n Reserved Energy: " + ReservedTechPoints
			+ "\n EnergyGain: " + UnresolvedTechPointGain
			+ "\n EnergyLoss: " + UnresolvedTechPointLoss
			+ "\n CL Reserved Energy: " + ClientReservedTechPoints
			+ "\n CL EnergyGain: " + ClientUnresolvedTechPointGain
			+ "\n CL EnergyLoss: " + ClientUnresolvedTechPointLoss
			+ "\n CL Total HoT: " + num
			+ "\n CL HoT This Turn/Applied: " + expectedHoTThisTurn + " / " + clientAppliedHoTThisTurn;
	}

	public string DebugFSMStateString()
	{
		string text = "";
		if (GetActorTurnSM() != null)
		{
			text = string.Concat(text, "ActorTurnSM: CurrentState= ", this.GetActorTurnSM().CurrentState, " | PrevState= ", this.GetActorTurnSM().PreviousState, "\n");

			// rogues
			//text = text + GetActorTurnSM().GetUsedActionsDebugString() + "\n";
		}
		return text;
	}

	public ActorData[] AsArray()
	{
		return new ActorData[]
		{
			this
		};
	}

	public List<ActorData> AsList()
	{
		return new List<ActorData>
		{
			this
		};
	}

	private void OnDrawGizmos()
	{
		if (CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			Gizmos.color = Color.green;
			if (GetCurrentBoardSquare() != null)
			{
				Gizmos.DrawWireCube(GetCurrentBoardSquare().CameraBounds.center, GetCurrentBoardSquare().CameraBounds.size * 0.9f);
				Gizmos.DrawRay(GetCurrentBoardSquare().ToVector3(), transform.forward);
			}
		}
	}

	public bool HasTag(string tag)
	{
		return m_actorTags != null && m_actorTags.HasTag(tag);
	}

	public void AddTag(string tag)
	{
		if (m_actorTags == null)
		{
			m_actorTags = gameObject.AddComponent<ActorTag>();
		}
		m_actorTags.AddTag(tag);
	}

	public void RemoveTag(string tag)
	{
		if (m_actorTags != null)
		{
			m_actorTags.RemoveTag(tag);
		}
	}

	// TODO server-only or rogues?
//#if SERVER
//	public IEnumerable<string> GetAllActorTags()
//	{
//		if (m_actorTags == null)
//		{
//			return null;
//		}
//		return m_actorTags.GetAllTags();
//	}
//#endif

	public CharacterResourceLink GetCharacterResourceLink()
	{
		if (m_characterResourceLink == null && m_characterType != CharacterType.None)
		{
			GameWideData gameWideData = GameWideData.Get();
			if (gameWideData)
			{
				m_characterResourceLink = gameWideData.GetCharacterResourceLink(m_characterType);
			}
		}
		return m_characterResourceLink;
	}

	public GameObject ReplaceSequence(GameObject originalSequencePrefab)
	{
		if (originalSequencePrefab == null)
		{
			return null;
		}
		CharacterResourceLink characterResourceLink = GetCharacterResourceLink();
		if (characterResourceLink == null)
		{
			return originalSequencePrefab;
		}
		return characterResourceLink.ReplaceSequence(originalSequencePrefab, m_visualInfo, m_abilityVfxSwapInfo);
	}

	public void OnAnimEvent(UnityEngine.Object eventObject, GameObject sourceObject)
	{
		OnAnimationEventDelegates?.Invoke(eventObject, sourceObject);
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.GametimeScaleChange)
		{
			Animator modelAnimator = GetModelAnimator();
			if (modelAnimator != null)
			{
				modelAnimator.speed = GameTime.scale;
			}
		}
	}

	// removed in rogues
	private void UNetVersion() 
	{
	}

	protected static void InvokeCmdCmdSetPausedForDebugging(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetPausedForDebugging called on client.");
			return;
		}
		((ActorData)obj).CmdSetPausedForDebugging(reader.ReadBoolean());
	}

	protected static void InvokeCmdCmdSetResolutionSingleStepping(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetResolutionSingleStepping called on client.");
			return;
		}
		((ActorData)obj).CmdSetResolutionSingleStepping(reader.ReadBoolean());
	}

	protected static void InvokeCmdCmdSetResolutionSingleSteppingAdvance(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetResolutionSingleSteppingAdvance called on client.");
			return;
		}
		((ActorData)obj).CmdSetResolutionSingleSteppingAdvance();
	}

	protected static void InvokeCmdCmdSetDebugToggleParam(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetDebugToggleParam called on client.");
			return;
		}
		((ActorData)obj).CmdSetDebugToggleParam(reader.ReadString(), reader.ReadBoolean());
	}

	protected static void InvokeCmdCmdDebugReslotCards(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDebugReslotCards called on client.");
			return;
		}
		((ActorData)obj).CmdDebugReslotCards(reader.ReadBoolean(), (int)reader.ReadPackedUInt32());  // parameters removed in rogues
	}

	//removed in rogues
	protected static void InvokeCmdCmdDebugSetAbilityMod(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDebugSetAbilityMod called on client.");
			return;
		}
		((ActorData)obj).CmdDebugSetAbilityMod((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdDebugReplaceWithBot(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDebugReplaceWithBot called on client.");
			return;
		}
		((ActorData)obj).CmdDebugReplaceWithBot();
	}

	protected static void InvokeCmdCmdDebugSetHealthOrEnergy(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDebugSetHealthOrEnergy called on client.");
			return;
		}
		((ActorData)obj).CmdDebugSetHealthOrEnergy((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());  // all ints in rogues
	}

	public void CallCmdSetPausedForDebugging(bool pause)
	{
		if (!NetworkClient.active)  // removed in rogues
		{
			Debug.LogError("Command function CmdSetPausedForDebugging called on server.");
			return;
		}
		if (isServer)
		{
			CmdSetPausedForDebugging(pause);
			return;
		}

		// reactor
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdSetPausedForDebugging);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(pause);
		SendCommandInternal(networkWriter, 0, "CmdSetPausedForDebugging");
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//networkWriter.Write(pause);
		//base.SendCommandInternal(typeof(ActorData), "CmdSetPausedForDebugging", networkWriter, 0);
	}

	public void CallCmdSetResolutionSingleStepping(bool singleStepping)
	{
		if (!NetworkClient.active)  // removed in rogues
		{
			Debug.LogError("Command function CmdSetResolutionSingleStepping called on server.");
			return;
		}
		if (isServer)
		{
			CmdSetResolutionSingleStepping(singleStepping);
			return;
		}

		// reactor
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdSetResolutionSingleStepping);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(singleStepping);
		SendCommandInternal(networkWriter, 0, "CmdSetResolutionSingleStepping");
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//networkWriter.Write(singleStepping);
		//base.SendCommandInternal(typeof(ActorData), "CmdSetResolutionSingleStepping", networkWriter, 0);
	}

	public void CallCmdSetResolutionSingleSteppingAdvance()
	{
		if (!NetworkClient.active)  // removed in rogues
		{
			Debug.LogError("Command function CmdSetResolutionSingleSteppingAdvance called on server.");
			return;
		}
		if (isServer)
		{
			CmdSetResolutionSingleSteppingAdvance();
			return;
		}

		// reactor
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdSetResolutionSingleSteppingAdvance);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, "CmdSetResolutionSingleSteppingAdvance");
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//base.SendCommandInternal(typeof(ActorData), "CmdSetResolutionSingleSteppingAdvance", networkWriter, 0);
	}

	public void CallCmdSetDebugToggleParam(string name, bool value)
	{
		if (!NetworkClient.active)  // removed in rogues
		{
			Debug.LogError("Command function CmdSetDebugToggleParam called on server.");
			return;
		}
		if (isServer)
		{
			CmdSetDebugToggleParam(name, value);
			return;
		}

		// reactor
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdSetDebugToggleParam);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(name);
		networkWriter.Write(value);
		SendCommandInternal(networkWriter, 0, "CmdSetDebugToggleParam");
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//networkWriter.Write(name);
		//networkWriter.Write(value);
		//base.SendCommandInternal(typeof(ActorData), "CmdSetDebugToggleParam", networkWriter, 0);
	}

	public void CallCmdDebugReslotCards(bool reslotAll, int cardTypeInt)  // no params in rogues
	{
		if (!NetworkClient.active)  // removed in rogues
		{
			Debug.LogError("Command function CmdDebugReslotCards called on server.");
			return;
		}
		if (isServer)
		{
			CmdDebugReslotCards(reslotAll, cardTypeInt);  // no params in rogues
			return;
		}

		// reactor
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdDebugReslotCards);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(reslotAll);
		networkWriter.WritePackedUInt32((uint)cardTypeInt);
		SendCommandInternal(networkWriter, 0, "CmdDebugReslotCards");
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//base.SendCommandInternal(typeof(ActorData), "CmdDebugReslotCards", networkWriter, 0);
	}

	// removed in rogues
	public void CallCmdDebugSetAbilityMod(int abilityIndex, int modId)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdDebugSetAbilityMod called on server.");
			return;
		}
		if (isServer)
		{
			CmdDebugSetAbilityMod(abilityIndex, modId);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdDebugSetAbilityMod);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)abilityIndex);
		networkWriter.WritePackedUInt32((uint)modId);
		SendCommandInternal(networkWriter, 0, "CmdDebugSetAbilityMod");
	}

	// removed in rogues
	public void CallCmdDebugReplaceWithBot()
	{
		if (!NetworkClient.active)  
		{
			Debug.LogError("Command function CmdDebugReplaceWithBot called on server.");
			return;
		}
		if (isServer)
		{
			CmdDebugReplaceWithBot();
			return;
		}

		// reactor
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdDebugReplaceWithBot);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, "CmdDebugReplaceWithBot");
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//base.SendCommandInternal(typeof(ActorData), "CmdDebugReplaceWithBot", networkWriter, 0);
	}

	public void CallCmdDebugSetHealthOrEnergy(int actorIndex, int valueToSet, int flag)
	{
		if (!NetworkClient.active)  // removed in rogues
		{
			Debug.LogError("Command function CmdDebugSetHealthOrEnergy called on server.");
			return;
		}
		if (isServer)
		{
			CmdDebugSetHealthOrEnergy(actorIndex, valueToSet, flag);
			return;
		}

		// reactor
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdDebugSetHealthOrEnergy);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actorIndex);
		networkWriter.WritePackedUInt32((uint)valueToSet);
		networkWriter.WritePackedUInt32((uint)flag);
		SendCommandInternal(networkWriter, 0, "CmdDebugSetHealthOrEnergy");
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//networkWriter.WritePackedInt32(actorIndex);
		//networkWriter.WritePackedInt32(valueToSet);
		//networkWriter.WritePackedInt32(flag);
		//base.SendCommandInternal(typeof(ActorData), "CmdDebugSetHealthOrEnergy", networkWriter, 0);
	}

	protected static void InvokeRpcRpcOnHitPointsResolved(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcOnHitPointsResolved called on server.");
			return;
		}
		((ActorData)obj).RpcOnHitPointsResolved((int)reader.ReadPackedUInt32());  // int32 in rogues
	}

	// rogues
	//protected static void InvokeRpcRpcOnTechPointsResolved(NetworkBehaviour obj, NetworkReader reader)
	//{
	//	if (!NetworkClient.active)
	//	{
	//		Debug.LogError("RPC RpcOnTechPointsResolved called on server.");
	//		return;
	//	}
	//	((ActorData)obj).RpcOnTechPointsResolved(reader.ReadPackedInt32());
	//}

	protected static void InvokeRpcRpcCombatText(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcCombatText called on server.");
			return;
		}
		((ActorData)obj).RpcCombatText(reader.ReadString(), reader.ReadString(), (CombatTextCategory)reader.ReadInt32(), (BuffIconToDisplay)reader.ReadInt32()); // int32 -> packed int32 in rogues
	}

	protected static void InvokeRpcRpcApplyAbilityModById(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcApplyAbilityModById called on server.");
			return;
		}
		// reactor
		((ActorData)obj).RpcApplyAbilityModById((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
		// rogues
		//((ActorData)obj).RpcApplyAbilityModById(reader.ReadPackedInt32(), reader.ReadPackedInt32());
	}

	//protected static void InvokeRpcRpcApplyGearById(NetworkBehaviour obj, NetworkReader reader)  // rogues
	//{
	//	if (!NetworkClient.active)
	//	{
	//		Debug.LogError("RPC RpcApplyGearById called on server.");
	//		return;
	//	}
	//	((ActorData)obj).RpcApplyGearById(reader.ReadPackedInt32(), reader.ReadPackedInt32());
	//}

	protected static void InvokeRpcRpcMarkForRecalculateClientVisibility(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcMarkForRecalculateClientVisibility called on server.");
			return;
		}
		((ActorData)obj).RpcMarkForRecalculateClientVisibility();
	}

	protected static void InvokeRpcRpcForceLeaveGame(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcForceLeaveGame called on server.");
			return;
		}
		// reactor
		((ActorData)obj).RpcForceLeaveGame((GameResult)reader.ReadInt32());
		// rogues
		//((ActorData)obj).RpcForceLeaveGame((GameResult)reader.ReadPackedInt32());
	}

	public void CallRpcOnHitPointsResolved(int resolvedHitPoints)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcOnHitPointsResolved called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcOnHitPointsResolved);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)resolvedHitPoints);
		SendRPCInternal(networkWriter, 0, "RpcOnHitPointsResolved");
	}

	//public void CallRpcOnHitPointsResolved(int resolvedHitPoints)  // rogues
	//{
	//	NetworkWriter networkWriter = new NetworkWriter();
	//	networkWriter.WritePackedInt32(resolvedHitPoints);
	//	SendRPCInternal(typeof(ActorData), "RpcOnHitPointsResolved", networkWriter, 0);
	//}

	//public void CallRpcOnTechPointsResolved(int resolvedTechPoints)  // rogues
	//{
	//	NetworkWriter networkWriter = new NetworkWriter();
	//	networkWriter.WritePackedInt32(resolvedTechPoints);
	//	SendRPCInternal(typeof(ActorData), "RpcOnTechPointsResolved", networkWriter, 0);
	//}

	public void CallRpcCombatText(string combatText, string logText, CombatTextCategory category, BuffIconToDisplay icon)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcCombatText called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcCombatText);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(combatText);
		networkWriter.Write(logText);
		networkWriter.Write((int)category);
		networkWriter.Write((int)icon);
		SendRPCInternal(networkWriter, 0, "RpcCombatText");
	}

	//public void CallRpcCombatText(string combatText, string logText, CombatTextCategory category, BuffIconToDisplay icon)  // rogues
	//{
	//	NetworkWriter networkWriter = new NetworkWriter();
	//	networkWriter.Write(combatText);
	//	networkWriter.Write(logText);
	//	networkWriter.WritePackedInt32((int)category);
	//	networkWriter.WritePackedInt32((int)icon);
	//	SendRPCInternal(typeof(ActorData), "RpcCombatText", networkWriter, 0);
	//}

	public void CallRpcApplyAbilityModById(int actionTypeInt, int abilityScopeId)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcApplyAbilityModById called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcApplyAbilityModById);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actionTypeInt);
		networkWriter.WritePackedUInt32((uint)abilityScopeId);
		SendRPCInternal(networkWriter, 0, "RpcApplyAbilityModById");
	}

	//public void CallRpcApplyAbilityModById(int actionTypeInt, int abilityScopeId)  // rogues
	//{
	//	NetworkWriter networkWriter = new NetworkWriter();
	//	networkWriter.WritePackedInt32(actionTypeInt);
	//	networkWriter.WritePackedInt32(abilityScopeId);
	//	SendRPCInternal(typeof(ActorData), "RpcApplyAbilityModById", networkWriter, 0);
	//}

	//public void CallRpcApplyGearById(int actionTypeInt, int gearID)  // rogues
	//{
	//	NetworkWriter networkWriter = new NetworkWriter();
	//	networkWriter.WritePackedInt32(actionTypeInt);
	//	networkWriter.WritePackedInt32(gearID);
	//	SendRPCInternal(typeof(ActorData), "RpcApplyGearById", networkWriter, 0);
	//}

	public void CallRpcMarkForRecalculateClientVisibility()
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcMarkForRecalculateClientVisibility called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcMarkForRecalculateClientVisibility);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendRPCInternal(networkWriter, 0, "RpcMarkForRecalculateClientVisibility");
	}

	//public void CallRpcMarkForRecalculateClientVisibility()  // rogues
	//{
	//	NetworkWriter networkWriter = new NetworkWriter();
	//	SendRPCInternal(typeof(ActorData), "RpcMarkForRecalculateClientVisibility", networkWriter, 0);
	//}

	public void CallRpcForceLeaveGame(GameResult gameResult)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcForceLeaveGame called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcForceLeaveGame);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write((int)gameResult);
		SendRPCInternal(networkWriter, 0, "RpcForceLeaveGame");
	}

	//public void CallRpcForceLeaveGame(GameResult gameResult)  // rogues
	//{
	//	NetworkWriter networkWriter = new NetworkWriter();
	//	networkWriter.WritePackedInt32((int)gameResult);
	//	SendRPCInternal(typeof(ActorData), "RpcForceLeaveGame", networkWriter, 0);
	//}

	// added in rogues
#if SERVER
	public ActorTeamSensitiveData TeamSensitiveData_friendly
	{
		get
		{
			return m_teamSensitiveData_friendly;
		}
	}
#endif

	// added in rogues
#if SERVER
	private void OnSpawnerIdUpdated(int spawnerId)
	{
		if (!NetworkServer.active && spawnerId > -1)
		{
			InitModel();
		}
	}
#endif

	// added in rogues
#if SERVER
	public void SetServerLastKnownPosSquare(BoardSquare square, string callerStr)  // added in rogues
	{
		// rogues
		//      if (NetworkServer.active && (square == null || m_serverLastKnownPosX != square.x || m_serverLastKnownPosY != square.y))
		//      {
		//          ServerLastKnownPosSquare = square;
		//}

		// custom
		BoardSquare oldSquare = ServerLastKnownPosSquare;
		if (NetworkServer.active
		    && (square == null
		        || ServerLastKnownPosSquare == null
		        || ServerLastKnownPosSquare.x != square.x
		        || ServerLastKnownPosSquare.y != square.y))
		{
			ServerLastKnownPosSquare = square;
			
			// TODO LOW It doesn't look like this is how it was handled in the original server,
			//  but otherwise position is updated on the client too late
			//  (e.g. player gets hit, plays damage animation while standing on wrong square, and then teleports)
			if (ServerLastKnownPosSquare != null)
			{
				Log.Info($"ServerLastKnownPosSquare {DisplayName} {callerStr}" +
				         $"{oldSquare?.GetGridPos().ToString() ?? "null"} -> {square?.GetGridPos().ToString() ?? "null"}"); // custom debug
				TeamSensitiveData_hostile.BroadcastMovement(
					GameEventManager.EventType.ClientResolutionStarted,
					ServerLastKnownPosSquare.GetGridPos(),
					ServerLastKnownPosSquare,
					MovementType.None,
					TeleportType.Reappear,
					null);
			}
		}
		else
		{
			Log.Info($"ServerLastKnownPosSquare NOT BROADCASTING {DisplayName} {callerStr}" +
			         $"{oldSquare?.GetGridPos().ToString() ?? "null"} -> {square?.GetGridPos().ToString() ?? "null"}"); // custom debug
		}
		// end custom

		// TODO LOW check m_serverLastKnownPosX/Y
	}
#endif

	//internal void SetupAbilityGearOnReconnect()  // rogues
	//{
	//	using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().GetActors().GetEnumerator())
	//	{
	//		while (enumerator.MoveNext())
	//		{
	//			ActorData actor = enumerator.Current;
	//			IEnumerable<Gear> enumerable = from g in RunManager.Get().Inventory.Gears.Values
	//			where g.EquippedTo == actor.m_characterType
	//			select g;
	//			AbilityData abilityData = actor.GetAbilityData();
	//			if (actor != null && abilityData != null)
	//			{
	//				foreach (Gear gear in enumerable)
	//				{
	//					Ability abilityOfActionType = abilityData.GetAbilityOfActionType((AbilityData.ActionType)gear.Slot);
	//					actor.ApplyGearToAbility(abilityOfActionType, gear, false);
	//				}
	//				ActorTargeting component = actor.GetComponent<ActorTargeting>();
	//				if (component != null)
	//				{
	//					component.MarkForForceRedraw();
	//				}
	//			}
	//		}
	//	}
	//}

	// rogues
	//public int CountEffectsWithTag(string tag)
	//{
	//	return CountEffectsWithAllTags(new string[]
	//	{
	//		tag
	//	});
	//}

	// rogues
	//public int CountEffectsWithAllTags(IEnumerable<string> tags)
	//{
	//	return ServerEffectManager.Get().GetActorEffects(this).Count(delegate (global::Effect effect)
	//	{
	//		EffectSystem.Effect effectSystemEffect;
	//		return (effectSystemEffect = (effect as EffectSystem.Effect)) != null && tags.All((string tag) => effectSystemEffect.EffectTemplate.tags.Contains(tag));
	//	});
	//}

	// added in rogues
#if SERVER
	public void GenerateEventData(EventLogMessage eventLogMessage, bool condensed = true)
	{
		eventLogMessage.AddData("Name", m_displayName);
		if (PlayerData != null && GameFlow.Get() != null && GameFlow.Get().playerDetails != null && GameFlow.Get().playerDetails.ContainsKey(PlayerData.GetPlayer()))
		{
			PlayerDetails playerDetails = GameFlow.Get().playerDetails[PlayerData.GetPlayer()];
			if (playerDetails != null)
			{
				eventLogMessage.AddData("AccountId", playerDetails.m_accountId);
			}
		}
		eventLogMessage.AddData("CharacterType", m_characterType);
		CharacterResourceLink characterResourceLink = GetCharacterResourceLink();
		eventLogMessage.AddData("CharacterName", characterResourceLink ? characterResourceLink.m_displayName : "?");
		eventLogMessage.AddData("Team", (int)m_team);
		GridPos gridPos = GetGridPos();
		eventLogMessage.AddData("GridPosX", gridPos.x);
		eventLogMessage.AddData("GridPosY", gridPos.y);
		if (!condensed)
		{
			eventLogMessage.AddData("IsBotControlled", m_hasBotController);
		}
	}
#endif

	// added in rogues
#if SERVER
	public static List<ActorData> GetAllActorsInShapeCenteredOn(AbilityAreaShape shape, ActorData actor, bool ignoreLOS, Team team)
	{
		new List<ActorData>();
		return AreaEffectUtils.GetActorsInShape(shape, actor.GetFreePos(), actor.CurrentBoardSquare, ignoreLOS, actor, team, null);
	}
#endif

	// added in rogues
#if SERVER
	public static List<ActorData> GetAllTargets(ActorData actor, Team team)
	{
		List<ActorData> list = new List<ActorData>();
		List<ActorData> actors = GameFlowData.Get().GetActors();
		int num = 0;
		ActorTargeting actorTargeting = actor.GetActorTargeting();
		if (actorTargeting != null)
		{
			bool flag = false;
			foreach (ActorData actorData in actors)
			{
				Ability selectedAbility = actor.GetAbilityData().GetSelectedAbility();
				if (selectedAbility != null && selectedAbility.Targeters != null)
				{
					flag = selectedAbility.IsActorInTargetRange(actorData);  // , out HitChanceBracketType hitChanceBracketType in rogues
				}
				if (!list.Contains(actorData) && (actorTargeting.IsTargetingActor(actorData, (team == Team.TeamA) ? AbilityTooltipSymbol.Healing : AbilityTooltipSymbol.Damage, ref num) || (flag && !actorData.IsDead() && actorData.IsActorVisibleToActor(actor) && actorData.m_team == team)))
				{
					list.Add(actorData);
				}
			}
		}
		return list;
	}
#endif

	// TODO SCRIPT rogues?? like PveScript???
	//public List<Ability> GetAbilitiesFromScriptTags(List<string> tags)
	//{
	//	List<Ability> list = new List<Ability>();
	//	foreach (Ability ability in GetAbilityData().GetAbilitiesAsList())
	//	{
	//		if (tags.Except(ability.m_scriptTags).Count<string>() == 0)
	//		{
	//			list.Add(ability);
	//		}
	//	}
	//	return list;
	//}

	// TODO SCRIPT rogues??
	//private void RegisterScriptData()
	//{
	//	m_script = new Script();
	//	m_script.Options.DebugPrint = delegate (string aString)
	//	{
	//		Debug.Log(aString);
	//	};
	//	UserData.RegisterType(typeof(ActorData), 7, null);
	//	UserData.RegisterType(typeof(AbilityAreaShape), 7, null);
	//	UserData.RegisterType(typeof(Team), 7, null);
	//	UserData.RegisterType(typeof(Ability), 7, null);
	//	ScriptTemplate.RegisterClass<ActorData>();
	//	ScriptTemplate.RegisterClass<PveGameplayData>();
	//	m_script.Globals.Set("Shape", UserData.CreateStatic(typeof(AbilityAreaShape)));
	//	m_script.Globals.Set("Team", UserData.CreateStatic(typeof(Team)));
	//	m_script.Globals.Set("Caster", DynValue.FromObject(m_script, this));
	//	m_script.Globals["GetActorsInShape"] = new Func<ActorData, AbilityAreaShape, bool, Team, List<ActorData>>((ActorData target, AbilityAreaShape shape, bool ignorelos, Team team) => ActorData.GetAllActorsInShapeCenteredOn(shape, target, ignorelos, team));
	//	m_script.Globals["GetAllTargets"] = new Func<Team, List<ActorData>>((Team team) => ActorData.GetAllTargets(this, team));
	//	m_script.Globals["PveGameplayData"] = PveGameplayData.Get();
	//}

	// all added in rogues, no syncvars in reactor
	//public int NetworkPlayerIndex
	//{
	//	get
	//	{
	//		return PlayerIndex;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		if (NetworkServer.localClientActive && !syncVarHookGuard)
	//		{
	//			syncVarHookGuard = true;
	//			OnPlayerIndexUpdated(value);
	//			syncVarHookGuard = false;
	//		}
	//		SetSyncVar(value, ref PlayerIndex, 1U);
	//	}
	//}

	//public bool Networkm_alwaysHideNameplate
	//{
	//	get
	//	{
	//		return m_alwaysHideNameplate;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		SetSyncVar(value, ref m_alwaysHideNameplate, 2U);
	//	}
	//}

	//public CharacterVisualInfo Networkm_visualInfo
	//{
	//	get
	//	{
	//		return m_visualInfo;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		SetSyncVar(value, ref m_visualInfo, 4U);
	//	}
	//}

	//public CharacterAbilityVfxSwapInfo Networkm_abilityVfxSwapInfo
	//{
	//	get
	//	{
	//		return m_abilityVfxSwapInfo;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		SetSyncVar(value, ref m_abilityVfxSwapInfo, 8U);
	//	}
	//}

	//public Team Networkm_team
	//{
	//	get
	//	{
	//		return m_team;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		SetSyncVar(value, ref m_team, 16U);
	//	}
	//}

	//public int Networkm_lastVisibleTurnToClient
	//{
	//	get
	//	{
	//		return m_lastVisibleTurnToClient;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		SetSyncVar(value, ref m_lastVisibleTurnToClient, 32U);
	//	}
	//}

	//public int Networkm_serverLastKnownPosX
	//{
	//	get
	//	{
	//		return m_serverLastKnownPosX;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<int>(value, ref m_serverLastKnownPosX, 64UL);
	//	}
	//}

	//public int Networkm_serverLastKnownPosY
	//{
	//	get
	//	{
	//		return m_serverLastKnownPosY;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<int>(value, ref m_serverLastKnownPosY, 128UL);
	//	}
	//}

	// rogues
	//public byte Networkm_currentMovementFlags
	//{
	//	get
	//	{
	//		return m_currentMovementFlags;
	//	}
	//	[param: In]
	//	set
	//	{
	//		if (NetworkServer.localClientActive && !base.syncVarHookGuard)
	//		{
	//			base.syncVarHookGuard = true;
	//			OnCurrentMovementFlagsUpdated(value);
	//			base.syncVarHookGuard = false;
	//		}
	//		base.SetSyncVar<byte>(value, ref m_currentMovementFlags, 256UL);
	//	}
	//}

	// rogues?
	//public sbyte Networkm_queuedChaseTargetActorIndex
	//{
	//    get
	//    {
	//        return m_queuedChaseTargetActorIndex;
	//    }
	//    [param: In]
	//    set
	//    {
	//        base.SetSyncVar<sbyte>(value, ref m_queuedChaseTargetActorIndex, 512UL);
	//    }
	//}

	// rogues
	//public float Networkm_alertDist
	//{
	//	get
	//	{
	//		return m_alertDist;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<float>(value, ref m_alertDist, 1024UL);
	//	}
	//}

	// rogues
	//public bool Networkm_suspend
	//{
	//	get
	//	{
	//		return m_suspend;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<bool>(value, ref m_suspend, 2048UL);
	//	}
	//}

	// rogues
	//public CharacterGearInfo Networkm_selectedGear
	//{
	//	get
	//	{
	//		return m_selectedGear;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<CharacterGearInfo>(value, ref m_selectedGear, 4096UL);
	//	}
	//}

	//public CharacterCardInfo Networkm_selectedCards
	//{
	//	get
	//	{
	//		return m_selectedCards;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		SetSyncVar(value, ref m_selectedCards, 8192U);
	//	}
	//}

	//public string Networkm_displayName
	//{
	//	get
	//	{
	//		return m_displayName;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		SetSyncVar(value, ref m_displayName, 16384U);
	//	}
	//}

	//public int Networkm_actorIndex
	//{
	//	get
	//	{
	//		return m_actorIndex;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		if (NetworkServer.localClientActive && !syncVarHookGuard)
	//		{
	//			syncVarHookGuard = true;
	//			OnActorIndexUpdated(value);
	//			syncVarHookGuard = false;
	//		}
	//		SetSyncVar(value, ref m_actorIndex, 32768U);
	//	}
	//}

	//public bool Networkm_showInGameHud
	//{
	//	get
	//	{
	//		return m_showInGameHud;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		SetSyncVar(value, ref m_showInGameHud, 65536U);
	//	}
	//}

	//public int Networkm_hitPoints
	//{
	//	get
	//	{
	//		return m_hitPoints;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		SetSyncVar(value, ref m_hitPoints, 131072U);
	//	}
	//}

	//public int Network_unresolvedDamage
	//{
	//	get
	//	{
	//		return _unresolvedDamage;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		SetSyncVar(value, ref _unresolvedDamage, 262144U);
	//	}
	//}

	//public int Network_unresolvedHealing
	//{
	//	get
	//	{
	//		return _unresolvedHealing;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		SetSyncVar(value, ref _unresolvedHealing, 524288U);
	//	}
	//}

	//public int Network_unresolvedTechPointGain
	//{
	//	get
	//	{
	//		return _unresolvedTechPointGain;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		SetSyncVar(value, ref _unresolvedTechPointGain, 0x100000U);
	//	}
	//}

	//public int Network_unresolvedTechPointLoss
	//{
	//	get
	//	{
	//		return _unresolvedTechPointLoss;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		SetSyncVar(value, ref _unresolvedTechPointLoss, 0x200000U);
	//	}
	//}

	//public int Networkm_serverExpectedHoTTotal
	//{
	//	get
	//	{
	//		return m_serverExpectedHoTTotal;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		SetSyncVar(value, ref m_serverExpectedHoTTotal, 0x400000U);
	//	}
	//}

	//public int Networkm_serverExpectedHoTThisTurn
	//{
	//	get
	//	{
	//		return m_serverExpectedHoTThisTurn;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		//base.SetSyncVar<int>(value, ref m_serverExpectedHoTThisTurn, 0x800000UL);
	//		SetDirtyBit(0x1U); // we always replicate everyting on dirty anyway
	//	}
	//}

	//public int Networkm_techPoints
	//{
	//	get
	//	{
	//		return m_techPoints;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		//base.SetSyncVar<int>(value, ref m_techPoints, 0x1000000UL);
	//		SetDirtyBit(0x1U); // we always replicate everyting on dirty anyway
	//	}
	//}

	//public int Networkm_reservedTechPoints
	//{
	//	get
	//	{
	//		return m_reservedTechPoints;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		//base.SetSyncVar<int>(value, ref m_reservedTechPoints, 0x2000000UL);
	//		SetDirtyBit(0x1U); // we always replicate everyting on dirty anyway
	//	}
	//}

	//public bool Networkm_ignoreFromAbilityHits
	//{
	//	get
	//	{
	//		return m_ignoreFromAbilityHits;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		SetSyncVar(value, ref m_ignoreFromAbilityHits, 0x4000000U);
	//	}
	//}

	//public int Networkm_absorbPoints
	//{
	//	get
	//	{
	//		return m_absorbPoints;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		SetSyncVar(value, ref m_absorbPoints, 0x8000000U);
	//	}
	//}

	//public int Networkm_mechanicPoints
	//{
	//	get
	//	{
	//		return m_mechanicPoints;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		SetSyncVar(value, ref m_mechanicPoints, 0x10000000U);
	//	}
	//}

	//public int Networkm_spawnerId
	//{
	//	get
	//	{
	//		return m_spawnerId;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		if (NetworkServer.localClientActive && !syncVarHookGuard)
	//		{
	//			syncVarHookGuard = true;
	//			OnSpawnerIdUpdated(value);
	//			syncVarHookGuard = false;
	//		}
	//		SetSyncVar(value, ref m_spawnerId, 0x20000000U);
	//	}
	//}

	//public float Networkm_remainingHorizontalMovement
	//{
	//	get
	//	{
	//		return m_remainingHorizontalMovement;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		SetSyncVar(value, ref m_remainingHorizontalMovement, 0x40000000U);
	//	}
	//}

	//public float Networkm_remainingMovementWithQueuedAbility
	//{
	//	get
	//	{
	//		return m_remainingMovementWithQueuedAbility;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		SetSyncVar(value, ref m_remainingMovementWithQueuedAbility, 0x80000000U);
	//	}
	//}

	//public int Networkm_lastDeathTurn
	//{
	//	get
	//	{
	//		return m_lastDeathTurn;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		//base.SetSyncVar<int>(value, ref m_lastDeathTurn, 0x100000000U);
	//		SetDirtyBit(0x1U); // we always replicate everyting on dirty anyway
	//	}
	//}

	//public int Networkm_lastSpawnTurn
	//{
	//	get
	//	{
	//		return m_lastSpawnTurn;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		//base.SetSyncVar<int>(value, ref m_lastSpawnTurn, 0x200000000U);
	//		SetDirtyBit(0x1U); // we always replicate everyting on dirty anyway
	//	}
	//}

	//public int Networkm_nextRespawnTurn
	//{
	//	get
	//	{
	//		return m_nextRespawnTurn;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		//base.SetSyncVar<int>(value, ref m_nextRespawnTurn, 0x400000000U);
	//		SetDirtyBit(0x1U); // we always replicate everyting on dirty anyway
	//	}
	//}

	//public bool Networkm_hasBotController
	//{
	//	get
	//	{
	//		return m_hasBotController;
	//	}
	//	//[param: In]
	//	set
	//	{
	//		//base.SetSyncVar<bool>(value, ref m_hasBotController, 0x800000000U);
	//		SetDirtyBit(0x1U); // we always replicate everyting on dirty anyway
	//	}
	//}


	//public int Networkm_turnPriority
	//{
	//	get
	//	{
	//		return m_turnPriority;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<int>(value, ref m_turnPriority, 68719476736UL);
	//	}
	//}

	// rogues
	//public bool Networkm_visibleTillEndOfPhase
	//{
	//	get
	//	{
	//		return m_visibleTillEndOfPhase;
	//	}
	//	[param: In]
	//	set
	//	{
	//		base.SetSyncVar<bool>(value, ref m_visibleTillEndOfPhase, 137438953472UL);
	//	}
	//}
}
