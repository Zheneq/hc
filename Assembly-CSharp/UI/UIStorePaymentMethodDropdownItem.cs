using TMPro;
using UnityEngine;

public class UIStorePaymentMethodDropdownItem : MonoBehaviour
{
	public TextMeshProUGUI[] m_textLabels;

	public _ButtonSwapSprite m_hitbox;

	private PaymentMethod m_paymentMethodRef;

	public PaymentMethod GetPaymentMethod()
	{
		return m_paymentMethodRef;
	}

	public void SetPaymentMethod(PaymentMethod payMethod)
	{
		m_paymentMethodRef = payMethod;
	}

	public void SetText(string newText)
	{
		TextMeshProUGUI[] textLabels = m_textLabels;
		foreach (TextMeshProUGUI textMeshProUGUI in textLabels)
		{
			textMeshProUGUI.text = newText;
		}
	}
}
