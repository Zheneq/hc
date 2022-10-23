using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AbilityModManager : NetworkBehaviour
{
	private static AbilityModManager s_instance;

	private Dictionary<Type, List<AbilityMod>> m_abilityTypeToMods = new Dictionary<Type, List<AbilityMod>>();

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
			case GameState.BothTeams_Decision:
				break;
			case GameState.BothTeams_Resolve:
				ShowDebugGUI = false;
				break;
			case GameState.EndingGame:
				ShowDebugGUI = false;
				m_abilityTypeToMods.Clear();
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

	public List<AbilityMod> GetAvailableModsForAbility(Ability ability)
	{
		List<AbilityMod> list = new List<AbilityMod>();
		if (ability != null)
		{
			if (!m_abilityTypeToMods.ContainsKey(ability.GetType()))
			{
				LoadAvailableModsForAbility(ability);
			}
			foreach (AbilityMod mod in m_abilityTypeToMods[ability.GetType()])
			{
				list.Add(mod);
			}
		}
		return list;
	}

	public AbilityMod GetDefaultModForAbility(Ability ability)
	{
		if (ability == null)
		{
			return null;
		}
		if (!m_abilityTypeToMods.ContainsKey(ability.GetType()))
		{
			LoadAvailableModsForAbility(ability);
		}
		foreach (AbilityMod mod in m_abilityTypeToMods[ability.GetType()])
		{
			if (mod.m_availableInGame && mod.m_defaultEquip)
			{
				return mod;
			}
		}
		return null;
	}

	public AbilityMod GetAbilityModForAbilityById(Ability ability, int abilityScopeId)
	{
		if (ability == null)
		{
			return null;
		}
		foreach (AbilityMod mod in GetAvailableModsForAbility(ability))
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
				return GetAbilityModForAbilityById(abilityOfActionType, abilityScopeId);
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
			LoadAvailableModsForAbility(abilityOfActionType);
		}
	}

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

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		return false;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
