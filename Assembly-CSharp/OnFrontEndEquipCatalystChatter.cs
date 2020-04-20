using System;
using UnityEngine;

[Serializable]
public class OnFrontEndEquipCatalystChatter : ScriptableObject, IChatterData
{
	public ChatterData m_baseData = new ChatterData();

	public ChatterData GetCommonData()
	{
		return this.m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.FrontEndEquipCatalyst;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (!ChatterData.ShouldPlayChatter(this, eventType, args, component))
		{
			return false;
		}
		if (!(UIFrontEnd.GetVisibleCharacters() == null))
		{
			if (!(UIFrontEnd.GetVisibleCharacters().CharacterResourceLinkInSlot(0) != component.GetCharacterResourceLink()))
			{
				return true;
			}
		}
		return false;
	}
}
