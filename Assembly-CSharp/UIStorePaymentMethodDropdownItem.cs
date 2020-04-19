using System;
using TMPro;
using UnityEngine;

public class UIStorePaymentMethodDropdownItem : MonoBehaviour
{
	public TextMeshProUGUI[] m_textLabels;

	public _ButtonSwapSprite m_hitbox;

	private PaymentMethod m_paymentMethodRef;

	public PaymentMethod GetPaymentMethod()
	{
		return this.m_paymentMethodRef;
	}

	public void SetPaymentMethod(PaymentMethod payMethod)
	{
		this.m_paymentMethodRef = payMethod;
	}

	public void SetText(string newText)
	{
		foreach (TextMeshProUGUI textMeshProUGUI in this.m_textLabels)
		{
			textMeshProUGUI.text = newText;
		}
	}
}
