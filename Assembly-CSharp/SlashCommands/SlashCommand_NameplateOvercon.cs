using System;

public class SlashCommand_NameplateOvercon : SlashCommand
{
    public SlashCommand_NameplateOvercon()
        : base("/overcon", SlashCommandType.InGame)
    {
    }

    public override void OnSlashCommand(string arguments)
    {
        ActorData actorData = GameFlowData.Get() != null ? GameFlowData.Get().activeOwnedActorData : null;
        if (actorData == null
            || actorData.GetActorController() == null
            || HUD_UI.Get() == null
            || UIOverconData.Get() == null
            || ClientGameManager.Get() == null)
        {
            return;
        }

        string[] array = arguments.Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
        if (arguments.Length <= 0)
        {
            return;
        }

        string overconName = array[0];
        int overconIdByName = UIOverconData.Get().GetOverconIdByName(overconName);
        if (overconIdByName <= 0)
        {
            TextConsole.Get().Write(string.Format(StringUtil.TR("DidNotFindOvercon", "SlashCommand"), overconName));
            return;
        }

        if (!ClientGameManager.Get().IsOverconUnlocked(overconIdByName))
        {
            TextConsole.Get().Write(
                string.Format(StringUtil.TR("OverconNotUnlocked", "SlashCommand"), overconName));
            return;
        }

        ClientGameManager.Get().SendUseOverconRequest(
            overconIdByName,
            overconName,
            actorData.ActorIndex,
            GameFlowData.Get().CurrentTurn);
    }
}