using System.Text;
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
		string text = new StringBuilder().Append("VRAM: ").Append(SystemInfo.graphicsMemorySize).Append("\n").ToString();
		string text2 = text;
		text = new StringBuilder().Append(text2).Append("ShaderVersion: ").Append(SystemInfo.graphicsShaderLevel).Append("\n").ToString();
		text2 = text;
		text = new StringBuilder().Append(text2).Append("MaxTextureSize: ").Append(SystemInfo.maxTextureSize).Append("\n").ToString();
		text2 = text;
		text = new StringBuilder().Append(text2).Append("MaxCubemapSize: ").Append(SystemInfo.maxCubemapSize).Append("\n").ToString();
		text = new StringBuilder().Append(text).Append("GraphicsDeviceVersion: ").Append(SystemInfo.graphicsDeviceVersion).Append("\n").ToString();
		TextConsole.Get().Write(text);
	}

	public override bool OnSlashCommand(string arguments)
	{
		string text = new StringBuilder().Append("VRAM: ").Append(SystemInfo.graphicsMemorySize).Append("\n").ToString();
		string text2 = text;
		text = new StringBuilder().Append(text2).Append("ShaderVersion: ").Append(SystemInfo.graphicsShaderLevel).Append("\n").ToString();
		text2 = text;
		text = new StringBuilder().Append(text2).Append("MaxTextureSize: ").Append(SystemInfo.maxTextureSize).Append("\n").ToString();
		text2 = text;
		text = new StringBuilder().Append(text2).Append("MaxCubemapSize: ").Append(SystemInfo.maxCubemapSize).Append("\n").ToString();
		text = new StringBuilder().Append(text).Append("GraphicsDeviceVersion: ").Append(SystemInfo.graphicsDeviceVersion).Append("\n").ToString();
		TextConsole.Get().Write(text);
		return true;
	}
}
