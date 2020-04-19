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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(OnPowerUpChatter.ShouldPlayChatter(GameEventManager.EventType, GameEventManager.GameEventArgs, ChatterComponent)).MethodHandle;
			}
			GameEventManager.PowerUpActivatedArgs powerUpActivatedArgs = args as GameEventManager.PowerUpActivatedArgs;
			if (!(powerUpActivatedArgs.byActor == null))
			{
				for (;;)
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
					if (this.m_requiredPowerupType != PowerUp.PowerUpCategory.NoCategory)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.m_requiredPowerupType != powerUpActivatedArgs.powerUp.m_chatterCategory)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							return false;
						}
					}
					return ChatterData.ShouldPlayChatter(this, eventType, args, component);
				}
				for (;;)
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
		return false;
	}
}
