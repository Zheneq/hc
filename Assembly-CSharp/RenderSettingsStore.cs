using System;
using UnityEngine;
using UnityEngine.Rendering;

public class RenderSettingsStore : MonoBehaviour
{
	public Color ambientEquatorColor;

	public Color ambientGroundColor;

	public Color ambientLight;

	public AmbientMode ambientMode;

	public SphericalHarmonicsL2 ambientProbe;

	public Color ambientSkyColor;

	public float ambientSkyboxAmount;

	public Cubemap customReflection;

	public DefaultReflectionMode defaultReflectionMode;

	public int defaultReflectionResolution;

	public float flareFadeSpeed;

	public float flareStrength;

	public bool fog;

	public Color fogColor;

	public float fogDensity;

	public float fogEndDistance;

	public FogMode fogMode;

	public float fogStartDistance;

	public float haloStrength;

	public int reflectionBounces;

	public Material skybox;

	private void Start()
	{
	}

	public void StoreGlobalRenderSettings()
	{
		this.ambientEquatorColor = RenderSettings.ambientEquatorColor;
		this.ambientGroundColor = RenderSettings.ambientGroundColor;
		this.ambientLight = RenderSettings.ambientLight;
		this.ambientMode = RenderSettings.ambientMode;
		this.ambientProbe = RenderSettings.ambientProbe;
		this.ambientSkyColor = RenderSettings.ambientSkyColor;
		this.ambientSkyboxAmount = RenderSettings.ambientIntensity;
		this.customReflection = RenderSettings.customReflection;
		this.defaultReflectionMode = RenderSettings.defaultReflectionMode;
		this.defaultReflectionResolution = RenderSettings.defaultReflectionResolution;
		this.flareFadeSpeed = RenderSettings.flareFadeSpeed;
		this.flareStrength = RenderSettings.flareStrength;
		this.fog = RenderSettings.fog;
		this.fogColor = RenderSettings.fogColor;
		this.fogDensity = RenderSettings.fogDensity;
		this.fogEndDistance = RenderSettings.fogEndDistance;
		this.fogMode = RenderSettings.fogMode;
		this.fogStartDistance = RenderSettings.fogStartDistance;
		this.haloStrength = RenderSettings.haloStrength;
		this.reflectionBounces = RenderSettings.reflectionBounces;
		this.customReflection = RenderSettings.customReflection;
		this.skybox = RenderSettings.skybox;
	}

	public void RestoreGlobalRenderSettings()
	{
		RenderSettings.ambientEquatorColor = this.ambientEquatorColor;
		RenderSettings.ambientGroundColor = this.ambientGroundColor;
		RenderSettings.ambientLight = this.ambientLight;
		RenderSettings.ambientMode = this.ambientMode;
		RenderSettings.ambientProbe = this.ambientProbe;
		RenderSettings.ambientSkyColor = this.ambientSkyColor;
		RenderSettings.ambientIntensity = this.ambientSkyboxAmount;
		RenderSettings.customReflection = this.customReflection;
		RenderSettings.defaultReflectionMode = this.defaultReflectionMode;
		RenderSettings.defaultReflectionResolution = this.defaultReflectionResolution;
		RenderSettings.flareFadeSpeed = this.flareFadeSpeed;
		RenderSettings.flareStrength = this.flareStrength;
		RenderSettings.fog = this.fog;
		RenderSettings.fogColor = this.fogColor;
		RenderSettings.fogDensity = this.fogDensity;
		RenderSettings.fogEndDistance = this.fogEndDistance;
		RenderSettings.fogMode = this.fogMode;
		RenderSettings.fogStartDistance = this.fogStartDistance;
		RenderSettings.haloStrength = this.haloStrength;
		RenderSettings.reflectionBounces = this.reflectionBounces;
		RenderSettings.skybox = this.skybox;
	}

	public static void UpdateRenderSettingsStore()
	{
		GameObject gameObject = GameObject.Find("RenderSettingsStore");
		if (gameObject != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RenderSettingsStore.UpdateRenderSettingsStore()).MethodHandle;
			}
			UnityEngine.Object.DestroyImmediate(gameObject);
		}
		GameObject gameObject2 = new GameObject("RenderSettingsStore");
		RenderSettingsStore renderSettingsStore = gameObject2.AddComponent<RenderSettingsStore>();
		renderSettingsStore.StoreGlobalRenderSettings();
	}

	public static bool LoadRenderSettingsStore()
	{
		bool result = false;
		GameObject gameObject = GameObject.Find("RenderSettingsStore");
		if (gameObject != null)
		{
			RenderSettingsStore component = gameObject.GetComponent<RenderSettingsStore>();
			if (component != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(RenderSettingsStore.LoadRenderSettingsStore()).MethodHandle;
				}
				component.RestoreGlobalRenderSettings();
				result = true;
			}
		}
		return result;
	}
}
