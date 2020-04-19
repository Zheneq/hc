using System;
using UnityEngine;

[AddComponentMenu("Image Effects/Player Selection")]
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
internal class PlayerSelectionEffect : PostEffectsCSBase, IGameEventListener
{
	private Camera playerCamera;

	public GameObject playerCameraPrefab;

	public bool doNotDestroyCamOnDisable;

	private GameObject playerCameraObject;

	public Color friendlyInner = new Color(0.3203125f, 1f, 0.171875f, 0f);

	public Color friendlyOuter = new Color(0.3203125f, 0.7f, 0.171875f, 0f);

	public Color enemyInner = new Color(1f, 0.31f, 0.35f, 0f);

	public Color enemyOuter = new Color(0.7f, 0.1f, 0.171875f, 0f);

	[Range(0.2f, 3.99f)]
	public float OutlineFalloff = 1.25f;

	[Range(0.01f, 0.99f)]
	public float OutlinePosition = 0.222f;

	[Range(0f, 10f)]
	public float AntiHalo = 5.6f;

	[Range(0f, 2f)]
	public int downsample = 1;

	[Range(0f, 10f)]
	public float blurSize = 0.8f;

	[Range(1f, 4f)]
	public int blurIterations = 2;

	public Shader IDShader;

	public Shader outlineShader;

	public bool m_drawSelection;

	public bool m_flipIfNotLowQuality;

	private Material m_selectionPostMaterial;

	private bool m_drawingInConfirm;

	private float m_drawingInConfirmStartTime;

	private float m_shrinkSpeed = 1f;

