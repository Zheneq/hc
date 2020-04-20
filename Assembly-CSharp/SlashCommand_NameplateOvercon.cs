using System;

public class SlashCommand_NameplateOvercon : SlashCommand
{
	public SlashCommand_NameplateOvercon() : base("/overcon", SlashCommandType.InGame)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		ActorData actorData = (!(GameFlowData.Get() != null)) ? null : GameFlowData.Get().activeOwnedActorData;
		if (actorData != null && actorData.GetActorController() != null && HUD_UI.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SlashCommand_NameplateOvercon.OnSlashCommand(string)).MethodHandle;
			}
			if (UIOverconData.Get() != null)
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
				if (ClientGameManager.Get() != null)
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
					string[] array = arguments.Split(null, StringSplitOptions.RemoveEmptyEntries);
					if (arguments.Length > 0)
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
						string text = array[0];
						int overconIdByName = UIOverconData.Get().GetOverconIdByName(text);
						if (overconIdByName > 0)
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
							if (ClientGameManager.Get().IsOverconUnlocked(overconIdByName))
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
								ClientGameManager.Get().SendUseOverconRequest(overconIdByName, text, actorData.ActorIndex, GameFlowData.Get().CurrentTurn);
							}
							else
							{
								TextConsole.Get().Write(string.Format(StringUtil.TR("OverconNotUnlocked", "SlashCommand"), text), ConsoleMessageType.SystemMessage);
							}
						}
						else
						{
							TextConsole.Get().Write(string.Format(StringUtil.TR("DidNotFindOvercon", "SlashCommand"), text), ConsoleMessageType.SystemMessage);
						}
					}
				}
			}
		}
	}
}
