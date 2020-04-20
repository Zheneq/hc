﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILockboxRewardItem : MonoBehaviour
{
	public TextMeshProUGUI m_rewardName;

	public TextMeshProUGUI m_rewardType;

	public TextMeshProUGUI m_rewardAmount;

	public Image m_rewardIcon;

	public Image m_rewardFg;

	public Image m_rewardBanner;

	public Image m_bannerShadow;

	public Image m_ownedIcon;

	public Image[] m_rewardIcons;

	public Image[] m_rewardFgs;

	public Image[] m_rewardBanners;

	public Image[] m_commonIndicators;

	public Image[] m_uncommonIndicators;

	public Image[] m_rareIndicators;

	public Image[] m_epicIndicators;

	public Image[] m_legendaryIndicators;

	public TextMeshProUGUI m_isoCountText;

	public Animator m_duplicateAnimator;

	public Image m_circleHitbox;

	private bool m_isDuplicate;

	private InventoryItemTemplate m_template;

	private bool m_animationFinishedOnce;

	private void Awake()
	{
		if (this.m_rewardName != null)
		{
			this.m_rewardName.raycastTarget = false;
		}
		if (this.m_rewardType != null)
		{
			this.m_rewardType.raycastTarget = false;
		}
		if (this.m_rewardAmount != null)
		{
			this.m_rewardAmount.raycastTarget = false;
		}
		if (this.m_circleHitbox != null)
		{
			UIEventTriggerUtils.AddListener(this.m_circleHitbox.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnClick));
			UIEventTriggerUtils.AddListener(this.m_circleHitbox.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.OnHoverEnter));
			UIEventTriggerUtils.AddListener(this.m_circleHitbox.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.OnHoverExit));
		}
	}

	public InventoryItemTemplate GetTemplate()
	{
		return this.m_template;
	}

	public void Setup(InventoryItem item, InventoryItemTemplate template, bool isDuplicate, int isoCount)
	{
		this.m_animationFinishedOnce = false;
		if (this.m_rewardName != null)
		{
			this.m_rewardName.text = template.GetDisplayName();
			if (template.Type == InventoryItemType.Currency)
			{
				CurrencyType currencyType = (CurrencyType)template.TypeSpecificData[0];
				if (currencyType == CurrencyType.Dust)
				{
					currencyType = CurrencyType.ISO;
				}
				int num = 0;
				int num2 = 0;
				ActiveAlertMission currentAlert = ClientGameManager.Get().AlertMissionsData.CurrentAlert;
				if (currentAlert != null)
				{
					if (currentAlert.Type == AlertMissionType.Bonus)
					{
						if (currencyType == currentAlert.BonusType)
						{
							float num3 = (float)currentAlert.BonusMultiplier / 100f;
							num = (int)((float)template.TypeSpecificData[1] * num3);
						}
					}
				}
				GameManager gameManager = GameManager.Get();
				if (gameManager != null)
				{
					if (this.GetIsEligibleForEventBonus(gameManager.GameplayOverrides))
					{
						if (gameManager.GameplayOverrides.EventISOBonusPercent > 0)
						{
							if (currencyType == CurrencyType.ISO)
							{
								num2 = (int)((double)(template.TypeSpecificData[1] * gameManager.GameplayOverrides.EventISOBonusPercent) / 100.0);
							}
						}
					}
				}
				int num4 = num + num2;
				if (num4 > 0)
				{
					TextMeshProUGUI rewardName = this.m_rewardName;
					string text = rewardName.text;
					rewardName.text = string.Concat(new object[]
					{
						text,
						" (+",
						num4,
						")"
					});
				}
			}
		}
		if (this.m_rewardAmount != null)
		{
			this.m_rewardAmount.text = string.Format(StringUtil.TR("AmountToCraft", "Inventory"), item.Count);
			UIManager.SetGameObjectActive(this.m_rewardAmount, item.Count > 1, null);
		}
		if (this.m_rewardType != null)
		{
			this.m_rewardType.text = InventoryWideData.GetTypeString(template, item.Count);
		}
		if (this.m_rewardIcon != null)
		{
			if (this.m_rewardBanner != null)
			{
				if (template.Type == InventoryItemType.BannerID && GameBalanceVars.Get().GetBanner(template.TypeSpecificData[0]).m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
				{
					this.m_rewardBanner.sprite = (Sprite)Resources.Load(InventoryWideData.GetSpritePath(template), typeof(Sprite));
					UIManager.SetGameObjectActive(this.m_rewardIcon, false, null);
					UIManager.SetGameObjectActive(this.m_rewardBanner, true, null);
					if (this.m_bannerShadow != null)
					{
						UIManager.SetGameObjectActive(this.m_bannerShadow, true, null);
					}
				}
				else
				{
					this.m_rewardIcon.sprite = (Sprite)Resources.Load(InventoryWideData.GetSpritePath(template), typeof(Sprite));
					UIManager.SetGameObjectActive(this.m_rewardIcon, true, null);
					UIManager.SetGameObjectActive(this.m_rewardBanner, false, null);
					if (this.m_bannerShadow != null)
					{
						UIManager.SetGameObjectActive(this.m_bannerShadow, false, null);
					}
				}
			}
		}
		if (this.m_rewardIcons != null)
		{
			if (this.m_rewardBanners != null)
			{
				Sprite sprite = (Sprite)Resources.Load(InventoryWideData.GetSpritePath(template), typeof(Sprite));
				bool flag;
				if (template.Type == InventoryItemType.BannerID)
				{
					flag = (GameBalanceVars.Get().GetBanner(template.TypeSpecificData[0]).m_type == GameBalanceVars.PlayerBanner.BannerType.Background);
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				for (int i = 0; i < this.m_rewardIcons.Length; i++)
				{
					this.m_rewardIcons[i].sprite = sprite;
					UIManager.SetGameObjectActive(this.m_rewardIcons[i], !flag2, null);
				}
				for (int j = 0; j < this.m_rewardBanners.Length; j++)
				{
					this.m_rewardBanners[j].sprite = sprite;
					UIManager.SetGameObjectActive(this.m_rewardBanners[j], flag2, null);
				}
			}
		}
		if (this.m_rewardFgs != null)
		{
			for (int k = 0; k < this.m_rewardFgs.Length; k++)
			{
				this.m_rewardFgs[k].sprite = InventoryWideData.GetItemFg(template);
				UIManager.SetGameObjectActive(this.m_rewardFgs[k], this.m_rewardFgs[k].sprite != null, null);
			}
		}
		if (this.m_rewardFg != null)
		{
			this.m_rewardFg.sprite = InventoryWideData.GetItemFg(template);
			UIManager.SetGameObjectActive(this.m_rewardFg, this.m_rewardFg.sprite != null, null);
		}
		if (this.m_ownedIcon != null)
		{
			UIManager.SetGameObjectActive(this.m_ownedIcon, isDuplicate, null);
		}
		if (this.m_isoCountText != null)
		{
			if (isoCount > template.Value)
			{
				int num5 = isoCount - template.Value;
				this.m_isoCountText.text = string.Concat(new object[]
				{
					"+",
					template.Value,
					" (+",
					num5,
					")"
				});
			}
			else
			{
				this.m_isoCountText.text = "+" + isoCount;
			}
		}
		this.m_isDuplicate = isDuplicate;
		this.PlayDuplicateAnimation();
		if (this.m_rewardType != null)
		{
			UIManager.SetGameObjectActive(this.m_rewardType, template.Type != InventoryItemType.Currency, null);
		}
		this.m_template = template;
		this.SetIndicatorVisibility(template.Rarity == InventoryItemRarity.Common, this.m_commonIndicators);
		this.SetIndicatorVisibility(template.Rarity == InventoryItemRarity.Uncommon, this.m_uncommonIndicators);
		this.SetIndicatorVisibility(template.Rarity == InventoryItemRarity.Rare, this.m_rareIndicators);
		this.SetIndicatorVisibility(template.Rarity == InventoryItemRarity.Epic, this.m_epicIndicators);
		this.SetIndicatorVisibility(template.Rarity == InventoryItemRarity.Legendary, this.m_legendaryIndicators);
	}

	private void SetIndicatorVisibility(bool isVisible, Image[] indicators)
	{
		for (int i = 0; i < indicators.Length; i++)
		{
			UIManager.SetGameObjectActive(indicators[i], isVisible, null);
		}
	}

	public void SetSize(float width)
	{
		LayoutElement component = base.GetComponent<LayoutElement>();
		LayoutElement layoutElement = component;
		component.preferredWidth = width;
		layoutElement.preferredHeight = width;
	}

	public void PlayDuplicateAnimation()
	{
		if (this.m_duplicateAnimator != null)
		{
			if (this.m_duplicateAnimator.isInitialized)
			{
				string str = "LockboxRewardDuplicate";
				string str2;
				if (this.m_isDuplicate)
				{
					str2 = "DefaultIN";
				}
				else
				{
					str2 = "Invalid";
				}
				string stateName = str + str2;
				this.m_duplicateAnimator.Play(stateName, -1, 0f);
			}
			else
			{
				UILootMatrixScreen.Get().QueueDuplicateAnimation(this);
			}
		}
	}

	private void OnEnable()
	{
		if (this.m_duplicateAnimator != null)
		{
			this.m_duplicateAnimator.Update(Time.deltaTime);
		}
	}

	private void OnClick(BaseEventData data)
	{
		if (this.m_template != null)
		{
			if (!(UIStorePanel.Get() == null))
			{
				if (this.m_template.Type != InventoryItemType.Currency)
				{
					if (this.m_template.Type != InventoryItemType.Experience)
					{
						if (this.m_template.Type != InventoryItemType.Faction)
						{
							if (this.m_template.Type != InventoryItemType.Unlock && this.m_template.Type != InventoryItemType.Conveyance)
							{
								if (this.m_template.Type != InventoryItemType.FreelancerExpBonus)
								{
									UIFrontEnd.Get().m_frontEndNavPanel.CollectionsBtnClicked(null);
									UIStorePanel.Get().SelectItem(this.m_template);
									return;
								}
							}
						}
					}
				}
				return;
			}
		}
	}

	private void OnHoverEnter(BaseEventData data)
	{
		if (this.m_duplicateAnimator != null)
		{
			if (this.m_isDuplicate)
			{
				if (!this.m_animationFinishedOnce)
				{
					if (this.m_duplicateAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
					{
						if (!this.m_duplicateAnimator.IsInTransition(0))
						{
							this.m_animationFinishedOnce = true;
							goto IL_83;
						}
					}
					return;
				}
				IL_83:
				this.m_duplicateAnimator.Play("LockboxRewardDuplicateInvalid", -1, 0f);
			}
		}
	}

	private void OnHoverExit(BaseEventData data)
	{
		if (this.m_duplicateAnimator != null)
		{
			if (this.m_isDuplicate)
			{
				if (this.m_animationFinishedOnce)
				{
					this.m_duplicateAnimator.Play("LockboxRewardDuplicateDefaultIDLE", -1, 0f);
				}
			}
		}
	}

	public bool GetIsEligibleForEventBonus(LobbyGameplayOverrides gameplayOverrides)
	{
		if (gameplayOverrides != null && gameplayOverrides.EnableEventBonus)
		{
			if (!(gameplayOverrides.EventBonusStartDate > ClientGameManager.Get().UtcNow()))
			{
				if (!(gameplayOverrides.EventBonusEndDate < ClientGameManager.Get().UtcNow()))
				{
					if (!gameplayOverrides.RequiredEventBonusEntitlement.IsNullOrEmpty())
					{
						if (gameplayOverrides.RequiredEventBonusEntitlement != "*")
						{
							if (ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetAppliedEntitlementCount(gameplayOverrides.RequiredEventBonusEntitlement) == 0)
							{
								return false;
							}
						}
					}
					return true;
				}
			}
		}
		return false;
	}
}
