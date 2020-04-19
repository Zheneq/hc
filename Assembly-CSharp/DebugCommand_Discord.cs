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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(DebugCommand_Discord.OnSlashCommand(string)).MethodHandle;
			}
			if (!array[0].EqualsIgnoreCase("help"))
			{
				if (array[0].EqualsIgnoreCase("check"))
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					ClientGameManager.Get().CheckDiscord();
				}
				else if (array[0].EqualsIgnoreCase("join"))
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!DiscordClientInterface.CanJoinTeamChat)
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
						if (!DiscordClientInterface.CanJoinGroupChat)
						{
							TextConsole.Get().Write("Failed to join Discord chat. You are not in a team or group.", ConsoleMessageType.SystemMessage);
							goto IL_E4;
						}
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					ClientGameManager.Get().JoinDiscord();
					IL_E4:;
				}
				else if (array[0].EqualsIgnoreCase("leave"))
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
					ClientGameManager.Get().LeaveDiscord();
				}
				else if (array[0].EqualsIgnoreCase("autojoin"))
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
					bool autoJoin = true;
					if (array.Length > 1)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!bool.TryParse(array[1], out autoJoin))
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
							if (array[1].EqualsIgnoreCase("on"))
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
								autoJoin = true;
							}
							else if (array[1].EqualsIgnoreCase("off"))
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
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						DiscordClientInterface.s_RpcPortOverride = s_RpcPortOverride;
					}
				}
				else if (array[0].EqualsIgnoreCase("shutdown"))
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
					DiscordClientInterface.Shutdown();
				}
				else if (array[0].EqualsIgnoreCase("debug"))
				{
					bool flag = true;
					if (array.Length > 1 && !bool.TryParse(array[1], out flag))
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
						if (array[1].EqualsIgnoreCase("on"))
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
							flag = true;
						}
						else if (array[1].EqualsIgnoreCase("off"))
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
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
