using Fabric;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
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
	}

	public enum MovementChangeType
	{
		MoreMovement,
		LessMovement
	}

	[HideInInspector]
	public int PlayerIndex = -1;
	[HideInInspector]
	public PlayerData PlayerData;

	[Separator("Character Type", true)]
	public CharacterType m_characterType;

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

	[AssetFileSelector("Assets/UI/Textures/Resources/CharacterIcons/", "CharacterIcons/", ".png")]
	[Header("-- Icon: Portrait, OffscreenIndicator, Team Panel --")]
	public string m_aliveHUDIconResourceString;
	[AssetFileSelector("Assets/UI/Textures/Resources/CharacterIcons/", "CharacterIcons/", ".png")]
	public string m_deadHUDIconResourceString;
	[Header("-- Icon: Last Known Position Indicator --")]
	[AssetFileSelector("Assets/UI/Textures/Resources/CharacterIcons/", "CharacterIcons/", ".png")]
	public string m_screenIndicatorIconResourceString;
	[AssetFileSelector("Assets/UI/Textures/Resources/CharacterIcons/", "CharacterIcons/", ".png")]
	public string m_screenIndicatorBWIconResourceString;

	public ActorDataDelegate m_onResolvedHitPoints;

	internal bool m_callHandleOnSelectInUpdate;
	internal bool m_hideNameplate;
	internal float m_endVisibilityForHitTime = -10000f;
	internal bool m_needAddToTeam;
	private bool m_alwaysHideNameplate;

	private ActorBehavior m_actorBehavior;
	private ActorModelData m_actorModelData;
	private ActorModelData m_faceActorModelData;
	private ActorMovement m_actorMovement;
	private ActorStats m_actorStats;
	private ActorStatus m_actorStatus;
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

	[HideInInspector]
	public PrefabResourceLink m_actorSkinPrefabLink;
	[HideInInspector]
	public CharacterVisualInfo m_visualInfo;
	[HideInInspector]
	public CharacterAbilityVfxSwapInfo m_abilityVfxSwapInfo;

	private bool m_setTeam;
	private Team m_team;
	private EasedQuaternionNoAccel m_targetRotation = new EasedQuaternionNoAccel(Quaternion.identity);
	private bool m_shouldUpdateLastVisibleToClientThisFrame = true;

	private float m_lastIsVisibleToClientTime;
	private bool m_isVisibleToClientCache;
	private int m_lastVisibleTurnToClient;
	private BoardSquare m_clientLastKnownPosSquare;
	private BoardSquare m_serverLastKnownPosSquare;

	private bool m_addedToUI;

	[HideInInspector]
	public CharacterModInfo m_selectedMods;
	[HideInInspector]
	public CharacterAbilityVfxSwapInfo m_selectedAbilityVfxSwaps;
	[HideInInspector]
	public CharacterCardInfo m_selectedCards;
	[HideInInspector]
	public List<int> m_availableTauntIDs;

	internal string m_displayName = "Connecting Player";
	private int m_actorIndex = s_invalidActorIndex;
	private bool m_showInGameHud = true;

	[Header("-- Stats --")]
	public int m_maxHitPoints = 100;
	public int m_hitPointRegen;
	[Space(5f)]
	public int m_maxTechPoints = 100;
	public int m_techPointRegen = 10;
	public int m_techPointsOnSpawn = 100;
	public int m_techPointsOnRespawn = 100;
	[Space(5f)]
	public float m_maxHorizontalMovement = 8f;
	public float m_postAbilityHorizontalMovement = 5f;
	public int m_maxVerticalUpwardMovement = 1;
	public int m_maxVerticalDownwardMovement = 2;
	public float m_sightRange = 10f;
	public float m_runSpeed = 8f;
	public float m_vaultSpeed = 4f;
	[Tooltip("The speed the actor travels when being knocked back by any ability.")]
	public float m_knockbackSpeed = 8f;

	[Header("-- Audio Events --")]
	[AudioEvent(false)]
	public string m_onDeathAudioEvent = "";

	[Header("-- Additional Network Objects to call Register Prefab on")]
	public List<GameObject> m_additionalNetworkObjectsToRegister;

	private int m_hitPoints = 1;
	private int _unresolvedDamage;
	private int _unresolvedHealing;
	private int _unresolvedTechPointGain;
	private int _unresolvedTechPointLoss;
	private int m_serverExpectedHoTTotal;
	private int m_serverExpectedHoTThisTurn;
	private int m_techPoints;
	private int m_reservedTechPoints;
	private bool m_ignoreForEnergyForHit;
	private bool m_ignoreFromAbilityHits;
	private int m_absorbPoints;
	private int m_mechanicPoints;
	private int m_spawnerId;
	private Vector3 m_facingDirAfterMovement;
	private GameEventManager.EventType m_serverMovementWaitForEvent;
	private BoardSquare m_serverMovementDestination;
	private BoardSquarePathInfo m_serverMovementPath;
	private bool m_disappearingAfterCurrentMovement;
	private BoardSquare m_clientCurrentBoardSquare;
	private BoardSquare m_mostRecentDeathSquare;
	private ActorTeamSensitiveData m_teamSensitiveData_friendly;
	private ActorTeamSensitiveData m_teamSensitiveData_hostile;
	private BoardSquare m_trueMoveFromBoardSquare;
	private BoardSquare m_serverInitialMoveStartSquare;
	private bool m_internalQueuedMovementAllowsAbility;
	private bool m_queuedMovementRequest;
	private bool m_queuedChaseRequest;
	private ActorData m_queuedChaseTarget;
	private bool m_knockbackMoveStarted;
	private int m_lastSpawnTurn = -1;
	private int m_nextRespawnTurn = -1;
	private List<BoardSquare> m_trueRespawnSquares = new List<BoardSquare>();
	private BoardSquare m_trueRespawnPositionSquare;
	private GameObject m_respawnPositionFlare;
	private BoardSquare m_respawnFlareVfxSquare;
	private bool m_respawnFlareForSameTeam;

	[HideInInspector]
	private bool m_hasBotController;

	private bool m_currentlyVisibleForAbilityCast;
	private bool m_movedForEvade;
	private bool m_serverSuppressInvisibility;
	private List<ActorData> m_lineOfSightVisibleExceptions = new List<ActorData>();
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

	internal static int Layer { get; private set; }

	internal static int Layer_Mask { get; private set; }


	public bool ForceDisplayTargetHighlight { get; set; }


	internal Vector3 PreviousBoardSquarePosition { get; private set; }


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
				m_actorIndex = value;
			}
		}
	}

	public bool ShowInGameGUI
	{
		get
		{
			return m_showInGameHud;
		}
		set
		{
			m_showInGameHud = value;
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
			if (NetworkServer.active)
			{
				m_hitPoints = Mathf.Clamp(value, 0, GetMaxHitPoints());
			}
			else
			{
				m_hitPoints = value;
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
				gameObject.SendMessage("OnDeath");
				if (GameFlowData.Get() != null)
				{
					GameFlowData.Get().NotifyOnActorDeath(this);
				}
				UnoccupyCurrentBoardSquare();
				SetCurrentBoardSquare(null);
				ClientLastKnownPosSquare = null;
				ServerLastKnownPosSquare = null;
			}
			else if (!wasAlive && m_hitPoints > 0 && LastDeathTurn > 0)
			{
				gameObject.SendMessage("OnRespawn");
				m_lastVisibleTurnToClient = 0;
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
				m_serverExpectedHoTTotal = value;
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
				m_serverExpectedHoTThisTurn = value;
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

	internal int TechPoints
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
				m_techPoints = Mathf.Clamp(value, 0, GetMaxTechPoints());
			}
			else
			{
				m_techPoints = value;
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
				m_reservedTechPoints = value;
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
				m_ignoreFromAbilityHits = value;
			}
		}
	}

	internal int AbsorbPoints
	{
		get
		{
			return m_absorbPoints;
		}
		private set
		{
			if (m_absorbPoints == value)
			{
				return;
			}
			if (NetworkServer.active)
			{
				m_absorbPoints = Mathf.Max(value, 0);
			}
			else
			{
				m_absorbPoints = Mathf.Max(value, 0);
			}
			ClientUnresolvedAbsorb = 0;
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
				m_mechanicPoints = Mathf.Max(value, 0);
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
				m_spawnerId = value;
			}
		}
	}

	public bool DisappearingAfterCurrentMovement => m_disappearingAfterCurrentMovement;

	public BoardSquare CurrentBoardSquare => m_clientCurrentBoardSquare;

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

	public float RemainingHorizontalMovement { get; set; }

	public float RemainingMovementWithQueuedAbility { get; set; }

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

	internal int LastDeathTurn { get; private set; }

	public int NextRespawnTurn
	{
		get
		{
			return m_nextRespawnTurn;
		}
		set
		{
			m_nextRespawnTurn = Mathf.Max(value, LastDeathTurn + 1);
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
			if (m_teamSensitiveData_hostile != null)
			{
				return m_teamSensitiveData_hostile.RespawnPickedSquare;
			}
			return null;
		}
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
			m_hasBotController = value;
		}
	}

	public bool VisibleTillEndOfPhase { get; set; }

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

	public event Action OnTurnStartDelegates;
	public event Action<UnityEngine.Object, GameObject> OnAnimationEventDelegates;
	public event Action<Ability> OnSelectedAbilityChangedDelegates;
	public event Action OnClientQueuedActionChangedDelegates;

	public ActorData()
	{
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
			m_lastVisibleTurnToClient = GameFlowData.Get().CurrentTurn;
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

	internal ActorController GetActorController()
	{
		return GetComponent<ActorController>();
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
		m_displayName = newDisplayName;
	}

	public Sprite GetAliveHUDIcon()
	{
		return (Sprite)Resources.Load(m_aliveHUDIconResourceString, typeof(Sprite));
	}

	public Sprite GetDeadHUDIcon()
	{
		return (Sprite)Resources.Load(m_deadHUDIconResourceString, typeof(Sprite));
	}

	public Sprite GetScreenIndicatorIcon()
	{
		return (Sprite)Resources.Load(m_screenIndicatorIconResourceString, typeof(Sprite));
	}

	public Sprite GetScreenIndicatorBWIcon()
	{
		return (Sprite)Resources.Load(m_screenIndicatorBWIconResourceString, typeof(Sprite));
	}

	public string GetClassName()
	{
		return name.Replace("(Clone)", "");
	}

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
		}
		else if (previousMax > actualMaxTechPoints)
		{
			int techPoints = TechPoints;
			TechPoints = Mathf.Min(TechPoints, actualMaxTechPoints);
			if (techPoints - TechPoints != 0)
			{
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
			m_lastVisibleTurnToClient = GameFlowData.Get().CurrentTurn;
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
					m_lastVisibleTurnToClient = GameFlowData.Get().CurrentTurn;
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

	public void UpdateServerLastVisibleTurn()
	{
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
				}
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
		value = Mathf.Clamp(value, 0, GetMaxTechPoints());
		int diff = value - TechPoints;
		TechPoints = value;

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
		if (m_teamSensitiveData_hostile != null)
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

	public bool IsRespawnLocationVisibleToEnemy(BoardSquare respawnLocation)
	{
		return false;
	}

	internal void AddLineOfSightVisibleException(ActorData visibleActor)
	{
		m_lineOfSightVisibleExceptions.Add(visibleActor);
		GetFogOfWar().MarkForRecalculateVisibility();
	}

	internal void RemoveLineOfSightVisibleException(ActorData visibleActor)
	{
		m_lineOfSightVisibleExceptions.Remove(visibleActor);
		GetFogOfWar().MarkForRecalculateVisibility();
	}

	internal bool IsLineOfSightVisibleException(ActorData visibleActor)
	{
		return m_lineOfSightVisibleExceptions.Contains(visibleActor);
	}

	public void OnClientQueuedActionChanged()
	{
		OnClientQueuedActionChangedDelegates?.Invoke();
	}

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
		m_actorTargeting = GetComponent<ActorTargeting>();
		m_passiveData = GetComponent<PassiveData>();
		m_combatText = GetComponent<CombatText>();
		m_actorTags = GetComponent<ActorTag>();
		m_freelancerStats = GetComponent<FreelancerStats>();
		if (NetworkServer.active)
		{
			ActorIndex = checked(++s_nextActorIndex);
		}
		Layer = LayerMask.NameToLayer("Actor");
		Layer_Mask = 1 << Layer;
		if (GameFlowData.Get())
		{
			m_lastSpawnTurn = Mathf.Max(1, GameFlowData.Get().CurrentTurn);
		}
		else
		{
			m_lastSpawnTurn = 1;
		}
		LastDeathTurn = -2;
		NextRespawnTurn = -1;
		HasBotController = false;
		SpawnerId = -1;
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
			Log.Warning(Log.Category.ActorData, string.Format("ActorData already initialized to a different prefab.  Currently [{0}], setting to [{1}]", m_actorSkinPrefabLink.ToString()));
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
			if (addMasterSkinVfx && NetworkClient.active && MasterSkinVfxData.Get() != null)
			{
				GameObject masterSkinVfxInst = MasterSkinVfxData.Get().AddMasterSkinVfxOnCharacterObject(m_actorModelData.gameObject, m_characterType, 1f);
				m_actorModelData.SetMasterSkinVfxInst(masterSkinVfxInst);
			}
		}
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

	private void Start()
	{
		if (NetworkClient.active)
		{
			m_nameplateJoint = new JointPopupProperty();
			m_nameplateJoint.m_joint = "VFX_name";
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
		GameFlowData.Get().AddActor(this);
		EnableRagdoll(false);
		if (!m_addedToUI && HUD_UI.Get() != null && HUD_UI.Get().m_mainScreenPanel != null)
		{
			m_addedToUI = true;
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.AddActor(this);
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddActor(this);
		}
		if (GameFlow.Get().playerDetails.TryGetValue(PlayerData.GetPlayer(), out var playerDetails) && playerDetails.IsLocal())
		{
			Log.Info("ActorData.Start {0} {1}", this, playerDetails);
			GameFlowData.Get().AddOwnedActorData(this);
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
							&& actor.PlayerIndex != PlayerData.s_invalidPlayerIndex
							&& NetworkClient.active
							&& !NetworkServer.active
							&& GameFlowData.Get().LocalPlayerData.IsViewingTeam(actor.GetTeam()))
						{
							Debug.LogError("On client, living friendly-to-client actor " + actor.DebugNameString() + " has null square on Turn Tick");
							flag = true;
						}
					}
					if (!NetworkServer.active)
					{
						return;
					}
					if (flag)
					{
						return;
					}
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
				actorData.DoVisualDeath(Sequence.CreateImpulseInfoWithActorForward(actorData));
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

	public void PlayDamageReactionAnim(string customDamageReactTriggerName)
	{
		Animator modelAnimator = GetModelAnimator();
		if (modelAnimator != null
			&& m_actorMovement.GetAestheticPath() == null
			&& !m_actorMovement.AmMoving()
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
		Animator modelAnimator = GetModelAnimator();
		return modelAnimator == null || !modelAnimator.enabled;
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
		if (ragDollOn && GetHitPointsToDisplay() > 0 && !isDebugRagdoll)
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
		if (!NetworkServer.active && NPCCoordinator.IsSpawnedNPC(this))
		{
			NPCCoordinator.Get().OnActorSpawn(this);
		}
		GameEventManager.Get().FireEvent(GameEventManager.EventType.CharacterRespawn, new GameEventManager.CharacterRespawnEventArgs
		{
			respawningCharacter = this
		});
		if (GameFlowData.Get().activeOwnedActorData == this)
		{
			CameraManager.Get().SetTargetObject(gameObject, CameraManager.CameraTargetReason.ClientActorRespawned);
		}
		m_lastSpawnTurn = GameFlowData.Get().CurrentTurn;
	}

	public bool IsActorInvisibleForRespawn()
	{
		return !IsDead() &&
			NextRespawnTurn > 0 &&
			NextRespawnTurn == GameFlowData.Get().CurrentTurn &&
			SpawnPointManager.Get() != null &&
			SpawnPointManager.Get().m_spawnInDuringMovement;
	}

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
			SetDirtyBit(1u);
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
		m_alwaysHideNameplate = alwaysHide;
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
		if (CurrentlyVisibleForAbilityCast)
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
		if (VisibleTillEndOfPhase && !MovedForEvade)
		{
			return true;
		}
		if (CurrentlyVisibleForAbilityCast)
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

	public bool IsActorVisibleToActor(ActorData observer, bool forceViewingTeam = false)
	{
		if (this == observer)
		{
			return true;
		}
		if (!NetworkServer.active && observer == GameFlowData.Get().activeOwnedActorData)
		{
			return IsActorVisibleToClient();
		}
		if (!NetworkServer.active)
		{
			Log.Warning("Calling the server-only method ActorData::IsActorVisibleToActor on a client. Clients can only depend on ActorData::IsActorVisibleToClient.");
		}
		if (IsAlwaysVisibleTo(observer.PlayerData))
		{
			return true;
		}
		if (IsNeverVisibleTo(observer.PlayerData, true, forceViewingTeam))
		{
			return false;
		}
		return observer.GetFogOfWar().IsVisible(GetTravelBoardSquare());
	}

	public bool IsActorVisibleToAnyEnemy()
	{
		foreach (ActorData enemy in GameFlowData.Get().GetAllTeamMembers(GetEnemyTeam()))
		{
			if (!enemy.IsDead() && IsActorVisibleToActor(enemy, true))
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

	public void ApplyForce(Vector3 dir, float amount)
	{
		Rigidbody hipJointRigidBody = GetHipJoint();
		if (hipJointRigidBody)
		{
			hipJointRigidBody.AddForce(dir * amount, ForceMode.Impulse);
		}
	}

	public Vector3 GetOverheadPosition(float offsetInPixels)
	{
		if (Camera.main == null || m_nameplateJoint == null)
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
			Log.Warning("Trying to get LoS check pos wrt a null square (IsDead: " + IsDead() + ")  for " + DisplayName + " Code issue-- actor probably instantiated but not on Board yet.");
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
			Log.Warning("Trying to get free pos wrt a null square (IsDead: " + IsDead() + ").  for " + DisplayName + " Code issue-- actor probably instantiated but not on Board yet.");
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
			return value.IsHumanControlled;
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


			string jsonLog = $"[JSON] {{\"actorData\": {{\"playerIndex\": {_playerIndex}" +
				$",\"actorIndex\": {_actorIndex}" +
				$",\"displayName\": \"{_displayName}\"" +
				$",\"team\": {_team}" + 
				$",\"QueuedMovementAllowsAbility\": {DefaultJsonSerializer.Serialize(_QueuedMovementAllowsAbility)}" +
				$",\"HasQueuedMovement\": {DefaultJsonSerializer.Serialize(_HasQueuedMovement)}" +
				$",\"HasQueuedChase\": {DefaultJsonSerializer.Serialize(_HasQueuedChase)}" +
				$",\"RemainingHorizontalMovement\": {_RemainingHorizontalMovement}" +
				$",\"RemainingMovementWithQueuedAbility\": {_RemainingMovementWithQueuedAbility}" +
				$",\"queuedChaseTargetActorIndex\": {_queuedChaseTargetActorIndex}" +
				$",\"HitPoints\": {_HitPoints}" +
				$",\"TechPoints\": {_TechPoints}";


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
				jsonLog += $",\"UnresolvedDamage\": {_UnresolvedDamage}";
			}
			if (_hasUnresolvedHealing)
			{
				stream.Serialize(ref _UnresolvedHealing);
				jsonLog += $",\"UnresolvedHealing\": {_UnresolvedHealing}";
			}
			if (_hasUnresolvedTechPointGain)
			{
				stream.Serialize(ref _UnresolvedTechPointGain);
				jsonLog += $",\"UnresolvedTechPointGain\": {_UnresolvedTechPointGain}";
			}
			if (_hasUnresolvedTechPointLoss)
			{
				stream.Serialize(ref _UnresolvedTechPointLoss);
				jsonLog += $",\"UnresolvedTechPointLoss\": {_UnresolvedTechPointLoss}";
			}
			if (_hasReservedTechPoints)
			{
				stream.Serialize(ref _ReservedTechPoints);
				jsonLog += $",\"ReservedTechPoints\": {_ReservedTechPoints}";
			}
			if (_hasAbsorbPoints)
			{
				stream.Serialize(ref _AbsorbPoints);
				jsonLog += $",\"AbsorbPoints\": {_AbsorbPoints}";
			}
			if (_hasMechanicPoints)
			{
				stream.Serialize(ref _MechanicPoints);
				jsonLog += $",\"MechanicPoints\": {_MechanicPoints}";
			}
			if (out16)
			{
				stream.Serialize(ref _ExpectedHoTTotal);
				stream.Serialize(ref _ExpectedHoTThisTurn);
				jsonLog += $",\"ExpectedHoTTotal\": {_ExpectedHoTTotal}";
				jsonLog += $",\"ExpectedHoTThisTurn\": {_ExpectedHoTThisTurn}";
			}
			stream.Serialize(ref _LastDeathTurn);
			stream.Serialize(ref _lastSpawnTurn);
			stream.Serialize(ref _NextRespawnTurn);
			stream.Serialize(ref _SpawnerId);
			stream.Serialize(ref _UiGameplayBitfield);
			jsonLog += $",\"LastDeathTurn\": {_LastDeathTurn}";
			jsonLog += $",\"lastSpawnTurn\": {_lastSpawnTurn}";
			jsonLog += $",\"NextRespawnTurn\": {_NextRespawnTurn}";
			jsonLog += $",\"SpawnerId\": {_SpawnerId}";

			ServerClientUtils.GetBoolsFromBitfield(_UiGameplayBitfield, out _HasBotController, out _showInGameHud, out _VisibleTillEndOfPhase, out _ignoreFromAbilityHits, out _alwaysHideNameplate);
			jsonLog += $",\"HasBotController\": {DefaultJsonSerializer.Serialize(_HasBotController)}";
			jsonLog += $",\"showInGameHud\": {DefaultJsonSerializer.Serialize(_showInGameHud)}";
			jsonLog += $",\"VisibleTillEndOfPhase\": {DefaultJsonSerializer.Serialize(_VisibleTillEndOfPhase)}";
			jsonLog += $",\"ignoreFromAbilityHits\": {DefaultJsonSerializer.Serialize(_ignoreFromAbilityHits)}";
			jsonLog += $",\"alwaysHideNameplate\": {DefaultJsonSerializer.Serialize(_alwaysHideNameplate)}";

			SerializeCharacterVisualInfo(stream, ref m_visualInfo);
			jsonLog += $",\"visualInfo\":{{\"skinIndex\": {m_visualInfo.skinIndex}";
			jsonLog += $",\"patternIndex\": {m_visualInfo.patternIndex}";
			jsonLog += $",\"colorIndex\": {m_visualInfo.colorIndex}}}";

			SerializeCharacterCardInfo(stream, ref m_selectedCards);
			jsonLog += $",\"selectedCards\":{{\"PrepCard\": \"{m_selectedCards.PrepCard}\"";
			jsonLog += $",\"DashCard\": \"{m_selectedCards.DashCard}\"";
			jsonLog += $",\"CombatCard\": \"{m_selectedCards.CombatCard}\"}}";

			SerializeCharacterModInfo(stream, ref m_selectedMods);
			jsonLog += $",\"selectedMods\":{{\"ModForAbility0\": {m_selectedMods.ModForAbility0}";
			jsonLog += $",\"ModForAbility1\": {m_selectedMods.ModForAbility1}";
			jsonLog += $",\"ModForAbility2\": {m_selectedMods.ModForAbility2}";
			jsonLog += $",\"ModForAbility3\": {m_selectedMods.ModForAbility3}";
			jsonLog += $",\"ModForAbility4\": {m_selectedMods.ModForAbility4}";
			jsonLog += $"}}";

			SerializeCharacterAbilityVfxSwapInfo(stream, ref m_abilityVfxSwapInfo);
			jsonLog += $",\"abilityVfxSwapInfo\":{{\"VfxSwapForAbility0\": {m_abilityVfxSwapInfo.VfxSwapForAbility0}";
			jsonLog += $",\"VfxSwapForAbility1\": {m_abilityVfxSwapInfo.VfxSwapForAbility1}";
			jsonLog += $",\"VfxSwapForAbility2\": {m_abilityVfxSwapInfo.VfxSwapForAbility2}";
			jsonLog += $",\"VfxSwapForAbility3\": {m_abilityVfxSwapInfo.VfxSwapForAbility3}";
			jsonLog += $",\"VfxSwapForAbility4\": {m_abilityVfxSwapInfo.VfxSwapForAbility4}";
			jsonLog += $"}}";
			
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
			jsonLog += $",\"lineOfSightVisibleExceptionsCount\": {_lineOfSightVisibleExceptionsCount}";
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
			jsonLog += $",\"lineOfSightVisibleExceptions\": {DefaultJsonSerializer.Serialize(m_lineOfSightVisibleExceptions)}";

			stream.Serialize(ref _lastVisibleTurnToClient);
			jsonLog += $",\"_lastVisibleTurnToClient\": {_lastVisibleTurnToClient}";
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
			jsonLog += $",\"ServerLastKnownPosSquare\": {DefaultJsonSerializer.Serialize(ServerLastKnownPosSquare)}";
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
			jsonLog += "}}";
			Log.Info(jsonLog);
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
				if (m_teamSensitiveData_hostile != null)
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

	public void MoveToBoardSquareLocal(BoardSquare dest, MovementType movementType, BoardSquarePathInfo path, bool moverWillDisappear)
	{
		m_disappearingAfterCurrentMovement = moverWillDisappear;
		if (dest == null)
		{
			if (moverWillDisappear && path == null)
			{
				UnoccupyCurrentBoardSquare();
				SetCurrentBoardSquare(null);
				ForceUpdateIsVisibleToClientCache();
				ForceUpdateActorModelVisibility();
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
		if (ServerClientUtils.GetCurrentAbilityPhase() == AbilityPriority.Evasion)
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
		}
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
		if (movementType == MovementType.Teleport)
		{
			ForceUpdateIsVisibleToClientCache();
			ForceUpdateActorModelVisibility();
			SetTransformPositionToSquare(dest);
			m_actorMovement.ClearPath();
			if (m_actorCover)
			{
				m_actorCover.RecalculateCover();
			}
			UpdateFacingAfterMovement();
			if (currentBoardSquare != null)
			{
				BoardSquarePathInfo boardSquarePathInfo = MovementUtils.Build2PointTeleportPath(currentBoardSquare, dest);
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
		m_actorMovement.UpdateSquaresCanMoveTo();
		GetFogOfWar().MarkForRecalculateVisibility();
	}

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
			m_clientCurrentBoardSquare = square;
			Animator modelAnimator = GetModelAnimator();
			if (modelAnimator != null)
			{
				modelAnimator.SetBool("Cover", ActorCover.CalcCoverLevelGeoOnly(out bool[] _, CurrentBoardSquare));
			}
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
			if (m_teamSensitiveData_hostile != null)
			{
				m_teamSensitiveData_hostile.ClearPreviousMovementInfo();
			}
		}
	}

	public void SetClientFriendlyTeamSensitiveData(ActorTeamSensitiveData friendlyTSD)
	{
		if (m_teamSensitiveData_friendly != friendlyTSD)
		{
			Log.Info("Setting Friendly TeamSensitiveData for " + DebugNameString());
			m_teamSensitiveData_friendly = friendlyTSD;
			m_teamSensitiveData_friendly.OnClientAssociatedWithActor(this);
		}
	}

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

	public void SetTeam(Team team)
	{
		m_team = team;
		GameFlowData.Get().AddToTeam(this);
		TeamStatusDisplay.GetTeamStatusDisplay().RebuildTeamDisplay();
		if (!NetworkServer.active)
		{
			return;
		}
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
		CurrentlyVisibleForAbilityCast = false;
		MovedForEvade = false;
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

	public bool HasQueuedMovement()
	{
		return m_queuedMovementRequest || m_queuedChaseRequest;
	}

	public bool HasQueuedChase()
	{
		return m_queuedChaseRequest;
	}

	public ActorData GetQueuedChaseTarget()
	{
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
	}

	private void HandleOnSelectedAsActiveActor()
	{
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.ProcessTeams();
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.MarkFramesForForceUpdate();
			HUD_UI.Get().m_mainScreenPanel.m_characterProfile.UpdateStatusDisplay(true);
		}
		m_actorTurnSM.OnSelect();
		GetFogOfWar().MarkForRecalculateVisibility();
		CameraManager.Get().OnActiveOwnedActorChange(this);
		if (GetActorMovement() != null)
		{
			GetActorMovement().UpdateSquaresCanMoveTo();
		}
	}

	public void OnMovementChanged(MovementChangeType changeType, bool forceChased = false)
	{
	}

	public bool BeingTargetedByClientAbility(out bool inCover, out bool updatingInConfirm)
	{
		bool isInRange = false;
		inCover = false;
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
						}
					}
					else if (m_wasUpdatingForConfirmedTargeting)
					{
						HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.StartTargetingNumberFadeout(this);
						m_showingTargetingNumAtFullAlpha = false;
					}
				}
				if (actorTurnSM.CurrentState == TurnStateEnum.DECIDING
					&& !activeOwnedActorData.ForceDisplayTargetHighlight
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

	public void AddForceShowOutlineChecker(IForceActorOutlineChecker checker)
	{
		if (checker != null && !m_forceShowOutlineCheckers.Contains(checker))
		{
			m_forceShowOutlineCheckers.Add(checker);
		}
	}

	public void RemoveForceShowOutlineChecker(IForceActorOutlineChecker checker)
	{
		if (m_forceShowOutlineCheckers != null)
		{
			m_forceShowOutlineCheckers.Remove(checker);
		}
	}

	public bool ShouldForceTargetOutlineForActor(ActorData actor)
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
		if (text != eventName)
		{
		}
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
	internal void CmdDebugSetAbilityMod(int abilityIndex, int modId)
	{
	}

	[Command]
	private void CmdDebugReplaceWithBot()
	{
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
		Log.Info($"[JSON] {{\"RpcOnHitPointsResolved\":{{\"resolvedHitPoints\":{DefaultJsonSerializer.Serialize(resolvedHitPoints)}}}}}");
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

	[ClientRpc]
	internal void RpcCombatText(string combatText, string logText, CombatTextCategory category, BuffIconToDisplay icon)
	{
		Log.Info($"[JSON] {{\"RpcCombatText\":{{\"combatText\":{DefaultJsonSerializer.Serialize(combatText)}," +
			$"\"logText\":{DefaultJsonSerializer.Serialize(logText)}," +
			$"\"category\":{DefaultJsonSerializer.Serialize(category)}," +
			$"\"icon\":{DefaultJsonSerializer.Serialize(icon)}}}}}");
		AddCombatText(combatText, logText, category, icon);
	}

	internal void AddCombatText(string combatText, string logText, CombatTextCategory category, BuffIconToDisplay icon)
	{
		if (m_combatText == null)
		{
			Log.Error(gameObject.name + " does not have a combat text component.");
			return;
		}
		m_combatText.Add(combatText, logText, category, icon);
	}

	[Client]
	internal void ShowDamage(string combatText)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void ActorData::ShowDamage(System.String)' called on server");
		}
	}

	[ClientRpc]
	internal void RpcApplyAbilityModById(int actionTypeInt, int abilityScopeId)
	{
		Log.Info($"[JSON] {{\"RpcApplyAbilityModById\":{{\"actionTypeInt\":{DefaultJsonSerializer.Serialize(actionTypeInt)},\"abilityScopeId\":{DefaultJsonSerializer.Serialize(abilityScopeId)}}}}}");
		if (!NetworkServer.active && NetworkClient.active && abilityScopeId >= 0)
		{
			ApplyAbilityModById(actionTypeInt, abilityScopeId);
		}
	}

	internal void ApplyAbilityModById(int actionTypeInt, int abilityScopeId)
	{
		if (GameManager.Get().GameConfig.GameType != GameType.Tutorial && !AbilityModHelper.IsModAllowed(m_characterType, actionTypeInt, abilityScopeId))
		{
			Debug.LogWarning("Mod with ID " + abilityScopeId + " is not allowed on ability at index " + actionTypeInt + " for character " + m_characterType.ToString());
			return;
		}

		AbilityData component = GetComponent<AbilityData>();
		if (component != null)
		{
			Ability abilityOfActionType = component.GetAbilityOfActionType((AbilityData.ActionType)actionTypeInt);
			AbilityMod abilityModForAbilityById = AbilityModManager.Get().GetAbilityModForAbilityById(abilityOfActionType, abilityScopeId);
			if (abilityModForAbilityById != null)
			{
				GameType gameType = GameManager.Get().GameConfig.GameType;
				GameSubType instanceSubType = GameManager.Get().GameConfig.InstanceSubType;
				if (abilityModForAbilityById.EquippableForGameType())
				{
					ApplyAbilityModToAbility(abilityOfActionType, abilityModForAbilityById);
					if (NetworkServer.active)
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

	internal void DebugApplyModToAbility(int actionTypeInt, int abilityScopeId)
	{
	}

	private void ApplyAbilityModToAbility(Ability ability, AbilityMod abilityMod, bool log = false)
	{
		if (ability.GetType() != abilityMod.GetTargetAbilityType())
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
		Log.Info($"[JSON] {{\"RpcMarkForRecalculateClientVisibility\":{{}}}}");
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
		Log.Info($"[JSON] {{\"RpcForceLeaveGame\":{{\"gameResult\":{DefaultJsonSerializer.Serialize(gameResult)}}}}}");
		if (GameFlowData.Get().activeOwnedActorData == this && !ClientGameManager.Get().IsFastForward)
		{
			ClientGameManager.Get().LeaveGame(false, gameResult);
		}
	}

	public void SendPingRequestToServer(int teamIndex, Vector3 worldPosition, ActorController.PingType pingType)
	{
		if (GetActorController() != null)
		{
			GetActorController().CallCmdSendMinimapPing(teamIndex, worldPosition, pingType);
		}
	}

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
		bool pause = reader.ReadBoolean();
		Log.Info($"[JSON] {{\"CmdSetPausedForDebugging\":{{\"pause\":{DefaultJsonSerializer.Serialize(pause)}}}}}");
		((ActorData)obj).CmdSetPausedForDebugging(pause);
	}

	protected static void InvokeCmdCmdSetResolutionSingleStepping(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetResolutionSingleStepping called on client.");
			return;
		}
		bool singleStepping = reader.ReadBoolean();
		Log.Info($"[JSON] {{\"CmdSetResolutionSingleStepping\":{{\"singleStepping\":{DefaultJsonSerializer.Serialize(singleStepping)}}}}}");
		((ActorData)obj).CmdSetResolutionSingleStepping(singleStepping);
	}

	protected static void InvokeCmdCmdSetResolutionSingleSteppingAdvance(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetResolutionSingleSteppingAdvance called on client.");
			return;
		}
		Log.Info($"[JSON] {{\"CmdSetResolutionSingleSteppingAdvance\":{{}}}}");
		((ActorData)obj).CmdSetResolutionSingleSteppingAdvance();
	}

	protected static void InvokeCmdCmdSetDebugToggleParam(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetDebugToggleParam called on client.");
			return;
		}
		string name = reader.ReadString();
		bool value = reader.ReadBoolean();
		Log.Info($"[JSON] {{\"CmdSetDebugToggleParam\":{{\"name\":{DefaultJsonSerializer.Serialize(name)},\"value\":{DefaultJsonSerializer.Serialize(value)}}}}}");
		((ActorData)obj).CmdSetDebugToggleParam(name, value);
	}

	protected static void InvokeCmdCmdDebugReslotCards(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDebugReslotCards called on client.");
			return;
		}
		bool reslotAll = reader.ReadBoolean();
		int cardTypeInt = (int)reader.ReadPackedUInt32();
		Log.Info($"[JSON] {{\"CmdDebugReslotCards\":{{\"reslotAll\":{DefaultJsonSerializer.Serialize(reslotAll)},\"cardTypeInt\":{DefaultJsonSerializer.Serialize(cardTypeInt)}}}}}");
		((ActorData)obj).CmdDebugReslotCards(reslotAll, cardTypeInt);
	}

	protected static void InvokeCmdCmdDebugSetAbilityMod(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDebugSetAbilityMod called on client.");
			return;
		}
		int abilityIndex = (int)reader.ReadPackedUInt32();
		int modId = (int)reader.ReadPackedUInt32();
		Log.Info($"[JSON] {{\"CmdDebugSetAbilityMod\":{{\"abilityIndex\":{DefaultJsonSerializer.Serialize(abilityIndex)},\"modId\":{DefaultJsonSerializer.Serialize(modId)}}}}}");
		((ActorData)obj).CmdDebugSetAbilityMod(abilityIndex, modId);
	}

	protected static void InvokeCmdCmdDebugReplaceWithBot(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDebugReplaceWithBot called on client.");
			return;
		}
		Log.Info($"[JSON] {{\"CmdDebugReplaceWithBot\":{{}}}}");
		((ActorData)obj).CmdDebugReplaceWithBot();
	}

	protected static void InvokeCmdCmdDebugSetHealthOrEnergy(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDebugSetHealthOrEnergy called on client.");
			return;
		}
		int actorIndex = (int)reader.ReadPackedUInt32();
		int valueToSet = (int)reader.ReadPackedUInt32();
		int flag = (int)reader.ReadPackedUInt32();
		Log.Info($"[JSON] {{\"CmdDebugSetHealthOrEnergy\":{{" +
			$"\"actorIndex\":{DefaultJsonSerializer.Serialize(actorIndex)}," +
			$"\"valueToSet\":{DefaultJsonSerializer.Serialize(valueToSet)}," +
			$"\"flag\":{DefaultJsonSerializer.Serialize(flag)}}}}}");
		((ActorData)obj).CmdDebugSetHealthOrEnergy(actorIndex, valueToSet, flag);
	}

	public void CallCmdSetPausedForDebugging(bool pause)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdSetPausedForDebugging called on server.");
			return;
		}
		if (isServer)
		{
			CmdSetPausedForDebugging(pause);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdSetPausedForDebugging);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(pause);
		SendCommandInternal(networkWriter, 0, "CmdSetPausedForDebugging");
	}

	public void CallCmdSetResolutionSingleStepping(bool singleStepping)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdSetResolutionSingleStepping called on server.");
			return;
		}
		if (isServer)
		{
			CmdSetResolutionSingleStepping(singleStepping);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdSetResolutionSingleStepping);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(singleStepping);
		SendCommandInternal(networkWriter, 0, "CmdSetResolutionSingleStepping");
	}

	public void CallCmdSetResolutionSingleSteppingAdvance()
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdSetResolutionSingleSteppingAdvance called on server.");
			return;
		}
		if (isServer)
		{
			CmdSetResolutionSingleSteppingAdvance();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdSetResolutionSingleSteppingAdvance);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, "CmdSetResolutionSingleSteppingAdvance");
	}

	public void CallCmdSetDebugToggleParam(string name, bool value)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdSetDebugToggleParam called on server.");
			return;
		}
		if (isServer)
		{
			CmdSetDebugToggleParam(name, value);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdSetDebugToggleParam);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(name);
		networkWriter.Write(value);
		SendCommandInternal(networkWriter, 0, "CmdSetDebugToggleParam");
	}

	public void CallCmdDebugReslotCards(bool reslotAll, int cardTypeInt)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdDebugReslotCards called on server.");
			return;
		}
		if (isServer)
		{
			CmdDebugReslotCards(reslotAll, cardTypeInt);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdDebugReslotCards);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(reslotAll);
		networkWriter.WritePackedUInt32((uint)cardTypeInt);
		SendCommandInternal(networkWriter, 0, "CmdDebugReslotCards");
	}

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
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdDebugReplaceWithBot);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, "CmdDebugReplaceWithBot");
	}

	public void CallCmdDebugSetHealthOrEnergy(int actorIndex, int valueToSet, int flag)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdDebugSetHealthOrEnergy called on server.");
			return;
		}
		if (isServer)
		{
			CmdDebugSetHealthOrEnergy(actorIndex, valueToSet, flag);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdDebugSetHealthOrEnergy);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actorIndex);
		networkWriter.WritePackedUInt32((uint)valueToSet);
		networkWriter.WritePackedUInt32((uint)flag);
		SendCommandInternal(networkWriter, 0, "CmdDebugSetHealthOrEnergy");
	}

	protected static void InvokeRpcRpcOnHitPointsResolved(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcOnHitPointsResolved called on server.");
			return;
		}
		((ActorData)obj).RpcOnHitPointsResolved((int)reader.ReadPackedUInt32());
	}

	protected static void InvokeRpcRpcCombatText(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcCombatText called on server.");
			return;
		}
		((ActorData)obj).RpcCombatText(reader.ReadString(), reader.ReadString(), (CombatTextCategory)reader.ReadInt32(), (BuffIconToDisplay)reader.ReadInt32());
	}

	protected static void InvokeRpcRpcApplyAbilityModById(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcApplyAbilityModById called on server.");
			return;
		}
		((ActorData)obj).RpcApplyAbilityModById((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

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
		((ActorData)obj).RpcForceLeaveGame((GameResult)reader.ReadInt32());
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
}
