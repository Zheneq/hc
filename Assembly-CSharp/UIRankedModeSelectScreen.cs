using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRankedModeSelectScreen : MonoBehaviour
{
	public RectTransform m_selectScreenContainer;

	public UIPlayerProfileRankDisplay[] m_rankDisplays;

	public TextMeshProUGUI[] m_queueButtonLabels;

	public _SelectableBtn m_startQueueBtn;

	public _SelectableBtn m_soloRankTabButton;

	public _SelectableBtn m_duoRankTabButton;

	public _SelectableBtn m_teamRankTabButton;

	public _SelectableBtn m_rewardTabButton;

	public RectTransform m_leaderboardContainer;

	public RectTransform m_rewardContainer;

	public _SelectableBtn m_filterListDropdownBtn;

	public RectTransform m_filterListDropdown;

	public _LargeScrollList m_rankList;

	public _SelectableBtn m_RankDropdownDivision;

	public _SelectableBtn m_RankDropdownFriends;

	public _SelectableBtn m_RankDropdownTopPlayers;

	public TextMeshProUGUI m_InstanceLabel;

	public RectTransform m_emptyRankList;

	public TextMeshProUGUI m_emptyRankListText;

	public RectTransform m_loadingRankList;

	public TextMeshProUGUI m_rankRewardDisabledNotice;

	public RectTransform m_streakContainer;

	public TextMeshProUGUI m_StreakLabel;

	public RectTransform m_LockedRankedModeContainer;

	public RectTransform m_UnlockedRankedModeContainer;

	public TextMeshProUGUI m_UnlockText;

	public Image m_UnlockFillBar;

	private static Dictionary<int, PerGroupSizeTierInfo> s_tierInfoPerGroupSize;

	private static UIRankedModeSelectScreen s_instance;

	private UIRankDisplayType m_selectedQueueType;

	private TextMeshProUGUI[] m_dropdownBtnTextLabels;

	private TextMeshProUGUI[] m_dropdownDivisionTextLabels;

	private bool filterDropdownOpen;

	private bool m_isVisible;

	private UIRankDisplayType m_selectedViewTab;

	private int m_selectedRank;

	private UIRankedModeSelectScreen.RankedQueueRejectionReasons m_cannotQueue;

	private bool m_loadedData;

	private int m_ourTier = -1;

	private int m_ourDivisionId = -1;

	public static Dictionary<int, PerGroupSizeTierInfo> TierInfoPerGroupSize
	{
		get
		{
			return UIRankedModeSelectScreen.s_tierInfoPerGroupSize;
		}
	}

	internal static UIRankedModeSelectScreen Get()
	{
		return UIRankedModeSelectScreen.s_instance;
	}

	private void Awake()
	{
		this.m_dropdownBtnTextLabels = this.m_filterListDropdownBtn.GetComponentsInChildren<TextMeshProUGUI>(true);
		this.m_dropdownDivisionTextLabels = this.m_RankDropdownDivision.GetComponentsInChildren<TextMeshProUGUI>(true);
		UIRankedModeSelectScreen.s_instance = this;
		this.m_startQueueBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.StartQueueBtnClicked);
		this.m_filterListDropdownBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.DropdownClicked);
		this.m_filterListDropdownBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.RankDropdownClick;
		this.m_soloRankTabButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.TabClicked);
		this.m_duoRankTabButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.TabClicked);
		this.m_teamRankTabButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.TabClicked);
		this.m_rewardTabButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.TabClicked);
		this.m_soloRankTabButton.spriteController.m_soundToPlay = FrontEndButtonSounds.RankTabClick;
		this.m_duoRankTabButton.spriteController.m_soundToPlay = FrontEndButtonSounds.RankTabClick;
		this.m_teamRankTabButton.spriteController.m_soundToPlay = FrontEndButtonSounds.RankTabClick;
		this.m_rewardTabButton.spriteController.m_soundToPlay = FrontEndButtonSounds.RankTabClick;
		this.m_RankDropdownDivision.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.RankListClicked);
		this.m_RankDropdownFriends.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.RankListClicked);
		this.m_RankDropdownTopPlayers.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.RankListClicked);
		this.m_RankDropdownDivision.spriteController.m_soundToPlay = FrontEndButtonSounds.RankDropdownSelect;
		this.m_RankDropdownFriends.spriteController.m_soundToPlay = FrontEndButtonSounds.RankDropdownSelect;
		this.m_RankDropdownTopPlayers.spriteController.m_soundToPlay = FrontEndButtonSounds.RankDropdownSelect;
		this.m_startQueueBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.QueueButtonTooltipSetup), null);
		UIManager.SetGameObjectActive(this.m_rewardContainer, false, null);
		this.DoFilterDropdownVisible(false);
		this.SetDropdownDivisionText(StringUtil.TR("MyInstance", "RankMode"));
		this.SetDropdownText(StringUtil.TR("MyInstance", "RankMode"));
		ClientGameManager.Get().OnGroupUpdateNotification += this.HandleGroupUpdateNotification;
		this.m_rankList.GetScrollRect().movementType = ScrollRect.MovementType.Clamped;
		CanvasGroup component = this.m_rankList.GetComponent<CanvasGroup>();
		if (component != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.Awake()).MethodHandle;
			}
			component.blocksRaycasts = true;
			component.interactable = true;
		}
		this.m_loadedData = false;
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.OnDestroy()).MethodHandle;
			}
			ClientGameManager.Get().OnGroupUpdateNotification -= this.HandleGroupUpdateNotification;
		}
		UIRankedModeSelectScreen.s_instance = null;
	}

	private void SetDropdownText(string text)
	{
		for (int i = 0; i < this.m_dropdownBtnTextLabels.Length; i++)
		{
			this.m_dropdownBtnTextLabels[i].text = text;
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.SetDropdownText(string)).MethodHandle;
		}
	}

	private void SetDropdownDivisionText(string text)
	{
		for (int i = 0; i < this.m_dropdownDivisionTextLabels.Length; i++)
		{
			this.m_dropdownDivisionTextLabels[i].text = text;
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.SetDropdownDivisionText(string)).MethodHandle;
		}
	}

	private void SelectRankFilter(int rank)
	{
		this.m_selectedRank = rank;
		int num;
		if (this.m_selectedViewTab == UIRankDisplayType.Solo)
		{
			num = 1;
		}
		else if (this.m_selectedViewTab == UIRankDisplayType.Duo)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.SelectRankFilter(int)).MethodHandle;
			}
			num = 2;
		}
		else
		{
			num = 4;
		}
		int num2 = num;
		if (this.m_selectedRank == 2)
		{
			this.RequestSetupFromLobby(GameType.Ranked, num2, RankedLeaderboardSpecificRequest.RequestSpecificationType.\u0012);
			this.m_InstanceLabel.text = string.Empty;
			UIManager.SetGameObjectActive(this.m_streakContainer, false, null);
		}
		else if (this.m_selectedRank == 0)
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
			if (UIRankedModeSelectScreen.TierInfoPerGroupSize != null)
			{
				if (UIRankedModeSelectScreen.TierInfoPerGroupSize.ContainsKey(num2))
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
					if (UIRankedModeSelectScreen.TierInfoPerGroupSize[num2].OurEntry != null)
					{
						this.m_ourTier = UIRankedModeSelectScreen.TierInfoPerGroupSize[num2].OurEntry.Value.Tier;
						this.m_ourDivisionId = UIRankedModeSelectScreen.TierInfoPerGroupSize[num2].OurEntry.Value.InstanceId;
						string text;
						string text2;
						this.GetTierLocalizedName(this.m_ourTier, this.m_ourDivisionId, num2, out text, out text2);
						this.m_InstanceLabel.text = text2;
						int winStreak = UIRankedModeSelectScreen.TierInfoPerGroupSize[num2].OurEntry.Value.WinStreak;
						if (winStreak != 0)
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
							UIManager.SetGameObjectActive(this.m_streakContainer, false, null);
							if (winStreak > 0)
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
								if (winStreak > 1)
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
									this.m_StreakLabel.text = string.Format(StringUtil.TR("RankedWinStreak", "RankMode"), winStreak.ToString());
								}
								else
								{
									this.m_StreakLabel.text = StringUtil.TR("RankedOneWinStreak", "RankMode");
								}
							}
							else if (winStreak < -1)
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
								this.m_StreakLabel.text = string.Format(StringUtil.TR("RankedLossStreak", "RankMode"), Mathf.Abs(winStreak).ToString());
							}
							else
							{
								this.m_StreakLabel.text = StringUtil.TR("RankedOneLossStreak", "RankMode");
							}
						}
						else
						{
							UIManager.SetGameObjectActive(this.m_streakContainer, false, null);
						}
						this.RequestSetupFromLobby(GameType.Ranked, num2, RankedLeaderboardSpecificRequest.RequestSpecificationType.\u001D);
					}
					else
					{
						this.DisplayEmptyList(true, UIRankedModeSelectScreen.EmptyListReasons.NeedToPlayMoreGames, null);
					}
				}
				else
				{
					this.DisplayEmptyList(true, UIRankedModeSelectScreen.EmptyListReasons.QueueIsDisabled, null);
				}
			}
			else
			{
				this.DisplayEmptyList(true, UIRankedModeSelectScreen.EmptyListReasons.FailedToReceiveTeirInfo, null);
			}
		}
		else
		{
			this.RequestSetupFromLobby(GameType.Ranked, num2, RankedLeaderboardSpecificRequest.RequestSpecificationType.\u000E);
			this.m_InstanceLabel.text = string.Empty;
			UIManager.SetGameObjectActive(this.m_streakContainer, false, null);
		}
		this.m_RankDropdownDivision.SetSelected(this.m_selectedRank == 0, false, string.Empty, string.Empty);
		this.m_RankDropdownFriends.SetSelected(this.m_selectedRank == 1, false, string.Empty, string.Empty);
		this.m_RankDropdownTopPlayers.SetSelected(this.m_selectedRank == 2, false, string.Empty, string.Empty);
		this.m_RankDropdownDivision.spriteController.SetClickable(this.m_selectedRank != 0);
		this.m_RankDropdownFriends.spriteController.SetClickable(this.m_selectedRank != 1);
		this.m_RankDropdownTopPlayers.spriteController.SetClickable(this.m_selectedRank != 2);
		if (this.m_selectedRank == 0)
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
			this.SetDropdownText(StringUtil.TR("MyInstance", "RankMode"));
		}
		else if (this.m_selectedRank == 1)
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
			this.SetDropdownText(StringUtil.TR("Friends", "Global"));
		}
		else if (this.m_selectedRank == 2)
		{
			this.SetDropdownText(StringUtil.TR("TopPlayers", "RankMode"));
		}
		this.SetFilterDropdownVisible(false);
	}

	public void RankListClicked(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		if (pointerEventData.selectedObject == this.m_RankDropdownDivision.spriteController.gameObject)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.RankListClicked(BaseEventData)).MethodHandle;
			}
			this.SelectRankFilter(0);
		}
		else if (pointerEventData.selectedObject == this.m_RankDropdownFriends.spriteController.gameObject)
		{
			this.SelectRankFilter(1);
		}
		else if (pointerEventData.selectedObject == this.m_RankDropdownTopPlayers.spriteController.gameObject)
		{
			this.SelectRankFilter(2);
		}
	}

	private void DoFilterDropdownVisible(bool visible)
	{
		this.filterDropdownOpen = visible;
		UIManager.SetGameObjectActive(this.m_filterListDropdown, visible, null);
		this.m_filterListDropdownBtn.SetSelected(visible, false, string.Empty, string.Empty);
		if (!visible)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.DoFilterDropdownVisible(bool)).MethodHandle;
			}
			this.m_RankDropdownDivision.spriteController.ForceSetPointerEntered(false);
			this.m_RankDropdownFriends.spriteController.ForceSetPointerEntered(false);
			this.m_RankDropdownTopPlayers.spriteController.ForceSetPointerEntered(false);
		}
	}

	public void SetFilterDropdownVisible(bool visible)
	{
		if (this.filterDropdownOpen != visible)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.SetFilterDropdownVisible(bool)).MethodHandle;
			}
			this.DoFilterDropdownVisible(visible);
		}
	}

	public void ToggleFilterDropdown()
	{
		this.SetFilterDropdownVisible(!this.filterDropdownOpen);
	}

	public void DropdownClicked(BaseEventData data)
	{
		this.ToggleFilterDropdown();
	}

	public List<IDataEntry> GetRankListDisplayInfo(List<RankedScoreboardEntry> entries, int groupSize)
	{
		return this.GetRankingEntries(entries, groupSize).ConvertAll<IDataEntry>(new Converter<UIRankingDisplayEntry, IDataEntry>(UIRankedModeSelectScreen.RankingEntryToDataEntry));
	}

	private int ConvertDisplayTypeToGroupSize(UIRankDisplayType type)
	{
		int result = 0;
		if (this.m_selectedQueueType == UIRankDisplayType.Solo)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.ConvertDisplayTypeToGroupSize(UIRankDisplayType)).MethodHandle;
			}
			result = 1;
		}
		else if (this.m_selectedQueueType == UIRankDisplayType.Duo)
		{
			result = 2;
		}
		else if (this.m_selectedQueueType == UIRankDisplayType.FullTeam)
		{
			result = 4;
		}
		return result;
	}

	private bool ServerAllowQueueRank()
	{
		using (Dictionary<GameType, GameTypeAvailability>.Enumerator enumerator = ClientGameManager.Get().GameTypeAvailabilies.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<GameType, GameTypeAvailability> keyValuePair = enumerator.Current;
				if (keyValuePair.Key == GameType.Ranked)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.ServerAllowQueueRank()).MethodHandle;
					}
					if (keyValuePair.Value.IsActive)
					{
						return true;
					}
					return false;
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}

	private bool IsQueueTypeValidWithGroupSize()
	{
		using (Dictionary<GameType, GameTypeAvailability>.Enumerator enumerator = ClientGameManager.Get().GameTypeAvailabilies.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<GameType, GameTypeAvailability> keyValuePair = enumerator.Current;
				if (keyValuePair.Key == GameType.Ranked)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.IsQueueTypeValidWithGroupSize()).MethodHandle;
					}
					int key = this.ConvertDisplayTypeToGroupSize(this.m_selectedQueueType);
					if (!keyValuePair.Value.QueueableGroupSizes.IsNullOrEmpty<KeyValuePair<int, RequirementCollection>>())
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
						if (keyValuePair.Value.QueueableGroupSizes.ContainsKey(key))
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
							return true;
						}
					}
					return false;
				}
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}

	private void DisplayEmptyList(bool displayEmptyList, UIRankedModeSelectScreen.EmptyListReasons reason = UIRankedModeSelectScreen.EmptyListReasons.None, LocalizationPayload localizedReason = null)
	{
		UIManager.SetGameObjectActive(this.m_emptyRankList, displayEmptyList, null);
		UIManager.SetGameObjectActive(this.m_rankList, !displayEmptyList, null);
		if (this.m_emptyRankListText == null)
		{
			this.m_emptyRankListText = this.m_emptyRankList.GetComponentInChildren<TextMeshProUGUI>(true);
		}
		if (localizedReason != null)
		{
			this.m_emptyRankListText.text = localizedReason.ToString();
		}
		else if (reason == UIRankedModeSelectScreen.EmptyListReasons.None)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.DisplayEmptyList(bool, UIRankedModeSelectScreen.EmptyListReasons, LocalizationPayload)).MethodHandle;
			}
			if (this.m_emptyRankListText != null)
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
				if (this.m_selectedRank == 2)
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
					this.m_emptyRankListText.text = StringUtil.TR("NoPlayersRankedHighEnough", "RankMode");
				}
				else
				{
					this.m_emptyRankListText.text = StringUtil.TR("PlayMoreToGetRanked", "RankMode");
				}
			}
		}
		else if (reason == UIRankedModeSelectScreen.EmptyListReasons.FailedToReceiveTeirInfo)
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
			this.m_emptyRankListText.text = StringUtil.TR("FailedToRecieveRankData", "RankMode");
		}
		else if (reason == UIRankedModeSelectScreen.EmptyListReasons.QueueIsDisabled)
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
			this.m_emptyRankListText.text = StringUtil.TR("QueSizeDisabled", "RankMode");
		}
		else if (reason == UIRankedModeSelectScreen.EmptyListReasons.NeedToPlayMoreGames)
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
			this.m_emptyRankListText.text = StringUtil.TR("PlayMoreToGetRanked", "RankMode");
		}
	}

	public void UpdateUnlockStatus()
	{
		QueueBlockOutReasonDetails queueBlockOutReasonDetails;
		ClientGameManager.Get().GetBlockingQueueRestriction(GameType.Ranked, out queueBlockOutReasonDetails);
		bool flag = false;
		if (queueBlockOutReasonDetails.RequirementTypeNotMet != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.UpdateUnlockStatus()).MethodHandle;
			}
			if (queueBlockOutReasonDetails.RequirementTypeNotMet.Value == QueueRequirement.RequirementType.VsHumanMatches && queueBlockOutReasonDetails.CausedBySelf != null)
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
				if (queueBlockOutReasonDetails.CausedBySelf.Value)
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
					if (queueBlockOutReasonDetails.NumGamesPlayed != null)
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
						if (queueBlockOutReasonDetails.NumGamesRequired != null)
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
							flag = true;
							this.m_UnlockText.text = string.Format("{0}/{1}", queueBlockOutReasonDetails.NumGamesPlayed.Value, queueBlockOutReasonDetails.NumGamesRequired.Value);
							this.m_UnlockFillBar.fillAmount = UIPlayerProfileRankDisplay.GetRankFillAmt((float)queueBlockOutReasonDetails.NumGamesPlayed.Value / (float)queueBlockOutReasonDetails.NumGamesRequired.Value);
						}
					}
				}
			}
		}
		UIManager.SetGameObjectActive(this.m_LockedRankedModeContainer, flag, null);
		UIManager.SetGameObjectActive(this.m_UnlockedRankedModeContainer, !flag, null);
	}

	private void DisplayLoading(bool displayLoading)
	{
		if (displayLoading)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.DisplayLoading(bool)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_rewardContainer, false, null);
			UIManager.SetGameObjectActive(this.m_LockedRankedModeContainer, false, null);
			UIManager.SetGameObjectActive(this.m_UnlockedRankedModeContainer, false, null);
		}
		else
		{
			this.UpdateUnlockStatus();
		}
		UIManager.SetGameObjectActive(this.m_loadingRankList, displayLoading, null);
		UIManager.SetGameObjectActive(this.m_rankList, !displayLoading, null);
	}

	private void RequestSetupFromLobby(GameType gameType, int groupSize, RankedLeaderboardSpecificRequest.RequestSpecificationType specification)
	{
		this.DisplayLoading(true);
		ClientGameManager.Get().RequestRankedLeaderboardSpecific(gameType, groupSize, specification, delegate(RankedLeaderboardSpecificResponse specificResponse)
		{
			this.DisplayLoading(false);
			if (specificResponse.Success)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.<RequestSetupFromLobby>c__AnonStorey0.<>m__0(RankedLeaderboardSpecificResponse)).MethodHandle;
				}
				if (!specificResponse.Entries.IsNullOrEmpty<RankedScoreboardEntry>())
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
					this.DisplayEmptyList(false, UIRankedModeSelectScreen.EmptyListReasons.None, null);
					List<RankedScoreboardEntry> entries = specificResponse.Entries;
					entries.Sort();
					this.m_rankList.Setup(this.GetRankListDisplayInfo(entries, groupSize), 0);
					this.m_rankList.GetComponent<Mask>().enabled = false;
					this.m_rankList.GetComponent<Mask>().enabled = true;
				}
				else
				{
					this.DisplayEmptyList(true, UIRankedModeSelectScreen.EmptyListReasons.None, null);
				}
			}
			else
			{
				this.DisplayEmptyList(true, UIRankedModeSelectScreen.EmptyListReasons.QueueIsDisabled, specificResponse.LocalizedFailure);
				Log.Error("Failed to load specific {0} Leaderboard info for {1}-player {2}: {3}", new object[]
				{
					gameType,
					groupSize,
					specification,
					specificResponse.ErrorMessage
				});
			}
		});
	}

	public void OpenTab(UIRankDisplayType tab)
	{
		this.m_soloRankTabButton.SetSelected(tab == UIRankDisplayType.Solo, false, string.Empty, string.Empty);
		this.m_duoRankTabButton.SetSelected(tab == UIRankDisplayType.Duo, false, string.Empty, string.Empty);
		this.m_teamRankTabButton.SetSelected(tab == UIRankDisplayType.FullTeam, false, string.Empty, string.Empty);
		this.m_rewardTabButton.SetSelected(tab == UIRankDisplayType.Reward, false, string.Empty, string.Empty);
		UIManager.SetGameObjectActive(this.m_leaderboardContainer, tab != UIRankDisplayType.Reward, null);
		UIManager.SetGameObjectActive(this.m_rewardContainer, tab == UIRankDisplayType.Reward, null);
		if (tab == UIRankDisplayType.Reward)
		{
			return;
		}
		if (this.m_selectedViewTab != tab)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.OpenTab(UIRankDisplayType)).MethodHandle;
			}
			this.m_ourTier = -1;
			this.m_ourDivisionId = -1;
		}
		this.m_selectedViewTab = tab;
		int key = 0;
		if (tab == UIRankDisplayType.Solo)
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
			key = 1;
		}
		else if (tab == UIRankDisplayType.Duo)
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
			key = 2;
		}
		else if (tab == UIRankDisplayType.FullTeam)
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
			key = 4;
		}
		if (UIRankedModeSelectScreen.TierInfoPerGroupSize.ContainsKey(key) && this.m_selectedRank == 0)
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
			RankedScoreboardEntry? ourEntry = UIRankedModeSelectScreen.TierInfoPerGroupSize[key].OurEntry;
			if (ourEntry != null)
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
				if (UIRankedModeSelectScreen.TierInfoPerGroupSize[key].OurEntry != null)
				{
					goto IL_16E;
				}
			}
			this.m_selectedRank = 2;
		}
		IL_16E:
		this.SelectRankFilter(this.m_selectedRank);
	}

	public void TabClicked(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		if (pointerEventData.selectedObject == this.m_soloRankTabButton.spriteController.gameObject)
		{
			this.OpenTab(UIRankDisplayType.Solo);
		}
		else if (pointerEventData.selectedObject == this.m_duoRankTabButton.spriteController.gameObject)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.TabClicked(BaseEventData)).MethodHandle;
			}
			this.OpenTab(UIRankDisplayType.Duo);
		}
		else if (pointerEventData.selectedObject == this.m_teamRankTabButton.spriteController.gameObject)
		{
			this.OpenTab(UIRankDisplayType.FullTeam);
		}
		else if (pointerEventData.selectedObject == this.m_rewardTabButton.spriteController.gameObject)
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
			this.OpenTab(UIRankDisplayType.Reward);
		}
	}

	private void HandleGroupUpdateNotification()
	{
		this.m_selectedQueueType = UIRankDisplayType.None;
		if (ClientGameManager.Get().GroupInfo.InAGroup)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.HandleGroupUpdateNotification()).MethodHandle;
			}
			if (ClientGameManager.Get().GroupInfo.Members.Count == 2)
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
				this.m_selectedQueueType = UIRankDisplayType.Duo;
			}
			else if (ClientGameManager.Get().GroupInfo.Members.Count == 4)
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
				this.m_selectedQueueType = UIRankDisplayType.FullTeam;
			}
		}
		else
		{
			this.m_selectedQueueType = UIRankDisplayType.Solo;
		}
		for (int i = 0; i < this.m_rankDisplays.Length; i++)
		{
			this.m_rankDisplays[i].SetAsActiveQueue(i == (int)this.m_selectedQueueType);
		}
		switch (this.m_selectedQueueType)
		{
		case UIRankDisplayType.Solo:
			this.SetQueueButtonLabel(StringUtil.TR("StartSoloQueue", "OverlayScreensScene"));
			break;
		case UIRankDisplayType.Duo:
			this.SetQueueButtonLabel(StringUtil.TR("QueueDuoRanked", "RankMode"));
			break;
		case UIRankDisplayType.FullTeam:
			this.SetQueueButtonLabel(StringUtil.TR("QueueTeamRanked", "RankMode"));
			break;
		default:
			this.SetQueueButtonLabel(StringUtil.TR("UnableToQueue", "RankMode"));
			break;
		}
		this.CheckQueueButtonStatus();
	}

	private void CheckQueueButtonStatus()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		bool flag = clientGameManager.MeetsAllQueueRequirements(GameType.Ranked);
		int num = clientGameManager.GroupInfo.Members.Count;
		if (num < 1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.CheckQueueButtonStatus()).MethodHandle;
			}
			num = 1;
		}
		bool flag2 = clientGameManager.MeetsGroupSizeRequirement(GameType.Ranked, num);
		bool flag3 = this.ServerAllowQueueRank();
		bool flag4;
		if (clientGameManager.GameTypeAvailabilies.ContainsKey(GameType.Ranked))
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
			DateTime? penaltyTimeout = clientGameManager.GameTypeAvailabilies[GameType.Ranked].PenaltyTimeout;
			if (penaltyTimeout != null)
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
				flag4 = (DateTime.UtcNow < clientGameManager.GameTypeAvailabilies[GameType.Ranked].PenaltyTimeout.Value);
				goto IL_BB;
			}
		}
		flag4 = false;
		IL_BB:
		bool flag5 = flag4;
		bool flag6 = false;
		if (clientGameManager.GroupInfo.InAGroup)
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
			for (int i = 0; i < clientGameManager.GroupInfo.Members.Count; i++)
			{
				DateTime? penaltyTimeout2 = clientGameManager.GroupInfo.Members[i].PenaltyTimeout;
				if (penaltyTimeout2 != null)
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
					if (DateTime.UtcNow < clientGameManager.GroupInfo.Members[i].PenaltyTimeout.Value)
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
						flag6 = true;
						goto IL_177;
					}
				}
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		IL_177:
		bool flag7;
		if (this.m_selectedQueueType != UIRankDisplayType.None)
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
			if (flag)
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
				if (flag2)
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
					if (flag3)
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
						if (!flag5)
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
							flag7 = !flag6;
							goto IL_1C8;
						}
					}
				}
			}
		}
		flag7 = false;
		IL_1C8:
		bool flag8 = flag7;
		if (!flag8)
		{
			if (!flag3)
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
				this.m_cannotQueue = UIRankedModeSelectScreen.RankedQueueRejectionReasons.ServerDisabled;
			}
			else if (!flag2)
			{
				this.m_cannotQueue = UIRankedModeSelectScreen.RankedQueueRejectionReasons.IncorrectGroupSize;
			}
			else if (!flag)
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
				this.m_cannotQueue = UIRankedModeSelectScreen.RankedQueueRejectionReasons.DoNotMeetRequirements;
			}
			else if (flag5)
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
				this.m_cannotQueue = UIRankedModeSelectScreen.RankedQueueRejectionReasons.PenaltyTimeout;
			}
			else if (flag6)
			{
				this.m_cannotQueue = UIRankedModeSelectScreen.RankedQueueRejectionReasons.GroupTimeout;
			}
		}
		else
		{
			this.m_cannotQueue = UIRankedModeSelectScreen.RankedQueueRejectionReasons.None;
		}
		this.SetQueueButtonClickable(flag8);
		this.ForceQueueButtonCallback(this.m_cannotQueue != UIRankedModeSelectScreen.RankedQueueRejectionReasons.None);
	}

	private void ForceQueueButtonCallback(bool forceCallback)
	{
		this.m_startQueueBtn.spriteController.SetForceHovercallback(forceCallback);
		this.m_startQueueBtn.spriteController.SetForceExitCallback(forceCallback);
	}

	private void SetQueueButtonClickable(bool setQueueButtonClickable)
	{
		this.m_startQueueBtn.spriteController.SetClickable(setQueueButtonClickable);
		UIManager.SetGameObjectActive(this.m_startQueueBtn.spriteController.m_defaultImage, setQueueButtonClickable, null);
		UIManager.SetGameObjectActive(this.m_startQueueBtn.spriteController.m_hoverImage, setQueueButtonClickable, null);
		UIManager.SetGameObjectActive(this.m_startQueueBtn.spriteController.m_pressedImage, setQueueButtonClickable, null);
	}

	private bool QueueButtonTooltipSetup(UITooltipBase tooltip)
	{
		if (!this.m_startQueueBtn.spriteController.IsClickable())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.QueueButtonTooltipSetup(UITooltipBase)).MethodHandle;
			}
			UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
			if (this.m_cannotQueue == UIRankedModeSelectScreen.RankedQueueRejectionReasons.DoNotMeetRequirements)
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
				ClientGameManager clientGameManager = ClientGameManager.Get();
				LocalizationPayload blockingQueueRestriction = clientGameManager.GetBlockingQueueRestriction(GameType.Ranked);
				string text;
				if (blockingQueueRestriction == null)
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
					text = StringUtil.TR("UnknownError", "Global");
				}
				else
				{
					text = blockingQueueRestriction.ToString();
				}
				string tooltipText = text;
				uititledTooltip.Setup(StringUtil.TR("DoNotMeetRequirements", "Ranked"), tooltipText, string.Empty);
				return true;
			}
			if (this.m_cannotQueue == UIRankedModeSelectScreen.RankedQueueRejectionReasons.IncorrectGroupSize)
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
				int num = ClientGameManager.Get().GroupInfo.Members.Count;
				if (num < 1)
				{
					num = 1;
				}
				LocalizationPayload localizationPayload = ClientGameManager.Get().GetReasonGroupSizeCantQueue(GameType.Ranked, num);
				if (localizationPayload == null)
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
					localizationPayload = LocalizationPayload.Create("BadGroupSizeForQueue", "Matchmaking", new LocalizationArg[]
					{
						LocalizationArg_Int32.Create(num)
					});
				}
				uititledTooltip.Setup(StringUtil.TR("IncorrectGroupSizeTitle", "Ranked"), localizationPayload.ToString(), string.Empty);
				return true;
			}
			if (this.m_cannotQueue == UIRankedModeSelectScreen.RankedQueueRejectionReasons.ServerDisabled)
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
				uititledTooltip.Setup(StringUtil.TR("Disabled", "Global"), string.Format(StringUtil.TR("RankedModeDisabled", "Ranked"), new object[0]), string.Empty);
				return true;
			}
			if (this.m_cannotQueue == UIRankedModeSelectScreen.RankedQueueRejectionReasons.PenaltyTimeout)
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
				ClientGameManager clientGameManager2 = ClientGameManager.Get();
				TimeSpan difference = clientGameManager2.GameTypeAvailabilies[GameType.Ranked].PenaltyTimeout.Value - DateTime.UtcNow;
				string timeDifferenceText = StringUtil.GetTimeDifferenceText(difference, false);
				uititledTooltip.Setup(StringUtil.TR("YouHaveBeenPenalized", "Ranked"), string.Format(StringUtil.TR("CannotQueueUntilTimeout", "Ranked"), timeDifferenceText), string.Empty);
				return true;
			}
			if (this.m_cannotQueue == UIRankedModeSelectScreen.RankedQueueRejectionReasons.GroupTimeout)
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
				string text2 = string.Empty;
				for (int i = 0; i < ClientGameManager.Get().GroupInfo.Members.Count; i++)
				{
					DateTime? penaltyTimeout = ClientGameManager.Get().GroupInfo.Members[i].PenaltyTimeout;
					if (penaltyTimeout != null)
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
						text2 = text2 + ClientGameManager.Get().GroupInfo.Members[i].MemberDisplayName + " ";
					}
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				uititledTooltip.Setup(StringUtil.TR("GroupMembersPenalized", "Ranked"), string.Format(StringUtil.TR("CannotQueueMembersPenalized", "Ranked"), text2), string.Empty);
				return true;
			}
		}
		return false;
	}

	public void SetQueueButtonLabel(string text)
	{
		for (int i = 0; i < this.m_queueButtonLabels.Length; i++)
		{
			this.m_queueButtonLabels[i].text = text;
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.SetQueueButtonLabel(string)).MethodHandle;
		}
	}

	private List<UIRankingDisplayEntry> GetRankingEntries(List<RankedScoreboardEntry> entries, int groupSize)
	{
		List<UIRankingDisplayEntry> list = new List<UIRankingDisplayEntry>();
		if (!entries.IsNullOrEmpty<RankedScoreboardEntry>())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.GetRankingEntries(List<RankedScoreboardEntry>, int)).MethodHandle;
			}
			for (int i = 0; i < entries.Count; i++)
			{
				UIRankingDisplayEntry item = new UIRankingDisplayEntry(entries[i], groupSize);
				list.Add(item);
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return list;
	}

	public void ProcessTierInfoPerGroupSize(Dictionary<int, PerGroupSizeTierInfo> tierInfoPerGroupSize)
	{
		if (UIRankedModeSelectScreen.s_tierInfoPerGroupSize != null)
		{
			using (Dictionary<int, PerGroupSizeTierInfo>.Enumerator enumerator = UIRankedModeSelectScreen.s_tierInfoPerGroupSize.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, PerGroupSizeTierInfo> keyValuePair = enumerator.Current;
					if (keyValuePair.Value.OurEntry != null)
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.ProcessTierInfoPerGroupSize(Dictionary<int, PerGroupSizeTierInfo>)).MethodHandle;
						}
						int key = keyValuePair.Key;
						int tier = keyValuePair.Value.OurEntry.Value.Tier;
						PerGroupSizeTierInfo perGroupSizeTierInfo;
						if (tierInfoPerGroupSize.TryGetValue(key, out perGroupSizeTierInfo) && perGroupSizeTierInfo.OurEntry != null)
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
							string text = null;
							int tier2 = perGroupSizeTierInfo.OurEntry.Value.Tier;
							if (tier < 1 && tier2 != tier)
							{
								text = "TierPostPlacement";
							}
							else if (tier2 < tier)
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
								text = "TierRaised";
							}
							else if (tier2 > tier)
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
								text = "TierLowered";
							}
							if (text != null)
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
								string[] array = new string[]
								{
									"SoloGroup",
									"DuoGroup",
									"TripleGroup",
									"FourGroup"
								};
								string arg = StringUtil.TR(array[key - 1], "RankMode");
								string tierName = ClientGameManager.Get().GetTierName(GameType.Ranked, tier2);
								TextConsole.Get().Write(new TextConsole.Message
								{
									Text = string.Format(StringUtil.TR(text, "RankMode"), tierName, arg),
									MessageType = ConsoleMessageType.SystemMessage
								}, null);
							}
						}
					}
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		UIRankedModeSelectScreen.s_tierInfoPerGroupSize = tierInfoPerGroupSize;
		if (this.m_isVisible)
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
			for (int i = 0; i < this.m_rankDisplays.Length; i++)
			{
				this.m_rankDisplays[i].Setup((UIRankDisplayType)i, UIRankedModeSelectScreen.s_tierInfoPerGroupSize);
			}
			UIRankDisplayType tab = UIRankDisplayType.Solo;
			if (ClientGameManager.Get().GroupInfo.Members.Count == 2)
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
				tab = UIRankDisplayType.Duo;
			}
			else if (ClientGameManager.Get().GroupInfo.Members.Count == 4)
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
				tab = UIRankDisplayType.FullTeam;
			}
			this.OpenTab(tab);
			this.HandleGroupUpdateNotification();
		}
	}

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(this.m_selectScreenContainer, visible, null);
		bool flag = this.m_isVisible != visible;
		this.m_isVisible = visible;
		if (visible)
		{
			if (flag)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.SetVisible(bool)).MethodHandle;
				}
				if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
					UIManager.SetGameObjectActive(this.m_rankRewardDisabledNotice, !GameManager.Get().GameplayOverrides.RankedUpdatesEnabled, null);
					this.m_rankRewardDisabledNotice.text = StringUtil.TR("RankRewardsDisabledDescription", "OverlayScreensScene");
				}
				else
				{
					UIManager.SetGameObjectActive(this.m_rankRewardDisabledNotice, false, null);
				}
				for (int i = 0; i < this.m_rankDisplays.Length; i++)
				{
					UIManager.SetGameObjectActive(this.m_rankDisplays[i].m_selectedQueueRankContainer, false, null);
					UIManager.SetGameObjectActive(this.m_rankDisplays[i].m_InPlacementMatchesContainer, false, null);
					UIManager.SetGameObjectActive(this.m_rankDisplays[i].m_HasRankAlreadyContainer, false, null);
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!this.m_loadedData)
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
					this.m_soloRankTabButton.spriteController.SetClickable(false);
					this.m_duoRankTabButton.spriteController.SetClickable(false);
					this.m_teamRankTabButton.spriteController.SetClickable(false);
					this.m_rewardTabButton.spriteController.SetClickable(false);
					this.m_filterListDropdownBtn.spriteController.SetClickable(false);
					this.m_InstanceLabel.text = string.Empty;
				}
				this.DisplayLoading(true);
				ClientGameManager.Get().RequestRankedLeaderboardOverview(GameType.Ranked, delegate(RankedLeaderboardOverviewResponse overviewResponse)
				{
					this.DisplayLoading(false);
					if (overviewResponse.Success)
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
							RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIRankedModeSelectScreen.<SetVisible>m__0(RankedLeaderboardOverviewResponse)).MethodHandle;
						}
						this.m_loadedData = true;
						this.m_soloRankTabButton.spriteController.SetClickable(true);
						this.m_duoRankTabButton.spriteController.SetClickable(true);
						this.m_teamRankTabButton.spriteController.SetClickable(true);
						this.m_rewardTabButton.spriteController.SetClickable(true);
						this.m_filterListDropdownBtn.spriteController.SetClickable(true);
						this.ProcessTierInfoPerGroupSize(overviewResponse.TierInfoPerGroupSize);
					}
					else
					{
						LobbyGameClientInterface lobbyInterface = ClientGameManager.Get().LobbyInterface;
						if (lobbyInterface != null)
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
							lobbyInterface.WriteErrorToConsole(overviewResponse.LocalizedFailure, overviewResponse.ErrorMessage);
						}
						Log.Error("Failed to load overall Ranked Leaderboard info: " + overviewResponse.ErrorMessage, new object[0]);
					}
				});
			}
			this.HandleGroupUpdateNotification();
		}
	}

	public static IDataEntry RankingEntryToDataEntry(UIRankingDisplayEntry entry)
	{
		return entry;
	}

	public void UpdateReadyButton(bool shouldBeOn)
	{
		if (shouldBeOn)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.UpdateReadyButton(bool)).MethodHandle;
			}
			this.CheckQueueButtonStatus();
		}
		else
		{
			this.SetQueueButtonClickable(false);
		}
	}

	public void StartQueueBtnClicked(BaseEventData data)
	{
		UICharacterSelectScreenController.Get().DoReadyClick(FrontEndButtonSounds.RankQueueButtonClick);
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.Update()).MethodHandle;
			}
			bool flag = true;
			if (EventSystem.current != null)
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
				if (EventSystem.current.IsPointerOverGameObject(-1))
				{
					StandaloneInputModuleWithEventDataAccess component = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
					if (component != null && component.GetLastPointerEventDataPublic(-1).pointerEnter != null)
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
						UIMainMenu componentInParent = component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<UIMainMenu>();
						bool flag2 = false;
						if (componentInParent == null)
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
							_SelectableBtn componentInParent2 = component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<_SelectableBtn>();
							if (UIFrontEnd.Get() != null)
							{
								while (componentInParent2 != null)
								{
									if (!(componentInParent2 == this.m_filterListDropdownBtn))
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
										if (!(componentInParent2 == this.m_RankDropdownDivision))
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
											if (!(componentInParent2 == this.m_RankDropdownFriends))
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
												if (!(componentInParent2 == this.m_RankDropdownTopPlayers))
												{
													componentInParent2 = componentInParent2.transform.parent.GetComponentInParent<_SelectableBtn>();
													continue;
												}
												for (;;)
												{
													switch (1)
													{
													case 0:
														continue;
													}
													break;
												}
											}
										}
									}
									flag2 = true;
									goto IL_197;
								}
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
							}
						}
						IL_197:
						if (!(componentInParent != null))
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
							if (!flag2)
							{
								goto IL_1B1;
							}
						}
						flag = false;
					}
				}
			}
			IL_1B1:
			if (flag)
			{
				this.DoFilterDropdownVisible(false);
			}
		}
	}

	public static string GetTierIconResource(int tier)
	{
		if (tier > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.GetTierIconResource(int)).MethodHandle;
			}
			ClientGameManager clientGameManager = ClientGameManager.Get();
			if (clientGameManager != null)
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
				GameType key = GameType.Ranked;
				GameTypeAvailability gameTypeAvailability;
				if (clientGameManager.GameTypeAvailabilies.TryGetValue(key, out gameTypeAvailability))
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
					if (tier <= gameTypeAvailability.PerTierDefinitions.Count)
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
						return gameTypeAvailability.PerTierDefinitions[tier - 1].IconResource;
					}
				}
			}
		}
		return null;
	}

	public static bool IsRatchetTier(int tier)
	{
		if (tier > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.IsRatchetTier(int)).MethodHandle;
			}
			ClientGameManager clientGameManager = ClientGameManager.Get();
			if (clientGameManager != null)
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
				GameType key = GameType.Ranked;
				GameTypeAvailability gameTypeAvailability;
				if (clientGameManager.GameTypeAvailabilies.TryGetValue(key, out gameTypeAvailability))
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
					if (tier <= gameTypeAvailability.PerTierDefinitions.Count)
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
						return gameTypeAvailability.PerTierDefinitions[tier - 1].IsRachet;
					}
				}
			}
		}
		return false;
	}

	public unsafe void GetTierLocalizedName(int tier, int instanceId, int groupSize, out string tierName, out string instanceName)
	{
		instanceName = null;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeSelectScreen.GetTierLocalizedName(int, int, int, string*, string*)).MethodHandle;
			}
			if (tier >= 2)
			{
				instanceName = clientGameManager.GetTierInstanceName(instanceId);
			}
			tierName = clientGameManager.GetTierName(GameType.Ranked, tier);
		}
		else
		{
			tierName = "[BadCGM]";
		}
	}

	public enum RankedQueueRejectionReasons
	{
		None,
		DoNotMeetRequirements,
		IncorrectGroupSize,
		ServerDisabled,
		PenaltyTimeout,
		GroupTimeout
	}

	public enum EmptyListReasons
	{
		None,
		FailedToReceiveTeirInfo,
		QueueIsDisabled,
		NeedToPlayMoreGames
	}
}
