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
				if (s_Instance == null)
				{
					while (true)
					{
						switch (3)
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
					s_Instance = new MaterialReferenceManager();
				}
				return s_Instance;
			}
		}

		public static void AddFontAsset(TMP_FontAsset fontAsset)
		{
			instance.AddFontAssetInternal(fontAsset);
		}

		private void AddFontAssetInternal(TMP_FontAsset fontAsset)
		{
			if (!m_FontAssetReferenceLookup.ContainsKey(fontAsset.hashCode))
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
				m_FontAssetReferenceLookup.Add(fontAsset.hashCode, fontAsset);
			}
			if (m_FontMaterialReferenceLookup.ContainsKey(fontAsset.materialHashCode))
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
				m_FontMaterialReferenceLookup.Add(fontAsset.materialHashCode, fontAsset.material);
				return;
			}
		}

		public static void AddSpriteAsset(TMP_SpriteAsset spriteAsset)
		{
			instance.AddSpriteAssetInternal(spriteAsset);
		}

		private void AddSpriteAssetInternal(TMP_SpriteAsset spriteAsset)
		{
			if (m_SpriteAssetReferenceLookup.ContainsKey(spriteAsset.hashCode))
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			m_SpriteAssetReferenceLookup.Add(spriteAsset.hashCode, spriteAsset);
			m_FontMaterialReferenceLookup.Add(spriteAsset.hashCode, spriteAsset.material);
		}

		public static void AddSpriteAsset(int hashCode, TMP_SpriteAsset spriteAsset)
		{
			instance.AddSpriteAssetInternal(hashCode, spriteAsset);
		}

		private void AddSpriteAssetInternal(int hashCode, TMP_SpriteAsset spriteAsset)
		{
			if (m_SpriteAssetReferenceLookup.ContainsKey(hashCode))
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			m_SpriteAssetReferenceLookup.Add(hashCode, spriteAsset);
			m_FontMaterialReferenceLookup.Add(hashCode, spriteAsset.material);
			if (spriteAsset.hashCode != 0)
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				spriteAsset.hashCode = hashCode;
				return;
			}
		}

		public static void AddFontMaterial(int hashCode, Material material)
		{
			instance.AddFontMaterialInternal(hashCode, material);
		}

		private void AddFontMaterialInternal(int hashCode, Material material)
		{
			m_FontMaterialReferenceLookup.Add(hashCode, material);
		}

		public bool Contains(TMP_FontAsset font)
		{
			if (m_FontAssetReferenceLookup.ContainsKey(font.hashCode))
			{
				return true;
			}
			return false;
		}

		public bool Contains(TMP_SpriteAsset sprite)
		{
			if (m_FontAssetReferenceLookup.ContainsKey(sprite.hashCode))
			{
				return true;
			}
			return false;
		}

		public static bool TryGetFontAsset(int hashCode, out TMP_FontAsset fontAsset)
		{
			return instance.TryGetFontAssetInternal(hashCode, out fontAsset);
		}

		private bool TryGetFontAssetInternal(int hashCode, out TMP_FontAsset fontAsset)
		{
			fontAsset = null;
			if (m_FontAssetReferenceLookup.TryGetValue(hashCode, out fontAsset))
			{
				return true;
			}
			return false;
		}

		public static bool TryGetSpriteAsset(int hashCode, out TMP_SpriteAsset spriteAsset)
		{
			return instance.TryGetSpriteAssetInternal(hashCode, out spriteAsset);
		}

		private bool TryGetSpriteAssetInternal(int hashCode, out TMP_SpriteAsset spriteAsset)
		{
			spriteAsset = null;
			if (m_SpriteAssetReferenceLookup.TryGetValue(hashCode, out spriteAsset))
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return true;
					}
				}
			}
			return false;
		}

		public static bool TryGetMaterial(int hashCode, out Material material)
		{
			return instance.TryGetMaterialInternal(hashCode, out material);
		}

		private bool TryGetMaterialInternal(int hashCode, out Material material)
		{
			material = null;
			if (m_FontMaterialReferenceLookup.TryGetValue(hashCode, out material))
			{
				return true;
			}
			return false;
		}
	}
}
