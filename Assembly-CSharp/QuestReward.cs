using System.Text;
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
		return m_theBtn;
	}

	private void Awake()
	{
		Initialize();
	}

	private void Initialize()
	{
		if (m_initialized)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_initialized = true;
		m_theBtn = base.gameObject.GetComponent<_SelectableBtn>();
		if (!(m_theBtn != null))
		{
			return;
		}
		while (true)
		{
			m_tooltipHoverObj = m_theBtn.spriteController.GetComponent<UITooltipHoverObject>();
			return;
		}
	}

	public void Update()
	{
		if (!(m_shineContainer != null))
		{
			return;
		}
		while (true)
		{
			bool doActive = false;
			if (UIGameOverPanel.Get() != null)
			{
				doActive = (UIGameOverPanel.Get().XPStage == UIGameOverPanel.UpdateXPStage.Quest);
			}
			UIManager.SetGameObjectActive(m_shineContainer, doActive);
			return;
		}
	}

	public void SetSelectable(bool selectable)
	{
		if (!(m_theBtn != null))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_theBtn.spriteController, selectable);
			return;
		}
	}

	private bool SetupTooltip(UITooltipBase tooltip)
	{
		if (tooltip is UIInventoryItemTooltip)
		{
			if (m_itemTemplate != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						(tooltip as UIInventoryItemTooltip).Setup(m_itemTemplate);
						return true;
					}
				}
			}
		}
		else if (tooltip is UISimpleTooltip)
		{
			if (m_currencyReward != null)
			{
				if (m_currencyReward.Type == CurrencyType.UnlockFreelancerToken)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
						{
							string text;
							if (m_currencyReward.Amount == 1)
							{
								text = StringUtil.TR("FreeFreelancerUnlocked", "Global");
							}
							else
							{
								text = string.Format(StringUtil.TR("FreeFreelancerUnlocks", "Global"), m_currencyReward.Amount);
							}
							string text2 = text;
							(tooltip as UISimpleTooltip).Setup(text2);
							return true;
						}
						}
					}
				}
			}
		}
		return false;
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

	public void Setup(QuestCurrencyReward currencyReward, int rejectedCount)
	{
		Initialize();
		m_itemTemplate = null;
		m_currencyReward = currencyReward;
		float num = 1f;
		if (rejectedCount <= 0)
		{
			goto IL_012a;
		}
		if (currencyReward.Type != 0)
		{
			if (currencyReward.Type != CurrencyType.Experience)
			{
				if (currencyReward.Type != CurrencyType.FreelancerCurrency)
				{
					goto IL_012a;
				}
			}
		}
		int questBonusPerRejection = QuestWideData.Get().m_questBonusPerRejection;
		int questMaxRejectPercentage = QuestWideData.Get().m_questMaxRejectPercentage;
		if (m_bonusText != null)
		{
			UIManager.SetGameObjectActive(m_bonusText, true);
			if (rejectedCount >= questMaxRejectPercentage / questBonusPerRejection)
			{
				m_bonusText.text = StringUtil.TR("MaxBonusReward", "Quests");
			}
			else
			{
				m_bonusText.text = string.Format(StringUtil.TR("BonusReward", "Quests"), Mathf.Min(questMaxRejectPercentage, rejectedCount * questBonusPerRejection));
			}
		}
		num = 1f + (float)Mathf.Min(questMaxRejectPercentage, rejectedCount * questBonusPerRejection) / 100f;
		goto IL_0151;
		IL_0293:
		if (m_popoutContainer != null)
		{
			UIManager.SetGameObjectActive(m_popoutContainer, false);
		}
		CheckEXPReward();
		if (!(m_tooltipHoverObj != null))
		{
			return;
		}
		while (true)
		{
			m_tooltipHoverObj.Setup(TooltipType.Simple, SetupTooltip);
			m_tooltipHoverObj.Refresh();
			return;
		}
		IL_012a:
		if (m_bonusText != null)
		{
			UIManager.SetGameObjectActive(m_bonusText, false);
		}
		goto IL_0151;
		IL_0151:
		if (m_rewardText != null)
		{
			int num2 = (int)Mathf.Floor((float)currencyReward.Amount * num);
			TextMeshProUGUI rewardText = m_rewardText;
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
				if (currencyReward.Type != 0 && currencyReward.Type != CurrencyType.ModToken)
				{
					if (currencyReward.Type == CurrencyType.Experience)
					{
						string path = new StringBuilder().Append("Localization/").Append(StringUtil.TR("EXP", "TEXTURE")).ToString();
						m_rewardImage.sprite = (Resources.Load(path, typeof(Sprite)) as Sprite);
					}
					else if (currencyReward.Type == CurrencyType.UnlockFreelancerToken)
					{
						string path2 = "QuestRewards/FreelancerCoin";
						m_rewardImage.sprite = (Resources.Load(path2, typeof(Sprite)) as Sprite);
					}
					else
					{
						m_rewardImage.sprite = m_lockwoodSkinImage;
					}
					goto IL_0293;
				}
			}
		}
		m_rewardImage.sprite = Resources.Load<Sprite>(RewardUtils.GetCurrencyIconPath(currencyReward.Type));
		goto IL_0293;
	}

	public void SetupHack(string imageOverride, int amount = 0)
	{
		SetupHack(null, imageOverride, amount);
	}

	public void SetupHack(InventoryItemTemplate itemTemplate, string imageOverride, int amount = 0)
	{
		Initialize();
		m_itemTemplate = itemTemplate;
		m_currencyReward = null;
		if (imageOverride == string.Empty)
		{
			imageOverride = InventoryWideData.GetSpritePath(itemTemplate);
		}
		Sprite sprite = (Sprite)Resources.Load(imageOverride, typeof(Sprite));
		if ((bool)sprite)
		{
			m_rewardImage.sprite = sprite;
		}
		else
		{
			m_rewardImage.sprite = m_lockwoodSkinImage;
		}
		if (m_popoutContainer != null)
		{
			UIManager.SetGameObjectActive(m_popoutContainer, true);
			m_hoverIcon.sprite = m_rewardImage.sprite;
			SetIndicatorVisibility(itemTemplate.Rarity == InventoryItemRarity.Common, m_commonIndicators);
			SetIndicatorVisibility(itemTemplate.Rarity == InventoryItemRarity.Uncommon, m_uncommonIndicators);
			SetIndicatorVisibility(itemTemplate.Rarity == InventoryItemRarity.Rare, m_rareIndicators);
			SetIndicatorVisibility(itemTemplate.Rarity == InventoryItemRarity.Epic, m_epicIndicators);
			SetIndicatorVisibility(itemTemplate.Rarity == InventoryItemRarity.Legendary, m_legendaryIndicators);
		}
		if (m_rewardText != null)
		{
			if (amount > 1)
			{
				m_rewardText.text = UIStorePanel.FormatIntToString(amount, true);
			}
			else
			{
				m_rewardText.text = string.Empty;
			}
		}
		if (m_bonusText != null)
		{
			UIManager.SetGameObjectActive(m_bonusText, false);
		}
		CheckEXPReward();
		if (!(m_tooltipHoverObj != null))
		{
			return;
		}
		while (true)
		{
			m_tooltipHoverObj.Setup(TooltipType.InventoryItem, SetupTooltip);
			m_tooltipHoverObj.Refresh();
			return;
		}
	}

	private void CheckEXPReward()
	{
		if (!(UIFrontEnd.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (m_ExpUPAnim != null)
			{
				UIManager.SetGameObjectActive(m_ExpUPAnim, true);
			}
			return;
		}
	}
}
