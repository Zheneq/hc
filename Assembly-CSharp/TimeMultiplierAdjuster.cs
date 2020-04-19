using System;
using UnityEngine;

public class TimeMultiplierAdjuster : MonoBehaviour, IGameEventListener
{
	private PKFxRenderingPlugin m_plugin;

	public void Start()
	{
		this.m_plugin = base.GetComponent<PKFxRenderingPlugin>();
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GametimeScaleChange);
	}

	public void OnDestroy()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GametimeScaleChange);
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.GametimeScaleChange)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TimeMultiplierAdjuster.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
			}
			if (this.m_plugin != null)
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
				this.m_plugin.m_TimeMultiplier = GameTime.scale;
			}
		}
	}
}
