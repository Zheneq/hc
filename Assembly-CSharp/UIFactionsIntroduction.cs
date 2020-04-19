using System;
using LobbyGameClientMessages;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIFactionsIntroduction : MonoBehaviour
{
	public GameObject m_unlockNoticeContainer;

	public GameObject m_factionsDescriptionContainer;

	public _ButtonSwapSprite m_noticeCloseBtn;

	private static UIFactionsIntroduction s_instance;

	public static UIFactionsIntroduction Get()
	{
		return UIFactionsIntroduction.s_instance;
	}

	private void Awake()
	{
		UIFactionsIntroduction.s_instance = this;
		UIManager.SetGameObjectActive(this.m_unlockNoticeContainer, false, null);
		UIManager.SetGameObjectActive(this.m_factionsDescriptionContainer, false, null);
		this.m_noticeCloseBtn.callback = delegate(BaseEventData data)
		{
			UIManager.SetGameObjectActive(this.m_unlockNoticeContainer, false, null);
		};
	}

	private void CloseQuestListPanel()
	{
		if (UIFrontEnd.Get().m_frontEndNavPanel.m_notificationsBtn.IsSelected())
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFactionsIntroduction.CloseQuestListPanel()).MethodHandle;
			}
			UIFrontEnd.Get().m_frontEndNavPanel.m_notificationsBtn.SetSelected(false, false, string.Empty, string.Empty);
		}
		QuestListPanel.Get().SetVisible(false, false, false);
	}

	public void SetupIntro(QuestOfferNotification quests)
	{
		this.CloseQuestListPanel();
		UIManager.SetGameObjectActive(this.m_unlockNoticeContainer, true, null);
	}

	public bool IsActive()
	{
		bool result;
		if (!this.m_unlockNoticeContainer.activeInHierarchy)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFactionsIntroduction.IsActive()).MethodHandle;
			}
			result = this.m_factionsDescriptionContainer.activeInHierarchy;
		}
		else
		{
			result = true;
		}
		return result;
	}
}
