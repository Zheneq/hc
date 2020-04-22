using System;

[Serializable]
public class StorePanelData
{
	public UIStoreSideNavButton Button;

	public UIStoreBaseInventoryPanel Panel;

	public StorePanelData(UIStoreBaseInventoryPanel panel, UIStoreSideNavButton button)
	{
		Panel = panel;
		Button = button;
	}
}
