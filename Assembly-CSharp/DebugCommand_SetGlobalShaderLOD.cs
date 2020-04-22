using UnityEngine;

public class DebugCommand_SetGlobalShaderLOD : DebugCommand
{
	public override string GetDebugItemName()
	{
		return "Global Maximum Shader LOD";
	}

	public override string GetPath()
	{
		return "Graphics";
	}

	public override string GetDebugItemValue()
	{
		return Shader.globalMaximumLOD.ToString();
	}

	public override string GetSlashCommand()
	{
		return "/maxshaderlod";
	}

	public override bool AvailableInFrontEnd()
	{
		return true;
	}

	public override bool DisplayDecreaseButton()
	{
		return true;
	}

	public override void OnIncreaseClick()
	{
		int globalMaximumLOD = Shader.globalMaximumLOD;
		globalMaximumLOD = Mathf.Clamp(globalMaximumLOD, 0, 650);
		globalMaximumLOD += 50;
		globalMaximumLOD = (Shader.globalMaximumLOD = Mathf.Clamp(globalMaximumLOD, 0, 650));
	}

	public override void OnDecreaseClick()
	{
		int globalMaximumLOD = Shader.globalMaximumLOD;
		globalMaximumLOD = Mathf.Clamp(globalMaximumLOD, 0, 650);
		globalMaximumLOD -= 50;
		globalMaximumLOD = (Shader.globalMaximumLOD = Mathf.Clamp(globalMaximumLOD, 0, 650));
	}

	public override bool OnSlashCommand(string arguments)
	{
		int value = int.Parse(arguments);
		value = (Shader.globalMaximumLOD = Mathf.Clamp(value, 0, 650));
		return true;
	}
}
