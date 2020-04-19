using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class AbilityData : NetworkBehaviour
{
	public const int INVALID_ACTION = -1;

	public const int ABILITY_0 = 0;

	public const int ABILITY_1 = 1;

	public const int ABILITY_2 = 2;

	public const int ABILITY_3 = 3;

	public const int ABILITY_4 = 4;

	public const int ABILITY_5 = 5;

	public const int ABILITY_6 = 6;

	public const int CARD_0 = 7;

	public const int CARD_1 = 8;

	public const int CARD_2 = 9;

	public const int CHAIN_0 = 0xA;

	public const int CHAIN_1 = 0xB;

	public const int CHAIN_2 = 0xC;

	public const int CHAIN_3 = 0xD;

	public const int NUM_ACTIONS = 0xE;

	public const int NUM_ABILITIES = 7;

	public const int NUM_CARDS = 3;

	public const int LAST_CARD = 9;

	private SyncListInt m_cooldownsSync = new SyncListInt();

	private SyncListInt m_consumedStockCount = new SyncListInt();

	private SyncListInt m_stockRefreshCountdowns = new SyncListInt();

	private SyncListInt m_currentCardIds = new SyncListInt();

	private AbilityData.AbilityEntry[] m_abilities;

	private List<Ability> m_allChainAbilities;

	private List<AbilityData.ActionType> m_allChainAbilityParentActionTypes;

	private Dictionary<CardType, Card> m_cardTypeToCardInstance = new Dictionary<CardType, Card>();

	private List<Ability> m_cachedCardAbilities = new List<Ability>();

	private Dictionary<string, int> m_cooldowns;

	[Header("-- Whether to skip for localization for abilities and mods --")]
	public bool m_ignoreForLocalization;

	[Header("-- Abilities --")]
	public Ability m_ability0;

	[AssetFileSelector("", "", "")]
	public string m_spritePath0;

	[Space(5f)]
	public Ability m_ability1;

	[AssetFileSelector("", "", "")]
	public string m_spritePath1;

	[Space(5f)]
	public Ability m_ability2;

	[AssetFileSelector("", "", "")]
	public string m_spritePath2;

	[Space(5f)]
	public Ability m_ability3;

	[AssetFileSelector("", "", "")]
	public string m_spritePath3;

	[Space(5f)]
	public Ability m_ability4;

	[AssetFileSelector("", "", "")]
	public string m_spritePath4;

	[Header("NOTE: Ability 5 and 6 are unused at the moment")]
	public Ability m_ability5;

	[AssetFileSelector("", "", "")]
	public string m_spritePath5;

	public Ability m_ability6;

	[AssetFileSelector("", "", "")]
	public string m_spritePath6;

	[Header("The first matching entry is used for mods. Or the mods marked as 'default'.")]
	public List<AbilityData.BotAbilityModSet> m_botDifficultyAbilityModSets = new List<AbilityData.BotAbilityModSet>();

	private List<Ability> m_abilitiesList;

	[Separator("For Ability Kit Inspector", true)]
	public List<Component> m_compsToInspectInAbilityKitInspector;

	[Header("-- Name of directory where sequence prefab is located")]
	public string m_sequenceDirNameOverride = string.Empty;

	public AbilitySetupNotes m_setupNotes;

	private ActorData m_softTargetedActor;

	private Ability m_selectedAbility;

	[SyncVar]
	private AbilityData.ActionType m_selectedActionForTargeting = AbilityData.ActionType.INVALID_ACTION;

	private List<AbilityData.ActionType> m_actionsToCancelForTurnRedo;

	private bool m_loggedErrorForNullAction;

	private bool m_cancelMovementForTurnRedo;

	private AbilityData.ActionType m_actionToSelectWhenEnteringDecisionState = AbilityData.ActionType.INVALID_ACTION;

	private bool m_retargetActionWithoutClearingOldAbilities;

	private ActorData m_actor;

	private float m_lastPingSendTime;

	private bool m_abilitySpritesInitialized;

	public static Color s_freeActionTextColor = new Color(0f, 1f, 0f);

	public static float s_heightPerButton = 75f;

	public static float s_heightFromBottom = 75f;

	public static float s_widthPerButton = 64f;

	public static int s_abilityButtonSlots = 8;

	public static float s_widthOfAllButtons = AbilityData.s_widthPerButton * (float)AbilityData.s_abilityButtonSlots;

	private Ability m_lastSelectedAbility;

	[CompilerGenerated]
	private static Comparison<ActorData> <>f__mg$cache0;

	private static int kListm_cooldownsSync;

	private static int kListm_consumedStockCount;

	private static int kListm_stockRefreshCountdowns;

	private static int kListm_currentCardIds;

	private static int kCmdCmdClearCooldowns = 0x228F4CF;

	private static int kCmdCmdRefillStocks;

	static AbilityData()
	{
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbilityData), AbilityData.kCmdCmdClearCooldowns, new NetworkBehaviour.CmdDelegate(AbilityData.InvokeCmdCmdClearCooldowns));
		AbilityData.kCmdCmdRefillStocks = 0x42186527;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbilityData), AbilityData.kCmdCmdRefillStocks, new NetworkBehaviour.CmdDelegate(AbilityData.InvokeCmdCmdRefillStocks));
		AbilityData.kListm_cooldownsSync = -0x6512D205;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(AbilityData), AbilityData.kListm_cooldownsSync, new NetworkBehaviour.CmdDelegate(AbilityData.InvokeSyncListm_cooldownsSync));
		AbilityData.kListm_consumedStockCount = 0x52CC1FC9;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(AbilityData), AbilityData.kListm_consumedStockCount, new NetworkBehaviour.CmdDelegate(AbilityData.InvokeSyncListm_consumedStockCount));
		AbilityData.kListm_stockRefreshCountdowns = -0x3C9C58B1;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(AbilityData), AbilityData.kListm_stockRefreshCountdowns, new NetworkBehaviour.CmdDelegate(AbilityData.InvokeSyncListm_stockRefreshCountdowns));
		AbilityData.kListm_currentCardIds = 0x16E4E7F7;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(AbilityData), AbilityData.kListm_currentCardIds, new NetworkBehaviour.CmdDelegate(AbilityData.InvokeSyncListm_currentCardIds));
		NetworkCRC.RegisterBehaviour("AbilityData", 0);
	}

	public SyncListInt CurrentCardIDs
	{
		get
		{
			return this.m_currentCardIds;
		}
	}

	public AbilityData.AbilityEntry[] abilityEntries
	{
		get
		{
			return this.m_abilities;
		}
	}

	private Sprite GetSpriteFromPath(string path)
	{
		if (path.IsNullOrEmpty())
		{
			return null;
		}
		return (Sprite)Resources.Load(path, typeof(Sprite));
	}

	public Sprite m_sprite0
	{
		get
		{
			return this.GetSpriteFromPath(this.m_spritePath0);
		}
	}

	public Sprite m_sprite1
	{
		get
		{
			return this.GetSpriteFromPath(this.m_spritePath1);
		}
	}

	public Sprite m_sprite2
	{
		get
		{
			return this.GetSpriteFromPath(this.m_spritePath2);
		}
	}

	public Sprite m_sprite3
	{
		get
		{
			return this.GetSpriteFromPath(this.m_spritePath3);
		}
	}

	public Sprite m_sprite4
	{
		get
		{
			return this.GetSpriteFromPath(this.m_spritePath4);
		}
	}

	public Sprite m_sprite5
	{
		get
		{
			return this.GetSpriteFromPath(this.m_spritePath5);
		}
	}

	public Sprite m_sprite6
	{
		get
		{
			return this.GetSpriteFromPath(this.m_spritePath6);
		}
	}

	public List<Ability> GetAbilitiesAsList()
	{
		if (this.m_abilitiesList != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetAbilitiesAsList()).MethodHandle;
			}
			if (this.m_abilitiesList.Count != 0)
			{
				goto IL_88;
			}
		}
		this.m_abilitiesList = new List<Ability>();
		this.m_abilitiesList.Add(this.m_ability0);
		this.m_abilitiesList.Add(this.m_ability1);
		this.m_abilitiesList.Add(this.m_ability2);
		this.m_abilitiesList.Add(this.m_ability3);
		this.m_abilitiesList.Add(this.m_ability4);
		IL_88:
		return this.m_abilitiesList;
	}

	public Ability GetAbilityAtIndex(int index)
	{
		switch (index)
		{
		case 0:
			return this.m_ability0;
		case 1:
			return this.m_ability1;
		case 2:
			return this.m_ability2;
		case 3:
			return this.m_ability3;
		case 4:
			return this.m_ability4;
		default:
			return null;
		}
	}

	public ActorData SoftTargetedActor
	{
		get
		{
			return this.m_softTargetedActor;
		}
		set
		{
			if (this.m_softTargetedActor != value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.set_SoftTargetedActor(ActorData)).MethodHandle;
				}
				this.m_softTargetedActor = value;
				ActorData actor = this.m_actor;
				if (actor == GameFlowData.Get().activeOwnedActorData)
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
					CameraManager cameraManager = CameraManager.Get();
					if (this.m_softTargetedActor)
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
						cameraManager.SetTargetObjectToMouse(this.m_softTargetedActor.gameObject, CameraManager.CameraTargetReason.AbilitySoftTargeting);
					}
					else if (!cameraManager.IsOnMainCamera(GameFlowData.Get().activeOwnedActorData))
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
						cameraManager.SetTargetObject(base.gameObject, CameraManager.CameraTargetReason.AbilitySoftTargeting);
					}
				}
			}
		}
	}

	public AbilityData.ActionType GetSelectedActionTypeForTargeting()
	{
		return this.m_selectedActionForTargeting;
	}

	public bool HasToggledAction(AbilityData.ActionType actionType)
	{
		bool result = false;
		ActorTeamSensitiveData teamSensitiveData_authority = this.m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.HasToggledAction(AbilityData.ActionType)).MethodHandle;
			}
			result = teamSensitiveData_authority.HasToggledAction(actionType);
		}
		return result;
	}

	public bool HasQueuedAction(AbilityData.ActionType actionType)
	{
		bool result = false;
		ActorTeamSensitiveData teamSensitiveData_authority = this.m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.HasQueuedAction(AbilityData.ActionType)).MethodHandle;
			}
			result = teamSensitiveData_authority.HasQueuedAction(actionType);
		}
		return result;
	}

	public bool HasQueuedAbilityOfType(Type abilityType)
	{
		AbilityData.ActionType actionTypeOfAbility = this.GetActionTypeOfAbility(this.GetAbilityOfType(abilityType));
		return this.HasQueuedAction(actionTypeOfAbility);
	}

	public bool HasQueuedAbilityInPhase(AbilityPriority phase)
	{
		bool result = false;
		ActorTeamSensitiveData teamSensitiveData_authority = this.m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.HasQueuedAbilityInPhase(AbilityPriority)).MethodHandle;
			}
			result = teamSensitiveData_authority.HasQueuedAbilityInPhase(phase);
		}
		return result;
	}

	private void Awake()
	{
		this.m_abilities = new AbilityData.AbilityEntry[0xE];
		for (int i = 0; i < 0xE; i++)
		{
			this.m_abilities[i] = new AbilityData.AbilityEntry();
		}
		this.m_abilities[0].Setup(this.m_ability0, KeyPreference.Ability1);
		this.m_abilities[1].Setup(this.m_ability1, KeyPreference.Ability2);
		this.m_abilities[2].Setup(this.m_ability2, KeyPreference.Ability3);
		this.m_abilities[3].Setup(this.m_ability3, KeyPreference.Ability4);
		this.m_abilities[4].Setup(this.m_ability4, KeyPreference.Ability5);
		this.InitAbilitySprites();
		this.m_allChainAbilities = new List<Ability>();
		this.m_allChainAbilityParentActionTypes = new List<AbilityData.ActionType>();
		for (int j = 0; j < this.m_abilities.Length; j++)
		{
			AbilityData.AbilityEntry abilityEntry = this.m_abilities[j];
			if (abilityEntry.ability != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.Awake()).MethodHandle;
				}
				foreach (Ability ability in abilityEntry.ability.GetChainAbilities())
				{
					if (ability != null)
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
						this.AddToAllChainAbilitiesList(ability, (AbilityData.ActionType)j);
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
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		this.m_cooldowns = new Dictionary<string, int>();
		this.m_actor = base.GetComponent<ActorData>();
		for (int l = 0; l < 3; l++)
		{
			this.m_cachedCardAbilities.Add(null);
		}
		this.m_cooldownsSync.InitializeBehaviour(this, AbilityData.kListm_cooldownsSync);
		this.m_consumedStockCount.InitializeBehaviour(this, AbilityData.kListm_consumedStockCount);
		this.m_stockRefreshCountdowns.InitializeBehaviour(this, AbilityData.kListm_stockRefreshCountdowns);
		this.m_currentCardIds.InitializeBehaviour(this, AbilityData.kListm_currentCardIds);
	}

	public void InitAbilitySprites()
	{
		if (!this.m_abilitySpritesInitialized)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.InitAbilitySprites()).MethodHandle;
			}
			this.SetSpriteForAbility(this.m_ability0, this.m_sprite0);
			this.SetSpriteForAbility(this.m_ability1, this.m_sprite1);
			this.SetSpriteForAbility(this.m_ability2, this.m_sprite2);
			this.SetSpriteForAbility(this.m_ability3, this.m_sprite3);
			this.SetSpriteForAbility(this.m_ability4, this.m_sprite4);
			this.m_abilitySpritesInitialized = true;
		}
	}

	private void AddToAllChainAbilitiesList(Ability aChainAbility, AbilityData.ActionType parentActionType)
	{
		this.m_allChainAbilities.Add(aChainAbility);
		this.m_allChainAbilityParentActionTypes.Add(parentActionType);
	}

	private void ClearAllChainAbilitiesList()
	{
		this.m_allChainAbilities.Clear();
		this.m_allChainAbilityParentActionTypes.Clear();
	}

	private void SetSpriteForAbility(Ability ability, Sprite sprite)
	{
		if (ability != null)
		{
			ability.sprite = sprite;
			if (ability.m_chainAbilities != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.SetSpriteForAbility(Ability, Sprite)).MethodHandle;
				}
				foreach (Ability ability2 in ability.m_chainAbilities)
				{
					if (ability2 != null)
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
						ability2.sprite = sprite;
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

	public override void OnStartClient()
	{
		this.m_cooldownsSync.Callback = new SyncList<int>.SyncListChanged(this.SyncListCallbackCoolDownsSync);
		this.m_currentCardIds.Callback = new SyncList<int>.SyncListChanged(this.SyncListCallbackCurrentCardsChanged);
	}

	public override void OnStartServer()
	{
		for (int i = 0; i < 0xE; i++)
		{
			this.m_cooldownsSync.Add(0);
			this.m_consumedStockCount.Add(0);
			this.m_stockRefreshCountdowns.Add(0);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.OnStartServer()).MethodHandle;
		}
		for (int j = 0; j < 3; j++)
		{
			this.m_currentCardIds.Add(-1);
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
		if (GameplayUtils.IsPlayerControlled(this))
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
			int num = GameplayData.Get().m_turnsAbilitiesUnlock.Length;
			int k = 0;
			while (k < num)
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
				if (k >= 7)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						goto IL_C0;
					}
				}
				else
				{
					this.PlaceInCooldownTillTurn((AbilityData.ActionType)k, GameplayData.Get().m_turnsAbilitiesUnlock[k]);
					k++;
				}
			}
		}
		IL_C0:
		this.InitializeStockCounts();
	}

	public void ReInitializeChainAbilityList()
	{
		this.ClearAllChainAbilitiesList();
		for (int i = 0; i < this.m_abilities.Length; i++)
		{
			AbilityData.AbilityEntry abilityEntry = this.m_abilities[i];
			if (abilityEntry.ability != null)
			{
				foreach (Ability ability in abilityEntry.ability.GetChainAbilities())
				{
					if (ability != null)
					{
						this.AddToAllChainAbilitiesList(ability, (AbilityData.ActionType)i);
					}
				}
			}
		}
	}

	internal static bool IsCard(AbilityData.ActionType actionType)
	{
		bool result;
		if (actionType >= AbilityData.ActionType.CARD_0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.IsCard(AbilityData.ActionType)).MethodHandle;
			}
			result = (actionType <= AbilityData.ActionType.CARD_2);
		}
		else
		{
			result = false;
		}
		return result;
	}

	internal static bool IsChain(AbilityData.ActionType actionType)
	{
		bool result;
		if (actionType >= AbilityData.ActionType.CHAIN_0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.IsChain(AbilityData.ActionType)).MethodHandle;
			}
			result = (actionType <= AbilityData.ActionType.CHAIN_2);
		}
		else
		{
			result = false;
		}
		return result;
	}

	internal static bool IsCharacterSpecificAbility(AbilityData.ActionType actionType)
	{
		if (actionType == AbilityData.ActionType.INVALID_ACTION)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.IsCharacterSpecificAbility(AbilityData.ActionType)).MethodHandle;
			}
			return false;
		}
		if (AbilityData.IsCard(actionType))
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
			return false;
		}
		return true;
	}

	internal List<CameraShotSequence> GetTauntListForActionTypeForPlayer(PersistedCharacterData characterData, CharacterResourceLink character, AbilityData.ActionType actionType)
	{
		List<CameraShotSequence> list = new List<CameraShotSequence>();
		if (characterData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetTauntListForActionTypeForPlayer(PersistedCharacterData, CharacterResourceLink, AbilityData.ActionType)).MethodHandle;
			}
			Ability abilityOfActionType = this.GetAbilityOfActionType(actionType);
			if (abilityOfActionType != null)
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
				for (int i = 0; i < character.m_taunts.Count; i++)
				{
					CharacterTaunt characterTaunt = character.m_taunts[i];
					if (characterTaunt.m_actionForTaunt == actionType)
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
						if (i < characterData.CharacterComponent.Taunts.Count && characterData.CharacterComponent.Taunts[i].Unlocked)
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
							TauntCameraSet tauntCamSetData = this.m_actor.m_tauntCamSetData;
							CameraShotSequence cameraShotSequence;
							if (tauntCamSetData != null)
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
								cameraShotSequence = tauntCamSetData.GetTauntCam(characterTaunt.m_uniqueID);
							}
							else
							{
								cameraShotSequence = null;
							}
							CameraShotSequence cameraShotSequence2 = cameraShotSequence;
							if (cameraShotSequence2 != null)
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
								if (abilityOfActionType.CanTriggerAnimAtIndexForTaunt(cameraShotSequence2.m_animIndex))
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
									list.Add(cameraShotSequence2);
								}
							}
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
		}
		return list;
	}

	internal List<CharacterTaunt> GetFullTauntListForActionType(CharacterResourceLink character, AbilityData.ActionType actionType, bool includeHidden = false)
	{
		List<CharacterTaunt> list = new List<CharacterTaunt>();
		for (int i = 0; i < character.m_taunts.Count; i++)
		{
			CharacterTaunt characterTaunt = character.m_taunts[i];
			if (characterTaunt.m_actionForTaunt == actionType)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetFullTauntListForActionType(CharacterResourceLink, AbilityData.ActionType, bool)).MethodHandle;
				}
				if (!includeHidden)
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
					if (characterTaunt.m_isHidden)
					{
						goto IL_5B;
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
				list.Add(characterTaunt);
			}
			IL_5B:;
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
		return list;
	}

	internal static bool CanTauntForActionTypeForPlayer(PersistedCharacterData characterData, CharacterResourceLink character, AbilityData.ActionType actionType, bool checkTauntUniqueId, int uniqueId)
	{
		if (CameraManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.CanTauntForActionTypeForPlayer(PersistedCharacterData, CharacterResourceLink, AbilityData.ActionType, bool, int)).MethodHandle;
			}
			if (CameraManager.Get().m_abilityCinematicState == CameraManager.AbilityCinematicState.Never)
			{
				return false;
			}
		}
		if (characterData != null && character.m_characterType != CharacterType.None && actionType != AbilityData.ActionType.INVALID_ACTION)
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
			int count = characterData.CharacterComponent.Taunts.Count;
			int i = 0;
			while (i < character.m_taunts.Count)
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
				if (i >= count)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						return false;
					}
				}
				else
				{
					CharacterTaunt characterTaunt = character.m_taunts[i];
					if (characterTaunt != null)
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
						if (characterTaunt.m_actionForTaunt == actionType)
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
							if (checkTauntUniqueId)
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
								if (characterTaunt.m_uniqueID != uniqueId)
								{
									goto IL_10A;
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
							if (characterData.CharacterComponent.Taunts[i].Unlocked && GameManager.Get().GameplayOverrides.IsTauntAllowed(character.m_characterType, (int)actionType, characterTaunt.m_uniqueID))
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
								return true;
							}
						}
					}
					IL_10A:
					i++;
				}
			}
		}
		return false;
	}

	internal static bool CanTauntApplyToActionType(CharacterResourceLink character, AbilityData.ActionType actionType)
	{
		if (CameraManager.Get() != null && CameraManager.Get().m_abilityCinematicState == CameraManager.AbilityCinematicState.Never)
		{
			return false;
		}
		if (character.m_characterType != CharacterType.None)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.CanTauntApplyToActionType(CharacterResourceLink, AbilityData.ActionType)).MethodHandle;
			}
			if (actionType != AbilityData.ActionType.INVALID_ACTION)
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
				int num = 0;
				foreach (CharacterTaunt characterTaunt in character.m_taunts)
				{
					if (characterTaunt.m_actionForTaunt == actionType)
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
						if (GameManager.Get().GameplayOverrides.IsTauntAllowed(character.m_characterType, (int)actionType, characterTaunt.m_uniqueID))
						{
							return true;
						}
					}
					num++;
				}
				return false;
			}
		}
		return false;
	}

	public List<AbilityData.AbilityEntry> GetQueuedOrAimingAbilitiesForPhase(UIQueueListPanel.UIPhase actionPhase)
	{
		List<AbilityData.AbilityEntry> list = new List<AbilityData.AbilityEntry>();
		ActorTeamSensitiveData teamSensitiveData_authority = this.m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetQueuedOrAimingAbilitiesForPhase(UIQueueListPanel.UIPhase)).MethodHandle;
			}
			int i = 0;
			while (i < 0xE)
			{
				Ability ability = this.m_abilities[i].ability;
				if (teamSensitiveData_authority.HasQueuedAction(i))
				{
					goto IL_8E;
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
				if (ability != null)
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
					if (ability == this.GetSelectedAbility())
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							goto IL_8E;
						}
					}
				}
				IL_BE:
				i++;
				continue;
				IL_8E:
				UIQueueListPanel.UIPhase uiphaseFromAbilityPriority = UIQueueListPanel.GetUIPhaseFromAbilityPriority(this.m_abilities[i].ability.RunPriority);
				if (uiphaseFromAbilityPriority == actionPhase)
				{
					list.Add(this.m_abilities[i]);
					goto IL_BE;
				}
				goto IL_BE;
			}
		}
		return list;
	}

	public List<AbilityData.AbilityEntry> GetQueuedOrAimingAbilities()
	{
		List<AbilityData.AbilityEntry> list = new List<AbilityData.AbilityEntry>();
		ActorTeamSensitiveData teamSensitiveData_authority = this.m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetQueuedOrAimingAbilities()).MethodHandle;
			}
			int i = 0;
			while (i < 0xE)
			{
				Ability ability = this.m_abilities[i].ability;
				if (teamSensitiveData_authority.HasQueuedAction(i))
				{
					goto IL_81;
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
				if (ability != null && ability == this.GetSelectedAbility())
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						goto IL_81;
					}
				}
				IL_8F:
				i++;
				continue;
				IL_81:
				list.Add(this.m_abilities[i]);
				goto IL_8F;
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
		return list;
	}

	private static int CompareTabTargetsByActiveOwnedActorDistance(ActorData a, ActorData b)
	{
		if (GameFlowData.Get().activeOwnedActorData == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.CompareTabTargetsByActiveOwnedActorDistance(ActorData, ActorData)).MethodHandle;
			}
			return 0;
		}
		Vector3 position = GameFlowData.Get().activeOwnedActorData.transform.position;
		float sqrMagnitude = (position - a.transform.position).sqrMagnitude;
		float sqrMagnitude2 = (position - b.transform.position).sqrMagnitude;
		int result;
		if (sqrMagnitude == sqrMagnitude2)
		{
			result = 0;
		}
		else if (sqrMagnitude > sqrMagnitude2)
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
			result = 1;
		}
		else
		{
			result = -1;
		}
		return result;
	}

	public void NextSoftTarget()
	{
		ActorTurnSM actorTurnSM = this.m_actor.\u000E();
		if (actorTurnSM)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.NextSoftTarget()).MethodHandle;
			}
			int targetSelectionIndex = actorTurnSM.GetTargetSelectionIndex();
			int numTargets = this.m_selectedAbility.GetNumTargets();
			if (targetSelectionIndex >= numTargets)
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
				this.SoftTargetedActor = null;
			}
			else
			{
				List<ActorData> validTargets = this.GetValidTargets(this.m_selectedAbility, targetSelectionIndex);
				if (validTargets.Count > 0)
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
					List<ActorData> list = validTargets;
					if (AbilityData.<>f__mg$cache0 == null)
					{
						AbilityData.<>f__mg$cache0 = new Comparison<ActorData>(AbilityData.CompareTabTargetsByActiveOwnedActorDistance);
					}
					list.Sort(AbilityData.<>f__mg$cache0);
					if (this.SoftTargetedActor == null)
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
						this.SoftTargetedActor = validTargets[0];
					}
					else
					{
						int num = validTargets.FindIndex((ActorData entry) => entry == this.SoftTargetedActor);
						ActorData softTargetedActor = validTargets[(num + 1) % validTargets.Count];
						this.SoftTargetedActor = softTargetedActor;
					}
				}
				else
				{
					this.SoftTargetedActor = null;
				}
			}
		}
	}

	private void Update()
	{
		if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.Update()).MethodHandle;
			}
			if (GameFlowData.Get().activeOwnedActorData == this.m_actor)
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
				ActorTurnSM actorTurnSM = this.m_actor.\u000E();
				if (this.m_actionToSelectWhenEnteringDecisionState != AbilityData.ActionType.INVALID_ACTION && actorTurnSM.CurrentState == TurnStateEnum.DECIDING)
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
					if (this.RedoTurn(this.GetAbilityOfActionType(this.m_actionToSelectWhenEnteringDecisionState), this.m_actionToSelectWhenEnteringDecisionState, this.m_actionsToCancelForTurnRedo, this.m_cancelMovementForTurnRedo, this.m_retargetActionWithoutClearingOldAbilities))
					{
						this.ClearActionsToCancelOnTargetingComplete();
					}
					this.m_actionToSelectWhenEnteringDecisionState = AbilityData.ActionType.INVALID_ACTION;
				}
				else
				{
					if (this.m_actionsToCancelForTurnRedo != null)
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
						if (actorTurnSM.CurrentState == TurnStateEnum.DECIDING)
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
							if (this.RedoTurn(null, AbilityData.ActionType.INVALID_ACTION, this.m_actionsToCancelForTurnRedo, false, false))
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
								this.ClearActionsToCancelOnTargetingComplete();
							}
							return;
						}
					}
					if (!actorTurnSM.CanSelectAbility())
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
						if (!actorTurnSM.CanQueueSimpleAction())
						{
							return;
						}
					}
					if (!this.m_actor.\u000E())
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
						for (int i = 0; i < this.m_abilities.Length; i++)
						{
							AbilityData.AbilityEntry abilityEntry = this.m_abilities[i];
							if (abilityEntry != null && abilityEntry.ability != null && abilityEntry.keyPreference != KeyPreference.NullPreference && InputManager.Get().IsKeyBindingNewlyHeld(abilityEntry.keyPreference))
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
								if (!UITutorialFullscreenPanel.Get().IsAnyPanelVisible())
								{
									AbilityData.ActionType actionType = (AbilityData.ActionType)i;
									if (this.AbilityButtonPressed(actionType, abilityEntry.ability))
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
										return;
									}
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
				}
			}
		}
	}

	public bool AbilityButtonPressed(AbilityData.ActionType actionType, Ability ability)
	{
		if (ability == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.AbilityButtonPressed(AbilityData.ActionType, Ability)).MethodHandle;
			}
			return false;
		}
		bool flag = InputManager.Get().IsKeyBindingHeld(KeyPreference.MinimapPing);
		ActorTurnSM actorTurnSM = this.m_actor.\u000E();
		bool flag2 = actorTurnSM.CanSelectAbility();
		bool flag3 = actorTurnSM.CanQueueSimpleAction();
		if (!flag2 && !flag3)
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
				this.SendAbilityPing(false, actionType, ability);
			}
			return false;
		}
		this.m_cancelMovementForTurnRedo = false;
		this.m_actionsToCancelForTurnRedo = null;
		bool flag4 = InputManager.Get().IsKeyBindingHeld(KeyPreference.AbilityRetargetingModifier);
		if (this.HasQueuedAction(actionType))
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
			if (!flag4)
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
					this.SendAbilityPing(true, actionType, ability);
				}
				else if (actorTurnSM.CurrentState == TurnStateEnum.CONFIRMED)
				{
					actorTurnSM.BackToDecidingState();
					this.m_actionsToCancelForTurnRedo = new List<AbilityData.ActionType>();
					this.m_actionsToCancelForTurnRedo.Add(actionType);
					this.m_retargetActionWithoutClearingOldAbilities = false;
				}
				else
				{
					actorTurnSM.RequestCancelAction(actionType, false);
					UISounds.GetUISounds().Play("ui/ingame/v1/action_undo");
				}
				return false;
			}
		}
		List<AbilityData.ActionType> list;
		bool flag5;
		if (this.CanQueueActionByCancelingOthers(ability, actionType, flag3, flag2, out list, out flag5))
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
				this.SendAbilityPing(true, actionType, ability);
			}
			else
			{
				this.m_retargetActionWithoutClearingOldAbilities = false;
				if (actorTurnSM.CurrentState != TurnStateEnum.CONFIRMED)
				{
					if (actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION)
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
						if (this.GetSelectedActionType() == actionType)
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
							if (!SinglePlayerManager.IsCancelDisabled())
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
								this.ClearSelectedAbility();
								actorTurnSM.BackToDecidingState();
								goto IL_1ED;
							}
						}
					}
					return this.RedoTurn(ability, actionType, list, flag5, flag4);
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
				actorTurnSM.BackToDecidingState();
				this.m_cancelMovementForTurnRedo = flag5;
				this.m_actionsToCancelForTurnRedo = list;
				this.m_actionToSelectWhenEnteringDecisionState = actionType;
				this.m_retargetActionWithoutClearingOldAbilities = flag4;
			}
			IL_1ED:;
		}
		else if (flag)
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
			this.SendAbilityPing(false, actionType, ability);
		}
		return false;
	}

	private void SendAbilityPing(bool selectable, AbilityData.ActionType actionType, Ability ability)
	{
		if (TextConsole.Get() != null)
		{
			if (this.m_lastPingSendTime > 0f)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.SendAbilityPing(bool, AbilityData.ActionType, Ability)).MethodHandle;
				}
				if (Time.time - this.m_lastPingSendTime <= HUD_UIResources.Get().m_mapPingCooldown)
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
			LocalizationArg_AbilityPing localizedPing = LocalizationArg_AbilityPing.Create(this.m_actor.m_characterType, ability, selectable, Mathf.Max(this.GetAbilityEntryOfActionType(actionType).GetCooldownRemaining(), this.GetTurnsTillUnlock(actionType)), actionType == AbilityData.ActionType.ABILITY_4, this.m_actor.\u0019(), this.m_actor.\u0016());
			this.m_actor.SendAbilityPingRequestToServer((int)this.m_actor.\u000E(), localizedPing);
			this.m_lastPingSendTime = Time.time;
		}
	}

	public bool RedoTurn(Ability ability, AbilityData.ActionType actionType, List<AbilityData.ActionType> actionsToCancel, bool cancelMovement, bool retargetingModifierKeyHeld)
	{
		ActorController actorController;
		if (this.m_actor != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.RedoTurn(Ability, AbilityData.ActionType, List<AbilityData.ActionType>, bool, bool)).MethodHandle;
			}
			actorController = this.m_actor.\u000E();
		}
		else
		{
			actorController = null;
		}
		ActorController actorController2 = actorController;
		ActorTurnSM actorTurnSM;
		if (this.m_actor != null)
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
			actorTurnSM = this.m_actor.\u000E();
		}
		else
		{
			actorTurnSM = null;
		}
		ActorTurnSM actorTurnSM2 = actorTurnSM;
		if (ability != null && !ability.IsSimpleAction())
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
			if (retargetingModifierKeyHeld)
			{
				if (!actionsToCancel.IsNullOrEmpty<AbilityData.ActionType>())
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
					if (actionsToCancel.Contains(actionType))
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
						ability.BackupTargetingForRedo(actorTurnSM2);
					}
				}
				this.SetSelectedAbility(ability);
				actorController2.SendSelectAbilityRequest();
				this.m_cancelMovementForTurnRedo = cancelMovement;
				this.m_actionsToCancelForTurnRedo = actionsToCancel;
				return false;
			}
		}
		if (cancelMovement)
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
			actorTurnSM2.RequestCancelMovement();
			UISounds.GetUISounds().Play("ui/ingame/v1/move_undo");
		}
		if (actionsToCancel != null)
		{
			this.m_loggedErrorForNullAction = false;
			using (List<AbilityData.ActionType>.Enumerator enumerator = actionsToCancel.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityData.ActionType actionType2 = enumerator.Current;
					actorTurnSM2.RequestCancelAction(actionType2, true);
					UISounds.GetUISounds().Play("ui/ingame/v1/action_undo");
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
		else if (!this.m_loggedErrorForNullAction)
		{
			this.m_loggedErrorForNullAction = true;
			Debug.LogError("RedoTurn() - actionsToCancel is null");
		}
		if (ability != null)
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
			if (ability.IsSimpleAction())
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
				if (actionsToCancel != null)
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
					if (actionsToCancel.Contains(actionType))
					{
						goto IL_1E5;
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
				if (this.HasQueuedAction(actionType))
				{
					actorTurnSM2.RequestCancelAction(actionType, true);
					UISounds.GetUISounds().Play("ui/ingame/v1/action_undo");
				}
				else
				{
					actorTurnSM2.OnQueueAbilityRequest(actionType);
					actorController2.SendQueueSimpleActionRequest(actionType);
				}
				IL_1E5:;
			}
			else
			{
				this.SetSelectedAbility(ability);
				actorController2.SendSelectAbilityRequest();
			}
		}
		return true;
	}

	public unsafe bool CanQueueActionByCancelingOthers(Ability ability, AbilityData.ActionType actionType, bool canQueueSimpleAction, bool canSelectAbility, out List<AbilityData.ActionType> actionsToCancel, out bool cancelMovement)
	{
		bool flag = false;
		actionsToCancel = new List<AbilityData.ActionType>();
		cancelMovement = false;
		if (ability.IsSimpleAction())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.CanQueueActionByCancelingOthers(Ability, AbilityData.ActionType, bool, bool, List<AbilityData.ActionType>*, bool*)).MethodHandle;
			}
			if (canQueueSimpleAction)
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
				if (this.ValidateActionIsRequestableDisregardingQueuedActions(actionType))
				{
					flag = true;
				}
			}
		}
		else if (canSelectAbility)
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
			if (this.ValidateActionIsRequestableDisregardingQueuedActions(actionType))
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
		}
		if (flag)
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
			if (ability.IsFreeAction())
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
				if (ability.GetRunPriority() != AbilityPriority.Evasion)
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
					if (!AbilityData.IsCard(actionType))
					{
						if (ability.IsFreeAction() && this.HasQueuedAction(actionType))
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
							actionsToCancel.Add(actionType);
							goto IL_179;
						}
						goto IL_179;
					}
				}
			}
			for (int i = 0; i < 0xE; i++)
			{
				AbilityData.ActionType actionType2 = (AbilityData.ActionType)i;
				if (this.HasQueuedAction(actionType2))
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
					Ability abilityOfActionType = this.GetAbilityOfActionType(actionType2);
					if (abilityOfActionType != null)
					{
						if ((abilityOfActionType.IsFreeAction() || ability.IsFreeAction()) && (abilityOfActionType.GetRunPriority() != AbilityPriority.Evasion || ability.GetRunPriority() != AbilityPriority.Evasion))
						{
							if (!AbilityData.IsCard(this.GetActionTypeOfAbility(abilityOfActionType)))
							{
								goto IL_145;
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
							if (!AbilityData.IsCard(actionType))
							{
								goto IL_145;
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
						actionsToCancel.Add(actionType2);
					}
				}
				IL_145:;
			}
			IL_179:
			if (this.m_actor.HasQueuedMovement())
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
				if (ability.GetMovementAdjustment() == Ability.MovementAdjustment.NoMovement)
				{
					cancelMovement = true;
				}
				else if (ability.GetMovementAdjustment() == Ability.MovementAdjustment.ReducedMovement)
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
					cancelMovement = !this.m_actor.QueuedMovementAllowsAbility;
				}
			}
		}
		return flag;
	}

	public unsafe bool GetActionsToCancelOnTargetingComplete(ref List<AbilityData.ActionType> actionsToCancel, ref bool cancelMovement)
	{
		if (!this.m_cancelMovementForTurnRedo)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetActionsToCancelOnTargetingComplete(List<AbilityData.ActionType>*, bool*)).MethodHandle;
			}
			if (this.m_actionsToCancelForTurnRedo.IsNullOrEmpty<AbilityData.ActionType>())
			{
				return false;
			}
		}
		cancelMovement = this.m_cancelMovementForTurnRedo;
		actionsToCancel = this.m_actionsToCancelForTurnRedo;
		return true;
	}

	public void ClearActionsToCancelOnTargetingComplete()
	{
		this.m_cancelMovementForTurnRedo = false;
		this.m_actionsToCancelForTurnRedo = null;
		this.m_actionToSelectWhenEnteringDecisionState = AbilityData.ActionType.INVALID_ACTION;
		if (this.m_lastSelectedAbility != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.ClearActionsToCancelOnTargetingComplete()).MethodHandle;
			}
			this.m_lastSelectedAbility.DestroyBackupTargetingInfo(false);
		}
		this.m_retargetActionWithoutClearingOldAbilities = false;
	}

	public List<ActorData> GetValidTargets(Ability testAbility, int targetIndex)
	{
		ActorData actor = this.m_actor;
		FogOfWar component = base.GetComponent<FogOfWar>();
		if (actor.\u000E())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetValidTargets(Ability, int)).MethodHandle;
			}
			return new List<ActorData>();
		}
		List<ActorData> list = new List<ActorData>();
		bool checkLoS = testAbility.GetCheckLoS(targetIndex);
		bool flag = GameplayUtils.IsPlayerControlled(this);
		using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().GetActors().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (flag)
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
					if (!component.IsVisible(actorData.\u0012()))
					{
						continue;
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
				if (checkLoS)
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
					if (!component.IsVisibleBySelf(actorData.\u0012()))
					{
						if (!actor.\u000E(actorData))
						{
							continue;
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
				list.Add(actorData);
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
		List<ActorData> list2 = new List<ActorData>();
		for (int i = 0; i < list.Count; i++)
		{
			ActorData actorData2 = list[i];
			if (!actorData2.\u000E())
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
				AbilityTarget target = AbilityTarget.CreateAbilityTargetFromActor(actorData2, actor);
				if (this.ValidateAbilityOnTarget(testAbility, target, targetIndex, -1f, -1f))
				{
					list2.Add(actorData2);
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
		return list2;
	}

	private void SyncListCallbackCoolDownsSync(SyncList<int>.Operation op, int _incorrectIndexBugIn51And52)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.SyncListCallbackCoolDownsSync(SyncList<int>.Operation, int)).MethodHandle;
			}
			for (int i = 0; i < 0xE; i++)
			{
				this.m_abilities[i].SetCooldownRemaining(this.m_cooldownsSync[i]);
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
	}

	private void SyncListCallbackCurrentCardsChanged(SyncList<int>.Operation op, int index)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.SyncListCallbackCurrentCardsChanged(SyncList<int>.Operation, int)).MethodHandle;
			}
			if (CardManagerData.Get() != null)
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
				for (int i = 0; i < this.m_currentCardIds.Count; i++)
				{
					Ability useAbility = null;
					if (this.m_currentCardIds[i] > 0)
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
						Card spawnedCardInstance = this.GetSpawnedCardInstance((CardType)this.m_currentCardIds[i]);
						Ability ability;
						if (spawnedCardInstance != null)
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
							ability = spawnedCardInstance.m_useAbility;
						}
						else
						{
							ability = null;
						}
						useAbility = ability;
					}
					this.SetupCardAbility(i, useAbility);
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
				this.UpdateCardBarUI();
			}
		}
	}

	public void OnQueuedAbilitiesChanged()
	{
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < 0xE; i++)
		{
			AbilityData.ActionType type = (AbilityData.ActionType)i;
			Ability abilityOfActionType = this.GetAbilityOfActionType(type);
			if (!flag)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.OnQueuedAbilitiesChanged()).MethodHandle;
				}
				if (abilityOfActionType != null)
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
					if (abilityOfActionType.GetAffectsMovement())
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
						flag = true;
					}
				}
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
				if (abilityOfActionType != null)
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
					if (abilityOfActionType.ShouldUpdateDrawnTargetersOnQueueChange())
					{
						flag2 = true;
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
		if (flag)
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
			if (!GameplayUtils.IsMinion(this))
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
				ActorMovement component = base.GetComponent<ActorMovement>();
				component.UpdateSquaresCanMoveTo();
			}
		}
		if (flag2 && this.m_actor.\u000E() != null)
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
			this.m_actor.\u000E().MarkForForceRedraw();
		}
	}

	private void OnRespawn()
	{
		for (int i = 0; i < this.m_abilities.Length; i++)
		{
			AbilityData.AbilityEntry abilityEntry = this.m_abilities[i];
			if (abilityEntry != null)
			{
				AbilityData.ActionType action = (AbilityData.ActionType)i;
				Ability ability = abilityEntry.ability;
				if (ability == null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.OnRespawn()).MethodHandle;
					}
				}
				else
				{
					if (AbilityUtils.AbilityHasTag(ability, AbilityTags.TriggerCooldownOnRespawn))
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
						this.TriggerCooldown(action);
					}
					if (AbilityUtils.AbilityHasTag(ability, AbilityTags.ClearCooldownOnRespawn))
					{
						this.ClearCooldown(action);
					}
				}
			}
		}
	}

	public void SetSelectedAbility(Ability selectedAbility)
	{
		ActorTurnSM actorTurnSM;
		if (this.m_actor == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.SetSelectedAbility(Ability)).MethodHandle;
			}
			actorTurnSM = null;
		}
		else
		{
			actorTurnSM = this.m_actor.\u000E();
		}
		ActorTurnSM actorTurnSM2 = actorTurnSM;
		bool flag;
		if (GameFlowData.Get().activeOwnedActorData == this.m_actor)
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
			flag = (this.m_actor != null);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		if (this.m_selectedAbility)
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
				this.m_selectedAbility.OnAbilityDeselect();
			}
		}
		this.m_selectedAbility = selectedAbility;
		this.Networkm_selectedActionForTargeting = this.GetActionTypeOfAbility(this.m_selectedAbility);
		if (this.m_selectedAbility)
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
			if (flag2)
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
				this.m_selectedAbility.OnAbilitySelect();
			}
		}
		if (actorTurnSM2 != null)
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
			actorTurnSM2.OnSelectedAbilityChanged(selectedAbility);
		}
		if (this.m_actor != null)
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
			this.m_actor.OnSelectedAbilityChanged(selectedAbility);
		}
		if (CameraManager.Get() != null)
		{
			CameraManager.Get().OnSelectedAbilityChanged(selectedAbility);
		}
		if (selectedAbility != null)
		{
			this.m_lastSelectedAbility = selectedAbility;
		}
		Board.\u000E().MarkForUpdateValidSquares(true);
	}

	public Ability GetSelectedAbility()
	{
		return this.m_selectedAbility;
	}

	public void ClearSelectedAbility()
	{
		this.SetSelectedAbility(null);
	}

	public void SelectAbilityFromActionType(AbilityData.ActionType actionType)
	{
		this.SetSelectedAbility(this.GetAbilityOfActionType(actionType));
	}

	public AbilityData.ActionType GetSelectedActionType()
	{
		return this.GetActionTypeOfAbility(this.m_selectedAbility);
	}

	public void SetLastSelectedAbility(Ability ability)
	{
		this.m_lastSelectedAbility = ability;
	}

	public Ability GetLastSelectedAbility()
	{
		return this.m_lastSelectedAbility;
	}

	public void ClearLastSelectedAbility()
	{
		this.m_lastSelectedAbility = null;
	}

	public AbilityData.ActionType GetActionType(string abilityName)
	{
		AbilityData.ActionType actionType = AbilityData.ActionType.INVALID_ACTION;
		if (abilityName != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetActionType(string)).MethodHandle;
			}
			for (int i = 0; i < 0xE; i++)
			{
				if (this.m_abilities[i] != null)
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
					if (this.m_abilities[i].ability != null)
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
						if (this.m_abilities[i].ability.m_abilityName == abilityName)
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
							actionType = (AbilityData.ActionType)i;
							IL_8F:
							if (actionType != AbilityData.ActionType.INVALID_ACTION)
							{
								return actionType;
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
							sbyte b = 0;
							while ((int)b < this.m_allChainAbilities.Count)
							{
								if (this.m_allChainAbilities[(int)b] != null)
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
									if (this.m_allChainAbilities[(int)b].m_abilityName == abilityName)
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
										sbyte b2 = b;
										b2 = checked((sbyte)(unchecked((int)b2) + 0xA));
										return (AbilityData.ActionType)b2;
									}
								}
								b = (sbyte)((int)b + 1);
							}
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								return actionType;
							}
						}
					}
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				goto IL_8F;
			}
		}
		return actionType;
	}

	public AbilityData.ActionType GetActionTypeOfAbilityOfType(Type abilityType)
	{
		Ability abilityOfType = this.GetAbilityOfType(abilityType);
		return this.GetActionTypeOfAbility(abilityOfType);
	}

	public AbilityData.ActionType GetActionTypeOfAbility(Ability ability)
	{
		AbilityData.ActionType actionType = AbilityData.ActionType.INVALID_ACTION;
		if (ability != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetActionTypeOfAbility(Ability)).MethodHandle;
			}
			int i = 0;
			while (i < 0xE)
			{
				if (this.m_abilities[i] != null && this.m_abilities[i].ability == ability)
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
					actionType = (AbilityData.ActionType)i;
					IL_67:
					if (actionType != AbilityData.ActionType.INVALID_ACTION)
					{
						return actionType;
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
					sbyte b = 0;
					while ((int)b < this.m_allChainAbilities.Count)
					{
						if (this.m_allChainAbilities[(int)b] != null && this.m_allChainAbilities[(int)b] == ability)
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
							sbyte b2 = b;
							b2 = checked((sbyte)(unchecked((int)b2) + 0xA));
							return (AbilityData.ActionType)b2;
						}
						b = (sbyte)((int)b + 1);
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						return actionType;
					}
				}
				else
				{
					i++;
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				goto IL_67;
			}
		}
		return actionType;
	}

	public AbilityData.ActionType GetParentAbilityActionType(Ability ability)
	{
		AbilityData.ActionType actionTypeOfAbility = this.GetActionTypeOfAbility(ability);
		if (AbilityData.IsChain(actionTypeOfAbility))
		{
			for (int i = 0; i < this.m_allChainAbilities.Count; i++)
			{
				if (this.m_allChainAbilities[i] == ability)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetParentAbilityActionType(Ability)).MethodHandle;
					}
					return this.m_allChainAbilityParentActionTypes[i];
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
		return actionTypeOfAbility;
	}

	public Ability GetAbilityOfActionType(AbilityData.ActionType type)
	{
		Ability result;
		if (AbilityData.IsChain(type))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetAbilityOfActionType(AbilityData.ActionType)).MethodHandle;
			}
			int num = type - AbilityData.ActionType.CHAIN_0;
			if (num >= 0)
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
				if (num < this.m_allChainAbilities.Count)
				{
					result = this.m_allChainAbilities[num];
					goto IL_59;
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
			result = null;
			IL_59:;
		}
		else
		{
			if (type >= AbilityData.ActionType.ABILITY_0)
			{
				if (type < (AbilityData.ActionType)this.m_abilities.Length)
				{
					return this.m_abilities[(int)type].ability;
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
			result = null;
		}
		return result;
	}

	public Ability GetAbilityOfType(Type abilityType)
	{
		foreach (AbilityData.AbilityEntry abilityEntry in this.m_abilities)
		{
			if (abilityEntry != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetAbilityOfType(Type)).MethodHandle;
				}
				if (abilityEntry.ability != null && abilityEntry.ability.GetType() == abilityType)
				{
					return abilityEntry.ability;
				}
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
		return null;
	}

	public T GetAbilityOfType<T>() where T : Ability
	{
		foreach (AbilityData.AbilityEntry abilityEntry in this.m_abilities)
		{
			if (abilityEntry != null && abilityEntry.ability != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetAbilityOfType()).MethodHandle;
				}
				if (abilityEntry.ability.GetType() == typeof(T))
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
					return abilityEntry.ability as T;
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
		return (T)((object)null);
	}

	public AbilityData.AbilityEntry GetAbilityEntryOfActionType(AbilityData.ActionType type)
	{
		if (type >= AbilityData.ActionType.ABILITY_0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetAbilityEntryOfActionType(AbilityData.ActionType)).MethodHandle;
			}
			if (type < (AbilityData.ActionType)this.m_abilities.Length)
			{
				return this.m_abilities[(int)type];
			}
		}
		return null;
	}

	public static CardType GetCardTypeByActionType(CharacterCardInfo cardInfo, AbilityData.ActionType actionType)
	{
		if (actionType == AbilityData.ActionType.CARD_0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetCardTypeByActionType(CharacterCardInfo, AbilityData.ActionType)).MethodHandle;
			}
			return cardInfo.PrepCard;
		}
		if (actionType == AbilityData.ActionType.CARD_1)
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
			return cardInfo.DashCard;
		}
		if (actionType == AbilityData.ActionType.CARD_2)
		{
			return cardInfo.CombatCard;
		}
		return CardType.None;
	}

	public List<Ability> GetCachedCardAbilities()
	{
		return this.m_cachedCardAbilities;
	}

	public bool IsAbilityAllowedByUnlockTurns(AbilityData.ActionType actionType)
	{
		int turnsTillUnlock = this.GetTurnsTillUnlock(actionType);
		return turnsTillUnlock <= 0;
	}

	public int GetTurnsTillUnlock(AbilityData.ActionType actionType)
	{
		if (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("NoCooldowns"))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetTurnsTillUnlock(AbilityData.ActionType)).MethodHandle;
			}
			return 0;
		}
		int b = 0;
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
			if (GameplayData.Get() != null)
			{
				int currentTurn = GameFlowData.Get().CurrentTurn;
				SpawnPointManager spawnPointManager = SpawnPointManager.Get();
				if (spawnPointManager != null)
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
					if (spawnPointManager.m_spawnInDuringMovement && GameplayData.Get().m_disableAbilitiesOnRespawn)
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
						if (this.m_actor.NextRespawnTurn == currentTurn)
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
							b = 1;
							goto IL_116;
						}
					}
				}
				if (AbilityData.IsCard(actionType))
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
					int turnCatalystsUnlock = GameplayData.Get().m_turnCatalystsUnlock;
					b = turnCatalystsUnlock - currentTurn;
				}
				else
				{
					int[] turnsAbilitiesUnlock = GameplayData.Get().m_turnsAbilitiesUnlock;
					if (actionType < (AbilityData.ActionType)turnsAbilitiesUnlock.Length)
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
						b = turnsAbilitiesUnlock[(int)actionType] - currentTurn;
					}
				}
			}
		}
		IL_116:
		return Mathf.Max(0, b);
	}

	public int GetCooldownRemaining(AbilityData.ActionType action)
	{
		if (AbilityData.IsChain(action))
		{
			return 0;
		}
		AbilityData.AbilityEntry abilityEntry = this.m_abilities[(int)action];
		return abilityEntry.GetCooldownRemaining();
	}

	public bool IsActionInCooldown(AbilityData.ActionType action)
	{
		if (AbilityData.IsChain(action))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.IsActionInCooldown(AbilityData.ActionType)).MethodHandle;
			}
			return false;
		}
		AbilityData.AbilityEntry abilityEntry = this.m_abilities[(int)action];
		return abilityEntry.GetCooldownRemaining() != 0;
	}

	public void TriggerCooldown(AbilityData.ActionType action)
	{
		if (!AbilityData.IsChain(action))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.TriggerCooldown(AbilityData.ActionType)).MethodHandle;
			}
			AbilityData.AbilityEntry abilityEntry = this.m_abilities[(int)action];
			if (abilityEntry.ability != null)
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
				int moddedCooldown = abilityEntry.ability.GetModdedCooldown();
				if (moddedCooldown > 0)
				{
					if (GameplayMutators.Get() != null)
					{
						int num = moddedCooldown + 1;
						int cooldownTimeAdjustment = GameplayMutators.GetCooldownTimeAdjustment();
						float cooldownMultiplier = GameplayMutators.GetCooldownMultiplier();
						int num2 = Mathf.RoundToInt((float)(num + cooldownTimeAdjustment) * cooldownMultiplier);
						num2 = Math.Max(num2, 0);
						this.m_cooldowns[abilityEntry.ability.m_abilityName] = num2;
					}
					else
					{
						this.m_cooldowns[abilityEntry.ability.m_abilityName] = moddedCooldown + 1;
					}
				}
				else if (abilityEntry.ability.m_cooldown == -1)
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
					this.m_cooldowns[abilityEntry.ability.m_abilityName] = -1;
				}
				this.SynchronizeCooldownsToSlots();
			}
		}
	}

	public void OverrideCooldown(AbilityData.ActionType action, int cooldownRemainingOverride)
	{
		if (!AbilityData.IsChain(action))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.OverrideCooldown(AbilityData.ActionType, int)).MethodHandle;
			}
			AbilityData.AbilityEntry abilityEntry = this.m_abilities[(int)action];
			if (abilityEntry.ability != null)
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
				this.m_cooldowns[abilityEntry.ability.m_abilityName] = cooldownRemainingOverride;
				this.SynchronizeCooldownsToSlots();
			}
		}
	}

	public void ApplyCooldownReduction(AbilityData.ActionType action, int cooldownReduction)
	{
		if (cooldownReduction > 0)
		{
			int num = this.GetCooldownRemaining(action);
			if (num > 0)
			{
				num -= cooldownReduction;
				num = Mathf.Max(0, num);
				this.OverrideCooldown(action, num);
			}
		}
	}

	public void ProgressCooldowns()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>(this.m_cooldowns);
		using (Dictionary<string, int>.Enumerator enumerator = dictionary.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, int> keyValuePair = enumerator.Current;
				string key = keyValuePair.Key;
				if (this.m_cooldowns[key] > 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.ProgressCooldowns()).MethodHandle;
					}
					int num = 1;
					if (GameplayMutators.Get() != null)
					{
						num += GameplayMutators.GetCooldownSpeedAdjustment();
						num = Mathf.Min(num, this.m_cooldowns[key]);
					}
					Dictionary<string, int> cooldowns;
					string key2;
					(cooldowns = this.m_cooldowns)[key2 = key] = cooldowns[key2] - num;
					if (this.m_cooldowns[key] == 0)
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
						this.m_cooldowns.Remove(key);
					}
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
		this.SynchronizeCooldownsToSlots();
	}

	public void ProgressCooldownsOfAbilities(List<Ability> abilities)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>(this.m_cooldowns);
		using (Dictionary<string, int>.Enumerator enumerator = dictionary.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, int> keyValuePair = enumerator.Current;
				string key = keyValuePair.Key;
				if (this.m_cooldowns[key] > 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.ProgressCooldownsOfAbilities(List<Ability>)).MethodHandle;
					}
					bool flag = false;
					using (List<Ability>.Enumerator enumerator2 = abilities.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Ability ability = enumerator2.Current;
							if (ability.m_abilityName == key)
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
								goto IL_B0;
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
					IL_B0:
					if (flag)
					{
						Dictionary<string, int> cooldowns;
						string key2;
						(cooldowns = this.m_cooldowns)[key2 = key] = cooldowns[key2] - 1;
						if (this.m_cooldowns[key] == 0)
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
							this.m_cooldowns.Remove(key);
						}
					}
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
		}
		this.SynchronizeCooldownsToSlots();
	}

	public void ProgressCharacterAbilityCooldowns()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>(this.m_cooldowns);
		using (Dictionary<string, int>.Enumerator enumerator = dictionary.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, int> keyValuePair = enumerator.Current;
				string key = keyValuePair.Key;
				AbilityData.ActionType actionType = this.GetActionType(key);
				if (actionType >= AbilityData.ActionType.ABILITY_0 && actionType <= AbilityData.ActionType.ABILITY_6)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.ProgressCharacterAbilityCooldowns()).MethodHandle;
					}
					if (this.m_cooldowns[key] > 0)
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
						Dictionary<string, int> cooldowns;
						string key2;
						(cooldowns = this.m_cooldowns)[key2 = key] = cooldowns[key2] - 1;
						if (this.m_cooldowns[key] == 0)
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
							this.m_cooldowns.Remove(key);
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
		this.SynchronizeCooldownsToSlots();
	}

	[Command]
	internal void CmdClearCooldowns()
	{
		if (!HydrogenConfig.Get().AllowDebugCommands)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.CmdClearCooldowns()).MethodHandle;
			}
			return;
		}
		this.ClearCooldowns();
	}

	public void ClearCooldowns()
	{
		this.m_cooldowns.Clear();
		this.SynchronizeCooldownsToSlots();
	}

	public void ClearCharacterAbilityCooldowns()
	{
		bool flag = false;
		for (int i = 0; i < 7; i++)
		{
			AbilityData.ActionType actionType = (AbilityData.ActionType)i;
			AbilityData.AbilityEntry abilityEntry = this.m_abilities[(int)actionType];
			if (abilityEntry.ability != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.ClearCharacterAbilityCooldowns()).MethodHandle;
				}
				string abilityName = abilityEntry.ability.m_abilityName;
				if (this.m_cooldowns.ContainsKey(abilityName))
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
					this.m_cooldowns.Remove(abilityName);
					flag = true;
				}
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
		if (flag)
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
			this.SynchronizeCooldownsToSlots();
		}
	}

	public void SetCooldown(AbilityData.ActionType action, int cooldown)
	{
		AbilityData.AbilityEntry abilityEntry = this.m_abilities[(int)action];
		if (abilityEntry.ability != null)
		{
			string abilityName = abilityEntry.ability.m_abilityName;
			if (this.m_cooldowns.ContainsKey(abilityName))
			{
				this.m_cooldowns[abilityName] = cooldown;
				this.SynchronizeCooldownsToSlots();
			}
		}
	}

	public void ClearCooldown(AbilityData.ActionType action)
	{
		AbilityData.AbilityEntry abilityEntry = this.m_abilities[(int)action];
		if (abilityEntry.ability != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.ClearCooldown(AbilityData.ActionType)).MethodHandle;
			}
			string abilityName = abilityEntry.ability.m_abilityName;
			if (this.m_cooldowns.ContainsKey(abilityName))
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
				this.m_cooldowns.Remove(abilityName);
				this.SynchronizeCooldownsToSlots();
			}
		}
	}

	public void PlaceInCooldownTillTurn(AbilityData.ActionType action, int turnNumber)
	{
		AbilityData.AbilityEntry abilityEntry = this.m_abilities[(int)action];
		if (abilityEntry.ability != null)
		{
			int num = turnNumber - GameFlowData.Get().CurrentTurn;
			if (num > 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.PlaceInCooldownTillTurn(AbilityData.ActionType, int)).MethodHandle;
				}
				this.m_cooldowns[abilityEntry.ability.m_abilityName] = num;
				this.SynchronizeCooldownsToSlots();
			}
		}
	}

	[Server]
	internal void SynchronizeCooldownsToSlots()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AbilityData::SynchronizeCooldownsToSlots()' called on client");
			return;
		}
		int i = 0;
		while (i < this.m_abilities.Length)
		{
			AbilityData.AbilityEntry abilityEntry = this.m_abilities[i];
			string text;
			if (abilityEntry.ability == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.SynchronizeCooldownsToSlots()).MethodHandle;
				}
				text = null;
			}
			else
			{
				text = abilityEntry.ability.m_abilityName;
			}
			string key = text;
			if (!(abilityEntry.ability != null))
			{
				goto IL_99;
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
			if (!this.m_cooldowns.ContainsKey(key))
			{
				goto IL_99;
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
			int num = this.m_cooldowns[key];
			IL_9B:
			if (abilityEntry.GetCooldownRemaining() != num)
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
				abilityEntry.SetCooldownRemaining(num);
				if (this.m_cooldownsSync[i] != num)
				{
					this.m_cooldownsSync[i] = num;
				}
			}
			i++;
			continue;
			IL_99:
			num = 0;
			goto IL_9B;
		}
	}

	[Server]
	private void InitializeStockCounts()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.InitializeStockCounts()).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void AbilityData::InitializeStockCounts()' called on client");
			return;
		}
		for (int i = 0; i < 7; i++)
		{
			AbilityData.ActionType actionType = (AbilityData.ActionType)i;
			AbilityData.AbilityEntry abilityEntry = this.m_abilities[(int)actionType];
			if (abilityEntry.ability != null)
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
				if (abilityEntry.ability.GetModdedMaxStocks() > 0)
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
					if (abilityEntry.ability.m_initialStockAmount >= 0)
					{
						int desiredAmount = Mathf.Min(abilityEntry.ability.GetModdedMaxStocks(), abilityEntry.ability.m_initialStockAmount);
						this.OverrideStockRemaining(actionType, desiredAmount);
					}
				}
			}
		}
	}

	public int GetMaxStocksCount(AbilityData.ActionType actionType)
	{
		AbilityData.AbilityEntry abilityEntry = this.m_abilities[(int)actionType];
		if (abilityEntry.ability != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetMaxStocksCount(AbilityData.ActionType)).MethodHandle;
			}
			return abilityEntry.ability.GetModdedMaxStocks();
		}
		return 0;
	}

	public int GetConsumedStocksCount(AbilityData.ActionType actionType)
	{
		return this.m_consumedStockCount[(int)actionType];
	}

	public int GetStocksRemaining(AbilityData.ActionType actionType)
	{
		return Mathf.Max(0, this.GetMaxStocksCount(actionType) - this.GetConsumedStocksCount(actionType));
	}

	public int GetStockRefreshCountdown(AbilityData.ActionType actionType)
	{
		return this.m_stockRefreshCountdowns[(int)actionType];
	}

	public bool ActionHasEnoughStockToTrigger(AbilityData.ActionType action)
	{
		if (!AbilityData.IsChain(action))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.ActionHasEnoughStockToTrigger(AbilityData.ActionType)).MethodHandle;
			}
			if (DebugParameters.Get() != null)
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
				if (DebugParameters.Get().GetParameterAsBool("NoCooldowns"))
				{
					return true;
				}
			}
			AbilityData.AbilityEntry abilityEntry = this.m_abilities[(int)action];
			bool result;
			if (abilityEntry.ability.m_abilityManagedStockCount)
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
			else
			{
				int moddedMaxStocks = abilityEntry.ability.GetModdedMaxStocks();
				result = (moddedMaxStocks <= 0 || this.m_consumedStockCount[(int)action] < moddedMaxStocks);
			}
			return result;
		}
		return true;
	}

	public void ConsumeStock(AbilityData.ActionType action)
	{
		if (!AbilityData.IsChain(action))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.ConsumeStock(AbilityData.ActionType)).MethodHandle;
			}
			AbilityData.AbilityEntry abilityEntry = this.m_abilities[(int)action];
			if (abilityEntry.ability != null)
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
				int moddedMaxStocks = abilityEntry.ability.GetModdedMaxStocks();
				if (moddedMaxStocks > 0)
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
					if (this.m_consumedStockCount[(int)action] < moddedMaxStocks)
					{
						if (this.m_consumedStockCount[(int)action] == 0)
						{
							int moddedStockRefreshDuration = abilityEntry.ability.GetModdedStockRefreshDuration();
							int num = moddedStockRefreshDuration + 1;
							if (this.m_stockRefreshCountdowns[(int)action] != num)
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
								this.m_stockRefreshCountdowns[(int)action] = num;
							}
						}
						int num2 = Mathf.Clamp(this.m_consumedStockCount[(int)action] + abilityEntry.ability.m_stockConsumedOnCast, 0, moddedMaxStocks);
						if (this.m_consumedStockCount[(int)action] != num2)
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
							this.m_consumedStockCount[(int)action] = num2;
						}
					}
				}
			}
		}
	}

	[Command]
	internal void CmdRefillStocks()
	{
		if (!HydrogenConfig.Get().AllowDebugCommands)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.CmdRefillStocks()).MethodHandle;
			}
			return;
		}
		this.RefillStocks();
	}

	public void RefillStocks()
	{
		for (int i = 0; i < this.m_consumedStockCount.Count; i++)
		{
			if (this.m_consumedStockCount[i] != 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.RefillStocks()).MethodHandle;
				}
				this.m_consumedStockCount[i] = 0;
			}
			if (this.m_stockRefreshCountdowns[i] != 0)
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
				this.m_stockRefreshCountdowns[i] = 0;
			}
		}
	}

	public void ProgressStockRefreshTimes()
	{
		for (int i = 0; i < 0xE; i++)
		{
			this.ProgressStockRefreshTimeForAction((AbilityData.ActionType)i, 1);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.ProgressStockRefreshTimes()).MethodHandle;
		}
	}

	public void ProgressStockRefreshTimeForAction(AbilityData.ActionType action, int advanceAmount)
	{
		if (action < (AbilityData.ActionType)this.m_stockRefreshCountdowns.Count)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.ProgressStockRefreshTimeForAction(AbilityData.ActionType, int)).MethodHandle;
			}
			if (advanceAmount > 0)
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
				AbilityData.AbilityEntry abilityEntry = this.m_abilities[(int)action];
				bool flag = this.m_stockRefreshCountdowns[(int)action] > 0 || this.m_consumedStockCount[(int)action] > 0;
				if (flag)
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
					if (abilityEntry != null)
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
						if (abilityEntry.ability != null && abilityEntry.ability.GetModdedStockRefreshDuration() > 0)
						{
							int moddedStockRefreshDuration = abilityEntry.ability.GetModdedStockRefreshDuration();
							int num = advanceAmount / moddedStockRefreshDuration;
							int num2 = advanceAmount % moddedStockRefreshDuration;
							int num3 = Mathf.Max(0, this.m_consumedStockCount[(int)action] - num);
							if (this.m_consumedStockCount[(int)action] != num3)
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
								this.m_consumedStockCount[(int)action] = num3;
							}
							if (this.m_stockRefreshCountdowns[(int)action] >= num2)
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
								if (num2 != 0)
								{
									SyncListInt syncListInt;
									(syncListInt = this.m_stockRefreshCountdowns)[(int)action] = syncListInt[(int)action] - num2;
								}
								if (this.m_stockRefreshCountdowns[(int)action] <= 0)
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
									if (this.m_consumedStockCount[(int)action] > 0)
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
										if (abilityEntry.ability.RefillAllStockOnRefresh())
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
											this.m_consumedStockCount[(int)action] = 0;
										}
										else
										{
											SyncListInt syncListInt;
											(syncListInt = this.m_consumedStockCount)[(int)action] = syncListInt[(int)action] - 1;
										}
										if (this.m_consumedStockCount[(int)action] > 0)
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
											int num4 = moddedStockRefreshDuration;
											if (this.m_stockRefreshCountdowns[(int)action] != num4)
											{
												this.m_stockRefreshCountdowns[(int)action] = num4;
											}
										}
										else if (this.m_stockRefreshCountdowns[(int)action] != 0)
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
											this.m_stockRefreshCountdowns[(int)action] = 0;
										}
									}
								}
							}
							else
							{
								int num5 = num2 - this.m_stockRefreshCountdowns[(int)action];
								int num6 = moddedStockRefreshDuration - num5;
								if (this.m_stockRefreshCountdowns[(int)action] != num6)
								{
									this.m_stockRefreshCountdowns[(int)action] = num6;
								}
								if (this.m_consumedStockCount[(int)action] > 0)
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
									if (abilityEntry.ability.RefillAllStockOnRefresh())
									{
										this.m_consumedStockCount[(int)action] = 0;
									}
									else
									{
										SyncListInt syncListInt;
										(syncListInt = this.m_consumedStockCount)[(int)action] = syncListInt[(int)action] - 1;
									}
								}
							}
						}
					}
				}
			}
		}
	}

	public void OverrideStockRemaining(AbilityData.ActionType action, int desiredAmount)
	{
		if (!AbilityData.IsChain(action))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.OverrideStockRemaining(AbilityData.ActionType, int)).MethodHandle;
			}
			AbilityData.AbilityEntry abilityEntry = this.m_abilities[(int)action];
			if (abilityEntry.ability != null)
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
				int value = Mathf.Max(0, abilityEntry.ability.GetModdedMaxStocks() - desiredAmount);
				this.m_consumedStockCount[(int)action] = value;
				if (this.m_consumedStockCount[(int)action] == 0)
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
					if (this.m_stockRefreshCountdowns[(int)action] != 0)
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
						this.m_stockRefreshCountdowns[(int)action] = 0;
					}
				}
			}
		}
	}

	public void OverrideStockRefreshCountdown(AbilityData.ActionType action, int desiredCountdown)
	{
		if (!AbilityData.IsChain(action))
		{
			AbilityData.AbilityEntry abilityEntry = this.m_abilities[(int)action];
			if (abilityEntry.ability != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.OverrideStockRefreshCountdown(AbilityData.ActionType, int)).MethodHandle;
				}
				int num = Mathf.Clamp(desiredCountdown, 0, abilityEntry.ability.GetModdedStockRefreshDuration());
				if (this.m_stockRefreshCountdowns[(int)action] != num)
				{
					this.m_stockRefreshCountdowns[(int)action] = num;
				}
			}
		}
	}

	public int GetStockRefreshDurationForAbility(AbilityData.ActionType action)
	{
		if (!AbilityData.IsChain(action))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetStockRefreshDurationForAbility(AbilityData.ActionType)).MethodHandle;
			}
			AbilityData.AbilityEntry abilityEntry = this.m_abilities[(int)action];
			if (abilityEntry.ability != null)
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
				return abilityEntry.ability.GetModdedStockRefreshDuration();
			}
		}
		return 0;
	}

	public bool IsAbilityTargetInRange(Ability ability, AbilityTarget target, int targetIndex, float calculatedMinRangeInSquares = -1f, float calculatedMaxRangeInSquares = -1f)
	{
		bool flag = false;
		ActorData actor = this.m_actor;
		BoardSquare src = actor.\u0012();
		float num = calculatedMaxRangeInSquares;
		float num2 = calculatedMinRangeInSquares;
		if (num < 0f)
		{
			num = AbilityUtils.GetCurrentRangeInSquares(ability, actor, targetIndex);
		}
		if (num2 < 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.IsAbilityTargetInRange(Ability, AbilityTarget, int, float, float)).MethodHandle;
			}
			num2 = AbilityUtils.GetCurrentMinRangeInSquares(ability, actor, targetIndex);
		}
		Ability.TargetingParadigm targetingParadigm = ability.GetTargetingParadigm(targetIndex);
		if (targetingParadigm == Ability.TargetingParadigm.BoardSquare)
		{
			BoardSquare dest = Board.\u000E().\u000E(target.GridPos);
			flag |= this.IsTargetSquareInRangeOfAbilityFromSquare(dest, src, num, num2);
		}
		else if (targetingParadigm == Ability.TargetingParadigm.Position)
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
			Vector3 b = actor.\u0015();
			float num3 = num * Board.\u000E().squareSize;
			float num4 = num2 * Board.\u000E().squareSize;
			if (GameplayData.Get().m_abilityRangeType == GameplayData.AbilityRangeType.WorldDistToFreePos)
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
				Vector3 vector = target.FreePos - b;
				vector.y = 0f;
				float sqrMagnitude = vector.sqrMagnitude;
				bool flag2;
				if (sqrMagnitude <= num3 * num3)
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
					flag2 = (sqrMagnitude >= num4 * num4);
				}
				else
				{
					flag2 = false;
				}
				flag = flag2;
			}
			else
			{
				BoardSquare dest2 = Board.\u000E().\u000E(target.GridPos);
				flag |= this.IsTargetSquareInRangeOfAbilityFromSquare(dest2, src, num, num2);
			}
		}
		else if (targetingParadigm == Ability.TargetingParadigm.Direction)
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
			flag = true;
		}
		else
		{
			Log.Error("Checking range for ability " + ability.m_abilityName + ", but its targeting paradigm is invalid.", new object[0]);
			flag = false;
		}
		return flag;
	}

	public bool IsTargetSquareInRangeOfAbilityFromSquare(BoardSquare dest, BoardSquare src, float rangeInSquares, float minRangeInSquares)
	{
		bool result = true;
		if (src)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.IsTargetSquareInRangeOfAbilityFromSquare(BoardSquare, BoardSquare, float, float)).MethodHandle;
			}
			if (dest)
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
				float num;
				if (GameplayData.Get().m_abilityRangeType == GameplayData.AbilityRangeType.WorldDistToFreePos)
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
					num = src.HorizontalDistanceInSquaresTo(dest);
				}
				else
				{
					num = src.HorizontalDistanceOnBoardTo(dest);
				}
				bool flag;
				if (rangeInSquares >= 0f)
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
					flag = (num <= rangeInSquares);
				}
				else
				{
					flag = true;
				}
				bool flag2 = flag;
				bool flag3 = num >= minRangeInSquares;
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
					if (flag3)
					{
						return result;
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
				result = false;
			}
		}
		return result;
	}

	public bool HasLineOfSightToTarget(Ability specificAbility, AbilityTarget target, int targetIndex)
	{
		bool result = false;
		ActorData actor = this.m_actor;
		Ability.TargetingParadigm targetingParadigm = specificAbility.GetTargetingParadigm(targetIndex);
		if (actor.\u0012() == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.HasLineOfSightToTarget(Ability, AbilityTarget, int)).MethodHandle;
			}
			result = false;
		}
		else
		{
			if (targetingParadigm != Ability.TargetingParadigm.BoardSquare)
			{
				if (targetingParadigm == Ability.TargetingParadigm.Position)
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
				}
				else
				{
					if (targetingParadigm == Ability.TargetingParadigm.Direction)
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
					return result;
				}
			}
			BoardSquare value = Board.\u000E().\u000E(target.GridPos);
			ReadOnlyCollection<BoardSquare> lineOfSightVisibleExceptionSquares = actor.LineOfSightVisibleExceptionSquares;
			if (lineOfSightVisibleExceptionSquares.Contains(value))
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
			else if (actor.\u0012().\u0013(target.GridPos.x, target.GridPos.y))
			{
				result = true;
			}
		}
		return result;
	}

	public bool HasLineOfSightToActor(ActorData target, bool ignoreExceptions = false)
	{
		ActorData actor = this.m_actor;
		if (!(target == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.HasLineOfSightToActor(ActorData, bool)).MethodHandle;
			}
			if (!(actor == null))
			{
				if (!ignoreExceptions)
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
					if (actor.\u000E(target))
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
				BoardSquare boardSquare = target.\u0012();
				BoardSquare boardSquare2 = actor.\u0012();
				return boardSquare2.\u0013(boardSquare.x, boardSquare.y);
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
		return false;
	}

	public bool ValidateAbilityOnTarget(Ability ability, AbilityTarget target, int targetIndex, float calculatedMinRangeInSquares = -1f, float calculatedMaxRangeInSquares = -1f)
	{
		bool result = false;
		ActorTurnSM actorTurnSM = this.m_actor.\u000E();
		if (actorTurnSM)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.ValidateAbilityOnTarget(Ability, AbilityTarget, int, float, float)).MethodHandle;
			}
			List<AbilityTarget> abilityTargets = actorTurnSM.GetAbilityTargets();
			if (abilityTargets != null)
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
				bool flag;
				if (targetIndex <= abilityTargets.Count)
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
					flag = this.ValidateAbilityOnTarget(ability, target, targetIndex, abilityTargets, calculatedMinRangeInSquares, calculatedMaxRangeInSquares);
				}
				else
				{
					flag = false;
				}
				result = flag;
			}
		}
		return result;
	}

	public bool ValidateAbilityOnTarget(Ability ability, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets, float calculatedMinRangeInSquares = -1f, float calculatedMaxRangeInSquares = -1f)
	{
		if (target == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.ValidateAbilityOnTarget(Ability, AbilityTarget, int, List<AbilityTarget>, float, float)).MethodHandle;
			}
			return false;
		}
		ActorData actor = this.m_actor;
		bool flag = true;
		Ability.TargetingParadigm targetingParadigm = ability.GetTargetingParadigm(targetIndex);
		if (targetingParadigm != Ability.TargetingParadigm.BoardSquare)
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
			if (targetingParadigm != Ability.TargetingParadigm.Position)
			{
				goto IL_D8;
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
		BoardSquare boardSquare = Board.\u000E().\u000E(target.GridPos);
		bool flag2;
		if (boardSquare != null)
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
			flag2 = (ability.AllowInvalidSquareForSquareBasedTarget() || Board.\u000E().\u000E(target.GridPos).\u0016());
		}
		else
		{
			flag2 = false;
		}
		flag = flag2;
		if (!ability.IsSimpleAction())
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
			if (flag && BarrierManager.Get().IsPositionTargetingBlocked(actor, boardSquare))
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
				flag = false;
			}
			if (!flag)
			{
				return false;
			}
		}
		IL_D8:
		bool flag3;
		if (!ability.IsSimpleAction())
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
			flag3 = this.IsAbilityTargetInRange(ability, target, targetIndex, calculatedMinRangeInSquares, calculatedMaxRangeInSquares);
		}
		else
		{
			flag3 = true;
		}
		bool flag4 = flag3;
		if (!flag4)
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
		bool flag5 = true;
		if (AbilityUtils.AbilityHasTag(ability, AbilityTags.ValidOnlyWhereInCoverToCaster))
		{
			BoardSquare targetSquare = Board.\u000E().\u000E(target.GridPos);
			flag5 = ActorCover.IsInCoverWrt(actor.\u0016(), targetSquare, null, null, null);
		}
		else if (AbilityUtils.AbilityHasTag(ability, AbilityTags.ValidOnlyWhereInCover))
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
			BoardSquare square = Board.\u000E().\u000E(target.GridPos);
			bool[] array;
			flag5 = ActorCover.CalcCoverLevelGeoOnly(out array, square);
		}
		bool flag6 = true;
		if (ability.GetCheckLoS(targetIndex) && !ability.IsSimpleAction())
		{
			if (flag4)
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
				flag6 = this.HasLineOfSightToTarget(ability, target, targetIndex);
			}
			else
			{
				flag6 = false;
			}
		}
		bool flag7;
		if (flag4)
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
			if (flag && flag5)
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
				flag7 = flag6;
				goto IL_1DE;
			}
		}
		flag7 = false;
		IL_1DE:
		bool flag8 = flag7;
		bool result;
		if (flag8 && this.ValidateAbilityIsCastableDisregardingMovement(ability))
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
			result = ability.CustomTargetValidation(actor, target, targetIndex, currentTargets);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool ValidateActionRequest(AbilityData.ActionType actionType, List<AbilityTarget> targets)
	{
		bool result = true;
		Ability abilityOfActionType = this.GetAbilityOfActionType(actionType);
		if (!this.ValidateActionIsRequestable(actionType))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.ValidateActionRequest(AbilityData.ActionType, List<AbilityTarget>)).MethodHandle;
			}
			result = false;
		}
		else
		{
			for (int i = 0; i < targets.Count; i++)
			{
				AbilityTarget target = targets[i];
				if (!this.ValidateAbilityOnTarget(abilityOfActionType, target, i, targets, -1f, -1f))
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
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return result;
	}

	public bool ValidateAbilityIsCastable(Ability ability)
	{
		if (ability != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.ValidateAbilityIsCastable(Ability)).MethodHandle;
			}
			bool flag = this.ValidateAbilityIsCastableDisregardingMovement(ability);
			bool flag2 = ability.GetMovementAdjustment() != Ability.MovementAdjustment.NoMovement || !this.m_actor.HasQueuedMovement();
			bool flag3;
			if (!this.m_actor.QueuedMovementAllowsAbility)
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
				flag3 = !ability.GetAffectsMovement();
			}
			else
			{
				flag3 = true;
			}
			bool flag4 = flag3;
			bool flag5 = ability.RunPriority != AbilityPriority.Evasion || !this.HasQueuedAbilityInPhase(AbilityPriority.Evasion);
			bool flag6;
			if (AbilityData.IsCard(this.GetActionTypeOfAbility(ability)))
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
				flag6 = !this.HasQueuedCardAbility();
			}
			else
			{
				flag6 = true;
			}
			bool result = flag6;
			if (flag)
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
					if (flag4)
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
						if (flag5)
						{
							return result;
						}
					}
				}
			}
			return false;
		}
		Log.Error("Actor " + this.m_actor.DisplayName + " calling ValidateAbilityIsCastable on a null ability.", new object[0]);
		return false;
	}

	public bool ValidateActionIsRequestable(AbilityData.ActionType abilityAction)
	{
		Ability abilityOfActionType = this.GetAbilityOfActionType(abilityAction);
		if (abilityOfActionType != null)
		{
			bool flag = this.ValidateActionIsRequestableDisregardingQueuedActions(abilityAction);
			bool flag2 = !this.HasQueuedAction(abilityAction);
			bool flag3;
			if (!abilityOfActionType.IsFreeAction())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.ValidateActionIsRequestable(AbilityData.ActionType)).MethodHandle;
				}
				flag3 = (this.GetActionCostOfQueuedAbilities(abilityAction) == 0);
			}
			else
			{
				flag3 = true;
			}
			bool flag4 = flag3;
			bool flag5;
			if (abilityOfActionType.GetMovementAdjustment() == Ability.MovementAdjustment.NoMovement)
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
				flag5 = !this.m_actor.HasQueuedMovement();
			}
			else
			{
				flag5 = true;
			}
			bool flag6 = flag5;
			bool flag7 = this.m_actor.QueuedMovementAllowsAbility || !abilityOfActionType.GetAffectsMovement();
			bool flag8;
			if (abilityOfActionType.RunPriority == AbilityPriority.Evasion)
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
				flag8 = !this.HasQueuedAbilityInPhase(AbilityPriority.Evasion);
			}
			else
			{
				flag8 = true;
			}
			bool flag9 = flag8;
			bool result = !AbilityData.IsCard(abilityAction) || !this.HasQueuedCardAbility();
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
					if (flag4)
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
						if (flag6)
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
							if (flag7)
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
								if (flag9)
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
									return result;
								}
							}
						}
					}
				}
			}
			return false;
		}
		Log.Error("Actor " + this.m_actor.DisplayName + " calling ValidateActionIsRequestable on a null ability.", new object[0]);
		return false;
	}

	public bool ValidateAbilityIsCastableDisregardingMovement(Ability ability)
	{
		ActorData actor = this.m_actor;
		if (ability != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.ValidateAbilityIsCastableDisregardingMovement(Ability)).MethodHandle;
			}
			AbilityData.ActionType actionTypeOfAbility = this.GetActionTypeOfAbility(ability);
			bool flag = actionTypeOfAbility != AbilityData.ActionType.INVALID_ACTION;
			bool flag2 = !actor.\u000E();
			bool flag3 = actor.TechPoints >= ability.GetModdedCost();
			bool flag4;
			if (AbilityUtils.AbilityHasTag(ability, AbilityTags.ValidOnlyWhenOutOfCombat))
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
				flag4 = actor.OutOfCombat;
			}
			else
			{
				flag4 = true;
			}
			bool flag5 = flag4;
			bool flag6 = !AbilityUtils.AbilityHasTag(ability, AbilityTags.ValidOnlyWhenInCombat) || !actor.OutOfCombat;
			bool flag7 = !actor.\u000E().IsActionSilenced(actionTypeOfAbility, false);
			bool flag8;
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
					if (flag3 && flag5)
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
						if (flag6)
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
							flag8 = flag7;
							goto IL_EB;
						}
					}
				}
			}
			flag8 = false;
			IL_EB:
			bool flag9 = flag8;
			bool result;
			if (flag9)
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
				result = ability.CustomCanCastValidation(actor);
			}
			else
			{
				result = false;
			}
			return result;
		}
		Log.Error("Actor " + actor.DisplayName + " calling ValidateAbilityIsCastableDisregardingMovement on a null ability.", new object[0]);
		return false;
	}

	public bool ValidateActionIsRequestableDisregardingQueuedActions(AbilityData.ActionType abilityAction)
	{
		Ability abilityOfActionType = this.GetAbilityOfActionType(abilityAction);
		if (abilityOfActionType != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.ValidateActionIsRequestableDisregardingQueuedActions(AbilityData.ActionType)).MethodHandle;
			}
			ActorData actor = this.m_actor;
			bool flag;
			if (this.IsActionInCooldown(abilityAction))
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
				flag = (abilityOfActionType.GetModdedMaxStocks() > 0);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (!flag2)
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
					if (AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.IgnoreCooldownIfFullEnergy))
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
						flag2 = (actor.TechPoints + actor.ReservedTechPoints >= actor.\u0016());
					}
				}
			}
			bool flag3 = this.ActionHasEnoughStockToTrigger(abilityAction);
			bool flag4 = this.IsAbilityAllowedByUnlockTurns(abilityAction);
			bool flag5 = this.ValidateAbilityIsCastableDisregardingMovement(abilityOfActionType);
			bool result = SinglePlayerManager.IsActionAllowed(actor, abilityAction);
			if (flag2 && flag3)
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
				if (flag4 && flag5)
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
					return result;
				}
			}
			return false;
		}
		Log.Error("Actor " + this.m_actor.DisplayName + " calling ValidateActionIsRequestableDisregardingQueuedActions on a null ability.", new object[0]);
		return false;
	}

	public BoardSquare GetAutoSelectTarget()
	{
		BoardSquare result = null;
		if (this.m_selectedAbility)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetAutoSelectTarget()).MethodHandle;
			}
			if (this.m_selectedAbility.IsAutoSelect())
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
				result = this.m_actor.\u0012();
			}
		}
		return result;
	}

	public bool HasQueuedAbilities()
	{
		bool result = false;
		ActorTeamSensitiveData teamSensitiveData_authority = this.m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.HasQueuedAbilities()).MethodHandle;
			}
			for (int i = 0; i < 0xE; i++)
			{
				if (teamSensitiveData_authority.HasQueuedAction(i))
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
					break;
				}
			}
		}
		return result;
	}

	public bool HasQueuedCardAbility()
	{
		bool result = false;
		ActorTeamSensitiveData teamSensitiveData_authority = this.m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.HasQueuedCardAbility()).MethodHandle;
			}
			for (int i = 0; i < 0xE; i++)
			{
				if (teamSensitiveData_authority.HasQueuedAction(i))
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
					if (AbilityData.IsCard((AbilityData.ActionType)i))
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
		return result;
	}

	public int GetNumQueuedAbilities()
	{
		int num = 0;
		ActorTeamSensitiveData teamSensitiveData_authority = this.m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetNumQueuedAbilities()).MethodHandle;
			}
			for (int i = 0; i < 0xE; i++)
			{
				if (teamSensitiveData_authority.HasQueuedAction(i))
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
					num++;
				}
			}
		}
		return num;
	}

	public Ability.MovementAdjustment GetQueuedAbilitiesMovementAdjustType()
	{
		Ability.MovementAdjustment movementAdjustment = Ability.MovementAdjustment.FullMovement;
		ActorTeamSensitiveData teamSensitiveData_authority = this.m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetQueuedAbilitiesMovementAdjustType()).MethodHandle;
			}
			for (int i = 0; i < 0xE; i++)
			{
				AbilityData.ActionType actionType = (AbilityData.ActionType)i;
				if (teamSensitiveData_authority.HasQueuedAction(actionType))
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
					Ability abilityOfActionType = this.GetAbilityOfActionType(actionType);
					if (abilityOfActionType != null)
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
						if (abilityOfActionType.GetMovementAdjustment() > movementAdjustment)
						{
							movementAdjustment = abilityOfActionType.GetMovementAdjustment();
						}
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
		SpawnPointManager spawnPointManager = SpawnPointManager.Get();
		if (spawnPointManager != null)
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
			if (spawnPointManager.m_spawnInDuringMovement)
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
				if (this.m_actor.NextRespawnTurn == GameFlowData.Get().CurrentTurn)
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
					if (GameplayData.Get().m_movementAllowedOnRespawn < movementAdjustment)
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
						movementAdjustment = GameplayData.Get().m_movementAllowedOnRespawn;
					}
				}
			}
		}
		return movementAdjustment;
	}

	public float GetQueuedAbilitiesMovementAdjust()
	{
		float result = 0f;
		Ability.MovementAdjustment queuedAbilitiesMovementAdjustType = this.GetQueuedAbilitiesMovementAdjustType();
		if (queuedAbilitiesMovementAdjustType == Ability.MovementAdjustment.ReducedMovement)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetQueuedAbilitiesMovementAdjust()).MethodHandle;
			}
			result = -1f * this.m_actor.\u000E();
		}
		return result;
	}

	public List<StatusType> GetQueuedAbilitiesOnRequestStatuses()
	{
		List<StatusType> list = new List<StatusType>();
		ActorTeamSensitiveData teamSensitiveData_authority = this.m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
		{
			for (int i = 0; i < 0xE; i++)
			{
				AbilityData.ActionType actionType = (AbilityData.ActionType)i;
				if (teamSensitiveData_authority.HasQueuedAction(actionType))
				{
					Ability abilityOfActionType = this.GetAbilityOfActionType(actionType);
					if (abilityOfActionType != null)
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetQueuedAbilitiesOnRequestStatuses()).MethodHandle;
						}
						list.AddRange(abilityOfActionType.GetStatusToApplyWhenRequested());
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
		return list;
	}

	public bool HasPendingStatusFromQueuedAbilities(StatusType status)
	{
		ActorTeamSensitiveData teamSensitiveData_authority = this.m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.HasPendingStatusFromQueuedAbilities(StatusType)).MethodHandle;
			}
			for (int i = 0; i < 0xE; i++)
			{
				AbilityData.ActionType actionType = (AbilityData.ActionType)i;
				if (teamSensitiveData_authority.HasQueuedAction(actionType))
				{
					Ability abilityOfActionType = this.GetAbilityOfActionType(actionType);
					if (abilityOfActionType != null && abilityOfActionType.GetStatusToApplyWhenRequested().Contains(status))
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
			return false;
		}
		return false;
	}

	public bool GetQueuedAbilitiesAllowSprinting()
	{
		bool result = true;
		ActorTeamSensitiveData teamSensitiveData_authority = this.m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetQueuedAbilitiesAllowSprinting()).MethodHandle;
			}
			for (int i = 0; i < 0xE; i++)
			{
				AbilityData.ActionType actionType = (AbilityData.ActionType)i;
				if (teamSensitiveData_authority.HasQueuedAction(actionType))
				{
					Ability abilityOfActionType = this.GetAbilityOfActionType(actionType);
					if (abilityOfActionType != null)
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
						if (abilityOfActionType.GetAffectsMovement())
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
							break;
						}
					}
				}
			}
		}
		return result;
	}

	public bool GetQueuedAbilitiesAllowMovement()
	{
		bool result = true;
		ActorTeamSensitiveData teamSensitiveData_authority = this.m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetQueuedAbilitiesAllowMovement()).MethodHandle;
			}
			for (int i = 0; i < 0xE; i++)
			{
				AbilityData.ActionType actionType = (AbilityData.ActionType)i;
				if (teamSensitiveData_authority.HasQueuedAction(actionType))
				{
					Ability abilityOfActionType = this.GetAbilityOfActionType(actionType);
					if (abilityOfActionType != null)
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
						if (abilityOfActionType.GetPreventsMovement())
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
					}
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
		}
		return result;
	}

	public int GetActionCostOfQueuedAbilities(AbilityData.ActionType actionToSkip = AbilityData.ActionType.INVALID_ACTION)
	{
		int num = 0;
		ActorTeamSensitiveData teamSensitiveData_authority = this.m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetActionCostOfQueuedAbilities(AbilityData.ActionType)).MethodHandle;
			}
			for (int i = 0; i < 0xE; i++)
			{
				AbilityData.ActionType actionType = (AbilityData.ActionType)i;
				if (teamSensitiveData_authority.HasQueuedAction(actionType))
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
					if (actionType == actionToSkip)
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
					}
					else
					{
						Ability abilityOfActionType = this.GetAbilityOfActionType(actionType);
						if (abilityOfActionType != null)
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
							if (!abilityOfActionType.IsFreeAction())
							{
								num++;
							}
						}
					}
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
		return num;
	}

	private Card GetSpawnedCardInstance(CardType cardType)
	{
		if (this.m_cardTypeToCardInstance.ContainsKey(cardType))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetSpawnedCardInstance(CardType)).MethodHandle;
			}
			return this.m_cardTypeToCardInstance[cardType];
		}
		GameObject cardPrefab = CardManagerData.Get().GetCardPrefab(cardType);
		if (cardPrefab != null)
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
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(cardPrefab);
			Card component = gameObject.GetComponent<Card>();
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
				if (component.m_useAbility != null)
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
					gameObject.transform.parent = base.gameObject.transform;
					this.m_cardTypeToCardInstance[cardType] = component;
					component.m_useAbility.OverrideActorDataIndex(this.m_actor.ActorIndex);
					return component;
				}
			}
			Log.Error("Card prefab " + cardPrefab.name + " does not have Card component", new object[0]);
			UnityEngine.Object.Destroy(gameObject);
			return component;
		}
		return null;
	}

	public void SetupCardAbility(int cardSlotIndex, Ability useAbility)
	{
		int num = 7 + cardSlotIndex;
		KeyPreference keyPreference = KeyPreference.Card1;
		if (cardSlotIndex == 1)
		{
			keyPreference = KeyPreference.Card2;
		}
		else if (cardSlotIndex == 2)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.SetupCardAbility(int, Ability)).MethodHandle;
			}
			keyPreference = KeyPreference.Card3;
		}
		this.m_abilities[num].Setup(useAbility, keyPreference);
		if (cardSlotIndex < this.m_cachedCardAbilities.Count)
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
			this.m_cachedCardAbilities[cardSlotIndex] = useAbility;
		}
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
			this.SynchronizeCooldownsToSlots();
		}
	}

	public void SpawnAndSetupCardsOnReconnect()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.SpawnAndSetupCardsOnReconnect()).MethodHandle;
			}
			for (int i = 0; i < this.m_currentCardIds.Count; i++)
			{
				Card spawnedCardInstance = this.GetSpawnedCardInstance((CardType)this.m_currentCardIds[i]);
				int cardSlotIndex = i;
				Ability useAbility;
				if (spawnedCardInstance != null)
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
					useAbility = spawnedCardInstance.m_useAbility;
				}
				else
				{
					useAbility = null;
				}
				this.SetupCardAbility(cardSlotIndex, useAbility);
			}
			this.UpdateCardBarUI();
		}
	}

	public IEnumerable<Card> GetActiveCards()
	{
		for (int i = 0; i < this.m_currentCardIds.Count; i++)
		{
			yield return this.GetSpawnedCardInstance((CardType)this.m_currentCardIds[i]);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.<GetActiveCards>c__Iterator0.MoveNext()).MethodHandle;
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
		yield break;
	}

	public void UpdateCatalystDisplay()
	{
		if (HUD_UI.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.UpdateCatalystDisplay()).MethodHandle;
			}
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateCatalysts(this.m_actor, this.m_cachedCardAbilities);
			HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.UpdateCatalysts(this.m_actor, this.m_cachedCardAbilities);
		}
	}

	private void UpdateCardBarUI()
	{
		if (NetworkClient.active && this.m_actor == GameFlowData.Get().activeOwnedActorData && HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_cardBar.Rebuild();
		}
		this.UpdateCatalystDisplay();
	}

	public float GetTargetableRadius(int actionTypeInt, ActorData caster)
	{
		if (actionTypeInt < this.m_abilities.Length)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.GetTargetableRadius(int, ActorData)).MethodHandle;
			}
			if (this.m_abilities[actionTypeInt].ability != null)
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
				if (this.m_abilities[actionTypeInt].ability.CanShowTargetableRadiusPreview())
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
					return this.m_abilities[actionTypeInt].ability.GetTargetableRadiusInSquares(caster);
				}
			}
		}
		return 0f;
	}

	public void OnClientCombatPhasePlayDataReceived(List<ClientResolutionAction> resolutionActions)
	{
		for (int i = 0; i <= 4; i++)
		{
			if (this.m_abilities[i] != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.OnClientCombatPhasePlayDataReceived(List<ClientResolutionAction>)).MethodHandle;
				}
				if (this.m_abilities[i].ability != null)
				{
					this.m_abilities[i].ability.OnClientCombatPhasePlayDataReceived(resolutionActions, this.m_actor);
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

	private void OnDrawGizmos()
	{
		if (CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			if (this.m_abilities != null)
			{
				foreach (AbilityData.AbilityEntry abilityEntry in this.m_abilities)
				{
					if (abilityEntry != null)
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
						if (abilityEntry.ability != null)
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
							abilityEntry.ability.DrawGizmos();
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
				this.DrawBoardVisibilityGizmos();
				return;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.OnDrawGizmos()).MethodHandle;
			}
		}
	}

	private void DrawBoardVisibilityGizmos()
	{
		if (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("ShowBoardSquareVisGizmo"))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.DrawBoardVisibilityGizmos()).MethodHandle;
			}
			BoardSquare playerFreeSquare = Board.\u000E().PlayerFreeSquare;
			if (playerFreeSquare != null)
			{
				for (int i = 0; i < 0xF; i++)
				{
					List<BoardSquare> squaresInBorderLayer = AreaEffectUtils.GetSquaresInBorderLayer(playerFreeSquare, i, false);
					using (List<BoardSquare>.Enumerator enumerator = squaresInBorderLayer.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							BoardSquare boardSquare = enumerator.Current;
							if (boardSquare.\u0016())
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
								if (playerFreeSquare.\u0013(boardSquare.x, boardSquare.y))
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
									Color white = Color.white;
									white.a = 0.5f;
									Gizmos.color = white;
									Vector3 size = 0.05f * Board.\u000E().squareSize * Vector3.one;
									size.y = 0.1f;
									Gizmos.DrawWireCube(boardSquare.ToVector3(), size);
								}
								else
								{
									Color red = Color.red;
									red.a = 0.5f;
									Gizmos.color = red;
									Vector3 size2 = 0.05f * Board.\u000E().squareSize * Vector3.one;
									size2.y = 0.1f;
									Gizmos.DrawWireCube(boardSquare.ToVector3(), size2);
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
			}
		}
	}

	private void UNetVersion()
	{
	}

	public AbilityData.ActionType Networkm_selectedActionForTargeting
	{
		get
		{
			return this.m_selectedActionForTargeting;
		}
		[param: In]
		set
		{
			base.SetSyncVar<AbilityData.ActionType>(value, ref this.m_selectedActionForTargeting, 0x10U);
		}
	}

	protected static void InvokeSyncListm_cooldownsSync(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_cooldownsSync called on server.");
			return;
		}
		((AbilityData)obj).m_cooldownsSync.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_consumedStockCount(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.InvokeSyncListm_consumedStockCount(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("SyncList m_consumedStockCount called on server.");
			return;
		}
		((AbilityData)obj).m_consumedStockCount.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_stockRefreshCountdowns(NetworkBehaviour obj, NetworkReader reader)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.InvokeSyncListm_stockRefreshCountdowns(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("SyncList m_stockRefreshCountdowns called on server.");
			return;
		}
		((AbilityData)obj).m_stockRefreshCountdowns.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_currentCardIds(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_currentCardIds called on server.");
			return;
		}
		((AbilityData)obj).m_currentCardIds.HandleMsg(reader);
	}

	protected static void InvokeCmdCmdClearCooldowns(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdClearCooldowns called on client.");
			return;
		}
		((AbilityData)obj).CmdClearCooldowns();
	}

	protected static void InvokeCmdCmdRefillStocks(NetworkBehaviour obj, NetworkReader reader)
	{
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.InvokeCmdCmdRefillStocks(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("Command CmdRefillStocks called on client.");
			return;
		}
		((AbilityData)obj).CmdRefillStocks();
	}

	public void CallCmdClearCooldowns()
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.CallCmdClearCooldowns()).MethodHandle;
			}
			Debug.LogError("Command function CmdClearCooldowns called on server.");
			return;
		}
		if (base.isServer)
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
			this.CmdClearCooldowns();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)AbilityData.kCmdCmdClearCooldowns);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		base.SendCommandInternal(networkWriter, 0, "CmdClearCooldowns");
	}

	public void CallCmdRefillStocks()
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.CallCmdRefillStocks()).MethodHandle;
			}
			Debug.LogError("Command function CmdRefillStocks called on server.");
			return;
		}
		if (base.isServer)
		{
			this.CmdRefillStocks();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)AbilityData.kCmdCmdRefillStocks);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		base.SendCommandInternal(networkWriter, 0, "CmdRefillStocks");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			SyncListInt.WriteInstance(writer, this.m_cooldownsSync);
			SyncListInt.WriteInstance(writer, this.m_consumedStockCount);
			SyncListInt.WriteInstance(writer, this.m_stockRefreshCountdowns);
			SyncListInt.WriteInstance(writer, this.m_currentCardIds);
			writer.Write((int)this.m_selectedActionForTargeting);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, this.m_cooldownsSync);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, this.m_consumedStockCount);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
		{
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, this.m_stockRefreshCountdowns);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, this.m_currentCardIds);
		}
		if ((base.syncVarDirtyBits & 0x10U) != 0U)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)this.m_selectedActionForTargeting);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			SyncListInt.ReadReference(reader, this.m_cooldownsSync);
			SyncListInt.ReadReference(reader, this.m_consumedStockCount);
			SyncListInt.ReadReference(reader, this.m_stockRefreshCountdowns);
			SyncListInt.ReadReference(reader, this.m_currentCardIds);
			this.m_selectedActionForTargeting = (AbilityData.ActionType)reader.ReadInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListInt.ReadReference(reader, this.m_cooldownsSync);
		}
		if ((num & 2) != 0)
		{
			SyncListInt.ReadReference(reader, this.m_consumedStockCount);
		}
		if ((num & 4) != 0)
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
			SyncListInt.ReadReference(reader, this.m_stockRefreshCountdowns);
		}
		if ((num & 8) != 0)
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
			SyncListInt.ReadReference(reader, this.m_currentCardIds);
		}
		if ((num & 0x10) != 0)
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
			this.m_selectedActionForTargeting = (AbilityData.ActionType)reader.ReadInt32();
		}
	}

	public enum ActionType
	{
		INVALID_ACTION = -1,
		ABILITY_0,
		ABILITY_1,
		ABILITY_2,
		ABILITY_3,
		ABILITY_4,
		ABILITY_5,
		ABILITY_6,
		CARD_0,
		CARD_1,
		CARD_2,
		CHAIN_0,
		CHAIN_1,
		CHAIN_2,
		CHAIN_3,
		NUM_ACTIONS
	}

	public class AbilityEntry
	{
		public Ability ability;

		public KeyPreference keyPreference;

		public string hotkey;

		private int m_cooldownRemaining;

		public int GetCooldownRemaining()
		{
			if (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("NoCooldowns"))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.AbilityEntry.GetCooldownRemaining()).MethodHandle;
				}
				return 0;
			}
			return this.m_cooldownRemaining;
		}

		public void SetCooldownRemaining(int remaining)
		{
			if (this.m_cooldownRemaining != remaining)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityData.AbilityEntry.SetCooldownRemaining(int)).MethodHandle;
				}
				this.m_cooldownRemaining = remaining;
			}
		}

		public void Setup(Ability ability, KeyPreference keyPreference)
		{
			this.ability = ability;
			this.keyPreference = keyPreference;
			this.InitHotkey();
		}

		public void InitHotkey()
		{
			this.hotkey = InputManager.Get().GetFullKeyString(this.keyPreference, true, true);
		}
	}

	[Serializable]
	public struct BotAbilityModSet
	{
		public BotDifficulty m_botDifficulty;

		public int[] m_abilityModIds;
	}
}
