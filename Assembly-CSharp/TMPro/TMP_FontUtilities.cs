using System.Collections.Generic;

namespace TMPro
{
	public static class TMP_FontUtilities
	{
		private static List<int> k_searchedFontAssets;

		public static TMP_FontAsset SearchForGlyph(TMP_FontAsset font, int character, out TMP_Glyph glyph)
		{
			if (k_searchedFontAssets == null)
			{
				k_searchedFontAssets = new List<int>();
			}
			k_searchedFontAssets.Clear();
			return SearchForGlyphInternal(font, character, out glyph);
		}

		public static TMP_FontAsset SearchForGlyph(List<TMP_FontAsset> fonts, int character, out TMP_Glyph glyph)
		{
			return SearchForGlyphInternal(fonts, character, out glyph);
		}

		private static TMP_FontAsset SearchForGlyphInternal(TMP_FontAsset font, int character, out TMP_Glyph glyph)
		{
			glyph = null;
			if (font == null)
			{
				return null;
			}
			if (font.characterDictionary.TryGetValue(character, out glyph))
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
						return font;
					}
				}
			}
			if (font.fallbackFontAssets != null)
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
				if (font.fallbackFontAssets.Count > 0)
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
					for (int i = 0; i < font.fallbackFontAssets.Count; i++)
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
						if (glyph != null)
						{
							break;
						}
						TMP_FontAsset tMP_FontAsset = font.fallbackFontAssets[i];
						if (tMP_FontAsset == null)
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
							continue;
						}
						int instanceID = tMP_FontAsset.GetInstanceID();
						if (k_searchedFontAssets.Contains(instanceID))
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
							continue;
						}
						k_searchedFontAssets.Add(instanceID);
						tMP_FontAsset = SearchForGlyphInternal(tMP_FontAsset, character, out glyph);
						if (!(tMP_FontAsset != null))
						{
							continue;
						}
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							return tMP_FontAsset;
						}
					}
				}
			}
			return null;
		}

		private static TMP_FontAsset SearchForGlyphInternal(List<TMP_FontAsset> fonts, int character, out TMP_Glyph glyph)
		{
			glyph = null;
			if (fonts != null)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (fonts.Count > 0)
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
					for (int i = 0; i < fonts.Count; i++)
					{
						TMP_FontAsset tMP_FontAsset = SearchForGlyphInternal(fonts[i], character, out glyph);
						if (!(tMP_FontAsset != null))
						{
							continue;
						}
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							return tMP_FontAsset;
						}
					}
				}
			}
			return null;
		}
	}
}
