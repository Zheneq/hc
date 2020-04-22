using System;

[Serializable]
public class GrantKarma
{
	public int KarmaTemplateId;

	public int MinValue;

	public int MaxValue;

	public override string ToString()
	{
		return $"KarmaIndex {KarmaTemplateId}, ({MinValue} - {MaxValue})";
	}
}
