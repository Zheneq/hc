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
        m_debugCommands.Add(new DebugCommand_Options());
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
        bool inFrontEnd = GameFlowData.Get() == null;
        bool executed = false;
        foreach (DebugCommand debugCommand in m_debugCommands)
        {
            if ((inFrontEnd && !debugCommand.AvailableInFrontEnd())
                || !debugCommand.GetSlashCommands().Contains(command.ToLower()))
            {
                continue;
            }

            executed = debugCommand.OnSlashCommand(arguments);
            if (executed)
            {
                ClientGameManager.Get().symbol_001D(command, arguments);
                break;
            }
        }

        return executed;
    }

    public List<string> GetAvailableCommandNames(string searchStr)
    {
        bool inFrontEnd = GameFlowData.Get() == null;
        return GetAvailableCommandNames(searchStr, inFrontEnd);
    }

    public List<string> GetAvailableCommandNames(string searchStr, bool inFrontEnd)
    {
        List<string> result = new List<string>();
        searchStr.Trim(); // TODO client bug -- not assigned
        bool flag = searchStr.Length > 0;
        foreach (DebugCommand debugCommand in m_debugCommands)
        {
            if (inFrontEnd && !debugCommand.AvailableInFrontEnd())
            {
                continue;
            }

            string slashCommand = debugCommand.GetSlashCommand();
            if (!string.IsNullOrEmpty(slashCommand))
            {
                if (flag && slashCommand.IndexOf(searchStr, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                if (!result.Contains(slashCommand))
                {
                    result.Add(slashCommand);
                }
            }
        }

        result.Sort();
        return result;
    }

    public void OnIncreaseClick(DebugCommand command)
    {
        command.OnIncreaseClick();
        if (command.GetType() != typeof(DebugCommand_Category)
            && command.GetType() != typeof(DebugCommand_Back)
            && command.GetPath() != "Options")
        {
            ClientGameManager.Get().symbol_001D(command.GetDebugItemName(), command.GetDebugItemValue());
        }
    }

    public void OnDecreaseClick(DebugCommand command)
    {
        command.OnDecreaseClick();
        if (command.GetType() != typeof(DebugCommand_Category)
            && command.GetType() != typeof(DebugCommand_Back)
            && command.GetPath() != "Options")
        {
            ClientGameManager.Get().symbol_001D("Decrease " + command.GetDebugItemName(), command.GetDebugItemValue());
        }
    }
}