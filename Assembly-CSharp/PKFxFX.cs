using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PKFxFX : PKFxPackDependent
{
	private struct SAttributePinned
	{
		public int m_Type;

		public float m_Value0;

		public float m_Value1;

		public float m_Value2;

		public float m_Value3;
	}

	private struct SSamplerPinned
	{
		public int m_Type1;

		public int m_Type2;

		public float m_SizeX;

		public float m_SizeY;

		public float m_SizeZ;

		public float m_PosX;

		public float m_PosY;

		public float m_PosZ;

		public float m_EulX;

		public float m_EulY;

		public float m_EulZ;

		public int m_MeshChanged;

		public int m_HashCode;

		public int m_IndexCount;

		public int m_VertexCount;

		public int m_BoneCount;

		public int m_SamplingChannels;

		public IntPtr m_Data;
	}

	public delegate void OnFxStoppedDelegate(PKFxFX component);

	private static class PKImageConverter
	{
		public static void ARGB2BGRA(ref byte[] data)
		{
			int num = 0;
			while (num < data.Length)
			{
				byte[] array = new byte[4]
				{
					data[num + 3],
					data[num + 2],
					data[num + 1],
					data[num]
				};
				data[num++] = array[0];
				data[num++] = array[1];
				data[num++] = array[2];
				data[num++] = array[3];
			}
		}

		public static void RGBA2BGRA(ref byte[] data)
		{
			for (int i = 0; i < data.Length; i += 4)
			{
				byte b = data[i];
				data[i] = data[i + 2];
				data[i + 2] = b;
			}
			while (true)
			{
				return;
			}
		}

		public static void RGB2BGR(ref byte[] data)
		{
			for (int i = 0; i < data.Length; i += 3)
			{
				byte b = data[i];
				data[i] = data[i + 2];
				data[i + 2] = b;
			}
		}
	}

	private bool m_IsStopped;

	private int m_FXGUID = -1;

	private bool m_AskedToStart;

	public List<PKFxManager.Attribute> m_FxAttributesList;

	public List<PKFxManager.Sampler> m_FxSamplersList;

	public bool m_PlayOnStart = true;

	public bool m_IsPlaying;

	public static Dictionary<int, PKFxFX> m_ListEffects = new Dictionary<int, PKFxFX>();

	private bool m_ForceUpdateAttributes;

	private SAttributePinned[] m_AttributesCache;

	private GCHandle m_AttributesGCH;

	private IntPtr m_AttributesHandler;

	private SSamplerPinned[] m_SamplersCache;

	private GCHandle m_SamplersGCH;

	private IntPtr m_SamplersHandler;

	private float[] m_SamplersCurvesDataCache;

	private GCHandle m_SamplersCurvesDataGCH;

	private IntPtr m_SamplersCurvesDataHandler;

	public OnFxStoppedDelegate m_OnFxStopped;

	public UnityEngine.Object m_BoundFx;

	public string m_FxName;

	public int FXGUID => m_FXGUID;

	public string FxPath => m_FxName;

	private void Awake()
	{
		if (m_FxAttributesList == null)
		{
			m_FxAttributesList = new List<PKFxManager.Attribute>();
		}
		if (m_FxSamplersList == null)
		{
			m_FxSamplersList = new List<PKFxManager.Sampler>();
		}
		m_IsPlaying = false;
	}

	private IEnumerator Start()
	{
		base.BaseInitialize();
		yield return WaitForPack(true);
		/*Error: Unable to find new state assignment for yield return*/;
	}

	private void LateUpdate()
	{
		if (!m_IsPlaying)
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
		PKFxManager.UpdateTransformEffect(m_FXGUID, base.transform);
		UpdateAttributes(false);
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawIcon(base.transform.position, "FX.png", true);
	}

	private void OnDestroy()
	{
		KillEffect();
		StopEffect();
		for (int i = 0; i < m_FxSamplersList.Count; i++)
		{
			if (m_SamplersCache == null || !(m_SamplersCache[i].m_Data != IntPtr.Zero))
			{
				continue;
			}
			if (m_SamplersCache[i].m_Type1 != 1)
			{
				Marshal.FreeHGlobal(m_SamplersCache[i].m_Data);
			}
		}
		m_SamplersCache = null;
		if (m_AttributesGCH.IsAllocated)
		{
			m_AttributesGCH.Free();
		}
		if (m_SamplersGCH.IsAllocated)
		{
			m_SamplersGCH.Free();
		}
		if (!m_SamplersCurvesDataGCH.IsAllocated)
		{
			return;
		}
		while (true)
		{
			m_SamplersCurvesDataGCH.Free();
			return;
		}
	}

	public void StartEffect()
	{
		if (!PKFxManager.m_PackLoaded)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					m_AskedToStart = true;
					return;
				}
			}
		}
		if (m_IsStopped)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[PKFX] Attempt to start an effect while the stopped effect is still running.");
					return;
				}
			}
		}
		if (m_IsPlaying)
		{
			while (true)
			{
				switch (6)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		if (!string.IsNullOrEmpty(m_FxName))
		{
			m_FXGUID = PKFxManager.CreateEffect(FxPath, base.transform);
		}
		m_IsPlaying = (m_FXGUID != -1);
		if (m_FXGUID != -1)
		{
			if (m_ListEffects.ContainsKey(m_FXGUID))
			{
				m_ListEffects[m_FXGUID] = this;
			}
			else
			{
				m_ListEffects.Add(m_FXGUID, this);
			}
		}
		LoadAttributes(PKFxManager.ListEffectAttributesFromGUID(m_FXGUID), false);
		LoadSamplers(PKFxManager.ListEffectSamplersFromFx(m_FxName), false);
		UpdateAttributes(true);
	}

	public void TerminateEffect()
	{
		if (!m_IsPlaying)
		{
			return;
		}
		if (m_FXGUID == -1)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		PKFxManager.TerminateFx(m_FXGUID);
		m_IsPlaying = false;
	}

	public void StopEffect()
	{
		if (!m_IsPlaying)
		{
			return;
		}
		while (true)
		{
			if (m_FXGUID == -1)
			{
				while (true)
				{
					switch (6)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			if (PKFxManager.StopFx(m_FXGUID))
			{
				while (true)
				{
					m_IsStopped = true;
					return;
				}
			}
			return;
		}
	}

	public void OnFxStopPlaying()
	{
		m_IsPlaying = false;
		m_IsStopped = false;
	}

	public void KillEffect()
	{
		if (!m_IsPlaying)
		{
			return;
		}
		while (true)
		{
			if (m_FXGUID != -1 && PKFxManager.KillIndividualEffectEnabled())
			{
				PKFxManager.KillFx(m_FXGUID);
				m_IsPlaying = false;
			}
			return;
		}
	}

	public bool IsPlayable()
	{
		int result;
		if (!m_IsPlaying)
		{
			result = ((m_FXGUID != -1) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public bool IsPlaying()
	{
		return m_IsPlaying;
	}

	public bool Alive()
	{
		if (!m_IsPlaying || m_FXGUID == -1)
		{
			return false;
		}
		return PKFxManager.IsFxAlive(m_FXGUID);
	}

	public void OnFxHotReloaded(int newGuid)
	{
		if (newGuid == -1)
		{
			return;
		}
		while (true)
		{
			if (newGuid != m_FXGUID)
			{
				m_ListEffects.Remove(m_FXGUID);
				m_FXGUID = newGuid;
				m_ListEffects.Add(m_FXGUID, this);
			}
			m_ForceUpdateAttributes = true;
			m_IsPlaying = true;
			return;
		}
	}

	public PKFxManager.Attribute GetAttribute(string name)
	{
		using (List<PKFxManager.Attribute>.Enumerator enumerator = m_FxAttributesList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.Attribute current = enumerator.Current;
				if (current.m_Descriptor.Name == name)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							return current;
						}
					}
				}
			}
		}
		return null;
	}

	public PKFxManager.Attribute GetAttribute(string name, PKFxManager.BaseType type)
	{
		using (List<PKFxManager.Attribute>.Enumerator enumerator = m_FxAttributesList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.Attribute current = enumerator.Current;
				if (current.m_Descriptor.Name == name)
				{
					if (current.m_Descriptor.Type == type)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								return current;
							}
						}
					}
				}
			}
		}
		return null;
	}

	public void SetAttribute(PKFxManager.Attribute attr)
	{
		if (!AttributeExists(attr.m_Descriptor))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Debug.LogError("[PKFX] FX.SetAttribute : " + attr.m_Descriptor.Name + " doesn't exist");
					return;
				}
			}
		}
		for (int i = 0; i < m_FxAttributesList.Count; i++)
		{
			if (m_FxAttributesList[i].m_Descriptor.Name == attr.m_Descriptor.Name)
			{
				m_FxAttributesList[i].m_Value0 = attr.m_Value0;
				m_FxAttributesList[i].m_Value1 = attr.m_Value1;
				m_FxAttributesList[i].m_Value2 = attr.m_Value2;
				m_FxAttributesList[i].m_Value3 = attr.m_Value3;
			}
		}
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public PKFxManager.Sampler GetSampler(string name)
	{
		using (List<PKFxManager.Sampler>.Enumerator enumerator = m_FxSamplersList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.Sampler current = enumerator.Current;
				if (current.m_Descriptor.Name == name)
				{
					return current;
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					goto end_IL_000c;
				}
			}
			end_IL_000c:;
		}
		return null;
	}

	public PKFxManager.Sampler GetSampler(string name, PKFxManager.ESamplerType type)
	{
		using (List<PKFxManager.Sampler>.Enumerator enumerator = m_FxSamplersList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.Sampler current = enumerator.Current;
				if (current.m_Descriptor.Name == name && current.m_Descriptor.Type == (int)type)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							return current;
						}
					}
				}
			}
		}
		return null;
	}

	public void SetSampler(PKFxManager.Sampler sampler)
	{
		if (!SamplerExists(sampler.m_Descriptor))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Debug.LogError("[PKFX] FX.SetSampler : " + sampler.m_Descriptor.Name + " doesn't exist");
					return;
				}
			}
		}
		for (int i = 0; i < m_FxSamplersList.Count; i++)
		{
			if (m_FxSamplersList[i].m_Descriptor.Name == sampler.m_Descriptor.Name)
			{
				if (m_FxSamplersList[i].m_Descriptor.Type == sampler.m_Descriptor.Type)
				{
					m_FxSamplersList[i].Copy(sampler);
				}
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

	public void LoadSamplers(List<PKFxManager.SamplerDesc> FxSamplersDesc, bool flushAttributes)
	{
		if (flushAttributes)
		{
			m_FxSamplersList.Clear();
		}
		List<PKFxManager.Sampler> list = new List<PKFxManager.Sampler>();
		using (List<PKFxManager.SamplerDesc>.Enumerator enumerator = FxSamplersDesc.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.SamplerDesc current = enumerator.Current;
				if (!SamplerExists(current))
				{
					list.Add(new PKFxManager.Sampler(current));
				}
				else
				{
					list.Add(GetSamplerFromDesc(current));
				}
			}
		}
		m_FxSamplersList = list;
	}

	public void ResetAttributesToDefault(List<PKFxManager.AttributeDesc> FxAttributesDesc)
	{
		m_FxAttributesList.Clear();
		List<PKFxManager.Attribute> list = new List<PKFxManager.Attribute>();
		using (List<PKFxManager.AttributeDesc>.Enumerator enumerator = FxAttributesDesc.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.AttributeDesc current = enumerator.Current;
				list.Add(new PKFxManager.Attribute(current));
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					goto end_IL_001a;
				}
			}
			end_IL_001a:;
		}
		m_FxAttributesList = list;
	}

	public void LoadAttributes(List<PKFxManager.AttributeDesc> FxAttributesDesc, bool flushAttributes)
	{
		if (flushAttributes)
		{
			m_FxAttributesList.Clear();
		}
		List<PKFxManager.Attribute> list = new List<PKFxManager.Attribute>();
		using (List<PKFxManager.AttributeDesc>.Enumerator enumerator = FxAttributesDesc.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.AttributeDesc current = enumerator.Current;
				if (!AttributeExists(current))
				{
					list.Add(new PKFxManager.Attribute(current));
				}
				else
				{
					list.Add(GetAttributeFromDesc(current));
				}
			}
		}
		m_FxAttributesList = list;
	}

	private void AllocAttributesCacheIFN()
	{
		if (m_AttributesCache != null)
		{
			if (m_AttributesCache.Length >= m_FxAttributesList.Count)
			{
				goto IL_0091;
			}
		}
		m_AttributesCache = new SAttributePinned[m_FxAttributesList.Count];
		if (m_AttributesGCH.IsAllocated)
		{
			m_AttributesGCH.Free();
		}
		m_AttributesGCH = GCHandle.Alloc(m_AttributesCache, GCHandleType.Pinned);
		m_AttributesHandler = m_AttributesGCH.AddrOfPinnedObject();
		goto IL_0091;
		IL_0091:
		if (m_SamplersCache != null)
		{
			if (m_SamplersCache.Length >= m_FxSamplersList.Count)
			{
				return;
			}
		}
		m_SamplersCache = new SSamplerPinned[m_FxSamplersList.Count];
		if (m_SamplersGCH.IsAllocated)
		{
			m_SamplersGCH.Free();
		}
		m_SamplersGCH = GCHandle.Alloc(m_SamplersCache, GCHandleType.Pinned);
		m_SamplersHandler = m_SamplersGCH.AddrOfPinnedObject();
	}

	private void AllocCurvesDataCacheIFN()
	{
		int num = 0;
		foreach (PKFxManager.Sampler fxSamplers in m_FxSamplersList)
		{
			if (fxSamplers.m_CurvesTimeKeys != null)
			{
				num += fxSamplers.m_CurvesTimeKeys.Length;
				AnimationCurve[] curvesArray = fxSamplers.m_CurvesArray;
				foreach (AnimationCurve animationCurve in curvesArray)
				{
					num += animationCurve.keys.Length * 3;
				}
			}
		}
		if (m_SamplersCurvesDataCache != null)
		{
			if (m_SamplersCurvesDataCache.Length >= num)
			{
				return;
			}
		}
		m_SamplersCurvesDataCache = new float[num];
		if (m_SamplersCurvesDataGCH.IsAllocated)
		{
			m_SamplersCurvesDataGCH.Free();
		}
		m_SamplersCurvesDataGCH = GCHandle.Alloc(m_SamplersCurvesDataCache, GCHandleType.Pinned);
		m_SamplersCurvesDataHandler = m_SamplersCurvesDataGCH.AddrOfPinnedObject();
	}

	private void UpdateMesh(IntPtr outPtr, int[] trianglesSrc, Mesh mesh, int samplingChannels, bool withSkinning)
	{
		int vertexCount = mesh.vertexCount;
		Marshal.Copy(trianglesSrc, 0, outPtr, trianglesSrc.Length);
		outPtr = new IntPtr(outPtr.ToInt64() + trianglesSrc.Length * 4);
		int num;
		if ((samplingChannels & 1) != 0)
		{
			float[] array = new float[vertexCount * 3];
			Vector3[] vertices = mesh.vertices;
			num = 0;
			if (vertices.Length == vertexCount)
			{
				Vector3[] array2 = vertices;
				for (int i = 0; i < array2.Length; i++)
				{
					Vector3 vector = array2[i];
					array[num] = vector.x;
					array[num + 1] = vector.y;
					array[num + 2] = vector.z;
					num += 3;
				}
			}
			else
			{
				Debug.LogWarning("[PKFX] The FX wants to sample Positions but the Mesh " + mesh.name + " doesn't have them.");
			}
			Marshal.Copy(array, 0, outPtr, array.Length);
			outPtr = new IntPtr(outPtr.ToInt64() + vertexCount * 4 * 3);
		}
		if ((samplingChannels & 2) != 0)
		{
			float[] array3 = new float[vertexCount * 3];
			Vector3[] normals = mesh.normals;
			num = 0;
			if (normals.Length == vertexCount)
			{
				Vector3[] array4 = normals;
				for (int j = 0; j < array4.Length; j++)
				{
					Vector3 vector2 = array4[j];
					array3[num] = vector2.x;
					array3[num + 1] = vector2.y;
					array3[num + 2] = vector2.z;
					num += 3;
				}
			}
			else
			{
				Debug.LogWarning("[PKFX] The FX wants to sample Normals but the Mesh " + mesh.name + " doesn't have them.");
			}
			Marshal.Copy(array3, 0, outPtr, array3.Length);
			outPtr = new IntPtr(outPtr.ToInt64() + vertexCount * 4 * 3);
		}
		if ((samplingChannels & 4) != 0)
		{
			float[] array5 = new float[vertexCount * 4];
			Vector4[] tangents = mesh.tangents;
			num = 0;
			if (tangents.Length == vertexCount)
			{
				Vector4[] array6 = tangents;
				for (int k = 0; k < array6.Length; k++)
				{
					Vector4 vector3 = array6[k];
					array5[num] = vector3.x;
					array5[num + 1] = vector3.y;
					array5[num + 2] = vector3.z;
					array5[num + 3] = vector3.w;
					num += 4;
				}
			}
			else
			{
				Debug.LogWarning("[PKFX] The FX wants to sample Tangents but the Mesh " + mesh.name + " doesn't have them.");
			}
			Marshal.Copy(array5, 0, outPtr, array5.Length);
			outPtr = new IntPtr(outPtr.ToInt64() + vertexCount * 4 * 4);
		}
		if ((samplingChannels & 0x10) != 0)
		{
			float[] array7 = new float[vertexCount * 2];
			Vector2[] uv = mesh.uv;
			num = 0;
			if (uv.Length == vertexCount)
			{
				Vector2[] array8 = uv;
				for (int l = 0; l < array8.Length; l++)
				{
					Vector2 vector4 = array8[l];
					array7[num] = vector4.x;
					array7[num + 1] = vector4.y;
					num += 2;
				}
			}
			else
			{
				Debug.LogWarning("[PKFX] The FX wants to sample UVs but the Mesh " + mesh.name + " doesn't have them.");
			}
			Marshal.Copy(array7, 0, outPtr, array7.Length);
			outPtr = new IntPtr(outPtr.ToInt64() + vertexCount * 4 * 2);
		}
		if ((samplingChannels & 0x20) != 0)
		{
			float[] array9 = new float[vertexCount * 4];
			Color[] colors = mesh.colors;
			num = 0;
			if (colors.Length == vertexCount)
			{
				Color[] array10 = colors;
				for (int m = 0; m < array10.Length; m++)
				{
					Color color = array10[m];
					array9[num] = color.r;
					array9[num + 1] = color.g;
					array9[num + 2] = color.b;
					array9[num + 3] = color.a;
					num += 4;
				}
			}
			else
			{
				Debug.LogWarning("[PKFX] The FX wants to sample Vertex Colors but the Mesh " + mesh.name + " doesn't have them.");
			}
			Marshal.Copy(array9, 0, outPtr, array9.Length);
			outPtr = new IntPtr(outPtr.ToInt64() + vertexCount * 4 * 4);
		}
		if (!withSkinning || mesh.boneWeights.Length <= 0)
		{
			return;
		}
		BoneWeight[] boneWeights = mesh.boneWeights;
		float[] array11 = new float[boneWeights.Length * 8];
		num = 0;
		BoneWeight[] array12 = boneWeights;
		for (int n = 0; n < array12.Length; n++)
		{
			BoneWeight boneWeight = array12[n];
			array11[num] = boneWeight.boneIndex0;
			array11[num + 1] = boneWeight.boneIndex1;
			array11[num + 2] = boneWeight.boneIndex2;
			array11[num + 3] = boneWeight.boneIndex3;
			num += 4;
		}
		while (true)
		{
			BoneWeight[] array13 = boneWeights;
			for (int num2 = 0; num2 < array13.Length; num2++)
			{
				BoneWeight boneWeight2 = array13[num2];
				array11[num] = boneWeight2.weight0;
				array11[num + 1] = boneWeight2.weight1;
				array11[num + 2] = boneWeight2.weight2;
				array11[num + 3] = boneWeight2.weight3;
				num += 4;
			}
			while (true)
			{
				Marshal.Copy(array11, 0, outPtr, array11.Length);
				return;
			}
		}
	}

	private void UpdateBones(IntPtr outPtr, PKFxManager.Sampler sampler)
	{
		int num = 0;
		int num2 = 0;
		Matrix4x4 worldToLocalMatrix = sampler.m_SkinnedMeshRenderer.transform.parent.worldToLocalMatrix;
		Matrix4x4 identity = Matrix4x4.identity;
		float[] skeletonDataBuffer = sampler.m_SkinnedMeshData.m_SkeletonDataBuffer;
		Transform[] bones = sampler.m_SkinnedMeshRenderer.bones;
		foreach (Transform transform in bones)
		{
			identity = worldToLocalMatrix * transform.localToWorldMatrix * sampler.m_SkinnedMeshData.m_Bindposes[num2];
			skeletonDataBuffer[num] = identity[0, 0];
			skeletonDataBuffer[num + 1] = identity[0, 1];
			skeletonDataBuffer[num + 2] = identity[0, 2];
			skeletonDataBuffer[num + 3] = identity[0, 3];
			skeletonDataBuffer[num + 4] = identity[1, 0];
			skeletonDataBuffer[num + 5] = identity[1, 1];
			skeletonDataBuffer[num + 6] = identity[1, 2];
			skeletonDataBuffer[num + 7] = identity[1, 3];
			skeletonDataBuffer[num + 8] = identity[2, 0];
			skeletonDataBuffer[num + 9] = identity[2, 1];
			skeletonDataBuffer[num + 10] = identity[2, 2];
			skeletonDataBuffer[num + 11] = identity[2, 3];
			skeletonDataBuffer[num + 12] = identity[3, 0];
			skeletonDataBuffer[num + 13] = identity[3, 1];
			skeletonDataBuffer[num + 14] = identity[3, 2];
			skeletonDataBuffer[num + 15] = identity[3, 3];
			num2++;
			num += 16;
		}
		while (true)
		{
			Marshal.Copy(skeletonDataBuffer, 0, outPtr, skeletonDataBuffer.Length);
			return;
		}
	}

	public void UpdateAttributes(bool forceUpdate)
	{
		int num = -1;
		int num2 = -1;
		if (m_FxAttributesList.Count <= 0)
		{
			if (m_FxSamplersList.Count <= 0)
			{
				while (true)
				{
					switch (5)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
		AllocAttributesCacheIFN();
		AllocCurvesDataCacheIFN();
		for (int i = 0; i < m_FxAttributesList.Count; i++)
		{
			PKFxManager.Attribute attribute = m_FxAttributesList[i];
			int num3;
			if (m_AttributesCache[i].m_Value0 == attribute.m_Value0 && m_AttributesCache[i].m_Value1 == attribute.m_Value1)
			{
				if (m_AttributesCache[i].m_Value2 == attribute.m_Value2)
				{
					num3 = ((m_AttributesCache[i].m_Value3 != attribute.m_Value3) ? 1 : 0);
					goto IL_00e2;
				}
			}
			num3 = 1;
			goto IL_00e2;
			IL_00e2:
			if (num3 == 0)
			{
				if (!forceUpdate)
				{
					if (!m_ForceUpdateAttributes)
					{
						continue;
					}
				}
			}
			m_AttributesCache[i].m_Type = (int)m_FxAttributesList[i].m_Descriptor.Type;
			m_AttributesCache[i].m_Value0 = attribute.m_Value0;
			m_AttributesCache[i].m_Value1 = attribute.m_Value1;
			m_AttributesCache[i].m_Value2 = attribute.m_Value2;
			m_AttributesCache[i].m_Value3 = attribute.m_Value3;
			num = i;
		}
		while (true)
		{
			if (num >= 0)
			{
				if (!PKFxManager.EffectSetAttributes(m_FXGUID, num + 1, m_AttributesHandler))
				{
					Debug.LogError("[PKFX] Attribute through pinned memory failed.");
					Debug.LogError("[PKFX] Did you try to change an FX without stopping it beforehand?");
				}
			}
			int num4 = 0;
			for (int j = 0; j < m_FxSamplersList.Count; j++)
			{
				PKFxManager.Sampler sampler = m_FxSamplersList[j];
				int type = m_FxSamplersList[j].m_Descriptor.Type;
				Vector3 lhs = new Vector3(m_SamplersCache[j].m_PosX, m_SamplersCache[j].m_PosY, m_SamplersCache[j].m_PosZ);
				Vector3 lhs2 = new Vector3(m_SamplersCache[j].m_EulX, m_SamplersCache[j].m_EulY, m_SamplersCache[j].m_EulZ);
				Vector3 lhs3 = new Vector3(m_SamplersCache[j].m_SizeX, m_SamplersCache[j].m_SizeY, m_SamplersCache[j].m_SizeZ);
				int samplingChannels = m_SamplersCache[j].m_SamplingChannels;
				m_SamplersCache[j].m_Type1 = type;
				bool flag;
				bool flag2;
				int num6;
				if (m_SamplersCache[j].m_Type1 == 0)
				{
					int num5;
					if (sampler.m_ShapeType != 4)
					{
						num5 = ((sampler.m_ShapeType == 5) ? 1 : 0);
					}
					else
					{
						num5 = 1;
					}
					flag = ((byte)num5 != 0);
					flag2 = (sampler.m_ShapeType == 5 && sampler.m_SkinnedMeshRenderer != null);
					if (sampler.m_EditorShapeType == 5)
					{
						if (sampler.m_MeshFilter == null)
						{
							sampler.m_Mesh = null;
						}
						else if (sampler.m_Mesh != sampler.m_MeshFilter.sharedMesh)
						{
							sampler.m_Mesh = sampler.m_MeshFilter.sharedMesh;
							if (sampler.m_Mesh != null)
							{
								sampler.m_MeshHashCode = sampler.m_Mesh.name.GetHashCode();
							}
							else
							{
								sampler.m_MeshHashCode = 0;
							}
						}
					}
					if (sampler.m_MeshHashCode == m_SamplersCache[j].m_HashCode)
					{
						if (!(lhs3 != sampler.m_Dimensions) && m_SamplersCache[j].m_Type2 == sampler.m_ShapeType && !(lhs != sampler.m_ShapeCenter))
						{
							if (!(lhs2 != sampler.m_EulerOrientation))
							{
								if (flag)
								{
									num6 = ((samplingChannels != sampler.m_SamplingChannels) ? 1 : 0);
								}
								else
								{
									num6 = 0;
								}
								goto IL_04d9;
							}
						}
					}
					num6 = 1;
					goto IL_04d9;
				}
				if (m_SamplersCache[j].m_Type1 == 2)
				{
					if (!sampler.m_TextureChanged && sampler.m_TextureTexcoordMode == (PKFxManager.ETexcoordMode)(int)m_SamplersCache[j].m_PosX)
					{
						if (!forceUpdate)
						{
							if (!m_ForceUpdateAttributes)
							{
								continue;
							}
						}
					}
					if (sampler.m_Texture == null)
					{
						m_SamplersCache[j].m_SizeX = 0f;
						m_SamplersCache[j].m_SizeY = 0f;
						if (m_SamplersCache[j].m_Data != IntPtr.Zero)
						{
							Marshal.FreeHGlobal(m_SamplersCache[j].m_Data);
						}
						m_SamplersCache[j].m_Data = IntPtr.Zero;
						int num7 = 0;
						m_SamplersCache[j].m_SizeZ = num7;
						m_SamplersCache[j].m_PosX = 0f;
					}
					else
					{
						byte[] data = sampler.m_Texture.GetRawTextureData();
						if (data.Length == 0)
						{
							Debug.LogError("[PKFX] Sampler " + sampler.m_Descriptor.Name + " : Could not get raw texture data. Enable read/write in import settings.");
						}
						if (sampler.m_Texture.format == TextureFormat.DXT1)
						{
							m_SamplersCache[j].m_Type2 = 8;
						}
						else if (sampler.m_Texture.format == TextureFormat.DXT5)
						{
							m_SamplersCache[j].m_Type2 = 12;
						}
						else if (sampler.m_Texture.format == TextureFormat.ARGB32)
						{
							PKImageConverter.ARGB2BGRA(ref data);
							m_SamplersCache[j].m_Type2 = 4;
						}
						else if (sampler.m_Texture.format == TextureFormat.RGBA32)
						{
							PKImageConverter.RGBA2BGRA(ref data);
							m_SamplersCache[j].m_Type2 = 4;
						}
						else if (sampler.m_Texture.format == TextureFormat.BGRA32)
						{
							m_SamplersCache[j].m_Type2 = 4;
						}
						else if (sampler.m_Texture.format == TextureFormat.RGB24)
						{
							PKImageConverter.RGB2BGR(ref data);
							m_SamplersCache[j].m_Type2 = 3;
						}
						else if (sampler.m_Texture.format == TextureFormat.PVRTC_RGB4)
						{
							m_SamplersCache[j].m_Type2 = 20;
						}
						else if (sampler.m_Texture.format == TextureFormat.PVRTC_RGBA4)
						{
							m_SamplersCache[j].m_Type2 = 22;
						}
						else if (sampler.m_Texture.format == TextureFormat.PVRTC_RGB2)
						{
							m_SamplersCache[j].m_Type2 = 21;
						}
						else if (sampler.m_Texture.format == TextureFormat.PVRTC_RGBA2)
						{
							m_SamplersCache[j].m_Type2 = 23;
						}
						else if (sampler.m_Texture.format == TextureFormat.ETC_RGB4)
						{
							m_SamplersCache[j].m_Type2 = 16;
						}
						else if (sampler.m_Texture.format == TextureFormat.ETC2_RGB)
						{
							m_SamplersCache[j].m_Type2 = 17;
						}
						else if (sampler.m_Texture.format == TextureFormat.ETC2_RGBA8)
						{
							m_SamplersCache[j].m_Type2 = 18;
						}
						else if (sampler.m_Texture.format == TextureFormat.ETC2_RGBA1)
						{
							m_SamplersCache[j].m_Type2 = 19;
						}
						else
						{
							m_SamplersCache[j].m_Type2 = 0;
							Debug.LogError("[PKFX] Sampler " + sampler.m_Descriptor.Name + " texture format not supported : " + sampler.m_Texture.format);
						}
						m_SamplersCache[j].m_SizeX = sampler.m_Texture.width;
						m_SamplersCache[j].m_SizeY = sampler.m_Texture.height;
						if (m_SamplersCache[j].m_Data != IntPtr.Zero)
						{
							Marshal.FreeHGlobal(m_SamplersCache[j].m_Data);
						}
						int num8 = data.Length;
						m_SamplersCache[j].m_Data = Marshal.AllocHGlobal(num8);
						Marshal.Copy(data, 0, m_SamplersCache[j].m_Data, num8);
						m_SamplersCache[j].m_SizeZ = num8;
						m_SamplersCache[j].m_PosX = (float)sampler.m_TextureTexcoordMode;
					}
					sampler.m_TextureChanged = false;
					num2 = j;
					continue;
				}
				bool flag3;
				if (m_SamplersCache[j].m_Type1 == 1)
				{
					int num9 = (sampler.m_CurvesTimeKeys != null) ? sampler.m_CurvesTimeKeys.Length : 0;
					if (num9 == 0)
					{
						if ((float)num9 != m_SamplersCache[j].m_SizeX)
						{
							if (m_SamplersCurvesDataGCH.IsAllocated)
							{
								m_SamplersCurvesDataGCH.Free();
							}
							m_SamplersCurvesDataCache = null;
							m_SamplersCache[j].m_Data = IntPtr.Zero;
							m_SamplersCache[j].m_SizeX = 0f;
							num2 = j;
							continue;
						}
					}
					if (sampler.m_CurvesArray == null)
					{
						continue;
					}
					int num10 = sampler.m_CurvesArray.Length;
					int num11 = 1 + num10 * 3;
					flag3 = ((float)num9 != m_SamplersCache[j].m_SizeX);
					if (!flag3 && !forceUpdate)
					{
						if (!m_ForceUpdateAttributes)
						{
							int num12 = 0;
							while (true)
							{
								if (num12 < num9)
								{
									if (m_SamplersCache[j].m_Data != IntPtr.Zero)
									{
										int num13 = num4 + num12 * num11;
										float num14 = sampler.m_CurvesTimeKeys[num12];
										if (num14 != m_SamplersCurvesDataCache[num13])
										{
											flag3 = true;
											break;
										}
									}
									num12++;
									continue;
								}
								break;
							}
						}
					}
					if (!flag3)
					{
						if (!forceUpdate)
						{
							if (!m_ForceUpdateAttributes)
							{
								for (int k = 0; k < sampler.m_CurvesArray.Length; k++)
								{
									if (!(m_SamplersCache[j].m_Data != IntPtr.Zero))
									{
										continue;
									}
									AnimationCurve animationCurve = sampler.m_CurvesArray[k];
									int num15 = 0;
									while (num15 < animationCurve.keys.Length)
									{
										int num16 = num4 + num15 * num11 + 1 + k * 3;
										Keyframe keyframe = animationCurve.keys[num15];
										if (keyframe.value == m_SamplersCurvesDataCache[num16] && keyframe.inTangent == m_SamplersCurvesDataCache[num16 + 1])
										{
											if (keyframe.outTangent == m_SamplersCurvesDataCache[num16 + 2])
											{
												num15++;
												continue;
											}
										}
										flag3 = true;
										break;
									}
								}
							}
						}
					}
					if (!flag3)
					{
						if (!forceUpdate)
						{
							if (!m_ForceUpdateAttributes)
							{
								continue;
							}
						}
					}
					m_SamplersCache[j].m_Data = new IntPtr(m_SamplersCurvesDataHandler.ToInt64() + num4);
					for (int l = 0; l < sampler.m_CurvesTimeKeys.Length; l++)
					{
						int num17 = num4 + l * num11;
						m_SamplersCurvesDataCache[num17] = sampler.m_CurvesTimeKeys[l];
						for (int m = 0; m < sampler.m_CurvesArray.Length; m++)
						{
							AnimationCurve animationCurve2 = sampler.m_CurvesArray[m];
							int num18 = num17 + 1 + m * 3;
							Keyframe keyframe2 = animationCurve2.keys[l];
							m_SamplersCurvesDataCache[num18] = keyframe2.value;
							m_SamplersCurvesDataCache[num18 + 1] = keyframe2.inTangent;
							m_SamplersCurvesDataCache[num18 + 2] = keyframe2.outTangent;
						}
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								goto end_IL_142a;
							}
							continue;
							end_IL_142a:
							break;
						}
					}
					num4 += num9 * num11;
					m_SamplersCache[j].m_SizeX = num9;
					m_SamplersCache[j].m_SizeY = sampler.m_CurvesArray.Length;
					num2 = j;
					continue;
				}
				if (m_SamplersCache[j].m_Type1 != 3)
				{
					continue;
				}
				flag3 = ((float)sampler.m_Text.Length != m_SamplersCache[j].m_SizeX);
				if (!flag3)
				{
					if (sampler.m_Text.Length > 0)
					{
						if (m_SamplersCache[j].m_Data == IntPtr.Zero)
						{
							flag3 = true;
						}
					}
				}
				if (!flag3)
				{
					if (m_SamplersCache[j].m_Data != IntPtr.Zero && Marshal.PtrToStringAnsi(m_SamplersCache[j].m_Data) != sampler.m_Text)
					{
						flag3 = true;
					}
				}
				if (!flag3)
				{
					if (!forceUpdate)
					{
						if (!m_ForceUpdateAttributes)
						{
							continue;
						}
					}
				}
				if (m_SamplersCache[j].m_Data != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(m_SamplersCache[j].m_Data);
					m_SamplersCache[j].m_Data = IntPtr.Zero;
				}
				int length = sampler.m_Text.Length;
				if (length > 0)
				{
					m_SamplersCache[j].m_Data = Marshal.StringToHGlobalAnsi(sampler.m_Text);
				}
				m_SamplersCache[j].m_SizeX = length;
				num2 = j;
				continue;
				IL_04d9:
				flag3 = ((byte)num6 != 0);
				if (m_SamplersCache[j].m_MeshChanged != 0 && m_SamplersCache[j].m_Data != IntPtr.Zero)
				{
					if (sampler.m_SkinnedMeshData == null)
					{
						Marshal.FreeHGlobal(m_SamplersCache[j].m_Data);
						m_SamplersCache[j].m_Data = IntPtr.Zero;
					}
				}
				m_SamplersCache[j].m_MeshChanged = 0;
				if (!flag3)
				{
					if (!forceUpdate)
					{
						if (!m_ForceUpdateAttributes)
						{
							continue;
						}
					}
				}
				m_SamplersCache[j].m_SamplingChannels = sampler.m_SamplingChannels;
				if (flag)
				{
					if (sampler.m_Mesh != null)
					{
						if (!forceUpdate)
						{
							if (m_SamplersCache[j].m_HashCode == sampler.m_MeshHashCode)
							{
								if (m_SamplersCache[j].m_Type2 == m_FxSamplersList[j].m_ShapeType)
								{
									if (samplingChannels == sampler.m_SamplingChannels)
									{
										goto IL_0969;
									}
								}
							}
						}
						int[] triangles = sampler.m_Mesh.triangles;
						int num19;
						if (flag2)
						{
							num19 = sampler.m_SkinnedMeshRenderer.bones.Length;
						}
						else
						{
							num19 = 0;
						}
						int num20 = num19;
						int num21 = triangles.Length * 4;
						int num22;
						if ((sampler.m_SamplingChannels & 1) != 0)
						{
							num22 = sampler.m_Mesh.vertexCount * 4 * 3;
						}
						else
						{
							num22 = 0;
						}
						int num23 = num22;
						int num24;
						if ((sampler.m_SamplingChannels & 2) != 0)
						{
							num24 = sampler.m_Mesh.vertexCount * 4 * 3;
						}
						else
						{
							num24 = 0;
						}
						int num25 = num24;
						int num26;
						if ((sampler.m_SamplingChannels & 4) != 0)
						{
							num26 = sampler.m_Mesh.vertexCount * 4 * 4;
						}
						else
						{
							num26 = 0;
						}
						int num27 = num26;
						int num28 = ((sampler.m_SamplingChannels & 0x10) != 0) ? (sampler.m_Mesh.vertexCount * 4 * 2) : 0;
						int num29;
						if ((sampler.m_SamplingChannels & 0x20) != 0)
						{
							num29 = sampler.m_Mesh.vertexCount * 4 * 4;
						}
						else
						{
							num29 = 0;
						}
						int num30 = num29;
						int num31 = (num20 != 0) ? (sampler.m_Mesh.boneWeights.Length * 4 * 8) : 0;
						int num32 = num21 + num23 + num25 + num27 + num28 + num30 + num31;
						if (num32 > 0)
						{
							m_SamplersCache[j].m_IndexCount = sampler.m_Mesh.triangles.Length;
							m_SamplersCache[j].m_VertexCount = sampler.m_Mesh.vertices.Length;
							m_SamplersCache[j].m_BoneCount = num20;
							if (m_SamplersCache[j].m_Data != IntPtr.Zero)
							{
								Marshal.FreeHGlobal(m_SamplersCache[j].m_Data);
							}
							m_SamplersCache[j].m_Data = Marshal.AllocHGlobal(num32);
							UpdateMesh(m_SamplersCache[j].m_Data, triangles, sampler.m_Mesh, sampler.m_SamplingChannels, flag2);
							m_SamplersCache[j].m_MeshChanged = 42;
							m_SamplersCache[j].m_HashCode = sampler.m_MeshHashCode;
							sampler.m_SkinnedMeshData = null;
						}
						goto IL_0969;
					}
				}
				if (m_SamplersCache[j].m_Data != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(m_SamplersCache[j].m_Data);
					m_SamplersCache[j].m_Data = IntPtr.Zero;
				}
				m_SamplersCache[j].m_IndexCount = 0;
				m_SamplersCache[j].m_VertexCount = 0;
				m_SamplersCache[j].m_MeshChanged = 42;
				m_SamplersCache[j].m_HashCode = 0;
				m_SamplersCache[j].m_BoneCount = 0;
				goto IL_0969;
				IL_0969:
				sampler.m_TextureChanged = false;
				m_SamplersCache[j].m_Type2 = m_FxSamplersList[j].m_ShapeType;
				m_SamplersCache[j].m_SizeX = sampler.m_Dimensions.x;
				m_SamplersCache[j].m_SizeY = sampler.m_Dimensions.y;
				m_SamplersCache[j].m_SizeZ = sampler.m_Dimensions.z;
				m_SamplersCache[j].m_PosX = sampler.m_ShapeCenter.x;
				m_SamplersCache[j].m_PosY = sampler.m_ShapeCenter.y;
				m_SamplersCache[j].m_PosZ = sampler.m_ShapeCenter.z;
				m_SamplersCache[j].m_EulX = sampler.m_EulerOrientation.x;
				m_SamplersCache[j].m_EulY = sampler.m_EulerOrientation.y;
				m_SamplersCache[j].m_EulZ = sampler.m_EulerOrientation.z;
				num2 = j;
			}
			while (true)
			{
				if (num2 >= 0)
				{
					if (!PKFxManager.EffectSetSamplers(m_FXGUID, num2 + 1, m_SamplersHandler))
					{
						Debug.LogError("[PKFX] Sampler through pinned memory failed.");
						Debug.LogError("[PKFX] Did you try to change an FX without stopping it beforehand?");
					}
				}
				for (int n = 0; n < m_FxSamplersList.Count; n++)
				{
					PKFxManager.Sampler sampler2 = m_FxSamplersList[n];
					int num33;
					if (m_FxSamplersList[n].m_ShapeType == 5)
					{
						num33 = ((m_FxSamplersList[n].m_SkinnedMeshRenderer != null) ? 1 : 0);
					}
					else
					{
						num33 = 0;
					}
					if (num33 == 0)
					{
						continue;
					}
					int num34;
					if (sampler2.m_SkinnedMeshRenderer != null)
					{
						num34 = sampler2.m_SkinnedMeshRenderer.bones.Length;
					}
					else
					{
						num34 = 0;
					}
					int num35 = num34;
					if (sampler2.m_SkinnedMeshData == null)
					{
						int cb = (num35 != 0) ? (sampler2.m_SkinnedMeshRenderer.bones.Length * 4 * 16) : 0;
						if (m_SamplersCache[n].m_Data != IntPtr.Zero)
						{
							Marshal.FreeHGlobal(m_SamplersCache[n].m_Data);
						}
						m_SamplersCache[n].m_Data = Marshal.AllocHGlobal(cb);
						sampler2.m_SkinnedMeshData = new PKFxManager.Sampler.SkinnedMeshData();
						sampler2.m_SkinnedMeshData.InitData(sampler2.m_SkinnedMeshRenderer);
					}
					UpdateBones(m_SamplersCache[n].m_Data, sampler2);
					if (!PKFxManager.EffectUpdateSamplerSkinning(m_FXGUID, n, m_SamplersHandler, Time.deltaTime))
					{
						Debug.LogError("[PKFX] Skinning through pinned memory failed.");
					}
				}
				while (true)
				{
					m_ForceUpdateAttributes = false;
					return;
				}
			}
		}
	}

	public PKFxManager.Sampler GetSamplerFromDesc(PKFxManager.SamplerDesc desc)
	{
		using (List<PKFxManager.Sampler>.Enumerator enumerator = m_FxSamplersList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.Sampler current = enumerator.Current;
				if (current.m_Descriptor.Name == desc.Name && current.m_Descriptor.Type == desc.Type)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							return current;
						}
					}
				}
			}
		}
		return null;
	}

	public void DeleteAttribute(PKFxManager.AttributeDesc desc)
	{
		using (List<PKFxManager.Attribute>.Enumerator enumerator = m_FxAttributesList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.Attribute current = enumerator.Current;
				if (current.m_Descriptor.Name == desc.Name && current.m_Descriptor.Type == desc.Type)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							m_FxAttributesList.Remove(current);
							return;
						}
					}
				}
			}
			while (true)
			{
				switch (1)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public PKFxManager.Attribute GetAttributeFromDesc(PKFxManager.AttributeDesc desc)
	{
		using (List<PKFxManager.Attribute>.Enumerator enumerator = m_FxAttributesList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.Attribute current = enumerator.Current;
				if (current.m_Descriptor.Name == desc.Name && current.m_Descriptor.Type == desc.Type)
				{
					current.m_Descriptor = desc;
					return current;
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					goto end_IL_000e;
				}
			}
			end_IL_000e:;
		}
		return null;
	}

	public bool SamplerExists(PKFxManager.SamplerDesc desc)
	{
		foreach (PKFxManager.Sampler fxSamplers in m_FxSamplersList)
		{
			if (fxSamplers.m_Descriptor.Name == desc.Name)
			{
				if (fxSamplers.m_Descriptor.Type == desc.Type)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public bool AttributeExists(PKFxManager.AttributeDesc desc)
	{
		using (List<PKFxManager.Attribute>.Enumerator enumerator = m_FxAttributesList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.Attribute current = enumerator.Current;
				if (current.m_Descriptor.Name == desc.Name)
				{
					if (current.m_Descriptor.Type == desc.Type)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	private bool AttributesDescExistIn(List<PKFxManager.AttributeDesc> FxAttributesDesc, PKFxManager.Attribute attr)
	{
		using (List<PKFxManager.AttributeDesc>.Enumerator enumerator = FxAttributesDesc.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.AttributeDesc current = enumerator.Current;
				if (attr.m_Descriptor.Name == current.Name)
				{
					if (attr.m_Descriptor.Type == current.Type)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}
}
