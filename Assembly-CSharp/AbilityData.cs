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
	public string m_sequenceDirNameOverride = "";

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

	public static Color s_freeActionTextColor = new Color(0f, 1f, 0f);
	public static float s_heightPerButton = 75f;
	public static float s_heightFromBottom = 75f;
	public static float s_widthPerButton = 64f;
	public static int s_abilityButtonSlots = 8;
	public static float s_widthOfAllButtons = s_widthPerButton * s_abilityButtonSlots;

	private Ability m_lastSelectedAbility;

	private static int kListm_cooldownsSync = -0x6512D205;
	private static int kListm_consumedStockCount = 0x52CC1FC9;
	private static int kListm_stockRefreshCountdowns = -0x3C9C58B1;
	private static int kListm_currentCardIds = 0x16E4E7F7;
	private static int kCmdCmdClearCooldowns = 0x228F4CF;
	private static int kCmdCmdRefillStocks = 0x42186527;

	static AbilityData()
	{
		RegisterCommandDelegate(typeof(AbilityData), kCmdCmdClearCooldowns, new CmdDelegate(InvokeCmdCmdClearCooldowns));
		RegisterCommandDelegate(typeof(AbilityData), kCmdCmdRefillStocks, new CmdDelegate(InvokeCmdCmdRefillStocks));
		RegisterSyncListDelegate(typeof(AbilityData), kListm_cooldownsSync, new CmdDelegate(InvokeSyncListm_cooldownsSync));
		RegisterSyncListDelegate(typeof(AbilityData), kListm_consumedStockCount, new CmdDelegate(InvokeSyncListm_consumedStockCount));
		RegisterSyncListDelegate(typeof(AbilityData), kListm_stockRefreshCountdowns, new CmdDelegate(InvokeSyncListm_stockRefreshCountdowns));
		RegisterSyncListDelegate(typeof(AbilityData), kListm_currentCardIds, new CmdDelegate(InvokeSyncListm_currentCardIds));
		NetworkCRC.RegisterBehaviour("AbilityData", 0);
	}

	public SyncListInt CurrentCardIDs
	{
		get
		{
			return m_currentCardIds;
		}
	}

	public AbilityEntry[] abilityEntries
	{
		get
		{
			return m_abilities;
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
			return GetSpriteFromPath(m_spritePath0);
		}
	}

	public Sprite m_sprite1
	{
		get
		{
			return GetSpriteFromPath(m_spritePath1);
		}
	}

	public Sprite m_sprite2
	{
		get
		{
			return GetSpriteFromPath(m_spritePath2);
		}
	}

	public Sprite m_sprite3
	{
		get
		{
			return GetSpriteFromPath(m_spritePath3);
		}
	}

	public Sprite m_sprite4
	{
		get
		{
			return GetSpriteFromPath(m_spritePath4);
		}
	}

	public Sprite m_sprite5
	{
		get
		{
			return GetSpriteFromPath(m_spritePath5);
		}
	}

	public Sprite m_sprite6
	{
		get
		{
			return GetSpriteFromPath(m_spritePath6);
		}
	}

	public List<Ability> GetAbilitiesAsList()
	{
		if (m_abilitiesList == null || m_abilitiesList.Count == 0)
		{
			m_abilitiesList = new List<Ability>();
			m_abilitiesList.Add(m_ability0);
			m_abilitiesList.Add(m_ability1);
			m_abilitiesList.Add(m_ability2);
			m_abilitiesList.Add(m_ability3);
			m_abilitiesList.Add(m_ability4);
		}
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

	public ActorData SoftTargetedActor
	{
		get
		{
			return m_softTargetedActor;
		}
		set
		{
			if (m_softTargetedActor != value)
			{
				m_softTargetedActor = value;
				if (m_actor == GameFlowData.Get().activeOwnedActorData)
				{
					CameraManager cameraManager = CameraManager.Get();
					if (m_softTargetedActor)
					{
						cameraManager.SetTargetObjectToMouse(m_softTargetedActor.gameObject, CameraManager.CameraTargetReason.AbilitySoftTargeting);
					}
					else if (!cameraManager.IsOnMainCamera(GameFlowData.Get().activeOwnedActorData))
					{
						cameraManager.SetTargetObject(gameObject, CameraManager.CameraTargetReason.AbilitySoftTargeting);
					}
				}
			}
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
		m_abilities = new AbilityEntry[NUM_ACTIONS];
		for (int i = 0; i < NUM_ACTIONS; i++)
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
			if (abilityEntry.ability != null)
			{
				foreach (Ability ability in abilityEntry.ability.GetChainAbilities())
				{
					if (ability != null)
					{
						AddToAllChainAbilitiesList(ability, (ActionType)j);
					}
				}
			}
		}
		m_cooldowns = new Dictionary<string, int>();
		m_actor = GetComponent<ActorData>();
		for (int l = 0; l < NUM_CARDS; l++)
		{
			m_cachedCardAbilities.Add(null);
		}
		m_cooldownsSync.InitializeBehaviour(this, kListm_cooldownsSync);
		m_consumedStockCount.InitializeBehaviour(this, kListm_consumedStockCount);
		m_stockRefreshCountdowns.InitializeBehaviour(this, kListm_stockRefreshCountdowns);
		m_currentCardIds.InitializeBehaviour(this, kListm_currentCardIds);
	}

	public void InitAbilitySprites()
	{
		if (!m_abilitySpritesInitialized)
		{
			SetSpriteForAbility(m_ability0, m_sprite0);
			SetSpriteForAbility(m_ability1, m_sprite1);
			SetSpriteForAbility(m_ability2, m_sprite2);
			SetSpriteForAbility(m_ability3, m_sprite3);
			SetSpriteForAbility(m_ability4, m_sprite4);
			m_abilitySpritesInitialized = true;
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
		if (ability != null)
		{
			ability.sprite = sprite;
			if (ability.m_chainAbilities != null)
			{
				foreach (Ability ability2 in ability.m_chainAbilities)
				{
					if (ability2 != null)
					{
						ability2.sprite = sprite;
					}
				}
			}
		}
	}

	public override void OnStartClient()
	{
		m_cooldownsSync.Callback = new SyncList<int>.SyncListChanged(SyncListCallbackCoolDownsSync);
		m_currentCardIds.Callback = new SyncList<int>.SyncListChanged(SyncListCallbackCurrentCardsChanged);
	}

	public override void OnStartServer()
	{
		for (int i = 0; i < NUM_ACTIONS; i++)
		{
			m_cooldownsSync.Add(0);
			m_consumedStockCount.Add(0);
			m_stockRefreshCountdowns.Add(0);
		}
		for (int j = 0; j < NUM_CARDS; j++)
		{
			m_currentCardIds.Add(-1);
		}
		if (GameplayUtils.IsPlayerControlled(this))
		{
			int num = GameplayData.Get().m_turnsAbilitiesUnlock.Length;
			for (int k = 0; k < num && k < NUM_ABILITIES; k++)
			{
				PlaceInCooldownTillTurn((ActionType)k, GameplayData.Get().m_turnsAbilitiesUnlock[k]);
			}
		}
		InitializeStockCounts();
	}

	public void ReInitializeChainAbilityList()
	{
		ClearAllChainAbilitiesList();
		for (int i = 0; i < m_abilities.Length; i++)
		{
			AbilityEntry abilityEntry = m_abilities[i];
			if (abilityEntry.ability != null)
			{
				foreach (Ability ability in abilityEntry.ability.GetChainAbilities())
				{
					if (ability != null)
					{
						AddToAllChainAbilitiesList(ability, (ActionType)i);
					}
				}
			}
		}
	}

	internal static bool IsCard(ActionType actionType)
	{
		return actionType >= ActionType.CARD_0 && actionType <= ActionType.CARD_2;
	}

	internal static bool IsChain(ActionType actionType)
	{
		return actionType >= ActionType.CHAIN_0 && actionType <= ActionType.CHAIN_2;
	}

	internal static bool IsCharacterSpecificAbility(ActionType actionType)
	{
		return actionType != ActionType.INVALID_ACTION && !IsCard(actionType);
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
					if (characterTaunt.m_actionForTaunt == actionType
						&& i < characterData.CharacterComponent.Taunts.Count
						&& characterData.CharacterComponent.Taunts[i].Unlocked)
					{
						TauntCameraSet tauntCamSetData = m_actor.m_tauntCamSetData;
						CameraShotSequence cameraShotSequence = tauntCamSetData?.GetTauntCam(characterTaunt.m_uniqueID);
						if (cameraShotSequence != null && abilityOfActionType.CanTriggerAnimAtIndexForTaunt(cameraShotSequence.m_animIndex))
						{
							list.Add(cameraShotSequence);
						}
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
			if (characterTaunt.m_actionForTaunt == actionType
				&& (includeHidden || !characterTaunt.m_isHidden))
			{
				list.Add(characterTaunt);
			}
		}
		return list;
	}

	internal static bool CanTauntForActionTypeForPlayer(PersistedCharacterData characterData, CharacterResourceLink character, ActionType actionType, bool checkTauntUniqueId, int uniqueId)
	{
		if (CameraManager.Get() != null
			&& CameraManager.Get().m_abilityCinematicState == CameraManager.AbilityCinematicState.Never)
		{
			return false;
		}
		if (characterData != null && character.m_characterType != CharacterType.None && actionType != ActionType.INVALID_ACTION)
		{
			int count = characterData.CharacterComponent.Taunts.Count;

			for (int i = 0; i < character.m_taunts.Count && i < count; i++)
			{
				CharacterTaunt characterTaunt = character.m_taunts[i];
				if (characterTaunt != null
					&& characterTaunt.m_actionForTaunt == actionType
					&& (!checkTauntUniqueId || characterTaunt.m_uniqueID == uniqueId)
					&& characterData.CharacterComponent.Taunts[i].Unlocked
					&& GameManager.Get().GameplayOverrides.IsTauntAllowed(character.m_characterType, (int)actionType, characterTaunt.m_uniqueID))
				{
					return true;
				}
			}
		}
		return false;
	}

	internal static bool CanTauntApplyToActionType(CharacterResourceLink character, ActionType actionType)
	{
		if (CameraManager.Get() != null
			&& CameraManager.Get().m_abilityCinematicState == CameraManager.AbilityCinematicState.Never)
		{
			return false;
		}
		if (character.m_characterType != CharacterType.None && actionType != ActionType.INVALID_ACTION)
		{
			int num = 0;
			foreach (CharacterTaunt characterTaunt in character.m_taunts)
			{
				if (characterTaunt.m_actionForTaunt == actionType
					&& GameManager.Get().GameplayOverrides.IsTauntAllowed(character.m_characterType, (int)actionType, characterTaunt.m_uniqueID))
				{
					return true;
				}
				num++;
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

			for (int i = 0; i < NUM_ACTIONS; i++)
			{
				Ability ability = m_abilities[i].ability;
				if ((teamSensitiveData_authority.HasQueuedAction(i)
					|| ability != null && ability == GetSelectedAbility())
					&& UIQueueListPanel.GetUIPhaseFromAbilityPriority(m_abilities[i].ability.RunPriority) == actionPhase)
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
			for (int i = 0; i < NUM_ACTIONS; i++)
			{
				Ability ability = m_abilities[i].ability;
				if (teamSensitiveData_authority.HasQueuedAction(i)
					|| ability != null && ability == GetSelectedAbility())
				{
					list.Add(m_abilities[i]);
				}
			}
		}
		return list;
	}

	private static int CompareTabTargetsByActiveOwnedActorDistance(ActorData a, ActorData b)
	{
		if (GameFlowData.Get().activeOwnedActorData == null)
		{
			return 0;
		}
		Vector3 position = GameFlowData.Get().activeOwnedActorData.transform.position;
		float sqrMagnitude = (position - a.transform.position).sqrMagnitude;
		float sqrMagnitude2 = (position - b.transform.position).sqrMagnitude;
		if (sqrMagnitude == sqrMagnitude2)
		{
			return 0;
		}
		if (sqrMagnitude <= sqrMagnitude2)
		{
			return -1;
		}
		return 1;
	}

	public void NextSoftTarget()
	{
		ActorTurnSM actorTurnSM = m_actor.GetActorTurnSM();
		if (actorTurnSM != null)
		{
			int targetSelectionIndex = actorTurnSM.GetTargetSelectionIndex();
			int numTargets = m_selectedAbility.GetNumTargets();
			if (targetSelectionIndex >= numTargets)
			{
				SoftTargetedActor = null;
				return;
			}

			List<ActorData> validTargets = GetValidTargets(m_selectedAbility, targetSelectionIndex);
			if (validTargets.Count > 0)
			{
				validTargets.Sort(new Comparison<ActorData>(CompareTabTargetsByActiveOwnedActorDistance));
				if (SoftTargetedActor == null)
				{
					SoftTargetedActor = validTargets[0];
				}
				else
				{
					int num = validTargets.FindIndex((ActorData entry) => entry == SoftTargetedActor);
					ActorData softTargetedActor = validTargets[(num + 1) % validTargets.Count];
					SoftTargetedActor = softTargetedActor;
				}
			}
			else
			{
				SoftTargetedActor = null;
			}
		}
	}

	private void Update()
	{
		if (GameFlowData.Get().gameState == GameState.BothTeams_Decision
			&& GameFlowData.Get().activeOwnedActorData == m_actor)
		{
			ActorTurnSM actorTurnSM = m_actor.GetActorTurnSM();
			if (m_actionToSelectWhenEnteringDecisionState != ActionType.INVALID_ACTION
				&& actorTurnSM.CurrentState == TurnStateEnum.DECIDING)
			{
				if (RedoTurn(GetAbilityOfActionType(m_actionToSelectWhenEnteringDecisionState), m_actionToSelectWhenEnteringDecisionState, m_actionsToCancelForTurnRedo, m_cancelMovementForTurnRedo, m_retargetActionWithoutClearingOldAbilities))
				{
					ClearActionsToCancelOnTargetingComplete();
				}
				m_actionToSelectWhenEnteringDecisionState = ActionType.INVALID_ACTION;
			}
			else if (m_actionsToCancelForTurnRedo != null
				&& actorTurnSM.CurrentState == TurnStateEnum.DECIDING)
			{
				if (m_actionsToCancelForTurnRedo != null && actorTurnSM.CurrentState == TurnStateEnum.DECIDING)
				{
					ClearActionsToCancelOnTargetingComplete();
				}
			}
			else if ((actorTurnSM.CanSelectAbility() || actorTurnSM.CanQueueSimpleAction())
				&& !m_actor.IsDead())
			{
				for (int i = 0; i < m_abilities.Length; i++)
				{
					AbilityEntry abilityEntry = m_abilities[i];
					if (abilityEntry != null
						&& abilityEntry.ability != null
						&& abilityEntry.keyPreference != KeyPreference.NullPreference
						&& InputManager.Get().IsKeyBindingNewlyHeld(abilityEntry.keyPreference)
						&& !UITutorialFullscreenPanel.Get().IsAnyPanelVisible())
					{
						ActionType actionType = (ActionType)i;
						if (AbilityButtonPressed(actionType, abilityEntry.ability))
						{
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
			return false;
		}
		bool isPing = InputManager.Get().IsKeyBindingHeld(KeyPreference.MinimapPing);
		ActorTurnSM actorTurnSM = m_actor.GetActorTurnSM();
		bool canSelectAbility = actorTurnSM.CanSelectAbility();
		bool canQueueSimpleAction = actorTurnSM.CanQueueSimpleAction();
		if (!canSelectAbility && !canQueueSimpleAction)
		{
			if (isPing)
			{
				SendAbilityPing(false, actionType, ability);
			}
			return false;
		}
		m_cancelMovementForTurnRedo = false;
		m_actionsToCancelForTurnRedo = null;
		bool isRetargeting = InputManager.Get().IsKeyBindingHeld(KeyPreference.AbilityRetargetingModifier);
		if (HasQueuedAction(actionType) && !isRetargeting)
		{
			if (isPing)
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
		}
		else if (CanQueueActionByCancelingOthers(ability, actionType, canQueueSimpleAction, canSelectAbility, out List<ActionType> actionsToCancel, out bool cancelMovement))
		{
			if (isPing)
			{
				SendAbilityPing(true, actionType, ability);
			}
			else
			{
				m_retargetActionWithoutClearingOldAbilities = false;
				if (actorTurnSM.CurrentState == TurnStateEnum.CONFIRMED)
				{
					actorTurnSM.BackToDecidingState();
					m_cancelMovementForTurnRedo = cancelMovement;
					m_actionsToCancelForTurnRedo = actionsToCancel;
					m_actionToSelectWhenEnteringDecisionState = actionType;
					m_retargetActionWithoutClearingOldAbilities = isRetargeting;
				}
				else
				{
					if (actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION
						&& GetSelectedActionType() == actionType
						&& !SinglePlayerManager.IsCancelDisabled())
					{
						ClearSelectedAbility();
						actorTurnSM.BackToDecidingState();
					}
					else
					{
						return RedoTurn(ability, actionType, actionsToCancel, cancelMovement, isRetargeting);
					}
				}
			}
		}
		else if (isPing)
		{
			SendAbilityPing(false, actionType, ability);
		}
		return false;
	}

	private void SendAbilityPing(bool selectable, ActionType actionType, Ability ability)
	{
		if (TextConsole.Get() != null)
		{
			if (m_lastPingSendTime > 0f
				&& Time.time - m_lastPingSendTime <= HUD_UIResources.Get().m_mapPingCooldown)
			{
				return;
			}
			int remainingCooldown = Mathf.Max(GetAbilityEntryOfActionType(actionType).GetCooldownRemaining(), GetTurnsTillUnlock(actionType));
			bool isUlt = actionType == ActionType.ABILITY_4;
			LocalizationArg_AbilityPing localizedPing = LocalizationArg_AbilityPing.Create(
				m_actor.m_characterType,
				ability,
				selectable,
				remainingCooldown,
				isUlt,
				m_actor.GetTechPointsToDisplay(),
				m_actor.GetMaxTechPoints());
			m_actor.SendAbilityPingRequestToServer((int)m_actor.GetTeam(), localizedPing);
			m_lastPingSendTime = Time.time;
		}
	}

	public bool RedoTurn(Ability ability, ActionType actionType, List<ActionType> actionsToCancel, bool cancelMovement, bool retargetingModifierKeyHeld)
	{
		ActorController actorController = m_actor?.GetActorController();
		ActorTurnSM actorTurnSM = m_actor?.GetActorTurnSM();
		if (ability != null && !ability.IsSimpleAction() && retargetingModifierKeyHeld)
		{
			if (!actionsToCancel.IsNullOrEmpty() && actionsToCancel.Contains(actionType))
			{
				ability.BackupTargetingForRedo(actorTurnSM);
			}
			SetSelectedAbility(ability);
			actorController.SendSelectAbilityRequest();
			m_cancelMovementForTurnRedo = cancelMovement;
			m_actionsToCancelForTurnRedo = actionsToCancel;
			return false;
		}
		if (cancelMovement)
		{
			actorTurnSM.RequestCancelMovement();
			UISounds.GetUISounds().Play("ui/ingame/v1/move_undo");
		}
		if (actionsToCancel != null)
		{
			m_loggedErrorForNullAction = false;
			foreach (ActionType actionType2 in actionsToCancel)
			{
				actorTurnSM.RequestCancelAction(actionType2, true);
				UISounds.GetUISounds().Play("ui/ingame/v1/action_undo");
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
				if (actionsToCancel == null || !actionsToCancel.Contains(actionType))
				{
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
			}
			else
			{
				SetSelectedAbility(ability);
				actorController.SendSelectAbilityRequest();
			}
		}
		return true;
	}

	public unsafe bool CanQueueActionByCancelingOthers(Ability ability, ActionType actionType, bool canQueueSimpleAction, bool canSelectAbility, out List<ActionType> actionsToCancel, out bool cancelMovement)
	{
		bool flag = false;
		actionsToCancel = new List<ActionType>();
		cancelMovement = false;
		if (ability.IsSimpleAction())
		{
			if (canQueueSimpleAction && ValidateActionIsRequestableDisregardingQueuedActions(actionType))
			{
				flag = true;
			}
		}
		else if (canSelectAbility && ValidateActionIsRequestableDisregardingQueuedActions(actionType))
		{
			flag = true;
		}
		if (flag)
		{
			if (!ability.IsFreeAction()
				|| ability.GetRunPriority() == AbilityPriority.Evasion
				|| IsCard(actionType))
			{
				for (int i = 0; i < NUM_ACTIONS; i++)
				{
					ActionType actionType2 = (ActionType)i;
					if (HasQueuedAction(actionType2))
					{
						Ability abilityOfActionType = GetAbilityOfActionType(actionType2);
						if (abilityOfActionType != null
							&& (!abilityOfActionType.IsFreeAction() && !ability.IsFreeAction()
								|| abilityOfActionType.GetRunPriority() == AbilityPriority.Evasion && ability.GetRunPriority() == AbilityPriority.Evasion
								|| IsCard(GetActionTypeOfAbility(abilityOfActionType)) && IsCard(actionType)))
						{
							actionsToCancel.Add(actionType2);
						}
					}
				}
			}
			else if (ability.IsFreeAction() && HasQueuedAction(actionType))
			{
				actionsToCancel.Add(actionType);
			}
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
		}
		return flag;
	}

	public unsafe bool GetActionsToCancelOnTargetingComplete(ref List<ActionType> actionsToCancel, ref bool cancelMovement)
	{
		if (m_cancelMovementForTurnRedo || !m_actionsToCancelForTurnRedo.IsNullOrEmpty())
		{
			cancelMovement = m_cancelMovementForTurnRedo;
			actionsToCancel = m_actionsToCancelForTurnRedo;
			return true;
		}
		return false;
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
			return new List<ActorData>();
		}
		List<ActorData> list = new List<ActorData>();
		bool checkLoS = testAbility.GetCheckLoS(targetIndex);
		bool isPlayerControlled = GameplayUtils.IsPlayerControlled(this);
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if ((!isPlayerControlled || component.IsVisible(actorData.GetCurrentBoardSquare()))
				&& (!checkLoS || component.IsVisibleBySelf(actorData.GetCurrentBoardSquare()) || actor.IsLineOfSightVisibleException(actorData)))
			{
				list.Add(actorData);
			}
		}
		List<ActorData> list2 = new List<ActorData>();
		for (int i = 0; i < list.Count; i++)
		{
			ActorData actorData2 = list[i];
			if (!actorData2.IsDead())
			{
				AbilityTarget target = AbilityTarget.CreateAbilityTargetFromActor(actorData2, actor);
				if (ValidateAbilityOnTarget(testAbility, target, targetIndex, -1f, -1f))
				{
					list2.Add(actorData2);
				}
			}
		}
		return list2;
	}

	private void SyncListCallbackCoolDownsSync(SyncList<int>.Operation op, int index)
	{
		if (!NetworkServer.active)
		{
			for (int i = 0; i < NUM_ACTIONS; i++)
			{
				m_abilities[i].SetCooldownRemaining(m_cooldownsSync[i]);
			}
		}
	}

	private void SyncListCallbackCurrentCardsChanged(SyncList<int>.Operation op, int index)
	{
		if (!NetworkServer.active && CardManagerData.Get() != null)
		{
			for (int i = 0; i < m_currentCardIds.Count; i++)
			{
				Ability useAbility = null;
				if (m_currentCardIds[i] > 0)
				{
					Card spawnedCardInstance = GetSpawnedCardInstance((CardType)m_currentCardIds[i]);
					useAbility = spawnedCardInstance?.m_useAbility;
				}
				SetupCardAbility(i, useAbility);
			}
			UpdateCardBarUI();
		}
	}

	public void OnQueuedAbilitiesChanged()
	{
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < NUM_ACTIONS; i++)
		{
			ActionType type = (ActionType)i;
			Ability abilityOfActionType = GetAbilityOfActionType(type);
			if (!flag && abilityOfActionType != null && abilityOfActionType.GetAffectsMovement())
			{
				flag = true;
			}
			if (!flag2 && abilityOfActionType != null && abilityOfActionType.ShouldUpdateDrawnTargetersOnQueueChange())
			{
				flag2 = true;
			}
		}
		if (flag && !GameplayUtils.IsMinion(this))
		{
			ActorMovement component = GetComponent<ActorMovement>();
			component.UpdateSquaresCanMoveTo();
		}
		if (flag2 && m_actor.GetActorTargeting() != null)
		{
			m_actor.GetActorTargeting().MarkForForceRedraw();
		}
	}

	private void OnRespawn()
	{
		for (int i = 0; i < m_abilities.Length; i++)
		{
			AbilityEntry abilityEntry = m_abilities[i];
			if (abilityEntry != null)
			{
				ActionType action = (ActionType)i;
				Ability ability = abilityEntry.ability;
				if (ability != null)
				{
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
		}
	}

	public void SetSelectedAbility(Ability selectedAbility)
	{
		ActorTurnSM actorTurnSM = m_actor?.GetActorTurnSM();
		bool isActiveOwnedActorData = GameFlowData.Get().activeOwnedActorData == m_actor && m_actor != null;
		if (m_selectedAbility && isActiveOwnedActorData)
		{
			m_selectedAbility.OnAbilityDeselect();
		}
		m_selectedAbility = selectedAbility;
		Networkm_selectedActionForTargeting = GetActionTypeOfAbility(m_selectedAbility);
		if (m_selectedAbility && isActiveOwnedActorData)
		{
			m_selectedAbility.OnAbilitySelect();
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
		Board.Get().MarkForUpdateValidSquares(true);
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
			for (int i = (int)ActionType.ABILITY_0; i < (int)ActionType.NUM_ACTIONS; i++)
			{
				if (m_abilities[i] != null
					&& m_abilities[i].ability != null
					&& m_abilities[i].ability.m_abilityName == abilityName)
				{
					actionType = (ActionType)i;
					break;
				}
			}
			if (actionType == ActionType.INVALID_ACTION)
			{
				for (int j = 0; j < m_allChainAbilities.Count; j++)
				{
					if (m_allChainAbilities[j] != null && m_allChainAbilities[j].m_abilityName == abilityName)
					{
						return (ActionType)(j + (int)ActionType.CHAIN_0);
					}
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
			for (int i = (int)ActionType.ABILITY_0; i < (int)ActionType.NUM_ACTIONS; i++)
			{
				if (m_abilities[i] != null && m_abilities[i].ability == ability)
				{
					actionType = (ActionType)i;
					break;
				}
			}
			if (actionType == ActionType.INVALID_ACTION)
			{
				for (int j = 0; j < m_allChainAbilities.Count; j++)
				{
					if (m_allChainAbilities[j] != null && m_allChainAbilities[j] == ability)
					{
						return (ActionType)(j + (int)ActionType.CHAIN_0);
					}
				}
			}
		}
		return actionType;
	}

	public ActionType GetParentAbilityActionType(Ability ability)
	{
		ActionType actionTypeOfAbility = GetActionTypeOfAbility(ability);
		if (IsChain(actionTypeOfAbility))
		{
			for (int i = 0; i < m_allChainAbilities.Count; i++)
			{
				if (m_allChainAbilities[i] == ability)
				{
					return m_allChainAbilityParentActionTypes[i];
				}
			}
		}
		return actionTypeOfAbility;
	}

	public Ability GetAbilityOfActionType(ActionType type)
	{
		Ability result;
		if (IsChain(type))
		{
			int num = type - ActionType.CHAIN_0;
			if (num >= 0 && num < m_allChainAbilities.Count)
			{
				result = m_allChainAbilities[num];
			}
			else
			{
				result = null;
			}
		}
		else
		{
			if (type >= ActionType.ABILITY_0 && type < (ActionType)m_abilities.Length)
			{
				return m_abilities[(int)type].ability;
			}
			result = null;
		}
		return result;
	}

	public Ability GetAbilityOfType(Type abilityType)
	{
		foreach (AbilityEntry abilityEntry in m_abilities)
		{
			if (abilityEntry != null
				&& abilityEntry.ability != null
				&& abilityEntry.ability.GetType() == abilityType)
			{
				return abilityEntry.ability;
			}
		}
		return null;
	}

	public T GetAbilityOfType<T>() where T : Ability
	{
		foreach (AbilityEntry abilityEntry in m_abilities)
		{
			if (abilityEntry != null
				&& abilityEntry.ability != null
				&& abilityEntry.ability.GetType() == typeof(T))
			{
				return abilityEntry.ability as T;
			}
		}
		return null;
	}

	public AbilityEntry GetAbilityEntryOfActionType(ActionType type)
	{
		if (type >= ActionType.ABILITY_0 && type < (ActionType)m_abilities.Length)
		{
			return m_abilities[(int)type];
		}
		return null;
	}

	public static CardType GetCardTypeByActionType(CharacterCardInfo cardInfo, ActionType actionType)
	{
		if (actionType == ActionType.CARD_0)
		{
			return cardInfo.PrepCard;
		}
		if (actionType == ActionType.CARD_1)
		{
			return cardInfo.DashCard;
		}
		if (actionType == ActionType.CARD_2)
		{
			return cardInfo.CombatCard;
		}
		return CardType.None;
	}

	public List<Ability> GetCachedCardAbilities()
	{
		return m_cachedCardAbilities;
	}

	public bool IsAbilityAllowedByUnlockTurns(ActionType actionType)
	{
		return GetTurnsTillUnlock(actionType) <= 0;
	}

	public int GetTurnsTillUnlock(ActionType actionType)
	{
		if (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("NoCooldowns"))
		{
			return 0;
		}
		int turns = 0;
		if (GameFlowData.Get() != null && GameplayData.Get() != null)
		{
			int currentTurn = GameFlowData.Get().CurrentTurn;
			SpawnPointManager spawnPointManager = SpawnPointManager.Get();
			if (spawnPointManager != null
				&& spawnPointManager.m_spawnInDuringMovement
				&& GameplayData.Get().m_disableAbilitiesOnRespawn
				&& m_actor.NextRespawnTurn == currentTurn)
			{
				turns = 1;
			}
			else if (IsCard(actionType))
			{
				turns = GameplayData.Get().m_turnCatalystsUnlock - currentTurn;
			}
			else
			{
				int[] turnsAbilitiesUnlock = GameplayData.Get().m_turnsAbilitiesUnlock;
				if (actionType < (ActionType)turnsAbilitiesUnlock.Length)
				{
					turns = turnsAbilitiesUnlock[(int)actionType] - currentTurn;
				}
			}
		}
		return Mathf.Max(0, turns);
	}

	public int GetCooldownRemaining(ActionType action)
	{
		if (IsChain(action))
		{
			return 0;
		}
		return m_abilities[(int)action].GetCooldownRemaining();
	}

	public bool IsActionInCooldown(ActionType action)
	{
		return !IsChain(action) && m_abilities[(int)action].GetCooldownRemaining() != 0;
	}

	public void TriggerCooldown(ActionType action)
	{
		if (!IsChain(action))
		{
			AbilityEntry abilityEntry = m_abilities[(int)action];
			if (abilityEntry.ability != null)
			{
				int moddedCooldown = abilityEntry.ability.GetModdedCooldown();
				if (moddedCooldown > 0)
				{
					if (GameplayMutators.Get() != null)
					{
						int num = moddedCooldown + 1;
						int cooldownTimeAdjustment = GameplayMutators.GetCooldownTimeAdjustment();
						float cooldownMultiplier = GameplayMutators.GetCooldownMultiplier();
						int num2 = Mathf.RoundToInt((num + cooldownTimeAdjustment) * cooldownMultiplier);
						num2 = Math.Max(num2, 0);
						m_cooldowns[abilityEntry.ability.m_abilityName] = num2;
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
			}
		}
	}

	public void OverrideCooldown(ActionType action, int cooldownRemainingOverride)
	{
		if (!IsChain(action))
		{
			AbilityEntry abilityEntry = m_abilities[(int)action];
			if (abilityEntry.ability != null)
			{
				m_cooldowns[abilityEntry.ability.m_abilityName] = cooldownRemainingOverride;
				SynchronizeCooldownsToSlots();
			}
		}
	}

	public void ApplyCooldownReduction(ActionType action, int cooldownReduction)
	{
		if (cooldownReduction > 0)
		{
			int num = GetCooldownRemaining(action);
			if (num > 0)
			{
				num -= cooldownReduction;
				num = Mathf.Max(0, num);
				OverrideCooldown(action, num);
			}
		}
	}

	public void ProgressCooldowns()
	{
		Dictionary<string, int> cooldownsCopy = new Dictionary<string, int>(m_cooldowns);
		foreach (string key in cooldownsCopy.Keys)
		{
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
		SynchronizeCooldownsToSlots();
	}

	public void ProgressCooldownsOfAbilities(List<Ability> abilities)
	{
		Dictionary<string, int> cooldownsCopy = new Dictionary<string, int>(m_cooldowns);
		foreach (string key in cooldownsCopy.Keys)
		{
			if (m_cooldowns[key] > 0)
			{
				bool flag = false;
				foreach (Ability ability in abilities)
				{
					if (ability.m_abilityName == key)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					m_cooldowns[key] -= 1;
					if (m_cooldowns[key] == 0)
					{
						m_cooldowns.Remove(key);
					}
				}
			}
		}
		SynchronizeCooldownsToSlots();
	}

	public void ProgressCharacterAbilityCooldowns()
	{
		Dictionary<string, int> cooldownsCopy = new Dictionary<string, int>(m_cooldowns);
		foreach (string key in cooldownsCopy.Keys)
		{
			ActionType actionType = GetActionType(key);
			if (actionType >= ActionType.ABILITY_0
				&& actionType <= ActionType.ABILITY_6
				&& m_cooldowns[key] > 0)
			{
				m_cooldowns[key] -= 1;
				if (m_cooldowns[key] == 0)
				{
					m_cooldowns.Remove(key);
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
			return;
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
		for (int i = 0; i < NUM_ABILITIES; i++)
		{
			ActionType actionType = (ActionType)i;
			AbilityEntry abilityEntry = m_abilities[(int)actionType];
			if (abilityEntry.ability != null)
			{
				string abilityName = abilityEntry.ability.m_abilityName;
				if (m_cooldowns.ContainsKey(abilityName))
				{
					m_cooldowns.Remove(abilityName);
					flag = true;
				}
			}
		}
		if (flag)
		{
			SynchronizeCooldownsToSlots();
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
		if (abilityEntry.ability != null)
		{
			string abilityName = abilityEntry.ability.m_abilityName;
			if (m_cooldowns.ContainsKey(abilityName))
			{
				m_cooldowns.Remove(abilityName);
				SynchronizeCooldownsToSlots();
			}
		}
	}

	public void PlaceInCooldownTillTurn(ActionType action, int turnNumber)
	{
		AbilityEntry abilityEntry = m_abilities[(int)action];
		if (abilityEntry.ability != null)
		{
			int num = turnNumber - GameFlowData.Get().CurrentTurn;
			if (num > 0)
			{
				m_cooldowns[abilityEntry.ability.m_abilityName] = num;
				SynchronizeCooldownsToSlots();
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

		for (int i = 0; i < m_abilities.Length; i++)
		{
			AbilityEntry abilityEntry = m_abilities[i];
			string key = abilityEntry.ability?.m_abilityName;
			int num;
			if (abilityEntry.ability == null || !m_cooldowns.ContainsKey(key))
			{
				num = 0;
			}
			else
			{
				num = m_cooldowns[key];
			}
			if (abilityEntry.GetCooldownRemaining() != num)
			{
				abilityEntry.SetCooldownRemaining(num);
				if (m_cooldownsSync[i] != num)
				{
					m_cooldownsSync[i] = num;
				}
			}
		}
	}

	[Server]
	private void InitializeStockCounts()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AbilityData::InitializeStockCounts()' called on client");
			return;
		}
		for (int i = 0; i < NUM_ABILITIES; i++)
		{
			ActionType actionType = (ActionType)i;
			AbilityEntry abilityEntry = m_abilities[(int)actionType];
			if (abilityEntry.ability != null
				&& abilityEntry.ability.GetModdedMaxStocks() > 0
				&& abilityEntry.ability.m_initialStockAmount >= 0)
			{
				int desiredAmount = Mathf.Min(abilityEntry.ability.GetModdedMaxStocks(), abilityEntry.ability.m_initialStockAmount);
				OverrideStockRemaining(actionType, desiredAmount);
			}
		}
	}

	public int GetMaxStocksCount(ActionType actionType)
	{
		AbilityEntry abilityEntry = m_abilities[(int)actionType];
		if (abilityEntry.ability != null)
		{
			return abilityEntry.ability.GetModdedMaxStocks();
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
		if (IsChain(action))
		{
			return true;
		}
		if (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("NoCooldowns"))
		{
			return true;
		}
		AbilityEntry abilityEntry = m_abilities[(int)action];
		if (abilityEntry.ability.m_abilityManagedStockCount)
		{
			return true;
		}
		int moddedMaxStocks = abilityEntry.ability.GetModdedMaxStocks();
		return moddedMaxStocks <= 0 || m_consumedStockCount[(int)action] < moddedMaxStocks;
	}

	public void ConsumeStock(ActionType action)
	{
		if (!IsChain(action))
		{
			AbilityEntry abilityEntry = m_abilities[(int)action];
			if (abilityEntry.ability != null)
			{
				int moddedMaxStocks = abilityEntry.ability.GetModdedMaxStocks();
				if (moddedMaxStocks > 0 && m_consumedStockCount[(int)action] < moddedMaxStocks)
				{
					if (m_consumedStockCount[(int)action] == 0)
					{
						int num = abilityEntry.ability.GetModdedStockRefreshDuration() + 1;
						if (m_stockRefreshCountdowns[(int)action] != num)
						{
							m_stockRefreshCountdowns[(int)action] = num;
						}
					}
					int num2 = Mathf.Clamp(m_consumedStockCount[(int)action] + abilityEntry.ability.m_stockConsumedOnCast, 0, moddedMaxStocks);
					if (m_consumedStockCount[(int)action] != num2)
					{
						m_consumedStockCount[(int)action] = num2;
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
			return;
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
		for (int i = 0; i < NUM_ACTIONS; i++)
		{
			ProgressStockRefreshTimeForAction((ActionType)i, 1);
		}
	}

	public void ProgressStockRefreshTimeForAction(ActionType action, int advanceAmount)
	{
		if (action < (ActionType)m_stockRefreshCountdowns.Count && advanceAmount > 0)
		{
			AbilityEntry abilityEntry = m_abilities[(int)action];
			if ((m_stockRefreshCountdowns[(int)action] > 0 || m_consumedStockCount[(int)action] > 0)
				&& abilityEntry != null
				&& abilityEntry.ability != null
				&& abilityEntry.ability.GetModdedStockRefreshDuration() > 0)
			{
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
					if (num2 != 0)
					{
						m_stockRefreshCountdowns[(int)action] -= num2;
					}
					if (m_stockRefreshCountdowns[(int)action] <= 0 && m_consumedStockCount[(int)action] > 0)
					{
						if (abilityEntry.ability.RefillAllStockOnRefresh())
						{
							m_consumedStockCount[(int)action] = 0;
						}
						else
						{
							m_consumedStockCount[(int)action] -= 1;
						}
						if (m_consumedStockCount[(int)action] > 0)
						{
							int num4 = moddedStockRefreshDuration;
							if (m_stockRefreshCountdowns[(int)action] != num4)
							{
								m_stockRefreshCountdowns[(int)action] = num4;
							}
						}
						else if (m_stockRefreshCountdowns[(int)action] != 0)
						{
							m_stockRefreshCountdowns[(int)action] = 0;
						}
					}
				}
				else
				{
					int num5 = num2 - m_stockRefreshCountdowns[(int)action];
					int num6 = moddedStockRefreshDuration - num5;
					if (m_stockRefreshCountdowns[(int)action] != num6)
					{
						m_stockRefreshCountdowns[(int)action] = num6;
					}
					if (m_consumedStockCount[(int)action] > 0)
					{
						if (abilityEntry.ability.RefillAllStockOnRefresh())
						{
							m_consumedStockCount[(int)action] = 0;
						}
						else
						{
							m_consumedStockCount[(int)action] -= 1;
						}
					}
				}
			}
		}
	}

	public void OverrideStockRemaining(ActionType action, int desiredAmount)
	{
		if (!IsChain(action))
		{
			AbilityEntry abilityEntry = m_abilities[(int)action];
			if (abilityEntry.ability != null)
			{
				int value = Mathf.Max(0, abilityEntry.ability.GetModdedMaxStocks() - desiredAmount);
				m_consumedStockCount[(int)action] = value;
				if (m_consumedStockCount[(int)action] == 0 && m_stockRefreshCountdowns[(int)action] != 0)
				{
					m_stockRefreshCountdowns[(int)action] = 0;
				}
			}
		}
	}

	public void OverrideStockRefreshCountdown(ActionType action, int desiredCountdown)
	{
		if (!IsChain(action))
		{
			AbilityEntry abilityEntry = m_abilities[(int)action];
			if (abilityEntry.ability != null)
			{
				int num = Mathf.Clamp(desiredCountdown, 0, abilityEntry.ability.GetModdedStockRefreshDuration());
				if (m_stockRefreshCountdowns[(int)action] != num)
				{
					m_stockRefreshCountdowns[(int)action] = num;
				}
			}
		}
	}

	public int GetStockRefreshDurationForAbility(ActionType action)
	{
		if (!IsChain(action))
		{
			AbilityEntry abilityEntry = m_abilities[(int)action];
			if (abilityEntry.ability != null)
			{
				return abilityEntry.ability.GetModdedStockRefreshDuration();
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
			BoardSquare square = Board.Get().GetSquare(target.GridPos);
			flag |= IsTargetSquareInRangeOfAbilityFromSquare(square, currentBoardSquare, num, num2);
		}
		else if (targetingParadigm == Ability.TargetingParadigm.Position)
		{
			Vector3 loSCheckPos = actor.GetLoSCheckPos();
			float num3 = num * Board.Get().squareSize;
			float num4 = num2 * Board.Get().squareSize;
			if (GameplayData.Get().m_abilityRangeType == GameplayData.AbilityRangeType.WorldDistToFreePos)
			{
				Vector3 vector = target.FreePos - loSCheckPos;
				vector.y = 0f;
				float sqrMagnitude = vector.sqrMagnitude;
				flag = sqrMagnitude <= num3 * num3 && sqrMagnitude >= num4 * num4;
			}
			else
			{
				BoardSquare square2 = Board.Get().GetSquare(target.GridPos);
				flag |= IsTargetSquareInRangeOfAbilityFromSquare(square2, currentBoardSquare, num, num2);
			}
		}
		else if (targetingParadigm == Ability.TargetingParadigm.Direction)
		{
			flag = true;
		}
		else
		{
			Log.Error("Checking range for ability " + ability.m_abilityName + ", but its targeting paradigm is invalid.");
			flag = false;
		}
		return flag;
	}

	public bool IsTargetSquareInRangeOfAbilityFromSquare(BoardSquare dest, BoardSquare src, float rangeInSquares, float minRangeInSquares)
	{
		bool result = true;
		if (src && dest)
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
			bool flag2 = rangeInSquares < 0f || num <= rangeInSquares;
			bool flag3 = num >= minRangeInSquares;
			if (!flag2 || !flag3)
			{
				return false;
			}
		}
		return true;
	}

	public bool HasLineOfSightToTarget(Ability specificAbility, AbilityTarget target, int targetIndex)
	{
		ActorData actor = m_actor;
		Ability.TargetingParadigm targetingParadigm = specificAbility.GetTargetingParadigm(targetIndex);
		if (actor.GetCurrentBoardSquare() == null)
		{
			return false;
		}
		else
		{
			if (targetingParadigm == Ability.TargetingParadigm.BoardSquare
				|| targetingParadigm == Ability.TargetingParadigm.Position)
			{
				BoardSquare square = Board.Get().GetSquare(target.GridPos);
				if (actor.LineOfSightVisibleExceptionSquares.Contains(square)
					|| actor.GetCurrentBoardSquare().GetLOS(target.GridPos.x, target.GridPos.y))
				{
					return true;
				}
			}
			else if (targetingParadigm == Ability.TargetingParadigm.Direction)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasLineOfSightToActor(ActorData target, bool ignoreExceptions = false)
	{
		ActorData actor = m_actor;
		if (target == null || actor == null)
		{
			return false;
		}
		if (!ignoreExceptions && actor.IsLineOfSightVisibleException(target))
		{
			return true;
		}
		BoardSquare currentBoardSquare = target.GetCurrentBoardSquare();
		return actor.GetCurrentBoardSquare().GetLOS(currentBoardSquare.x, currentBoardSquare.y);
	}

	public bool ValidateAbilityOnTarget(Ability ability, AbilityTarget target, int targetIndex, float calculatedMinRangeInSquares = -1f, float calculatedMaxRangeInSquares = -1f)
	{
		bool result = false;
		ActorTurnSM actorTurnSM = m_actor.GetActorTurnSM();
		if (actorTurnSM != null)
		{
			List<AbilityTarget> abilityTargets = actorTurnSM.GetAbilityTargets();
			if (abilityTargets != null)
			{
				result = targetIndex <= abilityTargets.Count
					&& ValidateAbilityOnTarget(ability, target, targetIndex, abilityTargets, calculatedMinRangeInSquares, calculatedMaxRangeInSquares);
			}
		}
		return result;
	}

	public bool ValidateAbilityOnTarget(Ability ability, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets, float calculatedMinRangeInSquares = -1f, float calculatedMaxRangeInSquares = -1f)
	{
		if (target == null)
		{
			return false;
		}
		ActorData actor = m_actor;
		bool flag = true;
		Ability.TargetingParadigm targetingParadigm = ability.GetTargetingParadigm(targetIndex);
		if (targetingParadigm == Ability.TargetingParadigm.BoardSquare
			|| targetingParadigm == Ability.TargetingParadigm.Position)
		{
			BoardSquare square = Board.Get().GetSquare(target.GridPos);
			flag = square != null
				&& (ability.AllowInvalidSquareForSquareBasedTarget()
					|| Board.Get().GetSquare(target.GridPos).IsValidForGameplay());
			if (!ability.IsSimpleAction())
			{
				if (flag && BarrierManager.Get().IsPositionTargetingBlocked(actor, square))
				{
					flag = false;
				}
				if (!flag)
				{
					return false;
				}
			}
		}
		bool flag2 = ability.IsSimpleAction() || IsAbilityTargetInRange(ability, target, targetIndex, calculatedMinRangeInSquares, calculatedMaxRangeInSquares);
		if (!flag2)
		{
			return false;
		}
		bool flag3 = true;
		if (AbilityUtils.AbilityHasTag(ability, AbilityTags.ValidOnlyWhereInCoverToCaster))
		{
			BoardSquare square2 = Board.Get().GetSquare(target.GridPos);
			flag3 = ActorCover.IsInCoverWrt(actor.GetFreePos(), square2, null, null, null);
		}
		else if (AbilityUtils.AbilityHasTag(ability, AbilityTags.ValidOnlyWhereInCover))
		{
			BoardSquare square3 = Board.Get().GetSquare(target.GridPos);
			flag3 = ActorCover.CalcCoverLevelGeoOnly(out bool[] array, square3);
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
		if (flag2 && flag && flag3 && flag4 && ValidateAbilityIsCastableDisregardingMovement(ability))
		{
			return ability.CustomTargetValidation(actor, target, targetIndex, currentTargets);
		}
		return false;
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
			for (int i = 0; i < targets.Count; i++)
			{
				AbilityTarget target = targets[i];
				if (!ValidateAbilityOnTarget(abilityOfActionType, target, i, targets, -1f, -1f))
				{
					return false;
				}
			}
		}
		return result;
	}

	public bool ValidateAbilityIsCastable(Ability ability)
	{
		if (ability == null)
		{
			Log.Error("Actor " + m_actor.DisplayName + " calling ValidateAbilityIsCastable on a null ability.");
			return false;
		}

		bool flag = ValidateAbilityIsCastableDisregardingMovement(ability);
		bool flag2 = ability.GetMovementAdjustment() != Ability.MovementAdjustment.NoMovement || !m_actor.HasQueuedMovement();
		bool flag3 = m_actor.QueuedMovementAllowsAbility || !ability.GetAffectsMovement();
		bool flag4 = ability.RunPriority != AbilityPriority.Evasion || !HasQueuedAbilityInPhase(AbilityPriority.Evasion);
		bool flag5 = !IsCard(GetActionTypeOfAbility(ability)) || !HasQueuedCardAbility();
		return flag && flag2 && flag3 && flag4 && flag5;
	}

	public bool ValidateActionIsRequestable(ActionType abilityAction)
	{
		Ability abilityOfActionType = GetAbilityOfActionType(abilityAction);
		if (abilityOfActionType == null)
		{
			Log.Error("Actor " + m_actor.DisplayName + " calling ValidateActionIsRequestable on a null ability.");
			return false;
		}

		bool flag = ValidateActionIsRequestableDisregardingQueuedActions(abilityAction);
		bool flag2 = !HasQueuedAction(abilityAction);
		bool flag3 = abilityOfActionType.IsFreeAction() || GetActionCostOfQueuedAbilities(abilityAction) == 0;
		bool flag4 = abilityOfActionType.GetMovementAdjustment() != Ability.MovementAdjustment.NoMovement || !m_actor.HasQueuedMovement();
		bool flag5 = m_actor.QueuedMovementAllowsAbility || !abilityOfActionType.GetAffectsMovement();
		bool flag6 = abilityOfActionType.RunPriority != AbilityPriority.Evasion || !HasQueuedAbilityInPhase(AbilityPriority.Evasion);
		bool flag7 = !IsCard(abilityAction) || !HasQueuedCardAbility();
		return flag && flag2 && flag3 && flag4 && flag5 && flag6 && flag7;
	}

	public bool ValidateAbilityIsCastableDisregardingMovement(Ability ability)
	{
		if (ability == null)
		{
			Log.Error("Actor " + m_actor.DisplayName + " calling ValidateAbilityIsCastableDisregardingMovement on a null ability.");
			return false;
		}
		ActionType actionTypeOfAbility = GetActionTypeOfAbility(ability);
		bool flag = actionTypeOfAbility != ActionType.INVALID_ACTION;
		bool flag2 = !m_actor.IsDead();
		bool flag3 = m_actor.TechPoints >= ability.GetModdedCost();
		bool flag4 = !AbilityUtils.AbilityHasTag(ability, AbilityTags.ValidOnlyWhenOutOfCombat) || m_actor.OutOfCombat;
		bool flag5 = !AbilityUtils.AbilityHasTag(ability, AbilityTags.ValidOnlyWhenInCombat) || !m_actor.OutOfCombat;
		bool flag6 = !m_actor.GetActorStatus().IsActionSilenced(actionTypeOfAbility, false);
		bool flag7 = ability.CustomCanCastValidation(m_actor);
		bool result = flag && flag2 && flag3 && flag4 && flag5 && flag6 && flag7;
		return result;
	}

	public bool ValidateActionIsRequestableDisregardingQueuedActions(ActionType abilityAction)
	{
		Ability abilityOfActionType = GetAbilityOfActionType(abilityAction);
		if (abilityOfActionType == null)
		{
			Log.Error("Actor " + m_actor.DisplayName + " calling ValidateActionIsRequestableDisregardingQueuedActions on a null ability.");
			return false;
		}

		ActorData actor = m_actor;
		bool flag = !IsActionInCooldown(abilityAction) || abilityOfActionType.GetModdedMaxStocks() > 0;
		if (!flag
			&& actor != null
			&& AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.IgnoreCooldownIfFullEnergy))
		{
			flag = actor.TechPoints + actor.ReservedTechPoints >= actor.GetMaxTechPoints();
		}
		bool flag2 = ActionHasEnoughStockToTrigger(abilityAction);
		bool flag3 = IsAbilityAllowedByUnlockTurns(abilityAction);
		bool flag4 = ValidateAbilityIsCastableDisregardingMovement(abilityOfActionType);
		bool flag5 = SinglePlayerManager.IsActionAllowed(actor, abilityAction);
		return flag && flag2 && flag3 && flag4 && flag5;
	}

	public BoardSquare GetAutoSelectTarget()
	{
		BoardSquare result = null;
		if (m_selectedAbility && m_selectedAbility.IsAutoSelect())
		{
			result = m_actor.GetCurrentBoardSquare();
		}
		return result;
	}

	public bool HasQueuedAbilities()
	{
		bool result = false;
		ActorTeamSensitiveData teamSensitiveData_authority = m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
		{
			for (int i = 0; i < NUM_ACTIONS; i++)
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
			for (int i = 0; i < NUM_ACTIONS; i++)
			{
				if (teamSensitiveData_authority.HasQueuedAction(i) && IsCard((ActionType)i))
				{
					return true;
				}
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
			for (int i = 0; i < NUM_ACTIONS; i++)
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
			for (int i = 0; i < NUM_ACTIONS; i++)
			{
				ActionType actionType = (ActionType)i;
				if (teamSensitiveData_authority.HasQueuedAction(actionType))
				{
					Ability abilityOfActionType = GetAbilityOfActionType(actionType);
					if (abilityOfActionType != null && abilityOfActionType.GetMovementAdjustment() > movementAdjustment)
					{
						movementAdjustment = abilityOfActionType.GetMovementAdjustment();
					}
				}
			}
		}
		SpawnPointManager spawnPointManager = SpawnPointManager.Get();
		if (spawnPointManager != null
			&& spawnPointManager.m_spawnInDuringMovement
			&& m_actor.NextRespawnTurn == GameFlowData.Get().CurrentTurn
			&& GameplayData.Get().m_movementAllowedOnRespawn < movementAdjustment)
		{
			movementAdjustment = GameplayData.Get().m_movementAllowedOnRespawn;
		}
		return movementAdjustment;
	}

	public float GetQueuedAbilitiesMovementAdjust()
	{
		float result = 0f;
		if (GetQueuedAbilitiesMovementAdjustType() == Ability.MovementAdjustment.ReducedMovement)
		{
			result = -1f * m_actor.GetAbilityMovementCost();
		}
		return result;
	}

	public List<StatusType> GetQueuedAbilitiesOnRequestStatuses()
	{
		List<StatusType> list = new List<StatusType>();
		ActorTeamSensitiveData teamSensitiveData_authority = m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
		{
			for (int i = 0; i < NUM_ACTIONS; i++)
			{
				ActionType actionType = (ActionType)i;
				if (teamSensitiveData_authority.HasQueuedAction(actionType))
				{
					Ability abilityOfActionType = GetAbilityOfActionType(actionType);
					if (abilityOfActionType != null)
					{
						list.AddRange(abilityOfActionType.GetStatusToApplyWhenRequested());
					}
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
			for (int i = 0; i < NUM_ACTIONS; i++)
			{
				ActionType actionType = (ActionType)i;
				if (teamSensitiveData_authority.HasQueuedAction(actionType))
				{
					Ability abilityOfActionType = GetAbilityOfActionType(actionType);
					if (abilityOfActionType != null && abilityOfActionType.GetStatusToApplyWhenRequested().Contains(status))
					{
						return true;
					}
				}
			}
			return false;
		}
		return false;
	}

	public bool GetQueuedAbilitiesAllowSprinting()
	{
		bool result = true;
		ActorTeamSensitiveData teamSensitiveData_authority = m_actor.TeamSensitiveData_authority;
		if (teamSensitiveData_authority != null)
		{
			for (int i = 0; i < NUM_ACTIONS; i++)
			{
				ActionType actionType = (ActionType)i;
				if (teamSensitiveData_authority.HasQueuedAction(actionType))
				{
					Ability abilityOfActionType = GetAbilityOfActionType(actionType);
					if (abilityOfActionType != null && abilityOfActionType.GetAffectsMovement())
					{
						result = false;
						break;
					}
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
			for (int i = 0; i < NUM_ACTIONS; i++)
			{
				ActionType actionType = (ActionType)i;
				if (teamSensitiveData_authority.HasQueuedAction(actionType))
				{
					Ability abilityOfActionType = GetAbilityOfActionType(actionType);
					if (abilityOfActionType != null && abilityOfActionType.GetPreventsMovement())
					{
						return false;
					}
				}
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
			for (int i = 0; i < NUM_ACTIONS; i++)
			{
				ActionType actionType = (ActionType)i;
				if (teamSensitiveData_authority.HasQueuedAction(actionType) && actionType != actionToSkip)
				{
					Ability abilityOfActionType = GetAbilityOfActionType(actionType);
					if (abilityOfActionType != null && !abilityOfActionType.IsFreeAction())
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
			return m_cardTypeToCardInstance[cardType];
		}
		GameObject cardPrefab = CardManagerData.Get().GetCardPrefab(cardType);
		if (cardPrefab != null)
		{
			GameObject gameObject = Instantiate(cardPrefab);
			Card component = gameObject.GetComponent<Card>();
			if (component != null && component.m_useAbility != null)
			{
				gameObject.transform.parent = base.gameObject.transform;
				m_cardTypeToCardInstance[cardType] = component;
				component.m_useAbility.OverrideActorDataIndex(m_actor.ActorIndex);
			}
			else
			{
				Log.Error("Card prefab " + cardPrefab.name + " does not have Card component");
				Destroy(gameObject);
			}
			return component;
		}
		return null;
	}

	public void SetupCardAbility(int cardSlotIndex, Ability useAbility)
	{
		int num = CARD_0 + cardSlotIndex;
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
		if (NetworkServer.active)
		{
			SynchronizeCooldownsToSlots();
		}
	}

	public void SpawnAndSetupCardsOnReconnect()
	{
		if (!NetworkServer.active)
		{
			for (int i = 0; i < m_currentCardIds.Count; i++)
			{
				Card spawnedCardInstance = GetSpawnedCardInstance((CardType)m_currentCardIds[i]);
				SetupCardAbility(i, spawnedCardInstance?.m_useAbility);
			}
			UpdateCardBarUI();
		}
	}

	public IEnumerable<Card> GetActiveCards()
	{
		for (int i = 0; i < m_currentCardIds.Count; i++)
		{
			yield return GetSpawnedCardInstance((CardType)m_currentCardIds[i]);
		}
		yield break;
	}

	public void UpdateCatalystDisplay()
	{
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateCatalysts(m_actor, m_cachedCardAbilities);
			HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.UpdateCatalysts(m_actor, m_cachedCardAbilities);
		}
	}

	private void UpdateCardBarUI()
	{
		if (NetworkClient.active
			&& m_actor == GameFlowData.Get().activeOwnedActorData
			&& HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_cardBar.Rebuild();
		}
		UpdateCatalystDisplay();
	}

	public float GetTargetableRadius(int actionTypeInt, ActorData caster)
	{
		if (actionTypeInt < m_abilities.Length
			&& m_abilities[actionTypeInt].ability != null
			&& m_abilities[actionTypeInt].ability.CanShowTargetableRadiusPreview())
		{
			return m_abilities[actionTypeInt].ability.GetTargetableRadiusInSquares(caster);
		}
		return 0f;
	}

	public void OnClientCombatPhasePlayDataReceived(List<ClientResolutionAction> resolutionActions)
	{
		for (int i = 0; i <= 4; i++)
		{
			if (m_abilities[i] != null && m_abilities[i].ability != null)
			{
				m_abilities[i].ability.OnClientCombatPhasePlayDataReceived(resolutionActions, m_actor);
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera() || m_abilities == null)
		{
			return;
		}
		foreach (AbilityEntry abilityEntry in m_abilities)
		{
			if (abilityEntry != null && abilityEntry.ability != null)
			{
				abilityEntry.ability.DrawGizmos();
			}
		}
		DrawBoardVisibilityGizmos();
	}

	private void DrawBoardVisibilityGizmos()
	{
		if (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("ShowBoardSquareVisGizmo"))
		{
			BoardSquare playerFreeSquare = Board.Get().PlayerFreeSquare;
			if (playerFreeSquare != null)
			{
				for (int i = 0; i < 0xF; i++)
				{
					foreach (BoardSquare boardSquare in AreaEffectUtils.GetSquaresInBorderLayer(playerFreeSquare, i, false))
					{
						if (boardSquare.IsValidForGameplay())
						{
							if (playerFreeSquare.GetLOS(boardSquare.x, boardSquare.y))
							{
								Color white = Color.white;
								white.a = 0.5f;
								Gizmos.color = white;
								Vector3 size = 0.05f * Board.Get().squareSize * Vector3.one;
								size.y = 0.1f;
								Gizmos.DrawWireCube(boardSquare.ToVector3(), size);
							}
							else
							{
								Color red = Color.red;
								red.a = 0.5f;
								Gizmos.color = red;
								Vector3 size2 = 0.05f * Board.Get().squareSize * Vector3.one;
								size2.y = 0.1f;
								Gizmos.DrawWireCube(boardSquare.ToVector3(), size2);
							}
						}
					}
				}
			}
		}
	}

	private void UNetVersion()
	{
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
			SetSyncVar(value, ref m_selectedActionForTargeting, 0x10U);
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
			Debug.LogError("SyncList m_consumedStockCount called on server.");
			return;
		}
		((AbilityData)obj).m_consumedStockCount.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_stockRefreshCountdowns(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
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
			Debug.LogError("Command CmdRefillStocks called on client.");
			return;
		}
		((AbilityData)obj).CmdRefillStocks();
	}

	public void CallCmdClearCooldowns()
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdClearCooldowns called on server.");
			return;
		}
		if (isServer)
		{
			CmdClearCooldowns();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)kCmdCmdClearCooldowns);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, "CmdClearCooldowns");
	}

	public void CallCmdRefillStocks()
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdRefillStocks called on server.");
			return;
		}
		if (isServer)
		{
			CmdRefillStocks();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)kCmdCmdRefillStocks);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, "CmdRefillStocks");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			SyncListInt.WriteInstance(writer, m_cooldownsSync);
			SyncListInt.WriteInstance(writer, m_consumedStockCount);
			SyncListInt.WriteInstance(writer, m_stockRefreshCountdowns);
			SyncListInt.WriteInstance(writer, m_currentCardIds);
			writer.Write((int)m_selectedActionForTargeting);
			return true;
		}
		bool flag = false;
		if ((syncVarDirtyBits & 1U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, m_cooldownsSync);
		}
		if ((syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, m_consumedStockCount);
		}
		if ((syncVarDirtyBits & 4U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, m_stockRefreshCountdowns);
		}
		if ((syncVarDirtyBits & 8U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, m_currentCardIds);
		}
		if ((syncVarDirtyBits & 0x10U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)m_selectedActionForTargeting);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		var jsonLog = new List<string>();
		if (initialState)
		{
			SyncListInt.ReadReference(reader, m_cooldownsSync);
			SyncListInt.ReadReference(reader, m_consumedStockCount);
			SyncListInt.ReadReference(reader, m_stockRefreshCountdowns);
			SyncListInt.ReadReference(reader, m_currentCardIds);
			m_selectedActionForTargeting = (ActionType)reader.ReadInt32();
			jsonLog.Add($"\"cooldownsSync\":{DefaultJsonSerializer.Serialize(m_cooldownsSync)}");
			jsonLog.Add($"\"consumedStockCount\":{DefaultJsonSerializer.Serialize(m_consumedStockCount)}");
			jsonLog.Add($"\"stockRefreshCountdowns\":{DefaultJsonSerializer.Serialize(m_stockRefreshCountdowns)}");
			jsonLog.Add($"\"currentCardIds\":{DefaultJsonSerializer.Serialize(m_currentCardIds)}");
			jsonLog.Add($"\"selectedActionForTargeting\":\"{m_selectedActionForTargeting}\"");
			Log.Info($"[JSON] {{\"abilityData\":{{{String.Join(",", jsonLog.ToArray())}}}}}");
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListInt.ReadReference(reader, m_cooldownsSync);
			jsonLog.Add($"\"cooldownsSync\":{DefaultJsonSerializer.Serialize(m_cooldownsSync)}");
		}
		if ((num & 2) != 0)
		{
			SyncListInt.ReadReference(reader, m_consumedStockCount);
			jsonLog.Add($"\"consumedStockCount\":{DefaultJsonSerializer.Serialize(m_consumedStockCount)}");
		}
		if ((num & 4) != 0)
		{
			SyncListInt.ReadReference(reader, m_stockRefreshCountdowns);
			jsonLog.Add($"\"stockRefreshCountdowns\":{DefaultJsonSerializer.Serialize(m_stockRefreshCountdowns)}");
		}
		if ((num & 8) != 0)
		{
			SyncListInt.ReadReference(reader, m_currentCardIds);
			jsonLog.Add($"\"currentCardIds\":{DefaultJsonSerializer.Serialize(m_currentCardIds)}");
		}
		if ((num & 0x10) != 0)
		{
			m_selectedActionForTargeting = (ActionType)reader.ReadInt32();
			jsonLog.Add($"\"selectedActionForTargeting\":\"{m_selectedActionForTargeting}\"");
		}
		Log.Info($"[JSON] {{\"abilityData\":{{{String.Join(",", jsonLog.ToArray())}}}}}");
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
				return 0;
			}
			return m_cooldownRemaining;
		}

		public void SetCooldownRemaining(int remaining)
		{
			if (m_cooldownRemaining != remaining)
			{
				m_cooldownRemaining = remaining;
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
}
