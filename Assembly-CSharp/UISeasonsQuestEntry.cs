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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonsQuestEntry.UpdateCurrentProgressValue(int, int, QuestComponent, int)).MethodHandle;
			}
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
				if (questComponent.GetCompletedCount(questID) > 0)
				{
					return maxProgress;
				}
			}
		}
		if (!this.m_infoReference.Completed)
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
			if (this.m_infoReference is UISeasonQuestDisplayInfo)
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
				UISeasonQuestDisplayInfo uiseasonQuestDisplayInfo = this.m_infoReference as UISeasonQuestDisplayInfo;
				if (!uiseasonQuestDisplayInfo.IsQuestStatic)
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
					DateTime dateTime = ClientGameManager.Get().PacificNow();
					bool flag;
					if (uiseasonQuestDisplayInfo.ChapterStartDate < dateTime)
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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonsQuestEntry.SetProgress(int, int, QuestComponent, int)).MethodHandle;
			}
			UISeasonQuestDisplayInfo uiseasonQuestDisplayInfo = this.m_infoReference as UISeasonQuestDisplayInfo;
			if (!uiseasonQuestDisplayInfo.IsQuestStatic)
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
				if (uiseasonQuestDisplayInfo.Completed)
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
					UIManager.SetGameObjectActive(this.m_lockedChapterOverlay, false, null);
				}
				else
				{
					DateTime dateTime = ClientGameManager.Get().PacificNow();
					bool flag;
					if (uiseasonQuestDisplayInfo.ChapterStartDate < dateTime)
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonsQuestEntry.Setup(UISeasonQuestDisplayInfo, bool)).MethodHandle;
			}
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
