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

	public int m_ShadowmapRes = 0x400;

	private Camera m_ShadowmapCamera;

	private RenderTexture m_Shadowmap;

	public Shader m_DepthShader;

	private RenderTexture m_ColorFilter;

	public Shader m_ColorFilterShader;

	public bool m_Colored;

	public float m_ColorBalance = 1f;

	public int m_EpipolarLines = 0x100;

	public int m_EpipolarSamples = 0x200;

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

	public int m_InterpolationStep = 0x20;

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

	private void InitLUTs()
	{
		if (this.m_AttenuationCurveTex)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.InitLUTs()).MethodHandle;
			}
			return;
		}
		this.m_AttenuationCurveTex = new Texture2D(0x100, 1, TextureFormat.ARGB32, false, true);
		this.m_AttenuationCurveTex.wrapMode = TextureWrapMode.Clamp;
		this.m_AttenuationCurveTex.hideFlags = HideFlags.HideAndDontSave;
		if (this.m_AttenuationCurve != null)
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
			if (this.m_AttenuationCurve.length != 0)
			{
				goto IL_C2;
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
		this.m_AttenuationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(1f, 1f)
		});
		IL_C2:
		if (this.m_AttenuationCurveTex)
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
			this.UpdateLUTs();
		}
	}

	public void UpdateLUTs()
	{
		this.InitLUTs();
		if (this.m_AttenuationCurve == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.UpdateLUTs()).MethodHandle;
			}
			return;
		}
		for (int i = 0; i < 0x100; i++)
		{
			float num = Mathf.Clamp(this.m_AttenuationCurve.Evaluate((float)i / 255f), 0f, 1f);
			this.m_AttenuationCurveTex.SetPixel(i, 0, new Color(num, num, num, num));
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
		this.m_AttenuationCurveTex.Apply();
	}

	private unsafe void InitRenderTexture(ref RenderTexture rt, int width, int height, int depth, RenderTextureFormat format, bool temp = true)
	{
		if (temp)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.InitRenderTexture(RenderTexture*, int, int, int, RenderTextureFormat, bool)).MethodHandle;
			}
			rt = RenderTexture.GetTemporary(width, height, depth, format);
		}
		else
		{
			if (rt != null)
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
				if (rt.width == width)
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
					if (rt.height == height)
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
						if (rt.depth == depth)
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
	}

	private void InitShadowmap()
	{
		bool flag = this.m_ShadowmapMode == LightShaftsShadowmapMode.Dynamic;
		if (flag && this.m_ShadowmapMode != this.m_ShadowmapModeOld)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.InitShadowmap()).MethodHandle;
			}
			if (this.m_Shadowmap)
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
				this.m_Shadowmap.Release();
			}
			if (this.m_ColorFilter)
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
				this.m_ColorFilter.Release();
			}
		}
		this.InitRenderTexture(ref this.m_Shadowmap, this.m_ShadowmapRes, this.m_ShadowmapRes, 0x18, RenderTextureFormat.RFloat, flag);
		this.m_Shadowmap.filterMode = FilterMode.Point;
		this.m_Shadowmap.wrapMode = TextureWrapMode.Clamp;
		if (this.m_Colored)
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
			this.InitRenderTexture(ref this.m_ColorFilter, this.m_ShadowmapRes, this.m_ShadowmapRes, 0, RenderTextureFormat.ARGB32, flag);
		}
		this.m_ShadowmapModeOld = this.m_ShadowmapMode;
	}

	private void ReleaseShadowmap()
	{
		if (this.m_ShadowmapMode == LightShaftsShadowmapMode.Static)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.ReleaseShadowmap()).MethodHandle;
			}
			return;
		}
		RenderTexture.ReleaseTemporary(this.m_Shadowmap);
		RenderTexture.ReleaseTemporary(this.m_ColorFilter);
	}

	private void InitEpipolarTextures()
	{
		this.m_EpipolarLines = ((this.m_EpipolarLines >= 8) ? this.m_EpipolarLines : 8);
		this.m_EpipolarSamples = ((this.m_EpipolarSamples >= 4) ? this.m_EpipolarSamples : 4);
		this.InitRenderTexture(ref this.m_CoordEpi, this.m_EpipolarSamples, this.m_EpipolarLines, 0, RenderTextureFormat.RGFloat, true);
		this.m_CoordEpi.filterMode = FilterMode.Point;
		this.InitRenderTexture(ref this.m_DepthEpi, this.m_EpipolarSamples, this.m_EpipolarLines, 0, RenderTextureFormat.RFloat, true);
		this.m_DepthEpi.filterMode = FilterMode.Point;
		int epipolarSamples = this.m_EpipolarSamples;
		int epipolarLines = this.m_EpipolarLines;
		int depth = 0;
		RenderTextureFormat format;
		if (this.m_DX11Support)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.InitEpipolarTextures()).MethodHandle;
			}
			format = RenderTextureFormat.RGInt;
		}
		else
		{
			format = RenderTextureFormat.RGFloat;
		}
		this.InitRenderTexture(ref this.m_InterpolationEpi, epipolarSamples, epipolarLines, depth, format, true);
		this.m_InterpolationEpi.filterMode = FilterMode.Point;
		this.InitRenderTexture(ref this.m_RaymarchedLightEpi, this.m_EpipolarSamples, this.m_EpipolarLines, 0x18, RenderTextureFormat.ARGBFloat, true);
		this.m_RaymarchedLightEpi.filterMode = FilterMode.Point;
		this.InitRenderTexture(ref this.m_InterpolateAlongRaysEpi, this.m_EpipolarSamples, this.m_EpipolarLines, 0, RenderTextureFormat.ARGBFloat, true);
		this.m_InterpolateAlongRaysEpi.filterMode = FilterMode.Point;
	}

	private unsafe void InitMaterial(ref Material material, Shader shader)
	{
		if (!material)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.InitMaterial(Material*, Shader)).MethodHandle;
			}
			if (shader)
			{
				material = new Material(shader);
				material.hideFlags = HideFlags.HideAndDontSave;
				return;
			}
		}
	}

	private void InitMaterials()
	{
		this.InitMaterial(ref this.m_FinalInterpolationMaterial, this.m_FinalInterpolationShader);
		this.InitMaterial(ref this.m_CoordMaterial, this.m_CoordShader);
		this.InitMaterial(ref this.m_SamplePositionsMaterial, this.m_SamplePositionsShader);
		this.InitMaterial(ref this.m_RaymarchMaterial, this.m_RaymarchShader);
		this.InitMaterial(ref this.m_DepthBreaksMaterial, this.m_DepthBreaksShader);
		this.InitMaterial(ref this.m_InterpolateAlongRaysMaterial, this.m_InterpolateAlongRaysShader);
	}

	private void InitSpotFrustumMesh()
	{
		if (!this.m_SpotMesh)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.InitSpotFrustumMesh()).MethodHandle;
			}
			this.m_SpotMesh = new Mesh();
			this.m_SpotMesh.hideFlags = HideFlags.HideAndDontSave;
		}
		Light light = this.m_Light;
		if (this.m_SpotMeshNear == this.m_SpotNear && this.m_SpotMeshFar == this.m_SpotFar && this.m_SpotMeshAngle == light.spotAngle)
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
			if (this.m_SpotMeshRange == light.range)
			{
				return;
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		float num = light.range * this.m_SpotFar;
		float num2 = light.range * this.m_SpotNear;
		float num3 = Mathf.Tan(light.spotAngle * 0.0174532924f * 0.5f);
		float num4 = num * num3;
		float num5 = num2 * num3;
		Vector3[] array;
		if (this.m_SpotMesh.vertices != null)
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
			if (this.m_SpotMesh.vertices.Length == 8)
			{
				array = this.m_SpotMesh.vertices;
				goto IL_110;
			}
		}
		array = new Vector3[8];
		IL_110:
		Vector3[] array2 = array;
		array2[0] = new Vector3(-num4, -num4, num);
		array2[1] = new Vector3(num4, -num4, num);
		array2[2] = new Vector3(num4, num4, num);
		array2[3] = new Vector3(-num4, num4, num);
		array2[4] = new Vector3(-num5, -num5, num2);
		array2[5] = new Vector3(num5, -num5, num2);
		array2[6] = new Vector3(num5, num5, num2);
		array2[7] = new Vector3(-num5, num5, num2);
		this.m_SpotMesh.vertices = array2;
		if (this.m_SpotMesh.GetTopology(0) == MeshTopology.Triangles && this.m_SpotMesh.triangles != null)
		{
			if (this.m_SpotMesh.triangles.Length == 0x24)
			{
				goto IL_23A;
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		int[] triangles = new int[]
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
		this.m_SpotMesh.triangles = triangles;
		IL_23A:
		this.m_SpotMeshNear = this.m_SpotNear;
		this.m_SpotMeshFar = this.m_SpotFar;
		this.m_SpotMeshAngle = light.spotAngle;
		this.m_SpotMeshRange = light.range;
	}

	public void UpdateLightType()
	{
		if (this.m_Light == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.UpdateLightType()).MethodHandle;
			}
			this.m_Light = base.GetComponent<Light>();
		}
		this.m_LightType = this.m_Light.type;
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
		this.m_DX11Support = (SystemInfo.graphicsShaderLevel >= 0x32);
		this.m_MinRequirements = (SystemInfo.graphicsShaderLevel >= 0x1E);
		this.m_MinRequirements &= SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGFloat);
		this.m_MinRequirements &= SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RFloat);
		if (!this.m_MinRequirements)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.CheckMinRequirements()).MethodHandle;
			}
			Debug.LogError("LightShafts require Shader Model 3.0 and render textures (including the RGFloat and RFloat) formats. Disabling.");
		}
		bool flag;
		if (this.ShaderCompiles(this.m_DepthShader))
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
			if (this.ShaderCompiles(this.m_ColorFilterShader))
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
				if (this.ShaderCompiles(this.m_CoordShader) && this.ShaderCompiles(this.m_DepthBreaksShader))
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
					if (this.ShaderCompiles(this.m_RaymarchShader) && this.ShaderCompiles(this.m_InterpolateAlongRaysShader))
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
						flag = this.ShaderCompiles(this.m_FinalInterpolationShader);
						goto IL_10B;
					}
				}
			}
		}
		flag = false;
		IL_10B:
		bool flag2 = flag;
		if (!flag2)
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
			Debug.LogError("LightShafts require above shaders. Disabling.");
		}
		this.m_MinRequirements = (this.m_MinRequirements && flag2);
		this.m_SamplePositionsShaderCompiles = this.m_SamplePositionsShader.isSupported;
		return this.m_MinRequirements;
	}

	private void InitResources()
	{
		this.UpdateLightType();
		this.InitMaterials();
		this.InitEpipolarTextures();
		this.InitLUTs();
		this.InitSpotFrustumMesh();
	}

	private void ReleaseResources()
	{
		this.ReleaseShadowmap();
		RenderTexture.ReleaseTemporary(this.m_CoordEpi);
		RenderTexture.ReleaseTemporary(this.m_DepthEpi);
		RenderTexture.ReleaseTemporary(this.m_InterpolationEpi);
		RenderTexture.ReleaseTemporary(this.m_RaymarchedLightEpi);
		RenderTexture.ReleaseTemporary(this.m_InterpolateAlongRaysEpi);
	}

	public bool directional
	{
		get
		{
			return this.m_LightType == LightType.Directional;
		}
	}

	public bool spot
	{
		get
		{
			return this.m_LightType == LightType.Spot;
		}
	}

	private Bounds GetBoundsLocal()
	{
		if (this.directional)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.GetBoundsLocal()).MethodHandle;
			}
			return new Bounds(new Vector3(0f, 0f, this.m_Size.z * 0.5f), this.m_Size);
		}
		Light light = this.m_Light;
		Vector3 center = new Vector3(0f, 0f, light.range * (this.m_SpotFar + this.m_SpotNear) * 0.5f);
		float z = (this.m_SpotFar - this.m_SpotNear) * light.range;
		float num = Mathf.Tan(light.spotAngle * 0.0174532924f * 0.5f) * this.m_SpotFar * light.range * 2f;
		return new Bounds(center, new Vector3(num, num, z));
	}

	private Matrix4x4 GetBoundsMatrix()
	{
		Bounds boundsLocal = this.GetBoundsLocal();
		Transform transform = base.transform;
		return Matrix4x4.TRS(transform.position + transform.forward * boundsLocal.center.z, transform.rotation, boundsLocal.size);
	}

	private float GetFrustumApex()
	{
		return -this.m_SpotNear / (this.m_SpotFar - this.m_SpotNear) - 0.5f;
	}

	private void OnDrawGizmosSelected()
	{
		this.UpdateLightType();
		Gizmos.color = Color.yellow;
		if (this.directional)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.OnDrawGizmosSelected()).MethodHandle;
			}
			Gizmos.matrix = this.GetBoundsMatrix();
			Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
		}
		else if (this.spot)
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
			Transform transform = base.transform;
			Light light = this.m_Light;
			Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
			Gizmos.DrawFrustum(transform.position, light.spotAngle, light.range * this.m_SpotFar, light.range * this.m_SpotNear, 1f);
		}
	}

	private void RenderQuadSections(Vector4 lightPos)
	{
		int i = 0;
		while (i < 4)
		{
			if (i != 0)
			{
				goto IL_2E;
			}
			if (lightPos.y <= 1f)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.RenderQuadSections(Vector4)).MethodHandle;
					goto IL_2E;
				}
				goto IL_2E;
			}
			IL_104:
			i++;
			continue;
			IL_A0:
			goto IL_104;
			IL_2E:
			if (i == 1)
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
				if (lightPos.x > 1f)
				{
					goto IL_A0;
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
			}
			if (i == 2)
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
				if (lightPos.y < -1f)
				{
					goto IL_A0;
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (i == 3)
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
				if (lightPos.x < -1f)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						goto IL_A0;
					}
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
			goto IL_104;
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
		Graphics.DrawMeshNow(this.m_SpotMesh, base.transform.position, base.transform.rotation);
	}

	private Vector4 GetLightViewportPos()
	{
		Vector3 position = base.transform.position;
		if (this.directional)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.GetLightViewportPos()).MethodHandle;
			}
			position = this.m_CurrentCamera.transform.position + base.transform.forward;
		}
		Vector3 vector = this.m_CurrentCamera.WorldToViewportPoint(position);
		return new Vector4(vector.x * 2f - 1f, vector.y * 2f - 1f, 0f, 0f);
	}

	private bool IsVisible()
	{
		Matrix4x4 worldToProjectionMatrix = this.m_CurrentCamera.projectionMatrix * this.m_CurrentCamera.worldToCameraMatrix * base.transform.localToWorldMatrix;
		return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(worldToProjectionMatrix), this.GetBoundsLocal());
	}

	private bool IntersectsNearPlane()
	{
		Vector3[] vertices = this.m_SpotMesh.vertices;
		float num = this.m_CurrentCamera.nearClipPlane - 0.001f;
		Transform transform = base.transform;
		for (int i = 0; i < vertices.Length; i++)
		{
			float z = this.m_CurrentCamera.WorldToViewportPoint(transform.TransformPoint(vertices[i])).z;
			if (z < num)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.IntersectsNearPlane()).MethodHandle;
				}
				return true;
			}
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
		return false;
	}

	private void SetKeyword(bool firstOn, string firstKeyword, string secondKeyword)
	{
		Shader.EnableKeyword((!firstOn) ? secondKeyword : firstKeyword);
		string keyword;
		if (firstOn)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.SetKeyword(bool, string, string)).MethodHandle;
			}
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
		this.m_ShadowmapDirty = true;
	}

	private unsafe void GetFrustumRays(out Matrix4x4 frustumRays, out Vector3 cameraPosLocal)
	{
		float farClipPlane = this.m_CurrentCamera.farClipPlane;
		Vector3 position = this.m_CurrentCamera.transform.position;
		Matrix4x4 inverse = this.GetBoundsMatrix().inverse;
		Vector2[] array = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(1f, 1f),
			new Vector2(0f, 1f)
		};
		frustumRays = default(Matrix4x4);
		for (int i = 0; i < 4; i++)
		{
			Vector3 v = this.m_CurrentCamera.ViewportToWorldPoint(new Vector3(array[i].x, array[i].y, farClipPlane)) - position;
			v = inverse.MultiplyVector(v);
			frustumRays.SetRow(i, v);
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.GetFrustumRays(Matrix4x4*, Vector3*)).MethodHandle;
		}
		cameraPosLocal = inverse.MultiplyPoint3x4(position);
	}

	private void SetFrustumRays(Material material)
	{
		Matrix4x4 value;
		Vector3 v;
		this.GetFrustumRays(out value, out v);
		material.SetVector("_CameraPosLocal", v);
		material.SetMatrix("_FrustumRays", value);
		material.SetFloat("_FrustumApex", this.GetFrustumApex());
	}

	private float GetDepthThresholdAdjusted()
	{
		return this.m_DepthThreshold / this.m_CurrentCamera.farClipPlane;
	}

	private bool CheckCamera()
	{
		if (this.m_Cameras == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.CheckCamera()).MethodHandle;
			}
			return false;
		}
		foreach (Camera x in this.m_Cameras)
		{
			if (x == this.m_CurrentCamera)
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
				return true;
			}
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
		return false;
	}

	public void UpdateCameraDepthMode()
	{
		if (this.m_Cameras == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.UpdateCameraDepthMode()).MethodHandle;
			}
			return;
		}
		foreach (Camera camera in this.m_Cameras)
		{
			if (camera)
			{
				camera.depthTextureMode |= DepthTextureMode.Depth;
			}
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
	}

	public void Start()
	{
		this.CheckMinRequirements();
		if (this.m_Cameras != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.Start()).MethodHandle;
			}
			if (this.m_Cameras.Length != 0)
			{
				goto IL_4E;
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
		}
		this.m_Cameras = new Camera[]
		{
			Camera.main
		};
		IL_4E:
		this.UpdateCameraDepthMode();
	}

	private void UpdateShadowmap()
	{
		if (this.m_ShadowmapMode == LightShaftsShadowmapMode.Static)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.UpdateShadowmap()).MethodHandle;
			}
			if (!this.m_ShadowmapDirty)
			{
				return;
			}
		}
		this.InitShadowmap();
		if (this.m_ShadowmapCamera == null)
		{
			GameObject gameObject = new GameObject("Depth Camera");
			gameObject.AddComponent(typeof(Camera));
			this.m_ShadowmapCamera = gameObject.GetComponent<Camera>();
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			this.m_ShadowmapCamera.enabled = false;
			this.m_ShadowmapCamera.clearFlags = CameraClearFlags.Color;
		}
		Transform transform = this.m_ShadowmapCamera.transform;
		transform.position = base.transform.position;
		transform.rotation = base.transform.rotation;
		if (this.directional)
		{
			this.m_ShadowmapCamera.orthographic = true;
			this.m_ShadowmapCamera.nearClipPlane = 0f;
			this.m_ShadowmapCamera.farClipPlane = this.m_Size.z;
			this.m_ShadowmapCamera.orthographicSize = this.m_Size.y * 0.5f;
			this.m_ShadowmapCamera.aspect = this.m_Size.x / this.m_Size.y;
		}
		else
		{
			this.m_ShadowmapCamera.orthographic = false;
			this.m_ShadowmapCamera.nearClipPlane = this.m_SpotNear * this.m_Light.range;
			this.m_ShadowmapCamera.farClipPlane = this.m_SpotFar * this.m_Light.range;
			this.m_ShadowmapCamera.fieldOfView = this.m_Light.spotAngle;
			this.m_ShadowmapCamera.aspect = 1f;
		}
		this.m_ShadowmapCamera.renderingPath = RenderingPath.Forward;
		this.m_ShadowmapCamera.targetTexture = this.m_Shadowmap;
		this.m_ShadowmapCamera.cullingMask = this.m_CullingMask;
		this.m_ShadowmapCamera.backgroundColor = Color.white;
		this.m_ShadowmapCamera.RenderWithShader(this.m_DepthShader, "RenderType");
		if (this.m_Colored)
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
			this.m_ShadowmapCamera.targetTexture = this.m_ColorFilter;
			this.m_ShadowmapCamera.cullingMask = this.m_ColorFilterMask;
			this.m_ShadowmapCamera.backgroundColor = new Color(this.m_ColorBalance, this.m_ColorBalance, this.m_ColorBalance);
			this.m_ShadowmapCamera.RenderWithShader(this.m_ColorFilterShader, string.Empty);
		}
		this.m_ShadowmapDirty = false;
	}

	private void RenderCoords(int width, int height, Vector4 lightPos)
	{
		this.SetFrustumRays(this.m_CoordMaterial);
		RenderBuffer[] colorBuffers = new RenderBuffer[]
		{
			this.m_CoordEpi.colorBuffer,
			this.m_DepthEpi.colorBuffer
		};
		Graphics.SetRenderTarget(colorBuffers, this.m_DepthEpi.depthBuffer);
		this.m_CoordMaterial.SetVector("_LightPos", lightPos);
		this.m_CoordMaterial.SetVector("_CoordTexDim", new Vector4((float)this.m_CoordEpi.width, (float)this.m_CoordEpi.height, 1f / (float)this.m_CoordEpi.width, 1f / (float)this.m_CoordEpi.height));
		this.m_CoordMaterial.SetVector("_ScreenTexDim", new Vector4((float)width, (float)height, 1f / (float)width, 1f / (float)height));
		this.m_CoordMaterial.SetPass(0);
		this.RenderQuad();
	}

	private void RenderInterpolationTexture(Vector4 lightPos)
	{
		Graphics.SetRenderTarget(this.m_InterpolationEpi.colorBuffer, this.m_RaymarchedLightEpi.depthBuffer);
		if (!this.m_DX11Support)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.RenderInterpolationTexture(Vector4)).MethodHandle;
			}
			if (Application.platform != RuntimePlatform.WindowsEditor)
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
				if (Application.platform != RuntimePlatform.WindowsPlayer)
				{
					goto IL_75;
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
			this.m_DepthBreaksMaterial.SetPass(1);
			this.RenderQuad();
			goto IL_95;
		}
		IL_75:
		GL.Clear(true, true, new Color(0f, 0f, 0f, 1f));
		IL_95:
		this.m_DepthBreaksMaterial.SetFloat("_InterpolationStep", (float)this.m_InterpolationStep);
		this.m_DepthBreaksMaterial.SetFloat("_DepthThreshold", this.GetDepthThresholdAdjusted());
		this.m_DepthBreaksMaterial.SetTexture("_DepthEpi", this.m_DepthEpi);
		this.m_DepthBreaksMaterial.SetVector("_DepthEpiTexDim", new Vector4((float)this.m_DepthEpi.width, (float)this.m_DepthEpi.height, 1f / (float)this.m_DepthEpi.width, 1f / (float)this.m_DepthEpi.height));
		this.m_DepthBreaksMaterial.SetPass(0);
		this.RenderQuadSections(lightPos);
	}

	private void InterpolateAlongRays(Vector4 lightPos)
	{
		Graphics.SetRenderTarget(this.m_InterpolateAlongRaysEpi);
		this.m_InterpolateAlongRaysMaterial.SetFloat("_InterpolationStep", (float)this.m_InterpolationStep);
		this.m_InterpolateAlongRaysMaterial.SetTexture("_InterpolationEpi", this.m_InterpolationEpi);
		this.m_InterpolateAlongRaysMaterial.SetTexture("_RaymarchedLightEpi", this.m_RaymarchedLightEpi);
		this.m_InterpolateAlongRaysMaterial.SetVector("_RaymarchedLightEpiTexDim", new Vector4((float)this.m_RaymarchedLightEpi.width, (float)this.m_RaymarchedLightEpi.height, 1f / (float)this.m_RaymarchedLightEpi.width, 1f / (float)this.m_RaymarchedLightEpi.height));
		this.m_InterpolateAlongRaysMaterial.SetPass(0);
		this.RenderQuadSections(lightPos);
	}

	private void RenderSamplePositions(int width, int height, Vector4 lightPos)
	{
		this.InitRenderTexture(ref this.m_SamplePositions, width, height, 0, RenderTextureFormat.ARGB32, false);
		this.m_SamplePositions.enableRandomWrite = true;
		this.m_SamplePositions.filterMode = FilterMode.Point;
		Graphics.SetRenderTarget(this.m_SamplePositions);
		GL.Clear(false, true, new Color(0f, 0f, 0f, 1f));
		Graphics.ClearRandomWriteTargets();
		Graphics.SetRandomWriteTarget(1, this.m_SamplePositions);
		Graphics.SetRenderTarget(this.m_RaymarchedLightEpi);
		this.m_SamplePositionsMaterial.SetVector("_OutputTexDim", new Vector4((float)(width - 1), (float)(height - 1), 0f, 0f));
		this.m_SamplePositionsMaterial.SetVector("_CoordTexDim", new Vector4((float)this.m_CoordEpi.width, (float)this.m_CoordEpi.height, 0f, 0f));
		this.m_SamplePositionsMaterial.SetTexture("_Coord", this.m_CoordEpi);
		this.m_SamplePositionsMaterial.SetTexture("_InterpolationEpi", this.m_InterpolationEpi);
		if (this.m_ShowInterpolatedSamples)
		{
			this.m_SamplePositionsMaterial.SetFloat("_SampleType", 1f);
			this.m_SamplePositionsMaterial.SetVector("_Color", new Vector4(0.4f, 0.4f, 0f, 0f));
			this.m_SamplePositionsMaterial.SetPass(0);
			this.RenderQuad();
		}
		this.m_SamplePositionsMaterial.SetFloat("_SampleType", 0f);
		this.m_SamplePositionsMaterial.SetVector("_Color", new Vector4(1f, 0f, 0f, 0f));
		this.m_SamplePositionsMaterial.SetPass(0);
		this.RenderQuadSections(lightPos);
		Graphics.ClearRandomWriteTargets();
	}

	private void ShowSamples(int width, int height, Vector4 lightPos)
	{
		bool flag;
		if (this.m_ShowSamples && this.m_DX11Support)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.ShowSamples(int, int, Vector4)).MethodHandle;
			}
			flag = this.m_SamplePositionsShaderCompiles;
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		this.SetKeyword(flag2, "SHOW_SAMPLES_ON", "SHOW_SAMPLES_OFF");
		if (flag2)
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
			this.RenderSamplePositions(width, height, lightPos);
		}
		this.m_FinalInterpolationMaterial.SetFloat("_ShowSamplesBackgroundFade", this.m_ShowSamplesBackgroundFade);
	}

	private void Raymarch(int width, int height, Vector4 lightPos)
	{
		this.SetFrustumRays(this.m_RaymarchMaterial);
		int width2 = this.m_Shadowmap.width;
		int height2 = this.m_Shadowmap.height;
		Graphics.SetRenderTarget(this.m_RaymarchedLightEpi.colorBuffer, this.m_RaymarchedLightEpi.depthBuffer);
		GL.Clear(false, true, new Color(0f, 0f, 0f, 1f));
		this.m_RaymarchMaterial.SetTexture("_Coord", this.m_CoordEpi);
		this.m_RaymarchMaterial.SetTexture("_InterpolationEpi", this.m_InterpolationEpi);
		this.m_RaymarchMaterial.SetTexture("_Shadowmap", this.m_Shadowmap);
		float num = (!this.m_Colored) ? this.m_Brightness : (this.m_BrightnessColored / this.m_ColorBalance);
		num *= this.m_Light.intensity;
		this.m_RaymarchMaterial.SetFloat("_Brightness", num);
		this.m_RaymarchMaterial.SetFloat("_Extinction", -this.m_Extinction);
		this.m_RaymarchMaterial.SetVector("_ShadowmapDim", new Vector4((float)width2, (float)height2, 1f / (float)width2, 1f / (float)height2));
		this.m_RaymarchMaterial.SetVector("_ScreenTexDim", new Vector4((float)width, (float)height, 1f / (float)width, 1f / (float)height));
		this.m_RaymarchMaterial.SetVector("_LightColor", this.m_Light.color.linear);
		this.m_RaymarchMaterial.SetFloat("_MinDistFromCamera", this.m_MinDistFromCamera);
		this.SetKeyword(this.m_Colored, "COLORED_ON", "COLORED_OFF");
		this.m_RaymarchMaterial.SetTexture("_ColorFilter", this.m_ColorFilter);
		this.SetKeyword(this.m_AttenuationCurveOn, "ATTENUATION_CURVE_ON", "ATTENUATION_CURVE_OFF");
		this.m_RaymarchMaterial.SetTexture("_AttenuationCurveTex", this.m_AttenuationCurveTex);
		Texture cookie = this.m_Light.cookie;
		this.SetKeyword(cookie != null, "COOKIE_TEX_ON", "COOKIE_TEX_OFF");
		if (cookie != null)
		{
			this.m_RaymarchMaterial.SetTexture("_Cookie", cookie);
		}
		this.m_RaymarchMaterial.SetPass(0);
		this.RenderQuadSections(lightPos);
	}

	public void OnRenderObject()
	{
		this.m_CurrentCamera = Camera.current;
		if (this.m_MinRequirements)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LightShafts.OnRenderObject()).MethodHandle;
			}
			if (this.CheckCamera())
			{
				if (this.IsVisible())
				{
					RenderBuffer activeDepthBuffer = Graphics.activeDepthBuffer;
					RenderBuffer activeColorBuffer = Graphics.activeColorBuffer;
					this.InitResources();
					Vector4 lightViewportPos = this.GetLightViewportPos();
					bool flag;
					if (lightViewportPos.x >= -1f && lightViewportPos.x <= 1f)
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
						if (lightViewportPos.y >= -1f)
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
							flag = (lightViewportPos.y <= 1f);
							goto IL_B8;
						}
					}
					flag = false;
					IL_B8:
					bool firstOn = flag;
					this.SetKeyword(firstOn, "LIGHT_ON_SCREEN", "LIGHT_OFF_SCREEN");
					int width = Screen.width;
					int height = Screen.height;
					this.UpdateShadowmap();
					this.SetKeyword(this.directional, "DIRECTIONAL_SHAFTS", "SPOT_SHAFTS");
					this.RenderCoords(width, height, lightViewportPos);
					this.RenderInterpolationTexture(lightViewportPos);
					this.Raymarch(width, height, lightViewportPos);
					this.InterpolateAlongRays(lightViewportPos);
					this.ShowSamples(width, height, lightViewportPos);
					this.SetFrustumRays(this.m_FinalInterpolationMaterial);
					this.m_FinalInterpolationMaterial.SetTexture("_InterpolationEpi", this.m_InterpolationEpi);
					this.m_FinalInterpolationMaterial.SetTexture("_DepthEpi", this.m_DepthEpi);
					this.m_FinalInterpolationMaterial.SetTexture("_Shadowmap", this.m_Shadowmap);
					this.m_FinalInterpolationMaterial.SetTexture("_Coord", this.m_CoordEpi);
					this.m_FinalInterpolationMaterial.SetTexture("_SamplePositions", this.m_SamplePositions);
					this.m_FinalInterpolationMaterial.SetTexture("_RaymarchedLight", this.m_InterpolateAlongRaysEpi);
					this.m_FinalInterpolationMaterial.SetVector("_CoordTexDim", new Vector4((float)this.m_CoordEpi.width, (float)this.m_CoordEpi.height, 1f / (float)this.m_CoordEpi.width, 1f / (float)this.m_CoordEpi.height));
					this.m_FinalInterpolationMaterial.SetVector("_ScreenTexDim", new Vector4((float)width, (float)height, 1f / (float)width, 1f / (float)height));
					this.m_FinalInterpolationMaterial.SetVector("_LightPos", lightViewportPos);
					this.m_FinalInterpolationMaterial.SetFloat("_DepthThreshold", this.GetDepthThresholdAdjusted());
					bool flag2;
					if (!this.directional)
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
						flag2 = this.IntersectsNearPlane();
					}
					else
					{
						flag2 = true;
					}
					bool flag3 = flag2;
					Material finalInterpolationMaterial = this.m_FinalInterpolationMaterial;
					string name = "_ZTest";
					float num;
					if (flag3)
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
						num = (float)8;
					}
					else
					{
						num = (float)2;
					}
					finalInterpolationMaterial.SetFloat(name, num);
					this.SetKeyword(flag3, "QUAD_SHAFTS", "FRUSTUM_SHAFTS");
					Graphics.SetRenderTarget(activeColorBuffer, activeDepthBuffer);
					this.m_FinalInterpolationMaterial.SetPass(0);
					if (flag3)
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
						this.RenderQuad();
					}
					else
					{
						this.RenderSpotFrustum();
					}
					this.ReleaseResources();
					return;
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
	}
}
