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
						string text = $"Language changed to: {LocalizationManager.CurrentLanguage}";
						TextConsole.Get().Write(text);
						ClientGameManager.Get().SetNewSessionLanguage(LocalizationManager.CurrentLanguageCode);
						return;
					}
					}
				}
			}
			TextConsole.Get().Write($"Unrecognized Language: {arguments}");
		}
		else
		{
			string text2 = $"Current Language: {LocalizationManager.CurrentLanguage}";
			TextConsole.Get().Write(text2);
		}
	}
}
