public class DebugCommand_Back : DebugCommand
{
	public UIDebugMenu m_debugMenu;

	public override string GetDebugItemName()
	{
		return "Back";
	}

	public override string GetIncreaseString()
	{
		return "<";
	}

	public override void OnIncreaseClick()
	{
		m_debugMenu.UpPathLevel();
	}
}
