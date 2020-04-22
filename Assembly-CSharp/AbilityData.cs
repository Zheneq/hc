using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class AbilityData : NetworkBehaviour
{
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
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return 0;
					}
				}
			}
			return m_cooldownRemaining;
		}

		public void SetCooldownRemaining(int remaining)
		{
			if (m_cooldownRemaining == remaining)
			{
				return;
			}
			while (true)
			{
				m_cooldownRemaining = remaining;
				return;
			}
		}

		public void Setup(Ability ability, KeyPreference keyPreference)
		{
			this.ability = ability;
			this.keyPreference = keyPreference;
			InitHotkey();
		}

		public void InitHotkey()
		{
			hotkey = InputManager.Get().GetFullKeyString(keyPreference, true, true);
		}
	}

	[Serializable]
	public struct BotAbilityModSet
	{
		public BotDifficulty m_botDifficulty;

		public int[] m_abilityModIds;
	}

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

	public const int CHAIN_0 = 10;

	public const int CHAIN_1 = 11;

	public const int CHAIN_2 = 12;

	public const int CHAIN_3 = 13;

	public const int NUM_ACTIONS = 14;

	public const int NUM_ABILITIES = 7;

	public const int NUM_CARDS = 3;

	public const int LAST_CARD = 9;

	private SyncListInt m_cooldownsSync = new SyncListInt();

	private SyncListInt m_consumedStockCount = new SyncListInt();

	private SyncListInt m_stockRefreshCountdowns = new SyncListInt();

	private SyncListInt m_currentCardIds = new SyncListInt();

	private AbilityEntry[] m_abilities;

	private List<Ability> m_allChainAbilities;

	private List<ActionType> m_allChainAbilityParentActionTypes;

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
	public List<BotAbilityModSet> m_botDifficultyAbilityModSets = new List<BotAbilityModSet>();

	private List<Ability> m_abilitiesList;

	[Separator("For Ability Kit Inspector", true)]
	public List<Component> m_compsToInspectInAbilityKitInspector;

	[Header("-- Name of directory where sequence prefab is located")]
	public string m_sequenceDirNameOverride = string.Empty;

	public AbilitySetupNotes m_setupNotes;

	private ActorData m_softTargetedActor;

	private Ability m_selectedAbility;

	[SyncVar]
	private ActionType m_selectedActionForTargeting = ActionType.INVALID_ACTION;

	private List<ActionType> m_actionsToCancelForTurnRedo;

	private bool m_loggedErrorForNullAction;

	private bool m_cancelMovementForTurnRedo;

	private ActionType m_actionToSelectWhenEnteringDecisionState = ActionType.INVALID_ACTION;

	private bool m_retargetActionWithoutClearingOldAbilities;

	private ActorData m_actor;

	private float m_lastPingSendTime;

	private bool m_abilitySpritesInitialized;

	public static Color s_freeActionTextColor;

	public static float s_heightPerButton;

	public static float s_heightFromBottom;

	public static float s_widthPerButton;

	public static int s_abilityButtonSlots;

	public static float s_widthOfAllButtons;

	private Ability m_lastSelectedAbility;

	[CompilerGenerated]
	private static Comparison<ActorData> _003C_003Ef__mg_0024cache0;

	private static int kListm_cooldownsSync;

	private static int kListm_consumedStockCount;

	private static int kListm_stockRefreshCountdowns;

	private static int kListm_currentCardIds;

	private static int kCmdCmdClearCooldowns;

	private static int kCmdCmdRefillStocks;

	public SyncListInt CurrentCardIDs => m_currentCardIds;

	public AbilityEntry[] abilityEntries => m_abilities;

	public Sprite m_sprite0 => GetSpriteFromPath(m_spritePath0);

	public Sprite m_sprite1 => GetSpriteFromPath(m_spritePath1);

	public Sprite m_sprite2 => GetSpriteFromPath(m_spritePath2);

	public Sprite m_sprite3 => GetSpriteFromPath(m_spritePath3);

	public Sprite m_sprite4 => GetSpriteFromPath(m_spritePath4);

	public Sprite m_sprite5 => GetSpriteFromPath(m_spritePath5);

	public Sprite m_sprite6 => GetSpriteFromPath(m_spritePath6);

	public ActorData SoftTargetedActor
	{
		get
		{
			return m_softTargetedActor;
		}
		set
		{
			if (!(m_softTargetedActor != value))
			{
				return;
			}
			while (true)
			{
				m_softTargetedActor = value;
				ActorData actor = m_actor;
				if (!(actor == GameFlowData.Get().activeOwnedActorData))
				{
					return;
				}
				while (true)
				{
					CameraManager cameraManager = CameraManager.Get();
					if ((bool)m_softTargetedActor)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								cameraManager.SetTargetObjectToMouse(m_softTargetedActor.gameObject, CameraManager.CameraTargetReason.AbilitySoftTargeting);
								return;
							}
						}
					}
					if (!cameraManager.IsOnMainCamera(GameFlowData.Get().activeOwnedActorData))
					{
						while (true)
						{
							cameraManager.SetTargetObject(base.gameObject, CameraManager.CameraTargetReason.AbilitySoftTargeting);
							return;
						}
					}
					return;
				}
			}
		}
	}

	public ActionType Networkm_selectedActionForTargeting
	{
		get
		{
			return m_selectedActionForTargeting;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_selectedActionForTargeting, 16u);
		}
	}

	static AbilityData()
	{
		s_freeActionTextColor = new Color(0f, 1f, 0f);
		s_heightPerButton = 75f;
		s_heightFromBottom = 75f;
		s_widthPerButton = 64f;
		s_abilityButtonSlots = 8;
		s_widthOfAllButtons = s_widthPerButton * (float)s_abilityButtonSlots;
		kCmdCmdClearCooldowns = 36238543;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbilityData), kCmdCmdClearCooldowns, InvokeCmdCmdClearCooldowns);
		kCmdCmdRefillStocks = 1108895015;
		NetworkBehaviour.RegisterCommandDelegate(typeof(AbilityData), kCmdCmdRefillStocks, InvokeCmdCmdRefillStocks);
		kListm_cooldownsSync = -1695732229;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(AbilityData), kListm_cooldownsSync, InvokeSyncListm_cooldownsSync);
		kListm_consumedStockCount = 1389109193;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(AbilityData), kListm_consumedStockCount, InvokeSyncListm_consumedStockCount);
		kListm_stockRefreshCountdowns = -1016879281;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(AbilityData), kListm_stockRefreshCountdowns, InvokeSyncListm_stockRefreshCountdowns);
		kListm_currentCardIds = 384100343;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(AbilityData), kListm_currentCardIds, InvokeSyncListm_currentCardIds);
		NetworkCRC.RegisterBehaviour("AbilityData", 0);
	}

	private Sprite GetSpriteFromPath(string path)
	{
		if (path.IsNullOrEmpty())
		{
			return null;
		}
		return (Sprite)Resources.Load(path, typeof(Sprite));
	}

	public List<Ability> GetAbilitiesAsList()
	{
		if (m_abilitiesList != null)
		{
			if (m_abilitiesList.Count != 0)
			{
				goto IL_0088;
			}
		}
		m_abilitiesList = new List<Ability>();
		m_abilitiesList.Add(m_ability0);
		m_abilitiesList.Add(m_ability1);
		m_abilitiesList.Add(m_ability2);
		m_abilitiesList.Add(m_ability3);
		m_abilitiesList.Add(m_ability4);
		goto IL_0088;
		IL_0088:
		return m_abilitiesList;
	}

	public Ability GetAbilityAtIndex(int index)
	{
		switch (index)
		{
		case 0:
			return m_ability0;
		case 1:
			return m_ability1;
		case 2:
			return m_ability2;
		case 3:
			return m_ability3;
		case 4:
			return m_ability4;
		default:
			return null;
		}
	}

	public ActionType GetSelectedActionTypeForTargeting()
	{
		return m_selectedActionForTargeting;
	}

	public bool HasToggledAction(ActionType actionType)
	{
		bool result = false;
		ActorTeamSensitiveData teamSensitiveData_authority = m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
		{
			result = teamSensitiveData_authority.HasToggledAction(actionType);
		}
		return result;
	}

	public bool HasQueuedAction(ActionType actionType)
	{
		bool result = false;
		ActorTeamSensitiveData teamSensitiveData_authority = m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
		{
			result = teamSensitiveData_authority.HasQueuedAction(actionType);
		}
		return result;
	}

	public bool HasQueuedAbilityOfType(Type abilityType)
	{
		ActionType actionTypeOfAbility = GetActionTypeOfAbility(GetAbilityOfType(abilityType));
		return HasQueuedAction(actionTypeOfAbility);
	}

	public bool HasQueuedAbilityInPhase(AbilityPriority phase)
	{
		bool result = false;
		ActorTeamSensitiveData teamSensitiveData_authority = m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
		{
			result = teamSensitiveData_authority.HasQueuedAbilityInPhase(phase);
		}
		return result;
	}

	private void Awake()
	{
		m_abilities = new AbilityEntry[14];
		for (int i = 0; i < 14; i++)
		{
			m_abilities[i] = new AbilityEntry();
		}
		m_abilities[0].Setup(m_ability0, KeyPreference.Ability1);
		m_abilities[1].Setup(m_ability1, KeyPreference.Ability2);
		m_abilities[2].Setup(m_ability2, KeyPreference.Ability3);
		m_abilities[3].Setup(m_ability3, KeyPreference.Ability4);
		m_abilities[4].Setup(m_ability4, KeyPreference.Ability5);
		InitAbilitySprites();
		m_allChainAbilities = new List<Ability>();
		m_allChainAbilityParentActionTypes = new List<ActionType>();
		for (int j = 0; j < m_abilities.Length; j++)
		{
			AbilityEntry abilityEntry = m_abilities[j];
			if (!(abilityEntry.ability != null))
			{
				continue;
			}
			Ability[] chainAbilities = abilityEntry.ability.GetChainAbilities();
			foreach (Ability ability in chainAbilities)
			{
				if (ability != null)
				{
					AddToAllChainAbilitiesList(ability, (ActionType)j);
				}
			}
		}
		while (true)
		{
			m_cooldowns = new Dictionary<string, int>();
			m_actor = GetComponent<ActorData>();
			for (int l = 0; l < 3; l++)
			{
				m_cachedCardAbilities.Add(null);
			}
			m_cooldownsSync.InitializeBehaviour(this, kListm_cooldownsSync);
			m_consumedStockCount.InitializeBehaviour(this, kListm_consumedStockCount);
			m_stockRefreshCountdowns.InitializeBehaviour(this, kListm_stockRefreshCountdowns);
			m_currentCardIds.InitializeBehaviour(this, kListm_currentCardIds);
			return;
		}
	}

	public void InitAbilitySprites()
	{
		if (m_abilitySpritesInitialized)
		{
			return;
		}
		while (true)
		{
			SetSpriteForAbility(m_ability0, m_sprite0);
			SetSpriteForAbility(m_ability1, m_sprite1);
			SetSpriteForAbility(m_ability2, m_sprite2);
			SetSpriteForAbility(m_ability3, m_sprite3);
			SetSpriteForAbility(m_ability4, m_sprite4);
			m_abilitySpritesInitialized = true;
			return;
		}
	}

	private void AddToAllChainAbilitiesList(Ability aChainAbility, ActionType parentActionType)
	{
		m_allChainAbilities.Add(aChainAbility);
		m_allChainAbilityParentActionTypes.Add(parentActionType);
	}

	private void ClearAllChainAbilitiesList()
	{
		m_allChainAbilities.Clear();
		m_allChainAbilityParentActionTypes.Clear();
	}

	private void SetSpriteForAbility(Ability ability, Sprite sprite)
	{
		if (!(ability != null))
		{
			return;
		}
		ability.sprite = sprite;
		if (ability.m_chainAbilities == null)
		{
			return;
		}
		while (true)
		{
			Ability[] chainAbilities = ability.m_chainAbilities;
			foreach (Ability ability2 in chainAbilities)
			{
				if (ability2 != null)
				{
					ability2.sprite = sprite;
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

	public override void OnStartClient()
	{
		m_cooldownsSync.Callback = SyncListCallbackCoolDownsSync;
		m_currentCardIds.Callback = SyncListCallbackCurrentCardsChanged;
	}

	public override void OnStartServer()
	{
		for (int i = 0; i < 14; i++)
		{
			m_cooldownsSync.Add(0);
			m_consumedStockCount.Add(0);
			m_stockRefreshCountdowns.Add(0);
		}
		while (true)
		{
			for (int j = 0; j < 3; j++)
			{
				m_currentCardIds.Add(-1);
			}
			while (true)
			{
				if (GameplayUtils.IsPlayerControlled(this))
				{
					int num = GameplayData.Get().m_turnsAbilitiesUnlock.Length;
					for (int k = 0; k < num; k++)
					{
						if (k < 7)
						{
							PlaceInCooldownTillTurn((ActionType)k, GameplayData.Get().m_turnsAbilitiesUnlock[k]);
							continue;
						}
						break;
					}
				}
				InitializeStockCounts();
				return;
			}
		}
	}

	public void ReInitializeChainAbilityList()
	{
		ClearAllChainAbilitiesList();
		for (int i = 0; i < m_abilities.Length; i++)
		{
			AbilityEntry abilityEntry = m_abilities[i];
			if (!(abilityEntry.ability != null))
			{
				continue;
			}
			Ability[] chainAbilities = abilityEntry.ability.GetChainAbilities();
			foreach (Ability ability in chainAbilities)
			{
				if (ability != null)
				{
					AddToAllChainAbilitiesList(ability, (ActionType)i);
				}
			}
		}
	}

	internal static bool IsCard(ActionType actionType)
	{
		int result;
		if (actionType >= ActionType.CARD_0)
		{
			result = ((actionType <= ActionType.CARD_2) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	internal static bool IsChain(ActionType actionType)
	{
		int result;
		if (actionType >= ActionType.CHAIN_0)
		{
			result = ((actionType <= ActionType.CHAIN_2) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	internal static bool IsCharacterSpecificAbility(ActionType actionType)
	{
		if (actionType == ActionType.INVALID_ACTION)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (IsCard(actionType))
		{
			while (true)
			{
				switch (2)
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

	internal List<CameraShotSequence> GetTauntListForActionTypeForPlayer(PersistedCharacterData characterData, CharacterResourceLink character, ActionType actionType)
	{
		List<CameraShotSequence> list = new List<CameraShotSequence>();
		if (characterData != null)
		{
			Ability abilityOfActionType = GetAbilityOfActionType(actionType);
			if (abilityOfActionType != null)
			{
				for (int i = 0; i < character.m_taunts.Count; i++)
				{
					CharacterTaunt characterTaunt = character.m_taunts[i];
					if (characterTaunt.m_actionForTaunt != actionType)
					{
						continue;
					}
					if (i >= characterData.CharacterComponent.Taunts.Count || !characterData.CharacterComponent.Taunts[i].Unlocked)
					{
						continue;
					}
					TauntCameraSet tauntCamSetData = m_actor.m_tauntCamSetData;
					object obj;
					if (tauntCamSetData != null)
					{
						obj = tauntCamSetData.GetTauntCam(characterTaunt.m_uniqueID);
					}
					else
					{
						obj = null;
					}
					CameraShotSequence cameraShotSequence = (CameraShotSequence)obj;
					if (!(cameraShotSequence != null))
					{
						continue;
					}
					if (abilityOfActionType.CanTriggerAnimAtIndexForTaunt(cameraShotSequence.m_animIndex))
					{
						list.Add(cameraShotSequence);
					}
				}
			}
		}
		return list;
	}

	internal List<CharacterTaunt> GetFullTauntListForActionType(CharacterResourceLink character, ActionType actionType, bool includeHidden = false)
	{
		List<CharacterTaunt> list = new List<CharacterTaunt>();
		for (int i = 0; i < character.m_taunts.Count; i++)
		{
			CharacterTaunt characterTaunt = character.m_taunts[i];
			if (characterTaunt.m_actionForTaunt != actionType)
			{
				continue;
			}
			if (!includeHidden)
			{
				if (characterTaunt.m_isHidden)
				{
					continue;
				}
			}
			list.Add(characterTaunt);
		}
		while (true)
		{
			return list;
		}
	}

	internal static bool CanTauntForActionTypeForPlayer(PersistedCharacterData characterData, CharacterResourceLink character, ActionType actionType, bool checkTauntUniqueId, int uniqueId)
	{
		if (CameraManager.Get() != null)
		{
			if (CameraManager.Get().m_abilityCinematicState == CameraManager.AbilityCinematicState.Never)
			{
				return false;
			}
		}
		if (characterData != null && character.m_characterType != 0 && actionType != ActionType.INVALID_ACTION)
		{
			int count = characterData.CharacterComponent.Taunts.Count;
			for (int i = 0; i < character.m_taunts.Count; i++)
			{
				if (i < count)
				{
					CharacterTaunt characterTaunt = character.m_taunts[i];
					if (characterTaunt == null)
					{
						continue;
					}
					if (characterTaunt.m_actionForTaunt != actionType)
					{
						continue;
					}
					if (checkTauntUniqueId)
					{
						if (characterTaunt.m_uniqueID != uniqueId)
						{
							continue;
						}
					}
					if (!characterData.CharacterComponent.Taunts[i].Unlocked || !GameManager.Get().GameplayOverrides.IsTauntAllowed(character.m_characterType, (int)actionType, characterTaunt.m_uniqueID))
					{
						continue;
					}
					while (true)
					{
						return true;
					}
				}
				break;
			}
		}
		return false;
	}

	internal static bool CanTauntApplyToActionType(CharacterResourceLink character, ActionType actionType)
	{
		if (CameraManager.Get() != null && CameraManager.Get().m_abilityCinematicState == CameraManager.AbilityCinematicState.Never)
		{
			return false;
		}
		if (character.m_characterType != 0)
		{
			if (actionType != ActionType.INVALID_ACTION)
			{
				int num = 0;
				foreach (CharacterTaunt taunt in character.m_taunts)
				{
					if (taunt.m_actionForTaunt == actionType)
					{
						if (GameManager.Get().GameplayOverrides.IsTauntAllowed(character.m_characterType, (int)actionType, taunt.m_uniqueID))
						{
							return true;
						}
					}
					num++;
				}
			}
		}
		return false;
	}

	public List<AbilityEntry> GetQueuedOrAimingAbilitiesForPhase(UIQueueListPanel.UIPhase actionPhase)
	{
		List<AbilityEntry> list = new List<AbilityEntry>();
		ActorTeamSensitiveData teamSensitiveData_authority = m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
		{
			for (int i = 0; i < 14; i++)
			{
				Ability ability = m_abilities[i].ability;
				if (!teamSensitiveData_authority.HasQueuedAction(i))
				{
					if (!(ability != null))
					{
						continue;
					}
					if (!(ability == GetSelectedAbility()))
					{
						continue;
					}
				}
				UIQueueListPanel.UIPhase uIPhaseFromAbilityPriority = UIQueueListPanel.GetUIPhaseFromAbilityPriority(m_abilities[i].ability.RunPriority);
				if (uIPhaseFromAbilityPriority == actionPhase)
				{
					list.Add(m_abilities[i]);
				}
			}
		}
		return list;
	}

	public List<AbilityEntry> GetQueuedOrAimingAbilities()
	{
		List<AbilityEntry> list = new List<AbilityEntry>();
		ActorTeamSensitiveData teamSensitiveData_authority = m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
		{
			for (int i = 0; i < 14; i++)
			{
				Ability ability = m_abilities[i].ability;
				if (!teamSensitiveData_authority.HasQueuedAction(i))
				{
					if (!(ability != null) || !(ability == GetSelectedAbility()))
					{
						continue;
					}
				}
				list.Add(m_abilities[i]);
			}
		}
		return list;
	}

	private static int CompareTabTargetsByActiveOwnedActorDistance(ActorData a, ActorData b)
	{
		if (GameFlowData.Get().activeOwnedActorData == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return 0;
				}
			}
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
		ActorTurnSM actorTurnSM = m_actor.GetActorTurnSM();
		if (!actorTurnSM)
		{
			return;
		}
		while (true)
		{
			int targetSelectionIndex = actorTurnSM.GetTargetSelectionIndex();
			int numTargets = m_selectedAbility.GetNumTargets();
			if (targetSelectionIndex >= numTargets)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						SoftTargetedActor = null;
						return;
					}
				}
			}
			List<ActorData> validTargets = GetValidTargets(m_selectedAbility, targetSelectionIndex);
			if (validTargets.Count > 0)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						validTargets.Sort(CompareTabTargetsByActiveOwnedActorDistance);
						if (SoftTargetedActor == null)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									SoftTargetedActor = validTargets[0];
									return;
								}
							}
						}
						int num = validTargets.FindIndex((ActorData entry) => entry == SoftTargetedActor);
						ActorData actorData2 = SoftTargetedActor = validTargets[(num + 1) % validTargets.Count];
						return;
					}
					}
				}
			}
			SoftTargetedActor = null;
			return;
		}
	}

	private void Update()
	{
		if (GameFlowData.Get().gameState != GameState.BothTeams_Decision)
		{
			return;
		}
		while (true)
		{
			if (!(GameFlowData.Get().activeOwnedActorData == m_actor))
			{
				return;
			}
			while (true)
			{
				ActorTurnSM actorTurnSM = m_actor.GetActorTurnSM();
				if (m_actionToSelectWhenEnteringDecisionState != ActionType.INVALID_ACTION && actorTurnSM.CurrentState == TurnStateEnum.DECIDING)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							if (RedoTurn(GetAbilityOfActionType(m_actionToSelectWhenEnteringDecisionState), m_actionToSelectWhenEnteringDecisionState, m_actionsToCancelForTurnRedo, m_cancelMovementForTurnRedo, m_retargetActionWithoutClearingOldAbilities))
							{
								ClearActionsToCancelOnTargetingComplete();
							}
							m_actionToSelectWhenEnteringDecisionState = ActionType.INVALID_ACTION;
							return;
						}
					}
				}
				if (m_actionsToCancelForTurnRedo != null)
				{
					if (actorTurnSM.CurrentState == TurnStateEnum.DECIDING)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								if (RedoTurn(null, ActionType.INVALID_ACTION, m_actionsToCancelForTurnRedo, false, false))
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											break;
										default:
											ClearActionsToCancelOnTargetingComplete();
											return;
										}
									}
								}
								return;
							}
						}
					}
				}
				if (!actorTurnSM.CanSelectAbility())
				{
					if (!actorTurnSM.CanQueueSimpleAction())
					{
						return;
					}
				}
				if (m_actor.IsDead())
				{
					return;
				}
				while (true)
				{
					for (int i = 0; i < m_abilities.Length; i++)
					{
						AbilityEntry abilityEntry = m_abilities[i];
						if (abilityEntry == null || !(abilityEntry.ability != null) || abilityEntry.keyPreference == KeyPreference.NullPreference || !InputManager.Get().IsKeyBindingNewlyHeld(abilityEntry.keyPreference))
						{
							continue;
						}
						if (UITutorialFullscreenPanel.Get().IsAnyPanelVisible())
						{
							continue;
						}
						ActionType actionType = (ActionType)i;
						if (AbilityButtonPressed(actionType, abilityEntry.ability))
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
					}
					while (true)
					{
						switch (1)
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
	}

	public bool AbilityButtonPressed(ActionType actionType, Ability ability)
	{
		if (ability == null)
		{
			while (true)
			{
				return false;
			}
		}
		bool flag = InputManager.Get().IsKeyBindingHeld(KeyPreference.MinimapPing);
		ActorTurnSM actorTurnSM = m_actor.GetActorTurnSM();
		bool flag2 = actorTurnSM.CanSelectAbility();
		bool flag3 = actorTurnSM.CanQueueSimpleAction();
		if (!flag2 && !flag3)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (flag)
					{
						SendAbilityPing(false, actionType, ability);
					}
					return false;
				}
			}
		}
		m_cancelMovementForTurnRedo = false;
		m_actionsToCancelForTurnRedo = null;
		bool flag4 = InputManager.Get().IsKeyBindingHeld(KeyPreference.AbilityRetargetingModifier);
		if (HasQueuedAction(actionType))
		{
			if (!flag4)
			{
				if (flag)
				{
					SendAbilityPing(true, actionType, ability);
				}
				else if (actorTurnSM.CurrentState == TurnStateEnum.CONFIRMED)
				{
					actorTurnSM.BackToDecidingState();
					m_actionsToCancelForTurnRedo = new List<ActionType>();
					m_actionsToCancelForTurnRedo.Add(actionType);
					m_retargetActionWithoutClearingOldAbilities = false;
				}
				else
				{
					actorTurnSM.RequestCancelAction(actionType, false);
					UISounds.GetUISounds().Play("ui/ingame/v1/action_undo");
				}
				goto IL_0205;
			}
		}
		if (CanQueueActionByCancelingOthers(ability, actionType, flag3, flag2, out List<ActionType> actionsToCancel, out bool cancelMovement))
		{
			if (flag)
			{
				SendAbilityPing(true, actionType, ability);
			}
			else
			{
				m_retargetActionWithoutClearingOldAbilities = false;
				if (actorTurnSM.CurrentState != TurnStateEnum.CONFIRMED)
				{
					if (actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION)
					{
						if (GetSelectedActionType() == actionType)
						{
							if (!SinglePlayerManager.IsCancelDisabled())
							{
								ClearSelectedAbility();
								actorTurnSM.BackToDecidingState();
								goto IL_0205;
							}
						}
					}
					return RedoTurn(ability, actionType, actionsToCancel, cancelMovement, flag4);
				}
				actorTurnSM.BackToDecidingState();
				m_cancelMovementForTurnRedo = cancelMovement;
				m_actionsToCancelForTurnRedo = actionsToCancel;
				m_actionToSelectWhenEnteringDecisionState = actionType;
				m_retargetActionWithoutClearingOldAbilities = flag4;
			}
		}
		else if (flag)
		{
			SendAbilityPing(false, actionType, ability);
		}
		goto IL_0205;
		IL_0205:
		return false;
	}

	private void SendAbilityPing(bool selectable, ActionType actionType, Ability ability)
	{
		if (TextConsole.Get() == null)
		{
			return;
		}
		if (!(m_lastPingSendTime <= 0f))
		{
			if (!(Time.time - m_lastPingSendTime > HUD_UIResources.Get().m_mapPingCooldown))
			{
				return;
			}
		}
		LocalizationArg_AbilityPing localizedPing = LocalizationArg_AbilityPing.Create(m_actor.m_characterType, ability, selectable, Mathf.Max(GetAbilityEntryOfActionType(actionType).GetCooldownRemaining(), GetTurnsTillUnlock(actionType)), actionType == ActionType.ABILITY_4, m_actor.GetEnergyToDisplay(), m_actor.GetActualMaxTechPoints());
		m_actor.SendAbilityPingRequestToServer((int)m_actor.GetTeam(), localizedPing);
		m_lastPingSendTime = Time.time;
	}

	public bool RedoTurn(Ability ability, ActionType actionType, List<ActionType> actionsToCancel, bool cancelMovement, bool retargetingModifierKeyHeld)
	{
		object obj;
		if (m_actor != null)
		{
			obj = m_actor.GetActorController();
		}
		else
		{
			obj = null;
		}
		ActorController actorController = (ActorController)obj;
		object obj2;
		if (m_actor != null)
		{
			obj2 = m_actor.GetActorTurnSM();
		}
		else
		{
			obj2 = null;
		}
		ActorTurnSM actorTurnSM = (ActorTurnSM)obj2;
		if (ability != null && !ability.IsSimpleAction())
		{
			if (retargetingModifierKeyHeld)
			{
				if (!actionsToCancel.IsNullOrEmpty())
				{
					if (actionsToCancel.Contains(actionType))
					{
						ability.BackupTargetingForRedo(actorTurnSM);
					}
				}
				SetSelectedAbility(ability);
				actorController.SendSelectAbilityRequest();
				m_cancelMovementForTurnRedo = cancelMovement;
				m_actionsToCancelForTurnRedo = actionsToCancel;
				return false;
			}
		}
		if (cancelMovement)
		{
			actorTurnSM.RequestCancelMovement();
			UISounds.GetUISounds().Play("ui/ingame/v1/move_undo");
		}
		if (actionsToCancel != null)
		{
			m_loggedErrorForNullAction = false;
			using (List<ActionType>.Enumerator enumerator = actionsToCancel.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActionType current = enumerator.Current;
					actorTurnSM.RequestCancelAction(current, true);
					UISounds.GetUISounds().Play("ui/ingame/v1/action_undo");
				}
			}
		}
		else if (!m_loggedErrorForNullAction)
		{
			m_loggedErrorForNullAction = true;
			Debug.LogError("RedoTurn() - actionsToCancel is null");
		}
		if (ability != null)
		{
			if (ability.IsSimpleAction())
			{
				if (actionsToCancel != null)
				{
					if (actionsToCancel.Contains(actionType))
					{
						goto IL_01f4;
					}
				}
				if (HasQueuedAction(actionType))
				{
					actorTurnSM.RequestCancelAction(actionType, true);
					UISounds.GetUISounds().Play("ui/ingame/v1/action_undo");
				}
				else
				{
					actorTurnSM.OnQueueAbilityRequest(actionType);
					actorController.SendQueueSimpleActionRequest(actionType);
				}
			}
			else
			{
				SetSelectedAbility(ability);
				actorController.SendSelectAbilityRequest();
			}
		}
		goto IL_01f4;
		IL_01f4:
		return true;
	}

	public bool CanQueueActionByCancelingOthers(Ability ability, ActionType actionType, bool canQueueSimpleAction, bool canSelectAbility, out List<ActionType> actionsToCancel, out bool cancelMovement)
	{
		bool flag = false;
		actionsToCancel = new List<ActionType>();
		cancelMovement = false;
		if (ability.IsSimpleAction())
		{
			if (canQueueSimpleAction)
			{
				if (ValidateActionIsRequestableDisregardingQueuedActions(actionType))
				{
					flag = true;
				}
			}
		}
		else if (canSelectAbility)
		{
			if (ValidateActionIsRequestableDisregardingQueuedActions(actionType))
			{
				flag = true;
			}
		}
		if (flag)
		{
			if (ability.IsFreeAction())
			{
				if (ability.GetRunPriority() != AbilityPriority.Evasion)
				{
					if (!IsCard(actionType))
					{
						if (ability.IsFreeAction() && HasQueuedAction(actionType))
						{
							actionsToCancel.Add(actionType);
						}
						goto IL_0179;
					}
				}
			}
			for (int i = 0; i < 14; i++)
			{
				ActionType actionType2 = (ActionType)i;
				if (!HasQueuedAction(actionType2))
				{
					continue;
				}
				Ability abilityOfActionType = GetAbilityOfActionType(actionType2);
				if (!(abilityOfActionType != null))
				{
					continue;
				}
				if ((abilityOfActionType.IsFreeAction() || ability.IsFreeAction()) && (abilityOfActionType.GetRunPriority() != AbilityPriority.Evasion || ability.GetRunPriority() != AbilityPriority.Evasion))
				{
					if (!IsCard(GetActionTypeOfAbility(abilityOfActionType)))
					{
						continue;
					}
					if (!IsCard(actionType))
					{
						continue;
					}
				}
				actionsToCancel.Add(actionType2);
			}
			goto IL_0179;
		}
		goto IL_01cb;
		IL_01cb:
		return flag;
		IL_0179:
		if (m_actor.HasQueuedMovement())
		{
			if (ability.GetMovementAdjustment() == Ability.MovementAdjustment.NoMovement)
			{
				cancelMovement = true;
			}
			else if (ability.GetMovementAdjustment() == Ability.MovementAdjustment.ReducedMovement)
			{
				cancelMovement = !m_actor.QueuedMovementAllowsAbility;
			}
		}
		goto IL_01cb;
	}

	public bool GetActionsToCancelOnTargetingComplete(ref List<ActionType> actionsToCancel, ref bool cancelMovement)
	{
		if (!m_cancelMovementForTurnRedo)
		{
			if (m_actionsToCancelForTurnRedo.IsNullOrEmpty())
			{
				return false;
			}
		}
		cancelMovement = m_cancelMovementForTurnRedo;
		actionsToCancel = m_actionsToCancelForTurnRedo;
		return true;
	}

	public void ClearActionsToCancelOnTargetingComplete()
	{
		m_cancelMovementForTurnRedo = false;
		m_actionsToCancelForTurnRedo = null;
		m_actionToSelectWhenEnteringDecisionState = ActionType.INVALID_ACTION;
		if (m_lastSelectedAbility != null)
		{
			m_lastSelectedAbility.DestroyBackupTargetingInfo(false);
		}
		m_retargetActionWithoutClearingOldAbilities = false;
	}

	public List<ActorData> GetValidTargets(Ability testAbility, int targetIndex)
	{
		ActorData actor = m_actor;
		FogOfWar component = GetComponent<FogOfWar>();
		if (actor.IsDead())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return new List<ActorData>();
				}
			}
		}
		List<ActorData> list = new List<ActorData>();
		bool checkLoS = testAbility.GetCheckLoS(targetIndex);
		bool flag = GameplayUtils.IsPlayerControlled(this);
		using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().GetActors().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (flag)
				{
					if (!component.IsVisible(current.GetCurrentBoardSquare()))
					{
						continue;
					}
				}
				if (checkLoS)
				{
					if (!component.IsVisibleBySelf(current.GetCurrentBoardSquare()))
					{
						if (!actor.IsLineOfSightVisibleException(current))
						{
							continue;
						}
					}
				}
				list.Add(current);
			}
		}
		List<ActorData> list2 = new List<ActorData>();
		for (int i = 0; i < list.Count; i++)
		{
			ActorData actorData = list[i];
			if (!actorData.IsDead())
			{
				AbilityTarget target = AbilityTarget.CreateAbilityTargetFromActor(actorData, actor);
				if (ValidateAbilityOnTarget(testAbility, target, targetIndex))
				{
					list2.Add(actorData);
				}
			}
		}
		while (true)
		{
			return list2;
		}
	}

	private void SyncListCallbackCoolDownsSync(SyncList<int>.Operation op, int _incorrectIndexBugIn51And52)
	{
		if (NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < 14; i++)
			{
				m_abilities[i].SetCooldownRemaining(m_cooldownsSync[i]);
			}
			while (true)
			{
				switch (1)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private void SyncListCallbackCurrentCardsChanged(SyncList<int>.Operation op, int index)
	{
		if (NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			if (!(CardManagerData.Get() != null))
			{
				return;
			}
			while (true)
			{
				for (int i = 0; i < m_currentCardIds.Count; i++)
				{
					Ability useAbility = null;
					if (m_currentCardIds[i] > 0)
					{
						Card spawnedCardInstance = GetSpawnedCardInstance((CardType)m_currentCardIds[i]);
						object obj;
						if (spawnedCardInstance != null)
						{
							obj = spawnedCardInstance.m_useAbility;
						}
						else
						{
							obj = null;
						}
						useAbility = (Ability)obj;
					}
					SetupCardAbility(i, useAbility);
				}
				while (true)
				{
					UpdateCardBarUI();
					return;
				}
			}
		}
	}

	public void OnQueuedAbilitiesChanged()
	{
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < 14; i++)
		{
			ActionType type = (ActionType)i;
			Ability abilityOfActionType = GetAbilityOfActionType(type);
			if (!flag)
			{
				if (abilityOfActionType != null)
				{
					if (abilityOfActionType.GetAffectsMovement())
					{
						flag = true;
					}
				}
			}
			if (flag2)
			{
				continue;
			}
			if (abilityOfActionType != null)
			{
				if (abilityOfActionType.ShouldUpdateDrawnTargetersOnQueueChange())
				{
					flag2 = true;
				}
			}
		}
		while (true)
		{
			if (flag)
			{
				if (!GameplayUtils.IsMinion(this))
				{
					ActorMovement component = GetComponent<ActorMovement>();
					component.UpdateSquaresCanMoveTo();
				}
			}
			if (flag2 && m_actor.GetActorTargeting() != null)
			{
				while (true)
				{
					m_actor.GetActorTargeting().MarkForForceRedraw();
					return;
				}
			}
			return;
		}
	}

	private void OnRespawn()
	{
		for (int i = 0; i < m_abilities.Length; i++)
		{
			AbilityEntry abilityEntry = m_abilities[i];
			if (abilityEntry == null)
			{
				continue;
			}
			ActionType action = (ActionType)i;
			Ability ability = abilityEntry.ability;
			if (ability == null)
			{
				continue;
			}
			if (AbilityUtils.AbilityHasTag(ability, AbilityTags.TriggerCooldownOnRespawn))
			{
				TriggerCooldown(action);
			}
			if (AbilityUtils.AbilityHasTag(ability, AbilityTags.ClearCooldownOnRespawn))
			{
				ClearCooldown(action);
			}
		}
	}

	public void SetSelectedAbility(Ability selectedAbility)
	{
		object obj;
		if (m_actor == null)
		{
			obj = null;
		}
		else
		{
			obj = m_actor.GetActorTurnSM();
		}
		ActorTurnSM actorTurnSM = (ActorTurnSM)obj;
		int num;
		if (GameFlowData.Get().activeOwnedActorData == m_actor)
		{
			num = ((m_actor != null) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		if ((bool)m_selectedAbility)
		{
			if (flag)
			{
				m_selectedAbility.OnAbilityDeselect();
			}
		}
		m_selectedAbility = selectedAbility;
		Networkm_selectedActionForTargeting = GetActionTypeOfAbility(m_selectedAbility);
		if ((bool)m_selectedAbility)
		{
			if (flag)
			{
				m_selectedAbility.OnAbilitySelect();
			}
		}
		if (actorTurnSM != null)
		{
			actorTurnSM.OnSelectedAbilityChanged(selectedAbility);
		}
		if (m_actor != null)
		{
			m_actor.OnSelectedAbilityChanged(selectedAbility);
		}
		if (CameraManager.Get() != null)
		{
			CameraManager.Get().OnSelectedAbilityChanged(selectedAbility);
		}
		if (selectedAbility != null)
		{
			m_lastSelectedAbility = selectedAbility;
		}
		Board.Get().MarkForUpdateValidSquares();
	}

	public Ability GetSelectedAbility()
	{
		return m_selectedAbility;
	}

	public void ClearSelectedAbility()
	{
		SetSelectedAbility(null);
	}

	public void SelectAbilityFromActionType(ActionType actionType)
	{
		SetSelectedAbility(GetAbilityOfActionType(actionType));
	}

	public ActionType GetSelectedActionType()
	{
		return GetActionTypeOfAbility(m_selectedAbility);
	}

	public void SetLastSelectedAbility(Ability ability)
	{
		m_lastSelectedAbility = ability;
	}

	public Ability GetLastSelectedAbility()
	{
		return m_lastSelectedAbility;
	}

	public void ClearLastSelectedAbility()
	{
		m_lastSelectedAbility = null;
	}

	public ActionType GetActionType(string abilityName)
	{
		ActionType actionType = ActionType.INVALID_ACTION;
		if (abilityName != null)
		{
			int num = 0;
			while (true)
			{
				if (num < 14)
				{
					if (m_abilities[num] != null)
					{
						if (m_abilities[num].ability != null)
						{
							if (m_abilities[num].ability.m_abilityName == abilityName)
							{
								actionType = (ActionType)num;
								break;
							}
						}
					}
					num++;
					continue;
				}
				break;
			}
			if (actionType == ActionType.INVALID_ACTION)
			{
				sbyte b = 0;
				while (true)
				{
					if (b < m_allChainAbilities.Count)
					{
						if (m_allChainAbilities[b] != null)
						{
							if (m_allChainAbilities[b].m_abilityName == abilityName)
							{
								sbyte b2 = b;
								b2 = checked((sbyte)(b2 + 10));
								actionType = (ActionType)b2;
								break;
							}
						}
						b = (sbyte)(b + 1);
						continue;
					}
					break;
				}
			}
		}
		return actionType;
	}

	public ActionType GetActionTypeOfAbilityOfType(Type abilityType)
	{
		Ability abilityOfType = GetAbilityOfType(abilityType);
		return GetActionTypeOfAbility(abilityOfType);
	}

	public ActionType GetActionTypeOfAbility(Ability ability)
	{
		ActionType actionType = ActionType.INVALID_ACTION;
		if (ability != null)
		{
			int num = 0;
			while (true)
			{
				if (num < 14)
				{
					if (m_abilities[num] != null && m_abilities[num].ability == ability)
					{
						actionType = (ActionType)num;
						break;
					}
					num++;
					continue;
				}
				break;
			}
			if (actionType == ActionType.INVALID_ACTION)
			{
				sbyte b = 0;
				while (true)
				{
					if (b < m_allChainAbilities.Count)
					{
						if (m_allChainAbilities[b] != null && m_allChainAbilities[b] == ability)
						{
							sbyte b2 = b;
							b2 = checked((sbyte)(b2 + 10));
							actionType = (ActionType)b2;
							break;
						}
						b = (sbyte)(b + 1);
						continue;
					}
					break;
				}
			}
		}
		return actionType;
	}

	public ActionType GetParentAbilityActionType(Ability ability)
	{
		ActionType actionType = GetActionTypeOfAbility(ability);
		if (IsChain(actionType))
		{
			int num = 0;
			while (true)
			{
				if (num < m_allChainAbilities.Count)
				{
					if (m_allChainAbilities[num] == ability)
					{
						actionType = m_allChainAbilityParentActionTypes[num];
						break;
					}
					num++;
					continue;
				}
				break;
			}
		}
		return actionType;
	}

	public Ability GetAbilityOfActionType(ActionType type)
	{
		if (IsChain(type))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					int num = (int)(type - 10);
					if (num >= 0)
					{
						if (num < m_allChainAbilities.Count)
						{
							return m_allChainAbilities[num];
						}
					}
					return null;
				}
				}
			}
		}
		if (type >= ActionType.ABILITY_0)
		{
			if ((int)type < m_abilities.Length)
			{
				return m_abilities[(int)type].ability;
			}
		}
		return null;
	}

	public Ability GetAbilityOfType(Type abilityType)
	{
		AbilityEntry[] abilities = m_abilities;
		foreach (AbilityEntry abilityEntry in abilities)
		{
			if (abilityEntry != null)
			{
				if (abilityEntry.ability != null && abilityEntry.ability.GetType() == abilityType)
				{
					return abilityEntry.ability;
				}
			}
		}
		while (true)
		{
			return null;
		}
	}

	public T GetAbilityOfType<T>() where T : Ability
	{
		AbilityEntry[] abilities = m_abilities;
		foreach (AbilityEntry abilityEntry in abilities)
		{
			if (abilityEntry == null || !(abilityEntry.ability != null))
			{
				continue;
			}
			if (abilityEntry.ability.GetType() != typeof(T))
			{
				continue;
			}
			while (true)
			{
				return abilityEntry.ability as T;
			}
		}
		while (true)
		{
			return (T)null;
		}
	}

	public AbilityEntry GetAbilityEntryOfActionType(ActionType type)
	{
		if (type >= ActionType.ABILITY_0)
		{
			if ((int)type < m_abilities.Length)
			{
				return m_abilities[(int)type];
			}
		}
		return null;
	}

	public static CardType GetCardTypeByActionType(CharacterCardInfo cardInfo, ActionType actionType)
	{
		if (actionType == ActionType.CARD_0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return cardInfo.PrepCard;
				}
			}
		}
		switch (actionType)
		{
		case ActionType.CARD_1:
			while (true)
			{
				return cardInfo.DashCard;
			}
		case ActionType.CARD_2:
			return cardInfo.CombatCard;
		default:
			return CardType.None;
		}
	}

	public List<Ability> GetCachedCardAbilities()
	{
		return m_cachedCardAbilities;
	}

	public bool IsAbilityAllowedByUnlockTurns(ActionType actionType)
	{
		int turnsTillUnlock = GetTurnsTillUnlock(actionType);
		return turnsTillUnlock <= 0;
	}

	public int GetTurnsTillUnlock(ActionType actionType)
	{
		if (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("NoCooldowns"))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return 0;
				}
			}
		}
		int b = 0;
		if (GameFlowData.Get() != null)
		{
			if (GameplayData.Get() != null)
			{
				int currentTurn = GameFlowData.Get().CurrentTurn;
				SpawnPointManager spawnPointManager = SpawnPointManager.Get();
				if (spawnPointManager != null)
				{
					if (spawnPointManager.m_spawnInDuringMovement && GameplayData.Get().m_disableAbilitiesOnRespawn)
					{
						if (m_actor.NextRespawnTurn == currentTurn)
						{
							b = 1;
							goto IL_0116;
						}
					}
				}
				if (IsCard(actionType))
				{
					int turnCatalystsUnlock = GameplayData.Get().m_turnCatalystsUnlock;
					b = turnCatalystsUnlock - currentTurn;
				}
				else
				{
					int[] turnsAbilitiesUnlock = GameplayData.Get().m_turnsAbilitiesUnlock;
					if ((int)actionType < turnsAbilitiesUnlock.Length)
					{
						b = turnsAbilitiesUnlock[(int)actionType] - currentTurn;
					}
				}
			}
		}
		goto IL_0116;
		IL_0116:
		return Mathf.Max(0, b);
	}

	public int GetCooldownRemaining(ActionType action)
	{
		if (IsChain(action))
		{
			return 0;
		}
		AbilityEntry abilityEntry = m_abilities[(int)action];
		return abilityEntry.GetCooldownRemaining();
	}

	public bool IsActionInCooldown(ActionType action)
	{
		if (IsChain(action))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		AbilityEntry abilityEntry = m_abilities[(int)action];
		return abilityEntry.GetCooldownRemaining() != 0;
	}

	public void TriggerCooldown(ActionType action)
	{
		if (IsChain(action))
		{
			return;
		}
		while (true)
		{
			AbilityEntry abilityEntry = m_abilities[(int)action];
			if (!(abilityEntry.ability != null))
			{
				return;
			}
			while (true)
			{
				int moddedCooldown = abilityEntry.ability.GetModdedCooldown();
				if (moddedCooldown > 0)
				{
					if (GameplayMutators.Get() != null)
					{
						int num = moddedCooldown + 1;
						int cooldownTimeAdjustment = GameplayMutators.GetCooldownTimeAdjustment();
						float cooldownMultiplier = GameplayMutators.GetCooldownMultiplier();
						int val = Mathf.RoundToInt((float)(num + cooldownTimeAdjustment) * cooldownMultiplier);
						val = Math.Max(val, 0);
						m_cooldowns[abilityEntry.ability.m_abilityName] = val;
					}
					else
					{
						m_cooldowns[abilityEntry.ability.m_abilityName] = moddedCooldown + 1;
					}
				}
				else if (abilityEntry.ability.m_cooldown == -1)
				{
					m_cooldowns[abilityEntry.ability.m_abilityName] = -1;
				}
				SynchronizeCooldownsToSlots();
				return;
			}
		}
	}

	public void OverrideCooldown(ActionType action, int cooldownRemainingOverride)
	{
		if (IsChain(action))
		{
			return;
		}
		while (true)
		{
			AbilityEntry abilityEntry = m_abilities[(int)action];
			if (abilityEntry.ability != null)
			{
				while (true)
				{
					m_cooldowns[abilityEntry.ability.m_abilityName] = cooldownRemainingOverride;
					SynchronizeCooldownsToSlots();
					return;
				}
			}
			return;
		}
	}

	public void ApplyCooldownReduction(ActionType action, int cooldownReduction)
	{
		if (cooldownReduction > 0)
		{
			int cooldownRemaining = GetCooldownRemaining(action);
			if (cooldownRemaining > 0)
			{
				cooldownRemaining -= cooldownReduction;
				cooldownRemaining = Mathf.Max(0, cooldownRemaining);
				OverrideCooldown(action, cooldownRemaining);
			}
		}
	}

	public void ProgressCooldowns()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>(m_cooldowns);
		using (Dictionary<string, int>.Enumerator enumerator = dictionary.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string key = enumerator.Current.Key;
				if (m_cooldowns[key] > 0)
				{
					int num = 1;
					if (GameplayMutators.Get() != null)
					{
						num += GameplayMutators.GetCooldownSpeedAdjustment();
						num = Mathf.Min(num, m_cooldowns[key]);
					}
					m_cooldowns[key] -= num;
					if (m_cooldowns[key] == 0)
					{
						m_cooldowns.Remove(key);
					}
				}
			}
		}
		SynchronizeCooldownsToSlots();
	}

	public void ProgressCooldownsOfAbilities(List<Ability> abilities)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>(m_cooldowns);
		using (Dictionary<string, int>.Enumerator enumerator = dictionary.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string key = enumerator.Current.Key;
				if (m_cooldowns[key] > 0)
				{
					bool flag = false;
					using (List<Ability>.Enumerator enumerator2 = abilities.GetEnumerator())
					{
						while (true)
						{
							if (!enumerator2.MoveNext())
							{
								break;
							}
							Ability current = enumerator2.Current;
							if (current.m_abilityName == key)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										break;
									default:
										flag = true;
										goto end_IL_0060;
									}
								}
							}
						}
						end_IL_0060:;
					}
					if (flag)
					{
						m_cooldowns[key]--;
						if (m_cooldowns[key] == 0)
						{
							m_cooldowns.Remove(key);
						}
					}
				}
			}
		}
		SynchronizeCooldownsToSlots();
	}

	public void ProgressCharacterAbilityCooldowns()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>(m_cooldowns);
		using (Dictionary<string, int>.Enumerator enumerator = dictionary.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string key = enumerator.Current.Key;
				ActionType actionType = GetActionType(key);
				if (actionType >= ActionType.ABILITY_0 && actionType <= ActionType.ABILITY_6)
				{
					if (m_cooldowns[key] > 0)
					{
						m_cooldowns[key]--;
						if (m_cooldowns[key] == 0)
						{
							m_cooldowns.Remove(key);
						}
					}
				}
			}
		}
		SynchronizeCooldownsToSlots();
	}

	[Command]
	internal void CmdClearCooldowns()
	{
		if (!HydrogenConfig.Get().AllowDebugCommands)
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
		ClearCooldowns();
	}

	public void ClearCooldowns()
	{
		m_cooldowns.Clear();
		SynchronizeCooldownsToSlots();
	}

	public void ClearCharacterAbilityCooldowns()
	{
		bool flag = false;
		for (int i = 0; i < 7; i++)
		{
			ActionType actionType = (ActionType)i;
			AbilityEntry abilityEntry = m_abilities[(int)actionType];
			if (!(abilityEntry.ability != null))
			{
				continue;
			}
			string abilityName = abilityEntry.ability.m_abilityName;
			if (m_cooldowns.ContainsKey(abilityName))
			{
				m_cooldowns.Remove(abilityName);
				flag = true;
			}
		}
		while (true)
		{
			if (flag)
			{
				while (true)
				{
					SynchronizeCooldownsToSlots();
					return;
				}
			}
			return;
		}
	}

	public void SetCooldown(ActionType action, int cooldown)
	{
		AbilityEntry abilityEntry = m_abilities[(int)action];
		if (abilityEntry.ability != null)
		{
			string abilityName = abilityEntry.ability.m_abilityName;
			if (m_cooldowns.ContainsKey(abilityName))
			{
				m_cooldowns[abilityName] = cooldown;
				SynchronizeCooldownsToSlots();
			}
		}
	}

	public void ClearCooldown(ActionType action)
	{
		AbilityEntry abilityEntry = m_abilities[(int)action];
		if (!(abilityEntry.ability != null))
		{
			return;
		}
		while (true)
		{
			string abilityName = abilityEntry.ability.m_abilityName;
			if (m_cooldowns.ContainsKey(abilityName))
			{
				while (true)
				{
					m_cooldowns.Remove(abilityName);
					SynchronizeCooldownsToSlots();
					return;
				}
			}
			return;
		}
	}

	public void PlaceInCooldownTillTurn(ActionType action, int turnNumber)
	{
		AbilityEntry abilityEntry = m_abilities[(int)action];
		if (!(abilityEntry.ability != null))
		{
			return;
		}
		int num = turnNumber - GameFlowData.Get().CurrentTurn;
		if (num <= 0)
		{
			return;
		}
		while (true)
		{
			m_cooldowns[abilityEntry.ability.m_abilityName] = num;
			SynchronizeCooldownsToSlots();
			return;
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
		for (int num = 0; num < m_abilities.Length; num++)
		{
			AbilityEntry abilityEntry = m_abilities[num];
			object obj;
			if (abilityEntry.ability == null)
			{
				obj = null;
			}
			else
			{
				obj = abilityEntry.ability.m_abilityName;
			}
			string key = (string)obj;
			int num2;
			if (abilityEntry.ability != null)
			{
				if (m_cooldowns.ContainsKey(key))
				{
					num2 = m_cooldowns[key];
					goto IL_009b;
				}
			}
			num2 = 0;
			goto IL_009b;
			IL_009b:
			if (abilityEntry.GetCooldownRemaining() != num2)
			{
				abilityEntry.SetCooldownRemaining(num2);
				if (m_cooldownsSync[num] != num2)
				{
					m_cooldownsSync[num] = num2;
				}
			}
		}
	}

	[Server]
	private void InitializeStockCounts()
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
					Debug.LogWarning("[Server] function 'System.Void AbilityData::InitializeStockCounts()' called on client");
					return;
				}
			}
		}
		for (int i = 0; i < 7; i++)
		{
			ActionType actionType = (ActionType)i;
			AbilityEntry abilityEntry = m_abilities[(int)actionType];
			if (!(abilityEntry.ability != null))
			{
				continue;
			}
			if (abilityEntry.ability.GetModdedMaxStocks() > 0)
			{
				if (abilityEntry.ability.m_initialStockAmount >= 0)
				{
					int desiredAmount = Mathf.Min(abilityEntry.ability.GetModdedMaxStocks(), abilityEntry.ability.m_initialStockAmount);
					OverrideStockRemaining(actionType, desiredAmount);
				}
			}
		}
	}

	public int GetMaxStocksCount(ActionType actionType)
	{
		AbilityEntry abilityEntry = m_abilities[(int)actionType];
		if (abilityEntry.ability != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return abilityEntry.ability.GetModdedMaxStocks();
				}
			}
		}
		return 0;
	}

	public int GetConsumedStocksCount(ActionType actionType)
	{
		return m_consumedStockCount[(int)actionType];
	}

	public int GetStocksRemaining(ActionType actionType)
	{
		return Mathf.Max(0, GetMaxStocksCount(actionType) - GetConsumedStocksCount(actionType));
	}

	public int GetStockRefreshCountdown(ActionType actionType)
	{
		return m_stockRefreshCountdowns[(int)actionType];
	}

	public bool ActionHasEnoughStockToTrigger(ActionType action)
	{
		if (!IsChain(action))
		{
			if (DebugParameters.Get() != null)
			{
				if (DebugParameters.Get().GetParameterAsBool("NoCooldowns"))
				{
					goto IL_0045;
				}
			}
			AbilityEntry abilityEntry = m_abilities[(int)action];
			bool flag = false;
			if (abilityEntry.ability.m_abilityManagedStockCount)
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
			int moddedMaxStocks = abilityEntry.ability.GetModdedMaxStocks();
			return moddedMaxStocks <= 0 || m_consumedStockCount[(int)action] < moddedMaxStocks;
		}
		goto IL_0045;
		IL_0045:
		return true;
	}

	public void ConsumeStock(ActionType action)
	{
		if (IsChain(action))
		{
			return;
		}
		while (true)
		{
			AbilityEntry abilityEntry = m_abilities[(int)action];
			if (!(abilityEntry.ability != null))
			{
				return;
			}
			while (true)
			{
				int moddedMaxStocks = abilityEntry.ability.GetModdedMaxStocks();
				if (moddedMaxStocks <= 0)
				{
					return;
				}
				while (true)
				{
					if (m_consumedStockCount[(int)action] >= moddedMaxStocks)
					{
						return;
					}
					if (m_consumedStockCount[(int)action] == 0)
					{
						int moddedStockRefreshDuration = abilityEntry.ability.GetModdedStockRefreshDuration();
						int num = moddedStockRefreshDuration + 1;
						if (m_stockRefreshCountdowns[(int)action] != num)
						{
							m_stockRefreshCountdowns[(int)action] = num;
						}
					}
					int num2 = Mathf.Clamp(m_consumedStockCount[(int)action] + abilityEntry.ability.m_stockConsumedOnCast, 0, moddedMaxStocks);
					if (m_consumedStockCount[(int)action] != num2)
					{
						while (true)
						{
							m_consumedStockCount[(int)action] = num2;
							return;
						}
					}
					return;
				}
			}
		}
	}

	[Command]
	internal void CmdRefillStocks()
	{
		if (!HydrogenConfig.Get().AllowDebugCommands)
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
		RefillStocks();
	}

	public void RefillStocks()
	{
		for (int i = 0; i < m_consumedStockCount.Count; i++)
		{
			if (m_consumedStockCount[i] != 0)
			{
				m_consumedStockCount[i] = 0;
			}
			if (m_stockRefreshCountdowns[i] != 0)
			{
				m_stockRefreshCountdowns[i] = 0;
			}
		}
	}

	public void ProgressStockRefreshTimes()
	{
		for (int i = 0; i < 14; i++)
		{
			ProgressStockRefreshTimeForAction((ActionType)i, 1);
		}
		while (true)
		{
			return;
		}
	}

	public void ProgressStockRefreshTimeForAction(ActionType action, int advanceAmount)
	{
		if ((int)action >= m_stockRefreshCountdowns.Count)
		{
			return;
		}
		while (true)
		{
			if (advanceAmount <= 0)
			{
				return;
			}
			while (true)
			{
				AbilityEntry abilityEntry = m_abilities[(int)action];
				if (m_stockRefreshCountdowns[(int)action] <= 0 && m_consumedStockCount[(int)action] <= 0)
				{
					return;
				}
				while (true)
				{
					if (abilityEntry == null)
					{
						return;
					}
					while (true)
					{
						if (!(abilityEntry.ability != null) || abilityEntry.ability.GetModdedStockRefreshDuration() <= 0)
						{
							return;
						}
						int moddedStockRefreshDuration = abilityEntry.ability.GetModdedStockRefreshDuration();
						int num = advanceAmount / moddedStockRefreshDuration;
						int num2 = advanceAmount % moddedStockRefreshDuration;
						int num3 = Mathf.Max(0, m_consumedStockCount[(int)action] - num);
						if (m_consumedStockCount[(int)action] != num3)
						{
							m_consumedStockCount[(int)action] = num3;
						}
						if (m_stockRefreshCountdowns[(int)action] >= num2)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									if (num2 != 0)
									{
										m_stockRefreshCountdowns[(int)action] -= num2;
									}
									if (m_stockRefreshCountdowns[(int)action] <= 0)
									{
										while (true)
										{
											switch (4)
											{
											case 0:
												break;
											default:
												if (m_consumedStockCount[(int)action] > 0)
												{
													while (true)
													{
														switch (2)
														{
														case 0:
															break;
														default:
															if (abilityEntry.ability.RefillAllStockOnRefresh())
															{
																m_consumedStockCount[(int)action] = 0;
															}
															else
															{
																m_consumedStockCount[(int)action]--;
															}
															if (m_consumedStockCount[(int)action] > 0)
															{
																while (true)
																{
																	switch (6)
																	{
																	case 0:
																		break;
																	default:
																	{
																		int num4 = moddedStockRefreshDuration;
																		if (m_stockRefreshCountdowns[(int)action] != num4)
																		{
																			m_stockRefreshCountdowns[(int)action] = num4;
																		}
																		return;
																	}
																	}
																}
															}
															if (m_stockRefreshCountdowns[(int)action] != 0)
															{
																while (true)
																{
																	switch (4)
																	{
																	case 0:
																		break;
																	default:
																		m_stockRefreshCountdowns[(int)action] = 0;
																		return;
																	}
																}
															}
															return;
														}
													}
												}
												return;
											}
										}
									}
									return;
								}
							}
						}
						int num5 = num2 - m_stockRefreshCountdowns[(int)action];
						int num6 = moddedStockRefreshDuration - num5;
						if (m_stockRefreshCountdowns[(int)action] != num6)
						{
							m_stockRefreshCountdowns[(int)action] = num6;
						}
						if (m_consumedStockCount[(int)action] <= 0)
						{
							return;
						}
						while (true)
						{
							if (abilityEntry.ability.RefillAllStockOnRefresh())
							{
								m_consumedStockCount[(int)action] = 0;
							}
							else
							{
								m_consumedStockCount[(int)action]--;
							}
							return;
						}
					}
				}
			}
		}
	}

	public void OverrideStockRemaining(ActionType action, int desiredAmount)
	{
		if (IsChain(action))
		{
			return;
		}
		while (true)
		{
			AbilityEntry abilityEntry = m_abilities[(int)action];
			if (!(abilityEntry.ability != null))
			{
				return;
			}
			while (true)
			{
				int value = Mathf.Max(0, abilityEntry.ability.GetModdedMaxStocks() - desiredAmount);
				m_consumedStockCount[(int)action] = value;
				if (m_consumedStockCount[(int)action] != 0)
				{
					return;
				}
				while (true)
				{
					if (m_stockRefreshCountdowns[(int)action] != 0)
					{
						while (true)
						{
							m_stockRefreshCountdowns[(int)action] = 0;
							return;
						}
					}
					return;
				}
			}
		}
	}

	public void OverrideStockRefreshCountdown(ActionType action, int desiredCountdown)
	{
		if (IsChain(action))
		{
			return;
		}
		AbilityEntry abilityEntry = m_abilities[(int)action];
		if (!(abilityEntry.ability != null))
		{
			return;
		}
		while (true)
		{
			int num = Mathf.Clamp(desiredCountdown, 0, abilityEntry.ability.GetModdedStockRefreshDuration());
			if (m_stockRefreshCountdowns[(int)action] != num)
			{
				m_stockRefreshCountdowns[(int)action] = num;
			}
			return;
		}
	}

	public int GetStockRefreshDurationForAbility(ActionType action)
	{
		if (!IsChain(action))
		{
			AbilityEntry abilityEntry = m_abilities[(int)action];
			if (abilityEntry.ability != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return abilityEntry.ability.GetModdedStockRefreshDuration();
					}
				}
			}
		}
		return 0;
	}

	public bool IsAbilityTargetInRange(Ability ability, AbilityTarget target, int targetIndex, float calculatedMinRangeInSquares = -1f, float calculatedMaxRangeInSquares = -1f)
	{
		bool flag = false;
		ActorData actor = m_actor;
		BoardSquare currentBoardSquare = actor.GetCurrentBoardSquare();
		float num = calculatedMaxRangeInSquares;
		float num2 = calculatedMinRangeInSquares;
		if (num < 0f)
		{
			num = AbilityUtils.GetCurrentRangeInSquares(ability, actor, targetIndex);
		}
		if (num2 < 0f)
		{
			num2 = AbilityUtils.GetCurrentMinRangeInSquares(ability, actor, targetIndex);
		}
		Ability.TargetingParadigm targetingParadigm = ability.GetTargetingParadigm(targetIndex);
		if (targetingParadigm == Ability.TargetingParadigm.BoardSquare)
		{
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
			return flag | IsTargetSquareInRangeOfAbilityFromSquare(boardSquareSafe, currentBoardSquare, num, num2);
		}
		if (targetingParadigm == Ability.TargetingParadigm.Position)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					Vector3 travelBoardSquareWorldPositionForLos = actor.GetTravelBoardSquareWorldPositionForLos();
					float num3 = num * Board.Get().squareSize;
					float num4 = num2 * Board.Get().squareSize;
					if (GameplayData.Get().m_abilityRangeType == GameplayData.AbilityRangeType.WorldDistToFreePos)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
							{
								Vector3 vector = target.FreePos - travelBoardSquareWorldPositionForLos;
								vector.y = 0f;
								float sqrMagnitude = vector.sqrMagnitude;
								int result;
								if (sqrMagnitude <= num3 * num3)
								{
									result = ((sqrMagnitude >= num4 * num4) ? 1 : 0);
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
					BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(target.GridPos);
					return flag | IsTargetSquareInRangeOfAbilityFromSquare(boardSquareSafe2, currentBoardSquare, num, num2);
				}
				}
			}
		}
		if (targetingParadigm == Ability.TargetingParadigm.Direction)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		Log.Error("Checking range for ability " + ability.m_abilityName + ", but its targeting paradigm is invalid.");
		return false;
	}

	public bool IsTargetSquareInRangeOfAbilityFromSquare(BoardSquare dest, BoardSquare src, float rangeInSquares, float minRangeInSquares)
	{
		bool result = true;
		if ((bool)src)
		{
			if ((bool)dest)
			{
				float num;
				if (GameplayData.Get().m_abilityRangeType == GameplayData.AbilityRangeType.WorldDistToFreePos)
				{
					num = src.HorizontalDistanceInSquaresTo(dest);
				}
				else
				{
					num = src.HorizontalDistanceOnBoardTo(dest);
				}
				int num2;
				if (!(rangeInSquares < 0f))
				{
					num2 = ((num <= rangeInSquares) ? 1 : 0);
				}
				else
				{
					num2 = 1;
				}
				bool flag = (byte)num2 != 0;
				bool flag2 = num >= minRangeInSquares;
				if (flag)
				{
					if (flag2)
					{
						goto IL_00a7;
					}
				}
				result = false;
			}
		}
		goto IL_00a7;
		IL_00a7:
		return result;
	}

	public bool HasLineOfSightToTarget(Ability specificAbility, AbilityTarget target, int targetIndex)
	{
		bool result = false;
		ActorData actor = m_actor;
		Ability.TargetingParadigm targetingParadigm = specificAbility.GetTargetingParadigm(targetIndex);
		if (actor.GetCurrentBoardSquare() == null)
		{
			result = false;
		}
		else
		{
			if (targetingParadigm != Ability.TargetingParadigm.BoardSquare)
			{
				if (targetingParadigm != Ability.TargetingParadigm.Position)
				{
					if (targetingParadigm == Ability.TargetingParadigm.Direction)
					{
						result = true;
					}
					goto IL_00d0;
				}
			}
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
			ReadOnlyCollection<BoardSquare> lineOfSightVisibleExceptionSquares = actor.LineOfSightVisibleExceptionSquares;
			if (lineOfSightVisibleExceptionSquares.Contains(boardSquareSafe))
			{
				result = true;
			}
			else if (actor.GetCurrentBoardSquare()._0013(target.GridPos.x, target.GridPos.y))
			{
				result = true;
			}
		}
		goto IL_00d0;
		IL_00d0:
		return result;
	}

	public bool HasLineOfSightToActor(ActorData target, bool ignoreExceptions = false)
	{
		ActorData actor = m_actor;
		if (!(target == null))
		{
			if (!(actor == null))
			{
				if (!ignoreExceptions)
				{
					if (actor.IsLineOfSightVisibleException(target))
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
				}
				BoardSquare currentBoardSquare = target.GetCurrentBoardSquare();
				BoardSquare currentBoardSquare2 = actor.GetCurrentBoardSquare();
				return currentBoardSquare2._0013(currentBoardSquare.x, currentBoardSquare.y);
			}
		}
		return false;
	}

	public bool ValidateAbilityOnTarget(Ability ability, AbilityTarget target, int targetIndex, float calculatedMinRangeInSquares = -1f, float calculatedMaxRangeInSquares = -1f)
	{
		bool result = false;
		ActorTurnSM actorTurnSM = m_actor.GetActorTurnSM();
		if ((bool)actorTurnSM)
		{
			List<AbilityTarget> abilityTargets = actorTurnSM.GetAbilityTargets();
			if (abilityTargets != null)
			{
				int num;
				if (targetIndex <= abilityTargets.Count)
				{
					num = (ValidateAbilityOnTarget(ability, target, targetIndex, abilityTargets, calculatedMinRangeInSquares, calculatedMaxRangeInSquares) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				result = ((byte)num != 0);
			}
		}
		return result;
	}

	public bool ValidateAbilityOnTarget(Ability ability, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets, float calculatedMinRangeInSquares = -1f, float calculatedMaxRangeInSquares = -1f)
	{
		if (target == null)
		{
			while (true)
			{
				return false;
			}
		}
		ActorData actor = m_actor;
		bool flag = true;
		Ability.TargetingParadigm targetingParadigm = ability.GetTargetingParadigm(targetIndex);
		if (targetingParadigm != Ability.TargetingParadigm.BoardSquare)
		{
			if (targetingParadigm != Ability.TargetingParadigm.Position)
			{
				goto IL_00d8;
			}
		}
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		int num;
		if (boardSquareSafe != null)
		{
			num = ((ability.AllowInvalidSquareForSquareBasedTarget() || Board.Get().GetBoardSquareSafe(target.GridPos).IsBaselineHeight()) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		flag = ((byte)num != 0);
		if (!ability.IsSimpleAction())
		{
			if (flag && BarrierManager.Get().IsPositionTargetingBlocked(actor, boardSquareSafe))
			{
				flag = false;
			}
			if (!flag)
			{
				return false;
			}
		}
		goto IL_00d8;
		IL_01de:
		int num2;
		int result;
		if (num2 != 0 && ValidateAbilityIsCastableDisregardingMovement(ability))
		{
			result = (ability.CustomTargetValidation(actor, target, targetIndex, currentTargets) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
		IL_00d8:
		int num3;
		if (!ability.IsSimpleAction())
		{
			num3 = (IsAbilityTargetInRange(ability, target, targetIndex, calculatedMinRangeInSquares, calculatedMaxRangeInSquares) ? 1 : 0);
		}
		else
		{
			num3 = 1;
		}
		bool flag2 = (byte)num3 != 0;
		if (!flag2)
		{
			while (true)
			{
				return false;
			}
		}
		bool flag3 = true;
		if (AbilityUtils.AbilityHasTag(ability, AbilityTags.ValidOnlyWhereInCoverToCaster))
		{
			BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(target.GridPos);
			flag3 = ActorCover.IsInCoverWrt(actor.GetTravelBoardSquareWorldPosition(), boardSquareSafe2, null, null, null);
		}
		else if (AbilityUtils.AbilityHasTag(ability, AbilityTags.ValidOnlyWhereInCover))
		{
			BoardSquare boardSquareSafe3 = Board.Get().GetBoardSquareSafe(target.GridPos);
			flag3 = ActorCover.CalcCoverLevelGeoOnly(out bool[] _, boardSquareSafe3);
		}
		bool flag4 = true;
		if (ability.GetCheckLoS(targetIndex) && !ability.IsSimpleAction())
		{
			if (flag2)
			{
				flag4 = HasLineOfSightToTarget(ability, target, targetIndex);
			}
			else
			{
				flag4 = false;
			}
		}
		if (flag2)
		{
			if (flag && flag3)
			{
				num2 = (flag4 ? 1 : 0);
				goto IL_01de;
			}
		}
		num2 = 0;
		goto IL_01de;
	}

	public bool ValidateActionRequest(ActionType actionType, List<AbilityTarget> targets)
	{
		bool result = true;
		Ability abilityOfActionType = GetAbilityOfActionType(actionType);
		if (!ValidateActionIsRequestable(actionType))
		{
			result = false;
		}
		else
		{
			int num = 0;
			while (true)
			{
				if (num < targets.Count)
				{
					AbilityTarget target = targets[num];
					if (!ValidateAbilityOnTarget(abilityOfActionType, target, num, targets))
					{
						result = false;
						break;
					}
					num++;
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
			while (true)
			{
				int result;
				switch (4)
				{
				case 0:
					break;
				default:
					{
						bool flag = ValidateAbilityIsCastableDisregardingMovement(ability);
						bool flag2 = ability.GetMovementAdjustment() != Ability.MovementAdjustment.NoMovement || !m_actor.HasQueuedMovement();
						int num;
						if (!m_actor.QueuedMovementAllowsAbility)
						{
							num = ((!ability.GetAffectsMovement()) ? 1 : 0);
						}
						else
						{
							num = 1;
						}
						bool flag3 = (byte)num != 0;
						bool flag4 = ability.RunPriority != AbilityPriority.Evasion || !HasQueuedAbilityInPhase(AbilityPriority.Evasion);
						int num2;
						if (IsCard(GetActionTypeOfAbility(ability)))
						{
							num2 = ((!HasQueuedCardAbility()) ? 1 : 0);
						}
						else
						{
							num2 = 1;
						}
						bool flag5 = (byte)num2 != 0;
						if (flag)
						{
							if (flag2)
							{
								if (flag3)
								{
									if (flag4)
									{
										result = (flag5 ? 1 : 0);
										goto IL_00e0;
									}
								}
							}
						}
						result = 0;
						goto IL_00e0;
					}
					IL_00e0:
					return (byte)result != 0;
				}
			}
		}
		Log.Error("Actor " + m_actor.DisplayName + " calling ValidateAbilityIsCastable on a null ability.");
		return false;
	}

	public bool ValidateActionIsRequestable(ActionType abilityAction)
	{
		Ability abilityOfActionType = GetAbilityOfActionType(abilityAction);
		int result;
		if (abilityOfActionType != null)
		{
			bool flag = ValidateActionIsRequestableDisregardingQueuedActions(abilityAction);
			bool flag2 = !HasQueuedAction(abilityAction);
			int num;
			if (!abilityOfActionType.IsFreeAction())
			{
				num = ((GetActionCostOfQueuedAbilities(abilityAction) == 0) ? 1 : 0);
			}
			else
			{
				num = 1;
			}
			bool flag3 = (byte)num != 0;
			int num2;
			if (abilityOfActionType.GetMovementAdjustment() == Ability.MovementAdjustment.NoMovement)
			{
				num2 = ((!m_actor.HasQueuedMovement()) ? 1 : 0);
			}
			else
			{
				num2 = 1;
			}
			bool flag4 = (byte)num2 != 0;
			bool flag5 = m_actor.QueuedMovementAllowsAbility || !abilityOfActionType.GetAffectsMovement();
			int num3;
			if (abilityOfActionType.RunPriority == AbilityPriority.Evasion)
			{
				num3 = ((!HasQueuedAbilityInPhase(AbilityPriority.Evasion)) ? 1 : 0);
			}
			else
			{
				num3 = 1;
			}
			bool flag6 = (byte)num3 != 0;
			bool flag7 = !IsCard(abilityAction) || !HasQueuedCardAbility();
			if (flag)
			{
				if (flag2)
				{
					if (flag3)
					{
						if (flag4)
						{
							if (flag5)
							{
								if (flag6)
								{
									result = (flag7 ? 1 : 0);
									goto IL_0139;
								}
							}
						}
					}
				}
			}
			result = 0;
			goto IL_0139;
		}
		Log.Error("Actor " + m_actor.DisplayName + " calling ValidateActionIsRequestable on a null ability.");
		return false;
		IL_0139:
		return (byte)result != 0;
	}

	public bool ValidateAbilityIsCastableDisregardingMovement(Ability ability)
	{
		ActorData actor = m_actor;
		if (ability != null)
		{
			while (true)
			{
				int num2;
				int result;
				switch (3)
				{
				case 0:
					break;
				default:
					{
						ActionType actionTypeOfAbility = GetActionTypeOfAbility(ability);
						bool flag = actionTypeOfAbility != ActionType.INVALID_ACTION;
						bool flag2 = !actor.IsDead();
						bool flag3 = actor.TechPoints >= ability.GetModdedCost();
						int num;
						if (AbilityUtils.AbilityHasTag(ability, AbilityTags.ValidOnlyWhenOutOfCombat))
						{
							num = (actor.OutOfCombat ? 1 : 0);
						}
						else
						{
							num = 1;
						}
						bool flag4 = (byte)num != 0;
						bool flag5 = !AbilityUtils.AbilityHasTag(ability, AbilityTags.ValidOnlyWhenInCombat) || !actor.OutOfCombat;
						bool flag6 = !actor.GetActorStatus().IsActionSilenced(actionTypeOfAbility);
						if (flag)
						{
							if (flag2)
							{
								if (flag3 && flag4)
								{
									if (flag5)
									{
										num2 = (flag6 ? 1 : 0);
										goto IL_00eb;
									}
								}
							}
						}
						num2 = 0;
						goto IL_00eb;
					}
					IL_00eb:
					if (num2 != 0)
					{
						result = (ability.CustomCanCastValidation(actor) ? 1 : 0);
					}
					else
					{
						result = 0;
					}
					return (byte)result != 0;
				}
			}
		}
		Log.Error("Actor " + actor.DisplayName + " calling ValidateAbilityIsCastableDisregardingMovement on a null ability.");
		return false;
	}

	public bool ValidateActionIsRequestableDisregardingQueuedActions(ActionType abilityAction)
	{
		Ability abilityOfActionType = GetAbilityOfActionType(abilityAction);
		if (abilityOfActionType != null)
		{
			while (true)
			{
				int result;
				switch (3)
				{
				case 0:
					break;
				default:
					{
						ActorData actor = m_actor;
						int num;
						if (IsActionInCooldown(abilityAction))
						{
							num = ((abilityOfActionType.GetModdedMaxStocks() > 0) ? 1 : 0);
						}
						else
						{
							num = 1;
						}
						bool flag = (byte)num != 0;
						if (!flag)
						{
							if (actor != null)
							{
								if (AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.IgnoreCooldownIfFullEnergy))
								{
									flag = (actor.TechPoints + actor.ReservedTechPoints >= actor.GetActualMaxTechPoints());
								}
							}
						}
						bool flag2 = ActionHasEnoughStockToTrigger(abilityAction);
						bool flag3 = IsAbilityAllowedByUnlockTurns(abilityAction);
						bool flag4 = ValidateAbilityIsCastableDisregardingMovement(abilityOfActionType);
						bool flag5 = true;
						flag5 = SinglePlayerManager.IsActionAllowed(actor, abilityAction);
						if (flag && flag2)
						{
							if (flag3 && flag4)
							{
								result = (flag5 ? 1 : 0);
								goto IL_00fc;
							}
						}
						result = 0;
						goto IL_00fc;
					}
					IL_00fc:
					return (byte)result != 0;
				}
			}
		}
		Log.Error("Actor " + m_actor.DisplayName + " calling ValidateActionIsRequestableDisregardingQueuedActions on a null ability.");
		return false;
	}

	public BoardSquare GetAutoSelectTarget()
	{
		BoardSquare result = null;
		if ((bool)m_selectedAbility)
		{
			if (m_selectedAbility.IsAutoSelect())
			{
				result = m_actor.GetCurrentBoardSquare();
			}
		}
		return result;
	}

	public bool HasQueuedAbilities()
	{
		bool result = false;
		ActorTeamSensitiveData teamSensitiveData_authority = m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
		{
			for (int i = 0; i < 14; i++)
			{
				if (teamSensitiveData_authority.HasQueuedAction(i))
				{
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
		ActorTeamSensitiveData teamSensitiveData_authority = m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
		{
			int num = 0;
			while (true)
			{
				if (num < 14)
				{
					if (teamSensitiveData_authority.HasQueuedAction(num))
					{
						if (IsCard((ActionType)num))
						{
							result = true;
							break;
						}
					}
					num++;
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
		ActorTeamSensitiveData teamSensitiveData_authority = m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
		{
			for (int i = 0; i < 14; i++)
			{
				if (teamSensitiveData_authority.HasQueuedAction(i))
				{
					num++;
				}
			}
		}
		return num;
	}

	public Ability.MovementAdjustment GetQueuedAbilitiesMovementAdjustType()
	{
		Ability.MovementAdjustment movementAdjustment = Ability.MovementAdjustment.FullMovement;
		ActorTeamSensitiveData teamSensitiveData_authority = m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
		{
			for (int i = 0; i < 14; i++)
			{
				ActionType actionType = (ActionType)i;
				if (!teamSensitiveData_authority.HasQueuedAction(actionType))
				{
					continue;
				}
				Ability abilityOfActionType = GetAbilityOfActionType(actionType);
				if (abilityOfActionType != null)
				{
					if (abilityOfActionType.GetMovementAdjustment() > movementAdjustment)
					{
						movementAdjustment = abilityOfActionType.GetMovementAdjustment();
					}
				}
			}
		}
		SpawnPointManager spawnPointManager = SpawnPointManager.Get();
		if (spawnPointManager != null)
		{
			if (spawnPointManager.m_spawnInDuringMovement)
			{
				if (m_actor.NextRespawnTurn == GameFlowData.Get().CurrentTurn)
				{
					if (GameplayData.Get().m_movementAllowedOnRespawn < movementAdjustment)
					{
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
		Ability.MovementAdjustment queuedAbilitiesMovementAdjustType = GetQueuedAbilitiesMovementAdjustType();
		if (queuedAbilitiesMovementAdjustType == Ability.MovementAdjustment.ReducedMovement)
		{
			result = -1f * m_actor.GetPostAbilityHorizontalMovementChange();
		}
		return result;
	}

	public List<StatusType> GetQueuedAbilitiesOnRequestStatuses()
	{
		List<StatusType> list = new List<StatusType>();
		ActorTeamSensitiveData teamSensitiveData_authority = m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
		{
			for (int i = 0; i < 14; i++)
			{
				ActionType actionType = (ActionType)i;
				if (!teamSensitiveData_authority.HasQueuedAction(actionType))
				{
					continue;
				}
				Ability abilityOfActionType = GetAbilityOfActionType(actionType);
				if (abilityOfActionType != null)
				{
					list.AddRange(abilityOfActionType.GetStatusToApplyWhenRequested());
				}
			}
		}
		return list;
	}

	public bool HasPendingStatusFromQueuedAbilities(StatusType status)
	{
		ActorTeamSensitiveData teamSensitiveData_authority = m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					for (int i = 0; i < 14; i++)
					{
						ActionType actionType = (ActionType)i;
						if (teamSensitiveData_authority.HasQueuedAction(actionType))
						{
							Ability abilityOfActionType = GetAbilityOfActionType(actionType);
							if (abilityOfActionType != null && abilityOfActionType.GetStatusToApplyWhenRequested().Contains(status))
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										break;
									default:
										return true;
									}
								}
							}
						}
					}
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
				}
			}
		}
		return false;
	}

	public bool GetQueuedAbilitiesAllowSprinting()
	{
		bool result = true;
		ActorTeamSensitiveData teamSensitiveData_authority = m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
		{
			for (int i = 0; i < 14; i++)
			{
				ActionType actionType = (ActionType)i;
				if (!teamSensitiveData_authority.HasQueuedAction(actionType))
				{
					continue;
				}
				Ability abilityOfActionType = GetAbilityOfActionType(actionType);
				if (!(abilityOfActionType != null))
				{
					continue;
				}
				if (abilityOfActionType.GetAffectsMovement())
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}

	public bool GetQueuedAbilitiesAllowMovement()
	{
		bool result = true;
		ActorTeamSensitiveData teamSensitiveData_authority = m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
		{
			int num = 0;
			while (true)
			{
				if (num < 14)
				{
					ActionType actionType = (ActionType)num;
					if (teamSensitiveData_authority.HasQueuedAction(actionType))
					{
						Ability abilityOfActionType = GetAbilityOfActionType(actionType);
						if (abilityOfActionType != null)
						{
							if (abilityOfActionType.GetPreventsMovement())
							{
								result = false;
								break;
							}
						}
					}
					num++;
					continue;
				}
				break;
			}
		}
		return result;
	}

	public int GetActionCostOfQueuedAbilities(ActionType actionToSkip = ActionType.INVALID_ACTION)
	{
		int num = 0;
		ActorTeamSensitiveData teamSensitiveData_authority = m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
		{
			for (int i = 0; i < 14; i++)
			{
				ActionType actionType = (ActionType)i;
				if (!teamSensitiveData_authority.HasQueuedAction(actionType))
				{
					continue;
				}
				if (actionType == actionToSkip)
				{
					continue;
				}
				Ability abilityOfActionType = GetAbilityOfActionType(actionType);
				if (abilityOfActionType != null)
				{
					if (!abilityOfActionType.IsFreeAction())
					{
						num++;
					}
				}
			}
		}
		return num;
	}

	private Card GetSpawnedCardInstance(CardType cardType)
	{
		if (m_cardTypeToCardInstance.ContainsKey(cardType))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_cardTypeToCardInstance[cardType];
				}
			}
		}
		GameObject cardPrefab = CardManagerData.Get().GetCardPrefab(cardType);
		if (cardPrefab != null)
		{
			while (true)
			{
				Card component;
				switch (6)
				{
				case 0:
					break;
				default:
					{
						GameObject gameObject = UnityEngine.Object.Instantiate(cardPrefab);
						component = gameObject.GetComponent<Card>();
						if (component != null)
						{
							if (component.m_useAbility != null)
							{
								gameObject.transform.parent = base.gameObject.transform;
								m_cardTypeToCardInstance[cardType] = component;
								component.m_useAbility.OverrideActorDataIndex(m_actor.ActorIndex);
								goto IL_0100;
							}
						}
						Log.Error("Card prefab " + cardPrefab.name + " does not have Card component");
						UnityEngine.Object.Destroy(gameObject);
						goto IL_0100;
					}
					IL_0100:
					return component;
				}
			}
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
			keyPreference = KeyPreference.Card3;
		}
		m_abilities[num].Setup(useAbility, keyPreference);
		if (cardSlotIndex < m_cachedCardAbilities.Count)
		{
			m_cachedCardAbilities[cardSlotIndex] = useAbility;
		}
		if (!NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			SynchronizeCooldownsToSlots();
			return;
		}
	}

	public void SpawnAndSetupCardsOnReconnect()
	{
		if (NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < m_currentCardIds.Count; i++)
			{
				Card spawnedCardInstance = GetSpawnedCardInstance((CardType)m_currentCardIds[i]);
				int cardSlotIndex = i;
				object useAbility;
				if (spawnedCardInstance != null)
				{
					useAbility = spawnedCardInstance.m_useAbility;
				}
				else
				{
					useAbility = null;
				}
				SetupCardAbility(cardSlotIndex, (Ability)useAbility);
			}
			UpdateCardBarUI();
			return;
		}
	}

	public IEnumerable<Card> GetActiveCards()
	{
		int i = 0;
		if (i < m_currentCardIds.Count)
		{
			yield return GetSpawnedCardInstance((CardType)m_currentCardIds[i]);
			/*Error: Unable to find new state assignment for yield return*/;
		}
		while (true)
		{
			switch (4)
			{
			default:
				yield break;
			case 0:
				break;
			}
		}
	}

	public void UpdateCatalystDisplay()
	{
		if (!(HUD_UI.Get() != null))
		{
			return;
		}
		while (true)
		{
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateCatalysts(m_actor, m_cachedCardAbilities);
			HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.UpdateCatalysts(m_actor, m_cachedCardAbilities);
			return;
		}
	}

	private void UpdateCardBarUI()
	{
		if (NetworkClient.active && m_actor == GameFlowData.Get().activeOwnedActorData && HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_cardBar.Rebuild();
		}
		UpdateCatalystDisplay();
	}

	public float GetTargetableRadius(int actionTypeInt, ActorData caster)
	{
		if (actionTypeInt < m_abilities.Length)
		{
			if (m_abilities[actionTypeInt].ability != null)
			{
				if (m_abilities[actionTypeInt].ability.CanShowTargetableRadiusPreview())
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							return m_abilities[actionTypeInt].ability.GetTargetableRadiusInSquares(caster);
						}
					}
				}
			}
		}
		return 0f;
	}

	public void OnClientCombatPhasePlayDataReceived(List<ClientResolutionAction> resolutionActions)
	{
		for (int i = 0; i <= 4; i++)
		{
			if (m_abilities[i] != null)
			{
				if (m_abilities[i].ability != null)
				{
					m_abilities[i].ability.OnClientCombatPhasePlayDataReceived(resolutionActions, m_actor);
				}
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

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			return;
		}
		if (m_abilities == null)
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
		AbilityEntry[] abilities = m_abilities;
		foreach (AbilityEntry abilityEntry in abilities)
		{
			if (abilityEntry == null)
			{
				continue;
			}
			if (abilityEntry.ability != null)
			{
				abilityEntry.ability.DrawGizmos();
			}
		}
		while (true)
		{
			DrawBoardVisibilityGizmos();
			return;
		}
	}

	private void DrawBoardVisibilityGizmos()
	{
		if (DebugParameters.Get() == null || !DebugParameters.Get().GetParameterAsBool("ShowBoardSquareVisGizmo"))
		{
			return;
		}
		while (true)
		{
			BoardSquare playerFreeSquare = Board.Get().PlayerFreeSquare;
			if (playerFreeSquare != null)
			{
				for (int i = 0; i < 15; i++)
				{
					List<BoardSquare> squaresInBorderLayer = AreaEffectUtils.GetSquaresInBorderLayer(playerFreeSquare, i, false);
					using (List<BoardSquare>.Enumerator enumerator = squaresInBorderLayer.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							BoardSquare current = enumerator.Current;
							if (current.IsBaselineHeight())
							{
								if (playerFreeSquare._0013(current.x, current.y))
								{
									Color white = Color.white;
									white.a = 0.5f;
									Gizmos.color = white;
									Vector3 size = 0.05f * Board.Get().squareSize * Vector3.one;
									size.y = 0.1f;
									Gizmos.DrawWireCube(current.ToVector3(), size);
								}
								else
								{
									Color red = Color.red;
									red.a = 0.5f;
									Gizmos.color = red;
									Vector3 size2 = 0.05f * Board.Get().squareSize * Vector3.one;
									size2.y = 0.1f;
									Gizmos.DrawWireCube(current.ToVector3(), size2);
								}
							}
						}
					}
				}
			}
			return;
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_cooldownsSync(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_cooldownsSync called on server.");
		}
		else
		{
			((AbilityData)obj).m_cooldownsSync.HandleMsg(reader);
		}
	}

	protected static void InvokeSyncListm_consumedStockCount(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Debug.LogError("SyncList m_consumedStockCount called on server.");
					return;
				}
			}
		}
		((AbilityData)obj).m_consumedStockCount.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_stockRefreshCountdowns(NetworkBehaviour obj, NetworkReader reader)
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
					Debug.LogError("SyncList m_stockRefreshCountdowns called on server.");
					return;
				}
			}
		}
		((AbilityData)obj).m_stockRefreshCountdowns.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_currentCardIds(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_currentCardIds called on server.");
		}
		else
		{
			((AbilityData)obj).m_currentCardIds.HandleMsg(reader);
		}
	}

	protected static void InvokeCmdCmdClearCooldowns(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdClearCooldowns called on client.");
		}
		else
		{
			((AbilityData)obj).CmdClearCooldowns();
		}
	}

	protected static void InvokeCmdCmdRefillStocks(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command CmdRefillStocks called on client.");
					return;
				}
			}
		}
		((AbilityData)obj).CmdRefillStocks();
	}

	public void CallCmdClearCooldowns()
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command function CmdClearCooldowns called on server.");
					return;
				}
			}
		}
		if (base.isServer)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					CmdClearCooldowns();
					return;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdClearCooldowns);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, "CmdClearCooldowns");
	}

	public void CallCmdRefillStocks()
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command function CmdRefillStocks called on server.");
					return;
				}
			}
		}
		if (base.isServer)
		{
			CmdRefillStocks();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdRefillStocks);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, "CmdRefillStocks");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					SyncListInt.WriteInstance(writer, m_cooldownsSync);
					SyncListInt.WriteInstance(writer, m_consumedStockCount);
					SyncListInt.WriteInstance(writer, m_stockRefreshCountdowns);
					SyncListInt.WriteInstance(writer, m_currentCardIds);
					writer.Write((int)m_selectedActionForTargeting);
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
			SyncListInt.WriteInstance(writer, m_cooldownsSync);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, m_consumedStockCount);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, m_stockRefreshCountdowns);
		}
		if ((base.syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, m_currentCardIds);
		}
		if ((base.syncVarDirtyBits & 0x10) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)m_selectedActionForTargeting);
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
				switch (7)
				{
				case 0:
					break;
				default:
					SyncListInt.ReadReference(reader, m_cooldownsSync);
					SyncListInt.ReadReference(reader, m_consumedStockCount);
					SyncListInt.ReadReference(reader, m_stockRefreshCountdowns);
					SyncListInt.ReadReference(reader, m_currentCardIds);
					m_selectedActionForTargeting = (ActionType)reader.ReadInt32();
					return;
				}
			}
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListInt.ReadReference(reader, m_cooldownsSync);
		}
		if ((num & 2) != 0)
		{
			SyncListInt.ReadReference(reader, m_consumedStockCount);
		}
		if ((num & 4) != 0)
		{
			SyncListInt.ReadReference(reader, m_stockRefreshCountdowns);
		}
		if ((num & 8) != 0)
		{
			SyncListInt.ReadReference(reader, m_currentCardIds);
		}
		if ((num & 0x10) == 0)
		{
			return;
		}
		while (true)
		{
			m_selectedActionForTargeting = (ActionType)reader.ReadInt32();
			return;
		}
	}
}
