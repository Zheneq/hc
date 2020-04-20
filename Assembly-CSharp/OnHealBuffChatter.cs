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
			return false;
		}
		GameEventManager.CharacterHealBuffArgs characterHealBuffArgs = args as GameEventManager.CharacterHealBuffArgs;
		if (characterHealBuffArgs == null)
		{
			Log.Error("Missing args for heal/buff game event.", new object[0]);
			return false;
		}
		if (!(characterHealBuffArgs.casterActor == null))
		{
			if (!(characterHealBuffArgs.casterActor == characterHealBuffArgs.targetCharacter))
			{
				if (this.m_onSelfBeingHealed)
				{
					if (characterHealBuffArgs.targetCharacter != component.gameObject.GetComponent<ActorData>())
					{
						return false;
					}
				}
				return true;
			}
		}
		return false;
	}
}
