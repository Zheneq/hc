using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayerProgressBanners : UIPlayerProgressSubPanel
{
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

	private UIPlayerProgressBanners.CurrentList m_currentList;

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
			HitchDetector.Get().AddNewLayoutGroup(this.m_bannerGrid);
			HitchDetector.Get().AddNewLayoutGroup(this.m_emblemGrid);
			HitchDetector.Get().AddNewLayoutGroup(this.m_titleGrid);
			HitchDetector.Get().AddNewLayoutGroup(this.m_ribbonGrid);
		}
		this.Init();
		this.m_prevPage.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ClickedOnPrevPage);
		this.m_nextPage.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ClickedOnNextPage);
		this.m_foregroundBtn.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ForegroundBtnClicked);
		this.m_backgroundBtn.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BackgroundBtnClicked);
		this.m_titlesBtn.callback = new _ButtonSwapSprite.ButtonClickCallback(this.TitlesBtnClicked);
		this.m_ribbonsBtn.callback = new _ButtonSwapSprite.ButtonClickCallback(this.RibbonsBtnClicked);
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.Scroll, new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		UIEventTriggerUtils.AddListener(this.m_prevPage.gameObject, EventTriggerType.Scroll, new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		UIEventTriggerUtils.AddListener(this.m_nextPage.gameObject, EventTriggerType.Scroll, new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		ClientGameManager.Get().OnAccountDataUpdated += this.OnAccountDataUpdate;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressBanners.Start()).MethodHandle;
			}
			this.OnAccountDataUpdate(ClientGameManager.Get().GetPlayerAccountData());
		}
		UITooltipHoverObject component = this.m_ownedToggle.GetComponent<UITooltipHoverObject>();
		component.Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
		{
			UISimpleTooltip uisimpleTooltip = (UISimpleTooltip)tooltip;
			uisimpleTooltip.Setup(StringUtil.TR("Owned", "Store"));
			return true;
		}, null);
		this.m_ownedToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnOwnedCheckedChanged));
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnAccountDataUpdated -= this.OnAccountDataUpdate;
		}
	}

	private void OnAccountDataUpdate(PersistedAccountData accountData)
	{
		this.UpdateVisibleItems(accountData.AccountComponent);
		int currentPage = this.m_currentPage;
		this.ShowList(this.m_currentList);
		this.m_currentPage = currentPage;
		this.ResetPage();
	}

	private List<T> SortList<T>(List<T> listToSort, List<int> sortOrders)
	{
		List<T> list = new List<T>();
		List<UIPlayerProgressBanners.SortedItem<T>> list2 = new List<UIPlayerProgressBanners.SortedItem<T>>();
		for (int i = 0; i < listToSort.Count; i++)
		{
			list2.Add(new UIPlayerProgressBanners.SortedItem<T>(listToSort[i], sortOrders[i] * listToSort.Count + i));
		}
		list2.Sort(delegate(UIPlayerProgressBanners.SortedItem<T> one, UIPlayerProgressBanners.SortedItem<T> two)
		{
			if (one == null && two == null)
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
					RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIPlayerProgressBanners.<SortList`1>m__1(UIPlayerProgressBanners.SortedItem<T>, UIPlayerProgressBanners.SortedItem<T>)).MethodHandle;
				}
				return 0;
			}
			if (one == null)
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
				return -1;
			}
			if (two == null)
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
				return 1;
			}
			return one.SortOrder.CompareTo(two.SortOrder);
		});
		for (int j = 0; j < list2.Count; j++)
		{
			list.Add(list2[j].Payload);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressBanners.SortList(List<T>, List<int>)).MethodHandle;
		}
		return list;
	}

	private void OnOwnedCheckedChanged(bool isChecked)
	{
		this.m_showLocked = !isChecked;
		this.ShowList(this.m_currentList);
		this.m_currentPage = 0;
		this.ResetPage();
	}

	private void UpdateVisibleItems(AccountComponent accountComponent)
	{
		bool flag;
		if (GameManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressBanners.UpdateVisibleItems(AccountComponent)).MethodHandle;
			}
			flag = GameManager.Get().GameplayOverrides.EnableHiddenCharacters;
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		if (flag2)
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
			this.m_visibleTitles = new List<GameBalanceVars.PlayerTitle>(GameBalanceVars.Get().PlayerTitles);
		}
		else
		{
			this.m_visibleTitles = new List<GameBalanceVars.PlayerTitle>();
			GameBalanceVars.PlayerTitle[] playerTitles = GameBalanceVars.Get().PlayerTitles;
			int i = 0;
			while (i < playerTitles.Length)
			{
				GameBalanceVars.PlayerTitle playerTitle = playerTitles[i];
				if (accountComponent.IsTitleUnlocked(playerTitle))
				{
					goto IL_E7;
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				CharacterType unlockCharacterType = playerTitle.GetUnlockCharacterType();
				if (!playerTitle.m_isHidden)
				{
					if (unlockCharacterType != CharacterType.None)
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
						if (GameWideData.Get().GetCharacterResourceLink(unlockCharacterType).m_isHidden)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								goto IL_D9;
							}
						}
					}
					if (GameBalanceVarsExtensions.MeetsVisibilityConditions(playerTitle))
					{
						goto IL_E7;
					}
				}
				IL_D9:
				IL_F3:
				i++;
				continue;
				IL_E7:
				this.m_visibleTitles.Add(playerTitle);
				goto IL_F3;
			}
		}
		this.m_visibleBackgroundBanners = new List<GameBalanceVars.PlayerBanner>();
		this.m_visibleForegroundBanners = new List<GameBalanceVars.PlayerBanner>();
		GameBalanceVars.PlayerBanner[] playerBanners = GameBalanceVars.Get().PlayerBanners;
		int j = 0;
		while (j < playerBanners.Length)
		{
			GameBalanceVars.PlayerBanner playerBanner = playerBanners[j];
			if (flag2 || accountComponent.IsBannerUnlocked(playerBanner))
			{
				goto IL_1B6;
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
			CharacterType unlockCharacterType2 = playerBanner.GetUnlockCharacterType();
			if (!playerBanner.m_isHidden)
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
				if (unlockCharacterType2 != CharacterType.None)
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
					if (GameWideData.Get().GetCharacterResourceLink(unlockCharacterType2).m_isHidden)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							goto IL_19D;
						}
					}
				}
				if (GameBalanceVarsExtensions.MeetsVisibilityConditions(playerBanner))
				{
					goto IL_1B6;
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			IL_19D:
			IL_1E6:
			j++;
			continue;
			IL_1B6:
			if (playerBanner.m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
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
				this.m_visibleBackgroundBanners.Add(playerBanner);
				goto IL_1E6;
			}
			this.m_visibleForegroundBanners.Add(playerBanner);
			goto IL_1E6;
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
		this.m_visibleRibbons = new List<GameBalanceVars.PlayerRibbon>();
		GameBalanceVars.PlayerRibbon[] playerRibbons = GameBalanceVars.Get().PlayerRibbons;
		int k = 0;
		while (k < playerRibbons.Length)
		{
			GameBalanceVars.PlayerRibbon playerRibbon = playerRibbons[k];
			if (accountComponent.IsRibbonUnlocked(playerRibbon))
			{
				goto IL_29A;
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
			if (flag2)
			{
				goto IL_29A;
			}
			CharacterType unlockCharacterType3 = playerRibbon.GetUnlockCharacterType();
			if (!playerRibbon.m_isHidden)
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
				if (unlockCharacterType3 != CharacterType.None && GameWideData.Get().GetCharacterResourceLink(unlockCharacterType3).m_isHidden)
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
				}
				else
				{
					if (GameBalanceVarsExtensions.MeetsVisibilityConditions(playerRibbon))
					{
						goto IL_29A;
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			IL_2CE:
			k++;
			continue;
			IL_29A:
			if (!FactionWideData.Get().IsRibbonInCompetition(playerRibbon.ID, ClientGameManager.Get().ActiveFactionCompetition))
			{
				goto IL_2CE;
			}
			this.m_visibleRibbons.Add(playerRibbon);
			goto IL_2CE;
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
		List<int> list = new List<int>();
		for (int l = 0; l < this.m_visibleTitles.Count; l++)
		{
			list.Add(this.m_visibleTitles[l].m_sortOrder);
		}
		this.m_visibleTitles = this.SortList<GameBalanceVars.PlayerTitle>(this.m_visibleTitles, list);
		list.Clear();
		for (int m = 0; m < this.m_visibleBackgroundBanners.Count; m++)
		{
			list.Add(this.m_visibleBackgroundBanners[m].m_sortOrder);
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
		this.m_visibleBackgroundBanners = this.SortList<GameBalanceVars.PlayerBanner>(this.m_visibleBackgroundBanners, list);
		list.Clear();
		for (int n = 0; n < this.m_visibleForegroundBanners.Count; n++)
		{
			list.Add(this.m_visibleForegroundBanners[n].m_sortOrder);
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
		this.m_visibleForegroundBanners = this.SortList<GameBalanceVars.PlayerBanner>(this.m_visibleForegroundBanners, list);
		list.Clear();
		for (int num = 0; num < this.m_visibleRibbons.Count; num++)
		{
			list.Add(this.m_visibleRibbons[num].m_sortOrder);
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
		this.m_visibleRibbons = this.SortList<GameBalanceVars.PlayerRibbon>(this.m_visibleRibbons, list);
		this.m_visibleTitles.Insert(0, null);
		this.m_visibleRibbons.Insert(0, null);
		this.m_unlockedTitles = new List<GameBalanceVars.PlayerTitle>();
		foreach (GameBalanceVars.PlayerTitle playerTitle2 in this.m_visibleTitles)
		{
			if (playerTitle2 != null)
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
				if (!ClientGameManager.Get().IsTitleUnlocked(playerTitle2))
				{
					continue;
				}
			}
			this.m_unlockedTitles.Add(playerTitle2);
		}
		this.m_unlockedBackgroundBanners = new List<GameBalanceVars.PlayerBanner>();
		foreach (GameBalanceVars.PlayerBanner playerBanner2 in this.m_visibleBackgroundBanners)
		{
			if (playerBanner2 != null)
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
				List<GameBalanceVars.UnlockConditionValue> list2;
				if (!ClientGameManager.Get().IsBannerUnlocked(playerBanner2, out list2))
				{
					continue;
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
			}
			this.m_unlockedBackgroundBanners.Add(playerBanner2);
		}
		this.m_unlockedForegroundBanners = new List<GameBalanceVars.PlayerBanner>();
		using (List<GameBalanceVars.PlayerBanner>.Enumerator enumerator3 = this.m_visibleForegroundBanners.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				GameBalanceVars.PlayerBanner playerBanner3 = enumerator3.Current;
				if (playerBanner3 != null)
				{
					List<GameBalanceVars.UnlockConditionValue> list2;
					if (!ClientGameManager.Get().IsBannerUnlocked(playerBanner3, out list2))
					{
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
				this.m_unlockedForegroundBanners.Add(playerBanner3);
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
		this.m_unlockedRibbons = new List<GameBalanceVars.PlayerRibbon>();
		using (List<GameBalanceVars.PlayerRibbon>.Enumerator enumerator4 = this.m_visibleRibbons.GetEnumerator())
		{
			while (enumerator4.MoveNext())
			{
				GameBalanceVars.PlayerRibbon playerRibbon2 = enumerator4.Current;
				if (playerRibbon2 != null)
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
					List<GameBalanceVars.UnlockConditionValue> list2;
					if (!ClientGameManager.Get().IsRibbonUnlocked(playerRibbon2, out list2))
					{
						continue;
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
				this.m_unlockedRibbons.Add(playerRibbon2);
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	private void Init()
	{
		if (!this.initialized)
		{
			this.m_pageIndicators = new List<UIPageIndicator>(this.m_pageGrid.GetComponentsInChildren<UIPageIndicator>(true));
			this.m_bannerButtons = new List<UIPlayerProgressBannersButton>(this.m_bannerGrid.GetComponentsInChildren<UIPlayerProgressBannersButton>(true));
			this.m_titleButtons = new List<UIPlayerProgressBannersButton>(this.m_titleGrid.GetComponentsInChildren<UIPlayerProgressBannersButton>(true));
			this.m_ribbonButtons = new List<UIPlayerProgressBannersButton>(this.m_ribbonGrid.GetComponentsInChildren<UIPlayerProgressBannersButton>(true));
			this.m_emblemButtons = new List<UIPlayerProgressBannersButton>(this.m_emblemGrid.GetComponentsInChildren<UIPlayerProgressBannersButton>(true));
			this.initialized = true;
			List<UIPlayerProgressBannersButton> list = this.m_bannerButtons.Concat(this.m_titleButtons).Concat(this.m_emblemButtons).Concat(this.m_ribbonButtons).ToList<UIPlayerProgressBannersButton>();
			for (int i = 0; i < list.Count; i++)
			{
				list[i].m_selectableButton.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
				UIManager.SetGameObjectActive(list[i], false, null);
				StaggerComponent.SetStaggerComponent(list[i].gameObject, true, true);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressBanners.Init()).MethodHandle;
			}
			this.UpdateVisibleItems(ClientGameManager.Get().GetPlayerAccountData().AccountComponent);
		}
	}

	private List<UIPlayerProgressBannersButton> GetCurrentButtonsList()
	{
		switch (this.m_currentList)
		{
		case UIPlayerProgressBanners.CurrentList.Foreground:
			return this.m_emblemButtons;
		case UIPlayerProgressBanners.CurrentList.Background:
			return this.m_bannerButtons;
		case UIPlayerProgressBanners.CurrentList.Title:
			return this.m_titleButtons;
		case UIPlayerProgressBanners.CurrentList.Ribbon:
			return this.m_ribbonButtons;
		default:
			return null;
		}
	}

	private void OnScroll(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		if (pointerEventData.scrollDelta.y > 0f)
		{
			this.ClickedOnPrevPage(data);
		}
		else if (pointerEventData.scrollDelta.y < 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressBanners.OnScroll(BaseEventData)).MethodHandle;
			}
			this.ClickedOnNextPage(data);
		}
	}

	public void BannerClicked(UIPlayerProgressBannersButton btnClicked)
	{
		if (btnClicked.m_unlocked)
		{
			List<UIPlayerProgressBannersButton> currentButtonsList = this.GetCurrentButtonsList();
			for (int i = 0; i < currentButtonsList.Count; i++)
			{
				if (btnClicked == currentButtonsList[i])
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressBanners.BannerClicked(UIPlayerProgressBannersButton)).MethodHandle;
					}
					currentButtonsList[i].SetSelected(true);
				}
				else
				{
					currentButtonsList[i].SetSelected(false);
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
	}

	public void ShowList(UIPlayerProgressBanners.CurrentList listType)
	{
		this.m_currentList = listType;
		this.m_numItems = this.GetNumberOfItems(listType);
		this.numBannersPerPage = this.GetCurrentButtonsList().Count;
		int num = Mathf.CeilToInt((float)this.m_numItems / (float)this.numBannersPerPage);
		while (this.m_pageIndicators.Count < num)
		{
			UIPageIndicator uipageIndicator = UnityEngine.Object.Instantiate<UIPageIndicator>(this.m_pageIndicatorPrefab);
			uipageIndicator.transform.SetParent(this.m_pageGrid.transform);
			uipageIndicator.transform.localEulerAngles = Vector3.zero;
			uipageIndicator.transform.localPosition = Vector3.zero;
			uipageIndicator.transform.localScale = Vector3.one;
			UIEventTriggerUtils.AddListener(uipageIndicator.m_hitbox.gameObject, EventTriggerType.Scroll, new UIEventTriggerUtils.EventDelegate(this.OnScroll));
			uipageIndicator.SetSelected(false);
			this.m_pageIndicators.Add(uipageIndicator);
		}
		while (this.m_pageIndicators.Count > num)
		{
			UnityEngine.Object.Destroy(this.m_pageIndicators[0].gameObject);
			this.m_pageIndicators.RemoveAt(0);
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressBanners.ShowList(UIPlayerProgressBanners.CurrentList)).MethodHandle;
		}
		if (this.m_pageIndicators.Count > 0)
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
			this.ClickedOnPageIndicator(this.m_pageIndicators[0]);
			for (int i = 0; i < this.m_pageIndicators.Count; i++)
			{
				this.m_pageIndicators[i].SetPageNumber(i + 1);
				this.m_pageIndicators[i].SetClickCallback(new Action<UIPageIndicator>(UIPlayerProgressPanel.Get().ClickedOnPage));
			}
			for (;;)
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
			this.ClickedOnPageIndicator(null);
		}
		bool doActive = false;
		bool doActive2 = false;
		bool doActive3 = false;
		bool doActive4 = false;
		this.m_nextPage.transform.parent.SetAsLastSibling();
		switch (this.m_currentList)
		{
		case UIPlayerProgressBanners.CurrentList.Foreground:
			this.m_foregroundBtn.selectableButton.SetSelected(true, false, string.Empty, string.Empty);
			this.m_backgroundBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
			this.m_titlesBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
			this.m_ribbonsBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
			doActive = true;
			break;
		case UIPlayerProgressBanners.CurrentList.Background:
			this.m_foregroundBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
			this.m_backgroundBtn.selectableButton.SetSelected(true, false, string.Empty, string.Empty);
			this.m_titlesBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
			this.m_ribbonsBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
			doActive2 = true;
			break;
		case UIPlayerProgressBanners.CurrentList.Title:
			this.m_foregroundBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
			this.m_backgroundBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
			this.m_titlesBtn.selectableButton.SetSelected(true, false, string.Empty, string.Empty);
			this.m_ribbonsBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
			doActive3 = true;
			break;
		case UIPlayerProgressBanners.CurrentList.Ribbon:
			this.m_foregroundBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
			this.m_backgroundBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
			this.m_titlesBtn.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
			this.m_ribbonsBtn.selectableButton.SetSelected(true, false, string.Empty, string.Empty);
			doActive4 = true;
			break;
		}
		UIManager.SetGameObjectActive(this.m_emblemGrid, doActive, null);
		UIManager.SetGameObjectActive(this.m_bannerGrid, doActive2, null);
		UIManager.SetGameObjectActive(this.m_titleGrid, doActive3, null);
		UIManager.SetGameObjectActive(this.m_ribbonGrid, doActive4, null);
	}

	public void ForegroundBtnClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
		this.ShowList(UIPlayerProgressBanners.CurrentList.Foreground);
	}

	public void BackgroundBtnClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
		this.ShowList(UIPlayerProgressBanners.CurrentList.Background);
	}

	public void TitlesBtnClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
		this.ShowList(UIPlayerProgressBanners.CurrentList.Title);
	}

	public void RibbonsBtnClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
		this.ShowList(UIPlayerProgressBanners.CurrentList.Ribbon);
	}

	public void ClickedOnPrevPage(BaseEventData data)
	{
		if (this.m_currentPage > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressBanners.ClickedOnPrevPage(BaseEventData)).MethodHandle;
			}
			this.m_currentPage--;
			UIPageIndicator pageIndicator;
			if (this.m_pageIndicators.Count > this.m_currentPage)
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
				pageIndicator = this.m_pageIndicators[this.m_currentPage];
			}
			else
			{
				pageIndicator = null;
			}
			this.ClickedOnPageIndicator(pageIndicator);
			return;
		}
	}

	public void ClickedOnNextPage(BaseEventData data)
	{
		if (this.m_currentPage + 1 < this.m_pageIndicators.Count)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressBanners.ClickedOnNextPage(BaseEventData)).MethodHandle;
			}
			this.m_currentPage++;
			UIPageIndicator pageIndicator;
			if (this.m_pageIndicators.Count > this.m_currentPage)
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
				pageIndicator = this.m_pageIndicators[this.m_currentPage];
			}
			else
			{
				pageIndicator = null;
			}
			this.ClickedOnPageIndicator(pageIndicator);
			return;
		}
	}

	public int GetNumberOfItems(UIPlayerProgressBanners.CurrentList listType)
	{
		if (listType == UIPlayerProgressBanners.CurrentList.Title)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressBanners.GetNumberOfItems(UIPlayerProgressBanners.CurrentList)).MethodHandle;
			}
			if (this.m_showLocked)
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
				return this.m_visibleTitles.Count;
			}
			return this.m_unlockedTitles.Count;
		}
		else if (listType == UIPlayerProgressBanners.CurrentList.Background)
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
			if (this.m_showLocked)
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
				return this.m_visibleBackgroundBanners.Count;
			}
			return this.m_unlockedBackgroundBanners.Count;
		}
		else if (listType == UIPlayerProgressBanners.CurrentList.Foreground)
		{
			if (this.m_showLocked)
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
				return this.m_visibleForegroundBanners.Count;
			}
			return this.m_unlockedForegroundBanners.Count;
		}
		else
		{
			if (listType != UIPlayerProgressBanners.CurrentList.Ribbon)
			{
				return 0;
			}
			if (this.m_showLocked)
			{
				return this.m_visibleRibbons.Count;
			}
			return this.m_unlockedRibbons.Count;
		}
	}

	private bool IsSelectedIndex(int i)
	{
		return i == 0;
	}

	public void ResetPage()
	{
		UIPageIndicator pageIndicator;
		if (this.m_pageIndicators.Count > this.m_currentPage)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressBanners.ResetPage()).MethodHandle;
			}
			pageIndicator = this.m_pageIndicators[this.m_currentPage];
		}
		else
		{
			pageIndicator = null;
		}
		this.ClickedOnPageIndicator(pageIndicator);
	}

	public override void ClickedOnPageIndicator(UIPageIndicator pageIndicator)
	{
		for (int i = 0; i < this.m_pageIndicators.Count; i++)
		{
			if (this.m_pageIndicators[i] == pageIndicator)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressBanners.ClickedOnPageIndicator(UIPageIndicator)).MethodHandle;
				}
				this.m_pageIndicators[i].SetSelected(true);
				this.m_currentPage = i;
			}
			else
			{
				this.m_pageIndicators[i].SetSelected(false);
			}
		}
		List<UIPlayerProgressBannersButton> currentButtonsList = this.GetCurrentButtonsList();
		for (int j = 0; j < currentButtonsList.Count; j++)
		{
			int num = this.m_currentPage * this.numBannersPerPage + j;
			if (num < this.m_numItems)
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
				if (this.m_currentList == UIPlayerProgressBanners.CurrentList.Title)
				{
					if (this.m_showLocked)
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
						currentButtonsList[j].SetupTitle(this.m_visibleTitles[num]);
					}
					else
					{
						currentButtonsList[j].SetupTitle(this.m_unlockedTitles[num]);
					}
				}
				else if (this.m_currentList == UIPlayerProgressBanners.CurrentList.Background)
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
					if (this.m_showLocked)
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
						currentButtonsList[j].SetupBanner(this.m_visibleBackgroundBanners[num]);
					}
					else
					{
						currentButtonsList[j].SetupBanner(this.m_unlockedBackgroundBanners[num]);
					}
				}
				else if (this.m_currentList == UIPlayerProgressBanners.CurrentList.Foreground)
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
					if (this.m_showLocked)
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
						currentButtonsList[j].SetupBanner(this.m_visibleForegroundBanners[num]);
					}
					else
					{
						currentButtonsList[j].SetupBanner(this.m_unlockedForegroundBanners[num]);
					}
				}
				else
				{
					if (this.m_currentList != UIPlayerProgressBanners.CurrentList.Ribbon)
					{
						throw new Exception(this.m_currentList + " not supported");
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_showLocked)
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
						currentButtonsList[j].SetupRibbon(this.m_visibleRibbons[num]);
					}
					else
					{
						currentButtonsList[j].SetupRibbon(this.m_unlockedRibbons[num]);
					}
				}
			}
			else
			{
				currentButtonsList[j].SetupBanner(null);
			}
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
	}

	public override void SetActive(bool visible)
	{
		base.SetActive(visible);
		this.Init();
		if (!visible)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressBanners.SetActive(bool)).MethodHandle;
			}
			return;
		}
		this.ShowList(UIPlayerProgressBanners.CurrentList.Foreground);
	}

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
			this.Payload = payload;
			this.SortOrder = sortOrder;
		}
	}
}
