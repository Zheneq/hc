using System;
using UnityEngine;

[Serializable]
public class OnKillingBlowChatter : ScriptableObject, IChatterData
{
	public ChatterData m_baseData = new ChatterData();

	public ChatterData GetCommonData()
	{
		return this.m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.ClientRagdollTriggerHit;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (!ChatterData.ShouldPlayChatter(this, eventType, args, component))
		{
			return false;
		}
		GameEventManager.CharacterRagdollHitEventArgs characterRagdollHitEventArgs = args as GameEventManager.CharacterRagdollHitEventArgs;
		if (characterRagdollHitEventArgs != null)
		{
			if (!(characterRagdollHitEventArgs.m_triggeringActor != component.gameObject.GetComponent<ActorData>()))
			{
				return true;
			}
		}
		return false;
	}
}