	private void Awake()
	{
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GraphicsQualityChanged);
	}

	private void OnDestroy()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GraphicsQualityChanged);
	}

	public void SetDrawingInConfirm(bool draw)
	{
		if (!this.m_drawingInConfirm)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerSelectionEffect.SetDrawingInConfirm(bool)).MethodHandle;
			}
			if (draw)
			{
				this.m_drawingInConfirmStartTime = Time.time;
				this.m_shrinkSpeed = Mathf.Max(0f, HUD_UIResources.Get().m_confirmedTargetingShrinkSpeed);
			}
		}
		this.m_drawingInConfirm = draw;
	}

	public override bool CheckResources()
	{
		if (GameManager.IsEditorAndNotGame())
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerSelectionEffect.CheckResources()).MethodHandle;
			}
			return false;
		}
		base.CheckSupport(false);
		this.m_selectionPostMaterial = base.CheckShaderAndCreateMaterial(this.outlineShader, this.m_selectionPostMaterial);
		this.InitializeSelectionPostMaterialParameters();
		if (!this.isSupported)
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
			base.ReportAutoDisable();
		}
		return this.isSupported;
	}

	private void SetCameraFlag()
	{
		base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
	}

	private void OnEnable()
	{
		this.SetCameraFlag();
	}

	private void OnDisable()
	{
		if (this.m_selectionPostMaterial)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerSelectionEffect.OnDisable()).MethodHandle;
			}
			UnityEngine.Object.DestroyImmediate(this.m_selectionPostMaterial);
		}
		this.playerCamera = null;
		if (!this.doNotDestroyCamOnDisable)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.playerCameraObject)
			{
				UnityEngine.Object.DestroyImmediate(this.playerCameraObject);
				this.playerCameraObject = null;
			}
		}
	}

	private Camera GetPlayerCam()
	{
		if (!this.playerCamera)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerSelectionEffect.GetPlayerCam()).MethodHandle;
			}
			if (this.playerCameraPrefab != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!this.playerCamera)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					this.playerCameraObject = UnityEngine.Object.Instantiate<GameObject>(this.playerCameraPrefab);
					this.playerCamera = this.playerCameraObject.GetComponent<Camera>();
				}
			}
		}
		return this.playerCamera;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		this.InitializeSelectionPostMaterialParameters();
	}

	private void OnValidate()
	{
		this.InitializeSelectionPostMaterialParameters();
	}

	private void InitializeSelectionPostMaterialParameters()
	{
		if (Options_UI.Get() != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerSelectionEffect.InitializeSelectionPostMaterialParameters()).MethodHandle;
			}
			if (this.m_selectionPostMaterial != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (Camera.main.actualRenderingPath == RenderingPath.Forward)
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
					if (SystemInfo.graphicsDeviceVersion.Contains("Direct3D"))
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
						GraphicsQuality currentGraphicsQuality = Options_UI.Get().GetCurrentGraphicsQuality();
						bool flag = currentGraphicsQuality <= GraphicsQuality.Low;
						float num;
						if (!flag)
						{
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							if (this.m_flipIfNotLowQuality)
							{
								num = 1f;
								goto IL_BA;
							}
						}
						num = 0f;
						IL_BA:
						float value = num;
						this.m_selectionPostMaterial.SetFloat("_Flip", value);
						string name = "_LowQuality";
						float value2;
						if (flag)
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
							value2 = 1f;
						}
						else
						{
							value2 = 0f;
						}
						Shader.SetGlobalFloat(name, value2);
					}
				}
			}
		}
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.m_drawSelection)
		{
			if (this.CheckResources())
			{
				Camera playerCam = this.GetPlayerCam();
				float num = this.blurSize;
				if (this.m_drawingInConfirm)
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
					num = Mathf.Max(0f, this.blurSize * (1f - this.m_shrinkSpeed * (Time.time - this.m_drawingInConfirmStartTime)));
				}
				if (num != 0f)
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
					if (playerCam)
					{
						RenderTexture renderTexture = RenderTexture.GetTemporary(source.width, source.height, 0x18, source.format);
						renderTexture.filterMode = FilterMode.Bilinear;
						playerCam.CopyFrom(base.GetComponent<Camera>());
						playerCam.targetTexture = renderTexture;
						playerCam.cullingMask = 1 << LayerMask.NameToLayer("ActorSelected");
						playerCam.depthTextureMode = DepthTextureMode.Depth;
						playerCam.backgroundColor = new Color(0f, 0f, 0f, 0f);
						playerCam.clearFlags = CameraClearFlags.Color;
						playerCam.RenderWithShader(this.IDShader, string.Empty);
						playerCam.targetTexture = null;
						float num2 = 1f / (1f * (float)(1 << this.downsample));
						this.m_selectionPostMaterial.SetVector("_Parameter", new Vector4(num * num2, -num * num2, 0f, 0f));
						source.filterMode = FilterMode.Bilinear;
						int depthBuffer = 0x18;
						int width = source.width >> this.downsample;
						int height = source.height >> this.downsample;
						RenderTexture temporary = RenderTexture.GetTemporary(width, height, depthBuffer, source.format);
						temporary.filterMode = FilterMode.Bilinear;
						Graphics.Blit(renderTexture, temporary, this.m_selectionPostMaterial, 0);
						RenderTexture.ReleaseTemporary(renderTexture);
						renderTexture = temporary;
						for (int i = 0; i < this.blurIterations; i++)
						{
							float num3 = (float)i * 1f;
							this.m_selectionPostMaterial.SetVector("_Parameter", new Vector4(num * num2 + num3, -num * num2 - num3, 0f, 0f));
							RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, depthBuffer, source.format);
							temporary2.filterMode = FilterMode.Bilinear;
							Graphics.Blit(renderTexture, temporary2, this.m_selectionPostMaterial, 1);
							RenderTexture.ReleaseTemporary(renderTexture);
							renderTexture = temporary2;
							temporary2 = RenderTexture.GetTemporary(width, height, depthBuffer, source.format);
							temporary2.filterMode = FilterMode.Bilinear;
							Graphics.Blit(renderTexture, temporary2, this.m_selectionPostMaterial, 2);
							RenderTexture.ReleaseTemporary(renderTexture);
							renderTexture = temporary2;
						}
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_selectionPostMaterial.SetColor("_FriendlyInner", this.friendlyInner);
						this.m_selectionPostMaterial.SetColor("_FriendlyOuter", this.friendlyOuter);
						this.m_selectionPostMaterial.SetColor("_EnemyInner", this.enemyInner);
						this.m_selectionPostMaterial.SetColor("_EnemyOuter", this.enemyOuter);
						this.m_selectionPostMaterial.SetVector("_OutlineParameter", new Vector4(this.OutlineFalloff, this.OutlinePosition, this.AntiHalo + 1f, 0f));
						RenderTexture temporary3 = RenderTexture.GetTemporary(width, height, depthBuffer, source.format);
						Graphics.Blit(renderTexture, temporary3, this.m_selectionPostMaterial, 3);
						RenderTexture.ReleaseTemporary(renderTexture);
						this.m_selectionPostMaterial.SetTexture("_OutlineTex", temporary3);
						Graphics.Blit(source, destination, this.m_selectionPostMaterial, 4);
						this.m_selectionPostMaterial.SetTexture("_OutlineTex", null);
						RenderTexture.ReleaseTemporary(temporary3);
						this.m_drawSelection = false;
						return;
					}
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
				Graphics.Blit(source, destination);
				return;
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerSelectionEffect.OnRenderImage(RenderTexture, RenderTexture)).MethodHandle;
			}
		}
		Graphics.Blit(source, destination);
	}

	private enum PlayerSelectPass
	{
		Downsample,
		VBlur,
		HBlur,
		DrawOutlines,
		Composite
	}
}
