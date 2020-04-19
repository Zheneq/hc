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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModManager.Awake()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModManager.OnGameStateChanged(GameState)).MethodHandle;
			}
		}
		else if (newState == GameState.BothTeams_Resolve)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModManager.OnActiveOwnedActorChange(ActorData)).MethodHandle;
			}
			Debug.LogWarning("[Client] function 'System.Void AbilityModManager::OnActiveOwnedActorChange(ActorData)' called on server");
			return;
		}
		if (activeActor != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModManager.GetAvailableModsForAbility(Ability)).MethodHandle;
				}
				this.LoadAvailableModsForAbility(ability);
			}
			using (List<AbilityMod>.Enumerator enumerator = this.m_abilityTypeToMods[ability.GetType()].GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityMod item = enumerator.Current;
					list.Add(item);
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
		}
		return list;
	}

	public AbilityMod GetDefaultModForAbility(Ability ability)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModManager.GetDefaultModForAbility(Ability)).MethodHandle;
			}
			if (!this.m_abilityTypeToMods.ContainsKey(ability.GetType()))
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
				this.LoadAvailableModsForAbility(ability);
			}
			using (List<AbilityMod>.Enumerator enumerator = this.m_abilityTypeToMods[ability.GetType()].GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityMod abilityMod = enumerator.Current;
					if (abilityMod.m_availableInGame)
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
						if (abilityMod.m_defaultEquip)
						{
							return abilityMod;
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModManager.GetAbilityModForAbilityById(Ability, int)).MethodHandle;
					}
					return abilityMod;
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
		return null;
	}

	public AbilityMod GetAbilityModOnActor(int actorIndex, int actionTypeInt, int abilityScopeId)
	{
		ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
		if (actorData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModManager.GetAbilityModOnActor(int, int, int)).MethodHandle;
			}
			AbilityData component = actorData.GetComponent<AbilityData>();
			if (component != null)
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
				Ability abilityOfActionType = component.GetAbilityOfActionType((AbilityData.ActionType)actionTypeInt);
				return this.GetAbilityModForAbilityById(abilityOfActionType, abilityScopeId);
			}
		}
		return null;
	}

	private void LoadAvailableModsForActor(ActorData actor)
	{
		if (actor != null && actor.\u000E() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModManager.LoadAvailableModsForActor(ActorData)).MethodHandle;
			}
			for (int i = 0; i <= 4; i++)
			{
				Ability abilityOfActionType = actor.\u000E().GetAbilityOfActionType((AbilityData.ActionType)i);
				this.LoadAvailableModsForAbility(abilityOfActionType);
			}
		}
	}

	private void LoadAvailableModsForAbility(Ability ability)
	{
		if (ability == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModManager.LoadAvailableModsForAbility(Ability)).MethodHandle;
			}
			return;
		}
		Type type = ability.GetType();
		if (this.m_abilityTypeToMods.ContainsKey(type))
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

	public bool ShowDebugGUI { get; set; }

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result;
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
