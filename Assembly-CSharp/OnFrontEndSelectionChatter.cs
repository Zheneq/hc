﻿using System;
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
			return false;
		}
		return !(UIFrontEnd.GetVisibleCharacters() == null) && !(UIFrontEnd.GetVisibleCharacters().CharacterResourceLinkInSlot(0) != component.GetCharacterResourceLink());
	}
}
