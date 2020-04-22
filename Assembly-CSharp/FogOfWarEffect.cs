using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

[AddComponentMenu("Image Effects/Fog Of War")]
public class FogOfWarEffect : ImageEffectBase, IGameEventListener
{
	public Shader blurShader;

	public FogOfWarCameraSettings defaultFogOfWarCameraSettingsPrefab;

	private float minHeight = 1f;

	private FogOfWarCameraSettings fogOfWarCameraSettings;

	private Camera fogOfWarCam;

	private Camera postCam;

	private RenderTexture m_renderTexture;

	private float CAMERA_NEAR = 0.5f;

	private float CAMERA_FAR = 50f;

	private float CAMERA_FOV = 60f;

	private float CAMERA_ASPECT_RATIO = 1.333333f;

	private RenderingPath camRenderPath = RenderingPath.Forward;

	private int camRenderPass;

	private bool minHeightSet;

	private int m_fogOfWarMatrixID;

	private int m_fogOfWarTexID;

	private int m_fogOfWarColorID;

	private int m_frustumCornersWSID;

	private int m_originID;

	private int m_minHeightID;

	private int m_prepassTexID;

	private bool m_fogOfWarCamNeedsRender = true;

	private int m_fogOfWarCamForceRenderFrames;

	private int m_fogOfWarCamRenderedScreenHeight;

	private int m_fogOfWarCamRenderedScreenWidth;

	private bool m_fogOfWarCamRenderedScreenFullscreen;

	private bool m_usingDefaultFogOfWarCamSettings;

	private bool m_started;

	private RenderTexture GetRenderTexture()
	{
		if (!m_renderTexture)
		{
			m_renderTexture = new RenderTexture(512, 512, 0);
			m_renderTexture.name = "FogOfWarRenderTexture";
			m_renderTexture.hideFlags = HideFlags.HideAndDontSave;
			m_renderTexture.isPowerOfTwo = true;
		}
		return m_renderTexture;
	}

	private void CopySourceToBG(RenderTexture source)
	{
		Camera camera = GetPostCam();
		if (!camera)
		{
			return;
		}
		FogOfWarBackgroundEffect component = camera.GetComponent<FogOfWarBackgroundEffect>();
		if (!component)
		{
			return;
		}
		while (true)
		{
			Graphics.Blit(source, component.GetSourceTexture());
			return;
		}
	}

