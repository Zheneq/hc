using I2.Loc;
using System.Collections.Generic;

public abstract class SlashCommand
{
	private string internalCommand;

	public List<string> Aliases;

	public string Command
	{
		get;
		private set;
	}

	public SlashCommandType Type
	{
		get;
		private set;
	}

	public string Description
	{
		get;
		private set;
	}

	public bool PublicFacing
	{
		get;
		set;
	}

	public bool AvailableInFrontEnd
	{
		get { return (Type & SlashCommandType.InFrontEnd) != 0; }
	}

	public bool AvailableInGame
	{
		get { return (Type & SlashCommandType.InGame) != 0; }
	}

	public SlashCommand(string command, SlashCommandType type)
	{
		internalCommand = (Command = command);
		Type = type;
		PublicFacing = true;
		Aliases = new List<string>();
	}

	public abstract void OnSlashCommand(string arguments);

	public bool IsSlashCommand(string command)
	{
		if (!command.EqualsIgnoreCase(Command))
		{
			if (!command.EqualsIgnoreCase(internalCommand))
			{
				if (!Aliases.IsNullOrEmpty())
				{
					foreach (string alias in Aliases)
					{
						if (command.EqualsIgnoreCase(alias))
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									return true;
								}
							}
						}
					}
				}
				return false;
			}
		}
		return true;
	}

	public void Localize()
	{
		if (!PublicFacing)
		{
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().HasDeveloperAccess())
				{
					goto IL_004e;
				}
			}
			Command = internalCommand;
			Description = string.Empty;
			Aliases.Clear();
			return;
		}
		goto IL_004e;
		IL_004e:
		Command = LocalizeSlashCommand(internalCommand);
		Description = ScriptLocalization.Get(ScriptLocalization.GetSlashCommandDescKey(internalCommand));
		Aliases.Clear();
		int num = 1;
		int num2 = 10;
		bool flag = true;
		while (flag)
		{
			if (num < num2)
			{
				string Translation;
				if (LocalizationManager.TryGetTermTranslation(ScriptLocalization.GetSlashCommandAliasKey(internalCommand, num), out Translation))
				{
					Aliases.Add(Translation);
					num++;
				}
				else
				{
					flag = false;
				}
				continue;
			}
			break;
		}
	}

	private string LocalizeSlashCommand(string command)
	{
		string Translation;
		if (LocalizationManager.TryGetTermTranslation(ScriptLocalization.GetSlashCommandKey(command), out Translation))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return Translation;
				}
			}
		}
		return command;
	}
}
