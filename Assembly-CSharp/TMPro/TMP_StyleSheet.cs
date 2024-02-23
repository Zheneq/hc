using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	[Serializable]
	public class TMP_StyleSheet : ScriptableObject
	{
		private static TMP_StyleSheet s_Instance;

		[SerializeField]
		private List<TMP_Style> m_StyleList = new List<TMP_Style>(1);

		private Dictionary<int, TMP_Style> m_StyleDictionary = new Dictionary<int, TMP_Style>();

		public static TMP_StyleSheet instance
		{
			get
			{
				if (s_Instance == null)
				{
					s_Instance = TMP_Settings.defaultStyleSheet;
					if (s_Instance == null)
					{
						s_Instance = (Resources.Load("Style Sheets/TMP Default Style Sheet") as TMP_StyleSheet);
					}
					if (s_Instance == null)
					{
						while (true)
						{
							return null;
						}
					}
					s_Instance.LoadStyleDictionaryInternal();
				}
				return s_Instance;
			}
		}

		public static TMP_StyleSheet LoadDefaultStyleSheet()
		{
			return instance;
		}

		public static TMP_Style GetStyle(int hashCode)
		{
			return instance.GetStyleInternal(hashCode);
		}

		private TMP_Style GetStyleInternal(int hashCode)
		{
			TMP_Style value;
			if (m_StyleDictionary.TryGetValue(hashCode, out value))
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return value;
					}
				}
			}
			return null;
		}

		public void UpdateStyleDictionaryKey(int old_key, int new_key)
		{
			if (!m_StyleDictionary.ContainsKey(old_key))
			{
				return;
			}
			while (true)
			{
				TMP_Style value = m_StyleDictionary[old_key];
				m_StyleDictionary.Add(new_key, value);
				m_StyleDictionary.Remove(old_key);
				return;
			}
		}

		public static void RefreshStyles()
		{
			instance.LoadStyleDictionaryInternal();
		}

		private void LoadStyleDictionaryInternal()
		{
			m_StyleDictionary.Clear();
			for (int i = 0; i < m_StyleList.Count; i++)
			{
				m_StyleList[i].RefreshStyle();
				if (!m_StyleDictionary.ContainsKey(m_StyleList[i].hashCode))
				{
					m_StyleDictionary.Add(m_StyleList[i].hashCode, m_StyleList[i]);
				}
			}
			while (true)
			{
				return;
			}
		}
	}
}
