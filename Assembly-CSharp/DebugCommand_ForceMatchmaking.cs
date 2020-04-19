using System;

public class DebugCommand_ForceMatchmaking : DebugCommand
{
	private const string c_itemName = "Force Matchmaking";

	public override string GetDebugItemName()
	{
		return "Force Matchmaking";
	}

	public override string GetPath()
	{
		return "Queue";
	}

	public override bool AvailableInFrontEnd()
	{
		return true;
	}

	public override void OnIncreaseClick()
	{
		this.DoWork();
	}

	public override string GetSlashCommand()
	{
		return "/forcematchmaking";
	}

	public override bool OnSlashCommand(string arguments)
	{
		this.DoWork();
		return true;
	}

	private void DoWork()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DebugCommand_ForceMatchmaking.DoWork()).MethodHandle;
			}
			if (clientGameManager.LobbyInterface != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				clientGameManager.\u000E();
			}
		}
	}
}
