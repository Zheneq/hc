using System;
using System.Collections.Generic;
using I2.Loc;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterGeneralPanel : MonoBehaviour
{
	public Image m_roleIcon;

	public TextMeshProUGUI m_name;

	public TextMeshProUGUI m_levelLabel;

	public TextMeshProUGUI m_takedownsLabel;

	public TextMeshProUGUI m_matchesWonLabel;

	public TextMeshProUGUI m_gamesPlayedLabel;

	public TextMeshProUGUI m_description;

	public ScrollRect m_descriptionScrollRect;

	public TextMeshProUGUI m_expAmountText;

	public Image m_experienceSlider;

	public Image m_nextRewardIcon;

	public Image m_nextRewardFG;

	public RectTransform m_rewardContainer;

	public UITooltipHoverObject m_rewardTooltipObj;

	public Sprite m_supportIcon;

	public Sprite m_assassinIcon;

	public Sprite m_tankIcon;

	public UINotchedFillBar m_healthFill;

	public UINotchedFillBar m_damageFill;

	public UINotchedFillBar m_survivalFill;

	public UINotchedFillBar m_difficultyFill;

	public _SelectableBtn m_twitterBtn;

	public TextMeshProUGUI[] m_twitterHandleLabel;

	public ScrollRect m_bottomScrollRect;

	public TextMeshProUGUI m_bioText;

	public RectTransform m_articlesContainer;

	public GridLayoutGroup m_articlesGrid;

	public UIArticleSelectButton m_articleSelectButtonPrefab;

	public RectTransform m_buyHeroContainer;

	public _SelectableBtn m_buyInGameButton;

	public TextMeshProUGUI[] m_buyInGameLabels;

	public _SelectableBtn m_buyForCashButton;

	public TextMeshProUGUI[] m_buyForCashLabels;

	public _SelectableBtn m_buyForTokenButton;

	private CharacterType m_characterType;

	private CharacterResourceLink m_charLink;

	private bool m_needResetScrollbar;

	private List<UIArticleSelectButton> m_articleButtons;

	private int m_expDataLevel;

	private List<RewardUtils.RewardData> m_rewards;

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(this, visible, null);
	}

	private void Update()
	{
		if (this.m_needResetScrollbar)
		{
			if (this.m_descriptionScrollRect.verticalScrollbar.gameObject.activeSelf)
			{
				if (this.m_descriptionScrollRect.verticalScrollbar.value != 1f)
				{
					this.m_needResetScrollbar = false;
					this.m_descriptionScrollRect.verticalScrollbar.value = 1f;
				}
			}
		}
	}

	private void Init()
	{
		if (this.m_articleButtons == null)
		{
			this.m_articleButtons = new List<UIArticleSelectButton>(this.m_articlesGrid.GetComponentsInChildren<UIArticleSelectButton>(true));
			using (List<UIArticleSelectButton>.Enumerator enumerator = this.m_articleButtons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIArticleSelectButton uiarticleSelectButton = enumerator.Current;
					uiarticleSelectButton.m_hitbox.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
				}
			}
		}
	}

	private void Awake()
	{
		this.Init();
		this.m_rewardTooltipObj.Setup(TooltipType.RewardList, new TooltipPopulateCall(this.PopulateTooltip), null);
		UIManager.SetGameObjectActive(this.m_articlesContainer, LocalizationManager.CurrentLanguageCode != "zh", null);
		if (this.m_buyHeroContainer != null)
		{
			this.m_buyInGameButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BuyInGameClicked);
			this.m_buyForCashButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BuyForCashClicked);
			this.m_buyForTokenButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BuyForTokenClicked);
		}
	}

	private bool PopulateTooltip(UITooltipBase tooltip)
	{
		if (this.m_rewards != null)
		{
			if (this.m_rewards.Count != 0)
			{
				UIRewardListTooltip uirewardListTooltip = tooltip as UIRewardListTooltip;
				uirewardListTooltip.Setup(this.m_rewards, this.m_expDataLevel, UIRewardListTooltip.RewardsType.Character, false);
				return true;
			}
		}
		return false;
	}

	private void Start()
	{
		if (this.m_descriptionScrollRect != null)
		{
			_MouseEventPasser mouseEventPasser = this.m_description.gameObject.AddComponent<_MouseEventPasser>();
			mouseEventPasser.AddNewHandler(this.m_descriptionScrollRect);
			mouseEventPasser = this.m_descriptionScrollRect.verticalScrollbar.handleRect.gameObject.AddComponent<_MouseEventPasser>();
			mouseEventPasser.AddNewHandler(this.m_descriptionScrollRect);
			this.m_descriptionScrollRect.scrollSensitivity = 75f;
		}
		this.m_twitterBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.TwitterBtnClicked);
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnCharacterDataUpdated += this.OnCharacterDataUpdated;
			ClientGameManager.Get().OnBankBalanceChange += this.HandleBankBalanceChange;
		}
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnCharacterDataUpdated -= this.OnCharacterDataUpdated;
			ClientGameManager.Get().OnBankBalanceChange -= this.HandleBankBalanceChange;
		}
	}

	public void OnCharacterDataUpdated(PersistedCharacterData newData)
	{
		if (newData.CharacterType == this.m_characterType)
		{
			this.Setup(this.m_characterType);
		}
	}

	public void Setup(CharacterType characterType)
	{
		if (characterType.IsValidForHumanGameplay())
		{
			this.Setup(GameWideData.Get().GetCharacterResourceLink(characterType));
		}
	}

	public void Setup(CharacterResourceLink charLink)
	{
		this.m_charLink = charLink;
		if (!(this.m_charLink == null))
		{
			if (!this.m_charLink.m_characterType.IsValidForHumanGameplay())
			{
			}
			else
			{
				this.Init();
				GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
				PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(charLink.m_characterType);
				ExperienceComponent experienceComponent;
				if (playerCharacterData != null)
				{
					experienceComponent = playerCharacterData.ExperienceComponent;
				}
				else
				{
					experienceComponent = null;
				}
				ExperienceComponent experienceComponent2 = experienceComponent;
				this.m_characterType = charLink.m_characterType;
				this.m_name.text = charLink.GetDisplayName();
				this.m_description.text = charLink.GetCharSelectAboutDescription();
				(this.m_description.transform as RectTransform).sizeDelta = new Vector2((this.m_description.transform as RectTransform).sizeDelta.x, this.m_description.GetPreferredValues().y);
				this.m_needResetScrollbar = true;
				if (experienceComponent2 != null)
				{
					this.m_levelLabel.text = string.Format(StringUtil.TR("LevelLabel", "Global"), experienceComponent2.Level);
					this.m_takedownsLabel.text = experienceComponent2.Kills.ToString();
					this.m_matchesWonLabel.text = experienceComponent2.Wins.ToString();
					this.m_gamesPlayedLabel.text = experienceComponent2.Matches.ToString();
					if (experienceComponent2.Level > 0 && experienceComponent2.Level < gameBalanceVars.MaxCharacterLevel)
					{
						int num = gameBalanceVars.CharacterExperienceToLevel(experienceComponent2.Level);
						this.m_experienceSlider.fillAmount = (float)playerCharacterData.ExperienceComponent.XPProgressThroughLevel / (float)num;
						UIManager.SetGameObjectActive(this.m_experienceSlider, true, null);
						this.m_expAmountText.text = playerCharacterData.ExperienceComponent.XPProgressThroughLevel + " / " + num;
					}
					else
					{
						UIManager.SetGameObjectActive(this.m_experienceSlider, false, null);
						this.m_expAmountText.text = StringUtil.TR("MAX", "Global");
					}
					this.m_expDataLevel = experienceComponent2.Level;
				}
				else
				{
					this.m_expDataLevel = 1;
				}
				if (charLink.m_characterRole == CharacterRole.Assassin)
				{
					this.m_roleIcon.sprite = this.m_assassinIcon;
				}
				else if (charLink.m_characterRole == CharacterRole.Support)
				{
					this.m_roleIcon.sprite = this.m_supportIcon;
				}
				else if (charLink.m_characterRole == CharacterRole.Tank)
				{
					this.m_roleIcon.sprite = this.m_tankIcon;
				}
				else
				{
					if (charLink.m_characterType.IsWillFill())
					{
						return;
					}
					throw new Exception("No icon for " + charLink.m_characterRole);
				}
				this.m_healthFill.Setup(charLink.m_statHealth);
				this.m_damageFill.Setup(charLink.m_statDamage);
				this.m_survivalFill.Setup(charLink.m_statSurvival);
				this.m_difficultyFill.Setup(charLink.m_statDifficulty);
				UIManager.SetGameObjectActive(this.m_twitterBtn, !charLink.m_twitterHandle.IsNullOrEmpty(), null);
				for (int i = 0; i < this.m_twitterHandleLabel.Length; i++)
				{
					this.m_twitterHandleLabel[i].text = "@" + charLink.m_twitterHandle;
				}
				this.m_rewards = RewardUtils.GetCharacterRewards(charLink, null);
				for (int j = 0; j < gameBalanceVars.RepeatingCharacterLevelRewards.Length; j++)
				{
					if (gameBalanceVars.RepeatingCharacterLevelRewards[j].charType == (int)charLink.m_characterType && gameBalanceVars.RepeatingCharacterLevelRewards[j].repeatingLevel > 0)
					{
						RewardUtils.RewardData rewardData = new RewardUtils.RewardData();
						rewardData.Amount = gameBalanceVars.RepeatingCharacterLevelRewards[j].reward.Amount;
						InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(gameBalanceVars.RepeatingCharacterLevelRewards[j].reward.ItemTemplateId);
						rewardData.Name = itemTemplate.GetDisplayName();
						rewardData.SpritePath = itemTemplate.IconPath;
						rewardData.Level = gameBalanceVars.RepeatingCharacterLevelRewards[j].startLevel;
						rewardData.InventoryTemplate = itemTemplate;
						rewardData.repeatLevels = gameBalanceVars.RepeatingCharacterLevelRewards[j].repeatingLevel;
						rewardData.isRepeating = true;
						this.m_rewards.Add(rewardData);
					}
				}
				UIManager.SetGameObjectActive(this.m_rewardContainer, false, null);
				int k = 0;
				while (k < this.m_rewards.Count)
				{
					if (this.m_rewards[k].Level <= this.m_expDataLevel)
					{
						if (!this.m_rewards[k].isRepeating)
						{
							k++;
							continue;
						}
					}
					UIManager.SetGameObjectActive(this.m_rewardContainer, true, null);
					this.m_nextRewardIcon.sprite = Resources.Load<Sprite>(this.m_rewards[k].SpritePath);
					UIManager.SetGameObjectActive(this.m_nextRewardFG, this.m_rewards[k].Foreground != null, null);
					this.m_nextRewardFG.sprite = this.m_rewards[k].Foreground;
					IL_5C9:
					this.m_bioText.text = charLink.GetCharBio();
					List<LoreArticle> articlesByCharacter = LoreWideData.Get().GetArticlesByCharacter(charLink.m_characterType);
					articlesByCharacter.Sort(delegate(LoreArticle a, LoreArticle b)
					{
						DateTime value = Convert.ToDateTime(a.DatePublished);
						return Convert.ToDateTime(a.DatePublished).CompareTo(value);
					});
					int l;
					for (l = 0; l < articlesByCharacter.Count; l++)
					{
						UIArticleSelectButton uiarticleSelectButton;
						if (l < this.m_articleButtons.Count)
						{
							uiarticleSelectButton = this.m_articleButtons[l];
						}
						else
						{
							uiarticleSelectButton = UnityEngine.Object.Instantiate<UIArticleSelectButton>(this.m_articleSelectButtonPrefab);
							uiarticleSelectButton.transform.SetParent(this.m_articlesGrid.transform);
							uiarticleSelectButton.transform.localPosition = Vector3.zero;
							uiarticleSelectButton.transform.localScale = Vector3.one;
							uiarticleSelectButton.m_hitbox.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
							this.m_articleButtons.Add(uiarticleSelectButton);
						}
						uiarticleSelectButton.Setup(articlesByCharacter[l]);
						UIManager.SetGameObjectActive(uiarticleSelectButton, true, null);
					}
					while (l < this.m_articleButtons.Count)
					{
						UIManager.SetGameObjectActive(this.m_articleButtons[l], false, null);
						l++;
					}
					LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_bottomScrollRect.transform as RectTransform);
					this.m_bottomScrollRect.verticalNormalizedPosition = 1f;
					this.m_descriptionScrollRect.verticalNormalizedPosition = 1f;
					this.m_rewardTooltipObj.Refresh();
					this.UpdateBuyButtons();
					return;
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					goto IL_5C9;
				}
			}
		}
	}

	public void TwitterBtnClicked(BaseEventData data)
	{
		Application.OpenURL("http://twitter.com/" + this.m_charLink.m_twitterHandle);
	}

	public void OnScroll(BaseEventData data)
	{
		this.m_bottomScrollRect.OnScroll((PointerEventData)data);
	}

	public CharacterResourceLink GetDisplayedCharacter()
	{
		return this.m_charLink;
	}

	private void HandleBankBalanceChange(CurrencyData data)
	{
		if (data.m_Type == CurrencyType.UnlockFreelancerToken)
		{
			this.UpdateBuyButtons();
		}
	}

	private void UpdateBuyButtons()
	{
		if (this.m_buyHeroContainer == null)
		{
			return;
		}
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (!(this.m_charLink == null))
		{
			if (!(clientGameManager == null))
			{
				if (clientGameManager.IsPlayerCharacterDataAvailable(this.m_charLink.m_characterType))
				{
					PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(this.m_charLink.m_characterType);
					if (!clientGameManager.HasPurchasedGame && !playerCharacterData.CharacterComponent.Unlocked && UICashShopPanel.Get().IsVisible())
					{
						int unlockFreelancerCurrencyPrice = this.m_charLink.m_charUnlockData.GetUnlockFreelancerCurrencyPrice();
						for (int i = 0; i < this.m_buyInGameLabels.Length; i++)
						{
							this.m_buyInGameLabels[i].text = "<sprite name=credit>" + unlockFreelancerCurrencyPrice;
						}
						UIManager.SetGameObjectActive(this.m_buyInGameButton, unlockFreelancerCurrencyPrice > 0, null);
						string accountCurrency = HydrogenConfig.Get().Ticket.AccountCurrency;
						float freelancerPrice = CommerceClient.Get().GetFreelancerPrice(this.m_charLink.m_characterType, accountCurrency);
						string localizedPriceString = UIStorePanel.GetLocalizedPriceString(freelancerPrice, accountCurrency);
						for (int j = 0; j < this.m_buyForCashLabels.Length; j++)
						{
							this.m_buyForCashLabels[j].text = localizedPriceString;
						}
						UIManager.SetGameObjectActive(this.m_buyForCashButton, freelancerPrice > 0f, null);
						bool flag = ClientGameManager.Get().PlayerWallet.GetValue(CurrencyType.UnlockFreelancerToken).m_Amount > 0;
						UIManager.SetGameObjectActive(this.m_buyForTokenButton, flag, null);
						Component buyHeroContainer = this.m_buyHeroContainer;
						bool doActive;
						if (unlockFreelancerCurrencyPrice <= 0)
						{
							if (freelancerPrice <= 0f)
							{
								doActive = flag;
								goto IL_1EC;
							}
						}
						doActive = true;
						IL_1EC:
						UIManager.SetGameObjectActive(buyHeroContainer, doActive, null);
					}
					else
					{
						UIManager.SetGameObjectActive(this.m_buyHeroContainer, false, null);
					}
					return;
				}
			}
		}
	}

	private void BuyInGameClicked(BaseEventData data)
	{
		UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
		uipurchaseableItem.m_itemType = PurchaseItemType.Character;
		uipurchaseableItem.m_charLink = this.m_charLink;
		uipurchaseableItem.m_currencyType = CurrencyType.FreelancerCurrency;
		UIStorePanel.Get().OpenPurchaseDialog(uipurchaseableItem, new UIStorePanel.PurchaseCharacterCallback(this.PurchaseCharacterResponseHandler));
	}

	private void BuyForCashClicked(BaseEventData data)
	{
		UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
		uipurchaseableItem.m_itemType = PurchaseItemType.Character;
		uipurchaseableItem.m_charLink = this.m_charLink;
		uipurchaseableItem.m_purchaseForCash = true;
		UIStorePanel.Get().OpenPurchaseDialog(uipurchaseableItem, new UIStorePanel.PurchaseCharacterCallback(this.PurchaseCharacterResponseHandler));
	}

	private void BuyForTokenClicked(BaseEventData data)
	{
		ClientGameManager.Get().PurchaseCharacter(CurrencyType.UnlockFreelancerToken, this.m_charLink.m_characterType, delegate(PurchaseCharacterResponse response)
		{
			this.PurchaseCharacterResponseHandler(response.Success, response.Result, response.CharacterType);
		});
	}

	private void PurchaseCharacterResponseHandler(bool success, PurchaseResult result, CharacterType characterType)
	{
		if (success && result == PurchaseResult.Success)
		{
			if (characterType == this.m_charLink.m_characterType)
			{
				UIManager.SetGameObjectActive(this.m_buyHeroContainer, false, null);
			}
		}
	}
}
