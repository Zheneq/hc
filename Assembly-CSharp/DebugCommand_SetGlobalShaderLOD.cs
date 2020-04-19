using System;
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
		int num = Shader.globalMaximumLOD;
		num = Mathf.Clamp(num, 0, 0x28A);
		num += 0x32;
		num = Mathf.Clamp(num, 0, 0x28A);
		Shader.globalMaximumLOD = num;
	}

	public override void OnDecreaseClick()
	{
		int num = Shader.globalMaximumLOD;
		num = Mathf.Clamp(num, 0, 0x28A);
		num -= 0x32;
		num = Mathf.Clamp(num, 0, 0x28A);
		Shader.globalMaximumLOD = num;
	}

	public override bool OnSlashCommand(string arguments)
	{
		int num = int.Parse(arguments);
		num = Mathf.Clamp(num, 0, 0x28A);
		Shader.globalMaximumLOD = num;
		return true;
	}
}
