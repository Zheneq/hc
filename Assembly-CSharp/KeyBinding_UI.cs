using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyBinding_UI : UIScene
{
	public RectTransform m_container;

	public _SelectableBtn m_okButton;

	public _SelectableBtn m_applyButton;

	public _SelectableBtn m_revertDefaultsButton;

	public _SelectableBtn m_closeButton;

	public VerticalLayoutGroup m_keyCommandContainer;

	public UIKeyCommandEntry m_keyCommandEntryPrefab;

	[HideInInspector]
	private List<UIKeyCommandEntry> m_keybindButtons;

	[HideInInspector]
	private Dictionary<int, KeyCodeData> m_originalKeyCodeMapping;

	[HideInInspector]
	private KeyPreference m_setKeyBindPreference;

	[HideInInspector]
	private bool m_setKeyBindPrimary;

	private ScrollRect m_scrollRect;

	private static KeyBinding_UI s_instance;

	public static KeyBinding_UI Get()
	{
		return s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.KeyBindings;
	}

	public override void Awake()
	{
		s_instance = this;
		m_scrollRect = GetComponentInChildren<ScrollRect>();
		UIManager.SetGameObjectActive(m_container, false);
		m_keybindButtons = new List<UIKeyCommandEntry>();
		m_originalKeyCodeMapping = new Dictionary<int, KeyCodeData>();
		m_setKeyBindPreference = KeyPreference.NullPreference;
		m_setKeyBindPrimary = true;
		base.Awake();
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void Start()
	{
		if (m_okButton != null)
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
			m_okButton.spriteController.callback = OnOkButton;
		}
		if (m_applyButton != null)
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
			m_applyButton.spriteController.callback = OnApplyButton;
		}
		if (m_revertDefaultsButton != null)
		{
			m_revertDefaultsButton.spriteController.callback = OnRevertDefaultsButton;
		}
		if (m_closeButton != null)
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
			m_closeButton.spriteController.callback = OnCancelButton;
		}
		m_keybindButtons.Clear();
		for (int i = 0; i < AccountPreferences.Get().GetKeyDefaultsSize(); i++)
		{
			AccountPreferences.KeyCodeDefault keyCodeDefault = AccountPreferences.Get().GetKeyCodeDefault(i);
			KeyBindingCommand keyBindingCommand = GameWideData.Get().GetKeyBindingCommand(keyCodeDefault.m_preference.ToString());
			if (keyBindingCommand == null)
			{
				continue;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!keyBindingCommand.Settable)
			{
				continue;
			}
			UIKeyCommandEntry uIKeyCommandEntry = UnityEngine.Object.Instantiate(m_keyCommandEntryPrefab);
			uIKeyCommandEntry.transform.SetParent(m_keyCommandContainer.transform);
			uIKeyCommandEntry.transform.localScale = Vector3.one;
			uIKeyCommandEntry.transform.localPosition = Vector3.zero;
			uIKeyCommandEntry.transform.localEulerAngles = Vector3.zero;
			if (m_scrollRect != null)
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
				uIKeyCommandEntry.m_primaryKeyButton.spriteController.RegisterScrollListener(OnScroll);
				uIKeyCommandEntry.m_secondaryKeyButton.spriteController.RegisterScrollListener(OnScroll);
			}
			uIKeyCommandEntry.Init(keyCodeDefault.m_preference);
			m_keybindButtons.Add(uIKeyCommandEntry);
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			SortDisplayList();
			return;
		}
	}

	private void OnScroll(BaseEventData data)
	{
		m_scrollRect.OnScroll((PointerEventData)data);
	}

	private void UpdateKeyBindText()
	{
		for (int i = 0; i < m_keybindButtons.Count; i++)
		{
			KeyPreference keyPreference = m_keybindButtons[i].GetKeyPreference();
			string keyCommand = StringUtil.TR_KeyBindCommand(keyPreference.ToString());
			string fullKeyString = InputManager.Get().GetFullKeyString(keyPreference, true);
			string fullKeyString2 = InputManager.Get().GetFullKeyString(keyPreference, false);
			m_keybindButtons[i].SetLabels(keyCommand, fullKeyString, fullKeyString2);
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	private int CompareKeybindButton(UIKeyCommandEntry first, UIKeyCommandEntry second)
	{
		KeyPreference keyPreference = first.GetKeyPreference();
		KeyPreference keyPreference2 = second.GetKeyPreference();
		return keyPreference - keyPreference2;
	}

	private void SortDisplayList()
	{
		m_keybindButtons.Sort(CompareKeybindButton);
		for (int i = 0; i < m_keybindButtons.Count; i++)
		{
			UIKeyCommandEntry uIKeyCommandEntry = m_keybindButtons[i];
			if (uIKeyCommandEntry.transform.GetSiblingIndex() != i)
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
				uIKeyCommandEntry.transform.SetSiblingIndex(i);
			}
		}
	}

	private void OnOkButton(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
		ApplyCurrentSettings();
		HideKeybinds();
	}

	private void OnApplyButton(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
		ApplyCurrentSettings();
	}

	private void OnRevertDefaultsButton(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
		InputManager.Get().SetDefaultKeyMapping();
		ApplyCurrentSettings();
		UpdateKeyBindText();
	}

	private void OnCancelButton(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.Back);
		SetPlayerKeyMappingsToSetOriginalKeyMappings();
		m_setKeyBindPreference = KeyPreference.NullPreference;
		HideKeybinds();
	}

	public void ToggleKeybinds()
	{
		if (!m_container.gameObject.activeSelf)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					ShowKeybinds();
					return;
				}
			}
		}
		HideKeybinds();
	}

	private void SetOriginalKeyMappingsToPlayerKeyMappings()
	{
		string value = JsonConvert.SerializeObject(InputManager.Get().m_keyCodeMapping);
		m_originalKeyCodeMapping = JsonConvert.DeserializeObject<Dictionary<int, KeyCodeData>>(value);
	}

	private void SetPlayerKeyMappingsToSetOriginalKeyMappings()
	{
		string value = JsonConvert.SerializeObject(m_originalKeyCodeMapping);
		InputManager.Get().m_keyCodeMapping = JsonConvert.DeserializeObject<Dictionary<int, KeyCodeData>>(value);
	}

	public void ShowKeybinds()
	{
		SetOriginalKeyMappingsToPlayerKeyMappings();
		UpdateKeyBindText();
		UIManager.SetGameObjectActive(m_container, true);
		m_setKeyBindPreference = KeyPreference.NullPreference;
		m_setKeyBindPrimary = true;
		ShowCurrentSetKeyBind();
	}

	public void HideKeybinds()
	{
		UIManager.SetGameObjectActive(m_container, false);
	}

	public bool IsVisible()
	{
		return m_container.gameObject.activeSelf;
	}

	public void ApplyCurrentSettings()
	{
		if (InputManager.Get() != null)
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
					try
					{
						ClearSetKeyBind();
						InputManager.Get().SaveAllKeyBinds();
						SetOriginalKeyMappingsToPlayerKeyMappings();
					}
					catch (Exception ex)
					{
						Log.Error("ApplyCurrentSettings SetKeybind error. {0} {1}", AppState.GetCurrentName(), ex);
					}
					try
					{
						if (UICharacterSelectScreenController.Get() != null)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									if (UICharacterSelectCharacterSettingsPanel.Get() != null)
									{
										while (true)
										{
											switch (2)
											{
											case 0:
												break;
											default:
												UICharacterSelectCharacterSettingsPanel.Get().m_abilitiesSubPanel.RefreshKeyBindings();
												goto end_IL_0062;
											}
										}
									}
									goto end_IL_0062;
								}
							}
						}
						end_IL_0062:;
					}
					catch (Exception ex2)
					{
						Log.Error("ApplyCurrentSettings refresh Frontend UI error. {0} {1}", AppState.GetCurrentName(), ex2);
					}
					try
					{
						if (UIMainScreenPanel.Get() != null)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									break;
								default:
									if (UIMainScreenPanel.Get().m_abilityBar != null)
									{
										UIMainScreenPanel.Get().m_abilityBar.RefreshHotkeys();
									}
									return;
								}
							}
						}
					}
					catch (Exception ex3)
					{
						Log.Error("ApplyCurrentSettings refresh Ingame UI error. State:{0} {1}", AppState.GetCurrentName(), ex3);
					}
					return;
				}
			}
		}
		Log.Error("InputManager is null when trying to ApplyCurrentSettings for KeyBinding_UI. {0}", AppState.GetCurrentName());
	}

	public void ShowCurrentSetKeyBind()
	{
		for (int i = 0; i < m_keybindButtons.Count; i++)
		{
			UIKeyCommandEntry uIKeyCommandEntry = m_keybindButtons[i];
			if (uIKeyCommandEntry.GetKeyPreference() == m_setKeyBindPreference)
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
				if (m_setKeyBindPrimary)
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
					uIKeyCommandEntry.m_primaryKeyButton.SetSelected(true, false, string.Empty, string.Empty);
					uIKeyCommandEntry.m_secondaryKeyButton.SetSelected(false, false, string.Empty, string.Empty);
				}
				else
				{
					uIKeyCommandEntry.m_primaryKeyButton.SetSelected(false, false, string.Empty, string.Empty);
					uIKeyCommandEntry.m_secondaryKeyButton.SetSelected(true, false, string.Empty, string.Empty);
				}
			}
			else
			{
				uIKeyCommandEntry.m_primaryKeyButton.SetSelected(false, false, string.Empty, string.Empty);
				uIKeyCommandEntry.m_secondaryKeyButton.SetSelected(false, false, string.Empty, string.Empty);
			}
		}
	}

	public void ClearSetKeyBind()
	{
		if (m_setKeyBindPreference == KeyPreference.NullPreference)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_setKeyBindPreference = KeyPreference.NullPreference;
			ShowCurrentSetKeyBind();
			return;
		}
	}

	public void ToggleKeyBindButton(KeyPreference setKeyBindPreference, bool setKeyBindPrimary)
	{
		if (m_setKeyBindPreference == setKeyBindPreference)
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
			if (m_setKeyBindPrimary == setKeyBindPrimary)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						ClearSetKeyBind();
						return;
					}
				}
			}
		}
		m_setKeyBindPreference = setKeyBindPreference;
		m_setKeyBindPrimary = setKeyBindPrimary;
		ShowCurrentSetKeyBind();
	}

	public void ClearKeyBindButton(KeyPreference setKeyBindPreference, bool setKeyBindPrimary)
	{
		if (setKeyBindPreference == m_setKeyBindPreference)
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
			if (setKeyBindPrimary == m_setKeyBindPrimary)
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
				ClearSetKeyBind();
			}
		}
		InputManager.Get().ClearKeyBind(setKeyBindPreference, setKeyBindPrimary);
		UpdateKeyBindText();
	}

	public bool IsSettingKeybindCommand()
	{
		int result;
		if (m_setKeyBindPreference != 0)
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
			result = (IsVisible() ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public void Update()
	{
		if (!IsSettingKeybindCommand())
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			IEnumerator enumerator = Enum.GetValues(typeof(KeyCode)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyCode keyCode = (KeyCode)enumerator.Current;
					if (Input.GetKeyUp(keyCode))
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
						if (!InputManager.Get().IsUnbindableKey(keyCode))
						{
							KeyCode modifier = KeyCode.None;
							KeyCode additionalModifier = KeyCode.None;
							if (!InputManager.Get().IsModifierKey(keyCode))
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
								InputManager.Get().GetModifierKeys(out modifier, out additionalModifier);
							}
							InputManager.Get().SetCustomKeyBind(m_setKeyBindPreference, keyCode, modifier, additionalModifier, m_setKeyBindPrimary);
							UIFrontEnd.PlaySound(FrontEndButtonSounds.SelectChoice);
							ClearSetKeyBind();
							UpdateKeyBindText();
							return;
						}
					}
				}
				while (true)
				{
					switch (2)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}
}
