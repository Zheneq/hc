using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIGraphicSettingsNotification : MonoBehaviour
{
	public RectTransform m_graphicSettingsContainer;

	public _SelectableBtn m_closeBtn;

	private static UIGraphicSettingsNotification s_instance;

	private static bool s_isVisible;

	private static UIGraphicSettingsNotification.CloseCallback m_callback;

	private void Awake()
	{
		UIGraphicSettingsNotification.s_instance = this;
		UIManager.SetGameObjectActive(this.m_graphicSettingsContainer, false, null);
		this.m_closeBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseClicked);
	}

	public void CloseClicked(BaseEventData data)
	{
		if (UIGraphicSettingsNotification.m_callback != null)
		{
			UIGraphicSettingsNotification.m_callback();
		}
		UIGraphicSettingsNotification.SetVisible(false, null);
	}

	public static void SetVisible(bool visible, UIGraphicSettingsNotification.CloseCallback callback)
	{
		UIGraphicSettingsNotification.s_isVisible = visible;
		UIGraphicSettingsNotification.m_callback = callback;
		if (visible)
		{
			if (UIGraphicSettingsNotification.s_instance == null)
			{
				Log.Warning("Called to display Graphic Settings Notification Visible while HUD has not been loaded in yet", new object[0]);
			}
		}
	}

	private void Update()
	{
		if (this.m_graphicSettingsContainer != null)
		{
			if (this.m_graphicSettingsContainer.gameObject.activeSelf != UIGraphicSettingsNotification.s_isVisible)
			{
				UIManager.SetGameObjectActive(this.m_graphicSettingsContainer, UIGraphicSettingsNotification.s_isVisible, null);
			}
		}
	}

	public delegate void CloseCallback();
}
