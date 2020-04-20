using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
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
		return KeyBinding_UI.s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.KeyBindings;
	}

	public override void Awake()
	{
		KeyBinding_UI.s_instance = this;
		this.m_scrollRect = base.GetComponentInChildren<ScrollRect>();
		UIManager.SetGameObjectActive(this.m_container, false, null);
		this.m_keybindButtons = new List<UIKeyCommandEntry>();
		this.m_originalKeyCodeMapping = new Dictionary<int, KeyCodeData>();
		this.m_setKeyBindPreference = KeyPreference.NullPreference;
		this.m_setKeyBindPrimary = true;
		base.Awake();
	}

	private void OnDestroy()
	{
		KeyBinding_UI.s_instance = null;
	}

	private void Start()
	{
		if (this.m_okButton != null)
		{
			this.m_okButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnOkButton);
		}
		if (this.m_applyButton != null)
		{
			this.m_applyButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnApplyButton);
		}
		if (this.m_revertDefaultsButton != null)
		{
			this.m_revertDefaultsButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnRevertDefaultsButton);
		}
		if (this.m_closeButton != null)
		{
			this.m_closeButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnCancelButton);
		}
		this.m_keybindButtons.Clear();
		for (int i = 0; i < AccountPreferences.Get().GetKeyDefaultsSize(); i++)
		{
			AccountPreferences.KeyCodeDefault keyCodeDefault = AccountPreferences.Get().GetKeyCodeDefault(i);
			KeyBindingCommand keyBindingCommand = GameWideData.Get().GetKeyBindingCommand(keyCodeDefault.m_preference.ToString());
			if (keyBindingCommand != null)
			{
				if (keyBindingCommand.Settable)
				{
					UIKeyCommandEntry uikeyCommandEntry = UnityEngine.Object.Instantiate<UIKeyCommandEntry>(this.m_keyCommandEntryPrefab);
					uikeyCommandEntry.transform.SetParent(this.m_keyCommandContainer.transform);
					uikeyCommandEntry.transform.localScale = Vector3.one;
					uikeyCommandEntry.transform.localPosition = Vector3.zero;
					uikeyCommandEntry.transform.localEulerAngles = Vector3.zero;
					if (this.m_scrollRect != null)
					{
						uikeyCommandEntry.m_primaryKeyButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
						uikeyCommandEntry.m_secondaryKeyButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
					}
					uikeyCommandEntry.Init(keyCodeDefault.m_preference);
					this.m_keybindButtons.Add(uikeyCommandEntry);
				}
			}
		}
		this.SortDisplayList();
	}

	private void OnScroll(BaseEventData data)
	{
		this.m_scrollRect.OnScroll((PointerEventData)data);
	}

	private void UpdateKeyBindText()
	{
		for (int i = 0; i < this.m_keybindButtons.Count; i++)
		{
			KeyPreference keyPreference = this.m_keybindButtons[i].GetKeyPreference();
			string keyCommand = StringUtil.TR_KeyBindCommand(keyPreference.ToString());
			string fullKeyString = InputManager.Get().GetFullKeyString(keyPreference, true, false);
			string fullKeyString2 = InputManager.Get().GetFullKeyString(keyPreference, false, false);
			this.m_keybindButtons[i].SetLabels(keyCommand, fullKeyString, fullKeyString2);
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
		this.m_keybindButtons.Sort(new Comparison<UIKeyCommandEntry>(this.CompareKeybindButton));
		for (int i = 0; i < this.m_keybindButtons.Count; i++)
		{
			UIKeyCommandEntry uikeyCommandEntry = this.m_keybindButtons[i];
			if (uikeyCommandEntry.transform.GetSiblingIndex() != i)
			{
				uikeyCommandEntry.transform.SetSiblingIndex(i);
			}
		}
	}

	private void OnOkButton(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
		this.ApplyCurrentSettings();
		this.HideKeybinds();
	}

	private void OnApplyButton(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
		this.ApplyCurrentSettings();
	}

	private void OnRevertDefaultsButton(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
		InputManager.Get().SetDefaultKeyMapping();
		this.ApplyCurrentSettings();
		this.UpdateKeyBindText();
	}

	private void OnCancelButton(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.Back);
		this.SetPlayerKeyMappingsToSetOriginalKeyMappings();
		this.m_setKeyBindPreference = KeyPreference.NullPreference;
		this.HideKeybinds();
	}

	public void ToggleKeybinds()
	{
		if (!this.m_container.gameObject.activeSelf)
		{
			this.ShowKeybinds();
		}
		else
		{
			this.HideKeybinds();
		}
	}

	private void SetOriginalKeyMappingsToPlayerKeyMappings()
	{
		string value = JsonConvert.SerializeObject(InputManager.Get().m_keyCodeMapping);
		this.m_originalKeyCodeMapping = JsonConvert.DeserializeObject<Dictionary<int, KeyCodeData>>(value);
	}

	private void SetPlayerKeyMappingsToSetOriginalKeyMappings()
	{
		string value = JsonConvert.SerializeObject(this.m_originalKeyCodeMapping);
		InputManager.Get().m_keyCodeMapping = JsonConvert.DeserializeObject<Dictionary<int, KeyCodeData>>(value);
	}

	public void ShowKeybinds()
	{
		this.SetOriginalKeyMappingsToPlayerKeyMappings();
		this.UpdateKeyBindText();
		UIManager.SetGameObjectActive(this.m_container, true, null);
		this.m_setKeyBindPreference = KeyPreference.NullPreference;
		this.m_setKeyBindPrimary = true;
		this.ShowCurrentSetKeyBind();
	}

	public void HideKeybinds()
	{
		UIManager.SetGameObjectActive(this.m_container, false, null);
	}

	public bool IsVisible()
	{
		return this.m_container.gameObject.activeSelf;
	}

	public void ApplyCurrentSettings()
	{
		if (InputManager.Get() != null)
		{
			try
			{
				this.ClearSetKeyBind();
				InputManager.Get().SaveAllKeyBinds();
				this.SetOriginalKeyMappingsToPlayerKeyMappings();
			}
			catch (Exception ex)
			{
				Log.Error("ApplyCurrentSettings SetKeybind error. {0} {1}", new object[]
				{
					AppState.GetCurrentName(),
					ex
				});
			}
			try
			{
				if (UICharacterSelectScreenController.Get() != null)
				{
					if (UICharacterSelectCharacterSettingsPanel.Get() != null)
					{
						UICharacterSelectCharacterSettingsPanel.Get().m_abilitiesSubPanel.RefreshKeyBindings();
					}
				}
			}
			catch (Exception ex2)
			{
				Log.Error("ApplyCurrentSettings refresh Frontend UI error. {0} {1}", new object[]
				{
					AppState.GetCurrentName(),
					ex2
				});
			}
			try
			{
				if (UIMainScreenPanel.Get() != null)
				{
					if (UIMainScreenPanel.Get().m_abilityBar != null)
					{
						UIMainScreenPanel.Get().m_abilityBar.RefreshHotkeys();
					}
				}
			}
			catch (Exception ex3)
			{
				Log.Error("ApplyCurrentSettings refresh Ingame UI error. State:{0} {1}", new object[]
				{
					AppState.GetCurrentName(),
					ex3
				});
			}
		}
		else
		{
			Log.Error("InputManager is null when trying to ApplyCurrentSettings for KeyBinding_UI. {0}", new object[]
			{
				AppState.GetCurrentName()
			});
		}
	}

	public void ShowCurrentSetKeyBind()
	{
		for (int i = 0; i < this.m_keybindButtons.Count; i++)
		{
			UIKeyCommandEntry uikeyCommandEntry = this.m_keybindButtons[i];
			if (uikeyCommandEntry.GetKeyPreference() == this.m_setKeyBindPreference)
			{
				if (this.m_setKeyBindPrimary)
				{
					uikeyCommandEntry.m_primaryKeyButton.SetSelected(true, false, string.Empty, string.Empty);
					uikeyCommandEntry.m_secondaryKeyButton.SetSelected(false, false, string.Empty, string.Empty);
				}
				else
				{
					uikeyCommandEntry.m_primaryKeyButton.SetSelected(false, false, string.Empty, string.Empty);
					uikeyCommandEntry.m_secondaryKeyButton.SetSelected(true, false, string.Empty, string.Empty);
				}
			}
			else
			{
				uikeyCommandEntry.m_primaryKeyButton.SetSelected(false, false, string.Empty, string.Empty);
				uikeyCommandEntry.m_secondaryKeyButton.SetSelected(false, false, string.Empty, string.Empty);
			}
		}
	}

	public void ClearSetKeyBind()
	{
		if (this.m_setKeyBindPreference != KeyPreference.NullPreference)
		{
			this.m_setKeyBindPreference = KeyPreference.NullPreference;
			this.ShowCurrentSetKeyBind();
		}
	}

	public void ToggleKeyBindButton(KeyPreference setKeyBindPreference, bool setKeyBindPrimary)
	{
		if (this.m_setKeyBindPreference == setKeyBindPreference)
		{
			if (this.m_setKeyBindPrimary == setKeyBindPrimary)
			{
				this.ClearSetKeyBind();
				return;
			}
		}
		this.m_setKeyBindPreference = setKeyBindPreference;
		this.m_setKeyBindPrimary = setKeyBindPrimary;
		this.ShowCurrentSetKeyBind();
	}

	public void ClearKeyBindButton(KeyPreference setKeyBindPreference, bool setKeyBindPrimary)
	{
		if (setKeyBindPreference == this.m_setKeyBindPreference)
		{
			if (setKeyBindPrimary == this.m_setKeyBindPrimary)
			{
				this.ClearSetKeyBind();
			}
		}
		InputManager.Get().ClearKeyBind(setKeyBindPreference, setKeyBindPrimary);
		this.UpdateKeyBindText();
	}

	public bool IsSettingKeybindCommand()
	{
		bool result;
		if (this.m_setKeyBindPreference != KeyPreference.NullPreference)
		{
			result = this.IsVisible();
		}
		else
		{
			result = false;
		}
		return result;
	}

	public void Update()
	{
		if (this.IsSettingKeybindCommand())
		{
			IEnumerator enumerator = Enum.GetValues(typeof(KeyCode)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					KeyCode keyCode = (KeyCode)obj;
					if (Input.GetKeyUp(keyCode))
					{
						if (!InputManager.Get().IsUnbindableKey(keyCode))
						{
							KeyCode modifierKey = KeyCode.None;
							KeyCode additionalModifierKey = KeyCode.None;
							if (!InputManager.Get().IsModifierKey(keyCode))
							{
								InputManager.Get().GetModifierKeys(out modifierKey, out additionalModifierKey);
							}
							InputManager.Get().SetCustomKeyBind(this.m_setKeyBindPreference, keyCode, modifierKey, additionalModifierKey, this.m_setKeyBindPrimary);
							UIFrontEnd.PlaySound(FrontEndButtonSounds.SelectChoice);
							this.ClearSetKeyBind();
							this.UpdateKeyBindText();
							return;
						}
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
