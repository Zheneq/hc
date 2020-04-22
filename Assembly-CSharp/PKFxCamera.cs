using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VR;

public class PKFxCamera : PKFxPackDependent
{
	public static short g_CameraUID;

	[Tooltip("Set to true to prevent inverted axis.")]
	public bool m_FlipRendering;

	[HideInInspector]
	[Tooltip("Time multiplier for particle simulation. Range [0; 8].")]
	public float m_TimeMultiplier = 1f;

	[HideInInspector]
	[Tooltip("Specifies the particles textures level-of-detail bias.")]
	public float m_TextureLODBias = -0.5f;

	protected short m_CameraID;

	protected short m_VRReservedID;

	protected short m_CurrentCameraID;

	protected Camera m_Camera;

	protected PKFxManager.CamDesc m_CameraDescription;

	protected uint m_CurrentFrameID;

	protected uint m_LastUpdateFrameID;

	private static int m_LastFrameCount = -1;

	public RenderTexture m_DepthRT;

	private CommandBuffer m_CmdBufDepthGrabber;

	private static RenderTextureFormat g_DepthFormat = RenderTextureFormat.Depth;

	private static string g_DepthShaderName = "Hidden/PKFx Depth Copy";

	private static bool g_isDepthResolved;

	private Material m_DepthGrabMat;

	private int m_PrevScreenWidth;

	private int m_PrevScreenHeight;

	protected bool m_IsDepthCopyEnabled;

	private CommandBuffer m_CmdBufDisto;

	protected Material m_DistortionMat;

	protected Material m_DistBlurMat;

	protected RenderTexture m_DistortionRT;

	[Tooltip("Enables soft particles material")]
	[HideInInspector]
	public bool m_EnableSoftParticles;

	[HideInInspector]
	[Tooltip("Enables the distortion particles material, adding a postFX pass.")]
	public bool m_EnableDistortion;

	[Tooltip("Set to true to use the depth grabbed as a depth buffer.")]
	[HideInInspector]
	public bool m_UseDepthGrabToZTest;

	[Tooltip("Choose the depth greb texture format.")]
	[HideInInspector]
	public PKFxManager.DepthGrabFormat m_DepthGrabFormat = PKFxManager.DepthGrabFormat.Depth16Bits;

	[Tooltip("Enables the distortion blur pass, adding another postFX pass.")]
	[HideInInspector]
	public bool m_EnableBlur;

	[HideInInspector]
	[Tooltip("Blur factor. Ajusts the blur's spread.")]
	public float m_BlurFactor = 0.2f;

	private CommandBuffer m_CmdBuf;

	public int RenderPass
	{
		get
		{
			return m_CameraDescription.RenderPass;
		}
		set
		{
			m_CameraDescription.RenderPass = value;
		}
	}

	public IntPtr DepthRT
	{
		get
		{
			return m_CameraDescription.DepthRT;
		}
		set
		{
			m_CameraDescription.DepthRT = value;
		}
	}

	public static short GetUniqueID()
	{
		return g_CameraUID++;
	}

