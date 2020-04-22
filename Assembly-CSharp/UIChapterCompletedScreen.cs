using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIChapterCompletedScreen : MonoBehaviour
{
	[Serializable]
	public class Takeover
	{
		public RectTransform m_container;

		public Animator m_animController;

		public TextMeshProUGUI m_ChapterTitleTextLabel;

		public _SelectableBtn m_nextChapter;

		public _SelectableBtn m_okBtn;

		public QuestReward[] m_questRewards;

		internal int m_chapter;
	}

	private struct PendingTakeover
	{
		public Takeover m_takeover;

		public int m_season;

		public int m_chapter;

		public PendingTakeover(Takeover takeover, int season, int chapter)
		{
			m_takeover = takeover;
			m_season = season;
			m_chapter = chapter;
		}
	}

	public Takeover m_completedTakeover;

	public Takeover m_unlockedTakeover;

	public TextMeshProUGUI m_seasonsBlurb;

	[Header("End of Season")]
	public RectTransform m_EndOfSeasonContainer;

	public TextMeshProUGUI m_SeasonEndTitle;

	public Animator m_EndOfSeasonAnimator;

	public TextMeshProUGUI m_SeasonLevelsTitle;

	public TextMeshProUGUI m_SeasonLevelsGained;

	public TextMeshProUGUI m_ReactorLevelsGained;

	public QuestReward[] m_questRewards;

	public _SelectableBtn m_okBtn;

	public RectTransform[] m_levelContainers;

	private Takeover m_currentTakeover;

	private Queue<PendingTakeover> m_pendingTakeovers;

	private bool m_active;

	private bool m_WaitingToDisplayEndOfSeasonTakeover;

	private SeasonStatusNotification m_endSeasonNotification;

	private float[] SeasonLevelUpAnimTimes = new float[7];

	private bool m_playingSeasonPoints;

	private bool m_playingReactorPoints;

	private int m_reactorLevelAtStart;

	private void Awake()
	{
		m_pendingTakeovers = new Queue<PendingTakeover>();
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnChapterUnlockNotification += ChapterUnlocked;
			ClientGameManager.Get().OnChapterCompleteNotification += ChapterCompleted;
			ClientGameManager.Get().OnSeasonCompleteNotification += HandleSeasonStatusNotification;
		}
		m_completedTakeover.m_nextChapter.spriteController.callback = OnNextChapterClicked;
		m_completedTakeover.m_okBtn.spriteController.callback = OkButtonClicked;
		m_unlockedTakeover.m_nextChapter.spriteController.callback = OnNextChapterClicked;
		m_unlockedTakeover.m_okBtn.spriteController.callback = OnNextChapterClicked;
		m_completedTakeover.m_nextChapter.spriteController.m_ignoreDialogboxes = true;
		m_completedTakeover.m_okBtn.spriteController.m_ignoreDialogboxes = true;
		m_unlockedTakeover.m_nextChapter.spriteController.m_ignoreDialogboxes = true;
		m_unlockedTakeover.m_okBtn.spriteController.m_ignoreDialogboxes = true;
		m_okBtn.spriteController.callback = EndOfSeasonOkClicked;
		m_okBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.DialogBoxButton;
	}

	private void OnDestroy()
	{
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			ClientGameManager.Get().OnChapterUnlockNotification -= ChapterUnlocked;
			ClientGameManager.Get().OnChapterCompleteNotification -= ChapterCompleted;
			ClientGameManager.Get().OnSeasonCompleteNotification -= HandleSeasonStatusNotification;
			return;
		}
	}

	private void OnNextChapterClicked(BaseEventData data)
	{
		ClientGameManager.Get().SendSeasonStatusConfirm(SeasonStatusConfirmed.DialogType._000E);
		if (UIFrontEnd.Get() != null)
		{
			if (UIFrontEnd.Get().m_frontEndNavPanel != null)
			{
				UIFrontEnd.Get().m_frontEndNavPanel.SeasonsBtnClicked(null);
			}
		}
		if (UISeasonsPanel.Get() != null)
		{
			UISeasonsPanel.Get().SelectChapter(m_currentTakeover.m_chapter);
		}
		Close();
	}

	private void OkButtonClicked(BaseEventData data)
	{
		Close();
	}

	private void EndOfSeasonOkClicked(BaseEventData data)
	{
		UIAnimationEventManager.Get().PlayAnimation(m_EndOfSeasonAnimator, "EndOfSeasonDefaultOUT", delegate
		{
			m_WaitingToDisplayEndOfSeasonTakeover = false;
			UIManager.SetGameObjectActive(m_EndOfSeasonContainer, false);
			m_endSeasonNotification = null;
			ClientGameManager.Get().SendSeasonStatusConfirm(SeasonStatusConfirmed.DialogType._001D);
		}, string.Empty);
	}

	private void DisplayEndOfSeason()
	{
		if (!m_EndOfSeasonContainer.gameObject.activeSelf)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(m_endSeasonNotification.SeasonEndedIndex);
					if (seasonTemplate != null)
					{
						if (seasonTemplate.IsTutorial)
						{
							m_WaitingToDisplayEndOfSeasonTakeover = false;
							m_endSeasonNotification = null;
							UINewUserFlowManager.MarkSeasonsNew(true);
							if (UIFrontEnd.Get().m_frontEndNavPanel != null)
							{
								ClientGameManager.Get().PreventNextAccountStatusCheck();
								UIFrontEnd.Get().m_frontEndNavPanel.LandingPageBtnClicked(null);
							}
							UINewUserFlowManager.OnTutorialSeasonEnded();
							m_pendingTakeovers.Clear();
							return;
						}
					}
					m_reactorLevelAtStart = m_endSeasonNotification.TotalSeasonLevel - m_endSeasonNotification.SeasonLevelEarnedFromEnded;
					UIManager.SetGameObjectActive(m_EndOfSeasonContainer, true);
					UIFrontEnd.PlaySound(FrontEndButtonSounds.SeasonTransitionIntro);
					m_SeasonEndTitle.text = string.Empty;
					m_SeasonLevelsTitle.text = string.Format(StringUtil.TR("SeasonLevel", "Seasons"), m_endSeasonNotification.SeasonEndedIndex);
					m_SeasonLevelsGained.text = "0";
					m_ReactorLevelsGained.text = m_reactorLevelAtStart.ToString();
					AnimatorClipInfo[] currentAnimatorClipInfo = m_EndOfSeasonAnimator.GetCurrentAnimatorClipInfo(0);
					for (int i = 0; i < currentAnimatorClipInfo.Length; i++)
					{
						if (currentAnimatorClipInfo[i].clip.name == "EndOfSeasonDefaultIN")
						{
							if (currentAnimatorClipInfo[i].clip.events.Length == 7)
							{
								for (int j = 0; j < currentAnimatorClipInfo[i].clip.events.Length; j++)
								{
									SeasonLevelUpAnimTimes[j] = currentAnimatorClipInfo[i].clip.events[j].time / currentAnimatorClipInfo[i].clip.length;
								}
							}
						}
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							m_playingSeasonPoints = false;
							m_playingReactorPoints = false;
							if (seasonTemplate != null)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										break;
									default:
									{
										int num = 0;
										m_SeasonEndTitle.text = seasonTemplate.GetSeasonEndHeader();
										for (int k = 0; k < m_levelContainers.Length; k++)
										{
											UIManager.SetGameObjectActive(m_levelContainers[k], !seasonTemplate.IsTutorial);
										}
										List<SeasonTemplate.SeasonEndRewards> list = new List<SeasonTemplate.SeasonEndRewards>();
										if (m_endSeasonNotification.SeasonLevelEarnedFromEnded >= 2)
										{
											list.Add(seasonTemplate.EndRewards);
											using (List<SeasonTemplate.ConditionalSeasonEndRewards>.Enumerator enumerator = seasonTemplate.ConditionalEndRewards.GetEnumerator())
											{
												while (enumerator.MoveNext())
												{
													SeasonTemplate.ConditionalSeasonEndRewards current = enumerator.Current;
													if (QuestWideData.AreConditionsMet(current.Prerequisites.Conditions, current.Prerequisites.LogicStatement))
													{
														list.Add(current);
													}
												}
											}
										}
										using (List<SeasonTemplate.SeasonEndRewards>.Enumerator enumerator2 = list.GetEnumerator())
										{
											while (enumerator2.MoveNext())
											{
												SeasonTemplate.SeasonEndRewards current2 = enumerator2.Current;
												foreach (QuestItemReward itemReward in current2.ItemRewards)
												{
													if (num < m_questRewards.Length)
													{
														InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(itemReward.ItemTemplateId);
														UIManager.SetGameObjectActive(m_questRewards[num], true);
														m_questRewards[num].SetupHack(itemTemplate, itemTemplate.IconPath, itemReward.Amount);
														num++;
													}
												}
												using (List<QuestCurrencyReward>.Enumerator enumerator4 = current2.CurrencyRewards.GetEnumerator())
												{
													while (enumerator4.MoveNext())
													{
														QuestCurrencyReward current4 = enumerator4.Current;
														if (num < m_questRewards.Length)
														{
															UIManager.SetGameObjectActive(m_questRewards[num], true);
															m_questRewards[num].Setup(current4, 0);
															num++;
														}
													}
												}
												using (List<QuestUnlockReward>.Enumerator enumerator5 = current2.UnlockRewards.GetEnumerator())
												{
													while (enumerator5.MoveNext())
													{
														QuestUnlockReward current5 = enumerator5.Current;
														if (num < m_questRewards.Length)
														{
															UIManager.SetGameObjectActive(m_questRewards[num], true);
															m_questRewards[num].SetupHack(current5.resourceString);
															num++;
														}
													}
												}
											}
										}
										for (int l = num; l < m_questRewards.Length; l++)
										{
											UIManager.SetGameObjectActive(m_questRewards[l], false);
										}
										while (true)
										{
											switch (3)
											{
											default:
												return;
											case 0:
												break;
											}
										}
									}
									}
								}
							}
							return;
						}
					}
				}
				}
			}
		}
		AnimatorStateInfo currentAnimatorStateInfo = m_EndOfSeasonAnimator.GetCurrentAnimatorStateInfo(0);
		if (!currentAnimatorStateInfo.IsName("EndOfSeasonDefaultIN"))
		{
			return;
		}
		while (true)
		{
			if (SeasonLevelUpAnimTimes[0] <= currentAnimatorStateInfo.normalizedTime)
			{
				if (currentAnimatorStateInfo.normalizedTime < SeasonLevelUpAnimTimes[1])
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
						{
							float num2 = (currentAnimatorStateInfo.normalizedTime - SeasonLevelUpAnimTimes[0]) / (SeasonLevelUpAnimTimes[1] - SeasonLevelUpAnimTimes[0]);
							m_SeasonLevelsGained.text = Mathf.FloorToInt((float)m_endSeasonNotification.SeasonLevelEarnedFromEnded * num2).ToString();
							if (!m_playingSeasonPoints)
							{
								UIFrontEnd.PlayLoopingSound(FrontEndButtonSounds.SeasonTransitionSeasonPoints);
								m_playingSeasonPoints = true;
							}
							return;
						}
						}
					}
				}
			}
			if (SeasonLevelUpAnimTimes[1] <= currentAnimatorStateInfo.normalizedTime)
			{
				if (currentAnimatorStateInfo.normalizedTime < SeasonLevelUpAnimTimes[3])
				{
					m_SeasonLevelsGained.text = m_endSeasonNotification.SeasonLevelEarnedFromEnded.ToString();
					m_ReactorLevelsGained.text = m_reactorLevelAtStart.ToString();
					if (m_playingSeasonPoints)
					{
						while (true)
						{
							UIFrontEnd.StopLoopingSound(FrontEndButtonSounds.SeasonTransitionSeasonPoints);
							m_playingSeasonPoints = false;
							return;
						}
					}
					return;
				}
			}
			if (SeasonLevelUpAnimTimes[3] <= currentAnimatorStateInfo.normalizedTime)
			{
				if (currentAnimatorStateInfo.normalizedTime < SeasonLevelUpAnimTimes[4])
				{
					float num3 = (currentAnimatorStateInfo.normalizedTime - SeasonLevelUpAnimTimes[3]) / (SeasonLevelUpAnimTimes[4] - SeasonLevelUpAnimTimes[3]);
					m_SeasonLevelsGained.text = Mathf.FloorToInt((float)m_endSeasonNotification.SeasonLevelEarnedFromEnded * (1f - num3)).ToString();
					m_ReactorLevelsGained.text = Mathf.FloorToInt((float)m_endSeasonNotification.SeasonLevelEarnedFromEnded * num3 + (float)m_reactorLevelAtStart).ToString();
					if (!m_playingReactorPoints)
					{
						while (true)
						{
							UIFrontEnd.PlayLoopingSound(FrontEndButtonSounds.SeasonTransitionReactorPoints);
							m_playingReactorPoints = true;
							return;
						}
					}
					return;
				}
			}
			m_SeasonLevelsGained.text = "0";
			m_ReactorLevelsGained.text = m_endSeasonNotification.TotalSeasonLevel.ToString();
			if (m_playingReactorPoints)
			{
				while (true)
				{
					UIFrontEnd.StopLoopingSound(FrontEndButtonSounds.SeasonTransitionReactorPoints);
					m_playingReactorPoints = false;
					return;
				}
			}
			return;
		}
	}

	public bool IsCurrentlyDisplaying()
	{
		return m_active || (m_WaitingToDisplayEndOfSeasonTakeover && m_EndOfSeasonContainer.gameObject.activeSelf);
	}

	public void HandleSeasonStatusNotification(SeasonStatusNotification notification)
	{
		if (m_WaitingToDisplayEndOfSeasonTakeover && m_endSeasonNotification.SeasonEndedIndex == notification.SeasonEndedIndex)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_WaitingToDisplayEndOfSeasonTakeover = true;
		m_endSeasonNotification = notification;
		if (!SeasonWideData.Get().GetSeasonTemplate(notification.SeasonEndedIndex).IsTutorial)
		{
			return;
		}
		while (true)
		{
			UINewReward.Get().NotifyNewRewardReceived(RewardUtils.GetSeasonsUnlockedReward());
			return;
		}
	}

	private void Display(PendingTakeover takeoverToDisplay)
	{
		m_active = true;
		m_currentTakeover = takeoverToDisplay.m_takeover;
		m_currentTakeover.m_chapter = takeoverToDisplay.m_chapter;
		UIManager.SetGameObjectActive(takeoverToDisplay.m_takeover.m_container, true);
		int num;
		if (takeoverToDisplay.m_takeover == m_unlockedTakeover)
		{
			num = ((takeoverToDisplay.m_chapter == 1) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		if (flag)
		{
			takeoverToDisplay.m_takeover.m_ChapterTitleTextLabel.text = StringUtil.TR("SeasonUnlocked", "Seasons");
			UIManager.SetGameObjectActive(m_seasonsBlurb, true);
		}
		else
		{
			takeoverToDisplay.m_takeover.m_ChapterTitleTextLabel.text = string.Format(StringUtil.TR("ChapterUnlocked", "Seasons"), takeoverToDisplay.m_chapter);
			UIManager.SetGameObjectActive(m_seasonsBlurb, false);
		}
		SeasonChapter seasonChapter = SeasonWideData.Get().GetSeasonTemplate(takeoverToDisplay.m_season).Chapters[takeoverToDisplay.m_chapter - 1];
		int num2 = 0;
		if (!flag)
		{
			using (List<QuestItemReward>.Enumerator enumerator = seasonChapter.ItemRewards.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QuestItemReward current = enumerator.Current;
					if (num2 < takeoverToDisplay.m_takeover.m_questRewards.Length)
					{
						InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(current.ItemTemplateId);
						UIManager.SetGameObjectActive(takeoverToDisplay.m_takeover.m_questRewards[num2], true);
						takeoverToDisplay.m_takeover.m_questRewards[num2].SetupHack(itemTemplate, itemTemplate.IconPath, current.Amount);
						num2++;
					}
				}
			}
			using (List<QuestCurrencyReward>.Enumerator enumerator2 = seasonChapter.CurrencyRewards.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					QuestCurrencyReward current2 = enumerator2.Current;
					if (num2 < takeoverToDisplay.m_takeover.m_questRewards.Length)
					{
						UIManager.SetGameObjectActive(takeoverToDisplay.m_takeover.m_questRewards[num2], true);
						takeoverToDisplay.m_takeover.m_questRewards[num2].Setup(current2, 0);
						num2++;
					}
				}
			}
			using (List<QuestUnlockReward>.Enumerator enumerator3 = seasonChapter.UnlockRewards.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					QuestUnlockReward current3 = enumerator3.Current;
					if (num2 < takeoverToDisplay.m_takeover.m_questRewards.Length)
					{
						UIManager.SetGameObjectActive(takeoverToDisplay.m_takeover.m_questRewards[num2], true);
						takeoverToDisplay.m_takeover.m_questRewards[num2].SetupHack(current3.resourceString);
						num2++;
					}
				}
			}
		}
		for (int i = num2; i < takeoverToDisplay.m_takeover.m_questRewards.Length; i++)
		{
			UIManager.SetGameObjectActive(takeoverToDisplay.m_takeover.m_questRewards[i], false);
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void Close()
	{
		m_active = false;
		m_currentTakeover.m_animController.Play("ChapterCompletedDefaultOUT");
	}

	private void Update()
	{
		if (!m_WaitingToDisplayEndOfSeasonTakeover && m_pendingTakeovers.Count == 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (m_active)
		{
			return;
		}
		while (true)
		{
			if (UIFrontEnd.Get() == null)
			{
				return;
			}
			while (true)
			{
				if (UISeasonsPanel.Get() == null || UINewReward.Get().IsActive() || UIFrontEnd.Get().m_playerPanel.IsUpdatingExperience())
				{
					return;
				}
				while (true)
				{
					if (UIFrontendLoadingScreen.Get().IsVisible())
					{
						return;
					}
					while (true)
					{
						if (QuestOfferPanel.Get().IsActive())
						{
							return;
						}
						while (true)
						{
							if (UIFactionsIntroduction.Get().IsActive())
							{
								return;
							}
							if (m_WaitingToDisplayEndOfSeasonTakeover)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										break;
									default:
										DisplayEndOfSeason();
										return;
									}
								}
							}
							Display(m_pendingTakeovers.Dequeue());
							return;
						}
					}
				}
			}
		}
	}

	private void ChapterUnlocked(int season, int chapter)
	{
		m_pendingTakeovers.Enqueue(new PendingTakeover(m_unlockedTakeover, season, chapter));
	}

	private void ChapterCompleted(int season, int chapter)
	{
		m_pendingTakeovers.Enqueue(new PendingTakeover(m_completedTakeover, season, chapter));
	}
}
