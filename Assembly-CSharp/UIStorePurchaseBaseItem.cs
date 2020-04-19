using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStorePurchaseBaseItem : MonoBehaviour
{
	public TextMeshProUGUI m_gameCurrencyLabel;

	public TextMeshProUGUI m_realCurrencyLabel;

	public TextMeshProUGUI m_discountPriceLabel;

	public TextMeshProUGUI m_headerNameLabel;

	public TextMeshProUGUI m_headerPriceLabel;

	public Image m_lockedIcon;

	public Image m_ownedIcon;

	public Image m_selectedCurrent;

	public Image m_selectedInUse;

	public Image m_realCurrencyIcon;

	public Image m_gameCurrencyIcon;

	public Image m_itemIcon;

	public Image m_itemFG;

	public _ButtonSwapSprite m_hitBox;

	public RectTransform m_discountLabelContainer;

	public RectTransform m_headerNameContainer;

	public RectTransform m_headerPriceContainer;

	public RectTransform m_priceContainer;

	public bool IsSelected()
	{
		return this.m_selectedCurrent.gameObject.activeSelf;
	}

	public void SetSelectedInUse(bool selected)
	{
		UIManager.SetGameObjectActive(this.m_selectedInUse, selected, null);
	}

	public void SetSelected(bool selected)
	{
		UIManager.SetGameObjectActive(this.m_selectedCurrent, selected, null);
	}
}
