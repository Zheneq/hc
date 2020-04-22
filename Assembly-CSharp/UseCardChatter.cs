using System;
using UnityEngine;

[Serializable]
public class UseCardChatter : ScriptableObject, IChatterData
{
	public ChatterData m_baseData = new ChatterData();

	public ChatterData GetCommonData()
	{
		return m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.CardUsed;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (!ChatterData.ShouldPlayChatter(this, eventType, args, component))
		{
			while (true)
			{
				switch (1)
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
		GameEventManager.CardUsedArgs cardUsedArgs = args as GameEventManager.CardUsedArgs;
		if (cardUsedArgs == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					Log.Error("Missing args for card use game event.");
					return false;
				}
			}
		}
		if (cardUsedArgs.userActor != component.gameObject.GetComponent<ActorData>())
		{
			while (true)
			{
				switch (6)
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
