using System;
using UnityEngine;

public class LightIntensitySetterForLowQuality : MonoBehaviour, IGameEventListener
{
	[Separator("If low quality, use this light intensity. Ignored if < 0", true)]
	public float m_lightIntensityAtLowQuality = 1f;

	private Light m_light;

	private float m_initialIntensity = 1f;

	private void Awake()
	{
		this.m_light = base.GetComponent<Light>();
		if (this.m_light != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightIntensitySetterForLowQuality.Awake()).MethodHandle;
			}
			this.m_initialIntensity = this.m_light.intensity;
		}
		if (GameEventManager.Get() != null)
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
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.GraphicsQualityChanged);
		}
	}

	private void Start()
	{
		this.SetLightIntensityForCurrentQuality();
	}

	private void OnDestroy()
	{
		if (GameEventManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightIntensitySetterForLowQuality.OnDestroy()).MethodHandle;
			}
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GraphicsQualityChanged);
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.GraphicsQualityChanged)
		{
			this.SetLightIntensityForCurrentQuality();
		}
	}

	private void SetLightIntensityForCurrentQuality()
	{
		if (Options_UI.Get() == null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightIntensitySetterForLowQuality.SetLightIntensityForCurrentQuality()).MethodHandle;
			}
			if (this.m_light == null && this.m_lightIntensityAtLowQuality >= 0f)
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
				return;
			}
		}
		GraphicsQuality currentGraphicsQuality = Options_UI.Get().GetCurrentGraphicsQuality();
		if (currentGraphicsQuality <= GraphicsQuality.Low)
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
			this.m_light.intensity = this.m_lightIntensityAtLowQuality;
		}
		else
		{
			this.m_light.intensity = this.m_initialIntensity;
		}
	}
}
