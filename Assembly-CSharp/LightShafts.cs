using System;
using UnityEngine;

[RequireComponent(typeof(Light))]
[ExecuteInEditMode]
public class LightShafts : MonoBehaviour
{
	public LightShaftsShadowmapMode m_ShadowmapMode;

	private LightShaftsShadowmapMode m_ShadowmapModeOld;

	public Camera[] m_Cameras;

	public Camera m_CurrentCamera;

	private bool m_ShadowmapDirty = true;

	public Vector3 m_Size = new Vector3(10f, 10f, 20f);

	public float m_SpotNear = 0.1f;

	public float m_SpotFar = 1f;

	public LayerMask m_CullingMask = -1;

	public LayerMask m_ColorFilterMask = 0;

	public float m_Brightness = 5f;

	public float m_BrightnessColored = 5f;

	public float m_Extinction = 0.5f;

	public float m_MinDistFromCamera;

	public int m_ShadowmapRes = 1024;

	private Camera m_ShadowmapCamera;

	private RenderTexture m_Shadowmap;

	public Shader m_DepthShader;

	private RenderTexture m_ColorFilter;

	public Shader m_ColorFilterShader;

	public bool m_Colored;

	public float m_ColorBalance = 1f;

	public int m_EpipolarLines = 256;

	public int m_EpipolarSamples = 512;

	private RenderTexture m_CoordEpi;

	private RenderTexture m_DepthEpi;

	public Shader m_CoordShader;

	private Material m_CoordMaterial;

	private Camera m_CoordsCamera;

	private RenderTexture m_InterpolationEpi;

	public Shader m_DepthBreaksShader;

	private Material m_DepthBreaksMaterial;

	private RenderTexture m_RaymarchedLightEpi;

	private Material m_RaymarchMaterial;

	public Shader m_RaymarchShader;

	private RenderTexture m_InterpolateAlongRaysEpi;

	public Shader m_InterpolateAlongRaysShader;

	private Material m_InterpolateAlongRaysMaterial;

	private RenderTexture m_SamplePositions;

	public Shader m_SamplePositionsShader;

	private Material m_SamplePositionsMaterial;

	private bool m_SamplePositionsShaderCompiles;

	public Shader m_FinalInterpolationShader;

	private Material m_FinalInterpolationMaterial;

	public float m_DepthThreshold = 0.5f;

	public int m_InterpolationStep = 32;

	public bool m_ShowSamples;

	public bool m_ShowInterpolatedSamples;

	public float m_ShowSamplesBackgroundFade = 0.8f;

	public bool m_AttenuationCurveOn;

	public AnimationCurve m_AttenuationCurve;

	private Texture2D m_AttenuationCurveTex;

	private Light m_Light;

	private LightType m_LightType = LightType.Directional;

	private bool m_DX11Support;

	private bool m_MinRequirements;

	private Mesh m_SpotMesh;

	private float m_SpotMeshNear = -1f;

	private float m_SpotMeshFar = -1f;

	private float m_SpotMeshAngle = -1f;

	private float m_SpotMeshRange = -1f;

	public bool directional => m_LightType == LightType.Directional;

	public bool spot => m_LightType == LightType.Spot;

