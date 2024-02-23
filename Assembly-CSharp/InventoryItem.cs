using System;
using System.Text;

[Serializable]
public class InventoryItem : ICloneable
{
	public int Id;

	public int TemplateId;

	public int Count;

	public InventoryItem()
	{
		Id = 0;
		TemplateId = 0;
		Count = 0;
	}

	public InventoryItem(int templateId, int count = 1, int id = 0)
	{
		Id = id;
		TemplateId = templateId;
		Count = count;
	}

	public InventoryItem(InventoryItem itemToCopy, int id = 0)
	{
		Id = id;
		TemplateId = itemToCopy.TemplateId;
		Count = itemToCopy.Count;
	}

	public override string ToString()
	{
		return new StringBuilder().Append("[").Append(Id).Append("] ItemTemplateId ").Append(TemplateId).Append(", Count ").Append(Count).ToString();
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
