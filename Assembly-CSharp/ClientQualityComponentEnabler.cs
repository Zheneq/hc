using System;
using UnityEngine;
using UnityEngine.Rendering;

public class ClientQualityComponentEnabler : MonoBehaviour, IGameEventListener
{
	[Serializable]
	public struct BehaviourGraphicsQualityPair
	{
		public Behaviour m_behaviour;

		public GraphicsQuality m_minQuality;

		public bool m_enableEvenIfDisabled;
	}

	public GraphicsQuality m_editorQuality = GraphicsQuality.High;

	public BehaviourGraphicsQualityPair[] m_componentsAndMinGraphicsQuality;

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
		return s_instance;
	}

	internal static bool OptimizeForMemory()
	{
		return SystemInfo.systemMemorySize / 1024 <= 4;
	}

	private void Awake()
	{
		s_instance = this;
		PKFxRenderingPlugin component = Camera.main.GetComponent<PKFxRenderingPlugin>();
		if (component != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (OptimizeForMemory())
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
				component.m_EnableDistortion = false;
			}
		}
		if (m_componentsAndMinGraphicsQuality != null && m_componentsAndMinGraphicsQuality.Length > 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			m_behaviourEnabledInPrefab = new bool[m_componentsAndMinGraphicsQuality.Length];
			for (int i = 0; i < m_componentsAndMinGraphicsQuality.Length; i++)
			{
				Behaviour behaviour = m_componentsAndMinGraphicsQuality[i].m_behaviour;
				if (!(behaviour != null))
				{
					continue;
				}
				bool[] behaviourEnabledInPrefab = m_behaviourEnabledInPrefab;
				int num = i;
				int num2;
				if (!behaviour.enabled)
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
					num2 = (m_componentsAndMinGraphicsQuality[i].m_enableEvenIfDisabled ? 1 : 0);
				}
				else
				{
					num2 = 1;
				}
				behaviourEnabledInPrefab[num] = ((byte)num2 != 0);
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_lights = (UnityEngine.Object.FindObjectsOfType(typeof(Light)) as Light[]);
		m_lightsIntensityInScene = new float[m_lights.Length];
		m_lightsRenderModeInScene = new LightRenderMode[m_lights.Length];
		m_lightsBounceIntensityInScene = new float[m_lights.Length];
		for (int j = 0; j < m_lights.Length; j++)
		{
			Light light = m_lights[j];
			float[] lightsIntensityInScene = m_lightsIntensityInScene;
			int num3 = j;
			float num4;
			if (light.isActiveAndEnabled)
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
				num4 = light.intensity;
			}
			else
			{
				num4 = 0f;
			}
			lightsIntensityInScene[num3] = num4;
			m_lightsRenderModeInScene[j] = light.renderMode;
			m_lightsBounceIntensityInScene[j] = light.bounceIntensity;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			m_ambientIntensityInScene = RenderSettings.ambientIntensity;
			m_ambientLightInScene = RenderSettings.ambientLight;
			m_ambientModeInScene = RenderSettings.ambientMode;
			m_ambientSkyColor = RenderSettings.ambientSkyColor;
			m_ambientEquatorColor = RenderSettings.ambientEquatorColor;
			m_ambientGroundColor = RenderSettings.ambientGroundColor;
			m_backdropCullingMask = ((1 << LayerMask.NameToLayer("Backdrop")) | (1 << LayerMask.NameToLayer("Background")));
			m_particleSystemsEnabledInScene = (UnityEngine.Object.FindObjectsOfType(typeof(ParticleSystem)) as ParticleSystem[]);
			if (m_particleSystemsEnabledInScene != null)
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
				for (int k = 0; k < m_particleSystemsEnabledInScene.Length; k++)
				{
					ParticleSystem particleSystem = m_particleSystemsEnabledInScene[k];
					if (particleSystem.gameObject.activeInHierarchy)
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
						if (particleSystem.emission.enabled)
						{
							continue;
						}
					}
					m_particleSystemsEnabledInScene[k] = null;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			m_cosmeticPkfxEnabledInScene = (UnityEngine.Object.FindObjectsOfType(typeof(PKFxFX)) as PKFxFX[]);
			if (m_cosmeticPkfxEnabledInScene != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				for (int l = 0; l < m_cosmeticPkfxEnabledInScene.Length; l++)
				{
					PKFxFX pKFxFX = m_cosmeticPkfxEnabledInScene[l];
					if (pKFxFX.gameObject.activeInHierarchy)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (pKFxFX.CompareTag("cosmetic"))
						{
							continue;
						}
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (pKFxFX.gameObject.layer == LayerMask.NameToLayer("Backdrop") || pKFxFX.gameObject.layer == LayerMask.NameToLayer("Background"))
						{
							continue;
						}
					}
					m_cosmeticPkfxEnabledInScene[l] = null;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			ConfigureComponentsForGraphicsQuality();
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.GraphicsQualityChanged);
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameCameraCreatedPre);
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameCameraCreatedPost);
			Shader.SetGlobalColor("_LowQualityAmbientColor", m_lowQualityAmbientColor);
			return;
		}
	}

	private void Start()
	{
		if (!OptimizeForMemory())
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			QualitySettings.masterTextureLimit = 1;
			return;
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.GameCameraCreatedPre)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_cosmeticPkfxEnabledInScene != null)
			{
				for (int i = 0; i < m_cosmeticPkfxEnabledInScene.Length; i++)
				{
					PKFxFX pKFxFX = m_cosmeticPkfxEnabledInScene[i];
					if (pKFxFX != null)
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
						pKFxFX.m_PlayOnStart = false;
					}
				}
				while (true)
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
		ConfigureComponentsForGraphicsQuality();
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
		s_instance = null;
	}

	private void OnValidate()
	{
		Shader.SetGlobalColor("_LowQualityAmbientColor", m_lowQualityAmbientColor);
		if (!Application.isPlaying)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_lights != null)
			{
				ConfigureComponentsForGraphicsQuality();
			}
			return;
		}
	}

	private void ConfigureComponentsForGraphicsQuality()
	{
		if (Options_UI.Get() == null)
		{
			return;
		}
		if (SystemInfo.graphicsShaderLevel < 50)
		{
			while (true)
			{
				switch (5)
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
			Shader.EnableKeyword("_GLOSSYREFLECTIONS_OFF");
		}
		GraphicsQuality currentGraphicsQuality = Options_UI.Get().GetCurrentGraphicsQuality();
		for (int i = 0; i < m_componentsAndMinGraphicsQuality.Length; i++)
		{
			Behaviour behaviour = m_componentsAndMinGraphicsQuality[i].m_behaviour;
			if (!(behaviour != null))
			{
				continue;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_behaviourEnabledInPrefab[i])
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				behaviour.enabled = (currentGraphicsQuality >= m_componentsAndMinGraphicsQuality[i].m_minQuality);
			}
		}
		Camera.main.allowHDR = false;
		PKFxRenderingPlugin component = Camera.main.GetComponent<PKFxRenderingPlugin>();
		if (component != null)
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
			if (currentGraphicsQuality > GraphicsQuality.Low)
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
				if (!OptimizeForMemory())
				{
					component.m_EnableDistortion = true;
					goto IL_0116;
				}
			}
			if (component.m_EnableDistortion)
			{
				while (true)
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
		goto IL_0116;
		IL_0116:
		int globalMaximumLOD;
		if (currentGraphicsQuality <= GraphicsQuality.Low)
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
			globalMaximumLOD = m_lowQualityShaderLod;
		}
		else
		{
			globalMaximumLOD = int.MaxValue;
		}
		Shader.globalMaximumLOD = globalMaximumLOD;
		if (currentGraphicsQuality >= m_minQualityBackdropLayer)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			Camera.main.cullingMask |= m_backdropCullingMask;
		}
		else
		{
			Camera.main.cullingMask &= ~m_backdropCullingMask;
		}
		if (currentGraphicsQuality >= m_minQualityCosmeticPkfx)
		{
			if (m_cosmeticPkfxEnabledInScene != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				for (int j = 0; j < m_cosmeticPkfxEnabledInScene.Length; j++)
				{
					PKFxFX pKFxFX = m_cosmeticPkfxEnabledInScene[j];
					if (pKFxFX != null)
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
						pKFxFX.StartEffect();
					}
				}
				while (true)
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
		else if (m_cosmeticPkfxEnabledInScene != null)
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
			for (int k = 0; k < m_cosmeticPkfxEnabledInScene.Length; k++)
			{
				PKFxFX pKFxFX2 = m_cosmeticPkfxEnabledInScene[k];
				if (pKFxFX2 != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					pKFxFX2.TerminateEffect();
					pKFxFX2.KillEffect();
				}
			}
		}
		if (currentGraphicsQuality <= GraphicsQuality.Medium)
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
			if (m_lowQualityChangeAmbient)
			{
				RenderSettings.ambientMode = AmbientMode.Flat;
				RenderSettings.ambientLight = m_lowQualityAmbientColor;
				RenderSettings.ambientIntensity = m_lowQualityAmbientIntensity;
				goto IL_02b4;
			}
		}
		RenderSettings.ambientMode = m_ambientModeInScene;
		RenderSettings.ambientLight = m_ambientLightInScene;
		RenderSettings.ambientEquatorColor = m_ambientEquatorColor;
		RenderSettings.ambientSkyColor = m_ambientSkyColor;
		RenderSettings.ambientGroundColor = m_ambientGroundColor;
		RenderSettings.ambientIntensity = m_ambientIntensityInScene;
		goto IL_02b4;
		IL_02b4:
		if (currentGraphicsQuality >= m_minQualityForRTLights)
		{
			for (int l = 0; l < m_lights.Length; l++)
			{
				if (m_lightsIntensityInScene[l] > 0f)
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
					m_lights[l].intensity = m_lightsIntensityInScene[l];
					m_lights[l].renderMode = m_lightsRenderModeInScene[l];
					m_lights[l].bounceIntensity = m_lightsBounceIntensityInScene[l];
					m_lights[l].enabled = true;
				}
			}
			while (true)
			{
				switch (5)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		if (m_lights != null)
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
			for (int m = 0; m < m_lights.Length; m++)
			{
				if (m_lights[m].type == LightType.Directional)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_lightsIntensityInScene[m] > 0f && (m_lights[m].cullingMask & 1) == 0)
					{
						m_lights[m].intensity = m_lowQualityCharacterLightIntensity;
						m_lights[m].renderMode = LightRenderMode.ForceVertex;
						m_lights[m].bounceIntensity = 0f;
						continue;
					}
				}
				m_lights[m].enabled = false;
			}
		}
		if (m_particleSystemsEnabledInScene == null)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (currentGraphicsQuality >= m_minQualityParticleSystems)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
					{
						for (int n = 0; n < m_particleSystemsEnabledInScene.Length; n++)
						{
							ParticleSystem particleSystem = m_particleSystemsEnabledInScene[n];
							if (particleSystem != null)
							{
								while (true)
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
							ParticleSystem.EmissionModule emission = particleSystem.emission;
							emission.enabled = true;
						}
						return;
					}
					}
				}
			}
			for (int num = 0; num < m_particleSystemsEnabledInScene.Length; num++)
			{
				ParticleSystem particleSystem2 = m_particleSystemsEnabledInScene[num];
				if (particleSystem2 != null)
				{
					particleSystem2.gameObject.SetActive(false);
					particleSystem2.Stop();
					particleSystem2.Clear();
				}
				ParticleSystem.EmissionModule emission2 = particleSystem2.emission;
				emission2.enabled = false;
			}
			return;
		}
	}
}
