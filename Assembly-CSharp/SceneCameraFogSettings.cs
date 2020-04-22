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
			while (true)
			{
				switch (3)
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
			if (UIManager.Get() != null)
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
				if (UIManager.Get().GetEnvirontmentCamera() != null)
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
					gameObject = UIManager.Get().GetEnvirontmentCamera().gameObject;
				}
			}
		}
		else if (Camera.main != null)
		{
			gameObject = Camera.main.gameObject;
		}
		if (!(gameObject != null))
		{
			return;
		}
		if (!m_fogParametersSet)
		{
			GlobalFog component = gameObject.GetComponent<GlobalFog>();
			if (component != null)
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
				m_fogParametersSet = true;
				component.distanceFog = m_distanceFog;
				component.useRadialDistance = m_useRadialDistance;
				component.heightFog = m_heightFog;
				component.height = m_height;
				component.heightDensity = m_heightDensity;
				component.startDistance = m_startDistance;
				component.fogShader = m_fogShader;
			}
		}
		if (m_bloomParameterSet)
		{
			return;
		}
		Bloom component2 = gameObject.GetComponent<Bloom>();
		if (!(component2 != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			m_bloomParameterSet = true;
			component2.quality = m_bloomQuality;
			component2.tweakMode = m_bloomMode;
			component2.screenBlendMode = m_bloomBlendMode;
			component2.hdr = m_bloomHDR;
			component2.bloomIntensity = m_bloomIntensity;
			component2.bloomThreshold = m_bloomThreshold;
			component2.sepBlurSpread = m_bloomSampleDistance;
			return;
		}
	}
}
