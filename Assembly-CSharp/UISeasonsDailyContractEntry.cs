using LobbyGameClientMessages;
using UnityEngine;
using UnityEngine.UI;

public class UISeasonsDailyContractEntry : UISeasonsBaseContract
{
	public RectTransform m_emptyContainer;

	public Image m_contractImage;

	private int questIndex;

	private QuestTemplate QuestTemplateRef;

	private QuestItemState m_questItemState;

	private Sprite m_defaultSprite;

	protected override void Init()
	{
		if (m_initialized)
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
					return;
				}
			}
		}
		base.Init();
		m_defaultSprite = m_contractImage.sprite;
	}

	public void Setup(int questID)
	{
		Init();
		questIndex = questID;
		QuestTemplateRef = null;
		if (-1 < questID - 1)
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
			if (questID - 1 < QuestWideData.Get().m_quests.Count)
			{
				QuestTemplateRef = QuestWideData.Get().m_quests[questID - 1];
			}
		}
		if (QuestTemplateRef != null)
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
			Sprite sprite = Resources.Load<Sprite>(QuestTemplateRef.IconFilename);
			if ((bool)sprite)
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
				m_contractImage.sprite = sprite;
			}
			else
			{
				m_contractImage.sprite = m_defaultSprite;
			}
		}
		UIBaseQuestDisplayInfo uIBaseQuestDisplayInfo = new UIBaseQuestDisplayInfo();
		uIBaseQuestDisplayInfo.Setup(questIndex);
		Setup(uIBaseQuestDisplayInfo);
	}

	public UIBaseQuestDisplayInfo DeleteCache()
	{
		UIBaseQuestDisplayInfo infoReference = m_infoReference;
		m_infoReference = null;
		return infoReference;
	}

	public bool UpdateProgress(QuestProgress newProgress)
	{
		if (m_infoReference != null)
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
			if (m_infoReference.QuestProgressRef.Id == newProgress.Id)
			{
				UIBaseQuestDisplayInfo uIBaseQuestDisplayInfo = DeleteCache();
				uIBaseQuestDisplayInfo.QuestProgressRef = newProgress;
				Setup(uIBaseQuestDisplayInfo);
				return true;
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
		return false;
	}

	protected override void DoExpand(bool expanded)
	{
		base.DoExpand(expanded);
		if (!expanded)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			QuestListPanel.Get().NotifyEntryExpanded(this);
			return;
		}
	}

	protected override int GetRejectedCount()
	{
		int result = 0;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
			QuestComponent questComponent = playerAccountData.QuestComponent;
			questComponent = playerAccountData.QuestComponent;
			int index = m_infoReference.QuestTemplateRef.Index;
			if (questComponent != null)
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
				result = questComponent.GetRejectedCount(index);
			}
		}
		return result;
	}

	public void SetState(QuestItemState newState)
	{
		if (m_questItemState == newState)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		m_questItemState = newState;
		UIManager.SetGameObjectActive(m_contractedRewardsContainer, m_questItemState != QuestItemState.Empty);
		if (m_questItemState == QuestItemState.Empty)
		{
			UIManager.SetGameObjectActive(m_contractImage, false);
			UIManager.SetGameObjectActive(m_progressText, false);
			UIManager.SetGameObjectActive(m_emptyContainer, true);
			UIManager.SetGameObjectActive(base.gameObject, true);
			UIManager.SetGameObjectActive(m_btnHitBox.spriteController.m_defaultImage, false);
			UIManager.SetGameObjectActive(m_btnHitBox.spriteController.m_hoverImage, false);
			UIManager.SetGameObjectActive(m_btnHitBox.spriteController.m_pressedImage, false);
			UIManager.SetGameObjectActive(m_btnHitBox.m_selectedContainer, false);
			UIManager.SetGameObjectActive(m_btnHitBox.spriteController, false);
			return;
		}
		if (m_questItemState == QuestItemState.Filled)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					UIManager.SetGameObjectActive(base.gameObject, true);
					UIManager.SetGameObjectActive(m_contractImage, true);
					UIManager.SetGameObjectActive(m_progressText, true);
					UIManager.SetGameObjectActive(m_emptyContainer, false);
					UIManager.SetGameObjectActive(m_btnHitBox.spriteController.m_defaultImage, true);
					UIManager.SetGameObjectActive(m_btnHitBox.spriteController.m_hoverImage, false);
					UIManager.SetGameObjectActive(m_btnHitBox.spriteController.m_pressedImage, false);
					UIManager.SetGameObjectActive(m_btnHitBox.m_selectedContainer, false);
					UIManager.SetGameObjectActive(m_btnHitBox.spriteController, true);
					return;
				}
			}
		}
		if (m_questItemState == QuestItemState.Expanded)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					UIManager.SetGameObjectActive(base.gameObject, false);
					UIManager.SetGameObjectActive(m_contractImage, true);
					UIManager.SetGameObjectActive(m_progressText, true);
					UIManager.SetGameObjectActive(m_emptyContainer, false);
					UIManager.SetGameObjectActive(m_btnHitBox.spriteController.m_defaultImage, false);
					UIManager.SetGameObjectActive(m_btnHitBox.spriteController.m_hoverImage, false);
					UIManager.SetGameObjectActive(m_btnHitBox.spriteController.m_pressedImage, false);
					UIManager.SetGameObjectActive(m_btnHitBox.m_selectedContainer, false);
					UIManager.SetGameObjectActive(m_btnHitBox.spriteController, true);
					return;
				}
			}
		}
		if (m_questItemState == QuestItemState.Finished)
		{
			UIManager.SetGameObjectActive(base.gameObject, true);
			UIManager.SetGameObjectActive(m_contractImage, true);
			UIManager.SetGameObjectActive(m_progressText, true);
			UIManager.SetGameObjectActive(m_emptyContainer, false);
			UIManager.SetGameObjectActive(m_btnHitBox.spriteController.m_defaultImage, true);
			UIManager.SetGameObjectActive(m_btnHitBox.spriteController.m_hoverImage, false);
			UIManager.SetGameObjectActive(m_btnHitBox.spriteController.m_pressedImage, false);
			UIManager.SetGameObjectActive(m_btnHitBox.m_selectedContainer, false);
			UIManager.SetGameObjectActive(m_btnHitBox.spriteController, true);
		}
	}

	public void AbandonDailyQuestResponseHandler(AbandonDailyQuestResponse response)
	{
		if (response.Success)
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
			SetExpanded(false);
		}
		SetTrashSelected(false);
	}

	public override void AbandonQuest()
	{
		base.AbandonQuest();
		ClientGameManager.Get().AbandonDailyQuest(questIndex, AbandonDailyQuestResponseHandler);
	}

	protected override void NotifyDoneAnimating()
	{
		UIManager.SetGameObjectActive(m_expandedGroup, m_expanded);
	}

	protected override void PlayExpandAnimation()
	{
		m_animationController.Play("SeasonChallengeEntryExpandedIN");
	}

	protected override void PlayContractAnimation()
	{
		m_animationController.Play("SeasonChallengeEntryContractedIN");
	}
}
