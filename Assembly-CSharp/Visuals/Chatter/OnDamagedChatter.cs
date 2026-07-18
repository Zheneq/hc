using System;
using UnityEngine;

[Serializable]
public class OnDamagedChatter : ScriptableObject, IChatterData
{
	public enum HealthThreshMode
	{
		UsePercentage,
		UseDirectValue
	}

	public ChatterData m_baseData = new ChatterData();

	public bool m_onSelfDamage = true;

	[Separator("For getting hit that brings health below threhold", true)]
	public HealthThreshMode m_healthThresholdMode;

	public float m_healthThresholdPct = 0.25f;

	public int m_healthThresholdDirect = 20;

	[AudioEvent(false)]
	public string m_closeCallAudioEvent;

	public ChatterData GetCommonData()
	{
		return m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.ActorDamaged_Client;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (!ChatterData.ShouldPlayChatter(this, eventType, args, component))
		{
			while (true)
			{
				return false;
			}
		}
		GameEventManager.ActorHitHealthChangeArgs actorHitHealthChangeArgs = args as GameEventManager.ActorHitHealthChangeArgs;
		bool flag = actorHitHealthChangeArgs.m_target == component.gameObject.GetComponent<ActorData>();
		if (m_onSelfDamage)
		{
			if (!flag)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
		}
		GetCommonData().ClearAudioEventOverride();
		bool flag2;
		if (m_onSelfDamage && flag && !string.IsNullOrEmpty(m_closeCallAudioEvent))
		{
			int hitPointsAfterResolution = actorHitHealthChangeArgs.m_target.GetHitPointsToDisplay();
			int maxHitPoints = actorHitHealthChangeArgs.m_target.GetMaxHitPoints();
			flag2 = false;
			if (hitPointsAfterResolution > 0)
			{
				if (m_healthThresholdMode == HealthThreshMode.UseDirectValue)
				{
					if (hitPointsAfterResolution < m_healthThresholdDirect)
					{
						flag2 = true;
						goto IL_0109;
					}
				}
				if (m_healthThresholdMode == HealthThreshMode.UsePercentage)
				{
					if (maxHitPoints > 0)
					{
						flag2 = ((float)hitPointsAfterResolution / (float)maxHitPoints < m_healthThresholdPct);
					}
				}
				goto IL_0109;
			}
		}
		goto IL_0128;
		IL_0109:
		if (flag2)
		{
			GetCommonData().SetAudioEventOverride(m_closeCallAudioEvent);
		}
		goto IL_0128;
		IL_0128:
		return true;
	}
}
