using System;
using UnityEngine;

[Serializable]
public class UseCardChatter : ScriptableObject, IChatterData
{
	public ChatterData m_baseData = new ChatterData();

	public ChatterData GetCommonData()
	{
		return this.m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.CardUsed;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (!ChatterData.ShouldPlayChatter(this, eventType, args, component))
		{
			return false;
		}
		GameEventManager.CardUsedArgs cardUsedArgs = args as GameEventManager.CardUsedArgs;
		if (cardUsedArgs == null)
		{
			Log.Error("Missing args for card use game event.", new object[0]);
			return false;
		}
		if (cardUsedArgs.userActor != component.gameObject.GetComponent<ActorData>())
		{
			return false;
		}
		return true;
	}
}
