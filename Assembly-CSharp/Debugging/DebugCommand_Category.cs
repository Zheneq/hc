public class DebugCommand_Category : DebugCommand
{
	public string m_category;

	public UIDebugMenu m_debugMenu;

	public override string GetDebugItemName()
	{
		return m_category;
	}

	public override string GetIncreaseString()
	{
		return ">";
	}

	public override void OnIncreaseClick()
	{
		m_debugMenu.AddToPath(m_category);
	}
}
