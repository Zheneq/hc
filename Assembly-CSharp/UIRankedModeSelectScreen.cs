using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRankedModeSelectScreen : MonoBehaviour
{
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

	private RankedQueueRejectionReasons m_cannotQueue;

	private bool m_loadedData;

	private int m_ourTier = -1;

	private int m_ourDivisionId = -1;

	public static Dictionary<int, PerGroupSizeTierInfo> TierInfoPerGroupSize
	{
		get { return s_tierInfoPerGroupSize; }
	}

	internal static UIRankedModeSelectScreen Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		m_dropdownBtnTextLabels = m_filterListDropdownBtn.GetComponentsInChildren<TextMeshProUGUI>(true);
		m_dropdownDivisionTextLabels = m_RankDropdownDivision.GetComponentsInChildren<TextMeshProUGUI>(true);
		s_instance = this;
		m_startQueueBtn.spriteController.callback = StartQueueBtnClicked;
		m_filterListDropdownBtn.spriteController.callback = DropdownClicked;
		m_filterListDropdownBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.RankDropdownClick;
		m_soloRankTabButton.spriteController.callback = TabClicked;
		m_duoRankTabButton.spriteController.callback = TabClicked;
		m_teamRankTabButton.spriteController.callback = TabClicked;
		m_rewardTabButton.spriteController.callback = TabClicked;
		m_soloRankTabButton.spriteController.m_soundToPlay = FrontEndButtonSounds.RankTabClick;
		m_duoRankTabButton.spriteController.m_soundToPlay = FrontEndButtonSounds.RankTabClick;
		m_teamRankTabButton.spriteController.m_soundToPlay = FrontEndButtonSounds.RankTabClick;
		m_rewardTabButton.spriteController.m_soundToPlay = FrontEndButtonSounds.RankTabClick;
		m_RankDropdownDivision.spriteController.callback = RankListClicked;
		m_RankDropdownFriends.spriteController.callback = RankListClicked;
		m_RankDropdownTopPlayers.spriteController.callback = RankListClicked;
		m_RankDropdownDivision.spriteController.m_soundToPlay = FrontEndButtonSounds.RankDropdownSelect;
		m_RankDropdownFriends.spriteController.m_soundToPlay = FrontEndButtonSounds.RankDropdownSelect;
		m_RankDropdownTopPlayers.spriteController.m_soundToPlay = FrontEndButtonSounds.RankDropdownSelect;
		m_startQueueBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, QueueButtonTooltipSetup);
		UIManager.SetGameObjectActive(m_rewardContainer, false);
		DoFilterDropdownVisible(false);
		SetDropdownDivisionText(StringUtil.TR("MyInstance", "RankMode"));
		SetDropdownText(StringUtil.TR("MyInstance", "RankMode"));
		ClientGameManager.Get().OnGroupUpdateNotification += HandleGroupUpdateNotification;
		m_rankList.GetScrollRect().movementType = ScrollRect.MovementType.Clamped;
		CanvasGroup component = m_rankList.GetComponent<CanvasGroup>();
		if (component != null)
		{
			component.blocksRaycasts = true;
			component.interactable = true;
		}
		m_loadedData = false;
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnGroupUpdateNotification -= HandleGroupUpdateNotification;
		}
		s_instance = null;
	}

	private void SetDropdownText(string text)
	{
		for (int i = 0; i < m_dropdownBtnTextLabels.Length; i++)
		{
			m_dropdownBtnTextLabels[i].text = text;
		}
		while (true)
		{
			return;
		}
	}

	private void SetDropdownDivisionText(string text)
	{
		for (int i = 0; i < m_dropdownDivisionTextLabels.Length; i++)
		{
			m_dropdownDivisionTextLabels[i].text = text;
		}
		while (true)
		{
			return;
		}
	}

	private void SelectRankFilter(int rank)
	{
		m_selectedRank = rank;
		int num;
		if (m_selectedViewTab == UIRankDisplayType.Solo)
		{
			num = 1;
		}
		else if (m_selectedViewTab == UIRankDisplayType.Duo)
		{
			num = 2;
		}
		else
		{
			num = 4;
		}
		int num2 = num;
		if (m_selectedRank == 2)
		{
			RequestSetupFromLobby(GameType.Ranked, num2, RankedLeaderboardSpecificRequest.RequestSpecificationType._0012);
			m_InstanceLabel.text = string.Empty;
			UIManager.SetGameObjectActive(m_streakContainer, false);
		}
		else if (m_selectedRank == 0)
		{
			if (TierInfoPerGroupSize != null)
			{
				if (TierInfoPerGroupSize.ContainsKey(num2))
				{
					if (TierInfoPerGroupSize[num2].OurEntry.HasValue)
					{
						RankedScoreboardEntry value = TierInfoPerGroupSize[num2].OurEntry.Value;
						m_ourTier = value.Tier;
						RankedScoreboardEntry value2 = TierInfoPerGroupSize[num2].OurEntry.Value;
						m_ourDivisionId = value2.InstanceId;
						string foo;
						string instanceName;
						GetTierLocalizedName(m_ourTier, m_ourDivisionId, num2, out foo, out instanceName);
						m_InstanceLabel.text = instanceName;
						RankedScoreboardEntry value3 = TierInfoPerGroupSize[num2].OurEntry.Value;
						int winStreak = value3.WinStreak;
						if (winStreak != 0)
						{
							UIManager.SetGameObjectActive(m_streakContainer, false);
							if (winStreak > 0)
							{
								if (winStreak > 1)
								{
									m_StreakLabel.text = string.Format(StringUtil.TR("RankedWinStreak", "RankMode"), winStreak.ToString());
								}
								else
								{
									m_StreakLabel.text = StringUtil.TR("RankedOneWinStreak", "RankMode");
								}
							}
							else if (winStreak < -1)
							{
								m_StreakLabel.text = string.Format(StringUtil.TR("RankedLossStreak", "RankMode"), Mathf.Abs(winStreak).ToString());
							}
							else
							{
								m_StreakLabel.text = StringUtil.TR("RankedOneLossStreak", "RankMode");
							}
						}
						else
						{
							UIManager.SetGameObjectActive(m_streakContainer, false);
						}
						RequestSetupFromLobby(GameType.Ranked, num2, RankedLeaderboardSpecificRequest.RequestSpecificationType._001D);
					}
					else
					{
						DisplayEmptyList(true, EmptyListReasons.NeedToPlayMoreGames);
					}
				}
				else
				{
					DisplayEmptyList(true, EmptyListReasons.QueueIsDisabled);
				}
			}
			else
			{
				DisplayEmptyList(true, EmptyListReasons.FailedToReceiveTeirInfo);
			}
		}
		else
		{
			RequestSetupFromLobby(GameType.Ranked, num2, RankedLeaderboardSpecificRequest.RequestSpecificationType._000E);
			m_InstanceLabel.text = string.Empty;
			UIManager.SetGameObjectActive(m_streakContainer, false);
		}
		m_RankDropdownDivision.SetSelected(m_selectedRank == 0, false, string.Empty, string.Empty);
		m_RankDropdownFriends.SetSelected(m_selectedRank == 1, false, string.Empty, string.Empty);
		m_RankDropdownTopPlayers.SetSelected(m_selectedRank == 2, false, string.Empty, string.Empty);
		m_RankDropdownDivision.spriteController.SetClickable(m_selectedRank != 0);
		m_RankDropdownFriends.spriteController.SetClickable(m_selectedRank != 1);
		m_RankDropdownTopPlayers.spriteController.SetClickable(m_selectedRank != 2);
		if (m_selectedRank == 0)
		{
			SetDropdownText(StringUtil.TR("MyInstance", "RankMode"));
		}
		else if (m_selectedRank == 1)
		{
			SetDropdownText(StringUtil.TR("Friends", "Global"));
		}
		else if (m_selectedRank == 2)
		{
			SetDropdownText(StringUtil.TR("TopPlayers", "RankMode"));
		}
		SetFilterDropdownVisible(false);
	}

	public void RankListClicked(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		if (pointerEventData.selectedObject == m_RankDropdownDivision.spriteController.gameObject)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					SelectRankFilter(0);
					return;
				}
			}
		}
		if (pointerEventData.selectedObject == m_RankDropdownFriends.spriteController.gameObject)
		{
			SelectRankFilter(1);
		}
		else if (pointerEventData.selectedObject == m_RankDropdownTopPlayers.spriteController.gameObject)
		{
			SelectRankFilter(2);
		}
	}

	private void DoFilterDropdownVisible(bool visible)
	{
		filterDropdownOpen = visible;
		UIManager.SetGameObjectActive(m_filterListDropdown, visible);
		m_filterListDropdownBtn.SetSelected(visible, false, string.Empty, string.Empty);
		if (visible)
		{
			return;
		}
		while (true)
		{
			m_RankDropdownDivision.spriteController.ForceSetPointerEntered(false);
			m_RankDropdownFriends.spriteController.ForceSetPointerEntered(false);
			m_RankDropdownTopPlayers.spriteController.ForceSetPointerEntered(false);
			return;
		}
	}

	public void SetFilterDropdownVisible(bool visible)
	{
		if (filterDropdownOpen == visible)
		{
			return;
		}
		while (true)
		{
			DoFilterDropdownVisible(visible);
			return;
		}
	}

	public void ToggleFilterDropdown()
	{
		SetFilterDropdownVisible(!filterDropdownOpen);
	}

	public void DropdownClicked(BaseEventData data)
	{
		ToggleFilterDropdown();
	}

	public List<IDataEntry> GetRankListDisplayInfo(List<RankedScoreboardEntry> entries, int groupSize)
	{
		return GetRankingEntries(entries, groupSize).ConvertAll<IDataEntry>(RankingEntryToDataEntry);
	}

	private int ConvertDisplayTypeToGroupSize(UIRankDisplayType type)
	{
		int result = 0;
		if (m_selectedQueueType == UIRankDisplayType.Solo)
		{
			result = 1;
		}
		else if (m_selectedQueueType == UIRankDisplayType.Duo)
		{
			result = 2;
		}
		else if (m_selectedQueueType == UIRankDisplayType.FullTeam)
		{
			result = 4;
		}
		return result;
	}

	private bool ServerAllowQueueRank()
	{
		using (Dictionary<GameType, GameTypeAvailability>.Enumerator enumerator = ClientGameManager.Get().GameTypeAvailabilies.GetEnumerator())
		{
			while (true)
			{
				if (!enumerator.MoveNext())
				{
					break;
				}
				KeyValuePair<GameType, GameTypeAvailability> current = enumerator.Current;
				if (current.Key == GameType.Ranked)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							if (current.Value.IsActive)
							{
								return true;
							}
							goto end_IL_0014;
						}
					}
				}
			}
			end_IL_0014:;
		}
		return false;
	}

	private bool IsQueueTypeValidWithGroupSize()
	{
		using (Dictionary<GameType, GameTypeAvailability>.Enumerator enumerator = ClientGameManager.Get().GameTypeAvailabilies.GetEnumerator())
		{
			while (true)
			{
				if (!enumerator.MoveNext())
				{
					break;
				}
				KeyValuePair<GameType, GameTypeAvailability> current = enumerator.Current;
				if (current.Key == GameType.Ranked)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
						{
							int key = ConvertDisplayTypeToGroupSize(m_selectedQueueType);
							if (!current.Value.QueueableGroupSizes.IsNullOrEmpty())
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										break;
									default:
										if (current.Value.QueueableGroupSizes.ContainsKey(key))
										{
											while (true)
											{
												switch (7)
												{
												case 0:
													break;
												default:
													return true;
												}
											}
										}
										goto end_IL_0014;
									}
								}
							}
							goto end_IL_0014;
						}
						}
					}
				}
			}
			end_IL_0014:;
		}
		return false;
	}

	private void DisplayEmptyList(bool displayEmptyList, EmptyListReasons reason = EmptyListReasons.None, LocalizationPayload localizedReason = null)
	{
		UIManager.SetGameObjectActive(m_emptyRankList, displayEmptyList);
		UIManager.SetGameObjectActive(m_rankList, !displayEmptyList);
		if (m_emptyRankListText == null)
		{
			m_emptyRankListText = m_emptyRankList.GetComponentInChildren<TextMeshProUGUI>(true);
		}
		if (localizedReason != null)
		{
			m_emptyRankListText.text = localizedReason.ToString();
			return;
		}
		if (reason == EmptyListReasons.None)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (m_emptyRankListText != null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								if (m_selectedRank == 2)
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											break;
										default:
											m_emptyRankListText.text = StringUtil.TR("NoPlayersRankedHighEnough", "RankMode");
											return;
										}
									}
								}
								m_emptyRankListText.text = StringUtil.TR("PlayMoreToGetRanked", "RankMode");
								return;
							}
						}
					}
					return;
				}
			}
		}
		if (reason == EmptyListReasons.FailedToReceiveTeirInfo)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					m_emptyRankListText.text = StringUtil.TR("FailedToRecieveRankData", "RankMode");
					return;
				}
			}
		}
		if (reason == EmptyListReasons.QueueIsDisabled)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					m_emptyRankListText.text = StringUtil.TR("QueSizeDisabled", "RankMode");
					return;
				}
			}
		}
		if (reason != EmptyListReasons.NeedToPlayMoreGames)
		{
			return;
		}
		while (true)
		{
			m_emptyRankListText.text = StringUtil.TR("PlayMoreToGetRanked", "RankMode");
			return;
		}
	}

	public void UpdateUnlockStatus()
	{
		QueueBlockOutReasonDetails Details;
		ClientGameManager.Get().GetBlockingQueueRestriction(GameType.Ranked, out Details);
		bool flag = false;
		if (Details.RequirementTypeNotMet.HasValue)
		{
			if (Details.RequirementTypeNotMet.Value == QueueRequirement.RequirementType.VsHumanMatches && Details.CausedBySelf.HasValue)
			{
				if (Details.CausedBySelf.Value)
				{
					if (Details.NumGamesPlayed.HasValue)
					{
						if (Details.NumGamesRequired.HasValue)
						{
							flag = true;
							m_UnlockText.text = new StringBuilder().Append(Details.NumGamesPlayed.Value).Append("/").Append(Details.NumGamesRequired.Value).ToString();
							m_UnlockFillBar.fillAmount = UIPlayerProfileRankDisplay.GetRankFillAmt((float)Details.NumGamesPlayed.Value / (float)Details.NumGamesRequired.Value);
						}
					}
				}
			}
		}
		UIManager.SetGameObjectActive(m_LockedRankedModeContainer, flag);
		UIManager.SetGameObjectActive(m_UnlockedRankedModeContainer, !flag);
	}

	private void DisplayLoading(bool displayLoading)
	{
		if (displayLoading)
		{
			UIManager.SetGameObjectActive(m_rewardContainer, false);
			UIManager.SetGameObjectActive(m_LockedRankedModeContainer, false);
			UIManager.SetGameObjectActive(m_UnlockedRankedModeContainer, false);
		}
		else
		{
			UpdateUnlockStatus();
		}
		UIManager.SetGameObjectActive(m_loadingRankList, displayLoading);
		UIManager.SetGameObjectActive(m_rankList, !displayLoading);
	}

	private void RequestSetupFromLobby(GameType gameType, int groupSize, RankedLeaderboardSpecificRequest.RequestSpecificationType specification)
	{
		DisplayLoading(true);
		ClientGameManager.Get().RequestRankedLeaderboardSpecific(gameType, groupSize, specification, delegate(RankedLeaderboardSpecificResponse specificResponse)
		{
			DisplayLoading(false);
			if (specificResponse.Success)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						if (!specificResponse.Entries.IsNullOrEmpty())
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
								{
									DisplayEmptyList(false);
									List<RankedScoreboardEntry> entries = specificResponse.Entries;
									entries.Sort();
									m_rankList.Setup(GetRankListDisplayInfo(entries, groupSize));
									m_rankList.GetComponent<Mask>().enabled = false;
									m_rankList.GetComponent<Mask>().enabled = true;
									return;
								}
								}
							}
						}
						DisplayEmptyList(true);
						return;
					}
				}
			}
			DisplayEmptyList(true, EmptyListReasons.QueueIsDisabled, specificResponse.LocalizedFailure);
			Log.Error("Failed to load specific {0} Leaderboard info for {1}-player {2}: {3}", gameType, groupSize, specification, specificResponse.ErrorMessage);
		});
	}

	public void OpenTab(UIRankDisplayType tab)
	{
		m_soloRankTabButton.SetSelected(tab == UIRankDisplayType.Solo, false, string.Empty, string.Empty);
		m_duoRankTabButton.SetSelected(tab == UIRankDisplayType.Duo, false, string.Empty, string.Empty);
		m_teamRankTabButton.SetSelected(tab == UIRankDisplayType.FullTeam, false, string.Empty, string.Empty);
		m_rewardTabButton.SetSelected(tab == UIRankDisplayType.Reward, false, string.Empty, string.Empty);
		UIManager.SetGameObjectActive(m_leaderboardContainer, tab != UIRankDisplayType.Reward);
		UIManager.SetGameObjectActive(m_rewardContainer, tab == UIRankDisplayType.Reward);
		if (tab == UIRankDisplayType.Reward)
		{
			return;
		}
		if (m_selectedViewTab != tab)
		{
			m_ourTier = -1;
			m_ourDivisionId = -1;
		}
		m_selectedViewTab = tab;
		int key = 0;
		if (tab == UIRankDisplayType.Solo)
		{
			key = 1;
		}
		else if (tab == UIRankDisplayType.Duo)
		{
			key = 2;
		}
		else if (tab == UIRankDisplayType.FullTeam)
		{
			key = 4;
		}
		if (TierInfoPerGroupSize.ContainsKey(key) && m_selectedRank == 0)
		{
			PerGroupSizeTierInfo perGroupSizeTierInfo = TierInfoPerGroupSize[key];
			RankedScoreboardEntry? ourEntry = perGroupSizeTierInfo.OurEntry;
			if (ourEntry.HasValue)
			{
				if (TierInfoPerGroupSize[key].OurEntry.HasValue)
				{
					goto IL_016e;
				}
			}
			m_selectedRank = 2;
		}
		goto IL_016e;
		IL_016e:
		SelectRankFilter(m_selectedRank);
	}

	public void TabClicked(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		if (pointerEventData.selectedObject == m_soloRankTabButton.spriteController.gameObject)
		{
			OpenTab(UIRankDisplayType.Solo);
			return;
		}
		if (pointerEventData.selectedObject == m_duoRankTabButton.spriteController.gameObject)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					OpenTab(UIRankDisplayType.Duo);
					return;
				}
			}
		}
		if (pointerEventData.selectedObject == m_teamRankTabButton.spriteController.gameObject)
		{
			OpenTab(UIRankDisplayType.FullTeam);
		}
		else
		{
			if (!(pointerEventData.selectedObject == m_rewardTabButton.spriteController.gameObject))
			{
				return;
			}
			while (true)
			{
				OpenTab(UIRankDisplayType.Reward);
				return;
			}
		}
	}

	private void HandleGroupUpdateNotification()
	{
		m_selectedQueueType = UIRankDisplayType.None;
		if (ClientGameManager.Get().GroupInfo.InAGroup)
		{
			if (ClientGameManager.Get().GroupInfo.Members.Count == 2)
			{
				m_selectedQueueType = UIRankDisplayType.Duo;
			}
			else if (ClientGameManager.Get().GroupInfo.Members.Count == 4)
			{
				m_selectedQueueType = UIRankDisplayType.FullTeam;
			}
		}
		else
		{
			m_selectedQueueType = UIRankDisplayType.Solo;
		}
		for (int i = 0; i < m_rankDisplays.Length; i++)
		{
			m_rankDisplays[i].SetAsActiveQueue(i == (int)m_selectedQueueType);
		}
		switch (m_selectedQueueType)
		{
		case UIRankDisplayType.Solo:
			SetQueueButtonLabel(StringUtil.TR("StartSoloQueue", "OverlayScreensScene"));
			break;
		case UIRankDisplayType.Duo:
			SetQueueButtonLabel(StringUtil.TR("QueueDuoRanked", "RankMode"));
			break;
		case UIRankDisplayType.FullTeam:
			SetQueueButtonLabel(StringUtil.TR("QueueTeamRanked", "RankMode"));
			break;
		default:
			SetQueueButtonLabel(StringUtil.TR("UnableToQueue", "RankMode"));
			break;
		}
		CheckQueueButtonStatus();
	}

	private void CheckQueueButtonStatus()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		bool flag = clientGameManager.MeetsAllQueueRequirements(GameType.Ranked);
		int num = clientGameManager.GroupInfo.Members.Count;
		if (num < 1)
		{
			num = 1;
		}
		bool flag2 = clientGameManager.MeetsGroupSizeRequirement(GameType.Ranked, num);
		bool flag3 = ServerAllowQueueRank();
		int num2;
		if (clientGameManager.GameTypeAvailabilies.ContainsKey(GameType.Ranked))
		{
			DateTime? penaltyTimeout = clientGameManager.GameTypeAvailabilies[GameType.Ranked].PenaltyTimeout;
			if (penaltyTimeout.HasValue)
			{
				num2 = ((DateTime.UtcNow < clientGameManager.GameTypeAvailabilies[GameType.Ranked].PenaltyTimeout.Value) ? 1 : 0);
				goto IL_00bb;
			}
		}
		num2 = 0;
		goto IL_00bb;
		IL_01c8:
		int num3;
		bool flag4 = (byte)num3 != 0;
		bool flag5;
		bool flag6;
		if (!flag4)
		{
			if (!flag3)
			{
				m_cannotQueue = RankedQueueRejectionReasons.ServerDisabled;
			}
			else if (!flag2)
			{
				m_cannotQueue = RankedQueueRejectionReasons.IncorrectGroupSize;
			}
			else if (!flag)
			{
				m_cannotQueue = RankedQueueRejectionReasons.DoNotMeetRequirements;
			}
			else if (flag5)
			{
				m_cannotQueue = RankedQueueRejectionReasons.PenaltyTimeout;
			}
			else if (flag6)
			{
				m_cannotQueue = RankedQueueRejectionReasons.GroupTimeout;
			}
		}
		else
		{
			m_cannotQueue = RankedQueueRejectionReasons.None;
		}
		SetQueueButtonClickable(flag4);
		ForceQueueButtonCallback(m_cannotQueue != RankedQueueRejectionReasons.None);
		return;
		IL_00bb:
		flag5 = ((byte)num2 != 0);
		flag6 = false;
		if (clientGameManager.GroupInfo.InAGroup)
		{
			int num4 = 0;
			while (true)
			{
				if (num4 < clientGameManager.GroupInfo.Members.Count)
				{
					DateTime? penaltyTimeout2 = clientGameManager.GroupInfo.Members[num4].PenaltyTimeout;
					if (penaltyTimeout2.HasValue)
					{
						if (DateTime.UtcNow < clientGameManager.GroupInfo.Members[num4].PenaltyTimeout.Value)
						{
							flag6 = true;
							break;
						}
					}
					num4++;
					continue;
				}
				break;
			}
		}
		if (m_selectedQueueType != UIRankDisplayType.None)
		{
			if (flag)
			{
				if (flag2)
				{
					if (flag3)
					{
						if (!flag5)
						{
							num3 = ((!flag6) ? 1 : 0);
							goto IL_01c8;
						}
					}
				}
			}
		}
		num3 = 0;
		goto IL_01c8;
	}

	private void ForceQueueButtonCallback(bool forceCallback)
	{
		m_startQueueBtn.spriteController.SetForceHovercallback(forceCallback);
		m_startQueueBtn.spriteController.SetForceExitCallback(forceCallback);
	}

	private void SetQueueButtonClickable(bool setQueueButtonClickable)
	{
		m_startQueueBtn.spriteController.SetClickable(setQueueButtonClickable);
		UIManager.SetGameObjectActive(m_startQueueBtn.spriteController.m_defaultImage, setQueueButtonClickable);
		UIManager.SetGameObjectActive(m_startQueueBtn.spriteController.m_hoverImage, setQueueButtonClickable);
		UIManager.SetGameObjectActive(m_startQueueBtn.spriteController.m_pressedImage, setQueueButtonClickable);
	}

	private bool QueueButtonTooltipSetup(UITooltipBase tooltip)
	{
		if (!m_startQueueBtn.spriteController.IsClickable())
		{
			UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
			if (m_cannotQueue == RankedQueueRejectionReasons.DoNotMeetRequirements)
			{
				while (true)
				{
					ClientGameManager clientGameManager = ClientGameManager.Get();
					LocalizationPayload blockingQueueRestriction = clientGameManager.GetBlockingQueueRestriction(GameType.Ranked);
					string text;
					if (blockingQueueRestriction == null)
					{
						text = StringUtil.TR("UnknownError", "Global");
					}
					else
					{
						text = blockingQueueRestriction.ToString();
					}
					string tooltipText = text;
					uITitledTooltip.Setup(StringUtil.TR("DoNotMeetRequirements", "Ranked"), tooltipText, string.Empty);
					return true;
				}
			}
			if (m_cannotQueue == RankedQueueRejectionReasons.IncorrectGroupSize)
			{
				while (true)
				{
					int num = ClientGameManager.Get().GroupInfo.Members.Count;
					if (num < 1)
					{
						num = 1;
					}
					LocalizationPayload localizationPayload = ClientGameManager.Get().GetReasonGroupSizeCantQueue(GameType.Ranked, num);
					if (localizationPayload == null)
					{
						localizationPayload = LocalizationPayload.Create("BadGroupSizeForQueue", "Matchmaking", LocalizationArg_Int32.Create(num));
					}
					uITitledTooltip.Setup(StringUtil.TR("IncorrectGroupSizeTitle", "Ranked"), localizationPayload.ToString(), string.Empty);
					return true;
				}
			}
			if (m_cannotQueue == RankedQueueRejectionReasons.ServerDisabled)
			{
				while (true)
				{
					uITitledTooltip.Setup(StringUtil.TR("Disabled", "Global"), string.Format(StringUtil.TR("RankedModeDisabled", "Ranked")), string.Empty);
					return true;
				}
			}
			if (m_cannotQueue == RankedQueueRejectionReasons.PenaltyTimeout)
			{
				while (true)
				{
					ClientGameManager clientGameManager2 = ClientGameManager.Get();
					TimeSpan difference = clientGameManager2.GameTypeAvailabilies[GameType.Ranked].PenaltyTimeout.Value - DateTime.UtcNow;
					string timeDifferenceText = StringUtil.GetTimeDifferenceText(difference);
					uITitledTooltip.Setup(StringUtil.TR("YouHaveBeenPenalized", "Ranked"), string.Format(StringUtil.TR("CannotQueueUntilTimeout", "Ranked"), timeDifferenceText), string.Empty);
					return true;
				}
			}
			if (m_cannotQueue == RankedQueueRejectionReasons.GroupTimeout)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
					{
						string text2 = string.Empty;
						for (int i = 0; i < ClientGameManager.Get().GroupInfo.Members.Count; i++)
						{
							DateTime? penaltyTimeout = ClientGameManager.Get().GroupInfo.Members[i].PenaltyTimeout;
							if (penaltyTimeout.HasValue)
							{
								text2 = new StringBuilder().Append(text2).Append(ClientGameManager.Get().GroupInfo.Members[i].MemberDisplayName).Append(" ").ToString();
							}
						}
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								uITitledTooltip.Setup(StringUtil.TR("GroupMembersPenalized", "Ranked"), string.Format(StringUtil.TR("CannotQueueMembersPenalized", "Ranked"), text2), string.Empty);
								return true;
							}
						}
					}
					}
				}
			}
		}
		return false;
	}

	public void SetQueueButtonLabel(string text)
	{
		for (int i = 0; i < m_queueButtonLabels.Length; i++)
		{
			m_queueButtonLabels[i].text = text;
		}
		while (true)
		{
			return;
		}
	}

	private List<UIRankingDisplayEntry> GetRankingEntries(List<RankedScoreboardEntry> entries, int groupSize)
	{
		List<UIRankingDisplayEntry> list = new List<UIRankingDisplayEntry>();
		if (!entries.IsNullOrEmpty())
		{
			for (int i = 0; i < entries.Count; i++)
			{
				UIRankingDisplayEntry item = new UIRankingDisplayEntry(entries[i], groupSize);
				list.Add(item);
			}
		}
		return list;
	}

	public void ProcessTierInfoPerGroupSize(Dictionary<int, PerGroupSizeTierInfo> tierInfoPerGroupSize)
	{
		if (s_tierInfoPerGroupSize != null)
		{
			using (Dictionary<int, PerGroupSizeTierInfo>.Enumerator enumerator = s_tierInfoPerGroupSize.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, PerGroupSizeTierInfo> current = enumerator.Current;
					if (current.Value.OurEntry.HasValue)
					{
						int key = current.Key;
						RankedScoreboardEntry value = current.Value.OurEntry.Value;
						int tier = value.Tier;
						PerGroupSizeTierInfo value2;
						if (tierInfoPerGroupSize.TryGetValue(key, out value2) && value2.OurEntry.HasValue)
						{
							string text = null;
							RankedScoreboardEntry value3 = value2.OurEntry.Value;
							int tier2 = value3.Tier;
							if (tier < 1 && tier2 != tier)
							{
								text = "TierPostPlacement";
							}
							else if (tier2 < tier)
							{
								text = "TierRaised";
							}
							else if (tier2 > tier)
							{
								text = "TierLowered";
							}
							if (text != null)
							{
								string[] array = new string[4]
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
								});
							}
						}
					}
				}
			}
		}
		s_tierInfoPerGroupSize = tierInfoPerGroupSize;
		if (!m_isVisible)
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < m_rankDisplays.Length; i++)
			{
				m_rankDisplays[i].Setup((UIRankDisplayType)i, s_tierInfoPerGroupSize);
			}
			UIRankDisplayType tab = UIRankDisplayType.Solo;
			if (ClientGameManager.Get().GroupInfo.Members.Count == 2)
			{
				tab = UIRankDisplayType.Duo;
			}
			else if (ClientGameManager.Get().GroupInfo.Members.Count == 4)
			{
				tab = UIRankDisplayType.FullTeam;
			}
			OpenTab(tab);
			HandleGroupUpdateNotification();
			return;
		}
	}

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(m_selectScreenContainer, visible);
		bool flag = m_isVisible != visible;
		m_isVisible = visible;
		if (!visible)
		{
			return;
		}
		if (flag)
		{
			if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
			{
				UIManager.SetGameObjectActive(m_rankRewardDisabledNotice, !GameManager.Get().GameplayOverrides.RankedUpdatesEnabled);
				m_rankRewardDisabledNotice.text = StringUtil.TR("RankRewardsDisabledDescription", "OverlayScreensScene");
			}
			else
			{
				UIManager.SetGameObjectActive(m_rankRewardDisabledNotice, false);
			}
			for (int i = 0; i < m_rankDisplays.Length; i++)
			{
				UIManager.SetGameObjectActive(m_rankDisplays[i].m_selectedQueueRankContainer, false);
				UIManager.SetGameObjectActive(m_rankDisplays[i].m_InPlacementMatchesContainer, false);
				UIManager.SetGameObjectActive(m_rankDisplays[i].m_HasRankAlreadyContainer, false);
			}
			if (!m_loadedData)
			{
				m_soloRankTabButton.spriteController.SetClickable(false);
				m_duoRankTabButton.spriteController.SetClickable(false);
				m_teamRankTabButton.spriteController.SetClickable(false);
				m_rewardTabButton.spriteController.SetClickable(false);
				m_filterListDropdownBtn.spriteController.SetClickable(false);
				m_InstanceLabel.text = string.Empty;
			}
			DisplayLoading(true);
			ClientGameManager.Get().RequestRankedLeaderboardOverview(GameType.Ranked, delegate(RankedLeaderboardOverviewResponse overviewResponse)
			{
				DisplayLoading(false);
				if (overviewResponse.Success)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							m_loadedData = true;
							m_soloRankTabButton.spriteController.SetClickable(true);
							m_duoRankTabButton.spriteController.SetClickable(true);
							m_teamRankTabButton.spriteController.SetClickable(true);
							m_rewardTabButton.spriteController.SetClickable(true);
							m_filterListDropdownBtn.spriteController.SetClickable(true);
							ProcessTierInfoPerGroupSize(overviewResponse.TierInfoPerGroupSize);
							return;
						}
					}
				}
				LobbyGameClientInterface lobbyInterface = ClientGameManager.Get().LobbyInterface;
				if (lobbyInterface != null)
				{
					lobbyInterface.WriteErrorToConsole(overviewResponse.LocalizedFailure, overviewResponse.ErrorMessage);
				}
				Log.Error(new StringBuilder().Append("Failed to load overall Ranked Leaderboard info: ").Append(overviewResponse.ErrorMessage).ToString());
			});
		}
		HandleGroupUpdateNotification();
	}

	public static IDataEntry RankingEntryToDataEntry(UIRankingDisplayEntry entry)
	{
		return entry;
	}

	public void UpdateReadyButton(bool shouldBeOn)
	{
		if (shouldBeOn)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					CheckQueueButtonStatus();
					return;
				}
			}
		}
		SetQueueButtonClickable(false);
	}

	public void StartQueueBtnClicked(BaseEventData data)
	{
		UICharacterSelectScreenController.Get().DoReadyClick(FrontEndButtonSounds.RankQueueButtonClick);
	}

	private void Update()
	{
		if (!Input.GetMouseButtonDown(0))
		{
			return;
		}
		while (true)
		{
			bool flag = true;
			if (EventSystem.current != null)
			{
				if (EventSystem.current.IsPointerOverGameObject(-1))
				{
					StandaloneInputModuleWithEventDataAccess component = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
					if (component != null && component.GetLastPointerEventDataPublic(-1).pointerEnter != null)
					{
						UIMainMenu componentInParent = component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<UIMainMenu>();
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
										if (!(componentInParent2 == m_filterListDropdownBtn))
										{
											if (!(componentInParent2 == m_RankDropdownDivision))
											{
												if (!(componentInParent2 == m_RankDropdownFriends))
												{
													if (!(componentInParent2 == m_RankDropdownTopPlayers))
													{
														componentInParent2 = componentInParent2.transform.parent.GetComponentInParent<_SelectableBtn>();
														continue;
													}
												}
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
								goto IL_01b1;
							}
						}
						flag = false;
					}
				}
			}
			goto IL_01b1;
			IL_01b1:
			if (flag)
			{
				DoFilterDropdownVisible(false);
			}
			return;
		}
	}

	public static string GetTierIconResource(int tier)
	{
		if (tier > 0)
		{
			ClientGameManager clientGameManager = ClientGameManager.Get();
			if (clientGameManager != null)
			{
				GameType key = GameType.Ranked;
				GameTypeAvailability value;
				if (clientGameManager.GameTypeAvailabilies.TryGetValue(key, out value))
				{
					if (tier <= value.PerTierDefinitions.Count)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
							{
								TierDefinitions tierDefinitions = value.PerTierDefinitions[tier - 1];
								return tierDefinitions.IconResource;
							}
							}
						}
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
			ClientGameManager clientGameManager = ClientGameManager.Get();
			if (clientGameManager != null)
			{
				GameType key = GameType.Ranked;
				GameTypeAvailability value;
				if (clientGameManager.GameTypeAvailabilies.TryGetValue(key, out value))
				{
					if (tier <= value.PerTierDefinitions.Count)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
							{
								TierDefinitions tierDefinitions = value.PerTierDefinitions[tier - 1];
								return tierDefinitions.IsRachet;
							}
							}
						}
					}
				}
			}
		}
		return false;
	}

	public void GetTierLocalizedName(int tier, int instanceId, int groupSize, out string tierName, out string instanceName)
	{
		instanceName = null;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (tier >= 2)
					{
						instanceName = clientGameManager.GetTierInstanceName(instanceId);
					}
					tierName = clientGameManager.GetTierName(GameType.Ranked, tier);
					return;
				}
			}
		}
		tierName = "[BadCGM]";
	}
}
