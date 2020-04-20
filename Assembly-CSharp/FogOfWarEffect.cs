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
		if (!this.m_renderTexture)
		{
			this.m_renderTexture = new RenderTexture(0x200, 0x200, 0);
			this.m_renderTexture.name = "FogOfWarRenderTexture";
			this.m_renderTexture.hideFlags = HideFlags.HideAndDontSave;
			this.m_renderTexture.isPowerOfTwo = true;
		}
		return this.m_renderTexture;
	}

	private void CopySourceToBG(RenderTexture source)
	{
		Camera camera = this.GetPostCam();
		if (camera)
		{
			FogOfWarBackgroundEffect component = camera.GetComponent<FogOfWarBackgroundEffect>();
			if (component)
			{
				Graphics.Blit(source, component.GetSourceTexture());
			}
		}
	}

	private RenderTexture GetPostTexture()
	{
		if (this.GetPostCam())
		{
			return this.GetPostCam().GetComponent<FogOfWarBackgroundEffect>().GetRenderTexture();
		}
		return null;
	}

	private Camera GetPostCam()
	{
		if (!Application.isPlaying)
		{
			return null;
		}
		if (this.postCam)
		{
			return this.postCam;
		}
		this.CheckFogCamPrefab();
		if (this.fogOfWarCameraSettings)
		{
			this.postCam = this.fogOfWarCameraSettings.m_postCamera;
			this.postCam.enabled = false;
		}
		return this.postCam;
	}

	private void CheckFogCamPrefab()
	{
		if (!this.fogOfWarCameraSettings)
		{
			if (FogOfWarCameraSettings.Get() != null)
			{
				this.fogOfWarCameraSettings = FogOfWarCameraSettings.Get();
				this.m_usingDefaultFogOfWarCamSettings = false;
			}
			else if (this.defaultFogOfWarCameraSettingsPrefab != null)
			{
				this.fogOfWarCameraSettings = UnityEngine.Object.Instantiate<FogOfWarCameraSettings>(this.defaultFogOfWarCameraSettingsPrefab);
				this.m_usingDefaultFogOfWarCamSettings = true;
			}
		}
	}

	private Camera GetFogOfWarCam()
	{
		if (!Application.isPlaying)
		{
			return null;
		}
		if (this.fogOfWarCam)
		{
			return this.fogOfWarCam;
		}
		this.CheckFogCamPrefab();
		if (this.fogOfWarCameraSettings)
		{
			this.fogOfWarCam = this.fogOfWarCameraSettings.m_camera;
			this.fogOfWarCam.enabled = false;
		}
		return this.fogOfWarCam;
	}

	private void CheckMinHeight()
	{
		if (Application.isPlaying)
		{
			if (!this.minHeightSet)
			{
				GameObject gameObject = GameObject.Find("LOSHighlights");
				if (!gameObject)
				{
					Log.Error("Design scene file {0} is missing LOSHighlights, Fog of War will not render!", new object[]
					{
						SceneManager.GetActiveScene().name
					});
					return;
				}
				float num = float.MaxValue;
				IEnumerator enumerator = gameObject.transform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Transform transform = (Transform)obj;
						MeshFilter component;
						if (component = transform.GetComponent<MeshFilter>())
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
						disposable.Dispose();
					}
				}
				if (num < 3.40282347E+38f)
				{
					this.minHeight = num;
				}
				else
				{
					Log.Error("Design scene file {0} has invalid LOSHighlights, Fog of War may not render!", new object[]
					{
						SceneManager.GetActiveScene().name
					});
				}
				this.minHeight -= 85f;
				this.minHeightSet = true;
			}
		}
	}

	protected override void Start()
	{
		base.Start();
		this.m_fogOfWarMatrixID = Shader.PropertyToID("_FogOfWarMatrix");
		this.m_fogOfWarTexID = Shader.PropertyToID("_FogOfWarTex");
		this.m_fogOfWarColorID = Shader.PropertyToID("_FogOfWarColor");
		this.m_frustumCornersWSID = Shader.PropertyToID("_FrustumCornersWS");
		this.m_originID = Shader.PropertyToID("_Origin");
		this.m_minHeightID = Shader.PropertyToID("_MinHeight");
		this.m_prepassTexID = Shader.PropertyToID("_PrepassTex");
		this.m_fogOfWarMatrixID = Shader.PropertyToID("_FogOfWarMatrix");
		this.Initialize();
		if (!GameManager.IsEditorAndNotGame())
		{
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.BoardSquareVisibleShadeChanged);
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.GraphicsQualityChanged);
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.SystemEscapeMenuOnReturnToGameClick);
		}
		this.m_started = true;
	}

	private void OnDestroy()
	{
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.BoardSquareVisibleShadeChanged);
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GraphicsQualityChanged);
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.SystemEscapeMenuOnReturnToGameClick);
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		this.m_fogOfWarCamNeedsRender = true;
		if (eventType == GameEventManager.EventType.GraphicsQualityChanged)
		{
			this.m_fogOfWarCamForceRenderFrames = 2;
		}
	}

	private void OnPreRender()
	{
		this.CheckMinHeight();
		if (this.m_fogOfWarCamForceRenderFrames > 0)
		{
			this.m_fogOfWarCamNeedsRender = true;
			this.m_fogOfWarCamForceRenderFrames--;
		}
		if (this.m_fogOfWarCamNeedsRender && this.GetFogOfWarCam())
		{
			this.GetFogOfWarCam().Render();
			this.m_fogOfWarCamNeedsRender = false;
			this.m_fogOfWarCamRenderedScreenWidth = Screen.width;
			this.m_fogOfWarCamRenderedScreenHeight = Screen.height;
			this.m_fogOfWarCamRenderedScreenFullscreen = Screen.fullScreen;
		}
	}

	private void OnEnable()
	{
		if (HydrogenConfig.Get().HeadlessMode)
		{
			base.enabled = false;
		}
		else if (this.m_started)
		{
			this.Initialize();
		}
	}

	private void Initialize()
	{
		this.camRenderPath = RenderingPath.Forward;
		this.camRenderPass = 0;
		base.material.SetPass(0);
		Camera.main.depthTextureMode |= DepthTextureMode.Depth;
		if (this.GetFogOfWarCam())
		{
			this.GetFogOfWarCam().depth = Camera.main.depth - 1f;
			this.GetFogOfWarCam().targetTexture = this.GetRenderTexture();
		}
		if (this.GetPostCam())
		{
			this.GetPostCam().depth = Camera.main.depth - 2f;
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_usingDefaultFogOfWarCamSettings)
		{
			if (this.fogOfWarCam)
			{
				this.fogOfWarCam.targetTexture = null;
				this.fogOfWarCam = null;
			}
			if (this.fogOfWarCameraSettings)
			{
				UnityEngine.Object.DestroyObject(this.fogOfWarCameraSettings.gameObject);
				this.fogOfWarCameraSettings = null;
			}
		}
		if (this.m_renderTexture)
		{
			UnityEngine.Object.DestroyImmediate(this.m_renderTexture);
			this.m_renderTexture = null;
		}
		this.minHeightSet = false;
	}

	private void UpdateRenderPath()
	{
		if (this.camRenderPath != Camera.main.actualRenderingPath)
		{
			this.camRenderPath = Camera.main.actualRenderingPath;
			if (this.camRenderPath == RenderingPath.Forward)
			{
				this.camRenderPass = 0;
			}
			else
			{
				this.camRenderPass = 1;
			}
		}
	}

	private void Update()
	{
		Camera camera = this.GetFogOfWarCam();
		if (camera != null)
		{
			Transform transform = base.transform;
			Vector3 vector = transform.position;
			Ray ray = new Ray(transform.position, transform.forward);
			float num;
			if (!GameManager.IsEditorAndNotGame())
			{
				if (!(Board.Get() == null))
				{
					num = (float)Board.Get().BaselineHeight;
					goto IL_93;
				}
			}
			num = 0f;
			IL_93:
			float y = num;
			Plane plane = new Plane(Vector3.up, new Vector3(0f, y, 0f));
			float num2 = this.fogOfWarCam.orthographicSize / 2f;
			float distance;
			if (plane.Raycast(ray, out distance))
			{
				Vector3 point = ray.GetPoint(distance);
				Vector3 vector2 = point - transform.position;
				vector2.y = 0f;
				if (vector2.sqrMagnitude < num2 * num2)
				{
					vector = point;
				}
			}
			vector.y = camera.transform.position.y;
			if ((vector - camera.transform.position).sqrMagnitude > num2 * num2)
			{
				camera.transform.position = vector;
				this.m_fogOfWarCamNeedsRender = true;
			}
			if (this.m_fogOfWarCamRenderedScreenWidth == Screen.width)
			{
				if (this.m_fogOfWarCamRenderedScreenHeight == Screen.height)
				{
					if (this.m_fogOfWarCamRenderedScreenFullscreen == Screen.fullScreen)
					{
						return;
					}
				}
			}
			this.m_fogOfWarCamNeedsRender = true;
		}
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.GetFogOfWarCam())
		{
			return;
		}
		if (!this.GetPostCam())
		{
			return;
		}
		this.UpdateRenderPath();
		this.CopySourceToBG(source);
		this.GetPostCam().Render();
		this.CAMERA_NEAR = Camera.main.nearClipPlane;
		this.CAMERA_FAR = Camera.main.farClipPlane;
		this.CAMERA_FOV = Camera.main.fieldOfView;
		this.CAMERA_ASPECT_RATIO = Camera.main.aspect;
		Matrix4x4 identity = Matrix4x4.identity;
		float num = this.CAMERA_FOV * 0.5f;
		Vector3 b = Camera.main.transform.right * this.CAMERA_NEAR * Mathf.Tan(num * 0.0174532924f) * this.CAMERA_ASPECT_RATIO;
		Vector3 b2 = Camera.main.transform.up * this.CAMERA_NEAR * Mathf.Tan(num * 0.0174532924f);
		Vector3 vector = Camera.main.transform.forward * this.CAMERA_NEAR - b + b2;
		float d = vector.magnitude * this.CAMERA_FAR / this.CAMERA_NEAR;
		vector.Normalize();
		vector *= d;
		Vector3 vector2 = Camera.main.transform.forward * this.CAMERA_NEAR + b + b2;
		vector2.Normalize();
		vector2 *= d;
		Vector3 vector3 = Camera.main.transform.forward * this.CAMERA_NEAR + b - b2;
		vector3.Normalize();
		vector3 *= d;
		Vector3 vector4 = Camera.main.transform.forward * this.CAMERA_NEAR - b - b2;
		vector4.Normalize();
		vector4 *= d;
		identity.SetRow(0, vector);
		identity.SetRow(1, vector2);
		identity.SetRow(2, vector3);
		identity.SetRow(3, vector4);
		Matrix4x4 worldToLocalMatrix = this.GetFogOfWarCam().transform.worldToLocalMatrix;
		Vector3 position = Camera.main.transform.position;
		Vector4 value;
		value.x = position.x;
		value.y = position.y;
		value.z = position.z;
		value.w = 0f;
		Shader.SetGlobalMatrix(this.m_fogOfWarMatrixID, worldToLocalMatrix);
		Shader.SetGlobalTexture(this.m_fogOfWarTexID, this.GetRenderTexture());
		Shader.SetGlobalColor(this.m_fogOfWarColorID, this.fogOfWarCameraSettings.fogColor);
		Material material = base.material;
		material.SetMatrix(this.m_frustumCornersWSID, identity);
		material.SetVector(this.m_originID, value);
		material.SetFloat(this.m_minHeightID, this.minHeight + this.fogOfWarCameraSettings.minHeightOffset);
		material.SetTexture(this.m_prepassTexID, this.GetPostTexture());
		Graphics.Blit(source, destination, material, this.camRenderPass);
	}
}
