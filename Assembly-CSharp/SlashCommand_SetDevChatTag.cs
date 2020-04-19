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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SlashCommand_SetDevChatTag.OnSlashCommand(string)).MethodHandle;
			}
			if (!(ClientGameManager.Get() == null))
			{
				if (arguments.EqualsIgnoreCase(StringUtil.TR("on", "SlashCommand")))
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
					ClientGameManager clientGameManager = ClientGameManager.Get();
					bool active = true;
					if (SlashCommand_SetDevChatTag.<>f__am$cache0 == null)
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
						SlashCommand_SetDevChatTag.<>f__am$cache0 = delegate(SetDevTagResponse response)
						{
							TextConsole.Get().HandleSetDevTagResponse(response);
						};
					}
					clientGameManager.SendSetDevTagRequest(active, SlashCommand_SetDevChatTag.<>f__am$cache0);
				}
				else if (arguments.EqualsIgnoreCase(StringUtil.TR("off", "SlashCommand")))
				{
					ClientGameManager clientGameManager2 = ClientGameManager.Get();
					bool active2 = false;
					if (SlashCommand_SetDevChatTag.<>f__am$cache1 == null)
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
						SlashCommand_SetDevChatTag.<>f__am$cache1 = delegate(SetDevTagResponse response)
						{
							TextConsole.Get().HandleSetDevTagResponse(response);
						};
					}
					clientGameManager2.SendSetDevTagRequest(active2, SlashCommand_SetDevChatTag.<>f__am$cache1);
				}
				return;
			}
		}
	}
}
