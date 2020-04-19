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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DebugCommand_SetExtraLowGraphicsQuality.GetDebugItemValue()).MethodHandle;
			}
			if (Options_UI.Get().GetCurrentGraphicsQuality() == GraphicsQuality.VeryLow)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(DebugCommand_SetExtraLowGraphicsQuality.OnIncreaseClick()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DebugCommand_SetExtraLowGraphicsQuality.OnSlashCommand(string)).MethodHandle;
			}
			this.OnIncreaseClick();
			return true;
		}
		return false;
	}
}
