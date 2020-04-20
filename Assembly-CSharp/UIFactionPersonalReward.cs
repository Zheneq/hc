using System;
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
			this.m_rewardImage.sprite = Resources.Load<Sprite>(InventoryWideData.GetSpritePath(itemTemplate));
			text = itemTemplate.GetDisplayName();
		}
		FactionCurrencyReward factionCurrencyReward = reward as FactionCurrencyReward;
		if (factionCurrencyReward != null)
		{
			RewardUtils.RewardType type;
			string currencyIconPath = RewardUtils.GetCurrencyIconPath(factionCurrencyReward.CurrencyReward.Type, out type);
			this.m_rewardImage.sprite = Resources.Load<Sprite>(currencyIconPath);
			text = RewardUtils.GetTypeDisplayString(type, factionCurrencyReward.CurrencyReward.Amount > 1);
			if (factionCurrencyReward.CurrencyReward.Amount > 1)
			{
				text = text + " x" + factionCurrencyReward.CurrencyReward.Amount;
			}
		}
		FactionUnlockReward factionUnlockReward = reward as FactionUnlockReward;
		if (factionUnlockReward != null)
		{
			this.m_rewardImage.sprite = Resources.Load<Sprite>(factionUnlockReward.UnlockReward.resourceString);
			text = RewardUtils.GetRewardDisplayName(factionUnlockReward.UnlockReward.purchaseType, factionUnlockReward.UnlockReward.typeSpecificData);
		}
		UIManager.SetGameObjectActive(this.m_obtainedContainer, obtained, null);
		for (int i = 0; i < this.m_rewardLevelText.Length; i++)
		{
			this.m_rewardLevelText[i].text = (currentLevel + 1).ToString();
		}
		for (int j = 0; j < this.m_itemDescription.Length; j++)
		{
			this.m_itemDescription[j].text = text;
		}
	}
}
