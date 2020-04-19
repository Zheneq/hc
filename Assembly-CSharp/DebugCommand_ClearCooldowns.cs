using System;

public class DebugCommand_ClearCooldowns : DebugCommand
{
	public override string GetDebugItemName()
	{
		return "Cooldowns";
	}

	public override string GetPath()
	{
		return "Clear";
	}

	public override void OnIncreaseClick()
	{
		GameFlowData.Get().ClearCooldowns();
		GameFlowData.Get().RefillStocks();
	}

	public override string GetSlashCommand()
	{
		return "/clearcooldowns";
	}

	public override bool OnSlashCommand(string arguments)
	{
		this.OnIncreaseClick();
		return true;
	}
}
