using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OnKilledChatter : ScriptableObject, IChatterData
{
	public ChatterData m_baseData = new ChatterData();

	public OnKilledChatter.TypeOfKill m_KillType;

	public ChatterData GetCommonData()
	{
		return this.m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.CharacterVisualDeath;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (!ChatterData.ShouldPlayChatter(this, eventType, args, component))
		{
			return false;
		}
		ActorData actorData = null;
		if (component != null)
		{
			if (component.gameObject != null)
			{
				actorData = component.gameObject.GetComponent<ActorData>();
			}
		}
		if (actorData == null)
		{
			return false;
		}
		GameEventManager.CharacterDeathEventArgs characterDeathEventArgs = args as GameEventManager.CharacterDeathEventArgs;
		OnKilledChatter.TypeOfKill killType = this.m_KillType;
		if (killType != OnKilledChatter.TypeOfKill.OnSelfKilled)
		{
			if (killType != OnKilledChatter.TypeOfKill.OnKilledOtherAny)
			{
				if (killType != OnKilledChatter.TypeOfKill.OnKilledSpecificCharacter)
				{
				}
				else
				{
					List<ActorData> contributorsToKillOnClient = GameFlowData.Get().GetContributorsToKillOnClient(characterDeathEventArgs.deadCharacter, true);
					if (!contributorsToKillOnClient.Contains(actorData))
					{
						return false;
					}
				}
			}
			else
			{
				if (characterDeathEventArgs.deadCharacter == actorData)
				{
					return false;
				}
				List<ActorData> contributorsToKillOnClient2 = GameFlowData.Get().GetContributorsToKillOnClient(characterDeathEventArgs.deadCharacter, true);
				if (!contributorsToKillOnClient2.Contains(actorData))
				{
					return false;
				}
			}
		}
		else if (characterDeathEventArgs.deadCharacter != actorData)
		{
			return false;
		}
		return true;
	}

	public enum TypeOfKill
	{
		OnSelfKilled,
		OnSelfKilledBySpecificCharacter,
		OnKilledOtherAny,
		OnKilledSpecificCharacter
	}
}
