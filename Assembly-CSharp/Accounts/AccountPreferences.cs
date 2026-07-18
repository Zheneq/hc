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
        return s_instance != null && s_instance.ApplicationHasFocus;
    }

    private void Awake()
    {
        s_instance = this;
    }

    private void Start()
    {
        SetLanguageIndex();
        foreach (BoolDefault value in m_defaultSettings[m_languageIndex].m_boolDefaults)
        {
            if (!PlayerPrefs.HasKey(value.m_preference.ToString()))
            {
                SetBool(value.m_preference, value.m_value);
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
        int languageIndex = 0;

        foreach (DefaultSettingsData defaultSettingsData in m_defaultSettings)
        {
            if (defaultSettingsData.Name == LocalizationManager.CurrentLanguageCode)
            {
                m_languageIndex = languageIndex;
                return;
            }

            languageIndex++;
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
        PlayerPrefs.SetInt(key.ToString(), value ? 1 : 0);
    }

    private void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }
}