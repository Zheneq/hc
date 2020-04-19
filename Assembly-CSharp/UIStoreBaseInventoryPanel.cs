using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UIStoreBaseInventoryPanel : UIStoreBasePanel
{
	[Header("Note: Banners are not freelancer items, even in freelancer page")]
	public bool m_isFreelancerItems;

	public GridLayoutGroup m_itemsGrid;

	public UIStorePageIndicator m_pageItemPrefab;

	public GridLayoutGroup m_pageListContainer;

	public _ButtonSwapSprite m_prevPage;

	public _ButtonSwapSprite m_nextPage;

	public RectTransform m_buyContainer;

	public TextMeshProUGUI m_buyLabel;

	public UIStoreBaseInventoryPanel.BuyButton[] m_buyButtons;

	private const float kUnlockLabelPadding = 25f;

	private GameBalanceVars.PlayerUnlockable[] m_rawItemsList;

	private List<GameBalanceVars.PlayerUnlockable> m_visibleItemsList;

	private UIStoreItemBtn[] m_itemBtns;

	private int m_numOwned;

	private int m_numTotal;

	private int m_pageNum;

	private int m_numberOfPages;

	private List<UIStorePageIndicator> m_pageMarkers;

	private bool isInitialized;

	private bool isUpdatePending;

	private GameBalanceVars.PlayerUnlockable m_currentSelectedItem;

	private UIStoreItemBtn m_currentSelectedButton;

	private GameObject m_parentContainer;

	protected CharacterType m_charType;

	protected UIStoreBaseInventoryPanel()
	{
		if (UIStoreBaseInventoryPanel.<>f__am$cache1 == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel..ctor()).MethodHandle;
			}
			UIStoreBaseInventoryPanel.<>f__am$cache1 = delegate(UIStoreBaseInventoryPanel A_0, int A_1, int A_2)
			{
			};
		}
		this.OnCountsRefreshed = UIStoreBaseInventoryPanel.<>f__am$cache1;
		base..ctor();
	}

	public event Action<UIStoreBaseInventoryPanel, int, int> OnCountsRefreshed
	{
		add
		{
			Action<UIStoreBaseInventoryPanel, int, int> action = this.OnCountsRefreshed;
			Action<UIStoreBaseInventoryPanel, int, int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<UIStoreBaseInventoryPanel, int, int>>(ref this.OnCountsRefreshed, (Action<UIStoreBaseInventoryPanel, int, int>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<UIStoreBaseInventoryPanel, int, int> action = this.OnCountsRefreshed;
			Action<UIStoreBaseInventoryPanel, int, int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<UIStoreBaseInventoryPanel, int, int>>(ref this.OnCountsRefreshed, (Action<UIStoreBaseInventoryPanel, int, int>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.remove_OnCountsRefreshed(Action<UIStoreBaseInventoryPanel, int, int>)).MethodHandle;
			}
		}
	}

	protected abstract GameBalanceVars.PlayerUnlockable[] GetRawItemsList();

	protected abstract void PurchaseItem(GameBalanceVars.PlayerUnlockable item, CurrencyType type);

	protected virtual bool ShouldCheckmark(GameBalanceVars.PlayerUnlockable item)
	{
		return false;
	}

	protected virtual bool ShouldFilter(GameBalanceVars.PlayerUnlockable item)
	{
		return false;
	}

	protected virtual Toggle[] GetFilters()
	{
		return new Toggle[0];
	}

	protected virtual void ItemSelected(GameBalanceVars.PlayerUnlockable item)
	{
	}

	protected virtual void ItemClicked(GameBalanceVars.PlayerUnlockable item)
	{
	}

	protected virtual CurrencyData GetItemCost(GameBalanceVars.PlayerUnlockable item, CurrencyType type)
	{
		CurrencyData currencyData = new CurrencyData();
		if (type == CurrencyType.ISO)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.GetItemCost(GameBalanceVars.PlayerUnlockable, CurrencyType)).MethodHandle;
			}
			currencyData.Amount = item.GetUnlockISOPrice();
		}
		else if (type == CurrencyType.FreelancerCurrency)
		{
			currencyData.Amount = item.GetUnlockFreelancerCurrencyPrice();
		}
		else if (type == CurrencyType.RankedCurrency)
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
			currencyData.Amount = item.GetUnlockRankedCurrencyPrice();
		}
		else
		{
			currencyData.Amount = 0;
		}
		currencyData.Type = type;
		return currencyData;
	}

	public virtual TooltipType? GetItemTooltipType()
	{
		return null;
	}

	public virtual bool ItemTooltipPopulate(UITooltipBase tooltip, UIStoreItemBtn slot, GameBalanceVars.PlayerUnlockable item)
	{
		return false;
	}

	public void Initialize()
	{
		if (this.isInitialized)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.Initialize()).MethodHandle;
			}
			return;
		}
		this.isInitialized = true;
		this.InitRawItemsList();
		if (HitchDetector.Get() != null)
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
			HitchDetector.Get().AddNewLayoutGroup(this.m_itemsGrid);
		}
		this.m_pageNum = 0;
		this.m_itemBtns = this.m_itemsGrid.GetComponentsInChildren<UIStoreItemBtn>();
		this.m_visibleItemsList = new List<GameBalanceVars.PlayerUnlockable>();
		this.m_pageMarkers = new List<UIStorePageIndicator>();
		foreach (UIStoreItemBtn uistoreItemBtn in this.m_itemBtns)
		{
			uistoreItemBtn.m_selectableBtn.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
			uistoreItemBtn.SetParent(this);
			UIManager.SetGameObjectActive(uistoreItemBtn, false, null);
			StaggerComponent.SetStaggerComponent(uistoreItemBtn.gameObject, true, true);
		}
		Image component = base.GetComponent<Image>();
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
			UIEventTriggerUtils.AddListener(component.gameObject, EventTriggerType.Scroll, new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		foreach (Toggle toggle in this.GetFilters())
		{
			toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnFilterChange));
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
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			this.RefreshVisibleItemsList();
			this.ShowPage(0);
		}
		for (int k = 0; k < this.m_buyButtons.Length; k++)
		{
			UIStoreBaseInventoryPanel.BuyButton buyButton = this.m_buyButtons[k];
			buyButton.m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BuyButtonClicked);
			buyButton.m_hitbox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, (UITooltipBase tooltip) => this.BuyButtonTooltipSetup(tooltip, buyButton), null);
			buyButton.m_hitbox.SetForceHovercallback(true);
			buyButton.m_hitbox.SetForceExitCallback(true);
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

	protected void Start()
	{
		this.Initialize();
		if (this.m_isFreelancerItems)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.Start()).MethodHandle;
			}
			ClientGameManager.Get().OnCharacterDataUpdated += this.OnCharacterDataUpdated;
		}
		else
		{
			ClientGameManager.Get().OnAccountDataUpdated += this.OnAccountDataUpdated;
		}
		ClientGameManager.Get().OnLobbyGameplayOverridesChange += this.OnLobbyGameplayOverridesChange;
	}

	protected void OnDestroy()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.OnDestroy()).MethodHandle;
			}
			if (this.m_isFreelancerItems)
			{
				ClientGameManager.Get().OnCharacterDataUpdated -= this.OnCharacterDataUpdated;
			}
			else
			{
				ClientGameManager.Get().OnAccountDataUpdated -= this.OnAccountDataUpdated;
			}
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= this.OnLobbyGameplayOverridesChange;
		}
	}

	protected void UpdatePanel()
	{
		if (this.m_parentContainer != null && this.m_parentContainer.gameObject.activeInHierarchy)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.UpdatePanel()).MethodHandle;
			}
			this.RefreshVisibleItemsList();
			this.ShowPage(this.m_pageNum);
		}
		else
		{
			this.isUpdatePending = true;
		}
	}

	private void OnAccountDataUpdated(PersistedAccountData data)
	{
		this.UpdatePanel();
	}

	public void OnCharacterDataUpdated(PersistedCharacterData newData)
	{
		if (this.m_charType == newData.CharacterType)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.OnCharacterDataUpdated(PersistedCharacterData)).MethodHandle;
			}
			this.UpdatePanel();
		}
	}

	private void OnLobbyGameplayOverridesChange(LobbyGameplayOverrides gameplayOverrides)
	{
		this.UpdatePanel();
	}

	private void OnFilterChange(bool newValue)
	{
		this.RefreshVisibleItemsList();
		this.ShowPage(0);
	}

	public void HandlePendingUpdates()
	{
		if (this.isInitialized)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.HandlePendingUpdates()).MethodHandle;
			}
			if (this.isUpdatePending)
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
				this.RefreshVisibleItemsList();
				this.ShowPage(this.m_pageNum);
				this.isUpdatePending = false;
			}
		}
	}

	private void InitRawItemsList()
	{
		this.m_rawItemsList = this.GetRawItemsList();
	}

	public void SetParentContainer(GameObject parentContainer)
	{
		this.m_parentContainer = parentContainer;
	}

	public void SetCharacter(CharacterType type)
	{
		this.m_charType = type;
		this.InitRawItemsList();
		this.RefreshVisibleItemsList();
		this.ShowPage(0);
		this.isUpdatePending = false;
	}

	protected List<GameBalanceVars.PlayerUnlockable> SortItems(List<GameBalanceVars.PlayerUnlockable> input)
	{
		List<UIStoreBaseInventoryPanel.AdjustedPlayerUnlockable> list = new List<UIStoreBaseInventoryPanel.AdjustedPlayerUnlockable>();
		for (int i = 0; i < input.Count; i++)
		{
			list.Add(new UIStoreBaseInventoryPanel.AdjustedPlayerUnlockable
			{
				Unlockable = input[i],
				AdjustedSortOrder = input[i].m_sortOrder * input.Count + i
			});
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.SortItems(List<GameBalanceVars.PlayerUnlockable>)).MethodHandle;
		}
		List<UIStoreBaseInventoryPanel.AdjustedPlayerUnlockable> list2 = list;
		if (UIStoreBaseInventoryPanel.<>f__am$cache0 == null)
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
			UIStoreBaseInventoryPanel.<>f__am$cache0 = ((UIStoreBaseInventoryPanel.AdjustedPlayerUnlockable first, UIStoreBaseInventoryPanel.AdjustedPlayerUnlockable second) => first.AdjustedSortOrder.CompareTo(second.AdjustedSortOrder));
		}
		list2.Sort(UIStoreBaseInventoryPanel.<>f__am$cache0);
		List<GameBalanceVars.PlayerUnlockable> list3 = new List<GameBalanceVars.PlayerUnlockable>();
		for (int j = 0; j < list.Count; j++)
		{
			list3.Add(list[j].Unlockable);
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
		if (list3.Count > 0)
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
			if (list3[list3.Count - 1].m_sortOrder != 0)
			{
				while (list3[0].m_sortOrder == 0)
				{
					GameBalanceVars.PlayerUnlockable item = list3[0];
					list3.RemoveAt(0);
					list3.Add(item);
				}
			}
		}
		return list3;
	}

	protected void RefreshVisibleItemsList()
	{
		LobbyGameplayOverrides gameplayOverrides = GameManager.Get().GameplayOverrides;
		bool enableHiddenCharacters = gameplayOverrides.EnableHiddenCharacters;
		this.m_numOwned = (this.m_numTotal = 0);
		this.m_visibleItemsList.Clear();
		int i = 0;
		while (i < this.m_rawItemsList.Length)
		{
			if (this.m_rawItemsList[i] is GameBalanceVars.ColorUnlockData)
			{
				bool flag = !gameplayOverrides.IsColorAllowed((CharacterType)this.m_rawItemsList[i].Index1, this.m_rawItemsList[i].Index2, this.m_rawItemsList[i].Index3, this.m_rawItemsList[i].ID);
				if (!flag)
				{
					goto IL_ED;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.RefreshVisibleItemsList()).MethodHandle;
				}
			}
			else
			{
				if (!(this.m_rawItemsList[i] is GameBalanceVars.PlayerRibbon))
				{
					goto IL_ED;
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
				if (FactionWideData.Get().IsRibbonInCompetition(this.m_rawItemsList[i].ID, ClientGameManager.Get().ActiveFactionCompetition))
				{
					goto IL_ED;
				}
			}
			IL_247:
			i++;
			continue;
			IL_ED:
			if (!this.m_rawItemsList[i].IsOwned())
			{
				bool flag2;
				if (!this.m_rawItemsList[i].m_isHidden)
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
					flag2 = !GameBalanceVarsExtensions.MeetsVisibilityConditions(this.m_rawItemsList[i]);
				}
				else
				{
					flag2 = true;
				}
				bool flag3 = flag2;
				if (!flag3)
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
					if (this.m_rawItemsList[i] is GameBalanceVars.ColorUnlockData)
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
						flag3 = GameBalanceVars.Get().GetCharacterUnlockData((CharacterType)this.m_rawItemsList[i].Index1).skinUnlockData[this.m_rawItemsList[i].Index2].m_isHidden;
						if (!flag3)
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
							flag3 = GameBalanceVars.Get().GetCharacterUnlockData((CharacterType)this.m_rawItemsList[i].Index1).skinUnlockData[this.m_rawItemsList[i].Index2].patternUnlockData[this.m_rawItemsList[i].Index3].m_isHidden;
						}
					}
				}
				if (!enableHiddenCharacters && flag3)
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
					goto IL_247;
				}
			}
			else
			{
				this.m_numOwned++;
			}
			this.m_numTotal++;
			if (this.ShouldFilter(this.m_rawItemsList[i]))
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
				goto IL_247;
			}
			this.m_visibleItemsList.Add(this.m_rawItemsList[i]);
			goto IL_247;
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
		this.OnCountsRefreshed(this, this.m_numOwned, this.m_numTotal);
		this.m_numberOfPages = this.m_visibleItemsList.Count / this.m_itemBtns.Length;
		if (this.m_visibleItemsList.Count % this.m_itemBtns.Length > 0)
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
			this.m_numberOfPages++;
		}
		for (int j = this.m_pageMarkers.Count; j < this.m_numberOfPages; j++)
		{
			UIStorePageIndicator uistorePageIndicator = UnityEngine.Object.Instantiate<UIStorePageIndicator>(this.m_pageItemPrefab);
			uistorePageIndicator.transform.SetParent(this.m_pageListContainer.transform);
			uistorePageIndicator.transform.localScale = Vector3.one;
			uistorePageIndicator.transform.localPosition = Vector3.zero;
			uistorePageIndicator.SetSelected(j == 0);
			uistorePageIndicator.m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PageClicked);
			uistorePageIndicator.m_hitbox.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
			uistorePageIndicator.SetPageNumber(j + 1);
			this.m_pageMarkers.Add(uistorePageIndicator);
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
		for (int k = this.m_numberOfPages; k < this.m_pageMarkers.Count; k++)
		{
			UnityEngine.Object.Destroy(this.m_pageMarkers[k].gameObject);
			this.m_pageMarkers.RemoveAt(k);
			k--;
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
		this.m_nextPage.transform.parent.SetAsLastSibling();
		this.m_prevPage.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ClickedOnPrevPage);
		this.m_nextPage.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ClickedOnNextPage);
		UIManager.SetGameObjectActive(this.m_pageListContainer, this.m_numberOfPages > 1, null);
	}

	public void PageClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
		for (int i = 0; i < this.m_pageMarkers.Count; i++)
		{
			if (this.m_pageMarkers[i].m_hitbox.gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.PageClicked(BaseEventData)).MethodHandle;
				}
				this.ShowPage(i);
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

	private void OnScroll(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		if (pointerEventData.scrollDelta.y > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.OnScroll(BaseEventData)).MethodHandle;
			}
			this.ClickedOnPrevPage(null);
		}
		else if (pointerEventData.scrollDelta.y < 0f)
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
			this.ClickedOnNextPage(null);
		}
	}

	public void ClickedOnPrevPage(BaseEventData data)
	{
		if (this.m_pageNum - 1 < 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.ClickedOnPrevPage(BaseEventData)).MethodHandle;
			}
			return;
		}
		UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
		this.ShowPage(this.m_pageNum - 1);
	}

	public void ClickedOnNextPage(BaseEventData data)
	{
		if (this.m_pageNum + 1 >= this.m_numberOfPages)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.ClickedOnNextPage(BaseEventData)).MethodHandle;
			}
			return;
		}
		UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
		this.ShowPage(this.m_pageNum + 1);
	}

	private void ShowPage(int pageNum)
	{
		this.Initialize();
		for (int i = 0; i < this.m_pageMarkers.Count; i++)
		{
			this.m_pageMarkers[i].SetSelected(i == pageNum);
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.ShowPage(int)).MethodHandle;
		}
		if (this.m_pageNum != pageNum)
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
			this.m_currentSelectedButton = null;
		}
		this.m_pageNum = pageNum;
		int num = this.m_itemBtns.Length * pageNum;
		for (int j = 0; j < this.m_itemBtns.Length; j++)
		{
			GameBalanceVars.PlayerUnlockable playerUnlockable = null;
			int num2 = j + num;
			if (num2 < this.m_visibleItemsList.Count)
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
				playerUnlockable = this.m_visibleItemsList[num2];
			}
			this.m_itemBtns[j].Setup(playerUnlockable);
			if (playerUnlockable != null)
			{
				this.m_itemBtns[j].DisplayCheckMark(this.ShouldCheckmark(playerUnlockable));
			}
			this.m_itemBtns[j].m_selectableBtn.SetSelected(false, false, string.Empty, string.Empty);
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
		if (this.m_currentSelectedButton != null)
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
			this.DoItemSelect(this.m_currentSelectedButton, this.m_currentSelectedButton.GetItem());
		}
		else
		{
			this.m_currentSelectedButton = null;
			this.m_currentSelectedItem = null;
			UIManager.SetGameObjectActive(this.m_buyContainer, false, null);
		}
	}

	public void RefreshPage()
	{
		this.ShowPage(this.m_pageNum);
	}

	public int GetNumOwned()
	{
		return this.m_numOwned;
	}

	public int GetNumTotal()
	{
		return this.m_numTotal;
	}

	public void DoItemClick(UIStoreItemBtn btn, GameBalanceVars.PlayerUnlockable item)
	{
		btn.m_selectableBtn.spriteController.ResetMouseState();
		this.DoItemSelect(btn, item);
		this.ItemClicked(item);
	}

	private void DoItemSelect(UIStoreItemBtn btn, GameBalanceVars.PlayerUnlockable item)
	{
		if (this.m_currentSelectedButton != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.DoItemSelect(UIStoreItemBtn, GameBalanceVars.PlayerUnlockable)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_currentSelectedButton.m_selectableBtn.m_selectedContainer, false, null);
		}
		this.m_currentSelectedItem = item;
		this.m_currentSelectedButton = btn;
		if (item == null)
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
			return;
		}
		UIManager.SetGameObjectActive(btn.m_selectableBtn.m_selectedContainer, true, null);
		btn.transform.SetAsLastSibling();
		bool doActive = false;
		for (int i = 0; i < this.m_buyButtons.Length; i++)
		{
			if (this.m_buyButtons[i].Setup(item, this))
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
				doActive = true;
			}
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
		if (item is GameBalanceVars.StoreItemForPurchase)
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
			this.m_buyLabel.text = StringUtil.TR("Purchase", "Global");
		}
		else
		{
			this.m_buyLabel.text = StringUtil.TR("Unlock", "OverlayScreensScene");
		}
		UIManager.SetGameObjectActive(this.m_buyContainer, doActive, null);
		this.ItemSelected(item);
	}

	public bool SelectItem(InventoryItemTemplate template)
	{
		return this.SelectItem(delegate(GameBalanceVars.PlayerUnlockable unlockable)
		{
			if (unlockable is GameBalanceVars.PlayerTitle)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.<SelectItem>c__AnonStorey1.<>m__0(GameBalanceVars.PlayerUnlockable)).MethodHandle;
				}
				if (template.Type == InventoryItemType.TitleID)
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
					if (unlockable.ID == template.TypeSpecificData[0])
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
						return true;
					}
				}
			}
			if (unlockable is GameBalanceVars.PlayerBanner)
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
				if (template.Type == InventoryItemType.BannerID)
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
					if (unlockable.ID == template.TypeSpecificData[0])
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
						return true;
					}
				}
			}
			if (unlockable is GameBalanceVars.SkinUnlockData)
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
				if (template.Type == InventoryItemType.Skin)
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
					if (unlockable.Index1 == template.TypeSpecificData[0])
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
						if (unlockable.ID == template.TypeSpecificData[1])
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
							return true;
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.PatternUnlockData && template.Type == InventoryItemType.Texture)
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
				if (unlockable.Index1 == template.TypeSpecificData[0])
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
					if (unlockable.Index2 == template.TypeSpecificData[1])
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
						if (unlockable.ID == template.TypeSpecificData[2])
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
				}
			}
			if (unlockable is GameBalanceVars.ColorUnlockData)
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
				if (template.Type == InventoryItemType.Style && unlockable.Index1 == template.TypeSpecificData[0])
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
					if (unlockable.Index2 == template.TypeSpecificData[1] && unlockable.Index3 == template.TypeSpecificData[2])
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
						if (unlockable.ID == template.TypeSpecificData[3])
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
							return true;
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.TauntUnlockData)
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
				if (template.Type == InventoryItemType.Taunt)
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
					if (unlockable.Index1 == template.TypeSpecificData[0])
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
						if (unlockable.ID == template.TypeSpecificData[1])
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
							return true;
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.AbilityModUnlockData)
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
				if (template.Type == InventoryItemType.Mod && unlockable.Index1 == template.TypeSpecificData[0])
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
					if (unlockable.ID == template.TypeSpecificData[1])
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
						return true;
					}
				}
			}
			if (unlockable is GameBalanceVars.AbilityVfxUnlockData)
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
				if (template.Type == InventoryItemType.AbilityVfxSwap && unlockable.Index1 == template.TypeSpecificData[0])
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
					if (unlockable.ID == template.TypeSpecificData[1])
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
			}
			if (unlockable is GameBalanceVars.ChatEmoticon)
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
				if (template.Type == InventoryItemType.ChatEmoji)
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
					if (unlockable.ID == template.TypeSpecificData[0])
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
						return true;
					}
				}
			}
			if (unlockable is GameBalanceVars.OverconUnlockData && template.Type == InventoryItemType.Overcon)
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
				if (unlockable.ID == template.TypeSpecificData[0])
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
					return true;
				}
			}
			if (unlockable is GameBalanceVars.StoreItemForPurchase)
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
				if (template.Type == InventoryItemType.FreelancerExpBonus)
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
					if ((unlockable as GameBalanceVars.StoreItemForPurchase).m_itemTemplateId == template.Index)
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
						return true;
					}
				}
			}
			if (unlockable is GameBalanceVars.LoadingScreenBackground && template.Type == InventoryItemType.LoadingScreenBackground)
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
				if (unlockable.ID == template.TypeSpecificData[0])
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
					return true;
				}
			}
			return false;
		});
	}

	public bool SelectItem(UIPurchaseableItem item)
	{
		if (item.m_itemType == PurchaseItemType.InventoryItem)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.SelectItem(UIPurchaseableItem)).MethodHandle;
			}
			return this.SelectItem(InventoryWideData.Get().GetItemTemplate(item.m_inventoryTemplateId));
		}
		return this.SelectItem(delegate(GameBalanceVars.PlayerUnlockable unlockable)
		{
			if (unlockable is GameBalanceVars.PlayerTitle && item.m_itemType == PurchaseItemType.Title)
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
					RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIStoreBaseInventoryPanel.<SelectItem>c__AnonStorey2.<>m__0(GameBalanceVars.PlayerUnlockable)).MethodHandle;
				}
				if (unlockable.ID == item.m_titleID)
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
					return true;
				}
			}
			if (unlockable is GameBalanceVars.PlayerBanner)
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
				if (item.m_itemType == PurchaseItemType.Banner)
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
					if (unlockable.ID == item.m_bannerID)
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
						return true;
					}
				}
			}
			if (unlockable is GameBalanceVars.SkinUnlockData)
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
				if (item.m_itemType == PurchaseItemType.Skin)
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
					if (unlockable.Index1 == (int)item.m_charLink.m_characterType)
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
						if (unlockable.ID == item.m_skinIndex)
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
							return true;
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.PatternUnlockData)
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
				if (item.m_itemType == PurchaseItemType.Texture)
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
					if (unlockable.Index1 == (int)item.m_charLink.m_characterType)
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
						if (unlockable.Index2 == item.m_skinIndex && unlockable.ID == item.m_textureIndex)
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
							return true;
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.ColorUnlockData)
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
				if (item.m_itemType == PurchaseItemType.Tint && unlockable.Index1 == (int)item.m_charLink.m_characterType)
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
					if (unlockable.Index2 == item.m_skinIndex)
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
						if (unlockable.Index3 == item.m_textureIndex)
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
							if (unlockable.ID == item.m_tintIndex)
							{
								return true;
							}
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.TauntUnlockData)
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
				if (item.m_itemType == PurchaseItemType.Taunt)
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
					if (unlockable.Index1 == (int)item.m_charLink.m_characterType)
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
						if (unlockable.ID == item.m_tauntIndex)
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
							return true;
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.AbilityVfxUnlockData && item.m_itemType == PurchaseItemType.AbilityVfx)
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
				if (unlockable.Index1 == (int)item.m_charLink.m_characterType)
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
					if (unlockable.ID == item.m_abilityVfxID)
					{
						return true;
					}
				}
			}
			if (unlockable is GameBalanceVars.ChatEmoticon)
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
				if (item.m_itemType == PurchaseItemType.Emoticon && unlockable.ID == item.m_emoticonID)
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
					return true;
				}
			}
			if (unlockable is GameBalanceVars.OverconUnlockData)
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
				if (item.m_itemType == PurchaseItemType.Overcon && unlockable.ID == item.m_overconID)
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
					return true;
				}
			}
			if (unlockable is GameBalanceVars.LoadingScreenBackground)
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
				if (item.m_itemType == PurchaseItemType.LoadingScreenBackground && unlockable.ID == item.m_loadingScreenBackgroundId)
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
			if (unlockable is GameBalanceVars.CharacterUnlockData)
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
				if (item.m_itemType == PurchaseItemType.Character)
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
					if (unlockable.ID == (int)item.m_charLink.m_characterType)
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
						return true;
					}
				}
			}
			return false;
		});
	}

	private bool SelectItem(Func<GameBalanceVars.PlayerUnlockable, bool> foundCheck)
	{
		this.Initialize();
		this.HandlePendingUpdates();
		for (int i = 0; i < this.m_visibleItemsList.Count; i++)
		{
			if (foundCheck(this.m_visibleItemsList[i]))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.SelectItem(Func<GameBalanceVars.PlayerUnlockable, bool>)).MethodHandle;
				}
				int num = i % this.m_itemBtns.Length;
				int pageNum;
				if (i > 0)
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
					pageNum = i / this.m_itemBtns.Length;
				}
				else
				{
					pageNum = 0;
				}
				this.ShowPage(pageNum);
				this.DoItemSelect(this.m_itemBtns[num], this.m_itemBtns[num].GetItem());
				return true;
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
		return false;
	}

	private UIStoreBaseInventoryPanel.BuyButton FindBuyButtonFromEventData(BaseEventData data, bool isHover)
	{
		PointerEventData pointerEventData = (PointerEventData)data;
		if (pointerEventData.selectedObject == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.FindBuyButtonFromEventData(BaseEventData, bool)).MethodHandle;
			}
			return null;
		}
		UIStoreBaseInventoryPanel.BuyButton result = null;
		for (int i = 0; i < this.m_buyButtons.Length; i++)
		{
			UnityEngine.Object gameObject = this.m_buyButtons[i].m_hitbox.gameObject;
			UnityEngine.Object y;
			if (isHover)
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
				y = pointerEventData.pointerEnter;
			}
			else
			{
				y = pointerEventData.selectedObject;
			}
			if (gameObject == y)
			{
				result = this.m_buyButtons[i];
				break;
			}
		}
		return result;
	}

	private void BuyButtonClicked(BaseEventData data)
	{
		if (this.m_currentSelectedItem == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.BuyButtonClicked(BaseEventData)).MethodHandle;
			}
			return;
		}
		UIStoreBaseInventoryPanel.BuyButton buyButton = this.FindBuyButtonFromEventData(data, false);
		if (buyButton != null)
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
			if (buyButton.CanAfford())
			{
				UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
				this.PurchaseItem(this.m_currentSelectedItem, buyButton.m_currencyType);
				buyButton.m_hitbox.ResetMouseState();
				return;
			}
		}
	}

	private bool BuyButtonTooltipSetup(UITooltipBase tooltip, UIStoreBaseInventoryPanel.BuyButton buyButton)
	{
		string tooltipMessage = buyButton.GetTooltipMessage();
		if (!tooltipMessage.IsNullOrEmpty())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.BuyButtonTooltipSetup(UITooltipBase, UIStoreBaseInventoryPanel.BuyButton)).MethodHandle;
			}
			(tooltip as UITitledTooltip).Setup(StringUtil.TR("Purchase", "Global"), tooltipMessage, string.Empty);
			return true;
		}
		if (!buyButton.CanAfford())
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
			CurrencyType currencyType = buyButton.m_currencyType;
			string term;
			if (currencyType != CurrencyType.ISO)
			{
				if (currencyType != CurrencyType.ModToken)
				{
					if (currencyType != CurrencyType.RankedCurrency)
					{
						if (currencyType != CurrencyType.FreelancerCurrency)
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
							throw new NotImplementedException(buyButton.m_currencyType + " does not have can't buy message");
						}
						term = "InsufficientFreelancerCurrency";
					}
					else
					{
						term = "InsufficientRankedCurrency";
					}
				}
				else
				{
					term = "InsufficientFunds";
				}
			}
			else
			{
				term = "InsufficientISO";
			}
			(tooltip as UITitledTooltip).Setup(StringUtil.TR(term, "Global"), StringUtil.TR("NotEnoughToUnlock", "Global"), string.Empty);
			return true;
		}
		return false;
	}

	[Serializable]
	public class BuyButton
	{
		public CurrencyType m_currencyType;

		public RectTransform m_container;

		public TextMeshProUGUI[] m_labels;

		public _ButtonSwapSprite m_hitbox;

		private bool m_canAfford;

		private string m_tooltipMessage;

		private CurrencyData m_currencyData;

		private CountryPrice m_realCurrencyData;

		public bool Setup(GameBalanceVars.PlayerUnlockable item, UIStoreBaseInventoryPanel parent)
		{
			this.m_canAfford = false;
			if (item.IsOwned())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBaseInventoryPanel.BuyButton.Setup(GameBalanceVars.PlayerUnlockable, UIStoreBaseInventoryPanel)).MethodHandle;
				}
				if (!item.CanStillPurchaseIfOwned())
				{
					goto IL_6E;
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
			if (!item.m_unlockData.UnlockConditions.IsNullOrEmpty<GameBalanceVars.UnlockCondition>())
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
				if (!ClientGameManager.Get().AreUnlockConditionsMet(item, true))
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						goto IL_6E;
					}
				}
			}
			if (this.m_currencyType == CurrencyType.NONE)
			{
				this.m_currencyData = null;
				this.m_realCurrencyData = null;
				if (item.Prices != null && item.Prices.Prices != null)
				{
					string accountCurrency = HydrogenConfig.Get().Ticket.AccountCurrency;
					for (int i = 0; i < item.Prices.Prices.Length; i++)
					{
						if (item.Prices.Prices[i].Currency.ToString() == accountCurrency)
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
							this.m_realCurrencyData = new CountryPrice();
							this.m_realCurrencyData.Currency = item.Prices.Prices[i].Currency;
							this.m_realCurrencyData.Price = item.Prices.Prices[i].Price;
							if (item is GameBalanceVars.StoreItemForPurchase)
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
								int itemTemplateId = ((GameBalanceVars.StoreItemForPurchase)item).m_itemTemplateId;
								float num;
								this.m_realCurrencyData.Price = CommerceClient.Get().GetStoreItemPrice(itemTemplateId, accountCurrency, out num);
							}
							else if (item is GameBalanceVars.ColorUnlockData)
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
								this.m_realCurrencyData.Price = CommerceClient.Get().GetStylePrice((CharacterType)item.Index1, item.Index2, item.Index3, item.ID, accountCurrency);
							}
							goto IL_1F0;
						}
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
				IL_1F0:
				if (this.m_realCurrencyData != null)
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
					if (this.m_realCurrencyData.Price > 0f)
					{
						UIManager.SetGameObjectActive(this.m_container, true, null);
						this.m_canAfford = true;
						goto IL_2AA;
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
				UIManager.SetGameObjectActive(this.m_container, false, null);
				return false;
			}
			this.m_currencyData = parent.GetItemCost(item, this.m_currencyType);
			this.m_realCurrencyData = null;
			UIManager.SetGameObjectActive(this.m_container, this.m_currencyData.Amount > 0, null);
			if (this.m_currencyData.Amount <= 0)
			{
				return false;
			}
			this.m_canAfford = ClientGameManager.Get().PlayerWallet.CanAfford(this.m_currencyData);
			IL_2AA:
			bool flag = GameBalanceVarsExtensions.MeetsPurchaseabilityConditions(item);
			bool flag2 = false;
			if (flag && item is GameBalanceVars.ColorUnlockData)
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
				CharacterType index = (CharacterType)item.Index1;
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(index);
				CharacterColor characterColor = characterResourceLink.m_skins[item.Index2].m_patterns[item.Index3].m_colors[item.ID];
				flag2 = (characterColor.m_requiredLevelForEquip > ClientGameManager.Get().GetPlayerCharacterLevel(index));
				if (flag2)
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
					flag = false;
				}
			}
			_SelectableBtn selectableButton = this.m_hitbox.selectableButton;
			bool disabled;
			if (this.m_canAfford)
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
				disabled = !flag;
			}
			else
			{
				disabled = true;
			}
			selectableButton.SetDisabled(disabled);
			this.m_tooltipMessage = null;
			if (!flag)
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
				if (!flag2)
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
					if (!item.PurchaseDescription.IsNullOrEmpty())
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
						this.m_tooltipMessage = item.GetPurchaseDescription();
					}
				}
			}
			CurrencyType currencyType = this.m_currencyType;
			string text;
			switch (currencyType)
			{
			case CurrencyType.RAFPoints:
				text = "referToken";
				break;
			case CurrencyType.RankedCurrency:
				text = "rankedCurrency";
				break;
			case CurrencyType.FreelancerCurrency:
				text = "credit";
				break;
			default:
				if (currencyType != CurrencyType.ISO)
				{
					if (currencyType != CurrencyType.ModToken)
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
						if (currencyType != CurrencyType.NONE)
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
							throw new NotImplementedException(this.m_currencyType + " doesn't have a sprite");
						}
						text = null;
					}
					else
					{
						text = "modToken";
					}
				}
				else
				{
					text = "iso";
				}
				break;
			}
			for (int j = 0; j < this.m_labels.Length; j++)
			{
				if (text == null)
				{
					this.m_labels[j].text = UIStorePanel.GetLocalizedPriceString(this.m_realCurrencyData.Price, this.m_realCurrencyData.Currency.ToString());
				}
				else
				{
					this.m_labels[j].text = string.Format("<sprite name={0}>{1}", text, UIStorePanel.FormatIntToString(this.m_currencyData.Amount, true));
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
			return true;
			IL_6E:
			UIManager.SetGameObjectActive(this.m_container, false, null);
			return false;
		}

		public bool CanAfford()
		{
			return this.m_canAfford;
		}

		public string GetTooltipMessage()
		{
			return this.m_tooltipMessage;
		}
	}

	private struct AdjustedPlayerUnlockable
	{
		public GameBalanceVars.PlayerUnlockable Unlockable;

		public int AdjustedSortOrder;
	}
}
