using UnityEngine;

public class LightIntensitySetterForLowQuality : MonoBehaviour, IGameEventListener
{
	[Separator("If low quality, use this light intensity. Ignored if < 0", true)]
	public float m_lightIntensityAtLowQuality = 1f;

	private Light m_light;

	private float m_initialIntensity = 1f;

	private void Awake()
	{
		m_light = GetComponent<Light>();
		if (m_light != null)
		{
			m_initialIntensity = m_light.intensity;
		}
		if (GameEventManager.Get() == null)
		{
			return;
		}
		while (true)
		{
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.GraphicsQualityChanged);
			return;
		}
	}

	private void Start()
	{
		SetLightIntensityForCurrentQuality();
	}

	private void OnDestroy()
	{
		if (GameEventManager.Get() == null)
		{
			return;
		}
		while (true)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GraphicsQualityChanged);
			return;
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.GraphicsQualityChanged)
		{
			SetLightIntensityForCurrentQuality();
		}
	}

	private void SetLightIntensityForCurrentQuality()
	{
		if (Options_UI.Get() == null)
		{
			if (m_light == null && m_lightIntensityAtLowQuality >= 0f)
			{
				while (true)
				{
					switch (4)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
		GraphicsQuality currentGraphicsQuality = Options_UI.Get().GetCurrentGraphicsQuality();
		if (currentGraphicsQuality <= GraphicsQuality.Low)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					m_light.intensity = m_lightIntensityAtLowQuality;
					return;
				}
			}
		}
		m_light.intensity = m_initialIntensity;
	}
}
