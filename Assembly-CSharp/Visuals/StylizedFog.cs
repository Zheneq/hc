using UnityEngine;

[AddComponentMenu("Image Effects/Stylized Fog")]
[ExecuteInEditMode]
public class StylizedFog : MonoBehaviour
{
	public enum StylizedFogMode
	{
		Blend,
		Additive,
		Multiply,
		Screen
	}

	public enum StylizedFogGradient
	{
		Textures,
		Gradients
	}

	public StylizedFogMode fogMode;

	[SerializeField]
	[Header("Blend")]
	[Tooltip("Use a second ramp for transition")]
	private bool useBlend;

	[Range(0f, 1f)]
	[Tooltip("Amount of blend between 2 gradients")]
	public float blend;

	[Tooltip("Use ramp from textures or gradient fields")]
	[Header("Gradients")]
	public StylizedFogGradient gradientSource;

	public Gradient rampGradient;

	public Gradient rampBlendGradient;

	public Texture2D rampTexture;

	public Texture2D rampBlendTexture;

	[SerializeField]
	[Header("Noise Texture")]
	private bool useNoise;

	public Texture2D noiseTexture;

	[Space(5f)]
	[Tooltip("XY: Speed1 XY | WH: Speed2 XY")]
	public Vector4 noiseSpeed;

	[Space(5f)]
	[Tooltip("XY: Tiling1 XY | WH: Tiling2 XY")]
	public Vector4 noiseTiling = new Vector4(1f, 1f, 1f, 1f);

	private Camera cam;

	private Texture2D mainRamp;

	private Texture2D blendRamp;

	private Shader fogShader;

	private Material fogMat;

