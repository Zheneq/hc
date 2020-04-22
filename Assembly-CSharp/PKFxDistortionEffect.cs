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
		if (!(_DistortionRT == null))
		{
			if (_DistortionRT.IsCreated())
			{
				if (!(m_MaterialDistortion == null))
				{
					if (m_MaterialBlur == null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								m_MaterialDistortion.SetTexture("_DistortionTex", _DistortionRT);
								Graphics.Blit(source, destination, m_MaterialDistortion);
								return;
							}
						}
					}
					m_MaterialBlur.SetTexture("_DistortionTex", _DistortionRT);
					m_MaterialBlur.SetFloat("_BlurFactor", m_BlurFactor);
					m_MaterialDistortion.SetTexture("_DistortionTex", _DistortionRT);
					m_TmpRT = RenderTexture.GetTemporary(source.width, source.height, source.volumeDepth, source.format);
					Graphics.Blit(source, m_TmpRT, m_MaterialDistortion);
					Graphics.Blit(m_TmpRT, destination, m_MaterialBlur);
					RenderTexture.ReleaseTemporary(m_TmpRT);
					return;
				}
			}
		}
		Graphics.Blit(source, destination);
		Debug.LogError("[PKFX] Distortion effect setup failed.");
	}
}
