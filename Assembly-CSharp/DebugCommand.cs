using System;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommand
{
	private List<string> m_slashCommands;

	public virtual KeyCode symbol_001D()
	{
		return KeyCode.None;
	}

	public virtual bool CheckGameControllerTrigger()
	{
		return false;
	}

	public virtual bool symbol_000E()
	{
		return false;
	}

	public virtual bool symbol_0012()
	{
		return false;
	}

	public virtual bool symbol_0015()
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

	public virtual bool symbol_0016()
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

	public virtual string symbol_0013()
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

	public virtual List<string> symbol_0018()
	{
		if (this.m_slashCommands == null)
		{
			this.m_slashCommands = new List<string>();
			string slashCommand = this.GetSlashCommand();
			if (!string.IsNullOrEmpty(slashCommand))
			{
				this.m_slashCommands.Add(slashCommand.ToLower());
			}
		}
		return this.m_slashCommands;
	}

	public virtual bool OnSlashCommand(string arguments)
	{
		return false;
	}

	public bool CheatEnabled
	{
		get
		{
			ClientGameManager clientGameManager = ClientGameManager.Get();
			if (clientGameManager != null)
			{
				if (clientGameManager.IsReady)
				{
					return clientGameManager.EnvironmentType != EnvironmentType.External || clientGameManager.ClientAccessLevel == ClientAccessLevel.Admin;
				}
			}
			return false;
		}
	}
}
