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
			while (true)
			{
				switch (4)
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
			if (!array[0].EqualsIgnoreCase("help"))
			{
				if (array[0].EqualsIgnoreCase("check"))
				{
					while (true)
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
					while (true)
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
						while (true)
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
							TextConsole.Get().Write("Failed to join Discord chat. You are not in a team or group.");
							goto IL_02b0;
						}
						while (true)
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
				}
				else if (array[0].EqualsIgnoreCase("leave"))
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
					ClientGameManager.Get().LeaveDiscord();
				}
				else if (array[0].EqualsIgnoreCase("autojoin"))
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
					bool result = true;
					if (array.Length > 1)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!bool.TryParse(array[1], out result))
						{
							while (true)
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
								while (true)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								result = true;
							}
							else if (array[1].EqualsIgnoreCase("off"))
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
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						DiscordClientInterface.s_RpcPortOverride = result2;
					}
				}
				else if (array[0].EqualsIgnoreCase("shutdown"))
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
					DiscordClientInterface.Shutdown();
				}
				else if (array[0].EqualsIgnoreCase("debug"))
				{
					bool result3 = true;
					if (array.Length > 1 && !bool.TryParse(array[1], out result3))
					{
						while (true)
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
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							result3 = true;
						}
						else if (array[1].EqualsIgnoreCase("off"))
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							result3 = false;
						}
					}
					Log.Info("Discord | debugOutput={0}", result3);
					TextConsole.Get().Write("Discord | debugOutput=" + result3);
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
