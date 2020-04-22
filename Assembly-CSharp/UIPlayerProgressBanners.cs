using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayerProgressBanners : UIPlayerProgressSubPanel
{
	public enum CurrentList
	{
		Foreground,
		Background,
		Title,
		Ribbon
	}

	private class SortedItem<T>
	{
		public T Payload;

		public int SortOrder;

		public SortedItem(T payload, int sortOrder)
		{
			Payload = payload;
			SortOrder = sortOrder;
		}
	}

	public _ButtonSwapSprite m_foregroundBtn;

	public _ButtonSwapSprite m_backgroundBtn;

	public _ButtonSwapSprite m_titlesBtn;

	public _ButtonSwapSprite m_ribbonsBtn;

	public GridLayoutGroup m_pageGrid;

	public GridLayoutGroup m_bannerGrid;

	public GridLayoutGroup m_emblemGrid;

	public GridLayoutGroup m_titleGrid;

	public GridLayoutGroup m_ribbonGrid;

	public Toggle m_ownedToggle;

	public _ButtonSwapSprite m_prevPage;

	public _ButtonSwapSprite m_nextPage;

	public UIPageIndicator m_pageIndicatorPrefab;

	private List<UIPageIndicator> m_pageIndicators;

	private List<UIPlayerProgressBannersButton> m_titleButtons;

	private List<UIPlayerProgressBannersButton> m_emblemButtons;

	private List<UIPlayerProgressBannersButton> m_bannerButtons;

	private List<UIPlayerProgressBannersButton> m_ribbonButtons;

	private int m_currentPage;

	private int m_numItems;

	private bool initialized;

	private bool m_showLocked = true;

	private int numBannersPerPage;

	private CurrentList m_currentList;

	private List<GameBalanceVars.PlayerTitle> m_visibleTitles;

	private List<GameBalanceVars.PlayerBanner> m_visibleBackgroundBanners;

	private List<GameBalanceVars.PlayerBanner> m_visibleForegroundBanners;

	private List<GameBalanceVars.PlayerRibbon> m_visibleRibbons;

	private List<GameBalanceVars.PlayerTitle> m_unlockedTitles;

	private List<GameBalanceVars.PlayerBanner> m_unlockedBackgroundBanners;

	private List<GameBalanceVars.PlayerBanner> m_unlockedForegroundBanners;

	private List<GameBalanceVars.PlayerRibbon> m_unlockedRibbons;

	private void Start()
	{
		if (HitchDetector.Get() != null)
		{
			HitchDetector.Get().AddNewLayoutGroup(m_bannerGrid);
			HitchDetector.Get().AddNewLayoutGroup(m_emblemGrid);
			HitchDetector.Get().AddNewLayoutGroup(m_titleGrid);
			HitchDetector.Get().AddNewLayoutGroup(m_ribbonGrid);
		}
		Init();
		m_prevPage.callback = ClickedOnPrevPage;
		m_nextPage.callback = ClickedOnNextPage;
		m_foregroundBtn.callback = ForegroundBtnClicked;
		m_backgroundBtn.callback = BackgroundBtnClicked;
		m_titlesBtn.callback = TitlesBtnClicked;
		m_ribbonsBtn.callback = RibbonsBtnClicked;
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.Scroll, OnScroll);
		UIEventTriggerUtils.AddListener(m_prevPage.gameObject, EventTriggerType.Scroll, OnScroll);
		UIEventTriggerUtils.AddListener(m_nextPage.gameObject, EventTriggerType.Scroll, OnScroll);
		ClientGameManager.Get().OnAccountDataUpdated += OnAccountDataUpdate;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
			OnAccountDataUpdate(ClientGameManager.Get().GetPlayerAccountData());
		}
		UITooltipHoverObject component = m_ownedToggle.GetComponent<UITooltipHoverObject>();
		component.Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
		{
			UISimpleTooltip uISimpleTooltip = (UISimpleTooltip)tooltip;
			uISimpleTooltip.Setup(StringUtil.TR("Owned", "Store"));
			return true;
		});
		m_ownedToggle.onValueChanged.AddListener(OnOwnedCheckedChanged);
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnAccountDataUpdated -= OnAccountDataUpdate;
		}
	}

	private void OnAccountDataUpdate(PersistedAccountData accountData)
	{
		UpdateVisibleItems(accountData.AccountComponent);
		int currentPage = m_currentPage;
		ShowList(m_currentList);
		m_currentPage = currentPage;
		ResetPage();
	}

	private List<T> SortList<T>(List<T> listToSort, List<int> sortOrders)
	{
		List<T> list = new List<T>();
		List<SortedItem<T>> list2 = new List<SortedItem<T>>();
		for (int i = 0; i < listToSort.Count; i++)
		{
			list2.Add(new SortedItem<T>(listToSort[i], sortOrders[i] * listToSort.Count + i));
		}
		list2.Sort(delegate(SortedItem<T> one, SortedItem<T> two)
		{
			if (one == null && two == null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return 0;
					}
				}
			}
			if (one == null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return -1;
					}
				}
			}
			if (two == null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return 1;
					}
				}
			}
			return one.SortOrder.CompareTo(two.SortOrder);
		});
		for (int j = 0; j < list2.Count; j++)
		{
			list.Add(list2[j].Payload);
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return list;
		}
	}

	private void OnOwnedCheckedChanged(bool isChecked)
	{
		m_showLocked = !isChecked;
		ShowList(m_currentList);
		m_currentPage = 0;
		ResetPage();
	}

	private void UpdateVisibleItems(AccountComponent accountComponent)
	{
		int num;
		if (GameManager.Get() != null)
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
			num = (GameManager.Get().GameplayOverrides.EnableHiddenCharacters ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		if (flag)
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
			m_visibleTitles = new List<GameBalanceVars.PlayerTitle>(GameBalanceVars.Get().PlayerTitles);
		}
		else
		{
			m_visibleTitles = new List<GameBalanceVars.PlayerTitle>();
			GameBalanceVars.PlayerTitle[] playerTitles = GameBalanceVars.Get().PlayerTitles;
			foreach (GameBalanceVars.PlayerTitle playerTitle in playerTitles)
			{
				if (!accountComponent.IsTitleUnlocked(playerTitle))
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
					CharacterType unlockCharacterType = playerTitle.GetUnlockCharacterType();
					if (playerTitle.m_isHidden)
					{
						continue;
					}
					if (unlockCharacterType != 0)
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
						if (GameWideData.Get().GetCharacterResourceLink(unlockCharacterType).m_isHidden)
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
							continue;
						}
					}
					if (!GameBalanceVarsExtensions.MeetsVisibilityConditions(playerTitle))
					{
						continue;
					}
				}
				m_visibleTitles.Add(playerTitle);
			}
		}
		m_visibleBackgroundBanners = new List<GameBalanceVars.PlayerBanner>();
		m_visibleForegroundBanners = new List<GameBalanceVars.PlayerBanner>();
		GameBalanceVars.PlayerBanner[] playerBanners = GameBalanceVars.Get().PlayerBanners;
		foreach (GameBalanceVars.PlayerBanner playerBanner in playerBanners)
		{
			if (!flag && !accountComponent.IsBannerUnlocked(playerBanner))
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
				CharacterType unlockCharacterType2 = playerBanner.GetUnlockCharacterType();
				if (playerBanner.m_isHidden)
				{
					continue;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (unlockCharacterType2 != 0)
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
					if (GameWideData.Get().GetCharacterResourceLink(unlockCharacterType2).m_isHidden)
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
						continue;
					}
				}
				if (!GameBalanceVarsExtensions.MeetsVisibilityConditions(playerBanner))
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					continue;
				}
			}
			if (playerBanner.m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
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
				m_visibleBackgroundBanners.Add(playerBanner);
			}
			else
			{
				m_visibleForegroundBanners.Add(playerBanner);
			}
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			m_visibleRibbons = new List<GameBalanceVars.PlayerRibbon>();
			GameBalanceVars.PlayerRibbon[] playerRibbons = GameBalanceVars.Get().PlayerRibbons;
			foreach (GameBalanceVars.PlayerRibbon playerRibbon in playerRibbons)
			{
				if (!accountComponent.IsRibbonUnlocked(playerRibbon))
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
					if (!flag)
					{
						CharacterType unlockCharacterType3 = playerRibbon.GetUnlockCharacterType();
						if (playerRibbon.m_isHidden)
						{
							continue;
						}
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (unlockCharacterType3 != 0 && GameWideData.Get().GetCharacterResourceLink(unlockCharacterType3).m_isHidden)
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
							continue;
						}
						if (!GameBalanceVarsExtensions.MeetsVisibilityConditions(playerRibbon))
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
							continue;
						}
					}
				}
				if (FactionWideData.Get().IsRibbonInCompetition(playerRibbon.ID, ClientGameManager.Get().ActiveFactionCompetition))
				{
					m_visibleRibbons.Add(playerRibbon);
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				List<int> list = new List<int>();
				for (int l = 0; l < m_visibleTitles.Count; l++)
				{
					list.Add(m_visibleTitles[l].m_sortOrder);
				}
				m_visibleTitles = SortList(m_visibleTitles, list);
				list.Clear();
				for (int m = 0; m < m_visibleBackgroundBanners.Count; m++)
				{
					list.Add(m_visibleBackgroundBanners[m].m_sortOrder);
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					m_visibleBackgroundBanners = SortList(m_visibleBackgroundBanners, list);
					list.Clear();
					for (int n = 0; n < m_visibleForegroundBanners.Count; n++)
					{
						list.Add(m_visibleForegroundBanners[n].m_sortOrder);
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						m_visibleForegroundBanners = SortList(m_visibleForegroundBanners, list);
						list.Clear();
						for (int num2 = 0; num2 < m_visibleRibbons.Count; num2++)
						{
							list.Add(m_visibleRibbons[num2].m_sortOrder);
						}
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							m_visibleRibbons = SortList(m_visibleRibbons, list);
							m_visibleTitles.Insert(0, null);
							m_visibleRibbons.Insert(0, null);
							m_unlockedTitles = new List<GameBalanceVars.PlayerTitle>();
							foreach (GameBalanceVars.PlayerTitle visibleTitle in m_visibleTitles)
							{
								if (visibleTitle != null)
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
									if (!ClientGameManager.Get().IsTitleUnlocked(visibleTitle))
									{
										continue;
									}
								}
								m_unlockedTitles.Add(visibleTitle);
							}
							m_unlockedBackgroundBanners = new List<GameBalanceVars.PlayerBanner>();
							List<GameBalanceVars.UnlockConditionValue> unlockConditionValues;
							foreach (GameBalanceVars.PlayerBanner visibleBackgroundBanner in m_visibleBackgroundBanners)
							{
								if (visibleBackgroundBanner != null)
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
									if (!ClientGameManager.Get().IsBannerUnlocked(visibleBackgroundBanner, out unlockConditionValues))
									{
										continue;
									}
									while (true)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
								}
								m_unlockedBackgroundBanners.Add(visibleBackgroundBanner);
							}
							m_unlockedForegroundBanners = new List<GameBalanceVars.PlayerBanner>();
							using (List<GameBalanceVars.PlayerBanner>.Enumerator enumerator3 = m_visibleForegroundBanners.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									GameBalanceVars.PlayerBanner current3 = enumerator3.Current;
									if (current3 != null)
									{
										if (!ClientGameManager.Get().IsBannerUnlocked(current3, out unlockConditionValues))
										{
											continue;
										}
										while (true)
										{
											switch (1)
											{
											case 0:
												continue;
											}
											break;
										}
									}
									m_unlockedForegroundBanners.Add(current3);
								}
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
							}
							m_unlockedRibbons = new List<GameBalanceVars.PlayerRibbon>();
							using (List<GameBalanceVars.PlayerRibbon>.Enumerator enumerator4 = m_visibleRibbons.GetEnumerator())
							{
								while (enumerator4.MoveNext())
								{
									GameBalanceVars.PlayerRibbon current4 = enumerator4.Current;
									if (current4 != null)
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
										if (!ClientGameManager.Get().IsRibbonUnlocked(current4, out unlockConditionValues))
										{
											continue;
										}
										while (true)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
									}
									m_unlockedRibbons.Add(current4);
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
						}
					}
				}
			}
		}
	}

	private void Init()
	{
		if (initialized)
		{
			return;
		}
		m_pageIndicators = new List<UIPageIndicator>(m_pageGrid.GetComponentsInChildren<UIPageIndicator>(true));
		m_bannerButtons = new List<UIPlayerProgressBannersButton>(m_bannerGrid.GetComponentsInChildren<UIPlayerProgressBannersButton>(true));
		m_titleButtons = new List<UIPlayerProgressBannersButton>(m_titleGrid.GetComponentsInChildren<UIPlayerProgressBannersButton>(true));
		m_ribbonButtons = new List<UIPlayerProgressBannersButton>(m_ribbonGrid.GetComponentsInChildren<UIPlayerProgressBannersButton>(true));
		m_emblemButtons = new List<UIPlayerProgressBannersButton>(m_emblemGrid.GetComponentsInChildren<UIPlayerProgressBannersButton>(true));
		initialized = true;
		List<UIPlayerProgressBannersButton> list = m_bannerButtons.Concat(m_titleButtons).Concat(m_emblemButtons).Concat(m_ribbonButtons)
			.ToList();
		for (int i = 0; i < list.Count; i++)
		{
			list[i].m_selectableButton.spriteController.RegisterScrollListener(OnScroll);
			UIManager.SetGameObjectActive(list[i], false);
			StaggerComponent.SetStaggerComponent(list[i].gameObject, true);
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UpdateVisibleItems(ClientGameManager.Get().GetPlayerAccountData().AccountComponent);
			return;
		}
	}

	private List<UIPlayerProgressBannersButton> GetCurrentButtonsList()
	{
		switch (m_currentList)
		{
		case CurrentList.Foreground:
			return m_emblemButtons;
		case CurrentList.Background:
			return m_bannerButtons;
		case CurrentList.Title:
			return m_titleButtons;
		case CurrentList.Ribbon:
			return m_ribbonButtons;
		default:
			return null;
		}
	}

	private void OnScroll(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		Vector2 scrollDelta = pointerEventData.scrollDelta;
		if (scrollDelta.y > 0f)
		{
			ClickedOnPrevPage(data);
			return;
		}
		Vector2 scrollDelta2 = pointerEventData.scrollDelta;
		if (!(scrollDelta2.y < 0f))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ClickedOnNextPage(data);
			return;
		}
	}

	public void BannerClicked(UIPlayerProgressBannersButton btnClicked)
	{
		if (!btnClicked.m_unlocked)
		{
			return;
		}
		List<UIPlayerProgressBannersButton> currentButtonsList = GetCurrentButtonsList();
		for (int i = 0; i < currentButtonsList.Count; i++)
		{
			if (btnClicked == currentButtonsList[i])
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				currentButtonsList[i].SetSelected(true);
			}
			else
			{
				currentButtonsList[i].SetSelected(false);
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

	public void ShowList(CurrentList listType)
	{
		m_currentList = listType;
		m_numItems = GetNumberOfItems(listType);
		numBannersPerPage = GetCurrentButtonsList().Count;
		int num = Mathf.CeilToInt((float)m_numItems / (float)numBannersPerPage);
		while (m_pageIndicators.Count < num)
		{
			UIPageIndicator uIPageIndicator = UnityEngine.Object.Instantiate(m_pageIndicatorPrefab);
			uIPageIndicator.transform.SetParent(m_pageGrid.transform);
			uIPageIndicator.transform.localEulerAngles = Vector3.zero;
			uIPageIndicator.transform.localPosition = Vector3.zero;
			uIPageIndicator.transform.localScale = Vector3.one;
			UIEventTriggerUtils.AddListener(uIPageIndicator.m_hitbox.gameObject, EventTriggerType.Scroll, OnScroll);
			uIPageIndicator.SetSelected(false);
			m_pageIndicators.Add(uIPageIndicator);
		}
		while (m_pageIndicators.Count > num)
		{
			UnityEngine.Object.Destroy(m_pageIndicators[0].gameObject);
			m_pageIndicators.RemoveAt(0);
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_pageIndicators.Count > 0)
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
				ClickedOnPageIndicator(m_pageIndicators[0]);
				for (int i = 0; i < m_pageIndicators.Count; i++)
				{
					m_pageIndicators[i].SetPageNumber(i + 1);
					m_pageIndicators[i].SetClickCallback(UIPlayerProgressPanel.Get().ClickedOnPage);
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			else
			{
				ClickedOnPageIndicator(null);
			}
			bool doActive = false;
			bool doActive2 = false;
			bool doActive3 = false;
			bool doActive4 = false;
			m_nextPage.transform.parent.SetAsLastSibling();
			switch (m_currentList)
			{
			case CurrentList.Foreground:
				m_foregroundBtn.selectableButton.SetSelected(true, false, string.Empty, string.Empty);
				m_backgroundBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
				m_titlesBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
				m_ribbonsBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
				doActive = true;
				break;
			case CurrentList.Background:
				m_foregroundBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
				m_backgroundBtn.selectableButton.SetSelected(true, false, string.Empty, string.Empty);
				m_titlesBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
				m_ribbonsBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
				doActive2 = true;
				break;
			case CurrentList.Title:
				m_foregroundBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
				m_backgroundBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
				m_titlesBtn.selectableButton.SetSelected(true, false, string.Empty, string.Empty);
				m_ribbonsBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
				doActive3 = true;
				break;
			case CurrentList.Ribbon:
				m_foregroundBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
				m_backgroundBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
				m_titlesBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
				m_ribbonsBtn.selectableButton.SetSelected(true, false, string.Empty, string.Empty);
				doActive4 = true;
				break;
			}
			UIManager.SetGameObjectActive(m_emblemGrid, doActive);
			UIManager.SetGameObjectActive(m_bannerGrid, doActive2);
			UIManager.SetGameObjectActive(m_titleGrid, doActive3);
			UIManager.SetGameObjectActive(m_ribbonGrid, doActive4);
			return;
		}
	}

	public void ForegroundBtnClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
		ShowList(CurrentList.Foreground);
	}

	public void BackgroundBtnClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
		ShowList(CurrentList.Background);
	}

	public void TitlesBtnClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
		ShowList(CurrentList.Title);
	}

	public void RibbonsBtnClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
		ShowList(CurrentList.Ribbon);
	}

	public void ClickedOnPrevPage(BaseEventData data)
	{
		if (m_currentPage <= 0)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_currentPage--;
			object pageIndicator;
			if (m_pageIndicators.Count > m_currentPage)
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
				pageIndicator = m_pageIndicators[m_currentPage];
			}
			else
			{
				pageIndicator = null;
			}
			ClickedOnPageIndicator((UIPageIndicator)pageIndicator);
			return;
		}
	}

	public void ClickedOnNextPage(BaseEventData data)
	{
		if (m_currentPage + 1 >= m_pageIndicators.Count)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_currentPage++;
			object pageIndicator;
			if (m_pageIndicators.Count > m_currentPage)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				pageIndicator = m_pageIndicators[m_currentPage];
			}
			else
			{
				pageIndicator = null;
			}
			ClickedOnPageIndicator((UIPageIndicator)pageIndicator);
			return;
		}
	}

	public int GetNumberOfItems(CurrentList listType)
	{
		if (listType == CurrentList.Title)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (m_showLocked)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								return m_visibleTitles.Count;
							}
						}
					}
					return m_unlockedTitles.Count;
				}
			}
		}
		switch (listType)
		{
		case CurrentList.Background:
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (m_showLocked)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return m_visibleBackgroundBanners.Count;
						}
					}
				}
				return m_unlockedBackgroundBanners.Count;
			}
		case CurrentList.Foreground:
			if (m_showLocked)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return m_visibleForegroundBanners.Count;
					}
				}
			}
			return m_unlockedForegroundBanners.Count;
		case CurrentList.Ribbon:
			if (m_showLocked)
			{
				return m_visibleRibbons.Count;
			}
			return m_unlockedRibbons.Count;
		default:
			return 0;
		}
	}

	private bool IsSelectedIndex(int i)
	{
		return i == 0;
	}

	public void ResetPage()
	{
		object pageIndicator;
		if (m_pageIndicators.Count > m_currentPage)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			pageIndicator = m_pageIndicators[m_currentPage];
		}
		else
		{
			pageIndicator = null;
		}
		ClickedOnPageIndicator((UIPageIndicator)pageIndicator);
	}

	public override void ClickedOnPageIndicator(UIPageIndicator pageIndicator)
	{
		for (int i = 0; i < m_pageIndicators.Count; i++)
		{
			if (m_pageIndicators[i] == pageIndicator)
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
				m_pageIndicators[i].SetSelected(true);
				m_currentPage = i;
			}
			else
			{
				m_pageIndicators[i].SetSelected(false);
			}
		}
		List<UIPlayerProgressBannersButton> currentButtonsList = GetCurrentButtonsList();
		for (int j = 0; j < currentButtonsList.Count; j++)
		{
			int num = m_currentPage * numBannersPerPage + j;
			if (num < m_numItems)
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
				if (m_currentList == CurrentList.Title)
				{
					if (m_showLocked)
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
						currentButtonsList[j].SetupTitle(m_visibleTitles[num]);
					}
					else
					{
						currentButtonsList[j].SetupTitle(m_unlockedTitles[num]);
					}
					continue;
				}
				if (m_currentList == CurrentList.Background)
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
					if (m_showLocked)
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
						currentButtonsList[j].SetupBanner(m_visibleBackgroundBanners[num]);
					}
					else
					{
						currentButtonsList[j].SetupBanner(m_unlockedBackgroundBanners[num]);
					}
					continue;
				}
				if (m_currentList == CurrentList.Foreground)
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
					if (m_showLocked)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						currentButtonsList[j].SetupBanner(m_visibleForegroundBanners[num]);
					}
					else
					{
						currentButtonsList[j].SetupBanner(m_unlockedForegroundBanners[num]);
					}
					continue;
				}
				if (m_currentList != CurrentList.Ribbon)
				{
					throw new Exception(string.Concat(m_currentList, " not supported"));
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_showLocked)
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
					currentButtonsList[j].SetupRibbon(m_visibleRibbons[num]);
				}
				else
				{
					currentButtonsList[j].SetupRibbon(m_unlockedRibbons[num]);
				}
			}
			else
			{
				currentButtonsList[j].SetupBanner(null);
			}
		}
		while (true)
		{
			switch (6)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public override void SetActive(bool visible)
	{
		base.SetActive(visible);
		Init();
		if (!visible)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		ShowList(CurrentList.Foreground);
	}
}
