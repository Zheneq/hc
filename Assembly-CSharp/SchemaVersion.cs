using System;

[Serializable]
public class SchemaVersion<TEnum> : SchemaVersionBase where TEnum : IConvertible
{
	public SchemaVersion()
		: base(0uL)
	{
	}

	public bool IsChangeApplied(TEnum change)
	{
		return ((long)IntValue & (1L << Convert.ToInt32(change))) != 0;
	}

	public void MarkChangeApplied(TEnum change)
	{
		IntValue |= (ulong)(1L << Convert.ToInt32(change));
	}
}
