using System;

public class DebugCommand_Category : DebugCommand
{
	public string m_category;

	public UIDebugMenu m_debugMenu;

	public override string GetDebugItemName()
	{
		return this.m_category;
	}

	public override string GetIncreaseString()
	{
		return ">";
	}

	public override void OnIncreaseClick()
	{
		this.m_debugMenu.AddToPath(this.m_category);
	}
}
