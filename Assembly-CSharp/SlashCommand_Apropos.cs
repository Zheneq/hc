using System;
using System.Collections.Generic;

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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SlashCommand_Apropos.DumpCommand(string, string, List<string>, bool, bool)).MethodHandle;
			}
			foreach (string text in aliases)
			{
				if (text.Contains(arguments))
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!bAvailableBecauseWereInFrontEnd)
			{
				for (;;)
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
			if (!aliases.IsNullOrEmpty<string>())
			{
				for (;;)
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
						string str = enumerator2.Current;
						message.Text = message.Text + ", " + str;
					}
					for (;;)
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
			TextConsole.Get().Write(message, null);
		}
	}

	public override void OnSlashCommand(string arguments)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SlashCommand_Apropos.OnSlashCommand(string)).MethodHandle;
			}
			bool flag = GameFlowData.Get() == null;
			using (List<SlashCommand>.Enumerator enumerator = SlashCommands.Get().m_slashCommands.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SlashCommand slashCommand = enumerator.Current;
					if (!slashCommand.PublicFacing)
					{
						for (;;)
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
					bool bAvailableBecauseWereInFrontEnd = flag && slashCommand.AvailableInFrontEnd;
					bool flag2;
					if (!flag)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						flag2 = slashCommand.AvailableInGame;
					}
					else
					{
						flag2 = false;
					}
					bool bAvailableBecauseWereInGame = flag2;
					this.DumpCommand(arguments, slashCommand.Command, slashCommand.Aliases, bAvailableBecauseWereInFrontEnd, bAvailableBecauseWereInGame);
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
	}
}
