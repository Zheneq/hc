using System;
using UnityEngine;

[Serializable]
public class OnLockedChatter : ScriptableObject, IChatterData
{
	public ChatterData m_baseData = new ChatterData();

	public ChatterData GetCommonData()
	{
		return this.m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.CharacterLocked;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (GameFlowData.Get().activeOwnedActorData != component.gameObject.GetComponent<ActorData>())
		{
			return false;
		}
		return ChatterData.ShouldPlayChatter(this, eventType, args, component);
	}
}
