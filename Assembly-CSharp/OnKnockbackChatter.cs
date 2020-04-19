using System;
using UnityEngine;

[Serializable]
public class OnKnockbackChatter : ScriptableObject, IChatterData
{
	public ChatterData m_baseData = new ChatterData();

	public ChatterData GetCommonData()
	{
		return this.m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.ActorKnockback;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (!ChatterData.ShouldPlayChatter(this, eventType, args, component))
		{
			return false;
		}
		GameEventManager.ActorKnockback actorKnockback = args as GameEventManager.ActorKnockback;
		return !(actorKnockback.m_target != component.gameObject.GetComponent<ActorData>());
	}
}
