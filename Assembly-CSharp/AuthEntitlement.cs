using System;

public struct AuthEntitlement
{
	public long accountEntitlementId;

	public long entitlementId;

	public string entitlementCode;

	public int entitlementAmount;

	public DateTime modifiedDate;

	public DateTime expirationDate;

	public bool Expires
	{
		get
		{
			return this.expirationDate < DateTime.MaxValue;
		}
	}
}
