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
		materialColorProperty = Shader.PropertyToID("_Color");
		materialSrcBlendProperty = Shader.PropertyToID("_SrcBlend");
		materialDstBlendProperty = Shader.PropertyToID("_DstBlend");
		materialZWriteProperty = Shader.PropertyToID("_ZWrite");
		materialModeProperty = Shader.PropertyToID("_Mode");
		materialAlphaProperty = Shader.PropertyToID("_VerticalFadeAlpha");
		materialMaxWorldYProperty = Shader.PropertyToID("_MaxWorldY");
	}

	internal bool ShouldProcessEvenIfRendererIsDisabled()
	{
		return (float)m_alphaMultBottom == 0f;
	}

	private bool ShouldDoHeightFade()
	{
		Vector3 position = Camera.main.transform.position;
		return position.y - (float)Board.Get().BaselineHeight <= FadeObjectsCameraComponent.Get().m_cameraHeightForVerticalFade;
	}

	internal void SetTargetTransparency(float transparency, float fadeOutDuration, float fadeInDuration, Shader transparentShader)
	{
		if (m_rendererComponent == null)
		{
			while (true)
			{
				switch (6)
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
			m_rendererComponent = GetComponent<Renderer>();
		}
		m_setTargetTransparencyTime = Time.time;
		m_alphaMultBottom.EaseTo(transparency, fadeOutDuration);
		EasedFloatCubic alphaMultTop = m_alphaMultTop;
		float endValue;
		if (ShouldDoHeightFade())
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
			endValue = 0f;
		}
		else
		{
			endValue = transparency;
		}
		alphaMultTop.EaseTo(endValue, fadeOutDuration);
		m_fadeInDuration = fadeInDuration;
		if (!base.enabled || m_originalMaterials == null)
		{
			if (m_originalMaterials == null)
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
				m_originalMaterials = (Material[])m_rendererComponent.sharedMaterials.Clone();
			}
			if (m_originalAlphas == null)
			{
				m_originalAlphas = new float[m_rendererComponent.sharedMaterials.Length];
				for (int i = 0; i < m_rendererComponent.sharedMaterials.Length; i++)
				{
					if (m_rendererComponent.sharedMaterials[i] != null)
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
						if (m_rendererComponent.sharedMaterials[i].HasProperty(materialColorProperty))
						{
							float[] originalAlphas = m_originalAlphas;
							int num = i;
							Color color = m_rendererComponent.sharedMaterials[i].color;
							originalAlphas[num] = color.a;
							continue;
						}
					}
					m_originalAlphas[i] = 1f;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (m_fadeMaterials == null)
			{
				m_fadeMaterials = new Material[m_rendererComponent.sharedMaterials.Length];
				for (int j = 0; j < m_rendererComponent.sharedMaterials.Length; j++)
				{
					if (m_rendererComponent.sharedMaterials[j] != null)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						Material material = new Material(m_rendererComponent.sharedMaterials[j]);
						material.shader = transparentShader;
						material.SetOverrideTag("RenderType", "Transparent");
						material.SetInt(materialSrcBlendProperty, 5);
						material.SetInt(materialDstBlendProperty, 10);
						material.SetInt(materialZWriteProperty, 0);
						material.SetInt(materialModeProperty, 2);
						material.shaderKeywords = m_rendererComponent.sharedMaterials[j].shaderKeywords;
						material.DisableKeyword("_ALPHATEST_ON");
						material.EnableKeyword("_ALPHABLEND_ON");
						material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
						material.renderQueue = 3000;
						int nameID = materialAlphaProperty;
						Color color2 = material.color;
						material.SetFloat(nameID, color2.a);
						material.SetFloat(materialMaxWorldYProperty, 99999f);
						m_fadeMaterials[j] = material;
					}
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			m_rendererComponent.materials = m_fadeMaterials;
		}
		base.enabled = true;
	}

	private void Update()
	{
		if (Board.Get() == null || m_rendererComponent == null)
		{
			return;
		}
		if (FadeObjectsCameraComponent.Get() == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		if ((float)m_alphaMultBottom < 1f && m_originalAlphas != null)
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
			if (Time.time - m_setTargetTransparencyTime > 0.25f)
			{
				m_alphaMultBottom.EaseTo(1f, m_fadeInDuration);
				m_alphaMultTop.EaseTo(1f, m_fadeInDuration);
			}
			FadeObjectsCameraComponent fadeObjectsCameraComponent = FadeObjectsCameraComponent.Get();
			m_needToDestroyOldMaterials = true;
			for (int i = 0; i < m_rendererComponent.materials.Length; i++)
			{
				Material material = m_rendererComponent.materials[i];
				if (material == null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					continue;
				}
				m_rendererComponent.enabled = ((float)m_alphaMultBottom * m_originalAlphas[i] > 0f);
				material.SetFloat(materialAlphaProperty, (float)m_alphaMultTop * m_originalAlphas[i]);
				if (!(fadeObjectsCameraComponent != null))
				{
					continue;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				int nameID = materialMaxWorldYProperty;
				float value;
				if ((float)m_alphaMultTop <= fadeObjectsCameraComponent.m_minAlphaTopDepth)
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
					value = (float)Board.Get().BaselineHeight + fadeObjectsCameraComponent.m_fadeEndFloorOffset;
				}
				else
				{
					value = 99999f;
				}
				material.SetFloat(nameID, value);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		else if (m_alphaMultBottom.EaseFinished())
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
			if (m_originalMaterials != null)
			{
				Material[] materials = m_rendererComponent.materials;
				m_rendererComponent.materials = m_originalMaterials;
				if (m_needToDestroyOldMaterials)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (materials != null)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						DestroyMaterials(materials);
					}
					m_needToDestroyOldMaterials = false;
				}
			}
			base.enabled = false;
		}
		if ((float)m_alphaMultBottom == 0f)
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
			if (m_rendererComponent.enabled)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						m_rendererComponent.enabled = false;
						return;
					}
				}
			}
		}
		if ((float)m_alphaMultBottom == 0f)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (!m_rendererComponent.enabled)
			{
				m_rendererComponent.enabled = true;
			}
			return;
		}
	}

	private void DestroyMaterials(Material[] mats)
	{
		if (mats == null)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			foreach (Material obj in mats)
			{
				Object.Destroy(obj);
			}
			while (true)
			{
				switch (6)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private void OnDestroy()
	{
		if (m_fadeMaterials == null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int i = 0; i < m_fadeMaterials.Length; i++)
			{
				Object.Destroy(m_fadeMaterials[i]);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				m_fadeMaterials = null;
				return;
			}
		}
	}

	public float GetCurrentAlpha()
	{
		return m_alphaMultBottom;
	}
}
