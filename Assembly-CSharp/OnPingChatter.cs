using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(order = 999, fileName = "CharacterOnPingChatter", menuName = "On Ping Chatter")]
public class OnPingChatter : ScriptableObject, IChatterData
{
	public ChatterData m_baseData = new ChatterData();

	public ActorController.PingType m_pingType;

	public ChatterData GetCommonData()
	{
		return m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.ActorPing;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (!ChatterData.ShouldPlayChatter(this, eventType, args, component))
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
		ActorData x = null;
		if (component != null)
		{
			if (component.gameObject != null)
			{
				x = component.gameObject.GetComponent<ActorData>();
			}
		}
		if (x == null)
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
		GameEventManager.ActorPingEventArgs actorPingEventArgs = args as GameEventManager.ActorPingEventArgs;
		if (actorPingEventArgs == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Log.Error("Missing args for Actor Ping game event.");
					return false;
				}
			}
		}
		if (x != actorPingEventArgs.byActor)
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
		if (actorPingEventArgs.pingType != m_pingType)
		{
			while (true)
			{
				switch (7)
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
