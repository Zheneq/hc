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
		SetExpanded(m_expanded, true);
	}

	protected override int UpdateCurrentProgressValue(int currentProgress, int maxProgress, QuestComponent questComponent, int questID)
	{
		if (m_infoReference.Completed)
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
				if (questComponent.GetCompletedCount(questID) > 0)
				{
					return maxProgress;
				}
			}
		}
		if (!m_infoReference.Completed)
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
			if (m_infoReference is UISeasonQuestDisplayInfo)
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
				UISeasonQuestDisplayInfo uISeasonQuestDisplayInfo = m_infoReference as UISeasonQuestDisplayInfo;
				if (!uISeasonQuestDisplayInfo.IsQuestStatic)
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
					DateTime dateTime = ClientGameManager.Get().PacificNow();
					int num;
					if (uISeasonQuestDisplayInfo.ChapterStartDate < dateTime)
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
						num = ((dateTime <= uISeasonQuestDisplayInfo.ChapterEndDate) ? 1 : 0);
					}
					else
					{
						num = 0;
					}
					if (num == 0 || !questComponent.Progress.ContainsKey(questID))
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
		if (m_infoReference is UISeasonQuestDisplayInfo)
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
			UISeasonQuestDisplayInfo uISeasonQuestDisplayInfo = m_infoReference as UISeasonQuestDisplayInfo;
			if (!uISeasonQuestDisplayInfo.IsQuestStatic)
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
				if (uISeasonQuestDisplayInfo.Completed)
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
					UIManager.SetGameObjectActive(m_lockedChapterOverlay, false);
				}
				else
				{
					DateTime dateTime = ClientGameManager.Get().PacificNow();
					int num;
					if (uISeasonQuestDisplayInfo.ChapterStartDate < dateTime)
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
						num = ((dateTime <= uISeasonQuestDisplayInfo.ChapterEndDate) ? 1 : 0);
					}
					else
					{
						num = 0;
					}
					bool flag = (byte)num != 0;
					UIManager.SetGameObjectActive(m_lockedChapterOverlay, !flag);
				}
			}
		}
		UIManager.SetGameObjectActive(m_CompletedContainer, currentProgress == maxProgress);
		RectTransform inProgressContainer = m_InProgressContainer;
		int doActive;
		if (currentProgress > 0)
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
			doActive = ((currentProgress < maxProgress) ? 1 : 0);
		}
		else
		{
			doActive = 0;
		}
		UIManager.SetGameObjectActive(inProgressContainer, (byte)doActive != 0);
	}

	public void Setup(UISeasonQuestDisplayInfo displayInfo, bool seasonChapterLocked)
	{
		if (displayInfo.IsQuestStatic)
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
			UIManager.SetGameObjectActive(m_lockedChapterOverlay, seasonChapterLocked);
		}
		UIManager.SetGameObjectActive(m_abandonElement, false);
		Setup(displayInfo);
	}

	public override float GetExpandedHeight()
	{
		float num = 15f;
		num += (m_headerElement.transform as RectTransform).rect.height;
		num += (m_detailsElement.transform as RectTransform).rect.height + 12f;
		return num + (m_rewardsElement.transform as RectTransform).rect.height;
	}

	protected override void PlayExpandAnimation()
	{
		UIManager.SetGameObjectActive(m_expandedGroup, true);
		m_animationController.Play("SeasonChallengeEntryExpandedIN");
	}

	protected override void PlayContractAnimation()
	{
		m_animationController.Play("SeasonChallengeEntryContractedIN");
	}
}
