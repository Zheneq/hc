public class SlashCommand_CustomGamePause : SlashCommand
{
    public SlashCommand_CustomGamePause()
        : base("/customgamepause", SlashCommandType.InGame)
    {
    }

    public override void OnSlashCommand(string arguments)
    {
        if (GameManager.Get() == null
            || GameManager.Get().GameConfig == null
            || !GameManager.Get().IsAllowingPlayerRequestedPause())
        {
            TextConsole.Get().Write(StringUtil.TR("PauseDisabled", "Global"));
            return;
        }

        ActorData actorData = GameFlowData.Get() != null ? GameFlowData.Get().activeOwnedActorData : null;
        if (actorData == null || actorData.GetActorController() == null)
        {
            TextConsole.Get().Write(StringUtil.TR("PauseError", "Global"));
            return;
        }

        bool desiredPause = true;
        if (!arguments.IsNullOrEmpty())
        {
            string[] array = arguments.Split(null);
            if (array.Length > 0 && array[0].EqualsIgnoreCase("false"))
            {
                desiredPause = false;
            }
        }

        actorData.GetActorController().RequestCustomGamePause(desiredPause, actorData.ActorIndex);
    }
}