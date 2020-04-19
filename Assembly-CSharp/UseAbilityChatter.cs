using System;
using UnityEngine;

[Serializable]
public class UseAbilityChatter : ScriptableObject, IChatterData
{
	public ChatterData m_baseData = new ChatterData();

	public Ability m_ability;

	public ActorData m_abilityOwner;

	public ChatterData GetCommonData()
	{
		return this.m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.AbilityUsed;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (!ChatterData.ShouldPlayChatter(this, eventType, args, component))
		{
			return false;
		}
		GameEventManager.AbilityUseArgs abilityUseArgs = args as GameEventManager.AbilityUseArgs;
		if (abilityUseArgs == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UseAbilityChatter.ShouldPlayChatter(GameEventManager.EventType, GameEventManager.GameEventArgs, ChatterComponent)).MethodHandle;
			}
			Log.Error("Missing args for ability game event.", new object[0]);
			return false;
		}
		if (abilityUseArgs.userActor != component.gameObject.GetComponent<ActorData>())
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
		if (abilityUseArgs.ability == null)
		{
			return false;
		}
		if (this.m_ability != null)
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
			if (this.m_ability.GetType() != abilityUseArgs.ability.GetType())
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
		}
		return !TheatricsManager.Get().IsCinematicsRequestedInCurrentPhase(abilityUseArgs.userActor, this.m_ability);
	}
}
