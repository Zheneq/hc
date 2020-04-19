using System;

[Serializable]
public class Sale
{
	public string Name;

	public ProductStringType ProductStringType;

	public string ProductString;

	public int PercentOff;

	public string StartTime;

	public string EndTime;

	public string AbTestGroupName;

	public int AbTestPercent;

	public string RequiredEntitlement;

	public bool IsEnabled;
}
