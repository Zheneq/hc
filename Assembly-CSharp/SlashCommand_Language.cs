using System.Text;
using I2.Loc;

public class SlashCommand_Language : SlashCommand
{
	public SlashCommand_Language()
		: base("/language", SlashCommandType.Everywhere)
	{
		base.PublicFacing = false;
	}

	public override void OnSlashCommand(string arguments)
	{
		if (!arguments.IsNullOrEmpty())
		{
			if (LocalizationManager.HasLanguage(arguments))
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						LocalizationManager.CurrentLanguage = arguments;
						string text = new StringBuilder().Append("Language changed to: ").Append(LocalizationManager.CurrentLanguage).ToString();
						TextConsole.Get().Write(text);
						ClientGameManager.Get().SetNewSessionLanguage(LocalizationManager.CurrentLanguageCode);
						return;
					}
					}
				}
			}
			TextConsole.Get().Write(new StringBuilder().Append("Unrecognized Language: ").Append(arguments).ToString());
		}
		else
		{
			string text2 = new StringBuilder().Append("Current Language: ").Append(LocalizationManager.CurrentLanguage).ToString();
			TextConsole.Get().Write(text2);
		}
	}
}
