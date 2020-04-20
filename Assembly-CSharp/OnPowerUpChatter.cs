using System;
using UnityEngine;

[Serializable]
public class OnPowerUpChatter : ScriptableObject, IChatterData
{
	public ChatterData m_baseData = new ChatterData();

	public PowerUp.PowerUpCategory m_requiredPowerupType;

	public ChatterData GetCommonData()
	{
		return this.m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.PowerUpActivated;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (args is GameEventManager.PowerUpActivatedArgs)
		{
			GameEventManager.PowerUpActivatedArgs powerUpActivatedArgs = args as GameEventManager.PowerUpActivatedArgs;
			if (!(powerUpActivatedArgs.byActor == null))
			{
				if (!(GameFlowData.Get().activeOwnedActorData != powerUpActivatedArgs.byActor))
				{
					if (this.m_requiredPowerupType != PowerUp.PowerUpCategory.NoCategory)
					{
						if (this.m_requiredPowerupType != powerUpActivatedArgs.powerUp.m_chatterCategory)
						{
							return false;
						}
					}
					return ChatterData.ShouldPlayChatter(this, eventType, args, component);
				}
			}
			return false;
		}
		return false;
	}
}
