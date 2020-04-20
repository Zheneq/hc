using System;
using UnityEngine;

public class FadeObject : MonoBehaviour
{
	private float[] m_originalAlphas;

	private EasedFloatCubic m_alphaMultBottom = new EasedFloatCubic(1f);

	private EasedFloatCubic m_alphaMultTop = new EasedFloatCubic(1f);

	private bool m_needToDestroyOldMaterials;

	private float m_fadeInDuration;

	private float m_setTargetTransparencyTime;

	private Renderer m_rendererComponent;

	private Material[] m_originalMaterials;

	private Material[] m_fadeMaterials;

	private static int materialColorProperty;

	private static int materialSrcBlendProperty;

	private static int materialDstBlendProperty;

	private static int materialZWriteProperty;

	private static int materialModeProperty;

	private static int materialAlphaProperty;

	private static int materialMaxWorldYProperty;

	private void Awake()
	{
		FadeObject.materialColorProperty = Shader.PropertyToID("_Color");
		FadeObject.materialSrcBlendProperty = Shader.PropertyToID("_SrcBlend");
		FadeObject.materialDstBlendProperty = Shader.PropertyToID("_DstBlend");
		FadeObject.materialZWriteProperty = Shader.PropertyToID("_ZWrite");
		FadeObject.materialModeProperty = Shader.PropertyToID("_Mode");
		FadeObject.materialAlphaProperty = Shader.PropertyToID("_VerticalFadeAlpha");
		FadeObject.materialMaxWorldYProperty = Shader.PropertyToID("_MaxWorldY");
	}

	internal bool ShouldProcessEvenIfRendererIsDisabled()
	{
		return this.m_alphaMultBottom == 0f;
	}

	private bool ShouldDoHeightFade()
	{
		return Camera.main.transform.position.y - (float)Board.Get().BaselineHeight <= FadeObjectsCameraComponent.Get().m_cameraHeightForVerticalFade;
	}

	internal void SetTargetTransparency(float transparency, float fadeOutDuration, float fadeInDuration, Shader transparentShader)
	{
		if (this.m_rendererComponent == null)
		{
			this.m_rendererComponent = base.GetComponent<Renderer>();
		}
		this.m_setTargetTransparencyTime = Time.time;
		this.m_alphaMultBottom.EaseTo(transparency, fadeOutDuration);
		Eased<float> alphaMultTop = this.m_alphaMultTop;
		float endValue;
		if (this.ShouldDoHeightFade())
		{
			endValue = 0f;
		}
		else
		{
			endValue = transparency;
		}
		alphaMultTop.EaseTo(endValue, fadeOutDuration);
		this.m_fadeInDuration = fadeInDuration;
		if (!base.enabled || this.m_originalMaterials == null)
		{
			if (this.m_originalMaterials == null)
			{
				this.m_originalMaterials = (Material[])this.m_rendererComponent.sharedMaterials.Clone();
			}
			if (this.m_originalAlphas == null)
			{
				this.m_originalAlphas = new float[this.m_rendererComponent.sharedMaterials.Length];
				int i = 0;
				while (i < this.m_rendererComponent.sharedMaterials.Length)
				{
					if (!(this.m_rendererComponent.sharedMaterials[i] != null))
					{
						goto IL_154;
					}
					if (!this.m_rendererComponent.sharedMaterials[i].HasProperty(FadeObject.materialColorProperty))
					{
						goto IL_154;
					}
					this.m_originalAlphas[i] = this.m_rendererComponent.sharedMaterials[i].color.a;
					IL_161:
					i++;
					continue;
					IL_154:
					this.m_originalAlphas[i] = 1f;
					goto IL_161;
				}
			}
			if (this.m_fadeMaterials == null)
			{
				this.m_fadeMaterials = new Material[this.m_rendererComponent.sharedMaterials.Length];
				for (int j = 0; j < this.m_rendererComponent.sharedMaterials.Length; j++)
				{
					if (this.m_rendererComponent.sharedMaterials[j] != null)
					{
						Material material = new Material(this.m_rendererComponent.sharedMaterials[j]);
						material.shader = transparentShader;
						material.SetOverrideTag("RenderType", "Transparent");
						material.SetInt(FadeObject.materialSrcBlendProperty, 5);
						material.SetInt(FadeObject.materialDstBlendProperty, 0xA);
						material.SetInt(FadeObject.materialZWriteProperty, 0);
						material.SetInt(FadeObject.materialModeProperty, 2);
						material.shaderKeywords = this.m_rendererComponent.sharedMaterials[j].shaderKeywords;
						material.DisableKeyword("_ALPHATEST_ON");
						material.EnableKeyword("_ALPHABLEND_ON");
						material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
						material.renderQueue = 0xBB8;
						material.SetFloat(FadeObject.materialAlphaProperty, material.color.a);
						material.SetFloat(FadeObject.materialMaxWorldYProperty, 99999f);
						this.m_fadeMaterials[j] = material;
					}
				}
			}
			this.m_rendererComponent.materials = this.m_fadeMaterials;
		}
		base.enabled = true;
	}

