using I2.Loc;
using System.Collections.Generic;

public class SlashCommands
{
	private static SlashCommands s_instance;

	public List<SlashCommand> m_slashCommands = new List<SlashCommand>();

	public SlashCommands()
	{
		m_slashCommands.Add(new SlashCommand_Apropos());
		m_slashCommands.Add(new SlashCommand_ChatGame());
		m_slashCommands.Add(new SlashCommand_ChatGeneral());
		m_slashCommands.Add(new SlashCommand_ChatTeam());
		m_slashCommands.Add(new SlashCommand_ShowGeneralChat());
		m_slashCommands.Add(new SlashCommand_ShowAllChat());
		m_slashCommands.Add(new SlashCommand_EnableProfanityFilter());
		m_slashCommands.Add(new SlashCommand_ChatWhisper());
		m_slashCommands.Add(new SlashCommand_Friend());
		m_slashCommands.Add(new SlashCommand_GroupChat());
		m_slashCommands.Add(new SlashCommand_GroupInvite());
		m_slashCommands.Add(new SlashCommand_GroupKick());
		m_slashCommands.Add(new SlashCommand_GroupLeave());
		m_slashCommands.Add(new SlashCommand_GroupPromote());
		m_slashCommands.Add(new SlashCommand_InviteToGame());
		m_slashCommands.Add(new SlashCommand_SpectateGame());
		m_slashCommands.Add(new SlashCommand_UserBlock());
		m_slashCommands.Add(new SlashCommand_UserUnblock());
		m_slashCommands.Add(new SlashCommand_NameplateOvercon());
		m_slashCommands.Add(new SlashCommand_CustomGamePause());
		m_slashCommands.Add(new SlashCommand_Help());
		m_slashCommands.Add(new SlashCommand_UserReport());
		m_slashCommands.Add(new SlashCommand_PlayReplay());
		m_slashCommands.Add(new SlashCommand_Replay_FastForward());
		m_slashCommands.Add(new SlashCommand_Replay_Restart());
		m_slashCommands.Add(new SlashCommand_Replay_Seek());
		m_slashCommands.Add(new SlashCommand_Language());
		m_slashCommands.Add(new SlashCommand_Version());
		m_slashCommands.Add(new SlashCommand_Log());
		m_slashCommands.Add(new SlashCommand_SetDevChatTag());
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
		using (List<SlashCommand>.Enumerator enumerator = m_slashCommands.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SlashCommand current = enumerator.Current;
				current.Localize();
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
	}

	public bool RunSlashCommand(string command, string arguments)
	{
		bool flag = GameFlowData.Get() == null;
		bool result = false;
		foreach (SlashCommand slashCommand in m_slashCommands)
		{
			if (flag)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!slashCommand.AvailableInFrontEnd)
				{
					continue;
				}
			}
			if (!flag)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!slashCommand.AvailableInGame)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					continue;
				}
			}
			if (slashCommand.IsSlashCommand(command))
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						slashCommand.OnSlashCommand(arguments);
						return true;
					}
				}
			}
		}
		return result;
	}
}
