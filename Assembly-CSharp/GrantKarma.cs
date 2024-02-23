using System;
using System.Text;

[Serializable]
public class GrantKarma
{
	public int KarmaTemplateId;

	public int MinValue;

	public int MaxValue;

	public override string ToString()
	{
		return new StringBuilder().Append("KarmaIndex ").Append(KarmaTemplateId).Append(", (").Append(MinValue).Append(" - ").Append(MaxValue).Append(")").ToString();
	}
}
