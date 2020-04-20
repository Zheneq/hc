using System;
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
		UIUtils.s_inputHasFocusLastSetTime = -1f;
	}

	internal static bool InputFieldHasFocus()
	{
		if (Time.time == UIUtils.s_inputHasFocusLastSetTime)
		{
			return UIUtils.s_inputHasFocusValueThisFrame;
		}
		bool flag = false;
		if (EventSystem.current != null)
		{
			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
			if (currentSelectedGameObject != null)
			{
				if (UIUtils.s_prevSelectedGameObject == currentSelectedGameObject)
				{
					flag = UIUtils.s_prevHasInputField;
					goto IL_D2;
				}
			}
			if (currentSelectedGameObject != null)
			{
				flag = (currentSelectedGameObject.GetComponent<TMP_InputField>() != null);
				if (!flag)
				{
					flag = (currentSelectedGameObject.GetComponent<InputField>() != null);
				}
				UIUtils.s_prevSelectedGameObject = currentSelectedGameObject;
				UIUtils.s_prevHasInputField = flag;
			}
			IL_D2:
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
		}
		UIUtils.s_inputHasFocusLastSetTime = Time.time;
		UIUtils.s_inputHasFocusValueThisFrame = flag;
		return flag;
	}

	internal static bool SettingKeybindCommand()
	{
		if (KeyBinding_UI.Get() != null)
		{
			if (KeyBinding_UI.Get().IsSettingKeybindCommand())
			{
				return true;
			}
		}
		if (UIFrontEnd.Get())
		{
			if (UIFrontEnd.Get().m_frontEndNavPanel != null)
			{
				return UIFrontEnd.Get().m_frontEndNavPanel.m_voiceListMenu.m_pushToTalkClickBlocker.gameObject.activeSelf;
			}
		}
		return false;
	}

	internal static bool IsMouseInGameWindow()
	{
		Rect rect = new Rect(1f, 1f, (float)(Screen.width - 1), (float)(Screen.height - 1));
		return rect.Contains(Input.mousePosition);
	}

	internal static bool IsFullScreenTakeoverVisible()
	{
		if (Options_UI.Get() != null)
		{
			if (Options_UI.Get().IsVisible())
			{
				goto IL_9B;
			}
		}
		if (KeyBinding_UI.Get() != null)
		{
			if (KeyBinding_UI.Get().IsVisible())
			{
				goto IL_9B;
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
		return result != 0;
		IL_9B:
		result = 1;
		return result != 0;
	}

	internal static bool IsMouseOnGUI()
	{
		if (UIUtils.IsFullScreenTakeoverVisible())
		{
			return true;
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
		UIUtils.TextWithOutline(layoutRect, text, style, outlineColor, outlineOffset);
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
			text2 = "0" + text2;
		}
		string text3 = Mathf.RoundToInt(color.g * 255f).ToString("X");
		if (text3.Length == 1)
		{
			text3 = "0" + text3;
		}
		string text4 = Mathf.RoundToInt(color.b * 255f).ToString("X");
		if (text4.Length == 1)
		{
			text4 = "0" + text4;
		}
		string text5 = Mathf.RoundToInt(color.a * 255f).ToString("X");
		if (text5.Length == 1)
		{
			text5 = "0" + text5;
		}
		string text6 = text;
		return string.Concat(new string[]
		{
			text6,
			text2,
			text3,
			text4,
			text5,
			">"
		});
	}

	public static string ColorToNGUIRichTextTag(Color color)
	{
		string text = "[";
		string text2 = Mathf.RoundToInt(color.r * 255f).ToString("X");
		if (text2.Length == 1)
		{
			text2 = "0" + text2;
		}
		string text3 = Mathf.RoundToInt(color.g * 255f).ToString("X");
		if (text3.Length == 1)
		{
			text3 = "0" + text3;
		}
		string text4 = Mathf.RoundToInt(color.b * 255f).ToString("X");
		if (text4.Length == 1)
		{
			text4 = "0" + text4;
		}
		string text5 = text;
		return string.Concat(new string[]
		{
			text5,
			text2,
			text3,
			text4,
			"]"
		});
	}

	public unsafe static void SweepRectRect(Rect rectA, Rect rectB, Vector2 velocity, out float hitTime)
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
		if (velocity.y != 0f)
		{
			hitTime = Mathf.Min(hitTime, num2 / Mathf.Abs(velocity.y));
		}
	}

	internal static void SetAsLastSiblingIfNeeded(Transform transform)
	{
		if (transform != null && transform.parent != null)
		{
			if (transform.GetSiblingIndex() != transform.parent.childCount - 1)
			{
				transform.SetAsLastSibling();
			}
		}
	}
}