	private void Awake()
	{
		m_CameraID = GetUniqueID();
		m_CurrentCameraID = m_CameraID;
		if (Application.platform != RuntimePlatform.IPhonePlayer)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (VRSettings.enabled && VRDevice.isPresent)
			{
				m_VRReservedID = GetUniqueID();
			}
		}
		m_Camera = GetComponent<Camera>();
		m_CmdBuf = new CommandBuffer();
		m_CmdBuf.name = "PopcornFX Rendering";
		m_Camera.AddCommandBuffer((CameraEvent)PKFxManager.m_GlobalConf.globalEventSetting, m_CmdBuf);
	}

	private static bool ResolveDepthShaderAndTextureFormat()
	{
		if (g_isDepthResolved)
		{
			return false;
		}
		g_DepthShaderName = "Hidden/PKFx Depth Copy";
		if (Shader.Find(g_DepthShaderName).isSupported)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (SystemInfo.SupportsRenderTextureFormat(g_DepthFormat))
			{
				goto IL_0138;
			}
		}
		Debug.LogWarning(string.Concat("[PKFX] ", g_DepthShaderName, " shader or ", g_DepthFormat, " texture format not supported."));
		g_DepthShaderName = "Hidden/PKFx Depth Copy to Color";
		g_DepthFormat = RenderTextureFormat.RFloat;
		if (!SystemInfo.SupportsRenderTextureFormat(g_DepthFormat))
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
			Debug.LogWarning(string.Concat("[PKFX] ", g_DepthFormat, " fallback texture format not supported."));
			g_DepthFormat = RenderTextureFormat.RHalf;
			Debug.LogWarning(string.Concat("[PKFX] Resorting to ", g_DepthFormat, " (may produce artefacts)."));
		}
		Debug.LogWarning(string.Concat("[PKFX] Falling back to ", g_DepthShaderName, " shader / ", g_DepthFormat, " texture format."));
		goto IL_0138;
		IL_0138:
		g_isDepthResolved = true;
		return true;
	}

	protected bool SetupDepthGrab()
	{
		bool flag = ResolveDepthShaderAndTextureFormat();
		if (m_DepthGrabMat == null)
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
			if (!SystemInfo.SupportsRenderTextureFormat(g_DepthFormat))
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					if (flag)
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
						Debug.LogError(string.Concat("[PKFX] ", g_DepthFormat, " texture format not supported."));
						Debug.LogError("[PKFX] Soft particles/distortion disabled.");
					}
					m_IsDepthCopyEnabled = false;
					return false;
				}
			}
			m_DepthGrabMat = new Material(Shader.Find(g_DepthShaderName));
			if (m_DepthGrabMat == null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (flag)
						{
							Debug.LogError("[PKFX] Depth copy shader not found.");
							Debug.LogError("[PKFX] Soft particles/distortion disabled.");
						}
						m_IsDepthCopyEnabled = false;
						return false;
					}
				}
			}
		}
		if (m_DepthRT == null)
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
			m_DepthRT = new RenderTexture(m_Camera.pixelWidth, m_Camera.pixelHeight, (int)m_DepthGrabFormat, g_DepthFormat);
		}
		if (!m_DepthRT.IsCreated())
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
			m_DepthRT.Create();
		}
		if (m_CmdBufDepthGrabber == null)
		{
			m_CmdBufDepthGrabber = new CommandBuffer();
			m_CmdBufDepthGrabber.name = "PopcornFX Depth Grabber";
		}
		if (m_Camera.actualRenderingPath != RenderingPath.DeferredLighting)
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
			if (m_Camera.actualRenderingPath != RenderingPath.DeferredShading)
			{
				m_Camera.AddCommandBuffer(CameraEvent.AfterForwardOpaque, m_CmdBufDepthGrabber);
				goto IL_01c1;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_Camera.AddCommandBuffer(CameraEvent.AfterFinalPass, m_CmdBufDepthGrabber);
		goto IL_01c1;
		IL_01c1:
		DepthRT = m_DepthRT.GetNativeTexturePtr();
		m_Camera.depthTextureMode |= DepthTextureMode.Depth;
		m_IsDepthCopyEnabled = true;
		return true;
	}

	private void ReleaseDepthGrabResources()
	{
		if (m_CmdBufDepthGrabber != null)
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
			m_CmdBufDepthGrabber.Clear();
			m_Camera.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, m_CmdBufDepthGrabber);
			m_Camera.RemoveCommandBuffer(CameraEvent.AfterFinalPass, m_CmdBufDepthGrabber);
			m_CmdBufDepthGrabber = null;
		}
		if (m_DepthRT != null)
		{
			m_DepthRT.Release();
			m_DepthRT = null;
		}
		DepthRT = IntPtr.Zero;
		m_IsDepthCopyEnabled = false;
	}

	protected bool SetupDistortionPass()
	{
		if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_DistortionRT = new RenderTexture(m_Camera.pixelWidth, m_Camera.pixelHeight, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.sRGB);
		}
		else if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf))
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
			m_DistortionRT = new RenderTexture(m_Camera.pixelWidth, m_Camera.pixelHeight, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.sRGB);
		}
		else
		{
			Debug.LogError("[PKFX] This device does not support ARGBFloat nor ARGBHalf render texture formats...");
			Debug.LogError("[PKFX] Distortion disabled.");
			m_EnableDistortion = false;
			m_EnableBlur = false;
		}
		if (m_DistortionRT != null)
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
			if (!m_DistortionRT.IsCreated())
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
				m_DistortionRT.Create();
			}
		}
		if (m_DistortionMat == null)
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
			m_DistortionMat = new Material(Shader.Find("Hidden/PKFx Distortion"));
			if (m_DistortionMat == null)
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
				Debug.LogError("[PKFX] Failed to load FxDistortionEffect shader...");
				Debug.LogError("[PKFX] Distortion disabled.");
				m_EnableDistortion = false;
				m_EnableBlur = false;
			}
		}
		if (m_DistBlurMat == null)
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
			m_DistBlurMat = new Material(Shader.Find("Hidden/PKFx Blur Shader for Distortion Pass"));
			if (m_DistBlurMat == null)
			{
				Debug.LogError("[PKFX] Failed to load blur shader...");
				Debug.LogError("[PKFX] Distortion blur disabled.");
				m_EnableBlur = false;
			}
		}
		if (m_Camera.actualRenderingPath != RenderingPath.Forward)
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
			m_CmdBufDisto = new CommandBuffer();
			m_CmdBufDisto.name = "PopcornFX Distortion Post-Effect";
			m_Camera.AddCommandBuffer(CameraEvent.BeforeImageEffects, m_CmdBufDisto);
		}
		return m_EnableDistortion;
	}

	private void ReleaseDistortionResources()
	{
		if (m_CmdBufDisto != null)
		{
			m_Camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffects, m_CmdBufDisto);
			m_CmdBufDisto = null;
		}
		if (m_DistortionRT != null)
		{
			m_DistortionRT.Release();
			m_DistortionRT = null;
		}
		m_EnableDistortion = false;
	}

	private void SetupRendering()
	{
		if (m_CmdBufDepthGrabber != null)
		{
			m_CmdBufDepthGrabber.Clear();
		}
		if (!m_EnableDistortion)
		{
			if (!m_EnableSoftParticles)
			{
				goto IL_0179;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
		}
		if (m_CmdBufDepthGrabber == null)
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
			if (!SetupDepthGrab())
			{
				goto IL_0179;
			}
		}
		if (m_DepthRT != null && !m_DepthRT.IsCreated())
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
			m_DepthRT.Release();
			m_DepthRT = new RenderTexture(m_Camera.pixelWidth, m_Camera.pixelHeight, (int)m_DepthGrabFormat, g_DepthFormat);
			if (!m_DepthRT.IsCreated())
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
				m_DepthRT.Create();
			}
			DepthRT = m_DepthRT.GetNativeTexturePtr();
		}
		else
		{
			if (m_Camera.actualRenderingPath == RenderingPath.Forward)
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
				if (!m_FlipRendering)
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
					if (SystemInfo.graphicsDeviceVersion.Contains("Direct3D"))
					{
						m_DepthGrabMat.SetFloat("_Flip", 1f);
					}
				}
			}
			m_CmdBufDepthGrabber.Blit(m_DepthRT, m_DepthRT, m_DepthGrabMat);
		}
		goto IL_01ce;
		IL_0302:
		m_CmdBuf.Clear();
		m_CmdBuf.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
		m_CmdBuf.IssuePluginEvent(PKFxManager.GetRenderEventFunc(), m_CurrentCameraID | 0x5AFE0000);
		if (m_EnableDistortion)
		{
			m_CmdBuf.SetRenderTarget(m_DistortionRT);
			m_CmdBuf.ClearRenderTarget(false, true, Color.black);
			m_CmdBuf.IssuePluginEvent(PKFxManager.GetRenderEventFunc(), m_CurrentCameraID | 0x5AFE0000 | 0x2000);
		}
		m_PrevScreenWidth = m_Camera.pixelWidth;
		m_PrevScreenHeight = m_Camera.pixelHeight;
		return;
		IL_0179:
		if (m_CmdBufDepthGrabber != null)
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
			m_CmdBufDepthGrabber.Clear();
			m_Camera.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, m_CmdBufDepthGrabber);
			m_Camera.RemoveCommandBuffer(CameraEvent.AfterFinalPass, m_CmdBufDepthGrabber);
			m_CmdBufDepthGrabber = null;
			DepthRT = IntPtr.Zero;
		}
		goto IL_01ce;
		IL_023e:
		int nameID = Shader.PropertyToID("PKFxDistTmp");
		RenderTargetIdentifier renderTargetIdentifier = new RenderTargetIdentifier(nameID);
		m_CmdBufDisto.GetTemporaryRT(nameID, -1, -1, -1, FilterMode.Trilinear, RenderTextureFormat.DefaultHDR, RenderTextureReadWrite.Linear);
		m_DistortionMat.SetTexture("_DistortionTex", m_DistortionRT);
		m_CmdBufDisto.Blit(BuiltinRenderTextureType.CameraTarget, renderTargetIdentifier, m_DistortionMat);
		if (m_EnableBlur)
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
			m_DistBlurMat.SetTexture("_DistortionTex", m_DistortionRT);
			m_DistBlurMat.SetFloat("_BlurFactor", m_BlurFactor);
			m_CmdBufDisto.Blit(renderTargetIdentifier, BuiltinRenderTextureType.CameraTarget, m_DistBlurMat);
		}
		else
		{
			m_CmdBufDisto.Blit(renderTargetIdentifier, BuiltinRenderTextureType.CameraTarget);
		}
		goto IL_0302;
		IL_01ce:
		if (m_CmdBufDisto != null)
		{
			m_CmdBufDisto.Clear();
		}
		if (m_Camera.actualRenderingPath != RenderingPath.Forward)
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
			if (m_EnableDistortion)
			{
				if (m_CmdBufDisto != null)
				{
					if (m_DistortionRT != null)
					{
						goto IL_023e;
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
				}
				if (SetupDistortionPass())
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
					goto IL_023e;
				}
			}
		}
		goto IL_0302;
	}

	private void UpdateFrame()
	{
		m_CameraDescription.DT = Time.smoothDeltaTime;
		m_TimeMultiplier = Mathf.Max(m_TimeMultiplier, 0f);
		m_TimeMultiplier = Mathf.Min(m_TimeMultiplier, 8f);
		m_CameraDescription.DT *= m_TimeMultiplier;
		m_CameraDescription.NearClip = m_Camera.nearClipPlane;
		m_CameraDescription.FarClip = m_Camera.farClipPlane;
		m_CameraDescription.LODBias = m_TextureLODBias;
		m_CameraDescription.DepthBpp = (int)m_DepthGrabFormat;
		m_CameraDescription.Flags = 0;
		m_CameraDescription.Flags |= (m_UseDepthGrabToZTest ? 1 : 0);
		if (m_Camera.pixelWidth == m_PrevScreenWidth)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_Camera.pixelHeight == m_PrevScreenHeight)
			{
				goto IL_02c4;
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
		m_CameraDescription.Flags |= 2;
		if (m_CmdBufDepthGrabber != null)
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
			if (m_Camera.actualRenderingPath != RenderingPath.DeferredLighting)
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
				if (m_Camera.actualRenderingPath != RenderingPath.DeferredShading)
				{
					m_Camera.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, m_CmdBufDepthGrabber);
					goto IL_0199;
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
			m_Camera.RemoveCommandBuffer(CameraEvent.AfterFinalPass, m_CmdBufDepthGrabber);
			goto IL_0199;
		}
		goto IL_01ab;
		IL_0199:
		m_CmdBufDepthGrabber.Clear();
		m_CmdBufDepthGrabber = null;
		goto IL_01ab;
		IL_01ab:
		if (m_DepthRT != null)
		{
			DepthRT = IntPtr.Zero;
			m_DepthRT.Release();
			m_DepthRT = null;
		}
		if (m_DistortionRT != null)
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
			m_DistortionRT.Release();
			m_DistortionRT = null;
		}
		if (m_CmdBufDisto != null)
		{
			m_Camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffects, m_CmdBufDisto);
			m_CmdBufDisto.Clear();
			m_CmdBufDisto = null;
		}
		else if (m_EnableDistortion)
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
			if (m_DistortionRT == null)
			{
				SetupDistortionPass();
				if (m_Camera.actualRenderingPath == RenderingPath.Forward)
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
					PKFxDistortionEffect component = GetComponent<PKFxDistortionEffect>();
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
						component._DistortionRT = m_DistortionRT;
					}
				}
			}
		}
		m_PrevScreenWidth = m_Camera.pixelWidth;
		m_PrevScreenHeight = m_Camera.pixelHeight;
		goto IL_02c4;
		IL_02c4:
		PKFxManager.LogicalUpdate(m_CameraDescription.DT);
		if (!m_Camera.stereoEnabled)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					UpdateViewMatrix();
					UpdateProjectionMatrix();
					m_CurrentCameraID = m_CameraID;
					PKFxManager.UpdateCamDesc(m_CurrentCameraID, m_CameraDescription, false);
					return;
				}
			}
		}
		UpdateViewMatrix(true);
		UpdateProjectionMatrix(true);
		m_CurrentCameraID = m_CameraID;
		PKFxManager.UpdateCamDesc(m_CurrentCameraID, m_CameraDescription, false);
		UpdateViewMatrix(true, Camera.StereoscopicEye.Right);
		UpdateProjectionMatrix(true, Camera.StereoscopicEye.Right);
		m_CurrentCameraID = m_VRReservedID;
		PKFxManager.UpdateCamDesc(m_CurrentCameraID, m_CameraDescription, false);
	}

	private void UpdateViewMatrix(bool isVR = false, Camera.StereoscopicEye eye = Camera.StereoscopicEye.Left)
	{
		if (!isVR)
		{
			m_CameraDescription.ViewMatrix = m_Camera.worldToCameraMatrix;
		}
		else
		{
			m_CameraDescription.ViewMatrix = m_Camera.GetStereoViewMatrix(eye);
		}
	}

	private void UpdateProjectionMatrix(bool isVR = false, Camera.StereoscopicEye eye = Camera.StereoscopicEye.Left)
	{
		int num;
		if (!m_FlipRendering)
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
			if (m_Camera.actualRenderingPath != RenderingPath.DeferredLighting)
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
				num = ((m_Camera.actualRenderingPath == RenderingPath.DeferredShading) ? 1 : 0);
				goto IL_0046;
			}
		}
		num = 1;
		goto IL_0046;
		IL_0046:
		bool renderIntoTexture = (byte)num != 0;
		Matrix4x4 proj = isVR ? m_Camera.GetStereoProjectionMatrix(eye) : m_Camera.projectionMatrix;
		m_CameraDescription.ProjectionMatrix = GL.GetGPUProjectionMatrix(proj, renderIntoTexture);
	}

	private void OnPreCull()
	{
		UpdateFrame();
	}

	private void OnPreRender()
	{
		if (!PKFxManager.m_PackLoaded)
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
		if (m_CurrentFrameID != m_LastUpdateFrameID)
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
			m_CurrentCameraID = m_CameraID;
			SetupRendering();
			m_LastUpdateFrameID = m_CurrentFrameID;
		}
		else
		{
			m_CurrentCameraID = m_VRReservedID;
			SetupRendering();
		}
		if (m_LastFrameCount != Time.frameCount)
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
			PKFxManager.UpdateParticles(m_CameraDescription);
			m_LastFrameCount = Time.frameCount;
		}
		PKFxManager.Render(m_CurrentCameraID);
	}

	private void Update()
	{
		m_CurrentFrameID++;
	}

	protected void OnDestroy()
	{
		ReleaseDepthGrabResources();
		ReleaseDistortionResources();
	}
}
