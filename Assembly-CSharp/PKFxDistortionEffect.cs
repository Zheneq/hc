using System;
using UnityEngine;

internal class PKFxDistortionEffect : MonoBehaviour
{
	public Material m_MaterialDistortion;

	public Material m_MaterialBlur;

	public float m_BlurFactor;

	private RenderTexture m_TmpRT;

	public RenderTexture _DistortionRT;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!(this._DistortionRT == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PKFxDistortionEffect.OnRenderImage(RenderTexture, RenderTexture)).MethodHandle;
			}
			if (this._DistortionRT.IsCreated())
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
				if (this.m_MaterialDistortion == null)
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
				}
				else
				{
					if (this.m_MaterialBlur == null)
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
						this.m_MaterialDistortion.SetTexture("_DistortionTex", this._DistortionRT);
						Graphics.Blit(source, destination, this.m_MaterialDistortion);
						return;
					}
					this.m_MaterialBlur.SetTexture("_DistortionTex", this._DistortionRT);
					this.m_MaterialBlur.SetFloat("_BlurFactor", this.m_BlurFactor);
					this.m_MaterialDistortion.SetTexture("_DistortionTex", this._DistortionRT);
					this.m_TmpRT = RenderTexture.GetTemporary(source.width, source.height, source.volumeDepth, source.format);
					Graphics.Blit(source, this.m_TmpRT, this.m_MaterialDistortion);
					Graphics.Blit(this.m_TmpRT, destination, this.m_MaterialBlur);
					RenderTexture.ReleaseTemporary(this.m_TmpRT);
					return;
				}
			}
		}
		Graphics.Blit(source, destination);
		Debug.LogError("[PKFX] Distortion effect setup failed.");
	}
}
