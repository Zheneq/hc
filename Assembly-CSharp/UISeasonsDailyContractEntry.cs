using System;
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
		if (this.m_initialized)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonsDailyContractEntry.Init()).MethodHandle;
			}
			return;
		}
		base.Init();
		this.m_defaultSprite = this.m_contractImage.sprite;
	}

	public void Setup(int questID)
	{
		this.Init();
		this.questIndex = questID;
		this.QuestTemplateRef = null;
		if (-1 < questID - 1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonsDailyContractEntry.Setup(int)).MethodHandle;
			}
			if (questID - 1 < QuestWideData.Get().m_quests.Count)
			{
				this.QuestTemplateRef = QuestWideData.Get().m_quests[questID - 1];
			}
		}
		if (this.QuestTemplateRef != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			Sprite sprite = Resources.Load<Sprite>(this.QuestTemplateRef.IconFilename);
			if (sprite)
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
				this.m_contractImage.sprite = sprite;
			}
			else
			{
				this.m_contractImage.sprite = this.m_defaultSprite;
			}
		}
		UIBaseQuestDisplayInfo uibaseQuestDisplayInfo = new UIBaseQuestDisplayInfo();
		uibaseQuestDisplayInfo.Setup(this.questIndex);
		base.Setup(uibaseQuestDisplayInfo);
	}

	public UIBaseQuestDisplayInfo DeleteCache()
	{
		UIBaseQuestDisplayInfo infoReference = this.m_infoReference;
		this.m_infoReference = null;
		return infoReference;
	}

	public bool UpdateProgress(QuestProgress newProgress)
	{
		if (this.m_infoReference != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonsDailyContractEntry.UpdateProgress(QuestProgress)).MethodHandle;
			}
			if (this.m_infoReference.QuestProgressRef.Id == newProgress.Id)
			{
				UIBaseQuestDisplayInfo uibaseQuestDisplayInfo = this.DeleteCache();
				uibaseQuestDisplayInfo.QuestProgressRef = newProgress;
				base.Setup(uibaseQuestDisplayInfo);
				return true;
			}
			for (;;)
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
		if (expanded)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonsDailyContractEntry.DoExpand(bool)).MethodHandle;
			}
			QuestListPanel.Get().NotifyEntryExpanded(this);
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
			int index = this.m_infoReference.QuestTemplateRef.Index;
			if (questComponent != null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonsDailyContractEntry.GetRejectedCount()).MethodHandle;
				}
				result = questComponent.GetRejectedCount(index);
			}
		}
		return result;
	}

	public void SetState(QuestItemState newState)
	{
		if (this.m_questItemState == newState)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonsDailyContractEntry.SetState(QuestItemState)).MethodHandle;
			}
			return;
		}
		this.m_questItemState = newState;
		UIManager.SetGameObjectActive(this.m_contractedRewardsContainer, this.m_questItemState != QuestItemState.Empty, null);
		if (this.m_questItemState == QuestItemState.Empty)
		{
			UIManager.SetGameObjectActive(this.m_contractImage, false, null);
			UIManager.SetGameObjectActive(this.m_progressText, false, null);
			UIManager.SetGameObjectActive(this.m_emptyContainer, true, null);
			UIManager.SetGameObjectActive(base.gameObject, true, null);
			UIManager.SetGameObjectActive(this.m_btnHitBox.spriteController.m_defaultImage, false, null);
			UIManager.SetGameObjectActive(this.m_btnHitBox.spriteController.m_hoverImage, false, null);
			UIManager.SetGameObjectActive(this.m_btnHitBox.spriteController.m_pressedImage, false, null);
			UIManager.SetGameObjectActive(this.m_btnHitBox.m_selectedContainer, false, null);
			UIManager.SetGameObjectActive(this.m_btnHitBox.spriteController, false, null);
		}
		else if (this.m_questItemState == QuestItemState.Filled)
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
			UIManager.SetGameObjectActive(base.gameObject, true, null);
			UIManager.SetGameObjectActive(this.m_contractImage, true, null);
			UIManager.SetGameObjectActive(this.m_progressText, true, null);
			UIManager.SetGameObjectActive(this.m_emptyContainer, false, null);
			UIManager.SetGameObjectActive(this.m_btnHitBox.spriteController.m_defaultImage, true, null);
			UIManager.SetGameObjectActive(this.m_btnHitBox.spriteController.m_hoverImage, false, null);
			UIManager.SetGameObjectActive(this.m_btnHitBox.spriteController.m_pressedImage, false, null);
			UIManager.SetGameObjectActive(this.m_btnHitBox.m_selectedContainer, false, null);
			UIManager.SetGameObjectActive(this.m_btnHitBox.spriteController, true, null);
		}
		else if (this.m_questItemState == QuestItemState.Expanded)
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
			UIManager.SetGameObjectActive(base.gameObject, false, null);
			UIManager.SetGameObjectActive(this.m_contractImage, true, null);
			UIManager.SetGameObjectActive(this.m_progressText, true, null);
			UIManager.SetGameObjectActive(this.m_emptyContainer, false, null);
			UIManager.SetGameObjectActive(this.m_btnHitBox.spriteController.m_defaultImage, false, null);
			UIManager.SetGameObjectActive(this.m_btnHitBox.spriteController.m_hoverImage, false, null);
			UIManager.SetGameObjectActive(this.m_btnHitBox.spriteController.m_pressedImage, false, null);
			UIManager.SetGameObjectActive(this.m_btnHitBox.m_selectedContainer, false, null);
			UIManager.SetGameObjectActive(this.m_btnHitBox.spriteController, true, null);
		}
		else if (this.m_questItemState == QuestItemState.Finished)
		{
			UIManager.SetGameObjectActive(base.gameObject, true, null);
			UIManager.SetGameObjectActive(this.m_contractImage, true, null);
			UIManager.SetGameObjectActive(this.m_progressText, true, null);
			UIManager.SetGameObjectActive(this.m_emptyContainer, false, null);
			UIManager.SetGameObjectActive(this.m_btnHitBox.spriteController.m_defaultImage, true, null);
			UIManager.SetGameObjectActive(this.m_btnHitBox.spriteController.m_hoverImage, false, null);
			UIManager.SetGameObjectActive(this.m_btnHitBox.spriteController.m_pressedImage, false, null);
			UIManager.SetGameObjectActive(this.m_btnHitBox.m_selectedContainer, false, null);
			UIManager.SetGameObjectActive(this.m_btnHitBox.spriteController, true, null);
		}
	}

	public void AbandonDailyQuestResponseHandler(AbandonDailyQuestResponse response)
	{
		if (response.Success)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonsDailyContractEntry.AbandonDailyQuestResponseHandler(AbandonDailyQuestResponse)).MethodHandle;
			}
			base.SetExpanded(false, false);
		}
		base.SetTrashSelected(false);
	}

	public override void AbandonQuest()
	{
		base.AbandonQuest();
		ClientGameManager.Get().AbandonDailyQuest(this.questIndex, new Action<AbandonDailyQuestResponse>(this.AbandonDailyQuestResponseHandler));
	}

	protected override void NotifyDoneAnimating()
	{
		UIManager.SetGameObjectActive(this.m_expandedGroup, this.m_expanded, null);
	}

	protected override void PlayExpandAnimation()
	{
		this.m_animationController.Play("SeasonChallengeEntryExpandedIN");
	}

	protected override void PlayContractAnimation()
	{
		this.m_animationController.Play("SeasonChallengeEntryContractedIN");
	}
}
