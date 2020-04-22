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
			m_rewardName.raycastTarget = false;
		}
		if (m_rewardType != null)
		{
			m_rewardType.raycastTarget = false;
		}
		if (m_rewardAmount != null)
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
				while (true)
				{
					switch (6)
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
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (currentAlert.Type == AlertMissionType.Bonus)
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
						if (currencyType == currentAlert.BonusType)
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
							float num3 = (float)currentAlert.BonusMultiplier / 100f;
							num = (int)((float)template.TypeSpecificData[1] * num3);
						}
					}
				}
				GameManager gameManager = GameManager.Get();
				if (gameManager != null)
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
					if (GetIsEligibleForEventBonus(gameManager.GameplayOverrides))
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
						if (gameManager.GameplayOverrides.EventISOBonusPercent > 0)
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
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					TextMeshProUGUI rewardName = m_rewardName;
					string text = rewardName.text;
					rewardName.text = text + " (+" + num4 + ")";
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
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			m_rewardType.text = InventoryWideData.GetTypeString(template, item.Count);
		}
		if (m_rewardIcon != null)
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
			if (m_rewardBanner != null)
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
				if (template.Type == InventoryItemType.BannerID && GameBalanceVars.Get().GetBanner(template.TypeSpecificData[0]).m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
				{
					m_rewardBanner.sprite = (Sprite)Resources.Load(InventoryWideData.GetSpritePath(template), typeof(Sprite));
					UIManager.SetGameObjectActive(m_rewardIcon, false);
					UIManager.SetGameObjectActive(m_rewardBanner, true);
					if (m_bannerShadow != null)
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
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						UIManager.SetGameObjectActive(m_bannerShadow, false);
					}
				}
			}
		}
		if (m_rewardIcons != null)
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
			if (m_rewardBanners != null)
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
				Sprite sprite = (Sprite)Resources.Load(InventoryWideData.GetSpritePath(template), typeof(Sprite));
				int num5;
				if (template.Type == InventoryItemType.BannerID)
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
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				for (int j = 0; j < m_rewardBanners.Length; j++)
				{
					m_rewardBanners[j].sprite = sprite;
					UIManager.SetGameObjectActive(m_rewardBanners[j], flag);
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
			}
		}
		if (m_rewardFgs != null)
		{
			for (int k = 0; k < m_rewardFgs.Length; k++)
			{
				m_rewardFgs[k].sprite = InventoryWideData.GetItemFg(template);
				UIManager.SetGameObjectActive(m_rewardFgs[k], m_rewardFgs[k].sprite != null);
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
		if (m_rewardFg != null)
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
			m_rewardFg.sprite = InventoryWideData.GetItemFg(template);
			UIManager.SetGameObjectActive(m_rewardFg, m_rewardFg.sprite != null);
		}
		if (m_ownedIcon != null)
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
			UIManager.SetGameObjectActive(m_ownedIcon, isDuplicate);
		}
		if (m_isoCountText != null)
		{
			if (isoCount > template.Value)
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
				int num6 = isoCount - template.Value;
				m_isoCountText.text = "+" + template.Value + " (+" + num6 + ")";
			}
			else
			{
				m_isoCountText.text = "+" + isoCount;
			}
		}
		m_isDuplicate = isDuplicate;
		PlayDuplicateAnimation();
		if (m_rewardType != null)
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
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							str = "DefaultIN";
						}
						else
						{
							str = "Invalid";
						}
						string stateName = "LockboxRewardDuplicate" + (string)str;
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
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
				switch (5)
				{
				case 0:
					continue;
				}
				if (m_template.Type == InventoryItemType.Experience)
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
					if (m_template.Type == InventoryItemType.Faction)
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
						if (m_template.Type == InventoryItemType.Unlock || m_template.Type == InventoryItemType.Conveyance)
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
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_duplicateAnimator.IsInTransition(0))
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
					break;
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
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!m_isDuplicate)
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
				if (m_animationFinishedOnce)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
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
			while (true)
			{
				switch (6)
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
			if (!(gameplayOverrides.EventBonusStartDate > ClientGameManager.Get().UtcNow()))
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
				if (!(gameplayOverrides.EventBonusEndDate < ClientGameManager.Get().UtcNow()))
				{
					if (!gameplayOverrides.RequiredEventBonusEntitlement.IsNullOrEmpty())
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
						if (gameplayOverrides.RequiredEventBonusEntitlement != "*")
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
							if (ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetAppliedEntitlementCount(gameplayOverrides.RequiredEventBonusEntitlement) == 0)
							{
								return false;
							}
						}
					}
					return true;
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
		}
		return false;
	}
}
