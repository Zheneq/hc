using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Blur/Blur (Optimized) C#")]
internal class BlurCS : PostEffectsCSBase
{
	[Range(0f, 2f)]
	public int downsample = 1;

	[Range(0f, 10f)]
	public float blurSize = 3f;

	[Range(1f, 4f)]
	public int blurIterations = 2;

	public int blurType;

	public Shader blurShader;

	private Material blurMaterial;

	public override bool CheckResources()
	{
		base.CheckSupport(false);
		this.blurMaterial = base.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
		if (!this.isSupported)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlurCS.CheckResources()).MethodHandle;
			}
			base.ReportAutoDisable();
		}
		return this.isSupported;
	}

	private void OnDisable()
	{
		if (this.blurMaterial)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlurCS.OnDisable()).MethodHandle;
			}
			UnityEngine.Object.DestroyImmediate(this.blurMaterial);
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.CheckResources())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlurCS.OnRenderImage(RenderTexture, RenderTexture)).MethodHandle;
			}
			Graphics.Blit(source, destination);
			return;
		}
		float num = 1f / (1f * (float)(1 << this.downsample));
		this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num, -this.blurSize * num, 0f, 0f));
		source.filterMode = FilterMode.Bilinear;
		int width = source.width >> this.downsample;
		int height = source.height >> this.downsample;
		RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
		renderTexture.filterMode = FilterMode.Bilinear;
		Graphics.Blit(source, renderTexture, this.blurMaterial, 0);
		int num2;
		if (this.blurType == 0)
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
			num2 = 0;
		}
		else
		{
			num2 = 2;
		}
		int num3 = num2;
		for (int i = 0; i < this.blurIterations; i++)
		{
			float num4 = (float)i * 1f;
			this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num + num4, -this.blurSize * num - num4, 0f, 0f));
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
			temporary.filterMode = FilterMode.Bilinear;
			Graphics.Blit(renderTexture, temporary, this.blurMaterial, 1 + num3);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary;
			temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
			temporary.filterMode = FilterMode.Bilinear;
			Graphics.Blit(renderTexture, temporary, this.blurMaterial, 2 + num3);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary;
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		Graphics.Blit(renderTexture, destination);
		RenderTexture.ReleaseTemporary(renderTexture);
	}

	public enum BlurType
	{
		StandardGauss,
		SgxGauss
	}
}
