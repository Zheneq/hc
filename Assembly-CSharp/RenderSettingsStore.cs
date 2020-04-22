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
		ambientEquatorColor = RenderSettings.ambientEquatorColor;
		ambientGroundColor = RenderSettings.ambientGroundColor;
		ambientLight = RenderSettings.ambientLight;
		ambientMode = RenderSettings.ambientMode;
		ambientProbe = RenderSettings.ambientProbe;
		ambientSkyColor = RenderSettings.ambientSkyColor;
		ambientSkyboxAmount = RenderSettings.ambientIntensity;
		customReflection = RenderSettings.customReflection;
		defaultReflectionMode = RenderSettings.defaultReflectionMode;
		defaultReflectionResolution = RenderSettings.defaultReflectionResolution;
		flareFadeSpeed = RenderSettings.flareFadeSpeed;
		flareStrength = RenderSettings.flareStrength;
		fog = RenderSettings.fog;
		fogColor = RenderSettings.fogColor;
		fogDensity = RenderSettings.fogDensity;
		fogEndDistance = RenderSettings.fogEndDistance;
		fogMode = RenderSettings.fogMode;
		fogStartDistance = RenderSettings.fogStartDistance;
		haloStrength = RenderSettings.haloStrength;
		reflectionBounces = RenderSettings.reflectionBounces;
		customReflection = RenderSettings.customReflection;
		skybox = RenderSettings.skybox;
	}

	public void RestoreGlobalRenderSettings()
	{
		RenderSettings.ambientEquatorColor = ambientEquatorColor;
		RenderSettings.ambientGroundColor = ambientGroundColor;
		RenderSettings.ambientLight = ambientLight;
		RenderSettings.ambientMode = ambientMode;
		RenderSettings.ambientProbe = ambientProbe;
		RenderSettings.ambientSkyColor = ambientSkyColor;
		RenderSettings.ambientIntensity = ambientSkyboxAmount;
		RenderSettings.customReflection = customReflection;
		RenderSettings.defaultReflectionMode = defaultReflectionMode;
		RenderSettings.defaultReflectionResolution = defaultReflectionResolution;
		RenderSettings.flareFadeSpeed = flareFadeSpeed;
		RenderSettings.flareStrength = flareStrength;
		RenderSettings.fog = fog;
		RenderSettings.fogColor = fogColor;
		RenderSettings.fogDensity = fogDensity;
		RenderSettings.fogEndDistance = fogEndDistance;
		RenderSettings.fogMode = fogMode;
		RenderSettings.fogStartDistance = fogStartDistance;
		RenderSettings.haloStrength = haloStrength;
		RenderSettings.reflectionBounces = reflectionBounces;
		RenderSettings.skybox = skybox;
	}

	public static void UpdateRenderSettingsStore()
	{
		GameObject gameObject = GameObject.Find("RenderSettingsStore");
		if (gameObject != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Object.DestroyImmediate(gameObject);
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
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				component.RestoreGlobalRenderSettings();
				result = true;
			}
		}
		return result;
	}
}
