using System;
using System.Collections.Generic;

public class DebugCommands
{
	private static DebugCommands s_instance;

	public List<DebugCommand> m_debugCommands = new List<DebugCommand>();

	public DebugCommands()
	{
		this.m_debugCommands.Add(new DebugCommand_EndGameWin());
		this.m_debugCommands.Add(new DebugCommand_EndGameLoss());
		this.m_debugCommands.Add(new DebugCommand_EndGameTie());
		this.m_debugCommands.Add(new DebugCommand_EndGameNoResult());
		this.m_debugCommands.Add(new DebugCommand_EndGameWithParams());
		this.m_debugCommands.Add(new DebugCommand_ClearCooldowns());
		this.m_debugCommands.Add(new DebugCommand_ForceMatchmaking());
		this.m_debugCommands.Add(new DebugCommand_Discord());
		this.m_debugCommands.Add(new DebugCommand_Snapshot());
		this.m_debugCommands.Add(new DebugCommand_Gibberish());
		this.m_debugCommands.Add(new DebugCommand_SetExtraLowGraphicsQuality());
	}

	public static DebugCommands Get()
	{
		return DebugCommands.s_instance;
	}

	public static void Instantiate()
	{
		DebugCommands.s_instance = new DebugCommands();
	}

	~DebugCommands()
	{
		DebugCommands.s_instance = null;
	}

	public bool RunDebugCommand(string command, string arguments)
	{
		bool flag = GameFlowData.Get() == null;
		bool flag2 = false;
		using (List<DebugCommand>.Enumerator enumerator = this.m_debugCommands.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DebugCommand debugCommand = enumerator.Current;
				if (flag)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(DebugCommands.RunDebugCommand(string, string)).MethodHandle;
					}
					if (!debugCommand.AvailableInFrontEnd())
					{
						continue;
					}
				}
				if (debugCommand.\u0018().Contains(command.ToLower()))
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					flag2 = debugCommand.OnSlashCommand(arguments);
					if (flag2)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						ClientGameManager.Get().\u001D(command, arguments);
						return flag2;
					}
				}
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return flag2;
	}

	public List<string> GetAvailableCommandNames(string searchStr)
	{
		bool inFrontEnd = GameFlowData.Get() == null;
		return this.GetAvailableCommandNames(searchStr, inFrontEnd);
	}

	public List<string> GetAvailableCommandNames(string searchStr, bool inFrontEnd)
	{
		List<string> list = new List<string>();
		searchStr.Trim();
		bool flag = searchStr.Length > 0;
		foreach (DebugCommand debugCommand in this.m_debugCommands)
		{
			if (inFrontEnd)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(DebugCommands.GetAvailableCommandNames(string, bool)).MethodHandle;
				}
				if (!debugCommand.AvailableInFrontEnd())
				{
					continue;
				}
			}
			string slashCommand = debugCommand.GetSlashCommand();
			if (!string.IsNullOrEmpty(slashCommand))
			{
				for (;;)
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
					for (;;)
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
					for (;;)
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
					for (;;)
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
		if (command.GetType() != typeof(DebugCommand_Category) && command.GetType() != typeof(DebugCommand_Back))
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(DebugCommands.OnIncreaseClick(DebugCommand)).MethodHandle;
			}
			ClientGameManager.Get().\u001D(command.GetDebugItemName(), command.GetDebugItemValue());
		}
	}

	public void OnDecreaseClick(DebugCommand command)
	{
		command.OnDecreaseClick();
		if (command.GetType() != typeof(DebugCommand_Category) && command.GetType() != typeof(DebugCommand_Back))
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(DebugCommands.OnDecreaseClick(DebugCommand)).MethodHandle;
			}
			ClientGameManager.Get().\u001D("Decrease " + command.GetDebugItemName(), command.GetDebugItemValue());
		}
	}
}
