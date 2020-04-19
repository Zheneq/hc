using System;
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
		SystemMenuBroadcast.s_instance = this;
		UIManager.SetGameObjectActive(this.m_SystemMessageContainer, false, null);
		this.m_startDisplaySystemMessage = -1f;
		base.Awake();
	}

	public static SystemMenuBroadcast Get()
	{
		return SystemMenuBroadcast.s_instance;
	}

	public void NotifySystemMessageOutDone()
	{
		UIManager.SetGameObjectActive(UIMainMenu.Get(), false, null);
	}

	public void DisplaySystemMessage(ChatNotification notification)
	{
		string s = (notification.LocalizedText == null) ? notification.Text : notification.LocalizedText.ToString();
		if (s.IsNullOrEmpty())
		{
			return;
		}
		UIManager.SetGameObjectActive(this.m_SystemMessageContainer, true, null);
		TMP_Text systemMessageContent = this.m_SystemMessageContent;
		string text;
		if (notification.LocalizedText != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SystemMenuBroadcast.DisplaySystemMessage(ChatNotification)).MethodHandle;
			}
			text = notification.LocalizedText.ToString();
		}
		else
		{
			text = notification.Text;
		}
		systemMessageContent.text = text;
		this.m_startDisplaySystemMessage = Time.time;
		UISounds.GetUISounds().Play("ui/notification/importantmessage_cs");
	}

	private void Update()
	{
		if (this.m_startDisplaySystemMessage > 0f)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SystemMenuBroadcast.Update()).MethodHandle;
			}
			if (Time.time - this.m_startDisplaySystemMessage >= 5f)
			{
				if (this.m_SystemMessageContainer.GetComponent<Animator>() != null)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_SystemMessageContainer.GetComponent<Animator>().Play("PanelDefaultOUT");
				}
				this.m_startDisplaySystemMessage = -1f;
			}
		}
	}
}
