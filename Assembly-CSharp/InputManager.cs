using System;
using System.Collections;
using System.Collections.Generic;
using Corale.Colore.Razer.Keyboard;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	internal bool KeyMappingInitialized;

	public Dictionary<int, KeyCodeData> m_keyCodeMapping;

	public Dictionary<int, ControlpadInputValue> m_controlPadMapping;

	private static InputManager s_instance;

	private Dictionary<KeyCode, Key> m_razerKeyMapping;

	private Dictionary<InputManager.KeyCodeCacheEntry, List<InputManager.KeyCodeCacheEntry>> m_heaviliyModifiedCommandCache;

	private void Awake()
	{
		InputManager.s_instance = this;
		this.m_keyCodeMapping = new Dictionary<int, KeyCodeData>();
		this.m_controlPadMapping = new Dictionary<int, ControlpadInputValue>();
		this.m_heaviliyModifiedCommandCache = new Dictionary<InputManager.KeyCodeCacheEntry, List<InputManager.KeyCodeCacheEntry>>();
	}

	private void Start()
	{
		this.SetDefaultKeyMapping();
		ClientGameManager.Get().OnAccountDataUpdated += this.HandleAccountDataUpdated;
	}

	private void OnDestroy()
	{
		InputManager.s_instance = null;
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnAccountDataUpdated -= this.HandleAccountDataUpdated;
		}
	}

	public static InputManager Get()
	{
		return InputManager.s_instance;
	}

	public void SetDefaultKeyMapping()
	{
		this.m_keyCodeMapping.Clear();
		this.m_controlPadMapping.Clear();
		this.m_heaviliyModifiedCommandCache.Clear();
		for (int i = 0; i < AccountPreferences.Get().GetKeyDefaultsSize(); i++)
		{
			AccountPreferences.KeyCodeDefault keyCodeDefault = AccountPreferences.Get().GetKeyCodeDefault(i);
			this.m_keyCodeMapping.Add((int)keyCodeDefault.m_preference, new KeyCodeData
			{
				m_primary = (int)keyCodeDefault.m_primary,
				m_modifierKey1 = (int)keyCodeDefault.m_modifierKey1,
				m_additionalModifierKey1 = (int)keyCodeDefault.m_additionalModifierKey1,
				m_secondary = (int)keyCodeDefault.m_secondary,
				m_modifierKey2 = (int)keyCodeDefault.m_modifierKey2,
				m_additionalModifierKey2 = (int)keyCodeDefault.m_additionalModifierKey2
			});
		}
		this.m_controlPadMapping[0xCB] = ControlpadInputValue.Button_X;
		this.m_controlPadMapping[0xC8] = ControlpadInputValue.Button_Y;
		this.m_controlPadMapping[0xC9] = ControlpadInputValue.Button_B;
		this.m_controlPadMapping[1] = ControlpadInputValue.Button_leftStickIn;
		this.m_controlPadMapping[0xD2] = ControlpadInputValue.Button_rightShoulder;
		this.m_controlPadMapping[0xCC] = ControlpadInputValue.Button_start;
		this.m_controlPadMapping[2] = ControlpadInputValue.Button_rightStickIn;
	}

	public void ClearKeyBind(KeyPreference m_preference, bool primary)
	{
		KeyCodeData keyCodeData = null;
		if (this.m_keyCodeMapping.TryGetValue((int)m_preference, out keyCodeData))
		{
			if (primary)
			{
				keyCodeData.m_primary = 0;
				keyCodeData.m_modifierKey1 = 0;
				keyCodeData.m_additionalModifierKey1 = 0;
			}
			else
			{
				keyCodeData.m_secondary = 0;
				keyCodeData.m_modifierKey2 = 0;
				keyCodeData.m_additionalModifierKey2 = 0;
			}
			this.m_heaviliyModifiedCommandCache.Clear();
		}
	}

	public bool SetCustomKeyBind(KeyPreference preference, KeyCode keyCode, KeyCode modifierKey, KeyCode additionalModifierKey, bool primary)
	{
		KeyCodeData keyCodeData = null;
		KeyBindingCommand keyBindingCommand = GameWideData.Get().GetKeyBindingCommand(preference.ToString());
		if (keyBindingCommand == null)
		{
			return false;
		}
		if (this.m_keyCodeMapping.TryGetValue((int)preference, out keyCodeData))
		{
			if (primary)
			{
				keyCodeData.m_primary = (int)keyCode;
				keyCodeData.m_modifierKey1 = (int)modifierKey;
				keyCodeData.m_additionalModifierKey1 = (int)additionalModifierKey;
			}
			else
			{
				keyCodeData.m_secondary = (int)keyCode;
				keyCodeData.m_modifierKey2 = (int)modifierKey;
				keyCodeData.m_additionalModifierKey2 = (int)additionalModifierKey;
			}
			using (Dictionary<int, KeyCodeData>.Enumerator enumerator = this.m_keyCodeMapping.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, KeyCodeData> keyValuePair = enumerator.Current;
					KeyPreference key = (KeyPreference)keyValuePair.Key;
					KeyBindingCommand keyBindingCommand2 = GameWideData.Get().GetKeyBindingCommand(key.ToString());
					if (keyBindingCommand2 == null)
					{
						Log.Error("Could not find KeyBindingCommand for {0} in GameWideData", new object[]
						{
							key.ToString()
						});
					}
					else if (!keyBindingCommand2.Settable)
					{
					}
					else
					{
						if (keyBindingCommand.Category != KeyBindCategory.Global && keyBindingCommand2.Category != KeyBindCategory.Global)
						{
							if (keyBindingCommand2.Category != keyBindingCommand.Category)
							{
								continue;
							}
						}
						if (key != preference)
						{
							goto IL_17A;
						}
						if (!primary)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								goto IL_17A;
							}
						}
						IL_1ED:
						if (key == preference)
						{
							if (!primary)
							{
								continue;
							}
						}
						if (keyValuePair.Value.m_secondary != (int)keyCode)
						{
							continue;
						}
						if (keyValuePair.Value.m_modifierKey2 == (int)modifierKey && keyValuePair.Value.m_additionalModifierKey2 == (int)additionalModifierKey)
						{
							keyValuePair.Value.m_secondary = 0;
							keyValuePair.Value.m_modifierKey2 = 0;
							keyValuePair.Value.m_additionalModifierKey2 = 0;
							continue;
						}
						continue;
						IL_17A:
						if (keyValuePair.Value.m_primary != (int)keyCode)
						{
							goto IL_1ED;
						}
						if (keyValuePair.Value.m_modifierKey1 != (int)modifierKey)
						{
							goto IL_1ED;
						}
						if (keyValuePair.Value.m_additionalModifierKey1 == (int)additionalModifierKey)
						{
							keyValuePair.Value.m_primary = 0;
							keyValuePair.Value.m_modifierKey1 = 0;
							keyValuePair.Value.m_additionalModifierKey1 = 0;
							goto IL_1ED;
						}
						goto IL_1ED;
					}
				}
			}
			this.m_heaviliyModifiedCommandCache.Clear();
			return true;
		}
		return false;
	}

	public void SaveAllKeyBinds()
	{
		ClientGameManager.Get().NotifyCustomKeyBinds(this.m_keyCodeMapping);
	}

	public void HandleAccountDataUpdated(PersistedAccountData accountData)
	{
		if (!this.KeyMappingInitialized)
		{
			this.m_keyCodeMapping.Clear();
			this.m_heaviliyModifiedCommandCache.Clear();
			bool flag = false;
			for (int i = 0; i < AccountPreferences.Get().GetKeyDefaultsSize(); i++)
			{
				AccountPreferences.KeyCodeDefault keyCodeDefault = AccountPreferences.Get().GetKeyCodeDefault(i);
				int preference = (int)keyCodeDefault.m_preference;
				KeyCodeData value = null;
				if (accountData.AccountComponent.KeyCodeMapping.TryGetValue(preference, out value))
				{
					this.m_keyCodeMapping.Add(preference, value);
				}
				else
				{
					this.m_keyCodeMapping.Add(preference, new KeyCodeData
					{
						m_primary = (int)keyCodeDefault.m_primary,
						m_modifierKey1 = (int)keyCodeDefault.m_modifierKey1,
						m_additionalModifierKey1 = (int)keyCodeDefault.m_additionalModifierKey1,
						m_secondary = (int)keyCodeDefault.m_secondary,
						m_modifierKey2 = (int)keyCodeDefault.m_modifierKey2,
						m_additionalModifierKey2 = (int)keyCodeDefault.m_additionalModifierKey2
					});
					flag = true;
				}
			}
			if (flag)
			{
				this.SaveAllKeyBinds();
			}
			this.KeyMappingInitialized = true;
		}
	}

	public bool IsModifierKey(KeyCode key)
	{
		if (key != KeyCode.RightControl)
		{
			if (key != KeyCode.LeftControl)
			{
				if (key != KeyCode.RightShift && key != KeyCode.LeftShift && key != KeyCode.RightAlt)
				{
					return key == KeyCode.LeftAlt;
				}
			}
		}
		return true;
	}

	private KeyCode CombineLeftRightModifiers(KeyCode key)
	{
		KeyCode result = key;
		switch (key)
		{
		case KeyCode.RightShift:
		case KeyCode.LeftShift:
			result = KeyCode.LeftShift;
			break;
		case KeyCode.RightControl:
		case KeyCode.LeftControl:
			result = KeyCode.LeftControl;
			break;
		case KeyCode.RightAlt:
		case KeyCode.LeftAlt:
			result = KeyCode.LeftAlt;
			break;
		}
		return result;
	}

	public bool IsControlDown()
	{
		bool result;
		if (!Input.GetKey(KeyCode.LeftControl))
		{
			result = Input.GetKey(KeyCode.RightControl);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool IsAltDown()
	{
		return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
	}

	public bool IsShiftDown()
	{
		bool result;
		if (!Input.GetKey(KeyCode.LeftShift))
		{
			result = Input.GetKey(KeyCode.RightShift);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public unsafe void GetModifierKeys(out KeyCode modifier, out KeyCode additionalModifier)
	{
		modifier = KeyCode.None;
		additionalModifier = KeyCode.None;
		if (this.IsControlDown())
		{
			modifier = KeyCode.LeftControl;
		}
		else if (this.IsAltDown())
		{
			modifier = KeyCode.LeftAlt;
		}
		else if (this.IsShiftDown())
		{
			modifier = KeyCode.LeftShift;
		}
		if (modifier == KeyCode.LeftControl)
		{
			if (this.IsAltDown())
			{
				additionalModifier = KeyCode.LeftAlt;
			}
			else if (this.IsShiftDown())
			{
				additionalModifier = KeyCode.LeftShift;
			}
		}
		else if (modifier == KeyCode.LeftAlt)
		{
			if (this.IsShiftDown())
			{
				additionalModifier = KeyCode.LeftShift;
			}
		}
	}

	public bool IsUnbindableKey(KeyCode key)
	{
		if (key != KeyCode.Pause)
		{
			if (key != KeyCode.ScrollLock && key != KeyCode.Break && key != KeyCode.Mouse0 && key != KeyCode.Mouse1 && key != KeyCode.Menu)
			{
				if (key != KeyCode.Slash && key != KeyCode.Return)
				{
					return key == KeyCode.KeypadEnter;
				}
			}
		}
		return true;
	}

	private bool CheckModifierDown(KeyCode modifierKey)
	{
		bool result = false;
		switch (modifierKey)
		{
		case KeyCode.RightShift:
		case KeyCode.LeftShift:
			result = this.IsShiftDown();
			break;
		case KeyCode.RightControl:
		case KeyCode.LeftControl:
			result = this.IsControlDown();
			break;
		case KeyCode.RightAlt:
		case KeyCode.LeftAlt:
			result = this.IsAltDown();
			break;
		}
		return result;
	}

	private bool IsModifierDown(KeyCode modifierKey, KeyCode additionalModifierKey)
	{
		bool flag = true;
		if (modifierKey != KeyCode.None)
		{
			flag = this.CheckModifierDown(modifierKey);
		}
		if (flag && additionalModifierKey != KeyCode.None)
		{
			flag = this.CheckModifierDown(additionalModifierKey);
		}
		return flag;
	}

	private bool IsMoreHeavilyModifiedKeyCommandDown(KeyCode key, KeyCode modifierKey, KeyCode additionalModifierKey)
	{
		InputManager.KeyCodeCacheEntry key2;
		key2.m_primary = (int)key;
		key2.m_modifierKey = (int)modifierKey;
		key2.m_additionalModifierKey = (int)additionalModifierKey;
		if (!this.m_heaviliyModifiedCommandCache.ContainsKey(key2))
		{
			List<InputManager.KeyCodeCacheEntry> list = new List<InputManager.KeyCodeCacheEntry>();
			using (Dictionary<int, KeyCodeData>.Enumerator enumerator = this.m_keyCodeMapping.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, KeyCodeData> keyValuePair = enumerator.Current;
					KeyCodeData value = keyValuePair.Value;
					if (value.m_primary == (int)key)
					{
						if (modifierKey == KeyCode.None)
						{
							if (value.m_modifierKey1 != 0)
							{
								goto IL_AF;
							}
						}
						if (additionalModifierKey != KeyCode.None || value.m_additionalModifierKey1 == 0)
						{
							goto IL_E1;
						}
						IL_AF:
						InputManager.KeyCodeCacheEntry item;
						item.m_primary = value.m_primary;
						item.m_modifierKey = value.m_modifierKey1;
						item.m_additionalModifierKey = value.m_additionalModifierKey1;
						list.Add(item);
					}
					IL_E1:
					if (value.m_secondary == (int)key)
					{
						if (modifierKey == KeyCode.None)
						{
							if (value.m_modifierKey2 != 0)
							{
								goto IL_121;
							}
						}
						if (additionalModifierKey != KeyCode.None)
						{
							continue;
						}
						if (value.m_additionalModifierKey2 == 0)
						{
							continue;
						}
						IL_121:
						InputManager.KeyCodeCacheEntry item2;
						item2.m_primary = value.m_primary;
						item2.m_modifierKey = value.m_modifierKey2;
						item2.m_additionalModifierKey = value.m_additionalModifierKey2;
						list.Add(item2);
					}
				}
			}
			this.m_heaviliyModifiedCommandCache[key2] = list;
		}
		foreach (InputManager.KeyCodeCacheEntry keyCodeCacheEntry in this.m_heaviliyModifiedCommandCache[key2])
		{
			if (this.IsModifierDown((KeyCode)keyCodeCacheEntry.m_modifierKey, (KeyCode)keyCodeCacheEntry.m_additionalModifierKey))
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckKeyAction(KeyCode key, KeyCode modifierKey, KeyCode additionalModifierKey, InputManager.KeyActionType keyDownType)
	{
		bool flag = false;
		if (key != KeyCode.None)
		{
			if (keyDownType != InputManager.KeyActionType.KeyHeld)
			{
				if (keyDownType != InputManager.KeyActionType.KeyDown)
				{
					if (keyDownType != InputManager.KeyActionType.KeyUp)
					{
					}
					else
					{
						flag = Input.GetKeyUp(key);
					}
				}
				else
				{
					flag = Input.GetKeyDown(key);
				}
			}
			else
			{
				flag = Input.GetKey(key);
			}
		}
		if (flag)
		{
			bool flag2;
			if (this.IsModifierDown(modifierKey, additionalModifierKey))
			{
				flag2 = !this.IsMoreHeavilyModifiedKeyCommandDown(key, modifierKey, additionalModifierKey);
			}
			else
			{
				flag2 = false;
			}
			flag = flag2;
		}
		return flag;
	}

	internal bool IsKeyBindingHeld(KeyPreference actionName)
	{
		bool flag = false;
		if (actionName != KeyPreference.NullPreference)
		{
			if (!UIUtils.InputFieldHasFocus())
			{
				if (!UIUtils.SettingKeybindCommand())
				{
					if (!AccountPreferences.DoesApplicationHaveFocus())
					{
					}
					else
					{
						KeyCodeData keyCodeData = null;
						this.m_keyCodeMapping.TryGetValue((int)actionName, out keyCodeData);
						if (keyCodeData != null)
						{
							bool flag2;
							if (!this.CheckKeyAction((KeyCode)keyCodeData.m_primary, (KeyCode)keyCodeData.m_modifierKey1, (KeyCode)keyCodeData.m_additionalModifierKey1, InputManager.KeyActionType.KeyHeld))
							{
								flag2 = this.CheckKeyAction((KeyCode)keyCodeData.m_secondary, (KeyCode)keyCodeData.m_modifierKey2, (KeyCode)keyCodeData.m_additionalModifierKey2, InputManager.KeyActionType.KeyHeld);
							}
							else
							{
								flag2 = true;
							}
							flag = flag2;
						}
						if (flag)
						{
							return flag;
						}
						if (!this.m_controlPadMapping.ContainsKey((int)actionName))
						{
							return flag;
						}
						ControlpadInputValue controlpadInputValue = this.m_controlPadMapping[(int)actionName];
						if (controlpadInputValue != ControlpadInputValue.INVALID)
						{
							return ControlpadGameplay.Get().GetButton(controlpadInputValue);
						}
						return flag;
					}
				}
			}
			flag = false;
		}
		return flag;
	}

	internal bool IsKeyBindingNewlyHeld(KeyPreference actionName)
	{
		bool flag = false;
		if (actionName != KeyPreference.NullPreference)
		{
			if (!UIUtils.InputFieldHasFocus() && !UIUtils.SettingKeybindCommand())
			{
				if (!AccountPreferences.DoesApplicationHaveFocus())
				{
				}
				else
				{
					KeyCodeData keyCodeData = null;
					this.m_keyCodeMapping.TryGetValue((int)actionName, out keyCodeData);
					if (keyCodeData != null)
					{
						bool flag2;
						if (!this.CheckKeyAction((KeyCode)keyCodeData.m_primary, (KeyCode)keyCodeData.m_modifierKey1, (KeyCode)keyCodeData.m_additionalModifierKey1, InputManager.KeyActionType.KeyDown))
						{
							flag2 = this.CheckKeyAction((KeyCode)keyCodeData.m_secondary, (KeyCode)keyCodeData.m_modifierKey2, (KeyCode)keyCodeData.m_additionalModifierKey2, InputManager.KeyActionType.KeyDown);
						}
						else
						{
							flag2 = true;
						}
						flag = flag2;
					}
					if (flag || !this.m_controlPadMapping.ContainsKey((int)actionName))
					{
						return flag;
					}
					ControlpadInputValue controlpadInputValue = ControlpadInputValue.INVALID;
					this.m_controlPadMapping.TryGetValue((int)actionName, out controlpadInputValue);
					if (controlpadInputValue != ControlpadInputValue.INVALID)
					{
						return ControlpadGameplay.Get().GetButtonDown(controlpadInputValue);
					}
					return flag;
				}
			}
			flag = false;
		}
		return flag;
	}

	internal bool IsKeyBindingNewlyReleased(KeyPreference actionName)
	{
		bool flag = false;
		if (actionName != KeyPreference.NullPreference)
		{
			if (!UIUtils.InputFieldHasFocus() && !UIUtils.SettingKeybindCommand())
			{
				if (!AccountPreferences.DoesApplicationHaveFocus())
				{
				}
				else
				{
					KeyCodeData keyCodeData = null;
					this.m_keyCodeMapping.TryGetValue((int)actionName, out keyCodeData);
					if (keyCodeData != null)
					{
						bool flag2;
						if (!this.CheckKeyAction((KeyCode)keyCodeData.m_primary, (KeyCode)keyCodeData.m_modifierKey1, (KeyCode)keyCodeData.m_additionalModifierKey1, InputManager.KeyActionType.KeyUp))
						{
							flag2 = this.CheckKeyAction((KeyCode)keyCodeData.m_secondary, (KeyCode)keyCodeData.m_modifierKey2, (KeyCode)keyCodeData.m_additionalModifierKey2, InputManager.KeyActionType.KeyUp);
						}
						else
						{
							flag2 = true;
						}
						flag = flag2;
					}
					if (flag)
					{
						return flag;
					}
					if (!this.m_controlPadMapping.ContainsKey((int)actionName))
					{
						return flag;
					}
					ControlpadInputValue controlpadInputValue = ControlpadInputValue.INVALID;
					this.m_controlPadMapping.TryGetValue((int)actionName, out controlpadInputValue);
					if (controlpadInputValue != ControlpadInputValue.INVALID)
					{
						return ControlpadGameplay.Get().GetButtonUp(controlpadInputValue);
					}
					return flag;
				}
			}
			flag = false;
		}
		return flag;
	}

	internal bool GetAcceptButtonDown()
	{
		return ControlpadGameplay.Get().GetButtonDown(ControlpadInputValue.Button_A);
	}

	internal bool GetCancelButtonDown()
	{
		return ControlpadGameplay.Get().GetButtonDown(ControlpadInputValue.Button_B);
	}

	internal bool IsKeyCodeMatchKeyBind(KeyPreference actionName, KeyCode code)
	{
		bool result = false;
		if (actionName != KeyPreference.NullPreference)
		{
			KeyCodeData keyCodeData = null;
			if (this.m_keyCodeMapping.TryGetValue((int)actionName, out keyCodeData))
			{
				bool flag;
				if (keyCodeData.m_primary != (int)code)
				{
					flag = (keyCodeData.m_secondary == (int)code);
				}
				else
				{
					flag = true;
				}
				result = flag;
			}
		}
		return result;
	}

	public string GetFullKeyString(KeyPreference actionName, bool primaryKey, bool shortStr = false)
	{
		string result = string.Empty;
		KeyCodeData keyCodeData = null;
		if (this.m_keyCodeMapping.TryGetValue((int)actionName, out keyCodeData))
		{
			if (primaryKey)
			{
				result = this.GetFullKeyString((KeyCode)keyCodeData.m_primary, (KeyCode)keyCodeData.m_modifierKey1, (KeyCode)keyCodeData.m_additionalModifierKey1, shortStr);
			}
			else
			{
				result = this.GetFullKeyString((KeyCode)keyCodeData.m_secondary, (KeyCode)keyCodeData.m_modifierKey2, (KeyCode)keyCodeData.m_additionalModifierKey2, shortStr);
			}
		}
		return result;
	}

	private string GetFullKeyString(KeyCode key, KeyCode modifierKey, KeyCode additionalModifierKey, bool shortStr = false)
	{
		string result = string.Empty;
		string text = this.GetKeyString(key, shortStr);
		text = text.ToUpper();
		string modifierKeyString = this.GetModifierKeyString(modifierKey, shortStr);
		string modifierKeyString2 = this.GetModifierKeyString(additionalModifierKey, shortStr);
		if (!text.IsNullOrEmpty())
		{
			if (modifierKeyString.IsNullOrEmpty())
			{
				if (modifierKeyString2.IsNullOrEmpty())
				{
					return text;
				}
			}
			if (shortStr)
			{
				result = string.Format("{0}{1}-{2}", modifierKeyString, modifierKeyString2, text);
			}
			else
			{
				if (!modifierKeyString.IsNullOrEmpty())
				{
					if (modifierKeyString2.IsNullOrEmpty())
					{
						return string.Format("{0} {1}", modifierKeyString, text);
					}
				}
				result = string.Format("{0} {1} {2}", modifierKeyString, modifierKeyString2, text);
			}
		}
		return result;
	}

	private string GetModifierKeyString(KeyCode key, bool shortStr)
	{
		string result = string.Empty;
		switch (key)
		{
		case KeyCode.RightShift:
		case KeyCode.LeftShift:
			if (shortStr)
			{
				result = StringUtil.TR("ShiftShort", "Keyboard");
			}
			else
			{
				result = StringUtil.TR("Shift", "Keyboard");
			}
			break;
		case KeyCode.RightControl:
		case KeyCode.LeftControl:
			if (shortStr)
			{
				result = StringUtil.TR("CtrlShort", "Keyboard");
			}
			else
			{
				result = StringUtil.TR("Ctrl", "Keyboard");
			}
			break;
		case KeyCode.RightAlt:
		case KeyCode.LeftAlt:
			if (shortStr)
			{
				result = StringUtil.TR("AltShort", "Keyboard");
			}
			else
			{
				result = StringUtil.TR("Alt", "Keyboard");
			}
			break;
		}
		return result;
	}

	private string GetKeyString(KeyCode key, bool shortStr)
	{
		string result = string.Empty;
		switch (key)
		{
		case KeyCode.Keypad0:
			result = StringUtil.TR("NumPad0", "Keyboard");
			break;
		case KeyCode.Keypad1:
			result = StringUtil.TR("NumPad1", "Keyboard");
			break;
		case KeyCode.Keypad2:
			result = StringUtil.TR("NumPad2", "Keyboard");
			break;
		case KeyCode.Keypad3:
			result = StringUtil.TR("NumPad3", "Keyboard");
			break;
		case KeyCode.Keypad4:
			result = StringUtil.TR("NumPad4", "Keyboard");
			break;
		case KeyCode.Keypad5:
			result = StringUtil.TR("NumPad5", "Keyboard");
			break;
		case KeyCode.Keypad6:
			result = StringUtil.TR("NumPad6", "Keyboard");
			break;
		case KeyCode.Keypad7:
			result = StringUtil.TR("NumPad7", "Keyboard");
			break;
		case KeyCode.Keypad8:
			result = StringUtil.TR("NumPad8", "Keyboard");
			break;
		case KeyCode.Keypad9:
			result = StringUtil.TR("NumPad9", "Keyboard");
			break;
		default:
			switch (key)
			{
			case KeyCode.Comma:
				result = ",";
				break;
			case KeyCode.Minus:
				result = "-";
				break;
			case KeyCode.Period:
				result = ".";
				break;
			default:
				switch (key)
				{
				case KeyCode.Mouse2:
				case KeyCode.Mouse3:
				case KeyCode.Mouse4:
				case KeyCode.Mouse5:
				case KeyCode.Mouse6:
				{
					int num = key - KeyCode.Mouse0 + 1;
					if (shortStr)
					{
						result = string.Format(StringUtil.TR("MouseButtonShort", "Keyboard"), num);
					}
					else
					{
						result = string.Format(StringUtil.TR("MouseButton", "Keyboard"), num);
					}
					break;
				}
				default:
					switch (key)
					{
					case KeyCode.LeftBracket:
						result = "[";
						break;
					case KeyCode.Backslash:
						result = "\\";
						break;
					case KeyCode.RightBracket:
						result = "]";
						break;
					default:
						switch (key)
						{
						case KeyCode.Backspace:
							result = StringUtil.TR("Backspace", "Keyboard");
							break;
						case KeyCode.Tab:
							result = StringUtil.TR("Tab", "Keyboard");
							break;
						default:
							if (key != KeyCode.None)
							{
								if (key != KeyCode.Pause)
								{
									if (key != KeyCode.Escape)
									{
										if (key != KeyCode.Space)
										{
											if (key != KeyCode.Quote)
											{
												if (key != KeyCode.Delete)
												{
													result = key.ToString();
												}
												else
												{
													result = StringUtil.TR("Delete", "Keyboard");
												}
											}
											else
											{
												result = "'";
											}
										}
										else
										{
											result = StringUtil.TR("Space", "Keyboard");
										}
									}
									else
									{
										result = StringUtil.TR("Escape", "Keyboard");
									}
								}
								else
								{
									result = StringUtil.TR("Pause", "Keyboard");
								}
							}
							break;
						case KeyCode.Return:
							result = StringUtil.TR("Return", "Keyboard");
							break;
						}
						break;
					case KeyCode.BackQuote:
						result = "`";
						break;
					}
					break;
				}
				break;
			case KeyCode.Alpha0:
				result = "0";
				break;
			case KeyCode.Alpha1:
				result = "1";
				break;
			case KeyCode.Alpha2:
				result = "2";
				break;
			case KeyCode.Alpha3:
				result = "3";
				break;
			case KeyCode.Alpha4:
				result = "4";
				break;
			case KeyCode.Alpha5:
				result = "5";
				break;
			case KeyCode.Alpha6:
				result = "6";
				break;
			case KeyCode.Alpha7:
				result = "7";
				break;
			case KeyCode.Alpha8:
				result = "8";
				break;
			case KeyCode.Alpha9:
				result = "9";
				break;
			case KeyCode.Equals:
				result = "=";
				break;
			}
			break;
		case KeyCode.KeypadDivide:
			result = StringUtil.TR("NumPadDivide", "Keyboard");
			break;
		case KeyCode.KeypadMultiply:
			result = StringUtil.TR("NumPadMultiply", "Keyboard");
			break;
		case KeyCode.KeypadMinus:
			result = StringUtil.TR("NumPadMinus", "Keyboard");
			break;
		case KeyCode.KeypadPlus:
			result = StringUtil.TR("NumPadPlus", "Keyboard");
			break;
		case KeyCode.UpArrow:
			result = StringUtil.TR("UpArrow", "Keyboard");
			break;
		case KeyCode.DownArrow:
			result = StringUtil.TR("DownArrow", "Keyboard");
			break;
		case KeyCode.RightArrow:
			result = StringUtil.TR("RightArrow", "Keyboard");
			break;
		case KeyCode.LeftArrow:
			result = StringUtil.TR("LeftArrow", "Keyboard");
			break;
		case KeyCode.Insert:
			result = StringUtil.TR("Insert", "Keyboard");
			break;
		case KeyCode.Home:
			result = StringUtil.TR("Home", "Keyboard");
			break;
		case KeyCode.End:
			result = StringUtil.TR("End", "Keyboard");
			break;
		case KeyCode.PageUp:
			result = StringUtil.TR("PageUp", "Keyboard");
			break;
		case KeyCode.PageDown:
			result = StringUtil.TR("PageDown", "Keyboard");
			break;
		case KeyCode.Numlock:
			result = StringUtil.TR("NumLock", "Keyboard");
			break;
		case KeyCode.CapsLock:
			result = StringUtil.TR("CapsLock", "Keyboard");
			break;
		case KeyCode.RightShift:
			if (shortStr)
			{
				result = StringUtil.TR("RightShiftShort", "Keyboard");
			}
			else
			{
				result = StringUtil.TR("RightShift", "Keyboard");
			}
			break;
		case KeyCode.LeftShift:
			if (shortStr)
			{
				result = StringUtil.TR("LeftShiftShort", "Keyboard");
			}
			else
			{
				result = StringUtil.TR("LeftShift", "Keyboard");
			}
			break;
		case KeyCode.RightControl:
			if (shortStr)
			{
				result = StringUtil.TR("RightCtrlShort", "Keyboard");
			}
			else
			{
				result = StringUtil.TR("RightCtrl", "Keyboard");
			}
			break;
		case KeyCode.LeftControl:
			if (shortStr)
			{
				result = StringUtil.TR("LeftCtrlShort", "Keyboard");
			}
			else
			{
				result = StringUtil.TR("LeftCtrl", "Keyboard");
			}
			break;
		case KeyCode.RightAlt:
			if (shortStr)
			{
				result = StringUtil.TR("RightAltShort", "Keyboard");
			}
			else
			{
				result = StringUtil.TR("RightAlt", "Keyboard");
			}
			break;
		case KeyCode.LeftAlt:
			if (shortStr)
			{
				result = StringUtil.TR("LeftAltShort", "Keyboard");
			}
			else
			{
				result = StringUtil.TR("LeftAlt", "Keyboard");
			}
			break;
		case KeyCode.Print:
			result = StringUtil.TR("PrintScreen", "Keyboard");
			break;
		}
		return result;
	}

	public void KeyCommandDisplay(KeyPreference keyPreference)
	{
		string arg = StringUtil.TR_KeyBindCommand(keyPreference.ToString());
		string fullKeyString = this.GetFullKeyString(keyPreference, true, false);
		TextConsole.Get().Write(string.Format("key:{0} maps to:{1}", arg, fullKeyString), ConsoleMessageType.SystemMessage);
	}

	private void BuildRazorKeyLookupMap()
	{
		this.m_razerKeyMapping = new Dictionary<KeyCode, Key>();
		this.m_razerKeyMapping[KeyCode.Alpha0] = Key.D0;
		this.m_razerKeyMapping[KeyCode.Alpha1] = Key.D1;
		this.m_razerKeyMapping[KeyCode.Alpha2] = Key.D2;
		this.m_razerKeyMapping[KeyCode.Alpha3] = Key.D3;
		this.m_razerKeyMapping[KeyCode.Alpha4] = Key.D4;
		this.m_razerKeyMapping[KeyCode.Alpha5] = Key.D5;
		this.m_razerKeyMapping[KeyCode.Alpha6] = Key.D6;
		this.m_razerKeyMapping[KeyCode.Alpha7] = Key.D7;
		this.m_razerKeyMapping[KeyCode.Alpha8] = Key.D8;
		this.m_razerKeyMapping[KeyCode.Alpha9] = Key.D9;
		this.m_razerKeyMapping[KeyCode.Keypad0] = Key.Num0;
		this.m_razerKeyMapping[KeyCode.Keypad1] = Key.Num1;
		this.m_razerKeyMapping[KeyCode.Keypad2] = Key.Num2;
		this.m_razerKeyMapping[KeyCode.Keypad3] = Key.Num3;
		this.m_razerKeyMapping[KeyCode.Keypad4] = Key.Num4;
		this.m_razerKeyMapping[KeyCode.Keypad5] = Key.Num5;
		this.m_razerKeyMapping[KeyCode.Keypad6] = Key.Num6;
		this.m_razerKeyMapping[KeyCode.Keypad7] = Key.Num7;
		this.m_razerKeyMapping[KeyCode.Keypad8] = Key.Num8;
		this.m_razerKeyMapping[KeyCode.Keypad9] = Key.Num9;
		this.m_razerKeyMapping[KeyCode.KeypadPeriod] = Key.NumDecimal;
		this.m_razerKeyMapping[KeyCode.KeypadDivide] = Key.NumDivide;
		this.m_razerKeyMapping[KeyCode.KeypadMultiply] = Key.NumMultiply;
		this.m_razerKeyMapping[KeyCode.KeypadMinus] = Key.NumSubtract;
		this.m_razerKeyMapping[KeyCode.KeypadPlus] = Key.NumAdd;
		this.m_razerKeyMapping[KeyCode.KeypadEnter] = Key.NumEnter;
		this.m_razerKeyMapping[KeyCode.UpArrow] = Key.Up;
		this.m_razerKeyMapping[KeyCode.DownArrow] = Key.Down;
		this.m_razerKeyMapping[KeyCode.RightArrow] = Key.Right;
		this.m_razerKeyMapping[KeyCode.LeftArrow] = Key.Left;
		this.m_razerKeyMapping[KeyCode.Numlock] = Key.NumLock;
		IEnumerator enumerator = Enum.GetValues(typeof(KeyCode)).GetEnumerator();
		try
		{
			IL_38D:
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				KeyCode key = (KeyCode)obj;
				IEnumerator enumerator2 = Enum.GetValues(typeof(Key)).GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						Key value = (Key)obj2;
						if (!(key.ToString() == value.ToString()))
						{
							if (!(key.ToString() == value.ToString().Replace("Oem", string.Empty)))
							{
								continue;
							}
						}
						this.m_razerKeyMapping[key] = value;
						goto IL_38D;
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator2 as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
	}

	public unsafe bool GetRazorKey(KeyPreference actionName, out Key razerKey)
	{
		if (this.m_razerKeyMapping.IsNullOrEmpty<KeyValuePair<KeyCode, Key>>())
		{
			this.BuildRazorKeyLookupMap();
		}
		razerKey = Key.Invalid;
		KeyCodeData keyCodeData = null;
		this.m_keyCodeMapping.TryGetValue((int)actionName, out keyCodeData);
		if (keyCodeData != null)
		{
			KeyCode key;
			if (keyCodeData.m_primary != 0)
			{
				key = (KeyCode)keyCodeData.m_primary;
			}
			else
			{
				if (keyCodeData.m_secondary == 0)
				{
					return false;
				}
				key = (KeyCode)keyCodeData.m_secondary;
			}
			if (this.m_razerKeyMapping.TryGetValue(key, out razerKey))
			{
				return true;
			}
		}
		return false;
	}

	internal enum KeyActionType
	{
		KeyHeld,
		KeyDown,
		KeyUp
	}

	internal struct KeyCodeCacheEntry
	{
		public int m_primary;

		public int m_modifierKey;

		public int m_additionalModifierKey;
	}
}
