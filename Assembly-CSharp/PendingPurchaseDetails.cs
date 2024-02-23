using System;
using System.Text;

[Serializable]
public class PendingPurchaseDetails
{
	public string purchaserName;

	public Guid channelTransactionId;

	public PurchaseType purchaseType;

	public int[] typeSpecificData;

	public string productCode;

	public int quantity = 1;

	public int pollCount;

	public DateTime PricesRequestPacificTimeWithServerTimeOffsetAsOfPurchase;

	public string Name
	{
		get { return new StringBuilder().Append("product ").Append(productCode).Append(" for player ").Append(purchaserName).Append(" (").Append(channelTransactionId).Append(")").ToString(); }
	}
}
