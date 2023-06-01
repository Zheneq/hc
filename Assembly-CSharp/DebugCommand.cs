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

	public virtual KeyCode _001D()
	{
		return KeyCode.None;
	}

	public virtual bool CheckGameControllerTrigger()
	{
		return false;
	}

	public virtual bool _000E()
	{
		return false;
	}

	public virtual bool _0012()
	{
		return false;
	}

	public virtual bool _0015()
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

	public virtual bool _0016()
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

	public virtual string _0013()
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

	public virtual List<string> _0018()
	{
		if (m_slashCommands == null)
		{
			m_slashCommands = new List<string>();
			string slashCommand = GetSlashCommand();
			if (!string.IsNullOrEmpty(slashCommand))
			{
				m_slashCommands.Add(slashCommand.ToLower());
			}
		}
		return m_slashCommands;
	}

	public virtual bool OnSlashCommand(string arguments)
	{
		return false;
	}
}
