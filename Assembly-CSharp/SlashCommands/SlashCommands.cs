using I2.Loc;
using System.Collections.Generic;

public class SlashCommands
{
    private static SlashCommands s_instance;

    public List<SlashCommand> m_slashCommands;

    public SlashCommands()
    {
        m_slashCommands = new List<SlashCommand>
        {
            new SlashCommand_Apropos(),
            new SlashCommand_ChatGame(),
            new SlashCommand_ChatGeneral(),
            new SlashCommand_ChatTeam(),
            new SlashCommand_ShowGeneralChat(),
            new SlashCommand_ShowAllChat(),
            new SlashCommand_EnableProfanityFilter(),
            new SlashCommand_ChatWhisper(),
            new SlashCommand_Friend(),
            new SlashCommand_GroupChat(),
            new SlashCommand_GroupInvite(),
            new SlashCommand_GroupKick(),
            new SlashCommand_GroupLeave(),
            new SlashCommand_GroupPromote(),
            new SlashCommand_InviteToGame(),
            new SlashCommand_SpectateGame(),
            new SlashCommand_UserBlock(),
            new SlashCommand_UserUnblock(),
            new SlashCommand_NameplateOvercon(),
            new SlashCommand_CustomGamePause(),
            new SlashCommand_Help(),
            new SlashCommand_UserReport(),
            new SlashCommand_PlayReplay(),
            new SlashCommand_Replay_FastForward(),
            new SlashCommand_Replay_Restart(),
            new SlashCommand_Replay_Seek(),
            new SlashCommand_Language(),
            new SlashCommand_Version(),
            new SlashCommand_Log(),
            new SlashCommand_SetDevChatTag()
        };
        RebuildLocalizedText();
        LocalizationManager.OnLocalizeEvent += RebuildLocalizedText;
    }

    public static SlashCommands Get()
    {
        return s_instance;
    }

    public static void Instantiate()
    {
        s_instance = new SlashCommands();
    }

    private void RebuildLocalizedText()
    {
        foreach (SlashCommand command in m_slashCommands)
        {
            command.Localize();
        }
    }

    public bool RunSlashCommand(string command, string arguments)
    {
        bool isNotInGame = GameFlowData.Get() == null;
        foreach (SlashCommand slashCommand in m_slashCommands)
        {
            if ((!isNotInGame || slashCommand.AvailableInFrontEnd)
                && (isNotInGame || slashCommand.AvailableInGame)
                && slashCommand.IsSlashCommand(command))
            {
                slashCommand.OnSlashCommand(arguments);
                return true;
            }
        }

        return false;
    }
}