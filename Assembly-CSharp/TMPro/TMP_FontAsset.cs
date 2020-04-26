using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TMPro
{
	[Serializable]
	public class TMP_FontAsset : TMP_Asset
	{
		public enum FontAssetTypes
		{
			None,
			SDF,
			Bitmap
		}

		private static TMP_FontAsset s_defaultFontAsset;

		public FontAssetTypes fontAssetType;

		[SerializeField]
		private FaceInfo m_fontInfo;

		[SerializeField]
		public Texture2D atlas;

		[SerializeField]
		private List<TMP_Glyph> m_glyphInfoList;

		private Dictionary<int, TMP_Glyph> m_characterDictionary;

		private Dictionary<int, KerningPair> m_kerningDictionary;

		[SerializeField]
		private KerningTable m_kerningInfo;

		[SerializeField]
		private KerningPair m_kerningPair;

		[SerializeField]
		public List<TMP_FontAsset> fallbackFontAssets;

		[SerializeField]
		public FontCreationSetting fontCreationSettings;

		[SerializeField]
		public TMP_FontWeights[] fontWeights = new TMP_FontWeights[10];

		private int[] m_characterSet;

		public float normalStyle;

		public float normalSpacingOffset;

		public float boldStyle = 0.75f;

		public float boldSpacing = 7f;

		public byte italicStyle = 35;

		public byte tabSize = 10;

		private byte m_oldTabSize;

		public static TMP_FontAsset defaultFontAsset
		{
			get
			{
				if (s_defaultFontAsset == null)
				{
					s_defaultFontAsset = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
				}
				return s_defaultFontAsset;
			}
		}

		public FaceInfo fontInfo => m_fontInfo;

		public Dictionary<int, TMP_Glyph> characterDictionary
		{
			get
			{
				if (m_characterDictionary == null)
				{
					ReadFontDefinition();
				}
				return m_characterDictionary;
			}
		}

		public Dictionary<int, KerningPair> kerningDictionary => m_kerningDictionary;

		public KerningTable kerningInfo => m_kerningInfo;

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		public void AddFaceInfo(FaceInfo faceInfo)
		{
			m_fontInfo = faceInfo;
		}

		public void AddGlyphInfo(TMP_Glyph[] glyphInfo)
		{
			m_glyphInfoList = new List<TMP_Glyph>();
			int num = glyphInfo.Length;
			m_fontInfo.CharacterCount = num;
			m_characterSet = new int[num];
			for (int i = 0; i < num; i++)
			{
				TMP_Glyph tMP_Glyph = new TMP_Glyph();
				tMP_Glyph.id = glyphInfo[i].id;
				tMP_Glyph.x = glyphInfo[i].x;
				tMP_Glyph.y = glyphInfo[i].y;
				tMP_Glyph.width = glyphInfo[i].width;
				tMP_Glyph.height = glyphInfo[i].height;
				tMP_Glyph.xOffset = glyphInfo[i].xOffset;
				tMP_Glyph.yOffset = glyphInfo[i].yOffset;
				tMP_Glyph.xAdvance = glyphInfo[i].xAdvance;
				tMP_Glyph.scale = 1f;
				m_glyphInfoList.Add(tMP_Glyph);
				m_characterSet[i] = tMP_Glyph.id;
			}
			while (true)
			{
				List<TMP_Glyph> glyphInfoList = m_glyphInfoList;
				
				m_glyphInfoList = glyphInfoList.OrderBy(((TMP_Glyph s) => s.id)).ToList();
				return;
			}
		}

		public void AddKerningInfo(KerningTable kerningTable)
		{
			m_kerningInfo = kerningTable;
		}

		public void ReadFontDefinition()
		{
			if (m_fontInfo == null)
			{
				return;
			}
			m_characterDictionary = new Dictionary<int, TMP_Glyph>();
			for (int i = 0; i < m_glyphInfoList.Count; i++)
			{
				TMP_Glyph tMP_Glyph = m_glyphInfoList[i];
				if (!m_characterDictionary.ContainsKey(tMP_Glyph.id))
				{
					m_characterDictionary.Add(tMP_Glyph.id, tMP_Glyph);
				}
				if (tMP_Glyph.scale == 0f)
				{
					tMP_Glyph.scale = 1f;
				}
			}
			while (true)
			{
				TMP_Glyph tMP_Glyph2 = new TMP_Glyph();
				if (m_characterDictionary.ContainsKey(32))
				{
					m_characterDictionary[32].width = m_characterDictionary[32].xAdvance;
					m_characterDictionary[32].height = m_fontInfo.Ascender - m_fontInfo.Descender;
					m_characterDictionary[32].yOffset = m_fontInfo.Ascender;
					m_characterDictionary[32].scale = 1f;
				}
				else
				{
					tMP_Glyph2 = new TMP_Glyph();
					tMP_Glyph2.id = 32;
					tMP_Glyph2.x = 0f;
					tMP_Glyph2.y = 0f;
					tMP_Glyph2.width = m_fontInfo.Ascender / 5f;
					tMP_Glyph2.height = m_fontInfo.Ascender - m_fontInfo.Descender;
					tMP_Glyph2.xOffset = 0f;
					tMP_Glyph2.yOffset = m_fontInfo.Ascender;
					tMP_Glyph2.xAdvance = m_fontInfo.PointSize / 4f;
					tMP_Glyph2.scale = 1f;
					m_characterDictionary.Add(32, tMP_Glyph2);
				}
				if (!m_characterDictionary.ContainsKey(160))
				{
					tMP_Glyph2 = TMP_Glyph.Clone(m_characterDictionary[32]);
					m_characterDictionary.Add(160, tMP_Glyph2);
				}
				if (!m_characterDictionary.ContainsKey(8203))
				{
					tMP_Glyph2 = TMP_Glyph.Clone(m_characterDictionary[32]);
					tMP_Glyph2.width = 0f;
					tMP_Glyph2.xAdvance = 0f;
					m_characterDictionary.Add(8203, tMP_Glyph2);
				}
				if (!m_characterDictionary.ContainsKey(8288))
				{
					tMP_Glyph2 = TMP_Glyph.Clone(m_characterDictionary[32]);
					tMP_Glyph2.width = 0f;
					tMP_Glyph2.xAdvance = 0f;
					m_characterDictionary.Add(8288, tMP_Glyph2);
				}
				if (!m_characterDictionary.ContainsKey(10))
				{
					tMP_Glyph2 = new TMP_Glyph();
					tMP_Glyph2.id = 10;
					tMP_Glyph2.x = 0f;
					tMP_Glyph2.y = 0f;
					tMP_Glyph2.width = 10f;
					tMP_Glyph2.height = m_characterDictionary[32].height;
					tMP_Glyph2.xOffset = 0f;
					tMP_Glyph2.yOffset = m_characterDictionary[32].yOffset;
					tMP_Glyph2.xAdvance = 0f;
					tMP_Glyph2.scale = 1f;
					m_characterDictionary.Add(10, tMP_Glyph2);
					if (!m_characterDictionary.ContainsKey(13))
					{
						m_characterDictionary.Add(13, tMP_Glyph2);
					}
				}
				if (!m_characterDictionary.ContainsKey(9))
				{
					tMP_Glyph2 = new TMP_Glyph();
					tMP_Glyph2.id = 9;
					tMP_Glyph2.x = m_characterDictionary[32].x;
					tMP_Glyph2.y = m_characterDictionary[32].y;
					tMP_Glyph2.width = m_characterDictionary[32].width * (float)(int)tabSize + (m_characterDictionary[32].xAdvance - m_characterDictionary[32].width) * (float)(tabSize - 1);
					tMP_Glyph2.height = m_characterDictionary[32].height;
					tMP_Glyph2.xOffset = m_characterDictionary[32].xOffset;
					tMP_Glyph2.yOffset = m_characterDictionary[32].yOffset;
					tMP_Glyph2.xAdvance = m_characterDictionary[32].xAdvance * (float)(int)tabSize;
					tMP_Glyph2.scale = 1f;
					m_characterDictionary.Add(9, tMP_Glyph2);
				}
				m_fontInfo.TabWidth = m_characterDictionary[9].xAdvance;
				if (m_fontInfo.CapHeight == 0f && m_characterDictionary.ContainsKey(72))
				{
					m_fontInfo.CapHeight = m_characterDictionary[72].yOffset;
				}
				if (m_fontInfo.Scale == 0f)
				{
					m_fontInfo.Scale = 1f;
				}
				if (m_fontInfo.strikethrough == 0f)
				{
					m_fontInfo.strikethrough = m_fontInfo.CapHeight / 2.5f;
				}
				if (m_fontInfo.Padding == 0f)
				{
					if (material.HasProperty(ShaderUtilities.ID_GradientScale))
					{
						m_fontInfo.Padding = material.GetFloat(ShaderUtilities.ID_GradientScale) - 1f;
					}
				}
				m_kerningDictionary = new Dictionary<int, KerningPair>();
				List<KerningPair> kerningPairs = m_kerningInfo.kerningPairs;
				for (int j = 0; j < kerningPairs.Count; j++)
				{
					KerningPair kerningPair = kerningPairs[j];
					KerningPairKey kerningPairKey = new KerningPairKey(kerningPair.AscII_Left, kerningPair.AscII_Right);
					if (!m_kerningDictionary.ContainsKey(kerningPairKey.key))
					{
						m_kerningDictionary.Add(kerningPairKey.key, kerningPair);
					}
					else if (!TMP_Settings.warningsDisabled)
					{
						Debug.LogWarning("Kerning Key for [" + kerningPairKey.ascii_Left + "] and [" + kerningPairKey.ascii_Right + "] already exists.");
					}
				}
				hashCode = TMP_TextUtilities.GetSimpleHashCode(base.name);
				materialHashCode = TMP_TextUtilities.GetSimpleHashCode(material.name);
				return;
			}
		}

		public void SortGlyphs()
		{
			if (m_glyphInfoList == null)
			{
				return;
			}
			while (true)
			{
				if (m_glyphInfoList.Count == 0)
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
				List<TMP_Glyph> glyphInfoList = m_glyphInfoList;
				
				m_glyphInfoList = glyphInfoList.OrderBy(((TMP_Glyph item) => item.id)).ToList();
				return;
			}
		}

		public bool HasCharacter(int character)
		{
			if (m_characterDictionary == null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
			if (m_characterDictionary.ContainsKey(character))
			{
				return true;
			}
			return false;
		}

		public bool HasCharacter(char character)
		{
			if (m_characterDictionary == null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
			if (m_characterDictionary.ContainsKey(character))
			{
				return true;
			}
			return false;
		}

		public bool HasCharacter(char character, bool searchFallbacks)
		{
			if (m_characterDictionary == null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
			if (m_characterDictionary.ContainsKey(character))
			{
				return true;
			}
			if (searchFallbacks)
			{
				if (fallbackFontAssets != null)
				{
					if (fallbackFontAssets.Count > 0)
					{
						for (int i = 0; i < fallbackFontAssets.Count; i++)
						{
							if (fallbackFontAssets[i] != null)
							{
								if (fallbackFontAssets[i].characterDictionary == null)
								{
									continue;
								}
								if (!fallbackFontAssets[i].characterDictionary.ContainsKey(character))
								{
									continue;
								}
								while (true)
								{
									return true;
								}
							}
							break;
						}
					}
				}
				if (TMP_Settings.fallbackFontAssets != null)
				{
					if (TMP_Settings.fallbackFontAssets.Count > 0)
					{
						for (int j = 0; j < TMP_Settings.fallbackFontAssets.Count; j++)
						{
							if (TMP_Settings.fallbackFontAssets[j] != null)
							{
								if (TMP_Settings.fallbackFontAssets[j].characterDictionary != null && TMP_Settings.fallbackFontAssets[j].characterDictionary.ContainsKey(character))
								{
									return true;
								}
								continue;
							}
							break;
						}
					}
				}
			}
			return false;
		}

		public bool HasCharacters(string text, out List<char> missingCharacters)
		{
			if (m_characterDictionary == null)
			{
				missingCharacters = null;
				return false;
			}
			missingCharacters = new List<char>();
			for (int i = 0; i < text.Length; i++)
			{
				if (!m_characterDictionary.ContainsKey(text[i]))
				{
					missingCharacters.Add(text[i]);
				}
			}
			while (true)
			{
				if (missingCharacters.Count == 0)
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
				return false;
			}
		}

		public bool HasCharacters(string text)
		{
			if (m_characterDictionary == null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
			for (int i = 0; i < text.Length; i++)
			{
				if (m_characterDictionary.ContainsKey(text[i]))
				{
					continue;
				}
				while (true)
				{
					return false;
				}
			}
			while (true)
			{
				return true;
			}
		}

		public static string GetCharacters(TMP_FontAsset fontAsset)
		{
			string text = string.Empty;
			for (int i = 0; i < fontAsset.m_glyphInfoList.Count; i++)
			{
				text += (char)fontAsset.m_glyphInfoList[i].id;
			}
			while (true)
			{
				return text;
			}
		}

		public static int[] GetCharactersArray(TMP_FontAsset fontAsset)
		{
			int[] array = new int[fontAsset.m_glyphInfoList.Count];
			for (int i = 0; i < fontAsset.m_glyphInfoList.Count; i++)
			{
				array[i] = fontAsset.m_glyphInfoList[i].id;
			}
			while (true)
			{
				return array;
			}
		}
	}
}
