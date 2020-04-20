using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestListPanel : UIScene
{
	public Animator m_animator;

	[Header("Alert")]
	public RectTransform m_alertContainer;

	public RectTransform m_emptyAlertContainer;

	public TextMeshProUGUI m_normalAlertText;

	public UIAlertQuestEntry m_alertMissionEntry;

	public TextMeshProUGUI m_alertTimeRemaining;

	public TextMeshProUGUI m_nextAlertTime;

	public TextMeshProUGUI m_emptyAlertLabel;

	[Header("Daily Quests")]
	public _SelectableBtn m_dailyTabBtn;

	public _SelectableBtn m_chapterTabBtn;

	public TextMeshProUGUI m_accountLevelReqText;

	public RectTransform m_dailyContainer;

	public RectTransform m_chapterContainer;

	public UISeasonsDailyContractEntry[] dailyQuests;

	public GameObject m_gridLayout;

	public Image m_contractBackgroundClickBlocker;

	[Header("Chapter Info")]
	public TextMeshProUGUI m_chapterHeader;

	public ScrollRect m_chapterQuestChallengeScrollList;

	public VerticalLayoutGroup m_chapterQuestListContainer;

	public UISeasonsQuestEntry m_chapterQuestListEntryPrefab;

	public ScrollRect m_dailyQuestScrollList;

	private static QuestListPanel s_instance;

	private static bool m_firstLoggedIn;

	private QuestListPanel.DisplayStates m_displayState;

	private bool m_initialized;

	private List<int> m_lastSetupQuestIds;

	private int m_expandedQuestId;

	private List<GameObject> m_clickListenerExceptions;

	private bool m_isVisible;

	private bool m_areDailesUnlocked;

	private List<UISeasonsQuestEntry> m_questEntryList;

	private SeasonLockoutReason m_seasonLockoutReason;

	private DateTime? m_timeTillNextAlert;

	public static QuestListPanel Get()
	{
		return QuestListPanel.s_instance;
	}

	public QuestListPanel.DisplayStates DisplayState
	{
		get
		{
			return this.m_displayState;
		}
	}

	public bool IsVisible()
	{
		return this.m_isVisible;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.QuestList;
	}

	public override void Awake()
	{
		QuestListPanel.s_instance = this;
		this.m_displayState = QuestListPanel.DisplayStates.None;
		UIManager.SetGameObjectActive(this.m_animator, false, null);
		this.m_lastSetupQuestIds = new List<int>();
		this.m_expandedQuestId = 0;
		if (HitchDetector.Get() != null)
		{
			HitchDetector.Get().AddNewLayoutGroup(this.m_gridLayout.GetComponent<VerticalLayoutGroup>());
		}
		if (this.m_accountLevelReqText != null)
		{
			this.m_accountLevelReqText.text = string.Format(StringUtil.TR("DailyQuestsUnlockRequirements", "Quests"), new object[0]);
			UIManager.SetGameObjectActive(this.m_accountLevelReqText, true, null);
		}
		this.m_questEntryList = new List<UISeasonsQuestEntry>();
		ClientGameManager.Get().OnAccountDataUpdated += this.HandleAccountDataUpdated;
		ClientGameManager.Get().OnLobbyServerClientAccessLevelChange += this.HandleLobbyServerClientAccessLevelChange;
		ClientGameManager.Get().OnQuestProgressChanged += this.HandleQuestProgressChanged;
		ClientGameManager.Get().OnAlertMissionDataChange += this.SetupLatestAlert;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
			this.HandleAccountDataUpdated(playerAccountData);
			this.SetupLatestAlert(ClientGameManager.Get().AlertMissionsData);
		}
		this.m_clickListenerExceptions = new List<GameObject>
		{
			this.m_contractBackgroundClickBlocker.gameObject
		};
		foreach (UISeasonsDailyContractEntry uiseasonsDailyContractEntry in this.dailyQuests)
		{
			this.m_clickListenerExceptions.Add(uiseasonsDailyContractEntry.m_btnHitBox.gameObject);
			uiseasonsDailyContractEntry.m_btnHitBox.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnDailyScroll));
			uiseasonsDailyContractEntry.SetMouseEventScroll(this.m_dailyQuestScrollList);
		}
		this.m_dailyTabBtn.SetSelected(true, false, string.Empty, string.Empty);
		this.m_chapterTabBtn.SetSelected(false, false, string.Empty, string.Empty);
		UIManager.SetGameObjectActive(this.m_dailyContainer, true, null);
		UIManager.SetGameObjectActive(this.m_chapterContainer, false, null);
		this.m_dailyTabBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.DailyTabClicked);
		this.m_chapterTabBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ChapterTabClicked);
		this.m_chapterTabBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.SetupTooltip), null);
		base.Awake();
	}

	private void DailyTabClicked(BaseEventData data)
	{
		this.m_dailyTabBtn.SetSelected(true, false, string.Empty, string.Empty);
		this.m_chapterTabBtn.SetSelected(false, false, string.Empty, string.Empty);
		UIManager.SetGameObjectActive(this.m_dailyContainer, true, null);
		UIManager.SetGameObjectActive(this.m_chapterContainer, false, null);
	}

	private void ChapterTabClicked(BaseEventData date)
	{
		this.m_alertMissionEntry.SetExpanded(false, false);
		for (int i = 0; i < this.dailyQuests.Length; i++)
		{
			this.dailyQuests[i].SetExpanded(false, false);
		}
		this.SetupSeasonChapter();
		this.m_dailyTabBtn.SetSelected(false, false, string.Empty, string.Empty);
		this.m_chapterTabBtn.SetSelected(true, false, string.Empty, string.Empty);
		UIManager.SetGameObjectActive(this.m_dailyContainer, false, null);
		UIManager.SetGameObjectActive(this.m_chapterContainer, true, null);
	}

	public void NotifyEntryExpanded(UISeasonsBaseContract entry)
	{
		if (entry != this.m_alertMissionEntry)
		{
			for (int i = 0; i < this.dailyQuests.Length; i++)
			{
				if (entry != this.dailyQuests[i])
				{
					this.dailyQuests[i].SetExpanded(false, false);
				}
			}
		}
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnAccountDataUpdated -= this.HandleAccountDataUpdated;
			ClientGameManager.Get().OnLobbyServerClientAccessLevelChange -= this.HandleLobbyServerClientAccessLevelChange;
			ClientGameManager.Get().OnQuestProgressChanged -= this.HandleQuestProgressChanged;
			ClientGameManager.Get().OnAlertMissionDataChange -= this.SetupLatestAlert;
		}
		QuestListPanel.s_instance = null;
	}

	private void UpdateQuests(List<int> currentQuestList, bool force = false)
	{
		bool flag = false;
		if (this.m_lastSetupQuestIds.Count != currentQuestList.Count)
		{
			flag = true;
		}
		else
		{
			for (int i = 0; i < this.m_lastSetupQuestIds.Count; i++)
			{
				if (this.m_lastSetupQuestIds[i] != currentQuestList[i])
				{
					flag = true;
					goto IL_6F;
				}
			}
		}
		IL_6F:
		if (!flag)
		{
			if (!force)
			{
				return;
			}
		}
		this.m_lastSetupQuestIds = currentQuestList;
		for (int j = 0; j < this.dailyQuests.Length; j++)
		{
			if (j < currentQuestList.Count)
			{
				this.dailyQuests[j].DeleteCache();
				this.dailyQuests[j].Setup(currentQuestList[j]);
				this.dailyQuests[j].SetState(QuestItemState.Filled);
			}
			else
			{
				this.dailyQuests[j].SetState(QuestItemState.Empty);
			}
		}
		this.Setup();
	}

	private void Start()
	{
		if (QuestOfferPanel.Get() != null && UIClickListener.Get() != null)
		{
			if (!this.m_isVisible)
			{
				UIClickListener.Get().Disable();
			}
			else
			{
				if (!this.m_clickListenerExceptions.Contains(QuestOfferPanel.Get().m_questListButton.spriteController.gameObject))
				{
					this.m_clickListenerExceptions.Add(QuestOfferPanel.Get().m_questListButton.spriteController.gameObject);
				}
				UIClickListener.Get().Enable(this.m_clickListenerExceptions, delegate
				{
					this.SetVisible(false, false, false);
				});
			}
		}
	}

	private void Setup()
	{
		this.m_initialized = true;
		int generalSlotCount = QuestWideData.Get().m_generalSlotCount;
		for (int i = 0; i < this.dailyQuests.Length; i++)
		{
			if (this.m_lastSetupQuestIds.Count <= i)
			{
				if (i < generalSlotCount)
				{
					StaggerComponent.SetStaggerComponent(this.dailyQuests[i].gameObject, true, true);
					this.dailyQuests[i].SetState(QuestItemState.Empty);
				}
				else
				{
					UIManager.SetGameObjectActive(this.dailyQuests[i], false, null);
					StaggerComponent.SetStaggerComponent(this.dailyQuests[i].gameObject, false, true);
				}
			}
			else if (this.m_lastSetupQuestIds[i] == this.m_expandedQuestId)
			{
				this.dailyQuests[i].SetExpanded(true, false);
				this.dailyQuests[i].SetState(QuestItemState.Expanded);
			}
			else
			{
				this.dailyQuests[i].SetExpanded(false, false);
				this.dailyQuests[i].SetState(QuestItemState.Filled);
			}
		}
	}

	public void ExpandQuestId(int questId)
	{
		if (this.m_lastSetupQuestIds.Contains(questId))
		{
			this.m_expandedQuestId = questId;
			this.Setup();
		}
	}

	public void CollapseQuestId(int questId)
	{
		if (this.m_lastSetupQuestIds.Contains(questId))
		{
			if (this.m_expandedQuestId == questId)
			{
				this.m_expandedQuestId = 0;
				this.Setup();
			}
		}
	}

	public void SetVisible(bool visible, bool replayAnim = false, bool ignoreSound = false)
	{
		if (visible)
		{
			if (!this.m_areDailesUnlocked)
			{
				return;
			}
		}
		if (!this.m_initialized)
		{
			this.Setup();
		}
		if (!visible)
		{
			if (this.m_displayState != QuestListPanel.DisplayStates.FadeIn)
			{
				if (this.m_displayState != QuestListPanel.DisplayStates.Idle)
				{
					goto IL_7C;
				}
			}
			if (!ignoreSound)
			{
				UIFrontEnd.PlaySound(FrontEndButtonSounds.MainMenuClose);
			}
			this.StartFadeOut();
			goto IL_B7;
		}
		IL_7C:
		if (visible)
		{
			if (this.m_displayState == QuestListPanel.DisplayStates.None || this.m_displayState == QuestListPanel.DisplayStates.FadeOut)
			{
				UIManager.SetGameObjectActive(this.m_animator, true, null);
				this.StartFadeIn();
				if (!ignoreSound)
				{
					UIFrontEnd.PlaySound(FrontEndButtonSounds.MainMenuOpen);
				}
			}
		}
		IL_B7:
		this.m_isVisible = visible;
		if (visible)
		{
			this.m_alertMissionEntry.SetExpanded(false, false);
			for (int i = 0; i < this.dailyQuests.Length; i++)
			{
				this.dailyQuests[i].SetExpanded(false, false);
			}
		}
		if (QuestOfferPanel.Get() != null && UIClickListener.Get() != null)
		{
			if (!visible)
			{
				UIClickListener.Get().Disable();
			}
			else
			{
				if (!this.m_clickListenerExceptions.Contains(QuestOfferPanel.Get().m_questListButton.spriteController.gameObject))
				{
					this.m_clickListenerExceptions.Add(QuestOfferPanel.Get().m_questListButton.spriteController.gameObject);
				}
				UIClickListener.Get().Enable(this.m_clickListenerExceptions, delegate
				{
					this.SetVisible(false, false, false);
				});
			}
		}
	}

	private void StartFadeIn()
	{
		this.m_displayState = QuestListPanel.DisplayStates.FadeIn;
		if (this.m_animator != null)
		{
			this.m_animator.Play("ContractListDefaultIN");
		}
	}

	private void StartIdle()
	{
		this.m_displayState = QuestListPanel.DisplayStates.Idle;
		if (this.m_animator != null)
		{
			this.m_animator.Play("ContractListDefaultIDLE");
		}
	}

	private void StartFadeOut()
	{
		this.m_displayState = QuestListPanel.DisplayStates.FadeOut;
		if (this.m_animator != null)
		{
			this.m_animator.Play("ContractListDefaultOUT");
		}
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			bool flag = true;
			if (EventSystem.current != null)
			{
				if (EventSystem.current.IsPointerOverGameObject(-1))
				{
					StandaloneInputModuleWithEventDataAccess component = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
					if (component != null)
					{
						if (component.GetLastPointerEventDataPublic(-1).pointerEnter != null)
						{
							QuestListPanel componentInParent = component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<QuestListPanel>();
							bool flag2 = false;
							if (componentInParent == null)
							{
								_SelectableBtn componentInParent2 = component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<_SelectableBtn>();
								if (UIFrontEnd.Get() != null)
								{
									while (componentInParent2 != null)
									{
										_SelectableBtn notificationsBtn = UIFrontEnd.Get().m_frontEndNavPanel.m_notificationsBtn;
										_SelectableBtn questListButton = QuestOfferPanel.Get().m_questListButton;
										if (!(componentInParent2 == notificationsBtn))
										{
											if (!(componentInParent2 == questListButton))
											{
												componentInParent2 = componentInParent2.transform.parent.GetComponentInParent<_SelectableBtn>();
												continue;
											}
										}
										flag2 = true;
										goto IL_190;
									}
								}
							}
							IL_190:
							if (!(componentInParent != null))
							{
								if (!flag2)
								{
									goto IL_1A8;
								}
							}
							flag = false;
						}
					}
				}
			}
			IL_1A8:
			if (flag)
			{
				if (this.m_isVisible)
				{
					if (UIFrontEnd.Get() != null)
					{
						UIFrontEnd.Get().m_frontEndNavPanel.NotificationBtnClicked(null);
					}
				}
			}
		}
		if (this.m_displayState == QuestListPanel.DisplayStates.FadeIn)
		{
			if (this.IsAnimationDone(this.m_animator, "ContractListDefaultIN"))
			{
				this.StartIdle();
			}
		}
		if (this.m_displayState == QuestListPanel.DisplayStates.FadeOut)
		{
			if (this.IsAnimationDone(this.m_animator, "ContractListDefaultOUT"))
			{
				UIManager.SetGameObjectActive(this.m_animator, false, null);
				this.m_displayState = QuestListPanel.DisplayStates.None;
			}
		}
		int num = 0;
		bool flag3 = false;
		for (int i = 0; i < this.dailyQuests.Length; i++)
		{
			if (this.dailyQuests[i].IsExpanded())
			{
				num++;
			}
			if (this.dailyQuests[i].IsAnimating())
			{
				flag3 = true;
			}
		}
		if (!flag3)
		{
			if (num == 0)
			{
				if ((this.m_gridLayout.transform as RectTransform).anchoredPosition.y != 0f)
				{
					(this.m_gridLayout.transform as RectTransform).anchoredPosition = new Vector2((this.m_gridLayout.transform as RectTransform).anchoredPosition.x, 0f);
				}
			}
		}
		if (this.m_timeTillNextAlert != null)
		{
			TimeSpan timeSpan = this.m_timeTillNextAlert.Value.Subtract(ClientGameManager.Get().PacificNow());
			string timeDifferenceText = StringUtil.GetTimeDifferenceText(timeSpan, true);
			if (timeSpan > TimeSpan.Zero && !timeDifferenceText.IsNullOrEmpty())
			{
				this.m_nextAlertTime.text = string.Format(StringUtil.TR("TimeUntilNextAlert", "Global"), timeDifferenceText);
			}
			else
			{
				this.m_nextAlertTime.text = StringUtil.TR("AlertIncoming", "Global");
			}
			this.m_alertTimeRemaining.text = string.Empty;
			UIManager.SetGameObjectActive(this.m_emptyAlertLabel, false, null);
		}
		else if (ClientGameManager.Get().AlertMissionsData.CurrentAlert != null)
		{
			ActiveAlertMission currentAlert = ClientGameManager.Get().AlertMissionsData.CurrentAlert;
			TimeSpan timeSpan2 = currentAlert.StartTimePST.AddHours((double)currentAlert.DurationHours).Subtract(ClientGameManager.Get().PacificNow());
			string timeDifferenceText2 = StringUtil.GetTimeDifferenceText(timeSpan2, true);
			if (timeSpan2 > TimeSpan.Zero)
			{
				if (!timeDifferenceText2.IsNullOrEmpty())
				{
					this.m_alertTimeRemaining.text = string.Format(StringUtil.TR("QuestRemainingTime", "Global"), timeDifferenceText2);
					goto IL_530;
				}
			}
			if (!this.m_alertTimeRemaining.text.IsNullOrEmpty())
			{
				this.m_alertTimeRemaining.text = string.Empty;
				UIManager.SetGameObjectActive(this.m_emptyAlertContainer, true, null);
				UIManager.SetGameObjectActive(this.m_normalAlertText, false, null);
				this.m_alertMissionEntry.Setup(null);
			}
			IL_530:
			this.m_nextAlertTime.text = string.Empty;
			UIManager.SetGameObjectActive(this.m_emptyAlertLabel, true, null);
		}
	}

	private bool IsAnimationDone(Animator animator, string animName)
	{
		if (!animator.isInitialized)
		{
			return false;
		}
		AnimatorClipInfo[] currentAnimatorClipInfo = animator.GetCurrentAnimatorClipInfo(0);
		if (currentAnimatorClipInfo.Length == 0)
		{
			return false;
		}
		AnimatorClipInfo animatorClipInfo = currentAnimatorClipInfo[0];
		AnimationClip clip = animatorClipInfo.clip;
		if (clip == null)
		{
			return false;
		}
		if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
		{
			return false;
		}
		if (clip.name != animName)
		{
			return false;
		}
		return true;
	}

	public void HandleQuestAdded(int questId)
	{
		if (this.m_lastSetupQuestIds.Contains(questId))
		{
			return;
		}
		this.m_lastSetupQuestIds.Add(questId);
		this.UpdateQuests(this.m_lastSetupQuestIds, true);
	}

	private void HandleAccountDataUpdated(PersistedAccountData accountData)
	{
		List<int> list = new List<int>();
		int num = -1;
		this.CheckSeasonsVisibility();
		foreach (KeyValuePair<int, QuestProgress> keyValuePair in accountData.QuestComponent.Progress)
		{
			int key = keyValuePair.Key;
			if (QuestWideData.Get().IsDailyQuest(key))
			{
				list.Add(key);
			}
			if (ClientGameManager.Get().IsCurrentAlertQuest(key))
			{
				num = key;
			}
		}
		this.UpdateQuests(list, false);
		if (num > -1)
		{
			this.SetupLatestAlert(ClientGameManager.Get().AlertMissionsData);
		}
		this.SetupSeasonChapter();
		if (!QuestListPanel.m_firstLoggedIn)
		{
			QuestListPanel.m_firstLoggedIn = true;
			int num2 = 0;
			using (Dictionary<int, QuestProgress>.Enumerator enumerator2 = accountData.QuestComponent.Progress.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<int, QuestProgress> keyValuePair2 = enumerator2.Current;
					int key2 = keyValuePair2.Key;
					if (QuestWideData.Get().IsDailyQuest(key2))
					{
						num2++;
					}
					if (ClientGameManager.Get().AlertMissionsData != null)
					{
						if (ClientGameManager.Get().AlertMissionsData.CurrentAlert != null)
						{
							if (ClientGameManager.Get().AlertMissionsData.CurrentAlert.QuestId == key2)
							{
								num2++;
							}
						}
					}
				}
			}
			if (num2 > 0)
			{
				UIFrontEnd.s_firstLogInQuestCount = num2;
			}
		}
		this.m_areDailesUnlocked = accountData.AccountComponent.DailyQuestsAvailable;
		if (this.m_accountLevelReqText != null)
		{
			UIManager.SetGameObjectActive(this.m_accountLevelReqText, !this.m_areDailesUnlocked, null);
		}
	}

	private void HandleLobbyServerClientAccessLevelChange(ClientAccessLevel oldLevel, ClientAccessLevel newLevel)
	{
		this.UpdateQuests(this.m_lastSetupQuestIds, true);
		if (ClientGameManager.Get().AlertMissionsData != null)
		{
			this.SetupLatestAlert(ClientGameManager.Get().AlertMissionsData);
		}
		this.SetupSeasonChapter();
	}

	private void HandleQuestProgressChanged(QuestProgress[] progress)
	{
		int i = 0;
		IL_3D:
		while (i < this.dailyQuests.Length)
		{
			for (int j = 0; j < progress.Length; j++)
			{
				if (this.dailyQuests[i].UpdateProgress(progress[j]))
				{
					IL_39:
					i++;
					goto IL_3D;
				}
			}
			goto IL_39;
		}
	}

	public void CheckSeasonsVisibility()
	{
		bool flag = UISeasonsPanel.CheckSeasonsVisibility(out this.m_seasonLockoutReason);
		this.m_chapterTabBtn.SetDisabled(!flag);
		this.m_chapterTabBtn.spriteController.SetForceHovercallback(!flag);
		this.m_chapterTabBtn.spriteController.SetForceExitCallback(!flag);
	}

	private bool SetupTooltip(UITooltipBase tooltip)
	{
		if (this.m_seasonLockoutReason == SeasonLockoutReason.InTutorialSeason)
		{
			if (ClientGameManager.Get() != null && ClientGameManager.Get().GetPlayerAccountData() != null)
			{
				int activeSeason = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
				SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(activeSeason);
				UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
				uititledTooltip.Setup(StringUtil.TR("Locked", "Global"), string.Format(StringUtil.TR("RequiresMatchesPlayed", "Global"), QuestWideData.GetEndLevel(seasonTemplate.Prerequisites, activeSeason)), string.Empty);
				return true;
			}
		}
		else if (this.m_seasonLockoutReason == SeasonLockoutReason.Disabled)
		{
			UITitledTooltip uititledTooltip2 = tooltip as UITitledTooltip;
			uititledTooltip2.Setup(StringUtil.TR("Locked", "Global"), StringUtil.TR("SeasonsDisabled", "Global"), string.Empty);
			return true;
		}
		return false;
	}

	private void SetupLatestAlert(LobbyAlertMissionDataNotification notification)
	{
		this.m_alertTimeRemaining.text = string.Empty;
		this.m_nextAlertTime.text = string.Empty;
		UIManager.SetGameObjectActive(this.m_emptyAlertLabel, true, null);
		UIManager.SetGameObjectActive(this.m_alertContainer, notification.AlertMissionsEnabled, null);
		if (!notification.AlertMissionsEnabled)
		{
			this.m_timeTillNextAlert = null;
			return;
		}
		this.m_timeTillNextAlert = notification.NextAlert;
		if (notification.CurrentAlert == null)
		{
			UIManager.SetGameObjectActive(this.m_emptyAlertContainer, true, null);
			UIManager.SetGameObjectActive(this.m_normalAlertText, false, null);
			this.m_alertMissionEntry.Setup(null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_emptyAlertContainer, false, null);
			if (notification.CurrentAlert.Type == AlertMissionType.Quest)
			{
				UIManager.SetGameObjectActive(this.m_normalAlertText, false, null);
				this.m_alertMissionEntry.Setup(notification.CurrentAlert);
			}
			else
			{
				if (notification.CurrentAlert.Type != AlertMissionType.Bonus)
				{
					throw new NotImplementedException();
				}
				this.m_alertMissionEntry.Setup(null);
				UIManager.SetGameObjectActive(this.m_normalAlertText, true, null);
				this.m_normalAlertText.text = string.Format(StringUtil.TR("BonusAlertDescription" + notification.CurrentAlert.BonusType, "Global"), notification.CurrentAlert.BonusMultiplier);
			}
		}
	}

	private void SetupSeasonChapter()
	{
		PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
		int num = 0;
		using (List<SeasonTemplate>.Enumerator enumerator = SeasonWideData.Get().m_seasons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SeasonTemplate seasonTemplate = enumerator.Current;
				if (seasonTemplate.Index != playerAccountData.QuestComponent.ActiveSeason)
				{
					if (playerAccountData.QuestComponent.ActiveSeason != 0)
					{
						continue;
					}
				}
				num = seasonTemplate.Index;
				goto IL_9E;
			}
		}
		IL_9E:
		int num2 = 0;
		using (List<int>.Enumerator enumerator2 = playerAccountData.QuestComponent.GetUnlockedSeasonChapters(num).GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				int num3 = enumerator2.Current;
				if (num3 > num2)
				{
					num2 = num3;
				}
			}
		}
		UIPlayerSeasonDisplayInfo uiplayerSeasonDisplayInfo = new UIPlayerSeasonDisplayInfo();
		uiplayerSeasonDisplayInfo.Setup(num, playerAccountData);
		UISeasonChapterEntry uiseasonChapterEntry = uiplayerSeasonDisplayInfo.ChapterEntries[num2];
		int i = 0;
		int num4 = 0;
		bool flag = false;
		for (int j = 0; j < uiseasonChapterEntry.QuestInfo.Count; j++)
		{
			if (j >= this.m_questEntryList.Count)
			{
				flag = true;
				UISeasonsQuestEntry uiseasonsQuestEntry = UnityEngine.Object.Instantiate<UISeasonsQuestEntry>(this.m_chapterQuestListEntryPrefab);
				uiseasonsQuestEntry.transform.SetParent(this.m_chapterQuestListContainer.transform);
				uiseasonsQuestEntry.transform.localScale = Vector3.one;
				uiseasonsQuestEntry.transform.localPosition = Vector3.zero;
				this.m_questEntryList.Add(uiseasonsQuestEntry);
				uiseasonsQuestEntry.m_btnHitBox.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnChapterScroll));
			}
			StaggerComponent.SetStaggerComponent(this.m_questEntryList[j].gameObject, true, true);
			this.m_questEntryList[j].Setup(uiseasonChapterEntry.QuestInfo[j], false);
			this.m_questEntryList[j].SetExpanded(false, false);
			this.m_questEntryList[j].SetMouseEventScroll(this.m_chapterQuestChallengeScrollList);
			i = j;
			if (uiseasonChapterEntry.QuestInfo[j].Completed)
			{
				num4++;
			}
		}
		if (flag)
		{
			if (HitchDetector.Get() != null)
			{
				HitchDetector.Get().AddNewLayoutGroup(this.m_chapterQuestListContainer);
			}
		}
		for (i++; i < this.m_questEntryList.Count; i++)
		{
			StaggerComponent.SetStaggerComponent(this.m_questEntryList[i].gameObject, false, true);
		}
		int num5 = uiplayerSeasonDisplayInfo.SeasonNumber;
		if (SeasonWideData.Get() != null)
		{
			num5 = SeasonWideData.Get().GetPlayerFacingSeasonNumber(uiplayerSeasonDisplayInfo.SeasonNumber);
		}
		this.m_chapterHeader.text = string.Concat(new object[]
		{
			string.Format(StringUtil.TR("SeasonNumber", "Global"), num5),
			": ",
			string.Format(StringUtil.TR("ChapterNumber", "Global"), num2 + 1),
			"  (",
			num4,
			"/",
			uiseasonChapterEntry.QuestInfo.Count,
			")"
		});
	}

	private void OnChapterScroll(BaseEventData data)
	{
		this.m_chapterQuestChallengeScrollList.OnScroll((PointerEventData)data);
	}

	private void OnDailyScroll(BaseEventData data)
	{
		this.m_dailyQuestScrollList.OnScroll((PointerEventData)data);
	}

	public enum DisplayStates
	{
		None,
		FadeIn,
		Idle,
		FadeOut
	}
}