	private void Start()
	{
		if (GetComponent<Camera>() != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					createResources();
					UpdateTextures();
					SetKeywords();
					return;
				}
			}
		}
		base.enabled = false;
	}

	private void OnEnable()
	{
		if (GetComponent<Camera>() != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					createResources();
					UpdateTextures();
					SetKeywords();
					return;
				}
			}
		}
		base.enabled = false;
	}

	private void OnDisable()
	{
		if (!(GetComponent<Camera>() != null))
		{
			return;
		}
		while (true)
		{
			clearResources();
			return;
		}
	}

	public void UpdateTextures()
	{
		setGradient();
		SetKeywords();
		updateValues();
	}

	private void updateValues()
	{
		if (!(fogMat == null))
		{
			if (!(fogShader == null))
			{
				goto IL_0043;
			}
		}
		createResources();
		goto IL_0043;
		IL_0043:
		if (mainRamp != null)
		{
			fogMat.SetTexture("_MainRamp", mainRamp);
			Shader.SetGlobalTexture("_SF_MainRamp", mainRamp);
		}
		if (useBlend)
		{
			if (blendRamp != null)
			{
				fogMat.SetTexture("_BlendRamp", blendRamp);
				fogMat.SetFloat("_Blend", blend);
				Shader.SetGlobalTexture("_SF_BlendRamp", blendRamp);
				Shader.SetGlobalFloat("_SF_Blend", blend);
			}
		}
		if (!useNoise || !(noiseTexture != null))
		{
			return;
		}
		while (true)
		{
			fogMat.SetTexture("_NoiseTex", noiseTexture);
			fogMat.SetVector("_NoiseSpeed", noiseSpeed);
			fogMat.SetVector("_NoiseTiling", noiseTiling);
			Shader.SetGlobalTexture("_SF_NoiseTex", noiseTexture);
			Shader.SetGlobalVector("_SF_NoiseSpeed", noiseSpeed);
			Shader.SetGlobalVector("_SF_NoiseTiling", noiseTiling);
			return;
		}
	}

	private void setGradient()
	{
		if (gradientSource == StylizedFogGradient.Textures)
		{
			mainRamp = rampTexture;
			if (!useBlend)
			{
				return;
			}
			while (true)
			{
				blendRamp = rampBlendTexture;
				return;
			}
		}
		if (gradientSource != StylizedFogGradient.Gradients)
		{
			return;
		}
		if (mainRamp != null)
		{
			Object.DestroyImmediate(mainRamp);
		}
		mainRamp = GenerateGradient(rampGradient, 256, 8);
		if (useBlend)
		{
			if (blendRamp != null)
			{
				Object.DestroyImmediate(blendRamp);
			}
			blendRamp = GenerateGradient(rampBlendGradient, 256, 8);
		}
	}

	private Texture2D GenerateGradient(Gradient gradient, int gWidth, int gHeight)
	{
		Texture2D texture2D = new Texture2D(gWidth, gHeight, TextureFormat.ARGB32, false);
		texture2D.wrapMode = TextureWrapMode.Clamp;
		texture2D.hideFlags = HideFlags.HideAndDontSave;
		Color white = Color.white;
		if (gradient != null)
		{
			for (int i = 0; i < gWidth; i++)
			{
				white = gradient.Evaluate((float)i / (float)gWidth);
				for (int j = 0; j < gHeight; j++)
				{
					texture2D.SetPixel(i, j, white);
				}
			}
		}
		texture2D.Apply();
		return texture2D;
	}

	private void createResources()
	{
		if (fogShader == null)
		{
			fogShader = Shader.Find("Hidden/StylizedFog");
		}
		if (fogMat == null && fogShader != null)
		{
			fogMat = new Material(fogShader);
			fogMat.hideFlags = HideFlags.HideAndDontSave;
		}
		if (!(mainRamp == null))
		{
			if (!(blendRamp == null))
			{
				goto IL_00a9;
			}
		}
		setGradient();
		goto IL_00a9;
		IL_00a9:
		if (cam == null)
		{
			cam = GetComponent<Camera>();
			cam.depthTextureMode |= DepthTextureMode.Depth;
		}
	}

	private void clearResources()
	{
		if (fogMat != null)
		{
			Object.DestroyImmediate(fogMat);
		}
		disableKeywords();
		cam.depthTextureMode = DepthTextureMode.None;
	}

	public void SetKeywords()
	{
		switch (fogMode)
		{
		case StylizedFogMode.Blend:
			Shader.EnableKeyword("_FOG_BLEND");
			Shader.DisableKeyword("_FOG_ADDITIVE");
			Shader.DisableKeyword("_FOG_MULTIPLY");
			Shader.DisableKeyword("_FOG_SCREEN");
			break;
		case StylizedFogMode.Additive:
			Shader.DisableKeyword("_FOG_BLEND");
			Shader.EnableKeyword("_FOG_ADDITIVE");
			Shader.DisableKeyword("_FOG_MULTIPLY");
			Shader.DisableKeyword("_FOG_SCREEN");
			break;
		case StylizedFogMode.Multiply:
			Shader.DisableKeyword("_FOG_BLEND");
			Shader.DisableKeyword("_FOG_ADDITIVE");
			Shader.EnableKeyword("_FOG_MULTIPLY");
			Shader.DisableKeyword("_FOG_SCREEN");
			break;
		case StylizedFogMode.Screen:
			Shader.DisableKeyword("_FOG_BLEND");
			Shader.DisableKeyword("_FOG_ADDITIVE");
			Shader.DisableKeyword("_FOG_MULTIPLY");
			Shader.EnableKeyword("_FOG_SCREEN");
			break;
		}
		if (useBlend)
		{
			Shader.EnableKeyword("_FOG_BLEND_ON");
			Shader.DisableKeyword("_FOG_BLEND_OFF");
		}
		else
		{
			Shader.EnableKeyword("_FOG_BLEND_OFF");
			Shader.DisableKeyword("_FOG_BLEND_ON");
		}
		if (useNoise)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Shader.EnableKeyword("_FOG_NOISE_ON");
					Shader.DisableKeyword("_FOG_NOISE_OFF");
					return;
				}
			}
		}
		Shader.EnableKeyword("_FOG_NOISE_OFF");
		Shader.DisableKeyword("_FOG_NOISE_ON");
	}

	private void disableKeywords()
	{
		Shader.DisableKeyword("_FOG_BLEND");
		Shader.DisableKeyword("_FOG_ADDITIVE");
		Shader.DisableKeyword("_FOG_MULTIPLY");
		Shader.DisableKeyword("_FOG_SCREEN");
		Shader.DisableKeyword("_FOG_BLEND_OFF");
		Shader.DisableKeyword("_FOG_BLEND_ON");
		Shader.DisableKeyword("_FOG_NOISE_OFF");
		Shader.DisableKeyword("_FOG_NOISE_ON");
	}

	private bool isSupported()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (fogShader.isSupported)
		{
			if (!(fogShader == null))
			{
				return true;
			}
		}
		return false;
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!isSupported())
		{
			Graphics.Blit(source, destination);
			return;
		}
		updateValues();
		Graphics.Blit(source, destination, fogMat);
	}
}
