using System;
using System.Collections.Generic;
using I2.Loc;

public class SlashCommands
{
	private static SlashCommands s_instance;

	public List<SlashCommand> m_slashCommands = new List<SlashCommand>();

	public SlashCommands()
	{
		this.m_slashCommands.Add(new SlashCommand_Apropos());
		this.m_slashCommands.Add(new SlashCommand_ChatGame());
		this.m_slashCommands.Add(new SlashCommand_ChatGeneral());
		this.m_slashCommands.Add(new SlashCommand_ChatTeam());
		this.m_slashCommands.Add(new SlashCommand_ShowGeneralChat());
		this.m_slashCommands.Add(new SlashCommand_ShowAllChat());
		this.m_slashCommands.Add(new SlashCommand_EnableProfanityFilter());
		this.m_slashCommands.Add(new SlashCommand_ChatWhisper());
		this.m_slashCommands.Add(new SlashCommand_Friend());
		this.m_slashCommands.Add(new SlashCommand_GroupChat());
		this.m_slashCommands.Add(new SlashCommand_GroupInvite());
		this.m_slashCommands.Add(new SlashCommand_GroupKick());
		this.m_slashCommands.Add(new SlashCommand_GroupLeave());
		this.m_slashCommands.Add(new SlashCommand_GroupPromote());
		this.m_slashCommands.Add(new SlashCommand_InviteToGame());
		this.m_slashCommands.Add(new SlashCommand_SpectateGame());
		this.m_slashCommands.Add(new SlashCommand_UserBlock());
		this.m_slashCommands.Add(new SlashCommand_UserUnblock());
		this.m_slashCommands.Add(new SlashCommand_NameplateOvercon());
		this.m_slashCommands.Add(new SlashCommand_CustomGamePause());
		this.m_slashCommands.Add(new SlashCommand_Help());
		this.m_slashCommands.Add(new SlashCommand_UserReport());
		this.m_slashCommands.Add(new SlashCommand_PlayReplay());
		this.m_slashCommands.Add(new SlashCommand_Replay_FastForward());
		this.m_slashCommands.Add(new SlashCommand_Replay_Restart());
		this.m_slashCommands.Add(new SlashCommand_Replay_Seek());
		this.m_slashCommands.Add(new SlashCommand_Language());
		this.m_slashCommands.Add(new SlashCommand_Version());
		this.m_slashCommands.Add(new SlashCommand_Log());
		this.m_slashCommands.Add(new SlashCommand_SetDevChatTag());
		this.RebuildLocalizedText();
		LocalizationManager.OnLocalizeEvent += this.RebuildLocalizedText;
	}

	public static SlashCommands Get()
	{
		return SlashCommands.s_instance;
	}

	public static void Instantiate()
	{
		SlashCommands.s_instance = new SlashCommands();
	}

	private void RebuildLocalizedText()
	{
		using (List<SlashCommand>.Enumerator enumerator = this.m_slashCommands.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SlashCommand slashCommand = enumerator.Current;
				slashCommand.Localize();
			}
		}
	}

	public bool RunSlashCommand(string command, string arguments)
	{
		bool flag = GameFlowData.Get() == null;
		bool result = false;
		foreach (SlashCommand slashCommand in this.m_slashCommands)
		{
			if (flag)
			{
				if (!slashCommand.AvailableInFrontEnd)
				{
					continue;
				}
			}
			if (!flag)
			{
				if (!slashCommand.AvailableInGame)
				{
					continue;
				}
			}
			if (slashCommand.IsSlashCommand(command))
			{
				slashCommand.OnSlashCommand(arguments);
				result = true;
				break;
			}
		}
		return result;
	}
}
