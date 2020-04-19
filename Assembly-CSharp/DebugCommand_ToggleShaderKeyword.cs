using System;
using System.Linq;
using UnityEngine;

public class DebugCommand_ToggleShaderKeyword : DebugCommand
{
	public override string GetDebugItemName()
	{
		return "Toggle Shader Keyword";
	}

	public override string GetPath()
	{
		return "Graphics";
	}

	public override string GetDebugItemValue()
	{
		return string.Empty;
	}

	public override string GetSlashCommand()
	{
		return "/toggleshaderkeyword";
	}

	public override void OnIncreaseClick()
	{
	}

	public override bool OnSlashCommand(string arguments)
	{
		string[] array = arguments.Split(" ".ToCharArray(), 2);
		if (array.Count<string>() == 2)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DebugCommand_ToggleShaderKeyword.OnSlashCommand(string)).MethodHandle;
			}
			if (array[1].EqualsIgnoreCase("off"))
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
				Shader.DisableKeyword(array[0]);
			}
			else
			{
				Shader.EnableKeyword(array[0]);
			}
		}
		else
		{
			TextConsole.Get().Write("usage: /toggleshaderkeyword <shaderkeyword> [on|off]", ConsoleMessageType.SystemMessage);
		}
		return true;
	}
}
