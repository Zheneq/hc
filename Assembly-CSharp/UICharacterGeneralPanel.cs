using I2.Loc;
using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
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
		UIManager.SetGameObjectActive(this, visible);
	}

	private void Update()
	{
		if (!m_needResetScrollbar)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!m_descriptionScrollRect.verticalScrollbar.gameObject.activeSelf)
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
				if (m_descriptionScrollRect.verticalScrollbar.value != 1f)
				{
					m_needResetScrollbar = false;
					m_descriptionScrollRect.verticalScrollbar.value = 1f;
				}
				return;
			}
		}
	}

	private void Init()
	{
		if (m_articleButtons != null)
		{
			return;
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
			m_articleButtons = new List<UIArticleSelectButton>(m_articlesGrid.GetComponentsInChildren<UIArticleSelectButton>(true));
			using (List<UIArticleSelectButton>.Enumerator enumerator = m_articleButtons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIArticleSelectButton current = enumerator.Current;
					current.m_hitbox.RegisterScrollListener(OnScroll);
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

	private void Awake()
	{
		Init();
		m_rewardTooltipObj.Setup(TooltipType.RewardList, PopulateTooltip);
		UIManager.SetGameObjectActive(m_articlesContainer, LocalizationManager.CurrentLanguageCode != "zh");
		if (!(m_buyHeroContainer != null))
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
			m_buyInGameButton.spriteController.callback = BuyInGameClicked;
			m_buyForCashButton.spriteController.callback = BuyForCashClicked;
			m_buyForTokenButton.spriteController.callback = BuyForTokenClicked;
			return;
		}
	}

	private bool PopulateTooltip(UITooltipBase tooltip)
	{
		if (m_rewards != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_rewards.Count != 0)
			{
				UIRewardListTooltip uIRewardListTooltip = tooltip as UIRewardListTooltip;
				uIRewardListTooltip.Setup(m_rewards, m_expDataLevel, UIRewardListTooltip.RewardsType.Character);
				return true;
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
		return false;
	}

	private void Start()
	{
		if (m_descriptionScrollRect != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			_MouseEventPasser mouseEventPasser = m_description.gameObject.AddComponent<_MouseEventPasser>();
			mouseEventPasser.AddNewHandler(m_descriptionScrollRect);
			mouseEventPasser = m_descriptionScrollRect.verticalScrollbar.handleRect.gameObject.AddComponent<_MouseEventPasser>();
			mouseEventPasser.AddNewHandler(m_descriptionScrollRect);
			m_descriptionScrollRect.scrollSensitivity = 75f;
		}
		m_twitterBtn.spriteController.callback = TwitterBtnClicked;
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnCharacterDataUpdated += OnCharacterDataUpdated;
			ClientGameManager.Get().OnBankBalanceChange += HandleBankBalanceChange;
		}
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnCharacterDataUpdated -= OnCharacterDataUpdated;
			ClientGameManager.Get().OnBankBalanceChange -= HandleBankBalanceChange;
		}
	}

	public void OnCharacterDataUpdated(PersistedCharacterData newData)
	{
		if (newData.CharacterType != m_characterType)
		{
			return;
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
			Setup(m_characterType);
			return;
		}
	}

	public void Setup(CharacterType characterType)
	{
		if (!characterType.IsValidForHumanGameplay())
		{
			return;
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
			Setup(GameWideData.Get().GetCharacterResourceLink(characterType));
			return;
		}
	}

	public void Setup(CharacterResourceLink charLink)
	{
		m_charLink = charLink;
		if (m_charLink == null)
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
			if (!m_charLink.m_characterType.IsValidForHumanGameplay())
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
			Init();
			GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(charLink.m_characterType);
			object obj;
			if (playerCharacterData != null)
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
				obj = playerCharacterData.ExperienceComponent;
			}
			else
			{
				obj = null;
			}
			ExperienceComponent experienceComponent = (ExperienceComponent)obj;
			m_characterType = charLink.m_characterType;
			m_name.text = charLink.GetDisplayName();
			m_description.text = charLink.GetCharSelectAboutDescription();
			RectTransform obj2 = m_description.transform as RectTransform;
			Vector2 sizeDelta = (m_description.transform as RectTransform).sizeDelta;
			float x = sizeDelta.x;
			Vector2 preferredValues = m_description.GetPreferredValues();
			obj2.sizeDelta = new Vector2(x, preferredValues.y);
			m_needResetScrollbar = true;
			if (experienceComponent != null)
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
				m_levelLabel.text = string.Format(StringUtil.TR("LevelLabel", "Global"), experienceComponent.Level);
				m_takedownsLabel.text = experienceComponent.Kills.ToString();
				m_matchesWonLabel.text = experienceComponent.Wins.ToString();
				m_gamesPlayedLabel.text = experienceComponent.Matches.ToString();
				if (experienceComponent.Level > 0 && experienceComponent.Level < gameBalanceVars.MaxCharacterLevel)
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
					int num = gameBalanceVars.CharacterExperienceToLevel(experienceComponent.Level);
					m_experienceSlider.fillAmount = (float)playerCharacterData.ExperienceComponent.XPProgressThroughLevel / (float)num;
					UIManager.SetGameObjectActive(m_experienceSlider, true);
					m_expAmountText.text = playerCharacterData.ExperienceComponent.XPProgressThroughLevel + " / " + num;
				}
				else
				{
					UIManager.SetGameObjectActive(m_experienceSlider, false);
					m_expAmountText.text = StringUtil.TR("MAX", "Global");
				}
				m_expDataLevel = experienceComponent.Level;
			}
			else
			{
				m_expDataLevel = 1;
			}
			if (charLink.m_characterRole == CharacterRole.Assassin)
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
				m_roleIcon.sprite = m_assassinIcon;
			}
			else if (charLink.m_characterRole == CharacterRole.Support)
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
				m_roleIcon.sprite = m_supportIcon;
			}
			else
			{
				if (charLink.m_characterRole != CharacterRole.Tank)
				{
					if (charLink.m_characterType.IsWillFill())
					{
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
					throw new Exception("No icon for " + charLink.m_characterRole);
				}
				m_roleIcon.sprite = m_tankIcon;
			}
			m_healthFill.Setup(charLink.m_statHealth);
			m_damageFill.Setup(charLink.m_statDamage);
			m_survivalFill.Setup(charLink.m_statSurvival);
			m_difficultyFill.Setup(charLink.m_statDifficulty);
			UIManager.SetGameObjectActive(m_twitterBtn, !charLink.m_twitterHandle.IsNullOrEmpty());
			for (int i = 0; i < m_twitterHandleLabel.Length; i++)
			{
				m_twitterHandleLabel[i].text = "@" + charLink.m_twitterHandle;
			}
			m_rewards = RewardUtils.GetCharacterRewards(charLink);
			for (int j = 0; j < gameBalanceVars.RepeatingCharacterLevelRewards.Length; j++)
			{
				if (gameBalanceVars.RepeatingCharacterLevelRewards[j].charType == (int)charLink.m_characterType && gameBalanceVars.RepeatingCharacterLevelRewards[j].repeatingLevel > 0)
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
					RewardUtils.RewardData rewardData = new RewardUtils.RewardData();
					rewardData.Amount = gameBalanceVars.RepeatingCharacterLevelRewards[j].reward.Amount;
					InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(gameBalanceVars.RepeatingCharacterLevelRewards[j].reward.ItemTemplateId);
					rewardData.Name = itemTemplate.GetDisplayName();
					rewardData.SpritePath = itemTemplate.IconPath;
					rewardData.Level = gameBalanceVars.RepeatingCharacterLevelRewards[j].startLevel;
					rewardData.InventoryTemplate = itemTemplate;
					rewardData.repeatLevels = gameBalanceVars.RepeatingCharacterLevelRewards[j].repeatingLevel;
					rewardData.isRepeating = true;
					m_rewards.Add(rewardData);
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				UIManager.SetGameObjectActive(m_rewardContainer, false);
				int num2 = 0;
				while (true)
				{
					if (num2 < m_rewards.Count)
					{
						if (m_rewards[num2].Level <= m_expDataLevel)
						{
							if (!m_rewards[num2].isRepeating)
							{
								num2++;
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
						UIManager.SetGameObjectActive(m_rewardContainer, true);
						m_nextRewardIcon.sprite = Resources.Load<Sprite>(m_rewards[num2].SpritePath);
						UIManager.SetGameObjectActive(m_nextRewardFG, m_rewards[num2].Foreground != null);
						m_nextRewardFG.sprite = m_rewards[num2].Foreground;
					}
					else
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
					}
					break;
				}
				m_bioText.text = charLink.GetCharBio();
				List<LoreArticle> articlesByCharacter = LoreWideData.Get().GetArticlesByCharacter(charLink.m_characterType);
				articlesByCharacter.Sort(delegate(LoreArticle a, LoreArticle b)
				{
					DateTime value = Convert.ToDateTime(a.DatePublished);
					return Convert.ToDateTime(a.DatePublished).CompareTo(value);
				});
				int k;
				for (k = 0; k < articlesByCharacter.Count; k++)
				{
					UIArticleSelectButton uIArticleSelectButton;
					if (k < m_articleButtons.Count)
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
						uIArticleSelectButton = m_articleButtons[k];
					}
					else
					{
						uIArticleSelectButton = UnityEngine.Object.Instantiate(m_articleSelectButtonPrefab);
						uIArticleSelectButton.transform.SetParent(m_articlesGrid.transform);
						uIArticleSelectButton.transform.localPosition = Vector3.zero;
						uIArticleSelectButton.transform.localScale = Vector3.one;
						uIArticleSelectButton.m_hitbox.RegisterScrollListener(OnScroll);
						m_articleButtons.Add(uIArticleSelectButton);
					}
					uIArticleSelectButton.Setup(articlesByCharacter[k]);
					UIManager.SetGameObjectActive(uIArticleSelectButton, true);
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
				for (; k < m_articleButtons.Count; k++)
				{
					UIManager.SetGameObjectActive(m_articleButtons[k], false);
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					LayoutRebuilder.ForceRebuildLayoutImmediate(m_bottomScrollRect.transform as RectTransform);
					m_bottomScrollRect.verticalNormalizedPosition = 1f;
					m_descriptionScrollRect.verticalNormalizedPosition = 1f;
					m_rewardTooltipObj.Refresh();
					UpdateBuyButtons();
					return;
				}
			}
		}
	}

	public void TwitterBtnClicked(BaseEventData data)
	{
		Application.OpenURL("http://twitter.com/" + m_charLink.m_twitterHandle);
	}

	public void OnScroll(BaseEventData data)
	{
		m_bottomScrollRect.OnScroll((PointerEventData)data);
	}

	public CharacterResourceLink GetDisplayedCharacter()
	{
		return m_charLink;
	}

	private void HandleBankBalanceChange(CurrencyData data)
	{
		if (data.m_Type != CurrencyType.UnlockFreelancerToken)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UpdateBuyButtons();
			return;
		}
	}

	private void UpdateBuyButtons()
	{
		if (m_buyHeroContainer == null)
		{
			return;
		}
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (m_charLink == null)
		{
			return;
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
			if (clientGameManager == null)
			{
				return;
			}
			if (!clientGameManager.IsPlayerCharacterDataAvailable(m_charLink.m_characterType))
			{
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
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(m_charLink.m_characterType);
			if (!clientGameManager.HasPurchasedGame && !playerCharacterData.CharacterComponent.Unlocked && UICashShopPanel.Get().IsVisible())
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
					{
						int unlockFreelancerCurrencyPrice = m_charLink.m_charUnlockData.GetUnlockFreelancerCurrencyPrice();
						for (int i = 0; i < m_buyInGameLabels.Length; i++)
						{
							m_buyInGameLabels[i].text = "<sprite name=credit>" + unlockFreelancerCurrencyPrice;
						}
						UIManager.SetGameObjectActive(m_buyInGameButton, unlockFreelancerCurrencyPrice > 0);
						string accountCurrency = HydrogenConfig.Get().Ticket.AccountCurrency;
						float freelancerPrice = CommerceClient.Get().GetFreelancerPrice(m_charLink.m_characterType, accountCurrency);
						string localizedPriceString = UIStorePanel.GetLocalizedPriceString(freelancerPrice, accountCurrency);
						for (int j = 0; j < m_buyForCashLabels.Length; j++)
						{
							m_buyForCashLabels[j].text = localizedPriceString;
						}
						while (true)
						{
							RectTransform buyHeroContainer;
							int doActive;
							switch (5)
							{
							case 0:
								break;
							default:
								{
									UIManager.SetGameObjectActive(m_buyForCashButton, freelancerPrice > 0f);
									bool flag = ClientGameManager.Get().PlayerWallet.GetValue(CurrencyType.UnlockFreelancerToken).m_Amount > 0;
									UIManager.SetGameObjectActive(m_buyForTokenButton, flag);
									buyHeroContainer = m_buyHeroContainer;
									if (unlockFreelancerCurrencyPrice <= 0)
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
										if (!(freelancerPrice > 0f))
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
											doActive = (flag ? 1 : 0);
											goto IL_01ec;
										}
									}
									doActive = 1;
									goto IL_01ec;
								}
								IL_01ec:
								UIManager.SetGameObjectActive(buyHeroContainer, (byte)doActive != 0);
								return;
							}
						}
					}
					}
				}
			}
			UIManager.SetGameObjectActive(m_buyHeroContainer, false);
			return;
		}
	}

	private void BuyInGameClicked(BaseEventData data)
	{
		UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
		uIPurchaseableItem.m_itemType = PurchaseItemType.Character;
		uIPurchaseableItem.m_charLink = m_charLink;
		uIPurchaseableItem.m_currencyType = CurrencyType.FreelancerCurrency;
		UIStorePanel.Get().OpenPurchaseDialog(uIPurchaseableItem, PurchaseCharacterResponseHandler);
	}

	private void BuyForCashClicked(BaseEventData data)
	{
		UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
		uIPurchaseableItem.m_itemType = PurchaseItemType.Character;
		uIPurchaseableItem.m_charLink = m_charLink;
		uIPurchaseableItem.m_purchaseForCash = true;
		UIStorePanel.Get().OpenPurchaseDialog(uIPurchaseableItem, PurchaseCharacterResponseHandler);
	}

	private void BuyForTokenClicked(BaseEventData data)
	{
		ClientGameManager.Get().PurchaseCharacter(CurrencyType.UnlockFreelancerToken, m_charLink.m_characterType, delegate(PurchaseCharacterResponse response)
		{
			PurchaseCharacterResponseHandler(response.Success, response.Result, response.CharacterType);
		});
	}

	private void PurchaseCharacterResponseHandler(bool success, PurchaseResult result, CharacterType characterType)
	{
		if (!success || result != PurchaseResult.Success)
		{
			return;
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
			if (characterType == m_charLink.m_characterType)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					UIManager.SetGameObjectActive(m_buyHeroContainer, false);
					return;
				}
			}
			return;
		}
	}
}
