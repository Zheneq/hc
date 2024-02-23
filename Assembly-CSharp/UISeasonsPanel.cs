using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using I2.Loc;
using LobbyGameClientMessages;
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
		return this.m_selectedSeason;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.Seasons;
	}

	public override void Awake()
	{
		UISeasonsPanel.s_instance = this;
		base.Awake();
		this.m_scrollMasks = new List<Mask>();
		this.m_scrollMasks.Add(this.m_ChapterViewQuestChallengeScrollRect.GetComponent<Mask>());
		this.m_scrollMasks.Add(this.m_QuestChallengeScrollList.GetComponent<Mask>());
		this.m_scrollMasks.Add(this.m_seasonsScrollList.GetScrollRect().GetComponent<Mask>());
		UIManager.SetGameObjectActive(this.m_previousSeasonsBtn, LocalizationManager.CurrentLanguageCode != "zh", null);
		this.m_previousSeasonsBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PreviousSeasonsBtnClicked);
		this.m_OverviewButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.HeaderBtnClicked);
		this.m_factionsMoreInfoButton.callback = new _ButtonSwapSprite.ButtonClickCallback(this.FactionsMoreInfoClicked);
		UIManager.SetGameObjectActive(this.m_purchaseLevelsBtn, false, null);
		this.SetupChapterButtonName(0);
		this.endChapterExpandLocation = this.m_ChapterTextContainer.sizeDelta;
		this.m_ChapterTextClickArea.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ChapterTextAreaClicked);
		_MouseEventPasser mouseEventPasser = this.m_ChapterTextClickArea.gameObject.AddComponent<_MouseEventPasser>();
		mouseEventPasser.AddNewHandler(this.ChapterDescriptionContainer);
		mouseEventPasser = this.ChapterDescriptionContainer.verticalScrollbar.handleRect.gameObject.AddComponent<_MouseEventPasser>();
		mouseEventPasser.AddNewHandler(this.ChapterDescriptionContainer);
		this.displayInfo = new List<UIPlayerSeasonDisplayInfo>();
		this.m_questEntryList = new List<UISeasonsQuestEntry>();
		UIManager.SetGameObjectActive(this.m_seasonsChapterBtnContainer, false, null);
		UIManager.SetGameObjectActive(this.m_questsCompletedStamp, false, null);
		this.m_seasonsScrollList.GetScrollRect().scrollSensitivity = 150f;
		this.m_ChapterViewQuestChallengeScrollRect.scrollSensitivity = 125f;
		this.m_ChapterViewQuestChallengeScrollRect.elasticity = 0.01f;
		UIManager.SetGameObjectActive(this.m_bottomCommunityContainer, false, null);
		UIManager.SetGameObjectActive(this.m_topCommunityContainer, false, null);
		this.OnFactionCompetitionNotification(null);
		ClientGameManager.Get().OnAccountDataUpdated += this.OnAccountDataUpdated;
		ClientGameManager.Get().OnCharacterDataUpdated += this.OnCharacterDataUpdated;
		ClientGameManager.Get().OnSeasonChapterQuestsChange += this.OnSeasonChapterQuestsChange;
		ClientGameManager.Get().OnFactionCompetitionNotification += this.OnFactionCompetitionNotification;
		ClientGameManager.Get().OnPlayerFactionContributionChangeNotification += this.OnPlayerFactionContributionChangeNotification;
		ClientGameManager.Get().OnLobbyGameplayOverridesChange += this.OnLobbyGameplayOverridesChange;
		ClientGameManager.Get().OnSeasonCompleteNotification += this.OnSeasonCompleteNotification;
		if (HitchDetector.Get() != null)
		{
			HitchDetector.Get().AddNewLayoutGroup(this.m_repeatingGridContainer);
			HitchDetector.Get().AddNewLayoutGroup(this.m_QuestListContainer);
			HitchDetector.Get().AddNewLayoutGroup(this.m_seasonFactionList);
			HitchDetector.Get().AddNewLayoutGroup(this.m_seasonsChapterBtnContainer.GetComponentInChildren<LayoutGroup>(true));
		}
	}

	private void SetupChapterButtonName(int chapterIndex)
	{
		int index = chapterIndex;
		this.m_currentChapterButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.HeaderBtnClicked);
		this.m_currentChapterButton.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, (UITooltipBase tooltip) => this.HeaderBtnTooltipSetup(tooltip, index), null);
		TextMeshProUGUI[] componentsInChildren = this.m_currentChapterButton.GetComponentsInChildren<TextMeshProUGUI>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].text = string.Format(StringUtil.TR("SeasonChapterLabel", "Seasons"), (chapterIndex + 1).ToString());
		}
		Image[] componentsInChildren2 = this.m_currentChapterButton.GetComponentsInChildren<Image>(true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			if (componentsInChildren2[j].gameObject.name == "LockedIcon")
			{
				this.m_currentChapterLockIcon = componentsInChildren2[j];
				return;
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			return;
		}
	}

	private void SetupChapterButtonSpriteController(UIPlayerSeasonDisplayInfo seasonInfo)
	{
		bool setActive = false;
		if (this.m_selectedChapterIndex >= 0 && this.m_selectedChapterIndex < seasonInfo.ChapterEntries.Count)
		{
			if (!seasonInfo.ChapterEntries[this.m_selectedChapterIndex].IsChapterHidden)
			{
				setActive = true;
			}
			bool isChapterViewable = seasonInfo.ChapterEntries[this.m_selectedChapterIndex].IsChapterViewable;
			bool flag = seasonInfo.ChapterEntries[this.m_selectedChapterIndex].IsChapterLocked;
			if (flag && !seasonInfo.ChapterEntries[this.m_selectedChapterIndex].AreQuestsStatic)
			{
				if (ClientGameManager.Get() != null)
				{
					if (seasonInfo.ChapterEntries[this.m_selectedChapterIndex].StartDate < ClientGameManager.Get().PacificNow())
					{
						flag = false;
					}
				}
			}
			if (this.m_currentChapterLockIcon != null)
			{
				UIManager.SetGameObjectActive(this.m_currentChapterLockIcon, flag, null);
			}
			UIManager.SetGameObjectActive(this.m_currentChapterButton.spriteController.m_defaultImage, isChapterViewable, null);
			UIManager.SetGameObjectActive(this.m_currentChapterButton.spriteController.m_hoverImage, isChapterViewable, null);
			UIManager.SetGameObjectActive(this.m_currentChapterButton.spriteController.m_pressedImage, isChapterViewable, null);
			this.m_currentChapterButton.spriteController.SetClickable(isChapterViewable);
			this.m_currentChapterButton.spriteController.SetForceExitCallback(true);
			this.m_currentChapterButton.spriteController.SetForceHovercallback(true);
		}
		StaggerComponent.SetStaggerComponent(this.m_currentChapterButton.gameObject, setActive, true);
	}

	private void OnDestroy()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager == null)
		{
			return;
		}
		clientGameManager.OnAccountDataUpdated -= this.OnAccountDataUpdated;
		clientGameManager.OnCharacterDataUpdated -= this.OnCharacterDataUpdated;
		clientGameManager.OnSeasonChapterQuestsChange -= this.OnSeasonChapterQuestsChange;
		clientGameManager.OnFactionCompetitionNotification -= this.OnFactionCompetitionNotification;
		clientGameManager.OnPlayerFactionContributionChangeNotification -= this.OnPlayerFactionContributionChangeNotification;
		clientGameManager.OnLobbyGameplayOverridesChange -= this.OnLobbyGameplayOverridesChange;
		clientGameManager.OnSeasonCompleteNotification -= this.OnSeasonCompleteNotification;
	}

	private void OnAccountDataUpdated(PersistedAccountData newData)
	{
		this.Setup(false, false, false);
	}

	private void OnCharacterDataUpdated(PersistedCharacterData newData)
	{
		this.RefreshRewardsEntries();
	}

	private void OnSeasonChapterQuestsChange(Dictionary<int, SeasonChapterQuests> seasonChapterQuests)
	{
		this.Setup(false, false, false);
	}

	private void OnPlayerFactionContributionChangeNotification(PlayerFactionContributionChangeNotification notification)
	{
		this.UpdatePersonalContribution();
	}

	private void OnLobbyGameplayOverridesChange(LobbyGameplayOverrides gameplayOverrides)
	{
		this.OnFactionCompetitionNotification(null);
	}

	public void UpdatePersonalContribution()
	{
		UISeasonPanelViewEntry[] componentsInChildren = this.m_seasonFactionList.GetComponentsInChildren<UISeasonPanelViewEntry>(true);
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
					UIManager.SetGameObjectActive(this.m_topFactionsContainer, true, null);
					UIManager.SetGameObjectActive(this.m_bottomFactionsContainer, true, null);
					List<UISeasonPanelViewEntry> list = new List<UISeasonPanelViewEntry>();
					list.AddRange(this.m_seasonFactionList.GetComponentsInChildren<UISeasonPanelViewEntry>(true));
					for (int i = list.Count - 1; i >= factionCompetition.Factions.Count; i--)
					{
						UnityEngine.Object.Destroy(list[i].gameObject);
						list.RemoveAt(i);
					}
					for (int j = list.Count; j < factionCompetition.Factions.Count; j++)
					{
						UISeasonPanelViewEntry uiseasonPanelViewEntry = UnityEngine.Object.Instantiate<UISeasonPanelViewEntry>(this.m_seasonFactionPrefab);
						uiseasonPanelViewEntry.transform.SetParent(this.m_seasonFactionList.transform);
						uiseasonPanelViewEntry.transform.localScale = Vector3.one;
						uiseasonPanelViewEntry.transform.localPosition = Vector3.zero;
						list.Add(uiseasonPanelViewEntry);
					}
					List<UISeasonFactionPercentageBar> list2 = new List<UISeasonFactionPercentageBar>();
					list2.AddRange(this.m_topPercentBarcontainer.GetComponentsInChildren<UISeasonFactionPercentageBar>(true));
					for (int k = list2.Count - 1; k >= factionCompetition.Factions.Count; k--)
					{
						UnityEngine.Object.Destroy(list2[k].gameObject);
						list2.RemoveAt(k);
					}
					for (int l = list2.Count; l < factionCompetition.Factions.Count; l++)
					{
						UISeasonFactionPercentageBar uiseasonFactionPercentageBar = UnityEngine.Object.Instantiate<UISeasonFactionPercentageBar>(this.m_topPercentBarPrefab);
						uiseasonFactionPercentageBar.transform.SetParent(this.m_topPercentBarcontainer.transform);
						uiseasonFactionPercentageBar.transform.localScale = Vector3.one;
						uiseasonFactionPercentageBar.transform.localPosition = Vector3.zero;
						list2.Add(uiseasonFactionPercentageBar);
					}
					long num = 0L;
					for (int m = 0; m < factionCompetition.Factions.Count; m++)
					{
						long num2;
						dictionary.TryGetValue(m, out num2);
						list[m].Setup(factionCompetition.Factions[m], num2, m);
						num += num2;
					}
					float num3 = 0f;
					for (int n = 0; n < factionCompetition.Factions.Count; n++)
					{
						if (num == 0L)
						{
							UIManager.SetGameObjectActive(list2[n], false, null);
						}
						else
						{
							long num4;
							dictionary.TryGetValue(n, out num4);
							if (num4 > 0L)
							{
								float num5 = (float)num4 / (float)num;
								UIManager.SetGameObjectActive(list2[n], true, null);
								float[] rbga = FactionWideData.Get().GetRBGA(factionCompetition.Factions[n]);
								Color factionColor = new Color(rbga[0], rbga[1], rbga[2], rbga[3]);
								num3 = list2[n].Setup(num3, num5 + num3, factionColor);
							}
							else
							{
								UIManager.SetGameObjectActive(list2[n], true, null);
							}
						}
					}
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(this.m_topFactionsContainer, false, null);
		UIManager.SetGameObjectActive(this.m_bottomFactionsContainer, false, null);
	}

	public void PreviousSeasonsBtnClicked(BaseEventData data)
	{
		UILandingPageFullScreenMenus.Get().DisplayPreviousSeasonChapter();
	}

	public void ChapterTextAreaClicked(BaseEventData data)
	{
		this.ToggleChapterTextAreaExpand();
	}

	private void UpdateChapterText()
	{
		if (this.m_selectedSeason != null)
		{
			if (this.m_ChapterTextExpanded)
			{
				this.ChapterDescription.text = StringUtil.TR_SeasonStorytimeLongBody(this.m_selectedSeason.SeasonNumber, this.m_selectedChapterIndex + 1, 1);
				for (int i = 1; i < this.m_selectedSeason.ChapterEntries[this.m_selectedChapterIndex].SeasonChapterStory.Count; i++)
				{
					TextMeshProUGUI chapterDescription = this.ChapterDescription;
					chapterDescription.text += StringUtil.TR_SeasonStorytimeLongBody(this.m_selectedSeason.SeasonNumber, this.m_selectedChapterIndex + 1, i + 1);
				}
			}
			else
			{
				this.ChapterDescription.text = StringUtil.TR_SeasonStorytimeBody(this.m_selectedSeason.SeasonNumber, this.m_selectedChapterIndex + 1, 1);
			}
			RectTransform rectTransform = this.ChapterDescription.transform as RectTransform;
			float x = (this.ChapterDescription.transform as RectTransform).sizeDelta.x;
			float y = this.ChapterDescription.GetPreferredValues().y;
			float num;
			if (this.m_ChapterTextExpanded)
			{
				num = this.m_scrollHeightMax;
			}
			else
			{
				num = this.m_scrollHeightMin;
			}
			rectTransform.sizeDelta = new Vector2(x, y - num);
		}
	}

	private IEnumerator DoSetChapterTextAreaExpand(bool expanded)
	{
		this.m_ChapterTextExpanded = expanded;
		UIManager.SetGameObjectActive(this.m_ChapterClickToReadMoreText, !expanded, null);
		this.UpdateChapterText();
		yield return 0;
		this.startChapterExpandLocation = this.m_ChapterTextContainer.sizeDelta;
		this.ChapterExpandStartTime = Time.time;
		this.ChapterDescriptionContainer.verticalScrollbar.value = 1f;
		if (this.m_ChapterTextExpanded)
		{
			this.endChapterExpandLocation = new Vector2(this.m_ChapterTextContainer.sizeDelta.x, this.m_scrollHeightMax);
		}
		else
		{
			this.endChapterExpandLocation = new Vector2(this.m_ChapterTextContainer.sizeDelta.x, this.m_scrollHeightMin);
		}
		RectTransform content = this.ChapterDescriptionContainer.content;
		float x = this.ChapterDescriptionContainer.content.sizeDelta.x;
		float num = this.ChapterDescription.GetPreferredValues().y + this.ChapterName.GetPreferredValues().y + this.ChapterDescription.fontSize * 2f;
		float num2;
		if (this.m_ChapterTextExpanded)
		{
			num2 = this.m_scrollHeightMax;
		}
		else
		{
			num2 = this.m_scrollHeightMin;
		}
		content.sizeDelta = new Vector2(x, num - num2);
		this.ChapterExpandJourneyLength = Vector2.Distance(this.startChapterExpandLocation, this.endChapterExpandLocation);
		this.ChapterExpandSpeed = this.ChapterExpandJourneyLength * 4f;
		yield break;
	}

	private void ToggleChapterTextAreaExpand()
	{
		this.SetToggleChapterTextAreaExpand(!this.m_ChapterTextExpanded);
	}

	private void SetToggleChapterTextAreaExpand(bool expanded)
	{
		if (this.m_ChapterTextExpanded != expanded)
		{
			base.StartCoroutine(this.DoSetChapterTextAreaExpand(expanded));
		}
	}

	public void SelectChapter(int chapterNumber)
	{
		this.SetSelectedHeaderButton(this.m_currentChapterButton);
	}

	public void HeaderBtnClicked(BaseEventData data)
	{
		_SelectableBtn componentInParent = (data as PointerEventData).pointerCurrentRaycast.gameObject.GetComponentInParent<_SelectableBtn>();
		if (componentInParent != null)
		{
			this.SetSelectedHeaderButton(componentInParent);
		}
		this.SetToggleChapterTextAreaExpand(false);
	}

	private bool HeaderBtnTooltipSetup(UITooltipBase tooltip, int index)
	{
		if (index < this.m_selectedSeason.ChapterEntries.Count && this.m_selectedSeason.ChapterEntries[index].IsChapterLocked)
		{
			string text = string.Empty;
			if (index > 0)
			{
				if (!this.m_selectedSeason.ChapterEntries[index - 1].AreQuestsStatic)
				{
					return false;
				}
			}
			if (index > 0)
			{
				if (this.m_selectedSeason.ChapterEntries[index - 1].NumQuestsToAdvance != 0)
				{
					string arg = this.m_selectedSeason.ChapterEntries[index].NumQuestsToAdvance.ToString();
					string text2;
					if (this.m_selectedSeason.ChapterEntries[index].AreAllQuestsCompleteFromPreviousChapter)
					{
						text2 = string.Format(StringUtil.TR("UnlockChapterGreenDesc", "Seasons"), index, arg);
					}
					else
					{
						text2 = string.Format(StringUtil.TR("UnlockChapterRedDesc", "Seasons"), index, arg);
					}
					text = text2;
					goto IL_198;
				}
			}
			text = ((!this.m_selectedSeason.ChapterEntries[index].AreAllQuestsCompleteFromPreviousChapter) ? string.Format(StringUtil.TR("UnlockChapterAllRedDesc", "Seasons"), index) : string.Format(StringUtil.TR("UnlockChapterAllGreenDesc", "Seasons"), index));
			IL_198:
			bool flag = false;
			if (this.m_selectedSeason.ChapterEntries[index].AreOtherConditionsFromPreviousChapterMet)
			{
				flag = true;
			}
			if (!flag)
			{
				text = new StringBuilder().Append(text).Append("<color=red>").Append(StringUtil.TR_SeasonChapterUnlock(this.m_selectedSeason.SeasonNumber, index + 1)).Append("</color>").ToString();
			}
			(tooltip as UITitledTooltip).Setup(StringUtil.TR("ChapterIsLocked", "Seasons"), text, string.Empty);
			return true;
		}
		return false;
	}

	public void FactionsMoreInfoClicked(BaseEventData data)
	{
		UIFactionsIntroduction.Get().SetupIntro(null);
	}

	public static UISeasonsPanel Get()
	{
		return UISeasonsPanel.s_instance;
	}

	public bool IsVisible()
	{
		return this.m_isVisible;
	}

	private void Update()
	{
		if (this.m_ChapterTextContainer.sizeDelta != this.endChapterExpandLocation)
		{
			float num = (Time.time - this.ChapterExpandStartTime) * this.ChapterExpandSpeed;
			float t = num / this.ChapterExpandJourneyLength;
			this.m_ChapterTextContainer.sizeDelta = Vector2.Lerp(this.startChapterExpandLocation, this.endChapterExpandLocation, t);
			(this.ChapterDescriptionContainer.verticalScrollbar.transform as RectTransform).sizeDelta = new Vector2((this.ChapterDescriptionContainer.verticalScrollbar.transform as RectTransform).sizeDelta.x, this.m_ChapterTextContainer.sizeDelta.y - this.m_scrollTopOffset);
		}
	}

	private void SelectChapterStoryPage(UISeasonChapterEntry chapterInfo, int pageIndex)
	{
		if (pageIndex < chapterInfo.SeasonChapterStory.Count)
		{
			this.UpdateChapterText();
			this.ChapterName.text = StringUtil.TR_SeasonStorytimeHeader(this.m_selectedSeason.SeasonNumber, this.m_selectedChapterIndex + 1, pageIndex + 1);
			this.ChapterImage.sprite = (Resources.Load(chapterInfo.SeasonChapterStory[pageIndex].ImageFilename, typeof(Sprite)) as Sprite);
		}
		else
		{
			this.ChapterName.text = chapterInfo.SeasonChapterName;
			this.ChapterDescription.text = chapterInfo.SeasonChapterName;
		}
		UIManager.SetGameObjectActive(this.m_lockedSeasonChapterOverlay, chapterInfo.IsChapterLocked, null);
	}

	private void SetupChapter(int chapterIndex)
	{
		if (this.m_selectedSeason != null)
		{
			if (chapterIndex >= 0)
			{
				if (chapterIndex < this.m_selectedSeason.ChapterEntries.Count)
				{
					this.m_selectedChapterIndex = chapterIndex;
					UISeasonChapterEntry uiseasonChapterEntry = this.m_selectedSeason.ChapterEntries[chapterIndex];
					this.ChapterName.text = uiseasonChapterEntry.SeasonChapterName;
					this.SelectChapterStoryPage(uiseasonChapterEntry, 0);
					int i = 0;
					int num = 0;
					bool flag = false;
					for (int j = 0; j < uiseasonChapterEntry.QuestInfo.Count; j++)
					{
						if (j >= this.m_questEntryList.Count)
						{
							flag = true;
							UISeasonsQuestEntry uiseasonsQuestEntry = UnityEngine.Object.Instantiate<UISeasonsQuestEntry>(this.m_QuestListEntryPrefab);
							uiseasonsQuestEntry.transform.SetParent(this.m_QuestListContainer.transform);
							uiseasonsQuestEntry.transform.localScale = Vector3.one;
							uiseasonsQuestEntry.transform.localPosition = Vector3.zero;
							this.m_questEntryList.Add(uiseasonsQuestEntry);
						}
						StaggerComponent.SetStaggerComponent(this.m_questEntryList[j].gameObject, true, true);
						this.m_questEntryList[j].Setup(uiseasonChapterEntry.QuestInfo[j], uiseasonChapterEntry.IsChapterLocked);
						this.m_questEntryList[j].SetExpanded(false, false);
						this.m_questEntryList[j].SetMouseEventScroll(this.m_QuestChallengeScrollList);
						i = j;
						if (uiseasonChapterEntry.QuestInfo[j].Completed)
						{
							num++;
						}
					}
					if (flag)
					{
						if (HitchDetector.Get() != null)
						{
							HitchDetector.Get().AddNewLayoutGroup(this.m_QuestListContainer);
						}
					}
					for (i++; i < this.m_questEntryList.Count; i++)
					{
						StaggerComponent.SetStaggerComponent(this.m_questEntryList[i].gameObject, false, true);
					}
					this.m_questHeaderTitle.text = string.Format(StringUtil.TR("ChapterContracts", "Seasons"), num, uiseasonChapterEntry.QuestInfo.Count);
					if (num == uiseasonChapterEntry.QuestInfo.Count)
					{
						UIManager.SetGameObjectActive(this.m_questsCompletedStamp, true, null);
						UIManager.SetGameObjectActive(this.m_questsEndedStamp, false, null);
					}
					else if (!uiseasonChapterEntry.AreQuestsStatic)
					{
						if (ClientGameManager.Get().PacificNow() < uiseasonChapterEntry.EndDate)
						{
							string arg = string.Format(StringUtil.TR("DayMonthYear", "Global"), uiseasonChapterEntry.EndDate.Day, StringUtil.TR(new StringBuilder().Append("Month").Append(uiseasonChapterEntry.EndDate.Month).ToString(), "Global"), uiseasonChapterEntry.EndDate.Year);
							string str = string.Format(StringUtil.TR("MustBeCompletedBy", "Global"), arg);
							TextMeshProUGUI questHeaderTitle = this.m_questHeaderTitle;
							questHeaderTitle.text = new StringBuilder().Append(questHeaderTitle.text).Append(" <color=red>").Append(str).Append("</color>").ToString();
							UIManager.SetGameObjectActive(this.m_questsEndedStamp, false, null);
						}
						else
						{
							UIManager.SetGameObjectActive(this.m_questsEndedStamp, true, null);
						}
						UIManager.SetGameObjectActive(this.m_questsCompletedStamp, false, null);
					}
					else
					{
						UIManager.SetGameObjectActive(this.m_questsCompletedStamp, false, null);
						UIManager.SetGameObjectActive(this.m_questsEndedStamp, false, null);
					}
					int num2 = 0;
					using (List<QuestItemReward>.Enumerator enumerator = uiseasonChapterEntry.ItemRewards.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							QuestItemReward questItemReward = enumerator.Current;
							InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(questItemReward.ItemTemplateId);
							this.m_CompletedChapterRewards[num2].SetupHack(itemTemplate, itemTemplate.IconPath, questItemReward.Amount);
							num2++;
						}
					}
					using (List<QuestCurrencyReward>.Enumerator enumerator2 = uiseasonChapterEntry.CurrencyRewards.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							QuestCurrencyReward currencyReward = enumerator2.Current;
							this.m_CompletedChapterRewards[num2].Setup(currencyReward, 0);
							num2++;
						}
					}
					using (List<QuestUnlockReward>.Enumerator enumerator3 = uiseasonChapterEntry.UnlockRewards.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							QuestUnlockReward questUnlockReward = enumerator3.Current;
							this.m_CompletedChapterRewards[num2].SetupHack(questUnlockReward.resourceString, 0);
							num2++;
						}
					}
					if (num2 > 0)
					{
						UIManager.SetGameObjectActive(this.m_ChapterRewardsContainer, true, null);
						for (int k = 0; k < this.m_CompletedChapterRewards.Length; k++)
						{
							UIManager.SetGameObjectActive(this.m_CompletedChapterRewards[k], k < num2, null);
						}
					}
					else
					{
						UIManager.SetGameObjectActive(this.m_ChapterRewardsContainer, false, null);
					}
					this.m_ChapterViewQuestChallengeScrollRect.verticalScrollbar.value = 1f;
					return;
				}
			}
		}
	}

	private void SetSelectedHeaderButton(_SelectableBtn selectedHeaderBtn)
	{
		bool flag = selectedHeaderBtn == this.m_OverviewButton;
		int selectedChapterIndex = this.m_selectedChapterIndex;
		this.m_OverviewButton.SetSelected(flag, false, string.Empty, string.Empty);
		UIManager.SetGameObjectActive(this.m_ChapterContainer, !flag, null);
		UIManager.SetGameObjectActive(this.m_OverviewContainer, flag, null);
		if (flag)
		{
			this.UpdatePersonalContribution();
			if (this.m_selectedSeason != null)
			{
				List<IDataEntry> itemList = this.m_selectedSeason.SeasonRewardEntries.ConvertAll<IDataEntry>(new Converter<UISeasonRewardEntry, IDataEntry>(UISeasonsPanel.SeasonEntryToDataEntry));
				this.m_seasonsScrollList.Setup(itemList, Mathf.Max(this.m_selectedSeason.currentLevelDisplayIndex - 2, 0));
			}
			else
			{
				Log.Warning("Failed to setup season scroll list, current season is null (possibly no season is active?)", new object[0]);
			}
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_ChapterPageBtnContainer, false, null);
			this.SetupChapter(selectedChapterIndex);
		}
		foreach (Mask mask in this.m_scrollMasks)
		{
			mask.enabled = false;
			mask.enabled = true;
		}
	}

	public static IDataEntry SeasonEntryToDataEntry(UISeasonRewardEntry entry)
	{
		return entry;
	}

	private void SetSelectedSeason(UIPlayerSeasonDisplayInfo seasonInfo, bool displayOverviewButton, bool displayHighestChapter = true)
	{
		this.m_selectedSeason = seasonInfo;
		this.m_PlayerCurrentSeasonLevelProgress.fillAmount = this.m_selectedSeason.currentPercentThroughPlayerSeasonLevel;
		this.m_PlayerSeasonLevelLabel.text = string.Format(StringUtil.TR("SeasonLevelNumber", "Seasons"), this.m_selectedSeason.PlayerSeasonLevel.ToString());
		this.m_PlayerSeasonEXPLevelLabel.text = string.Format(StringUtil.TR("SeasonEXPNumber", "Seasons"), this.m_selectedSeason.currentXPThroughPlayerLevel, SeasonWideData.Get().GetSeasonExperience(seasonInfo.SeasonNumber, seasonInfo.PlayerSeasonLevel));
		this.m_CommunityRankLabel.text = StringUtil.TR("CommunityRank", "Seasons");
		this.m_CommunityEXPRankLabel.text = StringUtil.TR("CommunityEXPRank", "Seasons");
		this.m_CommunityRankProgressBar.fillAmount = this.m_selectedSeason.currentPercentThroughCommunityRank;
		this.m_selectedChapterIndex = this.m_selectedSeason.CurrentChapter;
		this.SetupChapterButtonName(this.m_selectedChapterIndex);
		this.SetupChapterButtonSpriteController(seasonInfo);
		this.SeasonName.text = seasonInfo.SeasonName;
		this.SeasonEndTime.text = seasonInfo.SeasonEndTime;
		int i = -1;
		int num = -1;
		int num2 = 0;
		for (int j = 0; j < seasonInfo.RepeatingRewards.Count; j++)
		{
			if (num != seasonInfo.RepeatingRewards[j].RepeatEveryXLevels)
			{
				if (i > -1)
				{
					for (int k = num2; k < this.m_rewardElements[i].m_rewardImages.Length; k++)
					{
						UIManager.SetGameObjectActive(this.m_rewardElements[i].m_rewardImages[k], false, null);
					}
				}
				i++;
				num = seasonInfo.RepeatingRewards[j].RepeatEveryXLevels;
				num2 = 0;
				if (i >= this.m_rewardElements.Length)
				{
					break;
				}
				StaggerComponent.SetStaggerComponent(this.m_rewardElements[i].gameObject, true, true);
				if (seasonInfo.RepeatingRewards[j].RepeatEveryXLevels == 1)
				{
					this.m_rewardElements[i].m_headerTitle.text = StringUtil.TR("EveryLevel", "Seasons");
				}
				else
				{
					this.m_rewardElements[i].m_headerTitle.text = string.Format(StringUtil.TR("EveryLevelNumber", "Seasons"), seasonInfo.RepeatingRewards[j].RepeatEveryXLevels);
				}
			}
			UIManager.SetGameObjectActive(this.m_rewardElements[i].m_rewardImages[num2], true, null);
			this.m_rewardElements[i].Setup(num2, seasonInfo.RepeatingRewards[j]);
			num2++;
		}
		i = 0;
		if (i >= 0)
		{
			if (i < this.m_rewardElements.Length)
			{
				for (int l = num2; l < this.m_rewardElements[i].m_rewardImages.Length; l++)
				{
					UIManager.SetGameObjectActive(this.m_rewardElements[i].m_rewardImages[l], false, null);
				}
			}
		}
		for (i++; i < this.m_rewardElements.Length; i++)
		{
			StaggerComponent.SetStaggerComponent(this.m_rewardElements[i].gameObject, false, true);
		}
		if (displayOverviewButton)
		{
			this.SetSelectedHeaderButton(this.m_OverviewButton);
		}
		else
		{
			if (displayHighestChapter)
			{
				this.SetSelectedHeaderButton(this.m_currentChapterButton);
			}
			else
			{
				this.SetupChapter(this.m_selectedChapterIndex);
			}
			List<IDataEntry> itemList = this.m_selectedSeason.SeasonRewardEntries.ConvertAll<IDataEntry>(new Converter<UISeasonRewardEntry, IDataEntry>(UISeasonsPanel.SeasonEntryToDataEntry));
			this.m_seasonsScrollList.Setup(itemList, Mathf.Max(this.m_selectedSeason.currentLevelDisplayIndex - 2, 0));
		}
		return;
	}

	private void Setup(bool setOverheadBtn = false, bool displayHighestChapter = true, bool resetChapterText = true)
	{
		bool flag = false;
		if (this.displayInfo.Count == SeasonWideData.Get().m_seasons.Count)
		{
			for (int i = 0; i < this.displayInfo.Count; i++)
			{
				UIPlayerSeasonDisplayInfo uiplayerSeasonDisplayInfo = new UIPlayerSeasonDisplayInfo();
				uiplayerSeasonDisplayInfo.Setup(this.displayInfo[i].SeasonNumber, ClientGameManager.Get().GetPlayerAccountData());
				if (!uiplayerSeasonDisplayInfo.Equals(this.displayInfo[i]))
				{
					flag = true;
					goto IL_AF;
				}
			}
		}
		else
		{
			flag = true;
		}
		IL_AF:
		if (resetChapterText)
		{
			base.StartCoroutine(this.DoSetChapterTextAreaExpand(false));
		}
		if (!flag)
		{
			return;
		}
		this.DisplaySeason(setOverheadBtn, displayHighestChapter, -1);
	}

	private void DisplaySeason(bool setOverheadBtn = false, bool displayHighestChapter = true, int seasonIndex = -1)
	{
		for (int i = 0; i < this.displayInfo.Count; i++)
		{
			this.displayInfo[i].Clear();
		}
		this.displayInfo.Clear();
		PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
		if (seasonIndex < 0)
		{
			seasonIndex = playerAccountData.QuestComponent.ActiveSeason;
		}
		UIPlayerSeasonDisplayInfo seasonInfo = null;
		using (List<SeasonTemplate>.Enumerator enumerator = SeasonWideData.Get().m_seasons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SeasonTemplate seasonTemplate = enumerator.Current;
				UIPlayerSeasonDisplayInfo uiplayerSeasonDisplayInfo = new UIPlayerSeasonDisplayInfo();
				uiplayerSeasonDisplayInfo.Setup(seasonTemplate.Index, playerAccountData);
				if (seasonTemplate.Index == seasonIndex)
				{
					goto IL_C1;
				}
				if (seasonIndex == 0)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						goto IL_C1;
					}
				}
				IL_C4:
				this.displayInfo.Add(uiplayerSeasonDisplayInfo);
				continue;
				IL_C1:
				seasonInfo = uiplayerSeasonDisplayInfo;
				goto IL_C4;
			}
		}
		this.SetSelectedSeason(seasonInfo, setOverheadBtn, displayHighestChapter);
	}

	private void RefreshRewardsEntries()
	{
		if (this.m_seasonsScrollList != null)
		{
			List<_LargeScrollList.ScrollListItemEntry> visibleListEntries = this.m_seasonsScrollList.GetVisibleListEntries();
			using (List<_LargeScrollList.ScrollListItemEntry>.Enumerator enumerator = visibleListEntries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					_LargeScrollList.ScrollListItemEntry scrollListItemEntry = enumerator.Current;
					if (scrollListItemEntry.m_theEntry != null)
					{
						UISeasonsRewardEntry component = scrollListItemEntry.m_theEntry.GetComponent<UISeasonsRewardEntry>();
						if (component != null)
						{
							component.RefreshDisplay();
						}
					}
				}
			}
		}
	}

	public void SetVisible(bool visible, bool displayOverviewBtn = false, bool displayHighestChapter = true)
	{
		bool flag = this.m_isVisible != visible;
		this.m_isVisible = visible;
		UIManager.SetGameObjectActive(this.m_container, visible, null);
		UIManager.SetGameObjectActive(this.m_seasonsChapterBtnContainer, visible, null);
		UIManager.Get().SetSceneVisible(this.GetSceneType(), visible, new SceneVisibilityParameters());
		if (visible)
		{
			UICharacterSelectScreenController.Get().SetVisible(false, false);
			this.Setup(false, true, true);
			bool flag2 = true;
			UIManager.SetGameObjectActive(this.m_lockedSeasonsContainer, !flag2, null);
			if (flag)
			{
				if (!displayOverviewBtn)
				{
					PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
					UIPlayerSeasonDisplayInfo uiplayerSeasonDisplayInfo = null;
					for (int i = 0; i < this.displayInfo.Count; i++)
					{
						if (this.displayInfo[i].SeasonNumber == playerAccountData.QuestComponent.ActiveSeason)
						{
							uiplayerSeasonDisplayInfo = this.displayInfo[i];
							break;
						}
					}
					if (uiplayerSeasonDisplayInfo != null)
					{
						this.SetSelectedHeaderButton(this.m_currentChapterButton);
					}
					return;
				}
				else
				{
					this.SetSelectedHeaderButton(this.m_OverviewButton);
				}
			}
		}
	}

	public void NotifyLoseFocus()
	{
		UIManager.SetGameObjectActive(base.gameObject, false, null);
		UIManager.SetGameObjectActive(this.m_seasonsChapterBtnContainer, false, null);
	}

	public void NotifyGetFocus()
	{
		UIManager.SetGameObjectActive(base.gameObject, true, null);
		UIManager.SetGameObjectActive(this.m_seasonsChapterBtnContainer, true, null);
	}

	public unsafe static bool CheckSeasonsVisibility(out SeasonLockoutReason lockoutReason)
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
		this.DisplaySeason(false, true, notification.SeasonStartedIndex);
	}

	public static List<int> GetChapterQuests(SeasonChapter chapter, int seasonNumber, int chapterIndex)
	{
		if (chapter != null)
		{
			if (!chapter.NormalQuests.IsNullOrEmpty<int>())
			{
				return chapter.NormalQuests;
			}
		}
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager.SeasonChapterQuests != null)
		{
			SeasonChapterQuests seasonChapterQuests;
			if (clientGameManager.SeasonChapterQuests.TryGetValue(seasonNumber, out seasonChapterQuests))
			{
				if (seasonChapterQuests.m_chapterQuests != null)
				{
					List<int> result;
					if (seasonChapterQuests.m_chapterQuests.TryGetValue(chapterIndex + 1, out result))
					{
						return result;
					}
				}
			}
		}
		return new List<int>();
	}
}
