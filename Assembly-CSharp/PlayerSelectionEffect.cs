using UnityEngine;

[AddComponentMenu("Image Effects/Player Selection")]
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
internal class PlayerSelectionEffect : PostEffectsCSBase, IGameEventListener
{
	private enum PlayerSelectPass
	{
		Downsample,
		VBlur,
		HBlur,
		DrawOutlines,
		Composite
	}

	private Camera playerCamera;

	public GameObject playerCameraPrefab;

	public bool doNotDestroyCamOnDisable;

	private GameObject playerCameraObject;

	public Color friendlyInner = new Color(41f / 128f, 1f, 11f / 64f, 0f);

	public Color friendlyOuter = new Color(41f / 128f, 0.7f, 11f / 64f, 0f);

	public Color enemyInner = new Color(1f, 0.31f, 0.35f, 0f);

	public Color enemyOuter = new Color(0.7f, 0.1f, 11f / 64f, 0f);

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
		if (!m_drawingInConfirm)
		{
			if (draw)
			{
				m_drawingInConfirmStartTime = Time.time;
				m_shrinkSpeed = Mathf.Max(0f, HUD_UIResources.Get().m_confirmedTargetingShrinkSpeed);
			}
		}
		m_drawingInConfirm = draw;
	}

	public override bool CheckResources()
	{
		if (GameManager.IsEditorAndNotGame())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		CheckSupport(false);
		m_selectionPostMaterial = CheckShaderAndCreateMaterial(outlineShader, m_selectionPostMaterial);
		InitializeSelectionPostMaterialParameters();
		if (!isSupported)
		{
			ReportAutoDisable();
		}
		return isSupported;
	}

	private void SetCameraFlag()
	{
		GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
	}

	private void OnEnable()
	{
		SetCameraFlag();
	}

	private void OnDisable()
	{
		if ((bool)m_selectionPostMaterial)
		{
			Object.DestroyImmediate(m_selectionPostMaterial);
		}
		playerCamera = null;
		if (doNotDestroyCamOnDisable)
		{
			return;
		}
		while (true)
		{
			if ((bool)playerCameraObject)
			{
				Object.DestroyImmediate(playerCameraObject);
				playerCameraObject = null;
			}
			return;
		}
	}

	private Camera GetPlayerCam()
	{
		if (!playerCamera)
		{
			if (playerCameraPrefab != null)
			{
				if (!playerCamera)
				{
					playerCameraObject = Object.Instantiate(playerCameraPrefab);
					playerCamera = playerCameraObject.GetComponent<Camera>();
				}
			}
		}
		return playerCamera;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		InitializeSelectionPostMaterialParameters();
	}

	private void OnValidate()
	{
		InitializeSelectionPostMaterialParameters();
	}

	private void InitializeSelectionPostMaterialParameters()
	{
		if (!(Options_UI.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (!(m_selectionPostMaterial != null))
			{
				return;
			}
			while (true)
			{
				if (Camera.main.actualRenderingPath != RenderingPath.Forward)
				{
					return;
				}
				while (true)
				{
					if (!SystemInfo.graphicsDeviceVersion.Contains("Direct3D"))
					{
						return;
					}
					while (true)
					{
						GraphicsQuality currentGraphicsQuality = Options_UI.Get().GetCurrentGraphicsQuality();
						bool flag = currentGraphicsQuality <= GraphicsQuality.Low;
						float num;
						if (!flag)
						{
							if (m_flipIfNotLowQuality)
							{
								num = 1f;
								goto IL_00ba;
							}
						}
						num = 0f;
						goto IL_00ba;
						IL_00ba:
						float value = num;
						m_selectionPostMaterial.SetFloat("_Flip", value);
						float value2;
						if (flag)
						{
							value2 = 1f;
						}
						else
						{
							value2 = 0f;
						}
						Shader.SetGlobalFloat("_LowQuality", value2);
						return;
					}
				}
			}
		}
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (m_drawSelection)
		{
			if (CheckResources())
			{
				Camera playerCam = GetPlayerCam();
				float num = blurSize;
				if (m_drawingInConfirm)
				{
					num = Mathf.Max(0f, blurSize * (1f - m_shrinkSpeed * (Time.time - m_drawingInConfirmStartTime)));
				}
				if (num != 0f)
				{
					if ((bool)playerCam)
					{
						RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 24, source.format);
						temporary.filterMode = FilterMode.Bilinear;
						playerCam.CopyFrom(GetComponent<Camera>());
						playerCam.targetTexture = temporary;
						playerCam.cullingMask = 1 << LayerMask.NameToLayer("ActorSelected");
						playerCam.depthTextureMode = DepthTextureMode.Depth;
						playerCam.backgroundColor = new Color(0f, 0f, 0f, 0f);
						playerCam.clearFlags = CameraClearFlags.Color;
						playerCam.RenderWithShader(IDShader, string.Empty);
						playerCam.targetTexture = null;
						float num2 = 1f / (1f * (float)(1 << downsample));
						m_selectionPostMaterial.SetVector("_Parameter", new Vector4(num * num2, (0f - num) * num2, 0f, 0f));
						source.filterMode = FilterMode.Bilinear;
						int depthBuffer = 24;
						int width = source.width >> downsample;
						int height = source.height >> downsample;
						RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, depthBuffer, source.format);
						temporary2.filterMode = FilterMode.Bilinear;
						Graphics.Blit(temporary, temporary2, m_selectionPostMaterial, 0);
						RenderTexture.ReleaseTemporary(temporary);
						temporary = temporary2;
						for (int i = 0; i < blurIterations; i++)
						{
							float num3 = (float)i * 1f;
							m_selectionPostMaterial.SetVector("_Parameter", new Vector4(num * num2 + num3, (0f - num) * num2 - num3, 0f, 0f));
							RenderTexture temporary3 = RenderTexture.GetTemporary(width, height, depthBuffer, source.format);
							temporary3.filterMode = FilterMode.Bilinear;
							Graphics.Blit(temporary, temporary3, m_selectionPostMaterial, 1);
							RenderTexture.ReleaseTemporary(temporary);
							temporary = temporary3;
							temporary3 = RenderTexture.GetTemporary(width, height, depthBuffer, source.format);
							temporary3.filterMode = FilterMode.Bilinear;
							Graphics.Blit(temporary, temporary3, m_selectionPostMaterial, 2);
							RenderTexture.ReleaseTemporary(temporary);
							temporary = temporary3;
						}
						while (true)
						{
							m_selectionPostMaterial.SetColor("_FriendlyInner", friendlyInner);
							m_selectionPostMaterial.SetColor("_FriendlyOuter", friendlyOuter);
							m_selectionPostMaterial.SetColor("_EnemyInner", enemyInner);
							m_selectionPostMaterial.SetColor("_EnemyOuter", enemyOuter);
							m_selectionPostMaterial.SetVector("_OutlineParameter", new Vector4(OutlineFalloff, OutlinePosition, AntiHalo + 1f, 0f));
							RenderTexture temporary4 = RenderTexture.GetTemporary(width, height, depthBuffer, source.format);
							Graphics.Blit(temporary, temporary4, m_selectionPostMaterial, 3);
							RenderTexture.ReleaseTemporary(temporary);
							m_selectionPostMaterial.SetTexture("_OutlineTex", temporary4);
							Graphics.Blit(source, destination, m_selectionPostMaterial, 4);
							m_selectionPostMaterial.SetTexture("_OutlineTex", null);
							RenderTexture.ReleaseTemporary(temporary4);
							m_drawSelection = false;
							return;
						}
					}
				}
				Graphics.Blit(source, destination);
				return;
			}
		}
		Graphics.Blit(source, destination);
	}
}
