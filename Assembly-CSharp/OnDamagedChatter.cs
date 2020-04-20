using System;
using UnityEngine;

[Serializable]
public class OnDamagedChatter : ScriptableObject, IChatterData
{
	public ChatterData m_baseData = new ChatterData();

	public bool m_onSelfDamage = true;

	[Separator("For getting hit that brings health below threhold", true)]
	public OnDamagedChatter.HealthThreshMode m_healthThresholdMode;

	public float m_healthThresholdPct = 0.25f;

	public int m_healthThresholdDirect = 0x14;

	[AudioEvent(false)]
	public string m_closeCallAudioEvent;

	public ChatterData GetCommonData()
	{
		return this.m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.ActorDamaged_Client;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(OnDamagedChatter.ShouldPlayChatter(GameEventManager.EventType, GameEventManager.GameEventArgs, ChatterComponent)).MethodHandle;
			}
			return false;
		}
		GameEventManager.ActorHitHealthChangeArgs actorHitHealthChangeArgs = args as GameEventManager.ActorHitHealthChangeArgs;
		bool flag = actorHitHealthChangeArgs.m_target == component.gameObject.GetComponent<ActorData>();
		if (this.m_onSelfDamage)
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
			if (!flag)
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
				return false;
			}
		}
		this.GetCommonData().ClearAudioEventOverride();
		if (this.m_onSelfDamage && flag && !string.IsNullOrEmpty(this.m_closeCallAudioEvent))
		{
			int hitPointsAfterResolution = actorHitHealthChangeArgs.m_target.GetHitPointsAfterResolution();
			int maxHitPoints = actorHitHealthChangeArgs.m_target.GetMaxHitPoints();
			bool flag2 = false;
			if (hitPointsAfterResolution > 0)
			{
				if (this.m_healthThresholdMode == OnDamagedChatter.HealthThreshMode.UseDirectValue)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (hitPointsAfterResolution < this.m_healthThresholdDirect)
					{
						flag2 = true;
						goto IL_109;
					}
				}
				if (this.m_healthThresholdMode == OnDamagedChatter.HealthThreshMode.UsePercentage)
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
					if (maxHitPoints > 0)
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
						flag2 = ((float)hitPointsAfterResolution / (float)maxHitPoints < this.m_healthThresholdPct);
					}
				}
				IL_109:
				if (flag2)
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
					this.GetCommonData().SetAudioEventOverride(this.m_closeCallAudioEvent);
				}
			}
		}
		return true;
	}

	public enum HealthThreshMode
	{
		UsePercentage,
		UseDirectValue
	}
}
