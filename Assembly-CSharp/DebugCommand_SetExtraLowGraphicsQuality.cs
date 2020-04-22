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
		object result;
		if (Options_UI.Get() != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (Options_UI.Get().GetCurrentGraphicsQuality() == GraphicsQuality.VeryLow)
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
				result = "on";
				goto IL_004c;
			}
		}
		result = "off";
		goto IL_004c;
		IL_004c:
		return (string)result;
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
		if (!base.CheatEnabled)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(Options_UI.Get() == null))
			{
				Options_UI.Get().SetPendingGraphicsQuality((Options_UI.Get().GetCurrentGraphicsQuality() != GraphicsQuality.VeryLow) ? GraphicsQuality.VeryLow : GraphicsQuality.Low);
				Options_UI.Get().ApplyCurrentSettings();
			}
			return;
		}
	}

	public override bool OnSlashCommand(string arguments)
	{
		if (arguments.EqualsIgnoreCase("lowgraphics"))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					OnIncreaseClick();
					return true;
				}
			}
		}
		return false;
	}
}
