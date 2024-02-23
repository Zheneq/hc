using System.Text;

public class DebugCommand_Discord : DebugCommand
{
	public override bool AvailableInFrontEnd()
	{
		return true;
	}

	public override string GetSlashCommand()
	{
		return "/discord";
	}

	public override bool OnSlashCommand(string arguments)
	{
		if (!DiscordClientInterface.IsEnabled)
		{
			return false;
		}
		string[] array = arguments.Split(null);
		string text = "/discord command requires following format\r\n/discord join\r\n/discord leave\r\n/discord autojoin [on/off]\r\n/discord check\r\n/discord shutdown\r\n/discord help\r\n";
		if (!array[0].IsNullOrEmpty())
		{
			if (!array[0].EqualsIgnoreCase("help"))
			{
				if (array[0].EqualsIgnoreCase("check"))
				{
					ClientGameManager.Get().CheckDiscord();
				}
				else if (array[0].EqualsIgnoreCase("join"))
				{
					if (!DiscordClientInterface.CanJoinTeamChat)
					{
						if (!DiscordClientInterface.CanJoinGroupChat)
						{
							TextConsole.Get().Write("Failed to join Discord chat. You are not in a team or group.");
							goto IL_02b0;
						}
					}
					ClientGameManager.Get().JoinDiscord();
				}
				else if (array[0].EqualsIgnoreCase("leave"))
				{
					ClientGameManager.Get().LeaveDiscord();
				}
				else if (array[0].EqualsIgnoreCase("autojoin"))
				{
					bool result = true;
					if (array.Length > 1)
					{
						if (!bool.TryParse(array[1], out result))
						{
							if (array[1].EqualsIgnoreCase("on"))
							{
								result = true;
							}
							else if (array[1].EqualsIgnoreCase("off"))
							{
								result = false;
							}
						}
					}
					ClientGameManager.Get().ConfigureDiscord(result);
				}
				else if (array[0].EqualsIgnoreCase("port"))
				{
					int result2 = 0;
					if (int.TryParse(array[1], out result2))
					{
						DiscordClientInterface.s_RpcPortOverride = result2;
					}
				}
				else if (array[0].EqualsIgnoreCase("shutdown"))
				{
					DiscordClientInterface.Shutdown();
				}
				else if (array[0].EqualsIgnoreCase("debug"))
				{
					bool result3 = true;
					if (array.Length > 1 && !bool.TryParse(array[1], out result3))
					{
						if (array[1].EqualsIgnoreCase("on"))
						{
							result3 = true;
						}
						else if (array[1].EqualsIgnoreCase("off"))
						{
							result3 = false;
						}
					}
					Log.Info("Discord | debugOutput={0}", result3);
					TextConsole.Get().Write(new StringBuilder().Append("Discord | debugOutput=").Append(result3).ToString());
					DiscordClientInterface.s_debugOutput = result3;
				}
				goto IL_02b0;
			}
		}
		TextConsole.Get().Write(text);
		return false;
		IL_02b0:
		return true;
	}
}
