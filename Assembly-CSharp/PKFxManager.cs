using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;
using AOT;
using UnityEngine;

public static class PKFxManager
{
	internal static HashSet<string> m_preloadedPKFXPaths = new HashSet<string>();

	public const uint POPCORN_MAGIC_NUMBER = 0x5AFE0000U;

	public const uint PK_DESC_NAME_MAX_LEN = 0x40U;

	public const uint PK_DESC_DESC_MAX_LEN = 0x80U;

	private const string kPopcornPluginName = "PK-UnityPlugin";

	private const string m_UnityVersion = "Unity 5.2 and up";

	public const string m_PluginVersion = "2.9p6 for Unity 5.2 and up";

	public static string m_PackPath = Application.streamingAssetsPath;

	public static string m_CurrentVersionString = string.Empty;

	public static bool m_PackCopied = false;

	public static bool m_PackLoaded = false;

	public static PKFxManager.PKFxConf m_GlobalConf;

	public static string m_LogFilePath = Path.GetFullPath(Path.Combine(Application.dataPath, "../popcorn.htm"));

	public static bool m_IsStarted = false;

	private static float[] m_Samples;

	private static GCHandle m_SamplesHandle;

	private static bool m_HasSpawnerIDs = false;

	private static bool m_HasFileLogging = false;

	private static bool m_IsUsingOrthographicProjection = false;

