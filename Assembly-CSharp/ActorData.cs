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
	private static Action<GameState> f__mg_cache0;

	[CompilerGenerated]
	private static Action<GameState> f__mg_cache1;

	[CompilerGenerated]
	private static Action<GameState> f__mg_cache2;

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
		this.OnTurnStartDelegatesHolder = delegate()
		{
		};
		this.OnAnimationEventDelegatesHolder = delegate(UnityEngine.Object A_0, GameObject A_1)
		{
		};
		
		this.OnSelectedAbilityChangedDelegatesHolder = delegate(Ability A_0)
			{
			};
		
		this.OnClientQueuedActionChangedDelegatesHolder = delegate()
			{
			};
		this.m_serializeHelper = new SerializeHelper();
		this.m_forceShowOutlineCheckers = new List<IForceActorOutlineChecker>();
		
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
				if (ActorDebugUtils.Get() != null)
				{
					if (ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.LastKnownPosition, false))
					{
						string[] array = new string[5];
						array[0] = this.GetDebugName();
						array[1] = "----Setting ClientLastKnownPosSquare from ";
						array[2] = ((!this.m_clientLastKnownPosSquare) ? "null" : this.m_clientLastKnownPosSquare.ToString());
						array[3] = " to ";
						int num = 4;
						string text;
						if (value)
						{
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
				if (ActorDebugUtils.Get() != null && ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.LastKnownPosition, false))
				{
					string[] array = new string[5];
					array[0] = this.GetDebugName();
					array[1] = "=====ServerLastKnownPosSquare from ";
					int num = 2;
					string text;
					if (this.m_serverLastKnownPosSquare)
					{
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

	public int GetLastVisibleTurnToClient()
	{
		return this.m_lastVisibleTurnToClient;
	}

	public Vector3 GetClientLastKnownPos()
	{
		if (this.ClientLastKnownPosSquare)
		{
			return this.ClientLastKnownPosSquare.transform.position;
		}
		return Vector3.zero;
	}

	public Vector3 GetServerLastKnownPos()
	{
		if (this.ServerLastKnownPosSquare)
		{
			return this.ServerLastKnownPosSquare.transform.position;
		}
		return Vector3.zero;
	}

	public void ActorData_OnActorMoved(BoardSquare movementSquare, bool visibleToEnemies, bool updateLastKnownPos)
	{
		if (NetworkClient.active)
		{
			if (updateLastKnownPos)
			{
				this.ClientLastKnownPosSquare = movementSquare;
				this.m_lastVisibleTurnToClient = GameFlowData.Get().CurrentTurn;
			}
			this.m_shouldUpdateLastVisibleToClientThisFrame = false;
		}
	}

	public ActorBehavior GetActorBehavior()
	{
		return this.m_actorBehavior;
	}

	public ActorModelData GetActorModelData()
	{
		return this.m_actorModelData;
	}

	internal ActorModelData GetFaceActorModelData()
	{
		return this.m_faceActorModelData;
	}

	public Renderer GetActorModelDataRenderer()
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

	internal ItemData GetItemData()
	{
		return this.m_itemData;
	}

	internal AbilityData GetAbilityData()
	{
		return this.m_abilityData;
	}

	internal ActorMovement GetActorMovement()
	{
		return this.m_actorMovement;
	}

	internal ActorStats GetActorStats()
	{
		return this.m_actorStats;
	}

	internal ActorStatus GetActorStatus()
	{
		return this.m_actorStatus;
	}

	internal ActorController GetActorController()
	{
		return base.GetComponent<ActorController>();
	}

	internal ActorTargeting GetActorTargeting()
	{
		return this.m_actorTargeting;
	}

	internal FreelancerStats GetFreelancerStats()
	{
		return this.m_freelancerStats;
	}

	internal NPCBrain GetEnabledNPCBrain()
	{
		ActorController actorController = this.GetActorController();
		if (actorController != null)
		{
			NPCBrain[] components = base.GetComponents<NPCBrain>();
			foreach (NPCBrain npcbrain in components)
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
		return this.m_actorTurnSM;
	}

	internal ActorCover GetActorCover()
	{
		return this.m_actorCover;
	}

	internal ActorVFX GetActorVFX()
	{
		return this.m_actorVFX;
	}

	internal TimeBank GetTimeBank()
	{
		return this.m_timeBank;
	}

	internal FogOfWar GetFogOfWar()
	{
		return this.PlayerData.GetFogOfWar();
	}

	internal ActorAdditionalVisionProviders GetActorAdditionalVisionProviders()
	{
		return this.m_additionalVisionProvider;
	}

	internal PassiveData GetPassiveData()
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

	public string GetFancyDisplayName()
	{
		if (this.HasBotController)
		{
			if (this.GetAccountIdWithSomeConditionB_zq() == 0L)
			{
				if (this.m_characterType != CharacterType.None)
				{
					if (!this.GetPlayerDetails().m_botsMasqueradeAsHumans)
					{
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
								return component.GetFancyDisplayName();
							}
						}
					}
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

	public Sprite GetAliveHUDIcon()
	{
		return (Sprite)Resources.Load(this.m_aliveHUDIconResourceString, typeof(Sprite));
	}

	public Sprite GetDeadHUDIcon()
	{
		return (Sprite)Resources.Load(this.m_deadHUDIconResourceString, typeof(Sprite));
	}

	public Sprite GetScreenIndicatorIcon()
	{
		return (Sprite)Resources.Load(this.m_screenIndicatorIconResourceString, typeof(Sprite));
	}

	public Sprite GetScreenIndicatorBWIcon()
	{
		return (Sprite)Resources.Load(this.m_screenIndicatorBWIconResourceString, typeof(Sprite));
	}

	public string GetName()
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

	public float GetPostAbilityHorizontalMovementChange()
	{
		return this.m_maxHorizontalMovement - this.m_postAbilityHorizontalMovement;
	}

	public int GetMaxHitPoints()
	{
		int result = 1;
		ActorStats actorStats = this.m_actorStats;
		if (actorStats != null)
		{
			result = actorStats.GetModifiedStatInt(StatType.MaxHitPoints);
		}
		return result;
	}

	public void OnMaxHitPointsChanged(int previousMax)
	{
		if (this.IsDead())
		{
			return;
		}
		float num = (float)this.HitPoints / (float)previousMax;
		int maxHitPoints = this.GetMaxHitPoints();
		this.HitPoints = Mathf.RoundToInt((float)maxHitPoints * num);
	}

	public float GetHitPointShareOfMax()
	{
		int maxHitPoints = this.GetMaxHitPoints();
		int hitPoints = this.HitPoints;
		return (float)hitPoints / (float)maxHitPoints;
	}

	public int GetActualPassiveHpRegen()
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
			num = Mathf.RoundToInt((float)num * GameplayMutators.GetPassiveHpRegenMultiplier());
		}
		return num;
	}

	public int GetActualMaxTechPoints()
	{
		int result = 1;
		ActorStats actorStats = this.m_actorStats;
		if (actorStats != null)
		{
			result = actorStats.GetModifiedStatInt(StatType.MaxTechPoints);
		}
		return result;
	}

	public void OnMaxTechPointsChanged(int previousMax)
	{
		if (this.IsDead())
		{
			return;
		}
		int actualMaxTechPoints = this.GetActualMaxTechPoints();
		if (actualMaxTechPoints > previousMax)
		{
			int num = actualMaxTechPoints - previousMax;
			this.TechPoints += num;
		}
		else if (previousMax > actualMaxTechPoints)
		{
			int techPoints = this.TechPoints;
			this.TechPoints = Mathf.Min(this.TechPoints, actualMaxTechPoints);
			if (techPoints - this.TechPoints != 0)
			{
			}
		}
	}

	public void TriggerVisibilityForHit(bool movementHit, bool updateClientLastKnownPos = true)
	{
		if (movementHit)
		{
			this.m_endVisibilityForHitTime = Time.time + ActorData.s_visibleTimeAfterMovementHit;
		}
		else
		{
			this.m_endVisibilityForHitTime = Time.time + ActorData.s_visibleTimeAfterHit;
		}
		this.ForceUpdateIsVisibleToClientCache();
		if (updateClientLastKnownPos)
		{
			if (this.GetCurrentBoardSquare() != null)
			{
				this.ClientLastKnownPosSquare = this.GetTravelBoardSquare();
				this.m_lastVisibleTurnToClient = GameFlowData.Get().CurrentTurn;
			}
		}
	}

	private void UpdateClientLastKnownPosSquare()
	{
		if (this.m_shouldUpdateLastVisibleToClientThisFrame)
		{
			if (this.ClientLastKnownPosSquare != this.GetCurrentBoardSquare())
			{
				Team team;
				if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData != null)
				{
					team = GameFlowData.Get().activeOwnedActorData.GetTeam();
				}
				else
				{
					team = Team.Invalid;
				}
				bool flag;
				if (GameFlowData.Get() != null)
				{
					flag = GameFlowData.Get().IsInResolveState();
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				bool flag3 = this.GetTravelBoardSquare() == this.GetCurrentBoardSquare() && !this.GetActorMovement().AmMoving() && !this.GetActorMovement().IsYetToCompleteGameplayPath();
				bool flag4 = this.GetTeam() != team;
				if (flag2)
				{
					if (flag3)
					{
						if (flag4 && this.IsVisibleToClient())
						{
							this.ForceUpdateIsVisibleToClientCache();
							if (this.IsVisibleToClient())
							{
								this.ClientLastKnownPosSquare = this.GetCurrentBoardSquare();
								this.m_lastVisibleTurnToClient = GameFlowData.Get().CurrentTurn;
							}
						}
					}
				}
			}
		}
		this.m_shouldUpdateLastVisibleToClientThisFrame = true;
	}

	public Player GetPlayer()
	{
		return this.PlayerData.GetPlayer();
	}

	public PlayerDetails GetPlayerDetails()
	{
		if (this.PlayerData != null)
		{
			if (GameFlow.Get() != null)
			{
				if (GameFlow.Get().playerDetails != null && GameFlow.Get().playerDetails.ContainsKey(this.PlayerData.GetPlayer()))
				{
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
				if (actorData.GetAbilityData() != null)
				{
					for (int i = 0; i <= 4; i++)
					{
						actorData.ApplyAbilityModById(i, actorData.m_selectedMods.GetModForAbility(i));
					}
					ActorTargeting component = actorData.GetComponent<ActorTargeting>();
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
		if (this.ShouldPickRespawn_zq())
		{
			if (GameFlowData.Get() != null)
			{
				if (ServerClientUtils.GetCurrentActionPhase() >= ActionBufferPhase.Movement)
				{
					if (ServerClientUtils.GetCurrentActionPhase() <= ActionBufferPhase.MovementWait)
					{
						return;
					}
				}
				ActorModelData actorModelData = this.GetActorModelData();
				if (actorModelData != null)
				{
					actorModelData.DisableAndHideRenderers();
				}
				if (HighlightUtils.Get().m_recentlySpawnedShader != null)
				{
					TricksterAfterImageNetworkBehaviour.InitializeAfterImageMaterial(actorModelData, GameFlowData.Get().LocalPlayerData.GetTeamViewing() == this.GetTeam(), 0.5f, HighlightUtils.Get().m_recentlySpawnedShader, false);
				}
				if (this.RespawnPickedPositionSquare != null)
				{
					this.ShowRespawnFlare(this.RespawnPickedPositionSquare, true);
				}
			}
		}
	}

	public void SetupAbilityMods(CharacterModInfo characterMods)
	{
		this.m_selectedMods = characterMods;
		AbilityData abilityData = this.GetAbilityData();
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
					AbilityMod defaultModForAbility = AbilityModManager.Get().GetDefaultModForAbility(ability);
					int num2;
					if (defaultModForAbility != null)
					{
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
					if (num3 > 0)
					{
						this.ApplyAbilityModById((int)actionTypeOfAbility, num3);
					}
				}
				num++;
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
			if (this.ClientLastKnownPosSquare != this.ServerLastKnownPosSquare)
			{
				this.ClientLastKnownPosSquare = this.ServerLastKnownPosSquare;
				if (GameFlowData.Get().activeOwnedActorData != null)
				{
					if (this.GetTeam() != GameFlowData.Get().activeOwnedActorData.GetTeam())
					{
					}
				}
			}
		}
	}

	public int GetActualPassiveEnergyRegen()
	{
		int num = 0;
		ActorStats actorStats = this.m_actorStats;
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

	public float GetActualSightRange()
	{
		float result = 1f;
		if (this.m_actorStats != null)
		{
			result = this.m_actorStats.GetModifiedStatFloat(StatType.SightRange);
		}
		if (this.m_actorStatus != null)
		{
			if (this.m_actorStatus.HasStatus(StatusType.Blind, true))
			{
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
				this.m_hitPoints = Mathf.Clamp(value, 0, this.GetMaxHitPoints());
			}
			else
			{
				this.m_hitPoints = value;
			}
			int num = 0;
			if (GameFlowData.Get() != null)
			{
				num = GameFlowData.Get().CurrentTurn;
			}
			if (flag)
			{
				if (this.m_hitPoints == 0)
				{
					if (GameFlowData.Get() != null)
					{
						this.LastDeathTurn = GameFlowData.Get().CurrentTurn;
					}
					this.LastDeathPosition = base.gameObject.transform.position;
					this.NextRespawnTurn = -1;
					FogOfWar.CalculateFogOfWarForTeam(this.GetTeam());
					if (this.GetCurrentBoardSquare() != null)
					{
						this.SetMostRecentDeathSquare(this.GetCurrentBoardSquare());
					}
					base.gameObject.SendMessage("OnDeath");
					if (GameFlowData.Get() != null)
					{
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
				if (this.m_hitPoints > 0 && this.LastDeathTurn > 0)
				{
					base.gameObject.SendMessage("OnRespawn");
					this.m_lastVisibleTurnToClient = 0;
					if (NetworkServer.active)
					{
						if (this.m_teamSensitiveData_friendly != null)
						{
							this.m_teamSensitiveData_friendly.MarkAsRespawning();
						}
						if (this.m_teamSensitiveData_hostile != null)
						{
							this.m_teamSensitiveData_hostile.MarkAsRespawning();
						}
						if (num > 0)
						{
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
				if (DebugParameters.Get().GetParameterAsBool("InfiniteTP"))
				{
					return this.GetActualMaxTechPoints();
				}
			}
			return this.m_techPoints;
		}
		private set
		{
			if (NetworkServer.active)
			{
				this.m_techPoints = Mathf.Clamp(value, 0, this.GetActualMaxTechPoints());
			}
			else
			{
				this.m_techPoints = value;
			}
		}
	}

	internal void SetTechPoints(int value, bool combatText = false, ActorData caster = null, string sourceName = null)
	{
		int actualMaxTechPoints = this.GetActualMaxTechPoints();
		value = Mathf.Clamp(value, 0, actualMaxTechPoints);
		int num = value - this.TechPoints;
		this.TechPoints = value;
		if (combatText)
		{
			if (sourceName != null)
			{
				if (num != 0)
				{
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

	internal bool IsDead()
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
				if (NetworkServer.active)
				{
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
				return this.m_teamSensitiveData_friendly;
			}
			return this.m_teamSensitiveData_hostile;
		}
	}

	public ActorTeamSensitiveData GetFriendlyOrHostileTeamSensitiveData()
	{
		if (this.m_teamSensitiveData_friendly != null)
		{
			return this.m_teamSensitiveData_friendly;
		}
		if (this.m_teamSensitiveData_hostile != null)
		{
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
				return this.m_trueMoveFromBoardSquare;
			}
			if (this.m_teamSensitiveData_friendly != null)
			{
				return this.m_teamSensitiveData_friendly.MoveFromBoardSquare;
			}
			return this.CurrentBoardSquare;
		}
		set
		{
			if (NetworkServer.active && this.m_trueMoveFromBoardSquare != value)
			{
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
				return this.m_serverInitialMoveStartSquare;
			}
			if (this.m_teamSensitiveData_friendly != null)
			{
				return this.m_teamSensitiveData_friendly.InitialMoveStartSquare;
			}
			return this.CurrentBoardSquare;
		}
		set
		{
			if (NetworkServer.active)
			{
				if (this.m_serverInitialMoveStartSquare != value)
				{
					this.m_serverInitialMoveStartSquare = value;
					if (this.GetActorMovement() != null)
					{
						this.GetActorMovement().UpdateSquaresCanMoveTo();
					}
					if (this.m_teamSensitiveData_friendly != null)
					{
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
				return this.m_trueRespawnSquares;
			}
			if (this.m_teamSensitiveData_friendly != null)
			{
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
			this.m_teamSensitiveData_friendly.RespawnAvailableSquares = new List<BoardSquare>();
		}
	}

	public bool no_op_return_false(BoardSquare square)
	{
		return false;
	}

	public BoardSquare RespawnPickedPositionSquare
	{
		get
		{
			if (NetworkServer.active)
			{
				return this.m_trueRespawnPositionSquare;
			}
			if (this.m_teamSensitiveData_friendly != null)
			{
				return this.m_teamSensitiveData_friendly.RespawnPickedSquare;
			}
			if (this.m_teamSensitiveData_hostile != null)
			{
				return this.m_teamSensitiveData_hostile.RespawnPickedSquare;
			}
			return null;
		}
		set
		{
			if (NetworkServer.active)
			{
				this.m_trueRespawnPositionSquare = value;
				if (this.m_teamSensitiveData_friendly != null)
				{
					if (!GameFlowData.Get().IsInDecisionState())
					{
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
						if (this.no_op_return_false(value))
						{
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
					if (ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.LastKnownPosition, false))
					{
						UnityEngine.Debug.LogWarning(this.GetDebugName() + "Setting visible for ability cast to " + value);
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
				this.m_serverSuppressInvisibility = value;
			}
		}
	}

	internal void AddLineOfSightVisibleException(ActorData visibleActor)
	{
		this.m_lineOfSightVisibleExceptions.Add(visibleActor);
		this.GetFogOfWar().MarkForRecalculateVisibility();
	}

	internal void RemoveLineOfSightVisibleException(ActorData visibleActor)
	{
		this.m_lineOfSightVisibleExceptions.Remove(visibleActor);
		this.GetFogOfWar().MarkForRecalculateVisibility();
	}

	internal bool IsLineOfSightVisibleException(ActorData actor)
	{
		return this.m_lineOfSightVisibleExceptions.Contains(actor);
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
				list.Add(actorData.GetCurrentBoardSquare());
			}
			return list.AsReadOnly();
		}
	}

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
				action = Interlocked.CompareExchange<Action>(ref this.OnTurnStartDelegatesHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.OnTurnStartDelegatesHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnTurnStartDelegatesHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
				action = Interlocked.CompareExchange<Action<Ability>>(ref this.OnSelectedAbilityChangedDelegatesHolder, (Action<Ability>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<Ability> action = this.OnSelectedAbilityChangedDelegatesHolder;
			Action<Ability> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<Ability>>(ref this.OnSelectedAbilityChangedDelegatesHolder, (Action<Ability>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				action = Interlocked.CompareExchange<Action>(ref this.OnClientQueuedActionChangedDelegatesHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.OnClientQueuedActionChangedDelegatesHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnClientQueuedActionChangedDelegatesHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public void OnClientQueuedActionChanged()
	{
		if (this.OnClientQueuedActionChangedDelegatesHolder != null)
		{
			this.OnClientQueuedActionChangedDelegatesHolder();
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
		
		GameFlowData.s_onGameStateChanged -= new Action<GameState>(ActorData.OnGameStateChanged);
		
		GameFlowData.s_onGameStateChanged += new Action<GameState>(ActorData.OnGameStateChanged);
		this.PlayerData = base.GetComponent<PlayerData>();
		if (this.PlayerData == null)
		{
			throw new Exception(string.Format("Character {0} needs a PlayerData component", base.gameObject.name));
		}
		this.m_actorMovement = base.gameObject.GetComponent<ActorMovement>();
		if (this.m_actorMovement == null)
		{
			this.m_actorMovement = base.gameObject.AddComponent<ActorMovement>();
		}
		this.m_actorTurnSM = base.gameObject.GetComponent<ActorTurnSM>();
		if (this.m_actorTurnSM == null)
		{
			this.m_actorTurnSM = base.gameObject.AddComponent<ActorTurnSM>();
		}
		this.m_actorCover = base.gameObject.GetComponent<ActorCover>();
		if (this.m_actorCover == null)
		{
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
			this.m_timeBank = base.gameObject.AddComponent<TimeBank>();
		}
		this.m_additionalVisionProvider = base.gameObject.GetComponent<ActorAdditionalVisionProviders>();
		if (this.m_additionalVisionProvider == null)
		{
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
			this.ActorIndex = (int)(checked(ActorData.s_nextActorIndex += 1));
		}
		ActorData.Layer = LayerMask.NameToLayer("Actor");
		ActorData.Layer_Mask = 1 << ActorData.Layer;
		if (GameFlowData.Get())
		{
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
			return;
		}
		if (this.m_actorSkinPrefabLink != null && !this.m_actorSkinPrefabLink.IsEmpty)
		{
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
		}
		GameObject gameObject = heroPrefabLink.InstantiatePrefab(false);
		if (gameObject)
		{
			this.m_actorModelData = gameObject.GetComponent<ActorModelData>();
			if (this.m_actorModelData)
			{
				int layer = LayerMask.NameToLayer("Actor");
				foreach (Transform transform in this.m_actorModelData.gameObject.GetComponentsInChildren<Transform>(true))
				{
					transform.gameObject.layer = layer;
				}
			}
		}
		IL_155:
		if (this.m_actorModelData != null)
		{
			this.m_actorModelData.Setup(this);
			if (addMasterSkinVfx)
			{
				if (NetworkClient.active && MasterSkinVfxData.Get() != null)
				{
					GameObject masterSkinVfxInst = MasterSkinVfxData.Get().AddMasterSkinVfxOnCharacterObject(this.m_actorModelData.gameObject, this.m_characterType, 1f);
					this.m_actorModelData.SetMasterSkinVfxInst(masterSkinVfxInst);
				}
			}
		}
		if (this.m_faceActorModelData != null)
		{
			this.m_faceActorModelData.Setup(this);
		}
		if (NPCCoordinator.IsSpawnedNPC(this))
		{
			if (!this.m_addedToUI)
			{
				if (HUD_UI.Get() != null && HUD_UI.Get().m_mainScreenPanel != null)
				{
					this.m_addedToUI = true;
					HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.AddActor(this);
					HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddActor(this);
				}
			}
			NPCCoordinator.Get().OnActorSpawn(this);
			if (this.GetActorModelData() != null)
			{
				this.GetActorModelData().ForceUpdateVisibility();
			}
		}
	}

	private void Start()
	{
		if (NetworkClient.active)
		{
			this.m_nameplateJoint = new JointPopupProperty();
			this.m_nameplateJoint.m_joint = "VFX_name";
			this.m_nameplateJoint.Initialize(base.gameObject);
		}
		if (NetworkServer.active)
		{
			this.HitPoints = this.GetMaxHitPoints();
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
			if (HUD_UI.Get().m_mainScreenPanel != null)
			{
				this.m_addedToUI = true;
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.AddActor(this);
				HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddActor(this);
			}
		}
		PlayerDetails playerDetails = null;
		if (GameFlow.Get().playerDetails.TryGetValue(this.PlayerData.GetPlayer(), out playerDetails) && playerDetails.IsLocal())
		{
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
			this.CallCmdDebugReplaceWithBot();
		}
	}

	private static void OnGameStateChanged(GameState newState)
	{
		if (newState != GameState.BothTeams_Decision)
		{
			if (newState != GameState.BothTeams_Resolve)
			{
				if (newState != GameState.EndingGame)
				{
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
							if (actorData.GetActorModelData() != null)
							{
								Animator modelAnimator = actorData.GetModelAnimator();
								if (modelAnimator != null)
								{
									if (actorData.GetActorModelData().HasTurnStartParameter())
									{
										modelAnimator.SetBool("TurnStart", false);
									}
								}
							}
							if (actorData.GetComponent<LineData>() != null)
							{
								actorData.GetComponent<LineData>().OnResolveStart();
							}
							if (HUD_UI.Get() != null)
							{
								HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateNameplateUntargeted(actorData, false);
							}
						}
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
						if (!actorData2.IsDead())
						{
							if (actorData2.GetActorModelData() != null)
							{
								if (actorData2.IsModelAnimatorDisabled())
								{
									UnityEngine.Debug.LogError(string.Concat(new object[]
									{
										"Unragdolling undead actor on Turn Tick (",
										currentTurn,
										"): ",
										actorData2.GetDebugName()
									}));
									actorData2.EnableRagdoll(false, null, false);
									flag = true;
								}
							}
						}
					}
					if (actorData2 != null && !actorData2.IsDead())
					{
						if (actorData2.GetCurrentBoardSquare() == null)
						{
							if (actorData2.PlayerIndex != PlayerData.s_invalidPlayerIndex)
							{
								if (NetworkClient.active)
								{
									if (!NetworkServer.active)
									{
										if (GameFlowData.Get().LocalPlayerData.IsViewingTeam(actorData2.GetTeam()))
										{
											UnityEngine.Debug.LogError("On client, living friendly-to-client actor " + actorData2.GetDebugName() + " has null square on Turn Tick");
											flag = true;
										}
									}
								}
							}
						}
					}
				}
			}
			if (NetworkServer.active)
			{
				if (flag)
				{
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
				if (actorData.IsDead())
				{
					if (actorData.LastDeathTurn != GameFlowData.Get().CurrentTurn && !actorData.IsModelAnimatorDisabled())
					{
						if (actorData.NextRespawnTurn != GameFlowData.Get().CurrentTurn)
						{
							actorData.DoVisualDeath(Sequence.CreateImpulseInfoWithActorForward(actorData));
						}
					}
				}
			}
		}
	}

	public Animator GetModelAnimator()
	{
		if (this.m_actorModelData != null)
		{
			return this.m_actorModelData.GetModelAnimator();
		}
		return null;
	}

	public void PlayDamageReactionAnim(string customDamageReactTriggerName)
	{
		Animator modelAnimator = this.GetModelAnimator();
		if (modelAnimator == null)
		{
			return;
		}
		if (this.m_actorMovement.GetAestheticPath() == null)
		{
			if (!this.m_actorMovement.AmMoving())
			{
				bool flag;
				if (ServerClientUtils.GetCurrentAbilityPhase() == AbilityPriority.Combat_Knockback)
				{
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
					Animator animator = modelAnimator;
					string trigger;
					if (string.IsNullOrEmpty(customDamageReactTriggerName))
					{
						trigger = "StartDamageReaction";
					}
					else
					{
						trigger = customDamageReactTriggerName;
					}
					animator.SetTrigger(trigger);
				}
			}
		}
	}

	internal bool IsModelAnimatorDisabled()
	{
		Animator modelAnimator = this.GetModelAnimator();
		bool result;
		if (modelAnimator == null)
		{
			result = true;
		}
		else
		{
			result = !modelAnimator.enabled;
		}
		return result;
	}

	internal void DoVisualDeath(ActorModelData.ImpulseInfo impulseInfo)
	{
		if (this.IsModelAnimatorDisabled())
		{
			UnityEngine.Debug.LogWarning("Already in ragdoll");
		}
		if (this.m_actorVFX != null)
		{
			this.m_actorVFX.ShowOnDeathVfx(this.GetActorMovement().transform.position);
		}
		this.EnableRagdoll(true, impulseInfo, false);
		GameEventManager.Get().FireEvent(GameEventManager.EventType.CharacterVisualDeath, new GameEventManager.CharacterDeathEventArgs
		{
			deadCharacter = this
		});
		if (AudioManager.s_deathAudio)
		{
			AudioManager.PostEvent("ui/ingame/death", base.gameObject);
			if (!string.IsNullOrEmpty(this.m_onDeathAudioEvent))
			{
				AudioManager.PostEvent(this.m_onDeathAudioEvent, base.gameObject);
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
			if (this.GetTeam() == team)
			{
				clientFog.MarkForRecalculateVisibility();
			}
		}
		if (NetworkClient.active)
		{
			if (ObjectivePoints.Get() != null)
			{
				ObjectivePoints.Get().Client_OnActorDeath(this);
				if (GameplayUtils.IsPlayerControlled(this) && GameFlowData.Get().LocalPlayerData != null)
				{
					int num = ObjectivePoints.Get().Client_GetNumDeathOnTeamForCurrentTurn(this.GetTeam());
					if (num > 0)
					{
						if (UIDeathNotifications.Get() != null)
						{
							UIDeathNotifications.Get().NotifyDeathOccurred(this, this.GetTeam() == team);
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
			if (GameWideData.Get().FreeAutomaticOverconOnDeath_OverconID != -1)
			{
				if (UIOverconData.Get() != null)
				{
					UIOverconData.Get().UseOvercon(GameWideData.Get().FreeAutomaticOverconOnDeath_OverconID, this.ActorIndex, true);
				}
			}
		}
	}

	private void EnableRagdoll(bool ragDollOn, ActorModelData.ImpulseInfo impulseInfo = null, bool isDebugRagdoll = false)
	{
		if (ragDollOn && this.GetHitPointsAfterResolution() > 0)
		{
			if (!isDebugRagdoll)
			{
				Log.Error(string.Concat(new object[]
				{
					"early_ragdoll: enabling ragdoll on ",
					this.GetDebugName(),
					" with ",
					this.HitPoints,
					" HP,  (HP for display ",
					this.GetHitPointsAfterResolution(),
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
			if (SpawnPointManager.Get() != null)
			{
				if (SpawnPointManager.Get().m_spawnInDuringMovement)
				{
					InterfaceManager.Get().DisplayAlert(StringUtil.TR("PostRespawnMovement", "Global"), BoardSquare.s_respawnOptionHighlightColor, 60f, true, 0);
				}
			}
		}
	}

	private void OnRespawn()
	{
		this.EnableRagdoll(false, null, false);
		ActorModelData actorModelData = this.GetActorModelData();
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
		this.m_lastSpawnTurn = GameFlowData.Get().CurrentTurn;
	}

	public bool ShouldPickRespawn_zq()
	{
		if (!this.IsDead())
		{
			if (this.NextRespawnTurn > 0)
			{
				if (this.NextRespawnTurn == GameFlowData.Get().CurrentTurn && SpawnPointManager.Get() != null)
				{
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
			GameFlowData.Get().RemoveReferencesToDestroyedActor(this);
		}
		
		GameFlowData.s_onGameStateChanged -= new Action<GameState>(ActorData.OnGameStateChanged);
		this.m_actorModelData = null;
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GametimeScaleChange);
	}

	private void Update()
	{
		if (this.m_needAddToTeam)
		{
			if (GameFlowData.Get() != null)
			{
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
			base.transform.localRotation = this.m_targetRotation;
		}
		if (this.m_callHandleOnSelectInUpdate)
		{
			this.HandleOnSelect();
			this.m_callHandleOnSelectInUpdate = false;
		}
		if (NetworkServer.active)
		{
			base.SetDirtyBit(1U);
		}
		if (!this.m_addedToUI)
		{
			if (HUD_UI.Get() != null)
			{
				this.m_addedToUI = true;
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.AddActor(this);
				HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddActor(this);
			}
		}
	}

	public bool IsHiddenInBrush()
	{
		int travelBoardSquareBrushRegion = this.GetTravelBoardSquareBrushRegion();
		bool result;
		if (travelBoardSquareBrushRegion == -1)
		{
			result = false;
		}
		else if (!BrushCoordinator.Get().IsRegionFunctioning(travelBoardSquareBrushRegion))
		{
			result = false;
		}
		else
		{
			result = true;
		}
		return result;
	}

	public int GetTravelBoardSquareBrushRegion()
	{
		int result = -1;
		BoardSquare travelBoardSquare = this.GetTravelBoardSquare();
		if (travelBoardSquare != null)
		{
			if (travelBoardSquare.IsInBrushRegion())
			{
				result = travelBoardSquare.BrushRegion;
			}
		}
		return result;
	}

	public bool ShouldShowNameplate()
	{
		int result;
		if (!this.m_hideNameplate && !this.m_alwaysHideNameplate && this.ShowInGameGUI)
		{
			if (this.IsVisibleToClient())
			{
				if (!this.IsModelAnimatorDisabled())
				{
					if (!(this.GetActorModelData() == null))
					{
						result = (this.GetActorModelData().IsVisibleToClient() ? 1 : 0);
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
			if (GameFlowData.Get().LocalPlayerData != null)
			{
				PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (DebugParameters.Get() != null)
				{
					if (DebugParameters.Get().GetParameterAsBool("AllCharactersVisible"))
					{
						return true;
					}
				}
				if (GameFlowData.Get().gameState == GameState.Deployment)
				{
					result = true;
				}
				else
				{
					if (activeOwnedActorData != null)
					{
						if (activeOwnedActorData.GetTeam() == this.GetTeam())
						{
							return true;
						}
					}
					if (activeOwnedActorData == null)
					{
						if (localPlayerData.IsViewingTeam(this.GetTeam()))
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
							if (this.m_actorModelData.IsInCinematicCam())
							{
								return true;
							}
						}
						if (this.CurrentlyVisibleForAbilityCast)
						{
							result = true;
						}
						else if (this.m_disappearingAfterCurrentMovement && this.CurrentBoardSquare == null && !this.GetActorMovement().AmMoving())
						{
							result = false;
						}
						else
						{
							bool flag = this.SomeVisibilityCheckA_zq(localPlayerData, false);
							bool flag2 = this.SomeVisibilityCheckB_zq(localPlayerData, false, false);
							if (flag)
							{
								result = true;
							}
							else if (flag2)
							{
								result = false;
							}
							else if (FogOfWar.GetClientFog() == null)
							{
								result = false;
							}
							else if (FogOfWar.GetClientFog().IsVisible(this.GetTravelBoardSquare()))
							{
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

	public bool IsVisibleToClient()
	{
		this.UpdateIsVisibleToClientCache();
		return this.m_isVisibleToClientCache;
	}

	public bool symbol_0012(ActorData symbol_001D)
	{
		bool result = false;
		if (GameFlowData.Get().IsActorDataOwned(this))
		{
			if (this.GetTeam() == symbol_001D.GetTeam())
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
			bool flag = this.SomeVisibilityCheckA_zq(symbol_001D.PlayerData, true);
			bool flag2 = this.SomeVisibilityCheckB_zq(symbol_001D.PlayerData, true, false);
			if (flag)
			{
				result = true;
			}
			else if (flag2)
			{
				result = false;
			}
			else if (symbol_001D.GetFogOfWar())
			{
				result = symbol_001D.GetFogOfWar().IsVisible(this.GetTravelBoardSquare());
			}
		}
		return result;
	}

	public bool SomeVisibilityCheckA_zq(PlayerData symbol_001D, bool symbol_000E = true)
	{
		bool flag;
		if (symbol_001D == null)
		{
			flag = false;
		}
		else if (this.GetActorStatus().HasStatus(StatusType.Revealed, symbol_000E) && this.GetTeam() != symbol_001D.GetTeamViewing())
		{
			flag = true;
		}
		else
		{
			if (!NetworkServer.active)
			{
				if (CaptureTheFlag.IsActorRevealedByFlag_Client(this))
				{
					return true;
				}
			}
			if (this.VisibleTillEndOfPhase)
			{
				if (!this.MovedForEvade)
				{
					return true;
				}
			}
			if (this.CurrentlyVisibleForAbilityCast)
			{
				flag = true;
			}
			else
			{
				flag = this.IsDead();
				flag = (flag && this.IsModelAnimatorDisabled());
			}
		}
		return flag;
	}

	public bool SomeVisibilityCheckB_zq(PlayerData player, bool flag = true, bool flag2 = false)
	{
		Team team = Team.TeamA;
		if (player != null)
		{
			if (flag2)
			{
				if (player.ActorData != null)
				{
					team = player.ActorData.GetTeam();
					goto IL_5E;
				}
			}
			team = player.GetTeamViewing();
		}
		IL_5E:
		bool result;
		if (player == null)
		{
			result = false;
		}
		else
		{
			if (this.GetActorStatus().IsInvisibleToEnemies(flag))
			{
				if (this.GetTeam() != team)
				{
					if (player.ActorData)
					{
						if (player.ActorData.GetActorStatus().HasStatus(StatusType.SeeInvisible, flag))
						{
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

	public bool IsActorVisibleToActor(ActorData symbol_001D, bool symbol_000E = false)
	{
		if (this == symbol_001D)
		{
			return true;
		}
		if (!NetworkServer.active && symbol_001D == GameFlowData.Get().activeOwnedActorData)
		{
			return this.IsVisibleToClient();
		}
		if (!NetworkServer.active)
		{
			Log.Warning("Calling the server-only method ActorData::IsActorVisibleToActor on a client. Clients can only depend on ActorData::IsActorVisibleToClient.", new object[0]);
		}
		bool flag = this.SomeVisibilityCheckA_zq(symbol_001D.PlayerData, true);
		bool flag2 = this.SomeVisibilityCheckB_zq(symbol_001D.PlayerData, true, symbol_000E);
		bool result;
		if (flag)
		{
			result = true;
		}
		else if (flag2)
		{
			result = false;
		}
		else
		{
			FogOfWar fogOfWar = symbol_001D.GetFogOfWar();
			result = fogOfWar.IsVisible(this.GetTravelBoardSquare());
		}
		return result;
	}

	public bool IsVisibleToOpposingTeam()
	{
		bool result = false;
		List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(this.GetOpposingTeam());
		using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (!actorData.IsDead())
				{
					if (this.IsActorVisibleToActor(actorData, true))
					{
						return true;
					}
				}
			}
		}
		return result;
	}

	public bool symbol_0015(ActorData symbol_001D)
	{
		bool flag = this.SomeVisibilityCheckA_zq(symbol_001D.PlayerData, true);
		bool flag2 = this.SomeVisibilityCheckB_zq(symbol_001D.PlayerData, true, true);
		bool result;
		if (flag)
		{
			result = true;
		}
		else if (flag2)
		{
			result = false;
		}
		else
		{
			bool flag3 = this.GetTravelBoardSquareBrushRegion() < 0 || BrushRegion.HasTeamMemberInRegion(this.GetOpposingTeam(), this.GetTravelBoardSquareBrushRegion());
			bool flag4;
			if (this.IsHiddenInBrush())
			{
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
		Vector3 vector = this.GetBonePosition("hip_JNT") - pos;
		if (vector.magnitude < 1.5f)
		{
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
		Rigidbody hipJointRigidBody = this.GetHipJointRigidBody();
		if (hipJointRigidBody)
		{
			hipJointRigidBody.AddForce(dir * amount, ForceMode.Impulse);
		}
	}

	public Vector3 GetNameplatePosition(float offsetInPixels)
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
		float d = offsetInPixels / vector.magnitude;
		Vector3 b2 = Camera.main.transform.up * d;
		return position + b2;
	}

	public Vector3 GetTravelBoardSquareWorldPositionForLos()
	{
		return this.GetSquareWorldPositionForLoS(this.GetTravelBoardSquare());
	}

	public Vector3 GetTravelBoardSquareWorldPosition()
	{
		return this.GetSquareWorldPosition(this.GetTravelBoardSquare());
	}

	public Vector3 GetSquareWorldPositionForLoS(BoardSquare square)
	{
		if (square != null)
		{
			Vector3 squareWorldPosition = this.GetSquareWorldPosition(square);
			squareWorldPosition.y += BoardSquare.s_LoSHeightOffset;
			return squareWorldPosition;
		}
		Log.Warning(string.Concat(new object[]
		{
			"Trying to get LoS check pos wrt a null square (IsDead: ",
			this.IsDead(),
			")  for ",
			this.DisplayName,
			" Code issue-- actor probably instantiated but not on Board yet."
		}), new object[0]);
		return this.m_actorMovement.transform.position;
	}

	public Vector3 GetSquareWorldPosition(BoardSquare square)
	{
		if (square != null)
		{
			return square.GetWorldPosition();
		}
		Log.Warning(string.Concat(new object[]
		{
			"Trying to get free pos wrt a null square (IsDead: ",
			this.IsDead(),
			").  for ",
			this.DisplayName,
			" Code issue-- actor probably instantiated but not on Board yet."
		}), new object[0]);
		return this.m_actorMovement.transform.position;
	}

	public int GetHitPointsAfterResolution()
	{
		int num = this.UnresolvedDamage + this.ClientUnresolvedDamage;
		int num2 = this.UnresolvedHealing + this.ClientUnresolvedHealing;
		int num3 = this.AbsorbPoints + this.ClientUnresolvedAbsorb;
		int num4 = Mathf.Max(0, num - num3);
		return Mathf.Clamp(this.HitPoints - num4 + num2, 0, this.GetMaxHitPoints());
	}

	public int GetHitPointsAfterResolutionWithDelta(int delta)
	{
		int num = this.UnresolvedDamage + this.ClientUnresolvedDamage;
		int num2 = this.UnresolvedHealing + this.ClientUnresolvedHealing;
		int num3 = this.AbsorbPoints + this.ClientUnresolvedAbsorb;
		if (delta > 0)
		{
			num2 += delta;
		}
		else
		{
			num -= delta;
		}
		int num4 = Mathf.Max(0, num - num3);
		return Mathf.Clamp(this.HitPoints - num4 + num2, 0, this.GetMaxHitPoints());
	}

	public int GetEnergyToDisplay()
	{
		int num = this.UnresolvedTechPointGain + this.ClientUnresolvedTechPointGain;
		int num2 = this.UnresolvedTechPointLoss + this.ClientUnresolvedTechPointLoss;
		return Mathf.Clamp(this.TechPoints + this.ReservedTechPoints + this.ClientReservedTechPoints + num - num2, 0, this.GetActualMaxTechPoints());
	}

	public int GetClientUnappliedHoTTotal_ToDisplay_zq()
	{
		return Mathf.Max(0, this.ExpectedHoTTotal + this.ClientExpectedHoTTotalAdjust - this.ClientAppliedHoTThisTurn);
	}

	public int GetClientUnappliedHoTThisTurn_ToDisplay_zq()
	{
		return Mathf.Max(0, this.ExpectedHoTThisTurn - this.ClientAppliedHoTThisTurn);
	}

	public string GetHitPointsAfterResolutionDebugString()
	{
		int hitPointsAfterResolution = this.GetHitPointsAfterResolution();
		string text = string.Format("{0}", hitPointsAfterResolution);
		if (this.AbsorbPoints > 0)
		{
			int num = this.UnresolvedDamage + this.ClientUnresolvedDamage;
			int num2 = this.AbsorbPoints + this.ClientUnresolvedAbsorb;
			int num3 = Mathf.Max(0, num2 - num);
			text += string.Format(" +{0} shield", num3);
		}
		return text;
	}

	public int symbol_0004()
	{
		int num = this.UnresolvedDamage + this.ClientUnresolvedDamage;
		int num2 = this.AbsorbPoints + this.ClientUnresolvedAbsorb;
		return Mathf.Max(0, num2 - num);
	}

	public bool GetIsHumanControlled()
	{
		if (!(GameFlow.Get() == null))
		{
			if (GameFlow.Get().playerDetails != null)
			{
				PlayerDetails playerDetails;
				return GameFlow.Get().playerDetails.TryGetValue(this.PlayerData.GetPlayer(), out playerDetails) && playerDetails.IsHumanControlled;
			}
		}
		Log.Error("Method called too early, results may be incorrect", new object[0]);
		return false;
	}

	public bool symbol_0011()
	{
		if (!GameplayUtils.IsPlayerControlled(this))
		{
			return false;
		}
		if (!GameplayUtils.IsBot(this))
		{
			return false;
		}
		PlayerDetails playerDetails;
		if (!GameFlow.Get().playerDetails.TryGetValue(this.PlayerData.GetPlayer(), out playerDetails))
		{
			return false;
		}
		if (playerDetails == null)
		{
			return false;
		}
		if (!playerDetails.m_botsMasqueradeAsHumans)
		{
			return false;
		}
		return true;
	}

	public long GetAccountIdWithSomeConditionA_zq()
	{
		PlayerDetails playerDetails;
		if (!GameFlow.Get().playerDetails.TryGetValue(this.PlayerData.GetPlayer(), out playerDetails))
		{
			return -1L;
		}
		if (!this.symbol_0011() && !playerDetails.IsLoadTestBot && !this.GetIsHumanControlled())
		{
			return -1L;
		}
		return playerDetails.m_accountId;
	}

	public long GetAccountIdWithSomeConditionB_zq()
	{
		long result = -1L;
		if (this.PlayerData != null)
		{
			if (GameFlow.Get().playerDetails.ContainsKey(this.PlayerData.GetPlayer()))
			{
				result = GameFlow.Get().playerDetails[this.PlayerData.GetPlayer()].m_accountId;
			}
		}
		return result;
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

	public static bool no_op_return_false_unused
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
			num = reader.ReadPackedUInt32();
		}
		if (num != 0U)
		{
			this.OnSerializeHelper(new NetworkReaderAdapter(reader), initialState);
		}
		if (initialState)
		{
			if (AsyncPump.Current != null)
			{
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
			b = (sbyte)this.PlayerIndex;
			b2 = (sbyte)this.ActorIndex;
			b4 = (sbyte)this.m_team;
			num12 = this.m_lastVisibleTurnToClient;
			if (this.ServerLastKnownPosSquare != null)
			{
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
			b3 = (sbyte)GameplayUtils.GetActorIndexOfActor(this.GetQueuedChaseTarget());
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
				stream.Serialize(ref num4);
			}
			if (num6 > 0)
			{
				stream.Serialize(ref num6);
			}
			if (num7 > 0)
			{
				stream.Serialize(ref num7);
			}
			if (num5 != 0)
			{
				stream.Serialize(ref num5);
			}
			if (num8 > 0)
			{
				stream.Serialize(ref num8);
			}
			if (num9 > 0)
			{
				stream.Serialize(ref num9);
			}
			if (flag9)
			{
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
			}
			stream.Serialize(ref num12);
			stream.Serialize(ref num13);
			stream.Serialize(ref num14);
		}
		if (stream.isReading)
		{
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
				stream.Serialize(ref num5);
			}
			if (flag15)
			{
				stream.Serialize(ref num8);
			}
			if (flag16)
			{
				stream.Serialize(ref num9);
			}
			if (flag17)
			{
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
			}
			PrefabResourceLink prefabResourceLink = null;
			CharacterResourceLink characterResourceLink = null;
			if (this.m_characterType > CharacterType.None)
			{
				characterResourceLink = GameWideData.Get().GetCharacterResourceLink(this.m_characterType);
			}
			if (characterResourceLink == null)
			{
				if (NPCCoordinator.Get() != null)
				{
					characterResourceLink = NPCCoordinator.Get().GetNpcCharacterResourceLinkBySpawnerId((int)b5);
				}
			}
			if (characterResourceLink != null)
			{
				CharacterSkin characterSkin;
				prefabResourceLink = characterResourceLink.GetHeroPrefabLinkFromSelection(this.m_visualInfo, out characterSkin);
			}
			if (prefabResourceLink != null)
			{
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
						if (MasterSkinVfxData.Get().m_addMasterSkinVfx)
						{
							if (characterResourceLink.IsVisualInfoSelectionValid(this.m_visualInfo))
							{
								CharacterColor characterColor = characterResourceLink.GetCharacterColor(this.m_visualInfo);
								addMasterSkinVfx = (characterColor.m_styleLevel == StyleLevelType.Mastery);
							}
						}
					}
					this.InitializeModel(prefabResourceLink, addMasterSkinVfx);
					goto IL_8B4;
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
					this.m_lineOfSightVisibleExceptions.Add(actorData2);
				}
			}
			stream.Serialize(ref num12);
			stream.Serialize(ref num13);
			stream.Serialize(ref num14);
			if (num12 > this.m_lastVisibleTurnToClient)
			{
				this.m_lastVisibleTurnToClient = num12;
			}
			if (num13 == -1)
			{
				if (num14 == -1)
				{
					this.ServerLastKnownPosSquare = null;
					goto IL_9B6;
				}
			}
			this.ServerLastKnownPosSquare = Board.Get().GetBoardSquare((int)num13, (int)num14);
			IL_9B6:
			this.m_ignoreFromAbilityHits = flag7;
			this.m_alwaysHideNameplate = flag8;
			this.GetFogOfWar().MarkForRecalculateVisibility();
			this.m_showInGameHud = flag5;
			this.VisibleTillEndOfPhase = flag6;
			if (this.m_setTeam)
			{
				if (team == this.m_team)
				{
					goto IL_ACC;
				}
			}
			if (GameFlowData.Get() != null)
			{
				GameFlowData.Get().AddToTeam(this);
			}
			else
			{
				this.m_needAddToTeam = true;
			}
			if (TeamStatusDisplay.GetTeamStatusDisplay() != null)
			{
				TeamStatusDisplay.GetTeamStatusDisplay().RebuildTeamDisplay();
			}
			if (GameplayUtils.IsMinion(base.gameObject))
			{
				if (MinionManager.Get() != null)
				{
					if (this.m_setTeam)
					{
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
				this.RemainingHorizontalMovement = num;
				flag18 = true;
			}
			if (num2 != this.RemainingMovementWithQueuedAbility)
			{
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
				this.m_queuedChaseTarget = actorOfActorIndex;
			}
			if (flag18)
			{
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

	public GridPos GetGridPosWithIncrementedHeight()
	{
		GridPos result = default(GridPos);
		if (this.GetCurrentBoardSquare())
		{
			result = this.GetCurrentBoardSquare().GetGridPos();
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
			this.m_facingDirAfterMovement = facingDirAfterMovement;
			if (NetworkServer.active)
			{
				if (this.m_teamSensitiveData_friendly != null)
				{
					this.m_teamSensitiveData_friendly.FacingDirAfterMovement = this.m_facingDirAfterMovement;
				}
				if (this.m_teamSensitiveData_hostile != null)
				{
					this.m_teamSensitiveData_hostile.FacingDirAfterMovement = this.m_facingDirAfterMovement;
				}
			}
		}
	}

	public Vector3 GetFacingDirAfterMovement()
	{
		return this.m_facingDirAfterMovement;
	}

	public void OnMovementWhileDisappeared(ActorData.MovementType movementType)
	{
		UnityEngine.Debug.Log(this.GetDebugName() + ": calling OnMovementWhileDisappeared.");
		if (ClientMovementManager.Get() != null)
		{
			ClientMovementManager.Get().OnActorMoveStart_Disappeared(this, movementType);
		}
		if (this.GetCurrentBoardSquare() != null)
		{
			if (this.GetCurrentBoardSquare().occupant == base.gameObject)
			{
				this.UnoccupyCurrentBoardSquare();
			}
		}
		this.m_actorMovement.ClearPath();
		this.SetCurrentBoardSquare(null);
		this.GetFogOfWar().MarkForRecalculateVisibility();
	}

	public void MoveToBoardSquareLocal(BoardSquare dest, ActorData.MovementType movementType, BoardSquarePathInfo path, bool moverWillDisappear)
	{
		this.m_disappearingAfterCurrentMovement = moverWillDisappear;
		if (dest == null)
		{
			if (moverWillDisappear)
			{
				if (path == null)
				{
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
				this.MovedForEvade = true;
			}
			bool flag;
			if (path != null)
			{
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
				ClientMovementManager.Get().OnActorMoveStart_ClientMovementManager(this, dest2, movementType, path);
				ClientResolutionManager.Get().OnActorMoveStart_ClientResolutionManager(this, path);
			}
			if (this.GetCurrentBoardSquare() != null)
			{
				if (this.GetCurrentBoardSquare().occupant == base.gameObject)
				{
					this.UnoccupyCurrentBoardSquare();
				}
			}
			BoardSquare currentBoardSquare = this.CurrentBoardSquare;
			if (movementType == ActorData.MovementType.Teleport)
			{
				this.m_actorMovement.ClearPath();
			}
			if (!flag2)
			{
				if (!moverWillDisappear)
				{
					this.SetCurrentBoardSquare(dest);
					if (this.GetCurrentBoardSquare() != null)
					{
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
				this.ForceUpdateIsVisibleToClientCache();
				this.ForceUpdateActorModelVisibility();
				this.SetTransformPositionToSquare(dest);
				this.m_actorMovement.ClearPath();
				if (this.m_actorCover)
				{
					this.m_actorCover.RecalculateCover();
				}
				this.UpdateFacingAfterMovement();
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
			else
			{
				if (movementType != ActorData.MovementType.Normal)
				{
					if (movementType != ActorData.MovementType.Flight)
					{
						if (movementType == ActorData.MovementType.WaypointFlight)
						{
						}
						else
						{
							if (movementType != ActorData.MovementType.Knockback)
							{
								if (movementType != ActorData.MovementType.Charge)
								{
									goto IL_458;
								}
							}
							if (this.m_actorCover)
							{
								this.m_actorCover.DisableCover();
							}
							this.m_actorMovement.BeginChargeOrKnockback(currentBoardSquare, dest, path, movementType);
							this.m_actorMovement.UpdatePosition();
							if (!flag2)
							{
								if (!moverWillDisappear)
								{
									if (path.square == dest)
									{
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
					this.m_actorCover.DisableCover();
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
				this.m_actorMovement.BeginTravellingAlongPath(path, movementType);
				this.m_actorMovement.UpdatePosition();
			}
			IL_458:
			this.m_actorMovement.UpdateSquaresCanMoveTo();
			this.GetFogOfWar().MarkForRecalculateVisibility();
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
			if (this.GetCurrentBoardSquare() != null)
			{
				if (this.GetCurrentBoardSquare().occupant == base.gameObject)
				{
					this.UnoccupyCurrentBoardSquare();
				}
			}
			this.SetCurrentBoardSquare(dest);
			this.SetTransformPositionToSquare(dest);
			if (this.GetCurrentBoardSquare() != null)
			{
				this.OccupyCurrentBoardSquare();
			}
		}
	}

	public void SetTransformPositionToSquare(BoardSquare refSquare)
	{
		if (refSquare != null)
		{
			Vector3 worldPosition = refSquare.GetWorldPosition();
			this.SetTransformPositionToVector(worldPosition);
		}
	}

	public void SetTransformPositionToVector(Vector3 newPos)
	{
		if (base.transform.position != newPos)
		{
			BoardSquare boardSquare = Board.Get().GetBoardSquare(base.transform.position);
			BoardSquare boardSquare2 = Board.Get().GetBoardSquare(newPos);
			if (boardSquare != boardSquare2 && boardSquare != null)
			{
				this.PreviousBoardSquarePosition = boardSquare.ToVector3();
			}
			base.transform.position = newPos;
		}
	}

	public void UnoccupyCurrentBoardSquare()
	{
		if (this.GetCurrentBoardSquare() != null)
		{
			if (this.GetCurrentBoardSquare().occupant == base.gameObject)
			{
				this.GetCurrentBoardSquare().occupant = null;
			}
		}
	}

	public BoardSquare GetTravelBoardSquare()
	{
		return this.m_actorMovement.GetTravelBoardSquare();
	}

	public BoardSquare GetCurrentBoardSquare()
	{
		return this.CurrentBoardSquare;
	}

	public BoardSquare GetMostResetDeathSquare()
	{
		return this.m_mostRecentDeathSquare;
	}

	public void SetMostRecentDeathSquare(BoardSquare square)
	{
		this.m_mostRecentDeathSquare = square;
	}

	public void OccupyCurrentBoardSquare()
	{
		if (this.GetCurrentBoardSquare() != null)
		{
			this.GetCurrentBoardSquare().occupant = base.gameObject;
		}
	}

	private void SetCurrentBoardSquare(BoardSquare square)
	{
		if (square == this.CurrentBoardSquare)
		{
			return;
		}
		this.m_clientCurrentBoardSquare = square;
		Animator modelAnimator = this.GetModelAnimator();
		if (modelAnimator != null)
		{
			bool[] array;
			modelAnimator.SetBool("Cover", ActorCover.CalcCoverLevelGeoOnly(out array, this.CurrentBoardSquare));
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
			this.UnoccupyCurrentBoardSquare();
		}
		this.m_clientCurrentBoardSquare = null;
		this.MoveFromBoardSquare = null;
	}

	public void ClearPreviousMovementInfo()
	{
		if (NetworkServer.active)
		{
			if (this.m_teamSensitiveData_friendly != null)
			{
				this.m_teamSensitiveData_friendly.ClearPreviousMovementInfo();
			}
			if (this.m_teamSensitiveData_hostile != null)
			{
				this.m_teamSensitiveData_hostile.ClearPreviousMovementInfo();
			}
		}
	}

	public void SetClientFriendlyTeamSensitiveData(ActorTeamSensitiveData friendlyTSD)
	{
		if (this.m_teamSensitiveData_friendly != friendlyTSD)
		{
			Log.Info("Setting Friendly TeamSensitiveData for " + this.GetDebugName(), new object[0]);
			this.m_teamSensitiveData_friendly = friendlyTSD;
			this.m_teamSensitiveData_friendly.OnClientAssociatedWithActor(this);
		}
	}

	public void SetClientHostileTeamSensitiveData(ActorTeamSensitiveData hostileTSD)
	{
		if (this.m_teamSensitiveData_hostile != hostileTSD)
		{
			Log.Info("Setting Hostile TeamSensitiveData for " + this.GetDebugName(), new object[0]);
			this.m_teamSensitiveData_hostile = hostileTSD;
			this.m_teamSensitiveData_hostile.OnClientAssociatedWithActor(this);
		}
	}

	public void UpdateFacingAfterMovement()
	{
		if (this.m_facingDirAfterMovement != Vector3.zero)
		{
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
		}
	}

	public Team GetTeam()
	{
		return this.m_team;
	}

	public Team GetOpposingTeam()
	{
		if (this.m_team == Team.TeamA)
		{
			return Team.TeamB;
		}
		if (this.m_team == Team.TeamB)
		{
			return Team.TeamA;
		}
		return Team.Objects;
	}

	public List<Team> GetOtherTeams()
	{
		return GameplayUtils.GetOtherTeamsThan(this.m_team);
	}

	public List<Team> GetTeams()
	{
		return new List<Team>
		{
			this.GetTeam()
		};
	}

	public List<Team> GetOpposingTeams()
	{
		return new List<Team>
		{
			this.GetOpposingTeam()
		};
	}

	public string GetTeamColorName()
	{
		string result;
		if (this.m_team == Team.TeamA)
		{
			result = "Blue";
		}
		else
		{
			result = "Orange";
		}
		return result;
	}

	public string GetOpposingTeamColorName()
	{
		return (this.m_team != Team.TeamA) ? "Blue" : "Orange";
	}

	public Color GetTeamColor()
	{
		Color result;
		if (this.m_team == Team.TeamA)
		{
			result = ActorData.s_teamAColor;
		}
		else
		{
			result = ActorData.s_teamBColor;
		}
		return result;
	}

	public Color GetOpposingTeamColor()
	{
		Color result;
		if (this.m_team == Team.TeamA)
		{
			result = ActorData.s_teamBColor;
		}
		else
		{
			result = ActorData.s_teamAColor;
		}
		return result;
	}

	public Color GetColorForTeam(Team observingTeam)
	{
		Color result;
		if (observingTeam == this.GetTeam())
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
		this.GetFogOfWar().MarkForRecalculateVisibility();
		if (!NetworkServer.active)
		{
			if (this.m_serverMovementWaitForEvent != GameEventManager.EventType.Invalid)
			{
				if (this.m_serverMovementDestination != this.GetCurrentBoardSquare() && !this.IsDead())
				{
					this.MoveToBoardSquareLocal(this.m_serverMovementDestination, ActorData.MovementType.Teleport, this.m_serverMovementPath, false);
				}
			}
		}
		if (this.GetActorModelData() != null)
		{
			Animator modelAnimator = this.GetModelAnimator();
			if (modelAnimator != null)
			{
				if (this.GetActorModelData().HasTurnStartParameter())
				{
					modelAnimator.SetBool("TurnStart", true);
				}
				modelAnimator.SetInteger("Attack", 0);
				modelAnimator.SetBool("CinematicCam", false);
			}
		}
		if (NetworkClient.active)
		{
			if (this.ClientUnresolvedDamage != 0)
			{
				Log.Error("ClientUnresolvedDamage not cleared on TurnTick for " + this.GetDebugName(), new object[0]);
				this.ClientUnresolvedDamage = 0;
			}
			if (this.ClientUnresolvedHealing != 0)
			{
				Log.Error("ClientUnresolvedHealing not cleared on TurnTick for " + this.GetDebugName(), new object[0]);
				this.ClientUnresolvedHealing = 0;
			}
			if (this.ClientUnresolvedTechPointGain != 0)
			{
				this.ClientUnresolvedTechPointGain = 0;
			}
			if (this.ClientUnresolvedTechPointLoss != 0)
			{
				this.ClientUnresolvedTechPointLoss = 0;
			}
			if (this.ClientReservedTechPoints != 0)
			{
				this.ClientReservedTechPoints = 0;
			}
			if (this.ClientUnresolvedAbsorb != 0)
			{
				Log.Error("ClientUnresolvedAbsorb not cleared on TurnTick for " + this.GetDebugName(), new object[0]);
				this.ClientUnresolvedAbsorb = 0;
			}
			this.ClientExpectedHoTTotalAdjust = 0;
			this.ClientAppliedHoTThisTurn = 0;
			this.SynchClientLastKnownPosToServerLastKnownPos();
			if (this.GetFriendlyOrHostileTeamSensitiveData() != null)
			{
				this.GetFriendlyOrHostileTeamSensitiveData().OnTurnTick();
			}
			PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
			if (localPlayerData != null && HighlightUtils.Get().m_recentlySpawnedShader != null)
			{
				int currentTurn = GameFlowData.Get().CurrentTurn;
				if (currentTurn == 1)
				{
					TricksterAfterImageNetworkBehaviour.InitializeAfterImageMaterial(this.GetActorModelData(), GameFlowData.Get().LocalPlayerData.GetTeamViewing() == this.GetTeam(), 0.5f, HighlightUtils.Get().m_recentlySpawnedShader, false);
				}
				else
				{
					if (currentTurn != 2)
					{
						if (currentTurn <= 2)
						{
							goto IL_2ED;
						}
						if (currentTurn != this.NextRespawnTurn + 1)
						{
							goto IL_2ED;
						}
					}
					TricksterAfterImageNetworkBehaviour.DisableAfterImageMaterial(this.GetActorModelData());
				}
			}
		}
		IL_2ED:
		this.m_actorVFX.OnTurnTick();
		this.m_wasUpdatingForConfirmedTargeting = false;
		this.KnockbackMoveStarted = false;
		if (this.GetActorBehavior() != null)
		{
			this.GetActorBehavior().Client_ResetKillAssistContribution();
		}
		if (this.GetActorCover() != null)
		{
			this.GetActorCover().RecalculateCover();
			if (!this.IsDead())
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
										if (this.IsVisibleToClient())
										{
											this.GetActorCover().StartShowMoveIntoCoverIndicator();
										}
									}
								}
							}
						}
					}
				}
			}
		}
		if (this.OnTurnStartDelegatesHolder != null)
		{
			this.OnTurnStartDelegatesHolder();
		}
	}

	public bool HasQueuedMovement()
	{
		bool result;
		if (!this.m_queuedMovementRequest)
		{
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

	public ActorData GetQueuedChaseTarget()
	{
		return this.m_queuedChaseTarget;
	}

	public void TurnToDirection(Vector3 dir)
	{
		Quaternion quaternion = Quaternion.LookRotation(dir);
		if (Quaternion.Angle(quaternion, this.m_targetRotation.GetEndValue()) > 0.01f)
		{
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
				this.m_targetRotation.EaseTo(quaternion, turnDuration);
			}
		}
	}

	public float GetRotationTimeRemaining()
	{
		return this.m_targetRotation.CalcTimeRemaining();
	}

	public void TurnToPositionInstant(Vector3 position)
	{
		Vector3 vector = position - base.transform.position;
		vector.y = 0f;
		if (vector != Vector3.zero)
		{
			Quaternion quaternion = default(Quaternion);
			quaternion.SetLookRotation(vector);
			base.transform.localRotation = quaternion;
			this.m_targetRotation.SnapTo(quaternion);
		}
	}

	public Rigidbody GetRigidBody(string boneName)
	{
		Rigidbody result = null;
		GameObject gameObject = base.gameObject.FindInChildren(boneName, 0);
		if (gameObject)
		{
			result = gameObject.GetComponentInChildren<Rigidbody>();
		}
		else
		{
			Log.Warning(string.Format("GetRigidBody trying to find body of bone {0} on actor '{1}' (obj name '{2}'), but the bone cannot be found.", boneName, this.DisplayName, base.gameObject.name), new object[0]);
		}
		return result;
	}

	public Rigidbody GetHipJointRigidBody()
	{
		if (this.m_cachedHipJoint == null)
		{
			this.m_cachedHipJoint = this.GetRigidBody("hip_JNT");
		}
		return this.m_cachedHipJoint;
	}

	public Vector3 GetHipJointRigidBodyPosition()
	{
		Rigidbody hipJointRigidBody = this.GetHipJointRigidBody();
		if (hipJointRigidBody != null)
		{
			return hipJointRigidBody.transform.position;
		}
		return base.gameObject.transform.position;
	}

	public Vector3 GetBonePosition(string bone)
	{
		Vector3 result = Vector3.zero;
		GameObject gameObject = base.gameObject.FindInChildren(bone, 0);
		if (gameObject)
		{
			result = gameObject.transform.position;
		}
		else
		{
			result = base.gameObject.transform.position;
		}
		return result;
	}

	public Quaternion GetBoneRotation(string boneName)
	{
		Quaternion result = Quaternion.identity;
		GameObject gameObject = base.gameObject.FindInChildren(boneName, 0);
		if (gameObject)
		{
			result = gameObject.transform.rotation;
		}
		else
		{
			Log.Warning(string.Format("GetBoneRotation trying to find rotation of bone {0} on actor '{1}' (obj name '{2}'), but the bone cannot be found.", boneName, this.DisplayName, base.gameObject.name), new object[0]);
			result = base.gameObject.transform.rotation;
		}
		return result;
	}

	public void OnDeselect()
	{
		if (this.GetActorController() != null)
		{
			this.GetActorController().ClearHighlights();
		}
		this.GetActorCover().UpdateCoverHighlights(null);
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
		this.GetFogOfWar().MarkForRecalculateVisibility();
		CameraManager.Get().OnActiveOwnedActorChange(this);
		if (this.GetActorMovement() != null)
		{
			this.GetActorMovement().UpdateSquaresCanMoveTo();
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
			if (gameFlowData.gameState == GameState.BothTeams_Decision)
			{
				if (gameFlowData.activeOwnedActorData != null)
				{
					ActorData activeOwnedActorData = gameFlowData.activeOwnedActorData;
					AbilityData abilityData = activeOwnedActorData.GetAbilityData();
					ActorTurnSM actorTurnSM = activeOwnedActorData.GetActorTurnSM();
					if (actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION)
					{
						Ability selectedAbility = abilityData.GetSelectedAbility();
						if (selectedAbility != null)
						{
							if (selectedAbility.Targeters != null)
							{
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
							Ability lastSelectedAbility = abilityData.GetLastSelectedAbility();
							if (this.ShouldUpdateForConfirmedTargeting(lastSelectedAbility, actorTurnSM.GetAbilityTargets().Count))
							{
								flag = lastSelectedAbility.IsActorInTargetRange(this, out inCover);
								int num2;
								if (lastSelectedAbility.IsSimpleAction())
								{
									num2 = 0;
								}
								else
								{
									num2 = actorTurnSM.GetAbilityTargets().Count - 1;
								}
								int num3 = num2;
								if (num3 >= 0)
								{
									if (lastSelectedAbility.Targeters != null)
									{
										num3 = Mathf.Clamp(num3, 0, lastSelectedAbility.Targeters.Count - 1);
										this.UpdateNameplateForTargetingAbility(activeOwnedActorData, lastSelectedAbility, flag, inCover, num3, true);
										updatingInConfirm = true;
										if (HUD_UI.Get() != null)
										{
											if (activeOwnedActorData.ForceDisplayTargetHighlight)
											{
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
								HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.StartTargetingNumberFadeout(this);
								this.m_showingTargetingNumAtFullAlpha = false;
							}
						}
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
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				bool flag = false;
				int i = 0;
				while (i < this.m_forceShowOutlineCheckers.Count)
				{
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
			UnityEngine.Debug.LogWarning("[Client] function 'System.Void ActorData::UpdateNameplateForTargetingAbility(ActorData,Ability,System.Boolean,System.Boolean,System.Int32,System.Boolean)' called on server");
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
			UnityEngine.Debug.LogWarning("[Client] function 'System.Boolean ActorData::ShouldUpdateForConfirmedTargeting(Ability,System.Int32)' called on server");
			return false;
		}
		if (lastSelectedAbility == null)
		{
			return false;
		}
		bool result;
		if (!this.ForceDisplayTargetHighlight)
		{
			if (lastSelectedAbility.Targeter != null)
			{
				if (lastSelectedAbility.Targeter.GetConfirmedTargetingRemainingTime() > 0f)
				{
					if (!lastSelectedAbility.IsSimpleAction())
					{
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
		if (!activeOwnedActorData.symbol_0012(square))
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
			return IgnoreChosenChaseTarget || !(square == activeOwnedActorData.GetQueuedChaseTarget().GetCurrentBoardSquare());
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

	public bool symbol_0012(BoardSquare square)
	{
		if (!(square == null))
		{
			if (!(square.occupant == null))
			{
				if (!(square.occupant.GetComponent<ActorData>() == null))
				{
					if (!(GameFlowData.Get() == null))
					{
						if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
						{
							ActorData component = square.occupant.GetComponent<ActorData>();
							AbilityData component2 = this.GetComponent<AbilityData>();
							if (component.IsDead())
							{
								return false;
							}
							if (component == this)
							{
								return false;
							}
							if (!component.symbol_0012(this))
							{
								return false;
							}
							if (!component2.GetQueuedAbilitiesAllowMovement())
							{
								return false;
							}
							if (component.IgnoreForAbilityHits)
							{
								return false;
							}
							return true;
						}
					}
					return false;
				}
			}
		}
		return false;
	}

	public void OnHitWhileInCover(Vector3 hitOrigin, ActorData caster)
	{
		if (!this.IsDead())
		{
			if (this.m_actorVFX != null)
			{
				this.m_actorVFX.ShowHitWhileInCoverVfx(this.GetTravelBoardSquareWorldPosition(), hitOrigin, caster);
				AudioManager.PostEvent("ablty/generic/feedback/behind_cover_hit", base.gameObject);
			}
		}
	}

	public void OnKnockbackWhileUnstoppable(Vector3 hitOrigin, ActorData caster)
	{
		if (!this.IsDead())
		{
			if (this.m_actorVFX != null)
			{
				this.m_actorVFX.ShowKnockbackWhileUnstoppableVfx(this.GetTravelBoardSquareWorldPosition(), hitOrigin, caster);
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
			audioTag = "default";
			eventName = eventAndTag;
		}
		else
		{
			audioTag = eventAndTag.Substring(0, num);
			eventName = eventAndTag.Substring(num + 1);
		}
		CharacterResourceLink characterResourceLink = this.GetCharacterResourceLink();
		if (characterResourceLink != null)
		{
			if (!characterResourceLink.AllowAudioTag(audioTag, this.m_visualInfo))
			{
				return;
			}
		}
		this.PostAudioEvent(eventName, null, AudioManager.EventAction.PlaySound);
	}

	public void PostAudioEvent(string eventName, OnEventNotify notifyCallback = null, AudioManager.EventAction action = AudioManager.EventAction.PlaySound)
	{
		CharacterResourceLink characterResourceLink = this.GetCharacterResourceLink();
		string text;
		if (characterResourceLink != null)
		{
			text = characterResourceLink.ReplaceAudioEvent(eventName, this.m_visualInfo);
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
			bool flag = this.IsDead();
			this.UnresolvedDamage = 0;
			this.UnresolvedHealing = 0;
			this.ClientUnresolvedDamage = 0;
			this.ClientUnresolvedHealing = 0;
			this.ClientUnresolvedAbsorb = 0;
			this.SetHitPoints(resolvedHitPoints);
			if (!flag && this.IsDead())
			{
				if (!this.IsModelAnimatorDisabled())
				{
					UnityEngine.Debug.LogError("Actor " + this.GetDebugName() + " died on HP resolved; he should have already been ragdolled, but wasn't.");
					this.DoVisualDeath(new ActorModelData.ImpulseInfo(this.GetTravelBoardSquareWorldPositionForLos(), Vector3.up));
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
			UnityEngine.Debug.LogWarning("[Client] function 'System.Void ActorData::ShowDamage(System.String)' called on server");
			return;
		}
	}

	[ClientRpc]
	internal void RpcApplyAbilityModById(int actionTypeInt, int abilityScopeId)
	{
		if (!NetworkServer.active)
		{
			if (NetworkClient.active && abilityScopeId >= 0)
			{
				this.ApplyAbilityModById(actionTypeInt, abilityScopeId);
			}
		}
	}

	internal void ApplyAbilityModById(int actionTypeInt, int abilityScopeId)
	{
		bool flag;
		if (GameManager.Get().GameConfig.GameType != GameType.Tutorial)
		{
			flag = AbilityModHelper.IsModAllowed(this.m_characterType, actionTypeInt, abilityScopeId);
		}
		else
		{
			flag = true;
		}
		if (!flag)
		{
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
			Ability abilityOfActionType = component.GetAbilityOfActionType((AbilityData.ActionType)actionTypeInt);
			AbilityMod abilityModForAbilityById = AbilityModManager.Get().GetAbilityModForAbilityById(abilityOfActionType, abilityScopeId);
			if (abilityModForAbilityById != null)
			{
				GameType gameType = GameManager.Get().GameConfig.GameType;
				GameSubType instanceSubType = GameManager.Get().GameConfig.InstanceSubType;
				if (abilityModForAbilityById.EquippableForGameType())
				{
					this.ApplyAbilityModToAbility(abilityOfActionType, abilityModForAbilityById, false);
					if (NetworkServer.active)
					{
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

	internal void no_op(int symbol_001D, int symbol_000E)
	{
	}

	private void ApplyAbilityModToAbility(Ability ability, AbilityMod abilityMod, bool log = false)
	{
		if (ability.GetType() == abilityMod.GetTargetAbilityType())
		{
			ability.ApplyAbilityMod(abilityMod, this);
			if (abilityMod.m_useChainAbilityOverrides)
			{
				ability.SanitizeChainAbilities();
				this.GetAbilityData().ReInitializeChainAbilityList();
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
				UnityEngine.Debug.LogWarning("Applied " + abilityMod.GetDebugIdentifier("white") + " to ability " + ability.GetDebugIdentifier("orange"));
			}
		}
	}

	[ClientRpc]
	public void RpcMarkForRecalculateClientVisibility()
	{
		if (this.GetFogOfWar() != null)
		{
			this.GetFogOfWar().MarkForRecalculateVisibility();
		}
	}

	public void ShowRespawnFlare(BoardSquare flareSquare, bool respawningThisTurn)
	{
		bool flag = GameFlowData.Get().LocalPlayerData != null && GameFlowData.Get().LocalPlayerData.GetTeamViewing() == this.GetTeam();
		bool flag2 = false;
		if (this.m_respawnPositionFlare != null)
		{
			flag2 = (this.m_respawnFlareVfxSquare == flareSquare && this.m_respawnFlareForSameTeam == flag);
			UnityEngine.Object.Destroy(this.m_respawnPositionFlare);
			this.m_respawnPositionFlare = null;
			UICharacterMovementPanel.Get().RemoveRespawnIndicator(this);
			this.m_respawnFlareVfxSquare = null;
			this.m_respawnFlareForSameTeam = false;
		}
		if (SpawnPointManager.Get() != null)
		{
			if (!SpawnPointManager.Get().m_spawnInDuringMovement)
			{
				return;
			}
		}
		if (flareSquare != null)
		{
			GameObject original;
			if (flag)
			{
				if (respawningThisTurn)
				{
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
			if (!ClientGameManager.Get().IsFastForward)
			{
				ClientGameManager.Get().LeaveGame(false, gameResult);
			}
		}
	}

	public void SendPingRequestToServer(int teamIndex, Vector3 worldPosition, ActorController.PingType pingType)
	{
		if (this.GetActorController() != null)
		{
			this.GetActorController().CallCmdSendMinimapPing(teamIndex, worldPosition, pingType);
		}
	}

	public void SendAbilityPingRequestToServer(int teamIndex, LocalizationArg_AbilityPing localizedPing)
	{
		if (this.GetActorController() != null)
		{
			this.GetActorController().CallCmdSendAbilityPing(teamIndex, localizedPing);
		}
	}

	public override string ToString()
	{
		return string.Format("[ActorData: {0}, {1}, ActorIndex: {2}, {3}] {4}", new object[]
		{
			this.m_displayName,
			this.GetName(),
			this.m_actorIndex,
			this.m_team,
			this.PlayerData
		});
	}

	public string GetDebugName()
	{
		return string.Concat(new object[]
		{
			"[",
			this.GetName(),
			" (",
			this.DisplayName,
			"), ",
			this.ActorIndex,
			"]"
		});
	}

	public string GetColoredDebugName(string color)
	{
		return string.Concat(new string[]
		{
			"<color=",
			color,
			">",
			this.GetDebugName(),
			"</color>"
		});
	}

	public string GetPointsDebugString()
	{
		int num = this.ExpectedHoTTotal + this.ClientExpectedHoTTotalAdjust;
		int expectedHoTThisTurn = this.ExpectedHoTThisTurn;
		int clientAppliedHoTThisTurn = this.ClientAppliedHoTThisTurn;
		return string.Concat(new object[]
		{
			"Max HP: ",
			this.GetMaxHitPoints(),
			"\nHP to Display: ",
			this.GetHitPointsAfterResolution(),
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
			this.GetEnergyToDisplay(),
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

	public string GetActorTurnSMDebugString()
	{
		string text = string.Empty;
		if (this.GetActorTurnSM() != null)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"ActorTurnSM: CurrentState= ",
				this.GetActorTurnSM().CurrentState,
				" | PrevState= ",
				this.GetActorTurnSM().PreviousState,
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
			return;
		}
		Gizmos.color = Color.green;
		if (this.GetCurrentBoardSquare() != null)
		{
			Gizmos.DrawWireCube(this.GetCurrentBoardSquare().CameraBounds.center, this.GetCurrentBoardSquare().CameraBounds.size * 0.9f);
			Gizmos.DrawRay(this.GetCurrentBoardSquare().ToVector3(), base.transform.forward);
		}
	}

	public bool HasTag(string tag)
	{
		if (this.m_actorTags != null)
		{
			return this.m_actorTags.HasTag(tag);
		}
		return false;
	}

	public void AddTag(string tag)
	{
		if (this.m_actorTags == null)
		{
			this.m_actorTags = base.gameObject.AddComponent<ActorTag>();
		}
		this.m_actorTags.AddTag(tag);
	}

	public void RemoveTag(string tag)
	{
		if (this.m_actorTags != null)
		{
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

	public CharacterResourceLink GetCharacterResourceLink()
	{
		if (this.m_characterResourceLink == null)
		{
			if (this.m_characterType != CharacterType.None)
			{
				GameWideData gameWideData = GameWideData.Get();
				if (gameWideData)
				{
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
			return null;
		}
		CharacterResourceLink characterResourceLink = this.GetCharacterResourceLink();
		if (characterResourceLink == null)
		{
			return originalSequencePrefab;
		}
		return characterResourceLink.ReplaceSequence(originalSequencePrefab, this.m_visualInfo, this.m_abilityVfxSwapInfo);
	}

	public void OnAnimEvent(UnityEngine.Object eventObject, GameObject sourceObject)
	{
		if (this.OnAnimationEventDelegatesHolder != null)
		{
			this.OnAnimationEventDelegatesHolder(eventObject, sourceObject);
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.GametimeScaleChange)
		{
			Animator modelAnimator = this.GetModelAnimator();
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
			UnityEngine.Debug.LogError("Command CmdSetPausedForDebugging called on client.");
			return;
		}
		((ActorData)obj).CmdSetPausedForDebugging(reader.ReadBoolean());
	}

	protected static void InvokeCmdCmdSetResolutionSingleStepping(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogError("Command CmdSetResolutionSingleStepping called on client.");
			return;
		}
		((ActorData)obj).CmdSetResolutionSingleStepping(reader.ReadBoolean());
	}

	protected static void InvokeCmdCmdSetResolutionSingleSteppingAdvance(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogError("Command CmdSetResolutionSingleSteppingAdvance called on client.");
			return;
		}
		((ActorData)obj).CmdSetResolutionSingleSteppingAdvance();
	}

	protected static void InvokeCmdCmdSetDebugToggleParam(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogError("Command CmdSetDebugToggleParam called on client.");
			return;
		}
		((ActorData)obj).CmdSetDebugToggleParam(reader.ReadString(), reader.ReadBoolean());
	}

	protected static void InvokeCmdCmdDebugReslotCards(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
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
			UnityEngine.Debug.LogError("Command function CmdSetResolutionSingleStepping called on server.");
			return;
		}
		if (base.isServer)
		{
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
			UnityEngine.Debug.LogError("Command function CmdSetDebugToggleParam called on server.");
			return;
		}
		if (base.isServer)
		{
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
			UnityEngine.Debug.LogError("Command function CmdDebugReslotCards called on server.");
			return;
		}
		if (base.isServer)
		{
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
			UnityEngine.Debug.LogError("Command function CmdDebugSetAbilityMod called on server.");
			return;
		}
		if (base.isServer)
		{
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
		symbol_001D,
		TricksterAfterImage
	}

	public enum MovementChangeType
	{
		MoreMovement,
		LessMovement
	}
}
