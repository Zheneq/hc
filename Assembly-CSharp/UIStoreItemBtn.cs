using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStoreItemBtn : MonoBehaviour
{
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

	public UIStoreItemBtn.CurrencyLabels[] m_currencyLabels;

	private const float kDimmedState = 0.5f;

	private GameBalanceVars.PlayerUnlockable m_item;

	private UIStoreBaseInventoryPanel m_panel;

	private UITooltipHoverObject m_tooltipHoverObj;

	private Dictionary<TextMeshProUGUI, float> m_overrideStoreItemAlpha = new Dictionary<TextMeshProUGUI, float>();

	private void Awake()
	{
		this.m_selectableBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.Clicked);
	}

	public void SetParent(UIStoreBaseInventoryPanel panel)
	{
		this.m_panel = panel;
		if (panel.GetItemTooltipType() != null)
		{
			this.m_tooltipHoverObj = this.m_selectableBtn.spriteController.GetComponent<UITooltipHoverObject>();
			if (this.m_tooltipHoverObj != null)
			{
				this.m_tooltipHoverObj.Setup(panel.GetItemTooltipType().Value, new TooltipPopulateCall(this.TooltipPopulateEvent), null);
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
			if (this.m_item != null)
			{
				if (this.m_item.GetType() == item.GetType())
				{
					if (this.m_item.ID == item.ID)
					{
						if (this.m_item.Index1 == item.Index1)
						{
							if (this.m_item.Index2 == item.Index2)
							{
								if (this.m_item.Index3 == item.Index3)
								{
									if (!this.m_ownedContainer.gameObject.activeSelf)
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
			this.m_item = item;
			Sprite sprite = Resources.Load<Sprite>(item.GetSpritePath());
			this.SetSprite(sprite, this.m_icons);
			this.SetSprite(sprite, this.m_bannerIcons);
			bool flag3 = item is GameBalanceVars.PlayerBanner && (item as GameBalanceVars.PlayerBanner).m_type == GameBalanceVars.PlayerBanner.BannerType.Background;
			this.SetVisible(!flag3, this.m_icons);
			this.SetVisible(flag3, this.m_bannerIcons);
			this.SetVisible(flag3, this.m_bannerShadow);
			Sprite itemFg = item.GetItemFg();
			this.SetSprite(itemFg, this.m_iconFgs);
			bool visible;
			if (!flag3)
			{
				visible = (itemFg != null);
			}
			else
			{
				visible = false;
			}
			this.SetVisible(visible, this.m_iconFgs);
			UIManager.SetGameObjectActive(this.m_levelLockContainer, false, null);
			this.SetVisible(!(item is GameBalanceVars.PlayerTitle), this.m_iconContainers);
			this.SetVisible(item is GameBalanceVars.PlayerTitle, this.m_titleContainers);
			this.SetVisible(false, this.m_modIcons);
			string text = item.Rarity.GetColorHexString();
			string text2;
			if (item is GameBalanceVars.ColorUnlockData)
			{
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)item.Index1);
				CharacterColor characterColor = characterResourceLink.m_skins[item.Index2].m_patterns[item.Index3].m_colors[item.ID];
				UIManager.SetGameObjectActive(this.m_levelLockContainer, characterColor.m_requiredLevelForEquip > 0, null);
				this.m_levelLockText.text = string.Format(StringUtil.TR("LevelRequirement", "Rewards"), characterColor.m_requiredLevelForEquip);
				text2 = characterResourceLink.GetPatternColorName(item.Index2, item.Index3, item.ID);
				if (characterColor.m_requiredLevelForEquip >= 0x14)
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
				text2 = (item as GameBalanceVars.PlayerTitle).GetTitleText(-1);
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
				this.SetSprite(abilityMod.m_iconSprite, this.m_modIcons);
				UIManager.SetGameObjectActive(this.m_levelLockContainer, false, null);
				this.SetVisible(true, this.m_modIcons);
				this.SetVisible(false, this.m_icons);
			}
			else if (item is GameBalanceVars.AbilityVfxUnlockData)
			{
				CharacterResourceLink characterResourceLink4 = GameWideData.Get().GetCharacterResourceLink((CharacterType)item.Index1);
				text2 = characterResourceLink4.GetVFXSwapName(item.Index2, item.ID);
			}
			else if (item is GameBalanceVars.OverconUnlockData)
			{
				UIOverconData.NameToOverconEntry nameToOverconEntry = null;
				foreach (UIOverconData.NameToOverconEntry nameToOverconEntry2 in UIOverconData.Get().m_nameToOverconEntry)
				{
					if (nameToOverconEntry2.m_overconId == item.ID)
					{
						nameToOverconEntry = nameToOverconEntry2;
						break;
					}
				}
				if (nameToOverconEntry != null)
				{
					text2 = nameToOverconEntry.GetDisplayName();
				}
				else
				{
					text2 = string.Format("#overcon{0}", item.ID);
				}
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
			this.SetVisible(!(item is GameBalanceVars.PlayerTitle), this.m_nameTexts);
			this.SetText(string.Concat(new string[]
			{
				"<color=",
				text,
				">",
				text2,
				"</color>"
			}), this.m_nameTexts, false);
			this.SetText(text2, this.m_titleTexts, false);
			bool flag4 = GameBalanceVarsExtensions.MeetsPurchaseabilityConditions(item);
			for (int i = 0; i < this.m_currencyLabels.Length; i++)
			{
				string text3;
				float num;
				if (this.m_currencyLabels[i].m_currencyType == CurrencyType.ISO)
				{
					text3 = "iso";
					num = (float)item.GetUnlockISOPrice();
				}
				else if (this.m_currencyLabels[i].m_currencyType == CurrencyType.RankedCurrency)
				{
					text3 = "rankedCurrency";
					num = (float)item.GetUnlockRankedCurrencyPrice();
				}
				else if (this.m_currencyLabels[i].m_currencyType == CurrencyType.FreelancerCurrency)
				{
					text3 = "credit";
					if (item is GameBalanceVars.AbilityModUnlockData)
					{
						num = (float)GameBalanceVars.Get().FreelancerCurrencyToUnlockMod;
					}
					else
					{
						num = (float)item.GetUnlockFreelancerCurrencyPrice();
					}
				}
				else if (this.m_currencyLabels[i].m_currencyType == CurrencyType.ModToken)
				{
					text3 = "modToken";
					num = 0f;
				}
				else
				{
					if (this.m_currencyLabels[i].m_currencyType != CurrencyType.NONE)
					{
						throw new NotImplementedException("Implement for type " + this.m_currencyLabels[i].m_currencyType);
					}
					text3 = null;
					if (item is GameBalanceVars.ColorUnlockData)
					{
						num = CommerceClient.Get().GetStylePrice((CharacterType)item.Index1, item.Index2, item.Index3, item.ID, HydrogenConfig.Get().Ticket.AccountCurrency);
					}
					else if (item is GameBalanceVars.StoreItemForPurchase)
					{
						int itemTemplateId = ((GameBalanceVars.StoreItemForPurchase)item).m_itemTemplateId;
						float num2;
						num = CommerceClient.Get().GetStoreItemPrice(itemTemplateId, HydrogenConfig.Get().Ticket.AccountCurrency, out num2);
					}
					else
					{
						num = item.GetRealCurrencyPrice();
					}
				}
				this.SetVisible(num > 0f, this.m_currencyLabels[i].m_allCostLabels);
				if (text3.IsNullOrEmpty())
				{
					this.SetText(UIStorePanel.GetLocalizedPriceString(num, HydrogenConfig.Get().Ticket.AccountCurrency), this.m_currencyLabels[i].m_allCostLabels, flag2);
				}
				else
				{
					this.SetText(string.Format("<sprite name={0}>{1}", text3, UIStorePanel.FormatIntToString((int)num, true)), this.m_currencyLabels[i].m_allCostLabels, flag2);
				}
				if (this.m_currencyLabels[i].m_normalCost != null && this.m_currencyLabels[i].m_unavailableCost != null)
				{
					if (num > 0f)
					{
						UIManager.SetGameObjectActive(this.m_currencyLabels[i].m_normalCost, flag4, null);
						UIManager.SetGameObjectActive(this.m_currencyLabels[i].m_unavailableCost, !flag4, null);
					}
				}
			}
			Component ownedContainer = this.m_ownedContainer;
			bool doActive2;
			if (!flag)
			{
				doActive2 = flag2;
			}
			else
			{
				doActive2 = true;
			}
			UIManager.SetGameObjectActive(ownedContainer, doActive2, null);
			this.DisplayCheckMark(false);
		}
		else
		{
			this.m_item = null;
			this.SetVisible(false, this.m_nameTexts);
			for (int j = 0; j < this.m_currencyLabels.Length; j++)
			{
				this.SetVisible(false, this.m_currencyLabels[j].m_allCostLabels);
			}
			this.SetVisible(false, this.m_iconContainers);
			this.SetVisible(false, this.m_titleContainers);
			UIManager.SetGameObjectActive(this.m_levelLockContainer, false, null);
			UIManager.SetGameObjectActive(this.m_ownedContainer, false, null);
			this.DisplayCheckMark(false);
		}
		this.m_tooltipHoverObj.Refresh();
		UIManager.SetGameObjectActive(this.m_collectedGroup, doActive, null);
		this.m_selectableBtn.spriteController.ResetMouseState();
		this.m_selectableBtn.m_ignoreHoverAnimationCall = (item == null);
		this.m_selectableBtn.m_ignorePressAnimationCall = (item == null);
		UIManager.SetGameObjectActive(this.m_selectableBtn.m_selectedContainer, false, null);
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
					this.m_overrideStoreItemAlpha[tmps[i]] = tmps[i].alpha;
					tmps[i].alpha = 1f;
				}
			}
			else if (this.m_overrideStoreItemAlpha.ContainsKey(tmps[i]))
			{
				tmps[i].alpha = this.m_overrideStoreItemAlpha[tmps[i]];
				this.m_overrideStoreItemAlpha.Remove(tmps[i]);
			}
		}
	}

	private void SetSprite(Sprite sprite, Image[] tmps)
	{
		for (int i = 0; i < tmps.Length; i++)
		{
			tmps[i].sprite = sprite;
		}
	}

	private void SetVisible(bool visible, Image[] tmps)
	{
		for (int i = 0; i < tmps.Length; i++)
		{
			UIManager.SetGameObjectActive(tmps[i], visible, null);
		}
	}

	private void SetVisible(bool visible, TextMeshProUGUI[] tmps)
	{
		for (int i = 0; i < tmps.Length; i++)
		{
			UIManager.SetGameObjectActive(tmps[i], visible, null);
		}
	}

	private void SetVisible(bool visible, RectTransform[] tmps)
	{
		for (int i = 0; i < tmps.Length; i++)
		{
			UIManager.SetGameObjectActive(tmps[i], visible, null);
		}
	}

	public void DisplayCheckMark(bool display)
	{
		UIManager.SetGameObjectActive(this.m_checkMark, display, null);
	}

	public void Clicked(BaseEventData data)
	{
		if (this.m_panel != null && this.m_item != null)
		{
			this.m_panel.DoItemClick(this, this.m_item);
		}
	}

	private bool TooltipPopulateEvent(UITooltipBase tooltip)
	{
		if (this.m_item == null)
		{
			return false;
		}
		return this.m_panel.ItemTooltipPopulate(tooltip, this, this.m_item);
	}

	public GameBalanceVars.PlayerUnlockable GetItem()
	{
		return this.m_item;
	}

	[Serializable]
	public class CurrencyLabels
	{
		public CurrencyType m_currencyType;

		public TextMeshProUGUI[] m_allCostLabels;

		public RectTransform m_normalCost;

		public RectTransform m_unavailableCost;
	}
}
