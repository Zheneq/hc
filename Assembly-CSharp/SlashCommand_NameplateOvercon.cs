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
			if (UIOverconData.Get() != null)
			{
				if (ClientGameManager.Get() != null)
				{
					string[] array = arguments.Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
					if (arguments.Length > 0)
					{
						string text = array[0];
						int overconIdByName = UIOverconData.Get().GetOverconIdByName(text);
						if (overconIdByName > 0)
						{
							if (ClientGameManager.Get().IsOverconUnlocked(overconIdByName))
							{
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
