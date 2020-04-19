using System;

[Serializable]
public class GrantKarma
{
	public int KarmaTemplateId;

	public int MinValue;

	public int MaxValue;

	public override string ToString()
	{
		return string.Format("KarmaIndex {0}, ({1} - {2})", this.KarmaTemplateId, this.MinValue, this.MaxValue);
	}
}
