using System;

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
							TextConsole.Get().Write("Failed to join Discord chat. You are not in a team or group.", ConsoleMessageType.SystemMessage);
							goto IL_E4;
						}
					}
					ClientGameManager.Get().JoinDiscord();
					IL_E4:;
				}
				else if (array[0].EqualsIgnoreCase("leave"))
				{
					ClientGameManager.Get().LeaveDiscord();
				}
				else if (array[0].EqualsIgnoreCase("autojoin"))
				{
					bool autoJoin = true;
					if (array.Length > 1)
					{
						if (!bool.TryParse(array[1], out autoJoin))
						{
							if (array[1].EqualsIgnoreCase("on"))
							{
								autoJoin = true;
							}
							else if (array[1].EqualsIgnoreCase("off"))
							{
								autoJoin = false;
							}
						}
					}
					ClientGameManager.Get().ConfigureDiscord(autoJoin);
				}
				else if (array[0].EqualsIgnoreCase("port"))
				{
					int s_RpcPortOverride = 0;
					if (int.TryParse(array[1], out s_RpcPortOverride))
					{
						DiscordClientInterface.s_RpcPortOverride = s_RpcPortOverride;
					}
				}
				else if (array[0].EqualsIgnoreCase("shutdown"))
				{
					DiscordClientInterface.Shutdown();
				}
				else if (array[0].EqualsIgnoreCase("debug"))
				{
					bool flag = true;
					if (array.Length > 1 && !bool.TryParse(array[1], out flag))
					{
						if (array[1].EqualsIgnoreCase("on"))
						{
							flag = true;
						}
						else if (array[1].EqualsIgnoreCase("off"))
						{
							flag = false;
						}
					}
					Log.Info("Discord | debugOutput={0}", new object[]
					{
						flag
					});
					TextConsole.Get().Write("Discord | debugOutput=" + flag, ConsoleMessageType.SystemMessage);
					DiscordClientInterface.s_debugOutput = flag;
				}
				return true;
			}
		}
		TextConsole.Get().Write(text, ConsoleMessageType.SystemMessage);
		return false;
	}
}
