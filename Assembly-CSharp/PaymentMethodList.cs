using System;
using System.Collections.Generic;

[Serializable]
public class PaymentMethodList
{
	public List<PaymentMethod> PaymentMethods = new List<PaymentMethod>();

	public bool IsError;
}
