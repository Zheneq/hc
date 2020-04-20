using System;
using UnityEngine;

[AddComponentMenu("Image Effects/Stylized Fog")]
[ExecuteInEditMode]
public class StylizedFog : MonoBehaviour
{
	public StylizedFog.StylizedFogMode fogMode;

	[SerializeField]
	[Header("Blend")]
	[Tooltip("Use a second ramp for transition")]
	private bool useBlend;

	[Range(0f, 1f)]
	[Tooltip("Amount of blend between 2 gradients")]
	public float blend;

	[Tooltip("Use ramp from textures or gradient fields")]
	[Header("Gradients")]
	public StylizedFog.StylizedFogGradient gradientSource;

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
		if (base.GetComponent<Camera>() != null)
		{
			this.createResources();
			this.UpdateTextures();
			this.SetKeywords();
		}
		else
		{
			base.enabled = false;
		}
	}

	private void OnEnable()
	{
		if (base.GetComponent<Camera>() != null)
		{
			this.createResources();
			this.UpdateTextures();
			this.SetKeywords();
		}
		else
		{
			base.enabled = false;
		}
	}

	private void OnDisable()
	{
		if (base.GetComponent<Camera>() != null)
		{
			this.clearResources();
		}
	}

	public void UpdateTextures()
	{
		this.setGradient();
		this.SetKeywords();
		this.updateValues();
	}

	private void updateValues()
	{
		if (!(this.fogMat == null))
		{
			if (!(this.fogShader == null))
			{
				goto IL_43;
			}
		}
		this.createResources();
		IL_43:
		if (this.mainRamp != null)
		{
			this.fogMat.SetTexture("_MainRamp", this.mainRamp);
			Shader.SetGlobalTexture("_SF_MainRamp", this.mainRamp);
		}
		if (this.useBlend)
		{
			if (this.blendRamp != null)
			{
				this.fogMat.SetTexture("_BlendRamp", this.blendRamp);
				this.fogMat.SetFloat("_Blend", this.blend);
				Shader.SetGlobalTexture("_SF_BlendRamp", this.blendRamp);
				Shader.SetGlobalFloat("_SF_Blend", this.blend);
			}
		}
		if (this.useNoise && this.noiseTexture != null)
		{
			this.fogMat.SetTexture("_NoiseTex", this.noiseTexture);
			this.fogMat.SetVector("_NoiseSpeed", this.noiseSpeed);
			this.fogMat.SetVector("_NoiseTiling", this.noiseTiling);
			Shader.SetGlobalTexture("_SF_NoiseTex", this.noiseTexture);
			Shader.SetGlobalVector("_SF_NoiseSpeed", this.noiseSpeed);
			Shader.SetGlobalVector("_SF_NoiseTiling", this.noiseTiling);
		}
	}

	private void setGradient()
	{
		if (this.gradientSource == StylizedFog.StylizedFogGradient.Textures)
		{
			this.mainRamp = this.rampTexture;
			if (this.useBlend)
			{
				this.blendRamp = this.rampBlendTexture;
			}
		}
		else if (this.gradientSource == StylizedFog.StylizedFogGradient.Gradients)
		{
			if (this.mainRamp != null)
			{
				UnityEngine.Object.DestroyImmediate(this.mainRamp);
			}
			this.mainRamp = this.GenerateGradient(this.rampGradient, 0x100, 8);
			if (this.useBlend)
			{
				if (this.blendRamp != null)
				{
					UnityEngine.Object.DestroyImmediate(this.blendRamp);
				}
				this.blendRamp = this.GenerateGradient(this.rampBlendGradient, 0x100, 8);
			}
		}
	}

	private Texture2D GenerateGradient(Gradient gradient, int gWidth, int gHeight)
	{
		Texture2D texture2D = new Texture2D(gWidth, gHeight, TextureFormat.ARGB32, false);
		texture2D.wrapMode = TextureWrapMode.Clamp;
		texture2D.hideFlags = HideFlags.HideAndDontSave;
		Color color = Color.white;
		if (gradient != null)
		{
			for (int i = 0; i < gWidth; i++)
			{
				color = gradient.Evaluate((float)i / (float)gWidth);
				for (int j = 0; j < gHeight; j++)
				{
					texture2D.SetPixel(i, j, color);
				}
			}
		}
		texture2D.Apply();
		return texture2D;
	}

	private void createResources()
	{
		if (this.fogShader == null)
		{
			this.fogShader = Shader.Find("Hidden/StylizedFog");
		}
		if (this.fogMat == null && this.fogShader != null)
		{
			this.fogMat = new Material(this.fogShader);
			this.fogMat.hideFlags = HideFlags.HideAndDontSave;
		}
		if (!(this.mainRamp == null))
		{
			if (!(this.blendRamp == null))
			{
				goto IL_A9;
			}
		}
		this.setGradient();
		IL_A9:
		if (this.cam == null)
		{
			this.cam = base.GetComponent<Camera>();
			this.cam.depthTextureMode |= DepthTextureMode.Depth;
		}
	}

	private void clearResources()
	{
		if (this.fogMat != null)
		{
			UnityEngine.Object.DestroyImmediate(this.fogMat);
		}
		this.disableKeywords();
		this.cam.depthTextureMode = DepthTextureMode.None;
	}

	public void SetKeywords()
	{
		switch (this.fogMode)
		{
		case StylizedFog.StylizedFogMode.Blend:
			Shader.EnableKeyword("_FOG_BLEND");
			Shader.DisableKeyword("_FOG_ADDITIVE");
			Shader.DisableKeyword("_FOG_MULTIPLY");
			Shader.DisableKeyword("_FOG_SCREEN");
			break;
		case StylizedFog.StylizedFogMode.Additive:
			Shader.DisableKeyword("_FOG_BLEND");
			Shader.EnableKeyword("_FOG_ADDITIVE");
			Shader.DisableKeyword("_FOG_MULTIPLY");
			Shader.DisableKeyword("_FOG_SCREEN");
			break;
		case StylizedFog.StylizedFogMode.Multiply:
			Shader.DisableKeyword("_FOG_BLEND");
			Shader.DisableKeyword("_FOG_ADDITIVE");
			Shader.EnableKeyword("_FOG_MULTIPLY");
			Shader.DisableKeyword("_FOG_SCREEN");
			break;
		case StylizedFog.StylizedFogMode.Screen:
			Shader.DisableKeyword("_FOG_BLEND");
			Shader.DisableKeyword("_FOG_ADDITIVE");
			Shader.DisableKeyword("_FOG_MULTIPLY");
			Shader.EnableKeyword("_FOG_SCREEN");
			break;
		}
		if (this.useBlend)
		{
			Shader.EnableKeyword("_FOG_BLEND_ON");
			Shader.DisableKeyword("_FOG_BLEND_OFF");
		}
		else
		{
			Shader.EnableKeyword("_FOG_BLEND_OFF");
			Shader.DisableKeyword("_FOG_BLEND_ON");
		}
		if (this.useNoise)
		{
			Shader.EnableKeyword("_FOG_NOISE_ON");
			Shader.DisableKeyword("_FOG_NOISE_OFF");
		}
		else
		{
			Shader.EnableKeyword("_FOG_NOISE_OFF");
			Shader.DisableKeyword("_FOG_NOISE_ON");
		}
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
			return false;
		}
		if (this.fogShader.isSupported)
		{
			if (!(this.fogShader == null))
			{
				return true;
			}
		}
		return false;
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.isSupported())
		{
			Graphics.Blit(source, destination);
			return;
		}
		this.updateValues();
		Graphics.Blit(source, destination, this.fogMat);
	}

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
}
