using System.Linq;
using UnityEngine;

public class DebugCommand_TurnOffObjectShaderValue : DebugCommand
{
	public override string GetDebugItemName()
	{
		return "Turn Off Object Shader Value";
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
		return "/turnoffshadervalue";
	}

	public override void OnIncreaseClick()
	{
	}

	public override bool OnSlashCommand(string arguments)
	{
		string[] array = arguments.Split(" ".ToCharArray(), 3);
		if (array.Count() == 3)
		{
			while (true)
			{
				switch (3)
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
			GameObject gameObject = GameObject.Find(array[0]);
			if (gameObject != null)
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
				MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
				if (component != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					for (int i = 0; i < component.sharedMaterials.Length; i++)
					{
						if (array[2].EqualsIgnoreCase("texture"))
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
							component.sharedMaterials[i].SetTexture(array[1], null);
						}
						else
						{
							component.sharedMaterials[i].SetFloat(array[1], 0f);
						}
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
		}
		else
		{
			TextConsole.Get().Write("usage: /turnoffshadervalue <objectname> <shadervalue> [texture|float]");
		}
		return true;
	}
}
