using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStoreGameItem : MonoBehaviour
{
	public RectTransform m_discountBanner;

	public TextMeshProUGUI m_currentPrice;

	public RectTransform m_originalPriceContainer;

	public TextMeshProUGUI m_originalPrice;

	public RectTransform m_upgradeOriginalPriceContainer;

	public TextMeshProUGUI m_upgradeOriginalPrice;

	public Image m_banner;

	public TextMeshProUGUI m_editionName;

	public TextMeshProUGUI m_description;

	public GridLayoutGroup m_inventoryItemContainer;

	public UIInventoryItem m_inventoryItemPrefab;

	public _ButtonSwapSprite m_hitbox;

	public Image m_disabled;

	private GamePack m_packReference;

	public void Setup(GamePack pack)
	{
		if (pack != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					m_packReference = pack;
					UIManager.SetGameObjectActive(base.gameObject, true);
					ClientGameManager clientGameManager = ClientGameManager.Get();
					HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
					string accountCurrency = hydrogenConfig.Ticket.AccountCurrency;
					bool hasPurchasedGame = ClientGameManager.Get().HasPurchasedGame;
					float originalPrice;
					float gamePackPrice = CommerceClient.Get().GetGamePackPrice(pack.ProductCode, accountCurrency, out originalPrice);
					if (hasPurchasedGame)
					{
						int num;
						if (hasPurchasedGame)
						{
							num = ((clientGameManager.HighestPurchasedGamePack < pack.Index) ? 1 : 0);
						}
						else
						{
							num = 0;
						}
						bool flag = (byte)num != 0;
						UIManager.SetGameObjectActive(m_disabled, !flag);
						UIManager.SetGameObjectActive(m_upgradeOriginalPriceContainer, true);
						UIManager.SetGameObjectActive(m_discountBanner, false);
						UIManager.SetGameObjectActive(m_originalPriceContainer, false);
						if (flag)
						{
							int num2 = 0;
							while (true)
							{
								if (num2 >= pack.Upgrades.Length)
								{
									break;
								}
								if (pack.Upgrades[num2].AlreadyOwnedGamePack == clientGameManager.HighestPurchasedGamePack)
								{
									float originalPrice2 = 0f;
									float gamePackPrice2 = CommerceClient.Get().GetGamePackPrice(pack.Upgrades[num2].ProductCode, accountCurrency, out originalPrice2);
									m_currentPrice.text = UIStorePanel.GetLocalizedPriceString(gamePackPrice2, accountCurrency);
									UIManager.SetGameObjectActive(m_discountBanner, gamePackPrice2 < originalPrice2);
									UIManager.SetGameObjectActive(m_originalPriceContainer, gamePackPrice2 < originalPrice2);
									m_originalPrice.text = "<s>" + UIStorePanel.GetLocalizedPriceString(originalPrice2, accountCurrency) + "</s>";
									UIManager.SetGameObjectActive(m_upgradeOriginalPriceContainer, gamePackPrice2 == originalPrice2);
									break;
								}
								num2++;
							}
						}
						else
						{
							m_currentPrice.text = string.Empty;
						}
						string localizedPriceString = UIStorePanel.GetLocalizedPriceString(originalPrice, accountCurrency);
						m_upgradeOriginalPrice.text = string.Format(StringUtil.TR("OriginalPrice", "CashShop"), localizedPriceString);
					}
					else
					{
						UIManager.SetGameObjectActive(m_disabled, false);
						UIManager.SetGameObjectActive(m_upgradeOriginalPriceContainer, false);
						m_currentPrice.text = UIStorePanel.GetLocalizedPriceString(gamePackPrice, accountCurrency);
						UIManager.SetGameObjectActive(m_discountBanner, originalPrice > gamePackPrice);
						m_originalPrice.text = "<s>" + UIStorePanel.GetLocalizedPriceString(originalPrice, accountCurrency) + "</s>";
						UIManager.SetGameObjectActive(m_originalPriceContainer, originalPrice > gamePackPrice);
					}
					m_banner.sprite = pack.Sprite;
					m_editionName.text = string.Format(StringUtil.TR("EditionBreak", "Store"), pack.GetEditionName());
					m_description.text = pack.GetDescription();
					List<UIInventoryItem> list = new List<UIInventoryItem>(m_inventoryItemContainer.GetComponentsInChildren<UIInventoryItem>(true));
					for (int i = 0; i < pack.InventoryItemTemplateIds.Length; i++)
					{
						UIInventoryItem uIInventoryItem;
						if (i < list.Count)
						{
							uIInventoryItem = list[i];
						}
						else
						{
							uIInventoryItem = Object.Instantiate(m_inventoryItemPrefab);
							uIInventoryItem.transform.SetParent(m_inventoryItemContainer.transform);
							uIInventoryItem.transform.localPosition = Vector3.zero;
							uIInventoryItem.transform.localScale = Vector3.one;
						}
						uIInventoryItem.m_hitbox.selectableButton.m_ignoreHoverAnimationCall = true;
						uIInventoryItem.m_hitbox.selectableButton.m_ignoreHoverOutAnimCall = true;
						uIInventoryItem.m_hitbox.selectableButton.m_ignorePressAnimationCall = true;
						uIInventoryItem.Setup(InventoryWideData.Get().GetItemTemplate(pack.InventoryItemTemplateIds[i]), null);
						TextMeshProUGUI buyButton3xLabel = uIInventoryItem.m_buyButton3xLabel;
						int doActive;
						if (pack.InventoryItemTemplateIds[i] >= 629)
						{
							doActive = ((pack.InventoryItemTemplateIds[i] > 631) ? 1 : 0);
						}
						else
						{
							doActive = 1;
						}
						UIManager.SetGameObjectActive(buyButton3xLabel, (byte)doActive != 0);
						m_hitbox.AddSubButton(uIInventoryItem.m_hitbox);
						uIInventoryItem.m_hitbox.callback = GameItemClicked;
					}
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							m_hitbox.callback = GameItemClicked;
							return;
						}
					}
				}
				}
			}
		}
		UIManager.SetGameObjectActive(base.gameObject, false);
	}

	public GamePack GetGamePackReference()
	{
		return m_packReference;
	}

	public void GameItemClicked(BaseEventData data)
	{
		m_hitbox.ResetMouseState();
		UICashShopPanel.Get().m_gamePanel.PurchaseGame(this);
	}
}
