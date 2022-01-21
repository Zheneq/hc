using Fabric;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
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
		_001D,
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

	public static int s_invalidActorIndex;

	public static short s_nextActorIndex;

	public static Color s_friendlyPlayerColor;

	public static Color s_friendlyDamagedHealthBarColor;

	public static Color s_hostilePlayerColor;

	public static Color s_hostileDamagedHealthBarColor;

	public static Color s_neutralPlayerColor;

	public static Color s_teamAColor;

	public static Color s_teamBColor;

	internal static float s_visibleTimeAfterHit;

	internal static float s_visibleTimeAfterMovementHit;

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
	public string m_onDeathAudioEvent = string.Empty;

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

	//[CompilerGenerated]
	//private static Action<GameState> OnGameStateChanged;

	//[CompilerGenerated]
	//private static Action<GameState> OnGameStateChanged;

	[CompilerGenerated]
	private static Action<GameState> _003C_003Ef__mg_0024cache2;

	private static int kCmdCmdSetPausedForDebugging;

	private static int kCmdCmdSetResolutionSingleStepping;

	private static int kCmdCmdSetResolutionSingleSteppingAdvance;

	private static int kCmdCmdSetDebugToggleParam;

	private static int kCmdCmdDebugReslotCards;

	private static int kCmdCmdDebugSetAbilityMod;

	private static int kCmdCmdDebugReplaceWithBot;

	private static int kCmdCmdDebugSetHealthOrEnergy;

	private static int kRpcRpcOnHitPointsResolved;

	private static int kRpcRpcCombatText;

	private static int kRpcRpcApplyAbilityModById;

	private static int kRpcRpcMarkForRecalculateClientVisibility;

	private static int kRpcRpcForceLeaveGame;

	internal static int Layer
	{
		get;
		private set;
	}

	internal static int Layer_Mask
	{
		get;
		private set;
	}

	public bool ForceDisplayTargetHighlight
	{
		get;
		set;
	}

	internal Vector3 PreviousBoardSquarePosition
	{
		get;
		private set;
	}

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
				if (ActorDebugUtils.Get() != null)
				{
					if (ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.LastKnownPosition, false))
					{
						string[] obj = new string[5]
						{
							GetDebugName(),
							"----Setting ClientLastKnownPosSquare from ",
							(!m_clientLastKnownPosSquare) ? "null" : m_clientLastKnownPosSquare.ToString(),
							" to ",
							null
						};
						object obj2;
						if ((bool)value)
						{
							obj2 = value.ToString();
						}
						else
						{
							obj2 = "null";
						}
						obj[4] = (string)obj2;
						Debug.LogWarning(string.Concat(obj));
					}
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
			if (!(m_serverLastKnownPosSquare != value))
			{
				return;
			}
			while (true)
			{
				if (ActorDebugUtils.Get() != null && ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.LastKnownPosition, false))
				{
					string[] obj = new string[5]
					{
						GetDebugName(),
						"=====ServerLastKnownPosSquare from ",
						null,
						null,
						null
					};
					object obj2;
					if ((bool)m_serverLastKnownPosSquare)
					{
						obj2 = m_serverLastKnownPosSquare.ToString();
					}
					else
					{
						obj2 = "null";
					}
					obj[2] = (string)obj2;
					obj[3] = " to ";
					obj[4] = ((!value) ? "null" : value.ToString());
					Debug.LogWarning(string.Concat(obj));
				}
				m_serverLastKnownPosSquare = value;
				return;
			}
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
				MatchLogger.Get().Log(string.Concat(this, " HitPoints.set ", value, ", old: ", HitPoints));
			}
			bool flag = m_hitPoints > 0;
			if (NetworkServer.active)
			{
				m_hitPoints = Mathf.Clamp(value, 0, GetMaxHitPoints());
			}
			else
			{
				m_hitPoints = value;
			}
			int num = 0;
			if (GameFlowData.Get() != null)
			{
				num = GameFlowData.Get().CurrentTurn;
			}
			if (flag)
			{
				if (m_hitPoints == 0)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							if (GameFlowData.Get() != null)
							{
								LastDeathTurn = GameFlowData.Get().CurrentTurn;
							}
							LastDeathPosition = base.gameObject.transform.position;
							NextRespawnTurn = -1;
							FogOfWar.CalculateFogOfWarForTeam(GetTeam());
							if (GetCurrentBoardSquare() != null)
							{
								SetMostRecentDeathSquare(GetCurrentBoardSquare());
							}
							base.gameObject.SendMessage("OnDeath");
							if (GameFlowData.Get() != null)
							{
								GameFlowData.Get().NotifyOnActorDeath(this);
							}
							UnoccupyCurrentBoardSquare();
							SetCurrentBoardSquare(null);
							ClientLastKnownPosSquare = null;
							ServerLastKnownPosSquare = null;
							return;
						}
					}
				}
			}
			if (flag)
			{
				return;
			}
			while (true)
			{
				if (m_hitPoints <= 0 || LastDeathTurn <= 0)
				{
					return;
				}
				while (true)
				{
					base.gameObject.SendMessage("OnRespawn");
					m_lastVisibleTurnToClient = 0;
					if (!NetworkServer.active)
					{
						return;
					}
					while (true)
					{
						if (m_teamSensitiveData_friendly != null)
						{
							m_teamSensitiveData_friendly.MarkAsRespawning();
						}
						if (m_teamSensitiveData_hostile != null)
						{
							m_teamSensitiveData_hostile.MarkAsRespawning();
						}
						if (num > 0)
						{
							while (true)
							{
								switch (5)
								{
								default:
									return;
								case 0:
									break;
								}
							}
						}
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
			if (_unresolvedDamage == value)
			{
				return;
			}
			while (true)
			{
				_unresolvedDamage = value;
				ClientUnresolvedDamage = 0;
				return;
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
			if (_unresolvedHealing == value)
			{
				return;
			}
			while (true)
			{
				_unresolvedHealing = value;
				ClientUnresolvedHealing = 0;
				return;
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
			if (_unresolvedTechPointGain == value)
			{
				return;
			}
			while (true)
			{
				_unresolvedTechPointGain = value;
				ClientUnresolvedTechPointGain = 0;
				return;
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
			if (_unresolvedTechPointLoss == value)
			{
				return;
			}
			while (true)
			{
				_unresolvedTechPointLoss = value;
				ClientUnresolvedTechPointLoss = 0;
				return;
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
			if (m_serverExpectedHoTTotal == value)
			{
				return;
			}
			while (true)
			{
				m_serverExpectedHoTTotal = value;
				ClientExpectedHoTTotalAdjust = 0;
				return;
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
			if (m_serverExpectedHoTThisTurn == value)
			{
				return;
			}
			while (true)
			{
				m_serverExpectedHoTThisTurn = value;
				return;
			}
		}
	}

	internal int ClientUnresolvedDamage
	{
		get;
		set;
	}

	internal int ClientUnresolvedHealing
	{
		get;
		set;
	}

	internal int ClientUnresolvedTechPointGain
	{
		get;
		set;
	}

	internal int ClientUnresolvedTechPointLoss
	{
		get;
		set;
	}

	internal int ClientUnresolvedAbsorb
	{
		get;
		set;
	}

	internal int ClientReservedTechPoints
	{
		get;
		set;
	}

	internal int ClientExpectedHoTTotalAdjust
	{
		get;
		set;
	}

	internal int ClientAppliedHoTThisTurn
	{
		get;
		set;
	}

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
				return;
			}
			m_techPoints = value;
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
			if (!NetworkServer.active)
			{
				return;
			}
			while (true)
			{
				m_ignoreForEnergyForHit = value;
				return;
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
			while (true)
			{
				if (NetworkServer.active)
				{
					m_absorbPoints = Mathf.Max(value, 0);
				}
				else
				{
					m_absorbPoints = Mathf.Max(value, 0);
				}
				ClientUnresolvedAbsorb = 0;
				return;
			}
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
			if (m_spawnerId == value)
			{
				return;
			}
			while (true)
			{
				m_spawnerId = value;
				return;
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
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return m_teamSensitiveData_friendly;
					}
				}
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
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return m_trueMoveFromBoardSquare;
					}
				}
			}
			if (m_teamSensitiveData_friendly != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return m_teamSensitiveData_friendly.MoveFromBoardSquare;
					}
				}
			}
			return CurrentBoardSquare;
		}
		set
		{
			if (!NetworkServer.active || !(m_trueMoveFromBoardSquare != value))
			{
				return;
			}
			while (true)
			{
				m_trueMoveFromBoardSquare = value;
				if (m_teamSensitiveData_friendly != null)
				{
					m_teamSensitiveData_friendly.MoveFromBoardSquare = value;
				}
				return;
			}
		}
	}

	public BoardSquare InitialMoveStartSquare
	{
		get
		{
			if (NetworkServer.active)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return m_serverInitialMoveStartSquare;
					}
				}
			}
			if (m_teamSensitiveData_friendly != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return m_teamSensitiveData_friendly.InitialMoveStartSquare;
					}
				}
			}
			return CurrentBoardSquare;
		}
		set
		{
			if (!NetworkServer.active)
			{
				return;
			}
			while (true)
			{
				if (!(m_serverInitialMoveStartSquare != value))
				{
					return;
				}
				while (true)
				{
					m_serverInitialMoveStartSquare = value;
					if (GetActorMovement() != null)
					{
						GetActorMovement().UpdateSquaresCanMoveTo();
					}
					if (m_teamSensitiveData_friendly != null)
					{
						while (true)
						{
							m_teamSensitiveData_friendly.InitialMoveStartSquare = value;
							return;
						}
					}
					return;
				}
			}
		}
	}

	public float RemainingHorizontalMovement
	{
		get;
		set;
	}

	public float RemainingMovementWithQueuedAbility
	{
		get;
		set;
	}

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

	internal Vector3 LastDeathPosition
	{
		get;
		private set;
	}

	internal int LastDeathTurn
	{
		get;
		private set;
	}

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
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return m_trueRespawnSquares;
					}
				}
			}
			if (m_teamSensitiveData_friendly != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return m_teamSensitiveData_friendly.RespawnAvailableSquares;
					}
				}
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
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return m_trueRespawnPositionSquare;
					}
				}
			}
			if (m_teamSensitiveData_friendly != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return m_teamSensitiveData_friendly.RespawnPickedSquare;
					}
				}
			}
			if (m_teamSensitiveData_hostile != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return m_teamSensitiveData_hostile.RespawnPickedSquare;
					}
				}
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
					if (ShouldRevealRespawnSquareToEnemy(value))
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

	public bool VisibleTillEndOfPhase
	{
		get;
		set;
	}

	public bool CurrentlyVisibleForAbilityCast
	{
		get
		{
			return m_currentlyVisibleForAbilityCast;
		}
		set
		{
			if (m_currentlyVisibleForAbilityCast == value)
			{
				return;
			}
			if (ActorDebugUtils.Get() != null)
			{
				if (ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.LastKnownPosition, false))
				{
					Debug.LogWarning(GetDebugName() + "Setting visible for ability cast to " + value);
				}
			}
			m_currentlyVisibleForAbilityCast = value;
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
			if (m_movedForEvade == value)
			{
				return;
			}
			while (true)
			{
				m_movedForEvade = value;
				return;
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
			if (m_serverSuppressInvisibility == value)
			{
				return;
			}
			while (true)
			{
				m_serverSuppressInvisibility = value;
				return;
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

	private Action OnTurnStartDelegatesHolder;
	public event Action OnTurnStartDelegates
	{
		add
		{
			Action action = this.OnTurnStartDelegatesHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnTurnStartDelegatesHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action action = this.OnTurnStartDelegatesHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnTurnStartDelegatesHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	private Action<UnityEngine.Object, GameObject> OnAnimationEventDelegatesHolder;
	public event Action<UnityEngine.Object, GameObject> OnAnimationEventDelegates;

	private Action<Ability> OnSelectedAbilityChangedDelegatesHolder;
	public event Action<Ability> OnSelectedAbilityChangedDelegates
	{
		add
		{
			Action<Ability> action = this.OnSelectedAbilityChangedDelegatesHolder;
			Action<Ability> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnSelectedAbilityChangedDelegatesHolder, (Action<Ability>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<Ability> action = this.OnSelectedAbilityChangedDelegatesHolder;
			Action<Ability> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnSelectedAbilityChangedDelegatesHolder, (Action<Ability>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	private Action OnClientQueuedActionChangedDelegatesHolder;
	public event Action OnClientQueuedActionChangedDelegates
	{
		add
		{
			Action action = this.OnClientQueuedActionChangedDelegatesHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnClientQueuedActionChangedDelegatesHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action action = this.OnClientQueuedActionChangedDelegatesHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnClientQueuedActionChangedDelegatesHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public ActorData()
	{
		this.OnTurnStartDelegatesHolder = delegate
		{
		};
		this.OnAnimationEventDelegatesHolder = delegate
		{
		};
		
		this.OnSelectedAbilityChangedDelegatesHolder = delegate
			{
			};
		
		this.OnClientQueuedActionChangedDelegatesHolder = delegate
			{
			};
		m_serializeHelper = new SerializeHelper();
		m_forceShowOutlineCheckers = new List<IForceActorOutlineChecker>();
		
	}

	static ActorData()
	{
		s_invalidActorIndex = -1;
		s_nextActorIndex = 0;
		s_friendlyPlayerColor = new Color(0f, 1f, 0f, 1f);
		s_friendlyDamagedHealthBarColor = new Color(0.5f, 0.9f, 0.5f, 1f);
		s_hostilePlayerColor = new Color(1f, 0f, 0f, 1f);
		s_hostileDamagedHealthBarColor = new Color(1f, 0.6f, 0.6f, 1f);
		s_neutralPlayerColor = new Color(0f, 1f, 1f, 1f);
		s_teamAColor = new Color(0.2f, 0.2f, 1f, 1f);
		s_teamBColor = new Color(1f, 0.5f, 0f, 1f);
		s_visibleTimeAfterHit = 1f;
		s_visibleTimeAfterMovementHit = 0.3f;
		kCmdCmdSetPausedForDebugging = 1605310550;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), kCmdCmdSetPausedForDebugging, InvokeCmdCmdSetPausedForDebugging);
		kCmdCmdSetResolutionSingleStepping = -1306898411;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), kCmdCmdSetResolutionSingleStepping, InvokeCmdCmdSetResolutionSingleStepping);
		kCmdCmdSetResolutionSingleSteppingAdvance = -1612013907;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), kCmdCmdSetResolutionSingleSteppingAdvance, InvokeCmdCmdSetResolutionSingleSteppingAdvance);
		kCmdCmdSetDebugToggleParam = -77600279;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), kCmdCmdSetDebugToggleParam, InvokeCmdCmdSetDebugToggleParam);
		kCmdCmdDebugReslotCards = -967932322;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), kCmdCmdDebugReslotCards, InvokeCmdCmdDebugReslotCards);
		kCmdCmdDebugSetAbilityMod = 1885146022;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), kCmdCmdDebugSetAbilityMod, InvokeCmdCmdDebugSetAbilityMod);
		kCmdCmdDebugReplaceWithBot = -1932690655;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), kCmdCmdDebugReplaceWithBot, InvokeCmdCmdDebugReplaceWithBot);
		kCmdCmdDebugSetHealthOrEnergy = 946344949;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), kCmdCmdDebugSetHealthOrEnergy, InvokeCmdCmdDebugSetHealthOrEnergy);
		kRpcRpcOnHitPointsResolved = 189834458;
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorData), kRpcRpcOnHitPointsResolved, InvokeRpcRpcOnHitPointsResolved);
		kRpcRpcCombatText = -1860175530;
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorData), kRpcRpcCombatText, InvokeRpcRpcCombatText);
		kRpcRpcApplyAbilityModById = -1097840381;
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorData), kRpcRpcApplyAbilityModById, InvokeRpcRpcApplyAbilityModById);
		kRpcRpcMarkForRecalculateClientVisibility = -701731415;
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorData), kRpcRpcMarkForRecalculateClientVisibility, InvokeRpcRpcMarkForRecalculateClientVisibility);
		kRpcRpcForceLeaveGame = -1193160397;
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorData), kRpcRpcForceLeaveGame, InvokeRpcRpcForceLeaveGame);
		NetworkCRC.RegisterBehaviour("ActorData", 0);
	}

	public int GetLastVisibleTurnToClient()
	{
		return m_lastVisibleTurnToClient;
	}

	public Vector3 GetClientLastKnownPos()
	{
		if ((bool)ClientLastKnownPosSquare)
		{
			return ClientLastKnownPosSquare.transform.position;
		}
		return Vector3.zero;
	}

	public Vector3 GetServerLastKnownPos()
	{
		if ((bool)ServerLastKnownPosSquare)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return ServerLastKnownPosSquare.transform.position;
				}
			}
		}
		return Vector3.zero;
	}

	public void ActorData_OnActorMoved(BoardSquare movementSquare, bool visibleToEnemies, bool updateLastKnownPos)
	{
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			if (updateLastKnownPos)
			{
				ClientLastKnownPosSquare = movementSquare;
				m_lastVisibleTurnToClient = GameFlowData.Get().CurrentTurn;
			}
			m_shouldUpdateLastVisibleToClientThisFrame = false;
			return;
		}
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

	internal NPCBrain GetEnabledNPCBrain()
	{
		ActorController actorController = GetActorController();
		if (actorController != null)
		{
			NPCBrain[] components = GetComponents<NPCBrain>();
			NPCBrain[] array = components;
			foreach (NPCBrain nPCBrain in array)
			{
				if (!nPCBrain.enabled)
				{
					continue;
				}
				while (true)
				{
					return nPCBrain;
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

	internal ActorAdditionalVisionProviders GetActorAdditionalVisionProviders()
	{
		return m_additionalVisionProvider;
	}

	internal PassiveData GetPassiveData()
	{
		return m_passiveData;
	}

	public string GetDisplayNameForLog()
	{
		if (HasBotController &&
			GetAccountId() == 0 &&
			m_characterType != CharacterType.None &&
			!GetPlayerDetails().m_botsMasqueradeAsHumans)
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
						ActorData x = GameFlowData.Get().FindActorByActorIndex(actorIndex);
						if (x == this)
						{
							ActorData component = componentsInChildren[i].GetComponent<ActorData>();
							if (component.m_displayName != "FT")
							{
								return component.GetDisplayNameForLog();
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

	public string GetObjectName()
	{
		return base.name.Replace("(Clone)", string.Empty);
	}

	public float GetPostAbilityHorizontalMovementChange()
	{
		return m_maxHorizontalMovement - m_postAbilityHorizontalMovement;
	}

	public int GetMaxHitPoints()
	{
		int result = 1;
		ActorStats actorStats = m_actorStats;
		if (actorStats != null)
		{
			result = actorStats.GetModifiedStatInt(StatType.MaxHitPoints);
		}
		return result;
	}

	public void OnMaxHitPointsChanged(int previousMax)
	{
		if (IsDead())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		float num = (float)HitPoints / (float)previousMax;
		int maxHitPoints = GetMaxHitPoints();
		HitPoints = Mathf.RoundToInt((float)maxHitPoints * num);
	}

	public float GetHitPointShareOfMax()
	{
		int maxHitPoints = GetMaxHitPoints();
		int hitPoints = HitPoints;
		return (float)hitPoints / (float)maxHitPoints;
	}

	public int GetPassiveHpRegen()
	{
		int num = 0;
		ActorStats actorStats = m_actorStats;
		if (actorStats != null)
		{
			num = actorStats.GetModifiedStatInt(StatType.HitPointRegen);
			float modifiedStatFloat = actorStats.GetModifiedStatFloat(StatType.HitPointRegenPercentOfMax);
			float modifiedStatFloat2 = actorStats.GetModifiedStatFloat(StatType.MaxHitPoints);
			int num2 = Mathf.RoundToInt(modifiedStatFloat * modifiedStatFloat2);
			num += num2;
		}
		if (GameplayMutators.Get() != null)
		{
			num = Mathf.RoundToInt((float)num * GameplayMutators.GetPassiveHpRegenMultiplier());
		}
		return num;
	}

	public int GetMaxTechPoints()
	{
		int result = 1;
		ActorStats actorStats = m_actorStats;
		if (actorStats != null)
		{
			result = actorStats.GetModifiedStatInt(StatType.MaxTechPoints);
		}
		return result;
	}

	public void OnMaxTechPointsChanged(int previousMax)
	{
		if (IsDead())
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
		int actualMaxTechPoints = GetMaxTechPoints();
		if (actualMaxTechPoints > previousMax)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					int num = actualMaxTechPoints - previousMax;
					TechPoints += num;
					return;
				}
				}
			}
		}
		if (previousMax <= actualMaxTechPoints)
		{
			return;
		}
		while (true)
		{
			int techPoints = TechPoints;
			TechPoints = Mathf.Min(TechPoints, actualMaxTechPoints);
			if (techPoints - TechPoints == 0)
			{
			}
			return;
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
			bool isInResolveState = GameFlowData.Get()?.IsInResolveState() ?? false;
			bool isNotMoving = GetTravelBoardSquare() == GetCurrentBoardSquare() && !GetActorMovement().AmMoving() && !GetActorMovement().IsYetToCompleteGameplayPath();
			bool isFromAnotherTeam = GetTeam() != team;
			if (isInResolveState && isNotMoving && isFromAnotherTeam && IsVisibleToClient())
			{
				ForceUpdateIsVisibleToClientCache();
				if (IsVisibleToClient())
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
		if (PlayerData != null)
		{
			if (GameFlow.Get() != null)
			{
				if (GameFlow.Get().playerDetails != null && GameFlow.Get().playerDetails.ContainsKey(PlayerData.GetPlayer()))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return GameFlow.Get().playerDetails[PlayerData.GetPlayer()];
						}
					}
				}
			}
		}
		return null;
	}

	public void SetupAbilityModOnReconnect()
	{
		List<ActorData> actors = GameFlowData.Get().GetActors();
		foreach (ActorData item in actors)
		{
			if (item != null)
			{
				if (item.GetAbilityData() != null)
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
	}

	public void SetupForRespawnOnReconnect()
	{
		if (!IsPickingRespawnSquare())
		{
			return;
		}
		while (true)
		{
			if (!(GameFlowData.Get() != null))
			{
				return;
			}
			if (ServerClientUtils.GetCurrentActionPhase() >= ActionBufferPhase.Movement)
			{
				if (ServerClientUtils.GetCurrentActionPhase() <= ActionBufferPhase.MovementWait)
				{
					return;
				}
			}
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
				while (true)
				{
					ShowRespawnFlare(RespawnPickedPositionSquare, true);
					return;
				}
			}
			return;
		}
	}

	public void SetupAbilityMods(CharacterModInfo characterMods)
	{
		m_selectedMods = characterMods;
		AbilityData abilityData = GetAbilityData();
		List<Ability> abilitiesAsList = abilityData.GetAbilitiesAsList();
		int num = 0;
		using (List<Ability>.Enumerator enumerator = abilitiesAsList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Ability current = enumerator.Current;
				int num2 = -1;
				if (GameManager.Get().GameConfig.GameType == GameType.Tutorial)
				{
					AbilityMod defaultModForAbility = AbilityModManager.Get().GetDefaultModForAbility(current);
					int num3;
					if (defaultModForAbility != null)
					{
						num3 = defaultModForAbility.m_abilityScopeId;
					}
					else
					{
						num3 = -1;
					}
					num2 = num3;
				}
				else
				{
					num2 = m_selectedMods.GetModForAbility(num);
				}
				AbilityData.ActionType actionTypeOfAbility = abilityData.GetActionTypeOfAbility(current);
				if (actionTypeOfAbility != AbilityData.ActionType.INVALID_ACTION)
				{
					if (num2 > 0)
					{
						ApplyAbilityModById((int)actionTypeOfAbility, num2);
					}
				}
				num++;
			}
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void UpdateServerLastVisibleTurn()
	{
	}

	public void SynchClientLastKnownPosToServerLastKnownPos()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			if (!(ClientLastKnownPosSquare != ServerLastKnownPosSquare))
			{
				return;
			}
			ClientLastKnownPosSquare = ServerLastKnownPosSquare;
			if (!(GameFlowData.Get().activeOwnedActorData != null))
			{
				return;
			}
			while (true)
			{
				if (GetTeam() == GameFlowData.Get().activeOwnedActorData.GetTeam())
				{
				}
				return;
			}
		}
	}

	public int GetPassiveEnergyRegen()
	{
		int num = 0;
		ActorStats actorStats = m_actorStats;
		if (actorStats != null)
		{
			num = actorStats.GetModifiedStatInt(StatType.TechPointRegen);
		}
		if (GameplayMutators.Get() != null)
		{
			num = Mathf.RoundToInt((float)num * GameplayMutators.GetPassiveEnergyRegenMultiplier());
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
		if (m_actorStatus != null)
		{
			if (m_actorStatus.HasStatus(StatusType.Blind))
			{
				result = 0.1f;
			}
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
		bool flag = healAmount >= 0;
		string combatText = $"{healAmount}";
		string text;
		if (caster == null)
		{
			text = string.Empty;
		}
		else
		{
			text = $"{caster.DisplayName}'s ";
		}
		string text2 = text;
		string logText = (!flag) ? string.Format("{0}{1} removes {3} Energy from {2}", text2, sourceName, target.DisplayName, healAmount) : string.Format("{0}{1} adds {3} Energy to {2}", text2, sourceName, target.DisplayName, healAmount);
		int category;
		if (flag)
		{
			category = 5;
		}
		else
		{
			category = 4;
		}
		target.CallRpcCombatText(combatText, logText, (CombatTextCategory)category, BuffIconToDisplay.None);
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

	public ActorTeamSensitiveData GetTeamSensitiveData()
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
		if (!(m_teamSensitiveData_friendly != null))
		{
			return;
		}
		while (true)
		{
			m_teamSensitiveData_friendly.RespawnAvailableSquares = new List<BoardSquare>();
			return;
		}
	}

	public bool ShouldRevealRespawnSquareToEnemy(BoardSquare square)
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

	internal bool IsLineOfSightVisibleException(ActorData actor)
	{
		return m_lineOfSightVisibleExceptions.Contains(actor);
	}

	public void OnClientQueuedActionChanged()
	{
		if (this.OnClientQueuedActionChangedDelegatesHolder == null)
		{
			return;
		}
		while (true)
		{
			this.OnClientQueuedActionChangedDelegatesHolder();
			return;
		}
	}

	public void OnSelectedAbilityChanged(Ability ability)
	{
		if (this.OnSelectedAbilityChangedDelegatesHolder != null)
		{
			this.OnSelectedAbilityChangedDelegatesHolder(ability);
		}
	}

	private void Awake()
	{
		
		GameFlowData.s_onGameStateChanged -= OnGameStateChanged;
		
		GameFlowData.s_onGameStateChanged += OnGameStateChanged;
		PlayerData = GetComponent<PlayerData>();
		if (PlayerData == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					throw new Exception($"Character {base.gameObject.name} needs a PlayerData component");
				}
			}
		}
		m_actorMovement = base.gameObject.GetComponent<ActorMovement>();
		if (m_actorMovement == null)
		{
			m_actorMovement = base.gameObject.AddComponent<ActorMovement>();
		}
		m_actorTurnSM = base.gameObject.GetComponent<ActorTurnSM>();
		if (m_actorTurnSM == null)
		{
			m_actorTurnSM = base.gameObject.AddComponent<ActorTurnSM>();
		}
		m_actorCover = base.gameObject.GetComponent<ActorCover>();
		if (m_actorCover == null)
		{
			m_actorCover = base.gameObject.AddComponent<ActorCover>();
		}
		m_actorVFX = base.gameObject.GetComponent<ActorVFX>();
		if (m_actorVFX == null)
		{
			m_actorVFX = base.gameObject.AddComponent<ActorVFX>();
		}
		m_timeBank = base.gameObject.GetComponent<TimeBank>();
		if (m_timeBank == null)
		{
			m_timeBank = base.gameObject.AddComponent<TimeBank>();
		}
		m_additionalVisionProvider = base.gameObject.GetComponent<ActorAdditionalVisionProviders>();
		if (m_additionalVisionProvider == null)
		{
			m_additionalVisionProvider = base.gameObject.AddComponent<ActorAdditionalVisionProviders>();
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
		if ((bool)GameFlowData.Get())
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

	public void InitializeModel(PrefabResourceLink heroPrefabLink, bool addMasterSkinVfx)
	{
		if (m_actorSkinPrefabLink.GUID == heroPrefabLink.GUID)
		{
			while (true)
			{
				return;
			}
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
		if (!NetworkClient.active)
		{
			if (HydrogenConfig.Get().SkipCharacterModelSpawnOnServer)
			{
				goto IL_0155;
			}
		}
		GameObject gameObject = heroPrefabLink.InstantiatePrefab();
		if ((bool)gameObject)
		{
			m_actorModelData = gameObject.GetComponent<ActorModelData>();
			if ((bool)m_actorModelData)
			{
				int layer = LayerMask.NameToLayer("Actor");
				Transform[] componentsInChildren = m_actorModelData.gameObject.GetComponentsInChildren<Transform>(true);
				foreach (Transform transform in componentsInChildren)
				{
					transform.gameObject.layer = layer;
				}
			}
		}
		goto IL_0155;
		IL_0155:
		if (m_actorModelData != null)
		{
			m_actorModelData.Setup(this);
			if (addMasterSkinVfx)
			{
				if (NetworkClient.active && MasterSkinVfxData.Get() != null)
				{
					GameObject masterSkinVfxInst = MasterSkinVfxData.Get().AddMasterSkinVfxOnCharacterObject(m_actorModelData.gameObject, m_characterType, 1f);
					m_actorModelData.SetMasterSkinVfxInst(masterSkinVfxInst);
				}
			}
		}
		if (m_faceActorModelData != null)
		{
			m_faceActorModelData.Setup(this);
		}
		if (!NPCCoordinator.IsSpawnedNPC(this))
		{
			return;
		}
		while (true)
		{
			if (!m_addedToUI)
			{
				if (HUD_UI.Get() != null && HUD_UI.Get().m_mainScreenPanel != null)
				{
					m_addedToUI = true;
					HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.AddActor(this);
					HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddActor(this);
				}
			}
			NPCCoordinator.Get().OnActorSpawn(this);
			if (GetActorModelData() != null)
			{
				while (true)
				{
					GetActorModelData().ForceUpdateVisibility();
					return;
				}
			}
			return;
		}
	}

	private void Start()
	{
		if (NetworkClient.active)
		{
			m_nameplateJoint = new JointPopupProperty();
			m_nameplateJoint.m_joint = "VFX_name";
			m_nameplateJoint.Initialize(base.gameObject);
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
		base.transform.parent = GameFlowData.Get().GetActorRoot().transform;
		GameFlowData.Get().AddActor(this);
		EnableRagdoll(false);
		if (!m_addedToUI && HUD_UI.Get() != null)
		{
			if (HUD_UI.Get().m_mainScreenPanel != null)
			{
				m_addedToUI = true;
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.AddActor(this);
				HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddActor(this);
			}
		}
		PlayerDetails value = null;
		if (!GameFlow.Get().playerDetails.TryGetValue(PlayerData.GetPlayer(), out value) || !value.IsLocal())
		{
			return;
		}
		while (true)
		{
			Log.Info("ActorData.Start {0} {1}", this, value);
			GameFlowData.Get().AddOwnedActorData(this);
			return;
		}
	}

	public override void OnStartLocalPlayer()
	{
		Log.Info("ActorData.OnStartLocalPlayer {0}", this);
		GameFlowData.Get().AddOwnedActorData(this);
		if (!ClientBootstrap.LoadTest)
		{
			return;
		}
		while (true)
		{
			CallCmdDebugReplaceWithBot();
			return;
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
			List<ActorData> actors2 = GameFlowData.Get().GetActors();
			bool flag = false;
			using (List<ActorData>.Enumerator enumerator2 = actors2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData current2 = enumerator2.Current;
					if (current2 != null)
					{
						if (!current2.IsDead())
						{
							if (current2.GetActorModelData() != null)
							{
								if (current2.IsModelAnimatorDisabled())
								{
									Debug.LogError("Unragdolling undead actor on Turn Tick (" + currentTurn + "): " + current2.GetDebugName());
									current2.EnableRagdoll(false);
									flag = true;
								}
							}
						}
					}
					if (current2 != null && !current2.IsDead())
					{
						if (current2.GetCurrentBoardSquare() == null)
						{
							if (current2.PlayerIndex != PlayerData.s_invalidPlayerIndex)
							{
								if (NetworkClient.active)
								{
									if (!NetworkServer.active)
									{
										if (GameFlowData.Get().LocalPlayerData.IsViewingTeam(current2.GetTeam()))
										{
											Debug.LogError("On client, living friendly-to-client actor " + current2.GetDebugName() + " has null square on Turn Tick");
											flag = true;
										}
									}
								}
							}
						}
					}
				}
			}
			if (!NetworkServer.active)
			{
				return;
			}
			while (true)
			{
				if (flag)
				{
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
				return;
			}
		}
		case GameState.BothTeams_Resolve:
		{
			List<ActorData> actors = GameFlowData.Get().GetActors();
			using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					if (current != null)
					{
						if (current.GetActorModelData() != null)
						{
							Animator modelAnimator = current.GetModelAnimator();
							if (modelAnimator != null)
							{
								if (current.GetActorModelData().HasTurnStartParameter())
								{
									modelAnimator.SetBool("TurnStart", false);
								}
							}
						}
						if (current.GetComponent<LineData>() != null)
						{
							current.GetComponent<LineData>().OnResolveStart();
						}
						if (HUD_UI.Get() != null)
						{
							HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateNameplateUntargeted(current);
						}
					}
				}
				while (true)
				{
					switch (5)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
		}
		while (true)
		{
			if (newState != GameState.EndingGame)
			{
				while (true)
				{
					switch (4)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			s_nextActorIndex = 0;
			return;
		}
	}

	private static void HandleRagdollOnDecisionStart()
	{
		List<ActorData> actors = GameFlowData.Get().GetActors();
		for (int i = 0; i < actors.Count; i++)
		{
			ActorData actorData = actors[i];
			if (!(actorData != null))
			{
				continue;
			}
			if (!actorData.IsDead())
			{
				continue;
			}
			if (actorData.LastDeathTurn == GameFlowData.Get().CurrentTurn || actorData.IsModelAnimatorDisabled())
			{
				continue;
			}
			if (actorData.NextRespawnTurn != GameFlowData.Get().CurrentTurn)
			{
				actorData.DoVisualDeath(Sequence.CreateImpulseInfoWithActorForward(actorData));
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

	public Animator GetModelAnimator()
	{
		if (m_actorModelData != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return m_actorModelData.GetModelAnimator();
				}
			}
		}
		return null;
	}

	public void PlayDamageReactionAnim(string customDamageReactTriggerName)
	{
		Animator modelAnimator = GetModelAnimator();
		if (modelAnimator == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (m_actorMovement.GetAestheticPath() != null)
		{
			return;
		}
		while (true)
		{
			if (m_actorMovement.AmMoving())
			{
				return;
			}
			while (true)
			{
				bool flag = false;
				int num;
				if (ServerClientUtils.GetCurrentAbilityPhase() == AbilityPriority.Combat_Knockback)
				{
					if (ClientKnockbackManager.Get() != null)
					{
						num = (ClientKnockbackManager.Get().ActorHasIncomingKnockback(this) ? 1 : 0);
						goto IL_0093;
					}
				}
				num = 0;
				goto IL_0093;
				IL_0093:
				if (num != 0)
				{
					return;
				}
				while (true)
				{
					object trigger;
					if (string.IsNullOrEmpty(customDamageReactTriggerName))
					{
						trigger = "StartDamageReaction";
					}
					else
					{
						trigger = customDamageReactTriggerName;
					}
					modelAnimator.SetTrigger((string)trigger);
					return;
				}
			}
		}
	}

	internal bool IsModelAnimatorDisabled()
	{
		return !(GetModelAnimator()?.enabled ?? false);
	}

	internal void DoVisualDeath(ActorModelData.ImpulseInfo impulseInfo)
	{
		if (IsModelAnimatorDisabled())
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
			AudioManager.PostEvent("ui/ingame/death", base.gameObject);
			if (!string.IsNullOrEmpty(m_onDeathAudioEvent))
			{
				AudioManager.PostEvent(m_onDeathAudioEvent, base.gameObject);
			}
		}
		Team team = Team.Invalid;
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().LocalPlayerData != null)
			{
				team = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
			}
		}
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (clientFog != null)
		{
			if (GetTeam() == team)
			{
				clientFog.MarkForRecalculateVisibility();
			}
		}
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			if (ObjectivePoints.Get() != null)
			{
				ObjectivePoints.Get().Client_OnActorDeath(this);
				if (GameplayUtils.IsPlayerControlled(this) && GameFlowData.Get().LocalPlayerData != null)
				{
					int num = ObjectivePoints.Get().Client_GetNumDeathOnTeamForCurrentTurn(GetTeam());
					if (num > 0)
					{
						if (UIDeathNotifications.Get() != null)
						{
							UIDeathNotifications.Get().NotifyDeathOccurred(this, GetTeam() == team);
						}
					}
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
			if (GameWideData.Get().FreeAutomaticOverconOnDeath_OverconID == -1)
			{
				return;
			}
			while (true)
			{
				if (UIOverconData.Get() != null)
				{
					while (true)
					{
						UIOverconData.Get().UseOvercon(GameWideData.Get().FreeAutomaticOverconOnDeath_OverconID, ActorIndex, true);
						return;
					}
				}
				return;
			}
		}
	}

	private void EnableRagdoll(bool ragDollOn, ActorModelData.ImpulseInfo impulseInfo = null, bool isDebugRagdoll = false)
	{
		if (ragDollOn && GetHitPointsToDisplay() > 0)
		{
			if (!isDebugRagdoll)
			{
				Log.Error("early_ragdoll: enabling ragdoll on " + GetDebugName() + " with " + HitPoints + " HP,  (HP for display " + GetHitPointsToDisplay() + ")\n" + Environment.StackTrace);
			}
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
		if (!(this == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		while (true)
		{
			if (!(SpawnPointManager.Get() != null))
			{
				return;
			}
			while (true)
			{
				if (SpawnPointManager.Get().m_spawnInDuringMovement)
				{
					while (true)
					{
						InterfaceManager.Get().DisplayAlert(StringUtil.TR("PostRespawnMovement", "Global"), BoardSquare.s_respawnOptionHighlightColor, 60f, true);
						return;
					}
				}
				return;
			}
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
		if (!NetworkServer.active)
		{
			if (NPCCoordinator.IsSpawnedNPC(this))
			{
				NPCCoordinator.Get().OnActorSpawn(this);
			}
		}
		GameEventManager.Get().FireEvent(GameEventManager.EventType.CharacterRespawn, new GameEventManager.CharacterRespawnEventArgs
		{
			respawningCharacter = this
		});
		if (GameFlowData.Get().activeOwnedActorData == this)
		{
			CameraManager.Get().SetTargetObject(base.gameObject, CameraManager.CameraTargetReason.ClientActorRespawned);
		}
		m_lastSpawnTurn = GameFlowData.Get().CurrentTurn;
	}

	public bool IsPickingRespawnSquare()
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
		if (m_needAddToTeam)
		{
			if (GameFlowData.Get() != null)
			{
				m_needAddToTeam = false;
				GameFlowData.Get().AddToTeam(this);
				TeamStatusDisplay.GetTeamStatusDisplay().RebuildTeamDisplay();
			}
		}
		if (NetworkClient.active)
		{
			UpdateClientLastKnownPosSquare();
		}
		if (Quaternion.Angle(base.transform.localRotation, m_targetRotation) > 0.01f)
		{
			base.transform.localRotation = m_targetRotation;
		}
		if (m_callHandleOnSelectInUpdate)
		{
			HandleOnSelect();
			m_callHandleOnSelectInUpdate = false;
		}
		if (NetworkServer.active)
		{
			SetDirtyBit(1u);
		}
		if (m_addedToUI)
		{
			return;
		}
		while (true)
		{
			if (HUD_UI.Get() != null)
			{
				m_addedToUI = true;
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.AddActor(this);
				HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddActor(this);
			}
			return;
		}
	}

	public bool IsHiddenInBrush()
	{
		int travelBoardSquareBrushRegion = GetTravelBoardSquareBrushRegion();
		if (travelBoardSquareBrushRegion == -1)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (!BrushCoordinator.Get().IsRegionFunctioning(travelBoardSquareBrushRegion))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		return true;
	}

	public int GetTravelBoardSquareBrushRegion()
	{
		int result = -1;
		BoardSquare travelBoardSquare = GetTravelBoardSquare();
		if (travelBoardSquare != null)
		{
			if (travelBoardSquare.IsInBrush())
			{
				result = travelBoardSquare.BrushRegion;
			}
		}
		return result;
	}

	public bool ShouldShowNameplate()
	{
		int result;
		if (!m_hideNameplate && !m_alwaysHideNameplate && ShowInGameGUI)
		{
			if (IsVisibleToClient())
			{
				if (!IsModelAnimatorDisabled())
				{
					if (!(GetActorModelData() == null))
					{
						result = (GetActorModelData().IsVisibleToClient() ? 1 : 0);
					}
					else
					{
						result = 1;
					}
					goto IL_0084;
				}
			}
		}
		result = 0;
		goto IL_0084;
		IL_0084:
		return (byte)result != 0;
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
		if (IsRevealed(localPlayerData, false))
		{
			return true;
		}
		if (IsHidden(localPlayerData, false))
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

	public bool IsVisibleToClient()
	{
		UpdateIsVisibleToClientCache();
		return m_isVisibleToClientCache;
	}

	public bool IsVisibleForChase(ActorData observer)
	{
		if (GameFlowData.Get().IsActorDataOwned(this) && GetTeam() == observer.GetTeam())
		{
			return true;
		}
		if (m_endVisibilityForHitTime > Time.time)
		{
			return true;
		}
		if (IsRevealed(observer.PlayerData))
		{
			return true;
		}
		if (IsHidden(observer.PlayerData))
		{
			return false;
		}
		if (observer.GetFogOfWar() != null)
		{
			return observer.GetFogOfWar().IsVisible(GetTravelBoardSquare());
		}
		return false;
	}

	public bool IsRevealed(PlayerData observer, bool includePendingStatus = true)
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
		return IsDead() && IsModelAnimatorDisabled();
	}

	public bool IsHidden(PlayerData observer, bool includePendingStatus = true, bool forceViewingTeam = false)
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
			if ((bool)observer.ActorData && observer.ActorData.GetActorStatus().HasStatus(StatusType.SeeInvisible, includePendingStatus))
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
			return IsVisibleToClient();
		}
		if (!NetworkServer.active)
		{
			Log.Warning("Calling the server-only method ActorData::IsActorVisibleToActor on a client. Clients can only depend on ActorData::IsActorVisibleToClient.");
		}
		if (IsRevealed(observer.PlayerData))
		{
			return true;
		}
		if (IsHidden(observer.PlayerData, true, forceViewingTeam))
		{
			return false;
		}
		return observer.GetFogOfWar().IsVisible(GetTravelBoardSquare());
	}

	public bool IsVisibleToEnemyTeam()
	{
		List<ActorData> enemyTeamMembers = GameFlowData.Get().GetAllTeamMembers(GetEnemyTeam());
		foreach (ActorData enemy in enemyTeamMembers)
		{
			if (!enemy.IsDead() && IsActorVisibleToActor(enemy, true))
			{
				return true;
			}
		}
		return false;
	}

	public bool CanBeSeen(ActorData observer)
	{
		if (IsRevealed(observer.PlayerData))
		{
			return true;
		}
		if (IsHidden(observer.PlayerData, true, true))
		{
			return false;
		}
		bool IsBrushRevealed = GetTravelBoardSquareBrushRegion() < 0 || BrushRegion.HasTeamMemberInRegion(GetEnemyTeam(), GetTravelBoardSquareBrushRegion());
		return !IsHiddenInBrush() || IsBrushRevealed;
	}

	public void ApplyForceFromPoint(Vector3 pos, float amount, Vector3 overrideDir)
	{
		Vector3 vector = GetBonePosition("hip_JNT") - pos;
		if (!(vector.magnitude < 1.5f))
		{
			return;
		}
		while (true)
		{
			if (overrideDir != Vector3.zero)
			{
				ApplyForce(overrideDir.normalized, amount);
			}
			else
			{
				ApplyForce(vector.normalized, amount);
			}
			return;
		}
	}

	public void ApplyForce(Vector3 dir, float amount)
	{
		Rigidbody hipJointRigidBody = GetHipJointRigidBody();
		if (!hipJointRigidBody)
		{
			return;
		}
		while (true)
		{
			hipJointRigidBody.AddForce(dir * amount, ForceMode.Impulse);
			return;
		}
	}

	public Vector3 GetNameplatePosition(float offsetInPixels)
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

	public Vector3 GetTravelBoardSquareWorldPositionForLos()
	{
		return GetSquareWorldPositionForLoS(GetTravelBoardSquare());
	}

	public Vector3 GetTravelBoardSquareWorldPosition()
	{
		return GetSquareWorldPosition(GetTravelBoardSquare());
	}

	public Vector3 GetSquareWorldPositionForLoS(BoardSquare square)
	{
		if (square != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					Vector3 squareWorldPosition = GetSquareWorldPosition(square);
					squareWorldPosition.y += BoardSquare.s_LoSHeightOffset;
					return squareWorldPosition;
				}
				}
			}
		}
		Log.Warning("Trying to get LoS check pos wrt a null square (IsDead: " + IsDead() + ")  for " + DisplayName + " Code issue-- actor probably instantiated but not on Board yet.");
		return m_actorMovement.transform.position;
	}

	public Vector3 GetSquareWorldPosition(BoardSquare square)
	{
		if (square != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return square.GetOccupantRefPos();
				}
			}
		}
		Log.Warning("Trying to get free pos wrt a null square (IsDead: " + IsDead() + ").  for " + DisplayName + " Code issue-- actor probably instantiated but not on Board yet.");
		return m_actorMovement.transform.position;
	}

	public int GetHitPointsToDisplay()
	{
		int num = UnresolvedDamage + ClientUnresolvedDamage;
		int num2 = UnresolvedHealing + ClientUnresolvedHealing;
		int num3 = AbsorbPoints + ClientUnresolvedAbsorb;
		int num4 = Mathf.Max(0, num - num3);
		return Mathf.Clamp(HitPoints - num4 + num2, 0, GetMaxHitPoints());
	}

	public int GetHitPointsToDisplayWithDelta(int delta)
	{
		int num = UnresolvedDamage + ClientUnresolvedDamage;
		int num2 = UnresolvedHealing + ClientUnresolvedHealing;
		int num3 = AbsorbPoints + ClientUnresolvedAbsorb;
		if (delta > 0)
		{
			num2 += delta;
		}
		else
		{
			num -= delta;
		}
		int num4 = Mathf.Max(0, num - num3);
		return Mathf.Clamp(HitPoints - num4 + num2, 0, GetMaxHitPoints());
	}

	public int GetEnergyToDisplay()
	{
		int num = UnresolvedTechPointGain + ClientUnresolvedTechPointGain;
		int num2 = UnresolvedTechPointLoss + ClientUnresolvedTechPointLoss;
		return Mathf.Clamp(TechPoints + ReservedTechPoints + ClientReservedTechPoints + num - num2, 0, GetMaxTechPoints());
	}

	public int GetPendingHoTTotalToDisplay()
	{
		return Mathf.Max(0, ExpectedHoTTotal + ClientExpectedHoTTotalAdjust - ClientAppliedHoTThisTurn);
	}

	public int GetPendingHoTThisTurnToDisplay()
	{
		return Mathf.Max(0, ExpectedHoTThisTurn - ClientAppliedHoTThisTurn);
	}

	public string GetHitPointsToDisplayDebugString()
	{
		int hitPointsAfterResolution = GetHitPointsToDisplay();
		string text = $"{hitPointsAfterResolution}";
		if (AbsorbPoints > 0)
		{
			int num = UnresolvedDamage + ClientUnresolvedDamage;
			int num2 = AbsorbPoints + ClientUnresolvedAbsorb;
			int num3 = Mathf.Max(0, num2 - num);
			text += $" +{num3} shield";
		}
		return text;
	}

	public int GetAbsorbToDisplay()
	{
		int num = UnresolvedDamage + ClientUnresolvedDamage;
		int num2 = AbsorbPoints + ClientUnresolvedAbsorb;
		return Mathf.Max(0, num2 - num);
	}

	public bool GetIsHumanControlled()
	{
		if (!(GameFlow.Get() == null))
		{
			if (GameFlow.Get().playerDetails != null)
			{
				if (!GameFlow.Get().playerDetails.TryGetValue(PlayerData.GetPlayer(), out PlayerDetails value))
				{
					return false;
				}
				return value.IsHumanControlled;
			}
		}
		Log.Error("Method called too early, results may be incorrect");
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

	public long GetActualAccountId()
	{
		if (!GameFlow.Get().playerDetails.TryGetValue(PlayerData.GetPlayer(), out PlayerDetails playerDetails))
		{
			return -1L;
		}
		if (!IsBotMasqueradingAsHuman() && !playerDetails.IsLoadTestBot && !GetIsHumanControlled())
		{
			return -1L;
		}
		return playerDetails.m_accountId;
	}

	public long GetAccountId()
	{
		if (PlayerData == null)
		{
			return -1L;
		}
		if (!GameFlow.Get().playerDetails.TryGetValue(PlayerData.GetPlayer(), out PlayerDetails playerDetails))
		{
			return -1L;
		}
		return playerDetails.m_accountId;
	}

	private void OnDisable()
	{
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.RemoveActor(this);
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.RemoveActor(this);
		}
		if ((bool)GameFlowData.Get())
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
		while (true)
		{
			if (AsyncPump.Current != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						Log.Info("Waiting for objects to be created . . .");
						AsyncPump.Current.Break();
						return;
					}
				}
			}
			Log.Info("ActorData initialState without an async pump; something may be broken!");
			return;
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
                        addMasterSkinVfx = (characterColor.m_styleLevel == StyleLevelType.Mastery);
                    }
                    InitializeModel(prefabResourceLink, addMasterSkinVfx);
                }
                else
                {
                    Log.Error(string.Concat("Failed to find character resource link for ", m_characterType, " with visual info ", m_visualInfo.ToString()));
                    GameObject gameObject = GameFlowData.Get().m_availableCharacterResourceLinkPrefabs[0];
                    CharacterResourceLink component = gameObject.GetComponent<CharacterResourceLink>();
                    InitializeModel(component.m_skins[0].m_patterns[0].m_colors[0].m_heroPrefab, false);
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
				ServerLastKnownPosSquare = Board.Get().GetSquare(_serverLastKnownPosSquare_x, _serverLastKnownPosSquare_y);
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
				if (GameplayUtils.IsMinion(base.gameObject) && MinionManager.Get() != null)
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
		return m_serializeHelper.End(initialState, base.syncVarDirtyBits);
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
		if (!stream.isReading)
		{
			return;
		}
		while (true)
		{
			abilityVfxSwapInfo.VfxSwapForAbility0 = value;
			abilityVfxSwapInfo.VfxSwapForAbility1 = value2;
			abilityVfxSwapInfo.VfxSwapForAbility2 = value3;
			abilityVfxSwapInfo.VfxSwapForAbility3 = value4;
			abilityVfxSwapInfo.VfxSwapForAbility4 = value5;
			return;
		}
	}

	public static void SerializeCharacterCardInfo(IBitStream stream, ref CharacterCardInfo cardInfo)
	{
		sbyte value = 0;
		sbyte value2 = 0;
		sbyte value3 = 0;
		if (stream.isWriting)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					value = (sbyte)cardInfo.PrepCard;
					value2 = (sbyte)cardInfo.DashCard;
					value3 = (sbyte)cardInfo.CombatCard;
					stream.Serialize(ref value);
					stream.Serialize(ref value2);
					stream.Serialize(ref value3);
					return;
				}
			}
		}
		stream.Serialize(ref value);
		stream.Serialize(ref value2);
		stream.Serialize(ref value3);
		cardInfo.PrepCard = (CardType)value;
		cardInfo.DashCard = (CardType)value2;
		cardInfo.CombatCard = (CardType)value3;
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

	public GridPos GetGridPosWithIncrementedHeight()
	{
		GridPos result = default(GridPos);
		if ((bool)GetCurrentBoardSquare())
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
		if (!(m_facingDirAfterMovement != facingDirAfterMovement))
		{
			return;
		}
		while (true)
		{
			m_facingDirAfterMovement = facingDirAfterMovement;
			if (!NetworkServer.active)
			{
				return;
			}
			while (true)
			{
				if (m_teamSensitiveData_friendly != null)
				{
					m_teamSensitiveData_friendly.FacingDirAfterMovement = m_facingDirAfterMovement;
				}
				if (m_teamSensitiveData_hostile != null)
				{
					while (true)
					{
						m_teamSensitiveData_hostile.FacingDirAfterMovement = m_facingDirAfterMovement;
						return;
					}
				}
				return;
			}
		}
	}

	public Vector3 GetFacingDirectionAfterMovement()
	{
		return m_facingDirAfterMovement;
	}

	public void OnMovementWhileDisappeared(MovementType movementType)
	{
		Debug.Log(GetDebugName() + ": calling OnMovementWhileDisappeared.");
		if (ClientMovementManager.Get() != null)
		{
			ClientMovementManager.Get().OnActorMoveStart_Disappeared(this, movementType);
		}
		if (GetCurrentBoardSquare() != null)
		{
			if (GetCurrentBoardSquare().occupant == base.gameObject)
			{
				UnoccupyCurrentBoardSquare();
			}
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
			if (moverWillDisappear)
			{
				if (path == null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							UnoccupyCurrentBoardSquare();
							SetCurrentBoardSquare(null);
							ForceUpdateIsVisibleToClientCache();
							ForceUpdateActorModelVisibility();
							return;
						}
					}
				}
			}
			string message = $"Actor {DisplayName} in MoveToBoardSquare has null destination (movementType = {movementType.ToString()})";
			Log.Error(message);
			return;
		}
		if (path == null && movementType != MovementType.Teleport)
		{
			string message2 = $"Actor {DisplayName} in MoveToBoardSquare has null path (movementType = {movementType.ToString()})";
			Log.Error(message2);
			return;
		}
		if (ServerClientUtils.GetCurrentAbilityPhase() == AbilityPriority.Evasion)
		{
			MovedForEvade = true;
		}
		int num;
		if (path != null)
		{
			num = (path.WillDieAtEnd() ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		BoardSquare dest2;
		if (path != null && path.GetPathEndpoint() != null)
		{
			if (path.GetPathEndpoint().square != null)
			{
				dest2 = path.GetPathEndpoint().square;
				goto IL_0131;
			}
		}
		dest2 = dest;
		goto IL_0131;
		IL_021e:
		BoardSquare currentBoardSquare;
		BoardSquarePathInfo boardSquarePathInfo;
		if (movementType == MovementType.Teleport)
		{
			ForceUpdateIsVisibleToClientCache();
			ForceUpdateActorModelVisibility();
			SetTransformPositionToSquare(dest);
			m_actorMovement.ClearPath();
			if ((bool)m_actorCover)
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
		else
		{
			if (movementType != 0)
			{
				if (movementType != MovementType.Flight)
				{
					if (movementType != MovementType.WaypointFlight)
					{
						if (movementType != MovementType.Knockback)
						{
							if (movementType != MovementType.Charge)
							{
								goto IL_0458;
							}
						}
						if ((bool)m_actorCover)
						{
							m_actorCover.DisableCover();
						}
						m_actorMovement.BeginChargeOrKnockback(currentBoardSquare, dest, path, movementType);
						m_actorMovement.UpdatePosition();
						if (!flag)
						{
							if (!moverWillDisappear)
							{
								if (path.square == dest)
								{
									if (path.next == null)
									{
										UpdateFacingAfterMovement();
									}
								}
							}
						}
						if (movementType == MovementType.Knockback)
						{
							KnockbackMoveStarted = true;
						}
						goto IL_0458;
					}
				}
			}
			if ((bool)m_actorCover)
			{
				m_actorCover.DisableCover();
			}
			if (path == null)
			{
				path = new BoardSquarePathInfo();
				path.square = currentBoardSquare;
				path.prev = null;
				path.next = new BoardSquarePathInfo();
				path.next.square = dest;
				path.next.prev = path;
				path.next.next = null;
			}
			m_actorMovement.BeginTravellingAlongPath(path, movementType);
			m_actorMovement.UpdatePosition();
		}
		goto IL_0458;
		IL_0458:
		m_actorMovement.UpdateSquaresCanMoveTo();
		GetFogOfWar().MarkForRecalculateVisibility();
		return;
		IL_0131:
		if (ClientMovementManager.Get() != null)
		{
			ClientMovementManager.Get().OnActorMoveStart_ClientMovementManager(this, dest2, movementType, path);
			ClientResolutionManager.Get().OnActorMoveStart_ClientResolutionManager(this, path);
		}
		if (GetCurrentBoardSquare() != null)
		{
			if (GetCurrentBoardSquare().occupant == base.gameObject)
			{
				UnoccupyCurrentBoardSquare();
			}
		}
		currentBoardSquare = CurrentBoardSquare;
		boardSquarePathInfo = null;
		if (movementType == MovementType.Teleport)
		{
			m_actorMovement.ClearPath();
		}
		if (!flag)
		{
			if (!moverWillDisappear)
			{
				SetCurrentBoardSquare(dest);
				if (GetCurrentBoardSquare() != null)
				{
					OccupyCurrentBoardSquare();
				}
				goto IL_021e;
			}
		}
		SetCurrentBoardSquare(null);
		SetMostRecentDeathSquare(dest);
		goto IL_021e;
	}

	public void AppearAtBoardSquare(BoardSquare dest)
	{
		if (dest == null)
		{
			string message = $"Actor {DisplayName} in AppearAtBoardSquare has null destination)";
			Log.Error(message);
			return;
		}
		if (GetCurrentBoardSquare() != null)
		{
			if (GetCurrentBoardSquare().occupant == base.gameObject)
			{
				UnoccupyCurrentBoardSquare();
			}
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
		if (!(refSquare != null))
		{
			return;
		}
		while (true)
		{
			Vector3 worldPosition = refSquare.GetOccupantRefPos();
			SetTransformPositionToVector(worldPosition);
			return;
		}
	}

	public void SetTransformPositionToVector(Vector3 newPos)
	{
		if (!(base.transform.position != newPos))
		{
			return;
		}
		while (true)
		{
			BoardSquare boardSquare = Board.Get().GetSquare(base.transform.position);
			BoardSquare boardSquare2 = Board.Get().GetSquare(newPos);
			if (boardSquare != boardSquare2 && boardSquare != null)
			{
				PreviousBoardSquarePosition = boardSquare.ToVector3();
			}
			base.transform.position = newPos;
			return;
		}
	}

	public void UnoccupyCurrentBoardSquare()
	{
		if (!(GetCurrentBoardSquare() != null))
		{
			return;
		}
		while (true)
		{
			if (GetCurrentBoardSquare().occupant == base.gameObject)
			{
				while (true)
				{
					GetCurrentBoardSquare().occupant = null;
					return;
				}
			}
			return;
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
		if (!(GetCurrentBoardSquare() != null))
		{
			return;
		}
		while (true)
		{
			GetCurrentBoardSquare().occupant = base.gameObject;
			return;
		}
	}

	private void SetCurrentBoardSquare(BoardSquare square)
	{
		if (square == CurrentBoardSquare)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
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
		if (!NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			if (m_teamSensitiveData_friendly != null)
			{
				m_teamSensitiveData_friendly.ClearPreviousMovementInfo();
			}
			if (m_teamSensitiveData_hostile != null)
			{
				while (true)
				{
					m_teamSensitiveData_hostile.ClearPreviousMovementInfo();
					return;
				}
			}
			return;
		}
	}

	public void SetClientFriendlyTeamSensitiveData(ActorTeamSensitiveData friendlyTSD)
	{
		if (m_teamSensitiveData_friendly != friendlyTSD)
		{
			Log.Info("Setting Friendly TeamSensitiveData for " + GetDebugName());
			m_teamSensitiveData_friendly = friendlyTSD;
			m_teamSensitiveData_friendly.OnClientAssociatedWithActor(this);
		}
	}

	public void SetClientHostileTeamSensitiveData(ActorTeamSensitiveData hostileTSD)
	{
		if (m_teamSensitiveData_hostile != hostileTSD)
		{
			Log.Info("Setting Hostile TeamSensitiveData for " + GetDebugName());
			m_teamSensitiveData_hostile = hostileTSD;
			m_teamSensitiveData_hostile.OnClientAssociatedWithActor(this);
		}
	}

	public void UpdateFacingAfterMovement()
	{
		if (!(m_facingDirAfterMovement != Vector3.zero))
		{
			return;
		}
		while (true)
		{
			TurnToDirection(m_facingDirAfterMovement);
			return;
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
		while (true)
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return Team.TeamB;
				}
			}
		}
		if (m_team == Team.TeamB)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return Team.TeamA;
				}
			}
		}
		return Team.Objects;
	}

	public List<Team> GetOtherTeams()
	{
		return GameplayUtils.GetOtherTeamsThan(m_team);
	}

	public List<Team> GetTeams()
	{
		List<Team> list = new List<Team>();
		list.Add(GetTeam());
		return list;
	}

	public List<Team> GetEnemyTeams()
	{
		List<Team> list = new List<Team>();
		list.Add(GetEnemyTeam());
		return list;
	}

	public string GetTeamColorName()
	{
		if (m_team == Team.TeamA)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return "Blue";
				}
			}
		}
		return "Orange";
	}

	public string GetEnemyTeamColorName()
	{
		return (m_team != Team.TeamA) ? "Blue" : "Orange";
	}

	public Color GetTeamColor()
	{
		if (m_team == Team.TeamA)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return s_teamAColor;
				}
			}
		}
		return s_teamBColor;
	}

	public Color GetEnemyTeamColor()
	{
		if (m_team == Team.TeamA)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return s_teamBColor;
				}
			}
		}
		return s_teamAColor;
	}

	public Color GetColorForTeam(Team observingTeam)
	{
		if (observingTeam == GetTeam())
		{
			return s_friendlyPlayerColor;
		}
		return s_hostilePlayerColor;
	}

	internal void OnTurnTick()
	{
		CurrentlyVisibleForAbilityCast = false;
		MovedForEvade = false;
		m_actorMovement.ClearPath();
		GetFogOfWar().MarkForRecalculateVisibility();
		if (!NetworkServer.active)
		{
			if (m_serverMovementWaitForEvent != 0)
			{
				if (m_serverMovementDestination != GetCurrentBoardSquare() && !IsDead())
				{
					MoveToBoardSquareLocal(m_serverMovementDestination, MovementType.Teleport, m_serverMovementPath, false);
				}
			}
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
				Log.Error("ClientUnresolvedDamage not cleared on TurnTick for " + GetDebugName());
				ClientUnresolvedDamage = 0;
			}
			if (ClientUnresolvedHealing != 0)
			{
				Log.Error("ClientUnresolvedHealing not cleared on TurnTick for " + GetDebugName());
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
				Log.Error("ClientUnresolvedAbsorb not cleared on TurnTick for " + GetDebugName());
				ClientUnresolvedAbsorb = 0;
			}
			ClientExpectedHoTTotalAdjust = 0;
			ClientAppliedHoTThisTurn = 0;
			SynchClientLastKnownPosToServerLastKnownPos();
			if (GetTeamSensitiveData() != null)
			{
				GetTeamSensitiveData().OnTurnTick();
			}
			PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
			if (localPlayerData != null && HighlightUtils.Get().m_recentlySpawnedShader != null)
			{
				int currentTurn = GameFlowData.Get().CurrentTurn;
				if (currentTurn == 1)
				{
					TricksterAfterImageNetworkBehaviour.InitializeAfterImageMaterial(GetActorModelData(), GameFlowData.Get().LocalPlayerData.GetTeamViewing() == GetTeam(), 0.5f, HighlightUtils.Get().m_recentlySpawnedShader, false);
				}
				else
				{
					if (currentTurn == 2)
					{
						goto IL_02e2;
					}
					if (currentTurn > 2)
					{
						if (currentTurn == NextRespawnTurn + 1)
						{
							goto IL_02e2;
						}
					}
				}
			}
		}
		goto IL_02ed;
		IL_02ed:
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
			if (!IsDead())
			{
				if (GameFlowData.Get() != null)
				{
					if (GameFlowData.Get().CurrentTurn > 1)
					{
						if (HighlightUtils.Get() != null)
						{
							if (HighlightUtils.Get().m_coverDirIndicatorTiming == HighlightUtils.MoveIntoCoverIndicatorTiming.ShowOnTurnStart)
							{
								if (HighlightUtils.Get().m_showMoveIntoCoverIndicators)
								{
									ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
									if (activeOwnedActorData != null && activeOwnedActorData == this)
									{
										if (IsVisibleToClient())
										{
											GetActorCover().StartShowMoveIntoCoverIndicator();
										}
									}
								}
							}
						}
					}
				}
			}
		}
		if (this.OnTurnStartDelegatesHolder == null)
		{
			return;
		}
		while (true)
		{
			this.OnTurnStartDelegatesHolder();
			return;
		}
		IL_02e2:
		TricksterAfterImageNetworkBehaviour.DisableAfterImageMaterial(GetActorModelData());
		goto IL_02ed;
	}

	public bool HasQueuedMovement()
	{
		int result;
		if (!m_queuedMovementRequest)
		{
			result = (m_queuedChaseRequest ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
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
		if (!(Quaternion.Angle(quaternion, m_targetRotation.GetEndValue()) > 0.01f))
		{
			return;
		}
		while (true)
		{
			base.transform.localRotation = m_targetRotation.GetEndValue();
			m_targetRotation.EaseTo(quaternion, 0.1f);
			return;
		}
	}

	public void TurnToPosition(Vector3 position, float turnDuration = 0.2f)
	{
		Vector3 vector = position - base.transform.position;
		vector.y = 0f;
		if (!(vector != Vector3.zero))
		{
			return;
		}
		base.transform.localRotation = m_targetRotation.GetEndValue();
		Quaternion quaternion = default(Quaternion);
		quaternion.SetLookRotation(vector);
		if (!(Quaternion.Angle(quaternion, m_targetRotation.GetEndValue()) > 0.01f))
		{
			return;
		}
		while (true)
		{
			m_targetRotation.EaseTo(quaternion, turnDuration);
			return;
		}
	}

	public float GetRotationTimeRemaining()
	{
		return m_targetRotation.CalcTimeRemaining();
	}

	public void TurnToPositionInstant(Vector3 position)
	{
		Vector3 vector = position - base.transform.position;
		vector.y = 0f;
		if (!(vector != Vector3.zero))
		{
			return;
		}
		while (true)
		{
			Quaternion quaternion = default(Quaternion);
			quaternion.SetLookRotation(vector);
			base.transform.localRotation = quaternion;
			m_targetRotation.SnapTo(quaternion);
			return;
		}
	}

	public Rigidbody GetRigidBody(string boneName)
	{
		Rigidbody result = null;
		GameObject gameObject = base.gameObject.FindInChildren(boneName);
		if ((bool)gameObject)
		{
			result = gameObject.GetComponentInChildren<Rigidbody>();
		}
		else
		{
			Log.Warning($"GetRigidBody trying to find body of bone {boneName} on actor '{DisplayName}' (obj name '{base.gameObject.name}'), but the bone cannot be found.");
		}
		return result;
	}

	public Rigidbody GetHipJointRigidBody()
	{
		if (m_cachedHipJoint == null)
		{
			m_cachedHipJoint = GetRigidBody("hip_JNT");
		}
		return m_cachedHipJoint;
	}

	public Vector3 GetHipJointRigidBodyPosition()
	{
		Rigidbody hipJointRigidBody = GetHipJointRigidBody();
		if (hipJointRigidBody != null)
		{
			return hipJointRigidBody.transform.position;
		}
		return base.gameObject.transform.position;
	}

	public Vector3 GetBonePosition(string boneName)
	{
		Vector3 zero = Vector3.zero;
		GameObject gameObject = base.gameObject.FindInChildren(boneName);
		if ((bool)gameObject)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return gameObject.transform.position;
				}
			}
		}
		return base.gameObject.transform.position;
	}

	public Quaternion GetBoneRotation(string boneName)
	{
		Quaternion identity = Quaternion.identity;
		GameObject gameObject = base.gameObject.FindInChildren(boneName);
		if ((bool)gameObject)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return gameObject.transform.rotation;
				}
			}
		}
		Log.Warning($"GetBoneRotation trying to find rotation of bone {boneName} on actor '{DisplayName}' (obj name '{base.gameObject.name}'), but the bone cannot be found.");
		return base.gameObject.transform.rotation;
	}

	public void OnDeselect()
	{
		if (GetActorController() != null)
		{
			GetActorController().ClearHighlights();
		}
		GetActorCover().UpdateCoverHighlights(null);
		RespawnPickedPositionSquare = RespawnPickedPositionSquare;
	}

	public void OnSelect()
	{
		m_callHandleOnSelectInUpdate = true;
	}

	private void HandleOnSelect()
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
		if (!(GetActorMovement() != null))
		{
			return;
		}
		while (true)
		{
			GetActorMovement().UpdateSquaresCanMoveTo();
			return;
		}
	}

	public void OnMovementChanged(MovementChangeType changeType, bool forceChased = false)
	{
	}

	public bool BeingTargetedByClientAbility(out bool inCover, out bool updatingInConfirm)
	{
		bool flag = false;
		inCover = false;
		updatingInConfirm = false;
		GameFlowData gameFlowData = GameFlowData.Get();
		ActorData activeOwnedActorData;
		ActorTurnSM actorTurnSM;
		if (gameFlowData != null)
		{
			if (gameFlowData.gameState == GameState.BothTeams_Decision)
			{
				if (gameFlowData.activeOwnedActorData != null)
				{
					activeOwnedActorData = gameFlowData.activeOwnedActorData;
					AbilityData abilityData = activeOwnedActorData.GetAbilityData();
					actorTurnSM = activeOwnedActorData.GetActorTurnSM();
					if (actorTurnSM.CurrentState != TurnStateEnum.TARGETING_ACTION)
					{
						if (actorTurnSM.CurrentState == TurnStateEnum.DECIDING && abilityData != null)
						{
							Ability lastSelectedAbility = abilityData.GetLastSelectedAbility();
							if (ShouldUpdateForConfirmedTargeting(lastSelectedAbility, actorTurnSM.GetAbilityTargets().Count))
							{
								flag = lastSelectedAbility.IsActorInTargetRange(this, out inCover);
								int num;
								if (lastSelectedAbility.IsSimpleAction())
								{
									num = 0;
								}
								else
								{
									num = actorTurnSM.GetAbilityTargets().Count - 1;
								}
								int num2 = num;
								if (num2 >= 0)
								{
									if (lastSelectedAbility.Targeters != null)
									{
										num2 = Mathf.Clamp(num2, 0, lastSelectedAbility.Targeters.Count - 1);
										UpdateNameplateForTargetingAbility(activeOwnedActorData, lastSelectedAbility, flag, inCover, num2, true);
										updatingInConfirm = true;
										if (HUD_UI.Get() != null)
										{
											if (activeOwnedActorData.ForceDisplayTargetHighlight)
											{
												HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.ShowTargetingNumberForConfirmedTargeting(this);
												m_showingTargetingNumAtFullAlpha = true;
											}
											else
											{
												if (m_wasUpdatingForConfirmedTargeting)
												{
													if (!m_showingTargetingNumAtFullAlpha)
													{
														goto IL_02a3;
													}
												}
												HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.StartTargetingNumberFadeout(this);
												m_showingTargetingNumAtFullAlpha = false;
											}
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
						goto IL_02a3;
					}
					Ability selectedAbility = abilityData.GetSelectedAbility();
					if (selectedAbility != null)
					{
						if (selectedAbility.Targeters != null)
						{
							flag = selectedAbility.IsActorInTargetRange(this, out inCover);
							int count = actorTurnSM.GetAbilityTargets().Count;
							count = Mathf.Clamp(count, 0, selectedAbility.Targeters.Count - 1);
							UpdateNameplateForTargetingAbility(activeOwnedActorData, selectedAbility, flag, inCover, count, false);
						}
					}
				}
			}
		}
		goto IL_0310;
		IL_0310:
		m_wasUpdatingForConfirmedTargeting = updatingInConfirm;
		return flag;
		IL_02a3:
		if (actorTurnSM.CurrentState == TurnStateEnum.DECIDING)
		{
			if (!activeOwnedActorData.ForceDisplayTargetHighlight)
			{
				if (!flag)
				{
					if (HUD_UI.Get() != null)
					{
						HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateNameplateUntargeted(this, !updatingInConfirm);
					}
				}
			}
		}
		goto IL_0310;
	}

	public void AddForceShowOutlineChecker(IForceActorOutlineChecker checker)
	{
		if (checker == null || m_forceShowOutlineCheckers.Contains(checker))
		{
			return;
		}
		while (true)
		{
			m_forceShowOutlineCheckers.Add(checker);
			return;
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
		if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
		{
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
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
					}
				}
			}
		}
		return false;
	}

	[Client]
	private void UpdateNameplateForTargetingAbility(ActorData targetingActor, Ability selectedAbility, bool targeted, bool inCover, int currentTargeterIndex, bool inConfirm)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[Client] function 'System.Void ActorData::UpdateNameplateForTargetingAbility(ActorData,Ability,System.Boolean,System.Boolean,System.Int32,System.Boolean)' called on server");
					return;
				}
			}
		}
		if (!(HUD_UI.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (this == targetingActor)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateSelfNameplate(this, selectedAbility, inCover, currentTargeterIndex, inConfirm);
						return;
					}
				}
			}
			if (targeted)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateNameplateTargeted(targetingActor, this, selectedAbility, inCover, currentTargeterIndex, inConfirm);
						return;
					}
				}
			}
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateNameplateUntargeted(this, true);
			return;
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
			while (true)
			{
				return false;
			}
		}
		int result;
		if (!ForceDisplayTargetHighlight)
		{
			if (lastSelectedAbility.Targeter != null)
			{
				if (lastSelectedAbility.Targeter.GetConfirmedTargetingRemainingTime() > 0f)
				{
					if (!lastSelectedAbility.IsSimpleAction())
					{
						result = ((numAbilityTargets > 0) ? 1 : 0);
					}
					else
					{
						result = 1;
					}
					goto IL_0092;
				}
			}
			result = 0;
		}
		else
		{
			result = 1;
		}
		goto IL_0092;
		IL_0092:
		return (byte)result != 0;
	}

	public static bool WouldSquareBeChasedByClient(BoardSquare square, bool IgnoreChosenChaseTarget = false)
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (!activeOwnedActorData.CanChase(square))
		{
			return false;
		}
		if (!activeOwnedActorData.HasQueuedMovement())
		{
			if (!activeOwnedActorData.HasQueuedChase())
			{
				return true;
			}
		}
		if (activeOwnedActorData.HasQueuedChase())
		{
			if (!IgnoreChosenChaseTarget && square == activeOwnedActorData.GetQueuedChaseTarget().GetCurrentBoardSquare())
			{
				return false;
			}
			return true;
		}
		if (!(square == activeOwnedActorData.MoveFromBoardSquare))
		{
			if (activeOwnedActorData.CanMoveToBoardSquare(square))
			{
				return false;
			}
		}
		return true;
	}

	public bool CanChase(BoardSquare square)
	{
		if (square == null || square.occupant == null || square.occupant.GetComponent<ActorData>() == null || GameFlowData.Get() == null)
		{
			return false;
		}

		if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
		{
			ActorData occupant = square.occupant.GetComponent<ActorData>();
			AbilityData actor = GetComponent<AbilityData>();
			if (occupant.IsDead())
			{
				return false;
			}
			if (occupant == this)
			{
				return false;
			}
			if (!occupant.IsVisibleForChase(this))
			{
				return false;
			}
			if (!actor.GetQueuedAbilitiesAllowMovement())
			{
				return false;
			}
			if (occupant.IgnoreForAbilityHits)
			{
				return false;
			}
			return true;
		}
		
		return false;
	}

	public void OnHitWhileInCover(Vector3 hitOrigin, ActorData caster)
	{
		if (IsDead())
		{
			return;
		}
		while (true)
		{
			if (m_actorVFX != null)
			{
				m_actorVFX.ShowHitWhileInCoverVfx(GetTravelBoardSquareWorldPosition(), hitOrigin, caster);
				AudioManager.PostEvent("ablty/generic/feedback/behind_cover_hit", base.gameObject);
			}
			return;
		}
	}

	public void OnKnockbackWhileUnstoppable(Vector3 hitOrigin, ActorData caster)
	{
		if (IsDead())
		{
			return;
		}
		while (true)
		{
			if (m_actorVFX != null)
			{
				while (true)
				{
					m_actorVFX.ShowKnockbackWhileUnstoppableVfx(GetTravelBoardSquareWorldPosition(), hitOrigin, caster);
					AudioManager.PostEvent("ablty/generic/feedback/unstoppable", base.gameObject);
					return;
				}
			}
			return;
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
		if (characterResourceLink != null)
		{
			if (!characterResourceLink.AllowAudioTag(audioTag, m_visualInfo))
			{
				return;
			}
		}
		PostAudioEvent(eventName);
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
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					AudioManager.PostEventNotify(text, action, notifyCallback, null, base.gameObject);
					return;
				}
			}
		}
		AudioManager.PostEvent(text, action, null, base.gameObject);
	}

	[Command]
	internal void CmdSetPausedForDebugging(bool pause)
	{
		if (!HydrogenConfig.Get().AllowDebugCommands)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		GameFlowData.Get().SetPausedForDebugging(pause);
	}

	[Command]
	internal void CmdSetResolutionSingleStepping(bool singleStepping)
	{
		if (!(GameFlowData.Get() != null))
		{
			return;
		}
		while (true)
		{
			GameFlowData.Get().SetResolutionSingleStepping(singleStepping);
			return;
		}
	}

	[Command]
	internal void CmdSetResolutionSingleSteppingAdvance()
	{
		if (!(GameFlowData.Get() != null))
		{
			return;
		}
		while (true)
		{
			GameFlowData.Get().SetResolutionSingleSteppingAdvance();
			return;
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
		if (NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			bool flag = IsDead();
			UnresolvedDamage = 0;
			UnresolvedHealing = 0;
			ClientUnresolvedDamage = 0;
			ClientUnresolvedHealing = 0;
			ClientUnresolvedAbsorb = 0;
			SetHitPoints(resolvedHitPoints);
			if (flag || !IsDead())
			{
				return;
			}
			while (true)
			{
				if (!IsModelAnimatorDisabled())
				{
					Debug.LogError("Actor " + GetDebugName() + " died on HP resolved; he should have already been ragdolled, but wasn't.");
					DoVisualDeath(new ActorModelData.ImpulseInfo(GetTravelBoardSquareWorldPositionForLos(), Vector3.up));
				}
				return;
			}
		}
	}

	[ClientRpc]
	internal void RpcCombatText(string combatText, string logText, CombatTextCategory category, BuffIconToDisplay icon)
	{
		AddCombatText(combatText, logText, category, icon);
	}

	internal void AddCombatText(string combatText, string logText, CombatTextCategory category, BuffIconToDisplay icon)
	{
		if (m_combatText == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Log.Error(base.gameObject.name + " does not have a combat text component.");
					return;
				}
			}
		}
		m_combatText.Add(combatText, logText, category, icon);
	}

	[Client]
	internal void ShowDamage(string combatText)
	{
		if (NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			Debug.LogWarning("[Client] function 'System.Void ActorData::ShowDamage(System.String)' called on server");
			return;
		}
	}

	[ClientRpc]
	internal void RpcApplyAbilityModById(int actionTypeInt, int abilityScopeId)
	{
		if (NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			if (NetworkClient.active && abilityScopeId >= 0)
			{
				while (true)
				{
					ApplyAbilityModById(actionTypeInt, abilityScopeId);
					return;
				}
			}
			return;
		}
	}

	internal void ApplyAbilityModById(int actionTypeInt, int abilityScopeId)
	{
		int num;
		if (GameManager.Get().GameConfig.GameType != GameType.Tutorial)
		{
			num = (AbilityModHelper.IsModAllowed(m_characterType, actionTypeInt, abilityScopeId) ? 1 : 0);
		}
		else
		{
			num = 1;
		}
		if (num == 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("Mod with ID " + abilityScopeId + " is not allowed on ability at index " + actionTypeInt + " for character " + m_characterType.ToString());
					return;
				}
			}
		}
		AbilityData component = GetComponent<AbilityData>();
		if (!(component != null))
		{
			return;
		}
		while (true)
		{
			Ability abilityOfActionType = component.GetAbilityOfActionType((AbilityData.ActionType)actionTypeInt);
			AbilityMod abilityModForAbilityById = AbilityModManager.Get().GetAbilityModForAbilityById(abilityOfActionType, abilityScopeId);
			if (!(abilityModForAbilityById != null))
			{
				return;
			}
			GameType gameType = GameManager.Get().GameConfig.GameType;
			GameSubType instanceSubType = GameManager.Get().GameConfig.InstanceSubType;
			if (abilityModForAbilityById.EquippableForGameType())
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						ApplyAbilityModToAbility(abilityOfActionType, abilityModForAbilityById);
						if (NetworkServer.active)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									CallRpcApplyAbilityModById(actionTypeInt, abilityScopeId);
									return;
								}
							}
						}
						return;
					}
				}
			}
			Log.Warning("Mod with ID " + abilityModForAbilityById.m_abilityScopeId + " is not allowed in game type: " + gameType.ToString() + ", subType: " + instanceSubType.LocalizedName);
			return;
		}
	}

	internal void no_op(int _001D, int _000E)
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
				Ability[] array = chainAbilities;
				foreach (Ability ability2 in array)
				{
					if (ability2 != null)
					{
						ability2.sprite = ability.sprite;
					}
				}
			}
		}
		if (!log)
		{
			return;
		}
		while (true)
		{
			Debug.LogWarning("Applied " + abilityMod.GetDebugIdentifier("white") + " to ability " + ability.GetDebugIdentifier("orange"));
			return;
		}
	}

	[ClientRpc]
	public void RpcMarkForRecalculateClientVisibility()
	{
		if (!(GetFogOfWar() != null))
		{
			return;
		}
		while (true)
		{
			GetFogOfWar().MarkForRecalculateVisibility();
			return;
		}
	}

	public void ShowRespawnFlare(BoardSquare flareSquare, bool respawningThisTurn)
	{
		bool flag = GameFlowData.Get().LocalPlayerData != null && GameFlowData.Get().LocalPlayerData.GetTeamViewing() == GetTeam();
		bool flag2 = false;
		if (m_respawnPositionFlare != null)
		{
			flag2 = (m_respawnFlareVfxSquare == flareSquare && m_respawnFlareForSameTeam == flag);
			UnityEngine.Object.Destroy(m_respawnPositionFlare);
			m_respawnPositionFlare = null;
			UICharacterMovementPanel.Get().RemoveRespawnIndicator(this);
			m_respawnFlareVfxSquare = null;
			m_respawnFlareForSameTeam = false;
		}
		if (SpawnPointManager.Get() != null)
		{
			if (!SpawnPointManager.Get().m_spawnInDuringMovement)
			{
				while (true)
				{
					switch (5)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
		if (!(flareSquare != null))
		{
			return;
		}
		while (true)
		{
			GameObject original;
			if (!flag)
			{
				original = ((!respawningThisTurn) ? HighlightUtils.Get().m_respawnPositionFlareEnemyVFXPrefab : HighlightUtils.Get().m_respawnPositionFinalEnemyVFXPrefab);
			}
			else if (respawningThisTurn)
			{
				original = HighlightUtils.Get().m_respawnPositionFinalFriendlyVFXPrefab;
			}
			else
			{
				original = HighlightUtils.Get().m_respawnPositionFlareFriendlyVFXPrefab;
			}
			m_respawnPositionFlare = UnityEngine.Object.Instantiate(original);
			m_respawnFlareVfxSquare = flareSquare;
			m_respawnFlareForSameTeam = flag;
			if (!flag2 && this == GameFlowData.Get().activeOwnedActorData)
			{
				UISounds.GetUISounds().Play("ui/ingame/v1/respawn_locator");
			}
			m_respawnPositionFlare.transform.position = flareSquare.ToVector3();
			UICharacterMovementPanel.Get().AddRespawnIndicator(flareSquare, this);
			return;
		}
	}

	[ClientRpc]
	public void RpcForceLeaveGame(GameResult gameResult)
	{
		if (!(GameFlowData.Get().activeOwnedActorData == this))
		{
			return;
		}
		while (true)
		{
			if (!ClientGameManager.Get().IsFastForward)
			{
				while (true)
				{
					ClientGameManager.Get().LeaveGame(false, gameResult);
					return;
				}
			}
			return;
		}
	}

	public void SendPingRequestToServer(int teamIndex, Vector3 worldPosition, ActorController.PingType pingType)
	{
		if (!(GetActorController() != null))
		{
			return;
		}
		while (true)
		{
			GetActorController().CallCmdSendMinimapPing(teamIndex, worldPosition, pingType);
			return;
		}
	}

	public void SendAbilityPingRequestToServer(int teamIndex, LocalizationArg_AbilityPing localizedPing)
	{
		if (!(GetActorController() != null))
		{
			return;
		}
		while (true)
		{
			GetActorController().CallCmdSendAbilityPing(teamIndex, localizedPing);
			return;
		}
	}

	public override string ToString()
	{
		return $"[ActorData: {m_displayName}, {GetObjectName()}, ActorIndex: {m_actorIndex}, {m_team}] {PlayerData}";
	}

	public string GetDebugName()
	{
		return "[" + GetObjectName() + " (" + DisplayName + "), " + ActorIndex + "]";
	}

	public string GetColoredDebugName(string color)
	{
		return "<color=" + color + ">" + GetDebugName() + "</color>";
	}

	public string GetPointsDebugString()
	{
		int num = ExpectedHoTTotal + ClientExpectedHoTTotalAdjust;
		int expectedHoTThisTurn = ExpectedHoTThisTurn;
		int clientAppliedHoTThisTurn = ClientAppliedHoTThisTurn;
		return "Max HP: " + GetMaxHitPoints() + "\nHP to Display: " + GetHitPointsToDisplay() + "\n HP: " + HitPoints + "\n Damage: " + UnresolvedDamage + "\n Healing: " + UnresolvedHealing + "\n Absorb: " + AbsorbPoints + "\n CL Damage: " + ClientUnresolvedDamage + "\n CL Healing: " + ClientUnresolvedHealing + "\n CL Absorb: " + ClientUnresolvedAbsorb + "\n\n Energy to Display: " + GetEnergyToDisplay() + "\n  Energy: " + TechPoints + "\n Reserved Energy: " + ReservedTechPoints + "\n EnergyGain: " + UnresolvedTechPointGain + "\n EnergyLoss: " + UnresolvedTechPointLoss + "\n CL Reserved Energy: " + ClientReservedTechPoints + "\n CL EnergyGain: " + ClientUnresolvedTechPointGain + "\n CL EnergyLoss: " + ClientUnresolvedTechPointLoss + "\n CL Total HoT: " + num + "\n CL HoT This Turn/Applied: " + expectedHoTThisTurn + " / " + clientAppliedHoTThisTurn;
	}

	public string GetActorTurnSMDebugString()
	{
		string text = string.Empty;
		if (GetActorTurnSM() != null)
		{
			string text2 = text;
			text = string.Concat(text2, "ActorTurnSM: CurrentState= ", GetActorTurnSM().CurrentState, " | PrevState= ", GetActorTurnSM().PreviousState, "\n");
		}
		return text;
	}

	public ActorData[] AsArray()
	{
		return new ActorData[1]
		{
			this
		};
	}

	public List<ActorData> AsList()
	{
		List<ActorData> list = new List<ActorData>();
		list.Add(this);
		return list;
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		Gizmos.color = Color.green;
		if (!(GetCurrentBoardSquare() != null))
		{
			return;
		}
		while (true)
		{
			Gizmos.DrawWireCube(GetCurrentBoardSquare().CameraBounds.center, GetCurrentBoardSquare().CameraBounds.size * 0.9f);
			Gizmos.DrawRay(GetCurrentBoardSquare().ToVector3(), base.transform.forward);
			return;
		}
	}

	public bool HasTag(string tag)
	{
		if (m_actorTags != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return m_actorTags.HasTag(tag);
				}
			}
		}
		return false;
	}

	public void AddTag(string tag)
	{
		if (m_actorTags == null)
		{
			m_actorTags = base.gameObject.AddComponent<ActorTag>();
		}
		m_actorTags.AddTag(tag);
	}

	public void RemoveTag(string tag)
	{
		if (!(m_actorTags != null))
		{
			return;
		}
		while (true)
		{
			m_actorTags.RemoveTag(tag);
			return;
		}
	}

	public CharacterResourceLink GetCharacterResourceLink()
	{
		if (m_characterResourceLink == null)
		{
			if (m_characterType != 0)
			{
				GameWideData gameWideData = GameWideData.Get();
				if ((bool)gameWideData)
				{
					m_characterResourceLink = gameWideData.GetCharacterResourceLink(m_characterType);
				}
			}
		}
		return m_characterResourceLink;
	}

	public GameObject ReplaceSequence(GameObject originalSequencePrefab)
	{
		if (originalSequencePrefab == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		CharacterResourceLink characterResourceLink = GetCharacterResourceLink();
		if (characterResourceLink == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return originalSequencePrefab;
				}
			}
		}
		return characterResourceLink.ReplaceSequence(originalSequencePrefab, m_visualInfo, m_abilityVfxSwapInfo);
	}

	public void OnAnimEvent(UnityEngine.Object eventObject, GameObject sourceObject)
	{
		if (this.OnAnimationEventDelegatesHolder == null)
		{
			return;
		}
		while (true)
		{
			this.OnAnimationEventDelegatesHolder(eventObject, sourceObject);
			return;
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.GametimeScaleChange)
		{
			return;
		}
		while (true)
		{
			Animator modelAnimator = GetModelAnimator();
			if (modelAnimator != null)
			{
				while (true)
				{
					modelAnimator.speed = GameTime.scale;
					return;
				}
			}
			return;
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeCmdCmdSetPausedForDebugging(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command CmdSetPausedForDebugging called on client.");
					return;
				}
			}
		}
		((ActorData)obj).CmdSetPausedForDebugging(reader.ReadBoolean());
	}

	protected static void InvokeCmdCmdSetResolutionSingleStepping(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command CmdSetResolutionSingleStepping called on client.");
					return;
				}
			}
		}
		((ActorData)obj).CmdSetResolutionSingleStepping(reader.ReadBoolean());
	}

	protected static void InvokeCmdCmdSetResolutionSingleSteppingAdvance(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command CmdSetResolutionSingleSteppingAdvance called on client.");
					return;
				}
			}
		}
		((ActorData)obj).CmdSetResolutionSingleSteppingAdvance();
	}

	protected static void InvokeCmdCmdSetDebugToggleParam(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command CmdSetDebugToggleParam called on client.");
					return;
				}
			}
		}
		((ActorData)obj).CmdSetDebugToggleParam(reader.ReadString(), reader.ReadBoolean());
	}

	protected static void InvokeCmdCmdDebugReslotCards(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command CmdDebugReslotCards called on client.");
					return;
				}
			}
		}
		((ActorData)obj).CmdDebugReslotCards(reader.ReadBoolean(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdDebugSetAbilityMod(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDebugSetAbilityMod called on client.");
		}
		else
		{
			((ActorData)obj).CmdDebugSetAbilityMod((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
		}
	}

	protected static void InvokeCmdCmdDebugReplaceWithBot(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command CmdDebugReplaceWithBot called on client.");
					return;
				}
			}
		}
		((ActorData)obj).CmdDebugReplaceWithBot();
	}

	protected static void InvokeCmdCmdDebugSetHealthOrEnergy(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDebugSetHealthOrEnergy called on client.");
		}
		else
		{
			((ActorData)obj).CmdDebugSetHealthOrEnergy((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
		}
	}

	public void CallCmdSetPausedForDebugging(bool pause)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdSetPausedForDebugging called on server.");
			return;
		}
		if (base.isServer)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					CmdSetPausedForDebugging(pause);
					return;
				}
			}
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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command function CmdSetResolutionSingleStepping called on server.");
					return;
				}
			}
		}
		if (base.isServer)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					CmdSetResolutionSingleStepping(singleStepping);
					return;
				}
			}
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
		if (base.isServer)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					CmdSetResolutionSingleSteppingAdvance();
					return;
				}
			}
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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command function CmdSetDebugToggleParam called on server.");
					return;
				}
			}
		}
		if (base.isServer)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					CmdSetDebugToggleParam(name, value);
					return;
				}
			}
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command function CmdDebugReslotCards called on server.");
					return;
				}
			}
		}
		if (base.isServer)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					CmdDebugReslotCards(reslotAll, cardTypeInt);
					return;
				}
			}
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
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command function CmdDebugSetAbilityMod called on server.");
					return;
				}
			}
		}
		if (base.isServer)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					CmdDebugSetAbilityMod(abilityIndex, modId);
					return;
				}
			}
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
		if (base.isServer)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					CmdDebugReplaceWithBot();
					return;
				}
			}
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
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command function CmdDebugSetHealthOrEnergy called on server.");
					return;
				}
			}
		}
		if (base.isServer)
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
		}
		else
		{
			((ActorData)obj).RpcOnHitPointsResolved((int)reader.ReadPackedUInt32());
		}
	}

	protected static void InvokeRpcRpcCombatText(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcCombatText called on server.");
		}
		else
		{
			((ActorData)obj).RpcCombatText(reader.ReadString(), reader.ReadString(), (CombatTextCategory)reader.ReadInt32(), (BuffIconToDisplay)reader.ReadInt32());
		}
	}

	protected static void InvokeRpcRpcApplyAbilityModById(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC RpcApplyAbilityModById called on server.");
					return;
				}
			}
		}
		((ActorData)obj).RpcApplyAbilityModById((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeRpcRpcMarkForRecalculateClientVisibility(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcMarkForRecalculateClientVisibility called on server.");
		}
		else
		{
			((ActorData)obj).RpcMarkForRecalculateClientVisibility();
		}
	}

	protected static void InvokeRpcRpcForceLeaveGame(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcForceLeaveGame called on server.");
		}
		else
		{
			((ActorData)obj).RpcForceLeaveGame((GameResult)reader.ReadInt32());
		}
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
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC Function RpcCombatText called on client.");
					return;
				}
			}
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
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC Function RpcMarkForRecalculateClientVisibility called on client.");
					return;
				}
			}
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
