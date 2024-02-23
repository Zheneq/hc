using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class UIUtils
{
	private static GameObject s_prevSelectedGameObject;

	private static bool s_prevHasInputField;

	private static float s_inputHasFocusLastSetTime = -1f;

	private static bool s_inputHasFocusValueThisFrame;

	internal static void MarkInputFieldHasFocusDirty()
	{
		s_inputHasFocusLastSetTime = -1f;
	}

	internal static bool InputFieldHasFocus()
	{
		if (Time.time == s_inputHasFocusLastSetTime)
		{
			while (true)
			{
				return s_inputHasFocusValueThisFrame;
			}
		}
		bool flag = false;
		if (EventSystem.current != null)
		{
			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
			if (currentSelectedGameObject != null)
			{
				if (s_prevSelectedGameObject == currentSelectedGameObject)
				{
					flag = s_prevHasInputField;
					goto IL_00d2;
				}
			}
			if (currentSelectedGameObject != null)
			{
				flag = (currentSelectedGameObject.GetComponent<TMP_InputField>() != null);
				if (!flag)
				{
					flag = (currentSelectedGameObject.GetComponent<InputField>() != null);
				}
				s_prevSelectedGameObject = currentSelectedGameObject;
				s_prevHasInputField = flag;
			}
			goto IL_00d2;
		}
		goto IL_0132;
		IL_00d2:
		if (!flag)
		{
			if (HUD_UI.Get() != null)
			{
				if (HUD_UI.Get().m_textConsole != null)
				{
					flag = HUD_UI.Get().m_textConsole.EscapeJustPressed();
				}
			}
		}
		goto IL_0132;
		IL_0132:
		s_inputHasFocusLastSetTime = Time.time;
		s_inputHasFocusValueThisFrame = flag;
		return flag;
	}

	internal static bool SettingKeybindCommand()
	{
		int result;
		if (KeyBinding_UI.Get() != null)
		{
			if (KeyBinding_UI.Get().IsSettingKeybindCommand())
			{
				result = 1;
				goto IL_00a2;
			}
		}
		if ((bool)UIFrontEnd.Get())
		{
			if (UIFrontEnd.Get().m_frontEndNavPanel != null)
			{
				result = (UIFrontEnd.Get().m_frontEndNavPanel.m_voiceListMenu.m_pushToTalkClickBlocker.gameObject.activeSelf ? 1 : 0);
				goto IL_00a2;
			}
		}
		result = 0;
		goto IL_00a2;
		IL_00a2:
		return (byte)result != 0;
	}

	internal static bool IsMouseInGameWindow()
	{
		bool flag = false;
		return new Rect(1f, 1f, Screen.width - 1, Screen.height - 1).Contains(Input.mousePosition);
	}

	internal static bool IsFullScreenTakeoverVisible()
	{
		if (Options_UI.Get() != null)
		{
			if (Options_UI.Get().IsVisible())
			{
				goto IL_009b;
			}
		}
		if (KeyBinding_UI.Get() != null)
		{
			if (KeyBinding_UI.Get().IsVisible())
			{
				goto IL_009b;
			}
		}
		int result;
		if (UISystemEscapeMenu.Get() != null)
		{
			result = (UISystemEscapeMenu.Get().IsOpen() ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		goto IL_009c;
		IL_009b:
		result = 1;
		goto IL_009c;
		IL_009c:
		return (byte)result != 0;
	}

	internal static bool IsMouseOnGUI()
	{
		if (IsFullScreenTakeoverVisible())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		bool result = false;
		if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1))
		{
			StandaloneInputModuleWithEventDataAccess component = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
			if (component != null)
			{
				if (component.GetLastPointerEventDataPublic(-1).pointerEnter != null)
				{
					Button componentInParent = component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<Button>();
					if (componentInParent != null && UIEventTriggerUtils.HasTriggerOfType(componentInParent.gameObject, EventTriggerType.PointerClick))
					{
						result = true;
					}
				}
			}
		}
		return result;
	}

	public static void TextWithOutline(Rect layoutRect, string text, GUIStyle style, Color textColor, Color outlineColor, int outlineOffset)
	{
		Color textColor2 = style.normal.textColor;
		style.normal.textColor = textColor;
		TextWithOutline(layoutRect, text, style, outlineColor, outlineOffset);
		style.normal.textColor = textColor2;
	}

	public static void TextWithOutline(Rect layoutRect, string text, GUIStyle style, Color outlineColor, int outlineOffset)
	{
		Color textColor = style.normal.textColor;
		float x = layoutRect.x;
		float y = layoutRect.y;
		style.normal.textColor = outlineColor;
		layoutRect.x = x - (float)outlineOffset;
		layoutRect.y = y - (float)outlineOffset;
		GUI.Label(layoutRect, text, style);
		layoutRect.x = x;
		layoutRect.y = y - (float)outlineOffset;
		GUI.Label(layoutRect, text, style);
		layoutRect.x = x + (float)outlineOffset;
		layoutRect.y = y - (float)outlineOffset;
		GUI.Label(layoutRect, text, style);
		layoutRect.x = x - (float)outlineOffset;
		layoutRect.y = y;
		GUI.Label(layoutRect, text, style);
		layoutRect.x = x + (float)outlineOffset;
		layoutRect.y = y;
		GUI.Label(layoutRect, text, style);
		layoutRect.x = x - (float)outlineOffset;
		layoutRect.y = y + (float)outlineOffset;
		GUI.Label(layoutRect, text, style);
		layoutRect.x = x;
		layoutRect.y = y + (float)outlineOffset;
		GUI.Label(layoutRect, text, style);
		layoutRect.x = x + (float)outlineOffset;
		layoutRect.y = y + (float)outlineOffset;
		GUI.Label(layoutRect, text, style);
		style.normal.textColor = textColor;
		layoutRect.x = x;
		layoutRect.y = y;
		GUI.Label(layoutRect, text, style);
	}

	public static string ColorToRichTextTag(Color color)
	{
		string text = "<color=#";
		string text2 = Mathf.RoundToInt(color.r * 255f).ToString("X");
		if (text2.Length == 1)
		{
			text2 = new StringBuilder().Append("0").Append(text2).ToString();
		}
		string text3 = Mathf.RoundToInt(color.g * 255f).ToString("X");
		if (text3.Length == 1)
		{
			text3 = new StringBuilder().Append("0").Append(text3).ToString();
		}
		string text4 = Mathf.RoundToInt(color.b * 255f).ToString("X");
		if (text4.Length == 1)
		{
			text4 = new StringBuilder().Append("0").Append(text4).ToString();
		}
		string text5 = Mathf.RoundToInt(color.a * 255f).ToString("X");
		if (text5.Length == 1)
		{
			text5 = new StringBuilder().Append("0").Append(text5).ToString();
		}
		string text6 = text;
		return new StringBuilder().Append(text6).Append(text2).Append(text3).Append(text4).Append(text5).Append(">").ToString();
	}

	public static string ColorToNGUIRichTextTag(Color color)
	{
		string text = "[";
		string text2 = Mathf.RoundToInt(color.r * 255f).ToString("X");
		if (text2.Length == 1)
		{
			text2 = new StringBuilder().Append("0").Append(text2).ToString();
		}
		string text3 = Mathf.RoundToInt(color.g * 255f).ToString("X");
		if (text3.Length == 1)
		{
			text3 = new StringBuilder().Append("0").Append(text3).ToString();
		}
		string text4 = Mathf.RoundToInt(color.b * 255f).ToString("X");
		if (text4.Length == 1)
		{
			text4 = new StringBuilder().Append("0").Append(text4).ToString();
		}
		string text5 = text;
		return new StringBuilder().Append(text5).Append(text2).Append(text3).Append(text4).Append("]").ToString();
	}

	public static void SweepRectRect(Rect rectA, Rect rectB, Vector2 velocity, out float hitTime)
	{
		float num = float.MaxValue;
		if (velocity.x > 0f)
		{
			if (rectA.xMax < rectB.xMin)
			{
				num = Mathf.Min(num, rectB.xMin - rectA.xMax);
			}
			else if (rectA.xMax < rectB.xMax)
			{
				num = Mathf.Min(num, rectB.xMax - rectA.xMax);
			}
		}
		if (velocity.x < 0f)
		{
			if (rectA.xMin > rectB.xMax)
			{
				num = Mathf.Min(num, rectA.xMin - rectB.xMax);
			}
			else if (rectA.xMin > rectB.xMin)
			{
				num = Mathf.Min(num, rectA.xMin - rectB.xMin);
			}
		}
		float num2 = float.MaxValue;
		if (velocity.y > 0f)
		{
			if (rectA.yMax < rectB.yMin)
			{
				num2 = Mathf.Min(num2, rectB.yMin - rectA.yMax);
			}
			else if (rectA.yMax < rectB.yMax)
			{
				num2 = Mathf.Min(num2, rectB.yMax - rectA.yMax);
			}
		}
		if (velocity.y < 0f)
		{
			if (rectA.yMin > rectB.yMax)
			{
				num2 = Mathf.Min(num2, rectA.yMin - rectB.yMax);
			}
			else if (rectA.yMin > rectB.yMin)
			{
				num2 = Mathf.Min(num2, rectA.yMin - rectB.yMin);
			}
		}
		hitTime = float.MaxValue;
		if (velocity.x != 0f)
		{
			hitTime = Mathf.Min(hitTime, num / Mathf.Abs(velocity.x));
		}
		if (velocity.y == 0f)
		{
			return;
		}
		while (true)
		{
			hitTime = Mathf.Min(hitTime, num2 / Mathf.Abs(velocity.y));
			return;
		}
	}

	internal static void SetAsLastSiblingIfNeeded(Transform transform)
	{
		if (!(transform != null) || !(transform.parent != null))
		{
			return;
		}
		while (true)
		{
			if (transform.GetSiblingIndex() != transform.parent.childCount - 1)
			{
				while (true)
				{
					transform.SetAsLastSibling();
					return;
				}
			}
			return;
		}
	}
}
