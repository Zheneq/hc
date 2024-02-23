using System;
using System.Text;

[Serializable]
public class KarmaTemplate
{
	public int Index;

	public string Name;

	public string Description;

	public bool OneTimeOnly;

	public bool Enabled;

	public bool IsValid()
	{
		return Index > 0;
	}

	public string GetName()
	{
		return StringUtil.TR_KarmaName(Index);
	}

	public string GetDescription()
	{
		return StringUtil.TR_KarmaDescription(Index);
	}

	public Karma Process(int quantity)
	{
		Karma karma = new Karma();
		karma.TemplateId = Index;
		karma.Quantity = quantity;
		return karma;
	}

	public override string ToString()
	{
		return new StringBuilder().Append("[").Append(Index).Append("] ").Append(Name).Append(", OneTimeOnly=").Append(OneTimeOnly).ToString();
	}
}
