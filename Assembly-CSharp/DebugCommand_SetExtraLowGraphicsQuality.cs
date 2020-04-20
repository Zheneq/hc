using System;

public class DebugCommand_SetExtraLowGraphicsQuality : DebugCommand
{
	public override string GetDebugItemName()
	{
		return "Very Low Quality";
	}

	public override string GetPath()
	{
		return "Graphics";
	}

	public override string GetDebugItemValue()
	{
		if (Options_UI.Get() != null)
		{
			if (Options_UI.Get().GetCurrentGraphicsQuality() == GraphicsQuality.VeryLow)
			{
				return "on";
			}
		}
		return "off";
	}

	public override string GetSlashCommand()
	{
		return "/toggle";
	}

	public override bool AvailableInFrontEnd()
	{
		return true;
	}

	public override void OnIncreaseClick()
	{
		if (base.CheatEnabled)
		{
			if (Options_UI.Get() == null)
			{
				return;
			}
			Options_UI.Get().SetPendingGraphicsQuality((Options_UI.Get().GetCurrentGraphicsQuality() != GraphicsQuality.VeryLow) ? GraphicsQuality.VeryLow : GraphicsQuality.Low);
			Options_UI.Get().ApplyCurrentSettings();
		}
	}

	public override bool OnSlashCommand(string arguments)
	{
		if (arguments.EqualsIgnoreCase("lowgraphics"))
		{
			this.OnIncreaseClick();
			return true;
		}
		return false;
	}
}
