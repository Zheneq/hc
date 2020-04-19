using System;

[Serializable]
public class InventoryItem : ICloneable
{
	public int Id;

	public int TemplateId;

	public int Count;

	public InventoryItem()
	{
		this.Id = 0;
		this.TemplateId = 0;
		this.Count = 0;
	}

	public InventoryItem(int templateId, int count = 1, int id = 0)
	{
		this.Id = id;
		this.TemplateId = templateId;
		this.Count = count;
	}

	public InventoryItem(InventoryItem itemToCopy, int id = 0)
	{
		this.Id = id;
		this.TemplateId = itemToCopy.TemplateId;
		this.Count = itemToCopy.Count;
	}

	public override string ToString()
	{
		return string.Format("[{0}] ItemTemplateId {1}, Count {2}", this.Id, this.TemplateId, this.Count);
	}

	public object Clone()
	{
		return base.MemberwiseClone();
	}
}
