﻿using System;
using System.Collections.Generic;
using I2.Loc;

public abstract class SlashCommand
{
	private string internalCommand;

	public List<string> Aliases;

	public SlashCommand(string command, SlashCommandType type)
	{
		this.Command = command;
		this.internalCommand = command;
		this.Type = type;
		this.PublicFacing = true;
		this.Aliases = new List<string>();
	}

	public string Command { get; private set; }

	public SlashCommandType Type { get; private set; }

	public string Description { get; private set; }

	public bool PublicFacing { get; set; }

	public bool AvailableInFrontEnd
	{
		get
		{
			return (this.Type & SlashCommandType.InFrontEnd) != (SlashCommandType)0;
		}
	}

	public bool AvailableInGame
	{
		get
		{
			return (this.Type & SlashCommandType.InGame) != (SlashCommandType)0;
		}
	}

	public abstract void OnSlashCommand(string arguments);

	public bool IsSlashCommand(string command)
	{
		if (!command.EqualsIgnoreCase(this.Command))
		{
			if (!command.EqualsIgnoreCase(this.internalCommand))
			{
				if (!this.Aliases.IsNullOrEmpty<string>())
				{
					foreach (string rhs in this.Aliases)
					{
						if (command.EqualsIgnoreCase(rhs))
						{
							return true;
						}
					}
					return false;
				}
				return false;
			}
		}
		return true;
	}

	public void Localize()
	{
		if (!this.PublicFacing)
		{
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().HasDeveloperAccess())
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						goto IL_4E;
					}
				}
			}
			this.Command = this.internalCommand;
			this.Description = string.Empty;
			this.Aliases.Clear();
			return;
		}
		IL_4E:
		this.Command = this.LocalizeSlashCommand(this.internalCommand);
		this.Description = ScriptLocalization.Get(ScriptLocalization.GetSlashCommandDescKey(this.internalCommand));
		this.Aliases.Clear();
		int num = 1;
		int num2 = 0xA;
		bool flag = true;
		while (flag)
		{
			if (num >= num2)
			{
				break;
			}
			string item;
			if (LocalizationManager.TryGetTermTranslation(ScriptLocalization.GetSlashCommandAliasKey(this.internalCommand, num), out item))
			{
				this.Aliases.Add(item);
				num++;
			}
			else
			{
				flag = false;
			}
		}
	}

	private string LocalizeSlashCommand(string command)
	{
		string result;
		if (LocalizationManager.TryGetTermTranslation(ScriptLocalization.GetSlashCommandKey(command), out result))
		{
			return result;
		}
		return command;
	}
}
