using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public class PkFxCustomShader : ScriptableObject
{
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
		PixelPC = 10,
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
		PixelP = 10,
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

	private SShaderConstantPinned[] m_ShaderConstantsCache;

	private GCHandle m_ShaderConstantsGCH;

	private IntPtr m_ShaderConstantsHandler;

	public bool m_IsSoft;

	public bool m_HasSoftAnimBlend;

	public bool m_IsDistortion;

	public bool m_IsMesh;

	public bool m_HasMeshTexture;

	public bool m_IsGL3;

	public EShaderApi m_Api;

	public int m_VertexType;

	public int m_PixelType;

	public string m_ShaderName;

	public string m_ShaderGroup;

	public bool m_GlobalShader;

	public uint m_LoadedShaderId;

	public List<PKFxManager.ShaderConstant> m_ShaderConstantList;

	public void SetConstant(PKFxManager.ShaderConstant constant)
	{
		if (!ShaderConstantExist(constant.m_Descriptor))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("[PKFX] CustomShader.SetConstant : " + constant.m_Descriptor.Name + " doesn't exist");
					return;
				}
			}
		}
		for (int i = 0; i < m_ShaderConstantList.Count; i++)
		{
			if (m_ShaderConstantList[i].m_Descriptor.Name == constant.m_Descriptor.Name)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				m_ShaderConstantList[i].m_Value0 = constant.m_Value0;
				m_ShaderConstantList[i].m_Value1 = constant.m_Value1;
				m_ShaderConstantList[i].m_Value2 = constant.m_Value2;
				m_ShaderConstantList[i].m_Value3 = constant.m_Value3;
			}
		}
	}

	public void LoadShaderConstants(List<PKFxManager.ShaderConstantDesc> ShaderConstantsDesc, bool flushAttributes)
	{
		if (flushAttributes)
		{
			m_ShaderConstantList.Clear();
		}
		List<PKFxManager.ShaderConstant> list = new List<PKFxManager.ShaderConstant>();
		foreach (PKFxManager.ShaderConstantDesc item in ShaderConstantsDesc)
		{
			if (!ShaderConstantExist(item))
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				list.Add(new PKFxManager.ShaderConstant(item));
			}
			else
			{
				list.Add(GetShaderConstantFromDesc(item));
			}
		}
		m_ShaderConstantList = list;
	}

	private void AllocAttributesCacheIFN()
	{
		if (m_ShaderConstantsCache != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_ShaderConstantsCache.Length >= m_ShaderConstantList.Count)
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_ShaderConstantsCache = new SShaderConstantPinned[m_ShaderConstantList.Count];
		if (m_ShaderConstantsGCH.IsAllocated)
		{
			m_ShaderConstantsGCH.Free();
		}
		m_ShaderConstantsGCH = GCHandle.Alloc(m_ShaderConstantsCache, GCHandleType.Pinned);
		m_ShaderConstantsHandler = m_ShaderConstantsGCH.AddrOfPinnedObject();
	}

	public void UpdateShaderConstants(bool forceUpdate)
	{
		int num = -1;
		if (m_ShaderConstantList.Count == 0)
		{
			return;
		}
		AllocAttributesCacheIFN();
		for (int i = 0; i < m_ShaderConstantList.Count; i++)
		{
			PKFxManager.ShaderConstant shaderConstant = m_ShaderConstantList[i];
			int num2;
			if (m_ShaderConstantsCache[i].m_Value0 == shaderConstant.m_Value0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (m_ShaderConstantsCache[i].m_Value1 == shaderConstant.m_Value1)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_ShaderConstantsCache[i].m_Value2 == shaderConstant.m_Value2)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						num2 = ((m_ShaderConstantsCache[i].m_Value3 != shaderConstant.m_Value3) ? 1 : 0);
						goto IL_00c2;
					}
				}
			}
			num2 = 1;
			goto IL_00c2;
			IL_00c2:
			if (num2 == 0)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!forceUpdate)
				{
					continue;
				}
			}
			m_ShaderConstantsCache[i].m_Type = (int)m_ShaderConstantList[i].m_Descriptor.Type;
			m_ShaderConstantsCache[i].m_Value0 = shaderConstant.m_Value0;
			m_ShaderConstantsCache[i].m_Value1 = shaderConstant.m_Value1;
			m_ShaderConstantsCache[i].m_Value2 = shaderConstant.m_Value2;
			m_ShaderConstantsCache[i].m_Value3 = shaderConstant.m_Value3;
			num = i;
		}
		if (num < 0)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (!PKFxManager.ShaderSetConstant(m_LoadedShaderId, num + 1, m_ShaderConstantsHandler))
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					Debug.LogError("[PKFX] Shader constant through pinned memory failed.");
					Debug.LogError("[PKFX] Did you try to change an FX without stopping it beforehand?");
					return;
				}
			}
			return;
		}
	}

	public PKFxManager.ShaderConstant GetShaderConstantFromDesc(PKFxManager.ShaderConstantDesc desc)
	{
		if (m_ShaderConstantList == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return null;
				}
			}
		}
		foreach (PKFxManager.ShaderConstant shaderConstant in m_ShaderConstantList)
		{
			if (shaderConstant.m_Descriptor.Name == desc.Name && shaderConstant.m_Descriptor.Type == desc.Type)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return shaderConstant;
					}
				}
			}
		}
		return null;
	}

	public bool ShaderConstantExist(PKFxManager.ShaderConstantDesc desc)
	{
		if (m_ShaderConstantList == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return false;
				}
			}
		}
		using (List<PKFxManager.ShaderConstant>.Enumerator enumerator = m_ShaderConstantList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PKFxManager.ShaderConstant current = enumerator.Current;
				if (current.m_Descriptor.Name == desc.Name && current.m_Descriptor.Type == desc.Type)
				{
					return true;
				}
			}
			while (true)
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
		PKFxManager.ShaderDesc result = default(PKFxManager.ShaderDesc);
		result.ShaderGroup = m_ShaderGroup;
		result.ShaderPath = m_ShaderName;
		result.Api = (int)m_Api;
		result.VertexType = m_VertexType;
		result.PixelType = m_PixelType;
		return result;
	}

	public bool ApiInUse()
	{
		if (m_Api == EShaderApi.GL)
		{
			return SystemInfo.graphicsDeviceVersion.StartsWith("OpenGL");
		}
		if (m_Api == EShaderApi.DX9)
		{
			return SystemInfo.graphicsDeviceVersion.StartsWith("Direct3D 9");
		}
		if (m_Api == EShaderApi.DX11)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return SystemInfo.graphicsDeviceVersion.StartsWith("Direct3D 11");
				}
			}
		}
		if (m_Api == EShaderApi.GLES)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return SystemInfo.graphicsDeviceVersion.StartsWith("OpenGL ES");
				}
			}
		}
		Debug.LogError("Invalid API");
		return false;
	}
}
