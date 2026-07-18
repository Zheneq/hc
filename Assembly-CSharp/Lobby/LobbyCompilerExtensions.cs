public static class LobbyCompilerExtensions
{
	public static bool IsValid(this CharacterType characterType)
	{
		return characterType > CharacterType.None && characterType < CharacterType.Last;
	}

	public static LocalizationPayload GetLocalizedPayload(this CharacterRole characterRole)
	{
		switch (characterRole)
		{
		case CharacterRole.Assassin:
			return LocalizationPayload.Create("Firepower", "OverlayScreensScene");
		case CharacterRole.Tank:
			return LocalizationPayload.Create("Frontline", "OverlayScreensScene");
		case CharacterRole.Support:
			return LocalizationPayload.Create("Support", "OverlayScreensScene");
		default:
			return LocalizationPayload.Create($"Role{characterRole}", "OverlayScreensScene");
		}
	}

	public static bool HasAccepted(this ReadyState readyState)
	{
		return readyState == ReadyState.Accepted || readyState == ReadyState.Ready;
	}

	public static Team OtherTeam(this Team team)
	{
		int result;
		if (team == Team.TeamA)
		{
			result = 1;
		}
		else
		{
			result = 0;
		}
		return (Team)result;
	}
}
