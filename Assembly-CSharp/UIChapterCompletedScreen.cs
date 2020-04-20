using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIChapterCompletedScreen : MonoBehaviour
{
	public UIChapterCompletedScreen.Takeover m_completedTakeover;

	public UIChapterCompletedScreen.Takeover m_unlockedTakeover;

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

	private UIChapterCompletedScreen.Takeover m_currentTakeover;

	private Queue<UIChapterCompletedScreen.PendingTakeover> m_pendingTakeovers;

	private bool m_active;

	private bool m_WaitingToDisplayEndOfSeasonTakeover;

	private SeasonStatusNotification m_endSeasonNotification;

	private float[] SeasonLevelUpAnimTimes = new float[7];

	private bool m_playingSeasonPoints;

	private bool m_playingReactorPoints;

	private int m_reactorLevelAtStart;

	private void Awake()
	{
		this.m_pendingTakeovers = new Queue<UIChapterCompletedScreen.PendingTakeover>();
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnChapterUnlockNotification += this.ChapterUnlocked;
			ClientGameManager.Get().OnChapterCompleteNotification += this.ChapterCompleted;
			ClientGameManager.Get().OnSeasonCompleteNotification += this.HandleSeasonStatusNotification;
		}
		this.m_completedTakeover.m_nextChapter.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnNextChapterClicked);
		this.m_completedTakeover.m_okBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OkButtonClicked);
		this.m_unlockedTakeover.m_nextChapter.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnNextChapterClicked);
		this.m_unlockedTakeover.m_okBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnNextChapterClicked);
		this.m_completedTakeover.m_nextChapter.spriteController.m_ignoreDialogboxes = true;
		this.m_completedTakeover.m_okBtn.spriteController.m_ignoreDialogboxes = true;
		this.m_unlockedTakeover.m_nextChapter.spriteController.m_ignoreDialogboxes = true;
		this.m_unlockedTakeover.m_okBtn.spriteController.m_ignoreDialogboxes = true;
		this.m_okBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.EndOfSeasonOkClicked);
		this.m_okBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.DialogBoxButton;
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnChapterUnlockNotification -= this.ChapterUnlocked;
			ClientGameManager.Get().OnChapterCompleteNotification -= this.ChapterCompleted;
			ClientGameManager.Get().OnSeasonCompleteNotification -= this.HandleSeasonStatusNotification;
		}
	}

	private void OnNextChapterClicked(BaseEventData data)
	{
		ClientGameManager.Get().SendSeasonStatusConfirm(SeasonStatusConfirmed.DialogType.symbol_000E);
		if (UIFrontEnd.Get() != null)
		{
			if (UIFrontEnd.Get().m_frontEndNavPanel != null)
			{
				UIFrontEnd.Get().m_frontEndNavPanel.SeasonsBtnClicked(null);
			}
		}
		if (UISeasonsPanel.Get() != null)
		{
			UISeasonsPanel.Get().SelectChapter(this.m_currentTakeover.m_chapter);
		}
		this.Close();
	}

	private void OkButtonClicked(BaseEventData data)
	{
		this.Close();
	}

	private void EndOfSeasonOkClicked(BaseEventData data)
	{
		UIAnimationEventManager.Get().PlayAnimation(this.m_EndOfSeasonAnimator, "EndOfSeasonDefaultOUT", delegate
		{
			this.m_WaitingToDisplayEndOfSeasonTakeover = false;
			UIManager.SetGameObjectActive(this.m_EndOfSeasonContainer, false, null);
			this.m_endSeasonNotification = null;
			ClientGameManager.Get().SendSeasonStatusConfirm(SeasonStatusConfirmed.DialogType.symbol_001D);
		}, string.Empty, 0, 0f, true, false, null, null);
	}

	private void DisplayEndOfSeason()
	{
		if (!this.m_EndOfSeasonContainer.gameObject.activeSelf)
		{
			SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(this.m_endSeasonNotification.SeasonEndedIndex);
			if (seasonTemplate != null)
			{
				if (seasonTemplate.IsTutorial)
				{
					this.m_WaitingToDisplayEndOfSeasonTakeover = false;
					this.m_endSeasonNotification = null;
					UINewUserFlowManager.MarkSeasonsNew(true);
					if (UIFrontEnd.Get().m_frontEndNavPanel != null)
					{
						ClientGameManager.Get().PreventNextAccountStatusCheck();
						UIFrontEnd.Get().m_frontEndNavPanel.LandingPageBtnClicked(null);
					}
					UINewUserFlowManager.OnTutorialSeasonEnded();
					this.m_pendingTakeovers.Clear();
					return;
				}
			}
			this.m_reactorLevelAtStart = this.m_endSeasonNotification.TotalSeasonLevel - this.m_endSeasonNotification.SeasonLevelEarnedFromEnded;
			UIManager.SetGameObjectActive(this.m_EndOfSeasonContainer, true, null);
			UIFrontEnd.PlaySound(FrontEndButtonSounds.SeasonTransitionIntro);
			this.m_SeasonEndTitle.text = string.Empty;
			this.m_SeasonLevelsTitle.text = string.Format(StringUtil.TR("SeasonLevel", "Seasons"), this.m_endSeasonNotification.SeasonEndedIndex);
			this.m_SeasonLevelsGained.text = "0";
			this.m_ReactorLevelsGained.text = this.m_reactorLevelAtStart.ToString();
			AnimatorClipInfo[] currentAnimatorClipInfo = this.m_EndOfSeasonAnimator.GetCurrentAnimatorClipInfo(0);
			for (int i = 0; i < currentAnimatorClipInfo.Length; i++)
			{
				if (currentAnimatorClipInfo[i].clip.name == "EndOfSeasonDefaultIN")
				{
					if (currentAnimatorClipInfo[i].clip.events.Length == 7)
					{
						for (int j = 0; j < currentAnimatorClipInfo[i].clip.events.Length; j++)
						{
							this.SeasonLevelUpAnimTimes[j] = currentAnimatorClipInfo[i].clip.events[j].time / currentAnimatorClipInfo[i].clip.length;
						}
					}
				}
			}
			this.m_playingSeasonPoints = false;
			this.m_playingReactorPoints = false;
			if (seasonTemplate != null)
			{
				int num = 0;
				this.m_SeasonEndTitle.text = seasonTemplate.GetSeasonEndHeader();
				for (int k = 0; k < this.m_levelContainers.Length; k++)
				{
					UIManager.SetGameObjectActive(this.m_levelContainers[k], !seasonTemplate.IsTutorial, null);
				}
				List<SeasonTemplate.SeasonEndRewards> list = new List<SeasonTemplate.SeasonEndRewards>();
				if (this.m_endSeasonNotification.SeasonLevelEarnedFromEnded >= 2)
				{
					list.Add(seasonTemplate.EndRewards);
					using (List<SeasonTemplate.ConditionalSeasonEndRewards>.Enumerator enumerator = seasonTemplate.ConditionalEndRewards.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							SeasonTemplate.ConditionalSeasonEndRewards conditionalSeasonEndRewards = enumerator.Current;
							if (QuestWideData.AreConditionsMet(conditionalSeasonEndRewards.Prerequisites.Conditions, conditionalSeasonEndRewards.Prerequisites.LogicStatement, false))
							{
								list.Add(conditionalSeasonEndRewards);
							}
						}
					}
				}
				using (List<SeasonTemplate.SeasonEndRewards>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						SeasonTemplate.SeasonEndRewards seasonEndRewards = enumerator2.Current;
						foreach (QuestItemReward questItemReward in seasonEndRewards.ItemRewards)
						{
							if (num < this.m_questRewards.Length)
							{
								InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(questItemReward.ItemTemplateId);
								UIManager.SetGameObjectActive(this.m_questRewards[num], true, null);
								this.m_questRewards[num].SetupHack(itemTemplate, itemTemplate.IconPath, questItemReward.Amount);
								num++;
							}
						}
						using (List<QuestCurrencyReward>.Enumerator enumerator4 = seasonEndRewards.CurrencyRewards.GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								QuestCurrencyReward currencyReward = enumerator4.Current;
								if (num < this.m_questRewards.Length)
								{
									UIManager.SetGameObjectActive(this.m_questRewards[num], true, null);
									this.m_questRewards[num].Setup(currencyReward, 0);
									num++;
								}
							}
						}
						using (List<QuestUnlockReward>.Enumerator enumerator5 = seasonEndRewards.UnlockRewards.GetEnumerator())
						{
							while (enumerator5.MoveNext())
							{
								QuestUnlockReward questUnlockReward = enumerator5.Current;
								if (num < this.m_questRewards.Length)
								{
									UIManager.SetGameObjectActive(this.m_questRewards[num], true, null);
									this.m_questRewards[num].SetupHack(questUnlockReward.resourceString, 0);
									num++;
								}
							}
						}
					}
				}
				for (int l = num; l < this.m_questRewards.Length; l++)
				{
					UIManager.SetGameObjectActive(this.m_questRewards[l], false, null);
				}
			}
		}
		else
		{
			AnimatorStateInfo currentAnimatorStateInfo = this.m_EndOfSeasonAnimator.GetCurrentAnimatorStateInfo(0);
			if (currentAnimatorStateInfo.IsName("EndOfSeasonDefaultIN"))
			{
				if (this.SeasonLevelUpAnimTimes[0] <= currentAnimatorStateInfo.normalizedTime)
				{
					if (currentAnimatorStateInfo.normalizedTime < this.SeasonLevelUpAnimTimes[1])
					{
						float num2 = (currentAnimatorStateInfo.normalizedTime - this.SeasonLevelUpAnimTimes[0]) / (this.SeasonLevelUpAnimTimes[1] - this.SeasonLevelUpAnimTimes[0]);
						this.m_SeasonLevelsGained.text = Mathf.FloorToInt((float)this.m_endSeasonNotification.SeasonLevelEarnedFromEnded * num2).ToString();
						if (!this.m_playingSeasonPoints)
						{
							UIFrontEnd.PlayLoopingSound(FrontEndButtonSounds.SeasonTransitionSeasonPoints);
							this.m_playingSeasonPoints = true;
						}
						return;
					}
				}
				if (this.SeasonLevelUpAnimTimes[1] <= currentAnimatorStateInfo.normalizedTime)
				{
					if (currentAnimatorStateInfo.normalizedTime < this.SeasonLevelUpAnimTimes[3])
					{
						this.m_SeasonLevelsGained.text = this.m_endSeasonNotification.SeasonLevelEarnedFromEnded.ToString();
						this.m_ReactorLevelsGained.text = this.m_reactorLevelAtStart.ToString();
						if (this.m_playingSeasonPoints)
						{
							UIFrontEnd.StopLoopingSound(FrontEndButtonSounds.SeasonTransitionSeasonPoints);
							this.m_playingSeasonPoints = false;
						}
						return;
					}
				}
				if (this.SeasonLevelUpAnimTimes[3] <= currentAnimatorStateInfo.normalizedTime)
				{
					if (currentAnimatorStateInfo.normalizedTime < this.SeasonLevelUpAnimTimes[4])
					{
						float num3 = (currentAnimatorStateInfo.normalizedTime - this.SeasonLevelUpAnimTimes[3]) / (this.SeasonLevelUpAnimTimes[4] - this.SeasonLevelUpAnimTimes[3]);
						this.m_SeasonLevelsGained.text = Mathf.FloorToInt((float)this.m_endSeasonNotification.SeasonLevelEarnedFromEnded * (1f - num3)).ToString();
						this.m_ReactorLevelsGained.text = Mathf.FloorToInt((float)this.m_endSeasonNotification.SeasonLevelEarnedFromEnded * num3 + (float)this.m_reactorLevelAtStart).ToString();
						if (!this.m_playingReactorPoints)
						{
							UIFrontEnd.PlayLoopingSound(FrontEndButtonSounds.SeasonTransitionReactorPoints);
							this.m_playingReactorPoints = true;
						}
						return;
					}
				}
				this.m_SeasonLevelsGained.text = "0";
				this.m_ReactorLevelsGained.text = this.m_endSeasonNotification.TotalSeasonLevel.ToString();
				if (this.m_playingReactorPoints)
				{
					UIFrontEnd.StopLoopingSound(FrontEndButtonSounds.SeasonTransitionReactorPoints);
					this.m_playingReactorPoints = false;
				}
			}
		}
	}

	public bool IsCurrentlyDisplaying()
	{
		return this.m_active || (this.m_WaitingToDisplayEndOfSeasonTakeover && this.m_EndOfSeasonContainer.gameObject.activeSelf);
	}

	public void HandleSeasonStatusNotification(SeasonStatusNotification notification)
	{
		if (this.m_WaitingToDisplayEndOfSeasonTakeover && this.m_endSeasonNotification.SeasonEndedIndex == notification.SeasonEndedIndex)
		{
			return;
		}
		this.m_WaitingToDisplayEndOfSeasonTakeover = true;
		this.m_endSeasonNotification = notification;
		if (SeasonWideData.Get().GetSeasonTemplate(notification.SeasonEndedIndex).IsTutorial)
		{
			UINewReward.Get().NotifyNewRewardReceived(RewardUtils.GetSeasonsUnlockedReward(), CharacterType.None, -1, -1);
		}
	}

	private void Display(UIChapterCompletedScreen.PendingTakeover takeoverToDisplay)
	{
		this.m_active = true;
		this.m_currentTakeover = takeoverToDisplay.m_takeover;
		this.m_currentTakeover.m_chapter = takeoverToDisplay.m_chapter;
		UIManager.SetGameObjectActive(takeoverToDisplay.m_takeover.m_container, true, null);
		bool flag;
		if (takeoverToDisplay.m_takeover == this.m_unlockedTakeover)
		{
			flag = (takeoverToDisplay.m_chapter == 1);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		if (flag2)
		{
			takeoverToDisplay.m_takeover.m_ChapterTitleTextLabel.text = StringUtil.TR("SeasonUnlocked", "Seasons");
			UIManager.SetGameObjectActive(this.m_seasonsBlurb, true, null);
		}
		else
		{
			takeoverToDisplay.m_takeover.m_ChapterTitleTextLabel.text = string.Format(StringUtil.TR("ChapterUnlocked", "Seasons"), takeoverToDisplay.m_chapter);
			UIManager.SetGameObjectActive(this.m_seasonsBlurb, false, null);
		}
		SeasonChapter seasonChapter = SeasonWideData.Get().GetSeasonTemplate(takeoverToDisplay.m_season).Chapters[takeoverToDisplay.m_chapter - 1];
		int num = 0;
		if (!flag2)
		{
			using (List<QuestItemReward>.Enumerator enumerator = seasonChapter.ItemRewards.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QuestItemReward questItemReward = enumerator.Current;
					if (num < takeoverToDisplay.m_takeover.m_questRewards.Length)
					{
						InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(questItemReward.ItemTemplateId);
						UIManager.SetGameObjectActive(takeoverToDisplay.m_takeover.m_questRewards[num], true, null);
						takeoverToDisplay.m_takeover.m_questRewards[num].SetupHack(itemTemplate, itemTemplate.IconPath, questItemReward.Amount);
						num++;
					}
				}
			}
			using (List<QuestCurrencyReward>.Enumerator enumerator2 = seasonChapter.CurrencyRewards.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					QuestCurrencyReward currencyReward = enumerator2.Current;
					if (num < takeoverToDisplay.m_takeover.m_questRewards.Length)
					{
						UIManager.SetGameObjectActive(takeoverToDisplay.m_takeover.m_questRewards[num], true, null);
						takeoverToDisplay.m_takeover.m_questRewards[num].Setup(currencyReward, 0);
						num++;
					}
				}
			}
			using (List<QuestUnlockReward>.Enumerator enumerator3 = seasonChapter.UnlockRewards.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					QuestUnlockReward questUnlockReward = enumerator3.Current;
					if (num < takeoverToDisplay.m_takeover.m_questRewards.Length)
					{
						UIManager.SetGameObjectActive(takeoverToDisplay.m_takeover.m_questRewards[num], true, null);
						takeoverToDisplay.m_takeover.m_questRewards[num].SetupHack(questUnlockReward.resourceString, 0);
						num++;
					}
				}
			}
		}
		for (int i = num; i < takeoverToDisplay.m_takeover.m_questRewards.Length; i++)
		{
			UIManager.SetGameObjectActive(takeoverToDisplay.m_takeover.m_questRewards[i], false, null);
		}
	}

	private void Close()
	{
		this.m_active = false;
		this.m_currentTakeover.m_animController.Play("ChapterCompletedDefaultOUT");
	}

	private void Update()
	{
		if (!this.m_WaitingToDisplayEndOfSeasonTakeover && this.m_pendingTakeovers.Count == 0)
		{
			return;
		}
		if (!this.m_active)
		{
			if (!(UIFrontEnd.Get() == null))
			{
				if (!(UISeasonsPanel.Get() == null) && !UINewReward.Get().IsActive() && !UIFrontEnd.Get().m_playerPanel.IsUpdatingExperience())
				{
					if (!UIFrontendLoadingScreen.Get().IsVisible())
					{
						if (!QuestOfferPanel.Get().IsActive())
						{
							if (!UIFactionsIntroduction.Get().IsActive())
							{
								if (this.m_WaitingToDisplayEndOfSeasonTakeover)
								{
									this.DisplayEndOfSeason();
									return;
								}
								this.Display(this.m_pendingTakeovers.Dequeue());
								return;
							}
						}
					}
				}
			}
		}
	}

	private void ChapterUnlocked(int season, int chapter)
	{
		this.m_pendingTakeovers.Enqueue(new UIChapterCompletedScreen.PendingTakeover(this.m_unlockedTakeover, season, chapter));
	}

	private void ChapterCompleted(int season, int chapter)
	{
		this.m_pendingTakeovers.Enqueue(new UIChapterCompletedScreen.PendingTakeover(this.m_completedTakeover, season, chapter));
	}

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
		public UIChapterCompletedScreen.Takeover m_takeover;

		public int m_season;

		public int m_chapter;

		public PendingTakeover(UIChapterCompletedScreen.Takeover takeover, int season, int chapter)
		{
			this.m_takeover = takeover;
			this.m_season = season;
			this.m_chapter = chapter;
		}
	}
}
