// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AbilityModManager : NetworkBehaviour
{
	private static AbilityModManager s_instance;

	// reactor
	private Dictionary<Type, List<AbilityMod>> m_abilityTypeToMods = new Dictionary<Type, List<AbilityMod>>();
	// rogues
	// private List<List<AbilityMod>> m_abilityMods = new List<List<AbilityMod>> { null, null, null, null, null };

	public bool ShowDebugGUI { get; set; }

	internal static AbilityModManager Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		if (s_instance == null)
		{
			s_instance = this;
		}
	}

	private void Start()
	{
		GameFlowData.s_onGameStateChanged += OnGameStateChanged;
	}

	public override void OnStartClient()
	{
		GameFlowData.s_onActiveOwnedActorChange += OnActiveOwnedActorChange;
	}

	private void OnDestroy()
	{
		GameFlowData.s_onGameStateChanged -= OnGameStateChanged;
		GameFlowData.s_onActiveOwnedActorChange -= OnActiveOwnedActorChange;
		s_instance = null;
	}

	private void OnGameStateChanged(GameState newState)
	{
		switch (newState)
		{
			case GameState.BothTeams_Decision: // GameFlowData.IsDecisionStateEnum(newState) in rogues
#if SERVER
				// added in rogues
				if (GameFlowData.Get().CurrentTurn != 1)
				{
					break;
				}
				foreach (ActorData actorData in GameFlowData.Get().GetActors())
				{
					// rogues
					// if (actorData.GetTeam() != GameFlowData.Get().ActingTeam)
					// {
					// 	continue;
					// }
					
					// custom
					ServerPlayerState playerState = ServerGameManager.Get().GetPlayerStateByPlayerId(actorData.PlayerIndex);
					if (playerState != null)
					{
						actorData.SetupAbilityMods(playerState.PlayerInfo.CharacterMods);
					}
					else if (actorData.GetAccountId() > 0) // actor is a proxy
					{
						playerState = ServerGameManager.Get().GetPlayerStateByAccountId(actorData.GetAccountId());
						if (playerState != null)
						{
							foreach (ServerPlayerInfo proxy in playerState.PlayerInfo.ProxyPlayerInfos)
							{
								if (proxy.PlayerId == actorData.PlayerIndex)
								{
									actorData.SetupAbilityMods(proxy.CharacterMods);
									break;
								}
							}
						}
					}
					// rogues
					// PersistedCharacterData persistedCharacterData = ClientGameManager.Get().GetPlayerCharacterData(actorData.m_characterType);
					// if (actorData.GetTeam() == Team.TeamA)
					// {
					// 	actorData.SetupAbilityGear();
					// }

					// rogues
					// AbilityData abilityData = actorData.GetAbilityData();
					// foreach (Ability ability in abilityData.GetAbilitiesAsList())
					// {
					// 	if (abilityData.GetActionTypeOfAbility(ability) != AbilityData.ActionType.INVALID_ACTION)
					// 	{
					// 		ability.ClearAbilityMod(actorData);
					// 	}
					// }
				}
#endif
				break;
			case GameState.BothTeams_Resolve:
				ShowDebugGUI = false;
				break;
			case GameState.EndingGame:
				ShowDebugGUI = false;
				// reactor
				m_abilityTypeToMods.Clear();
				// rogues
				// foreach (List<AbilityMod> list in m_abilityMods)
				// {
				// 	if (list != null)
				// 	{
				// 		list.Clear();
				// 	}
				// }
				break;
		}
	}

	[Client]
	private void OnActiveOwnedActorChange(ActorData activeActor)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void AbilityModManager::OnActiveOwnedActorChange(ActorData)' called on server");
			return;
		}

		if (activeActor != null)
		{
			LoadAvailableModsForActor(activeActor);
		}
	}

	public List<AbilityMod> GetAvailableModsForAbility(Ability ability)  // , int abilityIndex in rogues
	{
		List<AbilityMod> list = new List<AbilityMod>();
		if (ability != null)  //  && abilityIndex < m_abilityMods.Count in rogues
		{
			// reactor
			if (!m_abilityTypeToMods.ContainsKey(ability.GetType()))
			// rogues
			// if (m_abilityMods[abilityIndex] == null)
			{
				LoadAvailableModsForAbility(ability); // , abilityIndex in rogues
			}
			// reactor
			foreach (AbilityMod mod in m_abilityTypeToMods[ability.GetType()])
			// rogues
			// foreach (AbilityMod mod in m_abilityMods[abilityIndex])
			{
				list.Add(mod);
			}
		}
		return list;
	}

	public AbilityMod GetDefaultModForAbility(Ability ability)  // , int abilityIndex in rogues
	{
		if (ability == null)  // || abilityIndex >= m_abilityMods.Count in rogues
		{
			return null;
		}
		// reactor
		if (!m_abilityTypeToMods.ContainsKey(ability.GetType()))
		// rogues
		// if (m_abilityMods[abilityIndex] == null)
		{
			LoadAvailableModsForAbility(ability);  // , abilityIndex in rogues
		}
		// reactor
		foreach (AbilityMod mod in m_abilityTypeToMods[ability.GetType()])
		// rogues
		// foreach (AbilityMod mod in m_abilityMods[abilityIndex])
		{
			if (mod.m_availableInGame && mod.m_defaultEquip)
			{
				return mod;
			}
		}
		return null;
	}

	// rogues
	// public Gear GetDefaultGearForAbility(Ability ability)
	// {
	// 	ability != null;
	// 	return null;
	// }

	public AbilityMod GetAbilityModForAbilityById(Ability ability, int abilityScopeId)  // (Ability ability, int abilityIndex, int abilityScopeId) in rogues
	{
		if (ability == null)
		{
			return null;
		}
		foreach (AbilityMod mod in GetAvailableModsForAbility(ability))  // , abilityIndex in rogues
		{
			if (mod != null && mod.m_abilityScopeId == abilityScopeId)
			{
				return mod;
			}
		}
		return null;
	}

	public AbilityMod GetAbilityModOnActor(int actorIndex, int actionTypeInt, int abilityScopeId)
	{
		ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
		if (actorData != null)
		{
			AbilityData component = actorData.GetComponent<AbilityData>();
			if (component != null)
			{
				Ability abilityOfActionType = component.GetAbilityOfActionType((AbilityData.ActionType)actionTypeInt);
				return GetAbilityModForAbilityById(abilityOfActionType, abilityScopeId);  // (abilityOfActionType, actionTypeInt, abilityScopeId) in rogues
			}
		}
		return null;
	}

	private void LoadAvailableModsForActor(ActorData actor)
	{
		if (actor == null || actor.GetAbilityData() == null)
		{
			return;
		}
		for (AbilityData.ActionType i = AbilityData.ActionType.ABILITY_0; i <= AbilityData.ActionType.ABILITY_4; i++)
		{
			Ability abilityOfActionType = actor.GetAbilityData().GetAbilityOfActionType(i);
			LoadAvailableModsForAbility(abilityOfActionType);  // , (int)i in rogues
		}
	}

	// reactor
	private void LoadAvailableModsForAbility(Ability ability)
	{
		if (ability == null)
		{
			return;
		}
		Type type = ability.GetType();
		if (m_abilityTypeToMods.ContainsKey(type))
		{
			m_abilityTypeToMods[type].Clear();
		}
		else
		{
			m_abilityTypeToMods[type] = new List<AbilityMod>();
		}
		foreach (AbilityMod mod in AbilityModHelper.GetAvailableModsForAbilityType(type))
		{
			m_abilityTypeToMods[type].Add(mod);
		}
	}
	// rogues
	// private void LoadAvailableModsForAbility(Ability ability, int abilityIndex)
	// {
	// 	if (ability == null || abilityIndex >= m_abilityMods.Count)
	// 	{
	// 		return;
	// 	}
	// 	if (m_abilityMods[abilityIndex] != null)
	// 	{
	// 		m_abilityMods[abilityIndex].Clear();
	// 	}
	// 	else
	// 	{
	// 		m_abilityMods[abilityIndex] = new List<AbilityMod>();
	// 	}
	// 	foreach (AbilityMod item in AbilityModHelper.GetAvailableModsForAbility(ability))
	// 	{
	// 		m_abilityMods[abilityIndex].Add(item);
	// 	}
	// }

	private void UNetVersion()
	{
	}

	// removed in rogues
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		return false;
	}

	// removed in rogues
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
