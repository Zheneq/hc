using System;
using UnityEngine;

[ExecuteInEditMode]
public class DepthTextureRenderer : MonoBehaviour
{
	[Tooltip("Should be Hydrogen/DepthTextureRender, need reference to fix build.")]
	public Shader m_depthTextureShader;

	public string[] m_layerNames;

	public string m_depthTextureNameGlobal = "_DepthTextureTransparentObjects";

	public GraphicsQuality m_minGraphicsQuality = GraphicsQuality.Medium;

	private int m_layersMask;

	private RenderTexture m_depthTex;

	private GameObject m_depthCamObj;

	private Camera m_depthCam;

	private int m_depthTextureNameGlobalID;

	internal static DepthTextureRenderer Instance
	{
		get;
		private set;
	}

	private void Start()
	{
		Instance = this;
		for (int i = 0; i < m_layerNames.Length; i++)
		{
			m_layersMask |= 1 << LayerMask.NameToLayer(m_layerNames[i]);
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
			m_depthTextureNameGlobalID = Shader.PropertyToID(m_depthTextureNameGlobal);
			return;
		}
	}

	private void OnDestroy()
	{
		Instance = null;
	}

	private bool IsSupported()
	{
		return SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth);
	}

	internal bool IsFunctioning()
	{
		int result;
		if (IsSupported() && Options_UI.Get() != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = ((Options_UI.Get().GetCurrentGraphicsQuality() >= m_minGraphicsQuality) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	internal RenderTexture GetRenderTexture()
	{
		if (m_depthTex == null)
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
			if (m_depthCam != null)
			{
				try
				{
					m_depthTex = RenderTexture.GetTemporary(Screen.width, Screen.height, 24, RenderTextureFormat.Depth);
					m_depthTex.name = m_depthTextureNameGlobal;
					RenderTexture depthTex = m_depthTex;
					int isPowerOfTwo;
					if (Mathf.IsPowerOfTwo(m_depthCam.pixelWidth))
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
						isPowerOfTwo = (Mathf.IsPowerOfTwo(m_depthCam.pixelHeight) ? 1 : 0);
					}
					else
					{
						isPowerOfTwo = 0;
					}
					depthTex.isPowerOfTwo = ((byte)isPowerOfTwo != 0);
					Shader.SetGlobalTexture(m_depthTextureNameGlobalID, m_depthTex);
				}
				catch (Exception ex)
				{
					Log.Error("Failed to create {0}. {1}:{2}. Transparent environment objects will look incorrect temporarily.", m_depthTextureNameGlobal, ex, ex.Message);
				}
			}
		}
		return m_depthTex;
	}

	private void CleanUpTextures()
	{
		if (!(m_depthTex != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			RenderTexture.ReleaseTemporary(m_depthTex);
			m_depthTex = null;
			return;
		}
	}

	private void OnPreRender()
	{
		if (!base.enabled)
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
			if (!base.gameObject.activeSelf)
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
				if (!IsFunctioning())
				{
					while (true)
					{
						switch (5)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
				CleanUpTextures();
				if (m_depthCamObj == null)
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
					m_depthCamObj = new GameObject($"DepthTextureCamera_{m_layerNames[0]}");
					m_depthCam = m_depthCamObj.AddComponent<Camera>();
					m_depthCam.enabled = false;
				}
				if (GetRenderTexture() != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						m_depthCam.CopyFrom(GetComponent<Camera>());
						m_depthCam.backgroundColor = new Color(0f, 0f, 0f, 0f);
						m_depthCam.clearFlags = CameraClearFlags.Color;
						m_depthCam.cullingMask = (IsFunctioning() ? m_layersMask : 0);
						m_depthCam.targetTexture = m_depthTex;
						m_depthCam.depthTextureMode = DepthTextureMode.None;
						m_depthCam.RenderWithShader(m_depthTextureShader, null);
						return;
					}
				}
				return;
			}
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination);
		CleanUpTextures();
	}

	private void OnDisable()
	{
		if (!m_depthCamObj)
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
			UnityEngine.Object.DestroyImmediate(m_depthCamObj);
			return;
		}
	}
}
