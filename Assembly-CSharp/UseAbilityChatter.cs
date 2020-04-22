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
		return m_baseData;
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Log.Error("Missing args for ability game event.");
					return false;
				}
			}
		}
		if (abilityUseArgs.userActor != component.gameObject.GetComponent<ActorData>())
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
		if (abilityUseArgs.ability == null)
		{
			return false;
		}
		if (m_ability != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_ability.GetType() != abilityUseArgs.ability.GetType())
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
		}
		if (TheatricsManager.Get().IsCinematicsRequestedInCurrentPhase(abilityUseArgs.userActor, m_ability))
		{
			return false;
		}
		return true;
	}
}
