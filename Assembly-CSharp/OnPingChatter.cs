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
			return false;
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
			return false;
		}
		GameEventManager.ActorPingEventArgs actorPingEventArgs = args as GameEventManager.ActorPingEventArgs;
		if (actorPingEventArgs == null)
		{
			Log.Error("Missing args for Actor Ping game event.", new object[0]);
			return false;
		}
		if (x != actorPingEventArgs.byActor)
		{
			return false;
		}
		if (actorPingEventArgs.pingType != this.m_pingType)
		{
			return false;
		}
		return true;
	}
}
