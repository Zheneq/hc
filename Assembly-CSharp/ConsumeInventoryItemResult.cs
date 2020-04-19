using System;

public enum ConsumeInventoryItemResult
{
	None,
	Success,
	ItemNotFound,
	InventroyIsFull,
	ItemTemplateNotFound,
	ItemTemplateDisabled,
	ItemTemplateNotConsumable,
	ItemTemplateNotValid,
	CannotConsumeItem
}
