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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWarBackgroundEffect.GetResolutionBasedOnScreenResolution()).MethodHandle;
			}
			return 0x1000;
		}
		if (Screen.currentResolution.width > 0x800)
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
			return 0x800;
		}
		return 0x400;
	}

	private void InitializeSourceTexture()
	{
		if (!this.m_sourceTexture)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWarBackgroundEffect.InitializeSourceTexture()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWarBackgroundEffect.InitializeRenderTexture()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWarBackgroundEffect.OnEnable()).MethodHandle;
			}
			component.targetTexture = this.GetRenderTexture();
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		Camera component = base.GetComponent<Camera>();
		if (component)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWarBackgroundEffect.OnDisable()).MethodHandle;
			}
			component.targetTexture = null;
		}
		if (this.m_sourceTexture)
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
			UnityEngine.Object.DestroyImmediate(this.m_sourceTexture);
			this.m_sourceTexture = null;
		}
		if (this.m_renderTexture)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWarBackgroundEffect.Start()).MethodHandle;
			}
			component.targetTexture = this.GetRenderTexture();
		}
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.GetSourceTexture())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWarBackgroundEffect.OnRenderImage(RenderTexture, RenderTexture)).MethodHandle;
			}
			return;
		}
		Graphics.Blit(this.GetSourceTexture(), destination);
	}
}
