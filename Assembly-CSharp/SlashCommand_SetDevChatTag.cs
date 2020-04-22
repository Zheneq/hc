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
						
						clientGameManager.SendSetDevTagRequest(true, delegate(SetDevTagResponse response)
							{
								TextConsole.Get().HandleSetDevTagResponse(response);
							});
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
			
			clientGameManager2.SendSetDevTagRequest(false, delegate(SetDevTagResponse response)
				{
					TextConsole.Get().HandleSetDevTagResponse(response);
				});
			return;
		}
	}
}
