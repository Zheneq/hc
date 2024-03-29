public class SlashCommand_CustomGamePause : SlashCommand
{
	public SlashCommand_CustomGamePause()
		: base("/customgamepause", SlashCommandType.InGame)
	{
	}

	public override void OnSlashCommand(string arguments)
	{
		if (GameManager.Get() != null)
		{
			if (GameManager.Get().GameConfig != null && GameManager.Get().IsAllowingPlayerRequestedPause())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						ActorData actorData = (!(GameFlowData.Get() != null)) ? null : GameFlowData.Get().activeOwnedActorData;
						if (actorData != null)
						{
							if (actorData.GetActorController() != null)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										break;
									default:
									{
										bool desiredPause = true;
										if (!arguments.IsNullOrEmpty())
										{
											string[] array = arguments.Split(null);
											if (array.Length > 0)
											{
												string lhs = array[0];
												if (lhs.EqualsIgnoreCase("false"))
												{
													desiredPause = false;
												}
											}
										}
										actorData.GetActorController().RequestCustomGamePause(desiredPause, actorData.ActorIndex);
										return;
									}
									}
								}
							}
						}
						TextConsole.Get().Write(StringUtil.TR("PauseError", "Global"));
						return;
					}
					}
				}
			}
		}
		TextConsole.Get().Write(StringUtil.TR("PauseDisabled", "Global"));
	}
}
