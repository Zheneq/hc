using System;
using UnityEngine;

[Serializable]
public class OnFrontEndSelectionChatter : ScriptableObject, IChatterData
{
	public ChatterData m_baseData = new ChatterData();

	public ChatterData GetCommonData()
	{
		return this.m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.FrontEndSelectionChatterCue;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (!ChatterData.ShouldPlayChatter(this, eventType, args, component))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(OnFrontEndSelectionChatter.ShouldPlayChatter(GameEventManager.EventType, GameEventManager.GameEventArgs, ChatterComponent)).MethodHandle;
			}
			return false;
		}
		return !(UIFrontEnd.GetVisibleCharacters() == null) && !(UIFrontEnd.GetVisibleCharacters().CharacterResourceLinkInSlot(0) != component.GetCharacterResourceLink());
	}
}
