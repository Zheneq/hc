using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	[Serializable]
	[ExecuteInEditMode]
	public class TMP_Settings : ScriptableObject
	{
		public class LineBreakingTable
		{
			public Dictionary<int, char> leadingCharacters;

			public Dictionary<int, char> followingCharacters;
		}

		private static TMP_Settings s_Instance;

		[SerializeField]
		private bool m_enableWordWrapping;

		[SerializeField]
		private bool m_enableKerning;

		[SerializeField]
		private bool m_enableExtraPadding;

		[SerializeField]
		private bool m_enableTintAllSprites;

		[SerializeField]
		private bool m_enableParseEscapeCharacters;

		[SerializeField]
		private int m_missingGlyphCharacter;

		[SerializeField]
		private bool m_warningsDisabled;

		[SerializeField]
		private TMP_FontAsset m_defaultFontAsset;

		[SerializeField]
		private string m_defaultFontAssetPath;

		[SerializeField]
		private float m_defaultFontSize;

		[SerializeField]
		private float m_defaultAutoSizeMinRatio;

		[SerializeField]
		private float m_defaultAutoSizeMaxRatio;

		[SerializeField]
		private Vector2 m_defaultTextMeshProTextContainerSize;

		[SerializeField]
		private Vector2 m_defaultTextMeshProUITextContainerSize;

		[SerializeField]
		private bool m_autoSizeTextContainer;

		[SerializeField]
		private List<TMP_FontAsset> m_fallbackFontAssets;

		[SerializeField]
		private bool m_matchMaterialPreset;

		[SerializeField]
		private TMP_SpriteAsset m_defaultSpriteAsset;

		[SerializeField]
		private string m_defaultSpriteAssetPath;

		[SerializeField]
		private bool m_enableEmojiSupport;

		[SerializeField]
		private TMP_StyleSheet m_defaultStyleSheet;

		[SerializeField]
		private TextAsset m_leadingCharacters;

		[SerializeField]
		private TextAsset m_followingCharacters;

		[SerializeField]
		private LineBreakingTable m_linebreakingRules;

		public static bool enableWordWrapping => instance.m_enableWordWrapping;

		public static bool enableKerning => instance.m_enableKerning;

		public static bool enableExtraPadding => instance.m_enableExtraPadding;

		public static bool enableTintAllSprites => instance.m_enableTintAllSprites;

		public static bool enableParseEscapeCharacters => instance.m_enableParseEscapeCharacters;

		public static int missingGlyphCharacter => instance.m_missingGlyphCharacter;

		public static bool warningsDisabled => instance.m_warningsDisabled;

		public static TMP_FontAsset defaultFontAsset => instance.m_defaultFontAsset;

		public static string defaultFontAssetPath => instance.m_defaultFontAssetPath;

		public static float defaultFontSize => instance.m_defaultFontSize;

		public static float defaultTextAutoSizingMinRatio => instance.m_defaultAutoSizeMinRatio;

		public static float defaultTextAutoSizingMaxRatio => instance.m_defaultAutoSizeMaxRatio;

		public static Vector2 defaultTextMeshProTextContainerSize => instance.m_defaultTextMeshProTextContainerSize;

		public static Vector2 defaultTextMeshProUITextContainerSize => instance.m_defaultTextMeshProUITextContainerSize;

		public static bool autoSizeTextContainer => instance.m_autoSizeTextContainer;

		public static List<TMP_FontAsset> fallbackFontAssets => instance.m_fallbackFontAssets;

		public static bool matchMaterialPreset => instance.m_matchMaterialPreset;

		public static TMP_SpriteAsset defaultSpriteAsset => instance.m_defaultSpriteAsset;

		public static string defaultSpriteAssetPath => instance.m_defaultSpriteAssetPath;

		public static bool enableEmojiSupport
		{
			get
			{
				return instance.m_enableEmojiSupport;
			}
			set
			{
				instance.m_enableEmojiSupport = value;
			}
		}

		public static TMP_StyleSheet defaultStyleSheet => instance.m_defaultStyleSheet;

		public static TextAsset leadingCharacters => instance.m_leadingCharacters;

		public static TextAsset followingCharacters => instance.m_followingCharacters;

		public static LineBreakingTable linebreakingRules
		{
			get
			{
				if (instance.m_linebreakingRules == null)
				{
					LoadLinebreakingRules();
				}
				return instance.m_linebreakingRules;
			}
		}

		public static TMP_Settings instance
		{
			get
			{
				if (s_Instance == null)
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
					s_Instance = (Resources.Load("TMP Settings") as TMP_Settings);
				}
				return s_Instance;
			}
		}

		public static TMP_Settings LoadDefaultSettings()
		{
			if (s_Instance == null)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				TMP_Settings x = Resources.Load("TMP Settings") as TMP_Settings;
				if (x != null)
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
					s_Instance = x;
				}
			}
			return s_Instance;
		}

		public static TMP_Settings GetSettings()
		{
			if (instance == null)
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
			return instance;
		}

		public static TMP_FontAsset GetFontAsset()
		{
			if (instance == null)
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
			return instance.m_defaultFontAsset;
		}

		public static TMP_SpriteAsset GetSpriteAsset()
		{
			if (instance == null)
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
			return instance.m_defaultSpriteAsset;
		}

		public static TMP_StyleSheet GetStyleSheet()
		{
			if (instance == null)
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
			return instance.m_defaultStyleSheet;
		}

		public static void LoadLinebreakingRules()
		{
			if (instance == null)
			{
				return;
			}
			if (s_Instance.m_linebreakingRules == null)
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
				s_Instance.m_linebreakingRules = new LineBreakingTable();
			}
			s_Instance.m_linebreakingRules.leadingCharacters = GetCharacters(s_Instance.m_leadingCharacters);
			s_Instance.m_linebreakingRules.followingCharacters = GetCharacters(s_Instance.m_followingCharacters);
		}

		private static Dictionary<int, char> GetCharacters(TextAsset file)
		{
			Dictionary<int, char> dictionary = new Dictionary<int, char>();
			string text = file.text;
			foreach (char c in text)
			{
				if (!dictionary.ContainsKey(c))
				{
					dictionary.Add(c, c);
				}
			}
			return dictionary;
		}
	}
}