	private void Update()
	{
		if (!(Board.Get() == null) && !(this.m_rendererComponent == null))
		{
			if (!(FadeObjectsCameraComponent.Get() == null))
			{
				if (this.m_alphaMultBottom < 1f && this.m_originalAlphas != null)
				{
					if (Time.time - this.m_setTargetTransparencyTime > 0.25f)
					{
						this.m_alphaMultBottom.EaseTo(1f, this.m_fadeInDuration);
						this.m_alphaMultTop.EaseTo(1f, this.m_fadeInDuration);
					}
					FadeObjectsCameraComponent fadeObjectsCameraComponent = FadeObjectsCameraComponent.Get();
					this.m_needToDestroyOldMaterials = true;
					for (int i = 0; i < this.m_rendererComponent.materials.Length; i++)
					{
						Material material = this.m_rendererComponent.materials[i];
						if (material == null)
						{
						}
						else
						{
							this.m_rendererComponent.enabled = (this.m_alphaMultBottom * this.m_originalAlphas[i] > 0f);
							material.SetFloat(FadeObject.materialAlphaProperty, this.m_alphaMultTop * this.m_originalAlphas[i]);
							if (fadeObjectsCameraComponent != null)
							{
								Material material2 = material;
								int nameID = FadeObject.materialMaxWorldYProperty;
								float value;
								if (this.m_alphaMultTop <= fadeObjectsCameraComponent.m_minAlphaTopDepth)
								{
									value = (float)Board.Get().BaselineHeight + fadeObjectsCameraComponent.m_fadeEndFloorOffset;
								}
								else
								{
									value = 99999f;
								}
								material2.SetFloat(nameID, value);
							}
						}
					}
				}
				else if (this.m_alphaMultBottom.EaseFinished())
				{
					if (this.m_originalMaterials != null)
					{
						Material[] materials = this.m_rendererComponent.materials;
						this.m_rendererComponent.materials = this.m_originalMaterials;
						if (this.m_needToDestroyOldMaterials)
						{
							if (materials != null)
							{
								this.DestroyMaterials(materials);
							}
							this.m_needToDestroyOldMaterials = false;
						}
					}
					base.enabled = false;
				}
				if (this.m_alphaMultBottom == 0f)
				{
					if (this.m_rendererComponent.enabled)
					{
						this.m_rendererComponent.enabled = false;
						return;
					}
				}
				if (this.m_alphaMultBottom != 0f)
				{
					if (!this.m_rendererComponent.enabled)
					{
						this.m_rendererComponent.enabled = true;
					}
				}
				return;
			}
		}
	}

	private void DestroyMaterials(Material[] mats)
	{
		if (mats != null)
		{
			foreach (Material obj in mats)
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
	}

	private void OnDestroy()
	{
		if (this.m_fadeMaterials != null)
		{
			for (int i = 0; i < this.m_fadeMaterials.Length; i++)
			{
				UnityEngine.Object.Destroy(this.m_fadeMaterials[i]);
			}
			this.m_fadeMaterials = null;
		}
	}

	public float GetCurrentAlpha()
	{
		return this.m_alphaMultBottom;
	}
}
