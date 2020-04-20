using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGGBoostPurchaseButton : MonoBehaviour
{
	public TextMeshProUGUI m_packCount;

	public Image m_packImage;

	public TextMeshProUGUI m_priceAmount;

	public TextMeshProUGUI m_originalPriceAmount;

	public _SelectableBtn m_clickBtn;

	public TextMeshProUGUI m_eventText;

	public GGPack PackRef { get; private set; }

	private void Awake()
	{
		this.m_clickBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ButtonClicked);
	}

	public void ButtonClicked(BaseEventData data)
	{
		this.m_clickBtn.spriteController.ResetMouseState();
		UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
		uipurchaseableItem.m_itemType = PurchaseItemType.GGBoost;
		uipurchaseableItem.m_ggPack = this.PackRef;
		UIStorePanel.Get().OpenPurchaseDialog(uipurchaseableItem);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.StorePurchased);
	}

	public void Setup(GGPack pack)
	{
		this.PackRef = pack;
		UIManager.SetGameObjectActive(this.m_eventText, false, null);
		this.m_packCount.text = pack.NumberOfBoosts.ToString();
		this.m_packImage.sprite = pack.GGPackSprite;
		string accountCurrency = HydrogenConfig.Get().Ticket.AccountCurrency;
		float num = pack.Prices.GetPrice(accountCurrency);
		float num2 = CommerceClient.Get().GetGGPackPrice(pack.ProductCode, accountCurrency);
		if (num2 <= 0f)
		{
			num2 = num;
		}
		else if (num < num2)
		{
			num = num2;
		}
		this.m_priceAmount.text = UIStorePanel.GetLocalizedPriceString(num2, accountCurrency);
		UIManager.SetGameObjectActive(this.m_originalPriceAmount, num2 != num, null);
		this.m_originalPriceAmount.text = UIStorePanel.GetLocalizedPriceString(num, accountCurrency);
	}
}
