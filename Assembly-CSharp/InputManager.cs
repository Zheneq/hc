using Corale.Colore.Razer.Keyboard;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
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

	internal bool KeyMappingInitialized;

	public Dictionary<int, KeyCodeData> m_keyCodeMapping;

	public Dictionary<int, ControlpadInputValue> m_controlPadMapping;

	private static InputManager s_instance;

	private Dictionary<KeyCode, Key> m_razerKeyMapping;

	private Dictionary<KeyCodeCacheEntry, List<KeyCodeCacheEntry>> m_heaviliyModifiedCommandCache;

	private void Awake()
	{
		s_instance = this;
		m_keyCodeMapping = new Dictionary<int, KeyCodeData>();
		m_controlPadMapping = new Dictionary<int, ControlpadInputValue>();
		m_heaviliyModifiedCommandCache = new Dictionary<KeyCodeCacheEntry, List<KeyCodeCacheEntry>>();
	}

	private void Start()
	{
		SetDefaultKeyMapping();
		ClientGameManager.Get().OnAccountDataUpdated += HandleAccountDataUpdated;
	}

	private void OnDestroy()
	{
		s_instance = null;
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ClientGameManager.Get().OnAccountDataUpdated -= HandleAccountDataUpdated;
			return;
		}
	}

	public static InputManager Get()
	{
		return s_instance;
	}

	public void SetDefaultKeyMapping()
	{
		m_keyCodeMapping.Clear();
		m_controlPadMapping.Clear();
		m_heaviliyModifiedCommandCache.Clear();
		for (int i = 0; i < AccountPreferences.Get().GetKeyDefaultsSize(); i++)
		{
			AccountPreferences.KeyCodeDefault keyCodeDefault = AccountPreferences.Get().GetKeyCodeDefault(i);
			m_keyCodeMapping.Add((int)keyCodeDefault.m_preference, new KeyCodeData
			{
				m_primary = (int)keyCodeDefault.m_primary,
				m_modifierKey1 = (int)keyCodeDefault.m_modifierKey1,
				m_additionalModifierKey1 = (int)keyCodeDefault.m_additionalModifierKey1,
				m_secondary = (int)keyCodeDefault.m_secondary,
				m_modifierKey2 = (int)keyCodeDefault.m_modifierKey2,
				m_additionalModifierKey2 = (int)keyCodeDefault.m_additionalModifierKey2
			});
		}
		m_controlPadMapping[203] = ControlpadInputValue.Button_X;
		m_controlPadMapping[200] = ControlpadInputValue.Button_Y;
		m_controlPadMapping[201] = ControlpadInputValue.Button_B;
		m_controlPadMapping[1] = ControlpadInputValue.Button_leftStickIn;
		m_controlPadMapping[210] = ControlpadInputValue.Button_rightShoulder;
		m_controlPadMapping[204] = ControlpadInputValue.Button_start;
		m_controlPadMapping[2] = ControlpadInputValue.Button_rightStickIn;
	}

	public void ClearKeyBind(KeyPreference m_preference, bool primary)
	{
		KeyCodeData value = null;
		if (!m_keyCodeMapping.TryGetValue((int)m_preference, out value))
		{
			return;
		}
		if (primary)
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
			value.m_primary = 0;
			value.m_modifierKey1 = 0;
			value.m_additionalModifierKey1 = 0;
		}
		else
		{
			value.m_secondary = 0;
			value.m_modifierKey2 = 0;
			value.m_additionalModifierKey2 = 0;
		}
		m_heaviliyModifiedCommandCache.Clear();
	}

	public bool SetCustomKeyBind(KeyPreference preference, KeyCode keyCode, KeyCode modifierKey, KeyCode additionalModifierKey, bool primary)
	{
		KeyCodeData value = null;
		KeyBindingCommand keyBindingCommand = GameWideData.Get().GetKeyBindingCommand(preference.ToString());
		if (keyBindingCommand == null)
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
					return false;
				}
			}
		}
		if (m_keyCodeMapping.TryGetValue((int)preference, out value))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					if (primary)
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
						value.m_primary = (int)keyCode;
						value.m_modifierKey1 = (int)modifierKey;
						value.m_additionalModifierKey1 = (int)additionalModifierKey;
					}
					else
					{
						value.m_secondary = (int)keyCode;
						value.m_modifierKey2 = (int)modifierKey;
						value.m_additionalModifierKey2 = (int)additionalModifierKey;
					}
					using (Dictionary<int, KeyCodeData>.Enumerator enumerator = m_keyCodeMapping.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<int, KeyCodeData> current = enumerator.Current;
							KeyPreference key = (KeyPreference)current.Key;
							KeyBindingCommand keyBindingCommand2 = GameWideData.Get().GetKeyBindingCommand(key.ToString());
							if (keyBindingCommand2 == null)
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
								Log.Error("Could not find KeyBindingCommand for {0} in GameWideData", key.ToString());
								continue;
							}
							if (!keyBindingCommand2.Settable)
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
								continue;
							}
							if (keyBindingCommand.Category != 0 && keyBindingCommand2.Category != 0)
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
								if (keyBindingCommand2.Category != keyBindingCommand.Category)
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
									continue;
								}
							}
							if (key == preference)
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
								if (primary)
								{
									goto IL_01ed;
								}
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
							}
							if (current.Value.m_primary == (int)keyCode)
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
								if (current.Value.m_modifierKey1 == (int)modifierKey)
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
									if (current.Value.m_additionalModifierKey1 == (int)additionalModifierKey)
									{
										current.Value.m_primary = 0;
										current.Value.m_modifierKey1 = 0;
										current.Value.m_additionalModifierKey1 = 0;
									}
								}
							}
							goto IL_01ed;
							IL_01ed:
							if (key == preference)
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
								if (!primary)
								{
									continue;
								}
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
							}
							if (current.Value.m_secondary == (int)keyCode)
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
								if (current.Value.m_modifierKey2 == (int)modifierKey && current.Value.m_additionalModifierKey2 == (int)additionalModifierKey)
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
									current.Value.m_secondary = 0;
									current.Value.m_modifierKey2 = 0;
									current.Value.m_additionalModifierKey2 = 0;
								}
							}
						}
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					m_heaviliyModifiedCommandCache.Clear();
					return true;
				}
				}
			}
		}
		return false;
	}

	public void SaveAllKeyBinds()
	{
		ClientGameManager.Get().NotifyCustomKeyBinds(m_keyCodeMapping);
	}

	public void HandleAccountDataUpdated(PersistedAccountData accountData)
	{
		if (KeyMappingInitialized)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_keyCodeMapping.Clear();
			m_heaviliyModifiedCommandCache.Clear();
			bool flag = false;
			for (int i = 0; i < AccountPreferences.Get().GetKeyDefaultsSize(); i++)
			{
				AccountPreferences.KeyCodeDefault keyCodeDefault = AccountPreferences.Get().GetKeyCodeDefault(i);
				int preference = (int)keyCodeDefault.m_preference;
				KeyCodeData value = null;
				if (accountData.AccountComponent.KeyCodeMapping.TryGetValue(preference, out value))
				{
					m_keyCodeMapping.Add(preference, value);
					continue;
				}
				m_keyCodeMapping.Add(preference, new KeyCodeData
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
			if (flag)
			{
				SaveAllKeyBinds();
			}
			KeyMappingInitialized = true;
			return;
		}
	}

	public bool IsModifierKey(KeyCode key)
	{
		int result;
		if (key != KeyCode.RightControl)
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
			if (key != KeyCode.LeftControl)
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
				if (key != KeyCode.RightShift && key != KeyCode.LeftShift && key != KeyCode.RightAlt)
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
					result = ((key == KeyCode.LeftAlt) ? 1 : 0);
					goto IL_005a;
				}
			}
		}
		result = 1;
		goto IL_005a;
		IL_005a:
		return (byte)result != 0;
	}

	private KeyCode CombineLeftRightModifiers(KeyCode key)
	{
		KeyCode result = key;
		switch (key)
		{
		case KeyCode.RightControl:
		case KeyCode.LeftControl:
			result = KeyCode.LeftControl;
			break;
		case KeyCode.RightShift:
		case KeyCode.LeftShift:
			result = KeyCode.LeftShift;
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
		int result;
		if (!Input.GetKey(KeyCode.LeftControl))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = (Input.GetKey(KeyCode.RightControl) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public bool IsAltDown()
	{
		return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
	}

	public bool IsShiftDown()
	{
		int result;
		if (!Input.GetKey(KeyCode.LeftShift))
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
			result = (Input.GetKey(KeyCode.RightShift) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public void GetModifierKeys(out KeyCode modifier, out KeyCode additionalModifier)
	{
		modifier = KeyCode.None;
		additionalModifier = KeyCode.None;
		if (IsControlDown())
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
			modifier = KeyCode.LeftControl;
		}
		else if (IsAltDown())
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
			modifier = KeyCode.LeftAlt;
		}
		else if (IsShiftDown())
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
			modifier = KeyCode.LeftShift;
		}
		if (modifier == KeyCode.LeftControl)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (IsAltDown())
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								additionalModifier = KeyCode.LeftAlt;
								return;
							}
						}
					}
					if (IsShiftDown())
					{
						additionalModifier = KeyCode.LeftShift;
					}
					return;
				}
			}
		}
		if (modifier != KeyCode.LeftAlt)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (IsShiftDown())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					additionalModifier = KeyCode.LeftShift;
					return;
				}
			}
			return;
		}
	}

	public bool IsUnbindableKey(KeyCode key)
	{
		int result;
		if (key != KeyCode.Pause)
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
			if (key != KeyCode.ScrollLock && key != KeyCode.Break && key != KeyCode.Mouse0 && key != KeyCode.Mouse1 && key != KeyCode.Menu)
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
				if (key != KeyCode.Slash && key != KeyCode.Return)
				{
					result = ((key == KeyCode.KeypadEnter) ? 1 : 0);
					goto IL_005f;
				}
			}
		}
		result = 1;
		goto IL_005f;
		IL_005f:
		return (byte)result != 0;
	}

	private bool CheckModifierDown(KeyCode modifierKey)
	{
		bool result = false;
		switch (modifierKey)
		{
		case KeyCode.RightControl:
		case KeyCode.LeftControl:
			result = IsControlDown();
			break;
		case KeyCode.RightShift:
		case KeyCode.LeftShift:
			result = IsShiftDown();
			break;
		case KeyCode.RightAlt:
		case KeyCode.LeftAlt:
			result = IsAltDown();
			break;
		}
		return result;
	}

	private bool IsModifierDown(KeyCode modifierKey, KeyCode additionalModifierKey)
	{
		bool flag = true;
		if (modifierKey != 0)
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
			flag = CheckModifierDown(modifierKey);
		}
		if (flag && additionalModifierKey != 0)
		{
			flag = CheckModifierDown(additionalModifierKey);
		}
		return flag;
	}

	private bool IsMoreHeavilyModifiedKeyCommandDown(KeyCode key, KeyCode modifierKey, KeyCode additionalModifierKey)
	{
		KeyCodeCacheEntry key2 = default(KeyCodeCacheEntry);
		key2.m_primary = (int)key;
		key2.m_modifierKey = (int)modifierKey;
		key2.m_additionalModifierKey = (int)additionalModifierKey;
		if (!m_heaviliyModifiedCommandCache.ContainsKey(key2))
		{
			List<KeyCodeCacheEntry> list = new List<KeyCodeCacheEntry>();
			using (Dictionary<int, KeyCodeData>.Enumerator enumerator = m_keyCodeMapping.GetEnumerator())
			{
				KeyCodeCacheEntry item = default(KeyCodeCacheEntry);
				KeyCodeCacheEntry item2 = default(KeyCodeCacheEntry);
				while (enumerator.MoveNext())
				{
					KeyCodeData value = enumerator.Current.Value;
					if (value.m_primary == (int)key)
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
						if (modifierKey == KeyCode.None)
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
							if (value.m_modifierKey1 != 0)
							{
								goto IL_00af;
							}
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						if (additionalModifierKey == KeyCode.None && value.m_additionalModifierKey1 != 0)
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
							goto IL_00af;
						}
					}
					goto IL_00e1;
					IL_00af:
					item.m_primary = value.m_primary;
					item.m_modifierKey = value.m_modifierKey1;
					item.m_additionalModifierKey = value.m_additionalModifierKey1;
					list.Add(item);
					goto IL_00e1;
					IL_0121:
					item2.m_primary = value.m_primary;
					item2.m_modifierKey = value.m_modifierKey2;
					item2.m_additionalModifierKey = value.m_additionalModifierKey2;
					list.Add(item2);
					continue;
					IL_00e1:
					if (value.m_secondary != (int)key)
					{
						continue;
					}
					if (modifierKey == KeyCode.None)
					{
						if (value.m_modifierKey2 != 0)
						{
							goto IL_0121;
						}
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					if (additionalModifierKey != 0)
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
						break;
					}
					if (value.m_additionalModifierKey2 == 0)
					{
						continue;
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					goto IL_0121;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			m_heaviliyModifiedCommandCache[key2] = list;
		}
		foreach (KeyCodeCacheEntry item3 in m_heaviliyModifiedCommandCache[key2])
		{
			if (IsModifierDown((KeyCode)item3.m_modifierKey, (KeyCode)item3.m_additionalModifierKey))
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
		}
		return false;
	}

	private bool CheckKeyAction(KeyCode key, KeyCode modifierKey, KeyCode additionalModifierKey, KeyActionType keyDownType)
	{
		bool flag = false;
		if (key != 0)
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
			if (keyDownType != 0)
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
				if (keyDownType != KeyActionType.KeyDown)
				{
					if (keyDownType != KeyActionType.KeyUp)
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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			int num;
			if (IsModifierDown(modifierKey, additionalModifierKey))
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
				num = ((!IsMoreHeavilyModifiedKeyCommandDown(key, modifierKey, additionalModifierKey)) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			flag = ((byte)num != 0);
		}
		return flag;
	}

	internal bool IsKeyBindingHeld(KeyPreference actionName)
	{
		bool flag = false;
		if (actionName != 0)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!UIUtils.InputFieldHasFocus())
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
				if (!UIUtils.SettingKeybindCommand())
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
					if (AccountPreferences.DoesApplicationHaveFocus())
					{
						KeyCodeData value = null;
						m_keyCodeMapping.TryGetValue((int)actionName, out value);
						if (value != null)
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
							int num;
							if (!CheckKeyAction((KeyCode)value.m_primary, (KeyCode)value.m_modifierKey1, (KeyCode)value.m_additionalModifierKey1, KeyActionType.KeyHeld))
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
								num = (CheckKeyAction((KeyCode)value.m_secondary, (KeyCode)value.m_modifierKey2, (KeyCode)value.m_additionalModifierKey2, KeyActionType.KeyHeld) ? 1 : 0);
							}
							else
							{
								num = 1;
							}
							flag = ((byte)num != 0);
						}
						if (!flag)
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
							if (m_controlPadMapping.ContainsKey((int)actionName))
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
								ControlpadInputValue controlpadInputValue = m_controlPadMapping[(int)actionName];
								if (controlpadInputValue != ControlpadInputValue.INVALID)
								{
									flag = ControlpadGameplay.Get().GetButton(controlpadInputValue);
								}
							}
						}
						goto IL_0105;
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			flag = false;
		}
		goto IL_0105;
		IL_0105:
		return flag;
	}

	internal bool IsKeyBindingNewlyHeld(KeyPreference actionName)
	{
		bool flag = false;
		if (actionName != 0)
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
			if (!UIUtils.InputFieldHasFocus() && !UIUtils.SettingKeybindCommand())
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
				if (AccountPreferences.DoesApplicationHaveFocus())
				{
					KeyCodeData value = null;
					m_keyCodeMapping.TryGetValue((int)actionName, out value);
					if (value != null)
					{
						int num;
						if (!CheckKeyAction((KeyCode)value.m_primary, (KeyCode)value.m_modifierKey1, (KeyCode)value.m_additionalModifierKey1, KeyActionType.KeyDown))
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
							num = (CheckKeyAction((KeyCode)value.m_secondary, (KeyCode)value.m_modifierKey2, (KeyCode)value.m_additionalModifierKey2, KeyActionType.KeyDown) ? 1 : 0);
						}
						else
						{
							num = 1;
						}
						flag = ((byte)num != 0);
					}
					if (!flag && m_controlPadMapping.ContainsKey((int)actionName))
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
						ControlpadInputValue value2 = ControlpadInputValue.INVALID;
						m_controlPadMapping.TryGetValue((int)actionName, out value2);
						if (value2 != ControlpadInputValue.INVALID)
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
							flag = ControlpadGameplay.Get().GetButtonDown(value2);
						}
					}
					goto IL_00f5;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			flag = false;
		}
		goto IL_00f5;
		IL_00f5:
		return flag;
	}

	internal bool IsKeyBindingNewlyReleased(KeyPreference actionName)
	{
		bool flag = false;
		if (actionName != 0)
		{
			if (!UIUtils.InputFieldHasFocus() && !UIUtils.SettingKeybindCommand())
			{
				if (AccountPreferences.DoesApplicationHaveFocus())
				{
					KeyCodeData value = null;
					m_keyCodeMapping.TryGetValue((int)actionName, out value);
					if (value != null)
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
						int num;
						if (!CheckKeyAction((KeyCode)value.m_primary, (KeyCode)value.m_modifierKey1, (KeyCode)value.m_additionalModifierKey1, KeyActionType.KeyUp))
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
							num = (CheckKeyAction((KeyCode)value.m_secondary, (KeyCode)value.m_modifierKey2, (KeyCode)value.m_additionalModifierKey2, KeyActionType.KeyUp) ? 1 : 0);
						}
						else
						{
							num = 1;
						}
						flag = ((byte)num != 0);
					}
					if (!flag)
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
						if (m_controlPadMapping.ContainsKey((int)actionName))
						{
							ControlpadInputValue value2 = ControlpadInputValue.INVALID;
							m_controlPadMapping.TryGetValue((int)actionName, out value2);
							if (value2 != ControlpadInputValue.INVALID)
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
								flag = ControlpadGameplay.Get().GetButtonUp(value2);
							}
						}
					}
					goto IL_00e9;
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
			}
			flag = false;
		}
		goto IL_00e9;
		IL_00e9:
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
		if (actionName != 0)
		{
			KeyCodeData value = null;
			if (m_keyCodeMapping.TryGetValue((int)actionName, out value))
			{
				int num;
				if (value.m_primary != (int)code)
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
					num = ((value.m_secondary == (int)code) ? 1 : 0);
				}
				else
				{
					num = 1;
				}
				result = ((byte)num != 0);
			}
		}
		return result;
	}

	public string GetFullKeyString(KeyPreference actionName, bool primaryKey, bool shortStr = false)
	{
		string result = string.Empty;
		KeyCodeData value = null;
		if (m_keyCodeMapping.TryGetValue((int)actionName, out value))
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
			if (primaryKey)
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
				result = GetFullKeyString((KeyCode)value.m_primary, (KeyCode)value.m_modifierKey1, (KeyCode)value.m_additionalModifierKey1, shortStr);
			}
			else
			{
				result = GetFullKeyString((KeyCode)value.m_secondary, (KeyCode)value.m_modifierKey2, (KeyCode)value.m_additionalModifierKey2, shortStr);
			}
		}
		return result;
	}

	private string GetFullKeyString(KeyCode key, KeyCode modifierKey, KeyCode additionalModifierKey, bool shortStr = false)
	{
		string result = string.Empty;
		string keyString = GetKeyString(key, shortStr);
		keyString = keyString.ToUpper();
		string modifierKeyString = GetModifierKeyString(modifierKey, shortStr);
		string modifierKeyString2 = GetModifierKeyString(additionalModifierKey, shortStr);
		if (!keyString.IsNullOrEmpty())
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
			if (modifierKeyString.IsNullOrEmpty())
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
				if (modifierKeyString2.IsNullOrEmpty())
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
					result = keyString;
					goto IL_00de;
				}
			}
			if (shortStr)
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
				result = $"{modifierKeyString}{modifierKeyString2}-{keyString}";
			}
			else
			{
				if (!modifierKeyString.IsNullOrEmpty())
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
					if (modifierKeyString2.IsNullOrEmpty())
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
						result = $"{modifierKeyString} {keyString}";
						goto IL_00de;
					}
				}
				result = $"{modifierKeyString} {modifierKeyString2} {keyString}";
			}
		}
		goto IL_00de;
		IL_00de:
		return result;
	}

	private string GetModifierKeyString(KeyCode key, bool shortStr)
	{
		string result = string.Empty;
		switch (key)
		{
		case KeyCode.RightControl:
		case KeyCode.LeftControl:
			if (shortStr)
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
				result = StringUtil.TR("CtrlShort", "Keyboard");
			}
			else
			{
				result = StringUtil.TR("Ctrl", "Keyboard");
			}
			break;
		case KeyCode.RightShift:
		case KeyCode.LeftShift:
			if (shortStr)
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
				result = StringUtil.TR("ShiftShort", "Keyboard");
			}
			else
			{
				result = StringUtil.TR("Shift", "Keyboard");
			}
			break;
		case KeyCode.RightAlt:
		case KeyCode.LeftAlt:
			if (shortStr)
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
		default:
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
			if (key != KeyCode.Space)
			{
				if (key != KeyCode.Quote)
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
					if (key != KeyCode.Delete)
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
			break;
		case KeyCode.UpArrow:
			result = StringUtil.TR("UpArrow", "Keyboard");
			break;
		case KeyCode.LeftArrow:
			result = StringUtil.TR("LeftArrow", "Keyboard");
			break;
		case KeyCode.DownArrow:
			result = StringUtil.TR("DownArrow", "Keyboard");
			break;
		case KeyCode.RightArrow:
			result = StringUtil.TR("RightArrow", "Keyboard");
			break;
		case KeyCode.RightControl:
			if (shortStr)
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
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				result = StringUtil.TR("LeftCtrlShort", "Keyboard");
			}
			else
			{
				result = StringUtil.TR("LeftCtrl", "Keyboard");
			}
			break;
		case KeyCode.RightShift:
			if (shortStr)
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
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				result = StringUtil.TR("LeftShiftShort", "Keyboard");
			}
			else
			{
				result = StringUtil.TR("LeftShift", "Keyboard");
			}
			break;
		case KeyCode.RightAlt:
			if (shortStr)
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
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				result = StringUtil.TR("LeftAltShort", "Keyboard");
			}
			else
			{
				result = StringUtil.TR("LeftAlt", "Keyboard");
			}
			break;
		case KeyCode.Tab:
			result = StringUtil.TR("Tab", "Keyboard");
			break;
		case KeyCode.Escape:
			result = StringUtil.TR("Escape", "Keyboard");
			break;
		case KeyCode.Backspace:
			result = StringUtil.TR("Backspace", "Keyboard");
			break;
		case KeyCode.Numlock:
			result = StringUtil.TR("NumLock", "Keyboard");
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
		case KeyCode.KeypadMultiply:
			result = StringUtil.TR("NumPadMultiply", "Keyboard");
			break;
		case KeyCode.KeypadPlus:
			result = StringUtil.TR("NumPadPlus", "Keyboard");
			break;
		case KeyCode.KeypadMinus:
			result = StringUtil.TR("NumPadMinus", "Keyboard");
			break;
		case KeyCode.KeypadDivide:
			result = StringUtil.TR("NumPadDivide", "Keyboard");
			break;
		case KeyCode.PageUp:
			result = StringUtil.TR("PageUp", "Keyboard");
			break;
		case KeyCode.PageDown:
			result = StringUtil.TR("PageDown", "Keyboard");
			break;
		case KeyCode.Pause:
			result = StringUtil.TR("Pause", "Keyboard");
			break;
		case KeyCode.Return:
			result = StringUtil.TR("Return", "Keyboard");
			break;
		case KeyCode.CapsLock:
			result = StringUtil.TR("CapsLock", "Keyboard");
			break;
		case KeyCode.Print:
			result = StringUtil.TR("PrintScreen", "Keyboard");
			break;
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
		case KeyCode.Alpha0:
			result = "0";
			break;
		case KeyCode.Mouse2:
		case KeyCode.Mouse3:
		case KeyCode.Mouse4:
		case KeyCode.Mouse5:
		case KeyCode.Mouse6:
		{
			int num = (int)(key - 323 + 1);
			if (shortStr)
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
				result = string.Format(StringUtil.TR("MouseButtonShort", "Keyboard"), num);
			}
			else
			{
				result = string.Format(StringUtil.TR("MouseButton", "Keyboard"), num);
			}
			break;
		}
		case KeyCode.BackQuote:
			result = "`";
			break;
		case KeyCode.Minus:
			result = "-";
			break;
		case KeyCode.Equals:
			result = "=";
			break;
		case KeyCode.LeftBracket:
			result = "[";
			break;
		case KeyCode.RightBracket:
			result = "]";
			break;
		case KeyCode.Backslash:
			result = "\\";
			break;
		case KeyCode.Comma:
			result = ",";
			break;
		case KeyCode.Period:
			result = ".";
			break;
		case KeyCode.None:
			break;
		}
		return result;
	}

	public void KeyCommandDisplay(KeyPreference keyPreference)
	{
		string arg = StringUtil.TR_KeyBindCommand(keyPreference.ToString());
		string fullKeyString = GetFullKeyString(keyPreference, true);
		TextConsole.Get().Write($"key:{arg} maps to:{fullKeyString}");
	}

	private void BuildRazorKeyLookupMap()
	{
		m_razerKeyMapping = new Dictionary<KeyCode, Key>();
		m_razerKeyMapping[KeyCode.Alpha0] = Key.D0;
		m_razerKeyMapping[KeyCode.Alpha1] = Key.D1;
		m_razerKeyMapping[KeyCode.Alpha2] = Key.D2;
		m_razerKeyMapping[KeyCode.Alpha3] = Key.D3;
		m_razerKeyMapping[KeyCode.Alpha4] = Key.D4;
		m_razerKeyMapping[KeyCode.Alpha5] = Key.D5;
		m_razerKeyMapping[KeyCode.Alpha6] = Key.D6;
		m_razerKeyMapping[KeyCode.Alpha7] = Key.D7;
		m_razerKeyMapping[KeyCode.Alpha8] = Key.D8;
		m_razerKeyMapping[KeyCode.Alpha9] = Key.D9;
		m_razerKeyMapping[KeyCode.Keypad0] = Key.Num0;
		m_razerKeyMapping[KeyCode.Keypad1] = Key.Num1;
		m_razerKeyMapping[KeyCode.Keypad2] = Key.Num2;
		m_razerKeyMapping[KeyCode.Keypad3] = Key.Num3;
		m_razerKeyMapping[KeyCode.Keypad4] = Key.Num4;
		m_razerKeyMapping[KeyCode.Keypad5] = Key.Num5;
		m_razerKeyMapping[KeyCode.Keypad6] = Key.Num6;
		m_razerKeyMapping[KeyCode.Keypad7] = Key.Num7;
		m_razerKeyMapping[KeyCode.Keypad8] = Key.Num8;
		m_razerKeyMapping[KeyCode.Keypad9] = Key.Num9;
		m_razerKeyMapping[KeyCode.KeypadPeriod] = Key.NumDecimal;
		m_razerKeyMapping[KeyCode.KeypadDivide] = Key.NumDivide;
		m_razerKeyMapping[KeyCode.KeypadMultiply] = Key.NumMultiply;
		m_razerKeyMapping[KeyCode.KeypadMinus] = Key.NumSubtract;
		m_razerKeyMapping[KeyCode.KeypadPlus] = Key.NumAdd;
		m_razerKeyMapping[KeyCode.KeypadEnter] = Key.NumEnter;
		m_razerKeyMapping[KeyCode.UpArrow] = Key.Up;
		m_razerKeyMapping[KeyCode.DownArrow] = Key.Down;
		m_razerKeyMapping[KeyCode.RightArrow] = Key.Right;
		m_razerKeyMapping[KeyCode.LeftArrow] = Key.Left;
		m_razerKeyMapping[KeyCode.Numlock] = Key.NumLock;
		IEnumerator enumerator = Enum.GetValues(typeof(KeyCode)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				KeyCode key = (KeyCode)enumerator.Current;
				IEnumerator enumerator2 = Enum.GetValues(typeof(Key)).GetEnumerator();
				try
				{
					while (true)
					{
						IL_0357:
						if (!enumerator2.MoveNext())
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
							break;
						}
						Key value = (Key)enumerator2.Current;
						if (!(key.ToString() == value.ToString()))
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
									if (!(key.ToString() == value.ToString().Replace("Oem", string.Empty)))
									{
										goto IL_0357;
									}
									while (true)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										break;
									}
									goto IL_0348;
								}
							}
						}
						goto IL_0348;
						IL_0348:
						m_razerKeyMapping[key] = value;
						break;
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator2 as IDisposable)) != null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								disposable.Dispose();
								goto end_IL_0370;
							}
						}
					}
					end_IL_0370:;
				}
			}
			while (true)
			{
				switch (7)
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
			IDisposable disposable2;
			if ((disposable2 = (enumerator as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
	}

	public bool GetRazorKey(KeyPreference actionName, out Key razerKey)
	{
		if (m_razerKeyMapping.IsNullOrEmpty())
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
			BuildRazorKeyLookupMap();
		}
		razerKey = Key.Invalid;
		KeyCodeData value = null;
		m_keyCodeMapping.TryGetValue((int)actionName, out value);
		if (value != null)
		{
			KeyCode keyCode = KeyCode.None;
			if (value.m_primary != 0)
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
				keyCode = (KeyCode)value.m_primary;
			}
			else
			{
				if (value.m_secondary == 0)
				{
					return false;
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
				keyCode = (KeyCode)value.m_secondary;
			}
			if (m_razerKeyMapping.TryGetValue(keyCode, out razerKey))
			{
				return true;
			}
		}
		return false;
	}
}
