using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFactionPersonalReward : MonoBehaviour
{
	public Image m_rewardImage;

	public TextMeshProUGUI[] m_rewardLevelText;

	public TextMeshProUGUI[] m_itemDescription;

	public RectTransform m_notObtainedContainer;

	public RectTransform m_obtainedContainer;

	public void Setup(FactionReward reward, int currentLevel, bool obtained)
	{
		FactionItemReward factionItemReward = reward as FactionItemReward;
		string text = string.Empty;
		if (factionItemReward != null)
		{
			InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(factionItemReward.ItemReward.ItemTemplateId);
			m_rewardImage.sprite = Resources.Load<Sprite>(InventoryWideData.GetSpritePath(itemTemplate));
			text = itemTemplate.GetDisplayName();
		}
		FactionCurrencyReward factionCurrencyReward = reward as FactionCurrencyReward;
		if (factionCurrencyReward != null)
		{
			RewardUtils.RewardType rewardType;
			string currencyIconPath = RewardUtils.GetCurrencyIconPath(factionCurrencyReward.CurrencyReward.Type, out rewardType);
			m_rewardImage.sprite = Resources.Load<Sprite>(currencyIconPath);
			text = RewardUtils.GetTypeDisplayString(rewardType, factionCurrencyReward.CurrencyReward.Amount > 1);
			if (factionCurrencyReward.CurrencyReward.Amount > 1)
			{
				text = new StringBuilder().Append(text).Append(" x").Append(factionCurrencyReward.CurrencyReward.Amount).ToString();
			}
		}
		FactionUnlockReward factionUnlockReward = reward as FactionUnlockReward;
		if (factionUnlockReward != null)
		{
			m_rewardImage.sprite = Resources.Load<Sprite>(factionUnlockReward.UnlockReward.resourceString);
			text = RewardUtils.GetRewardDisplayName(factionUnlockReward.UnlockReward.purchaseType, factionUnlockReward.UnlockReward.typeSpecificData);
		}
		UIManager.SetGameObjectActive(m_obtainedContainer, obtained);
		for (int i = 0; i < m_rewardLevelText.Length; i++)
		{
			m_rewardLevelText[i].text = (currentLevel + 1).ToString();
		}
		for (int j = 0; j < m_itemDescription.Length; j++)
		{
			m_itemDescription[j].text = text;
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}
}
