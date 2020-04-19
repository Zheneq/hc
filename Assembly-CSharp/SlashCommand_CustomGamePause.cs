using System;

public class SlashCommand_CustomGamePause : SlashCommand
{
	public SlashCommand_CustomGamePause() : base("/customgamepause", SlashCommandType.InGame)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (GameManager.Get() != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SlashCommand_CustomGamePause.OnSlashCommand(string)).MethodHandle;
			}
			if (GameManager.Get().GameConfig != null && GameManager.Get().IsAllowingPlayerRequestedPause())
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
				ActorData actorData = (!(GameFlowData.Get() != null)) ? null : GameFlowData.Get().activeOwnedActorData;
				if (actorData != null)
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
					if (actorData.\u000E() != null)
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
						bool desiredPause = true;
						if (!arguments.IsNullOrEmpty())
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
							string[] array = arguments.Split(null);
							if (array.Length > 0)
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
								string lhs = array[0];
								if (lhs.EqualsIgnoreCase("false"))
								{
									desiredPause = false;
								}
							}
						}
						actorData.\u000E().RequestCustomGamePause(desiredPause, actorData.ActorIndex);
						goto IL_121;
					}
				}
				TextConsole.Get().Write(StringUtil.TR("PauseError", "Global"), ConsoleMessageType.SystemMessage);
				IL_121:
				return;
			}
		}
		TextConsole.Get().Write(StringUtil.TR("PauseDisabled", "Global"), ConsoleMessageType.SystemMessage);
	}
}
