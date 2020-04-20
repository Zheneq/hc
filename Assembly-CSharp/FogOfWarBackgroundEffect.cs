using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class FogOfWarBackgroundEffect : ImageEffectBase
{
	private RenderTexture m_sourceTexture;

	private RenderTexture m_renderTexture;

	public RenderTexture GetSourceTexture()
	{
		return this.m_sourceTexture;
	}

	private int GetResolutionBasedOnScreenResolution()
	{
		if (Screen.currentResolution.width > 0xC00)
		{
			return 0x1000;
		}
		if (Screen.currentResolution.width > 0x800)
		{
			return 0x800;
		}
		return 0x400;
	}

	private void InitializeSourceTexture()
	{
		if (!this.m_sourceTexture)
		{
			int resolutionBasedOnScreenResolution = this.GetResolutionBasedOnScreenResolution();
			this.m_sourceTexture = new RenderTexture(resolutionBasedOnScreenResolution, resolutionBasedOnScreenResolution, 0);
			this.m_sourceTexture.name = "UnseenRenderTexture";
			this.m_sourceTexture.hideFlags = HideFlags.HideAndDontSave;
			this.m_sourceTexture.isPowerOfTwo = true;
			this.m_sourceTexture.useMipMap = true;
			this.m_sourceTexture.autoGenerateMips = true;
			this.m_sourceTexture.Create();
		}
	}

	public RenderTexture GetRenderTexture()
	{
		return this.m_renderTexture;
	}

	private void InitializeRenderTexture()
	{
		if (!this.m_renderTexture)
		{
			int resolutionBasedOnScreenResolution = this.GetResolutionBasedOnScreenResolution();
			this.m_renderTexture = new RenderTexture(resolutionBasedOnScreenResolution, resolutionBasedOnScreenResolution, 0);
			this.m_renderTexture.name = "OutputRenderTexture";
			this.m_renderTexture.hideFlags = HideFlags.HideAndDontSave;
			this.m_renderTexture.isPowerOfTwo = true;
			Shader.SetGlobalTexture("_FogOfWarScreenTex", this.GetSourceTexture());
		}
	}

	private void OnEnable()
	{
		this.InitializeSourceTexture();
		this.InitializeRenderTexture();
		Camera component = base.GetComponent<Camera>();
		if (component)
		{
			component.targetTexture = this.GetRenderTexture();
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		Camera component = base.GetComponent<Camera>();
		if (component)
		{
			component.targetTexture = null;
		}
		if (this.m_sourceTexture)
		{
			UnityEngine.Object.DestroyImmediate(this.m_sourceTexture);
			this.m_sourceTexture = null;
		}
		if (this.m_renderTexture)
		{
			UnityEngine.Object.DestroyImmediate(this.m_renderTexture);
			this.m_renderTexture = null;
		}
	}

	protected override void Start()
	{
		this.InitializeSourceTexture();
		this.InitializeRenderTexture();
		Camera component = base.GetComponent<Camera>();
		if (component)
		{
			component.targetTexture = this.GetRenderTexture();
		}
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.GetSourceTexture())
		{
			return;
		}
		Graphics.Blit(this.GetSourceTexture(), destination);
	}
}
