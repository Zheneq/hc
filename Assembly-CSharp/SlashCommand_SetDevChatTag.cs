using System;
using LobbyGameClientMessages;

public class SlashCommand_SetDevChatTag : SlashCommand
{
	public SlashCommand_SetDevChatTag() : base("/devtag", SlashCommandType.Everywhere)
	{
		base.PublicFacing = false;
	}

	public override void OnSlashCommand(string arguments)
	{
		if (!arguments.IsNullOrEmpty())
		{
			if (!(ClientGameManager.Get() == null))
			{
				if (arguments.EqualsIgnoreCase(StringUtil.TR("on", "SlashCommand")))
				{
					ClientGameManager clientGameManager = ClientGameManager.Get();
					bool active = true;
					
					clientGameManager.SendSetDevTagRequest(active, delegate(SetDevTagResponse response)
						{
							TextConsole.Get().HandleSetDevTagResponse(response);
						});
				}
				else if (arguments.EqualsIgnoreCase(StringUtil.TR("off", "SlashCommand")))
				{
					ClientGameManager clientGameManager2 = ClientGameManager.Get();
					bool active2 = false;
					
					clientGameManager2.SendSetDevTagRequest(active2, delegate(SetDevTagResponse response)
						{
							TextConsole.Get().HandleSetDevTagResponse(response);
						});
				}
				return;
			}
		}
	}
}
