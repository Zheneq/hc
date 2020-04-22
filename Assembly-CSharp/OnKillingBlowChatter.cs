using System;
using UnityEngine;

[Serializable]
public class OnKillingBlowChatter : ScriptableObject, IChatterData
{
	public ChatterData m_baseData = new ChatterData();

	public ChatterData GetCommonData()
	{
		return m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.ClientRagdollTriggerHit;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (!ChatterData.ShouldPlayChatter(this, eventType, args, component))
		{
			while (true)
			{
				switch (5)
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
		GameEventManager.CharacterRagdollHitEventArgs characterRagdollHitEventArgs = args as GameEventManager.CharacterRagdollHitEventArgs;
		if (characterRagdollHitEventArgs != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!(characterRagdollHitEventArgs.m_triggeringActor != component.gameObject.GetComponent<ActorData>()))
			{
				return true;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}
}
