using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	public class TMP_SpriteAsset : TMP_Asset
	{
		private Dictionary<int, int> m_UnicodeLookup;

		private Dictionary<int, int> m_NameLookup;

		public static TMP_SpriteAsset m_defaultSpriteAsset;

		public Texture spriteSheet;

		public List<TMP_Sprite> spriteInfoList;

		private Dictionary<int, int> m_SpriteUnicodeLookup;

		[SerializeField]
		public List<TMP_SpriteAsset> fallbackSpriteAssets;

		public static TMP_SpriteAsset defaultSpriteAsset
		{
			get
			{
				if (m_defaultSpriteAsset == null)
				{
					while (true)
					{
						switch (7)
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
					m_defaultSpriteAsset = Resources.Load<TMP_SpriteAsset>("Sprite Assets/Default Sprite Asset");
				}
				return m_defaultSpriteAsset;
			}
		}

		private void OnEnable()
		{
		}

		private Material GetDefaultSpriteMaterial()
		{
			ShaderUtilities.GetShaderPropertyIDs();
			Shader shader = Shader.Find("TextMeshPro/Sprite");
			Material material = new Material(shader);
			material.SetTexture(ShaderUtilities.ID_MainTex, spriteSheet);
			material.hideFlags = HideFlags.HideInHierarchy;
			return material;
		}

		public void UpdateLookupTables()
		{
			if (m_NameLookup == null)
			{
				m_NameLookup = new Dictionary<int, int>();
			}
			if (m_UnicodeLookup == null)
			{
				m_UnicodeLookup = new Dictionary<int, int>();
			}
			for (int i = 0; i < spriteInfoList.Count; i++)
			{
				int hashCode = spriteInfoList[i].hashCode;
				if (!m_NameLookup.ContainsKey(hashCode))
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
					m_NameLookup.Add(hashCode, i);
				}
				int unicode = spriteInfoList[i].unicode;
				if (!m_UnicodeLookup.ContainsKey(unicode))
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
					m_UnicodeLookup.Add(unicode, i);
				}
			}
		}

		public int GetSpriteIndexFromHashcode(int hashCode)
		{
			if (m_NameLookup == null)
			{
				UpdateLookupTables();
			}
			int value = 0;
			if (m_NameLookup.TryGetValue(hashCode, out value))
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
						return value;
					}
				}
			}
			return -1;
		}

		public int GetSpriteIndexFromUnicode(int unicode)
		{
			if (m_UnicodeLookup == null)
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
				UpdateLookupTables();
			}
			int value = 0;
			if (m_UnicodeLookup.TryGetValue(unicode, out value))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return value;
					}
				}
			}
			return -1;
		}

		public int GetSpriteIndexFromName(string name)
		{
			if (m_NameLookup == null)
			{
				while (true)
				{
					switch (7)
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
				UpdateLookupTables();
			}
			int simpleHashCode = TMP_TextUtilities.GetSimpleHashCode(name);
			return GetSpriteIndexFromHashcode(simpleHashCode);
		}

		public static TMP_SpriteAsset SearchFallbackForSprite(TMP_SpriteAsset spriteAsset, int unicode, out int spriteIndex)
		{
			spriteIndex = -1;
			if (spriteAsset == null)
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
						return null;
					}
				}
			}
			spriteIndex = spriteAsset.GetSpriteIndexFromUnicode(unicode);
			if (spriteIndex != -1)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return spriteAsset;
					}
				}
			}
			if (spriteAsset.fallbackSpriteAssets != null)
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
				if (spriteAsset.fallbackSpriteAssets.Count > 0)
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
					for (int i = 0; i < spriteAsset.fallbackSpriteAssets.Count; i++)
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
						if (spriteIndex == -1)
						{
							TMP_SpriteAsset tMP_SpriteAsset = SearchFallbackForSprite(spriteAsset.fallbackSpriteAssets[i], unicode, out spriteIndex);
							if (!(tMP_SpriteAsset != null))
							{
								continue;
							}
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								return tMP_SpriteAsset;
							}
						}
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						break;
					}
				}
			}
			return null;
		}

		public static TMP_SpriteAsset SearchFallbackForSprite(List<TMP_SpriteAsset> spriteAssets, int unicode, out int spriteIndex)
		{
			spriteIndex = -1;
			if (spriteAssets != null && spriteAssets.Count > 0)
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
				for (int i = 0; i < spriteAssets.Count; i++)
				{
					TMP_SpriteAsset tMP_SpriteAsset = SearchFallbackForSprite(spriteAssets[i], unicode, out spriteIndex);
					if (!(tMP_SpriteAsset != null))
					{
						continue;
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						return tMP_SpriteAsset;
					}
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			return null;
		}
	}
}
