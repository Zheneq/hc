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
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_graphicSettingsContainer.gameObject.activeSelf != s_isVisible)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					UIManager.SetGameObjectActive(m_graphicSettingsContainer, s_isVisible);
					return;
				}
			}
			return;
		}
	}
}
