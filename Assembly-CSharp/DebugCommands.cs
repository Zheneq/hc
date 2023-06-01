using System;
using System.Collections.Generic;

public class DebugCommands
{
	private static DebugCommands s_instance;

	public List<DebugCommand> m_debugCommands = new List<DebugCommand>();

	public DebugCommands()
	{
		m_debugCommands.Add(new DebugCommand_EndGameWin());
		m_debugCommands.Add(new DebugCommand_EndGameLoss());
		m_debugCommands.Add(new DebugCommand_EndGameTie());
		m_debugCommands.Add(new DebugCommand_EndGameNoResult());
		m_debugCommands.Add(new DebugCommand_EndGameWithParams());
		m_debugCommands.Add(new DebugCommand_ClearCooldowns());
		m_debugCommands.Add(new DebugCommand_ForceMatchmaking());
		m_debugCommands.Add(new DebugCommand_Discord());
		m_debugCommands.Add(new DebugCommand_Snapshot());
		m_debugCommands.Add(new DebugCommand_Gibberish());
		m_debugCommands.Add(new DebugCommand_SetExtraLowGraphicsQuality());
	}

	public static DebugCommands Get()
	{
		return s_instance;
	}

	public static void Instantiate()
	{
		s_instance = new DebugCommands();
	}

	~DebugCommands()
	{
		s_instance = null;
	}

  public bool RunDebugCommand(string command, string arguments)
  {
    bool flag1 = GameFlowData.Get() == null;
    bool flag2 = false;
    foreach (DebugCommand debugCommand in m_debugCommands)
    {
      if ((!flag1 || debugCommand.AvailableInFrontEnd()) && (debugCommand._0018().Contains(command.ToLower()) && (flag2 = debugCommand.OnSlashCommand(arguments))))
      {
        ClientGameManager.Get().symbol_001D(command, arguments);
        break;
      }
    }
    return flag2;
  }

	public List<string> GetAvailableCommandNames(string searchStr)
	{
		bool inFrontEnd = GameFlowData.Get() == null;
		return GetAvailableCommandNames(searchStr, inFrontEnd);
	}

	public List<string> GetAvailableCommandNames(string searchStr, bool inFrontEnd)
	{
		List<string> list = new List<string>();
		searchStr.Trim();
		bool flag = searchStr.Length > 0;
		foreach (DebugCommand debugCommand in m_debugCommands)
		{
			if (inFrontEnd)
			{
				if (!debugCommand.AvailableInFrontEnd())
				{
					continue;
				}
			}
			string slashCommand = debugCommand.GetSlashCommand();
			if (!string.IsNullOrEmpty(slashCommand))
			{
				if (flag)
				{
					if (slashCommand.IndexOf(searchStr, StringComparison.OrdinalIgnoreCase) < 0)
					{
						continue;
					}
				}
				if (!list.Contains(slashCommand))
				{
					list.Add(slashCommand);
				}
			}
		}
		list.Sort();
		return list;
	}

  public void OnIncreaseClick(DebugCommand command)
  {
    command.OnIncreaseClick();
    if (command.GetType() == typeof (DebugCommand_Category) || command.GetType() == typeof (DebugCommand_Back))
      return;
    ClientGameManager.Get().symbol_001D(command.GetDebugItemName(), command.GetDebugItemValue());
  }

  public void OnDecreaseClick(DebugCommand command)
  {
    command.OnDecreaseClick();
    if (command.GetType() == typeof (DebugCommand_Category) || command.GetType() == typeof (DebugCommand_Back))
      return;
    ClientGameManager.Get().symbol_001D("Decrease " + command.GetDebugItemName(), command.GetDebugItemValue());
  }
}
