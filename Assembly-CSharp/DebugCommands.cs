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
		bool flag = GameFlowData.Get() == null;
		bool flag2 = false;
		using (List<DebugCommand>.Enumerator enumerator = m_debugCommands.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DebugCommand current = enumerator.Current;
				if (flag)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (!current.AvailableInFrontEnd())
					{
						continue;
					}
				}
				if (current._0018().Contains(command.ToLower()))
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					flag2 = current.OnSlashCommand(arguments);
					if (flag2)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								ClientGameManager.Get()._001D(command, arguments);
								return flag2;
							}
						}
					}
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return flag2;
				}
			}
		}
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
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!debugCommand.AvailableInFrontEnd())
				{
					continue;
				}
			}
			string slashCommand = debugCommand.GetSlashCommand();
			if (!string.IsNullOrEmpty(slashCommand))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (flag)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (slashCommand.IndexOf(searchStr, StringComparison.OrdinalIgnoreCase) < 0)
					{
						continue;
					}
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (!list.Contains(slashCommand))
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
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
		if (command.GetType() == typeof(DebugCommand_Category) || command.GetType() == typeof(DebugCommand_Back))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ClientGameManager.Get()._001D(command.GetDebugItemName(), command.GetDebugItemValue());
			return;
		}
	}

	public void OnDecreaseClick(DebugCommand command)
	{
		command.OnDecreaseClick();
		if (command.GetType() == typeof(DebugCommand_Category) || command.GetType() == typeof(DebugCommand_Back))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ClientGameManager.Get()._001D("Decrease " + command.GetDebugItemName(), command.GetDebugItemValue());
			return;
		}
	}
}
