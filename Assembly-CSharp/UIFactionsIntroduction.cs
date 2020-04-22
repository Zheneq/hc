using LobbyGameClientMessages;
using UnityEngine;

public class UIFactionsIntroduction : MonoBehaviour
{
	public GameObject m_unlockNoticeContainer;

	public GameObject m_factionsDescriptionContainer;

	public _ButtonSwapSprite m_noticeCloseBtn;

	private static UIFactionsIntroduction s_instance;

	public static UIFactionsIntroduction Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		UIManager.SetGameObjectActive(m_unlockNoticeContainer, false);
		UIManager.SetGameObjectActive(m_factionsDescriptionContainer, false);
		m_noticeCloseBtn.callback = delegate
		{
			UIManager.SetGameObjectActive(m_unlockNoticeContainer, false);
		};
	}

	private void CloseQuestListPanel()
	{
		if (UIFrontEnd.Get().m_frontEndNavPanel.m_notificationsBtn.IsSelected())
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
			UIFrontEnd.Get().m_frontEndNavPanel.m_notificationsBtn.SetSelected(false, false, string.Empty, string.Empty);
		}
		QuestListPanel.Get().SetVisible(false);
	}

	public void SetupIntro(QuestOfferNotification quests)
	{
		CloseQuestListPanel();
		UIManager.SetGameObjectActive(m_unlockNoticeContainer, true);
	}

	public bool IsActive()
	{
		int result;
		if (!m_unlockNoticeContainer.activeInHierarchy)
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
			result = (m_factionsDescriptionContainer.activeInHierarchy ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}
}
