using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStoreItemBtn : MonoBehaviour
{
	[Serializable]
	public class CurrencyLabels
	{
		public CurrencyType m_currencyType;

		public TextMeshProUGUI[] m_allCostLabels;

		public RectTransform m_normalCost;

		public RectTransform m_unavailableCost;
	}

	public _SelectableBtn m_selectableBtn;

	public RectTransform[] m_iconContainers;

	public Image[] m_icons;

	public Image[] m_iconFgs;

	public Image[] m_modIcons;

	public Image[] m_bannerIcons;

	public Image[] m_bannerShadow;

	public RectTransform[] m_titleContainers;

	public TextMeshProUGUI[] m_titleTexts;

	public TextMeshProUGUI[] m_nameTexts;

	public RectTransform m_checkMark;

	public RectTransform m_levelLockContainer;

	public TextMeshProUGUI m_levelLockText;

	public RectTransform m_ownedContainer;

	public RectTransform m_collectedGroup;

	public CurrencyLabels[] m_currencyLabels;

	private const float kDimmedState = 0.5f;

	private GameBalanceVars.PlayerUnlockable m_item;

	private UIStoreBaseInventoryPanel m_panel;

	private UITooltipHoverObject m_tooltipHoverObj;

	private Dictionary<TextMeshProUGUI, float> m_overrideStoreItemAlpha = new Dictionary<TextMeshProUGUI, float>();

	private void Awake()
	{
		m_selectableBtn.spriteController.callback = Clicked;
	}

	public void SetParent(UIStoreBaseInventoryPanel panel)
	{
		m_panel = panel;
		if (panel.GetItemTooltipType().HasValue)
		{
			m_tooltipHoverObj = m_selectableBtn.spriteController.GetComponent<UITooltipHoverObject>();
			if (m_tooltipHoverObj != null)
			{
				m_tooltipHoverObj.Setup(panel.GetItemTooltipType().Value, TooltipPopulateEvent);
			}
		}
	}

	public void Setup(GameBalanceVars.PlayerUnlockable item)
	{
		bool doActive = false;
		if (item != null)
		{
			bool flag = item.IsOwned();
			bool flag2 = false;
			if (m_item != null)
			{
				if (m_item.GetType() == item.GetType())
				{
					if (m_item.ID == item.ID)
					{
						if (m_item.Index1 == item.Index1)
						{
							if (m_item.Index2 == item.Index2)
							{
								if (m_item.Index3 == item.Index3)
								{
									if (!m_ownedContainer.gameObject.activeSelf)
									{
										if (flag)
										{
											doActive = true;
										}
									}
								}
							}
						}
					}
				}
			}
			m_item = item;
			Sprite sprite = Resources.Load<Sprite>(item.GetSpritePath());
			SetSprite(sprite, m_icons);
			SetSprite(sprite, m_bannerIcons);
			bool flag3 = item is GameBalanceVars.PlayerBanner && (item as GameBalanceVars.PlayerBanner).m_type == GameBalanceVars.PlayerBanner.BannerType.Background;
			SetVisible(!flag3, m_icons);
			SetVisible(flag3, m_bannerIcons);
			SetVisible(flag3, m_bannerShadow);
			Sprite itemFg = item.GetItemFg();
			SetSprite(itemFg, m_iconFgs);
			int visible;
			if (!flag3)
			{
				visible = ((itemFg != null) ? 1 : 0);
			}
			else
			{
				visible = 0;
			}
			SetVisible((byte)visible != 0, m_iconFgs);
			UIManager.SetGameObjectActive(m_levelLockContainer, false);
			SetVisible(!(item is GameBalanceVars.PlayerTitle), m_iconContainers);
			SetVisible(item is GameBalanceVars.PlayerTitle, m_titleContainers);
			SetVisible(false, m_modIcons);
			string text = item.Rarity.GetColorHexString();
			string text2;
			if (item is GameBalanceVars.ColorUnlockData)
			{
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)item.Index1);
				CharacterColor characterColor = characterResourceLink.m_skins[item.Index2].m_patterns[item.Index3].m_colors[item.ID];
				UIManager.SetGameObjectActive(m_levelLockContainer, characterColor.m_requiredLevelForEquip > 0);
				m_levelLockText.text = string.Format(StringUtil.TR("LevelRequirement", "Rewards"), characterColor.m_requiredLevelForEquip);
				text2 = characterResourceLink.GetPatternColorName(item.Index2, item.Index3, item.ID);
				if (characterColor.m_requiredLevelForEquip >= 20)
				{
					text = "#00E9FF";
				}
			}
			else if (item is GameBalanceVars.TauntUnlockData)
			{
				CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink((CharacterType)item.Index1);
				text2 = characterResourceLink2.GetTauntName(item.ID);
			}
			else if (item is GameBalanceVars.PlayerBanner)
			{
				text2 = StringUtil.TR_BannerName(item.ID);
			}
			else if (item is GameBalanceVars.PlayerTitle)
			{
				text2 = (item as GameBalanceVars.PlayerTitle).GetTitleText();
			}
			else if (item is GameBalanceVars.ChatEmoticon)
			{
				text2 = (item as GameBalanceVars.ChatEmoticon).GetEmojiName();
			}
			else if (item is GameBalanceVars.StoreItemForPurchase)
			{
				text2 = (item as GameBalanceVars.StoreItemForPurchase).GetStoreItemName();
				flag2 = true;
			}
			else if (item is GameBalanceVars.AbilityModUnlockData)
			{
				CharacterResourceLink characterResourceLink3 = GameWideData.Get().GetCharacterResourceLink((CharacterType)item.Index1);
				AbilityData component = characterResourceLink3.ActorDataPrefab.GetComponent<AbilityData>();
				Ability ability = null;
				switch (item.Index2)
				{
				case 0:
					ability = component.m_ability0;
					break;
				case 1:
					ability = component.m_ability1;
					break;
				case 2:
					ability = component.m_ability2;
					break;
				case 3:
					ability = component.m_ability3;
					break;
				case 4:
					ability = component.m_ability4;
					break;
				case 5:
					ability = component.m_ability5;
					break;
				case 6:
					ability = component.m_ability6;
					break;
				}
				AbilityMod abilityMod = AbilityModHelper.GetAvailableModsForAbility(ability).Find((AbilityMod x) => x.m_abilityScopeId == item.ID);
				text2 = abilityMod.GetName();
				SetSprite(abilityMod.m_iconSprite, m_modIcons);
				UIManager.SetGameObjectActive(m_levelLockContainer, false);
				SetVisible(true, m_modIcons);
				SetVisible(false, m_icons);
			}
			else if (item is GameBalanceVars.AbilityVfxUnlockData)
			{
				CharacterResourceLink characterResourceLink4 = GameWideData.Get().GetCharacterResourceLink((CharacterType)item.Index1);
				text2 = characterResourceLink4.GetVFXSwapName(item.Index2, item.ID);
			}
			else if (item is GameBalanceVars.OverconUnlockData)
			{
				UIOverconData.NameToOverconEntry nameToOverconEntry = null;
				foreach (UIOverconData.NameToOverconEntry item2 in UIOverconData.Get().m_nameToOverconEntry)
				{
					if (item2.m_overconId == item.ID)
					{
						nameToOverconEntry = item2;
					}
				}
				text2 = ((nameToOverconEntry == null) ? $"#overcon{item.ID}" : nameToOverconEntry.GetDisplayName());
			}
			else if (item is GameBalanceVars.LoadingScreenBackground)
			{
				GameBalanceVars.LoadingScreenBackground loadingScreenBackground = item as GameBalanceVars.LoadingScreenBackground;
				text2 = loadingScreenBackground.GetLoadingScreenBackgroundName();
			}
			else
			{
				text2 = item.Name + "#NotLocalized";
			}
			SetVisible(!(item is GameBalanceVars.PlayerTitle), m_nameTexts);
			SetText("<color=" + text + ">" + text2 + "</color>", m_nameTexts);
			SetText(text2, m_titleTexts);
			bool flag4 = GameBalanceVarsExtensions.MeetsPurchaseabilityConditions(item);
			for (int i = 0; i < m_currencyLabels.Length; i++)
			{
				string text3;
				float num;
				if (m_currencyLabels[i].m_currencyType == CurrencyType.ISO)
				{
					text3 = "iso";
					num = item.GetUnlockISOPrice();
				}
				else if (m_currencyLabels[i].m_currencyType == CurrencyType.RankedCurrency)
				{
					text3 = "rankedCurrency";
					num = item.GetUnlockRankedCurrencyPrice();
				}
				else if (m_currencyLabels[i].m_currencyType == CurrencyType.FreelancerCurrency)
				{
					text3 = "credit";
					num = ((!(item is GameBalanceVars.AbilityModUnlockData)) ? ((float)item.GetUnlockFreelancerCurrencyPrice()) : ((float)GameBalanceVars.Get().FreelancerCurrencyToUnlockMod));
				}
				else if (m_currencyLabels[i].m_currencyType == CurrencyType.ModToken)
				{
					text3 = "modToken";
					num = 0f;
				}
				else
				{
					if (m_currencyLabels[i].m_currencyType != CurrencyType.NONE)
					{
						throw new NotImplementedException("Implement for type " + m_currencyLabels[i].m_currencyType);
					}
					text3 = null;
					if (item is GameBalanceVars.ColorUnlockData)
					{
						num = CommerceClient.Get().GetStylePrice((CharacterType)item.Index1, item.Index2, item.Index3, item.ID, HydrogenConfig.Get().Ticket.AccountCurrency);
					}
					else if (item is GameBalanceVars.StoreItemForPurchase)
					{
						int itemTemplateId = ((GameBalanceVars.StoreItemForPurchase)item).m_itemTemplateId;
						num = CommerceClient.Get().GetStoreItemPrice(itemTemplateId, HydrogenConfig.Get().Ticket.AccountCurrency, out float _);
					}
					else
					{
						num = item.GetRealCurrencyPrice();
					}
				}
				SetVisible(num > 0f, m_currencyLabels[i].m_allCostLabels);
				if (text3.IsNullOrEmpty())
				{
					SetText(UIStorePanel.GetLocalizedPriceString(num, HydrogenConfig.Get().Ticket.AccountCurrency), m_currencyLabels[i].m_allCostLabels, flag2);
				}
				else
				{
					SetText($"<sprite name={text3}>{UIStorePanel.FormatIntToString((int)num, true)}", m_currencyLabels[i].m_allCostLabels, flag2);
				}
				if (!(m_currencyLabels[i].m_normalCost != null) || !(m_currencyLabels[i].m_unavailableCost != null))
				{
					continue;
				}
				if (num > 0f)
				{
					UIManager.SetGameObjectActive(m_currencyLabels[i].m_normalCost, flag4);
					UIManager.SetGameObjectActive(m_currencyLabels[i].m_unavailableCost, !flag4);
				}
			}
			RectTransform ownedContainer = m_ownedContainer;
			int doActive2;
			if (!flag)
			{
				doActive2 = (flag2 ? 1 : 0);
			}
			else
			{
				doActive2 = 1;
			}
			UIManager.SetGameObjectActive(ownedContainer, (byte)doActive2 != 0);
			DisplayCheckMark(false);
		}
		else
		{
			m_item = null;
			SetVisible(false, m_nameTexts);
			for (int j = 0; j < m_currencyLabels.Length; j++)
			{
				SetVisible(false, m_currencyLabels[j].m_allCostLabels);
			}
			SetVisible(false, m_iconContainers);
			SetVisible(false, m_titleContainers);
			UIManager.SetGameObjectActive(m_levelLockContainer, false);
			UIManager.SetGameObjectActive(m_ownedContainer, false);
			DisplayCheckMark(false);
		}
		m_tooltipHoverObj.Refresh();
		UIManager.SetGameObjectActive(m_collectedGroup, doActive);
		m_selectableBtn.spriteController.ResetMouseState();
		m_selectableBtn.m_ignoreHoverAnimationCall = (item == null);
		m_selectableBtn.m_ignorePressAnimationCall = (item == null);
		UIManager.SetGameObjectActive(m_selectableBtn.m_selectedContainer, false);
	}

	private void SetText(string text, TextMeshProUGUI[] tmps, bool storeItem = false)
	{
		for (int i = 0; i < tmps.Length; i++)
		{
			tmps[i].text = text;
			if (storeItem)
			{
				if (tmps[i].alpha != 1f)
				{
					m_overrideStoreItemAlpha[tmps[i]] = tmps[i].alpha;
					tmps[i].alpha = 1f;
				}
			}
			else if (m_overrideStoreItemAlpha.ContainsKey(tmps[i]))
			{
				tmps[i].alpha = m_overrideStoreItemAlpha[tmps[i]];
				m_overrideStoreItemAlpha.Remove(tmps[i]);
			}
		}
	}

	private void SetSprite(Sprite sprite, Image[] tmps)
	{
		for (int i = 0; i < tmps.Length; i++)
		{
			tmps[i].sprite = sprite;
		}
		while (true)
		{
			return;
		}
	}

	private void SetVisible(bool visible, Image[] tmps)
	{
		for (int i = 0; i < tmps.Length; i++)
		{
			UIManager.SetGameObjectActive(tmps[i], visible);
		}
	}

	private void SetVisible(bool visible, TextMeshProUGUI[] tmps)
	{
		for (int i = 0; i < tmps.Length; i++)
		{
			UIManager.SetGameObjectActive(tmps[i], visible);
		}
		while (true)
		{
			return;
		}
	}

	private void SetVisible(bool visible, RectTransform[] tmps)
	{
		for (int i = 0; i < tmps.Length; i++)
		{
			UIManager.SetGameObjectActive(tmps[i], visible);
		}
	}

	public void DisplayCheckMark(bool display)
	{
		UIManager.SetGameObjectActive(m_checkMark, display);
	}

	public void Clicked(BaseEventData data)
	{
		if (m_panel != null && m_item != null)
		{
			m_panel.DoItemClick(this, m_item);
		}
	}

	private bool TooltipPopulateEvent(UITooltipBase tooltip)
	{
		if (m_item == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		return m_panel.ItemTooltipPopulate(tooltip, this, m_item);
	}

	public GameBalanceVars.PlayerUnlockable GetItem()
	{
		return m_item;
	}
}
