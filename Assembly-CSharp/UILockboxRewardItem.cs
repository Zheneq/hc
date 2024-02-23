using System.Text;
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
		if (m_rewardName != null)
		{
			m_rewardName.raycastTarget = false;
		}
		if (m_rewardType != null)
		{
			m_rewardType.raycastTarget = false;
		}
		if (m_rewardAmount != null)
		{
			m_rewardAmount.raycastTarget = false;
		}
		if (m_circleHitbox != null)
		{
			UIEventTriggerUtils.AddListener(m_circleHitbox.gameObject, EventTriggerType.PointerClick, OnClick);
			UIEventTriggerUtils.AddListener(m_circleHitbox.gameObject, EventTriggerType.PointerEnter, OnHoverEnter);
			UIEventTriggerUtils.AddListener(m_circleHitbox.gameObject, EventTriggerType.PointerExit, OnHoverExit);
		}
	}

	public InventoryItemTemplate GetTemplate()
	{
		return m_template;
	}

	public void Setup(InventoryItem item, InventoryItemTemplate template, bool isDuplicate, int isoCount)
	{
		m_animationFinishedOnce = false;
		if (m_rewardName != null)
		{
			m_rewardName.text = template.GetDisplayName();
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
					if (GetIsEligibleForEventBonus(gameManager.GameplayOverrides))
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
					TextMeshProUGUI rewardName = m_rewardName;
					string text = rewardName.text;
					rewardName.text = new StringBuilder().Append(text).Append(" (+").Append(num4).Append(")").ToString();
				}
			}
		}
		if (m_rewardAmount != null)
		{
			m_rewardAmount.text = string.Format(StringUtil.TR("AmountToCraft", "Inventory"), item.Count);
			UIManager.SetGameObjectActive(m_rewardAmount, item.Count > 1);
		}
		if (m_rewardType != null)
		{
			m_rewardType.text = InventoryWideData.GetTypeString(template, item.Count);
		}
		if (m_rewardIcon != null)
		{
			if (m_rewardBanner != null)
			{
				if (template.Type == InventoryItemType.BannerID && GameBalanceVars.Get().GetBanner(template.TypeSpecificData[0]).m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
				{
					m_rewardBanner.sprite = (Sprite)Resources.Load(InventoryWideData.GetSpritePath(template), typeof(Sprite));
					UIManager.SetGameObjectActive(m_rewardIcon, false);
					UIManager.SetGameObjectActive(m_rewardBanner, true);
					if (m_bannerShadow != null)
					{
						UIManager.SetGameObjectActive(m_bannerShadow, true);
					}
				}
				else
				{
					m_rewardIcon.sprite = (Sprite)Resources.Load(InventoryWideData.GetSpritePath(template), typeof(Sprite));
					UIManager.SetGameObjectActive(m_rewardIcon, true);
					UIManager.SetGameObjectActive(m_rewardBanner, false);
					if (m_bannerShadow != null)
					{
						UIManager.SetGameObjectActive(m_bannerShadow, false);
					}
				}
			}
		}
		if (m_rewardIcons != null)
		{
			if (m_rewardBanners != null)
			{
				Sprite sprite = (Sprite)Resources.Load(InventoryWideData.GetSpritePath(template), typeof(Sprite));
				int num5;
				if (template.Type == InventoryItemType.BannerID)
				{
					num5 = ((GameBalanceVars.Get().GetBanner(template.TypeSpecificData[0]).m_type == GameBalanceVars.PlayerBanner.BannerType.Background) ? 1 : 0);
				}
				else
				{
					num5 = 0;
				}
				bool flag = (byte)num5 != 0;
				for (int i = 0; i < m_rewardIcons.Length; i++)
				{
					m_rewardIcons[i].sprite = sprite;
					UIManager.SetGameObjectActive(m_rewardIcons[i], !flag);
				}
				for (int j = 0; j < m_rewardBanners.Length; j++)
				{
					m_rewardBanners[j].sprite = sprite;
					UIManager.SetGameObjectActive(m_rewardBanners[j], flag);
				}
			}
		}
		if (m_rewardFgs != null)
		{
			for (int k = 0; k < m_rewardFgs.Length; k++)
			{
				m_rewardFgs[k].sprite = InventoryWideData.GetItemFg(template);
				UIManager.SetGameObjectActive(m_rewardFgs[k], m_rewardFgs[k].sprite != null);
			}
		}
		if (m_rewardFg != null)
		{
			m_rewardFg.sprite = InventoryWideData.GetItemFg(template);
			UIManager.SetGameObjectActive(m_rewardFg, m_rewardFg.sprite != null);
		}
		if (m_ownedIcon != null)
		{
			UIManager.SetGameObjectActive(m_ownedIcon, isDuplicate);
		}
		if (m_isoCountText != null)
		{
			if (isoCount > template.Value)
			{
				int num6 = isoCount - template.Value;
				m_isoCountText.text = new StringBuilder().Append("+").Append(template.Value).Append(" (+").Append(num6).Append(")").ToString();
			}
			else
			{
				m_isoCountText.text = new StringBuilder().Append("+").Append(isoCount).ToString();
			}
		}
		m_isDuplicate = isDuplicate;
		PlayDuplicateAnimation();
		if (m_rewardType != null)
		{
			UIManager.SetGameObjectActive(m_rewardType, template.Type != InventoryItemType.Currency);
		}
		m_template = template;
		SetIndicatorVisibility(template.Rarity == InventoryItemRarity.Common, m_commonIndicators);
		SetIndicatorVisibility(template.Rarity == InventoryItemRarity.Uncommon, m_uncommonIndicators);
		SetIndicatorVisibility(template.Rarity == InventoryItemRarity.Rare, m_rareIndicators);
		SetIndicatorVisibility(template.Rarity == InventoryItemRarity.Epic, m_epicIndicators);
		SetIndicatorVisibility(template.Rarity == InventoryItemRarity.Legendary, m_legendaryIndicators);
	}

	private void SetIndicatorVisibility(bool isVisible, Image[] indicators)
	{
		for (int i = 0; i < indicators.Length; i++)
		{
			UIManager.SetGameObjectActive(indicators[i], isVisible);
		}
		while (true)
		{
			return;
		}
	}

	public void SetSize(float width)
	{
		LayoutElement component = GetComponent<LayoutElement>();
		float num3 = component.preferredHeight = (component.preferredWidth = width);
	}

	public void PlayDuplicateAnimation()
	{
		if (!(m_duplicateAnimator != null))
		{
			return;
		}
		while (true)
		{
			if (m_duplicateAnimator.isInitialized)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
					{
						object str;
						if (m_isDuplicate)
						{
							str = "DefaultIN";
						}
						else
						{
							str = "Invalid";
						}
						string stateName = new StringBuilder().Append("LockboxRewardDuplicate").Append((string)str).ToString();
						m_duplicateAnimator.Play(stateName, -1, 0f);
						return;
					}
					}
				}
			}
			UILootMatrixScreen.Get().QueueDuplicateAnimation(this);
			return;
		}
	}

	private void OnEnable()
	{
		if (!(m_duplicateAnimator != null))
		{
			return;
		}
		while (true)
		{
			m_duplicateAnimator.Update(Time.deltaTime);
			return;
		}
	}

	private void OnClick(BaseEventData data)
	{
		if (m_template == null)
		{
			return;
		}
		while (true)
		{
			if (UIStorePanel.Get() == null)
			{
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
			if (m_template.Type == InventoryItemType.Currency)
			{
				return;
			}
			while (true)
			{
				if (m_template.Type == InventoryItemType.Experience)
				{
					return;
				}
				while (true)
				{
					if (m_template.Type == InventoryItemType.Faction)
					{
						return;
					}
					while (true)
					{
						if (m_template.Type == InventoryItemType.Unlock || m_template.Type == InventoryItemType.Conveyance)
						{
							return;
						}
						while (true)
						{
							if (m_template.Type == InventoryItemType.FreelancerExpBonus)
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
							UIFrontEnd.Get().m_frontEndNavPanel.CollectionsBtnClicked(null);
							UIStorePanel.Get().SelectItem(m_template);
							return;
						}
					}
				}
			}
		}
	}

	private void OnHoverEnter(BaseEventData data)
	{
		if (!(m_duplicateAnimator != null))
		{
			return;
		}
		while (true)
		{
			if (!m_isDuplicate)
			{
				return;
			}
			if (!m_animationFinishedOnce)
			{
				if (!(m_duplicateAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f))
				{
					return;
				}
				if (m_duplicateAnimator.IsInTransition(0))
				{
					return;
				}
				m_animationFinishedOnce = true;
			}
			m_duplicateAnimator.Play("LockboxRewardDuplicateInvalid", -1, 0f);
			return;
		}
	}

	private void OnHoverExit(BaseEventData data)
	{
		if (!(m_duplicateAnimator != null))
		{
			return;
		}
		while (true)
		{
			if (!m_isDuplicate)
			{
				return;
			}
			while (true)
			{
				if (m_animationFinishedOnce)
				{
					while (true)
					{
						m_duplicateAnimator.Play("LockboxRewardDuplicateDefaultIDLE", -1, 0f);
						return;
					}
				}
				return;
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
