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

	public static short GetUniqueID()
	{
		short num = PKFxCamera.g_CameraUID;
		PKFxCamera.g_CameraUID = (short)(num + 1);
		return num;
	}

	public int RenderPass
	{
		get
		{
			return this.m_CameraDescription.RenderPass;
		}
		set
		{
			this.m_CameraDescription.RenderPass = value;
		}
	}

	public IntPtr DepthRT
	{
		get
		{
			return this.m_CameraDescription.DepthRT;
		}
		set
		{
			this.m_CameraDescription.DepthRT = value;
		}
	}

	private void Awake()
	{
		this.m_CameraID = PKFxCamera.GetUniqueID();
		this.m_CurrentCameraID = this.m_CameraID;
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			if (VRSettings.enabled && VRDevice.isPresent)
			{
				this.m_VRReservedID = PKFxCamera.GetUniqueID();
			}
		}
		this.m_Camera = base.GetComponent<Camera>();
		this.m_CmdBuf = new CommandBuffer();
		this.m_CmdBuf.name = "PopcornFX Rendering";
		this.m_Camera.AddCommandBuffer((CameraEvent)PKFxManager.m_GlobalConf.globalEventSetting, this.m_CmdBuf);
	}

	private static bool ResolveDepthShaderAndTextureFormat()
	{
		if (PKFxCamera.g_isDepthResolved)
		{
			return false;
		}
		PKFxCamera.g_DepthShaderName = "Hidden/PKFx Depth Copy";
		if (Shader.Find(PKFxCamera.g_DepthShaderName).isSupported)
		{
			if (SystemInfo.SupportsRenderTextureFormat(PKFxCamera.g_DepthFormat))
			{
				goto IL_138;
			}
		}
		Debug.LogWarning(string.Concat(new object[]
		{
			"[PKFX] ",
			PKFxCamera.g_DepthShaderName,
			" shader or ",
			PKFxCamera.g_DepthFormat,
			" texture format not supported."
		}));
		PKFxCamera.g_DepthShaderName = "Hidden/PKFx Depth Copy to Color";
		PKFxCamera.g_DepthFormat = RenderTextureFormat.RFloat;
		if (!SystemInfo.SupportsRenderTextureFormat(PKFxCamera.g_DepthFormat))
		{
			Debug.LogWarning("[PKFX] " + PKFxCamera.g_DepthFormat + " fallback texture format not supported.");
			PKFxCamera.g_DepthFormat = RenderTextureFormat.RHalf;
			Debug.LogWarning("[PKFX] Resorting to " + PKFxCamera.g_DepthFormat + " (may produce artefacts).");
		}
		Debug.LogWarning(string.Concat(new object[]
		{
			"[PKFX] Falling back to ",
			PKFxCamera.g_DepthShaderName,
			" shader / ",
			PKFxCamera.g_DepthFormat,
			" texture format."
		}));
		IL_138:
		PKFxCamera.g_isDepthResolved = true;
		return true;
	}

	protected bool SetupDepthGrab()
	{
		bool flag = PKFxCamera.ResolveDepthShaderAndTextureFormat();
		if (this.m_DepthGrabMat == null)
		{
			if (!SystemInfo.SupportsRenderTextureFormat(PKFxCamera.g_DepthFormat))
			{
				if (flag)
				{
					Debug.LogError("[PKFX] " + PKFxCamera.g_DepthFormat + " texture format not supported.");
					Debug.LogError("[PKFX] Soft particles/distortion disabled.");
				}
				this.m_IsDepthCopyEnabled = false;
				return false;
			}
			this.m_DepthGrabMat = new Material(Shader.Find(PKFxCamera.g_DepthShaderName));
			if (this.m_DepthGrabMat == null)
			{
				if (flag)
				{
					Debug.LogError("[PKFX] Depth copy shader not found.");
					Debug.LogError("[PKFX] Soft particles/distortion disabled.");
				}
				this.m_IsDepthCopyEnabled = false;
				return false;
			}
		}
		if (this.m_DepthRT == null)
		{
			this.m_DepthRT = new RenderTexture(this.m_Camera.pixelWidth, this.m_Camera.pixelHeight, (int)this.m_DepthGrabFormat, PKFxCamera.g_DepthFormat);
		}
		if (!this.m_DepthRT.IsCreated())
		{
			this.m_DepthRT.Create();
		}
		if (this.m_CmdBufDepthGrabber == null)
		{
			this.m_CmdBufDepthGrabber = new CommandBuffer();
			this.m_CmdBufDepthGrabber.name = "PopcornFX Depth Grabber";
		}
		if (this.m_Camera.actualRenderingPath != RenderingPath.DeferredLighting)
		{
			if (this.m_Camera.actualRenderingPath != RenderingPath.DeferredShading)
			{
				this.m_Camera.AddCommandBuffer(CameraEvent.AfterForwardOpaque, this.m_CmdBufDepthGrabber);
				goto IL_1C1;
			}
		}
		this.m_Camera.AddCommandBuffer(CameraEvent.AfterFinalPass, this.m_CmdBufDepthGrabber);
		IL_1C1:
		this.DepthRT = this.m_DepthRT.GetNativeTexturePtr();
		this.m_Camera.depthTextureMode |= DepthTextureMode.Depth;
		this.m_IsDepthCopyEnabled = true;
		return true;
	}

	private void ReleaseDepthGrabResources()
	{
		if (this.m_CmdBufDepthGrabber != null)
		{
			this.m_CmdBufDepthGrabber.Clear();
			this.m_Camera.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, this.m_CmdBufDepthGrabber);
			this.m_Camera.RemoveCommandBuffer(CameraEvent.AfterFinalPass, this.m_CmdBufDepthGrabber);
			this.m_CmdBufDepthGrabber = null;
		}
		if (this.m_DepthRT != null)
		{
			this.m_DepthRT.Release();
			this.m_DepthRT = null;
		}
		this.DepthRT = IntPtr.Zero;
		this.m_IsDepthCopyEnabled = false;
	}

	protected bool SetupDistortionPass()
	{
		if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat))
		{
			this.m_DistortionRT = new RenderTexture(this.m_Camera.pixelWidth, this.m_Camera.pixelHeight, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.sRGB);
		}
		else if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf))
		{
			this.m_DistortionRT = new RenderTexture(this.m_Camera.pixelWidth, this.m_Camera.pixelHeight, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.sRGB);
		}
		else
		{
			Debug.LogError("[PKFX] This device does not support ARGBFloat nor ARGBHalf render texture formats...");
			Debug.LogError("[PKFX] Distortion disabled.");
			this.m_EnableDistortion = false;
			this.m_EnableBlur = false;
		}
		if (this.m_DistortionRT != null)
		{
			if (!this.m_DistortionRT.IsCreated())
			{
				this.m_DistortionRT.Create();
			}
		}
		if (this.m_DistortionMat == null)
		{
			this.m_DistortionMat = new Material(Shader.Find("Hidden/PKFx Distortion"));
			if (this.m_DistortionMat == null)
			{
				Debug.LogError("[PKFX] Failed to load FxDistortionEffect shader...");
				Debug.LogError("[PKFX] Distortion disabled.");
				this.m_EnableDistortion = false;
				this.m_EnableBlur = false;
			}
		}
		if (this.m_DistBlurMat == null)
		{
			this.m_DistBlurMat = new Material(Shader.Find("Hidden/PKFx Blur Shader for Distortion Pass"));
			if (this.m_DistBlurMat == null)
			{
				Debug.LogError("[PKFX] Failed to load blur shader...");
				Debug.LogError("[PKFX] Distortion blur disabled.");
				this.m_EnableBlur = false;
			}
		}
		if (this.m_Camera.actualRenderingPath != RenderingPath.Forward)
		{
			this.m_CmdBufDisto = new CommandBuffer();
			this.m_CmdBufDisto.name = "PopcornFX Distortion Post-Effect";
			this.m_Camera.AddCommandBuffer(CameraEvent.BeforeImageEffects, this.m_CmdBufDisto);
		}
		return this.m_EnableDistortion;
	}

	private void ReleaseDistortionResources()
	{
		if (this.m_CmdBufDisto != null)
		{
			this.m_Camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffects, this.m_CmdBufDisto);
			this.m_CmdBufDisto = null;
		}
		if (this.m_DistortionRT != null)
		{
			this.m_DistortionRT.Release();
			this.m_DistortionRT = null;
		}
		this.m_EnableDistortion = false;
	}

	private void SetupRendering()
	{
		if (this.m_CmdBufDepthGrabber != null)
		{
			this.m_CmdBufDepthGrabber.Clear();
		}
		if (!this.m_EnableDistortion)
		{
			if (!this.m_EnableSoftParticles)
			{
				goto IL_179;
			}
		}
		if (this.m_CmdBufDepthGrabber == null)
		{
			if (!this.SetupDepthGrab())
			{
				goto IL_179;
			}
		}
		if (this.m_DepthRT != null && !this.m_DepthRT.IsCreated())
		{
			this.m_DepthRT.Release();
			this.m_DepthRT = new RenderTexture(this.m_Camera.pixelWidth, this.m_Camera.pixelHeight, (int)this.m_DepthGrabFormat, PKFxCamera.g_DepthFormat);
			if (!this.m_DepthRT.IsCreated())
			{
				this.m_DepthRT.Create();
			}
			this.DepthRT = this.m_DepthRT.GetNativeTexturePtr();
		}
		else
		{
			if (this.m_Camera.actualRenderingPath == RenderingPath.Forward)
			{
				if (!this.m_FlipRendering)
				{
					if (SystemInfo.graphicsDeviceVersion.Contains("Direct3D"))
					{
						this.m_DepthGrabMat.SetFloat("_Flip", 1f);
					}
				}
			}
			this.m_CmdBufDepthGrabber.Blit(this.m_DepthRT, this.m_DepthRT, this.m_DepthGrabMat);
		}
		goto IL_1CE;
		IL_179:
		if (this.m_CmdBufDepthGrabber != null)
		{
			this.m_CmdBufDepthGrabber.Clear();
			this.m_Camera.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, this.m_CmdBufDepthGrabber);
			this.m_Camera.RemoveCommandBuffer(CameraEvent.AfterFinalPass, this.m_CmdBufDepthGrabber);
			this.m_CmdBufDepthGrabber = null;
			this.DepthRT = IntPtr.Zero;
		}
		IL_1CE:
		if (this.m_CmdBufDisto != null)
		{
			this.m_CmdBufDisto.Clear();
		}
		if (this.m_Camera.actualRenderingPath != RenderingPath.Forward)
		{
			if (this.m_EnableDistortion)
			{
				if (this.m_CmdBufDisto != null)
				{
					if (this.m_DistortionRT != null)
					{
						goto IL_23E;
					}
				}
				if (!this.SetupDistortionPass())
				{
					goto IL_302;
				}
				IL_23E:
				int nameID = Shader.PropertyToID("PKFxDistTmp");
				RenderTargetIdentifier renderTargetIdentifier = new RenderTargetIdentifier(nameID);
				this.m_CmdBufDisto.GetTemporaryRT(nameID, -1, -1, -1, FilterMode.Trilinear, RenderTextureFormat.DefaultHDR, RenderTextureReadWrite.Linear);
				this.m_DistortionMat.SetTexture("_DistortionTex", this.m_DistortionRT);
				this.m_CmdBufDisto.Blit(BuiltinRenderTextureType.CameraTarget, renderTargetIdentifier, this.m_DistortionMat);
				if (this.m_EnableBlur)
				{
					this.m_DistBlurMat.SetTexture("_DistortionTex", this.m_DistortionRT);
					this.m_DistBlurMat.SetFloat("_BlurFactor", this.m_BlurFactor);
					this.m_CmdBufDisto.Blit(renderTargetIdentifier, BuiltinRenderTextureType.CameraTarget, this.m_DistBlurMat);
				}
				else
				{
					this.m_CmdBufDisto.Blit(renderTargetIdentifier, BuiltinRenderTextureType.CameraTarget);
				}
			}
		}
		IL_302:
		this.m_CmdBuf.Clear();
		this.m_CmdBuf.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
		this.m_CmdBuf.IssuePluginEvent(PKFxManager.GetRenderEventFunc(), (int)((uint)this.m_CurrentCameraID | 0x5AFE0000U));
		if (this.m_EnableDistortion)
		{
			this.m_CmdBuf.SetRenderTarget(this.m_DistortionRT);
			this.m_CmdBuf.ClearRenderTarget(false, true, Color.black);
			this.m_CmdBuf.IssuePluginEvent(PKFxManager.GetRenderEventFunc(), (int)((uint)this.m_CurrentCameraID | 0x5AFE0000U | 0x2000U));
		}
		this.m_PrevScreenWidth = this.m_Camera.pixelWidth;
		this.m_PrevScreenHeight = this.m_Camera.pixelHeight;
	}

	private void UpdateFrame()
	{
		this.m_CameraDescription.DT = Time.smoothDeltaTime;
		this.m_TimeMultiplier = Mathf.Max(this.m_TimeMultiplier, 0f);
		this.m_TimeMultiplier = Mathf.Min(this.m_TimeMultiplier, 8f);
		this.m_CameraDescription.DT = this.m_CameraDescription.DT * this.m_TimeMultiplier;
		this.m_CameraDescription.NearClip = this.m_Camera.nearClipPlane;
		this.m_CameraDescription.FarClip = this.m_Camera.farClipPlane;
		this.m_CameraDescription.LODBias = this.m_TextureLODBias;
		this.m_CameraDescription.DepthBpp = (int)this.m_DepthGrabFormat;
		this.m_CameraDescription.Flags = 0;
		this.m_CameraDescription.Flags = (this.m_CameraDescription.Flags | ((!this.m_UseDepthGrabToZTest) ? 0 : 1));
		if (this.m_Camera.pixelWidth == this.m_PrevScreenWidth)
		{
			if (this.m_Camera.pixelHeight == this.m_PrevScreenHeight)
			{
				goto IL_2C4;
			}
		}
		this.m_CameraDescription.Flags = (this.m_CameraDescription.Flags | 2);
		if (this.m_CmdBufDepthGrabber != null)
		{
			if (this.m_Camera.actualRenderingPath != RenderingPath.DeferredLighting)
			{
				if (this.m_Camera.actualRenderingPath != RenderingPath.DeferredShading)
				{
					this.m_Camera.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, this.m_CmdBufDepthGrabber);
					goto IL_199;
				}
			}
			this.m_Camera.RemoveCommandBuffer(CameraEvent.AfterFinalPass, this.m_CmdBufDepthGrabber);
			IL_199:
			this.m_CmdBufDepthGrabber.Clear();
			this.m_CmdBufDepthGrabber = null;
		}
		if (this.m_DepthRT != null)
		{
			this.DepthRT = IntPtr.Zero;
			this.m_DepthRT.Release();
			this.m_DepthRT = null;
		}
		if (this.m_DistortionRT != null)
		{
			this.m_DistortionRT.Release();
			this.m_DistortionRT = null;
		}
		if (this.m_CmdBufDisto != null)
		{
			this.m_Camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffects, this.m_CmdBufDisto);
			this.m_CmdBufDisto.Clear();
			this.m_CmdBufDisto = null;
		}
		else if (this.m_EnableDistortion)
		{
			if (this.m_DistortionRT == null)
			{
				this.SetupDistortionPass();
				if (this.m_Camera.actualRenderingPath == RenderingPath.Forward)
				{
					PKFxDistortionEffect component = base.GetComponent<PKFxDistortionEffect>();
					if (component != null)
					{
						component._DistortionRT = this.m_DistortionRT;
					}
				}
			}
		}
		this.m_PrevScreenWidth = this.m_Camera.pixelWidth;
		this.m_PrevScreenHeight = this.m_Camera.pixelHeight;
		IL_2C4:
		PKFxManager.LogicalUpdate(this.m_CameraDescription.DT);
		if (!this.m_Camera.stereoEnabled)
		{
			this.UpdateViewMatrix(false, Camera.StereoscopicEye.Left);
			this.UpdateProjectionMatrix(false, Camera.StereoscopicEye.Left);
			this.m_CurrentCameraID = this.m_CameraID;
			PKFxManager.UpdateCamDesc((int)this.m_CurrentCameraID, this.m_CameraDescription, false);
		}
		else
		{
			this.UpdateViewMatrix(true, Camera.StereoscopicEye.Left);
			this.UpdateProjectionMatrix(true, Camera.StereoscopicEye.Left);
			this.m_CurrentCameraID = this.m_CameraID;
			PKFxManager.UpdateCamDesc((int)this.m_CurrentCameraID, this.m_CameraDescription, false);
			this.UpdateViewMatrix(true, Camera.StereoscopicEye.Right);
			this.UpdateProjectionMatrix(true, Camera.StereoscopicEye.Right);
			this.m_CurrentCameraID = this.m_VRReservedID;
			PKFxManager.UpdateCamDesc((int)this.m_CurrentCameraID, this.m_CameraDescription, false);
		}
	}

	private void UpdateViewMatrix(bool isVR = false, Camera.StereoscopicEye eye = Camera.StereoscopicEye.Left)
	{
		if (!isVR)
		{
			this.m_CameraDescription.ViewMatrix = this.m_Camera.worldToCameraMatrix;
		}
		else
		{
			this.m_CameraDescription.ViewMatrix = this.m_Camera.GetStereoViewMatrix(eye);
		}
	}

	private void UpdateProjectionMatrix(bool isVR = false, Camera.StereoscopicEye eye = Camera.StereoscopicEye.Left)
	{
		bool flag;
		if (!this.m_FlipRendering)
		{
			if (this.m_Camera.actualRenderingPath != RenderingPath.DeferredLighting)
			{
				flag = (this.m_Camera.actualRenderingPath == RenderingPath.DeferredShading);
				goto IL_46;
			}
		}
		flag = true;
		IL_46:
		bool renderIntoTexture = flag;
		Matrix4x4 proj;
		if (!isVR)
		{
			proj = this.m_Camera.projectionMatrix;
		}
		else
		{
			proj = this.m_Camera.GetStereoProjectionMatrix(eye);
		}
		this.m_CameraDescription.ProjectionMatrix = GL.GetGPUProjectionMatrix(proj, renderIntoTexture);
	}

	private void OnPreCull()
	{
		this.UpdateFrame();
	}

	private void OnPreRender()
	{
		if (!PKFxManager.m_PackLoaded)
		{
			return;
		}
		if (this.m_CurrentFrameID != this.m_LastUpdateFrameID)
		{
			this.m_CurrentCameraID = this.m_CameraID;
			this.SetupRendering();
			this.m_LastUpdateFrameID = this.m_CurrentFrameID;
		}
		else
		{
			this.m_CurrentCameraID = this.m_VRReservedID;
			this.SetupRendering();
		}
		if (PKFxCamera.m_LastFrameCount != Time.frameCount)
		{
			PKFxManager.UpdateParticles(this.m_CameraDescription);
			PKFxCamera.m_LastFrameCount = Time.frameCount;
		}
		PKFxManager.Render(this.m_CurrentCameraID);
	}

	private void Update()
	{
		this.m_CurrentFrameID += 1U;
	}

	protected void OnDestroy()
	{
		this.ReleaseDepthGrabResources();
		this.ReleaseDistortionResources();
	}
}
