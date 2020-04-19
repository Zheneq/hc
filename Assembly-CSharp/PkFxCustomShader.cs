using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public class PkFxCustomShader : ScriptableObject
{
	private PkFxCustomShader.SShaderConstantPinned[] m_ShaderConstantsCache;

	private GCHandle m_ShaderConstantsGCH;

	private IntPtr m_ShaderConstantsHandler;

	public bool m_IsSoft;

	public bool m_HasSoftAnimBlend;

	public bool m_IsDistortion;

	public bool m_IsMesh;

	public bool m_HasMeshTexture;

	public bool m_IsGL3;

	public PkFxCustomShader.EShaderApi m_Api;

	public int m_VertexType;

	public int m_PixelType;

	public string m_ShaderName;

	public string m_ShaderGroup;

	public bool m_GlobalShader;

	public uint m_LoadedShaderId;

	public List<PKFxManager.ShaderConstant> m_ShaderConstantList;

	public void SetConstant(PKFxManager.ShaderConstant constant)
	{
		if (!this.ShaderConstantExist(constant.m_Descriptor))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PkFxCustomShader.SetConstant(PKFxManager.ShaderConstant)).MethodHandle;
			}
			Debug.LogError("[PKFX] CustomShader.SetConstant : " + constant.m_Descriptor.Name + " doesn't exist");
		}
		else
		{
			for (int i = 0; i < this.m_ShaderConstantList.Count; i++)
			{
				if (this.m_ShaderConstantList[i].m_Descriptor.Name == constant.m_Descriptor.Name)
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
					this.m_ShaderConstantList[i].m_Value0 = constant.m_Value0;
					this.m_ShaderConstantList[i].m_Value1 = constant.m_Value1;
					this.m_ShaderConstantList[i].m_Value2 = constant.m_Value2;
					this.m_ShaderConstantList[i].m_Value3 = constant.m_Value3;
				}
			}
		}
	}

	public void LoadShaderConstants(List<PKFxManager.ShaderConstantDesc> ShaderConstantsDesc, bool flushAttributes)
	{
		if (flushAttributes)
		{
			this.m_ShaderConstantList.Clear();
		}
		List<PKFxManager.ShaderConstant> list = new List<PKFxManager.ShaderConstant>();
		foreach (PKFxManager.ShaderConstantDesc desc in ShaderConstantsDesc)
		{
			if (!this.ShaderConstantExist(desc))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(PkFxCustomShader.LoadShaderConstants(List<PKFxManager.ShaderConstantDesc>, bool)).MethodHandle;
				}
				list.Add(new PKFxManager.ShaderConstant(desc));
			}
			else
			{
				list.Add(this.GetShaderConstantFromDesc(desc));
			}
		}
		this.m_ShaderConstantList = list;
	}

	private void AllocAttributesCacheIFN()
	{
		if (this.m_ShaderConstantsCache != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PkFxCustomShader.AllocAttributesCacheIFN()).MethodHandle;
			}
			if (this.m_ShaderConstantsCache.Length >= this.m_ShaderConstantList.Count)
			{
				return;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_ShaderConstantsCache = new PkFxCustomShader.SShaderConstantPinned[this.m_ShaderConstantList.Count];
		if (this.m_ShaderConstantsGCH.IsAllocated)
		{
			this.m_ShaderConstantsGCH.Free();
		}
		this.m_ShaderConstantsGCH = GCHandle.Alloc(this.m_ShaderConstantsCache, GCHandleType.Pinned);
		this.m_ShaderConstantsHandler = this.m_ShaderConstantsGCH.AddrOfPinnedObject();
	}

	public void UpdateShaderConstants(bool forceUpdate)
	{
		int num = -1;
		if (this.m_ShaderConstantList.Count == 0)
		{
			return;
		}
		this.AllocAttributesCacheIFN();
		int i = 0;
		while (i < this.m_ShaderConstantList.Count)
		{
			PKFxManager.ShaderConstant shaderConstant = this.m_ShaderConstantList[i];
			if (this.m_ShaderConstantsCache[i].m_Value0 != shaderConstant.m_Value0)
			{
				goto IL_C1;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PkFxCustomShader.UpdateShaderConstants(bool)).MethodHandle;
			}
			if (this.m_ShaderConstantsCache[i].m_Value1 != shaderConstant.m_Value1)
			{
				goto IL_C1;
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
			if (this.m_ShaderConstantsCache[i].m_Value2 != shaderConstant.m_Value2)
			{
				goto IL_C1;
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
			bool flag = this.m_ShaderConstantsCache[i].m_Value3 != shaderConstant.m_Value3;
			IL_C2:
			if (flag)
			{
				goto IL_D6;
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
			if (forceUpdate)
			{
				goto IL_D6;
			}
			IL_15D:
			i++;
			continue;
			IL_D6:
			this.m_ShaderConstantsCache[i].m_Type = (int)this.m_ShaderConstantList[i].m_Descriptor.Type;
			this.m_ShaderConstantsCache[i].m_Value0 = shaderConstant.m_Value0;
			this.m_ShaderConstantsCache[i].m_Value1 = shaderConstant.m_Value1;
			this.m_ShaderConstantsCache[i].m_Value2 = shaderConstant.m_Value2;
			this.m_ShaderConstantsCache[i].m_Value3 = shaderConstant.m_Value3;
			num = i;
			goto IL_15D;
			IL_C1:
			flag = true;
			goto IL_C2;
		}
		if (num >= 0)
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
			if (!PKFxManager.ShaderSetConstant(this.m_LoadedShaderId, num + 1, this.m_ShaderConstantsHandler))
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
				Debug.LogError("[PKFX] Shader constant through pinned memory failed.");
				Debug.LogError("[PKFX] Did you try to change an FX without stopping it beforehand?");
			}
		}
	}

	public PKFxManager.ShaderConstant GetShaderConstantFromDesc(PKFxManager.ShaderConstantDesc desc)
	{
		if (this.m_ShaderConstantList == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PkFxCustomShader.GetShaderConstantFromDesc(PKFxManager.ShaderConstantDesc)).MethodHandle;
			}
			return null;
		}
		foreach (PKFxManager.ShaderConstant shaderConstant in this.m_ShaderConstantList)
		{
			if (shaderConstant.m_Descriptor.Name == desc.Name && shaderConstant.m_Descriptor.Type == desc.Type)
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
				return shaderConstant;
			}
		}
		return null;
	}

	public bool ShaderConstantExist(PKFxManager.ShaderConstantDesc desc)
	{
		if (this.m_ShaderConstantList == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PkFxCustomShader.ShaderConstantExist(PKFxManager.ShaderConstantDesc)).MethodHandle;
			}
			return false;
		}
		using (List<PKFxManager.ShaderConstant>.Enumerator enumerator = this.m_ShaderConstantList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.ShaderConstant shaderConstant = enumerator.Current;
				if (shaderConstant.m_Descriptor.Name == desc.Name && shaderConstant.m_Descriptor.Type == desc.Type)
				{
					return true;
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}

	public PKFxManager.ShaderDesc GetDesc()
	{
		PKFxManager.ShaderDesc result;
		result.ShaderGroup = this.m_ShaderGroup;
		result.ShaderPath = this.m_ShaderName;
		result.Api = (int)this.m_Api;
		result.VertexType = this.m_VertexType;
		result.PixelType = this.m_PixelType;
		return result;
	}

	public bool ApiInUse()
	{
		if (this.m_Api == PkFxCustomShader.EShaderApi.GL)
		{
			return SystemInfo.graphicsDeviceVersion.StartsWith("OpenGL");
		}
		if (this.m_Api == PkFxCustomShader.EShaderApi.DX9)
		{
			return SystemInfo.graphicsDeviceVersion.StartsWith("Direct3D 9");
		}
		if (this.m_Api == PkFxCustomShader.EShaderApi.DX11)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PkFxCustomShader.ApiInUse()).MethodHandle;
			}
			return SystemInfo.graphicsDeviceVersion.StartsWith("Direct3D 11");
		}
		if (this.m_Api == PkFxCustomShader.EShaderApi.GLES)
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
			return SystemInfo.graphicsDeviceVersion.StartsWith("OpenGL ES");
		}
		Debug.LogError("Invalid API");
		return false;
	}

	public enum EVertexShaderTypeDX9
	{
		VertexP,
		VertexPC,
		VertexPCT,
		VertexPCTA,
		VertexRibbon,
		VertexPC_Mesh,
		VertexPCD_Mesh,
		VertexMAX
	}

	public enum EPixelShaderTypeDX9
	{
		PixelP = 7,
		PixelPC,
		PixelPCT,
		PixelPCTA,
		PixelPCT_Soft,
		PixelPCTA_Soft,
		PixelD,
		PixelDA,
		PixelRibbon,
		PixelPC_Mesh,
		PixelPCD_Mesh
	}

	public enum EVertexShaderTypeDX11
	{
		VertexPCT,
		VertexPC,
		VertexPCTA,
		VertexPCTA_Depth,
		VertexPCTN,
		VertexPCTNA,
		VertexPCT_Depth,
		VertexRibbon,
		VertexPC_Mesh,
		VertexPCD_Mesh,
		VertexMAX
	}

	public enum EPixelShaderTypeDX11
	{
		PixelPC = 0xA,
		PixelPCT,
		PixelPCTA,
		PixelPCTA_Distortion,
		PixelPCTA_Soft,
		PixelPCTL,
		PixelPCTLA,
		PixelPCT_Distortion,
		PixelPCT_Soft,
		PixelRibbon,
		PixelPC_Mesh,
		PixelPCD_Mesh
	}

	public enum EVertexShaderTypeGL
	{
		VertexP,
		VertexPC,
		VertexPCT,
		VertexPCTA,
		VertexPCTA_Soft,
		VertexPCT_Soft,
		VertexRibbon_GL3,
		VertexRibbon_GL2,
		VertexPC_Mesh_GL3,
		VertexPC_Mesh_GL2,
		VertexMAX
	}

	public enum EPixelShaderTypeGL
	{
		PixelP = 0xA,
		PixelPC,
		PixelPCT,
		PixelPCTA,
		PixelPCTA_Dist,
		PixelPCTA_Soft,
		PixelPCT_Dist,
		PixelPCT_Soft,
		PixelRibbon_GL3,
		PixelRibbon_GL2
	}

	public enum EVertexShaderTypeGLES
	{
		VertexP,
		VertexPC,
		VertexPCT,
		VertexPCTA,
		VertexPCTA_Soft,
		VertexPCT_Soft,
		VertexRibbon,
		VertexPC_Mesh,
		VertexMAX
	}

	public enum EPixelShaderTypeGLES
	{
		PixelP = 8,
		PixelPC,
		PixelPCT,
		PixelPCTA,
		PixelPCTA_Dist,
		PixelPCTA_Soft,
		PixelPCT_Dist,
		PixelPCT_Soft,
		PixelRibbon
	}

	public enum EShaderApi
	{
		DX9,
		DX11,
		GL,
		GLES
	}

	private struct SShaderConstantPinned
	{
		public int m_Type;

		public float m_Value0;

		public float m_Value1;

		public float m_Value2;

		public float m_Value3;
	}
}
