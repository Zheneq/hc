using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class SceneCameraFogSettings : MonoBehaviour
{
	public bool m_distanceFog;

	public bool m_useRadialDistance;

	public bool m_heightFog;

	public float m_height;

	[Range(0f, 10f)]
	public float m_heightDensity;

	public float m_startDistance;

	public Shader m_fogShader;

	public Bloom.BloomQuality m_bloomQuality;

	public Bloom.TweakMode m_bloomMode;

	public Bloom.BloomScreenBlendMode m_bloomBlendMode;

	public Bloom.HDRBloomMode m_bloomHDR;

	public float m_bloomIntensity;

	[Range(-0.05f, 4f)]
	public float m_bloomThreshold;

	[Range(1f, 4f)]
	public int m_bloomBlurIterations;

	[Range(0.1f, 10f)]
	public float m_bloomSampleDistance;

	private bool m_fogParametersSet;

	private bool m_bloomParameterSet;

	private void Update()
	{
		GameObject gameObject = null;
		if (CameraManager.Get() == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SceneCameraFogSettings.Update()).MethodHandle;
			}
			if (UIManager.Get() != null)
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
				if (UIManager.Get().GetEnvirontmentCamera() != null)
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
					gameObject = UIManager.Get().GetEnvirontmentCamera().gameObject;
				}
			}
		}
		else if (Camera.main != null)
		{
			gameObject = Camera.main.gameObject;
		}
		if (gameObject != null)
		{
			if (!this.m_fogParametersSet)
			{
				GlobalFog component = gameObject.GetComponent<GlobalFog>();
				if (component != null)
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
					this.m_fogParametersSet = true;
					component.distanceFog = this.m_distanceFog;
					component.useRadialDistance = this.m_useRadialDistance;
					component.heightFog = this.m_heightFog;
					component.height = this.m_height;
					component.heightDensity = this.m_heightDensity;
					component.startDistance = this.m_startDistance;
					component.fogShader = this.m_fogShader;
				}
			}
			if (!this.m_bloomParameterSet)
			{
				Bloom component2 = gameObject.GetComponent<Bloom>();
				if (component2 != null)
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
					this.m_bloomParameterSet = true;
					component2.quality = this.m_bloomQuality;
					component2.tweakMode = this.m_bloomMode;
					component2.screenBlendMode = this.m_bloomBlendMode;
					component2.hdr = this.m_bloomHDR;
					component2.bloomIntensity = this.m_bloomIntensity;
					component2.bloomThreshold = this.m_bloomThreshold;
					component2.sepBlurSpread = this.m_bloomSampleDistance;
				}
			}
		}
	}
}
