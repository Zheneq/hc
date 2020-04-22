using System.Collections.Generic;

public class SlashCommand_Apropos : SlashCommand
{
	public SlashCommand_Apropos()
		: base("/apropos", SlashCommandType.Everywhere)
	{
	}

	private void DumpCommand(string arguments, string command, List<string> aliases, bool bAvailableBecauseWereInFrontEnd, bool bAvailableBecauseWereInGame)
	{
		bool flag = arguments.IsNullOrEmpty() || command.Contains(arguments);
		if (!flag && !aliases.IsNullOrEmpty())
		{
			while (true)
			{
				switch (5)
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
			foreach (string alias in aliases)
			{
				if (alias.Contains(arguments))
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							flag = true;
							goto end_IL_003c;
						}
					}
				}
			}
		}
		if (!flag)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (!bAvailableBecauseWereInFrontEnd)
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
				if (!bAvailableBecauseWereInGame)
				{
					return;
				}
			}
			TextConsole.Message message = default(TextConsole.Message);
			message.MessageType = ConsoleMessageType.SystemMessage;
			message.Text = command;
			if (!aliases.IsNullOrEmpty())
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				using (List<string>.Enumerator enumerator2 = aliases.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						string current2 = enumerator2.Current;
						message.Text = message.Text + ", " + current2;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			TextConsole.Get().Write(message);
			return;
		}
	}

	public override void OnSlashCommand(string arguments)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (!(clientGameManager != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			bool flag = GameFlowData.Get() == null;
			using (List<SlashCommand>.Enumerator enumerator = SlashCommands.Get().m_slashCommands.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SlashCommand current = enumerator.Current;
					if (!current.PublicFacing)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!clientGameManager.HasDeveloperAccess())
						{
							continue;
						}
					}
					bool bAvailableBecauseWereInFrontEnd = flag && current.AvailableInFrontEnd;
					int num;
					if (!flag)
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
						num = (current.AvailableInGame ? 1 : 0);
					}
					else
					{
						num = 0;
					}
					bool bAvailableBecauseWereInGame = (byte)num != 0;
					DumpCommand(arguments, current.Command, current.Aliases, bAvailableBecauseWereInFrontEnd, bAvailableBecauseWereInGame);
				}
				while (true)
				{
					switch (5)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}
}
