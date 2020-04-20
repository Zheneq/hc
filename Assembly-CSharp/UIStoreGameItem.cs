using System;
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
			this.m_packReference = pack;
			UIManager.SetGameObjectActive(base.gameObject, true, null);
			ClientGameManager clientGameManager = ClientGameManager.Get();
			HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
			string accountCurrency = hydrogenConfig.Ticket.AccountCurrency;
			bool hasPurchasedGame = ClientGameManager.Get().HasPurchasedGame;
			float num;
			float gamePackPrice = CommerceClient.Get().GetGamePackPrice(pack.ProductCode, accountCurrency, out num);
			if (hasPurchasedGame)
			{
				bool flag;
				if (hasPurchasedGame)
				{
					flag = (clientGameManager.HighestPurchasedGamePack < pack.Index);
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				UIManager.SetGameObjectActive(this.m_disabled, !flag2, null);
				UIManager.SetGameObjectActive(this.m_upgradeOriginalPriceContainer, true, null);
				UIManager.SetGameObjectActive(this.m_discountBanner, false, null);
				UIManager.SetGameObjectActive(this.m_originalPriceContainer, false, null);
				if (flag2)
				{
					for (int i = 0; i < pack.Upgrades.Length; i++)
					{
						if (pack.Upgrades[i].AlreadyOwnedGamePack == clientGameManager.HighestPurchasedGamePack)
						{
							float num2 = 0f;
							float gamePackPrice2 = CommerceClient.Get().GetGamePackPrice(pack.Upgrades[i].ProductCode, accountCurrency, out num2);
							this.m_currentPrice.text = UIStorePanel.GetLocalizedPriceString(gamePackPrice2, accountCurrency);
							UIManager.SetGameObjectActive(this.m_discountBanner, gamePackPrice2 < num2, null);
							UIManager.SetGameObjectActive(this.m_originalPriceContainer, gamePackPrice2 < num2, null);
							this.m_originalPrice.text = "<s>" + UIStorePanel.GetLocalizedPriceString(num2, accountCurrency) + "</s>";
							UIManager.SetGameObjectActive(this.m_upgradeOriginalPriceContainer, gamePackPrice2 == num2, null);
							goto IL_1E7;
						}
					}
				}
				else
				{
					this.m_currentPrice.text = string.Empty;
				}
				IL_1E7:
				string localizedPriceString = UIStorePanel.GetLocalizedPriceString(num, accountCurrency);
				this.m_upgradeOriginalPrice.text = string.Format(StringUtil.TR("OriginalPrice", "CashShop"), localizedPriceString);
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_disabled, false, null);
				UIManager.SetGameObjectActive(this.m_upgradeOriginalPriceContainer, false, null);
				this.m_currentPrice.text = UIStorePanel.GetLocalizedPriceString(gamePackPrice, accountCurrency);
				UIManager.SetGameObjectActive(this.m_discountBanner, num > gamePackPrice, null);
				this.m_originalPrice.text = "<s>" + UIStorePanel.GetLocalizedPriceString(num, accountCurrency) + "</s>";
				UIManager.SetGameObjectActive(this.m_originalPriceContainer, num > gamePackPrice, null);
			}
			this.m_banner.sprite = pack.Sprite;
			this.m_editionName.text = string.Format(StringUtil.TR("EditionBreak", "Store"), pack.GetEditionName());
			this.m_description.text = pack.GetDescription();
			List<UIInventoryItem> list = new List<UIInventoryItem>(this.m_inventoryItemContainer.GetComponentsInChildren<UIInventoryItem>(true));
			for (int j = 0; j < pack.InventoryItemTemplateIds.Length; j++)
			{
				UIInventoryItem uiinventoryItem;
				if (j < list.Count)
				{
					uiinventoryItem = list[j];
				}
				else
				{
					uiinventoryItem = UnityEngine.Object.Instantiate<UIInventoryItem>(this.m_inventoryItemPrefab);
					uiinventoryItem.transform.SetParent(this.m_inventoryItemContainer.transform);
					uiinventoryItem.transform.localPosition = Vector3.zero;
					uiinventoryItem.transform.localScale = Vector3.one;
				}
				uiinventoryItem.m_hitbox.selectableButton.m_ignoreHoverAnimationCall = true;
				uiinventoryItem.m_hitbox.selectableButton.m_ignoreHoverOutAnimCall = true;
				uiinventoryItem.m_hitbox.selectableButton.m_ignorePressAnimationCall = true;
				uiinventoryItem.Setup(InventoryWideData.Get().GetItemTemplate(pack.InventoryItemTemplateIds[j]), null);
				Component buyButton3xLabel = uiinventoryItem.m_buyButton3xLabel;
				bool doActive;
				if (pack.InventoryItemTemplateIds[j] >= 0x275)
				{
					doActive = (pack.InventoryItemTemplateIds[j] > 0x277);
				}
				else
				{
					doActive = true;
				}
				UIManager.SetGameObjectActive(buyButton3xLabel, doActive, null);
				this.m_hitbox.AddSubButton(uiinventoryItem.m_hitbox);
				uiinventoryItem.m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.GameItemClicked);
			}
			this.m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.GameItemClicked);
		}
		else
		{
			UIManager.SetGameObjectActive(base.gameObject, false, null);
		}
	}

	public GamePack GetGamePackReference()
	{
		return this.m_packReference;
	}

	public void GameItemClicked(BaseEventData data)
	{
		this.m_hitbox.ResetMouseState();
		UICashShopPanel.Get().m_gamePanel.PurchaseGame(this);
	}
}
