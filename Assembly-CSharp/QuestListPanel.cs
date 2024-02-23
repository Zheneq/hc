using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestListPanel : UIScene
{
	public enum DisplayStates
	{
		None,
		FadeIn,
		Idle,
		FadeOut
	}

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

	private DisplayStates m_displayState;

	private bool m_initialized;

	private List<int> m_lastSetupQuestIds;

	private int m_expandedQuestId;

	private List<GameObject> m_clickListenerExceptions;

	private bool m_isVisible;

	private bool m_areDailesUnlocked;

	private List<UISeasonsQuestEntry> m_questEntryList;

	private SeasonLockoutReason m_seasonLockoutReason;

	private DateTime? m_timeTillNextAlert;

	public DisplayStates DisplayState
	{
		get { return m_displayState; }
	}

	public static QuestListPanel Get()
	{
		return s_instance;
	}

	public bool IsVisible()
	{
		return m_isVisible;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.QuestList;
	}

	public override void Awake()
	{
		s_instance = this;
		m_displayState = DisplayStates.None;
		UIManager.SetGameObjectActive(m_animator, false);
		m_lastSetupQuestIds = new List<int>();
		m_expandedQuestId = 0;
		if (HitchDetector.Get() != null)
		{
			HitchDetector.Get().AddNewLayoutGroup(m_gridLayout.GetComponent<VerticalLayoutGroup>());
		}
		if (m_accountLevelReqText != null)
		{
			m_accountLevelReqText.text = string.Format(StringUtil.TR("DailyQuestsUnlockRequirements", "Quests"));
			UIManager.SetGameObjectActive(m_accountLevelReqText, true);
		}
		m_questEntryList = new List<UISeasonsQuestEntry>();
		ClientGameManager.Get().OnAccountDataUpdated += HandleAccountDataUpdated;
		ClientGameManager.Get().OnLobbyServerClientAccessLevelChange += HandleLobbyServerClientAccessLevelChange;
		ClientGameManager.Get().OnQuestProgressChanged += HandleQuestProgressChanged;
		ClientGameManager.Get().OnAlertMissionDataChange += SetupLatestAlert;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
			HandleAccountDataUpdated(playerAccountData);
			SetupLatestAlert(ClientGameManager.Get().AlertMissionsData);
		}
		m_clickListenerExceptions = new List<GameObject>
		{
			m_contractBackgroundClickBlocker.gameObject
		};
		UISeasonsDailyContractEntry[] array = dailyQuests;
		foreach (UISeasonsDailyContractEntry uISeasonsDailyContractEntry in array)
		{
			m_clickListenerExceptions.Add(uISeasonsDailyContractEntry.m_btnHitBox.gameObject);
			uISeasonsDailyContractEntry.m_btnHitBox.spriteController.RegisterScrollListener(OnDailyScroll);
			uISeasonsDailyContractEntry.SetMouseEventScroll(m_dailyQuestScrollList);
		}
		m_dailyTabBtn.SetSelected(true, false, string.Empty, string.Empty);
		m_chapterTabBtn.SetSelected(false, false, string.Empty, string.Empty);
		UIManager.SetGameObjectActive(m_dailyContainer, true);
		UIManager.SetGameObjectActive(m_chapterContainer, false);
		m_dailyTabBtn.spriteController.callback = DailyTabClicked;
		m_chapterTabBtn.spriteController.callback = ChapterTabClicked;
		m_chapterTabBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, SetupTooltip);
		base.Awake();
	}

	private void DailyTabClicked(BaseEventData data)
	{
		m_dailyTabBtn.SetSelected(true, false, string.Empty, string.Empty);
		m_chapterTabBtn.SetSelected(false, false, string.Empty, string.Empty);
		UIManager.SetGameObjectActive(m_dailyContainer, true);
		UIManager.SetGameObjectActive(m_chapterContainer, false);
	}

	private void ChapterTabClicked(BaseEventData date)
	{
		m_alertMissionEntry.SetExpanded(false);
		for (int i = 0; i < dailyQuests.Length; i++)
		{
			dailyQuests[i].SetExpanded(false);
		}
		while (true)
		{
			SetupSeasonChapter();
			m_dailyTabBtn.SetSelected(false, false, string.Empty, string.Empty);
			m_chapterTabBtn.SetSelected(true, false, string.Empty, string.Empty);
			UIManager.SetGameObjectActive(m_dailyContainer, false);
			UIManager.SetGameObjectActive(m_chapterContainer, true);
			return;
		}
	}

	public void NotifyEntryExpanded(UISeasonsBaseContract entry)
	{
		if (!(entry != m_alertMissionEntry))
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < dailyQuests.Length; i++)
			{
				if (entry != dailyQuests[i])
				{
					dailyQuests[i].SetExpanded(false);
				}
			}
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnAccountDataUpdated -= HandleAccountDataUpdated;
			ClientGameManager.Get().OnLobbyServerClientAccessLevelChange -= HandleLobbyServerClientAccessLevelChange;
			ClientGameManager.Get().OnQuestProgressChanged -= HandleQuestProgressChanged;
			ClientGameManager.Get().OnAlertMissionDataChange -= SetupLatestAlert;
		}
		s_instance = null;
	}

	private void UpdateQuests(List<int> currentQuestList, bool force = false)
	{
		bool flag = false;
		if (m_lastSetupQuestIds.Count != currentQuestList.Count)
		{
			flag = true;
		}
		else
		{
			int num = 0;
			while (true)
			{
				if (num < m_lastSetupQuestIds.Count)
				{
					if (m_lastSetupQuestIds[num] != currentQuestList[num])
					{
						flag = true;
						break;
					}
					num++;
					continue;
				}
				break;
			}
		}
		if (!flag)
		{
			if (!force)
			{
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
		m_lastSetupQuestIds = currentQuestList;
		for (int i = 0; i < dailyQuests.Length; i++)
		{
			if (i < currentQuestList.Count)
			{
				dailyQuests[i].DeleteCache();
				dailyQuests[i].Setup(currentQuestList[i]);
				dailyQuests[i].SetState(QuestItemState.Filled);
			}
			else
			{
				dailyQuests[i].SetState(QuestItemState.Empty);
			}
		}
		while (true)
		{
			Setup();
			return;
		}
	}

	private void Start()
	{
		if (!(QuestOfferPanel.Get() != null) || !(UIClickListener.Get() != null))
		{
			return;
		}
		if (!m_isVisible)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					UIClickListener.Get().Disable();
					return;
				}
			}
		}
		if (!m_clickListenerExceptions.Contains(QuestOfferPanel.Get().m_questListButton.spriteController.gameObject))
		{
			m_clickListenerExceptions.Add(QuestOfferPanel.Get().m_questListButton.spriteController.gameObject);
		}
		UIClickListener.Get().Enable(m_clickListenerExceptions, delegate
		{
			SetVisible(false);
		});
	}

	private void Setup()
	{
		m_initialized = true;
		int generalSlotCount = QuestWideData.Get().m_generalSlotCount;
		for (int i = 0; i < dailyQuests.Length; i++)
		{
			if (m_lastSetupQuestIds.Count <= i)
			{
				if (i < generalSlotCount)
				{
					StaggerComponent.SetStaggerComponent(dailyQuests[i].gameObject, true);
					dailyQuests[i].SetState(QuestItemState.Empty);
				}
				else
				{
					UIManager.SetGameObjectActive(dailyQuests[i], false);
					StaggerComponent.SetStaggerComponent(dailyQuests[i].gameObject, false);
				}
			}
			else if (m_lastSetupQuestIds[i] == m_expandedQuestId)
			{
				dailyQuests[i].SetExpanded(true);
				dailyQuests[i].SetState(QuestItemState.Expanded);
			}
			else
			{
				dailyQuests[i].SetExpanded(false);
				dailyQuests[i].SetState(QuestItemState.Filled);
			}
		}
		while (true)
		{
			switch (2)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void ExpandQuestId(int questId)
	{
		if (!m_lastSetupQuestIds.Contains(questId))
		{
			return;
		}
		while (true)
		{
			m_expandedQuestId = questId;
			Setup();
			return;
		}
	}

	public void CollapseQuestId(int questId)
	{
		if (!m_lastSetupQuestIds.Contains(questId))
		{
			return;
		}
		while (true)
		{
			if (m_expandedQuestId == questId)
			{
				while (true)
				{
					m_expandedQuestId = 0;
					Setup();
					return;
				}
			}
			return;
		}
	}

	public void SetVisible(bool visible, bool replayAnim = false, bool ignoreSound = false)
	{
		if (visible)
		{
			if (!m_areDailesUnlocked)
			{
				return;
			}
		}
		if (!m_initialized)
		{
			Setup();
		}
		if (visible)
		{
			goto IL_007c;
		}
		if (m_displayState != DisplayStates.FadeIn)
		{
			if (m_displayState != DisplayStates.Idle)
			{
				goto IL_007c;
			}
		}
		if (!ignoreSound)
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.MainMenuClose);
		}
		StartFadeOut();
		goto IL_00b7;
		IL_007c:
		if (visible)
		{
			if (m_displayState == DisplayStates.None || m_displayState == DisplayStates.FadeOut)
			{
				UIManager.SetGameObjectActive(m_animator, true);
				StartFadeIn();
				if (!ignoreSound)
				{
					UIFrontEnd.PlaySound(FrontEndButtonSounds.MainMenuOpen);
				}
			}
		}
		goto IL_00b7;
		IL_00b7:
		m_isVisible = visible;
		if (visible)
		{
			m_alertMissionEntry.SetExpanded(false);
			for (int i = 0; i < dailyQuests.Length; i++)
			{
				dailyQuests[i].SetExpanded(false);
			}
		}
		if (!(QuestOfferPanel.Get() != null) || !(UIClickListener.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (!visible)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						UIClickListener.Get().Disable();
						return;
					}
				}
			}
			if (!m_clickListenerExceptions.Contains(QuestOfferPanel.Get().m_questListButton.spriteController.gameObject))
			{
				m_clickListenerExceptions.Add(QuestOfferPanel.Get().m_questListButton.spriteController.gameObject);
			}
			UIClickListener.Get().Enable(m_clickListenerExceptions, delegate
			{
				SetVisible(false);
			});
			return;
		}
	}

	private void StartFadeIn()
	{
		m_displayState = DisplayStates.FadeIn;
		if (!(m_animator != null))
		{
			return;
		}
		while (true)
		{
			m_animator.Play("ContractListDefaultIN");
			return;
		}
	}

	private void StartIdle()
	{
		m_displayState = DisplayStates.Idle;
		if (!(m_animator != null))
		{
			return;
		}
		while (true)
		{
			m_animator.Play("ContractListDefaultIDLE");
			return;
		}
	}

	private void StartFadeOut()
	{
		m_displayState = DisplayStates.FadeOut;
		if (m_animator != null)
		{
			m_animator.Play("ContractListDefaultOUT");
		}
	}

	private void Update()
	{
		bool flag;
		if (Input.GetMouseButtonDown(0))
		{
			flag = true;
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
									while (true)
									{
										if (componentInParent2 != null)
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
										}
										else
										{
										}
										break;
									}
								}
							}
							if (!(componentInParent != null))
							{
								if (!flag2)
								{
									goto IL_01a8;
								}
							}
							flag = false;
						}
					}
				}
			}
			goto IL_01a8;
		}
		goto IL_01e8;
		IL_01a8:
		if (flag)
		{
			if (m_isVisible)
			{
				if (UIFrontEnd.Get() != null)
				{
					UIFrontEnd.Get().m_frontEndNavPanel.NotificationBtnClicked(null);
				}
			}
		}
		goto IL_01e8;
		IL_01e8:
		if (m_displayState == DisplayStates.FadeIn)
		{
			if (IsAnimationDone(m_animator, "ContractListDefaultIN"))
			{
				StartIdle();
			}
		}
		if (m_displayState == DisplayStates.FadeOut)
		{
			if (IsAnimationDone(m_animator, "ContractListDefaultOUT"))
			{
				UIManager.SetGameObjectActive(m_animator, false);
				m_displayState = DisplayStates.None;
			}
		}
		int num = 0;
		bool flag3 = false;
		for (int i = 0; i < dailyQuests.Length; i++)
		{
			if (dailyQuests[i].IsExpanded())
			{
				num++;
			}
			if (dailyQuests[i].IsAnimating())
			{
				flag3 = true;
			}
		}
		while (true)
		{
			if (!flag3)
			{
				if (num == 0)
				{
					Vector2 anchoredPosition = (m_gridLayout.transform as RectTransform).anchoredPosition;
					if (anchoredPosition.y != 0f)
					{
						RectTransform obj = m_gridLayout.transform as RectTransform;
						Vector2 anchoredPosition2 = (m_gridLayout.transform as RectTransform).anchoredPosition;
						obj.anchoredPosition = new Vector2(anchoredPosition2.x, 0f);
					}
				}
			}
			if (m_timeTillNextAlert.HasValue)
			{
				while (true)
				{
					TimeSpan timeSpan = m_timeTillNextAlert.Value.Subtract(ClientGameManager.Get().PacificNow());
					string timeDifferenceText = StringUtil.GetTimeDifferenceText(timeSpan, true);
					if (timeSpan > TimeSpan.Zero && !timeDifferenceText.IsNullOrEmpty())
					{
						m_nextAlertTime.text = string.Format(StringUtil.TR("TimeUntilNextAlert", "Global"), timeDifferenceText);
					}
					else
					{
						m_nextAlertTime.text = StringUtil.TR("AlertIncoming", "Global");
					}
					m_alertTimeRemaining.text = string.Empty;
					UIManager.SetGameObjectActive(m_emptyAlertLabel, false);
					return;
				}
			}
			if (ClientGameManager.Get().AlertMissionsData.CurrentAlert == null)
			{
				return;
			}
			ActiveAlertMission currentAlert = ClientGameManager.Get().AlertMissionsData.CurrentAlert;
			TimeSpan timeSpan2 = currentAlert.StartTimePST.AddHours(currentAlert.DurationHours).Subtract(ClientGameManager.Get().PacificNow());
			string timeDifferenceText2 = StringUtil.GetTimeDifferenceText(timeSpan2, true);
			if (timeSpan2 > TimeSpan.Zero)
			{
				if (!timeDifferenceText2.IsNullOrEmpty())
				{
					m_alertTimeRemaining.text = string.Format(StringUtil.TR("QuestRemainingTime", "Global"), timeDifferenceText2);
					goto IL_0530;
				}
			}
			if (!m_alertTimeRemaining.text.IsNullOrEmpty())
			{
				m_alertTimeRemaining.text = string.Empty;
				UIManager.SetGameObjectActive(m_emptyAlertContainer, true);
				UIManager.SetGameObjectActive(m_normalAlertText, false);
				m_alertMissionEntry.Setup(null);
			}
			goto IL_0530;
			IL_0530:
			m_nextAlertTime.text = string.Empty;
			UIManager.SetGameObjectActive(m_emptyAlertLabel, true);
			return;
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (clip.name != animName)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		return true;
	}

	public void HandleQuestAdded(int questId)
	{
		if (m_lastSetupQuestIds.Contains(questId))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_lastSetupQuestIds.Add(questId);
		UpdateQuests(m_lastSetupQuestIds, true);
	}

	private void HandleAccountDataUpdated(PersistedAccountData accountData)
	{
		List<int> list = new List<int>();
		int num = -1;
		CheckSeasonsVisibility();
		foreach (KeyValuePair<int, QuestProgress> item in accountData.QuestComponent.Progress)
		{
			int key = item.Key;
			if (QuestWideData.Get().IsDailyQuest(key))
			{
				list.Add(key);
			}
			if (ClientGameManager.Get().IsCurrentAlertQuest(key))
			{
				num = key;
			}
		}
		UpdateQuests(list);
		if (num > -1)
		{
			SetupLatestAlert(ClientGameManager.Get().AlertMissionsData);
		}
		SetupSeasonChapter();
		if (!m_firstLoggedIn)
		{
			m_firstLoggedIn = true;
			int num2 = 0;
			using (Dictionary<int, QuestProgress>.Enumerator enumerator2 = accountData.QuestComponent.Progress.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					int key2 = enumerator2.Current.Key;
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
		m_areDailesUnlocked = accountData.AccountComponent.DailyQuestsAvailable;
		if (!(m_accountLevelReqText != null))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_accountLevelReqText, !m_areDailesUnlocked);
			return;
		}
	}

	private void HandleLobbyServerClientAccessLevelChange(ClientAccessLevel oldLevel, ClientAccessLevel newLevel)
	{
		UpdateQuests(m_lastSetupQuestIds, true);
		if (ClientGameManager.Get().AlertMissionsData != null)
		{
			SetupLatestAlert(ClientGameManager.Get().AlertMissionsData);
		}
		SetupSeasonChapter();
	}

	private void HandleQuestProgressChanged(QuestProgress[] progress)
	{
		for (int i = 0; i < dailyQuests.Length; i++)
		{
			int num = 0;
			while (true)
			{
				if (num < progress.Length)
				{
					if (dailyQuests[i].UpdateProgress(progress[num]))
					{
						break;
					}
					num++;
					continue;
				}
				break;
			}
		}
		while (true)
		{
			switch (7)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void CheckSeasonsVisibility()
	{
		bool flag = UISeasonsPanel.CheckSeasonsVisibility(out m_seasonLockoutReason);
		m_chapterTabBtn.SetDisabled(!flag);
		m_chapterTabBtn.spriteController.SetForceHovercallback(!flag);
		m_chapterTabBtn.spriteController.SetForceExitCallback(!flag);
	}

	private bool SetupTooltip(UITooltipBase tooltip)
	{
		if (m_seasonLockoutReason == SeasonLockoutReason.InTutorialSeason)
		{
			if (ClientGameManager.Get() != null && ClientGameManager.Get().GetPlayerAccountData() != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
					{
						int activeSeason = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
						SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(activeSeason);
						UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
						uITitledTooltip.Setup(StringUtil.TR("Locked", "Global"), string.Format(StringUtil.TR("RequiresMatchesPlayed", "Global"), QuestWideData.GetEndLevel(seasonTemplate.Prerequisites, activeSeason)), string.Empty);
						return true;
					}
					}
				}
			}
		}
		else if (m_seasonLockoutReason == SeasonLockoutReason.Disabled)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					UITitledTooltip uITitledTooltip2 = tooltip as UITitledTooltip;
					uITitledTooltip2.Setup(StringUtil.TR("Locked", "Global"), StringUtil.TR("SeasonsDisabled", "Global"), string.Empty);
					return true;
				}
				}
			}
		}
		return false;
	}

	private void SetupLatestAlert(LobbyAlertMissionDataNotification notification)
	{
		m_alertTimeRemaining.text = string.Empty;
		m_nextAlertTime.text = string.Empty;
		UIManager.SetGameObjectActive(m_emptyAlertLabel, true);
		UIManager.SetGameObjectActive(m_alertContainer, notification.AlertMissionsEnabled);
		if (!notification.AlertMissionsEnabled)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					m_timeTillNextAlert = null;
					return;
				}
			}
		}
		m_timeTillNextAlert = notification.NextAlert;
		if (notification.CurrentAlert == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					UIManager.SetGameObjectActive(m_emptyAlertContainer, true);
					UIManager.SetGameObjectActive(m_normalAlertText, false);
					m_alertMissionEntry.Setup(null);
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_emptyAlertContainer, false);
		if (notification.CurrentAlert.Type == AlertMissionType.Quest)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					UIManager.SetGameObjectActive(m_normalAlertText, false);
					m_alertMissionEntry.Setup(notification.CurrentAlert);
					return;
				}
			}
		}
		if (notification.CurrentAlert.Type == AlertMissionType.Bonus)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					m_alertMissionEntry.Setup(null);
					UIManager.SetGameObjectActive(m_normalAlertText, true);
					m_normalAlertText.text = string.Format(StringUtil.TR(new StringBuilder().Append("BonusAlertDescription").Append(notification.CurrentAlert.BonusType).ToString(), "Global"), notification.CurrentAlert.BonusMultiplier);
					return;
				}
			}
		}
		throw new NotImplementedException();
	}

	private void SetupSeasonChapter()
	{
		PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
		int num = 0;
		using (List<SeasonTemplate>.Enumerator enumerator = SeasonWideData.Get().m_seasons.GetEnumerator())
		{
			while (true)
			{
				IL_0079:
				if (!enumerator.MoveNext())
				{
					break;
				}
				SeasonTemplate current = enumerator.Current;
				if (current.Index != playerAccountData.QuestComponent.ActiveSeason)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							if (playerAccountData.QuestComponent.ActiveSeason != 0)
							{
								goto IL_0079;
							}
							goto IL_0070;
						}
					}
				}
				goto IL_0070;
				IL_0070:
				num = current.Index;
				break;
			}
		}
		int num2 = 0;
		using (List<int>.Enumerator enumerator2 = playerAccountData.QuestComponent.GetUnlockedSeasonChapters(num).GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				int current2 = enumerator2.Current;
				if (current2 > num2)
				{
					num2 = current2;
				}
			}
		}
		UIPlayerSeasonDisplayInfo uIPlayerSeasonDisplayInfo = new UIPlayerSeasonDisplayInfo();
		uIPlayerSeasonDisplayInfo.Setup(num, playerAccountData);
		UISeasonChapterEntry uISeasonChapterEntry = uIPlayerSeasonDisplayInfo.ChapterEntries[num2];
		int num3 = 0;
		int num4 = 0;
		bool flag = false;
		for (int i = 0; i < uISeasonChapterEntry.QuestInfo.Count; i++)
		{
			if (i >= m_questEntryList.Count)
			{
				flag = true;
				UISeasonsQuestEntry uISeasonsQuestEntry = UnityEngine.Object.Instantiate(m_chapterQuestListEntryPrefab);
				uISeasonsQuestEntry.transform.SetParent(m_chapterQuestListContainer.transform);
				uISeasonsQuestEntry.transform.localScale = Vector3.one;
				uISeasonsQuestEntry.transform.localPosition = Vector3.zero;
				m_questEntryList.Add(uISeasonsQuestEntry);
				uISeasonsQuestEntry.m_btnHitBox.spriteController.RegisterScrollListener(OnChapterScroll);
			}
			StaggerComponent.SetStaggerComponent(m_questEntryList[i].gameObject, true);
			m_questEntryList[i].Setup(uISeasonChapterEntry.QuestInfo[i], false);
			m_questEntryList[i].SetExpanded(false);
			m_questEntryList[i].SetMouseEventScroll(m_chapterQuestChallengeScrollList);
			num3 = i;
			if (uISeasonChapterEntry.QuestInfo[i].Completed)
			{
				num4++;
			}
		}
		while (true)
		{
			if (flag)
			{
				if (HitchDetector.Get() != null)
				{
					HitchDetector.Get().AddNewLayoutGroup(m_chapterQuestListContainer);
				}
			}
			for (num3++; num3 < m_questEntryList.Count; num3++)
			{
				StaggerComponent.SetStaggerComponent(m_questEntryList[num3].gameObject, false);
			}
			int num5 = uIPlayerSeasonDisplayInfo.SeasonNumber;
			if (SeasonWideData.Get() != null)
			{
				num5 = SeasonWideData.Get().GetPlayerFacingSeasonNumber(uIPlayerSeasonDisplayInfo.SeasonNumber);
			}

			m_chapterHeader.text = new StringBuilder().Append(string.Format(StringUtil.TR("SeasonNumber", "Global"), num5)).Append(": ").Append(string.Format(StringUtil.TR("ChapterNumber", "Global"), num2 + 1)).Append("  (").Append(num4).Append("/").Append(uISeasonChapterEntry.QuestInfo.Count).Append(")").ToString();
			return;
		}
	}

	private void OnChapterScroll(BaseEventData data)
	{
		m_chapterQuestChallengeScrollList.OnScroll((PointerEventData)data);
	}

	private void OnDailyScroll(BaseEventData data)
	{
		m_dailyQuestScrollList.OnScroll((PointerEventData)data);
	}
}
