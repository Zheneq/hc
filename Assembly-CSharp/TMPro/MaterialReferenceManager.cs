using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	public class MaterialReferenceManager
	{
		private static MaterialReferenceManager s_Instance;

		private Dictionary<int, Material> m_FontMaterialReferenceLookup = new Dictionary<int, Material>();

		private Dictionary<int, TMP_FontAsset> m_FontAssetReferenceLookup = new Dictionary<int, TMP_FontAsset>();

		private Dictionary<int, TMP_SpriteAsset> m_SpriteAssetReferenceLookup = new Dictionary<int, TMP_SpriteAsset>();

		public static MaterialReferenceManager instance
		{
			get
			{
				if (MaterialReferenceManager.s_Instance == null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(MaterialReferenceManager.get_instance()).MethodHandle;
					}
					MaterialReferenceManager.s_Instance = new MaterialReferenceManager();
				}
				return MaterialReferenceManager.s_Instance;
			}
		}

		public static void AddFontAsset(TMP_FontAsset fontAsset)
		{
			MaterialReferenceManager.instance.AddFontAssetInternal(fontAsset);
		}

		private void AddFontAssetInternal(TMP_FontAsset fontAsset)
		{
			if (!this.m_FontAssetReferenceLookup.ContainsKey(fontAsset.hashCode))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(MaterialReferenceManager.AddFontAssetInternal(TMP_FontAsset)).MethodHandle;
				}
				this.m_FontAssetReferenceLookup.Add(fontAsset.hashCode, fontAsset);
			}
			if (!this.m_FontMaterialReferenceLookup.ContainsKey(fontAsset.materialHashCode))
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
				this.m_FontMaterialReferenceLookup.Add(fontAsset.materialHashCode, fontAsset.material);
			}
		}

		public static void AddSpriteAsset(TMP_SpriteAsset spriteAsset)
		{
			MaterialReferenceManager.instance.AddSpriteAssetInternal(spriteAsset);
		}

		private void AddSpriteAssetInternal(TMP_SpriteAsset spriteAsset)
		{
			if (this.m_SpriteAssetReferenceLookup.ContainsKey(spriteAsset.hashCode))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(MaterialReferenceManager.AddSpriteAssetInternal(TMP_SpriteAsset)).MethodHandle;
				}
				return;
			}
			this.m_SpriteAssetReferenceLookup.Add(spriteAsset.hashCode, spriteAsset);
			this.m_FontMaterialReferenceLookup.Add(spriteAsset.hashCode, spriteAsset.material);
		}

		public static void AddSpriteAsset(int hashCode, TMP_SpriteAsset spriteAsset)
		{
			MaterialReferenceManager.instance.AddSpriteAssetInternal(hashCode, spriteAsset);
		}

		private void AddSpriteAssetInternal(int hashCode, TMP_SpriteAsset spriteAsset)
		{
			if (this.m_SpriteAssetReferenceLookup.ContainsKey(hashCode))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(MaterialReferenceManager.AddSpriteAssetInternal(int, TMP_SpriteAsset)).MethodHandle;
				}
				return;
			}
			this.m_SpriteAssetReferenceLookup.Add(hashCode, spriteAsset);
			this.m_FontMaterialReferenceLookup.Add(hashCode, spriteAsset.material);
			if (spriteAsset.hashCode == 0)
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
				spriteAsset.hashCode = hashCode;
			}
		}

		public static void AddFontMaterial(int hashCode, Material material)
		{
			MaterialReferenceManager.instance.AddFontMaterialInternal(hashCode, material);
		}

		private void AddFontMaterialInternal(int hashCode, Material material)
		{
			this.m_FontMaterialReferenceLookup.Add(hashCode, material);
		}

		public bool Contains(TMP_FontAsset font)
		{
			return this.m_FontAssetReferenceLookup.ContainsKey(font.hashCode);
		}

		public bool Contains(TMP_SpriteAsset sprite)
		{
			return this.m_FontAssetReferenceLookup.ContainsKey(sprite.hashCode);
		}

		public static bool TryGetFontAsset(int hashCode, out TMP_FontAsset fontAsset)
		{
			return MaterialReferenceManager.instance.TryGetFontAssetInternal(hashCode, out fontAsset);
		}

		private bool TryGetFontAssetInternal(int hashCode, out TMP_FontAsset fontAsset)
		{
			fontAsset = null;
			return this.m_FontAssetReferenceLookup.TryGetValue(hashCode, out fontAsset);
		}

		public static bool TryGetSpriteAsset(int hashCode, out TMP_SpriteAsset spriteAsset)
		{
			return MaterialReferenceManager.instance.TryGetSpriteAssetInternal(hashCode, out spriteAsset);
		}

		private unsafe bool TryGetSpriteAssetInternal(int hashCode, out TMP_SpriteAsset spriteAsset)
		{
			spriteAsset = null;
			if (this.m_SpriteAssetReferenceLookup.TryGetValue(hashCode, out spriteAsset))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(MaterialReferenceManager.TryGetSpriteAssetInternal(int, TMP_SpriteAsset*)).MethodHandle;
				}
				return true;
			}
			return false;
		}

		public static bool TryGetMaterial(int hashCode, out Material material)
		{
			return MaterialReferenceManager.instance.TryGetMaterialInternal(hashCode, out material);
		}

		private bool TryGetMaterialInternal(int hashCode, out Material material)
		{
			material = null;
			return this.m_FontMaterialReferenceLookup.TryGetValue(hashCode, out material);
		}
	}
}
