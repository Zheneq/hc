using LobbyGameClientMessages;
using TMPro;
using UnityEngine;

public class SystemMenuBroadcast : UIScene
{
	public RectTransform m_SystemMessageContainer;

	public TextMeshProUGUI m_SystemMessageContent;

	public const float m_timeToDisplaySystem = 5f;

	private float m_startDisplaySystemMessage;

	private static SystemMenuBroadcast s_instance;

	public override SceneType GetSceneType()
	{
		return SceneType.SystemMessage;
	}

	public override void Awake()
	{
		s_instance = this;
		UIManager.SetGameObjectActive(m_SystemMessageContainer, false);
		m_startDisplaySystemMessage = -1f;
		base.Awake();
	}

	public static SystemMenuBroadcast Get()
	{
		return s_instance;
	}

	public void NotifySystemMessageOutDone()
	{
		UIManager.SetGameObjectActive(UIMainMenu.Get(), false);
	}

	public void DisplaySystemMessage(ChatNotification notification)
	{
		string s = (notification.LocalizedText == null) ? notification.Text : notification.LocalizedText.ToString();
		if (s.IsNullOrEmpty())
		{
			return;
		}
		UIManager.SetGameObjectActive(m_SystemMessageContainer, true);
		TextMeshProUGUI systemMessageContent = m_SystemMessageContent;
		string text;
		if (notification.LocalizedText != null)
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
			text = notification.LocalizedText.ToString();
		}
		else
		{
			text = notification.Text;
		}
		systemMessageContent.text = text;
		m_startDisplaySystemMessage = Time.time;
		UISounds.GetUISounds().Play("ui/notification/importantmessage_cs");
	}

	private void Update()
	{
		if (!(m_startDisplaySystemMessage > 0f))
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
			if (!(Time.time - m_startDisplaySystemMessage >= 5f))
			{
				return;
			}
			if (m_SystemMessageContainer.GetComponent<Animator>() != null)
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
				m_SystemMessageContainer.GetComponent<Animator>().Play("PanelDefaultOUT");
			}
			m_startDisplaySystemMessage = -1f;
			return;
		}
	}
}
