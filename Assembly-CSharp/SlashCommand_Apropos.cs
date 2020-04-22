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
			foreach (string alias in aliases)
			{
				if (alias.Contains(arguments))
				{
					flag = true;
				}
			}
		}
		if (!flag)
		{
			return;
		}
		while (true)
		{
			if (!bAvailableBecauseWereInFrontEnd)
			{
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
				using (List<string>.Enumerator enumerator2 = aliases.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						string current2 = enumerator2.Current;
						message.Text = message.Text + ", " + current2;
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
			bool flag = GameFlowData.Get() == null;
			using (List<SlashCommand>.Enumerator enumerator = SlashCommands.Get().m_slashCommands.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SlashCommand current = enumerator.Current;
					if (!current.PublicFacing)
					{
						if (!clientGameManager.HasDeveloperAccess())
						{
							continue;
						}
					}
					bool bAvailableBecauseWereInFrontEnd = flag && current.AvailableInFrontEnd;
					int num;
					if (!flag)
					{
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
