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

	internal static DepthTextureRenderer Instance { get; private set; }

	private void Start()
	{
		DepthTextureRenderer.Instance = this;
		for (int i = 0; i < this.m_layerNames.Length; i++)
		{
			this.m_layersMask |= 1 << LayerMask.NameToLayer(this.m_layerNames[i]);
		}
		this.m_depthTextureNameGlobalID = Shader.PropertyToID(this.m_depthTextureNameGlobal);
	}

	private void OnDestroy()
	{
		DepthTextureRenderer.Instance = null;
	}

	private bool IsSupported()
	{
		return SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth);
	}

	internal bool IsFunctioning()
	{
		bool result;
		if (this.IsSupported() && Options_UI.Get() != null)
		{
			result = (Options_UI.Get().GetCurrentGraphicsQuality() >= this.m_minGraphicsQuality);
		}
		else
		{
			result = false;
		}
		return result;
	}

	internal RenderTexture GetRenderTexture()
	{
		if (this.m_depthTex == null)
		{
			if (this.m_depthCam != null)
			{
				try
				{
					this.m_depthTex = RenderTexture.GetTemporary(Screen.width, Screen.height, 0x18, RenderTextureFormat.Depth);
					this.m_depthTex.name = this.m_depthTextureNameGlobal;
					RenderTexture depthTex = this.m_depthTex;
					bool isPowerOfTwo;
					if (Mathf.IsPowerOfTwo(this.m_depthCam.pixelWidth))
					{
						isPowerOfTwo = Mathf.IsPowerOfTwo(this.m_depthCam.pixelHeight);
					}
					else
					{
						isPowerOfTwo = false;
					}
					depthTex.isPowerOfTwo = isPowerOfTwo;
					Shader.SetGlobalTexture(this.m_depthTextureNameGlobalID, this.m_depthTex);
				}
				catch (Exception ex)
				{
					Log.Error("Failed to create {0}. {1}:{2}. Transparent environment objects will look incorrect temporarily.", new object[]
					{
						this.m_depthTextureNameGlobal,
						ex,
						ex.Message
					});
				}
			}
		}
		return this.m_depthTex;
	}

	private void CleanUpTextures()
	{
		if (this.m_depthTex != null)
		{
			RenderTexture.ReleaseTemporary(this.m_depthTex);
			this.m_depthTex = null;
		}
	}

	private void OnPreRender()
	{
		if (base.enabled)
		{
			if (base.gameObject.activeSelf)
			{
				if (this.IsFunctioning())
				{
					this.CleanUpTextures();
					if (this.m_depthCamObj == null)
					{
						this.m_depthCamObj = new GameObject(string.Format("DepthTextureCamera_{0}", this.m_layerNames[0]));
						this.m_depthCam = this.m_depthCamObj.AddComponent<Camera>();
						this.m_depthCam.enabled = false;
					}
					if (this.GetRenderTexture() != null)
					{
						this.m_depthCam.CopyFrom(base.GetComponent<Camera>());
						this.m_depthCam.backgroundColor = new Color(0f, 0f, 0f, 0f);
						this.m_depthCam.clearFlags = CameraClearFlags.Color;
						this.m_depthCam.cullingMask = ((!this.IsFunctioning()) ? 0 : this.m_layersMask);
						this.m_depthCam.targetTexture = this.m_depthTex;
						this.m_depthCam.depthTextureMode = DepthTextureMode.None;
						this.m_depthCam.RenderWithShader(this.m_depthTextureShader, null);
					}
					return;
				}
			}
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination);
		this.CleanUpTextures();
	}

	private void OnDisable()
	{
		if (this.m_depthCamObj)
		{
			UnityEngine.Object.DestroyImmediate(this.m_depthCamObj);
		}
	}
}
