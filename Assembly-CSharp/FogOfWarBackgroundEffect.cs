using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class FogOfWarBackgroundEffect : ImageEffectBase
{
	private RenderTexture m_sourceTexture;

	private RenderTexture m_renderTexture;

	public RenderTexture GetSourceTexture()
	{
		return m_sourceTexture;
	}

	private int GetResolutionBasedOnScreenResolution()
	{
		if (Screen.currentResolution.width > 3072)
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
					return 4096;
				}
			}
		}
		if (Screen.currentResolution.width > 2048)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return 2048;
				}
			}
		}
		return 1024;
	}

	private void InitializeSourceTexture()
	{
		if ((bool)m_sourceTexture)
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
			int resolutionBasedOnScreenResolution = GetResolutionBasedOnScreenResolution();
			m_sourceTexture = new RenderTexture(resolutionBasedOnScreenResolution, resolutionBasedOnScreenResolution, 0);
			m_sourceTexture.name = "UnseenRenderTexture";
			m_sourceTexture.hideFlags = HideFlags.HideAndDontSave;
			m_sourceTexture.isPowerOfTwo = true;
			m_sourceTexture.useMipMap = true;
			m_sourceTexture.autoGenerateMips = true;
			m_sourceTexture.Create();
			return;
		}
	}

	public RenderTexture GetRenderTexture()
	{
		return m_renderTexture;
	}

	private void InitializeRenderTexture()
	{
		if ((bool)m_renderTexture)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			int resolutionBasedOnScreenResolution = GetResolutionBasedOnScreenResolution();
			m_renderTexture = new RenderTexture(resolutionBasedOnScreenResolution, resolutionBasedOnScreenResolution, 0);
			m_renderTexture.name = "OutputRenderTexture";
			m_renderTexture.hideFlags = HideFlags.HideAndDontSave;
			m_renderTexture.isPowerOfTwo = true;
			Shader.SetGlobalTexture("_FogOfWarScreenTex", GetSourceTexture());
			return;
		}
	}

	private void OnEnable()
	{
		InitializeSourceTexture();
		InitializeRenderTexture();
		Camera component = GetComponent<Camera>();
		if (!component)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			component.targetTexture = GetRenderTexture();
			return;
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		Camera component = GetComponent<Camera>();
		if ((bool)component)
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
			component.targetTexture = null;
		}
		if ((bool)m_sourceTexture)
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
			Object.DestroyImmediate(m_sourceTexture);
			m_sourceTexture = null;
		}
		if (!m_renderTexture)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			Object.DestroyImmediate(m_renderTexture);
			m_renderTexture = null;
			return;
		}
	}

	protected override void Start()
	{
		InitializeSourceTexture();
		InitializeRenderTexture();
		Camera component = GetComponent<Camera>();
		if (!component)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			component.targetTexture = GetRenderTexture();
			return;
		}
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!GetSourceTexture())
		{
			while (true)
			{
				switch (7)
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
		Graphics.Blit(GetSourceTexture(), destination);
	}
}
