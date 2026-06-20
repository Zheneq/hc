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

	public GGPack PackRef
	{
		get;
		private set;
	}

	private void Awake()
	{
		m_clickBtn.spriteController.callback = ButtonClicked;
	}

	public void ButtonClicked(BaseEventData data)
	{
		m_clickBtn.spriteController.ResetMouseState();
		UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
		uIPurchaseableItem.m_itemType = PurchaseItemType.GGBoost;
		uIPurchaseableItem.m_ggPack = PackRef;
		UIStorePanel.Get().OpenPurchaseDialog(uIPurchaseableItem);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.StorePurchased);
	}

	public void Setup(GGPack pack)
	{
		PackRef = pack;
		UIManager.SetGameObjectActive(m_eventText, false);
		m_packCount.text = pack.NumberOfBoosts.ToString();
		m_packImage.sprite = pack.GGPackSprite;
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
		m_priceAmount.text = UIStorePanel.GetLocalizedPriceString(num2, accountCurrency);
		UIManager.SetGameObjectActive(m_originalPriceAmount, num2 != num);
		m_originalPriceAmount.text = UIStorePanel.GetLocalizedPriceString(num, accountCurrency);
	}
}
