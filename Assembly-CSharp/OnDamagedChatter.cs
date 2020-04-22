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
				switch (1)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return false;
			}
		}
		GameEventManager.ActorHitHealthChangeArgs actorHitHealthChangeArgs = args as GameEventManager.ActorHitHealthChangeArgs;
		bool flag = actorHitHealthChangeArgs.m_target == component.gameObject.GetComponent<ActorData>();
		if (m_onSelfDamage)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
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
			int hitPointsAfterResolution = actorHitHealthChangeArgs.m_target.GetHitPointsAfterResolution();
			int maxHitPoints = actorHitHealthChangeArgs.m_target.GetMaxHitPoints();
			flag2 = false;
			if (hitPointsAfterResolution > 0)
			{
				if (m_healthThresholdMode == HealthThreshMode.UseDirectValue)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (hitPointsAfterResolution < m_healthThresholdDirect)
					{
						flag2 = true;
						goto IL_0109;
					}
				}
				if (m_healthThresholdMode == HealthThreshMode.UsePercentage)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (maxHitPoints > 0)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			GetCommonData().SetAudioEventOverride(m_closeCallAudioEvent);
		}
		goto IL_0128;
		IL_0128:
		return true;
	}
}
