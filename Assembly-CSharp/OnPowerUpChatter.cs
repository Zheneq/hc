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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					GameEventManager.PowerUpActivatedArgs powerUpActivatedArgs = args as GameEventManager.PowerUpActivatedArgs;
					if (!(powerUpActivatedArgs.byActor == null))
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!(GameFlowData.Get().activeOwnedActorData != powerUpActivatedArgs.byActor))
						{
							if (m_requiredPowerupType != 0)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
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
			}
		}
		return false;
	}
}
