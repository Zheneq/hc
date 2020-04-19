using System;

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
		get
		{
			return string.Format("product {0} for player {1} ({2})", this.productCode, this.purchaserName, this.channelTransactionId);
		}
	}
}
