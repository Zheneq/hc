using System;

[Serializable]
public class SchemaVersion<TEnum> : SchemaVersionBase where TEnum : IConvertible
{
	public SchemaVersion() : base(0UL)
	{
	}

	public bool IsChangeApplied(TEnum change)
	{
		return (this.IntValue & 1UL << Convert.ToInt32(change)) != 0UL;
	}

	public void MarkChangeApplied(TEnum change)
	{
		this.IntValue |= 1UL << Convert.ToInt32(change);
	}
}
