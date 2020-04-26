using UnityEngine;
using UnityEngine.EventSystems;

public class UIGraphicSettingsNotification : MonoBehaviour
{
	public delegate void CloseCallback();

	public RectTransform m_graphicSettingsContainer;

	public _SelectableBtn m_closeBtn;

	private static UIGraphicSettingsNotification s_instance;

	private static bool s_isVisible;

	private static CloseCallback m_callback;

	private void Awake()
	{
		s_instance = this;
		UIManager.SetGameObjectActive(m_graphicSettingsContainer, false);
		m_closeBtn.spriteController.callback = CloseClicked;
	}

	public void CloseClicked(BaseEventData data)
	{
		if (m_callback != null)
		{
			m_callback();
		}
		SetVisible(false, null);
	}

	public static void SetVisible(bool visible, CloseCallback callback)
	{
		s_isVisible = visible;
		m_callback = callback;
		if (!visible)
		{
			return;
		}
		while (true)
		{
			if (s_instance == null)
			{
				Log.Warning("Called to display Graphic Settings Notification Visible while HUD has not been loaded in yet");
			}
			return;
		}
	}

	private void Update()
	{
		if (!(m_graphicSettingsContainer != null))
		{
			return;
		}
		while (true)
		{
			if (m_graphicSettingsContainer.gameObject.activeSelf != s_isVisible)
			{
				while (true)
				{
					UIManager.SetGameObjectActive(m_graphicSettingsContainer, s_isVisible);
					return;
				}
			}
			return;
		}
	}
}
