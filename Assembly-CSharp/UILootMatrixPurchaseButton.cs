using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILootMatrixPurchaseButton : MonoBehaviour
{
	public TextMeshProUGUI m_packCount;

	public Image m_packImage;

	public TextMeshProUGUI m_priceAmount;

	public TextMeshProUGUI m_discountedAmount;

	public _SelectableBtn m_clickBtn;

	public TextMeshProUGUI m_eventText;

	public _SelectableBtn m_viewContentBtn;

	public LootMatrixPack PackRef
	{
		get;
		private set;
	}

	private void Awake()
	{
		m_viewContentBtn.spriteController.callback = delegate
		{
			if (PackRef != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						List<InventoryItemTemplate> list = new List<InventoryItemTemplate>();
						if (PackRef.IsInEvent())
						{
							for (int i = 0; i < PackRef.BonusMatrixes.Length; i++)
							{
								InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(PackRef.BonusMatrixes[i].LootMatrixId);
								if (itemTemplate.TypeSpecificData.Length > 1 && itemTemplate.TypeSpecificData[1] == 1)
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
									list.Add(itemTemplate);
								}
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
						if (list.Count > 0)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									UILootMatrixContentViewer.Get().Setup(list.ToArray());
									UILootMatrixContentViewer.Get().SetVisible(true);
									return;
								}
							}
						}
						return;
					}
					}
				}
			}
		};
	}

	public void ButtonClicked(BaseEventData data)
	{
		UILootMatrixPurchaseScreen.Get().PackClicked(this);
		m_clickBtn.spriteController.ForceSetPointerEntered(false);
	}

	public void Setup(LootMatrixPack pack)
	{
		bool flag = pack.IsInEvent();
		PackRef = pack;
		m_clickBtn.spriteController.callback = ButtonClicked;
		int num = pack.NumberOfMatrixes;
		if (num == 0)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int i = 0; i < pack.BonusMatrixes.Length; i++)
			{
				num += pack.BonusMatrixes[i].NumberOfMatrixes;
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
		m_packCount.text = num.ToString();
		Image packImage = m_packImage;
		Sprite sprite;
		if (flag)
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
			sprite = pack.EventPackSprite;
		}
		else
		{
			sprite = pack.LootMatrixPackSprite;
		}
		packImage.sprite = sprite;
		string accountCurrency = HydrogenConfig.Get().Ticket.AccountCurrency;
		float lootMatrixPackPrice = CommerceClient.Get().GetLootMatrixPackPrice(pack.ProductCode, accountCurrency);
		m_priceAmount.text = UIStorePanel.GetLocalizedPriceString(lootMatrixPackPrice, accountCurrency);
		m_eventText.text = pack.GetEventText();
		UIManager.SetGameObjectActive(m_eventText, flag);
		bool flag2 = false;
		if (PackRef.IsInEvent())
		{
			int num2 = 0;
			while (true)
			{
				if (num2 < PackRef.BonusMatrixes.Length)
				{
					InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(PackRef.BonusMatrixes[num2].LootMatrixId);
					if (itemTemplate.TypeSpecificData.Length > 1)
					{
						flag2 = (itemTemplate.TypeSpecificData[1] == 1);
						if (flag2)
						{
							break;
						}
					}
					num2++;
					continue;
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
				break;
			}
		}
		UIManager.SetGameObjectActive(m_viewContentBtn, flag2);
		float price = pack.Prices.GetPrice(accountCurrency);
		if (price == lootMatrixPackPrice)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					m_discountedAmount.text = string.Empty;
					return;
				}
			}
		}
		m_discountedAmount.text = UIStorePanel.GetLocalizedPriceString(price, accountCurrency);
	}
}
