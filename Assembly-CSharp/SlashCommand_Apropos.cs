using System;
using System.Collections.Generic;
using System.Text;

public class SlashCommand_Apropos : SlashCommand
{
	public SlashCommand_Apropos() : base("/apropos", SlashCommandType.Everywhere)
	{
	}

	private void DumpCommand(string arguments, string command, List<string> aliases, bool bAvailableBecauseWereInFrontEnd, bool bAvailableBecauseWereInGame)
	{
		bool flag = arguments.IsNullOrEmpty() || command.Contains(arguments);
		if (!flag && !aliases.IsNullOrEmpty<string>())
		{
			foreach (string text in aliases)
			{
				if (text.Contains(arguments))
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
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
			if (!aliases.IsNullOrEmpty<string>())
			{
				using (List<string>.Enumerator enumerator2 = aliases.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						string str = enumerator2.Current;
						message.Text = new StringBuilder().Append(message.Text).Append(", ").Append(str).ToString();
					}
				}
			}
			TextConsole.Get().Write(message, null);
		}
	}

	public override void OnSlashCommand(string arguments)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager != null)
		{
			bool flag = GameFlowData.Get() == null;
			using (List<SlashCommand>.Enumerator enumerator = SlashCommands.Get().m_slashCommands.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SlashCommand slashCommand = enumerator.Current;
					if (!slashCommand.PublicFacing)
					{
						if (!clientGameManager.HasDeveloperAccess())
						{
							continue;
						}
					}
					bool bAvailableBecauseWereInFrontEnd = flag && slashCommand.AvailableInFrontEnd;
					bool flag2;
					if (!flag)
					{
						flag2 = slashCommand.AvailableInGame;
					}
					else
					{
						flag2 = false;
					}
					bool bAvailableBecauseWereInGame = flag2;
					this.DumpCommand(arguments, slashCommand.Command, slashCommand.Aliases, bAvailableBecauseWereInFrontEnd, bAvailableBecauseWereInGame);
				}
			}
		}
	}
}
