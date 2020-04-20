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
			if (GameManager.Get().GameConfig != null && GameManager.Get().IsAllowingPlayerRequestedPause())
			{
				ActorData actorData = (!(GameFlowData.Get() != null)) ? null : GameFlowData.Get().activeOwnedActorData;
				if (actorData != null)
				{
					if (actorData.GetActorController() != null)
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
