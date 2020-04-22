using UnityEngine;

public class DebugCommand_LogSystemInfo : DebugCommand
{
	public override string GetDebugItemName()
	{
		return "Log System Info";
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
		return "/logsysteminfo";
	}

	public override bool AvailableInFrontEnd()
	{
		return true;
	}

	public override void OnIncreaseClick()
	{
		string text = "VRAM: " + SystemInfo.graphicsMemorySize + "\n";
		string text2 = text;
		text = text2 + "ShaderVersion: " + SystemInfo.graphicsShaderLevel + "\n";
		text2 = text;
		text = text2 + "MaxTextureSize: " + SystemInfo.maxTextureSize + "\n";
		text2 = text;
		text = text2 + "MaxCubemapSize: " + SystemInfo.maxCubemapSize + "\n";
		text = text + "GraphicsDeviceVersion: " + SystemInfo.graphicsDeviceVersion + "\n";
		TextConsole.Get().Write(text);
	}

	public override bool OnSlashCommand(string arguments)
	{
		string text = "VRAM: " + SystemInfo.graphicsMemorySize + "\n";
		string text2 = text;
		text = text2 + "ShaderVersion: " + SystemInfo.graphicsShaderLevel + "\n";
		text2 = text;
		text = text2 + "MaxTextureSize: " + SystemInfo.maxTextureSize + "\n";
		text2 = text;
		text = text2 + "MaxCubemapSize: " + SystemInfo.maxCubemapSize + "\n";
		text = text + "GraphicsDeviceVersion: " + SystemInfo.graphicsDeviceVersion + "\n";
		TextConsole.Get().Write(text);
		return true;
	}
}