	private void InitLUTs()
	{
		if ((bool)m_AttenuationCurveTex)
		{
			while (true)
			{
				return;
			}
		}
		m_AttenuationCurveTex = new Texture2D(256, 1, TextureFormat.ARGB32, false, true);
		m_AttenuationCurveTex.wrapMode = TextureWrapMode.Clamp;
		m_AttenuationCurveTex.hideFlags = HideFlags.HideAndDontSave;
		if (m_AttenuationCurve != null)
		{
			if (m_AttenuationCurve.length != 0)
			{
				goto IL_00c2;
			}
		}
		m_AttenuationCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));
		goto IL_00c2;
		IL_00c2:
		if (!m_AttenuationCurveTex)
		{
			return;
		}
		while (true)
		{
			UpdateLUTs();
			return;
		}
	}

	public void UpdateLUTs()
	{
		InitLUTs();
		if (m_AttenuationCurve == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		for (int i = 0; i < 256; i++)
		{
			float num = Mathf.Clamp(m_AttenuationCurve.Evaluate((float)i / 255f), 0f, 1f);
			m_AttenuationCurveTex.SetPixel(i, 0, new Color(num, num, num, num));
		}
		while (true)
		{
			m_AttenuationCurveTex.Apply();
			return;
		}
	}

	private void InitRenderTexture(ref RenderTexture rt, int width, int height, int depth, RenderTextureFormat format, bool temp = true)
	{
		if (temp)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					rt = RenderTexture.GetTemporary(width, height, depth, format);
					return;
				}
			}
		}
		if (rt != null)
		{
			if (rt.width == width)
			{
				if (rt.height == height)
				{
					if (rt.depth == depth)
					{
						if (rt.format == format)
						{
							return;
						}
					}
				}
			}
			rt.Release();
			UnityEngine.Object.DestroyImmediate(rt);
		}
		rt = new RenderTexture(width, height, depth, format);
		rt.hideFlags = HideFlags.HideAndDontSave;
	}

	private void InitShadowmap()
	{
		bool flag = m_ShadowmapMode == LightShaftsShadowmapMode.Dynamic;
		if (flag && m_ShadowmapMode != m_ShadowmapModeOld)
		{
			if ((bool)m_Shadowmap)
			{
				m_Shadowmap.Release();
			}
			if ((bool)m_ColorFilter)
			{
				m_ColorFilter.Release();
			}
		}
		InitRenderTexture(ref m_Shadowmap, m_ShadowmapRes, m_ShadowmapRes, 24, RenderTextureFormat.RFloat, flag);
		m_Shadowmap.filterMode = FilterMode.Point;
		m_Shadowmap.wrapMode = TextureWrapMode.Clamp;
		if (m_Colored)
		{
			InitRenderTexture(ref m_ColorFilter, m_ShadowmapRes, m_ShadowmapRes, 0, RenderTextureFormat.ARGB32, flag);
		}
		m_ShadowmapModeOld = m_ShadowmapMode;
	}

	private void ReleaseShadowmap()
	{
		if (m_ShadowmapMode == LightShaftsShadowmapMode.Static)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		RenderTexture.ReleaseTemporary(m_Shadowmap);
		RenderTexture.ReleaseTemporary(m_ColorFilter);
	}

	private void InitEpipolarTextures()
	{
		m_EpipolarLines = ((m_EpipolarLines >= 8) ? m_EpipolarLines : 8);
		m_EpipolarSamples = ((m_EpipolarSamples >= 4) ? m_EpipolarSamples : 4);
		InitRenderTexture(ref m_CoordEpi, m_EpipolarSamples, m_EpipolarLines, 0, RenderTextureFormat.RGFloat);
		m_CoordEpi.filterMode = FilterMode.Point;
		InitRenderTexture(ref m_DepthEpi, m_EpipolarSamples, m_EpipolarLines, 0, RenderTextureFormat.RFloat);
		m_DepthEpi.filterMode = FilterMode.Point;
		ref RenderTexture interpolationEpi = ref m_InterpolationEpi;
		int epipolarSamples = m_EpipolarSamples;
		int epipolarLines = m_EpipolarLines;
		int format;
		if (m_DX11Support)
		{
			format = 18;
		}
		else
		{
			format = 12;
		}
		InitRenderTexture(ref interpolationEpi, epipolarSamples, epipolarLines, 0, (RenderTextureFormat)format);
		m_InterpolationEpi.filterMode = FilterMode.Point;
		InitRenderTexture(ref m_RaymarchedLightEpi, m_EpipolarSamples, m_EpipolarLines, 24, RenderTextureFormat.ARGBFloat);
		m_RaymarchedLightEpi.filterMode = FilterMode.Point;
		InitRenderTexture(ref m_InterpolateAlongRaysEpi, m_EpipolarSamples, m_EpipolarLines, 0, RenderTextureFormat.ARGBFloat);
		m_InterpolateAlongRaysEpi.filterMode = FilterMode.Point;
	}

	private void InitMaterial(ref Material material, Shader shader)
	{
		if ((bool)material)
		{
			return;
		}
		while (true)
		{
			if ((bool)shader)
			{
				material = new Material(shader);
				material.hideFlags = HideFlags.HideAndDontSave;
			}
			return;
		}
	}

	private void InitMaterials()
	{
		InitMaterial(ref m_FinalInterpolationMaterial, m_FinalInterpolationShader);
		InitMaterial(ref m_CoordMaterial, m_CoordShader);
		InitMaterial(ref m_SamplePositionsMaterial, m_SamplePositionsShader);
		InitMaterial(ref m_RaymarchMaterial, m_RaymarchShader);
		InitMaterial(ref m_DepthBreaksMaterial, m_DepthBreaksShader);
		InitMaterial(ref m_InterpolateAlongRaysMaterial, m_InterpolateAlongRaysShader);
	}

	private void InitSpotFrustumMesh()
	{
		if (!m_SpotMesh)
		{
			m_SpotMesh = new Mesh();
			m_SpotMesh.hideFlags = HideFlags.HideAndDontSave;
		}
		Light light = m_Light;
		if (m_SpotMeshNear == m_SpotNear && m_SpotMeshFar == m_SpotFar && m_SpotMeshAngle == light.spotAngle)
		{
			if (m_SpotMeshRange == light.range)
			{
				return;
			}
		}
		float num = light.range * m_SpotFar;
		float num2 = light.range * m_SpotNear;
		float num3 = Mathf.Tan(light.spotAngle * ((float)Math.PI / 180f) * 0.5f);
		float num4 = num * num3;
		float num5 = num2 * num3;
		Vector3[] array;
		if (m_SpotMesh.vertices != null)
		{
			if (m_SpotMesh.vertices.Length == 8)
			{
				array = m_SpotMesh.vertices;
				goto IL_0110;
			}
		}
		array = new Vector3[8];
		goto IL_0110;
		IL_023a:
		m_SpotMeshNear = m_SpotNear;
		m_SpotMeshFar = m_SpotFar;
		m_SpotMeshAngle = light.spotAngle;
		m_SpotMeshRange = light.range;
		return;
		IL_0110:
		Vector3[] array2 = array;
		array2[0] = new Vector3(0f - num4, 0f - num4, num);
		array2[1] = new Vector3(num4, 0f - num4, num);
		array2[2] = new Vector3(num4, num4, num);
		array2[3] = new Vector3(0f - num4, num4, num);
		array2[4] = new Vector3(0f - num5, 0f - num5, num2);
		array2[5] = new Vector3(num5, 0f - num5, num2);
		array2[6] = new Vector3(num5, num5, num2);
		array2[7] = new Vector3(0f - num5, num5, num2);
		m_SpotMesh.vertices = array2;
		if (m_SpotMesh.GetTopology(0) == MeshTopology.Triangles && m_SpotMesh.triangles != null)
		{
			if (m_SpotMesh.triangles.Length == 36)
			{
				goto IL_023a;
			}
		}
		int[] triangles = new int[36]
		{
			0,
			1,
			2,
			0,
			2,
			3,
			6,
			5,
			4,
			7,
			6,
			4,
			3,
			2,
			6,
			3,
			6,
			7,
			2,
			1,
			5,
			2,
			5,
			6,
			0,
			3,
			7,
			0,
			7,
			4,
			5,
			1,
			0,
			5,
			0,
			4
		};
		m_SpotMesh.triangles = triangles;
		goto IL_023a;
	}

	public void UpdateLightType()
	{
		if (m_Light == null)
		{
			m_Light = GetComponent<Light>();
		}
		m_LightType = m_Light.type;
	}

	private bool ShaderCompiles(Shader shader)
	{
		if (!shader.isSupported)
		{
			Debug.LogError("LightShafts' " + shader.name + " didn't compile on this platform.");
			return false;
		}
		return true;
	}

	public bool CheckMinRequirements()
	{
		m_DX11Support = (SystemInfo.graphicsShaderLevel >= 50);
		m_MinRequirements = (SystemInfo.graphicsShaderLevel >= 30);
		m_MinRequirements &= SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGFloat);
		m_MinRequirements &= SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RFloat);
		if (!m_MinRequirements)
		{
			Debug.LogError("LightShafts require Shader Model 3.0 and render textures (including the RGFloat and RFloat) formats. Disabling.");
		}
		int num;
		if (ShaderCompiles(m_DepthShader))
		{
			if (ShaderCompiles(m_ColorFilterShader))
			{
				if (ShaderCompiles(m_CoordShader) && ShaderCompiles(m_DepthBreaksShader))
				{
					if (ShaderCompiles(m_RaymarchShader) && ShaderCompiles(m_InterpolateAlongRaysShader))
					{
						num = (ShaderCompiles(m_FinalInterpolationShader) ? 1 : 0);
						goto IL_010b;
					}
				}
			}
		}
		num = 0;
		goto IL_010b;
		IL_010b:
		bool flag = (byte)num != 0;
		if (!flag)
		{
			Debug.LogError("LightShafts require above shaders. Disabling.");
		}
		m_MinRequirements &= flag;
		m_SamplePositionsShaderCompiles = m_SamplePositionsShader.isSupported;
		return m_MinRequirements;
	}

	private void InitResources()
	{
		UpdateLightType();
		InitMaterials();
		InitEpipolarTextures();
		InitLUTs();
		InitSpotFrustumMesh();
	}

	private void ReleaseResources()
	{
		ReleaseShadowmap();
		RenderTexture.ReleaseTemporary(m_CoordEpi);
		RenderTexture.ReleaseTemporary(m_DepthEpi);
		RenderTexture.ReleaseTemporary(m_InterpolationEpi);
		RenderTexture.ReleaseTemporary(m_RaymarchedLightEpi);
		RenderTexture.ReleaseTemporary(m_InterpolateAlongRaysEpi);
	}

	private Bounds GetBoundsLocal()
	{
		if (directional)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return new Bounds(new Vector3(0f, 0f, m_Size.z * 0.5f), m_Size);
				}
			}
		}
		Light light = m_Light;
		Vector3 center = new Vector3(0f, 0f, light.range * (m_SpotFar + m_SpotNear) * 0.5f);
		float z = (m_SpotFar - m_SpotNear) * light.range;
		float num = Mathf.Tan(light.spotAngle * ((float)Math.PI / 180f) * 0.5f) * m_SpotFar * light.range * 2f;
		return new Bounds(center, new Vector3(num, num, z));
	}

	private Matrix4x4 GetBoundsMatrix()
	{
		Bounds boundsLocal = GetBoundsLocal();
		Transform transform = base.transform;
		Vector3 position = transform.position;
		Vector3 forward = transform.forward;
		Vector3 center = boundsLocal.center;
		return Matrix4x4.TRS(position + forward * center.z, transform.rotation, boundsLocal.size);
	}

	private float GetFrustumApex()
	{
		return (0f - m_SpotNear) / (m_SpotFar - m_SpotNear) - 0.5f;
	}

	private void OnDrawGizmosSelected()
	{
		UpdateLightType();
		Gizmos.color = Color.yellow;
		if (directional)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Gizmos.matrix = GetBoundsMatrix();
					Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
					return;
				}
			}
		}
		if (!spot)
		{
			return;
		}
		while (true)
		{
			Transform transform = base.transform;
			Light light = m_Light;
			Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
			Gizmos.DrawFrustum(transform.position, light.spotAngle, light.range * m_SpotFar, light.range * m_SpotNear, 1f);
			return;
		}
	}

	private void RenderQuadSections(Vector4 lightPos)
	{
		for (int i = 0; i < 4; i++)
		{
			if (i == 0)
			{
				if (lightPos.y > 1f)
				{
					continue;
				}
			}
			if (i == 1)
			{
				if (lightPos.x > 1f)
				{
					continue;
				}
			}
			if (i == 2)
			{
				if (lightPos.y < -1f)
				{
					continue;
				}
			}
			if (i == 3)
			{
				if (lightPos.x < -1f)
				{
					continue;
				}
			}
			float num = (float)i / 2f - 1f;
			float y = num + 0.5f;
			GL.Begin(7);
			GL.Vertex3(-1f, num, 0f);
			GL.Vertex3(1f, num, 0f);
			GL.Vertex3(1f, y, 0f);
			GL.Vertex3(-1f, y, 0f);
			GL.End();
		}
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

	private void RenderQuad()
	{
		GL.Begin(7);
		GL.TexCoord2(0f, 0f);
		GL.Vertex3(-1f, -1f, 0f);
		GL.TexCoord2(0f, 1f);
		GL.Vertex3(-1f, 1f, 0f);
		GL.TexCoord2(1f, 1f);
		GL.Vertex3(1f, 1f, 0f);
		GL.TexCoord2(1f, 0f);
		GL.Vertex3(1f, -1f, 0f);
		GL.End();
	}

	private void RenderSpotFrustum()
	{
		Graphics.DrawMeshNow(m_SpotMesh, base.transform.position, base.transform.rotation);
	}

	private Vector4 GetLightViewportPos()
	{
		Vector3 position = base.transform.position;
		if (directional)
		{
			position = m_CurrentCamera.transform.position + base.transform.forward;
		}
		Vector3 vector = m_CurrentCamera.WorldToViewportPoint(position);
		return new Vector4(vector.x * 2f - 1f, vector.y * 2f - 1f, 0f, 0f);
	}

	private bool IsVisible()
	{
		Matrix4x4 worldToProjectionMatrix = m_CurrentCamera.projectionMatrix * m_CurrentCamera.worldToCameraMatrix * base.transform.localToWorldMatrix;
		return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(worldToProjectionMatrix), GetBoundsLocal());
	}

	private bool IntersectsNearPlane()
	{
		Vector3[] vertices = m_SpotMesh.vertices;
		float num = m_CurrentCamera.nearClipPlane - 0.001f;
		Transform transform = base.transform;
		for (int i = 0; i < vertices.Length; i++)
		{
			Vector3 vector = m_CurrentCamera.WorldToViewportPoint(transform.TransformPoint(vertices[i]));
			float z = vector.z;
			if (!(z < num))
			{
				continue;
			}
			while (true)
			{
				return true;
			}
		}
		while (true)
		{
			return false;
		}
	}

	private void SetKeyword(bool firstOn, string firstKeyword, string secondKeyword)
	{
		Shader.EnableKeyword((!firstOn) ? secondKeyword : firstKeyword);
		string keyword;
		if (firstOn)
		{
			keyword = secondKeyword;
		}
		else
		{
			keyword = firstKeyword;
		}
		Shader.DisableKeyword(keyword);
	}

	public void SetShadowmapDirty()
	{
		m_ShadowmapDirty = true;
	}

	private void GetFrustumRays(out Matrix4x4 frustumRays, out Vector3 cameraPosLocal)
	{
		float farClipPlane = m_CurrentCamera.farClipPlane;
		Vector3 position = m_CurrentCamera.transform.position;
		Matrix4x4 inverse = GetBoundsMatrix().inverse;
		Vector2[] array = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(1f, 1f),
			new Vector2(0f, 1f)
		};
		frustumRays = default(Matrix4x4);
		for (int i = 0; i < 4; i++)
		{
			Vector3 v = m_CurrentCamera.ViewportToWorldPoint(new Vector3(array[i].x, array[i].y, farClipPlane)) - position;
			v = inverse.MultiplyVector(v);
			frustumRays.SetRow(i, v);
		}
		while (true)
		{
			cameraPosLocal = inverse.MultiplyPoint3x4(position);
			return;
		}
	}

	private void SetFrustumRays(Material material)
	{
		GetFrustumRays(out Matrix4x4 frustumRays, out Vector3 cameraPosLocal);
		material.SetVector("_CameraPosLocal", cameraPosLocal);
		material.SetMatrix("_FrustumRays", frustumRays);
		material.SetFloat("_FrustumApex", GetFrustumApex());
	}

	private float GetDepthThresholdAdjusted()
	{
		return m_DepthThreshold / m_CurrentCamera.farClipPlane;
	}

	private bool CheckCamera()
	{
		if (m_Cameras == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		Camera[] cameras = m_Cameras;
		foreach (Camera x in cameras)
		{
			if (!(x == m_CurrentCamera))
			{
				continue;
			}
			while (true)
			{
				return true;
			}
		}
		while (true)
		{
			return false;
		}
	}

	public void UpdateCameraDepthMode()
	{
		if (m_Cameras == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		Camera[] cameras = m_Cameras;
		foreach (Camera camera in cameras)
		{
			if ((bool)camera)
			{
				camera.depthTextureMode |= DepthTextureMode.Depth;
			}
		}
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

	public void Start()
	{
		CheckMinRequirements();
		if (m_Cameras != null)
		{
			if (m_Cameras.Length != 0)
			{
				goto IL_004e;
			}
		}
		m_Cameras = new Camera[1]
		{
			Camera.main
		};
		goto IL_004e;
		IL_004e:
		UpdateCameraDepthMode();
	}

	private void UpdateShadowmap()
	{
		if (m_ShadowmapMode == LightShaftsShadowmapMode.Static)
		{
			if (!m_ShadowmapDirty)
			{
				return;
			}
		}
		InitShadowmap();
		if (m_ShadowmapCamera == null)
		{
			GameObject gameObject = new GameObject("Depth Camera");
			gameObject.AddComponent(typeof(Camera));
			m_ShadowmapCamera = gameObject.GetComponent<Camera>();
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			m_ShadowmapCamera.enabled = false;
			m_ShadowmapCamera.clearFlags = CameraClearFlags.Color;
		}
		Transform transform = m_ShadowmapCamera.transform;
		transform.position = base.transform.position;
		transform.rotation = base.transform.rotation;
		if (directional)
		{
			m_ShadowmapCamera.orthographic = true;
			m_ShadowmapCamera.nearClipPlane = 0f;
			m_ShadowmapCamera.farClipPlane = m_Size.z;
			m_ShadowmapCamera.orthographicSize = m_Size.y * 0.5f;
			m_ShadowmapCamera.aspect = m_Size.x / m_Size.y;
		}
		else
		{
			m_ShadowmapCamera.orthographic = false;
			m_ShadowmapCamera.nearClipPlane = m_SpotNear * m_Light.range;
			m_ShadowmapCamera.farClipPlane = m_SpotFar * m_Light.range;
			m_ShadowmapCamera.fieldOfView = m_Light.spotAngle;
			m_ShadowmapCamera.aspect = 1f;
		}
		m_ShadowmapCamera.renderingPath = RenderingPath.Forward;
		m_ShadowmapCamera.targetTexture = m_Shadowmap;
		m_ShadowmapCamera.cullingMask = m_CullingMask;
		m_ShadowmapCamera.backgroundColor = Color.white;
		m_ShadowmapCamera.RenderWithShader(m_DepthShader, "RenderType");
		if (m_Colored)
		{
			m_ShadowmapCamera.targetTexture = m_ColorFilter;
			m_ShadowmapCamera.cullingMask = m_ColorFilterMask;
			m_ShadowmapCamera.backgroundColor = new Color(m_ColorBalance, m_ColorBalance, m_ColorBalance);
			m_ShadowmapCamera.RenderWithShader(m_ColorFilterShader, string.Empty);
		}
		m_ShadowmapDirty = false;
	}

	private void RenderCoords(int width, int height, Vector4 lightPos)
	{
		SetFrustumRays(m_CoordMaterial);
		RenderBuffer[] colorBuffers = new RenderBuffer[2]
		{
			m_CoordEpi.colorBuffer,
			m_DepthEpi.colorBuffer
		};
		Graphics.SetRenderTarget(colorBuffers, m_DepthEpi.depthBuffer);
		m_CoordMaterial.SetVector("_LightPos", lightPos);
		m_CoordMaterial.SetVector("_CoordTexDim", new Vector4(m_CoordEpi.width, m_CoordEpi.height, 1f / (float)m_CoordEpi.width, 1f / (float)m_CoordEpi.height));
		m_CoordMaterial.SetVector("_ScreenTexDim", new Vector4(width, height, 1f / (float)width, 1f / (float)height));
		m_CoordMaterial.SetPass(0);
		RenderQuad();
	}

	private void RenderInterpolationTexture(Vector4 lightPos)
	{
		Graphics.SetRenderTarget(m_InterpolationEpi.colorBuffer, m_RaymarchedLightEpi.depthBuffer);
		if (m_DX11Support)
		{
			goto IL_0075;
		}
		if (Application.platform != RuntimePlatform.WindowsEditor)
		{
			if (Application.platform != RuntimePlatform.WindowsPlayer)
			{
				goto IL_0075;
			}
		}
		m_DepthBreaksMaterial.SetPass(1);
		RenderQuad();
		goto IL_0095;
		IL_0075:
		GL.Clear(true, true, new Color(0f, 0f, 0f, 1f));
		goto IL_0095;
		IL_0095:
		m_DepthBreaksMaterial.SetFloat("_InterpolationStep", m_InterpolationStep);
		m_DepthBreaksMaterial.SetFloat("_DepthThreshold", GetDepthThresholdAdjusted());
		m_DepthBreaksMaterial.SetTexture("_DepthEpi", m_DepthEpi);
		m_DepthBreaksMaterial.SetVector("_DepthEpiTexDim", new Vector4(m_DepthEpi.width, m_DepthEpi.height, 1f / (float)m_DepthEpi.width, 1f / (float)m_DepthEpi.height));
		m_DepthBreaksMaterial.SetPass(0);
		RenderQuadSections(lightPos);
	}

	private void InterpolateAlongRays(Vector4 lightPos)
	{
		Graphics.SetRenderTarget(m_InterpolateAlongRaysEpi);
		m_InterpolateAlongRaysMaterial.SetFloat("_InterpolationStep", m_InterpolationStep);
		m_InterpolateAlongRaysMaterial.SetTexture("_InterpolationEpi", m_InterpolationEpi);
		m_InterpolateAlongRaysMaterial.SetTexture("_RaymarchedLightEpi", m_RaymarchedLightEpi);
		m_InterpolateAlongRaysMaterial.SetVector("_RaymarchedLightEpiTexDim", new Vector4(m_RaymarchedLightEpi.width, m_RaymarchedLightEpi.height, 1f / (float)m_RaymarchedLightEpi.width, 1f / (float)m_RaymarchedLightEpi.height));
		m_InterpolateAlongRaysMaterial.SetPass(0);
		RenderQuadSections(lightPos);
	}

	private void RenderSamplePositions(int width, int height, Vector4 lightPos)
	{
		InitRenderTexture(ref m_SamplePositions, width, height, 0, RenderTextureFormat.ARGB32, false);
		m_SamplePositions.enableRandomWrite = true;
		m_SamplePositions.filterMode = FilterMode.Point;
		Graphics.SetRenderTarget(m_SamplePositions);
		GL.Clear(false, true, new Color(0f, 0f, 0f, 1f));
		Graphics.ClearRandomWriteTargets();
		Graphics.SetRandomWriteTarget(1, m_SamplePositions);
		Graphics.SetRenderTarget(m_RaymarchedLightEpi);
		m_SamplePositionsMaterial.SetVector("_OutputTexDim", new Vector4(width - 1, height - 1, 0f, 0f));
		m_SamplePositionsMaterial.SetVector("_CoordTexDim", new Vector4(m_CoordEpi.width, m_CoordEpi.height, 0f, 0f));
		m_SamplePositionsMaterial.SetTexture("_Coord", m_CoordEpi);
		m_SamplePositionsMaterial.SetTexture("_InterpolationEpi", m_InterpolationEpi);
		if (m_ShowInterpolatedSamples)
		{
			m_SamplePositionsMaterial.SetFloat("_SampleType", 1f);
			m_SamplePositionsMaterial.SetVector("_Color", new Vector4(0.4f, 0.4f, 0f, 0f));
			m_SamplePositionsMaterial.SetPass(0);
			RenderQuad();
		}
		m_SamplePositionsMaterial.SetFloat("_SampleType", 0f);
		m_SamplePositionsMaterial.SetVector("_Color", new Vector4(1f, 0f, 0f, 0f));
		m_SamplePositionsMaterial.SetPass(0);
		RenderQuadSections(lightPos);
		Graphics.ClearRandomWriteTargets();
	}

	private void ShowSamples(int width, int height, Vector4 lightPos)
	{
		int num;
		if (m_ShowSamples && m_DX11Support)
		{
			num = (m_SamplePositionsShaderCompiles ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		SetKeyword(flag, "SHOW_SAMPLES_ON", "SHOW_SAMPLES_OFF");
		if (flag)
		{
			RenderSamplePositions(width, height, lightPos);
		}
		m_FinalInterpolationMaterial.SetFloat("_ShowSamplesBackgroundFade", m_ShowSamplesBackgroundFade);
	}

	private void Raymarch(int width, int height, Vector4 lightPos)
	{
		SetFrustumRays(m_RaymarchMaterial);
		int width2 = m_Shadowmap.width;
		int height2 = m_Shadowmap.height;
		Graphics.SetRenderTarget(m_RaymarchedLightEpi.colorBuffer, m_RaymarchedLightEpi.depthBuffer);
		GL.Clear(false, true, new Color(0f, 0f, 0f, 1f));
		m_RaymarchMaterial.SetTexture("_Coord", m_CoordEpi);
		m_RaymarchMaterial.SetTexture("_InterpolationEpi", m_InterpolationEpi);
		m_RaymarchMaterial.SetTexture("_Shadowmap", m_Shadowmap);
		float num = (!m_Colored) ? m_Brightness : (m_BrightnessColored / m_ColorBalance);
		num *= m_Light.intensity;
		m_RaymarchMaterial.SetFloat("_Brightness", num);
		m_RaymarchMaterial.SetFloat("_Extinction", 0f - m_Extinction);
		m_RaymarchMaterial.SetVector("_ShadowmapDim", new Vector4(width2, height2, 1f / (float)width2, 1f / (float)height2));
		m_RaymarchMaterial.SetVector("_ScreenTexDim", new Vector4(width, height, 1f / (float)width, 1f / (float)height));
		m_RaymarchMaterial.SetVector("_LightColor", m_Light.color.linear);
		m_RaymarchMaterial.SetFloat("_MinDistFromCamera", m_MinDistFromCamera);
		SetKeyword(m_Colored, "COLORED_ON", "COLORED_OFF");
		m_RaymarchMaterial.SetTexture("_ColorFilter", m_ColorFilter);
		SetKeyword(m_AttenuationCurveOn, "ATTENUATION_CURVE_ON", "ATTENUATION_CURVE_OFF");
		m_RaymarchMaterial.SetTexture("_AttenuationCurveTex", m_AttenuationCurveTex);
		Texture cookie = m_Light.cookie;
		SetKeyword(cookie != null, "COOKIE_TEX_ON", "COOKIE_TEX_OFF");
		if (cookie != null)
		{
			m_RaymarchMaterial.SetTexture("_Cookie", cookie);
		}
		m_RaymarchMaterial.SetPass(0);
		RenderQuadSections(lightPos);
	}

	public void OnRenderObject()
	{
		m_CurrentCamera = Camera.current;
		if (!m_MinRequirements)
		{
			return;
		}
		while (true)
		{
			if (!CheckCamera())
			{
				return;
			}
			if (!IsVisible())
			{
				while (true)
				{
					switch (4)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			RenderBuffer activeDepthBuffer = Graphics.activeDepthBuffer;
			RenderBuffer activeColorBuffer = Graphics.activeColorBuffer;
			InitResources();
			Vector4 lightViewportPos = GetLightViewportPos();
			int num;
			if (lightViewportPos.x >= -1f && lightViewportPos.x <= 1f)
			{
				if (lightViewportPos.y >= -1f)
				{
					num = ((lightViewportPos.y <= 1f) ? 1 : 0);
					goto IL_00b8;
				}
			}
			num = 0;
			goto IL_00b8;
			IL_00b8:
			bool firstOn = (byte)num != 0;
			SetKeyword(firstOn, "LIGHT_ON_SCREEN", "LIGHT_OFF_SCREEN");
			int width = Screen.width;
			int height = Screen.height;
			UpdateShadowmap();
			SetKeyword(directional, "DIRECTIONAL_SHAFTS", "SPOT_SHAFTS");
			RenderCoords(width, height, lightViewportPos);
			RenderInterpolationTexture(lightViewportPos);
			Raymarch(width, height, lightViewportPos);
			InterpolateAlongRays(lightViewportPos);
			ShowSamples(width, height, lightViewportPos);
			SetFrustumRays(m_FinalInterpolationMaterial);
			m_FinalInterpolationMaterial.SetTexture("_InterpolationEpi", m_InterpolationEpi);
			m_FinalInterpolationMaterial.SetTexture("_DepthEpi", m_DepthEpi);
			m_FinalInterpolationMaterial.SetTexture("_Shadowmap", m_Shadowmap);
			m_FinalInterpolationMaterial.SetTexture("_Coord", m_CoordEpi);
			m_FinalInterpolationMaterial.SetTexture("_SamplePositions", m_SamplePositions);
			m_FinalInterpolationMaterial.SetTexture("_RaymarchedLight", m_InterpolateAlongRaysEpi);
			m_FinalInterpolationMaterial.SetVector("_CoordTexDim", new Vector4(m_CoordEpi.width, m_CoordEpi.height, 1f / (float)m_CoordEpi.width, 1f / (float)m_CoordEpi.height));
			m_FinalInterpolationMaterial.SetVector("_ScreenTexDim", new Vector4(width, height, 1f / (float)width, 1f / (float)height));
			m_FinalInterpolationMaterial.SetVector("_LightPos", lightViewportPos);
			m_FinalInterpolationMaterial.SetFloat("_DepthThreshold", GetDepthThresholdAdjusted());
			int num2;
			if (!directional)
			{
				num2 = (IntersectsNearPlane() ? 1 : 0);
			}
			else
			{
				num2 = 1;
			}
			bool flag = (byte)num2 != 0;
			Material finalInterpolationMaterial = m_FinalInterpolationMaterial;
			int num3;
			if (flag)
			{
				num3 = 8;
			}
			else
			{
				num3 = 2;
			}
			finalInterpolationMaterial.SetFloat("_ZTest", num3);
			SetKeyword(flag, "QUAD_SHAFTS", "FRUSTUM_SHAFTS");
			Graphics.SetRenderTarget(activeColorBuffer, activeDepthBuffer);
			m_FinalInterpolationMaterial.SetPass(0);
			if (flag)
			{
				RenderQuad();
			}
			else
			{
				RenderSpotFrustum();
			}
			ReleaseResources();
			return;
		}
	}
}
