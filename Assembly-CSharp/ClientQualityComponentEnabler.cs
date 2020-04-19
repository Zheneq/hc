using System;
using UnityEngine;
using UnityEngine.Rendering;

public class ClientQualityComponentEnabler : MonoBehaviour, IGameEventListener
{
	public GraphicsQuality m_editorQuality = GraphicsQuality.High;

	public ClientQualityComponentEnabler.BehaviourGraphicsQualityPair[] m_componentsAndMinGraphicsQuality;

	public GraphicsQuality m_minQualityForHDR = GraphicsQuality.Medium;

	public GraphicsQuality m_minQualityForRTLights;

	public GraphicsQuality m_minQualityBackdropLayer = GraphicsQuality.Medium;

	public GraphicsQuality m_minQualityCosmeticPkfx = GraphicsQuality.Medium;

	public GraphicsQuality m_minQualityParticleSystems = GraphicsQuality.Medium;

	public int m_lowQualityShaderLod = int.MaxValue;

	[Range(0f, 5f)]
	public float m_lowQualityCharacterLightIntensity = 1f;

	public bool m_lowQualityChangeAmbient;

	[Range(0f, 5f)]
	public float m_lowQualityAmbientIntensity = 1f;

	public Color m_lowQualityAmbientColor = Color.white;

	private bool[] m_behaviourEnabledInPrefab;

	private Light[] m_lights;

	private float[] m_lightsIntensityInScene;

	private LightRenderMode[] m_lightsRenderModeInScene;

	private float[] m_lightsBounceIntensityInScene;

	private float m_ambientIntensityInScene;

	private Color m_ambientLightInScene;

	private Color m_ambientSkyColor;

	private Color m_ambientEquatorColor;

	private Color m_ambientGroundColor;

	private AmbientMode m_ambientModeInScene;

	private ParticleSystem[] m_particleSystemsEnabledInScene;

	private PKFxFX[] m_cosmeticPkfxEnabledInScene;

	private int m_backdropCullingMask;

	private static ClientQualityComponentEnabler s_instance;

	internal static ClientQualityComponentEnabler Get()
	{
		return ClientQualityComponentEnabler.s_instance;
	}

	internal static bool OptimizeForMemory()
	{
		return SystemInfo.systemMemorySize / 0x400 <= 4;
	}

