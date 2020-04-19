using System;

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
		return this.Index > 0;
	}

	public string GetName()
	{
		return StringUtil.TR_KarmaName(this.Index);
	}

	public string GetDescription()
	{
		return StringUtil.TR_KarmaDescription(this.Index);
	}

	public Karma Process(int quantity)
	{
		return new Karma
		{
			TemplateId = this.Index,
			Quantity = quantity
		};
	}

	public override string ToString()
	{
		return string.Format("[{0}] {1}, OneTimeOnly={2}", this.Index, this.Name, this.OneTimeOnly);
	}
}
