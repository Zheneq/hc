using System;

[Serializable]
public class PaymentMethod
{
	public long id;

	public string specificType;

	public string generalType;

	public string tier;

	public string maskedPaymentInfo;

	public string expirationDate;

	public bool isDefault;

	public enum PaymentMethodType : long
	{
		Steam = -1L
	}
}
