using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	[ExecuteInEditMode]
	[Serializable]
	public class TMP_Settings : ScriptableObject
	{
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
		private TMP_Settings.LineBreakingTable m_linebreakingRules;

		public static bool enableWordWrapping
		{
			get
			{
				return TMP_Settings.instance.m_enableWordWrapping;
			}
		}

		public static bool enableKerning
		{
			get
			{
				return TMP_Settings.instance.m_enableKerning;
			}
		}

		public static bool enableExtraPadding
		{
			get
			{
				return TMP_Settings.instance.m_enableExtraPadding;
			}
		}

		public static bool enableTintAllSprites
		{
			get
			{
				return TMP_Settings.instance.m_enableTintAllSprites;
			}
		}

		public static bool enableParseEscapeCharacters
		{
			get
			{
				return TMP_Settings.instance.m_enableParseEscapeCharacters;
			}
		}

		public static int missingGlyphCharacter
		{
			get
			{
				return TMP_Settings.instance.m_missingGlyphCharacter;
			}
		}

		public static bool warningsDisabled
		{
			get
			{
				return TMP_Settings.instance.m_warningsDisabled;
			}
		}

		public static TMP_FontAsset defaultFontAsset
		{
			get
			{
				return TMP_Settings.instance.m_defaultFontAsset;
			}
		}

		public static string defaultFontAssetPath
		{
			get
			{
				return TMP_Settings.instance.m_defaultFontAssetPath;
			}
		}

		public static float defaultFontSize
		{
			get
			{
				return TMP_Settings.instance.m_defaultFontSize;
			}
		}

		public static float defaultTextAutoSizingMinRatio
		{
			get
			{
				return TMP_Settings.instance.m_defaultAutoSizeMinRatio;
			}
		}

		public static float defaultTextAutoSizingMaxRatio
		{
			get
			{
				return TMP_Settings.instance.m_defaultAutoSizeMaxRatio;
			}
		}

		public static Vector2 defaultTextMeshProTextContainerSize
		{
			get
			{
				return TMP_Settings.instance.m_defaultTextMeshProTextContainerSize;
			}
		}

		public static Vector2 defaultTextMeshProUITextContainerSize
		{
			get
			{
				return TMP_Settings.instance.m_defaultTextMeshProUITextContainerSize;
			}
		}

		public static bool autoSizeTextContainer
		{
			get
			{
				return TMP_Settings.instance.m_autoSizeTextContainer;
			}
		}

		public static List<TMP_FontAsset> fallbackFontAssets
		{
			get
			{
				return TMP_Settings.instance.m_fallbackFontAssets;
			}
		}

		public static bool matchMaterialPreset
		{
			get
			{
				return TMP_Settings.instance.m_matchMaterialPreset;
			}
		}

		public static TMP_SpriteAsset defaultSpriteAsset
		{
			get
			{
				return TMP_Settings.instance.m_defaultSpriteAsset;
			}
		}

		public static string defaultSpriteAssetPath
		{
			get
			{
				return TMP_Settings.instance.m_defaultSpriteAssetPath;
			}
		}

		public static bool enableEmojiSupport
		{
			get
			{
				return TMP_Settings.instance.m_enableEmojiSupport;
			}
			set
			{
				TMP_Settings.instance.m_enableEmojiSupport = value;
			}
		}

		public static TMP_StyleSheet defaultStyleSheet
		{
			get
			{
				return TMP_Settings.instance.m_defaultStyleSheet;
			}
		}

		public static TextAsset leadingCharacters
		{
			get
			{
				return TMP_Settings.instance.m_leadingCharacters;
			}
		}

		public static TextAsset followingCharacters
		{
			get
			{
				return TMP_Settings.instance.m_followingCharacters;
			}
		}

		public static TMP_Settings.LineBreakingTable linebreakingRules
		{
			get
			{
				if (TMP_Settings.instance.m_linebreakingRules == null)
				{
					TMP_Settings.LoadLinebreakingRules();
				}
				return TMP_Settings.instance.m_linebreakingRules;
			}
		}

		public static TMP_Settings instance
		{
			get
			{
				if (TMP_Settings.s_Instance == null)
				{
					TMP_Settings.s_Instance = (Resources.Load("TMP Settings") as TMP_Settings);
				}
				return TMP_Settings.s_Instance;
			}
		}

		public static TMP_Settings LoadDefaultSettings()
		{
			if (TMP_Settings.s_Instance == null)
			{
				TMP_Settings x = Resources.Load("TMP Settings") as TMP_Settings;
				if (x != null)
				{
					TMP_Settings.s_Instance = x;
				}
			}
			return TMP_Settings.s_Instance;
		}

		public static TMP_Settings GetSettings()
		{
			if (TMP_Settings.instance == null)
			{
				return null;
			}
			return TMP_Settings.instance;
		}

		public static TMP_FontAsset GetFontAsset()
		{
			if (TMP_Settings.instance == null)
			{
				return null;
			}
			return TMP_Settings.instance.m_defaultFontAsset;
		}

		public static TMP_SpriteAsset GetSpriteAsset()
		{
			if (TMP_Settings.instance == null)
			{
				return null;
			}
			return TMP_Settings.instance.m_defaultSpriteAsset;
		}

		public static TMP_StyleSheet GetStyleSheet()
		{
			if (TMP_Settings.instance == null)
			{
				return null;
			}
			return TMP_Settings.instance.m_defaultStyleSheet;
		}

		public static void LoadLinebreakingRules()
		{
			if (TMP_Settings.instance == null)
			{
				return;
			}
			if (TMP_Settings.s_Instance.m_linebreakingRules == null)
			{
				TMP_Settings.s_Instance.m_linebreakingRules = new TMP_Settings.LineBreakingTable();
			}
			TMP_Settings.s_Instance.m_linebreakingRules.leadingCharacters = TMP_Settings.GetCharacters(TMP_Settings.s_Instance.m_leadingCharacters);
			TMP_Settings.s_Instance.m_linebreakingRules.followingCharacters = TMP_Settings.GetCharacters(TMP_Settings.s_Instance.m_followingCharacters);
		}

		private static Dictionary<int, char> GetCharacters(TextAsset file)
		{
			Dictionary<int, char> dictionary = new Dictionary<int, char>();
			foreach (char c in file.text)
			{
				if (!dictionary.ContainsKey((int)c))
				{
					dictionary.Add((int)c, c);
				}
			}
			return dictionary;
		}

		public class LineBreakingTable
		{
			public Dictionary<int, char> leadingCharacters;

			public Dictionary<int, char> followingCharacters;
		}
	}
}
