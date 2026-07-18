using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PKFxFX : PKFxPackDependent
{
	private bool m_IsStopped;

	private int m_FXGUID = -1;

	private bool m_AskedToStart;

	public List<PKFxManager.Attribute> m_FxAttributesList;

	public List<PKFxManager.Sampler> m_FxSamplersList;

	public bool m_PlayOnStart = true;

	public bool m_IsPlaying;

	public static Dictionary<int, PKFxFX> m_ListEffects = new Dictionary<int, PKFxFX>();

	private bool m_ForceUpdateAttributes;

	private PKFxFX.SAttributePinned[] m_AttributesCache;

	private GCHandle m_AttributesGCH;

	private IntPtr m_AttributesHandler;

	private PKFxFX.SSamplerPinned[] m_SamplersCache;

	private GCHandle m_SamplersGCH;

	private IntPtr m_SamplersHandler;

	private float[] m_SamplersCurvesDataCache;

	private GCHandle m_SamplersCurvesDataGCH;

	private IntPtr m_SamplersCurvesDataHandler;

	public PKFxFX.OnFxStoppedDelegate m_OnFxStopped;

	public UnityEngine.Object m_BoundFx;

	public string m_FxName;

	public int FXGUID
	{
		get
		{
			return this.m_FXGUID;
		}
	}

	public string FxPath
	{
		get
		{
			return this.m_FxName;
		}
	}

	private void Awake()
	{
		if (this.m_FxAttributesList == null)
		{
			this.m_FxAttributesList = new List<PKFxManager.Attribute>();
		}
		if (this.m_FxSamplersList == null)
		{
			this.m_FxSamplersList = new List<PKFxManager.Sampler>();
		}
		this.m_IsPlaying = false;
	}

	private IEnumerator Start()
	{
		base.BaseInitialize();
		yield return base.WaitForPack(true);
		if (!string.IsNullOrEmpty(this.m_FxName))
		{
			PKFxManager.PreLoadFxIFN(this.FxPath);
		}
		if (!this.m_PlayOnStart)
		{
			if (!this.m_AskedToStart)
			{
				goto IL_CA;
			}
		}
		this.StartEffect();
		IL_CA:
		this.m_AskedToStart = false;
		yield return null;
		yield break;
	}

	private void LateUpdate()
	{
		if (!this.m_IsPlaying)
		{
			return;
		}
		PKFxManager.UpdateTransformEffect(this.m_FXGUID, base.transform);
		this.UpdateAttributes(false);
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawIcon(base.transform.position, "FX.png", true);
	}

	private void OnDestroy()
	{
		this.KillEffect();
		this.StopEffect();
		for (int i = 0; i < this.m_FxSamplersList.Count; i++)
		{
			if (this.m_SamplersCache != null && this.m_SamplersCache[i].m_Data != IntPtr.Zero)
			{
				if (this.m_SamplersCache[i].m_Type1 != 1)
				{
					Marshal.FreeHGlobal(this.m_SamplersCache[i].m_Data);
				}
			}
		}
		this.m_SamplersCache = null;
		if (this.m_AttributesGCH.IsAllocated)
		{
			this.m_AttributesGCH.Free();
		}
		if (this.m_SamplersGCH.IsAllocated)
		{
			this.m_SamplersGCH.Free();
		}
		if (this.m_SamplersCurvesDataGCH.IsAllocated)
		{
			this.m_SamplersCurvesDataGCH.Free();
		}
	}

	public void StartEffect()
	{
		if (!PKFxManager.m_PackLoaded)
		{
			this.m_AskedToStart = true;
			return;
		}
		if (this.m_IsStopped)
		{
			Debug.LogWarning("[PKFX] Attempt to start an effect while the stopped effect is still running.");
			return;
		}
		if (this.m_IsPlaying)
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.m_FxName))
		{
			this.m_FXGUID = PKFxManager.CreateEffect(this.FxPath, base.transform);
		}
		this.m_IsPlaying = (this.m_FXGUID != -1);
		if (this.m_FXGUID != -1)
		{
			if (PKFxFX.m_ListEffects.ContainsKey(this.m_FXGUID))
			{
				PKFxFX.m_ListEffects[this.m_FXGUID] = this;
			}
			else
			{
				PKFxFX.m_ListEffects.Add(this.m_FXGUID, this);
			}
		}
		this.LoadAttributes(PKFxManager.ListEffectAttributesFromGUID(this.m_FXGUID), false);
		this.LoadSamplers(PKFxManager.ListEffectSamplersFromFx(this.m_FxName), false);
		this.UpdateAttributes(true);
	}

	public void TerminateEffect()
	{
		if (this.m_IsPlaying)
		{
			if (this.m_FXGUID != -1)
			{
				PKFxManager.TerminateFx(this.m_FXGUID);
				this.m_IsPlaying = false;
				return;
			}
		}
	}

	public void StopEffect()
	{
		if (this.m_IsPlaying)
		{
			if (this.m_FXGUID != -1)
			{
				if (PKFxManager.StopFx(this.m_FXGUID))
				{
					this.m_IsStopped = true;
				}
				return;
			}
		}
	}

	public void OnFxStopPlaying()
	{
		this.m_IsPlaying = false;
		this.m_IsStopped = false;
	}

	public void KillEffect()
	{
		if (this.m_IsPlaying)
		{
			if (this.m_FXGUID != -1 && PKFxManager.KillIndividualEffectEnabled())
			{
				PKFxManager.KillFx(this.m_FXGUID);
				this.m_IsPlaying = false;
				return;
			}
		}
	}

	public bool IsPlayable()
	{
		bool result;
		if (!this.m_IsPlaying)
		{
			result = (this.m_FXGUID != -1);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool IsPlaying()
	{
		return this.m_IsPlaying;
	}

	public bool Alive()
	{
		return this.m_IsPlaying && this.m_FXGUID != -1 && PKFxManager.IsFxAlive(this.m_FXGUID);
	}

	public void OnFxHotReloaded(int newGuid)
	{
		if (newGuid != -1)
		{
			if (newGuid != this.m_FXGUID)
			{
				PKFxFX.m_ListEffects.Remove(this.m_FXGUID);
				this.m_FXGUID = newGuid;
				PKFxFX.m_ListEffects.Add(this.m_FXGUID, this);
			}
			this.m_ForceUpdateAttributes = true;
			this.m_IsPlaying = true;
		}
	}

	public PKFxManager.Attribute GetAttribute(string name)
	{
		using (List<PKFxManager.Attribute>.Enumerator enumerator = this.m_FxAttributesList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.Attribute attribute = enumerator.Current;
				if (attribute.m_Descriptor.Name == name)
				{
					return attribute;
				}
			}
		}
		return null;
	}

	public PKFxManager.Attribute GetAttribute(string name, PKFxManager.BaseType type)
	{
		using (List<PKFxManager.Attribute>.Enumerator enumerator = this.m_FxAttributesList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.Attribute attribute = enumerator.Current;
				if (attribute.m_Descriptor.Name == name)
				{
					if (attribute.m_Descriptor.Type == type)
					{
						return attribute;
					}
				}
			}
		}
		return null;
	}

	public void SetAttribute(PKFxManager.Attribute attr)
	{
		if (!this.AttributeExists(attr.m_Descriptor))
		{
			Debug.LogError("[PKFX] FX.SetAttribute : " + attr.m_Descriptor.Name + " doesn't exist");
		}
		else
		{
			for (int i = 0; i < this.m_FxAttributesList.Count; i++)
			{
				if (this.m_FxAttributesList[i].m_Descriptor.Name == attr.m_Descriptor.Name)
				{
					this.m_FxAttributesList[i].m_Value0 = attr.m_Value0;
					this.m_FxAttributesList[i].m_Value1 = attr.m_Value1;
					this.m_FxAttributesList[i].m_Value2 = attr.m_Value2;
					this.m_FxAttributesList[i].m_Value3 = attr.m_Value3;
				}
			}
		}
	}

	public PKFxManager.Sampler GetSampler(string name)
	{
		using (List<PKFxManager.Sampler>.Enumerator enumerator = this.m_FxSamplersList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.Sampler sampler = enumerator.Current;
				if (sampler.m_Descriptor.Name == name)
				{
					return sampler;
				}
			}
		}
		return null;
	}

	public PKFxManager.Sampler GetSampler(string name, PKFxManager.ESamplerType type)
	{
		using (List<PKFxManager.Sampler>.Enumerator enumerator = this.m_FxSamplersList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.Sampler sampler = enumerator.Current;
				if (sampler.m_Descriptor.Name == name && sampler.m_Descriptor.Type == (int)type)
				{
					return sampler;
				}
			}
		}
		return null;
	}

	public void SetSampler(PKFxManager.Sampler sampler)
	{
		if (!this.SamplerExists(sampler.m_Descriptor))
		{
			Debug.LogError("[PKFX] FX.SetSampler : " + sampler.m_Descriptor.Name + " doesn't exist");
		}
		else
		{
			for (int i = 0; i < this.m_FxSamplersList.Count; i++)
			{
				if (this.m_FxSamplersList[i].m_Descriptor.Name == sampler.m_Descriptor.Name)
				{
					if (this.m_FxSamplersList[i].m_Descriptor.Type == sampler.m_Descriptor.Type)
					{
						this.m_FxSamplersList[i].Copy(sampler);
					}
				}
			}
		}
	}

	public void LoadSamplers(List<PKFxManager.SamplerDesc> FxSamplersDesc, bool flushAttributes)
	{
		if (flushAttributes)
		{
			this.m_FxSamplersList.Clear();
		}
		List<PKFxManager.Sampler> list = new List<PKFxManager.Sampler>();
		using (List<PKFxManager.SamplerDesc>.Enumerator enumerator = FxSamplersDesc.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.SamplerDesc samplerDesc = enumerator.Current;
				if (!this.SamplerExists(samplerDesc))
				{
					list.Add(new PKFxManager.Sampler(samplerDesc));
				}
				else
				{
					list.Add(this.GetSamplerFromDesc(samplerDesc));
				}
			}
		}
		this.m_FxSamplersList = list;
	}

	public void ResetAttributesToDefault(List<PKFxManager.AttributeDesc> FxAttributesDesc)
	{
		this.m_FxAttributesList.Clear();
		List<PKFxManager.Attribute> list = new List<PKFxManager.Attribute>();
		using (List<PKFxManager.AttributeDesc>.Enumerator enumerator = FxAttributesDesc.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.AttributeDesc desc = enumerator.Current;
				list.Add(new PKFxManager.Attribute(desc));
			}
		}
		this.m_FxAttributesList = list;
	}

	public void LoadAttributes(List<PKFxManager.AttributeDesc> FxAttributesDesc, bool flushAttributes)
	{
		if (flushAttributes)
		{
			this.m_FxAttributesList.Clear();
		}
		List<PKFxManager.Attribute> list = new List<PKFxManager.Attribute>();
		using (List<PKFxManager.AttributeDesc>.Enumerator enumerator = FxAttributesDesc.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.AttributeDesc desc = enumerator.Current;
				if (!this.AttributeExists(desc))
				{
					list.Add(new PKFxManager.Attribute(desc));
				}
				else
				{
					list.Add(this.GetAttributeFromDesc(desc));
				}
			}
		}
		this.m_FxAttributesList = list;
	}

	private void AllocAttributesCacheIFN()
	{
		if (this.m_AttributesCache != null)
		{
			if (this.m_AttributesCache.Length >= this.m_FxAttributesList.Count)
			{
				goto IL_91;
			}
		}
		this.m_AttributesCache = new PKFxFX.SAttributePinned[this.m_FxAttributesList.Count];
		if (this.m_AttributesGCH.IsAllocated)
		{
			this.m_AttributesGCH.Free();
		}
		this.m_AttributesGCH = GCHandle.Alloc(this.m_AttributesCache, GCHandleType.Pinned);
		this.m_AttributesHandler = this.m_AttributesGCH.AddrOfPinnedObject();
		IL_91:
		if (this.m_SamplersCache != null)
		{
			if (this.m_SamplersCache.Length >= this.m_FxSamplersList.Count)
			{
				return;
			}
		}
		this.m_SamplersCache = new PKFxFX.SSamplerPinned[this.m_FxSamplersList.Count];
		if (this.m_SamplersGCH.IsAllocated)
		{
			this.m_SamplersGCH.Free();
		}
		this.m_SamplersGCH = GCHandle.Alloc(this.m_SamplersCache, GCHandleType.Pinned);
		this.m_SamplersHandler = this.m_SamplersGCH.AddrOfPinnedObject();
	}

	private void AllocCurvesDataCacheIFN()
	{
		int num = 0;
		foreach (PKFxManager.Sampler sampler in this.m_FxSamplersList)
		{
			if (sampler.m_CurvesTimeKeys != null)
			{
				num += sampler.m_CurvesTimeKeys.Length;
				foreach (AnimationCurve animationCurve in sampler.m_CurvesArray)
				{
					num += animationCurve.keys.Length * 3;
				}
			}
		}
		if (this.m_SamplersCurvesDataCache != null)
		{
			if (this.m_SamplersCurvesDataCache.Length >= num)
			{
				return;
			}
		}
		this.m_SamplersCurvesDataCache = new float[num];
		if (this.m_SamplersCurvesDataGCH.IsAllocated)
		{
			this.m_SamplersCurvesDataGCH.Free();
		}
		this.m_SamplersCurvesDataGCH = GCHandle.Alloc(this.m_SamplersCurvesDataCache, GCHandleType.Pinned);
		this.m_SamplersCurvesDataHandler = this.m_SamplersCurvesDataGCH.AddrOfPinnedObject();
	}

	private void UpdateMesh(IntPtr outPtr, int[] trianglesSrc, Mesh mesh, int samplingChannels, bool withSkinning)
	{
		int vertexCount = mesh.vertexCount;
		Marshal.Copy(trianglesSrc, 0, outPtr, trianglesSrc.Length);
		outPtr = new IntPtr(outPtr.ToInt64() + (long)(trianglesSrc.Length * 4));
		if ((samplingChannels & 1) != 0)
		{
			float[] array = new float[vertexCount * 3];
			Vector3[] vertices = mesh.vertices;
			int num = 0;
			if (vertices.Length == vertexCount)
			{
				foreach (Vector3 vector in vertices)
				{
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
			outPtr = new IntPtr(outPtr.ToInt64() + (long)(vertexCount * 4 * 3));
		}
		if ((samplingChannels & 2) != 0)
		{
			float[] array3 = new float[vertexCount * 3];
			Vector3[] normals = mesh.normals;
			int num = 0;
			if (normals.Length == vertexCount)
			{
				foreach (Vector3 vector2 in normals)
				{
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
			outPtr = new IntPtr(outPtr.ToInt64() + (long)(vertexCount * 4 * 3));
		}
		if ((samplingChannels & 4) != 0)
		{
			float[] array5 = new float[vertexCount * 4];
			Vector4[] tangents = mesh.tangents;
			int num = 0;
			if (tangents.Length == vertexCount)
			{
				foreach (Vector4 vector3 in tangents)
				{
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
			outPtr = new IntPtr(outPtr.ToInt64() + (long)(vertexCount * 4 * 4));
		}
		if ((samplingChannels & 0x10) != 0)
		{
			float[] array7 = new float[vertexCount * 2];
			Vector2[] uv = mesh.uv;
			int num = 0;
			if (uv.Length == vertexCount)
			{
				foreach (Vector2 vector4 in uv)
				{
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
			outPtr = new IntPtr(outPtr.ToInt64() + (long)(vertexCount * 4 * 2));
		}
		if ((samplingChannels & 0x20) != 0)
		{
			float[] array9 = new float[vertexCount * 4];
			Color[] colors = mesh.colors;
			int num = 0;
			if (colors.Length == vertexCount)
			{
				foreach (Color color in colors)
				{
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
			outPtr = new IntPtr(outPtr.ToInt64() + (long)(vertexCount * 4 * 4));
		}
		if (withSkinning && mesh.boneWeights.Length > 0)
		{
			BoneWeight[] boneWeights = mesh.boneWeights;
			float[] array11 = new float[boneWeights.Length * 8];
			int num = 0;
			foreach (BoneWeight boneWeight in boneWeights)
			{
				array11[num] = (float)boneWeight.boneIndex0;
				array11[num + 1] = (float)boneWeight.boneIndex1;
				array11[num + 2] = (float)boneWeight.boneIndex2;
				array11[num + 3] = (float)boneWeight.boneIndex3;
				num += 4;
			}
			foreach (BoneWeight boneWeight2 in boneWeights)
			{
				array11[num] = boneWeight2.weight0;
				array11[num + 1] = boneWeight2.weight1;
				array11[num + 2] = boneWeight2.weight2;
				array11[num + 3] = boneWeight2.weight3;
				num += 4;
			}
			Marshal.Copy(array11, 0, outPtr, array11.Length);
		}
	}

	private void UpdateBones(IntPtr outPtr, PKFxManager.Sampler sampler)
	{
		int num = 0;
		int num2 = 0;
		Matrix4x4 worldToLocalMatrix = sampler.m_SkinnedMeshRenderer.transform.parent.worldToLocalMatrix;
		Matrix4x4 matrix4x = Matrix4x4.identity;
		float[] skeletonDataBuffer = sampler.m_SkinnedMeshData.m_SkeletonDataBuffer;
		foreach (Transform transform in sampler.m_SkinnedMeshRenderer.bones)
		{
			matrix4x = worldToLocalMatrix * transform.localToWorldMatrix * sampler.m_SkinnedMeshData.m_Bindposes[num2];
			skeletonDataBuffer[num] = matrix4x[0, 0];
			skeletonDataBuffer[num + 1] = matrix4x[0, 1];
			skeletonDataBuffer[num + 2] = matrix4x[0, 2];
			skeletonDataBuffer[num + 3] = matrix4x[0, 3];
			skeletonDataBuffer[num + 4] = matrix4x[1, 0];
			skeletonDataBuffer[num + 5] = matrix4x[1, 1];
			skeletonDataBuffer[num + 6] = matrix4x[1, 2];
			skeletonDataBuffer[num + 7] = matrix4x[1, 3];
			skeletonDataBuffer[num + 8] = matrix4x[2, 0];
			skeletonDataBuffer[num + 9] = matrix4x[2, 1];
			skeletonDataBuffer[num + 0xA] = matrix4x[2, 2];
			skeletonDataBuffer[num + 0xB] = matrix4x[2, 3];
			skeletonDataBuffer[num + 0xC] = matrix4x[3, 0];
			skeletonDataBuffer[num + 0xD] = matrix4x[3, 1];
			skeletonDataBuffer[num + 0xE] = matrix4x[3, 2];
			skeletonDataBuffer[num + 0xF] = matrix4x[3, 3];
			num2++;
			num += 0x10;
		}
		Marshal.Copy(skeletonDataBuffer, 0, outPtr, skeletonDataBuffer.Length);
	}

	public void UpdateAttributes(bool forceUpdate)
	{
		int num = -1;
		int num2 = -1;
		if (this.m_FxAttributesList.Count <= 0)
		{
			if (this.m_FxSamplersList.Count <= 0)
			{
				return;
			}
		}
		this.AllocAttributesCacheIFN();
		this.AllocCurvesDataCacheIFN();
		int i = 0;
		while (i < this.m_FxAttributesList.Count)
		{
			PKFxManager.Attribute attribute = this.m_FxAttributesList[i];
			if (this.m_AttributesCache[i].m_Value0 != attribute.m_Value0 || this.m_AttributesCache[i].m_Value1 != attribute.m_Value1)
			{
				goto IL_E1;
			}
			if (this.m_AttributesCache[i].m_Value2 != attribute.m_Value2)
			{
				goto IL_E1;
			}
			bool flag = this.m_AttributesCache[i].m_Value3 != attribute.m_Value3;
			IL_E2:
			if (flag)
			{
				goto IL_114;
			}
			if (forceUpdate)
			{
				goto IL_114;
			}
			if (this.m_ForceUpdateAttributes)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					goto IL_114;
				}
			}
			IL_19B:
			i++;
			continue;
			IL_114:
			this.m_AttributesCache[i].m_Type = (int)this.m_FxAttributesList[i].m_Descriptor.Type;
			this.m_AttributesCache[i].m_Value0 = attribute.m_Value0;
			this.m_AttributesCache[i].m_Value1 = attribute.m_Value1;
			this.m_AttributesCache[i].m_Value2 = attribute.m_Value2;
			this.m_AttributesCache[i].m_Value3 = attribute.m_Value3;
			num = i;
			goto IL_19B;
			IL_E1:
			flag = true;
			goto IL_E2;
		}
		if (num >= 0)
		{
			if (!PKFxManager.EffectSetAttributes(this.m_FXGUID, num + 1, this.m_AttributesHandler))
			{
				Debug.LogError("[PKFX] Attribute through pinned memory failed.");
				Debug.LogError("[PKFX] Did you try to change an FX without stopping it beforehand?");
			}
		}
		int num3 = 0;
		for (int j = 0; j < this.m_FxSamplersList.Count; j++)
		{
			PKFxManager.Sampler sampler = this.m_FxSamplersList[j];
			int type = this.m_FxSamplersList[j].m_Descriptor.Type;
			Vector3 lhs = new Vector3(this.m_SamplersCache[j].m_PosX, this.m_SamplersCache[j].m_PosY, this.m_SamplersCache[j].m_PosZ);
			Vector3 lhs2 = new Vector3(this.m_SamplersCache[j].m_EulX, this.m_SamplersCache[j].m_EulY, this.m_SamplersCache[j].m_EulZ);
			Vector3 lhs3 = new Vector3(this.m_SamplersCache[j].m_SizeX, this.m_SamplersCache[j].m_SizeY, this.m_SamplersCache[j].m_SizeZ);
			int samplingChannels = this.m_SamplersCache[j].m_SamplingChannels;
			this.m_SamplersCache[j].m_Type1 = type;
			if (this.m_SamplersCache[j].m_Type1 == 0)
			{
				bool flag2;
				if (sampler.m_ShapeType != 4)
				{
					flag2 = (sampler.m_ShapeType == 5);
				}
				else
				{
					flag2 = true;
				}
				bool flag3 = flag2;
				bool flag4 = sampler.m_ShapeType == 5 && sampler.m_SkinnedMeshRenderer != null;
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
				if (sampler.m_MeshHashCode != this.m_SamplersCache[j].m_HashCode)
				{
					goto IL_4D8;
				}
				if (lhs3 != sampler.m_Dimensions || this.m_SamplersCache[j].m_Type2 != sampler.m_ShapeType || lhs != sampler.m_ShapeCenter)
				{
					goto IL_4D8;
				}
				if (lhs2 != sampler.m_EulerOrientation)
				{
					goto IL_4D8;
				}
				bool flag5;
				if (flag3)
				{
					flag5 = (samplingChannels != sampler.m_SamplingChannels);
				}
				else
				{
					flag5 = false;
				}
				IL_4D9:
				bool flag6 = flag5;
				if (this.m_SamplersCache[j].m_MeshChanged != 0 && this.m_SamplersCache[j].m_Data != IntPtr.Zero)
				{
					if (sampler.m_SkinnedMeshData == null)
					{
						Marshal.FreeHGlobal(this.m_SamplersCache[j].m_Data);
						this.m_SamplersCache[j].m_Data = IntPtr.Zero;
					}
				}
				this.m_SamplersCache[j].m_MeshChanged = 0;
				if (flag6)
				{
					goto IL_59D;
				}
				if (forceUpdate)
				{
					goto IL_59D;
				}
				if (this.m_ForceUpdateAttributes)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						goto IL_59D;
					}
				}
				IL_AA8:
				goto IL_165A;
				IL_59D:
				this.m_SamplersCache[j].m_SamplingChannels = sampler.m_SamplingChannels;
				if (!flag3)
				{
					goto IL_8BB;
				}
				if (!(sampler.m_Mesh != null))
				{
					goto IL_8BB;
				}
				if (forceUpdate)
				{
					goto IL_657;
				}
				if (this.m_SamplersCache[j].m_HashCode != sampler.m_MeshHashCode)
				{
					goto IL_657;
				}
				if (this.m_SamplersCache[j].m_Type2 != this.m_FxSamplersList[j].m_ShapeType)
				{
					goto IL_657;
				}
				if (samplingChannels != sampler.m_SamplingChannels)
				{
					goto IL_657;
				}
				goto IL_969;
				IL_657:
				int[] triangles = sampler.m_Mesh.triangles;
				int num4;
				if (flag4)
				{
					num4 = sampler.m_SkinnedMeshRenderer.bones.Length;
				}
				else
				{
					num4 = 0;
				}
				int num5 = num4;
				int num6 = triangles.Length * 4;
				int num7;
				if ((sampler.m_SamplingChannels & 1) != 0)
				{
					num7 = sampler.m_Mesh.vertexCount * 4 * 3;
				}
				else
				{
					num7 = 0;
				}
				int num8 = num7;
				int num9;
				if ((sampler.m_SamplingChannels & 2) != 0)
				{
					num9 = sampler.m_Mesh.vertexCount * 4 * 3;
				}
				else
				{
					num9 = 0;
				}
				int num10 = num9;
				int num11;
				if ((sampler.m_SamplingChannels & 4) != 0)
				{
					num11 = sampler.m_Mesh.vertexCount * 4 * 4;
				}
				else
				{
					num11 = 0;
				}
				int num12 = num11;
				int num13 = ((sampler.m_SamplingChannels & 0x10) == 0) ? 0 : (sampler.m_Mesh.vertexCount * 4 * 2);
				int num14;
				if ((sampler.m_SamplingChannels & 0x20) != 0)
				{
					num14 = sampler.m_Mesh.vertexCount * 4 * 4;
				}
				else
				{
					num14 = 0;
				}
				int num15 = num14;
				int num16 = (num5 == 0) ? 0 : (sampler.m_Mesh.boneWeights.Length * 4 * 8);
				int num17 = num6 + num8 + num10 + num12 + num13 + num15 + num16;
				if (num17 > 0)
				{
					this.m_SamplersCache[j].m_IndexCount = sampler.m_Mesh.triangles.Length;
					this.m_SamplersCache[j].m_VertexCount = sampler.m_Mesh.vertices.Length;
					this.m_SamplersCache[j].m_BoneCount = num5;
					if (this.m_SamplersCache[j].m_Data != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(this.m_SamplersCache[j].m_Data);
					}
					this.m_SamplersCache[j].m_Data = Marshal.AllocHGlobal(num17);
					this.UpdateMesh(this.m_SamplersCache[j].m_Data, triangles, sampler.m_Mesh, sampler.m_SamplingChannels, flag4);
					this.m_SamplersCache[j].m_MeshChanged = 0x2A;
					this.m_SamplersCache[j].m_HashCode = sampler.m_MeshHashCode;
					sampler.m_SkinnedMeshData = null;
				}
				IL_969:
				sampler.m_TextureChanged = false;
				this.m_SamplersCache[j].m_Type2 = this.m_FxSamplersList[j].m_ShapeType;
				this.m_SamplersCache[j].m_SizeX = sampler.m_Dimensions.x;
				this.m_SamplersCache[j].m_SizeY = sampler.m_Dimensions.y;
				this.m_SamplersCache[j].m_SizeZ = sampler.m_Dimensions.z;
				this.m_SamplersCache[j].m_PosX = sampler.m_ShapeCenter.x;
				this.m_SamplersCache[j].m_PosY = sampler.m_ShapeCenter.y;
				this.m_SamplersCache[j].m_PosZ = sampler.m_ShapeCenter.z;
				this.m_SamplersCache[j].m_EulX = sampler.m_EulerOrientation.x;
				this.m_SamplersCache[j].m_EulY = sampler.m_EulerOrientation.y;
				this.m_SamplersCache[j].m_EulZ = sampler.m_EulerOrientation.z;
				num2 = j;
				goto IL_AA8;
				IL_8BB:
				if (this.m_SamplersCache[j].m_Data != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.m_SamplersCache[j].m_Data);
					this.m_SamplersCache[j].m_Data = IntPtr.Zero;
				}
				this.m_SamplersCache[j].m_IndexCount = 0;
				this.m_SamplersCache[j].m_VertexCount = 0;
				this.m_SamplersCache[j].m_MeshChanged = 0x2A;
				this.m_SamplersCache[j].m_HashCode = 0;
				this.m_SamplersCache[j].m_BoneCount = 0;
				goto IL_969;
				IL_4D8:
				flag5 = true;
				goto IL_4D9;
			}
			if (this.m_SamplersCache[j].m_Type1 == 2)
			{
				if (sampler.m_TextureChanged || sampler.m_TextureTexcoordMode != (PKFxManager.ETexcoordMode)this.m_SamplersCache[j].m_PosX)
				{
					goto IL_B2C;
				}
				if (forceUpdate)
				{
					goto IL_B2C;
				}
				if (this.m_ForceUpdateAttributes)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						goto IL_B2C;
					}
				}
				goto IL_165A;
				IL_B2C:
				if (sampler.m_Texture == null)
				{
					this.m_SamplersCache[j].m_SizeX = 0f;
					this.m_SamplersCache[j].m_SizeY = 0f;
					if (this.m_SamplersCache[j].m_Data != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(this.m_SamplersCache[j].m_Data);
					}
					this.m_SamplersCache[j].m_Data = IntPtr.Zero;
					int num18 = 0;
					this.m_SamplersCache[j].m_SizeZ = (float)num18;
					this.m_SamplersCache[j].m_PosX = 0f;
				}
				else
				{
					byte[] rawTextureData = sampler.m_Texture.GetRawTextureData();
					if (rawTextureData.Length == 0)
					{
						Debug.LogError("[PKFX] Sampler " + sampler.m_Descriptor.Name + " : Could not get raw texture data. Enable read/write in import settings.");
					}
					if (sampler.m_Texture.format == TextureFormat.DXT1)
					{
						this.m_SamplersCache[j].m_Type2 = 8;
					}
					else if (sampler.m_Texture.format == TextureFormat.DXT5)
					{
						this.m_SamplersCache[j].m_Type2 = 0xC;
					}
					else if (sampler.m_Texture.format == TextureFormat.ARGB32)
					{
						PKFxFX.PKImageConverter.ARGB2BGRA(ref rawTextureData);
						this.m_SamplersCache[j].m_Type2 = 4;
					}
					else if (sampler.m_Texture.format == TextureFormat.RGBA32)
					{
						PKFxFX.PKImageConverter.RGBA2BGRA(ref rawTextureData);
						this.m_SamplersCache[j].m_Type2 = 4;
					}
					else if (sampler.m_Texture.format == TextureFormat.BGRA32)
					{
						this.m_SamplersCache[j].m_Type2 = 4;
					}
					else if (sampler.m_Texture.format == TextureFormat.RGB24)
					{
						PKFxFX.PKImageConverter.RGB2BGR(ref rawTextureData);
						this.m_SamplersCache[j].m_Type2 = 3;
					}
					else if (sampler.m_Texture.format == TextureFormat.PVRTC_RGB4)
					{
						this.m_SamplersCache[j].m_Type2 = 0x14;
					}
					else if (sampler.m_Texture.format == TextureFormat.PVRTC_RGBA4)
					{
						this.m_SamplersCache[j].m_Type2 = 0x16;
					}
					else if (sampler.m_Texture.format == TextureFormat.PVRTC_RGB2)
					{
						this.m_SamplersCache[j].m_Type2 = 0x15;
					}
					else if (sampler.m_Texture.format == TextureFormat.PVRTC_RGBA2)
					{
						this.m_SamplersCache[j].m_Type2 = 0x17;
					}
					else if (sampler.m_Texture.format == TextureFormat.ETC_RGB4)
					{
						this.m_SamplersCache[j].m_Type2 = 0x10;
					}
					else if (sampler.m_Texture.format == TextureFormat.ETC2_RGB)
					{
						this.m_SamplersCache[j].m_Type2 = 0x11;
					}
					else if (sampler.m_Texture.format == TextureFormat.ETC2_RGBA8)
					{
						this.m_SamplersCache[j].m_Type2 = 0x12;
					}
					else if (sampler.m_Texture.format == TextureFormat.ETC2_RGBA1)
					{
						this.m_SamplersCache[j].m_Type2 = 0x13;
					}
					else
					{
						this.m_SamplersCache[j].m_Type2 = 0;
						Debug.LogError(string.Concat(new object[]
						{
							"[PKFX] Sampler ",
							sampler.m_Descriptor.Name,
							" texture format not supported : ",
							sampler.m_Texture.format
						}));
					}
					this.m_SamplersCache[j].m_SizeX = (float)sampler.m_Texture.width;
					this.m_SamplersCache[j].m_SizeY = (float)sampler.m_Texture.height;
					if (this.m_SamplersCache[j].m_Data != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(this.m_SamplersCache[j].m_Data);
					}
					int num19 = rawTextureData.Length;
					this.m_SamplersCache[j].m_Data = Marshal.AllocHGlobal(num19);
					Marshal.Copy(rawTextureData, 0, this.m_SamplersCache[j].m_Data, num19);
					this.m_SamplersCache[j].m_SizeZ = (float)num19;
					this.m_SamplersCache[j].m_PosX = (float)sampler.m_TextureTexcoordMode;
				}
				sampler.m_TextureChanged = false;
				num2 = j;
			}
			else if (this.m_SamplersCache[j].m_Type1 == 1)
			{
				int num20 = (sampler.m_CurvesTimeKeys != null) ? sampler.m_CurvesTimeKeys.Length : 0;
				if (num20 != 0)
				{
					goto IL_112F;
				}
				if ((float)num20 == this.m_SamplersCache[j].m_SizeX)
				{
					goto IL_112F;
				}
				if (this.m_SamplersCurvesDataGCH.IsAllocated)
				{
					this.m_SamplersCurvesDataGCH.Free();
				}
				this.m_SamplersCurvesDataCache = null;
				this.m_SamplersCache[j].m_Data = IntPtr.Zero;
				this.m_SamplersCache[j].m_SizeX = 0f;
				num2 = j;
				IL_1488:
				goto IL_165A;
				IL_112F:
				if (sampler.m_CurvesArray != null)
				{
					int num21 = sampler.m_CurvesArray.Length;
					int num22 = 1 + num21 * 3;
					bool flag6 = (float)num20 != this.m_SamplersCache[j].m_SizeX;
					if (!flag6 && !forceUpdate)
					{
						if (!this.m_ForceUpdateAttributes)
						{
							for (int k = 0; k < num20; k++)
							{
								if (this.m_SamplersCache[j].m_Data != IntPtr.Zero)
								{
									int num23 = num3 + k * num22;
									float num24 = sampler.m_CurvesTimeKeys[k];
									if (num24 != this.m_SamplersCurvesDataCache[num23])
									{
										flag6 = true;
										goto IL_120D;
									}
								}
							}
						}
					}
					IL_120D:
					if (!flag6)
					{
						if (!forceUpdate)
						{
							if (!this.m_ForceUpdateAttributes)
							{
								for (int l = 0; l < sampler.m_CurvesArray.Length; l++)
								{
									if (this.m_SamplersCache[j].m_Data != IntPtr.Zero)
									{
										AnimationCurve animationCurve = sampler.m_CurvesArray[l];
										int m = 0;
										while (m < animationCurve.keys.Length)
										{
											int num25 = num3 + m * num22 + 1 + l * 3;
											Keyframe keyframe = animationCurve.keys[m];
											if (keyframe.value == this.m_SamplersCurvesDataCache[num25] && keyframe.inTangent == this.m_SamplersCurvesDataCache[num25 + 1])
											{
												if (keyframe.outTangent == this.m_SamplersCurvesDataCache[num25 + 2])
												{
													m++;
													continue;
												}
											}
											flag6 = true;
											break;
										}
									}
								}
							}
						}
					}
					if (!flag6)
					{
						if (!forceUpdate)
						{
							if (!this.m_ForceUpdateAttributes)
							{
								goto IL_1488;
							}
						}
					}
					this.m_SamplersCache[j].m_Data = new IntPtr(this.m_SamplersCurvesDataHandler.ToInt64() + (long)num3);
					for (int n = 0; n < sampler.m_CurvesTimeKeys.Length; n++)
					{
						int num26 = num3 + n * num22;
						this.m_SamplersCurvesDataCache[num26] = sampler.m_CurvesTimeKeys[n];
						for (int num27 = 0; num27 < sampler.m_CurvesArray.Length; num27++)
						{
							AnimationCurve animationCurve2 = sampler.m_CurvesArray[num27];
							int num28 = num26 + 1 + num27 * 3;
							Keyframe keyframe2 = animationCurve2.keys[n];
							this.m_SamplersCurvesDataCache[num28] = keyframe2.value;
							this.m_SamplersCurvesDataCache[num28 + 1] = keyframe2.inTangent;
							this.m_SamplersCurvesDataCache[num28 + 2] = keyframe2.outTangent;
						}
					}
					num3 += num20 * num22;
					this.m_SamplersCache[j].m_SizeX = (float)num20;
					this.m_SamplersCache[j].m_SizeY = (float)sampler.m_CurvesArray.Length;
					num2 = j;
				}
			}
			else if (this.m_SamplersCache[j].m_Type1 == 3)
			{
				bool flag6 = (float)sampler.m_Text.Length != this.m_SamplersCache[j].m_SizeX;
				if (!flag6)
				{
					if (sampler.m_Text.Length > 0)
					{
						if (this.m_SamplersCache[j].m_Data == IntPtr.Zero)
						{
							flag6 = true;
						}
					}
				}
				if (!flag6)
				{
					if (this.m_SamplersCache[j].m_Data != IntPtr.Zero && Marshal.PtrToStringAnsi(this.m_SamplersCache[j].m_Data) != sampler.m_Text)
					{
						flag6 = true;
					}
				}
				if (!flag6)
				{
					if (!forceUpdate)
					{
						if (!this.m_ForceUpdateAttributes)
						{
							goto IL_165A;
						}
					}
				}
				if (this.m_SamplersCache[j].m_Data != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.m_SamplersCache[j].m_Data);
					this.m_SamplersCache[j].m_Data = IntPtr.Zero;
				}
				int length = sampler.m_Text.Length;
				if (length > 0)
				{
					this.m_SamplersCache[j].m_Data = Marshal.StringToHGlobalAnsi(sampler.m_Text);
				}
				this.m_SamplersCache[j].m_SizeX = (float)length;
				num2 = j;
			}
			IL_165A:;
		}
		if (num2 >= 0)
		{
			if (!PKFxManager.EffectSetSamplers(this.m_FXGUID, num2 + 1, this.m_SamplersHandler))
			{
				Debug.LogError("[PKFX] Sampler through pinned memory failed.");
				Debug.LogError("[PKFX] Did you try to change an FX without stopping it beforehand?");
			}
		}
		for (int num29 = 0; num29 < this.m_FxSamplersList.Count; num29++)
		{
			PKFxManager.Sampler sampler2 = this.m_FxSamplersList[num29];
			bool flag7;
			if (this.m_FxSamplersList[num29].m_ShapeType == 5)
			{
				flag7 = (this.m_FxSamplersList[num29].m_SkinnedMeshRenderer != null);
			}
			else
			{
				flag7 = false;
			}
			bool flag8 = flag7;
			if (flag8)
			{
				int num30;
				if (sampler2.m_SkinnedMeshRenderer != null)
				{
					num30 = sampler2.m_SkinnedMeshRenderer.bones.Length;
				}
				else
				{
					num30 = 0;
				}
				int num31 = num30;
				if (sampler2.m_SkinnedMeshData == null)
				{
					int cb = (num31 == 0) ? 0 : (sampler2.m_SkinnedMeshRenderer.bones.Length * 4 * 0x10);
					if (this.m_SamplersCache[num29].m_Data != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(this.m_SamplersCache[num29].m_Data);
					}
					this.m_SamplersCache[num29].m_Data = Marshal.AllocHGlobal(cb);
					sampler2.m_SkinnedMeshData = new PKFxManager.Sampler.SkinnedMeshData();
					sampler2.m_SkinnedMeshData.InitData(sampler2.m_SkinnedMeshRenderer);
				}
				this.UpdateBones(this.m_SamplersCache[num29].m_Data, sampler2);
				if (!PKFxManager.EffectUpdateSamplerSkinning(this.m_FXGUID, num29, this.m_SamplersHandler, Time.deltaTime))
				{
					Debug.LogError("[PKFX] Skinning through pinned memory failed.");
				}
			}
		}
		this.m_ForceUpdateAttributes = false;
	}

	public PKFxManager.Sampler GetSamplerFromDesc(PKFxManager.SamplerDesc desc)
	{
		using (List<PKFxManager.Sampler>.Enumerator enumerator = this.m_FxSamplersList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.Sampler sampler = enumerator.Current;
				if (sampler.m_Descriptor.Name == desc.Name && sampler.m_Descriptor.Type == desc.Type)
				{
					return sampler;
				}
			}
		}
		return null;
	}

	public void DeleteAttribute(PKFxManager.AttributeDesc desc)
	{
		using (List<PKFxManager.Attribute>.Enumerator enumerator = this.m_FxAttributesList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.Attribute attribute = enumerator.Current;
				if (attribute.m_Descriptor.Name == desc.Name && attribute.m_Descriptor.Type == desc.Type)
				{
					this.m_FxAttributesList.Remove(attribute);
					return;
				}
			}
		}
	}

	public PKFxManager.Attribute GetAttributeFromDesc(PKFxManager.AttributeDesc desc)
	{
		using (List<PKFxManager.Attribute>.Enumerator enumerator = this.m_FxAttributesList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.Attribute attribute = enumerator.Current;
				if (attribute.m_Descriptor.Name == desc.Name && attribute.m_Descriptor.Type == desc.Type)
				{
					attribute.m_Descriptor = desc;
					return attribute;
				}
			}
		}
		return null;
	}

	public bool SamplerExists(PKFxManager.SamplerDesc desc)
	{
		foreach (PKFxManager.Sampler sampler in this.m_FxSamplersList)
		{
			if (sampler.m_Descriptor.Name == desc.Name)
			{
				if (sampler.m_Descriptor.Type == desc.Type)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool AttributeExists(PKFxManager.AttributeDesc desc)
	{
		using (List<PKFxManager.Attribute>.Enumerator enumerator = this.m_FxAttributesList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.Attribute attribute = enumerator.Current;
				if (attribute.m_Descriptor.Name == desc.Name)
				{
					if (attribute.m_Descriptor.Type == desc.Type)
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
				PKFxManager.AttributeDesc attributeDesc = enumerator.Current;
				if (attr.m_Descriptor.Name == attributeDesc.Name)
				{
					if (attr.m_Descriptor.Type == attributeDesc.Type)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

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
			int i = 0;
			while (i < data.Length)
			{
				byte[] array = new byte[]
				{
					data[i + 3],
					data[i + 2],
					data[i + 1],
					data[i]
				};
				data[i++] = array[0];
				data[i++] = array[1];
				data[i++] = array[2];
				data[i++] = array[3];
			}
		}

		public unsafe static void RGBA2BGRA(ref byte[] data)
		{
			for (int i = 0; i < data.Length; i += 4)
			{
				byte b = data[i];
				data[i] = data[i + 2];
				data[i + 2] = b;
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
}
