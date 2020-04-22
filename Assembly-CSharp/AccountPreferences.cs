using I2.Loc;
using System;
using UnityEngine;

public class AccountPreferences : MonoBehaviour
{
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

		public BoolDefault[] m_boolDefaults = new BoolDefault[0];

		public KeyCodeDefault[] m_keyDefaults = new KeyCodeDefault[0];
	}

	internal bool ApplicationHasFocus = true;

	public DefaultSettingsData[] m_defaultSettings;

	public int m_languageIndex;

	private static AccountPreferences s_instance;

	private void OnApplicationFocus(bool hasFocus)
	{
		ApplicationHasFocus = hasFocus;
	}

	internal static AccountPreferences Get()
	{
		return s_instance;
	}

	public static bool DoesApplicationHaveFocus()
	{
		int result;
		if (s_instance != null)
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
			result = (s_instance.ApplicationHasFocus ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void Start()
	{
		SetLanguageIndex();
		for (int i = 0; i < m_defaultSettings[m_languageIndex].m_boolDefaults.Length; i++)
		{
			if (!PlayerPrefs.HasKey(m_defaultSettings[m_languageIndex].m_boolDefaults[i].m_preference.ToString()))
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
				SetBool(m_defaultSettings[m_languageIndex].m_boolDefaults[i].m_preference, m_defaultSettings[m_languageIndex].m_boolDefaults[i].m_value);
			}
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void SetLanguageIndex()
	{
		m_languageIndex = 0;
		int num = 0;
		DefaultSettingsData[] defaultSettings = m_defaultSettings;
		int num2 = 0;
		while (true)
		{
			if (num2 < defaultSettings.Length)
			{
				DefaultSettingsData defaultSettingsData = defaultSettings[num2];
				if (defaultSettingsData.Name == LocalizationManager.CurrentLanguageCode)
				{
					break;
				}
				num++;
				num2++;
				continue;
			}
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_languageIndex = num;
			return;
		}
	}

	public int GetKeyDefaultsSize()
	{
		return m_defaultSettings[m_languageIndex].m_keyDefaults.Length;
	}

	public KeyCodeDefault GetKeyCodeDefault(int index)
	{
		return m_defaultSettings[m_languageIndex].m_keyDefaults[index];
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
			while (true)
			{
				switch (6)
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
}
