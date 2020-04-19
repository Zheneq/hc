using System;
using UnityEngine;

[Serializable]
public class OnHealBuffChatter : ScriptableObject, IChatterData
{
	public ChatterData m_baseData = new ChatterData();

	public bool m_onSelfBeingHealed = true;

	public ChatterData GetCommonData()
	{
		return this.m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.CharacterHealedOrBuffed;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (!ChatterData.ShouldPlayChatter(this, eventType, args, component))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(OnHealBuffChatter.ShouldPlayChatter(GameEventManager.EventType, GameEventManager.GameEventArgs, ChatterComponent)).MethodHandle;
			}
			return false;
		}
		GameEventManager.CharacterHealBuffArgs characterHealBuffArgs = args as GameEventManager.CharacterHealBuffArgs;
		if (characterHealBuffArgs == null)
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
			Log.Error("Missing args for heal/buff game event.", new object[0]);
			return false;
		}
		if (!(characterHealBuffArgs.casterActor == null))
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!(characterHealBuffArgs.casterActor == characterHealBuffArgs.targetCharacter))
			{
				if (this.m_onSelfBeingHealed)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (characterHealBuffArgs.targetCharacter != component.gameObject.GetComponent<ActorData>())
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						return false;
					}
				}
				return true;
			}
			for (;;)
			{
				switch (1)
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
