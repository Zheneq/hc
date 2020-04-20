using System;

public class DebugCommand_Snapshot : DebugCommand
{
	public override bool AvailableInFrontEnd()
	{
		return true;
	}

	public override string GetSlashCommand()
	{
		return "/snapshot";
	}

	public override bool OnSlashCommand(string arguments)
	{
		ClientGameManager.Get().symbol_0012();
		return true;
	}
}