	private RenderTexture GetPostTexture()
	{
		if ((bool)GetPostCam())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return GetPostCam().GetComponent<FogOfWarBackgroundEffect>().GetRenderTexture();
				}
			}
		}
		return null;
	}

	private Camera GetPostCam()
	{
		if (!Application.isPlaying)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		if ((bool)postCam)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return postCam;
				}
			}
		}
		CheckFogCamPrefab();
		if ((bool)fogOfWarCameraSettings)
		{
			postCam = fogOfWarCameraSettings.m_postCamera;
			postCam.enabled = false;
		}
		return postCam;
	}

	private void CheckFogCamPrefab()
	{
		if ((bool)fogOfWarCameraSettings)
		{
			return;
		}
		while (true)
		{
			if (FogOfWarCameraSettings.Get() != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						fogOfWarCameraSettings = FogOfWarCameraSettings.Get();
						m_usingDefaultFogOfWarCamSettings = false;
						return;
					}
				}
			}
			if (defaultFogOfWarCameraSettingsPrefab != null)
			{
				while (true)
				{
					fogOfWarCameraSettings = UnityEngine.Object.Instantiate(defaultFogOfWarCameraSettingsPrefab);
					m_usingDefaultFogOfWarCamSettings = true;
					return;
				}
			}
			return;
		}
	}

	private Camera GetFogOfWarCam()
	{
		if (!Application.isPlaying)
		{
			return null;
		}
		if ((bool)fogOfWarCam)
		{
			return fogOfWarCam;
		}
		CheckFogCamPrefab();
		if ((bool)fogOfWarCameraSettings)
		{
			fogOfWarCam = fogOfWarCameraSettings.m_camera;
			fogOfWarCam.enabled = false;
		}
		return fogOfWarCam;
	}

	private void CheckMinHeight()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		while (true)
		{
			if (minHeightSet)
			{
				return;
			}
			while (true)
			{
				GameObject gameObject = GameObject.Find("LOSHighlights");
				if (!gameObject)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							Log.Error("Design scene file {0} is missing LOSHighlights, Fog of War will not render!", SceneManager.GetActiveScene().name);
							return;
						}
					}
				}
				float num = float.MaxValue;
				IEnumerator enumerator = gameObject.transform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Transform transform = (Transform)enumerator.Current;
						MeshFilter component;
						if ((bool)(component = transform.GetComponent<MeshFilter>()))
						{
							Mesh mesh = component.mesh;
							Vector3[] vertices = mesh.vertices;
							for (int i = 0; i < vertices.Length; i++)
							{
								if (vertices[i].y < num)
								{
									num = vertices[i].y;
								}
							}
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								disposable.Dispose();
								goto end_IL_0116;
							}
						}
					}
					end_IL_0116:;
				}
				if (num < float.MaxValue)
				{
					minHeight = num;
				}
				else
				{
					Log.Error("Design scene file {0} has invalid LOSHighlights, Fog of War may not render!", SceneManager.GetActiveScene().name);
				}
				minHeight -= 85f;
				minHeightSet = true;
				return;
			}
		}
	}

	protected override void Start()
	{
		base.Start();
		m_fogOfWarMatrixID = Shader.PropertyToID("_FogOfWarMatrix");
		m_fogOfWarTexID = Shader.PropertyToID("_FogOfWarTex");
		m_fogOfWarColorID = Shader.PropertyToID("_FogOfWarColor");
		m_frustumCornersWSID = Shader.PropertyToID("_FrustumCornersWS");
		m_originID = Shader.PropertyToID("_Origin");
		m_minHeightID = Shader.PropertyToID("_MinHeight");
		m_prepassTexID = Shader.PropertyToID("_PrepassTex");
		m_fogOfWarMatrixID = Shader.PropertyToID("_FogOfWarMatrix");
		Initialize();
		if (!GameManager.IsEditorAndNotGame())
		{
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.BoardSquareVisibleShadeChanged);
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.GraphicsQualityChanged);
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.SystemEscapeMenuOnReturnToGameClick);
		}
		m_started = true;
	}

	private void OnDestroy()
	{
		if (GameEventManager.Get() == null)
		{
			return;
		}
		while (true)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.BoardSquareVisibleShadeChanged);
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GraphicsQualityChanged);
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.SystemEscapeMenuOnReturnToGameClick);
			return;
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		m_fogOfWarCamNeedsRender = true;
		if (eventType != GameEventManager.EventType.GraphicsQualityChanged)
		{
			return;
		}
		while (true)
		{
			m_fogOfWarCamForceRenderFrames = 2;
			return;
		}
	}

	private void OnPreRender()
	{
		CheckMinHeight();
		if (m_fogOfWarCamForceRenderFrames > 0)
		{
			m_fogOfWarCamNeedsRender = true;
			m_fogOfWarCamForceRenderFrames--;
		}
		if (!m_fogOfWarCamNeedsRender || !GetFogOfWarCam())
		{
			return;
		}
		while (true)
		{
			GetFogOfWarCam().Render();
			m_fogOfWarCamNeedsRender = false;
			m_fogOfWarCamRenderedScreenWidth = Screen.width;
			m_fogOfWarCamRenderedScreenHeight = Screen.height;
			m_fogOfWarCamRenderedScreenFullscreen = Screen.fullScreen;
			return;
		}
	}

	private void OnEnable()
	{
		if (HydrogenConfig.Get().HeadlessMode)
		{
			base.enabled = false;
		}
		else
		{
			if (!m_started)
			{
				return;
			}
			while (true)
			{
				Initialize();
				return;
			}
		}
	}

	private void Initialize()
	{
		camRenderPath = RenderingPath.Forward;
		camRenderPass = 0;
		base.material.SetPass(0);
		Camera.main.depthTextureMode |= DepthTextureMode.Depth;
		if ((bool)GetFogOfWarCam())
		{
			GetFogOfWarCam().depth = Camera.main.depth - 1f;
			GetFogOfWarCam().targetTexture = GetRenderTexture();
		}
		if (!GetPostCam())
		{
			return;
		}
		while (true)
		{
			GetPostCam().depth = Camera.main.depth - 2f;
			return;
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		if (m_usingDefaultFogOfWarCamSettings)
		{
			if ((bool)fogOfWarCam)
			{
				fogOfWarCam.targetTexture = null;
				fogOfWarCam = null;
			}
			if ((bool)fogOfWarCameraSettings)
			{
				UnityEngine.Object.DestroyObject(fogOfWarCameraSettings.gameObject);
				fogOfWarCameraSettings = null;
			}
		}
		if ((bool)m_renderTexture)
		{
			UnityEngine.Object.DestroyImmediate(m_renderTexture);
			m_renderTexture = null;
		}
		minHeightSet = false;
	}

	private void UpdateRenderPath()
	{
		if (camRenderPath == Camera.main.actualRenderingPath)
		{
			return;
		}
		while (true)
		{
			camRenderPath = Camera.main.actualRenderingPath;
			if (camRenderPath == RenderingPath.Forward)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						camRenderPass = 0;
						return;
					}
				}
			}
			camRenderPass = 1;
			return;
		}
	}

	private void Update()
	{
		Camera camera = GetFogOfWarCam();
		if (!(camera != null))
		{
			return;
		}
		while (true)
		{
			Transform transform = base.transform;
			Vector3 vector = transform.position;
			Ray ray = new Ray(transform.position, transform.forward);
			float num;
			if (!GameManager.IsEditorAndNotGame())
			{
				if (!(Board.Get() == null))
				{
					num = Board.Get().BaselineHeight;
					goto IL_0093;
				}
			}
			num = 0f;
			goto IL_0093;
			IL_0093:
			float y = num;
			Plane plane = new Plane(Vector3.up, new Vector3(0f, y, 0f));
			float num2 = fogOfWarCam.orthographicSize / 2f;
			if (plane.Raycast(ray, out float enter))
			{
				Vector3 point = ray.GetPoint(enter);
				Vector3 vector2 = point - transform.position;
				vector2.y = 0f;
				if (vector2.sqrMagnitude < num2 * num2)
				{
					vector = point;
				}
			}
			Vector3 position = camera.transform.position;
			vector.y = position.y;
			if ((vector - camera.transform.position).sqrMagnitude > num2 * num2)
			{
				camera.transform.position = vector;
				m_fogOfWarCamNeedsRender = true;
			}
			if (m_fogOfWarCamRenderedScreenWidth == Screen.width)
			{
				if (m_fogOfWarCamRenderedScreenHeight == Screen.height)
				{
					if (m_fogOfWarCamRenderedScreenFullscreen == Screen.fullScreen)
					{
						return;
					}
				}
			}
			m_fogOfWarCamNeedsRender = true;
			return;
		}
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!GetFogOfWarCam())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (!GetPostCam())
		{
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		UpdateRenderPath();
		CopySourceToBG(source);
		GetPostCam().Render();
		CAMERA_NEAR = Camera.main.nearClipPlane;
		CAMERA_FAR = Camera.main.farClipPlane;
		CAMERA_FOV = Camera.main.fieldOfView;
		CAMERA_ASPECT_RATIO = Camera.main.aspect;
		Matrix4x4 identity = Matrix4x4.identity;
		float num = CAMERA_FOV * 0.5f;
		Vector3 b = Camera.main.transform.right * CAMERA_NEAR * Mathf.Tan(num * ((float)Math.PI / 180f)) * CAMERA_ASPECT_RATIO;
		Vector3 b2 = Camera.main.transform.up * CAMERA_NEAR * Mathf.Tan(num * ((float)Math.PI / 180f));
		Vector3 v = Camera.main.transform.forward * CAMERA_NEAR - b + b2;
		float num2 = v.magnitude * CAMERA_FAR / CAMERA_NEAR;
		v.Normalize();
		v *= num2;
		Vector3 v2 = Camera.main.transform.forward * CAMERA_NEAR + b + b2;
		v2.Normalize();
		v2 *= num2;
		Vector3 v3 = Camera.main.transform.forward * CAMERA_NEAR + b - b2;
		v3.Normalize();
		v3 *= num2;
		Vector3 v4 = Camera.main.transform.forward * CAMERA_NEAR - b - b2;
		v4.Normalize();
		v4 *= num2;
		identity.SetRow(0, v);
		identity.SetRow(1, v2);
		identity.SetRow(2, v3);
		identity.SetRow(3, v4);
		Matrix4x4 worldToLocalMatrix = GetFogOfWarCam().transform.worldToLocalMatrix;
		Vector3 position = Camera.main.transform.position;
		Vector4 value = default(Vector4);
		value.x = position.x;
		value.y = position.y;
		value.z = position.z;
		value.w = 0f;
		Shader.SetGlobalMatrix(m_fogOfWarMatrixID, worldToLocalMatrix);
		Shader.SetGlobalTexture(m_fogOfWarTexID, GetRenderTexture());
		Shader.SetGlobalColor(m_fogOfWarColorID, fogOfWarCameraSettings.fogColor);
		Material material = base.material;
		material.SetMatrix(m_frustumCornersWSID, identity);
		material.SetVector(m_originID, value);
		material.SetFloat(m_minHeightID, minHeight + fogOfWarCameraSettings.minHeightOffset);
		material.SetTexture(m_prepassTexID, GetPostTexture());
		Graphics.Blit(source, destination, material, camRenderPass);
	}
}
