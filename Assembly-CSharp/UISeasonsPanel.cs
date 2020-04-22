using I2.Loc;
using LobbyGameClientMessages;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISeasonsPanel : UIScene
{
	public RectTransform m_container;

	public RectTransform m_OverviewContainer;

	public RectTransform m_seasonsChapterBtnContainer;

	public RectTransform m_lockedSeasonsContainer;

	public RectTransform m_lockedSeasonChapterOverlay;

	public ScrollRect m_ChapterViewQuestChallengeScrollRect;

	public _SelectableBtn m_OverviewButton;

	public _SelectableBtn m_currentChapterButton;

	[Header("Top Bar Elements")]
	public _SelectableBtn m_previousSeasonsBtn;

	public _SelectableBtn m_purchaseLevelsBtn;

	public ImageFilledSloped m_PlayerCurrentSeasonLevelProgress;

	public TextMeshProUGUI m_PlayerSeasonLevelLabel;

	public TextMeshProUGUI m_PlayerSeasonEXPLevelLabel;

	public ImageFilledSloped m_CommunityRankProgressBar;

	public TextMeshProUGUI m_CommunityRankLabel;

	public TextMeshProUGUI m_CommunityEXPRankLabel;

	public TextMeshProUGUI SeasonName;

	public TextMeshProUGUI SeasonEndTime;

	public RectTransform m_topCommunityContainer;

	public RectTransform m_topFactionsContainer;

	public UISeasonFactionPercentageBar m_topPercentBarPrefab;

	public RectTransform m_topPercentBarcontainer;

	[Header("Overview/Rewards Elements")]
	public _LargeScrollList m_seasonsScrollList;

	public HorizontalLayoutGroup m_repeatingGridContainer;

	public UISeasonsLevelHeaderElement[] m_rewardElements;

	public RectTransform m_bottomCommunityContainer;

	public RectTransform m_bottomFactionsContainer;

	[Header("Chapter Elements")]
	public RectTransform m_ChapterContainer;

	public RectTransform m_ChapterPageBtnContainer;

	public RectTransform m_ChapterRewardsContainer;

	public ScrollRect m_QuestChallengeScrollList;

	public _SelectableBtn[] m_ChapterPageBtn;

	public _SelectableBtn m_prevChapterPageBtn;

	public _SelectableBtn m_nextChapterPageBtn;

	public TextMeshProUGUI ChapterName;

	public TextMeshProUGUI ChapterDescription;

	public Image ChapterImage;

	public VerticalLayoutGroup m_QuestListContainer;

	public UISeasonsQuestEntry m_QuestListEntryPrefab;

	public TextMeshProUGUI m_questHeaderTitle;

	public Animator m_questsCompletedStamp;

	public Animator m_questsEndedStamp;

	public QuestReward[] m_CompletedChapterRewards;

	public RectTransform m_ChapterTextContainer;

	public ScrollRect ChapterDescriptionContainer;

	public _ButtonSwapSprite m_ChapterTextClickArea;

	public TextMeshProUGUI m_ChapterClickToReadMoreText;

	public float m_scrollHeightMin = 100f;

	public float m_scrollHeightMax = 300f;

	public float m_scrollTopOffset = 5f;

	[Header("Factions Elements")]
	public UISeasonPanelViewEntry m_seasonFactionPrefab;

	public VerticalLayoutGroup m_seasonFactionList;

	public _ButtonSwapSprite m_factionsMoreInfoButton;

	private static UISeasonsPanel s_instance;

	private bool m_isVisible;

	private int m_selectedChapterIndex;

	private UIPlayerSeasonDisplayInfo m_selectedSeason;

	private List<UIPlayerSeasonDisplayInfo> displayInfo;

	private List<UISeasonsQuestEntry> m_questEntryList;

	private List<Mask> m_scrollMasks;

	private Image m_currentChapterLockIcon;

	private bool m_ChapterTextExpanded;

	private Vector2 startChapterExpandLocation;

	private Vector2 endChapterExpandLocation;

	private float ChapterExpandStartTime;

	private float ChapterExpandJourneyLength;

	private float ChapterExpandSpeed = 1f;

	public UIPlayerSeasonDisplayInfo GetSeasonDisplayInfo()
	{
		return m_selectedSeason;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.Seasons;
	}

	public override void Awake()
	{
		s_instance = this;
		base.Awake();
		m_scrollMasks = new List<Mask>();
		m_scrollMasks.Add(m_ChapterViewQuestChallengeScrollRect.GetComponent<Mask>());
		m_scrollMasks.Add(m_QuestChallengeScrollList.GetComponent<Mask>());
		m_scrollMasks.Add(m_seasonsScrollList.GetScrollRect().GetComponent<Mask>());
		UIManager.SetGameObjectActive(m_previousSeasonsBtn, LocalizationManager.CurrentLanguageCode != "zh");
		m_previousSeasonsBtn.spriteController.callback = PreviousSeasonsBtnClicked;
		m_OverviewButton.spriteController.callback = HeaderBtnClicked;
		m_factionsMoreInfoButton.callback = FactionsMoreInfoClicked;
		UIManager.SetGameObjectActive(m_purchaseLevelsBtn, false);
		SetupChapterButtonName(0);
		endChapterExpandLocation = m_ChapterTextContainer.sizeDelta;
		m_ChapterTextClickArea.callback = ChapterTextAreaClicked;
		_MouseEventPasser mouseEventPasser = m_ChapterTextClickArea.gameObject.AddComponent<_MouseEventPasser>();
		mouseEventPasser.AddNewHandler(ChapterDescriptionContainer);
		mouseEventPasser = ChapterDescriptionContainer.verticalScrollbar.handleRect.gameObject.AddComponent<_MouseEventPasser>();
		mouseEventPasser.AddNewHandler(ChapterDescriptionContainer);
		displayInfo = new List<UIPlayerSeasonDisplayInfo>();
		m_questEntryList = new List<UISeasonsQuestEntry>();
		UIManager.SetGameObjectActive(m_seasonsChapterBtnContainer, false);
		UIManager.SetGameObjectActive(m_questsCompletedStamp, false);
		m_seasonsScrollList.GetScrollRect().scrollSensitivity = 150f;
		m_ChapterViewQuestChallengeScrollRect.scrollSensitivity = 125f;
		m_ChapterViewQuestChallengeScrollRect.elasticity = 0.01f;
		UIManager.SetGameObjectActive(m_bottomCommunityContainer, false);
		UIManager.SetGameObjectActive(m_topCommunityContainer, false);
		OnFactionCompetitionNotification(null);
		ClientGameManager.Get().OnAccountDataUpdated += OnAccountDataUpdated;
		ClientGameManager.Get().OnCharacterDataUpdated += OnCharacterDataUpdated;
		ClientGameManager.Get().OnSeasonChapterQuestsChange += OnSeasonChapterQuestsChange;
		ClientGameManager.Get().OnFactionCompetitionNotification += OnFactionCompetitionNotification;
		ClientGameManager.Get().OnPlayerFactionContributionChangeNotification += OnPlayerFactionContributionChangeNotification;
		ClientGameManager.Get().OnLobbyGameplayOverridesChange += OnLobbyGameplayOverridesChange;
		ClientGameManager.Get().OnSeasonCompleteNotification += OnSeasonCompleteNotification;
		if (!(HitchDetector.Get() != null))
		{
			return;
		}
		while (true)
		{
			HitchDetector.Get().AddNewLayoutGroup(m_repeatingGridContainer);
			HitchDetector.Get().AddNewLayoutGroup(m_QuestListContainer);
			HitchDetector.Get().AddNewLayoutGroup(m_seasonFactionList);
			HitchDetector.Get().AddNewLayoutGroup(m_seasonsChapterBtnContainer.GetComponentInChildren<LayoutGroup>(true));
			return;
		}
	}

	private void SetupChapterButtonName(int chapterIndex)
	{
		m_currentChapterButton.spriteController.callback = HeaderBtnClicked;
		m_currentChapterButton.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, (UITooltipBase tooltip) => HeaderBtnTooltipSetup(tooltip, chapterIndex));
		TextMeshProUGUI[] componentsInChildren = m_currentChapterButton.GetComponentsInChildren<TextMeshProUGUI>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].text = string.Format(StringUtil.TR("SeasonChapterLabel", "Seasons"), (chapterIndex + 1).ToString());
		}
		while (true)
		{
			Image[] componentsInChildren2 = m_currentChapterButton.GetComponentsInChildren<Image>(true);
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				if (!(componentsInChildren2[j].gameObject.name == "LockedIcon"))
				{
					continue;
				}
				while (true)
				{
					m_currentChapterLockIcon = componentsInChildren2[j];
					return;
				}
			}
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private void SetupChapterButtonSpriteController(UIPlayerSeasonDisplayInfo seasonInfo)
	{
		bool setActive = false;
		if (m_selectedChapterIndex >= 0 && m_selectedChapterIndex < seasonInfo.ChapterEntries.Count)
		{
			if (!seasonInfo.ChapterEntries[m_selectedChapterIndex].IsChapterHidden)
			{
				setActive = true;
			}
			bool isChapterViewable = seasonInfo.ChapterEntries[m_selectedChapterIndex].IsChapterViewable;
			bool flag = seasonInfo.ChapterEntries[m_selectedChapterIndex].IsChapterLocked;
			if (flag && !seasonInfo.ChapterEntries[m_selectedChapterIndex].AreQuestsStatic)
			{
				if (ClientGameManager.Get() != null)
				{
					if (seasonInfo.ChapterEntries[m_selectedChapterIndex].StartDate < ClientGameManager.Get().PacificNow())
					{
						flag = false;
					}
				}
			}
			if (m_currentChapterLockIcon != null)
			{
				UIManager.SetGameObjectActive(m_currentChapterLockIcon, flag);
			}
			UIManager.SetGameObjectActive(m_currentChapterButton.spriteController.m_defaultImage, isChapterViewable);
			UIManager.SetGameObjectActive(m_currentChapterButton.spriteController.m_hoverImage, isChapterViewable);
			UIManager.SetGameObjectActive(m_currentChapterButton.spriteController.m_pressedImage, isChapterViewable);
			m_currentChapterButton.spriteController.SetClickable(isChapterViewable);
			m_currentChapterButton.spriteController.SetForceExitCallback(true);
			m_currentChapterButton.spriteController.SetForceHovercallback(true);
		}
		StaggerComponent.SetStaggerComponent(m_currentChapterButton.gameObject, setActive);
	}

	private void OnDestroy()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		clientGameManager.OnAccountDataUpdated -= OnAccountDataUpdated;
		clientGameManager.OnCharacterDataUpdated -= OnCharacterDataUpdated;
		clientGameManager.OnSeasonChapterQuestsChange -= OnSeasonChapterQuestsChange;
		clientGameManager.OnFactionCompetitionNotification -= OnFactionCompetitionNotification;
		clientGameManager.OnPlayerFactionContributionChangeNotification -= OnPlayerFactionContributionChangeNotification;
		clientGameManager.OnLobbyGameplayOverridesChange -= OnLobbyGameplayOverridesChange;
		clientGameManager.OnSeasonCompleteNotification -= OnSeasonCompleteNotification;
	}

	private void OnAccountDataUpdated(PersistedAccountData newData)
	{
		Setup(false, false, false);
	}

	private void OnCharacterDataUpdated(PersistedCharacterData newData)
	{
		RefreshRewardsEntries();
	}

	private void OnSeasonChapterQuestsChange(Dictionary<int, SeasonChapterQuests> seasonChapterQuests)
	{
		Setup(false, false, false);
	}

	private void OnPlayerFactionContributionChangeNotification(PlayerFactionContributionChangeNotification notification)
	{
		UpdatePersonalContribution();
	}

	private void OnLobbyGameplayOverridesChange(LobbyGameplayOverrides gameplayOverrides)
	{
		OnFactionCompetitionNotification(null);
	}

	public void UpdatePersonalContribution()
	{
		UISeasonPanelViewEntry[] componentsInChildren = m_seasonFactionList.GetComponentsInChildren<UISeasonPanelViewEntry>(true);
		int activeFactionCompetition = ClientGameManager.Get().ActiveFactionCompetition;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			FactionPlayerData playerCompetitionFactionData = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetPlayerCompetitionFactionData(activeFactionCompetition, i);
			componentsInChildren[i].UpdateContributionBar(playerCompetitionFactionData);
		}
	}

	private void OnFactionCompetitionNotification(FactionCompetitionNotification notification)
	{
		int index;
		Dictionary<int, long> dictionary;
		if (notification == null)
		{
			index = ClientGameManager.Get().ActiveFactionCompetition;
			dictionary = ClientGameManager.Get().FactionScores;
		}
		else
		{
			index = notification.ActiveIndex;
			dictionary = notification.Scores;
		}
		FactionCompetition factionCompetition = FactionWideData.Get().GetFactionCompetition(index);
		if (factionCompetition != null)
		{
			if (factionCompetition.Enabled)
			{
				if (factionCompetition.ShouldShowcase)
				{
					UIManager.SetGameObjectActive(m_topFactionsContainer, true);
					UIManager.SetGameObjectActive(m_bottomFactionsContainer, true);
					List<UISeasonPanelViewEntry> list = new List<UISeasonPanelViewEntry>();
					list.AddRange(m_seasonFactionList.GetComponentsInChildren<UISeasonPanelViewEntry>(true));
					for (int num = list.Count - 1; num >= factionCompetition.Factions.Count; num--)
					{
						Object.Destroy(list[num].gameObject);
						list.RemoveAt(num);
					}
					while (true)
					{
						for (int i = list.Count; i < factionCompetition.Factions.Count; i++)
						{
							UISeasonPanelViewEntry uISeasonPanelViewEntry = Object.Instantiate(m_seasonFactionPrefab);
							uISeasonPanelViewEntry.transform.SetParent(m_seasonFactionList.transform);
							uISeasonPanelViewEntry.transform.localScale = Vector3.one;
							uISeasonPanelViewEntry.transform.localPosition = Vector3.zero;
							list.Add(uISeasonPanelViewEntry);
						}
						while (true)
						{
							List<UISeasonFactionPercentageBar> list2 = new List<UISeasonFactionPercentageBar>();
							list2.AddRange(m_topPercentBarcontainer.GetComponentsInChildren<UISeasonFactionPercentageBar>(true));
							for (int num2 = list2.Count - 1; num2 >= factionCompetition.Factions.Count; num2--)
							{
								Object.Destroy(list2[num2].gameObject);
								list2.RemoveAt(num2);
							}
							for (int j = list2.Count; j < factionCompetition.Factions.Count; j++)
							{
								UISeasonFactionPercentageBar uISeasonFactionPercentageBar = Object.Instantiate(m_topPercentBarPrefab);
								uISeasonFactionPercentageBar.transform.SetParent(m_topPercentBarcontainer.transform);
								uISeasonFactionPercentageBar.transform.localScale = Vector3.one;
								uISeasonFactionPercentageBar.transform.localPosition = Vector3.zero;
								list2.Add(uISeasonFactionPercentageBar);
							}
							while (true)
							{
								long num3 = 0L;
								for (int k = 0; k < factionCompetition.Factions.Count; k++)
								{
									dictionary.TryGetValue(k, out long value);
									list[k].Setup(factionCompetition.Factions[k], value, k);
									num3 += value;
								}
								while (true)
								{
									float num4 = 0f;
									for (int l = 0; l < factionCompetition.Factions.Count; l++)
									{
										if (num3 == 0)
										{
											UIManager.SetGameObjectActive(list2[l], false);
											continue;
										}
										dictionary.TryGetValue(l, out long value2);
										if (value2 > 0)
										{
											float num5 = (float)value2 / (float)num3;
											UIManager.SetGameObjectActive(list2[l], true);
											float[] rBGA = FactionWideData.Get().GetRBGA(factionCompetition.Factions[l]);
											Color factionColor = new Color(rBGA[0], rBGA[1], rBGA[2], rBGA[3]);
											num4 = list2[l].Setup(num4, num5 + num4, factionColor);
										}
										else
										{
											UIManager.SetGameObjectActive(list2[l], true);
										}
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
				}
			}
		}
		UIManager.SetGameObjectActive(m_topFactionsContainer, false);
		UIManager.SetGameObjectActive(m_bottomFactionsContainer, false);
	}

	public void PreviousSeasonsBtnClicked(BaseEventData data)
	{
		UILandingPageFullScreenMenus.Get().DisplayPreviousSeasonChapter();
	}

	public void ChapterTextAreaClicked(BaseEventData data)
	{
		ToggleChapterTextAreaExpand();
	}

	private void UpdateChapterText()
	{
		if (m_selectedSeason == null)
		{
			return;
		}
		while (true)
		{
			if (m_ChapterTextExpanded)
			{
				ChapterDescription.text = StringUtil.TR_SeasonStorytimeLongBody(m_selectedSeason.SeasonNumber, m_selectedChapterIndex + 1, 1);
				for (int i = 1; i < m_selectedSeason.ChapterEntries[m_selectedChapterIndex].SeasonChapterStory.Count; i++)
				{
					ChapterDescription.text += StringUtil.TR_SeasonStorytimeLongBody(m_selectedSeason.SeasonNumber, m_selectedChapterIndex + 1, i + 1);
				}
			}
			else
			{
				ChapterDescription.text = StringUtil.TR_SeasonStorytimeBody(m_selectedSeason.SeasonNumber, m_selectedChapterIndex + 1, 1);
			}
			RectTransform obj = ChapterDescription.transform as RectTransform;
			Vector2 sizeDelta = (ChapterDescription.transform as RectTransform).sizeDelta;
			float x = sizeDelta.x;
			Vector2 preferredValues = ChapterDescription.GetPreferredValues();
			float y = preferredValues.y;
			float num;
			if (m_ChapterTextExpanded)
			{
				num = m_scrollHeightMax;
			}
			else
			{
				num = m_scrollHeightMin;
			}
			obj.sizeDelta = new Vector2(x, y - num);
			return;
		}
	}

	private IEnumerator DoSetChapterTextAreaExpand(bool expanded)
	{
		m_ChapterTextExpanded = expanded;
		UIManager.SetGameObjectActive(m_ChapterClickToReadMoreText, !expanded);
		UpdateChapterText();
		yield return 0;
		startChapterExpandLocation = m_ChapterTextContainer.sizeDelta;
		ChapterExpandStartTime = Time.time;
		ChapterDescriptionContainer.verticalScrollbar.value = 1f;
		if (m_ChapterTextExpanded)
		{
			Vector2 sizeDelta = m_ChapterTextContainer.sizeDelta;
			endChapterExpandLocation = new Vector2(sizeDelta.x, m_scrollHeightMax);
		}
		else
		{
			Vector2 sizeDelta2 = m_ChapterTextContainer.sizeDelta;
			endChapterExpandLocation = new Vector2(sizeDelta2.x, m_scrollHeightMin);
		}
		RectTransform content = ChapterDescriptionContainer.content;
		Vector2 sizeDelta3 = ChapterDescriptionContainer.content.sizeDelta;
		float x = sizeDelta3.x;
		Vector2 preferredValues = ChapterDescription.GetPreferredValues();
		float y = preferredValues.y;
		Vector2 preferredValues2 = ChapterName.GetPreferredValues();
		float num = y + preferredValues2.y + ChapterDescription.fontSize * 2f;
		float num2;
		if (m_ChapterTextExpanded)
		{
			num2 = m_scrollHeightMax;
		}
		else
		{
			num2 = m_scrollHeightMin;
		}
		content.sizeDelta = new Vector2(x, num - num2);
		ChapterExpandJourneyLength = Vector2.Distance(startChapterExpandLocation, endChapterExpandLocation);
		ChapterExpandSpeed = ChapterExpandJourneyLength * 4f;
	}

	private void ToggleChapterTextAreaExpand()
	{
		SetToggleChapterTextAreaExpand(!m_ChapterTextExpanded);
	}

	private void SetToggleChapterTextAreaExpand(bool expanded)
	{
		if (m_ChapterTextExpanded == expanded)
		{
			return;
		}
		while (true)
		{
			StartCoroutine(DoSetChapterTextAreaExpand(expanded));
			return;
		}
	}

	public void SelectChapter(int chapterNumber)
	{
		SetSelectedHeaderButton(m_currentChapterButton);
	}

	public void HeaderBtnClicked(BaseEventData data)
	{
		_SelectableBtn componentInParent = (data as PointerEventData).pointerCurrentRaycast.gameObject.GetComponentInParent<_SelectableBtn>();
		if (componentInParent != null)
		{
			SetSelectedHeaderButton(componentInParent);
		}
		SetToggleChapterTextAreaExpand(false);
	}

	private bool HeaderBtnTooltipSetup(UITooltipBase tooltip, int index)
	{
		string empty;
		if (index < m_selectedSeason.ChapterEntries.Count && m_selectedSeason.ChapterEntries[index].IsChapterLocked)
		{
			empty = string.Empty;
			if (index > 0)
			{
				if (!m_selectedSeason.ChapterEntries[index - 1].AreQuestsStatic)
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
			}
			if (index > 0)
			{
				if (m_selectedSeason.ChapterEntries[index - 1].NumQuestsToAdvance != 0)
				{
					string arg = m_selectedSeason.ChapterEntries[index].NumQuestsToAdvance.ToString();
					string text;
					if (m_selectedSeason.ChapterEntries[index].AreAllQuestsCompleteFromPreviousChapter)
					{
						text = string.Format(StringUtil.TR("UnlockChapterGreenDesc", "Seasons"), index, arg);
					}
					else
					{
						text = string.Format(StringUtil.TR("UnlockChapterRedDesc", "Seasons"), index, arg);
					}
					empty = text;
					goto IL_0198;
				}
			}
			empty = ((!m_selectedSeason.ChapterEntries[index].AreAllQuestsCompleteFromPreviousChapter) ? string.Format(StringUtil.TR("UnlockChapterAllRedDesc", "Seasons"), index) : string.Format(StringUtil.TR("UnlockChapterAllGreenDesc", "Seasons"), index));
			goto IL_0198;
		}
		return false;
		IL_0198:
		bool flag = false;
		if (m_selectedSeason.ChapterEntries[index].AreOtherConditionsFromPreviousChapterMet)
		{
			flag = true;
		}
		if (!flag)
		{
			empty = empty + "<color=red>" + StringUtil.TR_SeasonChapterUnlock(m_selectedSeason.SeasonNumber, index + 1) + "</color>";
		}
		(tooltip as UITitledTooltip).Setup(StringUtil.TR("ChapterIsLocked", "Seasons"), empty, string.Empty);
		return true;
	}

	public void FactionsMoreInfoClicked(BaseEventData data)
	{
		UIFactionsIntroduction.Get().SetupIntro(null);
	}

	public static UISeasonsPanel Get()
	{
		return s_instance;
	}

	public bool IsVisible()
	{
		return m_isVisible;
	}

	private void Update()
	{
		if (!(m_ChapterTextContainer.sizeDelta != endChapterExpandLocation))
		{
			return;
		}
		while (true)
		{
			float num = (Time.time - ChapterExpandStartTime) * ChapterExpandSpeed;
			float t = num / ChapterExpandJourneyLength;
			m_ChapterTextContainer.sizeDelta = Vector2.Lerp(startChapterExpandLocation, endChapterExpandLocation, t);
			RectTransform obj = ChapterDescriptionContainer.verticalScrollbar.transform as RectTransform;
			Vector2 sizeDelta = (ChapterDescriptionContainer.verticalScrollbar.transform as RectTransform).sizeDelta;
			float x = sizeDelta.x;
			Vector2 sizeDelta2 = m_ChapterTextContainer.sizeDelta;
			obj.sizeDelta = new Vector2(x, sizeDelta2.y - m_scrollTopOffset);
			return;
		}
	}

	private void SelectChapterStoryPage(UISeasonChapterEntry chapterInfo, int pageIndex)
	{
		if (pageIndex < chapterInfo.SeasonChapterStory.Count)
		{
			UpdateChapterText();
			ChapterName.text = StringUtil.TR_SeasonStorytimeHeader(m_selectedSeason.SeasonNumber, m_selectedChapterIndex + 1, pageIndex + 1);
			ChapterImage.sprite = (Resources.Load(chapterInfo.SeasonChapterStory[pageIndex].ImageFilename, typeof(Sprite)) as Sprite);
		}
		else
		{
			ChapterName.text = chapterInfo.SeasonChapterName;
			ChapterDescription.text = chapterInfo.SeasonChapterName;
		}
		UIManager.SetGameObjectActive(m_lockedSeasonChapterOverlay, chapterInfo.IsChapterLocked);
	}

	private void SetupChapter(int chapterIndex)
	{
		if (m_selectedSeason == null)
		{
			return;
		}
		while (true)
		{
			if (chapterIndex < 0)
			{
				return;
			}
			while (true)
			{
				if (chapterIndex >= m_selectedSeason.ChapterEntries.Count)
				{
					return;
				}
				m_selectedChapterIndex = chapterIndex;
				UISeasonChapterEntry uISeasonChapterEntry = m_selectedSeason.ChapterEntries[chapterIndex];
				ChapterName.text = uISeasonChapterEntry.SeasonChapterName;
				SelectChapterStoryPage(uISeasonChapterEntry, 0);
				int num = 0;
				int num2 = 0;
				bool flag = false;
				for (int i = 0; i < uISeasonChapterEntry.QuestInfo.Count; i++)
				{
					if (i >= m_questEntryList.Count)
					{
						flag = true;
						UISeasonsQuestEntry uISeasonsQuestEntry = Object.Instantiate(m_QuestListEntryPrefab);
						uISeasonsQuestEntry.transform.SetParent(m_QuestListContainer.transform);
						uISeasonsQuestEntry.transform.localScale = Vector3.one;
						uISeasonsQuestEntry.transform.localPosition = Vector3.zero;
						m_questEntryList.Add(uISeasonsQuestEntry);
					}
					StaggerComponent.SetStaggerComponent(m_questEntryList[i].gameObject, true);
					m_questEntryList[i].Setup(uISeasonChapterEntry.QuestInfo[i], uISeasonChapterEntry.IsChapterLocked);
					m_questEntryList[i].SetExpanded(false);
					m_questEntryList[i].SetMouseEventScroll(m_QuestChallengeScrollList);
					num = i;
					if (uISeasonChapterEntry.QuestInfo[i].Completed)
					{
						num2++;
					}
				}
				if (flag)
				{
					if (HitchDetector.Get() != null)
					{
						HitchDetector.Get().AddNewLayoutGroup(m_QuestListContainer);
					}
				}
				for (num++; num < m_questEntryList.Count; num++)
				{
					StaggerComponent.SetStaggerComponent(m_questEntryList[num].gameObject, false);
				}
				while (true)
				{
					m_questHeaderTitle.text = string.Format(StringUtil.TR("ChapterContracts", "Seasons"), num2, uISeasonChapterEntry.QuestInfo.Count);
					if (num2 == uISeasonChapterEntry.QuestInfo.Count)
					{
						UIManager.SetGameObjectActive(m_questsCompletedStamp, true);
						UIManager.SetGameObjectActive(m_questsEndedStamp, false);
					}
					else if (!uISeasonChapterEntry.AreQuestsStatic)
					{
						if (ClientGameManager.Get().PacificNow() < uISeasonChapterEntry.EndDate)
						{
							string arg = string.Format(StringUtil.TR("DayMonthYear", "Global"), uISeasonChapterEntry.EndDate.Day, StringUtil.TR("Month" + uISeasonChapterEntry.EndDate.Month, "Global"), uISeasonChapterEntry.EndDate.Year);
							string str = string.Format(StringUtil.TR("MustBeCompletedBy", "Global"), arg);
							TextMeshProUGUI questHeaderTitle = m_questHeaderTitle;
							questHeaderTitle.text = questHeaderTitle.text + " <color=red>" + str + "</color>";
							UIManager.SetGameObjectActive(m_questsEndedStamp, false);
						}
						else
						{
							UIManager.SetGameObjectActive(m_questsEndedStamp, true);
						}
						UIManager.SetGameObjectActive(m_questsCompletedStamp, false);
					}
					else
					{
						UIManager.SetGameObjectActive(m_questsCompletedStamp, false);
						UIManager.SetGameObjectActive(m_questsEndedStamp, false);
					}
					int num3 = 0;
					using (List<QuestItemReward>.Enumerator enumerator = uISeasonChapterEntry.ItemRewards.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							QuestItemReward current = enumerator.Current;
							InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(current.ItemTemplateId);
							m_CompletedChapterRewards[num3].SetupHack(itemTemplate, itemTemplate.IconPath, current.Amount);
							num3++;
						}
					}
					using (List<QuestCurrencyReward>.Enumerator enumerator2 = uISeasonChapterEntry.CurrencyRewards.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							QuestCurrencyReward current2 = enumerator2.Current;
							m_CompletedChapterRewards[num3].Setup(current2, 0);
							num3++;
						}
					}
					using (List<QuestUnlockReward>.Enumerator enumerator3 = uISeasonChapterEntry.UnlockRewards.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							QuestUnlockReward current3 = enumerator3.Current;
							m_CompletedChapterRewards[num3].SetupHack(current3.resourceString);
							num3++;
						}
					}
					if (num3 > 0)
					{
						UIManager.SetGameObjectActive(m_ChapterRewardsContainer, true);
						for (int j = 0; j < m_CompletedChapterRewards.Length; j++)
						{
							UIManager.SetGameObjectActive(m_CompletedChapterRewards[j], j < num3);
						}
					}
					else
					{
						UIManager.SetGameObjectActive(m_ChapterRewardsContainer, false);
					}
					m_ChapterViewQuestChallengeScrollRect.verticalScrollbar.value = 1f;
					return;
				}
			}
		}
	}

	private void SetSelectedHeaderButton(_SelectableBtn selectedHeaderBtn)
	{
		bool flag = selectedHeaderBtn == m_OverviewButton;
		int selectedChapterIndex = m_selectedChapterIndex;
		m_OverviewButton.SetSelected(flag, false, string.Empty, string.Empty);
		UIManager.SetGameObjectActive(m_ChapterContainer, !flag);
		UIManager.SetGameObjectActive(m_OverviewContainer, flag);
		if (flag)
		{
			UpdatePersonalContribution();
			if (m_selectedSeason != null)
			{
				List<IDataEntry> itemList = m_selectedSeason.SeasonRewardEntries.ConvertAll<IDataEntry>(SeasonEntryToDataEntry);
				m_seasonsScrollList.Setup(itemList, Mathf.Max(m_selectedSeason.currentLevelDisplayIndex - 2, 0));
			}
			else
			{
				Log.Warning("Failed to setup season scroll list, current season is null (possibly no season is active?)");
			}
		}
		else
		{
			UIManager.SetGameObjectActive(m_ChapterPageBtnContainer, false);
			SetupChapter(selectedChapterIndex);
		}
		foreach (Mask scrollMask in m_scrollMasks)
		{
			scrollMask.enabled = false;
			scrollMask.enabled = true;
		}
	}

	public static IDataEntry SeasonEntryToDataEntry(UISeasonRewardEntry entry)
	{
		return entry;
	}

	private void SetSelectedSeason(UIPlayerSeasonDisplayInfo seasonInfo, bool displayOverviewButton, bool displayHighestChapter = true)
	{
		m_selectedSeason = seasonInfo;
		m_PlayerCurrentSeasonLevelProgress.fillAmount = m_selectedSeason.currentPercentThroughPlayerSeasonLevel;
		m_PlayerSeasonLevelLabel.text = string.Format(StringUtil.TR("SeasonLevelNumber", "Seasons"), m_selectedSeason.PlayerSeasonLevel.ToString());
		m_PlayerSeasonEXPLevelLabel.text = string.Format(StringUtil.TR("SeasonEXPNumber", "Seasons"), m_selectedSeason.currentXPThroughPlayerLevel, SeasonWideData.Get().GetSeasonExperience(seasonInfo.SeasonNumber, seasonInfo.PlayerSeasonLevel));
		m_CommunityRankLabel.text = StringUtil.TR("CommunityRank", "Seasons");
		m_CommunityEXPRankLabel.text = StringUtil.TR("CommunityEXPRank", "Seasons");
		m_CommunityRankProgressBar.fillAmount = m_selectedSeason.currentPercentThroughCommunityRank;
		m_selectedChapterIndex = m_selectedSeason.CurrentChapter;
		SetupChapterButtonName(m_selectedChapterIndex);
		SetupChapterButtonSpriteController(seasonInfo);
		SeasonName.text = seasonInfo.SeasonName;
		SeasonEndTime.text = seasonInfo.SeasonEndTime;
		int num = -1;
		int num2 = -1;
		int num3 = 0;
		int num4 = 0;
		while (true)
		{
			if (num4 < seasonInfo.RepeatingRewards.Count)
			{
				if (num2 != seasonInfo.RepeatingRewards[num4].RepeatEveryXLevels)
				{
					if (num > -1)
					{
						for (int i = num3; i < m_rewardElements[num].m_rewardImages.Length; i++)
						{
							UIManager.SetGameObjectActive(m_rewardElements[num].m_rewardImages[i], false);
						}
					}
					num++;
					num2 = seasonInfo.RepeatingRewards[num4].RepeatEveryXLevels;
					num3 = 0;
					if (num >= m_rewardElements.Length)
					{
						break;
					}
					StaggerComponent.SetStaggerComponent(m_rewardElements[num].gameObject, true);
					if (seasonInfo.RepeatingRewards[num4].RepeatEveryXLevels == 1)
					{
						m_rewardElements[num].m_headerTitle.text = StringUtil.TR("EveryLevel", "Seasons");
					}
					else
					{
						m_rewardElements[num].m_headerTitle.text = string.Format(StringUtil.TR("EveryLevelNumber", "Seasons"), seasonInfo.RepeatingRewards[num4].RepeatEveryXLevels);
					}
				}
				UIManager.SetGameObjectActive(m_rewardElements[num].m_rewardImages[num3], true);
				m_rewardElements[num].Setup(num3, seasonInfo.RepeatingRewards[num4]);
				num3++;
				num4++;
				continue;
			}
			break;
		}
		num = 0;
		if (num >= 0)
		{
			if (num < m_rewardElements.Length)
			{
				for (int j = num3; j < m_rewardElements[num].m_rewardImages.Length; j++)
				{
					UIManager.SetGameObjectActive(m_rewardElements[num].m_rewardImages[j], false);
				}
			}
		}
		for (num++; num < m_rewardElements.Length; num++)
		{
			StaggerComponent.SetStaggerComponent(m_rewardElements[num].gameObject, false);
		}
		if (displayOverviewButton)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					SetSelectedHeaderButton(m_OverviewButton);
					return;
				}
			}
		}
		if (displayHighestChapter)
		{
			SetSelectedHeaderButton(m_currentChapterButton);
		}
		else
		{
			SetupChapter(m_selectedChapterIndex);
		}
		List<IDataEntry> itemList = m_selectedSeason.SeasonRewardEntries.ConvertAll<IDataEntry>(SeasonEntryToDataEntry);
		m_seasonsScrollList.Setup(itemList, Mathf.Max(m_selectedSeason.currentLevelDisplayIndex - 2, 0));
	}

	private void Setup(bool setOverheadBtn = false, bool displayHighestChapter = true, bool resetChapterText = true)
	{
		bool flag = false;
		if (displayInfo.Count == SeasonWideData.Get().m_seasons.Count)
		{
			int num = 0;
			while (true)
			{
				if (num < displayInfo.Count)
				{
					UIPlayerSeasonDisplayInfo uIPlayerSeasonDisplayInfo = new UIPlayerSeasonDisplayInfo();
					uIPlayerSeasonDisplayInfo.Setup(displayInfo[num].SeasonNumber, ClientGameManager.Get().GetPlayerAccountData());
					if (!uIPlayerSeasonDisplayInfo.Equals(displayInfo[num]))
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
		else
		{
			flag = true;
		}
		if (resetChapterText)
		{
			StartCoroutine(DoSetChapterTextAreaExpand(false));
		}
		if (!flag)
		{
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
		DisplaySeason(setOverheadBtn, displayHighestChapter);
	}

	private void DisplaySeason(bool setOverheadBtn = false, bool displayHighestChapter = true, int seasonIndex = -1)
	{
		for (int i = 0; i < displayInfo.Count; i++)
		{
			displayInfo[i].Clear();
		}
		displayInfo.Clear();
		PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
		if (seasonIndex < 0)
		{
			seasonIndex = playerAccountData.QuestComponent.ActiveSeason;
		}
		UIPlayerSeasonDisplayInfo seasonInfo = null;
		using (List<SeasonTemplate>.Enumerator enumerator = SeasonWideData.Get().m_seasons.GetEnumerator())
		{
			UIPlayerSeasonDisplayInfo uIPlayerSeasonDisplayInfo;
			for (; enumerator.MoveNext(); displayInfo.Add(uIPlayerSeasonDisplayInfo))
			{
				SeasonTemplate current = enumerator.Current;
				uIPlayerSeasonDisplayInfo = new UIPlayerSeasonDisplayInfo();
				uIPlayerSeasonDisplayInfo.Setup(current.Index, playerAccountData);
				if (current.Index != seasonIndex)
				{
					if (seasonIndex != 0)
					{
						continue;
					}
				}
				seasonInfo = uIPlayerSeasonDisplayInfo;
			}
		}
		SetSelectedSeason(seasonInfo, setOverheadBtn, displayHighestChapter);
	}

	private void RefreshRewardsEntries()
	{
		if (!(m_seasonsScrollList != null))
		{
			return;
		}
		while (true)
		{
			List<_LargeScrollList.ScrollListItemEntry> visibleListEntries = m_seasonsScrollList.GetVisibleListEntries();
			using (List<_LargeScrollList.ScrollListItemEntry>.Enumerator enumerator = visibleListEntries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					_LargeScrollList.ScrollListItemEntry current = enumerator.Current;
					if (current.m_theEntry != null)
					{
						UISeasonsRewardEntry component = current.m_theEntry.GetComponent<UISeasonsRewardEntry>();
						if (component != null)
						{
							component.RefreshDisplay();
						}
					}
				}
				while (true)
				{
					switch (4)
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

	public void SetVisible(bool visible, bool displayOverviewBtn = false, bool displayHighestChapter = true)
	{
		bool flag = m_isVisible != visible;
		m_isVisible = visible;
		UIManager.SetGameObjectActive(m_container, visible);
		UIManager.SetGameObjectActive(m_seasonsChapterBtnContainer, visible);
		UIManager.Get().SetSceneVisible(GetSceneType(), visible, new SceneVisibilityParameters());
		if (!visible)
		{
			return;
		}
		while (true)
		{
			UICharacterSelectScreenController.Get().SetVisible(false);
			Setup();
			bool flag2 = true;
			UIManager.SetGameObjectActive(m_lockedSeasonsContainer, !flag2);
			if (!flag)
			{
				return;
			}
			while (true)
			{
				if (!displayOverviewBtn)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
						{
							PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
							UIPlayerSeasonDisplayInfo uIPlayerSeasonDisplayInfo = null;
							int num = 0;
							while (true)
							{
								if (num >= displayInfo.Count)
								{
									break;
								}
								if (displayInfo[num].SeasonNumber == playerAccountData.QuestComponent.ActiveSeason)
								{
									uIPlayerSeasonDisplayInfo = displayInfo[num];
									break;
								}
								num++;
							}
							if (uIPlayerSeasonDisplayInfo != null)
							{
								SetSelectedHeaderButton(m_currentChapterButton);
							}
							return;
						}
						}
					}
				}
				SetSelectedHeaderButton(m_OverviewButton);
				return;
			}
		}
	}

	public void NotifyLoseFocus()
	{
		UIManager.SetGameObjectActive(base.gameObject, false);
		UIManager.SetGameObjectActive(m_seasonsChapterBtnContainer, false);
	}

	public void NotifyGetFocus()
	{
		UIManager.SetGameObjectActive(base.gameObject, true);
		UIManager.SetGameObjectActive(m_seasonsChapterBtnContainer, true);
	}

	public static bool CheckSeasonsVisibility(out SeasonLockoutReason lockoutReason)
	{
		bool result = false;
		if (GameManager.Get() != null && GameManager.Get().GameplayOverrides != null && GameManager.Get().GameplayOverrides.EnableSeasons)
		{
			result = true;
			lockoutReason = SeasonLockoutReason.None;
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().GetPlayerAccountData() != null)
				{
					int activeSeason = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
					SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(activeSeason);
					if (seasonTemplate != null)
					{
						if (seasonTemplate.IsTutorial)
						{
							result = false;
							lockoutReason = SeasonLockoutReason.InTutorialSeason;
						}
					}
					else
					{
						result = false;
						lockoutReason = SeasonLockoutReason.Disabled;
					}
				}
			}
		}
		else
		{
			lockoutReason = SeasonLockoutReason.Disabled;
		}
		return result;
	}

	private void OnSeasonCompleteNotification(SeasonStatusNotification notification)
	{
		DisplaySeason(false, true, notification.SeasonStartedIndex);
	}

	public static List<int> GetChapterQuests(SeasonChapter chapter, int seasonNumber, int chapterIndex)
	{
		if (chapter != null)
		{
			if (!chapter.NormalQuests.IsNullOrEmpty())
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return chapter.NormalQuests;
					}
				}
			}
		}
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager.SeasonChapterQuests != null)
		{
			if (clientGameManager.SeasonChapterQuests.TryGetValue(seasonNumber, out SeasonChapterQuests value))
			{
				if (value.m_chapterQuests != null)
				{
					if (value.m_chapterQuests.TryGetValue(chapterIndex + 1, out List<int> value2))
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								return value2;
							}
						}
					}
				}
			}
		}
		return new List<int>();
	}
}