	private void Awake()
	{
		ClientQualityComponentEnabler.s_instance = this;
		PKFxRenderingPlugin component = Camera.main.GetComponent<PKFxRenderingPlugin>();
		if (component != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientQualityComponentEnabler.Awake()).MethodHandle;
			}
			if (ClientQualityComponentEnabler.OptimizeForMemory())
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
				component.m_EnableDistortion = false;
			}
		}
		if (this.m_componentsAndMinGraphicsQuality != null && this.m_componentsAndMinGraphicsQuality.Length > 0)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_behaviourEnabledInPrefab = new bool[this.m_componentsAndMinGraphicsQuality.Length];
			for (int i = 0; i < this.m_componentsAndMinGraphicsQuality.Length; i++)
			{
				Behaviour behaviour = this.m_componentsAndMinGraphicsQuality[i].m_behaviour;
				if (behaviour != null)
				{
					bool[] behaviourEnabledInPrefab = this.m_behaviourEnabledInPrefab;
					int num = i;
					int num2;
					if (!behaviour.enabled)
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
						num2 = (this.m_componentsAndMinGraphicsQuality[i].m_enableEvenIfDisabled ? 1 : 0);
					}
					else
					{
						num2 = 1;
					}
					behaviourEnabledInPrefab[num] = num2;
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_lights = (UnityEngine.Object.FindObjectsOfType(typeof(Light)) as Light[]);
		this.m_lightsIntensityInScene = new float[this.m_lights.Length];
		this.m_lightsRenderModeInScene = new LightRenderMode[this.m_lights.Length];
		this.m_lightsBounceIntensityInScene = new float[this.m_lights.Length];
		for (int j = 0; j < this.m_lights.Length; j++)
		{
			Light light = this.m_lights[j];
			float[] lightsIntensityInScene = this.m_lightsIntensityInScene;
			int num3 = j;
			float num4;
			if (light.isActiveAndEnabled)
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
				num4 = light.intensity;
			}
			else
			{
				num4 = 0f;
			}
			lightsIntensityInScene[num3] = num4;
			this.m_lightsRenderModeInScene[j] = light.renderMode;
			this.m_lightsBounceIntensityInScene[j] = light.bounceIntensity;
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		this.m_ambientIntensityInScene = RenderSettings.ambientIntensity;
		this.m_ambientLightInScene = RenderSettings.ambientLight;
		this.m_ambientModeInScene = RenderSettings.ambientMode;
		this.m_ambientSkyColor = RenderSettings.ambientSkyColor;
		this.m_ambientEquatorColor = RenderSettings.ambientEquatorColor;
		this.m_ambientGroundColor = RenderSettings.ambientGroundColor;
		this.m_backdropCullingMask = (1 << LayerMask.NameToLayer("Backdrop") | 1 << LayerMask.NameToLayer("Background"));
		this.m_particleSystemsEnabledInScene = (UnityEngine.Object.FindObjectsOfType(typeof(ParticleSystem)) as ParticleSystem[]);
		if (this.m_particleSystemsEnabledInScene != null)
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
			int k = 0;
			while (k < this.m_particleSystemsEnabledInScene.Length)
			{
				ParticleSystem particleSystem = this.m_particleSystemsEnabledInScene[k];
				if (!particleSystem.gameObject.activeInHierarchy)
				{
					goto IL_297;
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!particleSystem.emission.enabled)
				{
					goto IL_297;
				}
				IL_2A1:
				k++;
				continue;
				IL_297:
				this.m_particleSystemsEnabledInScene[k] = null;
				goto IL_2A1;
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_cosmeticPkfxEnabledInScene = (UnityEngine.Object.FindObjectsOfType(typeof(PKFxFX)) as PKFxFX[]);
		if (this.m_cosmeticPkfxEnabledInScene != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			int l = 0;
			while (l < this.m_cosmeticPkfxEnabledInScene.Length)
			{
				PKFxFX pkfxFX = this.m_cosmeticPkfxEnabledInScene[l];
				if (!pkfxFX.gameObject.activeInHierarchy)
				{
					goto IL_371;
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!pkfxFX.CompareTag("cosmetic"))
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (pkfxFX.gameObject.layer != LayerMask.NameToLayer("Backdrop") && pkfxFX.gameObject.layer != LayerMask.NameToLayer("Background"))
					{
						goto IL_371;
					}
				}
				IL_37B:
				l++;
				continue;
				IL_371:
				this.m_cosmeticPkfxEnabledInScene[l] = null;
				goto IL_37B;
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.ConfigureComponentsForGraphicsQuality();
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GraphicsQualityChanged);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameCameraCreatedPre);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameCameraCreatedPost);
		Shader.SetGlobalColor("_LowQualityAmbientColor", this.m_lowQualityAmbientColor);
	}

	private void Start()
	{
		if (ClientQualityComponentEnabler.OptimizeForMemory())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientQualityComponentEnabler.Start()).MethodHandle;
			}
			QualitySettings.masterTextureLimit = 1;
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.GameCameraCreatedPre)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientQualityComponentEnabler.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
			}
			if (this.m_cosmeticPkfxEnabledInScene != null)
			{
				for (int i = 0; i < this.m_cosmeticPkfxEnabledInScene.Length; i++)
				{
					PKFxFX pkfxFX = this.m_cosmeticPkfxEnabledInScene[i];
					if (pkfxFX != null)
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
						pkfxFX.m_PlayOnStart = false;
					}
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		this.ConfigureComponentsForGraphicsQuality();
	}

	private void OnDestroy()
	{
		Shader.globalMaximumLOD = int.MaxValue;
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GraphicsQualityChanged);
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameCameraCreatedPre);
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameCameraCreatedPost);
		}
		ClientQualityComponentEnabler.s_instance = null;
	}

	private void OnValidate()
	{
		Shader.SetGlobalColor("_LowQualityAmbientColor", this.m_lowQualityAmbientColor);
		if (Application.isPlaying)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientQualityComponentEnabler.OnValidate()).MethodHandle;
			}
			if (this.m_lights != null)
			{
				this.ConfigureComponentsForGraphicsQuality();
			}
		}
	}

	private void ConfigureComponentsForGraphicsQuality()
	{
		if (Options_UI.Get() == null)
		{
			return;
		}
		if (SystemInfo.graphicsShaderLevel < 0x32)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientQualityComponentEnabler.ConfigureComponentsForGraphicsQuality()).MethodHandle;
			}
			Shader.EnableKeyword("_GLOSSYREFLECTIONS_OFF");
		}
		GraphicsQuality currentGraphicsQuality = Options_UI.Get().GetCurrentGraphicsQuality();
		for (int i = 0; i < this.m_componentsAndMinGraphicsQuality.Length; i++)
		{
			Behaviour behaviour = this.m_componentsAndMinGraphicsQuality[i].m_behaviour;
			if (behaviour != null)
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
				if (this.m_behaviourEnabledInPrefab[i])
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
					behaviour.enabled = (currentGraphicsQuality >= this.m_componentsAndMinGraphicsQuality[i].m_minQuality);
				}
			}
		}
		Camera.main.allowHDR = false;
		PKFxRenderingPlugin component = Camera.main.GetComponent<PKFxRenderingPlugin>();
		if (component != null)
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
			if (currentGraphicsQuality > GraphicsQuality.Low)
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
				if (!ClientQualityComponentEnabler.OptimizeForMemory())
				{
					component.m_EnableDistortion = true;
					goto IL_116;
				}
			}
			if (component.m_EnableDistortion)
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
				component.m_EnableDistortion = false;
			}
		}
		IL_116:
		int globalMaximumLOD;
		if (currentGraphicsQuality <= GraphicsQuality.Low)
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
			globalMaximumLOD = this.m_lowQualityShaderLod;
		}
		else
		{
			globalMaximumLOD = int.MaxValue;
		}
		Shader.globalMaximumLOD = globalMaximumLOD;
		if (currentGraphicsQuality >= this.m_minQualityBackdropLayer)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			Camera.main.cullingMask |= this.m_backdropCullingMask;
		}
		else
		{
			Camera.main.cullingMask &= ~this.m_backdropCullingMask;
		}
		if (currentGraphicsQuality >= this.m_minQualityCosmeticPkfx)
		{
			if (this.m_cosmeticPkfxEnabledInScene != null)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				for (int j = 0; j < this.m_cosmeticPkfxEnabledInScene.Length; j++)
				{
					PKFxFX pkfxFX = this.m_cosmeticPkfxEnabledInScene[j];
					if (pkfxFX != null)
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
						pkfxFX.StartEffect();
					}
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		else if (this.m_cosmeticPkfxEnabledInScene != null)
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
			for (int k = 0; k < this.m_cosmeticPkfxEnabledInScene.Length; k++)
			{
				PKFxFX pkfxFX2 = this.m_cosmeticPkfxEnabledInScene[k];
				if (pkfxFX2 != null)
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
					pkfxFX2.TerminateEffect();
					pkfxFX2.KillEffect();
				}
			}
		}
		if (currentGraphicsQuality <= GraphicsQuality.Medium)
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
			if (this.m_lowQualityChangeAmbient)
			{
				RenderSettings.ambientMode = AmbientMode.Flat;
				RenderSettings.ambientLight = this.m_lowQualityAmbientColor;
				RenderSettings.ambientIntensity = this.m_lowQualityAmbientIntensity;
				goto IL_2B4;
			}
		}
		RenderSettings.ambientMode = this.m_ambientModeInScene;
		RenderSettings.ambientLight = this.m_ambientLightInScene;
		RenderSettings.ambientEquatorColor = this.m_ambientEquatorColor;
		RenderSettings.ambientSkyColor = this.m_ambientSkyColor;
		RenderSettings.ambientGroundColor = this.m_ambientGroundColor;
		RenderSettings.ambientIntensity = this.m_ambientIntensityInScene;
		IL_2B4:
		if (currentGraphicsQuality >= this.m_minQualityForRTLights)
		{
			for (int l = 0; l < this.m_lights.Length; l++)
			{
				if (this.m_lightsIntensityInScene[l] > 0f)
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
					this.m_lights[l].intensity = this.m_lightsIntensityInScene[l];
					this.m_lights[l].renderMode = this.m_lightsRenderModeInScene[l];
					this.m_lights[l].bounceIntensity = this.m_lightsBounceIntensityInScene[l];
					this.m_lights[l].enabled = true;
				}
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		else
		{
			if (this.m_lights != null)
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
				int m = 0;
				while (m < this.m_lights.Length)
				{
					if (this.m_lights[m].type != LightType.Directional)
					{
						goto IL_3ED;
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_lightsIntensityInScene[m] <= 0f || (this.m_lights[m].cullingMask & 1) != 0)
					{
						goto IL_3ED;
					}
					this.m_lights[m].intensity = this.m_lowQualityCharacterLightIntensity;
					this.m_lights[m].renderMode = LightRenderMode.ForceVertex;
					this.m_lights[m].bounceIntensity = 0f;
					IL_3FC:
					m++;
					continue;
					IL_3ED:
					this.m_lights[m].enabled = false;
					goto IL_3FC;
				}
			}
			if (this.m_particleSystemsEnabledInScene != null)
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
				if (currentGraphicsQuality >= this.m_minQualityParticleSystems)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					for (int n = 0; n < this.m_particleSystemsEnabledInScene.Length; n++)
					{
						ParticleSystem particleSystem = this.m_particleSystemsEnabledInScene[n];
						if (particleSystem != null)
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
							particleSystem.gameObject.SetActive(true);
							particleSystem.Play();
						}
						particleSystem.emission.enabled = true;
					}
				}
				else
				{
					for (int num = 0; num < this.m_particleSystemsEnabledInScene.Length; num++)
					{
						ParticleSystem particleSystem2 = this.m_particleSystemsEnabledInScene[num];
						if (particleSystem2 != null)
						{
							particleSystem2.gameObject.SetActive(false);
							particleSystem2.Stop();
							particleSystem2.Clear();
						}
						particleSystem2.emission.enabled = false;
					}
				}
			}
		}
	}

	[Serializable]
	public struct BehaviourGraphicsQualityPair
	{
		public Behaviour m_behaviour;

		public GraphicsQuality m_minQuality;

		public bool m_enableEvenIfDisabled;
	}
}
