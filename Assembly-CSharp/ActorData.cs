using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Fabric;
using UnityEngine;
using UnityEngine.Networking;

public class ActorData : NetworkBehaviour, IGameEventListener
{
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

	public ActorData.ActorDataDelegate m_onResolvedHitPoints;

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

	private int m_actorIndex = ActorData.s_invalidActorIndex;

	private bool m_showInGameHud = true;

	[Header("-- Stats --")]
	public int m_maxHitPoints = 0x64;

	public int m_hitPointRegen;

	[Space(5f)]
	public int m_maxTechPoints = 0x64;

	public int m_techPointRegen = 0xA;

	public int m_techPointsOnSpawn = 0x64;

	public int m_techPointsOnRespawn = 0x64;

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

	[CompilerGenerated]
	private static Action<GameState> <>f__mg$cache0;

	[CompilerGenerated]
	private static Action<GameState> <>f__mg$cache1;

	[CompilerGenerated]
	private static Action<GameState> <>f__mg$cache2;

	private static int kCmdCmdSetPausedForDebugging = 0x5FAF1856;

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

	public ActorData()
	{
		this.OnTurnStartDelegates = delegate()
		{
		};
		this.OnAnimationEventDelegates = delegate(UnityEngine.Object A_0, GameObject A_1)
		{
		};
		if (ActorData.<>f__am$cache2 == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData..ctor()).MethodHandle;
			}
			ActorData.<>f__am$cache2 = delegate(Ability A_0)
			{
			};
		}
		this.OnSelectedAbilityChangedDelegates = ActorData.<>f__am$cache2;
		if (ActorData.<>f__am$cache3 == null)
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
			ActorData.<>f__am$cache3 = delegate()
			{
			};
		}
		this.OnClientQueuedActionChangedDelegates = ActorData.<>f__am$cache3;
		this.m_serializeHelper = new SerializeHelper();
		this.m_forceShowOutlineCheckers = new List<IForceActorOutlineChecker>();
		base..ctor();
	}

	static ActorData()
	{
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), ActorData.kCmdCmdSetPausedForDebugging, new NetworkBehaviour.CmdDelegate(ActorData.InvokeCmdCmdSetPausedForDebugging));
		ActorData.kCmdCmdSetResolutionSingleStepping = -0x4DE5AFEB;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), ActorData.kCmdCmdSetResolutionSingleStepping, new NetworkBehaviour.CmdDelegate(ActorData.InvokeCmdCmdSetResolutionSingleStepping));
		ActorData.kCmdCmdSetResolutionSingleSteppingAdvance = -0x60156153;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), ActorData.kCmdCmdSetResolutionSingleSteppingAdvance, new NetworkBehaviour.CmdDelegate(ActorData.InvokeCmdCmdSetResolutionSingleSteppingAdvance));
		ActorData.kCmdCmdSetDebugToggleParam = -0x4A01617;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), ActorData.kCmdCmdSetDebugToggleParam, new NetworkBehaviour.CmdDelegate(ActorData.InvokeCmdCmdSetDebugToggleParam));
		ActorData.kCmdCmdDebugReslotCards = -0x39B179A2;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), ActorData.kCmdCmdDebugReslotCards, new NetworkBehaviour.CmdDelegate(ActorData.InvokeCmdCmdDebugReslotCards));
		ActorData.kCmdCmdDebugSetAbilityMod = 0x705D0BA6;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), ActorData.kCmdCmdDebugSetAbilityMod, new NetworkBehaviour.CmdDelegate(ActorData.InvokeCmdCmdDebugSetAbilityMod));
		ActorData.kCmdCmdDebugReplaceWithBot = -0x733284DF;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), ActorData.kCmdCmdDebugReplaceWithBot, new NetworkBehaviour.CmdDelegate(ActorData.InvokeCmdCmdDebugReplaceWithBot));
		ActorData.kCmdCmdDebugSetHealthOrEnergy = 0x386813F5;
		NetworkBehaviour.RegisterCommandDelegate(typeof(ActorData), ActorData.kCmdCmdDebugSetHealthOrEnergy, new NetworkBehaviour.CmdDelegate(ActorData.InvokeCmdCmdDebugSetHealthOrEnergy));
		ActorData.kRpcRpcOnHitPointsResolved = 0xB50A4DA;
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorData), ActorData.kRpcRpcOnHitPointsResolved, new NetworkBehaviour.CmdDelegate(ActorData.InvokeRpcRpcOnHitPointsResolved));
		ActorData.kRpcRpcCombatText = -0x6EE006AA;
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorData), ActorData.kRpcRpcCombatText, new NetworkBehaviour.CmdDelegate(ActorData.InvokeRpcRpcCombatText));
		ActorData.kRpcRpcApplyAbilityModById = -0x416FB6FD;
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorData), ActorData.kRpcRpcApplyAbilityModById, new NetworkBehaviour.CmdDelegate(ActorData.InvokeRpcRpcApplyAbilityModById));
		ActorData.kRpcRpcMarkForRecalculateClientVisibility = -0x29D39257;
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorData), ActorData.kRpcRpcMarkForRecalculateClientVisibility, new NetworkBehaviour.CmdDelegate(ActorData.InvokeRpcRpcMarkForRecalculateClientVisibility));
		ActorData.kRpcRpcForceLeaveGame = -0x471E2ECD;
		NetworkBehaviour.RegisterRpcDelegate(typeof(ActorData), ActorData.kRpcRpcForceLeaveGame, new NetworkBehaviour.CmdDelegate(ActorData.InvokeRpcRpcForceLeaveGame));
		NetworkCRC.RegisterBehaviour("ActorData", 0);
	}

	internal static int Layer { get; private set; }

	internal static int Layer_Mask { get; private set; }

	public bool ForceDisplayTargetHighlight { get; set; }

	internal Vector3 PreviousBoardSquarePosition { get; private set; }

	public BoardSquare ClientLastKnownPosSquare
	{
		get
		{
			return this.m_clientLastKnownPosSquare;
		}
		private set
		{
			if (this.m_clientLastKnownPosSquare != value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.set_ClientLastKnownPosSquare(BoardSquare)).MethodHandle;
				}
				if (ActorDebugUtils.Get() != null)
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
					if (ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.LastKnownPosition, false))
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
						string[] array = new string[5];
						array[0] = this.\u0018();
						array[1] = "----Setting ClientLastKnownPosSquare from ";
						array[2] = ((!this.m_clientLastKnownPosSquare) ? "null" : this.m_clientLastKnownPosSquare.ToString());
						array[3] = " to ";
						int num = 4;
						string text;
						if (value)
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
							text = value.ToString();
						}
						else
						{
							text = "null";
						}
						array[num] = text;
						UnityEngine.Debug.LogWarning(string.Concat(array));
					}
				}
				this.m_clientLastKnownPosSquare = value;
			}
			this.m_shouldUpdateLastVisibleToClientThisFrame = false;
		}
	}

	public BoardSquare ServerLastKnownPosSquare
	{
		get
		{
			return this.m_serverLastKnownPosSquare;
		}
		set
		{
			if (this.m_serverLastKnownPosSquare != value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.set_ServerLastKnownPosSquare(BoardSquare)).MethodHandle;
				}
				if (ActorDebugUtils.Get() != null && ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.LastKnownPosition, false))
				{
					string[] array = new string[5];
					array[0] = this.\u0018();
					array[1] = "=====ServerLastKnownPosSquare from ";
					int num = 2;
					string text;
					if (this.m_serverLastKnownPosSquare)
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
						text = this.m_serverLastKnownPosSquare.ToString();
					}
					else
					{
						text = "null";
					}
					array[num] = text;
					array[3] = " to ";
					array[4] = ((!value) ? "null" : value.ToString());
					UnityEngine.Debug.LogWarning(string.Concat(array));
				}
				this.m_serverLastKnownPosSquare = value;
			}
		}
	}

	public int \u000E()
	{
		return this.m_lastVisibleTurnToClient;
	}

	public Vector3 \u000E()
	{
		if (this.ClientLastKnownPosSquare)
		{
			return this.ClientLastKnownPosSquare.transform.position;
		}
		return Vector3.zero;
	}

	public Vector3 \u0012()
	{
		if (this.ServerLastKnownPosSquare)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0012()).MethodHandle;
			}
			return this.ServerLastKnownPosSquare.transform.position;
		}
		return Vector3.zero;
	}

	public void ActorData_OnActorMoved(BoardSquare movementSquare, bool visibleToEnemies, bool updateLastKnownPos)
	{
		if (NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.ActorData_OnActorMoved(BoardSquare, bool, bool)).MethodHandle;
			}
			if (updateLastKnownPos)
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
				this.ClientLastKnownPosSquare = movementSquare;
				this.m_lastVisibleTurnToClient = GameFlowData.Get().CurrentTurn;
			}
			this.m_shouldUpdateLastVisibleToClientThisFrame = false;
		}
	}

	public ActorBehavior \u000E()
	{
		return this.m_actorBehavior;
	}

	public ActorModelData \u000E()
	{
		return this.m_actorModelData;
	}

	internal ActorModelData \u0012()
	{
		return this.m_faceActorModelData;
	}

	public Renderer \u000E()
	{
		return this.m_actorModelData.GetModelRenderer(0);
	}

	public void EnableRendererAndUpdateVisibility()
	{
		if (this.m_actorModelData != null)
		{
			this.m_actorModelData.EnableRendererAndUpdateVisibility();
		}
	}

	internal ItemData \u000E()
	{
		return this.m_itemData;
	}

	internal AbilityData \u000E()
	{
		return this.m_abilityData;
	}

	internal ActorMovement \u000E()
	{
		return this.m_actorMovement;
	}

	internal ActorStats \u000E()
	{
		return this.m_actorStats;
	}

	internal ActorStatus \u000E()
	{
		return this.m_actorStatus;
	}

	internal ActorController \u000E()
	{
		return base.GetComponent<ActorController>();
	}

	internal ActorTargeting \u000E()
	{
		return this.m_actorTargeting;
	}

	internal FreelancerStats \u000E()
	{
		return this.m_freelancerStats;
	}

	internal NPCBrain \u000E()
	{
		ActorController x = this.\u000E();
		if (x != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u000E()).MethodHandle;
			}
			NPCBrain[] components = base.GetComponents<NPCBrain>();
			foreach (NPCBrain npcbrain in components)
			{
				if (npcbrain.enabled)
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
					return npcbrain;
				}
			}
		}
		return null;
	}

	internal ActorTurnSM \u000E()
	{
		return this.m_actorTurnSM;
	}

	internal ActorCover \u000E()
	{
		return this.m_actorCover;
	}

	internal ActorVFX \u000E()
	{
		return this.m_actorVFX;
	}

	internal TimeBank \u000E()
	{
		return this.m_timeBank;
	}

	internal FogOfWar \u000E()
	{
		return this.PlayerData.GetFogOfWar();
	}

	internal ActorAdditionalVisionProviders \u000E()
	{
		return this.m_additionalVisionProvider;
	}

	internal PassiveData \u000E()
	{
		return this.m_passiveData;
	}

	internal string DisplayName
	{
		get
		{
			return this.m_displayName;
		}
	}

	public string \u000E()
	{
		if (this.HasBotController)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u000E()).MethodHandle;
			}
			if (this.\u0012() == 0L)
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
				if (this.m_characterType != CharacterType.None)
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
					if (!this.\u000E().m_botsMasqueradeAsHumans)
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
						return StringUtil.TR_CharacterName(this.m_characterType.ToString());
					}
				}
			}
		}
		if (this.m_displayName == "FT")
		{
			TricksterAfterImageNetworkBehaviour[] componentsInChildren = GameFlowData.Get().GetActorRoot().GetComponentsInChildren<TricksterAfterImageNetworkBehaviour>();
			if (componentsInChildren != null)
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
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					for (int j = 0; j < componentsInChildren[i].m_afterImages.Count; j++)
					{
						int actorIndex = componentsInChildren[i].m_afterImages[j];
						ActorData x = GameFlowData.Get().FindActorByActorIndex(actorIndex);
						if (x == this)
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
							ActorData component = componentsInChildren[i].GetComponent<ActorData>();
							if (component.m_displayName != "FT")
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
								return component.\u000E();
							}
						}
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
		if (CollectTheCoins.Get() != null)
		{
			return string.Format("{0} ({1}c)", this.m_displayName, CollectTheCoins.Get().GetCoinsForActor_Client(this));
		}
		return this.m_displayName;
	}

	public void UpdateDisplayName(string newDisplayName)
	{
		this.m_displayName = newDisplayName;
	}

	public Sprite \u000E()
	{
		return (Sprite)Resources.Load(this.m_aliveHUDIconResourceString, typeof(Sprite));
	}

	public Sprite \u0012()
	{
		return (Sprite)Resources.Load(this.m_deadHUDIconResourceString, typeof(Sprite));
	}

	public Sprite \u0015()
	{
		return (Sprite)Resources.Load(this.m_screenIndicatorIconResourceString, typeof(Sprite));
	}

	public Sprite \u0016()
	{
		return (Sprite)Resources.Load(this.m_screenIndicatorBWIconResourceString, typeof(Sprite));
	}

	public string \u0012()
	{
		return base.name.Replace("(Clone)", string.Empty);
	}

	public int ActorIndex
	{
		get
		{
			return this.m_actorIndex;
		}
		set
		{
			if (this.m_actorIndex != value)
			{
				this.m_actorIndex = value;
			}
		}
	}

	public bool ShowInGameGUI
	{
		get
		{
			return this.m_showInGameHud;
		}
		set
		{
			this.m_showInGameHud = value;
		}
	}

	public float \u000E()
	{
		return this.m_maxHorizontalMovement - this.m_postAbilityHorizontalMovement;
	}

	public int \u0012()
	{
		int result = 1;
		ActorStats actorStats = this.m_actorStats;
		if (actorStats != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0012()).MethodHandle;
			}
			result = actorStats.GetModifiedStatInt(StatType.MaxHitPoints);
		}
		return result;
	}

	public void OnMaxHitPointsChanged(int previousMax)
	{
		if (this.\u000E())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.OnMaxHitPointsChanged(int)).MethodHandle;
			}
			return;
		}
		float num = (float)this.HitPoints / (float)previousMax;
		int num2 = this.\u0012();
		this.HitPoints = Mathf.RoundToInt((float)num2 * num);
	}

	public float \u0012()
	{
		int num = this.\u0012();
		int hitPoints = this.HitPoints;
		return (float)hitPoints / (float)num;
	}

	public int \u0015()
	{
		int num = 0;
		ActorStats actorStats = this.m_actorStats;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0015()).MethodHandle;
			}
			num = Mathf.RoundToInt((float)num * GameplayMutators.GetPassiveHpRegenMultiplier());
		}
		return num;
	}

	public int \u0016()
	{
		int result = 1;
		ActorStats actorStats = this.m_actorStats;
		if (actorStats != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0016()).MethodHandle;
			}
			result = actorStats.GetModifiedStatInt(StatType.MaxTechPoints);
		}
		return result;
	}

	public void OnMaxTechPointsChanged(int previousMax)
	{
		if (this.\u000E())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.OnMaxTechPointsChanged(int)).MethodHandle;
			}
			return;
		}
		int num = this.\u0016();
		if (num > previousMax)
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
			int num2 = num - previousMax;
			this.TechPoints += num2;
		}
		else if (previousMax > num)
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
			int techPoints = this.TechPoints;
			this.TechPoints = Mathf.Min(this.TechPoints, num);
			if (techPoints - this.TechPoints != 0)
			{
			}
		}
	}

	public void TriggerVisibilityForHit(bool movementHit, bool updateClientLastKnownPos = true)
	{
		if (movementHit)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.TriggerVisibilityForHit(bool, bool)).MethodHandle;
			}
			this.m_endVisibilityForHitTime = Time.time + ActorData.s_visibleTimeAfterMovementHit;
		}
		else
		{
			this.m_endVisibilityForHitTime = Time.time + ActorData.s_visibleTimeAfterHit;
		}
		this.ForceUpdateIsVisibleToClientCache();
		if (updateClientLastKnownPos)
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
			if (this.\u0012() != null)
			{
				this.ClientLastKnownPosSquare = this.\u000E();
				this.m_lastVisibleTurnToClient = GameFlowData.Get().CurrentTurn;
			}
		}
	}

	private void UpdateClientLastKnownPosSquare()
	{
		if (this.m_shouldUpdateLastVisibleToClientThisFrame)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.UpdateClientLastKnownPosSquare()).MethodHandle;
			}
			if (this.ClientLastKnownPosSquare != this.\u0012())
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
				Team team;
				if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData != null)
				{
					team = GameFlowData.Get().activeOwnedActorData.\u000E();
				}
				else
				{
					team = Team.Invalid;
				}
				bool flag;
				if (GameFlowData.Get() != null)
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
					flag = GameFlowData.Get().IsInResolveState();
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				bool flag3 = this.\u000E() == this.\u0012() && !this.\u000E().AmMoving() && !this.\u000E().IsYetToCompleteGameplayPath();
				bool flag4 = this.\u000E() != team;
				if (flag2)
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
					if (flag3)
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
						if (flag4 && this.\u0018())
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
							this.ForceUpdateIsVisibleToClientCache();
							if (this.\u0018())
							{
								this.ClientLastKnownPosSquare = this.\u0012();
								this.m_lastVisibleTurnToClient = GameFlowData.Get().CurrentTurn;
							}
						}
					}
				}
			}
		}
		this.m_shouldUpdateLastVisibleToClientThisFrame = true;
	}

	public Player \u000E()
	{
		return this.PlayerData.GetPlayer();
	}

	public PlayerDetails \u000E()
	{
		if (this.PlayerData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u000E()).MethodHandle;
			}
			if (GameFlow.Get() != null)
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
				if (GameFlow.Get().playerDetails != null && GameFlow.Get().playerDetails.ContainsKey(this.PlayerData.GetPlayer()))
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
					return GameFlow.Get().playerDetails[this.PlayerData.GetPlayer()];
				}
			}
		}
		return null;
	}

	public void SetupAbilityModOnReconnect()
	{
		List<ActorData> actors = GameFlowData.Get().GetActors();
		foreach (ActorData actorData in actors)
		{
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.SetupAbilityModOnReconnect()).MethodHandle;
				}
				if (actorData.\u000E() != null)
				{
					for (int i = 0; i <= 4; i++)
					{
						actorData.ApplyAbilityModById(i, actorData.m_selectedMods.GetModForAbility(i));
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
					ActorTargeting component = actorData.GetComponent<ActorTargeting>();
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
						component.MarkForForceRedraw();
					}
				}
			}
		}
	}

	public void SetupForRespawnOnReconnect()
	{
		if (this.\u0015())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.SetupForRespawnOnReconnect()).MethodHandle;
			}
			if (GameFlowData.Get() != null)
			{
				if (ServerClientUtils.GetCurrentActionPhase() >= ActionBufferPhase.Movement)
				{
					if (ServerClientUtils.GetCurrentActionPhase() <= ActionBufferPhase.MovementWait)
					{
						return;
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
				ActorModelData actorModelData = this.\u000E();
				if (actorModelData != null)
				{
					actorModelData.DisableAndHideRenderers();
				}
				if (HighlightUtils.Get().m_recentlySpawnedShader != null)
				{
					TricksterAfterImageNetworkBehaviour.InitializeAfterImageMaterial(actorModelData, GameFlowData.Get().LocalPlayerData.GetTeamViewing() == this.\u000E(), 0.5f, HighlightUtils.Get().m_recentlySpawnedShader, false);
				}
				if (this.RespawnPickedPositionSquare != null)
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
					this.ShowRespawnFlare(this.RespawnPickedPositionSquare, true);
				}
			}
		}
	}

	public void SetupAbilityMods(CharacterModInfo characterMods)
	{
		this.m_selectedMods = characterMods;
		AbilityData abilityData = this.\u000E();
		List<Ability> abilitiesAsList = abilityData.GetAbilitiesAsList();
		int num = 0;
		using (List<Ability>.Enumerator enumerator = abilitiesAsList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Ability ability = enumerator.Current;
				int num3;
				if (GameManager.Get().GameConfig.GameType == GameType.Tutorial)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.SetupAbilityMods(CharacterModInfo)).MethodHandle;
					}
					AbilityMod defaultModForAbility = AbilityModManager.Get().GetDefaultModForAbility(ability);
					int num2;
					if (defaultModForAbility != null)
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
						num2 = defaultModForAbility.m_abilityScopeId;
					}
					else
					{
						num2 = -1;
					}
					num3 = num2;
				}
				else
				{
					num3 = this.m_selectedMods.GetModForAbility(num);
				}
				AbilityData.ActionType actionTypeOfAbility = abilityData.GetActionTypeOfAbility(ability);
				if (actionTypeOfAbility != AbilityData.ActionType.INVALID_ACTION)
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
					if (num3 > 0)
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
						this.ApplyAbilityModById((int)actionTypeOfAbility, num3);
					}
				}
				num++;
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

	public void UpdateServerLastVisibleTurn()
	{
	}

	public void SynchClientLastKnownPosToServerLastKnownPos()
	{
		if (NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.SynchClientLastKnownPosToServerLastKnownPos()).MethodHandle;
			}
			if (this.ClientLastKnownPosSquare != this.ServerLastKnownPosSquare)
			{
				this.ClientLastKnownPosSquare = this.ServerLastKnownPosSquare;
				if (GameFlowData.Get().activeOwnedActorData != null)
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
					if (this.\u000E() != GameFlowData.Get().activeOwnedActorData.\u000E())
					{
					}
				}
			}
		}
	}

	public int \u0013()
	{
		int num = 0;
		ActorStats actorStats = this.m_actorStats;
		if (actorStats != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0013()).MethodHandle;
			}
			num = actorStats.GetModifiedStatInt(StatType.TechPointRegen);
		}
		if (GameplayMutators.Get() != null)
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
			num = Mathf.RoundToInt((float)num * GameplayMutators.GetPassiveEnergyRegenMultiplier());
		}
		return num;
	}

	public float \u0015()
	{
		float result = 1f;
		if (this.m_actorStats != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0015()).MethodHandle;
			}
			result = this.m_actorStats.GetModifiedStatFloat(StatType.SightRange);
		}
		if (this.m_actorStatus != null)
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
			if (this.m_actorStatus.HasStatus(StatusType.Blind, true))
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
				result = 0.1f;
			}
		}
		return result;
	}

	internal int HitPoints
	{
		get
		{
			return this.m_hitPoints;
		}
		private set
		{
			if (MatchLogger.Get() != null)
			{
				MatchLogger.Get().Log(string.Concat(new object[]
				{
					this,
					" HitPoints.set ",
					value,
					", old: ",
					this.HitPoints
				}));
			}
			bool flag = this.m_hitPoints > 0;
			if (NetworkServer.active)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.set_HitPoints(int)).MethodHandle;
				}
				this.m_hitPoints = Mathf.Clamp(value, 0, this.\u0012());
			}
			else
			{
				this.m_hitPoints = value;
			}
			int num = 0;
			if (GameFlowData.Get() != null)
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
				num = GameFlowData.Get().CurrentTurn;
			}
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
				if (this.m_hitPoints == 0)
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
					if (GameFlowData.Get() != null)
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
						this.LastDeathTurn = GameFlowData.Get().CurrentTurn;
					}
					this.LastDeathPosition = base.gameObject.transform.position;
					this.NextRespawnTurn = -1;
					FogOfWar.CalculateFogOfWarForTeam(this.\u000E());
					if (this.\u0012() != null)
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
						this.SetMostRecentDeathSquare(this.\u0012());
					}
					base.gameObject.SendMessage("OnDeath");
					if (GameFlowData.Get() != null)
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
						GameFlowData.Get().NotifyOnActorDeath(this);
					}
					this.UnoccupyCurrentBoardSquare();
					this.SetCurrentBoardSquare(null);
					this.ClientLastKnownPosSquare = null;
					this.ServerLastKnownPosSquare = null;
					return;
				}
			}
			if (!flag)
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
				if (this.m_hitPoints > 0 && this.LastDeathTurn > 0)
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
					base.gameObject.SendMessage("OnRespawn");
					this.m_lastVisibleTurnToClient = 0;
					if (NetworkServer.active)
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
						if (this.m_teamSensitiveData_friendly != null)
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
							this.m_teamSensitiveData_friendly.MarkAsRespawning();
						}
						if (this.m_teamSensitiveData_hostile != null)
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
							this.m_teamSensitiveData_hostile.MarkAsRespawning();
						}
						if (num > 0)
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
						}
					}
				}
			}
		}
	}

	internal int UnresolvedDamage
	{
		get
		{
			return this._unresolvedDamage;
		}
		set
		{
			if (MatchLogger.Get() != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.set_UnresolvedDamage(int)).MethodHandle;
				}
				MatchLogger.Get().Log(string.Concat(new object[]
				{
					this,
					" UnresolvedDamage.set ",
					value,
					", old: ",
					this.UnresolvedDamage
				}));
			}
			if (this._unresolvedDamage != value)
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
				this._unresolvedDamage = value;
				this.ClientUnresolvedDamage = 0;
			}
		}
	}

	internal int UnresolvedHealing
	{
		get
		{
			return this._unresolvedHealing;
		}
		set
		{
			if (MatchLogger.Get() != null)
			{
				MatchLogger.Get().Log(string.Concat(new object[]
				{
					this,
					" UnresolvedHealing.set ",
					value,
					", old: ",
					this.UnresolvedHealing
				}));
			}
			if (this._unresolvedHealing != value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.set_UnresolvedHealing(int)).MethodHandle;
				}
				this._unresolvedHealing = value;
				this.ClientUnresolvedHealing = 0;
			}
		}
	}

	internal int UnresolvedTechPointGain
	{
		get
		{
			return this._unresolvedTechPointGain;
		}
		set
		{
			if (MatchLogger.Get() != null)
			{
				MatchLogger.Get().Log(string.Concat(new object[]
				{
					this,
					" UnresolvedTechPointGain.set ",
					value,
					", old: ",
					this.UnresolvedTechPointGain
				}));
			}
			if (this._unresolvedTechPointGain != value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.set_UnresolvedTechPointGain(int)).MethodHandle;
				}
				this._unresolvedTechPointGain = value;
				this.ClientUnresolvedTechPointGain = 0;
			}
		}
	}

	internal int UnresolvedTechPointLoss
	{
		get
		{
			return this._unresolvedTechPointLoss;
		}
		set
		{
			if (MatchLogger.Get() != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.set_UnresolvedTechPointLoss(int)).MethodHandle;
				}
				MatchLogger.Get().Log(string.Concat(new object[]
				{
					this,
					" UnresolvedTechPointLoss.set ",
					value,
					", old: ",
					this.UnresolvedTechPointLoss
				}));
			}
			if (this._unresolvedTechPointLoss != value)
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
				this._unresolvedTechPointLoss = value;
				this.ClientUnresolvedTechPointLoss = 0;
			}
		}
	}

	internal int ExpectedHoTTotal
	{
		get
		{
			return this.m_serverExpectedHoTTotal;
		}
		set
		{
			if (this.m_serverExpectedHoTTotal != value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.set_ExpectedHoTTotal(int)).MethodHandle;
				}
				this.m_serverExpectedHoTTotal = value;
				this.ClientExpectedHoTTotalAdjust = 0;
			}
		}
	}

	internal int ExpectedHoTThisTurn
	{
		get
		{
			return this.m_serverExpectedHoTThisTurn;
		}
		set
		{
			if (this.m_serverExpectedHoTThisTurn != value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.set_ExpectedHoTThisTurn(int)).MethodHandle;
				}
				this.m_serverExpectedHoTThisTurn = value;
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
			if (DebugParameters.Get() != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.get_TechPoints()).MethodHandle;
				}
				if (DebugParameters.Get().GetParameterAsBool("InfiniteTP"))
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
					return this.\u0016();
				}
			}
			return this.m_techPoints;
		}
		private set
		{
			if (NetworkServer.active)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.set_TechPoints(int)).MethodHandle;
				}
				this.m_techPoints = Mathf.Clamp(value, 0, this.\u0016());
			}
			else
			{
				this.m_techPoints = value;
			}
		}
	}

	internal void SetTechPoints(int value, bool combatText = false, ActorData caster = null, string sourceName = null)
	{
		int max = this.\u0016();
		value = Mathf.Clamp(value, 0, max);
		int num = value - this.TechPoints;
		this.TechPoints = value;
		if (combatText)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.SetTechPoints(int, bool, ActorData, string)).MethodHandle;
			}
			if (sourceName != null)
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
				if (num != 0)
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
					this.DoTechPointsLogAndCombatText(caster, this, sourceName, num);
				}
			}
		}
	}

	private void DoTechPointsLogAndCombatText(ActorData caster, ActorData target, string sourceName, int healAmount)
	{
		bool flag = healAmount >= 0;
		string text = string.Format("{0}", healAmount);
		string text2;
		if (caster == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.DoTechPointsLogAndCombatText(ActorData, ActorData, string, int)).MethodHandle;
			}
			text2 = string.Empty;
		}
		else
		{
			text2 = string.Format("{0}'s ", caster.DisplayName);
		}
		string text3 = text2;
		string text4 = (!flag) ? string.Format("{0}{1} removes {3} Energy from {2}", new object[]
		{
			text3,
			sourceName,
			target.DisplayName,
			healAmount
		}) : string.Format("{0}{1} adds {3} Energy to {2}", new object[]
		{
			text3,
			sourceName,
			target.DisplayName,
			healAmount
		});
		string combatText = text;
		string logText = text4;
		CombatTextCategory category;
		if (flag)
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
			category = CombatTextCategory.TP_Recovery;
		}
		else
		{
			category = CombatTextCategory.TP_Damage;
		}
		target.CallRpcCombatText(combatText, logText, category, BuffIconToDisplay.None);
	}

	internal void DoCheatLogAndCombatText(string cheatName)
	{
		string combatText = string.Format("{0}", cheatName);
		string logText = string.Format("{0} used cheat: {1}", this.DisplayName, cheatName);
		this.CallRpcCombatText(combatText, logText, CombatTextCategory.Other, BuffIconToDisplay.None);
	}

	internal void SetHitPoints(int value)
	{
		this.HitPoints = value;
	}

	internal void SetAbsorbPoints(int value)
	{
		this.AbsorbPoints = value;
	}

	public int ReservedTechPoints
	{
		get
		{
			return this.m_reservedTechPoints;
		}
		set
		{
			if (this.m_reservedTechPoints != value)
			{
				this.m_reservedTechPoints = value;
				this.ClientReservedTechPoints = 0;
			}
		}
	}

	internal bool \u000E()
	{
		return this.HitPoints == 0;
	}

	public bool IgnoreForEnergyOnHit
	{
		get
		{
			return this.m_ignoreForEnergyForHit;
		}
		set
		{
			if (NetworkServer.active)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.set_IgnoreForEnergyOnHit(bool)).MethodHandle;
				}
				this.m_ignoreForEnergyForHit = value;
			}
		}
	}

	public bool IgnoreForAbilityHits
	{
		get
		{
			return this.m_ignoreFromAbilityHits;
		}
		set
		{
			if (NetworkServer.active)
			{
				this.m_ignoreFromAbilityHits = value;
			}
		}
	}

	internal int AbsorbPoints
	{
		get
		{
			return this.m_absorbPoints;
		}
		private set
		{
			if (this.m_absorbPoints != value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.set_AbsorbPoints(int)).MethodHandle;
				}
				if (NetworkServer.active)
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
					this.m_absorbPoints = Mathf.Max(value, 0);
				}
				else
				{
					this.m_absorbPoints = Mathf.Max(value, 0);
				}
				this.ClientUnresolvedAbsorb = 0;
			}
		}
	}

	internal int MechanicPoints
	{
		get
		{
			return this.m_mechanicPoints;
		}
		set
		{
			if (this.m_mechanicPoints != value)
			{
				this.m_mechanicPoints = Mathf.Max(value, 0);
			}
		}
	}

	internal int SpawnerId
	{
		get
		{
			return this.m_spawnerId;
		}
		set
		{
			if (this.m_spawnerId != value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.set_SpawnerId(int)).MethodHandle;
				}
				this.m_spawnerId = value;
			}
		}
	}

	public bool DisappearingAfterCurrentMovement
	{
		get
		{
			return this.m_disappearingAfterCurrentMovement;
		}
	}

	public BoardSquare CurrentBoardSquare
	{
		get
		{
			return this.m_clientCurrentBoardSquare;
		}
	}

	public ActorTeamSensitiveData TeamSensitiveData_authority
	{
		get
		{
			if (this.m_teamSensitiveData_friendly != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.get_TeamSensitiveData_authority()).MethodHandle;
				}
				return this.m_teamSensitiveData_friendly;
			}
			return this.m_teamSensitiveData_hostile;
		}
	}

	public ActorTeamSensitiveData \u000E()
	{
		if (this.m_teamSensitiveData_friendly != null)
		{
			return this.m_teamSensitiveData_friendly;
		}
		if (this.m_teamSensitiveData_hostile != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u000E()).MethodHandle;
			}
			return this.m_teamSensitiveData_hostile;
		}
		return null;
	}

	public BoardSquare MoveFromBoardSquare
	{
		get
		{
			if (NetworkServer.active)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.get_MoveFromBoardSquare()).MethodHandle;
				}
				return this.m_trueMoveFromBoardSquare;
			}
			if (this.m_teamSensitiveData_friendly != null)
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
				return this.m_teamSensitiveData_friendly.MoveFromBoardSquare;
			}
			return this.CurrentBoardSquare;
		}
		set
		{
			if (NetworkServer.active && this.m_trueMoveFromBoardSquare != value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.set_MoveFromBoardSquare(BoardSquare)).MethodHandle;
				}
				this.m_trueMoveFromBoardSquare = value;
				if (this.m_teamSensitiveData_friendly != null)
				{
					this.m_teamSensitiveData_friendly.MoveFromBoardSquare = value;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.get_InitialMoveStartSquare()).MethodHandle;
				}
				return this.m_serverInitialMoveStartSquare;
			}
			if (this.m_teamSensitiveData_friendly != null)
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
				return this.m_teamSensitiveData_friendly.InitialMoveStartSquare;
			}
			return this.CurrentBoardSquare;
		}
		set
		{
			if (NetworkServer.active)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.set_InitialMoveStartSquare(BoardSquare)).MethodHandle;
				}
				if (this.m_serverInitialMoveStartSquare != value)
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
					this.m_serverInitialMoveStartSquare = value;
					if (this.\u000E() != null)
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
						this.\u000E().UpdateSquaresCanMoveTo();
					}
					if (this.m_teamSensitiveData_friendly != null)
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
						this.m_teamSensitiveData_friendly.InitialMoveStartSquare = value;
					}
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
			return this.m_internalQueuedMovementAllowsAbility;
		}
		set
		{
			if (value != this.m_internalQueuedMovementAllowsAbility)
			{
				this.m_internalQueuedMovementAllowsAbility = value;
			}
		}
	}

	public bool KnockbackMoveStarted
	{
		get
		{
			return this.m_knockbackMoveStarted;
		}
		set
		{
			this.m_knockbackMoveStarted = value;
		}
	}

	internal Vector3 LastDeathPosition { get; private set; }

	internal int LastDeathTurn { get; private set; }

	public int NextRespawnTurn
	{
		get
		{
			return this.m_nextRespawnTurn;
		}
		set
		{
			this.m_nextRespawnTurn = Mathf.Max(value, this.LastDeathTurn + 1);
		}
	}

	public List<BoardSquare> respawnSquares
	{
		get
		{
			if (NetworkServer.active)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.get_respawnSquares()).MethodHandle;
				}
				return this.m_trueRespawnSquares;
			}
			if (this.m_teamSensitiveData_friendly != null)
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
				return this.m_teamSensitiveData_friendly.RespawnAvailableSquares;
			}
			return new List<BoardSquare>();
		}
		set
		{
			if (NetworkServer.active)
			{
				this.m_trueRespawnSquares = value;
				if (this.m_teamSensitiveData_friendly != null)
				{
					this.m_teamSensitiveData_friendly.RespawnAvailableSquares = value;
				}
			}
		}
	}

	public void ClearRespawnSquares()
	{
		this.m_trueRespawnSquares.Clear();
		if (this.m_teamSensitiveData_friendly != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.ClearRespawnSquares()).MethodHandle;
			}
			this.m_teamSensitiveData_friendly.RespawnAvailableSquares = new List<BoardSquare>();
		}
	}

	public bool \u000E(BoardSquare \u001D)
	{
		return false;
	}

	public BoardSquare RespawnPickedPositionSquare
	{
		get
		{
			if (NetworkServer.active)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.get_RespawnPickedPositionSquare()).MethodHandle;
				}
				return this.m_trueRespawnPositionSquare;
			}
			if (this.m_teamSensitiveData_friendly != null)
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
				return this.m_teamSensitiveData_friendly.RespawnPickedSquare;
			}
			if (this.m_teamSensitiveData_hostile != null)
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
				return this.m_teamSensitiveData_hostile.RespawnPickedSquare;
			}
			return null;
		}
		set
		{
			if (NetworkServer.active)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.set_RespawnPickedPositionSquare(BoardSquare)).MethodHandle;
				}
				this.m_trueRespawnPositionSquare = value;
				if (this.m_teamSensitiveData_friendly != null)
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
					if (!GameFlowData.Get().IsInDecisionState())
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
						if (GameFlowData.Get().CurrentTurn != this.NextRespawnTurn)
						{
							this.m_teamSensitiveData_friendly.RespawnPickedSquare = null;
							goto IL_8A;
						}
					}
					this.m_teamSensitiveData_friendly.RespawnPickedSquare = value;
				}
				IL_8A:
				if (this.m_teamSensitiveData_hostile != null)
				{
					if (GameFlowData.Get().CurrentTurn == this.NextRespawnTurn)
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
						if (this.\u000E(value))
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
							this.m_teamSensitiveData_hostile.RespawnPickedSquare = value;
						}
						else
						{
							this.m_teamSensitiveData_hostile.RespawnPickedSquare = null;
						}
					}
					else
					{
						this.m_teamSensitiveData_hostile.RespawnPickedSquare = null;
					}
				}
			}
		}
	}

	public bool HasBotController
	{
		get
		{
			return this.m_hasBotController;
		}
		set
		{
			this.m_hasBotController = value;
		}
	}

	public bool VisibleTillEndOfPhase { get; set; }

	public bool CurrentlyVisibleForAbilityCast
	{
		get
		{
			return this.m_currentlyVisibleForAbilityCast;
		}
		set
		{
			if (this.m_currentlyVisibleForAbilityCast != value)
			{
				if (ActorDebugUtils.Get() != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.set_CurrentlyVisibleForAbilityCast(bool)).MethodHandle;
					}
					if (ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.LastKnownPosition, false))
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
						UnityEngine.Debug.LogWarning(this.\u0018() + "Setting visible for ability cast to " + value);
					}
				}
				this.m_currentlyVisibleForAbilityCast = value;
			}
		}
	}

	public bool MovedForEvade
	{
		get
		{
			return this.m_movedForEvade;
		}
		set
		{
			if (this.m_movedForEvade != value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.set_MovedForEvade(bool)).MethodHandle;
				}
				this.m_movedForEvade = value;
			}
		}
	}

	public bool ServerSuppressInvisibility
	{
		get
		{
			return this.m_serverSuppressInvisibility;
		}
		set
		{
			if (this.m_serverSuppressInvisibility != value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.set_ServerSuppressInvisibility(bool)).MethodHandle;
				}
				this.m_serverSuppressInvisibility = value;
			}
		}
	}

	internal void AddLineOfSightVisibleException(ActorData visibleActor)
	{
		this.m_lineOfSightVisibleExceptions.Add(visibleActor);
		this.\u000E().MarkForRecalculateVisibility();
	}

	internal void RemoveLineOfSightVisibleException(ActorData visibleActor)
	{
		this.m_lineOfSightVisibleExceptions.Remove(visibleActor);
		this.\u000E().MarkForRecalculateVisibility();
	}

	internal bool \u000E(ActorData \u001D)
	{
		return this.m_lineOfSightVisibleExceptions.Contains(\u001D);
	}

	internal ReadOnlyCollection<ActorData> LineOfSightVisibleExceptions
	{
		get
		{
			return this.m_lineOfSightVisibleExceptions.AsReadOnly();
		}
	}

	internal ReadOnlyCollection<BoardSquare> LineOfSightVisibleExceptionSquares
	{
		get
		{
			List<BoardSquare> list = new List<BoardSquare>(this.m_lineOfSightVisibleExceptions.Count);
			foreach (ActorData actorData in this.m_lineOfSightVisibleExceptions)
			{
				list.Add(actorData.\u0012());
			}
			return list.AsReadOnly();
		}
	}

	public event Action OnTurnStartDelegates
	{
		add
		{
			Action action = this.OnTurnStartDelegates;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnTurnStartDelegates, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.add_OnTurnStartDelegates(Action)).MethodHandle;
			}
		}
		remove
		{
			Action action = this.OnTurnStartDelegates;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnTurnStartDelegates, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<UnityEngine.Object, GameObject> OnAnimationEventDelegates;

	public event Action<Ability> OnSelectedAbilityChangedDelegates
	{
		add
		{
			Action<Ability> action = this.OnSelectedAbilityChangedDelegates;
			Action<Ability> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<Ability>>(ref this.OnSelectedAbilityChangedDelegates, (Action<Ability>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.add_OnSelectedAbilityChangedDelegates(Action<Ability>)).MethodHandle;
			}
		}
		remove
		{
			Action<Ability> action = this.OnSelectedAbilityChangedDelegates;
			Action<Ability> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<Ability>>(ref this.OnSelectedAbilityChangedDelegates, (Action<Ability>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.remove_OnSelectedAbilityChangedDelegates(Action<Ability>)).MethodHandle;
			}
		}
	}

	public event Action OnClientQueuedActionChangedDelegates
	{
		add
		{
			Action action = this.OnClientQueuedActionChangedDelegates;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnClientQueuedActionChangedDelegates, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.add_OnClientQueuedActionChangedDelegates(Action)).MethodHandle;
			}
		}
		remove
		{
			Action action = this.OnClientQueuedActionChangedDelegates;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnClientQueuedActionChangedDelegates, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public void OnClientQueuedActionChanged()
	{
		if (this.OnClientQueuedActionChangedDelegates != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.OnClientQueuedActionChanged()).MethodHandle;
			}
			this.OnClientQueuedActionChangedDelegates();
		}
	}

	public void OnSelectedAbilityChanged(Ability ability)
	{
		if (this.OnSelectedAbilityChangedDelegates != null)
		{
			this.OnSelectedAbilityChangedDelegates(ability);
		}
	}

	private void Awake()
	{
		if (ActorData.<>f__mg$cache0 == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.Awake()).MethodHandle;
			}
			ActorData.<>f__mg$cache0 = new Action<GameState>(ActorData.OnGameStateChanged);
		}
		GameFlowData.s_onGameStateChanged -= ActorData.<>f__mg$cache0;
		if (ActorData.<>f__mg$cache1 == null)
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
			ActorData.<>f__mg$cache1 = new Action<GameState>(ActorData.OnGameStateChanged);
		}
		GameFlowData.s_onGameStateChanged += ActorData.<>f__mg$cache1;
		this.PlayerData = base.GetComponent<PlayerData>();
		if (this.PlayerData == null)
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
			throw new Exception(string.Format("Character {0} needs a PlayerData component", base.gameObject.name));
		}
		this.m_actorMovement = base.gameObject.GetComponent<ActorMovement>();
		if (this.m_actorMovement == null)
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
			this.m_actorMovement = base.gameObject.AddComponent<ActorMovement>();
		}
		this.m_actorTurnSM = base.gameObject.GetComponent<ActorTurnSM>();
		if (this.m_actorTurnSM == null)
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
			this.m_actorTurnSM = base.gameObject.AddComponent<ActorTurnSM>();
		}
		this.m_actorCover = base.gameObject.GetComponent<ActorCover>();
		if (this.m_actorCover == null)
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
			this.m_actorCover = base.gameObject.AddComponent<ActorCover>();
		}
		this.m_actorVFX = base.gameObject.GetComponent<ActorVFX>();
		if (this.m_actorVFX == null)
		{
			this.m_actorVFX = base.gameObject.AddComponent<ActorVFX>();
		}
		this.m_timeBank = base.gameObject.GetComponent<TimeBank>();
		if (this.m_timeBank == null)
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
			this.m_timeBank = base.gameObject.AddComponent<TimeBank>();
		}
		this.m_additionalVisionProvider = base.gameObject.GetComponent<ActorAdditionalVisionProviders>();
		if (this.m_additionalVisionProvider == null)
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
			this.m_additionalVisionProvider = base.gameObject.AddComponent<ActorAdditionalVisionProviders>();
		}
		this.m_actorBehavior = base.GetComponent<ActorBehavior>();
		this.m_abilityData = base.GetComponent<AbilityData>();
		this.m_itemData = base.GetComponent<ItemData>();
		this.m_actorStats = base.GetComponent<ActorStats>();
		this.m_actorStatus = base.GetComponent<ActorStatus>();
		this.m_actorTargeting = base.GetComponent<ActorTargeting>();
		this.m_passiveData = base.GetComponent<PassiveData>();
		this.m_combatText = base.GetComponent<CombatText>();
		this.m_actorTags = base.GetComponent<ActorTag>();
		this.m_freelancerStats = base.GetComponent<FreelancerStats>();
		if (NetworkServer.active)
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
			this.ActorIndex = (int)(checked(ActorData.s_nextActorIndex += 1));
		}
		ActorData.Layer = LayerMask.NameToLayer("Actor");
		ActorData.Layer_Mask = 1 << ActorData.Layer;
		if (GameFlowData.Get())
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
			this.m_lastSpawnTurn = Mathf.Max(1, GameFlowData.Get().CurrentTurn);
		}
		else
		{
			this.m_lastSpawnTurn = 1;
		}
		this.LastDeathTurn = -2;
		this.NextRespawnTurn = -1;
		this.HasBotController = false;
		this.SpawnerId = -1;
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GametimeScaleChange);
	}

	public void InitializeModel(PrefabResourceLink heroPrefabLink, bool addMasterSkinVfx)
	{
		if (this.m_actorSkinPrefabLink.GUID == heroPrefabLink.GUID)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.InitializeModel(PrefabResourceLink, bool)).MethodHandle;
			}
			return;
		}
		if (this.m_actorSkinPrefabLink != null && !this.m_actorSkinPrefabLink.IsEmpty)
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
			Log.Warning(Log.Category.ActorData, string.Format("ActorData already initialized to a different prefab.  Currently [{0}], setting to [{1}]", this.m_actorSkinPrefabLink.ToString()), new object[0]);
		}
		this.m_actorSkinPrefabLink = heroPrefabLink;
		if (heroPrefabLink == null)
		{
			throw new ApplicationException("Actor skin not set on awake. [" + base.gameObject.name + "]");
		}
		if (!NetworkClient.active)
		{
			if (HydrogenConfig.Get().SkipCharacterModelSpawnOnServer)
			{
				goto IL_155;
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
		GameObject gameObject = heroPrefabLink.InstantiatePrefab(false);
		if (gameObject)
		{
			this.m_actorModelData = gameObject.GetComponent<ActorModelData>();
			if (this.m_actorModelData)
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
				int layer = LayerMask.NameToLayer("Actor");
				foreach (Transform transform in this.m_actorModelData.gameObject.GetComponentsInChildren<Transform>(true))
				{
					transform.gameObject.layer = layer;
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
		}
		IL_155:
		if (this.m_actorModelData != null)
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
			this.m_actorModelData.Setup(this);
			if (addMasterSkinVfx)
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
				if (NetworkClient.active && MasterSkinVfxData.Get() != null)
				{
					GameObject masterSkinVfxInst = MasterSkinVfxData.Get().AddMasterSkinVfxOnCharacterObject(this.m_actorModelData.gameObject, this.m_characterType, 1f);
					this.m_actorModelData.SetMasterSkinVfxInst(masterSkinVfxInst);
				}
			}
		}
		if (this.m_faceActorModelData != null)
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
			this.m_faceActorModelData.Setup(this);
		}
		if (NPCCoordinator.IsSpawnedNPC(this))
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
			if (!this.m_addedToUI)
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
				if (HUD_UI.Get() != null && HUD_UI.Get().m_mainScreenPanel != null)
				{
					this.m_addedToUI = true;
					HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.AddActor(this);
					HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddActor(this);
				}
			}
			NPCCoordinator.Get().OnActorSpawn(this);
			if (this.\u000E() != null)
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
				this.\u000E().ForceUpdateVisibility();
			}
		}
	}

	private void Start()
	{
		if (NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.Start()).MethodHandle;
			}
			this.m_nameplateJoint = new JointPopupProperty();
			this.m_nameplateJoint.m_joint = "VFX_name";
			this.m_nameplateJoint.Initialize(base.gameObject);
		}
		if (NetworkServer.active)
		{
			this.HitPoints = this.\u0012();
			this.UnresolvedDamage = 0;
			this.UnresolvedHealing = 0;
			this.TechPoints = this.m_techPointsOnSpawn;
			this.ReservedTechPoints = 0;
		}
		this.ClientUnresolvedDamage = 0;
		this.ClientUnresolvedHealing = 0;
		this.ClientUnresolvedTechPointGain = 0;
		this.ClientUnresolvedTechPointLoss = 0;
		this.ClientUnresolvedAbsorb = 0;
		this.ClientReservedTechPoints = 0;
		this.ClientAppliedHoTThisTurn = 0;
		base.transform.parent = GameFlowData.Get().GetActorRoot().transform;
		GameFlowData.Get().AddActor(this);
		this.EnableRagdoll(false, null, false);
		if (!this.m_addedToUI && HUD_UI.Get() != null)
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
			if (HUD_UI.Get().m_mainScreenPanel != null)
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
				this.m_addedToUI = true;
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.AddActor(this);
				HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddActor(this);
			}
		}
		PlayerDetails playerDetails = null;
		if (GameFlow.Get().playerDetails.TryGetValue(this.PlayerData.GetPlayer(), out playerDetails) && playerDetails.IsLocal())
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
			Log.Info("ActorData.Start {0} {1}", new object[]
			{
				this,
				playerDetails
			});
			GameFlowData.Get().AddOwnedActorData(this);
		}
	}

	public override void OnStartLocalPlayer()
	{
		Log.Info("ActorData.OnStartLocalPlayer {0}", new object[]
		{
			this
		});
		GameFlowData.Get().AddOwnedActorData(this);
		if (ClientBootstrap.LoadTest)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.OnStartLocalPlayer()).MethodHandle;
			}
			this.CallCmdDebugReplaceWithBot();
		}
	}

	private static void OnGameStateChanged(GameState newState)
	{
		if (newState != GameState.BothTeams_Decision)
		{
			if (newState != GameState.BothTeams_Resolve)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.OnGameStateChanged(GameState)).MethodHandle;
				}
				if (newState != GameState.EndingGame)
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
				else
				{
					ActorData.s_nextActorIndex = 0;
				}
			}
			else
			{
				List<ActorData> actors = GameFlowData.Get().GetActors();
				using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData actorData = enumerator.Current;
						if (actorData != null)
						{
							if (actorData.\u000E() != null)
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
								Animator animator = actorData.\u000E();
								if (animator != null)
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
									if (actorData.\u000E().HasTurnStartParameter())
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
										animator.SetBool("TurnStart", false);
									}
								}
							}
							if (actorData.GetComponent<LineData>() != null)
							{
								actorData.GetComponent<LineData>().OnResolveStart();
							}
							if (HUD_UI.Get() != null)
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
								HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateNameplateUntargeted(actorData, false);
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
		}
		else
		{
			ActorData.HandleRagdollOnDecisionStart();
			int currentTurn = GameFlowData.Get().CurrentTurn;
			List<ActorData> actors2 = GameFlowData.Get().GetActors();
			bool flag = false;
			using (List<ActorData>.Enumerator enumerator2 = actors2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData actorData2 = enumerator2.Current;
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
						if (!actorData2.\u000E())
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
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								if (actorData2.\u0012())
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
									UnityEngine.Debug.LogError(string.Concat(new object[]
									{
										"Unragdolling undead actor on Turn Tick (",
										currentTurn,
										"): ",
										actorData2.\u0018()
									}));
									actorData2.EnableRagdoll(false, null, false);
									flag = true;
								}
							}
						}
					}
					if (actorData2 != null && !actorData2.\u000E())
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
						if (actorData2.\u0012() == null)
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
							if (actorData2.PlayerIndex != PlayerData.s_invalidPlayerIndex)
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
								if (NetworkClient.active)
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
									if (!NetworkServer.active)
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
										if (GameFlowData.Get().LocalPlayerData.IsViewingTeam(actorData2.\u000E()))
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
											UnityEngine.Debug.LogError("On client, living friendly-to-client actor " + actorData2.\u0018() + " has null square on Turn Tick");
											flag = true;
										}
									}
								}
							}
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
			if (NetworkServer.active)
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
				if (flag)
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
				}
			}
		}
	}

	private static void HandleRagdollOnDecisionStart()
	{
		List<ActorData> actors = GameFlowData.Get().GetActors();
		for (int i = 0; i < actors.Count; i++)
		{
			ActorData actorData = actors[i];
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.HandleRagdollOnDecisionStart()).MethodHandle;
				}
				if (actorData.\u000E())
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
					if (actorData.LastDeathTurn != GameFlowData.Get().CurrentTurn && !actorData.\u0012())
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
						if (actorData.NextRespawnTurn != GameFlowData.Get().CurrentTurn)
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
							actorData.DoVisualDeath(Sequence.CreateImpulseInfoWithActorForward(actorData));
						}
					}
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

	public Animator \u000E()
	{
		if (this.m_actorModelData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u000E()).MethodHandle;
			}
			return this.m_actorModelData.GetModelAnimator();
		}
		return null;
	}

	public void PlayDamageReactionAnim(string customDamageReactTriggerName)
	{
		Animator animator = this.\u000E();
		if (animator == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.PlayDamageReactionAnim(string)).MethodHandle;
			}
			return;
		}
		if (this.m_actorMovement.GetAestheticPath() == null)
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
			if (!this.m_actorMovement.AmMoving())
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
				bool flag;
				if (ServerClientUtils.GetCurrentAbilityPhase() == AbilityPriority.Combat_Knockback)
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
					if (ClientKnockbackManager.Get() != null)
					{
						flag = ClientKnockbackManager.Get().ActorHasIncomingKnockback(this);
						goto IL_93;
					}
				}
				flag = false;
				IL_93:
				if (!flag)
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
					Animator animator2 = animator;
					string trigger;
					if (string.IsNullOrEmpty(customDamageReactTriggerName))
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
						trigger = "StartDamageReaction";
					}
					else
					{
						trigger = customDamageReactTriggerName;
					}
					animator2.SetTrigger(trigger);
				}
			}
		}
	}

	internal bool \u0012()
	{
		Animator animator = this.\u000E();
		bool result;
		if (animator == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0012()).MethodHandle;
			}
			result = true;
		}
		else
		{
			result = !animator.enabled;
		}
		return result;
	}

	internal void DoVisualDeath(ActorModelData.ImpulseInfo impulseInfo)
	{
		if (this.\u0012())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.DoVisualDeath(ActorModelData.ImpulseInfo)).MethodHandle;
			}
			UnityEngine.Debug.LogWarning("Already in ragdoll");
		}
		if (this.m_actorVFX != null)
		{
			this.m_actorVFX.ShowOnDeathVfx(this.\u000E().transform.position);
		}
		this.EnableRagdoll(true, impulseInfo, false);
		GameEventManager.Get().FireEvent(GameEventManager.EventType.CharacterVisualDeath, new GameEventManager.CharacterDeathEventArgs
		{
			deadCharacter = this
		});
		if (AudioManager.s_deathAudio)
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
			AudioManager.PostEvent("ui/ingame/death", base.gameObject);
			if (!string.IsNullOrEmpty(this.m_onDeathAudioEvent))
			{
				AudioManager.PostEvent(this.m_onDeathAudioEvent, base.gameObject);
			}
		}
		Team team = Team.Invalid;
		if (GameFlowData.Get() != null)
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
			if (GameFlowData.Get().LocalPlayerData != null)
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
				team = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
			}
		}
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
			if (this.\u000E() == team)
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
				clientFog.MarkForRecalculateVisibility();
			}
		}
		if (NetworkClient.active)
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
			if (ObjectivePoints.Get() != null)
			{
				ObjectivePoints.Get().Client_OnActorDeath(this);
				if (GameplayUtils.IsPlayerControlled(this) && GameFlowData.Get().LocalPlayerData != null)
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
					int num = ObjectivePoints.Get().Client_GetNumDeathOnTeamForCurrentTurn(this.\u000E());
					if (num > 0)
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
						if (UIDeathNotifications.Get() != null)
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
							UIDeathNotifications.Get().NotifyDeathOccurred(this, this.\u000E() == team);
						}
					}
				}
			}
			if (CaptureTheFlag.Get() != null)
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
				CaptureTheFlag.Get().Client_OnActorDeath(this);
			}
			if (CollectTheCoins.Get() != null)
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
				CollectTheCoins.Get().Client_OnActorDeath(this);
			}
			if (GameWideData.Get().FreeAutomaticOverconOnDeath_OverconID != -1)
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
				if (UIOverconData.Get() != null)
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
					UIOverconData.Get().UseOvercon(GameWideData.Get().FreeAutomaticOverconOnDeath_OverconID, this.ActorIndex, true);
				}
			}
		}
	}

	private void EnableRagdoll(bool ragDollOn, ActorModelData.ImpulseInfo impulseInfo = null, bool isDebugRagdoll = false)
	{
		if (ragDollOn && this.\u0009() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.EnableRagdoll(bool, ActorModelData.ImpulseInfo, bool)).MethodHandle;
			}
			if (!isDebugRagdoll)
			{
				Log.Error(string.Concat(new object[]
				{
					"early_ragdoll: enabling ragdoll on ",
					this.\u0018(),
					" with ",
					this.HitPoints,
					" HP,  (HP for display ",
					this.\u0009(),
					")\n",
					Environment.StackTrace
				}), new object[0]);
			}
		}
		if (this.m_actorModelData != null)
		{
			this.m_actorModelData.EnableRagdoll(ragDollOn, impulseInfo);
		}
	}

	public void OnReplayRestart()
	{
		this.EnableRendererAndUpdateVisibility();
		this.EnableRagdoll(false, null, false);
		this.ShowRespawnFlare(null, false);
	}

	public void OnRespawnTeleport()
	{
		this.EnableRagdoll(false, null, false);
		if (this == GameFlowData.Get().activeOwnedActorData)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.OnRespawnTeleport()).MethodHandle;
			}
			if (SpawnPointManager.Get() != null)
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
				if (SpawnPointManager.Get().m_spawnInDuringMovement)
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
					InterfaceManager.Get().DisplayAlert(StringUtil.TR("PostRespawnMovement", "Global"), BoardSquare.s_respawnOptionHighlightColor, 60f, true, 0);
				}
			}
		}
	}

	private void OnRespawn()
	{
		this.EnableRagdoll(false, null, false);
		ActorModelData actorModelData = this.\u000E();
		if (actorModelData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.OnRespawn()).MethodHandle;
			}
			actorModelData.ForceUpdateVisibility();
		}
		if (!NetworkServer.active)
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
			if (NPCCoordinator.IsSpawnedNPC(this))
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
				NPCCoordinator.Get().OnActorSpawn(this);
			}
		}
		GameEventManager.Get().FireEvent(GameEventManager.EventType.CharacterRespawn, new GameEventManager.CharacterRespawnEventArgs
		{
			respawningCharacter = this
		});
		if (GameFlowData.Get().activeOwnedActorData == this)
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
			CameraManager.Get().SetTargetObject(base.gameObject, CameraManager.CameraTargetReason.ClientActorRespawned);
		}
		this.m_lastSpawnTurn = GameFlowData.Get().CurrentTurn;
	}

	public bool \u0015()
	{
		if (!this.\u000E())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0015()).MethodHandle;
			}
			if (this.NextRespawnTurn > 0)
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
				if (this.NextRespawnTurn == GameFlowData.Get().CurrentTurn && SpawnPointManager.Get() != null)
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
					return SpawnPointManager.Get().m_spawnInDuringMovement;
				}
			}
		}
		return false;
	}

	private void OnDestroy()
	{
		if (GameFlowData.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.OnDestroy()).MethodHandle;
			}
			GameFlowData.Get().RemoveReferencesToDestroyedActor(this);
		}
		if (ActorData.<>f__mg$cache2 == null)
		{
			ActorData.<>f__mg$cache2 = new Action<GameState>(ActorData.OnGameStateChanged);
		}
		GameFlowData.s_onGameStateChanged -= ActorData.<>f__mg$cache2;
		this.m_actorModelData = null;
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GametimeScaleChange);
	}

	private void Update()
	{
		if (this.m_needAddToTeam)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.Update()).MethodHandle;
			}
			if (GameFlowData.Get() != null)
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
				this.m_needAddToTeam = false;
				GameFlowData.Get().AddToTeam(this);
				TeamStatusDisplay.GetTeamStatusDisplay().RebuildTeamDisplay();
			}
		}
		if (NetworkClient.active)
		{
			this.UpdateClientLastKnownPosSquare();
		}
		if (Quaternion.Angle(base.transform.localRotation, this.m_targetRotation) > 0.01f)
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
			base.transform.localRotation = this.m_targetRotation;
		}
		if (this.m_callHandleOnSelectInUpdate)
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
			this.HandleOnSelect();
			this.m_callHandleOnSelectInUpdate = false;
		}
		if (NetworkServer.active)
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
			base.SetDirtyBit(1U);
		}
		if (!this.m_addedToUI)
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
			if (HUD_UI.Get() != null)
			{
				this.m_addedToUI = true;
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.AddActor(this);
				HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddActor(this);
			}
		}
	}

	public bool \u0016()
	{
		int num = this.\u0018();
		bool result;
		if (num == -1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0016()).MethodHandle;
			}
			result = false;
		}
		else if (!BrushCoordinator.Get().IsRegionFunctioning(num))
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
			result = false;
		}
		else
		{
			result = true;
		}
		return result;
	}

	public int \u0018()
	{
		int result = -1;
		BoardSquare boardSquare = this.\u000E();
		if (boardSquare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0018()).MethodHandle;
			}
			if (boardSquare.\u0012())
			{
				result = boardSquare.BrushRegion;
			}
		}
		return result;
	}

	public bool \u0013()
	{
		int result;
		if (!this.m_hideNameplate && !this.m_alwaysHideNameplate && this.ShowInGameGUI)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0013()).MethodHandle;
			}
			if (this.\u0018())
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
				if (!this.\u0012())
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
					if (!(this.\u000E() == null))
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
						result = (this.\u000E().IsVisibleToClient() ? 1 : 0);
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

	public void SetNameplateAlwaysInvisible(bool alwaysHide)
	{
		this.m_alwaysHideNameplate = alwaysHide;
	}

	private bool CalculateIsActorVisibleToClient()
	{
		bool result = false;
		if (GameFlowData.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.CalculateIsActorVisibleToClient()).MethodHandle;
			}
			if (GameFlowData.Get().LocalPlayerData != null)
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
				PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (DebugParameters.Get() != null)
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
					if (DebugParameters.Get().GetParameterAsBool("AllCharactersVisible"))
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
						return true;
					}
				}
				if (GameFlowData.Get().gameState == GameState.Deployment)
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
				}
				else
				{
					if (activeOwnedActorData != null)
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
						if (activeOwnedActorData.\u000E() == this.\u000E())
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
							return true;
						}
					}
					if (activeOwnedActorData == null)
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
						if (localPlayerData.IsViewingTeam(this.\u000E()))
						{
							return true;
						}
					}
					if (this.m_endVisibilityForHitTime > Time.time)
					{
						result = true;
					}
					else
					{
						if (this.m_actorModelData != null)
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
							if (this.m_actorModelData.IsInCinematicCam())
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
								return true;
							}
						}
						if (this.CurrentlyVisibleForAbilityCast)
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
						}
						else if (this.m_disappearingAfterCurrentMovement && this.CurrentBoardSquare == null && !this.\u000E().AmMoving())
						{
							result = false;
						}
						else
						{
							bool flag = this.\u000E(localPlayerData, false);
							bool flag2 = this.\u000E(localPlayerData, false, false);
							if (flag)
							{
								result = true;
							}
							else if (flag2)
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
								result = false;
							}
							else if (FogOfWar.GetClientFog() == null)
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
								result = false;
							}
							else if (FogOfWar.GetClientFog().IsVisible(this.\u000E()))
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
								result = false;
							}
						}
					}
				}
			}
		}
		return result;
	}

	public void ForceUpdateActorModelVisibility()
	{
		if (NetworkClient.active && this.m_actorModelData != null)
		{
			this.m_actorModelData.ForceUpdateVisibility();
		}
	}

	public void ForceUpdateIsVisibleToClientCache()
	{
		this.m_lastIsVisibleToClientTime = 0f;
		this.UpdateIsVisibleToClientCache();
	}

	private void UpdateIsVisibleToClientCache()
	{
		if (this.m_lastIsVisibleToClientTime < Time.time)
		{
			this.m_lastIsVisibleToClientTime = Time.time;
			this.m_isVisibleToClientCache = this.CalculateIsActorVisibleToClient();
		}
	}

	public bool \u0018()
	{
		this.UpdateIsVisibleToClientCache();
		return this.m_isVisibleToClientCache;
	}

	public bool \u0012(ActorData \u001D)
	{
		bool result = false;
		if (GameFlowData.Get().IsActorDataOwned(this))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0012(ActorData)).MethodHandle;
			}
			if (this.\u000E() == \u001D.\u000E())
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
				return true;
			}
		}
		if (this.m_endVisibilityForHitTime > Time.time)
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
			result = true;
		}
		else
		{
			bool flag = this.\u000E(\u001D.PlayerData, true);
			bool flag2 = this.\u000E(\u001D.PlayerData, true, false);
			if (flag)
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
			else if (flag2)
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
				result = false;
			}
			else if (\u001D.\u000E())
			{
				result = \u001D.\u000E().IsVisible(this.\u000E());
			}
		}
		return result;
	}

	public bool \u000E(PlayerData \u001D, bool \u000E = true)
	{
		bool flag;
		if (\u001D == null)
		{
			flag = false;
		}
		else if (this.\u000E().HasStatus(StatusType.Revealed, \u000E) && this.\u000E() != \u001D.GetTeamViewing())
		{
			flag = true;
		}
		else
		{
			if (!NetworkServer.active)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u000E(PlayerData, bool)).MethodHandle;
				}
				if (CaptureTheFlag.IsActorRevealedByFlag_Client(this))
				{
					return true;
				}
			}
			if (this.VisibleTillEndOfPhase)
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
				if (!this.MovedForEvade)
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
					return true;
				}
			}
			if (this.CurrentlyVisibleForAbilityCast)
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
				flag = true;
			}
			else
			{
				flag = this.\u000E();
				flag = (flag && this.\u0012());
			}
		}
		return flag;
	}

	public bool \u000E(PlayerData \u001D, bool \u000E = true, bool \u0012 = false)
	{
		Team team = Team.TeamA;
		if (\u001D != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u000E(PlayerData, bool, bool)).MethodHandle;
			}
			if (\u0012)
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
				if (\u001D.ActorData != null)
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
					team = \u001D.ActorData.\u000E();
					goto IL_5E;
				}
			}
			team = \u001D.GetTeamViewing();
		}
		IL_5E:
		bool result;
		if (\u001D == null)
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
			result = false;
		}
		else
		{
			if (this.\u000E().IsInvisibleToEnemies(\u000E))
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
				if (this.\u000E() != team)
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
					if (\u001D.ActorData)
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
						if (\u001D.ActorData.\u000E().HasStatus(StatusType.SeeInvisible, \u000E))
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
							result = false;
							goto IL_E7;
						}
					}
					result = true;
					IL_E7:
					return result;
				}
			}
			result = false;
		}
		return result;
	}

	public bool \u000E(ActorData \u001D, bool \u000E = false)
	{
		if (this == \u001D)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u000E(ActorData, bool)).MethodHandle;
			}
			return true;
		}
		if (!NetworkServer.active && \u001D == GameFlowData.Get().activeOwnedActorData)
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
			return this.\u0018();
		}
		if (!NetworkServer.active)
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
			Log.Warning("Calling the server-only method ActorData::IsActorVisibleToActor on a client. Clients can only depend on ActorData::IsActorVisibleToClient.", new object[0]);
		}
		bool flag = this.\u000E(\u001D.PlayerData, true);
		bool flag2 = this.\u000E(\u001D.PlayerData, true, \u000E);
		bool result;
		if (flag)
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
			result = true;
		}
		else if (flag2)
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
			result = false;
		}
		else
		{
			FogOfWar fogOfWar = \u001D.\u000E();
			result = fogOfWar.IsVisible(this.\u000E());
		}
		return result;
	}

	public bool \u0009()
	{
		bool result = false;
		List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(this.\u0012());
		using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (!actorData.\u000E())
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0009()).MethodHandle;
					}
					if (this.\u000E(actorData, true))
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
						return true;
					}
				}
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
		return result;
	}

	public bool \u0015(ActorData \u001D)
	{
		bool flag = this.\u000E(\u001D.PlayerData, true);
		bool flag2 = this.\u000E(\u001D.PlayerData, true, true);
		bool result;
		if (flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0015(ActorData)).MethodHandle;
			}
			result = true;
		}
		else if (flag2)
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
			result = false;
		}
		else
		{
			bool flag3 = this.\u0018() < 0 || BrushRegion.HasTeamMemberInRegion(this.\u0012(), this.\u0018());
			bool flag4;
			if (this.\u0016())
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
				flag4 = flag3;
			}
			else
			{
				flag4 = true;
			}
			result = flag4;
		}
		return result;
	}

	public void ApplyForceFromPoint(Vector3 pos, float amount, Vector3 overrideDir)
	{
		Vector3 vector = this.\u000E("hip_JNT") - pos;
		if (vector.magnitude < 1.5f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.ApplyForceFromPoint(Vector3, float, Vector3)).MethodHandle;
			}
			if (overrideDir != Vector3.zero)
			{
				this.ApplyForce(overrideDir.normalized, amount);
			}
			else
			{
				this.ApplyForce(vector.normalized, amount);
			}
		}
	}

	public void ApplyForce(Vector3 dir, float amount)
	{
		Rigidbody rigidbody = this.\u000E();
		if (rigidbody)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.ApplyForce(Vector3, float)).MethodHandle;
			}
			rigidbody.AddForce(dir * amount, ForceMode.Impulse);
		}
	}

	public Vector3 \u000E(float \u001D)
	{
		if (Camera.main == null || this.m_nameplateJoint == null)
		{
			return default(Vector3);
		}
		Vector3 position = this.m_nameplateJoint.m_jointObject.transform.position;
		Vector3 b = Camera.main.WorldToScreenPoint(position);
		Vector3 a = Camera.main.WorldToScreenPoint(position + Camera.main.transform.up);
		Vector3 vector = a - b;
		vector.z = 0f;
		float d = \u001D / vector.magnitude;
		Vector3 b2 = Camera.main.transform.up * d;
		return position + b2;
	}

	public Vector3 \u0015()
	{
		return this.\u000E(this.\u000E());
	}

	public Vector3 \u0016()
	{
		return this.\u0012(this.\u000E());
	}

	public Vector3 \u000E(BoardSquare \u001D)
	{
		if (\u001D != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u000E(BoardSquare)).MethodHandle;
			}
			Vector3 result = this.\u0012(\u001D);
			result.y += BoardSquare.s_LoSHeightOffset;
			return result;
		}
		Log.Warning(string.Concat(new object[]
		{
			"Trying to get LoS check pos wrt a null square (IsDead: ",
			this.\u000E(),
			")  for ",
			this.DisplayName,
			" Code issue-- actor probably instantiated but not on Board yet."
		}), new object[0]);
		return this.m_actorMovement.transform.position;
	}

	public Vector3 \u0012(BoardSquare \u001D)
	{
		if (\u001D != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0012(BoardSquare)).MethodHandle;
			}
			return \u001D.\u001D();
		}
		Log.Warning(string.Concat(new object[]
		{
			"Trying to get free pos wrt a null square (IsDead: ",
			this.\u000E(),
			").  for ",
			this.DisplayName,
			" Code issue-- actor probably instantiated but not on Board yet."
		}), new object[0]);
		return this.m_actorMovement.transform.position;
	}

	public int \u0009()
	{
		int num = this.UnresolvedDamage + this.ClientUnresolvedDamage;
		int num2 = this.UnresolvedHealing + this.ClientUnresolvedHealing;
		int num3 = this.AbsorbPoints + this.ClientUnresolvedAbsorb;
		int num4 = Mathf.Max(0, num - num3);
		return Mathf.Clamp(this.HitPoints - num4 + num2, 0, this.\u0012());
	}

	public int \u000E(int \u001D)
	{
		int num = this.UnresolvedDamage + this.ClientUnresolvedDamage;
		int num2 = this.UnresolvedHealing + this.ClientUnresolvedHealing;
		int num3 = this.AbsorbPoints + this.ClientUnresolvedAbsorb;
		if (\u001D > 0)
		{
			num2 += \u001D;
		}
		else
		{
			num -= \u001D;
		}
		int num4 = Mathf.Max(0, num - num3);
		return Mathf.Clamp(this.HitPoints - num4 + num2, 0, this.\u0012());
	}

	public int \u0019()
	{
		int num = this.UnresolvedTechPointGain + this.ClientUnresolvedTechPointGain;
		int num2 = this.UnresolvedTechPointLoss + this.ClientUnresolvedTechPointLoss;
		return Mathf.Clamp(this.TechPoints + this.ReservedTechPoints + this.ClientReservedTechPoints + num - num2, 0, this.\u0016());
	}

	public int \u0011()
	{
		return Mathf.Max(0, this.ExpectedHoTTotal + this.ClientExpectedHoTTotalAdjust - this.ClientAppliedHoTThisTurn);
	}

	public int \u001A()
	{
		return Mathf.Max(0, this.ExpectedHoTThisTurn - this.ClientAppliedHoTThisTurn);
	}

	public string \u0015()
	{
		int num = this.\u0009();
		string text = string.Format("{0}", num);
		if (this.AbsorbPoints > 0)
		{
			int num2 = this.UnresolvedDamage + this.ClientUnresolvedDamage;
			int num3 = this.AbsorbPoints + this.ClientUnresolvedAbsorb;
			int num4 = Mathf.Max(0, num3 - num2);
			text += string.Format(" +{0} shield", num4);
		}
		return text;
	}

	public int \u0004()
	{
		int num = this.UnresolvedDamage + this.ClientUnresolvedDamage;
		int num2 = this.AbsorbPoints + this.ClientUnresolvedAbsorb;
		return Mathf.Max(0, num2 - num);
	}

	public bool \u0019()
	{
		if (!(GameFlow.Get() == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0019()).MethodHandle;
			}
			if (GameFlow.Get().playerDetails != null)
			{
				PlayerDetails playerDetails;
				return GameFlow.Get().playerDetails.TryGetValue(this.PlayerData.GetPlayer(), out playerDetails) && playerDetails.IsHumanControlled;
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
		Log.Error("Method called too early, results may be incorrect", new object[0]);
		return false;
	}

	public bool \u0011()
	{
		if (!GameplayUtils.IsPlayerControlled(this))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0011()).MethodHandle;
			}
			return false;
		}
		if (!GameplayUtils.IsBot(this))
		{
			return false;
		}
		PlayerDetails playerDetails;
		if (!GameFlow.Get().playerDetails.TryGetValue(this.PlayerData.GetPlayer(), out playerDetails))
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
		if (playerDetails == null)
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
			return false;
		}
		if (!playerDetails.m_botsMasqueradeAsHumans)
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
			return false;
		}
		return true;
	}

	public long \u000E()
	{
		PlayerDetails playerDetails;
		if (!GameFlow.Get().playerDetails.TryGetValue(this.PlayerData.GetPlayer(), out playerDetails))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u000E()).MethodHandle;
			}
			return -1L;
		}
		if (!this.\u0011() && !playerDetails.IsLoadTestBot && !this.\u0019())
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
			return -1L;
		}
		return playerDetails.m_accountId;
	}

	public long \u0012()
	{
		long result = -1L;
		if (this.PlayerData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0012()).MethodHandle;
			}
			if (GameFlow.Get().playerDetails.ContainsKey(this.PlayerData.GetPlayer()))
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
				result = GameFlow.Get().playerDetails[this.PlayerData.GetPlayer()].m_accountId;
			}
		}
		return result;
	}

	private void OnDisable()
	{
		if (HUD_UI.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.OnDisable()).MethodHandle;
			}
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.RemoveActor(this);
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.RemoveActor(this);
		}
		if (GameFlowData.Get())
		{
			GameFlowData.Get().RemoveFromTeam(this);
		}
	}

	public static bool \u001A
	{
		get
		{
			return false;
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		return this.OnSerializeHelper(new NetworkWriterAdapter(writer), initialState);
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint num = uint.MaxValue;
		if (!initialState)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			num = reader.ReadPackedUInt32();
		}
		if (num != 0U)
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
			this.OnSerializeHelper(new NetworkReaderAdapter(reader), initialState);
		}
		if (initialState)
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
			if (AsyncPump.Current != null)
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
				Log.Info("Waiting for objects to be created . . .", new object[0]);
				AsyncPump.Current.Break();
			}
			else
			{
				Log.Info("ActorData initialState without an async pump; something may be broken!", new object[0]);
			}
		}
	}

	private bool OnSerializeHelper(IBitStream stream, bool initialState)
	{
		if (!initialState && this.m_serializeHelper.ShouldReturnImmediately(ref stream))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.OnSerializeHelper(IBitStream, bool)).MethodHandle;
			}
			return false;
		}
		uint position = stream.Position;
		sbyte b = 0;
		sbyte b2 = 0;
		string displayName = this.m_displayName;
		float num = 0f;
		float num2 = 0f;
		sbyte b3 = (sbyte)ActorData.s_invalidActorIndex;
		bool flag = true;
		bool flag2 = false;
		bool flag3 = false;
		byte bitField = 0;
		sbyte b4 = 0;
		short hitPoints = 0;
		short num3 = 0;
		short num4 = 0;
		short techPoints = 0;
		short num5 = 0;
		short num6 = 0;
		short num7 = 0;
		short num8 = 0;
		short num9 = 0;
		short num10 = 0;
		short num11 = 0;
		byte bitField2 = 0;
		int lastDeathTurn = -1;
		int lastSpawnTurn = -1;
		int nextRespawnTurn = -1;
		sbyte b5 = -1;
		sbyte b6 = 0;
		int num12 = 0;
		short num13 = 0;
		short num14 = 0;
		bool flag4 = false;
		bool flag5 = true;
		bool flag6 = false;
		bool flag7 = false;
		bool flag8 = false;
		byte bitField3 = 0;
		if (stream.isWriting)
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
			b = (sbyte)this.PlayerIndex;
			b2 = (sbyte)this.ActorIndex;
			b4 = (sbyte)this.m_team;
			num12 = this.m_lastVisibleTurnToClient;
			if (this.ServerLastKnownPosSquare != null)
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
				num13 = (short)this.ServerLastKnownPosSquare.x;
				num14 = (short)this.ServerLastKnownPosSquare.y;
			}
			else
			{
				num13 = -1;
				num14 = -1;
			}
			num = this.RemainingHorizontalMovement;
			num2 = this.RemainingMovementWithQueuedAbility;
			flag = this.QueuedMovementAllowsAbility;
			flag2 = this.HasQueuedMovement();
			flag3 = this.HasQueuedChase();
			bitField = ServerClientUtils.CreateBitfieldFromBools(flag, flag2, flag3, false, false, false, false, false);
			b3 = (sbyte)GameplayUtils.GetActorIndexOfActor(this.\u0012());
			hitPoints = (short)this.HitPoints;
			num3 = (short)this.UnresolvedDamage;
			num4 = (short)this.UnresolvedHealing;
			techPoints = (short)this.TechPoints;
			num5 = (short)this.ReservedTechPoints;
			num6 = (short)this.UnresolvedTechPointGain;
			num7 = (short)this.UnresolvedTechPointLoss;
			num8 = (short)this.AbsorbPoints;
			num9 = (short)this.MechanicPoints;
			num10 = (short)this.ExpectedHoTTotal;
			num11 = (short)this.ExpectedHoTThisTurn;
			bool flag9 = num10 > 0 || num11 > 0;
			bitField2 = ServerClientUtils.CreateBitfieldFromBools(num3 > 0, num4 > 0, num6 > 0, num7 > 0, num5 != 0, num8 > 0, num9 > 0, flag9);
			lastDeathTurn = this.LastDeathTurn;
			lastSpawnTurn = this.m_lastSpawnTurn;
			nextRespawnTurn = this.NextRespawnTurn;
			b5 = (sbyte)this.SpawnerId;
			b6 = (sbyte)this.m_lineOfSightVisibleExceptions.Count;
			flag4 = this.HasBotController;
			flag5 = this.m_showInGameHud;
			flag6 = this.VisibleTillEndOfPhase;
			flag7 = this.m_ignoreFromAbilityHits;
			flag8 = this.m_alwaysHideNameplate;
			bitField3 = ServerClientUtils.CreateBitfieldFromBools(flag4, flag5, flag6, flag7, flag8, false, false, false);
			stream.Serialize(ref b);
			stream.Serialize(ref b2);
			stream.Serialize(ref displayName);
			stream.Serialize(ref b4);
			stream.Serialize(ref bitField);
			stream.Serialize(ref num);
			stream.Serialize(ref num2);
			stream.Serialize(ref b3);
			stream.Serialize(ref hitPoints);
			stream.Serialize(ref techPoints);
			stream.Serialize(ref bitField2);
			if (num3 > 0)
			{
				stream.Serialize(ref num3);
			}
			if (num4 > 0)
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
				stream.Serialize(ref num4);
			}
			if (num6 > 0)
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
				stream.Serialize(ref num6);
			}
			if (num7 > 0)
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
				stream.Serialize(ref num7);
			}
			if (num5 != 0)
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
				stream.Serialize(ref num5);
			}
			if (num8 > 0)
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
				stream.Serialize(ref num8);
			}
			if (num9 > 0)
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
				stream.Serialize(ref num9);
			}
			if (flag9)
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
				stream.Serialize(ref num10);
				stream.Serialize(ref num11);
			}
			stream.Serialize(ref lastDeathTurn);
			stream.Serialize(ref lastSpawnTurn);
			stream.Serialize(ref nextRespawnTurn);
			stream.Serialize(ref b5);
			stream.Serialize(ref bitField3);
			this.m_debugSerializeSizeBeforeVisualInfo = stream.Position - position;
			ActorData.SerializeCharacterVisualInfo(stream, ref this.m_visualInfo);
			ActorData.SerializeCharacterCardInfo(stream, ref this.m_selectedCards);
			ActorData.SerializeCharacterModInfo(stream, ref this.m_selectedMods);
			ActorData.SerializeCharacterAbilityVfxSwapInfo(stream, ref this.m_abilityVfxSwapInfo);
			this.m_debugSerializeSizeBeforeSpawnSquares = stream.Position - position;
			stream.Serialize(ref b6);
			using (List<ActorData>.Enumerator enumerator = this.m_lineOfSightVisibleExceptions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					short num15 = (short)actorData.ActorIndex;
					stream.Serialize(ref num15);
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
			stream.Serialize(ref num12);
			stream.Serialize(ref num13);
			stream.Serialize(ref num14);
		}
		if (stream.isReading)
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
			stream.Serialize(ref b);
			stream.Serialize(ref b2);
			stream.Serialize(ref displayName);
			stream.Serialize(ref b4);
			stream.Serialize(ref bitField);
			ServerClientUtils.GetBoolsFromBitfield(bitField, out flag, out flag2, out flag3);
			stream.Serialize(ref num);
			stream.Serialize(ref num2);
			stream.Serialize(ref b3);
			stream.Serialize(ref hitPoints);
			stream.Serialize(ref techPoints);
			stream.Serialize(ref bitField2);
			bool flag10 = false;
			bool flag11 = false;
			bool flag12 = false;
			bool flag13 = false;
			bool flag14 = false;
			bool flag15 = false;
			bool flag16 = false;
			bool flag17 = false;
			ServerClientUtils.GetBoolsFromBitfield(bitField2, out flag10, out flag11, out flag12, out flag13, out flag14, out flag15, out flag16, out flag17);
			if (flag10)
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
				stream.Serialize(ref num3);
			}
			if (flag11)
			{
				stream.Serialize(ref num4);
			}
			if (flag12)
			{
				stream.Serialize(ref num6);
			}
			if (flag13)
			{
				stream.Serialize(ref num7);
			}
			if (flag14)
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
				stream.Serialize(ref num5);
			}
			if (flag15)
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
				stream.Serialize(ref num8);
			}
			if (flag16)
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
				stream.Serialize(ref num9);
			}
			if (flag17)
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
				stream.Serialize(ref num10);
				stream.Serialize(ref num11);
			}
			stream.Serialize(ref lastDeathTurn);
			stream.Serialize(ref lastSpawnTurn);
			stream.Serialize(ref nextRespawnTurn);
			stream.Serialize(ref b5);
			stream.Serialize(ref bitField3);
			ServerClientUtils.GetBoolsFromBitfield(bitField3, out flag4, out flag5, out flag6, out flag7, out flag8);
			ActorData.SerializeCharacterVisualInfo(stream, ref this.m_visualInfo);
			ActorData.SerializeCharacterCardInfo(stream, ref this.m_selectedCards);
			ActorData.SerializeCharacterModInfo(stream, ref this.m_selectedMods);
			ActorData.SerializeCharacterAbilityVfxSwapInfo(stream, ref this.m_abilityVfxSwapInfo);
			this.PlayerIndex = (int)b;
			this.ActorIndex = (int)b2;
			Team team = this.m_team;
			this.m_team = (Team)b4;
			this.SpawnerId = (int)b5;
			if (this.m_actorSkinPrefabLink != null)
			{
				if (!(this.m_actorModelData == null))
				{
					goto IL_8B4;
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
			PrefabResourceLink prefabResourceLink = null;
			CharacterResourceLink characterResourceLink = null;
			if (this.m_characterType > CharacterType.None)
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
				characterResourceLink = GameWideData.Get().GetCharacterResourceLink(this.m_characterType);
			}
			if (characterResourceLink == null)
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
				if (NPCCoordinator.Get() != null)
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
					characterResourceLink = NPCCoordinator.Get().GetNpcCharacterResourceLinkBySpawnerId((int)b5);
				}
			}
			if (characterResourceLink != null)
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
				CharacterSkin characterSkin;
				prefabResourceLink = characterResourceLink.GetHeroPrefabLinkFromSelection(this.m_visualInfo, out characterSkin);
			}
			if (prefabResourceLink != null)
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
				if (!prefabResourceLink.IsEmpty)
				{
					GameObject prefab = prefabResourceLink.GetPrefab(true);
					if (prefab == null && !this.m_visualInfo.IsDefaultSelection())
					{
						CharacterVisualInfo visualInfo = this.m_visualInfo;
						visualInfo.ResetToDefault();
						CharacterSkin characterSkin2;
						prefabResourceLink = characterResourceLink.GetHeroPrefabLinkFromSelection(visualInfo, out characterSkin2);
					}
					bool addMasterSkinVfx = false;
					if (MasterSkinVfxData.Get() != null)
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
						if (MasterSkinVfxData.Get().m_addMasterSkinVfx)
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
							if (characterResourceLink.IsVisualInfoSelectionValid(this.m_visualInfo))
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
								CharacterColor characterColor = characterResourceLink.GetCharacterColor(this.m_visualInfo);
								addMasterSkinVfx = (characterColor.m_styleLevel == StyleLevelType.Mastery);
							}
						}
					}
					this.InitializeModel(prefabResourceLink, addMasterSkinVfx);
					goto IL_8B4;
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
			Log.Error(string.Concat(new object[]
			{
				"Failed to find character resource link for ",
				this.m_characterType,
				" with visual info ",
				this.m_visualInfo.ToString()
			}), new object[0]);
			GameObject gameObject = GameFlowData.Get().m_availableCharacterResourceLinkPrefabs[0];
			CharacterResourceLink component = gameObject.GetComponent<CharacterResourceLink>();
			this.InitializeModel(component.m_skins[0].m_patterns[0].m_colors[0].m_heroPrefab, false);
			IL_8B4:
			this.m_displayName = displayName;
			if (initialState)
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
				TeamSensitiveDataMatchmaker.Get().SetTeamSensitiveDataForActor(this);
			}
			stream.Serialize(ref b6);
			this.m_lineOfSightVisibleExceptions.Clear();
			for (int i = 0; i < (int)b6; i++)
			{
				sbyte b7 = 0;
				stream.Serialize(ref b7);
				ActorData actorData2 = GameFlowData.Get().FindActorByActorIndex((int)b7);
				if (actorData2 != null)
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
					this.m_lineOfSightVisibleExceptions.Add(actorData2);
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
			stream.Serialize(ref num12);
			stream.Serialize(ref num13);
			stream.Serialize(ref num14);
			if (num12 > this.m_lastVisibleTurnToClient)
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
				this.m_lastVisibleTurnToClient = num12;
			}
			if (num13 == -1)
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
				if (num14 == -1)
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
					this.ServerLastKnownPosSquare = null;
					goto IL_9B6;
				}
			}
			this.ServerLastKnownPosSquare = Board.\u000E().\u0016((int)num13, (int)num14);
			IL_9B6:
			this.m_ignoreFromAbilityHits = flag7;
			this.m_alwaysHideNameplate = flag8;
			this.\u000E().MarkForRecalculateVisibility();
			this.m_showInGameHud = flag5;
			this.VisibleTillEndOfPhase = flag6;
			if (this.m_setTeam)
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
				if (team == this.m_team)
				{
					goto IL_ACC;
				}
			}
			if (GameFlowData.Get() != null)
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
				GameFlowData.Get().AddToTeam(this);
			}
			else
			{
				this.m_needAddToTeam = true;
			}
			if (TeamStatusDisplay.GetTeamStatusDisplay() != null)
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
				TeamStatusDisplay.GetTeamStatusDisplay().RebuildTeamDisplay();
			}
			if (GameplayUtils.IsMinion(base.gameObject))
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
				if (MinionManager.Get() != null)
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
					if (this.m_setTeam)
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
						MinionManager.Get().RemoveMinion(this);
						MinionManager.Get().AddMinion(this);
					}
					else
					{
						MinionManager.Get().AddMinion(this);
					}
				}
			}
			this.m_setTeam = true;
			IL_ACC:
			this.UnresolvedDamage = (int)num3;
			this.UnresolvedHealing = (int)num4;
			this.ReservedTechPoints = (int)num5;
			this.UnresolvedTechPointGain = (int)num6;
			this.UnresolvedTechPointLoss = (int)num7;
			this.LastDeathTurn = lastDeathTurn;
			this.m_lastSpawnTurn = lastSpawnTurn;
			this.NextRespawnTurn = nextRespawnTurn;
			this.HasBotController = flag4;
			this.AbsorbPoints = (int)num8;
			this.TechPoints = (int)techPoints;
			this.HitPoints = (int)hitPoints;
			this.MechanicPoints = (int)num9;
			this.ExpectedHoTTotal = (int)num10;
			this.ExpectedHoTThisTurn = (int)num11;
			bool flag18 = false;
			if (num != this.RemainingHorizontalMovement)
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
				this.RemainingHorizontalMovement = num;
				flag18 = true;
			}
			if (num2 != this.RemainingMovementWithQueuedAbility)
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
				this.RemainingMovementWithQueuedAbility = num2;
				flag18 = true;
			}
			this.QueuedMovementAllowsAbility = flag;
			if (this.m_queuedMovementRequest != flag2)
			{
				this.m_queuedMovementRequest = flag2;
				flag18 = true;
			}
			if (this.m_queuedChaseRequest != flag3)
			{
				this.m_queuedChaseRequest = flag3;
				flag18 = true;
			}
			ActorData actorOfActorIndex = GameplayUtils.GetActorOfActorIndex((int)b3);
			if (this.m_queuedChaseTarget != actorOfActorIndex)
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
				this.m_queuedChaseTarget = actorOfActorIndex;
			}
			if (flag18)
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
				this.m_actorMovement.UpdateSquaresCanMoveTo();
			}
		}
		return this.m_serializeHelper.End(initialState, base.syncVarDirtyBits);
	}

	public static void SerializeCharacterVisualInfo(IBitStream stream, ref CharacterVisualInfo visualInfo)
	{
		sbyte b = (sbyte)visualInfo.skinIndex;
		sbyte b2 = (sbyte)visualInfo.patternIndex;
		short colorIndex = (short)visualInfo.colorIndex;
		stream.Serialize(ref b);
		stream.Serialize(ref b2);
		stream.Serialize(ref colorIndex);
		if (stream.isReading)
		{
			visualInfo.skinIndex = (int)b;
			visualInfo.patternIndex = (int)b2;
			visualInfo.colorIndex = (int)colorIndex;
		}
	}

	public unsafe static void SerializeCharacterAbilityVfxSwapInfo(IBitStream stream, ref CharacterAbilityVfxSwapInfo abilityVfxSwapInfo)
	{
		short vfxSwapForAbility = (short)abilityVfxSwapInfo.VfxSwapForAbility0;
		short vfxSwapForAbility2 = (short)abilityVfxSwapInfo.VfxSwapForAbility1;
		short vfxSwapForAbility3 = (short)abilityVfxSwapInfo.VfxSwapForAbility2;
		short vfxSwapForAbility4 = (short)abilityVfxSwapInfo.VfxSwapForAbility3;
		short vfxSwapForAbility5 = (short)abilityVfxSwapInfo.VfxSwapForAbility4;
		stream.Serialize(ref vfxSwapForAbility);
		stream.Serialize(ref vfxSwapForAbility2);
		stream.Serialize(ref vfxSwapForAbility3);
		stream.Serialize(ref vfxSwapForAbility4);
		stream.Serialize(ref vfxSwapForAbility5);
		if (stream.isReading)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.SerializeCharacterAbilityVfxSwapInfo(IBitStream, CharacterAbilityVfxSwapInfo*)).MethodHandle;
			}
			abilityVfxSwapInfo.VfxSwapForAbility0 = (int)vfxSwapForAbility;
			abilityVfxSwapInfo.VfxSwapForAbility1 = (int)vfxSwapForAbility2;
			abilityVfxSwapInfo.VfxSwapForAbility2 = (int)vfxSwapForAbility3;
			abilityVfxSwapInfo.VfxSwapForAbility3 = (int)vfxSwapForAbility4;
			abilityVfxSwapInfo.VfxSwapForAbility4 = (int)vfxSwapForAbility5;
		}
	}

	public unsafe static void SerializeCharacterCardInfo(IBitStream stream, ref CharacterCardInfo cardInfo)
	{
		sbyte b = 0;
		sbyte b2 = 0;
		sbyte b3 = 0;
		if (stream.isWriting)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.SerializeCharacterCardInfo(IBitStream, CharacterCardInfo*)).MethodHandle;
			}
			b = (sbyte)cardInfo.PrepCard;
			b2 = (sbyte)cardInfo.DashCard;
			b3 = (sbyte)cardInfo.CombatCard;
			stream.Serialize(ref b);
			stream.Serialize(ref b2);
			stream.Serialize(ref b3);
		}
		else
		{
			stream.Serialize(ref b);
			stream.Serialize(ref b2);
			stream.Serialize(ref b3);
			cardInfo.PrepCard = (CardType)b;
			cardInfo.DashCard = (CardType)b2;
			cardInfo.CombatCard = (CardType)b3;
		}
	}

	public static void SerializeCharacterModInfo(IBitStream stream, ref CharacterModInfo modInfo)
	{
		sbyte b = -1;
		sbyte b2 = -1;
		sbyte b3 = -1;
		sbyte b4 = -1;
		sbyte b5 = -1;
		if (stream.isWriting)
		{
			b = (sbyte)modInfo.ModForAbility0;
			b2 = (sbyte)modInfo.ModForAbility1;
			b3 = (sbyte)modInfo.ModForAbility2;
			b4 = (sbyte)modInfo.ModForAbility3;
			b5 = (sbyte)modInfo.ModForAbility4;
			stream.Serialize(ref b);
			stream.Serialize(ref b2);
			stream.Serialize(ref b3);
			stream.Serialize(ref b4);
			stream.Serialize(ref b5);
		}
		else
		{
			stream.Serialize(ref b);
			stream.Serialize(ref b2);
			stream.Serialize(ref b3);
			stream.Serialize(ref b4);
			stream.Serialize(ref b5);
			modInfo.ModForAbility0 = (int)b;
			modInfo.ModForAbility1 = (int)b2;
			modInfo.ModForAbility2 = (int)b3;
			modInfo.ModForAbility3 = (int)b4;
			modInfo.ModForAbility4 = (int)b5;
		}
	}

	public GridPos \u000E()
	{
		GridPos result = default(GridPos);
		if (this.\u0012())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u000E()).MethodHandle;
			}
			result = this.\u0012().\u001D();
			result.height++;
		}
		return result;
	}

	public bool CanMoveToBoardSquare(int x, int y)
	{
		return this.m_actorMovement.CanMoveToBoardSquare(x, y);
	}

	public bool CanMoveToBoardSquare(BoardSquare dest)
	{
		return this.m_actorMovement.CanMoveToBoardSquare(dest);
	}

	public void ClearFacingDirectionAfterMovement()
	{
		this.SetFacingDirectionAfterMovement(Vector3.zero);
	}

	public void SetFacingDirectionAfterMovement(Vector3 facingDirAfterMovement)
	{
		if (this.m_facingDirAfterMovement != facingDirAfterMovement)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.SetFacingDirectionAfterMovement(Vector3)).MethodHandle;
			}
			this.m_facingDirAfterMovement = facingDirAfterMovement;
			if (NetworkServer.active)
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
				if (this.m_teamSensitiveData_friendly != null)
				{
					this.m_teamSensitiveData_friendly.FacingDirAfterMovement = this.m_facingDirAfterMovement;
				}
				if (this.m_teamSensitiveData_hostile != null)
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
					this.m_teamSensitiveData_hostile.FacingDirAfterMovement = this.m_facingDirAfterMovement;
				}
			}
		}
	}

	public Vector3 \u0013()
	{
		return this.m_facingDirAfterMovement;
	}

	public void OnMovementWhileDisappeared(ActorData.MovementType movementType)
	{
		UnityEngine.Debug.Log(this.\u0018() + ": calling OnMovementWhileDisappeared.");
		if (ClientMovementManager.Get() != null)
		{
			ClientMovementManager.Get().OnActorMoveStart_Disappeared(this, movementType);
		}
		if (this.\u0012() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.OnMovementWhileDisappeared(ActorData.MovementType)).MethodHandle;
			}
			if (this.\u0012().occupant == base.gameObject)
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
				this.UnoccupyCurrentBoardSquare();
			}
		}
		this.m_actorMovement.ClearPath();
		this.SetCurrentBoardSquare(null);
		this.\u000E().MarkForRecalculateVisibility();
	}

	public void MoveToBoardSquareLocal(BoardSquare dest, ActorData.MovementType movementType, BoardSquarePathInfo path, bool moverWillDisappear)
	{
		this.m_disappearingAfterCurrentMovement = moverWillDisappear;
		if (dest == null)
		{
			if (moverWillDisappear)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.MoveToBoardSquareLocal(BoardSquare, ActorData.MovementType, BoardSquarePathInfo, bool)).MethodHandle;
				}
				if (path == null)
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
					this.UnoccupyCurrentBoardSquare();
					this.SetCurrentBoardSquare(null);
					this.ForceUpdateIsVisibleToClientCache();
					this.ForceUpdateActorModelVisibility();
					goto IL_80;
				}
			}
			string message = string.Format("Actor {0} in MoveToBoardSquare has null destination (movementType = {1})", this.DisplayName, movementType.ToString());
			Log.Error(message, new object[0]);
			IL_80:;
		}
		else if (path == null && movementType != ActorData.MovementType.Teleport)
		{
			string message2 = string.Format("Actor {0} in MoveToBoardSquare has null path (movementType = {1})", this.DisplayName, movementType.ToString());
			Log.Error(message2, new object[0]);
		}
		else
		{
			if (ServerClientUtils.GetCurrentAbilityPhase() == AbilityPriority.Evasion)
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
				this.MovedForEvade = true;
			}
			bool flag;
			if (path != null)
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
				flag = path.WillDieAtEnd();
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			BoardSquare dest2;
			if (path != null && path.GetPathEndpoint() != null)
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
				if (path.GetPathEndpoint().square != null)
				{
					dest2 = path.GetPathEndpoint().square;
					goto IL_131;
				}
			}
			dest2 = dest;
			IL_131:
			if (ClientMovementManager.Get() != null)
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
				ClientMovementManager.Get().OnActorMoveStart_ClientMovementManager(this, dest2, movementType, path);
				ClientResolutionManager.Get().OnActorMoveStart_ClientResolutionManager(this, path);
			}
			if (this.\u0012() != null)
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
				if (this.\u0012().occupant == base.gameObject)
				{
					this.UnoccupyCurrentBoardSquare();
				}
			}
			BoardSquare currentBoardSquare = this.CurrentBoardSquare;
			if (movementType == ActorData.MovementType.Teleport)
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
				this.m_actorMovement.ClearPath();
			}
			if (!flag2)
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
				if (!moverWillDisappear)
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
					this.SetCurrentBoardSquare(dest);
					if (this.\u0012() != null)
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
						this.OccupyCurrentBoardSquare();
					}
					goto IL_21E;
				}
			}
			this.SetCurrentBoardSquare(null);
			this.SetMostRecentDeathSquare(dest);
			IL_21E:
			if (movementType == ActorData.MovementType.Teleport)
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
				this.ForceUpdateIsVisibleToClientCache();
				this.ForceUpdateActorModelVisibility();
				this.SetTransformPositionToSquare(dest);
				this.m_actorMovement.ClearPath();
				if (this.m_actorCover)
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
					this.m_actorCover.RecalculateCover();
				}
				this.UpdateFacingAfterMovement();
				if (currentBoardSquare != null)
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
					BoardSquarePathInfo boardSquarePathInfo = MovementUtils.Build2PointTeleportPath(currentBoardSquare, dest);
					boardSquarePathInfo = boardSquarePathInfo.next;
					if (ClientClashManager.Get() != null)
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
						ClientClashManager.Get().OnActorMoved_ClientClashManager(this, boardSquarePathInfo);
					}
					if (ClientResolutionManager.Get() != null)
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
						ClientResolutionManager.Get().OnActorMoved_ClientResolutionManager(this, boardSquarePathInfo);
					}
				}
			}
			else
			{
				if (movementType != ActorData.MovementType.Normal)
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
					if (movementType != ActorData.MovementType.Flight)
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
						if (movementType == ActorData.MovementType.WaypointFlight)
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
						}
						else
						{
							if (movementType != ActorData.MovementType.Knockback)
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
								if (movementType != ActorData.MovementType.Charge)
								{
									goto IL_458;
								}
							}
							if (this.m_actorCover)
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
								this.m_actorCover.DisableCover();
							}
							this.m_actorMovement.BeginChargeOrKnockback(currentBoardSquare, dest, path, movementType);
							this.m_actorMovement.UpdatePosition();
							if (!flag2)
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
								if (!moverWillDisappear)
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
									if (path.square == dest)
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
										if (path.next == null)
										{
											this.UpdateFacingAfterMovement();
										}
									}
								}
							}
							if (movementType == ActorData.MovementType.Knockback)
							{
								this.KnockbackMoveStarted = true;
								goto IL_458;
							}
							goto IL_458;
						}
					}
				}
				if (this.m_actorCover)
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
					this.m_actorCover.DisableCover();
				}
				if (path == null)
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
					path = new BoardSquarePathInfo();
					path.square = currentBoardSquare;
					path.prev = null;
					path.next = new BoardSquarePathInfo();
					path.next.square = dest;
					path.next.prev = path;
					path.next.next = null;
				}
				this.m_actorMovement.BeginTravellingAlongPath(path, movementType);
				this.m_actorMovement.UpdatePosition();
			}
			IL_458:
			this.m_actorMovement.UpdateSquaresCanMoveTo();
			this.\u000E().MarkForRecalculateVisibility();
		}
	}

	public void AppearAtBoardSquare(BoardSquare dest)
	{
		if (dest == null)
		{
			string message = string.Format("Actor {0} in AppearAtBoardSquare has null destination)", this.DisplayName);
			Log.Error(message, new object[0]);
		}
		else
		{
			if (this.\u0012() != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.AppearAtBoardSquare(BoardSquare)).MethodHandle;
				}
				if (this.\u0012().occupant == base.gameObject)
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
					this.UnoccupyCurrentBoardSquare();
				}
			}
			this.SetCurrentBoardSquare(dest);
			this.SetTransformPositionToSquare(dest);
			if (this.\u0012() != null)
			{
				this.OccupyCurrentBoardSquare();
			}
		}
	}

	public void SetTransformPositionToSquare(BoardSquare refSquare)
	{
		if (refSquare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.SetTransformPositionToSquare(BoardSquare)).MethodHandle;
			}
			Vector3 transformPositionToVector = refSquare.\u001D();
			this.SetTransformPositionToVector(transformPositionToVector);
		}
	}

	public void SetTransformPositionToVector(Vector3 newPos)
	{
		if (base.transform.position != newPos)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.SetTransformPositionToVector(Vector3)).MethodHandle;
			}
			BoardSquare boardSquare = Board.\u000E().\u000E(base.transform.position);
			BoardSquare y = Board.\u000E().\u000E(newPos);
			if (boardSquare != y && boardSquare != null)
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
				this.PreviousBoardSquarePosition = boardSquare.ToVector3();
			}
			base.transform.position = newPos;
		}
	}

	public void UnoccupyCurrentBoardSquare()
	{
		if (this.\u0012() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.UnoccupyCurrentBoardSquare()).MethodHandle;
			}
			if (this.\u0012().occupant == base.gameObject)
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
				this.\u0012().occupant = null;
			}
		}
	}

	public BoardSquare \u000E()
	{
		return this.m_actorMovement.GetTravelBoardSquare();
	}

	public BoardSquare \u0012()
	{
		return this.CurrentBoardSquare;
	}

	public BoardSquare \u0015()
	{
		return this.m_mostRecentDeathSquare;
	}

	public void SetMostRecentDeathSquare(BoardSquare square)
	{
		this.m_mostRecentDeathSquare = square;
	}

	public void OccupyCurrentBoardSquare()
	{
		if (this.\u0012() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.OccupyCurrentBoardSquare()).MethodHandle;
			}
			this.\u0012().occupant = base.gameObject;
		}
	}

	private void SetCurrentBoardSquare(BoardSquare square)
	{
		if (square == this.CurrentBoardSquare)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.SetCurrentBoardSquare(BoardSquare)).MethodHandle;
			}
			return;
		}
		this.m_clientCurrentBoardSquare = square;
		Animator animator = this.\u000E();
		if (animator != null)
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
			bool[] array;
			animator.SetBool("Cover", ActorCover.CalcCoverLevelGeoOnly(out array, this.CurrentBoardSquare));
		}
		if (this.MoveFromBoardSquare == null)
		{
			this.MoveFromBoardSquare = square;
		}
		this.InitialMoveStartSquare = square;
	}

	public void ClearCurrentBoardSquare()
	{
		if (this.CurrentBoardSquare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.ClearCurrentBoardSquare()).MethodHandle;
			}
			this.UnoccupyCurrentBoardSquare();
		}
		this.m_clientCurrentBoardSquare = null;
		this.MoveFromBoardSquare = null;
	}

	public void ClearPreviousMovementInfo()
	{
		if (NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.ClearPreviousMovementInfo()).MethodHandle;
			}
			if (this.m_teamSensitiveData_friendly != null)
			{
				this.m_teamSensitiveData_friendly.ClearPreviousMovementInfo();
			}
			if (this.m_teamSensitiveData_hostile != null)
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
				this.m_teamSensitiveData_hostile.ClearPreviousMovementInfo();
			}
		}
	}

	public void SetClientFriendlyTeamSensitiveData(ActorTeamSensitiveData friendlyTSD)
	{
		if (this.m_teamSensitiveData_friendly != friendlyTSD)
		{
			Log.Info("Setting Friendly TeamSensitiveData for " + this.\u0018(), new object[0]);
			this.m_teamSensitiveData_friendly = friendlyTSD;
			this.m_teamSensitiveData_friendly.OnClientAssociatedWithActor(this);
		}
	}

	public void SetClientHostileTeamSensitiveData(ActorTeamSensitiveData hostileTSD)
	{
		if (this.m_teamSensitiveData_hostile != hostileTSD)
		{
			Log.Info("Setting Hostile TeamSensitiveData for " + this.\u0018(), new object[0]);
			this.m_teamSensitiveData_hostile = hostileTSD;
			this.m_teamSensitiveData_hostile.OnClientAssociatedWithActor(this);
		}
	}

	public void UpdateFacingAfterMovement()
	{
		if (this.m_facingDirAfterMovement != Vector3.zero)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.UpdateFacingAfterMovement()).MethodHandle;
			}
			this.TurnToDirection(this.m_facingDirAfterMovement);
		}
	}

	public void SetTeam(Team team)
	{
		this.m_team = team;
		GameFlowData.Get().AddToTeam(this);
		TeamStatusDisplay.GetTeamStatusDisplay().RebuildTeamDisplay();
		if (NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.SetTeam(Team)).MethodHandle;
			}
		}
	}

	public Team \u000E()
	{
		return this.m_team;
	}

	public Team \u0012()
	{
		if (this.m_team == Team.TeamA)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0012()).MethodHandle;
			}
			return Team.TeamB;
		}
		if (this.m_team == Team.TeamB)
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
			return Team.TeamA;
		}
		return Team.Objects;
	}

	public List<Team> \u000E()
	{
		return GameplayUtils.GetOtherTeamsThan(this.m_team);
	}

	public List<Team> \u0012()
	{
		return new List<Team>
		{
			this.\u000E()
		};
	}

	public List<Team> \u0015()
	{
		return new List<Team>
		{
			this.\u0012()
		};
	}

	public string \u0016()
	{
		string result;
		if (this.m_team == Team.TeamA)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0016()).MethodHandle;
			}
			result = "Blue";
		}
		else
		{
			result = "Orange";
		}
		return result;
	}

	public string \u0013()
	{
		return (this.m_team != Team.TeamA) ? "Blue" : "Orange";
	}

	public Color \u000E()
	{
		Color result;
		if (this.m_team == Team.TeamA)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u000E()).MethodHandle;
			}
			result = ActorData.s_teamAColor;
		}
		else
		{
			result = ActorData.s_teamBColor;
		}
		return result;
	}

	public Color \u0012()
	{
		Color result;
		if (this.m_team == Team.TeamA)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0012()).MethodHandle;
			}
			result = ActorData.s_teamBColor;
		}
		else
		{
			result = ActorData.s_teamAColor;
		}
		return result;
	}

	public Color \u000E(Team \u001D)
	{
		Color result;
		if (\u001D == this.\u000E())
		{
			result = ActorData.s_friendlyPlayerColor;
		}
		else
		{
			result = ActorData.s_hostilePlayerColor;
		}
		return result;
	}

	internal void OnTurnTick()
	{
		this.CurrentlyVisibleForAbilityCast = false;
		this.MovedForEvade = false;
		this.m_actorMovement.ClearPath();
		this.\u000E().MarkForRecalculateVisibility();
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.OnTurnTick()).MethodHandle;
			}
			if (this.m_serverMovementWaitForEvent != GameEventManager.EventType.Invalid)
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
				if (this.m_serverMovementDestination != this.\u0012() && !this.\u000E())
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
					this.MoveToBoardSquareLocal(this.m_serverMovementDestination, ActorData.MovementType.Teleport, this.m_serverMovementPath, false);
				}
			}
		}
		if (this.\u000E() != null)
		{
			Animator animator = this.\u000E();
			if (animator != null)
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
				if (this.\u000E().HasTurnStartParameter())
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
					animator.SetBool("TurnStart", true);
				}
				animator.SetInteger("Attack", 0);
				animator.SetBool("CinematicCam", false);
			}
		}
		if (NetworkClient.active)
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
			if (this.ClientUnresolvedDamage != 0)
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
				Log.Error("ClientUnresolvedDamage not cleared on TurnTick for " + this.\u0018(), new object[0]);
				this.ClientUnresolvedDamage = 0;
			}
			if (this.ClientUnresolvedHealing != 0)
			{
				Log.Error("ClientUnresolvedHealing not cleared on TurnTick for " + this.\u0018(), new object[0]);
				this.ClientUnresolvedHealing = 0;
			}
			if (this.ClientUnresolvedTechPointGain != 0)
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
				this.ClientUnresolvedTechPointGain = 0;
			}
			if (this.ClientUnresolvedTechPointLoss != 0)
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
				this.ClientUnresolvedTechPointLoss = 0;
			}
			if (this.ClientReservedTechPoints != 0)
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
				this.ClientReservedTechPoints = 0;
			}
			if (this.ClientUnresolvedAbsorb != 0)
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
				Log.Error("ClientUnresolvedAbsorb not cleared on TurnTick for " + this.\u0018(), new object[0]);
				this.ClientUnresolvedAbsorb = 0;
			}
			this.ClientExpectedHoTTotalAdjust = 0;
			this.ClientAppliedHoTThisTurn = 0;
			this.SynchClientLastKnownPosToServerLastKnownPos();
			if (this.\u000E() != null)
			{
				this.\u000E().OnTurnTick();
			}
			PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
			if (localPlayerData != null && HighlightUtils.Get().m_recentlySpawnedShader != null)
			{
				int currentTurn = GameFlowData.Get().CurrentTurn;
				if (currentTurn == 1)
				{
					TricksterAfterImageNetworkBehaviour.InitializeAfterImageMaterial(this.\u000E(), GameFlowData.Get().LocalPlayerData.GetTeamViewing() == this.\u000E(), 0.5f, HighlightUtils.Get().m_recentlySpawnedShader, false);
				}
				else
				{
					if (currentTurn != 2)
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
						if (currentTurn <= 2)
						{
							goto IL_2ED;
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
						if (currentTurn != this.NextRespawnTurn + 1)
						{
							goto IL_2ED;
						}
					}
					TricksterAfterImageNetworkBehaviour.DisableAfterImageMaterial(this.\u000E());
				}
			}
		}
		IL_2ED:
		this.m_actorVFX.OnTurnTick();
		this.m_wasUpdatingForConfirmedTargeting = false;
		this.KnockbackMoveStarted = false;
		if (this.\u000E() != null)
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
			this.\u000E().Client_ResetKillAssistContribution();
		}
		if (this.\u000E() != null)
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
			this.\u000E().RecalculateCover();
			if (!this.\u000E())
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
				if (GameFlowData.Get() != null)
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
					if (GameFlowData.Get().CurrentTurn > 1)
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
						if (HighlightUtils.Get() != null)
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
							if (HighlightUtils.Get().m_coverDirIndicatorTiming == HighlightUtils.MoveIntoCoverIndicatorTiming.ShowOnTurnStart)
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
								if (HighlightUtils.Get().m_showMoveIntoCoverIndicators)
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
									ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
									if (activeOwnedActorData != null && activeOwnedActorData == this)
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
										if (this.\u0018())
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
											this.\u000E().StartShowMoveIntoCoverIndicator();
										}
									}
								}
							}
						}
					}
				}
			}
		}
		if (this.OnTurnStartDelegates != null)
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
			this.OnTurnStartDelegates();
		}
	}

	public bool HasQueuedMovement()
	{
		bool result;
		if (!this.m_queuedMovementRequest)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.HasQueuedMovement()).MethodHandle;
			}
			result = this.m_queuedChaseRequest;
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool HasQueuedChase()
	{
		return this.m_queuedChaseRequest;
	}

	public ActorData \u0012()
	{
		return this.m_queuedChaseTarget;
	}

	public void TurnToDirection(Vector3 dir)
	{
		Quaternion quaternion = Quaternion.LookRotation(dir);
		if (Quaternion.Angle(quaternion, this.m_targetRotation.GetEndValue()) > 0.01f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.TurnToDirection(Vector3)).MethodHandle;
			}
			base.transform.localRotation = this.m_targetRotation.GetEndValue();
			this.m_targetRotation.EaseTo(quaternion, 0.1f);
		}
	}

	public void TurnToPosition(Vector3 position, float turnDuration = 0.2f)
	{
		Vector3 vector = position - base.transform.position;
		vector.y = 0f;
		if (vector != Vector3.zero)
		{
			base.transform.localRotation = this.m_targetRotation.GetEndValue();
			Quaternion quaternion = default(Quaternion);
			quaternion.SetLookRotation(vector);
			if (Quaternion.Angle(quaternion, this.m_targetRotation.GetEndValue()) > 0.01f)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.TurnToPosition(Vector3, float)).MethodHandle;
				}
				this.m_targetRotation.EaseTo(quaternion, turnDuration);
			}
		}
	}

	public float \u0016()
	{
		return this.m_targetRotation.CalcTimeRemaining();
	}

	public void TurnToPositionInstant(Vector3 position)
	{
		Vector3 vector = position - base.transform.position;
		vector.y = 0f;
		if (vector != Vector3.zero)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.TurnToPositionInstant(Vector3)).MethodHandle;
			}
			Quaternion quaternion = default(Quaternion);
			quaternion.SetLookRotation(vector);
			base.transform.localRotation = quaternion;
			this.m_targetRotation.SnapTo(quaternion);
		}
	}

	public Rigidbody \u000E(string \u001D)
	{
		Rigidbody result = null;
		GameObject gameObject = base.gameObject.FindInChildren(\u001D, 0);
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u000E(string)).MethodHandle;
			}
			result = gameObject.GetComponentInChildren<Rigidbody>();
		}
		else
		{
			Log.Warning(string.Format("GetRigidBody trying to find body of bone {0} on actor '{1}' (obj name '{2}'), but the bone cannot be found.", \u001D, this.DisplayName, base.gameObject.name), new object[0]);
		}
		return result;
	}

	public Rigidbody \u000E()
	{
		if (this.m_cachedHipJoint == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u000E()).MethodHandle;
			}
			this.m_cachedHipJoint = this.\u000E("hip_JNT");
		}
		return this.m_cachedHipJoint;
	}

	public Vector3 \u0018()
	{
		Rigidbody rigidbody = this.\u000E();
		if (rigidbody != null)
		{
			return rigidbody.transform.position;
		}
		return base.gameObject.transform.position;
	}

	public Vector3 \u000E(string \u001D)
	{
		Vector3 result = Vector3.zero;
		GameObject gameObject = base.gameObject.FindInChildren(\u001D, 0);
		if (gameObject)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u000E(string)).MethodHandle;
			}
			result = gameObject.transform.position;
		}
		else
		{
			result = base.gameObject.transform.position;
		}
		return result;
	}

	public Quaternion \u000E(string \u001D)
	{
		Quaternion result = Quaternion.identity;
		GameObject gameObject = base.gameObject.FindInChildren(\u001D, 0);
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u000E(string)).MethodHandle;
			}
			result = gameObject.transform.rotation;
		}
		else
		{
			Log.Warning(string.Format("GetBoneRotation trying to find rotation of bone {0} on actor '{1}' (obj name '{2}'), but the bone cannot be found.", \u001D, this.DisplayName, base.gameObject.name), new object[0]);
			result = base.gameObject.transform.rotation;
		}
		return result;
	}

	public void OnDeselect()
	{
		if (this.\u000E() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.OnDeselect()).MethodHandle;
			}
			this.\u000E().ClearHighlights();
		}
		this.\u000E().UpdateCoverHighlights(null);
		this.RespawnPickedPositionSquare = this.RespawnPickedPositionSquare;
	}

	public void OnSelect()
	{
		this.m_callHandleOnSelectInUpdate = true;
	}

	private void HandleOnSelect()
	{
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.ProcessTeams();
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.MarkFramesForForceUpdate();
			HUD_UI.Get().m_mainScreenPanel.m_characterProfile.UpdateStatusDisplay(true);
		}
		this.m_actorTurnSM.OnSelect();
		this.\u000E().MarkForRecalculateVisibility();
		CameraManager.Get().OnActiveOwnedActorChange(this);
		if (this.\u000E() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.HandleOnSelect()).MethodHandle;
			}
			this.\u000E().UpdateSquaresCanMoveTo();
		}
	}

	public void OnMovementChanged(ActorData.MovementChangeType changeType, bool forceChased = false)
	{
	}

	public bool OutOfCombat
	{
		get
		{
			return this.m_outOfCombat;
		}
		private set
		{
			this.m_outOfCombat = value;
		}
	}

	public unsafe bool BeingTargetedByClientAbility(out bool inCover, out bool updatingInConfirm)
	{
		bool flag = false;
		inCover = false;
		updatingInConfirm = false;
		GameFlowData gameFlowData = GameFlowData.Get();
		if (gameFlowData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.BeingTargetedByClientAbility(bool*, bool*)).MethodHandle;
			}
			if (gameFlowData.gameState == GameState.BothTeams_Decision)
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
				if (gameFlowData.activeOwnedActorData != null)
				{
					ActorData activeOwnedActorData = gameFlowData.activeOwnedActorData;
					AbilityData abilityData = activeOwnedActorData.\u000E();
					ActorTurnSM actorTurnSM = activeOwnedActorData.\u000E();
					if (actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION)
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
						Ability selectedAbility = abilityData.GetSelectedAbility();
						if (selectedAbility != null)
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
							if (selectedAbility.Targeters != null)
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
								flag = selectedAbility.IsActorInTargetRange(this, out inCover);
								int num = actorTurnSM.GetAbilityTargets().Count;
								num = Mathf.Clamp(num, 0, selectedAbility.Targeters.Count - 1);
								this.UpdateNameplateForTargetingAbility(activeOwnedActorData, selectedAbility, flag, inCover, num, false);
							}
						}
					}
					else
					{
						if (actorTurnSM.CurrentState == TurnStateEnum.DECIDING && abilityData != null)
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
							Ability lastSelectedAbility = abilityData.GetLastSelectedAbility();
							if (this.ShouldUpdateForConfirmedTargeting(lastSelectedAbility, actorTurnSM.GetAbilityTargets().Count))
							{
								flag = lastSelectedAbility.IsActorInTargetRange(this, out inCover);
								int num2;
								if (lastSelectedAbility.IsSimpleAction())
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
									num2 = 0;
								}
								else
								{
									num2 = actorTurnSM.GetAbilityTargets().Count - 1;
								}
								int num3 = num2;
								if (num3 >= 0)
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
									if (lastSelectedAbility.Targeters != null)
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
										num3 = Mathf.Clamp(num3, 0, lastSelectedAbility.Targeters.Count - 1);
										this.UpdateNameplateForTargetingAbility(activeOwnedActorData, lastSelectedAbility, flag, inCover, num3, true);
										updatingInConfirm = true;
										if (HUD_UI.Get() != null)
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
											if (activeOwnedActorData.ForceDisplayTargetHighlight)
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
												HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.ShowTargetingNumberForConfirmedTargeting(this);
												this.m_showingTargetingNumAtFullAlpha = true;
											}
											else
											{
												if (this.m_wasUpdatingForConfirmedTargeting)
												{
													if (!this.m_showingTargetingNumAtFullAlpha)
													{
														goto IL_271;
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
												HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.StartTargetingNumberFadeout(this);
												this.m_showingTargetingNumAtFullAlpha = false;
											}
										}
									}
								}
								IL_271:;
							}
							else if (this.m_wasUpdatingForConfirmedTargeting)
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
								HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.StartTargetingNumberFadeout(this);
								this.m_showingTargetingNumAtFullAlpha = false;
							}
						}
						if (actorTurnSM.CurrentState == TurnStateEnum.DECIDING)
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
							if (!activeOwnedActorData.ForceDisplayTargetHighlight)
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
									if (HUD_UI.Get() != null)
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
										HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateNameplateUntargeted(this, !updatingInConfirm);
									}
								}
							}
						}
					}
				}
			}
		}
		this.m_wasUpdatingForConfirmedTargeting = updatingInConfirm;
		return flag;
	}

	public void AddForceShowOutlineChecker(IForceActorOutlineChecker checker)
	{
		if (checker != null && !this.m_forceShowOutlineCheckers.Contains(checker))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.AddForceShowOutlineChecker(IForceActorOutlineChecker)).MethodHandle;
			}
			this.m_forceShowOutlineCheckers.Add(checker);
		}
	}

	public void RemoveForceShowOutlineChecker(IForceActorOutlineChecker checker)
	{
		if (this.m_forceShowOutlineCheckers != null)
		{
			this.m_forceShowOutlineCheckers.Remove(checker);
		}
	}

	public bool ShouldForceTargetOutlineForActor(ActorData actor)
	{
		if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.ShouldForceTargetOutlineForActor(ActorData)).MethodHandle;
			}
			if (GameFlowData.Get().activeOwnedActorData != null)
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
				bool flag = false;
				int i = 0;
				while (i < this.m_forceShowOutlineCheckers.Count)
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
					if (flag)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							return flag;
						}
					}
					else
					{
						IForceActorOutlineChecker forceActorOutlineChecker = this.m_forceShowOutlineCheckers[i];
						if (forceActorOutlineChecker != null)
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
							flag = forceActorOutlineChecker.ShouldForceShowOutline(actor);
						}
						i++;
					}
				}
				return flag;
			}
		}
		return false;
	}

	[Client]
	private void UpdateNameplateForTargetingAbility(ActorData targetingActor, Ability selectedAbility, bool targeted, bool inCover, int currentTargeterIndex, bool inConfirm)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.UpdateNameplateForTargetingAbility(ActorData, Ability, bool, bool, int, bool)).MethodHandle;
			}
			UnityEngine.Debug.LogWarning("[Client] function 'System.Void ActorData::UpdateNameplateForTargetingAbility(ActorData,Ability,System.Boolean,System.Boolean,System.Int32,System.Boolean)' called on server");
			return;
		}
		if (HUD_UI.Get() != null)
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
			if (this == targetingActor)
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
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateSelfNameplate(this, selectedAbility, inCover, currentTargeterIndex, inConfirm);
			}
			else if (targeted)
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
			UnityEngine.Debug.LogWarning("[Client] function 'System.Boolean ActorData::ShouldUpdateForConfirmedTargeting(Ability,System.Int32)' called on server");
			return false;
		}
		if (lastSelectedAbility == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.ShouldUpdateForConfirmedTargeting(Ability, int)).MethodHandle;
			}
			return false;
		}
		bool result;
		if (!this.ForceDisplayTargetHighlight)
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
			if (lastSelectedAbility.Targeter != null)
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
				if (lastSelectedAbility.Targeter.GetConfirmedTargetingRemainingTime() > 0f)
				{
					if (!lastSelectedAbility.IsSimpleAction())
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
						result = (numAbilityTargets > 0);
					}
					else
					{
						result = true;
					}
					goto IL_8F;
				}
			}
			result = false;
			IL_8F:;
		}
		else
		{
			result = true;
		}
		return result;
	}

	public static bool WouldSquareBeChasedByClient(BoardSquare square, bool IgnoreChosenChaseTarget = false)
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (!activeOwnedActorData.\u0012(square))
		{
			return false;
		}
		if (!activeOwnedActorData.HasQueuedMovement())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.WouldSquareBeChasedByClient(BoardSquare, bool)).MethodHandle;
			}
			if (!activeOwnedActorData.HasQueuedChase())
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
				return true;
			}
		}
		if (activeOwnedActorData.HasQueuedChase())
		{
			return IgnoreChosenChaseTarget || !(square == activeOwnedActorData.\u0012().\u0012());
		}
		if (!(square == activeOwnedActorData.MoveFromBoardSquare))
		{
			if (activeOwnedActorData.CanMoveToBoardSquare(square))
			{
				return false;
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
		return true;
	}

	public bool \u0012(BoardSquare \u001D)
	{
		if (!(\u001D == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0012(BoardSquare)).MethodHandle;
			}
			if (!(\u001D.occupant == null))
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
				if (!(\u001D.occupant.GetComponent<ActorData>() == null))
				{
					if (!(GameFlowData.Get() == null))
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
						if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
						{
							ActorData component = \u001D.occupant.GetComponent<ActorData>();
							AbilityData component2 = this.GetComponent<AbilityData>();
							if (component.\u000E())
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
								return false;
							}
							if (component == this)
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
								return false;
							}
							if (!component.\u0012(this))
							{
								return false;
							}
							if (!component2.GetQueuedAbilitiesAllowMovement())
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
								return false;
							}
							if (component.IgnoreForAbilityHits)
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
								return false;
							}
							return true;
						}
					}
					return false;
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
		return false;
	}

	public void OnHitWhileInCover(Vector3 hitOrigin, ActorData caster)
	{
		if (!this.\u000E())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.OnHitWhileInCover(Vector3, ActorData)).MethodHandle;
			}
			if (this.m_actorVFX != null)
			{
				this.m_actorVFX.ShowHitWhileInCoverVfx(this.\u0016(), hitOrigin, caster);
				AudioManager.PostEvent("ablty/generic/feedback/behind_cover_hit", base.gameObject);
			}
		}
	}

	public void OnKnockbackWhileUnstoppable(Vector3 hitOrigin, ActorData caster)
	{
		if (!this.\u000E())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.OnKnockbackWhileUnstoppable(Vector3, ActorData)).MethodHandle;
			}
			if (this.m_actorVFX != null)
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
				this.m_actorVFX.ShowKnockbackWhileUnstoppableVfx(this.\u0016(), hitOrigin, caster);
				AudioManager.PostEvent("ablty/generic/feedback/unstoppable", base.gameObject);
			}
		}
	}

	public void PostAnimationAudioEvent(string eventAndTag)
	{
		int num = eventAndTag.IndexOf(':');
		string audioTag;
		string eventName;
		if (num == -1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.PostAnimationAudioEvent(string)).MethodHandle;
			}
			audioTag = "default";
			eventName = eventAndTag;
		}
		else
		{
			audioTag = eventAndTag.Substring(0, num);
			eventName = eventAndTag.Substring(num + 1);
		}
		CharacterResourceLink characterResourceLink = this.\u000E();
		if (characterResourceLink != null)
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
			if (!characterResourceLink.AllowAudioTag(audioTag, this.m_visualInfo))
			{
				return;
			}
		}
		this.PostAudioEvent(eventName, null, AudioManager.EventAction.PlaySound);
	}

	public void PostAudioEvent(string eventName, OnEventNotify notifyCallback = null, AudioManager.EventAction action = AudioManager.EventAction.PlaySound)
	{
		CharacterResourceLink characterResourceLink = this.\u000E();
		string text;
		if (characterResourceLink != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.PostAudioEvent(string, OnEventNotify, AudioManager.EventAction)).MethodHandle;
			}
			text = characterResourceLink.ReplaceAudioEvent(eventName, this.m_visualInfo);
		}
		else
		{
			text = eventName;
		}
		if (text != eventName)
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
		}
		if (notifyCallback != null)
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
			AudioManager.PostEventNotify(text, action, notifyCallback, null, base.gameObject);
		}
		else
		{
			AudioManager.PostEvent(text, action, null, base.gameObject);
		}
	}

	[Command]
	internal void CmdSetPausedForDebugging(bool pause)
	{
		if (!HydrogenConfig.Get().AllowDebugCommands)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.CmdSetPausedForDebugging(bool)).MethodHandle;
			}
			return;
		}
		GameFlowData.Get().SetPausedForDebugging(pause);
	}

	[Command]
	internal void CmdSetResolutionSingleStepping(bool singleStepping)
	{
		if (GameFlowData.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.CmdSetResolutionSingleStepping(bool)).MethodHandle;
			}
			GameFlowData.Get().SetResolutionSingleStepping(singleStepping);
		}
	}

	[Command]
	internal void CmdSetResolutionSingleSteppingAdvance()
	{
		if (GameFlowData.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.CmdSetResolutionSingleSteppingAdvance()).MethodHandle;
			}
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
		this.CallCmdDebugSetHealthOrEnergy(actorIndex, valueToSet, 0);
	}

	public void HandleDebugSetEnergy(int actorIndex, int valueToSet)
	{
		this.CallCmdDebugSetHealthOrEnergy(actorIndex, valueToSet, 1);
	}

	[ClientRpc]
	internal void RpcOnHitPointsResolved(int resolvedHitPoints)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.RpcOnHitPointsResolved(int)).MethodHandle;
			}
			bool flag = this.\u000E();
			this.UnresolvedDamage = 0;
			this.UnresolvedHealing = 0;
			this.ClientUnresolvedDamage = 0;
			this.ClientUnresolvedHealing = 0;
			this.ClientUnresolvedAbsorb = 0;
			this.SetHitPoints(resolvedHitPoints);
			if (!flag && this.\u000E())
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
				if (!this.\u0012())
				{
					UnityEngine.Debug.LogError("Actor " + this.\u0018() + " died on HP resolved; he should have already been ragdolled, but wasn't.");
					this.DoVisualDeath(new ActorModelData.ImpulseInfo(this.\u0015(), Vector3.up));
				}
			}
		}
	}

	[ClientRpc]
	internal void RpcCombatText(string combatText, string logText, CombatTextCategory category, BuffIconToDisplay icon)
	{
		this.AddCombatText(combatText, logText, category, icon);
	}

	internal void AddCombatText(string combatText, string logText, CombatTextCategory category, BuffIconToDisplay icon)
	{
		if (this.m_combatText == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.AddCombatText(string, string, CombatTextCategory, BuffIconToDisplay)).MethodHandle;
			}
			Log.Error(base.gameObject.name + " does not have a combat text component.", new object[0]);
			return;
		}
		this.m_combatText.Add(combatText, logText, category, icon);
	}

	[Client]
	internal void ShowDamage(string combatText)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.ShowDamage(string)).MethodHandle;
			}
			UnityEngine.Debug.LogWarning("[Client] function 'System.Void ActorData::ShowDamage(System.String)' called on server");
			return;
		}
	}

	[ClientRpc]
	internal void RpcApplyAbilityModById(int actionTypeInt, int abilityScopeId)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.RpcApplyAbilityModById(int, int)).MethodHandle;
			}
			if (NetworkClient.active && abilityScopeId >= 0)
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
				this.ApplyAbilityModById(actionTypeInt, abilityScopeId);
			}
		}
	}

	internal void ApplyAbilityModById(int actionTypeInt, int abilityScopeId)
	{
		bool flag;
		if (GameManager.Get().GameConfig.GameType != GameType.Tutorial)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.ApplyAbilityModById(int, int)).MethodHandle;
			}
			flag = AbilityModHelper.IsModAllowed(this.m_characterType, actionTypeInt, abilityScopeId);
		}
		else
		{
			flag = true;
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
			UnityEngine.Debug.LogWarning(string.Concat(new object[]
			{
				"Mod with ID ",
				abilityScopeId,
				" is not allowed on ability at index ",
				actionTypeInt,
				" for character ",
				this.m_characterType.ToString()
			}));
			return;
		}
		AbilityData component = base.GetComponent<AbilityData>();
		if (component != null)
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
			Ability abilityOfActionType = component.GetAbilityOfActionType((AbilityData.ActionType)actionTypeInt);
			AbilityMod abilityModForAbilityById = AbilityModManager.Get().GetAbilityModForAbilityById(abilityOfActionType, abilityScopeId);
			if (abilityModForAbilityById != null)
			{
				GameType gameType = GameManager.Get().GameConfig.GameType;
				GameSubType instanceSubType = GameManager.Get().GameConfig.InstanceSubType;
				if (abilityModForAbilityById.EquippableForGameType())
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
					this.ApplyAbilityModToAbility(abilityOfActionType, abilityModForAbilityById, false);
					if (NetworkServer.active)
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
						this.CallRpcApplyAbilityModById(actionTypeInt, abilityScopeId);
					}
				}
				else
				{
					Log.Warning(string.Concat(new object[]
					{
						"Mod with ID ",
						abilityModForAbilityById.m_abilityScopeId,
						" is not allowed in game type: ",
						gameType.ToString(),
						", subType: ",
						instanceSubType.LocalizedName
					}), new object[0]);
				}
			}
		}
	}

	internal void \u000E(int \u001D, int \u000E)
	{
	}

	private void ApplyAbilityModToAbility(Ability ability, AbilityMod abilityMod, bool log = false)
	{
		if (ability.GetType() == abilityMod.GetTargetAbilityType())
		{
			ability.ApplyAbilityMod(abilityMod, this);
			if (abilityMod.m_useChainAbilityOverrides)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.ApplyAbilityModToAbility(Ability, AbilityMod, bool)).MethodHandle;
				}
				ability.SanitizeChainAbilities();
				this.\u000E().ReInitializeChainAbilityList();
				Ability[] chainAbilities = ability.GetChainAbilities();
				if (chainAbilities != null)
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
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				UnityEngine.Debug.LogWarning("Applied " + abilityMod.GetDebugIdentifier("white") + " to ability " + ability.GetDebugIdentifier("orange"));
			}
		}
	}

	[ClientRpc]
	public void RpcMarkForRecalculateClientVisibility()
	{
		if (this.\u000E() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.RpcMarkForRecalculateClientVisibility()).MethodHandle;
			}
			this.\u000E().MarkForRecalculateVisibility();
		}
	}

	public void ShowRespawnFlare(BoardSquare flareSquare, bool respawningThisTurn)
	{
		bool flag = GameFlowData.Get().LocalPlayerData != null && GameFlowData.Get().LocalPlayerData.GetTeamViewing() == this.\u000E();
		bool flag2 = false;
		if (this.m_respawnPositionFlare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.ShowRespawnFlare(BoardSquare, bool)).MethodHandle;
			}
			flag2 = (this.m_respawnFlareVfxSquare == flareSquare && this.m_respawnFlareForSameTeam == flag);
			UnityEngine.Object.Destroy(this.m_respawnPositionFlare);
			this.m_respawnPositionFlare = null;
			UICharacterMovementPanel.Get().RemoveRespawnIndicator(this);
			this.m_respawnFlareVfxSquare = null;
			this.m_respawnFlareForSameTeam = false;
		}
		if (SpawnPointManager.Get() != null)
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
			if (!SpawnPointManager.Get().m_spawnInDuringMovement)
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
				return;
			}
		}
		if (flareSquare != null)
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
			GameObject original;
			if (flag)
			{
				if (respawningThisTurn)
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
					original = HighlightUtils.Get().m_respawnPositionFinalFriendlyVFXPrefab;
				}
				else
				{
					original = HighlightUtils.Get().m_respawnPositionFlareFriendlyVFXPrefab;
				}
			}
			else if (respawningThisTurn)
			{
				original = HighlightUtils.Get().m_respawnPositionFinalEnemyVFXPrefab;
			}
			else
			{
				original = HighlightUtils.Get().m_respawnPositionFlareEnemyVFXPrefab;
			}
			this.m_respawnPositionFlare = UnityEngine.Object.Instantiate<GameObject>(original);
			this.m_respawnFlareVfxSquare = flareSquare;
			this.m_respawnFlareForSameTeam = flag;
			if (!flag2 && this == GameFlowData.Get().activeOwnedActorData)
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
				UISounds.GetUISounds().Play("ui/ingame/v1/respawn_locator");
			}
			this.m_respawnPositionFlare.transform.position = flareSquare.ToVector3();
			UICharacterMovementPanel.Get().AddRespawnIndicator(flareSquare, this);
		}
	}

	[ClientRpc]
	public void RpcForceLeaveGame(GameResult gameResult)
	{
		if (GameFlowData.Get().activeOwnedActorData == this)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.RpcForceLeaveGame(GameResult)).MethodHandle;
			}
			if (!ClientGameManager.Get().IsFastForward)
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
				ClientGameManager.Get().LeaveGame(false, gameResult);
			}
		}
	}

	public void SendPingRequestToServer(int teamIndex, Vector3 worldPosition, ActorController.PingType pingType)
	{
		if (this.\u000E() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.SendPingRequestToServer(int, Vector3, ActorController.PingType)).MethodHandle;
			}
			this.\u000E().CallCmdSendMinimapPing(teamIndex, worldPosition, pingType);
		}
	}

	public void SendAbilityPingRequestToServer(int teamIndex, LocalizationArg_AbilityPing localizedPing)
	{
		if (this.\u000E() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.SendAbilityPingRequestToServer(int, LocalizationArg_AbilityPing)).MethodHandle;
			}
			this.\u000E().CallCmdSendAbilityPing(teamIndex, localizedPing);
		}
	}

	public override string ToString()
	{
		return string.Format("[ActorData: {0}, {1}, ActorIndex: {2}, {3}] {4}", new object[]
		{
			this.m_displayName,
			this.\u0012(),
			this.m_actorIndex,
			this.m_team,
			this.PlayerData
		});
	}

	public string \u0018()
	{
		return string.Concat(new object[]
		{
			"[",
			this.\u0012(),
			" (",
			this.DisplayName,
			"), ",
			this.ActorIndex,
			"]"
		});
	}

	public string \u0012(string \u001D)
	{
		return string.Concat(new string[]
		{
			"<color=",
			\u001D,
			">",
			this.\u0018(),
			"</color>"
		});
	}

	public string \u0009()
	{
		int num = this.ExpectedHoTTotal + this.ClientExpectedHoTTotalAdjust;
		int expectedHoTThisTurn = this.ExpectedHoTThisTurn;
		int clientAppliedHoTThisTurn = this.ClientAppliedHoTThisTurn;
		return string.Concat(new object[]
		{
			"Max HP: ",
			this.\u0012(),
			"\nHP to Display: ",
			this.\u0009(),
			"\n HP: ",
			this.HitPoints,
			"\n Damage: ",
			this.UnresolvedDamage,
			"\n Healing: ",
			this.UnresolvedHealing,
			"\n Absorb: ",
			this.AbsorbPoints,
			"\n CL Damage: ",
			this.ClientUnresolvedDamage,
			"\n CL Healing: ",
			this.ClientUnresolvedHealing,
			"\n CL Absorb: ",
			this.ClientUnresolvedAbsorb,
			"\n\n Energy to Display: ",
			this.\u0019(),
			"\n  Energy: ",
			this.TechPoints,
			"\n Reserved Energy: ",
			this.ReservedTechPoints,
			"\n EnergyGain: ",
			this.UnresolvedTechPointGain,
			"\n EnergyLoss: ",
			this.UnresolvedTechPointLoss,
			"\n CL Reserved Energy: ",
			this.ClientReservedTechPoints,
			"\n CL EnergyGain: ",
			this.ClientUnresolvedTechPointGain,
			"\n CL EnergyLoss: ",
			this.ClientUnresolvedTechPointLoss,
			"\n CL Total HoT: ",
			num,
			"\n CL HoT This Turn/Applied: ",
			expectedHoTThisTurn,
			" / ",
			clientAppliedHoTThisTurn
		});
	}

	public string \u0019()
	{
		string text = string.Empty;
		if (this.\u000E() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u0019()).MethodHandle;
			}
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"ActorTurnSM: CurrentState= ",
				this.\u000E().CurrentState,
				" | PrevState= ",
				this.\u000E().PreviousState,
				"\n"
			});
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
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.OnDrawGizmos()).MethodHandle;
			}
			return;
		}
		Gizmos.color = Color.green;
		if (this.\u0012() != null)
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
			Gizmos.DrawWireCube(this.\u0012().CameraBounds.center, this.\u0012().CameraBounds.size * 0.9f);
			Gizmos.DrawRay(this.\u0012().ToVector3(), base.transform.forward);
		}
	}

	public bool HasTag(string tag)
	{
		if (this.m_actorTags != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.HasTag(string)).MethodHandle;
			}
			return this.m_actorTags.HasTag(tag);
		}
		return false;
	}

	public void AddTag(string tag)
	{
		if (this.m_actorTags == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.AddTag(string)).MethodHandle;
			}
			this.m_actorTags = base.gameObject.AddComponent<ActorTag>();
		}
		this.m_actorTags.AddTag(tag);
	}

	public void RemoveTag(string tag)
	{
		if (this.m_actorTags != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.RemoveTag(string)).MethodHandle;
			}
			this.m_actorTags.RemoveTag(tag);
		}
	}

	public BoardSquare InitialSpawnSquare
	{
		get
		{
			return this.m_initialSpawnSquare;
		}
	}

	public CharacterResourceLink \u000E()
	{
		if (this.m_characterResourceLink == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.\u000E()).MethodHandle;
			}
			if (this.m_characterType != CharacterType.None)
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
				GameWideData gameWideData = GameWideData.Get();
				if (gameWideData)
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
					this.m_characterResourceLink = gameWideData.GetCharacterResourceLink(this.m_characterType);
				}
			}
		}
		return this.m_characterResourceLink;
	}

	public GameObject ReplaceSequence(GameObject originalSequencePrefab)
	{
		if (originalSequencePrefab == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.ReplaceSequence(GameObject)).MethodHandle;
			}
			return null;
		}
		CharacterResourceLink characterResourceLink = this.\u000E();
		if (characterResourceLink == null)
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
			return originalSequencePrefab;
		}
		return characterResourceLink.ReplaceSequence(originalSequencePrefab, this.m_visualInfo, this.m_abilityVfxSwapInfo);
	}

	public void OnAnimEvent(UnityEngine.Object eventObject, GameObject sourceObject)
	{
		if (this.OnAnimationEventDelegates != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.OnAnimEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			this.OnAnimationEventDelegates(eventObject, sourceObject);
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.GametimeScaleChange)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
			}
			Animator animator = this.\u000E();
			if (animator != null)
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
				animator.speed = GameTime.scale;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.InvokeCmdCmdSetPausedForDebugging(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			UnityEngine.Debug.LogError("Command CmdSetPausedForDebugging called on client.");
			return;
		}
		((ActorData)obj).CmdSetPausedForDebugging(reader.ReadBoolean());
	}

	protected static void InvokeCmdCmdSetResolutionSingleStepping(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.InvokeCmdCmdSetResolutionSingleStepping(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			UnityEngine.Debug.LogError("Command CmdSetResolutionSingleStepping called on client.");
			return;
		}
		((ActorData)obj).CmdSetResolutionSingleStepping(reader.ReadBoolean());
	}

	protected static void InvokeCmdCmdSetResolutionSingleSteppingAdvance(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.InvokeCmdCmdSetResolutionSingleSteppingAdvance(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			UnityEngine.Debug.LogError("Command CmdSetResolutionSingleSteppingAdvance called on client.");
			return;
		}
		((ActorData)obj).CmdSetResolutionSingleSteppingAdvance();
	}

	protected static void InvokeCmdCmdSetDebugToggleParam(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.InvokeCmdCmdSetDebugToggleParam(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			UnityEngine.Debug.LogError("Command CmdSetDebugToggleParam called on client.");
			return;
		}
		((ActorData)obj).CmdSetDebugToggleParam(reader.ReadString(), reader.ReadBoolean());
	}

	protected static void InvokeCmdCmdDebugReslotCards(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.InvokeCmdCmdDebugReslotCards(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			UnityEngine.Debug.LogError("Command CmdDebugReslotCards called on client.");
			return;
		}
		((ActorData)obj).CmdDebugReslotCards(reader.ReadBoolean(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdDebugSetAbilityMod(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogError("Command CmdDebugSetAbilityMod called on client.");
			return;
		}
		((ActorData)obj).CmdDebugSetAbilityMod((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeCmdCmdDebugReplaceWithBot(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.InvokeCmdCmdDebugReplaceWithBot(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			UnityEngine.Debug.LogError("Command CmdDebugReplaceWithBot called on client.");
			return;
		}
		((ActorData)obj).CmdDebugReplaceWithBot();
	}

	protected static void InvokeCmdCmdDebugSetHealthOrEnergy(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogError("Command CmdDebugSetHealthOrEnergy called on client.");
			return;
		}
		((ActorData)obj).CmdDebugSetHealthOrEnergy((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	public void CallCmdSetPausedForDebugging(bool pause)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("Command function CmdSetPausedForDebugging called on server.");
			return;
		}
		if (base.isServer)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.CallCmdSetPausedForDebugging(bool)).MethodHandle;
			}
			this.CmdSetPausedForDebugging(pause);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)ActorData.kCmdCmdSetPausedForDebugging);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(pause);
		base.SendCommandInternal(networkWriter, 0, "CmdSetPausedForDebugging");
	}

	public void CallCmdSetResolutionSingleStepping(bool singleStepping)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.CallCmdSetResolutionSingleStepping(bool)).MethodHandle;
			}
			UnityEngine.Debug.LogError("Command function CmdSetResolutionSingleStepping called on server.");
			return;
		}
		if (base.isServer)
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
			this.CmdSetResolutionSingleStepping(singleStepping);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)ActorData.kCmdCmdSetResolutionSingleStepping);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(singleStepping);
		base.SendCommandInternal(networkWriter, 0, "CmdSetResolutionSingleStepping");
	}

	public void CallCmdSetResolutionSingleSteppingAdvance()
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("Command function CmdSetResolutionSingleSteppingAdvance called on server.");
			return;
		}
		if (base.isServer)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.CallCmdSetResolutionSingleSteppingAdvance()).MethodHandle;
			}
			this.CmdSetResolutionSingleSteppingAdvance();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)ActorData.kCmdCmdSetResolutionSingleSteppingAdvance);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		base.SendCommandInternal(networkWriter, 0, "CmdSetResolutionSingleSteppingAdvance");
	}

	public void CallCmdSetDebugToggleParam(string name, bool value)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.CallCmdSetDebugToggleParam(string, bool)).MethodHandle;
			}
			UnityEngine.Debug.LogError("Command function CmdSetDebugToggleParam called on server.");
			return;
		}
		if (base.isServer)
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
			this.CmdSetDebugToggleParam(name, value);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)ActorData.kCmdCmdSetDebugToggleParam);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(name);
		networkWriter.Write(value);
		base.SendCommandInternal(networkWriter, 0, "CmdSetDebugToggleParam");
	}

	public void CallCmdDebugReslotCards(bool reslotAll, int cardTypeInt)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.CallCmdDebugReslotCards(bool, int)).MethodHandle;
			}
			UnityEngine.Debug.LogError("Command function CmdDebugReslotCards called on server.");
			return;
		}
		if (base.isServer)
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
			this.CmdDebugReslotCards(reslotAll, cardTypeInt);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)ActorData.kCmdCmdDebugReslotCards);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(reslotAll);
		networkWriter.WritePackedUInt32((uint)cardTypeInt);
		base.SendCommandInternal(networkWriter, 0, "CmdDebugReslotCards");
	}

	public void CallCmdDebugSetAbilityMod(int abilityIndex, int modId)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.CallCmdDebugSetAbilityMod(int, int)).MethodHandle;
			}
			UnityEngine.Debug.LogError("Command function CmdDebugSetAbilityMod called on server.");
			return;
		}
		if (base.isServer)
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
			this.CmdDebugSetAbilityMod(abilityIndex, modId);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)ActorData.kCmdCmdDebugSetAbilityMod);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)abilityIndex);
		networkWriter.WritePackedUInt32((uint)modId);
		base.SendCommandInternal(networkWriter, 0, "CmdDebugSetAbilityMod");
	}

	public void CallCmdDebugReplaceWithBot()
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("Command function CmdDebugReplaceWithBot called on server.");
			return;
		}
		if (base.isServer)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.CallCmdDebugReplaceWithBot()).MethodHandle;
			}
			this.CmdDebugReplaceWithBot();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)ActorData.kCmdCmdDebugReplaceWithBot);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		base.SendCommandInternal(networkWriter, 0, "CmdDebugReplaceWithBot");
	}

	public void CallCmdDebugSetHealthOrEnergy(int actorIndex, int valueToSet, int flag)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.CallCmdDebugSetHealthOrEnergy(int, int, int)).MethodHandle;
			}
			UnityEngine.Debug.LogError("Command function CmdDebugSetHealthOrEnergy called on server.");
			return;
		}
		if (base.isServer)
		{
			this.CmdDebugSetHealthOrEnergy(actorIndex, valueToSet, flag);
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)ActorData.kCmdCmdDebugSetHealthOrEnergy);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actorIndex);
		networkWriter.WritePackedUInt32((uint)valueToSet);
		networkWriter.WritePackedUInt32((uint)flag);
		base.SendCommandInternal(networkWriter, 0, "CmdDebugSetHealthOrEnergy");
	}

	protected static void InvokeRpcRpcOnHitPointsResolved(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("RPC RpcOnHitPointsResolved called on server.");
			return;
		}
		((ActorData)obj).RpcOnHitPointsResolved((int)reader.ReadPackedUInt32());
	}

	protected static void InvokeRpcRpcCombatText(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("RPC RpcCombatText called on server.");
			return;
		}
		((ActorData)obj).RpcCombatText(reader.ReadString(), reader.ReadString(), (CombatTextCategory)reader.ReadInt32(), (BuffIconToDisplay)reader.ReadInt32());
	}

	protected static void InvokeRpcRpcApplyAbilityModById(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.InvokeRpcRpcApplyAbilityModById(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			UnityEngine.Debug.LogError("RPC RpcApplyAbilityModById called on server.");
			return;
		}
		((ActorData)obj).RpcApplyAbilityModById((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	protected static void InvokeRpcRpcMarkForRecalculateClientVisibility(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("RPC RpcMarkForRecalculateClientVisibility called on server.");
			return;
		}
		((ActorData)obj).RpcMarkForRecalculateClientVisibility();
	}

	protected static void InvokeRpcRpcForceLeaveGame(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("RPC RpcForceLeaveGame called on server.");
			return;
		}
		((ActorData)obj).RpcForceLeaveGame((GameResult)reader.ReadInt32());
	}

	public void CallRpcOnHitPointsResolved(int resolvedHitPoints)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogError("RPC Function RpcOnHitPointsResolved called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)ActorData.kRpcRpcOnHitPointsResolved);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)resolvedHitPoints);
		this.SendRPCInternal(networkWriter, 0, "RpcOnHitPointsResolved");
	}

	public void CallRpcCombatText(string combatText, string logText, CombatTextCategory category, BuffIconToDisplay icon)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.CallRpcCombatText(string, string, CombatTextCategory, BuffIconToDisplay)).MethodHandle;
			}
			UnityEngine.Debug.LogError("RPC Function RpcCombatText called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)ActorData.kRpcRpcCombatText);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.Write(combatText);
		networkWriter.Write(logText);
		networkWriter.Write((int)category);
		networkWriter.Write((int)icon);
		this.SendRPCInternal(networkWriter, 0, "RpcCombatText");
	}

	public void CallRpcApplyAbilityModById(int actionTypeInt, int abilityScopeId)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogError("RPC Function RpcApplyAbilityModById called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)ActorData.kRpcRpcApplyAbilityModById);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actionTypeInt);
		networkWriter.WritePackedUInt32((uint)abilityScopeId);
		this.SendRPCInternal(networkWriter, 0, "RpcApplyAbilityModById");
	}

	public void CallRpcMarkForRecalculateClientVisibility()
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorData.CallRpcMarkForRecalculateClientVisibility()).MethodHandle;
			}
			UnityEngine.Debug.LogError("RPC Function RpcMarkForRecalculateClientVisibility called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)ActorData.kRpcRpcMarkForRecalculateClientVisibility);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		this.SendRPCInternal(networkWriter, 0, "RpcMarkForRecalculateClientVisibility");
	}

	public void CallRpcForceLeaveGame(GameResult gameResult)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogError("RPC Function RpcForceLeaveGame called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)ActorData.kRpcRpcForceLeaveGame);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.Write((int)gameResult);
		this.SendRPCInternal(networkWriter, 0, "RpcForceLeaveGame");
	}

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
		\u001D,
		TricksterAfterImage
	}

	public enum MovementChangeType
	{
		MoreMovement,
		LessMovement
	}
}
