using System;
using UnityEngine;
using UnityEngine.UI;

public class UISeasonsQuestEntry : UISeasonsBaseContract
{
	[Header("Seasons Quest Entry")]
	public RectTransform m_CompletedContainer;

	public RectTransform m_InProgressContainer;

	public Image m_RewardImage;

	public RectTransform m_lockedChapterOverlay;

	private void OnEnable()
	{
		base.SetExpanded(this.m_expanded, true);
	}

	protected override int UpdateCurrentProgressValue(int currentProgress, int maxProgress, QuestComponent questComponent, int questID)
	{
		if (this.m_infoReference.Completed)
		{
			if (questComponent != null)
			{
				if (questComponent.GetCompletedCount(questID) > 0)
				{
					return maxProgress;
				}
			}
		}
		if (!this.m_infoReference.Completed)
		{
			if (this.m_infoReference is UISeasonQuestDisplayInfo)
			{
				UISeasonQuestDisplayInfo uiseasonQuestDisplayInfo = this.m_infoReference as UISeasonQuestDisplayInfo;
				if (!uiseasonQuestDisplayInfo.IsQuestStatic)
				{
					DateTime dateTime = ClientGameManager.Get().PacificNow();
					bool flag;
					if (uiseasonQuestDisplayInfo.ChapterStartDate < dateTime)
					{
						flag = (dateTime <= uiseasonQuestDisplayInfo.ChapterEndDate);
					}
					else
					{
						flag = false;
					}
					bool flag2 = flag;
					if (!flag2 || !questComponent.Progress.ContainsKey(questID))
					{
						return 0;
					}
				}
			}
		}
		return currentProgress;
	}

	protected override void SetProgress(int currentProgress, int maxProgress, QuestComponent questComponent, int questID)
	{
		base.SetProgress(currentProgress, maxProgress, questComponent, questID);
		if (this.m_infoReference is UISeasonQuestDisplayInfo)
		{
			UISeasonQuestDisplayInfo uiseasonQuestDisplayInfo = this.m_infoReference as UISeasonQuestDisplayInfo;
			if (!uiseasonQuestDisplayInfo.IsQuestStatic)
			{
				if (uiseasonQuestDisplayInfo.Completed)
				{
					UIManager.SetGameObjectActive(this.m_lockedChapterOverlay, false, null);
				}
				else
				{
					DateTime dateTime = ClientGameManager.Get().PacificNow();
					bool flag;
					if (uiseasonQuestDisplayInfo.ChapterStartDate < dateTime)
					{
						flag = (dateTime <= uiseasonQuestDisplayInfo.ChapterEndDate);
					}
					else
					{
						flag = false;
					}
					bool flag2 = flag;
					UIManager.SetGameObjectActive(this.m_lockedChapterOverlay, !flag2, null);
				}
			}
		}
		UIManager.SetGameObjectActive(this.m_CompletedContainer, currentProgress == maxProgress, null);
		Component inProgressContainer = this.m_InProgressContainer;
		bool doActive;
		if (currentProgress > 0)
		{
			doActive = (currentProgress < maxProgress);
		}
		else
		{
			doActive = false;
		}
		UIManager.SetGameObjectActive(inProgressContainer, doActive, null);
	}

	public void Setup(UISeasonQuestDisplayInfo displayInfo, bool seasonChapterLocked)
	{
		if (displayInfo.IsQuestStatic)
		{
			UIManager.SetGameObjectActive(this.m_lockedChapterOverlay, seasonChapterLocked, null);
		}
		UIManager.SetGameObjectActive(this.m_abandonElement, false, null);
		base.Setup(displayInfo);
	}

	public override float GetExpandedHeight()
	{
		float num = 15f;
		num += (this.m_headerElement.transform as RectTransform).rect.height;
		num += (this.m_detailsElement.transform as RectTransform).rect.height + 12f;
		return num + (this.m_rewardsElement.transform as RectTransform).rect.height;
	}

	protected override void PlayExpandAnimation()
	{
		UIManager.SetGameObjectActive(this.m_expandedGroup, true, null);
		this.m_animationController.Play("SeasonChallengeEntryExpandedIN");
	}

	protected override void PlayContractAnimation()
	{
		this.m_animationController.Play("SeasonChallengeEntryContractedIN");
	}
}
