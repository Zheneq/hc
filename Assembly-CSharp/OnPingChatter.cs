using System;
using UnityEngine;

[CreateAssetMenu(order = 0x3E7, fileName = "CharacterOnPingChatter", menuName = "On Ping Chatter")]
[Serializable]
public class OnPingChatter : ScriptableObject, IChatterData
{
	public ChatterData m_baseData = new ChatterData();

	public ActorController.PingType m_pingType;

	public ChatterData GetCommonData()
	{
		return this.m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.ActorPing;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (!ChatterData.ShouldPlayChatter(this, eventType, args, component))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(OnPingChatter.ShouldPlayChatter(GameEventManager.EventType, GameEventManager.GameEventArgs, ChatterComponent)).MethodHandle;
			}
			return false;
		}
		ActorData x = null;
		if (component != null)
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
			if (component.gameObject != null)
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
				x = component.gameObject.GetComponent<ActorData>();
			}
		}
		if (x == null)
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
		GameEventManager.ActorPingEventArgs actorPingEventArgs = args as GameEventManager.ActorPingEventArgs;
		if (actorPingEventArgs == null)
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
			Log.Error("Missing args for Actor Ping game event.", new object[0]);
			return false;
		}
		if (x != actorPingEventArgs.byActor)
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
			return false;
		}
		if (actorPingEventArgs.pingType != this.m_pingType)
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
			return false;
		}
		return true;
	}
}
