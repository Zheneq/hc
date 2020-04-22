using System;
using UnityEngine;

[Serializable]
public class OnPowerUpChatter : ScriptableObject, IChatterData
{
	public ChatterData m_baseData = new ChatterData();

	public PowerUp.PowerUpCategory m_requiredPowerupType;

	public ChatterData GetCommonData()
	{
		return m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.PowerUpActivated;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (args is GameEventManager.PowerUpActivatedArgs)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					GameEventManager.PowerUpActivatedArgs powerUpActivatedArgs = args as GameEventManager.PowerUpActivatedArgs;
					if (!(powerUpActivatedArgs.byActor == null))
					{
						if (!(GameFlowData.Get().activeOwnedActorData != powerUpActivatedArgs.byActor))
						{
							if (m_requiredPowerupType != 0)
							{
								if (m_requiredPowerupType != powerUpActivatedArgs.powerUp.m_chatterCategory)
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
							}
							return ChatterData.ShouldPlayChatter(this, eventType, args, component);
						}
					}
					return false;
				}
				}
			}
		}
		return false;
	}
}
