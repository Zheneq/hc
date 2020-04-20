using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AbilityModManager : NetworkBehaviour
{
	private static AbilityModManager s_instance;

	private Dictionary<Type, List<AbilityMod>> m_abilityTypeToMods = new Dictionary<Type, List<AbilityMod>>();

	internal static AbilityModManager Get()
	{
		return AbilityModManager.s_instance;
	}

	private void Awake()
	{
		if (AbilityModManager.s_instance == null)
		{
			AbilityModManager.s_instance = this;
		}
	}

	private void Start()
	{
		GameFlowData.s_onGameStateChanged += this.OnGameStateChanged;
	}

	public override void OnStartClient()
	{
		GameFlowData.s_onActiveOwnedActorChange += this.OnActiveOwnedActorChange;
	}

	private void OnDestroy()
	{
		GameFlowData.s_onGameStateChanged -= this.OnGameStateChanged;
		GameFlowData.s_onActiveOwnedActorChange -= this.OnActiveOwnedActorChange;
		AbilityModManager.s_instance = null;
	}

	private void OnGameStateChanged(GameState newState)
	{
		if (newState == GameState.BothTeams_Decision)
		{
		}
		else if (newState == GameState.BothTeams_Resolve)
		{
			this.ShowDebugGUI = false;
		}
		else if (newState == GameState.EndingGame)
		{
			this.ShowDebugGUI = false;
			this.m_abilityTypeToMods.Clear();
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
			this.LoadAvailableModsForActor(activeActor);
		}
	}

	public List<AbilityMod> GetAvailableModsForAbility(Ability ability)
	{
		List<AbilityMod> list = new List<AbilityMod>();
		if (ability != null)
		{
			if (!this.m_abilityTypeToMods.ContainsKey(ability.GetType()))
			{
				this.LoadAvailableModsForAbility(ability);
			}
			using (List<AbilityMod>.Enumerator enumerator = this.m_abilityTypeToMods[ability.GetType()].GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityMod item = enumerator.Current;
					list.Add(item);
				}
			}
		}
		return list;
	}

	public AbilityMod GetDefaultModForAbility(Ability ability)
	{
		if (ability != null)
		{
			if (!this.m_abilityTypeToMods.ContainsKey(ability.GetType()))
			{
				this.LoadAvailableModsForAbility(ability);
			}
			using (List<AbilityMod>.Enumerator enumerator = this.m_abilityTypeToMods[ability.GetType()].GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityMod abilityMod = enumerator.Current;
					if (abilityMod.m_availableInGame)
					{
						if (abilityMod.m_defaultEquip)
						{
							return abilityMod;
						}
					}
				}
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
		List<AbilityMod> availableModsForAbility = this.GetAvailableModsForAbility(ability);
		using (List<AbilityMod>.Enumerator enumerator = availableModsForAbility.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AbilityMod abilityMod = enumerator.Current;
				if (abilityMod != null && abilityMod.m_abilityScopeId == abilityScopeId)
				{
					return abilityMod;
				}
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
				return this.GetAbilityModForAbilityById(abilityOfActionType, abilityScopeId);
			}
		}
		return null;
	}

	private void LoadAvailableModsForActor(ActorData actor)
	{
		if (actor != null && actor.GetAbilityData() != null)
		{
			for (int i = 0; i <= 4; i++)
			{
				Ability abilityOfActionType = actor.GetAbilityData().GetAbilityOfActionType((AbilityData.ActionType)i);
				this.LoadAvailableModsForAbility(abilityOfActionType);
			}
		}
	}

	private void LoadAvailableModsForAbility(Ability ability)
	{
		if (ability == null)
		{
			return;
		}
		Type type = ability.GetType();
		if (this.m_abilityTypeToMods.ContainsKey(type))
		{
			this.m_abilityTypeToMods[type].Clear();
		}
		else
		{
			this.m_abilityTypeToMods[type] = new List<AbilityMod>();
		}
		List<AbilityMod> availableModsForAbilityType = AbilityModHelper.GetAvailableModsForAbilityType(type);
		using (List<AbilityMod>.Enumerator enumerator = availableModsForAbilityType.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AbilityMod item = enumerator.Current;
				this.m_abilityTypeToMods[type].Add(item);
			}
		}
	}

	public bool ShowDebugGUI { get; set; }

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
