using System.Collections.Generic;
using UnityEngine;

public class DebugCommand
{
    private List<string> m_slashCommands;

    public bool CheatEnabled
    {
        get
        {
            ClientGameManager clientGameManager = ClientGameManager.Get();
            return clientGameManager != null
                   && clientGameManager.IsReady
                   && (clientGameManager.EnvironmentType != EnvironmentType.External
                       || clientGameManager.ClientAccessLevel == ClientAccessLevel.Admin);
        }
    }

    public virtual KeyCode GetKeyCode()
    {
        return KeyCode.None;
    }

    public virtual bool CheckGameControllerTrigger()
    {
        return false;
    }

    public virtual bool RequireCtrlKey()
    {
        return false;
    }

    public virtual bool RequireAltKey()
    {
        return false;
    }

    public virtual bool RequireShiftKey()
    {
        return false;
    }

    public virtual string GetPath()
    {
        return string.Empty;
    }

    public virtual string GetDebugItemName()
    {
        return string.Empty;
    }

    public virtual string GetDebugItemValue()
    {
        return string.Empty;
    }

    public virtual void OnIncreaseClick()
    {
    }

    public virtual void OnDecreaseClick()
    {
    }

    public virtual bool DisplayIncreaseButton()
    {
        return true;
    }

    public virtual bool DisplayDecreaseButton()
    {
        return false;
    }

    public virtual string GetIncreaseString()
    {
        return "+";
    }

    public virtual string GetDecreaseString()
    {
        return "-";
    }

    public virtual bool AvailableInFrontEnd()
    {
        return false;
    }

    public virtual string GetSlashCommand()
    {
        return string.Empty;
    }

    public virtual List<string> GetSlashCommands()
    {
        if (m_slashCommands != null)
        {
            return m_slashCommands;
        }

        m_slashCommands = new List<string>();
        string slashCommand = GetSlashCommand();
        if (!string.IsNullOrEmpty(slashCommand))
        {
            m_slashCommands.Add(slashCommand.ToLower());
        }

        return m_slashCommands;
    }

    public virtual bool OnSlashCommand(string arguments)
    {
        return false;
    }
}