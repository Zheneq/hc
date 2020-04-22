using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UIStoreBaseInventoryPanel : UIStoreBasePanel
{
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
			m_canAfford = false;
			if (item.IsOwned())
			{
				if (!item.CanStillPurchaseIfOwned())
				{
					goto IL_006e;
				}
			}
			if (!item.m_unlockData.UnlockConditions.IsNullOrEmpty())
			{
				if (!ClientGameManager.Get().AreUnlockConditionsMet(item, true))
				{
					goto IL_006e;
				}
			}
			if (m_currencyType == CurrencyType.NONE)
			{
				m_currencyData = null;
				m_realCurrencyData = null;
				if (item.Prices != null && item.Prices.Prices != null)
				{
					string accountCurrency = HydrogenConfig.Get().Ticket.AccountCurrency;
					int num = 0;
					while (true)
					{
						if (num < item.Prices.Prices.Length)
						{
							if (item.Prices.Prices[num].Currency.ToString() == accountCurrency)
							{
								m_realCurrencyData = new CountryPrice();
								m_realCurrencyData.Currency = item.Prices.Prices[num].Currency;
								m_realCurrencyData.Price = item.Prices.Prices[num].Price;
								if (item is GameBalanceVars.StoreItemForPurchase)
								{
									int itemTemplateId = ((GameBalanceVars.StoreItemForPurchase)item).m_itemTemplateId;
									m_realCurrencyData.Price = CommerceClient.Get().GetStoreItemPrice(itemTemplateId, accountCurrency, out float _);
								}
								else if (item is GameBalanceVars.ColorUnlockData)
								{
									m_realCurrencyData.Price = CommerceClient.Get().GetStylePrice((CharacterType)item.Index1, item.Index2, item.Index3, item.ID, accountCurrency);
								}
								break;
							}
							num++;
							continue;
						}
						break;
					}
				}
				if (m_realCurrencyData != null)
				{
					if (!(m_realCurrencyData.Price <= 0f))
					{
						UIManager.SetGameObjectActive(m_container, true);
						m_canAfford = true;
						goto IL_02aa;
					}
				}
				UIManager.SetGameObjectActive(m_container, false);
				return false;
			}
			m_currencyData = parent.GetItemCost(item, m_currencyType);
			m_realCurrencyData = null;
			UIManager.SetGameObjectActive(m_container, m_currencyData.Amount > 0);
			if (m_currencyData.Amount <= 0)
			{
				return false;
			}
			m_canAfford = ClientGameManager.Get().PlayerWallet.CanAfford(m_currencyData);
			goto IL_02aa;
			IL_006e:
			UIManager.SetGameObjectActive(m_container, false);
			return false;
			IL_02aa:
			bool flag = GameBalanceVarsExtensions.MeetsPurchaseabilityConditions(item);
			bool flag2 = false;
			if (flag && item is GameBalanceVars.ColorUnlockData)
			{
				CharacterType index = (CharacterType)item.Index1;
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(index);
				CharacterColor characterColor = characterResourceLink.m_skins[item.Index2].m_patterns[item.Index3].m_colors[item.ID];
				flag2 = (characterColor.m_requiredLevelForEquip > ClientGameManager.Get().GetPlayerCharacterLevel(index));
				if (flag2)
				{
					flag = false;
				}
			}
			_SelectableBtn selectableButton = m_hitbox.selectableButton;
			int disabled;
			if (m_canAfford)
			{
				disabled = ((!flag) ? 1 : 0);
			}
			else
			{
				disabled = 1;
			}
			selectableButton.SetDisabled((byte)disabled != 0);
			m_tooltipMessage = null;
			if (!flag)
			{
				if (!flag2)
				{
					if (!item.PurchaseDescription.IsNullOrEmpty())
					{
						m_tooltipMessage = item.GetPurchaseDescription();
					}
				}
			}
			CurrencyType currencyType = m_currencyType;
			string text;
			switch (currencyType)
			{
			default:
				if (currencyType != CurrencyType.NONE)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							throw new NotImplementedException(string.Concat(m_currencyType, " doesn't have a sprite"));
						}
					}
				}
				text = null;
				break;
			case CurrencyType.ISO:
				text = "iso";
				break;
			case CurrencyType.FreelancerCurrency:
				text = "credit";
				break;
			case CurrencyType.ModToken:
				text = "modToken";
				break;
			case CurrencyType.RankedCurrency:
				text = "rankedCurrency";
				break;
			case CurrencyType.RAFPoints:
				text = "referToken";
				break;
			}
			for (int i = 0; i < m_labels.Length; i++)
			{
				if (text == null)
				{
					m_labels[i].text = UIStorePanel.GetLocalizedPriceString(m_realCurrencyData.Price, m_realCurrencyData.Currency.ToString());
				}
				else
				{
					m_labels[i].text = $"<sprite name={text}>{UIStorePanel.FormatIntToString(m_currencyData.Amount, true)}";
				}
			}
			while (true)
			{
				return true;
			}
		}

		public bool CanAfford()
		{
			return m_canAfford;
		}

		public string GetTooltipMessage()
		{
			return m_tooltipMessage;
		}
	}

	private struct AdjustedPlayerUnlockable
	{
		public GameBalanceVars.PlayerUnlockable Unlockable;

		public int AdjustedSortOrder;
	}

	[Header("Note: Banners are not freelancer items, even in freelancer page")]
	public bool m_isFreelancerItems;

	public GridLayoutGroup m_itemsGrid;

	public UIStorePageIndicator m_pageItemPrefab;

	public GridLayoutGroup m_pageListContainer;

	public _ButtonSwapSprite m_prevPage;

	public _ButtonSwapSprite m_nextPage;

	public RectTransform m_buyContainer;

	public TextMeshProUGUI m_buyLabel;

	public BuyButton[] m_buyButtons;

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

	private Action<UIStoreBaseInventoryPanel, int, int> OnCountsRefreshedHolder;
	public event Action<UIStoreBaseInventoryPanel, int, int> OnCountsRefreshed
	{
		add
		{
			Action<UIStoreBaseInventoryPanel, int, int> action = this.OnCountsRefreshedHolder;
			Action<UIStoreBaseInventoryPanel, int, int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnCountsRefreshedHolder, (Action<UIStoreBaseInventoryPanel, int, int>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<UIStoreBaseInventoryPanel, int, int> action = this.OnCountsRefreshedHolder;
			Action<UIStoreBaseInventoryPanel, int, int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnCountsRefreshedHolder, (Action<UIStoreBaseInventoryPanel, int, int>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	protected UIStoreBaseInventoryPanel()
	{
		if (_003C_003Ef__am_0024cache1 == null)
		{
			_003C_003Ef__am_0024cache1 = delegate
			{
			};
		}
		this.OnCountsRefreshedHolder = _003C_003Ef__am_0024cache1;
		
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
			currencyData.Amount = item.GetUnlockISOPrice();
		}
		else if (type == CurrencyType.FreelancerCurrency)
		{
			currencyData.Amount = item.GetUnlockFreelancerCurrencyPrice();
		}
		else if (type == CurrencyType.RankedCurrency)
		{
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
		if (isInitialized)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		isInitialized = true;
		InitRawItemsList();
		if (HitchDetector.Get() != null)
		{
			HitchDetector.Get().AddNewLayoutGroup(m_itemsGrid);
		}
		m_pageNum = 0;
		m_itemBtns = m_itemsGrid.GetComponentsInChildren<UIStoreItemBtn>();
		m_visibleItemsList = new List<GameBalanceVars.PlayerUnlockable>();
		m_pageMarkers = new List<UIStorePageIndicator>();
		UIStoreItemBtn[] itemBtns = m_itemBtns;
		foreach (UIStoreItemBtn uIStoreItemBtn in itemBtns)
		{
			uIStoreItemBtn.m_selectableBtn.spriteController.RegisterScrollListener(OnScroll);
			uIStoreItemBtn.SetParent(this);
			UIManager.SetGameObjectActive(uIStoreItemBtn, false);
			StaggerComponent.SetStaggerComponent(uIStoreItemBtn.gameObject, true);
		}
		Image component = GetComponent<Image>();
		if (component != null)
		{
			UIEventTriggerUtils.AddListener(component.gameObject, EventTriggerType.Scroll, OnScroll);
		}
		Toggle[] filters = GetFilters();
		foreach (Toggle toggle in filters)
		{
			toggle.onValueChanged.AddListener(OnFilterChange);
		}
		while (true)
		{
			if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
			{
				RefreshVisibleItemsList();
				ShowPage(0);
			}
			for (int k = 0; k < m_buyButtons.Length; k++)
			{
				BuyButton buyButton = m_buyButtons[k];
				buyButton.m_hitbox.callback = BuyButtonClicked;
				buyButton.m_hitbox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, (UITooltipBase tooltip) => BuyButtonTooltipSetup(tooltip, buyButton));
				buyButton.m_hitbox.SetForceHovercallback(true);
				buyButton.m_hitbox.SetForceExitCallback(true);
			}
			while (true)
			{
				switch (5)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	protected void Start()
	{
		Initialize();
		if (m_isFreelancerItems)
		{
			ClientGameManager.Get().OnCharacterDataUpdated += OnCharacterDataUpdated;
		}
		else
		{
			ClientGameManager.Get().OnAccountDataUpdated += OnAccountDataUpdated;
		}
		ClientGameManager.Get().OnLobbyGameplayOverridesChange += OnLobbyGameplayOverridesChange;
	}

	protected void OnDestroy()
	{
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (m_isFreelancerItems)
			{
				ClientGameManager.Get().OnCharacterDataUpdated -= OnCharacterDataUpdated;
			}
			else
			{
				ClientGameManager.Get().OnAccountDataUpdated -= OnAccountDataUpdated;
			}
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= OnLobbyGameplayOverridesChange;
			return;
		}
	}

	protected void UpdatePanel()
	{
		if (m_parentContainer != null && m_parentContainer.gameObject.activeInHierarchy)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					RefreshVisibleItemsList();
					ShowPage(m_pageNum);
					return;
				}
			}
		}
		isUpdatePending = true;
	}

	private void OnAccountDataUpdated(PersistedAccountData data)
	{
		UpdatePanel();
	}

	public void OnCharacterDataUpdated(PersistedCharacterData newData)
	{
		if (m_charType != newData.CharacterType)
		{
			return;
		}
		while (true)
		{
			UpdatePanel();
			return;
		}
	}

	private void OnLobbyGameplayOverridesChange(LobbyGameplayOverrides gameplayOverrides)
	{
		UpdatePanel();
	}

	private void OnFilterChange(bool newValue)
	{
		RefreshVisibleItemsList();
		ShowPage(0);
	}

	public void HandlePendingUpdates()
	{
		if (!isInitialized)
		{
			return;
		}
		while (true)
		{
			if (isUpdatePending)
			{
				while (true)
				{
					RefreshVisibleItemsList();
					ShowPage(m_pageNum);
					isUpdatePending = false;
					return;
				}
			}
			return;
		}
	}

	private void InitRawItemsList()
	{
		m_rawItemsList = GetRawItemsList();
	}

	public void SetParentContainer(GameObject parentContainer)
	{
		m_parentContainer = parentContainer;
	}

	public void SetCharacter(CharacterType type)
	{
		m_charType = type;
		InitRawItemsList();
		RefreshVisibleItemsList();
		ShowPage(0);
		isUpdatePending = false;
	}

	protected List<GameBalanceVars.PlayerUnlockable> SortItems(List<GameBalanceVars.PlayerUnlockable> input)
	{
		List<AdjustedPlayerUnlockable> list = new List<AdjustedPlayerUnlockable>();
		for (int i = 0; i < input.Count; i++)
		{
			list.Add(new AdjustedPlayerUnlockable
			{
				Unlockable = input[i],
				AdjustedSortOrder = input[i].m_sortOrder * input.Count + i
			});
		}
		while (true)
		{
			if (_003C_003Ef__am_0024cache0 == null)
			{
				_003C_003Ef__am_0024cache0 = ((AdjustedPlayerUnlockable first, AdjustedPlayerUnlockable second) => first.AdjustedSortOrder.CompareTo(second.AdjustedSortOrder));
			}
			list.Sort(_003C_003Ef__am_0024cache0);
			List<GameBalanceVars.PlayerUnlockable> list2 = new List<GameBalanceVars.PlayerUnlockable>();
			for (int j = 0; j < list.Count; j++)
			{
				AdjustedPlayerUnlockable adjustedPlayerUnlockable = list[j];
				list2.Add(adjustedPlayerUnlockable.Unlockable);
			}
			while (true)
			{
				if (list2.Count > 0)
				{
					if (list2[list2.Count - 1].m_sortOrder != 0)
					{
						while (list2[0].m_sortOrder == 0)
						{
							GameBalanceVars.PlayerUnlockable item = list2[0];
							list2.RemoveAt(0);
							list2.Add(item);
						}
					}
				}
				return list2;
			}
		}
	}

	protected void RefreshVisibleItemsList()
	{
		LobbyGameplayOverrides gameplayOverrides = GameManager.Get().GameplayOverrides;
		bool enableHiddenCharacters = gameplayOverrides.EnableHiddenCharacters;
		m_numOwned = (m_numTotal = 0);
		m_visibleItemsList.Clear();
		for (int i = 0; i < m_rawItemsList.Length; i++)
		{
			if (m_rawItemsList[i] is GameBalanceVars.ColorUnlockData)
			{
				if (!gameplayOverrides.IsColorAllowed((CharacterType)m_rawItemsList[i].Index1, m_rawItemsList[i].Index2, m_rawItemsList[i].Index3, m_rawItemsList[i].ID))
				{
					continue;
				}
			}
			else if (m_rawItemsList[i] is GameBalanceVars.PlayerRibbon)
			{
				if (!FactionWideData.Get().IsRibbonInCompetition(m_rawItemsList[i].ID, ClientGameManager.Get().ActiveFactionCompetition))
				{
					continue;
				}
			}
			if (!m_rawItemsList[i].IsOwned())
			{
				int num;
				if (!m_rawItemsList[i].m_isHidden)
				{
					num = ((!GameBalanceVarsExtensions.MeetsVisibilityConditions(m_rawItemsList[i])) ? 1 : 0);
				}
				else
				{
					num = 1;
				}
				bool flag = (byte)num != 0;
				if (!flag)
				{
					if (m_rawItemsList[i] is GameBalanceVars.ColorUnlockData)
					{
						flag = GameBalanceVars.Get().GetCharacterUnlockData((CharacterType)m_rawItemsList[i].Index1).skinUnlockData[m_rawItemsList[i].Index2].m_isHidden;
						if (!flag)
						{
							flag = GameBalanceVars.Get().GetCharacterUnlockData((CharacterType)m_rawItemsList[i].Index1).skinUnlockData[m_rawItemsList[i].Index2].patternUnlockData[m_rawItemsList[i].Index3].m_isHidden;
						}
					}
				}
				if (!enableHiddenCharacters && flag)
				{
					continue;
				}
			}
			else
			{
				m_numOwned++;
			}
			m_numTotal++;
			if (ShouldFilter(m_rawItemsList[i]))
			{
			}
			else
			{
				m_visibleItemsList.Add(m_rawItemsList[i]);
			}
		}
		while (true)
		{
			this.OnCountsRefreshedHolder(this, m_numOwned, m_numTotal);
			m_numberOfPages = m_visibleItemsList.Count / m_itemBtns.Length;
			if (m_visibleItemsList.Count % m_itemBtns.Length > 0)
			{
				m_numberOfPages++;
			}
			for (int j = m_pageMarkers.Count; j < m_numberOfPages; j++)
			{
				UIStorePageIndicator uIStorePageIndicator = UnityEngine.Object.Instantiate(m_pageItemPrefab);
				uIStorePageIndicator.transform.SetParent(m_pageListContainer.transform);
				uIStorePageIndicator.transform.localScale = Vector3.one;
				uIStorePageIndicator.transform.localPosition = Vector3.zero;
				uIStorePageIndicator.SetSelected(j == 0);
				uIStorePageIndicator.m_hitbox.callback = PageClicked;
				uIStorePageIndicator.m_hitbox.RegisterScrollListener(OnScroll);
				uIStorePageIndicator.SetPageNumber(j + 1);
				m_pageMarkers.Add(uIStorePageIndicator);
			}
			while (true)
			{
				int num2;
				for (num2 = m_numberOfPages; num2 < m_pageMarkers.Count; num2++)
				{
					UnityEngine.Object.Destroy(m_pageMarkers[num2].gameObject);
					m_pageMarkers.RemoveAt(num2);
					num2--;
				}
				while (true)
				{
					m_nextPage.transform.parent.SetAsLastSibling();
					m_prevPage.callback = ClickedOnPrevPage;
					m_nextPage.callback = ClickedOnNextPage;
					UIManager.SetGameObjectActive(m_pageListContainer, m_numberOfPages > 1);
					return;
				}
			}
		}
	}

	public void PageClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
		for (int i = 0; i < m_pageMarkers.Count; i++)
		{
			if (!(m_pageMarkers[i].m_hitbox.gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject))
			{
				continue;
			}
			while (true)
			{
				ShowPage(i);
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

	private void OnScroll(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		Vector2 scrollDelta = pointerEventData.scrollDelta;
		if (scrollDelta.y > 0f)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					ClickedOnPrevPage(null);
					return;
				}
			}
		}
		Vector2 scrollDelta2 = pointerEventData.scrollDelta;
		if (!(scrollDelta2.y < 0f))
		{
			return;
		}
		while (true)
		{
			ClickedOnNextPage(null);
			return;
		}
	}

	public void ClickedOnPrevPage(BaseEventData data)
	{
		if (m_pageNum - 1 < 0)
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
		UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
		ShowPage(m_pageNum - 1);
	}

	public void ClickedOnNextPage(BaseEventData data)
	{
		if (m_pageNum + 1 >= m_numberOfPages)
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
		UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
		ShowPage(m_pageNum + 1);
	}

	private void ShowPage(int pageNum)
	{
		Initialize();
		for (int i = 0; i < m_pageMarkers.Count; i++)
		{
			m_pageMarkers[i].SetSelected(i == pageNum);
		}
		while (true)
		{
			if (m_pageNum != pageNum)
			{
				m_currentSelectedButton = null;
			}
			m_pageNum = pageNum;
			int num = m_itemBtns.Length * pageNum;
			for (int j = 0; j < m_itemBtns.Length; j++)
			{
				GameBalanceVars.PlayerUnlockable playerUnlockable = null;
				int num2 = j + num;
				if (num2 < m_visibleItemsList.Count)
				{
					playerUnlockable = m_visibleItemsList[num2];
				}
				m_itemBtns[j].Setup(playerUnlockable);
				if (playerUnlockable != null)
				{
					m_itemBtns[j].DisplayCheckMark(ShouldCheckmark(playerUnlockable));
				}
				m_itemBtns[j].m_selectableBtn.SetSelected(false, false, string.Empty, string.Empty);
			}
			while (true)
			{
				if (m_currentSelectedButton != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							DoItemSelect(m_currentSelectedButton, m_currentSelectedButton.GetItem());
							return;
						}
					}
				}
				m_currentSelectedButton = null;
				m_currentSelectedItem = null;
				UIManager.SetGameObjectActive(m_buyContainer, false);
				return;
			}
		}
	}

	public void RefreshPage()
	{
		ShowPage(m_pageNum);
	}

	public int GetNumOwned()
	{
		return m_numOwned;
	}

	public int GetNumTotal()
	{
		return m_numTotal;
	}

	public void DoItemClick(UIStoreItemBtn btn, GameBalanceVars.PlayerUnlockable item)
	{
		btn.m_selectableBtn.spriteController.ResetMouseState();
		DoItemSelect(btn, item);
		ItemClicked(item);
	}

	private void DoItemSelect(UIStoreItemBtn btn, GameBalanceVars.PlayerUnlockable item)
	{
		if (m_currentSelectedButton != null)
		{
			UIManager.SetGameObjectActive(m_currentSelectedButton.m_selectableBtn.m_selectedContainer, false);
		}
		m_currentSelectedItem = item;
		m_currentSelectedButton = btn;
		if (item == null)
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
		UIManager.SetGameObjectActive(btn.m_selectableBtn.m_selectedContainer, true);
		btn.transform.SetAsLastSibling();
		bool doActive = false;
		for (int i = 0; i < m_buyButtons.Length; i++)
		{
			if (m_buyButtons[i].Setup(item, this))
			{
				doActive = true;
			}
		}
		while (true)
		{
			if (item is GameBalanceVars.StoreItemForPurchase)
			{
				m_buyLabel.text = StringUtil.TR("Purchase", "Global");
			}
			else
			{
				m_buyLabel.text = StringUtil.TR("Unlock", "OverlayScreensScene");
			}
			UIManager.SetGameObjectActive(m_buyContainer, doActive);
			ItemSelected(item);
			return;
		}
	}

	public bool SelectItem(InventoryItemTemplate template)
	{
		return SelectItem(delegate(GameBalanceVars.PlayerUnlockable unlockable)
		{
			if (unlockable is GameBalanceVars.PlayerTitle)
			{
				if (template.Type == InventoryItemType.TitleID)
				{
					if (unlockable.ID == template.TypeSpecificData[0])
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.PlayerBanner)
			{
				if (template.Type == InventoryItemType.BannerID)
				{
					if (unlockable.ID == template.TypeSpecificData[0])
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.SkinUnlockData)
			{
				if (template.Type == InventoryItemType.Skin)
				{
					if (unlockable.Index1 == template.TypeSpecificData[0])
					{
						if (unlockable.ID == template.TypeSpecificData[1])
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									return true;
								}
							}
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.PatternUnlockData && template.Type == InventoryItemType.Texture)
			{
				if (unlockable.Index1 == template.TypeSpecificData[0])
				{
					if (unlockable.Index2 == template.TypeSpecificData[1])
					{
						if (unlockable.ID == template.TypeSpecificData[2])
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
					}
				}
			}
			if (unlockable is GameBalanceVars.ColorUnlockData)
			{
				if (template.Type == InventoryItemType.Style && unlockable.Index1 == template.TypeSpecificData[0])
				{
					if (unlockable.Index2 == template.TypeSpecificData[1] && unlockable.Index3 == template.TypeSpecificData[2])
					{
						if (unlockable.ID == template.TypeSpecificData[3])
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									return true;
								}
							}
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.TauntUnlockData)
			{
				if (template.Type == InventoryItemType.Taunt)
				{
					if (unlockable.Index1 == template.TypeSpecificData[0])
					{
						if (unlockable.ID == template.TypeSpecificData[1])
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									break;
								default:
									return true;
								}
							}
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.AbilityModUnlockData)
			{
				if (template.Type == InventoryItemType.Mod && unlockable.Index1 == template.TypeSpecificData[0])
				{
					if (unlockable.ID == template.TypeSpecificData[1])
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.AbilityVfxUnlockData)
			{
				if (template.Type == InventoryItemType.AbilityVfxSwap && unlockable.Index1 == template.TypeSpecificData[0])
				{
					if (unlockable.ID == template.TypeSpecificData[1])
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
				}
			}
			if (unlockable is GameBalanceVars.ChatEmoticon)
			{
				if (template.Type == InventoryItemType.ChatEmoji)
				{
					if (unlockable.ID == template.TypeSpecificData[0])
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.OverconUnlockData && template.Type == InventoryItemType.Overcon)
			{
				if (unlockable.ID == template.TypeSpecificData[0])
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.StoreItemForPurchase)
			{
				if (template.Type == InventoryItemType.FreelancerExpBonus)
				{
					if ((unlockable as GameBalanceVars.StoreItemForPurchase).m_itemTemplateId == template.Index)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.LoadingScreenBackground && template.Type == InventoryItemType.LoadingScreenBackground)
			{
				if (unlockable.ID == template.TypeSpecificData[0])
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
			}
			return false;
		});
	}

	public bool SelectItem(UIPurchaseableItem item)
	{
		if (item.m_itemType == PurchaseItemType.InventoryItem)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return SelectItem(InventoryWideData.Get().GetItemTemplate(item.m_inventoryTemplateId));
				}
			}
		}
		return SelectItem(delegate(GameBalanceVars.PlayerUnlockable unlockable)
		{
			if (unlockable is GameBalanceVars.PlayerTitle && item.m_itemType == PurchaseItemType.Title)
			{
				if (unlockable.ID == item.m_titleID)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.PlayerBanner)
			{
				if (item.m_itemType == PurchaseItemType.Banner)
				{
					if (unlockable.ID == item.m_bannerID)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.SkinUnlockData)
			{
				if (item.m_itemType == PurchaseItemType.Skin)
				{
					if (unlockable.Index1 == (int)item.m_charLink.m_characterType)
					{
						if (unlockable.ID == item.m_skinIndex)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									return true;
								}
							}
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.PatternUnlockData)
			{
				if (item.m_itemType == PurchaseItemType.Texture)
				{
					if (unlockable.Index1 == (int)item.m_charLink.m_characterType)
					{
						if (unlockable.Index2 == item.m_skinIndex && unlockable.ID == item.m_textureIndex)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									break;
								default:
									return true;
								}
							}
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.ColorUnlockData)
			{
				if (item.m_itemType == PurchaseItemType.Tint && unlockable.Index1 == (int)item.m_charLink.m_characterType)
				{
					if (unlockable.Index2 == item.m_skinIndex)
					{
						if (unlockable.Index3 == item.m_textureIndex)
						{
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
				if (item.m_itemType == PurchaseItemType.Taunt)
				{
					if (unlockable.Index1 == (int)item.m_charLink.m_characterType)
					{
						if (unlockable.ID == item.m_tauntIndex)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									return true;
								}
							}
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.AbilityVfxUnlockData && item.m_itemType == PurchaseItemType.AbilityVfx)
			{
				if (unlockable.Index1 == (int)item.m_charLink.m_characterType)
				{
					if (unlockable.ID == item.m_abilityVfxID)
					{
						return true;
					}
				}
			}
			if (unlockable is GameBalanceVars.ChatEmoticon)
			{
				if (item.m_itemType == PurchaseItemType.Emoticon && unlockable.ID == item.m_emoticonID)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.OverconUnlockData)
			{
				if (item.m_itemType == PurchaseItemType.Overcon && unlockable.ID == item.m_overconID)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
			}
			if (unlockable is GameBalanceVars.LoadingScreenBackground)
			{
				if (item.m_itemType == PurchaseItemType.LoadingScreenBackground && unlockable.ID == item.m_loadingScreenBackgroundId)
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
			}
			if (unlockable is GameBalanceVars.CharacterUnlockData)
			{
				if (item.m_itemType == PurchaseItemType.Character)
				{
					if (unlockable.ID == (int)item.m_charLink.m_characterType)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
				}
			}
			return false;
		});
	}

	private bool SelectItem(Func<GameBalanceVars.PlayerUnlockable, bool> foundCheck)
	{
		Initialize();
		HandlePendingUpdates();
		for (int i = 0; i < m_visibleItemsList.Count; i++)
		{
			if (!foundCheck(m_visibleItemsList[i]))
			{
				continue;
			}
			while (true)
			{
				int num = i % m_itemBtns.Length;
				int pageNum;
				if (i > 0)
				{
					pageNum = i / m_itemBtns.Length;
				}
				else
				{
					pageNum = 0;
				}
				ShowPage(pageNum);
				DoItemSelect(m_itemBtns[num], m_itemBtns[num].GetItem());
				return true;
			}
		}
		while (true)
		{
			return false;
		}
	}

	private BuyButton FindBuyButtonFromEventData(BaseEventData data, bool isHover)
	{
		PointerEventData pointerEventData = (PointerEventData)data;
		if (pointerEventData.selectedObject == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		BuyButton result = null;
		for (int i = 0; i < m_buyButtons.Length; i++)
		{
			GameObject gameObject = m_buyButtons[i].m_hitbox.gameObject;
			GameObject y;
			if (isHover)
			{
				y = pointerEventData.pointerEnter;
			}
			else
			{
				y = pointerEventData.selectedObject;
			}
			if (gameObject == y)
			{
				result = m_buyButtons[i];
				break;
			}
		}
		return result;
	}

	private void BuyButtonClicked(BaseEventData data)
	{
		if (m_currentSelectedItem == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		BuyButton buyButton = FindBuyButtonFromEventData(data, false);
		if (buyButton == null)
		{
			return;
		}
		while (true)
		{
			if (buyButton.CanAfford())
			{
				UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
				PurchaseItem(m_currentSelectedItem, buyButton.m_currencyType);
				buyButton.m_hitbox.ResetMouseState();
			}
			return;
		}
	}

	private bool BuyButtonTooltipSetup(UITooltipBase tooltip, BuyButton buyButton)
	{
		string tooltipMessage = buyButton.GetTooltipMessage();
		if (!tooltipMessage.IsNullOrEmpty())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					(tooltip as UITitledTooltip).Setup(StringUtil.TR("Purchase", "Global"), tooltipMessage, string.Empty);
					return true;
				}
			}
		}
		if (!buyButton.CanAfford())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					CurrencyType currencyType = buyButton.m_currencyType;
					string term;
					if (currencyType != 0)
					{
						if (currencyType != CurrencyType.ModToken)
						{
							if (currencyType != CurrencyType.RankedCurrency)
							{
								if (currencyType != CurrencyType.FreelancerCurrency)
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											break;
										default:
											throw new NotImplementedException(string.Concat(buyButton.m_currencyType, " does not have can't buy message"));
										}
									}
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
				}
			}
		}
		return false;
	}
}
