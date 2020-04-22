using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OnKilledChatter : ScriptableObject, IChatterData
{
	public enum TypeOfKill
	{
		OnSelfKilled,
		OnSelfKilledBySpecificCharacter,
		OnKilledOtherAny,
		OnKilledSpecificCharacter
	}

	public ChatterData m_baseData = new ChatterData();

	public TypeOfKill m_KillType;

	public ChatterData GetCommonData()
	{
		return m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.CharacterVisualDeath;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (!ChatterData.ShouldPlayChatter(this, eventType, args, component))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return false;
				}
			}
		}
		ActorData actorData = null;
		if (component != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (component.gameObject != null)
			{
				actorData = component.gameObject.GetComponent<ActorData>();
			}
		}
		if (actorData == null)
		{
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
		GameEventManager.CharacterDeathEventArgs characterDeathEventArgs = args as GameEventManager.CharacterDeathEventArgs;
		TypeOfKill killType = m_KillType;
		if (killType != 0)
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
			if (killType != TypeOfKill.OnKilledOtherAny)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (killType != TypeOfKill.OnKilledSpecificCharacter)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
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
					while (true)
					{
						switch (4)
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
		else if (characterDeathEventArgs.deadCharacter != actorData)
		{
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
		return true;
	}
}
