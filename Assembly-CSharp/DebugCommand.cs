using System;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommand
{
	private List<string> m_slashCommands;

	public virtual KeyCode \u001D()
	{
		return KeyCode.None;
	}

	public virtual bool CheckGameControllerTrigger()
	{
		return false;
	}

	public virtual bool \u000E()
	{
		return false;
	}

	public virtual bool \u0012()
	{
		return false;
	}

	public virtual bool \u0015()
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

	public virtual bool \u0016()
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

	public virtual string \u0013()
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

	public virtual List<string> \u0018()
	{
		if (this.m_slashCommands == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DebugCommand.\u0018()).MethodHandle;
			}
			this.m_slashCommands = new List<string>();
			string slashCommand = this.GetSlashCommand();
			if (!string.IsNullOrEmpty(slashCommand))
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
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(DebugCommand.get_CheatEnabled()).MethodHandle;
				}
				if (clientGameManager.IsReady)
				{
					return clientGameManager.EnvironmentType != EnvironmentType.External || clientGameManager.ClientAccessLevel == ClientAccessLevel.Admin;
				}
			}
			return false;
		}
	}
}
