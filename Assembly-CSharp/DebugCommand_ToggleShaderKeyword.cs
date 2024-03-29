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
		if (array.Count() == 2)
		{
			if (array[1].EqualsIgnoreCase("off"))
			{
				Shader.DisableKeyword(array[0]);
			}
			else
			{
				Shader.EnableKeyword(array[0]);
			}
		}
		else
		{
			TextConsole.Get().Write("usage: /toggleshaderkeyword <shaderkeyword> [on|off]");
		}
		return true;
	}
}
