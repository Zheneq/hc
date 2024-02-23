using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	public static class TMP_MaterialManager
	{
		private class FallbackMaterial
		{
			public int baseID;

			public Material baseMaterial;

			public long fallbackID;

			public Material fallbackMaterial;

			public int count;
		}

		private class MaskingMaterial
		{
			public Material baseMaterial;

			public Material stencilMaterial;

			public int count;

			public int stencilID;
		}

		private static List<MaskingMaterial> m_materialList;

		private static Dictionary<long, FallbackMaterial> m_fallbackMaterials;

		private static Dictionary<int, long> m_fallbackMaterialLookup;

		private static List<FallbackMaterial> m_fallbackCleanupList;

		private static bool isFallbackListDirty;

		static TMP_MaterialManager()
		{
			m_materialList = new List<MaskingMaterial>();
			m_fallbackMaterials = new Dictionary<long, FallbackMaterial>();
			m_fallbackMaterialLookup = new Dictionary<int, long>();
			m_fallbackCleanupList = new List<FallbackMaterial>();
			Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(OnPreRender));
			Canvas.willRenderCanvases += OnPreRenderCanvas;
		}

		private static void OnPreRender(Camera cam)
		{
			if (!isFallbackListDirty)
			{
				return;
			}
			while (true)
			{
				CleanupFallbackMaterials();
				isFallbackListDirty = false;
				return;
			}
		}

		private static void OnPreRenderCanvas()
		{
			if (!isFallbackListDirty)
			{
				return;
			}
			while (true)
			{
				CleanupFallbackMaterials();
				isFallbackListDirty = false;
				return;
			}
		}

		public static Material GetStencilMaterial(Material baseMaterial, int stencilID)
		{
			if (!baseMaterial.HasProperty(ShaderUtilities.ID_StencilID))
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						Debug.LogWarning("Selected Shader does not support Stencil Masking. Please select the Distance Field or Mobile Distance Field Shader.");
						return baseMaterial;
					}
				}
			}
			int instanceID = baseMaterial.GetInstanceID();
			for (int i = 0; i < m_materialList.Count; i++)
			{
				if (m_materialList[i].baseMaterial.GetInstanceID() == instanceID)
				{
					if (m_materialList[i].stencilID == stencilID)
					{
						m_materialList[i].count++;
						return m_materialList[i].stencilMaterial;
					}
				}
			}
			while (true)
			{
				Material material = new Material(baseMaterial);
				material.hideFlags = HideFlags.HideAndDontSave;
				material.shaderKeywords = baseMaterial.shaderKeywords;
				ShaderUtilities.GetShaderPropertyIDs();
				material.SetFloat(ShaderUtilities.ID_StencilID, stencilID);
				material.SetFloat(ShaderUtilities.ID_StencilComp, 4f);
				MaskingMaterial maskingMaterial = new MaskingMaterial();
				maskingMaterial.baseMaterial = baseMaterial;
				maskingMaterial.stencilMaterial = material;
				maskingMaterial.stencilID = stencilID;
				maskingMaterial.count = 1;
				m_materialList.Add(maskingMaterial);
				return material;
			}
		}

		public static void ReleaseStencilMaterial(Material stencilMaterial)
		{
			int instanceID = stencilMaterial.GetInstanceID();
			for (int i = 0; i < m_materialList.Count; i++)
			{
				if (m_materialList[i].stencilMaterial.GetInstanceID() != instanceID)
				{
					continue;
				}
				while (true)
				{
					if (m_materialList[i].count > 1)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								m_materialList[i].count--;
								return;
							}
						}
					}
					UnityEngine.Object.DestroyImmediate(m_materialList[i].stencilMaterial);
					m_materialList.RemoveAt(i);
					stencilMaterial = null;
					return;
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

		public static Material GetBaseMaterial(Material stencilMaterial)
		{
			int num = m_materialList.FindIndex((MaskingMaterial item) => item.stencilMaterial == stencilMaterial);
			if (num == -1)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return null;
					}
				}
			}
			return m_materialList[num].baseMaterial;
		}

		public static Material SetStencil(Material material, int stencilID)
		{
			material.SetFloat(ShaderUtilities.ID_StencilID, stencilID);
			if (stencilID == 0)
			{
				material.SetFloat(ShaderUtilities.ID_StencilComp, 8f);
			}
			else
			{
				material.SetFloat(ShaderUtilities.ID_StencilComp, 4f);
			}
			return material;
		}

		public static void AddMaskingMaterial(Material baseMaterial, Material stencilMaterial, int stencilID)
		{
			int num = m_materialList.FindIndex((MaskingMaterial item) => item.stencilMaterial == stencilMaterial);
			if (num == -1)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
					{
						MaskingMaterial maskingMaterial = new MaskingMaterial();
						maskingMaterial.baseMaterial = baseMaterial;
						maskingMaterial.stencilMaterial = stencilMaterial;
						maskingMaterial.stencilID = stencilID;
						maskingMaterial.count = 1;
						m_materialList.Add(maskingMaterial);
						return;
					}
					}
				}
			}
			stencilMaterial = m_materialList[num].stencilMaterial;
			m_materialList[num].count++;
		}

		public static void RemoveStencilMaterial(Material stencilMaterial)
		{
			int num = m_materialList.FindIndex((MaskingMaterial item) => item.stencilMaterial == stencilMaterial);
			if (num == -1)
			{
				return;
			}
			while (true)
			{
				m_materialList.RemoveAt(num);
				return;
			}
		}

		public static void ReleaseBaseMaterial(Material baseMaterial)
		{
			int num = m_materialList.FindIndex((MaskingMaterial item) => item.baseMaterial == baseMaterial);
			if (num == -1)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						Debug.Log(new StringBuilder().Append("No Masking Material exists for ").Append(baseMaterial.name).ToString());
						return;
					}
				}
			}
			if (m_materialList[num].count > 1)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						m_materialList[num].count--;
						Debug.Log(new StringBuilder().Append("Removed (1) reference to ").Append(m_materialList[num].stencilMaterial.name).Append(". There are ").Append(m_materialList[num].count).Append(" references left.").ToString());
						return;
					}
				}
			}
			Debug.Log(new StringBuilder().Append("Removed last reference to ").Append(m_materialList[num].stencilMaterial.name).Append(" with ID ").Append(m_materialList[num].stencilMaterial.GetInstanceID()).ToString());
			UnityEngine.Object.DestroyImmediate(m_materialList[num].stencilMaterial);
			m_materialList.RemoveAt(num);
		}

		public static void ClearMaterials()
		{
			if (m_materialList.Count == 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						Debug.Log("Material List has already been cleared.");
						return;
					}
				}
			}
			for (int i = 0; i < m_materialList.Count; i++)
			{
				Material stencilMaterial = m_materialList[i].stencilMaterial;
				UnityEngine.Object.DestroyImmediate(stencilMaterial);
				m_materialList.RemoveAt(i);
			}
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}

		public static int GetStencilID(GameObject obj)
		{
			int num = 0;
			Transform transform = obj.transform;
			Transform y = FindRootSortOverrideCanvas(transform);
			if (transform == y)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return num;
					}
				}
			}
			Transform parent = transform.parent;
			List<Mask> list = TMP_ListPool<Mask>.Get();
			while (true)
			{
				if (parent != null)
				{
					parent.GetComponents(list);
					int num2 = 0;
					while (true)
					{
						if (num2 < list.Count)
						{
							Mask mask = list[num2];
							if (mask != null && mask.MaskEnabled() && mask.graphic.IsActive())
							{
								num++;
								break;
							}
							num2++;
							continue;
						}
						break;
					}
					if (parent == y)
					{
						break;
					}
					parent = parent.parent;
					continue;
				}
				break;
			}
			TMP_ListPool<Mask>.Release(list);
			return Mathf.Min((1 << num) - 1, 255);
		}

		public static Material GetMaterialForRendering(MaskableGraphic graphic, Material baseMaterial)
		{
			if (baseMaterial == null)
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
			List<IMaterialModifier> list = TMP_ListPool<IMaterialModifier>.Get();
			graphic.GetComponents(list);
			Material material = baseMaterial;
			for (int i = 0; i < list.Count; i++)
			{
				material = list[i].GetModifiedMaterial(material);
			}
			TMP_ListPool<IMaterialModifier>.Release(list);
			return material;
		}

		private static Transform FindRootSortOverrideCanvas(Transform start)
		{
			List<Canvas> list = TMP_ListPool<Canvas>.Get();
			start.GetComponentsInParent(false, list);
			Canvas canvas = null;
			int num = 0;
			while (true)
			{
				if (num < list.Count)
				{
					canvas = list[num];
					if (canvas.overrideSorting)
					{
						break;
					}
					num++;
					continue;
				}
				break;
			}
			TMP_ListPool<Canvas>.Release(list);
			object result;
			if (canvas != null)
			{
				result = canvas.transform;
			}
			else
			{
				result = null;
			}
			return (Transform)result;
		}

		public static Material GetFallbackMaterial(Material sourceMaterial, Material targetMaterial)
		{
			int instanceID = sourceMaterial.GetInstanceID();
			Texture texture = targetMaterial.GetTexture(ShaderUtilities.ID_MainTex);
			int instanceID2 = texture.GetInstanceID();
			long num = ((long)instanceID << 32) | (uint)instanceID2;
			FallbackMaterial value;
			if (m_fallbackMaterials.TryGetValue(num, out value))
			{
				while (true)
				{
					return value.fallbackMaterial;
				}
			}
			Material material = null;
			if (sourceMaterial.HasProperty(ShaderUtilities.ID_GradientScale))
			{
				if (targetMaterial.HasProperty(ShaderUtilities.ID_GradientScale))
				{
					material = new Material(sourceMaterial);
					material.hideFlags = HideFlags.HideAndDontSave;
					material.SetTexture(ShaderUtilities.ID_MainTex, texture);
					material.SetFloat(ShaderUtilities.ID_GradientScale, targetMaterial.GetFloat(ShaderUtilities.ID_GradientScale));
					material.SetFloat(ShaderUtilities.ID_TextureWidth, targetMaterial.GetFloat(ShaderUtilities.ID_TextureWidth));
					material.SetFloat(ShaderUtilities.ID_TextureHeight, targetMaterial.GetFloat(ShaderUtilities.ID_TextureHeight));
					material.SetFloat(ShaderUtilities.ID_WeightNormal, targetMaterial.GetFloat(ShaderUtilities.ID_WeightNormal));
					material.SetFloat(ShaderUtilities.ID_WeightBold, targetMaterial.GetFloat(ShaderUtilities.ID_WeightBold));
					goto IL_0129;
				}
			}
			material = new Material(targetMaterial);
			goto IL_0129;
			IL_0129:
			value = new FallbackMaterial();
			value.baseID = instanceID;
			value.baseMaterial = sourceMaterial;
			value.fallbackID = num;
			value.fallbackMaterial = material;
			value.count = 0;
			m_fallbackMaterials.Add(num, value);
			m_fallbackMaterialLookup.Add(material.GetInstanceID(), num);
			return material;
		}

		public static void AddFallbackMaterialReference(Material targetMaterial)
		{
			if (targetMaterial == null)
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
			int instanceID = targetMaterial.GetInstanceID();
			FallbackMaterial value2;
			long value;
			if (!m_fallbackMaterialLookup.TryGetValue(instanceID, out value) || !m_fallbackMaterials.TryGetValue(value, out value2))
			{
				return;
			}
			while (true)
			{
				value2.count++;
				return;
			}
		}

		public static void RemoveFallbackMaterialReference(Material targetMaterial)
		{
			if (targetMaterial == null)
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
			int instanceID = targetMaterial.GetInstanceID();
			long value;
			if (!m_fallbackMaterialLookup.TryGetValue(instanceID, out value))
			{
				return;
			}
			while (true)
			{
				FallbackMaterial value2;
				if (!m_fallbackMaterials.TryGetValue(value, out value2))
				{
					return;
				}
				while (true)
				{
					value2.count--;
					if (value2.count < 1)
					{
						m_fallbackCleanupList.Add(value2);
					}
					return;
				}
			}
		}

		public static void CleanupFallbackMaterials()
		{
			if (m_fallbackCleanupList.Count == 0)
			{
				return;
			}
			for (int i = 0; i < m_fallbackCleanupList.Count; i++)
			{
				FallbackMaterial fallbackMaterial = m_fallbackCleanupList[i];
				if (fallbackMaterial.count < 1)
				{
					Material fallbackMaterial2 = fallbackMaterial.fallbackMaterial;
					m_fallbackMaterials.Remove(fallbackMaterial.fallbackID);
					m_fallbackMaterialLookup.Remove(fallbackMaterial2.GetInstanceID());
					UnityEngine.Object.DestroyImmediate(fallbackMaterial2);
					fallbackMaterial2 = null;
				}
			}
			m_fallbackCleanupList.Clear();
		}

		public static void ReleaseFallbackMaterial(Material fallackMaterial)
		{
			if (fallackMaterial == null)
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
			int instanceID = fallackMaterial.GetInstanceID();
			long value;
			if (m_fallbackMaterialLookup.TryGetValue(instanceID, out value))
			{
				FallbackMaterial value2;
				if (m_fallbackMaterials.TryGetValue(value, out value2))
				{
					value2.count--;
					if (value2.count < 1)
					{
						m_fallbackCleanupList.Add(value2);
					}
				}
			}
			isFallbackListDirty = true;
		}

		public static void CopyMaterialPresetProperties(Material source, Material destination)
		{
			if (!source.HasProperty(ShaderUtilities.ID_GradientScale))
			{
				return;
			}
			while (true)
			{
				if (!destination.HasProperty(ShaderUtilities.ID_GradientScale))
				{
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
				Texture texture = destination.GetTexture(ShaderUtilities.ID_MainTex);
				float @float = destination.GetFloat(ShaderUtilities.ID_GradientScale);
				float float2 = destination.GetFloat(ShaderUtilities.ID_TextureWidth);
				float float3 = destination.GetFloat(ShaderUtilities.ID_TextureHeight);
				float float4 = destination.GetFloat(ShaderUtilities.ID_WeightNormal);
				float float5 = destination.GetFloat(ShaderUtilities.ID_WeightBold);
				destination.CopyPropertiesFromMaterial(source);
				destination.shaderKeywords = source.shaderKeywords;
				destination.SetTexture(ShaderUtilities.ID_MainTex, texture);
				destination.SetFloat(ShaderUtilities.ID_GradientScale, @float);
				destination.SetFloat(ShaderUtilities.ID_TextureWidth, float2);
				destination.SetFloat(ShaderUtilities.ID_TextureHeight, float3);
				destination.SetFloat(ShaderUtilities.ID_WeightNormal, float4);
				destination.SetFloat(ShaderUtilities.ID_WeightBold, float5);
				return;
			}
		}
	}
}