	static PKFxManager()
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(PKFxManager.PKFxConf));
		if (Application.platform == RuntimePlatform.Android)
		{
			PKFxManager.m_PackPath = Application.persistentDataPath;
			IEnumerator<object> enumerator = PKFxManager.AndroidRetrieveConfFile().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
				}
			}
			finally
			{
				if (enumerator != null)
				{
					enumerator.Dispose();
				}
			}
		}
		string text = PKFxManager.m_PackPath + "/PKconfig.cfg";
		if (File.Exists(text))
		{
			FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Read);
			StreamReader textReader = new StreamReader(fileStream, Encoding.ASCII);
			PKFxManager.m_GlobalConf = (xmlSerializer.Deserialize(textReader) as PKFxManager.PKFxConf);
			fileStream.Close();
		}
		else
		{
			Debug.LogWarning("[PKFX] Can't find conf file : " + text);
			PKFxManager.m_GlobalConf = new PKFxManager.PKFxConf();
			PKFxManager.m_GlobalConf.Save();
		}
		PKFxManager.EnableFileLoggingIFN(PKFxManager.m_GlobalConf.enableFileLog);
		PKFxManager.SetupPackInPersistantDataPathIFN(PKFxManager.m_GlobalConf.enablePackFxInPersistentDataPath);
	}

	internal static void UnloadAll()
	{
		using (HashSet<string>.Enumerator enumerator = PKFxManager.m_preloadedPKFXPaths.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string path = enumerator.Current;
				PKFxManager.UnloadEffect(path);
			}
		}
		PKFxManager.m_preloadedPKFXPaths.Clear();
	}

	[DllImport("PK-UnityPlugin")]
	public static extern IntPtr GetRuntimeVersion();

	[DllImport("PK-UnityPlugin")]
	public static extern void GetStats(ref PKFxManager.S_Stats stats);

	[DllImport("PK-UnityPlugin")]
	public static extern int LoadFx(PKFxManager.FxDesc path);

	[DllImport("PK-UnityPlugin")]
	public static extern bool StopFx(int guid);

	[DllImport("PK-UnityPlugin")]
	public static extern bool TerminateFx(int guid);

	[DllImport("PK-UnityPlugin")]
	public static extern bool KillFx(int guid);

	[DllImport("PK-UnityPlugin")]
	public static extern bool IsFxAlive(int guid);

	[DllImport("PK-UnityPlugin")]
	public static extern void PreLoadFxIFN(string path);

	[DllImport("PK-UnityPlugin")]
	public static extern uint LoadShader(PKFxManager.ShaderDesc desc);

	[DllImport("PK-UnityPlugin")]
	public static extern void UnloadShader(string path);

	[DllImport("PK-UnityPlugin")]
	public static extern IntPtr GetDefaultShaderString(int api, int type);

	[DllImport("PK-UnityPlugin")]
	public static extern int ShaderConstantsCount(string path, int api);

	[DllImport("PK-UnityPlugin")]
	public static extern bool ShaderFillConstantDesc(string path, int constantId, ref PKFxManager.S_ShaderConstantDesc desc);

	[DllImport("PK-UnityPlugin")]
	public static extern bool ShaderSetConstant(uint shaderId, int constantCount, [In] IntPtr attributes);

	[DllImport("PK-UnityPlugin")]
	public static extern bool LoadPack(string path);

	[DllImport("PK-UnityPlugin")]
	public static extern bool EffectSetTransforms(int guid, Matrix4x4 tranforms);

	[DllImport("PK-UnityPlugin")]
	public static extern int EffectSamplersCountFromFx(string fxName);

	[DllImport("PK-UnityPlugin")]
	public static extern bool EffectFillSamplerDescFromFx(string fxName, int samplerID, ref PKFxManager.S_SamplerDesc desc);

	[DllImport("PK-UnityPlugin")]
	public static extern bool EffectSetSamplers(int guid, int samplerCount, [In] IntPtr samplers);

	[DllImport("PK-UnityPlugin")]
	public static extern bool EffectUpdateSamplerSkinning(int guid, int samplerId, [In] IntPtr samplers, float dt);

	[DllImport("PK-UnityPlugin")]
	public static extern int EffectAttributesCount(int guid);

	[DllImport("PK-UnityPlugin")]
	public static extern bool EffectFillAttributeDesc(int fxGUID, int attrID, ref PKFxManager.S_AttributeDesc desc);

	[DllImport("PK-UnityPlugin")]
	public static extern int EffectAttributesCountFromFx(string fxName);

	[DllImport("PK-UnityPlugin")]
	public static extern bool EffectFillAttributeDescFromFx(string fxName, int attrID, ref PKFxManager.S_AttributeDesc desc);

	[DllImport("PK-UnityPlugin")]
	public static extern bool EffectSetAttributes(int guid, int attributeCount, [In] IntPtr attributes);

	[DllImport("PK-UnityPlugin")]
	public static extern void SetDelegateOnFxStopped(IntPtr delegatePtr);

	[DllImport("PK-UnityPlugin")]
	public static extern void SetDelegateOnFxHotReloaded(IntPtr delegatePtr);

	[DllImport("PK-UnityPlugin")]
	public static extern void SetDelegateOnAudioSpectrumData(IntPtr delegatePtr);

	[DllImport("PK-UnityPlugin")]
	public static extern void SetDelegateOnAudioWaveformData(IntPtr delegatePtr);

	[DllImport("PK-UnityPlugin")]
	public static extern void SetDelegateOnStartSound(IntPtr delegatePtr);

	[DllImport("PK-UnityPlugin")]
	public static extern void SetReversedZBuffer(bool zBufferReversed);

	[DllImport("PK-UnityPlugin")]
	public static extern void SetupColorSpace(bool isSRGB);

	[DllImport("PK-UnityPlugin")]
	public static extern bool UnloadEffect(string path);

	[DllImport("PK-UnityPlugin")]
	public static extern void LogicalUpdate(float dt);

	[DllImport("PK-UnityPlugin")]
	public static extern void UpdateParticles(PKFxManager.CamDesc desc);

	[DllImport("PK-UnityPlugin")]
	public static extern void UpdateCamDesc(int camID, PKFxManager.CamDesc desc, bool update);

	[DllImport("PK-UnityPlugin")]
	public static extern void Reset();

	[DllImport("PK-UnityPlugin")]
	public static extern void DeepReset();

	[DllImport("PK-UnityPlugin")]
	public static extern bool LoadPkmmAsSceneMesh(string pkmmVirtualPath);

	[DllImport("PK-UnityPlugin")]
	public static extern void SceneMeshClear();

	[DllImport("PK-UnityPlugin")]
	public static extern bool SceneMeshAddRawMesh(int indicesCount, int[] indices, int verticesCount, Vector3[] vertices, Vector3[] normals, Matrix4x4 MeshMatrix);

	[DllImport("PK-UnityPlugin")]
	public static extern int SceneMeshBuild(string outputPkmmVirtualPath);

	[DllImport("PK-UnityPlugin")]
	public static extern void UnitySetGraphicsDevice(IntPtr device, int deviceType, int eventType);

	[DllImport("PK-UnityPlugin")]
	public static extern void UnityRenderEvent(int camID);

	[DllImport("PK-UnityPlugin")]
	public static extern void EnableSpawnerIDs(bool enable);

	[DllImport("PK-UnityPlugin")]
	public static extern void WriteProfileReport(string path);

	[DllImport("PK-UnityPlugin")]
	public static extern void ProfilerSetEnable(bool enable);

	[DllImport("PK-UnityPlugin")]
	public static extern IntPtr GetRenderEventFunc();

	[DllImport("PK-UnityPlugin")]
	public static extern IntPtr GetGLConstantsCountEvent();

	[DllImport("PK-UnityPlugin")]
	public static extern void GLConstantsCountEvent(int eventId);

	[DllImport("PK-UnityPlugin")]
	public static extern void SetUseOrthographicProjection(bool enable);

	[DllImport("PK-UnityPlugin")]
	public static extern void TransformAllParticles(Matrix4x4 transform);

	private static int ftoi(float fff)
	{
		return BitConverter.ToInt32(BitConverter.GetBytes(fff), 0);
	}

	private static float itof(int i)
	{
		return BitConverter.ToSingle(BitConverter.GetBytes(i), 0);
	}

	public static bool SceneMeshAddMesh(Mesh mesh, Matrix4x4 localToWorldMatrix)
	{
		int subMeshCount = mesh.subMeshCount;
		if (subMeshCount <= 0)
		{
			Debug.LogError("[PKFX] Mesh doesn't have sub meshes");
			return false;
		}
		int vertexCount = mesh.vertexCount;
		if (mesh.subMeshCount > 1)
		{
			Debug.LogWarning("[PKFX] Mesh has more than 1 submesh: non opti");
		}
		int i = 0;
		while (i < mesh.subMeshCount)
		{
			int indicesCount = mesh.GetIndices(i).Length;
			Debug.Log(string.Concat(new string[]
			{
				"[PKFX] Mesh (",
				(i + 1).ToString(),
				"/",
				subMeshCount.ToString(),
				") idx:",
				indicesCount.ToString(),
				" v:",
				vertexCount.ToString(),
				" v:",
				mesh.vertices.Length.ToString(),
				" n:",
				mesh.normals.Length.ToString(),
				" uv:",
				mesh.uv.Length.ToString()
			}));
			if (mesh.vertices.Length != vertexCount)
			{
				goto IL_170;
			}
			if (mesh.normals.Length != vertexCount)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					goto IL_170;
				}
			}
			IL_17A:
			if (!PKFxManager.SceneMeshAddRawMesh(indicesCount, mesh.GetIndices(i), vertexCount, mesh.vertices, mesh.normals, localToWorldMatrix))
			{
				Debug.LogError("[PKFX] Fail to load raw mesh");
			}
			i++;
			continue;
			IL_170:
			Debug.LogError("[PKFX] Invalid mesh");
			goto IL_17A;
		}
		return true;
	}

	public static void Render(short cameraID)
	{
		if (cameraID < 0)
		{
			Debug.LogError("[PKFX] PKFxManager: invalid cameraID for rendering " + cameraID);
		}
	}

	private static IEnumerable<object> AndroidRetrieveConfFile()
	{
		WWW www = new WWW(Path.Combine(Application.streamingAssetsPath, "PKconfig.cfg"));
		while (!www.isDone)
		{
			yield return www;
		}
		File.WriteAllBytes(PKFxManager.m_PackPath + "/PKconfig.cfg", www.bytes);
		www.Dispose();
		yield break;
	}

	public static void Startup()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			PKFxManager.UnitySetGraphicsDevice(IntPtr.Zero, 8, 0);
		}
		if (SystemInfo.usesReversedZBuffer)
		{
			PKFxManager.SetReversedZBuffer(true);
		}
		PKFxManager.SetupColorSpace(QualitySettings.activeColorSpace == ColorSpace.Linear);
		PKFxManager.EnableKillIndividualEffect(PKFxManager.m_GlobalConf.enableEffectsKill);
		PKFxManager.SetUseOrthographicProjection(PKFxManager.m_GlobalConf.useOrthographicProjection);
		PKFxManager.m_IsUsingOrthographicProjection = PKFxManager.m_GlobalConf.useOrthographicProjection;
		PKFxManager.SetDelegateOnAudioSpectrumData(Marshal.GetFunctionPointerForDelegate(new Func<IntPtr, IntPtr, IntPtr>(PKFxManager.OnAudioSpectrumData)));
		PKFxManager.SetDelegateOnAudioWaveformData(Marshal.GetFunctionPointerForDelegate(new Func<IntPtr, IntPtr, IntPtr>(PKFxManager.OnAudioWaveformData)));
		PKFxManager.SetDelegateOnFxStopped(Marshal.GetFunctionPointerForDelegate(new Action<int>(PKFxManager.OnFxStopped)));
		PKFxManager.SetDelegateOnFxHotReloaded(Marshal.GetFunctionPointerForDelegate(new Action<int, int>(PKFxManager.OnFxHotReloaded)));
		PKFxManager.SetDelegateOnStartSound(Marshal.GetFunctionPointerForDelegate(new Action<IntPtr>(PKFxSoundManager.OnStartSound)));
		PKFxManager.m_Samples = new float[0x400];
		PKFxManager.m_SamplesHandle = GCHandle.Alloc(PKFxManager.m_Samples, GCHandleType.Pinned);
		PKFxManager.m_CurrentVersionString = Marshal.PtrToStringAnsi(PKFxManager.GetRuntimeVersion());
		PKFxManager.m_IsStarted = true;
	}

	public static bool TryLoadPackRelative()
	{
		string text = Application.dataPath;
		string packPath = PKFxManager.m_PackPath;
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		if (string.IsNullOrEmpty(packPath))
		{
			return false;
		}
		int num = text.LastIndexOf("/");
		if (num > 0)
		{
			text = text.Substring(0, num);
		}
		Uri uri = new Uri(text + "/");
		Uri uri2 = new Uri(packPath + "/");
		if (uri.Scheme != uri2.Scheme)
		{
			return false;
		}
		Uri uri3 = uri.MakeRelativeUri(uri2);
		string str = Uri.UnescapeDataString(uri3.ToString());
		return PKFxManager.LoadPack(str + "PackFx");
	}

	[MonoPInvokeCallback(typeof(PKFxManager.FxCallback))]
	public static void OnFxStopped(int guid)
	{
		PKFxFX pkfxFX;
		if (PKFxFX.m_ListEffects.TryGetValue(guid, out pkfxFX))
		{
			if (pkfxFX.m_OnFxStopped != null)
			{
				pkfxFX.m_OnFxStopped(pkfxFX);
			}
			pkfxFX.OnFxStopPlaying();
		}
	}

	[MonoPInvokeCallback(typeof(PKFxManager.FxHotReloadCallback))]
	public static void OnFxHotReloaded(int guid, int newGuid)
	{
		PKFxFX pkfxFX;
		if (PKFxFX.m_ListEffects.TryGetValue(guid, out pkfxFX))
		{
			pkfxFX.OnFxHotReloaded(newGuid);
		}
	}

	[MonoPInvokeCallback(typeof(PKFxManager.AudioCallback))]
	public static IntPtr OnAudioSpectrumData(IntPtr channelName, IntPtr nbSamples)
	{
		AudioListener.GetSpectrumData(PKFxManager.m_Samples, 0, FFTWindow.Rectangular);
		PKFxManager.m_Samples[0x3FF] = PKFxManager.m_Samples[0x3FE];
		return PKFxManager.m_SamplesHandle.AddrOfPinnedObject();
	}

	[MonoPInvokeCallback(typeof(PKFxManager.AudioCallback))]
	public static IntPtr OnAudioWaveformData(IntPtr channelName, IntPtr nbSamples)
	{
		AudioListener.GetOutputData(PKFxManager.m_Samples, 0);
		return PKFxManager.m_SamplesHandle.AddrOfPinnedObject();
	}

	public static List<PKFxManager.AttributeDesc> ListEffectAttributesFromGUID(int FxGUID)
	{
		List<PKFxManager.AttributeDesc> list = new List<PKFxManager.AttributeDesc>();
		int num = PKFxManager.EffectAttributesCount(FxGUID);
		for (int i = 0; i < num; i++)
		{
			PKFxManager.S_AttributeDesc desc = default(PKFxManager.S_AttributeDesc);
			desc.Name = Marshal.AllocHGlobal(0x40);
			desc.Description = Marshal.AllocHGlobal(0x80);
			if (PKFxManager.EffectFillAttributeDesc(FxGUID, i, ref desc))
			{
				list.Add(new PKFxManager.AttributeDesc(desc));
			}
			Marshal.FreeHGlobal(desc.Name);
			Marshal.FreeHGlobal(desc.Description);
		}
		return list;
	}

	public static List<PKFxManager.AttributeDesc> ListEffectAttributesFromFx(string name)
	{
		List<PKFxManager.AttributeDesc> list = new List<PKFxManager.AttributeDesc>();
		int num = PKFxManager.EffectAttributesCountFromFx(name);
		for (int i = 0; i < num; i++)
		{
			PKFxManager.S_AttributeDesc desc = default(PKFxManager.S_AttributeDesc);
			desc.Name = Marshal.AllocHGlobal(0x40);
			desc.Description = Marshal.AllocHGlobal(0x80);
			if (PKFxManager.EffectFillAttributeDescFromFx(name, i, ref desc))
			{
				list.Add(new PKFxManager.AttributeDesc(desc));
			}
			Marshal.FreeHGlobal(desc.Name);
			Marshal.FreeHGlobal(desc.Description);
		}
		return list;
	}

	public static List<PKFxManager.SamplerDesc> ListEffectSamplersFromFx(string name)
	{
		List<PKFxManager.SamplerDesc> list = new List<PKFxManager.SamplerDesc>();
		int num = PKFxManager.EffectSamplersCountFromFx(name);
		for (int i = 0; i < num; i++)
		{
			PKFxManager.S_SamplerDesc desc = default(PKFxManager.S_SamplerDesc);
			desc.Name = Marshal.AllocHGlobal(0x40);
			desc.Description = Marshal.AllocHGlobal(0x80);
			if (PKFxManager.EffectFillSamplerDescFromFx(name, i, ref desc))
			{
				list.Add(new PKFxManager.SamplerDesc(desc));
			}
			Marshal.FreeHGlobal(desc.Name);
			Marshal.FreeHGlobal(desc.Description);
		}
		return list;
	}

	public static List<PKFxManager.ShaderConstantDesc> ListShaderConstantsFromName(string name, int count)
	{
		List<PKFxManager.ShaderConstantDesc> list = new List<PKFxManager.ShaderConstantDesc>();
		for (int i = 0; i < count; i++)
		{
			PKFxManager.S_ShaderConstantDesc desc = default(PKFxManager.S_ShaderConstantDesc);
			if (PKFxManager.ShaderFillConstantDesc(name, i, ref desc))
			{
				list.Add(new PKFxManager.ShaderConstantDesc(desc));
			}
		}
		return list;
	}

	public static int CreateEffect(string path, Transform t)
	{
		return PKFxManager.CreateEffect(path, t.localToWorldMatrix);
	}

	public static int CreateEffect(string path, Vector3 position, Quaternion rotation, Vector3 scale)
	{
		Matrix4x4 identity = Matrix4x4.identity;
		identity.SetTRS(position, rotation, scale);
		return PKFxManager.CreateEffect(path, identity);
	}

	public static int CreateEffect(string path, Matrix4x4 m)
	{
		PKFxManager.FxDesc path2;
		path2.Transforms = m;
		path2.FxPath = path;
		return PKFxManager.LoadFx(path2);
	}

	public static bool UpdateTransformEffect(int FxGUID, Transform t)
	{
		Matrix4x4 localToWorldMatrix = t.localToWorldMatrix;
		return PKFxManager.EffectSetTransforms(FxGUID, localToWorldMatrix);
	}

	private static void EnableFileLoggingIFN(bool enable)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				try
				{
					PKFxManager.m_HasFileLogging = enable;
					if (enable)
					{
						if (!File.Exists(PKFxManager.m_LogFilePath))
						{
							FileStream fileStream = File.Create(PKFxManager.m_LogFilePath);
							fileStream.Close();
						}
					}
					if (!enable)
					{
						if (File.Exists(PKFxManager.m_LogFilePath))
						{
							File.Delete(PKFxManager.m_LogFilePath);
						}
					}
				}
				catch
				{
					Debug.LogError("[PKFX] Setting up file logging failed.");
				}
				return;
			}
		}
		PKFxManager.m_HasFileLogging = false;
	}

	public static bool FileLoggingEnabled()
	{
		return PKFxManager.m_HasFileLogging;
	}

	private static void SetupPackInPersistantDataPathIFN(bool enable)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			return;
		}
		if (enable)
		{
			List<string> list = new List<string>();
			list.AddRange(Directory.GetFiles(Application.streamingAssetsPath + "/PackFx", "*", SearchOption.AllDirectories));
			for (int i = 0; i < list.Count; i++)
			{
				list[i] = list[i].Replace("\\", "/");
			}
			list.Sort();
			using (List<string>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string text = enumerator.Current;
					if (Path.GetExtension(text) != ".meta")
					{
						string text2 = text.Substring(Application.streamingAssetsPath.Length);
						FileInfo fileInfo = new FileInfo(text);
						FileInfo fileInfo2 = new FileInfo(Application.persistentDataPath + text2);
						if (!fileInfo2.Exists)
						{
							Debug.Log("Copy " + Application.persistentDataPath + text2);
							if (!Directory.Exists(fileInfo2.Directory.FullName))
							{
								Directory.CreateDirectory(fileInfo2.Directory.FullName);
							}
							File.Copy(text, fileInfo2.FullName);
						}
						else if (fileInfo.LastWriteTime > fileInfo2.LastWriteTime)
						{
							Debug.Log("Overwriting " + Application.persistentDataPath + text2);
							File.Copy(text, fileInfo2.FullName, true);
						}
					}
				}
			}
			PKFxManager.m_PackPath = Application.persistentDataPath;
		}
		else
		{
			PKFxManager.m_PackPath = Application.streamingAssetsPath;
		}
	}

	public static bool PackInPersistantDataPathEnabled()
	{
		return PKFxManager.m_PackPath == Application.persistentDataPath;
	}

	public static bool IsUsingOrthographicProjection()
	{
		return PKFxManager.m_IsUsingOrthographicProjection;
	}

	private static void EnableKillIndividualEffect(bool enable)
	{
		PKFxManager.m_HasSpawnerIDs = enable;
		PKFxManager.EnableSpawnerIDs(enable);
	}

	public static bool KillIndividualEffectEnabled()
	{
		return PKFxManager.m_HasSpawnerIDs;
	}

	public static string GetDefaultShader(int api, int type)
	{
		return Marshal.PtrToStringAnsi(PKFxManager.GetDefaultShaderString(api, type));
	}

	public enum E_AvailableCamEvents
	{
		BeforeImageEffectsOpaque = 0xC,
		BeforeImageEffects = 0x12
	}

	[XmlRoot("PKFxGlobalConf")]
	public class PKFxConf
	{
		public bool enableFileLog = true;

		public bool enableEffectsKill;

		public bool enablePackFxInPersistentDataPath;

		public bool useOrthographicProjection;

		public PKFxManager.E_AvailableCamEvents globalEventSetting = PKFxManager.E_AvailableCamEvents.BeforeImageEffectsOpaque;

		public void Save()
		{
			string path = PKFxManager.m_PackPath + "/PKconfig.cfg";
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(PKFxManager.PKFxConf));
			if (!Directory.Exists(Path.GetDirectoryName(path)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(path));
			}
			FileStream fileStream = new FileStream(path, FileMode.Create);
			using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.ASCII))
			{
				xmlSerializer.Serialize(streamWriter, this);
			}
			fileStream.Close();
		}
	}

	public enum GfxDeviceRenderer
	{
		kGfxRendererOpenGL,
		kGfxRendererD3D9,
		kGfxRendererD3D11,
		kGfxRendererGCM,
		kGfxRendererNull,
		kGfxRendererHollywood,
		kGfxRendererXenon,
		kGfxRendererOpenGLES,
		kGfxRendererOpenGLES20Mobile,
		kGfxRendererMolehill,
		kGfxRendererOpenGLES20Desktop
	}

	public enum GfxDeviceEventType
	{
		kGfxDeviceEventInitialize,
		kGfxDeviceEventShutdown,
		kGfxDeviceEventBeforeReset,
		kGfxDeviceEventAfterReset
	}

	public enum BaseType
	{
		Int = 0x16,
		Int2,
		Int3,
		Int4,
		Float = 0x1C,
		Float2,
		Float3,
		Float4
	}

	public enum DepthGrabFormat
	{
		Depth16Bits = 0x10,
		Depth24Bits = 0x18
	}

	public enum CamFlags
	{
		UseDepthGrabberTexture = 1,
		ScreenResolutionChanged
	}

	public struct CamDesc
	{
		public Matrix4x4 ViewMatrix;

		public Matrix4x4 ProjectionMatrix;

		public float DT;

		public int RenderPass;

		public float NearClip;

		public float FarClip;

		public IntPtr DepthRT;

		public int DepthBpp;

		public float LODBias;

		public int Flags;
	}

	public struct FxDesc
	{
		public string FxPath;

		public Matrix4x4 Transforms;
	}

	public struct ShaderDesc
	{
		public string ShaderPath;

		public string ShaderGroup;

		public int Api;

		public int VertexType;

		public int PixelType;
	}

	public enum EAttrDescFlag : byte
	{
		HasMin = 1,
		HasMax,
		HasDesc = 4
	}

	public struct S_AttributeDesc
	{
		public int Type;

		public char MinMaxFlag;

		public IntPtr Name;

		public IntPtr Description;

		public float DefaultValue0;

		public float DefaultValue1;

		public float DefaultValue2;

		public float DefaultValue3;

		public float MinValue0;

		public float MinValue1;

		public float MinValue2;

		public float MinValue3;

		public float MaxValue0;

		public float MaxValue1;

		public float MaxValue2;

		public float MaxValue3;
	}

	public enum ESamplerType
	{
		SamplerShape,
		SamplerCurve,
		SamplerImage,
		SamplerText,
		SamplerUnsupported
	}

	public enum ETexcoordMode
	{
		Clamp,
		Wrap
	}

	public struct S_SamplerDesc
	{
		public int Type;

		public IntPtr Name;

		public IntPtr Description;
	}

	public struct S_ShaderConstantDesc
	{
		public int Type;

		public IntPtr Name;
	}

	public struct S_Stats
	{
		public float UpdateTime;

		public float RenderTime;

		public int TotalMemoryFootprint;

		public int TotalParticleMemory;

		public int UnusedParticleMemory;
	}

	public struct S_SoundDescriptor
	{
		public int ChannelGroup;

		public IntPtr Path;

		public IntPtr EventStart;

		public IntPtr EventStop;

		public Vector3 WorldPosition;

		public float Volume;

		public float StartTimeOffsetInSeconds;

		public float PlayTimeInSeconds;

		public int UserData;
	}

	[Serializable]
	public class SamplerDesc
	{
		public int Type;

		public string Name;

		public string Description;

		public SamplerDesc(PKFxManager.SamplerDesc desc)
		{
			this.Type = desc.Type;
			this.Name = desc.Name;
		}

		public SamplerDesc(PKFxManager.S_SamplerDesc desc)
		{
			this.Type = desc.Type;
			this.Name = Marshal.PtrToStringAnsi(desc.Name);
			this.Description = Marshal.PtrToStringAnsi(desc.Description);
		}

		public SamplerDesc(string name, int type)
		{
			this.Type = type;
			this.Name = name;
		}
	}

	public enum EShapeType
	{
		BoxShape,
		SphereShape,
		CylinderShape,
		CapsuleShape,
		MeshShape,
		SkinnedMeshShape
	}

	public enum EMeshChannels
	{
		Channel_Position = 1,
		Channel_Normal,
		Channel_Tangent = 4,
		Channel_Velocity = 8,
		Channel_UV = 0x10,
		Channel_VertexColor = 0x20
	}

	public enum EImageFormat
	{
		Invalid,
		BGR8 = 3,
		BGRA8,
		DXT1 = 8,
		DXT3 = 0xA,
		DXT5 = 0xC,
		RGB8_ETC1 = 0x10,
		RGB8_ETC2,
		RGBA8_ETC2,
		RGB8A1_ETC2,
		RGB4_PVRTC1,
		RGB2_PVRTC1,
		RGBA4_PVRTC1,
		RGBA2_PVRTC1
	}

	public class SamplerDescShapeBox
	{
		public Vector3 Center;

		public Vector3 Dimensions;

		public Vector3 EulerOrientation;

		public SamplerDescShapeBox()
		{
			this.Center = new Vector3(0f, 0f, 0f);
			this.Dimensions = new Vector3(1f, 1f, 1f);
			this.EulerOrientation = new Vector3(0f, 0f, 0f);
		}

		public SamplerDescShapeBox(Vector3 center, Vector3 dimension, Vector3 euler)
		{
			this.Center = center;
			this.Dimensions = dimension;
			this.EulerOrientation = euler;
		}
	}

	public class SamplerDescShapeSphere
	{
		public Vector3 Center;

		public float InnerRadius;

		public float Radius;

		public Vector3 EulerOrientation;

		public SamplerDescShapeSphere()
		{
			this.Center = new Vector3(0f, 0f, 0f);
			this.InnerRadius = 1f;
			this.Radius = 1f;
			this.EulerOrientation = new Vector3(0f, 0f, 0f);
		}

		public SamplerDescShapeSphere(Vector3 center, float radius, float innerRadius, Vector3 euler)
		{
			this.Center = center;
			this.InnerRadius = innerRadius;
			this.Radius = radius;
			this.EulerOrientation = euler;
		}
	}

	public class SamplerDescShapeCylinder
	{
		public Vector3 Center;

		public float InnerRadius;

		public float Radius;

		public float Height;

		public Vector3 EulerOrientation;

		public SamplerDescShapeCylinder()
		{
			this.Center = new Vector3(0f, 0f, 0f);
			this.InnerRadius = 1f;
			this.Radius = 1f;
			this.Height = 1f;
			this.EulerOrientation = new Vector3(0f, 0f, 0f);
		}

		public SamplerDescShapeCylinder(Vector3 center, float radius, float innerRadius, float height, Vector3 euler)
		{
			this.Center = center;
			this.InnerRadius = innerRadius;
			this.Radius = radius;
			this.Height = height;
			this.EulerOrientation = euler;
		}
	}

	public class SamplerDescShapeCapsule
	{
		public Vector3 Center;

		public float InnerRadius;

		public float Radius;

		public float Height;

		public Vector3 EulerOrientation;

		public SamplerDescShapeCapsule()
		{
			this.Center = new Vector3(0f, 0f, 0f);
			this.InnerRadius = 1f;
			this.Radius = 1f;
			this.Height = 1f;
			this.EulerOrientation = new Vector3(0f, 0f, 0f);
		}

		public SamplerDescShapeCapsule(Vector3 center, float radius, float innerRadius, float height, Vector3 euler)
		{
			this.Center = center;
			this.InnerRadius = innerRadius;
			this.Radius = radius;
			this.Height = height;
			this.EulerOrientation = euler;
		}
	}

	public class SamplerDescShapeMesh
	{
		public Vector3 Center;

		public Vector3 Dimensions;

		public Vector3 EulerOrientation;

		public Mesh Mesh;

		public int SamplingChannels;

		public SamplerDescShapeMesh()
		{
			this.Center = new Vector3(0f, 0f, 0f);
			this.Dimensions = new Vector3(1f, 1f, 1f);
			this.EulerOrientation = new Vector3(0f, 0f, 0f);
			this.SamplingChannels |= 1;
		}

		public SamplerDescShapeMesh(Vector3 center, Vector3 dimension, Vector3 euler, Mesh mesh, int samplingChannels)
		{
			this.Center = center;
			this.Dimensions = dimension;
			this.EulerOrientation = euler;
			this.Mesh = mesh;
			this.SamplingChannels = samplingChannels;
		}
	}

	public class SamplerDescShapeMeshFilter
	{
		public Vector3 Center;

		public Vector3 Dimensions;

		public Vector3 EulerOrientation;

		public MeshFilter MeshFilter;

		public int SamplingChannels;

		public SamplerDescShapeMeshFilter()
		{
			this.Center = new Vector3(0f, 0f, 0f);
			this.Dimensions = new Vector3(1f, 1f, 1f);
			this.EulerOrientation = new Vector3(0f, 0f, 0f);
			this.SamplingChannels |= 1;
		}

		public SamplerDescShapeMeshFilter(Vector3 center, Vector3 dimension, Vector3 euler, MeshFilter mesh, int samplingChannels)
		{
			this.Center = center;
			this.Dimensions = dimension;
			this.EulerOrientation = euler;
			this.MeshFilter = mesh;
			this.SamplingChannels = samplingChannels;
		}
	}

	public class SamplerDescShapeSkinnedMesh
	{
		public Vector3 Center;

		public Vector3 Dimensions;

		public Vector3 EulerOrientation;

		public SkinnedMeshRenderer SkinnedMesh;

		public int SamplingChannels;

		public SamplerDescShapeSkinnedMesh()
		{
			this.Center = new Vector3(0f, 0f, 0f);
			this.Dimensions = new Vector3(1f, 1f, 1f);
			this.EulerOrientation = new Vector3(0f, 0f, 0f);
			this.SamplingChannels |= 1;
		}

		public SamplerDescShapeSkinnedMesh(Vector3 center, Vector3 dimension, Vector3 euler, SkinnedMeshRenderer skinnedMesh, int samplingChannels)
		{
			this.Center = center;
			this.Dimensions = dimension;
			this.EulerOrientation = euler;
			this.SkinnedMesh = skinnedMesh;
			this.SamplingChannels = samplingChannels;
		}
	}

	[Serializable]
	public class Sampler
	{
		public PKFxManager.SamplerDesc m_Descriptor;

		public int m_ShapeType;

		public int m_EditorShapeType;

		public MeshFilter m_MeshFilter;

		public Mesh m_Mesh;

		public SkinnedMeshRenderer m_SkinnedMeshRenderer;

		public int m_MeshHashCode;

		public int m_SamplingChannels;

		public Vector3 m_ShapeCenter = Vector3.zero;

		public Vector3 m_Dimensions = Vector3.one;

		public Vector3 m_EulerOrientation = Vector3.zero;

		public Texture2D m_Texture;

		public bool m_TextureChanged;

		public PKFxManager.ETexcoordMode m_TextureTexcoordMode;

		public AnimationCurve[] m_CurvesArray;

		public float[] m_CurvesTimeKeys;

		public string m_Text = string.Empty;

		public PKFxManager.Sampler.SkinnedMeshData m_SkinnedMeshData;

		public Sampler(PKFxManager.SamplerDesc dsc)
		{
			this.m_Descriptor = new PKFxManager.SamplerDesc(dsc);
			this.m_ShapeType = -1;
			this.m_EditorShapeType = -1;
		}

		public Sampler(string name, PKFxManager.SamplerDescShapeBox dsc)
		{
			this.m_Descriptor = new PKFxManager.SamplerDesc(name, 0);
			this.m_ShapeCenter = dsc.Center;
			this.m_Dimensions = dsc.Dimensions;
			this.m_EulerOrientation = dsc.EulerOrientation;
			this.m_ShapeType = 0;
			this.m_EditorShapeType = 0;
		}

		public Sampler(string name, PKFxManager.SamplerDescShapeSphere dsc)
		{
			this.m_Descriptor = new PKFxManager.SamplerDesc(name, 0);
			this.m_ShapeCenter = dsc.Center;
			this.m_Dimensions = new Vector3(dsc.Radius, dsc.InnerRadius);
			this.m_Dimensions.y = Mathf.Min(this.m_Dimensions.x, this.m_Dimensions.y);
			this.m_Dimensions.x = Mathf.Max(this.m_Dimensions.x, this.m_Dimensions.y);
			this.m_EulerOrientation = dsc.EulerOrientation;
			this.m_ShapeType = 1;
			this.m_EditorShapeType = 1;
		}

		public Sampler(string name, PKFxManager.SamplerDescShapeCylinder dsc)
		{
			this.m_Descriptor = new PKFxManager.SamplerDesc(name, 0);
			this.m_ShapeCenter = dsc.Center;
			this.m_Dimensions = new Vector3(dsc.Radius, dsc.InnerRadius, dsc.Height);
			this.m_Dimensions.y = Mathf.Min(this.m_Dimensions.x, this.m_Dimensions.y);
			this.m_Dimensions.x = Mathf.Max(this.m_Dimensions.x, this.m_Dimensions.y);
			this.m_EulerOrientation = dsc.EulerOrientation;
			this.m_ShapeType = 2;
			this.m_EditorShapeType = 2;
		}

		public Sampler(string name, PKFxManager.SamplerDescShapeCapsule dsc)
		{
			this.m_Descriptor = new PKFxManager.SamplerDesc(name, 0);
			this.m_ShapeCenter = dsc.Center;
			this.m_Dimensions = new Vector3(dsc.Radius, dsc.InnerRadius, dsc.Height);
			this.m_Dimensions.y = Mathf.Min(this.m_Dimensions.x, this.m_Dimensions.y);
			this.m_Dimensions.x = Mathf.Max(this.m_Dimensions.x, this.m_Dimensions.y);
			this.m_EulerOrientation = dsc.EulerOrientation;
			this.m_ShapeType = 3;
			this.m_EditorShapeType = 3;
		}

		public Sampler(string name, PKFxManager.SamplerDescShapeMesh dsc)
		{
			this.m_Descriptor = new PKFxManager.SamplerDesc(name, 0);
			this.m_ShapeCenter = dsc.Center;
			this.m_Dimensions = dsc.Dimensions;
			this.m_EulerOrientation = dsc.EulerOrientation;
			this.m_Mesh = dsc.Mesh;
			this.m_MeshFilter = null;
			this.m_SkinnedMeshRenderer = null;
			if (this.m_Mesh != null)
			{
				this.m_MeshHashCode = this.m_Mesh.name.GetHashCode();
			}
			else
			{
				this.m_MeshHashCode = 0;
			}
			this.m_SkinnedMeshRenderer = null;
			this.m_SamplingChannels = dsc.SamplingChannels;
			this.m_ShapeType = 4;
			this.m_EditorShapeType = 4;
		}

		public Sampler(string name, PKFxManager.SamplerDescShapeMeshFilter dsc)
		{
			this.m_Descriptor = new PKFxManager.SamplerDesc(name, 0);
			this.m_ShapeCenter = dsc.Center;
			this.m_Dimensions = dsc.Dimensions;
			this.m_EulerOrientation = dsc.EulerOrientation;
			this.m_MeshFilter = dsc.MeshFilter;
			this.m_Mesh = this.m_MeshFilter.sharedMesh;
			this.m_SkinnedMeshRenderer = null;
			if (this.m_Mesh != null)
			{
				this.m_MeshHashCode = this.m_Mesh.name.GetHashCode();
			}
			else
			{
				this.m_MeshHashCode = 0;
			}
			this.m_SamplingChannels = dsc.SamplingChannels;
			this.m_ShapeType = 4;
			this.m_EditorShapeType = 5;
		}

		public Sampler(string name, PKFxManager.SamplerDescShapeSkinnedMesh dsc)
		{
			this.m_Descriptor = new PKFxManager.SamplerDesc(name, 0);
			this.m_ShapeCenter = dsc.Center;
			this.m_Dimensions = dsc.Dimensions;
			this.m_EulerOrientation = dsc.EulerOrientation;
			this.m_SkinnedMeshRenderer = dsc.SkinnedMesh;
			this.m_Mesh = dsc.SkinnedMesh.sharedMesh;
			this.m_MeshFilter = null;
			if (this.m_Mesh != null)
			{
				this.m_MeshHashCode = this.m_Mesh.name.GetHashCode();
			}
			else
			{
				this.m_MeshHashCode = 0;
			}
			this.m_SamplingChannels = dsc.SamplingChannels;
			this.m_ShapeType = 5;
			this.m_EditorShapeType = 6;
		}

		public Sampler(string name, AnimationCurve[] curvesArray)
		{
			this.m_Descriptor = new PKFxManager.SamplerDesc(name, 1);
			this.m_CurvesArray = curvesArray;
			if (this.m_CurvesArray.Length != 0)
			{
				int num = 0;
				this.m_CurvesTimeKeys = new float[this.m_CurvesArray[0].keys.Length];
				foreach (Keyframe keyframe in this.m_CurvesArray[0].keys)
				{
					this.m_CurvesTimeKeys[num++] = keyframe.time;
				}
			}
			this.m_ShapeType = -1;
			this.m_EditorShapeType = -1;
		}

		public Sampler(string name, Texture2D texture, PKFxManager.ETexcoordMode texcoordMode)
		{
			this.m_Descriptor = new PKFxManager.SamplerDesc(name, 2);
			this.m_Texture = texture;
			this.m_TextureChanged = true;
			this.m_TextureTexcoordMode = texcoordMode;
			this.m_ShapeType = -1;
			this.m_EditorShapeType = -1;
		}

		public Sampler(string name, string text)
		{
			this.m_Descriptor = new PKFxManager.SamplerDesc(name, 3);
			this.m_Text = text;
			this.m_ShapeType = -1;
			this.m_EditorShapeType = -1;
		}

		public void Copy(PKFxManager.Sampler other)
		{
			this.m_ShapeType = other.m_ShapeType;
			this.m_EditorShapeType = other.m_EditorShapeType;
			this.m_Mesh = other.m_Mesh;
			this.m_SkinnedMeshRenderer = other.m_SkinnedMeshRenderer;
			this.m_MeshHashCode = other.m_MeshHashCode;
			this.m_SamplingChannels = other.m_SamplingChannels;
			this.m_ShapeCenter = other.m_ShapeCenter;
			this.m_Dimensions = other.m_Dimensions;
			this.m_EulerOrientation = other.m_EulerOrientation;
			this.m_Texture = other.m_Texture;
			this.m_TextureChanged = other.m_TextureChanged;
			this.m_TextureTexcoordMode = other.m_TextureTexcoordMode;
			this.m_CurvesArray = other.m_CurvesArray;
			this.m_CurvesTimeKeys = other.m_CurvesTimeKeys;
			this.m_Text = other.m_Text;
			this.m_SkinnedMeshData = other.m_SkinnedMeshData;
		}

		public class SkinnedMeshData
		{
			public float[] m_SkeletonDataBuffer;

			public Matrix4x4[] m_Bindposes;

			public void InitData(SkinnedMeshRenderer skinnedMeshRenderer)
			{
				this.m_SkeletonDataBuffer = new float[skinnedMeshRenderer.bones.Length * 0x10];
				this.m_Bindposes = skinnedMeshRenderer.sharedMesh.bindposes;
			}
		}
	}

	[Serializable]
	public class AttributeDesc
	{
		public PKFxManager.BaseType Type;

		public int MinMaxFlag;

		public string Name;

		public string Description;

		public float DefaultValue0;

		public float DefaultValue1;

		public float DefaultValue2;

		public float DefaultValue3;

		public float MinValue0;

		public float MinValue1;

		public float MinValue2;

		public float MinValue3;

		public float MaxValue0;

		public float MaxValue1;

		public float MaxValue2;

		public float MaxValue3;

		public AttributeDesc(PKFxManager.S_AttributeDesc desc)
		{
			this.Type = (PKFxManager.BaseType)desc.Type;
			this.MinMaxFlag = (int)desc.MinMaxFlag;
			this.Name = Marshal.PtrToStringAnsi(desc.Name);
			this.Description = Marshal.PtrToStringAnsi(desc.Description);
			this.DefaultValue0 = desc.DefaultValue0;
			this.DefaultValue1 = desc.DefaultValue1;
			this.DefaultValue2 = desc.DefaultValue2;
			this.DefaultValue3 = desc.DefaultValue3;
			this.MinValue0 = desc.MinValue0;
			this.MinValue1 = desc.MinValue1;
			this.MinValue2 = desc.MinValue2;
			this.MinValue3 = desc.MinValue3;
			this.MaxValue0 = desc.MaxValue0;
			this.MaxValue1 = desc.MaxValue1;
			this.MaxValue2 = desc.MaxValue2;
			this.MaxValue3 = desc.MaxValue3;
		}

		public AttributeDesc(PKFxManager.BaseType type, IntPtr name)
		{
			this.Type = type;
			this.Name = Marshal.PtrToStringAnsi(name);
		}

		public AttributeDesc(PKFxManager.BaseType type, string name)
		{
			this.Type = type;
			this.Name = name;
		}
	}

	[Serializable]
	public class Attribute
	{
		public PKFxManager.AttributeDesc m_Descriptor;

		public float m_Value0;

		public float m_Value1;

		public float m_Value2;

		public float m_Value3;

		public Attribute(PKFxManager.S_AttributeDesc desc)
		{
			this.m_Descriptor = new PKFxManager.AttributeDesc(desc);
			this.m_Value0 = desc.DefaultValue0;
			this.m_Value1 = desc.DefaultValue1;
			this.m_Value2 = desc.DefaultValue2;
			this.m_Value3 = desc.DefaultValue3;
		}

		public Attribute(PKFxManager.AttributeDesc desc)
		{
			this.m_Descriptor = desc;
			this.m_Value0 = desc.DefaultValue0;
			this.m_Value1 = desc.DefaultValue1;
			this.m_Value2 = desc.DefaultValue2;
			this.m_Value3 = desc.DefaultValue3;
		}

		public Attribute(string name, float val)
		{
			this.m_Descriptor = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Float, name);
			this.ValueFloat = val;
		}

		public Attribute(string name, Vector2 val)
		{
			this.m_Descriptor = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Float2, name);
			this.ValueFloat2 = val;
		}

		public Attribute(string name, Vector3 val)
		{
			this.m_Descriptor = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Float3, name);
			this.ValueFloat3 = val;
		}

		public Attribute(string name, Vector4 val)
		{
			this.m_Descriptor = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Float4, name);
			this.ValueFloat4 = val;
		}

		public Attribute(string name, int val)
		{
			this.m_Descriptor = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Int, name);
			this.ValueInt = val;
		}

		public Attribute(string name, int[] val)
		{
			if (val.Length >= 1)
			{
				this.m_Descriptor = new PKFxManager.AttributeDesc(PKFxManager.BaseType.Int + val.Length - 1, name);
				this.m_Value0 = PKFxManager.itof(val[0]);
			}
			if (val.Length >= 2)
			{
				this.m_Value1 = PKFxManager.itof(val[1]);
			}
			if (val.Length >= 3)
			{
				this.m_Value2 = PKFxManager.itof(val[2]);
			}
			if (val.Length >= 4)
			{
				this.m_Value3 = PKFxManager.itof(val[3]);
			}
		}

		public float ValueFloat
		{
			get
			{
				return this.m_Value0;
			}
			set
			{
				this.m_Value0 = value;
			}
		}

		public Vector2 ValueFloat2
		{
			get
			{
				return new Vector2(this.m_Value0, this.m_Value1);
			}
			set
			{
				this.m_Value0 = value.x;
				this.m_Value1 = value.y;
			}
		}

		public Vector3 ValueFloat3
		{
			get
			{
				return new Vector3(this.m_Value0, this.m_Value1, this.m_Value2);
			}
			set
			{
				this.m_Value0 = value.x;
				this.m_Value1 = value.y;
				this.m_Value2 = value.z;
			}
		}

		public Vector4 ValueFloat4
		{
			get
			{
				return new Vector4(this.m_Value0, this.m_Value1, this.m_Value2, this.m_Value3);
			}
			set
			{
				this.m_Value0 = value.x;
				this.m_Value1 = value.y;
				this.m_Value2 = value.z;
				this.m_Value3 = value.w;
			}
		}

		public int ValueInt
		{
			get
			{
				return PKFxManager.ftoi(this.m_Value0);
			}
			set
			{
				this.m_Value0 = PKFxManager.itof(value);
			}
		}

		public int[] ValueInt2
		{
			get
			{
				return new int[]
				{
					PKFxManager.ftoi(this.m_Value0),
					PKFxManager.ftoi(this.m_Value1)
				};
			}
			set
			{
				this.m_Value0 = PKFxManager.itof(value[0]);
				this.m_Value1 = PKFxManager.itof(value[1]);
			}
		}

		public int[] ValueInt3
		{
			get
			{
				return new int[]
				{
					PKFxManager.ftoi(this.m_Value0),
					PKFxManager.ftoi(this.m_Value1),
					PKFxManager.ftoi(this.m_Value2)
				};
			}
			set
			{
				this.m_Value0 = PKFxManager.itof(value[0]);
				this.m_Value1 = PKFxManager.itof(value[1]);
				this.m_Value2 = PKFxManager.itof(value[2]);
			}
		}

		public int[] ValueInt4
		{
			get
			{
				return new int[]
				{
					PKFxManager.ftoi(this.m_Value0),
					PKFxManager.ftoi(this.m_Value1),
					PKFxManager.ftoi(this.m_Value2),
					PKFxManager.ftoi(this.m_Value3)
				};
			}
			set
			{
				this.m_Value0 = PKFxManager.itof(value[0]);
				this.m_Value1 = PKFxManager.itof(value[1]);
				this.m_Value2 = PKFxManager.itof(value[2]);
				this.m_Value3 = PKFxManager.itof(value[3]);
			}
		}
	}

	[Serializable]
	public class ShaderConstantDesc
	{
		public PKFxManager.BaseType Type;

		public string Name;

		public int MinMaxFlag;

		public string Description;

		public ShaderConstantDesc(PKFxManager.S_ShaderConstantDesc desc)
		{
			this.Type = (PKFxManager.BaseType)desc.Type;
			this.Name = Marshal.PtrToStringAnsi(desc.Name);
			this.MinMaxFlag = 0;
		}

		public ShaderConstantDesc(PKFxManager.BaseType type, IntPtr name)
		{
			this.Type = type;
			this.Name = Marshal.PtrToStringAnsi(name);
			this.MinMaxFlag = 0;
		}

		public ShaderConstantDesc(PKFxManager.BaseType type, string name)
		{
			this.Type = type;
			this.Name = name;
			this.MinMaxFlag = 0;
		}
	}

	[Serializable]
	public class ShaderConstant
	{
		public PKFxManager.ShaderConstantDesc m_Descriptor;

		public float m_Value0;

		public float m_Value1;

		public float m_Value2;

		public float m_Value3;

		public ShaderConstant(PKFxManager.S_ShaderConstantDesc desc)
		{
			this.m_Descriptor = new PKFxManager.ShaderConstantDesc(desc);
			this.m_Value0 = 0f;
			this.m_Value1 = 0f;
			this.m_Value2 = 0f;
			this.m_Value3 = 0f;
		}

		public ShaderConstant(PKFxManager.ShaderConstantDesc desc)
		{
			this.m_Descriptor = desc;
			this.m_Value0 = 0f;
			this.m_Value1 = 0f;
			this.m_Value2 = 0f;
			this.m_Value3 = 0f;
		}

		public ShaderConstant(string name, float val)
		{
			this.m_Descriptor = new PKFxManager.ShaderConstantDesc(PKFxManager.BaseType.Float, name);
			this.ValueFloat = val;
		}

		public ShaderConstant(string name, Vector2 val)
		{
			this.m_Descriptor = new PKFxManager.ShaderConstantDesc(PKFxManager.BaseType.Float2, name);
			this.ValueFloat2 = val;
		}

		public ShaderConstant(string name, Vector3 val)
		{
			this.m_Descriptor = new PKFxManager.ShaderConstantDesc(PKFxManager.BaseType.Float3, name);
			this.ValueFloat3 = val;
		}

		public ShaderConstant(string name, Vector4 val)
		{
			this.m_Descriptor = new PKFxManager.ShaderConstantDesc(PKFxManager.BaseType.Float4, name);
			this.ValueFloat4 = val;
		}

		public float ValueFloat
		{
			get
			{
				return this.m_Value0;
			}
			set
			{
				this.m_Value0 = value;
			}
		}

		public Vector2 ValueFloat2
		{
			get
			{
				return new Vector2(this.m_Value0, this.m_Value1);
			}
			set
			{
				this.m_Value0 = value.x;
				this.m_Value1 = value.y;
			}
		}

		public Vector3 ValueFloat3
		{
			get
			{
				return new Vector3(this.m_Value0, this.m_Value1, this.m_Value2);
			}
			set
			{
				this.m_Value0 = value.x;
				this.m_Value1 = value.y;
				this.m_Value2 = value.z;
			}
		}

		public Vector4 ValueFloat4
		{
			get
			{
				return new Vector4(this.m_Value0, this.m_Value1, this.m_Value2, this.m_Value3);
			}
			set
			{
				this.m_Value0 = value.x;
				this.m_Value1 = value.y;
				this.m_Value2 = value.z;
				this.m_Value3 = value.w;
			}
		}
	}

	[Serializable]
	public class SoundDescriptor
	{
		public int ChannelGroup;

		public string Path;

		public string EventStart;

		public string EventStop;

		public Vector3 WorldPosition;

		public float Volume;

		public float StartTimeOffsetInSeconds;

		public float PlayTimeInSeconds;

		public int UserData;

		public SoundDescriptor(PKFxManager.S_SoundDescriptor desc)
		{
			this.ChannelGroup = desc.ChannelGroup;
			this.Path = Marshal.PtrToStringAnsi(desc.Path);
			this.EventStart = Marshal.PtrToStringAnsi(desc.EventStart);
			this.EventStop = Marshal.PtrToStringAnsi(desc.EventStop);
			this.WorldPosition = desc.WorldPosition;
			this.Volume = desc.Volume;
			this.StartTimeOffsetInSeconds = desc.StartTimeOffsetInSeconds;
			this.PlayTimeInSeconds = desc.PlayTimeInSeconds;
			this.UserData = desc.UserData;
		}
	}

	private delegate void FxCallback(int guid);

	private delegate void FxHotReloadCallback(int guid, int newGuid);

	public delegate IntPtr AudioCallback(IntPtr channelName, IntPtr nbSamples);
}
