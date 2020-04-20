using System;
using I2.Loc;
using UnityEngine;

public class AccountPreferences : MonoBehaviour
{
	internal bool ApplicationHasFocus = true;

	public AccountPreferences.DefaultSettingsData[] m_defaultSettings;

	public int m_languageIndex;

	private static AccountPreferences s_instance;

	private void OnApplicationFocus(bool hasFocus)
	{
		this.ApplicationHasFocus = hasFocus;
	}

	internal static AccountPreferences Get()
	{
		return AccountPreferences.s_instance;
	}

	public static bool DoesApplicationHaveFocus()
	{
		bool result;
		if (AccountPreferences.s_instance != null)
		{
			result = AccountPreferences.s_instance.ApplicationHasFocus;
		}
		else
		{
			result = false;
		}
		return result;
	}

	private void Awake()
	{
		AccountPreferences.s_instance = this;
	}

	private void Start()
	{
		this.SetLanguageIndex();
		for (int i = 0; i < this.m_defaultSettings[this.m_languageIndex].m_boolDefaults.Length; i++)
		{
			if (!PlayerPrefs.HasKey(this.m_defaultSettings[this.m_languageIndex].m_boolDefaults[i].m_preference.ToString()))
			{
				this.SetBool(this.m_defaultSettings[this.m_languageIndex].m_boolDefaults[i].m_preference, this.m_defaultSettings[this.m_languageIndex].m_boolDefaults[i].m_value);
			}
		}
	}

	private void OnDestroy()
	{
		AccountPreferences.s_instance = null;
	}

	private void SetLanguageIndex()
	{
		this.m_languageIndex = 0;
		int num = 0;
		foreach (AccountPreferences.DefaultSettingsData defaultSettingsData in this.m_defaultSettings)
		{
			if (defaultSettingsData.Name == LocalizationManager.CurrentLanguageCode)
			{
				this.m_languageIndex = num;
				return;
			}
			num++;
		}
	}

	public int GetKeyDefaultsSize()
	{
		return this.m_defaultSettings[this.m_languageIndex].m_keyDefaults.Length;
	}

	public AccountPreferences.KeyCodeDefault GetKeyCodeDefault(int index)
	{
		return this.m_defaultSettings[this.m_languageIndex].m_keyDefaults[index];
	}

	internal bool GetBool(BoolPreference key)
	{
		return PlayerPrefs.GetInt(key.ToString()) != 0;
	}

	private int GetInt(string key)
	{
		return PlayerPrefs.GetInt(key);
	}

	internal void SetBool(BoolPreference key, bool value)
	{
		string key2 = key.ToString();
		int value2;
		if (value)
		{
			value2 = 1;
		}
		else
		{
			value2 = 0;
		}
		PlayerPrefs.SetInt(key2, value2);
	}

	private void SetInt(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
	}

	[Serializable]
	public class BoolDefault
	{
		public BoolPreference m_preference;

		public bool m_value;
	}

	[Serializable]
	public class KeyCodeDefault
	{
		public KeyPreference m_preference;

		public KeyCode m_primary;

		public KeyCode m_modifierKey1;

		public KeyCode m_additionalModifierKey1;

		public KeyCode m_secondary;

		public KeyCode m_modifierKey2;

		public KeyCode m_additionalModifierKey2;
	}

	[Serializable]
	public class DefaultSettingsData
	{
		public string Name;

		public AccountPreferences.BoolDefault[] m_boolDefaults = new AccountPreferences.BoolDefault[0];

		public AccountPreferences.KeyCodeDefault[] m_keyDefaults = new AccountPreferences.KeyCodeDefault[0];
	}
}
