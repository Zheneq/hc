using UnityEngine;

public class TimeMultiplierAdjuster : MonoBehaviour, IGameEventListener
{
	private PKFxRenderingPlugin m_plugin;

	public void Start()
	{
		m_plugin = GetComponent<PKFxRenderingPlugin>();
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GametimeScaleChange);
	}

	public void OnDestroy()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GametimeScaleChange);
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.GametimeScaleChange)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_plugin != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					m_plugin.m_TimeMultiplier = GameTime.scale;
					return;
				}
			}
			return;
		}
	}
}
