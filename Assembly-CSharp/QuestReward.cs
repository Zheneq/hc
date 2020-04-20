using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestReward : MonoBehaviour
{
	public Image m_rewardImage;

	public TextMeshProUGUI m_rewardText;

	public TextMeshProUGUI m_bonusText;

	public RectTransform m_ExpUPAnim;

	public RectTransform m_shineContainer;

	public Sprite m_lockwoodSkinImage;

	public Image m_tooltipMouseOverHitBox;

	[Header("Popout Hover Icon")]
	public Image m_popoutContainer;

	public Image m_hoverIcon;

	public Image[] m_commonIndicators;

	public Image[] m_uncommonIndicators;

	public Image[] m_rareIndicators;

	public Image[] m_epicIndicators;

	public Image[] m_legendaryIndicators;

	private InventoryItemTemplate m_itemTemplate;

	private QuestCurrencyReward m_currencyReward;

	private _SelectableBtn m_theBtn;

	private UITooltipHoverObject m_tooltipHoverObj;

	private bool m_initialized;

	public _SelectableBtn GetButton()
	{
		return this.m_theBtn;
	}

	private void Awake()
	{
		this.Initialize();
	}

	private void Initialize()
	{
		if (this.m_initialized)
		{
			return;
		}
		this.m_initialized = true;
		this.m_theBtn = base.gameObject.GetComponent<_SelectableBtn>();
		if (this.m_theBtn != null)
		{
			this.m_tooltipHoverObj = this.m_theBtn.spriteController.GetComponent<UITooltipHoverObject>();
		}
	}

	public void Update()
	{
		if (this.m_shineContainer != null)
		{
			bool doActive = false;
			if (UIGameOverPanel.Get() != null)
			{
				doActive = (UIGameOverPanel.Get().XPStage == UIGameOverPanel.UpdateXPStage.Quest);
			}
			UIManager.SetGameObjectActive(this.m_shineContainer, doActive, null);
		}
	}

	public void SetSelectable(bool selectable)
	{
		if (this.m_theBtn != null)
		{
			UIManager.SetGameObjectActive(this.m_theBtn.spriteController, selectable, null);
		}
	}

	private bool SetupTooltip(UITooltipBase tooltip)
	{
		if (tooltip is UIInventoryItemTooltip)
		{
			if (this.m_itemTemplate != null)
			{
				(tooltip as UIInventoryItemTooltip).Setup(this.m_itemTemplate);
				return true;
			}
		}
		else if (tooltip is UISimpleTooltip)
		{
			if (this.m_currencyReward != null)
			{
				if (this.m_currencyReward.Type == CurrencyType.UnlockFreelancerToken)
				{
					string text;
					if (this.m_currencyReward.Amount == 1)
					{
						text = StringUtil.TR("FreeFreelancerUnlocked", "Global");
					}
					else
					{
						text = string.Format(StringUtil.TR("FreeFreelancerUnlocks", "Global"), this.m_currencyReward.Amount);
					}
					string text2 = text;
					(tooltip as UISimpleTooltip).Setup(text2);
					return true;
				}
			}
		}
		return false;
	}

	private void SetIndicatorVisibility(bool isVisible, Image[] indicators)
	{
		for (int i = 0; i < indicators.Length; i++)
		{
			UIManager.SetGameObjectActive(indicators[i], isVisible, null);
		}
	}

	public void Setup(QuestCurrencyReward currencyReward, int rejectedCount)
	{
		this.Initialize();
		this.m_itemTemplate = null;
		this.m_currencyReward = currencyReward;
		float num = 1f;
		if (rejectedCount > 0)
		{
			if (currencyReward.Type != CurrencyType.ISO)
			{
				if (currencyReward.Type != CurrencyType.Experience)
				{
					if (currencyReward.Type != CurrencyType.FreelancerCurrency)
					{
						goto IL_12A;
					}
				}
			}
			int questBonusPerRejection = QuestWideData.Get().m_questBonusPerRejection;
			int questMaxRejectPercentage = QuestWideData.Get().m_questMaxRejectPercentage;
			if (this.m_bonusText != null)
			{
				UIManager.SetGameObjectActive(this.m_bonusText, true, null);
				if (rejectedCount >= questMaxRejectPercentage / questBonusPerRejection)
				{
					this.m_bonusText.text = StringUtil.TR("MaxBonusReward", "Quests");
				}
				else
				{
					this.m_bonusText.text = string.Format(StringUtil.TR("BonusReward", "Quests"), Mathf.Min(questMaxRejectPercentage, rejectedCount * questBonusPerRejection));
				}
			}
			num = 1f + (float)Mathf.Min(questMaxRejectPercentage, rejectedCount * questBonusPerRejection) / 100f;
			goto IL_151;
		}
		IL_12A:
		if (this.m_bonusText != null)
		{
			UIManager.SetGameObjectActive(this.m_bonusText, false, null);
		}
		IL_151:
		if (this.m_rewardText != null)
		{
			int num2 = (int)Mathf.Floor((float)currencyReward.Amount * num);
			TMP_Text rewardText = this.m_rewardText;
			string text;
			if (num2 > 1)
			{
				text = UIStorePanel.FormatIntToString(num2, true);
			}
			else
			{
				text = string.Empty;
			}
			rewardText.text = text;
		}
		if (currencyReward.Type != CurrencyType.FreelancerCurrency)
		{
			if (currencyReward.Type != CurrencyType.GGPack)
			{
				if (currencyReward.Type != CurrencyType.ISO && currencyReward.Type != CurrencyType.ModToken)
				{
					if (currencyReward.Type == CurrencyType.Experience)
					{
						string path = "Localization/" + StringUtil.TR("EXP", "TEXTURE");
						this.m_rewardImage.sprite = (Resources.Load(path, typeof(Sprite)) as Sprite);
						goto IL_293;
					}
					if (currencyReward.Type == CurrencyType.UnlockFreelancerToken)
					{
						string path2 = "QuestRewards/FreelancerCoin";
						this.m_rewardImage.sprite = (Resources.Load(path2, typeof(Sprite)) as Sprite);
						goto IL_293;
					}
					this.m_rewardImage.sprite = this.m_lockwoodSkinImage;
					goto IL_293;
				}
			}
		}
		this.m_rewardImage.sprite = Resources.Load<Sprite>(RewardUtils.GetCurrencyIconPath(currencyReward.Type));
		IL_293:
		if (this.m_popoutContainer != null)
		{
			UIManager.SetGameObjectActive(this.m_popoutContainer, false, null);
		}
		this.CheckEXPReward();
		if (this.m_tooltipHoverObj != null)
		{
			this.m_tooltipHoverObj.Setup(TooltipType.Simple, new TooltipPopulateCall(this.SetupTooltip), null);
			this.m_tooltipHoverObj.Refresh();
		}
	}

	public void SetupHack(string imageOverride, int amount = 0)
	{
		this.SetupHack(null, imageOverride, amount);
	}

	public void SetupHack(InventoryItemTemplate itemTemplate, string imageOverride, int amount = 0)
	{
		this.Initialize();
		this.m_itemTemplate = itemTemplate;
		this.m_currencyReward = null;
		if (imageOverride == string.Empty)
		{
			imageOverride = InventoryWideData.GetSpritePath(itemTemplate);
		}
		Sprite sprite = (Sprite)Resources.Load(imageOverride, typeof(Sprite));
		if (sprite)
		{
			this.m_rewardImage.sprite = sprite;
		}
		else
		{
			this.m_rewardImage.sprite = this.m_lockwoodSkinImage;
		}
		if (this.m_popoutContainer != null)
		{
			UIManager.SetGameObjectActive(this.m_popoutContainer, true, null);
			this.m_hoverIcon.sprite = this.m_rewardImage.sprite;
			this.SetIndicatorVisibility(itemTemplate.Rarity == InventoryItemRarity.Common, this.m_commonIndicators);
			this.SetIndicatorVisibility(itemTemplate.Rarity == InventoryItemRarity.Uncommon, this.m_uncommonIndicators);
			this.SetIndicatorVisibility(itemTemplate.Rarity == InventoryItemRarity.Rare, this.m_rareIndicators);
			this.SetIndicatorVisibility(itemTemplate.Rarity == InventoryItemRarity.Epic, this.m_epicIndicators);
			this.SetIndicatorVisibility(itemTemplate.Rarity == InventoryItemRarity.Legendary, this.m_legendaryIndicators);
		}
		if (this.m_rewardText != null)
		{
			if (amount > 1)
			{
				this.m_rewardText.text = UIStorePanel.FormatIntToString(amount, true);
			}
			else
			{
				this.m_rewardText.text = string.Empty;
			}
		}
		if (this.m_bonusText != null)
		{
			UIManager.SetGameObjectActive(this.m_bonusText, false, null);
		}
		this.CheckEXPReward();
		if (this.m_tooltipHoverObj != null)
		{
			this.m_tooltipHoverObj.Setup(TooltipType.InventoryItem, new TooltipPopulateCall(this.SetupTooltip), null);
			this.m_tooltipHoverObj.Refresh();
		}
	}

	private void CheckEXPReward()
	{
		if (UIFrontEnd.Get() != null)
		{
			if (this.m_ExpUPAnim != null)
			{
				UIManager.SetGameObjectActive(this.m_ExpUPAnim, true, null);
			}
		}
	}
}
