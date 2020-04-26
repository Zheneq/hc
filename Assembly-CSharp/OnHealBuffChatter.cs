using System;
using UnityEngine;

[Serializable]
public class OnHealBuffChatter : ScriptableObject, IChatterData
{
	public ChatterData m_baseData = new ChatterData();

	public bool m_onSelfBeingHealed = true;

	public ChatterData GetCommonData()
	{
		return m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.CharacterHealedOrBuffed;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (!ChatterData.ShouldPlayChatter(this, eventType, args, component))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		GameEventManager.CharacterHealBuffArgs characterHealBuffArgs = args as GameEventManager.CharacterHealBuffArgs;
		if (characterHealBuffArgs == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Log.Error("Missing args for heal/buff game event.");
					return false;
				}
			}
		}
		if (!(characterHealBuffArgs.casterActor == null))
		{
			if (!(characterHealBuffArgs.casterActor == characterHealBuffArgs.targetCharacter))
			{
				if (m_onSelfBeingHealed)
				{
					if (characterHealBuffArgs.targetCharacter != component.gameObject.GetComponent<ActorData>())
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								return false;
							}
						}
					}
				}
				return true;
			}
		}
		return false;
	}
}
