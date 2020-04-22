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
		DoWork();
	}

	public override string GetSlashCommand()
	{
		return "/forcematchmaking";
	}

	public override bool OnSlashCommand(string arguments)
	{
		DoWork();
		return true;
	}

	private void DoWork()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (!(clientGameManager != null))
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
			if (clientGameManager.LobbyInterface != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					clientGameManager._000E();
					return;
				}
			}
			return;
		}
	}
}
