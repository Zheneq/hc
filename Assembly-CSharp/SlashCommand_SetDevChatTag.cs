using LobbyGameClientMessages;

public class SlashCommand_SetDevChatTag : SlashCommand
{
	public SlashCommand_SetDevChatTag()
		: base("/devtag", SlashCommandType.Everywhere)
	{
		base.PublicFacing = false;
	}

	public override void OnSlashCommand(string arguments)
	{
		if (arguments.IsNullOrEmpty())
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (ClientGameManager.Get() == null)
			{
				return;
			}
			if (arguments.EqualsIgnoreCase(StringUtil.TR("on", "SlashCommand")))
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
					{
						ClientGameManager clientGameManager = ClientGameManager.Get();
						if (_003C_003Ef__am_0024cache0 == null)
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
							_003C_003Ef__am_0024cache0 = delegate(SetDevTagResponse response)
							{
								TextConsole.Get().HandleSetDevTagResponse(response);
							};
						}
						clientGameManager.SendSetDevTagRequest(true, _003C_003Ef__am_0024cache0);
						return;
					}
					}
				}
			}
			if (!arguments.EqualsIgnoreCase(StringUtil.TR("off", "SlashCommand")))
			{
				return;
			}
			ClientGameManager clientGameManager2 = ClientGameManager.Get();
			if (_003C_003Ef__am_0024cache1 == null)
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
				_003C_003Ef__am_0024cache1 = delegate(SetDevTagResponse response)
				{
					TextConsole.Get().HandleSetDevTagResponse(response);
				};
			}
			clientGameManager2.SendSetDevTagRequest(false, _003C_003Ef__am_0024cache1);
			return;
		}
	}
}
