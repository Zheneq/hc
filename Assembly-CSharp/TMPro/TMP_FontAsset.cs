using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TMPro
{
	[Serializable]
	public class TMP_FontAsset : TMP_Asset
	{
		private static TMP_FontAsset s_defaultFontAsset;

		public TMP_FontAsset.FontAssetTypes fontAssetType;

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
		public TMP_FontWeights[] fontWeights = new TMP_FontWeights[0xA];

		private int[] m_characterSet;

		public float normalStyle;

		public float normalSpacingOffset;

		public float boldStyle = 0.75f;

		public float boldSpacing = 7f;

		public byte italicStyle = 0x23;

		public byte tabSize = 0xA;

		private byte m_oldTabSize;

		public static TMP_FontAsset defaultFontAsset
		{
			get
			{
				if (TMP_FontAsset.s_defaultFontAsset == null)
				{
					TMP_FontAsset.s_defaultFontAsset = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
				}
				return TMP_FontAsset.s_defaultFontAsset;
			}
		}

		public FaceInfo fontInfo
		{
			get
			{
				return this.m_fontInfo;
			}
		}

		public Dictionary<int, TMP_Glyph> characterDictionary
		{
			get
			{
				if (this.m_characterDictionary == null)
				{
					this.ReadFontDefinition();
				}
				return this.m_characterDictionary;
			}
		}

		public Dictionary<int, KerningPair> kerningDictionary
		{
			get
			{
				return this.m_kerningDictionary;
			}
		}

		public KerningTable kerningInfo
		{
			get
			{
				return this.m_kerningInfo;
			}
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		public void AddFaceInfo(FaceInfo faceInfo)
		{
			this.m_fontInfo = faceInfo;
		}

		public void AddGlyphInfo(TMP_Glyph[] glyphInfo)
		{
			this.m_glyphInfoList = new List<TMP_Glyph>();
			int num = glyphInfo.Length;
			this.m_fontInfo.CharacterCount = num;
			this.m_characterSet = new int[num];
			for (int i = 0; i < num; i++)
			{
				TMP_Glyph tmp_Glyph = new TMP_Glyph();
				tmp_Glyph.id = glyphInfo[i].id;
				tmp_Glyph.x = glyphInfo[i].x;
				tmp_Glyph.y = glyphInfo[i].y;
				tmp_Glyph.width = glyphInfo[i].width;
				tmp_Glyph.height = glyphInfo[i].height;
				tmp_Glyph.xOffset = glyphInfo[i].xOffset;
				tmp_Glyph.yOffset = glyphInfo[i].yOffset;
				tmp_Glyph.xAdvance = glyphInfo[i].xAdvance;
				tmp_Glyph.scale = 1f;
				this.m_glyphInfoList.Add(tmp_Glyph);
				this.m_characterSet[i] = tmp_Glyph.id;
			}
			IEnumerable<TMP_Glyph> glyphInfoList = this.m_glyphInfoList;
			
			this.m_glyphInfoList = glyphInfoList.OrderBy(((TMP_Glyph s) => s.id)).ToList<TMP_Glyph>();
		}

		public void AddKerningInfo(KerningTable kerningTable)
		{
			this.m_kerningInfo = kerningTable;
		}

		public void ReadFontDefinition()
		{
			if (this.m_fontInfo == null)
			{
				return;
			}
			this.m_characterDictionary = new Dictionary<int, TMP_Glyph>();
			for (int i = 0; i < this.m_glyphInfoList.Count; i++)
			{
				TMP_Glyph tmp_Glyph = this.m_glyphInfoList[i];
				if (!this.m_characterDictionary.ContainsKey(tmp_Glyph.id))
				{
					this.m_characterDictionary.Add(tmp_Glyph.id, tmp_Glyph);
				}
				if (tmp_Glyph.scale == 0f)
				{
					tmp_Glyph.scale = 1f;
				}
			}
			TMP_Glyph tmp_Glyph2 = new TMP_Glyph();
			if (this.m_characterDictionary.ContainsKey(0x20))
			{
				this.m_characterDictionary[0x20].width = this.m_characterDictionary[0x20].xAdvance;
				this.m_characterDictionary[0x20].height = this.m_fontInfo.Ascender - this.m_fontInfo.Descender;
				this.m_characterDictionary[0x20].yOffset = this.m_fontInfo.Ascender;
				this.m_characterDictionary[0x20].scale = 1f;
			}
			else
			{
				tmp_Glyph2 = new TMP_Glyph();
				tmp_Glyph2.id = 0x20;
				tmp_Glyph2.x = 0f;
				tmp_Glyph2.y = 0f;
				tmp_Glyph2.width = this.m_fontInfo.Ascender / 5f;
				tmp_Glyph2.height = this.m_fontInfo.Ascender - this.m_fontInfo.Descender;
				tmp_Glyph2.xOffset = 0f;
				tmp_Glyph2.yOffset = this.m_fontInfo.Ascender;
				tmp_Glyph2.xAdvance = this.m_fontInfo.PointSize / 4f;
				tmp_Glyph2.scale = 1f;
				this.m_characterDictionary.Add(0x20, tmp_Glyph2);
			}
			if (!this.m_characterDictionary.ContainsKey(0xA0))
			{
				tmp_Glyph2 = TMP_Glyph.Clone(this.m_characterDictionary[0x20]);
				this.m_characterDictionary.Add(0xA0, tmp_Glyph2);
			}
			if (!this.m_characterDictionary.ContainsKey(0x200B))
			{
				tmp_Glyph2 = TMP_Glyph.Clone(this.m_characterDictionary[0x20]);
				tmp_Glyph2.width = 0f;
				tmp_Glyph2.xAdvance = 0f;
				this.m_characterDictionary.Add(0x200B, tmp_Glyph2);
			}
			if (!this.m_characterDictionary.ContainsKey(0x2060))
			{
				tmp_Glyph2 = TMP_Glyph.Clone(this.m_characterDictionary[0x20]);
				tmp_Glyph2.width = 0f;
				tmp_Glyph2.xAdvance = 0f;
				this.m_characterDictionary.Add(0x2060, tmp_Glyph2);
			}
			if (!this.m_characterDictionary.ContainsKey(0xA))
			{
				tmp_Glyph2 = new TMP_Glyph();
				tmp_Glyph2.id = 0xA;
				tmp_Glyph2.x = 0f;
				tmp_Glyph2.y = 0f;
				tmp_Glyph2.width = 10f;
				tmp_Glyph2.height = this.m_characterDictionary[0x20].height;
				tmp_Glyph2.xOffset = 0f;
				tmp_Glyph2.yOffset = this.m_characterDictionary[0x20].yOffset;
				tmp_Glyph2.xAdvance = 0f;
				tmp_Glyph2.scale = 1f;
				this.m_characterDictionary.Add(0xA, tmp_Glyph2);
				if (!this.m_characterDictionary.ContainsKey(0xD))
				{
					this.m_characterDictionary.Add(0xD, tmp_Glyph2);
				}
			}
			if (!this.m_characterDictionary.ContainsKey(9))
			{
				tmp_Glyph2 = new TMP_Glyph();
				tmp_Glyph2.id = 9;
				tmp_Glyph2.x = this.m_characterDictionary[0x20].x;
				tmp_Glyph2.y = this.m_characterDictionary[0x20].y;
				tmp_Glyph2.width = this.m_characterDictionary[0x20].width * (float)this.tabSize + (this.m_characterDictionary[0x20].xAdvance - this.m_characterDictionary[0x20].width) * (float)(this.tabSize - 1);
				tmp_Glyph2.height = this.m_characterDictionary[0x20].height;
				tmp_Glyph2.xOffset = this.m_characterDictionary[0x20].xOffset;
				tmp_Glyph2.yOffset = this.m_characterDictionary[0x20].yOffset;
				tmp_Glyph2.xAdvance = this.m_characterDictionary[0x20].xAdvance * (float)this.tabSize;
				tmp_Glyph2.scale = 1f;
				this.m_characterDictionary.Add(9, tmp_Glyph2);
			}
			this.m_fontInfo.TabWidth = this.m_characterDictionary[9].xAdvance;
			if (this.m_fontInfo.CapHeight == 0f && this.m_characterDictionary.ContainsKey(0x48))
			{
				this.m_fontInfo.CapHeight = this.m_characterDictionary[0x48].yOffset;
			}
			if (this.m_fontInfo.Scale == 0f)
			{
				this.m_fontInfo.Scale = 1f;
			}
			if (this.m_fontInfo.strikethrough == 0f)
			{
				this.m_fontInfo.strikethrough = this.m_fontInfo.CapHeight / 2.5f;
			}
			if (this.m_fontInfo.Padding == 0f)
			{
				if (this.material.HasProperty(ShaderUtilities.ID_GradientScale))
				{
					this.m_fontInfo.Padding = this.material.GetFloat(ShaderUtilities.ID_GradientScale) - 1f;
				}
			}
			this.m_kerningDictionary = new Dictionary<int, KerningPair>();
			List<KerningPair> kerningPairs = this.m_kerningInfo.kerningPairs;
			for (int j = 0; j < kerningPairs.Count; j++)
			{
				KerningPair kerningPair = kerningPairs[j];
				KerningPairKey kerningPairKey = new KerningPairKey(kerningPair.AscII_Left, kerningPair.AscII_Right);
				if (!this.m_kerningDictionary.ContainsKey(kerningPairKey.key))
				{
					this.m_kerningDictionary.Add(kerningPairKey.key, kerningPair);
				}
				else if (!TMP_Settings.warningsDisabled)
				{
					Debug.LogWarning(string.Concat(new object[]
					{
						"Kerning Key for [",
						kerningPairKey.ascii_Left,
						"] and [",
						kerningPairKey.ascii_Right,
						"] already exists."
					}));
				}
			}
			this.hashCode = TMP_TextUtilities.GetSimpleHashCode(base.name);
			this.materialHashCode = TMP_TextUtilities.GetSimpleHashCode(this.material.name);
		}

		public void SortGlyphs()
		{
			if (this.m_glyphInfoList != null)
			{
				if (this.m_glyphInfoList.Count != 0)
				{
					IEnumerable<TMP_Glyph> glyphInfoList = this.m_glyphInfoList;
					
					this.m_glyphInfoList = glyphInfoList.OrderBy(((TMP_Glyph item) => item.id)).ToList<TMP_Glyph>();
					return;
				}
			}
		}

		public bool HasCharacter(int character)
		{
			if (this.m_characterDictionary == null)
			{
				return false;
			}
			return this.m_characterDictionary.ContainsKey(character);
		}

		public bool HasCharacter(char character)
		{
			if (this.m_characterDictionary == null)
			{
				return false;
			}
			return this.m_characterDictionary.ContainsKey((int)character);
		}

		public bool HasCharacter(char character, bool searchFallbacks)
		{
			if (this.m_characterDictionary == null)
			{
				return false;
			}
			if (this.m_characterDictionary.ContainsKey((int)character))
			{
				return true;
			}
			if (searchFallbacks)
			{
				if (this.fallbackFontAssets != null)
				{
					if (this.fallbackFontAssets.Count > 0)
					{
						int i = 0;
						while (i < this.fallbackFontAssets.Count)
						{
							if (!(this.fallbackFontAssets[i] != null))
							{
								for (;;)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									goto IL_E7;
								}
							}
							else
							{
								if (this.fallbackFontAssets[i].characterDictionary != null)
								{
									if (this.fallbackFontAssets[i].characterDictionary.ContainsKey((int)character))
									{
										return true;
									}
								}
								i++;
							}
						}
					}
				}
				IL_E7:
				if (TMP_Settings.fallbackFontAssets != null)
				{
					if (TMP_Settings.fallbackFontAssets.Count > 0)
					{
						int j = 0;
						while (j < TMP_Settings.fallbackFontAssets.Count)
						{
							if (!(TMP_Settings.fallbackFontAssets[j] != null))
							{
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									return false;
								}
							}
							else
							{
								if (TMP_Settings.fallbackFontAssets[j].characterDictionary != null && TMP_Settings.fallbackFontAssets[j].characterDictionary.ContainsKey((int)character))
								{
									return true;
								}
								j++;
							}
						}
					}
				}
			}
			return false;
		}

		public unsafe bool HasCharacters(string text, out List<char> missingCharacters)
		{
			if (this.m_characterDictionary == null)
			{
				missingCharacters = null;
				return false;
			}
			missingCharacters = new List<char>();
			for (int i = 0; i < text.Length; i++)
			{
				if (!this.m_characterDictionary.ContainsKey((int)text[i]))
				{
					missingCharacters.Add(text[i]);
				}
			}
			if (missingCharacters.Count == 0)
			{
				return true;
			}
			return false;
		}

		public bool HasCharacters(string text)
		{
			if (this.m_characterDictionary == null)
			{
				return false;
			}
			for (int i = 0; i < text.Length; i++)
			{
				if (!this.m_characterDictionary.ContainsKey((int)text[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static string GetCharacters(TMP_FontAsset fontAsset)
		{
			string text = string.Empty;
			for (int i = 0; i < fontAsset.m_glyphInfoList.Count; i++)
			{
				text += (char)fontAsset.m_glyphInfoList[i].id;
			}
			return text;
		}

		public static int[] GetCharactersArray(TMP_FontAsset fontAsset)
		{
			int[] array = new int[fontAsset.m_glyphInfoList.Count];
			for (int i = 0; i < fontAsset.m_glyphInfoList.Count; i++)
			{
				array[i] = fontAsset.m_glyphInfoList[i].id;
			}
			return array;
		}

		public enum FontAssetTypes
		{
			None,
			SDF,
			Bitmap
		}
	}
}
