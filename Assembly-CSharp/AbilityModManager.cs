using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AbilityModManager : NetworkBehaviour
{
	private static AbilityModManager s_instance;

	private Dictionary<Type, List<AbilityMod>> m_abilityTypeToMods = new Dictionary<Type, List<AbilityMod>>();

	public bool ShowDebugGUI
	{
		get;
		set;
	}

	internal static AbilityModManager Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		if (!(s_instance == null))
		{
			return;
		}
		while (true)
		{
			s_instance = this;
			return;
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
		if (newState == GameState.BothTeams_Decision)
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
		switch (newState)
		{
		case GameState.BothTeams_Resolve:
			while (true)
			{
				ShowDebugGUI = false;
				return;
			}
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
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[Client] function 'System.Void AbilityModManager::OnActiveOwnedActorChange(ActorData)' called on server");
					return;
				}
			}
		}
		if (!(activeActor != null))
		{
			return;
		}
		while (true)
		{
			LoadAvailableModsForActor(activeActor);
			return;
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
			using (List<AbilityMod>.Enumerator enumerator = m_abilityTypeToMods[ability.GetType()].GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityMod current = enumerator.Current;
					list.Add(current);
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return list;
					}
				}
			}
		}
		return list;
	}

	public AbilityMod GetDefaultModForAbility(Ability ability)
	{
		if (ability != null)
		{
			if (!m_abilityTypeToMods.ContainsKey(ability.GetType()))
			{
				LoadAvailableModsForAbility(ability);
			}
			using (List<AbilityMod>.Enumerator enumerator = m_abilityTypeToMods[ability.GetType()].GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityMod current = enumerator.Current;
					if (current.m_availableInGame)
					{
						if (current.m_defaultEquip)
						{
							return current;
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
		List<AbilityMod> availableModsForAbility = GetAvailableModsForAbility(ability);
		using (List<AbilityMod>.Enumerator enumerator = availableModsForAbility.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AbilityMod current = enumerator.Current;
				if (current != null && current.m_abilityScopeId == abilityScopeId)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return current;
						}
					}
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
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
					{
						Ability abilityOfActionType = component.GetAbilityOfActionType((AbilityData.ActionType)actionTypeInt);
						return GetAbilityModForAbilityById(abilityOfActionType, abilityScopeId);
					}
					}
				}
			}
		}
		return null;
	}

	private void LoadAvailableModsForActor(ActorData actor)
	{
		if (!(actor != null) || !(actor.GetAbilityData() != null))
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i <= 4; i++)
			{
				Ability abilityOfActionType = actor.GetAbilityData().GetAbilityOfActionType((AbilityData.ActionType)i);
				LoadAvailableModsForAbility(abilityOfActionType);
			}
			return;
		}
	}

	private void LoadAvailableModsForAbility(Ability ability)
	{
		if (ability == null)
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
		Type type = ability.GetType();
		if (m_abilityTypeToMods.ContainsKey(type))
		{
			m_abilityTypeToMods[type].Clear();
		}
		else
		{
			m_abilityTypeToMods[type] = new List<AbilityMod>();
		}
		List<AbilityMod> availableModsForAbilityType = AbilityModHelper.GetAvailableModsForAbilityType(type);
		using (List<AbilityMod>.Enumerator enumerator = availableModsForAbilityType.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AbilityMod current = enumerator.Current;
				m_abilityTypeToMods[type].Add(current);
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

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result = default(bool);
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
