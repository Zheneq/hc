using System;
using UnityEngine;

[Serializable]
public class OnRespawnedChatter : ScriptableObject, IChatterData
{
	public ChatterData m_baseData = new ChatterData();

	public ChatterData GetCommonData()
	{
		return this.m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.CharacterRespawn;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (!ChatterData.ShouldPlayChatter(this, eventType, args, component))
		{
			return false;
		}
		if (GameFlowData.Get() != null && GameFlowData.Get().gameState == GameState.EndingGame)
		{
			return false;
		}
		GameEventManager.CharacterRespawnEventArgs characterRespawnEventArgs = args as GameEventManager.CharacterRespawnEventArgs;
		if (characterRespawnEventArgs.respawningCharacter != component.gameObject.GetComponent<ActorData>())
		{
			return false;
		}
		return true;
	}
}
