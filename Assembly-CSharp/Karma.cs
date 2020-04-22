using System;

[Serializable]
public class Karma
{
	public int TemplateId;

	public int Quantity;

	public bool IsValid()
	{
		return Quantity >= 0;
	}
}
