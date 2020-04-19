using System;

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
		case CharacterRole.Tank:
			return LocalizationPayload.Create("Frontline", "OverlayScreensScene");
		case CharacterRole.Assassin:
			return LocalizationPayload.Create("Firepower", "OverlayScreensScene");
		case CharacterRole.Support:
			return LocalizationPayload.Create("Support", "OverlayScreensScene");
		default:
			return LocalizationPayload.Create(string.Format("Role{0}", characterRole), "OverlayScreensScene");
		}
	}

	public static bool HasAccepted(this ReadyState readyState)
	{
		return readyState == ReadyState.Accepted || readyState == ReadyState.Ready;
	}

	public static Team OtherTeam(this Team team)
	{
		Team result;
		if (team == Team.TeamA)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Team.OtherTeam()).MethodHandle;
			}
			result = Team.TeamB;
		}
		else
		{
			result = Team.TeamA;
		}
		return result;
	}
}
